using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Match;
using System.Diagnostics;
using UnityEngine;

namespace BattleRaja.Presentation.AI
{
    [DefaultExecutionOrder(-50)]
    public sealed class BotBrain : MonoBehaviour
    {
        [SerializeField] private int seed = 10;
    [SerializeField] private int reactionDelayTicks = 15;
    [Range(0f, 1f)] [SerializeField] private float aimNoise = 0.12f;
    [Range(0f, 1f)] [SerializeField] private float retreatHealthFraction = 0.22f;
    [Min(0.1f)] [SerializeField] private float preferredRange = 6.5f;
    [Min(0.02f)] [SerializeField] private float decisionIntervalSeconds = 0.20f;
        [Min(0.1f)] [SerializeField] private float stuckTimeoutSeconds = 0.7f;
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private CombatAttackController attackController;
        [SerializeField] private MonoBehaviour fighterController;
        [SerializeField] private BotPerceptionSensor perception;
        [SerializeField] private GadgetUser gadgetUser;
        [SerializeField] private OfflineMatchController matchController;
        [SerializeField] private ProjectileWeaponAsset weaponAsset;

        private BotDifficultyProfile _profile;
        private BotDecisionEngine _engine;
        private BotNavigationRecovery _navigation;
        private SeededRandom _random;
        private BotDecision _decision;
        private IFighterAbilityController _abilityController;
        private FixedSimulationClock _clock;
        private int _nextDecisionTick;
        private int _decisionIntervalTicks;
        private int _simulationTick;
        private int _attackInputSequence;
        private bool _abilityIssued;
        private bool _subscribedToCanonicalTick;
        private readonly Stopwatch _decisionTimer = new Stopwatch();

        public BotDecision CurrentDecision => _decision;
        public IFighterAbilityController AbilityController => _abilityController;
        public int DecisionCount { get; private set; }
        public double MaxDecisionMilliseconds { get; private set; }
        public string DebugSummary => $"{_decision.State} target={_decision.TargetId.Value} utility={_decision.UtilityScore:0.0} threats={_decision.PerceivedThreats} stuck={_decision.StuckRecovery}";

        public void SetMatchController(OfflineMatchController controller)
        {
            if (_subscribedToCanonicalTick && matchController != null)
            {
                matchController.SimulationTickAdvanced -= OnCanonicalSimulationTick;
                _subscribedToCanonicalTick = false;
            }
            matchController = controller;
            SubscribeToCanonicalTick();
        }

        public bool CombatEnabled => matchController == null ||
            (matchController.CurrentPhase >= MatchPhase.Opening && matchController.CurrentPhase < MatchPhase.Resolution);

        private void Awake()
        {
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            attackController = attackController != null ? attackController : GetComponent<CombatAttackController>();
            // Do not default to Bijli: production bots may carry a fighter-specific
            // controller, and a missing serialized reference must not silently replace
            // Pehel or Maya with the dash bridge.
            _abilityController = fighterController as IFighterAbilityController;
            if (_abilityController == null) _abilityController = GetComponent<IFighterAbilityController>();
            perception = perception != null ? perception : GetComponent<BotPerceptionSensor>();
            gadgetUser = gadgetUser != null ? gadgetUser : GetComponent<GadgetUser>();
            _profile = new BotDifficultyProfile(
                reactionDelayTicks,
                aimNoise,
                retreatHealthFraction,
                preferredRange,
                decisionIntervalSeconds,
                stuckTimeoutSeconds,
                weaponAsset != null ? weaponAsset.ToDomain() : ProjectileWeaponDefinition.TrainingBolt);
            _clock = new FixedSimulationClock(30);
            _decisionIntervalTicks = Mathf.Max(1, Mathf.CeilToInt(_profile.DecisionIntervalSeconds * _clock.TickRate));
            _engine = new BotDecisionEngine();
            _navigation = new BotNavigationRecovery();
            _random = new SeededRandom((uint)seed);
            _decision = new BotDecision(BotDecisionState.Explore, default, Float2.Zero, Float2.Up, false, false, 0f, 0, false);
            _attackInputSequence = 0;
            if (movementAgent != null)
            {
                movementAgent.ExternalCommandMode = true;
            }
        }

        private void Start()
        {
            SubscribeToCanonicalTick();
        }

        private void OnDestroy()
        {
            if (_subscribedToCanonicalTick && matchController != null)
            {
                matchController.SimulationTickAdvanced -= OnCanonicalSimulationTick;
                _subscribedToCanonicalTick = false;
            }
        }

        private void SubscribeToCanonicalTick()
        {
            if (_subscribedToCanonicalTick || !isActiveAndEnabled || matchController == null) return;
            matchController.SimulationTickAdvanced += OnCanonicalSimulationTick;
            _subscribedToCanonicalTick = true;
        }

        private void Update()
        {
            if (movementAgent == null || perception == null || _engine == null)
            {
                return;
            }

            if (matchController != null && matchController.IsMatchStarted)
            {
                return;
            }

            var steps = _clock.Consume(Time.deltaTime);
            for (var step = 0; step < steps; step++) SimulateTick(_clock.GetConsumedTick(step), (float)_clock.StepSeconds);
        }

        private void OnCanonicalSimulationTick(int simulationTick, float fixedDeltaSeconds)
        {
            if (!isActiveAndEnabled || matchController == null || !matchController.IsMatchStarted) return;
            SimulateTick(simulationTick, fixedDeltaSeconds);
        }

        private void SimulateTick(int simulationTick, float fixedDeltaSeconds)
        {
            _simulationTick = simulationTick;
            var stuck = _navigation.Observe(
                new Float2(transform.position.x, transform.position.z),
                _decision.Movement,
                fixedDeltaSeconds,
                _profile.StuckTimeoutSeconds);
            if (_simulationTick >= _nextDecisionTick)
            {
                _decisionTimer.Restart();
                var snapshot = perception.Capture();
                _decision = _engine.Decide(snapshot, _simulationTick, _profile, _random, stuck);
                _decisionTimer.Stop();
                DecisionCount++;
                MaxDecisionMilliseconds = Mathf.Max((float)MaxDecisionMilliseconds, (float)_decisionTimer.Elapsed.TotalMilliseconds);
                _nextDecisionTick = _simulationTick + _decisionIntervalTicks;
                if (!_decision.Ability) _abilityIssued = false;
                if (CombatEnabled) gadgetUser?.UseForContext(snapshot);
            }

            var input = new MovementInputFrame(_decision.Movement, _decision.Aim);
            movementAgent.Submit(MovementCommandFactory.Create(movementAgent.ActorId, _simulationTick, input, movementAgent.Tuning), fixedDeltaSeconds);

            if (CombatEnabled && attackController != null && _decision.Attack)
            {
                var origin = new Float2(transform.position.x, transform.position.z) + _decision.Aim * 0.7f;
                attackController.Submit(AttackCommandFactory.Create(
                    new CombatEntityId(movementAgent.ActorId),
                    _simulationTick,
                    origin,
                    _decision.Aim,
                    true,
                    _attackInputSequence++));
            }

            if (CombatEnabled && _abilityController != null && _decision.Ability && !_abilityIssued)
            {
                _abilityController.Submit(AbilityCommandFactory.Create(
                    new CombatEntityId(movementAgent.ActorId),
                    _simulationTick,
                    _abilityController.AbilityId,
                    _decision.Aim,
                    true));
                _abilityIssued = true;
            }

            if (!_navigation.IsStuck && _decision.StuckRecovery) _engine.Reset();
        }
    }
}
