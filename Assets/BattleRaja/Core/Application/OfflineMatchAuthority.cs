using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public readonly struct MatchAuthorityDamage
    {
        public MatchAuthorityDamage(DamageRequest request, DamageResult result, int currentHealthAfter)
        {
            Request = request;
            Result = result;
            CurrentHealthAfter = currentHealthAfter;
        }

        public DamageRequest Request { get; }
        public DamageResult Result { get; }
        public int CurrentHealthAfter { get; }
    }

    public readonly struct MatchAuthorityMovement
    {
        public MatchAuthorityMovement(
            CombatEntityId actorId,
            int simulationTick,
            bool applied,
            MovementStep step,
            Float2 position)
        {
            ActorId = actorId;
            SimulationTick = simulationTick;
            Applied = applied;
            Step = step;
            Position = position;
        }

        public CombatEntityId ActorId { get; }
        public int SimulationTick { get; }
        public bool Applied { get; }
        public MovementStep Step { get; }
        public Float2 Position { get; }
    }

    public readonly struct MatchAuthorityDisplacement
    {
        public MatchAuthorityDisplacement(
            CombatEntityId actorId,
            int simulationTick,
            bool applied,
            Float2 displacement,
            Float2 position)
        {
            ActorId = actorId;
            SimulationTick = simulationTick;
            Applied = applied;
            Displacement = displacement;
            Position = position;
        }

        public CombatEntityId ActorId { get; }
        public int SimulationTick { get; }
        public bool Applied { get; }
        public Float2 Displacement { get; }
        public Float2 Position { get; }
    }

    public readonly struct MatchAuthorityDecoy
    {
        public MatchAuthorityDecoy(
            CombatEntityId ownerId,
            CombatEntityId decoyId,
            bool active,
            bool targetable,
            Float2 position,
            int currentHealth,
            int maxHealth,
            float remainingSeconds,
            float cooldownRemaining)
        {
            OwnerId = ownerId;
            DecoyId = decoyId;
            Active = active;
            Targetable = targetable;
            Position = position;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            RemainingSeconds = remainingSeconds;
            CooldownRemaining = cooldownRemaining;
        }

        public CombatEntityId OwnerId { get; }
        public CombatEntityId DecoyId { get; }
        public bool Active { get; }
        public bool Targetable { get; }
        public Float2 Position { get; }
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public float RemainingSeconds { get; }
        public float CooldownRemaining { get; }
    }

    /// <summary>
    /// Immutable result of one authority-owned Pehel charge step. The Unity
    /// controller consumes these view instructions; it does not decide the
    /// captured target, damage, or throw position.
    /// </summary>
    public readonly struct MatchAuthorityChargeThrow
    {
        public MatchAuthorityChargeThrow(
            CombatEntityId actorId,
            int simulationTick,
            bool accepted,
            ChargeThrowStep step,
            MatchAuthorityDamage damage,
            bool hasDamage,
            MatchAuthorityDisplacement actorDisplacement,
            MatchAuthorityDisplacement targetDisplacement,
            bool hasTargetDisplacement)
        {
            ActorId = actorId;
            SimulationTick = simulationTick;
            Accepted = accepted;
            Step = step;
            Damage = damage;
            HasDamage = hasDamage;
            ActorDisplacement = actorDisplacement;
            TargetDisplacement = targetDisplacement;
            HasTargetDisplacement = hasTargetDisplacement;
        }

        public CombatEntityId ActorId { get; }
        public int SimulationTick { get; }
        public bool Accepted { get; }
        public ChargeThrowStep Step { get; }
        public MatchAuthorityDamage Damage { get; }
        public bool HasDamage { get; }
        public MatchAuthorityDisplacement ActorDisplacement { get; }
        public MatchAuthorityDisplacement TargetDisplacement { get; }
        public bool HasTargetDisplacement { get; }
    }

    public readonly struct MatchAuthorityChargeThrowState
    {
        public MatchAuthorityChargeThrowState(
            CombatEntityId actorId,
            ChargeThrowState state,
            CombatEntityId capturedTargetId,
            float cooldownRemaining)
        {
            ActorId = actorId;
            State = state;
            CapturedTargetId = capturedTargetId;
            CooldownRemaining = cooldownRemaining;
        }

        public CombatEntityId ActorId { get; }
        public ChargeThrowState State { get; }
        public CombatEntityId CapturedTargetId { get; }
        public float CooldownRemaining { get; }
    }

    public readonly struct MatchAuthorityTick
    {
        public MatchAuthorityTick(MatchTickResult result, DamageRequest[] outsideDamageRequests)
            : this(0, result, outsideDamageRequests, Array.Empty<GadgetHealingIntent>(), Array.Empty<int>())
        {
        }

        public MatchAuthorityTick(int simulationTick, MatchTickResult result, DamageRequest[] outsideDamageRequests)
            : this(simulationTick, result, outsideDamageRequests, Array.Empty<GadgetHealingIntent>(), Array.Empty<int>())
        {
        }

        public MatchAuthorityTick(
            int simulationTick,
            MatchTickResult result,
            DamageRequest[] outsideDamageRequests,
            GadgetHealingIntent[] gadgetHealingIntents,
            int[] expiredStationIds)
        {
            SimulationTick = simulationTick;
            Result = result;
            OutsideDamageRequests = outsideDamageRequests ?? Array.Empty<DamageRequest>();
            GadgetHealingIntents = gadgetHealingIntents ?? Array.Empty<GadgetHealingIntent>();
            ExpiredStationIds = expiredStationIds ?? Array.Empty<int>();
        }

        public int SimulationTick { get; }
        public MatchTickResult Result { get; }
        public DamageRequest[] OutsideDamageRequests { get; }
        public GadgetHealingIntent[] GadgetHealingIntents { get; }
        public int[] ExpiredStationIds { get; }
    }

    /// <summary>
    /// Transport- and Unity-independent owner of offline match authority.
    /// Presentation supplies actor observations and consumes immutable tick intents.
    /// </summary>
    public sealed class OfflineMatchAuthority
    {
        private readonly OfflineMatchDefinition _definition;
        private readonly float _outsideDamageTickSeconds;
        private MatchPickupDefinition[] _pickupDefinitions = Array.Empty<MatchPickupDefinition>();
        private GadgetPickupDefinition[] _gadgetPickupDefinitions = Array.Empty<GadgetPickupDefinition>();
        private MatchPickupRuntime[] _pickups = Array.Empty<MatchPickupRuntime>();
        private GadgetPickupRuntime[] _gadgetPickups = Array.Empty<GadgetPickupRuntime>();
        private readonly Dictionary<CombatEntityId, GadgetInventory> _gadgetInventories = new Dictionary<CombatEntityId, GadgetInventory>();
        private readonly Dictionary<CombatEntityId, GadgetRuntime> _gadgetRuntimes = new Dictionary<CombatEntityId, GadgetRuntime>();
        private readonly Dictionary<CombatEntityId, UmbrellaGuardRuntime> _umbrellaGuards = new Dictionary<CombatEntityId, UmbrellaGuardRuntime>();
        private readonly Dictionary<CombatEntityId, MovementMotor> _movementMotors = new Dictionary<CombatEntityId, MovementMotor>();
        private readonly Dictionary<CombatEntityId, MovementTuning> _movementTunings = new Dictionary<CombatEntityId, MovementTuning>();
        private readonly Dictionary<CombatEntityId, int> _lastMovementTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastAbilityDisplacementTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, ChargeThrowRuntime> _pehelChargeRuntimes = new Dictionary<CombatEntityId, ChargeThrowRuntime>();
        private readonly Dictionary<CombatEntityId, int> _lastPehelCommandTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastPehelStepTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastPehelThrowTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, CombatFaction> _participantFactions = new Dictionary<CombatEntityId, CombatFaction>();
        private readonly Dictionary<CombatEntityId, DecoyRuntime> _mayaDecoys = new Dictionary<CombatEntityId, DecoyRuntime>();
        private readonly Dictionary<CombatEntityId, int> _lastDecoySpawnTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastDecoyDamageTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<int, GadgetStationRuntime> _stations = new Dictionary<int, GadgetStationRuntime>();
        private OfflineMatchSimulation _simulation;
        private double _outsideDamageAccumulator;
        private int _lastSimulationTick = -1;
        private int _nextStationId = 1;

        public OfflineMatchAuthority(OfflineMatchDefinition definition, float outsideDamageTickSeconds = 1f)
        {
            if (outsideDamageTickSeconds <= 0f || float.IsNaN(outsideDamageTickSeconds) || float.IsInfinity(outsideDamageTickSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(outsideDamageTickSeconds));
            }

            _definition = definition;
            _outsideDamageTickSeconds = outsideDamageTickSeconds;
        }

        public OfflineMatchSimulation Simulation => _simulation;

        public bool HasParticipant(CombatEntityId id)
        {
            if (_simulation == null) return false;
            var snapshots = _simulation.GetSnapshots();
            for (var i = 0; i < snapshots.Length; i++)
            {
                if (snapshots[i].Id == id) return true;
            }

            return false;
        }

        public void ConfigureItems(
            IReadOnlyList<MatchPickupDefinition> pickups,
            IReadOnlyList<GadgetPickupDefinition> gadgetPickups)
        {
            _pickupDefinitions = pickups != null ? Copy(pickups) : Array.Empty<MatchPickupDefinition>();
            _gadgetPickupDefinitions = gadgetPickups != null ? Copy(gadgetPickups) : Array.Empty<GadgetPickupDefinition>();
        }

        public void Start(IReadOnlyList<MatchSpawn> spawns)
        {
            _simulation = new OfflineMatchSimulation(_definition);
            _simulation.Start(spawns);
            _pickups = new MatchPickupRuntime[_pickupDefinitions.Length];
            for (var i = 0; i < _pickupDefinitions.Length; i++)
            {
                _pickups[i] = new MatchPickupRuntime(_pickupDefinitions[i]);
            }

            _gadgetPickups = new GadgetPickupRuntime[_gadgetPickupDefinitions.Length];
            for (var i = 0; i < _gadgetPickupDefinitions.Length; i++)
            {
                _gadgetPickups[i] = new GadgetPickupRuntime(_gadgetPickupDefinitions[i]);
            }

            _gadgetInventories.Clear();
            _gadgetRuntimes.Clear();
            _umbrellaGuards.Clear();
            _movementMotors.Clear();
            _movementTunings.Clear();
            _lastMovementTicks.Clear();
            _lastAbilityDisplacementTicks.Clear();
            _pehelChargeRuntimes.Clear();
            _lastPehelCommandTicks.Clear();
            _lastPehelStepTicks.Clear();
            _lastPehelThrowTicks.Clear();
            _participantFactions.Clear();
            _mayaDecoys.Clear();
            _lastDecoySpawnTicks.Clear();
            _lastDecoyDamageTicks.Clear();
            _stations.Clear();
            for (var i = 0; i < spawns.Count; i++)
            {
                _gadgetInventories[spawns[i].Id] = new GadgetInventory(1);
                _gadgetRuntimes[spawns[i].Id] = new GadgetRuntime();
                _umbrellaGuards[spawns[i].Id] = new UmbrellaGuardRuntime();
                _movementMotors[spawns[i].Id] = new MovementMotor();
                _movementTunings[spawns[i].Id] = MovementTuning.Default;
                _lastMovementTicks[spawns[i].Id] = -1;
                _lastAbilityDisplacementTicks[spawns[i].Id] = -1;
                _lastDecoySpawnTicks[spawns[i].Id] = -1;
            }

            _outsideDamageAccumulator = 0d;
            _lastSimulationTick = -1;
            _nextStationId = 1;
        }

        /// <summary>
        /// Registers the server-owned faction for a participant. Network
        /// adapters must populate this from authenticated match data rather
        /// than accepting a client-supplied faction on an ability command.
        /// </summary>
        public void ConfigureFaction(CombatEntityId actorId, CombatFaction faction)
        {
            if (!HasParticipant(actorId))
            {
                throw new ArgumentException("Faction can only be configured for a registered match participant.", nameof(actorId));
            }

            _participantFactions[actorId] = faction;
        }

        public bool SetPosition(CombatEntityId id, Float2 position) => RequireSimulation().SetPosition(id, position);

        public void ConfigureMovement(CombatEntityId id, MovementTuning tuning)
        {
            if (!_movementMotors.ContainsKey(id))
            {
                throw new ArgumentException("Movement can only be configured for a registered match participant.", nameof(id));
            }

            _movementTunings[id] = tuning;
        }

        /// <summary>
        /// Resolves one fixed-tick movement command against canonical participant state.
        /// The returned step is a view instruction; Unity must not re-run the motor.
        /// </summary>
        public MatchAuthorityMovement ResolveMovement(
            MovementCommand command,
            float fixedDeltaSeconds)
        {
            var actorId = new CombatEntityId(command.ActorId);
            var simulation = RequireSimulation();
            var hasCurrent = simulation.TryGetSnapshot(actorId, out var current);
            if (!_movementMotors.TryGetValue(actorId, out var motor) ||
                !_movementTunings.TryGetValue(actorId, out var tuning) ||
                command.SimulationTick < 0 ||
                fixedDeltaSeconds <= 0f ||
                float.IsNaN(fixedDeltaSeconds) ||
                float.IsInfinity(fixedDeltaSeconds) ||
                !_lastMovementTicks.TryGetValue(actorId, out var lastTick) ||
                command.SimulationTick <= lastTick ||
                !hasCurrent ||
                !current.Alive)
            {
                return new MatchAuthorityMovement(actorId, command.SimulationTick, false, default(MovementStep), current.Position);
            }

            var step = motor.Step(command, fixedDeltaSeconds, tuning);
            var position = current.Position + step.Displacement;
            if (!simulation.SetPosition(actorId, position))
            {
                return new MatchAuthorityMovement(actorId, command.SimulationTick, false, step, current.Position);
            }

            _lastMovementTicks[actorId] = command.SimulationTick;
            return new MatchAuthorityMovement(actorId, command.SimulationTick, true, step, position);
        }

        public MatchAuthorityDisplacement ResolveAbilityDisplacement(
            CombatEntityId actorId,
            int simulationTick,
            Float2 displacement)
        {
            var simulation = RequireSimulation();
            var hasCurrent = simulation.TryGetSnapshot(actorId, out var current);
            if (!_lastAbilityDisplacementTicks.TryGetValue(actorId, out var lastTick) ||
                simulationTick < 0 ||
                simulationTick <= lastTick ||
                !displacement.IsFinite ||
                !hasCurrent ||
                !current.Alive)
            {
                return new MatchAuthorityDisplacement(actorId, simulationTick, false, Float2.Zero, current.Position);
            }

            var position = current.Position + displacement;
            if (!simulation.SetPosition(actorId, position))
            {
                return new MatchAuthorityDisplacement(actorId, simulationTick, false, Float2.Zero, current.Position);
            }

            _lastAbilityDisplacementTicks[actorId] = simulationTick;
            return new MatchAuthorityDisplacement(actorId, simulationTick, true, displacement, position);
        }

        /// <summary>
        /// Starts Pehel's charge in the authority-owned runtime. The
        /// presentation controller submits only the common command and local
        /// input context; cooldown and state validation stay here.
        /// </summary>
        public bool TryStartPehelCharge(AbilityCommand command, Float2 movement, Float2 facing)
        {
            if (!command.Pressed || !command.AbilityId.Equals(FighterSpecialDefinition.PehelChargeThrow.AbilityId) ||
                command.SimulationTick < 0 ||
                (_lastPehelCommandTicks.TryGetValue(command.InstigatorId, out var lastTick) && command.SimulationTick <= lastTick) ||
                !RequireSimulation().TryGetSnapshot(command.InstigatorId, out var snapshot) ||
                !snapshot.Alive)
            {
                return false;
            }

            if (!_pehelChargeRuntimes.TryGetValue(command.InstigatorId, out var runtime))
            {
                runtime = new ChargeThrowRuntime(FighterSpecialDefinition.PehelChargeThrow);
                _pehelChargeRuntimes[command.InstigatorId] = runtime;
            }

            if (!runtime.TryStart(command, movement, facing)) return false;
            _lastPehelCommandTicks[command.InstigatorId] = command.SimulationTick;
            return true;
        }

        public MatchAuthorityChargeThrowState GetPehelChargeState(CombatEntityId actorId)
        {
            if (!_pehelChargeRuntimes.TryGetValue(actorId, out var runtime))
            {
                return new MatchAuthorityChargeThrowState(
                    actorId,
                    ChargeThrowState.Ready,
                    default(CombatEntityId),
                    0f);
            }

            return new MatchAuthorityChargeThrowState(
                actorId,
                runtime.State,
                runtime.CapturedTargetId,
                runtime.CooldownRemaining);
        }

        /// <summary>
        /// Advances one fixed Pehel step and resolves capture, damage and
        /// throw displacement against canonical participant snapshots.
        /// </summary>
        public MatchAuthorityChargeThrow AdvancePehelCharge(
            CombatEntityId actorId,
            int simulationTick,
            float fixedDeltaSeconds,
            float availableDistance)
        {
            var runtime = _pehelChargeRuntimes.TryGetValue(actorId, out var found)
                ? found
                : null;
            if (runtime == null || simulationTick < 0 ||
                !_lastPehelCommandTicks.TryGetValue(actorId, out var lastCommandTick) ||
                simulationTick < lastCommandTick)
            {
                return EmptyChargeThrow(actorId, simulationTick, runtime);
            }

            if (_lastPehelStepTicks.TryGetValue(actorId, out var lastStepTick) && simulationTick <= lastStepTick)
            {
                return EmptyChargeThrow(actorId, simulationTick, runtime);
            }

            if (!RequireSimulation().TryGetSnapshot(actorId, out var actorSnapshot) || !actorSnapshot.Alive)
            {
                return EmptyChargeThrow(actorId, simulationTick, runtime);
            }

            var step = runtime.Step(fixedDeltaSeconds, availableDistance);
            _lastPehelStepTicks[actorId] = simulationTick;
            var actorDisplacement = new MatchAuthorityDisplacement(
                actorId,
                simulationTick,
                false,
                Float2.Zero,
                actorSnapshot.Position);
            if (step.Displacement.SqrMagnitude > 0.000001f)
            {
                var position = actorSnapshot.Position + step.Displacement;
                if (RequireSimulation().SetPosition(actorId, position))
                {
                    actorDisplacement = new MatchAuthorityDisplacement(
                        actorId,
                        simulationTick,
                        true,
                        step.Displacement,
                        position);
                }
            }

            // Capture is selected from authority snapshots, not from a client
            // collider hit. The nearest living enemy wins deterministic ties by
            // entity id, and the runtime itself rejects duplicate capture.
            if (step.State == ChargeThrowState.Active)
            {
                TryCaptureNearestPehelTarget(actorId, runtime);
            }

            var damage = default(MatchAuthorityDamage);
            var hasDamage = false;
            var targetDisplacement = default(MatchAuthorityDisplacement);
            var hasTargetDisplacement = false;
            if (step.ThrowTriggered && step.CapturedTargetId.Value > 0)
            {
                hasDamage = TryResolvePehelThrow(
                    actorId,
                    simulationTick,
                    runtime.Direction,
                    step.CapturedTargetId,
                    out damage,
                    out targetDisplacement);
                hasTargetDisplacement = targetDisplacement.Applied;
            }

            return new MatchAuthorityChargeThrow(
                actorId,
                simulationTick,
                true,
                step,
                damage,
                hasDamage,
                actorDisplacement,
                targetDisplacement,
                hasTargetDisplacement);
        }

        private MatchAuthorityChargeThrow EmptyChargeThrow(
            CombatEntityId actorId,
            int simulationTick,
            ChargeThrowRuntime runtime)
        {
            var state = runtime != null
                ? new ChargeThrowStep(runtime.State, Float2.Zero, runtime.CapturedTargetId, false, false)
                : default(ChargeThrowStep);
            return new MatchAuthorityChargeThrow(
                actorId,
                simulationTick,
                false,
                state,
                default(MatchAuthorityDamage),
                false,
                default(MatchAuthorityDisplacement),
                default(MatchAuthorityDisplacement),
                false);
        }

        private bool TryCaptureNearestPehelTarget(
            CombatEntityId actorId,
            ChargeThrowRuntime runtime)
        {
            if (!_participantFactions.TryGetValue(actorId, out var sourceFaction)) return false;
            var simulation = RequireSimulation();
            if (!simulation.TryGetSnapshot(actorId, out var source) || !source.Alive) return false;

            var snapshots = simulation.GetSnapshots();
            var best = default(MatchParticipantSnapshot);
            var bestDistance = float.MaxValue;
            var found = false;
            for (var i = 0; i < snapshots.Length; i++)
            {
                var candidate = snapshots[i];
                if (!candidate.Alive || candidate.Id == actorId ||
                    !_participantFactions.TryGetValue(candidate.Id, out var targetFaction) ||
                    targetFaction == sourceFaction)
                {
                    continue;
                }

                var distance = Float2.Distance(source.Position, candidate.Position);
                if (distance > runtime.Definition.Radius ||
                    (found && (distance > bestDistance ||
                        (Math.Abs(distance - bestDistance) <= 0.0001f && candidate.Id.Value >= best.Id.Value))))
                {
                    continue;
                }

                best = candidate;
                bestDistance = distance;
                found = true;
            }

            if (!found || !_participantFactions.TryGetValue(best.Id, out var bestFaction)) return false;
            return runtime.TryCaptureTarget(best.Id, sourceFaction, bestFaction, bestDistance);
        }

        private bool TryResolvePehelThrow(
            CombatEntityId actorId,
            int simulationTick,
            Float2 direction,
            CombatEntityId targetId,
            out MatchAuthorityDamage damage,
            out MatchAuthorityDisplacement targetDisplacement)
        {
            damage = default(MatchAuthorityDamage);
            targetDisplacement = default(MatchAuthorityDisplacement);
            if (_lastPehelThrowTicks.TryGetValue(actorId, out var lastThrowTick) && simulationTick <= lastThrowTick)
            {
                return false;
            }

            var simulation = RequireSimulation();
            if (!_participantFactions.TryGetValue(actorId, out var sourceFaction) ||
                !_participantFactions.TryGetValue(targetId, out var targetFaction) ||
                !simulation.TryGetSnapshot(targetId, out var target) || !target.Alive)
            {
                _lastPehelThrowTicks[actorId] = simulationTick;
                return false;
            }

            var request = new DamageRequest(
                actorId,
                targetId,
                sourceFaction,
                FighterSpecialDefinition.PehelChargeThrow.Magnitude,
                DamageType.Ability,
                direction,
                simulationTick);
            var result = simulation.ApplyDamage(request, targetFaction, false, false);
            _lastPehelThrowTicks[actorId] = simulationTick;
            var after = simulation.TryGetSnapshot(targetId, out var afterSnapshot)
                ? afterSnapshot
                : target;
            damage = new MatchAuthorityDamage(request, result, after.CurrentHealth);
            if (!result.Applied) return false;

            var displacement = direction.Normalized * (FighterSpecialDefinition.PehelChargeThrow.Magnitude * 0.25f);
            var position = after.Position + displacement;
            if (!simulation.SetPosition(targetId, position)) return false;
            targetDisplacement = new MatchAuthorityDisplacement(
                targetId,
                simulationTick,
                true,
                displacement,
                position);
            return true;
        }

        public MatchAuthorityDecoy TrySpawnMayaDecoy(
            CombatEntityId ownerId,
            int simulationTick,
            Float2 position)
        {
            var simulation = RequireSimulation();
            if (simulationTick < 0 || !position.IsFinite ||
                !_lastDecoySpawnTicks.TryGetValue(ownerId, out var lastTick) ||
                simulationTick <= lastTick ||
                !simulation.TryGetSnapshot(ownerId, out var owner) || !owner.Alive)
            {
                return GetMayaDecoySnapshot(ownerId);
            }

            if (!_mayaDecoys.TryGetValue(ownerId, out var decoy))
            {
                decoy = new DecoyRuntime();
                _mayaDecoys[ownerId] = decoy;
            }

            if (!decoy.TrySpawn(ownerId, position, FighterSpecialDefinition.MayaDecoy))
            {
                return GetMayaDecoySnapshot(ownerId);
            }

            _lastDecoySpawnTicks[ownerId] = simulationTick;
            return GetMayaDecoySnapshot(ownerId);
        }

        public MatchAuthorityDecoy GetMayaDecoySnapshot(CombatEntityId ownerId)
        {
            if (!_mayaDecoys.TryGetValue(ownerId, out var decoy))
            {
                return default(MatchAuthorityDecoy);
            }

            return CreateDecoySnapshot(ownerId, decoy);
        }

        public bool TryGetMayaDecoySnapshot(CombatEntityId decoyId, out MatchAuthorityDecoy snapshot)
        {
            foreach (var pair in _mayaDecoys)
            {
                if (GetDecoyId(pair.Key) != decoyId) continue;
                snapshot = CreateDecoySnapshot(pair.Key, pair.Value);
                return true;
            }

            snapshot = default(MatchAuthorityDecoy);
            return false;
        }

        public bool IsAuthorityDecoy(CombatEntityId decoyId)
        {
            return TryGetMayaDecoySnapshot(decoyId, out _);
        }

        public MatchAuthorityDamage ResolveMayaDecoyDamage(
            DamageRequest request,
            CombatFaction targetFaction,
            bool allowSelfHit,
            bool allowFriendlyFire)
        {
            if (!TryFindMayaDecoy(request.TargetId, out _, out var decoy))
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, false, DamageRejectionReason.WrongTarget),
                    0);
            }

            if (request.RawAmount <= 0)
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, decoy.CurrentHealth <= 0, DamageRejectionReason.InvalidAmount),
                    decoy.CurrentHealth);
            }

            if (!allowSelfHit && request.InstigatorId == request.TargetId)
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, decoy.CurrentHealth <= 0, DamageRejectionReason.SelfHit),
                    decoy.CurrentHealth);
            }

            if (!allowFriendlyFire && request.InstigatorFaction == targetFaction)
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, decoy.CurrentHealth <= 0, DamageRejectionReason.FriendlyFire),
                    decoy.CurrentHealth);
            }

            if (!decoy.IsTargetable)
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, true, DamageRejectionReason.AlreadyDefeated),
                    decoy.CurrentHealth);
            }

            if (_lastDecoyDamageTicks.TryGetValue(request.TargetId, out var lastDamageTick) &&
                request.SimulationTick <= lastDamageTick)
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, decoy.CurrentHealth <= 0, DamageRejectionReason.AlreadyDefeated),
                    decoy.CurrentHealth);
            }

            var before = decoy.CurrentHealth;
            if (!decoy.TryDamage(request.RawAmount))
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, decoy.CurrentHealth <= 0, DamageRejectionReason.AlreadyDefeated),
                    decoy.CurrentHealth);
            }

            _lastDecoyDamageTicks[request.TargetId] = request.SimulationTick;
            var result = new DamageResult(
                true,
                before - decoy.CurrentHealth,
                !decoy.IsTargetable,
                DamageRejectionReason.None);
            return new MatchAuthorityDamage(request, result, decoy.CurrentHealth);
        }

        public bool SyncHealth(CombatEntityId id, int currentHealth) => RequireSimulation().SyncHealth(id, currentHealth);

        public int ApplyHealing(CombatEntityId id, int amount) => RequireSimulation().Heal(id, amount);

        /// <summary>
        /// Routes a resolved combat event through the authority-owned match simulation.
        /// Presentation may report immutable events, but it cannot mutate placements or
        /// statistics by reaching into the simulation directly.
        /// </summary>
        public bool RecordDamage(CombatDamageEvent damageEvent) => RequireSimulation().RecordDamage(damageEvent);

        /// <summary>
        /// Resolves actor damage against the authority-owned health and statistics state.
        /// Unity receives the resulting snapshot/event only after this method succeeds.
        /// </summary>
        public MatchAuthorityDamage ResolveDamage(
            DamageRequest request,
            CombatFaction targetFaction,
            bool allowSelfHit,
            bool allowFriendlyFire)
        {
            var mitigated = ApplyDamageMitigation(request);
            var result = RequireSimulation().ApplyDamage(mitigated, targetFaction, allowSelfHit, allowFriendlyFire);
            var currentHealthAfter = 0;
            var snapshots = RequireSimulation().GetSnapshots();
            for (var i = 0; i < snapshots.Length; i++)
            {
                if (snapshots[i].Id == request.TargetId)
                {
                    currentHealthAfter = snapshots[i].CurrentHealth;
                    break;
                }
            }

            return new MatchAuthorityDamage(mitigated, result, currentHealthAfter);
        }

        public MatchAuthorityTick Advance(float fixedDeltaSeconds)
        {
            return Advance(_lastSimulationTick + 1, fixedDeltaSeconds);
        }

        public MatchAuthorityTick Advance(int simulationTick, float fixedDeltaSeconds)
        {
            if (simulationTick < 0 || simulationTick <= _lastSimulationTick)
            {
                throw new ArgumentOutOfRangeException(nameof(simulationTick), "Simulation ticks must increase monotonically.");
            }

            if (fixedDeltaSeconds <= 0f || float.IsNaN(fixedDeltaSeconds) || float.IsInfinity(fixedDeltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(fixedDeltaSeconds));
            }

            _lastSimulationTick = simulationTick;
            var simulation = RequireSimulation();
            for (var i = 0; i < _pickups.Length; i++) _pickups[i].Advance(fixedDeltaSeconds);
            foreach (var runtime in _gadgetRuntimes.Values) runtime.Advance(fixedDeltaSeconds);
            foreach (var guard in _umbrellaGuards.Values) guard.Advance(fixedDeltaSeconds);
            AdvanceMayaDecoys(fixedDeltaSeconds, simulation);
            var result = simulation.Advance(fixedDeltaSeconds);
            var healingIntents = new List<GadgetHealingIntent>();
            var expiredStationIds = new List<int>();
            AdvanceStations(fixedDeltaSeconds, simulation.GetSnapshots(), healingIntents, expiredStationIds);
            if (result.OutsideDamagePerSecond <= 0)
            {
                _outsideDamageAccumulator = 0d;
                return new MatchAuthorityTick(
                    simulationTick,
                    result,
                    Array.Empty<DamageRequest>(),
                    healingIntents.ToArray(),
                    expiredStationIds.ToArray());
            }

            _outsideDamageAccumulator += fixedDeltaSeconds;
            if (_outsideDamageAccumulator < _outsideDamageTickSeconds)
            {
                return new MatchAuthorityTick(
                    simulationTick,
                    result,
                    Array.Empty<DamageRequest>(),
                    healingIntents.ToArray(),
                    expiredStationIds.ToArray());
            }

            var requests = new List<DamageRequest>(result.OutsideCount);
            while (_outsideDamageAccumulator >= _outsideDamageTickSeconds)
            {
                _outsideDamageAccumulator -= _outsideDamageTickSeconds;
                var snapshots = simulation.GetSnapshots();
                for (var i = 0; i < snapshots.Length; i++)
                {
                    var snapshot = snapshots[i];
                    if (!snapshot.Alive || Float2.Distance(snapshot.Position, result.ZoneCenter) <= result.ZoneRadius) continue;
                    requests.Add(new DamageRequest(
                        new CombatEntityId(-99),
                        snapshot.Id,
                        CombatFaction.Neutral,
                        result.OutsideDamagePerSecond,
                        DamageType.Aandhi,
                        Float2.Zero,
                        simulationTick));
                }
            }

            return new MatchAuthorityTick(
                simulationTick,
                result,
                requests.ToArray(),
                healingIntents.ToArray(),
                expiredStationIds.ToArray());
        }

        /// <summary>
        /// Resolves pickup proximity and collector selection from the authoritative
        /// simulation snapshot. Unity only applies the returned intents to its views.
        /// </summary>
        public MatchAuthorityCollections CollectNearby()
        {
            var simulation = RequireSimulation();
            var snapshots = simulation.GetSnapshots();
            var pickupCollections = new List<MatchPickupCollectionIntent>(_pickups.Length);
            var gadgetCollections = new List<GadgetPickupCollectionIntent>(_gadgetPickups.Length);

            for (var i = 0; i < _pickups.Length; i++)
            {
                var runtime = _pickups[i];
                if (!runtime.IsAvailable) continue;
                var definition = runtime.Definition;
                if (!TrySelectCollector(snapshots, definition.Position, definition.CollectionRadius, true, out var collector)) continue;
                var result = runtime.TryCollect(collector.CurrentHealth, collector.MaxHealth);
                if (result.Collected)
                {
                    pickupCollections.Add(new MatchPickupCollectionIntent(
                        definition.PickupId,
                        collector.Id,
                        result.HealAmount));
                }
            }

            for (var i = 0; i < _gadgetPickups.Length; i++)
            {
                var runtime = _gadgetPickups[i];
                if (!runtime.IsAvailable) continue;
                var definition = runtime.Definition;
                if (!TrySelectCollector(snapshots, definition.Position, definition.CollectionRadius, false, out var collector) ||
                    !_gadgetInventories.TryGetValue(collector.Id, out var inventory) || inventory.HasGadget) continue;
                var result = runtime.TryCollect(false);
                if (result.Collected && inventory.TryPickup(result.GadgetId))
                {
                    gadgetCollections.Add(new GadgetPickupCollectionIntent(
                        definition.PickupId,
                        collector.Id,
                        result.GadgetId));
                }
            }

            return new MatchAuthorityCollections(pickupCollections.ToArray(), gadgetCollections.ToArray());
        }

        public MatchPickupCollectResult TryCollectPickup(int pickupId, int currentHealth, int maxHealth)
        {
            var index = FindPickupIndex(pickupId);
            return index < 0
                ? new MatchPickupCollectResult(false, 0)
                : _pickups[index].TryCollect(currentHealth, maxHealth);
        }

        public bool IsPickupAvailable(int pickupId)
        {
            var index = FindPickupIndex(pickupId);
            return index >= 0 && _pickups[index].IsAvailable;
        }

        public GadgetPickupCollectResult TryCollectGadget(int pickupId, bool hasGadget)
        {
            var index = FindGadgetPickupIndex(pickupId);
            if (index < 0)
            {
                return new GadgetPickupCollectResult(false, default(ContentId));
            }

            return _gadgetPickups[index].TryCollect(hasGadget);
        }

        public GadgetPickupCollectResult TryCollectGadget(CombatEntityId collectorId, int pickupId)
        {
            var index = FindGadgetPickupIndex(pickupId);
            if (index < 0 || !_gadgetInventories.TryGetValue(collectorId, out var inventory))
            {
                return new GadgetPickupCollectResult(false, default(ContentId));
            }

            var result = _gadgetPickups[index].TryCollect(inventory.HasGadget);
            if (result.Collected) inventory.TryPickup(result.GadgetId);
            return result;
        }

        public GadgetUseResult TryUseGadget(GadgetUseCommand command)
        {
            if (!_gadgetInventories.TryGetValue(command.UserId, out var inventory) ||
                !_gadgetRuntimes.TryGetValue(command.UserId, out var runtime))
            {
                return new GadgetUseResult(false, GadgetUseFailure.NotHeld, default(GadgetEffect));
            }

            var result = runtime.TryUse(inventory, command);
            if (!result.Used)
            {
                return result;
            }

            var effect = result.Effect;
            if (effect.Kind == GadgetEffectKind.DholBurst)
            {
                var snapshots = RequireSimulation().GetSnapshots();
                var displacements = new List<GadgetDisplacementIntent>(snapshots.Length);
                for (var i = 0; i < snapshots.Length; i++)
                {
                    var snapshot = snapshots[i];
                    if (!snapshot.Alive || snapshot.Id == command.UserId) continue;
                    var delta = snapshot.Position - command.Origin;
                    if (delta.SqrMagnitude > effect.Definition.Radius * effect.Definition.Radius) continue;
                    var displacement = delta.Normalized * (effect.Definition.Magnitude * 0.08f);
                    var canonicalPosition = snapshot.Position + displacement;
                    if (!RequireSimulation().SetPosition(snapshot.Id, canonicalPosition)) continue;
                    displacements.Add(new GadgetDisplacementIntent(snapshot.Id, displacement));
                }

                effect = new GadgetEffect(
                    effect.Kind,
                    effect.Definition,
                    effect.Command,
                    displacements.ToArray());
            }
            else if (effect.Kind == GadgetEffectKind.TiffinStation)
            {
                var stationId = _nextStationId++;
                _stations[stationId] = new GadgetStationRuntime(stationId, command.Origin, effect.Definition);
                effect = new GadgetEffect(
                    effect.Kind,
                    effect.Definition,
                    effect.Command,
                    effect.Displacements,
                    stationId);
            }
            else if (effect.Kind == GadgetEffectKind.UmbrellaGuard && _umbrellaGuards.TryGetValue(command.UserId, out var guard))
            {
                guard.Activate(effect.Definition, command.Direction);
            }

            return new GadgetUseResult(true, GadgetUseFailure.None, effect);
        }

        public DamageRequest ApplyDamageMitigation(DamageRequest request)
        {
            if (!_umbrellaGuards.TryGetValue(request.TargetId, out var guard)) return request;
            var mitigated = guard.Mitigate(request);
            return mitigated == request.RawAmount
                ? request
                : new DamageRequest(
                    request.InstigatorId,
                    request.TargetId,
                    request.InstigatorFaction,
                    mitigated,
                    request.DamageType,
                    request.HitDirection,
                    request.SimulationTick);
        }

        public bool TryAcquireGadget(CombatEntityId collectorId, ContentId gadgetId)
        {
            return _gadgetInventories.TryGetValue(collectorId, out var inventory) &&
                GadgetCatalog.TryGet(gadgetId, out _) && inventory.TryPickup(gadgetId);
        }

        public GadgetStationDamageResult TryDamageStation(int stationId, int rawAmount)
        {
            GadgetStationRuntime station = null;
            if (stationId > 0)
            {
                _stations.TryGetValue(stationId, out station);
            }

            if (stationId <= 0 || rawAmount <= 0 || station == null || !station.IsActive)
            {
                return new GadgetStationDamageResult(false, 0, false, station != null ? station.CurrentHealth : 0);
            }

            var before = station.CurrentHealth;
            if (!station.TryDamage(rawAmount))
            {
                return new GadgetStationDamageResult(false, 0, station.IsActive == false && station.CurrentHealth == 0, station.CurrentHealth);
            }

            var destroyed = station.CurrentHealth <= 0;
            if (destroyed)
            {
                _stations.Remove(stationId);
            }

            return new GadgetStationDamageResult(true, before - station.CurrentHealth, destroyed, station.CurrentHealth);
        }

        public bool IsGadgetPickupAvailable(int pickupId)
        {
            var index = FindGadgetPickupIndex(pickupId);
            return index >= 0 && _gadgetPickups[index].IsAvailable;
        }

        private void AdvanceMayaDecoys(float fixedDeltaSeconds, OfflineMatchSimulation simulation)
        {
            foreach (var pair in _mayaDecoys)
            {
                var ownerId = pair.Key;
                var decoy = pair.Value;
                if (!decoy.IsActive) continue;
                if (!simulation.TryGetSnapshot(ownerId, out var owner) || !owner.Alive)
                {
                    decoy.Destroy();
                    continue;
                }

                decoy.Advance(fixedDeltaSeconds, owner.Position);
            }
        }

        private bool TryFindMayaDecoy(CombatEntityId decoyId, out CombatEntityId ownerId, out DecoyRuntime decoy)
        {
            foreach (var pair in _mayaDecoys)
            {
                if (GetDecoyId(pair.Key) != decoyId) continue;
                ownerId = pair.Key;
                decoy = pair.Value;
                return true;
            }

            ownerId = default(CombatEntityId);
            decoy = null;
            return false;
        }

        private static CombatEntityId GetDecoyId(CombatEntityId ownerId)
        {
            return new CombatEntityId(100000 + ownerId.Value);
        }

        private static MatchAuthorityDecoy CreateDecoySnapshot(CombatEntityId ownerId, DecoyRuntime decoy)
        {
            return new MatchAuthorityDecoy(
                ownerId,
                GetDecoyId(ownerId),
                decoy.IsActive,
                decoy.IsTargetable,
                decoy.Position,
                decoy.CurrentHealth,
                decoy.MaxHealth,
                decoy.RemainingSeconds,
                decoy.CooldownRemaining);
        }

        private void AdvanceStations(
            float fixedDeltaSeconds,
            MatchParticipantSnapshot[] snapshots,
            List<GadgetHealingIntent> healingIntents,
            List<int> expiredStationIds)
        {
            if (_stations.Count == 0) return;
            var expired = new List<int>();
            foreach (var pair in _stations)
            {
                var step = pair.Value.Advance(fixedDeltaSeconds, snapshots);
                for (var i = 0; i < step.Healing.Length; i++) healingIntents.Add(step.Healing[i]);
                if (step.Expired) expired.Add(pair.Key);
            }

            for (var i = 0; i < expired.Count; i++)
            {
                _stations.Remove(expired[i]);
                expiredStationIds.Add(expired[i]);
            }
        }

        private static T[] Copy<T>(IReadOnlyList<T> source)
        {
            var copy = new T[source.Count];
            for (var i = 0; i < source.Count; i++) copy[i] = source[i];
            return copy;
        }

        private static bool TrySelectCollector(
            MatchParticipantSnapshot[] snapshots,
            Float2 position,
            float collectionRadius,
            bool requireHealth,
            out MatchParticipantSnapshot selected)
        {
            selected = default(MatchParticipantSnapshot);
            var found = false;
            var radiusSquared = collectionRadius * collectionRadius;
            for (var i = 0; i < snapshots.Length; i++)
            {
                var candidate = snapshots[i];
                if (!candidate.Alive || candidate.Position.SqrMagnitudeFrom(position) > radiusSquared ||
                    (requireHealth && candidate.CurrentHealth >= candidate.MaxHealth)) continue;
                if (!found || candidate.Id.Value < selected.Id.Value)
                {
                    selected = candidate;
                    found = true;
                }
            }

            return found;
        }

        private int FindPickupIndex(int pickupId)
        {
            for (var i = 0; i < _pickups.Length; i++)
            {
                if (_pickups[i].Definition.PickupId == pickupId) return i;
            }

            return -1;
        }

        private int FindGadgetPickupIndex(int pickupId)
        {
            for (var i = 0; i < _gadgetPickups.Length; i++)
            {
                if (_gadgetPickups[i].Definition.PickupId == pickupId) return i;
            }

            return -1;
        }

        private OfflineMatchSimulation RequireSimulation()
        {
            if (_simulation == null) throw new InvalidOperationException("Start the match authority before using it.");
            return _simulation;
        }
    }
}
