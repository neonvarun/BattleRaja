namespace BattleRaja.Core.Domain
{
    public enum DamageType
    {
        Generic = 0,
        Projectile = 1,
        Aandhi = 2
    }

    public readonly struct DamageRequest
    {
        public DamageRequest(
            CombatEntityId instigatorId,
            CombatEntityId targetId,
            CombatFaction instigatorFaction,
            int rawAmount,
            DamageType damageType)
            : this(instigatorId, targetId, instigatorFaction, rawAmount, damageType, Float2.Zero)
        {
        }

        public DamageRequest(
            CombatEntityId instigatorId,
            CombatEntityId targetId,
            CombatFaction instigatorFaction,
            int rawAmount,
            DamageType damageType,
            Float2 hitDirection)
        {
            InstigatorId = instigatorId;
            TargetId = targetId;
            InstigatorFaction = instigatorFaction;
            RawAmount = rawAmount;
            DamageType = damageType;
            HitDirection = hitDirection;
        }

        public CombatEntityId InstigatorId { get; }
        public CombatEntityId TargetId { get; }
        public CombatFaction InstigatorFaction { get; }
        public int RawAmount { get; }
        public DamageType DamageType { get; }
        public Float2 HitDirection { get; }
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

    public readonly struct CombatDamageEvent
    {
        public CombatDamageEvent(
            DamageRequest request,
            int amountApplied,
            bool targetDefeated,
            int currentHealthAfter,
            int simulationTick)
        {
            Request = request;
            AmountApplied = amountApplied;
            TargetDefeated = targetDefeated;
            CurrentHealthAfter = currentHealthAfter;
            SimulationTick = simulationTick;
        }

        public DamageRequest Request { get; }
        public CombatEntityId InstigatorId => Request.InstigatorId;
        public CombatEntityId TargetId => Request.TargetId;
        public DamageType DamageType => Request.DamageType;
        public int AmountApplied { get; }
        public bool TargetDefeated { get; }
        public int CurrentHealthAfter { get; }
        public int SimulationTick { get; }
    }
}
