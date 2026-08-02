namespace BattleRaja.Core.Domain
{
    public enum DamageType
    {
        Generic = 0,
        Projectile = 1
    }

    public readonly struct DamageRequest
    {
        public DamageRequest(
            CombatEntityId instigatorId,
            CombatEntityId targetId,
            CombatFaction instigatorFaction,
            int rawAmount,
            DamageType damageType)
        {
            InstigatorId = instigatorId;
            TargetId = targetId;
            InstigatorFaction = instigatorFaction;
            RawAmount = rawAmount;
            DamageType = damageType;
        }

        public CombatEntityId InstigatorId { get; }
        public CombatEntityId TargetId { get; }
        public CombatFaction InstigatorFaction { get; }
        public int RawAmount { get; }
        public DamageType DamageType { get; }
    }

    public enum DamageRejectionReason
    {
        None = 0,
        InvalidAmount = 1,
        WrongTarget = 2,
        SelfHit = 3,
        FriendlyFire = 4,
        AlreadyDefeated = 5
    }

    public readonly struct DamageResult
    {
        public DamageResult(
            bool applied,
            int amountApplied,
            bool targetDefeated,
            DamageRejectionReason rejectionReason)
        {
            Applied = applied;
            AmountApplied = amountApplied;
            TargetDefeated = targetDefeated;
            RejectionReason = rejectionReason;
        }

        public bool Applied { get; }
        public int AmountApplied { get; }
        public bool TargetDefeated { get; }
        public DamageRejectionReason RejectionReason { get; }
    }
}
