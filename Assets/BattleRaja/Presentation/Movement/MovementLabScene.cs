using UnityEngine;
using BattleRaja.Presentation.Combat;

namespace BattleRaja.Presentation.Movement
{
    public sealed class MovementLabScene : MonoBehaviour
    {
        [SerializeField] private MovementPlayerAgent player;
        [SerializeField] private TopDownCameraController cameraController;
        [SerializeField] private TrainingDummy trainingDummy;
        [SerializeField] private CombatProjectilePool projectilePool;
        [SerializeField] private CombatDamageResolver damageResolver;

        public MovementPlayerAgent Player => player;
        public TopDownCameraController CameraController => cameraController;
        public TrainingDummy TrainingDummy => trainingDummy;
        public CombatProjectilePool ProjectilePool => projectilePool;
        public CombatDamageResolver DamageResolver => damageResolver;
    }
}
