using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Visuals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleRaja.Presentation.Match
{
    /// <summary>
    /// Canvas-based gameplay HUD and pause/settings surface. The strings are deliberately
    /// short, stable keys can replace them with a localization table later, and all controls
    /// use anchors/CanvasScaler rather than immediate-mode screen coordinates.
    /// </summary>
    public sealed class OfflineMatchHud : MonoBehaviour
    {
        private const string MatchFormat = "MATCH {0}\nALIVE {1}  ZONE {2:0.0} > {3:0.0}{4}";
        private const string CompactMatchFormat = "{0}\nALIVE {1}  Z {2:0.0} > {3:0.0}{4}";

        [SerializeField] private OfflineMatchController match;
        [SerializeField] private bool showZoneOverlay = true;
        [SerializeField] private Canvas canvas;

        private Text _statusText;
        private Text _spectatorText;
        private GameObject _resultsPanel;
        private Text _resultsText;
        private GameObject _settingsPanel;
        private Button _aimAssistButton;
        private PlayerInputAdapter _playerInput;
        private BattleRajaAudioDirector _audio;
        private bool _highContrast;
        private bool _leftHanded;
        private bool _reducedFlashes;
        private bool _aimAssist;
        private bool _paused;
        private bool _compactLayout;

        private void Awake()
        {
            match = match != null ? match : FindFirstObjectByType<OfflineMatchController>();
            canvas = canvas != null ? canvas : FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                var canvasObject = new GameObject("ProductionHudCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                canvas = canvasObject.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                var scaler = canvasObject.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                scaler.matchWidthOrHeight = 0.5f;
            }

            _audio = FindFirstObjectByType<BattleRajaAudioDirector>();
            _playerInput = FindFirstObjectByType<MovementPlayerAgent>()?.GetComponent<PlayerInputAdapter>();
            LoadPreferences();
            _playerInput?.SetAimAssistEnabled(_aimAssist);
            BuildCanvasUi();
            ApplyHandedLayout();
            ApplyReducedFlashes();
        }

        private void OnDestroy()
        {
            if (_paused) Time.timeScale = 1f;
        }

        private void Update()
        {
            if (match == null) return;
            ApplyResponsiveLayout();
            var compact = _compactLayout;
            if (_statusText != null)
            {
                _statusText.gameObject.SetActive(showZoneOverlay);
                _statusText.text = FormatMatchStatus(match.CurrentPhase, match.AliveCount, match.ZoneRadius, match.NextZoneRadius, match.AandhiState, match.AandhiWarningRemainingSeconds, compact);
                _statusText.color = _highContrast ? Color.white : new Color(0.9f, 0.96f, 1f, 1f);
            }

            if (_spectatorText != null)
            {
                _spectatorText.gameObject.SetActive(match.PlayerSpectating);
                _spectatorText.text = "SPECTATING  •  tap SPECTATE to cycle";
            }

            if (_resultsPanel != null)
            {
                _resultsPanel.SetActive(match.ResultsShown);
                if (match.ResultsShown && match.Results != null)
                {
                    var winner = default(MatchParticipantSnapshot);
                    var hasWinner = false;
                    for (var i = 0; i < match.Results.Length; i++)
                    {
                        if (match.Results[i].Placement != 1) continue;
                        winner = match.Results[i];
                        hasWinner = true;
                        break;
                    }

                    _resultsText.text = hasWinner
                        ? $"RESULTS\nWINNER {winner.Id.Value}\nKOs {winner.Eliminations}   ASSISTS {winner.Assists}\nDAMAGE {winner.DamageDealt}   SURVIVAL {winner.SurvivalTimeSeconds:0.0}s"
                        : "RESULTS";
                }
            }
        }

        public static string FormatMatchStatus(MatchPhase phase, int aliveCount, float zoneRadius, float nextZoneRadius, AandhiState aandhiState, float warningRemainingSeconds, bool compact)
        {
            var warning = aandhiState == AandhiState.Warning
                ? $"  WARN {warningRemainingSeconds:0.0}s"
                : aandhiState == AandhiState.Closing ? "  CLOSE" : string.Empty;
            var format = compact ? CompactMatchFormat : MatchFormat;
            return string.Format(format, phase.ToString().ToUpperInvariant(), aliveCount, zoneRadius, nextZoneRadius, warning);
        }

        private void BuildCanvasUi()
        {
            var root = new GameObject("ProductionHudRoot", typeof(RectTransform));
            root.transform.SetParent(canvas.transform, false);
            var rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            _statusText = CreateText(root.transform, "MatchStatus", new Vector2(0.42f, 0.94f), new Vector2(0.84f, 0.99f), 24, TextAnchor.UpperLeft);
            _spectatorText = CreateText(root.transform, "SpectatorStatus", new Vector2(0.42f, 0.88f), new Vector2(0.84f, 0.93f), 22, TextAnchor.UpperLeft);
            CreateButton(root.transform, "Pause", "PAUSE", new Vector2(0.86f, 0.92f), new Vector2(0.98f, 0.985f), ToggleSettings);
            CreateButton(root.transform, "Spectate", "SPECTATE", new Vector2(0.38f, 0.02f), new Vector2(0.52f, 0.085f), CycleSpectator);

            _resultsPanel = CreatePanel(root.transform, "ResultsPanel", new Vector2(0.27f, 0.30f), new Vector2(0.73f, 0.72f), new Color(0.04f, 0.08f, 0.13f, 0.94f));
            _resultsText = CreateText(_resultsPanel.transform, "ResultsText", new Vector2(0.08f, 0.40f), new Vector2(0.92f, 0.92f), 28, TextAnchor.MiddleCenter);
            CreateButton(_resultsPanel.transform, "Rematch", "REMATCH", new Vector2(0.08f, 0.06f), new Vector2(0.48f, 0.22f), Rematch);
            CreateButton(_resultsPanel.transform, "Menu", "MENU", new Vector2(0.52f, 0.06f), new Vector2(0.92f, 0.22f), ReturnToMenu);
            _resultsPanel.SetActive(false);

            _settingsPanel = CreatePanel(root.transform, "SettingsPanel", new Vector2(0.58f, 0.16f), new Vector2(0.96f, 0.86f), new Color(0.03f, 0.06f, 0.1f, 0.97f));
            CreateText(_settingsPanel.transform, "SettingsTitle", new Vector2(0.08f, 0.84f), new Vector2(0.92f, 0.96f), 26, TextAnchor.MiddleCenter).text = "SETTINGS";
            CreateButton(_settingsPanel.transform, "CloseSettings", "CLOSE", new Vector2(0.52f, 0.04f), new Vector2(0.92f, 0.16f), ToggleSettings);
            CreateButton(_settingsPanel.transform, "ReturnToMenu", "RETURN TO MENU", new Vector2(0.08f, 0.04f), new Vector2(0.48f, 0.16f), ReturnToMenu);
            CreateButton(_settingsPanel.transform, "LeftHanded", "LEFT-HANDED", new Vector2(0.08f, 0.65f), new Vector2(0.92f, 0.76f), ToggleLeftHanded);
            CreateButton(_settingsPanel.transform, "ReducedFlashes", "REDUCED FLASHES", new Vector2(0.08f, 0.50f), new Vector2(0.92f, 0.61f), ToggleReducedFlashes);
            CreateButton(_settingsPanel.transform, "HighContrast", "HIGH CONTRAST", new Vector2(0.08f, 0.35f), new Vector2(0.92f, 0.46f), ToggleHighContrast);
            _aimAssistButton = CreateButton(_settingsPanel.transform, "AimAssist", "AIM ASSIST", new Vector2(0.08f, 0.20f), new Vector2(0.92f, 0.31f), ToggleAimAssist);
            _settingsPanel.SetActive(false);
            RefreshAimAssistLabel();
            ApplyResponsiveLayout();
        }

        private void ApplyResponsiveLayout()
        {
            if (_statusText == null || _spectatorText == null || Screen.height <= 0) return;
            var compact = (float)Screen.width / Screen.height < 0.75f;
            if (compact == _compactLayout && _statusText.fontSize == (compact ? 18 : 22)) return;

            _compactLayout = compact;
            var statusRect = _statusText.rectTransform;
            statusRect.anchorMin = compact ? new Vector2(0.42f, 0.90f) : new Vector2(0.42f, 0.94f);
            statusRect.anchorMax = new Vector2(0.98f, 0.99f);
            _statusText.fontSize = compact ? 18 : 22;

            var spectatorRect = _spectatorText.rectTransform;
            spectatorRect.anchorMin = compact ? new Vector2(0.42f, 0.84f) : new Vector2(0.42f, 0.88f);
            spectatorRect.anchorMax = compact ? new Vector2(0.98f, 0.89f) : new Vector2(0.98f, 0.93f);
            _spectatorText.fontSize = compact ? 16 : 20;
        }

        private void ToggleSettings()
        {
            if (_settingsPanel == null) return;
            _settingsPanel.SetActive(!_settingsPanel.activeSelf);
            _paused = _settingsPanel.activeSelf;
            Time.timeScale = _paused ? 0f : 1f;
            _audio?.StartFromUserGesture();
        }

        private void CycleSpectator()
        {
            if (match != null && match.PlayerSpectating) match.CycleSpectator();
        }

        private void Rematch()
        {
            match?.RestartMatch();
        }

        private void ReturnToMenu()
        {
            Time.timeScale = 1f;
            _paused = false;
            SceneManager.LoadScene("Bootstrap", LoadSceneMode.Single);
        }

        private void ToggleLeftHanded()
        {
            _leftHanded = !_leftHanded;
            PlayerPrefs.SetInt("battleraja.settings.left_handed", _leftHanded ? 1 : 0);
            PlayerPrefs.Save();
            ApplyHandedLayout();
        }

        private void ApplyHandedLayout()
        {
            var movement = GameObject.Find("MovementStick")?.GetComponent<RectTransform>();
            var aim = GameObject.Find("AimStick")?.GetComponent<RectTransform>();
            if (movement == null || aim == null) return;
            movement.anchorMin = _leftHanded ? new Vector2(0.83f, 0.2f) : new Vector2(0.17f, 0.2f);
            movement.anchorMax = movement.anchorMin;
            aim.anchorMin = _leftHanded ? new Vector2(0.17f, 0.2f) : new Vector2(0.83f, 0.2f);
            aim.anchorMax = aim.anchorMin;
        }

        private void ToggleReducedFlashes()
        {
            _reducedFlashes = !_reducedFlashes;
            PlayerPrefs.SetInt("battleraja.settings.reduced_flashes", _reducedFlashes ? 1 : 0);
            PlayerPrefs.Save();
            ApplyReducedFlashes();
        }

        private void ApplyReducedFlashes()
        {
            foreach (var presentation in FindObjectsByType<FighterPresentation>(FindObjectsSortMode.None))
            {
                presentation.ReducedFlashMode = _reducedFlashes;
            }
        }

        private void ToggleHighContrast()
        {
            _highContrast = !_highContrast;
            PlayerPrefs.SetInt("battleraja.settings.high_contrast", _highContrast ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void ToggleAimAssist()
        {
            _aimAssist = !_aimAssist;
            PlayerPrefs.SetInt("battleraja.settings.aim_assist", _aimAssist ? 1 : 0);
            PlayerPrefs.Save();
            _playerInput?.SetAimAssistEnabled(_aimAssist);
            RefreshAimAssistLabel();
        }

        private void RefreshAimAssistLabel()
        {
            if (_aimAssistButton == null) return;
            var label = _aimAssistButton.GetComponentInChildren<Text>();
            if (label != null) label.text = _aimAssist ? "AIM ASSIST  ON" : "AIM ASSIST  OFF";
        }

        private void LoadPreferences()
        {
            _leftHanded = PlayerPrefs.GetInt("battleraja.settings.left_handed", 0) != 0;
            _reducedFlashes = PlayerPrefs.GetInt("battleraja.settings.reduced_flashes", 0) != 0;
            _highContrast = PlayerPrefs.GetInt("battleraja.settings.high_contrast", 0) != 0;
            _aimAssist = PlayerPrefs.GetInt("battleraja.settings.aim_assist", 0) != 0;
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 min, Vector2 max, Color color)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            panel.GetComponent<Image>().color = color;
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
            buttonObject.GetComponent<Image>().color = new Color(0.08f, 0.23f, 0.31f, 0.94f);
            var text = CreateText(buttonObject.transform, name + "Label", Vector2.zero, Vector2.one, 18, TextAnchor.MiddleCenter);
            text.text = label;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(action);
            return button;
        }
    }
}
