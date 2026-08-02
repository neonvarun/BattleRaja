using System;

namespace BattleRaja.Core.Domain
{
    public readonly struct FighterDefinition
    {
        public FighterDefinition(
            ContentId fighterId,
            string displayName,
            int maxHealth,
            MovementTuning movement,
            ProjectileWeaponDefinition basicAttack,
            DashAbilityDefinition ability)
        {
            FighterId = fighterId;
            DisplayName = displayName ?? string.Empty;
            MaxHealth = maxHealth;
            Movement = movement;
            BasicAttack = basicAttack;
            Ability = ability;
        }

        public ContentId FighterId { get; }
        public string DisplayName { get; }
        public int MaxHealth { get; }
        public MovementTuning Movement { get; }
        public ProjectileWeaponDefinition BasicAttack { get; }
        public DashAbilityDefinition Ability { get; }

        public bool IsValid(out string reason)
        {
            if (!FighterId.IsValid || FighterId.Kind != ContentIdKind.Fighter)
            {
                reason = "Fighter ID is missing or has the wrong kind.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(DisplayName) || MaxHealth <= 0)
            {
                reason = "Display name and max health are required.";
                return false;
            }

            if (!BasicAttack.IsValid(out reason))
            {
                return false;
            }

            return Ability.IsValid(out reason);
        }

        public static FighterDefinition Bijli => new FighterDefinition(
            ContentId.Fighter("fighter.bijli"),
            "Bijli",
            maxHealth: 85,
            MovementTuning.Default,
            ProjectileWeaponDefinition.BijliElectricBolt,
            DashAbilityDefinition.BijliElectricDash);
    }
}
