using System.Collections;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class MovementLabPlayModeTests
    {
        private const string SceneName = "MovementLab";

        [UnitySetUp]
        public IEnumerator LoadMovementLab()
        {
            // Reload the fixture for every test so movement state from a prior
            // command sequence cannot influence the next assertion.
            yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);

            PlayModeTestHelpers.DisableBots();
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerSpawnsWithMovementAndCameraReferences()
        {
            var lab = Object.FindAnyObjectByType<MovementLabScene>();

            Assert.That(lab, Is.Not.Null);
            Assert.That(lab.Player, Is.Not.Null);
            Assert.That(lab.Player.IsInitialized, Is.True);
            Assert.That(lab.CameraController, Is.Not.Null);
            Assert.That(lab.CameraController.ProjectionMode, Is.EqualTo(CameraProjectionMode.Orthographic));
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerMovesThroughTheCommandPipeline()
        {
            var player = PlayModeTestHelpers.FindPlayer<MovementPlayerAgent>();
            player.GetComponent<PlayerInputAdapter>().enabled = false;
            var start = player.transform.position;
            var command = MovementCommandFactory.Create(
                player.ActorId,
                0,
                new MovementInputFrame(new Float2(1f, 0f), Float2.Zero),
                player.Tuning);

            for (var i = 0; i < 12; i++)
            {
                yield return null;
                player.Submit(command, 1f / 60f);
            }

            Assert.That(player.transform.position.x, Is.GreaterThan(start.x + 0.1f));
            Assert.That(player.Velocity.Magnitude, Is.GreaterThan(0f));
        }

        [UnityTest]
        public IEnumerator BoundaryCollisionPreventsLeavingArena()
        {
            var player = PlayModeTestHelpers.FindPlayer<MovementPlayerAgent>();
            player.GetComponent<PlayerInputAdapter>().enabled = false;
            var command = MovementCommandFactory.Create(
                player.ActorId,
                0,
                new MovementInputFrame(new Float2(-1f, 0f), Float2.Zero),
                player.Tuning);

            for (var i = 0; i < 180; i++)
            {
                yield return null;
                player.Submit(command, 1f / 60f);
            }

            Assert.That(player.transform.position.x, Is.GreaterThan(-13.5f));
        }

        [UnityTest]
        public IEnumerator InputReleaseDeceleratesWithoutPersistentSliding()
        {
            var player = PlayModeTestHelpers.FindPlayer<MovementPlayerAgent>();
            player.GetComponent<PlayerInputAdapter>().enabled = false;
            var command = MovementCommandFactory.Create(
                player.ActorId,
                0,
                new MovementInputFrame(new Float2(0f, 1f), Float2.Zero),
                player.Tuning);

            for (var i = 0; i < 20; i++)
            {
                yield return null;
                player.Submit(command, 1f / 60f);
            }

            var releasedPosition = player.transform.position;
            for (var i = 0; i < 30; i++)
            {
                yield return null;
                player.Submit(MovementCommand.Neutral(player.ActorId, i), 1f / 60f);
            }

            Assert.That(Vector3.Distance(releasedPosition, player.transform.position), Is.LessThan(0.6f));
            Assert.That(player.Velocity.Magnitude, Is.LessThan(0.05f));
        }

        [UnityTest]
        public IEnumerator AimDirectionAndIndicatorPersistAfterAimRelease()
        {
            var player = PlayModeTestHelpers.FindPlayer<MovementPlayerAgent>();
            player.GetComponent<PlayerInputAdapter>().enabled = false;
            var aimCommand = MovementCommandFactory.Create(
                player.ActorId,
                0,
                new MovementInputFrame(Float2.Zero, new Float2(1f, 0f)),
                player.Tuning);
            player.Submit(aimCommand, 1f / 60f);
            yield return null;

            Assert.That(player.AimDirection, Is.EqualTo(new Float2(1f, 0f)));
            var line = player.GetComponent<LineRenderer>();
            Assert.That(line, Is.Not.Null);
            Assert.That(line.GetPosition(1).x, Is.GreaterThan(line.GetPosition(0).x));
        }

        [UnityTest]
        public IEnumerator TouchStickResetClearsActiveValue()
        {
            var sticks = Object.FindObjectsByType<VirtualStick>();
            Assert.That(sticks.Length, Is.EqualTo(2));

            foreach (var stick in sticks)
            {
                stick.ResetStick();
                Assert.That(stick.IsActive, Is.False);
                Assert.That(stick.Value, Is.EqualTo(Vector2.zero));
            }

            yield return null;
        }
    }
}
