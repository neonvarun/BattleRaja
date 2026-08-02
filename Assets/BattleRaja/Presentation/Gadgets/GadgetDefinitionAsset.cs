using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Gadgets
{
    [CreateAssetMenu(menuName = "BattleRaja/Gadget Definition")]
    public sealed class GadgetDefinitionAsset : ScriptableObject
    {
        [SerializeField] private string gadgetId = "gadget.umbrella_guard";
        [SerializeField] private GadgetKind kind = GadgetKind.UmbrellaGuard;
        [Min(0.1f)] [SerializeField] private float cooldownSeconds = 12f;
        [Min(0.1f)] [SerializeField] private float durationSeconds = 3.5f;
        [Min(0.1f)] [SerializeField] private float radius = 1.6f;
        [Min(1)] [SerializeField] private int magnitude = 70;
        [Min(1)] [SerializeField] private int stationHealth = 1;
        [Min(0.1f)] [SerializeField] private float placementRadius = 0.1f;

        public GadgetDefinition ToDomain()
        {
            return new GadgetDefinition(
                ContentId.Gadget(gadgetId), kind, cooldownSeconds, durationSeconds,
                radius, magnitude, stationHealth, placementRadius);
        }
    }
}
