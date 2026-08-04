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
    }
}
