using BattleRaja.Core.Domain;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Visuals;
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
        private readonly RaycastHit[] _chargeHits = new RaycastHit[32];
        private OfflineMatchController _match;
        private bool _subscribedToCanonicalTick;

        public ContentId AbilityId => _special.AbilityId;
        public FighterDefinition Definition => _definition;
        public ChargeThrowState ActionState => UsesAuthorityCharge
            ? _match.GetPehelChargeState(OwnerId).State
            : _runtime != null ? _runtime.State : ChargeThrowState.Ready;
        public CombatEntityId CapturedTargetId => UsesAuthorityCharge
            ? _match.GetPehelChargeState(OwnerId).CapturedTargetId
            : _runtime != null ? _runtime.CapturedTargetId : default;
        public float AbilityCooldownRemaining => UsesAuthorityCharge
            ? _match.GetPehelChargeState(OwnerId).CooldownRemaining
            : _runtime != null ? _runtime.CooldownRemaining : 0f;
        public bool IsMovementLocked => UsesAuthorityCharge
            ? ActionState != ChargeThrowState.Ready && ActionState != ChargeThrowState.Cooldown
            : _runtime != null && _runtime.IsMovementLocked;

        private CombatEntityId OwnerId => new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1);
        private bool UsesAuthorityCharge => movementAgent != null && movementAgent.AuthorityDrivenMovement &&
            _match != null && _match.Simulation != null;

        private void Awake()
        {
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            characterController = characterController != null ? characterController : GetComponent<CharacterController>();
            damageResolver = damageResolver != null ? damageResolver : FindAnyObjectByType<CombatDamageResolver>();
            _definition = fighterDefinition != null ? fighterDefinition.ToDomain() : FighterDefinition.Pehel;
            _match = FindAnyObjectByType<OfflineMatchController>();
            _special = FighterSpecialDefinition.PehelChargeThrow;
            _runtime = new ChargeThrowRuntime(_special);
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
            _self = GetComponent<CombatTarget>();
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

        private void SubscribeToCanonicalTick()
        {
            if (_subscribedToCanonicalTick || !isActiveAndEnabled || _match == null) return;
            _match.SimulationTickAdvanced += OnCanonicalSimulationTick;
            _subscribedToCanonicalTick = true;
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
            if (UsesAuthorityCharge && _match.IsMatchStarted)
            {
                return;
            }

            var steps = _clock.Consume(Time.deltaTime);
            for (var i = 0; i < steps; i++)
            {
                var simulationTick = _clock.GetConsumedTick(i);
                if (_abilityQueued)
                {
                    Submit(AbilityCommandFactory.Create(
                        _self != null ? _self.Id : new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1),
                        simulationTick,
                        AbilityId,
                        _queuedDirection,
                        true));
                    _abilityQueued = false;
                }

                if (UsesAuthorityCharge)
                {
                    var authorityStep = _match.AdvancePehelCharge(
                        OwnerId,
                        simulationTick,
                        (float)_clock.StepSeconds,
                        _special.Magnitude);
                    if (authorityStep.ActorDisplacement.Applied)
                    {
                        movementAgent.ApplyAuthoritativePosition(authorityStep.ActorDisplacement.Position);
                    }

                    ApplyAuthorityResult(authorityStep);
                    continue;
                }

                var step = _runtime.Step((float)_clock.StepSeconds, ComputeAvailableDistance(_runtime.Direction));
                if (step.Displacement.SqrMagnitude > 0.000001f && characterController != null)
                {
                    var appliedByAuthority = movementAgent != null && movementAgent.AuthorityDrivenMovement &&
                        _match != null && ApplyAuthorityDisplacement(step.Displacement, simulationTick);
                    if (!appliedByAuthority)
                    {
                        characterController.Move(new Vector3(step.Displacement.X, 0f, step.Displacement.Y));
                    }
                }

                if (_runtime.State == ChargeThrowState.Active) TryCaptureTarget();
                if (step.ThrowTriggered) ResolveThrow(step.CapturedTargetId, simulationTick);
            }
        }

        public void Submit(AbilityCommand command)
        {
            if (_runtime == null) return;
            var movement = inputAdapter != null ? inputAdapter.ReadInput().Movement : Float2.Zero;
            var facing = movementAgent != null ? movementAgent.AimDirection : Float2.Up;
            if (UsesAuthorityCharge)
            {
                if (_match.TryStartPehelCharge(command, movement, facing).Accepted)
                {
                    GetComponent<FighterPresentation>()?.NotifyAbility();
                }

                return;
            }

            if (_runtime.TryStart(command, movement, facing))
            {
                GetComponent<FighterPresentation>()?.NotifyAbility();
            }
        }

        public void ResetFighterState()
        {
            _runtime?.Reset();
            _abilityHeld = false;
            _abilityQueued = false;
        }

        private void ApplyAuthorityResult(MatchAuthorityChargeThrow authorityStep)
        {
            if (_match == null) return;

            if (authorityStep.HasDamage && _match.TryGetActorView(
                authorityStep.Damage.Request.TargetId,
                out _,
                out _,
                out var targetHealth))
            {
                targetHealth?.ApplyAuthoritativeDamage(
                    authorityStep.Damage.Request,
                    authorityStep.Damage.Result,
                    authorityStep.Damage.CurrentHealthAfter,
                    authorityStep.SimulationTick);
            }

            if (!authorityStep.HasTargetDisplacement) return;
            if (!_match.TryGetActorView(
                authorityStep.TargetDisplacement.ActorId,
                out _,
                out var targetAgent,
                out _))
            {
                return;
            }

            if (targetAgent != null && targetAgent.AuthorityDrivenMovement)
            {
                targetAgent.ApplyAuthoritativePosition(authorityStep.TargetDisplacement.Position);
                return;
            }

            var targetTransform = targetAgent != null ? targetAgent.transform : null;
            if (targetTransform == null) return;
            var position = targetTransform.position;
            targetTransform.position = new Vector3(
                authorityStep.TargetDisplacement.Position.X,
                position.y,
                authorityStep.TargetDisplacement.Position.Y);
            Physics.SyncTransforms();
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

        private void ResolveThrow(CombatEntityId targetId, int simulationTick)
        {
            if (targetId.Value <= 0) return;
            var targets = FindObjectsByType<CombatTarget>();
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
                    _runtime.Direction,
                    simulationTick);
                damageResolver?.Resolve(target, request, false, false, simulationTick);
                var throwDisplacement = _runtime.Direction * (_special.Magnitude * 0.25f);
                var targetAgent = target.GetComponent<MovementPlayerAgent>();
                var appliedByAuthority = targetAgent != null && targetAgent.AuthorityDrivenMovement &&
                    _match != null && ApplyAuthorityDisplacement(targetAgent, throwDisplacement, simulationTick);
                if (!appliedByAuthority)
                {
                    target.GetComponent<CharacterController>()?.Move(new Vector3(throwDisplacement.X, 0f, throwDisplacement.Y));
                }
                break;
            }
        }

        private bool ApplyAuthorityDisplacement(Float2 displacement, int simulationTick)
        {
            return ApplyAuthorityDisplacement(movementAgent, displacement, simulationTick);
        }

        private bool ApplyAuthorityDisplacement(MovementPlayerAgent agent, Float2 displacement, int simulationTick)
        {
            if (agent == null) return false;
            var result = _match.ResolveAbilityDisplacement(new CombatEntityId(agent.ActorId), simulationTick, displacement);
            if (!result.Applied) return false;
            agent.ApplyAuthoritativePosition(result.Position);
            return true;
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
            var hitCount = Physics.SphereCastNonAlloc(
                origin,
                _special.Radius * 0.25f,
                new Vector3(normalized.X, 0f, normalized.Y),
                _chargeHits,
                available,
                chargeCollisionMask,
                QueryTriggerInteraction.Ignore);
            var nearestObstacle = available;
            for (var i = 0; i < hitCount; i++)
            {
                var hit = _chargeHits[i];
                var target = hit.collider != null ? hit.collider.GetComponentInParent<CombatTarget>() : null;
                if (target != null) continue;
                nearestObstacle = Mathf.Min(nearestObstacle, Mathf.Max(0f, hit.distance - _special.Radius * 0.25f));
            }

            available = nearestObstacle;

            return Mathf.Max(0f, available);
        }

        private void OnCanonicalSimulationTick(int simulationTick, float fixedDeltaSeconds)
        {
            if (!isActiveAndEnabled || !UsesAuthorityCharge || !_match.IsMatchStarted) return;

            if (_abilityQueued)
            {
                Submit(AbilityCommandFactory.Create(
                    _self != null ? _self.Id : new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1),
                    simulationTick,
                    AbilityId,
                    _queuedDirection,
                    true));
                _abilityQueued = false;
            }

            var authorityStep = _match.AdvancePehelCharge(
                OwnerId,
                simulationTick,
                fixedDeltaSeconds,
                _special.Magnitude);
            if (authorityStep.ActorDisplacement.Applied)
            {
                movementAgent.ApplyAuthoritativePosition(authorityStep.ActorDisplacement.Position);
            }

            ApplyAuthorityResult(authorityStep);
        }

    }
}
