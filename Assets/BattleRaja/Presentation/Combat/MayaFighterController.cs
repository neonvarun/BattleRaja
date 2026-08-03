using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Visuals;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class MayaFighterController : MonoBehaviour, IFighterAbilityController, IFighterMovementLock
    {
        [SerializeField] private FighterDefinitionAsset fighterDefinition;
        [SerializeField] private PlayerInputAdapter inputAdapter;
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private CombatFaction faction = CombatFaction.Enemy;
        [SerializeField] private Material decoyMaterial;
        [SerializeField] private int simulationTickRate = 30;

        private FighterDefinition _definition;
        private FighterSpecialDefinition _special;
        private DecoyRuntime _runtime;
        private FixedSimulationClock _clock;
        private GameObject _decoyObject;
        private CombatHealth _decoyHealth;
        private bool _abilityHeld;
        private bool _abilityQueued;
        private Float2 _queuedDirection = Float2.Up;

        public ContentId AbilityId => _special.AbilityId;
        public FighterDefinition Definition => _definition;
        public bool IsMovementLocked => false;
        public bool IsDecoyActive => _runtime != null && _runtime.IsActive;
        public float AbilityCooldownRemaining => _runtime != null ? _runtime.CooldownRemaining : 0f;

        private void Awake()
        {
            fighterDefinition = fighterDefinition != null ? fighterDefinition : GetComponent<FighterDefinitionAsset>();
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            _definition = fighterDefinition != null ? fighterDefinition.ToDomain() : FighterDefinition.Maya;
            _special = FighterSpecialDefinition.MayaDecoy;
            _runtime = new DecoyRuntime();
            _clock = new FixedSimulationClock(Mathf.Max(1, simulationTickRate));
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
                var simulationTick = _clock.GetConsumedTick(i);
                if (_abilityQueued)
                {
                    Submit(AbilityCommandFactory.Create(
                        new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1),
                        simulationTick,
                        AbilityId,
                        _queuedDirection,
                        true));
                    _abilityQueued = false;
                }

                _runtime.Advance((float)_clock.StepSeconds, new Float2(transform.position.x, transform.position.z));
                if (!_runtime.IsActive)
                {
                    DestroyDecoy();
                    continue;
                }

                if (_decoyObject == null) SpawnDecoyObject();
                if (_decoyObject != null)
                {
                    _decoyObject.transform.position = new Vector3(_runtime.Position.X, 1f, _runtime.Position.Y);
                }
            }
        }

        public void Submit(AbilityCommand command)
        {
            if (_runtime == null || !command.Pressed || !command.AbilityId.Equals(AbilityId) || !_runtime.TrySpawn(
                    new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1),
                    new Float2(transform.position.x, transform.position.z),
                    _special))
            {
                return;
            }

            GetComponent<FighterPresentation>()?.NotifyAbility();
            SpawnDecoyObject();
        }

        public void ResetFighterState()
        {
            _runtime?.Destroy();
            _abilityHeld = false;
            _abilityQueued = false;
            DestroyDecoy();
        }

        private void SpawnDecoyObject()
        {
            if (_decoyObject != null) return;
            _decoyObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _decoyObject.name = "MayaDecoy";
            _decoyObject.transform.position = new Vector3(_runtime.Position.X, 1f, _runtime.Position.Y);
            _decoyObject.transform.localScale = Vector3.one * 0.9f;
            if (decoyMaterial != null) _decoyObject.GetComponent<Renderer>().sharedMaterial = decoyMaterial;
            _decoyHealth = _decoyObject.AddComponent<CombatHealth>();
            _decoyHealth.ConfigureMaxHealth(_special.Magnitude);
            var target = _decoyObject.AddComponent<CombatTarget>();
            target.Configure(100000 + (movementAgent != null ? movementAgent.ActorId : 1), faction, _decoyHealth);
            _decoyHealth.DamageApplied += OnDecoyDamage;
        }

        private void OnDecoyDamage(CombatDamageEvent damageEvent)
        {
            if (_decoyHealth != null && _decoyHealth.Snapshot.IsDefeated)
            {
                _runtime.Destroy();
                DestroyDecoy();
            }
        }

        private void DestroyDecoy()
        {
            if (_decoyHealth != null) _decoyHealth.DamageApplied -= OnDecoyDamage;
            if (_decoyObject != null) Destroy(_decoyObject);
            _decoyObject = null;
            _decoyHealth = null;
        }
    }

}
