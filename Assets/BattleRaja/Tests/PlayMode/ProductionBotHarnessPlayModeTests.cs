using System;
using System.Collections;
using System.IO;
using System.Linq;
using BattleRaja.Presentation.AI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class ProductionBotHarnessPlayModeTests
    {
        private const float MinimumReleaseDurationSeconds = 240f;
        private const float MaximumReleaseDurationSeconds = 360f;
        private const float MaximumRejectedAbilityRatio = 0.70f;
        private const float MaximumOutOfRangeAttackRatio = 0.02f;
        private const int MaximumContinuousStuckTicks = 60;

        [UnityTest]
        [Timeout(900000)]
        public IEnumerator Harness_CompletesSeededMatches_ThroughProductionPipeline()
        {
            var harnessObject = new GameObject("ProductionBotMatchHarness");
            var harness = harnessObject.AddComponent<ProductionBotMatchHarness>();

            var matchCount = ResolveMatchCount();
            yield return harness.RunMatches(matchCount, 9101u, ResolvePlaybackScale());

            Assert.That(harness.Results, Has.Count.EqualTo(matchCount));
            for (var i = 0; i < harness.Results.Count; i++)
            {
                var result = harness.Results[i];
                Assert.That(result.Seed, Is.EqualTo(9101u + (uint)i));
                Assert.That(result.CompletedWithinTickBudget, Is.True, $"match {i} exceeded tick budget");
                Assert.That(result.DurationSeconds, Is.InRange(1f, 360f));
                Assert.That(result.IsBastionCrown, Is.True);
                Assert.That(result.BastionElapsedSeconds, Is.InRange(1f, 360f));
                Assert.That(result.WinnerTeam, Is.Not.Null.And.Not.Empty);
                Assert.That(result.ResultReason, Is.Not.Null.And.Not.Empty);
                Assert.That(result.RajaTicketsMaximum, Is.EqualTo(12));
                Assert.That(result.RivalTicketsMaximum, Is.EqualTo(12));
                Assert.That(result.RajaTicketsRemaining, Is.InRange(0, result.RajaTicketsMaximum));
                Assert.That(result.RivalTicketsRemaining, Is.InRange(0, result.RivalTicketsMaximum));
                Assert.That(result.RajaTicketsSpent, Is.InRange(0, result.RajaTicketsMaximum));
                Assert.That(result.RivalTicketsSpent, Is.InRange(0, result.RivalTicketsMaximum));
                Assert.That(result.RajaScore, Is.GreaterThanOrEqualTo(0));
                Assert.That(result.RivalScore, Is.GreaterThanOrEqualTo(0));
                Assert.That(result.RajaDeposits, Is.GreaterThanOrEqualTo(0));
                Assert.That(result.RivalDeposits, Is.GreaterThanOrEqualTo(0));
                Assert.That(result.RajaObjectiveSeconds, Is.GreaterThanOrEqualTo(0f));
                Assert.That(result.RivalObjectiveSeconds, Is.GreaterThanOrEqualTo(0f));
                Assert.That(result.SquadSignalUpdates, Is.GreaterThan(0));
                Assert.That(result.SquadPlanRefreshes, Is.GreaterThan(0));
                Assert.That(result.AllySpacingSamples, Is.GreaterThan(0));
                Assert.That(result.MinAllySpacingMeters, Is.GreaterThanOrEqualTo(0f));
                Assert.That(result.MaxAllySpacingMeters, Is.GreaterThanOrEqualTo(result.MinAllySpacingMeters));
                Assert.That(result.Participants, Has.Count.EqualTo(8));
                Assert.That(result.Participants.All(item => item.Placement > 0), Is.True);
                Assert.That(result.Participants.Select(item => item.Placement).Distinct().Count(), Is.EqualTo(8));
                Assert.That(result.Participants.All(item => item.Fighter != "Unknown"), Is.True);
                Assert.That(result.Participants.All(item => item.Team == "Raja" || item.Team == "Rival"), Is.True);
                Assert.That(result.Participants.All(item => !string.IsNullOrEmpty(item.BastionRole)), Is.True);
                Assert.That(result.AttackAttempts, Is.GreaterThan(0), $"match {i} produced no production attack commands");
                Assert.That(result.ProjectileHits, Is.GreaterThan(0), $"match {i} produced no projectile hits");
                Assert.That(result.CommandCount, Is.GreaterThan(0), $"match {i} produced no bot command digest input");
                Assert.That(result.CommandDigest, Is.Not.Null.And.Not.Empty);
                Assert.That(result.TargetSwitches + result.StuckRecoveries, Is.GreaterThanOrEqualTo(0));
            }

            Assert.That(harness.Results.Sum(item => item.UniqueDamagingPairs), Is.GreaterThan(0));
            Assert.That(harness.Results.Sum(item => item.BotToBotDamagingPairs), Is.GreaterThan(0));
            Assert.That(harness.Results.Sum(item => item.CombatEliminations), Is.GreaterThan(0));
            Assert.That(harness.Results.Sum(item => item.AcceptedAbilities), Is.GreaterThan(0));
            Assert.That(harness.Results.Sum(item => item.SuccessfulGadgetUses), Is.GreaterThan(0));

            if (ResolveReleaseGateMode())
            {
                AssertReleaseGates(harness.Results);
            }
            Assert.That(harness.LastReportPath, Is.Not.Null.And.Not.Empty);
            Assert.That(File.Exists(harness.LastReportPath), Is.True);

            UnityEngine.Object.Destroy(harnessObject);
        }

        private static void AssertReleaseGates(System.Collections.Generic.IReadOnlyList<AutonomousBotMatchResult> results)
        {
            Assert.That(results.Count, Is.GreaterThanOrEqualTo(100), "release mode requires at least 100 seeded matches");
            var total = results.Count;
            var eightyPercent = Mathf.CeilToInt(total * 0.80f);
            var ninetyPercent = Mathf.CeilToInt(total * 0.90f);
            var ninetyFivePercent = Mathf.CeilToInt(total * 0.95f);

            Assert.That(results.Count(item => item.CompletedWithinTickBudget && item.DurationSeconds <= MaximumReleaseDurationSeconds),
                Is.EqualTo(total), "every match must reach a valid terminal result within 360 seconds");
            Assert.That(results.Count(item => item.DurationSeconds >= MinimumReleaseDurationSeconds &&
                                              item.DurationSeconds <= MaximumReleaseDurationSeconds),
                Is.GreaterThanOrEqualTo(eightyPercent), "at least 80% of matches must last 240-360 seconds");
            Assert.That(results.Count(item => item.BotToBotDamagingPairs > 0),
                Is.GreaterThanOrEqualTo(ninetyFivePercent), "at least 95% of matches must contain bot-to-bot combat damage");
            Assert.That(results.Count(item => item.CombatEliminations >= 1),
                Is.GreaterThanOrEqualTo(ninetyPercent), "at least 90% of matches must contain combat eliminations");
            Assert.That(results.Count(item => item.CombatEliminations == 0 && item.AandhiEliminations > 0),
                Is.LessThan(Mathf.CeilToInt(total * 0.10f)), "Aandhi-only resolutions must remain below 10%");
            Assert.That(results.Sum(item => item.ProtectedWarmupDamageEvents), Is.EqualTo(0),
                "protected warm-up must not apply damage");
            Assert.That(results.Sum(item => item.InvalidPositionSamples), Is.EqualTo(0),
                "authority snapshots must never contain invalid or penetrating positions");
            Assert.That(results.Max(item => item.MaxContinuousStuckTicks),
                Is.LessThanOrEqualTo(MaximumContinuousStuckTicks), "no bot may remain continuously stuck for more than two seconds");

            var attackAttempts = results.Sum(item => item.AttackAttempts);
            var outOfRangeAttempts = results.Sum(item => item.OutOfRangeAttackAttempts);
            Assert.That(outOfRangeAttempts, Is.LessThanOrEqualTo(Mathf.Max(5, Mathf.CeilToInt(attackAttempts * MaximumOutOfRangeAttackRatio))),
                "out-of-range attack attempts exceed the documented 2% release threshold");

            var abilityAttempts = results.Sum(item => item.AbilityAttempts);
            var rejectedAbilities = results.Sum(item => item.RejectedAbilities);
            Assert.That(abilityAttempts == 0 ? 0f : rejectedAbilities / (float)abilityAttempts,
                Is.LessThan(MaximumRejectedAbilityRatio), "rejected ability attempts exceed the documented 70% release threshold");

            var fighters = results.SelectMany(item => item.Participants).Select(item => item.Fighter).Distinct().ToArray();
            CollectionAssert.IsSubsetOf(new[] { "Bijli", "Pehel", "Maya" }, fighters);
            Assert.That(results.Sum(item => item.SuccessfulUmbrellaGuardUses), Is.GreaterThan(0), "Umbrella Guard was never used successfully");
            Assert.That(results.Sum(item => item.SuccessfulDholBurstUses), Is.GreaterThan(0), "Dhol Burst was never used successfully");
            Assert.That(results.Sum(item => item.SuccessfulTiffinStationUses), Is.GreaterThan(0), "Tiffin Station was never used successfully");
        }

        private static int ResolveMatchCount()
        {
            var value = Environment.GetEnvironmentVariable("BATTLERAJA_PRODUCTION_BOT_MATCHES");
            return int.TryParse(value, out var parsed) && parsed > 0 ? parsed : 2;
        }

        private static bool ResolveReleaseGateMode()
        {
            var value = Environment.GetEnvironmentVariable("BATTLERAJA_PRODUCTION_BOT_ASSERT_RELEASE_GATES");
            return string.Equals(value, "1", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
        }

        private static float ResolvePlaybackScale()
        {
            var value = Environment.GetEnvironmentVariable("BATTLERAJA_PRODUCTION_BOT_PLAYBACK_SCALE");
            return float.TryParse(value, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var parsed) && parsed >= 1f
                ? Mathf.Clamp(parsed, 1f, 90f)
                : 50f;
        }
    }
}
