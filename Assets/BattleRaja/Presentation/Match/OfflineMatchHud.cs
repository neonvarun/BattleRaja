using System;
using System.Collections.Generic;
using System.Text;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.UI;
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
        private const string CompactMatchFormat = "{0}\nALIVE {1}  ZONE {2:0.0} > {3:0.0}{4}";

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
        private float _textScale;
        private float _appliedTextScale = 1f;
        private bool _paused;
        private bool _lifecyclePaused;
        private bool _compactLayout;
        private bool _controlsCompactLayout;
        private bool _controlsLayoutInitialized;
        private AandhiState _lastAandhiState = AandhiState.Stable;
        private MatchPhase _lastMatchPhase = MatchPhase.LoadWarmup;
        private bool _resultsCuePlayed;
        private AandhiZoneVisual _aandhiVisual;
        private bool _hasStatusKey;
        private MatchPhase _lastStatusPhase;
        private int _lastStatusAliveCount;
        private int _lastStatusZoneRadiusTenth;
        private int _lastStatusNextZoneRadiusTenth;
        private AandhiState _lastStatusAandhiState;
        private int _lastStatusWarningTenth;
        private bool _lastStatusCompact;
        private bool _lastSpectating;
        private bool _hasSpectatingState;
        private MatchParticipantSnapshot[] _lastResultsReference;
        private bool _lastResultsCompact;

        private void Awake()
        {
            match = match != null ? match : FindAnyObjectByType<OfflineMatchController>();
            _aandhiVisual = GetComponent<AandhiZoneVisual>();
            if (_aandhiVisual == null) _aandhiVisual = gameObject.AddComponent<AandhiZoneVisual>();
            canvas = canvas != null ? canvas : FindAnyObjectByType<Canvas>();
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

            _audio = FindAnyObjectByType<BattleRajaAudioDirector>();
            EnsureTouchSurfaces();
            var movementAgents = FindObjectsByType<MovementPlayerAgent>();
            for (var i = 0; i < movementAgents.Length; i++)
            {
                if (movementAgents[i].ActorId != 1) continue;
                _playerInput = movementAgents[i].GetComponent<PlayerInputAdapter>();
                break;
            }
            LoadPreferences();
            _lastAandhiState = match != null ? match.AandhiState : AandhiState.Stable;
            _lastMatchPhase = match != null ? match.CurrentPhase : MatchPhase.LoadWarmup;
            _playerInput?.SetAimAssistEnabled(_aimAssist);
            BuildCanvasUi();
            ApplyHandedLayout();
            ApplyReducedFlashes();
            ApplyTextScale();
            BattleRajaUiTheme.ApplyContrast(canvas != null ? canvas.transform : null, _highContrast);
        }

        private static void EnsureTouchSurfaces()
        {
            EnsureTouchSurface("MovementStick", new Color(0.25f, 0.70f, 1f, 0.18f), true);
            EnsureTouchSurface("AimStick", new Color(1f, 0.64f, 0.22f, 0.18f), true);
            EnsureTouchSurface("AttackButton", new Color(1f, 0.36f, 0.18f, 0.28f), true);
            EnsureTouchSurface("AbilityButton", new Color(0.36f, 0.78f, 1f, 0.30f), true);
            EnsureTouchSurface("GadgetButton", new Color(0.72f, 0.32f, 0.95f, 0.30f), true);

            var movement = GameObject.Find("MovementStick");
            var aim = GameObject.Find("AimStick");
            EnsureTouchSurface(movement != null ? "MovementStick/Knob" : string.Empty, new Color(0.25f, 0.70f, 1f, 0.72f), false);
            EnsureTouchSurface(aim != null ? "AimStick/Knob" : string.Empty, new Color(1f, 0.64f, 0.22f, 0.72f), false);
        }

        private static void EnsureTouchSurface(string objectPath, Color color, bool ring)
        {
            if (string.IsNullOrEmpty(objectPath)) return;
            var objectToStyle = GameObject.Find(objectPath);
            if (objectToStyle == null) return;

            var directSurface = objectToStyle.GetComponent<BattleRajaTouchSurface>();
            if (directSurface != null)
            {
                directSurface.Configure(color, ring);
                directSurface.raycastTarget = true;
                return;
            }

            var legacyImage = objectToStyle.GetComponent<Image>();
            if (legacyImage == null)
            {
                var newSurface = objectToStyle.AddComponent<BattleRajaTouchSurface>();
                newSurface.Configure(color, ring);
                newSurface.raycastTarget = true;
                return;
            }

            // Unity defers Destroy(Image) until the end of the frame and rejects a
            // second Graphic on the same GameObject in the meantime. Keep the
            // authored Image as a transparent, non-raycast fallback and render the
            // circular treatment on a child overlay instead. Pointer events still
            // bubble to the authored control component on the parent.
            legacyImage.enabled = false;
            legacyImage.color = new Color(legacyImage.color.r, legacyImage.color.g, legacyImage.color.b, 0f);
            legacyImage.raycastTarget = false;

            var visualObject = objectToStyle.transform.Find("BattleRajaTouchSurface");
            if (visualObject == null)
            {
                visualObject = new GameObject("BattleRajaTouchSurface", typeof(RectTransform)).transform;
                visualObject.SetParent(objectToStyle.transform, false);
                visualObject.SetAsLastSibling();
            }

            var visualRect = visualObject as RectTransform;
            if (visualRect != null)
            {
                visualRect.anchorMin = Vector2.zero;
                visualRect.anchorMax = Vector2.one;
                visualRect.offsetMin = Vector2.zero;
                visualRect.offsetMax = Vector2.zero;
                visualRect.localScale = Vector3.one;
            }

            var surface = visualObject.GetComponent<BattleRajaTouchSurface>() ?? visualObject.gameObject.AddComponent<BattleRajaTouchSurface>();
            surface.Configure(color, ring);
            surface.raycastTarget = true;
        }

        private void OnDestroy()
        {
            if (_paused) Time.timeScale = 1f;
        }

        private void OnApplicationPause(bool paused)
        {
            SetLifecyclePause(paused);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetLifecyclePause(!hasFocus);
        }

        private void Update()
        {
            if (match == null) return;
            UpdateAudioCues();
            ApplyResponsiveLayout();
            var compactControls = Screen.height > 0 && (float)Screen.width / Screen.height < 0.75f;
            if (!_controlsLayoutInitialized || compactControls != _controlsCompactLayout)
            {
                ApplyHandedLayout();
            }
            var compact = _compactLayout;
            if (_statusText != null)
            {
                _statusText.gameObject.SetActive(showZoneOverlay);
                var phase = match.CurrentPhase;
                var aliveCount = match.AliveCount;
                var zoneRadiusTenth = Mathf.RoundToInt(match.ZoneRadius * 10f);
                var nextZoneRadiusTenth = Mathf.RoundToInt(match.NextZoneRadius * 10f);
                var warningTenth = Mathf.RoundToInt(match.AandhiWarningRemainingSeconds * 10f);
                var aandhiState = match.AandhiState;
                if (!_hasStatusKey ||
                    phase != _lastStatusPhase ||
                    aliveCount != _lastStatusAliveCount ||
                    zoneRadiusTenth != _lastStatusZoneRadiusTenth ||
                    nextZoneRadiusTenth != _lastStatusNextZoneRadiusTenth ||
                    aandhiState != _lastStatusAandhiState ||
                    warningTenth != _lastStatusWarningTenth ||
                    compact != _lastStatusCompact)
                {
                    _statusText.text = FormatMatchStatus(
                        phase,
                        aliveCount,
                        match.ZoneRadius,
                        match.NextZoneRadius,
                        aandhiState,
                        match.AandhiWarningRemainingSeconds,
                        compact);
                    _hasStatusKey = true;
                    _lastStatusPhase = phase;
                    _lastStatusAliveCount = aliveCount;
                    _lastStatusZoneRadiusTenth = zoneRadiusTenth;
                    _lastStatusNextZoneRadiusTenth = nextZoneRadiusTenth;
                    _lastStatusAandhiState = aandhiState;
                    _lastStatusWarningTenth = warningTenth;
                    _lastStatusCompact = compact;
                }
                _statusText.color = _highContrast ? Color.white : new Color(0.9f, 0.96f, 1f, 1f);
            }

            if (_spectatorText != null)
            {
                var spectating = match.PlayerSpectating;
                _spectatorText.gameObject.SetActive(spectating);
                if (!_hasSpectatingState || spectating != _lastSpectating)
                {
                    _spectatorText.text = "SPECTATING  •  tap SPECTATE to cycle";
                    _lastSpectating = spectating;
                    _hasSpectatingState = true;
                }
            }

            if (_resultsPanel != null)
            {
                var resultsShown = match.ResultsShown;
                var results = match.Results;
                _resultsPanel.SetActive(resultsShown);
                if (resultsShown && results != null &&
                    (results != _lastResultsReference || compact != _lastResultsCompact))
                {
                    _resultsText.text = FormatResults(results, compact);
                    _lastResultsReference = results;
                    _lastResultsCompact = compact;
                }
                else if (!resultsShown)
                {
                    _lastResultsReference = null;
                }
            }
        }

        private void UpdateAudioCues()
        {
            var matchPhase = match.CurrentPhase;
            if (matchPhase != _lastMatchPhase)
            {
                if (matchPhase == MatchPhase.FinalCircle) _audio?.PlayZoneFinalCircle();
                _lastMatchPhase = matchPhase;
            }

            var aandhiState = match.AandhiState;
            if (aandhiState != _lastAandhiState)
            {
                if (aandhiState == AandhiState.Warning) _audio?.PlayZoneWarning();
                else if (aandhiState == AandhiState.Closing) _audio?.PlayZoneClosing();
                _lastAandhiState = aandhiState;
            }

            if (!match.ResultsShown)
            {
                _resultsCuePlayed = false;
                return;
            }

            if (_resultsCuePlayed || match.Results == null) return;
            _resultsCuePlayed = true;
            var playerWon = false;
            for (var i = 0; i < match.Results.Length; i++)
            {
                if (match.Results[i].Id.Value == 1)
                {
                    playerWon = match.Results[i].Placement == 1;
                    break;
                }
            }

            if (playerWon) _audio?.PlayVictory();
            else _audio?.PlayDefeat();
        }

        public static string FormatMatchStatus(MatchPhase phase, int aliveCount, float zoneRadius, float nextZoneRadius, AandhiState aandhiState, float warningRemainingSeconds, bool compact)
        {
            var warning = aandhiState == AandhiState.Warning
                ? $"  WARN {warningRemainingSeconds:0.0}s"
                : aandhiState == AandhiState.Closing ? "  CLOSING" : string.Empty;
            var format = compact ? CompactMatchFormat : MatchFormat;
            return string.Format(format, FriendlyPhaseLabel(phase), aliveCount, zoneRadius, nextZoneRadius, warning);
        }

        public static string FriendlyPhaseLabel(MatchPhase phase)
        {
            switch (phase)
            {
                case MatchPhase.LoadWarmup: return "GET READY";
                case MatchPhase.SpawnProtection: return "SPAWN SHIELD";
                case MatchPhase.Opening: return "OPENING FIGHT";
                case MatchPhase.Pressure: return "AANDHI PRESSURE";
                case MatchPhase.FinalCircle: return "FINAL CIRCLE";
                case MatchPhase.Resolution: return "RESULTS";
                default: return "MATCH";
            }
        }

        public static string FormatResults(MatchParticipantSnapshot[] results, bool compact)
        {
            if (results == null || results.Length == 0) return "RESULTS";

            var ordered = new List<MatchParticipantSnapshot>(results);
            ordered.Sort((left, right) =>
            {
                var placement = left.Placement.CompareTo(right.Placement);
                return placement != 0 ? placement : left.Id.Value.CompareTo(right.Id.Value);
            });

            var builder = new StringBuilder(256);
            builder.Append("RESULTS\nWINNER ").Append(FriendlyParticipantLabel(ordered[0].Id.Value, 0)).Append('\n');
            for (var i = 0; i < ordered.Count; i++)
            {
                var participant = ordered[i];
                var participantLabel = FriendlyParticipantLabel(participant.Id.Value, i);
                if (compact)
                {
                    builder.Append('#').Append(participant.Placement)
                        .Append(' ').Append(participantLabel)
                        .Append("  KOs ").Append(participant.Eliminations)
                        .Append("  AST ").Append(participant.Assists)
                        .Append("  DMG ").Append(participant.DamageDealt)
                        .Append('\n');
                }
                else
                {
                    builder.Append('#').Append(participant.Placement)
                        .Append(' ').Append(participantLabel)
                        .Append("  KOs ").Append(participant.Eliminations)
                        .Append("  AST ").Append(participant.Assists)
                        .Append("  DMG ").Append(participant.DamageDealt)
                        .Append("  SURV ").Append(participant.SurvivalTimeSeconds.ToString("0.0"))
                        .Append('s').Append('\n');
                }
            }

            return builder.ToString().TrimEnd();
        }

        private static string FriendlyParticipantLabel(int actorId, int resultIndex)
        {
            if (actorId == 1) return "YOU";
            var rivalIndex = Mathf.Max(0, resultIndex - 1);
            return $"RIVAL {(char)('A' + (rivalIndex % 26))}";
        }

        private void BuildCanvasUi()
        {
            var root = new GameObject("ProductionHudRoot", typeof(RectTransform));
            root.transform.SetParent(EnsureSafeAreaParent(canvas), false);
            var rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            _statusText = CreateText(root.transform, "MatchStatus", new Vector2(0.42f, 0.94f), new Vector2(0.84f, 0.99f), 24, TextAnchor.UpperLeft);
            CreatePanel(root.transform, "MatchStatusCard", new Vector2(0.39f, 0.925f), new Vector2(0.99f, 0.995f), BattleRajaUiTheme.Surface);
            _statusText.transform.SetAsLastSibling();
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
            CreateButton(_settingsPanel.transform, "CloseSettings", "CLOSE", new Vector2(0.52f, 0.01f), new Vector2(0.92f, 0.075f), ToggleSettings);
            CreateButton(_settingsPanel.transform, "ReturnToMenu", "RETURN TO MENU", new Vector2(0.08f, 0.01f), new Vector2(0.48f, 0.075f), ReturnToMenu);
            CreateButton(_settingsPanel.transform, "LeftHanded", "LEFT-HANDED", new Vector2(0.08f, 0.65f), new Vector2(0.92f, 0.76f), ToggleLeftHanded);
            CreateButton(_settingsPanel.transform, "ReducedFlashes", "REDUCED FLASHES", new Vector2(0.08f, 0.50f), new Vector2(0.92f, 0.61f), ToggleReducedFlashes);
            CreateButton(_settingsPanel.transform, "HighContrast", "HIGH CONTRAST", new Vector2(0.08f, 0.35f), new Vector2(0.92f, 0.46f), ToggleHighContrast);
            _aimAssistButton = CreateButton(_settingsPanel.transform, "AimAssist", "AIM ASSIST", new Vector2(0.08f, 0.20f), new Vector2(0.92f, 0.31f), ToggleAimAssist);
            CreateButton(_settingsPanel.transform, "TextDown", "TEXT -", new Vector2(0.08f, 0.09f), new Vector2(0.44f, 0.18f), DecreaseTextScale);
            CreateButton(_settingsPanel.transform, "TextUp", "TEXT +", new Vector2(0.56f, 0.09f), new Vector2(0.92f, 0.18f), IncreaseTextScale);
            _settingsPanel.SetActive(false);
            RefreshAimAssistLabel();
            ApplyResponsiveLayout();
        }

        private static RectTransform EnsureSafeAreaParent(Canvas hudCanvas)
        {
            var safeArea = hudCanvas.transform.Find("SafeArea");
            if (safeArea == null)
            {
                var safeAreaObject = new GameObject("SafeArea", typeof(RectTransform), typeof(SafeAreaPanel));
                safeAreaObject.transform.SetParent(hudCanvas.transform, false);
                safeArea = safeAreaObject.transform;
            }
            else if (safeArea.GetComponent<SafeAreaPanel>() == null)
            {
                safeArea.gameObject.AddComponent<SafeAreaPanel>();
            }

            return safeArea as RectTransform;
        }

        private void ApplyResponsiveLayout()
        {
            if (_statusText == null || _spectatorText == null || Screen.height <= 0) return;
            var compact = (float)Screen.width / Screen.height < 0.75f;
            var targetStatusSize = Mathf.RoundToInt((compact ? 18f : 22f) * _textScale);
            if (compact == _compactLayout && _statusText.fontSize == targetStatusSize) return;

            _compactLayout = compact;
            var statusRect = _statusText.rectTransform;
            statusRect.anchorMin = compact ? new Vector2(0.42f, 0.90f) : new Vector2(0.42f, 0.94f);
            statusRect.anchorMax = new Vector2(0.98f, 0.99f);
            _statusText.fontSize = targetStatusSize;
            if (_resultsText != null) _resultsText.fontSize = Mathf.RoundToInt((compact ? 18f : 20f) * _textScale);

            var spectatorRect = _spectatorText.rectTransform;
            spectatorRect.anchorMin = compact ? new Vector2(0.42f, 0.84f) : new Vector2(0.42f, 0.88f);
            spectatorRect.anchorMax = compact ? new Vector2(0.98f, 0.89f) : new Vector2(0.98f, 0.93f);
            _spectatorText.fontSize = Mathf.RoundToInt((compact ? 16f : 20f) * _textScale);

            if (_settingsPanel != null)
            {
                // On a portrait phone, a centered modal keeps the controls readable
                // and leaves an intentional gameplay margin on both sides. Wide
                // layouts retain the compact side sheet for desktop/tablet play.
                var settingsRect = _settingsPanel.GetComponent<RectTransform>();
                settingsRect.anchorMin = compact ? new Vector2(0.06f, 0.10f) : new Vector2(0.58f, 0.16f);
                settingsRect.anchorMax = compact ? new Vector2(0.94f, 0.90f) : new Vector2(0.96f, 0.86f);
                settingsRect.offsetMin = Vector2.zero;
                settingsRect.offsetMax = Vector2.zero;
            }
        }

        private void ToggleSettings()
        {
            if (_settingsPanel == null) return;
            _settingsPanel.SetActive(!_settingsPanel.activeSelf);
            _paused = _settingsPanel.activeSelf;
            _lifecyclePaused = false;
            Time.timeScale = _paused ? 0f : 1f;
            _audio?.StartFromUserGesture();
        }

        private void SetLifecyclePause(bool paused)
        {
            if (_settingsPanel == null || match == null || match.ResultsShown) return;

            if (paused)
            {
                // Do not alter a pause the player opened deliberately. Only remember
                // and resume a pause that this lifecycle callback created.
                if (_paused) return;
                _settingsPanel.SetActive(true);
                _paused = true;
                _lifecyclePaused = true;
                Time.timeScale = 0f;
                return;
            }

            if (!_lifecyclePaused) return;
            _settingsPanel.SetActive(false);
            _paused = false;
            _lifecyclePaused = false;
            Time.timeScale = 1f;
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
            var compact = Screen.height > 0 && (float)Screen.width / Screen.height < 0.75f;
            var stickY = compact ? 0.18f : 0.20f;
            var actionY = compact ? 0.34f : 0.49f;
            movement.anchorMin = _leftHanded ? new Vector2(0.83f, stickY) : new Vector2(0.17f, stickY);
            movement.anchorMax = movement.anchorMin;
            aim.anchorMin = _leftHanded ? new Vector2(0.17f, stickY) : new Vector2(0.83f, stickY);
            aim.anchorMax = aim.anchorMin;

            var attack = GameObject.Find("AttackButton")?.GetComponent<RectTransform>();
            var ability = GameObject.Find("AbilityButton")?.GetComponent<RectTransform>();
            var gadget = GameObject.Find("GadgetButton")?.GetComponent<RectTransform>();
            SetActionAnchor(attack, new Vector2(_leftHanded ? 0.07f : 0.93f, actionY));
            SetActionAnchor(ability, new Vector2(_leftHanded ? 0.20f : 0.80f, actionY));
            SetActionAnchor(gadget, new Vector2(_leftHanded ? 0.33f : 0.67f, actionY));
            SetActionSize(attack, compact ? 146f : 170f);
            SetActionSize(ability, compact ? 122f : 140f);
            SetActionSize(gadget, compact ? 106f : 120f);
            _controlsCompactLayout = compact;
            _controlsLayoutInitialized = true;
        }

        private static void SetActionAnchor(RectTransform rect, Vector2 anchor)
        {
            if (rect == null) return;
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
        }

        private static void SetActionSize(RectTransform rect, float size)
        {
            if (rect == null) return;
            rect.sizeDelta = new Vector2(size, size);
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
            foreach (var presentation in FindObjectsByType<FighterPresentation>())
            {
                presentation.ReducedFlashMode = _reducedFlashes;
            }

            foreach (var impactPool in FindObjectsByType<CombatImpactFeedbackPool>())
            {
                impactPool.ReducedFlashMode = _reducedFlashes;
            }

            foreach (var hitFlash in FindObjectsByType<CombatHitFlash>())
            {
                hitFlash.ReducedFlashMode = _reducedFlashes;
            }

            if (_aandhiVisual != null) _aandhiVisual.ReducedFlashMode = _reducedFlashes;
        }

        private void ToggleHighContrast()
        {
            _highContrast = !_highContrast;
            PlayerPrefs.SetInt("battleraja.settings.high_contrast", _highContrast ? 1 : 0);
            PlayerPrefs.Save();
            BattleRajaUiTheme.ApplyContrast(canvas != null ? canvas.transform : null, _highContrast);
        }

        private void ToggleAimAssist()
        {
            _aimAssist = !_aimAssist;
            PlayerPrefs.SetInt("battleraja.settings.aim_assist", _aimAssist ? 1 : 0);
            PlayerPrefs.Save();
            _playerInput?.SetAimAssistEnabled(_aimAssist);
            RefreshAimAssistLabel();
        }

        private void IncreaseTextScale()
        {
            _textScale = Mathf.Clamp(_textScale + 0.1f, 0.9f, 1.3f);
            PlayerPrefs.SetFloat("battleraja.settings.text_scale", _textScale);
            PlayerPrefs.Save();
            ApplyTextScale();
            ApplyResponsiveLayout();
        }

        private void DecreaseTextScale()
        {
            _textScale = Mathf.Clamp(_textScale - 0.1f, 0.9f, 1.3f);
            PlayerPrefs.SetFloat("battleraja.settings.text_scale", _textScale);
            PlayerPrefs.Save();
            ApplyTextScale();
            ApplyResponsiveLayout();
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
            _textScale = Mathf.Clamp(PlayerPrefs.GetFloat("battleraja.settings.text_scale", 1f), 0.9f, 1.3f);
        }

        private void ApplyTextScale()
        {
            if (canvas == null || _appliedTextScale <= 0.001f) return;
            var ratio = _textScale / _appliedTextScale;
            foreach (var text in canvas.GetComponentsInChildren<Text>(true))
            {
                if (text == null || text == _statusText || text == _spectatorText || text == _resultsText) continue;
                text.fontSize = Mathf.Clamp(Mathf.RoundToInt(text.fontSize * ratio), 12, 48);
            }

            _appliedTextScale = _textScale;
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
            BattleRajaUiTheme.StylePanel(panel, color);
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
            BattleRajaUiTheme.StyleText(text, size, alignment);
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
            var text = CreateText(buttonObject.transform, name + "Label", Vector2.zero, Vector2.one, 18, TextAnchor.MiddleCenter);
            text.text = label;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                var audio = FindAnyObjectByType<BattleRajaAudioDirector>();
                audio?.StartFromUserGesture();
                audio?.PlayUiConfirm();
                action?.Invoke();
            });
            BattleRajaUiTheme.StyleButton(button, name == "Rematch" || name == "Pause");
            return button;
        }
    }
}
