namespace BattleRaja.Core.Domain
{
    public enum ProjectileDespawnReason
    {
        None = 0,
        Hit = 1,
        RangeExceeded = 2,
        LifetimeExpired = 3,
        Collision = 4,
        PoolReset = 5,
        HitActor = 6,
        HitDecoy = 7,
        HitStation = 8,
        HitWall = 9,
        RangeExpired = 10
    }

    public readonly struct ProjectileStepResult
    {
        public ProjectileStepResult(Float2 position, float travelledDistance, float lifetime, bool expired, ProjectileDespawnReason reason)
        {
            Position = position;
            TravelledDistance = travelledDistance;
            Lifetime = lifetime;
            Expired = expired;
            Reason = reason;
        }

        public Float2 Position { get; }
        public float TravelledDistance { get; }
        public float Lifetime { get; }
        public bool Expired { get; }
        public ProjectileDespawnReason Reason { get; }
    }

    public sealed class ProjectileSimulation
    {
        private Float2 _position;
        private readonly Float2 _direction;
        private readonly float _speed;
        private readonly float _maxRange;
        private readonly float _maxLifetime;
        private float _travelledDistance;
        private float _lifetime;

        public ProjectileSimulation(Float2 position, Float2 direction, float speed, float maxRange, float maxLifetime)
        {
            _position = position;
            _direction = direction.SqrMagnitude > 0.000001f ? direction.Normalized : Float2.Up;
            _speed = speed;
            _maxRange = maxRange;
            _maxLifetime = maxLifetime;
        }

        public Float2 Position => _position;
        public float TravelledDistance => _travelledDistance;
        public float Lifetime => _lifetime;

        public ProjectileStepResult Step(float deltaSeconds)
        {
            if (deltaSeconds <= 0f)
            {
                return new ProjectileStepResult(_position, _travelledDistance, _lifetime, false, ProjectileDespawnReason.None);
            }

            var distance = _speed * deltaSeconds;
            _position += _direction * distance;
            _travelledDistance += distance;
            _lifetime += deltaSeconds;
            if (_travelledDistance >= _maxRange)
            {
                return new ProjectileStepResult(_position, _travelledDistance, _lifetime, true, ProjectileDespawnReason.RangeExceeded);
            }

            if (_lifetime >= _maxLifetime)
            {
                return new ProjectileStepResult(_position, _travelledDistance, _lifetime, true, ProjectileDespawnReason.LifetimeExpired);
            }

            return new ProjectileStepResult(_position, _travelledDistance, _lifetime, false, ProjectileDespawnReason.None);
        }
    }
}
