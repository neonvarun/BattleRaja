using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class GadgetFoundationTests
    {
        [Test]
        public void CatalogDefinitionsHaveStableValidIds()
        {
            var definitions = GadgetCatalog.All;
            Assert.That(definitions, Has.Length.EqualTo(3));
            for (var i = 0; i < definitions.Length; i++)
            {
                Assert.That(definitions[i].IsValid(out var reason), Is.True, reason);
                Assert.That(definitions[i].GadgetId.Kind, Is.EqualTo(ContentIdKind.Gadget));
            }
        }

        [Test]
        public void InventoryHasOneSlotAndRejectsSecondPickup()
        {
            var inventory = new GadgetInventory();
            Assert.That(inventory.TryPickup(GadgetDefinition.UmbrellaGuard.GadgetId), Is.True);
            Assert.That(inventory.TryPickup(GadgetDefinition.DholBurst.GadgetId), Is.False);
            Assert.That(inventory.HeldGadget, Is.EqualTo(GadgetDefinition.UmbrellaGuard.GadgetId));
            Assert.That(inventory.TryPickup(GadgetDefinition.DholBurst.GadgetId, true), Is.True);
        }

        [Test]
        public void RuntimeConsumesHeldGadgetAndHonorsCooldown()
        {
            var inventory = new GadgetInventory();
            inventory.TryPickup(GadgetDefinition.DholBurst.GadgetId);
            var runtime = new GadgetRuntime();
            var command = new GadgetUseCommand(new CombatEntityId(1), GadgetDefinition.DholBurst.GadgetId, Float2.Zero, Float2.Up, 1);
            var used = runtime.TryUse(inventory, command);
            Assert.That(used.Used, Is.True);
            Assert.That(inventory.HasGadget, Is.False);
            Assert.That(runtime.TryUse(inventory, command).Failure, Is.EqualTo(GadgetUseFailure.NotHeld));
        }

        [Test]
        public void UmbrellaRequiresFacingAndTiffinRejectsUnsafePlacement()
        {
            var inventory = new GadgetInventory();
            inventory.TryPickup(GadgetDefinition.UmbrellaGuard.GadgetId);
            var runtime = new GadgetRuntime();
            var invalid = runtime.TryUse(inventory, new GadgetUseCommand(new CombatEntityId(1), GadgetDefinition.UmbrellaGuard.GadgetId, Float2.Zero, Float2.Zero, 1));
            Assert.That(invalid.Failure, Is.EqualTo(GadgetUseFailure.InvalidDirection));

            inventory.TryPickup(GadgetDefinition.TiffinStation.GadgetId, true);
            runtime.Advance(20f);
            var unsafeUse = runtime.TryUse(inventory, new GadgetUseCommand(new CombatEntityId(1), GadgetDefinition.TiffinStation.GadgetId, new Float2(20f, 0f), Float2.Up, 2));
            Assert.That(unsafeUse.Failure, Is.EqualTo(GadgetUseFailure.InvalidPlacement));
        }

        [Test]
        public void SpawnRulesKeepPickupsInZoneAndSeparated()
        {
            var existing = new[] { Float2.Zero };
            Assert.That(GadgetSpawnRules.IsEligible(new Float2(3f, 0f), existing, 2f, 5f), Is.True);
            Assert.That(GadgetSpawnRules.IsEligible(new Float2(1f, 0f), existing, 2f, 5f), Is.False);
            Assert.That(GadgetSpawnRules.IsEligible(new Float2(6f, 0f), existing, 2f, 5f), Is.False);
            Assert.That(GadgetSpawnRules.Select(10, 0).Kind, Is.EqualTo(ContentIdKind.Gadget));
        }

        [Test]
        public void StationHealingAndDholMagnitudeRemainBounded()
        {
            Assert.That(GadgetDefinition.TiffinStation.Magnitude, Is.LessThanOrEqualTo(25));
            Assert.That(GadgetDefinition.DholBurst.Magnitude, Is.LessThanOrEqualTo(6));
            Assert.That(GadgetDefinition.TiffinStation.DurationSeconds, Is.LessThanOrEqualTo(12f));
        }
    }
}
