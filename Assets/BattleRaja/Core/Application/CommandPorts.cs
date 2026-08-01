using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public interface IGameplayCommandSink
    {
        void Submit(GameplayCommand command);
    }
}
