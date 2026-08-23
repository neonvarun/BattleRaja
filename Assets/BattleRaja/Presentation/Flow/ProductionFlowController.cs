using System;
using System.Collections;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.Movement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleRaja.Presentation.Flow
{
    /// <summary>
    /// Runtime production flow for the first playable product loop. It owns presentation
    /// navigation and local preferences only; match outcomes remain in the match authority.
    /// </summary>
    public sealed class ProductionFlowController : MonoBehaviour
    {
        private const string SelectedFighterKey = "battleraja.selected_fighter";
        private const string LeftHandedKey = "battleraja.settings.left_handed";
        private const string ReducedFlashesKey = "battleraja.settings.reduced_flashes";
        private const string HighContrastKey = "battleraja.settings.high_contrast";
        private const string AimAssistKey = "battleraja.settings.aim_assist";
        private const string MusicVolumeKey = "battleraja.settings.music_volume";
        private const string EffectsVolumeKey = "battleraja.settings.effects_volume";

        [SerializeField] private string gameplaySceneName = "BazaarBastion";
        [SerializeField] private string tutorialSceneName = "TutorialArena";
        [SerializeField] private Canvas canvas;

        private readonly ProductionFlowMachine _flow = new ProductionFlowMachine();
        private ProductionFighter _selectedFighter;
        private bool _leftHanded;
        private bool _reducedFlashes;
        private bool _highContrast;
        private bool _aimAssist;
        private float _musicVolume;
        private float _effectsVolume;
        private bool _loading;

        private GameObject _safeArea;
        private GameObject _mainMenuPanel;
        private GameObject _modePanel;
        private GameObject _fighterPanel;
        private GameObject _loadingPanel;
        private GameObject _settingsPanel;
        private GameObject _errorPanel;
        private Text _titleText;
        private Text _messageText;
        private Text _fighterSummaryText;
        private Text _loadingText;
        private Text _errorText;
        private Text _settingsSummaryText;

        public ProductionFlowState State => _flow.State;
        public ProductionGameMode Mode => _flow.Mode;
        public ProductionFighter SelectedFighter => _selectedFighter;
        public string ErrorCode => _flow.ErrorCode;
        public bool IsLoading => _loading;

        private void Awake()
        {
            LoadPreferences();
            EnsureEventSystem();
            EnsureCanvas();
            BuildCanvasUi();
        }

        private IEnumerator Start()
        {
            // Let the canvas and EventSystem settle before selecting the first button. This
            // also makes the initial state deterministic in PlayMode and in Web builds.
            yield return null;
            Apply(_flow.FinishBootstrap());
            SelectFirstButton(_mainMenuPanel);
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                NavigateBack();
            }
        }

        public void OpenModeSelection()
        {
            Apply(_flow.OpenModeSelection());
        }

        public void SelectOfflineMode()
        {
            Apply(_flow.SelectMode(ProductionGameMode.Offline));
        }

        public void SelectOnlineMode()
        {
            if (_flow.State == ProductionFlowState.MainMenu)
            {
                Apply(_flow.OpenModeSelection());
            }

            Apply(_flow.SelectMode(ProductionGameMode.Online));
        }

        public void SelectBijli()
        {
            SelectFighter(ProductionFighter.Bijli);
        }

        public void SelectPehel()
        {
            SelectFighter(ProductionFighter.Pehel);
        }

        public void SelectMaya()
        {
            SelectFighter(ProductionFighter.Maya);
        }

        public void BeginOfflineMatch()
        {
            var transition = _flow.BeginMatchLoading();
            if (!transition.Accepted) return;
            SavePreferences();
            Apply(transition);
            StartCoroutine(LoadGameplayScene());
        }

        public void OpenTutorial()
        {
            var transition = _flow.OpenTutorial();
            if (!transition.Accepted) return;
            Apply(transition);
            StartCoroutine(LoadTutorialScene());
        }

        public void OpenSettings()
        {
            Apply(_flow.OpenSettings());
        }

        public void CloseSettings()
        {
            Apply(_flow.CloseSettings());
        }

        public void ToggleLeftHanded()
        {
            _leftHanded = !_leftHanded;
            SavePreferences();
            RefreshSettingsSummary();
        }

        public void ToggleReducedFlashes()
        {
            _reducedFlashes = !_reducedFlashes;
            SavePreferences();
            RefreshSettingsSummary();
        }

        public void ToggleHighContrast()
        {
            _highContrast = !_highContrast;
            SavePreferences();
            ApplyContrast();
            RefreshSettingsSummary();
        }

        public void ToggleAimAssist()
        {
            _aimAssist = !_aimAssist;
            SavePreferences();
            RefreshSettingsSummary();
        }

        public void IncreaseMusicVolume()
        {
            _musicVolume = Mathf.Clamp01(_musicVolume + 0.1f);
            SavePreferences();
            RefreshSettingsSummary();
        }

        public void DecreaseMusicVolume()
        {
            _musicVolume = Mathf.Clamp01(_musicVolume - 0.1f);
            SavePreferences();
            RefreshSettingsSummary();
        }

        public void IncreaseEffectsVolume()
        {
            _effectsVolume = Mathf.Clamp01(_effectsVolume + 0.1f);
            SavePreferences();
            RefreshSettingsSummary();
        }

        public void DecreaseEffectsVolume()
        {
            _effectsVolume = Mathf.Clamp01(_effectsVolume - 0.1f);
            SavePreferences();
            RefreshSettingsSummary();
        }

        public void Retry()
        {
            var transition = _flow.Retry();
            Apply(transition);
            if (transition.Accepted && transition.Current == ProductionFlowState.MatchLoading)
            {
                StartCoroutine(LoadGameplayScene());
            }
        }

        public void ReturnToMenu()
        {
            if (_loading) return;
            Apply(_flow.ReturnToMenu());
        }

        public void NavigateBack()
        {
            switch (_flow.State)
            {
                case ProductionFlowState.ModeSelection:
                case ProductionFlowState.FighterSelection:
                case ProductionFlowState.Error:
                    ReturnToMenu();
                    break;
                case ProductionFlowState.Settings:
                    CloseSettings();
                    break;
            }
        }

        private void SelectFighter(ProductionFighter fighter)
        {
            _selectedFighter = fighter;
            SavePreferences();
            Apply(_flow.SelectFighter(fighter));
        }

        private IEnumerator LoadGameplayScene()
        {
            if (_loading) yield break;
            _loading = true;
            if (!Application.CanStreamedLevelBeLoaded(gameplaySceneName))
            {
                _loading = false;
                Apply(_flow.ShowError("MATCH_SCENE_UNAVAILABLE", ProductionFlowState.MatchLoading));
                yield break;
            }

            AsyncOperation operation;
            try
            {
                operation = SceneManager.LoadSceneAsync(gameplaySceneName, LoadSceneMode.Single);
            }
            catch (Exception exception)
            {
                Debug.LogError($"BattleRaja match load failed: {exception.Message}");
                _loading = false;
                Apply(_flow.ShowError("MATCH_LOAD_FAILED", ProductionFlowState.MatchLoading));
                yield break;
            }

            if (operation == null)
            {
                _loading = false;
                Apply(_flow.ShowError("MATCH_LOAD_FAILED", ProductionFlowState.MatchLoading));
                yield break;
            }

            operation.allowSceneActivation = false;
            while (operation.progress < 0.9f)
            {
                var percentage = Mathf.Clamp(Mathf.RoundToInt((operation.progress / 0.9f) * 100f), 0, 99);
                if (_loadingText != null) _loadingText.text = $"LOADING BAZAAR BASTION  {percentage}%";
                yield return null;
            }

            if (_loadingText != null) _loadingText.text = "LOADING BAZAAR BASTION  100%";
            _flow.FinishMatchLoading();
            operation.allowSceneActivation = true;
            yield return operation;
            _loading = false;
        }

        private IEnumerator LoadTutorialScene()
        {
            if (_loading) yield break;
            _loading = true;
            if (!Application.CanStreamedLevelBeLoaded(tutorialSceneName))
            {
                _loading = false;
                Apply(_flow.ShowError("TUTORIAL_SCENE_UNAVAILABLE", ProductionFlowState.Tutorial));
                yield break;
            }

            AsyncOperation operation;
            try
            {
                operation = SceneManager.LoadSceneAsync(tutorialSceneName, LoadSceneMode.Single);
            }
            catch (Exception exception)
            {
                Debug.LogError($"BattleRaja tutorial load failed: {exception.Message}");
                _loading = false;
                Apply(_flow.ShowError("TUTORIAL_LOAD_FAILED", ProductionFlowState.Tutorial));
                yield break;
            }

            if (operation == null)
            {
                _loading = false;
                Apply(_flow.ShowError("TUTORIAL_LOAD_FAILED", ProductionFlowState.Tutorial));
                yield break;
            }

            operation.allowSceneActivation = false;
            while (operation.progress < 0.9f)
            {
                var percentage = Mathf.Clamp(Mathf.RoundToInt((operation.progress / 0.9f) * 100f), 0, 99);
                if (_loadingText != null) _loadingText.text = $"LOADING TUTORIAL ARENA  {percentage}%";
                yield return null;
            }

            if (_loadingText != null) _loadingText.text = "LOADING TUTORIAL ARENA  100%";
            operation.allowSceneActivation = true;
            yield return operation;
            _loading = false;
        }

        private void Apply(ProductionFlowTransition transition)
        {
            if (!transition.Accepted) return;

            SetAllPanelsInactive();
            switch (transition.Current)
            {
                case ProductionFlowState.MainMenu:
                    _mainMenuPanel.SetActive(true);
                    SetHeader("BATTLE RAJA", "A compact offline battle royale prototype");
                    break;
                case ProductionFlowState.ModeSelection:
                    _modePanel.SetActive(true);
                    SetHeader("CHOOSE MODE", "Online rooms remain locked until the approved Fusion session gate is complete.");
                    break;
                case ProductionFlowState.FighterSelection:
                    _fighterPanel.SetActive(true);
                    SetHeader("CHOOSE YOUR RAJA", "Select a fighter, then start the offline match.");
                    RefreshFighterSummary();
                    break;
                case ProductionFlowState.Tutorial:
                    _loadingPanel.SetActive(true);
                    SetHeader("TUTORIAL", "Loading the replayable offline controls and combat walkthrough.");
                    if (_loadingText != null) _loadingText.text = "LOADING TUTORIAL ARENA  0%";
                    break;
                case ProductionFlowState.MatchLoading:
                    _loadingPanel.SetActive(true);
                    SetHeader("PREPARING MATCH", "Loading the Bazaar Bastion vertical slice.");
                    if (_loadingText != null) _loadingText.text = "LOADING BAZAAR BASTION  0%";
                    break;
                case ProductionFlowState.Settings:
                    _settingsPanel.SetActive(true);
                    SetHeader("SETTINGS", "Preferences are stored locally on this device/browser.");
                    RefreshSettingsSummary();
                    break;
                case ProductionFlowState.Error:
                    _errorPanel.SetActive(true);
                    SetHeader("CAN'T CONTINUE", "The next step is unavailable in this build.");
                    _errorText.text = FormatError(_flow.ErrorCode);
                    break;
            }

            ApplyContrast();
            SelectFirstButton(transition.Current == ProductionFlowState.MainMenu ? _mainMenuPanel :
                transition.Current == ProductionFlowState.ModeSelection ? _modePanel :
                transition.Current == ProductionFlowState.FighterSelection ? _fighterPanel :
                transition.Current == ProductionFlowState.Tutorial ? _loadingPanel :
                transition.Current == ProductionFlowState.Settings ? _settingsPanel : _errorPanel);
        }

        private void SetAllPanelsInactive()
        {
            _mainMenuPanel.SetActive(false);
            _modePanel.SetActive(false);
            _fighterPanel.SetActive(false);
            _loadingPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _errorPanel.SetActive(false);
        }

        private void BuildCanvasUi()
        {
            var canvasObject = canvas != null ? canvas.gameObject : new GameObject("ProductionFlowCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            _safeArea = new GameObject("SafeArea", typeof(RectTransform), typeof(SafeAreaPanel));
            _safeArea.transform.SetParent(canvas.transform, false);
            var safeRect = _safeArea.GetComponent<RectTransform>();
            safeRect.anchorMin = Vector2.zero;
            safeRect.anchorMax = Vector2.one;
            safeRect.offsetMin = Vector2.zero;
            safeRect.offsetMax = Vector2.zero;

            _titleText = CreateText(_safeArea.transform, "Title", new Vector2(0.08f, 0.78f), new Vector2(0.92f, 0.94f), 42, TextAnchor.MiddleCenter);
            _messageText = CreateText(_safeArea.transform, "Message", new Vector2(0.12f, 0.70f), new Vector2(0.88f, 0.79f), 20, TextAnchor.MiddleCenter);

            _mainMenuPanel = CreatePanel(_safeArea.transform, "MainMenuPanel");
            CreateButton(_mainMenuPanel.transform, "Offline", "PLAY OFFLINE", new Vector2(0.28f, 0.53f), new Vector2(0.72f, 0.63f), OpenModeSelection);
            CreateButton(_mainMenuPanel.transform, "Online", "ONLINE (LOCKED)", new Vector2(0.28f, 0.40f), new Vector2(0.72f, 0.50f), SelectOnlineMode);
            CreateButton(_mainMenuPanel.transform, "Tutorial", "TUTORIAL REPLAY", new Vector2(0.28f, 0.27f), new Vector2(0.72f, 0.37f), OpenTutorial);
            CreateButton(_mainMenuPanel.transform, "Settings", "SETTINGS", new Vector2(0.28f, 0.14f), new Vector2(0.72f, 0.24f), OpenSettings);
            CreateButton(_mainMenuPanel.transform, "Quit", "FOCUS / POINTER HELP", new Vector2(0.28f, 0.01f), new Vector2(0.72f, 0.11f), ReleasePointerFocus);

            _modePanel = CreatePanel(_safeArea.transform, "ModePanel");
            CreateButton(_modePanel.transform, "Offline", "OFFLINE  •  1 HUMAN + 7 BOTS", new Vector2(0.18f, 0.54f), new Vector2(0.82f, 0.66f), SelectOfflineMode);
            CreateButton(_modePanel.transform, "Online", "ONLINE  •  UNAVAILABLE", new Vector2(0.18f, 0.39f), new Vector2(0.82f, 0.51f), SelectOnlineMode);
            CreateButton(_modePanel.transform, "Back", "BACK", new Vector2(0.32f, 0.16f), new Vector2(0.68f, 0.28f), ReturnToMenu);

            _fighterPanel = CreatePanel(_safeArea.transform, "FighterPanel");
            _fighterSummaryText = CreateText(_fighterPanel.transform, "FighterSummary", new Vector2(0.12f, 0.72f), new Vector2(0.88f, 0.84f), 22, TextAnchor.MiddleCenter);
            CreateButton(_fighterPanel.transform, "Bijli", "BIJLI  •  ELECTRIC DASH", new Vector2(0.10f, 0.52f), new Vector2(0.31f, 0.65f), SelectBijli);
            CreateButton(_fighterPanel.transform, "Pehel", "PEHEL  •  CHARGE THROW", new Vector2(0.395f, 0.52f), new Vector2(0.605f, 0.65f), SelectPehel);
            CreateButton(_fighterPanel.transform, "Maya", "MAYA  •  DECOY", new Vector2(0.69f, 0.52f), new Vector2(0.90f, 0.65f), SelectMaya);
            CreateButton(_fighterPanel.transform, "Start", "START OFFLINE MATCH", new Vector2(0.22f, 0.28f), new Vector2(0.78f, 0.41f), BeginOfflineMatch);
            CreateButton(_fighterPanel.transform, "Back", "BACK", new Vector2(0.32f, 0.10f), new Vector2(0.68f, 0.22f), OpenModeSelection);

            _loadingPanel = CreatePanel(_safeArea.transform, "LoadingPanel");
            _loadingText = CreateText(_loadingPanel.transform, "LoadingText", new Vector2(0.14f, 0.44f), new Vector2(0.86f, 0.58f), 26, TextAnchor.MiddleCenter);

            _settingsPanel = CreatePanel(_safeArea.transform, "SettingsPanel");
            _settingsSummaryText = CreateText(_settingsPanel.transform, "SettingsSummary", new Vector2(0.12f, 0.66f), new Vector2(0.88f, 0.90f), 20, TextAnchor.MiddleCenter);
            CreateButton(_settingsPanel.transform, "LeftHanded", "LEFT-HANDED", new Vector2(0.13f, 0.54f), new Vector2(0.43f, 0.64f), ToggleLeftHanded);
            CreateButton(_settingsPanel.transform, "Flashes", "REDUCED FLASHES", new Vector2(0.57f, 0.54f), new Vector2(0.87f, 0.64f), ToggleReducedFlashes);
            CreateButton(_settingsPanel.transform, "Contrast", "HIGH CONTRAST", new Vector2(0.13f, 0.42f), new Vector2(0.43f, 0.52f), ToggleHighContrast);
            CreateButton(_settingsPanel.transform, "AimAssist", "AIM ASSIST", new Vector2(0.57f, 0.42f), new Vector2(0.87f, 0.52f), ToggleAimAssist);
            CreateButton(_settingsPanel.transform, "MusicDown", "MUSIC -", new Vector2(0.13f, 0.30f), new Vector2(0.43f, 0.40f), DecreaseMusicVolume);
            CreateButton(_settingsPanel.transform, "MusicUp", "MUSIC +", new Vector2(0.57f, 0.30f), new Vector2(0.87f, 0.40f), IncreaseMusicVolume);
            CreateButton(_settingsPanel.transform, "EffectsDown", "EFFECTS -", new Vector2(0.13f, 0.18f), new Vector2(0.43f, 0.28f), DecreaseEffectsVolume);
            CreateButton(_settingsPanel.transform, "EffectsUp", "EFFECTS +", new Vector2(0.57f, 0.18f), new Vector2(0.87f, 0.28f), IncreaseEffectsVolume);
            CreateButton(_settingsPanel.transform, "Close", "CLOSE", new Vector2(0.32f, 0.05f), new Vector2(0.68f, 0.15f), CloseSettings);

            _errorPanel = CreatePanel(_safeArea.transform, "ErrorPanel");
            _errorText = CreateText(_errorPanel.transform, "ErrorText", new Vector2(0.12f, 0.51f), new Vector2(0.88f, 0.73f), 24, TextAnchor.MiddleCenter);
            CreateButton(_errorPanel.transform, "Retry", "RETRY", new Vector2(0.18f, 0.28f), new Vector2(0.48f, 0.40f), Retry);
            CreateButton(_errorPanel.transform, "Menu", "RETURN TO MENU", new Vector2(0.52f, 0.28f), new Vector2(0.82f, 0.40f), ReturnToMenu);

            SetAllPanelsInactive();
            ApplyContrast();
        }

        private void EnsureCanvas()
        {
            if (canvas != null) return;
            canvas = FindAnyObjectByType<Canvas>();
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;
            var eventObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            eventObject.GetComponent<EventSystem>().sendNavigationEvents = true;
        }

        private void LoadPreferences()
        {
            _selectedFighter = (ProductionFighter)Mathf.Clamp(PlayerPrefs.GetInt(SelectedFighterKey, (int)ProductionFighter.Bijli), 0, 2);
            _leftHanded = PlayerPrefs.GetInt(LeftHandedKey, 0) != 0;
            _reducedFlashes = PlayerPrefs.GetInt(ReducedFlashesKey, 0) != 0;
            _highContrast = PlayerPrefs.GetInt(HighContrastKey, 0) != 0;
            _aimAssist = PlayerPrefs.GetInt(AimAssistKey, 0) != 0;
            _musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.8f);
            _effectsVolume = PlayerPrefs.GetFloat(EffectsVolumeKey, 1f);
        }

        private void SavePreferences()
        {
            PlayerPrefs.SetInt(SelectedFighterKey, (int)_selectedFighter);
            PlayerPrefs.SetInt(LeftHandedKey, _leftHanded ? 1 : 0);
            PlayerPrefs.SetInt(ReducedFlashesKey, _reducedFlashes ? 1 : 0);
            PlayerPrefs.SetInt(HighContrastKey, _highContrast ? 1 : 0);
            PlayerPrefs.SetInt(AimAssistKey, _aimAssist ? 1 : 0);
            PlayerPrefs.SetFloat(MusicVolumeKey, _musicVolume);
            PlayerPrefs.SetFloat(EffectsVolumeKey, _effectsVolume);
            PlayerPrefs.Save();
        }

        private void RefreshFighterSummary()
        {
            if (_fighterSummaryText == null) return;
            _fighterSummaryText.text = $"SELECTED: {_selectedFighter.ToString().ToUpperInvariant()}\n\n{FighterDescription(_selectedFighter)}";
        }

        private void RefreshSettingsSummary()
        {
            if (_settingsSummaryText == null) return;
            _settingsSummaryText.text =
                $"LEFT-HANDED: {(_leftHanded ? "ON" : "OFF")}    REDUCED FLASHES: {(_reducedFlashes ? "ON" : "OFF")}\n" +
                $"HIGH CONTRAST: {(_highContrast ? "ON" : "OFF")}    AIM ASSIST: {(_aimAssist ? "ON" : "OFF")}\n" +
                $"MUSIC: {Mathf.RoundToInt(_musicVolume * 100f)}%    EFFECTS: {Mathf.RoundToInt(_effectsVolume * 100f)}%";
        }

        private void ApplyContrast()
        {
            if (_safeArea == null) return;
            var images = _safeArea.GetComponentsInChildren<Image>(true);
            for (var i = 0; i < images.Length; i++)
            {
                if (_highContrast)
                {
                    images[i].color = images[i].gameObject.name.EndsWith("Panel", StringComparison.Ordinal)
                        ? new Color(0f, 0f, 0f, 0.98f)
                        : Color.white;
                }
            }
        }

        private void SetHeader(string title, string message)
        {
            if (_titleText != null) _titleText.text = title;
            if (_messageText != null) _messageText.text = message;
        }

        private void ReleasePointerFocus()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private static string FormatError(string code)
        {
            switch (code)
            {
                case "ONLINE_UNAVAILABLE":
                    return "ONLINE PLAY IS LOCKED\nPhoton Fusion access/session configuration is not approved in this build.\nNo fake room or client-authoritative match is started.";
                case "MATCH_SCENE_UNAVAILABLE":
                    return "THE OFFLINE MATCH SCENE IS NOT IN BUILD SETTINGS.\nRebuild the development player with the production scene entrypoints.";
                case "MATCH_LOAD_FAILED":
                    return "THE OFFLINE MATCH COULD NOT LOAD.\nCheck the Unity player log and retry.";
                case "TUTORIAL_SCENE_UNAVAILABLE":
                    return "THE TUTORIAL SCENE IS NOT IN BUILD SETTINGS.\nRebuild the development player with the tutorial entrypoint.";
                case "TUTORIAL_LOAD_FAILED":
                    return "THE TUTORIAL COULD NOT LOAD.\nCheck the Unity player log and retry.";
                default:
                    return "AN UNEXPECTED FLOW ERROR OCCURRED.\nRetry or return to the main menu.";
            }
        }

        private static string FighterDescription(ProductionFighter fighter)
        {
            switch (fighter)
            {
                case ProductionFighter.Pehel: return "Charge through a lane, capture an enemy, and throw them back.";
                case ProductionFighter.Maya: return "Deploy a visible decoy to misdirect opponents and create space.";
                default: return "Dash with an electric burst and keep pressure at mid range.";
            }
        }

        private static GameObject CreatePanel(Transform parent, string name)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.08f, 0.06f);
            rect.anchorMax = new Vector2(0.92f, 0.68f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            panel.GetComponent<Image>().color = new Color(0.03f, 0.07f, 0.11f, 0.96f);
            return panel;
        }

        private static Text CreateText(Transform parent, string name, Vector2 min, Vector2 max, int size, TextAnchor alignment)
        {
            var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Text));
            objectToCreate.transform.SetParent(parent, false);
            var rect = objectToCreate.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = new Vector2(8f, 4f);
            rect.offsetMax = new Vector2(-8f, -4f);
            var text = objectToCreate.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.alignment = alignment;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        private static Button CreateButton(Transform parent, string name, string label, Vector2 min, Vector2 max, UnityEngine.Events.UnityAction action)
        {
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            buttonObject.GetComponent<Image>().color = new Color(0.08f, 0.28f, 0.35f, 0.96f);
            var text = CreateText(buttonObject.transform, name + "Label", Vector2.zero, Vector2.one, 18, TextAnchor.MiddleCenter);
            text.text = label;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(action);
            return button;
        }

        private static void SelectFirstButton(GameObject panel)
        {
            if (panel == null || EventSystem.current == null) return;
            var button = panel.GetComponentInChildren<Button>(true);
            if (button != null) EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }
}
