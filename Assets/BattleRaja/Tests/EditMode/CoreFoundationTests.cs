using System;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class CoreFoundationTests
    {
        [Test]
        public void FixedClockAdvancesOneTickAtATime()
        {
            var clock = new FixedSimulationClock(30);

            Assert.That(clock.Tick, Is.EqualTo(0));
            Assert.That(clock.StepSeconds, Is.EqualTo(1d / 30d).Within(0.0000001d));

            clock.Advance();

            Assert.That(clock.Tick, Is.EqualTo(1));
        }

        [Test]
        public void FixedClockConsumesRenderTimeWithoutChangingTheAuthoritativeRate()
        {
            var clock = new FixedSimulationClock(30);

            Assert.That(clock.Consume(1d / 60d), Is.EqualTo(0));
            Assert.That(clock.Consume(1d / 60d), Is.EqualTo(1));
            Assert.That(clock.Tick, Is.EqualTo(1));
            Assert.That(clock.Consume(1d / 30d), Is.EqualTo(1));
            Assert.That(clock.Tick, Is.EqualTo(2));
            Assert.That(clock.InterpolationAlpha, Is.InRange(0d, 1d));
        }

        [Test]
        public void FixedClockExposesSequentialTicksForEveryStepInOneRenderFrame()
        {
            var clock = new FixedSimulationClock(30);

            Assert.That(clock.Consume(0.1d), Is.EqualTo(3));
            Assert.That(clock.LastConsumedSteps, Is.EqualTo(3));
            Assert.That(clock.GetConsumedTick(0), Is.EqualTo(1));
            Assert.That(clock.GetConsumedTick(1), Is.EqualTo(2));
            Assert.That(clock.GetConsumedTick(2), Is.EqualTo(3));
            Assert.Throws<ArgumentOutOfRangeException>(() => clock.GetConsumedTick(3));

            Assert.That(clock.Consume(0d), Is.EqualTo(0));
            Assert.That(clock.LastConsumedSteps, Is.EqualTo(0));
        }

        [Test]
        public void OfflineAuthorityAcceptsEveryFixedStepFromOneRenderFrame()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new[]
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(-8f, 0f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            var clock = new FixedSimulationClock(30);
            var steps = clock.Consume(0.1d);

            for (var i = 0; i < steps; i++)
            {
                authority.Advance(clock.GetConsumedTick(i), (float)clock.StepSeconds);
            }

            Assert.That(authority.Simulation.ElapsedSeconds, Is.EqualTo(0.1f).Within(0.0001f));
            Assert.That(authority.Simulation.Phase, Is.EqualTo(MatchPhase.LoadWarmup));
        }

        [Test]
        public void FixedClockProducesTheSameTickCountForDifferentRenderRates()
        {
            var thirtyFps = new FixedSimulationClock(30);
            var oneTwentyFps = new FixedSimulationClock(30);

            for (var i = 0; i < 30; i++)
            {
                thirtyFps.Consume(1d / 30d);
            }

            for (var i = 0; i < 120; i++)
            {
                oneTwentyFps.Consume(1d / 120d);
            }

            Assert.That(thirtyFps.Tick, Is.EqualTo(oneTwentyFps.Tick));
            Assert.That(thirtyFps.Tick, Is.EqualTo(30));
            Assert.That(thirtyFps.AccumulatorSeconds, Is.EqualTo(oneTwentyFps.AccumulatorSeconds).Within(0.0000001d));
        }

        [Test]
        public void CommandCarriesStableActorAndSimulationIdentity()
        {
            var command = new GameplayCommand(7, 42, GameplayCommandKind.Intent, 99);

            Assert.That(command.ActorId, Is.EqualTo(7));
            Assert.That(command.SimulationTick, Is.EqualTo(42));
            Assert.That(command.Kind, Is.EqualTo(GameplayCommandKind.Intent));
            Assert.That(command.Payload, Is.EqualTo(99));
        }

        [Test]
        public void FixedClockProducesTheSameOfflineMatchStateAcrossRenderRates()
        {
            var thirtyFps = RunMatch(new[] { 1d / 30d }, 180);
            var sixtyFps = RunMatch(new[] { 1d / 60d }, 360);
            var ninetyFps = RunMatch(new[] { 1d / 90d }, 540);
            var variable = RunMatch(new[] { 1d / 120d, 1d / 60d, 1d / 40d, 1d / 120d, 1d / 60d, 1d / 40d }, 60);

            var expected = thirtyFps.Advance(0f);
            Assert.That(expected.Phase, Is.EqualTo(MatchPhase.SpawnProtection));
            Assert.That(expected.ZoneRadius, Is.EqualTo(sixtyFps.Advance(0f).ZoneRadius).Within(0.0001f));
            Assert.That(expected.ZoneRadius, Is.EqualTo(ninetyFps.Advance(0f).ZoneRadius).Within(0.0001f));
            Assert.That(expected.ZoneRadius, Is.EqualTo(variable.Advance(0f).ZoneRadius).Within(0.0001f));
            Assert.That(thirtyFps.ElapsedSeconds, Is.EqualTo(sixtyFps.ElapsedSeconds).Within(0.0001f));
            Assert.That(thirtyFps.ElapsedSeconds, Is.EqualTo(ninetyFps.ElapsedSeconds).Within(0.0001f));
            Assert.That(thirtyFps.ElapsedSeconds, Is.EqualTo(variable.ElapsedSeconds).Within(0.0001f));

            var expectedSnapshots = thirtyFps.GetSnapshots();
            AssertSnapshotsEqual(expectedSnapshots, sixtyFps.GetSnapshots());
            AssertSnapshotsEqual(expectedSnapshots, ninetyFps.GetSnapshots());
            AssertSnapshotsEqual(expectedSnapshots, variable.GetSnapshots());
        }

        private static OfflineMatchSimulation RunMatch(double[] renderFrameDurations, int repetitions)
        {
            var simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            simulation.Start(new[]
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(-8f, 0f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            var clock = new FixedSimulationClock(30);
            for (var repeat = 0; repeat < repetitions; repeat++)
            {
                for (var frame = 0; frame < renderFrameDurations.Length; frame++)
                {
                    var steps = clock.Consume(renderFrameDurations[frame]);
                    for (var step = 0; step < steps; step++) simulation.Advance((float)clock.StepSeconds);
                }
            }

            return simulation;
        }

        private static void AssertSnapshotsEqual(MatchParticipantSnapshot[] expected, MatchParticipantSnapshot[] actual)
        {
            Assert.That(actual, Has.Length.EqualTo(expected.Length));
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.That(actual[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(actual[i].CurrentHealth, Is.EqualTo(expected[i].CurrentHealth));
                Assert.That(actual[i].Placement, Is.EqualTo(expected[i].Placement));
                Assert.That(actual[i].Alive, Is.EqualTo(expected[i].Alive));
                Assert.That(actual[i].SurvivalTimeSeconds, Is.EqualTo(expected[i].SurvivalTimeSeconds).Within(0.0001f));
            }
        }
    }
}
