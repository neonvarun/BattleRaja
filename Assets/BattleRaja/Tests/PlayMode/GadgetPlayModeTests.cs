using System.Collections;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.AI;
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
            foreach (var pickup in Object.FindObjectsByType<GadgetPickup>(FindObjectsInactive.Include, FindObjectsSortMode.None)) pickup.ResetPickup();
            yield return null;
        }

        [UnityTest]
        public IEnumerator GadgetPickupsAndOneSlotInventoryBootstrap()
        {
            var pickups = Object.FindObjectsByType<GadgetPickup>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var user = PlayModeTestHelpers.FindPlayer<GadgetUser>();
            Assert.That(pickups, Has.Length.EqualTo(3));
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Inventory.Capacity, Is.EqualTo(1));
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerCanCollectAndUseDholBurst()
        {
            foreach (var bot in Object.FindObjectsByType<BotBrain>(FindObjectsSortMode.None)) bot.enabled = false;
            var user = PlayModeTestHelpers.FindPlayer<GadgetUser>();
            var pickup = Object.FindObjectsByType<GadgetPickup>(FindObjectsSortMode.None)[1];
            Assert.That(user.TryPickup(pickup.GadgetId), Is.True);
            Assert.That(user.UseHeld(), Is.True);
            Assert.That(user.HasGadget, Is.False);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TiffinStationSpawnsAndExpiresAfterConfiguredLifetime()
        {
            foreach (var bot in Object.FindObjectsByType<BotBrain>(FindObjectsSortMode.None)) bot.enabled = false;
            var user = PlayModeTestHelpers.FindPlayer<GadgetUser>();
            user.TryPickup(BattleRaja.Core.Domain.GadgetDefinition.TiffinStation.GadgetId);
            Assert.That(user.UseHeld(), Is.True);
            yield return new WaitForSeconds(0.1f);
            Assert.That(Object.FindObjectsByType<GadgetStation>(FindObjectsSortMode.None), Has.Length.EqualTo(1));
        }
    }
}
