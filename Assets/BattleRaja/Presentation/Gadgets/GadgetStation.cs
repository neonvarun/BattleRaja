using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using UnityEngine;

namespace BattleRaja.Presentation.Gadgets
{
    public sealed class GadgetStation : MonoBehaviour
    {
        [SerializeField] private float lifetimeSeconds = 10f;
        [SerializeField] private float healIntervalSeconds = 1f;
        [SerializeField] private float effectRadius = 2.4f;
        [SerializeField] private int healAmount = 18;
        [SerializeField] private int maxHealth = 45;

        private CombatHealth _health;
        private float _lifetime;
        private float _healAccumulator;
        private int _stationId = -1;
        private bool _authorityDriven;

        public bool IsExpired { get; private set; }
        public int StationId => _stationId;
        public float RemainingLifetime => Mathf.Max(0f, lifetimeSeconds - _lifetime);
        public int HealAmount => healAmount;
        public float EffectRadius => effectRadius;

        public void Configure(GadgetDefinition definition, int stationId = -1)
        {
            lifetimeSeconds = definition.DurationSeconds;
            effectRadius = definition.Radius;
            healAmount = definition.Magnitude;
            maxHealth = definition.StationHealth;
            _stationId = stationId;
            _authorityDriven = stationId > 0;
        }

        private void Awake()
        {
            _health = GetComponent<CombatHealth>();
            if (_health == null) _health = gameObject.AddComponent<CombatHealth>();
            _health.SetMaxHealthForGadget(maxHealth);
        }

        private void Update()
        {
            if (_authorityDriven) return;
            if (IsExpired) return;
            _lifetime += Time.deltaTime;
            _healAccumulator += Time.deltaTime;
            if (_healAccumulator >= healIntervalSeconds)
            {
                _healAccumulator = 0f;
                HealNearby();
            }

            if (_lifetime >= lifetimeSeconds || _health.Snapshot.IsDefeated)
            {
                IsExpired = true;
                Destroy(gameObject);
            }
        }

        public void ExpireFromAuthority()
        {
            if (IsExpired) return;
            IsExpired = true;
            Destroy(gameObject);
        }

        private void HealNearby()
        {
            var agents = FindObjectsByType<CombatHealth>(FindObjectsSortMode.None);
            for (var i = 0; i < agents.Length; i++)
            {
                var target = agents[i];
                if (target == _health || target.Snapshot.IsDefeated) continue;
                if (Vector3.Distance(transform.position, target.transform.position) <= effectRadius)
                {
                    target.Heal(healAmount);
                }
            }
        }
    }
}
