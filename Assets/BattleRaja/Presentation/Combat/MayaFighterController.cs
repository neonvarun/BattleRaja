using BattleRaja.Core.Domain;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Match;
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
        private OfflineMatchController _match;

        public ContentId AbilityId => _special.AbilityId;
        public FighterDefinition Definition => _definition;
        public bool IsMovementLocked => false;
        public bool IsDecoyActive
        {
            get
            {
                if (UsesAuthorityDecoy && _match.TryGetMayaDecoySnapshot(OwnerId, out var snapshot))
                {
                    return snapshot.Active;
                }

                return _runtime != null && _runtime.IsActive;
            }
        }

        public float AbilityCooldownRemaining
        {
            get
            {
                if (UsesAuthorityDecoy && _match.TryGetMayaDecoySnapshot(OwnerId, out var snapshot))
                {
                    return snapshot.CooldownRemaining;
                }

                return _runtime != null ? _runtime.CooldownRemaining : 0f;
            }
        }

        private CombatEntityId OwnerId => new CombatEntityId(movementAgent != null ? movementAgent.ActorId : 1);
        private bool UsesAuthorityDecoy => movementAgent != null && movementAgent.AuthorityDrivenMovement && _match != null;

        private void Awake()
        {
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            _definition = fighterDefinition != null ? fighterDefinition.ToDomain() : FighterDefinition.Maya;
            _match = FindAnyObjectByType<OfflineMatchController>();
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

                if (!UsesAuthorityDecoy)
                {
                    _runtime.Advance((float)_clock.StepSeconds, new Float2(transform.position.x, transform.position.z));
                }

                SyncDecoyView();
            }
        }

        public void Submit(AbilityCommand command)
        {
            if (_runtime == null || !command.Pressed || !command.AbilityId.Equals(AbilityId))
            {
                return;
            }

            var position = new Float2(transform.position.x, transform.position.z);
            if (UsesAuthorityDecoy)
            {
                var authority = _match.TrySpawnMayaDecoy(OwnerId, command.SimulationTick, position);
                if (authority.OwnerId != OwnerId || !authority.Active) return;
            }
            else if (!_runtime.TrySpawn(OwnerId, position, _special))
            {
                return;
            }

            GetComponent<FighterPresentation>()?.NotifyAbility();
            SyncDecoyView();
        }

        public void ResetFighterState()
        {
            _runtime?.Destroy();
            _abilityHeld = false;
            _abilityQueued = false;
            DestroyDecoy();
        }

        private void SyncDecoyView()
        {
            if (UsesAuthorityDecoy)
            {
                if (!_match.TryGetMayaDecoySnapshot(OwnerId, out var authority) || !authority.Active)
                {
                    DestroyDecoy();
                    return;
                }

                if (_decoyObject == null) SpawnDecoyObject(authority);
                if (_decoyObject != null)
                {
                    _decoyObject.transform.position = new Vector3(authority.Position.X, 1f, authority.Position.Y);
                    _decoyHealth?.SetAuthoritativeHealth(authority.CurrentHealth);
                }

                return;
            }

            if (_runtime == null || !_runtime.IsActive)
            {
                DestroyDecoy();
                return;
            }

            if (_decoyObject == null) SpawnDecoyObject(new MatchAuthorityDecoy(
                OwnerId,
                new CombatEntityId(100000 + OwnerId.Value),
                true,
                true,
                _runtime.Position,
                _runtime.CurrentHealth,
                _special.Magnitude,
                _runtime.RemainingSeconds,
                _runtime.CooldownRemaining));
            if (_decoyObject != null)
            {
                _decoyObject.transform.position = new Vector3(_runtime.Position.X, 1f, _runtime.Position.Y);
            }
        }

        private void SpawnDecoyObject(MatchAuthorityDecoy snapshot)
        {
            if (_decoyObject != null) return;
            _decoyObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _decoyObject.name = "MayaDecoy";
            _decoyObject.transform.position = new Vector3(snapshot.Position.X, 1f, snapshot.Position.Y);
            _decoyObject.transform.localScale = Vector3.one * 0.9f;
            if (decoyMaterial != null) _decoyObject.GetComponent<Renderer>().sharedMaterial = decoyMaterial;
            _decoyHealth = _decoyObject.AddComponent<CombatHealth>();
            _decoyHealth.ConfigureMaxHealth(Mathf.Max(1, snapshot.MaxHealth));
            var target = _decoyObject.AddComponent<CombatTarget>();
            target.Configure(snapshot.DecoyId.Value, faction, _decoyHealth);
            _decoyHealth.DamageApplied += OnDecoyDamage;
            RefreshBotPerceptionTargets();
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
            if (_decoyObject != null)
            {
                // Deactivate before deferred destruction so sensors do not retain a
                // target that is already gone from gameplay.
                _decoyObject.SetActive(false);
                Destroy(_decoyObject);
            }
            _decoyObject = null;
            _decoyHealth = null;
            RefreshBotPerceptionTargets();
        }

        private static void RefreshBotPerceptionTargets()
        {
            var sensors = FindObjectsByType<BotPerceptionSensor>();
            for (var i = 0; i < sensors.Length; i++) sensors[i].RefreshTargets();
        }
    }

}
