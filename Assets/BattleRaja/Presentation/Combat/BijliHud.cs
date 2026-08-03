using BattleRaja.Core.Application;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.Combat
{
    public sealed class BijliHud : MonoBehaviour
    {
        [SerializeField] private BijliFighterController fighter;
        [SerializeField] private PlayerFighterSelection selection;
        [SerializeField] private PehelFighterController pehel;
        [SerializeField] private MayaFighterController maya;
        [SerializeField] private CombatHealth health;
        [SerializeField] private CombatAttackController attack;
        [SerializeField] private Text statusText;

        private void Update()
        {
            if (statusText == null)
            {
                return;
            }

            ResolveSelectionReferences();
            var hp = health != null ? health.Snapshot : default;
            var activeFighter = selection != null ? selection.ActiveFighter : ProductionFighter.Bijli;
            var bolt = attack != null
                ? $"BOLT {(attack.CooldownRemaining > 0.01f ? attack.CooldownRemaining.ToString("0.0") + "s" : "READY")}"
                : "BOLT --";
            var ability = fighter != null
                ? $"DASH {fighter.ActionState.ToString().ToUpperInvariant()} {(fighter.DashCooldownRemaining > 0.01f ? fighter.DashCooldownRemaining.ToString("0.0") + "s" : "READY")}"
                : "DASH --";

            if (activeFighter == ProductionFighter.Pehel)
            {
                ability = pehel != null
                    ? $"CHARGE {pehel.ActionState.ToString().ToUpperInvariant()} {(pehel.AbilityCooldownRemaining > 0.01f ? pehel.AbilityCooldownRemaining.ToString("0.0") + "s" : "READY")}"
                    : "CHARGE --";
            }
            else if (activeFighter == ProductionFighter.Maya)
            {
                ability = maya != null
                    ? $"DECOY {(maya.IsDecoyActive ? "ACTIVE" : maya.AbilityCooldownRemaining > 0.01f ? maya.AbilityCooldownRemaining.ToString("0.0") + "s" : "READY")}"
                    : "DECOY --";
            }

            statusText.text = FormatStatus(activeFighter, hp.CurrentHealth, hp.MaxHealth, bolt, ability);
        }

        public static string FormatStatus(ProductionFighter fighterType, int currentHealth, int maxHealth, string basicAttack, string ability)
        {
            return $"{fighterType.ToString().ToUpperInvariant()}   HP {currentHealth}/{maxHealth}\n{basicAttack}   {ability}";
        }

        public void Configure(BijliFighterController fighterController, CombatHealth fighterHealth, CombatAttackController attackController, Text text)
        {
            fighter = fighterController;
            health = fighterHealth;
            attack = attackController;
            statusText = text;
        }

        private void ResolveSelectionReferences()
        {
            if (selection != null) return;
            selection = FindFirstObjectByType<PlayerFighterSelection>();
            if (selection == null) return;

            var player = selection.gameObject;
            pehel = pehel != null ? pehel : player.GetComponent<PehelFighterController>();
            maya = maya != null ? maya : player.GetComponent<MayaFighterController>();
            fighter = fighter != null ? fighter : player.GetComponent<BijliFighterController>();
            health = health != null ? health : player.GetComponent<CombatHealth>();
            attack = attack != null ? attack : player.GetComponent<CombatAttackController>();
        }
    }
}
