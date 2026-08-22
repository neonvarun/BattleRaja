using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class BijliFoundationTests
    {
        [Test]
        public void BijliDefinitionIsValidAndUsesStableUniqueIds()
        {
            var definition = FighterDefinition.Bijli;

            Assert.That(definition.IsValid(out var reason), Is.True, reason);
            Assert.That(definition.FighterId.Value, Is.EqualTo("fighter.bijli"));
            Assert.That(definition.BasicAttack.Damage, Is.EqualTo(12));
            Assert.That(definition.Ability.AbilityId.Value, Is.EqualTo("ability.bijli.electric_dash"));
            Assert.That(definition.FighterId.Equals(ContentId.Attack("fighter.bijli")), Is.False);
        }

        [Test]
        public void DashFallsBackFromAimToMovementThenFacing()
        {
            var runtime = new FighterRuntimeState(FighterDefinition.Bijli);
            var command = AbilityCommandFactory.Create(
                new CombatEntityId(1),
                0,
                FighterDefinition.Bijli.Ability.AbilityId,
                Float2.Zero,
                true);

            Assert.That(runtime.TryStartDash(command, new Float2(1f, 0f), Float2.Up), Is.True);
            Assert.That(runtime.DashDirection, Is.EqualTo(new Float2(1f, 0f)));
            runtime.Reset();
            var facingCommand = AbilityCommandFactory.Create(new CombatEntityId(1), 0, FighterDefinition.Bijli.Ability.AbilityId, Float2.Zero, true);
            Assert.That(runtime.TryStartDash(facingCommand, Float2.Zero, new Float2(-1f, 0f)), Is.True);
            Assert.That(runtime.DashDirection, Is.EqualTo(new Float2(-1f, 0f)));
        }

        [Test]
        public void DashTravelsConfiguredDistanceAcrossFrameSteps()
        {
            var runtime = new FighterRuntimeState(FighterDefinition.Bijli);
            var command = AbilityCommandFactory.Create(new CombatEntityId(1), 0, FighterDefinition.Bijli.Ability.AbilityId, Float2.Up, true);
            Assert.That(runtime.TryStartDash(command, Float2.Zero, Float2.Up), Is.True);

            var displacement = Float2.Zero;
            for (var i = 0; i < 20; i++)
            {
                displacement += runtime.Step(0.04f, 10f).Displacement;
            }

            Assert.That(displacement.Magnitude, Is.EqualTo(FighterDefinition.Bijli.Ability.Distance).Within(0.001f));
            Assert.That(runtime.ActionState, Is.EqualTo(FighterActionState.Cooldown));
        }

        [Test]
        public void DashStopsAtCollisionAndCannotStartConcurrently()
        {
            var runtime = new FighterRuntimeState(FighterDefinition.Bijli);
            var command = AbilityCommandFactory.Create(new CombatEntityId(1), 0, FighterDefinition.Bijli.Ability.AbilityId, Float2.Up, true);
            Assert.That(runtime.TryStartDash(command, Float2.Zero, Float2.Up), Is.True);
            Assert.That(runtime.TryStartDash(command, Float2.Zero, Float2.Up), Is.False);

            runtime.Step(0.08f, 10f);
            var blocked = runtime.Step(0.16f, 1f);
            Assert.That(blocked.Blocked, Is.True);
            Assert.That(runtime.DistanceTravelled, Is.EqualTo(1f).Within(0.001f));
            Assert.That(runtime.ActionState, Is.EqualTo(FighterActionState.Cooldown));
        }

        [Test]
        public void DashCooldownPreventsRetriggerUntilCooldownExpires()
        {
            var runtime = new FighterRuntimeState(FighterDefinition.Bijli);
            var command = AbilityCommandFactory.Create(new CombatEntityId(1), 0, FighterDefinition.Bijli.Ability.AbilityId, Float2.Up, true);
            Assert.That(runtime.TryStartDash(command, Float2.Zero, Float2.Up), Is.True);
            for (var i = 0; i < 20; i++) runtime.Step(0.04f, 10f);
            Assert.That(runtime.TryStartDash(command, Float2.Zero, Float2.Up), Is.False);
            runtime.Step(FighterDefinition.Bijli.Ability.CooldownSeconds, 10f);
            Assert.That(runtime.TryStartDash(command, Float2.Zero, Float2.Up), Is.True);
        }
    }
}
