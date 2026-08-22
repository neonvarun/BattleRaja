using System;

namespace BattleRaja.Core.Domain
{
    /// <summary>
    /// Transport-independent static collision contract for a match arena.
    /// Coordinates use the gameplay X/Z plane represented by Float2.X/Y.
    /// </summary>
    public readonly struct ArenaObstacle : IEquatable<ArenaObstacle>
    {
        public ArenaObstacle(int stableId, Float2 minimum, Float2 maximum)
        {
            if (stableId <= 0) throw new ArgumentOutOfRangeException(nameof(stableId));
            if (!minimum.IsFinite || !maximum.IsFinite || minimum.X >= maximum.X || minimum.Y >= maximum.Y)
            {
                throw new ArgumentException("An arena obstacle needs finite, strictly ordered bounds.");
            }

            StableId = stableId;
            Minimum = minimum;
            Maximum = maximum;
        }

        public int StableId { get; }
        public Float2 Minimum { get; }
        public Float2 Maximum { get; }

        public bool Contains(Float2 point, float radius = 0f)
        {
            var minX = Minimum.X - radius;
            var maxX = Maximum.X + radius;
            var minY = Minimum.Y - radius;
            var maxY = Maximum.Y + radius;
            return point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY;
        }

        public bool Equals(ArenaObstacle other) =>
            StableId == other.StableId && Minimum.Equals(other.Minimum) && Maximum.Equals(other.Maximum);

        public override bool Equals(object obj) => obj is ArenaObstacle other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(StableId, Minimum, Maximum);
    }

    /// <summary>
    /// Immutable arena bounds and stable obstacle order. Unity authoring code may
    /// construct this value from scene content, while Core never references Unity.
    /// </summary>
    public sealed class ArenaCollisionDefinition
    {
        public const string CurrentCollisionVersion = "1.0.0-bazaar";

        public ArenaCollisionDefinition(
            Float2 minimum,
            Float2 maximum,
            float actorRadius,
            ArenaObstacle[] obstacles = null,
            string collisionVersion = CurrentCollisionVersion)
        {
            if (!minimum.IsFinite || !maximum.IsFinite || minimum.X >= maximum.X || minimum.Y >= maximum.Y)
            {
                throw new ArgumentException("Arena bounds must be finite and strictly ordered.");
            }

            if (float.IsNaN(actorRadius) || float.IsInfinity(actorRadius) || actorRadius < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(actorRadius));
            }

            Minimum = minimum;
            Maximum = maximum;
            ActorRadius = actorRadius;
            CollisionVersion = string.IsNullOrWhiteSpace(collisionVersion) ? CurrentCollisionVersion : collisionVersion;
            _obstacles = CopyAndSort(obstacles);
        }

        private readonly ArenaObstacle[] _obstacles;
        public Float2 Minimum { get; }
        public Float2 Maximum { get; }
        public float ActorRadius { get; }
        public string CollisionVersion { get; }
        public int ObstacleCount => _obstacles.Length;

        public ArenaObstacle GetObstacle(int index) => _obstacles[index];

        /// <summary>
        /// Authored static obstacle geometry matching Bazaar Bastion arena.
        /// </summary>
        public static ArenaObstacle[] AuthoredBazaarObstacles => new[]
        {
            new ArenaObstacle(1, new Float2(-3.225f, -2.0f), new Float2(-2.775f, 8.0f)), // NarrowLaneWest
            new ArenaObstacle(2, new Float2(2.775f, -2.0f), new Float2(3.225f, 8.0f)),   // NarrowLaneEast
            new ArenaObstacle(3, new Float2(4.0f, 2.775f), new Float2(10.0f, 3.225f)),   // CornerWallHorizontal
            new ArenaObstacle(4, new Float2(9.775f, 3.0f), new Float2(10.225f, 9.0f)),   // CornerWallVertical
            new ArenaObstacle(5, new Float2(-9.0f, 4.0f), new Float2(-7.0f, 6.0f)),       // ObstacleNorthWest
            new ArenaObstacle(6, new Float2(-9.0f, -6.0f), new Float2(-7.0f, -4.0f)),     // ObstacleSouthWest
            new ArenaObstacle(7, new Float2(7.0f, -6.0f), new Float2(9.0f, -4.0f)),       // ObstacleSouthEast
            new ArenaObstacle(8, new Float2(-11.2f, 0.9f), new Float2(-8.8f, 2.1f)),     // BazaarStallWestCounter
            new ArenaObstacle(9, new Float2(8.8f, 0.9f), new Float2(11.2f, 2.1f)),      // BazaarStallEastCounter
            new ArenaObstacle(10, new Float2(-6.4f, 8.4f), new Float2(-5.6f, 9.2f)),     // BazaarArchLeft
            new ArenaObstacle(11, new Float2(5.6f, 8.4f), new Float2(6.4f, 9.2f))       // BazaarArchRight
        };

        /// <summary>
        /// Conservative Bazaar Bastion bounds and static obstacles.
        /// </summary>
        public static ArenaCollisionDefinition BazaarBastion => new ArenaCollisionDefinition(
            new Float2(-13.2f, -9.2f),
            new Float2(13.2f, 9.2f),
            0.45f,
            AuthoredBazaarObstacles);

        public bool IsPointBlocked(Float2 point, float radius = 0f)
        {
            if (!point.IsFinite) return true;
            var r = radius > 0f ? radius : ActorRadius;

            // Boundary comparisons tolerate a margin well beyond the collision
            // solver's own face epsilon (0.0001), because runtime floating-point
            // evaluation may carry extra intermediate precision. Every position
            // the solver can produce must classify as unblocked.
            const float boundaryEpsilon = 0.0005f;

            // Check arena outer bounds
            if (point.X < Minimum.X + r - boundaryEpsilon || point.X > Maximum.X - r + boundaryEpsilon ||
                point.Y < Minimum.Y + r - boundaryEpsilon || point.Y > Maximum.Y - r + boundaryEpsilon)
            {
                return true;
            }

            // Check obstacles. The margin shrinks the footprint so every
            // position the collision solver can produce (resting one solver
            // epsilon outside an expanded face) classifies as unblocked;
            // only points embedded meaningfully inside count as blocked.
            for (var i = 0; i < _obstacles.Length; i++)
            {
                var obstacle = _obstacles[i];
                if (point.X >= obstacle.Minimum.X - r + boundaryEpsilon &&
                    point.X <= obstacle.Maximum.X + r - boundaryEpsilon &&
                    point.Y >= obstacle.Minimum.Y - r + boundaryEpsilon &&
                    point.Y <= obstacle.Maximum.Y + r - boundaryEpsilon)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsLineOfSightClear(Float2 start, Float2 end)
        {
            if (!start.IsFinite || !end.IsFinite) return false;
            var delta = end - start;
            var maxDistance = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            if (maxDistance <= 0.0001f) return true;

            var dir = delta * (1f / maxDistance);
            return !Raycast(start, dir, maxDistance, out _, out _);
        }

        public bool Raycast(Float2 start, Float2 direction, float maxDistance, out Float2 hitPoint, out ArenaObstacle hitObstacle)
        {
            hitPoint = start + direction * maxDistance;
            hitObstacle = default;
            if (!start.IsFinite || !direction.IsFinite || maxDistance <= 0f) return false;

            var nearestT = maxDistance;
            var hitOccurred = false;

            // Check arena outer boundary walls with full slab entry/exit logic so
            // rays that start inside the inset play area (every projectile does)
            // register the exit crossing as the wall contact.
            var minX = Minimum.X + ActorRadius;
            var maxX = Maximum.X - ActorRadius;
            var minY = Minimum.Y + ActorRadius;
            var maxY = Maximum.Y - ActorRadius;

            var boundaryEnter = float.NegativeInfinity;
            var boundaryExit = float.PositiveInfinity;

            if (Math.Abs(direction.X) > 1e-6f)
            {
                var tx1 = (minX - start.X) / direction.X;
                var tx2 = (maxX - start.X) / direction.X;
                boundaryEnter = Math.Max(boundaryEnter, Math.Min(tx1, tx2));
                boundaryExit = Math.Min(boundaryExit, Math.Max(tx1, tx2));
            }
            else if (start.X < minX || start.X > maxX)
            {
                return false;
            }

            if (Math.Abs(direction.Y) > 1e-6f)
            {
                var ty1 = (minY - start.Y) / direction.Y;
                var ty2 = (maxY - start.Y) / direction.Y;
                boundaryEnter = Math.Max(boundaryEnter, Math.Min(ty1, ty2));
                boundaryExit = Math.Min(boundaryExit, Math.Max(ty1, ty2));
            }
            else if (start.Y < minY || start.Y > maxY)
            {
                return false;
            }

            if (boundaryExit >= Math.Max(boundaryEnter, 0f))
            {
                var boundaryDistance = boundaryEnter > 0f ? boundaryEnter : boundaryExit;
                if (boundaryDistance < nearestT)
                {
                    nearestT = boundaryDistance;
                    hitOccurred = true;
                }
            }

            // Check inner obstacles
            for (var i = 0; i < _obstacles.Length; i++)
            {
                var obs = _obstacles[i];
                if (IntersectRayAABB(start, direction, nearestT, obs.Minimum, obs.Maximum, out var tHit) && tHit < nearestT)
                {
                    nearestT = tHit;
                    hitObstacle = obs;
                    hitOccurred = true;
                }
            }

            if (hitOccurred)
            {
                hitPoint = start + direction * nearestT;
            }

            return hitOccurred;
        }

        private static bool IntersectRayAABB(Float2 start, Float2 dir, float maxDist, Float2 boxMin, Float2 boxMax, out float tHit)
        {
            tHit = maxDist;
            var tMinX = float.NegativeInfinity;
            var tMaxX = float.PositiveInfinity;
            if (Math.Abs(dir.X) > 1e-6f)
            {
                var tx1 = (boxMin.X - start.X) / dir.X;
                var tx2 = (boxMax.X - start.X) / dir.X;
                tMinX = Math.Min(tx1, tx2);
                tMaxX = Math.Max(tx1, tx2);
            }
            else if (start.X < boxMin.X || start.X > boxMax.X)
            {
                return false;
            }

            var tMinY = float.NegativeInfinity;
            var tMaxY = float.PositiveInfinity;
            if (Math.Abs(dir.Y) > 1e-6f)
            {
                var ty1 = (boxMin.Y - start.Y) / dir.Y;
                var ty2 = (boxMax.Y - start.Y) / dir.Y;
                tMinY = Math.Min(ty1, ty2);
                tMaxY = Math.Max(ty1, ty2);
            }
            else if (start.Y < boxMin.Y || start.Y > boxMax.Y)
            {
                return false;
            }

            var tEnter = Math.Max(tMinX, tMinY);
            var tExit = Math.Min(tMaxX, tMaxY);

            if (tEnter <= tExit && tExit >= 0f && tEnter < maxDist)
            {
                tHit = Math.Max(0f, tEnter);
                return true;
            }

            return false;
        }

        private static ArenaObstacle[] CopyAndSort(ArenaObstacle[] obstacles)
        {
            if (obstacles == null || obstacles.Length == 0) return Array.Empty<ArenaObstacle>();
            var copy = (ArenaObstacle[])obstacles.Clone();
            Array.Sort(copy, (left, right) => left.StableId.CompareTo(right.StableId));
            for (var i = 1; i < copy.Length; i++)
            {
                if (copy[i - 1].StableId == copy[i].StableId)
                {
                    throw new ArgumentException("Arena obstacle stable IDs must be unique.", nameof(obstacles));
                }
            }

            return copy;
        }
    }
}

