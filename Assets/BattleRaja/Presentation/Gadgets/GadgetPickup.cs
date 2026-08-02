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

        public bool TryCollect(GadgetUser user)
        {
            if (!available || user == null) return false;
            if (!user.TryPickup(GadgetId)) return false;
            available = false;
            gameObject.SetActive(false);
            return true;
        }

        public void ResetPickup()
        {
            available = true;
            gameObject.SetActive(true);
        }
    }
}
