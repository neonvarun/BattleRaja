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

        public MatchEventIdentityCounters Snapshot() => new MatchEventIdentityCounters(
            _attackExecutionId,
            _projectileId,
            _abilityExecutionId,
            _gadgetUseId,
            _damageEventId,
            _healingEventId,
            _collectionEventId,
            _eliminationEventId);

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

    public readonly struct MatchEventIdentityCounters
    {
        public MatchEventIdentityCounters(
            int attackExecutionId,
            int projectileId,
            int abilityExecutionId,
            int gadgetUseId,
            int damageEventId,
            int healingEventId,
            int collectionEventId,
            int eliminationEventId)
        {
            AttackExecutionId = attackExecutionId;
            ProjectileId = projectileId;
            AbilityExecutionId = abilityExecutionId;
            GadgetUseId = gadgetUseId;
            DamageEventId = damageEventId;
            HealingEventId = healingEventId;
            CollectionEventId = collectionEventId;
            EliminationEventId = eliminationEventId;
        }

        public int AttackExecutionId { get; }
        public int ProjectileId { get; }
        public int AbilityExecutionId { get; }
        public int GadgetUseId { get; }
        public int DamageEventId { get; }
        public int HealingEventId { get; }
        public int CollectionEventId { get; }
        public int EliminationEventId { get; }
    }
}
