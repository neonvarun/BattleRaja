using System.Collections.Generic;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Visuals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleRaja.Presentation.Gadgets
{
    public sealed class GadgetUser : MonoBehaviour
    {
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private CombatTarget combatTarget;
        [SerializeField] private CombatHealth health;
        [SerializeField] private CombatDamageResolver damageResolver;
        [SerializeField] private OfflineMatchController match;
        [SerializeField] private bool botControlled;
        [SerializeField] private int simulationTickRate = 30;

        private readonly GadgetInventory _inventory = new GadgetInventory(1);
        private readonly GadgetRuntime _runtime = new GadgetRuntime();
        private FixedSimulationClock _clock;
        private int _tick;
        private int _activeSimulationTick = -1;
        private float _shieldRemaining;
        private Float2 _shieldDirection = Float2.Up;
        private float _feedbackRemaining;
        private string _feedback = string.Empty;
        private bool _useQueued;
        private bool _subscribedToCanonicalTick;
        private BattleRajaAudioDirector _audio;

        public bool HasGadget => _inventory.HasGadget;
        public ContentId HeldGadget => _inventory.HeldGadget;
        public float CooldownRemaining => _runtime.CooldownRemaining;
        public float ShieldRemaining => Mathf.Max(0f, _shieldRemaining);
        public string Feedback => _feedback;
        public GadgetInventory Inventory => _inventory;
        public int SuccessfulPickupCount { get; private set; }
        public int SuccessfulUseCount { get; private set; }
        public int SuccessfulUmbrellaGuardUses { get; private set; }
        public int SuccessfulDholBurstUses { get; private set; }
        public int SuccessfulTiffinStationUses { get; private set; }
        public int ContextualUseAttemptCount { get; private set; }
        public int FailedUseCount { get; private set; }
        public bool AutonomousBotControlled
        {
            set => botControlled = value;
        }

        public void ResetTelemetry()
        {
            SuccessfulPickupCount = 0;
            SuccessfulUseCount = 0;
            SuccessfulUmbrellaGuardUses = 0;
            SuccessfulDholBurstUses = 0;
            SuccessfulTiffinStationUses = 0;
            ContextualUseAttemptCount = 0;
            FailedUseCount = 0;
        }

        private void Awake()
        {
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            combatTarget = combatTarget != null ? combatTarget : GetComponent<CombatTarget>();
            health = health != null ? health : GetComponent<CombatHealth>();
            damageResolver = damageResolver != null ? damageResolver : FindAnyObjectByType<CombatDamageResolver>();
            match = match != null ? match : FindAnyObjectByType<OfflineMatchController>();
            _audio = FindAnyObjectByType<BattleRajaAudioDirector>();
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
        }

        private void Start()
        {
            SubscribeToCanonicalTick();
        }

        private void OnDestroy()
        {
            if (_subscribedToCanonicalTick && match != null)
            {
                match.SimulationTickAdvanced -= OnCanonicalSimulationTick;
                _subscribedToCanonicalTick = false;
            }
        }

        private bool UsesAuthority => match != null && match.AuthorityDrivenMovement && match.Simulation != null;

        private void SubscribeToCanonicalTick()
        {
            if (_subscribedToCanonicalTick || !isActiveAndEnabled || match == null) return;
            match.SimulationTickAdvanced += OnCanonicalSimulationTick;
            _subscribedToCanonicalTick = true;
        }

        private void Update()
        {
            if (!botControlled && Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
            {
                _useQueued = true;
            }

            if (UsesAuthority && match.IsMatchStarted)
            {
                return;
            }

            var steps = _clock.Consume(Time.deltaTime);
            _activeSimulationTick = -1;
            for (var i = 0; i < steps; i++)
            {
                _activeSimulationTick = _clock.GetConsumedTick(i);
                if (_useQueued)
                {
                    UseHeld();
                    _useQueued = false;
                }

                AdvancePresentation((float)_clock.StepSeconds);
            }
            _activeSimulationTick = -1;
        }

        private void OnCanonicalSimulationTick(int simulationTick, float fixedDeltaSeconds)
        {
            if (!isActiveAndEnabled || !UsesAuthority || !match.IsMatchStarted) return;

            _activeSimulationTick = simulationTick;
            if (_useQueued)
            {
                UseHeld();
                _useQueued = false;
            }

            AdvancePresentation(fixedDeltaSeconds);
            _activeSimulationTick = -1;
        }

        private void AdvancePresentation(float deltaSeconds)
        {
            _runtime.Advance(deltaSeconds);
            _shieldRemaining = Mathf.Max(0f, _shieldRemaining - deltaSeconds);
            _feedbackRemaining = Mathf.Max(0f, _feedbackRemaining - deltaSeconds);
            if (_feedbackRemaining <= 0f) _feedback = string.Empty;
        }

        public bool TryPickup(ContentId id)
        {
            var accepted = _inventory.TryPickup(id);
            if (accepted && match != null && match.Simulation != null && combatTarget != null &&
                !match.TryAcquireGadget(combatTarget.Id, id))
            {
                _inventory.TryConsume(id);
                accepted = false;
            }

            SetFeedback(accepted ? $"Picked {id.Value}" : "Gadget slot full");
            if (accepted)
            {
                SuccessfulPickupCount++;
                if (combatTarget != null && combatTarget.Id.Value == 1) _audio?.PlayPickup();
            }
            return accepted;
        }

        public bool TryPickupFromAuthority(ContentId id)
        {
            var accepted = _inventory.TryPickup(id);
            SetFeedback(accepted ? $"Picked {id.Value}" : "Gadget slot full");
            if (accepted)
            {
                SuccessfulPickupCount++;
                if (combatTarget != null && combatTarget.Id.Value == 1) _audio?.PlayPickup();
            }
            return accepted;
        }

        public bool UseHeld()
        {
            if (!_inventory.HasGadget || movementAgent == null || combatTarget == null)
            {
                FailedUseCount++;
                SetFeedback("No gadget held");
                return false;
            }

            var direction = movementAgent.AimDirection;
            var command = new GadgetUseCommand(
                combatTarget.Id,
                _inventory.HeldGadget,
                new Float2(transform.position.x, transform.position.z),
                direction,
                NextTick());
            var result = TryUse(command, out var authoritative);
            if (!result.Used)
            {
                FailedUseCount++;
                SetFeedback(result.Failure.ToString());
                return false;
            }

            if (authoritative)
            {
                if (!_inventory.TryConsume(command.GadgetId))
                {
                    SetFeedback("Authority inventory mismatch");
                    return false;
                }

                _runtime.ApplyAuthoritativeUse(result.Effect.Definition);
            }

            ApplyEffect(result.Effect);
            SuccessfulUseCount++;
            RecordSuccessfulUse(command.GadgetId);
            if (combatTarget.Id.Value == 1) _audio?.PlayGadget(command.GadgetId.Value);
            return true;
        }

        public bool UseForContext(BotPerceptionSnapshot snapshot, int simulationTick = -1)
        {
            var visibleHostile = false;
            var nearestThreatDistance = float.MaxValue;
            var threatDirection = Float2.Zero;
            for (var i = 0; i < snapshot.TargetCount; i++)
            {
                var candidate = snapshot.Targets[i];
                if (candidate.HasLineOfSight && candidate.IsHostile)
                {
                    visibleHostile = true;
                    var delta = candidate.Position - snapshot.Position;
                    var distance = delta.Magnitude;
                    if (distance < nearestThreatDistance)
                    {
                        nearestThreatDistance = distance;
                        threatDirection = delta.Normalized;
                    }
                }
            }

            if (!botControlled || !_inventory.HasGadget || !visibleHostile)
            {
                return false;
            }

            var definition = GadgetCatalog.TryGet(_inventory.HeldGadget, out var found) ? found : default(GadgetDefinition);
            if (definition.Kind == GadgetKind.UmbrellaGuard && health != null && health.Snapshot.CurrentHealth > health.MaxHealth * 0.65f)
            {
                return false;
            }

            if (definition.Kind == GadgetKind.TiffinStation && health != null &&
                health.Snapshot.CurrentHealth >= health.MaxHealth * 0.85f)
            {
                return false;
            }

            if (definition.Kind == GadgetKind.DholBurst && nearestThreatDistance > definition.Radius * 1.4f)
            {
                return false;
            }

            var direction = threatDirection.SqrMagnitude > 0.000001f
                ? threatDirection
                : movementAgent != null ? movementAgent.AimDirection : Float2.Up;
            ContextualUseAttemptCount++;
            return UseHeldWithDirection(direction, simulationTick);
        }

        private bool UseHeldWithDirection(Float2 direction, int simulationTick = -1)
        {
            var command = new GadgetUseCommand(
                combatTarget.Id,
                _inventory.HeldGadget,
                new Float2(transform.position.x, transform.position.z),
                direction,
                NextTick(simulationTick));
            var result = TryUse(command, out var authoritative);
            if (!result.Used)
            {
                FailedUseCount++;
                return false;
            }

            if (authoritative)
            {
                if (!_inventory.TryConsume(command.GadgetId))
                {
                    FailedUseCount++;
                    return false;
                }

                _runtime.ApplyAuthoritativeUse(result.Effect.Definition);
            }

            ApplyEffect(result.Effect);
            SuccessfulUseCount++;
            RecordSuccessfulUse(command.GadgetId);
            if (combatTarget.Id.Value == 1) _audio?.PlayGadget(command.GadgetId.Value);
            return true;
        }

        private void RecordSuccessfulUse(ContentId gadgetId)
        {
            if (!GadgetCatalog.TryGet(gadgetId, out var definition)) return;
            switch (definition.Kind)
            {
                case GadgetKind.UmbrellaGuard:
                    SuccessfulUmbrellaGuardUses++;
                    break;
                case GadgetKind.DholBurst:
                    SuccessfulDholBurstUses++;
                    break;
                case GadgetKind.TiffinStation:
                    SuccessfulTiffinStationUses++;
                    break;
            }
        }

        private GadgetUseResult TryUse(GadgetUseCommand command, out bool authoritative)
        {
            authoritative = match != null && match.Simulation != null;
            return authoritative
                ? match.TryUseGadget(command)
                : _runtime.TryUse(_inventory, command);
        }

        public int ModifyIncomingDamage(DamageRequest request)
        {
            if (_shieldRemaining <= 0f || request.DamageType == DamageType.Aandhi || request.DamageType == DamageType.Generic)
            {
                return request.RawAmount;
            }

            var facing = _shieldDirection.Normalized;
            var incoming = request.HitDirection.SqrMagnitude > 0.000001f ? request.HitDirection.Normalized * -1f : facing;
            var dot = facing.X * incoming.X + facing.Y * incoming.Y;
            return dot >= 0.15f
                ? Mathf.Max(1, Mathf.CeilToInt((request.RawAmount * 0.30f) - 0.0001f))
                : request.RawAmount;
        }

        private void ApplyEffect(GadgetEffect effect)
        {
            switch (effect.Kind)
            {
                case GadgetEffectKind.UmbrellaGuard:
                    _shieldDirection = effect.Command.Direction.Normalized;
                    _shieldRemaining = effect.Definition.DurationSeconds;
                    SetFeedback("Umbrella Guard active");
                    break;
                case GadgetEffectKind.DholBurst:
                    ApplyDholBurst(effect);
                    SetFeedback("Dhol Burst");
                    break;
                case GadgetEffectKind.TiffinStation:
                    SpawnTiffin(effect);
                    SetFeedback("Tiffin Station deployed");
                    break;
            }
        }

        private void ApplyDholBurst(GadgetEffect effect)
        {
            if (effect.Displacements != null && effect.Displacements.Length > 0)
            {
                var targets = FindObjectsByType<CombatTarget>();
                for (var i = 0; i < effect.Displacements.Length; i++)
                {
                    var displacement = effect.Displacements[i];
                    for (var targetIndex = 0; targetIndex < targets.Length; targetIndex++)
                    {
                        var target = targets[targetIndex];
                        if (target == null || target.Id != displacement.TargetId) continue;
                        if (match != null && match.Simulation != null)
                        {
                            // Authority-driven views may have an intentionally disabled
                            // CharacterController. A failed authority lookup means the
                            // target left the match; never fall back to Unity physics in
                            // that path or a teardown/defeated view can emit a runtime
                            // CharacterController.Move error.
                            match.ApplyAuthoritativeDisplacement(displacement);
                        }
                        else
                        {
                            var controller = target.GetComponent<CharacterController>();
                            if (controller != null && controller.enabled)
                            {
                                controller.Move(new Vector3(
                                    displacement.Displacement.X,
                                    0f,
                                    displacement.Displacement.Y));
                            }
                        }
                        break;
                    }
                }

                return;
            }

            // Local lab fallback when no match authority is active.
            var agents = FindObjectsByType<MovementPlayerAgent>();
            for (var i = 0; i < agents.Length; i++)
            {
                var other = agents[i];
                if (other == movementAgent || Vector3.Distance(transform.position, other.transform.position) > effect.Definition.Radius) continue;
                var delta = new Float2(other.transform.position.x - transform.position.x, other.transform.position.z - transform.position.z).Normalized;
                var move = delta * effect.Definition.Magnitude * 0.08f;
                var controller = other.GetComponent<CharacterController>();
                if (controller != null && controller.enabled)
                {
                    controller.Move(new Vector3(move.X, 0f, move.Y));
                }
            }
        }

        private void SpawnTiffin(GadgetEffect effect)
        {
            var station = new GameObject("TiffinStation", typeof(MeshFilter), typeof(MeshRenderer));
            station.transform.position = new Vector3(effect.Command.Origin.X, 0.5f, effect.Command.Origin.Y);
            station.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            station.GetComponent<MeshFilter>().sharedMesh = PresentationMeshFactory.Cylinder("TiffinStationBody", 16);
            station.GetComponent<MeshRenderer>().sharedMaterial = CreateRuntimeMaterial(new Color(0.96f, 0.58f, 0.12f, 1f));
            var component = station.AddComponent<GadgetStation>();
            component.Configure(effect.Definition, effect.StationId);
            var target = station.AddComponent<CombatTarget>();
            var stationHealth = station.GetComponent<CombatHealth>();
            target.enabled = true;
        }

        private static Material CreateRuntimeMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = new Material(shader) { color = color };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            return material;
        }

        private void SetFeedback(string value)
        {
            _feedback = value;
            _feedbackRemaining = 2f;
        }

        private int NextTick(int simulationTick = -1)
        {
            if (simulationTick >= 0) return simulationTick;
            if (UsesAuthority && match.IsMatchStarted)
            {
                return match.SimulationTick;
            }

            return _clock != null
                ? (_activeSimulationTick >= 0 ? _activeSimulationTick : _clock.Tick)
                : _tick++;
        }
    }
}
