using System.Collections;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Flow;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class TutorialArenaPlayModeTests
    {
        [UnityTest]
        public IEnumerator TutorialLoadsRealMatchAuthorityWithReplayableOverlay()
        {
            yield return SceneManager.LoadSceneAsync("TutorialArena", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var overlay = Object.FindAnyObjectByType<TutorialOverlay>();
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            Assert.That(overlay, Is.Not.Null);
            Assert.That(match, Is.Not.Null);
            Assert.That(GameObject.Find("TutorialCanvas/SafeArea/TutorialBackdrop"), Is.Null,
                "The tutorial must keep the live arena visible behind its prompt.");
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Movement));
            Assert.That(match.Simulation, Is.Not.Null);
            Assert.That(match.Simulation.AliveCount, Is.EqualTo(8));

            foreach (var brain in Object.FindObjectsByType<BotBrain>())
            {
                Assert.That(brain.enabled, Is.False);
            }

            Assert.That(overlay.CurrentStepSatisfied, Is.False);
            Assert.DoesNotThrow(() => overlay.Advance());
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Movement));
            overlay.ObserveAction(TutorialAction.Movement);
            overlay.Advance();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Aim));
            overlay.Skip();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Complete));
        }

        private static GameObject FindSceneObject(string name)
        {
            return Object.FindObjectsByType<Transform>(FindObjectsInactive.Include)
                .First(item => item.name == name).gameObject;
        }

        [UnityTest]
        public IEnumerator TutorialWalkthroughPublishesAllEightStepsAndPersistsCompletion()
        {
            const string completedKey = "battleraja.tutorial.completed";
            var hadPrevious = PlayerPrefs.HasKey(completedKey);
            var previous = PlayerPrefs.GetInt(completedKey, 0);
            PlayerPrefs.DeleteKey(completedKey);
            PlayerPrefs.Save();

            yield return SceneManager.LoadSceneAsync("TutorialArena", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var overlay = Object.FindAnyObjectByType<TutorialOverlay>();
            var panel = FindSceneObject("TutorialPanel");
            Assert.That(overlay, Is.Not.Null);
            Assert.That(panel, Is.Not.Null);
            var title = panel.transform.Find("Title").GetComponent<Text>();
            var progress = panel.transform.Find("Progress").GetComponent<Text>();
            var expectedTitles = new[]
            {
                "MOVEMENT", "AIM", "BASIC ATTACK", "ABILITY",
                "GADGET", "AANDHI", "ELIMINATION", "VICTORY"
            };

            var actions = new[]
            {
                TutorialAction.Movement,
                TutorialAction.Aim,
                TutorialAction.BasicAttack,
                TutorialAction.Ability,
                TutorialAction.GadgetCollected,
                TutorialAction.AandhiObserved,
                TutorialAction.Elimination,
                TutorialAction.Victory
            };

            for (var i = 0; i < expectedTitles.Length; i++)
            {
                Assert.That(title.text, Does.Contain(expectedTitles[i]), $"step {i} title");
                Assert.That(progress.text, Does.StartWith($"{i + 1} / 8"), $"step {i} progress");
                if (i == 4)
                {
                    Assert.That(overlay.ObserveAction(TutorialAction.GadgetCollected), Is.True);
                    Assert.That(overlay.CurrentStepSatisfied, Is.False);
                    Assert.That(overlay.ObserveAction(TutorialAction.GadgetUsed), Is.True);
                }
                else
                {
                    Assert.That(overlay.ObserveAction(actions[i]), Is.True);
                }
                overlay.Advance();
                yield return null;
            }

            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Complete));
            Assert.That(panel.activeSelf, Is.True);
            Assert.That(title.text, Does.Contain("TUTORIAL COMPLETE"));
            Assert.That(progress.text, Does.Contain("8 / 8 COMPLETE"));
            Assert.That(PlayerPrefs.GetInt(completedKey, 0), Is.EqualTo(1));

            overlay.Replay();
            yield return null;
            yield return null;
            overlay = Object.FindAnyObjectByType<TutorialOverlay>();
            panel = FindSceneObject("TutorialPanel");
            title = panel.transform.Find("Title").GetComponent<Text>();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Movement));
            Assert.That(title.text, Does.Contain("MOVEMENT"));
            Assert.That(panel.activeSelf, Is.True);
            overlay.Skip();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Complete));
            Assert.That(panel.activeSelf, Is.True);

            if (hadPrevious) PlayerPrefs.SetInt(completedKey, previous);
            else PlayerPrefs.DeleteKey(completedKey);
            PlayerPrefs.Save();
        }

        [UnityTest]
        public IEnumerator TutorialPromptNamesTheActiveStickWhenLeftHanded()
        {
            const string leftHandedKey = "battleraja.settings.left_handed";
            var hadPrevious = PlayerPrefs.HasKey(leftHandedKey);
            var previous = PlayerPrefs.GetInt(leftHandedKey, 0);
            PlayerPrefs.SetInt(leftHandedKey, 1);
            PlayerPrefs.Save();

            try
            {
                yield return SceneManager.LoadSceneAsync("TutorialArena", LoadSceneMode.Single);
                yield return null;
                yield return null;

                var panel = FindSceneObject("TutorialPanel");
                var body = panel.transform.Find("Body").GetComponent<Text>();
                Assert.That(body.text, Does.Contain("right stick"));
            }
            finally
            {
                if (hadPrevious) PlayerPrefs.SetInt(leftHandedKey, previous);
                else PlayerPrefs.DeleteKey(leftHandedKey);
                PlayerPrefs.Save();
            }
        }

        [UnityTest]
        public IEnumerator PreCollectedGadgetIsReconciledWhenGadgetLessonBegins()
        {
            PlayerPrefs.DeleteKey("battleraja.tutorial.completed");
            PlayerPrefs.Save();

            yield return SceneManager.LoadSceneAsync("TutorialArena", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.5f);

            var overlay = Object.FindAnyObjectByType<TutorialOverlay>();
            var gadget = Object.FindObjectsByType<GadgetUser>()
                .First(user => user.GetComponent<CombatTarget>()?.Id.Value == 1);
            Assert.That(gadget.HasGadget, Is.True,
                "The tutorial authority should collect the nearby Tiffin before the later lesson.");

            var actions = new[]
            {
                TutorialAction.Movement,
                TutorialAction.Aim,
                TutorialAction.BasicAttack,
                TutorialAction.Ability
            };
            for (var i = 0; i < actions.Length; i++)
            {
                Assert.That(overlay.ObserveAction(actions[i]), Is.True, $"action {actions[i]}");
                overlay.Advance();
            }

            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Gadget));
            Assert.That(overlay.ObserveAction(TutorialAction.GadgetUsed), Is.True,
                "The already-authoritative pickup must count before the use sub-step.");
        }

        [UnityTest]
        public IEnumerator TutorialEliminationTargetStartsInReadableOpenLane()
        {
            yield return SceneManager.LoadSceneAsync("TutorialArena", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var player = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 1);
            var target = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 11);

            Assert.That(target.transform.position.x, Is.EqualTo(0f).Within(0.01f));
            Assert.That(target.transform.position.z, Is.EqualTo(-3.2f).Within(0.01f));
            Assert.That(target.transform.position.z, Is.LessThan(-2f),
                "The tutorial target must stay south of the central wall lane.");
            Assert.That(Vector3.Distance(player.transform.position, target.transform.position),
                Is.GreaterThanOrEqualTo(2.5f),
                "The target must remain a valid separated match participant.");
        }

        [UnityTest]
        public IEnumerator EliminationLessonUnlocksFromLiveAuthoritativeSnapshotBeforeResults()
        {
            yield return SceneManager.LoadSceneAsync("TutorialArena", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var overlay = Object.FindAnyObjectByType<TutorialOverlay>();
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            Assert.That(overlay, Is.Not.Null);
            Assert.That(match, Is.Not.Null);

            var actions = new[]
            {
                TutorialAction.Movement,
                TutorialAction.Aim,
                TutorialAction.BasicAttack,
                TutorialAction.Ability,
                TutorialAction.GadgetCollected,
                TutorialAction.GadgetUsed,
                TutorialAction.AandhiObserved
            };
            for (var i = 0; i < actions.Length; i++)
            {
                Assert.That(overlay.ObserveAction(actions[i]), Is.True, $"action {actions[i]}");
                overlay.Advance();
            }

            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Elimination));
            Assert.That(overlay.CurrentStepSatisfied, Is.False);

            // Let the real authority leave warmup/spawn protection, then resolve a
            // lethal player-authored hit against a live participant. The overlay must
            // unlock from this live snapshot; terminal Results are not required.
            yield return new WaitForSeconds(9.2f);
            var request = new DamageRequest(
                new CombatEntityId(1),
                new CombatEntityId(11),
                CombatFaction.Player,
                999,
                DamageType.Projectile,
                new Float2(-1f, 0f),
                match.SimulationTick + 1);
            var damage = match.ResolveDamage(request, CombatFaction.Enemy, false, false);
            Assert.That(damage.Result.Applied, Is.True);
            Assert.That(damage.Result.TargetDefeated, Is.True);
            Assert.That(match.ResultsShown, Is.False);

            yield return null;
            Assert.That(overlay.CurrentStepSatisfied, Is.True);
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Elimination));
        }

        [UnityTest]
        public IEnumerator TutorialLocalAttackCanResolveReadableTarget()
        {
            yield return SceneManager.LoadSceneAsync("TutorialArena", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var player = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 1);
            var target = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 11);
            var attack = player.GetComponent<CombatAttackController>();
            var health = target.GetComponent<CombatHealth>();
            Assert.That(match, Is.Not.Null);
            Assert.That(attack, Is.Not.Null);
            Assert.That(health, Is.Not.Null);

            yield return new WaitForSeconds(9.2f);
            var direction = new Float2(
                target.transform.position.x - player.transform.position.x,
                target.transform.position.z - player.transform.position.z).Normalized;
            var origin = new Float2(player.transform.position.x, player.transform.position.z) + direction * 0.7f;
            attack.Submit(AttackCommandFactory.Create(
                new CombatEntityId(1),
                match.SimulationTick + 1,
                origin,
                direction,
                true));

            yield return new WaitForSeconds(0.5f);
            Assert.That(health.Snapshot.CurrentHealth, Is.LessThan(health.MaxHealth));
        }
    }
}
