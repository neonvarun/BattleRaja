using System;

namespace BattleRaja.Core.Domain
{
    public enum ContentIdKind
    {
        Fighter = 1,
        Attack = 2,
        Ability = 3
    }

    public readonly struct ContentId : IEquatable<ContentId>
    {
        public ContentId(ContentIdKind kind, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("A content ID must not be empty.", nameof(value));
            }

            Kind = kind;
            Value = value.Trim();
        }

        public ContentIdKind Kind { get; }
        public string Value { get; }
        public bool IsValid => !string.IsNullOrWhiteSpace(Value);

        public bool Equals(ContentId other) => Kind == other.Kind && string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is ContentId other && Equals(other);
        public override int GetHashCode() => HashCode.Combine((int)Kind, Value);
        public override string ToString() => Value ?? string.Empty;

        public static ContentId Fighter(string value) => new ContentId(ContentIdKind.Fighter, value);
        public static ContentId Attack(string value) => new ContentId(ContentIdKind.Attack, value);
        public static ContentId Ability(string value) => new ContentId(ContentIdKind.Ability, value);
    }
}
