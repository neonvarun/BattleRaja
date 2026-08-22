using System;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public readonly struct MatchPickupCollectionIntent
    {
        public MatchPickupCollectionIntent(
            int pickupId,
            CombatEntityId collectorId,
            int healAmount,
            int collectionEventId = 0,
            int healingEventId = 0)
        {
            PickupId = pickupId;
            CollectorId = collectorId;
            HealAmount = healAmount;
            CollectionEventId = collectionEventId;
            HealingEventId = healingEventId;
        }

        public int PickupId { get; }
        public CombatEntityId CollectorId { get; }

        /// <summary>Canonical health amount actually applied by match authority.</summary>
        public int HealAmount { get; }

        /// <summary>Stable authority-assigned collection identity (0 while unassigned).</summary>
        public int CollectionEventId { get; }

        /// <summary>Stable authority-assigned healing identity for the pickup heal.</summary>
        public int HealingEventId { get; }
    }

    public readonly struct GadgetPickupCollectionIntent
    {
        public GadgetPickupCollectionIntent(
            int pickupId,
            CombatEntityId collectorId,
            ContentId gadgetId,
            int collectionEventId = 0)
        {
            PickupId = pickupId;
            CollectorId = collectorId;
            GadgetId = gadgetId;
            CollectionEventId = collectionEventId;
        }

        public int PickupId { get; }
        public CombatEntityId CollectorId { get; }
        public ContentId GadgetId { get; }

        /// <summary>Stable authority-assigned collection identity (0 while unassigned).</summary>
        public int CollectionEventId { get; }
    }

    public readonly struct MatchAuthorityCollections
    {
        public MatchAuthorityCollections(
            MatchPickupCollectionIntent[] pickupCollections,
            GadgetPickupCollectionIntent[] gadgetCollections)
        {
            PickupCollections = pickupCollections ?? Array.Empty<MatchPickupCollectionIntent>();
            GadgetCollections = gadgetCollections ?? Array.Empty<GadgetPickupCollectionIntent>();
        }

        public MatchPickupCollectionIntent[] PickupCollections { get; }
        public GadgetPickupCollectionIntent[] GadgetCollections { get; }
    }
}
