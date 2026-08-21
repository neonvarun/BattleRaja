using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatProjectile : MonoBehaviour
    {
        private const int MaxLocalHits = 16;

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
        private readonly ProjectileHitTracker _hitTracker = new ProjectileHitTracker();
        private readonly Collider[] _hitBuffer = new Collider[MaxLocalHits];

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
            _hitTracker.Clear();
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
            _hitTracker.Clear();
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

                // Authority projectiles (id > 0) are resolved by the canonical
                // match authority; local lab projectiles keep the M2 contract of
                // presentation-side collision feeding the central damage resolver.
                if (_projectileId <= 0 && ResolveLocalHit())
                {
                    return;
                }

                if (step.Expired)
                {
                    Despawn(step.Reason);
                    return;
                }
            }
        }

        private bool ResolveLocalHit()
        {
            if (_damageResolver == null)
            {
                return false;
            }

            var hits = Physics.OverlapSphereNonAlloc(
                transform.position,
                Mathf.Max(0.01f, _definition.Radius),
                _hitBuffer,
                _definition.CollisionLayerMask);

            CombatTarget actorTarget = null;
            var staticBlocked = false;
            for (var i = 0; i < hits; i++)
            {
                var collider = _hitBuffer[i];
                if (collider == null || collider.isTrigger)
                {
                    continue;
                }

                var target = collider.GetComponentInParent<CombatTarget>();
                if (target == null || target.Health == null)
                {
                    staticBlocked = true;
                    continue;
                }

                if (!_definition.AllowSelfHit && target.Id.Equals(_instigatorId))
                {
                    continue;
                }

                if (!_definition.AllowFriendlyFire && target.Faction == _instigatorFaction &&
                    !target.Id.Equals(_instigatorId))
                {
                    continue;
                }

                if (!_hitTracker.TryRegister(target.Id))
                {
                    continue;
                }

                actorTarget = target;
                break;
            }

            if (actorTarget != null)
            {
                var request = new DamageRequest(
                    _instigatorId,
                    actorTarget.Id,
                    _instigatorFaction,
                    _definition.Damage,
                    DamageType.Projectile,
                    _direction,
                    0);
                var result = _damageResolver.Resolve(
                    actorTarget,
                    request,
                    _definition.AllowSelfHit,
                    _definition.AllowFriendlyFire);
                _impactPool?.Play(actorTarget.transform.position, result.Applied);
                Despawn(ProjectileDespawnReason.HitActor);
                return true;
            }

            if (staticBlocked)
            {
                _impactPool?.Play(transform.position, false);
                Despawn(ProjectileDespawnReason.HitWall);
                return true;
            }

            return false;
        }

        public void Despawn(ProjectileDespawnReason reason)
        {
            if (!_active) return;
            _active = false;
            _pool?.Release(this, reason);
        }
    }
}
