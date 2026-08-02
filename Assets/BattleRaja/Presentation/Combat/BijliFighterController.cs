using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(CharacterController))]
    public sealed class BijliFighterController : MonoBehaviour, IFighterAbilityController, IFighterMovementLock
    {
        [SerializeField] private FighterDefinitionAsset fighterDefinition;
        [SerializeField] private PlayerInputAdapter inputAdapter;
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private TrailRenderer dashTrail;
        [SerializeField] private float playMinX = -13.2f;
        [SerializeField] private float playMaxX = 13.2f;
        [SerializeField] private float playMinZ = -9.2f;
        [SerializeField] private float playMaxZ = 9.2f;
        [SerializeField] private LayerMask dashCollisionMask = ~0;
        [SerializeField] private int simulationTickRate = 30;

        private FighterDefinition _definition;
        private FighterRuntimeState _runtime;
        private FixedSimulationClock _clock;
        private int _simulationTick;
        private bool _abilityHeld;
        private bool _abilityQueued;
        private Float2 _queuedDirection = Float2.Up;

        public FighterDefinition Definition => _definition;
        public ContentId AbilityId => _definition.Ability.AbilityId;
        public FighterActionState ActionState => _runtime != null ? _runtime.ActionState : FighterActionState.Ready;
        public float DashCooldownRemaining => _runtime != null ? _runtime.CooldownRemaining : 0f;
        public bool IsMovementLocked => _runtime != null && ActionState != FighterActionState.Ready && ActionState != FighterActionState.Cooldown;
        public bool IsInitialized => _runtime != null;

        private void Awake()
        {
            fighterDefinition = fighterDefinition != null ? fighterDefinition : FindFirstObjectByType<FighterDefinitionAsset>();
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            characterController = characterController != null ? characterController : GetComponent<CharacterController>();
            _definition = fighterDefinition != null ? fighterDefinition.ToDomain() : FighterDefinition.Bijli;
            _runtime = new FighterRuntimeState(_definition);
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
            if (dashTrail != null)
            {
                dashTrail.emitting = false;
            }
        }

        private void Update()
        {
            if (_runtime == null)
            {
                return;
            }

            var abilityPressed = inputAdapter != null && inputAdapter.IsAbilityPressed;
            if (abilityPressed && !_abilityHeld)
            {
                var input = inputAdapter.ReadInput();
                _queuedDirection = input.Aim;
                _abilityQueued = true;
            }

            _abilityHeld = abilityPressed;
            var steps = _clock.Consume(Time.deltaTime);
            for (var i = 0; i < steps; i++)
            {
                _simulationTick = _clock.Tick;
                if (_abilityQueued)
                {
                    var command = AbilityCommandFactory.Create(
                        new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1),
                        _simulationTick,
                        _definition.Ability.AbilityId,
                        _queuedDirection,
                        true);
                    Submit(command);
                    _abilityQueued = false;
                }

                var availableDistance = ComputeAvailableDistance(_runtime.DashDirection);
                var step = _runtime.Step((float)_clock.StepSeconds, availableDistance);
                if (step.Displacement.SqrMagnitude > 0.000001f && characterController != null)
                {
                    characterController.Move(new Vector3(step.Displacement.X, 0f, step.Displacement.Y));
                }

                if (dashTrail != null)
                {
                    dashTrail.emitting = ActionState == FighterActionState.Active;
                }
            }
        }

        public void Submit(AbilityCommand command)
        {
            if (_runtime == null)
            {
                return;
            }

            var movement = inputAdapter != null ? inputAdapter.ReadInput().Movement : Float2.Zero;
            var facing = movementAgent != null ? movementAgent.AimDirection : Float2.Up;
            _runtime.TryStartDash(command, movement, facing);
        }

        public void ResetFighterState()
        {
            _runtime?.Reset();
            _abilityHeld = false;
            _abilityQueued = false;
            if (dashTrail != null)
            {
                dashTrail.emitting = false;
            }
        }

        private float ComputeAvailableDistance(Float2 direction)
        {
            if (!IsMovementLocked || direction.SqrMagnitude <= 0.000001f)
            {
                return _definition.Ability.Distance;
            }

            var normalized = direction.Normalized;
            var position = transform.position;
            var available = _definition.Ability.Distance;
            if (normalized.X > 0f) available = Mathf.Min(available, (playMaxX - position.x) / normalized.X);
            if (normalized.X < 0f) available = Mathf.Min(available, (playMinX - position.x) / normalized.X);
            if (normalized.Y > 0f) available = Mathf.Min(available, (playMaxZ - position.z) / normalized.Y);
            if (normalized.Y < 0f) available = Mathf.Min(available, (playMinZ - position.z) / normalized.Y);

            var origin = position + Vector3.up * Mathf.Max(0.1f, characterController != null ? characterController.height * 0.5f : 0.5f);
            if (Physics.SphereCast(origin, _definition.Ability.CollisionRadius, new Vector3(normalized.X, 0f, normalized.Y), out var hit, available, dashCollisionMask, QueryTriggerInteraction.Ignore))
            {
                available = Mathf.Min(available, Mathf.Max(0f, hit.distance - _definition.Ability.CollisionRadius));
            }

            return Mathf.Max(0f, available);
        }
    }
}
