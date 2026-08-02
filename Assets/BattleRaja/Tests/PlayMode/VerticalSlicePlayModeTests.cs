using System.Collections;
using System.Linq;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Visuals;
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
        public IEnumerator LoadBazaarBastion()
        {
            yield return SceneManager.LoadSceneAsync("BazaarBastion", LoadSceneMode.Single);
            PlayModeTestHelpers.DisableBots();
            yield return null;
        }

        [UnityTest]
        public IEnumerator SceneContainsBijliPehelAndMayaDefinitions()
        {
            var ids = Object.FindObjectsByType<BijliFighterController>(FindObjectsSortMode.None)
                .Select(controller => controller.Definition.FighterId.Value)
                .Concat(Object.FindObjectsByType<PehelFighterController>(FindObjectsSortMode.None)
                    .Select(controller => controller.Definition.FighterId.Value))
                .Concat(Object.FindObjectsByType<MayaFighterController>(FindObjectsSortMode.None)
                    .Select(controller => controller.Definition.FighterId.Value))
                .Distinct()
                .ToArray();
            Assert.That(ids, Does.Contain("fighter.bijli"));
            Assert.That(ids, Does.Contain("fighter.pehel"));
            Assert.That(ids, Does.Contain("fighter.maya"));
            Assert.That(ids, Has.Length.EqualTo(3));
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionSceneUsesFighterSpecificAbilityControllers()
        {
            Assert.That(Object.FindObjectsByType<PehelFighterController>(FindObjectsSortMode.None), Has.Length.GreaterThanOrEqualTo(1));
            Assert.That(Object.FindObjectsByType<MayaFighterController>(FindObjectsSortMode.None), Has.Length.GreaterThanOrEqualTo(1));
            Assert.That(GameObject.Find("BazaarBastion"), Is.Not.Null);
            Assert.That(GameObject.Find("BazaarArchitecture"), Is.Not.Null);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionSceneHasReadableFighterAndAudioPresentation()
        {
            Assert.That(Object.FindObjectsByType<FighterPresentation>(FindObjectsSortMode.None), Has.Length.GreaterThanOrEqualTo(8));
            Assert.That(Object.FindObjectsByType<BattleRajaAudioDirector>(FindObjectsSortMode.None), Has.Length.EqualTo(1));
            var visual = Object.FindFirstObjectByType<FighterPresentation>();
            Assert.That(visual.CurrentAnimation, Is.EqualTo(FighterPresentation.AnimationState.Idle).Or.EqualTo(FighterPresentation.AnimationState.Locomotion));
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
