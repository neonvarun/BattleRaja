using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace BattleRaja.Editor
{
    /// <summary>
    /// Builds the saved, collider-free Bazaar Bastion presentation layer. The scene keeps
    /// its authored collision and navigation objects; this prefab owns only editable
    /// meshes, UVs, materials, textures, backdrop dressing and LOD metadata.
    /// </summary>
    public static class ProductionEnvironmentBuilder
    {
        private const string ArtRoot = "Assets/BattleRaja/Content/Art/V1";
        private const string EnvironmentRoot = ArtRoot + "/Environment";
        private const string MeshRoot = EnvironmentRoot + "/Meshes";
        private const string TextureRoot = EnvironmentRoot + "/Textures";
        private const string MaterialRoot = EnvironmentRoot + "/Materials";
        private const string PrefabRoot = "Assets/BattleRaja/Content/Prefabs/Production";

        public const string EnvironmentPrefabPath = PrefabRoot + "/BazaarBastionProduction.prefab";

        [MenuItem("BattleRaja/Build V1 Bazaar Bastion Environment")]
        public static void BuildAll()
        {
            EnsureFolders();
            if (HasGeneratedAssets()) return;
            Generate();
            Debug.Log("BattleRaja Bazaar Bastion production environment generated.");
        }

        [MenuItem("BattleRaja/Rebuild V1 Bazaar Bastion Environment")]
        public static void RebuildAll()
        {
            EnsureFolders();
            Generate();
            Debug.Log("BattleRaja Bazaar Bastion production environment rebuilt.");
        }

        public static bool HasGeneratedAssets()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(EnvironmentPrefabPath);
            if (prefab == null) return false;
            var ground = prefab.transform.Find("GroundMosaic");
            if (ground == null || ground.GetComponent<MeshFilter>()?.sharedMesh == null) return false;
            if (ground.GetComponent<MeshFilter>().sharedMesh.vertexCount < 3000) return false;
            if (ground.GetComponent<MeshFilter>().sharedMesh.subMeshCount != 3) return false;
            if (prefab.transform.Find("BastionCrownOrb") == null || prefab.transform.Find("BastionAwning0") == null) return false;
            return prefab.GetComponentsInChildren<Collider>(true).Length == 0;
        }

        private static void Generate()
        {
            var textures = BuildTextures();
            var materials = BuildMaterials(textures);
            var meshes = BuildMeshes();
            var root = new GameObject("V1BastionVisuals");
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            // Keep the authored arena grounded on tall phones.  The camera sees
            // beyond the 25.6-unit gameplay bounds during portrait framing; a
            // quiet woven backplate gives those pixels the same Bazaar material
            // language instead of exposing the clear-colour void around the plaza.
            // It is render-only, collider-free and sits below every gameplay tile.
            AddPart(root.transform, "BazaarBackplate", meshes["BackdropBox"], materials["Backdrop"],
                new Vector3(0f, -0.12f, 0f), new Vector3(64f, 0.12f, 64f), Quaternion.identity, false);
            AddPart(root.transform, "GroundMosaic", meshes["GroundMosaic"],
                new[] { materials["GroundBase"], materials["GroundAlt"], materials["GroundAccent"] },
                new Vector3(0f, 0.006f, 0f), Vector3.one, Quaternion.identity, false);

            AddPart(root.transform, "PlazaOuter", meshes["Cylinder"], materials["Ink"],
                new Vector3(0f, 0.035f, 0f), new Vector3(18.4f, 0.10f, 18.4f), Quaternion.identity);
            AddPart(root.transform, "PlazaInner", meshes["Cylinder"], materials["Sand"],
                new Vector3(0f, 0.062f, 0f), new Vector3(14.8f, 0.07f, 14.8f), Quaternion.identity);
            AddPart(root.transform, "BastionPlinth", meshes["Cylinder"], materials["Teal"],
                new Vector3(0f, 0.08f, 0f), new Vector3(5.0f, 0.32f, 5.0f), Quaternion.identity);
            AddPart(root.transform, "BastionCrown", meshes["TaperedColumn"], materials["Saffron"],
                new Vector3(0f, 0.76f, 0f), new Vector3(1.10f, 1.32f, 1.10f), Quaternion.identity);
            AddPart(root.transform, "BastionCrownCap", meshes["Cylinder"], materials["Cream"],
                new Vector3(0f, 1.48f, 0f), new Vector3(2.96f, 0.24f, 2.96f), Quaternion.identity);
            AddPart(root.transform, "BastionCrownOrb", meshes["Diamond"], materials["Gold"],
                new Vector3(0f, 1.78f, 0f), new Vector3(0.56f, 0.56f, 0.56f), Quaternion.identity);

            for (var panel = 0; panel < 6; panel++)
            {
                var angle = panel * 60f;
                var radians = angle * Mathf.Deg2Rad;
                var panelPosition = new Vector3(Mathf.Sin(radians) * 0.72f, 1.62f, Mathf.Cos(radians) * 0.72f);
                AddPart(root.transform, "BastionAwning" + panel, meshes["Awning"],
                    panel % 2 == 0 ? materials["Saffron"] : materials["Teal"], panelPosition,
                    new Vector3(0.92f, 0.16f, 0.44f), Quaternion.Euler(0f, angle, 8f));
                AddPart(root.transform, "BastionAwningTassel" + panel, meshes["Diamond"],
                    panel % 2 == 0 ? materials["Cream"] : materials["Mint"],
                    panelPosition + new Vector3(Mathf.Sin(radians) * 0.28f, -0.18f, Mathf.Cos(radians) * 0.28f),
                    new Vector3(0.16f, 0.32f, 0.16f), Quaternion.identity);
            }

            AddBand(root.transform, "TileBandNorth", new Vector3(0f, 0.018f, 4.4f), materials["Teal"], materials["Cream"], meshes);
            AddBand(root.transform, "TileBandSouth", new Vector3(0f, 0.019f, -4.4f), materials["Mint"], materials["Cream"], meshes);
            AddGate(root.transform, "BastionGateNorth", new Vector3(0f, 0f, 9.0f), materials["Teal"], materials["Saffron"], materials["Cream"], meshes);
            AddGate(root.transform, "BastionGateSouth", new Vector3(0f, 0f, -9.0f), materials["Violet"], materials["Mint"], materials["Cream"], meshes);

            AddGroundStripe(root.transform, "RouteStripeNorth", new Vector3(0f, 0.012f, 7.4f), new Vector3(15.5f, 0.025f, 0.22f), materials["Saffron"], meshes);
            AddGroundStripe(root.transform, "RouteStripeSouth", new Vector3(0f, 0.012f, -7.4f), new Vector3(15.5f, 0.025f, 0.22f), materials["Mint"], meshes);
            AddGroundStripe(root.transform, "RouteStripeWest", new Vector3(-11.4f, 0.014f, 0f), new Vector3(0.22f, 0.025f, 12.5f), materials["Violet"], meshes);
            AddGroundStripe(root.transform, "RouteStripeEast", new Vector3(11.4f, 0.014f, 0f), new Vector3(0.22f, 0.025f, 12.5f), materials["Clay"], meshes);

            for (var i = 0; i < 8; i++)
            {
                var angle = i * 45f * Mathf.Deg2Rad;
                var position = new Vector3(Mathf.Cos(angle) * 5.8f, 0.10f, Mathf.Sin(angle) * 5.8f);
                AddPart(root.transform, "PlazaMarker" + i, meshes["Awning"], i % 2 == 0 ? materials["Cream"] : materials["Rose"],
                    position, new Vector3(0.58f, 0.08f, 0.28f), Quaternion.Euler(0f, -i * 45f, 0f));
            }

            AddStall(root.transform, "StallNorthWest", new Vector3(-9.8f, 0f, 7.5f), materials["Clay"], materials["Saffron"], Quaternion.Euler(0f, 12f, 0f), meshes);
            AddStall(root.transform, "StallNorthEast", new Vector3(9.8f, 0f, 7.5f), materials["Teal"], materials["Cream"], Quaternion.Euler(0f, -12f, 0f), meshes);
            AddStall(root.transform, "StallSouthWest", new Vector3(-9.8f, 0f, -7.5f), materials["Violet"], materials["Mint"], Quaternion.Euler(0f, -12f, 0f), meshes);
            AddStall(root.transform, "StallSouthEast", new Vector3(9.8f, 0f, -7.5f), materials["Clay"], materials["Cream"], Quaternion.Euler(0f, 12f, 0f), meshes);

            AddBanner(root.transform, "BannerNorth", new Vector3(0f, 3.0f, 9.2f), materials["Saffron"], Quaternion.Euler(0f, 180f, 0f), meshes);
            AddBanner(root.transform, "BannerSouth", new Vector3(0f, 3.0f, -9.2f), materials["Mint"], Quaternion.identity, meshes);
            AddBanner(root.transform, "BannerWest", new Vector3(-13.2f, 3.0f, 0f), materials["Violet"], Quaternion.Euler(0f, 90f, 0f), meshes);
            AddBanner(root.transform, "BannerEast", new Vector3(13.2f, 3.0f, 0f), materials["Clay"], Quaternion.Euler(0f, -90f, 0f), meshes);
            AddLantern(root.transform, "LanternNorth", new Vector3(-4.8f, 2.6f, 8.6f), materials["Cream"], materials["Ink"], meshes);
            AddLantern(root.transform, "LanternSouth", new Vector3(4.8f, 2.6f, -8.6f), materials["Cream"], materials["Ink"], meshes);
            AddLantern(root.transform, "LanternWest", new Vector3(-8.7f, 2.2f, -4.8f), materials["Mint"], materials["Ink"], meshes);
            AddLantern(root.transform, "LanternEast", new Vector3(8.7f, 2.2f, 4.8f), materials["Saffron"], materials["Ink"], meshes);
            AddCrateStack(root.transform, "CratesNorthWest", new Vector3(-7.0f, 0f, 5.8f), materials["Brick"], materials["Saffron"], meshes);
            AddCrateStack(root.transform, "CratesSouthEast", new Vector3(7.0f, 0f, -5.8f), materials["Teal"], materials["Cream"], meshes);
            AddRug(root.transform, "RugWest", new Vector3(-8.0f, 0.03f, 0f), new Vector3(2.4f, 0.025f, 4.8f), materials["Violet"], materials["Mint"], meshes);
            AddRug(root.transform, "RugEast", new Vector3(8.0f, 0.03f, 0f), new Vector3(2.4f, 0.025f, 4.8f), materials["Clay"], materials["Saffron"], meshes);
            AddPalm(root.transform, "PalmNorthEast", new Vector3(10.4f, 0f, 5.0f), materials["Jade"], materials["Mint"], meshes);
            AddPalm(root.transform, "PalmSouthWest", new Vector3(-10.4f, 0f, -5.0f), materials["Jade"], materials["Sky"], meshes);
            AddBackdrop(root.transform, materials, meshes);

            PrefabUtility.SaveAsPrefabAsset(root, EnvironmentPrefabPath);
            UnityEngine.Object.DestroyImmediate(root);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static Dictionary<string, Texture2D> BuildTextures()
        {
            return new Dictionary<string, Texture2D>(StringComparer.Ordinal)
            {
                ["GroundBase"] = CreateTexture("GroundBase", new Color32(55, 45, 48, 255), new Color32(69, 54, 53, 255), 0),
                ["GroundAlt"] = CreateTexture("GroundAlt", new Color32(64, 49, 51, 255), new Color32(83, 62, 56, 255), 1),
                ["GroundAccent"] = CreateTexture("GroundAccent", new Color32(104, 69, 52, 255), new Color32(135, 86, 52, 255), 2),
                ["Teal"] = CreateTexture("Teal", new Color32(19, 92, 95, 255), new Color32(30, 125, 119, 255), 1),
                ["Clay"] = CreateTexture("Clay", new Color32(131, 47, 28, 255), new Color32(178, 67, 34, 255), 2),
                ["Saffron"] = CreateTexture("Saffron", new Color32(221, 111, 24, 255), new Color32(255, 160, 35, 255), 3),
                ["Mint"] = CreateTexture("Mint", new Color32(52, 146, 120, 255), new Color32(84, 205, 164, 255), 1),
                ["Violet"] = CreateTexture("Violet", new Color32(111, 44, 145, 255), new Color32(169, 69, 172, 255), 3),
                ["Sand"] = CreateTexture("Sand", new Color32(161, 111, 65, 255), new Color32(200, 145, 80, 255), 0),
                ["Cream"] = CreateTexture("Cream", new Color32(239, 198, 110, 255), new Color32(255, 229, 153, 255), 2),
                ["Ink"] = CreateTexture("Ink", new Color32(13, 23, 30, 255), new Color32(29, 41, 49, 255), 0),
                ["Rose"] = CreateTexture("Rose", new Color32(190, 42, 71, 255), new Color32(242, 78, 99, 255), 3),
                ["Gold"] = CreateTexture("Gold", new Color32(229, 160, 24, 255), new Color32(255, 217, 76, 255), 3),
                ["Brick"] = CreateTexture("Brick", new Color32(113, 39, 28, 255), new Color32(158, 55, 34, 255), 2),
                ["Jade"] = CreateTexture("Jade", new Color32(26, 116, 86, 255), new Color32(50, 166, 119, 255), 1),
                ["Sky"] = CreateTexture("Sky", new Color32(40, 114, 149, 255), new Color32(78, 171, 201, 255), 1)
            };
        }

        private static Texture2D CreateTexture(string name, Color32 baseColor, Color32 accent, int pattern)
        {
            const int size = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false, false) { name = name };
            var pixels = new Color32[size * size];
            var baseTone = (Color)baseColor;
            var accentTone = (Color)accent;
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    // Keep the authored Bazaar palette while replacing the hard
                    // checkerboard with low-frequency woven bands. The old pattern
                    // read as debug tiling from the top-down mobile camera; these
                    // restrained highlights give floors, cloth and masonry a shared
                    // material language without competing with gameplay telegraphs.
                    var weave = ((x * 5 + y * 3 + pattern * 13) % 37) < 3 ? 0.13f : 0.025f;
                    var band = ((y + pattern * 7) % 17) < 2 ? 0.08f : 0f;
                    var diagonal = ((x * 3 + y * 2 + pattern * 11) % 43) < 3 ? 0.05f : 0f;
                    var accentMix = Mathf.Clamp01(weave + band + diagonal);
                    var tone = Color.Lerp(baseTone, accentTone, accentMix);
                    var light = 0.95f + 0.05f * Mathf.Sin((x * 0.8f + y * 0.45f + pattern * 9f) * 0.16f);
                    tone *= light;
                    tone.a = 1f;
                    pixels[y * size + x] = tone;
                }
            }
            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.filterMode = FilterMode.Bilinear;
            var path = TextureRoot + "/" + name + ".asset";
            var existing = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (existing != null)
            {
                EditorUtility.CopySerialized(texture, existing);
                existing.name = name;
                EditorUtility.SetDirty(existing);
                UnityEngine.Object.DestroyImmediate(texture);
                return existing;
            }
            AssetDatabase.CreateAsset(texture, path);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        private static Dictionary<string, Material> BuildMaterials(Dictionary<string, Texture2D> textures)
        {
            var colors = new Dictionary<string, Color>
            {
                ["GroundBase"] = new Color(0.23f, 0.18f, 0.20f), ["GroundAlt"] = new Color(0.29f, 0.22f, 0.22f),
                ["GroundAccent"] = new Color(0.40f, 0.26f, 0.22f), ["Teal"] = new Color(0.08f, 0.42f, 0.45f),
                ["Clay"] = new Color(0.82f, 0.25f, 0.12f), ["Saffron"] = new Color(1f, 0.57f, 0.13f),
                ["Mint"] = new Color(0.28f, 0.82f, 0.70f), ["Violet"] = new Color(0.55f, 0.22f, 0.75f),
                ["Sand"] = new Color(0.78f, 0.53f, 0.30f), ["Cream"] = new Color(1f, 0.84f, 0.52f),
                ["Ink"] = new Color(0.07f, 0.10f, 0.14f), ["Rose"] = new Color(0.92f, 0.22f, 0.38f),
                ["Gold"] = new Color(1f, 0.78f, 0.16f), ["Brick"] = new Color(0.58f, 0.18f, 0.12f),
                ["Jade"] = new Color(0.10f, 0.62f, 0.48f), ["Sky"] = new Color(0.18f, 0.55f, 0.76f)
            };
            var result = new Dictionary<string, Material>(StringComparer.Ordinal);
            foreach (var pair in colors) result[pair.Key] = CreateMaterial(pair.Key, pair.Value, textures[pair.Key]);
            // The portrait backplate fills the camera outside the playable rim. It
            // should add authored surface language without paying for a full-screen
            // lit/shadowed pass on the target phone, so keep this one material unlit.
            result["Backdrop"] = CreateUnlitMaterial("Backdrop", new Color(0.74f, 0.60f, 0.64f, 1f), textures["GroundBase"]);
            return result;
        }

        private static Material CreateMaterial(string name, Color color, Texture2D texture)
        {
            var path = MaterialRoot + "/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard")) { name = name };
                AssetDatabase.CreateAsset(material, path);
            }
            material.color = color;
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            if (material.HasProperty("_BaseMap")) material.SetTexture("_BaseMap", texture);
            if (material.HasProperty("_MainTex")) material.SetTexture("_MainTex", texture);
            if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", 0.28f);
            material.enableInstancing = true;
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material CreateUnlitMaterial(string name, Color color, Texture2D texture)
        {
            var path = MaterialRoot + "/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Texture") ?? Shader.Find("Standard");
                material = new Material(shader) { name = name };
                AssetDatabase.CreateAsset(material, path);
            }

            material.color = color;
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            if (material.HasProperty("_Color")) material.SetColor("_Color", color);
            if (material.HasProperty("_BaseMap")) material.SetTexture("_BaseMap", texture);
            if (material.HasProperty("_MainTex")) material.SetTexture("_MainTex", texture);
            material.enableInstancing = true;
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Dictionary<string, Mesh> BuildMeshes()
        {
            var meshes = new Dictionary<string, Mesh>(StringComparer.Ordinal)
            {
                ["GroundMosaic"] = CreateGroundMosaic(),
                ["BackdropBox"] = CreateTiledBox("BackdropBox", 18f),
                ["Box"] = CreateBox("EnvironmentBox"),
                ["Cylinder"] = CreateCylinder("EnvironmentCylinder", 16),
                ["TaperedColumn"] = CreateTaperedColumn("EnvironmentTaperedColumn", 14),
                ["Diamond"] = CreateDiamond("EnvironmentDiamond"),
                ["Awning"] = CreateAwning("EnvironmentAwning")
            };
            var keys = new List<string>(meshes.Keys);
            for (var i = 0; i < keys.Count; i++) meshes[keys[i]] = SaveMesh(keys[i], meshes[keys[i]]);
            return meshes;
        }

        private static Mesh CreateGroundMosaic()
        {
            const int grid = 32;
            const float tileSize = 0.8f;
            const float origin = -12.8f;
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
                    vertices.Add(new Vector3(minX, 0f, minZ)); vertices.Add(new Vector3(minX, 0f, minZ + tileSize));
                    vertices.Add(new Vector3(minX + tileSize, 0f, minZ + tileSize)); vertices.Add(new Vector3(minX + tileSize, 0f, minZ));
                    uvs.Add(new Vector2(0f, 0f)); uvs.Add(new Vector2(0f, 1f)); uvs.Add(new Vector2(1f, 1f)); uvs.Add(new Vector2(1f, 0f));
                    var triangles = ((Mathf.Abs(x - grid / 2) <= 2 && Mathf.Abs(z - grid / 2) <= 2) || x == 0 || z == 0 || x == grid - 1 || z == grid - 1)
                        ? accentTriangles : ((x + z) & 1) == 0 ? baseTriangles : alternateTriangles;
                    triangles.Add(start); triangles.Add(start + 1); triangles.Add(start + 2);
                    triangles.Add(start + 2); triangles.Add(start + 3); triangles.Add(start);
                }
            }
            var mesh = new Mesh { name = "GroundMosaic" };
            mesh.SetVertices(vertices); mesh.SetUVs(0, uvs); mesh.subMeshCount = 3;
            mesh.SetTriangles(baseTriangles, 0, true); mesh.SetTriangles(alternateTriangles, 1, true); mesh.SetTriangles(accentTriangles, 2, true);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Mesh CreateBox(string name)
        {
            var v = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
            };
            var t = new[] { 0, 2, 1, 0, 3, 2, 4, 5, 6, 4, 6, 7, 0, 1, 5, 0, 5, 4, 1, 2, 6, 1, 6, 5, 2, 3, 7, 2, 7, 6, 3, 0, 4, 3, 4, 7 };
            return CreateMesh(name, v, t, BoxUv(v.Length));
        }

        private static Mesh CreateTiledBox(string name, float tileRepeat)
        {
            var mesh = CreateBox(name);
            var repeat = Mathf.Max(1f, tileRepeat);
            mesh.uv = new[]
            {
                new Vector2(0f, 0f), new Vector2(repeat, 0f), new Vector2(repeat, repeat), new Vector2(0f, repeat),
                new Vector2(0f, 0f), new Vector2(repeat, 0f), new Vector2(repeat, repeat), new Vector2(0f, repeat)
            };
            return mesh;
        }

        private static Mesh CreateCylinder(string name, int sides)
        {
            var vertices = new Vector3[sides * 2 + 2];
            var uvs = new Vector2[vertices.Length];
            for (var i = 0; i < sides; i++)
            {
                var a = i * Mathf.PI * 2f / sides;
                vertices[i] = new Vector3(Mathf.Cos(a) * 0.5f, -0.5f, Mathf.Sin(a) * 0.5f);
                vertices[sides + i] = new Vector3(Mathf.Cos(a) * 0.5f, 0.5f, Mathf.Sin(a) * 0.5f);
                uvs[i] = new Vector2((float)i / sides, 0f); uvs[sides + i] = new Vector2((float)i / sides, 1f);
            }
            var bottom = sides * 2; var top = bottom + 1; vertices[bottom] = Vector3.down * 0.5f; vertices[top] = Vector3.up * 0.5f;
            var triangles = new List<int>(sides * 12);
            for (var i = 0; i < sides; i++)
            {
                var n = (i + 1) % sides;
                triangles.Add(i); triangles.Add(n); triangles.Add(sides + n); triangles.Add(i); triangles.Add(sides + n); triangles.Add(sides + i);
                triangles.Add(bottom); triangles.Add(n); triangles.Add(i); triangles.Add(top); triangles.Add(sides + i); triangles.Add(sides + n);
            }
            return CreateMesh(name, vertices, triangles.ToArray(), uvs);
        }

        private static Mesh CreateTaperedColumn(string name, int sides)
        {
            var vertices = new Vector3[sides * 2 + 2];
            var uvs = new Vector2[vertices.Length];
            for (var i = 0; i < sides; i++)
            {
                var a = i * Mathf.PI * 2f / sides;
                vertices[i] = new Vector3(Mathf.Cos(a) * 0.5f, -0.5f, Mathf.Sin(a) * 0.5f);
                vertices[sides + i] = new Vector3(Mathf.Cos(a) * 0.36f, 0.5f, Mathf.Sin(a) * 0.36f);
                uvs[i] = new Vector2((float)i / sides, 0f); uvs[sides + i] = new Vector2((float)i / sides, 1f);
            }
            var bottom = sides * 2; var top = bottom + 1; vertices[bottom] = Vector3.down * 0.5f; vertices[top] = Vector3.up * 0.5f;
            var triangles = new List<int>(sides * 12);
            for (var i = 0; i < sides; i++)
            {
                var n = (i + 1) % sides;
                triangles.Add(i); triangles.Add(n); triangles.Add(sides + n); triangles.Add(i); triangles.Add(sides + n); triangles.Add(sides + i);
                triangles.Add(bottom); triangles.Add(n); triangles.Add(i); triangles.Add(top); triangles.Add(sides + i); triangles.Add(sides + n);
            }
            return CreateMesh(name, vertices, triangles.ToArray(), uvs);
        }

        private static Mesh CreateDiamond(string name)
        {
            var v = new[] { Vector3.up * 0.5f, Vector3.right * 0.5f, Vector3.forward * 0.5f, Vector3.left * 0.5f, Vector3.back * 0.5f, Vector3.down * 0.5f };
            var t = new[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1, 5, 2, 1, 5, 3, 2, 5, 4, 3, 5, 1, 4 };
            return CreateMesh(name, v, t, BoxUv(v.Length));
        }

        private static Mesh CreateAwning(string name)
        {
            var mesh = CreateBox(name);
            return mesh;
        }

        private static Mesh CreateMesh(string name, Vector3[] vertices, int[] triangles, Vector2[] uv)
        {
            var mesh = new Mesh { name = name };
            mesh.vertices = vertices; mesh.triangles = triangles; mesh.uv = uv; mesh.RecalculateNormals(); mesh.RecalculateBounds();
            return mesh;
        }

        private static Vector2[] BoxUv(int count)
        {
            var uv = new Vector2[count];
            for (var i = 0; i < count; i++) uv[i] = new Vector2((i & 1) == 0 ? 0f : 1f, (i & 2) == 0 ? 0f : 1f);
            return uv;
        }

        private static Mesh SaveMesh(string name, Mesh mesh)
        {
            var path = MeshRoot + "/" + name + ".asset";
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing != null)
            {
                EditorUtility.CopySerialized(mesh, existing); existing.name = name; EditorUtility.SetDirty(existing); UnityEngine.Object.DestroyImmediate(mesh); return existing;
            }
            mesh.name = name; AssetDatabase.CreateAsset(mesh, path); return AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        private static void AddPart(Transform parent, string name, Mesh mesh, Material material, Vector3 position, Vector3 scale, Quaternion rotation, bool castShadows = true)
        {
            AddPart(parent, name, mesh, new[] { material }, position, scale, rotation, castShadows);
        }

        private static void AddPart(Transform parent, string name, Mesh mesh, Material[] materials, Vector3 position, Vector3 scale, Quaternion rotation, bool castShadows = true)
        {
            var objectToAdd = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            objectToAdd.transform.SetParent(parent, false); objectToAdd.transform.localPosition = position; objectToAdd.transform.localRotation = rotation; objectToAdd.transform.localScale = scale;
            objectToAdd.GetComponent<MeshFilter>().sharedMesh = mesh;
            var renderer = objectToAdd.GetComponent<MeshRenderer>(); renderer.sharedMaterials = materials; renderer.shadowCastingMode = castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off; renderer.receiveShadows = castShadows; renderer.lightProbeUsage = LightProbeUsage.Off;
        }

        private static void AddBand(Transform parent, string name, Vector3 position, Material primary, Material secondary, Dictionary<string, Mesh> meshes)
        {
            AddPart(parent, name, meshes["Box"], primary, position, new Vector3(13.5f, 0.036f, 0.68f), Quaternion.identity, false);
            for (var i = -4; i <= 4; i++) AddPart(parent, name + "Tile" + i, meshes["Box"], secondary, new Vector3(position.x + i * 1.45f, position.y + 0.018f, position.z), new Vector3(0.72f, 0.044f, 0.82f), Quaternion.Euler(0f, (i & 1) == 0 ? 0f : 45f, 0f), false);
        }

        private static void AddGate(Transform parent, string name, Vector3 position, Material pillar, Material trim, Material cap, Dictionary<string, Mesh> meshes)
        {
            var root = new GameObject(name).transform; root.SetParent(parent, false); root.localPosition = position;
            AddPart(root, "Left", meshes["Box"], pillar, new Vector3(-2.7f, 1.15f, 0f), new Vector3(0.48f, 2.3f, 0.48f), Quaternion.identity);
            AddPart(root, "Right", meshes["Box"], pillar, new Vector3(2.7f, 1.15f, 0f), new Vector3(0.48f, 2.3f, 0.48f), Quaternion.identity);
            AddPart(root, "Lintel", meshes["Box"], trim, new Vector3(0f, 2.25f, 0f), new Vector3(5.9f, 0.42f, 0.52f), Quaternion.identity);
            AddPart(root, "Cap", meshes["Box"], cap, new Vector3(0f, 2.58f, 0f), new Vector3(6.4f, 0.12f, 0.70f), Quaternion.Euler(0f, 0f, 2f));
            AddPart(root, "Pennant", meshes["Diamond"], trim, new Vector3(0f, 3.02f, 0f), new Vector3(0.18f, 0.70f, 0.12f), Quaternion.identity);
        }

        private static void AddStall(Transform parent, string name, Vector3 position, Material wall, Material canopy, Quaternion rotation, Dictionary<string, Mesh> meshes)
        {
            var root = new GameObject(name).transform; root.SetParent(parent, false); root.localPosition = position; root.localRotation = rotation;
            AddPart(root, "Counter", meshes["Box"], wall, new Vector3(0f, 0.65f, 0f), new Vector3(2.6f, 0.8f, 1.0f), Quaternion.identity);
            AddPart(root, "Canopy", meshes["Awning"], canopy, new Vector3(0f, 2.15f, 0f), new Vector3(3.0f, 0.14f, 1.4f), Quaternion.Euler(0f, 0f, 3f));
            AddPart(root, "PostA", meshes["TaperedColumn"], wall, new Vector3(-1.2f, 1.45f, 0f), new Vector3(0.20f, 1.7f, 0.20f), Quaternion.identity);
            AddPart(root, "PostB", meshes["TaperedColumn"], wall, new Vector3(1.2f, 1.45f, 0f), new Vector3(0.20f, 1.7f, 0.20f), Quaternion.identity);
        }

        private static void AddBanner(Transform parent, string name, Vector3 position, Material material, Quaternion rotation, Dictionary<string, Mesh> meshes)
        {
            AddPart(parent, name, meshes["Box"], material, position, new Vector3(0.10f, 1.1f, 0.75f), rotation, false);
            AddPart(parent, name + "Top", meshes["Box"], material, position + Vector3.up * 0.72f, new Vector3(0.14f, 0.10f, 0.95f), rotation, false);
        }

        private static void AddLantern(Transform parent, string name, Vector3 position, Material glow, Material frame, Dictionary<string, Mesh> meshes)
        {
            AddPart(parent, name + "Frame", meshes["Cylinder"], frame, position, new Vector3(0.56f, 0.64f, 0.56f), Quaternion.identity);
            AddPart(parent, name + "Glow", meshes["Cylinder"], glow, position + Vector3.down * 0.03f, new Vector3(0.36f, 0.46f, 0.36f), Quaternion.identity, false);
        }

        private static void AddCrateStack(Transform parent, string name, Vector3 position, Material crate, Material trim, Dictionary<string, Mesh> meshes)
        {
            var root = new GameObject(name).transform; root.SetParent(parent, false); root.localPosition = position;
            AddPart(root, "CrateA", meshes["Box"], crate, new Vector3(-0.48f, 0.42f, 0f), new Vector3(0.82f, 0.82f, 0.82f), Quaternion.Euler(0f, 12f, 0f));
            AddPart(root, "CrateB", meshes["Box"], trim, new Vector3(0.38f, 0.40f, 0.08f), new Vector3(0.76f, 0.76f, 0.76f), Quaternion.Euler(0f, -18f, 0f));
            AddPart(root, "CrateTop", meshes["Box"], crate, new Vector3(-0.04f, 1.16f, 0.02f), new Vector3(0.70f, 0.54f, 0.70f), Quaternion.Euler(0f, 22f, 0f));
            AddPart(root, "Strap", meshes["Box"], trim, new Vector3(-0.04f, 1.16f, -0.36f), new Vector3(0.48f, 0.10f, 0.06f), Quaternion.identity, false);
        }

        private static void AddRug(Transform parent, string name, Vector3 position, Vector3 scale, Material baseMaterial, Material stripeMaterial, Dictionary<string, Mesh> meshes)
        {
            AddPart(parent, name, meshes["Box"], baseMaterial, position, scale, Quaternion.identity, false);
            var stripeScale = new Vector3(scale.x * 0.82f, scale.y * 1.35f, 0.12f);
            for (var i = -2; i <= 2; i++) AddPart(parent, name + "Stripe" + i, meshes["Box"], stripeMaterial, position + new Vector3(0f, 0.02f, i * 0.72f), stripeScale, Quaternion.identity, false);
        }

        private static void AddPalm(Transform parent, string name, Vector3 position, Material trunk, Material leaves, Dictionary<string, Mesh> meshes)
        {
            var root = new GameObject(name).transform; root.SetParent(parent, false); root.localPosition = position;
            AddPart(root, "Trunk", meshes["TaperedColumn"], trunk, new Vector3(0f, 1.0f, 0f), new Vector3(0.36f, 2f, 0.36f), Quaternion.identity);
            for (var i = 0; i < 5; i++)
            {
                var angle = i * 72f * Mathf.Deg2Rad;
                AddPart(root, "Leaf" + i, meshes["Awning"], leaves, new Vector3(Mathf.Cos(angle) * 0.58f, 2.0f, Mathf.Sin(angle) * 0.58f), new Vector3(0.32f, 0.12f, 1.44f), Quaternion.Euler(0f, -i * 72f, -18f), false);
            }
        }

        private static void AddGroundStripe(Transform parent, string name, Vector3 position, Vector3 scale, Material material, Dictionary<string, Mesh> meshes)
        {
            AddPart(parent, name, meshes["Box"], material, position, scale, Quaternion.identity, false);
        }

        private static void AddBackdrop(Transform parent, Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var backdrop = new GameObject("BastionBackdrop").transform; backdrop.SetParent(parent, false);
            var near = new List<Renderer>();
            var far = new List<Renderer>();
            for (var i = 0; i < 8; i++)
            {
                var x = -14f + i * 4f;
                var height = 1.5f + (i % 3) * 0.7f;
                var part = AddPartAndGetRenderer(backdrop, "SkylineNear" + i, meshes["Box"], i % 2 == 0 ? materials["Teal"] : materials["Violet"], new Vector3(x, height * 0.5f, 15.6f), new Vector3(2.4f, height, 0.55f), Quaternion.identity, false);
                near.Add(part);
            }
            for (var i = 0; i < 4; i++)
            {
                var part = AddPartAndGetRenderer(backdrop, "SkylineFar" + i, meshes["Box"], materials["Sky"], new Vector3(-12f + i * 8f, 1.5f, 17.2f), new Vector3(6.8f, 3f, 0.35f), Quaternion.identity, false);
                far.Add(part);
            }
            var lod = backdrop.gameObject.AddComponent<LODGroup>();
            lod.SetLODs(new[] { new LOD(0.28f, near.ToArray()), new LOD(0.06f, far.ToArray()) });
            lod.RecalculateBounds();
        }

        private static Renderer AddPartAndGetRenderer(Transform parent, string name, Mesh mesh, Material material, Vector3 position, Vector3 scale, Quaternion rotation, bool castShadows)
        {
            AddPart(parent, name, mesh, new[] { material }, position, scale, rotation, castShadows);
            return parent.Find(name).GetComponent<Renderer>();
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/BattleRaja/Content/Art"); EnsureFolder(ArtRoot); EnsureFolder(EnvironmentRoot);
            EnsureFolder(MeshRoot); EnsureFolder(TextureRoot); EnsureFolder(MaterialRoot); EnsureFolder("Assets/BattleRaja/Content/Prefabs"); EnsureFolder(PrefabRoot);
        }

        private static void EnsureFolder(string path)
        {
            var parts = path.Split('/'); var current = parts[0];
            for (var i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
