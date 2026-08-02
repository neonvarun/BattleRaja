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
        public void CommandCarriesStableActorAndSimulationIdentity()
        {
            var command = new GameplayCommand(7, 42, GameplayCommandKind.Intent, 99);

            Assert.That(command.ActorId, Is.EqualTo(7));
            Assert.That(command.SimulationTick, Is.EqualTo(42));
            Assert.That(command.Kind, Is.EqualTo(GameplayCommandKind.Intent));
            Assert.That(command.Payload, Is.EqualTo(99));
        }
    }
}
