using System.Collections;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Flow;
using BattleRaja.Presentation.Match;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

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

            var overlay = Object.FindFirstObjectByType<TutorialOverlay>();
            var match = Object.FindFirstObjectByType<OfflineMatchController>();
            Assert.That(overlay, Is.Not.Null);
            Assert.That(match, Is.Not.Null);
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Movement));
            Assert.That(match.Simulation, Is.Not.Null);
            Assert.That(match.Simulation.AliveCount, Is.EqualTo(8));

            foreach (var brain in Object.FindObjectsByType<BotBrain>(FindObjectsSortMode.None))
            {
                Assert.That(brain.enabled, Is.False);
            }

            overlay.Advance();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Aim));
            overlay.Skip();
            Assert.That(overlay.CurrentStep, Is.EqualTo(TutorialStep.Complete));
        }
    }
}
