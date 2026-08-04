using System.Collections;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Combat;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class GadgetPlayModeTests
    {
        [UnitySetUp]
        public IEnumerator LoadMovementLab()
        {
            yield return SceneManager.LoadSceneAsync("MovementLab", LoadSceneMode.Single);
            PlayModeTestHelpers.DisableBots();
            foreach (var pickup in Object.FindObjectsByType<GadgetPickup>(FindObjectsInactive.Include)) pickup.ResetPickup();
            yield return null;
        }

        [UnityTest]
        public IEnumerator GadgetPickupsAndOneSlotInventoryBootstrap()
        {
            var pickups = Object.FindObjectsByType<GadgetPickup>(FindObjectsInactive.Include);
            var user = PlayModeTestHelpers.FindPlayer<GadgetUser>();
            Assert.That(pickups, Has.Length.EqualTo(3));
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Inventory.Capacity, Is.EqualTo(1));
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerCanCollectAndUseDholBurst()
        {
            foreach (var bot in Object.FindObjectsByType<BotBrain>()) bot.enabled = false;
            var user = PlayModeTestHelpers.FindPlayer<GadgetUser>();
            var pickup = System.Linq.Enumerable.First(
                Object.FindObjectsByType<GadgetPickup>(),
                candidate => candidate.GadgetId.Equals(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(user.TryPickup(pickup.GadgetId), Is.True);
            Assert.That(user.UseHeld(), Is.True);
            Assert.That(user.HasGadget, Is.False);
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerCanCollectSpatialGadgetThroughMatchAuthority()
        {
            foreach (var bot in Object.FindObjectsByType<BotBrain>()) bot.enabled = false;
            var user = PlayModeTestHelpers.FindPlayer<GadgetUser>();
            var player = PlayModeTestHelpers.FindPlayer<MovementPlayerAgent>();
            var match = Object.FindAnyObjectByType<BattleRaja.Presentation.Match.OfflineMatchController>();
            var pickup = System.Linq.Enumerable.First(
                Object.FindObjectsByType<GadgetPickup>(),
                candidate => candidate.GadgetId.Equals(GadgetDefinition.DholBurst.GadgetId));

            player.ExternalCommandMode = true;
            pickup.transform.position = player.transform.position;
            match.StartMatch();
            yield return new WaitForSeconds(0.2f);

            Assert.That(user.HasGadget, Is.True,
                $"feedback={user.Feedback} player={player.transform.position} pickup={pickup.transform.position} active={pickup.IsAvailable}");
            Assert.That(user.HeldGadget, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(pickup.IsAvailable, Is.False);
            Assert.That(user.UseHeld(), Is.True);
            Assert.That(user.HasGadget, Is.False);
        }

        [UnityTest]
        public IEnumerator TiffinStationSpawnsAndExpiresAfterConfiguredLifetime()
        {
            foreach (var bot in Object.FindObjectsByType<BotBrain>()) bot.enabled = false;
            var user = PlayModeTestHelpers.FindPlayer<GadgetUser>();
            user.TryPickup(BattleRaja.Core.Domain.GadgetDefinition.TiffinStation.GadgetId);
            Assert.That(user.UseHeld(), Is.True);
            yield return new WaitForSeconds(0.1f);
            Assert.That(Object.FindObjectsByType<GadgetStation>(), Has.Length.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator TiffinStationDamageIsAcceptedThroughMatchAuthority()
        {
            foreach (var bot in Object.FindObjectsByType<BotBrain>()) bot.enabled = false;
            var user = PlayModeTestHelpers.FindPlayer<GadgetUser>();
            user.TryPickup(GadgetDefinition.TiffinStation.GadgetId);
            Assert.That(user.UseHeld(), Is.True);
            yield return null;

            var station = Object.FindAnyObjectByType<GadgetStation>();
            var target = station.GetComponent<CombatTarget>();
            var resolver = Object.FindAnyObjectByType<CombatDamageResolver>();
            var request = new DamageRequest(
                new CombatEntityId(2),
                target.Id,
                CombatFaction.Enemy,
                10,
                DamageType.Ability,
                Float2.Up,
                1);
            var result = resolver.Resolve(target, request, allowSelfHit: false, allowFriendlyFire: true, simulationTick: 1);

            Assert.That(result.Applied, Is.True);
            Assert.That(result.AmountApplied, Is.EqualTo(10));
            Assert.That(target.Health.Snapshot.CurrentHealth, Is.EqualTo(35));
        }
    }
}
