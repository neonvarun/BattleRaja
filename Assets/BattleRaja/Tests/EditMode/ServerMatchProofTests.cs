using BattleRaja.Infrastructure.Networking;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class ServerMatchProofTests
    {
        [Test]
        public void ServerBackfillsEightSlotsBeforeStartingMatch()
        {
            var server = new AuthoritativeMatchServer(ServerMatchConfig.M9Proof);
            Assert.That(server.StartRoom(ServerMatchConfig.M9Proof.ProtocolVersion), Is.EqualTo(NetworkConnectFailure.None));
            Assert.That(server.Join(1, ServerMatchConfig.M9Proof.ProtocolVersion), Is.EqualTo(NetworkConnectFailure.None));
            Assert.That(server.Join(2, ServerMatchConfig.M9Proof.ProtocolVersion), Is.EqualTo(NetworkConnectFailure.None));
            Assert.That(server.FillWithBots(), Is.EqualTo(6));
            Assert.That(server.StartMatch(), Is.True);
            Assert.That(server.SlotCount, Is.EqualTo(8));
            Assert.That(server.GetMatchSnapshots(), Has.Length.EqualTo(8));
        }

        [Test]
        public void ServerClampsMovementAndRejectsStaleInput()
        {
            var server = new AuthoritativeMatchServer(ServerMatchConfig.M9Proof);
            server.StartRoom(ServerMatchConfig.M9Proof.ProtocolVersion);
            server.Join(1, ServerMatchConfig.M9Proof.ProtocolVersion);
            server.Join(2, ServerMatchConfig.M9Proof.ProtocolVersion);
            server.StartMatch();
            var input = new NetworkInputFrame(1, 1, new BattleRaja.Core.Domain.Float2(100f, 0f), BattleRaja.Core.Domain.Float2.Up, false, false, default(BattleRaja.Core.Domain.ContentId));
            Assert.That(server.TrySubmitInput(input), Is.True);
            Assert.That(server.TrySubmitInput(input), Is.False);
            var snapshot = server.GetMatchSnapshot(1);
            Assert.That(snapshot.Position.Magnitude, Is.LessThan(8.1f));
        }

        [Test]
        public void DisconnectUsesGraceThenBotTakeoverAndAllowsReconnect()
        {
            var server = new AuthoritativeMatchServer(new ServerMatchConfig("m9-test", 8, 30, 2));
            server.StartRoom("m9-test");
            server.Join(1, "m9-test");
            server.Join(2, "m9-test");
            server.StartMatch();
            Assert.That(server.Disconnect(1), Is.True);
            Assert.That(server.Reconnect(1, "m9-test"), Is.True);
            Assert.That(server.Disconnect(1), Is.True);
            server.Advance(2);
            Assert.That(server.GetSlotSnapshots()[0].State, Is.EqualTo(ServerSlotState.BotTakeover));
            Assert.That(server.Reconnect(1, "m9-test"), Is.False);
        }

        [Test]
        public void ServerAuthorityDamageEliminatesOnceAndTracksState()
        {
            var server = new AuthoritativeMatchServer(ServerMatchConfig.M9Proof);
            server.StartRoom(ServerMatchConfig.M9Proof.ProtocolVersion);
            server.Join(1, ServerMatchConfig.M9Proof.ProtocolVersion);
            server.Join(2, ServerMatchConfig.M9Proof.ProtocolVersion);
            server.StartMatch();
            Assert.That(server.ApplyServerAuthorityDamage(1, 100), Is.True);
            Assert.That(server.ApplyServerAuthorityDamage(1, 1), Is.False);
            Assert.That(server.GetMatchSnapshot(1).Alive, Is.False);
        }
    }
}
