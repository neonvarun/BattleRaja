using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Match
{
    /// <summary>
    /// Render-only Aandhi boundary cue. It consumes the match snapshot and never
    /// participates in movement, collision, damage, or result decisions.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AandhiZoneVisual : MonoBehaviour
    {
        private const int DefaultSegments = 96;
        private const float RingHeight = 0.08f;

        [SerializeField] private OfflineMatchController match;
        [SerializeField, Min(24)] private int segments = DefaultSegments;
        [SerializeField] private float lineWidth = 0.10f;

        private LineRenderer _currentRing;
        private LineRenderer _nextRing;
        private Material _material;

        private static readonly Color CurrentColor = new Color(0.25f, 0.86f, 1f, 0.78f);
        private static readonly Color WarningColor = new Color(1f, 0.72f, 0.20f, 0.82f);
        private static readonly Color ClosingColor = new Color(1f, 0.27f, 0.15f, 0.92f);
        private static readonly Color NextColor = new Color(0.74f, 0.90f, 1f, 0.45f);

        private void Awake()
        {
            match = match != null ? match : GetComponent<OfflineMatchController>();
            if (match == null) match = FindAnyObjectByType<OfflineMatchController>();
            segments = Mathf.Max(24, segments);
            _material = CreateMaterial();
            _currentRing = CreateRing("AandhiCurrentBoundary", _material);
            _nextRing = CreateRing("AandhiNextBoundary", _material);
        }

        private void LateUpdate()
        {
            if (match == null || match.ZoneRadius <= 0.01f)
            {
                SetVisible(_currentRing, false);
                SetVisible(_nextRing, false);
                return;
            }

            var currentColor = match.AandhiState == AandhiState.Closing ? ClosingColor
                : match.AandhiState == AandhiState.Warning ? WarningColor
                : CurrentColor;
            UpdateRing(_currentRing, match.ZoneCenter, match.ZoneRadius, currentColor);

            var showNext = match.AandhiState == AandhiState.Warning
                && match.NextZoneRadius > 0.01f
                && Mathf.Abs(match.NextZoneRadius - match.ZoneRadius) > 0.01f;
            if (showNext)
            {
                UpdateRing(_nextRing, match.NextZoneCenter, match.NextZoneRadius, NextColor);
            }
            else
            {
                SetVisible(_nextRing, false);
            }
        }

        private void OnDestroy()
        {
            if (_material != null)
            {
                Destroy(_material);
                _material = null;
            }
        }

        private LineRenderer CreateRing(string name, Material material)
        {
            var ringObject = new GameObject(name);
            ringObject.transform.SetParent(transform, false);
            var line = ringObject.AddComponent<LineRenderer>();
            line.useWorldSpace = true;
            line.loop = true;
            line.positionCount = segments;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.numCapVertices = 2;
            line.numCornerVertices = 2;
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            line.receiveShadows = false;
            line.material = material;
            line.enabled = false;
            return line;
        }

        private void UpdateRing(LineRenderer ring, Float2 center, float radius, Color color)
        {
            if (ring == null) return;
            ring.startColor = color;
            ring.endColor = color;
            ring.startWidth = lineWidth;
            ring.endWidth = lineWidth;
            ring.positionCount = segments;

            for (var i = 0; i < segments; i++)
            {
                var angle = (Mathf.PI * 2f * i) / segments;
                ring.SetPosition(i, new Vector3(
                    center.X + Mathf.Cos(angle) * radius,
                    RingHeight,
                    center.Y + Mathf.Sin(angle) * radius));
            }

            ring.enabled = true;
        }

        private static void SetVisible(LineRenderer ring, bool visible)
        {
            if (ring != null) ring.enabled = visible;
        }

        private static Material CreateMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Sprites/Default")
                ?? Shader.Find("Unlit/Color");
            var material = new Material(shader) { name = "BattleRaja Aandhi Boundary Material" };
            if (material.HasProperty("_Surface")) material.SetFloat("_Surface", 1f);
            if (material.HasProperty("_Blend")) material.SetFloat("_Blend", 0f);
            if (material.HasProperty("_ZWrite")) material.SetFloat("_ZWrite", 0f);
            if (material.HasProperty("_Cull")) material.SetFloat("_Cull", 0f);
            return material;
        }
    }
}
