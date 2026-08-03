using System.Collections;
using System.Linq;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Gadgets;
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
            PlayModeTestHelpers.DisableBots();
            foreach (var gadgetUser in Object.FindObjectsByType<GadgetUser>(FindObjectsSortMode.None))
            {
                var agent = gadgetUser.GetComponent<MovementPlayerAgent>();
                if (agent != null && agent.ActorId != 1) gadgetUser.enabled = false;
            }
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
        public IEnumerator CombatDamageResolverAppliesAuthorityDamageOnceToViewAndSnapshot()
        {
            PlayModeTestHelpers.DisableBots();
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var resolver = Object.FindFirstObjectByType<CombatDamageResolver>();
            var source = Object.FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None)
                .First(agent => agent.ActorId == 1).GetComponent<CombatTarget>();
            var target = Object.FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None)
                .First(agent => agent.ActorId == 10).GetComponent<CombatTarget>();
            var beforeHealth = target.Health.Snapshot.CurrentHealth;
            var beforeDamage = match.Simulation.GetSnapshots().First(item => item.Id == source.Id).DamageDealt;

            var result = resolver.Resolve(
                target,
                new DamageRequest(source.Id, target.Id, source.Faction, 25, DamageType.Projectile, new Float2(1f, 0f), 1),
                allowSelfHit: false,
                allowFriendlyFire: false,
                simulationTick: 1);
            var snapshot = match.Simulation.GetSnapshots().First(item => item.Id == target.Id);
            var attacker = match.Simulation.GetSnapshots().First(item => item.Id == source.Id);

            Assert.That(result.Applied, Is.True);
            Assert.That(target.Health.Snapshot.CurrentHealth, Is.EqualTo(beforeHealth - 25));
            Assert.That(snapshot.CurrentHealth, Is.EqualTo(beforeHealth - 25));
            Assert.That(attacker.DamageDealt, Is.EqualTo(beforeDamage + 25));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InMatchAimAssistSettingUpdatesPlayerInputAndPersists()
        {
            var key = "battleraja.settings.aim_assist";
            var hadPrevious = PlayerPrefs.HasKey(key);
            var previous = PlayerPrefs.GetInt(key, 0);
            var input = Object.FindObjectsByType<PlayerInputAdapter>(FindObjectsSortMode.None)
                .First(adapter => adapter.GetComponent<MovementPlayerAgent>()?.ActorId == 1);
            var before = input.AimAssistEnabled;

            var pause = GameObject.Find("Pause").GetComponent<Button>();
            pause.onClick.Invoke();
            yield return null;

            var settings = GameObject.Find("SettingsPanel");
            Assert.That(settings, Is.Not.Null);
            Assert.That(settings.activeSelf, Is.True);
            var toggle = settings.transform.Find("AimAssist").GetComponent<Button>();
            toggle.onClick.Invoke();
            yield return null;

            Assert.That(input.AimAssistEnabled, Is.EqualTo(!before));
            Assert.That(PlayerPrefs.GetInt(key, 0), Is.EqualTo(!before ? 1 : 0));

            toggle.onClick.Invoke();
            yield return null;
            Assert.That(input.AimAssistEnabled, Is.EqualTo(before));

            if (hadPrevious) PlayerPrefs.SetInt(key, previous);
            else PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            pause.onClick.Invoke();
            yield return null;
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
        public IEnumerator RepeatedResultsRematchesKeepRuntimeGraphClean()
        {
            for (var round = 0; round < 3; round++)
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

                foreach (var target in targets)
                {
                    resolver.Resolve(
                        target,
                        new DamageRequest(
                            source.Id,
                            target.Id,
                            source.Faction,
                            1000,
                            DamageType.Ability,
                            Float2.Up,
                            round + 1),
                        allowSelfHit: false,
                        allowFriendlyFire: false,
                        simulationTick: round + 1);
                }

                yield return new WaitForSeconds(0.2f);

                Assert.That(match.ResultsShown, Is.True, $"round {round} did not publish results");
                var panel = GameObject.Find("ResultsPanel");
                Assert.That(panel, Is.Not.Null);
                Assert.That(panel.activeSelf, Is.True);
                Time.timeScale = 0f;
                panel.transform.Find("Rematch").GetComponent<Button>().onClick.Invoke();
                yield return new WaitForSecondsRealtime(0.45f);
                PlayModeTestHelpers.DisableBots();
                foreach (var gadgetUser in Object.FindObjectsByType<GadgetUser>(FindObjectsSortMode.None))
                {
                    var agent = gadgetUser.GetComponent<MovementPlayerAgent>();
                    if (agent != null && agent.ActorId != 1) gadgetUser.enabled = false;
                }
                Time.timeScale = 1f;
                yield return null;

                Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("MovementLab"));
                Assert.That(Object.FindObjectsByType<OfflineMatchController>(FindObjectsSortMode.None), Has.Length.EqualTo(1));
                Assert.That(Object.FindObjectsByType<OfflineMatchHud>(FindObjectsSortMode.None), Has.Length.EqualTo(1));
                Assert.That(Object.FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None)
                    .Count(agent => agent.GetComponent<CombatTarget>() != null), Is.EqualTo(8));
                Assert.That(Object.FindObjectsByType<GadgetStation>(FindObjectsSortMode.None), Is.Empty);
                Assert.That(Object.FindAnyObjectByType<OfflineMatchController>().ResultsShown, Is.False);
                Assert.That(Time.timeScale, Is.EqualTo(1f));
            }
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
