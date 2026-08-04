using System;

namespace BattleRaja.Core.Domain
{
    public enum MatchPickupKind
    {
        Health = 0
    }

    public readonly struct MatchPickupDefinition
    {
        public MatchPickupDefinition(int pickupId, MatchPickupKind kind, int value, float respawnSeconds)
            : this(pickupId, kind, value, respawnSeconds, Float2.Zero, 1.2f)
        {
        }

        public MatchPickupDefinition(
            int pickupId,
            MatchPickupKind kind,
            int value,
            float respawnSeconds,
            Float2 position,
            float collectionRadius)
        {
            PickupId = pickupId;
            Kind = kind;
            Value = value;
            RespawnSeconds = respawnSeconds;
            Position = position;
            CollectionRadius = collectionRadius;
        }

        public int PickupId { get; }
        public MatchPickupKind Kind { get; }
        public int Value { get; }
        public float RespawnSeconds { get; }
        public Float2 Position { get; }
        public float CollectionRadius { get; }

        public bool IsValid(out string reason)
        {
            if (PickupId < 0 || Kind != MatchPickupKind.Health || Value <= 0 || RespawnSeconds <= 0f ||
                float.IsNaN(RespawnSeconds) || float.IsInfinity(RespawnSeconds) || CollectionRadius <= 0f ||
                float.IsNaN(CollectionRadius) || float.IsInfinity(CollectionRadius) ||
                float.IsNaN(Position.X) || float.IsInfinity(Position.X) ||
                float.IsNaN(Position.Y) || float.IsInfinity(Position.Y))
            {
                reason = "Pickup identity, health value and respawn duration must be valid.";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }

    public readonly struct MatchPickupCollectResult
    {
        public MatchPickupCollectResult(bool collected, int healAmount)
        {
            Collected = collected;
            HealAmount = healAmount;
        }

        public bool Collected { get; }
        public int HealAmount { get; }
    }

    public sealed class MatchPickupRuntime
    {
        private readonly MatchPickupDefinition _definition;
        private float _respawnRemaining;

        public MatchPickupRuntime(MatchPickupDefinition definition)
        {
            if (!definition.IsValid(out var reason)) throw new ArgumentException(reason, nameof(definition));
            _definition = definition;
        }

        public MatchPickupDefinition Definition => _definition;
        public bool IsAvailable => _respawnRemaining <= 0f;
        public float RespawnRemaining => Math.Max(0f, _respawnRemaining);

        public MatchPickupCollectResult TryCollect(int currentHealth, int maxHealth)
        {
            if (!IsAvailable || _definition.Kind != MatchPickupKind.Health || currentHealth < 0 || maxHealth <= currentHealth)
            {
                return new MatchPickupCollectResult(false, 0);
            }

            var healAmount = Math.Min(_definition.Value, maxHealth - currentHealth);
            if (healAmount <= 0) return new MatchPickupCollectResult(false, 0);
            _respawnRemaining = _definition.RespawnSeconds;
            return new MatchPickupCollectResult(true, healAmount);
        }

        public void Advance(float deltaSeconds)
        {
            if (deltaSeconds < 0f || float.IsNaN(deltaSeconds) || float.IsInfinity(deltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            _respawnRemaining = Math.Max(0f, _respawnRemaining - deltaSeconds);
        }
    }

    public readonly struct GadgetPickupDefinition
    {
        public GadgetPickupDefinition(int pickupId, ContentId gadgetId)
            : this(pickupId, gadgetId, Float2.Zero, 1.3f)
        {
        }

        public GadgetPickupDefinition(int pickupId, ContentId gadgetId, Float2 position, float collectionRadius)
        {
            PickupId = pickupId;
            GadgetId = gadgetId;
            Position = position;
            CollectionRadius = collectionRadius;
        }

        public int PickupId { get; }
        public ContentId GadgetId { get; }
        public Float2 Position { get; }
        public float CollectionRadius { get; }

        public bool IsValid(out string reason)
        {
            if (PickupId < 0 || !GadgetId.IsValid || GadgetId.Kind != ContentIdKind.Gadget || CollectionRadius <= 0f ||
                float.IsNaN(CollectionRadius) || float.IsInfinity(CollectionRadius) ||
                float.IsNaN(Position.X) || float.IsInfinity(Position.X) ||
                float.IsNaN(Position.Y) || float.IsInfinity(Position.Y))
            {
                reason = "Gadget pickup identity and gadget content ID must be valid.";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }

    public readonly struct GadgetPickupCollectResult
    {
        public GadgetPickupCollectResult(bool collected, ContentId gadgetId)
        {
            Collected = collected;
            GadgetId = gadgetId;
        }

        public bool Collected { get; }
        public ContentId GadgetId { get; }
    }

    public sealed class GadgetPickupRuntime
    {
        private readonly GadgetPickupDefinition _definition;
        private bool _available = true;

        public GadgetPickupRuntime(GadgetPickupDefinition definition)
        {
            if (!definition.IsValid(out var reason)) throw new ArgumentException(reason, nameof(definition));
            _definition = definition;
        }

        public GadgetPickupDefinition Definition => _definition;
        public bool IsAvailable => _available;

        public GadgetPickupCollectResult TryCollect(bool hasGadget)
        {
            if (!_available || hasGadget || !GadgetCatalog.TryGet(_definition.GadgetId, out _))
            {
                return new GadgetPickupCollectResult(false, default(ContentId));
            }

            _available = false;
            return new GadgetPickupCollectResult(true, _definition.GadgetId);
        }
    }
}
