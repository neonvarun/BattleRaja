using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct CollisionMoveResult
    {
        public CollisionMoveResult(Float2 position, Float2 displacement, bool collided, int iterations)
        {
            Position = position;
            AppliedDisplacement = displacement;
            Collided = collided;
            Iterations = iterations;
        }

        public Float2 Position { get; }
        public Float2 AppliedDisplacement { get; }
        public bool Collided { get; }
        public int Iterations { get; }
    }

    /// <summary>
    /// Deterministic axis-separated swept-circle movement. The solver handles the
    /// arena bounds and ordered axis-aligned obstacles, preserving the remaining
    /// component as a slide instead of letting Unity physics decide the result.
    /// </summary>
    public sealed class DeterministicCollisionSolver
    {
        private const float Epsilon = 0.0001f;
        private readonly ArenaCollisionDefinition _definition;

        public DeterministicCollisionSolver(ArenaCollisionDefinition definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        public CollisionMoveResult Move(Float2 start, Float2 displacement)
        {
            if (!start.IsFinite) throw new ArgumentException("Start position must be finite.", nameof(start));
            if (!displacement.IsFinite) throw new ArgumentException("Displacement must be finite.", nameof(displacement));

            var position = ClampToBounds(start, out var clampedStart);
            var collided = clampedStart;
            var iterations = clampedStart ? 1 : 0;

            var x = ResolveAxis(position, displacement.X, true, ref collided);
            position = new Float2(x, position.Y);
            var y = ResolveAxis(position, displacement.Y, false, ref collided);
            position = new Float2(position.X, y);
            position = ClampToBounds(position, out var clampedEnd);
            collided |= clampedEnd;
            if (clampedEnd) iterations++;

            return new CollisionMoveResult(position, position - start, collided, iterations);
        }

        private float ResolveAxis(Float2 position, float delta, bool horizontal, ref bool collided)
        {
            if (Math.Abs(delta) <= Epsilon) return horizontal ? position.X : position.Y;

            var candidate = horizontal ? position.X + delta : position.Y + delta;
            var minimum = horizontal ? _definition.Minimum.X + _definition.ActorRadius : _definition.Minimum.Y + _definition.ActorRadius;
            var maximum = horizontal ? _definition.Maximum.X - _definition.ActorRadius : _definition.Maximum.Y - _definition.ActorRadius;
            if (candidate < minimum)
            {
                candidate = minimum;
                collided = true;
            }
            else if (candidate > maximum)
            {
                candidate = maximum;
                collided = true;
            }

            for (var i = 0; i < _definition.Obstacles.Length; i++)
            {
                var obstacle = _definition.Obstacles[i];
                var crossAxis = horizontal ? position.Y : position.X;
                var crossMinimum = (horizontal ? obstacle.Minimum.Y : obstacle.Minimum.X) - _definition.ActorRadius;
                var crossMaximum = (horizontal ? obstacle.Maximum.Y : obstacle.Maximum.X) + _definition.ActorRadius;
                if (crossAxis < crossMinimum - Epsilon || crossAxis > crossMaximum + Epsilon) continue;

                var obstacleMinimum = (horizontal ? obstacle.Minimum.X : obstacle.Minimum.Y) - _definition.ActorRadius;
                var obstacleMaximum = (horizontal ? obstacle.Maximum.X : obstacle.Maximum.Y) + _definition.ActorRadius;
                var current = horizontal ? position.X : position.Y;
                if (delta > 0f && current <= obstacleMinimum + Epsilon && candidate >= obstacleMinimum)
                {
                    candidate = Math.Min(candidate, obstacleMinimum - Epsilon);
                    collided = true;
                }
                else if (delta < 0f && current >= obstacleMaximum - Epsilon && candidate <= obstacleMaximum)
                {
                    candidate = Math.Max(candidate, obstacleMaximum + Epsilon);
                    collided = true;
                }
            }

            return Math.Max(minimum, Math.Min(maximum, candidate));
        }

        private Float2 ClampToBounds(Float2 position, out bool clamped)
        {
            var minimumX = _definition.Minimum.X + _definition.ActorRadius;
            var maximumX = _definition.Maximum.X - _definition.ActorRadius;
            var minimumY = _definition.Minimum.Y + _definition.ActorRadius;
            var maximumY = _definition.Maximum.Y - _definition.ActorRadius;
            var x = Math.Max(minimumX, Math.Min(maximumX, position.X));
            var y = Math.Max(minimumY, Math.Min(maximumY, position.Y));
            clamped = Math.Abs(x - position.X) > Epsilon || Math.Abs(y - position.Y) > Epsilon;
            return new Float2(x, y);
        }
    }
}
