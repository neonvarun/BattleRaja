using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Infrastructure.Networking
{
    public enum NetworkTopology
    {
        ClientServer = 1,
        Shared = 2
    }

    public enum NetworkConnectFailure
    {
        None = 0,
        CredentialsRequired = 1,
        VersionMismatch = 2,
        RoomFull = 3,
        Disconnected = 4
    }

    public readonly struct NetworkSessionConfig
    {
        public NetworkSessionConfig(string protocolVersion, NetworkTopology topology, int maxPlayers, int tickRate)
        {
            ProtocolVersion = protocolVersion ?? string.Empty;
            Topology = topology;
            MaxPlayers = maxPlayers;
            TickRate = tickRate;
        }

        public string ProtocolVersion { get; }
        public NetworkTopology Topology { get; }
        public int MaxPlayers { get; }
        public int TickRate { get; }

        public bool IsValid(out string reason)
        {
            if (string.IsNullOrWhiteSpace(ProtocolVersion) || MaxPlayers < 2 || MaxPlayers > 8 || TickRate < 10 || TickRate > 60)
            {
                reason = "Protocol, player count or tick rate is outside the proof bounds.";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        public static NetworkSessionConfig Proof => new NetworkSessionConfig("m8-proof-v1", NetworkTopology.ClientServer, 2, 30);
    }

    public readonly struct NetworkConditionProfile
    {
        public NetworkConditionProfile(int latencyMilliseconds, int jitterMilliseconds, float packetLoss)
        {
            LatencyMilliseconds = Math.Max(0, latencyMilliseconds);
            JitterMilliseconds = Math.Max(0, jitterMilliseconds);
            PacketLoss = Math.Max(0f, Math.Min(1f, packetLoss));
        }

        public int LatencyMilliseconds { get; }
        public int JitterMilliseconds { get; }
        public float PacketLoss { get; }
        public static NetworkConditionProfile Good => new NetworkConditionProfile(50, 10, 0f);
        public static NetworkConditionProfile Moderate => new NetworkConditionProfile(100, 25, 0.02f);
        public static NetworkConditionProfile Poor => new NetworkConditionProfile(200, 60, 0.05f);
    }

    public readonly struct NetworkInputFrame
    {
        public NetworkInputFrame(int clientId, int tick, Float2 movement, Float2 aim, bool attack, bool ability, ContentId gadgetId)
        {
            ClientId = clientId;
            Tick = tick;
            Movement = movement;
            Aim = aim;
            Attack = attack;
            Ability = ability;
            GadgetId = gadgetId;
        }

        public int ClientId { get; }
        public int Tick { get; }
        public Float2 Movement { get; }
        public Float2 Aim { get; }
        public bool Attack { get; }
        public bool Ability { get; }
        public ContentId GadgetId { get; }
    }

    public readonly struct NetworkActorSnapshot
    {
        public NetworkActorSnapshot(CombatEntityId actorId, Float2 position, int health, bool eliminated, int authoritativeTick)
        {
            ActorId = actorId;
            Position = position;
            Health = health;
            Eliminated = eliminated;
            AuthoritativeTick = authoritativeTick;
        }

        public CombatEntityId ActorId { get; }
        public Float2 Position { get; }
        public int Health { get; }
        public bool Eliminated { get; }
        public int AuthoritativeTick { get; }
    }

    public readonly struct NetworkDiagnostics
    {
        public NetworkDiagnostics(int sent, int delivered, int dropped, int lastTick, NetworkConditionProfile profile)
        {
            SentPackets = sent;
            DeliveredPackets = delivered;
            DroppedPackets = dropped;
            LastAuthoritativeTick = lastTick;
            Profile = profile;
        }

        public int SentPackets { get; }
        public int DeliveredPackets { get; }
        public int DroppedPackets { get; }
        public int LastAuthoritativeTick { get; }
        public NetworkConditionProfile Profile { get; }
    }

    public interface INetworkSessionAdapter
    {
        bool IsAvailable { get; }
        NetworkConnectFailure TryConnect(string roomName, string protocolVersion);
        void Disconnect();
    }

    public sealed class PhotonFusionAdapter : INetworkSessionAdapter
    {
        public bool IsAvailable => false;
        public NetworkConnectFailure TryConnect(string roomName, string protocolVersion) => NetworkConnectFailure.CredentialsRequired;
        public void Disconnect() { }
    }

    public sealed class NetworkSessionMock
    {
        private readonly NetworkSessionConfig _config;
        private readonly Dictionary<int, NetworkActorSnapshot> _actors = new Dictionary<int, NetworkActorSnapshot>();
        private NetworkConditionProfile _conditions = NetworkConditionProfile.Good;
        private int _seed;
        private int _sent;
        private int _delivered;
        private int _dropped;
        private int _lastTick;

        public NetworkSessionMock(NetworkSessionConfig config, int seed = 7)
        {
            _config = config;
            _seed = seed == 0 ? 7 : seed;
        }

        public int ConnectedClients { get; private set; }
        public bool IsStarted { get; private set; }
        public NetworkDiagnostics Diagnostics => new NetworkDiagnostics(_sent, _delivered, _dropped, _lastTick, _conditions);

        public NetworkConnectFailure Start(string protocolVersion)
        {
            if (!_config.IsValid(out _)) return NetworkConnectFailure.VersionMismatch;
            if (!string.Equals(protocolVersion, _config.ProtocolVersion, StringComparison.Ordinal)) return NetworkConnectFailure.VersionMismatch;
            IsStarted = true;
            ConnectedClients = 0;
            _actors.Clear();
            return NetworkConnectFailure.None;
        }

        public NetworkConnectFailure Join(int clientId, string protocolVersion)
        {
            if (!IsStarted) return NetworkConnectFailure.Disconnected;
            if (!string.Equals(protocolVersion, _config.ProtocolVersion, StringComparison.Ordinal)) return NetworkConnectFailure.VersionMismatch;
            if (ConnectedClients >= _config.MaxPlayers) return NetworkConnectFailure.RoomFull;
            ConnectedClients++;
            _actors[clientId] = new NetworkActorSnapshot(new CombatEntityId(clientId), Float2.Zero, 100, false, 0);
            return NetworkConnectFailure.None;
        }

        public void Leave(int clientId)
        {
            if (_actors.Remove(clientId)) ConnectedClients = Math.Max(0, ConnectedClients - 1);
        }

        public bool SubmitInput(NetworkInputFrame input)
        {
            _sent++;
            if (!ShouldDeliver())
            {
                _dropped++;
                return false;
            }

            if (!_actors.TryGetValue(input.ClientId, out var current) || current.Eliminated) return false;
            var movement = Float2.ClampMagnitude(input.Movement, 1f) * 0.2f;
            _lastTick = Math.Max(_lastTick, input.Tick);
            _actors[input.ClientId] = new NetworkActorSnapshot(current.ActorId, current.Position + movement, current.Health, false, _lastTick);
            _delivered++;
            return true;
        }

        public bool ApplyAuthoritativeDamage(int targetClientId, int amount, int tick)
        {
            if (amount <= 0 || !_actors.TryGetValue(targetClientId, out var current) || current.Eliminated) return false;
            var health = Math.Max(0, current.Health - amount);
            _actors[targetClientId] = new NetworkActorSnapshot(current.ActorId, current.Position, health, health == 0, Math.Max(_lastTick, tick));
            _lastTick = Math.Max(_lastTick, tick);
            return true;
        }

        public NetworkActorSnapshot GetSnapshot(int clientId) => _actors.TryGetValue(clientId, out var snapshot) ? snapshot : default(NetworkActorSnapshot);
        public void SetConditions(NetworkConditionProfile profile) => _conditions = profile;

        private bool ShouldDeliver()
        {
            unchecked { _seed = _seed * 1103515245 + 12345; }
            var normalized = (uint)_seed / (float)uint.MaxValue;
            return normalized >= _conditions.PacketLoss;
        }
    }
}
