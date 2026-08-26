using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleRaja.Editor
{
    /// <summary>
    /// Controlled generator for the V1 render-only production identity assets.
    /// Generated prefabs deliberately contain MeshFilter/MeshRenderer components
    /// only; authority, collision and gameplay state remain in scene components.
    /// </summary>
    public static class ProductionArtBuilder
    {
        private const string ArtRoot = "Assets/BattleRaja/Content/Art/V1";
        private const string MeshRoot = ArtRoot + "/Meshes";
        private const string MaterialRoot = ArtRoot + "/Materials";
        private const string PrefabRoot = "Assets/BattleRaja/Content/Prefabs/Production";

        public const string BijliPrefabPath = PrefabRoot + "/BijliProduction.prefab";
        public const string PehelPrefabPath = PrefabRoot + "/PehelProduction.prefab";
        public const string MayaPrefabPath = PrefabRoot + "/MayaProduction.prefab";
        public const string UmbrellaPrefabPath = PrefabRoot + "/UmbrellaProduction.prefab";
        public const string DholPrefabPath = PrefabRoot + "/DholProduction.prefab";
        public const string TiffinPrefabPath = PrefabRoot + "/TiffinProduction.prefab";

        [MenuItem("BattleRaja/Build V1 Production Fighter Art")]
        public static void BuildAll()
        {
            // Generated production prefabs are committed release inputs. Keep their
            // local file IDs stable during ordinary scene/build generation; a deliberate
            // deletion or fresh checkout still enters the generation path below.
            if (HasGeneratedAssets())
            {
                ProductionPresentationBuilder.BuildAll();
                Debug.Log("BattleRaja production art already exists; keeping committed asset identities.");
                return;
            }

            EnsureFolders();
            var materials = BuildMaterials();
            var meshes = BuildMeshes();
            BuildBijliPrefab(materials, meshes);
            BuildPehelPrefab(materials, meshes);
            BuildMayaPrefab(materials, meshes);
            BuildUmbrellaPrefab(materials, meshes);
            BuildDholPrefab(materials, meshes);
            BuildTiffinPrefab(materials, meshes);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            ProductionPresentationBuilder.BuildAll();
            Debug.Log("BattleRaja production art generated: six render-only V1 prefabs.");
        }

        public static bool HasGeneratedAssets()
        {
            var paths = new[]
            {
                BijliPrefabPath, PehelPrefabPath, MayaPrefabPath,
                UmbrellaPrefabPath, DholPrefabPath, TiffinPrefabPath
            };
            for (var i = 0; i < paths.Length; i++)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(paths[i]);
                if (prefab == null) return false;

                var filters = prefab.GetComponentsInChildren<MeshFilter>(true);
                var hasMesh = false;
                for (var j = 0; j < filters.Length; j++)
                {
                    if (filters[j] != null && filters[j].sharedMesh != null)
                    {
                        hasMesh = true;
                        break;
                    }
                }

                if (!hasMesh) return false;
            }

            return true;
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/BattleRaja/Content/Art");
            EnsureFolder(ArtRoot);
            EnsureFolder(MeshRoot);
            EnsureFolder(MaterialRoot);
            EnsureFolder("Assets/BattleRaja/Content/Prefabs");
            EnsureFolder(PrefabRoot);
        }

        private static void EnsureFolder(string path)
        {
            var parts = path.Split('/');
            var current = parts[0];
            for (var i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }

        private static Dictionary<string, Material> BuildMaterials()
        {
            return new Dictionary<string, Material>(StringComparer.Ordinal)
            {
                ["BijliCyan"] = CreateMaterial("BijliCyan", new Color(0.08f, 0.78f, 0.92f, 1f), 0.1f, 0.55f),
                ["BijliGold"] = CreateMaterial("BijliGold", new Color(1f, 0.72f, 0.16f, 1f), 0.2f, 0.62f),
                ["PehelClay"] = CreateMaterial("PehelClay", new Color(0.76f, 0.27f, 0.18f, 1f), 0.05f, 0.48f),
                ["PehelCream"] = CreateMaterial("PehelCream", new Color(0.97f, 0.76f, 0.45f, 1f), 0.05f, 0.5f),
                ["MayaViolet"] = CreateMaterial("MayaViolet", new Color(0.40f, 0.18f, 0.72f, 1f), 0.1f, 0.52f),
                ["MayaMint"] = CreateMaterial("MayaMint", new Color(0.20f, 0.86f, 0.68f, 1f), 0.05f, 0.55f),
                ["MayaRose"] = CreateMaterial("MayaRose", new Color(0.96f, 0.30f, 0.48f, 1f), 0.05f, 0.52f),
                ["Ink"] = CreateMaterial("Ink", new Color(0.025f, 0.04f, 0.07f, 1f), 0.05f, 0.38f),
                ["Crystal"] = CreateMaterial("Crystal", new Color(0.44f, 0.96f, 1f, 1f), 0.15f, 0.72f),
                ["GadgetInk"] = CreateMaterial("GadgetInk", new Color(0.03f, 0.08f, 0.12f, 1f), 0.1f, 0.44f),
                ["GadgetDhol"] = CreateMaterial("GadgetDhol", new Color(0.90f, 0.16f, 0.13f, 1f), 0.05f, 0.48f),
                ["GadgetTiffin"] = CreateMaterial("GadgetTiffin", new Color(0.96f, 0.58f, 0.12f, 1f), 0.25f, 0.66f),
                ["GadgetUmbrella"] = CreateMaterial("GadgetUmbrella", new Color(0.63f, 0.24f, 0.94f, 1f), 0.08f, 0.56f),
                ["GadgetHighlight"] = CreateMaterial("GadgetHighlight", new Color(0.17f, 0.86f, 0.78f, 1f), 0.1f, 0.62f)
            };
        }

        private static Material CreateMaterial(string name, Color color, float metallic, float smoothness)
        {
            var path = MaterialRoot + "/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
                material = new Material(shader) { name = name };
                AssetDatabase.CreateAsset(material, path);
            }
            material.color = color;
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", metallic);
            if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", smoothness);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Dictionary<string, Mesh> BuildMeshes()
        {
            var meshes = new Dictionary<string, Mesh>(StringComparer.Ordinal)
            {
                ["BodySlim"] = CreateBox("BodySlim", new Vector3(0.72f, 1.15f, 0.5f)),
                ["BodyBroad"] = CreateBox("BodyBroad", new Vector3(0.98f, 1.05f, 0.62f)),
                ["Cloak"] = CreateWedge("Cloak", new Vector3(1.12f, 1.14f, 0.36f), 0.28f),
                ["Head"] = CreateLathe("Head", new List<Vector2> { new Vector2(0.34f, -0.25f), new Vector2(0.39f, -0.12f), new Vector2(0.36f, 0.18f), new Vector2(0.25f, 0.32f) }, 12),
                ["Shoulder"] = CreateBox("Shoulder", new Vector3(0.36f, 0.24f, 0.44f)),
                ["Gem"] = CreateDiamond("Gem", new Vector3(0.24f, 0.38f, 0.18f)),
                ["Fin"] = CreateWedge("Fin", new Vector3(0.28f, 0.58f, 0.12f), 0.5f),
                ["Flat"] = CreateBox("Flat", new Vector3(0.52f, 0.08f, 0.16f)),
                ["Ring"] = CreateRing("Ring", 0.42f, 0.30f, 0.08f, 16),
                ["GadgetBody"] = CreateLathe("GadgetBody", new List<Vector2> { new Vector2(0.30f, -0.20f), new Vector2(0.36f, -0.12f), new Vector2(0.34f, 0.16f), new Vector2(0.26f, 0.22f) }, 16),
                ["GadgetRim"] = CreateRing("GadgetRim", 0.40f, 0.30f, 0.08f, 16),
                ["GadgetLid"] = CreateLathe("GadgetLid", new List<Vector2> { new Vector2(0.24f, -0.06f), new Vector2(0.34f, -0.02f), new Vector2(0.28f, 0.08f) }, 16),
                ["GadgetPole"] = CreateLathe("GadgetPole", new List<Vector2> { new Vector2(0.045f, -0.40f), new Vector2(0.065f, -0.36f), new Vector2(0.055f, 0.40f) }, 10),
                ["GadgetCanopy"] = CreateLathe("GadgetCanopy", new List<Vector2> { new Vector2(0.05f, -0.06f), new Vector2(0.48f, -0.01f), new Vector2(0.36f, 0.10f), new Vector2(0.06f, 0.16f) }, 16),
                ["PickupBeacon"] = CreateBox("PickupBeacon", new Vector3(0.055f, 0.72f, 0.055f)),
                ["PickupBeaconTop"] = CreateDiamond("PickupBeaconTop", new Vector3(0.24f, 0.11f, 0.24f))
            };
            var meshNames = new List<string>(meshes.Keys);
            for (var i = 0; i < meshNames.Count; i++)
            {
                var name = meshNames[i];
                meshes[name] = SaveMesh(name, meshes[name]);
            }
            return meshes;
        }

        private static Mesh SaveMesh(string name, Mesh mesh)
        {
            var path = MeshRoot + "/" + name + ".asset";
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing != null)
            {
                // Keep the imported asset GUID stable across controlled rebuilds. The
                // previous delete-and-recreate flow forced every prefab/scene reference
                // to churn whenever an Android build regenerated production art.
                EditorUtility.CopySerialized(mesh, existing);
                existing.name = name;
                EditorUtility.SetDirty(existing);
                UnityEngine.Object.DestroyImmediate(mesh);
                return existing;
            }

            mesh.name = name;
            AssetDatabase.CreateAsset(mesh, path);
            return AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        private static Mesh CreateBox(string name, Vector3 size)
        {
            var h = size * 0.5f;
            var v = new[]
            {
                new Vector3(-h.x, -h.y, -h.z), new Vector3(h.x, -h.y, -h.z), new Vector3(h.x, -h.y, h.z), new Vector3(-h.x, -h.y, h.z),
                new Vector3(-h.x, h.y, -h.z), new Vector3(h.x, h.y, -h.z), new Vector3(h.x, h.y, h.z), new Vector3(-h.x, h.y, h.z)
            };
            var t = new[] { 0, 2, 1, 0, 3, 2, 4, 5, 6, 4, 6, 7, 0, 1, 5, 0, 5, 4, 1, 2, 6, 1, 6, 5, 2, 3, 7, 2, 7, 6, 3, 0, 4, 3, 4, 7 };
            return CreateMesh(name, v, t);
        }

        private static Mesh CreateDiamond(string name, Vector3 size)
        {
            var h = size * 0.5f;
            var v = new[]
            {
                new Vector3(0f, h.y, 0f), new Vector3(h.x, 0f, 0f), new Vector3(0f, 0f, h.z), new Vector3(-h.x, 0f, 0f),
                new Vector3(0f, 0f, -h.z), new Vector3(0f, -h.y, 0f)
            };
            var t = new[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1, 5, 2, 1, 5, 3, 2, 5, 4, 3, 5, 1, 4 };
            return CreateMesh(name, v, t);
        }

        private static Mesh CreateWedge(string name, Vector3 size, float taper)
        {
            var h = size * 0.5f;
            var back = h.z;
            var front = h.z * taper;
            var v = new[]
            {
                new Vector3(-h.x, -h.y, -back), new Vector3(h.x, -h.y, -back), new Vector3(h.x, -h.y, front), new Vector3(-h.x, -h.y, front),
                new Vector3(-h.x * taper, h.y, -front), new Vector3(h.x * taper, h.y, -front), new Vector3(h.x * taper, h.y, front), new Vector3(-h.x * taper, h.y, front)
            };
            var t = new[] { 0, 2, 1, 0, 3, 2, 4, 5, 6, 4, 6, 7, 0, 1, 5, 0, 5, 4, 1, 2, 6, 1, 6, 5, 2, 3, 7, 2, 7, 6, 3, 0, 4, 3, 4, 7 };
            return CreateMesh(name, v, t);
        }

        private static Mesh CreateLathe(string name, IReadOnlyList<Vector2> profile, int sides)
        {
            var vertices = new Vector3[profile.Count * sides + 2];
            var triangles = new int[(profile.Count - 1) * sides * 6 + sides * 6];
            for (var ring = 0; ring < profile.Count; ring++)
            {
                for (var side = 0; side < sides; side++)
                {
                    var angle = side * Mathf.PI * 2f / sides;
                    vertices[ring * sides + side] = new Vector3(Mathf.Cos(angle) * profile[ring].x, profile[ring].y, Mathf.Sin(angle) * profile[ring].x);
                }
            }
            var bottomCenter = profile.Count * sides;
            var topCenter = bottomCenter + 1;
            vertices[bottomCenter] = new Vector3(0f, profile[0].y, 0f);
            vertices[topCenter] = new Vector3(0f, profile[profile.Count - 1].y, 0f);
            var index = 0;
            for (var ring = 0; ring < profile.Count - 1; ring++)
            {
                for (var side = 0; side < sides; side++)
                {
                    var next = (side + 1) % sides;
                    var a = ring * sides + side;
                    var b = ring * sides + next;
                    var c = (ring + 1) * sides + next;
                    var d = (ring + 1) * sides + side;
                    triangles[index++] = a; triangles[index++] = c; triangles[index++] = b;
                    triangles[index++] = a; triangles[index++] = d; triangles[index++] = c;
                }
            }
            for (var side = 0; side < sides; side++)
            {
                var next = (side + 1) % sides;
                triangles[index++] = bottomCenter; triangles[index++] = next; triangles[index++] = side;
                var top = (profile.Count - 1) * sides;
                triangles[index++] = topCenter; triangles[index++] = top + side; triangles[index++] = top + next;
            }
            return CreateMesh(name, vertices, triangles);
        }

        private static Mesh CreateRing(string name, float outerRadius, float innerRadius, float height, int sides)
        {
            var vertices = new Vector3[sides * 8];
            var triangles = new int[sides * 24];
            for (var i = 0; i < sides; i++)
            {
                var angle = i * Mathf.PI * 2f / sides;
                var nextAngle = (i + 1) * Mathf.PI * 2f / sides;
                var b = i * 8;
                var bottom = -height * 0.5f;
                var top = height * 0.5f;
                vertices[b + 0] = new Vector3(Mathf.Cos(angle) * outerRadius, bottom, Mathf.Sin(angle) * outerRadius);
                vertices[b + 1] = new Vector3(Mathf.Cos(nextAngle) * outerRadius, bottom, Mathf.Sin(nextAngle) * outerRadius);
                vertices[b + 2] = new Vector3(Mathf.Cos(angle) * innerRadius, bottom, Mathf.Sin(angle) * innerRadius);
                vertices[b + 3] = new Vector3(Mathf.Cos(nextAngle) * innerRadius, bottom, Mathf.Sin(nextAngle) * innerRadius);
                vertices[b + 4] = new Vector3(Mathf.Cos(angle) * outerRadius, top, Mathf.Sin(angle) * outerRadius);
                vertices[b + 5] = new Vector3(Mathf.Cos(nextAngle) * outerRadius, top, Mathf.Sin(nextAngle) * outerRadius);
                vertices[b + 6] = new Vector3(Mathf.Cos(angle) * innerRadius, top, Mathf.Sin(angle) * innerRadius);
                vertices[b + 7] = new Vector3(Mathf.Cos(nextAngle) * innerRadius, top, Mathf.Sin(nextAngle) * innerRadius);
                var t = i * 24;
                triangles[t + 0] = b; triangles[t + 1] = b + 3; triangles[t + 2] = b + 1;
                triangles[t + 3] = b; triangles[t + 4] = b + 2; triangles[t + 5] = b + 3;
                triangles[t + 6] = b + 4; triangles[t + 7] = b + 5; triangles[t + 8] = b + 7;
                triangles[t + 9] = b + 4; triangles[t + 10] = b + 7; triangles[t + 11] = b + 6;
                triangles[t + 12] = b; triangles[t + 13] = b + 1; triangles[t + 14] = b + 5;
                triangles[t + 15] = b; triangles[t + 16] = b + 5; triangles[t + 17] = b + 4;
                triangles[t + 18] = b + 2; triangles[t + 19] = b + 6; triangles[t + 20] = b + 7;
                triangles[t + 21] = b + 2; triangles[t + 22] = b + 7; triangles[t + 23] = b + 3;
            }
            return CreateMesh(name, vertices, triangles);
        }

        private static Mesh CreateMesh(string name, Vector3[] vertices, int[] triangles)
        {
            var mesh = new Mesh { name = name };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void BuildBijliPrefab(Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var root = CreateRoot("BijliProductionModel");
            AddPart(root, "Body", meshes["BodySlim"], materials["BijliCyan"], new Vector3(0f, 0.72f, 0f), Vector3.one, Quaternion.identity);
            AddPart(root, "Head", meshes["Head"], materials["BijliGold"], new Vector3(0f, 1.55f, 0f), Vector3.one * 0.92f, Quaternion.identity);
            AddPart(root, "Visor", meshes["Flat"], materials["Ink"], new Vector3(0f, 1.56f, 0.31f), new Vector3(1f, 0.8f, 0.4f), Quaternion.identity);
            AddPart(root, "CrestLeft", meshes["Fin"], materials["BijliGold"], new Vector3(-0.17f, 1.94f, 0f), Vector3.one * 0.6f, Quaternion.Euler(0f, 0f, -22f));
            AddPart(root, "CrestRight", meshes["Fin"], materials["BijliGold"], new Vector3(0.17f, 1.94f, 0f), Vector3.one * 0.6f, Quaternion.Euler(0f, 0f, 22f));
            AddPart(root, "ShoulderLeft", meshes["Shoulder"], materials["BijliCyan"], new Vector3(-0.51f, 1.05f, 0f), Vector3.one * 0.8f, Quaternion.identity);
            AddPart(root, "ShoulderRight", meshes["Shoulder"], materials["BijliCyan"], new Vector3(0.51f, 1.05f, 0f), Vector3.one * 0.8f, Quaternion.identity);
            AddPart(root, "BootLeft", meshes["Flat"], materials["Ink"], new Vector3(-0.22f, 0.12f, 0.06f), new Vector3(1.2f, 1.2f, 1.8f), Quaternion.identity);
            AddPart(root, "BootRight", meshes["Flat"], materials["Ink"], new Vector3(0.22f, 0.12f, 0.06f), new Vector3(1.2f, 1.2f, 1.8f), Quaternion.identity);
            AddPart(root, "BoltTab", meshes["Gem"], materials["Crystal"], new Vector3(0f, 0.86f, 0.33f), Vector3.one * 0.72f, Quaternion.identity);
            SavePrefab(root, BijliPrefabPath);
        }

        private static void BuildPehelPrefab(Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var root = CreateRoot("PehelProductionModel");
            AddPart(root, "Body", meshes["BodyBroad"], materials["PehelClay"], new Vector3(0f, 0.70f, 0f), Vector3.one, Quaternion.identity);
            AddPart(root, "Head", meshes["Head"], materials["PehelCream"], new Vector3(0f, 1.48f, 0f), Vector3.one * 1.03f, Quaternion.identity);
            AddPart(root, "Brow", meshes["Flat"], materials["Ink"], new Vector3(0f, 1.60f, 0.30f), new Vector3(1.4f, 0.5f, 0.5f), Quaternion.identity);
            AddPart(root, "Visor", meshes["Flat"], materials["PehelClay"], new Vector3(0f, 1.48f, 0.34f), new Vector3(1.1f, 0.4f, 0.35f), Quaternion.identity);
            AddPart(root, "ShoulderLeft", meshes["Shoulder"], materials["PehelCream"], new Vector3(-0.58f, 1.03f, 0f), Vector3.one * 1.05f, Quaternion.identity);
            AddPart(root, "ShoulderRight", meshes["Shoulder"], materials["PehelCream"], new Vector3(0.58f, 1.03f, 0f), Vector3.one * 1.05f, Quaternion.identity);
            AddPart(root, "Belt", meshes["Ring"], materials["PehelCream"], new Vector3(0f, 0.74f, 0f), new Vector3(1.15f, 1f, 0.88f), Quaternion.identity);
            AddPart(root, "Buckle", meshes["Gem"], materials["PehelCream"], new Vector3(0f, 0.72f, 0.36f), Vector3.one * 0.7f, Quaternion.identity);
            AddPart(root, "GauntletLeft", meshes["Shoulder"], materials["PehelClay"], new Vector3(-0.67f, 0.58f, 0f), Vector3.one * 0.8f, Quaternion.identity);
            AddPart(root, "GauntletRight", meshes["Shoulder"], materials["PehelClay"], new Vector3(0.67f, 0.58f, 0f), Vector3.one * 0.8f, Quaternion.identity);
            AddPart(root, "Sash", meshes["Flat"], materials["PehelCream"], new Vector3(0f, 1.00f, -0.34f), new Vector3(1.2f, 0.5f, 0.7f), Quaternion.Euler(0f, 0f, -18f));
            AddPart(root, "BootLeft", meshes["Flat"], materials["Ink"], new Vector3(-0.26f, 0.11f, 0.04f), new Vector3(1.5f, 1.4f, 2f), Quaternion.identity);
            AddPart(root, "BootRight", meshes["Flat"], materials["Ink"], new Vector3(0.26f, 0.11f, 0.04f), new Vector3(1.5f, 1.4f, 2f), Quaternion.identity);
            SavePrefab(root, PehelPrefabPath);
        }

        private static void BuildMayaPrefab(Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var root = CreateRoot("MayaProductionModel");
            AddPart(root, "Cloak", meshes["Cloak"], materials["MayaViolet"], new Vector3(0f, 0.72f, 0f), Vector3.one, Quaternion.identity);
            AddPart(root, "Head", meshes["Head"], materials["MayaMint"], new Vector3(0f, 1.52f, 0f), Vector3.one * 0.95f, Quaternion.identity);
            AddPart(root, "Hood", meshes["Cloak"], materials["MayaViolet"], new Vector3(0f, 1.54f, -0.03f), new Vector3(0.65f, 0.62f, 0.62f), Quaternion.identity);
            AddPart(root, "Mask", meshes["Flat"], materials["MayaRose"], new Vector3(0f, 1.48f, 0.30f), new Vector3(0.9f, 0.52f, 0.38f), Quaternion.identity);
            AddPart(root, "ScarfLeft", meshes["Fin"], materials["MayaRose"], new Vector3(-0.28f, 1.05f, 0.10f), new Vector3(0.75f, 1.1f, 0.65f), Quaternion.Euler(0f, 0f, 16f));
            AddPart(root, "ScarfRight", meshes["Fin"], materials["MayaRose"], new Vector3(0.28f, 1.05f, 0.10f), new Vector3(0.75f, 1.1f, 0.65f), Quaternion.Euler(0f, 0f, -16f));
            AddPart(root, "CloakTrim", meshes["Flat"], materials["MayaMint"], new Vector3(0f, 0.52f, 0.18f), new Vector3(1.3f, 0.5f, 0.75f), Quaternion.identity);
            AddPart(root, "CharmLeft", meshes["Gem"], materials["MayaMint"], new Vector3(-0.48f, 0.75f, 0.2f), Vector3.one * 0.55f, Quaternion.identity);
            AddPart(root, "CharmRight", meshes["Gem"], materials["MayaMint"], new Vector3(0.48f, 0.75f, 0.2f), Vector3.one * 0.55f, Quaternion.identity);
            AddPart(root, "BootLeft", meshes["Flat"], materials["Ink"], new Vector3(-0.2f, 0.1f, 0.02f), new Vector3(1.2f, 1.2f, 1.8f), Quaternion.identity);
            AddPart(root, "BootRight", meshes["Flat"], materials["Ink"], new Vector3(0.2f, 0.1f, 0.02f), new Vector3(1.2f, 1.2f, 1.8f), Quaternion.identity);
            SavePrefab(root, MayaPrefabPath);
        }

        private static void BuildUmbrellaPrefab(Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var root = CreateRoot("UmbrellaProductionModel");
            AddPart(root, "Pedestal", meshes["GadgetRim"], materials["GadgetInk"], new Vector3(0f, -0.18f, 0f), Vector3.one * 1.55f, Quaternion.identity);
            AddPart(root, "Pole", meshes["GadgetPole"], materials["GadgetHighlight"], new Vector3(0f, 0.28f, 0f), Vector3.one, Quaternion.identity);
            AddPart(root, "Canopy", meshes["GadgetCanopy"], materials["GadgetUmbrella"], new Vector3(0f, 0.64f, 0f), Vector3.one, Quaternion.identity);
            AddPart(root, "Stripe", meshes["Flat"], materials["GadgetHighlight"], new Vector3(0f, 0.75f, 0f), new Vector3(0.7f, 0.42f, 0.5f), Quaternion.Euler(0f, 45f, 0f));
            AddPart(root, "PickupBeacon", meshes["PickupBeacon"], materials["GadgetHighlight"], new Vector3(0f, 1.08f, 0f), Vector3.one, Quaternion.identity);
            AddPart(root, "PickupBeaconTop", meshes["PickupBeaconTop"], materials["GadgetUmbrella"], new Vector3(0f, 1.48f, 0f), Vector3.one, Quaternion.identity);
            SavePrefab(root, UmbrellaPrefabPath);
        }

        private static void BuildDholPrefab(Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var root = CreateRoot("DholProductionModel");
            AddPart(root, "Pedestal", meshes["GadgetRim"], materials["GadgetInk"], new Vector3(0f, -0.18f, 0f), Vector3.one * 1.55f, Quaternion.identity);
            AddPart(root, "DrumBody", meshes["GadgetBody"], materials["GadgetDhol"], new Vector3(0f, 0.28f, 0f), new Vector3(1f, 0.9f, 0.75f), Quaternion.Euler(90f, 0f, 0f));
            AddPart(root, "RimLeft", meshes["GadgetRim"], materials["GadgetHighlight"], new Vector3(-0.28f, 0.28f, 0f), new Vector3(0.48f, 0.7f, 0.48f), Quaternion.Euler(90f, 0f, 0f));
            AddPart(root, "RimRight", meshes["GadgetRim"], materials["GadgetHighlight"], new Vector3(0.28f, 0.28f, 0f), new Vector3(0.48f, 0.7f, 0.48f), Quaternion.Euler(90f, 0f, 0f));
            AddPart(root, "Strap", meshes["Flat"], materials["GadgetHighlight"], new Vector3(0f, 0.28f, -0.22f), new Vector3(0.32f, 0.72f, 0.35f), Quaternion.Euler(0f, 0f, 24f));
            AddPart(root, "PickupBeacon", meshes["PickupBeacon"], materials["GadgetHighlight"], new Vector3(0f, 1.08f, 0f), Vector3.one, Quaternion.identity);
            AddPart(root, "PickupBeaconTop", meshes["PickupBeaconTop"], materials["GadgetDhol"], new Vector3(0f, 1.48f, 0f), Vector3.one, Quaternion.identity);
            SavePrefab(root, DholPrefabPath);
        }

        private static void BuildTiffinPrefab(Dictionary<string, Material> materials, Dictionary<string, Mesh> meshes)
        {
            var root = CreateRoot("TiffinProductionModel");
            AddPart(root, "Pedestal", meshes["GadgetRim"], materials["GadgetInk"], new Vector3(0f, -0.18f, 0f), Vector3.one * 1.55f, Quaternion.identity);
            AddPart(root, "LowerTier", meshes["GadgetBody"], materials["GadgetTiffin"], new Vector3(0f, 0.16f, 0f), new Vector3(1.15f, 0.72f, 1.15f), Quaternion.identity);
            AddPart(root, "UpperTier", meshes["GadgetLid"], materials["GadgetHighlight"], new Vector3(0f, 0.42f, 0f), new Vector3(1.1f, 0.8f, 1.1f), Quaternion.identity);
            AddPart(root, "Handle", meshes["GadgetPole"], materials["GadgetTiffin"], new Vector3(0f, 0.72f, 0f), new Vector3(1.5f, 0.5f, 1.5f), Quaternion.identity);
            AddPart(root, "Stripe", meshes["Flat"], materials["GadgetHighlight"], new Vector3(0f, 0.25f, -0.36f), new Vector3(0.85f, 0.34f, 0.45f), Quaternion.identity);
            AddPart(root, "PickupBeacon", meshes["PickupBeacon"], materials["GadgetHighlight"], new Vector3(0f, 1.08f, 0f), Vector3.one, Quaternion.identity);
            AddPart(root, "PickupBeaconTop", meshes["PickupBeaconTop"], materials["GadgetTiffin"], new Vector3(0f, 1.48f, 0f), Vector3.one, Quaternion.identity);
            SavePrefab(root, TiffinPrefabPath);
        }

        private static GameObject CreateRoot(string name)
        {
            var root = new GameObject(name);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            return root;
        }

        private static GameObject AddPart(GameObject root, string name, Mesh mesh, Material material, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            var part = new GameObject(name);
            part.transform.SetParent(root.transform, false);
            part.transform.localPosition = position;
            part.transform.localRotation = rotation;
            part.transform.localScale = scale;
            part.AddComponent<MeshFilter>().sharedMesh = mesh;
            part.AddComponent<MeshRenderer>().sharedMaterial = material;
            return part;
        }

        private static void SavePrefab(GameObject root, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(root, path);
            UnityEngine.Object.DestroyImmediate(root);
        }
    }
}
