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
            : this(instigatorId, simulationTick, origin, direction, pressed, simulationTick)
        {
        }

        public AttackCommand(
            CombatEntityId instigatorId,
            int simulationTick,
            Float2 origin,
            Float2 direction,
            bool pressed,
            int inputSequence)
        {
            InstigatorId = instigatorId;
            SimulationTick = simulationTick;
            Origin = origin;
            // Preserve non-finite input so the authority can reject it. The old
            // normalization silently converted NaN/Infinity into Float2.Up,
            // making malformed network input indistinguishable from valid input.
            Direction = !direction.IsFinite
                ? direction
                : (direction.SqrMagnitude > 0.000001f ? direction.Normalized : Float2.Up);
            Pressed = pressed;
            InputSequence = inputSequence >= 0 ? inputSequence : simulationTick;
        }

        public CombatEntityId InstigatorId { get; }
        public int SimulationTick { get; }
        public Float2 Origin { get; }
        public Float2 Direction { get; }
        public bool Pressed { get; }
        public int InputSequence { get; }
    }

    public static class AttackCommandFactory
    {
        public static AttackCommand Create(
            CombatEntityId instigatorId,
            int simulationTick,
            Float2 origin,
            Float2 direction,
            bool pressed,
            int inputSequence = -1)
        {
            return new AttackCommand(instigatorId, simulationTick, origin, direction, pressed, inputSequence);
        }
    }
}
