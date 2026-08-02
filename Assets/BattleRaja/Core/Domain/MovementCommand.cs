namespace BattleRaja.Core.Domain
{
    public readonly struct MovementCommand
    {
        public MovementCommand(int actorId, int simulationTick, Float2 movement, Float2 aim)
        {
            ActorId = actorId;
            SimulationTick = simulationTick;
            Movement = movement;
            Aim = aim;
        }

        public int ActorId { get; }
        public int SimulationTick { get; }
        public Float2 Movement { get; }
        public Float2 Aim { get; }

        public static MovementCommand Neutral(int actorId, int simulationTick) =>
            new MovementCommand(actorId, simulationTick, Float2.Zero, Float2.Zero);
    }
}
