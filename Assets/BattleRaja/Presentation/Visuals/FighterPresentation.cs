using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using UnityEngine;

namespace BattleRaja.Presentation.Visuals
{
    /// <summary>
    /// Replaceable stylised presentation for a fighter. Gameplay state remains in the
    /// domain/health components; this component only creates pooled-lightweight readability
    /// primitives and code-driven animation states.
    /// </summary>
    public sealed class FighterPresentation : MonoBehaviour
    {
        public enum AnimationState
        {
            Idle,
            Locomotion,
            Attack,
            Ability,
            Hit,
            Knockback,
            Eliminated,
            Victory,
            Defeat
        }

        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private CombatHealth health;
        [SerializeField] private MovementPlayerAgent movementAgent;
        [SerializeField] private float bobAmplitude = 0.035f;
        [SerializeField] private float bobFrequency = 2.5f;
        [SerializeField] private bool reducedFlashMode;

        private MaterialPropertyBlock _bodyProperties;
        private Material _ringMaterial;
        private Material _barMaterial;
        private Transform _ring;
        private Transform _healthBar;
        private Transform _healthFill;
        private Transform _telegraph;
        private BattleRajaAudioDirector _audio;
        private float _attackPulse;
        private float _abilityPulse;
        private float _hitRemaining;
        private float _telegraphRemaining;
        private Color _baseBodyColor = Color.white;
        private Color _ringColor = new Color(0.18f, 0.78f, 1f, 1f);
        private bool _eliminated;
        private bool _victory;
        private Vector3 _bodyBaseLocalPosition;

        public AnimationState CurrentAnimation { get; private set; } = AnimationState.Idle;
        public bool IsEliminated => _eliminated;
        public bool IsVictory => _victory;
        public bool ReducedFlashMode { get => reducedFlashMode; set => reducedFlashMode = value; }

        private void Awake()
        {
            bodyRenderer = bodyRenderer != null ? bodyRenderer : GetComponentInChildren<Renderer>();
            health = health != null ? health : GetComponent<CombatHealth>();
            movementAgent = movementAgent != null ? movementAgent : GetComponent<MovementPlayerAgent>();
            _audio = FindAnyObjectByType<BattleRajaAudioDirector>();
            _bodyProperties = new MaterialPropertyBlock();
            _bodyBaseLocalPosition = bodyRenderer != null ? bodyRenderer.transform.localPosition : Vector3.zero;
            if (bodyRenderer != null)
            {
                _baseBodyColor = bodyRenderer.sharedMaterial != null && bodyRenderer.sharedMaterial.HasProperty("_BaseColor")
                    ? bodyRenderer.sharedMaterial.GetColor("_BaseColor")
                    : Color.white;
            }

            var target = GetComponent<CombatTarget>();
            _ringColor = ResolveRingColor(target != null ? target.Faction : CombatFaction.Enemy);
            CreateReadabilityPrimitives();
            if (health != null) health.DamageResolved += OnDamageResolved;
            UpdateHealthBar();
        }

        private void OnDestroy()
        {
            if (health != null) health.DamageResolved -= OnDamageResolved;
            if (_ringMaterial != null) Destroy(_ringMaterial);
            if (_barMaterial != null) Destroy(_barMaterial);
        }

        private void Update()
        {
            var delta = Time.deltaTime;
            _attackPulse = Mathf.Max(0f, _attackPulse - delta);
            _abilityPulse = Mathf.Max(0f, _abilityPulse - delta);
            _hitRemaining = Mathf.Max(0f, _hitRemaining - delta);
            _telegraphRemaining = Mathf.Max(0f, _telegraphRemaining - delta);
            UpdateHealthBar();

            if (health != null && health.Snapshot.IsDefeated && !_eliminated)
            {
                SetEliminated();
            }

            if (_eliminated)
            {
                CurrentAnimation = _victory ? AnimationState.Victory : AnimationState.Eliminated;
                if (_ring != null) _ring.localScale = Vector3.one * (1.0f + Mathf.Sin(Time.time * 4f) * 0.06f);
                return;
            }

            if (_hitRemaining > 0f) CurrentAnimation = AnimationState.Hit;
            else if (_abilityPulse > 0f) CurrentAnimation = AnimationState.Ability;
            else if (_attackPulse > 0f) CurrentAnimation = AnimationState.Attack;
            else if (movementAgent != null && movementAgent.Velocity.SqrMagnitude > 0.01f) CurrentAnimation = AnimationState.Locomotion;
            else CurrentAnimation = AnimationState.Idle;

            if (bodyRenderer != null)
            {
                var bob = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
                var pulse = _attackPulse > 0f ? 1.08f : _abilityPulse > 0f ? 1.14f : 1f;
                bodyRenderer.transform.localPosition = _bodyBaseLocalPosition + Vector3.up * bob;
                bodyRenderer.transform.localScale = Vector3.one * pulse;
            }

            if (_telegraph != null)
            {
                _telegraph.gameObject.SetActive(_telegraphRemaining > 0f);
                if (_telegraphRemaining > 0f)
                {
                    var pulse = 1f + Mathf.Sin(Time.time * 18f) * 0.08f;
                    _telegraph.localScale = Vector3.one * pulse;
                }
            }
        }

        public void NotifyAttack()
        {
            _attackPulse = 0.14f;
            _telegraphRemaining = 0.14f;
            _audio?.PlayAttack();
        }

        public void NotifyAbility()
        {
            _abilityPulse = 0.32f;
            _telegraphRemaining = 0.32f;
            _audio?.PlayAbility();
        }

        public void SetVictory(bool victory)
        {
            _victory = victory;
            if (victory) _eliminated = true;
            CurrentAnimation = victory ? AnimationState.Victory : AnimationState.Defeat;
        }

        private void OnDamageResolved(DamageResult result)
        {
            if (!result.Applied) return;
            _hitRemaining = reducedFlashMode ? 0.04f : 0.12f;
            _telegraphRemaining = reducedFlashMode ? 0.04f : 0.08f;
            if (!reducedFlashMode && bodyRenderer != null)
            {
                bodyRenderer.GetPropertyBlock(_bodyProperties);
                _bodyProperties.SetColor("_BaseColor", new Color(1f, 0.9f, 0.22f, 1f));
                bodyRenderer.SetPropertyBlock(_bodyProperties);
            }

            _audio?.PlayHit();
            if (result.TargetDefeated) SetEliminated();
        }

        private void SetEliminated()
        {
            _eliminated = true;
            CurrentAnimation = AnimationState.Eliminated;
            if (bodyRenderer != null)
            {
                bodyRenderer.GetPropertyBlock(_bodyProperties);
                _bodyProperties.SetColor("_BaseColor", new Color(0.22f, 0.22f, 0.25f, 1f));
                bodyRenderer.SetPropertyBlock(_bodyProperties);
            }

            if (_ringMaterial != null) _ringMaterial.color = new Color(0.86f, 0.12f, 0.12f, 1f);
            _audio?.PlayElimination();
        }

        private void UpdateHealthBar()
        {
            if (_healthFill == null || health == null) return;
            var ratio = health.MaxHealth > 0 ? Mathf.Clamp01((float)health.Snapshot.CurrentHealth / health.MaxHealth) : 0f;
            _healthFill.localScale = new Vector3(ratio, 1f, 1f);
            _healthFill.localPosition = new Vector3(-0.48f * (1f - ratio), 0f, 0f);
        }

        private void CreateReadabilityPrimitives()
        {
            _ringMaterial = CreateMaterial(_ringColor);
            _barMaterial = CreateMaterial(new Color(0.22f, 0.92f, 0.36f, 1f));

            var ringObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ringObject.name = "GameplayColorRing";
            ringObject.transform.SetParent(transform, false);
            ringObject.transform.localPosition = new Vector3(0f, -0.92f, 0f);
            ringObject.transform.localScale = new Vector3(0.82f, 0.025f, 0.82f);
            RemoveCollider(ringObject);
            ringObject.GetComponent<Renderer>().sharedMaterial = _ringMaterial;
            _ring = ringObject.transform;

            var barObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barObject.name = "HealthStatusBar";
            barObject.transform.SetParent(transform, false);
            barObject.transform.localPosition = new Vector3(0f, 2.12f, 0f);
            barObject.transform.localScale = new Vector3(1.05f, 0.08f, 0.06f);
            RemoveCollider(barObject);
            barObject.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.08f, 0.08f, 0.1f, 1f));
            _healthBar = barObject.transform;

            var fillObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fillObject.name = "HealthStatusFill";
            fillObject.transform.SetParent(_healthBar, false);
            fillObject.transform.localPosition = new Vector3(-0.48f, 0f, -0.04f);
            fillObject.transform.localScale = new Vector3(0.96f, 0.65f, 0.5f);
            RemoveCollider(fillObject);
            fillObject.GetComponent<Renderer>().sharedMaterial = _barMaterial;
            _healthFill = fillObject.transform;

            var telegraphObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            telegraphObject.name = "AttackAbilityTelegraph";
            telegraphObject.transform.SetParent(transform, false);
            telegraphObject.transform.localPosition = new Vector3(0f, -0.88f, 0f);
            telegraphObject.transform.localScale = new Vector3(0.98f, 0.008f, 0.98f);
            RemoveCollider(telegraphObject);
            telegraphObject.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(1f, 0.72f, 0.12f, 1f));
            telegraphObject.SetActive(false);
            _telegraph = telegraphObject.transform;
        }

        private static Material CreateMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = new Material(shader) { color = color };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            return material;
        }

        private static void RemoveCollider(GameObject objectToClean)
        {
            var collider = objectToClean.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
        }

        private static Color ResolveRingColor(CombatFaction faction)
        {
            return faction == CombatFaction.Player
                ? new Color(0.12f, 0.62f, 1f, 1f)
                : new Color(0.96f, 0.36f, 0.18f, 1f);
        }
    }
}
