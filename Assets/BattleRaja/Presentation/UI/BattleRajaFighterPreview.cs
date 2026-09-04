using BattleRaja.Presentation.Visuals;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Renders one of the saved production fighter prefabs into a small UI card.
    /// The preview owns only presentation objects; it never participates in the
    /// gameplay scene, collision, input or authority simulation.
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public sealed class BattleRajaFighterPreview : MonoBehaviour
    {
        private const int PreviewLayerFirst = 24;
        private const int PreviewLayerLast = 31;
        private const int DefaultRenderSize = 256;

        private static int _nextPreviewLayer = PreviewLayerFirst;

        [SerializeField] private int renderSize = DefaultRenderSize;
        [SerializeField] private float rotationSpeed = 8f;

        private RawImage _image;
        private int _previewLayer;
        private GameObject _stageRoot;
        private Transform _modelRoot;
        private GameObject _model;
        private Camera _camera;
        private Light _keyLight;
        private Light _fillLight;
        private RenderTexture _renderTexture;
        private GameObject _configuredPrefab;
        private Color _backgroundColor;
        private bool _selected = true;
        private bool _hasModel;

        public bool HasModel => _hasModel;

        private void Awake()
        {
            _image = GetComponent<RawImage>();
            _image.raycastTarget = false;
            EnsureStage();
        }

        private void OnEnable()
        {
            if (_stageRoot != null) _stageRoot.SetActive(_hasModel);
            if (_camera != null) _camera.enabled = _hasModel;
        }

        private void OnDisable()
        {
            if (_camera != null) _camera.enabled = false;
            if (_stageRoot != null) _stageRoot.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_stageRoot != null) Destroy(_stageRoot);
            if (_renderTexture != null)
            {
                _renderTexture.Release();
                Destroy(_renderTexture);
            }
        }

        private void Update()
        {
            if (!_hasModel || _modelRoot == null || !_modelRoot.gameObject.activeSelf) return;
            var speed = _selected ? rotationSpeed : rotationSpeed * 0.35f;
            _modelRoot.Rotate(0f, speed * Time.deltaTime, 0f, Space.Self);
        }

        /// <summary>
        /// Binds a saved production prefab to this card. The prefab is cloned into an
        /// isolated render-only layer and is never added to the active gameplay scene.
        /// </summary>
        public void Configure(GameObject prefab, Color backgroundColor)
        {
            _configuredPrefab = prefab;
            _backgroundColor = backgroundColor;
            EnsureStage();
            RebuildModel();
        }

        public void SetSelected(bool selected)
        {
            _selected = selected;
            if (_image != null)
            {
                _image.color = selected
                    ? Color.white
                    : new Color(0.76f, 0.82f, 0.86f, 1f);
            }
        }

        private void EnsureStage()
        {
            if (_stageRoot != null) return;

            _previewLayer = AllocatePreviewLayer();

            _stageRoot = new GameObject("FighterPreviewStage");
            _stageRoot.hideFlags = HideFlags.DontSave;
            _stageRoot.SetActive(false);

            _modelRoot = new GameObject("ModelRoot").transform;
            _modelRoot.SetParent(_stageRoot.transform, false);
            _modelRoot.gameObject.layer = _previewLayer;

            var cameraObject = new GameObject("PreviewCamera");
            cameraObject.transform.SetParent(_stageRoot.transform, false);
            cameraObject.layer = _previewLayer;
            _camera = cameraObject.AddComponent<Camera>();
            _camera.enabled = false;
            _camera.clearFlags = CameraClearFlags.SolidColor;
            _camera.backgroundColor = _backgroundColor;
            // A square portrait card benefits from a stable orthographic crop: it
            // keeps each Raja large and readable instead of shrinking the model to
            // accommodate a perspective distance chosen for the live arena.
            _camera.orthographic = true;
            _camera.orthographicSize = 1.35f;
            _camera.fieldOfView = 25f;
            _camera.nearClipPlane = 0.05f;
            _camera.farClipPlane = 30f;
            _camera.cullingMask = 1 << _previewLayer;
            _camera.useOcclusionCulling = false;
            _camera.allowHDR = false;
            _camera.allowMSAA = false;

            _keyLight = CreateLight("PreviewKeyLight", new Color(1f, 0.86f, 0.68f, 1f), 1.55f,
                Quaternion.Euler(38f, -32f, 0f));
            _fillLight = CreateLight("PreviewFillLight", new Color(0.35f, 0.72f, 1f, 1f), 0.55f,
                Quaternion.Euler(24f, 148f, 0f));

            var size = Mathf.Clamp(renderSize, 128, 512);
            _renderTexture = new RenderTexture(size, size, 16, RenderTextureFormat.ARGB32)
            {
                name = "FighterPreviewRenderTexture",
                filterMode = FilterMode.Bilinear,
                useMipMap = false,
                autoGenerateMips = false,
                antiAliasing = 1
            };
            _renderTexture.Create();
            _camera.targetTexture = _renderTexture;
            if (_image != null) _image.texture = _renderTexture;
        }

        private Light CreateLight(string name, Color color, float intensity, Quaternion rotation)
        {
            var lightObject = new GameObject(name);
            lightObject.transform.SetParent(_stageRoot.transform, false);
            lightObject.layer = _previewLayer;
            lightObject.transform.rotation = rotation;
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = color;
            light.intensity = intensity;
            light.shadows = LightShadows.None;
            light.cullingMask = 1 << _previewLayer;
            return light;
        }

        private void RebuildModel()
        {
            if (_model != null) Destroy(_model);
            _model = null;
            _hasModel = false;
            if (_stageRoot == null || _modelRoot == null || _configuredPrefab == null)
            {
                if (_image != null) _image.texture = null;
                return;
            }

            _camera.backgroundColor = _backgroundColor;
            _model = Instantiate(_configuredPrefab, _modelRoot, false);
            _model.name = _configuredPrefab.name + "Preview";
            SetLayerRecursively(_model, _previewLayer);
            _model.transform.localPosition = Vector3.zero;
            // The authored meshes face the camera's -Z presentation side. Keep the
            // card at a neutral yaw; the preview itself provides the gentle turn.
            _model.transform.localRotation = Quaternion.identity;
            _model.transform.localScale = Vector3.one * 1.16f;

            // Card previews are intentionally close-up. The saved prefabs also carry
            // a gameplay LODGroup whose far silhouette would collapse the authored
            // fighter into a tiny coloured capsule at this camera distance. Keep the
            // detailed identity mesh on this isolated presentation camera.
            foreach (var lodGroup in _model.GetComponentsInChildren<LODGroup>(true))
            {
                var lods = lodGroup.GetLODs();
                // The preview disables the gameplay LODGroup below and owns the
                // renderer visibility explicitly. Calling ForceLOD on a cloned
                // group that Unity has already disabled emits a runtime warning
                // on Android and adds no visual value here.
                if (lods != null && lods.Length > 0)
                {
                    for (var lodIndex = 0; lodIndex < lods.Length; lodIndex++)
                    {
                        var visible = lodIndex == 0;
                        foreach (var renderer in lods[lodIndex].renderers)
                        {
                            if (renderer != null) renderer.enabled = visible;
                        }
                    }
                }

                // This clone is a presentation portrait, not a live actor. Manual
                // renderer visibility avoids the LODGroup selecting the far capsule
                // before the isolated camera has rendered its first frame.
                lodGroup.enabled = false;
            }

            foreach (var child in _model.GetComponentsInChildren<Transform>(true))
            {
                if (child != null && child.name == "ProductionFarSilhouette")
                {
                    child.gameObject.SetActive(false);
                }
            }

            var animator = _model.GetComponentInChildren<Animator>(true);
            if (animator != null) animator.enabled = false;

            // VFX cues are useful in live combat but would create noisy, continuously
            // running particles in a menu card. Keep the authored mesh and Animator.
            foreach (var particle in _model.GetComponentsInChildren<ParticleSystem>(true))
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particle.gameObject.SetActive(false);
            }

            var vfxCue = _model.GetComponentInChildren<ProductionVfxCue>(true);
            if (vfxCue != null) vfxCue.enabled = false;

            FrameModel();
            _hasModel = true;
            _stageRoot.SetActive(isActiveAndEnabled);
            _camera.enabled = isActiveAndEnabled;
        }

        private void FrameModel()
        {
            var renderers = _model.GetComponentsInChildren<Renderer>(true);
            if (renderers == null || renderers.Length == 0) return;

            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                if (renderers[i] != null) bounds.Encapsulate(renderers[i].bounds);
            }

            var minY = bounds.min.y;
            _model.transform.localPosition = Vector3.up * -minY;
            bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                if (renderers[i] != null) bounds.Encapsulate(renderers[i].bounds);
            }

            var target = bounds.center + Vector3.up * 0.06f;
            var maxDimension = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            var distance = Mathf.Max(3.1f, maxDimension * 2.65f);
            // Use a friendly three-quarter portrait angle. A nearly level camera
            // makes the compact low-poly rigs read as flat tokens in a square card.
            // A modest three-quarter yaw exposes each fighter's asymmetry (Bijli's
            // bolt shoulder, Pehel's gauntlet profile and Maya's offset cloak) so
            // the selection cards communicate silhouette rather than a flat front
            // plate.  The angle stays shallow enough to keep the role read stable.
            var direction = new Vector3(0.20f, 0.18f, -0.963f).normalized;
            _camera.transform.position = target + direction * distance;
            _camera.transform.LookAt(target, Vector3.up);
            _camera.orthographicSize = Mathf.Clamp(maxDimension * 0.38f, 0.62f, 1.12f);
        }

        private static void SetLayerRecursively(GameObject root, int layer)
        {
            if (root == null) return;
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                if (child != null) SetLayerRecursively(child.gameObject, layer);
            }
        }

        private static int AllocatePreviewLayer()
        {
            var layer = _nextPreviewLayer;
            _nextPreviewLayer = layer >= PreviewLayerLast ? PreviewLayerFirst : layer + 1;
            return layer;
        }
    }
}
