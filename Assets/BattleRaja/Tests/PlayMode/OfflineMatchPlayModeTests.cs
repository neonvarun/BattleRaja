using System.Collections;
using System.Linq;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
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
            foreach (var gadgetUser in Object.FindObjectsByType<GadgetUser>())
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
            var actors = Object.FindObjectsByType<MovementPlayerAgent>()
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
            Assert.That(Object.FindObjectsByType<MatchPickup>().Length, Is.EqualTo(3));
            var zoneVisual = Object.FindAnyObjectByType<AandhiZoneVisual>();
            Assert.That(zoneVisual, Is.Not.Null);
            Assert.That(zoneVisual.GetComponentsInChildren<LineRenderer>(true), Has.Length.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator CombatDamageResolverAppliesAuthorityDamageOnceToViewAndSnapshot()
        {
            PlayModeTestHelpers.DisableBots();
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var resolver = Object.FindAnyObjectByType<CombatDamageResolver>();
            var source = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 1).GetComponent<CombatTarget>();
            var target = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 10).GetComponent<CombatTarget>();
            var beforeHealth = target.Health.Snapshot.CurrentHealth;
            var beforeDamage = match.Simulation.GetSnapshots().First(item => item.Id == source.Id).DamageDealt;
            for (var i = 0; i < 9; i++) match.Simulation.Advance(1f);
            match.ClearSpawnProtection(target.Id);

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
        public IEnumerator ProductionAttackCommandsUseAuthorityOrderingAndCooldown()
        {
            PlayModeTestHelpers.DisableBots();
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var player = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 1);
            var attack = player.GetComponent<CombatAttackController>();
            // Production authority rejects attacks during load warmup and spawn
            // protection. Move the pure match state into the opening phase without
            // waiting eight wall-clock seconds in this regression.
            for (var i = 0; i < 9; i++) match.Simulation.Advance(1f);
            var origin = new Float2(player.transform.position.x, player.transform.position.z);
            var command = new AttackCommand(player.GetComponent<CombatTarget>().Id, 1, origin, Float2.Up, true);

            attack.Submit(command);
            attack.Submit(command);
            attack.Submit(new AttackCommand(command.InstigatorId, 0, origin, Float2.Up, true));
            yield return null;

            Assert.That(match.Simulation, Is.Not.Null);
            Assert.That(attack.ActiveProjectileCount, Is.EqualTo(1));
            Assert.That(attack.CooldownRemaining, Is.GreaterThan(0f));
        }

        [UnityTest]
        public IEnumerator MatchTouchControlsUseOrientationAwareSizing()
        {
            var attack = GameObject.Find("AttackButton").GetComponent<RectTransform>();
            var compact = Screen.height > 0 && (float)Screen.width / Screen.height < 0.75f;
            var expected = compact ? 146f : 170f;

            Assert.That(attack.sizeDelta.x, Is.EqualTo(expected).Within(0.1f));
            Assert.That(attack.sizeDelta.y, Is.EqualTo(expected).Within(0.1f));
            yield return null;
        }

        [UnityTest]
        public IEnumerator InMatchAimAssistSettingUpdatesPlayerInputAndPersists()
        {
            var key = "battleraja.settings.aim_assist";
            var hadPrevious = PlayerPrefs.HasKey(key);
            var previous = PlayerPrefs.GetInt(key, 0);
            var input = Object.FindObjectsByType<PlayerInputAdapter>()
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
        public IEnumerator BackgroundLifecyclePausesAndResumesMatchSafely()
        {
            var hud = Object.FindAnyObjectByType<OfflineMatchHud>();
            var settings = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include)
                .FirstOrDefault(item => item.name == "SettingsPanel")?.gameObject;
            Assert.That(hud, Is.Not.Null);
            Assert.That(settings, Is.Not.Null);
            Assert.That(settings.activeSelf, Is.False);
            Assert.That(Time.timeScale, Is.EqualTo(1f));

            var adapter = Object.FindObjectsByType<PlayerInputAdapter>()
                .First(candidate => candidate.GetComponent<MovementPlayerAgent>()?.ActorId == 1);
            var attack = GameObject.Find("AttackButton")?.GetComponent<AttackButton>();
            Assert.That(attack, Is.Not.Null);
            var pointer = new PointerEventData(EventSystem.current)
            {
                pointerId = 91,
                position = Vector2.zero
            };
            attack.OnPointerDown(pointer);
            Assert.That(adapter.IsAttackHeld, Is.True);

            hud.SendMessage("OnApplicationPause", true);
            adapter.SendMessage("OnApplicationPause", true);
            yield return null;

            Assert.That(settings.activeSelf, Is.True);
            Assert.That(Time.timeScale, Is.EqualTo(0f));
            Assert.That(adapter.IsAttackHeld, Is.False,
                "Lifecycle pause must clear a held attack before the app resumes.");
            Assert.That(adapter.HasFocus, Is.False);

            hud.SendMessage("OnApplicationPause", false);
            adapter.SendMessage("OnApplicationPause", false);
            yield return null;

            Assert.That(settings.activeSelf, Is.False);
            Assert.That(Time.timeScale, Is.EqualTo(1f));
            Assert.That(adapter.HasFocus, Is.True);
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
        public IEnumerator EndedAuthorityStateAlwaysPublishesResultsPanel()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var simulation = match.Simulation;
            for (var id = 10; id <= 16; id++) simulation.SyncHealth(new BattleRaja.Core.Domain.CombatEntityId(id), 0);
            simulation.Advance(0.1f);

            Assert.That(simulation.IsEnded, Is.True);
            yield return null;
            yield return null;

            Assert.That(match.ResultsShown, Is.True);
            var panel = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include)
                .First(item => item.name == "ResultsPanel").gameObject;
            Assert.That(panel.activeSelf, Is.True);
        }

        [UnityTest]
        public IEnumerator LiveResultsSurfaceAppearsAndRematchReloadsMatch()
        {
            PlayModeTestHelpers.DisableBots();
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var resolver = Object.FindAnyObjectByType<CombatDamageResolver>();
            var player = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 1);
            var source = player.GetComponent<CombatTarget>();
            var targets = Object.FindObjectsByType<MovementPlayerAgent>()
                .Where(agent => agent.ActorId != 1)
                .Select(agent => agent.GetComponent<CombatTarget>())
                .Where(target => target != null)
                .ToArray();
            for (var i = 0; i < 9; i++) match.Simulation.Advance(1f);

            for (var i = 0; i < targets.Length; i++)
            {
                match.ClearSpawnProtection(targets[i].Id);
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
                var resolver = Object.FindAnyObjectByType<CombatDamageResolver>();
                var player = Object.FindObjectsByType<MovementPlayerAgent>()
                    .First(agent => agent.ActorId == 1);
                var source = player.GetComponent<CombatTarget>();
                var targets = Object.FindObjectsByType<MovementPlayerAgent>()
                    .Where(agent => agent.ActorId != 1)
                    .Select(agent => agent.GetComponent<CombatTarget>())
                    .Where(target => target != null)
                    .ToArray();
                for (var i = 0; i < 9; i++) match.Simulation.Advance(1f);

                foreach (var target in targets)
                {
                    match.ClearSpawnProtection(target.Id);
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
                foreach (var gadgetUser in Object.FindObjectsByType<GadgetUser>())
                {
                    var agent = gadgetUser.GetComponent<MovementPlayerAgent>();
                    if (agent != null && agent.ActorId != 1) gadgetUser.enabled = false;
                }
                Time.timeScale = 1f;
                yield return null;

                Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("MovementLab"));
                Assert.That(Object.FindObjectsByType<OfflineMatchController>(), Has.Length.EqualTo(1));
                Assert.That(Object.FindObjectsByType<OfflineMatchHud>(), Has.Length.EqualTo(1));
                Assert.That(Object.FindObjectsByType<MovementPlayerAgent>()
                    .Count(agent => agent.GetComponent<CombatTarget>() != null), Is.EqualTo(8));
                Assert.That(Object.FindObjectsByType<GadgetStation>(), Is.Empty);
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

                var matches = Object.FindObjectsByType<OfflineMatchController>();
                var actors = Object.FindObjectsByType<MovementPlayerAgent>()
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
