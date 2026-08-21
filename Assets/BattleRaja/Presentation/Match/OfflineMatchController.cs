using System;
using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.AI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleRaja.Presentation.Match
{
    public sealed class OfflineMatchController : MonoBehaviour
    {
        [SerializeField] private CombatDamageResolver damageResolver;
        [SerializeField] private TopDownCameraController cameraController;
        [SerializeField] private CombatProjectilePool projectilePool;
        [SerializeField] private MatchPickup[] pickups;
        [SerializeField] private GadgetPickup[] gadgetPickups;
        [SerializeField] private float outsideDamageTickSeconds = 1f;
        [SerializeField] private int simulationTickRate = 30;
        [SerializeField] private bool authorityDrivenMovement;
        [SerializeField] private bool autoStart = true;

        private readonly List<MatchActorBinding> _actors = new List<MatchActorBinding>(8);
        private OfflineMatchAuthority _authority;
        private FixedSimulationClock _simulationClock;
        private bool _playerSpectating;
        private bool _resultsShown;

        /// <summary>
        /// The single match tick source. Authority-relevant presentation adapters
        /// subscribe here instead of constructing independent simulation clocks.
        /// </summary>
        public event Action<int, float> SimulationTickAdvanced;

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
        public bool IsMatchStarted => _authority != null && Simulation != null;
        public float SimulationStepSeconds => _simulationClock != null ? (float)_simulationClock.StepSeconds : 1f / Mathf.Max(1, simulationTickRate);
        public double SimulationInterpolationAlpha => _simulationClock != null ? _simulationClock.InterpolationAlpha : 0d;
        public bool AuthorityDrivenMovement => authorityDrivenMovement;

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

        public bool ApplyAuthoritativeDisplacement(GadgetDisplacementIntent displacement)
        {
            if (_authority == null || Simulation == null) return false;
            var actor = _actors.FirstOrDefault(binding => binding.Target.Id == displacement.TargetId);
            if (actor == null || !Simulation.TryGetSnapshot(displacement.TargetId, out var snapshot)) return false;
            actor.Agent.ApplyAuthoritativePosition(snapshot.Position);
            return true;
        }

        public MatchAuthorityDisplacement ResolveAbilityDisplacement(
            CombatEntityId actorId,
            int simulationTick,
            Float2 displacement)
        {
            return _authority != null
                ? _authority.ResolveAbilityDisplacement(actorId, simulationTick, displacement)
                : default(MatchAuthorityDisplacement);
        }

        public MatchAuthorityAttack TryAcceptAttack(
            AttackCommand command,
            ProjectileWeaponDefinition definition,
            int tickRate)
        {
            return _authority != null
                ? _authority.TryAcceptAttack(command)
                : new MatchAuthorityAttack(
                    command.InstigatorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.UnknownActor,
                    0);
        }

        public MatchAuthorityAttack TryAcceptAttack(AttackCommand command)
        {
            return _authority != null
                ? _authority.TryAcceptAttack(command)
                : new MatchAuthorityAttack(
                    command.InstigatorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.UnknownActor,
                    0);
        }

        public float GetAttackCooldownRemaining(CombatEntityId actorId, int tickRate, int currentTick)
        {
            return _authority != null
                ? _authority.GetAttackCooldownRemaining(actorId, tickRate, currentTick)
                : 0f;
        }

        public bool TryStartPehelCharge(AbilityCommand command, Float2 movement, Float2 facing)
        {
            return _authority != null && _authority.TryStartPehelCharge(command, movement, facing);
        }

        public MatchAuthorityChargeThrowState GetPehelChargeState(CombatEntityId actorId)
        {
            return _authority != null
                ? _authority.GetPehelChargeState(actorId)
                : new MatchAuthorityChargeThrowState(actorId, ChargeThrowState.Ready, default(CombatEntityId), 0f);
        }

        public MatchAuthorityChargeThrow AdvancePehelCharge(
            CombatEntityId actorId,
            int simulationTick,
            float fixedDeltaSeconds,
            float availableDistance)
        {
            return _authority != null
                ? _authority.AdvancePehelCharge(actorId, simulationTick, fixedDeltaSeconds, availableDistance)
                : default(MatchAuthorityChargeThrow);
        }

        public bool IsAuthorityActor(CombatEntityId actorId)
        {
            return _authority != null && _authority.HasParticipant(actorId);
        }

        public MatchAuthorityDecoy TrySpawnMayaDecoy(
            CombatEntityId ownerId,
            int simulationTick,
            Float2 position)
        {
            return _authority != null
                ? _authority.TrySpawnMayaDecoy(ownerId, simulationTick, position)
                : default(MatchAuthorityDecoy);
        }

        public bool TryGetMayaDecoySnapshot(CombatEntityId ownerId, out MatchAuthorityDecoy snapshot)
        {
            if (_authority != null)
            {
                snapshot = _authority.GetMayaDecoySnapshot(ownerId);
                return snapshot.OwnerId == ownerId;
            }

            snapshot = default(MatchAuthorityDecoy);
            return false;
        }

        public bool IsAuthorityDecoy(CombatEntityId decoyId)
        {
            return _authority != null && _authority.IsAuthorityDecoy(decoyId);
        }

        public MatchAuthorityDamage ResolveMayaDecoyDamage(
            DamageRequest request,
            CombatFaction targetFaction,
            bool allowSelfHit,
            bool allowFriendlyFire)
        {
            return _authority != null
                ? _authority.ResolveMayaDecoyDamage(request, targetFaction, allowSelfHit, allowFriendlyFire)
                : default(MatchAuthorityDamage);
        }

        public GadgetStationDamageResult TryDamageStation(int stationId, int rawAmount)
        {
            return _authority != null
                ? _authority.TryDamageStation(stationId, rawAmount)
                : new GadgetStationDamageResult(false, 0, false, 0);
        }

        public DamageRequest ApplyDamageMitigation(DamageRequest request)
        {
            return _authority != null ? _authority.ApplyDamageMitigation(request) : request;
        }

        public MatchAuthorityDamage ResolveDamage(
            DamageRequest request,
            CombatFaction targetFaction,
            bool allowSelfHit,
            bool allowFriendlyFire)
        {
            if (_authority == null) return default(MatchAuthorityDamage);
            var result = _authority.ResolveDamage(request, targetFaction, allowSelfHit, allowFriendlyFire);
            if (Simulation != null && Simulation.IsEnded) PublishResults();
            return result;
        }

        private void Awake()
        {
            damageResolver = damageResolver != null ? damageResolver : FindAnyObjectByType<CombatDamageResolver>();
            cameraController = cameraController != null ? cameraController : FindAnyObjectByType<TopDownCameraController>();
            projectilePool = projectilePool != null ? projectilePool : FindAnyObjectByType<CombatProjectilePool>();
            pickups = pickups != null && pickups.Length > 0 ? pickups : FindObjectsByType<MatchPickup>();
            gadgetPickups = gadgetPickups != null && gadgetPickups.Length > 0 ? gadgetPickups : FindObjectsByType<GadgetPickup>();
            CacheActors();
            if (autoStart)
            {
                StartMatch();
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
                var simulationTick = _simulationClock.GetConsumedTick(step);
                SimulationTickAdvanced?.Invoke(simulationTick, (float)_simulationClock.StepSeconds);
                for (var i = 0; i < _actors.Count; i++)
                {
                    var actor = _actors[i];
                    if (authorityDrivenMovement)
                    {
                        var command = actor.Agent.GetAuthorityCommand(simulationTick);
                        var movement = _authority.ResolveMovement(command, (float)_simulationClock.StepSeconds);
                        actor.Agent.ApplyAuthoritativeMovement(movement, (float)_simulationClock.StepSeconds);
                    }
                    else
                    {
                        _authority.SetPosition(actor.Target.Id, new Float2(actor.Transform.position.x, actor.Transform.position.z));
                    }
                }

                var authorityTick = _authority.Advance(simulationTick, (float)_simulationClock.StepSeconds);
                var tick = authorityTick.Result;
                if (projectilePool != null)
                {
                    projectilePool.Reconcile(authorityTick.ProjectileSnapshots);
                }
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

                ApplyGadgetAuthorityIntents(authorityTick);

                CollectNearbyItems();
                UpdateSpectator(tick);
                if (tick.MatchEnded)
                {
                    PublishResults();
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
                        pickup.RespawnSeconds,
                        new Float2(pickup.transform.position.x, pickup.transform.position.z),
                        1.2f));
                }
            }

            var gadgetDefinitions = new List<GadgetPickupDefinition>(gadgetPickups != null ? gadgetPickups.Length : 0);
            if (gadgetPickups != null)
            {
                for (var i = 0; i < gadgetPickups.Length; i++)
                {
                    var pickup = gadgetPickups[i];
                    if (pickup != null)
                    {
                        gadgetDefinitions.Add(new GadgetPickupDefinition(
                            i,
                            pickup.GadgetId,
                            new Float2(pickup.transform.position.x, pickup.transform.position.z),
                            1.3f));
                    }
                }
            }

            _authority.ConfigureItems(pickupDefinitions, gadgetDefinitions);
            _authority.Start(spawns);
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                _authority.ConfigureFaction(actor.Target.Id, actor.Target.Faction);
                var attack = actor.Transform.GetComponent<CombatAttackController>();
                if (attack != null)
                {
                    _authority.ConfigureWeapon(
                        actor.Target.Id,
                        attack.AuthorityWeaponDefinition,
                        attack.AuthorityTickRate);
                }
                actor.Agent.AuthorityDrivenMovement = authorityDrivenMovement;
                actor.Transform.GetComponent<BotBrain>()?.SetMatchController(this);
                if (authorityDrivenMovement)
                {
                    _authority.ConfigureMovement(actor.Target.Id, actor.Agent.Tuning);
                }
            }
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
            _actors.Clear();
            var agents = FindObjectsByType<MovementPlayerAgent>().OrderBy(agent => agent.ActorId);
            foreach (var agent in agents)
            {
                var target = agent.GetComponent<CombatTarget>();
                var health = agent.GetComponent<CombatHealth>();
                if (target != null && health != null)
                {
                    _actors.Add(new MatchActorBinding(agent, agent.transform, target, health, agent.GetComponent<PlayerInputAdapter>()));
                }
            }
        }

        private void PublishResults()
        {
            if (_resultsShown || Simulation == null) return;
            Results = Simulation.GetSnapshots();
            _resultsShown = true;
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

        private void ApplyGadgetAuthorityIntents(MatchAuthorityTick authorityTick)
        {
            for (var i = 0; i < authorityTick.GadgetHealingIntents.Length; i++)
            {
                var intent = authorityTick.GadgetHealingIntents[i];
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == intent.TargetId);
                if (actor != null)
                {
                    _authority.ApplyHealing(intent.TargetId, intent.Amount);
                    ApplyAuthoritativeHealth(actor);
                }
            }

            if (authorityTick.ExpiredStationIds.Length == 0) return;
            var stations = FindObjectsByType<GadgetStation>();
            for (var i = 0; i < authorityTick.ExpiredStationIds.Length; i++)
            {
                var stationId = authorityTick.ExpiredStationIds[i];
                for (var stationIndex = 0; stationIndex < stations.Length; stationIndex++)
                {
                    if (stations[stationIndex] != null && stations[stationIndex].StationId == stationId)
                    {
                        stations[stationIndex].ExpireFromAuthority();
                        break;
                    }
                }
            }
        }

        private void CollectNearbyItems()
        {
            var collections = _authority.CollectNearby();
            for (var i = 0; i < collections.PickupCollections.Length; i++)
            {
                var collection = collections.PickupCollections[i];
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == collection.CollectorId);
                if (actor != null)
                {
                    _authority.ApplyHealing(collection.CollectorId, collection.HealAmount);
                    ApplyAuthoritativeHealth(actor);
                }
                if (pickups != null && collection.PickupId >= 0 && collection.PickupId < pickups.Length)
                {
                    pickups[collection.PickupId]?.SetAvailable(false);
                }
            }

            for (var i = 0; i < collections.GadgetCollections.Length; i++)
            {
                var collection = collections.GadgetCollections[i];
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == collection.CollectorId);
                var user = actor?.Transform.GetComponent<GadgetUser>();
                if (user != null)
                {
                    user.TryPickupFromAuthority(collection.GadgetId);
                }
                if (gadgetPickups != null && collection.PickupId >= 0 && collection.PickupId < gadgetPickups.Length)
                {
                    gadgetPickups[collection.PickupId]?.SetAvailable(false);
                }
            }

            if (pickups != null)
            {
                for (var i = 0; i < pickups.Length; i++)
                {
                    pickups[i]?.SetAvailable(_authority.IsPickupAvailable(i));
                }
            }

            if (gadgetPickups != null)
            {
                for (var i = 0; i < gadgetPickups.Length; i++)
                {
                    gadgetPickups[i]?.SetAvailable(_authority.IsGadgetPickupAvailable(i));
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

        private void ApplyAuthoritativeHealth(MatchActorBinding actor)
        {
            if (actor == null || Simulation == null) return;
            var snapshots = Simulation.GetSnapshots();
            for (var i = 0; i < snapshots.Length; i++)
            {
                if (snapshots[i].Id == actor.Target.Id)
                {
                    actor.Health.SetAuthoritativeHealth(snapshots[i].CurrentHealth);
                    return;
                }
            }
        }

        private sealed class MatchActorBinding
        {
            public MatchActorBinding(MovementPlayerAgent agent, Transform transform, CombatTarget target, CombatHealth health, PlayerInputAdapter input)
            {
                Agent = agent;
                Transform = transform;
                Target = target;
                Health = health;
                Input = input;
            }

            public MovementPlayerAgent Agent { get; }
            public Transform Transform { get; }
            public CombatTarget Target { get; }
            public CombatHealth Health { get; }
            public PlayerInputAdapter Input { get; }
        }
    }
}
