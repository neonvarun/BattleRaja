using BattleRaja.Infrastructure.Analytics;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class ReleaseProofTests
    {
        [Test]
        public void DevelopmentAnalyticsUsesBoundedNonIdentitySchema()
        {
            var sink = new DevelopmentAnalyticsSink();
            Assert.That(sink.TryRecord(new AnalyticsEvent("match_completed", "m11.0.0", "android", 298)), Is.True);
            Assert.That(sink.TryRecord(new AnalyticsEvent(string.Empty, "m11.0.0", "android", 0)), Is.False);
            Assert.That(sink.Events, Has.Count.EqualTo(1));
        }

        [Test]
        public void AnalyticsBufferIsBounded()
        {
            var sink = new DevelopmentAnalyticsSink();
            for (var i = 0; i < 140; i++) Assert.That(sink.TryRecord(new AnalyticsEvent("heartbeat", "m11.0.0", "web", i)), Is.True);
            Assert.That(sink.Events, Has.Count.EqualTo(128));
        }

        [Test]
        public void CrashAdapterIsExplicitlyUnavailableWithoutSdk()
        {
            var adapter = new CrashReportingAdapter();
            Assert.That(adapter.IsAvailable, Is.False);
            Assert.That(adapter.TryInitialize(), Is.False);
        }

        [Test]
        public void ClosedTestConfigurationRejectsSecretsAndAdminTools()
        {
            var safe = new ReleaseCandidateConfiguration("m11.0.0", "com.example.battleraja.m11", true, false, false);
            Assert.That(safe.IsSafeForClosedTest(out _), Is.True);
            var unsafeConfig = new ReleaseCandidateConfiguration("m11.0.0", "com.example.battleraja.m11", true, true, true);
            Assert.That(unsafeConfig.IsSafeForClosedTest(out _), Is.False);
        }
    }
}
