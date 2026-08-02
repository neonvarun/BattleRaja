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
    public sealed class CombatLabPlayModeTests
    {
        [UnitySetUp]
        public IEnumerator LoadMovementLab()
        {
            yield return SceneManager.LoadSceneAsync("MovementLab", LoadSceneMode.Single);
            yield return null;
        }

        [UnityTest]
        public IEnumerator CombatSystemsAndTrainingDummySpawn()
        {
            var lab = Object.FindAnyObjectByType<MovementLabScene>();

            Assert.That(lab, Is.Not.Null);
            Assert.That(lab.TrainingDummy, Is.Not.Null);
            Assert.That(lab.ProjectilePool, Is.Not.Null);
            Assert.That(lab.DamageResolver, Is.Not.Null);
            Assert.That(lab.ProjectilePool.CreatedCount, Is.GreaterThanOrEqualTo(1));
            yield return null;
        }

        [UnityTest]
        public IEnumerator AttackCommandSpawnsProjectileAndDamagesDummy()
        {
            var player = Object.FindAnyObjectByType<MovementPlayerAgent>();
            var attack = Object.FindAnyObjectByType<CombatAttackController>();
            var lab = Object.FindAnyObjectByType<MovementLabScene>();
            var dummy = lab.TrainingDummy.Target;
            var startHealth = dummy.Health.Snapshot.CurrentHealth;
            var origin = new Float2(player.transform.position.x, player.transform.position.z + 0.7f);
            var command = AttackCommandFactory.Create(
                new CombatEntityId(player.ActorId),
                0,
                origin,
                Float2.Up,
                true);

            attack.Submit(command);
            Assert.That(attack.ActiveProjectileCount, Is.EqualTo(1));
            yield return new WaitForSeconds(1f);

            Assert.That(dummy.Health.Snapshot.CurrentHealth, Is.LessThan(startHealth));
            Assert.That(attack.ActiveProjectileCount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator ProjectilePoolReusesObjectsAfterDespawn()
        {
            var player = Object.FindAnyObjectByType<MovementPlayerAgent>();
            var attack = Object.FindAnyObjectByType<CombatAttackController>();
            var pool = Object.FindAnyObjectByType<CombatProjectilePool>();
            var initialCreated = pool.CreatedCount;
            var origin = new Float2(player.transform.position.x, player.transform.position.z + 0.7f);
            var command = AttackCommandFactory.Create(new CombatEntityId(player.ActorId), 0, origin, new Float2(1f, 0f), true);

            attack.Submit(command);
            yield return new WaitForSeconds(2f);

            attack.ResetAttackState();
            attack.Submit(command);
            yield return null;

            Assert.That(pool.CreatedCount, Is.EqualTo(initialCreated));
        }

        [UnityTest]
        public IEnumerator InvalidLayerCollisionDespawnsProjectileWithoutDamage()
        {
            var player = Object.FindAnyObjectByType<MovementPlayerAgent>();
            var attack = Object.FindAnyObjectByType<CombatAttackController>();
            var dummy = Object.FindAnyObjectByType<TrainingDummy>();
            var startHealth = dummy.Target.Health.Snapshot.CurrentHealth;
            var origin = new Float2(player.transform.position.x, player.transform.position.z + 0.7f);
            var command = AttackCommandFactory.Create(new CombatEntityId(player.ActorId), 0, origin, new Float2(-1f, 0f), true);

            attack.Submit(command);
            yield return new WaitForSeconds(2f);

            Assert.That(dummy.Target.Health.Snapshot.CurrentHealth, Is.EqualTo(startHealth));
            Assert.That(attack.ActiveProjectileCount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator DummyDefeatAndResetAreSupported()
        {
            var dummy = Object.FindAnyObjectByType<TrainingDummy>();
            var resolver = Object.FindAnyObjectByType<CombatDamageResolver>();
            var request = new DamageRequest(new CombatEntityId(1), dummy.Target.Id, CombatFaction.Player, 1000, DamageType.Projectile);
            var result = resolver.Resolve(dummy.Target, request, false, false);

            Assert.That(result.TargetDefeated, Is.True);
            yield return new WaitForSeconds(1.2f);
            Assert.That(dummy.Target.Health.Snapshot.IsDefeated, Is.False);
        }

        [UnityTest]
        public IEnumerator FocusLossResetsAttackInput()
        {
            var adapter = Object.FindAnyObjectByType<PlayerInputAdapter>();
            Assert.That(adapter.IsAttackHeld, Is.False);
            adapter.ResetInputState();
            yield return null;
            Assert.That(adapter.IsAttackHeld, Is.False);
        }
    }
}
