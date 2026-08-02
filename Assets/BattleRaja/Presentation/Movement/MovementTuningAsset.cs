using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Movement
{
    [CreateAssetMenu(menuName = "BattleRaja/Movement Tuning", fileName = "MovementTuning")]
    public sealed class MovementTuningAsset : ScriptableObject
    {
        [Min(0.01f)] [SerializeField] private float maxSpeed = 5.5f;
        [Min(0.01f)] [SerializeField] private float acceleration = 24f;
        [Min(0.01f)] [SerializeField] private float deceleration = 30f;
        [Min(0.01f)] [SerializeField] private float rotationSpeed = 720f;
        [Range(0f, 0.99f)] [SerializeField] private float movementDeadZone = 0.12f;
        [Range(0f, 0.99f)] [SerializeField] private float aimDeadZone = 0.14f;
        [Min(0.01f)] [SerializeField] private float inputSensitivity = 1f;

        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float RotationSpeed => rotationSpeed;
        public float MovementDeadZone => movementDeadZone;
        public float AimDeadZone => aimDeadZone;
        public float InputSensitivity => inputSensitivity;

        public MovementTuning ToDomain()
        {
            return new MovementTuning(
                maxSpeed,
                acceleration,
                deceleration,
                rotationSpeed,
                movementDeadZone,
                aimDeadZone,
                inputSensitivity);
        }

        private void OnValidate()
        {
            try
            {
                ToDomain();
            }
            catch (System.ArgumentOutOfRangeException exception)
            {
                Debug.LogError($"Invalid movement tuning: {exception.Message}", this);
            }
        }
    }
}
