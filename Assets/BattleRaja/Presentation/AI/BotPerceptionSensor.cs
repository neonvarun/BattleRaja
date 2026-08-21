using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Match;
using UnityEngine;

namespace BattleRaja.Presentation.AI
{
    public sealed class BotPerceptionSensor : MonoBehaviour
    {
        [SerializeField] private int actorId = 10;
        [SerializeField] private CombatHealth health;
        [SerializeField] private CombatTarget selfTarget;
        [SerializeField] private Transform eye;
        [SerializeField] private OfflineMatchController match;
        [SerializeField] private LayerMask lineOfSightMask = 1;
        [SerializeField] private int maxTargets = 16;

        private CombatTarget[] _targets = new CombatTarget[0];
        private BotObservedTarget[] _observations = new BotObservedTarget[16];
        private readonly RaycastHit[] _lineOfSightHits = new RaycastHit[8];
        private Transform _selfRoot;

        public BotPerceptionSnapshot LastSnapshot { get; private set; }

        private void Awake()
        {
            health = health != null ? health : GetComponent<CombatHealth>();
            selfTarget = selfTarget != null ? selfTarget : GetComponent<CombatTarget>();
            eye = eye != null ? eye : transform;
            match = match != null ? match : FindAnyObjectByType<OfflineMatchController>();
            _selfRoot = selfTarget != null ? selfTarget.transform.root : transform.root;
            RefreshTargets();
        }

        public void RefreshTargets()
        {
            _targets = FindObjectsByType<CombatTarget>();
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
                var toTarget = targetPosition - eye.position;
                var distance = toTarget.magnitude;
                bool visible;
                if (distance <= 0.0001f)
                {
                    visible = true;
                }
                else
                {
                    // A plain Linecast would terminate inside the target's own
                    // CharacterController hull and always report a block. Cast a
                    // bounded ray and ignore colliders belonging to either
                    // endpoint actor; genuine cover between them still blocks.
                    var hits = Physics.RaycastNonAlloc(
                        eye.position,
                        toTarget / distance,
                        _lineOfSightHits,
                        distance,
                        lineOfSightMask,
                        QueryTriggerInteraction.Ignore);
                    visible = true;
                    for (var hitIndex = 0; hitIndex < hits; hitIndex++)
                    {
                        var hitCollider = _lineOfSightHits[hitIndex].collider;
                        // Fighter hulls (including both endpoints and third
                        // parties) never occlude perception; only world cover
                        // such as walls and stalls blocks line of sight.
                        var hitTarget = hitCollider != null ? hitCollider.GetComponentInParent<CombatTarget>() : null;
                        if (hitTarget != null)
                        {
                            continue;
                        }

                        visible = false;
                        break;
                    }
                }

                _observations[count++] = new BotObservedTarget(
                    candidate.Id,
                    candidate.Faction,
                    new Float2(targetPosition.x, targetPosition.z),
                    candidate.Health.Snapshot.CurrentHealth,
                    visible);
            }

            var current = health != null ? health.Snapshot : default;
            var zone = match != null
                ? new BotZoneObservation(match.ZoneCenter, match.ZoneRadius, match.NextZoneCenter, match.NextZoneRadius)
                : BotZoneObservation.Unbounded;
            LastSnapshot = new BotPerceptionSnapshot(
                selfTarget != null ? selfTarget.Id : new CombatEntityId(actorId),
                new Float2(transform.position.x, transform.position.z),
                current.CurrentHealth,
                current.MaxHealth,
                _observations,
                count,
                zone);
            return LastSnapshot;
        }
    }
}
