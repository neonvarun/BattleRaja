using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatProjectile : MonoBehaviour
    {
        private ProjectileSimulation _simulation;
        private ProjectileHitTracker _hitTracker;
        private ProjectileWeaponDefinition _definition;
        private CombatEntityId _instigatorId;
        private CombatFaction _instigatorFaction;
        private CombatProjectilePool _pool;
        private CombatDamageResolver _damageResolver;
        private CombatImpactFeedbackPool _impactPool;
        private FixedSimulationClock _clock;
        private Float2 _direction;
        private bool _active;

        public bool IsActive => _active;

        public void Launch(
            AttackCommand command,
            ProjectileWeaponDefinition definition,
            CombatFaction instigatorFaction,
            CombatProjectilePool pool,
            CombatDamageResolver damageResolver,
            CombatImpactFeedbackPool impactPool)
        {
            _definition = definition;
            _instigatorId = command.InstigatorId;
            _instigatorFaction = instigatorFaction;
            _pool = pool;
            _damageResolver = damageResolver;
            _impactPool = impactPool;
            _direction = command.Direction;
            _simulation = new ProjectileSimulation(
                command.Origin,
                command.Direction,
                definition.ProjectileSpeed,
                definition.MaxRange,
                definition.LifetimeSeconds);
            _clock = new FixedSimulationClock(30);
            _hitTracker = new ProjectileHitTracker();
            transform.position = new Vector3(command.Origin.X, pool.ProjectileHeight, command.Origin.Y);
            transform.forward = new Vector3(command.Direction.X, 0f, command.Direction.Y);
            _active = true;
            gameObject.SetActive(true);
        }

        public void ResetProjectile()
        {
            _active = false;
            _hitTracker?.Clear();
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
                var start = transform.position;
                var step = _simulation.Step((float)_clock.StepSeconds);
                var end = new Vector3(step.Position.X, start.y, step.Position.Y);
                var delta = end - start;
                var distance = delta.magnitude;
                if (distance > 0.00001f && Physics.SphereCast(
                        start,
                        _definition.Radius,
                        delta / distance,
                        out var hit,
                        distance,
                        _definition.CollisionLayerMask,
                        QueryTriggerInteraction.Ignore))
                {
                    var target = hit.collider.GetComponentInParent<CombatTarget>();
                    if (target != null && target.Id != _instigatorId && _hitTracker.TryRegister(target.Id))
                    {
                        var request = new DamageRequest(
                            _instigatorId,
                            target.Id,
                            _instigatorFaction,
                            _definition.Damage,
                            DamageType.Projectile,
                            _direction,
                            _clock.Tick);
                        var result = _damageResolver.Resolve(
                            target,
                            request,
                            _definition.AllowSelfHit,
                            _definition.AllowFriendlyFire,
                            _clock.Tick);
                        _impactPool?.Play(hit.point, result.Applied);
                        Despawn(ProjectileDespawnReason.Hit);
                        return;
                    }

                    if (target == null)
                    {
                        _impactPool?.Play(hit.point, false);
                        Despawn(ProjectileDespawnReason.Collision);
                        return;
                    }
                }

                transform.position = end;
                if (step.Expired)
                {
                    Despawn(step.Reason);
                    return;
                }
            }
        }

        private void Despawn(ProjectileDespawnReason reason)
        {
            _active = false;
            _pool.Release(this, reason);
        }
    }
}
