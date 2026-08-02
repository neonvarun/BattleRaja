namespace BattleRaja.Core.Domain
{
    public enum FighterActionState
    {
        Ready = 0,
        Startup = 1,
        Active = 2,
        Recovery = 3,
        Cooldown = 4
    }

    public readonly struct DashStep
    {
        public DashStep(FighterActionState state, Float2 displacement, bool completed, bool blocked)
        {
            State = state;
            Displacement = displacement;
            Completed = completed;
            Blocked = blocked;
        }

        public FighterActionState State { get; }
        public Float2 Displacement { get; }
        public bool Completed { get; }
        public bool Blocked { get; }
    }

    public sealed class FighterRuntimeState
    {
        private const float Epsilon = 0.000001f;
        private FighterActionState _state = FighterActionState.Ready;
        private Float2 _direction = Float2.Up;
        private float _phaseRemaining;
        private float _cooldownRemaining;
        private float _distanceTravelled;

        public FighterRuntimeState(FighterDefinition definition)
        {
            Definition = definition;
        }

        public FighterDefinition Definition { get; }
        public FighterActionState ActionState => _state;
        public float CooldownRemaining => _cooldownRemaining;
        public float DistanceTravelled => _distanceTravelled;
        public Float2 DashDirection => _direction;

        public bool TryStartDash(AbilityCommand command, Float2 movement, Float2 facing)
        {
            if (!command.Pressed || !command.AbilityId.Equals(Definition.Ability.AbilityId) || _state != FighterActionState.Ready)
            {
                return false;
            }

            _direction = ResolveDirection(command.RequestedDirection, movement, facing);
            _phaseRemaining = Definition.Ability.StartupSeconds;
            _distanceTravelled = 0f;
            _state = _phaseRemaining > Epsilon ? FighterActionState.Startup : FighterActionState.Active;
            _cooldownRemaining = Definition.Ability.CooldownSeconds;
            if (_state == FighterActionState.Active)
            {
                _phaseRemaining = Definition.Ability.ActiveSeconds;
            }

            return true;
        }

        public DashStep Step(float deltaSeconds, float availableDistance)
        {
            if (deltaSeconds < 0f || float.IsNaN(deltaSeconds) || float.IsInfinity(deltaSeconds))
            {
                throw new System.ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (availableDistance < 0f || float.IsNaN(availableDistance) || float.IsInfinity(availableDistance))
            {
                throw new System.ArgumentOutOfRangeException(nameof(availableDistance));
            }

            _cooldownRemaining = System.MathF.Max(0f, _cooldownRemaining - deltaSeconds);
            if (_state == FighterActionState.Ready || _state == FighterActionState.Cooldown)
            {
                if (_cooldownRemaining <= Epsilon)
                {
                    _state = FighterActionState.Ready;
                }

                return new DashStep(_state, Float2.Zero, false, false);
            }

            var remaining = deltaSeconds;
            var displacement = Float2.Zero;
            var blocked = false;
            while (remaining > Epsilon && _state != FighterActionState.Ready && _state != FighterActionState.Cooldown)
            {
                if (_phaseRemaining > remaining + Epsilon)
                {
                    if (_state == FighterActionState.Active)
                    {
                        displacement += MoveActive(remaining, ref availableDistance, ref blocked);
                    }

                    _phaseRemaining -= remaining;
                    remaining = 0f;
                    break;
                }

                var phaseSlice = _phaseRemaining;
                if (_state == FighterActionState.Active && phaseSlice > Epsilon)
                {
                    displacement += MoveActive(phaseSlice, ref availableDistance, ref blocked);
                }

                remaining -= phaseSlice;
                AdvancePhase();
                if (blocked)
                {
                    remaining = 0f;
                }
            }

            var completed = _state == FighterActionState.Ready || _state == FighterActionState.Cooldown;
            return new DashStep(_state, displacement, completed, blocked);
        }

        public void Reset()
        {
            _state = FighterActionState.Ready;
            _direction = Float2.Up;
            _phaseRemaining = 0f;
            _cooldownRemaining = 0f;
            _distanceTravelled = 0f;
        }

        private Float2 MoveActive(float seconds, ref float availableDistance, ref bool blocked)
        {
            var speed = Definition.Ability.Distance / Definition.Ability.ActiveSeconds;
            var intended = speed * seconds;
            var actual = System.MathF.Min(intended, availableDistance);
            availableDistance -= actual;
            _distanceTravelled += actual;
            if (actual + Epsilon < intended)
            {
                blocked = true;
                _phaseRemaining = 0f;
                _state = _cooldownRemaining > Epsilon ? FighterActionState.Cooldown : FighterActionState.Ready;
            }

            return _direction * actual;
        }

        private void AdvancePhase()
        {
            if (_state == FighterActionState.Startup)
            {
                _state = FighterActionState.Active;
                _phaseRemaining = Definition.Ability.ActiveSeconds;
            }
            else if (_state == FighterActionState.Active)
            {
                _state = FighterActionState.Recovery;
                _phaseRemaining = Definition.Ability.RecoverySeconds;
            }
            else if (_state == FighterActionState.Recovery)
            {
                _state = _cooldownRemaining > Epsilon ? FighterActionState.Cooldown : FighterActionState.Ready;
                _phaseRemaining = 0f;
            }
        }

        private static Float2 ResolveDirection(Float2 requested, Float2 movement, Float2 facing)
        {
            if (requested.SqrMagnitude > Epsilon)
            {
                return requested.Normalized;
            }

            if (movement.SqrMagnitude > Epsilon)
            {
                return movement.Normalized;
            }

            if (facing.SqrMagnitude > Epsilon)
            {
                return facing.Normalized;
            }

            return Float2.Up;
        }
    }
}
