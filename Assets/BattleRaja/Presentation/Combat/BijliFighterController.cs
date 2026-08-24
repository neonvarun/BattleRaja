using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Visuals;
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
        private OfflineMatchController _match;

        private FighterDefinition _definition;
        private FighterRuntimeState _runtime;
        private FixedSimulationClock _clock;
        private int _simulationTick;
        private bool _abilityHeld;
        private bool _abilityQueued;
        private Float2 _queuedDirection = Float2.Up;
        private bool _subscribedToCanonicalTick;

        public FighterDefinition Definition => _definition;
        public ContentId AbilityId => _definition.Ability.AbilityId;
        public FighterActionState ActionState => UsesAuthorityDash
            ? _match.GetBijliDashState(OwnerId).State
            : _runtime != null ? _runtime.ActionState : FighterActionState.Ready;
        public float DashCooldownRemaining => UsesAuthorityDash
            ? _match.GetBijliDashState(OwnerId).CooldownRemaining
            : _runtime != null ? _runtime.CooldownRemaining : 0f;
        public bool IsMovementLocked => ActionState != FighterActionState.Ready &&
            ActionState != FighterActionState.Cooldown;
        public bool IsInitialized => _runtime != null;

        private CombatEntityId OwnerId => new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1);
        private bool UsesAuthorityDash => movementAgent != null && movementAgent.AuthorityDrivenMovement &&
            _match != null && _match.Simulation != null;

        private void Awake()
        {
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            characterController = characterController != null ? characterController : GetComponent<CharacterController>();
            _definition = fighterDefinition != null ? fighterDefinition.ToDomain() : FighterDefinition.Bijli;
            _match = FindAnyObjectByType<OfflineMatchController>();
            _runtime = new FighterRuntimeState(_definition);
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
            if (dashTrail != null)
            {
                dashTrail.emitting = false;
            }
        }

        private void Start()
        {
            SubscribeToCanonicalTick();
        }

        private void OnDestroy()
        {
            if (_subscribedToCanonicalTick && _match != null)
            {
                _match.SimulationTickAdvanced -= OnCanonicalSimulationTick;
                _subscribedToCanonicalTick = false;
            }
        }

        private bool UsesAuthority => movementAgent != null && movementAgent.AuthorityDrivenMovement &&
            _match != null && _match.Simulation != null;

        private void SubscribeToCanonicalTick()
        {
            if (_subscribedToCanonicalTick || !isActiveAndEnabled || _match == null) return;
            _match.SimulationTickAdvanced += OnCanonicalSimulationTick;
            _subscribedToCanonicalTick = true;
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
            if (UsesAuthority && _match.IsMatchStarted)
            {
                return;
            }

            var steps = _clock.Consume(Time.deltaTime);
            for (var i = 0; i < steps; i++)
            {
                _simulationTick = _clock.GetConsumedTick(i);
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

                if (UsesAuthorityDash) continue;
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

        private void OnCanonicalSimulationTick(int simulationTick, float fixedDeltaSeconds)
        {
            if (!isActiveAndEnabled || !UsesAuthority || !_match.IsMatchStarted) return;

            if (_abilityQueued)
            {
                Submit(AbilityCommandFactory.Create(
                    OwnerId,
                    simulationTick,
                    _definition.Ability.AbilityId,
                    _queuedDirection,
                    true));
                _abilityQueued = false;
            }

            if (!UsesAuthorityDash) return;
            if (dashTrail != null)
            {
                dashTrail.emitting = _match.GetBijliDashState(OwnerId).State == FighterActionState.Active;
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
            if (UsesAuthorityDash)
            {
                if (_match.TryStartBijliDash(command, movement, facing).Accepted)
                {
                    GetComponent<FighterPresentation>()?.NotifyAbility();
                }

                return;
            }

            if (_runtime.TryStartDash(command, movement, facing))
            {
                GetComponent<FighterPresentation>()?.NotifyAbility();
            }
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
