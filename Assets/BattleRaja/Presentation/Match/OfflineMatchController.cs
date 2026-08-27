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
        // V1 bots never receive a damage bonus. A small conservative reduction is
        // allowed for the solo PvE difficulty curve, but values above human damage
        // are clamped out of the production path.
        [Range(0.5f, 1f)] [SerializeField] private float botWeaponDamageMultiplier = 0.9f;
        [SerializeField] private int simulationTickRate = 30;
        [SerializeField] private bool authorityDrivenMovement;
        [SerializeField] private bool autoStart = true;

        private readonly List<MatchActorBinding> _actors = new List<MatchActorBinding>(8);
        private OfflineMatchAuthority _authority;
        private FixedSimulationClock _simulationClock;
        private bool _playerSpectating;
        private bool _resultsShown;
        private MatchSpawn[] _replaySpawns = Array.Empty<MatchSpawn>();
        private MatchPickupDefinition[] _replayPickupDefinitions = Array.Empty<MatchPickupDefinition>();
        private GadgetPickupDefinition[] _replayGadgetPickupDefinitions = Array.Empty<GadgetPickupDefinition>();

        /// <summary>
        /// The single match tick source. Authority-relevant presentation adapters
        /// subscribe here instead of constructing independent simulation clocks.
        /// </summary>
        public event Action<int, float> SimulationTickAdvanced;

        /// <summary>Immutable post-advance view for diagnostics and test harnesses.</summary>
        public event Action<MatchAuthorityTick> AuthorityTickResolved;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public static string SuppressAutomaticStartForHarnessSceneName { get; set; }
        /// <summary>
        /// Editor/development-only switch used by the production bot harness. When
        /// enabled, the harness owns canonical tick advancement through
        /// <see cref="AdvanceHarnessSimulationTick"/> so render-frame timing cannot
        /// change the gameplay command stream.
        /// </summary>
        public static bool SuppressAutomaticSimulationForHarness { get; set; }

        /// <summary>Diagnostic-only command taps used to persist production replay files.</summary>
        public event Action<MovementCommand> AuthorityMovementCommandCaptured;
        public event Action<AttackCommand> AuthorityAttackCommandCaptured;
        public event Action<MatchReplayAbilityCommand> AuthorityAbilityCommandCaptured;
        public event Action<GadgetUseCommand> AuthorityGadgetCommandCaptured;
        public event Action<CombatEntityId, int> AuthorityPehelChargeStepCaptured;
#endif

        public OfflineMatchSimulation Simulation => _authority != null ? _authority.Simulation : null;
        public ArenaCollisionDefinition CollisionDefinition => _authority != null
            ? _authority.CollisionDefinition
            : ArenaCollisionDefinition.BazaarBastion;
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

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>
        /// Captures the immutable inputs and content configuration currently owned by
        /// this offline authority. The returned header is for diagnostics/replay files;
        /// it does not expose mutable authority state.
        /// </summary>
        public MatchReplayHeader CreateReplayHeader(uint matchSeed)
        {
            if (_authority == null || _simulationClock == null)
            {
                throw new InvalidOperationException("Start the match before creating a replay header.");
            }

            var participants = new MatchReplayParticipant[_actors.Count];
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                var attack = actor.Transform.GetComponent<CombatAttackController>();
                var weapon = attack != null ? attack.AuthorityWeaponDefinition : ProjectileWeaponDefinition.TrainingBolt;
                var autonomous = actor.Transform.GetComponent<BotBrain>() != null;
                if (autonomous || actor.Target.Id.Value > 1) weapon = ScaleAutonomousBotWeapon(weapon);

                participants[i] = new MatchReplayParticipant(
                    actor.Target.Id,
                    actor.Target.Faction,
                    weapon,
                    actor.Agent.Tuning,
                    ResolveFighterId(actor.Transform),
                    attack != null ? attack.AuthorityTickRate : simulationTickRate);
            }

            return new MatchReplayHeader(
                CollisionDefinition.CollisionVersion,
                matchSeed,
                (MatchSpawn[])_replaySpawns.Clone(),
                (float)_simulationClock.StepSeconds,
                MatchReplayScenario.SoloRaja,
                participants,
                (MatchPickupDefinition[])_replayPickupDefinitions.Clone(),
                (GadgetPickupDefinition[])_replayGadgetPickupDefinitions.Clone());
        }
#endif

        public ulong CalculateDeterministicTickHash(MatchAuthorityTick tick)
        {
            return _authority != null
                ? _authority.CalculateDeterministicTickHash(tick, Simulation != null ? Simulation.GetSnapshots() : null)
                : 0UL;
        }

        public GadgetUseResult TryUseGadget(GadgetUseCommand command)
        {
            if (_authority == null) return new GadgetUseResult(false, GadgetUseFailure.NotHeld, default(GadgetEffect));
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AuthorityGadgetCommandCaptured?.Invoke(command);
#endif
            return _authority.TryUseGadget(command);
        }

        public bool TryAcquireGadget(CombatEntityId collectorId, ContentId gadgetId)
        {
            return _authority != null && _authority.TryAcquireGadget(collectorId, gadgetId);
        }

        public bool AreActorsHostile(CombatEntityId first, CombatEntityId second)
        {
            return _authority != null && _authority.AreActorsHostile(first, second);
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
            return TryAcceptAttack(command);
        }

        public MatchAuthorityAttack TryAcceptAttack(AttackCommand command)
        {
            if (_authority == null)
            {
                return new MatchAuthorityAttack(
                    command.InstigatorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.UnknownActor,
                    0);
            }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AuthorityAttackCommandCaptured?.Invoke(command);
#endif
            return _authority.TryAcceptAttack(command);
        }

        public float GetAttackCooldownRemaining(CombatEntityId actorId, int tickRate, int currentTick)
        {
            return _authority != null
                ? _authority.GetAttackCooldownRemaining(actorId, tickRate, currentTick)
                : 0f;
        }

        public MatchAuthorityAbilityStart TryStartPehelCharge(AbilityCommand command, Float2 movement, Float2 facing)
        {
            if (_authority == null)
            {
                return MatchAuthorityAbilityStart.Rejected(
                    command.InstigatorId,
                    command.AbilityId,
                    command.SimulationTick);
            }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AuthorityAbilityCommandCaptured?.Invoke(new MatchReplayAbilityCommand(
                command, movement, facing, false, Float2.Zero));
#endif
            return _authority.TryStartPehelCharge(command, movement, facing);
        }

        public MatchAuthorityAbilityStart TryStartBijliDash(AbilityCommand command, Float2 movement, Float2 facing)
        {
            if (_authority == null)
            {
                return MatchAuthorityAbilityStart.Rejected(
                    command.InstigatorId,
                    FighterDefinition.Bijli.Ability.AbilityId,
                    command.SimulationTick);
            }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AuthorityAbilityCommandCaptured?.Invoke(new MatchReplayAbilityCommand(
                command, movement, facing, false, Float2.Zero));
#endif
            return _authority.TryStartBijliDash(command, movement, facing);
        }

        public MatchAuthorityDashState GetBijliDashState(CombatEntityId actorId)
        {
            return _authority != null
                ? _authority.GetBijliDashState(actorId)
                : new MatchAuthorityDashState(actorId, FighterActionState.Ready, 0f, Float2.Up);
        }

        public bool IsAuthorityMovementLocked(CombatEntityId actorId)
        {
            return _authority != null && _authority.IsAuthorityMovementLocked(actorId);
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
            if (_authority == null) return default(MatchAuthorityChargeThrow);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AuthorityPehelChargeStepCaptured?.Invoke(actorId, simulationTick);
#endif
            return _authority.AdvancePehelCharge(actorId, simulationTick, fixedDeltaSeconds, availableDistance);
        }

        public bool IsAuthorityActor(CombatEntityId actorId)
        {
            return _authority != null && _authority.HasParticipant(actorId);
        }

        /// <summary>
        /// Returns the already-cached Unity view for an authority participant.
        /// Authority-driven ability adapters use this instead of scanning the
        /// scene during a simulation tick. The lookup is presentation-only;
        /// canonical state remains owned by <see cref="OfflineMatchSimulation"/>.
        /// </summary>
        public bool TryGetActorView(
            CombatEntityId actorId,
            out CombatTarget target,
            out MovementPlayerAgent agent,
            out CombatHealth health)
        {
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                if (actor == null || actor.Target == null || actor.Target.Id != actorId) continue;
                target = actor.Target;
                agent = actor.Agent;
                health = actor.Health;
                return true;
            }

            target = null;
            agent = null;
            health = null;
            return false;
        }

        public MatchAuthorityDecoy TrySpawnMayaDecoy(
            CombatEntityId ownerId,
            int simulationTick,
            Float2 position)
        {
            if (_authority == null) return default(MatchAuthorityDecoy);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AuthorityAbilityCommandCaptured?.Invoke(new MatchReplayAbilityCommand(
                new AbilityCommand(
                    ownerId,
                    simulationTick,
                    FighterSpecialDefinition.MayaDecoy.AbilityId,
                    Float2.Up,
                    true),
                Float2.Zero,
                Float2.Up,
                true,
                position));
#endif
            return _authority.TrySpawnMayaDecoy(ownerId, simulationTick, position);
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
            System.Array.Sort(pickups, CompareMatchPickups);
            System.Array.Sort(gadgetPickups, CompareGadgetPickups);
            CacheActors();
            var suppressAutomaticStart = false;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            suppressAutomaticStart = string.Equals(
                SuppressAutomaticStartForHarnessSceneName,
                gameObject.scene.name,
                StringComparison.Ordinal);
            if (suppressAutomaticStart)
            {
                SuppressAutomaticStartForHarnessSceneName = null;
            }
#endif
            if (autoStart && !suppressAutomaticStart && !IsMatchStarted)
            {
                StartMatch();
            }
        }

        private void Update()
        {
            if (Simulation == null)
            {
                return;
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (SuppressAutomaticSimulationForHarness)
            {
                if (Simulation.IsEnded) PublishResults();
                return;
            }
#endif

            // Terminal authority state is immutable. Republishing is idempotent and
            // guarantees the HUD cannot miss the exact frame that ended the tick.
            if (Simulation.IsEnded)
            {
                PublishResults();
                return;
            }

            var simulationSteps = _simulationClock.Consume(Time.deltaTime);
            for (var step = 0; step < simulationSteps; step++)
            {
                var simulationTick = _simulationClock.GetConsumedTick(step);
                if (AdvanceSimulationTick(simulationTick)) break;
            }
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>
        /// Advances exactly one canonical 30 Hz tick for the editor/development
        /// production-bot harness. This deliberately bypasses render time while
        /// preserving the same event, movement, authority and presentation order as
        /// the normal production update path.
        /// </summary>
        public bool AdvanceHarnessSimulationTick()
        {
            if (Simulation == null)
            {
                return false;
            }

            if (Simulation.IsEnded)
            {
                PublishResults();
                return false;
            }

            _simulationClock.Advance();
            AdvanceSimulationTick(_simulationClock.Tick);
            return !Simulation.IsEnded;
        }
#endif

        private bool AdvanceSimulationTick(int simulationTick)
        {
            SimulationTickAdvanced?.Invoke(simulationTick, (float)_simulationClock.StepSeconds);
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                if (authorityDrivenMovement)
                {
                    var command = actor.Agent.GetAuthorityCommand(simulationTick);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    AuthorityMovementCommandCaptured?.Invoke(command);
#endif
                    if (!_authority.IsAuthorityMovementLocked(actor.Target.Id))
                    {
                        var movement = _authority.ResolveMovement(command, (float)_simulationClock.StepSeconds);
                        actor.Agent.ApplyAuthoritativeMovement(movement, (float)_simulationClock.StepSeconds);
                    }
                }
                else
                {
                    _authority.SetPosition(actor.Target.Id, new Float2(actor.Transform.position.x, actor.Transform.position.z));
                }
            }

            var authorityTick = _authority.Advance(simulationTick, (float)_simulationClock.StepSeconds);
            AuthorityTickResolved?.Invoke(authorityTick);
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
            ApplyAuthoritativeDamageEvents(authorityTick);
            ApplyAuthoritativeDashSteps(authorityTick);
            ApplyGadgetAuthorityIntents(authorityTick);
            ApplyAuthoritativeCollections(authorityTick);
            MirrorItemAvailability();
            UpdateSpectator(tick);
            if (tick.MatchEnded)
            {
                PublishResults();
                return true;
            }

            return false;
        }

        public void StartMatch()
        {
            CacheActors();
            var spawns = _actors.Select(actor => new MatchSpawn(actor.Target.Id, new Float2(actor.Transform.position.x, actor.Transform.position.z), actor.Health.MaxHealth)).ToList();
            _replaySpawns = spawns.ToArray();
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

            _replayPickupDefinitions = pickupDefinitions.ToArray();
            _replayGadgetPickupDefinitions = gadgetDefinitions.ToArray();
            _authority.ConfigureItems(_replayPickupDefinitions, _replayGadgetPickupDefinitions);
            _authority.Start(spawns);
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                _authority.ConfigureFaction(actor.Target.Id, actor.Target.Faction);
                // Solo Raja is a true free-for-all even though the presentation
                // compatibility label remains Enemy for every autonomous bot.
                _authority.ConfigureCombatGroup(actor.Target.Id, actor.Target.Id.Value);
                var attack = actor.Transform.GetComponent<CombatAttackController>();
                if (attack != null)
                {
                    var weapon = attack.AuthorityWeaponDefinition;
                    // The diagnostic harness can convert actor 1 into an autonomous
                    // participant. Scale by controller ownership rather than actor
                    // number so that all eight bots share the same bounded PvE policy,
                    // while a normal human actor keeps the authored weapon unchanged.
                    var autonomous = actor.Transform.GetComponent<BotBrain>() != null;
                    if (autonomous || actor.Target.Id.Value > 1)
                    {
                        weapon = ScaleAutonomousBotWeapon(weapon);
                    }

                    _authority.ConfigureWeapon(
                        actor.Target.Id,
                        weapon,
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

        private ProjectileWeaponDefinition ScaleAutonomousBotWeapon(ProjectileWeaponDefinition weapon)
        {
            var multiplier = Mathf.Clamp(botWeaponDamageMultiplier, 0.5f, 1f);
            if (Mathf.Abs(multiplier - 1f) <= 0.0001f) return weapon;
            return new ProjectileWeaponDefinition(
                Mathf.Max(1, Mathf.RoundToInt(weapon.Damage * multiplier)),
                weapon.FireIntervalSeconds,
                weapon.ProjectileSpeed,
                weapon.MaxRange,
                weapon.LifetimeSeconds,
                weapon.Radius,
                weapon.CollisionLayerMask,
                weapon.AllowSelfHit,
                weapon.AllowFriendlyFire);
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private static ContentId ResolveFighterId(Transform actor)
        {
            var bijli = actor.GetComponent<BijliFighterController>();
            if (bijli != null && bijli.enabled) return FighterDefinition.Bijli.FighterId;
            var pehel = actor.GetComponent<PehelFighterController>();
            if (pehel != null && pehel.enabled) return FighterDefinition.Pehel.FighterId;
            var maya = actor.GetComponent<MayaFighterController>();
            if (maya != null && maya.enabled) return FighterDefinition.Maya.FighterId;
            throw new InvalidOperationException($"Actor {actor.name} has no active fighter definition.");
        }
#endif

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

        private static int CompareMatchPickups(MatchPickup left, MatchPickup right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left == null) return 1;
            if (right == null) return -1;

            var xComparison = left.transform.position.x.CompareTo(right.transform.position.x);
            if (xComparison != 0) return xComparison;
            var zComparison = left.transform.position.z.CompareTo(right.transform.position.z);
            if (zComparison != 0) return zComparison;
            var typeComparison = left.PickupType.CompareTo(right.PickupType);
            if (typeComparison != 0) return typeComparison;
            var valueComparison = left.Value.CompareTo(right.Value);
            if (valueComparison != 0) return valueComparison;
            return left.RespawnSeconds.CompareTo(right.RespawnSeconds);
        }

        private static int CompareGadgetPickups(GadgetPickup left, GadgetPickup right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left == null) return 1;
            if (right == null) return -1;

            var xComparison = left.transform.position.x.CompareTo(right.transform.position.x);
            if (xComparison != 0) return xComparison;
            var zComparison = left.transform.position.z.CompareTo(right.transform.position.z);
            if (zComparison != 0) return zComparison;
            return string.CompareOrdinal(left.GadgetId.Value, right.GadgetId.Value);
        }

        private void PublishResults()
        {
            if (_resultsShown || Simulation == null) return;
            Results = Simulation.GetSnapshots();
            _resultsShown = true;
        }

        /// <summary>
        /// Mirrors damage already applied inside the canonical tick to Unity
        /// health views. Presentation-only; no gameplay mutation occurs here.
        /// </summary>
        private void ApplyAuthoritativeDamageEvents(MatchAuthorityTick authorityTick)
        {
            for (var i = 0; i < authorityTick.DamageEvents.Length; i++)
            {
                var damageEvent = authorityTick.DamageEvents[i];
                if (damageEvent.AmountApplied <= 0) continue;
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == damageEvent.TargetId);
                if (actor == null) continue;
                actor.Health.ApplyAuthoritativeDamage(
                    damageEvent.Request,
                    new global::BattleRaja.Core.Domain.DamageResult(
                        true,
                        damageEvent.AmountApplied,
                        damageEvent.TargetDefeated,
                        global::BattleRaja.Core.Domain.DamageRejectionReason.None),
                    damageEvent.CurrentHealthAfter,
                    damageEvent.SimulationTick);
            }
        }

        private void ApplyGadgetAuthorityIntents(MatchAuthorityTick authorityTick)
        {
            // Healing was applied inside the canonical tick; mirror views only.
            for (var i = 0; i < authorityTick.GadgetHealingIntents.Length; i++)
            {
                var intent = authorityTick.GadgetHealingIntents[i];
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == intent.TargetId);
                if (actor != null)
                {
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

        private void ApplyAuthoritativeDashSteps(MatchAuthorityTick authorityTick)
        {
            for (var i = 0; i < authorityTick.BijliDashSteps.Length; i++)
            {
                if (!authorityTick.BijliDashSteps[i].Displacement.Applied) continue;
                var displacement = authorityTick.BijliDashSteps[i].Displacement;
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == displacement.ActorId);
                actor?.Agent.ApplyAuthoritativePosition(displacement.Position);
            }
        }

        /// <summary>
        /// Mirrors atomic in-tick collections to Unity views: pickup health
        /// snapshots and gadget display inventory. Canonical mutation already
        /// happened inside the tick.
        /// </summary>
        private void ApplyAuthoritativeCollections(MatchAuthorityTick authorityTick)
        {
            for (var i = 0; i < authorityTick.PickupCollections.Length; i++)
            {
                var collection = authorityTick.PickupCollections[i];
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == collection.CollectorId);
                if (actor != null) ApplyAuthoritativeHealth(actor);
            }

            for (var i = 0; i < authorityTick.GadgetCollections.Length; i++)
            {
                var collection = authorityTick.GadgetCollections[i];
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == collection.CollectorId);
                actor?.Transform.GetComponent<GadgetUser>()?.TryPickupFromAuthority(collection.GadgetId);
            }
        }

        private void MirrorItemAvailability()
        {
            if (pickups != null)
            {
                for (var i = 0; i < pickups.Length; i++)
                {
                    pickups[i]?.SetAvailable(_authority != null && _authority.IsPickupAvailable(i));
                }
            }

            if (gadgetPickups != null)
            {
                for (var i = 0; i < gadgetPickups.Length; i++)
                {
                    gadgetPickups[i]?.SetAvailable(_authority != null && _authority.IsGadgetPickupAvailable(i));
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
