using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BattleRaja.Presentation.UI;

namespace BattleRaja.Presentation.Combat
{
    public sealed class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public bool IsPressed { get; private set; }

        private void Awake() => TouchControlLabel.Ensure(transform, "ATTACK");

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            BattleRajaHaptics.Pulse();
        }
        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
        public void OnPointerExit(PointerEventData eventData) => IsPressed = false;
        public void ResetButton() => IsPressed = false;

        private void OnApplicationPause(bool paused)
        {
            if (paused) ResetButton();
        }
    }

    /// <summary>
    /// Adds a small, non-raycast label to generated touch controls. The scene keeps the
    /// control surfaces data-light, while the runtime label makes the action discoverable
    /// on Android and Web without changing input semantics.
    /// </summary>
    public static class TouchControlLabel
    {
        public static void Ensure(Transform parent, string value)
        {
            if (parent == null) return;

            var labelObject = parent.Find("ControlLabel")?.gameObject;
            if (labelObject == null)
            {
                labelObject = new GameObject("ControlLabel", typeof(RectTransform), typeof(Text));
                labelObject.transform.SetParent(parent, false);
            }

            var rect = (RectTransform)labelObject.transform;
            rect.anchorMin = new Vector2(0.06f, 0.02f);
            rect.anchorMax = new Vector2(0.94f, 0.26f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var label = labelObject.GetComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 13;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = new Color(1f, 1f, 1f, 0.94f);
            label.raycastTarget = false;
            label.text = value ?? string.Empty;

            var glyphObject = parent.Find("ControlGlyph")?.gameObject;
            if (glyphObject == null)
            {
                glyphObject = new GameObject("ControlGlyph", typeof(RectTransform), typeof(BattleRajaTouchGlyph));
                glyphObject.transform.SetParent(parent, false);
            }

            var glyphRect = (RectTransform)glyphObject.transform;
            glyphRect.anchorMin = new Vector2(0.21f, 0.28f);
            glyphRect.anchorMax = new Vector2(0.79f, 0.84f);
            glyphRect.offsetMin = Vector2.zero;
            glyphRect.offsetMax = Vector2.zero;
            glyphObject.GetComponent<BattleRajaTouchGlyph>().Configure(
                ResolveGlyph(value),
                ResolveAccent(parent));
        }

        private static BattleRajaTouchGlyph.Kind ResolveGlyph(string value)
        {
            if (string.Equals(value, "MOVE", System.StringComparison.OrdinalIgnoreCase)) return BattleRajaTouchGlyph.Kind.Move;
            if (string.Equals(value, "AIM", System.StringComparison.OrdinalIgnoreCase)) return BattleRajaTouchGlyph.Kind.Aim;
            if (string.Equals(value, "ATTACK", System.StringComparison.OrdinalIgnoreCase)) return BattleRajaTouchGlyph.Kind.Attack;
            if (string.Equals(value, "ABILITY", System.StringComparison.OrdinalIgnoreCase)) return BattleRajaTouchGlyph.Kind.Ability;
            return BattleRajaTouchGlyph.Kind.Gadget;
        }

        private static Color ResolveAccent(Transform parent)
        {
            var surface = parent.GetComponent<BattleRajaTouchSurface>();
            return surface != null ? surface.GraphicAccent : Color.white;
        }
    }
}
