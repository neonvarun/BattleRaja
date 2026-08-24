using BattleRaja.Core.Application;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class TutorialStepMachineTests
    {
        [Test]
        public void StepsRequireTheMatchingActionBeforeAdvancing()
        {
            var machine = new TutorialStepMachine();

            Assert.That(machine.Current, Is.EqualTo(TutorialStep.Movement));
            Assert.That(machine.TryAdvance(), Is.False);
            Assert.That(machine.ObserveAction(TutorialAction.Movement), Is.True);
            Assert.That(machine.TryAdvance(), Is.True);
            Assert.That(machine.Current, Is.EqualTo(TutorialStep.Aim));
            Assert.That(machine.ObserveAction(TutorialAction.Aim), Is.True);
            machine.TryAdvance();
            machine.ObserveAction(TutorialAction.BasicAttack);
            machine.TryAdvance();
            machine.ObserveAction(TutorialAction.Ability);
            machine.TryAdvance();
            Assert.That(machine.ObserveAction(TutorialAction.GadgetUsed), Is.False);
            Assert.That(machine.ObserveAction(TutorialAction.GadgetCollected), Is.True);
            Assert.That(machine.CurrentStepSatisfied, Is.False);
            Assert.That(machine.ObserveAction(TutorialAction.GadgetUsed), Is.True);
            machine.TryAdvance();
            machine.ObserveAction(TutorialAction.AandhiObserved);
            machine.TryAdvance();
            machine.ObserveAction(TutorialAction.Elimination);
            machine.TryAdvance();
            machine.ObserveAction(TutorialAction.Victory);
            machine.TryAdvance();
            Assert.That(machine.IsComplete, Is.True);
        }

        [Test]
        public void CompletionIsIdempotent()
        {
            var machine = new TutorialStepMachine();
            machine.SkipToComplete();

            Assert.That(machine.TryAdvance(), Is.False);
            Assert.That(machine.Current, Is.EqualTo(TutorialStep.Complete));
        }

        [Test]
        public void ReplayRestartsAtMovement()
        {
            var machine = new TutorialStepMachine();
            machine.ObserveAction(TutorialAction.Movement);
            machine.TryAdvance();
            machine.ObserveAction(TutorialAction.Aim);
            machine.TryAdvance();

            machine.Restart();

            Assert.That(machine.Current, Is.EqualTo(TutorialStep.Movement));
            Assert.That(machine.IsComplete, Is.False);
        }
    }
}
