using System;
using System.Collections.Generic;

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

    public readonly struct GadgetDisplacementIntent
    {
        public GadgetDisplacementIntent(CombatEntityId targetId, Float2 displacement)
        {
            TargetId = targetId;
            Displacement = displacement;
        }

        public CombatEntityId TargetId { get; }
        public Float2 Displacement { get; }
    }

    public readonly struct GadgetEffect
    {
        public GadgetEffect(GadgetEffectKind kind, GadgetDefinition definition, GadgetUseCommand command)
            : this(kind, definition, command, Array.Empty<GadgetDisplacementIntent>())
        {
        }

        public GadgetEffect(
            GadgetEffectKind kind,
            GadgetDefinition definition,
            GadgetUseCommand command,
            GadgetDisplacementIntent[] displacements)
            : this(kind, definition, command, displacements, -1)
        {
        }

        public GadgetEffect(
            GadgetEffectKind kind,
            GadgetDefinition definition,
            GadgetUseCommand command,
            GadgetDisplacementIntent[] displacements,
            int stationId)
        {
            Kind = kind;
            Definition = definition;
            Command = command;
            Displacements = displacements ?? Array.Empty<GadgetDisplacementIntent>();
            StationId = stationId;
        }

        public GadgetEffectKind Kind { get; }
        public GadgetDefinition Definition { get; }
        public GadgetUseCommand Command { get; }
        public GadgetDisplacementIntent[] Displacements { get; }
        public int StationId { get; }
    }

    public readonly struct GadgetHealingIntent
    {
        public GadgetHealingIntent(
            int stationId,
            CombatEntityId targetId,
            int amount,
            int eventId = 0,
            CombatEntityId healerId = default(CombatEntityId))
        {
            StationId = stationId;
            TargetId = targetId;
            Amount = amount;
            EventId = eventId;
            HealerId = healerId;
        }

        public int StationId { get; }
        public CombatEntityId TargetId { get; }

        /// <summary>
        /// Canonical health amount actually applied by match authority. Intent
        /// emitters before atomic resolution may carry the requested amount;
        /// authoritative tick results always carry the applied amount.
        /// </summary>
        public int Amount { get; }

        /// <summary>
        /// Stable authority-assigned healing identity (0 while unassigned).
        /// Assigned only after canonical validation/application succeeds.
        /// </summary>
        public int EventId { get; }

        /// <summary>
        /// Authority-owned actor that created the station. A zero ID means the
        /// source is environmental/legacy and the target receives self-heal
        /// credit when the Bastion adapter mirrors the event.
        /// </summary>
        public CombatEntityId HealerId { get; }
    }

    public readonly struct GadgetStationDamageResult
    {
        public GadgetStationDamageResult(bool applied, int amountApplied, bool destroyed, int currentHealth)
        {
            Applied = applied;
            AmountApplied = amountApplied;
            Destroyed = destroyed;
            CurrentHealth = currentHealth;
        }

        public bool Applied { get; }
        public int AmountApplied { get; }
        public bool Destroyed { get; }
        public int CurrentHealth { get; }
    }

    public readonly struct GadgetStationStep
    {
        public GadgetStationStep(int stationId, GadgetHealingIntent[] healing, bool expired)
        {
            StationId = stationId;
            Healing = healing ?? Array.Empty<GadgetHealingIntent>();
            Expired = expired;
        }

        public int StationId { get; }
        public GadgetHealingIntent[] Healing { get; }
        public bool Expired { get; }
    }

    public sealed class GadgetStationRuntime
    {
        private const float HealIntervalSeconds = 1f;
        private readonly GadgetDefinition _definition;
        private float _remaining;
        private float _healAccumulator;
        private int _health;

        public GadgetStationRuntime(
            int stationId,
            Float2 position,
            GadgetDefinition definition,
            CombatEntityId ownerId = default(CombatEntityId))
        {
            if (stationId <= 0 || definition.Kind != GadgetKind.TiffinStation || !definition.IsValid(out var reason))
            {
                throw new ArgumentException("A Tiffin station definition and positive station ID are required.", nameof(definition));
            }

            StationId = stationId;
            Position = position;
            _definition = definition;
            OwnerId = ownerId;
            _remaining = definition.DurationSeconds;
            _health = definition.StationHealth;
        }

        public int StationId { get; }
        public CombatEntityId OwnerId { get; }
        public Float2 Position { get; }
        public float RemainingSeconds => Math.Max(0f, _remaining);
        public float HealAccumulator => _healAccumulator;
        public int CurrentHealth => Math.Max(0, _health);
        public bool IsActive => _remaining > 0f && _health > 0;

        public GadgetStationStep Advance(float deltaSeconds, MatchParticipantSnapshot[] snapshots)
        {
            if (deltaSeconds < 0f || float.IsNaN(deltaSeconds) || float.IsInfinity(deltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (!IsActive)
            {
                return new GadgetStationStep(StationId, Array.Empty<GadgetHealingIntent>(), true);
            }

            _remaining = Math.Max(0f, _remaining - deltaSeconds);
            _healAccumulator += deltaSeconds;
            var healing = new List<GadgetHealingIntent>();
            while (_healAccumulator + 0.000001f >= HealIntervalSeconds && IsActive)
            {
                _healAccumulator -= HealIntervalSeconds;
                if (snapshots == null) continue;
                var radiusSquared = _definition.Radius * _definition.Radius;
                for (var i = 0; i < snapshots.Length; i++)
                {
                    var snapshot = snapshots[i];
                    if (!snapshot.Alive || snapshot.CurrentHealth >= snapshot.MaxHealth ||
                        snapshot.Position.SqrMagnitudeFrom(Position) > radiusSquared) continue;
                    healing.Add(new GadgetHealingIntent(
                        StationId,
                        snapshot.Id,
                        _definition.Magnitude,
                        0,
                        OwnerId));
                }
            }

            return new GadgetStationStep(StationId, healing.ToArray(), _remaining <= 0f || _health <= 0);
        }

        public bool TryDamage(int amount)
        {
            if (!IsActive || amount <= 0) return false;
            _health = Math.Max(0, _health - amount);
            return true;
        }
    }

    public sealed class UmbrellaGuardRuntime
    {
        private float _remaining;
        private Float2 _direction = Float2.Up;

        public float RemainingSeconds => Math.Max(0f, _remaining);
        public Float2 Direction => _direction;
        public bool IsActive => _remaining > 0f;

        public void Activate(GadgetDefinition definition, Float2 direction)
        {
            if (definition.Kind != GadgetKind.UmbrellaGuard || !definition.IsValid(out var reason) ||
                direction.SqrMagnitude <= 0.000001f)
            {
                throw new ArgumentException("A valid Umbrella Guard definition and direction are required.", nameof(definition));
            }

            _direction = direction.Normalized;
            _remaining = definition.DurationSeconds;
        }

        public void Advance(float deltaSeconds)
        {
            if (deltaSeconds < 0f || float.IsNaN(deltaSeconds) || float.IsInfinity(deltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            _remaining = Math.Max(0f, _remaining - deltaSeconds);
        }

        public int Mitigate(DamageRequest request)
        {
            if (!IsActive || request.DamageType == DamageType.Aandhi || request.DamageType == DamageType.Generic)
            {
                return request.RawAmount;
            }

            var incoming = request.HitDirection.SqrMagnitude > 0.000001f
                ? request.HitDirection.Normalized * -1f
                : _direction;
            var dot = _direction.X * incoming.X + _direction.Y * incoming.Y;
            if (dot < 0.15f) return request.RawAmount;
            return Math.Max(1, (int)MathF.Ceiling((request.RawAmount * 0.30f) - 0.0001f));
        }
    }

    public readonly struct GadgetUseResult
    {
        public GadgetUseResult(bool used, GadgetUseFailure failure, GadgetEffect effect, int eventId = 0)
        {
            Used = used;
            Failure = failure;
            Effect = effect;
            EventId = eventId;
        }

        public bool Used { get; }
        public GadgetUseFailure Failure { get; }
        public GadgetEffect Effect { get; }

        /// <summary>
        /// Stable authority-assigned gadget-use identity (0 while unassigned
        /// or rejected). Rejected/cooldown attempts never consume an identity.
        /// </summary>
        public int EventId { get; }
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
