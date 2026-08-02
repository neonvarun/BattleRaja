using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct CombatEntityId : IEquatable<CombatEntityId>
    {
        public CombatEntityId(int value) => Value = value;

        public int Value { get; }

        public bool Equals(CombatEntityId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is CombatEntityId other && Equals(other);
        public override int GetHashCode() => Value;
        public override string ToString() => Value.ToString();

        public static bool operator ==(CombatEntityId left, CombatEntityId right) => left.Equals(right);
        public static bool operator !=(CombatEntityId left, CombatEntityId right) => !left.Equals(right);
    }
}
