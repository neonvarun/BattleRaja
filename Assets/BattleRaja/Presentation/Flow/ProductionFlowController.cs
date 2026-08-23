using System;
using System.Collections;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.UI;
using BattleRaja.Presentation.Visuals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
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
        private const string TextScaleKey = "battleraja.settings.text_scale";
        private const string HapticsKey = "battleraja.settings.haptics";

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
        private float _textScale;
        private bool _haptics;
        private float _appliedTextScale = 1f;
        private bool _loading;
        private BattleRajaAudioDirector _audio;
        private bool _enhancedTouchEnabled;

        private GameObject _safeArea;
        private BattleRajaUiBackdrop _backdrop;
        private BattleRajaLogoGraphic _logo;
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
            // Explicitly enable the native touch event stream for Android. The UI
            // module still owns routing, while EnhancedTouch keeps physical touch
            // devices active when the bootstrap EventSystem is created at runtime.
            EnhancedTouchSupport.Enable();
            _enhancedTouchEnabled = true;
            EnsureEventSystem();
            EnsureCanvas();
            EnsureAudioDirector();
            BuildCanvasUi();
            ApplyAudioPreferences();
            ApplyTextScale();
        }

        private void OnDestroy()
        {
            if (!_enhancedTouchEnabled) return;
            EnhancedTouchSupport.Disable();
            _enhancedTouchEnabled = false;
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
            ApplyAudioPreferences();
            RefreshSettingsSummary();
        }

        public void DecreaseMusicVolume()
        {
            _musicVolume = Mathf.Clamp01(_musicVolume - 0.1f);
            SavePreferences();
            ApplyAudioPreferences();
            RefreshSettingsSummary();
        }

        public void IncreaseEffectsVolume()
        {
            _effectsVolume = Mathf.Clamp01(_effectsVolume + 0.1f);
            SavePreferences();
            ApplyAudioPreferences();
            RefreshSettingsSummary();
        }

        public void DecreaseEffectsVolume()
        {
            _effectsVolume = Mathf.Clamp01(_effectsVolume - 0.1f);
            SavePreferences();
            ApplyAudioPreferences();
            RefreshSettingsSummary();
        }

        public void IncreaseTextScale()
        {
            _textScale = Mathf.Clamp(_textScale + 0.1f, 0.9f, 1.3f);
            SavePreferences();
            ApplyTextScale();
            RefreshSettingsSummary();
        }

        public void DecreaseTextScale()
        {
            _textScale = Mathf.Clamp(_textScale - 0.1f, 0.9f, 1.3f);
            SavePreferences();
            ApplyTextScale();
            RefreshSettingsSummary();
        }

        public void ToggleHaptics()
        {
            _haptics = !_haptics;
            BattleRajaHaptics.Enabled = _haptics;
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
                    SetHeader("BATTLE RAJA", "SOLO RAJA  •  1 RAJA + 7 RIVALS  •  NO INTERNET REQUIRED");
                    break;
                case ProductionFlowState.ModeSelection:
                    _modePanel.SetActive(true);
                    SetHeader("SOLO RAJA", "Choose your fighter, then drop into Bazaar Bastion.");
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

            var backdropObject = new GameObject("BattleRajaBackdrop", typeof(RectTransform), typeof(BattleRajaUiBackdrop));
            backdropObject.transform.SetParent(_safeArea.transform, false);
            var backdropRect = backdropObject.GetComponent<RectTransform>();
            backdropRect.anchorMin = Vector2.zero;
            backdropRect.anchorMax = Vector2.one;
            backdropRect.offsetMin = Vector2.zero;
            backdropRect.offsetMax = Vector2.zero;
            _backdrop = backdropObject.GetComponent<BattleRajaUiBackdrop>();
            backdropObject.transform.SetAsFirstSibling();

            var logoObject = new GameObject("BattleRajaMark", typeof(RectTransform), typeof(BattleRajaLogoGraphic));
            logoObject.transform.SetParent(_safeArea.transform, false);
            var logoRect = logoObject.GetComponent<RectTransform>();
            logoRect.anchorMin = new Vector2(0.42f, 0.80f);
            logoRect.anchorMax = new Vector2(0.58f, 0.96f);
            logoRect.offsetMin = Vector2.zero;
            logoRect.offsetMax = Vector2.zero;
            _logo = logoObject.GetComponent<BattleRajaLogoGraphic>();

            _titleText = CreateText(_safeArea.transform, "Title", new Vector2(0.08f, 0.69f), new Vector2(0.92f, 0.79f), 42, TextAnchor.MiddleCenter);
            _messageText = CreateText(_safeArea.transform, "Message", new Vector2(0.10f, 0.62f), new Vector2(0.90f, 0.70f), 20, TextAnchor.MiddleCenter);
            var eyebrow = CreateText(_safeArea.transform, "Eyebrow", new Vector2(0.18f, 0.965f), new Vector2(0.82f, 0.995f), 12, TextAnchor.MiddleCenter, BattleRajaUiTheme.Cyan, true);
            eyebrow.text = "OFFLINE ARCADE  •  BAZAAR BASTION";
            var version = CreateText(_safeArea.transform, "Version", new Vector2(0.04f, 0.015f), new Vector2(0.52f, 0.055f), 14, TextAnchor.MiddleLeft, BattleRajaUiTheme.MutedText);
            version.text = "OFFLINE V1.0 CANDIDATE  •  8-ACTOR MATCHES";

            _mainMenuPanel = CreatePanel(_safeArea.transform, "MainMenuPanel");
            CreateText(_mainMenuPanel.transform, "LoopSummary", new Vector2(0.12f, 0.70f), new Vector2(0.88f, 0.88f), 17, TextAnchor.MiddleCenter, BattleRajaUiTheme.MutedText).text =
                "1 RAJA  •  7 RIVALS\nREAD THE ZONE  •  GRAB A GADGET  •  SURVIVE";
            var heroObject = new GameObject("HeroIllustration", typeof(RectTransform), typeof(BattleRajaHeroGraphic));
            heroObject.transform.SetParent(_mainMenuPanel.transform, false);
            var heroRect = heroObject.GetComponent<RectTransform>();
            heroRect.anchorMin = new Vector2(0.10f, 0.48f);
            heroRect.anchorMax = new Vector2(0.90f, 0.69f);
            heroRect.offsetMin = Vector2.zero;
            heroRect.offsetMax = Vector2.zero;
            CreateButton(_mainMenuPanel.transform, "Offline", "PLAY OFFLINE", new Vector2(0.28f, 0.42f), new Vector2(0.72f, 0.52f), OpenModeSelection);
            CreateButton(_mainMenuPanel.transform, "Tutorial", "TUTORIAL REPLAY", new Vector2(0.28f, 0.30f), new Vector2(0.72f, 0.40f), OpenTutorial);
            CreateButton(_mainMenuPanel.transform, "Settings", "SETTINGS & ACCESSIBILITY", new Vector2(0.28f, 0.18f), new Vector2(0.72f, 0.28f), OpenSettings);
            CreateButton(_mainMenuPanel.transform, "Help", "HELP & CONTROLS", new Vector2(0.28f, 0.06f), new Vector2(0.72f, 0.16f), OpenTutorial);

            _modePanel = CreatePanel(_safeArea.transform, "ModePanel");
            CreateButton(_modePanel.transform, "Offline", "DROP IN  •  1 RAJA + 7 BOTS", new Vector2(0.18f, 0.49f), new Vector2(0.82f, 0.63f), SelectOfflineMode, true);
            CreateText(_modePanel.transform, "ModeHint", new Vector2(0.14f, 0.33f), new Vector2(0.86f, 0.46f), 18, TextAnchor.MiddleCenter).text = "Every match is deterministic, replayable and playable without an account.";
            CreateButton(_modePanel.transform, "Back", "BACK TO MENU", new Vector2(0.32f, 0.14f), new Vector2(0.68f, 0.26f), ReturnToMenu);

            _fighterPanel = CreatePanel(_safeArea.transform, "FighterPanel");
            _fighterSummaryText = CreateText(_fighterPanel.transform, "FighterSummary", new Vector2(0.12f, 0.72f), new Vector2(0.88f, 0.84f), 22, TextAnchor.MiddleCenter);
            var bijliButton = CreateButton(_fighterPanel.transform, "Bijli", "BIJLI\nELECTRIC DASH", new Vector2(0.08f, 0.49f), new Vector2(0.31f, 0.65f), SelectBijli);
            var pehelButton = CreateButton(_fighterPanel.transform, "Pehel", "PEHEL\nCHARGE THROW", new Vector2(0.385f, 0.49f), new Vector2(0.615f, 0.65f), SelectPehel);
            var mayaButton = CreateButton(_fighterPanel.transform, "Maya", "MAYA\nDECOY", new Vector2(0.69f, 0.49f), new Vector2(0.92f, 0.65f), SelectMaya);
            AddFighterCardArt(bijliButton, BattleRajaFighterCardKind.Bijli);
            AddFighterCardArt(pehelButton, BattleRajaFighterCardKind.Pehel);
            AddFighterCardArt(mayaButton, BattleRajaFighterCardKind.Maya);
            BattleRajaUiTheme.StyleButton(bijliButton, BattleRajaUiTheme.Cyan);
            BattleRajaUiTheme.StyleButton(pehelButton, BattleRajaUiTheme.Saffron);
            BattleRajaUiTheme.StyleButton(mayaButton, BattleRajaUiTheme.Magenta);
            CreateText(_fighterPanel.transform, "BijliHint", new Vector2(0.08f, 0.42f), new Vector2(0.31f, 0.49f), 13, TextAnchor.MiddleCenter, BattleRajaUiTheme.MutedText).text = "BURST • MID-RANGE";
            CreateText(_fighterPanel.transform, "PehelHint", new Vector2(0.385f, 0.42f), new Vector2(0.615f, 0.49f), 13, TextAnchor.MiddleCenter, BattleRajaUiTheme.MutedText).text = "CAPTURE • THROW";
            CreateText(_fighterPanel.transform, "MayaHint", new Vector2(0.69f, 0.42f), new Vector2(0.92f, 0.49f), 13, TextAnchor.MiddleCenter, BattleRajaUiTheme.MutedText).text = "DECOY • MISDIRECT";
            CreateButton(_fighterPanel.transform, "Start", "START SOLO RAJA", new Vector2(0.22f, 0.28f), new Vector2(0.78f, 0.41f), BeginOfflineMatch, true);
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
            CreateButton(_settingsPanel.transform, "TextDown", "TEXT -", new Vector2(0.08f, 0.06f), new Vector2(0.30f, 0.16f), DecreaseTextScale);
            CreateButton(_settingsPanel.transform, "Haptics", "HAPTICS", new Vector2(0.35f, 0.06f), new Vector2(0.65f, 0.16f), ToggleHaptics);
            CreateButton(_settingsPanel.transform, "TextUp", "TEXT +", new Vector2(0.70f, 0.06f), new Vector2(0.92f, 0.16f), IncreaseTextScale);
            CreateButton(_settingsPanel.transform, "Close", "CLOSE", new Vector2(0.32f, 0.005f), new Vector2(0.68f, 0.055f), CloseSettings);

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

        private void EnsureAudioDirector()
        {
            _audio = FindAnyObjectByType<BattleRajaAudioDirector>();
            if (_audio != null) return;

            var audioObject = new GameObject("AudioDirector");
            audioObject.transform.SetParent(transform, false);
            audioObject.AddComponent<AudioSource>();
            _audio = audioObject.AddComponent<BattleRajaAudioDirector>();
        }

        private static void EnsureEventSystem()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
            {
                var eventObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
                eventSystem = eventObject.GetComponent<EventSystem>();
            }

            // Bootstrap and older authored scenes can still contain the legacy
            // StandaloneInputModule. The project is Input System-only, so leave one
            // EventSystem in place but replace the legacy module before the first
            // interactive frame. This is required for physical Android touch input.
            var legacy = eventSystem.GetComponent<StandaloneInputModule>();
            if (legacy != null)
            {
                legacy.enabled = false;
                Destroy(legacy);
            }

            var modern = eventSystem.GetComponent<InputSystemUIInputModule>();
            if (modern == null)
            {
                modern = eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            }

            modern.enabled = true;
            eventSystem.sendNavigationEvents = true;
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
            _textScale = Mathf.Clamp(PlayerPrefs.GetFloat(TextScaleKey, 1f), 0.9f, 1.3f);
            _haptics = PlayerPrefs.GetInt(HapticsKey, 1) != 0;
            BattleRajaHaptics.Enabled = _haptics;
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
            PlayerPrefs.SetFloat(TextScaleKey, _textScale);
            PlayerPrefs.SetInt(HapticsKey, _haptics ? 1 : 0);
            PlayerPrefs.Save();
            ApplyAudioPreferences();
        }

        private void ApplyAudioPreferences()
        {
            if (_audio == null) return;
            _audio.SetMusicVolume(_musicVolume);
            _audio.SetEffectsVolume(_effectsVolume);
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
                $"MUSIC: {Mathf.RoundToInt(_musicVolume * 100f)}%    EFFECTS: {Mathf.RoundToInt(_effectsVolume * 100f)}%\n" +
                $"TEXT SIZE: {Mathf.RoundToInt(_textScale * 100f)}%    HAPTICS: {(_haptics ? "ON" : "OFF")}";
        }

        private void ApplyTextScale()
        {
            if (_safeArea == null || _appliedTextScale <= 0.001f) return;
            var ratio = _textScale / _appliedTextScale;
            foreach (var text in _safeArea.GetComponentsInChildren<Text>(true))
            {
                if (text == null || text == _settingsSummaryText) continue;
                text.fontSize = Mathf.Clamp(Mathf.RoundToInt(text.fontSize * ratio), 12, 64);
            }

            _appliedTextScale = _textScale;
        }

        private void ApplyContrast()
        {
            if (_safeArea == null) return;
            BattleRajaUiTheme.ApplyContrast(_safeArea.transform, _highContrast);
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
            BattleRajaUiTheme.StylePanel(panel);
            return panel;
        }

        private static Text CreateText(Transform parent, string name, Vector2 min, Vector2 max, int size, TextAnchor alignment, Color? color = null, bool bold = false)
        {
            var objectToCreate = new GameObject(name, typeof(RectTransform), typeof(Text));
            objectToCreate.transform.SetParent(parent, false);
            var rect = objectToCreate.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = new Vector2(8f, 4f);
            rect.offsetMax = new Vector2(-8f, -4f);
            var text = objectToCreate.GetComponent<Text>();
            BattleRajaUiTheme.StyleText(text, size, alignment, color, bold);
            return text;
        }

        private static void AddFighterCardArt(Button button, BattleRajaFighterCardKind fighter)
        {
            if (button == null) return;
            var label = button.GetComponentInChildren<Text>(true);
            if (label != null)
            {
                var labelRect = label.rectTransform;
                labelRect.anchorMin = new Vector2(0.04f, 0.02f);
                labelRect.anchorMax = new Vector2(0.96f, 0.34f);
                labelRect.offsetMin = new Vector2(4f, 2f);
                labelRect.offsetMax = new Vector2(-4f, -2f);
                label.fontSize = 14;
            }

            var artObject = new GameObject("FighterGlyph", typeof(RectTransform), typeof(BattleRajaFighterCardGraphic));
            artObject.transform.SetParent(button.transform, false);
            artObject.transform.SetAsFirstSibling();
            var artRect = artObject.GetComponent<RectTransform>();
            artRect.anchorMin = new Vector2(0.08f, 0.28f);
            artRect.anchorMax = new Vector2(0.92f, 0.98f);
            artRect.offsetMin = Vector2.zero;
            artRect.offsetMax = Vector2.zero;
            artObject.GetComponent<BattleRajaFighterCardGraphic>().SetFighter(fighter);
        }

        private static Button CreateButton(Transform parent, string name, string label, Vector2 min, Vector2 max, UnityEngine.Events.UnityAction action, bool primary = false)
        {
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var text = CreateText(buttonObject.transform, name + "Label", Vector2.zero, Vector2.one, 18, TextAnchor.MiddleCenter);
            text.text = label;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                BattleRajaHaptics.Pulse();
                action?.Invoke();
            });
            BattleRajaUiTheme.StyleButton(button, primary);
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
