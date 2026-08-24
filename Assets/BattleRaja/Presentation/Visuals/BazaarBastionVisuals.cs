using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace BattleRaja.Presentation.Visuals
{
    /// <summary>
    /// Runtime-only, original placeholder art kit for Bazaar Bastion. It replaces the
    /// unbroken greybox silhouette with a compact toy-box market landmark while leaving
    /// the authored collision and authority data untouched. Every generated object is
    /// render-only and has its collider removed, so it cannot change gameplay outcomes.
    /// </summary>
    public sealed class BazaarBastionVisuals : MonoBehaviour
    {
        [SerializeField] private bool enabledForBuilds = true;
        [SerializeField] private int decorationQuality = 1;

        private readonly List<GameObject> _objects = new List<GameObject>(64);
        private readonly List<Material> _materials = new List<Material>(12);
        private Transform _root;
        private GameObject _lightingObject;

        private void Awake()
        {
            if (!enabledForBuilds) return;
            EnsureLighting();
            if (transform.Find("V1BastionVisuals") != null) return;
            BuildVisualKit();
        }

        private void OnDestroy()
        {
            if (_lightingObject != null) Destroy(_lightingObject);
            for (var i = 0; i < _materials.Count; i++)
            {
                if (_materials[i] != null) Destroy(_materials[i]);
            }
        }

        private void EnsureLighting()
        {
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.22f, 0.28f, 0.34f, 1f);
            RenderSettings.ambientIntensity = 1f;

            var existingLights = FindObjectsByType<Light>();
            if (existingLights != null && existingLights.Length > 0) return;

            _lightingObject = new GameObject("BazaarKeyLight");
            _lightingObject.transform.SetParent(transform, false);
            _lightingObject.transform.rotation = Quaternion.Euler(52f, -28f, 0f);
            var light = _lightingObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1f, 0.86f, 0.72f, 1f);
            light.intensity = 1.15f;
            light.shadows = LightShadows.None;
        }

        private void BuildVisualKit()
        {
            _root = new GameObject("V1BastionVisuals").transform;
            _root.SetParent(transform, false);

            var clay = CreateMaterial("V1 Clay", new Color(0.82f, 0.25f, 0.12f, 1f));
            var saffron = CreateMaterial("V1 Saffron", new Color(1f, 0.57f, 0.13f, 1f));
            var teal = CreateMaterial("V1 Teal", new Color(0.08f, 0.42f, 0.45f, 1f));
            var mint = CreateMaterial("V1 Mint", new Color(0.28f, 0.82f, 0.70f, 1f));
            var violet = CreateMaterial("V1 Violet", new Color(0.55f, 0.22f, 0.75f, 1f));
            var cream = CreateMaterial("V1 Cream", new Color(1f, 0.84f, 0.52f, 1f));
            var gold = CreateMaterial("V1 Gold", new Color(1f, 0.78f, 0.16f, 1f));
            var dark = CreateMaterial("V1 Ink", new Color(0.07f, 0.10f, 0.14f, 1f));
            var sand = CreateMaterial("V1 Sand", new Color(0.78f, 0.53f, 0.30f, 1f));
            var rose = CreateMaterial("V1 Rose", new Color(0.92f, 0.22f, 0.38f, 1f));
            var jade = CreateMaterial("V1 Jade", new Color(0.10f, 0.62f, 0.48f, 1f));
            var brick = CreateMaterial("V1 Brick", new Color(0.58f, 0.18f, 0.12f, 1f));
            var sky = CreateMaterial("V1 Sky", new Color(0.18f, 0.55f, 0.76f, 1f));
            var groundBase = CreateMaterial("V1 Ground Base", new Color(0.23f, 0.18f, 0.20f, 1f));
            var groundAlt = CreateMaterial("V1 Ground Alt", new Color(0.29f, 0.22f, 0.22f, 1f));
            var groundAccent = CreateMaterial("V1 Ground Accent", new Color(0.40f, 0.26f, 0.22f, 1f));

            // One render-only mesh replaces the flat brown plane with a quiet, readable
            // tile rhythm. It has no collider and never participates in authority or
            // navigation; the authored collision representation remains the source of truth.
            CreateGroundMosaic(groundBase, groundAlt, groundAccent);

            // A small central landmark gives the arena a readable centre without blocking
            // the existing navigation lanes. It is intentionally low and collider-free.
            CreateCylinder("PlazaOuter", new Vector3(0f, 0.035f, 0f), new Vector3(9.2f, 0.05f, 9.2f), dark);
            CreateCylinder("PlazaInner", new Vector3(0f, 0.062f, 0f), new Vector3(7.4f, 0.035f, 7.4f), sand);
            CreateCylinder("BastionPlinth", new Vector3(0f, 0.08f, 0f), new Vector3(2.5f, 0.16f, 2.5f), teal);
            CreateCylinder("BastionCrown", new Vector3(0f, 0.76f, 0f), new Vector3(0.55f, 0.66f, 0.55f), saffron);
            CreateCylinder("BastionCrownCap", new Vector3(0f, 1.48f, 0f), new Vector3(1.48f, 0.12f, 1.48f), cream);
            CreateCylinder("BastionCrownOrb", new Vector3(0f, 1.78f, 0f), new Vector3(0.28f, 0.28f, 0.28f), gold);
            // The old prototype used two intersecting bars here. Besides reading as a
            // greybox cross from the elevated camera, that shape risked accidental
            // sacred-signifier readings. A six-panel bazaar canopy keeps the landmark
            // playful, fictional and legible without changing the authored collision.
            for (var panel = 0; panel < 6; panel++)
            {
                var angle = panel * 60f;
                var radians = angle * Mathf.Deg2Rad;
                var panelPosition = new Vector3(Mathf.Sin(radians) * 0.72f, 1.62f, Mathf.Cos(radians) * 0.72f);
                var panelMaterial = panel % 2 == 0 ? saffron : teal;
                CreateBlock("BastionAwning" + panel, panelPosition, new Vector3(0.92f, 0.08f, 0.44f), panelMaterial, Quaternion.Euler(0f, angle, 8f));
                CreateCylinder("BastionAwningTassel" + panel, panelPosition + new Vector3(Mathf.Sin(radians) * 0.28f, -0.18f, Mathf.Cos(radians) * 0.28f), new Vector3(0.08f, 0.16f, 0.08f), panel % 2 == 0 ? cream : mint);
            }

            // Layered floor tiles and a gate-like landmark give the arena a place identity
            // from the mobile camera while all geometry remains collider-free decoration.
            CreateFloorTileBand("TileBandNorth", new Vector3(0f, 0.018f, 4.4f), new Vector3(13.5f, 0.018f, 0.68f), teal, cream);
            CreateFloorTileBand("TileBandSouth", new Vector3(0f, 0.019f, -4.4f), new Vector3(13.5f, 0.018f, 0.68f), mint, cream);
            CreateGate("BastionGateNorth", new Vector3(0f, 0f, 9.0f), teal, saffron, cream);
            CreateGate("BastionGateSouth", new Vector3(0f, 0f, -9.0f), violet, mint, cream);

            CreateGroundStripe("RouteStripeNorth", new Vector3(0f, 0.012f, 7.4f), new Vector3(15.5f, 0.025f, 0.22f), saffron);
            CreateGroundStripe("RouteStripeSouth", new Vector3(0f, 0.012f, -7.4f), new Vector3(15.5f, 0.025f, 0.22f), mint);
            CreateGroundStripe("RouteStripeWest", new Vector3(-11.4f, 0.014f, 0f), new Vector3(0.22f, 0.025f, 12.5f), violet);
            CreateGroundStripe("RouteStripeEast", new Vector3(11.4f, 0.014f, 0f), new Vector3(0.22f, 0.025f, 12.5f), clay);

            for (var i = 0; i < 8; i++)
            {
                var angle = i * 45f * Mathf.Deg2Rad;
                var position = new Vector3(Mathf.Cos(angle) * 5.8f, 0.10f, Mathf.Sin(angle) * 5.8f);
                var markerMaterial = i % 2 == 0 ? cream : rose;
                CreateBlock("PlazaMarker" + i, position, new Vector3(0.58f, 0.08f, 0.28f), markerMaterial, Quaternion.Euler(0f, -i * 45f, 0f));
            }

            CreateMarketStall("StallNorthWest", new Vector3(-9.8f, 0f, 7.5f), clay, saffron, Quaternion.Euler(0f, 12f, 0f));
            CreateMarketStall("StallNorthEast", new Vector3(9.8f, 0f, 7.5f), teal, cream, Quaternion.Euler(0f, -12f, 0f));
            CreateMarketStall("StallSouthWest", new Vector3(-9.8f, 0f, -7.5f), violet, mint, Quaternion.Euler(0f, -12f, 0f));
            CreateMarketStall("StallSouthEast", new Vector3(9.8f, 0f, -7.5f), clay, cream, Quaternion.Euler(0f, 12f, 0f));

            if (decorationQuality > 0)
            {
                CreateBanner("BannerNorth", new Vector3(0f, 3.0f, 9.2f), saffron, Quaternion.Euler(0f, 180f, 0f));
                CreateBanner("BannerSouth", new Vector3(0f, 3.0f, -9.2f), mint, Quaternion.identity);
                CreateBanner("BannerWest", new Vector3(-13.2f, 3.0f, 0f), violet, Quaternion.Euler(0f, 90f, 0f));
                CreateBanner("BannerEast", new Vector3(13.2f, 3.0f, 0f), clay, Quaternion.Euler(0f, -90f, 0f));
                CreateLantern("LanternNorth", new Vector3(-4.8f, 2.6f, 8.6f), cream, dark);
                CreateLantern("LanternSouth", new Vector3(4.8f, 2.6f, -8.6f), cream, dark);
                CreateLantern("LanternWest", new Vector3(-8.7f, 2.2f, -4.8f), mint, dark);
                CreateLantern("LanternEast", new Vector3(8.7f, 2.2f, 4.8f), saffron, dark);
                CreateCrateStack("CratesNorthWest", new Vector3(-7.0f, 0.0f, 5.8f), brick, saffron);
                CreateCrateStack("CratesSouthEast", new Vector3(7.0f, 0.0f, -5.8f), teal, cream);
                CreateRug("RugWest", new Vector3(-8.0f, 0.03f, 0f), new Vector3(2.4f, 0.025f, 4.8f), violet, mint);
                CreateRug("RugEast", new Vector3(8.0f, 0.03f, 0f), new Vector3(2.4f, 0.025f, 4.8f), clay, saffron);
                CreatePalmCluster("PalmNorthEast", new Vector3(10.4f, 0f, 5.0f), jade, mint);
                CreatePalmCluster("PalmSouthWest", new Vector3(-10.4f, 0f, -5.0f), jade, sky);
            }
        }

        private void CreateFloorTileBand(string name, Vector3 position, Vector3 scale, Material primary, Material secondary)
        {
            CreateBlock(name, position, scale, primary, Quaternion.identity);
            for (var i = -4; i <= 4; i++)
            {
                var tile = new Vector3(position.x + i * 1.45f, position.y + 0.018f, position.z);
                CreateBlock(name + "Tile" + i, tile, new Vector3(0.72f, 0.022f, scale.z * 1.2f), secondary, Quaternion.Euler(0f, (i & 1) == 0 ? 0f : 45f, 0f));
            }
        }

        private void CreateGate(string name, Vector3 position, Material pillar, Material trim, Material cap)
        {
            var root = new GameObject(name).transform;
            root.SetParent(_root, false);
            root.localPosition = position;
            CreateBlock("Left", new Vector3(-2.7f, 1.15f, 0f), new Vector3(0.48f, 2.3f, 0.48f), pillar, Quaternion.identity, root);
            CreateBlock("Right", new Vector3(2.7f, 1.15f, 0f), new Vector3(0.48f, 2.3f, 0.48f), pillar, Quaternion.identity, root);
            CreateBlock("Lintel", new Vector3(0f, 2.25f, 0f), new Vector3(5.9f, 0.42f, 0.52f), trim, Quaternion.identity, root);
            CreateBlock("Cap", new Vector3(0f, 2.58f, 0f), new Vector3(6.4f, 0.12f, 0.70f), cap, Quaternion.Euler(0f, 0f, 2f), root);
            CreateBlock("Pennant", new Vector3(0f, 3.02f, 0f), new Vector3(0.12f, 0.70f, 0.08f), trim, Quaternion.identity, root);
        }

        private void CreateCrateStack(string name, Vector3 position, Material crate, Material trim)
        {
            var root = new GameObject(name).transform;
            root.SetParent(_root, false);
            root.localPosition = position;
            CreateBlock("CrateA", new Vector3(-0.48f, 0.42f, 0f), new Vector3(0.82f, 0.82f, 0.82f), crate, Quaternion.Euler(0f, 12f, 0f), root);
            CreateBlock("CrateB", new Vector3(0.38f, 0.40f, 0.08f), new Vector3(0.76f, 0.76f, 0.76f), trim, Quaternion.Euler(0f, -18f, 0f), root);
            CreateBlock("CrateTop", new Vector3(-0.04f, 1.16f, 0.02f), new Vector3(0.70f, 0.54f, 0.70f), crate, Quaternion.Euler(0f, 22f, 0f), root);
            CreateBlock("Strap", new Vector3(-0.04f, 1.16f, -0.36f), new Vector3(0.48f, 0.10f, 0.06f), trim, Quaternion.identity, root);
        }

        private void CreateRug(string name, Vector3 position, Vector3 scale, Material baseMaterial, Material stripeMaterial)
        {
            CreateBlock(name, position, scale, baseMaterial, Quaternion.identity);
            var stripeScale = new Vector3(scale.x * 0.82f, scale.y * 1.35f, 0.12f);
            for (var i = -2; i <= 2; i++)
            {
                CreateBlock(name + "Stripe" + i, position + new Vector3(0f, 0.02f, i * 0.72f), stripeScale, stripeMaterial, Quaternion.identity);
            }
        }

        private void CreatePalmCluster(string name, Vector3 position, Material trunk, Material leaves)
        {
            var root = new GameObject(name).transform;
            root.SetParent(_root, false);
            root.localPosition = position;
            CreateCylinder("Trunk", new Vector3(0f, 1.0f, 0f), new Vector3(0.18f, 1.0f, 0.18f), trunk, root);
            for (var i = 0; i < 5; i++)
            {
                var angle = i * 72f * Mathf.Deg2Rad;
                var leafPosition = new Vector3(Mathf.Cos(angle) * 0.58f, 2.0f, Mathf.Sin(angle) * 0.58f);
                CreateBlock("Leaf" + i, leafPosition, new Vector3(0.16f, 0.06f, 0.72f), leaves, Quaternion.Euler(0f, -i * 72f, -18f), root);
            }
        }

        private void CreateMarketStall(string name, Vector3 position, Material wall, Material canopy, Quaternion rotation)
        {
            var root = new GameObject(name).transform;
            root.SetParent(_root, false);
            root.localPosition = position;
            root.localRotation = rotation;
            CreateBlock("Counter", new Vector3(0f, 0.65f, 0f), new Vector3(2.6f, 0.8f, 1.0f), wall, Quaternion.identity, root);
            CreateBlock("Canopy", new Vector3(0f, 2.15f, 0f), new Vector3(3.0f, 0.14f, 1.4f), canopy, Quaternion.Euler(0f, 0f, 3f), root);
            CreateCylinder("PostA", new Vector3(-1.2f, 1.45f, 0f), new Vector3(0.10f, 0.85f, 0.10f), wall, root);
            CreateCylinder("PostB", new Vector3(1.2f, 1.45f, 0f), new Vector3(0.10f, 0.85f, 0.10f), wall, root);
        }

        private void CreateBanner(string name, Vector3 position, Material material, Quaternion rotation)
        {
            CreateBlock(name, position, new Vector3(0.10f, 1.1f, 0.75f), material, rotation);
            CreateBlock(name + "Top", position + Vector3.up * 0.72f, new Vector3(0.14f, 0.10f, 0.95f), material, rotation);
        }

        private void CreateLantern(string name, Vector3 position, Material glow, Material frame)
        {
            CreateCylinder(name + "Frame", position, new Vector3(0.28f, 0.32f, 0.28f), frame);
            CreateCylinder(name + "Glow", position + Vector3.down * 0.03f, new Vector3(0.18f, 0.23f, 0.18f), glow);
        }

        private void CreateGroundStripe(string name, Vector3 position, Vector3 scale, Material material)
        {
            CreateBlock(name, position, scale, material, Quaternion.identity);
        }

        private void CreateGroundMosaic(Material baseMaterial, Material alternateMaterial, Material accentMaterial)
        {
            const int grid = 32;
            const float tileSize = 0.8f;
            const float origin = -12.8f;
            var ground = new GameObject("GroundMosaic", typeof(MeshFilter), typeof(MeshRenderer));
            ground.transform.SetParent(_root, false);
            ground.transform.localPosition = new Vector3(0f, 0.006f, 0f);

            var vertices = new List<Vector3>(grid * grid * 4);
            var uvs = new List<Vector2>(grid * grid * 4);
            var baseTriangles = new List<int>(grid * grid * 6);
            var alternateTriangles = new List<int>(grid * grid * 6);
            var accentTriangles = new List<int>(grid * grid * 6);
            for (var z = 0; z < grid; z++)
            {
                for (var x = 0; x < grid; x++)
                {
                    var minX = origin + x * tileSize;
                    var minZ = origin + z * tileSize;
                    var start = vertices.Count;
                    vertices.Add(new Vector3(minX, 0f, minZ));
                    vertices.Add(new Vector3(minX, 0f, minZ + tileSize));
                    vertices.Add(new Vector3(minX + tileSize, 0f, minZ + tileSize));
                    vertices.Add(new Vector3(minX + tileSize, 0f, minZ));
                    uvs.Add(new Vector2(0f, 0f));
                    uvs.Add(new Vector2(0f, 1f));
                    uvs.Add(new Vector2(1f, 1f));
                    uvs.Add(new Vector2(1f, 0f));

                    var triangles = (Mathf.Abs(x - grid / 2) <= 2 && Mathf.Abs(z - grid / 2) <= 2)
                        || x == 0 || z == 0 || x == grid - 1 || z == grid - 1
                        ? accentTriangles
                        : ((x + z) & 1) == 0 ? baseTriangles : alternateTriangles;
                    triangles.Add(start);
                    triangles.Add(start + 1);
                    triangles.Add(start + 2);
                    triangles.Add(start + 2);
                    triangles.Add(start + 3);
                    triangles.Add(start);
                }
            }

            var mesh = new Mesh { name = "V1GroundMosaicMesh" };
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.subMeshCount = 3;
            mesh.SetTriangles(baseTriangles, 0, true);
            mesh.SetTriangles(alternateTriangles, 1, true);
            mesh.SetTriangles(accentTriangles, 2, true);
            mesh.RecalculateBounds();
            var filter = ground.GetComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            var renderer = ground.GetComponent<MeshRenderer>();
            renderer.sharedMaterials = new[] { baseMaterial, alternateMaterial, accentMaterial };
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.lightProbeUsage = LightProbeUsage.Off;
            _objects.Add(ground);
        }

        private GameObject CreateBlock(string name, Vector3 position, Vector3 scale, Material material, Quaternion rotation, Transform parent = null)
        {
            var objectToCreate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objectToCreate.name = name;
            objectToCreate.transform.SetParent(parent != null ? parent : _root, false);
            objectToCreate.transform.localPosition = position;
            objectToCreate.transform.localRotation = rotation;
            objectToCreate.transform.localScale = scale;
            ConfigureRenderer(objectToCreate, material);
            _objects.Add(objectToCreate);
            return objectToCreate;
        }

        private GameObject CreateCylinder(string name, Vector3 position, Vector3 scale, Material material, Transform parent = null)
        {
            var objectToCreate = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            objectToCreate.name = name;
            objectToCreate.transform.SetParent(parent != null ? parent : _root, false);
            objectToCreate.transform.localPosition = position;
            objectToCreate.transform.localScale = scale;
            ConfigureRenderer(objectToCreate, material);
            _objects.Add(objectToCreate);
            return objectToCreate;
        }

        private void ConfigureRenderer(GameObject objectToConfigure, Material material)
        {
            var collider = objectToConfigure.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
            var renderer = objectToConfigure.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.On;
                renderer.receiveShadows = true;
            }
        }

        private Material CreateMaterial(string name, Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = new Material(shader) { name = name, color = color };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            _materials.Add(material);
            return material;
        }
    }
}
