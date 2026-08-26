using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Match;
using UnityEngine;

namespace BattleRaja.Presentation.AI
{
    public sealed class BotPerceptionSensor : MonoBehaviour
    {
        [SerializeField] private int actorId = 10;
        [SerializeField] private CombatHealth health;
        [SerializeField] private CombatTarget selfTarget;
        [SerializeField] private ProjectileWeaponAsset weaponAsset;
        [SerializeField] private Transform eye;
        [SerializeField] private OfflineMatchController match;
        [SerializeField] private GadgetUser gadgetUser;
        [SerializeField] private LayerMask lineOfSightMask = 1;
        [SerializeField] private int maxTargets = 16;

        private CombatTarget[] _targets = new CombatTarget[0];
        private GadgetPickup[] _gadgetPickups = new GadgetPickup[0];
        private BotObservedTarget[] _observations = new BotObservedTarget[16];
        private readonly RaycastHit[] _lineOfSightHits = new RaycastHit[8];
        private Transform _selfRoot;
        private DeterministicCollisionSolver _movementCollisionSolver;
        private int _lastAttackerId;
        private int _lastAttackerTick = int.MinValue;
        private ProjectileWeaponDefinition _autonomousWeapon;
        private bool _hasAutonomousWeapon;

        public BotPerceptionSnapshot LastSnapshot { get; private set; }
        public int CaptureCount { get; private set; }
        public int MaxVisibleTargetCount { get; private set; }
        public int MaxHostileTargetCount { get; private set; }
        public int HostileCaptureCount { get; private set; }

        public void ResetTelemetry()
        {
            CaptureCount = 0;
            MaxVisibleTargetCount = 0;
            MaxHostileTargetCount = 0;
            HostileCaptureCount = 0;
            _lastAttackerId = 0;
            _lastAttackerTick = int.MinValue;
        }

        public bool IsWorldBlocked(Float2 direction, float lookahead = 1.35f)
        {
            if (direction.SqrMagnitude <= 0.000001f || lookahead <= 0f) return false;
            var minimumProgress = Mathf.Min(lookahead, 0.25f);
            return GetWorldMovementProgress(direction, lookahead) < minimumProgress;
        }

        /// <summary>
        /// Returns the distance the authoritative collision contract would allow for
        /// a short probe. Bot steering uses this to choose the most useful escape
        /// direction at wall and obstacle corners instead of accepting the first
        /// direction that is merely non-zero.
        /// </summary>
        public float GetWorldMovementProgress(Float2 direction, float lookahead = 1.35f)
        {
            if (direction.SqrMagnitude <= 0.000001f || lookahead <= 0f) return 0f;
            if (_movementCollisionSolver == null) return lookahead;

            var current = new Float2(transform.position.x, transform.position.z);
            var predicted = _movementCollisionSolver.Move(current, direction.Normalized * lookahead);
            return predicted.AppliedDisplacement.Magnitude;
        }

        public void ConfigureAutonomousWeapon(ProjectileWeaponDefinition definition)
        {
            _autonomousWeapon = definition;
            _hasAutonomousWeapon = true;
        }

        public bool IsTargetWithinRange(CombatEntityId targetId, float maxRange)
        {
            if (targetId.Value == 0 || maxRange <= 0f) return false;
            for (var i = 0; i < _targets.Length; i++)
            {
                var candidate = _targets[i];
                if (candidate == null || candidate.Id != targetId) continue;

                if (candidate.Health == null || candidate.Health.Snapshot.IsDefeated) continue;
                var targetPosition = candidate.transform.position;
                var dx = targetPosition.x - transform.position.x;
                var dz = targetPosition.z - transform.position.z;
                return (dx * dx) + (dz * dz) <= maxRange * maxRange;
            }

            return false;
        }

        public bool TryGetCurrentTargetAim(CombatEntityId targetId, out Float2 aim)
        {
            aim = Float2.Zero;
            for (var i = 0; i < _targets.Length; i++)
            {
                var candidate = _targets[i];
                if (candidate == null || candidate.Id != targetId) continue;

                if (candidate.Health == null || candidate.Health.Snapshot.IsDefeated) continue;
                var delta = candidate.transform.position - transform.position;
                var direction = new Float2(delta.x, delta.z);
                if (direction.SqrMagnitude <= 0.000001f) return false;
                aim = direction.Normalized;
                return true;
            }

            return false;
        }

        private void Awake()
        {
            health = health != null ? health : GetComponent<CombatHealth>();
            selfTarget = selfTarget != null ? selfTarget : GetComponent<CombatTarget>();
            eye = eye != null ? eye : transform;
            match = match != null ? match : FindAnyObjectByType<OfflineMatchController>();
            gadgetUser = gadgetUser != null ? gadgetUser : GetComponent<GadgetUser>();
            _selfRoot = selfTarget != null ? selfTarget.transform.root : transform.root;
            _movementCollisionSolver = new DeterministicCollisionSolver(ArenaCollisionDefinition.BazaarBastion);
            if (health != null) health.DamageApplied += OnDamageApplied;
            RefreshTargets();
            RefreshGadgetPickups();
        }

        private void OnDestroy()
        {
            if (health != null) health.DamageApplied -= OnDamageApplied;
        }

        public void RefreshTargets()
        {
            _targets = FindObjectsByType<CombatTarget>();
            System.Array.Sort(_targets, CompareTargets);
            var size = Mathf.Max(1, maxTargets);
            if (_observations.Length != size)
            {
                _observations = new BotObservedTarget[size];
            }
        }

        public void RefreshGadgetPickups()
        {
            _gadgetPickups = FindObjectsByType<GadgetPickup>();
            System.Array.Sort(_gadgetPickups, CompareGadgetPickups);
        }

        private static int CompareTargets(CombatTarget left, CombatTarget right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left == null) return 1;
            if (right == null) return -1;

            var idComparison = left.Id.Value.CompareTo(right.Id.Value);
            if (idComparison != 0) return idComparison;
            var positionComparison = left.transform.position.x.CompareTo(right.transform.position.x);
            if (positionComparison != 0) return positionComparison;
            return left.transform.position.z.CompareTo(right.transform.position.z);
        }

        private static int CompareGadgetPickups(GadgetPickup left, GadgetPickup right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left == null) return 1;
            if (right == null) return -1;

            var xComparison = left.transform.position.x.CompareTo(right.transform.position.x);
            if (xComparison != 0) return xComparison;
            var zComparison = left.transform.position.z.CompareTo(right.transform.position.z);
            if (zComparison != 0) return zComparison;
            return string.CompareOrdinal(left.GadgetId.Value, right.GadgetId.Value);
        }

        public BotPerceptionSnapshot Capture()
        {
            var count = 0;
            var visibleTargetCount = 0;
            var hostileTargetCount = 0;
            var selfFaction = selfTarget != null ? selfTarget.Faction : CombatFaction.Enemy;
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
                        if (hitTarget != null) continue;
                        visible = false;
                        break;
                    }
                }

                var isHostile = candidate.Faction != CombatFaction.Neutral && candidate.Faction != selfFaction;
                if (match != null && match.IsMatchStarted && selfTarget != null)
                {
                    isHostile = match.AreActorsHostile(selfTarget.Id, candidate.Id);
                }

                if (visible) visibleTargetCount++;
                if (visible && isHostile) hostileTargetCount++;

                _observations[count++] = new BotObservedTarget(
                    candidate.Id,
                    candidate.Faction,
                    new Float2(targetPosition.x, targetPosition.z),
                    candidate.Health.Snapshot.CurrentHealth,
                    visible,
                    isHostile);
            }

            var current = health != null ? health.Snapshot : default;
            var zone = match != null
                ? new BotZoneObservation(match.ZoneCenter, match.ZoneRadius, match.NextZoneCenter, match.NextZoneRadius)
                : BotZoneObservation.Unbounded;
            var weapon = _hasAutonomousWeapon
                ? _autonomousWeapon
                : weaponAsset != null ? weaponAsset.ToDomain() : ProjectileWeaponDefinition.TrainingBolt;
            var hasNearbyGadget = false;
            var nearestGadgetPosition = Float2.Zero;
            var nearestGadgetDistanceSquared = float.MaxValue;
            for (var i = 0; i < _gadgetPickups.Length; i++)
            {
                var pickup = _gadgetPickups[i];
                if (pickup == null || !pickup.IsAvailable) continue;
                var pickupPosition = pickup.transform.position;
                var dx = pickupPosition.x - transform.position.x;
                var dz = pickupPosition.z - transform.position.z;
                var distanceSquared = (dx * dx) + (dz * dz);
                if (distanceSquared < nearestGadgetDistanceSquared)
                {
                    nearestGadgetDistanceSquared = distanceSquared;
                    nearestGadgetPosition = new Float2(pickupPosition.x, pickupPosition.z);
                    hasNearbyGadget = distanceSquared <= 18f * 18f;
                }
            }

            // The observation buffer is reused to avoid per-capture allocations.
            // Clear its inactive tail so consumers that inspect the array directly
            // cannot observe a defeated or previously visible target as stale data.
            for (var i = count; i < _observations.Length; i++)
            {
                _observations[i] = default(BotObservedTarget);
            }

            var currentTick = match != null ? match.SimulationTick : _lastAttackerTick;
            var recentAttacker = _lastAttackerId != 0 &&
                currentTick >= _lastAttackerTick && currentTick - _lastAttackerTick <= 60
                    ? new CombatEntityId(_lastAttackerId)
                    : default(CombatEntityId);
            LastSnapshot = new BotPerceptionSnapshot(
                selfTarget != null ? selfTarget.Id : new CombatEntityId(actorId),
                new Float2(transform.position.x, transform.position.z),
                current.CurrentHealth,
                current.MaxHealth,
                _observations,
                count,
                zone,
                selfFaction,
                weapon,
                recentAttacker,
                nearestGadgetPosition,
                    hasNearbyGadget,
                    gadgetUser != null && gadgetUser.HasGadget);
            CaptureCount++;
            if (hostileTargetCount > 0) HostileCaptureCount++;
            MaxVisibleTargetCount = Mathf.Max(MaxVisibleTargetCount, visibleTargetCount);
            MaxHostileTargetCount = Mathf.Max(MaxHostileTargetCount, hostileTargetCount);
            return LastSnapshot;
        }

        private void OnDamageApplied(CombatDamageEvent damageEvent)
        {
            if (selfTarget == null || damageEvent.TargetId != selfTarget.Id || damageEvent.InstigatorId.Value == 0 ||
                damageEvent.InstigatorId == selfTarget.Id) return;

            _lastAttackerId = damageEvent.InstigatorId.Value;
            _lastAttackerTick = damageEvent.SimulationTick;
        }

    }
}
