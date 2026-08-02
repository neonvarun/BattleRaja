using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class CombatFoundationTests
    {
        [Test]
        public void HealthClampsDamageAndEmitsDefeatResult()
        {
            var health = new HealthState(50);
            var pipeline = new DamagePipeline();
            var target = new CombatEntityId(2);
            var request = new DamageRequest(new CombatEntityId(1), target, CombatFaction.Player, 80, DamageType.Projectile);

            var result = pipeline.Apply(request, target, CombatFaction.Enemy, health, false, false);

            Assert.That(result.Applied, Is.True);
            Assert.That(result.AmountApplied, Is.EqualTo(50));
            Assert.That(result.TargetDefeated, Is.True);
            Assert.That(health.Snapshot.CurrentHealth, Is.EqualTo(0));
        }

        [Test]
        public void InvalidDamageAndAlreadyDefeatedHealthAreRejected()
        {
            var health = new HealthState(10);
            var pipeline = new DamagePipeline();
            var target = new CombatEntityId(2);
            var invalid = new DamageRequest(new CombatEntityId(1), target, CombatFaction.Player, 0, DamageType.Projectile);
            var lethal = new DamageRequest(new CombatEntityId(1), target, CombatFaction.Player, 10, DamageType.Projectile);

            Assert.That(pipeline.Apply(invalid, target, CombatFaction.Enemy, health, false, false).RejectionReason,
                Is.EqualTo(DamageRejectionReason.InvalidAmount));
            Assert.That(pipeline.Apply(lethal, target, CombatFaction.Enemy, health, false, false).TargetDefeated, Is.True);
            Assert.That(pipeline.Apply(lethal, target, CombatFaction.Enemy, health, false, false).RejectionReason,
                Is.EqualTo(DamageRejectionReason.AlreadyDefeated));
        }

        [Test]
        public void SelfAndFriendlyFirePoliciesAreExplicit()
        {
            var health = new HealthState(20);
            var pipeline = new DamagePipeline();
            var target = new CombatEntityId(2);
            var self = new DamageRequest(target, target, CombatFaction.Player, 5, DamageType.Projectile);
            var friendly = new DamageRequest(new CombatEntityId(3), target, CombatFaction.Player, 5, DamageType.Projectile);

            Assert.That(pipeline.Apply(self, target, CombatFaction.Player, health, false, false).RejectionReason,
                Is.EqualTo(DamageRejectionReason.SelfHit));
            Assert.That(pipeline.Apply(friendly, target, CombatFaction.Player, health, false, false).RejectionReason,
                Is.EqualTo(DamageRejectionReason.FriendlyFire));
            Assert.That(pipeline.Apply(friendly, target, CombatFaction.Player, health, false, true).Applied, Is.True);
        }

        [Test]
        public void WeaponValidationRejectsUnsafeDefinitions()
        {
            var invalid = new ProjectileWeaponDefinition(10, 0f, 12f, 10f, 1f, 0.1f, ~0, false, false);
            var valid = ProjectileWeaponDefinition.TrainingBolt;

            Assert.That(invalid.IsValid(out _), Is.False);
            Assert.That(valid.IsValid(out _), Is.True);
        }

        [Test]
        public void CooldownEnforcesFireRateAndCanReset()
        {
            var cooldown = new WeaponCooldownState();

            Assert.That(cooldown.TryConsume(0f, 0.5f), Is.True);
            Assert.That(cooldown.TryConsume(0.49f, 0.5f), Is.False);
            Assert.That(cooldown.TryConsume(0.5f, 0.5f), Is.True);
            cooldown.Reset();
            Assert.That(cooldown.TryConsume(0f, 0.5f), Is.True);
        }

        [Test]
        public void ProjectileTravelStopsAtRangeOrLifetime()
        {
            var rangeProjectile = new ProjectileSimulation(new Float2(0f, 0f), Float2.Up, 10f, 2f, 5f);
            var lifetimeProjectile = new ProjectileSimulation(new Float2(0f, 0f), Float2.Up, 1f, 100f, 0.25f);

            Assert.That(rangeProjectile.Step(0.1f).Expired, Is.False);
            Assert.That(rangeProjectile.Step(0.2f).Reason, Is.EqualTo(ProjectileDespawnReason.RangeExceeded));
            Assert.That(lifetimeProjectile.Step(0.3f).Reason, Is.EqualTo(ProjectileDespawnReason.LifetimeExpired));
        }

        [Test]
        public void ProjectileHitTrackerPreventsDuplicateDamage()
        {
            var tracker = new ProjectileHitTracker();
            var target = new CombatEntityId(9);

            Assert.That(tracker.TryRegister(target), Is.True);
            Assert.That(tracker.TryRegister(target), Is.False);
            tracker.Clear();
            Assert.That(tracker.TryRegister(target), Is.True);
        }
    }
}
