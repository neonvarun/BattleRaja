using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public enum MatchReplayScenario
    {
        SoloRaja = 0,
        BastionCrown = 1
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

    public enum MatchReplayCommandKind
    {
        Movement = 1,
        Attack = 2,
        Ability = 3,
        Gadget = 4,
        PehelChargeStep = 5
    }

    /// <summary>
    /// Records authoritative submission order for one replay frame. Legacy
    /// frames leave this empty and retain their historical deterministic order;
    /// production capture fills it to preserve same-tick interactions.
    /// </summary>
    public readonly struct MatchReplayCommandOrder
    {
        public MatchReplayCommandOrder(MatchReplayCommandKind kind, int index)
        {
            Kind = kind;
            Index = index;
        }

        public MatchReplayCommandKind Kind { get; }
        public int Index { get; }
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
            GadgetPickupDefinition[] gadgetPickups,
            bool includesBastionState = false)
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
            IncludesBastionState = includesBastionState;
        }

        public string ArenaVersion { get; }
        public uint MatchSeed { get; }
        public MatchSpawn[] Spawns { get; }
        public float FixedDeltaSeconds { get; }
        public MatchReplayScenario Scenario { get; }
        public MatchReplayParticipant[] Participants { get; }
        public MatchPickupDefinition[] Pickups { get; }
        public GadgetPickupDefinition[] GadgetPickups { get; }
        public bool IncludesBastionState { get; }
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
            : this(simulationTick, movementCommands, attackCommands, abilityCommands, gadgetCommands, null)
        {
        }

        public MatchReplayFrame(
            int simulationTick,
            MovementCommand[] movementCommands,
            AttackCommand[] attackCommands,
            MatchReplayAbilityCommand[] abilityCommands,
            GadgetUseCommand[] gadgetCommands,
            MatchReplayCommandOrder[] commandOrder)
        {
            SimulationTick = simulationTick;
            MovementCommands = movementCommands ?? Array.Empty<MovementCommand>();
            AttackCommands = attackCommands ?? Array.Empty<AttackCommand>();
            AbilityCommands = abilityCommands ?? Array.Empty<MatchReplayAbilityCommand>();
            GadgetCommands = gadgetCommands ?? Array.Empty<GadgetUseCommand>();
            CommandOrder = commandOrder ?? Array.Empty<MatchReplayCommandOrder>();
        }

        public int SimulationTick { get; }
        public MovementCommand[] MovementCommands { get; }
        public AttackCommand[] AttackCommands { get; }
        public MatchReplayAbilityCommand[] AbilityCommands { get; }
        public GadgetUseCommand[] GadgetCommands { get; }
        public MatchReplayCommandOrder[] CommandOrder { get; }

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
            TickStateSnapshots = new List<MatchParticipantSnapshot[]>();
        }

        public MatchReplayHeader Header { get; }
        public List<MatchReplayFrame> Frames { get; }
        public List<ulong> TickStateHashes { get; }
        public List<MatchParticipantSnapshot[]> TickStateSnapshots { get; }

        public void AddFrame(MatchReplayFrame frame, ulong stateHash)
        {
            AddFrame(frame, stateHash, null);
        }

        public void AddFrame(
            MatchReplayFrame frame,
            ulong stateHash,
            MatchParticipantSnapshot[] stateSnapshots)
        {
            Frames.Add(frame);
            TickStateHashes.Add(stateHash);
            TickStateSnapshots.Add(stateSnapshots ?? Array.Empty<MatchParticipantSnapshot>());
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
            var bastion = header.Scenario == MatchReplayScenario.BastionCrown && header.IncludesBastionState
                ? CreateBastionMatch(header)
                : null;
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

                ApplyFrame(authority, frame, header.FixedDeltaSeconds, pehelActorIds, bijliActorIds, bastion);
                var tick = authority.Advance(frame.SimulationTick, header.FixedDeltaSeconds);
                if (bastion != null)
                {
                    SyncBastionFromAuthority(bastion, authority, tick);
                    ProcessBastionObjectiveInteractions(bastion, header.FixedDeltaSeconds);
                    var bastionTick = bastion.Advance(header.FixedDeltaSeconds, frame.SimulationTick);
                    for (var respawnIndex = 0; respawnIndex < bastionTick.RespawnedActors.Length; respawnIndex++)
                    {
                        var actorId = bastionTick.RespawnedActors[respawnIndex];
                        if (bastion.TryGetParticipant(actorId, out var respawned))
                        {
                            authority.RespawnParticipant(actorId, respawned.Position);
                        }
                    }

                    if (bastionTick.MatchEnded) authority.Simulation.ForceResolve();
                }

                var snapshots = authority.Simulation.GetSnapshots();
                var hash = DeterministicReplayHasher.CalculateTickHash(authority, tick, snapshots);
                if (bastion != null)
                {
                    var combined = MatchStateHashBuilder.Create();
                    combined.CombineULong(hash);
                    combined.CombineULong(bastion.CalculateDeterministicHash(frame.SimulationTick));
                    hash = combined.Value;
                }
                actualHashes.Add(hash);

                if (frameIndex < replay.TickStateSnapshots.Count &&
                    replay.TickStateSnapshots[frameIndex] != null &&
                    replay.TickStateSnapshots[frameIndex].Length > 0 &&
                    !SnapshotsEqual(replay.TickStateSnapshots[frameIndex], snapshots, out var snapshotDifference))
                {
                    return new ReplayExecutionResult(
                        false,
                        frame.SimulationTick,
                        replay.TickStateHashes[frameIndex],
                        hash,
                        $"Canonical authority snapshot diverged: {snapshotDifference}",
                        actualHashes,
                        authority);
                }

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

        private static BastionCrownMatch CreateBastionMatch(MatchReplayHeader header)
        {
            if (header.Spawns.Length != BastionCrownMatch.ParticipantCount ||
                header.Participants.Length != BastionCrownMatch.ParticipantCount)
            {
                throw new InvalidOperationException("Bastion replays require exactly eight participants.");
            }

            var slots = new BastionCrownSlot[BastionCrownMatch.ParticipantCount];
            for (var i = 0; i < header.Spawns.Length; i++)
            {
                var spawn = header.Spawns[i];
                var participant = FindReplayParticipant(header.Participants, spawn.Id);
                if (participant.ActorId.Value != i + 1)
                {
                    throw new InvalidOperationException("Bastion replay actor IDs must be 1 through 8 in order.");
                }

                var team = participant.ActorId.Value <= BastionCrownMatch.TeamSize
                    ? BastionTeamId.Raja
                    : BastionTeamId.Rival;
                var role = ResolveBastionRole(participant.FighterId, participant.ActorId.Value);
                slots[i] = new BastionCrownSlot(
                    new TeamMember(
                        participant.ActorId,
                        team,
                        participant.FighterId,
                        role,
                        participant.ActorId.Value == 1),
                    spawn.Position,
                    spawn.MaxHealth);
            }

            var match = new BastionCrownMatch(header.MatchSeed);
            match.Start(slots);
            return match;
        }

        private static MatchReplayParticipant FindReplayParticipant(
            MatchReplayParticipant[] participants,
            CombatEntityId actorId)
        {
            for (var i = 0; i < participants.Length; i++)
            {
                if (participants[i].ActorId == actorId) return participants[i];
            }

            throw new InvalidOperationException($"Replay participant {actorId.Value} is missing.");
        }

        private static BastionRole ResolveBastionRole(ContentId fighterId, int actorId)
        {
            if (fighterId.Equals(FighterDefinition.Pehel.FighterId)) return BastionRole.Anchor;
            if (fighterId.Equals(FighterDefinition.Maya.FighterId)) return BastionRole.Runner;
            return actorId == 4 || actorId == 8 ? BastionRole.Flex : BastionRole.Skirmisher;
        }

        private static void SyncBastionFromAuthority(
            BastionCrownMatch bastion,
            OfflineMatchAuthority authority,
            MatchAuthorityTick tick)
        {
            bastion.SyncAandhi(
                tick.Result.ZoneCenter,
                tick.Result.ZoneRadius,
                tick.Result.AandhiState,
                tick.Result.WarningRemainingSeconds);

            for (var i = 0; i < tick.DamageEvents.Length; i++)
            {
                var damage = tick.DamageEvents[i];
                if (damage.AmountApplied <= 0) continue;
                bastion.NotifyCombatDamage(
                    damage.InstigatorId,
                    damage.TargetId,
                    damage.AmountApplied,
                    damage.TargetDefeated,
                    damage.EventId);
            }

            var snapshots = authority.Simulation.GetSnapshots();
            for (var i = 0; i < snapshots.Length; i++)
            {
                var snapshot = snapshots[i];
                bastion.SetPosition(snapshot.Id, snapshot.Position);
                bastion.SetHealth(snapshot.Id, snapshot.CurrentHealth);
                if (!snapshot.Alive && bastion.TryGetParticipant(snapshot.Id, out var teamSnapshot) && teamSnapshot.Alive)
                {
                    bastion.SyncParticipant(snapshot.Id, snapshot.Position, 0, false);
                }
            }

            for (var i = 0; i < tick.GadgetHealingIntents.Length; i++)
            {
                var healing = tick.GadgetHealingIntents[i];
                var healerId = healing.HealerId.Value > 0 ? healing.HealerId : healing.TargetId;
                bastion.NotifyHealing(healerId, healing.TargetId, healing.Amount, healing.EventId);
            }

            for (var i = 0; i < tick.PickupCollections.Length; i++)
            {
                var collection = tick.PickupCollections[i];
                bastion.NotifyHealing(
                    collection.CollectorId,
                    collection.CollectorId,
                    collection.HealAmount,
                    collection.HealingEventId);
            }
        }

        private static void ProcessBastionObjectiveInteractions(
            BastionCrownMatch bastion,
            float fixedDeltaSeconds)
        {
            if (!bastion.IsLive || fixedDeltaSeconds <= 0f) return;
            var crown = bastion.Crown;
            if (!crown.IsCarried)
            {
                var candidateId = default(CombatEntityId);
                var candidateDistance = float.MaxValue;
                var snapshots = bastion.GetParticipantSnapshots();
                for (var i = 0; i < snapshots.Length; i++)
                {
                    var participant = snapshots[i];
                    var distance = Float2.Distance(participant.Position, crown.Position);
                    if (!participant.Alive || distance > bastion.Definition.Objective.ContactRadius ||
                        distance > candidateDistance ||
                        (Math.Abs(distance - candidateDistance) <= 0.0001f &&
                         (candidateId.Value == 0 || participant.ActorId.Value >= candidateId.Value)))
                    {
                        continue;
                    }

                    candidateId = participant.ActorId;
                    candidateDistance = distance;
                }

                if (candidateId.Value > 0)
                {
                    bastion.TryPickupCrown(candidateId, fixedDeltaSeconds);
                }
                else
                {
                    bastion.TryPickupCrown(new CombatEntityId(1), 0f);
                }

                return;
            }

            var carrierId = crown.CarrierId;
            if (!bastion.TryGetParticipant(carrierId, out var carrier) || !carrier.Alive)
            {
                bastion.CancelDeposit(carrierId);
                return;
            }

            var shrine = carrier.TeamId == BastionTeamId.Raja
                ? bastion.Definition.Raja.ShrinePosition
                : bastion.Definition.Rival.ShrinePosition;
            if (Float2.Distance(carrier.Position, shrine) <= bastion.Definition.Objective.ContactRadius * 1.35f)
            {
                bastion.TryBeginDeposit(carrierId);
            }
            else
            {
                bastion.CancelDeposit(carrierId);
            }
        }

        private static bool SnapshotsEqual(
            MatchParticipantSnapshot[] expected,
            MatchParticipantSnapshot[] actual,
            out string difference)
        {
            if (expected == null || actual == null || expected.Length != actual.Length)
            {
                difference = $"participant count expected={expected?.Length ?? 0} actual={actual?.Length ?? 0}";
                return false;
            }

            for (var i = 0; i < expected.Length; i++)
            {
                var left = expected[i];
                var right = actual[i];
                if (left.Id != right.Id || !left.Position.Equals(right.Position) ||
                    left.CurrentHealth != right.CurrentHealth || left.MaxHealth != right.MaxHealth ||
                    left.Alive != right.Alive || left.Placement != right.Placement ||
                    left.Eliminations != right.Eliminations || left.DamageDealt != right.DamageDealt ||
                    left.Assists != right.Assists || !left.SurvivalTimeSeconds.Equals(right.SurvivalTimeSeconds))
                {
                    difference = $"actor {left.Id.Value} expected pos={left.Position} hp={left.CurrentHealth}/{left.MaxHealth} " +
                        $"alive={left.Alive} placement={left.Placement} damage={left.DamageDealt} actual pos={right.Position} " +
                        $"hp={right.CurrentHealth}/{right.MaxHealth} alive={right.Alive} placement={right.Placement} damage={right.DamageDealt}";
                    return false;
                }
            }

            difference = string.Empty;
            return true;
        }

        private static OfflineMatchAuthority CreateAuthority(MatchReplayHeader header)
        {
            OfflineMatchDefinition definition;
            switch (header.Scenario)
            {
                case MatchReplayScenario.SoloRaja:
                    definition = OfflineMatchDefinition.SoloRaja;
                    break;
                case MatchReplayScenario.BastionCrown:
                    definition = OfflineMatchDefinition.BastionCrown;
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
                // Solo Raja replays use one combatant group per actor. Bastion
                // Crown replays use the canonical Raja (1-4) and Rival (5-8)
                // groups; the stored faction remains a compatibility label.
                var combatGroup = header.Scenario == MatchReplayScenario.BastionCrown
                    ? participant.ActorId.Value <= 4 ? 1 : 2
                    : participant.ActorId.Value;
                authority.ConfigureCombatGroup(participant.ActorId, combatGroup);
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
            List<CombatEntityId> bijliActorIds,
            BastionCrownMatch bastion)
        {
            if (frame.CommandOrder.Length > 0)
            {
                ApplyOrderedFrame(authority, frame, fixedDeltaSeconds, bastion);
                return;
            }

            for (var i = 0; i < frame.GadgetCommands.Length; i++)
            {
                var command = frame.GadgetCommands[i];
                if (command.Tick != frame.SimulationTick)
                {
                    throw new InvalidOperationException("Gadget command ticks must match their replay frame.");
                }

                var gadgetResult = authority.TryUseGadget(command);
                if (gadgetResult.Used) bastion?.RecordGadgetUse(command.UserId, gadgetResult.EventId);
            }

            // Production adapters submit gadgets, attacks and abilities while the
            // canonical tick event is being raised, then the controller resolves
            // queued movement commands. Keeping that order here is important when
            // a gadget displacement or fighter action lock affects same-tick movement.
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
                    var decoy = authority.TrySpawnMayaDecoy(command.InstigatorId, command.SimulationTick, recorded.DecoyPosition);
                    if (decoy.Active && decoy.AbilityExecutionId > 0)
                    {
                        bastion?.RecordAbilityUse(command.InstigatorId, decoy.AbilityExecutionId);
                    }
                    continue;
                }

                if (command.AbilityId.Equals(FighterSpecialDefinition.PehelChargeThrow.AbilityId))
                {
                    var start = authority.TryStartPehelCharge(command, recorded.Movement, recorded.Facing);
                    if (start.Accepted) bastion?.RecordAbilityUse(command.InstigatorId, start.AbilityExecutionId);
                    continue;
                }

                if (command.AbilityId.Equals(FighterDefinition.Bijli.Ability.AbilityId))
                {
                    var start = authority.TryStartBijliDash(command, recorded.Movement, recorded.Facing);
                    if (start.Accepted) bastion?.RecordAbilityUse(command.InstigatorId, start.AbilityExecutionId);
                    continue;
                }

                throw new InvalidOperationException("Replay contains an unsupported ability command.");
            }

            for (var i = 0; i < frame.MovementCommands.Length; i++)
            {
                var command = frame.MovementCommands[i];
                if (command.SimulationTick != frame.SimulationTick)
                {
                    throw new InvalidOperationException("Movement command ticks must match their replay frame.");
                }

                authority.ResolveMovement(command, fixedDeltaSeconds);
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

        private static void ApplyOrderedFrame(
            OfflineMatchAuthority authority,
            MatchReplayFrame frame,
            float fixedDeltaSeconds,
            BastionCrownMatch bastion)
        {
            for (var i = 0; i < frame.CommandOrder.Length; i++)
            {
                var order = frame.CommandOrder[i];
                switch (order.Kind)
                {
                    case MatchReplayCommandKind.Movement:
                        if (order.Index < 0 || order.Index >= frame.MovementCommands.Length)
                        {
                            throw new InvalidOperationException("Replay movement order index is invalid.");
                        }

                        var movement = frame.MovementCommands[order.Index];
                        if (movement.SimulationTick != frame.SimulationTick)
                        {
                            throw new InvalidOperationException("Movement command ticks must match their replay frame.");
                        }

                        if (!authority.IsAuthorityMovementLocked(new CombatEntityId(movement.ActorId)))
                        {
                            authority.ResolveMovement(movement, fixedDeltaSeconds);
                        }
                        break;
                    case MatchReplayCommandKind.Attack:
                        if (order.Index < 0 || order.Index >= frame.AttackCommands.Length)
                        {
                            throw new InvalidOperationException("Replay attack order index is invalid.");
                        }

                        var attack = frame.AttackCommands[order.Index];
                        if (attack.SimulationTick != frame.SimulationTick)
                        {
                            throw new InvalidOperationException("Attack command ticks must match their replay frame.");
                        }

                        authority.TryAcceptAttack(attack);
                        break;
                    case MatchReplayCommandKind.Ability:
                        if (order.Index < 0 || order.Index >= frame.AbilityCommands.Length)
                        {
                            throw new InvalidOperationException("Replay ability order index is invalid.");
                        }

                        ApplyAbility(authority, frame, frame.AbilityCommands[order.Index], bastion);
                        break;
                    case MatchReplayCommandKind.Gadget:
                        if (order.Index < 0 || order.Index >= frame.GadgetCommands.Length)
                        {
                            throw new InvalidOperationException("Replay gadget order index is invalid.");
                        }

                        var gadget = frame.GadgetCommands[order.Index];
                        if (gadget.Tick != frame.SimulationTick)
                        {
                            throw new InvalidOperationException("Gadget command ticks must match their replay frame.");
                        }

                        var gadgetResult = authority.TryUseGadget(gadget);
                        if (gadgetResult.Used) bastion?.RecordGadgetUse(gadget.UserId, gadgetResult.EventId);
                        break;
                    case MatchReplayCommandKind.PehelChargeStep:
                        if (order.Index <= 0)
                        {
                            throw new InvalidOperationException("Replay Pehel charge actor ID is invalid.");
                        }

                        authority.AdvancePehelCharge(
                            new CombatEntityId(order.Index),
                            frame.SimulationTick,
                            fixedDeltaSeconds,
                            FighterSpecialDefinition.PehelChargeThrow.Magnitude);
                        break;
                    default:
                        throw new InvalidOperationException("Replay contains an unsupported command-order kind.");
                }
            }
        }

        private static void ApplyAbility(
            OfflineMatchAuthority authority,
            MatchReplayFrame frame,
            MatchReplayAbilityCommand recorded,
            BastionCrownMatch bastion)
        {
            var command = recorded.Command;
            if (command.SimulationTick != frame.SimulationTick)
            {
                throw new InvalidOperationException("Ability command ticks must match their replay frame.");
            }

            if (recorded.SpawnDecoy)
            {
                var decoy = authority.TrySpawnMayaDecoy(command.InstigatorId, command.SimulationTick, recorded.DecoyPosition);
                if (decoy.Active && decoy.AbilityExecutionId > 0)
                {
                    bastion?.RecordAbilityUse(command.InstigatorId, decoy.AbilityExecutionId);
                }
                return;
            }

            if (command.AbilityId.Equals(FighterSpecialDefinition.PehelChargeThrow.AbilityId))
            {
                var start = authority.TryStartPehelCharge(command, recorded.Movement, recorded.Facing);
                if (start.Accepted) bastion?.RecordAbilityUse(command.InstigatorId, start.AbilityExecutionId);
                return;
            }

            if (command.AbilityId.Equals(FighterDefinition.Bijli.Ability.AbilityId))
            {
                var start = authority.TryStartBijliDash(command, recorded.Movement, recorded.Facing);
                if (start.Accepted) bastion?.RecordAbilityUse(command.InstigatorId, start.AbilityExecutionId);
                return;
            }

            throw new InvalidOperationException("Replay contains an unsupported ability command.");
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
