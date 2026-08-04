using System.Collections.Generic;
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
                new MatchSpawn(p2, new Float2(5f, 0f), 100)
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

            // Fire attack from p1 towards p2 at (5, 0)
            var attackCmd = new AttackCommand(p1, currentTick, new Float2(0.7f, 0f), new Float2(1f, 0f), true, 1);
            var attackResult = authority.TryAcceptAttack(attackCmd);

            Assert.IsTrue(attackResult.Accepted);
            Assert.Greater(attackResult.ProjectileId, 0);

            // Enemy start health is 100
            Assert.IsTrue(authority.Simulation.TryGetSnapshot(p2, out var snapshotBefore));
            Assert.AreEqual(100, snapshotBefore.CurrentHealth);

            // Advance simulation step (fixedDeltaSeconds = 0.5f, speed = 20 -> dist = 10, hits enemy at 5)
            var tick = authority.Advance(currentTick, 0.5f);

            Assert.AreEqual(1, tick.ProjectileSnapshots.Length);
            var projSnap = tick.ProjectileSnapshots[0];
            Assert.AreEqual(ProjectileDespawnReason.HitActor, projSnap.DespawnReason);
            Assert.AreEqual(p2, projSnap.HitTargetId);

            // Health of p2 must be reduced inside authority simulation
            Assert.IsTrue(authority.Simulation.TryGetSnapshot(p2, out var snapshotAfter));
            Assert.Less(snapshotAfter.CurrentHealth, 100);
        }

        [Test]
        public void AuthoritativeProjectile_SweepsAndHitsWall_DespawnsWithoutActorDamage()
        {
            var definition = OfflineMatchDefinition.SoloRaja;
            var authority = new OfflineMatchAuthority(definition);

            var p1 = new CombatEntityId(1);
            var spawns = new List<MatchSpawn>
            {
                new MatchSpawn(p1, new Float2(-13f, 0f), 100)
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

            // Fire attack directly west towards wall at X = -14.5
            var attackCmd = new AttackCommand(p1, currentTick, new Float2(-13.7f, 0f), new Float2(-1f, 0f), true, 1);
            var attackResult = authority.TryAcceptAttack(attackCmd);

            Assert.IsTrue(attackResult.Accepted);

            var tick = authority.Advance(currentTick, 0.5f);

            Assert.AreEqual(1, tick.ProjectileSnapshots.Length);
            var projSnap = tick.ProjectileSnapshots[0];
            Assert.AreEqual(ProjectileDespawnReason.HitWall, projSnap.DespawnReason);
        }
    }
}
