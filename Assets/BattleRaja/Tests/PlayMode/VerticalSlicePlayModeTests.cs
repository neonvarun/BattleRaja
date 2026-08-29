using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Application;
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
using UnityEngine.UI;
using UnityEngine.Audio;

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
            var ids = Object.FindObjectsByType<BijliFighterController>()
                .Select(controller => controller.Definition.FighterId.Value)
                .Concat(Object.FindObjectsByType<PehelFighterController>()
                    .Select(controller => controller.Definition.FighterId.Value))
                .Concat(Object.FindObjectsByType<MayaFighterController>()
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
            Assert.That(Object.FindObjectsByType<PehelFighterController>(), Has.Length.GreaterThanOrEqualTo(1));
            Assert.That(Object.FindObjectsByType<MayaFighterController>(), Has.Length.GreaterThanOrEqualTo(1));
            Assert.That(GameObject.Find("BazaarBastion"), Is.Not.Null);
            Assert.That(GameObject.Find("BazaarArchitecture"), Is.Not.Null);
            var production = Object.FindAnyObjectByType<BazaarBastionScene>();
            Assert.That(production, Is.Not.Null);
            Assert.That(Object.FindAnyObjectByType<MovementLabScene>(), Is.Null,
                "The production scene must not retain the MovementLab scene contract.");
            Assert.That(production.Player, Is.Not.Null);
            Assert.That(production.MatchController, Is.Not.Null);
            Assert.That(production.DamageResolver, Is.Not.Null);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionBotsResolveTheirOwnFighterAbilityControllers()
        {
            var brains = Object.FindObjectsByType<BotBrain>();
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
        public IEnumerator ProductionBotsRespectSpawnProtectionBeforeCombat()
        {
            var player = PlayModeTestHelpers.FindPlayer<CombatHealth>();
            var brains = Object.FindObjectsByType<BotBrain>();
            Assert.That(brains, Has.Length.EqualTo(7));
            for (var i = 0; i < brains.Length; i++) brains[i].enabled = true;

            var initialHealth = player.Snapshot.CurrentHealth;
            yield return new WaitForSeconds(4f);

            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            Assert.That(match.CurrentPhase, Is.EqualTo(MatchPhase.SpawnProtection));
            Assert.That(player.Snapshot.CurrentHealth, Is.EqualTo(initialHealth),
                "Bots must not deal combat damage during load warmup or spawn protection.");
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
            var botObject = new GameObject("MayaRuntimeObserver");
            var botHealth = botObject.AddComponent<CombatHealth>();
            var botTarget = botObject.AddComponent<CombatTarget>();
            var botSensor = botObject.AddComponent<BotPerceptionSensor>();
            yield return null;

            ownerTarget.Configure(9010, CombatFaction.Enemy, mayaHealth);
            attacker.Configure(9011, CombatFaction.Player, attackerHealth);
            botTarget.Configure(9012, CombatFaction.Player, botHealth);
            var resolver = Object.FindAnyObjectByType<CombatDamageResolver>();

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
            var observed = botSensor.Capture();
            Assert.That(observed.Targets.Take(observed.TargetCount).Any(target => target.Id == decoyTarget.Id), Is.True,
                "A bot perception sensor must observe a decoy spawned after its Awake phase.");

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
            Object.Destroy(botObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionSceneHasReadableFighterAndAudioPresentation()
        {
            Assert.That(Object.FindObjectsByType<FighterPresentation>(), Has.Length.GreaterThanOrEqualTo(8));
            Assert.That(Object.FindObjectsByType<BattleRajaAudioDirector>(), Has.Length.EqualTo(1));
            var visual = Object.FindAnyObjectByType<FighterPresentation>();
            Assert.That(visual.CurrentAnimation, Is.EqualTo(FighterPresentation.AnimationState.Idle).Or.EqualTo(FighterPresentation.AnimationState.Locomotion));
            yield return null;
        }

        [UnityTest]
        public IEnumerator ExistingOfflineMatchAndGadgetSystemsRemainPresent()
        {
            Assert.That(Object.FindAnyObjectByType<OfflineMatchController>(), Is.Not.Null);
            Assert.That(Object.FindObjectsByType<GadgetUser>(), Has.Length.EqualTo(8));
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionGadgetPickupAndUseRunsThroughAuthority()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var player = PlayModeTestHelpers.FindPlayer<MovementPlayerAgent>();
            var user = player != null ? player.GetComponent<GadgetUser>() : null;
            var dhol = Object.FindObjectsByType<GadgetPickup>()
                .First(pickup => pickup.GadgetId.Equals(GadgetDefinition.DholBurst.GadgetId));

            Assert.That(match, Is.Not.Null);
            Assert.That(player, Is.Not.Null);
            Assert.That(user, Is.Not.Null);
            Assert.That(dhol, Is.Not.Null);
            Assert.That(match.AuthorityDrivenMovement, Is.True);

            player.ExternalCommandMode = true;
            // The production route intentionally starts the player close to
            // Tiffin. Move the other fixtures away so this Dhol-specific
            // regression remains deterministic and exercises only the pickup
            // under test.
            foreach (var other in Object.FindObjectsByType<GadgetPickup>())
            {
                if (other != dhol) other.transform.position += new Vector3(100f, 0f, 100f);
            }
            dhol.transform.position = player.transform.position;
            match.StartMatch();
            PlayModeTestHelpers.AdvanceToCombatPhase(match);
            yield return new WaitForSecondsRealtime(0.25f);

            Assert.That(user.HasGadget, Is.True,
                $"feedback={user.Feedback} player={player.transform.position} pickup={dhol.transform.position} active={dhol.IsAvailable}");
            Assert.That(user.HeldGadget, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(dhol.IsAvailable, Is.False);
            Assert.That(user.UseHeld(), Is.True);
            Assert.That(user.HasGadget, Is.False);
            Assert.That(user.Feedback, Is.EqualTo("Dhol Burst"));
        }

        [UnityTest]
        public IEnumerator ProductionTiffinPickupIsReachableFromPlayerSpawn()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var player = PlayModeTestHelpers.FindPlayer<MovementPlayerAgent>();
            var user = player != null ? player.GetComponent<GadgetUser>() : null;
            var tiffin = Object.FindObjectsByType<GadgetPickup>()
                .First(pickup => pickup.GadgetId.Equals(GadgetDefinition.TiffinStation.GadgetId));

            Assert.That(match, Is.Not.Null);
            Assert.That(player, Is.Not.Null);
            Assert.That(user, Is.Not.Null);
            Assert.That(Vector3.Distance(player.transform.position, tiffin.transform.position), Is.LessThan(3f),
                "The tutorial gadget must be reachable from the protected player spawn.");
            Assert.That(Object.FindObjectsByType<BotBrain>()
                .All(bot => Vector3.Distance(bot.transform.position, tiffin.transform.position) > 2f), Is.True,
                "No bot spawn may overlap the tutorial gadget pickup.");

            player.ExternalCommandMode = true;
            match.StartMatch();
            var previousTimeScale = Time.timeScale;
            Time.timeScale = 1f;
            for (var i = 0; i < 150 && !user.HasGadget; i++)
            {
                // Queue the intent every rendered frame. The match controller
                // rewrites it to its consumed canonical tick before authority
                // resolution, so this test does not depend on render-frame timing.
                player.Submit(new MovementCommand(1, 1, Float2.Up, Float2.Up));
                yield return new WaitForSecondsRealtime(1f / 30f);
            }

            Time.timeScale = previousTimeScale;

            Assert.That(user.HasGadget, Is.True,
                $"feedback={user.Feedback} player={player.transform.position} pickup={tiffin.transform.position} active={tiffin.IsAvailable}");
            Assert.That(user.HeldGadget, Is.EqualTo(GadgetDefinition.TiffinStation.GadgetId));
            Assert.That(tiffin.IsAvailable, Is.False);
        }

        [UnityTest]
        public IEnumerator ProductionProjectileViewsRetireThroughAuthoritySnapshots()
        {
            PlayModeTestHelpers.DisableBots();
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var pool = Object.FindAnyObjectByType<CombatProjectilePool>();
            var player = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 1);
            var attack = player.GetComponent<CombatAttackController>();
            Assert.That(match.AuthorityDrivenMovement, Is.True);

            PlayModeTestHelpers.AdvanceToCombatPhase(match);

            var origin = new Float2(player.transform.position.x, player.transform.position.z + 0.7f);
            attack.Submit(new AttackCommand(new CombatEntityId(1), match.SimulationTick, origin, Float2.Up, true));
            // Wait past one canonical 30 Hz tick so the controller has reconciled
            // the authority-owned shell into the pool.
            yield return new WaitForSecondsRealtime(0.25f);

            Assert.That(pool.AuthoritativeShellCount, Is.GreaterThanOrEqualTo(1));

            // The authority resolves the bolt against arena geometry or an actor;
            // the view shell must retire through snapshot reconciliation instead
            // of free-running past canonical despawn.
            var timeout = Time.realtimeSinceStartup + 6f;
            while (Time.realtimeSinceStartup < timeout && pool.AuthoritativeShellCount > 0)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }

            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.AuthoritativeShellCount, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator ProductionBotPerceptionSeesAdjacentEnemyThroughOpenLane()
        {
            PlayModeTestHelpers.DisableBots();
            yield return null;
            var agents = Object.FindObjectsByType<MovementPlayerAgent>()
                .OrderBy(agent => agent.ActorId).ToArray();
            Assert.That(agents.Length, Is.GreaterThanOrEqualTo(3));
            var shooter = agents[1].GetComponent<BotPerceptionSensor>();
            var enemy = agents[2];

            // The y=-8 lane crosses no authored Bazaar obstacle.
            shooter.transform.position = new Vector3(-11f, 1f, -8f);
            enemy.transform.position = new Vector3(-8f, 1f, -8f);
            Physics.SyncTransforms();

            var openSnapshot = shooter.Capture();
            var observed = openSnapshot.Targets.Take(openSnapshot.TargetCount)
                .FirstOrDefault(candidate => candidate.Id == new CombatEntityId(enemy.ActorId));
            Assert.That(observed.Id, Is.EqualTo(new CombatEntityId(enemy.ActorId)));
            Assert.That(observed.HasLineOfSight, Is.True,
                "An adjacent enemy on an open lane must be perceived; endpoint hulls must not block line of sight.");

            var blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = new Vector3(-9.5f, 1f, -8f);
            blocker.name = "LosProbeBlocker";
            Physics.SyncTransforms();
            try
            {
                var blockedSnapshot = shooter.Capture();
                var blocked = blockedSnapshot.Targets.Take(blockedSnapshot.TargetCount)
                    .FirstOrDefault(candidate => candidate.Id == new CombatEntityId(enemy.ActorId));
                Assert.That(blocked.HasLineOfSight, Is.False,
                    "Genuine cover between actors must block bot line of sight.");
            }
            finally
            {
                Object.Destroy(blocker);
            }
        }

        [UnityTest]
        public IEnumerator ProductionMatchRoutesMovementThroughAuthoritySnapshots()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var player = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 1);
            Assert.That(match.AuthorityDrivenMovement, Is.True);
            Assert.That(match.Simulation.TryGetSnapshot(new CombatEntityId(1), out var before), Is.True);
            PlayModeTestHelpers.AdvanceToCombatPhase(match);

            var previousTimeScale = Time.timeScale;
            Time.timeScale = 1f;
            try
            {
                player.Submit(new MovementCommand(1, 1, new Float2(1f, 0f), new Float2(1f, 0f)));
                yield return new WaitForSecondsRealtime(0.2f);

                Assert.That(match.Simulation.TryGetSnapshot(new CombatEntityId(1), out var after), Is.True);
                Assert.That(after.Position.X, Is.GreaterThan(before.Position.X));
                Assert.That(player.LastAuthoritativePosition.x, Is.EqualTo(after.Position.X).Within(0.001f));
                Assert.That(player.LastAuthoritativePosition.z, Is.EqualTo(after.Position.Y).Within(0.001f));
            }
            finally
            {
                Time.timeScale = previousTimeScale;
            }
        }

        [UnityTest]
        public IEnumerator ProductionBijliAbilityRoutesDisplacementThroughAuthority()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var player = Object.FindObjectsByType<MovementPlayerAgent>()
                .First(agent => agent.ActorId == 1);
            var bijli = player.GetComponent<BijliFighterController>();
            Assert.That(bijli, Is.Not.Null);
            Assert.That(player.AuthorityDrivenMovement, Is.True);

            // Combat abilities are canonical action-phase actions. Accelerate the
            // shared simulation past load warmup and spawn protection before submitting.
            var previousTimeScale = Time.timeScale;
            Time.timeScale = 50f;
            yield return new WaitForSecondsRealtime(0.18f);
            Time.timeScale = previousTimeScale;
            Assert.That(match.CurrentPhase, Is.EqualTo(MatchPhase.Opening));

            Assert.That(match.Simulation.TryGetSnapshot(new CombatEntityId(1), out var before), Is.True);

            bijli.Submit(AbilityCommandFactory.Create(
                new CombatEntityId(1),
                match.SimulationTick + 1,
                bijli.AbilityId,
                new Float2(1f, 0f),
                true));
            yield return new WaitForSecondsRealtime(0.4f);

            Assert.That(match.Simulation.TryGetSnapshot(new CombatEntityId(1), out var after), Is.True);
            Assert.That(after.Position.X, Is.GreaterThan(before.Position.X));
            Assert.That(player.LastAuthoritativePosition.x, Is.EqualTo(after.Position.X).Within(0.001f));
        }

        [UnityTest]
        public IEnumerator ProductionPehelChargeThrowUsesAuthoritySnapshots()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var pehel = Object.FindObjectsByType<PehelFighterController>()
                .Where(controller => controller.isActiveAndEnabled &&
                    controller.GetComponent<MovementPlayerAgent>()?.AuthorityDrivenMovement == true &&
                    controller.GetComponent<BotBrain>() != null)
                .OrderBy(controller => controller.GetComponent<MovementPlayerAgent>().ActorId)
                .First();
            var pehelAgent = pehel.GetComponent<MovementPlayerAgent>();
            var player = PlayModeTestHelpers.FindPlayer<CombatTarget>();
            var playerAgent = player.GetComponent<MovementPlayerAgent>();
            Assert.That(match, Is.Not.Null);
            Assert.That(pehelAgent, Is.Not.Null);
            Assert.That(player, Is.Not.Null);
            Assert.That(playerAgent, Is.Not.Null);

            var pehelPosition = new Float2(-4f, 0f);
            var playerPosition = new Float2(-2.6f, 0f);

            var previousTimeScale = Time.timeScale;
            Time.timeScale = 50f;
            yield return new WaitForSecondsRealtime(0.18f);
            Time.timeScale = previousTimeScale;
            Assert.That(match.CurrentPhase, Is.EqualTo(MatchPhase.Opening));

            match.Simulation.SetPosition(pehelAgent.ActorId > 0 ? new CombatEntityId(pehelAgent.ActorId) : default, pehelPosition);
            match.Simulation.SetPosition(player.Id, playerPosition);
            pehelAgent.ApplyAuthoritativePosition(pehelPosition);
            playerAgent.ApplyAuthoritativePosition(playerPosition);
            pehelAgent.ResetMovement(new Float2(1f, 0f));
            playerAgent.ResetMovement(Float2.Up);
            Physics.SyncTransforms();

            var beforeHealth = player.Health.Snapshot.CurrentHealth;
            pehel.Submit(AbilityCommandFactory.Create(
                new CombatEntityId(pehelAgent.ActorId),
                match.SimulationTick + 1,
                pehel.AbilityId,
                new Float2(1f, 0f),
                true));

            // Allow the canonical match tick to advance through startup, active,
            // capture and recovery even when the editor is compiling/importing in
            // the background. The test still fails if the authority never captures.
            var captureTimeout = 2f;
            while (captureTimeout > 0f &&
                (pehel.CapturedTargetId != player.Id || player.Health.Snapshot.CurrentHealth >= beforeHealth))
            {
                captureTimeout -= Time.unscaledDeltaTime;
                yield return null;
            }

            Assert.That(pehel.CapturedTargetId, Is.EqualTo(player.Id));
            Assert.That(player.Health.Snapshot.CurrentHealth, Is.LessThan(beforeHealth));
            Assert.That(pehel.AbilityCooldownRemaining, Is.GreaterThan(0f));
            Assert.That(match.Simulation.TryGetSnapshot(player.Id, out var targetSnapshot), Is.True);
            Assert.That(targetSnapshot.Position.X, Is.GreaterThan(playerPosition.X));
        }

        [UnityTest]
        public IEnumerator ProductionBotProjectileUpdatesHealthEliminationPerceptionAndSpectator()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var agents = Object.FindObjectsByType<MovementPlayerAgent>()
                .Where(agent => agent.AuthorityDrivenMovement && agent.ActorId >= 10)
                .OrderBy(agent => agent.ActorId)
                .ToArray();
            Assert.That(agents.Length, Is.GreaterThanOrEqualTo(2));

            var shooterAgent = agents[0];
            var targetAgent = agents[1];
            var shooter = shooterAgent.GetComponent<CombatTarget>();
            var target = targetAgent.GetComponent<CombatTarget>();
            var presentation = target.GetComponent<FighterPresentation>();
            var perception = shooterAgent.GetComponent<BotPerceptionSensor>();
            Assert.That(perception, Is.Not.Null);
            var perceptionBefore = perception.Capture().Targets
                .Take(Mathf.Max(0, 16))
                .Count(item => item.Id == target.Id);
            Assert.That(perceptionBefore, Is.GreaterThanOrEqualTo(1),
                "The shooter must fairly perceive the selected bot target.");

            var weapon = shooterAgent.GetComponent<CombatAttackController>().AuthorityWeaponDefinition;
            var direction = new Float2(0f, -1f);

            // Place the two production actors on an unobstructed north-south line
            // and use the explicit test reconciliation seam to make this a one-hit
            // terminal check without giving either bot hidden combat power.
            var shooterPosition = new Float2(9f, 9f);
            var targetPosition = new Float2(9f, 7.5f);
            match.Simulation.SetPosition(shooter.Id, shooterPosition);
            match.Simulation.SetPosition(target.Id, targetPosition);
            shooterAgent.ApplyAuthoritativePosition(shooterPosition);
            targetAgent.ApplyAuthoritativePosition(targetPosition);
            shooterAgent.ResetMovement(direction);
            targetAgent.ResetMovement(Float2.Up);
            Physics.SyncTransforms();

            // Advance the pure match to Opening without waiting through protection.
            for (var i = 0; i < 9; i++) match.Simulation.Advance(1f);
            // Keep this authority/reconciliation regression independent of the
            // production bot's bounded PvE weapon scale: one point guarantees a
            // single accepted projectile is terminal for the selected target.
            var targetHealthAmount = 1;
            match.Simulation.SyncHealth(target.Id, targetHealthAmount);
            target.Health.SetAuthoritativeHealth(targetHealthAmount);

            var attackTick = match.SimulationTick + 1;
            var origin = new Float2(
                shooter.transform.position.x,
                shooter.transform.position.z);
            var accepted = match.TryAcceptAttack(new AttackCommand(
                shooter.Id,
                attackTick,
                origin,
                direction,
                true,
                1));
            Assert.That(accepted.Accepted, Is.True);

            yield return new WaitForSecondsRealtime(0.35f);

            var canonical = match.Simulation.GetSnapshots().First(item => item.Id == target.Id);
            Assert.That(canonical.Alive, Is.False);
            Assert.That(target.Health.Snapshot.IsDefeated, Is.True,
                "Canonical projectile damage must mirror to visible health immediately.");
            Assert.That(presentation != null && presentation.IsEliminated, Is.True);
            Assert.That(presentation != null &&
                presentation.CurrentAnimation == FighterPresentation.AnimationState.Eliminated, Is.True);

            var observation = perception.Capture().Targets.FirstOrDefault(item => item.Id == target.Id);
            Assert.That(observation.Id.Value, Is.EqualTo(0));
        }

        [UnityTest]
        public IEnumerator ProductionMayaDecoyRoutesLifetimeAndDamageThroughAuthority()
        {
            var match = Object.FindAnyObjectByType<OfflineMatchController>();
            var maya = Object.FindObjectsByType<MayaFighterController>()
                .First(controller => controller.GetComponent<MovementPlayerAgent>() != null &&
                    controller.GetComponent<MovementPlayerAgent>().AuthorityDrivenMovement &&
                    controller.GetComponent<BotBrain>() != null);
            var agent = maya.GetComponent<MovementPlayerAgent>();
            var ownerId = new CombatEntityId(agent.ActorId);
            Assert.That(match, Is.Not.Null);
            Assert.That(maya, Is.Not.Null);
            Assert.That(match.TryGetMayaDecoySnapshot(ownerId, out _), Is.False);
            for (var i = 0; i < 9; i++) match.Simulation.Advance(1f);

            maya.Submit(AbilityCommandFactory.Create(ownerId, 1, maya.AbilityId, Float2.Up, true));
            yield return new WaitForSecondsRealtime(0.25f);

            Assert.That(match.TryGetMayaDecoySnapshot(ownerId, out var spawned), Is.True);
            Assert.That(spawned.Active, Is.True);
            var decoy = Object.FindObjectsByType<CombatTarget>()
                .First(target => target.Id == spawned.DecoyId);
            var resolver = Object.FindAnyObjectByType<CombatDamageResolver>();
            Assert.That(resolver, Is.Not.Null);
            var attackerId = new CombatEntityId(ownerId.Value == 1 ? 2 : 1);
            var result = resolver.Resolve(
                decoy,
                new DamageRequest(
                    attackerId,
                    spawned.DecoyId,
                    CombatFaction.Player,
                    spawned.MaxHealth,
                    DamageType.Projectile,
                    Float2.Up,
                    2),
                allowSelfHit: false,
                allowFriendlyFire: false,
                simulationTick: 2);

            Assert.That(result.Applied, Is.True);
            yield return null;
            Assert.That(match.TryGetMayaDecoySnapshot(ownerId, out var after), Is.True);
            Assert.That(after.Active, Is.False);
            Assert.That(maya.IsDecoyActive, Is.False);
        }

        [UnityTest]
        public IEnumerator ProductionVisualKitBuildsDistinctFighterArenaAndGadgetIdentities()
        {
            var arena = Object.FindAnyObjectByType<BazaarBastionScene>();
            Assert.That(arena, Is.Not.Null);
            Assert.That(arena.transform.Find("V1BastionVisuals"), Is.Not.Null);
            Assert.That(arena.transform.Find("V1BastionVisuals/BastionCrownOrb"), Is.Not.Null,
                "Bazaar must expose the original canopy landmark instead of a flat greybox marker");
            Assert.That(arena.transform.Find("V1BastionVisuals/BastionAwning0"), Is.Not.Null,
                "Bazaar central canopy panels must be present");
            Assert.That(arena.transform.Find("V1BastionVisuals/BastionCrownTopCross"), Is.Null,
                "The deprecated cross-shaped prototype landmark must not return");
            var ground = arena.transform.Find("V1BastionVisuals/GroundMosaic");
            Assert.That(ground, Is.Not.Null, "Bazaar must expose the render-only ground mosaic");
            Assert.That(ground.GetComponent<Collider>(), Is.Null,
                "The visual ground must never add authoritative collision");
            var groundMesh = ground.GetComponent<MeshFilter>()?.sharedMesh;
            Assert.That(groundMesh, Is.Not.Null);
            Assert.That(groundMesh.subMeshCount, Is.EqualTo(3));
            Assert.That(groundMesh.vertexCount, Is.GreaterThan(3000));

            var fighters = Object.FindObjectsByType<FighterPresentation>();
            Assert.That(fighters, Has.Length.EqualTo(8));
            foreach (var fighter in fighters)
            {
                Assert.That(fighter.transform.Find("FighterIdentitySilhouette"), Is.Not.Null,
                    fighter.name + " is missing its presentation silhouette");
            }

            var pickups = Object.FindObjectsByType<GadgetPickup>();
            Assert.That(pickups, Has.Length.EqualTo(3));
            foreach (var pickup in pickups)
            {
                var identity = pickup.transform.Find("GadgetIdentityVisual");
                Assert.That(identity, Is.Not.Null,
                    pickup.name + " is missing its gadget identity visual");
                Assert.That(identity.GetComponentsInChildren<Transform>(true).Any(child => child.name == "PickupBeacon"), Is.True,
                    pickup.name + " is missing its pickup beacon");
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionFighterArtUsesSavedRenderOnlyPrefabs()
        {
            var fighters = Object.FindObjectsByType<FighterPresentation>();
            Assert.That(fighters, Has.Length.EqualTo(8));
            foreach (var fighter in fighters)
            {
                var modelRoot = fighter.transform.Find("FighterIdentitySilhouette")?.GetChild(0);
                Assert.That(modelRoot, Is.Not.Null, fighter.name + " is missing its saved production model instance");
                Assert.That(modelRoot.name, Does.Match("(Bijli|Pehel|Maya)Production"));
                var filters = modelRoot.GetComponentsInChildren<MeshFilter>(true);
                var renderers = modelRoot.GetComponentsInChildren<MeshRenderer>(true);
                Assert.That(filters, Is.Not.Empty, fighter.name + " production model has no reusable mesh assets");
                Assert.That(renderers, Is.Not.Empty, fighter.name + " production model has no renderers");
                foreach (var filter in filters)
                {
                    Assert.That(filter.sharedMesh, Is.Not.Null, fighter.name + " has a missing saved mesh reference");
                    Assert.That(filter.sharedMesh.uv, Has.Length.EqualTo(filter.sharedMesh.vertexCount),
                        fighter.name + " saved mesh must carry deterministic UVs for future authored materials");
                }

                var skins = modelRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                Assert.That(skins, Has.Length.EqualTo(1), fighter.name + " must have one skinned primary body/cloak renderer");
                foreach (var skin in skins)
                {
                    Assert.That(skin.sharedMesh, Is.Not.Null, fighter.name + " skinned primary has no saved mesh");
                    Assert.That(skin.bones, Has.Length.EqualTo(2), fighter.name + " skinned primary must use the hips/chest chain");
                    Assert.That(skin.sharedMesh.bindposes, Has.Length.EqualTo(skin.bones.Length),
                        fighter.name + " skinned primary bindposes do not match its bones");
                    Assert.That(skin.sharedMesh.boneWeights, Has.Length.EqualTo(skin.sharedMesh.vertexCount),
                        fighter.name + " skinned primary has incomplete bone weights");
                    Assert.That(skin.sharedMesh.boneWeights.Any(weight => weight.weight0 > 0.01f && weight.weight1 > 0.01f), Is.True,
                        fighter.name + " skinned primary must blend across the hips/chest chain");
                    Assert.That(skin.sharedMesh.uv, Has.Length.EqualTo(skin.sharedMesh.vertexCount),
                        fighter.name + " skinned primary must carry deterministic UVs");
                }

                Assert.That(modelRoot.GetComponentsInChildren<Collider>(true), Is.Empty,
                    fighter.name + " production art must not own gameplay collision");
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionFighterArtUsesDistinctFacetedSilhouetteMeshes()
        {
            var fighters = Object.FindObjectsByType<FighterPresentation>();
            Assert.That(fighters, Has.Length.EqualTo(8));
            var seenFighterProfiles = new HashSet<string>();
            foreach (var fighter in fighters)
            {
                var modelRoot = fighter.transform.Find("FighterIdentitySilhouette")?.GetChild(0);
                Assert.That(modelRoot, Is.Not.Null, fighter.name + " is missing its saved production model instance");
                var meshFilters = modelRoot.GetComponentsInChildren<MeshFilter>(true);
                var meshNames = meshFilters.Where(filter => filter.sharedMesh != null)
                    .Select(filter => filter.sharedMesh.name)
                    .ToArray();
                var vertexCount = meshFilters.Where(filter => filter.sharedMesh != null)
                    .Sum(filter => filter.sharedMesh.vertexCount);
                Assert.That(vertexCount, Is.GreaterThanOrEqualTo(260),
                    fighter.name + " production silhouette should use authored faceted mesh detail");

                var profile = modelRoot.name + ":" + string.Join(",", meshNames.OrderBy(name => name));
                seenFighterProfiles.Add(profile);
            }

            Assert.That(seenFighterProfiles, Has.Count.EqualTo(3),
                "Bijli, Pehel and Maya must retain distinct authored silhouette profiles");
            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionGadgetArtUsesSavedRenderOnlyPrefabs()
        {
            var pickups = Object.FindObjectsByType<GadgetPickup>();
            Assert.That(pickups, Has.Length.EqualTo(3));
            foreach (var pickup in pickups)
            {
                var identity = pickup.transform.Find("GadgetIdentityVisual");
                Assert.That(identity, Is.Not.Null, pickup.name + " is missing its gadget identity root");
                var modelRoot = identity.GetChild(0);
                Assert.That(modelRoot, Is.Not.Null, pickup.name + " is missing its saved production model instance");
                Assert.That(modelRoot.name, Does.Match("(Umbrella|Dhol|Tiffin)Production"));
                var filters = modelRoot.GetComponentsInChildren<MeshFilter>(true);
                var renderers = modelRoot.GetComponentsInChildren<MeshRenderer>(true);
                Assert.That(filters, Is.Not.Empty, pickup.name + " production model has no reusable mesh assets");
                Assert.That(renderers, Is.Not.Empty, pickup.name + " production model has no renderers");
                foreach (var filter in filters)
                {
                    Assert.That(filter.sharedMesh, Is.Not.Null, pickup.name + " has a missing saved mesh reference");
                    Assert.That(filter.sharedMesh.uv, Has.Length.EqualTo(filter.sharedMesh.vertexCount),
                        pickup.name + " saved mesh must carry deterministic UVs for future authored materials");
                }

                Assert.That(modelRoot.GetComponentsInChildren<Collider>(true), Is.Empty,
                    pickup.name + " production art must not own gameplay collision");
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionFighterArtUsesSavedRigAnimatorAndVfxCues()
        {
            var fighters = Object.FindObjectsByType<FighterPresentation>();
            Assert.That(fighters, Has.Length.EqualTo(8));
            foreach (var fighter in fighters)
            {
                var modelRoot = fighter.transform.Find("FighterIdentitySilhouette")?.GetChild(0);
                Assert.That(modelRoot, Is.Not.Null, fighter.name + " is missing its saved production model instance");

                var rig = modelRoot.Find("ProductionRig");
                Assert.That(rig, Is.Not.Null, fighter.name + " is missing the saved transform rig");
                Assert.That(rig.Find("Root/Hips/Chest/Head"), Is.Not.Null, fighter.name + " rig is missing the head chain");
                Assert.That(rig.Find("Root/Hips/Chest/LeftHand"), Is.Not.Null, fighter.name + " rig is missing the left hand chain");
                Assert.That(rig.Find("Root/Hips/LeftFoot"), Is.Not.Null, fighter.name + " rig is missing the left foot chain");

                var animator = modelRoot.GetComponent<Animator>();
                Assert.That(animator, Is.Not.Null, fighter.name + " is missing its production Animator");
                Assert.That(animator.runtimeAnimatorController, Is.Not.Null, fighter.name + " Animator has no saved controller");
                Assert.That(animator.parameters.Any(parameter => parameter.name == "State" && parameter.type == AnimatorControllerParameterType.Int), Is.True,
                    fighter.name + " Animator has no presentation state parameter");

                var cue = modelRoot.GetComponent<ProductionVfxCue>();
                Assert.That(cue, Is.Not.Null, fighter.name + " is missing its production VFX cue component");
                Assert.That(cue.HasAttackCue && cue.HasAbilityCue && cue.HasHitCue && cue.HasEliminationCue, Is.True,
                    fighter.name + " is missing one or more saved VFX cues");
                Assert.That(modelRoot.GetComponentsInChildren<ParticleSystem>(true), Has.Length.GreaterThanOrEqualTo(4));
            }

            var first = fighters[0];
            var cueOnFirst = first.transform.Find("FighterIdentitySilhouette")?.GetChild(0)?.GetComponent<ProductionVfxCue>();
            first.NotifyAttack();
            first.NotifyAbility();
            yield return null;
            Assert.That(cueOnFirst.AttackPlayCount, Is.EqualTo(1));
            Assert.That(cueOnFirst.AbilityPlayCount, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator ProductionAudioUsesOwnedSourcesAndMixerGroups()
        {
            Assert.That(Object.FindAnyObjectByType<BattleRajaAudioDirector>(), Is.Not.Null);
            var mixer = Resources.Load<AudioMixer>("Audio/V1/BattleRajaV1");
            Assert.That(mixer, Is.Not.Null, "V1 audio mixer asset is missing");
            foreach (var group in new[] { "Music", "Ambience", "UI", "Combat", "Abilities", "Gadgets", "Zone" })
            {
                Assert.That(mixer.FindMatchingGroups(group), Is.Not.Empty, group + " mixer group is missing");
            }

            foreach (var clip in new[]
                     {
                         "UiConfirm", "UiBack", "AttackBijli", "AttackPehel", "AttackMaya",
                         "AbilityBijli", "AbilityPehel", "AbilityMaya", "GadgetUmbrella", "GadgetDhol",
                         "GadgetTiffin", "Hit", "Elimination", "ZoneWarning", "ZoneClosing", "Victory",
                         "Defeat", "BazaarAmbience", "MatchMusic"
                     })
            {
                Assert.That(Resources.Load<AudioClip>("Audio/V1/" + clip), Is.Not.Null,
                    "Owned WAV source is missing: " + clip);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator ProductionVisualKitAnimatesFighterPartsAndPooledImpactCues()
        {
            var fighter = Object.FindAnyObjectByType<FighterPresentation>();
            var impactPool = Object.FindAnyObjectByType<CombatImpactFeedbackPool>();
            Assert.That(fighter, Is.Not.Null);
            Assert.That(fighter.AnimatedPartCount, Is.GreaterThan(0));
            Assert.That(impactPool, Is.Not.Null);

            fighter.NotifyAbility();
            yield return null;
            Assert.That(fighter.CurrentAnimation, Is.EqualTo(FighterPresentation.AnimationState.Ability));

            var activeBefore = impactPool.ActiveCount;
            impactPool.Play(fighter.transform.position, true);
            Assert.That(impactPool.ActiveCount, Is.EqualTo(activeBefore + 1));
            yield return new WaitForSeconds(0.22f);
            Assert.That(impactPool.ActiveCount, Is.LessThanOrEqualTo(activeBefore));
        }

        [UnityTest]
        public IEnumerator TouchControlsExposeReadableActionLabels()
        {
            Assert.That(GameObject.Find("AttackButton")?.GetComponentInChildren<Text>(true)?.text, Is.EqualTo("ATTACK"));
            Assert.That(GameObject.Find("AbilityButton")?.GetComponentInChildren<Text>(true)?.text, Is.EqualTo("ABILITY"));
            Assert.That(GameObject.Find("GadgetButton")?.GetComponentInChildren<Text>(true)?.text, Is.EqualTo("GADGET"));
            yield return null;
        }

        [Test]
        public void NarrowViewportsExpandOrthographicFramingWithoutChangingLandscapeSize()
        {
            var landscape = TopDownCameraController.CalculateResponsiveOrthographicSize(9.5f, 16f / 9f, 16f / 9f);
            var portrait = TopDownCameraController.CalculateResponsiveOrthographicSize(9.5f, 390f / 600f, 16f / 9f);
            var tallPortrait = TopDownCameraController.CalculateResponsiveOrthographicSize(9.5f, 1080f / 2460f, 16f / 9f);

            Assert.That(landscape, Is.EqualTo(9.5f).Within(0.0001f));
            Assert.That(portrait, Is.GreaterThan(landscape));
            Assert.That(tallPortrait, Is.LessThan(9.5f * 3.5f + 0.0001f));
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
            Assert.That(status, Does.Contain("SPAWN SHIELD"));
            Assert.That(status, Does.Not.Contain("SPAWNPROTECTION"));
            Assert.That(status, Does.Contain("Z 14.0 > 8.0"));
            Assert.That(status, Does.Contain("WARN 2.5s"));
        }

        [Test]
        public void GadgetHudUsesPlayerFacingLabels()
        {
            var held = GadgetHud.FormatStatus("gadget.tiffin_station", 0f, "Picked gadget.tiffin_station", string.Empty);
            var nearby = GadgetHud.FormatStatus(string.Empty, 0f, string.Empty, "gadget.umbrella_guard");

            Assert.That(held, Does.Contain("GADGET TIFFIN"));
            Assert.That(held, Does.Contain("TIFFIN READY"));
            Assert.That(held, Does.Not.Contain("tiffin_station"));
            Assert.That(held, Does.Not.Contain("[G]"));
            Assert.That(nearby, Does.Contain("NEAR UMBRELLA"));
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

            Assert.That(text, Does.Contain("WINNER YOU"));
            Assert.That(text, Does.Contain("#1 YOU  KOs 3  AST 1  DMG 120  SURV 25.0s"));
            Assert.That(text, Does.Contain("#2 RIVAL A  KOs 1  AST 2  DMG 40  SURV 12.0s"));
            Assert.That(text, Does.Not.Contain("PLAYER 1"));
        }

        [Test]
        public void FighterHudUsesSelectedFighterIdentity()
        {
            var pehel = BijliHud.FormatStatus(ProductionFighter.Pehel, 61, 85, "BOLT READY", "CHARGE READY");
            var maya = BijliHud.FormatStatus(ProductionFighter.Maya, 58, 80, "BOLT 0.2s", "DECOY ACTIVE");

            Assert.That(pehel, Does.StartWith("PEHEL   HP 61/85"));
            Assert.That(pehel, Does.Contain("CHARGE READY"));
            Assert.That(maya, Does.StartWith("MAYA   HP 58/80"));
            Assert.That(maya, Does.Contain("DECOY ACTIVE"));
        }
    }
}
