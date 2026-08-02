using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct MovementTuning : IEquatable<MovementTuning>
    {
        public MovementTuning(
            float maxSpeed,
            float acceleration,
            float deceleration,
            float rotationSpeed,
            float movementDeadZone,
            float aimDeadZone,
            float inputSensitivity)
        {
            ValidatePositive(maxSpeed, nameof(maxSpeed));
            ValidatePositive(acceleration, nameof(acceleration));
            ValidatePositive(deceleration, nameof(deceleration));
            ValidatePositive(rotationSpeed, nameof(rotationSpeed));
            ValidateDeadZone(movementDeadZone, nameof(movementDeadZone));
            ValidateDeadZone(aimDeadZone, nameof(aimDeadZone));
            ValidatePositive(inputSensitivity, nameof(inputSensitivity));

            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Deceleration = deceleration;
            RotationSpeed = rotationSpeed;
            MovementDeadZone = movementDeadZone;
            AimDeadZone = aimDeadZone;
            InputSensitivity = inputSensitivity;
        }

        public float MaxSpeed { get; }
        public float Acceleration { get; }
        public float Deceleration { get; }
        public float RotationSpeed { get; }
        public float MovementDeadZone { get; }
        public float AimDeadZone { get; }
        public float InputSensitivity { get; }

        public static MovementTuning Default => new MovementTuning(
            maxSpeed: 5.5f,
            acceleration: 24f,
            deceleration: 30f,
            rotationSpeed: 720f,
            movementDeadZone: 0.12f,
            aimDeadZone: 0.14f,
            inputSensitivity: 1f);

        public bool Equals(MovementTuning other) =>
            MaxSpeed.Equals(other.MaxSpeed) &&
            Acceleration.Equals(other.Acceleration) &&
            Deceleration.Equals(other.Deceleration) &&
            RotationSpeed.Equals(other.RotationSpeed) &&
            MovementDeadZone.Equals(other.MovementDeadZone) &&
            AimDeadZone.Equals(other.AimDeadZone) &&
            InputSensitivity.Equals(other.InputSensitivity);

        public override bool Equals(object obj) => obj is MovementTuning other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(MaxSpeed, Acceleration, Deceleration, RotationSpeed, MovementDeadZone, AimDeadZone, InputSensitivity);

        private static void ValidatePositive(float value, string name)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || value <= 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "The value must be finite and greater than zero.");
            }
        }

        private static void ValidateDeadZone(float value, string name)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || value < 0f || value >= 1f)
            {
                throw new ArgumentOutOfRangeException(name, value, "The dead zone must be finite and in the range [0, 1).");
            }
        }
    }
}
