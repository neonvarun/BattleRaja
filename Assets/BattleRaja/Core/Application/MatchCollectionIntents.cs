using System;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public readonly struct MatchPickupCollectionIntent
    {
        public MatchPickupCollectionIntent(int pickupId, CombatEntityId collectorId, int healAmount)
        {
            PickupId = pickupId;
            CollectorId = collectorId;
            HealAmount = healAmount;
        }

        public int PickupId { get; }
        public CombatEntityId CollectorId { get; }
        public int HealAmount { get; }
    }

    public readonly struct GadgetPickupCollectionIntent
    {
        public GadgetPickupCollectionIntent(int pickupId, CombatEntityId collectorId, ContentId gadgetId)
        {
            PickupId = pickupId;
            CollectorId = collectorId;
            GadgetId = gadgetId;
        }

        public int PickupId { get; }
        public CombatEntityId CollectorId { get; }
        public ContentId GadgetId { get; }
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
