using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BattleRaja.Infrastructure.Backend
{
    public enum BackendPlatform
    {
        Android = 1,
        Web = 2
    }

    public enum BackendFailure
    {
        None = 0,
        CredentialsRequired = 1,
        InvalidIdentity = 2,
        NotFound = 3,
        LinkConflict = 4,
        InvalidDisplayName = 5,
        DisplayNameTaken = 6,
        InvalidRewardEvidence = 7,
        RewardReplay = 8,
        VersionConflict = 9,
        Offline = 10
    }

    public readonly struct BackendConfiguration
    {
        public BackendConfiguration(string titleId, string environment, bool allowClientAccountCreation)
        {
            TitleId = titleId ?? string.Empty;
            Environment = environment ?? string.Empty;
            AllowClientAccountCreation = allowClientAccountCreation;
        }

        public string TitleId { get; }
        public string Environment { get; }
        public bool AllowClientAccountCreation { get; }
        public bool IsConfigured => !string.IsNullOrWhiteSpace(TitleId) && !string.IsNullOrWhiteSpace(Environment);
        public static BackendConfiguration LocalProof => new BackendConfiguration(string.Empty, "local-proof", false);
    }

    public readonly struct BackendIdentity
    {
        public BackendIdentity(string accountId, BackendPlatform platform, string localId)
        {
            AccountId = accountId ?? string.Empty;
            Platform = platform;
            LocalId = localId ?? string.Empty;
        }

        public string AccountId { get; }
        public BackendPlatform Platform { get; }
        public string LocalId { get; }
    }

    public readonly struct MatchRewardEvidence
    {
        public MatchRewardEvidence(string matchId, string idempotencyKey, int xp, int softCurrency, bool serverValidated)
        {
            MatchId = matchId ?? string.Empty;
            IdempotencyKey = idempotencyKey ?? string.Empty;
            Xp = xp;
            SoftCurrency = softCurrency;
            ServerValidated = serverValidated;
        }

        public string MatchId { get; }
        public string IdempotencyKey { get; }
        public int Xp { get; }
        public int SoftCurrency { get; }
        public bool ServerValidated { get; }
    }

    public readonly struct BackendProfile
    {
        public BackendProfile(string accountId, string displayName, int level, int xp, int softCurrency, int premiumCurrency, string[] ownedCosmetics, int bijliMastery, int pehelMastery, int mayaMastery)
        {
            AccountId = accountId;
            DisplayName = displayName;
            Level = level;
            Xp = xp;
            SoftCurrency = softCurrency;
            PremiumCurrency = premiumCurrency;
            OwnedCosmetics = ownedCosmetics ?? Array.Empty<string>();
            BijliMastery = bijliMastery;
            PehelMastery = pehelMastery;
            MayaMastery = mayaMastery;
        }

        public string AccountId { get; }
        public string DisplayName { get; }
        public int Level { get; }
        public int Xp { get; }
        public int SoftCurrency { get; }
        public int PremiumCurrency { get; }
        public string[] OwnedCosmetics { get; }
        public int BijliMastery { get; }
        public int PehelMastery { get; }
        public int MayaMastery { get; }
    }

    public readonly struct LeaderboardEntry
    {
        public LeaderboardEntry(string accountId, string displayName, int score, int rank)
        {
            AccountId = accountId;
            DisplayName = displayName;
            Score = score;
            Rank = rank;
        }

        public string AccountId { get; }
        public string DisplayName { get; }
        public int Score { get; }
        public int Rank { get; }
    }

    public readonly struct RemoteMatchConfig
    {
        public RemoteMatchConfig(string version, int targetDurationSeconds, int maxRewardXp, int maxRewardSoftCurrency)
        {
            Version = version ?? string.Empty;
            TargetDurationSeconds = targetDurationSeconds;
            MaxRewardXp = maxRewardXp;
            MaxRewardSoftCurrency = maxRewardSoftCurrency;
        }

        public string Version { get; }
        public int TargetDurationSeconds { get; }
        public int MaxRewardXp { get; }
        public int MaxRewardSoftCurrency { get; }
    }

    public interface IProgressionBackend
    {
        bool IsAvailable { get; }
        BackendFailure LoginGuest(BackendPlatform platform, string localId, out BackendIdentity identity);
        BackendFailure LinkIdentity(BackendIdentity identity, BackendPlatform platform, string externalId);
        BackendFailure SetDisplayName(BackendIdentity identity, string displayName);
        BackendFailure GetProfile(BackendIdentity identity, out BackendProfile profile);
        BackendFailure GrantMatchReward(BackendIdentity identity, MatchRewardEvidence evidence);
        BackendFailure AddCosmetic(BackendIdentity identity, string cosmeticId);
        BackendFailure SubmitLeaderboardScore(BackendIdentity identity, string leaderboardId, int score, bool serverValidated);
        BackendFailure GetLeaderboard(string leaderboardId, out LeaderboardEntry[] entries);
        RemoteMatchConfig GetRemoteMatchConfig();
    }

    public sealed class PlayFabBackendAdapter : IProgressionBackend
    {
        public PlayFabBackendAdapter(BackendConfiguration configuration) => Configuration = configuration;
        public BackendConfiguration Configuration { get; }
        public bool IsAvailable => false;
        public BackendFailure LoginGuest(BackendPlatform platform, string localId, out BackendIdentity identity) { identity = default(BackendIdentity); return BackendFailure.CredentialsRequired; }
        public BackendFailure LinkIdentity(BackendIdentity identity, BackendPlatform platform, string externalId) => BackendFailure.CredentialsRequired;
        public BackendFailure SetDisplayName(BackendIdentity identity, string displayName) => BackendFailure.CredentialsRequired;
        public BackendFailure GetProfile(BackendIdentity identity, out BackendProfile profile) { profile = default(BackendProfile); return BackendFailure.CredentialsRequired; }
        public BackendFailure GrantMatchReward(BackendIdentity identity, MatchRewardEvidence evidence) => BackendFailure.CredentialsRequired;
        public BackendFailure AddCosmetic(BackendIdentity identity, string cosmeticId) => BackendFailure.CredentialsRequired;
        public BackendFailure SubmitLeaderboardScore(BackendIdentity identity, string leaderboardId, int score, bool serverValidated) => BackendFailure.CredentialsRequired;
        public BackendFailure GetLeaderboard(string leaderboardId, out LeaderboardEntry[] entries) { entries = Array.Empty<LeaderboardEntry>(); return BackendFailure.CredentialsRequired; }
        public RemoteMatchConfig GetRemoteMatchConfig() => new RemoteMatchConfig("unconfigured", 298, 0, 0);
    }

    public sealed class FakeProgressionBackend : IProgressionBackend
    {
        private static readonly Regex IdentityPattern = new Regex("^[A-Za-z0-9_.:-]{3,96}$", RegexOptions.Compiled);
        private static readonly Regex DisplayNamePattern = new Regex("^[A-Za-z0-9 _-]{3,20}$", RegexOptions.Compiled);
        private readonly Dictionary<string, Account> _accounts = new Dictionary<string, Account>(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _identityToAccount = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _displayNameToAccount = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Dictionary<string, int>> _leaderboards = new Dictionary<string, Dictionary<string, int>>(StringComparer.Ordinal);
        private int _nextAccount = 1;

        public bool IsAvailable => true;
        public RemoteMatchConfig RemoteConfig { get; set; } = new RemoteMatchConfig("local-v1", 298, 100, 50);

        public BackendFailure LoginGuest(BackendPlatform platform, string localId, out BackendIdentity identity)
        {
            identity = default(BackendIdentity);
            if (!IsValidIdentity(localId)) return BackendFailure.InvalidIdentity;
            var key = Key(platform, localId);
            if (!_identityToAccount.TryGetValue(key, out var accountId))
            {
                accountId = "acct-" + _nextAccount++;
                var account = new Account(accountId, "Raja" + accountId.Substring(5));
                _accounts.Add(accountId, account);
                _identityToAccount.Add(key, accountId);
                _displayNameToAccount.Add(account.DisplayName, accountId);
            }

            identity = new BackendIdentity(accountId, platform, localId);
            return BackendFailure.None;
        }

        public BackendFailure LinkIdentity(BackendIdentity identity, BackendPlatform platform, string externalId)
        {
            if (!TryGetAccount(identity, out var account) || !IsValidIdentity(externalId)) return BackendFailure.InvalidIdentity;
            var key = Key(platform, externalId);
            if (_identityToAccount.TryGetValue(key, out var existing) && !string.Equals(existing, account.AccountId, StringComparison.Ordinal)) return BackendFailure.LinkConflict;
            _identityToAccount[key] = account.AccountId;
            return BackendFailure.None;
        }

        public BackendFailure SetDisplayName(BackendIdentity identity, string displayName)
        {
            if (!TryGetAccount(identity, out var account)) return BackendFailure.NotFound;
            if (string.IsNullOrWhiteSpace(displayName) || !DisplayNamePattern.IsMatch(displayName)) return BackendFailure.InvalidDisplayName;
            if (_displayNameToAccount.TryGetValue(displayName, out var owner) && !string.Equals(owner, account.AccountId, StringComparison.Ordinal)) return BackendFailure.DisplayNameTaken;
            _displayNameToAccount.Remove(account.DisplayName);
            account.DisplayName = displayName;
            _displayNameToAccount[displayName] = account.AccountId;
            return BackendFailure.None;
        }

        public BackendFailure GetProfile(BackendIdentity identity, out BackendProfile profile)
        {
            if (!TryGetAccount(identity, out var account)) { profile = default(BackendProfile); return BackendFailure.NotFound; }
            profile = account.ToProfile();
            return BackendFailure.None;
        }

        public BackendFailure GrantMatchReward(BackendIdentity identity, MatchRewardEvidence evidence)
        {
            if (!TryGetAccount(identity, out var account)) return BackendFailure.NotFound;
            if (!evidence.ServerValidated || string.IsNullOrWhiteSpace(evidence.MatchId) || string.IsNullOrWhiteSpace(evidence.IdempotencyKey) || evidence.Xp < 0 || evidence.SoftCurrency < 0 || evidence.Xp > RemoteConfig.MaxRewardXp || evidence.SoftCurrency > RemoteConfig.MaxRewardSoftCurrency) return BackendFailure.InvalidRewardEvidence;
            if (account.RewardKeys.Contains(evidence.IdempotencyKey)) return BackendFailure.RewardReplay;
            account.RewardKeys.Add(evidence.IdempotencyKey);
            account.Xp += evidence.Xp;
            account.SoftCurrency += evidence.SoftCurrency;
            account.Level = 1 + (account.Xp / 100);
            return BackendFailure.None;
        }

        public BackendFailure AddCosmetic(BackendIdentity identity, string cosmeticId)
        {
            if (!TryGetAccount(identity, out var account)) return BackendFailure.NotFound;
            if (string.IsNullOrWhiteSpace(cosmeticId) || cosmeticId.Length > 64) return BackendFailure.InvalidIdentity;
            account.OwnedCosmetics.Add(cosmeticId);
            return BackendFailure.None;
        }

        public BackendFailure SubmitLeaderboardScore(BackendIdentity identity, string leaderboardId, int score, bool serverValidated)
        {
            if (!TryGetAccount(identity, out var account)) return BackendFailure.NotFound;
            if (!serverValidated || string.IsNullOrWhiteSpace(leaderboardId) || score < 0) return BackendFailure.InvalidRewardEvidence;
            if (!_leaderboards.TryGetValue(leaderboardId, out var scores)) { scores = new Dictionary<string, int>(StringComparer.Ordinal); _leaderboards.Add(leaderboardId, scores); }
            if (!scores.TryGetValue(account.AccountId, out var previous) || score > previous) scores[account.AccountId] = score;
            return BackendFailure.None;
        }

        public BackendFailure GetLeaderboard(string leaderboardId, out LeaderboardEntry[] entries)
        {
            entries = Array.Empty<LeaderboardEntry>();
            if (string.IsNullOrWhiteSpace(leaderboardId) || !_leaderboards.TryGetValue(leaderboardId, out var scores)) return BackendFailure.NotFound;
            var ordered = new List<KeyValuePair<string, int>>(scores);
            ordered.Sort((left, right) => right.Value.CompareTo(left.Value));
            entries = new LeaderboardEntry[ordered.Count];
            for (var i = 0; i < ordered.Count; i++) entries[i] = new LeaderboardEntry(ordered[i].Key, _accounts[ordered[i].Key].DisplayName, ordered[i].Value, i + 1);
            return BackendFailure.None;
        }

        public RemoteMatchConfig GetRemoteMatchConfig() => RemoteConfig;

        private bool TryGetAccount(BackendIdentity identity, out Account account) => _accounts.TryGetValue(identity.AccountId ?? string.Empty, out account);
        private static bool IsValidIdentity(string value) => !string.IsNullOrWhiteSpace(value) && IdentityPattern.IsMatch(value);
        private static string Key(BackendPlatform platform, string localId) => platform + ":" + localId;

        private sealed class Account
        {
            public Account(string accountId, string displayName) { AccountId = accountId; DisplayName = displayName; }
            public string AccountId;
            public string DisplayName;
            public int Level = 1;
            public int Xp;
            public int SoftCurrency;
            public int PremiumCurrency;
            public readonly HashSet<string> OwnedCosmetics = new HashSet<string>(StringComparer.Ordinal);
            public readonly HashSet<string> RewardKeys = new HashSet<string>(StringComparer.Ordinal);
            public BackendProfile ToProfile() => new BackendProfile(AccountId, DisplayName, Level, Xp, SoftCurrency, PremiumCurrency, new List<string>(OwnedCosmetics).ToArray(), 0, 0, 0);
        }
    }
}
