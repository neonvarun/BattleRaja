using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.Combat
{
    public sealed class BijliHud : MonoBehaviour
    {
        [SerializeField] private BijliFighterController fighter;
        [SerializeField] private CombatHealth health;
        [SerializeField] private CombatAttackController attack;
        [SerializeField] private Text statusText;

        private void Update()
        {
            if (statusText == null)
            {
                return;
            }

            var hp = health != null ? health.Snapshot : default;
            var dash = fighter != null
                ? $"DASH {fighter.ActionState.ToString().ToUpperInvariant()} {(fighter.DashCooldownRemaining > 0.01f ? fighter.DashCooldownRemaining.ToString("0.0") + "s" : "READY")}"
                : "DASH --";
            var bolt = attack != null
                ? $"BOLT {(attack.CooldownRemaining > 0.01f ? attack.CooldownRemaining.ToString("0.0") + "s" : "READY")}"
                : "BOLT --";
            statusText.text = $"BIJLI   HP {hp.CurrentHealth}/{hp.MaxHealth}\n{bolt}   {dash}";
        }

        public void Configure(BijliFighterController fighterController, CombatHealth fighterHealth, CombatAttackController attackController, Text text)
        {
            fighter = fighterController;
            health = fighterHealth;
            attack = attackController;
            statusText = text;
        }
    }
}
