using System.Collections.Generic;
using BattleRaja.Presentation.Visuals;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatImpactFeedbackPool : MonoBehaviour
    {
        [SerializeField] private int prewarmCount = 8;
        [SerializeField] private float lifetimeSeconds = 0.16f;
        [SerializeField] private Material impactMaterial;
        [SerializeField] private bool reducedFlashMode;

        private readonly List<ImpactInstance> _instances = new List<ImpactInstance>();

        public int CreatedCount => _instances.Count;
        public int ActiveCount { get; private set; }
        public bool ReducedFlashMode { get => reducedFlashMode; set => reducedFlashMode = value; }

        private void Awake()
        {
            for (var i = 0; i < Mathf.Max(0, prewarmCount); i++)
            {
                CreateInstance();
            }
        }

        private void Update()
        {
            for (var i = 0; i < _instances.Count; i++)
            {
                var instance = _instances[i];
                if (!instance.Object.activeSelf)
                {
                    continue;
                }

                instance.Remaining -= Time.deltaTime;
                if (instance.Remaining <= 0f)
                {
                    instance.Object.SetActive(false);
                    if (instance.Halo != null) instance.Halo.gameObject.SetActive(false);
                    ActiveCount = Mathf.Max(0, ActiveCount - 1);
                }
                else
                {
                    var lifetime = Mathf.Max(0.01f, instance.Lifetime);
                    var progress = 1f - instance.Remaining / lifetime;
                    instance.Object.transform.localScale = Vector3.one * (0.22f + progress * 0.46f);
                    if (instance.Halo != null)
                    {
                        instance.Halo.localScale = new Vector3(0.30f + progress * 0.86f, 0.012f, 0.30f + progress * 0.86f);
                        instance.Halo.localRotation = Quaternion.Euler(0f, progress * 90f, 0f);
                    }
                    _instances[i] = instance;
                }
            }
        }

        public void Play(Vector3 position, bool successfulHit)
        {
            var instance = FindAvailable() ?? CreateInstance();
            var lifetimeScale = reducedFlashMode ? 0.35f : 1f;
            instance.Object.transform.position = position;
            if (instance.Halo != null) instance.Halo.position = position + Vector3.up * 0.02f;
            instance.Object.transform.localScale = Vector3.one * 0.22f;
            instance.Object.SetActive(true);
            instance.Lifetime = Mathf.Max(0.01f, lifetimeSeconds * lifetimeScale);
            instance.Remaining = instance.Lifetime;
            instance.Renderer.GetPropertyBlock(instance.Properties);
            var color = reducedFlashMode
                ? new Color(0.62f, 0.68f, 0.74f)
                : successfulHit ? new Color(1f, 0.82f, 0.18f) : new Color(0.75f, 0.75f, 0.78f);
            instance.Properties.SetColor("_BaseColor", color);
            instance.Renderer.SetPropertyBlock(instance.Properties);
            if (instance.Halo != null)
            {
                instance.Halo.localScale = Vector3.one * 0.30f;
                instance.Halo.localRotation = Quaternion.identity;
                instance.Halo.gameObject.SetActive(!reducedFlashMode);
                if (!reducedFlashMode)
                {
                    instance.HaloRenderer.GetPropertyBlock(instance.HaloProperties);
                    instance.HaloProperties.SetColor("_BaseColor", color);
                    instance.HaloRenderer.SetPropertyBlock(instance.HaloProperties);
                }
            }
            ActiveCount++;
        }

        private ImpactInstance FindAvailable()
        {
            for (var i = 0; i < _instances.Count; i++)
            {
                if (!_instances[i].Object.activeSelf)
                {
                    return _instances[i];
                }
            }

            return null;
        }

        private ImpactInstance CreateInstance()
        {
            var objectToPool = new GameObject("PooledImpact", typeof(MeshFilter), typeof(MeshRenderer));
            objectToPool.transform.SetParent(transform, false);
            objectToPool.GetComponent<MeshFilter>().sharedMesh = PresentationMeshFactory.FacetedOrb("PooledImpactOrb", 3, 12);
            var renderer = objectToPool.GetComponent<MeshRenderer>();
            if (impactMaterial != null)
            {
                renderer.sharedMaterial = impactMaterial;
            }

            objectToPool.SetActive(false);
            var haloObject = new GameObject("PooledImpactHalo", typeof(MeshFilter), typeof(MeshRenderer));
            haloObject.transform.SetParent(transform, false);
            haloObject.transform.localScale = new Vector3(0.30f, 0.012f, 0.30f);
            haloObject.GetComponent<MeshFilter>().sharedMesh = PresentationMeshFactory.Ring("PooledImpactRing", 0.42f, 0.5f, 20);
            var haloRenderer = haloObject.GetComponent<MeshRenderer>();
            if (impactMaterial != null)
            {
                haloRenderer.sharedMaterial = impactMaterial;
            }

            haloObject.SetActive(false);
            var instance = new ImpactInstance
            {
                Object = objectToPool,
                Renderer = renderer,
                Properties = new MaterialPropertyBlock(),
                Remaining = 0f,
                Lifetime = Mathf.Max(0.01f, lifetimeSeconds),
                Halo = haloObject.transform,
                HaloRenderer = haloRenderer,
                HaloProperties = new MaterialPropertyBlock()
            };
            _instances.Add(instance);
            return instance;
        }

        private sealed class ImpactInstance
        {
            public GameObject Object;
            public Renderer Renderer;
            public MaterialPropertyBlock Properties;
            public float Remaining;
            public float Lifetime;
            public Transform Halo;
            public Renderer HaloRenderer;
            public MaterialPropertyBlock HaloProperties;
        }
    }
}
