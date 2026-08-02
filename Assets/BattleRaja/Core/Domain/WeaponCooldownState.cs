namespace BattleRaja.Core.Domain
{
    public sealed class WeaponCooldownState
    {
        private float _nextAllowedTime;
        private int _nextAllowedTick;
        private bool _tickMode;

        public bool TryConsume(float nowSeconds, float intervalSeconds)
        {
            _tickMode = false;
            if (intervalSeconds <= 0f || nowSeconds + 0.000001f < _nextAllowedTime)
            {
                return false;
            }

            _nextAllowedTime = nowSeconds + intervalSeconds;
            return true;
        }

        public float Remaining(float nowSeconds)
        {
            return System.MathF.Max(0f, _nextAllowedTime - nowSeconds);
        }

        public bool TryConsume(int nowTick, int intervalTicks)
        {
            _tickMode = true;
            if (intervalTicks <= 0 || nowTick < _nextAllowedTick) return false;
            _nextAllowedTick = checked(nowTick + intervalTicks);
            return true;
        }

        public int RemainingTicks(int nowTick) => _tickMode ? System.Math.Max(0, _nextAllowedTick - nowTick) : 0;

        public float RemainingSeconds(int nowTick, int tickRate)
        {
            if (!_tickMode || tickRate <= 0) return 0f;
            return RemainingTicks(nowTick) / (float)tickRate;
        }

        public void Reset()
        {
            _nextAllowedTime = 0f;
            _nextAllowedTick = 0;
            _tickMode = false;
        }
    }
}
