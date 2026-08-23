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
            var held = user.HasGadget ? user.HeldGadget.Value.Replace("gadget.", string.Empty) : "empty";
            var nearby = ResolveNearbyPickup();
            var proximity = !user.HasGadget && !string.IsNullOrEmpty(nearby) ? $"\nNEAR {nearby}" : string.Empty;
            statusText.text = $"GADGET [G] {held}\nCD {user.CooldownRemaining:0.0}s {user.Feedback}{proximity}";
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
    }
}
