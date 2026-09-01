using System;
using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Visuals;
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
        [Range(0.5f, 1f)] [SerializeField] private float botWeaponDamageMultiplier = 1f;
        [SerializeField] private int simulationTickRate = 30;
        [SerializeField] private bool authorityDrivenMovement;
        [SerializeField] private bool autoStart = true;
        [Header("Tutorial safety")]
        [SerializeField] private bool tutorialMode;

        private readonly List<MatchActorBinding> _actors = new List<MatchActorBinding>(8);
        private OfflineMatchAuthority _authority;
        private BastionCrownMatch _bastionCrown;
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
        public bool IsTutorialMode => tutorialMode;
        public float SimulationStepSeconds => _simulationClock != null ? (float)_simulationClock.StepSeconds : 1f / Mathf.Max(1, simulationTickRate);
        public double SimulationInterpolationAlpha => _simulationClock != null ? _simulationClock.InterpolationAlpha : 0d;
        public bool AuthorityDrivenMovement => authorityDrivenMovement;
        public BastionCrownMatch BastionCrown => _bastionCrown;
        public bool IsBastionCrown => _bastionCrown != null;
        public float BastionElapsedSeconds => _bastionCrown != null ? _bastionCrown.ElapsedSeconds : 0f;
        public bool BastionOvertime => _bastionCrown != null && _bastionCrown.IsOvertime;
        public CrownSparkSnapshot BastionCrownState => _bastionCrown != null ? _bastionCrown.Crown : default(CrownSparkSnapshot);
        public TeamScore BastionRajaScore => _bastionCrown != null ? _bastionCrown.GetTeamScore(BastionTeamId.Raja) : default(TeamScore);
        public TeamScore BastionRivalScore => _bastionCrown != null ? _bastionCrown.GetTeamScore(BastionTeamId.Rival) : default(TeamScore);
        public TeamTicketPool BastionRajaTickets => _bastionCrown != null ? _bastionCrown.GetTickets(BastionTeamId.Raja) : default(TeamTicketPool);
        public TeamTicketPool BastionRivalTickets => _bastionCrown != null ? _bastionCrown.GetTickets(BastionTeamId.Rival) : default(TeamTicketPool);
        public BastionResultSummary BastionResult => _bastionCrown != null ? _bastionCrown.Result : default(BastionResultSummary);

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
                _bastionCrown != null ? MatchReplayScenario.BastionCrown : MatchReplayScenario.SoloRaja,
                participants,
                (MatchPickupDefinition[])_replayPickupDefinitions.Clone(),
                (GadgetPickupDefinition[])_replayGadgetPickupDefinitions.Clone(),
                _bastionCrown != null);
        }
#endif

        public ulong CalculateDeterministicTickHash(MatchAuthorityTick tick)
        {
            if (_authority == null) return 0UL;
            var authorityHash = _authority.CalculateDeterministicTickHash(
                tick,
                Simulation != null ? Simulation.GetSnapshots() : null);
            if (_bastionCrown == null) return authorityHash;
            var combined = MatchStateHashBuilder.Create();
            combined.CombineULong(authorityHash);
            combined.CombineULong(_bastionCrown.CalculateDeterministicHash(tick.SimulationTick));
            return combined.Value;
        }

        public GadgetUseResult TryUseGadget(GadgetUseCommand command)
        {
            if (_authority == null) return new GadgetUseResult(false, GadgetUseFailure.NotHeld, default(GadgetEffect));
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AuthorityGadgetCommandCaptured?.Invoke(command);
#endif
            var result = _authority.TryUseGadget(command);
            if (result.Used)
            {
                _bastionCrown?.RecordGadgetUse(command.UserId, result.EventId);
            }

            return result;
        }

        public bool TryAcquireGadget(CombatEntityId collectorId, ContentId gadgetId)
        {
            return _authority != null && _authority.TryAcquireGadget(collectorId, gadgetId);
        }

        public bool AreActorsHostile(CombatEntityId first, CombatEntityId second)
        {
            return _authority != null && _authority.AreActorsHostile(first, second);
        }

        /// <summary>
        /// Diagnostic/test seam for fast-forwarded offline fixtures. Production
        /// gameplay clears protection through elapsed authority ticks or a valid
        /// combat action; this method never mutates the pure team result layer.
        /// </summary>
        public void ClearSpawnProtection(CombatEntityId actorId)
        {
            _authority?.ClearSpawnProtection(actorId);
            _bastionCrown?.ClearSpawnProtection(actorId);
        }

        /// <summary>
        /// Supplies the deterministic squad-blackboard assignment for the
        /// production bot adapter. Combat targeting remains shared with Solo,
        /// while the Bastion planner adds role, spacing, escort/intercept and
        /// ticket-risk intent without granting hidden information.
        /// </summary>
        public bool TryGetBastionBotIntent(
            CombatEntityId actorId,
            out Float2 movement,
            out Float2 aim,
            out BastionSquadPlan plan)
        {
            movement = Float2.Zero;
            aim = Float2.Up;
            plan = BastionSquadPlan.Regroup;
            if (_bastionCrown == null || !_bastionCrown.IsLive ||
                !_bastionCrown.TryGetParticipant(actorId, out var self) || !self.Alive)
            {
                return false;
            }

            if (!_bastionCrown.TryGetSquadIntent(actorId, out var intent)) return false;
            movement = intent.Movement;
            aim = intent.Aim;
            plan = intent.Plan;
            return movement.SqrMagnitude > 0.000001f;
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
            var result = _authority.TryStartPehelCharge(command, movement, facing);
            if (result.Accepted)
            {
                _bastionCrown?.RecordAbilityUse(command.InstigatorId, result.AbilityExecutionId);
            }

            return result;
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
            var result = _authority.TryStartBijliDash(command, movement, facing);
            if (result.Accepted)
            {
                _bastionCrown?.RecordAbilityUse(command.InstigatorId, result.AbilityExecutionId);
            }

            return result;
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
            var snapshot = _authority.TrySpawnMayaDecoy(ownerId, simulationTick, position);
            if (snapshot.Active && snapshot.AbilityExecutionId > 0)
            {
                _bastionCrown?.RecordAbilityUse(ownerId, snapshot.AbilityExecutionId);
            }

            return snapshot;
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
            // Publish one shared, bounded-lag squad signal before any bot
            // receives its command callback. This keeps all four teammates on
            // the same deterministic objective/escort/support assignment.
            _bastionCrown?.BeginSquadCommandPhase(simulationTick);
            try
            {
                SimulationTickAdvanced?.Invoke(simulationTick, (float)_simulationClock.StepSeconds);
            }
            finally
            {
                _bastionCrown?.EndSquadCommandPhase(simulationTick);
            }
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                if (authorityDrivenMovement)
                {
                    var command = actor.Agent.GetAuthorityCommand(simulationTick);
                    if (_bastionCrown != null)
                    {
                        var carrierSpeed = _bastionCrown.GetMovementMultiplier(actor.Target.Id);
                        if (carrierSpeed < 0.9999f)
                        {
                            command = new MovementCommand(
                                command.ActorId,
                                command.SimulationTick,
                                command.Movement * carrierSpeed,
                                command.Aim);
                        }
                    }
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
            var tick = authorityTick.Result;
            BastionCrownTick bastionTick = default(BastionCrownTick);
            if (_bastionCrown != null)
            {
                SyncBastionFromAuthority(authorityTick);
                ProcessBastionObjectiveInteractions((float)_simulationClock.StepSeconds);
                bastionTick = _bastionCrown.Advance((float)_simulationClock.StepSeconds, simulationTick);
                ApplyBastionRespawns(bastionTick);
                if (bastionTick.MatchEnded)
                {
                    // The legacy snapshot remains useful to existing result and
                    // replay consumers, but its terminal phase is forced only
                    // after the team authority has selected the real outcome.
                    _authority.Simulation.ForceResolve();
                }
            }
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
            // Publish only after every authority-owned subsystem (legacy
            // combat, Bastion objective/respawn and atomic item intents) has
            // reached its post-tick state. Replay capture and diagnostics must
            // observe one coherent boundary rather than a pre-objective mirror.
            AuthorityTickResolved?.Invoke(authorityTick);
            MirrorItemAvailability();
            UpdateSpectator(tick);
            if (tick.MatchEnded || (_bastionCrown != null && bastionTick.MatchEnded))
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
            var useBastionCrown = IsBastionCrownScene();
            if (useBastionCrown && !HasCanonicalBastionSlots(spawns))
            {
                throw new InvalidOperationException("Bastion Crown requires exactly actor slots 1-8; the scene was not silently downgraded to Solo.");
            }

            _replaySpawns = spawns.ToArray();
            _bastionCrown = null;
            var definition = useBastionCrown ? OfflineMatchDefinition.BastionCrown : OfflineMatchDefinition.SoloRaja;
            // Keep the Aandhi warning and zone state visible during onboarding, but do not
            // let a player die while they are reading a lesson card. The safety cadence is
            // scoped to the authored tutorial scene; production Solo/Bastion matches keep
            // their configured outside-damage cadence.
            var effectiveOutsideDamageTickSeconds = tutorialMode ? 1000f : outsideDamageTickSeconds;
            _authority = new OfflineMatchAuthority(definition, effectiveOutsideDamageTickSeconds);
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
                            // The tutorial pickup is deliberately placed on the
                            // player's opening lane. Give that lesson a forgiving
                            // authority radius so an earlier movement exercise
                            // cannot strand the player just outside the item.
                            tutorialMode ? 3f : 1.3f));
                    }
                }
            }

            _replayPickupDefinitions = pickupDefinitions.ToArray();
            _replayGadgetPickupDefinitions = gadgetDefinitions.ToArray();
            _authority.ConfigureItems(_replayPickupDefinitions, _replayGadgetPickupDefinitions);
            _authority.Start(spawns);
            var bastionSlots = useBastionCrown ? new List<BastionCrownSlot>(BastionCrownMatch.ParticipantCount) : null;
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                _authority.ConfigureFaction(actor.Target.Id, actor.Target.Faction);
                // Solo Raja is a true free-for-all. Bastion Crown uses the
                // explicit team group only for authority target filtering; the
                // first-class team result/objective state lives in _bastionCrown.
                var team = actor.Target.Id.Value <= 4 ? BastionTeamId.Raja : BastionTeamId.Rival;
                _authority.ConfigureCombatGroup(actor.Target.Id, useBastionCrown ? (team == BastionTeamId.Raja ? 1 : 2) : actor.Target.Id.Value);
                if (useBastionCrown)
                {
                    var fighterId = ResolveFighterIdRuntime(actor.Transform);
                    bastionSlots.Add(new BastionCrownSlot(
                        new TeamMember(
                            actor.Target.Id,
                            team,
                            fighterId,
                            ResolveBastionRole(fighterId, actor.Target.Id.Value),
                            actor.Target.Id.Value == 1),
                        new Float2(actor.Transform.position.x, actor.Transform.position.z),
                        actor.Health.MaxHealth));
                }
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
            if (useBastionCrown)
            {
                _bastionCrown = new BastionCrownMatch(
                    unchecked((uint)DateTime.UtcNow.Ticks));
                _bastionCrown.Start(bastionSlots);
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

        private bool IsBastionCrownScene()
        {
            return string.Equals(gameObject.scene.name, "BazaarBastion", StringComparison.Ordinal);
        }

        private static bool HasCanonicalBastionSlots(IReadOnlyList<MatchSpawn> spawns)
        {
            if (spawns == null || spawns.Count != BastionCrownMatch.ParticipantCount) return false;
            for (var i = 0; i < spawns.Count; i++)
            {
                if (spawns[i].Id.Value != i + 1) return false;
            }

            return true;
        }

        private static ContentId ResolveFighterIdRuntime(Transform actor)
        {
            var bijli = actor.GetComponent<BijliFighterController>();
            if (bijli != null && bijli.enabled) return FighterDefinition.Bijli.FighterId;
            var pehel = actor.GetComponent<PehelFighterController>();
            if (pehel != null && pehel.enabled) return FighterDefinition.Pehel.FighterId;
            var maya = actor.GetComponent<MayaFighterController>();
            if (maya != null && maya.enabled) return FighterDefinition.Maya.FighterId;
            throw new InvalidOperationException($"Actor {actor.name} has no active fighter definition.");
        }

        private static BastionRole ResolveBastionRole(ContentId fighterId, int actorId)
        {
            if (fighterId.Equals(FighterDefinition.Pehel.FighterId)) return BastionRole.Anchor;
            if (fighterId.Equals(FighterDefinition.Maya.FighterId)) return BastionRole.Runner;
            // A second Bijli is a deliberate flex/skirmisher slot rather than a
            // hidden fourth fighter or a numerical difficulty override.
            return actorId == 4 || actorId == 8 ? BastionRole.Flex : BastionRole.Skirmisher;
        }

        private void SyncBastionFromAuthority(MatchAuthorityTick authorityTick)
        {
            if (_bastionCrown == null || Simulation == null) return;

            _bastionCrown.SyncAandhi(
                authorityTick.Result.ZoneCenter,
                authorityTick.Result.ZoneRadius,
                authorityTick.Result.AandhiState,
                authorityTick.Result.WarningRemainingSeconds);

            for (var i = 0; i < authorityTick.DamageEvents.Length; i++)
            {
                var damage = authorityTick.DamageEvents[i];
                if (damage.AmountApplied <= 0) continue;
                _bastionCrown.NotifyCombatDamage(
                    damage.InstigatorId,
                    damage.TargetId,
                    damage.AmountApplied,
                    damage.TargetDefeated,
                    damage.EventId);
            }

            var snapshots = Simulation.GetSnapshots();
            for (var i = 0; i < snapshots.Length; i++)
            {
                var snapshot = snapshots[i];
                _bastionCrown.SetPosition(snapshot.Id, snapshot.Position);
                _bastionCrown.SetHealth(snapshot.Id, snapshot.CurrentHealth);
                if (!snapshot.Alive && _bastionCrown.TryGetParticipant(snapshot.Id, out var teamSnapshot) && teamSnapshot.Alive)
                {
                    _bastionCrown.SyncParticipant(snapshot.Id, snapshot.Position, 0, false);
                }
            }
        }

        private void ProcessBastionObjectiveInteractions(float fixedDeltaSeconds)
        {
            if (_bastionCrown == null || !_bastionCrown.IsLive || fixedDeltaSeconds <= 0f) return;

            var crown = _bastionCrown.Crown;
            if (!crown.IsCarried)
            {
                var candidateId = default(CombatEntityId);
                var candidateDistance = float.MaxValue;
                for (var i = 0; i < _actors.Count; i++)
                {
                    var actorId = _actors[i].Target.Id;
                    if (!_bastionCrown.TryGetParticipant(actorId, out var participant) || !participant.Alive) continue;
                    var distance = Float2.Distance(participant.Position, crown.Position);
                    if (distance > _bastionCrown.Definition.Objective.ContactRadius ||
                        distance > candidateDistance ||
                        (Mathf.Abs(distance - candidateDistance) <= 0.0001f &&
                            (candidateId.Value == 0 || actorId.Value >= candidateId.Value)))
                    {
                        continue;
                    }

                    candidateId = actorId;
                    candidateDistance = distance;
                }

                if (candidateId.Value > 0)
                {
                    _bastionCrown.TryPickupCrown(candidateId, fixedDeltaSeconds);
                }
                else
                {
                    // A zero-delta invalid attempt is the domain's explicit
                    // deterministic reset for an interrupted pickup channel.
                    _bastionCrown.TryPickupCrown(new CombatEntityId(1), 0f);
                }

                return;
            }

            var carrierId = crown.CarrierId;
            if (!_bastionCrown.TryGetParticipant(carrierId, out var carrier) || !carrier.Alive)
            {
                _bastionCrown.CancelDeposit(carrierId);
                return;
            }

            var shrine = carrier.TeamId == BastionTeamId.Raja
                ? _bastionCrown.Definition.Raja.ShrinePosition
                : _bastionCrown.Definition.Rival.ShrinePosition;
            if (Float2.Distance(carrier.Position, shrine) <= _bastionCrown.Definition.Objective.ContactRadius * 1.35f)
            {
                _bastionCrown.TryBeginDeposit(carrierId);
            }
            else
            {
                _bastionCrown.CancelDeposit(carrierId);
            }
        }

        private void ApplyBastionRespawns(BastionCrownTick tick)
        {
            if (_bastionCrown == null || _authority == null) return;
            for (var i = 0; i < tick.RespawnedActors.Length; i++)
            {
                var actorId = tick.RespawnedActors[i];
                if (!_bastionCrown.TryGetParticipant(actorId, out var snapshot)) continue;
                if (!_authority.RespawnParticipant(actorId, snapshot.Position)) continue;
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == actorId);
                if (actor == null) continue;
                actor.Agent.ApplyAuthoritativePosition(snapshot.Position);
                actor.Health.SetAuthoritativeHealth(snapshot.CurrentHealth);
                actor.Input?.ResetInputState();
            }
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
            ApplyOutcomePresentation();
        }

        private void ApplyOutcomePresentation()
        {
            if (Results == null || Results.Length == 0) return;
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                var presentation = actor.Transform.GetComponent<FighterPresentation>();
                if (presentation == null) continue;

                for (var resultIndex = 0; resultIndex < Results.Length; resultIndex++)
                {
                    var result = Results[resultIndex];
                    if (result.Id != actor.Target.Id) continue;
                    presentation.SetVictory(result.Placement == 1);
                    break;
                }
            }
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

                if (_bastionCrown != null)
                {
                    var healerId = intent.HealerId.Value > 0 ? intent.HealerId : intent.TargetId;
                    _bastionCrown.NotifyHealing(healerId, intent.TargetId, intent.Amount, intent.EventId);
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
                _bastionCrown?.NotifyHealing(
                    collection.CollectorId,
                    collection.CollectorId,
                    collection.HealAmount,
                    collection.HealingEventId);
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
            if (player == null) return;
            if (_bastionCrown != null && _bastionCrown.TryGetParticipant(new CombatEntityId(1), out var bastionPlayer))
            {
                if (bastionPlayer.Alive)
                {
                    _playerSpectating = false;
                    cameraController?.SetFollowTarget(player.Transform);
                    return;
                }

                if (!_playerSpectating)
                {
                    _playerSpectating = true;
                    player.Input?.ReleasePointerFocus();
                    var nextBastion = SpectatorTargetSelector.SelectNext(Simulation.GetSnapshots(), player.Target.Id);
                    var bastionActor = _actors.FirstOrDefault(binding => binding.Target.Id == nextBastion);
                    if (bastionActor != null) cameraController?.SetFollowTarget(bastionActor.Transform);
                }

                return;
            }

            if (!player.Health.Snapshot.IsDefeated) return;
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
