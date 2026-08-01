using System;

namespace BattleRaja.Core.Domain
{
    public sealed class FixedSimulationClock
    {
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
        public double StepSeconds => 1d / TickRate;

        public void Advance()
        {
            checked
            {
                Tick++;
            }
        }
    }
}
