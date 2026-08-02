namespace BattleRaja.Core.Domain
{
    public interface IDamageMitigator
    {
        int Mitigate(DamageRequest request, int rawAmount);
    }

    public sealed class DamagePipeline
    {
        public DamageResult Apply(
            DamageRequest request,
            CombatEntityId targetId,
            CombatFaction targetFaction,
            HealthState targetHealth,
            bool allowSelfHit,
            bool allowFriendlyFire,
            IDamageMitigator mitigator = null)
        {
            if (request.TargetId != targetId)
            {
                return new DamageResult(false, 0, targetHealth.Snapshot.IsDefeated, DamageRejectionReason.WrongTarget);
            }

            if (!allowSelfHit && request.InstigatorId == targetId)
            {
                return new DamageResult(false, 0, targetHealth.Snapshot.IsDefeated, DamageRejectionReason.SelfHit);
            }

            if (!allowFriendlyFire && request.InstigatorFaction != CombatFaction.Neutral &&
                request.InstigatorFaction == targetFaction)
            {
                return new DamageResult(false, 0, targetHealth.Snapshot.IsDefeated, DamageRejectionReason.FriendlyFire);
            }

            var amount = mitigator == null ? request.RawAmount : mitigator.Mitigate(request, request.RawAmount);
            return targetHealth.ApplyDamage(amount);
        }
    }
}
