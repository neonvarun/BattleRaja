using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct AimAssistCandidate
    {
        public AimAssistCandidate(CombatEntityId id, Float2 position)
        {
            Id = id;
            Position = position;
        }

        public CombatEntityId Id { get; }
        public Float2 Position { get; }
    }

    /// <summary>
    /// Selects a nearby target inside a bounded aim cone without changing the
    /// player's intent outside that cone. This is an input convenience, not an
    /// authoritative hit or damage decision.
    /// </summary>
    public static class AimAssistTargeting
    {
        public static bool TryAssist(
            Float2 origin,
            Float2 inputDirection,
            AimAssistCandidate[] candidates,
            int candidateCount,
            float maxRange,
            float coneDegrees,
            out Float2 assistedDirection)
        {
            var requestedDirection = inputDirection.Normalized;
            assistedDirection = requestedDirection;
            if (candidates == null || candidateCount <= 0 || maxRange <= 0f ||
                float.IsNaN(maxRange) || float.IsInfinity(maxRange) ||
                coneDegrees <= 0f || coneDegrees >= 180f ||
                float.IsNaN(coneDegrees) || float.IsInfinity(coneDegrees) ||
                requestedDirection.SqrMagnitude <= 0.000001f)
            {
                return false;
            }

            var count = Math.Min(candidateCount, candidates.Length);
            var rangeSquared = maxRange * maxRange;
            var minimumDot = MathF.Cos(coneDegrees * (MathF.PI / 180f));
            const float comparisonEpsilon = 0.0001f;
            var bestDotScore = int.MinValue;
            var bestDistance = float.MaxValue;
            var bestId = int.MaxValue;
            var found = false;

            for (var i = 0; i < count; i++)
            {
                var candidate = candidates[i];
                var delta = candidate.Position - origin;
                var distanceSquared = delta.SqrMagnitude;
                if (distanceSquared <= 0.000001f || distanceSquared > rangeSquared) continue;

                var direction = delta / MathF.Sqrt(distanceSquared);
                var dot = (requestedDirection.X * direction.X) + (requestedDirection.Y * direction.Y);
                if (dot < minimumDot) continue;

                // Quantize the angular score before tie-breaking so symmetric
                // candidates do not depend on platform-specific float rounding.
                var dotScore = (int)MathF.Round(dot * 10000f);
                var isBetter = !found || dotScore > bestDotScore ||
                    (dotScore == bestDotScore &&
                     (distanceSquared < bestDistance - comparisonEpsilon ||
                      (MathF.Abs(distanceSquared - bestDistance) <= comparisonEpsilon && candidate.Id.Value < bestId)));
                if (!isBetter) continue;

                found = true;
                bestDotScore = dotScore;
                bestDistance = distanceSquared;
                bestId = candidate.Id.Value;
                assistedDirection = direction;
            }

            return found;
        }
    }
}
