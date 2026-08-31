using System;
using System.Collections.Generic;

namespace BattleRaja.Core.Domain
{
    /// <summary>
    /// Deterministic, Unity-independent Bastion Crown rules. The class owns only
    /// per-match mutable state; the mode definition and team/member contracts are
    /// immutable input data. Presentation may mirror this state but cannot award
    /// score, spend tickets, revive a fighter or decide a winner.
    /// </summary>
    public sealed class BastionCrownMatch
    {
        public const int ParticipantCount = 8;
        public const int TeamSize = 4;
        private const ulong HashOffsetBasis = 14695981039346656037UL;
        private const ulong HashPrime = 1099511628211UL;

        private readonly ModeDefinition _definition;
        private readonly uint _seed;
        private readonly ParticipantState[] _participants = new ParticipantState[ParticipantCount];
        private readonly BastionParticipantSnapshot[] _plannerSnapshots = new BastionParticipantSnapshot[ParticipantCount];
        private readonly Dictionary<CombatEntityId, int> _participantIndices = new Dictionary<CombatEntityId, int>(ParticipantCount);
        private readonly Dictionary<CombatEntityId, Dictionary<CombatEntityId, int>> _damageContributions =
            new Dictionary<CombatEntityId, Dictionary<CombatEntityId, int>>(ParticipantCount);
        private readonly HashSet<int> _processedDamageEvents = new HashSet<int>();
        private readonly HashSet<int> _processedHealingEvents = new HashSet<int>();
        private readonly HashSet<int> _processedGadgetEvents = new HashSet<int>();
        private readonly HashSet<int> _processedAbilityEvents = new HashSet<int>();
        private readonly TeamState _raja = new TeamState(BastionTeamId.Raja, ModeDefinition.BastionCrown.Respawn.StartingTickets);
        private readonly TeamState _rival = new TeamState(BastionTeamId.Rival, ModeDefinition.BastionCrown.Respawn.StartingTickets);
        private readonly List<CombatEntityId> _respawnedActors = new List<CombatEntityId>(TeamSize);

        private int _crownSocketIndex;
        private Float2 _crownPosition;
        private CombatEntityId _crownCarrier;
        private bool _crownDropped;
        private float _crownPickupProgress;
        private CombatEntityId _crownPickupActor;
        private float _crownDropLockRemaining;
        private float _crownDropRemaining;
        private float _crownRotationRemaining;
        private CombatEntityId _depositChannelActor;
        private float _depositChannelProgress;
        private float _elapsedSeconds;
        private float _overtimeElapsed;
        private int _lastTick = -1;
        private bool _started;
        private bool _overtime;
        private bool _ended;
        private BastionTeamId _winner;
        private BastionResultSummary _result;
        private AandhiState _aandhiState;
        private Float2 _aandhiZoneCenter;
        private float _aandhiZoneRadius;
        private float _aandhiWarningRemaining;

        public BastionCrownMatch(uint seed)
            : this(ModeDefinition.BastionCrown, seed)
        {
        }

        public BastionCrownMatch(ModeDefinition definition, uint seed)
        {
            _definition = definition;
            _seed = seed == 0u ? 1u : seed;
            // The fields are initialized with the canonical ticket count above;
            // Start rehydrates them from the supplied definition for custom tests.
            _raja.Reset(definition.Respawn.StartingTickets);
            _rival.Reset(definition.Respawn.StartingTickets);
        }

        public ModeDefinition Definition => _definition;
        public uint Seed => _seed;
        public bool IsStarted => _started;
        public bool IsEnded => _ended;
        public bool IsOvertime => _overtime && !_ended;
        public bool IsLive => _started && !_ended && ((_elapsedSeconds >= _definition.ReadySeconds && _elapsedSeconds < _definition.ReadySeconds + _definition.LiveSeconds) || _overtime);
        public float ElapsedSeconds => _elapsedSeconds;
        public BastionTeamId Winner => _winner;
        public BastionResultSummary Result => _result;
        public CrownSparkSnapshot Crown => CreateCrownSnapshot();
        public AandhiState AandhiState => _aandhiState;
        public Float2 AandhiZoneCenter => _aandhiZoneCenter;
        public float AandhiZoneRadius => _aandhiZoneRadius;
        public float AandhiWarningRemainingSeconds => _aandhiWarningRemaining;

        /// <summary>
        /// Computes a stable post-tick digest for the complete Bastion state.
        /// The digest deliberately includes objective, team, respawn and
        /// per-actor counters so a replay cannot pass while the legacy combat
        /// mirror has silently drifted from the team authority.
        /// </summary>
        public ulong CalculateDeterministicHash(int simulationTick = -1)
        {
            unchecked
            {
                var hash = HashOffsetBasis;

                HashInt(ref hash, simulationTick >= 0 ? simulationTick : _lastTick);
                HashText(ref hash, _definition.ModeId);
                HashInt(ref hash, (int)_seed);
                HashFloat(ref hash, _elapsedSeconds);
                HashFloat(ref hash, _overtimeElapsed);
                HashBool(ref hash, _started);
                HashBool(ref hash, _overtime);
                HashBool(ref hash, _ended);
                HashInt(ref hash, (int)_winner);
                HashInt(ref hash, (int)_aandhiState);
                HashFloat(ref hash, _aandhiZoneCenter.X);
                HashFloat(ref hash, _aandhiZoneCenter.Y);
                HashFloat(ref hash, _aandhiZoneRadius);
                HashFloat(ref hash, _aandhiWarningRemaining);

                HashInt(ref hash, _crownSocketIndex);
                HashFloat(ref hash, _crownPosition.X);
                HashFloat(ref hash, _crownPosition.Y);
                HashId(ref hash, _crownCarrier);
                HashBool(ref hash, _crownDropped);
                HashFloat(ref hash, _crownPickupProgress);
                HashId(ref hash, _crownPickupActor);
                HashFloat(ref hash, _crownDropLockRemaining);
                HashFloat(ref hash, _crownDropRemaining);
                HashFloat(ref hash, _crownRotationRemaining);
                HashId(ref hash, _depositChannelActor);
                HashFloat(ref hash, _depositChannelProgress);

                HashTeam(ref hash, _raja);
                HashTeam(ref hash, _rival);
                HashInt(ref hash, _processedDamageEvents.Count);
                HashInt(ref hash, _processedHealingEvents.Count);
                HashInt(ref hash, _processedGadgetEvents.Count);
                HashInt(ref hash, _processedAbilityEvents.Count);
                // Damage contribution maps influence future assist awards. Walk
                // canonical actor IDs rather than dictionary enumeration so the
                // digest remains stable across runtime implementations.
                for (var targetIndex = 0; targetIndex < _participants.Length; targetIndex++)
                {
                    var targetState = _participants[targetIndex];
                    if (targetState == null) continue;
                    HashId(ref hash, targetState.Member.ActorId);
                    if (!_damageContributions.TryGetValue(targetState.Member.ActorId, out var contributions))
                    {
                        HashInt(ref hash, 0);
                        continue;
                    }

                    var contributionCount = 0;
                    for (var sourceIndex = 0; sourceIndex < _participants.Length; sourceIndex++)
                    {
                        var sourceState = _participants[sourceIndex];
                        if (sourceState == null || !contributions.TryGetValue(sourceState.Member.ActorId, out var amount)) continue;
                        contributionCount++;
                    }

                    HashInt(ref hash, contributionCount);
                    for (var sourceIndex = 0; sourceIndex < _participants.Length; sourceIndex++)
                    {
                        var sourceState = _participants[sourceIndex];
                        if (sourceState == null || !contributions.TryGetValue(sourceState.Member.ActorId, out var amount)) continue;
                        HashId(ref hash, sourceState.Member.ActorId);
                        HashInt(ref hash, amount);
                    }
                }
                for (var i = 0; i < _participants.Length; i++)
                {
                    var participant = _participants[i];
                    if (participant == null)
                    {
                        HashBool(ref hash, false);
                        continue;
                    }

                    HashBool(ref hash, true);
                    HashId(ref hash, participant.Member.ActorId);
                    HashInt(ref hash, (int)participant.Member.TeamId);
                    HashInt(ref hash, (int)participant.Member.Role);
                    HashBool(ref hash, participant.Member.IsHuman);
                    HashFloat(ref hash, participant.Position.X);
                    HashFloat(ref hash, participant.Position.Y);
                    HashFloat(ref hash, participant.SpawnPosition.X);
                    HashFloat(ref hash, participant.SpawnPosition.Y);
                    HashInt(ref hash, participant.CurrentHealth);
                    HashInt(ref hash, participant.MaxHealth);
                    HashBool(ref hash, participant.Alive);
                    HashBool(ref hash, participant.Spectating);
                    HashBool(ref hash, participant.RespawnPending);
                    HashFloat(ref hash, participant.SpectatorRemaining);
                    HashFloat(ref hash, participant.RespawnRemaining);
                    HashBool(ref hash, participant.SpawnProtected);
                    HashFloat(ref hash, participant.SpawnProtectionRemaining);
                    HashInt(ref hash, participant.Eliminations);
                    HashInt(ref hash, participant.Deaths);
                    HashInt(ref hash, participant.Assists);
                    HashInt(ref hash, participant.DamageDealt);
                    HashInt(ref hash, participant.HealingDone);
                    HashInt(ref hash, participant.GadgetUses);
                    HashInt(ref hash, participant.AbilityUses);
                    HashFloat(ref hash, participant.ObjectiveSeconds);
                    HashFloat(ref hash, participant.SurvivalSeconds);
                }

                return hash;
            }
        }

        private static void HashInt(ref ulong hash, int value)
        {
            unchecked
            {
                hash ^= (ulong)(byte)value;
                hash *= HashPrime;
                hash ^= (ulong)(byte)(value >> 8);
                hash *= HashPrime;
                hash ^= (ulong)(byte)(value >> 16);
                hash *= HashPrime;
                hash ^= (ulong)(byte)(value >> 24);
                hash *= HashPrime;
            }
        }

        private static void HashBool(ref ulong hash, bool value) => HashInt(ref hash, value ? 1 : 0);

        private static void HashFloat(ref ulong hash, float value) => HashInt(ref hash, (int)(value * 1000f));

        private static void HashId(ref ulong hash, CombatEntityId id) => HashInt(ref hash, id.Value);

        private static void HashText(ref ulong hash, string value)
        {
            value = value ?? string.Empty;
            HashInt(ref hash, value.Length);
            for (var i = 0; i < value.Length; i++) HashInt(ref hash, value[i]);
        }

        private static void HashTeam(ref ulong hash, TeamState team)
        {
            HashInt(ref hash, (int)team.TeamId);
            HashInt(ref hash, team.Score);
            HashInt(ref hash, team.Deposits);
            HashInt(ref hash, team.KOs);
            HashInt(ref hash, team.Assists);
            HashInt(ref hash, team.CrownPickups);
            HashInt(ref hash, team.Deaths);
            HashInt(ref hash, team.DamageDealt);
            HashInt(ref hash, team.HealingDone);
            HashInt(ref hash, team.GadgetUses);
            HashInt(ref hash, team.AbilityUses);
            HashInt(ref hash, team.Tickets.Maximum);
            HashInt(ref hash, team.Tickets.Remaining);
            HashInt(ref hash, team.Tickets.Spent);
            HashFloat(ref hash, team.ObjectiveSeconds);
            HashInt(ref hash, team.OvertimeDeposits);
        }

        public void Start(IReadOnlyList<BastionCrownSlot> slots)
        {
            if (_started) throw new InvalidOperationException("A Bastion Crown match can only be started once.");
            ValidateSlots(slots);

            _participantIndices.Clear();
            _damageContributions.Clear();
            _processedDamageEvents.Clear();
            _processedHealingEvents.Clear();
            _processedGadgetEvents.Clear();
            _processedAbilityEvents.Clear();
            for (var i = 0; i < ParticipantCount; i++)
            {
                var slot = slots[i];
                _participants[i] = new ParticipantState(slot);
                _participants[i].SpawnProtected = _definition.Respawn.SpawnProtectionSeconds > 0f;
                _participants[i].SpawnProtectionRemaining = _definition.Respawn.SpawnProtectionSeconds;
                _participantIndices.Add(slot.Member.ActorId, i);
            }

            _raja.Reset(_definition.Respawn.StartingTickets);
            _rival.Reset(_definition.Respawn.StartingTickets);
            _crownSocketIndex = (int)(_seed % 3u);
            _crownPosition = _definition.Objective.SocketPositions[_crownSocketIndex];
            _crownCarrier = default(CombatEntityId);
            _crownDropped = false;
            _crownPickupProgress = 0f;
            _crownPickupActor = default(CombatEntityId);
            _crownDropLockRemaining = 0f;
            _crownDropRemaining = 0f;
            _crownRotationRemaining = _definition.Objective.RotationSeconds;
            _depositChannelActor = default(CombatEntityId);
            _depositChannelProgress = 0f;
            _elapsedSeconds = 0f;
            _overtimeElapsed = 0f;
            _lastTick = -1;
            _overtime = false;
            _ended = false;
            _winner = BastionTeamId.None;
            _result = default(BastionResultSummary);
            _aandhiState = AandhiState.Stable;
            _aandhiZoneCenter = Float2.Zero;
            _aandhiZoneRadius = 0f;
            _aandhiWarningRemaining = 0f;
            _started = true;
        }

        /// <summary>
        /// Mirrors the legacy authority's canonical Aandhi telemetry into the
        /// team planner. The zone remains advisory for scoring, but it is part
        /// of the deterministic squad decision and digest.
        /// </summary>
        public bool SyncAandhi(
            Float2 zoneCenter,
            float zoneRadius,
            AandhiState state,
            float warningRemainingSeconds = 0f)
        {
            if (!_started || !zoneCenter.IsFinite || zoneRadius < 0f ||
                float.IsNaN(zoneRadius) || float.IsInfinity(zoneRadius) ||
                warningRemainingSeconds < 0f ||
                float.IsNaN(warningRemainingSeconds) || float.IsInfinity(warningRemainingSeconds))
            {
                return false;
            }

            _aandhiZoneCenter = zoneCenter;
            _aandhiZoneRadius = zoneRadius;
            _aandhiState = state;
            _aandhiWarningRemaining = warningRemainingSeconds;
            return true;
        }

        public BastionTeamId GetTeam(CombatEntityId actorId)
        {
            return TryGetParticipant(actorId, out var participant) ? participant.Member.TeamId : BastionTeamId.None;
        }

        public bool AreAllies(CombatEntityId first, CombatEntityId second)
        {
            var firstTeam = GetTeam(first);
            return first != second && firstTeam != BastionTeamId.None && firstTeam == GetTeam(second);
        }

        public bool AreEnemies(CombatEntityId first, CombatEntityId second)
        {
            var firstTeam = GetTeam(first);
            return first != second && firstTeam != BastionTeamId.None && firstTeam != GetTeam(second) && GetTeam(second) != BastionTeamId.None;
        }

        public TeamScore GetTeamScore(BastionTeamId teamId) => GetTeamState(teamId).ToScore();

        public TeamTicketPool GetTickets(BastionTeamId teamId) => GetTeamState(teamId).Tickets;

        public BastionParticipantSnapshot[] GetParticipantSnapshots()
        {
            var snapshots = new BastionParticipantSnapshot[ParticipantCount];
            for (var i = 0; i < ParticipantCount; i++) snapshots[i] = _participants[i].ToSnapshot();
            return snapshots;
        }

        /// <summary>
        /// Returns the deterministic squad assignment for one actor. The
        /// planner consumes only canonical team/objective snapshots; it never
        /// reads Unity transforms, hidden targets or presentation state.
        /// </summary>
        public bool TryGetSquadIntent(CombatEntityId actorId, out BastionSquadIntent intent)
        {
            if (!_started || !TryGetParticipantState(actorId, out var participant) || !participant.Alive)
            {
                intent = default(BastionSquadIntent);
                return false;
            }

            for (var i = 0; i < ParticipantCount; i++) _plannerSnapshots[i] = _participants[i].ToSnapshot();
            intent = BastionSquadPlanner.Plan(
                participant.Member,
                participant.Position,
                _plannerSnapshots,
                CreateCrownSnapshot(),
                _raja.ToScore(),
                _rival.ToScore(),
                _raja.Tickets,
                _rival.Tickets,
                _definition,
                _overtime,
                _aandhiZoneCenter,
                _aandhiZoneRadius,
                _aandhiState);
            return true;
        }

        /// <summary>
        /// Records an authority-resolved heal exactly once. The source actor
        /// receives healing credit; environmental/self heals credit the target.
        /// </summary>
        public bool NotifyHealing(
            CombatEntityId healerId,
            CombatEntityId targetId,
            int amountApplied,
            int eventId = 0)
        {
            if (!_started || amountApplied <= 0 || !TryGetParticipantState(targetId, out var target) || !target.Alive)
            {
                return false;
            }

            if (eventId != 0 && !_processedHealingEvents.Add(eventId)) return false;
            var credited = TryGetParticipantState(healerId, out var healer) && healer.Alive
                ? healer
                : target;
            credited.HealingDone = SaturatingAdd(credited.HealingDone, amountApplied);
            GetTeamState(credited.Member.TeamId).HealingDone =
                SaturatingAdd(GetTeamState(credited.Member.TeamId).HealingDone, amountApplied);
            return true;
        }

        /// <summary>Records one accepted gadget use without trusting a view-side
        /// counter. Re-delivery of the same authority event is ignored.</summary>
        public bool RecordGadgetUse(CombatEntityId actorId, int eventId = 0)
        {
            if (!_started || !TryGetParticipantState(actorId, out var participant) || !participant.Alive)
            {
                return false;
            }

            if (eventId != 0 && !_processedGadgetEvents.Add(eventId)) return false;
            participant.GadgetUses = SaturatingAdd(participant.GadgetUses, 1);
            var team = GetTeamState(participant.Member.TeamId);
            team.GadgetUses = SaturatingAdd(team.GadgetUses, 1);
            return true;
        }

        /// <summary>Records one accepted fighter ability use. Re-delivery of the
        /// same authority event is ignored.</summary>
        public bool RecordAbilityUse(CombatEntityId actorId, int eventId = 0)
        {
            if (!_started || !TryGetParticipantState(actorId, out var participant) || !participant.Alive)
            {
                return false;
            }

            if (eventId != 0 && !_processedAbilityEvents.Add(eventId)) return false;
            participant.AbilityUses = SaturatingAdd(participant.AbilityUses, 1);
            var team = GetTeamState(participant.Member.TeamId);
            team.AbilityUses = SaturatingAdd(team.AbilityUses, 1);
            return true;
        }

        public bool TryGetParticipant(CombatEntityId actorId, out BastionParticipantSnapshot snapshot)
        {
            if (_participantIndices.TryGetValue(actorId, out var index) && _participants[index] != null)
            {
                snapshot = _participants[index].ToSnapshot();
                return true;
            }

            snapshot = default(BastionParticipantSnapshot);
            return false;
        }

        public bool SetPosition(CombatEntityId actorId, Float2 position)
        {
            if (!position.IsFinite || !TryGetParticipantState(actorId, out var participant)) return false;
            participant.Position = position;
            return true;
        }

        public bool SetHealth(CombatEntityId actorId, int currentHealth)
        {
            if (!TryGetParticipantState(actorId, out var participant)) return false;
            participant.CurrentHealth = !participant.Alive
                ? 0
                : Math.Max(0, Math.Min(participant.MaxHealth, currentHealth));
            return true;
        }

        /// <summary>Mirrors a canonical authority snapshot into the team layer.
        /// It never revives a fighter unless the team layer already issued a
        /// respawn; a spontaneous presentation-side revival is rejected.</summary>
        public bool SyncParticipant(CombatEntityId actorId, Float2 position, int currentHealth, bool alive)
        {
            if (!TryGetParticipantState(actorId, out var participant) || !position.IsFinite) return false;
            participant.Position = position;
            if (alive)
            {
                if (!participant.Alive && !participant.RespawnPending) return false;
                participant.CurrentHealth = Math.Max(0, Math.Min(participant.MaxHealth, currentHealth));
                return true;
            }

            participant.CurrentHealth = 0;
            if (participant.Alive) Defeat(participant, default(CombatEntityId), 0);
            return true;
        }

        /// <summary>Applies a validated damage request in the pure team mode.
        /// Existing Unity combat can instead call NotifyCombatDamage when the
        /// legacy authority has already applied health.</summary>
        public DamageResult ApplyDamage(DamageRequest request, int eventId = 0)
        {
            if (!_started || !IsLive || !TryGetParticipantState(request.TargetId, out var target))
            {
                return new DamageResult(false, 0, false, DamageRejectionReason.WrongTarget);
            }

            if (request.RawAmount <= 0)
            {
                return new DamageResult(false, 0, false, DamageRejectionReason.InvalidAmount);
            }

            if (request.InstigatorId == target.Member.ActorId)
            {
                return new DamageResult(false, 0, target.CurrentHealth <= 0, DamageRejectionReason.SelfHit);
            }

            if (AreAllies(request.InstigatorId, target.Member.ActorId))
            {
                return new DamageResult(false, 0, target.CurrentHealth <= 0, DamageRejectionReason.FriendlyFire);
            }

            if (!target.Alive || target.CurrentHealth <= 0)
            {
                return new DamageResult(false, 0, true, DamageRejectionReason.AlreadyDefeated);
            }

            if (target.SpawnProtected)
            {
                return new DamageResult(false, 0, false, DamageRejectionReason.SpawnProtection);
            }

            if (eventId != 0 && _processedDamageEvents.Contains(eventId))
            {
                return new DamageResult(false, 0, target.CurrentHealth <= 0, DamageRejectionReason.AlreadyDefeated);
            }

            var applied = Math.Min(target.CurrentHealth, request.RawAmount);
            target.CurrentHealth -= applied;
            var defeated = target.CurrentHealth == 0;
            RecordCombatDamage(request.InstigatorId, target.Member.ActorId, applied, defeated, eventId);
            if (TryGetParticipantState(request.InstigatorId, out var attacker)) attacker.SpawnProtected = false;
            return new DamageResult(true, applied, defeated, DamageRejectionReason.None);
        }

        /// <summary>Records damage that was resolved by the existing offline
        /// authority. Event identity makes repeated delivery side-effect free.</summary>
        public bool NotifyCombatDamage(
            CombatEntityId instigatorId,
            CombatEntityId targetId,
            int amountApplied,
            bool targetDefeated,
            int eventId = 0)
        {
            if (!_started || amountApplied <= 0 || !TryGetParticipantState(targetId, out var target)) return false;
            // A resolved event may mark a live target defeated, but an already
            // dead target can never accept another event, even when a caller
            // incorrectly repeats it with a new identity. Reject before
            // recording the event identity so a rejected delivery cannot block
            // a later authoritative delivery that reuses that identity.
            if (!target.Alive || target.CurrentHealth <= 0) return false;
            if (eventId != 0 && !_processedDamageEvents.Add(eventId)) return false;
            RecordCombatDamage(instigatorId, targetId, amountApplied, targetDefeated, eventIdAlreadyRecorded: eventId != 0);
            if (TryGetParticipantState(instigatorId, out var attacker)) attacker.SpawnProtected = false;
            if (targetDefeated && target.Alive) Defeat(target, instigatorId, eventId);
            return true;
        }

        public void ClearSpawnProtection(CombatEntityId actorId)
        {
            if (TryGetParticipantState(actorId, out var participant)) participant.SpawnProtected = false;
        }

        public float GetMovementMultiplier(CombatEntityId actorId) =>
            _crownCarrier == actorId ? _definition.Objective.CarrierSpeedMultiplier : 1f;

        /// <summary>Accumulates the canonical 0.25-second contact pickup.</summary>
        public bool TryPickupCrown(CombatEntityId actorId, float deltaSeconds)
        {
            if (!_started || !IsLive || deltaSeconds <= 0f || _crownCarrier.Value > 0 ||
                _crownDropLockRemaining > 0f || !TryGetParticipantState(actorId, out var participant) || !participant.Alive)
            {
                _crownPickupProgress = 0f;
                _crownPickupActor = default(CombatEntityId);
                return false;
            }

            if (Float2.Distance(participant.Position, _crownPosition) > _definition.Objective.ContactRadius)
            {
                _crownPickupProgress = 0f;
                _crownPickupActor = default(CombatEntityId);
                return false;
            }

            if (_crownPickupActor != actorId)
            {
                _crownPickupActor = actorId;
                _crownPickupProgress = 0f;
            }

            _crownPickupProgress = Math.Min(_definition.Objective.PickupSeconds, _crownPickupProgress + deltaSeconds);
            if (_crownPickupProgress + 0.00001f < _definition.Objective.PickupSeconds) return false;

            _crownCarrier = actorId;
            _crownDropped = false;
            _crownDropRemaining = 0f;
            _crownDropLockRemaining = 0f;
            _crownPickupProgress = 0f;
            _crownPickupActor = default(CombatEntityId);
            GetTeamState(participant.Member.TeamId).CrownPickups++;
            return true;
        }

        public bool TryBeginDeposit(CombatEntityId actorId)
        {
            if (!_started || !IsLive || _crownCarrier != actorId || !TryGetParticipantState(actorId, out var carrier) || !carrier.Alive)
            {
                return false;
            }

            var shrine = GetTeamDefinition(carrier.Member.TeamId).ShrinePosition;
            if (Float2.Distance(carrier.Position, shrine) > _definition.Objective.ContactRadius * 1.35f) return false;
            if (_depositChannelActor.Value != 0 && _depositChannelActor != actorId) return false;
            _depositChannelActor = actorId;
            return true;
        }

        public void CancelDeposit(CombatEntityId actorId)
        {
            if (_depositChannelActor == actorId)
            {
                _depositChannelActor = default(CombatEntityId);
                _depositChannelProgress = 0f;
            }
        }

        public void DropCrown(CombatEntityId actorId)
        {
            if (_crownCarrier != actorId || !TryGetParticipantState(actorId, out var carrier)) return;
            DropCrownAt(carrier.Position);
        }

        public BastionCrownTick Advance(float deltaSeconds, int simulationTick = -1)
        {
            if (!_started) throw new InvalidOperationException("Start Bastion Crown before advancing it.");
            if (deltaSeconds < 0f || float.IsNaN(deltaSeconds) || float.IsInfinity(deltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (simulationTick < 0) simulationTick = _lastTick + 1;
            if (simulationTick <= _lastTick) throw new ArgumentOutOfRangeException(nameof(simulationTick), "Ticks must increase monotonically.");
            _lastTick = simulationTick;
            _respawnedActors.Clear();

            if (!_ended)
            {
                var previousElapsed = _elapsedSeconds;
                _elapsedSeconds += deltaSeconds;
                var activeDelta = Math.Max(0f, _elapsedSeconds - Math.Max(previousElapsed, _definition.ReadySeconds));
                AdvanceParticipants(deltaSeconds, activeDelta);
                AdvanceCrown(deltaSeconds);
                AdvanceDeposit(deltaSeconds);
                if (!_ended) CheckTeamWipes();
                if (!_ended) CheckClock();
            }

            return CreateTick(simulationTick);
        }

        /// <summary>Used by the Unity adapter after it has respawned the
        /// matching legacy simulation participant. It is intentionally limited
        /// to a pending actor and cannot create a ninth slot.</summary>
        public bool ConfirmRespawn(CombatEntityId actorId)
        {
            if (!_started || !TryGetParticipantState(actorId, out var participant) || participant.Alive || !participant.RespawnPending)
            {
                return false;
            }

            participant.Alive = true;
            participant.Spectating = false;
            participant.RespawnPending = false;
            participant.SpectatorRemaining = 0f;
            participant.RespawnRemaining = 0f;
            participant.CurrentHealth = participant.MaxHealth;
            participant.Position = participant.SpawnPosition;
            participant.SpawnProtected = _definition.Respawn.SpawnProtectionSeconds > 0f;
            participant.SpawnProtectionRemaining = _definition.Respawn.SpawnProtectionSeconds;
            return true;
        }

        public void ForceResolve(BastionTeamId winner, BastionMatchResultReason reason = BastionMatchResultReason.Clock)
        {
            if (!_started || _ended) return;
            if (winner == BastionTeamId.None)
            {
                Resolve(BastionTeamId.None, true, BastionMatchResultReason.Draw);
            }
            else
            {
                Resolve(winner, false, reason);
            }
        }

        private void ValidateSlots(IReadOnlyList<BastionCrownSlot> slots)
        {
            if (slots == null || slots.Count != ParticipantCount)
            {
                throw new ArgumentException("Bastion Crown requires exactly eight participant slots.", nameof(slots));
            }

            var humanCount = 0;
            for (var i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                var expectedId = i + 1;
                if (slot.Member.ActorId.Value != expectedId)
                {
                    throw new ArgumentException("Bastion Crown slots must be actor IDs 1 through 8 in order.", nameof(slots));
                }

                var expectedTeam = i < TeamSize ? BastionTeamId.Raja : BastionTeamId.Rival;
                if (slot.Member.TeamId != expectedTeam || slot.Member.IsHuman != (i == 0))
                {
                    throw new ArgumentException("Bastion Crown requires actor 1 human on Raja, actors 2-4 allied AI and actors 5-8 rival AI.", nameof(slots));
                }

                if (slot.Member.IsHuman) humanCount++;
                for (var previous = 0; previous < i; previous++)
                {
                    if (Float2.Distance(slot.SpawnPosition, slots[previous].SpawnPosition) < 2.5f)
                    {
                        throw new ArgumentException("Bastion Crown spawn banks must be separated.", nameof(slots));
                    }
                }
            }

            if (humanCount != 1) throw new ArgumentException("Bastion Crown requires exactly one human slot.", nameof(slots));
        }

        private void AdvanceParticipants(float deltaSeconds, float activeDelta)
        {
            for (var i = 0; i < ParticipantCount; i++)
            {
                var participant = _participants[i];
                if (participant.Alive)
                {
                    if (participant.SpawnProtected)
                    {
                        participant.SpawnProtectionRemaining -= activeDelta;
                        if (participant.SpawnProtectionRemaining <= 0f)
                        {
                            participant.SpawnProtectionRemaining = 0f;
                            participant.SpawnProtected = false;
                        }
                    }

                    if (_crownCarrier == participant.Member.ActorId)
                    {
                        GetTeamState(participant.Member.TeamId).ObjectiveSeconds += deltaSeconds;
                        participant.ObjectiveSeconds += deltaSeconds;
                    }

                    participant.SurvivalSeconds += deltaSeconds;
                    continue;
                }

                if (!participant.RespawnPending) continue;
                participant.SpectatorRemaining = Math.Max(0f, participant.SpectatorRemaining - deltaSeconds);
                participant.RespawnRemaining -= deltaSeconds;
                if (participant.RespawnRemaining > 0f) continue;

                var team = GetTeamState(participant.Member.TeamId);
                if (!team.Tickets.HasTicket)
                {
                    participant.RespawnPending = false;
                    participant.Spectating = true;
                    continue;
                }

                team.SpendTicket();
                participant.RespawnPending = false;
                participant.Alive = true;
                participant.Spectating = false;
                participant.SpectatorRemaining = 0f;
                participant.RespawnRemaining = 0f;
                participant.CurrentHealth = participant.MaxHealth;
                participant.Position = participant.SpawnPosition;
                participant.SpawnProtected = _definition.Respawn.SpawnProtectionSeconds > 0f;
                participant.SpawnProtectionRemaining = _definition.Respawn.SpawnProtectionSeconds;
                _respawnedActors.Add(participant.Member.ActorId);
            }
        }

        private void AdvanceCrown(float deltaSeconds)
        {
            if (!IsLive) return;

            if (_crownCarrier.Value > 0)
            {
                if (!TryGetParticipantState(_crownCarrier, out var carrier) || !carrier.Alive)
                {
                    DropCrownAt(carrier != null ? carrier.Position : _crownPosition);
                }
                else
                {
                    _crownPosition = carrier.Position;
                }

                return;
            }

            if (_crownDropped)
            {
                _crownDropLockRemaining = Math.Max(0f, _crownDropLockRemaining - deltaSeconds);
                _crownDropRemaining -= deltaSeconds;
                if (_crownDropRemaining <= 0f) ResetCrownToSocket();
                return;
            }

            _crownRotationRemaining -= deltaSeconds;
            if (_crownRotationRemaining <= 0f) RotateCrown();
        }

        private void AdvanceDeposit(float deltaSeconds)
        {
            if (_depositChannelActor.Value == 0) return;
            if (!TryGetParticipantState(_depositChannelActor, out var carrier) || !carrier.Alive || _crownCarrier != _depositChannelActor)
            {
                CancelDeposit(_depositChannelActor);
                return;
            }

            var shrine = GetTeamDefinition(carrier.Member.TeamId).ShrinePosition;
            if (Float2.Distance(carrier.Position, shrine) > _definition.Objective.ContactRadius * 1.35f)
            {
                CancelDeposit(_depositChannelActor);
                return;
            }

            _depositChannelProgress = Math.Min(_definition.Objective.DepositChannelSeconds, _depositChannelProgress + deltaSeconds);
            if (_depositChannelProgress + 0.00001f >= _definition.Objective.DepositChannelSeconds)
            {
                CompleteDeposit(carrier);
            }
        }

        private void CompleteDeposit(ParticipantState carrier)
        {
            var team = GetTeamState(carrier.Member.TeamId);
            team.Deposits++;
            team.Score = SaturatingAdd(team.Score, 3);
            _depositChannelActor = default(CombatEntityId);
            _depositChannelProgress = 0f;
            _crownCarrier = default(CombatEntityId);
            _crownDropped = false;
            _crownDropLockRemaining = 0f;
            _crownDropRemaining = 0f;
            _crownPickupProgress = 0f;
            _crownPickupActor = default(CombatEntityId);
            // A completed delivery always hands the next Crown to the next
            // socket. Resetting to the same socket made repeat deposits a
            // deterministic stalemate and contradicted the mode contract.
            _crownSocketIndex = (_crownSocketIndex + 1) % _definition.Objective.SocketPositions.Length;
            _crownPosition = _definition.Objective.SocketPositions[_crownSocketIndex];
            _crownRotationRemaining = _definition.Objective.RotationSeconds;
            if (_overtime)
            {
                team.OvertimeDeposits++;
                Resolve(carrier.Member.TeamId, false, BastionMatchResultReason.OvertimeDeposit);
            }
            else if (team.Score >= _definition.ScoreToWin)
            {
                Resolve(carrier.Member.TeamId, false, BastionMatchResultReason.FirstToScore);
            }
        }

        private void RecordCombatDamage(
            CombatEntityId instigatorId,
            CombatEntityId targetId,
            int amountApplied,
            bool targetDefeated,
            int eventId = 0,
            bool eventIdAlreadyRecorded = false)
        {
            if (eventId != 0 && !eventIdAlreadyRecorded) _processedDamageEvents.Add(eventId);
            if (TryGetParticipantState(instigatorId, out var instigator) && instigatorId != targetId)
            {
                instigator.DamageDealt = SaturatingAdd(instigator.DamageDealt, amountApplied);
                GetTeamState(instigator.Member.TeamId).DamageDealt =
                    SaturatingAdd(GetTeamState(instigator.Member.TeamId).DamageDealt, amountApplied);
                if (!_damageContributions.TryGetValue(targetId, out var contributions))
                {
                    contributions = new Dictionary<CombatEntityId, int>();
                    _damageContributions.Add(targetId, contributions);
                }

                contributions.TryGetValue(instigatorId, out var existing);
                contributions[instigatorId] = SaturatingAdd(existing, amountApplied);
            }

            // Any applied combat damage breaks a shrine channel immediately.
            // This is checked before defeat handling so a lethal hit cannot
            // complete a deposit in the same fixed step.
            if (_depositChannelActor == targetId && amountApplied > 0)
            {
                CancelDeposit(targetId);
            }

            if (targetDefeated && TryGetParticipantState(targetId, out var target) && target.Alive)
            {
                Defeat(target, instigatorId, eventId);
            }
        }

        private void Defeat(ParticipantState participant, CombatEntityId instigatorId, int eventId)
        {
            if (!participant.Alive) return;
            participant.Alive = false;
            participant.CurrentHealth = 0;
            participant.Deaths = SaturatingAdd(participant.Deaths, 1);
            GetTeamState(participant.Member.TeamId).Deaths =
                SaturatingAdd(GetTeamState(participant.Member.TeamId).Deaths, 1);
            participant.Spectating = true;
            participant.SpectatorRemaining = _definition.Respawn.SpectatorSeconds;
            participant.RespawnRemaining = _definition.Respawn.RespawnSeconds;
            var team = GetTeamState(participant.Member.TeamId);
            participant.RespawnPending = team.Tickets.HasTicket;

            if (_crownCarrier == participant.Member.ActorId) DropCrownAt(participant.Position);
            CancelDeposit(participant.Member.ActorId);

            if (TryGetParticipantState(instigatorId, out var finisher) && AreEnemies(instigatorId, participant.Member.ActorId))
            {
                finisher.Eliminations = SaturatingAdd(finisher.Eliminations, 1);
                var finisherTeam = GetTeamState(finisher.Member.TeamId);
                finisherTeam.KOs++;
                finisherTeam.Score = SaturatingAdd(finisherTeam.Score, 1);
                CreditAssists(participant.Member.ActorId, instigatorId);
                if (!_overtime && finisherTeam.Score >= _definition.ScoreToWin)
                {
                    Resolve(finisher.Member.TeamId, false, BastionMatchResultReason.FirstToScore);
                }
            }

            _damageContributions.Remove(participant.Member.ActorId);
            CheckTeamWipes();
        }

        private void CreditAssists(CombatEntityId targetId, CombatEntityId finisherId)
        {
            if (!_damageContributions.TryGetValue(targetId, out var contributions)) return;
            foreach (var contribution in contributions)
            {
                if (contribution.Key == finisherId || !TryGetParticipantState(contribution.Key, out var assister) || !assister.Alive) continue;
                if (!AreEnemies(contribution.Key, targetId)) continue;
                assister.Assists = SaturatingAdd(assister.Assists, 1);
                GetTeamState(assister.Member.TeamId).Assists++;
            }
        }

        private void CheckTeamWipes()
        {
            if (IsTeamWiped(BastionTeamId.Raja) && IsTeamWiped(BastionTeamId.Rival))
            {
                Resolve(BastionTeamId.None, true, _overtime ? BastionMatchResultReason.OvertimeTeamWipe : BastionMatchResultReason.TeamWipe);
            }
            else if (IsTeamWiped(BastionTeamId.Raja))
            {
                Resolve(BastionTeamId.Rival, false, _overtime ? BastionMatchResultReason.OvertimeTeamWipe : BastionMatchResultReason.TeamWipe);
            }
            else if (IsTeamWiped(BastionTeamId.Rival))
            {
                Resolve(BastionTeamId.Raja, false, _overtime ? BastionMatchResultReason.OvertimeTeamWipe : BastionMatchResultReason.TeamWipe);
            }
        }

        private bool IsTeamWiped(BastionTeamId teamId)
        {
            for (var i = 0; i < ParticipantCount; i++)
            {
                var participant = _participants[i];
                if (participant.Member.TeamId != teamId) continue;
                if (participant.Alive || participant.RespawnPending) return false;
            }

            return true;
        }

        private void CheckClock()
        {
            var liveEnd = _definition.ReadySeconds + _definition.LiveSeconds;
            if (!_overtime && _elapsedSeconds >= liveEnd)
            {
                var comparison = CompareAtClock();
                if (comparison != BastionTeamId.None)
                {
                    Resolve(comparison, false, BastionMatchResultReason.Clock);
                    return;
                }

                _overtime = true;
                _overtimeElapsed = Math.Max(0f, _elapsedSeconds - liveEnd);
            }
            else if (_overtime)
            {
                _overtimeElapsed += Math.Max(0f, _elapsedSeconds - liveEnd - _overtimeElapsed);
            }

            if (_overtime && _overtimeElapsed >= _definition.OvertimeSeconds)
            {
                var comparison = CompareSuddenDeath();
                Resolve(comparison, comparison == BastionTeamId.None, comparison == BastionTeamId.None
                    ? BastionMatchResultReason.Draw
                    : BastionMatchResultReason.OvertimeCap);
            }
        }

        private BastionTeamId CompareAtClock()
        {
            if (_raja.Score != _rival.Score) return _raja.Score > _rival.Score ? BastionTeamId.Raja : BastionTeamId.Rival;
            if (_raja.Deposits != _rival.Deposits) return _raja.Deposits > _rival.Deposits ? BastionTeamId.Raja : BastionTeamId.Rival;
            if (_raja.KOs != _rival.KOs) return _raja.KOs > _rival.KOs ? BastionTeamId.Raja : BastionTeamId.Rival;
            if (_raja.Tickets.Remaining != _rival.Tickets.Remaining) return _raja.Tickets.Remaining > _rival.Tickets.Remaining ? BastionTeamId.Raja : BastionTeamId.Rival;
            return BastionTeamId.None;
        }

        private BastionTeamId CompareSuddenDeath()
        {
            if (_raja.OvertimeDeposits != _rival.OvertimeDeposits)
            {
                return _raja.OvertimeDeposits > _rival.OvertimeDeposits ? BastionTeamId.Raja : BastionTeamId.Rival;
            }

            return BastionTeamId.None;
        }

        private void Resolve(BastionTeamId winner, bool draw, BastionMatchResultReason reason)
        {
            if (_ended) return;
            _ended = true;
            _winner = winner;
            _result = new BastionResultSummary(
                winner,
                draw,
                reason,
                _elapsedSeconds,
                _seed,
                _raja.ToScore(),
                _rival.ToScore(),
                _raja.Tickets,
                _rival.Tickets);
        }

        private void DropCrownAt(Float2 position)
        {
            _crownCarrier = default(CombatEntityId);
            _crownDropped = true;
            _crownPosition = position;
            _crownDropLockRemaining = _definition.Objective.DropLockSeconds;
            _crownDropRemaining = _definition.Objective.DropLifetimeSeconds;
            _depositChannelActor = default(CombatEntityId);
            _depositChannelProgress = 0f;
            _crownPickupProgress = 0f;
            _crownPickupActor = default(CombatEntityId);
        }

        private void ResetCrownToSocket()
        {
            _crownDropped = false;
            _crownDropRemaining = 0f;
            _crownDropLockRemaining = 0f;
            _crownPosition = _definition.Objective.SocketPositions[_crownSocketIndex];
            _crownRotationRemaining = _definition.Objective.RotationSeconds;
            _crownPickupProgress = 0f;
            _crownPickupActor = default(CombatEntityId);
        }

        private void RotateCrown()
        {
            _crownSocketIndex = (_crownSocketIndex + 1) % _definition.Objective.SocketPositions.Length;
            _crownPosition = _definition.Objective.SocketPositions[_crownSocketIndex];
            _crownCarrier = default(CombatEntityId);
            _crownDropped = false;
            _crownRotationRemaining = _definition.Objective.RotationSeconds;
            _crownPickupProgress = 0f;
            _crownPickupActor = default(CombatEntityId);
        }

        private BastionCrownTick CreateTick(int simulationTick)
        {
            return new BastionCrownTick(
                simulationTick,
                _elapsedSeconds,
                IsLive,
                IsOvertime,
                _ended,
                _winner,
                CreateCrownSnapshot(),
                _raja.ToScore(),
                _rival.ToScore(),
                _raja.Tickets,
                _rival.Tickets,
                _respawnedActors.ToArray(),
                _result);
        }

        private CrownSparkSnapshot CreateCrownSnapshot()
        {
            return new CrownSparkSnapshot(
                _crownSocketIndex,
                _crownPosition,
                _crownCarrier,
                _crownDropped,
                _crownPickupProgress,
                _crownDropLockRemaining,
                _crownDropRemaining,
                _crownRotationRemaining,
                _depositChannelActor,
                _depositChannelProgress);
        }

        private TeamDefinition GetTeamDefinition(BastionTeamId teamId) => teamId == BastionTeamId.Raja ? _definition.Raja : _definition.Rival;

        private TeamState GetTeamState(BastionTeamId teamId)
        {
            if (teamId == BastionTeamId.Raja) return _raja;
            if (teamId == BastionTeamId.Rival) return _rival;
            throw new ArgumentOutOfRangeException(nameof(teamId));
        }

        private bool TryGetParticipantState(CombatEntityId actorId, out ParticipantState participant)
        {
            if (_participantIndices.TryGetValue(actorId, out var index))
            {
                participant = _participants[index];
                return participant != null;
            }

            participant = null;
            return false;
        }

        private static int SaturatingAdd(int value, int amount)
        {
            if (amount > 0 && value > int.MaxValue - amount) return int.MaxValue;
            return value + amount;
        }

        private sealed class ParticipantState
        {
            public ParticipantState(BastionCrownSlot slot)
            {
                Member = slot.Member;
                SpawnPosition = slot.SpawnPosition;
                Position = slot.SpawnPosition;
                MaxHealth = slot.MaxHealth;
                CurrentHealth = slot.MaxHealth;
                Alive = true;
                SpawnProtected = true;
                SpawnProtectionRemaining = 0f;
            }

            public TeamMember Member;
            public Float2 SpawnPosition;
            public Float2 Position;
            public int MaxHealth;
            public int CurrentHealth;
            public bool Alive;
            public bool Spectating;
            public bool RespawnPending;
            public float SpectatorRemaining;
            public float RespawnRemaining;
            public bool SpawnProtected;
            public float SpawnProtectionRemaining;
            public int Eliminations;
            public int Deaths;
            public int Assists;
            public int DamageDealt;
            public int HealingDone;
            public int GadgetUses;
            public int AbilityUses;
            public float ObjectiveSeconds;
            public float SurvivalSeconds;

            public BastionParticipantSnapshot ToSnapshot() => new BastionParticipantSnapshot(
                Member,
                Position,
                CurrentHealth,
                MaxHealth,
                Alive,
                Spectating,
                RespawnPending,
                SpectatorRemaining,
                RespawnRemaining,
                SpawnProtected,
                Eliminations,
                Deaths,
                Assists,
                DamageDealt,
                HealingDone,
                ObjectiveSeconds,
                GadgetUses,
                AbilityUses);
        }

        private sealed class TeamState
        {
            public TeamState(BastionTeamId teamId, int tickets)
            {
                TeamId = teamId;
                Tickets = new TeamTicketPool(teamId, Math.Max(0, tickets), Math.Max(0, tickets), 0);
            }

            public BastionTeamId TeamId;
            public int Score;
            public int Deposits;
            public int KOs;
            public int Assists;
            public int CrownPickups;
            public int Deaths;
            public int DamageDealt;
            public int HealingDone;
            public int GadgetUses;
            public int AbilityUses;
            public int OvertimeDeposits;
            public float ObjectiveSeconds;
            public TeamTicketPool Tickets;

            public void Reset(int tickets)
            {
                Score = 0;
                Deposits = 0;
                KOs = 0;
                Assists = 0;
                CrownPickups = 0;
                Deaths = 0;
                DamageDealt = 0;
                HealingDone = 0;
                GadgetUses = 0;
                AbilityUses = 0;
                OvertimeDeposits = 0;
                ObjectiveSeconds = 0f;
                Tickets = new TeamTicketPool(TeamId, Math.Max(0, tickets), Math.Max(0, tickets), 0);
            }

            public void SpendTicket() => Tickets = Tickets.Spend();

            public TeamScore ToScore() => new TeamScore(
                TeamId,
                Score,
                Deposits,
                KOs,
                Assists,
                CrownPickups,
                Tickets.Spent,
                ObjectiveSeconds,
                Deaths,
                DamageDealt,
                HealingDone,
                GadgetUses,
                AbilityUses);
        }
    }
}
