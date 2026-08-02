using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    [CreateAssetMenu(menuName = "BattleRaja/Combat/Projectile Weapon", fileName = "ProjectileWeapon")]
    public sealed class ProjectileWeaponAsset : ScriptableObject
    {
        [Min(1)] [SerializeField] private int damage = 25;
        [Min(0.01f)] [SerializeField] private float fireIntervalSeconds = 0.35f;
        [Min(0.01f)] [SerializeField] private float projectileSpeed = 12f;
        [Min(0.01f)] [SerializeField] private float maxRange = 18f;
        [Min(0.01f)] [SerializeField] private float lifetimeSeconds = 1.5f;
        [Min(0.01f)] [SerializeField] private float radius = 0.16f;
        [SerializeField] private LayerMask collisionLayers = -5;
        [SerializeField] private bool allowSelfHit;
        [SerializeField] private bool allowFriendlyFire;

        public ProjectileWeaponDefinition ToDomain()
        {
            return new ProjectileWeaponDefinition(
                damage,
                fireIntervalSeconds,
                projectileSpeed,
                maxRange,
                lifetimeSeconds,
                radius,
                collisionLayers.value,
                allowSelfHit,
                allowFriendlyFire);
        }

        private void OnValidate()
        {
            if (!ToDomain().IsValid(out var reason))
            {
                Debug.LogWarning($"{name} is invalid: {reason}", this);
            }
        }
    }
}
