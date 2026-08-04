using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public readonly struct MatchReplayHeader
    {
        public MatchReplayHeader(string arenaVersion, uint matchSeed, MatchSpawn[] spawns)
        {
            ArenaVersion = arenaVersion ?? "1.0.0-bazaar";
            MatchSeed = matchSeed;
            Spawns = spawns ?? Array.Empty<MatchSpawn>();
        }

        public string ArenaVersion { get; }
        public uint MatchSeed { get; }
        public MatchSpawn[] Spawns { get; }
    }

    public readonly struct MatchReplayFrame
    {
        public MatchReplayFrame(
            int simulationTick,
            AttackCommand[] attackCommands,
            AbilityCommand[] abilityCommands,
            GadgetUseCommand[] gadgetCommands)
        {
            SimulationTick = simulationTick;
            AttackCommands = attackCommands ?? Array.Empty<AttackCommand>();
            AbilityCommands = abilityCommands ?? Array.Empty<AbilityCommand>();
            GadgetCommands = gadgetCommands ?? Array.Empty<GadgetUseCommand>();
        }

        public int SimulationTick { get; }
        public AttackCommand[] AttackCommands { get; }
        public AbilityCommand[] AbilityCommands { get; }
        public GadgetUseCommand[] GadgetCommands { get; }
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
}
