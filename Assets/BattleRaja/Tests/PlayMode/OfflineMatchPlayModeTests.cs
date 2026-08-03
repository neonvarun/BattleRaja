using System.Collections;
using System.Linq;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

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
        public IEnumerator LiveResultsSurfaceAppearsAndRematchReloadsMatch()
        {
            PlayModeTestHelpers.DisableBots();
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var resolver = Object.FindFirstObjectByType<CombatDamageResolver>();
            var player = Object.FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None)
                .First(agent => agent.ActorId == 1);
            var source = player.GetComponent<CombatTarget>();
            var targets = Object.FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None)
                .Where(agent => agent.ActorId != 1)
                .Select(agent => agent.GetComponent<CombatTarget>())
                .Where(target => target != null)
                .ToArray();

            for (var i = 0; i < targets.Length; i++)
            {
                resolver.Resolve(
                    targets[i],
                    new DamageRequest(
                        source.Id,
                        targets[i].Id,
                        source.Faction,
                        1000,
                        DamageType.Ability,
                        Float2.Up,
                        1),
                    allowSelfHit: false,
                    allowFriendlyFire: false,
                    simulationTick: 1);
            }

            yield return new WaitForSeconds(0.25f);

            Assert.That(match.ResultsShown, Is.True);
            var panel = GameObject.Find("ResultsPanel");
            Assert.That(panel, Is.Not.Null);
            Assert.That(panel.activeSelf, Is.True);
            Assert.That(panel.transform.Find("ResultsText").GetComponent<Text>().text, Does.Contain("RESULTS"));

            var rematch = panel.transform.Find("Rematch").GetComponent<Button>();
            rematch.onClick.Invoke();
            yield return new WaitForSeconds(0.5f);

            var reloaded = Object.FindAnyObjectByType<OfflineMatchController>();
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("MovementLab"));
            Assert.That(reloaded, Is.Not.Null);
            Assert.That(reloaded.ResultsShown, Is.False);
            Assert.That(reloaded.AliveCount, Is.EqualTo(8));
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
