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
            var station = target.GetComponent<GadgetStation>();
            var stationDamage = default(GadgetStationDamageResult);
            var stationDamageApplied = false;
            if (authoritative && station != null && station.StationId > 0)
            {
                if (request.TargetId != target.Id)
                {
                    return new DamageResult(false, 0, false, DamageRejectionReason.WrongTarget);
                }

                if (!allowSelfHit && request.InstigatorId == target.Id)
                {
                    return new DamageResult(false, 0, false, DamageRejectionReason.SelfHit);
                }

                if (!allowFriendlyFire && request.InstigatorFaction == target.Faction)
                {
                    return new DamageResult(false, 0, false, DamageRejectionReason.FriendlyFire);
                }

                if (target.Health.Snapshot.IsDefeated)
                {
                    return new DamageResult(false, 0, false, DamageRejectionReason.AlreadyDefeated);
                }

                stationDamage = match.TryDamageStation(station.StationId, request.RawAmount);
                if (!stationDamage.Applied)
                {
                    return new DamageResult(
                        false,
                        0,
                        stationDamage.Destroyed,
                        stationDamage.Destroyed ? DamageRejectionReason.AlreadyDefeated : DamageRejectionReason.WrongTarget);
                }

                request = new DamageRequest(
                    request.InstigatorId,
                    request.TargetId,
                    request.InstigatorFaction,
                    stationDamage.AmountApplied,
                    request.DamageType,
                    request.HitDirection,
                    request.SimulationTick);
                stationDamageApplied = true;
            }
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

            var result = target.Health.ApplyThroughPipeline(
                _pipeline,
                request,
                target.Id,
                target.Faction,
                allowSelfHit,
                allowFriendlyFire,
                simulationTick);
            if (stationDamageApplied && stationDamage.Destroyed)
            {
                station.ExpireFromAuthority();
            }

            return result;
        }
    }
}
