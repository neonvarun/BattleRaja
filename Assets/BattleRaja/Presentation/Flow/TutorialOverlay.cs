using BattleRaja.Core.Application;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Movement;
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

        [SerializeField] private Canvas canvas;
        [SerializeField] private OfflineMatchController match;

        private readonly TutorialStepMachine _steps = new TutorialStepMachine();
        private Text _title;
        private Text _body;
        private Text _progress;
        private Button _advanceButton;
        private GameObject _panel;
        private bool _showing;

        public TutorialStep CurrentStep => _steps.Current;
        public bool IsShowing => _showing;

        private void Awake()
        {
            match = match != null ? match : FindAnyObjectByType<OfflineMatchController>();
            BuildCanvasUi();
            Refresh();
        }

        public void Advance()
        {
            _steps.Advance();
            if (_steps.IsComplete)
            {
                PlayerPrefs.SetInt(CompletedKey, 1);
                PlayerPrefs.Save();
            }

            Refresh();
        }

        public void Replay()
        {
            _steps.Restart();
            Refresh();
        }

        public void Skip()
        {
            _steps.Advance();
            while (!_steps.IsComplete) _steps.Advance();
            PlayerPrefs.SetInt(CompletedKey, 1);
            PlayerPrefs.Save();
            Refresh();
        }

        public void ReturnToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Bootstrap", LoadSceneMode.Single);
        }

        private void Refresh()
        {
            _showing = !_steps.IsComplete;
            _panel.SetActive(true);
            if (_steps.IsComplete)
            {
                _title.text = "TUTORIAL COMPLETE";
                _body.text = "You have seen the complete offline loop. Replay any time from the main menu.";
                _progress.text = "8 / 8 COMPLETE";
                SetButtonLabel("REPLAY TUTORIAL");
                _advanceButton.onClick.RemoveAllListeners();
                _advanceButton.onClick.AddListener(Replay);
                return;
            }

            var step = _steps.Current;
            _title.text = "TUTORIAL  •  " + StepTitle(step);
            _body.text = StepBody(step);
            _progress.text = $"{(int)step + 1} / 8   {ControlHint(step)}";
            SetButtonLabel(step == TutorialStep.Victory ? "FINISH TUTORIAL" : "I'M READY");
            _advanceButton.onClick.RemoveAllListeners();
            _advanceButton.onClick.AddListener(Advance);
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

            _panel = new GameObject("TutorialPanel", typeof(RectTransform), typeof(Image));
            _panel.transform.SetParent(safe.transform, false);
            var panelRect = _panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.10f, 0.66f);
            panelRect.anchorMax = new Vector2(0.90f, 0.96f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            _panel.GetComponent<Image>().color = new Color(0.02f, 0.06f, 0.09f, 0.94f);

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

        private static string StepBody(TutorialStep step)
        {
            switch (step)
            {
                case TutorialStep.Movement: return "Move with the left stick on Lava, or WASD/arrow keys on Web. Keep your fighter inside the arena routes.";
                case TutorialStep.Aim: return "Aim with the right stick, mouse direction or controller right stick. Direction is shared by human and bot commands.";
                case TutorialStep.BasicAttack: return "Hold ATTACK (or left mouse button) to fire. Watch the telegraph, projectile path and hit feedback.";
                case TutorialStep.Ability: return "Use the ability button (Space/right trigger) to trigger your fighter's ability. Each fighter has a different counterplay window.";
                case TutorialStep.Gadget: return "Walk over a coloured gadget pickup, then press the gadget button. One held gadget is validated by match authority.";
                case TutorialStep.Aandhi: return "Watch the zone warning and NEXT preview in the HUD. Move toward safety before the Aandhi closes and applies damage.";
                case TutorialStep.Elimination: return "Defeat a target to create an instigator-aware elimination. KOs, damage and survival time appear in results.";
                case TutorialStep.Victory: return "Survive the final circle. Spectate after elimination, then inspect placements and choose REMATCH or MENU.";
                default: return "Replay this tutorial whenever you need a refresher.";
            }
        }

        private static string ControlHint(TutorialStep step)
        {
            switch (step)
            {
                case TutorialStep.Movement: return "LEFT STICK / WASD";
                case TutorialStep.Aim: return "RIGHT STICK / MOUSE";
                case TutorialStep.BasicAttack: return "ATTACK / LEFT CLICK";
                case TutorialStep.Ability: return "ABILITY / SPACE";
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
            var text = CreateText(buttonObject.transform, name + "Label", Vector2.zero, Vector2.one, 16, TextAnchor.MiddleCenter);
            text.text = label;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(action);
            return button;
        }
    }
}
