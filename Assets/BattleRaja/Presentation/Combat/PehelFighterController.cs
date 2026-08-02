using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PehelFighterController : MonoBehaviour, IFighterAbilityController, IFighterMovementLock
    {
        [SerializeField] private FighterDefinitionAsset fighterDefinition;
        [SerializeField] private PlayerInputAdapter inputAdapter;
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CombatDamageResolver damageResolver;
        [SerializeField] private CombatFaction faction = CombatFaction.Enemy;
        [SerializeField] private int simulationTickRate = 30;
        [SerializeField] private LayerMask chargeCollisionMask = ~0;
        [SerializeField] private float playMinX = -13.2f;
        [SerializeField] private float playMaxX = 13.2f;
        [SerializeField] private float playMinZ = -9.2f;
        [SerializeField] private float playMaxZ = 9.2f;

        private FighterDefinition _definition;
        private FighterSpecialDefinition _special;
        private ChargeThrowRuntime _runtime;
        private FixedSimulationClock _clock;
        private CombatTarget _self;
        private bool _abilityHeld;
        private bool _abilityQueued;
        private Float2 _queuedDirection = Float2.Up;

        public ContentId AbilityId => _special.AbilityId;
        public FighterDefinition Definition => _definition;
        public ChargeThrowState ActionState => _runtime != null ? _runtime.State : ChargeThrowState.Ready;
        public float AbilityCooldownRemaining => _runtime != null ? _runtime.CooldownRemaining : 0f;
        public bool IsMovementLocked => _runtime != null && _runtime.IsMovementLocked;

        private void Awake()
        {
            fighterDefinition = fighterDefinition != null ? fighterDefinition : GetComponent<FighterDefinitionAsset>();
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            characterController = characterController != null ? characterController : GetComponent<CharacterController>();
            damageResolver = damageResolver != null ? damageResolver : FindFirstObjectByType<CombatDamageResolver>();
            _definition = fighterDefinition != null ? fighterDefinition.ToDomain() : FighterDefinition.Pehel;
            _special = FighterSpecialDefinition.PehelChargeThrow;
            _runtime = new ChargeThrowRuntime(_special);
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
            _self = GetComponent<CombatTarget>();
        }

        private void Update()
        {
            var pressed = inputAdapter != null && inputAdapter.IsAbilityPressed;
            if (pressed && !_abilityHeld)
            {
                _queuedDirection = inputAdapter.ReadInput().Aim;
                _abilityQueued = true;
            }

            _abilityHeld = pressed;
            var steps = _clock.Consume(Time.deltaTime);
            for (var i = 0; i < steps; i++)
            {
                if (_abilityQueued)
                {
                    Submit(AbilityCommandFactory.Create(
                        _self != null ? _self.Id : new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1),
                        _clock.Tick,
                        AbilityId,
                        _queuedDirection,
                        true));
                    _abilityQueued = false;
                }

                var step = _runtime.Step((float)_clock.StepSeconds, ComputeAvailableDistance(_runtime.Direction));
                if (step.Displacement.SqrMagnitude > 0.000001f && characterController != null)
                {
                    characterController.Move(new Vector3(step.Displacement.X, 0f, step.Displacement.Y));
                }

                if (_runtime.State == ChargeThrowState.Active) TryCaptureTarget();
                if (step.ThrowTriggered) ResolveThrow(step.CapturedTargetId);
            }
        }

        public void Submit(AbilityCommand command)
        {
            if (_runtime == null) return;
            var movement = inputAdapter != null ? inputAdapter.ReadInput().Movement : Float2.Zero;
            var facing = movementAgent != null ? movementAgent.AimDirection : Float2.Up;
            _runtime.TryStart(command, movement, facing);
        }

        public void ResetFighterState()
        {
            _runtime?.Reset();
            _abilityHeld = false;
            _abilityQueued = false;
        }

        private void TryCaptureTarget()
        {
            var origin = transform.position;
            var hits = Physics.OverlapSphere(origin, _special.Radius, chargeCollisionMask, QueryTriggerInteraction.Ignore);
            for (var i = 0; i < hits.Length; i++)
            {
                var target = hits[i].GetComponentInParent<CombatTarget>();
                if (target == null || (_self != null && target.Id == _self.Id)) continue;
                var distance = Vector3.Distance(origin, target.transform.position);
                if (_runtime.TryCaptureTarget(target.Id, faction, target.Faction, distance)) break;
            }
        }

        private void ResolveThrow(CombatEntityId targetId)
        {
            if (targetId.Value <= 0) return;
            var targets = FindObjectsByType<CombatTarget>(FindObjectsSortMode.None);
            for (var i = 0; i < targets.Length; i++)
            {
                var target = targets[i];
                if (target == null || target.Id != targetId) continue;
                var request = new DamageRequest(
                    _self != null ? _self.Id : new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1),
                    target.Id,
                    faction,
                    _special.Magnitude,
                    DamageType.Ability,
                    _runtime.Direction);
                damageResolver?.Resolve(target, request, false, false, _clock.Tick);
                var targetController = target.GetComponent<CharacterController>();
                targetController?.Move(new Vector3(_runtime.Direction.X, 0f, _runtime.Direction.Y) * (_special.Magnitude * 0.25f));
                break;
            }
        }

        private float ComputeAvailableDistance(Float2 direction)
        {
            if (!IsMovementLocked || direction.SqrMagnitude <= 0.000001f) return _special.Magnitude;
            var normalized = direction.Normalized;
            var position = transform.position;
            var available = (float)_special.Magnitude;
            if (normalized.X > 0f) available = Mathf.Min(available, (playMaxX - position.x) / normalized.X);
            if (normalized.X < 0f) available = Mathf.Min(available, (playMinX - position.x) / normalized.X);
            if (normalized.Y > 0f) available = Mathf.Min(available, (playMaxZ - position.z) / normalized.Y);
            if (normalized.Y < 0f) available = Mathf.Min(available, (playMinZ - position.z) / normalized.Y);
            var origin = position + Vector3.up * Mathf.Max(0.1f, characterController != null ? characterController.height * 0.5f : 0.5f);
            if (Physics.SphereCast(origin, _special.Radius * 0.25f, new Vector3(normalized.X, 0f, normalized.Y), out var hit, available, chargeCollisionMask, QueryTriggerInteraction.Ignore))
            {
                available = Mathf.Min(available, Mathf.Max(0f, hit.distance - _special.Radius * 0.25f));
            }

            return Mathf.Max(0f, available);
        }
    }
}
