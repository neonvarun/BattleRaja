using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleRaja.Presentation.Gadgets
{
    public sealed class GadgetUseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private GadgetUser user;
        public bool IsPressed { get; private set; }

        private void Awake() => user = user != null ? user : FindFirstObjectByType<GadgetUser>();
        public void OnPointerDown(PointerEventData eventData) { IsPressed = true; user?.UseHeld(); }
        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
        public void OnPointerExit(PointerEventData eventData) => IsPressed = false;
    }
}
