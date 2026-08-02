using System;

namespace BattleRaja.Core.Domain
{
    public static class MovementCommandFactory
    {
        public static MovementCommand Create(
            int actorId,
            int simulationTick,
            MovementInputFrame input,
            MovementTuning tuning)
        {
            return new MovementCommand(
                actorId,
                simulationTick,
                ApplyDeadZone(input.Movement, tuning.MovementDeadZone, 1f),
                ApplyDeadZone(input.Aim, tuning.AimDeadZone, tuning.InputSensitivity));
        }

        private static Float2 ApplyDeadZone(Float2 value, float deadZone, float sensitivity)
        {
            var magnitude = value.Magnitude;
            if (magnitude <= deadZone)
            {
                return Float2.Zero;
            }

            var remappedMagnitude = MathF.Min(1f, ((magnitude - deadZone) / (1f - deadZone)) * sensitivity);
            return value.Normalized * remappedMagnitude;
        }
    }
}
