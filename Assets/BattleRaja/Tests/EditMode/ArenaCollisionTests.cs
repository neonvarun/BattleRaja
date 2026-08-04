using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class ArenaCollisionTests
    {
        [Test]
        public void BazaarBoundsClampActorRadiusDeterministically()
        {
            var solver = new DeterministicCollisionSolver(ArenaCollisionDefinition.BazaarBastion);

            var result = solver.Move(new Float2(0f, 0f), new Float2(100f, -100f));

            Assert.That(result.Position.X, Is.EqualTo(12.75f).Within(0.0001f));
            Assert.That(result.Position.Y, Is.EqualTo(-8.75f).Within(0.0001f));
            Assert.That(result.Collided, Is.True);
        }

        [Test]
        public void OrderedObstacleStopsCrossingAxisAndPreservesSlide()
        {
            var definition = new ArenaCollisionDefinition(
                new Float2(-5f, -5f),
                new Float2(5f, 5f),
                0.25f,
                new[] { new ArenaObstacle(20, new Float2(0f, -1f), new Float2(1f, 1f)) });
            var solver = new DeterministicCollisionSolver(definition);

            var result = solver.Move(new Float2(-2f, 0f), new Float2(5f, 3f));

            Assert.That(result.Collided, Is.True);
            Assert.That(result.Position.X, Is.EqualTo(-0.2501f).Within(0.0002f));
            Assert.That(result.Position.Y, Is.EqualTo(3f).Within(0.0002f));
        }

        [Test]
        public void ObstacleOrderingIsIndependentOfInputArrayOrder()
        {
            var first = new ArenaCollisionDefinition(
                new Float2(-5f, -5f),
                new Float2(5f, 5f),
                0.25f,
                new[]
                {
                    new ArenaObstacle(2, new Float2(1f, -1f), new Float2(2f, 1f)),
                    new ArenaObstacle(1, new Float2(0f, -1f), new Float2(0.5f, 1f))
                });
            var second = new ArenaCollisionDefinition(
                new Float2(-5f, -5f),
                new Float2(5f, 5f),
                0.25f,
                new[]
                {
                    new ArenaObstacle(1, new Float2(0f, -1f), new Float2(0.5f, 1f)),
                    new ArenaObstacle(2, new Float2(1f, -1f), new Float2(2f, 1f))
                });

            var firstResult = new DeterministicCollisionSolver(first).Move(new Float2(-2f, 0f), new Float2(5f, 0f));
            var secondResult = new DeterministicCollisionSolver(second).Move(new Float2(-2f, 0f), new Float2(5f, 0f));

            Assert.That(secondResult.Position, Is.EqualTo(firstResult.Position));
            Assert.That(secondResult.AppliedDisplacement, Is.EqualTo(firstResult.AppliedDisplacement));
        }
    }
}
