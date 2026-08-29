using UnityEngine;
using UnityEngine.Rendering;

namespace BattleRaja.Presentation.Visuals
{
    /// <summary>
    /// Instantiates the saved Bazaar Bastion presentation prefab. The prefab is a
    /// collider-free, textured mesh kit; authored collision/navigation remains in the
    /// scene and all runtime state stays outside the environment asset.
    /// </summary>
    public sealed class BazaarBastionVisuals : MonoBehaviour
    {
        [SerializeField] private bool enabledForBuilds = true;
        [SerializeField, Range(0, 1)] private int decorationQuality = 1;
        [SerializeField] private GameObject environmentPrefab;
        [SerializeField] private bool allowRuntimeFallback;

        private GameObject _environmentInstance;
        private GameObject _lightingObject;

        public GameObject EnvironmentPrefab => environmentPrefab;
        public bool UsesSavedEnvironment => environmentPrefab != null;

        private void Awake()
        {
            if (!enabledForBuilds) return;
            EnsureLighting();

            if (transform.Find("V1BastionVisuals") != null) return;
            if (environmentPrefab != null)
            {
                _environmentInstance = Instantiate(environmentPrefab, transform, false);
                _environmentInstance.name = "V1BastionVisuals";
                ApplyDecorationQuality(_environmentInstance);
                return;
            }

            // A missing saved prefab is a build/configuration error. The opt-in fallback
            // keeps old development fixtures legible while making the omission visible;
            // production scenes leave this disabled and never create placeholder art.
            Debug.LogError("Bazaar Bastion saved environment prefab is not assigned.");
            if (allowRuntimeFallback) BuildRuntimeFallback();
        }

        private void OnDestroy()
        {
            if (_environmentInstance != null) Destroy(_environmentInstance);
            if (_lightingObject != null) Destroy(_lightingObject);
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

        private void ApplyDecorationQuality(GameObject instance)
        {
            if (instance == null || decorationQuality > 0) return;
            var backdrop = instance.transform.Find("BastionBackdrop");
            if (backdrop != null) backdrop.gameObject.SetActive(false);
        }

        private void BuildRuntimeFallback()
        {
            var root = new GameObject("V1BastionVisuals");
            root.transform.SetParent(transform, false);
            var material = CreateFallbackMaterial(new Color(0.08f, 0.42f, 0.45f, 1f));
            var ground = new GameObject("GroundMosaic", typeof(MeshFilter), typeof(MeshRenderer));
            ground.transform.SetParent(root.transform, false);
            ground.transform.localPosition = new Vector3(0f, 0.006f, 0f);
            ground.GetComponent<MeshFilter>().sharedMesh = PresentationMeshFactory.Box("EmergencyBazaarGround");
            ground.transform.localScale = new Vector3(25.6f, 0.02f, 25.6f);
            ground.GetComponent<MeshRenderer>().sharedMaterial = material;
            _environmentInstance = root;
        }

        private static Material CreateFallbackMaterial(Color color)
        {
            var material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard")) { color = color };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            return material;
        }
    }
}
