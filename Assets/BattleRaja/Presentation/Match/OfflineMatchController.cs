using System.Collections.Generic;
using System.Linq;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Gadgets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleRaja.Presentation.Match
{
    public sealed class OfflineMatchController : MonoBehaviour
    {
        [SerializeField] private CombatDamageResolver damageResolver;
        [SerializeField] private TopDownCameraController cameraController;
        [SerializeField] private MatchPickup[] pickups;
        [SerializeField] private GadgetPickup[] gadgetPickups;
        [SerializeField] private float outsideDamageTickSeconds = 1f;
        [SerializeField] private bool autoStart = true;

        private readonly List<MatchActorBinding> _actors = new List<MatchActorBinding>(8);
        private OfflineMatchSimulation _simulation;
        private float _outsideDamageAccumulator;
        private bool _playerSpectating;
        private bool _resultsShown;

        public OfflineMatchSimulation Simulation => _simulation;
        public MatchPhase CurrentPhase => _simulation != null ? _simulation.Phase : MatchPhase.LoadWarmup;
        public float ZoneRadius { get; private set; }
        public int AliveCount => _simulation != null ? _simulation.AliveCount : 0;
        public bool PlayerSpectating => _playerSpectating;
        public bool ResultsShown => _resultsShown;
        public MatchParticipantSnapshot[] Results { get; private set; }

        private void Awake()
        {
            damageResolver = damageResolver != null ? damageResolver : FindFirstObjectByType<CombatDamageResolver>();
            cameraController = cameraController != null ? cameraController : FindFirstObjectByType<TopDownCameraController>();
            pickups = pickups != null && pickups.Length > 0 ? pickups : FindObjectsByType<MatchPickup>(FindObjectsSortMode.None);
            gadgetPickups = gadgetPickups != null && gadgetPickups.Length > 0 ? gadgetPickups : FindObjectsByType<GadgetPickup>(FindObjectsSortMode.None);
            CacheActors();
            if (autoStart)
            {
                StartMatch();
            }
        }

        private void Update()
        {
            if (_simulation == null || _simulation.IsEnded)
            {
                return;
            }

            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                _simulation.SetPosition(actor.Target.Id, new Float2(actor.Transform.position.x, actor.Transform.position.z));
                _simulation.SyncHealth(actor.Target.Id, actor.Health.Snapshot.CurrentHealth);
            }

            var tick = _simulation.Advance(Time.deltaTime);
            ZoneRadius = tick.ZoneRadius;
            _outsideDamageAccumulator += Time.deltaTime;
            if (_outsideDamageAccumulator >= outsideDamageTickSeconds && tick.OutsideDamagePerSecond > 0)
            {
                _outsideDamageAccumulator = 0f;
                ApplyOutsideDamage(tick);
            }

            CollectPickups();
            CollectGadgets();
            UpdateSpectator(tick);
            if (tick.MatchEnded)
            {
                Results = _simulation.GetSnapshots();
                _resultsShown = true;
            }
        }

        public void StartMatch()
        {
            CacheActors();
            var spawns = _actors.Select(actor => new MatchSpawn(actor.Target.Id, new Float2(actor.Transform.position.x, actor.Transform.position.z), actor.Health.MaxHealth)).ToList();
            _simulation = new OfflineMatchSimulation(OfflineMatchDefinition.SoloRaja);
            _simulation.Start(spawns);
            _outsideDamageAccumulator = 0f;
            _playerSpectating = false;
            _resultsShown = false;
            Results = null;
        }

        public void RestartMatch()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public void CycleSpectator()
        {
            if (!_playerSpectating || _simulation == null || cameraController == null)
            {
                return;
            }

            var snapshots = _simulation.GetSnapshots();
            var next = SpectatorTargetSelector.SelectNext(snapshots, cameraController.FollowTarget != null
                ? cameraController.FollowTarget.GetComponent<CombatTarget>()?.Id ?? default
                : default);
            var actor = _actors.FirstOrDefault(binding => binding.Target.Id == next);
            if (actor != null) cameraController.SetFollowTarget(actor.Transform);
        }

        private void CacheActors()
        {
            _actors.Clear();
            var agents = FindObjectsByType<MovementPlayerAgent>(FindObjectsSortMode.None).OrderBy(agent => agent.ActorId);
            foreach (var agent in agents)
            {
                var target = agent.GetComponent<CombatTarget>();
                var health = agent.GetComponent<CombatHealth>();
                if (target != null && health != null)
                {
                    _actors.Add(new MatchActorBinding(agent.transform, target, health, agent.GetComponent<PlayerInputAdapter>()));
                }
            }
        }

        private void ApplyOutsideDamage(MatchTickResult tick)
        {
            for (var i = 0; i < _actors.Count; i++)
            {
                var actor = _actors[i];
                if (actor.Health.Snapshot.IsDefeated || Float2.Distance(new Float2(actor.Transform.position.x, actor.Transform.position.z), tick.ZoneCenter) <= tick.ZoneRadius)
                {
                    continue;
                }

                damageResolver?.Resolve(
                    actor.Target,
                    new DamageRequest(new CombatEntityId(-99), actor.Target.Id, CombatFaction.Neutral, tick.OutsideDamagePerSecond, DamageType.Aandhi),
                    allowSelfHit: true,
                    allowFriendlyFire: true);
            }
        }

        private void CollectPickups()
        {
            if (pickups == null) return;
            for (var p = 0; p < pickups.Length; p++)
            {
                var pickup = pickups[p];
                if (pickup == null || !pickup.IsAvailable) continue;
                for (var i = 0; i < _actors.Count; i++)
                {
                    var actor = _actors[i];
                    if (actor.Health.Snapshot.IsDefeated || Vector3.Distance(actor.Transform.position, pickup.transform.position) > 1.2f) continue;
                    if (pickup.TryCollect(actor.Health)) break;
                }
            }
        }

        private void CollectGadgets()
        {
            if (gadgetPickups == null) return;
            for (var p = 0; p < gadgetPickups.Length; p++)
            {
                var pickup = gadgetPickups[p];
                if (pickup == null || !pickup.IsAvailable) continue;
                for (var i = 0; i < _actors.Count; i++)
                {
                    var actor = _actors[i];
                    if (actor.Health.Snapshot.IsDefeated || Vector3.Distance(actor.Transform.position, pickup.transform.position) > 1.3f) continue;
                    var user = actor.Transform.GetComponent<GadgetUser>();
                    if (pickup.TryCollect(user)) break;
                }
            }
        }

        private void UpdateSpectator(MatchTickResult tick)
        {
            var player = _actors.FirstOrDefault(actor => actor.Target.Id.Value == 1);
            if (player == null || !player.Health.Snapshot.IsDefeated) return;
            if (!_playerSpectating)
            {
                _playerSpectating = true;
                player.Input?.ReleasePointerFocus();
                var next = SpectatorTargetSelector.SelectNext(_simulation.GetSnapshots(), player.Target.Id);
                var actor = _actors.FirstOrDefault(binding => binding.Target.Id == next);
                if (actor != null) cameraController?.SetFollowTarget(actor.Transform);
            }
        }

        private sealed class MatchActorBinding
        {
            public MatchActorBinding(Transform transform, CombatTarget target, CombatHealth health, PlayerInputAdapter input)
            {
                Transform = transform;
                Target = target;
                Health = health;
                Input = input;
            }

            public Transform Transform { get; }
            public CombatTarget Target { get; }
            public CombatHealth Health { get; }
            public PlayerInputAdapter Input { get; }
        }
    }
}
