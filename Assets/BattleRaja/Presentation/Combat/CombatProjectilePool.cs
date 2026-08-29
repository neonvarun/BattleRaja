using System.Collections.Generic;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Visuals;
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
        private readonly Dictionary<int, CombatProjectile> _authoritativeShells =
            new Dictionary<int, CombatProjectile>();
        private readonly List<int> _staleShellIds = new List<int>(8);
        private int _createdCount;

        public int CreatedCount => _createdCount;
        public int ActiveCount { get; private set; }
        public float ProjectileHeight => projectileHeight;
        public int AuthoritativeShellCount => _authoritativeShells.Count;

        private void Awake()
        {
            damageResolver = damageResolver != null ? damageResolver : FindAnyObjectByType<CombatDamageResolver>();
            impactPool = impactPool != null ? impactPool : FindAnyObjectByType<CombatImpactFeedbackPool>();
            var count = Mathf.Clamp(prewarmCount, 0, Mathf.Max(0, maxCount));
            for (var i = 0; i < count; i++)
            {
                _available.Push(CreateProjectile());
            }
        }

        public CombatProjectile Spawn(
            AttackCommand command,
            ProjectileWeaponDefinition definition,
            CombatFaction faction,
            int projectileId = 0)
        {
            if (!definition.IsValid(out _))
            {
                return null;
            }

            var projectile = AcquireShell();
            if (projectile == null)
            {
                return null;
            }

            projectile.Launch(command, definition, faction, this, damageResolver, impactPool, projectileId);
            if (projectileId > 0)
            {
                _authoritativeShells[projectileId] = projectile;
            }

            return projectile;
        }

        /// <summary>
        /// Applies one authoritative tick of projectile snapshots. Shells bound to
        /// authority projectiles are spawned, moved and retired here so the visible
        /// flight and impact timing always match the canonical match state.
        /// </summary>
        public void Reconcile(IReadOnlyList<DomainProjectileSnapshot> snapshots)
        {
            var liveIds = new HashSet<int>();
            if (snapshots != null)
            {
                for (var i = 0; i < snapshots.Count; i++)
                {
                    var snapshot = snapshots[i];
                    liveIds.Add(snapshot.ProjectileId);

                    if (!_authoritativeShells.TryGetValue(snapshot.ProjectileId, out var shell))
                    {
                        shell = AcquireShell();
                        if (shell == null) continue;
                        _authoritativeShells.Add(snapshot.ProjectileId, shell);
                        shell.LaunchAuthoritative(in snapshot, ProjectileHeight);
                    }
                    else
                    {
                        shell.SyncAuthoritative(in snapshot, ProjectileHeight);
                    }

                    if (!snapshot.IsActive)
                    {
                        RetireShell(shell, snapshot.DespawnReason, snapshot.Position,
                            IsSuccessfulHitReason(snapshot.DespawnReason));
                        _authoritativeShells.Remove(snapshot.ProjectileId);
                    }
                }
            }

            // Safety: retire shells whose authority ids vanished without a
            // terminal snapshot in this tick.
            if (_authoritativeShells.Count > 0)
            {
                _staleShellIds.Clear();
                foreach (var pair in _authoritativeShells)
                {
                    if (!liveIds.Contains(pair.Key))
                    {
                        _staleShellIds.Add(pair.Key);
                    }
                }

                for (var i = 0; i < _staleShellIds.Count; i++)
                {
                    var shell = _authoritativeShells[_staleShellIds[i]];
                    RetireShell(shell, ProjectileDespawnReason.PoolReset, default, false);
                    _authoritativeShells.Remove(_staleShellIds[i]);
                }
            }
        }

        private static bool IsSuccessfulHitReason(ProjectileDespawnReason reason) =>
            reason == ProjectileDespawnReason.HitActor ||
            reason == ProjectileDespawnReason.HitDecoy ||
            reason == ProjectileDespawnReason.HitStation;

        private CombatProjectile AcquireShell()
        {
            if (_available.Count == 0 && _createdCount >= maxCount)
            {
                return null;
            }

            ActiveCount++;
            return _available.Count > 0 ? _available.Pop() : CreateProjectile();
        }

        internal void Release(CombatProjectile projectile, ProjectileDespawnReason reason)
        {
            // Local-lab shells manage their own impact feedback before despawning.
            if (projectile == null)
            {
                return;
            }

            projectile.ResetProjectile();
            _available.Push(projectile);
            ActiveCount = Mathf.Max(0, ActiveCount - 1);
        }

        private void RetireShell(
            CombatProjectile projectile,
            ProjectileDespawnReason reason,
            Float2 position,
            bool successfulHit)
        {
            if (projectile == null)
            {
                return;
            }

            var shellId = projectile.ProjectileId;
            if (shellId > 0)
            {
                _authoritativeShells.Remove(shellId);
            }

            var worldPosition = new Vector3(position.X, ProjectileHeight, position.Y);
            if (successfulHit || reason == ProjectileDespawnReason.HitWall ||
                reason == ProjectileDespawnReason.Collision)
            {
                impactPool?.Play(worldPosition, successfulHit);
            }

            projectile.ResetProjectile();
            _available.Push(projectile);
            ActiveCount = Mathf.Max(0, ActiveCount - 1);
        }

        private CombatProjectile CreateProjectile()
        {
            var objectToPool = new GameObject("PooledProjectile", typeof(MeshFilter), typeof(MeshRenderer));
            objectToPool.transform.SetParent(transform, false);
            objectToPool.transform.localScale = Vector3.one * 0.28f;
            objectToPool.GetComponent<MeshFilter>().sharedMesh = PresentationMeshFactory.FacetedOrb("PooledProjectileOrb", 3, 12);
            var renderer = objectToPool.GetComponent<MeshRenderer>();
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
