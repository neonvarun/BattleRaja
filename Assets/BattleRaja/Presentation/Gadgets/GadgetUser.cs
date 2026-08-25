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
            if (combatTarget.Id.Value == 1) _audio?.PlayGadget();
            return true;
        }

        public bool UseForContext(BotPerceptionSnapshot snapshot)
        {
            var visibleHostile = false;
            for (var i = 0; i < snapshot.TargetCount; i++)
            {
                var candidate = snapshot.Targets[i];
                if (candidate.HasLineOfSight && candidate.Faction != CombatFaction.Neutral &&
                    candidate.Faction != snapshot.SelfFaction)
                {
                    visibleHostile = true;
                    break;
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

            var direction = movementAgent != null ? movementAgent.AimDirection : Float2.Up;
            return UseHeldWithDirection(direction);
        }

        private bool UseHeldWithDirection(Float2 direction)
        {
            var command = new GadgetUseCommand(
                combatTarget.Id,
                _inventory.HeldGadget,
                new Float2(transform.position.x, transform.position.z),
                direction,
                NextTick());
            var result = TryUse(command, out var authoritative);
            if (!result.Used) return false;
            if (authoritative)
            {
                if (!_inventory.TryConsume(command.GadgetId)) return false;
                _runtime.ApplyAuthoritativeUse(result.Effect.Definition);
            }

            ApplyEffect(result.Effect);
            SuccessfulUseCount++;
            if (combatTarget.Id.Value == 1) _audio?.PlayGadget();
            return true;
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
                        if (match == null || !match.ApplyAuthoritativeDisplacement(displacement))
                        {
                            target.GetComponent<CharacterController>()?.Move(new Vector3(
                                displacement.Displacement.X,
                                0f,
                                displacement.Displacement.Y));
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
                other.GetComponent<CharacterController>()?.Move(new Vector3(move.X, 0f, move.Y));
            }
        }

        private void SpawnTiffin(GadgetEffect effect)
        {
            var station = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            station.name = "TiffinStation";
            station.transform.position = new Vector3(effect.Command.Origin.X, 0.5f, effect.Command.Origin.Y);
            station.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            var component = station.AddComponent<GadgetStation>();
            component.Configure(effect.Definition, effect.StationId);
            var target = station.AddComponent<CombatTarget>();
            var stationHealth = station.GetComponent<CombatHealth>();
            target.enabled = true;
        }

        private void SetFeedback(string value)
        {
            _feedback = value;
            _feedbackRemaining = 2f;
        }

        private int NextTick()
        {
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
