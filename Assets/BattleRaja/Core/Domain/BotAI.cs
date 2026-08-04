using System;

namespace BattleRaja.Core.Domain
{
    public enum BotDecisionState
    {
        Explore = 0,
        Engage = 1,
        Reposition = 2,
        Retreat = 3,
        Recover = 4
    }

    public readonly struct BotDifficultyProfile
    {
        public BotDifficultyProfile(
            int reactionDelayTicks,
            float aimNoise,
            float retreatHealthFraction,
            float preferredRange,
            float decisionIntervalSeconds,
            float stuckTimeoutSeconds)
        {
            ReactionDelayTicks = reactionDelayTicks;
            AimNoise = aimNoise;
            RetreatHealthFraction = retreatHealthFraction;
            PreferredRange = preferredRange;
            DecisionIntervalSeconds = decisionIntervalSeconds;
            StuckTimeoutSeconds = stuckTimeoutSeconds;
        }

        public int ReactionDelayTicks { get; }
        public float AimNoise { get; }
        public float RetreatHealthFraction { get; }
        public float PreferredRange { get; }
        public float DecisionIntervalSeconds { get; }
        public float StuckTimeoutSeconds { get; }

        public static BotDifficultyProfile FairDefault => new BotDifficultyProfile(
            reactionDelayTicks: 8,
            aimNoise: 0.10f,
            retreatHealthFraction: 0.22f,
            preferredRange: 5.5f,
            decisionIntervalSeconds: 0.16f,
            stuckTimeoutSeconds: 0.7f);

        public bool IsValid(out string reason)
        {
            if (ReactionDelayTicks < 0 || AimNoise < 0f || AimNoise > 1f ||
                RetreatHealthFraction < 0f || RetreatHealthFraction > 1f ||
                PreferredRange <= 0f || DecisionIntervalSeconds <= 0f || StuckTimeoutSeconds <= 0f)
            {
                reason = "Bot profile values are outside their valid ranges.";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }

    public readonly struct BotObservedTarget
    {
        public BotObservedTarget(CombatEntityId id, CombatFaction faction, Float2 position, int currentHealth, bool hasLineOfSight)
        {
            Id = id;
            Faction = faction;
            Position = position;
            CurrentHealth = currentHealth;
            HasLineOfSight = hasLineOfSight;
        }

        public CombatEntityId Id { get; }
        public CombatFaction Faction { get; }
        public Float2 Position { get; }
        public int CurrentHealth { get; }
        public bool HasLineOfSight { get; }
    }

    public readonly struct BotZoneObservation
    {
        public BotZoneObservation(Float2 currentCenter, float currentRadius, Float2 nextCenter, float nextRadius)
        {
            CurrentCenter = currentCenter;
            CurrentRadius = currentRadius;
            NextCenter = nextCenter;
            NextRadius = nextRadius;
        }

        public Float2 CurrentCenter { get; }
        public float CurrentRadius { get; }
        public Float2 NextCenter { get; }
        public float NextRadius { get; }

        public bool IsOutsideCurrent(Float2 position) => CurrentRadius > 0f && CurrentRadius < float.MaxValue && position.SqrMagnitudeFrom(CurrentCenter) > CurrentRadius * CurrentRadius;
        public bool IsOutsideNext(Float2 position) => NextRadius > 0f && NextRadius < float.MaxValue && position.SqrMagnitudeFrom(NextCenter) > NextRadius * NextRadius;

        public static BotZoneObservation Unbounded => new BotZoneObservation(Float2.Zero, float.MaxValue, Float2.Zero, float.MaxValue);
    }

    public readonly struct BotPerceptionSnapshot
    {
        public BotPerceptionSnapshot(CombatEntityId selfId, Float2 position, int currentHealth, int maxHealth, BotObservedTarget[] targets, int targetCount = -1)
            : this(selfId, position, currentHealth, maxHealth, targets, targetCount, BotZoneObservation.Unbounded)
        {
        }

        public BotPerceptionSnapshot(CombatEntityId selfId, Float2 position, int currentHealth, int maxHealth, BotObservedTarget[] targets, int targetCount, BotZoneObservation zone)
        {
            SelfId = selfId;
            Position = position;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Targets = targets ?? Array.Empty<BotObservedTarget>();
            TargetCount = targetCount < 0 ? Targets.Length : Math.Min(targetCount, Targets.Length);
            Zone = zone;
        }

        public CombatEntityId SelfId { get; }
        public Float2 Position { get; }
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public BotObservedTarget[] Targets { get; }
        public int TargetCount { get; }
        public BotZoneObservation Zone { get; }
    }

    public readonly struct BotDecision
    {
        public BotDecision(
            BotDecisionState state,
            CombatEntityId targetId,
            Float2 movement,
            Float2 aim,
            bool attack,
            bool ability,
            float utilityScore,
            int perceivedThreats,
            bool stuckRecovery)
        {
            State = state;
            TargetId = targetId;
            Movement = movement;
            Aim = aim;
            Attack = attack;
            Ability = ability;
            UtilityScore = utilityScore;
            PerceivedThreats = perceivedThreats;
            StuckRecovery = stuckRecovery;
        }

        public BotDecisionState State { get; }
        public CombatEntityId TargetId { get; }
        public Float2 Movement { get; }
        public Float2 Aim { get; }
        public bool Attack { get; }
        public bool Ability { get; }
        public float UtilityScore { get; }
        public int PerceivedThreats { get; }
        public bool StuckRecovery { get; }
    }

    public sealed class SeededRandom : ISeededRandom
    {
        private uint _state;

        public SeededRandom(uint seed)
        {
            _state = seed == 0u ? 0x6D2B79F5u : seed;
        }

        public uint NextUInt()
        {
            var value = _state;
            value ^= value << 13;
            value ^= value >> 17;
            value ^= value << 5;
            _state = value == 0u ? 0x6D2B79F5u : value;
            return _state;
        }

        public float NextFloat() => (NextUInt() & 0x00FFFFFFu) / 16777216f;
        public float NextSigned() => (NextFloat() * 2f) - 1f;
    }

    public sealed class BotNavigationRecovery
    {
        private Float2 _lastPosition;
        private float _stuckSeconds;
        private bool _initialized;

        public bool IsStuck { get; private set; }

        public bool Observe(Float2 position, Float2 requestedMovement, float deltaSeconds, float timeoutSeconds)
        {
            if (!_initialized)
            {
                _lastPosition = position;
                _initialized = true;
                return false;
            }

            var moved = Float2.Distance(position, _lastPosition);
            _lastPosition = position;
            if (requestedMovement.SqrMagnitude > 0.04f && moved < 0.01f)
            {
                _stuckSeconds += MathF.Max(0f, deltaSeconds);
            }
            else
            {
                _stuckSeconds = 0f;
            }

            IsStuck = _stuckSeconds >= timeoutSeconds;
            return IsStuck;
        }

        public void Clear()
        {
            _stuckSeconds = 0f;
            IsStuck = false;
        }
    }

    public sealed class BotDecisionEngine
    {
        private BotDecision _lastDecision;
        private int _nextDecisionTick;

        public BotDecision CurrentDecision => _lastDecision;

        public BotDecision Decide(
            BotPerceptionSnapshot snapshot,
            int simulationTick,
            BotDifficultyProfile profile,
            ISeededRandom random,
            bool stuckRecovery)
        {
            if (simulationTick < _nextDecisionTick && _lastDecision.PerceivedThreats >= 0)
            {
                return _lastDecision;
            }

            _nextDecisionTick = simulationTick + profile.ReactionDelayTicks;
            var target = SelectTarget(snapshot, profile, out var targetScore, out var threatCount);
            var healthFraction = snapshot.MaxHealth > 0 ? (float)snapshot.CurrentHealth / snapshot.MaxHealth : 0f;
            BotDecision decision;
            if (stuckRecovery)
            {
                var recovery = new Float2(NextSigned(random), NextSigned(random)).Normalized;
                decision = new BotDecision(BotDecisionState.Recover, target.Id, recovery, recovery, false, false, 0.2f, threatCount, true);
            }
            else if (snapshot.Zone.IsOutsideCurrent(snapshot.Position) || snapshot.Zone.IsOutsideNext(snapshot.Position))
            {
                var destination = snapshot.Zone.IsOutsideCurrent(snapshot.Position)
                    ? snapshot.Zone.CurrentCenter
                    : snapshot.Zone.NextCenter;
                var toZone = (destination - snapshot.Position).Normalized;
                decision = new BotDecision(
                    BotDecisionState.Reposition,
                    default,
                    toZone,
                    toZone,
                    false,
                    false,
                    1.1f,
                    threatCount,
                    false);
            }
            else if (healthFraction <= profile.RetreatHealthFraction && target.Id.Value != 0)
            {
                var away = (snapshot.Position - target.Position).Normalized;
                decision = new BotDecision(BotDecisionState.Retreat, target.Id, away, away, false, false, 0.9f, threatCount, false);
            }
            else if (target.Id.Value != 0)
            {
                var toTarget = (target.Position - snapshot.Position).Normalized;
                var distance = Float2.Distance(snapshot.Position, target.Position);
                var aim = ApplyAimNoise(toTarget, profile.AimNoise, random);
                var movement = distance > profile.PreferredRange
                    ? toTarget
                    : new Float2(-toTarget.Y, toTarget.X) * (NextFloat(random) > 0.5f ? 1f : -1f);
                var ability = distance > profile.PreferredRange * 1.35f;
                decision = new BotDecision(
                    distance > profile.PreferredRange ? BotDecisionState.Reposition : BotDecisionState.Engage,
                    target.Id,
                    movement,
                    aim,
                    target.HasLineOfSight && distance <= profile.PreferredRange + 5f,
                    ability,
                    targetScore,
                    threatCount,
                    false);
            }
            else
            {
                var explore = new Float2(NextSigned(random), NextSigned(random)).Normalized;
                decision = new BotDecision(BotDecisionState.Explore, default, explore, explore, false, false, 0.1f, threatCount, false);
            }

            _lastDecision = decision;
            return decision;
        }

        public void Reset()
        {
            _lastDecision = default;
            _nextDecisionTick = 0;
        }

        private static BotObservedTarget SelectTarget(BotPerceptionSnapshot snapshot, BotDifficultyProfile profile, out float score, out int threatCount)
        {
            var best = default(BotObservedTarget);
            score = 0f;
            threatCount = 0;
            for (var i = 0; i < snapshot.TargetCount; i++)
            {
                var candidate = snapshot.Targets[i];
                if (candidate.Id == snapshot.SelfId || candidate.Faction == CombatFaction.Neutral || !candidate.HasLineOfSight)
                {
                    continue;
                }

                threatCount++;
                var distance = Float2.Distance(snapshot.Position, candidate.Position);
                var candidateScore = 100f / (1f + distance) + (candidate.CurrentHealth <= 25 ? 10f : 0f);
                if (candidateScore > score)
                {
                    score = candidateScore;
                    best = candidate;
                }
            }

            return best;
        }

        private static Float2 ApplyAimNoise(Float2 direction, float noise, ISeededRandom random)
        {
            if (direction.SqrMagnitude <= 0.000001f || noise <= 0f || random is not SeededRandom seeded)
            {
                return direction;
            }

            return (direction + new Float2(seeded.NextSigned(), seeded.NextSigned()) * noise).Normalized;
        }

        private static float NextFloat(ISeededRandom random)
        {
            return random is SeededRandom seeded ? seeded.NextFloat() : (random.NextUInt() & 0x00FFFFFFu) / 16777216f;
        }

        private static float NextSigned(ISeededRandom random) => (NextFloat(random) * 2f) - 1f;
    }
}
