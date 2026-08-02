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
            Assert.That(openingMidpoint.ZoneRadius, Is.InRange(11.1f, 11.4f));
            var pressure = simulation.Advance(45f);
            Assert.That(pressure.Phase, Is.EqualTo(MatchPhase.Pressure));
            Assert.That(pressure.ZoneRadius, Is.InRange(7.9f, 8f));
            Assert.That(pressure.OutsideCount, Is.GreaterThanOrEqualTo(1));
            Assert.That(pressure.OutsideDamagePerSecond, Is.EqualTo(10));
            var pressureMidpoint = simulation.Advance(60f);
            Assert.That(pressureMidpoint.ZoneRadius, Is.InRange(5.8f, 6f));
        }

        [Test]
        public void AandhiWarningPrecedesClosingAndPreviewsNextRadius()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns());
            var warning = simulation.Advance(8f);
            Assert.That(warning.AandhiState, Is.EqualTo(AandhiState.Warning));
            Assert.That(warning.WarningRemainingSeconds, Is.EqualTo(8f).Within(0.0001f));
            Assert.That(warning.ZoneRadius, Is.EqualTo(14f).Within(0.0001f));
            Assert.That(warning.NextZoneRadius, Is.EqualTo(8f).Within(0.0001f));

            simulation.Advance(4f);
            var closing = simulation.Advance(4.1f);
            Assert.That(closing.AandhiState, Is.EqualTo(AandhiState.Closing));
            Assert.That(closing.WarningRemainingSeconds, Is.EqualTo(0f));
            Assert.That(closing.ZoneRadius, Is.LessThan(14f));
            Assert.That(closing.ZoneRadius, Is.GreaterThan(8f));
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
        public void DamageEventsAttributeDamageAndEliminationExactlyOnce()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns(3));
            simulation.Advance(2f);

            var first = new DamageRequest(new CombatEntityId(1), new CombatEntityId(2), CombatFaction.Player, 30, DamageType.Projectile);
            Assert.That(simulation.RecordDamage(new CombatDamageEvent(first, 30, false, 70, 60)), Is.True);
            var finishing = new DamageRequest(new CombatEntityId(1), new CombatEntityId(2), CombatFaction.Player, 70, DamageType.Projectile);
            Assert.That(simulation.RecordDamage(new CombatDamageEvent(finishing, 70, true, 0, 61)), Is.True);
            Assert.That(simulation.RecordDamage(new CombatDamageEvent(finishing, 70, true, 0, 62)), Is.False);

            var snapshots = simulation.GetSnapshots();
            Assert.That(snapshots[0].DamageDealt, Is.EqualTo(100));
            Assert.That(snapshots[0].Eliminations, Is.EqualTo(1));
            Assert.That(snapshots[1].Alive, Is.False);
            Assert.That(snapshots[1].SurvivalTimeSeconds, Is.EqualTo(2f).Within(0.0001f));
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
            Assert.That(simulation.Advance(0f).WinnerId.Value, Is.EqualTo(2));
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
        public void TickPublishesExplicitNextZoneCenter()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns(2));

            var tick = simulation.Advance(8f);

            Assert.That(tick.NextZoneCenter, Is.EqualTo(tick.ZoneCenter));
            Assert.That(tick.NextZoneRadius, Is.EqualTo(8f).Within(0.0001f));
        }

        [Test]
        public void TimeoutUsesDamageDealtAfterHealthAndEliminations()
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(CreateSpawns(3));
            simulation.SyncHealth(new CombatEntityId(2), 90);
            simulation.SyncHealth(new CombatEntityId(3), 90);
            var request = new DamageRequest(new CombatEntityId(3), new CombatEntityId(1), CombatFaction.Enemy, 10, DamageType.Projectile);
            simulation.RecordDamage(new CombatDamageEvent(request, 10, false, 90, 1));

            simulation.Advance(OfflineMatchDefinition.SoloRaja.TargetDurationSeconds + 1f);

            var snapshots = simulation.GetSnapshots();
            Assert.That(snapshots[2].Placement, Is.EqualTo(1));
            Assert.That(snapshots[2].DamageDealt, Is.EqualTo(10));
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
