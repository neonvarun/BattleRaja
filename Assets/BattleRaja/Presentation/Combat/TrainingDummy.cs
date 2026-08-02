using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class TrainingDummy : MonoBehaviour
    {
        [SerializeField] private CombatTarget target;
        [SerializeField] private CombatHitFlash hitFlash;
        [SerializeField] private float resetDelaySeconds = 1f;

        private float _resetAt = -1f;

        public CombatTarget Target => target;
        public bool IsDefeated => target != null && target.Health != null && target.Health.Snapshot.IsDefeated;

        private void Awake()
        {
            target = target != null ? target : GetComponent<CombatTarget>();
            hitFlash = hitFlash != null ? hitFlash : GetComponent<CombatHitFlash>();
            if (target != null && target.Health != null)
            {
                target.Health.DamageResolved += OnDamageResolved;
            }
        }

        private void OnDestroy()
        {
            if (target != null && target.Health != null)
            {
                target.Health.DamageResolved -= OnDamageResolved;
            }
        }

        private void Update()
        {
            if (_resetAt > 0f && Time.time >= _resetAt)
            {
                ResetDummy();
            }
        }

        public void ResetDummy()
        {
            _resetAt = -1f;
            target?.Health.ResetHealth();
            gameObject.SetActive(true);
        }

        private void OnDamageResolved(DamageResult result)
        {
            hitFlash?.Flash(result);
            if (result.TargetDefeated)
            {
                _resetAt = Time.time + Mathf.Max(0f, resetDelaySeconds);
            }
        }
    }
}
