namespace BattleRaja.Core.Domain
{
    public sealed class WeaponCooldownState
    {
        private float _nextAllowedTime;

        public bool TryConsume(float nowSeconds, float intervalSeconds)
        {
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

        public void Reset() => _nextAllowedTime = 0f;
    }
}
