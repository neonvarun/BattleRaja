using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.UI;
using BattleRaja.Presentation.Visuals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleRaja.Presentation.Flow
{
    /// <summary>
    /// Replayable tutorial card over the real movement/combat arena. Prompts are deliberately
    /// concise and leave the underlying controls, pickups, Aandhi and match HUD active.
    /// </summary>
    public sealed class TutorialOverlay : MonoBehaviour
    {
        private const string CompletedKey = "battleraja.tutorial.completed";
        private const string LeftHandedKey = "battleraja.settings.left_handed";

        [SerializeField] private Canvas canvas;
        [SerializeField] private OfflineMatchController match;

        private readonly TutorialStepMachine _steps = new TutorialStepMachine();
        private Text _title;
        private Text _body;
        private Text _progress;
        private Button _advanceButton;
        private GameObject _panel;
        private bool _showing;
        private MovementPlayerAgent _playerAgent;
        private PlayerInputAdapter _playerInput;
        private FighterPresentation _playerPresentation;
        private GadgetUser _playerGadget;
        private Vector3 _startingPosition;
        private int _startingAttackCount;
        private int _startingAbilityCount;
        private int _startingPickupCount;
        private int _startingUseCount;
        private int _startingEliminationCount;
        private bool _telemetryReady;

        public TutorialStep CurrentStep => _steps.Current;
        public bool IsShowing => _showing;
        public bool CurrentStepSatisfied => _steps.CurrentStepSatisfied;

        private void Awake()
        {
            match = match != null ? match : FindAnyObjectByType<OfflineMatchController>();
            BuildCanvasUi();
            Refresh();
        }

        private void Start()
        {
            ResolvePlayerTelemetry();
            CaptureTelemetryBaseline();
            Refresh();
        }

        private void Update()
        {
            if (_steps.IsComplete) return;
            ResolvePlayerTelemetry();

            var changed = false;
            if (_telemetryReady)
            {
                if (Vector3.Distance(_playerAgent.transform.position, _startingPosition) >= 0.20f)
                {
                    changed |= _steps.ObserveAction(TutorialAction.Movement);
                }

                if (_playerInput != null && _playerInput.ReadInput().Aim.SqrMagnitude >= 0.06f)
                {
                    changed |= _steps.ObserveAction(TutorialAction.Aim);
                }

                if (_playerPresentation != null)
                {
                    if (_playerPresentation.AttackActivationCount > _startingAttackCount)
                    {
                        changed |= _steps.ObserveAction(TutorialAction.BasicAttack);
                    }

                    if (_playerPresentation.AbilityActivationCount > _startingAbilityCount)
                    {
                        changed |= _steps.ObserveAction(TutorialAction.Ability);
                    }
                }

                if (_playerGadget != null)
                {
                    if (_playerGadget.SuccessfulPickupCount > _startingPickupCount)
                    {
                        changed |= _steps.ObserveAction(TutorialAction.GadgetCollected);
                    }

                    if (_playerGadget.SuccessfulUseCount > _startingUseCount)
                    {
                        changed |= _steps.ObserveAction(TutorialAction.GadgetUsed);
                    }
                }
            }

            if (match != null && match.AandhiState != BattleRaja.Core.Domain.AandhiState.Stable)
            {
                changed |= _steps.ObserveAction(TutorialAction.AandhiObserved);
            }

            if (match != null && match.Simulation != null &&
                match.Simulation.TryGetSnapshot(new CombatEntityId(1), out var playerSnapshot))
            {
                if (playerSnapshot.Eliminations > _startingEliminationCount)
                {
                    // Elimination is an in-match lesson: observe the authoritative
                    // counter as soon as a KO is credited, without waiting for the
                    // terminal results screen. Victory remains result-gated below.
                    changed |= _steps.ObserveAction(TutorialAction.Elimination);
                }

                if (match.ResultsShown && playerSnapshot.Placement == 1)
                {
                    changed |= _steps.ObserveAction(TutorialAction.Victory);
                }
            }

            if (changed) Refresh();
        }

        public void Advance()
        {
            if (!_steps.TryAdvance())
            {
                Refresh();
                return;
            }

            // A lesson performed before its turn must not satisfy a later lesson.
            // Re-baseline the player telemetry whenever the explicit continue
            // gate advances the walkthrough. Gadget pickup is the one exception:
            // the real authority can collect the tutorial's nearby pickup while
            // the earlier lessons are still on screen, so reconcile that already
            // authoritative inventory state when the Gadget lesson begins.
            ReconcileGadgetLessonState();
            CaptureTelemetryBaseline();
            if (_steps.IsComplete) SaveCompletion();
            Refresh();
        }

        public bool ObserveAction(TutorialAction action)
        {
            var observed = _steps.ObserveAction(action);
            if (observed) Refresh();
            return observed;
        }

        public void Replay()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void Skip()
        {
            _steps.SkipToComplete();
            SaveCompletion();
            Refresh();
        }

        public void ReturnToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Bootstrap", LoadSceneMode.Single);
        }

        private void Refresh()
        {
            // Keep the completion card visible. The tutorial prompt is intentionally the
            // only full-width surface: the live arena, player and touch controls remain
            // visible behind it so every lesson can be performed in context.
            _showing = true;
            _panel.SetActive(true);
            if (_steps.IsComplete)
            {
                _title.text = "TUTORIAL COMPLETE";
                _body.text = "You have seen the complete offline loop. Replay any time from the main menu.";
                _progress.text = "8 / 8 COMPLETE";
                SetButtonLabel("REPLAY TUTORIAL");
                _advanceButton.onClick.RemoveAllListeners();
                _advanceButton.onClick.AddListener(Replay);
                _advanceButton.interactable = true;
                return;
            }

            var step = _steps.Current;
            var leftHanded = PlayerPrefs.GetInt(LeftHandedKey, 0) != 0;
            _title.text = "TUTORIAL  •  " + StepTitle(step);
            var body = StepBody(step, leftHanded);
            if (!_steps.CurrentStepSatisfied)
            {
                body += "\n\nDO IT IN THE ARENA TO UNLOCK CONTINUE.";
            }

            _body.text = body;
            _progress.text = $"{(int)step + 1} / 8   {ControlHint(step)}";
            SetButtonLabel(_steps.CurrentStepSatisfied
                ? (step == TutorialStep.Victory ? "FINISH TUTORIAL" : "CONTINUE")
                : "WAITING FOR ACTION");
            _advanceButton.onClick.RemoveAllListeners();
            _advanceButton.onClick.AddListener(Advance);
            _advanceButton.interactable = _steps.CurrentStepSatisfied;
        }

        private void ResolvePlayerTelemetry()
        {
            if (_telemetryReady && _playerAgent != null && _playerInput != null &&
                _playerPresentation != null && _playerGadget != null) return;

            var agents = FindObjectsByType<MovementPlayerAgent>();
            for (var i = 0; i < agents.Length; i++)
            {
                if (agents[i] == null || agents[i].ActorId != 1) continue;
                _playerAgent = agents[i];
                _playerInput = agents[i].GetComponent<PlayerInputAdapter>();
                _playerPresentation = agents[i].GetComponent<FighterPresentation>();
                _playerGadget = agents[i].GetComponent<GadgetUser>();
                _telemetryReady = _playerInput != null && _playerPresentation != null && _playerGadget != null;
                return;
            }
        }

        private void CaptureTelemetryBaseline()
        {
            if (!_telemetryReady) return;
            _startingPosition = _playerAgent.transform.position;
            _startingAttackCount = _playerPresentation.AttackActivationCount;
            _startingAbilityCount = _playerPresentation.AbilityActivationCount;
            _startingPickupCount = _playerGadget.SuccessfulPickupCount;
            _startingUseCount = _playerGadget.SuccessfulUseCount;
            _startingEliminationCount = match != null && match.Simulation != null &&
                match.Simulation.TryGetSnapshot(new CombatEntityId(1), out var playerSnapshot)
                ? playerSnapshot.Eliminations
                : 0;
        }

        private void ReconcileGadgetLessonState()
        {
            if (_steps.Current != TutorialStep.Gadget || _playerGadget == null) return;

            // A fresh tutorial scene has no prior user telemetry. If the authority
            // collected or consumed the nearby pickup before the card became active,
            // preserve those real events instead of trapping the player at WAITING.
            if (_playerGadget.SuccessfulPickupCount > 0 || _playerGadget.HasGadget)
            {
                _steps.ObserveAction(TutorialAction.GadgetCollected);
            }

            if (_playerGadget.SuccessfulUseCount > 0)
            {
                _steps.ObserveAction(TutorialAction.GadgetUsed);
            }
        }

        private void SaveCompletion()
        {
            PlayerPrefs.SetInt(CompletedKey, 1);
            PlayerPrefs.Save();
        }

        private void BuildCanvasUi()
        {
            var canvasObject = new GameObject("TutorialCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            var safe = new GameObject("SafeArea", typeof(RectTransform), typeof(SafeAreaPanel));
            safe.transform.SetParent(canvas.transform, false);
            var safeRect = safe.GetComponent<RectTransform>();
            safeRect.anchorMin = Vector2.zero;
            safeRect.anchorMax = Vector2.one;
            safeRect.offsetMin = Vector2.zero;
            safeRect.offsetMax = Vector2.zero;

            var logoObject = new GameObject("TutorialMark", typeof(RectTransform), typeof(BattleRajaLogoGraphic));
            logoObject.transform.SetParent(safe.transform, false);
            var logoRect = logoObject.GetComponent<RectTransform>();
            logoRect.anchorMin = new Vector2(0.46f, 0.885f);
            logoRect.anchorMax = new Vector2(0.54f, 0.965f);
            logoRect.offsetMin = Vector2.zero;
            logoRect.offsetMax = Vector2.zero;

            _panel = new GameObject("TutorialPanel", typeof(RectTransform), typeof(Image));
            _panel.transform.SetParent(safe.transform, false);
            var panelRect = _panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.10f, 0.66f);
            panelRect.anchorMax = new Vector2(0.90f, 0.96f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            BattleRajaUiTheme.StylePanel(_panel, BattleRajaUiTheme.Surface);

            _title = CreateText(_panel.transform, "Title", new Vector2(0.04f, 0.68f), new Vector2(0.96f, 0.94f), 28, TextAnchor.MiddleCenter);
            _body = CreateText(_panel.transform, "Body", new Vector2(0.07f, 0.32f), new Vector2(0.93f, 0.68f), 20, TextAnchor.MiddleCenter);
            _progress = CreateText(_panel.transform, "Progress", new Vector2(0.05f, 0.18f), new Vector2(0.95f, 0.32f), 16, TextAnchor.MiddleCenter);
            _advanceButton = CreateButton(_panel.transform, "Advance", "I'M READY", new Vector2(0.28f, 0.03f), new Vector2(0.72f, 0.16f), Advance);
            CreateButton(_panel.transform, "Skip", "SKIP", new Vector2(0.75f, 0.03f), new Vector2(0.95f, 0.16f), Skip);
            CreateButton(_panel.transform, "Menu", "MENU", new Vector2(0.05f, 0.03f), new Vector2(0.25f, 0.16f), ReturnToMenu);
        }

        private void SetButtonLabel(string label)
        {
            if (_advanceButton == null) return;
            var text = _advanceButton.GetComponentInChildren<Text>();
            if (text != null) text.text = label;
        }

        private static string StepTitle(TutorialStep step)
        {
            switch (step)
            {
                case TutorialStep.BasicAttack: return "BASIC ATTACK";
                case TutorialStep.Aandhi: return "AANDHI";
                default: return step.ToString().ToUpperInvariant();
            }
        }

        private static string StepBody(TutorialStep step, bool leftHanded)
        {
            switch (step)
            {
                case TutorialStep.Movement:
                    return $"Use the {(leftHanded ? "right" : "left")} stick to move. Keep your fighter inside the arena routes.";
                case TutorialStep.Aim:
                    return $"Use the {(leftHanded ? "left" : "right")} stick to aim. Keep the direction pointed toward the action.";
                case TutorialStep.BasicAttack: return "Hold ATTACK to fire. Watch the telegraph, projectile path and hit feedback.";
                case TutorialStep.Ability: return "Tap ABILITY to trigger your fighter's special move. Each fighter has a different counterplay window.";
                case TutorialStep.Gadget: return "Walk over a coloured gadget pickup, then tap the gadget button. Carry one gadget at a time.";
                case TutorialStep.Aandhi: return "Watch the zone warning and NEXT preview in the HUD. Move toward safety before the Aandhi closes and applies damage.";
                case TutorialStep.Elimination: return "Defeat a target to earn a KO. KOs, damage and survival time appear in results.";
                case TutorialStep.Victory: return "Survive the final circle. Spectate after elimination, then inspect placements and choose REMATCH or MENU.";
                default: return "Replay this tutorial whenever you need a refresher.";
            }
        }

        private static string ControlHint(TutorialStep step)
        {
            switch (step)
            {
                case TutorialStep.Movement: return "MOVE";
                case TutorialStep.Aim: return "AIM";
                case TutorialStep.BasicAttack: return "ATTACK";
                case TutorialStep.Ability: return "ABILITY";
                case TutorialStep.Gadget: return "PICKUP + GADGET";
                case TutorialStep.Aandhi: return "READ THE ZONE HUD";
                case TutorialStep.Elimination: return "DAMAGE + KO";
                default: return "RESULTS + REMATCH";
            }
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
            var text = CreateText(buttonObject.transform, name + "Label", Vector2.zero, Vector2.one, 16, TextAnchor.MiddleCenter);
            text.text = label;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(action);
            BattleRajaUiTheme.StyleButton(button, name == "Advance");
            return button;
        }
    }
}
