using System.Collections;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.Flow;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class BootstrapPlayModeTests
    {
        [UnityTest]
        public IEnumerator PlayerLoopAdvancesInPlayMode()
        {
            var frameBeforeYield = Time.frameCount;

            yield return null;

            Assert.That(Application.isPlaying, Is.True);
            Assert.That(Time.frameCount, Is.GreaterThanOrEqualTo(frameBeforeYield));
        }

        [UnityTest]
        public IEnumerator BootstrapShowsMainMenuAndOfflineFighterRoute()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var flow = Object.FindAnyObjectByType<ProductionFlowController>();
            Assert.That(flow, Is.Not.Null);
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.MainMenu));

            flow.OpenModeSelection();
            flow.SelectOfflineMode();
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.FighterSelection));
            Assert.That(flow.SelectedFighter, Is.EqualTo(ProductionFighter.Bijli));

            flow.ReturnToMenu();
            flow.SelectOnlineMode();
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.Error));
            Assert.That(flow.ErrorCode, Is.EqualTo("ONLINE_UNAVAILABLE"));
        }
    }
}
