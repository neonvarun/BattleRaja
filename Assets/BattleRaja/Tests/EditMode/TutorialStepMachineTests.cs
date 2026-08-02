using BattleRaja.Core.Application;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class TutorialStepMachineTests
    {
        [Test]
        public void StepsAreOrderedFromMovementToVictory()
        {
            var machine = new TutorialStepMachine();

            Assert.That(machine.Current, Is.EqualTo(TutorialStep.Movement));
            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.Aim));
            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.BasicAttack));
            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.Ability));
            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.Gadget));
            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.Aandhi));
            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.Elimination));
            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.Victory));
            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.Complete));
            Assert.That(machine.IsComplete, Is.True);
        }

        [Test]
        public void CompletionIsIdempotent()
        {
            var machine = new TutorialStepMachine();
            for (var i = 0; i < 12; i++) machine.Advance();

            Assert.That(machine.Advance(), Is.EqualTo(TutorialStep.Complete));
            Assert.That(machine.Current, Is.EqualTo(TutorialStep.Complete));
        }

        [Test]
        public void ReplayRestartsAtMovement()
        {
            var machine = new TutorialStepMachine();
            machine.Advance();
            machine.Advance();

            machine.Restart();

            Assert.That(machine.Current, Is.EqualTo(TutorialStep.Movement));
            Assert.That(machine.IsComplete, Is.False);
        }
    }
}
