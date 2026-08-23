using System.Collections.Generic;
using UnityEngine;

namespace BattleRaja.Presentation.Visuals
{
    /// <summary>
    /// Presentation-only decoy identity. It deliberately exaggerates the hood, split
    /// scarf and orbiting markers so a decoy is readable without pretending to be the
    /// authoritative actor or health state.
    /// </summary>
    public sealed class MayaDecoyVisuals : MonoBehaviour
    {
        private readonly List<GameObject> _objects = new List<GameObject>(10);
        private readonly List<Material> _materials = new List<Material>(4);
        private Transform _orbit;

        private void Awake()
        {
            if (transform.Find("MayaDecoyIdentity") != null) return;
            Build();
        }

        private void Update()
        {
            if (_orbit == null) return;
            _orbit.Rotate(0f, 110f * Time.deltaTime, 0f, Space.Self);
            var scale = 1f + Mathf.Sin(Time.time * 4.5f) * 0.06f;
            _orbit.localScale = Vector3.one * scale;
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] != null) Destroy(_objects[i]);
            }

            for (var i = 0; i < _materials.Count; i++)
            {
                if (_materials[i] != null) Destroy(_materials[i]);
            }
        }

        private void Build()
        {
            var root = new GameObject("MayaDecoyIdentity").transform;
            root.SetParent(transform, false);
            root.localPosition = Vector3.up * 0.12f;
            var violet = CreateMaterial(new Color(0.68f, 0.24f, 0.86f, 1f));
            var mint = CreateMaterial(new Color(0.18f, 0.90f, 0.70f, 1f));
            var ink = CreateMaterial(new Color(0.07f, 0.04f, 0.14f, 1f));

            CreatePrimitive("DecoyHood", PrimitiveType.Sphere, root, new Vector3(0f, 0.52f, 0f), new Vector3(0.80f, 0.48f, 0.64f), violet);
            CreatePrimitive("DecoyMask", PrimitiveType.Cube, root, new Vector3(0f, 0.40f, -0.36f), new Vector3(0.46f, 0.16f, 0.10f), ink, Quaternion.Euler(0f, 0f, -9f));
            CreatePrimitive("DecoyScarf", PrimitiveType.Cube, root, new Vector3(0f, 0.12f, -0.22f), new Vector3(0.72f, 0.12f, 0.14f), mint, Quaternion.Euler(0f, 9f, 0f));
            CreatePrimitive("DecoyTailL", PrimitiveType.Cube, root, new Vector3(-0.40f, 0.02f, 0.24f), new Vector3(0.16f, 0.12f, 0.42f), mint, Quaternion.Euler(0f, -18f, 0f));
            CreatePrimitive("DecoyTailR", PrimitiveType.Cube, root, new Vector3(0.40f, 0.02f, 0.24f), new Vector3(0.16f, 0.12f, 0.42f), violet, Quaternion.Euler(0f, 18f, 0f));

            var orbitObject = new GameObject("DecoyOrbit").transform;
            orbitObject.SetParent(root, false);
            orbitObject.localPosition = new Vector3(0f, 0.20f, 0f);
            _orbit = orbitObject;
            CreatePrimitive("OrbitMint", PrimitiveType.Cube, orbitObject, new Vector3(0.72f, 0f, 0f), new Vector3(0.12f, 0.06f, 0.12f), mint);
            CreatePrimitive("OrbitViolet", PrimitiveType.Cube, orbitObject, new Vector3(-0.72f, 0f, 0f), new Vector3(0.12f, 0.06f, 0.12f), violet);
        }

        private GameObject CreatePrimitive(string name, PrimitiveType type, Transform parent, Vector3 position, Vector3 scale, Material material, Quaternion rotation = default(Quaternion))
        {
            var item = GameObject.CreatePrimitive(type);
            item.name = name;
            item.transform.SetParent(parent, false);
            item.transform.localPosition = position;
            item.transform.localRotation = rotation == default(Quaternion) ? Quaternion.identity : rotation;
            item.transform.localScale = scale;
            var collider = item.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
            var renderer = item.GetComponent<Renderer>();
            if (renderer != null) renderer.sharedMaterial = material;
            _objects.Add(item);
            return item;
        }

        private Material CreateMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = new Material(shader) { color = color };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            _materials.Add(material);
            return material;
        }
    }
}
