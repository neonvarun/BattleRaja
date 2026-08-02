using System.Collections;
using System.Linq;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class OfflineMatchPlayModeTests
    {
        [UnitySetUp]
        public IEnumerator LoadMovementLab()
        {
            yield return SceneManager.LoadSceneAsync("MovementLab", LoadSceneMode.Single);
            yield return null;
        }

        [UnityTest]
        public IEnumerator OfflineMatchStartsWithEightSeparatedCombatants()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var actors = Object.FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None)
                .Where(agent => agent.GetComponent<CombatTarget>() != null)
                .ToArray();

            Assert.That(match, Is.Not.Null);
            Assert.That(match.Simulation.IsStarted, Is.True);
            Assert.That(actors.Length, Is.EqualTo(8));
            Assert.That(match.AliveCount, Is.EqualTo(8));
            yield return null;
        }

        [UnityTest]
        public IEnumerator MatchControllerPublishesZoneAndPickupPresentationState()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            yield return new WaitForSeconds(0.25f);

            Assert.That(match.CurrentPhase, Is.EqualTo(BattleRaja.Core.Domain.MatchPhase.LoadWarmup));
            Assert.That(match.ZoneRadius, Is.GreaterThan(0f));
            Assert.That(Object.FindObjectsByType<MatchPickup>(FindObjectsSortMode.None).Length, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator AcceleratedSimulationCanReachResultsAndStableWinner()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var simulation = match.Simulation;
            for (var id = 10; id <= 16; id++) simulation.SyncHealth(new BattleRaja.Core.Domain.CombatEntityId(id), 0);
            var tick = simulation.Advance(0.1f);

            Assert.That(tick.MatchEnded, Is.True);
            Assert.That(simulation.GetSnapshots().Count(snapshot => snapshot.Placement == 1), Is.EqualTo(1));
            yield return null;
        }

        [UnityTest]
        public IEnumerator RepeatedProductionSceneLoadsKeepOneOfflineRuntimeGraph()
        {
            for (var iteration = 0; iteration < 3; iteration++)
            {
                yield return SceneManager.LoadSceneAsync("BazaarBastion", LoadSceneMode.Single);
                yield return null;
                yield return null;

                var matches = Object.FindObjectsByType<OfflineMatchController>(FindObjectsSortMode.None);
                var actors = Object.FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None)
                    .Where(agent => agent.GetComponent<CombatTarget>() != null)
                    .ToArray();

                Assert.That(matches.Length, Is.EqualTo(1), $"iteration {iteration} left duplicate match controllers");
                Assert.That(actors.Length, Is.EqualTo(8), $"iteration {iteration} changed the authority actor count");
                Assert.That(matches[0].Simulation, Is.Not.Null);
                Assert.That(matches[0].Simulation.AliveCount, Is.EqualTo(8));
                Assert.That(Time.timeScale, Is.EqualTo(1f));
            }
        }
    }
}
