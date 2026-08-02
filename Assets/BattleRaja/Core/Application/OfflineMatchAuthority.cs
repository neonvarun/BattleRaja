using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public readonly struct MatchAuthorityTick
    {
        public MatchAuthorityTick(MatchTickResult result, DamageRequest[] outsideDamageRequests)
            : this(0, result, outsideDamageRequests)
        {
        }

        public MatchAuthorityTick(int simulationTick, MatchTickResult result, DamageRequest[] outsideDamageRequests)
        {
            SimulationTick = simulationTick;
            Result = result;
            OutsideDamageRequests = outsideDamageRequests ?? Array.Empty<DamageRequest>();
        }

        public int SimulationTick { get; }
        public MatchTickResult Result { get; }
        public DamageRequest[] OutsideDamageRequests { get; }
    }

    /// <summary>
    /// Transport- and Unity-independent owner of offline match authority.
    /// Presentation supplies actor observations and consumes immutable tick intents.
    /// </summary>
    public sealed class OfflineMatchAuthority
    {
        private readonly OfflineMatchDefinition _definition;
        private readonly float _outsideDamageTickSeconds;
        private MatchPickupDefinition[] _pickupDefinitions = Array.Empty<MatchPickupDefinition>();
        private GadgetPickupDefinition[] _gadgetPickupDefinitions = Array.Empty<GadgetPickupDefinition>();
        private MatchPickupRuntime[] _pickups = Array.Empty<MatchPickupRuntime>();
        private GadgetPickupRuntime[] _gadgetPickups = Array.Empty<GadgetPickupRuntime>();
        private readonly Dictionary<CombatEntityId, GadgetInventory> _gadgetInventories = new Dictionary<CombatEntityId, GadgetInventory>();
        private readonly Dictionary<CombatEntityId, GadgetRuntime> _gadgetRuntimes = new Dictionary<CombatEntityId, GadgetRuntime>();
        private OfflineMatchSimulation _simulation;
        private double _outsideDamageAccumulator;
        private int _lastSimulationTick = -1;

        public OfflineMatchAuthority(OfflineMatchDefinition definition, float outsideDamageTickSeconds = 1f)
        {
            if (outsideDamageTickSeconds <= 0f || float.IsNaN(outsideDamageTickSeconds) || float.IsInfinity(outsideDamageTickSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(outsideDamageTickSeconds));
            }

            _definition = definition;
            _outsideDamageTickSeconds = outsideDamageTickSeconds;
        }

        public OfflineMatchSimulation Simulation => _simulation;

        public void ConfigureItems(
            IReadOnlyList<MatchPickupDefinition> pickups,
            IReadOnlyList<GadgetPickupDefinition> gadgetPickups)
        {
            _pickupDefinitions = pickups != null ? Copy(pickups) : Array.Empty<MatchPickupDefinition>();
            _gadgetPickupDefinitions = gadgetPickups != null ? Copy(gadgetPickups) : Array.Empty<GadgetPickupDefinition>();
        }

        public void Start(IReadOnlyList<MatchSpawn> spawns)
        {
            _simulation = new OfflineMatchSimulation(_definition);
            _simulation.Start(spawns);
            _pickups = new MatchPickupRuntime[_pickupDefinitions.Length];
            for (var i = 0; i < _pickupDefinitions.Length; i++)
            {
                _pickups[i] = new MatchPickupRuntime(_pickupDefinitions[i]);
            }

            _gadgetPickups = new GadgetPickupRuntime[_gadgetPickupDefinitions.Length];
            for (var i = 0; i < _gadgetPickupDefinitions.Length; i++)
            {
                _gadgetPickups[i] = new GadgetPickupRuntime(_gadgetPickupDefinitions[i]);
            }

            _gadgetInventories.Clear();
            _gadgetRuntimes.Clear();
            for (var i = 0; i < spawns.Count; i++)
            {
                _gadgetInventories[spawns[i].Id] = new GadgetInventory(1);
                _gadgetRuntimes[spawns[i].Id] = new GadgetRuntime();
            }

            _outsideDamageAccumulator = 0d;
            _lastSimulationTick = -1;
        }

        public bool SetPosition(CombatEntityId id, Float2 position) => RequireSimulation().SetPosition(id, position);

        public bool SyncHealth(CombatEntityId id, int currentHealth) => RequireSimulation().SyncHealth(id, currentHealth);

        public MatchAuthorityTick Advance(float fixedDeltaSeconds)
        {
            return Advance(_lastSimulationTick + 1, fixedDeltaSeconds);
        }

        public MatchAuthorityTick Advance(int simulationTick, float fixedDeltaSeconds)
        {
            if (simulationTick < 0 || simulationTick <= _lastSimulationTick)
            {
                throw new ArgumentOutOfRangeException(nameof(simulationTick), "Simulation ticks must increase monotonically.");
            }

            if (fixedDeltaSeconds <= 0f || float.IsNaN(fixedDeltaSeconds) || float.IsInfinity(fixedDeltaSeconds))
            {
                throw new ArgumentOutOfRangeException(nameof(fixedDeltaSeconds));
            }

            _lastSimulationTick = simulationTick;
            var simulation = RequireSimulation();
            for (var i = 0; i < _pickups.Length; i++) _pickups[i].Advance(fixedDeltaSeconds);
            foreach (var runtime in _gadgetRuntimes.Values) runtime.Advance(fixedDeltaSeconds);
            var result = simulation.Advance(fixedDeltaSeconds);
            if (result.OutsideDamagePerSecond <= 0)
            {
                _outsideDamageAccumulator = 0d;
                return new MatchAuthorityTick(simulationTick, result, Array.Empty<DamageRequest>());
            }

            _outsideDamageAccumulator += fixedDeltaSeconds;
            if (_outsideDamageAccumulator < _outsideDamageTickSeconds)
            {
                return new MatchAuthorityTick(simulationTick, result, Array.Empty<DamageRequest>());
            }

            var requests = new List<DamageRequest>(result.OutsideCount);
            while (_outsideDamageAccumulator >= _outsideDamageTickSeconds)
            {
                _outsideDamageAccumulator -= _outsideDamageTickSeconds;
                var snapshots = simulation.GetSnapshots();
                for (var i = 0; i < snapshots.Length; i++)
                {
                    var snapshot = snapshots[i];
                    if (!snapshot.Alive || Float2.Distance(snapshot.Position, result.ZoneCenter) <= result.ZoneRadius) continue;
                    requests.Add(new DamageRequest(
                        new CombatEntityId(-99),
                        snapshot.Id,
                        CombatFaction.Neutral,
                        result.OutsideDamagePerSecond,
                        DamageType.Aandhi,
                        Float2.Zero,
                        simulationTick));
                }
            }

            return new MatchAuthorityTick(simulationTick, result, requests.ToArray());
        }

        public MatchPickupCollectResult TryCollectPickup(int pickupId, int currentHealth, int maxHealth)
        {
            if (pickupId < 0 || pickupId >= _pickups.Length) return new MatchPickupCollectResult(false, 0);
            return _pickups[pickupId].TryCollect(currentHealth, maxHealth);
        }

        public bool IsPickupAvailable(int pickupId)
        {
            return pickupId >= 0 && pickupId < _pickups.Length && _pickups[pickupId].IsAvailable;
        }

        public GadgetPickupCollectResult TryCollectGadget(int pickupId, bool hasGadget)
        {
            if (pickupId < 0 || pickupId >= _gadgetPickups.Length)
            {
                return new GadgetPickupCollectResult(false, default(ContentId));
            }

            return _gadgetPickups[pickupId].TryCollect(hasGadget);
        }

        public GadgetPickupCollectResult TryCollectGadget(CombatEntityId collectorId, int pickupId)
        {
            if (pickupId < 0 || pickupId >= _gadgetPickups.Length || !_gadgetInventories.TryGetValue(collectorId, out var inventory))
            {
                return new GadgetPickupCollectResult(false, default(ContentId));
            }

            var result = _gadgetPickups[pickupId].TryCollect(inventory.HasGadget);
            if (result.Collected) inventory.TryPickup(result.GadgetId);
            return result;
        }

        public GadgetUseResult TryUseGadget(GadgetUseCommand command)
        {
            if (!_gadgetInventories.TryGetValue(command.UserId, out var inventory) ||
                !_gadgetRuntimes.TryGetValue(command.UserId, out var runtime))
            {
                return new GadgetUseResult(false, GadgetUseFailure.NotHeld, default(GadgetEffect));
            }

            return runtime.TryUse(inventory, command);
        }

        public bool TryAcquireGadget(CombatEntityId collectorId, ContentId gadgetId)
        {
            return _gadgetInventories.TryGetValue(collectorId, out var inventory) &&
                GadgetCatalog.TryGet(gadgetId, out _) && inventory.TryPickup(gadgetId);
        }

        public bool IsGadgetPickupAvailable(int pickupId)
        {
            return pickupId >= 0 && pickupId < _gadgetPickups.Length && _gadgetPickups[pickupId].IsAvailable;
        }

        private static T[] Copy<T>(IReadOnlyList<T> source)
        {
            var copy = new T[source.Count];
            for (var i = 0; i < source.Count; i++) copy[i] = source[i];
            return copy;
        }

        private OfflineMatchSimulation RequireSimulation()
        {
            if (_simulation == null) throw new InvalidOperationException("Start the match authority before using it.");
            return _simulation;
        }
    }
}
