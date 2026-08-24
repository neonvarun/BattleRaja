using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public enum MatchReplayScenario
    {
        SoloRaja = 0
    }

    public readonly struct MatchReplayParticipant
    {
        public MatchReplayParticipant(
            CombatEntityId actorId,
            CombatFaction faction,
            ProjectileWeaponDefinition weapon,
            MovementTuning movement,
            ContentId fighterId,
            int tickRate)
        {
            if (actorId.Value <= 0) throw new ArgumentOutOfRangeException(nameof(actorId), "Actor IDs must be positive.");
            if (tickRate <= 0) throw new ArgumentOutOfRangeException(nameof(tickRate), "Tick rates must be positive.");
            if (!weapon.IsValid(out var weaponReason)) throw new ArgumentException(weaponReason, nameof(weapon));
            if (!fighterId.IsValid || fighterId.Kind != ContentIdKind.Fighter)
            {
                throw new ArgumentException("A valid fighter content ID is required.", nameof(fighterId));
            }

            ActorId = actorId;
            Faction = faction;
            Weapon = weapon;
            Movement = movement;
            FighterId = fighterId;
            TickRate = tickRate;
        }

        public CombatEntityId ActorId { get; }
        public CombatFaction Faction { get; }
        public ProjectileWeaponDefinition Weapon { get; }
        public MovementTuning Movement { get; }
        public ContentId FighterId { get; }
        public int TickRate { get; }
    }

    public readonly struct MatchReplayAbilityCommand
    {
        public MatchReplayAbilityCommand(
            AbilityCommand command,
            Float2 movement,
            Float2 facing,
            bool spawnDecoy,
            Float2 decoyPosition)
        {
            Command = command;
            Movement = movement;
            Facing = facing;
            SpawnDecoy = spawnDecoy;
            DecoyPosition = decoyPosition;
        }

        public AbilityCommand Command { get; }
        public Float2 Movement { get; }
        public Float2 Facing { get; }
        public bool SpawnDecoy { get; }
        public Float2 DecoyPosition { get; }
    }

    public readonly struct MatchReplayHeader
    {
        public MatchReplayHeader(string arenaVersion, uint matchSeed, MatchSpawn[] spawns)
            : this(
                arenaVersion,
                matchSeed,
                spawns,
                1f / 30f,
                MatchReplayScenario.SoloRaja,
                null,
                null,
                null)
        {
        }

        public MatchReplayHeader(
            string arenaVersion,
            uint matchSeed,
            MatchSpawn[] spawns,
            float fixedDeltaSeconds,
            MatchReplayScenario scenario,
            MatchReplayParticipant[] participants,
            MatchPickupDefinition[] pickups,
            GadgetPickupDefinition[] gadgetPickups)
        {
            if (fixedDeltaSeconds <= 0f || float.IsNaN(fixedDeltaSeconds) || float.IsInfinity(fixedDeltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(fixedDeltaSeconds), "Fixed delta seconds must be finite and positive.");
            }

            ArenaVersion = arenaVersion ?? "1.0.0-bazaar";
            MatchSeed = matchSeed;
            Spawns = spawns ?? Array.Empty<MatchSpawn>();
            FixedDeltaSeconds = fixedDeltaSeconds;
            Scenario = scenario;
            Participants = participants ?? Array.Empty<MatchReplayParticipant>();
            Pickups = pickups ?? Array.Empty<MatchPickupDefinition>();
            GadgetPickups = gadgetPickups ?? Array.Empty<GadgetPickupDefinition>();
        }

        public string ArenaVersion { get; }
        public uint MatchSeed { get; }
        public MatchSpawn[] Spawns { get; }
        public float FixedDeltaSeconds { get; }
        public MatchReplayScenario Scenario { get; }
        public MatchReplayParticipant[] Participants { get; }
        public MatchPickupDefinition[] Pickups { get; }
        public GadgetPickupDefinition[] GadgetPickups { get; }
    }

    public readonly struct MatchReplayFrame
    {
        public MatchReplayFrame(
            int simulationTick,
            AttackCommand[] attackCommands,
            AbilityCommand[] abilityCommands,
            GadgetUseCommand[] gadgetCommands)
            : this(simulationTick, null, attackCommands, ConvertLegacyAbilities(abilityCommands), gadgetCommands)
        {
        }

        public MatchReplayFrame(
            int simulationTick,
            MovementCommand[] movementCommands,
            AttackCommand[] attackCommands,
            MatchReplayAbilityCommand[] abilityCommands,
            GadgetUseCommand[] gadgetCommands)
        {
            SimulationTick = simulationTick;
            MovementCommands = movementCommands ?? Array.Empty<MovementCommand>();
            AttackCommands = attackCommands ?? Array.Empty<AttackCommand>();
            AbilityCommands = abilityCommands ?? Array.Empty<MatchReplayAbilityCommand>();
            GadgetCommands = gadgetCommands ?? Array.Empty<GadgetUseCommand>();
        }

        public int SimulationTick { get; }
        public MovementCommand[] MovementCommands { get; }
        public AttackCommand[] AttackCommands { get; }
        public MatchReplayAbilityCommand[] AbilityCommands { get; }
        public GadgetUseCommand[] GadgetCommands { get; }

        private static MatchReplayAbilityCommand[] ConvertLegacyAbilities(AbilityCommand[] commands)
        {
            if (commands == null || commands.Length == 0) return Array.Empty<MatchReplayAbilityCommand>();
            var converted = new MatchReplayAbilityCommand[commands.Length];
            for (var i = 0; i < commands.Length; i++)
            {
                converted[i] = new MatchReplayAbilityCommand(commands[i], Float2.Zero, Float2.Up, false, Float2.Zero);
            }

            return converted;
        }
    }

    public sealed class MatchReplayFile
    {
        public MatchReplayFile(MatchReplayHeader header)
        {
            Header = header;
            Frames = new List<MatchReplayFrame>();
            TickStateHashes = new List<ulong>();
        }

        public MatchReplayHeader Header { get; }
        public List<MatchReplayFrame> Frames { get; }
        public List<ulong> TickStateHashes { get; }

        public void AddFrame(MatchReplayFrame frame, ulong stateHash)
        {
            Frames.Add(frame);
            TickStateHashes.Add(stateHash);
        }
    }

    public static class DeterministicReplayHasher
    {
        public static ulong CalculateTickHash(
            OfflineMatchAuthority authority,
            MatchAuthorityTick tick,
            MatchParticipantSnapshot[] snapshots)
        {
            if (authority == null) throw new ArgumentNullException(nameof(authority));
            return authority.CalculateDeterministicTickHash(tick, snapshots);
        }

        public static ulong CalculateTickHash(
            int simulationTick,
            MatchPhase phase,
            Float2 zoneCenter,
            float zoneRadius,
            MatchParticipantSnapshot[] snapshots,
            DomainProjectileSnapshot[] projectileSnapshots)
        {
            const ulong FnvOffsetBasis = 14695981039346656037UL;
            const ulong FnvPrime = 1099511628211UL;

            ulong hash = FnvOffsetBasis;

            void CombineInt(int val)
            {
                unchecked
                {
                    hash ^= (ulong)(val & 0xFF);
                    hash *= FnvPrime;
                    hash ^= (ulong)((val >> 8) & 0xFF);
                    hash *= FnvPrime;
                    hash ^= (ulong)((val >> 16) & 0xFF);
                    hash *= FnvPrime;
                    hash ^= (ulong)((val >> 24) & 0xFF);
                    hash *= FnvPrime;
                }
            }

            CombineInt(simulationTick);
            CombineInt((int)phase);
            CombineInt((int)(zoneCenter.X * 1000f));
            CombineInt((int)(zoneCenter.Y * 1000f));
            CombineInt((int)(zoneRadius * 1000f));

            if (snapshots != null)
            {
                for (var i = 0; i < snapshots.Length; i++)
                {
                    var s = snapshots[i];
                    CombineInt(s.Id.Value);
                    CombineInt(s.Alive ? 1 : 0);
                    CombineInt(s.CurrentHealth);
                    CombineInt((int)(s.Position.X * 1000f));
                    CombineInt((int)(s.Position.Y * 1000f));
                }
            }

            if (projectileSnapshots != null)
            {
                for (var i = 0; i < projectileSnapshots.Length; i++)
                {
                    var p = projectileSnapshots[i];
                    CombineInt(p.ProjectileId);
                    CombineInt(p.InstigatorId.Value);
                    CombineInt((int)(p.Position.X * 1000f));
                    CombineInt((int)(p.Position.Y * 1000f));
                }
            }

            return hash;
        }
    }

    public sealed class ReplayDivergenceReport
    {
        public ReplayDivergenceReport(bool diverged, int divergenceTick, ulong expectedHash, ulong actualHash, string description)
        {
            Diverged = diverged;
            DivergenceTick = divergenceTick;
            ExpectedHash = expectedHash;
            ActualHash = actualHash;
            Description = description;
        }

        public bool Diverged { get; }
        public int DivergenceTick { get; }
        public ulong ExpectedHash { get; }
        public ulong ActualHash { get; }
        public string Description { get; }
    }

    public sealed class DeterministicReplayExecutor
    {
        public ReplayExecutionResult Execute(MatchReplayFile replay, bool verifyRecordedHashes = true)
        {
            if (replay == null) throw new ArgumentNullException(nameof(replay));
            var header = replay.Header;
            if (header.Participants.Length != header.Spawns.Length)
            {
                throw new InvalidOperationException("Replay headers must configure every spawned participant.");
            }

            var authority = CreateAuthority(header);
            var pehelActorIds = new List<CombatEntityId>();
            var bijliActorIds = new List<CombatEntityId>();
            for (var i = 0; i < header.Participants.Length; i++)
            {
                if (header.Participants[i].FighterId.Equals(FighterDefinition.Pehel.FighterId))
                {
                    pehelActorIds.Add(header.Participants[i].ActorId);
                }

                if (header.Participants[i].FighterId.Equals(FighterDefinition.Bijli.FighterId))
                {
                    bijliActorIds.Add(header.Participants[i].ActorId);
                }
            }

            var actualHashes = new List<ulong>(replay.Frames.Count);
            for (var frameIndex = 0; frameIndex < replay.Frames.Count; frameIndex++)
            {
                var frame = replay.Frames[frameIndex];
                if (frame.SimulationTick != frameIndex + 1)
                {
                    throw new InvalidOperationException($"Replay tick {frame.SimulationTick} is not contiguous at frame {frameIndex}.");
                }

                ApplyFrame(authority, frame, header.FixedDeltaSeconds, pehelActorIds, bijliActorIds);
                var tick = authority.Advance(frame.SimulationTick, header.FixedDeltaSeconds);
                var snapshots = authority.Simulation.GetSnapshots();
                var hash = DeterministicReplayHasher.CalculateTickHash(authority, tick, snapshots);
                actualHashes.Add(hash);

                if (verifyRecordedHashes &&
                    frameIndex < replay.TickStateHashes.Count &&
                    replay.TickStateHashes[frameIndex] != hash)
                {
                    return new ReplayExecutionResult(
                        false,
                        frame.SimulationTick,
                        replay.TickStateHashes[frameIndex],
                        hash,
                        "Canonical authority state diverged.",
                        actualHashes,
                        authority);
                }
            }

            if (verifyRecordedHashes && replay.TickStateHashes.Count != actualHashes.Count)
            {
                return new ReplayExecutionResult(
                    false,
                    actualHashes.Count,
                    0,
                    0,
                    "Recorded and executed hash streams have different lengths.",
                    actualHashes,
                    authority);
            }

            return new ReplayExecutionResult(true, 0, 0, 0, string.Empty, actualHashes, authority);
        }

        private static OfflineMatchAuthority CreateAuthority(MatchReplayHeader header)
        {
            OfflineMatchDefinition definition;
            switch (header.Scenario)
            {
                case MatchReplayScenario.SoloRaja:
                    definition = OfflineMatchDefinition.SoloRaja;
                    break;
                default:
                    throw new InvalidOperationException("Unsupported replay scenario.");
            }

            var authority = new OfflineMatchAuthority(definition);
            authority.ConfigureItems(header.Pickups, header.GadgetPickups);
            authority.Start(header.Spawns);
            for (var i = 0; i < header.Participants.Length; i++)
            {
                var participant = header.Participants[i];
                authority.ConfigureFaction(participant.ActorId, participant.Faction);
                // Solo Raja replays use one combatant group per actor. The stored
                // faction remains a presentation label and does not create teams.
                authority.ConfigureCombatGroup(participant.ActorId, participant.ActorId.Value);
                authority.ConfigureWeapon(participant.ActorId, participant.Weapon, participant.TickRate);
                authority.ConfigureMovement(participant.ActorId, participant.Movement);
            }

            return authority;
        }

        private static void ApplyFrame(
            OfflineMatchAuthority authority,
            MatchReplayFrame frame,
            float fixedDeltaSeconds,
            List<CombatEntityId> pehelActorIds,
            List<CombatEntityId> bijliActorIds)
        {
            for (var i = 0; i < frame.MovementCommands.Length; i++)
            {
                var command = frame.MovementCommands[i];
                if (command.SimulationTick != frame.SimulationTick)
                {
                    throw new InvalidOperationException("Movement command ticks must match their replay frame.");
                }

                authority.ResolveMovement(command, fixedDeltaSeconds);
            }

            for (var i = 0; i < frame.AttackCommands.Length; i++)
            {
                var command = frame.AttackCommands[i];
                if (command.SimulationTick != frame.SimulationTick)
                {
                    throw new InvalidOperationException("Attack command ticks must match their replay frame.");
                }

                authority.TryAcceptAttack(command);
            }

            for (var i = 0; i < frame.AbilityCommands.Length; i++)
            {
                var recorded = frame.AbilityCommands[i];
                var command = recorded.Command;
                if (command.SimulationTick != frame.SimulationTick)
                {
                    throw new InvalidOperationException("Ability command ticks must match their replay frame.");
                }

                if (recorded.SpawnDecoy)
                {
                    authority.TrySpawnMayaDecoy(command.InstigatorId, command.SimulationTick, recorded.DecoyPosition);
                    continue;
                }

                if (command.AbilityId.Equals(FighterSpecialDefinition.PehelChargeThrow.AbilityId))
                {
                    authority.TryStartPehelCharge(command, recorded.Movement, recorded.Facing);
                    continue;
                }

                if (command.AbilityId.Equals(FighterDefinition.Bijli.Ability.AbilityId))
                {
                    authority.TryStartBijliDash(command, recorded.Movement, recorded.Facing);
                    continue;
                }

                throw new InvalidOperationException("Replay contains an unsupported ability command.");
            }

            for (var i = 0; i < frame.GadgetCommands.Length; i++)
            {
                var command = frame.GadgetCommands[i];
                if (command.Tick != frame.SimulationTick)
                {
                    throw new InvalidOperationException("Gadget command ticks must match their replay frame.");
                }

                authority.TryUseGadget(command);
            }

            for (var i = 0; i < pehelActorIds.Count; i++)
            {
                authority.AdvancePehelCharge(
                    pehelActorIds[i],
                    frame.SimulationTick,
                    fixedDeltaSeconds,
                    FighterSpecialDefinition.PehelChargeThrow.Magnitude);
            }
        }
    }

    public sealed class ReplayExecutionResult
    {
        public ReplayExecutionResult(
            bool succeeded,
            int divergenceTick,
            ulong expectedHash,
            ulong actualHash,
            string description,
            IReadOnlyList<ulong> actualHashes,
            OfflineMatchAuthority authority)
        {
            Succeeded = succeeded;
            DivergenceTick = divergenceTick;
            ExpectedHash = expectedHash;
            ActualHash = actualHash;
            Description = description ?? string.Empty;
            ActualHashes = actualHashes ?? Array.Empty<ulong>();
            Authority = authority;
        }

        public bool Succeeded { get; }
        public int DivergenceTick { get; }
        public ulong ExpectedHash { get; }
        public ulong ActualHash { get; }
        public string Description { get; }
        public IReadOnlyList<ulong> ActualHashes { get; }
        public OfflineMatchAuthority Authority { get; }
    }

    public struct MatchStateHashBuilder
    {
        private const ulong FnvOffsetBasis = 14695981039346656037UL;
        private const ulong FnvPrime = 1099511628211UL;

        private ulong _hash;

        public static MatchStateHashBuilder Create() => new MatchStateHashBuilder { _hash = FnvOffsetBasis };

        public void CombineInt(int value)
        {
            unchecked
            {
                _hash ^= (ulong)(byte)value;
                _hash *= FnvPrime;
                _hash ^= (ulong)(byte)(value >> 8);
                _hash *= FnvPrime;
                _hash ^= (ulong)(byte)(value >> 16);
                _hash *= FnvPrime;
                _hash ^= (ulong)(byte)(value >> 24);
                _hash *= FnvPrime;
            }
        }

        public void CombineBool(bool value) => CombineInt(value ? 1 : 0);

        public void CombineULong(ulong value)
        {
            CombineInt(unchecked((int)value));
            CombineInt(unchecked((int)(value >> 32)));
        }

        public void CombineFloat(float value) => CombineInt((int)(value * 1000f));

        public void CombineContentId(ContentId value)
        {
            CombineInt((int)value.Kind);
            var text = value.Value ?? string.Empty;
            CombineText(text);
        }

        public void CombineText(string text)
        {
            text ??= string.Empty;
            CombineInt(text.Length);
            for (var i = 0; i < text.Length; i++) CombineInt(text[i]);
        }

        public ulong Value => _hash;
    }
}
