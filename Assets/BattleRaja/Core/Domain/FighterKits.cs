using System;

namespace BattleRaja.Core.Domain
{
    public enum FighterSpecialKind
    {
        ChargeThrow = 1,
        Decoy = 2
    }

    public readonly struct FighterSpecialDefinition
    {
        public FighterSpecialDefinition(ContentId abilityId, FighterSpecialKind kind, float cooldownSeconds, float durationSeconds, float radius, int magnitude)
        {
            AbilityId = abilityId;
            Kind = kind;
            CooldownSeconds = cooldownSeconds;
            DurationSeconds = durationSeconds;
            Radius = radius;
            Magnitude = magnitude;
        }

        public ContentId AbilityId { get; }
        public FighterSpecialKind Kind { get; }
        public float CooldownSeconds { get; }
        public float DurationSeconds { get; }
        public float Radius { get; }
        public int Magnitude { get; }

        public bool IsValid(out string reason)
        {
            if (!AbilityId.IsValid || AbilityId.Kind != ContentIdKind.Ability ||
                CooldownSeconds <= 0f || DurationSeconds <= 0f || Radius <= 0f || Magnitude <= 0)
            {
                reason = "Special definition values are invalid.";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public static FighterSpecialDefinition PehelChargeThrow => new FighterSpecialDefinition(
            ContentId.Ability("ability.pehel.charge_throw"), FighterSpecialKind.ChargeThrow, 6f, 0.35f, 2.2f, 3);

        public static FighterSpecialDefinition MayaDecoy => new FighterSpecialDefinition(
            ContentId.Ability("ability.maya.decoy"), FighterSpecialKind.Decoy, 9f, 4.5f, 0.5f, 35);
    }

    public enum ChargeThrowState
    {
        Ready = 0,
        Startup = 1,
        Active = 2,
        Grabbed = 3,
        Recovery = 4,
        Cooldown = 5
    }

    public readonly struct ChargeThrowStep
    {
        public ChargeThrowStep(ChargeThrowState state, Float2 displacement, CombatEntityId capturedTargetId, bool throwTriggered, bool blocked)
        {
            State = state;
            Displacement = displacement;
            CapturedTargetId = capturedTargetId;
            ThrowTriggered = throwTriggered;
            Blocked = blocked;
        }

        public ChargeThrowState State { get; }
        public Float2 Displacement { get; }
        public CombatEntityId CapturedTargetId { get; }
        public bool ThrowTriggered { get; }
        public bool Blocked { get; }
    }

    public sealed class ChargeThrowRuntime
    {
        private const float Epsilon = 0.000001f;
        private readonly FighterSpecialDefinition _definition;
        private ChargeThrowState _state = ChargeThrowState.Ready;
        private Float2 _direction = Float2.Up;
        private float _phaseRemaining;
        private float _cooldownRemaining;
        private CombatEntityId _capturedTargetId;
        private bool _hasCapturedTarget;

        public ChargeThrowRuntime(FighterSpecialDefinition definition)
        {
            var reason = string.Empty;
            if (definition.Kind != FighterSpecialKind.ChargeThrow || !definition.IsValid(out reason))
            {
                throw new ArgumentException(reason, nameof(definition));
            }

            _definition = definition;
        }

        public FighterSpecialDefinition Definition => _definition;
        public ChargeThrowState State => _state;
        public bool IsMovementLocked => _state != ChargeThrowState.Ready && _state != ChargeThrowState.Cooldown;
        public float CooldownRemaining => _cooldownRemaining;
        public float PhaseRemaining => _phaseRemaining;
        public Float2 Direction => _direction;
        public CombatEntityId CapturedTargetId => _capturedTargetId;
        public bool HasCapturedTarget => _hasCapturedTarget;

        public bool TryStart(AbilityCommand command, Float2 movement, Float2 facing)
        {
            if (!command.Pressed || !command.AbilityId.Equals(_definition.AbilityId) || _state != ChargeThrowState.Ready)
            {
                return false;
            }

            _direction = ResolveDirection(command.RequestedDirection, movement, facing);
            _phaseRemaining = Math.Max(0.08f, _definition.DurationSeconds * 0.35f);
            _cooldownRemaining = _definition.CooldownSeconds;
            _capturedTargetId = default(CombatEntityId);
            _hasCapturedTarget = false;
            _state = ChargeThrowState.Startup;
            return true;
        }

        public bool TryCaptureTarget(CombatEntityId targetId, CombatFaction sourceFaction, CombatFaction targetFaction, float distance)
        {
            if (_state != ChargeThrowState.Active || _hasCapturedTarget || targetId.Value <= 0 ||
                sourceFaction == targetFaction || distance < 0f || distance > _definition.Radius)
            {
                return false;
            }

            _capturedTargetId = targetId;
            _hasCapturedTarget = true;
            _state = ChargeThrowState.Grabbed;
            _phaseRemaining = Math.Max(0.08f, _definition.DurationSeconds * 0.30f);
            return true;
        }

        public ChargeThrowStep Step(float deltaSeconds, float availableDistance)
        {
            if (deltaSeconds < 0f || float.IsNaN(deltaSeconds) || float.IsInfinity(deltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (availableDistance < 0f || float.IsNaN(availableDistance) || float.IsInfinity(availableDistance))
            {
                throw new ArgumentOutOfRangeException(nameof(availableDistance));
            }

            _cooldownRemaining = Math.Max(0f, _cooldownRemaining - deltaSeconds);
            if (_state == ChargeThrowState.Ready || _state == ChargeThrowState.Cooldown)
            {
                if (_state == ChargeThrowState.Cooldown && _cooldownRemaining <= Epsilon) _state = ChargeThrowState.Ready;
                return new ChargeThrowStep(_state, Float2.Zero, _capturedTargetId, false, false);
            }

            var remaining = deltaSeconds;
            var displacement = Float2.Zero;
            var blocked = false;
            var throwTriggered = false;
            while (remaining > Epsilon && IsMovementLocked)
            {
                if (_phaseRemaining > remaining + Epsilon)
                {
                    if (_state == ChargeThrowState.Active)
                    {
                        displacement += Move(remaining, ref availableDistance, ref blocked);
                    }

                    _phaseRemaining -= remaining;
                    remaining = 0f;
                    break;
                }

                var slice = _phaseRemaining;
                if (_state == ChargeThrowState.Active && slice > Epsilon)
                {
                    displacement += Move(slice, ref availableDistance, ref blocked);
                }

                remaining -= slice;
                if (_state == ChargeThrowState.Grabbed)
                {
                    throwTriggered = true;
                    _state = ChargeThrowState.Recovery;
                    _phaseRemaining = Math.Max(0.05f, _definition.DurationSeconds * 0.35f);
                }
                else if (_state == ChargeThrowState.Startup)
                {
                    _state = ChargeThrowState.Active;
                    _phaseRemaining = Math.Max(0.08f, _definition.DurationSeconds * 0.35f);
                }
                else if (_state == ChargeThrowState.Active)
                {
                    _state = ChargeThrowState.Recovery;
                    _phaseRemaining = Math.Max(0.05f, _definition.DurationSeconds * 0.30f);
                }
                else if (_state == ChargeThrowState.Recovery)
                {
                    _state = _cooldownRemaining > Epsilon ? ChargeThrowState.Cooldown : ChargeThrowState.Ready;
                    _phaseRemaining = 0f;
                }

                if (blocked) remaining = 0f;
            }

            return new ChargeThrowStep(_state, displacement, _capturedTargetId, throwTriggered, blocked);
        }

        public void Reset()
        {
            _state = ChargeThrowState.Ready;
            _direction = Float2.Up;
            _phaseRemaining = 0f;
            _cooldownRemaining = 0f;
            _capturedTargetId = default(CombatEntityId);
            _hasCapturedTarget = false;
        }

        private Float2 Move(float seconds, ref float availableDistance, ref bool blocked)
        {
            var speed = _definition.Magnitude / Math.Max(Epsilon, _definition.DurationSeconds * 0.35f);
            var intended = speed * seconds;
            var actual = Math.Min(intended, availableDistance);
            availableDistance -= actual;
            if (actual + Epsilon < intended)
            {
                blocked = true;
                _state = _cooldownRemaining > Epsilon ? ChargeThrowState.Cooldown : ChargeThrowState.Ready;
                _phaseRemaining = 0f;
            }

            return _direction * actual;
        }

        private static Float2 ResolveDirection(Float2 requested, Float2 movement, Float2 facing)
        {
            if (requested.SqrMagnitude > Epsilon) return requested.Normalized;
            if (movement.SqrMagnitude > Epsilon) return movement.Normalized;
            if (facing.SqrMagnitude > Epsilon) return facing.Normalized;
            return Float2.Up;
        }
    }

    public sealed class DecoyRuntime
    {
        private float _remaining;
        private float _cooldownRemaining;
        private Float2 _position;
        private CombatEntityId _ownerId;
        private int _maxHealth;
        private int _health;

        public bool IsActive => _remaining > 0f;
        public Float2 Position => _position;
        public CombatEntityId OwnerId => _ownerId;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => Math.Max(0, _health);
        public bool IsTargetable => IsActive && _health > 0;
        public float RemainingSeconds => Math.Max(0f, _remaining);
        public float CooldownRemaining => Math.Max(0f, _cooldownRemaining);

        public bool TrySpawn(Float2 position, FighterSpecialDefinition definition)
        {
            if (definition.Kind != FighterSpecialKind.Decoy || IsActive || _cooldownRemaining > 0f) return false;
            _position = position;
            _remaining = definition.DurationSeconds;
            _cooldownRemaining = definition.CooldownSeconds;
            _ownerId = default(CombatEntityId);
            _maxHealth = definition.Magnitude;
            _health = definition.Magnitude;
            return true;
        }

        public bool TrySpawn(CombatEntityId ownerId, Float2 position, FighterSpecialDefinition definition)
        {
            if (!TrySpawn(position, definition)) return false;
            _ownerId = ownerId;
            return true;
        }

        public void Advance(float deltaSeconds, Float2 followPosition)
        {
            _cooldownRemaining = Math.Max(0f, _cooldownRemaining - Math.Max(0f, deltaSeconds));
            if (!IsActive) return;
            _remaining = Math.Max(0f, _remaining - Math.Max(0f, deltaSeconds));
            _position = _position + (followPosition - _position) * Math.Min(1f, Math.Max(0f, deltaSeconds) * 2f);
        }

        public bool TryDamage(int amount)
        {
            if (!IsTargetable || amount <= 0) return false;
            _health = Math.Max(0, _health - amount);
            if (_health == 0) _remaining = 0f;
            return true;
        }

        public void Destroy()
        {
            _remaining = 0f;
            _health = 0;
        }
    }
}
