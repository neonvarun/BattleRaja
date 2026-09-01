using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.AI;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.Gadgets;
using BattleRaja.Presentation.Flow;
using BattleRaja.Presentation.UI;
using BattleRaja.Presentation.Visuals;
using BattleRaja.Infrastructure.Networking;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleRaja.Editor
{
    public static class BuildEntrypoints
    {
        private const string ExpectedUnityVersion = "6000.5.6f1";
        private const string BootstrapScenePath = "Assets/BattleRaja/Scenes/Bootstrap/Bootstrap.unity";
        private const string MovementLabScenePath = "Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity";
        private const string BazaarBastionScenePath = "Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity";
        private const string TutorialArenaScenePath = "Assets/BattleRaja/Scenes/Tutorial/TutorialArena.unity";
        private const string MovementAssetFolder = "Assets/BattleRaja/Content/Movement";
        private const string BazaarPrefabFolder = "Assets/BattleRaja/Content/Prefabs";
        private const string BazaarArchitecturePrefabPath = BazaarPrefabFolder + "/BazaarArchitecture.prefab";
        private const string TuningAssetPath = MovementAssetFolder + "/M1-MovementTuning.asset";
        private const string InputAssetPath = MovementAssetFolder + "/BattleRajaMovement.inputactions";
        private const string WeaponAssetPath = "Assets/BattleRaja/Content/Weapons/M2-TrainingBolt.asset";
        private const string BijliWeaponAssetPath = "Assets/BattleRaja/Content/Weapons/M3-BijliElectricBolt.asset";
        private const string FighterAssetPath = "Assets/BattleRaja/Content/Fighters/M3-Bijli.asset";
        private const string PehelFighterAssetPath = "Assets/BattleRaja/Content/Fighters/M7-Pehel.asset";
        private const string MayaFighterAssetPath = "Assets/BattleRaja/Content/Fighters/M7-Maya.asset";
        private const string PehelWeaponAssetPath = "Assets/BattleRaja/Content/Weapons/M7-PehelSweep.asset";
        private const string MayaWeaponAssetPath = "Assets/BattleRaja/Content/Weapons/M7-MayaShard.asset";
        private const string DevelopmentApplicationId = "com.example.battleraja.m11";
        private const string AndroidApplicationIdEnvironmentVariable = "BATTLERAJA_ANDROID_APPLICATION_ID";
        private const string V1IconAssetPath = "Assets/BattleRaja/Art/V1/BattleRaja-AppIcon-PlayStore.png";
        private const string V1FeatureArtAssetPath = "Assets/BattleRaja/Art/V1/BattleRaja-FeatureArt-OriginalCandidate.png";
        private const string GadgetAssetFolder = "Assets/BattleRaja/Content/Gadgets";

        public static void CreateBootstrapScene()
        {
            EnsureUrpAsset();
            ProductionArtBuilder.BuildAll();

            var scene = File.Exists(BootstrapScenePath)
                ? EditorSceneManager.OpenScene(BootstrapScenePath, OpenSceneMode.Single)
                : EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            if (GameObject.Find("BootstrapCamera") == null)
            {
                var cameraObject = new GameObject("BootstrapCamera");
                var camera = cameraObject.AddComponent<Camera>();
                camera.transform.position = new Vector3(0f, 8f, -8f);
                camera.transform.rotation = Quaternion.Euler(35f, 0f, 0f);
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.08f, 0.10f, 0.14f, 1f);
            }

            if (GameObject.Find("BootstrapLight") == null)
            {
                var lightObject = new GameObject("BootstrapLight");
                var light = lightObject.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1f;
                light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            }

            if (GameObject.Find("ProductionFlow") == null)
            {
                var flowObject = new GameObject("ProductionFlow");
                flowObject.AddComponent<ProductionFlowController>();
            }

            var flow = GameObject.Find("ProductionFlow")?.GetComponent<ProductionFlowController>();
            if (flow != null)
            {
                SetObjectReference(flow, "menuFeatureArt", AssetDatabase.LoadAssetAtPath<Texture2D>(V1FeatureArtAssetPath));
                SetObjectReference(flow, "bijliPreviewPrefab", AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.BijliPrefabPath));
                SetObjectReference(flow, "pehelPreviewPrefab", AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.PehelPrefabPath));
                SetObjectReference(flow, "mayaPreviewPrefab", AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.MayaPrefabPath));
            }

            if (UnityEngine.Object.FindAnyObjectByType<EventSystem>() == null)
            {
                var eventObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
                eventObject.GetComponent<EventSystem>().sendNavigationEvents = true;
            }

            EditorSceneManager.SaveScene(scene, BootstrapScenePath);
        }

        public static void CreateMovementLabScene()
        {
            EnsureUrpAsset();
            ProductionArtBuilder.BuildAll();
            Directory.CreateDirectory(MovementAssetFolder);
            Directory.CreateDirectory("Assets/BattleRaja/Scenes/MovementLab");
            var tuningAsset = EnsureTuningAsset();
            var inputAsset = EnsureInputAsset();
            var weaponAsset = EnsureWeaponAsset();
            var bijliWeaponAsset = EnsureBijliWeaponAsset();
            var fighterAsset = EnsureFighterAsset();
            var pehelWeapon = EnsureVariantWeaponAsset(PehelWeaponAssetPath, FighterDefinition.Pehel.BasicAttack);
            var mayaWeapon = EnsureVariantWeaponAsset(MayaWeaponAssetPath, FighterDefinition.Maya.BasicAttack);
            var pehelAsset = EnsureFighterVariantAsset(PehelFighterAssetPath, "fighter.pehel", "Pehel", FighterDefinition.Pehel, pehelWeapon);
            var mayaAsset = EnsureFighterVariantAsset(MayaFighterAssetPath, "fighter.maya", "Maya", FighterDefinition.Maya, mayaWeapon);
            EnsureGadgetAssets();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            tuningAsset = AssetDatabase.LoadAssetAtPath<MovementTuningAsset>(TuningAssetPath);
            inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);
            weaponAsset = AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(WeaponAssetPath);
            bijliWeaponAsset = AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(BijliWeaponAssetPath);
            fighterAsset = AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(FighterAssetPath);
            AssetDatabase.ImportAsset(PehelFighterAssetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.ImportAsset(MayaFighterAssetPath, ImportAssetOptions.ForceUpdate);
            pehelAsset = AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(PehelFighterAssetPath);
            mayaAsset = AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(MayaFighterAssetPath);
            SetObjectReference(fighterAsset, "movementTuning", tuningAsset);
            SetObjectReference(fighterAsset, "basicAttack", bijliWeaponAsset);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            pehelAsset = AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(PehelFighterAssetPath);
            mayaAsset = AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(MayaFighterAssetPath);
            RenderSettings.ambientLight = new Color(0.28f, 0.31f, 0.38f, 1f);

            var arena = new GameObject("MovementLab");
            var marker = arena.AddComponent<MovementLabScene>();

            var floorMaterial = EnsureMaterial("MovementLabFloor", new Color(0.20f, 0.24f, 0.29f, 1f));
            var wallMaterial = EnsureMaterial("MovementLabWalls", new Color(0.42f, 0.48f, 0.56f, 1f));
            var obstacleMaterial = EnsureMaterial("MovementLabObstacles", new Color(0.56f, 0.38f, 0.23f, 1f));
            var playerMaterial = EnsureMaterial("MovementLabPlayer", new Color(0.19f, 0.64f, 0.92f, 1f));
            var indicatorMaterial = EnsureMaterial("MovementLabIndicator", new Color(1f, 0.78f, 0.16f, 1f));
            var projectileMaterial = EnsureMaterial("MovementLabProjectile", new Color(0.32f, 0.92f, 1f, 1f));
            var impactMaterial = EnsureMaterial("MovementLabImpact", new Color(1f, 0.78f, 0.16f, 1f));
            tuningAsset = AssetDatabase.LoadAssetAtPath<MovementTuningAsset>(TuningAssetPath);
            inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);
            weaponAsset = AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(WeaponAssetPath);

            CreateBlock("ArenaFloor", new Vector3(0f, -0.25f, 0f), new Vector3(28f, 0.5f, 20f), floorMaterial, arena.transform);
            CreateBlock("BoundaryWest", new Vector3(-14f, 1f, 0f), new Vector3(0.5f, 2f, 20f), wallMaterial, arena.transform);
            CreateBlock("BoundaryEast", new Vector3(14f, 1f, 0f), new Vector3(0.5f, 2f, 20f), wallMaterial, arena.transform);
            CreateBlock("BoundaryNorth", new Vector3(0f, 1f, 10f), new Vector3(28f, 2f, 0.5f), wallMaterial, arena.transform);
            CreateBlock("BoundarySouth", new Vector3(0f, 1f, -10f), new Vector3(28f, 2f, 0.5f), wallMaterial, arena.transform);
            CreateBlock("NarrowLaneWest", new Vector3(-3f, 1f, 3f), new Vector3(0.45f, 2f, 10f), wallMaterial, arena.transform);
            CreateBlock("NarrowLaneEast", new Vector3(3f, 1f, 3f), new Vector3(0.45f, 2f, 10f), wallMaterial, arena.transform);
            CreateBlock("CornerWallHorizontal", new Vector3(7f, 1f, 3f), new Vector3(6f, 2f, 0.45f), wallMaterial, arena.transform);
            CreateBlock("CornerWallVertical", new Vector3(10f, 1f, 6f), new Vector3(0.45f, 2f, 6f), wallMaterial, arena.transform);
            CreateBlock("ObstacleNorthWest", new Vector3(-8f, 1f, 5f), new Vector3(2f, 2f, 2f), obstacleMaterial, arena.transform);
            CreateBlock("ObstacleSouthWest", new Vector3(-8f, 1f, -5f), new Vector3(2f, 2f, 2f), obstacleMaterial, arena.transform);
            CreateBlock("ObstacleSouthEast", new Vector3(8f, 1f, -5f), new Vector3(2f, 2f, 2f), obstacleMaterial, arena.transform);

            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "MovementLabPlayer";
            player.transform.SetParent(arena.transform);
            player.transform.position = new Vector3(0f, 1f, -6f);
            player.layer = 2;
            player.GetComponent<Renderer>().sharedMaterial = playerMaterial;
            UnityEngine.Object.DestroyImmediate(player.GetComponent<Collider>());
            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.42f;
            controller.center = Vector3.zero;
            controller.stepOffset = 0.25f;
            controller.slopeLimit = 45f;
            var inputAdapter = player.AddComponent<PlayerInputAdapter>();
            player.AddComponent<InputFocusController>();
            var agent = player.AddComponent<MovementPlayerAgent>();
            var indicator = player.AddComponent<AimDirectionIndicator>();
            var attackController = player.AddComponent<CombatAttackController>();
            var fighterController = player.AddComponent<BijliFighterController>();
            var playerHealth = player.AddComponent<CombatHealth>();
            var playerGadget = player.AddComponent<GadgetUser>();
            var playerPresentation = player.AddComponent<FighterPresentation>();
            ConfigureProductionArt(playerPresentation);
            var dashTrail = player.AddComponent<TrailRenderer>();
            dashTrail.time = 0.24f;
            dashTrail.startWidth = 0.28f;
            dashTrail.endWidth = 0.02f;
            dashTrail.sharedMaterial = projectileMaterial;

            var cameraObject = new GameObject("MovementLabCamera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = player.transform.position + new Vector3(0f, 12f, -8f);
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.08f, 0.10f, 0.14f, 1f);
            var cameraController = cameraObject.AddComponent<TopDownCameraController>();

            var canvas = CreateTouchCanvas(out var movementStick, out var aimStick, out var attackButton, out var abilityButton, out var gadgetButton);
            var hud = CreateHud(canvas, out var hudText);
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            eventSystem.transform.SetParent(arena.transform);

            var combatSystems = new GameObject("CombatSystems");
            combatSystems.transform.SetParent(arena.transform);
            var damageResolver = combatSystems.AddComponent<CombatDamageResolver>();
            var impactPool = combatSystems.AddComponent<CombatImpactFeedbackPool>();
            var projectilePool = combatSystems.AddComponent<CombatProjectilePool>();
            var audioObject = new GameObject("AudioDirector");
            audioObject.transform.SetParent(arena.transform, false);
            audioObject.AddComponent<AudioSource>();
            audioObject.AddComponent<BattleRajaAudioDirector>();

            var dummy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            dummy.name = "TrainingDummy";
            dummy.transform.SetParent(arena.transform);
            dummy.transform.position = new Vector3(0f, 1f, 1f);
            dummy.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            dummy.GetComponent<Renderer>().sharedMaterial = obstacleMaterial;
            var dummyHealth = dummy.AddComponent<CombatHealth>();
            var dummyTarget = dummy.AddComponent<CombatTarget>();
            var dummyFlash = dummy.AddComponent<CombatHitFlash>();
            var trainingDummy = dummy.AddComponent<TrainingDummy>();

            var playerTarget = player.AddComponent<CombatTarget>();
            SetObjectReference(playerTarget, "health", playerHealth);
            SetInt(playerTarget, "entityId", 1);
            SetEnum(playerTarget, "faction", CombatFaction.Player);

            var botPositions = new[]
            {
                new Vector3(-10f, 1f, -7f),
                new Vector3(-6f, 1f, -1f),
                new Vector3(6f, 1f, -1f),
                new Vector3(10f, 1f, -7f),
                new Vector3(-10f, 1f, 7f),
                new Vector3(0f, 1f, 7f),
                new Vector3(10f, 1f, 7f)
            };
            for (var botIndex = 0; botIndex < botPositions.Length; botIndex++)
            {
                var botFighter = botIndex < 2 ? fighterAsset : botIndex < 5 ? pehelAsset : mayaAsset;
                CreateBotActor(
                    botIndex,
                    botPositions[botIndex],
                    arena.transform,
                    obstacleMaterial,
                    projectileMaterial,
                    tuningAsset,
                    botFighter,
                    projectilePool,
                    damageResolver,
                    weaponAsset,
                    pehelWeapon,
                    mayaWeapon);
            }

            CreatePickup("HealthPickup_A", new Vector3(-5f, 0.35f, 3f), arena.transform, impactMaterial);
            CreatePickup("HealthPickup_B", new Vector3(5f, 0.35f, 3f), arena.transform, impactMaterial);
            CreatePickup("HealthPickup_C", new Vector3(0f, 0.35f, -1f), arena.transform, impactMaterial);
            CreateGadgetPickup("GadgetPickup_Umbrella", GadgetDefinition.UmbrellaGuard.GadgetId.Value, new Vector3(-8f, 0.35f, 0f), arena.transform, indicatorMaterial);
            CreateGadgetPickup("GadgetPickup_Dhol", GadgetDefinition.DholBurst.GadgetId.Value, new Vector3(8f, 0.35f, 0f), arena.transform, indicatorMaterial);
            // Keep the tutorial-relevant healing gadget on the player's protected
            // south lane. The old north placement overlapped the (0, 7) bot spawn,
            // allowing a bot to consume it before a human could reach the route.
            CreateGadgetPickup("GadgetPickup_Tiffin", GadgetDefinition.TiffinStation.GadgetId.Value, new Vector3(0f, 0.35f, -4.8f), arena.transform, indicatorMaterial);
            var matchObject = new GameObject("OfflineMatch");
            matchObject.transform.SetParent(arena.transform);
            var matchController = matchObject.AddComponent<OfflineMatchController>();
            var matchHud = matchObject.AddComponent<OfflineMatchHud>();
            SetObjectReference(matchController, "damageResolver", damageResolver);
            SetObjectReference(matchController, "cameraController", cameraController);
            SetObjectReference(matchHud, "match", matchController);

            SetObjectReference(agent, "tuningAsset", tuningAsset);
            SetObjectReference(agent, "inputAdapter", inputAdapter);
            SetObjectReference(agent, "aimIndicator", indicator);
            SetObjectReference(indicator, "indicatorMaterial", indicatorMaterial);
            SetObjectReference(inputAdapter, "actionsAsset", inputAsset);
            SetObjectReference(inputAdapter, "worldCamera", camera);
            SetObjectReference(inputAdapter, "movementStick", movementStick);
            SetObjectReference(inputAdapter, "aimStick", aimStick);
            SetObjectReference(inputAdapter, "attackButton", attackButton);
            SetObjectReference(inputAdapter, "abilityButton", abilityButton);
            SetObjectReference(gadgetButton, "user", playerGadget);
            SetObjectReference(inputAdapter, "aimOrigin", player.transform);
            SetObjectReference(attackController, "weapon", weaponAsset);
            SetObjectReference(attackController, "fighterDefinition", fighterAsset);
            SetObjectReference(attackController, "inputAdapter", inputAdapter);
            SetObjectReference(attackController, "movementAgent", agent);
            SetObjectReference(attackController, "projectilePool", projectilePool);
            SetObjectReference(projectilePool, "damageResolver", damageResolver);
            SetObjectReference(projectilePool, "impactPool", impactPool);
            SetObjectReference(projectilePool, "projectileMaterial", projectileMaterial);
            SetObjectReference(impactPool, "impactMaterial", impactMaterial);
            SetObjectReference(dummyTarget, "health", dummyHealth);
            SetObjectReference(trainingDummy, "target", dummyTarget);
            SetObjectReference(trainingDummy, "hitFlash", dummyFlash);
            SetObjectReference(cameraController, "followTarget", player.transform);
            SetObjectReference(agent, "fighterController", fighterController);
            SetObjectReference(fighterController, "fighterDefinition", fighterAsset);
            SetObjectReference(fighterController, "inputAdapter", inputAdapter);
            SetObjectReference(fighterController, "movementAgent", agent);
            SetObjectReference(fighterController, "characterController", controller);
            SetObjectReference(fighterController, "dashTrail", dashTrail);
            SetInt(fighterController, "dashCollisionMask", 1);
            SetObjectReference(hud, "fighter", fighterController);
            SetObjectReference(hud, "health", playerHealth);
            SetObjectReference(hud, "attack", attackController);
            SetObjectReference(hud, "statusText", hudText);
            SetObjectReference(playerGadget, "movementAgent", agent);
            SetObjectReference(playerGadget, "combatTarget", playerTarget);
            SetObjectReference(playerGadget, "health", playerHealth);
            CreateGadgetHud(canvas, playerGadget);
            SetInt(playerHealth, "maxHealth", fighterAsset != null ? fighterAsset.ToDomain().MaxHealth : 85);
            SetObjectReference(marker, "player", agent);
            SetObjectReference(marker, "cameraController", cameraController);
            SetObjectReference(marker, "trainingDummy", trainingDummy);
            SetObjectReference(marker, "projectilePool", projectilePool);
            SetObjectReference(marker, "damageResolver", damageResolver);
            SetInt(cameraController, "obstructionMask", 1);

            EditorSceneManager.SaveScene(scene, MovementLabScenePath);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(MovementLabScenePath, true),
                new EditorBuildSettingsScene(BootstrapScenePath, true)
            };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Creates the first production-facing arena from the technical lab as a controlled
        /// scene copy. The lab stays the regression fixture; only the copy is decorated and
        /// converted to fighter-specific bot controllers.
        /// </summary>
        public static void CreateBazaarBastionScene()
        {
            EnsureUrpAsset();
            ProductionArtBuilder.BuildAll();
            ProductionEnvironmentBuilder.BuildAll();
            Directory.CreateDirectory("Assets/BattleRaja/Scenes/Gameplay");
            Directory.CreateDirectory(MovementAssetFolder);

            var tuningAsset = EnsureTuningAsset();
            var bijliWeapon = EnsureBijliWeaponAsset();
            var pehelWeapon = EnsureVariantWeaponAsset(PehelWeaponAssetPath, FighterDefinition.Pehel.BasicAttack);
            var mayaWeapon = EnsureVariantWeaponAsset(MayaWeaponAssetPath, FighterDefinition.Maya.BasicAttack);
            var bijliAsset = EnsureFighterAsset();
            var pehelAsset = EnsureFighterVariantAsset(PehelFighterAssetPath, "fighter.pehel", "Pehel", FighterDefinition.Pehel, pehelWeapon);
            var mayaAsset = EnsureFighterVariantAsset(MayaFighterAssetPath, "fighter.maya", "Maya", FighterDefinition.Maya, mayaWeapon);
            EnsureGadgetAssets();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var productionScene = EditorSceneManager.OpenScene(BazaarBastionScenePath, OpenSceneMode.Single);
            var arena = GameObject.Find("BazaarBastion");
            if (arena == null) throw new BuildFailedException("Bazaar Bastion root was not found while validating the production scene.");

            // Keep the tutorial-relevant healing gadget on the player's protected
            // south lane in the authored production scene as well as in fresh
            // development-scene generation. Keep it beyond the player's automatic
            // collection radius so the player can choose between nearby pickups.
            var tiffinPickup = arena.GetComponentsInChildren<GadgetPickup>(true)
                .FirstOrDefault(pickup => pickup.GadgetId.Equals(GadgetDefinition.TiffinStation.GadgetId));
            if (tiffinPickup != null)
            {
                tiffinPickup.transform.localPosition = new Vector3(0f, 0.35f, -4.8f);
            }

            // Existing production scenes were copied before the saved gadget
            // prefab references existed. Reconcile every pickup on each pass so
            // the scene cannot retain the old primitive fallback hierarchy.
            foreach (var pickup in arena.GetComponentsInChildren<GadgetPickup>(true))
            {
                var visuals = pickup.GetComponent<GadgetPickupVisuals>() ?? pickup.gameObject.AddComponent<GadgetPickupVisuals>();
                ConfigureProductionGadgetArt(visuals);
            }

            var labMarker = arena.GetComponent<MovementLabScene>();
            if (labMarker != null) UnityEngine.Object.DestroyImmediate(labMarker);
            var productionMarker = arena.GetComponent<BazaarBastionScene>() ?? arena.AddComponent<BazaarBastionScene>();

            var floor = EnsureMaterial("BazaarBastionFloor", new Color(0.32f, 0.24f, 0.19f, 1f));
            var wall = EnsureMaterial("BazaarBastionTeal", new Color(0.12f, 0.34f, 0.36f, 1f));
            var stall = EnsureMaterial("BazaarBastionTerracotta", new Color(0.62f, 0.24f, 0.14f, 1f));
            var bijliMaterial = EnsureMaterial("BazaarBastionBijli", new Color(0.16f, 0.68f, 0.92f, 1f));
            var pehelMaterial = EnsureMaterial("BazaarBastionPehel", new Color(0.92f, 0.39f, 0.16f, 1f));
            var mayaMaterial = EnsureMaterial("BazaarBastionMaya", new Color(0.72f, 0.32f, 0.86f, 1f));
            ApplyBazaarPalette(arena.transform, floor, wall, stall);
            // The old scene copy contained a runtime-generated primitive architecture
            // holder. Remove that presentation-only instance and bind the saved,
            // textured production environment through the controlled editor path below.
            var legacyArchitecture = arena.transform.Find("BazaarArchitecture");
            if (legacyArchitecture != null)
            {
                UnityEngine.Object.DestroyImmediate(legacyArchitecture.gameObject);
            }
            var environmentVisuals = arena.GetComponent<BazaarBastionVisuals>() ?? arena.AddComponent<BazaarBastionVisuals>();
            SetObjectReference(
                environmentVisuals,
                "environmentPrefab",
                AssetDatabase.LoadAssetAtPath<GameObject>(ProductionEnvironmentBuilder.EnvironmentPrefabPath));
            SetInt(environmentVisuals, "decorationQuality", 1);
            SetBool(environmentVisuals, "allowRuntimeFallback", false);
            if (arena.GetComponentInChildren<BattleRajaAudioDirector>(true) == null)
            {
                var audioObject = new GameObject("AudioDirector");
                audioObject.transform.SetParent(arena.transform, false);
                audioObject.AddComponent<AudioSource>();
                audioObject.AddComponent<BattleRajaAudioDirector>();
            }

            var presentationAgents = arena.GetComponentsInChildren<MovementPlayerAgent>(true);
            for (var i = 0; i < presentationAgents.Length; i++)
            {
                if (presentationAgents[i].GetComponent<FighterPresentation>() == null)
                {
                    presentationAgents[i].gameObject.AddComponent<FighterPresentation>();
                }
            }

            foreach (var overlay in arena.GetComponentsInChildren<BotDebugOverlay>(true))
            {
                SetBool(overlay, "showOverlay", false);
            }

            var damageResolver = arena.GetComponentInChildren<CombatDamageResolver>();
            var projectilePool = arena.GetComponentInChildren<CombatProjectilePool>();
            var matchController = arena.GetComponentInChildren<OfflineMatchController>(true);
            if (matchController == null) throw new BuildFailedException("Bazaar Bastion match controller is missing.");
            var objectiveView = arena.GetComponent<BastionCrownObjectiveView>() ?? arena.AddComponent<BastionCrownObjectiveView>();
            SetObjectReference(objectiveView, "match", matchController);
            var playerAgent = arena.GetComponentsInChildren<MovementPlayerAgent>(true)
                .FirstOrDefault(agent => agent.ActorId == 1);
            var cameraController = UnityEngine.Object.FindAnyObjectByType<TopDownCameraController>();
            // Frame the action like a compact arena game: the full combat space is
            // still discoverable, but fighters and pickups have enough screen
            // presence to read at a glance on the approved phone.
            SetVector3(cameraController, "cameraOffset", new Vector3(0f, 10f, -11f));
            SetFloat(cameraController, "orthographicSize", 8.25f);
            SetObjectReference(productionMarker, "player", playerAgent);
            SetObjectReference(productionMarker, "cameraController", cameraController);
            SetObjectReference(productionMarker, "matchController", matchController);
            SetObjectReference(productionMarker, "projectilePool", projectilePool);
            SetObjectReference(productionMarker, "damageResolver", damageResolver);
            SetBool(matchController, "authorityDrivenMovement", true);
            // Bots may use a conservative PvE scale, but never a damage bonus over
            // the human player's authoritative weapon definition.
            SetFloat(matchController, "botWeaponDamageMultiplier", 1f);
            var bots = UnityEngine.Object.FindObjectsByType<BotBrain>()
                .OrderBy(bot => bot.GetComponent<MovementPlayerAgent>() != null ? bot.GetComponent<MovementPlayerAgent>().ActorId : int.MaxValue)
                .ToArray();
            if (bots.Length != 7) throw new BuildFailedException($"Bazaar Bastion requires exactly seven autonomous bot actors in the production scene; found {bots.Length}.");

            ConfigureProductionBotSpawns(bots);

            // Reconcile every existing bot slot on each controlled generation pass.
            // Older scene copies can contain a controller whose definition asset belongs
            // to a different fighter; leaving that mismatch in place produces fair-looking
            // but mechanically invalid ability commands at runtime.
            ConfigureProductionBot(bots[0], bijliAsset, bijliMaterial, null, damageResolver, projectilePool, bijliWeapon, pehelWeapon, mayaWeapon);
            ConfigureProductionBot(bots[1], pehelAsset, pehelMaterial, null, damageResolver, projectilePool, bijliWeapon, pehelWeapon, mayaWeapon);
            ConfigureProductionBot(bots[2], mayaAsset, mayaMaterial, mayaMaterial, damageResolver, projectilePool, bijliWeapon, pehelWeapon, mayaWeapon);
            for (var i = 3; i < bots.Length; i++)
            {
                ConfigureProductionBot(bots[i], bijliAsset, bijliMaterial, null, damageResolver, projectilePool, bijliWeapon, pehelWeapon, mayaWeapon);
            }

            // Production is the canonical 1 human + 3 allied AI versus 4 rival AI
            // contract. Older scene copies used actor IDs 10-16 and marked every
            // bot as Enemy; reconcile the complete authority identity on every
            // controlled generation pass instead of relying on scene history.
            for (var i = 0; i < bots.Length; i++)
            {
                ConfigureBastionBotIdentity(
                    bots[i],
                    i + 2,
                    i < 3 ? CombatFaction.Player : CombatFaction.Enemy);
            }

            ConfigurePlayerFighterSelection(arena.transform, tuningAsset, pehelAsset, mayaAsset, mayaMaterial, damageResolver);

            EditorSceneManager.MarkSceneDirty(productionScene);
            EditorSceneManager.SaveScene(productionScene, BazaarBastionScenePath);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(BootstrapScenePath, true),
                new EditorBuildSettingsScene(TutorialArenaScenePath, true),
                new EditorBuildSettingsScene(BazaarBastionScenePath, true),
                new EditorBuildSettingsScene(MovementLabScenePath, true)
            };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void ConfigureProductionArt(FighterPresentation presentation)
        {
            if (presentation == null) return;
            SetObjectReference(
                presentation,
                "bijliModelPrefab",
                AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.BijliPrefabPath));
            SetObjectReference(
                presentation,
                "pehelModelPrefab",
                AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.PehelPrefabPath));
            SetObjectReference(
                presentation,
                "mayaModelPrefab",
                AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.MayaPrefabPath));
        }

        private static void ConfigureProductionGadgetArt(GadgetPickupVisuals visuals)
        {
            if (visuals == null) return;
            var umbrella = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.UmbrellaPrefabPath);
            var dhol = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.DholPrefabPath);
            var tiffin = AssetDatabase.LoadAssetAtPath<GameObject>(ProductionArtBuilder.TiffinPrefabPath);
            SetObjectReference(
                visuals,
                "umbrellaModelPrefab",
                umbrella);
            SetObjectReference(
                visuals,
                "dholModelPrefab",
                dhol);
            SetObjectReference(
                visuals,
                "tiffinModelPrefab",
                tiffin);
            visuals.ConfigureSavedPrefabs(umbrella, dhol, tiffin);
        }

        private static void ConfigureProductionBotSpawns(IReadOnlyList<BotBrain> bots)
        {
            var positions = new[]
            {
                new Vector3(-9.8f, 1f, -7.4f),
                new Vector3(-5.0f, 1f, -0.8f),
                new Vector3(5.0f, 1f, -0.8f),
                new Vector3(9.8f, 1f, -7.4f),
                new Vector3(-9.8f, 1f, 7.0f),
                new Vector3(0f, 1f, 7.2f),
                new Vector3(8.0f, 1f, 7.0f)
            };
            var collision = ArenaCollisionDefinition.BazaarBastion;
            if (bots.Count != positions.Length)
            {
                throw new BuildFailedException($"Expected {positions.Length} production bot slots, found {bots.Count}.");
            }

            for (var i = 0; i < bots.Count; i++)
            {
                var agent = bots[i] != null ? bots[i].GetComponent<MovementPlayerAgent>() : null;
                if (agent == null) throw new BuildFailedException($"Production bot slot {i} has no movement agent.");
                var position = positions[i];
                if (collision.IsPointBlocked(new Float2(position.x, position.z)))
                {
                    throw new BuildFailedException($"Production bot spawn {agent.ActorId} is inside authored collision.");
                }

                agent.transform.position = position;
            }
        }

        /// <summary>
        /// Creates a replayable onboarding arena from the tested movement lab. Bot actors stay
        /// in the simulation so the real match authority still has valid separated spawns, but
        /// their decision components are disabled and a tutorial overlay owns the prompts.
        /// </summary>
        public static void CreateTutorialArenaScene()
        {
            EnsureUrpAsset();
            Directory.CreateDirectory("Assets/BattleRaja/Scenes/Tutorial");

            // TutorialArena is a committed release input. Recopying MovementLab on
            // every Android build recreates TutorialOverlay with fresh Unity file IDs
            // even when nothing changed, dirtying an otherwise clean source tree. Keep
            // a valid authored scene stable; delete it (or its overlay) to request a
            // deliberate regeneration.
            if (File.Exists(TutorialArenaScenePath))
            {
                var existingTutorial = EditorSceneManager.OpenScene(TutorialArenaScenePath, OpenSceneMode.Single);
                var existingArena = GameObject.Find("TutorialArena");
                if (existingArena != null && existingArena.GetComponentInChildren<TutorialOverlay>(true) != null)
                {
                    if (ConfigureTutorialEliminationTarget(existingArena.transform))
                    {
                        EditorSceneManager.MarkSceneDirty(existingTutorial);
                        EditorSceneManager.SaveScene(existingTutorial, TutorialArenaScenePath);
                    }

                    EditorBuildSettings.scenes = new[]
                    {
                        new EditorBuildSettingsScene(BootstrapScenePath, true),
                        new EditorBuildSettingsScene(TutorialArenaScenePath, true),
                        new EditorBuildSettingsScene(BazaarBastionScenePath, true),
                        new EditorBuildSettingsScene(MovementLabScenePath, true)
                    };
                    Debug.Log("BattleRaja Tutorial Arena already exists; keeping committed scene file IDs.");
                    return;
                }
            }

            var sourceScene = EditorSceneManager.OpenScene(MovementLabScenePath, OpenSceneMode.Single);
            EditorSceneManager.SaveScene(sourceScene, TutorialArenaScenePath);
            var arena = GameObject.Find("MovementLab");
            if (arena == null) throw new BuildFailedException("MovementLab root was not found while creating Tutorial Arena.");
            arena.name = "TutorialArena";
            ConfigureTutorialEliminationTarget(arena.transform);

            foreach (var brain in arena.GetComponentsInChildren<BotBrain>(true))
            {
                brain.enabled = false;
                var overlay = brain.GetComponent<BotDebugOverlay>();
                if (overlay != null) SetBool(overlay, "showOverlay", false);
            }

            var tutorial = arena.GetComponentInChildren<TutorialOverlay>(true);
            if (tutorial == null)
            {
                var tutorialObject = new GameObject("TutorialOverlay");
                tutorialObject.transform.SetParent(arena.transform, false);
                tutorialObject.AddComponent<TutorialOverlay>();
            }

            EditorSceneManager.MarkSceneDirty(sourceScene);
            EditorSceneManager.SaveScene(sourceScene, TutorialArenaScenePath);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(BootstrapScenePath, true),
                new EditorBuildSettingsScene(TutorialArenaScenePath, true),
                new EditorBuildSettingsScene(BazaarBastionScenePath, true),
                new EditorBuildSettingsScene(MovementLabScenePath, true)
            };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Keeps the first elimination lesson readable on a compact portrait screen. The
        /// tutorial keeps the real match authority and all seven participants, but places
        /// actor 11 in the open south lane so a default upward aim can reach a stationary
        /// target without requiring a diagonal camera-coordinate guess. Production spawns
        /// and the MovementLab regression fixture are intentionally unchanged.
        /// </summary>
        private static bool ConfigureTutorialEliminationTarget(Transform arena)
        {
            if (arena == null) return false;

            var target = arena.GetComponentsInChildren<MovementPlayerAgent>(true)
                .FirstOrDefault(agent => agent != null && agent.ActorId == 11);
            if (target == null) return false;

            var desiredPosition = new Vector3(0f, 1f, -3.2f);
            if ((target.transform.position - desiredPosition).sqrMagnitude <= 0.0001f)
            {
                return false;
            }

            target.transform.position = desiredPosition;
            return true;
        }

        private static void ConfigurePlayerFighterSelection(
            Transform arena,
            MovementTuningAsset tuningAsset,
            FighterDefinitionAsset pehelAsset,
            FighterDefinitionAsset mayaAsset,
            Material mayaMaterial,
            CombatDamageResolver damageResolver)
        {
            var playerAgent = arena.GetComponentsInChildren<MovementPlayerAgent>(true)
                .FirstOrDefault(agent => agent.ActorId == 1);
            if (playerAgent == null) throw new BuildFailedException("Bazaar Bastion player actor is missing.");

            var player = playerAgent.gameObject;
            var playerInput = player.GetComponent<PlayerInputAdapter>();
            var characterController = player.GetComponent<CharacterController>();
            var attack = player.GetComponent<CombatAttackController>();
            var bijli = player.GetComponent<BijliFighterController>();
            var pehel = player.GetComponent<PehelFighterController>() ?? player.AddComponent<PehelFighterController>();
            var maya = player.GetComponent<MayaFighterController>() ?? player.AddComponent<MayaFighterController>();
            var selection = player.GetComponent<PlayerFighterSelection>() ?? player.AddComponent<PlayerFighterSelection>();
            ConfigureProductionArt(player.GetComponent<FighterPresentation>());

            SetObjectReference(playerAgent, "tuningAsset", tuningAsset);
            SetObjectReference(playerAgent, "fighterController", bijli);
            SetObjectReference(pehel, "fighterDefinition", pehelAsset);
            SetObjectReference(pehel, "inputAdapter", playerInput);
            SetObjectReference(pehel, "movementAgent", playerAgent);
            SetObjectReference(pehel, "characterController", characterController);
            SetObjectReference(pehel, "damageResolver", damageResolver);
            SetEnum(pehel, "faction", CombatFaction.Player);
            SetInt(pehel, "chargeCollisionMask", 1);
            SetObjectReference(maya, "fighterDefinition", mayaAsset);
            SetObjectReference(maya, "inputAdapter", playerInput);
            SetObjectReference(maya, "movementAgent", playerAgent);
            SetEnum(maya, "faction", CombatFaction.Player);
            SetObjectReference(maya, "decoyMaterial", mayaMaterial);
            SetObjectReference(selection, "bijliDefinition", AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(FighterAssetPath));
            SetObjectReference(selection, "pehelDefinition", pehelAsset);
            SetObjectReference(selection, "mayaDefinition", mayaAsset);
            SetObjectReference(selection, "bijliController", bijli);
            SetObjectReference(selection, "pehelController", pehel);
            SetObjectReference(selection, "mayaController", maya);
            SetObjectReference(selection, "movementAgent", playerAgent);
            SetObjectReference(selection, "attackController", attack);
            SetObjectReference(selection, "inputAdapter", playerInput);

            if (bijli != null) bijli.enabled = true;
            pehel.enabled = false;
            maya.enabled = false;
        }

        public static void ValidateProject()
        {
            if (!string.Equals(Application.unityVersion, ExpectedUnityVersion, StringComparison.Ordinal))
            {
                throw new BuildFailedException($"Expected Unity {ExpectedUnityVersion}, found {Application.unityVersion}.");
            }

            var manifest = File.ReadAllText("Packages/manifest.json");
            var lockFile = File.ReadAllText("Packages/packages-lock.json");
            RequireText(manifest, "com.unity.inputsystem", "Packages/manifest.json");
            RequireText(manifest, "com.unity.render-pipelines.universal", "Packages/manifest.json");
            RequireText(manifest, "com.unity.test-framework", "Packages/manifest.json");
            RequireText(lockFile, "\"version\": \"17.5.0\"", "Packages/packages-lock.json URP version");
            RequireText(lockFile, "\"version\": \"1.20.0\"", "Packages/packages-lock.json Input System version");
            RequireText(lockFile, "\"version\": \"1.7.0\"", "Packages/packages-lock.json Test Framework version");

            if (manifest.IndexOf("photon", StringComparison.OrdinalIgnoreCase) >= 0 ||
                manifest.IndexOf("playfab", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new BuildFailedException("Photon and PlayFab are prohibited before their approved milestones.");
            }

            if (!File.Exists(BootstrapScenePath) || !File.Exists(MovementLabScenePath) || !File.Exists(TutorialArenaScenePath))
            {
                throw new BuildFailedException("Bootstrap, Tutorial Arena and MovementLab scenes must exist.");
            }

            if (GraphicsSettings.defaultRenderPipeline == null)
            {
                throw new BuildFailedException("The URP render pipeline is not assigned in GraphicsSettings.");
            }

            if (!File.Exists(WeaponAssetPath) || !File.Exists(BijliWeaponAssetPath) || !File.Exists(FighterAssetPath))
            {
                throw new BuildFailedException("M2/M3 projectile or Bijli fighter asset is missing.");
            }

            if (!File.Exists(BazaarArchitecturePrefabPath))
            {
                throw new BuildFailedException("Bazaar architecture prefab is missing.");
            }

            if (!File.Exists(ProductionEnvironmentBuilder.EnvironmentPrefabPath))
            {
                throw new BuildFailedException("Saved Bazaar Bastion production environment prefab is missing.");
            }

            var fighter = AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(FighterAssetPath);
            if (fighter == null)
            {
                throw new BuildFailedException("Bijli fighter definition asset is missing.");
            }

            if (!fighter.ToDomain().IsValid(out var reason))
            {
                throw new BuildFailedException($"Bijli fighter definition is invalid: {reason}");
            }

            if (EnsureGadgetAssets().Length != 3 ||
                AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(PehelFighterAssetPath) == null ||
                AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(MayaFighterAssetPath) == null)
            {
                throw new BuildFailedException("M11 foundation definitions are incomplete.");
            }

            if (!NetworkSessionConfig.Proof.IsValid(out var networkReason))
            {
                throw new BuildFailedException($"M8 network proof configuration is invalid: {networkReason}");
            }

            Debug.Log("BattleRaja Milestone 11 validation passed (external services remain approval-blocked).");
        }

        public static void BuildAndroidDevelopment()
        {
            CreateBootstrapScene();
            CreateBazaarBastionScene();
            CreateTutorialArenaScene();
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, ResolveAndroidApplicationId());
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel36;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            ApplyCandidateAndroidIcon();
            Build(
                "Builds/M11/Android/BattleRaja-M11.apk",
                BuildTarget.Android,
                BootstrapScenePath,
                TutorialArenaScenePath,
                BazaarBastionScenePath);
        }

        public static void BuildAndroidCurrentSceneDevelopment()
        {
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, ResolveAndroidApplicationId());
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel36;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            Build("Builds/M11/Android/BattleRaja-M11.apk", BuildTarget.Android);
        }

        public static void BuildWebDevelopment()
        {
            CreateBootstrapScene();
            CreateBazaarBastionScene();
            CreateTutorialArenaScene();
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.template = "PROJECT:BattleRaja";
            Build(
                "Builds/M11/Web",
                BuildTarget.WebGL,
                BootstrapScenePath,
                TutorialArenaScenePath,
                BazaarBastionScenePath);
        }

        public static void BuildWebCurrentSceneDevelopment()
        {
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.template = "PROJECT:BattleRaja";
            Build("Builds/M11/Web", BuildTarget.WebGL);
        }

        public static void BuildAndroidBazaarBastionDevelopment()
        {
            CreateBootstrapScene();
            CreateBazaarBastionScene();
            CreateTutorialArenaScene();
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, ResolveAndroidApplicationId());
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel36;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            ApplyCandidateAndroidIcon();
            Build("Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk", BuildTarget.Android, BootstrapScenePath, TutorialArenaScenePath, BazaarBastionScenePath);
        }

        /// <summary>
        /// Produces a debug-signed/non-publishable release-shaped App Bundle for local
        /// Play checks. The owner-approved release key, package identity and Play Console
        /// upload stay outside this method and require the explicit release gate.
        /// </summary>
        public static void BuildAndroidV1ReleaseCandidate()
        {
            PrepareAndroidV1ReleaseCandidate();
            EditorUserBuildSettings.buildAppBundle = true;
            try
            {
                Build(
                    "Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab",
                    BuildTarget.Android,
                    BuildOptions.None,
                    BootstrapScenePath,
                    TutorialArenaScenePath,
                    BazaarBastionScenePath);
            }
            finally
            {
                EditorUserBuildSettings.buildAppBundle = false;
            }
        }

        /// <summary>
        /// Produces a non-development debug-signed APK with the same scenes and player
        /// settings as the release-shaped bundle. It exists for Lava performance and
        /// interaction validation; it is not a publishable signed artifact.
        /// </summary>
        public static void BuildAndroidV1ReleaseCandidateApk()
        {
            PrepareAndroidV1ReleaseCandidate();
            EditorUserBuildSettings.buildAppBundle = false;
            Build(
                "Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk",
                BuildTarget.Android,
                BuildOptions.None,
                BootstrapScenePath,
                TutorialArenaScenePath,
                BazaarBastionScenePath);
        }

        private static void PrepareAndroidV1ReleaseCandidate()
        {
            CreateBootstrapScene();
            CreateBazaarBastionScene();
            CreateTutorialArenaScene();
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            EditorUserBuildSettings.development = false;
            EditorUserBuildSettings.allowDebugging = false;
            EditorUserBuildSettings.connectProfiler = false;
            EditorUserBuildSettings.buildWithDeepProfilingSupport = false;
            var applicationId = ResolveAndroidApplicationId();
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, applicationId);
            Debug.Log($"BattleRaja Android application identifier: {applicationId}");
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel36;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            ApplyCandidateAndroidIcon();
            ApplyCandidateAndroidSplash();
            PlayerSettings.Android.useCustomKeystore = false;
        }

        private static string ResolveAndroidApplicationId()
        {
            var configured = Environment.GetEnvironmentVariable(AndroidApplicationIdEnvironmentVariable);
            if (string.IsNullOrWhiteSpace(configured)) return DevelopmentApplicationId;

            configured = configured.Trim();
            if (!Regex.IsMatch(configured, @"^[A-Za-z][A-Za-z0-9_]*(\.[A-Za-z][A-Za-z0-9_]*)+$"))
            {
                throw new BuildFailedException(
                    $"{AndroidApplicationIdEnvironmentVariable} must be a valid Android application ID such as com.example.battleraja.");
            }

            return configured;
        }

        public static void BuildWebBazaarBastionDevelopment()
        {
            CreateBootstrapScene();
            CreateBazaarBastionScene();
            CreateTutorialArenaScene();
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.template = "PROJECT:BattleRaja";
            Build("Builds/M11/Web-BazaarBastion", BuildTarget.WebGL, BootstrapScenePath, TutorialArenaScenePath, BazaarBastionScenePath);
        }

        private static void Build(string outputPath, BuildTarget target, params string[] scenePaths)
        {
            Build(outputPath, target, BuildOptions.Development | BuildOptions.AllowDebugging, scenePaths);
        }

        private static void ApplyCandidateAndroidIcon()
        {
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(V1IconAssetPath);
            if (icon == null)
            {
                Debug.LogWarning($"BattleRaja V1 icon candidate was not found at {V1IconAssetPath}; retaining the current PlayerSettings icon.");
                return;
            }

            PlayerSettings.SetIcons(NamedBuildTarget.Android, new[] { icon }, IconKind.Any);
        }

        private static void ApplyCandidateAndroidSplash()
        {
            var splashLogo = AssetDatabase.LoadAssetAtPath<Sprite>(V1IconAssetPath);
            if (splashLogo == null)
            {
                throw new InvalidOperationException(
                    $"BattleRaja V1 splash logo could not be loaded as a Sprite at {V1IconAssetPath}. " +
                    "The release candidate must not fall back to Unity branding.");
            }

            // Keep the native splash branded and quiet while the first scene loads. The
            // icon is an original BattleRaja asset; no Unity logo is included in V1.
            PlayerSettings.SplashScreen.show = true;
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SplashScreen.backgroundColor = new Color32(7, 21, 30, 255);
            PlayerSettings.SplashScreen.background = null;
            PlayerSettings.SplashScreen.backgroundPortrait = null;
            PlayerSettings.SplashScreen.overlayOpacity = 0f;
            PlayerSettings.SplashScreen.logos = new[]
            {
                PlayerSettings.SplashScreenLogo.Create(2f, splashLogo)
            };
        }

        private static void Build(string outputPath, BuildTarget target, BuildOptions buildOptions, params string[] scenePaths)
        {
            if (scenePaths == null || scenePaths.Length == 0)
            {
                scenePaths = new[] { MovementLabScenePath };
            }

            var options = new BuildPlayerOptions
            {
                scenes = scenePaths,
                locationPathName = outputPath,
                target = target,
                options = buildOptions
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException($"{target} build failed: {report.summary.result}. See the Unity build log.");
            }

            Debug.Log($"{target} BattleRaja build succeeded: {report.summary.outputPath} ({report.summary.totalSize} bytes).");
        }

        private static GameObject CreateBlock(string name, Vector3 position, Vector3 scale, Material material, Transform parent)
        {
            var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = name;
            block.transform.SetParent(parent);
            block.transform.position = position;
            block.transform.localScale = scale;
            block.GetComponent<Renderer>().sharedMaterial = material;
            return block;
        }

        private static MatchPickup CreatePickup(string name, Vector3 position, Transform parent, Material material)
        {
            var pickupObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pickupObject.name = name;
            pickupObject.transform.SetParent(parent);
            pickupObject.transform.position = position;
            pickupObject.transform.localScale = new Vector3(0.45f, 0.18f, 0.45f);
            pickupObject.GetComponent<Renderer>().sharedMaterial = material;
            return pickupObject.AddComponent<MatchPickup>();
        }

        private static GadgetPickup CreateGadgetPickup(string name, string gadgetId, Vector3 position, Transform parent, Material material)
        {
            var pickupObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pickupObject.name = name;
            pickupObject.transform.SetParent(parent);
            pickupObject.transform.position = position;
            pickupObject.transform.localScale = new Vector3(0.55f, 0.22f, 0.55f);
            pickupObject.GetComponent<Renderer>().sharedMaterial = material;
            var pickup = pickupObject.AddComponent<GadgetPickup>();
            SetString(pickup, "gadgetId", gadgetId);
            var visuals = pickupObject.AddComponent<GadgetPickupVisuals>();
            ConfigureProductionGadgetArt(visuals);
            return pickup;
        }

        private static void CreateBotActor(
            int botIndex,
            Vector3 position,
            Transform parent,
            Material material,
            Material trailMaterial,
            MovementTuningAsset tuningAsset,
            FighterDefinitionAsset fighterAsset,
            CombatProjectilePool projectilePool,
            CombatDamageResolver damageResolver,
            ProjectileWeaponAsset bijliWeaponAsset,
            ProjectileWeaponAsset pehelWeapon,
            ProjectileWeaponAsset mayaWeapon)
        {
            var bot = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            var fighterId = fighterAsset.ToDomain().FighterId;
            var isPehel = fighterId.Equals(FighterDefinition.Pehel.FighterId);
            var isMaya = fighterId.Equals(FighterDefinition.Maya.FighterId);
            bot.name = $"Bot{(isPehel ? "Pehel" : isMaya ? "Maya" : "Bijli")}_{botIndex + 1}";
            bot.transform.SetParent(parent);
            bot.transform.position = position;
            bot.layer = 2;
            bot.GetComponent<Renderer>().sharedMaterial = material;
            UnityEngine.Object.DestroyImmediate(bot.GetComponent<Collider>());

            var controller = bot.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.42f;
            controller.center = Vector3.zero;
            controller.stepOffset = 0.25f;
            controller.slopeLimit = 45f;
            var agent = bot.AddComponent<MovementPlayerAgent>();
            var health = bot.AddComponent<CombatHealth>();
            var target = bot.AddComponent<CombatTarget>();
            var attack = bot.AddComponent<CombatAttackController>();
            var fighter = isPehel
                ? (MonoBehaviour)bot.AddComponent<PehelFighterController>()
                : isMaya ? bot.AddComponent<MayaFighterController>() : bot.AddComponent<BijliFighterController>();
            var perception = bot.AddComponent<BotPerceptionSensor>();
            var brain = bot.AddComponent<BotBrain>();
            var gadget = bot.AddComponent<GadgetUser>();
            var presentation = bot.AddComponent<FighterPresentation>();
            ConfigureProductionArt(presentation);
            bot.AddComponent<BotDebugOverlay>();
            var trail = bot.AddComponent<TrailRenderer>();
            trail.time = 0.24f;
            trail.startWidth = 0.22f;
            trail.endWidth = 0.02f;
            trail.sharedMaterial = trailMaterial;

            var actorId = 10 + botIndex;
            SetInt(agent, "actorId", actorId);
            SetObjectReference(agent, "tuningAsset", tuningAsset);
            SetObjectReference(agent, "fighterController", fighter);
            SetInt(health, "maxHealth", fighterAsset.ToDomain().MaxHealth);
            SetInt(target, "entityId", actorId);
            SetEnum(target, "faction", CombatFaction.Enemy);
            SetObjectReference(target, "health", health);
            SetInt(attack, "actorId", actorId);
            SetEnum(attack, "faction", CombatFaction.Enemy);
            SetObjectReference(attack, "fighterDefinition", fighterAsset);
            SetObjectReference(attack, "projectilePool", projectilePool);
            SetObjectReference(fighter, "fighterDefinition", fighterAsset);
            SetObjectReference(fighter, "movementAgent", agent);
            if (fighter is BijliFighterController)
            {
                SetObjectReference(fighter, "characterController", controller);
                SetObjectReference(fighter, "dashTrail", trail);
                SetInt(fighter, "dashCollisionMask", 1);
            }
            else if (fighter is PehelFighterController)
            {
                SetObjectReference(fighter, "inputAdapter", null);
                SetObjectReference(fighter, "characterController", controller);
                SetObjectReference(fighter, "damageResolver", damageResolver);
                SetInt(fighter, "chargeCollisionMask", 1);
            }
            else if (fighter is MayaFighterController)
            {
                SetObjectReference(fighter, "inputAdapter", null);
                SetObjectReference(fighter, "decoyMaterial", trailMaterial);
            }
            SetInt(perception, "actorId", actorId);
            SetObjectReference(perception, "health", health);
            SetObjectReference(perception, "selfTarget", target);
            SetObjectReference(perception, "weaponAsset", fighterAsset.ToDomain().FighterId.Equals(FighterDefinition.Pehel.FighterId)
                ? pehelWeapon
                : fighterAsset.ToDomain().FighterId.Equals(FighterDefinition.Maya.FighterId) ? mayaWeapon : bijliWeaponAsset);
            SetInt(brain, "seed", 100 + botIndex);
            SetObjectReference(brain, "fighterController", fighter);
            SetFloat(brain, "aimNoise", 0.05f);
            SetFloat(brain, "attackCadenceMultiplier", 15f);
            SetFloat(brain, "stuckTimeoutSeconds", 0.7f);
            SetObjectReference(
                brain,
                "weaponAsset",
                isPehel ? pehelWeapon : isMaya ? mayaWeapon : bijliWeaponAsset);
            SetObjectReference(gadget, "movementAgent", agent);
            SetObjectReference(gadget, "combatTarget", target);
            SetObjectReference(gadget, "health", health);
            SetObjectReference(gadget, "damageResolver", damageResolver);
            SetBool(gadget, "botControlled", true);
        }

        private static void ConfigureProductionBot(
            BotBrain brain,
            FighterDefinitionAsset fighterAsset,
            Material fighterMaterial,
            Material decoyMaterial,
            CombatDamageResolver damageResolver,
            CombatProjectilePool projectilePool,
            ProjectileWeaponAsset bijliWeapon,
            ProjectileWeaponAsset pehelWeapon,
            ProjectileWeaponAsset mayaWeapon)
        {
            var bot = brain.gameObject;
            var oldFighters = bot.GetComponents<MonoBehaviour>()
                .Where(component => component is IFighterAbilityController)
                .ToArray();
            for (var i = 0; i < oldFighters.Length; i++)
            {
                if (oldFighters[i] != null) UnityEngine.Object.DestroyImmediate(oldFighters[i]);
            }

            var domain = fighterAsset.ToDomain();
            var fighter = domain.FighterId.Equals(FighterDefinition.Pehel.FighterId)
                ? (MonoBehaviour)bot.AddComponent<PehelFighterController>()
                : domain.FighterId.Equals(FighterDefinition.Maya.FighterId)
                    ? bot.AddComponent<MayaFighterController>()
                    : bot.AddComponent<BijliFighterController>();
            var agent = bot.GetComponent<MovementPlayerAgent>();
            var attack = bot.GetComponent<CombatAttackController>();
            var target = bot.GetComponent<CombatTarget>();
            var health = bot.GetComponent<CombatHealth>();
            var controller = bot.GetComponent<CharacterController>();
            var renderer = bot.GetComponent<Renderer>();
            var perception = bot.GetComponent<BotPerceptionSensor>();
            var gadget = bot.GetComponent<GadgetUser>();
            var presentation = bot.GetComponent<FighterPresentation>();
            if (renderer != null) renderer.sharedMaterial = fighterMaterial;
            ConfigureProductionArt(presentation);

            SetObjectReference(agent, "fighterController", fighter);
            SetObjectReference(attack, "fighterDefinition", fighterAsset);
            SetObjectReference(attack, "projectilePool", projectilePool);
            SetObjectReference(fighter, "fighterDefinition", fighterAsset);
            SetObjectReference(fighter, "movementAgent", agent);
            SetObjectReference(brain, "fighterController", fighter);
            SetFloat(brain, "aimNoise", 0.05f);
            SetFloat(brain, "attackCadenceMultiplier", 15f);
            SetFloat(brain, "stuckTimeoutSeconds", 0.7f);
            SetObjectReference(
                brain,
                "weaponAsset",
                domain.FighterId.Equals(FighterDefinition.Pehel.FighterId)
                    ? pehelWeapon
                    : domain.FighterId.Equals(FighterDefinition.Maya.FighterId) ? mayaWeapon : bijliWeapon);
            SetObjectReference(
                perception,
                "weaponAsset",
                domain.FighterId.Equals(FighterDefinition.Pehel.FighterId)
                    ? pehelWeapon
                    : domain.FighterId.Equals(FighterDefinition.Maya.FighterId) ? mayaWeapon : bijliWeapon);
            SetInt(health, "maxHealth", domain.MaxHealth);

            if (fighter is PehelFighterController)
            {
                SetObjectReference(fighter, "inputAdapter", null);
                SetObjectReference(fighter, "characterController", controller);
                SetObjectReference(fighter, "damageResolver", damageResolver);
                SetInt(fighter, "chargeCollisionMask", 1);
            }
            else if (fighter is MayaFighterController)
            {
                SetObjectReference(fighter, "inputAdapter", null);
                SetObjectReference(fighter, "decoyMaterial", decoyMaterial);
            }
            else if (fighter is BijliFighterController)
            {
                SetObjectReference(fighter, "inputAdapter", null);
                SetObjectReference(fighter, "characterController", controller);
                SetObjectReference(fighter, "dashTrail", bot.GetComponent<TrailRenderer>());
                SetInt(fighter, "dashCollisionMask", 1);
            }

            SetObjectReference(target, "health", health);
            SetObjectReference(gadget, "movementAgent", agent);
            SetObjectReference(gadget, "combatTarget", target);
            SetObjectReference(gadget, "health", health);
            SetObjectReference(gadget, "damageResolver", damageResolver);
            SetBool(gadget, "botControlled", true);
        }

        private static void ConfigureBastionBotIdentity(BotBrain brain, int actorId, CombatFaction faction)
        {
            if (brain == null) throw new BuildFailedException("A Bastion Crown bot slot is missing its BotBrain.");
            var bot = brain.gameObject;
            var agent = bot.GetComponent<MovementPlayerAgent>();
            var target = bot.GetComponent<CombatTarget>();
            var attack = bot.GetComponent<CombatAttackController>();
            var perception = bot.GetComponent<BotPerceptionSensor>();
            if (agent == null || target == null || attack == null || perception == null)
            {
                throw new BuildFailedException($"Bastion Crown bot slot {bot.name} is missing an authority identity component.");
            }

            SetInt(agent, "actorId", actorId);
            SetInt(target, "entityId", actorId);
            SetEnum(target, "faction", faction);
            SetInt(attack, "actorId", actorId);
            SetEnum(attack, "faction", faction);
            SetInt(perception, "actorId", actorId);

            // Pehel and Maya carry a faction field for their ability-side target
            // filtering. Bijli's dash is resolved through the authority attack
            // bridge and has no separate faction field.
            var pehel = bot.GetComponent<PehelFighterController>();
            if (pehel != null) SetEnum(pehel, "faction", faction);
            var maya = bot.GetComponent<MayaFighterController>();
            if (maya != null) SetEnum(maya, "faction", faction);
        }

        private static void ApplyBazaarPalette(Transform arena, Material floor, Material wall, Material stall)
        {
            var renderers = arena.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                var name = renderers[i].gameObject.name;
                if (name.IndexOf("ArenaFloor", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    renderers[i].sharedMaterial = floor;
                }
                else if (name.IndexOf("Boundary", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         name.IndexOf("NarrowLane", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         name.IndexOf("CornerWall", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    renderers[i].sharedMaterial = wall;
                }
                else if (name.IndexOf("Obstacle", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    renderers[i].sharedMaterial = stall;
                }
            }
        }

        private static void CreateBazaarDecor(Transform arena, Material wall, Material stall)
        {
            var decor = new GameObject("BazaarArchitecture");
            decor.transform.SetParent(arena, false);
            CreateBlock("BazaarArchLeft", new Vector3(-6f, 1.8f, 8.8f), new Vector3(0.8f, 3.6f, 0.8f), wall, decor.transform);
            CreateBlock("BazaarArchRight", new Vector3(6f, 1.8f, 8.8f), new Vector3(0.8f, 3.6f, 0.8f), wall, decor.transform);
            CreateBlock("BazaarArchLintel", new Vector3(0f, 3.4f, 8.8f), new Vector3(12.8f, 0.8f, 0.8f), wall, decor.transform);
            CreateMarketStall("BazaarStallWest", new Vector3(-10f, 0.8f, 1.5f), stall, decor.transform);
            CreateMarketStall("BazaarStallEast", new Vector3(10f, 0.8f, 1.5f), stall, decor.transform);
            CreateBlock("BazaarPlazaMarker", new Vector3(0f, 0.08f, 0f), new Vector3(5f, 0.16f, 5f), wall, decor.transform);
        }

        private static void CreateMarketStall(string name, Vector3 position, Material material, Transform parent)
        {
            CreateBlock(name + "Counter", position, new Vector3(2.4f, 1.6f, 1.2f), material, parent);
            CreateBlock(name + "Roof", position + new Vector3(0f, 1.8f, 0f), new Vector3(2.8f, 0.25f, 1.6f), material, parent);
        }

        private static void EnsureBazaarArchitecturePrefab(Transform arena)
        {
            var architecture = arena.Find("BazaarArchitecture");
            if (architecture == null)
            {
                throw new BuildFailedException("Bazaar architecture root is missing.");
            }

            Directory.CreateDirectory(BazaarPrefabFolder);
            if (PrefabUtility.IsPartOfPrefabInstance(architecture.gameObject)) return;

            var prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(
                architecture.gameObject,
                BazaarArchitecturePrefabPath,
                InteractionMode.AutomatedAction);
            if (prefab == null)
            {
                throw new BuildFailedException("Bazaar architecture prefab could not be created.");
            }
        }

        private static Canvas CreateTouchCanvas(out VirtualStick movementStick, out VirtualStick aimStick, out AttackButton attackButton, out AbilityButton abilityButton, out GadgetUseButton gadgetButton)
        {
            var canvasObject = new GameObject("TouchControls", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            var safeAreaObject = new GameObject("SafeArea", typeof(RectTransform), typeof(SafeAreaPanel));
            safeAreaObject.transform.SetParent(canvasObject.transform, false);
            var safeArea = safeAreaObject.GetComponent<RectTransform>();
            safeArea.anchorMin = Vector2.zero;
            safeArea.anchorMax = Vector2.one;
            safeArea.offsetMin = Vector2.zero;
            safeArea.offsetMax = Vector2.zero;

            movementStick = CreateStick("MovementStick", safeArea, new Vector2(0.17f, 0.2f), new Color(0.25f, 0.70f, 1f, 0.18f));
            aimStick = CreateStick("AimStick", safeArea, new Vector2(0.83f, 0.2f), new Color(1f, 0.64f, 0.22f, 0.18f));
            attackButton = CreateAttackButton(safeArea);
            abilityButton = CreateAbilityButton(safeArea);
            gadgetButton = CreateGadgetButton(safeArea);
            return canvas;
        }

        private static GadgetUseButton CreateGadgetButton(Transform parent)
        {
            var buttonObject = new GameObject("GadgetButton", typeof(RectTransform), typeof(BattleRajaTouchSurface), typeof(GadgetUseButton));
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.67f, 0.49f);
            rect.anchorMax = new Vector2(0.67f, 0.49f);
            rect.sizeDelta = new Vector2(120f, 120f);
            buttonObject.GetComponent<BattleRajaTouchSurface>().Configure(new Color(0.72f, 0.32f, 0.95f, 0.30f), true);
            return buttonObject.GetComponent<GadgetUseButton>();
        }

        private static void CreateGadgetHud(Canvas canvas, GadgetUser user)
        {
            var hudObject = new GameObject("GadgetHud", typeof(RectTransform), typeof(GadgetHud));
            hudObject.transform.SetParent(canvas.transform, false);
            var rect = hudObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.02f, 0.84f);
            rect.anchorMax = new Vector2(0.40f, 0.93f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var textObject = new GameObject("GadgetStatus", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(hudObject.transform, false);
            var textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            var text = textObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 20;
            text.color = Color.white;
            SetObjectReference(hudObject.GetComponent<GadgetHud>(), "user", user);
            SetObjectReference(hudObject.GetComponent<GadgetHud>(), "statusText", text);
        }

        private static AttackButton CreateAttackButton(Transform parent)
        {
            var buttonObject = new GameObject("AttackButton", typeof(RectTransform), typeof(BattleRajaTouchSurface), typeof(AttackButton));
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.91f, 0.49f);
            rect.anchorMax = new Vector2(0.91f, 0.49f);
            rect.sizeDelta = new Vector2(170f, 170f);
            buttonObject.GetComponent<BattleRajaTouchSurface>().Configure(new Color(1f, 0.36f, 0.18f, 0.28f), true);
            return buttonObject.GetComponent<AttackButton>();
        }

        private static AbilityButton CreateAbilityButton(Transform parent)
        {
            var buttonObject = new GameObject("AbilityButton", typeof(RectTransform), typeof(BattleRajaTouchSurface), typeof(AbilityButton));
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.79f, 0.49f);
            rect.anchorMax = new Vector2(0.79f, 0.49f);
            rect.sizeDelta = new Vector2(140f, 140f);
            buttonObject.GetComponent<BattleRajaTouchSurface>().Configure(new Color(0.36f, 0.78f, 1f, 0.30f), true);
            return buttonObject.GetComponent<AbilityButton>();
        }

        private static BijliHud CreateHud(Canvas canvas, out Text statusText)
        {
            var hudObject = new GameObject("BijliHud", typeof(RectTransform), typeof(BijliHud));
            hudObject.transform.SetParent(canvas.transform, false);
            var rect = hudObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.02f, 0.94f);
            rect.anchorMax = new Vector2(0.40f, 0.99f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var textObject = new GameObject("Status", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(hudObject.transform, false);
            var textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            statusText = textObject.GetComponent<Text>();
            statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            statusText.fontSize = 24;
            statusText.alignment = TextAnchor.UpperLeft;
            statusText.color = Color.white;
            return hudObject.GetComponent<BijliHud>();
        }

        private static VirtualStick CreateStick(string name, Transform parent, Vector2 anchor, Color color)
        {
            var stickObject = new GameObject(name, typeof(RectTransform), typeof(BattleRajaTouchSurface), typeof(VirtualStick));
            stickObject.transform.SetParent(parent, false);
            var rect = stickObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(220f, 220f);
            stickObject.GetComponent<BattleRajaTouchSurface>().Configure(color, true);
            TouchControlLabel.Ensure(stickObject.transform, name == "MovementStick" ? "MOVE" : "AIM");

            var knobObject = new GameObject("Knob", typeof(RectTransform), typeof(BattleRajaTouchSurface));
            knobObject.transform.SetParent(stickObject.transform, false);
            var knobRect = knobObject.GetComponent<RectTransform>();
            knobRect.anchorMin = new Vector2(0.5f, 0.5f);
            knobRect.anchorMax = new Vector2(0.5f, 0.5f);
            knobRect.sizeDelta = new Vector2(94f, 94f);
            knobObject.GetComponent<BattleRajaTouchSurface>().Configure(new Color(color.r, color.g, color.b, 0.72f), false);

            var stick = stickObject.GetComponent<VirtualStick>();
            SetObjectReference(stick, "knob", knobRect);
            return stick;
        }

        private static MovementTuningAsset EnsureTuningAsset()
        {
            var asset = AssetDatabase.LoadAssetAtPath<MovementTuningAsset>(TuningAssetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<MovementTuningAsset>();
                AssetDatabase.CreateAsset(asset, TuningAssetPath);
            }

            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<MovementTuningAsset>(TuningAssetPath);
        }

        private static ProjectileWeaponAsset EnsureWeaponAsset()
        {
            Directory.CreateDirectory("Assets/BattleRaja/Content/Weapons");
            var asset = AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(WeaponAssetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ProjectileWeaponAsset>();
                AssetDatabase.CreateAsset(asset, WeaponAssetPath);
            }

            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(WeaponAssetPath);
        }

        private static ProjectileWeaponAsset EnsureBijliWeaponAsset()
        {
            Directory.CreateDirectory("Assets/BattleRaja/Content/Weapons");
            var asset = AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(BijliWeaponAssetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ProjectileWeaponAsset>();
                AssetDatabase.CreateAsset(asset, BijliWeaponAssetPath);
            }

            var definition = ProjectileWeaponDefinition.BijliElectricBolt;
            SetInt(asset, "damage", definition.Damage);
            SetFloat(asset, "fireIntervalSeconds", definition.FireIntervalSeconds);
            SetFloat(asset, "projectileSpeed", definition.ProjectileSpeed);
            SetFloat(asset, "maxRange", definition.MaxRange);
            SetFloat(asset, "lifetimeSeconds", definition.LifetimeSeconds);
            SetFloat(asset, "radius", definition.Radius);
            SetInt(asset, "collisionLayers", definition.CollisionLayerMask);
            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(BijliWeaponAssetPath);
        }

        private static FighterDefinitionAsset EnsureFighterAsset()
        {
            Directory.CreateDirectory("Assets/BattleRaja/Content/Fighters");
            var asset = AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(FighterAssetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<FighterDefinitionAsset>();
                AssetDatabase.CreateAsset(asset, FighterAssetPath);
            }

            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(FighterAssetPath);
        }

        private static ProjectileWeaponAsset EnsureVariantWeaponAsset(string path, ProjectileWeaponDefinition definition)
        {
            Directory.CreateDirectory("Assets/BattleRaja/Content/Weapons");
            var asset = AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ProjectileWeaponAsset>();
                AssetDatabase.CreateAsset(asset, path);
            }

            SetInt(asset, "damage", definition.Damage);
            SetFloat(asset, "fireIntervalSeconds", definition.FireIntervalSeconds);
            SetFloat(asset, "projectileSpeed", definition.ProjectileSpeed);
            SetFloat(asset, "maxRange", definition.MaxRange);
            SetFloat(asset, "lifetimeSeconds", definition.LifetimeSeconds);
            SetFloat(asset, "radius", definition.Radius);
            SetInt(asset, "collisionLayers", definition.CollisionLayerMask);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            return AssetDatabase.LoadAssetAtPath<ProjectileWeaponAsset>(path);
        }

        private static FighterDefinitionAsset EnsureFighterVariantAsset(
            string path,
            string fighterId,
            string displayName,
            FighterDefinition definition,
            ProjectileWeaponAsset weapon)
        {
            Directory.CreateDirectory("Assets/BattleRaja/Content/Fighters");
            var asset = AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<FighterDefinitionAsset>();
                AssetDatabase.CreateAsset(asset, path);
            }

            var tuning = AssetDatabase.LoadAssetAtPath<MovementTuningAsset>(TuningAssetPath);
            SetString(asset, "fighterId", fighterId);
            SetString(asset, "displayName", displayName);
            SetInt(asset, "maxHealth", definition.MaxHealth);
            SetObjectReference(asset, "movementTuning", tuning);
            SetObjectReference(asset, "basicAttack", weapon);
            SetString(asset, "abilityId", definition.Ability.AbilityId.Value);
            SetFloat(asset, "abilityCooldownSeconds", definition.Ability.CooldownSeconds);
            SetFloat(asset, "dashDistance", definition.Ability.Distance);
            SetFloat(asset, "startupSeconds", definition.Ability.StartupSeconds);
            SetFloat(asset, "activeSeconds", definition.Ability.ActiveSeconds);
            SetFloat(asset, "recoverySeconds", definition.Ability.RecoverySeconds);
            SetFloat(asset, "collisionRadius", definition.Ability.CollisionRadius);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            return AssetDatabase.LoadAssetAtPath<FighterDefinitionAsset>(path);
        }

        private static GadgetDefinitionAsset[] EnsureGadgetAssets()
        {
            Directory.CreateDirectory(GadgetAssetFolder);
            var definitions = new[] { GadgetDefinition.UmbrellaGuard, GadgetDefinition.DholBurst, GadgetDefinition.TiffinStation };
            var assets = new GadgetDefinitionAsset[definitions.Length];
            for (var i = 0; i < definitions.Length; i++)
            {
                var path = GadgetAssetFolder + "/M6-" + definitions[i].Kind + ".asset";
                var asset = AssetDatabase.LoadAssetAtPath<GadgetDefinitionAsset>(path);
                if (asset == null)
                {
                    asset = ScriptableObject.CreateInstance<GadgetDefinitionAsset>();
                    AssetDatabase.CreateAsset(asset, path);
                }

                SetString(asset, "gadgetId", definitions[i].GadgetId.Value);
                SetEnum(asset, "kind", definitions[i].Kind);
                SetFloat(asset, "cooldownSeconds", definitions[i].CooldownSeconds);
                SetFloat(asset, "durationSeconds", definitions[i].DurationSeconds);
                SetFloat(asset, "radius", definitions[i].Radius);
                SetInt(asset, "magnitude", definitions[i].Magnitude);
                SetInt(asset, "stationHealth", definitions[i].StationHealth);
                SetFloat(asset, "placementRadius", definitions[i].PlacementRadius);
                assets[i] = asset;
            }

            AssetDatabase.SaveAssets();
            return assets;
        }

        private static InputActionAsset EnsureInputAsset()
        {
            var asset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);
            if (asset != null)
            {
                var existingMap = asset.FindActionMap("Player", throwIfNotFound: false);
                if (existingMap != null && existingMap.FindAction("Attack", throwIfNotFound: false) == null)
                {
                    var existingAttack = existingMap.AddAction("Attack", InputActionType.Button);
                    existingAttack.AddBinding("<Mouse>/leftButton", groups: "KeyboardMouse");
                    existingAttack.AddBinding("<Gamepad>/buttonSouth", groups: "Gamepad");
                    File.WriteAllText(InputAssetPath, asset.ToJson());
                    AssetDatabase.ImportAsset(InputAssetPath, ImportAssetOptions.ForceUpdate);
                }

                if (existingMap != null && existingMap.FindAction("Ability", throwIfNotFound: false) == null)
                {
                    var existingAbility = existingMap.AddAction("Ability", InputActionType.Button);
                    existingAbility.AddBinding("<Keyboard>/space", groups: "KeyboardMouse");
                    existingAbility.AddBinding("<Gamepad>/buttonEast", groups: "Gamepad");
                    File.WriteAllText(InputAssetPath, asset.ToJson());
                    AssetDatabase.ImportAsset(InputAssetPath, ImportAssetOptions.ForceUpdate);
                }

                return asset;
            }

            asset = ScriptableObject.CreateInstance<InputActionAsset>();
            var map = asset.AddActionMap("Player");
            var move = map.AddAction("Move", InputActionType.Value, expectedControlLayout: "Vector2");
            move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
            move.AddBinding("<Gamepad>/leftStick");

            var mouse = map.AddAction("MousePosition", InputActionType.Value, expectedControlLayout: "Vector2");
            mouse.AddBinding("<Mouse>/position");
            var aimStick = map.AddAction("AimStick", InputActionType.Value, expectedControlLayout: "Vector2");
            aimStick.AddBinding("<Gamepad>/rightStick");
            var attack = map.AddAction("Attack", InputActionType.Button);
            attack.AddBinding("<Mouse>/leftButton", groups: "KeyboardMouse");
            attack.AddBinding("<Gamepad>/buttonSouth", groups: "Gamepad");
            var ability = map.AddAction("Ability", InputActionType.Button);
            ability.AddBinding("<Keyboard>/space", groups: "KeyboardMouse");
            ability.AddBinding("<Gamepad>/buttonEast", groups: "Gamepad");

            asset.AddControlScheme("KeyboardMouse")
                .WithRequiredDevice("<Keyboard>")
                .WithRequiredDevice("<Mouse>");
            asset.AddControlScheme("Gamepad").WithRequiredDevice("<Gamepad>");
            File.WriteAllText(InputAssetPath, asset.ToJson());
            AssetDatabase.ImportAsset(InputAssetPath, ImportAssetOptions.ForceUpdate);
            return AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);
        }

        private static Material EnsureMaterial(string name, Color color)
        {
            var path = MovementAssetFolder + "/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Universal Render Pipeline/Lit"))
                {
                    name = name,
                    color = color
                };
                AssetDatabase.CreateAsset(material, path);
            }

            return material;
        }

        private static void SetObjectReference(UnityEngine.Object target, string propertyName, UnityEngine.Object value)
        {
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException($"Serialized property not found: {target.name}.{propertyName}");
            }

            property.objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private static void SetInt(UnityEngine.Object target, string propertyName, int value)
        {
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException($"Serialized property not found: {target.name}.{propertyName}");
            }

            property.intValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetFloat(UnityEngine.Object target, string propertyName, float value)
        {
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException($"Serialized property not found: {target.name}.{propertyName}");
            }

            property.floatValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetVector3(UnityEngine.Object target, string propertyName, Vector3 value)
        {
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException($"Serialized property not found: {target.name}.{propertyName}");
            }

            property.vector3Value = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetString(UnityEngine.Object target, string propertyName, string value)
        {
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyName);
            if (property == null) throw new InvalidOperationException($"Serialized property not found: {target.name}.{propertyName}");
            property.stringValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetBool(UnityEngine.Object target, string propertyName, bool value)
        {
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyName);
            if (property == null) throw new InvalidOperationException($"Serialized property not found: {target.name}.{propertyName}");
            property.boolValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetEnum(UnityEngine.Object target, string propertyName, System.Enum value)
        {
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException($"Serialized property not found: {target.name}.{propertyName}");
            }

            var names = property.enumNames;
            var name = value.ToString();
            var index = System.Array.IndexOf(names, name);
            if (index < 0) throw new InvalidOperationException($"Enum value not found: {target.name}.{propertyName}={name}");
            property.enumValueIndex = index;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void RequireText(string content, string expected, string source)
        {
            if (content.IndexOf(expected, StringComparison.Ordinal) < 0)
            {
                throw new BuildFailedException($"{source} is missing expected text: {expected}");
            }
        }

        private static void EnsureUrpAsset()
        {
            const string pipelinePath = "Assets/BattleRaja/Content/BattleRaja-M0-URP.asset";
            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(pipelinePath);
            if (pipeline == null)
            {
                pipeline = UniversalRenderPipelineAsset.Create();
                AssetDatabase.CreateAsset(pipeline, pipelinePath);
                var rendererData = pipeline.rendererDataList.Length > 0 ? pipeline.rendererDataList[0] : null;
                if (rendererData != null && string.IsNullOrEmpty(AssetDatabase.GetAssetPath(rendererData)))
                {
                    AssetDatabase.AddObjectToAsset(rendererData, pipeline);
                }
                AssetDatabase.SaveAssets();
            }

            GraphicsSettings.defaultRenderPipeline = pipeline;
            EditorUtility.SetDirty(pipeline);
        }
    }
}
