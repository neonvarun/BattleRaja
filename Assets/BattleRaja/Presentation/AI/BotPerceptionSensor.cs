using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using UnityEngine;

namespace BattleRaja.Presentation.AI
{
    public sealed class BotPerceptionSensor : MonoBehaviour
    {
        [SerializeField] private int actorId = 10;
        [SerializeField] private CombatHealth health;
        [SerializeField] private CombatTarget selfTarget;
        [SerializeField] private Transform eye;
        [SerializeField] private LayerMask lineOfSightMask = 1;
        [SerializeField] private int maxTargets = 16;

        private CombatTarget[] _targets = new CombatTarget[0];
        private BotObservedTarget[] _observations = new BotObservedTarget[16];

        public BotPerceptionSnapshot LastSnapshot { get; private set; }

        private void Awake()
        {
            health = health != null ? health : GetComponent<CombatHealth>();
            selfTarget = selfTarget != null ? selfTarget : GetComponent<CombatTarget>();
            eye = eye != null ? eye : transform;
            RefreshTargets();
        }

        public void RefreshTargets()
        {
            _targets = FindObjectsByType<CombatTarget>(FindObjectsSortMode.None);
            var size = Mathf.Max(1, maxTargets);
            if (_observations.Length != size)
            {
                _observations = new BotObservedTarget[size];
            }
        }

        public BotPerceptionSnapshot Capture()
        {
            var count = 0;
            for (var i = 0; i < _targets.Length && count < _observations.Length; i++)
            {
                var candidate = _targets[i];
                if (candidate == null || candidate == selfTarget || candidate.Health == null || candidate.Health.Snapshot.IsDefeated)
                {
                    continue;
                }

                var targetPosition = candidate.transform.position;
                var direction = targetPosition - eye.position;
                var visible = !Physics.Linecast(eye.position, targetPosition, lineOfSightMask, QueryTriggerInteraction.Ignore);
                _observations[count++] = new BotObservedTarget(
                    candidate.Id,
                    candidate.Faction,
                    new Float2(targetPosition.x, targetPosition.z),
                    candidate.Health.Snapshot.CurrentHealth,
                    visible);
            }

            var current = health != null ? health.Snapshot : default;
            LastSnapshot = new BotPerceptionSnapshot(
                selfTarget != null ? selfTarget.Id : new CombatEntityId(actorId),
                new Float2(transform.position.x, transform.position.z),
                current.CurrentHealth,
                current.MaxHealth,
                _observations,
                count);
            return LastSnapshot;
        }
    }
}
