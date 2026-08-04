using System;

namespace BattleRaja.Core.Domain
{
    /// <summary>
    /// Bounded identity sequence provider for deterministic match event tracking.
    /// Eliminates reliance on Unity InstanceIDs or un-sequenced callbacks.
    /// </summary>
    public sealed class MatchEventIdentityTracker
    {
        private int _attackExecutionId;
        private int _projectileId;
        private int _abilityExecutionId;
        private int _gadgetUseId;
        private int _damageEventId;
        private int _healingEventId;
        private int _collectionEventId;
        private int _eliminationEventId;

        public MatchEventIdentityTracker()
        {
            Reset();
        }

        public int NextAttackExecutionId() => Increment(ref _attackExecutionId);
        public int NextProjectileId() => Increment(ref _projectileId);
        public int NextAbilityExecutionId() => Increment(ref _abilityExecutionId);
        public int NextGadgetUseId() => Increment(ref _gadgetUseId);
        public int NextDamageEventId() => Increment(ref _damageEventId);
        public int NextHealingEventId() => Increment(ref _healingEventId);
        public int NextCollectionEventId() => Increment(ref _collectionEventId);
        public int NextEliminationEventId() => Increment(ref _eliminationEventId);

        public void Reset()
        {
            _attackExecutionId = 0;
            _projectileId = 0;
            _abilityExecutionId = 0;
            _gadgetUseId = 0;
            _damageEventId = 0;
            _healingEventId = 0;
            _collectionEventId = 0;
            _eliminationEventId = 0;
        }

        private static int Increment(ref int field)
        {
            if (field >= int.MaxValue - 1)
            {
                field = 1;
            }
            else
            {
                field++;
            }

            return field;
        }
    }
}
