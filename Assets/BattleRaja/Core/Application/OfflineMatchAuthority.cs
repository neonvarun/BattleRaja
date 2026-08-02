using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public readonly struct MatchAuthorityTick
    {
        public MatchAuthorityTick(MatchTickResult result, DamageRequest[] outsideDamageRequests)
            : this(0, result, outsideDamageRequests, Array.Empty<GadgetHealingIntent>(), Array.Empty<int>())
        {
        }

        public MatchAuthorityTick(int simulationTick, MatchTickResult result, DamageRequest[] outsideDamageRequests)
            : this(simulationTick, result, outsideDamageRequests, Array.Empty<GadgetHealingIntent>(), Array.Empty<int>())
        {
        }

        public MatchAuthorityTick(
            int simulationTick,
            MatchTickResult result,
            DamageRequest[] outsideDamageRequests,
            GadgetHealingIntent[] gadgetHealingIntents,
            int[] expiredStationIds)
        {
            SimulationTick = simulationTick;
            Result = result;
            OutsideDamageRequests = outsideDamageRequests ?? Array.Empty<DamageRequest>();
            GadgetHealingIntents = gadgetHealingIntents ?? Array.Empty<GadgetHealingIntent>();
            ExpiredStationIds = expiredStationIds ?? Array.Empty<int>();
        }

        public int SimulationTick { get; }
        public MatchTickResult Result { get; }
        public DamageRequest[] OutsideDamageRequests { get; }
        public GadgetHealingIntent[] GadgetHealingIntents { get; }
        public int[] ExpiredStationIds { get; }
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
        private readonly Dictionary<int, GadgetStationRuntime> _stations = new Dictionary<int, GadgetStationRuntime>();
        private OfflineMatchSimulation _simulation;
        private double _outsideDamageAccumulator;
        private int _lastSimulationTick = -1;
        private int _nextStationId = 1;

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
            _stations.Clear();
            for (var i = 0; i < spawns.Count; i++)
            {
                _gadgetInventories[spawns[i].Id] = new GadgetInventory(1);
                _gadgetRuntimes[spawns[i].Id] = new GadgetRuntime();
            }

            _outsideDamageAccumulator = 0d;
            _lastSimulationTick = -1;
            _nextStationId = 1;
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
            var healingIntents = new List<GadgetHealingIntent>();
            var expiredStationIds = new List<int>();
            AdvanceStations(fixedDeltaSeconds, simulation.GetSnapshots(), healingIntents, expiredStationIds);
            if (result.OutsideDamagePerSecond <= 0)
            {
                _outsideDamageAccumulator = 0d;
                return new MatchAuthorityTick(
                    simulationTick,
                    result,
                    Array.Empty<DamageRequest>(),
                    healingIntents.ToArray(),
                    expiredStationIds.ToArray());
            }

            _outsideDamageAccumulator += fixedDeltaSeconds;
            if (_outsideDamageAccumulator < _outsideDamageTickSeconds)
            {
                return new MatchAuthorityTick(
                    simulationTick,
                    result,
                    Array.Empty<DamageRequest>(),
                    healingIntents.ToArray(),
                    expiredStationIds.ToArray());
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

            return new MatchAuthorityTick(
                simulationTick,
                result,
                requests.ToArray(),
                healingIntents.ToArray(),
                expiredStationIds.ToArray());
        }

        /// <summary>
        /// Resolves pickup proximity and collector selection from the authoritative
        /// simulation snapshot. Unity only applies the returned intents to its views.
        /// </summary>
        public MatchAuthorityCollections CollectNearby()
        {
            var simulation = RequireSimulation();
            var snapshots = simulation.GetSnapshots();
            var pickupCollections = new List<MatchPickupCollectionIntent>(_pickups.Length);
            var gadgetCollections = new List<GadgetPickupCollectionIntent>(_gadgetPickups.Length);

            for (var i = 0; i < _pickups.Length; i++)
            {
                var runtime = _pickups[i];
                if (!runtime.IsAvailable) continue;
                var definition = runtime.Definition;
                if (!TrySelectCollector(snapshots, definition.Position, definition.CollectionRadius, true, out var collector)) continue;
                var result = runtime.TryCollect(collector.CurrentHealth, collector.MaxHealth);
                if (result.Collected)
                {
                    pickupCollections.Add(new MatchPickupCollectionIntent(
                        definition.PickupId,
                        collector.Id,
                        result.HealAmount));
                }
            }

            for (var i = 0; i < _gadgetPickups.Length; i++)
            {
                var runtime = _gadgetPickups[i];
                if (!runtime.IsAvailable) continue;
                var definition = runtime.Definition;
                if (!TrySelectCollector(snapshots, definition.Position, definition.CollectionRadius, false, out var collector) ||
                    !_gadgetInventories.TryGetValue(collector.Id, out var inventory) || inventory.HasGadget) continue;
                var result = runtime.TryCollect(false);
                if (result.Collected && inventory.TryPickup(result.GadgetId))
                {
                    gadgetCollections.Add(new GadgetPickupCollectionIntent(
                        definition.PickupId,
                        collector.Id,
                        result.GadgetId));
                }
            }

            return new MatchAuthorityCollections(pickupCollections.ToArray(), gadgetCollections.ToArray());
        }

        public MatchPickupCollectResult TryCollectPickup(int pickupId, int currentHealth, int maxHealth)
        {
            var index = FindPickupIndex(pickupId);
            return index < 0
                ? new MatchPickupCollectResult(false, 0)
                : _pickups[index].TryCollect(currentHealth, maxHealth);
        }

        public bool IsPickupAvailable(int pickupId)
        {
            var index = FindPickupIndex(pickupId);
            return index >= 0 && _pickups[index].IsAvailable;
        }

        public GadgetPickupCollectResult TryCollectGadget(int pickupId, bool hasGadget)
        {
            var index = FindGadgetPickupIndex(pickupId);
            if (index < 0)
            {
                return new GadgetPickupCollectResult(false, default(ContentId));
            }

            return _gadgetPickups[index].TryCollect(hasGadget);
        }

        public GadgetPickupCollectResult TryCollectGadget(CombatEntityId collectorId, int pickupId)
        {
            var index = FindGadgetPickupIndex(pickupId);
            if (index < 0 || !_gadgetInventories.TryGetValue(collectorId, out var inventory))
            {
                return new GadgetPickupCollectResult(false, default(ContentId));
            }

            var result = _gadgetPickups[index].TryCollect(inventory.HasGadget);
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

            var result = runtime.TryUse(inventory, command);
            if (!result.Used)
            {
                return result;
            }

            var effect = result.Effect;
            if (effect.Kind == GadgetEffectKind.DholBurst)
            {
                var snapshots = RequireSimulation().GetSnapshots();
                var displacements = new List<GadgetDisplacementIntent>(snapshots.Length);
                for (var i = 0; i < snapshots.Length; i++)
                {
                    var snapshot = snapshots[i];
                    if (!snapshot.Alive || snapshot.Id == command.UserId) continue;
                    var delta = snapshot.Position - command.Origin;
                    if (delta.SqrMagnitude > effect.Definition.Radius * effect.Definition.Radius) continue;
                    displacements.Add(new GadgetDisplacementIntent(
                        snapshot.Id,
                        delta.Normalized * (effect.Definition.Magnitude * 0.08f)));
                }

                effect = new GadgetEffect(
                    effect.Kind,
                    effect.Definition,
                    effect.Command,
                    displacements.ToArray());
            }
            else if (effect.Kind == GadgetEffectKind.TiffinStation)
            {
                var stationId = _nextStationId++;
                _stations[stationId] = new GadgetStationRuntime(stationId, command.Origin, effect.Definition);
                effect = new GadgetEffect(
                    effect.Kind,
                    effect.Definition,
                    effect.Command,
                    effect.Displacements,
                    stationId);
            }

            return new GadgetUseResult(true, GadgetUseFailure.None, effect);
        }

        public bool TryAcquireGadget(CombatEntityId collectorId, ContentId gadgetId)
        {
            return _gadgetInventories.TryGetValue(collectorId, out var inventory) &&
                GadgetCatalog.TryGet(gadgetId, out _) && inventory.TryPickup(gadgetId);
        }

        public bool IsGadgetPickupAvailable(int pickupId)
        {
            var index = FindGadgetPickupIndex(pickupId);
            return index >= 0 && _gadgetPickups[index].IsAvailable;
        }

        private void AdvanceStations(
            float fixedDeltaSeconds,
            MatchParticipantSnapshot[] snapshots,
            List<GadgetHealingIntent> healingIntents,
            List<int> expiredStationIds)
        {
            if (_stations.Count == 0) return;
            var expired = new List<int>();
            foreach (var pair in _stations)
            {
                var step = pair.Value.Advance(fixedDeltaSeconds, snapshots);
                for (var i = 0; i < step.Healing.Length; i++) healingIntents.Add(step.Healing[i]);
                if (step.Expired) expired.Add(pair.Key);
            }

            for (var i = 0; i < expired.Count; i++)
            {
                _stations.Remove(expired[i]);
                expiredStationIds.Add(expired[i]);
            }
        }

        private static T[] Copy<T>(IReadOnlyList<T> source)
        {
            var copy = new T[source.Count];
            for (var i = 0; i < source.Count; i++) copy[i] = source[i];
            return copy;
        }

        private static bool TrySelectCollector(
            MatchParticipantSnapshot[] snapshots,
            Float2 position,
            float collectionRadius,
            bool requireHealth,
            out MatchParticipantSnapshot selected)
        {
            selected = default(MatchParticipantSnapshot);
            var found = false;
            var radiusSquared = collectionRadius * collectionRadius;
            for (var i = 0; i < snapshots.Length; i++)
            {
                var candidate = snapshots[i];
                if (!candidate.Alive || candidate.Position.SqrMagnitudeFrom(position) > radiusSquared ||
                    (requireHealth && candidate.CurrentHealth >= candidate.MaxHealth)) continue;
                if (!found || candidate.Id.Value < selected.Id.Value)
                {
                    selected = candidate;
                    found = true;
                }
            }

            return found;
        }

        private int FindPickupIndex(int pickupId)
        {
            for (var i = 0; i < _pickups.Length; i++)
            {
                if (_pickups[i].Definition.PickupId == pickupId) return i;
            }

            return -1;
        }

        private int FindGadgetPickupIndex(int pickupId)
        {
            for (var i = 0; i < _gadgetPickups.Length; i++)
            {
                if (_gadgetPickups[i].Definition.PickupId == pickupId) return i;
            }

            return -1;
        }

        private OfflineMatchSimulation RequireSimulation()
        {
            if (_simulation == null) throw new InvalidOperationException("Start the match authority before using it.");
            return _simulation;
        }
    }
}
