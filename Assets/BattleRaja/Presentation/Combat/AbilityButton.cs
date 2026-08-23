using UnityEngine;
using UnityEngine.EventSystems;
using BattleRaja.Presentation.UI;

namespace BattleRaja.Presentation.Combat
{
    public sealed class AbilityButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public bool IsPressed { get; private set; }

        private void Awake() => TouchControlLabel.Ensure(transform, "ABILITY");

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            BattleRajaHaptics.Pulse();
        }
        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
        public void OnPointerExit(PointerEventData eventData) => IsPressed = false;
        public void ResetButton() => IsPressed = false;
    }
}
