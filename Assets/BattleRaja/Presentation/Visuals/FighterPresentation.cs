using System.Collections.Generic;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.UI;
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
        private readonly List<GameObject> _ownedObjects = new List<GameObject>(32);
        private readonly List<Material> _ownedMaterials = new List<Material>(20);
        private readonly List<Transform> _silhouetteParts = new List<Transform>(24);
        private readonly List<Vector3> _silhouetteBasePositions = new List<Vector3>(24);
        private readonly List<Quaternion> _silhouetteBaseRotations = new List<Quaternion>(24);
        private readonly List<Vector3> _silhouetteBaseScales = new List<Vector3>(24);
        private Transform _ring;
        private Transform _healthBar;
        private Transform _healthFill;
        private Transform _telegraph;
        private Transform _silhouetteRoot;
        private BattleRajaAudioDirector _audio;
        private CombatTarget _target;
        private float _attackPulse;
        private float _abilityPulse;
        private float _hitRemaining;
        private float _telegraphRemaining;
        private Color _baseBodyColor = Color.white;
        private Color _ringColor = new Color(0.18f, 0.78f, 1f, 1f);
        private bool _eliminated;
        private bool _victory;
        private bool _silhouetteBuilt;
        private bool _flashApplied;
        private Vector3 _bodyBaseLocalPosition;

        public AnimationState CurrentAnimation { get; private set; } = AnimationState.Idle;
        public bool IsEliminated => _eliminated;
        public bool IsVictory => _victory;
        public int AnimatedPartCount => _silhouetteParts.Count;
        public int AttackActivationCount { get; private set; }
        public int AbilityActivationCount { get; private set; }
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

            _target = GetComponent<CombatTarget>();
            _ringColor = ResolveRingColor(_target != null ? _target.Faction : CombatFaction.Enemy);
            CreateReadabilityPrimitives();
            CreateFighterSilhouette();
            if (health != null) health.DamageResolved += OnDamageResolved;
            UpdateHealthBar();
        }

        private void OnDestroy()
        {
            if (health != null) health.DamageResolved -= OnDamageResolved;
            for (var i = 0; i < _ownedObjects.Count; i++)
            {
                if (_ownedObjects[i] != null) Destroy(_ownedObjects[i]);
            }

            for (var i = 0; i < _ownedMaterials.Count; i++)
            {
                if (_ownedMaterials[i] != null) Destroy(_ownedMaterials[i]);
            }
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
                ApplySilhouetteAnimation();
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
                if (_silhouetteRoot != null)
                {
                    _silhouetteRoot.localPosition = Vector3.up * bob;
                    _silhouetteRoot.localScale = Vector3.one * pulse;
                }
            }

            ApplySilhouetteAnimation();

            if (_flashApplied && _hitRemaining <= 0f && bodyRenderer != null && !_eliminated)
            {
                bodyRenderer.GetPropertyBlock(_bodyProperties);
                _bodyProperties.SetColor("_BaseColor", _baseBodyColor);
                bodyRenderer.SetPropertyBlock(_bodyProperties);
                _flashApplied = false;
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
            AttackActivationCount++;
            _attackPulse = 0.14f;
            _telegraphRemaining = 0.14f;
            _audio?.PlayAttack();
        }

        public void NotifyAbility()
        {
            AbilityActivationCount++;
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
                _flashApplied = true;
            }

            _audio?.PlayHit();
            if (_target != null && _target.Id.Value == 1) BattleRajaHaptics.Pulse();
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

            _flashApplied = false;

            if (_ringMaterial != null) _ringMaterial.color = new Color(0.86f, 0.12f, 0.12f, 1f);
            _audio?.PlayElimination();
        }

        private void ApplySilhouetteAnimation()
        {
            if (_silhouetteParts.Count == 0) return;

            var time = Time.time;
            for (var i = 0; i < _silhouetteParts.Count; i++)
            {
                var part = _silhouetteParts[i];
                if (part == null) continue;

                var position = _silhouetteBasePositions[i];
                var rotation = _silhouetteBaseRotations[i];
                var scale = _silhouetteBaseScales[i];
                var phase = time * 7.5f + i * 0.37f;
                var sway = Mathf.Sin(phase);

                switch (CurrentAnimation)
                {
                    case AnimationState.Locomotion:
                        position.y += Mathf.Abs(sway) * 0.045f;
                        rotation *= Quaternion.Euler(0f, 0f, sway * 5f);
                        break;
                    case AnimationState.Attack:
                        position.z -= 0.06f;
                        rotation *= Quaternion.Euler(0f, sway * 8f, -sway * 10f);
                        scale *= 1f + Mathf.Abs(sway) * 0.06f;
                        break;
                    case AnimationState.Ability:
                        position.y += 0.06f + Mathf.Abs(sway) * 0.07f;
                        rotation *= Quaternion.Euler(0f, sway * 14f, sway * 8f);
                        scale *= 1.04f + Mathf.Abs(sway) * 0.08f;
                        break;
                    case AnimationState.Hit:
                    case AnimationState.Knockback:
                        position.x += 0.05f * Mathf.Sign(sway == 0f ? 1f : sway);
                        rotation *= Quaternion.Euler(0f, 0f, -sway * 12f);
                        break;
                    case AnimationState.Eliminated:
                    case AnimationState.Defeat:
                        position.y -= 0.12f;
                        rotation *= Quaternion.Euler(0f, 0f, -18f);
                        scale = Vector3.Scale(scale, new Vector3(1.08f, 0.72f, 1.08f));
                        break;
                    case AnimationState.Victory:
                        position.y += 0.10f + Mathf.Abs(sway) * 0.04f;
                        rotation *= Quaternion.Euler(0f, sway * 8f, sway * 6f);
                        scale *= 1.05f;
                        break;
                }

                part.localPosition = position;
                part.localRotation = rotation;
                part.localScale = scale;
            }
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

        private void CreateFighterSilhouette()
        {
            if (_silhouetteBuilt) return;
            _silhouetteBuilt = true;
            _silhouetteRoot = new GameObject("FighterIdentitySilhouette").transform;
            _silhouetteRoot.SetParent(transform, false);
            _silhouetteRoot.localPosition = new Vector3(0f, 0.74f, 0f);

            var bijli = GetComponent<BijliFighterController>();
            var pehel = GetComponent<PehelFighterController>();
            var maya = GetComponent<MayaFighterController>();
            if (bijli != null && bijli.enabled)
            {
                var cyan = CreateMaterial(new Color(0.08f, 0.82f, 0.98f, 1f));
                var gold = CreateMaterial(new Color(1f, 0.78f, 0.12f, 1f));
                var ink = CreateMaterial(new Color(0.03f, 0.12f, 0.18f, 1f));
                // A compact runner silhouette: visor, shoulder fins and a split bolt crest
                // remain legible from the overhead Lava camera without affecting collision.
                CreateVisualPrimitive("BijliVisor", PrimitiveType.Cube, new Vector3(0f, 0.37f, -0.28f), new Vector3(0.38f, 0.12f, 0.10f), Quaternion.identity, ink);
                CreateVisualPrimitive("BijliShoulderL", PrimitiveType.Capsule, new Vector3(-0.34f, 0.16f, 0f), new Vector3(0.18f, 0.30f, 0.18f), Quaternion.Euler(0f, 0f, 28f), cyan);
                CreateVisualPrimitive("BijliShoulderR", PrimitiveType.Capsule, new Vector3(0.34f, 0.16f, 0f), new Vector3(0.18f, 0.30f, 0.18f), Quaternion.Euler(0f, 0f, -28f), gold);
                CreateVisualPrimitive("BijliCrest", PrimitiveType.Cube, new Vector3(-0.08f, 0.58f, 0f), new Vector3(0.18f, 0.42f, 0.14f), Quaternion.Euler(0f, 0f, -22f), cyan);
                CreateVisualPrimitive("BijliCrestGold", PrimitiveType.Cube, new Vector3(0.12f, 0.68f, -0.02f), new Vector3(0.14f, 0.28f, 0.16f), Quaternion.Euler(0f, 0f, 26f), gold);
                CreateVisualPrimitive("BijliSparkL", PrimitiveType.Cube, new Vector3(-0.38f, 0.32f, 0f), new Vector3(0.08f, 0.28f, 0.08f), Quaternion.Euler(0f, 0f, -24f), cyan);
                CreateVisualPrimitive("BijliSparkR", PrimitiveType.Cube, new Vector3(0.38f, 0.32f, 0f), new Vector3(0.08f, 0.28f, 0.08f), Quaternion.Euler(0f, 0f, 24f), gold);
                CreateVisualPrimitive("BijliTrailTab", PrimitiveType.Cube, new Vector3(0f, 0.02f, 0.38f), new Vector3(0.24f, 0.10f, 0.32f), Quaternion.Euler(0f, 0f, 8f), cyan);
                return;
            }

            if (pehel != null && pehel.enabled)
            {
                var clay = CreateMaterial(new Color(0.88f, 0.34f, 0.16f, 1f));
                var cream = CreateMaterial(new Color(1f, 0.72f, 0.34f, 1f));
                var ink = CreateMaterial(new Color(0.16f, 0.08f, 0.08f, 1f));
                // A broad grappler silhouette: shoulder guards, belt, brow and gauntlets
                // make Pehel read as a tank even when the health bar is hidden.
                CreateVisualPrimitive("PehelChest", PrimitiveType.Cube, new Vector3(0f, 0.10f, -0.02f), new Vector3(0.74f, 0.34f, 0.46f), Quaternion.identity, clay);
                CreateVisualPrimitive("PehelBelt", PrimitiveType.Cube, new Vector3(0f, -0.15f, -0.04f), new Vector3(0.84f, 0.13f, 0.50f), Quaternion.identity, cream);
                CreateVisualPrimitive("PehelShoulderL", PrimitiveType.Sphere, new Vector3(-0.42f, 0.27f, 0f), new Vector3(0.38f, 0.25f, 0.40f), Quaternion.identity, clay);
                CreateVisualPrimitive("PehelShoulderR", PrimitiveType.Sphere, new Vector3(0.42f, 0.27f, 0f), new Vector3(0.38f, 0.25f, 0.40f), Quaternion.identity, clay);
                CreateVisualPrimitive("PehelBrow", PrimitiveType.Cube, new Vector3(0f, 0.62f, -0.16f), new Vector3(0.54f, 0.13f, 0.18f), Quaternion.identity, cream);
                CreateVisualPrimitive("PehelVisor", PrimitiveType.Cube, new Vector3(0f, 0.42f, -0.28f), new Vector3(0.38f, 0.10f, 0.10f), Quaternion.identity, ink);
                CreateVisualPrimitive("PehelGuardL", PrimitiveType.Sphere, new Vector3(-0.52f, -0.02f, -0.02f), new Vector3(0.24f, 0.24f, 0.24f), Quaternion.identity, cream);
                CreateVisualPrimitive("PehelGuardR", PrimitiveType.Sphere, new Vector3(0.52f, -0.02f, -0.02f), new Vector3(0.24f, 0.24f, 0.24f), Quaternion.identity, cream);
                CreateVisualPrimitive("PehelBackPennant", PrimitiveType.Cube, new Vector3(0f, 0.44f, 0.34f), new Vector3(0.10f, 0.42f, 0.22f), Quaternion.Euler(0f, 0f, -12f), cream);
                return;
            }

            if (maya != null && maya.enabled)
            {
                var violet = CreateMaterial(new Color(0.70f, 0.26f, 0.86f, 1f));
                var mint = CreateMaterial(new Color(0.18f, 0.88f, 0.70f, 1f));
                var ink = CreateMaterial(new Color(0.08f, 0.05f, 0.14f, 1f));
                // A hooded trickster silhouette uses a wide cloak, mask and asymmetric
                // charms. Its mint accents remain visible even when its colour ring is
                // disabled for high-contrast mode.
                CreateVisualPrimitive("MayaCloak", PrimitiveType.Capsule, new Vector3(0f, 0.05f, 0.04f), new Vector3(0.70f, 0.54f, 0.54f), Quaternion.identity, violet);
                CreateVisualPrimitive("MayaHood", PrimitiveType.Sphere, new Vector3(0f, 0.44f, 0f), new Vector3(0.68f, 0.38f, 0.54f), Quaternion.identity, violet);
                CreateVisualPrimitive("MayaMask", PrimitiveType.Cube, new Vector3(0f, 0.36f, -0.30f), new Vector3(0.42f, 0.16f, 0.10f), Quaternion.Euler(0f, 0f, -8f), ink);
                CreateVisualPrimitive("MayaScarf", PrimitiveType.Cube, new Vector3(0f, 0.08f, -0.22f), new Vector3(0.62f, 0.12f, 0.14f), Quaternion.Euler(0f, 12f, 0f), mint);
                CreateVisualPrimitive("MayaCharmL", PrimitiveType.Sphere, new Vector3(-0.42f, 0.02f, -0.14f), new Vector3(0.16f, 0.24f, 0.16f), Quaternion.identity, mint);
                CreateVisualPrimitive("MayaCharmR", PrimitiveType.Sphere, new Vector3(0.42f, 0.12f, -0.10f), new Vector3(0.14f, 0.22f, 0.14f), Quaternion.identity, violet);
                CreateVisualPrimitive("MayaTailL", PrimitiveType.Cube, new Vector3(-0.32f, -0.08f, 0.28f), new Vector3(0.18f, 0.12f, 0.36f), Quaternion.Euler(0f, -18f, 0f), mint);
                CreateVisualPrimitive("MayaTailR", PrimitiveType.Cube, new Vector3(0.32f, -0.08f, 0.28f), new Vector3(0.18f, 0.12f, 0.36f), Quaternion.Euler(0f, 18f, 0f), violet);
            }
        }

        private GameObject CreateVisualPrimitive(string name, PrimitiveType type, Vector3 position, Vector3 scale, Quaternion rotation, Material material)
        {
            var objectToCreate = GameObject.CreatePrimitive(type);
            objectToCreate.name = name;
            objectToCreate.transform.SetParent(_silhouetteRoot, false);
            objectToCreate.transform.localPosition = position;
            objectToCreate.transform.localRotation = rotation;
            objectToCreate.transform.localScale = scale;
            RemoveCollider(objectToCreate);
            var renderer = objectToCreate.GetComponent<Renderer>();
            if (renderer != null) renderer.sharedMaterial = material;
            _ownedObjects.Add(objectToCreate);
            _silhouetteParts.Add(objectToCreate.transform);
            _silhouetteBasePositions.Add(position);
            _silhouetteBaseRotations.Add(objectToCreate.transform.localRotation);
            _silhouetteBaseScales.Add(scale);
            return objectToCreate;
        }

        private Material CreateMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = new Material(shader) { color = color };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            _ownedMaterials.Add(material);
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
