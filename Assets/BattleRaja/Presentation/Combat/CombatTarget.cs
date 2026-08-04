using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    [RequireComponent(typeof(CombatHealth))]
    public sealed class CombatTarget : MonoBehaviour
    {
        [SerializeField] private int entityId = 100;
        [SerializeField] private CombatFaction faction = CombatFaction.Enemy;
        [SerializeField] private CombatHealth health;

        public CombatEntityId Id => new CombatEntityId(entityId);
        public CombatFaction Faction => faction;
        public CombatHealth Health => health;

        public void Configure(int id, CombatFaction targetFaction, CombatHealth targetHealth)
        {
            entityId = id;
            faction = targetFaction;
            health = targetHealth != null ? targetHealth : health;
        }

        private void Awake()
        {
            health = health != null ? health : GetComponent<CombatHealth>();
        }
    }
}
