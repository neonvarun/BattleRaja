using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;

namespace BattleRaja.Core.Application
{
    public readonly struct MatchAuthorityTick
    {
        public MatchAuthorityTick(MatchTickResult result, DamageRequest[] outsideDamageRequests)
        {
            Result = result;
            OutsideDamageRequests = outsideDamageRequests ?? Array.Empty<DamageRequest>();
        }

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
        private OfflineMatchSimulation _simulation;
        private double _outsideDamageAccumulator;

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

        public void Start(IReadOnlyList<MatchSpawn> spawns)
        {
            _simulation = new OfflineMatchSimulation(_definition);
            _simulation.Start(spawns);
            _outsideDamageAccumulator = 0d;
        }

        public bool SetPosition(CombatEntityId id, Float2 position) => RequireSimulation().SetPosition(id, position);

        public bool SyncHealth(CombatEntityId id, int currentHealth) => RequireSimulation().SyncHealth(id, currentHealth);

        public MatchAuthorityTick Advance(float fixedDeltaSeconds)
        {
            var simulation = RequireSimulation();
            var result = simulation.Advance(fixedDeltaSeconds);
            if (result.OutsideDamagePerSecond <= 0)
            {
                _outsideDamageAccumulator = 0d;
                return new MatchAuthorityTick(result, Array.Empty<DamageRequest>());
            }

            _outsideDamageAccumulator += fixedDeltaSeconds;
            if (_outsideDamageAccumulator < _outsideDamageTickSeconds)
            {
                return new MatchAuthorityTick(result, Array.Empty<DamageRequest>());
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
                        DamageType.Aandhi));
                }
            }

            return new MatchAuthorityTick(result, requests.ToArray());
        }

        private OfflineMatchSimulation RequireSimulation()
        {
            if (_simulation == null) throw new InvalidOperationException("Start the match authority before using it.");
            return _simulation;
        }
    }
}
