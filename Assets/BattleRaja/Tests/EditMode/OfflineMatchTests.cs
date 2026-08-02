using System.Collections.Generic;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class OfflineMatchTests
    {
        private static List<MatchSpawn> CreateSpawns(int count = 8)
        {
            var spawns = new List<MatchSpawn>();
            for (var i = 0; i < count; i++)
            {
                var angle = i * 0.7853982f;
                spawns.Add(new MatchSpawn(new CombatEntityId(i + 1), new Float2(System.MathF.Cos(angle) * 8f, System.MathF.Sin(angle) * 8f), 100));
            }

            return spawns;
        }

        [Test]
        public void SpawnValidationRejectsOverlappingActors()
        {
            var valid = CreateSpawns();
            Assert.That(SpawnPointValidator.AreSeparated(valid), Is.True);
            valid[1] = new MatchSpawn(valid[1].Id, valid[0].Position, 100);
            Assert.That(SpawnPointValidator.AreSeparated(valid), Is.False);
        }

        [Test]
        public void MatchPhasesAndAandhiRadiusProgressDataDriven()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns());
            Assert.That(simulation.Advance(3.1f).Phase, Is.EqualTo(MatchPhase.SpawnProtection));
            var opening = simulation.Advance(5f);
            Assert.That(opening.Phase, Is.EqualTo(MatchPhase.Opening));
            Assert.That(opening.ZoneRadius, Is.InRange(13.9f, 14f));
            simulation.SetPosition(new CombatEntityId(1), new Float2(14.5f, 0f));
            var openingMidpoint = simulation.Advance(45f);
            Assert.That(openingMidpoint.ZoneRadius, Is.InRange(10.9f, 11f));
            var pressure = simulation.Advance(45f);
            Assert.That(pressure.Phase, Is.EqualTo(MatchPhase.Pressure));
            Assert.That(pressure.ZoneRadius, Is.InRange(7.9f, 8f));
            Assert.That(pressure.OutsideCount, Is.GreaterThanOrEqualTo(1));
            Assert.That(pressure.OutsideDamagePerSecond, Is.EqualTo(10));
            var pressureMidpoint = simulation.Advance(60f);
            Assert.That(pressureMidpoint.ZoneRadius, Is.InRange(5.6f, 5.75f));
        }

        [Test]
        public void EliminationIsIdempotentAndPlacementIsStable()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns(3));
            Assert.That(simulation.SyncHealth(new CombatEntityId(2), 0), Is.True);
            Assert.That(simulation.SyncHealth(new CombatEntityId(2), 0), Is.True);
            var first = simulation.GetSnapshots()[1];
            Assert.That(first.Alive, Is.False);
            Assert.That(first.Placement, Is.EqualTo(3));
            Assert.That(simulation.SyncHealth(new CombatEntityId(3), 0), Is.True);
            var snapshots = simulation.GetSnapshots();
            Assert.That(simulation.IsEnded, Is.True);
            Assert.That(snapshots[0].Placement, Is.EqualTo(1));
            Assert.That(snapshots[2].Placement, Is.EqualTo(2));
        }

        [Test]
        public void TimeoutRanksSeveralSurvivorsAndAssignsEveryPlacement()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns(4));
            simulation.SyncHealth(new CombatEntityId(4), 0);
            simulation.SyncHealth(new CombatEntityId(1), 60);
            simulation.SyncHealth(new CombatEntityId(2), 80);
            simulation.SyncHealth(new CombatEntityId(3), 40);

            simulation.Advance(OfflineMatchDefinition.SoloRaja.TargetDurationSeconds + 1f);

            var snapshots = simulation.GetSnapshots();
            Assert.That(simulation.IsEnded, Is.True);
            Assert.That(snapshots[1].Placement, Is.EqualTo(1));
            Assert.That(snapshots[0].Placement, Is.EqualTo(2));
            Assert.That(snapshots[2].Placement, Is.EqualTo(3));
            Assert.That(snapshots[3].Placement, Is.EqualTo(4));
            Assert.That(simulation.GetSnapshots()[1].Id.Value, Is.EqualTo(2));
        }

        [Test]
        public void TimeoutUsesDistanceThenEntityIdForCompleteTies()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns(3));
            simulation.SetPosition(new CombatEntityId(1), new Float2(3f, 0f));
            simulation.SetPosition(new CombatEntityId(2), new Float2(1f, 0f));
            simulation.SetPosition(new CombatEntityId(3), new Float2(1f, 0f));

            simulation.Advance(OfflineMatchDefinition.SoloRaja.TargetDurationSeconds + 1f);

            var snapshots = simulation.GetSnapshots();
            Assert.That(snapshots[1].Placement, Is.EqualTo(1));
            Assert.That(snapshots[2].Placement, Is.EqualTo(2));
            Assert.That(snapshots[0].Placement, Is.EqualTo(3));
            Assert.That(snapshots[1].Id.Value, Is.EqualTo(2));
        }

        [Test]
        public void SpectatorSelectsOnlyLivingTargets()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns(4));
            simulation.SyncHealth(new CombatEntityId(2), 0);
            var next = SpectatorTargetSelector.SelectNext(simulation.GetSnapshots(), new CombatEntityId(1));
            Assert.That(next.Value, Is.EqualTo(3));
        }

        [Test]
        public void TwentyAcceleratedMatchesCompleteWithoutRuntimeLeakState()
        {
            var definition = OfflineMatchDefinition.SoloRaja;
            Assert.That(definition.TargetDurationSeconds, Is.InRange(240f, 360f));
            for (var match = 0; match < 20; match++)
            {
                var simulation = new OfflineMatchSimulation(definition);
                simulation.Start(CreateSpawns());
                for (var id = 2; id <= 8; id++) simulation.SyncHealth(new CombatEntityId(id), 0);
                Assert.That(simulation.IsEnded, Is.True);
                simulation.Restart();
                Assert.That(simulation.IsStarted, Is.False);
            }
        }
    }
}
