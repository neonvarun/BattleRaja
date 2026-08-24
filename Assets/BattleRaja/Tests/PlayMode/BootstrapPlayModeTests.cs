using System.Collections;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.Flow;
using BattleRaja.Presentation.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
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
            // The selected fighter is intentionally persisted for the product, so
            // this route test must choose its expected fixture explicitly rather
            // than depending on another test's PlayerPrefs state.
            flow.SelectBijli();
            Assert.That(flow.SelectedFighter, Is.EqualTo(ProductionFighter.Bijli));

            flow.ReturnToMenu();
            flow.SelectOnlineMode();
            Assert.That(flow.State, Is.EqualTo(ProductionFlowState.Error));
            Assert.That(flow.ErrorCode, Is.EqualTo("ONLINE_UNAVAILABLE"));
        }

        [UnityTest]
        public IEnumerator BootstrapUsesInputSystemUiModuleForTouch()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);
            yield return null;

            var eventSystem = Object.FindAnyObjectByType<EventSystem>();
            Assert.That(eventSystem, Is.Not.Null);
            var uiModule = eventSystem.GetComponent<InputSystemUIInputModule>();
            Assert.That(uiModule, Is.Not.Null);
            Assert.That(uiModule.actionsAsset, Is.Not.Null);
            Assert.That(uiModule.point, Is.Not.Null);
            Assert.That(uiModule.leftClick, Is.Not.Null);
            Assert.That(eventSystem.GetComponent<StandaloneInputModule>(), Is.Null);
        }

        [UnityTest]
        public IEnumerator BootstrapMenuUsesProductIdentityAndNoDeveloperPointerControl()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var logo = Object.FindAnyObjectByType<BattleRajaLogoGraphic>();
            var backdrop = Object.FindAnyObjectByType<BattleRajaUiBackdrop>();
            Assert.That(logo, Is.Not.Null);
            Assert.That(backdrop, Is.Not.Null);
            Assert.That(GameObject.Find("SafeArea/MainMenuPanel/Help"), Is.Not.Null);
            Assert.That(GameObject.Find("SafeArea/MainMenuPanel/Quit"), Is.Null);
        }

        [UnityTest]
        public IEnumerator BootstrapMenuUsesPlayerFacingOfflineStatusCopy()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var version = GameObject.Find("SafeArea/Version").GetComponent<Text>();
            Assert.That(version.text, Does.Contain("OFFLINE ARCADE"));
            Assert.That(version.text.ToUpperInvariant(), Does.Not.Contain("CANDIDATE"));
            Assert.That(version.text.ToUpperInvariant(), Does.Not.Contain("DEBUG"));
        }

        [UnityTest]
        public IEnumerator BootstrapMenuAndFighterSelectionExposeOriginalReadabilityGraphics()
        {
            yield return SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);
            yield return null;
            yield return null;

            Assert.That(Object.FindAnyObjectByType<BattleRajaHeroGraphic>(), Is.Not.Null);

            var flow = Object.FindAnyObjectByType<ProductionFlowController>();
            flow.OpenModeSelection();
            flow.SelectOfflineMode();
            Assert.That(GameObject.Find("SafeArea/FighterPanel/Bijli/FighterGlyph"), Is.Not.Null);
            Assert.That(GameObject.Find("SafeArea/FighterPanel/Pehel/FighterGlyph"), Is.Not.Null);
            Assert.That(GameObject.Find("SafeArea/FighterPanel/Maya/FighterGlyph"), Is.Not.Null);
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

        [UnityTest]
        public IEnumerator SettingsExposeAndPersistHaptics()
        {
            const string key = "battleraja.settings.haptics";
            var hadPrevious = PlayerPrefs.HasKey(key);
            var previous = PlayerPrefs.GetInt(key, 1);

            yield return SceneManager.LoadSceneAsync("Bootstrap", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var flow = Object.FindAnyObjectByType<ProductionFlowController>();
            flow.OpenSettings();
            yield return null;

            var summary = GameObject.Find("SettingsPanel/SettingsSummary").GetComponent<Text>();
            var haptics = GameObject.Find("SettingsPanel/Haptics").GetComponent<Button>();
            Assert.That(haptics, Is.Not.Null);
            var before = PlayerPrefs.GetInt(key, 1) != 0;

            haptics.onClick.Invoke();
            Assert.That(summary.text, Does.Contain(before ? "HAPTICS: OFF" : "HAPTICS: ON"));
            Assert.That(PlayerPrefs.GetInt(key, -1), Is.EqualTo(before ? 0 : 1));

            if (hadPrevious) PlayerPrefs.SetInt(key, previous);
            else PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }
}
