using System.Collections.Generic;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    [TestFixture]
    public class ReplayDeterminismTests
    {
        [Test]
        public void ReplayHasher_ProducesIdenticalHash_ForSameState()
        {
            var pos = new Float2(10f, 20f);
            var snapshot1 = new MatchParticipantSnapshot(new CombatEntityId(1), pos, 100, 100, true, 1, 0, 0, 0, 0f);
            var snapshot2 = new MatchParticipantSnapshot(new CombatEntityId(1), pos, 100, 100, true, 1, 0, 0, 0, 0f);

            var hash1 = DeterministicReplayHasher.CalculateTickHash(0, MatchPhase.Opening, Float2.Zero, 30f, new[] { snapshot1 }, null);
            var hash2 = DeterministicReplayHasher.CalculateTickHash(0, MatchPhase.Opening, Float2.Zero, 30f, new[] { snapshot2 }, null);

            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void MatchReplayFile_RecordsFrameAndDetectsStateParity()
        {
            var p1 = new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100);
            var header = new MatchReplayHeader("1.0.0-bazaar", 12345, new[] { p1 });
            var replay = new MatchReplayFile(header);

            var snapshot = new MatchParticipantSnapshot(new CombatEntityId(1), Float2.Zero, 100, 100, true, 1, 0, 0, 0, 0f);
            var hash = DeterministicReplayHasher.CalculateTickHash(0, MatchPhase.Opening, Float2.Zero, 30f, new[] { snapshot }, null);

            var frame = new MatchReplayFrame(0, null, null, null);
            replay.AddFrame(frame, hash);

            Assert.AreEqual(1, replay.Frames.Count);
            Assert.AreEqual(hash, replay.TickStateHashes[0]);
        }

        [Test]
        public void ExtendedAuthorityHash_DetectsCanonicalGadgetState()
        {
            var spawns = new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            };

            var baseline = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            baseline.Start(spawns);
            var baselineTick = baseline.Advance(1, 1f / 30f);
            var baselineHash = DeterministicReplayHasher.CalculateTickHash(
                baseline,
                baselineTick,
                baseline.Simulation.GetSnapshots());

            var gadgetId = GadgetDefinition.TiffinStation.GadgetId;
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                null,
                new[] { new GadgetPickupDefinition(0, gadgetId, Float2.Zero, 1.3f) });
            authority.Start(spawns);
            var collectionTick = authority.Advance(1, 1f / 30f);

            Assert.That(collectionTick.GadgetCollections, Has.Length.EqualTo(1));
            Assert.That(collectionTick.GadgetCollections[0].CollectionEventId, Is.EqualTo(1));

            var used = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                gadgetId,
                Float2.Zero,
                Float2.Up,
                1));
            Assert.That(used.Used, Is.True);
            Assert.That(used.EventId, Is.EqualTo(1));

            var authorityHash = DeterministicReplayHasher.CalculateTickHash(
                authority,
                collectionTick,
                authority.Simulation.GetSnapshots());

            Assert.That(authorityHash, Is.Not.EqualTo(baselineHash));
        }
    }
}
