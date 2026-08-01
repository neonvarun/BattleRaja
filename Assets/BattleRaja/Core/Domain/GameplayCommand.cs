namespace BattleRaja.Core.Domain
{
    public enum GameplayCommandKind
    {
        None = 0,
        Intent = 1
    }

    public readonly struct GameplayCommand
    {
        public GameplayCommand(int actorId, int simulationTick, GameplayCommandKind kind, int payload)
        {
            ActorId = actorId;
            SimulationTick = simulationTick;
            Kind = kind;
            Payload = payload;
        }

        public int ActorId { get; }
        public int SimulationTick { get; }
        public GameplayCommandKind Kind { get; }
        public int Payload { get; }
    }
}
