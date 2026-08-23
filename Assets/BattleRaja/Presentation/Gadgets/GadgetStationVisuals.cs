using System.Collections.Generic;
using UnityEngine;

namespace BattleRaja.Presentation.Gadgets
{
    /// <summary>Render-only healing station beacon and radius cue.</summary>
    public sealed class GadgetStationVisuals : MonoBehaviour
    {
        private readonly List<GameObject> _objects = new List<GameObject>(8);
        private readonly List<Material> _materials = new List<Material>(3);
        private Transform _beacon;
        private Transform _radius;

        private void Awake()
        {
            if (transform.Find("TiffinStationIdentity") != null) return;
            Build();
        }

        private void Update()
        {
            if (_beacon != null)
            {
                _beacon.Rotate(0f, 70f * Time.deltaTime, 0f, Space.Self);
                _beacon.localScale = Vector3.one * (1f + Mathf.Sin(Time.time * 3.8f) * 0.05f);
            }

            if (_radius != null)
            {
                _radius.localScale = new Vector3(2.4f + Mathf.Sin(Time.time * 2.4f) * 0.08f, 0.012f, 2.4f + Mathf.Sin(Time.time * 2.4f) * 0.08f);
            }
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
            var root = new GameObject("TiffinStationIdentity").transform;
            root.SetParent(transform, false);
            root.localPosition = Vector3.up * 0.35f;
            _beacon = root;
            var gold = CreateMaterial(new Color(1f, 0.72f, 0.16f, 1f));
            var mint = CreateMaterial(new Color(0.24f, 0.90f, 0.72f, 1f));
            var ink = CreateMaterial(new Color(0.05f, 0.11f, 0.13f, 1f));

            CreatePrimitive("StationLower", PrimitiveType.Cylinder, root, new Vector3(0f, 0.12f, 0f), new Vector3(0.62f, 0.14f, 0.62f), gold);
            CreatePrimitive("StationUpper", PrimitiveType.Cylinder, root, new Vector3(0f, 0.38f, 0f), new Vector3(0.45f, 0.12f, 0.45f), mint);
            CreatePrimitive("StationHandle", PrimitiveType.Cube, root, new Vector3(0f, 0.60f, 0f), new Vector3(0.12f, 0.20f, 0.12f), ink);
            CreatePrimitive("StationCrossA", PrimitiveType.Cube, root, new Vector3(0f, 0.78f, 0f), new Vector3(0.60f, 0.06f, 0.10f), mint);
            CreatePrimitive("StationCrossB", PrimitiveType.Cube, root, new Vector3(0f, 0.78f, 0f), new Vector3(0.10f, 0.06f, 0.60f), mint);

            var radiusObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            radiusObject.name = "HealingRadiusCue";
            radiusObject.transform.SetParent(transform, false);
            radiusObject.transform.localPosition = new Vector3(0f, 0.06f, 0f);
            radiusObject.transform.localScale = new Vector3(2.4f, 0.012f, 2.4f);
            var collider = radiusObject.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
            radiusObject.GetComponent<Renderer>().sharedMaterial = mint;
            _radius = radiusObject.transform;
            _objects.Add(radiusObject);
        }

        private GameObject CreatePrimitive(string name, PrimitiveType type, Transform parent, Vector3 position, Vector3 scale, Material material)
        {
            var item = GameObject.CreatePrimitive(type);
            item.name = name;
            item.transform.SetParent(parent, false);
            item.transform.localPosition = position;
            item.transform.localScale = scale;
            var collider = item.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
            item.GetComponent<Renderer>().sharedMaterial = material;
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
