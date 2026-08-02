using BattleRaja.Infrastructure.Backend;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class ProgressionBackendProofTests
    {
        [Test]
        public void GuestLoginIsStableAcrossAndroidAndWebIdentityReuse()
        {
            var backend = new FakeProgressionBackend();
            Assert.That(backend.LoginGuest(BackendPlatform.Android, "lava-guest-001", out var android), Is.EqualTo(BackendFailure.None));
            Assert.That(backend.LoginGuest(BackendPlatform.Android, "lava-guest-001", out var repeat), Is.EqualTo(BackendFailure.None));
            Assert.That(repeat.AccountId, Is.EqualTo(android.AccountId));
            Assert.That(backend.LoginGuest(BackendPlatform.Web, "lava-guest-001", out var web), Is.EqualTo(BackendFailure.None));
            Assert.That(web.AccountId, Is.Not.EqualTo(android.AccountId));
        }

        [Test]
        public void LinkConflictAndDisplayNameRulesAreExplicit()
        {
            var backend = new FakeProgressionBackend();
            backend.LoginGuest(BackendPlatform.Android, "guest-a", out var a);
            backend.LoginGuest(BackendPlatform.Web, "guest-b", out var b);
            Assert.That(backend.LinkIdentity(a, BackendPlatform.Web, "recover-a"), Is.EqualTo(BackendFailure.None));
            Assert.That(backend.LinkIdentity(b, BackendPlatform.Web, "recover-a"), Is.EqualTo(BackendFailure.LinkConflict));
            Assert.That(backend.SetDisplayName(a, "Raja One"), Is.EqualTo(BackendFailure.None));
            Assert.That(backend.SetDisplayName(b, "Raja One"), Is.EqualTo(BackendFailure.DisplayNameTaken));
            Assert.That(backend.SetDisplayName(b, "bad!"), Is.EqualTo(BackendFailure.InvalidDisplayName));
        }

        [Test]
        public void RewardsAreServerValidatedAndIdempotent()
        {
            var backend = new FakeProgressionBackend();
            backend.LoginGuest(BackendPlatform.Android, "guest-reward", out var identity);
            var evidence = new MatchRewardEvidence("match-1", "reward-key-1", 80, 20, true);
            Assert.That(backend.GrantMatchReward(identity, evidence), Is.EqualTo(BackendFailure.None));
            Assert.That(backend.GrantMatchReward(identity, evidence), Is.EqualTo(BackendFailure.RewardReplay));
            Assert.That(backend.GrantMatchReward(identity, new MatchRewardEvidence("match-2", "reward-key-2", 1000, 0, false)), Is.EqualTo(BackendFailure.InvalidRewardEvidence));
            backend.GetProfile(identity, out var profile);
            Assert.That(profile.Xp, Is.EqualTo(80));
            Assert.That(profile.SoftCurrency, Is.EqualTo(20));
        }

        [Test]
        public void InventoryAndLeaderboardOnlyAcceptTrustedWrites()
        {
            var backend = new FakeProgressionBackend();
            backend.LoginGuest(BackendPlatform.Web, "guest-score", out var identity);
            Assert.That(backend.AddCosmetic(identity, "cosmetic.blue-cloth"), Is.EqualTo(BackendFailure.None));
            Assert.That(backend.SubmitLeaderboardScore(identity, "wins", 1, false), Is.EqualTo(BackendFailure.InvalidRewardEvidence));
            Assert.That(backend.SubmitLeaderboardScore(identity, "wins", 3, true), Is.EqualTo(BackendFailure.None));
            Assert.That(backend.GetLeaderboard("wins", out var entries), Is.EqualTo(BackendFailure.None));
            Assert.That(entries, Has.Length.EqualTo(1));
            Assert.That(entries[0].Score, Is.EqualTo(3));
        }

        [Test]
        public void PlayFabAdapterIsCredentialBlockedAndConfigHasNoSecret()
        {
            var adapter = new PlayFabBackendAdapter(BackendConfiguration.LocalProof);
            Assert.That(adapter.IsAvailable, Is.False);
            Assert.That(adapter.Configuration.IsConfigured, Is.False);
            Assert.That(adapter.LoginGuest(BackendPlatform.Web, "guest", out _), Is.EqualTo(BackendFailure.CredentialsRequired));
        }
    }
}
