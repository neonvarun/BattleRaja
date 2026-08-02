using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    [CreateAssetMenu(menuName = "BattleRaja/Fighter Definition", fileName = "FighterDefinition")]
    public sealed class FighterDefinitionAsset : ScriptableObject
    {
        [SerializeField] private string fighterId = "fighter.bijli";
        [SerializeField] private string displayName = "Bijli";
        [Min(1)] [SerializeField] private int maxHealth = 85;
        [SerializeField] private MovementTuningAsset movementTuning;
        [SerializeField] private ProjectileWeaponAsset basicAttack;
        [SerializeField] private string abilityId = "ability.bijli.electric_dash";
        [Min(0.01f)] [SerializeField] private float abilityCooldownSeconds = 2.2f;
        [Min(0.01f)] [SerializeField] private float dashDistance = 4.2f;
        [Min(0f)] [SerializeField] private float startupSeconds = 0.08f;
        [Min(0.01f)] [SerializeField] private float activeSeconds = 0.16f;
        [Min(0f)] [SerializeField] private float recoverySeconds = 0.24f;
        [Min(0.01f)] [SerializeField] private float collisionRadius = 0.38f;

        public string FighterId => fighterId;
        public string DisplayName => displayName;
        public FighterDefinition ToDomain()
        {
            var movement = movementTuning != null ? movementTuning.ToDomain() : MovementTuning.Default;
            var attack = basicAttack != null ? basicAttack.ToDomain() : ProjectileWeaponDefinition.BijliElectricBolt;
            var ability = new DashAbilityDefinition(
                ContentId.Ability(abilityId),
                abilityCooldownSeconds,
                dashDistance,
                startupSeconds,
                activeSeconds,
                recoverySeconds,
                collisionRadius);
            return new FighterDefinition(
                ContentId.Fighter(fighterId),
                displayName,
                maxHealth,
                movement,
                attack,
                ability);
        }

        private void OnValidate()
        {
            try
            {
                if (!ToDomain().IsValid(out var reason))
                {
                    Debug.LogWarning($"{name} is invalid: {reason}", this);
                }
            }
            catch (System.ArgumentException exception)
            {
                Debug.LogWarning($"{name} is invalid: {exception.Message}", this);
            }
        }
    }
}
