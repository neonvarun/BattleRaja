using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Gadgets
{
    public sealed class GadgetPickup : MonoBehaviour
    {
        [SerializeField] private string gadgetId = "gadget.umbrella_guard";
        [SerializeField] private bool available = true;

        public ContentId GadgetId => ContentId.Gadget(gadgetId);
        public bool IsAvailable => available;

        private void Awake()
        {
            if (GetComponent<GadgetPickupVisuals>() == null)
            {
                gameObject.AddComponent<GadgetPickupVisuals>();
            }
        }

        public bool TryCollect(GadgetUser user)
        {
            if (!available || user == null) return false;
            if (!user.TryPickup(GadgetId)) return false;
            SetAvailable(false);
            return true;
        }

        public void ResetPickup()
        {
            SetAvailable(true);
        }

        public void SetAvailable(bool value)
        {
            available = value;
            gameObject.SetActive(value);
        }
    }
}
