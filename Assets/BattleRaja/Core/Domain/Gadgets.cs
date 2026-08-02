using System;

namespace BattleRaja.Core.Domain
{
    public enum GadgetKind
    {
        UmbrellaGuard = 1,
        DholBurst = 2,
        TiffinStation = 3
    }

    public enum GadgetUseFailure
    {
        None = 0,
        InvalidDefinition = 1,
        NotHeld = 2,
        Cooldown = 3,
        InvalidDirection = 4,
        InvalidPlacement = 5
    }

    public readonly struct GadgetDefinition
    {
        public GadgetDefinition(
            ContentId gadgetId,
            GadgetKind kind,
            float cooldownSeconds,
            float durationSeconds,
            float radius,
            int magnitude,
            int stationHealth,
            float placementRadius)
        {
            GadgetId = gadgetId;
            Kind = kind;
            CooldownSeconds = cooldownSeconds;
            DurationSeconds = durationSeconds;
            Radius = radius;
            Magnitude = magnitude;
            StationHealth = stationHealth;
            PlacementRadius = placementRadius;
        }

        public ContentId GadgetId { get; }
        public GadgetKind Kind { get; }
        public float CooldownSeconds { get; }
        public float DurationSeconds { get; }
        public float Radius { get; }
        public int Magnitude { get; }
        public int StationHealth { get; }
        public float PlacementRadius { get; }

        public bool IsValid(out string reason)
        {
            if (!GadgetId.IsValid || GadgetId.Kind != ContentIdKind.Gadget)
            {
                reason = "Gadget ID must be a valid Gadget content ID.";
                return false;
            }

            if (CooldownSeconds <= 0f || DurationSeconds <= 0f || Radius <= 0f || Magnitude <= 0)
            {
                reason = "Cooldown, duration, radius and magnitude must be positive.";
                return false;
            }

            if (Kind == GadgetKind.TiffinStation && StationHealth <= 0)
            {
                reason = "Tiffin Station health must be positive.";
                return false;
            }

            if (Kind == GadgetKind.TiffinStation && (PlacementRadius <= 0f || PlacementRadius > Radius))
            {
                reason = "Tiffin Station placement radius must be positive and no larger than its effect radius.";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public static GadgetDefinition UmbrellaGuard => new GadgetDefinition(
            ContentId.Gadget("gadget.umbrella_guard"), GadgetKind.UmbrellaGuard, 12f, 3.5f, 1.6f, 70, 1, 0.1f);

        public static GadgetDefinition DholBurst => new GadgetDefinition(
            ContentId.Gadget("gadget.dhol_burst"), GadgetKind.DholBurst, 10f, 0.35f, 3.2f, 4, 1, 0.1f);

        public static GadgetDefinition TiffinStation => new GadgetDefinition(
            ContentId.Gadget("gadget.tiffin_station"), GadgetKind.TiffinStation, 16f, 10f, 2.4f, 18, 45, 0.8f);
    }

    public static class GadgetCatalog
    {
        private static readonly GadgetDefinition[] Definitions =
        {
            GadgetDefinition.UmbrellaGuard,
            GadgetDefinition.DholBurst,
            GadgetDefinition.TiffinStation
        };

        public static GadgetDefinition[] All => (GadgetDefinition[])Definitions.Clone();

        public static bool TryGet(ContentId id, out GadgetDefinition definition)
        {
            for (var i = 0; i < Definitions.Length; i++)
            {
                if (Definitions[i].GadgetId.Equals(id))
                {
                    definition = Definitions[i];
                    return true;
                }
            }

            definition = default(GadgetDefinition);
            return false;
        }
    }

    public sealed class GadgetInventory
    {
        private readonly int _capacity;
        private ContentId? _held;

        public GadgetInventory(int capacity = 1)
        {
            _capacity = Math.Max(1, capacity);
        }

        public int Capacity => _capacity;
        public bool HasGadget => _held.HasValue;
        public ContentId HeldGadget => _held ?? default(ContentId);

        public bool TryPickup(ContentId id, bool allowReplacement = false)
        {
            if (!GadgetCatalog.TryGet(id, out _)) return false;
            if (_held.HasValue && !allowReplacement) return false;
            _held = id;
            return true;
        }

        public bool TryConsume(ContentId id)
        {
            if (!_held.HasValue || !_held.Value.Equals(id)) return false;
            _held = null;
            return true;
        }

        public void Clear() => _held = null;
    }

    public readonly struct GadgetUseCommand
    {
        public GadgetUseCommand(CombatEntityId userId, ContentId gadgetId, Float2 origin, Float2 direction, int tick)
        {
            UserId = userId;
            GadgetId = gadgetId;
            Origin = origin;
            Direction = direction;
            Tick = tick;
        }

        public CombatEntityId UserId { get; }
        public ContentId GadgetId { get; }
        public Float2 Origin { get; }
        public Float2 Direction { get; }
        public int Tick { get; }
    }

    public enum GadgetEffectKind
    {
        None = 0,
        UmbrellaGuard = 1,
        DholBurst = 2,
        TiffinStation = 3
    }

    public readonly struct GadgetEffect
    {
        public GadgetEffect(GadgetEffectKind kind, GadgetDefinition definition, GadgetUseCommand command)
        {
            Kind = kind;
            Definition = definition;
            Command = command;
        }

        public GadgetEffectKind Kind { get; }
        public GadgetDefinition Definition { get; }
        public GadgetUseCommand Command { get; }
    }

    public readonly struct GadgetUseResult
    {
        public GadgetUseResult(bool used, GadgetUseFailure failure, GadgetEffect effect)
        {
            Used = used;
            Failure = failure;
            Effect = effect;
        }

        public bool Used { get; }
        public GadgetUseFailure Failure { get; }
        public GadgetEffect Effect { get; }
    }

    public sealed class GadgetRuntime
    {
        private float _cooldownRemaining;

        public float CooldownRemaining => _cooldownRemaining;
        public bool IsCoolingDown => _cooldownRemaining > 0f;

        public void Advance(float deltaSeconds)
        {
            _cooldownRemaining = Math.Max(0f, _cooldownRemaining - Math.Max(0f, deltaSeconds));
        }

        public GadgetUseResult TryUse(GadgetInventory inventory, GadgetUseCommand command)
        {
            if (!GadgetCatalog.TryGet(command.GadgetId, out var definition))
            {
                return new GadgetUseResult(false, GadgetUseFailure.InvalidDefinition, default(GadgetEffect));
            }

            if (!inventory.HasGadget || !inventory.HeldGadget.Equals(command.GadgetId))
            {
                return new GadgetUseResult(false, GadgetUseFailure.NotHeld, default(GadgetEffect));
            }

            if (IsCoolingDown)
            {
                return new GadgetUseResult(false, GadgetUseFailure.Cooldown, default(GadgetEffect));
            }

            var direction = command.Direction.Normalized;
            if (definition.Kind == GadgetKind.UmbrellaGuard && direction.SqrMagnitude <= 0.000001f)
            {
                return new GadgetUseResult(false, GadgetUseFailure.InvalidDirection, default(GadgetEffect));
            }

            if (definition.Kind == GadgetKind.TiffinStation && command.Origin.SqrMagnitude > 196f)
            {
                return new GadgetUseResult(false, GadgetUseFailure.InvalidPlacement, default(GadgetEffect));
            }

            if (!inventory.TryConsume(command.GadgetId))
            {
                return new GadgetUseResult(false, GadgetUseFailure.NotHeld, default(GadgetEffect));
            }

            ApplyAuthoritativeUse(definition);
            var effectKind = definition.Kind == GadgetKind.UmbrellaGuard
                ? GadgetEffectKind.UmbrellaGuard
                : definition.Kind == GadgetKind.DholBurst ? GadgetEffectKind.DholBurst : GadgetEffectKind.TiffinStation;
            return new GadgetUseResult(true, GadgetUseFailure.None, new GadgetEffect(effectKind, definition, command));
        }

        public void ApplyAuthoritativeUse(GadgetDefinition definition)
        {
            _cooldownRemaining = Math.Max(_cooldownRemaining, definition.CooldownSeconds);
        }
    }

    public static class GadgetSpawnRules
    {
        public static bool IsEligible(Float2 position, Float2[] existing, float minSeparation, float zoneRadius)
        {
            if (position.SqrMagnitude > zoneRadius * zoneRadius) return false;
            if (existing == null) return true;
            for (var i = 0; i < existing.Length; i++)
            {
                if (Float2.Distance(position, existing[i]) < minSeparation) return false;
            }

            return true;
        }

        public static ContentId Select(int seed, int index)
        {
            var value = Math.Abs(seed * 31 + index * 17) % 3;
            return value == 0 ? GadgetDefinition.UmbrellaGuard.GadgetId
                : value == 1 ? GadgetDefinition.DholBurst.GadgetId
                : GadgetDefinition.TiffinStation.GadgetId;
        }
    }
}
