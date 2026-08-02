using BattleRaja.Core.Domain;
using BattleRaja.Infrastructure.Networking;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class NetworkingProofTests
    {
        [Test]
        public void ConfigRejectsVersionMismatchAndMockJoinsTwoClients()
        {
            var session = new NetworkSessionMock(NetworkSessionConfig.Proof);
            Assert.That(session.Start("wrong"), Is.EqualTo(NetworkConnectFailure.VersionMismatch));
            Assert.That(session.Start(NetworkSessionConfig.Proof.ProtocolVersion), Is.EqualTo(NetworkConnectFailure.None));
            Assert.That(session.Join(1, NetworkSessionConfig.Proof.ProtocolVersion), Is.EqualTo(NetworkConnectFailure.None));
            Assert.That(session.Join(2, NetworkSessionConfig.Proof.ProtocolVersion), Is.EqualTo(NetworkConnectFailure.None));
            Assert.That(session.ConnectedClients, Is.EqualTo(2));
            Assert.That(session.Join(3, NetworkSessionConfig.Proof.ProtocolVersion), Is.EqualTo(NetworkConnectFailure.RoomFull));
        }

        [Test]
        public void MockReplicatesInputAndAuthoritativeDamageOnce()
        {
            var session = new NetworkSessionMock(NetworkSessionConfig.Proof);
            session.Start(NetworkSessionConfig.Proof.ProtocolVersion);
            session.Join(1, NetworkSessionConfig.Proof.ProtocolVersion);
            Assert.That(session.SubmitInput(new NetworkInputFrame(1, 1, new Float2(1f, 0f), Float2.Up, false, false, default(ContentId))), Is.True);
            Assert.That(session.GetSnapshot(1).Position.X, Is.GreaterThan(0f));
            Assert.That(session.ApplyAuthoritativeDamage(1, 40, 2), Is.True);
            Assert.That(session.ApplyAuthoritativeDamage(1, 70, 3), Is.True);
            Assert.That(session.ApplyAuthoritativeDamage(1, 1, 4), Is.False);
            Assert.That(session.GetSnapshot(1).Eliminated, Is.True);
        }

        [Test]
        public void PacketLossAndDiagnosticsAreObservable()
        {
            var session = new NetworkSessionMock(NetworkSessionConfig.Proof, 11);
            session.Start(NetworkSessionConfig.Proof.ProtocolVersion);
            session.Join(1, NetworkSessionConfig.Proof.ProtocolVersion);
            session.SetConditions(new NetworkConditionProfile(200, 60, 1f));
            Assert.That(session.SubmitInput(new NetworkInputFrame(1, 1, Float2.Up, Float2.Up, false, false, default(ContentId))), Is.False);
            Assert.That(session.Diagnostics.SentPackets, Is.EqualTo(1));
            Assert.That(session.Diagnostics.DroppedPackets, Is.EqualTo(1));
        }

        [Test]
        public void PhotonAdapterIsExplicitlyCredentialBlockedWithoutPackageReference()
        {
            var adapter = new PhotonFusionAdapter();
            Assert.That(adapter.IsAvailable, Is.False);
            Assert.That(adapter.TryConnect("proof", NetworkSessionConfig.Proof.ProtocolVersion), Is.EqualTo(NetworkConnectFailure.CredentialsRequired));
        }
    }
}
