namespace BattleRaja.Core.Domain
{
    public readonly struct MovementInputFrame
    {
        public MovementInputFrame(Float2 movement, Float2 aim)
        {
            Movement = movement;
            Aim = aim;
        }

        public Float2 Movement { get; }
        public Float2 Aim { get; }
    }
}
