using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct MovementStep
    {
        public MovementStep(Float2 velocity, Float2 displacement, Float2 aimDirection)
        {
            Velocity = velocity;
            Displacement = displacement;
            AimDirection = aimDirection;
        }

        public Float2 Velocity { get; }
        public Float2 Displacement { get; }
        public Float2 AimDirection { get; }
    }

    public sealed class MovementMotor
    {
        private Float2 _velocity;
        private Float2 _aimDirection = Float2.Up;

        public Float2 Velocity => _velocity;
        public Float2 AimDirection => _aimDirection;

        public MovementStep Step(MovementCommand command, float deltaSeconds, MovementTuning tuning)
        {
            if (float.IsNaN(deltaSeconds) || float.IsInfinity(deltaSeconds) || deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            var desiredVelocity = Float2.ClampMagnitude(command.Movement, 1f) * tuning.MaxSpeed;
            var rate = desiredVelocity.SqrMagnitude > _velocity.SqrMagnitude
                ? tuning.Acceleration
                : tuning.Deceleration;
            _velocity = MoveTowards(_velocity, desiredVelocity, rate * deltaSeconds);

            if (command.Aim.SqrMagnitude > 0.000001f)
            {
                _aimDirection = command.Aim.Normalized;
            }

            return new MovementStep(_velocity, _velocity * deltaSeconds, _aimDirection);
        }

        public void Reset(Float2 aimDirection)
        {
            _velocity = Float2.Zero;
            _aimDirection = aimDirection.SqrMagnitude > 0.000001f ? aimDirection.Normalized : Float2.Up;
        }

        /// <summary>
        /// Applies a state snapshot produced by an authority-owned movement step.
        /// Presentation adapters use this to keep their local motor aligned with the
        /// canonical simulation without re-running movement rules a second time.
        /// </summary>
        public void ApplyAuthoritativeState(Float2 velocity, Float2 aimDirection)
        {
            _velocity = velocity;
            _aimDirection = aimDirection.SqrMagnitude > 0.000001f ? aimDirection.Normalized : Float2.Up;
        }

        private static Float2 MoveTowards(Float2 current, Float2 target, float maxDelta)
        {
            var distance = Float2.Distance(current, target);
            if (distance <= maxDelta || distance <= 0.000001f)
            {
                return target;
            }

            return current + ((target - current) * (maxDelta / distance));
        }
    }
}
