using BattleRaja.Core.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.Gadgets
{
    public sealed class GadgetHud : MonoBehaviour
    {
        [SerializeField] private GadgetUser user;
        [SerializeField] private Text statusText;

        private void Awake()
        {
            user = user != null ? user : FindFirstObjectByType<GadgetUser>();
            statusText = statusText != null ? statusText : GetComponentInChildren<Text>();
        }

        private void Update()
        {
            if (statusText == null || user == null) return;
            var held = user.HasGadget ? user.HeldGadget.Value.Replace("gadget.", string.Empty) : "empty";
            statusText.text = $"GADGET [G] {held}\nCD {user.CooldownRemaining:0.0}s {user.Feedback}";
        }
    }
}
