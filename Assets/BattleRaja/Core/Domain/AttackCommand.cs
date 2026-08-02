namespace BattleRaja.Core.Domain
{
    public readonly struct AttackCommand
    {
        public AttackCommand(
            CombatEntityId instigatorId,
            int simulationTick,
            Float2 origin,
            Float2 direction,
            bool pressed)
        {
            InstigatorId = instigatorId;
            SimulationTick = simulationTick;
            Origin = origin;
            Direction = direction.SqrMagnitude > 0.000001f ? direction.Normalized : Float2.Up;
            Pressed = pressed;
        }

        public CombatEntityId InstigatorId { get; }
        public int SimulationTick { get; }
        public Float2 Origin { get; }
        public Float2 Direction { get; }
        public bool Pressed { get; }
    }

    public static class AttackCommandFactory
    {
        public static AttackCommand Create(
            CombatEntityId instigatorId,
            int simulationTick,
            Float2 origin,
            Float2 direction,
            bool pressed)
        {
            return new AttackCommand(instigatorId, simulationTick, origin, direction, pressed);
        }
    }
}
