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
        public void SquadFocusNominationOnlyBiasesAVisibleTarget()
        {
            var targets = new[]
            {
                new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(2f, 0f), 100, true),
                new BotObservedTarget(new CombatEntityId(3), CombatFaction.Player, new Float2(1f, 0f), 100, true)
            };
            var snapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                targets,
                -1,
                BotZoneObservation.Unbounded,
                CombatFaction.Enemy,
                ProjectileWeaponDefinition.BijliElectricBolt);

            var focused = new BotDecisionEngine().Decide(
                snapshot,
                0,
                BotDifficultyProfile.FairDefault,
                new SeededRandom(101),
                false,
                new CombatEntityId(2));
            Assert.That(focused.TargetId.Value, Is.EqualTo(2));

            var hiddenTargets = new[]
            {
                new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(2f, 0f), 100, false),
                new BotObservedTarget(new CombatEntityId(3), CombatFaction.Player, new Float2(1f, 0f), 100, true)
            };
            var hiddenSnapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                hiddenTargets,
                -1,
                BotZoneObservation.Unbounded,
                CombatFaction.Enemy,
                ProjectileWeaponDefinition.BijliElectricBolt);
            var fallback = new BotDecisionEngine().Decide(
                hiddenSnapshot,
                0,
                BotDifficultyProfile.FairDefault,
                new SeededRandom(101),
                false,
                new CombatEntityId(2));
            Assert.That(fallback.TargetId.Value, Is.EqualTo(3));
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
        public void NavigationRecoveryUsesFixedSimulationSteps()
        {
            var recovery = new BotNavigationRecovery();
            for (var i = 0; i < 20; i++)
            {
                recovery.Observe(Float2.Zero, Float2.Up, 1f / 30f, 0.7f);
            }

            Assert.That(recovery.IsStuck, Is.False);
            recovery.Observe(Float2.Zero, Float2.Up, 1f / 30f, 0.7f);
            recovery.Observe(Float2.Zero, Float2.Up, 1f / 30f, 0.7f);
            Assert.That(recovery.IsStuck, Is.True);
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

        [Test]
        public void FreeForAllAuthorityRelationshipCanOverrideSharedViewFaction()
        {
            var snapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                new[]
                {
                    new BotObservedTarget(
                        new CombatEntityId(11),
                        CombatFaction.Enemy,
                        new Float2(3f, 0f),
                        100,
                        true,
                        true)
                },
                -1,
                BotZoneObservation.Unbounded,
                CombatFaction.Enemy,
                ProjectileWeaponDefinition.BijliElectricBolt);

            var decision = new BotDecisionEngine().Decide(
                snapshot,
                0,
                BotDifficultyProfile.FairDefault,
                new SeededRandom(17),
                false);

            Assert.That(decision.TargetId.Value, Is.EqualTo(11));
        }

        [Test]
        public void FighterWeaponRangeShapesEffectivePositioning()
        {
            var targets = new[]
            {
                new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(3.5f, 0f), 100, true)
            };
            var snapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                targets,
                -1,
                BotZoneObservation.Unbounded,
                CombatFaction.Enemy,
                ProjectileWeaponDefinition.PehelHeavyBolt);

            var decision = new BotDecisionEngine().Decide(
                snapshot,
                0,
                BotDifficultyProfile.FairDefault,
                new SeededRandom(13),
                false);

            Assert.That(decision.Attack, Is.True);
            Assert.That(decision.State, Is.EqualTo(BotDecisionState.Engage));
        }

        [Test]
        public void TargetHysteresisKeepsAValidTargetAcrossSmallScoreChanges()
        {
            var firstSnapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                new[]
                {
                    new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(4f, 0f), 100, true),
                    new BotObservedTarget(new CombatEntityId(3), CombatFaction.Player, new Float2(4.1f, 0f), 100, true)
                });
            var engine = new BotDecisionEngine();
            var random = new SeededRandom(21);
            var first = engine.Decide(firstSnapshot, 0, BotDifficultyProfile.FairDefault, random, false);

            var nextSnapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                new[]
                {
                    new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(4f, 0f), 100, true),
                    new BotObservedTarget(new CombatEntityId(3), CombatFaction.Player, new Float2(3.9f, 0f), 100, true)
                });
            var next = engine.Decide(nextSnapshot, 8, BotDifficultyProfile.FairDefault, random, false);

            Assert.That(first.TargetId.Value, Is.EqualTo(2));
            Assert.That(next.TargetId.Value, Is.EqualTo(2));
        }

        [Test]
        public void RecentAttackerReceivesTargetingPriority()
        {
            var snapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                new[]
                {
                    new BotObservedTarget(new CombatEntityId(2), CombatFaction.Player, new Float2(3f, 0f), 100, true),
                    new BotObservedTarget(new CombatEntityId(3), CombatFaction.Player, new Float2(1.5f, 0f), 100, true)
                },
                -1,
                BotZoneObservation.Unbounded,
                CombatFaction.Enemy,
                ProjectileWeaponDefinition.BijliElectricBolt,
                new CombatEntityId(2));

            var decision = new BotDecisionEngine().Decide(
                snapshot,
                0,
                BotDifficultyProfile.FairDefault,
                new SeededRandom(5),
                false);

            Assert.That(decision.TargetId.Value, Is.EqualTo(2));
        }

        [Test]
        public void BotsSeekAnAvailableGadgetWhenNoHostileIsVisible()
        {
            var snapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                new Float2(-2f, 0f),
                100,
                100,
                new BotObservedTarget[0],
                0,
                BotZoneObservation.Unbounded,
                CombatFaction.Enemy,
                ProjectileWeaponDefinition.BijliElectricBolt,
                default,
                new Float2(2f, 0f),
                true,
                false);

            var decision = new BotDecisionEngine().Decide(
                snapshot,
                0,
                BotDifficultyProfile.FairDefault,
                new SeededRandom(7),
                false);

            Assert.That(decision.State, Is.EqualTo(BotDecisionState.Loot));
            Assert.That(decision.Movement.X, Is.GreaterThan(0f));
            Assert.That(decision.UseGadget, Is.True);
        }

        [Test]
        public void BotsPrioritizeAVisibleHostileOverNearbyLoot()
        {
            var snapshot = new BotPerceptionSnapshot(
                new CombatEntityId(10),
                Float2.Zero,
                100,
                100,
                new[]
                {
                    new BotObservedTarget(
                        new CombatEntityId(2),
                        CombatFaction.Player,
                        new Float2(4f, 0f),
                        100,
                        true)
                },
                1,
                BotZoneObservation.Unbounded,
                CombatFaction.Enemy,
                ProjectileWeaponDefinition.BijliElectricBolt,
                default,
                new Float2(1f, 0f),
                true,
                false);

            var decision = new BotDecisionEngine().Decide(
                snapshot,
                0,
                BotDifficultyProfile.FairDefault,
                new SeededRandom(17),
                false);

            Assert.That(decision.TargetId.Value, Is.EqualTo(2));
            Assert.That(decision.State, Is.Not.EqualTo(BotDecisionState.Loot));
        }
    }
}
