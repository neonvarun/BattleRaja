using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.Gadgets
{
    public sealed class GadgetHud : MonoBehaviour
    {
        [SerializeField] private GadgetUser user;
        [SerializeField] private Text statusText;
        [SerializeField] private GadgetPickup[] pickups;
        [SerializeField] private float proximityRadius = 2.6f;

        private Transform _playerTransform;

        private void Awake()
        {
            user = user != null ? user : FindAnyObjectByType<GadgetUser>();
            statusText = statusText != null ? statusText : GetComponentInChildren<Text>();
            pickups = pickups != null && pickups.Length > 0 ? pickups : FindObjectsByType<GadgetPickup>();
            _playerTransform = user != null ? user.transform : FindAnyObjectByType<MovementPlayerAgent>()?.transform;
        }

        private void Update()
        {
            if (statusText == null || user == null) return;
            var held = user.HasGadget ? user.HeldGadget.Value : string.Empty;
            var nearby = ResolveNearbyPickup();
            statusText.text = FormatStatus(held, user.CooldownRemaining, user.Feedback, nearby);
        }

        /// <summary>
        /// Formats the player-facing gadget HUD without exposing keyboard shortcuts,
        /// authority terminology or serialized content IDs.
        /// </summary>
        public static string FormatStatus(string heldGadgetId, float cooldownRemaining, string feedback, string nearbyGadget)
        {
            var held = string.IsNullOrEmpty(heldGadgetId) ? "EMPTY" : FriendlyName(heldGadgetId);
            var cooldown = cooldownRemaining > 0.01f
                ? $"READY IN {cooldownRemaining:0.0}s"
                : "READY";
            var message = FriendlyFeedback(feedback);
            var status = $"GADGET {held}\n{cooldown}";
            if (!string.IsNullOrEmpty(message)) status += $"\n{message}";
            if (string.IsNullOrEmpty(heldGadgetId) && !string.IsNullOrEmpty(nearbyGadget))
            {
                status += $"\nNEAR {FriendlyName(nearbyGadget)}";
            }

            return status;
        }

        private string ResolveNearbyPickup()
        {
            if (_playerTransform == null || pickups == null) return string.Empty;

            var nearestDistance = float.MaxValue;
            var nearestName = string.Empty;
            var radius = Mathf.Max(0.1f, proximityRadius);
            for (var i = 0; i < pickups.Length; i++)
            {
                var pickup = pickups[i];
                if (pickup == null || !pickup.IsAvailable) continue;
                var distance = Vector3.Distance(_playerTransform.position, pickup.transform.position);
                if (distance > radius || distance >= nearestDistance) continue;
                nearestDistance = distance;
                nearestName = FriendlyName(pickup.GadgetId.Value);
            }

            return nearestName;
        }

        private static string FriendlyName(string id)
        {
            if (id.IndexOf("umbrella", System.StringComparison.OrdinalIgnoreCase) >= 0) return "UMBRELLA";
            if (id.IndexOf("dhol", System.StringComparison.OrdinalIgnoreCase) >= 0) return "DHOL";
            if (id.IndexOf("tiffin", System.StringComparison.OrdinalIgnoreCase) >= 0) return "TIFFIN";
            return "GADGET";
        }

        private static string FriendlyFeedback(string feedback)
        {
            if (string.IsNullOrWhiteSpace(feedback)) return string.Empty;
            if (feedback.StartsWith("Picked ", System.StringComparison.OrdinalIgnoreCase))
            {
                return $"{FriendlyName(feedback.Substring(7))} READY";
            }

            switch (feedback)
            {
                case "Gadget slot full": return "SLOT FULL";
                case "No gadget held":
                case "NotHeld": return "NO GADGET";
                case "Cooldown": return "ON COOLDOWN";
                case "InvalidDirection": return "AIM TO USE";
                case "InvalidPlacement": return "FIND A CLEAR SPOT";
                case "InvalidDefinition": return "GADGET UNAVAILABLE";
                case "Authority inventory mismatch": return "TRY AGAIN";
                default: return feedback.ToUpperInvariant();
            }
        }
    }
}
