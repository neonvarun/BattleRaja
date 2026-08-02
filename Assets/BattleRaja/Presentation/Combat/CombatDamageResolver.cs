using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Gadgets;
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

            var gadgetUser = target.GetComponent<GadgetUser>();
            if (gadgetUser != null)
            {
                var mitigated = gadgetUser.ModifyIncomingDamage(request);
                if (mitigated != request.RawAmount)
                {
                    request = new DamageRequest(request.InstigatorId, request.TargetId,
                        request.InstigatorFaction, mitigated, request.DamageType, request.HitDirection);
                }
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
