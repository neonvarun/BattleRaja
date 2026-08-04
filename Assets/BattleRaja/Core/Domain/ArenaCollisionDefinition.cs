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
        public ArenaCollisionDefinition(
            Float2 minimum,
            Float2 maximum,
            float actorRadius,
            ArenaObstacle[] obstacles = null)
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
            _obstacles = CopyAndSort(obstacles);
        }

        private readonly ArenaObstacle[] _obstacles;
        public Float2 Minimum { get; }
        public Float2 Maximum { get; }
        public float ActorRadius { get; }
        public int ObstacleCount => _obstacles.Length;

        public ArenaObstacle GetObstacle(int index) => _obstacles[index];

        /// <summary>
        /// Conservative Bazaar Bastion bounds matching the current authored play
        /// volume. Obstacle data is deliberately empty until scene collision
        /// authoring is promoted into a stable content contract.
        /// </summary>
        public static ArenaCollisionDefinition BazaarBastion => new ArenaCollisionDefinition(
            new Float2(-13.2f, -9.2f),
            new Float2(13.2f, 9.2f),
            0.45f);

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
