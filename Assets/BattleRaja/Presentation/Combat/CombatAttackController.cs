using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
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
        [SerializeField] private int simulationTickRate = 30;

        private WeaponCooldownState _cooldown;
        private ProjectileWeaponDefinition _definition;
        private FixedSimulationClock _clock;
        private int _simulationTick;

        public int ActiveProjectileCount => projectilePool != null ? projectilePool.ActiveCount : 0;
        public float CooldownRemaining => _cooldown != null && _clock != null
            ? _cooldown.RemainingSeconds(_clock.Tick, _clock.TickRate)
            : 0f;

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
            projectilePool = projectilePool != null ? projectilePool : FindFirstObjectByType<CombatProjectilePool>();
            _definition = fighterDefinition != null
                ? fighterDefinition.ToDomain().BasicAttack
                : (weapon != null ? weapon.ToDomain() : ProjectileWeaponDefinition.TrainingBolt);
            _cooldown = new WeaponCooldownState();
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
        }

        private void Update()
        {
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

        public void Submit(AttackCommand command)
        {
            if (!command.Pressed || projectilePool == null || !_definition.IsValid(out _))
            {
                return;
            }

            var intervalTicks = Mathf.Max(1, Mathf.CeilToInt(_definition.FireIntervalSeconds * _clock.TickRate));
            if (_cooldown.TryConsume(command.SimulationTick, intervalTicks))
            {
                if (projectilePool.Spawn(command, _definition, faction) != null)
                {
                    GetComponent<FighterPresentation>()?.NotifyAttack();
                }
            }
        }

        public void ResetAttackState() => _cooldown?.Reset();
    }
}
