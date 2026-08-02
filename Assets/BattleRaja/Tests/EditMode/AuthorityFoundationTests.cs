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
    }
}
