using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatProjectile : MonoBehaviour
    {
        private ProjectileSimulation _simulation;
        private ProjectileWeaponDefinition _definition;
        private CombatEntityId _instigatorId;
        private CombatFaction _instigatorFaction;
        private CombatProjectilePool _pool;
        private CombatDamageResolver _damageResolver;
        private CombatImpactFeedbackPool _impactPool;
        private FixedSimulationClock _clock;
        private Float2 _direction;
        private int _projectileId;
        private bool _active;

        public bool IsActive => _active;
        public int ProjectileId => _projectileId;

        public void Launch(
            AttackCommand command,
            ProjectileWeaponDefinition definition,
            CombatFaction instigatorFaction,
            CombatProjectilePool pool,
            CombatDamageResolver damageResolver,
            CombatImpactFeedbackPool impactPool,
            int projectileId = 0)
        {
            _definition = definition;
            _instigatorId = command.InstigatorId;
            _instigatorFaction = instigatorFaction;
            _pool = pool;
            _damageResolver = damageResolver;
            _impactPool = impactPool;
            _direction = command.Direction;
            _projectileId = projectileId;
            _simulation = new ProjectileSimulation(
                command.Origin,
                command.Direction,
                definition.ProjectileSpeed,
                definition.MaxRange,
                definition.LifetimeSeconds);
            _clock = new FixedSimulationClock(30);
            transform.position = new Vector3(command.Origin.X, pool != null ? pool.ProjectileHeight : 1f, command.Origin.Y);
            transform.forward = new Vector3(command.Direction.X, 0f, command.Direction.Y);
            _active = true;
            gameObject.SetActive(true);
        }

        public void ResetProjectile()
        {
            _active = false;
            _projectileId = 0;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_active)
            {
                return;
            }

            var steps = _clock.Consume(Time.deltaTime);
            for (var i = 0; i < steps; i++)
            {
                var step = _simulation.Step((float)_clock.StepSeconds);
                transform.position = new Vector3(step.Position.X, _pool != null ? _pool.ProjectileHeight : 1f, step.Position.Y);
                if (step.Expired)
                {
                    Despawn(step.Reason);
                    return;
                }
            }
        }

        public void Despawn(ProjectileDespawnReason reason)
        {
            if (!_active) return;
            _active = false;
            _pool?.Release(this, reason);
        }
    }
}
