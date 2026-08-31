namespace BattleRaja.Core.Domain
{
    public enum DamageType
    {
        Generic = 0,
        Projectile = 1,
        Aandhi = 2,
        Ability = 3
    }

    public readonly struct DamageRequest
    {
        public DamageRequest(
            CombatEntityId instigatorId,
            CombatEntityId targetId,
            CombatFaction instigatorFaction,
            int rawAmount,
            DamageType damageType)
            : this(instigatorId, targetId, instigatorFaction, rawAmount, damageType, Float2.Zero, 0)
        {
        }

        public DamageRequest(
            CombatEntityId instigatorId,
            CombatEntityId targetId,
            CombatFaction instigatorFaction,
            int rawAmount,
            DamageType damageType,
            Float2 hitDirection)
            : this(instigatorId, targetId, instigatorFaction, rawAmount, damageType, hitDirection, 0)
        {
        }

        public DamageRequest(
            CombatEntityId instigatorId,
            CombatEntityId targetId,
            CombatFaction instigatorFaction,
            int rawAmount,
            DamageType damageType,
            Float2 hitDirection,
            int simulationTick)
        {
            InstigatorId = instigatorId;
            TargetId = targetId;
            InstigatorFaction = instigatorFaction;
            RawAmount = rawAmount;
            DamageType = damageType;
            HitDirection = hitDirection;
            SimulationTick = simulationTick;
        }

        public CombatEntityId InstigatorId { get; }
        public CombatEntityId TargetId { get; }
        public CombatFaction InstigatorFaction { get; }
        public int RawAmount { get; }
        public DamageType DamageType { get; }
        public Float2 HitDirection { get; }
        public int SimulationTick { get; }
    }

    public enum DamageRejectionReason
    {
        None = 0,
        InvalidAmount = 1,
        WrongTarget = 2,
        SelfHit = 3,
        FriendlyFire = 4,
        AlreadyDefeated = 5,
        SpawnProtection = 6
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
            int simulationTick,
            int eventId = 0)
        {
            Request = request;
            AmountApplied = amountApplied;
            TargetDefeated = targetDefeated;
            CurrentHealthAfter = currentHealthAfter;
            SimulationTick = simulationTick;
            EventId = eventId;
        }

        public DamageRequest Request { get; }
        public CombatEntityId InstigatorId => Request.InstigatorId;
        public CombatEntityId TargetId => Request.TargetId;
        public DamageType DamageType => Request.DamageType;
        public int AmountApplied { get; }
        public bool TargetDefeated { get; }
        public int CurrentHealthAfter { get; }
        public int SimulationTick { get; }

        /// <summary>
        /// Stable authority-assigned identity (0 while unassigned). Assigned by
        /// the match simulation when the event is recorded, so transports can
        /// reject retransmissions independently per event.
        /// </summary>
        public int EventId { get; }
    }
}
