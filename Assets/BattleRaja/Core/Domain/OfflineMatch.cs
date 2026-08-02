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

    public readonly struct MatchPhaseDefinition
    {
        public MatchPhaseDefinition(MatchPhase phase, float durationSeconds, float radius, int outsideDamagePerSecond)
        {
            Phase = phase;
            DurationSeconds = durationSeconds;
            Radius = radius;
            OutsideDamagePerSecond = outsideDamagePerSecond;
        }

        public MatchPhase Phase { get; }
        public float DurationSeconds { get; }
        public float Radius { get; }
        public int OutsideDamagePerSecond { get; }
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
                new MatchPhaseDefinition(MatchPhase.Opening, 90f, 14f, 5),
                new MatchPhaseDefinition(MatchPhase.Pressure, 120f, 8f, 10),
                new MatchPhaseDefinition(MatchPhase.FinalCircle, 80f, 3.5f, 20)
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
        public MatchParticipantSnapshot(CombatEntityId id, Float2 position, int currentHealth, int maxHealth, bool alive, int placement, int eliminations)
        {
            Id = id;
            Position = position;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Alive = alive;
            Placement = placement;
            Eliminations = eliminations;
        }

        public CombatEntityId Id { get; }
        public Float2 Position { get; }
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public bool Alive { get; }
        public int Placement { get; }
        public int Eliminations { get; }
    }

    public readonly struct MatchTickResult
    {
        public MatchTickResult(MatchPhase phase, Float2 zoneCenter, float zoneRadius, int outsideDamagePerSecond, int outsideCount, bool matchEnded, CombatEntityId winnerId)
        {
            Phase = phase;
            ZoneCenter = zoneCenter;
            ZoneRadius = zoneRadius;
            OutsideDamagePerSecond = outsideDamagePerSecond;
            OutsideCount = outsideCount;
            MatchEnded = matchEnded;
            WinnerId = winnerId;
        }

        public MatchPhase Phase { get; }
        public Float2 ZoneCenter { get; }
        public float ZoneRadius { get; }
        public int OutsideDamagePerSecond { get; }
        public int OutsideCount { get; }
        public bool MatchEnded { get; }
        public CombatEntityId WinnerId { get; }
    }

    public sealed class OfflineMatchSimulation
    {
        private readonly OfflineMatchDefinition _definition;
        private readonly List<ParticipantState> _participants = new List<ParticipantState>(8);
        private float _elapsed;
        private MatchPhase _phase = MatchPhase.LoadWarmup;
        private bool _started;
        private int _nextPlacement;

        public OfflineMatchSimulation(OfflineMatchDefinition definition)
        {
            _definition = definition;
        }

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
            }

            var phaseDefinition = GetCurrentPhaseDefinition();
            var outside = 0;
            for (var i = 0; i < _participants.Count; i++)
            {
                var participant = _participants[i];
                if (participant.Alive && Float2.Distance(participant.Position, _definition.ZoneCenter) > phaseDefinition.Radius)
                {
                    outside++;
                }
            }

            var winner = FindWinner();
            return new MatchTickResult(_phase, _definition.ZoneCenter, phaseDefinition.Radius, phaseDefinition.OutsideDamagePerSecond, outside, IsEnded, winner);
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

        public void Restart()
        {
            _participants.Clear();
            _elapsed = 0f;
            _phase = MatchPhase.LoadWarmup;
            _nextPlacement = 0;
            _started = false;
        }

        private void UpdatePhase()
        {
            var elapsed = 0f;
            var next = MatchPhase.Resolution;
            for (var i = 0; i < _definition.Phases.Length; i++)
            {
                elapsed += _definition.Phases[i].DurationSeconds;
                if (_elapsed < elapsed)
                {
                    next = _definition.Phases[i].Phase;
                    break;
                }
            }

            _phase = next;
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
            // current-health percentage, eliminations, distance to the final zone
            // centre, then ascending entity id as the deterministic final tie-break.
            var leftHealth = (long)left.CurrentHealth * right.MaxHealth;
            var rightHealth = (long)right.CurrentHealth * left.MaxHealth;
            if (leftHealth != rightHealth) return leftHealth > rightHealth ? -1 : 1;
            if (left.Eliminations != right.Eliminations) return right.Eliminations.CompareTo(left.Eliminations);

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
        }

        private CombatEntityId FindWinner()
        {
            for (var i = 0; i < _participants.Count; i++)
            {
                if (_participants[i].Alive) return _participants[i].Id;
            }

            return default;
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
            }

            public CombatEntityId Id;
            public Float2 Position;
            public int CurrentHealth;
            public int MaxHealth;
            public bool Alive;
            public int Placement;
            public int Eliminations;

            public MatchParticipantSnapshot ToSnapshot() => new MatchParticipantSnapshot(Id, Position, CurrentHealth, MaxHealth, Alive, Placement, Eliminations);
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
