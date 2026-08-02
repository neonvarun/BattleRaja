using System.Collections.Generic;
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

            Assert.That(tick.Result.OutsideDamagePerSecond, Is.EqualTo(5));
            Assert.That(tick.OutsideDamageRequests, Has.Length.EqualTo(1));
            Assert.That(tick.OutsideDamageRequests[0].TargetId.Value, Is.EqualTo(1));
            Assert.That(tick.OutsideDamageRequests[0].DamageType, Is.EqualTo(DamageType.Aandhi));
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
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });

            Assert.That(authority.TryCollectGadget(new CombatEntityId(1), 0).Collected, Is.True);
            var command = new GadgetUseCommand(new CombatEntityId(1), GadgetDefinition.DholBurst.GadgetId, Float2.Zero, new Float2(1f, 0f), 1);
            var used = authority.TryUseGadget(command);
            var duplicate = authority.TryUseGadget(command);

            Assert.That(used.Used, Is.True);
            Assert.That(used.Effect.Kind, Is.EqualTo(GadgetEffectKind.DholBurst));
            Assert.That(duplicate.Used, Is.False);
            Assert.That(duplicate.Failure, Is.EqualTo(GadgetUseFailure.NotHeld));
        }
    }
}
