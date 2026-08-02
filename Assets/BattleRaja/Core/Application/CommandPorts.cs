using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public interface IGameplayCommandSink
    {
        void Submit(GameplayCommand command);
    }

    public interface IMovementCommandSink
    {
        void Submit(MovementCommand command);
    }
}
