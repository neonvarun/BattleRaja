using System.Collections.Generic;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatImpactFeedbackPool : MonoBehaviour
    {
        [SerializeField] private int prewarmCount = 8;
        [SerializeField] private float lifetimeSeconds = 0.16f;
        [SerializeField] private Material impactMaterial;

        private readonly List<ImpactInstance> _instances = new List<ImpactInstance>();

        public int CreatedCount => _instances.Count;
        public int ActiveCount { get; private set; }

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
                    ActiveCount = Mathf.Max(0, ActiveCount - 1);
                }
                else
                {
                    instance.Object.transform.localScale = Vector3.one * (0.3f + (1f - instance.Remaining / lifetimeSeconds) * 0.45f);
                    _instances[i] = instance;
                }
            }
        }

        public void Play(Vector3 position, bool successfulHit)
        {
            var instance = FindAvailable() ?? CreateInstance();
            instance.Object.transform.position = position;
            instance.Object.transform.localScale = Vector3.one * 0.3f;
            instance.Object.SetActive(true);
            instance.Remaining = Mathf.Max(0.01f, lifetimeSeconds);
            instance.Renderer.GetPropertyBlock(instance.Properties);
            instance.Properties.SetColor("_BaseColor", successfulHit ? new Color(1f, 0.82f, 0.18f) : new Color(0.75f, 0.75f, 0.78f));
            instance.Renderer.SetPropertyBlock(instance.Properties);
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
            var objectToPool = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            objectToPool.name = "PooledImpact";
            objectToPool.transform.SetParent(transform, false);
            var collider = objectToPool.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }

            var renderer = objectToPool.GetComponent<Renderer>();
            if (impactMaterial != null)
            {
                renderer.sharedMaterial = impactMaterial;
            }

            objectToPool.SetActive(false);
            var instance = new ImpactInstance
            {
                Object = objectToPool,
                Renderer = renderer,
                Properties = new MaterialPropertyBlock(),
                Remaining = 0f
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
        }
    }
}
