using System.Collections.Generic;
using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Gadgets
{
    /// <summary>
    /// Render-only toy-box identity for a world gadget pickup. The pickup component still
    /// owns collection/availability; this child hierarchy only communicates what can be
    /// collected and never participates in collision or authority decisions.
    /// </summary>
    public sealed class GadgetPickupVisuals : MonoBehaviour
    {
        private readonly List<GameObject> _objects = new List<GameObject>(12);
        private readonly List<Material> _materials = new List<Material>(4);
        private Transform _identityRoot;
        private Vector3 _identityBasePosition;

        public Transform IdentityRoot => _identityRoot;

        private void Awake()
        {
            if (transform.Find("GadgetIdentityVisual") != null) return;
            Build();
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

        private void Update()
        {
            if (_identityRoot == null) return;
            var bob = Mathf.Sin(Time.time * 3.2f) * 0.05f;
            _identityRoot.localPosition = _identityBasePosition + Vector3.up * bob;
            _identityRoot.Rotate(0f, 32f * Time.deltaTime, 0f, Space.Self);
            _identityRoot.localScale = Vector3.one * (1f + Mathf.Sin(Time.time * 4.8f) * 0.04f);
        }

        private void Build()
        {
            var pickup = GetComponent<GadgetPickup>();
            var id = pickup != null ? pickup.GadgetId.Value : GadgetDefinition.UmbrellaGuard.GadgetId.Value;
            var root = new GameObject("GadgetIdentityVisual").transform;
            root.SetParent(transform, false);
            root.localPosition = Vector3.up * 0.28f;
            _identityRoot = root;
            _identityBasePosition = root.localPosition;

            var pedestal = CreateMaterial(new Color(0.03f, 0.09f, 0.13f, 1f));
            var accent = id.IndexOf("dhol", System.StringComparison.OrdinalIgnoreCase) >= 0
                ? CreateMaterial(new Color(0.96f, 0.25f, 0.20f, 1f))
                : id.IndexOf("tiffin", System.StringComparison.OrdinalIgnoreCase) >= 0
                    ? CreateMaterial(new Color(0.98f, 0.72f, 0.18f, 1f))
                    : CreateMaterial(new Color(0.70f, 0.30f, 0.96f, 1f));
            var highlight = CreateMaterial(new Color(0.24f, 0.88f, 0.82f, 1f));

            CreatePrimitive("PickupHalo", PrimitiveType.Cylinder, root, new Vector3(0f, -0.18f, 0f), new Vector3(0.72f, 0.018f, 0.72f), pedestal);
            if (id.IndexOf("dhol", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                CreatePrimitive("DholBody", PrimitiveType.Cylinder, root, new Vector3(0f, 0.22f, 0f), new Vector3(0.34f, 0.24f, 0.34f), accent, Quaternion.Euler(90f, 0f, 0f));
                CreatePrimitive("DholRimL", PrimitiveType.Cylinder, root, new Vector3(-0.22f, 0.22f, 0f), new Vector3(0.12f, 0.08f, 0.12f), highlight, Quaternion.Euler(90f, 0f, 0f));
                CreatePrimitive("DholRimR", PrimitiveType.Cylinder, root, new Vector3(0.22f, 0.22f, 0f), new Vector3(0.12f, 0.08f, 0.12f), highlight, Quaternion.Euler(90f, 0f, 0f));
                CreatePrimitive("DholStrap", PrimitiveType.Cube, root, new Vector3(0f, 0.22f, -0.18f), new Vector3(0.08f, 0.42f, 0.08f), highlight, Quaternion.Euler(0f, 0f, 24f));
            }
            else if (id.IndexOf("tiffin", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                CreatePrimitive("TiffinLower", PrimitiveType.Cylinder, root, new Vector3(0f, 0.12f, 0f), new Vector3(0.42f, 0.12f, 0.42f), accent);
                CreatePrimitive("TiffinUpper", PrimitiveType.Cylinder, root, new Vector3(0f, 0.34f, 0f), new Vector3(0.32f, 0.10f, 0.32f), highlight);
                CreatePrimitive("TiffinHandle", PrimitiveType.Cube, root, new Vector3(0f, 0.53f, 0f), new Vector3(0.12f, 0.18f, 0.12f), accent);
                CreatePrimitive("TiffinStripe", PrimitiveType.Cube, root, new Vector3(0f, 0.22f, -0.34f), new Vector3(0.32f, 0.06f, 0.06f), highlight);
            }
            else
            {
                CreatePrimitive("UmbrellaPole", PrimitiveType.Cylinder, root, new Vector3(0f, 0.24f, 0f), new Vector3(0.08f, 0.42f, 0.08f), highlight);
                CreatePrimitive("UmbrellaCanopy", PrimitiveType.Cylinder, root, new Vector3(0f, 0.58f, 0f), new Vector3(0.55f, 0.10f, 0.55f), accent);
                CreatePrimitive("UmbrellaHandle", PrimitiveType.Cube, root, new Vector3(0.12f, 0.14f, 0f), new Vector3(0.08f, 0.22f, 0.08f), highlight, Quaternion.Euler(0f, 0f, -30f));
                CreatePrimitive("UmbrellaStripe", PrimitiveType.Cube, root, new Vector3(0f, 0.68f, 0f), new Vector3(0.12f, 0.03f, 0.58f), highlight, Quaternion.Euler(0f, 45f, 0f));
            }
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
