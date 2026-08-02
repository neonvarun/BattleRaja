using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleRaja.Presentation.Combat
{
    public sealed class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public bool IsPressed { get; private set; }

        public void OnPointerDown(PointerEventData eventData) => IsPressed = true;
        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
        public void OnPointerExit(PointerEventData eventData) => IsPressed = false;
        public void ResetButton() => IsPressed = false;
    }
}
