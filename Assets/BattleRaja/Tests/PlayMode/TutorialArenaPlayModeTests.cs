using System.Collections;
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

            overlay.Advance();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Aim));
            overlay.Skip();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Complete));
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
            var panel = GameObject.Find("TutorialPanel");
            Assert.That(overlay, Is.Not.Null);
            Assert.That(panel, Is.Not.Null);
            var title = panel.transform.Find("Title").GetComponent<Text>();
            var progress = panel.transform.Find("Progress").GetComponent<Text>();
            var expectedTitles = new[]
            {
                "MOVEMENT", "AIM", "BASIC ATTACK", "ABILITY",
                "GADGET", "AANDHI", "ELIMINATION", "VICTORY"
            };

            for (var i = 0; i < expectedTitles.Length; i++)
            {
                Assert.That(title.text, Does.Contain(expectedTitles[i]), $"step {i} title");
                Assert.That(progress.text, Does.StartWith($"{i + 1} / 8"), $"step {i} progress");
                overlay.Advance();
                yield return null;
            }

            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Complete));
            Assert.That(title.text, Does.Contain("TUTORIAL COMPLETE"));
            Assert.That(progress.text, Does.Contain("8 / 8 COMPLETE"));
            Assert.That(PlayerPrefs.GetInt(completedKey, 0), Is.EqualTo(1));

            overlay.Replay();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Movement));
            Assert.That(title.text, Does.Contain("MOVEMENT"));
            overlay.Skip();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Complete));

            if (hadPrevious) PlayerPrefs.SetInt(completedKey, previous);
            else PlayerPrefs.DeleteKey(completedKey);
            PlayerPrefs.Save();
        }
    }
}
