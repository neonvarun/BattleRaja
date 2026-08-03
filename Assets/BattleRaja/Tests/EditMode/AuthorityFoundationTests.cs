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
        public void MatchAuthorityEmitsZoneDamageIntentsOutsideTheCurrentZone()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja, 1f);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(0f, 0f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            authority.SetPosition(new CombatEntityId(1), new Float2(15f, 0f));

            authority.Advance(8f);
            var tick = authority.Advance(1f);

            Assert.That(tick.SimulationTick, Is.EqualTo(1));
            Assert.That(tick.Result.OutsideDamagePerSecond, Is.EqualTo(5));
            Assert.That(tick.OutsideDamageRequests, Has.Length.EqualTo(1));
            Assert.That(tick.OutsideDamageRequests[0].TargetId.Value, Is.EqualTo(1));
            Assert.That(tick.OutsideDamageRequests[0].DamageType, Is.EqualTo(DamageType.Aandhi));
            Assert.That(tick.OutsideDamageRequests[0].SimulationTick, Is.EqualTo(tick.SimulationTick));
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
        public void MatchAuthorityResolvesAbilityDisplacementExactlyOncePerTick()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });

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
        public void MatchAuthorityOwnsMayaDecoyLifetimeAndDamage()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });

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
            authority.ConfigureFaction(pehelId, CombatFaction.Enemy);
            authority.ConfigureFaction(targetId, CombatFaction.Player);
            authority.SetPosition(targetId, new Float2(-4.4f, 0f));

            var command = new AbilityCommand(
                pehelId,
                1,
                FighterSpecialDefinition.PehelChargeThrow.AbilityId,
                new Float2(1f, 0f),
                true);
            Assert.That(authority.TryStartPehelCharge(command, new Float2(1f, 0f), new Float2(1f, 0f)), Is.True);
            Assert.That(authority.TryStartPehelCharge(command, new Float2(1f, 0f), new Float2(1f, 0f)), Is.False);

            MatchAuthorityChargeThrow thrown = default(MatchAuthorityChargeThrow);
            for (var tick = 1; tick <= 30; tick++)
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
        public void MatchAuthorityOwnsPickupAvailabilityAndGadgetCollection()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja, 1f);
            authority.ConfigureItems(
                new[] { new MatchPickupDefinition(0, MatchPickupKind.Health, 25, 12f) },
                new[] { new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId) });
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), 100)
            });

            var heal = authority.TryCollectPickup(0, 50, 100);
            Assert.That(heal.Collected, Is.True);
            Assert.That(heal.HealAmount, Is.EqualTo(25));
            Assert.That(authority.IsPickupAvailable(0), Is.False);
            Assert.That(authority.TryCollectPickup(0, 50, 100).Collected, Is.False);

            authority.Advance(12f);
            Assert.That(authority.IsPickupAvailable(0), Is.True);

            var gadget = authority.TryCollectGadget(0, false);
            Assert.That(gadget.Collected, Is.True);
            Assert.That(gadget.GadgetId, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(authority.IsGadgetPickupAvailable(0), Is.False);
            Assert.That(authority.TryCollectGadget(0, false).Collected, Is.False);
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

            Assert.That(authority.TryCollectGadget(new CombatEntityId(1), 0).Collected, Is.True);
            var command = new GadgetUseCommand(new CombatEntityId(1), GadgetDefinition.DholBurst.GadgetId, Float2.Zero, new Float2(1f, 0f), 1);
            var used = authority.TryUseGadget(command);
            var duplicate = authority.TryUseGadget(command);

            Assert.That(used.Used, Is.True);
            Assert.That(used.Effect.Kind, Is.EqualTo(GadgetEffectKind.DholBurst));
            Assert.That(used.Effect.Displacements, Has.Length.EqualTo(1));
            Assert.That(used.Effect.Displacements[0].TargetId.Value, Is.EqualTo(2));
            Assert.That(used.Effect.Displacements[0].Displacement.X, Is.EqualTo(0.32f).Within(0.0001f));
            Assert.That(authority.Simulation.TryGetSnapshot(new CombatEntityId(2), out var displaced), Is.True);
            Assert.That(displaced.Position.X, Is.EqualTo(3.32f).Within(0.0001f));
            Assert.That(duplicate.Used, Is.False);
            Assert.That(duplicate.Failure, Is.EqualTo(GadgetUseFailure.NotHeld));
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
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            var first = new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 1);
            Assert.That(authority.TryUseGadget(first).Used, Is.True);

            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True);
            var blocked = authority.TryUseGadget(new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 2));
            Assert.That(blocked.Failure, Is.EqualTo(GadgetUseFailure.Cooldown));

            for (var tick = 1; tick <= 300; tick++) authority.Advance(tick, 1f / 30f);

            var second = authority.TryUseGadget(new GadgetUseCommand(new CombatEntityId(1), gadgetId, Float2.Zero, Float2.Up, 301));
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

            var collections = authority.CollectNearby();

            Assert.That(collections.PickupCollections, Has.Length.EqualTo(1));
            Assert.That(collections.PickupCollections[0].CollectorId.Value, Is.EqualTo(1));
            Assert.That(collections.PickupCollections[0].HealAmount, Is.EqualTo(25));
            Assert.That(collections.GadgetCollections, Has.Length.EqualTo(1));
            Assert.That(collections.GadgetCollections[0].CollectorId.Value, Is.EqualTo(1));
            Assert.That(collections.GadgetCollections[0].GadgetId, Is.EqualTo(GadgetDefinition.DholBurst.GadgetId));
            Assert.That(authority.IsPickupAvailable(0), Is.False);
            Assert.That(authority.IsGadgetPickupAvailable(0), Is.False);
            Assert.That(authority.CollectNearby().PickupCollections, Is.Empty);
            Assert.That(authority.CollectNearby().GadgetCollections, Is.Empty);
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

            var use = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                gadgetId,
                Float2.Zero,
                Float2.Up,
                1));
            Assert.That(use.Used, Is.True);
            Assert.That(use.Effect.StationId, Is.GreaterThan(0));

            var healed = false;
            var expired = false;
            for (var tick = 1; tick <= 300; tick++)
            {
                var result = authority.Advance(tick, 1f / 30f);
                healed |= result.GadgetHealingIntents.Any(intent => intent.TargetId.Value == 1 && intent.Amount == GadgetDefinition.TiffinStation.Magnitude);
                expired |= result.ExpiredStationIds.Contains(use.Effect.StationId);
            }

            Assert.That(healed, Is.True);
            Assert.That(expired, Is.True);
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
