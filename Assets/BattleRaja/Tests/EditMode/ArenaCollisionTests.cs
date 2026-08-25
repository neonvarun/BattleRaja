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
            authority.Advance(9f);

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
            authority.Advance(9f);

            var result = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                gadgetId,
                new Float2(99f, 99f),
                new Float2(0f, 1f),
                1));

            Assert.That(result.Used, Is.True);
            Assert.That(result.Effect.Command.Origin, Is.EqualTo(new Float2(0f, 0.8f)).Using<Float2>(new Float2Comparer(0.0001f)));
        }

        [Test]
        public void DiagonalMoveIntoBoundsCornerClampsBothAxesAndStaysUnblocked()
        {
            var definition = ArenaCollisionDefinition.BazaarBastion;
            var solver = new DeterministicCollisionSolver(definition);

            // The north-west open quadrant reaches the bounds corner without
            // crossing any authored obstacle, isolating corner clamping.
            var result = solver.Move(new Float2(-11f, 8f), new Float2(-100f, 100f));

            Assert.That(result.Position.X, Is.EqualTo(-12.75f).Within(0.0001f));
            Assert.That(result.Position.Y, Is.EqualTo(8.75f).Within(0.0001f));
            Assert.That(result.Collided, Is.True);
            Assert.That(definition.IsPointBlocked(result.Position), Is.False);
        }

        [Test]
        public void LargeDisplacementsCannotTunnelThinBazaarWalls()
        {
            var definition = ArenaCollisionDefinition.BazaarBastion;
            var solver = new DeterministicCollisionSolver(definition);
            var start = new Float2(-5f, 0f);
            Assert.That(definition.IsPointBlocked(start), Is.False);

            // NarrowLaneWest is only 0.45 thick; even displacements far larger
            // than one movement tick must stop at its expanded near face instead
            // of appearing on the far side.
            var straight = solver.Move(start, new Float2(30f, 0f));
            var diagonal = solver.Move(start, new Float2(30f, 30f));

            Assert.That(straight.Collided, Is.True);
            Assert.That(straight.Position.X, Is.EqualTo(-3.6751f).Within(0.001f));
            Assert.That(definition.IsPointBlocked(straight.Position), Is.False);

            Assert.That(diagonal.Collided, Is.True);
            Assert.That(diagonal.Position.X, Is.EqualTo(-3.6751f).Within(0.001f));
            Assert.That(diagonal.Position.Y, Is.GreaterThan(0f));
            Assert.That(definition.IsPointBlocked(diagonal.Position), Is.False);
        }

        [Test]
        public void RepeatedArbitraryMovesKeepActorOutsideObstacleFootprints()
        {
            var definition = ArenaCollisionDefinition.BazaarBastion;
            var solver = new DeterministicCollisionSolver(definition);
            var start = new Float2(0f, -7f);
            Assert.That(definition.IsPointBlocked(start), Is.False);

            // Deterministic seeded walk with per-step speeds beyond normal play;
            // every intermediate and final position must remain unblocked.
            uint seed = 20260822u;
            var position = start;
            for (var step = 0; step < 400; step++)
            {
                seed = seed * 1664525u + 1013904223u;
                var dx = ((seed >> 8) % 1201 - 600) / 1000f;
                seed = seed * 1664525u + 1013904223u;
                var dy = ((seed >> 8) % 1201 - 600) / 1000f;
                var result = solver.Move(position, new Float2(dx, dy));
                position = result.Position;
                Assert.That(definition.IsPointBlocked(position), Is.False,
                    $"Actor ended inside a footprint at step {step}: ({position.X}, {position.Y})");
            }
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
