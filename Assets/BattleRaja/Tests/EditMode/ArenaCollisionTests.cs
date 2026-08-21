using System.Collections.Generic;
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

            // Start inside the south-east open quadrant so the straight run to the
            // maximum-bounds corner crosses no authored Bazaar obstacle; the test
            // isolates bounds clamping from obstacle sliding (covered separately).
            var result = solver.Move(new Float2(11.5f, -8f), new Float2(100f, -100f));

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

        [Test]
        public void MayaPlacementUsesCanonicalOwnerPosition()
        {
            var authority = new BattleRaja.Core.Application.OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new[]
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(2f, 3f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(-2f, -3f), 100)
            });

            var snapshot = authority.TrySpawnMayaDecoy(new CombatEntityId(1), 1, new Float2(99f, 99f));

            Assert.That(snapshot.Active, Is.True);
            Assert.That(snapshot.Position, Is.EqualTo(new Float2(2f, 3f)));
        }

        [Test]
        public void TiffinPlacementUsesCanonicalActorAndAim()
        {
            var authority = new BattleRaja.Core.Application.OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new[]
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(0f, 0f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(-2f, -3f), 100)
            });
            var gadgetId = ContentId.Gadget("gadget.tiffin_station");
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);

            var result = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                gadgetId,
                new Float2(99f, 99f),
                new Float2(0f, 1f),
                1));

            Assert.That(result.Used, Is.True);
            Assert.That(result.Effect.Command.Origin, Is.EqualTo(new Float2(0f, 0.8f)).Using<Float2>(new Float2Comparer(0.0001f)));
        }

        private sealed class Float2Comparer : IEqualityComparer<Float2>
        {
            private readonly float _tolerance;

            public Float2Comparer(float tolerance)
            {
                _tolerance = tolerance;
            }

            public bool Equals(Float2 left, Float2 right) =>
                System.Math.Abs(left.X - right.X) <= _tolerance && System.Math.Abs(left.Y - right.Y) <= _tolerance;

            public int GetHashCode(Float2 value) => value.GetHashCode();
        }
    }
}
