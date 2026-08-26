#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleRaja.Presentation.AI
{
    [Serializable]
    public sealed class AutonomousBotParticipantResult
    {
        public int ActorId;
        public string Fighter = "Unknown";
        public int Placement;
        public int DamageDealt;
        public int Eliminations;
        public int Assists;
        public float SurvivalTimeSeconds;
        public int DecisionCount;
        public int TargetDecisionCount;
        public int EngageDecisionCount;
        public int AttackDecisionCount;
        public int AttackAttempts;
        public int AcceptedAttacks;
        public int RejectedAttacks;
        public int OutOfRangeAttackAttempts;
        public int ProjectileHits;
        public int AbilityAttempts;
        public int AcceptedAbilities;
        public int RejectedAbilities;
        public int EffectiveAbilities;
        public int TargetSwitches;
        public int StuckRecoveries;
        public int ZoneSafetyDecisions;
        public int MaxContinuousStuckTicks;
        public float MaxStuckPositionX;
        public float MaxStuckPositionZ;
        public int MaxStuckSimulationTick;
        public int MaxVisibleTargets;
        public int MaxHostileTargets;
        public int HostileCaptureCount;
        public double MaxDecisionMilliseconds;
        public int GadgetPickups;
        public int GadgetUseAttempts;
        public int SuccessfulGadgetUses;
        public int SuccessfulUmbrellaGuardUses;
        public int SuccessfulDholBurstUses;
        public int SuccessfulTiffinStationUses;
        public int FailedGadgetUses;
        public string CommandDigest;
        public int CommandCount;
        public int AuthorityCommandQueuedCount;
        public int AuthorityCommandRejectedCount;
        public int AuthorityCommandFallbackCount;
    }

    [Serializable]
    public sealed class AutonomousBotMatchResult
    {
        public uint Seed;
        public bool CompletedWithinTickBudget;
        public float DurationSeconds;
        public float FirstDamageSeconds = -1f;
        public float FirstEliminationSeconds = -1f;
        public float FinalThreeSeconds = -1f;
        public int CombatEliminations;
        public int AandhiEliminations;
        public int ProtectedWarmupDamageEvents;
        public int InvalidPositionSamples;
        public int UniqueDamagingPairs;
        public int BotToBotDamagingPairs;
        public int BotToBotDamageEvents;
        public int AttackAttempts;
        public int AcceptedAttacks;
        public int RejectedAttacks;
        public int OutOfRangeAttackAttempts;
        public int ProjectileHits;
        public int AbilityAttempts;
        public int AcceptedAbilities;
        public int RejectedAbilities;
        public int EffectiveAbilities;
        public int EffectiveDashSteps;
        public int TargetSwitches;
        public int StuckRecoveries;
        public int ZoneSafetyDecisions;
        public int MaxContinuousStuckTicks;
        public int MaxOutsideParticipants;
        public int OutsideParticipantTicks;
        public int GadgetPickups;
        public int ContextualGadgetUses;
        public int SuccessfulGadgetUses;
        public int SuccessfulUmbrellaGuardUses;
        public int SuccessfulDholBurstUses;
        public int SuccessfulTiffinStationUses;
        public int FailedGadgetUses;
        public double MaxDecisionMilliseconds;
        public string CommandDigest;
        public int CommandCount;
        public List<AutonomousBotParticipantResult> Participants =
            new List<AutonomousBotParticipantResult>();
    }

    [Serializable]
    public sealed class AutonomousBotBatchReport
    {
        public int SchemaVersion = 1;
        public string SceneName;
        public string UnityVersion;
        public uint BaseSeed;
        public float PlaybackScale;
        public string CapturedAtUtc;
        public List<AutonomousBotMatchResult> Matches = new List<AutonomousBotMatchResult>();
    }

    /// <summary>
    /// Runs complete matches through the production scene, perception, decision and
    /// authority pipeline. The runner is diagnostic-only and is not added to product scenes.
    /// </summary>
    public sealed class ProductionBotMatchHarness : MonoBehaviour
    {
        private const float FixedStepSeconds = 1f / 30f;
        private const int MaximumTicks = 10_800;
        private const string SelectedFighterKey = "battleraja.selected_fighter";

        private readonly HashSet<(int InstigatorId, int TargetId)> _damagingPairs =
            new HashSet<(int InstigatorId, int TargetId)>();
        private readonly HashSet<(int InstigatorId, int TargetId)> _botToBotDamagingPairs =
            new HashSet<(int InstigatorId, int TargetId)>();
        private readonly Dictionary<int, int> _projectileHitsByActor = new Dictionary<int, int>();
        private AutonomousBotMatchResult _currentResult;
        private OfflineMatchController _match;
        private string _reportRunId;
        private float _reportPlaybackScale;

        public List<AutonomousBotMatchResult> Results { get; } = new List<AutonomousBotMatchResult>();
        public string LastReportPath { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator RunMatches(int matchCount, uint baseSeed, float playbackScale = 50f)
        {
            if (matchCount <= 0) throw new ArgumentOutOfRangeException(nameof(matchCount));
            if (playbackScale < 1f || playbackScale > 90f)
            {
                throw new ArgumentOutOfRangeException(nameof(playbackScale));
            }

            Results.Clear();
            var previousTimeScale = Time.timeScale;
            var hadSelectedFighter = PlayerPrefs.HasKey(SelectedFighterKey);
            var previousSelectedFighter = PlayerPrefs.GetInt(SelectedFighterKey, 0);
            var previousScene = SceneManager.GetActiveScene();
            var previousSceneName = previousScene.IsValid() ? previousScene.name : string.Empty;
            var restorePreviousScene = !string.IsNullOrEmpty(previousSceneName) &&
                !string.Equals(previousSceneName, "BazaarBastion", StringComparison.Ordinal) &&
                Application.CanStreamedLevelBeLoaded(previousSceneName);
            _reportRunId = DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff");
            LastReportPath = null;
            _reportPlaybackScale = playbackScale;
            PlayerPrefs.SetInt(SelectedFighterKey, 0);
            PlayerPrefs.Save();
            Time.timeScale = 0f;
            try
            {
                for (var index = 0; index < matchCount; index++)
                {
                    var seed = baseSeed + (uint)index;
                    yield return RunMatch(seed);
                    yield return null;
                }

                WriteBatchReport(baseSeed, playbackScale);
                Debug.Log($"[ProductionBotMatchHarness] {Summarize(Results)}{SummarizeFighters(Results)}");
            }
            finally
            {
                OfflineMatchController.SuppressAutomaticSimulationForHarness = false;
                Time.timeScale = previousTimeScale;
                if (hadSelectedFighter)
                {
                    PlayerPrefs.SetInt(SelectedFighterKey, previousSelectedFighter);
                }
                else
                {
                    PlayerPrefs.DeleteKey(SelectedFighterKey);
                }

                PlayerPrefs.Save();
            }

            if (restorePreviousScene)
            {
                var restore = SceneManager.LoadSceneAsync(previousSceneName, LoadSceneMode.Single);
                while (restore != null && !restore.isDone) yield return null;
            }
        }

        private IEnumerator RunMatch(uint seed)
        {
            // Scene activation can render one or more frames before this coroutine
            // resumes. Freeze scaled time during that handoff so unconfigured bots
            // cannot consume local simulation ticks or move their presentation views.
            Time.timeScale = 0f;
            OfflineMatchController.SuppressAutomaticStartForHarnessSceneName = "BazaarBastion";
            var load = SceneManager.LoadSceneAsync("BazaarBastion", LoadSceneMode.Single);
            while (load != null && !load.isDone) yield return null;

            _match = FindAnyObjectByType<OfflineMatchController>();
            if (_match == null) throw new InvalidOperationException("Bazaar Bastion has no match controller.");

            var actors = FindObjectsByType<MovementPlayerAgent>()
                .Where(agent => agent.GetComponent<CombatTarget>() != null)
                .OrderBy(agent => agent.ActorId)
                .ToArray();
            if (actors.Length != 8) throw new InvalidOperationException($"Expected 8 actors, found {actors.Length}.");

            foreach (var actor in actors)
            {
                var input = actor.GetComponent<PlayerInputAdapter>();
                if (input != null) input.enabled = false;
            }

            var referenceBrain = FindAnyObjectByType<BotBrain>();
            if (referenceBrain == null) throw new InvalidOperationException("Production scene has no bot brain template.");

            ConfigurePlayerAsBot(actors[0], referenceBrain.AutonomousWeaponAsset, seed);

            OfflineMatchController.SuppressAutomaticSimulationForHarness = true;
            _match.StartMatch();
            foreach (var actor in actors)
            {
                actor.GetComponent<BotPerceptionSensor>()?.RefreshTargets();
                actor.GetComponent<BotPerceptionSensor>()?.RefreshGadgetPickups();
            }
            var brains = FindObjectsByType<BotBrain>().OrderBy(bot => bot.GetComponent<MovementPlayerAgent>().ActorId);
            foreach (var brain in brains)
            {
                brain.ConfigureForAutonomousMatch(seed);
            }

            _currentResult = new AutonomousBotMatchResult { Seed = seed };
            _damagingPairs.Clear();
            _botToBotDamagingPairs.Clear();
            _projectileHitsByActor.Clear();
            _match.AuthorityTickResolved += OnAuthorityTickResolved;
            try
            {
                while (!_match.Simulation.IsEnded && _match.SimulationTick < MaximumTicks)
                {
                    // Batch a bounded number of fixed ticks per frame. The controller
                    // owns all gameplay ordering; yielding only lets Unity service the
                    // scene lifecycle without feeding render delta time into the
                    // simulation.
                    for (var tick = 0; tick < 300 && !_match.Simulation.IsEnded &&
                             _match.SimulationTick < MaximumTicks; tick++)
                    {
                        _match.AdvanceHarnessSimulationTick();
                    }
                    yield return null;
                }
            }
            finally
            {
                _match.AuthorityTickResolved -= OnAuthorityTickResolved;
            }

            _currentResult.CompletedWithinTickBudget = _match.Simulation.IsEnded;
            _currentResult.DurationSeconds = _match.Simulation.ElapsedSeconds;
            CollectFinalState(_currentResult, actors);
            CollectComponentTelemetry(_currentResult, brains.ToArray());
            WriteMatchReport(_currentResult, seed);
            Results.Add(_currentResult);
            _currentResult = null;
        }

        private static void ConfigurePlayerAsBot(MovementPlayerAgent player, ProjectileWeaponAsset weaponAsset, uint seed)
        {
            var perception = player.GetComponent<BotPerceptionSensor>();
            if (perception == null) perception = player.gameObject.AddComponent<BotPerceptionSensor>();
            var gadgetUser = player.GetComponent<GadgetUser>();
            if (gadgetUser != null) gadgetUser.AutonomousBotControlled = true;

            var brain = player.GetComponent<BotBrain>();
            if (brain == null) brain = player.gameObject.AddComponent<BotBrain>();
            var attack = player.GetComponent<CombatAttackController>();
            brain.ConfigureForAutonomousMatch(
                seed,
                attack != null ? attack.AuthorityWeaponDefinition : ProjectileWeaponDefinition.TrainingBolt);
            brain.SetHarnessAttackCadenceMultiplier(25f);
        }

        private static string Summarize(IReadOnlyList<AutonomousBotMatchResult> results)
        {
            var completed = results.Count(item => item.CompletedWithinTickBudget);
            var duration = results.Count > 0 ? results.Average(item => item.DurationSeconds) : 0f;
            var firstDamageSamples = results.Where(item => item.FirstDamageSeconds >= 0f)
                .Select(item => item.FirstDamageSeconds).ToArray();
            var firstEliminationSamples = results.Where(item => item.FirstEliminationSeconds >= 0f)
                .Select(item => item.FirstEliminationSeconds).ToArray();
            var finalThreeSamples = results.Where(item => item.FinalThreeSeconds >= 0f)
                .Select(item => item.FinalThreeSeconds).ToArray();
            var firstDamage = firstDamageSamples.Length > 0 ? firstDamageSamples.Average() : -1f;
            var firstElimination = firstEliminationSamples.Length > 0 ? firstEliminationSamples.Average() : -1f;
            var finalThree = finalThreeSamples.Length > 0 ? finalThreeSamples.Average() : -1f;
            return $"matches={results.Count} completed={completed} durationAvg={duration:0.0}s " +
                $"firstDamage={firstDamage:0.0}s firstElimination={firstElimination:0.0}s " +
                $"finalThree={finalThree:0.0}s combatKOs={results.Sum(item => item.CombatEliminations)} " +
                $"botToBotPairs={results.Sum(item => item.BotToBotDamagingPairs)} " +
                $"aandhiKOs={results.Sum(item => item.AandhiEliminations)} attacks={results.Sum(item => item.AcceptedAttacks)}/" +
                $"{results.Sum(item => item.AttackAttempts)} outOfRange={results.Sum(item => item.OutOfRangeAttackAttempts)} " +
                $"hits={results.Sum(item => item.ProjectileHits)} " +
                $"abilities={results.Sum(item => item.AcceptedAbilities)}/{results.Sum(item => item.AbilityAttempts)} " +
                $"effective={results.Sum(item => item.EffectiveAbilities) + results.Sum(item => item.EffectiveDashSteps)} " +
                $"gadgets={results.Sum(item => item.SuccessfulGadgetUses)}/{results.Sum(item => item.ContextualGadgetUses)} " +
                $"gadgetKinds={results.Sum(item => item.SuccessfulUmbrellaGuardUses)}/" +
                $"{results.Sum(item => item.SuccessfulDholBurstUses)}/{results.Sum(item => item.SuccessfulTiffinStationUses)} " +
                $"failedGadgets={results.Sum(item => item.FailedGadgetUses)} " +
                $"targetSwitches={results.Sum(item => item.TargetSwitches)} stuckRecoveries={results.Sum(item => item.StuckRecoveries)} " +
                $"zoneDecisions={results.Sum(item => item.ZoneSafetyDecisions)} maxStuckTicks={results.Max(item => item.MaxContinuousStuckTicks)} " +
                $"maxOutside={results.Max(item => item.MaxOutsideParticipants)} outsideTicks={results.Sum(item => item.OutsideParticipantTicks)} " +
                $"maxDecisionMs={results.Max(item => item.MaxDecisionMilliseconds):0.000}";
        }

        private static string SummarizeFighters(IReadOnlyList<AutonomousBotMatchResult> results)
        {
            var participants = results.SelectMany(item => item.Participants);
            var groups = participants.GroupBy(item => item.Fighter).OrderBy(item => item.Key);
            var output = new System.Text.StringBuilder();
            foreach (var group in groups)
            {
                var items = group.ToArray();
                output.Append($" | {group.Key}");
                output.Append($" actors={items.Length}");
                output.Append($" wins={items.Count(item => item.Placement == 1)}");
                output.Append($" top3={items.Count(item => item.Placement <= 3)}");
                output.Append($" damage={items.Average(item => item.DamageDealt):0.0}");
                output.Append($" KOs={items.Average(item => item.Eliminations):0.00}");
                output.Append($" survival={items.Average(item => item.SurvivalTimeSeconds):0.0}s");
                output.Append($" attacks={items.Sum(item => item.AcceptedAttacks)}/{items.Sum(item => item.AttackAttempts)}");
                output.Append($" abilities={items.Sum(item => item.AcceptedAbilities)}/{items.Sum(item => item.AbilityAttempts)}");
                output.Append($" gadgets={items.Sum(item => item.SuccessfulGadgetUses)}/{items.Sum(item => item.GadgetUseAttempts)}");
            }

            return output.ToString();
        }

        private void OnAuthorityTickResolved(MatchAuthorityTick tick)
        {
            if (_currentResult == null) return;
            var elapsed = tick.SimulationTick * FixedStepSeconds;
            var snapshots = _match.Simulation.GetSnapshots();
            var collisionDefinition = _match.CollisionDefinition;
            for (var i = 0; i < snapshots.Length; i++)
            {
                var position = snapshots[i].Position;
                if (!position.IsFinite || collisionDefinition.IsPointBlocked(position))
                {
                    _currentResult.InvalidPositionSamples++;
                }
            }

            for (var i = 0; i < tick.DamageEvents.Length; i++)
            {
                var damageEvent = tick.DamageEvents[i];
                if (damageEvent.AmountApplied <= 0) continue;
                if (tick.Result.Phase <= MatchPhase.SpawnProtection)
                {
                    _currentResult.ProtectedWarmupDamageEvents++;
                }
                if (_currentResult.FirstDamageSeconds < 0f) _currentResult.FirstDamageSeconds = elapsed;
                if (damageEvent.InstigatorId.Value != 0 && damageEvent.TargetId.Value != 0)
                {
                    _damagingPairs.Add((damageEvent.InstigatorId.Value, damageEvent.TargetId.Value));
                    if (damageEvent.InstigatorId.Value > 1 && damageEvent.TargetId.Value > 1)
                    {
                        _botToBotDamagingPairs.Add((damageEvent.InstigatorId.Value, damageEvent.TargetId.Value));
                        _currentResult.BotToBotDamageEvents++;
                    }
                }

                if (damageEvent.DamageType == DamageType.Projectile)
                {
                    _currentResult.ProjectileHits++;
                    var instigatorId = damageEvent.InstigatorId.Value;
                    if (instigatorId != 0)
                    {
                        _projectileHitsByActor.TryGetValue(instigatorId, out var hits);
                        _projectileHitsByActor[instigatorId] = hits + 1;
                    }
                }

                if (damageEvent.TargetDefeated)
                {
                    if (damageEvent.DamageType == DamageType.Aandhi)
                    {
                        _currentResult.AandhiEliminations++;
                    }
                    else
                    {
                        _currentResult.CombatEliminations++;
                    }
                }
            }

            _currentResult.MaxOutsideParticipants = Math.Max(
                _currentResult.MaxOutsideParticipants,
                tick.Result.OutsideCount);
            _currentResult.OutsideParticipantTicks += tick.Result.OutsideCount;

            if (_currentResult.FirstEliminationSeconds < 0f && tick.DamageEvents.Any(item => item.TargetDefeated))
            {
                _currentResult.FirstEliminationSeconds = elapsed;
            }

            if (_currentResult.FinalThreeSeconds < 0f && _match.Simulation.AliveCount <= 3)
            {
                _currentResult.FinalThreeSeconds = elapsed;
            }

            for (var i = 0; i < tick.BijliDashSteps.Length; i++)
            {
                if (tick.BijliDashSteps[i].Accepted &&
                    tick.BijliDashSteps[i].Displacement.Displacement.SqrMagnitude > 0f)
                {
                    _currentResult.EffectiveDashSteps++;
                }
            }

            _currentResult.UniqueDamagingPairs = _damagingPairs.Count;
            _currentResult.BotToBotDamagingPairs = _botToBotDamagingPairs.Count;
        }

        private void CollectFinalState(
            AutonomousBotMatchResult result,
            IReadOnlyList<MovementPlayerAgent> agents)
        {
            var snapshots = FindAnyObjectByType<OfflineMatchController>().Simulation.GetSnapshots();
            for (var i = 0; i < agents.Count; i++)
            {
                var actorId = agents[i].ActorId;
                var snapshot = snapshots.FirstOrDefault(item => item.Id.Value == actorId);
                result.Participants.Add(new AutonomousBotParticipantResult
                {
                    ActorId = actorId,
                    Fighter = ResolveFighter(agents[i]),
                    Placement = snapshot.Placement,
                    DamageDealt = snapshot.DamageDealt,
                    Eliminations = snapshot.Eliminations,
                    Assists = snapshot.Assists,
                    SurvivalTimeSeconds = snapshot.SurvivalTimeSeconds,
                    ProjectileHits = _projectileHitsByActor.TryGetValue(actorId, out var projectileHits)
                        ? projectileHits
                        : 0
                });
            }
        }

        private static void CollectComponentTelemetry(
            AutonomousBotMatchResult result,
            BotBrain[] bots)
        {
            var attacks = FindObjectsByType<CombatAttackController>();
            var abilities = FindObjectsByType<MonoBehaviour>();
            var gadgets = FindObjectsByType<GadgetUser>();
            for (var i = 0; i < attacks.Length; i++)
            {
                var agent = attacks[i].GetComponent<MovementPlayerAgent>();
                var brain = attacks[i].GetComponent<BotBrain>();
                if (agent == null) continue;
                var participant = FindParticipant(result, agent.ActorId);
                participant.AttackAttempts = brain != null ? brain.AttackAttemptCount : 0;
                participant.OutOfRangeAttackAttempts = brain != null ? brain.OutOfRangeAttackAttemptCount : 0;
                participant.AcceptedAttacks = attacks[i].AcceptedAttackCount;
                participant.RejectedAttacks = attacks[i].RejectedAttackCount;
            }

            for (var i = 0; i < abilities.Length; i++)
            {
                var bijli = abilities[i] as BijliFighterController;
                if (bijli != null)
                {
                    var agent = bijli.GetComponent<MovementPlayerAgent>();
                    if (agent == null) continue;
                    var participant = FindParticipant(result, agent.ActorId);
                    participant.AbilityAttempts = bijli.AbilityAttemptCount;
                    participant.AcceptedAbilities = bijli.AbilityAcceptedCount;
                    participant.RejectedAbilities = bijli.AbilityRejectedCount;
                }

                var pehel = abilities[i] as PehelFighterController;
                if (pehel != null)
                {
                    var agent = pehel.GetComponent<MovementPlayerAgent>();
                    if (agent == null) continue;
                    var participant = FindParticipant(result, agent.ActorId);
                    participant.AbilityAttempts = pehel.AbilityAttemptCount;
                    participant.AcceptedAbilities = pehel.AbilityAcceptedCount;
                    participant.RejectedAbilities = pehel.AbilityRejectedCount;
                    participant.EffectiveAbilities = pehel.AbilityEffectiveOutcomeCount;
                }

                var maya = abilities[i] as MayaFighterController;
                if (maya != null)
                {
                    var agent = maya.GetComponent<MovementPlayerAgent>();
                    if (agent == null) continue;
                    var participant = FindParticipant(result, agent.ActorId);
                    participant.AbilityAttempts = maya.AbilityAttemptCount;
                    participant.AcceptedAbilities = maya.AbilityAcceptedCount;
                    participant.RejectedAbilities = maya.AbilityRejectedCount;
                    participant.EffectiveAbilities = maya.AbilityAcceptedCount;
                }
            }

            for (var i = 0; i < bots.Length; i++)
            {
                var agent = bots[i].GetComponent<MovementPlayerAgent>();
                if (agent == null) continue;
                var participant = FindParticipant(result, agent.ActorId);
                participant.TargetSwitches = bots[i].TargetSwitchCount;
                participant.DecisionCount = bots[i].DecisionCount;
                participant.TargetDecisionCount = bots[i].TargetDecisionCount;
                participant.EngageDecisionCount = bots[i].EngageDecisionCount;
                participant.AttackDecisionCount = bots[i].AttackDecisionCount;
                participant.StuckRecoveries = bots[i].StuckRecoveryCount;
                participant.ZoneSafetyDecisions = bots[i].ZoneSafetyDecisionCount;
                participant.MaxContinuousStuckTicks = bots[i].MaxContinuousStuckTicks;
                participant.MaxStuckPositionX = bots[i].MaxStuckPosition.X;
                participant.MaxStuckPositionZ = bots[i].MaxStuckPosition.Y;
                participant.MaxStuckSimulationTick = bots[i].MaxStuckSimulationTick;
                var sensor = bots[i].GetComponent<BotPerceptionSensor>();
                participant.MaxVisibleTargets = sensor != null ? sensor.MaxVisibleTargetCount : 0;
                participant.MaxHostileTargets = sensor != null ? sensor.MaxHostileTargetCount : 0;
                participant.HostileCaptureCount = sensor != null ? sensor.HostileCaptureCount : 0;
                participant.MaxDecisionMilliseconds = Math.Max(
                    participant.MaxDecisionMilliseconds,
                    bots[i].MaxDecisionMilliseconds);
                participant.CommandDigest = bots[i].CommandDigest.ToString("X16");
                participant.CommandCount = bots[i].CommandCount;
                participant.AuthorityCommandQueuedCount = agent.AuthorityCommandQueuedCount;
                participant.AuthorityCommandRejectedCount = agent.AuthorityCommandRejectedCount;
                participant.AuthorityCommandFallbackCount = agent.AuthorityCommandFallbackCount;
                result.MaxDecisionMilliseconds = Math.Max(
                    result.MaxDecisionMilliseconds,
                    bots[i].MaxDecisionMilliseconds);
                result.CommandDigest = MixDigest(
                    result.CommandDigest,
                    bots[i].CommandDigest).ToString("X16");
                result.CommandCount += bots[i].CommandCount;
            }

            for (var i = 0; i < gadgets.Length; i++)
            {
                var agent = gadgets[i].GetComponent<MovementPlayerAgent>();
                if (agent == null) continue;
                var participant = FindParticipant(result, agent.ActorId);
                participant.GadgetPickups = gadgets[i].SuccessfulPickupCount;
                participant.GadgetUseAttempts = gadgets[i].ContextualUseAttemptCount;
                participant.SuccessfulGadgetUses = gadgets[i].SuccessfulUseCount;
                participant.SuccessfulUmbrellaGuardUses = gadgets[i].SuccessfulUmbrellaGuardUses;
                participant.SuccessfulDholBurstUses = gadgets[i].SuccessfulDholBurstUses;
                participant.SuccessfulTiffinStationUses = gadgets[i].SuccessfulTiffinStationUses;
                participant.FailedGadgetUses = gadgets[i].FailedUseCount;
            }

            foreach (var participant in result.Participants)
            {
                result.AttackAttempts += participant.AttackAttempts;
                result.AcceptedAttacks += participant.AcceptedAttacks;
                result.RejectedAttacks += participant.RejectedAttacks;
                result.OutOfRangeAttackAttempts += participant.OutOfRangeAttackAttempts;
                result.AbilityAttempts += participant.AbilityAttempts;
                result.AcceptedAbilities += participant.AcceptedAbilities;
                result.RejectedAbilities += participant.RejectedAbilities;
                result.EffectiveAbilities += participant.EffectiveAbilities;
                result.TargetSwitches += participant.TargetSwitches;
                result.StuckRecoveries += participant.StuckRecoveries;
                result.ZoneSafetyDecisions += participant.ZoneSafetyDecisions;
                result.MaxContinuousStuckTicks = Math.Max(
                    result.MaxContinuousStuckTicks,
                    participant.MaxContinuousStuckTicks);
                result.GadgetPickups += participant.GadgetPickups;
                result.ContextualGadgetUses += participant.GadgetUseAttempts;
                result.SuccessfulGadgetUses += participant.SuccessfulGadgetUses;
                result.SuccessfulUmbrellaGuardUses += participant.SuccessfulUmbrellaGuardUses;
                result.SuccessfulDholBurstUses += participant.SuccessfulDholBurstUses;
                result.SuccessfulTiffinStationUses += participant.SuccessfulTiffinStationUses;
                result.FailedGadgetUses += participant.FailedGadgetUses;
            }
        }

        private static AutonomousBotParticipantResult FindParticipant(
            AutonomousBotMatchResult result,
            int actorId)
        {
            for (var i = 0; i < result.Participants.Count; i++)
            {
                if (result.Participants[i].ActorId == actorId) return result.Participants[i];
            }

            throw new InvalidOperationException($"Missing participant result for actor {actorId}.");
        }

        private static string ResolveFighter(Component component)
        {
            if (component.GetComponent<BijliFighterController>() != null &&
                component.GetComponent<BijliFighterController>().enabled) return "Bijli";
            if (component.GetComponent<PehelFighterController>() != null &&
                component.GetComponent<PehelFighterController>().enabled) return "Pehel";
            if (component.GetComponent<MayaFighterController>() != null &&
                component.GetComponent<MayaFighterController>().enabled) return "Maya";
            return "Unknown";
        }

        private void WriteMatchReport(AutonomousBotMatchResult result, uint seed)
        {
            var report = new AutonomousBotBatchReport
            {
                SceneName = "BazaarBastion",
                UnityVersion = Application.unityVersion,
                BaseSeed = seed,
                PlaybackScale = _reportPlaybackScale,
                CapturedAtUtc = DateTime.UtcNow.ToString("o")
            };
            report.Matches.Add(result);
            WriteJson(report, $"match-{_reportRunId}-{seed}.json");
        }

        private void WriteBatchReport(uint baseSeed, float playbackScale)
        {
            var report = new AutonomousBotBatchReport
            {
                SceneName = "BazaarBastion",
                UnityVersion = Application.unityVersion,
                BaseSeed = baseSeed,
                PlaybackScale = _reportPlaybackScale,
                CapturedAtUtc = DateTime.UtcNow.ToString("o"),
                Matches = new List<AutonomousBotMatchResult>(Results)
            };
            LastReportPath = WriteJson(report, $"batch-{_reportRunId}-{baseSeed}.json");
        }

        private static string WriteJson(AutonomousBotBatchReport report, string fileName)
        {
            var directory = Path.GetFullPath(Path.Combine(
                Application.dataPath,
                "..",
                "Builds",
                "Local",
                "V1GameplayTruth",
                "ProductionBotReports"));
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, fileName);
            File.WriteAllText(path, JsonUtility.ToJson(report, true));
            return path;
        }

        private static ulong MixDigest(string current, ulong next)
        {
            unchecked
            {
                var value = 14695981039346656037UL;
                if (!string.IsNullOrEmpty(current) && ulong.TryParse(current, System.Globalization.NumberStyles.HexNumber, null, out var parsed))
                {
                    value = parsed;
                }

                value ^= next;
                value *= 1099511628211UL;
                return value;
            }
        }
    }
}
#endif
