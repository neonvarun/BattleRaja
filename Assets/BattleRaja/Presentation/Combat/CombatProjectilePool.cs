using System.Collections.Generic;
using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatProjectilePool : MonoBehaviour
    {
        [SerializeField] private int prewarmCount = 8;
        [SerializeField] private int maxCount = 32;
        [SerializeField] private float projectileHeight = 1f;
        [SerializeField] private Material projectileMaterial;
        [SerializeField] private CombatDamageResolver damageResolver;
        [SerializeField] private CombatImpactFeedbackPool impactPool;

        private readonly Stack<CombatProjectile> _available = new Stack<CombatProjectile>();
        private int _createdCount;

        public int CreatedCount => _createdCount;
        public int ActiveCount { get; private set; }
        public float ProjectileHeight => projectileHeight;

        private void Awake()
        {
            damageResolver = damageResolver != null ? damageResolver : FindFirstObjectByType<CombatDamageResolver>();
            impactPool = impactPool != null ? impactPool : FindFirstObjectByType<CombatImpactFeedbackPool>();
            var count = Mathf.Clamp(prewarmCount, 0, Mathf.Max(0, maxCount));
            for (var i = 0; i < count; i++)
            {
                _available.Push(CreateProjectile());
            }
        }

        public CombatProjectile Spawn(
            AttackCommand command,
            ProjectileWeaponDefinition definition,
            CombatFaction faction)
        {
            if (!definition.IsValid(out _))
            {
                return null;
            }

            if (_available.Count == 0 && _createdCount >= maxCount)
            {
                return null;
            }

            var projectile = _available.Count > 0 ? _available.Pop() : CreateProjectile();
            ActiveCount++;
            projectile.Launch(command, definition, faction, this, damageResolver, impactPool);
            return projectile;
        }

        internal void Release(CombatProjectile projectile, ProjectileDespawnReason reason)
        {
            if (projectile == null)
            {
                return;
            }

            projectile.ResetProjectile();
            _available.Push(projectile);
            ActiveCount = Mathf.Max(0, ActiveCount - 1);
        }

        private CombatProjectile CreateProjectile()
        {
            var objectToPool = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            objectToPool.name = "PooledProjectile";
            objectToPool.transform.SetParent(transform, false);
            objectToPool.transform.localScale = Vector3.one * 0.28f;
            // Keep the concrete collider type referenced so IL2CPP/WebGL does not strip
            // SphereCollider when the primitive is created through Unity's factory.
            var collider = objectToPool.GetComponent<SphereCollider>();
            if (collider != null)
            {
                Destroy(collider);
            }

            var renderer = objectToPool.GetComponent<Renderer>();
            if (projectileMaterial != null)
            {
                renderer.sharedMaterial = projectileMaterial;
            }

            var projectile = objectToPool.AddComponent<CombatProjectile>();
            projectile.ResetProjectile();
            _createdCount++;
            return projectile;
        }
    }
}
