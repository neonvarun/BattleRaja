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
        [SerializeField] private bool authorityDrivenMovement;
        [SerializeField] private int simulationTickRate = 30;

        private CharacterController _characterController;
        private MovementMotor _motor;
        private IFighterMovementLock _fighterMovementLock;
        private MovementTuning _tuning;
        private FixedSimulationClock _clock;
        private MovementInputFrame _bufferedInput;
        private MovementInputFrame _authorityInput;
        private bool _authorityCommandQueued;
        private Vector3 _lastAuthoritativePosition;
        private bool _hasAuthoritativePosition;
        private int _simulationTick;
        private bool _initialized;

        public int ActorId => actorId;
        public Float2 AimDirection => _motor != null ? _motor.AimDirection : Float2.Up;
        public Float2 Velocity => _motor != null ? _motor.Velocity : Float2.Zero;
        public MovementTuning Tuning => _initialized ? _tuning : MovementTuning.Default;
        public bool IsInitialized => _initialized;
        public Vector3 LastAuthoritativePosition => _lastAuthoritativePosition;
        public bool ExternalCommandMode { get => externalCommandMode; set => externalCommandMode = value; }
        public bool AuthorityDrivenMovement
        {
            get => authorityDrivenMovement;
            set
            {
                authorityDrivenMovement = value;
                if (_characterController != null)
                {
                    _characterController.enabled = !value;
                }
            }
        }
        public bool HasPendingAuthorityCommand => _authorityCommandQueued;

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
            _authorityInput = new MovementInputFrame(Float2.Zero, Float2.Zero);
            _authorityCommandQueued = false;
            _lastAuthoritativePosition = transform.position;
            _hasAuthoritativePosition = false;
            _simulationTick = 0;
            _initialized = true;
            if (authorityDrivenMovement)
            {
                _characterController.enabled = false;
            }
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }

            if (authorityDrivenMovement)
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

        private void LateUpdate()
        {
            if (!authorityDrivenMovement || !_hasAuthoritativePosition)
            {
                return;
            }

            var position = transform.position;
            if (Mathf.Abs(position.x - _lastAuthoritativePosition.x) > 0.000001f
                || Mathf.Abs(position.z - _lastAuthoritativePosition.z) > 0.000001f)
            {
                transform.position = _lastAuthoritativePosition;
                Physics.SyncTransforms();
            }
        }

        public void Submit(MovementCommand command)
        {
            Submit(command, Time.deltaTime);
        }

        public void Submit(MovementCommand command, float deltaSeconds)
        {
            if (authorityDrivenMovement)
            {
                QueueAuthorityCommand(command);
                return;
            }

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
            _authorityCommandQueued = false;
        }

        public MovementCommand CaptureAuthorityCommand(int simulationTick)
        {
            var input = inputAdapter != null && inputAdapter.isActiveAndEnabled
                ? inputAdapter.ReadInput()
                : new MovementInputFrame(Float2.Zero, Float2.Zero);
            return MovementCommandFactory.Create(ActorId, simulationTick, input, _tuning);
        }

        public void QueueAuthorityCommand(MovementCommand command)
        {
            if (!_initialized || command.ActorId != ActorId ||
                (_fighterMovementLock != null && _fighterMovementLock.IsMovementLocked))
            {
                return;
            }

            _authorityInput = new MovementInputFrame(command.Movement, command.Aim);
            _authorityCommandQueued = true;
        }

        public MovementCommand GetAuthorityCommand(int simulationTick)
        {
            if (_authorityCommandQueued)
            {
                _authorityCommandQueued = false;
                return new MovementCommand(ActorId, simulationTick, _authorityInput.Movement, _authorityInput.Aim);
            }

            return CaptureAuthorityCommand(simulationTick);
        }

        public void ApplyAuthoritativeMovement(MatchAuthorityMovement movement, float fixedDeltaSeconds)
        {
            if (!_initialized || !movement.Applied || movement.ActorId.Value != ActorId)
            {
                return;
            }

            _motor.ApplyAuthoritativeState(movement.Step.Velocity, movement.Step.AimDirection);
            var position = transform.position;
            // The domain authority is canonical. CharacterController.Move performs a local
            // collision projection and can therefore reject a valid authoritative position.
            // Apply the canonical position directly; collision validation belongs in the
            // authority simulation and must not mutate the presentation result.
            _lastAuthoritativePosition = new Vector3(movement.Position.X, position.y, movement.Position.Y);
            _hasAuthoritativePosition = true;
            transform.position = _lastAuthoritativePosition;
            Physics.SyncTransforms();

            if (movement.Step.AimDirection.SqrMagnitude > 0.000001f)
            {
                var targetRotation = Quaternion.LookRotation(
                    new Vector3(movement.Step.AimDirection.X, 0f, movement.Step.AimDirection.Y),
                    Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    _tuning.RotationSpeed * Mathf.Max(0f, fixedDeltaSeconds));
            }

            aimIndicator?.SetAimDirection(movement.Step.AimDirection);
        }

        public void ApplyAuthoritativePosition(Float2 position)
        {
            var current = transform.position;
            _lastAuthoritativePosition = new Vector3(position.X, current.y, position.Y);
            _hasAuthoritativePosition = true;
            transform.position = _lastAuthoritativePosition;
            Physics.SyncTransforms();
        }
    }
}
