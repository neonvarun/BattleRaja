using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class AuthorityFoundationTests
    {
        [Test]
        public void MatchAuthorityEmitsZoneDamageIntentsOutsideTheCurrentZone()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja, 1f);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(0f, 0f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            authority.SetPosition(new CombatEntityId(1), new Float2(15f, 0f));

            authority.Advance(8f);
            var tick = authority.Advance(1f);

            Assert.That(tick.SimulationTick, Is.EqualTo(1));
            Assert.That(tick.Result.OutsideDamagePerSecond, Is.EqualTo(5));
            Assert.That(tick.OutsideDamageRequests, Has.Length.EqualTo(1));
            Assert.That(tick.OutsideDamageRequests[0].TargetId.Value, Is.EqualTo(1));
            Assert.That(tick.OutsideDamageRequests[0].DamageType, Is.EqualTo(DamageType.Aandhi));
            Assert.That(tick.OutsideDamageRequests[0].SimulationTick, Is.EqualTo(tick.SimulationTick));
        }

        [Test]
        public void MatchAuthorityOwnsPickupAvailabilityAndGadgetCollection()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja, 1f);
            authority.ConfigureItems(
                new[] { new MatchPickupDefinition(0, MatchPickupKind.Health, 25, 12f) },
                new[] { new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId) });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });

            var heal = authority.TryCollectPickup(0, 50, 100);
            Assert.That(heal.Collected, Is.True);
            Assert.That(heal.HealAmount, Is.EqualTo(25));
            Assert.That(authority.IsPickupAvailable(0), Is.False);
            Assert.That(authority.TryCollectPickup(0, 50, 100).Collected, Is.False);

            authority.Advance(12f);
            Assert.That(authority.IsPickupAvailable(0), Is.True);

            var gadget = authority.TryCollectGadget(0, false);
            Assert.That(gadget.Collected, Is.True);
            Assert.That(gadget.GadgetId, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(authority.IsGadgetPickupAvailable(0), Is.False);
            Assert.That(authority.TryCollectGadget(0, false).Collected, Is.False);
        }

        [Test]
        public void MatchAuthorityOwnsGadgetUseAndRejectsDuplicateCommands()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                null,
                new[] { new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId) });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            });

            Assert.That(authority.TryCollectGadget(new CombatEntityId(1), 0).Collected, Is.True);
            var command = new GadgetUseCommand(new CombatEntityId(1), GadgetDefinition.DholBurst.GadgetId, Float2.Zero, new Float2(1f, 0f), 1);
            var used = authority.TryUseGadget(command);
            var duplicate = authority.TryUseGadget(command);

            Assert.That(used.Used, Is.True);
            Assert.That(used.Effect.Kind, Is.EqualTo(GadgetEffectKind.DholBurst));
            Assert.That(used.Effect.Displacements, Has.Length.EqualTo(1));
            Assert.That(used.Effect.Displacements[0].TargetId.Value, Is.EqualTo(2));
            Assert.That(used.Effect.Displacements[0].Displacement.X, Is.EqualTo(0.32f).Within(0.0001f));
            Assert.That(duplicate.Used, Is.False);
            Assert.That(duplicate.Failure, Is.EqualTo(GadgetUseFailure.NotHeld));
        }

        [Test]
        public void AuthorityAdvancesGadgetCooldownOnAuthoritativeTicks()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });

            var gadgetId = GadgetDefinition.DholBurst.GadgetId;
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            var first = new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 1);
            Assert.That(authority.TryUseGadget(first).Used, Is.True);

            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            var blocked = authority.TryUseGadget(new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 2));
            Assert.That(blocked.Failure, Is.EqualTo(GadgetUseFailure.Cooldown));

            for (var tick = 1; tick <= 300; tick++) authority.Advance(tick, 1f / 30f);

            var second = authority.TryUseGadget(new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 301));
            Assert.That(second.Used, Is.True);
        }

        [Test]
        public void AuthoritySelectsDeterministicNearbyCollectorsAndOwnsRangeRules()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                new[]
                {
                    new MatchPickupDefinition(0, MatchPickupKind.Health, 25, 12f, Float2.Zero, 4f)
                },
                new[]
                {
                    new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId, Float2.Zero, 4f)
                });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            });
            authority.SyncHealth(new CombatEntityId(1), 50);
            authority.SyncHealth(new CombatEntityId(2), 50);

            var collections = authority.CollectNearby();

            Assert.That(collections.PickupCollections, Has.Length.EqualTo(1));
            Assert.That(collections.PickupCollections[0].CollectorId.Value, Is.EqualTo(1));
            Assert.That(collections.PickupCollections[0].HealAmount, Is.EqualTo(25));
            Assert.That(collections.GadgetCollections, Has.Length.EqualTo(1));
            Assert.That(collections.GadgetCollections[0].CollectorId.Value, Is.EqualTo(1));
            Assert.That(collections.GadgetCollections[0].GadgetId, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(authority.IsPickupAvailable(0), Is.False);
            Assert.That(authority.IsGadgetPickupAvailable(0), Is.False);
            Assert.That(authority.CollectNearby().PickupCollections, Is.Empty);
            Assert.That(authority.CollectNearby().GadgetCollections, Is.Empty);
        }

        [Test]
        public void AuthorityTicksTiffinHealingAndExpiry()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });
            authority.SyncHealth(new CombatEntityId(1), 50);
            var gadgetId = GadgetDefinition.TiffinStation.GadgetId;
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);

            var use = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                gadgetId,
                Float2.Zero,
                Float2.Up,
                1));
            Assert.That(use.Used, Is.True);
            Assert.That(use.Effect.StationId, Is.GreaterThan(0));

            var healed = false;
            var expired = false;
            for (var tick = 1; tick <= 300; tick++)
            {
                var result = authority.Advance(tick, 1f / 30f);
                healed |= result.GadgetHealingIntents.Any(intent => intent.TargetId.Value == 1 && intent.Amount == GadgetDefinition.TiffinStation.Magnitude);
                expired |= result.ExpiredStationIds.Contains(use.Effect.StationId);
            }

            Assert.That(healed, Is.True);
            Assert.That(expired, Is.True);
        }
    }
}
