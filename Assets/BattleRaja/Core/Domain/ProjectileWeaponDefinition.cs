namespace BattleRaja.Core.Domain
{
    public readonly struct ProjectileWeaponDefinition
    {
        public ProjectileWeaponDefinition(
            int damage,
            float fireIntervalSeconds,
            float projectileSpeed,
            float maxRange,
            float lifetimeSeconds,
            float radius,
            int collisionLayerMask,
            bool allowSelfHit,
            bool allowFriendlyFire)
        {
            Damage = damage;
            FireIntervalSeconds = fireIntervalSeconds;
            ProjectileSpeed = projectileSpeed;
            MaxRange = maxRange;
            LifetimeSeconds = lifetimeSeconds;
            Radius = radius;
            CollisionLayerMask = collisionLayerMask;
            AllowSelfHit = allowSelfHit;
            AllowFriendlyFire = allowFriendlyFire;
        }

        public int Damage { get; }
        public float FireIntervalSeconds { get; }
        public float ProjectileSpeed { get; }
        public float MaxRange { get; }
        public float LifetimeSeconds { get; }
        public float Radius { get; }
        public int CollisionLayerMask { get; }
        public bool AllowSelfHit { get; }
        public bool AllowFriendlyFire { get; }
        public ContentId WeaponId => ContentId.Attack("BasicAttack");

        public bool IsValid(out string reason)
        {
            if (float.IsNaN(FireIntervalSeconds) || float.IsInfinity(FireIntervalSeconds) ||
                float.IsNaN(ProjectileSpeed) || float.IsInfinity(ProjectileSpeed) ||
                float.IsNaN(MaxRange) || float.IsInfinity(MaxRange) ||
                float.IsNaN(LifetimeSeconds) || float.IsInfinity(LifetimeSeconds) ||
                float.IsNaN(Radius) || float.IsInfinity(Radius))
            {
                reason = "Weapon timing and projectile dimensions must be finite.";
                return false;
            }

            if (Damage <= 0) { reason = "Damage must be positive."; return false; }
            if (FireIntervalSeconds <= 0f) { reason = "Fire interval must be positive."; return false; }
            if (ProjectileSpeed <= 0f) { reason = "Projectile speed must be positive."; return false; }
            if (MaxRange <= 0f) { reason = "Maximum range must be positive."; return false; }
            if (LifetimeSeconds <= 0f) { reason = "Lifetime must be positive."; return false; }
            if (Radius <= 0f) { reason = "Projectile radius must be positive."; return false; }
            if (CollisionLayerMask == 0) { reason = "At least one collision layer is required."; return false; }
            reason = string.Empty;
            return true;
        }

        public static ProjectileWeaponDefinition TrainingBolt => new ProjectileWeaponDefinition(
            damage: 25,
            fireIntervalSeconds: 0.35f,
            projectileSpeed: 12f,
            maxRange: 18f,
            lifetimeSeconds: 1.5f,
            radius: 0.16f,
            collisionLayerMask: ~0,
            allowSelfHit: false,
            allowFriendlyFire: false);

        public static ProjectileWeaponDefinition BijliElectricBolt => new ProjectileWeaponDefinition(
            damage: 12,
            fireIntervalSeconds: 0.42f,
            projectileSpeed: 14f,
            maxRange: 16f,
            lifetimeSeconds: 1.2f,
            radius: 0.14f,
            collisionLayerMask: ~0,
            allowSelfHit: false,
            allowFriendlyFire: false);

        public static ProjectileWeaponDefinition PehelHeavyBolt => new ProjectileWeaponDefinition(
            damage: 20,
            fireIntervalSeconds: 0.72f,
            projectileSpeed: 10f,
            maxRange: 5.5f,
            lifetimeSeconds: 0.7f,
            radius: 0.45f,
            collisionLayerMask: ~0,
            allowSelfHit: false,
            allowFriendlyFire: false);
    }
}
