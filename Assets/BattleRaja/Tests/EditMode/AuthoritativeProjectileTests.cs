using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    [TestFixture]
    public class AuthoritativeProjectileTests
    {
        [Test]
        public void MatchEventIdentityTracker_GeneratesSequentialIds_AndResets()
        {
            var tracker = new MatchEventIdentityTracker();
            Assert.AreEqual(1, tracker.NextAttackExecutionId());
            Assert.AreEqual(2, tracker.NextAttackExecutionId());

            Assert.AreEqual(1, tracker.NextProjectileId());
            Assert.AreEqual(2, tracker.NextProjectileId());

            Assert.AreEqual(1, tracker.NextAbilityExecutionId());
            Assert.AreEqual(1, tracker.NextGadgetUseId());
            Assert.AreEqual(1, tracker.NextDamageEventId());
            Assert.AreEqual(1, tracker.NextHealingEventId());
            Assert.AreEqual(1, tracker.NextCollectionEventId());
            Assert.AreEqual(1, tracker.NextEliminationEventId());

            tracker.Reset();

            Assert.AreEqual(1, tracker.NextAttackExecutionId());
            Assert.AreEqual(1, tracker.NextProjectileId());
        }

        [Test]
        public void AuthoritativeProjectile_SweepsAndHitsParticipant_ResolvingDamageInAuthority()
        {
            var definition = OfflineMatchDefinition.SoloRaja;
            var authority = new OfflineMatchAuthority(definition);

            var p1 = new CombatEntityId(1);
            var p2 = new CombatEntityId(2);

            var spawns = new List<MatchSpawn>
            {
                new MatchSpawn(p1, new Float2(0f, 0f), 100),
                // Place the enemy on the unobstructed north lane: the +X lane at
                // y=0 is blocked by the authored NarrowLaneEast obstacle.
                new MatchSpawn(p2, new Float2(0f, 5f), 100)
            };

            authority.Start(spawns);
            authority.ConfigureWeapon(p1, ProjectileWeaponDefinition.TrainingBolt, 30);
            authority.ConfigureWeapon(p2, ProjectileWeaponDefinition.TrainingBolt, 30);
            authority.ConfigureFaction(p1, CombatFaction.Player);
            authority.ConfigureFaction(p2, CombatFaction.Enemy);

            // Advance past warmup and spawn protection
            for (var t = 0; t < 300; t++)
            {
                authority.Advance(t, 1f / 30f);
            }

            var currentTick = authority.CurrentSimulationTick + 1;

            // Fire attack from p1 towards p2 at (0, 5)
            var attackCmd = new AttackCommand(p1, currentTick, new Float2(0f, 0.7f), new Float2(0f, 1f), true, 1);
            var attackResult = authority.TryAcceptAttack(attackCmd);

            Assert.IsTrue(attackResult.Accepted);
            Assert.Greater(attackResult.ProjectileId, 0);

            // Enemy start health is 100
            Assert.IsTrue(authority.Simulation.TryGetSnapshot(p2, out var snapshotBefore));
            Assert.AreEqual(100, snapshotBefore.CurrentHealth);

            // Advance simulation step (fixedDeltaSeconds = 0.5f, speed = 20 -> dist = 10, hits enemy at y=5)
            var tick = authority.Advance(currentTick, 0.5f);

            Assert.AreEqual(1, tick.ProjectileSnapshots.Length);
            var projSnap = tick.ProjectileSnapshots[0];
            Assert.AreEqual(ProjectileDespawnReason.HitActor, projSnap.DespawnReason);
            Assert.AreEqual(p2, projSnap.HitTargetId);

            // Health of p2 must be reduced inside authority simulation
            Assert.IsTrue(authority.Simulation.TryGetSnapshot(p2, out var snapshotAfter));
            Assert.Less(snapshotAfter.CurrentHealth, 100);

            // The same canonical tick must publish the already-applied projectile
            // damage so presentation mirrors health and elimination state at once.
            Assert.AreEqual(1, tick.DamageEvents.Length);
            var projectileEvent = tick.DamageEvents[0];
            Assert.AreEqual(p1, projectileEvent.InstigatorId);
            Assert.AreEqual(p2, projectileEvent.TargetId);
            Assert.AreEqual(DamageType.Projectile, projectileEvent.DamageType);
            Assert.AreEqual(snapshotBefore.CurrentHealth - snapshotAfter.CurrentHealth,
                projectileEvent.AmountApplied);
            Assert.AreEqual(snapshotAfter.CurrentHealth, projectileEvent.CurrentHealthAfter);
            Assert.AreEqual(currentTick, projectileEvent.SimulationTick);
            Assert.Greater(projectileEvent.EventId, 0);
        }

        [Test]
        public void AuthoritativeProjectile_SweepsAndHitsWall_DespawnsWithoutActorDamage()
        {
            var definition = OfflineMatchDefinition.SoloRaja;
            var authority = new OfflineMatchAuthority(definition);

            var p1 = new CombatEntityId(1);
            var p2 = new CombatEntityId(2);
            // The simulation requires at least two separated valid spawns; p2
            // parks far away and never interacts with this wall-sweep fixture.
            var spawns = new List<MatchSpawn>
            {
                new MatchSpawn(p1, new Float2(-11f, 0f), 100),
                new MatchSpawn(p2, new Float2(13f, 8f), 100)
            };

            authority.Start(spawns);
            authority.ConfigureWeapon(p1, ProjectileWeaponDefinition.TrainingBolt, 30);
            authority.ConfigureFaction(p1, CombatFaction.Player);

            // Advance past warmup and spawn protection
            for (var t = 0; t < 300; t++)
            {
                authority.Advance(t, 1f / 30f);
            }

            var currentTick = authority.CurrentSimulationTick + 1;

            // Fire attack directly west towards the boundary wall at X = -12.75
            var attackCmd = new AttackCommand(p1, currentTick, new Float2(-11.7f, 0f), new Float2(-1f, 0f), true, 1);
            var attackResult = authority.TryAcceptAttack(attackCmd);

            Assert.IsTrue(attackResult.Accepted);

            var tick = authority.Advance(currentTick, 0.5f);

            Assert.AreEqual(1, tick.ProjectileSnapshots.Length);
            var projSnap = tick.ProjectileSnapshots[0];
            Assert.AreEqual(ProjectileDespawnReason.HitWall, projSnap.DespawnReason);
        }

        [Test]
        public void SoloRajaBotProjectileCanDamageAndEliminateAnotherBot()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var shooter = new CombatEntityId(2);
            var victim = new CombatEntityId(3);

            // Both production bots use the Enemy view label. Their authority-owned
            // combat groups default to their entity IDs, making this a true FFA.
            var spawns = new List<MatchSpawn>
            {
                new MatchSpawn(shooter, new Float2(0f, 0f), 100),
                new MatchSpawn(victim, new Float2(0f, 4f), ProjectileWeaponDefinition.TrainingBolt.Damage)
            };

            authority.Start(spawns);
            authority.ConfigureWeapon(shooter, ProjectileWeaponDefinition.TrainingBolt, 30);
            authority.ConfigureWeapon(victim, ProjectileWeaponDefinition.TrainingBolt, 30);
            authority.ConfigureFaction(shooter, CombatFaction.Enemy);
            authority.ConfigureFaction(victim, CombatFaction.Enemy);

            for (var tick = 0; tick < 300; tick++) authority.Advance(tick, 1f / 30f);

            var commandTick = authority.CurrentSimulationTick + 1;
            var attack = new AttackCommand(
                shooter,
                commandTick,
                new Float2(0f, 0.7f),
                new Float2(0f, 1f),
                true,
                1);
            var accepted = authority.TryAcceptAttack(attack);
            Assert.IsTrue(accepted.Accepted);

            var result = authority.Advance(commandTick, 0.5f);
            Assert.AreEqual(ProjectileDespawnReason.HitActor, result.ProjectileSnapshots[0].DespawnReason);
            Assert.AreEqual(1, result.DamageEvents.Length);
            Assert.AreEqual(shooter, result.DamageEvents[0].InstigatorId);
            Assert.AreEqual(victim, result.DamageEvents[0].TargetId);

            var snapshots = authority.Simulation.GetSnapshots();
            var victimAfter = snapshots.Single(s => s.Id == victim);
            var shooterAfter = snapshots.Single(s => s.Id == shooter);
            Assert.IsFalse(victimAfter.Alive);
            Assert.AreEqual(2, victimAfter.Placement);
            Assert.IsTrue(shooterAfter.Alive);
            Assert.AreEqual(1, shooterAfter.Placement);
            Assert.AreEqual(1, shooterAfter.Eliminations);
            Assert.AreEqual(ProjectileWeaponDefinition.TrainingBolt.Damage, shooterAfter.DamageDealt);
        }

        [Test]
        public void SameAuthorityCombatGroupBlocksSameViewFactionProjectile()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var shooter = new CombatEntityId(2);
            var teammate = new CombatEntityId(3);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(shooter, new Float2(0f, 0f), 100),
                new MatchSpawn(teammate, new Float2(0f, 4f), 100)
            });
            authority.ConfigureWeapon(shooter, ProjectileWeaponDefinition.TrainingBolt, 30);
            authority.ConfigureWeapon(teammate, ProjectileWeaponDefinition.TrainingBolt, 30);
            authority.ConfigureFaction(shooter, CombatFaction.Enemy);
            authority.ConfigureFaction(teammate, CombatFaction.Enemy);
            authority.ConfigureCombatGroup(shooter, 20);
            authority.ConfigureCombatGroup(teammate, 20);

            for (var tick = 0; tick < 300; tick++) authority.Advance(tick, 1f / 30f);

            var commandTick = authority.CurrentSimulationTick + 1;
            var accepted = authority.TryAcceptAttack(new AttackCommand(
                shooter,
                commandTick,
                new Float2(0f, 0.7f),
                new Float2(0f, 1f),
                true,
                1));
            Assert.IsTrue(accepted.Accepted);

            var result = authority.Advance(commandTick, 0.5f);
            Assert.AreEqual(0, result.DamageEvents.Length);
            var snapshot = authority.Simulation.GetSnapshots().Single(s => s.Id == teammate);
            Assert.AreEqual(100, snapshot.CurrentHealth);
        }
    }
}
