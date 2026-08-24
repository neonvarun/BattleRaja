using System;
using System.Collections.Generic;

namespace BattleRaja.Core.Domain
{
    public enum MatchPhase
    {
        LoadWarmup = 0,
        SpawnProtection = 1,
        Opening = 2,
        Pressure = 3,
        FinalCircle = 4,
        Resolution = 5
    }

    public enum AandhiState
    {
        Stable = 0,
        Warning = 1,
        Closing = 2
    }

    public readonly struct MatchPhaseDefinition
    {
        public MatchPhaseDefinition(MatchPhase phase, float durationSeconds, float radius, int outsideDamagePerSecond, float warningSeconds = 0f)
        {
            Phase = phase;
            DurationSeconds = durationSeconds;
            Radius = radius;
            OutsideDamagePerSecond = outsideDamagePerSecond;
            WarningSeconds = Math.Max(0f, warningSeconds);
        }

        public MatchPhase Phase { get; }
        public float DurationSeconds { get; }
        public float Radius { get; }
        public int OutsideDamagePerSecond { get; }
        public float WarningSeconds { get; }
    }

    public readonly struct OfflineMatchDefinition
    {
        public OfflineMatchDefinition(float spawnProtectionSeconds, Float2 zoneCenter, MatchPhaseDefinition[] phases)
        {
            SpawnProtectionSeconds = spawnProtectionSeconds;
            ZoneCenter = zoneCenter;
            Phases = phases ?? Array.Empty<MatchPhaseDefinition>();
        }

        public float SpawnProtectionSeconds { get; }
        public Float2 ZoneCenter { get; }
        public MatchPhaseDefinition[] Phases { get; }

        public float TargetDurationSeconds
        {
            get
            {
                var total = 0f;
                for (var i = 0; i < Phases.Length; i++) total += Phases[i].DurationSeconds;
                return total;
            }
        }

        public static OfflineMatchDefinition SoloRaja => new OfflineMatchDefinition(
            spawnProtectionSeconds: 5f,
            zoneCenter: Float2.Zero,
            phases: new[]
            {
                new MatchPhaseDefinition(MatchPhase.LoadWarmup, 3f, 14f, 0),
                new MatchPhaseDefinition(MatchPhase.SpawnProtection, 5f, 14f, 0),
                // Opening holds the full arena through a long warning window so
                // looting and early rotation happen before the first squeeze
                // toward the Pressure ring (target 4-6 minute matches).
                new MatchPhaseDefinition(MatchPhase.Opening, 105f, 14f, 5, 50f),
                new MatchPhaseDefinition(MatchPhase.Pressure, 115f, 11f, 10, 25f),
                new MatchPhaseDefinition(MatchPhase.FinalCircle, 78f, 4f, 20)
            });
    }

    public readonly struct MatchSpawn
    {
        public MatchSpawn(CombatEntityId id, Float2 position, int maxHealth)
        {
            Id = id;
            Position = position;
            MaxHealth = maxHealth;
        }

        public CombatEntityId Id { get; }
        public Float2 Position { get; }
        public int MaxHealth { get; }
    }

    public readonly struct MatchParticipantSnapshot
    {
        public MatchParticipantSnapshot(
            CombatEntityId id,
            Float2 position,
            int currentHealth,
            int maxHealth,
            bool alive,
            int placement,
            int eliminations,
            int damageDealt,
            int assists,
            float survivalTimeSeconds)
        {
            Id = id;
            Position = position;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Alive = alive;
            Placement = placement;
            Eliminations = eliminations;
            DamageDealt = damageDealt;
            Assists = assists;
            SurvivalTimeSeconds = survivalTimeSeconds;
        }

        public CombatEntityId Id { get; }
        public Float2 Position { get; }
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public bool Alive { get; }
        public int Placement { get; }
        public int Eliminations { get; }
        public int DamageDealt { get; }
        public int Assists { get; }
        public float SurvivalTimeSeconds { get; }
    }

    public readonly struct MatchTickResult
    {
        public MatchTickResult(
            MatchPhase phase,
            Float2 zoneCenter,
            Float2 nextZoneCenter,
            float zoneRadius,
            float nextZoneRadius,
            AandhiState aandhiState,
            float warningRemainingSeconds,
            int outsideDamagePerSecond,
            int outsideCount,
            bool matchEnded,
            CombatEntityId winnerId)
        {
            Phase = phase;
            ZoneCenter = zoneCenter;
            NextZoneCenter = nextZoneCenter;
            ZoneRadius = zoneRadius;
            NextZoneRadius = nextZoneRadius;
            AandhiState = aandhiState;
            WarningRemainingSeconds = warningRemainingSeconds;
            OutsideDamagePerSecond = outsideDamagePerSecond;
            OutsideCount = outsideCount;
            MatchEnded = matchEnded;
            WinnerId = winnerId;
        }

        public MatchPhase Phase { get; }
        public Float2 ZoneCenter { get; }
        public Float2 NextZoneCenter { get; }
        public float ZoneRadius { get; }
        public float NextZoneRadius { get; }
        public AandhiState AandhiState { get; }
        public float WarningRemainingSeconds { get; }
        public int OutsideDamagePerSecond { get; }
        public int OutsideCount { get; }
        public bool MatchEnded { get; }
    public CombatEntityId WinnerId { get; }
    }

    public readonly struct DamageContributionSnapshot
    {
        public DamageContributionSnapshot(CombatEntityId targetId, CombatEntityId instigatorId, int amount)
        {
            TargetId = targetId;
            InstigatorId = instigatorId;
            Amount = amount;
        }

        public CombatEntityId TargetId { get; }
        public CombatEntityId InstigatorId { get; }
        public int Amount { get; }
    }

    public sealed class OfflineMatchSimulation
    {
        private readonly OfflineMatchDefinition _definition;
        private readonly List<ParticipantState> _participants = new List<ParticipantState>(8);
        private readonly Dictionary<CombatEntityId, Dictionary<CombatEntityId, int>> _damageContributions = new Dictionary<CombatEntityId, Dictionary<CombatEntityId, int>>();
        private readonly MatchEventIdentityTracker _identityTracker = new MatchEventIdentityTracker();
        private float _elapsed;
        private MatchPhase _phase = MatchPhase.LoadWarmup;
        private bool _started;
        private int _nextPlacement;
        private int _lastDamageEventId;
        private int _emittedDamageEvents;

        public OfflineMatchSimulation(OfflineMatchDefinition definition)
        {
            _definition = definition;
        }

        /// <summary>Stable identity assigned to the most recently recorded damage event.</summary>
        public int LastDamageEventId => _lastDamageEventId;

        /// <summary>Count of recorded (non-rejected) damage events; identities are sequential per match.</summary>
        public int EmittedDamageEventCount => _emittedDamageEvents;

        public MatchPhase Phase => _phase;
        public float ElapsedSeconds => _elapsed;
        public bool IsStarted => _started;
        public bool IsEnded => _phase == MatchPhase.Resolution;
        public int AliveCount
        {
            get
            {
                var count = 0;
                for (var i = 0; i < _participants.Count; i++) if (_participants[i].Alive) count++;
                return count;
            }
        }

        public void Start(IReadOnlyList<MatchSpawn> spawns)
        {
            if (_started) throw new InvalidOperationException("A match can only be started once; call Restart first.");
            if (spawns == null || spawns.Count < 2 || !SpawnPointValidator.AreSeparated(spawns))
            {
                throw new ArgumentException("A match requires separated valid spawn points.", nameof(spawns));
            }

            _participants.Clear();
            _damageContributions.Clear();
            for (var i = 0; i < spawns.Count; i++)
            {
                _participants.Add(new ParticipantState(spawns[i]));
            }

            _elapsed = 0f;
            _phase = MatchPhase.LoadWarmup;
            _nextPlacement = spawns.Count;
            _started = true;
        }

        public MatchTickResult Advance(float deltaSeconds)
        {
            if (!_started) throw new InvalidOperationException("Start the match before advancing it.");
            if (deltaSeconds < 0f || float.IsNaN(deltaSeconds) || float.IsInfinity(deltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (_phase != MatchPhase.Resolution)
            {
                _elapsed += deltaSeconds;
                var timedOut = _elapsed >= _definition.TargetDurationSeconds;
                if (!timedOut) UpdatePhase();
                if (AliveCount <= 1 || timedOut)
                {
                    ResolveWinner();
                }

                for (var i = 0; i < _participants.Count; i++)
                {
                    if (_participants[i].Alive) _participants[i].SurvivalTimeSeconds = _elapsed;
                }
            }

            var phaseDefinition = GetCurrentPhaseDefinition();
            var zoneRadius = GetInterpolatedZoneRadius();
            var nextZoneRadius = GetNextZoneRadius();
            var aandhiState = GetAandhiState(out var warningRemainingSeconds);
            var outside = 0;
            for (var i = 0; i < _participants.Count; i++)
            {
                var participant = _participants[i];
                if (participant.Alive && Float2.Distance(participant.Position, _definition.ZoneCenter) > zoneRadius)
                {
                    outside++;
                }
            }

            var winner = FindWinner();
            return new MatchTickResult(
                _phase,
                _definition.ZoneCenter,
                _definition.ZoneCenter,
                zoneRadius,
                nextZoneRadius,
                aandhiState,
                warningRemainingSeconds,
                phaseDefinition.OutsideDamagePerSecond,
                outside,
                IsEnded,
                winner);
        }

        public bool SyncHealth(CombatEntityId id, int currentHealth)
        {
            var participant = Find(id);
            if (participant == null) return false;
            participant.CurrentHealth = Math.Max(0, Math.Min(participant.MaxHealth, currentHealth));
            if (participant.Alive && participant.CurrentHealth == 0)
            {
                Eliminate(participant);
            }

            if (_started && AliveCount <= 1) ResolveWinner();
            return true;
        }

        public int Heal(CombatEntityId id, int amount)
        {
            if (amount <= 0) return 0;
            var participant = Find(id);
            if (participant == null || !participant.Alive) return 0;
            var applied = Math.Min(amount, participant.MaxHealth - participant.CurrentHealth);
            participant.CurrentHealth += applied;
            return applied;
        }

        public bool RecordDamage(CombatDamageEvent damageEvent)
        {
            if (!_started || damageEvent.AmountApplied <= 0) return false;
            var target = Find(damageEvent.TargetId);
            if (target == null || !target.Alive) return false;

            // Identity is assigned only after validation so rejected or
            // duplicate submissions never consume a stable event ID.
            _lastDamageEventId = damageEvent.EventId != 0
                ? damageEvent.EventId
                : _identityTracker.NextDamageEventId();
            _emittedDamageEvents++;

            target.CurrentHealth = Math.Max(0, Math.Min(target.MaxHealth, damageEvent.CurrentHealthAfter));
            var instigator = Find(damageEvent.InstigatorId);
            if (instigator != null && instigator.Id != target.Id)
            {
                instigator.DamageDealt = SaturatingAdd(instigator.DamageDealt, damageEvent.AmountApplied);
                if (!_damageContributions.TryGetValue(target.Id, out var contributions))
                {
                    contributions = new Dictionary<CombatEntityId, int>();
                    _damageContributions[target.Id] = contributions;
                }

                contributions.TryGetValue(instigator.Id, out var previousAmount);
                contributions[instigator.Id] = SaturatingAdd(previousAmount, damageEvent.AmountApplied);
            }

            if (target.CurrentHealth == 0)
            {
                Eliminate(target);
                if (instigator != null && instigator.Id != target.Id)
                {
                    instigator.Eliminations = SaturatingAdd(instigator.Eliminations, 1);
                    CreditAssists(target.Id, instigator.Id);
                }

                _damageContributions.Remove(target.Id);

                if (AliveCount <= 1) ResolveWinner();
            }

            return true;
        }

        /// <summary>
        /// Applies a damage request to canonical participant health and records the
        /// resulting combat statistics in the same operation.
        /// </summary>
        public DamageResult ApplyDamage(
            DamageRequest request,
            CombatFaction targetFaction,
            bool allowSelfHit,
            bool allowFriendlyFire)
        {
            if (!_started) return new DamageResult(false, 0, false, DamageRejectionReason.WrongTarget);

            var target = Find(request.TargetId);
            if (target == null)
            {
                return new DamageResult(false, 0, false, DamageRejectionReason.WrongTarget);
            }

            if (!allowSelfHit && request.InstigatorId == target.Id)
            {
                return new DamageResult(false, 0, target.CurrentHealth <= 0, DamageRejectionReason.SelfHit);
            }

            if (!allowFriendlyFire && request.InstigatorFaction != CombatFaction.Neutral &&
                request.InstigatorFaction == targetFaction)
            {
                return new DamageResult(false, 0, target.CurrentHealth <= 0, DamageRejectionReason.FriendlyFire);
            }

            if (request.RawAmount <= 0)
            {
                return new DamageResult(false, 0, target.CurrentHealth <= 0, DamageRejectionReason.InvalidAmount);
            }

            if (!target.Alive || target.CurrentHealth <= 0)
            {
                return new DamageResult(false, 0, true, DamageRejectionReason.AlreadyDefeated);
            }

            var applied = Math.Min(request.RawAmount, target.CurrentHealth);
            target.CurrentHealth -= applied;
            var defeated = target.CurrentHealth == 0;
            var result = new DamageResult(true, applied, defeated, DamageRejectionReason.None);
            RecordDamage(new CombatDamageEvent(request, applied, defeated, target.CurrentHealth, request.SimulationTick));
            return result;
        }

        public bool SetPosition(CombatEntityId id, Float2 position)
        {
            var participant = Find(id);
            if (participant == null) return false;
            participant.Position = position;
            return true;
        }

        public MatchParticipantSnapshot[] GetSnapshots()
        {
            var snapshots = new MatchParticipantSnapshot[_participants.Count];
            for (var i = 0; i < _participants.Count; i++) snapshots[i] = _participants[i].ToSnapshot();
            return snapshots;
        }

        public DamageContributionSnapshot[] GetDamageContributions()
        {
            var targets = new List<CombatEntityId>(_damageContributions.Keys);
            targets.Sort((left, right) => left.Value.CompareTo(right.Value));
            var result = new List<DamageContributionSnapshot>();
            for (var targetIndex = 0; targetIndex < targets.Count; targetIndex++)
            {
                var contributions = _damageContributions[targets[targetIndex]];
                var instigators = new List<CombatEntityId>(contributions.Keys);
                instigators.Sort((left, right) => left.Value.CompareTo(right.Value));
                for (var instigatorIndex = 0; instigatorIndex < instigators.Count; instigatorIndex++)
                {
                    result.Add(new DamageContributionSnapshot(
                        targets[targetIndex],
                        instigators[instigatorIndex],
                        contributions[instigators[instigatorIndex]]));
                }
            }

            return result.ToArray();
        }

        public bool TryGetSnapshot(CombatEntityId id, out MatchParticipantSnapshot snapshot)
        {
            var participant = Find(id);
            if (participant == null)
            {
                snapshot = default(MatchParticipantSnapshot);
                return false;
            }

            snapshot = participant.ToSnapshot();
            return true;
        }

        public void Restart()
        {
            _participants.Clear();
            _damageContributions.Clear();
            _identityTracker.Reset();
            _elapsed = 0f;
            _phase = MatchPhase.LoadWarmup;
            _nextPlacement = 0;
            _started = false;
            _lastDamageEventId = 0;
            _emittedDamageEvents = 0;
        }

        private void UpdatePhase()
        {
            var phaseIndex = FindPhaseIndex(_elapsed);
            _phase = phaseIndex >= 0 ? _definition.Phases[phaseIndex].Phase : MatchPhase.Resolution;
        }

        private MatchPhaseDefinition GetCurrentPhaseDefinition()
        {
            if (_definition.Phases.Length == 0) return new MatchPhaseDefinition(MatchPhase.Resolution, 0f, 0f, 0);
            for (var i = _definition.Phases.Length - 1; i >= 0; i--)
            {
                if (_definition.Phases[i].Phase == _phase) return _definition.Phases[i];
            }

            return _definition.Phases[_definition.Phases.Length - 1];
        }

        private float GetInterpolatedZoneRadius()
        {
            if (_definition.Phases.Length == 0) return 0f;

            var phaseIndex = FindPhaseIndex(_elapsed);
            if (phaseIndex < 0) return _definition.Phases[_definition.Phases.Length - 1].Radius;

            var phase = _definition.Phases[phaseIndex];
            var next = phaseIndex + 1 < _definition.Phases.Length
                ? _definition.Phases[phaseIndex + 1]
                : phase;
            var phaseStart = 0f;
            for (var i = 0; i < phaseIndex; i++) phaseStart += _definition.Phases[i].DurationSeconds;
            var closingDuration = MathF.Max(0f, phase.DurationSeconds - MathF.Min(phase.DurationSeconds, phase.WarningSeconds));
            var progress = closingDuration > 0f
                ? MathF.Max(0f, MathF.Min(1f, (_elapsed - phaseStart - phase.WarningSeconds) / closingDuration))
                : 1f;
            return phase.Radius + ((next.Radius - phase.Radius) * progress);
        }

        private AandhiState GetAandhiState(out float warningRemainingSeconds)
        {
            warningRemainingSeconds = 0f;
            var phaseIndex = FindPhaseIndex(_elapsed);
            if (phaseIndex < 0 || phaseIndex + 1 >= _definition.Phases.Length) return AandhiState.Stable;
            var phase = _definition.Phases[phaseIndex];
            var next = _definition.Phases[phaseIndex + 1];
            if (MathF.Abs(next.Radius - phase.Radius) <= 0.0001f) return AandhiState.Stable;

            var phaseStart = 0f;
            for (var i = 0; i < phaseIndex; i++) phaseStart += _definition.Phases[i].DurationSeconds;
            var phaseElapsed = MathF.Max(0f, _elapsed - phaseStart);
            if (phaseElapsed < phase.WarningSeconds)
            {
                warningRemainingSeconds = phase.WarningSeconds - phaseElapsed;
                return AandhiState.Warning;
            }

            return AandhiState.Closing;
        }

        private float GetNextZoneRadius()
        {
            if (_definition.Phases.Length == 0) return 0f;
            var phaseIndex = FindPhaseIndex(_elapsed);
            if (phaseIndex < 0 || phaseIndex + 1 >= _definition.Phases.Length)
            {
                return _definition.Phases[_definition.Phases.Length - 1].Radius;
            }

            return _definition.Phases[phaseIndex + 1].Radius;
        }

        private int FindPhaseIndex(float elapsedSeconds)
        {
            var phaseStart = 0f;
            for (var i = 0; i < _definition.Phases.Length; i++)
            {
                var phaseEnd = phaseStart + _definition.Phases[i].DurationSeconds;
                if (elapsedSeconds < phaseEnd) return i;
                phaseStart = phaseEnd;
            }

            return -1;
        }

        private void ResolveWinner()
        {
            if (_phase == MatchPhase.Resolution) return;

            // Timeout results must never depend on spawn/list order. Living participants
            // are ranked by the documented deterministic rule, then receive every
            // remaining placement. Eliminated placements were already assigned by
            // Eliminate, so the ranges cannot overlap.
            var living = new List<ParticipantState>();
            for (var i = 0; i < _participants.Count; i++)
            {
                if (_participants[i].Alive) living.Add(_participants[i]);
            }

            living.Sort(CompareTimeoutRanking);
            for (var i = 0; i < living.Count; i++) living[i].Placement = i + 1;

            _phase = MatchPhase.Resolution;
        }

        private int CompareTimeoutRanking(ParticipantState left, ParticipantState right)
        {
            // Timeout ranking order: alive status (all candidates are alive here),
            // current-health percentage, eliminations, damage dealt, distance to the final zone
            // centre, then ascending entity id as the deterministic final tie-break.
            var leftHealth = (long)left.CurrentHealth * right.MaxHealth;
            var rightHealth = (long)right.CurrentHealth * left.MaxHealth;
            if (leftHealth != rightHealth) return leftHealth > rightHealth ? -1 : 1;
            if (left.Eliminations != right.Eliminations) return right.Eliminations.CompareTo(left.Eliminations);
            if (left.DamageDealt != right.DamageDealt) return right.DamageDealt.CompareTo(left.DamageDealt);

            var leftDistance = Float2.Distance(left.Position, _definition.ZoneCenter);
            var rightDistance = Float2.Distance(right.Position, _definition.ZoneCenter);
            var distanceComparison = leftDistance.CompareTo(rightDistance);
            if (distanceComparison != 0) return distanceComparison;
            return left.Id.Value.CompareTo(right.Id.Value);
        }

        private void Eliminate(ParticipantState participant)
        {
            if (!participant.Alive) return;
            participant.Alive = false;
            participant.Placement = _nextPlacement--;
            participant.SurvivalTimeSeconds = _elapsed;
        }

        private void CreditAssists(CombatEntityId targetId, CombatEntityId finishingInstigatorId)
        {
            if (!_damageContributions.TryGetValue(targetId, out var contributions)) return;

            foreach (var contribution in contributions)
            {
                if (contribution.Key == finishingInstigatorId) continue;
                var participant = Find(contribution.Key);
                if (participant == null || !participant.Alive) continue;
                participant.Assists = SaturatingAdd(participant.Assists, 1);
            }
        }

        private CombatEntityId FindWinner()
        {
            for (var i = 0; i < _participants.Count; i++)
            {
                if (_participants[i].Placement == 1) return _participants[i].Id;
            }

            ParticipantState best = null;
            for (var i = 0; i < _participants.Count; i++)
            {
                var candidate = _participants[i];
                if (!candidate.Alive) continue;
                if (best == null || CompareTimeoutRanking(candidate, best) < 0) best = candidate;
            }

            return best != null ? best.Id : default;
        }

        private ParticipantState Find(CombatEntityId id)
        {
            for (var i = 0; i < _participants.Count; i++) if (_participants[i].Id == id) return _participants[i];
            return null;
        }

        private sealed class ParticipantState
        {
            public ParticipantState(MatchSpawn spawn)
            {
                Id = spawn.Id;
                Position = spawn.Position;
                MaxHealth = Math.Max(1, spawn.MaxHealth);
                CurrentHealth = MaxHealth;
                Alive = true;
                SurvivalTimeSeconds = 0f;
            }

            public CombatEntityId Id;
            public Float2 Position;
            public int CurrentHealth;
            public int MaxHealth;
            public bool Alive;
            public int Placement;
            public int Eliminations;
            public int DamageDealt;
            public int Assists;
            public float SurvivalTimeSeconds;

            public MatchParticipantSnapshot ToSnapshot() => new MatchParticipantSnapshot(
                Id,
                Position,
                CurrentHealth,
                MaxHealth,
                Alive,
                Placement,
                Eliminations,
                DamageDealt,
                Assists,
                SurvivalTimeSeconds);
        }

        private static int SaturatingAdd(int value, int amount)
        {
            if (amount > 0 && value > int.MaxValue - amount) return int.MaxValue;
            return value + amount;
        }
    }

    public static class SpawnPointValidator
    {
        public static bool AreSeparated(IReadOnlyList<MatchSpawn> spawns, float minimumDistance = 2.5f)
        {
            if (spawns == null || spawns.Count < 2 || minimumDistance <= 0f) return false;
            for (var i = 0; i < spawns.Count; i++)
            {
                if (spawns[i].MaxHealth <= 0 || spawns[i].Position.SqrMagnitude > 400f) return false;
                for (var j = i + 1; j < spawns.Count; j++)
                {
                    if (Float2.Distance(spawns[i].Position, spawns[j].Position) < minimumDistance) return false;
                }
            }

            return true;
        }
    }

    public static class SpectatorTargetSelector
    {
        public static CombatEntityId SelectNext(MatchParticipantSnapshot[] snapshots, CombatEntityId current)
        {
            if (snapshots == null || snapshots.Length == 0) return default;
            var best = default(CombatEntityId);
            var foundCurrent = current.Value == 0;
            for (var i = 0; i < snapshots.Length; i++)
            {
                if (!snapshots[i].Alive) continue;
                if (foundCurrent) return snapshots[i].Id;
                if (snapshots[i].Id == current) foundCurrent = true;
                best = snapshots[i].Id;
            }

            return best;
        }
    }
}
