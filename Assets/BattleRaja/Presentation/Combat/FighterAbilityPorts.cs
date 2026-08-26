using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;

namespace BattleRaja.Presentation.Combat
{
    public interface IFighterAbilityController : IAbilityCommandSink
    {
        ContentId AbilityId { get; }
        float AbilityCooldownRemaining { get; }
    }

    public interface IFighterMovementLock
    {
        bool IsMovementLocked { get; }
    }
}
