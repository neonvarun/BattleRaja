using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Visuals;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatAttackController : MonoBehaviour, IAttackCommandSink
    {
        [SerializeField] private int actorId = 1;
        [SerializeField] private CombatFaction faction = CombatFaction.Player;
        [SerializeField] private ProjectileWeaponAsset weapon;
        [SerializeField] private FighterDefinitionAsset fighterDefinition;
        [SerializeField] private PlayerInputAdapter inputAdapter;
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private CombatProjectilePool projectilePool;
        [SerializeField] private OfflineMatchController match;
        [SerializeField] private int simulationTickRate = 30;

        private WeaponCooldownState _cooldown;
        private ProjectileWeaponDefinition _definition;
        private FixedSimulationClock _clock;
        private int _simulationTick;
        private int _inputSequence;

        public ProjectileWeaponDefinition AuthorityWeaponDefinition => ResolveDefinition();
        public int AuthorityTickRate => Mathf.Max(1, simulationTickRate);
        public CombatFaction AuthorityFaction => faction;

        public int ActiveProjectileCount => projectilePool != null ? projectilePool.ActiveCount : 0;
        public float CooldownRemaining
        {
            get
            {
                if (_clock == null) return 0f;
                if (UsesAuthority)
                {
                    return match.GetAttackCooldownRemaining(
                        new CombatEntityId(actorId),
                        AuthorityTickRate,
                        match.SimulationTick);
                }

                return _cooldown != null ? _cooldown.RemainingSeconds(_clock.Tick, _clock.TickRate) : 0f;
            }
        }

        private bool UsesAuthority => match != null && match.AuthorityDrivenMovement && match.Simulation != null &&
            match.IsAuthorityActor(new CombatEntityId(actorId));

        public void ConfigureFighter(FighterDefinitionAsset definition)
        {
            fighterDefinition = definition;
            if (definition != null)
            {
                _definition = definition.ToDomain().BasicAttack;
            }
        }

        private void Awake()
        {
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            projectilePool = projectilePool != null ? projectilePool : FindAnyObjectByType<CombatProjectilePool>();
            match = match != null ? match : FindAnyObjectByType<OfflineMatchController>();
            _definition = fighterDefinition != null
                ? fighterDefinition.ToDomain().BasicAttack
                : (weapon != null ? weapon.ToDomain() : ProjectileWeaponDefinition.TrainingBolt);
            _cooldown = new WeaponCooldownState();
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
            _inputSequence = 0;
        }

        private void Start()
        {
            if (match != null)
            {
                match.SimulationTickAdvanced += OnCanonicalSimulationTick;
            }
        }

        private void OnDestroy()
        {
            if (match != null)
            {
                match.SimulationTickAdvanced -= OnCanonicalSimulationTick;
            }
        }

        private void Update()
        {
            if (UsesAuthority && match.IsMatchStarted)
            {
                return;
            }

            var attackHeld = inputAdapter != null && inputAdapter.isActiveAndEnabled && inputAdapter.IsAttackHeld;
            var steps = _clock.Consume(Time.deltaTime);
            for (var step = 0; step < steps; step++)
            {
                _simulationTick = _clock.GetConsumedTick(step);
                if (attackHeld)
                {
                    var direction = movementAgent != null ? movementAgent.AimDirection : Float2.Up;
                    var origin = new Float2(transform.position.x, transform.position.z) + direction * 0.7f;
                    Submit(AttackCommandFactory.Create(
                        new CombatEntityId(actorId),
                        _simulationTick,
                        origin,
                        direction,
                        true));
                }
            }
        }

        private void OnCanonicalSimulationTick(int simulationTick, float fixedDeltaSeconds)
        {
            if (!isActiveAndEnabled || !UsesAuthority || inputAdapter == null || !inputAdapter.isActiveAndEnabled ||
                !inputAdapter.IsAttackHeld)
            {
                return;
            }

            var direction = movementAgent != null ? movementAgent.AimDirection : Float2.Up;
            var origin = new Float2(transform.position.x, transform.position.z) + direction * 0.7f;
            Submit(AttackCommandFactory.Create(
                new CombatEntityId(actorId),
                simulationTick,
                origin,
                direction,
                true,
                _inputSequence++));
        }

        public void Submit(AttackCommand command)
        {
            if (!command.Pressed || !_definition.IsValid(out _))
            {
                return;
            }

            var spawnDefinition = _definition;
            var spawnFaction = faction;
            var spawnCommand = command;

            if (UsesAuthority)
            {
                var authority = match.TryAcceptAttack(command);
                if (!authority.Accepted) return;
                spawnDefinition = authority.Weapon;
                spawnFaction = authority.Faction;
                spawnCommand = new AttackCommand(
                    command.InstigatorId,
                    authority.SimulationTick,
                    authority.Origin,
                    authority.Direction,
                    command.Pressed,
                    command.InputSequence);
            }
            else
            {
                var intervalTicks = Mathf.Max(1, Mathf.CeilToInt(_definition.FireIntervalSeconds * _clock.TickRate));
                if (!_cooldown.TryConsume(command.SimulationTick, intervalTicks)) return;
            }

            if (projectilePool != null && projectilePool.Spawn(spawnCommand, spawnDefinition, spawnFaction) != null)
            {
                GetComponent<FighterPresentation>()?.NotifyAttack();
            }
        }

        public void ResetAttackState()
        {
            _cooldown?.Reset();
            _inputSequence = 0;
        }

        private ProjectileWeaponDefinition ResolveDefinition()
        {
            if (fighterDefinition != null) return fighterDefinition.ToDomain().BasicAttack;
            return weapon != null ? weapon.ToDomain() : ProjectileWeaponDefinition.TrainingBolt;
        }
    }
}
