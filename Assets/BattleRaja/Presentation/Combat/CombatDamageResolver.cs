using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Match;
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
            bool allowFriendlyFire,
            int simulationTick = 0)
        {
            if (target == null || target.Health == null)
            {
                return new DamageResult(false, 0, false, DamageRejectionReason.WrongTarget);
            }

            var match = FindFirstObjectByType<OfflineMatchController>();
            var authoritative = match != null && match.Simulation != null;
            if (authoritative)
            {
                request = match.ApplyDamageMitigation(request);
            }
            else
            {
                var gadgetUser = target.GetComponent<GadgetUser>();
                if (gadgetUser != null)
                {
                    var mitigated = gadgetUser.ModifyIncomingDamage(request);
                    if (mitigated != request.RawAmount)
                    {
                        request = new DamageRequest(request.InstigatorId, request.TargetId,
                            request.InstigatorFaction, mitigated, request.DamageType, request.HitDirection,
                            request.SimulationTick);
                    }
                }
            }

            return target.Health.ApplyThroughPipeline(
                _pipeline,
                request,
                target.Id,
                target.Faction,
                allowSelfHit,
                allowFriendlyFire,
                simulationTick);
        }
    }
}
