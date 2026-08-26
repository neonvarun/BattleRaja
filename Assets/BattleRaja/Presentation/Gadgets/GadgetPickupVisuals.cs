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
        [SerializeField] private GameObject umbrellaModelPrefab;
        [SerializeField] private GameObject dholModelPrefab;
        [SerializeField] private GameObject tiffinModelPrefab;

        private readonly List<GameObject> _objects = new List<GameObject>(12);
        private readonly List<Material> _materials = new List<Material>(4);
        private Transform _identityRoot;
        private Vector3 _identityBasePosition;

        public Transform IdentityRoot => _identityRoot;

        /// <summary>
        /// Rebuilds the render-only hierarchy after an editor scene generator has
        /// assigned serialized prefab references. AddComponent invokes Awake before
        /// those references can be assigned, so the editor path uses this explicit
        /// refresh to remove the temporary primitive fallback before saving.
        /// </summary>
        public void RebuildFromSavedPrefab()
        {
            // This component owns the pickup's complete render-only child hierarchy.
            // Reuse the serialized anchor when present so Unity does not churn scene
            // file IDs on every controlled generation pass. Remove older generated
            // model children (including legacy direct children) before saving.
            Transform savedIdentityRoot = null;
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                if (child.name == "GadgetIdentityVisual" && savedIdentityRoot == null)
                {
                    savedIdentityRoot = child;
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
            _identityRoot = null;

            if (savedIdentityRoot != null)
            {
                for (var i = savedIdentityRoot.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(savedIdentityRoot.GetChild(i).gameObject);
                }

                _identityRoot = savedIdentityRoot;
                _identityBasePosition = savedIdentityRoot.localPosition;
            }

            for (var i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] != null) DestroyImmediate(_objects[i]);
            }

            for (var i = 0; i < _materials.Count; i++)
            {
                if (_materials[i] != null) DestroyImmediate(_materials[i]);
            }

            _objects.Clear();
            _materials.Clear();

            // Keep the scene serialization lightweight and deterministic. The saved
            // prefab references above are the source of truth; the render-only model is
            // instantiated into this anchor during PlayMode, just like fighter art.
            if (_identityRoot == null)
            {
                var identityRoot = new GameObject("GadgetIdentityVisual").transform;
                identityRoot.SetParent(transform, false);
                identityRoot.localPosition = Vector3.up * 0.28f;
                _identityRoot = identityRoot;
                _identityBasePosition = identityRoot.localPosition;
            }
        }

        /// <summary>Assigns generated prefab references directly during controlled scene generation.</summary>
        public void ConfigureSavedPrefabs(GameObject umbrella, GameObject dhol, GameObject tiffin)
        {
            umbrellaModelPrefab = umbrella;
            dholModelPrefab = dhol;
            tiffinModelPrefab = tiffin;
            RebuildFromSavedPrefab();
        }

        private void Awake()
        {
            // Editor generation persists only the anchor and prefab references. Avoid
            // rebuilding there; PlayMode owns the transient render hierarchy.
            if (!Application.isPlaying) return;

            var existingIdentity = transform.Find("GadgetIdentityVisual");
            if (existingIdentity != null)
            {
                _identityRoot = existingIdentity;
                _identityBasePosition = existingIdentity.localPosition;
                if (existingIdentity.childCount > 0) return;
            }

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
            var root = _identityRoot;
            if (root == null)
            {
                root = new GameObject("GadgetIdentityVisual").transform;
                root.SetParent(transform, false);
                root.localPosition = Vector3.up * 0.28f;
                _identityRoot = root;
                _identityBasePosition = root.localPosition;
            }

            var savedPrefab = SelectSavedPrefab(id);
            if (savedPrefab != null)
            {
                var model = Instantiate(savedPrefab, root, false);
                model.name = savedPrefab.name;
                return;
            }

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

            // A tall, low-cost beacon makes the pickup discoverable from the
            // overhead camera without changing its collider or authority position.
            CreatePrimitive("PickupBeacon", PrimitiveType.Cube, root, new Vector3(0f, 1.08f, 0f), new Vector3(0.055f, 0.72f, 0.055f), highlight);
            CreatePrimitive("PickupBeaconTop", PrimitiveType.Cube, root, new Vector3(0f, 1.48f, 0f), new Vector3(0.24f, 0.055f, 0.24f), accent, Quaternion.Euler(0f, 45f, 0f));
        }

        private GameObject SelectSavedPrefab(string id)
        {
            if (id.IndexOf("dhol", System.StringComparison.OrdinalIgnoreCase) >= 0) return dholModelPrefab;
            if (id.IndexOf("tiffin", System.StringComparison.OrdinalIgnoreCase) >= 0) return tiffinModelPrefab;
            return umbrellaModelPrefab;
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
