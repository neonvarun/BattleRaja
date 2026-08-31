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

    public enum MatchAuthorityAttackFailure
    {
        None = 0,
        InvalidCommand = 1,
        UnknownActor = 2,
        DefeatedActor = 3,
        OutOfOrder = 4,
        Cooldown = 5,
        Warmup = 6,
        SpawnProtection = 7,
        FutureTick = 8,
        InvalidSequence = 9,
        Resolution = 10,
        InvalidLoadout = 11,
        StaleTick = 12
    }

    public readonly struct MatchAuthorityAttack
    {
        public MatchAuthorityAttack(
            CombatEntityId actorId,
            int simulationTick,
            bool accepted,
            MatchAuthorityAttackFailure failure,
            int cooldownTicksRemaining)
            : this(
                actorId,
                simulationTick,
                accepted,
                failure,
                cooldownTicksRemaining,
                ProjectileWeaponDefinition.TrainingBolt,
                CombatFaction.Neutral,
                Float2.Zero,
                Float2.Up,
                0,
                0)
        {
        }

        public MatchAuthorityAttack(
            CombatEntityId actorId,
            int simulationTick,
            bool accepted,
            MatchAuthorityAttackFailure failure,
            int cooldownTicksRemaining,
            ProjectileWeaponDefinition weapon,
            CombatFaction faction,
            Float2 origin,
            Float2 direction,
            int projectileId = 0,
            int attackExecutionId = 0)
        {
            ActorId = actorId;
            SimulationTick = simulationTick;
            Accepted = accepted;
            Failure = failure;
            CooldownTicksRemaining = cooldownTicksRemaining;
            Weapon = weapon;
            Faction = faction;
            Origin = origin;
            Direction = direction;
            ProjectileId = projectileId;
            AttackExecutionId = attackExecutionId;
        }

        public CombatEntityId ActorId { get; }
        public int SimulationTick { get; }
        public bool Accepted { get; }
        public MatchAuthorityAttackFailure Failure { get; }
        public int CooldownTicksRemaining { get; }
        public ProjectileWeaponDefinition Weapon { get; }
        public CombatFaction Faction { get; }
        public Float2 Origin { get; }
        public Float2 Direction { get; }
        public int ProjectileId { get; }
        public int AttackExecutionId { get; }
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
            float cooldownRemaining,
            int abilityExecutionId = 0)
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
            AbilityExecutionId = abilityExecutionId;
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

    /// <summary>Stable authority-assigned ability identity for the spawn that
    /// created this decoy (0 for rejected/local snapshots).</summary>
    public int AbilityExecutionId { get; }
    }

    public enum MatchAuthorityActionPhaseFailure
    {
        None = 0,
        UnknownActor = 1,
        DefeatedActor = 2,
        Warmup = 3,
        SpawnProtection = 4,
        Resolution = 5
    }

    public readonly struct MatchAuthorityActionEligibility
    {
        public MatchAuthorityActionEligibility(
            MatchAuthorityActionPhaseFailure failure,
            MatchParticipantSnapshot snapshot)
        {
            Failure = failure;
            Snapshot = snapshot;
        }

        public MatchAuthorityActionPhaseFailure Failure { get; }
        public MatchParticipantSnapshot Snapshot { get; }
        public bool IsEligible => Failure == MatchAuthorityActionPhaseFailure.None;

        public static MatchAuthorityActionEligibility Eligible(MatchParticipantSnapshot snapshot) =>
            new MatchAuthorityActionEligibility(MatchAuthorityActionPhaseFailure.None, snapshot);

        public static MatchAuthorityActionEligibility Rejected(MatchAuthorityActionPhaseFailure failure) =>
            new MatchAuthorityActionEligibility(failure, default(MatchParticipantSnapshot));
    }

    /// <summary>Immutable authority verdict for one validated ability start.</summary>
    public readonly struct MatchAuthorityAbilityStart
    {
        public MatchAuthorityAbilityStart(
            CombatEntityId actorId,
            ContentId abilityId,
            int simulationTick,
            bool accepted,
            int abilityExecutionId)
        {
            ActorId = actorId;
            AbilityId = abilityId;
            SimulationTick = simulationTick;
            Accepted = accepted;
            AbilityExecutionId = abilityExecutionId;
        }

        public static MatchAuthorityAbilityStart Rejected(
            CombatEntityId actorId,
            ContentId abilityId,
            int simulationTick) => new MatchAuthorityAbilityStart(actorId, abilityId, simulationTick, false, 0);

        public CombatEntityId ActorId { get; }
        public ContentId AbilityId { get; }
        public int SimulationTick { get; }
        public bool Accepted { get; }

        /// <summary>Stable authority-assigned ability identity; 0 when rejected.</summary>
        public int AbilityExecutionId { get; }
    }

    /// <summary>
    /// Immutable authority view for one fighter's dash runtime.
    /// </summary>
    public readonly struct MatchAuthorityDashState
    {
        public MatchAuthorityDashState(
            CombatEntityId actorId,
            FighterActionState state,
            float cooldownRemaining,
            Float2 direction)
        {
            ActorId = actorId;
            State = state;
            CooldownRemaining = cooldownRemaining;
            Direction = direction;
        }

        public CombatEntityId ActorId { get; }
        public FighterActionState State { get; }
        public float CooldownRemaining { get; }
        public Float2 Direction { get; }
    }

    /// <summary>
    /// Immutable result of one canonical dash step. Presentation consumes the
    /// collision-resolved position only; it never advances the dash itself.
    /// </summary>
    public readonly struct MatchAuthorityDashStep
    {
        public MatchAuthorityDashStep(
            CombatEntityId actorId,
            int simulationTick,
            bool accepted,
            DashStep step,
            MatchAuthorityDisplacement displacement)
        {
            ActorId = actorId;
            SimulationTick = simulationTick;
            Accepted = accepted;
            Step = step;
            Displacement = displacement;
        }

        public CombatEntityId ActorId { get; }
        public int SimulationTick { get; }
        public bool Accepted { get; }
        public DashStep Step { get; }
        public MatchAuthorityDisplacement Displacement { get; }
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
            : this(
                0,
                result,
                Array.Empty<CombatDamageEvent>(),
                Array.Empty<GadgetHealingIntent>(),
                Array.Empty<int>(),
                Array.Empty<DomainProjectileSnapshot>(),
                Array.Empty<MatchPickupCollectionIntent>(),
                Array.Empty<GadgetPickupCollectionIntent>(),
                Array.Empty<MatchAuthorityDashStep>())
        {
        }

        public MatchAuthorityTick(int simulationTick, MatchTickResult result, DamageRequest[] outsideDamageRequests)
            : this(
                simulationTick,
                result,
                Array.Empty<CombatDamageEvent>(),
                Array.Empty<GadgetHealingIntent>(),
                Array.Empty<int>(),
                Array.Empty<DomainProjectileSnapshot>(),
                Array.Empty<MatchPickupCollectionIntent>(),
                Array.Empty<GadgetPickupCollectionIntent>(),
                Array.Empty<MatchAuthorityDashStep>())
        {
        }

        public MatchAuthorityTick(
            int simulationTick,
            MatchTickResult result,
            DamageRequest[] outsideDamageRequests,
            GadgetHealingIntent[] gadgetHealingIntents,
            int[] expiredStationIds)
            : this(
                simulationTick,
                result,
                ConvertLegacyRequests(outsideDamageRequests),
                gadgetHealingIntents,
                expiredStationIds,
                Array.Empty<DomainProjectileSnapshot>(),
                Array.Empty<MatchPickupCollectionIntent>(),
                Array.Empty<GadgetPickupCollectionIntent>(),
                Array.Empty<MatchAuthorityDashStep>())
        {
        }

        /// <summary>Compatibility shim for pre-atomic callers that only had raw
        /// Aandhi requests; legacy requests are surfaced as unapplied damage
        /// events so old call sites keep compiling without double application.</summary>
        private static CombatDamageEvent[] ConvertLegacyRequests(DamageRequest[] requests)
        {
            if (requests == null || requests.Length == 0) return Array.Empty<CombatDamageEvent>();
            var events = new CombatDamageEvent[requests.Length];
            for (var i = 0; i < requests.Length; i++)
            {
                var request = requests[i];
                events[i] = new CombatDamageEvent(request, 0, false, 0, request.SimulationTick);
            }

            return events;
        }

        public MatchAuthorityTick(
            int simulationTick,
            MatchTickResult result,
            CombatDamageEvent[] damageEvents,
            GadgetHealingIntent[] gadgetHealingIntents,
            int[] expiredStationIds,
            DomainProjectileSnapshot[] projectileSnapshots,
            MatchPickupCollectionIntent[] pickupCollections,
            GadgetPickupCollectionIntent[] gadgetCollections,
            MatchAuthorityDashStep[] bijliDashSteps)
        {
            SimulationTick = simulationTick;
            Result = result;
            DamageEvents = damageEvents ?? Array.Empty<CombatDamageEvent>();
            GadgetHealingIntents = gadgetHealingIntents ?? Array.Empty<GadgetHealingIntent>();
            ExpiredStationIds = expiredStationIds ?? Array.Empty<int>();
            ProjectileSnapshots = projectileSnapshots ?? Array.Empty<DomainProjectileSnapshot>();
            PickupCollections = pickupCollections ?? Array.Empty<MatchPickupCollectionIntent>();
            GadgetCollections = gadgetCollections ?? Array.Empty<GadgetPickupCollectionIntent>();
            BijliDashSteps = bijliDashSteps ?? Array.Empty<MatchAuthorityDashStep>();
        }

        public int SimulationTick { get; }
        public MatchTickResult Result { get; }

        /// <summary>
        /// Authoritative damage already applied inside this tick (Aandhi and any
        /// other authority-resolved sources surfaced per event). Presentation
        /// mirrors these to views; it must never re-apply them.
        /// </summary>
        public CombatDamageEvent[] DamageEvents { get; }

        /// <summary>
        /// Canonical healing already applied inside this tick. Amount is the
        /// applied value; EventId is stable per healing identity stream.
        /// </summary>
        public GadgetHealingIntent[] GadgetHealingIntents { get; }
        public int[] ExpiredStationIds { get; }
        public DomainProjectileSnapshot[] ProjectileSnapshots { get; }

        /// <summary>Health pickups collected atomically within this tick.</summary>
        public MatchPickupCollectionIntent[] PickupCollections { get; }

        /// <summary>Gadget pickups collected atomically within this tick.</summary>
        public GadgetPickupCollectionIntent[] GadgetCollections { get; }

        /// <summary>Canonical dash positions already applied inside this tick.</summary>
        public MatchAuthorityDashStep[] BijliDashSteps { get; }
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
        private readonly Dictionary<CombatEntityId, WeaponCooldownState> _attackCooldowns = new Dictionary<CombatEntityId, WeaponCooldownState>();
        private readonly Dictionary<CombatEntityId, int> _lastAttackTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastAttackSequences = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, float> _spawnProtectionRemaining = new Dictionary<CombatEntityId, float>();
        private readonly List<CombatEntityId> _spawnProtectionIds = new List<CombatEntityId>(8);
        private readonly Dictionary<CombatEntityId, ProjectileWeaponDefinition> _participantWeapons = new Dictionary<CombatEntityId, ProjectileWeaponDefinition>();
        private readonly Dictionary<CombatEntityId, int> _participantTickRates = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _participantCombatGroups = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, ChargeThrowRuntime> _pehelChargeRuntimes = new Dictionary<CombatEntityId, ChargeThrowRuntime>();
        private readonly Dictionary<CombatEntityId, FighterRuntimeState> _bijliDashRuntimes = new Dictionary<CombatEntityId, FighterRuntimeState>();
        private readonly Dictionary<CombatEntityId, int> _lastPehelCommandTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastBijliCommandTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastPehelStepTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastBijliStepTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastPehelThrowTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, CombatFaction> _participantFactions = new Dictionary<CombatEntityId, CombatFaction>();
        private readonly Dictionary<CombatEntityId, DecoyRuntime> _mayaDecoys = new Dictionary<CombatEntityId, DecoyRuntime>();
        private readonly Dictionary<CombatEntityId, int> _lastDecoySpawnTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _lastDecoyDamageTicks = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<CombatEntityId, int> _decoyExecutionIds = new Dictionary<CombatEntityId, int>();
        private readonly Dictionary<int, GadgetStationRuntime> _stations = new Dictionary<int, GadgetStationRuntime>();
        private readonly MatchEventIdentityTracker _identityTracker = new MatchEventIdentityTracker();
        private readonly List<AuthoritativeProjectile> _activeProjectiles = new List<AuthoritativeProjectile>();
        private readonly List<int> _sortedStationIds = new List<int>(8);
        private readonly List<CombatEntityId> _sortedDecoyOwnerIds = new List<CombatEntityId>(8);
        private ArenaCollisionDefinition _collisionDefinition = ArenaCollisionDefinition.BazaarBastion;
        private DeterministicCollisionSolver _collisionSolver;
        private OfflineMatchSimulation _simulation;
        private double _outsideDamageAccumulator;
        private int _lastSimulationTick = -1;
        private int _nextStationId = 1;
        private const int MaxAttackInputLeadTicks = 1;
    private const int MaxAttackInputStalenessTicks = 2;
        private const float MuzzleOffset = 0.7f;

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
        public int CurrentSimulationTick => _lastSimulationTick;
        public MatchPhase CurrentPhase => _simulation != null ? _simulation.Phase : MatchPhase.LoadWarmup;
        public ArenaCollisionDefinition CollisionDefinition => _collisionDefinition;

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

        /// <summary>
        /// Selects the immutable arena collision contract before a match starts.
        /// Unity scene adapters may supply authored obstacles, but the authority
        /// never consumes Unity colliders directly.
        /// </summary>
        public void ConfigureArenaCollision(ArenaCollisionDefinition definition)
        {
            if (_simulation != null)
            {
                throw new InvalidOperationException("Arena collision cannot change after match start.");
            }

            _collisionDefinition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        public void Start(IReadOnlyList<MatchSpawn> spawns)
        {
            _simulation = new OfflineMatchSimulation(_definition);
            _simulation.Start(spawns);
            _collisionSolver = new DeterministicCollisionSolver(_collisionDefinition);
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
            _attackCooldowns.Clear();
            _lastAttackTicks.Clear();
            _lastAttackSequences.Clear();
            _spawnProtectionRemaining.Clear();
            _spawnProtectionIds.Clear();
            _participantWeapons.Clear();
            _participantTickRates.Clear();
            _participantCombatGroups.Clear();
            _pehelChargeRuntimes.Clear();
            _bijliDashRuntimes.Clear();
            _lastPehelCommandTicks.Clear();
            _lastBijliCommandTicks.Clear();
            _lastPehelStepTicks.Clear();
            _lastBijliStepTicks.Clear();
            _lastPehelThrowTicks.Clear();
            _participantFactions.Clear();
            _mayaDecoys.Clear();
            _lastDecoySpawnTicks.Clear();
            _lastDecoyDamageTicks.Clear();
            _decoyExecutionIds.Clear();
            _stations.Clear();
            _identityTracker.Reset();
            _activeProjectiles.Clear();
            for (var i = 0; i < spawns.Count; i++)
            {
                _gadgetInventories[spawns[i].Id] = new GadgetInventory(1);
                _gadgetRuntimes[spawns[i].Id] = new GadgetRuntime();
                _umbrellaGuards[spawns[i].Id] = new UmbrellaGuardRuntime();
                _movementMotors[spawns[i].Id] = new MovementMotor();
                _movementTunings[spawns[i].Id] = MovementTuning.Default;
                _lastMovementTicks[spawns[i].Id] = -1;
                _lastAbilityDisplacementTicks[spawns[i].Id] = -1;
                _attackCooldowns[spawns[i].Id] = new WeaponCooldownState();
                _lastAttackTicks[spawns[i].Id] = -1;
                _lastAttackSequences[spawns[i].Id] = -1;
                _spawnProtectionRemaining[spawns[i].Id] = Math.Max(0f, _definition.SpawnProtectionSeconds);
                _spawnProtectionIds.Add(spawns[i].Id);
                _participantWeapons[spawns[i].Id] = ProjectileWeaponDefinition.TrainingBolt;
                _participantTickRates[spawns[i].Id] = 30;
                _participantCombatGroups[spawns[i].Id] = spawns[i].Id.Value;
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

        /// <summary>
        /// Registers the authority-owned combatant group used for eligibility.
        /// Solo Raja defaults every participant to its own group; a future team
        /// mode can explicitly place multiple participants in one group while
        /// CombatFaction remains a presentation compatibility label.
        /// </summary>
        public void ConfigureCombatGroup(CombatEntityId actorId, int combatGroup)
        {
            if (!HasParticipant(actorId))
            {
                throw new ArgumentException("Combat group can only be configured for a registered match participant.", nameof(actorId));
            }

            if (combatGroup <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(combatGroup), "Combat groups must be positive.");
            }

            _participantCombatGroups[actorId] = combatGroup;
        }

        private bool AreDifferentCombatGroups(CombatEntityId first, CombatEntityId second)
        {
            return _participantCombatGroups.TryGetValue(first, out var firstGroup) &&
                _participantCombatGroups.TryGetValue(second, out var secondGroup) &&
                firstGroup != secondGroup;
        }

        /// <summary>
        /// Exposes the already-authoritative relationship for presentation-side
        /// perception. This is a read-only query; it cannot alter combat state.
        /// </summary>
        public bool AreActorsHostile(CombatEntityId first, CombatEntityId second)
        {
            return first != second && AreDifferentCombatGroups(first, second);
        }

        private void RefreshSortedAuthorityTargets()
        {
            _sortedStationIds.Clear();
            _sortedStationIds.AddRange(_stations.Keys);
            _sortedStationIds.Sort((left, right) => left.CompareTo(right));

            _sortedDecoyOwnerIds.Clear();
            _sortedDecoyOwnerIds.AddRange(_mayaDecoys.Keys);
            _sortedDecoyOwnerIds.Sort((left, right) => left.Value.CompareTo(right.Value));
        }

        /// <summary>
        /// Registers the immutable weapon configuration for a participant at match
        /// setup. Runtime attack commands never supply or select their own weapon,
        /// faction, cooldown or tick rate.
        /// </summary>
        public void ConfigureWeapon(CombatEntityId actorId, ProjectileWeaponDefinition weapon, int tickRate)
        {
            if (!HasParticipant(actorId))
            {
                throw new ArgumentException("Weapon configuration requires a registered match participant.", nameof(actorId));
            }

            if (tickRate <= 0 || !weapon.IsValid(out _))
            {
                throw new ArgumentOutOfRangeException(nameof(weapon), "Weapon configuration must be valid and use a positive tick rate.");
            }

            _participantWeapons[actorId] = weapon;
            _participantTickRates[actorId] = tickRate;
        }

        public bool SetPosition(CombatEntityId id, Float2 position) => RequireSimulation().SetPosition(id, position);

        public bool IsSpawnProtected(CombatEntityId actorId)
        {
            return _spawnProtectionRemaining.TryGetValue(actorId, out var remaining) && remaining > 0.0001f;
        }

        public void ClearSpawnProtection(CombatEntityId actorId)
        {
            if (_spawnProtectionRemaining.ContainsKey(actorId)) _spawnProtectionRemaining[actorId] = 0f;
        }

        public bool RespawnParticipant(CombatEntityId actorId, Float2 position)
        {
            var respawned = RequireSimulation().Respawn(actorId, position);
            if (respawned)
            {
                _spawnProtectionRemaining[actorId] = Math.Max(0f, _definition.SpawnProtectionSeconds);
            }

            return respawned;
        }

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
                !command.Movement.IsFinite ||
                !command.Aim.IsFinite ||
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
            if (!GetActionEligibility(actorId).IsEligible)
            {
                return new MatchAuthorityMovement(actorId, command.SimulationTick, false, default(MovementStep), current.Position);
            }

            var collision = _collisionSolver.Move(current.Position, step.Displacement);
            var position = collision.Position;
            var appliedStep = new MovementStep(step.Velocity, collision.AppliedDisplacement, step.AimDirection);
            if (!simulation.SetPosition(actorId, position))
            {
                return new MatchAuthorityMovement(actorId, command.SimulationTick, false, appliedStep, current.Position);
            }

            _lastMovementTicks[actorId] = command.SimulationTick;
            return new MatchAuthorityMovement(actorId, command.SimulationTick, true, appliedStep, position);
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

            if (!GetActionEligibility(actorId).IsEligible)
            {
                return new MatchAuthorityDisplacement(actorId, simulationTick, false, Float2.Zero, current.Position);
            }

            var collision = _collisionSolver.Move(current.Position, displacement);
            var position = collision.Position;
            if (!simulation.SetPosition(actorId, position))
            {
                return new MatchAuthorityDisplacement(actorId, simulationTick, false, Float2.Zero, current.Position);
            }

            _lastAbilityDisplacementTicks[actorId] = simulationTick;
            return new MatchAuthorityDisplacement(actorId, simulationTick, true, collision.AppliedDisplacement, position);
        }

        /// <summary>
        /// Validates and consumes one fixed-tick attack command for a registered
        /// participant. Projectile spawning remains a presentation concern, but
        /// command ordering, alive-state validation and cooldown ownership stay in
        /// the transport-independent authority.
        /// </summary>
        public MatchAuthorityAttack TryAcceptAttack(AttackCommand command)
        {
            var actorId = command.InstigatorId;
            if (!command.Pressed || command.SimulationTick < 0 || command.InputSequence < 0 ||
                !command.Origin.IsFinite || !command.Direction.IsFinite ||
                command.Direction.SqrMagnitude <= 0.000001f)
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.InvalidCommand,
                    0);
            }

            if (!_attackCooldowns.TryGetValue(actorId, out var cooldown) ||
                !_participantWeapons.TryGetValue(actorId, out var definition) ||
                !_participantTickRates.TryGetValue(actorId, out var tickRate) ||
                !RequireSimulation().TryGetSnapshot(actorId, out var snapshot))
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.UnknownActor,
                    0);
            }

            if (!definition.IsValid(out _))
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.InvalidLoadout,
                    0);
            }

            var phaseEligibility = GetActionEligibility(actorId);
            if (!phaseEligibility.IsEligible)
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    ToAttackFailure(phaseEligibility.Failure),
                    0);
            }

            var latestAllowedTick = checked(_lastSimulationTick + 1 + MaxAttackInputLeadTicks);
            if (command.SimulationTick > latestAllowedTick)
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.FutureTick,
                    cooldown.RemainingTicks(_lastSimulationTick));
            }

            if (command.SimulationTick < _lastSimulationTick - MaxAttackInputStalenessTicks)
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.StaleTick,
                    cooldown.RemainingTicks(_lastSimulationTick));
            }

            if (!snapshot.Alive)
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.DefeatedActor,
                    cooldown.RemainingTicks(_lastSimulationTick));
            }

            if (_lastAttackTicks.TryGetValue(actorId, out var lastTick) && command.SimulationTick <= lastTick)
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.OutOfOrder,
                    cooldown.RemainingTicks(_lastSimulationTick));
            }

            if (_lastAttackSequences.TryGetValue(actorId, out var lastSequence) && command.InputSequence <= lastSequence)
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.OutOfOrder,
                    cooldown.RemainingTicks(_lastSimulationTick));
            }

            // Cooldowns are anchored to the authority clock, never to a
            // caller-supplied tick, so stale commands cannot compress fire rate.
            var cooldownAnchor = Math.Max(command.SimulationTick, _lastSimulationTick);
            _lastAttackTicks[actorId] = cooldownAnchor;
            _lastAttackSequences[actorId] = command.InputSequence;
            var intervalTicks = Math.Max(1, (int)Math.Ceiling(definition.FireIntervalSeconds * tickRate));
            if (!cooldown.TryConsume(cooldownAnchor, intervalTicks))
            {
                return new MatchAuthorityAttack(
                    actorId,
                    command.SimulationTick,
                    false,
                    MatchAuthorityAttackFailure.Cooldown,
                    cooldown.RemainingTicks(cooldownAnchor));
            }

            var canonicalDirection = command.Direction.Normalized;
            var canonicalOrigin = snapshot.Position + canonicalDirection * MuzzleOffset;
            var faction = _participantFactions.TryGetValue(actorId, out var configuredFaction)
                ? configuredFaction
                : CombatFaction.Neutral;

            var attackExecutionId = _identityTracker.NextAttackExecutionId();
            var projectileId = _identityTracker.NextProjectileId();
            var proj = new AuthoritativeProjectile(
                projectileId,
                attackExecutionId,
                actorId,
                definition.WeaponId,
                command.SimulationTick,
                canonicalOrigin,
                canonicalDirection,
                definition.ProjectileSpeed,
                definition.Radius,
                definition.MaxRange,
                definition.LifetimeSeconds,
                faction);
            _activeProjectiles.Add(proj);

            return new MatchAuthorityAttack(
                actorId,
                command.SimulationTick,
                true,
                MatchAuthorityAttackFailure.None,
                cooldown.RemainingTicks(cooldownAnchor),
                definition,
                faction,
                canonicalOrigin,
                canonicalDirection,
                projectileId,
                attackExecutionId);
        }

        /// <summary>
        /// Compatibility overload for old presentation/tests. The supplied weapon
        /// and tick rate are deliberately ignored; match configuration is authoritative.
        /// </summary>
        public MatchAuthorityAttack TryAcceptAttack(
            AttackCommand command,
            ProjectileWeaponDefinition ignoredDefinition,
            int ignoredTickRate)
        {
            return TryAcceptAttack(command);
        }

        public float GetAttackCooldownRemaining(CombatEntityId actorId, int tickRate, int currentTick)
        {
            if (!_attackCooldowns.TryGetValue(actorId, out var cooldown) ||
                !_participantTickRates.TryGetValue(actorId, out var configuredTickRate)) return 0f;
            return cooldown.RemainingSeconds(currentTick, configuredTickRate);
        }

        /// <summary>
        /// Starts Pehel's charge in the authority-owned runtime. The
        /// presentation controller submits only the common command and local
        /// input context; cooldown and state validation stay here.
        /// </summary>
        public MatchAuthorityAbilityStart TryStartPehelCharge(AbilityCommand command, Float2 movement, Float2 facing)
        {
            var rejected = MatchAuthorityAbilityStart.Rejected(
                command.InstigatorId,
                FighterSpecialDefinition.PehelChargeThrow.AbilityId,
                command.SimulationTick);
            if (!command.Pressed || !command.AbilityId.Equals(FighterSpecialDefinition.PehelChargeThrow.AbilityId) ||
                command.SimulationTick < 0 ||
                (_lastPehelCommandTicks.TryGetValue(command.InstigatorId, out var lastTick) && command.SimulationTick <= lastTick) ||
                command.SimulationTick <= CurrentSimulationTick ||
                !GetActionEligibility(command.InstigatorId).IsEligible ||
                !RequireSimulation().TryGetSnapshot(command.InstigatorId, out var snapshot) ||
                !snapshot.Alive)
            {
                return rejected;
            }

            if (!_pehelChargeRuntimes.TryGetValue(command.InstigatorId, out var runtime))
            {
                runtime = new ChargeThrowRuntime(FighterSpecialDefinition.PehelChargeThrow);
                _pehelChargeRuntimes[command.InstigatorId] = runtime;
            }

            if (!runtime.TryStart(command, movement, facing)) return rejected;
            _lastPehelCommandTicks[command.InstigatorId] = command.SimulationTick;
            return new MatchAuthorityAbilityStart(
                command.InstigatorId,
                command.AbilityId,
                command.SimulationTick,
                true,
                _identityTracker.NextAbilityExecutionId());
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
                var collision = _collisionSolver.Move(actorSnapshot.Position, step.Displacement);
                var position = collision.Position;
                if (RequireSimulation().SetPosition(actorId, position))
                {
                    actorDisplacement = new MatchAuthorityDisplacement(
                        actorId,
                        simulationTick,
                        true,
                        collision.AppliedDisplacement,
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

        /// <summary>
        /// Starts Bijli's dash in an authority-owned runtime. Presentation submits
        /// only the common ability command; eligibility, cooldown and direction
        /// selection remain canonical.
        /// </summary>
        public MatchAuthorityAbilityStart TryStartBijliDash(AbilityCommand command, Float2 movement, Float2 facing)
        {
            var rejected = MatchAuthorityAbilityStart.Rejected(
                command.InstigatorId,
                FighterDefinition.Bijli.Ability.AbilityId,
                command.SimulationTick);
            if (!command.Pressed ||
                !command.AbilityId.Equals(FighterDefinition.Bijli.Ability.AbilityId) ||
                command.SimulationTick < 0 ||
                (_lastBijliCommandTicks.TryGetValue(command.InstigatorId, out var lastTick) &&
                    command.SimulationTick <= lastTick) ||
                command.SimulationTick <= CurrentSimulationTick ||
                !GetActionEligibility(command.InstigatorId).IsEligible ||
                !RequireSimulation().TryGetSnapshot(command.InstigatorId, out var snapshot) ||
                !snapshot.Alive)
            {
                return rejected;
            }

            if (!_bijliDashRuntimes.TryGetValue(command.InstigatorId, out var runtime))
            {
                runtime = new FighterRuntimeState(FighterDefinition.Bijli);
                _bijliDashRuntimes[command.InstigatorId] = runtime;
            }

            if (!runtime.TryStartDash(command, movement, facing)) return rejected;
            _lastBijliCommandTicks[command.InstigatorId] = command.SimulationTick;
            return new MatchAuthorityAbilityStart(
                command.InstigatorId,
                command.AbilityId,
                command.SimulationTick,
                true,
                _identityTracker.NextAbilityExecutionId());
        }

        public MatchAuthorityDashState GetBijliDashState(CombatEntityId actorId)
        {
            return !_bijliDashRuntimes.TryGetValue(actorId, out var runtime)
                ? new MatchAuthorityDashState(actorId, FighterActionState.Ready, 0f, Float2.Up)
                : new MatchAuthorityDashState(
                    actorId,
                    runtime.ActionState,
                    runtime.CooldownRemaining,
                    runtime.DashDirection);
        }

        public bool IsAuthorityMovementLocked(CombatEntityId actorId)
        {
            if (_pehelChargeRuntimes.TryGetValue(actorId, out var charge))
            {
                return charge.State != ChargeThrowState.Ready && charge.State != ChargeThrowState.Cooldown;
            }

            if (!_bijliDashRuntimes.TryGetValue(actorId, out var dash)) return false;
            return dash.ActionState != FighterActionState.Ready &&
                dash.ActionState != FighterActionState.Cooldown;
        }

        /// <summary>
        /// Advances one fixed dash step against canonical position and arena
        /// collision. The authority solver is the sole collision decision maker.
        /// </summary>
        public MatchAuthorityDashStep AdvanceBijliDash(CombatEntityId actorId, int simulationTick, float fixedDeltaSeconds)
        {
            var runtime = _bijliDashRuntimes.TryGetValue(actorId, out var found) ? found : null;
            var emptyStep = default(DashStep);
            if (runtime != null)
            {
                emptyStep = new DashStep(runtime.ActionState, Float2.Zero, false, false);
            }

            var displacement = new MatchAuthorityDisplacement(
                actorId,
                simulationTick,
                false,
                Float2.Zero,
                RequireSimulation().TryGetSnapshot(actorId, out var snapshot)
                    ? snapshot.Position
                    : Float2.Zero);
            if (runtime == null || simulationTick < 0 ||
                !_lastBijliCommandTicks.TryGetValue(actorId, out var lastCommandTick) ||
                simulationTick <= lastCommandTick ||
                (_lastBijliStepTicks.TryGetValue(actorId, out var lastStepTick) && simulationTick <= lastStepTick) ||
                !RequireSimulation().TryGetSnapshot(actorId, out snapshot) ||
                !snapshot.Alive)
            {
                return new MatchAuthorityDashStep(
                    actorId,
                    simulationTick,
                    false,
                    emptyStep,
                    displacement);
            }

            var step = runtime.Step(fixedDeltaSeconds, FighterDefinition.Bijli.Ability.Distance);
            _lastBijliStepTicks[actorId] = simulationTick;
            if (step.Displacement.SqrMagnitude > 0.000001f)
            {
                var collision = _collisionSolver.Move(snapshot.Position, step.Displacement);
                if (RequireSimulation().SetPosition(actorId, collision.Position))
                {
                    displacement = new MatchAuthorityDisplacement(
                        actorId,
                        simulationTick,
                        true,
                        collision.AppliedDisplacement,
                        collision.Position);
                }
            }

            return new MatchAuthorityDashStep(
                actorId,
                simulationTick,
                true,
                step,
                displacement);
        }

        private bool TryCaptureNearestPehelTarget(
            CombatEntityId actorId,
            ChargeThrowRuntime runtime)
        {
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
                    !AreDifferentCombatGroups(actorId, candidate.Id))
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

            if (!found) return false;
            return runtime.TryCaptureTarget(best.Id, true, bestDistance);
        }

        private MatchAuthorityDashStep[] AdvanceBijliDashes(int simulationTick, float fixedDeltaSeconds)
        {
            if (_bijliDashRuntimes.Count == 0) return Array.Empty<MatchAuthorityDashStep>();

            var snapshots = RequireSimulation().GetSnapshots();
            var steps = default(List<MatchAuthorityDashStep>);
            for (var i = 0; i < snapshots.Length; i++)
            {
                if (!_bijliDashRuntimes.ContainsKey(snapshots[i].Id)) continue;
                var step = AdvanceBijliDash(snapshots[i].Id, simulationTick, fixedDeltaSeconds);
                if (steps == null) steps = new List<MatchAuthorityDashStep>(snapshots.Length);
                steps.Add(step);
            }

            return steps != null ? steps.ToArray() : Array.Empty<MatchAuthorityDashStep>();
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
            var authorityDamage = ResolveDamage(
                request,
                targetFaction,
                false,
                AreDifferentCombatGroups(actorId, targetId));
            _lastPehelThrowTicks[actorId] = simulationTick;
            var after = simulation.TryGetSnapshot(targetId, out var afterSnapshot)
                ? afterSnapshot
                : target;
            damage = new MatchAuthorityDamage(
                authorityDamage.Request,
                authorityDamage.Result,
                after.CurrentHealth);
            if (!authorityDamage.Result.Applied) return false;

            var displacement = direction.Normalized * (FighterSpecialDefinition.PehelChargeThrow.Magnitude * 0.25f);
            var collision = _collisionSolver.Move(after.Position, displacement);
            var position = collision.Position;
            if (!simulation.SetPosition(targetId, position)) return false;
            targetDisplacement = new MatchAuthorityDisplacement(
                targetId,
                simulationTick,
                true,
                collision.AppliedDisplacement,
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
                !GetActionEligibility(ownerId).IsEligible ||
                !simulation.TryGetSnapshot(ownerId, out var owner) || !owner.Alive)
            {
                return GetMayaDecoySnapshot(ownerId);
            }

            if (!_mayaDecoys.TryGetValue(ownerId, out var decoy))
            {
                decoy = new DecoyRuntime();
                _mayaDecoys[ownerId] = decoy;
            }

            // The owner snapshot is canonical. The presentation-supplied position
            // is an intent marker and cannot move a decoy remotely.
            var canonicalPosition = _collisionSolver.Move(owner.Position, Float2.Zero).Position;
            if (!decoy.TrySpawn(ownerId, canonicalPosition, FighterSpecialDefinition.MayaDecoy))
            {
                return GetMayaDecoySnapshot(ownerId);
            }

            _lastDecoySpawnTicks[ownerId] = simulationTick;
            _decoyExecutionIds[ownerId] = _identityTracker.NextAbilityExecutionId();
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
            if (!TryFindMayaDecoy(request.TargetId, out var ownerId, out var decoy))
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, false, DamageRejectionReason.WrongTarget),
                    0);
            }

            _participantFactions.TryGetValue(ownerId, out var ownedTargetFaction);
            if (!IsCombatActionPhase(CurrentPhase))
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, true, DamageRejectionReason.AlreadyDefeated),
                    decoy.CurrentHealth);
            }

            var hostileToOwner = AreDifferentCombatGroups(request.InstigatorId, ownerId);
            targetFaction = ownedTargetFaction;
            allowFriendlyFire = hostileToOwner;

            if (request.RawAmount <= 0)
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, decoy.CurrentHealth <= 0, DamageRejectionReason.InvalidAmount),
                    decoy.CurrentHealth);
            }

            if (!allowSelfHit && (request.InstigatorId == request.TargetId || request.InstigatorId == ownerId))
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

        public int ApplyHealing(CombatEntityId id, int amount) =>
            GetActionEligibility(id).IsEligible ? RequireSimulation().Heal(id, amount) : 0;

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
            var targetEligibility = GetActionEligibility(request.TargetId);
            if (!targetEligibility.IsEligible)
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(
                        false,
                        0,
                        false,
                        targetEligibility.Failure == MatchAuthorityActionPhaseFailure.UnknownActor
                            ? DamageRejectionReason.WrongTarget
                            : DamageRejectionReason.AlreadyDefeated),
                    0);
            }

            if (IsSpawnProtected(request.TargetId))
            {
                return new MatchAuthorityDamage(
                    request,
                    new DamageResult(false, 0, false, DamageRejectionReason.SpawnProtection),
                    targetEligibility.Snapshot.CurrentHealth);
            }

            var mitigated = ApplyDamageMitigation(request);
            var result = RequireSimulation().ApplyDamage(mitigated, targetFaction, allowSelfHit, allowFriendlyFire);
            if (result.Applied && request.InstigatorId.Value > 0)
            {
                ClearSpawnProtection(request.InstigatorId);
            }
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
            var projectileDamageEvents = new List<CombatDamageEvent>();
            var projectileSnapshots = AdvanceProjectiles(
                simulationTick,
                fixedDeltaSeconds,
                simulation,
                projectileDamageEvents);
            var dashSteps = AdvanceBijliDashes(simulationTick, fixedDeltaSeconds);
            var result = simulation.Advance(fixedDeltaSeconds);
            // Bastion Crown's ready countdown is presentation-only. Its spawn
            // shield starts at the live boundary, while Solo retains the legacy
            // warmup countdown semantics.
            if (_spawnProtectionRemaining.Count > 0 &&
                (!_definition.IsTeamMode || result.Phase >= MatchPhase.Opening))
            {
                for (var protectionIndex = 0; protectionIndex < _spawnProtectionIds.Count; protectionIndex++)
                {
                    var protectionId = _spawnProtectionIds[protectionIndex];
                    var remaining = Math.Max(0f, _spawnProtectionRemaining[protectionId] - fixedDeltaSeconds);
                    _spawnProtectionRemaining[protectionId] = remaining;
                }
            }
            var tickSnapshots = simulation.GetSnapshots();
            var healingIntents = new List<GadgetHealingIntent>();
            var expiredStationIds = new List<int>();
            AdvanceStations(
                fixedDeltaSeconds,
                result.Phase,
                tickSnapshots,
                healingIntents,
                expiredStationIds);
            var damageEvents = new List<CombatDamageEvent>(
                ApplyOutsideZoneDamage(simulationTick, fixedDeltaSeconds, result, tickSnapshots));
            if (projectileDamageEvents.Count > 0) damageEvents.AddRange(projectileDamageEvents);
            var pickupCollections = new List<MatchPickupCollectionIntent>(_pickups.Length);
            var gadgetCollections = new List<GadgetPickupCollectionIntent>(_gadgetPickups.Length);
            ResolveItemCollections(tickSnapshots, pickupCollections, gadgetCollections);

            return new MatchAuthorityTick(
                simulationTick,
                result,
                damageEvents.ToArray(),
                healingIntents.ToArray(),
                expiredStationIds.ToArray(),
                projectileSnapshots,
                pickupCollections.ToArray(),
                gadgetCollections.ToArray(),
                dashSteps);
        }

        /// <summary>
        /// Builds a canonical post-tick digest from authority-owned mutable
        /// state. The replay hasher delegates here because these fields are the
        /// authority's private invariants and must not be exposed mutably.
        /// </summary>
        public ulong CalculateDeterministicTickHash(
            MatchAuthorityTick tick,
            MatchParticipantSnapshot[] snapshots)
        {
            var hash = MatchStateHashBuilder.Create();
            var result = tick.Result;
            hash.CombineULong(_collisionDefinition.CalculateStableHash());
            hash.CombineInt(tick.SimulationTick);
            hash.CombineInt((int)result.Phase);
            hash.CombineFloat(result.ZoneCenter.X);
            hash.CombineFloat(result.ZoneCenter.Y);
            hash.CombineFloat(result.NextZoneCenter.X);
            hash.CombineFloat(result.NextZoneCenter.Y);
            hash.CombineFloat(result.ZoneRadius);
            hash.CombineFloat(result.NextZoneRadius);
            hash.CombineInt((int)result.AandhiState);
            hash.CombineFloat(result.WarningRemainingSeconds);
            hash.CombineInt(result.OutsideDamagePerSecond);
            hash.CombineInt(result.OutsideCount);
            hash.CombineBool(result.MatchEnded);
            hash.CombineInt(result.WinnerId.Value);

            var counters = _identityTracker.Snapshot();
            var simulation = RequireSimulation();
            hash.CombineInt(simulation.LastDamageEventId);
            hash.CombineInt(simulation.EmittedDamageEventCount);
            hash.CombineInt(_nextStationId);
            var damageContributions = simulation.GetDamageContributions();
            for (var i = 0; i < damageContributions.Length; i++)
            {
                hash.CombineInt(damageContributions[i].TargetId.Value);
                hash.CombineInt(damageContributions[i].InstigatorId.Value);
                hash.CombineInt(damageContributions[i].Amount);
            }

            hash.CombineInt(counters.AttackExecutionId);
            hash.CombineInt(counters.ProjectileId);
            hash.CombineInt(counters.AbilityExecutionId);
            hash.CombineInt(counters.GadgetUseId);
            hash.CombineInt(counters.DamageEventId);
            hash.CombineInt(counters.HealingEventId);
            hash.CombineInt(counters.CollectionEventId);
            hash.CombineInt(counters.EliminationEventId);
            hash.CombineFloat((float)_outsideDamageAccumulator);

            snapshots = snapshots ?? RequireSimulation().GetSnapshots();
            for (var i = 0; i < snapshots.Length; i++)
            {
                var s = snapshots[i];
                hash.CombineInt(s.Id.Value);
                hash.CombineFloat(s.Position.X);
                hash.CombineFloat(s.Position.Y);
                hash.CombineInt(s.CurrentHealth);
                hash.CombineInt(s.MaxHealth);
                hash.CombineBool(s.Alive);
                hash.CombineInt(s.Placement);
                hash.CombineInt(s.Eliminations);
                hash.CombineInt(s.DamageDealt);
                hash.CombineInt(s.Assists);
                hash.CombineFloat(s.SurvivalTimeSeconds);

                _participantWeapons.TryGetValue(s.Id, out var weapon);
                hash.CombineContentId(weapon.WeaponId);
                hash.CombineInt(weapon.Damage);
                hash.CombineFloat(weapon.FireIntervalSeconds);
                hash.CombineFloat(weapon.ProjectileSpeed);
                hash.CombineFloat(weapon.MaxRange);
                hash.CombineFloat(weapon.LifetimeSeconds);
                hash.CombineFloat(weapon.Radius);
                hash.CombineInt(weapon.CollisionLayerMask);
                hash.CombineBool(weapon.AllowSelfHit);
                hash.CombineBool(weapon.AllowFriendlyFire);
                _participantTickRates.TryGetValue(s.Id, out var tickRate);
                hash.CombineInt(tickRate);
                _participantFactions.TryGetValue(s.Id, out var faction);
                hash.CombineInt((int)faction);
                _participantCombatGroups.TryGetValue(s.Id, out var combatGroup);
                hash.CombineInt(combatGroup);
                _spawnProtectionRemaining.TryGetValue(s.Id, out var spawnProtection);
                hash.CombineFloat(spawnProtection);
                _movementTunings.TryGetValue(s.Id, out var tuning);
                hash.CombineFloat(tuning.MaxSpeed);
                hash.CombineFloat(tuning.Acceleration);
                hash.CombineFloat(tuning.Deceleration);
                hash.CombineFloat(tuning.RotationSpeed);
                hash.CombineFloat(tuning.MovementDeadZone);
                hash.CombineFloat(tuning.AimDeadZone);
                hash.CombineFloat(tuning.InputSensitivity);
                _movementMotors.TryGetValue(s.Id, out var motor);
                hash.CombineFloat(motor.Velocity.X);
                hash.CombineFloat(motor.Velocity.Y);
                hash.CombineFloat(motor.AimDirection.X);
                hash.CombineFloat(motor.AimDirection.Y);
                _lastMovementTicks.TryGetValue(s.Id, out var lastMovementTick);
                hash.CombineInt(lastMovementTick);
                _lastAbilityDisplacementTicks.TryGetValue(s.Id, out var lastDisplacementTick);
                hash.CombineInt(lastDisplacementTick);
                _attackCooldowns.TryGetValue(s.Id, out var attackCooldown);
                hash.CombineInt(attackCooldown.RemainingTicks(tick.SimulationTick));
                _lastAttackTicks.TryGetValue(s.Id, out var lastAttackTick);
                hash.CombineInt(lastAttackTick);
                _lastAttackSequences.TryGetValue(s.Id, out var lastAttackSequence);
                hash.CombineInt(lastAttackSequence);

                _gadgetInventories.TryGetValue(s.Id, out var inventory);
                hash.CombineContentId(inventory.HeldGadget);
                _gadgetRuntimes.TryGetValue(s.Id, out var gadgetRuntime);
                hash.CombineFloat(gadgetRuntime.CooldownRemaining);
                _umbrellaGuards.TryGetValue(s.Id, out var umbrella);
                hash.CombineBool(umbrella.IsActive);
                hash.CombineFloat(umbrella.RemainingSeconds);
                hash.CombineFloat(umbrella.Direction.X);
                hash.CombineFloat(umbrella.Direction.Y);

                _pehelChargeRuntimes.TryGetValue(s.Id, out var charge);
                hash.CombineInt(charge == null ? 0 : (int)charge.State);
                if (charge != null)
                {
                    hash.CombineFloat(charge.Direction.X);
                    hash.CombineFloat(charge.Direction.Y);
                    hash.CombineFloat(charge.PhaseRemaining);
                    hash.CombineFloat(charge.CooldownRemaining);
                    hash.CombineInt(charge.CapturedTargetId.Value);
                    hash.CombineBool(charge.HasCapturedTarget);
                }

                _mayaDecoys.TryGetValue(s.Id, out var decoy);
                if (decoy != null)
                {
                    hash.CombineBool(decoy.IsActive);
                    hash.CombineBool(decoy.IsTargetable);
                    hash.CombineFloat(decoy.Position.X);
                    hash.CombineFloat(decoy.Position.Y);
                    hash.CombineInt(decoy.CurrentHealth);
                    hash.CombineInt(decoy.MaxHealth);
                    hash.CombineFloat(decoy.RemainingSeconds);
                    hash.CombineFloat(decoy.CooldownRemaining);
                    _decoyExecutionIds.TryGetValue(s.Id, out var decoyExecutionId);
                    hash.CombineInt(decoyExecutionId);
                }

                _lastPehelCommandTicks.TryGetValue(s.Id, out var lastPehelCommand);
                hash.CombineInt(lastPehelCommand);
                _lastPehelStepTicks.TryGetValue(s.Id, out var lastPehelStep);
                hash.CombineInt(lastPehelStep);
                _lastPehelThrowTicks.TryGetValue(s.Id, out var lastPehelThrow);
                hash.CombineInt(lastPehelThrow);
                _bijliDashRuntimes.TryGetValue(s.Id, out var dash);
                hash.CombineInt(dash == null ? 0 : (int)dash.ActionState);
                if (dash != null)
                {
                    hash.CombineFloat(dash.DashDirection.X);
                    hash.CombineFloat(dash.DashDirection.Y);
                    hash.CombineFloat(dash.CooldownRemaining);
                    hash.CombineFloat(dash.DistanceTravelled);
                }

                _lastBijliCommandTicks.TryGetValue(s.Id, out var lastBijliCommand);
                hash.CombineInt(lastBijliCommand);
                _lastBijliStepTicks.TryGetValue(s.Id, out var lastBijliStep);
                hash.CombineInt(lastBijliStep);
                _lastDecoySpawnTicks.TryGetValue(s.Id, out var lastDecoySpawn);
                hash.CombineInt(lastDecoySpawn);
                _lastDecoyDamageTicks.TryGetValue(GetDecoyId(s.Id), out var lastDecoyDamage);
                hash.CombineInt(lastDecoyDamage);
            }

            for (var i = 0; i < _pickups.Length; i++)
            {
                var pickup = _pickups[i];
                hash.CombineInt(pickup.Definition.PickupId);
                hash.CombineInt((int)pickup.Definition.Kind);
                hash.CombineInt(pickup.Definition.Value);
                hash.CombineFloat(pickup.Definition.RespawnSeconds);
                hash.CombineFloat(pickup.Definition.Position.X);
                hash.CombineFloat(pickup.Definition.Position.Y);
                hash.CombineFloat(pickup.Definition.CollectionRadius);
                hash.CombineBool(pickup.IsAvailable);
                hash.CombineFloat(pickup.RespawnRemaining);
            }

            for (var i = 0; i < _gadgetPickups.Length; i++)
            {
                var pickup = _gadgetPickups[i];
                hash.CombineInt(pickup.Definition.PickupId);
                hash.CombineContentId(pickup.Definition.GadgetId);
                hash.CombineFloat(pickup.Definition.Position.X);
                hash.CombineFloat(pickup.Definition.Position.Y);
                hash.CombineFloat(pickup.Definition.CollectionRadius);
                hash.CombineBool(pickup.IsAvailable);
            }

            RefreshSortedAuthorityTargets();
            for (var stationIndex = 0; stationIndex < _sortedStationIds.Count; stationIndex++)
            {
                var station = _stations[_sortedStationIds[stationIndex]];
                hash.CombineInt(station.StationId);
                hash.CombineInt(station.OwnerId.Value);
                hash.CombineFloat(station.Position.X);
                hash.CombineFloat(station.Position.Y);
                hash.CombineFloat(station.RemainingSeconds);
                hash.CombineFloat(station.HealAccumulator);
                hash.CombineInt(station.CurrentHealth);
                hash.CombineBool(station.IsActive);
            }

            var projectileSnapshots = tick.ProjectileSnapshots;
            for (var i = 0; i < projectileSnapshots.Length; i++)
            {
                var p = projectileSnapshots[i];
                hash.CombineInt(p.ProjectileId);
                hash.CombineInt(p.AttackExecutionId);
                hash.CombineInt(p.InstigatorId.Value);
                hash.CombineContentId(p.WeaponId);
                hash.CombineInt(p.SpawnTick);
                hash.CombineFloat(p.Position.X);
                hash.CombineFloat(p.Position.Y);
                hash.CombineFloat(p.Direction.X);
                hash.CombineFloat(p.Direction.Y);
                hash.CombineFloat(p.Speed);
                hash.CombineFloat(p.Radius);
                hash.CombineFloat(p.RemainingRange);
                hash.CombineFloat(p.RemainingLifetime);
                hash.CombineInt((int)p.Faction);
                hash.CombineBool(p.IsActive);
                hash.CombineInt((int)p.DespawnReason);
                hash.CombineInt(p.HitTargetId.Value);
            }

            return hash.Value;
        }

        /// <summary>
        /// Applies accumulated Aandhi exposure inside the canonical tick and
        /// emits immutable applied-damage events. Presentation never feeds zone
        /// damage back into Core.
        /// </summary>
        private CombatDamageEvent[] ApplyOutsideZoneDamage(
            int simulationTick,
            float fixedDeltaSeconds,
            MatchTickResult result,
            MatchParticipantSnapshot[] snapshots)
        {
            if (result.OutsideCount <= 0 || result.OutsideDamagePerSecond <= 0)
            {
                _outsideDamageAccumulator = 0d;
                return Array.Empty<CombatDamageEvent>();
            }

            if (!IsCombatActionPhase(result.Phase))
            {
                _outsideDamageAccumulator = 0d;
                return Array.Empty<CombatDamageEvent>();
            }

            _outsideDamageAccumulator += fixedDeltaSeconds;
            if (_outsideDamageAccumulator < _outsideDamageTickSeconds)
            {
                return Array.Empty<CombatDamageEvent>();
            }

            var events = new List<CombatDamageEvent>(result.OutsideCount);
            while (_outsideDamageAccumulator >= _outsideDamageTickSeconds)
            {
                _outsideDamageAccumulator -= _outsideDamageTickSeconds;
                for (var i = 0; i < snapshots.Length; i++)
                {
                    var snapshot = snapshots[i];
                    if (!snapshot.Alive || Float2.Distance(snapshot.Position, result.ZoneCenter) <= result.ZoneRadius) continue;
                    var request = new DamageRequest(
                        new CombatEntityId(-99),
                        snapshot.Id,
                        CombatFaction.Neutral,
                        result.OutsideDamagePerSecond,
                        DamageType.Aandhi,
                        Float2.Zero,
                        simulationTick);
                    _participantFactions.TryGetValue(snapshot.Id, out var targetFaction);
                    var resolved = ResolveDamage(request, targetFaction, allowSelfHit: true, allowFriendlyFire: true);
                    if (!resolved.Result.Applied) continue;
                    events.Add(new CombatDamageEvent(
                        resolved.Request,
                        resolved.Result.AmountApplied,
                        resolved.Result.TargetDefeated,
                        resolved.CurrentHealthAfter,
                        simulationTick,
                        RequireSimulation().LastDamageEventId));
                }
            }

            return events.ToArray();
        }

        /// <summary>
        /// Resolves pickup proximity, collector selection, canonical healing and
        /// gadget inventory mutation atomically within the caller's tick.
        /// </summary>
        private void ResolveItemCollections(
            MatchParticipantSnapshot[] snapshots,
            List<MatchPickupCollectionIntent> pickupCollections,
            List<GadgetPickupCollectionIntent> gadgetCollections)
        {
            for (var i = 0; i < _pickups.Length; i++)
            {
                var runtime = _pickups[i];
                if (!runtime.IsAvailable) continue;
                var definition = runtime.Definition;
                if (!TrySelectCollector(snapshots, definition.Position, definition.CollectionRadius, true, out var collector)) continue;
                var result = runtime.TryCollect(collector.CurrentHealth, collector.MaxHealth);
                if (!result.Collected) continue;
                var appliedHeal = RequireSimulation().Heal(collector.Id, result.HealAmount);
                if (appliedHeal <= 0) continue;
                pickupCollections.Add(new MatchPickupCollectionIntent(
                    definition.PickupId,
                    collector.Id,
                    appliedHeal,
                    _identityTracker.NextCollectionEventId(),
                    _identityTracker.NextHealingEventId()));
            }

            for (var i = 0; i < _gadgetPickups.Length; i++)
            {
                var runtime = _gadgetPickups[i];
                var definition = runtime.Definition;
                if (!runtime.IsAvailable) continue;
                if (!TrySelectCollector(snapshots, definition.Position, definition.CollectionRadius, false, out var collector)) continue;
                if (!_gadgetInventories.TryGetValue(collector.Id, out var inventory) || inventory.HasGadget) continue;
                var result = runtime.TryCollect(false);
                if (!result.Collected || !inventory.TryPickup(result.GadgetId)) continue;
                gadgetCollections.Add(new GadgetPickupCollectionIntent(
                    definition.PickupId,
                    collector.Id,
                    result.GadgetId,
                    _identityTracker.NextCollectionEventId()));
            }
        }

        private DomainProjectileSnapshot[] AdvanceProjectiles(
            int simulationTick,
            float fixedDeltaSeconds,
            OfflineMatchSimulation simulation,
            List<CombatDamageEvent> damageEvents)
        {
            if (_activeProjectiles.Count == 0) return Array.Empty<DomainProjectileSnapshot>();

            var snapshots = new List<DomainProjectileSnapshot>(_activeProjectiles.Count);
            var participantSnapshots = simulation.GetSnapshots();
            RefreshSortedAuthorityTargets();

            for (var i = _activeProjectiles.Count - 1; i >= 0; i--)
            {
                var proj = _activeProjectiles[i];
                if (!proj.IsActive)
                {
                    _activeProjectiles.RemoveAt(i);
                    continue;
                }

                var stepDistance = proj.Speed * fixedDeltaSeconds;
                var maxDistance = Math.Min(stepDistance, proj.RemainingRange);

                if (maxDistance <= 0.00001f)
                {
                    proj.Despawn(ProjectileDespawnReason.RangeExpired);
                    snapshots.Add(proj.ToSnapshot());
                    _activeProjectiles.RemoveAt(i);
                    continue;
                }

                var bestHitDistance = maxDistance;
                var hitType = ProjectileDespawnReason.None;
                var hitTargetId = default(CombatEntityId);
                var hitStationId = 0;

                // 1. Arena collision (walls / obstacles)
                if (_collisionDefinition.Raycast(proj.Position, proj.Direction, maxDistance, out var wallHitPoint, out _))
                {
                    var dWall = Float2.Distance(proj.Position, wallHitPoint);
                    if (dWall < bestHitDistance)
                    {
                        bestHitDistance = dWall;
                        hitType = ProjectileDespawnReason.HitWall;
                    }
                }

                // 2. Participants
                for (var p = 0; p < participantSnapshots.Length; p++)
                {
                    var target = participantSnapshots[p];
                    if (!target.Alive || target.Id == proj.InstigatorId) continue;
                    if (!AreDifferentCombatGroups(proj.InstigatorId, target.Id)) continue;

                    var targetRadius = _collisionDefinition.ActorRadius + proj.Radius;
                    if (IntersectRayCircle(proj.Position, proj.Direction, maxDistance, target.Position, targetRadius, out var tHit))
                    {
                        if (tHit < bestHitDistance - 0.0001f ||
                            (Math.Abs(tHit - bestHitDistance) <= 0.0001f && (hitType != ProjectileDespawnReason.HitActor || target.Id.Value < hitTargetId.Value)))
                        {
                            bestHitDistance = tHit;
                            hitType = ProjectileDespawnReason.HitActor;
                            hitTargetId = target.Id;
                        }
                    }
                }

                // 3. Maya Decoys
                for (var decoyIndex = 0; decoyIndex < _sortedDecoyOwnerIds.Count; decoyIndex++)
                {
                    var ownerId = _sortedDecoyOwnerIds[decoyIndex];
                    var decoy = _mayaDecoys[ownerId];
                    if (!decoy.IsActive || !decoy.IsTargetable || ownerId == proj.InstigatorId) continue;
                    if (!AreDifferentCombatGroups(proj.InstigatorId, ownerId)) continue;

                    var decoyId = GetDecoyId(ownerId);
                    var decoyRadius = _collisionDefinition.ActorRadius + proj.Radius;
                    if (IntersectRayCircle(proj.Position, proj.Direction, maxDistance, decoy.Position, decoyRadius, out var tHit))
                    {
                        if (tHit < bestHitDistance - 0.0001f ||
                            (Math.Abs(tHit - bestHitDistance) <= 0.0001f && (hitType != ProjectileDespawnReason.HitDecoy || decoyId.Value < hitTargetId.Value)))
                        {
                            bestHitDistance = tHit;
                            hitType = ProjectileDespawnReason.HitDecoy;
                            hitTargetId = decoyId;
                        }
                    }
                }

                // 4. Tiffin Stations
                for (var stationIndex = 0; stationIndex < _sortedStationIds.Count; stationIndex++)
                {
                    var stationId = _sortedStationIds[stationIndex];
                    if (!_stations.TryGetValue(stationId, out var station) || !station.IsActive) continue;

                    var stationRadius = 0.55f + proj.Radius;
                    if (IntersectRayCircle(proj.Position, proj.Direction, maxDistance, station.Position, stationRadius, out var tHit))
                    {
                        if (tHit < bestHitDistance - 0.0001f)
                        {
                            bestHitDistance = tHit;
                            hitType = ProjectileDespawnReason.HitStation;
                            hitStationId = stationId;
                        }
                    }
                }

                if (hitType != ProjectileDespawnReason.None)
                {
                    var hitPos = proj.Position + proj.Direction * bestHitDistance;
                    proj.MoveTo(hitPos, bestHitDistance, fixedDeltaSeconds);
                    proj.Despawn(hitType, hitTargetId);

                    if (_participantWeapons.TryGetValue(proj.InstigatorId, out var weapon))
                    {
                        if (hitType == ProjectileDespawnReason.HitActor && hitTargetId.Value > 0)
                        {
                            _participantFactions.TryGetValue(hitTargetId, out var targetFaction);
                            var damageReq = new DamageRequest(
                                proj.InstigatorId,
                                hitTargetId,
                                proj.Faction,
                                weapon.Damage,
                                DamageType.Projectile,
                                proj.Direction,
                                simulationTick);
                            var resolved = ResolveDamage(
                                damageReq,
                                targetFaction,
                                false,
                                AreDifferentCombatGroups(proj.InstigatorId, hitTargetId));
                            if (resolved.Result.Applied)
                            {
                                damageEvents.Add(new CombatDamageEvent(
                                    resolved.Request,
                                    resolved.Result.AmountApplied,
                                    resolved.Result.TargetDefeated,
                                    resolved.CurrentHealthAfter,
                                    simulationTick,
                                    RequireSimulation().LastDamageEventId));
                            }
                        }
                        else if (hitType == ProjectileDespawnReason.HitDecoy && hitTargetId.Value > 0)
                        {
                            _participantFactions.TryGetValue(proj.InstigatorId, out var instigatorFaction);
                            if (TryFindMayaDecoy(hitTargetId, out var decoyOwnerId, out _))
                            {
                                _participantFactions.TryGetValue(decoyOwnerId, out var decoyTargetFaction);
                                var damageReq = new DamageRequest(
                                    proj.InstigatorId,
                                    hitTargetId,
                                    instigatorFaction,
                                    weapon.Damage,
                                    DamageType.Projectile,
                                    proj.Direction,
                                    simulationTick);
                                ResolveMayaDecoyDamage(
                                    damageReq,
                                    decoyTargetFaction,
                                    false,
                                    AreDifferentCombatGroups(proj.InstigatorId, decoyOwnerId));
                            }
                        }
                        else if (hitType == ProjectileDespawnReason.HitStation && hitStationId > 0)
                        {
                            TryDamageStation(hitStationId, weapon.Damage);
                        }
                    }

                    snapshots.Add(proj.ToSnapshot());
                    _activeProjectiles.RemoveAt(i);
                }
                else
                {
                    var nextPos = proj.Position + proj.Direction * stepDistance;
                    proj.MoveTo(nextPos, stepDistance, fixedDeltaSeconds);

                    if (proj.RemainingRange <= 0.0001f)
                    {
                        proj.Despawn(ProjectileDespawnReason.RangeExpired);
                        snapshots.Add(proj.ToSnapshot());
                        _activeProjectiles.RemoveAt(i);
                    }
                    else if (proj.RemainingLifetime <= 0.0001f)
                    {
                        proj.Despawn(ProjectileDespawnReason.LifetimeExpired);
                        snapshots.Add(proj.ToSnapshot());
                        _activeProjectiles.RemoveAt(i);
                    }
                    else
                    {
                        snapshots.Add(proj.ToSnapshot());
                    }
                }
            }

            return snapshots.ToArray();
        }

        private static bool IntersectRayCircle(Float2 rayStart, Float2 rayDir, float maxDist, Float2 circleCenter, float circleRadius, out float tHit)
        {
            tHit = maxDist;
            var v = circleCenter - rayStart;
            var vSq = v.X * v.X + v.Y * v.Y;
            var rSq = circleRadius * circleRadius;
            if (vSq <= rSq)
            {
                tHit = 0f;
                return true;
            }

            var tProj = v.X * rayDir.X + v.Y * rayDir.Y;
            if (tProj <= 0f) return false;

            var dSq = vSq - tProj * tProj;
            if (dSq > rSq) return false;

            var dHalf = (float)Math.Sqrt(rSq - dSq);
            var tEntry = tProj - dHalf;
            if (tEntry >= 0f && tEntry <= maxDist)
            {
                tHit = tEntry;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resolves pickup proximity and collector selection from the authoritative
        /// simulation snapshot. Unity only applies the returned intents to its views.
        /// </summary>
        public bool IsPickupAvailable(int pickupId)
        {
            var index = FindPickupIndex(pickupId);
            return index >= 0 && _pickups[index].IsAvailable;
        }

        public GadgetUseResult TryUseGadget(GadgetUseCommand command)
        {
            if (!_gadgetInventories.TryGetValue(command.UserId, out var inventory) ||
                !_gadgetRuntimes.TryGetValue(command.UserId, out var runtime))
            {
                return new GadgetUseResult(false, GadgetUseFailure.NotHeld, default(GadgetEffect));
            }

            if (!RequireSimulation().TryGetSnapshot(command.UserId, out var user) || !user.Alive ||
                !GadgetCatalog.TryGet(command.GadgetId, out var definition))
            {
                return new GadgetUseResult(false, GadgetUseFailure.InvalidPlacement, default(GadgetEffect));
            }

            if (!GetActionEligibility(command.UserId).IsEligible)
            {
                return new GadgetUseResult(false, GadgetUseFailure.InvalidPlacement, default(GadgetEffect));
            }

            var direction = command.Direction.Normalized;
            var canonicalOrigin = user.Position;
            if (definition.Kind == GadgetKind.TiffinStation)
            {
                if (direction.SqrMagnitude <= 0.000001f)
                {
                    return new GadgetUseResult(false, GadgetUseFailure.InvalidPlacement, default(GadgetEffect));
                }

                var placement = _collisionSolver.Move(user.Position, direction * definition.PlacementRadius);
                if (placement.AppliedDisplacement.SqrMagnitude <= 0.000001f)
                {
                    return new GadgetUseResult(false, GadgetUseFailure.InvalidPlacement, default(GadgetEffect));
                }

                canonicalOrigin = placement.Position;
            }

            var canonicalCommand = new GadgetUseCommand(
                command.UserId,
                command.GadgetId,
                canonicalOrigin,
                direction,
                command.Tick);
            var result = runtime.TryUse(inventory, canonicalCommand);
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
                    var delta = snapshot.Position - canonicalCommand.Origin;
                    if (delta.SqrMagnitude > effect.Definition.Radius * effect.Definition.Radius) continue;
                    var displacement = delta.Normalized * (effect.Definition.Magnitude * 0.08f);
                    var collision = _collisionSolver.Move(snapshot.Position, displacement);
                    if (!RequireSimulation().SetPosition(snapshot.Id, collision.Position)) continue;
                    displacements.Add(new GadgetDisplacementIntent(snapshot.Id, collision.AppliedDisplacement));
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
                _stations[stationId] = new GadgetStationRuntime(
                    stationId,
                    canonicalCommand.Origin,
                    effect.Definition,
                    command.UserId);
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

            return new GadgetUseResult(true, GadgetUseFailure.None, effect, _identityTracker.NextGadgetUseId());
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

            if (stationId <= 0 || rawAmount <= 0 || station == null || !station.IsActive ||
                !IsCombatActionPhase(CurrentPhase))
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

        private MatchAuthorityDecoy CreateDecoySnapshot(CombatEntityId ownerId, DecoyRuntime decoy)
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
                decoy.CooldownRemaining,
                _decoyExecutionIds.TryGetValue(ownerId, out var executionId) ? executionId : 0);
        }

        private void AdvanceStations(
            float fixedDeltaSeconds,
            MatchPhase phase,
            MatchParticipantSnapshot[] snapshots,
            List<GadgetHealingIntent> healingIntents,
            List<int> expiredStationIds)
        {
            if (_stations.Count == 0) return;
            var stationIds = new List<int>(_stations.Keys);
            stationIds.Sort();
            var expired = new List<int>();
            var healingAllowed = IsCombatActionPhase(phase);
            foreach (var stationId in stationIds)
            {
                var step = _stations[stationId].Advance(fixedDeltaSeconds, snapshots);
                if (healingAllowed)
                {
                    for (var i = 0; i < step.Healing.Length; i++)
                    {
                        var requested = step.Healing[i];
                        var applied = RequireSimulation().Heal(requested.TargetId, requested.Amount);
                        if (applied <= 0) continue;
                        healingIntents.Add(new GadgetHealingIntent(
                            requested.StationId,
                            requested.TargetId,
                            applied,
                            _identityTracker.NextHealingEventId()));
                    }
                }

                if (step.Expired) expired.Add(stationId);
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

        private MatchAuthorityActionEligibility GetActionEligibility(CombatEntityId actorId)
        {
            if (_simulation == null ||
                !_simulation.TryGetSnapshot(actorId, out var snapshot))
            {
                return MatchAuthorityActionEligibility.Rejected(MatchAuthorityActionPhaseFailure.UnknownActor);
            }

            if (!snapshot.Alive)
            {
                return MatchAuthorityActionEligibility.Rejected(MatchAuthorityActionPhaseFailure.DefeatedActor);
            }

            return IsCombatActionPhase(_simulation.Phase)
                ? MatchAuthorityActionEligibility.Eligible(snapshot)
                : MatchAuthorityActionEligibility.Rejected(ToActionFailure(_simulation.Phase));
        }

        private static bool IsCombatActionPhase(MatchPhase phase) =>
            phase >= MatchPhase.Opening && phase <= MatchPhase.FinalCircle;

        private static MatchAuthorityActionPhaseFailure ToActionFailure(MatchPhase phase)
        {
            switch (phase)
            {
                case MatchPhase.SpawnProtection:
                    return MatchAuthorityActionPhaseFailure.SpawnProtection;
                case MatchPhase.Resolution:
                    return MatchAuthorityActionPhaseFailure.Resolution;
                default:
                    return MatchAuthorityActionPhaseFailure.Warmup;
            }
        }

        private static MatchAuthorityAttackFailure ToAttackFailure(
            MatchAuthorityActionPhaseFailure failure)
        {
            switch (failure)
            {
                case MatchAuthorityActionPhaseFailure.UnknownActor:
                    return MatchAuthorityAttackFailure.UnknownActor;
                case MatchAuthorityActionPhaseFailure.DefeatedActor:
                    return MatchAuthorityAttackFailure.DefeatedActor;
                case MatchAuthorityActionPhaseFailure.SpawnProtection:
                    return MatchAuthorityAttackFailure.SpawnProtection;
                case MatchAuthorityActionPhaseFailure.Resolution:
                    return MatchAuthorityAttackFailure.Resolution;
                default:
                    return MatchAuthorityAttackFailure.Warmup;
            }
        }
    }
}
