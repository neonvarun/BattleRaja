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
            var p1 = new MatchParticipantDefinition(new CombatEntityId(1), "Player", ContentId.Maya, ProjectileWeaponDefinition.TrainingBolt, 100);
            var p2 = new MatchParticipantDefinition(new CombatEntityId(2), "Enemy", ContentId.Raja, ProjectileWeaponDefinition.TrainingBolt, 100);
            var definition = new OfflineMatchDefinition(new[] { p1, p2 }, MatchRuleset.Default);
            var authority = new OfflineMatchAuthority(definition);

            var spawns = new List<ParticipantSpawnInfo>
            {
                new ParticipantSpawnInfo(p1.Id, new Float2(0f, 0f), CombatFaction.Player),
                new ParticipantSpawnInfo(p2.Id, new Float2(5f, 0f), CombatFaction.Enemy)
            };

            authority.Start(spawns);

            // Fire attack from p1 towards p2 at (5, 0)
            var attackCmd = new AttackCommand(p1.Id, 0, new Float2(0.7f, 0f), Float2.Right, true, 1);
            var attackResult = authority.TryAcceptAttack(attackCmd);

            Assert.IsTrue(attackResult.Accepted);
            Assert.Greater(attackResult.ProjectileId, 0);

            // Enemy start health is 100
            Assert.IsTrue(authority.Simulation.TryGetSnapshot(p2.Id, out var snapshotBefore));
            Assert.AreEqual(100, snapshotBefore.CurrentHealth);

            // Advance simulation step (fixedDeltaSeconds = 0.5f, speed = 20 -> dist = 10, hits enemy at 5)
            var tick = authority.Advance(0, 0.5f);

            Assert.AreEqual(1, tick.ProjectileSnapshots.Length);
            var projSnap = tick.ProjectileSnapshots[0];
            Assert.AreEqual(ProjectileDespawnReason.HitActor, projSnap.DespawnReason);
            Assert.AreEqual(p2.Id, projSnap.HitTargetId);

            // Health of p2 must be reduced inside authority simulation
            Assert.IsTrue(authority.Simulation.TryGetSnapshot(p2.Id, out var snapshotAfter));
            Assert.Less(snapshotAfter.CurrentHealth, 100);
        }

        [Test]
        public void AuthoritativeProjectile_SweepsAndHitsWall_DespawnsWithoutActorDamage()
        {
            var p1 = new MatchParticipantDefinition(new CombatEntityId(1), "Player", ContentId.Maya, ProjectileWeaponDefinition.TrainingBolt, 100);
            var definition = new OfflineMatchDefinition(new[] { p1 }, MatchRuleset.Default);
            var authority = new OfflineMatchAuthority(definition);

            var spawns = new List<ParticipantSpawnInfo>
            {
                // Position near NarrowLaneWest wall at X = -14.5
                new ParticipantSpawnInfo(p1.Id, new Float2(-13f, 0f), CombatFaction.Player)
            };

            authority.Start(spawns);

            // Fire attack directly west towards wall at X = -14.5
            var attackCmd = new AttackCommand(p1.Id, 0, new Float2(-13.7f, 0f), Float2.Left, true, 1);
            var attackResult = authority.TryAcceptAttack(attackCmd);

            Assert.IsTrue(attackResult.Accepted);

            var tick = authority.Advance(0, 0.5f);

            Assert.AreEqual(1, tick.ProjectileSnapshots.Length);
            var projSnap = tick.ProjectileSnapshots[0];
            Assert.AreEqual(ProjectileDespawnReason.HitWall, projSnap.DespawnReason);
        }
    }
}
