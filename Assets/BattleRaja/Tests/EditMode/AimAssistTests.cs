using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class AimAssistTests
    {
        [Test]
        public void AimAssistSelectsTheBestCandidateInsideCone()
        {
            var candidates = new[]
            {
                new AimAssistCandidate(new CombatEntityId(2), new Float2(1f, 5f)),
                new AimAssistCandidate(new CombatEntityId(3), new Float2(2f, 4f))
            };

            var assisted = AimAssistTargeting.TryAssist(
                Float2.Zero,
                Float2.Up,
                candidates,
                candidates.Length,
                10f,
                30f,
                out var direction);

            Assert.That(assisted, Is.True);
            Assert.That(direction.X, Is.EqualTo(0.1961f).Within(0.001f));
            Assert.That(direction.Y, Is.EqualTo(0.9806f).Within(0.001f));
        }

        [Test]
        public void AimAssistLeavesIntentAloneOutsideConeOrRange()
        {
            var candidates = new[]
            {
                new AimAssistCandidate(new CombatEntityId(2), new Float2(10f, 0f)),
                new AimAssistCandidate(new CombatEntityId(3), new Float2(0f, 12f))
            };

            var assisted = AimAssistTargeting.TryAssist(
                Float2.Zero,
                Float2.Up,
                candidates,
                candidates.Length,
                10f,
                15f,
                out var direction);

            Assert.That(assisted, Is.False);
            Assert.That(direction, Is.EqualTo(Float2.Up));
        }

        [Test]
        public void AimAssistUsesDistanceThenEntityIdForTies()
        {
            var candidates = new[]
            {
                new AimAssistCandidate(new CombatEntityId(9), new Float2(1f, 4f)),
                new AimAssistCandidate(new CombatEntityId(4), new Float2(-1f, 4f))
            };

            var assisted = AimAssistTargeting.TryAssist(
                Float2.Zero,
                Float2.Up,
                candidates,
                candidates.Length,
                10f,
                30f,
                out var direction);

            Assert.That(assisted, Is.True);
            Assert.That(direction.X, Is.EqualTo(-0.24253562f).Within(0.000001f));
            Assert.That(direction.Y, Is.EqualTo(0.9701425f).Within(0.000001f));
        }

        [Test]
        public void AimAssistRejectsZeroInputAndInvalidCandidateCount()
        {
            var candidates = new[] { new AimAssistCandidate(new CombatEntityId(1), Float2.Up) };
            Assert.That(AimAssistTargeting.TryAssist(Float2.Zero, Float2.Zero, candidates, 1, 10f, 20f, out var zero), Is.False);
            Assert.That(zero, Is.EqualTo(Float2.Zero));
            Assert.That(AimAssistTargeting.TryAssist(Float2.Zero, Float2.Up, candidates, 0, 10f, 20f, out var none), Is.False);
            Assert.That(none, Is.EqualTo(Float2.Up));
        }
    }
}
