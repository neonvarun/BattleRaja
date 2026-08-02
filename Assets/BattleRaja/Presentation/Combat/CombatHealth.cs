using System;
using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatHealth : MonoBehaviour
    {
        [Min(1)] [SerializeField] private int maxHealth = 100;

        private HealthState _state;

        public event Action<DamageResult> DamageResolved;
        public HealthSnapshot Snapshot => _state != null ? _state.Snapshot : new HealthSnapshot(maxHealth, maxHealth);
        public int MaxHealth => maxHealth;

        private void Awake()
        {
            _state = new HealthState(maxHealth);
        }

        internal DamageResult ApplyThroughPipeline(
            DamagePipeline pipeline,
            DamageRequest request,
            CombatEntityId targetId,
            CombatFaction targetFaction,
            bool allowSelfHit,
            bool allowFriendlyFire)
        {
            var result = pipeline.Apply(
                request,
                targetId,
                targetFaction,
                _state,
                allowSelfHit,
                allowFriendlyFire);
            if (result.Applied)
            {
                DamageResolved?.Invoke(result);
            }

            return result;
        }

        public void ResetHealth()
        {
            if (_state == null)
            {
                _state = new HealthState(maxHealth);
            }

            _state.Reset();
        }
    }
}
