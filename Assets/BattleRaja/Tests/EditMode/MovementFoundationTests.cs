using System;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class MovementFoundationTests
    {
        private static readonly MovementTuning Tuning = new MovementTuning(
            maxSpeed: 5f,
            acceleration: 10f,
            deceleration: 20f,
            rotationSpeed: 360f,
            movementDeadZone: 0.1f,
            aimDeadZone: 0.2f,
            inputSensitivity: 1f);

        [Test]
        public void DiagonalMovementIsNormalised()
        {
            var command = MovementCommandFactory.Create(
                1,
                2,
                new MovementInputFrame(new Float2(1f, 1f), Float2.Zero),
                Tuning);

            Assert.That(command.Movement.Magnitude, Is.EqualTo(1f).Within(0.0001f));
            Assert.That(command.Movement.X, Is.EqualTo(command.Movement.Y).Within(0.0001f));
        }

        [Test]
        public void DeadZonesSuppressSmallMovementAndAim()
        {
            var command = MovementCommandFactory.Create(
                1,
                2,
                new MovementInputFrame(new Float2(0.1f, 0f), new Float2(0f, 0.2f)),
                Tuning);

            Assert.That(command.Movement, Is.EqualTo(Float2.Zero));
            Assert.That(command.Aim, Is.EqualTo(Float2.Zero));
        }

        [Test]
        public void InvalidTuningIsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MovementTuning(0f, 1f, 1f, 1f, 0.1f, 0.1f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MovementTuning(1f, 1f, 1f, 1f, 1f, 0.1f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new MovementTuning(1f, 1f, 1f, 1f, 0.1f, 0.1f, 0f));
        }

        [Test]
        public void AccelerationAndDecelerationRespectMaximumSpeed()
        {
            var motor = new MovementMotor();
            var moving = MovementCommandFactory.Create(1, 0, new MovementInputFrame(new Float2(1f, 0f), Float2.Zero), Tuning);

            var step = motor.Step(moving, 0.25f, Tuning);
            Assert.That(step.Velocity.Magnitude, Is.EqualTo(2.5f).Within(0.0001f));

            for (var i = 0; i < 10; i++)
            {
                step = motor.Step(moving, 0.25f, Tuning);
            }

            Assert.That(step.Velocity.Magnitude, Is.EqualTo(Tuning.MaxSpeed).Within(0.0001f));

            var stopped = MovementCommand.Neutral(1, 1);
            step = motor.Step(stopped, 0.1f, Tuning);
            Assert.That(step.Velocity.Magnitude, Is.EqualTo(3f).Within(0.0001f));
        }

        [Test]
        public void AimDirectionPersistsWhenAimInputStops()
        {
            var motor = new MovementMotor();
            var aimRight = MovementCommandFactory.Create(1, 0, new MovementInputFrame(Float2.Zero, new Float2(1f, 0f)), Tuning);
            motor.Step(aimRight, 0.016f, Tuning);

            var step = motor.Step(MovementCommand.Neutral(1, 1), 0.016f, Tuning);

            Assert.That(step.AimDirection, Is.EqualTo(new Float2(1f, 0f)));
        }

        [Test]
        public void EqualElapsedTimeProducesEqualDisplacementAcrossFrameSteps()
        {
            var command = MovementCommandFactory.Create(1, 0, new MovementInputFrame(new Float2(1f, 0f), Float2.Zero), Tuning);
            var oneStepMotor = new MovementMotor();
            var manyStepsMotor = new MovementMotor();
            for (var i = 0; i < 10; i++)
            {
                oneStepMotor.Step(command, 0.1f, Tuning);
                manyStepsMotor.Step(command, 0.1f, Tuning);
            }

            var oneStep = oneStepMotor.Step(command, 0.1f, Tuning);
            var manySteps = Float2.Zero;

            for (var i = 0; i < 10; i++)
            {
                manySteps += manyStepsMotor.Step(command, 0.01f, Tuning).Displacement;
            }

            Assert.That(manySteps.X, Is.EqualTo(oneStep.Displacement.X).Within(0.02f));
            Assert.That(manySteps.Y, Is.EqualTo(oneStep.Displacement.Y).Within(0.0001f));
        }
    }
}
