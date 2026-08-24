using System.Collections;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Flow;
using BattleRaja.Presentation.Match;
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
    }
}
