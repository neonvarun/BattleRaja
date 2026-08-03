using System.Collections;
using System.Linq;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class BotLabPlayModeTests
    {
        [UnitySetUp]
        public IEnumerator LoadMovementLab()
        {
            yield return SceneManager.LoadSceneAsync("MovementLab", LoadSceneMode.Single);
            yield return null;
        }

        [UnityTest]
        public IEnumerator SevenBijliBotsSpawnWithUniqueCommandActors()
        {
            var bots = Object.FindObjectsByType<BotBrain>();
            var agents = Object.FindObjectsByType<MovementPlayerAgent>()
                .Where(agent => agent.ActorId >= 10)
                .ToArray();

            Assert.That(bots.Length, Is.EqualTo(7));
            Assert.That(agents.Length, Is.EqualTo(7));
            Assert.That(agents.Select(agent => agent.ActorId).Distinct().Count(), Is.EqualTo(7));
            yield return null;
        }

        [UnityTest]
        public IEnumerator BotsPerceiveAndIssueImperfectDecisions()
        {
            var bots = Object.FindObjectsByType<BotBrain>();
            yield return new WaitForSeconds(1.4f);

            Assert.That(bots.All(bot => bot.DecisionCount > 0), Is.True);
            Assert.That(bots.Any(bot => bot.CurrentDecision.TargetId.Value != 0), Is.True);
            Assert.That(bots.All(bot => bot.MaxDecisionMilliseconds < 50d), Is.True);
        }

        [UnityTest]
        public IEnumerator SevenBotStressScenarioMaintainsProgressAndBoundedDecisionCost()
        {
            var bots = Object.FindObjectsByType<BotBrain>();
            var start = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(2f);
            var elapsed = Time.realtimeSinceStartup - start;

            Assert.That(elapsed, Is.GreaterThan(1.5f));
            Assert.That(bots.Sum(bot => bot.DecisionCount), Is.GreaterThan(7));
            Assert.That(bots.Max(bot => bot.MaxDecisionMilliseconds), Is.LessThan(50d));
            Debug.Log($"M4 seven-bot stress: elapsed={elapsed:0.000}s decisions={bots.Sum(bot => bot.DecisionCount)} maxDecisionMs={bots.Max(bot => bot.MaxDecisionMilliseconds):0.000}");
        }
    }
}
