using System.Collections;
using System.Linq;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Gadgets;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class VerticalSlicePlayModeTests
    {
        [UnitySetUp]
        public IEnumerator LoadLab()
        {
            yield return SceneManager.LoadSceneAsync("MovementLab", LoadSceneMode.Single);
            PlayModeTestHelpers.DisableBots();
            yield return null;
        }

        [UnityTest]
        public IEnumerator SceneContainsBijliPehelAndMayaDefinitions()
        {
            var ids = Object.FindObjectsByType<BijliFighterController>(FindObjectsSortMode.None)
                .Select(controller => controller.Definition.FighterId.Value)
                .Distinct()
                .ToArray();
            Assert.That(ids, Does.Contain("fighter.bijli"));
            Assert.That(ids, Does.Contain("fighter.pehel"));
            Assert.That(ids, Does.Contain("fighter.maya"));
            Assert.That(ids, Has.Length.EqualTo(3));
            yield return null;
        }

        [UnityTest]
        public IEnumerator ExistingOfflineMatchAndGadgetSystemsRemainPresent()
        {
            Assert.That(Object.FindFirstObjectByType<OfflineMatchController>(), Is.Not.Null);
            Assert.That(Object.FindObjectsByType<GadgetUser>(FindObjectsSortMode.None), Has.Length.EqualTo(8));
            yield return null;
        }
    }
}
