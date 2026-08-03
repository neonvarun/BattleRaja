using UnityEngine;
using UnityEngine.EventSystems;
using BattleRaja.Presentation.Combat;

namespace BattleRaja.Presentation.Gadgets
{
    public sealed class GadgetUseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private GadgetUser user;
        public bool IsPressed { get; private set; }

        private void Awake()
        {
            user = user != null ? user : FindAnyObjectByType<GadgetUser>();
            TouchControlLabel.Ensure(transform, "GADGET");
        }
        public void OnPointerDown(PointerEventData eventData) { IsPressed = true; user?.UseHeld(); }
        public void OnPointerUp(PointerEventData eventData) => IsPressed = false;
        public void OnPointerExit(PointerEventData eventData) => IsPressed = false;
    }
}
