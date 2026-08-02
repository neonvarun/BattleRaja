using BattleRaja.Presentation.Combat;
using UnityEngine;

namespace BattleRaja.Presentation.Match
{
    public enum MatchPickupType
    {
        Health = 0
    }

    public sealed class MatchPickup : MonoBehaviour
    {
        [SerializeField] private MatchPickupType pickupType = MatchPickupType.Health;
        [Min(1)] [SerializeField] private int value = 25;
        [Min(0.1f)] [SerializeField] private float respawnSeconds = 12f;

        private float _respawnAt = -1f;

        public bool IsAvailable => _respawnAt < 0f;
        public MatchPickupType PickupType => pickupType;
        public int Value => value;
        public float RespawnSeconds => respawnSeconds;

        private void Update()
        {
            if (_respawnAt > 0f && Time.time >= _respawnAt)
            {
                SetAvailable(true);
            }
        }

        public bool TryCollect(CombatHealth health)
        {
            if (!IsAvailable || health == null || health.Snapshot.IsDefeated)
            {
                return false;
            }

            var healed = pickupType == MatchPickupType.Health ? health.Heal(value) : 0;
            if (healed <= 0)
            {
                return false;
            }

            _respawnAt = Time.time + respawnSeconds;
            gameObject.SetActive(false);
            return true;
        }

        public void SetAvailable(bool available)
        {
            _respawnAt = available ? -1f : float.PositiveInfinity;
            gameObject.SetActive(available);
        }
    }
}
