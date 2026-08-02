using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using UnityEngine;

namespace BattleRaja.Presentation.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class MovementPlayerAgent : MonoBehaviour, IMovementCommandSink
    {
        [SerializeField] private int actorId = 1;
        [SerializeField] private MovementTuningAsset tuningAsset;
        [SerializeField] private PlayerInputAdapter inputAdapter;
        [SerializeField] private AimDirectionIndicator aimIndicator;
        [SerializeField] private BijliFighterController fighterController;

        private CharacterController _characterController;
        private MovementMotor _motor;
        private MovementTuning _tuning;
        private int _simulationTick;
        private bool _initialized;

        public int ActorId => actorId;
        public Float2 AimDirection => _motor != null ? _motor.AimDirection : Float2.Up;
        public Float2 Velocity => _motor != null ? _motor.Velocity : Float2.Zero;
        public MovementTuning Tuning => _tuning;
        public bool IsInitialized => _initialized;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            fighterController = fighterController != null ? fighterController : GetComponent<BijliFighterController>();
            _tuning = tuningAsset != null ? tuningAsset.ToDomain() : MovementTuning.Default;
            _motor = new MovementMotor();
            _simulationTick = 0;
            _initialized = true;
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }

            if (fighterController != null && fighterController.IsMovementLocked)
            {
                return;
            }

            var input = inputAdapter != null && inputAdapter.isActiveAndEnabled
                ? inputAdapter.ReadInput()
                : new MovementInputFrame(Float2.Zero, Float2.Zero);
            Submit(MovementCommandFactory.Create(actorId, _simulationTick, input, _tuning), Time.deltaTime);
            _simulationTick++;
        }

        public void Submit(MovementCommand command)
        {
            Submit(command, Time.deltaTime);
        }

        public void Submit(MovementCommand command, float deltaSeconds)
        {
            if (!_initialized)
            {
                return;
            }

            deltaSeconds = Mathf.Max(0f, deltaSeconds);
            var step = _motor.Step(command, deltaSeconds, _tuning);
            _characterController.Move(new Vector3(step.Displacement.X, 0f, step.Displacement.Y));

            if (step.AimDirection.SqrMagnitude > 0.000001f)
            {
                var targetRotation = Quaternion.LookRotation(new Vector3(step.AimDirection.X, 0f, step.AimDirection.Y), Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _tuning.RotationSpeed * deltaSeconds);
            }

            if (aimIndicator != null)
            {
                aimIndicator.SetAimDirection(step.AimDirection);
            }
        }

        public void ResetMovement(Float2 aimDirection)
        {
            _motor?.Reset(aimDirection);
        }
    }
}
