namespace BattleRaja.Core.Domain
{
    public readonly struct AbilityCommand
    {
        public AbilityCommand(
            CombatEntityId instigatorId,
            int simulationTick,
            ContentId abilityId,
            Float2 requestedDirection,
            bool pressed)
        {
            InstigatorId = instigatorId;
            SimulationTick = simulationTick;
            AbilityId = abilityId;
            RequestedDirection = requestedDirection;
            Pressed = pressed;
        }

        public CombatEntityId InstigatorId { get; }
        public int SimulationTick { get; }
        public ContentId AbilityId { get; }
        public Float2 RequestedDirection { get; }
        public bool Pressed { get; }
    }

    public static class AbilityCommandFactory
    {
        public static AbilityCommand Create(
            CombatEntityId instigatorId,
            int simulationTick,
            ContentId abilityId,
            Float2 requestedDirection,
            bool pressed)
        {
            return new AbilityCommand(instigatorId, simulationTick, abilityId, requestedDirection, pressed);
        }
    }
}
