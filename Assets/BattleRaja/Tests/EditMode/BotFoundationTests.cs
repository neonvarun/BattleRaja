using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class BotFoundationTests
    {
        [Test]
        public void FairBotProfileIsValid()
        {
            Assert.That(BotDifficultyProfile.FairDefault.IsValid(out var reason), Is.True, reason);
        }

        [Test]
        public void TargetSelectionRequiresLineOfSightAndIsDeterministic()
        {
            var targets = new[]
            {
                new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(2f, 0f), 100, true),
                new BotObservedTarget(new CombatEntityId(3), CombatFaction.Player, new Float2(1f, 0f), 100, false)
            };
            var snapshot = new BotPerceptionSnapshot(new CombatEntityId(10), Float2.Zero, 100, 100, targets);
            var first = new BotDecisionEngine().Decide(snapshot, 0, BotDifficultyProfile.FairDefault, new SeededRandom(77), false);
            var second = new BotDecisionEngine().Decide(snapshot, 0, BotDifficultyProfile.FairDefault, new SeededRandom(77), false);

            Assert.That(first.TargetId.Value, Is.EqualTo(2));
            Assert.That(first.State, Is.EqualTo(second.State));
            Assert.That(first.Aim, Is.EqualTo(second.Aim));
        }

        [Test]
        public void RetreatUsesHealthThresholdAndMovesAway()
        {
            var target = new[]
            {
                new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(3f, 0f), 100, true)
            };
            var snapshot = new BotPerceptionSnapshot(new CombatEntityId(10), Float2.Zero, 10, 100, target);
            var decision = new BotDecisionEngine().Decide(snapshot, 0, BotDifficultyProfile.FairDefault, new SeededRandom(1), false);

            Assert.That(decision.State, Is.EqualTo(BotDecisionState.Retreat));
            Assert.That(decision.Movement.X, Is.LessThan(0f));
            Assert.That(decision.Attack, Is.False);
        }

        [Test]
        public void ZoneAwarenessRepositionsInsideCurrentButOutsideNextZone()
        {
            var target = new[]
            {
                new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(0f, 4f), 100, true)
            };
            var zone = new BotZoneObservation(Float2.Zero, 4f, Float2.Zero, 3f);
            var snapshot = new BotPerceptionSnapshot(new CombatEntityId(10), new Float2(3.5f, 0f), 100, 100, target, -1, zone);
            var decision = new BotDecisionEngine().Decide(snapshot, 0, BotDifficultyProfile.FairDefault, new SeededRandom(11), false);

            Assert.That(decision.State, Is.EqualTo(BotDecisionState.Reposition));
            Assert.That(decision.TargetId.Value, Is.EqualTo(0));
            Assert.That(decision.Movement.X, Is.LessThan(0f));
            Assert.That(decision.Attack, Is.False);
        }

        [Test]
        public void AimNoiseStaysBoundedAndReactionDelayIsHonoured()
        {
            var target = new[]
            {
                new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(5f, 0f), 100, true)
            };
            var snapshot = new BotPerceptionSnapshot(new CombatEntityId(10), Float2.Zero, 100, 100, target);
            var profile = new BotDifficultyProfile(
                4, 0.2f, 0.2f, 5f, 0.1f, 0.7f, ProjectileWeaponDefinition.BijliElectricBolt);
            var engine = new BotDecisionEngine();
            var random = new SeededRandom(42);
            var first = engine.Decide(snapshot, 0, profile, random, false);
            var delayed = engine.Decide(snapshot, 1, profile, random, false);

            Assert.That(first.Aim.Magnitude, Is.EqualTo(1f).Within(0.001f));
            Assert.That(delayed.Aim, Is.EqualTo(first.Aim));
        }

        [Test]
        public void NavigationRecoveryTriggersAfterRepeatedBlockedSteps()
        {
            var recovery = new BotNavigationRecovery();
            for (var i = 0; i < 10; i++)
            {
                recovery.Observe(Float2.Zero, Float2.Up, 0.1f, 0.7f);
            }

            Assert.That(recovery.IsStuck, Is.True);
            recovery.Clear();
            Assert.That(recovery.IsStuck, Is.False);
        }

        [Test]
        public void FighterRuntimeDefinitionsRemainIndependentPerBot()
        {
            var first = new FighterRuntimeState(FighterDefinition.Bijli);
            var second = new FighterRuntimeState(FighterDefinition.Bijli);
            var command = AbilityCommandFactory.Create(new CombatEntityId(10), 0, FighterDefinition.Bijli.Ability.AbilityId, Float2.Up, true);

            Assert.That(first.TryStartDash(command, Float2.Zero, Float2.Up), Is.True);
            Assert.That(second.ActionState, Is.EqualTo(FighterActionState.Ready));
        }

        [Test]
        public void FreeForAllBotsIgnoreSameFactionAndRespectWeaponRange()
        {
            var sameFaction = new[]
            {
                new BotObservedTarget(new CombatEntityId(2), CombatFaction.Enemy, new Float2(1f, 0f), 100, true),
                new BotObservedTarget(new CombatEntityId(3), CombatFaction.Player, new Float2(20f, 0f), 100, true)
            };
            var snapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                sameFaction,
                -1,
                BotZoneObservation.Unbounded,
                CombatFaction.Enemy,
                ProjectileWeaponDefinition.PehelHeavyBolt);
            var decision = new BotDecisionEngine().Decide(
                snapshot,
                0,
                BotDifficultyProfile.FairDefault,
                new SeededRandom(9),
                false);

            Assert.That(decision.TargetId.Value, Is.EqualTo(3));
            Assert.That(decision.Attack, Is.False);
        }
    }
}
