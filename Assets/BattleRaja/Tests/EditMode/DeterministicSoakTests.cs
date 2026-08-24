using System;
using System.Collections.Generic;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    /// <summary>
    /// Real deterministic soak evidence for the offline authority: seeded
    /// matches generate complete input streams, execute them through the
    /// authority, then replay those recorded streams against canonical
    /// per-tick state hashes.
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
            var executor = new DeterministicReplayExecutor();

            for (var match = 0; match < matchCount; match++)
            {
                var seed = (uint)(match * 7919 + 13);
                var replay = CreateSeededReplay(seed);

                ReplayExecutionResult original;
                try
                {
                    original = executor.Execute(replay, false);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"replay seed {seed} threw", exception);
                }
                Assert.That(
                    original.Succeeded,
                    Is.True,
                    $"match {match} (seed {seed}) failed execution: {original.Description}");
                Assert.That(
                    original.ActualHashes,
                    Has.Count.GreaterThan(60),
                    $"match {match} (seed {seed}) ended suspiciously early");

                for (var tick = 0; tick < original.ActualHashes.Count; tick++)
                {
                    replay.TickStateHashes[tick] = original.ActualHashes[tick];
                }

                var verified = executor.Execute(replay);
                Assert.That(
                    verified.Succeeded,
                    Is.True,
                    $"match {match} (seed {seed}) diverged: {verified.Description} at tick {verified.DivergenceTick}");
            }
        }

        private static MatchReplayFile CreateSeededReplay(uint seed)
        {
            const float step = 1f / 30f;
            const int maxTicks = 9300; // full Solo Raja duration plus margin at 30 Hz

            var fighters = new[]
            {
                FighterDefinition.Bijli,
                FighterDefinition.Pehel,
                FighterDefinition.Maya
            };
            var spawns = CreateRingSpawns();
            var pickups = new[]
            {
                new MatchPickupDefinition(0, MatchPickupKind.Health, 35, 20f, new Float2(11f, 0f), 1.2f),
                new MatchPickupDefinition(1, MatchPickupKind.Health, 35, 20f, new Float2(0f, 7.2f), 1.2f),
                new MatchPickupDefinition(2, MatchPickupKind.Health, 35, 20f, new Float2(-11f, 0f), 1.2f)
            };
            var gadgetPickups = new[]
            {
                new GadgetPickupDefinition(0, GadgetDefinition.DholBurst.GadgetId, new Float2(7.78f, 5.3f), 1.3f),
                new GadgetPickupDefinition(1, GadgetDefinition.UmbrellaGuard.GadgetId, new Float2(-8.5f, 7.2f), 1.3f),
                new GadgetPickupDefinition(2, GadgetDefinition.TiffinStation.GadgetId, new Float2(-8.5f, -6.8f), 1.3f)
            };
            var participants = new List<MatchReplayParticipant>(spawns.Count);
            for (var i = 0; i < spawns.Count; i++)
            {
                var fighter = fighters[i % fighters.Length];
                participants.Add(new MatchReplayParticipant(
                    spawns[i].Id,
                    i == 0 ? CombatFaction.Player : CombatFaction.Enemy,
                    fighter.BasicAttack,
                    fighter.Movement,
                    fighter.FighterId,
                    30));
            }

            var header = new MatchReplayHeader(
                "1.0.0-bazaar",
                seed,
                spawns.ToArray(),
                step,
                MatchReplayScenario.SoloRaja,
                participants.ToArray(),
                pickups,
                gadgetPickups);
            var replay = new MatchReplayFile(header);

            var rng = new Random(unchecked((int)seed));
            var sequences = new int[spawns.Count];
            var directions = new Float2[spawns.Count];
            for (var i = 0; i < spawns.Count; i++) directions[i] = new Float2(1f, 0f);

            var gadgetIds = new Dictionary<int, ContentId>
            {
                { 1, GadgetDefinition.DholBurst.GadgetId },
                { 3, GadgetDefinition.UmbrellaGuard.GadgetId },
                { 5, GadgetDefinition.TiffinStation.GadgetId }
            };
            var pehelAbilityId = FighterSpecialDefinition.PehelChargeThrow.AbilityId;
            var mayaAbilityId = FighterSpecialDefinition.MayaDecoy.AbilityId;

            for (var tick = 1; tick <= maxTicks; tick++)
            {
                var phase = CalculatePhase((tick - 1) * step);
                if (phase == MatchPhase.Resolution) break;

                for (var i = 0; i < spawns.Count; i++)
                {
                    if ((tick + i) % 24 == 0 || directions[i].SqrMagnitude < 0.01f)
                    {
                        var angle = (float)(rng.NextDouble() * Math.PI * 2.0);
                        directions[i] = new Float2(MathF.Cos(angle), MathF.Sin(angle));
                    }
                }

                var movements = new MovementCommand[spawns.Count];
                var attacks = new List<AttackCommand>();
                var abilities = new List<MatchReplayAbilityCommand>();
                var gadgets = new List<GadgetUseCommand>();

                for (var i = 0; i < spawns.Count; i++)
                {
                    var actorValue = i + 1;
                    movements[i] = MovementCommandFactory.Create(
                        actorValue,
                        tick,
                        new MovementInputFrame(directions[i], directions[i]),
                        participants[i].Movement);
                }

                if (phase != MatchPhase.LoadWarmup && phase != MatchPhase.SpawnProtection)
                {
                    for (var i = 0; i < spawns.Count; i++)
                    {
                        if (rng.Next(100) >= 6) continue;
                        sequences[i]++;
                        attacks.Add(new AttackCommand(
                            spawns[i].Id,
                            tick,
                            Float2.Zero,
                            directions[i],
                            true,
                            sequences[i]));
                    }

                    for (var i = 0; i < spawns.Count; i++)
                    {
                        var isPehel = participants[i].FighterId.Equals(FighterDefinition.Pehel.FighterId);
                        var isMaya = participants[i].FighterId.Equals(FighterDefinition.Maya.FighterId);
                        var isBijli = participants[i].FighterId.Equals(FighterDefinition.Bijli.FighterId);
                        if (isPehel && rng.Next(100) < 4)
                        {
                            abilities.Add(new MatchReplayAbilityCommand(
                                AbilityCommandFactory.Create(
                                    spawns[i].Id,
                                    tick,
                                    pehelAbilityId,
                                    directions[i],
                                    true),
                                directions[i],
                                directions[i],
                                false,
                                Float2.Zero));
                        }

                        if (isMaya && rng.Next(100) < 4)
                        {
                            abilities.Add(new MatchReplayAbilityCommand(
                                AbilityCommandFactory.Create(
                                    spawns[i].Id,
                                    tick,
                                    mayaAbilityId,
                                    directions[i],
                                    true),
                                directions[i],
                                directions[i],
                                true,
                                spawns[i].Position));
                        }

                        if (isBijli && rng.Next(100) < 4)
                        {
                            abilities.Add(new MatchReplayAbilityCommand(
                                AbilityCommandFactory.Create(
                                    spawns[i].Id,
                                    tick,
                                    FighterDefinition.Bijli.Ability.AbilityId,
                                    directions[i],
                                    true),
                                directions[i],
                                directions[i],
                                false,
                                Float2.Zero));
                        }
                    }

                    foreach (var pair in gadgetIds)
                    {
                        if (rng.Next(100) >= 5) continue;
                        var index = pair.Key;
                        gadgets.Add(new GadgetUseCommand(
                            spawns[index].Id,
                            pair.Value,
                            spawns[index].Position,
                            directions[index],
                            tick));
                    }
                }

                replay.AddFrame(new MatchReplayFrame(
                    tick,
                    movements,
                    attacks.ToArray(),
                    abilities.ToArray(),
                    gadgets.ToArray()), 0UL);

                // Use elapsed-derived phases so command generation does not
                // accumulate floating-point drift across thousands of ticks.
                if (CalculatePhase(tick * step) == MatchPhase.Resolution) break;
            }

            return replay;
        }

        private static MatchPhase CalculatePhase(float elapsedSeconds)
        {
            var phases = OfflineMatchDefinition.SoloRaja.Phases;
            for (var i = 0; i < phases.Length; i++)
            {
                if (elapsedSeconds < phases[i].DurationSeconds) return phases[i].Phase;
                elapsedSeconds -= phases[i].DurationSeconds;
            }

            return MatchPhase.Resolution;
        }

        private static List<MatchSpawn> CreateRingSpawns()
        {
            // Hand-verified unblocked positions under ArenaCollisionDefinition.BazaarBastion.
            return new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(11f, 0f), FighterDefinition.Bijli.MaxHealth),
                new MatchSpawn(new CombatEntityId(2), new Float2(7.78f, 5.3f), FighterDefinition.Pehel.MaxHealth),
                new MatchSpawn(new CombatEntityId(3), new Float2(0f, 7.2f), FighterDefinition.Maya.MaxHealth),
                new MatchSpawn(new CombatEntityId(4), new Float2(-8.5f, 7.2f), FighterDefinition.Bijli.MaxHealth),
                new MatchSpawn(new CombatEntityId(5), new Float2(-11f, 0f), FighterDefinition.Pehel.MaxHealth),
                new MatchSpawn(new CombatEntityId(6), new Float2(-8.5f, -6.8f), FighterDefinition.Maya.MaxHealth),
                new MatchSpawn(new CombatEntityId(7), new Float2(0f, -7.5f), FighterDefinition.Bijli.MaxHealth),
                new MatchSpawn(new CombatEntityId(8), new Float2(8.5f, -6.8f), FighterDefinition.Pehel.MaxHealth)
            };
        }
    }
}
