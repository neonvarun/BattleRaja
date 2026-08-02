namespace BattleRaja.Core.Domain
{
    public readonly struct HealthSnapshot
    {
        public HealthSnapshot(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }

        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public bool IsDefeated => CurrentHealth <= 0;
    }

    public sealed class HealthState
    {
        private readonly int _maxHealth;
        private int _currentHealth;

        public HealthState(int maxHealth)
        {
            _maxHealth = maxHealth < 1 ? 1 : maxHealth;
            _currentHealth = _maxHealth;
        }

        public HealthSnapshot Snapshot => new HealthSnapshot(_maxHealth, _currentHealth);

        internal DamageResult ApplyDamage(int amount)
        {
            if (amount <= 0)
            {
                return new DamageResult(false, 0, _currentHealth <= 0, DamageRejectionReason.InvalidAmount);
            }

            if (_currentHealth <= 0)
            {
                return new DamageResult(false, 0, true, DamageRejectionReason.AlreadyDefeated);
            }

            var applied = amount > _currentHealth ? _currentHealth : amount;
            _currentHealth -= applied;
            return new DamageResult(true, applied, _currentHealth == 0, DamageRejectionReason.None);
        }

        public int Heal(int amount)
        {
            if (amount <= 0 || _currentHealth <= 0) return 0;
            var applied = System.Math.Min(amount, _maxHealth - _currentHealth);
            _currentHealth += applied;
            return applied;
        }

        public void Reset()
        {
            _currentHealth = _maxHealth;
        }
    }
}
