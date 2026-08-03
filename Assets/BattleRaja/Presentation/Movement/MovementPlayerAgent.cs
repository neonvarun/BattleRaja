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
        [SerializeField] private MonoBehaviour fighterController;
        [SerializeField] private bool externalCommandMode;
        [SerializeField] private int simulationTickRate = 30;

        private CharacterController _characterController;
        private MovementMotor _motor;
        private IFighterMovementLock _fighterMovementLock;
        private MovementTuning _tuning;
        private FixedSimulationClock _clock;
        private MovementInputFrame _bufferedInput;
        private int _simulationTick;
        private bool _initialized;

        public int ActorId => actorId;
        public Float2 AimDirection => _motor != null ? _motor.AimDirection : Float2.Up;
        public Float2 Velocity => _motor != null ? _motor.Velocity : Float2.Zero;
        public MovementTuning Tuning => _tuning;
        public bool IsInitialized => _initialized;
        public bool ExternalCommandMode { get => externalCommandMode; set => externalCommandMode = value; }

        /// <summary>
        /// Updates the ability controller used for movement locks when a production match
        /// applies the player's locally selected fighter after scene load.
        /// </summary>
        public void SetFighterController(MonoBehaviour controller)
        {
            fighterController = controller;
            _fighterMovementLock = controller as IFighterMovementLock;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            fighterController = fighterController != null ? fighterController : GetComponent<BijliFighterController>();
            _fighterMovementLock = fighterController as IFighterMovementLock;
            _tuning = tuningAsset != null ? tuningAsset.ToDomain() : MovementTuning.Default;
            _motor = new MovementMotor();
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
            _bufferedInput = new MovementInputFrame(Float2.Zero, Float2.Zero);
            _simulationTick = 0;
            _initialized = true;
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }

            if (_fighterMovementLock != null && _fighterMovementLock.IsMovementLocked)
            {
                return;
            }

            if (externalCommandMode)
            {
                return;
            }

            _bufferedInput = inputAdapter != null && inputAdapter.isActiveAndEnabled
                ? inputAdapter.ReadInput()
                : new MovementInputFrame(Float2.Zero, Float2.Zero);
            var steps = _clock.Consume(Time.deltaTime);
            for (var step = 0; step < steps; step++)
            {
                _simulationTick = _clock.GetConsumedTick(step);
                Submit(MovementCommandFactory.Create(actorId, _simulationTick, _bufferedInput, _tuning), (float)_clock.StepSeconds);
            }
        }

        public void Submit(MovementCommand command)
        {
            Submit(command, Time.deltaTime);
        }

        public void Submit(MovementCommand command, float deltaSeconds)
        {
            if (!_initialized || (_fighterMovementLock != null && _fighterMovementLock.IsMovementLocked))
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
