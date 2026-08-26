using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class AuthorityFoundationTests
    {
        [Test]
        public void AuthorityCollectsGadgetPickupDuringCanonicalTick()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                null,
                new[] { new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId) });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            });

            Assert.That(authority.IsGadgetPickupAvailable(0), Is.True, "available before tick");

            var tick = authority.Advance(1, 1f / 30f);

            Assert.That(authority.IsGadgetPickupAvailable(0), Is.False, "consumed after tick");
            Assert.That(tick.GadgetCollections, Has.Length.EqualTo(1), "one collection intent");
            Assert.That(tick.GadgetCollections[0].GadgetId, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(tick.GadgetCollections[0].CollectionEventId, Is.EqualTo(1));
        }

        [Test]
        public void MatchAuthorityEmitsZoneDamageIntentsOutsideTheCurrentZone()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja, 1f);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(0f, 0f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });

            // Warm through LoadWarmup(3s)+SpawnProtection(5s). The next fixed
            // 1/30s step enters Opening. Both actors stay safely inside the zone.
            // Advance through LoadWarmup(3s) + SpawnProtection(5s) + Opening(105s) to enter Pressure (where Aandhi damage is active).
            var pressureStartTick = (int)System.Math.Ceiling((3f + 5f + 105f) * 30f) + 1;
            for (var warmupTick = 1; warmupTick < pressureStartTick; warmupTick++) authority.Advance(warmupTick, 1f / 30f);

            // Now move actor 1 outside and advance until one damage window fires.
            authority.SetPosition(new CombatEntityId(1), new Float2(15f, 0f));
            MatchAuthorityTick tick = default;
            var totalEvents = 0;
            for (var tickIndex = pressureStartTick; tickIndex <= pressureStartTick + 60; tickIndex++)
            {
                tick = authority.Advance(tickIndex, 1f / 30f);
                totalEvents += tick.DamageEvents.Count(e => e.TargetId.Value == 1);
                if (totalEvents > 0) break;
            }

            Assert.That(totalEvents, Is.GreaterThanOrEqualTo(1));
            Assert.That(tick.Result.OutsideDamagePerSecond, Is.EqualTo(1));
            Assert.That(tick.DamageEvents[0].TargetId.Value, Is.EqualTo(1));
            Assert.That(tick.DamageEvents[0].DamageType, Is.EqualTo(DamageType.Aandhi));
            Assert.That(tick.DamageEvents[0].EventId, Is.GreaterThan(0));

            var damaged = authority.Simulation.GetSnapshots().Single(snapshot => snapshot.Id.Value == 1);
            Assert.That(damaged.CurrentHealth, Is.LessThan(100));
        }

        [Test]
        public void MatchAuthorityResolvesMovementAgainstCanonicalPositionExactlyOncePerTick()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            authority.ConfigureMovement(new CombatEntityId(1), new MovementTuning(
                maxSpeed: 4f,
                acceleration: 100f,
                deceleration: 100f,
                rotationSpeed: 720f,
                movementDeadZone: 0f,
                aimDeadZone: 0f,
                inputSensitivity: 1f));

            authority.Advance(9f);
            var command = new MovementCommand(1, 1, new Float2(1f, 0f), new Float2(1f, 0f));
            var first = authority.ResolveMovement(command, 1f / 30f);
            var duplicate = authority.ResolveMovement(command, 1f / 30f);
            var second = authority.ResolveMovement(new MovementCommand(1, 2, command.Movement, command.Aim), 1f / 30f);

            Assert.That(first.Applied, Is.True);
            Assert.That(first.Position.X, Is.EqualTo((100f / 30f) / 30f).Within(0.0001f));
            Assert.That(duplicate.Applied, Is.False);
            Assert.That(duplicate.Position, Is.EqualTo(first.Position));
            Assert.That(second.Applied, Is.True);
            Assert.That(second.Position.X, Is.GreaterThan(first.Position.X));
            Assert.That(authority.Simulation.TryGetSnapshot(new CombatEntityId(1), out var snapshot), Is.True);
            Assert.That(snapshot.Position, Is.EqualTo(second.Position));
        }

        [Test]
        public void MatchAuthorityRejectsNonFiniteMovementWithoutHaltingLaterCommands()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var actorId = new CombatEntityId(1);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(actorId, Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            authority.ConfigureMovement(actorId, new MovementTuning(
                maxSpeed: 4f,
                acceleration: 100f,
                deceleration: 100f,
                rotationSpeed: 720f,
                movementDeadZone: 0f,
                aimDeadZone: 0f,
                inputSensitivity: 1f));

            authority.Advance(9f);
            var nan = authority.ResolveMovement(
                new MovementCommand(1, 1, new Float2(float.NaN, 0f), Float2.Up),
                1f / 30f);
            var infinity = authority.ResolveMovement(
                new MovementCommand(1, 2, new Float2(float.PositiveInfinity, 0f), Float2.Up),
                1f / 30f);
            var valid = authority.ResolveMovement(
                new MovementCommand(1, 3, new Float2(1f, 0f), Float2.Up),
                1f / 30f);

            Assert.That(nan.Applied, Is.False);
            Assert.That(infinity.Applied, Is.False);
            Assert.That(valid.Applied, Is.True);
            Assert.That(valid.Position.X, Is.GreaterThan(0f));
        }

        [Test]
        public void UnifiedAuthorityEligibilityAllowsActionsOnlyInActiveCombatPhases()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var actorId = new CombatEntityId(1);
            var targetId = new CombatEntityId(2);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(actorId, Float2.Zero, 100),
                new MatchSpawn(targetId, new Float2(4f, 0f), 100)
            });
            authority.ConfigureMovement(actorId, new MovementTuning(
                maxSpeed: 4f,
                acceleration: 100f,
                deceleration: 100f,
                rotationSpeed: 720f,
                movementDeadZone: 0f,
                aimDeadZone: 0f,
                inputSensitivity: 1f));

            Assert.That(authority.ResolveMovement(new MovementCommand(1, 1, new Float2(1f, 0f), Float2.Up), 1f / 30f).Applied, Is.False);
            Assert.That(authority.ResolveAbilityDisplacement(actorId, 1, new Float2(1f, 0f)).Applied, Is.False);

            var warmupDamage = new DamageRequest(actorId, targetId, CombatFaction.Player, 10, DamageType.Projectile);
            Assert.That(authority.ResolveDamage(warmupDamage, CombatFaction.Enemy, false, false).Result.Applied, Is.False);
            authority.SyncHealth(actorId, 50);
            Assert.That(authority.ApplyHealing(actorId, 25), Is.Zero);

            var dholId = GadgetDefinition.DholBurst.GadgetId;
            Assert.That(authority.TryAcquireGadget(actorId, dholId), Is.True);
            var warmupGadget = authority.TryUseGadget(new GadgetUseCommand(
                actorId,
                dholId,
                Float2.Zero,
                Float2.Up,
                1));
            Assert.That(
                warmupGadget.Used,
                Is.False,
                $"phase={authority.CurrentPhase} failure={warmupGadget.Failure}");
            Assert.That(warmupGadget.Failure, Is.EqualTo(GadgetUseFailure.InvalidPlacement));

            for (var warmupTick = 1; warmupTick <= 241; warmupTick++) authority.Advance(warmupTick, 1f / 30f);
            Assert.That(authority.CurrentPhase, Is.EqualTo(MatchPhase.Opening));

            Assert.That(authority.ResolveMovement(new MovementCommand(1, 242, new Float2(1f, 0f), Float2.Up), 1f / 30f).Applied, Is.True);
            Assert.That(authority.ResolveAbilityDisplacement(actorId, 242, new Float2(1f, 0f)).Applied, Is.True);

            var activeDamage = new DamageRequest(actorId, targetId, CombatFaction.Player, 10, DamageType.Projectile, Float2.Up, 242);
            Assert.That(authority.ResolveDamage(activeDamage, CombatFaction.Enemy, false, false).Result.Applied, Is.True);
            authority.SyncHealth(actorId, 50);
            Assert.That(authority.ApplyHealing(actorId, 25), Is.EqualTo(25));

            Assert.That(authority.TryUseGadget(new GadgetUseCommand(
                actorId,
                dholId,
                Float2.Zero,
                Float2.Up,
                242)).Used,
                Is.True);

            Assert.That(authority.TryAcquireGadget(actorId, dholId), Is.True);
            authority.Advance(252, 300f);
            Assert.That(authority.CurrentPhase, Is.EqualTo(MatchPhase.Resolution));
            Assert.That(authority.ResolveMovement(new MovementCommand(1, 253, new Float2(1f, 0f), Float2.Up), 1f / 30f).Applied, Is.False);
            Assert.That(authority.ResolveAbilityDisplacement(actorId, 253, new Float2(1f, 0f)).Applied, Is.False);

            var resolutionDamage = new DamageRequest(actorId, targetId, CombatFaction.Player, 10, DamageType.Projectile, Float2.Up, 253);
            Assert.That(authority.ResolveDamage(resolutionDamage, CombatFaction.Enemy, false, false).Result.Applied, Is.False);
            Assert.That(authority.ApplyHealing(actorId, 25), Is.Zero);
            Assert.That(authority.TryUseGadget(new GadgetUseCommand(
                actorId,
                dholId,
                Float2.Zero,
                Float2.Up,
                253)).Used,
                Is.False);
        }

        [Test]
        public void MatchAuthorityResolvesAbilityDisplacementExactlyOncePerTick()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });

            authority.Advance(9f);
            var first = authority.ResolveAbilityDisplacement(new CombatEntityId(1), 1, new Float2(1f, 0f));
            var duplicate = authority.ResolveAbilityDisplacement(new CombatEntityId(1), 1, new Float2(1f, 0f));
            var invalid = authority.ResolveAbilityDisplacement(new CombatEntityId(1), 2, new Float2(float.NaN, 0f));

            Assert.That(first.Applied, Is.True);
            Assert.That(first.Position, Is.EqualTo(new Float2(1f, 0f)));
            Assert.That(duplicate.Applied, Is.False);
            Assert.That(duplicate.Position, Is.EqualTo(first.Position));
            Assert.That(invalid.Applied, Is.False);
            Assert.That(authority.Simulation.TryGetSnapshot(new CombatEntityId(1), out var snapshot), Is.True);
            Assert.That(snapshot.Position, Is.EqualTo(first.Position));
        }

        [Test]
        public void MatchAuthorityRejectsDuplicateAndOutOfOrderAttackCommands()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var actorId = new CombatEntityId(1);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(actorId, Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });

            // Commands are rejected during load warmup/spawn protection. Advance the
            // authority clock to the opening phase before exercising attack ordering.
            authority.Advance(9f);

            var first = authority.TryAcceptAttack(
                new AttackCommand(actorId, 1, Float2.Zero, Float2.Up, true),
                ProjectileWeaponDefinition.TrainingBolt,
                30);
            var duplicate = authority.TryAcceptAttack(
                new AttackCommand(actorId, 1, Float2.Zero, Float2.Up, true),
                ProjectileWeaponDefinition.TrainingBolt,
                30);
            var older = authority.TryAcceptAttack(
                new AttackCommand(actorId, 0, Float2.Zero, Float2.Up, true),
                ProjectileWeaponDefinition.TrainingBolt,
                30);
            var cooldown = authority.TryAcceptAttack(
                new AttackCommand(actorId, 2, Float2.Zero, Float2.Up, true),
                ProjectileWeaponDefinition.TrainingBolt,
                30);
            for (var i = 0; i < 10; i++) authority.Advance(1f / 30f);
            var afterCooldown = authority.TryAcceptAttack(
                new AttackCommand(actorId, 12, Float2.Zero, Float2.Up, true),
                ProjectileWeaponDefinition.TrainingBolt,
                30);

            Assert.That(first.Accepted, Is.True);
            Assert.That(duplicate.Accepted, Is.False);
            Assert.That(duplicate.Failure, Is.EqualTo(MatchAuthorityAttackFailure.OutOfOrder));
            Assert.That(older.Failure, Is.EqualTo(MatchAuthorityAttackFailure.OutOfOrder));
            Assert.That(cooldown.Failure, Is.EqualTo(MatchAuthorityAttackFailure.Cooldown));
            Assert.That(afterCooldown.Accepted, Is.True);
        }

        [Test]
        public void MatchAuthorityRejectsWarmupSpawnProtectionAndFarFutureAttacks()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var actorId = new CombatEntityId(1);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(actorId, Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });

            var warmup = authority.TryAcceptAttack(new AttackCommand(actorId, 1, Float2.Zero, Float2.Up, true));
            Assert.That(warmup.Failure, Is.EqualTo(MatchAuthorityAttackFailure.Warmup));

            authority.Advance(3f);
            var protectedAttack = authority.TryAcceptAttack(new AttackCommand(actorId, 1, Float2.Zero, Float2.Up, true));
            Assert.That(protectedAttack.Failure, Is.EqualTo(MatchAuthorityAttackFailure.SpawnProtection));

            authority.Advance(5f);
            var future = authority.TryAcceptAttack(new AttackCommand(actorId, 99, Float2.Zero, Float2.Up, true));
            Assert.That(future.Failure, Is.EqualTo(MatchAuthorityAttackFailure.FutureTick));
        }

        [Test]
        public void MatchAuthorityRejectsStaleAttackCommandsAndAnchorsCooldownToAuthorityClock()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var actorId = new CombatEntityId(1);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(actorId, Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });

            authority.Advance(9f);
            var first = authority.TryAcceptAttack(new AttackCommand(actorId, 1, Float2.Zero, Float2.Up, true));
            Assert.That(first.Accepted, Is.True);

            // Advance the authority well past the TrainingBolt cooldown so only
            // a stale caller-supplied tick could allow an extra immediate shot.
            for (var i = 0; i < 30; i++) authority.Advance(1f / 30f);

            // Tick 15 sits inside the first shot's already-expired-by-authority-
            // time cooldown window but far behind the authority clock; it must be
            // rejected instead of consuming its cooldown entirely in the past.
            var stale = authority.TryAcceptAttack(new AttackCommand(actorId, 15, Float2.Zero, Float2.Up, true, 2));
            Assert.That(stale.Accepted, Is.False);
            Assert.That(stale.Failure, Is.EqualTo(MatchAuthorityAttackFailure.StaleTick));

            var current = authority.TryAcceptAttack(new AttackCommand(actorId, 31, Float2.Zero, Float2.Up, true, 3));
            Assert.That(current.Accepted, Is.True);
        }

        [Test]
        public void MatchAuthorityUsesConfiguredWeaponAndCanonicalOriginInsteadOfCommandValues()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var actorId = new CombatEntityId(1);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(actorId, new Float2(2f, 3f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            authority.ConfigureFaction(actorId, CombatFaction.Player);
            authority.ConfigureWeapon(actorId, ProjectileWeaponDefinition.BijliElectricBolt, 30);
            authority.Advance(9f);

            var result = authority.TryAcceptAttack(new AttackCommand(
                actorId,
                1,
                new Float2(999f, 999f),
                new Float2(1f, 0f),
                true,
                42));

            Assert.That(result.Accepted, Is.True);
            Assert.That(result.Weapon.Damage, Is.EqualTo(ProjectileWeaponDefinition.BijliElectricBolt.Damage));
            Assert.That(result.Faction, Is.EqualTo(CombatFaction.Player));
            Assert.That(result.Origin, Is.EqualTo(new Float2(2.7f, 3f)));
            Assert.That(result.Direction, Is.EqualTo(new Float2(1f, 0f)));
        }

        [Test]
        public void MatchAuthorityOwnsMayaDecoyLifetimeAndDamage()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            authority.Advance(9f);

            var spawned = authority.TrySpawnMayaDecoy(new CombatEntityId(1), 1, Float2.Zero);
            var duplicate = authority.TrySpawnMayaDecoy(new CombatEntityId(1), 1, Float2.Zero);

            Assert.That(spawned.OwnerId.Value, Is.EqualTo(1));
            Assert.That(spawned.DecoyId.Value, Is.EqualTo(100001));
            Assert.That(spawned.Active, Is.True);
            Assert.That(spawned.Targetable, Is.True);
            Assert.That(duplicate.RemainingSeconds, Is.EqualTo(spawned.RemainingSeconds).Within(0.0001f));

            authority.SetPosition(new CombatEntityId(1), new Float2(2f, 0f));
            authority.Advance(1f / 30f);
            var followed = authority.GetMayaDecoySnapshot(new CombatEntityId(1));
            Assert.That(followed.Position.X, Is.GreaterThan(0f));

            var request = new DamageRequest(
                new CombatEntityId(2),
                spawned.DecoyId,
                CombatFaction.Player,
                spawned.MaxHealth,
                DamageType.Projectile,
                new Float2(-1f, 0f),
                2);
            var damage = authority.ResolveMayaDecoyDamage(request, CombatFaction.Enemy, false, false);
            var duplicateDamage = authority.ResolveMayaDecoyDamage(request, CombatFaction.Enemy, false, false);

            Assert.That(damage.Result.Applied, Is.True);
            Assert.That(damage.Result.TargetDefeated, Is.True);
            Assert.That(damage.CurrentHealthAfter, Is.EqualTo(0));
            Assert.That(duplicateDamage.Result.Applied, Is.False);
            Assert.That(authority.GetMayaDecoySnapshot(new CombatEntityId(1)).Active, Is.False);
        }

        [Test]
        public void AuthorityOwnsPehelChargeCaptureDamageAndThrowDisplacement()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var pehelId = new CombatEntityId(1);
            var targetId = new CombatEntityId(2);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(pehelId, new Float2(-6f, 0f), 125),
                new MatchSpawn(targetId, new Float2(6f, 0f), 100)
            });
            // Advance through the full spawn-protection window before combat actions.
            // Advance through the full spawn-protection window before combat actions.
            for (var warmupTick = 1; warmupTick <= 241; warmupTick++) authority.Advance(warmupTick, 1f / 30f);
            authority.ConfigureFaction(pehelId, CombatFaction.Enemy);
            authority.ConfigureFaction(targetId, CombatFaction.Enemy);
            authority.SetPosition(targetId, new Float2(-4.4f, 0f));

            var command = new AbilityCommand(
                pehelId,
                242,
                FighterSpecialDefinition.PehelChargeThrow.AbilityId,
                new Float2(1f, 0f),
                true);
            var firstStart = authority.TryStartPehelCharge(command, new Float2(1f, 0f), new Float2(1f, 0f));
            var duplicateStart = authority.TryStartPehelCharge(command, new Float2(1f, 0f), new Float2(1f, 0f));
            Assert.That(
                firstStart.Accepted,
                Is.True,
                $"phase={authority.CurrentPhase} tick={authority.CurrentSimulationTick}");
            Assert.That(firstStart.AbilityExecutionId, Is.GreaterThan(0));
            Assert.That(duplicateStart.Accepted, Is.False);
            Assert.That(duplicateStart.AbilityExecutionId, Is.EqualTo(0));

            MatchAuthorityChargeThrow thrown = default(MatchAuthorityChargeThrow);
            for (var tick = 242; tick <= 280; tick++)
            {
                thrown = authority.AdvancePehelCharge(pehelId, tick, 1f / 30f, 3.2f);
                if (thrown.HasDamage) break;
            }

            Assert.That(thrown.Accepted, Is.True);
            Assert.That(thrown.HasDamage, Is.True);
            Assert.That(thrown.Damage.Result.Applied, Is.True);
            Assert.That(thrown.Damage.Result.AmountApplied, Is.EqualTo(FighterSpecialDefinition.PehelChargeThrow.Magnitude));
            Assert.That(thrown.HasTargetDisplacement, Is.True);
            Assert.That(thrown.TargetDisplacement.Position.X, Is.GreaterThan(-4.4f));
            Assert.That(authority.Simulation.TryGetSnapshot(targetId, out var target), Is.True);
            Assert.That(target.CurrentHealth, Is.EqualTo(97));
            Assert.That(authority.GetPehelChargeState(pehelId).CapturedTargetId, Is.EqualTo(targetId));
        }

        [Test]
        public void AuthorityOwnsBijliDashEligibilityCollisionAndReplayState()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            var bijliId = new CombatEntityId(1);
            authority.ConfigureArenaCollision(new ArenaCollisionDefinition(
                new Float2(-13f, -9f),
                new Float2(13f, 9f),
                0.45f,
                new ArenaObstacle[0],
                "authority-dash-open"));
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(bijliId, new Float2(-6f, 0f), FighterDefinition.Bijli.MaxHealth),
                new MatchSpawn(new CombatEntityId(2), new Float2(6f, 0f), 100)
            });

            var warmupCommand = new AbilityCommand(
                bijliId,
                1,
                FighterDefinition.Bijli.Ability.AbilityId,
                new Float2(1f, 0f),
                true);
            Assert.That(
                authority.TryStartBijliDash(warmupCommand, Float2.Zero, Float2.Up).Accepted,
                Is.False,
                "stale command");

            for (var warmupTick = 1; warmupTick <= 241; warmupTick++) authority.Advance(warmupTick, 1f / 30f);
            Assert.That(authority.TryStartBijliDash(warmupCommand, Float2.Zero, Float2.Up).Accepted, Is.False);

            var command = new AbilityCommand(
                bijliId,
                251,
                FighterDefinition.Bijli.Ability.AbilityId,
                new Float2(1f, 0f),
                true);
            var firstStart = authority.TryStartBijliDash(command, new Float2(1f, 0f), new Float2(1f, 0f));
            var duplicateStart = authority.TryStartBijliDash(command, new Float2(1f, 0f), new Float2(1f, 0f));
            Assert.That(
                firstStart.Accepted,
                Is.True,
                $"phase={authority.CurrentPhase} tick={authority.CurrentSimulationTick}");
            Assert.That(firstStart.AbilityExecutionId, Is.GreaterThan(0));
            Assert.That(duplicateStart.Accepted, Is.False);

            var displacementTotal = Float2.Zero;
            MatchAuthorityDashStep step;
            for (var tick = 252; tick <= 280; tick++)
            {
                step = authority.AdvanceBijliDash(bijliId, tick, 1f / 30f);
                if (step.Displacement.Applied) displacementTotal += step.Displacement.Displacement;
            }

            Assert.That(displacementTotal.X, Is.EqualTo(FighterDefinition.Bijli.Ability.Distance).Within(0.001f));
            Assert.That(displacementTotal.Y, Is.EqualTo(0f).Within(0.001f));
            Assert.That(authority.GetBijliDashState(bijliId).State, Is.EqualTo(FighterActionState.Cooldown));
        }

        [Test]
        public void MatchAuthorityOwnsPickupAvailabilityAndGadgetCollection()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja, 1f);
            authority.ConfigureItems(
                new[] { new MatchPickupDefinition(0, MatchPickupKind.Health, 25, 12f) },
                new[] { new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId, new Float2(3f, 3f), 1.3f) });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });

            for (var warmupTick = 1; warmupTick <= 12; warmupTick++) authority.Advance(warmupTick, 1f);
            Assert.That(authority.IsPickupAvailable(0), Is.True);

            // Collections are resolved atomically inside authority ticks.
            authority.SetPosition(new CombatEntityId(1), new Float2(0.5f, 0f));
            authority.SyncHealth(new CombatEntityId(1), 50);
            var tick = authority.Advance(13, 1f);

            Assert.That(tick.PickupCollections, Has.Length.EqualTo(1));
            Assert.That(tick.PickupCollections[0].CollectorId.Value, Is.EqualTo(1));
            Assert.That(tick.PickupCollections[0].HealAmount, Is.EqualTo(25));
            Assert.That(tick.PickupCollections[0].CollectionEventId, Is.GreaterThan(0));
            Assert.That(tick.PickupCollections[0].HealingEventId, Is.GreaterThan(0));
            Assert.That(authority.Simulation.GetSnapshots().Single(s => s.Id.Value == 1).CurrentHealth, Is.EqualTo(75));

            for (var warmupTick = 14; warmupTick <= 25; warmupTick++) authority.Advance(warmupTick, 1f);
            authority.SetPosition(new CombatEntityId(1), new Float2(3f, 3.5f));
            var gadgetTick = authority.Advance(26, 1f);

            Assert.That(gadgetTick.GadgetCollections, Has.Length.EqualTo(1));
            Assert.That(gadgetTick.GadgetCollections[0].CollectorId.Value, Is.EqualTo(1));
            Assert.That(gadgetTick.GadgetCollections[0].GadgetId, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(gadgetTick.GadgetCollections[0].CollectionEventId, Is.GreaterThan(0));
            Assert.That(authority.IsGadgetPickupAvailable(0), Is.False);

            var replayTick = authority.Advance(27, 1f);
            Assert.That(replayTick.PickupCollections, Is.Empty);
            Assert.That(replayTick.GadgetCollections, Is.Empty);
        }

        [Test]
        public void MatchAuthorityRoutesDamageEventsAndRejectsDuplicateEliminations()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100),
                new MatchSpawn(new CombatEntityId(3), new Float2(-4f, 0f), 100)
            });

            var setup = new DamageRequest(new CombatEntityId(1), new CombatEntityId(2), CombatFaction.Player, 40, DamageType.Projectile, new Float2(1f, 0f), 1);
            var finishing = new DamageRequest(new CombatEntityId(1), new CombatEntityId(2), CombatFaction.Player, 60, DamageType.Projectile, new Float2(1f, 0f), 2);

            Assert.That(authority.RecordDamage(new CombatDamageEvent(setup, 40, false, 60, 1)), Is.True);
            Assert.That(authority.RecordDamage(new CombatDamageEvent(finishing, 60, true, 0, 2)), Is.True);
            Assert.That(authority.RecordDamage(new CombatDamageEvent(finishing, 60, true, 0, 3)), Is.False);

            var snapshots = authority.Simulation.GetSnapshots();
            Assert.That(snapshots.Single(snapshot => snapshot.Id.Value == 1).DamageDealt, Is.EqualTo(100));
            Assert.That(snapshots.Single(snapshot => snapshot.Id.Value == 1).Eliminations, Is.EqualTo(1));
            Assert.That(snapshots.Single(snapshot => snapshot.Id.Value == 2).Placement, Is.EqualTo(3));
        }

        [Test]
        public void MatchAuthorityResolvesDamageAgainstCanonicalHealthExactlyOnce()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100),
                new MatchSpawn(new CombatEntityId(3), new Float2(-4f, 0f), 100)
            });
            authority.Advance(9f);

            var request = new DamageRequest(
                new CombatEntityId(1),
                new CombatEntityId(2),
                CombatFaction.Player,
                40,
                DamageType.Projectile,
                new Float2(1f, 0f),
                1);
            var first = authority.ResolveDamage(request, CombatFaction.Enemy, false, false);
            var second = authority.ResolveDamage(
                new DamageRequest(request.InstigatorId, request.TargetId, request.InstigatorFaction, 60,
                    request.DamageType, request.HitDirection, 2),
                CombatFaction.Enemy,
                false,
                false);
            var duplicate = authority.ResolveDamage(request, CombatFaction.Enemy, false, false);
            var attacker = authority.Simulation.GetSnapshots().Single(snapshot => snapshot.Id.Value == 1);
            var target = authority.Simulation.GetSnapshots().Single(snapshot => snapshot.Id.Value == 2);

            Assert.That(first.Result.Applied, Is.True);
            Assert.That(first.CurrentHealthAfter, Is.EqualTo(60));
            Assert.That(second.Result.TargetDefeated, Is.True);
            Assert.That(second.CurrentHealthAfter, Is.Zero);
            Assert.That(duplicate.Result.Applied, Is.False);
            Assert.That(attacker.DamageDealt, Is.EqualTo(100));
            Assert.That(attacker.Eliminations, Is.EqualTo(1));
            Assert.That(target.Placement, Is.EqualTo(3));
        }

        [Test]
        public void MatchAuthorityAppliesHealingToCanonicalHealth()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });
            authority.SyncHealth(new CombatEntityId(1), 40);
            authority.Advance(9f);

            Assert.That(authority.ApplyHealing(new CombatEntityId(1), 25), Is.EqualTo(25));
            Assert.That(authority.Simulation.GetSnapshots().Single(snapshot => snapshot.Id.Value == 1).CurrentHealth, Is.EqualTo(65));
            Assert.That(authority.ApplyHealing(new CombatEntityId(1), 50), Is.EqualTo(35));
            Assert.That(authority.ApplyHealing(new CombatEntityId(2), 20), Is.EqualTo(0));
        }

        [Test]
        public void MatchAuthorityOwnsGadgetUseAndRejectsDuplicateCommands()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                null,
                new[] { new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId) });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            });

            // Gadget pickup resolves atomically through authority ticks. Capture
            // the first tick, then warm into the active combat window for use.
            var collectTick = authority.Advance(1, 1f / 30f);
            for (var warmupTick = 2; warmupTick <= 241; warmupTick++) authority.Advance(warmupTick, 1f / 30f);
            Assert.That(collectTick.GadgetCollections, Has.Length.EqualTo(1));
            Assert.That(collectTick.GadgetCollections[0].GadgetId, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));

            var command = new GadgetUseCommand(new CombatEntityId(1), GadgetDefinition.DholBurst.GadgetId, Float2.Zero, new Float2(1f, 0f), 242);
            var used = authority.TryUseGadget(command);
            var duplicate = authority.TryUseGadget(command);

            Assert.That(used.Used, Is.True);
            Assert.That(used.EventId, Is.EqualTo(1));
            Assert.That(used.Effect.Kind, Is.EqualTo(GadgetEffectKind.DholBurst));
            Assert.That(used.Effect.Displacements, Has.Length.EqualTo(1));
            Assert.That(used.Effect.Displacements[0].TargetId.Value, Is.EqualTo(2));
            Assert.That(used.Effect.Displacements[0].Displacement.X, Is.EqualTo(0.32f).Within(0.0001f));
            Assert.That(authority.Simulation.TryGetSnapshot(new CombatEntityId(2), out var displaced), Is.True);
            Assert.That(displaced.Position.X, Is.EqualTo(3.32f).Within(0.0001f));
            Assert.That(duplicate.Used, Is.False);
            Assert.That(duplicate.Failure, Is.EqualTo(GadgetUseFailure.NotHeld));
            Assert.That(duplicate.EventId, Is.EqualTo(0));
        }

        [Test]
        public void AuthorityAdvancesGadgetCooldownOnAuthoritativeTicks()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });

            var gadgetId = GadgetDefinition.DholBurst.GadgetId;
            for (var warmupTick = 1; warmupTick <= 241; warmupTick++) authority.Advance(warmupTick, 1f / 30f);
            Assert.That(
                authority.CurrentPhase,
                Is.EqualTo(MatchPhase.Opening),
                $"tick={authority.CurrentSimulationTick}");
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            var first = new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 242);
            var firstResult = authority.TryUseGadget(first);
            Assert.That(
                firstResult.Used,
                Is.True,
                $"phase={authority.CurrentPhase} failure={firstResult.Failure}");

            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            var blocked = authority.TryUseGadget(new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 243));
            Assert.That(blocked.Failure, Is.EqualTo(GadgetUseFailure.Cooldown));

            for (var tick = 242; tick <= 542; tick++) authority.Advance(tick, 1f / 30f);

            var second = authority.TryUseGadget(new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 543));
            Assert.That(second.Used, Is.True);
        }

        [Test]
        public void AuthoritySelectsDeterministicNearbyCollectorsAndOwnsRangeRules()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                new[]
                {
                    new MatchPickupDefinition(0, MatchPickupKind.Health, 25, 12f, Float2.Zero, 4f)
                },
                new[]
                {
                    new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId, Float2.Zero, 4f)
                });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            });
            authority.SyncHealth(new CombatEntityId(1), 50);
            authority.SyncHealth(new CombatEntityId(2), 50);

            var tick = authority.Advance(1, 1f);

            Assert.That(tick.PickupCollections, Has.Length.EqualTo(1));
            Assert.That(tick.PickupCollections[0].CollectorId.Value, Is.EqualTo(1));
            Assert.That(tick.PickupCollections[0].HealAmount, Is.EqualTo(25));
            Assert.That(tick.PickupCollections[0].CollectionEventId, Is.EqualTo(1));
            Assert.That(tick.PickupCollections[0].HealingEventId, Is.EqualTo(1));
            Assert.That(tick.GadgetCollections, Has.Length.EqualTo(1));
            Assert.That(tick.GadgetCollections[0].CollectorId.Value, Is.EqualTo(1));
            Assert.That(tick.GadgetCollections[0].GadgetId, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(tick.GadgetCollections[0].CollectionEventId, Is.EqualTo(2));
            Assert.That(authority.IsPickupAvailable(0), Is.False);
            Assert.That(authority.IsGadgetPickupAvailable(0), Is.False);

            var repeatTick = authority.Advance(2, 1f);
            Assert.That(repeatTick.PickupCollections, Is.Empty);
            Assert.That(repeatTick.GadgetCollections, Is.Empty);
        }

        [Test]
        public void AuthorityTicksTiffinHealingAndExpiry()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });
            authority.SyncHealth(new CombatEntityId(1), 50);
            var gadgetId = GadgetDefinition.TiffinStation.GadgetId;
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            for (var warmupTick = 1; warmupTick <= 241; warmupTick++) authority.Advance(warmupTick, 1f / 30f);

            var use = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                gadgetId,
                Float2.Zero,
                Float2.Up,
                242));
            Assert.That(use.Used, Is.True);
            Assert.That(use.Effect.StationId, Is.GreaterThan(0));

            var healed = false;
            var expired = false;
            for (var tick = 243; tick <= 542; tick++)
            {
                var result = authority.Advance(tick, 1f / 30f);
                healed |= result.GadgetHealingIntents.Any(intent => intent.TargetId.Value == 1 && intent.Amount == GadgetDefinition.TiffinStation.Magnitude);
                expired |= result.ExpiredStationIds.Contains(use.Effect.StationId);
            }

            Assert.That(healed, Is.True);
            Assert.That(expired, Is.True);
        }

        [Test]
        public void AuthorityAppliesTiffinHealingExactlyOnceWithUniqueEventIds()
        {
            // Use the standard SoloRaja definition; warm past LoadWarmup +
            // SpawnProtection so the match is inside the Opening combat window.
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });
            for (var warmupTick = 1; warmupTick <= 241; warmupTick++) authority.Advance(warmupTick, 1f / 30f);
            authority.SyncHealth(new CombatEntityId(1), 50);
            var gadgetId = GadgetDefinition.TiffinStation.GadgetId;
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            var use = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                gadgetId,
                Float2.Zero,
                Float2.Up,
                242));
            Assert.That(use.Used, Is.True);

            var healingIds = new List<int>();
            for (var tick = 242; tick <= 331; tick++)
            {
                var result = authority.Advance(tick, 1f / 30f);
                foreach (var intent in result.GadgetHealingIntents)
                {
                    if (intent.TargetId.Value == 1) healingIds.Add(intent.EventId);
                }
            }

            // Three one-second heal windows inside three seconds; every applied
            // heal carries a unique identity and no duplicate applications occur.
            Assert.That(healingIds.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(healingIds, Is.Unique);
            var healedHealth = authority.Simulation.GetSnapshots().Single(s => s.Id.Value == 1).CurrentHealth;
            Assert.That(healedHealth, Is.GreaterThan(50));
        }

        [Test]
        public void RestartResetsEveryIdentityStream()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                null,
                new[] { new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId, new Float2(8f, -8f), 1.3f) });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            });

            // Warm past LoadWarmup(3s) + SpawnProtection(5s). Both actors stay
            // inside the zone so no damage identity is consumed during setup.
            // Advance through LoadWarmup(3s) + SpawnProtection(5s) + Opening(105s) to enter Pressure.
            var pressureTick = (int)System.Math.Ceiling((3f + 5f + 105f) * 30f) + 1;
            for (var warmup = 1; warmup < pressureTick; warmup++) authority.Advance(warmup, 1f / 30f);

            // Move actor 1 outside and collect damage identities until one fires.
            authority.SetPosition(new CombatEntityId(1), new Float2(15f, 0f));
            var firstDamageId = -1;
            for (var tick = pressureTick; tick <= pressureTick + 60; tick++)
            {
                var t = authority.Advance(tick, 1f / 30f);
                var ids = t.DamageEvents.Where(e => e.TargetId.Value == 1).Select(e => e.EventId).ToList();
                if (ids.Count > 0)
                {
                    firstDamageId = ids[0];
                    break;
                }
            }

            Assert.That(firstDamageId, Is.EqualTo(1));

            authority.SetPosition(new CombatEntityId(1), new Float2(8f, -8f));
            var gadgetCollected = false;
            for (var tick = pressureTick + 61; tick <= pressureTick + 120; tick++)
            {
                var t = authority.Advance(tick, 1f / 30f);
                if (t.GadgetCollections.Any())
                {
                    Assert.That(t.GadgetCollections[0].CollectionEventId, Is.EqualTo(1));
                    gadgetCollected = true;
                    break;
                }
            }
            Assert.That(gadgetCollected, Is.True);

            var use = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                GadgetDefinition.DholBurst.GadgetId,
                new Float2(8f, -8f),
                new Float2(1f, 0f),
                11));
            Assert.That(use.EventId, Is.EqualTo(1));

            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            });
            authority.SetPosition(new CombatEntityId(1), new Float2(15f, 0f));

            // Every identity stream restarts from a clean deterministic base.
            var restartDamageIds = new List<int>();
            for (var tick = 1; tick <= pressureTick + 60; tick++)
            {
                restartDamageIds.AddRange(authority.Advance(tick, 1f / 30f).DamageEvents
                    .Select(e => e.EventId)
                    .Where(id => id > 0));
            }

            Assert.That(restartDamageIds.First(), Is.EqualTo(1));
        }

        [Test]
        public void AuthorityOwnsUmbrellaMitigationAndExpiresTheGuard()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });
            var gadgetId = GadgetDefinition.UmbrellaGuard.GadgetId;
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            authority.Advance(9f);
            Assert.That(authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 1)).Used, Is.True);

            var incoming = new DamageRequest(
                new CombatEntityId(2),
                new CombatEntityId(1),
                CombatFaction.Enemy,
                100,
                DamageType.Projectile,
                new Float2(0f, -1f),
                1);
            Assert.That(authority.ApplyDamageMitigation(incoming).RawAmount, Is.EqualTo(30));
            Assert.That(authority.ApplyDamageMitigation(new DamageRequest(
                incoming.InstigatorId,
                incoming.TargetId,
                incoming.InstigatorFaction,
                incoming.RawAmount,
                DamageType.Aandhi,
                incoming.HitDirection,
                incoming.SimulationTick)).RawAmount, Is.EqualTo(100));

            for (var tick = 1; tick <= 106; tick++) authority.Advance(tick, 1f / 30f);
            Assert.That(authority.ApplyDamageMitigation(incoming).RawAmount, Is.EqualTo(100));
        }

        [Test]
        public void AuthorityOwnsTiffinStationDamageAndRemovesDestroyedStations()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });
            var gadgetId = GadgetDefinition.TiffinStation.GadgetId;
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            authority.Advance(9f);
            var use = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 1));
            Assert.That(use.Used, Is.True);

            var first = authority.TryDamageStation(use.Effect.StationId, 10);
            Assert.That(first.Applied, Is.True);
            Assert.That(first.AmountApplied, Is.EqualTo(10));
            Assert.That(first.CurrentHealth, Is.EqualTo(35));
            Assert.That(first.Destroyed, Is.False);

            var destroyed = authority.TryDamageStation(use.Effect.StationId, 50);
            Assert.That(destroyed.Applied, Is.True);
            Assert.That(destroyed.AmountApplied, Is.EqualTo(35));
            Assert.That(destroyed.CurrentHealth, Is.Zero);
            Assert.That(destroyed.Destroyed, Is.True);
            Assert.That(authority.TryDamageStation(use.Effect.StationId, 1).Applied, Is.False);
        }
    }
}
