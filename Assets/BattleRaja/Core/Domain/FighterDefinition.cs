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

        public static FighterDefinition Pehel => new FighterDefinition(
            ContentId.Fighter("fighter.pehel"),
            "Pehel",
            maxHealth: 125,
            new MovementTuning(4.8f, 21f, 27f, 600f, 0.12f, 0.14f, 1f),
            new ProjectileWeaponDefinition(28, 0.72f, 10f, 5.5f, 0.7f, 0.45f, ~0, false, false),
            new DashAbilityDefinition(ContentId.Ability("ability.pehel.charge_throw"), 4.5f, 3.2f, 0.18f, 0.24f, 0.4f, 0.5f));

        public static FighterDefinition Maya => new FighterDefinition(
            ContentId.Fighter("fighter.maya"),
            "Maya",
            maxHealth: 72,
            new MovementTuning(6.2f, 27f, 33f, 780f, 0.12f, 0.14f, 1f),
            new ProjectileWeaponDefinition(12, 0.3f, 16f, 18f, 1.2f, 0.12f, ~0, false, false),
            new DashAbilityDefinition(ContentId.Ability("ability.maya.decoy"), 7f, 2.6f, 0.12f, 0.2f, 0.28f, 0.3f));
    }
}
