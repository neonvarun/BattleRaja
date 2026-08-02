using System;
using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Gadgets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleRaja.Presentation.Match
{
    public sealed class OfflineMatchController : MonoBehaviour
    {
        [SerializeField] private CombatDamageResolver damageResolver;
        [SerializeField] private TopDownCameraController cameraController;
        [SerializeField] private MatchPickup[] pickups;
        [SerializeField] private GadgetPickup[] gadgetPickups;
        [SerializeField] private float outsideDamageTickSeconds = 1f;
        [SerializeField] private int simulationTickRate = 30;
        [SerializeField] private bool autoStart = true;

        private readonly List<MatchActorBinding> _actors = new List<MatchActorBinding>(8);
        private OfflineMatchAuthority _authority;
        private FixedSimulationClock _simulationClock;
        private bool _playerSpectating;
        private bool _resultsShown;

        public OfflineMatchSimulation Simulation => _authority != null ? _authority.Simulation : null;
        public MatchPhase CurrentPhase => Simulation != null ? Simulation.Phase : MatchPhase.LoadWarmup;
        public float ZoneRadius { get; private set; }
        public Float2 ZoneCenter { get; private set; }
        public Float2 NextZoneCenter { get; private set; }
        public float NextZoneRadius { get; private set; }
        public AandhiState AandhiState { get; private set; }
        public float AandhiWarningRemainingSeconds { get; private set; }
        public int AliveCount => Simulation != null ? Simulation.AliveCount : 0;
        public bool PlayerSpectating => _playerSpectating;
        public bool ResultsShown => _resultsShown;
        public MatchParticipantSnapshot[] Results { get; private set; }
        public int SimulationTick => _simulationClock != null ? _simulationClock.Tick : 0;
        public double SimulationInterpolationAlpha => _simulationClock != null ? _simulationClock.InterpolationAlpha : 0d;

        public GadgetUseResult TryUseGadget(GadgetUseCommand command)
        {
            return _authority != null
                ? _authority.TryUseGadget(command)
                : new GadgetUseResult(false, GadgetUseFailure.NotHeld, default(GadgetEffect));
        }

        public bool TryAcquireGadget(CombatEntityId collectorId, ContentId gadgetId)
        {
            return _authority != null && _authority.TryAcquireGadget(collectorId, gadgetId);
        }

        private void Awake()
        {
            damageResolver = damageResolver != null ? damageResolver : FindFirstObjectByType<CombatDamageResolver>();
            cameraController = cameraController != null ? cameraController : FindFirstObjectByType<TopDownCameraController>();
            pickups = pickups != null && pickups.Length > 0 ? pickups : FindObjectsByType<MatchPickup>(FindObjectsSortMode.None);
            gadgetPickups = gadgetPickups != null && gadgetPickups.Length > 0 ? gadgetPickups : FindObjectsByType<GadgetPickup>(FindObjectsSortMode.None);
            CacheActors();
            if (autoStart)
            {
                StartMatch();
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _actors.Count; i++)
            {
                _actors[i].Health.DamageApplied -= OnDamageApplied;
            }
        }

        private void Update()
        {
            if (Simulation == null || Simulation.IsEnded)
            {
                return;
            }

            var simulationSteps = _simulationClock.Consume(Time.deltaTime);
            for (var step = 0; step < simulationSteps; step++)
            {
                for (var i = 0; i < _actors.Count; i++)
                {
                    var actor = _actors[i];
                    _authority.SetPosition(actor.Target.Id, new Float2(actor.Transform.position.x, actor.Transform.position.z));
                    _authority.SyncHealth(actor.Target.Id, actor.Health.Snapshot.CurrentHealth);
                }

                var authorityTick = _authority.Advance(_simulationClock.Tick, (float)_simulationClock.StepSeconds);
                var tick = authorityTick.Result;
                ZoneCenter = tick.ZoneCenter;
                NextZoneCenter = tick.NextZoneCenter;
                ZoneRadius = tick.ZoneRadius;
                NextZoneRadius = tick.NextZoneRadius;
                AandhiState = tick.AandhiState;
                AandhiWarningRemainingSeconds = tick.WarningRemainingSeconds;
                if (authorityTick.OutsideDamageRequests.Length > 0)
                {
                    ApplyOutsideDamage(authorityTick);
                }

                CollectPickups();
                CollectGadgets();
                UpdateSpectator(tick);
                if (tick.MatchEnded)
                {
                    Results = Simulation.GetSnapshots();
                    _resultsShown = true;
                    break;
                }
            }
        }

        public void StartMatch()
        {
            CacheActors();
            var spawns = _actors.Select(actor => new MatchSpawn(actor.Target.Id, new Float2(actor.Transform.position.x, actor.Transform.position.z), actor.Health.MaxHealth)).ToList();
            _authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja, outsideDamageTickSeconds);
            var pickupDefinitions = new List<MatchPickupDefinition>(pickups != null ? pickups.Length : 0);
            if (pickups != null)
            {
                for (var i = 0; i < pickups.Length; i++)
                {
                    var pickup = pickups[i];
                    if (pickup == null) continue;
                    pickupDefinitions.Add(new MatchPickupDefinition(
                        i,
                        MatchPickupKind.Health,
                        pickup.Value,
                        pickup.RespawnSeconds));
                }
            }

            var gadgetDefinitions = new List<GadgetPickupDefinition>(gadgetPickups != null ? gadgetPickups.Length : 0);
            if (gadgetPickups != null)
            {
                for (var i = 0; i < gadgetPickups.Length; i++)
                {
                    var pickup = gadgetPickups[i];
                    if (pickup != null) gadgetDefinitions.Add(new GadgetPickupDefinition(i, pickup.GadgetId));
                }
            }

            _authority.ConfigureItems(pickupDefinitions, gadgetDefinitions);
            _authority.Start(spawns);
            _simulationClock = new FixedSimulationClock(Math.Max(1, simulationTickRate));
            ZoneCenter = Float2.Zero;
            NextZoneCenter = Float2.Zero;
            ZoneRadius = 0f;
            NextZoneRadius = 0f;
            AandhiState = AandhiState.Stable;
            AandhiWarningRemainingSeconds = 0f;
            _playerSpectating = false;
            _resultsShown = false;
            Results = null;
        }

        public void RestartMatch()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void CycleSpectator()
        {
            if (!_playerSpectating || Simulation == null || cameraController == null)
            {
                return;
            }

            var snapshots = Simulation.GetSnapshots();
            var next = SpectatorTargetSelector.SelectNext(snapshots, cameraController.FollowTarget != null
                ? cameraController.FollowTarget.GetComponent<CombatTarget>()?.Id ?? default
                : default);
            var actor = _actors.FirstOrDefault(binding => binding.Target.Id == next);
            if (actor != null) cameraController.SetFollowTarget(actor.Transform);
        }

        private void CacheActors()
        {
            for (var i = 0; i < _actors.Count; i++)
            {
                _actors[i].Health.DamageApplied -= OnDamageApplied;
            }

            _actors.Clear();
            var agents = FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None).OrderBy(agent => agent.ActorId);
            foreach (var agent in agents)
            {
                var target = agent.GetComponent<CombatTarget>();
                var health = agent.GetComponent<CombatHealth>();
                if (target != null && health != null)
                {
                    health.DamageApplied += OnDamageApplied;
                    _actors.Add(new MatchActorBinding(agent.transform, target, health, agent.GetComponent<PlayerInputAdapter>()));
                }
            }
        }

        private void OnDamageApplied(CombatDamageEvent damageEvent)
        {
            Simulation?.RecordDamage(damageEvent);
        }

        private void ApplyOutsideDamage(MatchAuthorityTick authorityTick)
        {
            for (var i = 0; i < authorityTick.OutsideDamageRequests.Length; i++)
            {
                var request = authorityTick.OutsideDamageRequests[i];
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == request.TargetId);
                if (actor == null || actor.Health.Snapshot.IsDefeated) continue;
                damageResolver?.Resolve(actor.Target, request, allowSelfHit: true, allowFriendlyFire: true, authorityTick.SimulationTick);
            }
        }

        private void CollectPickups()
        {
            if (pickups == null) return;
            for (var p = 0; p < pickups.Length; p++)
            {
                var pickup = pickups[p];
                if (pickup == null || !_authority.IsPickupAvailable(p))
                {
                    pickup?.SetAvailable(false);
                    continue;
                }

                pickup.SetAvailable(true);
                for (var i = 0; i < _actors.Count; i++)
                {
                    var actor = _actors[i];
                    if (actor.Health.Snapshot.IsDefeated || Vector3.Distance(actor.Transform.position, pickup.transform.position) > 1.2f) continue;
                    var result = _authority.TryCollectPickup(
                        p,
                        actor.Health.Snapshot.CurrentHealth,
                        actor.Health.MaxHealth);
                    if (!result.Collected) continue;
                    actor.Health.Heal(result.HealAmount);
                    pickup.SetAvailable(false);
                    break;
                }
            }
        }

        private void CollectGadgets()
        {
            if (gadgetPickups == null) return;
            for (var p = 0; p < gadgetPickups.Length; p++)
            {
                var pickup = gadgetPickups[p];
                if (pickup == null || !_authority.IsGadgetPickupAvailable(p))
                {
                    pickup?.SetAvailable(false);
                    continue;
                }

                pickup.SetAvailable(true);
                for (var i = 0; i < _actors.Count; i++)
                {
                    var actor = _actors[i];
                    if (actor.Health.Snapshot.IsDefeated || Vector3.Distance(actor.Transform.position, pickup.transform.position) > 1.3f) continue;
                    var user = actor.Transform.GetComponent<GadgetUser>();
                    if (user == null) continue;
                    var result = _authority.TryCollectGadget(actor.Target.Id, p);
                    if (!result.Collected) continue;
                    user.TryPickupFromAuthority(result.GadgetId);
                    pickup.SetAvailable(false);
                    break;
                }
            }
        }

        private void UpdateSpectator(MatchTickResult tick)
        {
            var player = _actors.FirstOrDefault(actor => actor.Target.Id.Value == 1);
            if (player == null || !player.Health.Snapshot.IsDefeated) return;
            if (!_playerSpectating)
            {
                _playerSpectating = true;
                player.Input?.ReleasePointerFocus();
                var next = SpectatorTargetSelector.SelectNext(Simulation.GetSnapshots(), player.Target.Id);
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == next);
                if (actor != null) cameraController?.SetFollowTarget(actor.Transform);
            }
        }

        private sealed class MatchActorBinding
        {
            public MatchActorBinding(Transform transform, CombatTarget target, CombatHealth health, PlayerInputAdapter input)
            {
                Transform = transform;
                Target = target;
                Health = health;
                Input = input;
            }

            public Transform Transform { get; }
            public CombatTarget Target { get; }
            public CombatHealth Health { get; }
            public PlayerInputAdapter Input { get; }
        }
    }
}
