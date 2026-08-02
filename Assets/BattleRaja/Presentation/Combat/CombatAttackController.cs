using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
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

        private WeaponCooldownState _cooldown;
        private ProjectileWeaponDefinition _definition;
        private int _simulationTick;

        public int ActiveProjectileCount => projectilePool != null ? projectilePool.ActiveCount : 0;
        public float CooldownRemaining => _cooldown != null ? _cooldown.Remaining(Time.time) : 0f;

        private void Awake()
        {
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            projectilePool = projectilePool != null ? projectilePool : FindFirstObjectByType<CombatProjectilePool>();
            _definition = fighterDefinition != null
                ? fighterDefinition.ToDomain().BasicAttack
                : (weapon != null ? weapon.ToDomain() : ProjectileWeaponDefinition.TrainingBolt);
            _cooldown = new WeaponCooldownState();
        }

        private void Update()
        {
            if (inputAdapter != null && inputAdapter.isActiveAndEnabled && inputAdapter.IsAttackHeld)
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

            _simulationTick++;
        }

        public void Submit(AttackCommand command)
        {
            if (!command.Pressed || projectilePool == null || !_definition.IsValid(out _))
            {
                return;
            }

            if (_cooldown.TryConsume(Time.time, _definition.FireIntervalSeconds))
            {
                projectilePool.Spawn(command, _definition, faction);
            }
        }

        public void ResetAttackState() => _cooldown?.Reset();
    }
}
