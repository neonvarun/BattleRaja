using BattleRaja.Core.Application;
using BattleRaja.Presentation.Movement;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    /// <summary>
    /// Applies the locally selected fighter to actor 1 without moving fighter rules into the
    /// menu. The selected controller remains a normal first-party ability component and bots
    /// continue to use their own configured controllers.
    /// </summary>
    [DefaultExecutionOrder(-200)]
    public sealed class PlayerFighterSelection : MonoBehaviour
    {
        [SerializeField] private FighterDefinitionAsset bijliDefinition;
        [SerializeField] private FighterDefinitionAsset pehelDefinition;
        [SerializeField] private FighterDefinitionAsset mayaDefinition;
        [SerializeField] private BijliFighterController bijliController;
        [SerializeField] private PehelFighterController pehelController;
        [SerializeField] private MayaFighterController mayaController;
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private CombatAttackController attackController;
        [SerializeField] private PlayerInputAdapter inputAdapter;

        public ProductionFighter ActiveFighter { get; private set; }

        private void Awake()
        {
            ResolveReferences();
            ApplySelection(ReadSelection());
        }

        private void Start()
        {
            // Disabled alternative controllers do not receive Awake until enabled. Applying
            // again in Start ensures their runtime state exists before the first input tick.
            ApplySelection(ReadSelection());
        }

        public void ApplySelection(ProductionFighter fighter)
        {
            ResolveReferences();
            ActiveFighter = fighter;
            MonoBehaviour selectedController = bijliController;
            var selectedDefinition = bijliDefinition;
            if (fighter == ProductionFighter.Pehel)
            {
                selectedController = pehelController;
                selectedDefinition = pehelDefinition;
            }
            else if (fighter == ProductionFighter.Maya)
            {
                selectedController = mayaController;
                selectedDefinition = mayaDefinition;
            }

            if (bijliController != null) bijliController.enabled = selectedController == bijliController;
            if (pehelController != null) pehelController.enabled = selectedController == pehelController;
            if (mayaController != null) mayaController.enabled = selectedController == mayaController;
            movementAgent?.SetFighterController(selectedController);
            attackController?.ConfigureFighter(selectedDefinition);
            inputAdapter?.ResetInputState();
        }

        private void ResolveReferences()
        {
            bijliDefinition = bijliDefinition != null ? bijliDefinition : GetComponent<FighterDefinitionAsset>();
            bijliController = bijliController != null ? bijliController : GetComponent<BijliFighterController>();
            pehelController = pehelController != null ? pehelController : GetComponent<PehelFighterController>();
            mayaController = mayaController != null ? mayaController : GetComponent<MayaFighterController>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            attackController = attackController != null ? attackController : GetComponent<CombatAttackController>();
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
        }

        private static ProductionFighter ReadSelection()
        {
            return (ProductionFighter)Mathf.Clamp(
                PlayerPrefs.GetInt("battleraja.selected_fighter", (int)ProductionFighter.Bijli), 0, 2);
        }
    }
}
