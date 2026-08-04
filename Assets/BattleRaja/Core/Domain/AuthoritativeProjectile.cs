using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct DomainProjectileSnapshot
    {
        public DomainProjectileSnapshot(
            int projectileId,
            int attackExecutionId,
            CombatEntityId instigatorId,
            ContentId weaponId,
            int spawnTick,
            Float2 position,
            Float2 direction,
            float speed,
            float radius,
            float remainingRange,
            float remainingLifetime,
            CombatFaction faction,
            bool isActive,
            ProjectileDespawnReason despawnReason,
            CombatEntityId hitTargetId)
        {
            ProjectileId = projectileId;
            AttackExecutionId = attackExecutionId;
            InstigatorId = instigatorId;
            WeaponId = weaponId;
            SpawnTick = spawnTick;
            Position = position;
            Direction = direction;
            Speed = speed;
            Radius = radius;
            RemainingRange = remainingRange;
            RemainingLifetime = remainingLifetime;
            Faction = faction;
            IsActive = isActive;
            DespawnReason = despawnReason;
            HitTargetId = hitTargetId;
        }

        public int ProjectileId { get; }
        public int AttackExecutionId { get; }
        public CombatEntityId InstigatorId { get; }
        public ContentId WeaponId { get; }
        public int SpawnTick { get; }
        public Float2 Position { get; }
        public Float2 Direction { get; }
        public float Speed { get; }
        public float Radius { get; }
        public float RemainingRange { get; }
        public float RemainingLifetime { get; }
        public CombatFaction Faction { get; }
        public bool IsActive { get; }
        public ProjectileDespawnReason DespawnReason { get; }
        public CombatEntityId HitTargetId { get; }
    }

    public sealed class AuthoritativeProjectile
    {
        public AuthoritativeProjectile(
            int projectileId,
            int attackExecutionId,
            CombatEntityId instigatorId,
            ContentId weaponId,
            int spawnTick,
            Float2 position,
            Float2 direction,
            float speed,
            float radius,
            float maxRange,
            float maxLifetime,
            CombatFaction faction)
        {
            ProjectileId = projectileId;
            AttackExecutionId = attackExecutionId;
            InstigatorId = instigatorId;
            WeaponId = weaponId;
            SpawnTick = spawnTick;
            Position = position;
            Direction = direction.Normalized;
            Speed = speed;
            Radius = radius;
            RemainingRange = maxRange;
            RemainingLifetime = maxLifetime;
            Faction = faction;
            IsActive = true;
            DespawnReason = ProjectileDespawnReason.None;
            HitTargetId = default;
        }

        public int ProjectileId { get; }
        public int AttackExecutionId { get; }
        public CombatEntityId InstigatorId { get; }
        public ContentId WeaponId { get; }
        public int SpawnTick { get; }
        public Float2 Position { get; private set; }
        public Float2 Direction { get; }
        public float Speed { get; }
        public float Radius { get; }
        public float RemainingRange { get; private set; }
        public float RemainingLifetime { get; private set; }
        public CombatFaction Faction { get; }
        public bool IsActive { get; private set; }
        public ProjectileDespawnReason DespawnReason { get; private set; }
        public CombatEntityId HitTargetId { get; private set; }

        public void MoveTo(Float2 newPosition, float distanceTraveled, float deltaSeconds)
        {
            Position = newPosition;
            RemainingRange = Math.Max(0f, RemainingRange - distanceTraveled);
            RemainingLifetime = Math.Max(0f, RemainingLifetime - deltaSeconds);
        }

        public void Despawn(ProjectileDespawnReason reason, CombatEntityId hitTarget = default)
        {
            IsActive = false;
            DespawnReason = reason;
            HitTargetId = hitTarget;
        }

        public DomainProjectileSnapshot ToSnapshot() => new DomainProjectileSnapshot(
            ProjectileId,
            AttackExecutionId,
            InstigatorId,
            WeaponId,
            SpawnTick,
            Position,
            Direction,
            Speed,
            Radius,
            RemainingRange,
            RemainingLifetime,
            Faction,
            IsActive,
            DespawnReason,
            HitTargetId);
    }
}
