using System;

namespace BattleRaja.Core.Domain
{
    public sealed class FixedSimulationClock
    {
        private double _accumulatorSeconds;

        public FixedSimulationClock(int tickRate)
        {
            if (tickRate <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tickRate));
            }

            TickRate = tickRate;
            Tick = 0;
        }

        public int TickRate { get; }
        public int Tick { get; private set; }
        public int LastConsumedSteps { get; private set; }
        public double StepSeconds => 1d / TickRate;
        public double AccumulatorSeconds => _accumulatorSeconds;
        public double InterpolationAlpha => _accumulatorSeconds / StepSeconds;

        public int Consume(double elapsedSeconds)
        {
            if (elapsedSeconds < 0d || double.IsNaN(elapsedSeconds) || double.IsInfinity(elapsedSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(elapsedSeconds));
            }

            _accumulatorSeconds += elapsedSeconds;
            var steps = 0;
            while (_accumulatorSeconds + 0.0000000001d >= StepSeconds)
            {
                _accumulatorSeconds -= StepSeconds;
                if (_accumulatorSeconds < 0d) _accumulatorSeconds = 0d;
                Advance();
                steps++;
            }

            LastConsumedSteps = steps;
            return steps;
        }

        /// <summary>
        /// Returns the authoritative tick represented by one of the steps consumed
        /// by the most recent <see cref="Consume"/> call. A render frame can consume
        /// more than one fixed step, so callers must not reuse <see cref="Tick"/>
        /// for every iteration of that frame.
        /// </summary>
        public int GetConsumedTick(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= LastConsumedSteps)
            {
                throw new ArgumentOutOfRangeException(nameof(stepIndex));
            }

            return Tick - LastConsumedSteps + stepIndex + 1;
        }

        public void Advance()
        {
            checked
            {
                Tick++;
            }
        }
    }
}
