using System;
using System.Collections.Generic;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    /// <summary>
    /// Real deterministic soak evidence for the offline authority: seeded
    /// accelerated matches run twice; per-tick state hash streams must be
    /// byte-identical between the original and replayed runs.
    ///
    /// Default depth keeps the routine suite fast. Deeper soaks are executed
    /// by setting BATTLERAJA_SOAK_MATCHES and running this fixture via
    /// -testFilter, with the exact command recorded in Docs/QA/.
    /// </summary>
    public sealed class DeterministicSoakTests
    {
        private static int SoakMatchCount =>
            int.TryParse(Environment.GetEnvironmentVariable("BATTLERAJA_SOAK_MATCHES"), out var count) && count > 0
                ? count
                : 4;

        [Test]
        [Timeout(2400000)]
        public void AcceleratedSeededMatchesReproduceIdenticalHashStreams()
        {
            var matchCount = SoakMatchCount;
            var seeds = new uint[matchCount];
            var originals = new ulong[matchCount][];
            for (var match = 0; match < matchCount; match++)
            {
                seeds[match] = (uint)(match * 7919 + 13);
                originals[match] = RunSeededMatch(seeds[match]);
            }

            for (var match = 0; match < matchCount; match++)
            {
                var replayed = RunSeededMatch(seeds[match]);
                Assert.That(
                    replayed.Length,
                    Is.EqualTo(originals[match].Length),
                    $"match {match} (seed {seeds[match]}) produced a different tick count");
                for (var tick = 0; tick < replayed.Length; tick++)
                {
                    Assert.That(
                        replayed[tick],
                        Is.EqualTo(originals[match][tick]),
                        $"match {match} (seed {seeds[match]}) diverged at tick {tick + 1}");
                }
            }
        }

        private static ulong[] RunSeededMatch(uint seed)
        {
            const float step = 1f / 30f;
            const int maxTicks = 9300; // full Solo Raja duration plus margin at 30 Hz

            var fighters = new[]
            {
                FighterDefinition.Bijli,
                FighterDefinition.Pehel,
                FighterDefinition.Maya
            };
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja);
            authority.ConfigureItems(
                new[]
                {
                    new MatchPickupDefinition(0, MatchPickupKind.Health, 35, 20f, new Float2(11f, 0f), 1.2f),
                    new MatchPickupDefinition(1, MatchPickupKind.Health, 35, 20f, new Float2(0f, 7.2f), 1.2f),
                    new MatchPickupDefinition(2, MatchPickupKind.Health, 35, 20f, new Float2(-11f, 0f), 1.2f)
                },
                new[]
                {
                    new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId, new Float2(7.78f, 5.3f), 1.3f),
                    new GadgetPickupDefinition(1, GadgetDefinition.UmbrellaGuard.GadgetId, new Float2(-8.5f, 7.2f), 1.3f),
                    new GadgetPickupDefinition(2, GadgetDefinition.TiffinStation.GadgetId, new Float2(-8.5f, -6.8f), 1.3f)
                });
            authority.Start(CreateRingSpawns());
            for (var i = 1; i <= 8; i++)
            {
                var actorId = new CombatEntityId(i);
                var fighter = fighters[(i - 1) % fighters.Length];
                authority.ConfigureFaction(actorId, i == 1 ? CombatFaction.Player : CombatFaction.Enemy);
                authority.ConfigureWeapon(actorId, fighter.BasicAttack, 30);
                authority.ConfigureMovement(actorId, fighter.Movement);
            }

            // Single monotonic tick stream: commands are simply gated by phase
            // while the authority passes through warmup/spawn protection.
            var rng = new Random(unchecked((int)seed));
            var hashes = new List<ulong>(maxTicks);
            var sequences = new int[8];
            var directions = new Float2[8];
            for (var i = 0; i < 8; i++) directions[i] = new Float2(1f, 0f);
            var gadgetIds = new Dictionary<int, ContentId>
            {
                { 2, GadgetDefinition.DholBurst.GadgetId },
                { 4, GadgetDefinition.UmbrellaGuard.GadgetId },
                { 6, GadgetDefinition.TiffinStation.GadgetId }
            };
            var pehelAbilityId = FighterSpecialDefinition.PehelChargeThrow.AbilityId;

            for (var tick = 1; tick <= maxTicks; tick++)
            {
                for (var i = 1; i <= 8; i++)
                {
                    var actorId = new CombatEntityId(i);
                    if (tick % 24 == i % 24 || directions[i - 1].SqrMagnitude < 0.01f)
                    {
                        var angle = (float)(rng.NextDouble() * Math.PI * 2.0);
                        directions[i - 1] = new Float2(MathF.Cos(angle), MathF.Sin(angle));
                    }

                    if (!authority.Simulation.TryGetSnapshot(actorId, out var snapshot) || !snapshot.Alive) continue;

                    var command = MovementCommandFactory.Create(
                        i,
                        tick,
                        new MovementInputFrame(directions[i - 1], directions[i - 1]),
                        fighters[(i - 1) % fighters.Length].Movement);
                    authority.ResolveMovement(command, step);

                    var phase = authority.CurrentPhase;
                    if (rng.Next(100) < 6 &&
                        phase != MatchPhase.LoadWarmup &&
                        phase != MatchPhase.SpawnProtection &&
                        phase != MatchPhase.Resolution)
                    {
                        sequences[i - 1]++;
                        authority.TryAcceptAttack(new AttackCommand(
                            actorId,
                            tick,
                            Float2.Zero,
                            directions[i - 1],
                            true,
                            sequences[i - 1]));
                    }

                    if (phase != MatchPhase.LoadWarmup &&
                        phase != MatchPhase.SpawnProtection &&
                        phase != MatchPhase.Resolution)
                    {
                        if ((i == 2 || i == 5 || i == 8) && rng.Next(100) < 4)
                        {
                            authority.TryStartPehelCharge(AbilityCommandFactory.Create(
                                actorId,
                                tick,
                                pehelAbilityId,
                                directions[i - 1],
                                true), directions[i - 1], directions[i - 1]);
                        }

                        if (i == 2 || i == 5 || i == 8)
                        {
                            authority.AdvancePehelCharge(actorId, tick, step, FighterSpecialDefinition.PehelChargeThrow.Magnitude);
                        }

                        if ((i == 3 || i == 6) && rng.Next(100) < 4)
                        {
                            authority.TrySpawnMayaDecoy(actorId, tick, snapshot.Position);
                        }

                        if (gadgetIds.TryGetValue(i, out var gadgetId) && rng.Next(100) < 5)
                        {
                            authority.TryUseGadget(new GadgetUseCommand(
                                actorId,
                                gadgetId,
                                snapshot.Position,
                                directions[i - 1],
                                tick));
                        }
                    }
                }

                var result = authority.Advance(tick, step);
                hashes.Add(DeterministicReplayHasher.CalculateTickHash(
                    authority,
                    result,
                    authority.Simulation.GetSnapshots()));

                if (authority.CurrentPhase == MatchPhase.Resolution) break;
            }

            Assert.That(hashes.Count, Is.GreaterThan(60), "soak match ended suspiciously early");
            return hashes.ToArray();
        }

        private static List<MatchSpawn> CreateRingSpawns()
        {
            // Hand-verified unblocked positions under ArenaCollisionDefinition.BazaarBastion.
            return new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(11f, 0f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(7.78f, 5.3f), 100),
                new MatchSpawn(new CombatEntityId(3), new Float2(0f, 7.2f), 100),
                new MatchSpawn(new CombatEntityId(4), new Float2(-8.5f, 7.2f), 100),
                new MatchSpawn(new CombatEntityId(5), new Float2(-11f, 0f), 100),
                new MatchSpawn(new CombatEntityId(6), new Float2(-8.5f, -6.8f), 100),
                new MatchSpawn(new CombatEntityId(7), new Float2(0f, -7.5f), 100),
                new MatchSpawn(new CombatEntityId(8), new Float2(8.5f, -6.8f), 100)
            };
        }
    }
}
