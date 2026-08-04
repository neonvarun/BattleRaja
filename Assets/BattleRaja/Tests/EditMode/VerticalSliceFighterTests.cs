using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class VerticalSliceFighterTests
    {
        [Test]
        public void ThreeFightersHaveStableDistinctKits()
        {
            var bijli = FighterDefinition.Bijli;
            var pehel = FighterDefinition.Pehel;
            var maya = FighterDefinition.Maya;
            Assert.That(bijli.IsValid(out _), Is.True);
            Assert.That(pehel.IsValid(out _), Is.True);
            Assert.That(maya.IsValid(out _), Is.True);
            Assert.That(pehel.FighterId.Equals(maya.FighterId), Is.False);
            Assert.That(pehel.MaxHealth, Is.GreaterThan(bijli.MaxHealth));
            Assert.That(maya.BasicAttack.Damage, Is.LessThan(pehel.BasicAttack.Damage));
            Assert.That(FighterSpecialDefinition.PehelChargeThrow.IsValid(out _), Is.True);
            Assert.That(FighterSpecialDefinition.MayaDecoy.IsValid(out _), Is.True);
        }

        [Test]
        public void MayaDecoyCopiesMovementAndExpires()
        {
            var decoy = new DecoyRuntime();
            Assert.That(decoy.TrySpawn(Float2.Zero, FighterSpecialDefinition.MayaDecoy), Is.True);
            decoy.Advance(1f, new Float2(4f, 0f));
            Assert.That(decoy.IsActive, Is.True);
            Assert.That(decoy.Position.X, Is.GreaterThan(0f));
            decoy.Advance(10f, Float2.Zero);
            Assert.That(decoy.IsActive, Is.False);
        }

        [Test]
        public void MayaDecoyIsTargetableAndCanBeDestroyedWithoutOwnerHealthBypass()
        {
            var decoy = new DecoyRuntime();
            var definition = FighterSpecialDefinition.MayaDecoy;
            Assert.That(decoy.TrySpawn(new CombatEntityId(7), Float2.Zero, definition), Is.True);
            Assert.That(decoy.OwnerId.Value, Is.EqualTo(7));
            Assert.That(decoy.IsTargetable, Is.True);
            Assert.That(decoy.TryDamage(definition.Magnitude - 1), Is.True);
            Assert.That(decoy.IsTargetable, Is.True);
            Assert.That(decoy.TryDamage(1), Is.True);
            Assert.That(decoy.IsTargetable, Is.False);
        }

        [Test]
        public void PehelChargeThrowHasBoundedRadiusAndCooldown()
        {
            var kit = FighterSpecialDefinition.PehelChargeThrow;
            Assert.That(kit.Radius, Is.LessThanOrEqualTo(2.5f));
            Assert.That(kit.CooldownSeconds, Is.GreaterThan(4f));
            Assert.That(kit.Magnitude, Is.LessThanOrEqualTo(4));
        }

        [Test]
        public void PehelChargeThrowCapturesEnemyOnceAndEmitsControlledThrow()
        {
            var runtime = new ChargeThrowRuntime(FighterSpecialDefinition.PehelChargeThrow);
            var command = AbilityCommandFactory.Create(
                new CombatEntityId(10),
                1,
                FighterSpecialDefinition.PehelChargeThrow.AbilityId,
                Float2.Up,
                true);

            Assert.That(runtime.TryStart(command, Float2.Zero, Float2.Up), Is.True);
            runtime.Step(0.2f, 4f);
            Assert.That(runtime.State, Is.EqualTo(ChargeThrowState.Active));
            Assert.That(runtime.TryCaptureTarget(new CombatEntityId(11), CombatFaction.Enemy, CombatFaction.Player, 1.5f), Is.True);
            Assert.That(runtime.TryCaptureTarget(new CombatEntityId(12), CombatFaction.Enemy, CombatFaction.Player, 1.5f), Is.False);

            var step = runtime.Step(0.2f, 4f);
            Assert.That(step.ThrowTriggered, Is.True);
            Assert.That(step.CapturedTargetId.Value, Is.EqualTo(11));
            Assert.That(runtime.TryStart(command, Float2.Zero, Float2.Up), Is.False);
        }
    }
}
