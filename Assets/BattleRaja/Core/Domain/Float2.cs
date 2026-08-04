using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct Float2 : IEquatable<Float2>
    {
        public Float2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }
        public bool IsFinite => !float.IsNaN(X) && !float.IsInfinity(X) && !float.IsNaN(Y) && !float.IsInfinity(Y);
        public float SqrMagnitude => (X * X) + (Y * Y);
        public float Magnitude => MathF.Sqrt(SqrMagnitude);

        public float SqrMagnitudeFrom(Float2 origin)
        {
            var delta = this - origin;
            return delta.SqrMagnitude;
        }

        public Float2 Normalized
        {
            get
            {
                var magnitude = Magnitude;
                return magnitude > 0.000001f ? this / magnitude : Zero;
            }
        }

        public bool Equals(Float2 other) => X.Equals(other.X) && Y.Equals(other.Y);
        public override bool Equals(object obj) => obj is Float2 other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"({X}, {Y})";

        public static Float2 Zero => new Float2(0f, 0f);
        public static Float2 Up => new Float2(0f, 1f);

        public static Float2 operator +(Float2 left, Float2 right) => new Float2(left.X + right.X, left.Y + right.Y);
        public static Float2 operator -(Float2 left, Float2 right) => new Float2(left.X - right.X, left.Y - right.Y);
        public static Float2 operator *(Float2 value, float scalar) => new Float2(value.X * scalar, value.Y * scalar);
        public static Float2 operator /(Float2 value, float scalar) => new Float2(value.X / scalar, value.Y / scalar);

        public static Float2 ClampMagnitude(Float2 value, float maxMagnitude)
        {
            if (maxMagnitude < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(maxMagnitude));
            }

            var magnitude = value.Magnitude;
            return magnitude > maxMagnitude && magnitude > 0.000001f
                ? value * (maxMagnitude / magnitude)
                : value;
        }

        public static float Distance(Float2 left, Float2 right) => (left - right).Magnitude;
    }
}
