using System;
using System.Collections.Generic;
using System.IO;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    [TestFixture]
    public class ReplayDeterminismTests
    {
        [Test]
        public void ReplayHasher_ProducesIdenticalHash_ForSameState()
        {
            var pos = new Float2(10f, 20f);
            var snapshot1 = new MatchParticipantSnapshot(new CombatEntityId(1), pos, 100, 100, true, 1, 0, 0, 0, 0f);
            var snapshot2 = new MatchParticipantSnapshot(new CombatEntityId(1), pos, 100, 100, true, 1, 0, 0, 0, 0f);

            var hash1 = DeterministicReplayHasher.CalculateTickHash(0, MatchPhase.Opening, Float2.Zero, 30f, new[] { snapshot1 }, null);
            var hash2 = DeterministicReplayHasher.CalculateTickHash(0, MatchPhase.Opening, Float2.Zero, 30f, new[] { snapshot2 }, null);

            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void MatchReplayFile_RecordsFrameAndDetectsStateParity()
        {
            var p1 = new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100);
            var header = new MatchReplayHeader("1.0.0-bazaar", 12345, new[] { p1 });
            var replay = new MatchReplayFile(header);

            var snapshot = new MatchParticipantSnapshot(new CombatEntityId(1), Float2.Zero, 100, 100, true, 1, 0, 0, 0, 0f);
            var hash = DeterministicReplayHasher.CalculateTickHash(0, MatchPhase.Opening, Float2.Zero, 30f, new[] { snapshot }, null);

            var frame = new MatchReplayFrame(0, null, null, null);
            replay.AddFrame(frame, hash);

            Assert.AreEqual(1, replay.Frames.Count);
            Assert.AreEqual(hash, replay.TickStateHashes[0]);
        }

        [Test]
        public void ExtendedAuthorityHash_DetectsCanonicalGadgetState()
        {
            var spawns = new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100)
            };

            var baseline = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            baseline.Start(spawns);
            MatchAuthorityTick baselineTick = default;
            for (var warmupTick = 1; warmupTick <= 241; warmupTick++) baselineTick = baseline.Advance(warmupTick, 1f / 30f);
            var baselineHash = DeterministicReplayHasher.CalculateTickHash(
                baseline,
                baselineTick,
                baseline.Simulation.GetSnapshots());

            var gadgetId = GadgetDefinition.TiffinStation.GadgetId;
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                null,
                new[] { new GadgetPickupDefinition(0, gadgetId, Float2.Zero, 1.3f) });
            authority.Start(spawns);
            MatchAuthorityTick collectionTick = default;
            MatchAuthorityTick collectTick = default;
            for (var tick = 1; tick <= 241; tick++)
            {
                var advanced = authority.Advance(tick, 1f / 30f);
                if (advanced.GadgetCollections.Length > 0) collectTick = advanced;
                collectionTick = advanced;
            }

            Assert.That(collectTick.GadgetCollections, Has.Length.EqualTo(1));
            Assert.That(collectTick.GadgetCollections[0].CollectionEventId, Is.EqualTo(1));

            var used = authority.TryUseGadget(new GadgetUseCommand(
                new CombatEntityId(1),
                gadgetId,
                Float2.Zero,
                new Float2(0f, 1f),
                242));
            Assert.That(used.Used, Is.True, $"failure={used.Failure} phase={authority.CurrentPhase}");
            Assert.That(
                used.Failure,
                Is.EqualTo(GadgetUseFailure.None),
                $"failure={used.Failure} phase={authority.CurrentPhase}");
            Assert.That(authority.TryAcquireGadget(new CombatEntityId(1), gadgetId), Is.True, "replacement pickup");
            Assert.That(used.EventId, Is.EqualTo(1));

            var authorityHash = DeterministicReplayHasher.CalculateTickHash(
                authority,
                collectionTick,
                authority.Simulation.GetSnapshots());

            Assert.That(authorityHash, Is.Not.EqualTo(baselineHash));
        }

        [Test]
        public void ArenaCollisionHashDetectsGeometryAndVersionChanges()
        {
            var baseline = new ArenaCollisionDefinition(
                new Float2(-13f, -9f),
                new Float2(13f, 9f),
                0.45f,
                new[] { new ArenaObstacle(1, new Float2(-1f, -1f), new Float2(1f, 1f)) },
                "arena-v1");
            var movedObstacle = new ArenaCollisionDefinition(
                new Float2(-13f, -9f),
                new Float2(13f, 9f),
                0.45f,
                new[] { new ArenaObstacle(1, new Float2(-0.5f, -1f), new Float2(1.5f, 1f)) },
                "arena-v1");
            var changedVersion = new ArenaCollisionDefinition(
                new Float2(-13f, -9f),
                new Float2(13f, 9f),
                0.45f,
                new[] { new ArenaObstacle(1, new Float2(-1f, -1f), new Float2(1f, 1f)) },
                "arena-v2");

            Assert.That(movedObstacle.CalculateStableHash(), Is.Not.EqualTo(baseline.CalculateStableHash()));
            Assert.That(changedVersion.CalculateStableHash(), Is.Not.EqualTo(baseline.CalculateStableHash()));
        }

        [Test]
        public void AuthorityHashDetectsSortedDamageContributionState()
        {
            var spawns = new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), Float2.Zero, 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(3f, 0f), 100),
                new MatchSpawn(new CombatEntityId(3), new Float2(6f, 0f), 100)
            };
            var baseline = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            baseline.Start(spawns);
            var baselineTick = baseline.Advance(1, 1f / 30f);
            var baselineHash = DeterministicReplayHasher.CalculateTickHash(
                baseline,
                baselineTick,
                baseline.Simulation.GetSnapshots());

            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.Start(spawns);
            var request = new DamageRequest(
                new CombatEntityId(2),
                new CombatEntityId(3),
                CombatFaction.Enemy,
                10,
                DamageType.Projectile,
                Float2.Up,
                1);
            authority.RecordDamage(new CombatDamageEvent(request, request.RawAmount, false, 90, 1));
            var authorityTick = authority.Advance(1, 1f / 30f);
            var authorityHash = DeterministicReplayHasher.CalculateTickHash(
                authority,
                authorityTick,
                authority.Simulation.GetSnapshots());

            Assert.That(authority.Simulation.GetDamageContributions(), Has.Length.EqualTo(1));
            Assert.That(authorityHash, Is.Not.EqualTo(baselineHash));
        }

        [Test]
        public void ReplayExecutor_ReproducesCompleteAuthorityHashStream()
        {
            var header = CreateReplayHeader();
            var frames = CreateReplayFrames();
            var seedReplay = new MatchReplayFile(header);
            foreach (var frame in frames) seedReplay.AddFrame(frame, 0UL);

            var seeded = new DeterministicReplayExecutor().Execute(seedReplay, false);
            Assert.That(seeded.Succeeded, Is.True, $"seed: {seeded.Description} tick={seeded.DivergenceTick}");
            Assert.That(seeded.ActualHashes, Has.Count.EqualTo(frames.Count));

            var recorded = new MatchReplayFile(header);
            for (var i = 0; i < frames.Count; i++) recorded.AddFrame(frames[i], seeded.ActualHashes[i]);
            var replayed = new DeterministicReplayExecutor().Execute(recorded);
            Assert.That(replayed.Succeeded, Is.True, $"replay: {replayed.Description} tick={replayed.DivergenceTick}");
            Assert.That(replayed.ActualHashes, Is.EqualTo(seeded.ActualHashes));

            recorded.TickStateHashes[7]++;
            var corrupted = new DeterministicReplayExecutor().Execute(recorded);
            Assert.That(corrupted.Succeeded, Is.False);
            Assert.That(corrupted.DivergenceTick, Is.EqualTo(8));
        }

        [Test]
        public void MatchReplayFileSerializer_RoundTripsDeterministicallyAndRejectsCorruption()
        {
            var seedReplay = new MatchReplayFile(CreateReplayHeader());
            var frames = CreateReplayFrames();
            foreach (var frame in frames) seedReplay.AddFrame(frame, 0UL);

            var executed = new DeterministicReplayExecutor().Execute(seedReplay, false);
            Assert.That(executed.Succeeded, Is.True);
            var recorded = new MatchReplayFile(seedReplay.Header);
            for (var i = 0; i < frames.Count; i++) recorded.AddFrame(frames[i], executed.ActualHashes[i]);

            var bytes = MatchReplayFileSerializer.Serialize(recorded);
            var roundTripped = MatchReplayFileSerializer.Deserialize(bytes);
            var roundTripBytes = MatchReplayFileSerializer.Serialize(roundTripped);

            CollectionAssert.AreEqual(bytes, roundTripBytes);
            Assert.That(roundTripped.Header.ArenaVersion, Is.EqualTo(recorded.Header.ArenaVersion));
            Assert.That(roundTripped.Header.MatchSeed, Is.EqualTo(recorded.Header.MatchSeed));
            Assert.That(roundTripped.Header.Participants.Length, Is.EqualTo(recorded.Header.Participants.Length));
            Assert.That(roundTripped.Frames.Count, Is.EqualTo(recorded.Frames.Count));
            Assert.That(new DeterministicReplayExecutor().Execute(roundTripped).Succeeded, Is.True);

            var path = Path.Combine(Path.GetTempPath(), "battleraja-replay-" + Guid.NewGuid().ToString("N") + ".brr");
            try
            {
                MatchReplayFileSerializer.Write(recorded, path);
                var fromDisk = MatchReplayFileSerializer.Read(path);
                CollectionAssert.AreEqual(bytes, MatchReplayFileSerializer.Serialize(fromDisk));
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }

            var productionPath = Environment.GetEnvironmentVariable("BATTLERAJA_REPLAY_PATH");
            if (!string.IsNullOrWhiteSpace(productionPath))
            {
                Assert.That(File.Exists(productionPath), Is.True, "Configured production replay path does not exist.");
                var productionReplay = MatchReplayFileSerializer.Read(productionPath);
                var productionExecution = new DeterministicReplayExecutor().Execute(productionReplay);
                Assert.That(productionExecution.Succeeded, Is.True,
                    $"Production replay diverged at tick {productionExecution.DivergenceTick}: {productionExecution.Description}");
                Assert.That(productionReplay.Frames.Count, Is.GreaterThan(0));
            }

            var corrupted = (byte[])bytes.Clone();
            corrupted[corrupted.Length - 1] ^= 0xFF;
            Assert.Throws<InvalidDataException>(() => MatchReplayFileSerializer.Deserialize(corrupted));
        }

        private static MatchReplayHeader CreateReplayHeader()
        {
            var bijli = FighterDefinition.Bijli;
            var pehel = FighterDefinition.Pehel;
            var maya = FighterDefinition.Maya;
            return new MatchReplayHeader(
                "1.0.0-bazaar",
                12345,
                new[]
                {
                    new MatchSpawn(new CombatEntityId(1), Float2.Zero, bijli.MaxHealth),
                    new MatchSpawn(new CombatEntityId(2), new Float2(4f, 0f), pehel.MaxHealth),
                    new MatchSpawn(new CombatEntityId(3), new Float2(-4f, 0f), maya.MaxHealth)
                },
                1f / 30f,
                MatchReplayScenario.SoloRaja,
                new[]
                {
                    new MatchReplayParticipant(new CombatEntityId(1), CombatFaction.Player, bijli.BasicAttack, bijli.Movement, bijli.FighterId, 30),
                    new MatchReplayParticipant(new CombatEntityId(2), CombatFaction.Enemy, pehel.BasicAttack, pehel.Movement, pehel.FighterId, 30),
                    new MatchReplayParticipant(new CombatEntityId(3), CombatFaction.Enemy, maya.BasicAttack, maya.Movement, maya.FighterId, 30)
                },
                new[] { new MatchPickupDefinition(0, MatchPickupKind.Health, 25, 12f, Float2.Zero, 1.2f) },
                new[] { new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId, Float2.Zero, 1.3f) });
        }

        private static List<MatchReplayFrame> CreateReplayFrames()
        {
            var frames = new List<MatchReplayFrame>();
            var bijliId = new CombatEntityId(1);
            var pehelId = new CombatEntityId(2);
            var mayaId = new CombatEntityId(3);
            var right = new Float2(1f, 0f);
            var left = new Float2(-1f, 0f);
            var up = Float2.Up;

            for (var tick = 1; tick <= 20; tick++)
            {
                var movements = new[]
                {
                    new MovementCommand(bijliId.Value, tick, right, right),
                    new MovementCommand(pehelId.Value, tick, left, left),
                    new MovementCommand(mayaId.Value, tick, up, up)
                };
                var attacks = tick % 7 == 0
                    ? new[]
                    {
                        new AttackCommand(bijliId, tick, Float2.Zero, right, true, tick / 7),
                        new AttackCommand(pehelId, tick, Float2.Zero, left, true, tick / 7)
                    }
                    : null;
                var abilities = tick == 9
                    ? new[]
                    {
                        new MatchReplayAbilityCommand(
                            AbilityCommandFactory.Create(bijliId, tick, FighterDefinition.Bijli.Ability.AbilityId, right, true),
                            right,
                            right,
                            false,
                            Float2.Zero),
                        new MatchReplayAbilityCommand(
                            AbilityCommandFactory.Create(pehelId, tick, FighterSpecialDefinition.PehelChargeThrow.AbilityId, left, true),
                            left,
                            left,
                            false,
                            Float2.Zero),
                        new MatchReplayAbilityCommand(
                            AbilityCommandFactory.Create(mayaId, tick, FighterSpecialDefinition.MayaDecoy.AbilityId, up, true),
                            up,
                            up,
                            true,
                            new Float2(-4f, 0f))
                    }
                    : null;
                var gadgets = tick == 4
                    ? new[] { new GadgetUseCommand(bijliId, GadgetDefinition.DholBurst.GadgetId, Float2.Zero, right, tick) }
                    : null;

                frames.Add(new MatchReplayFrame(tick, movements, attacks, abilities, gadgets));
            }

            return frames;
        }
    }
}
