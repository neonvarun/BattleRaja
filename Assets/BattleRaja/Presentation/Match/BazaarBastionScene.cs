using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using UnityEngine;

namespace BattleRaja.Presentation.Match
{
    /// <summary>
    /// Production-scene contract for the Bazaar Bastion vertical slice.
    /// This marker keeps production orchestration separate from the MovementLab fixture
    /// while exposing only the references that production flow and validation need.
    /// </summary>
    public sealed class BazaarBastionScene : MonoBehaviour
    {
        [SerializeField] private MovementPlayerAgent player;
        [SerializeField] private TopDownCameraController cameraController;
        [SerializeField] private OfflineMatchController matchController;
        [SerializeField] private CombatProjectilePool projectilePool;
        [SerializeField] private CombatDamageResolver damageResolver;

        public MovementPlayerAgent Player => player;
        public TopDownCameraController CameraController => cameraController;
        public OfflineMatchController MatchController => matchController;
        public CombatProjectilePool ProjectilePool => projectilePool;
        public CombatDamageResolver DamageResolver => damageResolver;
    }
}
