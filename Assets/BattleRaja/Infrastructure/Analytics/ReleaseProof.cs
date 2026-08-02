using System;
using System.Collections.Generic;

namespace BattleRaja.Infrastructure.Analytics
{
    public readonly struct AnalyticsEvent
    {
        public AnalyticsEvent(string name, string buildVersion, string platform, int matchDurationSeconds)
        {
            Name = name ?? string.Empty;
            BuildVersion = buildVersion ?? string.Empty;
            Platform = platform ?? string.Empty;
            MatchDurationSeconds = matchDurationSeconds;
        }

        public string Name { get; }
        public string BuildVersion { get; }
        public string Platform { get; }
        public int MatchDurationSeconds { get; }

        public bool IsValid(out string reason)
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length > 48 || string.IsNullOrWhiteSpace(BuildVersion) || BuildVersion.Length > 32 || string.IsNullOrWhiteSpace(Platform) || MatchDurationSeconds < 0)
            {
                reason = "Event fields are missing or outside the bounded schema.";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }

    public interface IAnalyticsSink
    {
        bool IsAvailable { get; }
        bool TryRecord(AnalyticsEvent analyticsEvent);
    }

    public sealed class DevelopmentAnalyticsSink : IAnalyticsSink
    {
        private readonly List<AnalyticsEvent> _events = new List<AnalyticsEvent>(128);
        public bool IsAvailable => true;
        public IReadOnlyList<AnalyticsEvent> Events => _events;

        public bool TryRecord(AnalyticsEvent analyticsEvent)
        {
            if (!analyticsEvent.IsValid(out _)) return false;
            if (_events.Count >= 128) _events.RemoveAt(0);
            _events.Add(analyticsEvent);
            return true;
        }
    }

    public sealed class CrashReportingAdapter
    {
        public bool IsAvailable => false;
        public bool TryInitialize() => false;
    }

    public readonly struct ReleaseCandidateConfiguration
    {
        public ReleaseCandidateConfiguration(string version, string applicationId, bool developmentBuild, bool adminToolsEnabled, bool containsSecrets)
        {
            Version = version ?? string.Empty;
            ApplicationId = applicationId ?? string.Empty;
            DevelopmentBuild = developmentBuild;
            AdminToolsEnabled = adminToolsEnabled;
            ContainsSecrets = containsSecrets;
        }

        public string Version { get; }
        public string ApplicationId { get; }
        public bool DevelopmentBuild { get; }
        public bool AdminToolsEnabled { get; }
        public bool ContainsSecrets { get; }

        public bool IsSafeForClosedTest(out string reason)
        {
            if (string.IsNullOrWhiteSpace(Version) || string.IsNullOrWhiteSpace(ApplicationId) || !DevelopmentBuild || AdminToolsEnabled || ContainsSecrets)
            {
                reason = "Closed-test candidate must be versioned, development-build enabled, admin-disabled and secret-free.";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }
}
