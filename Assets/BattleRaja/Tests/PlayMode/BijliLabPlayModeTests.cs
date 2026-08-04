using System.Collections;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class BijliLabPlayModeTests
    {
        [UnitySetUp]
        public IEnumerator LoadMovementLab()
        {
            yield return SceneManager.LoadSceneAsync("MovementLab", LoadSceneMode.Single);
            PlayModeTestHelpers.DisableBots();
            yield return null;
        }

        [UnityTest]
        public IEnumerator BijliSpawnsWithSharedCombatAndAbilityInterfaces()
        {
            var fighter = PlayModeTestHelpers.FindPlayer<BijliFighterController>();
            var attack = PlayModeTestHelpers.FindPlayer<CombatAttackController>();
            var health = PlayModeTestHelpers.FindPlayer<CombatHealth>();
            var hud = Object.FindAnyObjectByType<BijliHud>();

            Assert.That(fighter, Is.Not.Null);
            Assert.That(fighter.Definition.FighterId.Value, Is.EqualTo("fighter.bijli"));
            Assert.That(attack, Is.Not.Null);
            Assert.That(health, Is.Not.Null);
            Assert.That(hud, Is.Not.Null);
            yield return null;
        }

        [UnityTest]
        public IEnumerator AbilityCommandMovesBijliAndEntersCooldown()
        {
            var fighter = PlayModeTestHelpers.FindPlayer<BijliFighterController>();
            var movement = PlayModeTestHelpers.FindPlayer<MovementPlayerAgent>();
            var characterController = movement.GetComponent<CharacterController>();
            characterController.enabled = false;
            movement.transform.position = new Vector3(0f, 1f, 0f);
            characterController.enabled = true;
            Physics.SyncTransforms();
            var start = movement.transform.position;
            var command = AbilityCommandFactory.Create(
                new CombatEntityId(movement.ActorId),
                0,
                fighter.Definition.Ability.AbilityId,
                Float2.Up,
                true);

            fighter.Submit(command);
            yield return null;
            Assert.That(fighter.IsMovementLocked, Is.True);
            yield return new WaitForSeconds(0.7f);

            Assert.That(Vector3.Distance(start, movement.transform.position), Is.GreaterThan(0.5f));
            Assert.That(fighter.DashCooldownRemaining, Is.GreaterThan(0f));
        }

        [UnityTest]
        public IEnumerator HudPublishesHealthAndCooldownState()
        {
            var hudText = Object.FindAnyObjectByType<BijliHud>().GetComponentInChildren<UnityEngine.UI.Text>();
            yield return null;
            Assert.That(hudText, Is.Not.Null);
            Assert.That(hudText.text, Does.Contain("BIJLI"));
            Assert.That(hudText.text, Does.Contain("HP"));
            Assert.That(hudText.text, Does.Contain("DASH"));
        }
    }
}
