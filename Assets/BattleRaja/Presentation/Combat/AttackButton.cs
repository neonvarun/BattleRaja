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
            if (parent == null || parent.Find("ControlLabel") != null) return;

            var labelObject = new GameObject("ControlLabel", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(parent, false);
            var rect = (RectTransform)labelObject.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var label = labelObject.GetComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 18;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = Color.white;
            label.raycastTarget = false;
            label.text = value ?? string.Empty;
        }
    }
}
