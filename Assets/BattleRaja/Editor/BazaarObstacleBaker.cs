using System;
using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Domain;
using UnityEditor;
using UnityEngine;

namespace BattleRaja.Editor
{
    /// <summary>
    /// Editor tool for auditing and baking scene obstacle colliders into deterministic
    /// ArenaCollisionDefinition values without hand-editing YAML.
    /// </summary>
    public static class BazaarObstacleBaker
    {
        [MenuItem("BattleRaja/Audit & Bake Bazaar Obstacles")]
        public static void BakeObstaclesFromCurrentScene()
        {
            var renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsSortMode.InstanceID);
            var obstacleList = new List<ArenaObstacle>();
            var nextId = 1;

            foreach (var r in renderers.OrderBy(r => r.gameObject.name))
            {
                var name = r.gameObject.name;
                if (name.IndexOf("Boundary", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("NarrowLane", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("CornerWall", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("Obstacle", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("Stall", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("Arch", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (name.IndexOf("Boundary", StringComparison.OrdinalIgnoreCase) >= 0) continue; // Boundary walls belong to arena bounds

                    var b = r.bounds;
                    var min = new Float2(b.min.x, b.min.z);
                    var max = new Float2(b.max.x, b.max.z);
                    obstacleList.Add(new ArenaObstacle(nextId++, min, max));
                }
            }

            Debug.Log($"[BazaarObstacleBaker] Discovered and baked {obstacleList.Count} obstacles from scene.");
            for (var i = 0; i < obstacleList.Count; i++)
            {
                var obs = obstacleList[i];
                Debug.Log($"Obstacle #{obs.StableId}: Min({obs.Minimum.X:F3}, {obs.Minimum.Y:F3}) Max({obs.Maximum.X:F3}, {obs.Maximum.Y:F3})");
            }

            var definition = new ArenaCollisionDefinition(
                new Float2(-13.2f, -9.2f),
                new Float2(13.2f, 9.2f),
                0.45f,
                obstacleList.ToArray());

            Debug.Log($"[BazaarObstacleBaker] ArenaCollisionDefinition constructed successfully with {definition.ObstacleCount} obstacles, Version {definition.CollisionVersion}.");
        }
    }
}
