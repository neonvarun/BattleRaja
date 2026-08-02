using System.Collections.Generic;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
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
        [SerializeField] private bool botControlled;
        [SerializeField] private int simulationTickRate = 30;

        private readonly GadgetInventory _inventory = new GadgetInventory(1);
        private readonly GadgetRuntime _runtime = new GadgetRuntime();
        private FixedSimulationClock _clock;
        private int _tick;
        private float _shieldRemaining;
        private Float2 _shieldDirection = Float2.Up;
        private float _feedbackRemaining;
        private string _feedback = string.Empty;
        private bool _useQueued;

        public bool HasGadget => _inventory.HasGadget;
        public ContentId HeldGadget => _inventory.HeldGadget;
        public float CooldownRemaining => _runtime.CooldownRemaining;
        public float ShieldRemaining => Mathf.Max(0f, _shieldRemaining);
        public string Feedback => _feedback;
        public GadgetInventory Inventory => _inventory;

        private void Awake()
        {
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            combatTarget = combatTarget != null ? combatTarget : GetComponent<CombatTarget>();
            health = health != null ? health : GetComponent<CombatHealth>();
            damageResolver = damageResolver != null ? damageResolver : FindFirstObjectByType<CombatDamageResolver>();
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
        }

        private void Update()
        {
            if (!botControlled && Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
            {
                _useQueued = true;
            }

            var steps = _clock.Consume(Time.deltaTime);
            for (var i = 0; i < steps; i++)
            {
                if (_useQueued)
                {
                    UseHeld();
                    _useQueued = false;
                }

                var delta = (float)_clock.StepSeconds;
                _runtime.Advance(delta);
                _shieldRemaining = Mathf.Max(0f, _shieldRemaining - delta);
                _feedbackRemaining = Mathf.Max(0f, _feedbackRemaining - delta);
                if (_feedbackRemaining <= 0f) _feedback = string.Empty;
            }
        }

        public bool TryPickup(ContentId id)
        {
            var accepted = _inventory.TryPickup(id);
            SetFeedback(accepted ? $"Picked {id.Value}" : "Gadget slot full");
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
            var result = _runtime.TryUse(_inventory, command);
            if (!result.Used)
            {
                SetFeedback(result.Failure.ToString());
                return false;
            }

            ApplyEffect(result.Effect);
            return true;
        }

        public bool UseForContext(BotPerceptionSnapshot snapshot)
        {
            if (!botControlled || !_inventory.HasGadget || snapshot.Targets == null || snapshot.Targets.Length == 0)
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
            var result = _runtime.TryUse(_inventory, command);
            if (!result.Used) return false;
            ApplyEffect(result.Effect);
            return true;
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
            return dot >= 0.15f ? Mathf.CeilToInt(request.RawAmount * 0.30f) : request.RawAmount;
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
            var agents = FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None);
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
            component.Configure(effect.Definition);
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
            return _clock != null ? _clock.Tick : _tick++;
        }
    }
}
