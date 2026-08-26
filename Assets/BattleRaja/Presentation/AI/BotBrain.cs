using System.Linq;
using BattleRaja.Core.Application;
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
        private const int RecoveryDirectionRefreshTicks = 10;
        // The production-loop digest is a replay diagnostic, not a binary float dump.
        // Quantize continuous movement/aim commands to centimetre-scale inputs so
        // harmless presentation transform noise cannot masquerade as a gameplay
        // divergence across fresh Unity processes.
        private const float CommandDigestQuantization = 100f;
        [SerializeField] private int seed = 10;
        [SerializeField] private int reactionDelayTicks = 15;
        [Range(0f, 1f)] [SerializeField] private float aimNoise = 0.05f;
        [Range(0f, 1f)] [SerializeField] private float retreatHealthFraction = 0.22f;
        [Min(0.1f)] [SerializeField] private float preferredRange = 6.5f;
        [Min(0.02f)] [SerializeField] private float decisionIntervalSeconds = 0.20f;
        [Min(0.1f)] [SerializeField] private float stuckTimeoutSeconds = 0.7f;
        [Min(1f)] [SerializeField] private float attackCadenceMultiplier = 1.25f;
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private CombatAttackController attackController;
        [SerializeField] private MonoBehaviour fighterController;
        [SerializeField] private BotPerceptionSensor perception;
        [SerializeField] private CombatHealth health;
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
        private int _nextAttackTick;
        private bool _abilityIssued;
        private int _lastTargetId;
        private int _recoveryDirectionIndex;
        private float _avoidanceSide = 1f;
        private bool _lastStuckRecovery;
        private Float2 _lastSubmittedMovement;
        private Float2 _recoveryMovement;
        private bool _hasRecoveryMovement;
        private int _recoveryTicks;
        private int _continuousStuckTicks;
        private ProjectileWeaponDefinition _autonomousWeaponDefinition;
        private bool _hasAutonomousWeaponDefinition;
        private bool _subscribedToCanonicalTick;
        private bool _subscribedToAuthorityTick;
        private readonly Stopwatch _decisionTimer = new Stopwatch();

        public BotDecision CurrentDecision => _decision;
        public IFighterAbilityController AbilityController => _abilityController;
        public int DecisionCount { get; private set; }
        public int TargetDecisionCount { get; private set; }
        public int EngageDecisionCount { get; private set; }
        public int AttackDecisionCount { get; private set; }
        public double MaxDecisionMilliseconds { get; private set; }
        public int AttackAttemptCount { get; private set; }
        public int OutOfRangeAttackAttemptCount { get; private set; }
        public int AbilityAttemptCount { get; private set; }
        public int TargetSwitchCount { get; private set; }
        public int StuckRecoveryCount { get; private set; }
        public int ZoneSafetyDecisionCount { get; private set; }
        public int MaxContinuousStuckTicks { get; private set; }
        public Float2 MaxStuckPosition { get; private set; }
        public int MaxStuckSimulationTick { get; private set; }
        public ulong CommandDigest { get; private set; }
        public int CommandCount { get; private set; }
        public ProjectileWeaponAsset AutonomousWeaponAsset => weaponAsset;
        public string DebugSummary => $"{_decision.State} target={_decision.TargetId.Value} utility={_decision.UtilityScore:0.0} threats={_decision.PerceivedThreats} stuck={_decision.StuckRecovery}";

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>
        /// Applies the reviewed production cadence to an actor that the diagnostic
        /// harness converts from the human slot into a bot. The normal player build
        /// never calls this editor/development-only hook.
        /// </summary>
        public void SetHarnessAttackCadenceMultiplier(float multiplier)
        {
            attackCadenceMultiplier = Mathf.Max(1f, multiplier);
        }
#endif

        /// <summary>
        /// Rebuilds deterministic bot state for a batch match. This is used only by the
        /// autonomous production-pipeline harness after a fresh scene has activated.
        /// </summary>
        public void ConfigureForAutonomousMatch(uint matchSeed, ProjectileWeaponAsset autonomousWeapon = null)
        {
            if (autonomousWeapon != null)
            {
                weaponAsset = autonomousWeapon;
                _hasAutonomousWeaponDefinition = false;
            }
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            attackController = attackController != null ? attackController : GetComponent<CombatAttackController>();
            perception = perception != null ? perception : GetComponent<BotPerceptionSensor>();
            health = health != null ? health : GetComponent<CombatHealth>();
            gadgetUser = gadgetUser != null ? gadgetUser : GetComponent<GadgetUser>();
            matchController = matchController != null ? matchController : FindAnyObjectByType<OfflineMatchController>();
            SelectActiveAbilityController();
            var autonomousWeaponDefinition = _hasAutonomousWeaponDefinition
                ? _autonomousWeaponDefinition
                : weaponAsset != null ? weaponAsset.ToDomain() : ProjectileWeaponDefinition.TrainingBolt;
            perception?.ConfigureAutonomousWeapon(autonomousWeaponDefinition);
            _profile = new BotDifficultyProfile(
                reactionDelayTicks,
                aimNoise,
                retreatHealthFraction,
                preferredRange,
                decisionIntervalSeconds,
                stuckTimeoutSeconds,
                autonomousWeaponDefinition);
            _clock = new FixedSimulationClock(30);
            _decisionIntervalTicks = Mathf.Max(1, Mathf.CeilToInt(_profile.DecisionIntervalSeconds * _clock.TickRate));
            var actorSeed = matchSeed * 397u + (uint)Mathf.Max(0, movementAgent != null ? movementAgent.ActorId : 0);
            seed = Mathf.Clamp((int)(actorSeed & 0x7fffffff), 1, int.MaxValue);
            _random = new SeededRandom(actorSeed == 0 ? 1u : actorSeed);
            _engine.Reset();
            _navigation.Clear();
            _decision = new BotDecision(
                BotDecisionState.Explore,
                default(CombatEntityId),
                Float2.Zero,
                Float2.Up,
                false,
                false,
                0f,
                0,
                false);
            _nextDecisionTick = 0;
            _simulationTick = 0;
            _attackInputSequence = 0;
            _nextAttackTick = 0;
            _abilityIssued = false;
            _lastTargetId = 0;
            _recoveryDirectionIndex = movementAgent != null ? Mathf.Abs(movementAgent.ActorId) & 7 : 0;
            _avoidanceSide = (movementAgent != null && movementAgent.ActorId % 2 == 0) ? -1f : 1f;
            _lastStuckRecovery = false;
            _lastSubmittedMovement = Float2.Zero;
            _recoveryMovement = Float2.Zero;
            _hasRecoveryMovement = false;
            _recoveryTicks = 0;
            _continuousStuckTicks = 0;
            attackController?.ResetAttackState();
            movementAgent?.ResetAuthorityCommandTelemetry();
            perception?.ResetTelemetry();
            gadgetUser?.ResetTelemetry();
            ResetAbilityTelemetry(_abilityController as MonoBehaviour);
            SetMatchController(matchController);
            DecisionCount = 0;
            TargetDecisionCount = 0;
            EngageDecisionCount = 0;
            AttackDecisionCount = 0;
            MaxDecisionMilliseconds = 0d;
            AttackAttemptCount = 0;
            OutOfRangeAttackAttemptCount = 0;
            AbilityAttemptCount = 0;
            TargetSwitchCount = 0;
            StuckRecoveryCount = 0;
            ZoneSafetyDecisionCount = 0;
            MaxContinuousStuckTicks = 0;
            MaxStuckPosition = Float2.Zero;
            MaxStuckSimulationTick = 0;
            CommandDigest = 14695981039346656037UL ^ (uint)Mathf.Max(0, movementAgent != null ? movementAgent.ActorId : 0);
            CommandCount = 0;
        }

        public void ConfigureForAutonomousMatch(uint matchSeed, ProjectileWeaponDefinition autonomousWeapon)
        {
            _autonomousWeaponDefinition = autonomousWeapon;
            _hasAutonomousWeaponDefinition = true;
            ConfigureForAutonomousMatch(matchSeed, (ProjectileWeaponAsset)null);
        }

        public void SetMatchController(OfflineMatchController controller)
        {
            if (_subscribedToCanonicalTick && matchController != null)
            {
                matchController.SimulationTickAdvanced -= OnCanonicalSimulationTick;
                _subscribedToCanonicalTick = false;
            }

            if (_subscribedToAuthorityTick && matchController != null)
            {
                matchController.AuthorityTickResolved -= OnAuthorityTickResolved;
                _subscribedToAuthorityTick = false;
            }

            matchController = controller;
            SubscribeToCanonicalTick();
            SubscribeToAuthorityTick();
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
            health = health != null ? health : GetComponent<CombatHealth>();
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
            SubscribeToAuthorityTick();
        }

        private void OnDestroy()
        {
            if (_subscribedToCanonicalTick && matchController != null)
            {
                matchController.SimulationTickAdvanced -= OnCanonicalSimulationTick;
                _subscribedToCanonicalTick = false;
            }

            if (_subscribedToAuthorityTick && matchController != null)
            {
                matchController.AuthorityTickResolved -= OnAuthorityTickResolved;
                _subscribedToAuthorityTick = false;
            }
        }

        private void SubscribeToCanonicalTick()
        {
            if (_subscribedToCanonicalTick || !isActiveAndEnabled || matchController == null) return;
            matchController.SimulationTickAdvanced += OnCanonicalSimulationTick;
            _subscribedToCanonicalTick = true;
        }

        private void SubscribeToAuthorityTick()
        {
            if (_subscribedToAuthorityTick || !isActiveAndEnabled || matchController == null) return;
            matchController.AuthorityTickResolved += OnAuthorityTickResolved;
            _subscribedToAuthorityTick = true;
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
            if (health != null && health.Snapshot.IsDefeated)
            {
                _lastSubmittedMovement = Float2.Zero;
                _continuousStuckTicks = 0;
                return;
            }

            var stuck = _navigation.IsStuck;
            if (_simulationTick >= _nextDecisionTick)
            {
                _decisionTimer.Restart();
                var snapshot = perception.Capture();
                _decision = _engine.Decide(snapshot, _simulationTick, _profile, _random, stuck);
                if (_decision.TargetId.Value != 0) TargetDecisionCount++;
                if (_decision.State == BotDecisionState.Engage) EngageDecisionCount++;
                if (_decision.Attack) AttackDecisionCount++;
                if ((snapshot.Zone.IsOutsideCurrent(snapshot.Position) || snapshot.Zone.IsOutsideNext(snapshot.Position)) &&
                    _decision.State == BotDecisionState.Reposition)
                {
                    ZoneSafetyDecisionCount++;
                }
                _decisionTimer.Stop();
                DecisionCount++;
                MaxDecisionMilliseconds = Mathf.Max((float)MaxDecisionMilliseconds, (float)_decisionTimer.Elapsed.TotalMilliseconds);
                _nextDecisionTick = _simulationTick + _decisionIntervalTicks;
                if (_decision.TargetId.Value != 0 && _decision.TargetId.Value != _lastTargetId)
                {
                    if (_lastTargetId != 0) TargetSwitchCount++;
                    _lastTargetId = _decision.TargetId.Value;
                }
            if (_decision.StuckRecovery && !_lastStuckRecovery)
            {
                StuckRecoveryCount++;
                _recoveryDirectionIndex = (_recoveryDirectionIndex + 1) & 7;
                _recoveryTicks = 0;
                _recoveryMovement = FindRecoveryMovement(_decision.Movement);
                _hasRecoveryMovement = true;
            }
            else if (_decision.StuckRecovery)
            {
                _recoveryTicks += _decisionIntervalTicks;
                if (_recoveryTicks >= RecoveryDirectionRefreshTicks)
                {
                    _recoveryTicks = 0;
                    _recoveryDirectionIndex = (_recoveryDirectionIndex + 1) & 7;
                    _recoveryMovement = FindRecoveryMovement(_decision.Movement);
                }
            }
            else
            {
                _recoveryTicks = 0;
                _hasRecoveryMovement = false;
            }
                _lastStuckRecovery = _decision.StuckRecovery;
                if (!_decision.Ability) _abilityIssued = false;
                if (CombatEnabled) gadgetUser?.UseForContext(snapshot, _simulationTick);
            }

            var movement = _decision.Movement;
            if (_decision.StuckRecovery)
            {
                movement = _hasRecoveryMovement ? _recoveryMovement : FindRecoveryMovement(movement);
            }
            else if (movement.SqrMagnitude > 0.04f && perception.IsWorldBlocked(movement))
            {
                var tangent = new Float2(-movement.Y, movement.X) * _avoidanceSide;
                var oppositeTangent = new Float2(-tangent.X, -tangent.Y);
                var tangentProgress = perception.GetWorldMovementProgress(tangent);
                var oppositeProgress = perception.GetWorldMovementProgress(oppositeTangent);
                tangent = oppositeProgress > tangentProgress ? oppositeTangent : tangent;
                movement = tangent.SqrMagnitude > 0.000001f ? tangent.Normalized : movement;
            }

            _lastSubmittedMovement = movement;
            var input = new MovementInputFrame(movement, _decision.Aim);
            movementAgent.Submit(MovementCommandFactory.Create(movementAgent.ActorId, _simulationTick, input, movementAgent.Tuning), fixedDeltaSeconds);

            var attackCommandIssued = false;
            var commandAim = _decision.Aim;
            if (CombatEnabled && attackController != null && _decision.Attack && _simulationTick >= _nextAttackTick)
            {
                var currentRange = attackController.AuthorityWeaponDefinition.MaxRange * 0.95f;
                if (!perception.IsTargetWithinRange(_decision.TargetId, currentRange))
                {
                    OutOfRangeAttackAttemptCount++;
                    _nextAttackTick = _simulationTick + Mathf.Max(1, _decisionIntervalTicks);
                }
                else
                {
                    AttackAttemptCount++;
                    if (perception.TryGetCurrentTargetAim(_decision.TargetId, out var currentAim))
                    {
                        commandAim = (currentAim * 0.88f + _decision.Aim * 0.12f).Normalized;
                    }

                    var origin = new Float2(transform.position.x, transform.position.z) + commandAim * 0.7f;
                    attackController.Submit(AttackCommandFactory.Create(
                        new CombatEntityId(movementAgent.ActorId),
                        _simulationTick,
                        origin,
                        commandAim,
                        true,
                        _attackInputSequence++));
                    attackCommandIssued = true;
                    _nextAttackTick = _simulationTick + Mathf.Max(
                        1,
                        Mathf.CeilToInt(
                            attackController.AuthorityWeaponDefinition.FireIntervalSeconds *
                            _clock.TickRate * Mathf.Max(1f, attackCadenceMultiplier)));
                }
            }

            var abilityCommandIssued = false;
            if (CombatEnabled && _abilityController != null && _decision.Ability && !_abilityIssued && _abilityController.AbilityCooldownRemaining <= 0.05f)
            {
                AbilityAttemptCount++;
                _abilityController.Submit(AbilityCommandFactory.Create(
                    new CombatEntityId(movementAgent.ActorId),
                    _simulationTick,
                    _abilityController.AbilityId,
                    _decision.Aim,
                    true));
                _abilityIssued = true;
                abilityCommandIssued = true;
            }

            RecordCommand(_simulationTick, movement, commandAim, attackCommandIssued, abilityCommandIssued);

            if (!_navigation.IsStuck && _decision.StuckRecovery) _engine.Reset();
        }

        private void OnAuthorityTickResolved(MatchAuthorityTick tick)
        {
            if (!isActiveAndEnabled || matchController == null || !matchController.IsMatchStarted) return;
            if (health != null && health.Snapshot.IsDefeated)
            {
                _continuousStuckTicks = 0;
                return;
            }

            if (!CombatEnabled || matchController.IsAuthorityMovementLocked(new CombatEntityId(movementAgent.ActorId)))
            {
                _navigation.Clear();
                _continuousStuckTicks = 0;
                return;
            }

            _navigation.Observe(
                new Float2(transform.position.x, transform.position.z),
                _lastSubmittedMovement,
                1f / 30f,
                _profile.StuckTimeoutSeconds);
            if (_navigation.IsStuck && !_hasRecoveryMovement)
            {
                _continuousStuckTicks++;
                if (_continuousStuckTicks > MaxContinuousStuckTicks)
                {
                    MaxContinuousStuckTicks = _continuousStuckTicks;
                    MaxStuckPosition = new Float2(transform.position.x, transform.position.z);
                    MaxStuckSimulationTick = tick.SimulationTick;
                }
            }
            else
            {
                _continuousStuckTicks = 0;
            }
        }

        private static void ResetAbilityTelemetry(MonoBehaviour controller)
        {
            if (controller == null) return;
            controller.SendMessage("ResetFighterState", SendMessageOptions.DontRequireReceiver);
        }

        private void RecordCommand(int simulationTick, Float2 movement, Float2 aim, bool attack, bool ability)
        {
            CommandDigest = Mix(CommandDigest, simulationTick);
            CommandDigest = Mix(CommandDigest, Quantize(movement.X));
            CommandDigest = Mix(CommandDigest, Quantize(movement.Y));
            CommandDigest = Mix(CommandDigest, Quantize(aim.X));
            CommandDigest = Mix(CommandDigest, Quantize(aim.Y));
            CommandDigest = Mix(CommandDigest, attack ? 1 : 0);
            CommandDigest = Mix(CommandDigest, ability ? 1 : 0);
            CommandCount++;
        }

        private static int Quantize(float value) => Mathf.RoundToInt(value * CommandDigestQuantization);

        private Float2 FindRecoveryMovement(Float2 requested)
        {
            var toArenaCenter = new Float2(-transform.position.x, -transform.position.z);
            var best = toArenaCenter.SqrMagnitude > 0.000001f ? toArenaCenter.Normalized : Float2.Up;
            var bestProgress = perception.GetWorldMovementProgress(best);

            var requestedDir = requested.SqrMagnitude > 0.000001f ? requested.Normalized : Float2.Up;
            var requestedProgress = perception.GetWorldMovementProgress(requestedDir);
            if (requestedProgress > bestProgress + 0.001f)
            {
                best = requestedDir;
                bestProgress = requestedProgress;
            }

            for (var attempt = 0; attempt < 8; attempt++)
            {
                var candidate = RecoveryDirection((_recoveryDirectionIndex + attempt) & 7);
                var progress = perception.GetWorldMovementProgress(candidate);
                if (progress > bestProgress + 0.001f)
                {
                    best = candidate;
                    bestProgress = progress;
                }
            }

            return best;
        }

        private static Float2 RecoveryDirection(int index)
        {
            switch (index & 7)
            {
                case 0: return Float2.Up;
                case 1: return new Float2(1f, 1f).Normalized;
                case 2: return new Float2(1f, 0f);
                case 3: return new Float2(1f, -1f).Normalized;
                case 4: return new Float2(0f, -1f);
                case 5: return new Float2(-1f, -1f).Normalized;
                case 6: return new Float2(-1f, 0f);
                default: return new Float2(-1f, 1f).Normalized;
            }
        }

        private static ulong Mix(ulong hash, int value)
        {
            unchecked
            {
                hash ^= (uint)value;
                hash *= 1099511628211UL;
                return hash;
            }
        }

        private void SelectActiveAbilityController()
        {
            var candidates = GetComponents<MonoBehaviour>();
            MonoBehaviour selected = null;
            for (var i = 0; i < candidates.Length; i++)
            {
                var candidate = candidates[i];
                if (candidate == null || !(candidate is IFighterAbilityController)) continue;
                if (candidate.enabled && selected == null)
                {
                    selected = candidate;
                }
            }

            fighterController = selected != null ? selected : GetComponents<MonoBehaviour>()
                .FirstOrDefault(candidate => candidate is IFighterAbilityController);
            _abilityController = fighterController as IFighterAbilityController;
        }
    }
}
