using System.Collections;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.Flow;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

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

        [UnityTest]
        public IEnumerator SettingsExposeAndPersistEffectsVolume()
        {
            const string key = "battleraja.settings.effects_volume";
            var hadPrevious = PlayerPrefs.HasKey(key);
            var previous = PlayerPrefs.GetFloat(key, 1f);

            yield return SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var flow = Object.FindAnyObjectByType<ProductionFlowController>();
            flow.OpenSettings();
            yield return null;

            var summary = GameObject.Find("SettingsPanel/SettingsSummary").GetComponent<Text>();
            var decrease = GameObject.Find("SettingsPanel/EffectsDown").GetComponent<Button>();
            var increase = GameObject.Find("SettingsPanel/EffectsUp").GetComponent<Button>();
            Assert.That(decrease, Is.Not.Null);
            Assert.That(increase, Is.Not.Null);

            increase.onClick.Invoke();
            Assert.That(summary.text, Does.Contain("EFFECTS: 100%"));
            Assert.That(PlayerPrefs.GetFloat(key, 0f), Is.EqualTo(1f).Within(0.001f));

            decrease.onClick.Invoke();
            Assert.That(summary.text, Does.Contain("EFFECTS: 90%"));
            Assert.That(PlayerPrefs.GetFloat(key, 0f), Is.EqualTo(0.9f).Within(0.001f));

            if (hadPrevious) PlayerPrefs.SetFloat(key, previous);
            else PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }
}
