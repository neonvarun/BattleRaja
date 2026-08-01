using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace BattleRaja.Editor
{
    public static class BuildEntrypoints
    {
        private const string ExpectedUnityVersion = "6000.5.6f1";
        private const string BootstrapScenePath = "Assets/BattleRaja/Scenes/Bootstrap/Bootstrap.unity";
        private const string DevelopmentApplicationId = "com.example.battleraja.m0";

        public static void CreateBootstrapScene()
        {
            EnsureUrpAsset();

            if (!File.Exists(BootstrapScenePath))
            {
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                var cameraObject = new GameObject("BootstrapCamera");
                var camera = cameraObject.AddComponent<Camera>();
                camera.transform.position = new Vector3(0f, 8f, -8f);
                camera.transform.rotation = Quaternion.Euler(35f, 0f, 0f);
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.08f, 0.10f, 0.14f, 1f);

                var lightObject = new GameObject("BootstrapLight");
                var light = lightObject.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1f;
                light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

                EditorSceneManager.SaveScene(scene, BootstrapScenePath);
            }

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(BootstrapScenePath, true)
            };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
            RequireText(lockFile, "com.unity.render-pipelines.universal", "Packages/packages-lock.json");
            RequireText(lockFile, "\"version\": \"17.5.0\"", "Packages/packages-lock.json URP version");
            RequireText(lockFile, "\"version\": \"1.20.0\"", "Packages/packages-lock.json Input System version");
            RequireText(lockFile, "\"version\": \"1.7.0\"", "Packages/packages-lock.json Test Framework version");

            if (manifest.IndexOf("photon", StringComparison.OrdinalIgnoreCase) >= 0 ||
                manifest.IndexOf("playfab", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new BuildFailedException("Photon and PlayFab are prohibited in Milestone 0.");
            }

            if (!File.Exists(BootstrapScenePath))
            {
                throw new BuildFailedException($"Bootstrap scene is missing: {BootstrapScenePath}");
            }

            if (GraphicsSettings.defaultRenderPipeline == null)
            {
                throw new BuildFailedException("The URP render pipeline is not assigned in GraphicsSettings.");
            }

            Debug.Log("BattleRaja Milestone 0 validation passed.");
        }

        public static void BuildAndroidDevelopment()
        {
            CreateBootstrapScene();
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, DevelopmentApplicationId);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel36;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

            var outputPath = "Builds/M0/Android/BattleRaja-M0.apk";
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            Build(outputPath, BuildTarget.Android);
        }

        public static void BuildWebDevelopment()
        {
            CreateBootstrapScene();
            ValidateProject();

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;

            var outputPath = "Builds/M0/Web";
            Directory.CreateDirectory(outputPath);
            Build(outputPath, BuildTarget.WebGL);
        }

        private static void Build(string outputPath, BuildTarget target)
        {
            var options = new BuildPlayerOptions
            {
                scenes = new[] { BootstrapScenePath },
                locationPathName = outputPath,
                target = target,
                options = BuildOptions.Development | BuildOptions.AllowDebugging
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException($"{target} build failed: {report.summary.result}. See the Unity build log.");
            }

            Debug.Log($"{target} development build succeeded: {report.summary.outputPath} ({report.summary.totalSize} bytes).");
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
