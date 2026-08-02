using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct DashAbilityDefinition
    {
        public DashAbilityDefinition(
            ContentId abilityId,
            float cooldownSeconds,
            float distance,
            float startupSeconds,
            float activeSeconds,
            float recoverySeconds,
            float collisionRadius)
        {
            AbilityId = abilityId;
            CooldownSeconds = cooldownSeconds;
            Distance = distance;
            StartupSeconds = startupSeconds;
            ActiveSeconds = activeSeconds;
            RecoverySeconds = recoverySeconds;
            CollisionRadius = collisionRadius;
        }

        public ContentId AbilityId { get; }
        public float CooldownSeconds { get; }
        public float Distance { get; }
        public float StartupSeconds { get; }
        public float ActiveSeconds { get; }
        public float RecoverySeconds { get; }
        public float CollisionRadius { get; }

        public static DashAbilityDefinition BijliElectricDash => new DashAbilityDefinition(
            ContentId.Ability("ability.bijli.electric_dash"),
            cooldownSeconds: 2.2f,
            distance: 4.2f,
            startupSeconds: 0.08f,
            activeSeconds: 0.16f,
            recoverySeconds: 0.24f,
            collisionRadius: 0.38f);

        public bool IsValid(out string reason)
        {
            if (!AbilityId.IsValid || AbilityId.Kind != ContentIdKind.Ability)
            {
                reason = "Ability ID is missing or has the wrong kind.";
                return false;
            }

            if (CooldownSeconds <= 0f || Distance <= 0f || ActiveSeconds <= 0f || CollisionRadius <= 0f)
            {
                reason = "Cooldown, distance, active duration and collision radius must be positive.";
                return false;
            }

            if (StartupSeconds < 0f || RecoverySeconds < 0f)
            {
                reason = "Startup and recovery durations must not be negative.";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }
}
