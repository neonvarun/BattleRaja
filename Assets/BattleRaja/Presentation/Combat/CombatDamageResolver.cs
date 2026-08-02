using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatDamageResolver : MonoBehaviour
    {
        private readonly DamagePipeline _pipeline = new DamagePipeline();

        public DamageResult Resolve(
            CombatTarget target,
            DamageRequest request,
            bool allowSelfHit,
            bool allowFriendlyFire)
        {
            if (target == null || target.Health == null)
            {
                return new DamageResult(false, 0, false, DamageRejectionReason.WrongTarget);
            }

            return target.Health.ApplyThroughPipeline(
                _pipeline,
                request,
                target.Id,
                target.Faction,
                allowSelfHit,
                allowFriendlyFire);
        }
    }
}
