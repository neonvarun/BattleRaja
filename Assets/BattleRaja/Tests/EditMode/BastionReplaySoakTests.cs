using System;
using System.Collections.Generic;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    /// <summary>
    /// Exercises the eight-actor Bastion replay path for a real-time match
    /// horizon. The test records the complete combined authority/team digest,
    /// serializes the v2 envelope, and executes the exact stream again.
    /// </summary>
    public sealed class BastionReplaySoakTests
    {
        private static int MatchCount => ReadPositiveEnvironment("BATTLERAJA_BASTION_SOAK_MATCHES", 2, 8);
        private static int TickCount => ReadPositiveEnvironment("BATTLERAJA_BASTION_SOAK_TICKS", 8400, 12000);

        [Test]
        [Timeout(1200000)]
        public void BastionReplay_ReproducesEightActorCombinedHashStream()
        {
            var executor = new DeterministicReplayExecutor();
            for (var matchIndex = 0; matchIndex < MatchCount; matchIndex++)
            {
                var seed = (uint)(3 + matchIndex * 7919);
                var replay = CreateReplay(seed, TickCount);
                var original = executor.Execute(replay, false);
                Assert.That(original.Succeeded, Is.True,
                    $"seed {seed} failed initial execution: {original.Description} at tick {original.DivergenceTick}");
                Assert.That(original.ActualHashes, Has.Count.EqualTo(replay.Frames.Count));
                Assert.That(original.ActualHashes, Has.Count.GreaterThan(6000),
                    $"seed {seed} did not cover a meaningful real-time Bastion horizon");

                for (var i = 0; i < original.ActualHashes.Count; i++)
                {
                    replay.TickStateHashes[i] = original.ActualHashes[i];
                }

                var encoded = MatchReplayFileSerializer.Serialize(replay);
                var decoded = MatchReplayFileSerializer.Deserialize(encoded);
                Assert.That(decoded.Header.Scenario, Is.EqualTo(MatchReplayScenario.BastionCrown));
                Assert.That(decoded.Header.IncludesBastionState, Is.True);

                var verified = executor.Execute(decoded);
                Assert.That(verified.Succeeded, Is.True,
                    $"seed {seed} diverged: {verified.Description} at tick {verified.DivergenceTick}");
                CollectionAssert.AreEqual(original.ActualHashes, verified.ActualHashes,
                    $"seed {seed} changed its combined authority/team hash stream after serialization");
            }
        }

        private static MatchReplayFile CreateReplay(uint seed, int ticks)
        {
            const float step = 1f / 30f;
            var spawns = CreateSpawns();
            var fighters = new[]
            {
                FighterDefinition.Bijli,
                FighterDefinition.Pehel,
                FighterDefinition.Maya,
                FighterDefinition.Bijli,
                FighterDefinition.Pehel,
                FighterDefinition.Maya,
                FighterDefinition.Bijli,
                FighterDefinition.Pehel
            };
            var participants = new MatchReplayParticipant[spawns.Count];
            for (var i = 0; i < participants.Length; i++)
            {
                var fighter = fighters[i];
                participants[i] = new MatchReplayParticipant(
                    spawns[i].Id,
                    i == 0 ? CombatFaction.Player : CombatFaction.Enemy,
                    fighter.BasicAttack,
                    fighter.Movement,
                    fighter.FighterId,
                    30);
            }

            var gadgetPickups = new[]
            {
                new GadgetPickupDefinition(0, GadgetDefinition.TiffinStation.GadgetId, spawns[0].Position, 1.3f),
                new GadgetPickupDefinition(1, GadgetDefinition.UmbrellaGuard.GadgetId, spawns[1].Position, 1.3f),
                new GadgetPickupDefinition(2, GadgetDefinition.DholBurst.GadgetId, spawns[2].Position, 1.3f)
            };
            var header = new MatchReplayHeader(
                "1.0.0-bazaar",
                seed,
                spawns.ToArray(),
                step,
                MatchReplayScenario.BastionCrown,
                participants,
                Array.Empty<MatchPickupDefinition>(),
                gadgetPickups,
                true);
            var replay = new MatchReplayFile(header);
            var attackSequences = new int[participants.Length];
            var crownDirection = new Float2(-0.82f, 0.57f);

            for (var tick = 1; tick <= ticks; tick++)
            {
                var movement = new MovementCommand[participants.Length];
                for (var i = 0; i < movement.Length; i++)
                {
                    var direction = Float2.Zero;
                    if (i == 0 && tick >= 101 && tick <= 170) direction = crownDirection;
                    movement[i] = new MovementCommand(i + 1, tick, direction, direction);
                }

                var attacks = new List<AttackCommand>(2);
                if (tick >= 150 && tick % 30 == 0)
                {
                    attacks.Add(CreateAttack(spawns[0].Id, tick, Float2.Up, ++attackSequences[0]));
                    attacks.Add(CreateAttack(spawns[4].Id, tick, new Float2(0f, -1f), ++attackSequences[4]));
                }

                var abilities = new List<MatchReplayAbilityCommand>(3);
                if (tick == 180)
                {
                    abilities.Add(new MatchReplayAbilityCommand(
                        AbilityCommandFactory.Create(
                            spawns[1].Id,
                            tick,
                            FighterSpecialDefinition.PehelChargeThrow.AbilityId,
                            new Float2(1f, 0f),
                            true),
                        new Float2(1f, 0f),
                        new Float2(1f, 0f),
                        false,
                        Float2.Zero));
                    abilities.Add(new MatchReplayAbilityCommand(
                        AbilityCommandFactory.Create(
                            spawns[2].Id,
                            tick,
                            FighterSpecialDefinition.MayaDecoy.AbilityId,
                            new Float2(-1f, 0f),
                            true),
                        new Float2(-1f, 0f),
                        new Float2(-1f, 0f),
                        true,
                        spawns[2].Position));
                }

                if (tick == 210)
                {
                    abilities.Add(new MatchReplayAbilityCommand(
                        AbilityCommandFactory.Create(
                            spawns[0].Id,
                            tick,
                            FighterDefinition.Bijli.Ability.AbilityId,
                            Float2.Up,
                            true),
                        Float2.Up,
                        Float2.Up,
                        false,
                        Float2.Zero));
                }

                var gadgets = Array.Empty<GadgetUseCommand>();
                if (tick == 150)
                {
                    gadgets = new[]
                    {
                        new GadgetUseCommand(
                            spawns[0].Id,
                            GadgetDefinition.TiffinStation.GadgetId,
                            spawns[0].Position,
                            Float2.Up,
                            tick)
                    };
                }
                else if (tick == 180)
                {
                    gadgets = new[]
                    {
                        new GadgetUseCommand(
                            spawns[1].Id,
                            GadgetDefinition.UmbrellaGuard.GadgetId,
                            spawns[1].Position,
                            new Float2(1f, 0f),
                            tick)
                    };
                }
                else if (tick == 210)
                {
                    gadgets = new[]
                    {
                        new GadgetUseCommand(
                            spawns[2].Id,
                            GadgetDefinition.DholBurst.GadgetId,
                            spawns[2].Position,
                            new Float2(-1f, 0f),
                            tick)
                    };
                }

                replay.AddFrame(new MatchReplayFrame(
                    tick,
                    movement,
                    attacks.ToArray(),
                    abilities.ToArray(),
                    gadgets),
                    0UL);
            }

            return replay;
        }

        private static AttackCommand CreateAttack(CombatEntityId actorId, int tick, Float2 direction, int sequence)
        {
            return new AttackCommand(actorId, tick, Float2.Zero, direction, true, sequence);
        }

        private static List<MatchSpawn> CreateSpawns()
        {
            return new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(0f, -7f), FighterDefinition.Bijli.MaxHealth),
                new MatchSpawn(new CombatEntityId(2), new Float2(-4f, -7f), FighterDefinition.Pehel.MaxHealth),
                new MatchSpawn(new CombatEntityId(3), new Float2(-8f, -7f), FighterDefinition.Maya.MaxHealth),
                new MatchSpawn(new CombatEntityId(4), new Float2(-12f, -7f), FighterDefinition.Bijli.MaxHealth),
                new MatchSpawn(new CombatEntityId(5), new Float2(0f, 7f), FighterDefinition.Pehel.MaxHealth),
                new MatchSpawn(new CombatEntityId(6), new Float2(4f, 7f), FighterDefinition.Maya.MaxHealth),
                new MatchSpawn(new CombatEntityId(7), new Float2(8f, 7f), FighterDefinition.Bijli.MaxHealth),
                new MatchSpawn(new CombatEntityId(8), new Float2(12f, 7f), FighterDefinition.Pehel.MaxHealth)
            };
        }

        private static int ReadPositiveEnvironment(string name, int fallback, int maximum)
        {
            if (!int.TryParse(Environment.GetEnvironmentVariable(name), out var value) || value <= 0)
            {
                return fallback;
            }

            return Math.Min(value, maximum);
        }
    }
}
