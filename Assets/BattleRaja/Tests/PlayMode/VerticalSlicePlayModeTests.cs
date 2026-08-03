using System.Collections;
using System.Linq;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Visuals;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class VerticalSlicePlayModeTests
    {
        [UnitySetUp]
        public IEnumerator LoadBazaarBastion()
        {
            yield return SceneManager.LoadSceneAsync("BazaarBastion", LoadSceneMode.Single);
            PlayModeTestHelpers.DisableBots();
            yield return null;
        }

        [UnityTest]
        public IEnumerator SceneContainsBijliPehelAndMayaDefinitions()
        {
            var ids = Object.FindObjectsByType<BijliFighterController>(FindObjectsSortMode.None)
                .Select(controller => controller.Definition.FighterId.Value)
                .Concat(Object.FindObjectsByType<PehelFighterController>(FindObjectsSortMode.None)
                    .Select(controller => controller.Definition.FighterId.Value))
                .Concat(Object.FindObjectsByType<MayaFighterController>(FindObjectsSortMode.None)
                    .Select(controller => controller.Definition.FighterId.Value))
                .Distinct()
                .ToArray();
            Assert.That(ids, Does.Contain("fighter.bijli"));
            Assert.That(ids, Does.Contain("fighter.pehel"));
            Assert.That(ids, Does.Contain("fighter.maya"));
            Assert.That(ids, Has.Length.EqualTo(3));
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionSceneUsesFighterSpecificAbilityControllers()
        {
            Assert.That(Object.FindObjectsByType<PehelFighterController>(FindObjectsSortMode.None), Has.Length.GreaterThanOrEqualTo(1));
            Assert.That(Object.FindObjectsByType<MayaFighterController>(FindObjectsSortMode.None), Has.Length.GreaterThanOrEqualTo(1));
            Assert.That(GameObject.Find("BazaarBastion"), Is.Not.Null);
            Assert.That(GameObject.Find("BazaarArchitecture"), Is.Not.Null);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionBotsResolveTheirOwnFighterAbilityControllers()
        {
            var brains = Object.FindObjectsByType<BotBrain>(FindObjectsSortMode.None);
            Assert.That(brains, Has.Length.GreaterThanOrEqualTo(1));
            for (var i = 0; i < brains.Length; i++)
            {
                Assert.That(brains[i].AbilityController, Is.Not.Null, brains[i].name);
                var pehel = brains[i].GetComponent<PehelFighterController>();
                var maya = brains[i].GetComponent<MayaFighterController>();
                var bijli = brains[i].GetComponent<BijliFighterController>();
                var expected = pehel != null
                    ? (IFighterAbilityController)pehel
                    : maya != null ? maya : bijli;
                Assert.That(brains[i].AbilityController, Is.SameAs(expected), brains[i].name);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator PehelChargeThrowRunsThroughTheLiveController()
        {
            var pehelObject = new GameObject("PehelRuntimeProbe");
            var pehelHealth = pehelObject.AddComponent<CombatHealth>();
            var pehelTarget = pehelObject.AddComponent<CombatTarget>();
            var pehel = pehelObject.AddComponent<PehelFighterController>();
            pehelObject.transform.position = new Vector3(10f, 1f, -8f);

            var targetObject = new GameObject("PehelRuntimeTarget");
            var targetHealth = targetObject.AddComponent<CombatHealth>();
            var target = targetObject.AddComponent<CombatTarget>();
            targetObject.AddComponent<CharacterController>();
            targetObject.transform.position = pehelObject.transform.position + Vector3.right * 1.4f;

            yield return null;

            pehelTarget.Configure(9001, CombatFaction.Enemy, pehelHealth);
            target.Configure(9002, CombatFaction.Player, targetHealth);
            Physics.SyncTransforms();
            Assert.That(Physics.OverlapSphere(pehelObject.transform.position, 2.2f)
                .Any(collider => collider.GetComponentInParent<CombatTarget>() == target), Is.True);
            var chargeStartTargetPosition = targetObject.transform.position;
            var beforeHealth = targetHealth.Snapshot.CurrentHealth;
            pehel.Submit(AbilityCommandFactory.Create(
                pehelTarget.Id,
                1,
                pehel.AbilityId,
                new Float2(1f, 0f),
                true));

            yield return new WaitForSeconds(0.6f);

            Assert.That(pehel.CapturedTargetId.Value, Is.EqualTo(target.Id.Value));
            Assert.That(targetHealth.Snapshot.CurrentHealth, Is.LessThan(beforeHealth));
            Assert.That(pehel.AbilityCooldownRemaining, Is.GreaterThan(0f));
            Assert.That(targetObject.transform.position.x, Is.GreaterThan(chargeStartTargetPosition.x));

            Object.Destroy(pehelObject);
            Object.Destroy(targetObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator MayaDecoySpawnsFollowsAndCanBeDestroyedByCombat()
        {
            var mayaObject = new GameObject("MayaRuntimeProbe");
            var mayaHealth = mayaObject.AddComponent<CombatHealth>();
            var ownerTarget = mayaObject.AddComponent<CombatTarget>();
            var maya = mayaObject.AddComponent<MayaFighterController>();
            mayaObject.transform.position = new Vector3(10f, 1f, 6f);

            var attackerObject = new GameObject("MayaRuntimeAttacker");
            var attackerHealth = attackerObject.AddComponent<CombatHealth>();
            var attacker = attackerObject.AddComponent<CombatTarget>();
            yield return null;

            ownerTarget.Configure(9010, CombatFaction.Enemy, mayaHealth);
            attacker.Configure(9011, CombatFaction.Player, attackerHealth);
            var resolver = Object.FindFirstObjectByType<CombatDamageResolver>();

            maya.Submit(AbilityCommandFactory.Create(
                ownerTarget.Id,
                1,
                maya.AbilityId,
                Float2.Up,
                true));
            yield return null;

            Assert.That(maya.IsDecoyActive, Is.True);
            var decoy = GameObject.Find("MayaDecoy");
            Assert.That(decoy, Is.Not.Null);
            var decoyTarget = decoy.GetComponent<CombatTarget>();
            var decoyHealth = decoy.GetComponent<CombatHealth>();
            Assert.That(decoyTarget, Is.Not.Null);
            Assert.That(decoyHealth, Is.Not.Null);
            Assert.That(decoyTarget.Faction, Is.Not.EqualTo(attacker.Faction));

            var beforeFollow = decoy.transform.position;
            maya.transform.position += Vector3.right * 3f;
            yield return new WaitForSeconds(0.5f);
            Assert.That(decoy.transform.position.x, Is.GreaterThan(beforeFollow.x));

            var result = resolver.Resolve(
                decoyTarget,
                new DamageRequest(
                    attacker.Id,
                    decoyTarget.Id,
                    attacker.Faction,
                    decoyHealth.Snapshot.CurrentHealth,
                    DamageType.Projectile,
                    new Float2(1f, 0f),
                    1),
                allowSelfHit: false,
                allowFriendlyFire: false,
                simulationTick: 1);
            Assert.That(result.Applied, Is.True);
            yield return null;
            Assert.That(maya.IsDecoyActive, Is.False);
            Assert.That(GameObject.Find("MayaDecoy"), Is.Null);

            Object.Destroy(mayaObject);
            Object.Destroy(attackerObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionSceneHasReadableFighterAndAudioPresentation()
        {
            Assert.That(Object.FindObjectsByType<FighterPresentation>(FindObjectsSortMode.None), Has.Length.GreaterThanOrEqualTo(8));
            Assert.That(Object.FindObjectsByType<BattleRajaAudioDirector>(FindObjectsSortMode.None), Has.Length.EqualTo(1));
            var visual = Object.FindFirstObjectByType<FighterPresentation>();
            Assert.That(visual.CurrentAnimation, Is.EqualTo(FighterPresentation.AnimationState.Idle).Or.EqualTo(FighterPresentation.AnimationState.Locomotion));
            yield return null;
        }

        [UnityTest]
        public IEnumerator ExistingOfflineMatchAndGadgetSystemsRemainPresent()
        {
            Assert.That(Object.FindFirstObjectByType<OfflineMatchController>(), Is.Not.Null);
            Assert.That(Object.FindObjectsByType<GadgetUser>(FindObjectsSortMode.None), Has.Length.EqualTo(8));
            yield return null;
        }

        [Test]
        public void NarrowViewportsExpandOrthographicFramingWithoutChangingLandscapeSize()
        {
            var landscape = TopDownCameraController.CalculateResponsiveOrthographicSize(9.5f, 16f / 9f, 16f / 9f);
            var portrait = TopDownCameraController.CalculateResponsiveOrthographicSize(9.5f, 390f / 600f, 16f / 9f);

            Assert.That(landscape, Is.EqualTo(9.5f).Within(0.0001f));
            Assert.That(portrait, Is.GreaterThan(landscape));
        }

        [Test]
        public void CompactMatchStatusKeepsZoneTelemetryReadable()
        {
            var status = OfflineMatchHud.FormatMatchStatus(
                BattleRaja.Core.Domain.MatchPhase.SpawnProtection,
                8,
                14f,
                8f,
                BattleRaja.Core.Domain.AandhiState.Warning,
                2.5f,
                compact: true);

            Assert.That(status, Does.Contain("\n"));
            Assert.That(status, Does.Contain("Z 14.0 > 8.0"));
            Assert.That(status, Does.Contain("WARN 2.5s"));
        }

        [Test]
        public void ResultsFormatterListsPlacementsAndCombatStats()
        {
            var results = new[]
            {
                new MatchParticipantSnapshot(new CombatEntityId(2), Float2.Zero, 0, 100, false, 2, 1, 40, 2, 12f),
                new MatchParticipantSnapshot(new CombatEntityId(1), Float2.Zero, 100, 100, true, 1, 3, 120, 1, 25f)
            };

            var text = OfflineMatchHud.FormatResults(results, compact: false);

            Assert.That(text, Does.Contain("WINNER 1"));
            Assert.That(text, Does.Contain("#1 PLAYER 1  KOs 3  AST 1  DMG 120  SURV 25.0s"));
            Assert.That(text, Does.Contain("#2 PLAYER 2  KOs 1  AST 2  DMG 40  SURV 12.0s"));
        }
    }
}
