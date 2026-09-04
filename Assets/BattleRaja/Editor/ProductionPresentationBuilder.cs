using System;
using System.Collections.Generic;
using BattleRaja.Presentation.Visuals;
using BattleRaja.Presentation.Gadgets;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleRaja.Editor
{
    /// <summary>
    /// Builds the saved presentation layer that sits on top of the authoritative V1
    /// actors. The rig is a lightweight transform rig with a small, deterministic
    /// two-bone primary body/cloak skin; accessories remain render-only parts, and
    /// every effect is a pooled-safe particle cue with no gameplay callbacks.
    /// </summary>
    public static class ProductionPresentationBuilder
    {
        private const string ArtRoot = "Assets/BattleRaja/Content/Art/V1";
        private const string AnimationRoot = ArtRoot + "/Animation";
        private const string ClipRoot = AnimationRoot + "/Clips";
        private const string VfxRoot = ArtRoot + "/VFX";
        private const string MeshRoot = ArtRoot + "/Meshes";
        private const string MaterialRoot = ArtRoot + "/Materials";
        private const string PrefabRoot = "Assets/BattleRaja/Content/Prefabs/Production";
        private const string ControllerPath = AnimationRoot + "/FighterProduction.controller";

        private static readonly string[] StateNames =
        {
            "Idle", "Locomotion", "Aim", "Attack", "Ability", "Hit", "Knockback", "Eliminated", "Victory", "Defeat"
        };

        private static readonly string[] VfxNames =
        {
            "BijliAttackVfx", "BijliAbilityVfx", "PehelAttackVfx", "PehelAbilityVfx",
            "MayaAttackVfx", "MayaAbilityVfx", "FighterHitVfx", "FighterEliminationVfx",
            "GadgetUseVfx", "HealingVfx", "ShieldVfx", "ZoneWarningVfx", "ZoneClosingVfx",
            "ZoneFinalCircleVfx", "VictoryVfx", "DefeatVfx"
        };

        private static readonly string[] FighterPrefabPaths =
        {
            ProductionArtBuilder.BijliPrefabPath,
            ProductionArtBuilder.PehelPrefabPath,
            ProductionArtBuilder.MayaPrefabPath
        };

        [MenuItem("BattleRaja/Build V1 Production Animation and VFX")]
        public static void BuildAll()
        {
            // Generated presentation assets are committed release inputs. Do not delete
            // and recreate them during every scene/build pass: that churns GUIDs and
            // causes otherwise clean scenes to drift. A deliberate asset deletion (or a
            // fresh checkout) still enters the full generation path below.
            if (HasGeneratedAssets())
            {
                Debug.Log("BattleRaja production presentation already exists; keeping committed asset identities.");
                return;
            }

            EnsureFolders();
            var controller = BuildController();
            var vfx = BuildVfxLibrary();
            for (var i = 0; i < FighterPrefabPaths.Length; i++)
            {
                BuildFighterPrefab(FighterPrefabPaths[i], controller, vfx, i);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("BattleRaja production presentation generated: transform rig, Animator clips/controller and saved VFX cues.");
        }

        /// <summary>
        /// Rebuilds the prefab rig/cue composition while preserving the existing
        /// controller and VFX asset identities. This is used only by the deliberate
        /// production-art rebuild path after a mesh recipe changes.
        /// </summary>
        public static void RebuildFighterPrefabs()
        {
            EnsureFolders();
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
            if (controller == null) controller = BuildController();

            var vfx = LoadVfxLibrary();
            if (vfx == null) vfx = BuildVfxLibrary();
            for (var i = 0; i < FighterPrefabPaths.Length; i++)
            {
                BuildFighterPrefab(FighterPrefabPaths[i], controller, vfx, i);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RefreshSceneReferences();
            Debug.Log("BattleRaja production fighter prefabs rebuilt with the existing Animator/VFX assets.");
        }

        public static bool HasGeneratedAssets()
        {
            if (AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath) == null) return false;
            for (var i = 0; i < StateNames.Length; i++)
            {
                var clipPath = ClipRoot + "/Fighter" + StateNames[i] + ".anim";
                if (AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath) == null) return false;
            }

            for (var i = 0; i < VfxNames.Length; i++)
            {
                var vfxPath = VfxRoot + "/" + VfxNames[i] + ".prefab";
                if (AssetDatabase.LoadAssetAtPath<GameObject>(vfxPath) == null) return false;
            }

            for (var i = 0; i < FighterPrefabPaths.Length; i++)
            {
                var fighter = AssetDatabase.LoadAssetAtPath<GameObject>(FighterPrefabPaths[i]);
                if (fighter == null
                    || fighter.transform.Find("ProductionRig") == null
                    || fighter.GetComponent<Animator>() == null
                    || fighter.GetComponent<ProductionVfxCue>() == null
                    || fighter.GetComponent<LODGroup>() == null)
                {
                    return false;
                }

                var skins = fighter.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                if (skins.Length != 1
                    || skins[0].sharedMesh == null
                    || skins[0].bones == null
                    || skins[0].bones.Length != 2
                    || skins[0].sharedMesh.bindposes == null
                    || skins[0].sharedMesh.bindposes.Length != skins[0].bones.Length
                    || skins[0].sharedMesh.boneWeights == null
                    || skins[0].sharedMesh.boneWeights.Length != skins[0].sharedMesh.vertexCount
                    || skins[0].sharedMesh.uv == null
                    || skins[0].sharedMesh.uv.Length != skins[0].sharedMesh.vertexCount)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Refreshes serialized scene references after a generated prefab is replaced.
        /// Unity prefab roots receive new local file IDs when regenerated, so this
        /// controlled editor pass prevents scenes from silently falling back to
        /// development primitives.
        /// </summary>
        public static void RefreshSceneReferences()
        {
            var scenes = new[]
            {
                "Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity",
                "Assets/BattleRaja/Scenes/Tutorial/TutorialArena.unity",
                "Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity"
            };
            var bijli = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.BijliPrefabPath);
            var pehel = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.PehelPrefabPath);
            var maya = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.MayaPrefabPath);
            var umbrella = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.UmbrellaPrefabPath);
            var dhol = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.DholPrefabPath);
            var tiffin = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.TiffinPrefabPath);

            for (var i = 0; i < scenes.Length; i++)
            {
                var scene = EditorSceneManager.OpenScene(scenes[i], OpenSceneMode.Single);
                var presentations = UnityEngine.Object.FindObjectsByType<FighterPresentation>(FindObjectsInactive.Include);
                for (var j = 0; j < presentations.Length; j++)
                {
                    var serialized = new SerializedObject(presentations[j]);
                    SetReference(serialized, "bijliModelPrefab", bijli);
                    SetReference(serialized, "pehelModelPrefab", pehel);
                    SetReference(serialized, "mayaModelPrefab", maya);
                    serialized.ApplyModifiedPropertiesWithoutUndo();
                    EditorUtility.SetDirty(presentations[j]);
                }

                var gadgetVisuals = UnityEngine.Object.FindObjectsByType<GadgetPickupVisuals>(FindObjectsInactive.Include);
                for (var j = 0; j < gadgetVisuals.Length; j++)
                {
                    var serialized = new SerializedObject(gadgetVisuals[j]);
                    SetReference(serialized, "umbrellaModelPrefab", umbrella);
                    SetReference(serialized, "dholModelPrefab", dhol);
                    SetReference(serialized, "tiffinModelPrefab", tiffin);
                    serialized.ApplyModifiedPropertiesWithoutUndo();
                    EditorUtility.SetDirty(gadgetVisuals[j]);
                }

                EditorSceneManager.SaveScene(scene);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("BattleRaja production scene prefab references refreshed in " + scenes.Length + " scenes.");
        }

        private static void SetReference(SerializedObject serialized, string propertyName, UnityEngine.Object value)
        {
            var property = serialized.FindProperty(propertyName);
            if (property != null) property.objectReferenceValue = value;
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/BattleRaja/Content/Art");
            EnsureFolder(ArtRoot);
            EnsureFolder(AnimationRoot);
            EnsureFolder(ClipRoot);
            EnsureFolder(VfxRoot);
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

        private static AnimatorController BuildController()
        {
            if (AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath) != null)
            {
                AssetDatabase.DeleteAsset(ControllerPath);
            }

            for (var i = 0; i < StateNames.Length; i++)
            {
                var clipPath = ClipRoot + "/Fighter" + StateNames[i] + ".anim";
                if (AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath) != null) AssetDatabase.DeleteAsset(clipPath);
            }

            var controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            controller.AddParameter(new AnimatorControllerParameter
            {
                name = "State",
                type = AnimatorControllerParameterType.Int
            });

            var stateMachine = controller.layers[0].stateMachine;
            for (var i = 0; i < StateNames.Length; i++)
            {
                var state = stateMachine.AddState(StateNames[i]);
                state.motion = CreateAnimationClip(StateNames[i]);
                if (i == 0) stateMachine.defaultState = state;

                var transition = stateMachine.AddAnyStateTransition(state);
                transition.hasExitTime = false;
                transition.duration = 0.04f;
                transition.AddCondition(AnimatorConditionMode.Equals, i, "State");
            }

            EditorUtility.SetDirty(controller);
            return controller;
        }

        private static AnimationClip CreateAnimationClip(string stateName)
        {
            var loops = stateName == "Idle" || stateName == "Locomotion" || stateName == "Aim";
            var clip = new AnimationClip
            {
                name = "Fighter" + stateName,
                frameRate = 30f,
                wrapMode = loops ? WrapMode.Loop : WrapMode.Once
            };

            var duration = stateName == "Idle" || stateName == "Locomotion" ? 0.8f
                : stateName == "Aim" ? 0.55f
                : stateName == "Attack" ? 0.18f
                : stateName == "Ability" ? 0.36f
                : 0.2f;
            var times = new[] { 0f, duration * 0.5f, duration };
            var hipsBase = 0.65f;
            var chestPath = "ProductionRig/Root/Hips/Chest";
            var leftHandPath = "ProductionRig/Root/Hips/Chest/LeftHand";
            var rightHandPath = "ProductionRig/Root/Hips/Chest/RightHand";
            var leftFootPath = "ProductionRig/Root/Hips/LeftFoot";
            var rightFootPath = "ProductionRig/Root/Hips/RightFoot";

            switch (stateName)
            {
                case "Idle":
                    SetCurve(clip, "ProductionRig/Root/Hips", "m_LocalPosition.y", times,
                        new[] { hipsBase - 0.015f, hipsBase + 0.015f, hipsBase - 0.015f });
                    break;
                case "Locomotion":
                    SetCurve(clip, leftFootPath, "m_LocalPosition.y", times, new[] { -0.65f, -0.55f, -0.65f });
                    SetCurve(clip, rightFootPath, "m_LocalPosition.y", times, new[] { -0.65f, -0.75f, -0.65f });
                    SetCurve(clip, "ProductionRig/Root/Hips", "m_LocalPosition.y", times,
                        new[] { hipsBase, hipsBase + 0.035f, hipsBase });
                    break;
                case "Aim":
                    SetCurve(clip, chestPath, "m_LocalRotation.x", times, new[] { 0f, -0.035f, 0f });
                    SetCurve(clip, chestPath, "m_LocalRotation.w", times, new[] { 1f, 0.999f, 1f });
                    SetCurve(clip, leftHandPath, "m_LocalPosition.z", times, new[] { 0f, 0.12f, 0f });
                    SetCurve(clip, rightHandPath, "m_LocalPosition.z", times, new[] { 0f, 0.16f, 0f });
                    break;
                case "Attack":
                    SetCurve(clip, leftHandPath, "m_LocalPosition.z", times, new[] { 0f, 0.20f, 0f });
                    SetCurve(clip, rightHandPath, "m_LocalPosition.z", times, new[] { 0f, 0.26f, 0f });
                    SetCurve(clip, chestPath, "m_LocalScale.x", times, new[] { 1f, 1.08f, 1f });
                    SetCurve(clip, chestPath, "m_LocalScale.y", times, new[] { 1f, 1.08f, 1f });
                    SetCurve(clip, chestPath, "m_LocalScale.z", times, new[] { 1f, 1.08f, 1f });
                    break;
                case "Ability":
                    SetCurve(clip, leftHandPath, "m_LocalPosition.z", times, new[] { 0f, 0.32f, 0f });
                    SetCurve(clip, rightHandPath, "m_LocalPosition.z", times, new[] { 0f, 0.38f, 0f });
                    SetCurve(clip, "ProductionRig/Root/Hips", "m_LocalPosition.y", times,
                        new[] { hipsBase, hipsBase + 0.10f, hipsBase });
                    SetCurve(clip, chestPath, "m_LocalScale.x", times, new[] { 1f, 1.13f, 1f });
                    SetCurve(clip, chestPath, "m_LocalScale.y", times, new[] { 1f, 1.13f, 1f });
                    SetCurve(clip, chestPath, "m_LocalScale.z", times, new[] { 1f, 1.13f, 1f });
                    break;
                case "Hit":
                case "Knockback":
                    SetCurve(clip, chestPath, "m_LocalPosition.x", times, new[] { 0f, stateName == "Hit" ? 0.06f : 0.12f, 0f });
                    SetCurve(clip, chestPath, "m_LocalScale.x", times, new[] { 1f, 0.94f, 1f });
                    SetCurve(clip, chestPath, "m_LocalScale.y", times, new[] { 1f, 0.94f, 1f });
                    SetCurve(clip, chestPath, "m_LocalScale.z", times, new[] { 1f, 0.94f, 1f });
                    break;
                case "Eliminated":
                case "Defeat":
                    SetCurve(clip, "ProductionRig/Root/Hips", "m_LocalPosition.y", times, new[] { hipsBase, 0.40f, 0.40f });
                    SetCurve(clip, chestPath, "m_LocalScale.y", times, new[] { 1f, 0.72f, 0.72f });
                    break;
                case "Victory":
                    SetCurve(clip, "ProductionRig/Root/Hips", "m_LocalPosition.y", times,
                        new[] { hipsBase, hipsBase + 0.12f, hipsBase });
                    SetCurve(clip, leftHandPath, "m_LocalPosition.y", times, new[] { -0.10f, 0.10f, -0.10f });
                    SetCurve(clip, rightHandPath, "m_LocalPosition.y", times, new[] { -0.10f, 0.10f, -0.10f });
                    break;
            }

            var path = ClipRoot + "/Fighter" + stateName + ".anim";
            AssetDatabase.CreateAsset(clip, path);
            return clip;
        }

        private static void SetCurve(AnimationClip clip, string path, string property, IReadOnlyList<float> times, IReadOnlyList<float> values)
        {
            var keys = new Keyframe[times.Count];
            for (var i = 0; i < keys.Length; i++) keys[i] = new Keyframe(times[i], values[i]);
            AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(path, typeof(Transform), property), new AnimationCurve(keys));
        }

        private static Dictionary<string, GameObject> BuildVfxLibrary()
        {
            var result = new Dictionary<string, GameObject>(StringComparer.Ordinal);
            result["BijliAttack"] = BuildVfxPrefab("BijliAttackVfx", new Color(0.12f, 0.85f, 1f), 10, 0.18f, 0.10f, 1.3f);
            result["BijliAbility"] = BuildVfxPrefab("BijliAbilityVfx", new Color(1f, 0.76f, 0.14f), 18, 0.34f, 0.14f, 1.6f);
            result["PehelAttack"] = BuildVfxPrefab("PehelAttackVfx", new Color(1f, 0.48f, 0.18f), 8, 0.18f, 0.14f, 0.75f);
            result["PehelAbility"] = BuildVfxPrefab("PehelAbilityVfx", new Color(0.96f, 0.26f, 0.12f), 20, 0.38f, 0.16f, 1.15f);
            result["MayaAttack"] = BuildVfxPrefab("MayaAttackVfx", new Color(0.30f, 0.96f, 0.76f), 10, 0.18f, 0.10f, 1.5f);
            result["MayaAbility"] = BuildVfxPrefab("MayaAbilityVfx", new Color(0.88f, 0.34f, 1f), 18, 0.38f, 0.13f, 1.3f);
            result["Hit"] = BuildVfxPrefab("FighterHitVfx", new Color(1f, 0.88f, 0.26f), 12, 0.16f, 0.10f, 1.0f);
            result["Elimination"] = BuildVfxPrefab("FighterEliminationVfx", new Color(1f, 0.28f, 0.22f), 22, 0.48f, 0.14f, 1.6f);
            result["Gadget"] = BuildVfxPrefab("GadgetUseVfx", new Color(0.18f, 0.86f, 0.80f), 14, 0.28f, 0.12f, 0.9f);
            result["Healing"] = BuildVfxPrefab("HealingVfx", new Color(0.34f, 1f, 0.42f), 14, 0.32f, 0.12f, 0.8f);
            result["Shield"] = BuildVfxPrefab("ShieldVfx", new Color(0.38f, 0.72f, 1f), 16, 0.36f, 0.12f, 0.7f);
            result["ZoneWarning"] = BuildVfxPrefab("ZoneWarningVfx", new Color(1f, 0.66f, 0.12f), 18, 0.42f, 0.10f, 0.9f);
            result["ZoneClosing"] = BuildVfxPrefab("ZoneClosingVfx", new Color(1f, 0.20f, 0.16f), 24, 0.52f, 0.12f, 1.0f);
            result["ZoneFinal"] = BuildVfxPrefab("ZoneFinalCircleVfx", new Color(0.72f, 0.24f, 1f), 28, 0.60f, 0.13f, 1.1f);
            result["Victory"] = BuildVfxPrefab("VictoryVfx", new Color(1f, 0.84f, 0.18f), 26, 0.72f, 0.15f, 1.35f);
            result["Defeat"] = BuildVfxPrefab("DefeatVfx", new Color(1f, 0.22f, 0.20f), 18, 0.56f, 0.13f, 0.65f);
            return result;
        }

        private static Dictionary<string, GameObject> LoadVfxLibrary()
        {
            var paths = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["BijliAttack"] = VfxRoot + "/BijliAttackVfx.prefab",
                ["BijliAbility"] = VfxRoot + "/BijliAbilityVfx.prefab",
                ["PehelAttack"] = VfxRoot + "/PehelAttackVfx.prefab",
                ["PehelAbility"] = VfxRoot + "/PehelAbilityVfx.prefab",
                ["MayaAttack"] = VfxRoot + "/MayaAttackVfx.prefab",
                ["MayaAbility"] = VfxRoot + "/MayaAbilityVfx.prefab",
                ["Hit"] = VfxRoot + "/FighterHitVfx.prefab",
                ["Elimination"] = VfxRoot + "/FighterEliminationVfx.prefab",
                ["Victory"] = VfxRoot + "/VictoryVfx.prefab",
                ["Defeat"] = VfxRoot + "/DefeatVfx.prefab"
            };
            var result = new Dictionary<string, GameObject>(StringComparer.Ordinal);
            foreach (var pair in paths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(pair.Value);
                if (prefab == null) return null;
                result[pair.Key] = prefab;
            }

            return result;
        }

        private static GameObject BuildVfxPrefab(string name, Color color, short count, float lifetime, float size, float speed)
        {
            var path = VfxRoot + "/" + name + ".prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null) AssetDatabase.DeleteAsset(path);

            var root = new GameObject(name);
            var particle = root.AddComponent<ParticleSystem>();
            var main = particle.main;
            main.playOnAwake = false;
            main.loop = false;
            main.duration = lifetime;
            main.startLifetime = lifetime;
            main.startSpeed = speed;
            main.startSize = size;
            main.startColor = color;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            var emission = particle.emission;
            emission.enabled = true;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, count) });
            var shape = particle.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.16f;
            var renderer = root.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.sharedMaterial = CreateVfxMaterial(name + "Material", color);
            var saved = PrefabUtility.SaveAsPrefabAsset(root, path);
            UnityEngine.Object.DestroyImmediate(root);
            return saved;
        }

        private static Material CreateVfxMaterial(string name, Color color)
        {
            var path = MaterialRoot + "/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit")
                             ?? Shader.Find("Universal Render Pipeline/Lit")
                             ?? Shader.Find("Particles/Standard Unlit")
                             ?? Shader.Find("Standard");
                material = new Material(shader) { name = name };
                AssetDatabase.CreateAsset(material, path);
            }

            material.color = color;
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            if (material.HasProperty("_Color")) material.SetColor("_Color", color);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void BuildFighterPrefab(string path, AnimatorController controller, Dictionary<string, GameObject> vfx, int fighterIndex)
        {
            var root = PrefabUtility.LoadPrefabContents(path);
            if (root == null)
            {
                Debug.LogWarning("Production fighter prefab is missing; skipped presentation build: " + path);
                return;
            }

            // A previous generation pass may have parented the source body/cloak under
            // the old rig. Snapshot the editable mesh/materials and its root-relative
            // transform before removing that rig, otherwise an idempotent rebuild would
            // destroy the only source geometry needed for the saved skinned primary.
            var primaryName = fighterIndex == 2 ? "Cloak" : "Body";
            var sourcePrimary = FindDescendant(root.transform, primaryName);
            var sourceFilter = sourcePrimary != null ? sourcePrimary.GetComponent<MeshFilter>() : null;
            var sourceRenderer = sourcePrimary != null ? sourcePrimary.GetComponent<MeshRenderer>() : null;
            var sourceMesh = sourceFilter != null ? sourceFilter.sharedMesh : null;
            var sourceMaterials = sourceRenderer != null ? sourceRenderer.sharedMaterials : null;
            var sourceToRoot = sourcePrimary != null
                ? root.transform.worldToLocalMatrix * sourcePrimary.localToWorldMatrix
                : Matrix4x4.identity;
            if (sourceMesh == null)
            {
                var sourceMeshName = fighterIndex == 2 ? "MayaCloak" : fighterIndex == 1 ? "PehelTorso" : "BijliTorso";
                sourceMesh = AssetDatabase.LoadAssetAtPath<Mesh>(MeshRoot + "/" + sourceMeshName + ".asset");
                sourceToRoot = Matrix4x4.identity;
            }
            if (sourceMaterials == null || sourceMaterials.Length == 0 || sourceMaterials[0] == null)
            {
                var sourceMaterialName = fighterIndex == 2 ? "MayaViolet" : fighterIndex == 1 ? "PehelClay" : "BijliCyan";
                var sourceMaterial = AssetDatabase.LoadAssetAtPath<Material>(MaterialRoot + "/" + sourceMaterialName + ".mat");
                sourceMaterials = sourceMaterial != null ? new[] { sourceMaterial } : Array.Empty<Material>();
            }

            RemoveChild(root.transform, "ProductionRig");
            RemoveChild(root.transform, "ProductionSkinnedPrimary");
            RemoveChild(root.transform, "ProductionFarSilhouette");
            var existingLod = root.GetComponent<LODGroup>();
            if (existingLod != null) UnityEngine.Object.DestroyImmediate(existingLod, true);
            RemoveChildrenWithPrefix(root.transform, "Vfx_");
            var existingAnimator = root.GetComponent<Animator>();
            if (existingAnimator != null) UnityEngine.Object.DestroyImmediate(existingAnimator, true);
            var existingCue = root.GetComponent<ProductionVfxCue>();
            if (existingCue != null) UnityEngine.Object.DestroyImmediate(existingCue, true);

            var rig = new GameObject("ProductionRig").transform;
            rig.SetParent(root.transform, false);
            var rigRoot = CreateBone("Root", rig, Vector3.zero);
            var hips = CreateBone("Hips", rigRoot, new Vector3(0f, 0.65f, 0f));
            var chest = CreateBone("Chest", hips, new Vector3(0f, 0.45f, 0f));
            var head = CreateBone("Head", chest, new Vector3(0f, 0.42f, 0f));
            var leftHand = CreateBone("LeftHand", chest, new Vector3(-0.55f, -0.10f, 0f));
            var rightHand = CreateBone("RightHand", chest, new Vector3(0.55f, -0.10f, 0f));
            var leftFoot = CreateBone("LeftFoot", hips, new Vector3(-0.22f, -0.65f, 0f));
            var rightFoot = CreateBone("RightFoot", hips, new Vector3(0.22f, -0.65f, 0f));

            ReparentPart(root.transform.Find("Body"), chest);
            ReparentPart(root.transform.Find("Cloak"), chest);
            ReparentPart(root.transform.Find("CloakTrim"), chest);
            ReparentPart(root.transform.Find("ChestMedallion"), chest);
            ReparentPart(root.transform.Find("EnergyCore"), chest);
            ReparentPart(root.transform.Find("ShardCore"), chest);
            ReparentPart(root.transform.Find("NeckCollar"), chest);
            ReparentPart(root.transform.Find("ChestPlate"), chest);
            ReparentPart(root.transform.Find("Head"), head);
            ReparentPart(root.transform.Find("Hood"), head);
            ReparentPart(root.transform.Find("Visor"), head);
            ReparentPart(root.transform.Find("EyeLeft"), head);
            ReparentPart(root.transform.Find("EyeRight"), head);
            ReparentPart(root.transform.Find("JawGuard"), head);
            ReparentPart(root.transform.Find("Mask"), head);
            ReparentPart(root.transform.Find("Brow"), head);
            ReparentPart(root.transform.Find("CrestLeft"), head);
            ReparentPart(root.transform.Find("CrestRight"), head);
            ReparentPart(root.transform.Find("Crest"), head);
            ReparentPart(root.transform.Find("HoodTip"), head);
            ReparentPart(root.transform.Find("ShoulderLeft"), leftHand);
            ReparentPart(root.transform.Find("ShoulderRight"), rightHand);
            ReparentPart(root.transform.Find("ArmSleeveLeft"), leftHand);
            ReparentPart(root.transform.Find("ArmSleeveRight"), rightHand);
            ReparentPart(root.transform.Find("GauntletLeft"), leftHand);
            ReparentPart(root.transform.Find("GauntletRight"), rightHand);
            ReparentPart(root.transform.Find("ArmGuardLeft"), leftHand);
            ReparentPart(root.transform.Find("ArmGuardRight"), rightHand);
            ReparentPart(root.transform.Find("ArcBlade"), rightHand);
            ReparentPart(root.transform.Find("WeaponHandle"), rightHand);
            ReparentPart(root.transform.Find("MaceHead"), rightHand);
            ReparentPart(root.transform.Find("FanBladeLeft"), leftHand);
            ReparentPart(root.transform.Find("FanBladeRight"), rightHand);
            ReparentPart(root.transform.Find("LegLeft"), leftFoot);
            ReparentPart(root.transform.Find("LegRight"), rightFoot);
            ReparentPart(root.transform.Find("BootLeft"), leftFoot);
            ReparentPart(root.transform.Find("BootRight"), rightFoot);
            ReparentPart(root.transform.Find("KneeGuardLeft"), leftFoot);
            ReparentPart(root.transform.Find("KneeGuardRight"), rightFoot);
            ReparentPart(root.transform.Find("WaistBand"), hips);
            ReparentPart(root.transform.Find("Belt"), hips);
            ReparentPart(root.transform.Find("Buckle"), hips);
            ReparentPart(root.transform.Find("Sash"), chest);
            ReparentPart(root.transform.Find("ScarfLeft"), chest);
            ReparentPart(root.transform.Find("ScarfRight"), chest);

            BuildPrimarySkinnedMesh(root, fighterIndex, hips, chest, sourceMesh, sourceMaterials, sourceToRoot);

            var animator = root.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.applyRootMotion = false;

            var cue = root.AddComponent<ProductionVfxCue>();
            var attackKey = fighterIndex == 0 ? "BijliAttack" : fighterIndex == 1 ? "PehelAttack" : "MayaAttack";
            var abilityKey = fighterIndex == 0 ? "BijliAbility" : fighterIndex == 1 ? "PehelAbility" : "MayaAbility";
            var attack = AddVfxInstance(root.transform, vfx[attackKey], "Vfx_Attack");
            var ability = AddVfxInstance(root.transform, vfx[abilityKey], "Vfx_Ability");
            var hit = AddVfxInstance(root.transform, vfx["Hit"], "Vfx_Hit");
            var elimination = AddVfxInstance(root.transform, vfx["Elimination"], "Vfx_Elimination");
            var victory = AddVfxInstance(root.transform, vfx["Victory"], "Vfx_Victory");
            var defeat = AddVfxInstance(root.transform, vfx["Defeat"], "Vfx_Defeat");
            cue.Configure(attack, ability, hit, elimination, victory, defeat);
            BuildLodGroup(root, fighterIndex);

            PrefabUtility.SaveAsPrefabAsset(root, path);
            PrefabUtility.UnloadPrefabContents(root);
        }

        private static void BuildPrimarySkinnedMesh(GameObject root, int fighterIndex, Transform hips, Transform chest,
            Mesh sourceMesh, Material[] sourceMaterials, Matrix4x4 sourceToRoot)
        {
            var primaryName = fighterIndex == 2 ? "Cloak" : "Body";
            if (sourceMesh == null || sourceMaterials == null || sourceMaterials.Length == 0 || sourceMaterials[0] == null)
            {
                Debug.LogWarning("Production primary mesh is missing; skipped skin build for " + root.name + ".");
                return;
            }

            var skinnedObject = new GameObject("ProductionSkinnedPrimary");
            skinnedObject.transform.SetParent(root.transform, false);
            skinnedObject.transform.localPosition = Vector3.zero;
            skinnedObject.transform.localRotation = Quaternion.identity;
            skinnedObject.transform.localScale = Vector3.one;

            var sourceVertices = sourceMesh.vertices;
            var vertices = new Vector3[sourceVertices.Length];
            for (var i = 0; i < sourceVertices.Length; i++)
            {
                vertices[i] = sourceToRoot.MultiplyPoint3x4(sourceVertices[i]);
            }

            var skinnedMesh = new Mesh
            {
                name = (fighterIndex == 2 ? "MayaSkinCloak" : fighterIndex == 1 ? "PehelSkinBody" : "BijliSkinBody")
            };
            skinnedMesh.vertices = vertices;
            skinnedMesh.subMeshCount = sourceMesh.subMeshCount;
            for (var subMesh = 0; subMesh < sourceMesh.subMeshCount; subMesh++)
            {
                skinnedMesh.SetTriangles(sourceMesh.GetTriangles(subMesh), subMesh, true);
            }

            var sourceUv = sourceMesh.uv;
            skinnedMesh.uv = sourceUv != null && sourceUv.Length == vertices.Length
                ? sourceUv
                : new Vector2[vertices.Length];
            skinnedMesh.bindposes = new[]
            {
                hips.worldToLocalMatrix * skinnedObject.transform.localToWorldMatrix,
                chest.worldToLocalMatrix * skinnedObject.transform.localToWorldMatrix
            };

            var weights = new BoneWeight[vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                // The generated body/cloak meshes are centered around the hips/chest
                // chain. Blend through the waist so the saved primary silhouette
                // follows the Animator without introducing gameplay dependencies.
                var chestWeight = Mathf.Clamp01(Mathf.InverseLerp(0.48f, 1.08f, vertices[i].y));
                weights[i] = new BoneWeight
                {
                    boneIndex0 = 0,
                    boneIndex1 = 1,
                    weight0 = 1f - chestWeight,
                    weight1 = chestWeight
                };
            }

            skinnedMesh.boneWeights = weights;
            skinnedMesh.RecalculateNormals();
            skinnedMesh.RecalculateBounds();
            var savedMesh = SaveSkinnedMesh(skinnedMesh);

            var skinnedRenderer = skinnedObject.AddComponent<SkinnedMeshRenderer>();
            skinnedRenderer.sharedMesh = savedMesh;
            skinnedRenderer.bones = new[] { hips, chest };
            skinnedRenderer.rootBone = hips;
            skinnedRenderer.sharedMaterials = sourceMaterials;
            skinnedRenderer.quality = SkinQuality.Bone2;
            skinnedRenderer.updateWhenOffscreen = false;

            // Keep the source MeshFilter for deterministic rebuilds and editor
            // inspection, but let the skinned primary be the only visible body.
            var sourcePrimary = FindDescendant(root.transform, primaryName);
            var sourceRenderer = sourcePrimary != null ? sourcePrimary.GetComponent<MeshRenderer>() : null;
            if (sourceRenderer != null) sourceRenderer.enabled = false;
        }

        private static Mesh SaveSkinnedMesh(Mesh mesh)
        {
            var path = MeshRoot + "/" + mesh.name + ".asset";
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing != null)
            {
                EditorUtility.CopySerialized(mesh, existing);
                existing.name = mesh.name;
                EditorUtility.SetDirty(existing);
                UnityEngine.Object.DestroyImmediate(mesh);
                return existing;
            }

            AssetDatabase.CreateAsset(mesh, path);
            return AssetDatabase.LoadAssetAtPath<Mesh>(path);
        }

        private static void BuildLodGroup(GameObject root, int fighterIndex)
        {
            var farObject = new GameObject("ProductionFarSilhouette", typeof(MeshFilter), typeof(MeshRenderer));
            farObject.transform.SetParent(root.transform, false);
            farObject.transform.localPosition = new Vector3(0f, 0.72f, 0f);
            farObject.transform.localScale = Vector3.one * 1.1f;
            var farMesh = CreateFarSilhouetteMesh("FarSilhouette" + fighterIndex, fighterIndex);
            farObject.GetComponent<MeshFilter>().sharedMesh = SaveSkinnedMesh(farMesh);

            Material farMaterial = null;
            var sourceRenderers = root.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < sourceRenderers.Length; i++)
            {
                if (sourceRenderers[i] == null || sourceRenderers[i].gameObject == farObject || sourceRenderers[i].gameObject.name.StartsWith("Vfx_", StringComparison.Ordinal)) continue;
                var materials = sourceRenderers[i].sharedMaterials;
                if (materials != null && materials.Length > 0 && materials[0] != null)
                {
                    farMaterial = materials[0];
                    break;
                }
            }
            farObject.GetComponent<MeshRenderer>().sharedMaterial = farMaterial;

            var nearRenderers = new List<Renderer>();
            for (var i = 0; i < sourceRenderers.Length; i++)
            {
                var renderer = sourceRenderers[i];
                if (renderer == null || renderer.gameObject == farObject || renderer.gameObject.name.StartsWith("Vfx_", StringComparison.Ordinal)) continue;
                nearRenderers.Add(renderer);
            }

            var lod = root.GetComponent<LODGroup>();
            if (lod == null) lod = root.AddComponent<LODGroup>();
            if (lod == null)
            {
                Debug.LogError("Could not add LODGroup to production fighter " + root.name + ".");
                return;
            }
            lod.fadeMode = LODFadeMode.CrossFade;
            lod.animateCrossFading = true;
            lod.SetLODs(new[]
            {
                new LOD(0.24f, nearRenderers.ToArray()),
                new LOD(0.055f, new[] { farObject.GetComponent<Renderer>() })
            });
            lod.RecalculateBounds();
        }

        private static Mesh CreateFarSilhouetteMesh(string name, int fighterIndex)
        {
            const int sides = 8;
            var vertices = new Vector3[sides * 2 + 2];
            var uv = new Vector2[vertices.Length];
            var lowerRadius = fighterIndex == 1 ? 0.46f : fighterIndex == 2 ? 0.42f : 0.34f;
            var upperRadius = fighterIndex == 1 ? 0.38f : fighterIndex == 2 ? 0.34f : 0.27f;
            for (var side = 0; side < sides; side++)
            {
                var angle = side * Mathf.PI * 2f / sides;
                vertices[side] = new Vector3(Mathf.Cos(angle) * lowerRadius, 0f, Mathf.Sin(angle) * lowerRadius);
                vertices[sides + side] = new Vector3(Mathf.Cos(angle) * upperRadius, 1.58f, Mathf.Sin(angle) * upperRadius);
                uv[side] = new Vector2((float)side / sides, 0f);
                uv[sides + side] = new Vector2((float)side / sides, 1f);
            }
            var bottom = sides * 2;
            var top = bottom + 1;
            vertices[bottom] = Vector3.zero;
            vertices[top] = Vector3.up * 1.75f;
            uv[bottom] = new Vector2(0.5f, 0f);
            uv[top] = new Vector2(0.5f, 1f);
            var triangles = new List<int>(sides * 12);
            for (var side = 0; side < sides; side++)
            {
                var next = (side + 1) % sides;
                triangles.Add(side); triangles.Add(next); triangles.Add(sides + next);
                triangles.Add(side); triangles.Add(sides + next); triangles.Add(sides + side);
                triangles.Add(bottom); triangles.Add(next); triangles.Add(side);
                triangles.Add(top); triangles.Add(sides + side); triangles.Add(sides + next);
            }
            var mesh = new Mesh { name = name };
            mesh.vertices = vertices;
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        private static Transform CreateBone(string name, Transform parent, Vector3 localPosition)
        {
            var bone = new GameObject(name).transform;
            bone.SetParent(parent, false);
            bone.localPosition = localPosition;
            bone.localRotation = Quaternion.identity;
            bone.localScale = Vector3.one;
            return bone;
        }

        private static void ReparentPart(Transform part, Transform parent)
        {
            if (part == null || parent == null) return;
            part.SetParent(parent, true);
        }

        private static Transform FindDescendant(Transform parent, string name)
        {
            if (parent == null) return null;
            if (parent.name == name) return parent;
            for (var i = 0; i < parent.childCount; i++)
            {
                var match = FindDescendant(parent.GetChild(i), name);
                if (match != null) return match;
            }

            return null;
        }

        private static ParticleSystem AddVfxInstance(Transform parent, GameObject prefab, string name)
        {
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = name;
            instance.transform.SetParent(parent, false);
            instance.transform.localPosition = new Vector3(0f, 1.0f, 0f);
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
            return instance.GetComponent<ParticleSystem>();
        }

        private static void RemoveChild(Transform parent, string name)
        {
            var child = parent.Find(name);
            if (child != null) UnityEngine.Object.DestroyImmediate(child.gameObject, true);
        }

        private static void RemoveChildrenWithPrefix(Transform parent, string prefix)
        {
            for (var i = parent.childCount - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
                if (child.name.StartsWith(prefix, StringComparison.Ordinal)) UnityEngine.Object.DestroyImmediate(child.gameObject, true);
            }
        }
    }
}
