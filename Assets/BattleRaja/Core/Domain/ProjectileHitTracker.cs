using System.Collections.Generic;

namespace BattleRaja.Core.Domain
{
    public sealed class ProjectileHitTracker
    {
        private readonly HashSet<CombatEntityId> _hitTargets = new HashSet<CombatEntityId>();

        public bool TryRegister(CombatEntityId targetId) => _hitTargets.Add(targetId);
        public void Clear() => _hitTargets.Clear();
    }
}
