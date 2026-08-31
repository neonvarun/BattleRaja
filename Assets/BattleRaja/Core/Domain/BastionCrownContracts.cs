using System;

namespace BattleRaja.Core.Domain
{
    /// <summary>First-class relationship used by Bastion Crown. The legacy
    /// CombatFaction remains available for Solo compatibility and presentation.
    /// </summary>
    public enum BastionTeamId
    {
        None = 0,
        Raja = 1,
        Rival = 2
    }

    public enum BastionRole
    {
        Anchor = 0,
        Runner = 1,
        Skirmisher = 2,
        Flex = 3
    }

    public enum BastionSquadPlan
    {
        ContestCrown = 0,
        EscortCarrier = 1,
        DefendShrine = 2,
        RecoverTickets = 3,
        CollapseTarget = 4,
        Regroup = 5,
        RetreatFromAandhi = 6
    }

    /// <summary>
    /// Read-only assignment produced by the deterministic Bastion squad
    /// planner. It is deliberately expressed as common movement/aim intent so
    /// human and bot adapters share the same command boundary.
    /// </summary>
    public readonly struct BastionSquadIntent
    {
        public BastionSquadIntent(
            BastionSquadPlan plan,
            Float2 destination,
            Float2 movement,
            Float2 aim,
            CombatEntityId focusTargetId,
            CombatEntityId supportTargetId,
            Float2 spacingOffset,
            bool ticketRisk)
        {
            Plan = plan;
            Destination = destination;
            Movement = movement;
            Aim = aim;
            FocusTargetId = focusTargetId;
            SupportTargetId = supportTargetId;
            SpacingOffset = spacingOffset;
            TicketRisk = ticketRisk;
        }

        public BastionSquadPlan Plan { get; }
        public Float2 Destination { get; }
        public Float2 Movement { get; }
        public Float2 Aim { get; }
        public CombatEntityId FocusTargetId { get; }
        public CombatEntityId SupportTargetId { get; }
        public Float2 SpacingOffset { get; }
        public bool TicketRisk { get; }
    }

    /// <summary>
    /// Pure deterministic squad blackboard. The planner uses only information
    /// present in the Bastion snapshots, so it cannot grant bots hidden vision
    /// or a numerical combat advantage. Tie breaks are actor-ID based.
    /// </summary>
    public static class BastionSquadPlanner
    {
        public static BastionSquadIntent Plan(
            TeamMember member,
            Float2 selfPosition,
            BastionParticipantSnapshot[] participants,
            CrownSparkSnapshot crown,
            TeamScore rajaScore,
            TeamScore rivalScore,
            TeamTicketPool rajaTickets,
            TeamTicketPool rivalTickets,
            ModeDefinition definition,
            bool overtime,
            Float2 zoneCenter = default(Float2),
            float zoneRadius = 0f,
            AandhiState aandhiState = AandhiState.Stable)
        {
            var ownShrine = member.TeamId == BastionTeamId.Raja
                ? definition.Raja.ShrinePosition
                : definition.Rival.ShrinePosition;
            var ownTickets = member.TeamId == BastionTeamId.Raja ? rajaTickets : rivalTickets;
            var ownScore = member.TeamId == BastionTeamId.Raja ? rajaScore : rivalScore;
            var enemyTeam = member.TeamId == BastionTeamId.Raja ? BastionTeamId.Rival : BastionTeamId.Raja;
            var selfHealthFraction = 1f;
            var ownCarrier = default(BastionParticipantSnapshot);
            var enemyCarrier = default(BastionParticipantSnapshot);
            var hasOwnCarrier = false;
            var hasEnemyCarrier = false;
            var supportTarget = default(BastionParticipantSnapshot);
            var hasSupportTarget = false;
            var focusTarget = default(BastionParticipantSnapshot);
            var focusDistance = float.MaxValue;

            if (participants != null)
            {
                for (var i = 0; i < participants.Length; i++)
                {
                    var participant = participants[i];
                    if (!participant.Alive) continue;
                    if (participant.ActorId == member.ActorId)
                    {
                        selfHealthFraction = participant.MaxHealth > 0
                            ? (float)participant.CurrentHealth / participant.MaxHealth
                            : 0f;
                    }

                    if (crown.IsCarried && participant.ActorId == crown.CarrierId)
                    {
                        if (participant.TeamId == member.TeamId)
                        {
                            ownCarrier = participant;
                            hasOwnCarrier = true;
                        }
                        else
                        {
                            enemyCarrier = participant;
                            hasEnemyCarrier = true;
                        }
                    }

                    if (participant.TeamId == member.TeamId && participant.ActorId != member.ActorId)
                    {
                        if (!hasSupportTarget ||
                            participant.CurrentHealth * supportTarget.MaxHealth < supportTarget.CurrentHealth * participant.MaxHealth ||
                            (participant.CurrentHealth * supportTarget.MaxHealth == supportTarget.CurrentHealth * participant.MaxHealth &&
                             participant.ActorId.Value < supportTarget.ActorId.Value))
                        {
                            supportTarget = participant;
                            hasSupportTarget = true;
                        }
                    }

                    if (participant.TeamId == enemyTeam)
                    {
                        var distanceToCrown = Float2.Distance(participant.Position, crown.Position);
                        if (distanceToCrown < focusDistance - 0.0001f ||
                            (Math.Abs(distanceToCrown - focusDistance) <= 0.0001f &&
                             (focusTarget.ActorId.Value == 0 || participant.ActorId.Value < focusTarget.ActorId.Value)))
                        {
                            focusTarget = participant;
                            focusDistance = distanceToCrown;
                        }
                    }
                }
            }

            var plan = BastionSquadPlan.ContestCrown;
            var destination = crown.Position;
            if (hasOwnCarrier)
            {
                if (ownCarrier.ActorId == member.ActorId)
                {
                    destination = ownShrine;
                    plan = BastionSquadPlan.EscortCarrier;
                }
                else if (member.Role == BastionRole.Anchor)
                {
                    destination = ownShrine;
                    plan = BastionSquadPlan.DefendShrine;
                }
                else
                {
                    destination = ownCarrier.Position;
                    plan = BastionSquadPlan.EscortCarrier;
                }
            }
            else if (hasEnemyCarrier)
            {
                destination = enemyCarrier.Position;
                plan = member.Role == BastionRole.Anchor
                    ? BastionSquadPlan.DefendShrine
                    : BastionSquadPlan.CollapseTarget;
                focusTarget = enemyCarrier;
            }
            else if (member.Role == BastionRole.Anchor)
            {
                destination = ownShrine;
                plan = BastionSquadPlan.DefendShrine;
            }
            else if (ownTickets.Remaining <= 2 && selfHealthFraction < 0.35f)
            {
                destination = ownShrine;
                plan = BastionSquadPlan.RecoverTickets;
            }
            else if (member.Role == BastionRole.Runner)
            {
                destination = crown.Position;
                plan = BastionSquadPlan.ContestCrown;
            }

            var movementTarget = destination;
            var direction = (destination - selfPosition).Normalized;
            var spacingOffset = Float2.Zero;
            if (member.Role != BastionRole.Runner && plan != BastionSquadPlan.EscortCarrier &&
                plan != BastionSquadPlan.CollapseTarget && direction.SqrMagnitude > 0.000001f)
            {
                var side = (member.ActorId.Value & 1) == 0 ? -1f : 1f;
                spacingOffset = new Float2(-direction.Y, direction.X) * (0.65f * side);
                movementTarget += spacingOffset;
                direction = (movementTarget - selfPosition).Normalized;
            }

            var focusId = focusTarget.ActorId.Value > 0 ? focusTarget.ActorId : default(CombatEntityId);
            var supportId = hasSupportTarget ? supportTarget.ActorId : default(CombatEntityId);
            var aim = focusId.Value > 0
                ? (focusTarget.Position - selfPosition).Normalized
                : direction.SqrMagnitude > 0.000001f ? direction : Float2.Up;
            var ticketRisk = ownTickets.Remaining <= 2 || (overtime && ownScore.Score < rivalScore.Score);
            var distanceToZone = zoneRadius > 0f ? Float2.Distance(selfPosition, zoneCenter) : 0f;
            if (zoneRadius > 0f &&
                (distanceToZone > zoneRadius + 0.001f ||
                 (aandhiState == AandhiState.Closing && distanceToZone > zoneRadius * 0.92f)))
            {
                plan = BastionSquadPlan.RetreatFromAandhi;
                destination = zoneCenter;
                movementTarget = destination;
                spacingOffset = Float2.Zero;
                direction = (destination - selfPosition).Normalized;
            }

            return new BastionSquadIntent(
                plan,
                movementTarget,
                direction,
                aim,
                focusId,
                supportId,
                spacingOffset,
                ticketRisk);
        }
    }

    public enum BastionMatchResultReason
    {
        None = 0,
        FirstToScore = 1,
        TeamWipe = 2,
        Clock = 3,
        OvertimeDeposit = 4,
        OvertimeTeamWipe = 5,
        OvertimeCap = 6,
        Draw = 7
    }

    public readonly struct TeamDefinition
    {
        public TeamDefinition(
            BastionTeamId id,
            string displayName,
            Float2 spawnBankCenter,
            Float2 shrinePosition,
            int slotStart,
            int slotCount)
        {
            if (id == BastionTeamId.None) throw new ArgumentOutOfRangeException(nameof(id));
            if (string.IsNullOrWhiteSpace(displayName)) throw new ArgumentException("Team name is required.", nameof(displayName));
            if (!spawnBankCenter.IsFinite || !shrinePosition.IsFinite) throw new ArgumentException("Team anchors must be finite.");
            if (slotStart <= 0 || slotCount <= 0) throw new ArgumentOutOfRangeException(nameof(slotStart));
            Id = id;
            DisplayName = displayName.Trim();
            SpawnBankCenter = spawnBankCenter;
            ShrinePosition = shrinePosition;
            SlotStart = slotStart;
            SlotCount = slotCount;
        }

        public BastionTeamId Id { get; }
        public string DisplayName { get; }
        public Float2 SpawnBankCenter { get; }
        public Float2 ShrinePosition { get; }
        public int SlotStart { get; }
        public int SlotCount { get; }
    }

    public readonly struct TeamMember
    {
        public TeamMember(
            CombatEntityId actorId,
            BastionTeamId teamId,
            ContentId fighterId,
            BastionRole role,
            bool isHuman)
        {
            if (actorId.Value <= 0) throw new ArgumentOutOfRangeException(nameof(actorId));
            if (teamId == BastionTeamId.None) throw new ArgumentOutOfRangeException(nameof(teamId));
            if (!fighterId.IsValid || fighterId.Kind != ContentIdKind.Fighter)
            {
                throw new ArgumentException("A valid fighter content ID is required.", nameof(fighterId));
            }

            ActorId = actorId;
            TeamId = teamId;
            FighterId = fighterId;
            Role = role;
            IsHuman = isHuman;
        }

        public CombatEntityId ActorId { get; }
        public BastionTeamId TeamId { get; }
        public ContentId FighterId { get; }
        public BastionRole Role { get; }
        public bool IsHuman { get; }
    }

    /// <summary>Immutable match-start data for one of the eight canonical slots.</summary>
    public readonly struct BastionCrownSlot
    {
        public BastionCrownSlot(TeamMember member, Float2 spawnPosition, int maxHealth)
        {
            if (!spawnPosition.IsFinite) throw new ArgumentException("Spawn positions must be finite.", nameof(spawnPosition));
            if (maxHealth <= 0) throw new ArgumentOutOfRangeException(nameof(maxHealth));
            Member = member;
            SpawnPosition = spawnPosition;
            MaxHealth = maxHealth;
        }

        public TeamMember Member { get; }
        public Float2 SpawnPosition { get; }
        public int MaxHealth { get; }
    }

    public readonly struct ObjectiveDefinition
    {
        public ObjectiveDefinition(
            Float2[] socketPositions,
            Float2 rajaShrine,
            Float2 rivalShrine,
            float contactRadius,
            float pickupSeconds,
            float carrierSpeedMultiplier,
            float dropLockSeconds,
            float dropLifetimeSeconds,
            float rotationSeconds,
            float depositChannelSeconds)
        {
            if (socketPositions == null || socketPositions.Length != 3)
            {
                throw new ArgumentException("Bastion Crown requires exactly three sockets.", nameof(socketPositions));
            }

            for (var i = 0; i < socketPositions.Length; i++)
            {
                if (!socketPositions[i].IsFinite) throw new ArgumentException("Socket positions must be finite.", nameof(socketPositions));
            }

            if (!rajaShrine.IsFinite || !rivalShrine.IsFinite) throw new ArgumentException("Shrine positions must be finite.");
            if (contactRadius <= 0f || pickupSeconds <= 0f || carrierSpeedMultiplier <= 0f || carrierSpeedMultiplier >= 1f ||
                dropLockSeconds < 0f || dropLifetimeSeconds <= 0f || rotationSeconds <= 0f || depositChannelSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(contactRadius), "Objective timings and ranges are invalid.");
            }

            SocketPositions = (Float2[])socketPositions.Clone();
            RajaShrine = rajaShrine;
            RivalShrine = rivalShrine;
            ContactRadius = contactRadius;
            PickupSeconds = pickupSeconds;
            CarrierSpeedMultiplier = carrierSpeedMultiplier;
            DropLockSeconds = dropLockSeconds;
            DropLifetimeSeconds = dropLifetimeSeconds;
            RotationSeconds = rotationSeconds;
            DepositChannelSeconds = depositChannelSeconds;
        }

        public Float2[] SocketPositions { get; }
        public Float2 RajaShrine { get; }
        public Float2 RivalShrine { get; }
        public float ContactRadius { get; }
        public float PickupSeconds { get; }
        public float CarrierSpeedMultiplier { get; }
        public float DropLockSeconds { get; }
        public float DropLifetimeSeconds { get; }
        public float RotationSeconds { get; }
        public float DepositChannelSeconds { get; }
    }

    public readonly struct RespawnPolicy
    {
        public RespawnPolicy(float spectatorSeconds, float respawnSeconds, float spawnProtectionSeconds, int startingTickets)
        {
            if (spectatorSeconds < 0f || respawnSeconds < spectatorSeconds || spawnProtectionSeconds < 0f || startingTickets < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(spectatorSeconds));
            }

            SpectatorSeconds = spectatorSeconds;
            RespawnSeconds = respawnSeconds;
            SpawnProtectionSeconds = spawnProtectionSeconds;
            StartingTickets = startingTickets;
        }

        public float SpectatorSeconds { get; }
        public float RespawnSeconds { get; }
        public float SpawnProtectionSeconds { get; }
        public int StartingTickets { get; }
    }

    public readonly struct TeamTicketPool
    {
        public TeamTicketPool(BastionTeamId teamId, int maximum, int remaining, int spent)
        {
            if (teamId == BastionTeamId.None) throw new ArgumentOutOfRangeException(nameof(teamId));
            if (maximum < 0 || remaining < 0 || spent < 0 || remaining > maximum || spent > maximum)
            {
                throw new ArgumentOutOfRangeException(nameof(maximum));
            }

            TeamId = teamId;
            Maximum = maximum;
            Remaining = remaining;
            Spent = spent;
        }

        public BastionTeamId TeamId { get; }
        public int Maximum { get; }
        public int Remaining { get; }
        public int Spent { get; }
        public bool HasTicket => Remaining > 0;

        public TeamTicketPool Spend()
        {
            return !HasTicket
                ? this
                : new TeamTicketPool(TeamId, Maximum, Remaining - 1, Spent + 1);
        }
    }

    public readonly struct TeamScore
    {
        public TeamScore(
            BastionTeamId teamId,
            int score,
            int deposits,
            int kos,
            int assists,
            int crownPickups,
            int ticketsSpent,
            float objectiveSeconds,
            int deaths = 0,
            int damageDealt = 0,
            int healingDone = 0,
            int gadgetUses = 0,
            int abilityUses = 0)
        {
            TeamId = teamId;
            Score = Math.Max(0, score);
            Deposits = Math.Max(0, deposits);
            KOs = Math.Max(0, kos);
            Assists = Math.Max(0, assists);
            CrownPickups = Math.Max(0, crownPickups);
            TicketsSpent = Math.Max(0, ticketsSpent);
            ObjectiveSeconds = Math.Max(0f, objectiveSeconds);
            Deaths = Math.Max(0, deaths);
            DamageDealt = Math.Max(0, damageDealt);
            HealingDone = Math.Max(0, healingDone);
            GadgetUses = Math.Max(0, gadgetUses);
            AbilityUses = Math.Max(0, abilityUses);
        }

        public BastionTeamId TeamId { get; }
        public int Score { get; }
        public int Deposits { get; }
        public int KOs { get; }
        public int Assists { get; }
        public int CrownPickups { get; }
        public int TicketsSpent { get; }
        public float ObjectiveSeconds { get; }
        public int Deaths { get; }
        public int DamageDealt { get; }
        public int HealingDone { get; }
        public int GadgetUses { get; }
        public int AbilityUses { get; }
    }

    public readonly struct ModeDefinition
    {
        public ModeDefinition(
            string modeId,
            string displayName,
            float arenaSize,
            float readySeconds,
            float liveSeconds,
            float overtimeSeconds,
            int scoreToWin,
            TeamDefinition raja,
            TeamDefinition rival,
            ObjectiveDefinition objective,
            RespawnPolicy respawn)
        {
            if (string.IsNullOrWhiteSpace(modeId) || string.IsNullOrWhiteSpace(displayName)) throw new ArgumentException("Mode identity is required.");
            if (arenaSize <= 0f || readySeconds < 0f || liveSeconds <= 0f || overtimeSeconds < 0f || scoreToWin <= 0) throw new ArgumentOutOfRangeException(nameof(arenaSize));
            if (raja.Id != BastionTeamId.Raja || rival.Id != BastionTeamId.Rival) throw new ArgumentException("Bastion Crown requires Raja and Rival teams.");
            ModeId = modeId.Trim();
            DisplayName = displayName.Trim();
            ArenaSize = arenaSize;
            ReadySeconds = readySeconds;
            LiveSeconds = liveSeconds;
            OvertimeSeconds = overtimeSeconds;
            ScoreToWin = scoreToWin;
            Raja = raja;
            Rival = rival;
            Objective = objective;
            Respawn = respawn;
        }

        public string ModeId { get; }
        public string DisplayName { get; }
        public float ArenaSize { get; }
        public float ReadySeconds { get; }
        public float LiveSeconds { get; }
        public float OvertimeSeconds { get; }
        public int ScoreToWin { get; }
        public TeamDefinition Raja { get; }
        public TeamDefinition Rival { get; }
        public ObjectiveDefinition Objective { get; }
        public RespawnPolicy Respawn { get; }

        public static ModeDefinition BastionCrown => new ModeDefinition(
            "BR_BastionCrown_V1",
            "Bastion Crown",
            32f,
            3f,
            240f,
            30f,
            15,
            new TeamDefinition(BastionTeamId.Raja, "Team Raja", new Float2(-11f, 0f), new Float2(-10f, 0f), 1, 4),
            new TeamDefinition(BastionTeamId.Rival, "Rival", new Float2(11f, 0f), new Float2(10f, 0f), 5, 4),
            new ObjectiveDefinition(
                new[] { new Float2(0f, -7f), new Float2(-7f, 4f), new Float2(7f, 4f) },
                new Float2(-10f, 0f),
                new Float2(10f, 0f),
                1.35f,
                0.25f,
                0.88f,
                1.25f,
                6f,
                35f,
                1.25f),
            new RespawnPolicy(4f, 5f, 2.5f, 12));
    }

    public readonly struct BastionParticipantSnapshot
    {
        public BastionParticipantSnapshot(
            TeamMember member,
            Float2 position,
            int currentHealth,
            int maxHealth,
            bool alive,
            bool spectating,
            bool respawnPending,
            float spectatorRemaining,
            float respawnRemaining,
            bool spawnProtected,
            int eliminations,
            int deaths,
            int assists,
            int damageDealt,
            int healingDone,
            float objectiveSeconds,
            int gadgetUses = 0,
            int abilityUses = 0)
        {
            Member = member;
            Position = position;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Alive = alive;
            Spectating = spectating;
            RespawnPending = respawnPending;
            SpectatorRemaining = spectatorRemaining;
            RespawnRemaining = respawnRemaining;
            SpawnProtected = spawnProtected;
            Eliminations = eliminations;
            Deaths = deaths;
            Assists = assists;
            DamageDealt = damageDealt;
            HealingDone = healingDone;
            ObjectiveSeconds = objectiveSeconds;
            GadgetUses = gadgetUses;
            AbilityUses = abilityUses;
        }

        public TeamMember Member { get; }
        public CombatEntityId ActorId => Member.ActorId;
        public BastionTeamId TeamId => Member.TeamId;
        public Float2 Position { get; }
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public bool Alive { get; }
        public bool Spectating { get; }
        public bool RespawnPending { get; }
        public float SpectatorRemaining { get; }
        public float RespawnRemaining { get; }
        public bool SpawnProtected { get; }
        public int Eliminations { get; }
        public int Deaths { get; }
        public int Assists { get; }
        public int DamageDealt { get; }
        public int HealingDone { get; }
        public float ObjectiveSeconds { get; }
        public int GadgetUses { get; }
        public int AbilityUses { get; }
    }

    public readonly struct CrownSparkSnapshot
    {
        public CrownSparkSnapshot(
            int socketIndex,
            Float2 position,
            CombatEntityId carrierId,
            bool dropped,
            float pickupProgress,
            float dropLockRemaining,
            float dropRemaining,
            float rotationRemaining,
            CombatEntityId channelActorId,
            float depositProgress)
        {
            SocketIndex = socketIndex;
            Position = position;
            CarrierId = carrierId;
            Dropped = dropped;
            PickupProgress = pickupProgress;
            DropLockRemaining = dropLockRemaining;
            DropRemaining = dropRemaining;
            RotationRemaining = rotationRemaining;
            ChannelActorId = channelActorId;
            DepositProgress = depositProgress;
        }

        public int SocketIndex { get; }
        public Float2 Position { get; }
        public CombatEntityId CarrierId { get; }
        public bool IsCarried => CarrierId.Value > 0;
        public bool Dropped { get; }
        public float PickupProgress { get; }
        public float DropLockRemaining { get; }
        public float DropRemaining { get; }
        public float RotationRemaining { get; }
        public CombatEntityId ChannelActorId { get; }
        public float DepositProgress { get; }
    }

    public readonly struct BastionResultSummary
    {
        public BastionResultSummary(
            BastionTeamId winner,
            bool draw,
            BastionMatchResultReason reason,
            float elapsedSeconds,
            uint seed,
            TeamScore raja,
            TeamScore rival,
            TeamTicketPool rajaTickets,
            TeamTicketPool rivalTickets)
        {
            Winner = winner;
            IsDraw = draw;
            Reason = reason;
            ElapsedSeconds = elapsedSeconds;
            Seed = seed;
            Raja = raja;
            Rival = rival;
            RajaTickets = rajaTickets;
            RivalTickets = rivalTickets;
        }

        public BastionTeamId Winner { get; }
        public bool IsDraw { get; }
        public BastionMatchResultReason Reason { get; }
        public float ElapsedSeconds { get; }
        public uint Seed { get; }
        public TeamScore Raja { get; }
        public TeamScore Rival { get; }
        public TeamTicketPool RajaTickets { get; }
        public TeamTicketPool RivalTickets { get; }
    }

    public readonly struct BastionCrownTick
    {
        public BastionCrownTick(
            int simulationTick,
            float elapsedSeconds,
            bool live,
            bool overtime,
            bool matchEnded,
            BastionTeamId winner,
            CrownSparkSnapshot crown,
            TeamScore rajaScore,
            TeamScore rivalScore,
            TeamTicketPool rajaTickets,
            TeamTicketPool rivalTickets,
            CombatEntityId[] respawnedActors,
            BastionResultSummary result)
        {
            SimulationTick = simulationTick;
            ElapsedSeconds = elapsedSeconds;
            Live = live;
            Overtime = overtime;
            MatchEnded = matchEnded;
            Winner = winner;
            Crown = crown;
            RajaScore = rajaScore;
            RivalScore = rivalScore;
            RajaTickets = rajaTickets;
            RivalTickets = rivalTickets;
            RespawnedActors = respawnedActors ?? Array.Empty<CombatEntityId>();
            Result = result;
        }

        public int SimulationTick { get; }
        public float ElapsedSeconds { get; }
        public bool Ready => !Live && !Overtime && !MatchEnded;
        public bool Live { get; }
        public bool Overtime { get; }
        public bool MatchEnded { get; }
        public BastionTeamId Winner { get; }
        public CrownSparkSnapshot Crown { get; }
        public TeamScore RajaScore { get; }
        public TeamScore RivalScore { get; }
        public TeamTicketPool RajaTickets { get; }
        public TeamTicketPool RivalTickets { get; }
        public CombatEntityId[] RespawnedActors { get; }
        public BastionResultSummary Result { get; }
    }
}
