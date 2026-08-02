using System;

namespace BattleRaja.Core.Domain
{
    public enum FighterSpecialKind
    {
        ChargeThrow = 1,
        Decoy = 2
    }

    public readonly struct FighterSpecialDefinition
    {
        public FighterSpecialDefinition(ContentId abilityId, FighterSpecialKind kind, float cooldownSeconds, float durationSeconds, float radius, int magnitude)
        {
            AbilityId = abilityId;
            Kind = kind;
            CooldownSeconds = cooldownSeconds;
            DurationSeconds = durationSeconds;
            Radius = radius;
            Magnitude = magnitude;
        }

        public ContentId AbilityId { get; }
        public FighterSpecialKind Kind { get; }
        public float CooldownSeconds { get; }
        public float DurationSeconds { get; }
        public float Radius { get; }
        public int Magnitude { get; }

        public bool IsValid(out string reason)
        {
            if (!AbilityId.IsValid || AbilityId.Kind != ContentIdKind.Ability ||
                CooldownSeconds <= 0f || DurationSeconds <= 0f || Radius <= 0f || Magnitude <= 0)
            {
                reason = "Special definition values are invalid.";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public static FighterSpecialDefinition PehelChargeThrow => new FighterSpecialDefinition(
            ContentId.Ability("ability.pehel.charge_throw"), FighterSpecialKind.ChargeThrow, 6f, 0.35f, 2.2f, 3);

        public static FighterSpecialDefinition MayaDecoy => new FighterSpecialDefinition(
            ContentId.Ability("ability.maya.decoy"), FighterSpecialKind.Decoy, 9f, 4.5f, 0.5f, 35);
    }

    public sealed class DecoyRuntime
    {
        private float _remaining;
        private Float2 _position;

        public bool IsActive => _remaining > 0f;
        public Float2 Position => _position;

        public bool TrySpawn(Float2 position, FighterSpecialDefinition definition)
        {
            if (definition.Kind != FighterSpecialKind.Decoy || IsActive) return false;
            _position = position;
            _remaining = definition.DurationSeconds;
            return true;
        }

        public void Advance(float deltaSeconds, Float2 followPosition)
        {
            if (!IsActive) return;
            _remaining = Math.Max(0f, _remaining - Math.Max(0f, deltaSeconds));
            _position = _position + (followPosition - _position) * Math.Min(1f, Math.Max(0f, deltaSeconds) * 2f);
        }
    }
}
