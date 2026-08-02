using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Infrastructure.Networking
{
    public enum ServerSlotState
    {
        Vacant = 0,
        Connected = 1,
        DisconnectedGrace = 2,
        BotTakeover = 3,
        Eliminated = 4
    }

    public readonly struct ServerMatchConfig
    {
        public ServerMatchConfig(string protocolVersion, int maxSlots, int tickRate, int reconnectGraceTicks)
        {
            ProtocolVersion = protocolVersion ?? string.Empty;
            MaxSlots = maxSlots;
            TickRate = tickRate;
            ReconnectGraceTicks = reconnectGraceTicks;
        }

        public string ProtocolVersion { get; }
        public int MaxSlots { get; }
        public int TickRate { get; }
        public int ReconnectGraceTicks { get; }

        public bool IsValid(out string reason)
        {
            if (string.IsNullOrWhiteSpace(ProtocolVersion) || MaxSlots < 2 || MaxSlots > 8 ||
                TickRate < 10 || TickRate > 60 || ReconnectGraceTicks < 1 || ReconnectGraceTicks > 900)
            {
                reason = "Protocol, slot count, tick rate or reconnect grace is outside the alpha bounds.";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public static ServerMatchConfig M9Proof => new ServerMatchConfig("m9-proof-v1", 8, 30, 90);
    }

    public readonly struct ServerSlotSnapshot
    {
        public ServerSlotSnapshot(int clientId, CombatEntityId actorId, ServerSlotState state, bool isBot, int lastInputTick, int graceRemainingTicks)
        {
            ClientId = clientId;
            ActorId = actorId;
            State = state;
            IsBot = isBot;
            LastInputTick = lastInputTick;
            GraceRemainingTicks = graceRemainingTicks;
        }

        public int ClientId { get; }
        public CombatEntityId ActorId { get; }
        public ServerSlotState State { get; }
        public bool IsBot { get; }
        public int LastInputTick { get; }
        public int GraceRemainingTicks { get; }
    }

    public sealed class AuthoritativeMatchServer
    {
        private readonly ServerMatchConfig _config;
        private readonly Dictionary<int, Slot> _slots = new Dictionary<int, Slot>(8);
        private OfflineMatchSimulation _simulation;
        private bool _roomStarted;
        private bool _matchStarted;
        private int _serverTick;
        private int _nextBotId = -1;

        public AuthoritativeMatchServer(ServerMatchConfig config)
        {
            if (!config.IsValid(out var reason)) throw new ArgumentException(reason, nameof(config));
            _config = config;
        }

        public ServerMatchConfig Config => _config;
        public int ServerTick => _serverTick;
        public bool RoomStarted => _roomStarted;
        public bool MatchStarted => _matchStarted;
        public int SlotCount => _slots.Count;
        public int ConnectedHumanCount
        {
            get
            {
                var count = 0;
                foreach (var slot in _slots.Values) if (!slot.IsBot && slot.State == ServerSlotState.Connected) count++;
                return count;
            }
        }

        public NetworkConnectFailure StartRoom(string protocolVersion)
        {
            if (!string.Equals(protocolVersion, _config.ProtocolVersion, StringComparison.Ordinal)) return NetworkConnectFailure.VersionMismatch;
            _roomStarted = true;
            _matchStarted = false;
            _serverTick = 0;
            _slots.Clear();
            _simulation = null;
            return NetworkConnectFailure.None;
        }

        public NetworkConnectFailure Join(int clientId, string protocolVersion)
        {
            if (!_roomStarted || _matchStarted) return NetworkConnectFailure.Disconnected;
            if (!string.Equals(protocolVersion, _config.ProtocolVersion, StringComparison.Ordinal)) return NetworkConnectFailure.VersionMismatch;
            if (_slots.TryGetValue(clientId, out var existing))
            {
                if (existing.State == ServerSlotState.DisconnectedGrace && !existing.IsBot)
                {
                    existing.State = ServerSlotState.Connected;
                    existing.GraceRemainingTicks = _config.ReconnectGraceTicks;
                    return NetworkConnectFailure.None;
                }

                return NetworkConnectFailure.Disconnected;
            }

            if (_slots.Count >= _config.MaxSlots) return NetworkConnectFailure.RoomFull;
            _slots.Add(clientId, new Slot(clientId, new CombatEntityId(clientId), false));
            return NetworkConnectFailure.None;
        }

        public int FillWithBots()
        {
            if (!_roomStarted || _matchStarted) return 0;
            var added = 0;
            while (_slots.Count < _config.MaxSlots)
            {
                var botId = _nextBotId--;
                _slots.Add(botId, new Slot(botId, new CombatEntityId(botId), true));
                added++;
            }

            return added;
        }

        public bool StartMatch()
        {
            if (!_roomStarted || _matchStarted || _slots.Count < 2) return false;
            var spawns = new MatchSpawn[_slots.Count];
            var index = 0;
            foreach (var slot in _slots.Values)
            {
                var angle = (MathF.PI * 2f * index) / _slots.Count;
                spawns[index++] = new MatchSpawn(slot.ActorId, new Float2(MathF.Cos(angle) * 8f, MathF.Sin(angle) * 8f), 100);
            }

            _simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            _simulation.Start(spawns);
            _matchStarted = true;
            return true;
        }

        public bool TrySubmitInput(NetworkInputFrame input)
        {
            if (!_matchStarted || !_slots.TryGetValue(input.ClientId, out var slot) || slot.IsBot || slot.State != ServerSlotState.Connected || input.Tick <= slot.LastInputTick)
            {
                return false;
            }

            var snapshot = FindSnapshot(slot.ActorId);
            if (!snapshot.Alive) return false;
            var boundedMovement = Float2.ClampMagnitude(input.Movement, 1f) * (2f / _config.TickRate);
            _simulation.SetPosition(slot.ActorId, snapshot.Position + boundedMovement);
            slot.LastInputTick = input.Tick;
            return true;
        }

        public bool ApplyServerAuthorityDamage(int clientId, int amount)
        {
            if (!_matchStarted || !_slots.TryGetValue(clientId, out var slot) || amount <= 0) return false;
            var current = FindSnapshot(slot.ActorId);
            if (!current.Alive || !_simulation.SyncHealth(slot.ActorId, current.CurrentHealth - amount)) return false;
            if (!FindSnapshot(slot.ActorId).Alive) slot.State = ServerSlotState.Eliminated;
            return true;
        }

        public bool Disconnect(int clientId)
        {
            if (!_slots.TryGetValue(clientId, out var slot) || slot.IsBot || slot.State != ServerSlotState.Connected) return false;
            slot.State = ServerSlotState.DisconnectedGrace;
            slot.GraceRemainingTicks = _config.ReconnectGraceTicks;
            return true;
        }

        public bool Reconnect(int clientId, string protocolVersion)
        {
            if (!string.Equals(protocolVersion, _config.ProtocolVersion, StringComparison.Ordinal) || !_slots.TryGetValue(clientId, out var slot) || slot.IsBot || slot.State != ServerSlotState.DisconnectedGrace || slot.GraceRemainingTicks <= 0)
            {
                return false;
            }

            slot.State = ServerSlotState.Connected;
            return true;
        }

        public void Advance(int ticks = 1)
        {
            if (!_matchStarted || ticks < 1) return;
            for (var i = 0; i < ticks; i++)
            {
                _serverTick++;
                _simulation.Advance(1f / _config.TickRate);
                foreach (var slot in _slots.Values)
                {
                    if (slot.State == ServerSlotState.DisconnectedGrace)
                    {
                        slot.GraceRemainingTicks--;
                        if (slot.GraceRemainingTicks <= 0) slot.State = ServerSlotState.BotTakeover;
                    }

                    if (slot.State != ServerSlotState.Eliminated && !FindSnapshot(slot.ActorId).Alive) slot.State = ServerSlotState.Eliminated;
                }
            }
        }

        public MatchParticipantSnapshot[] GetMatchSnapshots() => _simulation == null ? Array.Empty<MatchParticipantSnapshot>() : _simulation.GetSnapshots();

        public MatchParticipantSnapshot GetMatchSnapshot(int clientId)
        {
            if (!_slots.TryGetValue(clientId, out var slot) || _simulation == null) return default(MatchParticipantSnapshot);
            return FindSnapshot(slot.ActorId);
        }

        public ServerSlotSnapshot[] GetSlotSnapshots()
        {
            var snapshots = new ServerSlotSnapshot[_slots.Count];
            var index = 0;
            foreach (var slot in _slots.Values) snapshots[index++] = slot.ToSnapshot();
            return snapshots;
        }

        private MatchParticipantSnapshot FindSnapshot(CombatEntityId actorId)
        {
            var snapshots = _simulation.GetSnapshots();
            for (var i = 0; i < snapshots.Length; i++) if (snapshots[i].Id == actorId) return snapshots[i];
            return default(MatchParticipantSnapshot);
        }

        private sealed class Slot
        {
            public Slot(int clientId, CombatEntityId actorId, bool isBot)
            {
                ClientId = clientId;
                ActorId = actorId;
                IsBot = isBot;
                State = isBot ? ServerSlotState.BotTakeover : ServerSlotState.Connected;
                GraceRemainingTicks = 0;
            }

            public int ClientId;
            public CombatEntityId ActorId;
            public bool IsBot;
            public ServerSlotState State;
            public int LastInputTick;
            public int GraceRemainingTicks;

            public ServerSlotSnapshot ToSnapshot() => new ServerSlotSnapshot(ClientId, ActorId, State, IsBot, LastInputTick, GraceRemainingTicks);
        }
    }
}
