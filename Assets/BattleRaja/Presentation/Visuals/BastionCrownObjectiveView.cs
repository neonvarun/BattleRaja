using System.Collections.Generic;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Match;
using UnityEngine;

namespace BattleRaja.Presentation.Visuals
{
    /// <summary>
    /// Presentation-only Crown Spark and shrine telegraph. The match controller
    /// owns every objective decision; this view mirrors its immutable snapshot and
    /// uses collider-free line rings so the authored arena collision remains the
    /// only movement truth.
    /// </summary>
    public sealed class BastionCrownObjectiveView : MonoBehaviour
    {
        private const int RingPointCount = 25;

        [SerializeField] private OfflineMatchController match;
        [SerializeField] private Transform crownVisual;
        [SerializeField] private Material markerMaterial;
        [SerializeField] private float crownHeight = 1.65f;
        [SerializeField] private float pulseSpeed = 3.2f;

        private readonly List<LineRenderer> _socketRings = new List<LineRenderer>(3);
        private LineRenderer _rajaShrineRing;
        private LineRenderer _rivalShrineRing;
        private Material _runtimeMarkerMaterial;
        private Vector3[] _ringPoints;
        private Vector3 _crownBaseScale = Vector3.one;

        private void Start()
        {
            match = match != null ? match : FindAnyObjectByType<OfflineMatchController>();
            ResolveCrownVisual();
            BuildMarkers();
        }

        private void OnDestroy()
        {
            if (_runtimeMarkerMaterial != null) Destroy(_runtimeMarkerMaterial);
        }

        private void LateUpdate()
        {
            if (match == null || !match.IsBastionCrown)
            {
                SetMarkersActive(false);
                return;
            }

            var crown = match.BastionCrownState;
            var pulse = 1f + Mathf.Sin(Time.unscaledTime * pulseSpeed) * 0.08f;
            if (crownVisual != null)
            {
                crownVisual.gameObject.SetActive(!match.ResultsShown);
                crownVisual.position = new Vector3(crown.Position.X, crownHeight, crown.Position.Y);
                crownVisual.localScale = _crownBaseScale * pulse;
                var renderer = crownVisual.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    var color = crown.IsCarried
                        ? crown.CarrierId.Value <= 4 ? new Color(0.20f, 0.76f, 1f, 1f) : new Color(1f, 0.35f, 0.24f, 1f)
                        : crown.Dropped ? new Color(1f, 0.68f, 0.18f, 1f) : new Color(0.80f, 0.94f, 1f, 1f);
                    if (renderer.material.HasProperty("_BaseColor")) renderer.material.SetColor("_BaseColor", color);
                    else if (renderer.material.HasProperty("_Color")) renderer.material.SetColor("_Color", color);
                }
            }

            var definition = match.BastionCrown.Definition;
            for (var i = 0; i < _socketRings.Count; i++)
            {
                var active = !crown.IsCarried && !match.ResultsShown && i == crown.SocketIndex;
                _socketRings[i].gameObject.SetActive(active);
                if (active)
                {
                    DrawRing(_socketRings[i], definition.Objective.SocketPositions[i], 1.15f,
                        crown.Dropped ? new Color(1f, 0.66f, 0.18f, 0.9f) : new Color(0.60f, 0.90f, 1f, 0.85f));
                }
            }

            if (_rajaShrineRing != null)
            {
                _rajaShrineRing.gameObject.SetActive(!match.ResultsShown);
                DrawRing(_rajaShrineRing, definition.Raja.ShrinePosition, 1.4f, new Color(0.20f, 0.72f, 1f, 0.72f));
            }

            if (_rivalShrineRing != null)
            {
                _rivalShrineRing.gameObject.SetActive(!match.ResultsShown);
                DrawRing(_rivalShrineRing, definition.Rival.ShrinePosition, 1.4f, new Color(1f, 0.34f, 0.24f, 0.72f));
            }
        }

        private void ResolveCrownVisual()
        {
            if (crownVisual == null)
            {
                var authored = transform.Find("V1BastionVisuals/BastionCrownOrb");
                crownVisual = authored;
            }

            if (crownVisual != null) _crownBaseScale = crownVisual.localScale;
        }

        private void BuildMarkers()
        {
            _ringPoints = new Vector3[RingPointCount];
            if (markerMaterial == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                if (shader != null)
                {
                    _runtimeMarkerMaterial = new Material(shader) { name = "Bastion Crown Marker Runtime" };
                    markerMaterial = _runtimeMarkerMaterial;
                }
            }

            if (match == null || !match.IsBastionCrown) return;
            var definition = match.BastionCrown.Definition;
            for (var i = 0; i < definition.Objective.SocketPositions.Length; i++)
            {
                var ring = CreateRing("CrownSocketRing" + i);
                _socketRings.Add(ring);
                ring.gameObject.SetActive(false);
            }

            _rajaShrineRing = CreateRing("RajaShrineRing");
            _rivalShrineRing = CreateRing("RivalShrineRing");
        }

        private LineRenderer CreateRing(string name)
        {
            var ringObject = new GameObject(name);
            ringObject.transform.SetParent(transform, false);
            var ring = ringObject.AddComponent<LineRenderer>();
            ring.useWorldSpace = true;
            ring.loop = true;
            ring.widthMultiplier = 0.07f;
            ring.positionCount = RingPointCount;
            ring.numCapVertices = 2;
            ring.sharedMaterial = markerMaterial;
            ring.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            ring.receiveShadows = false;
            return ring;
        }

        private void DrawRing(LineRenderer ring, Float2 center, float radius, Color color)
        {
            if (ring == null || _ringPoints == null) return;
            ring.startColor = color;
            ring.endColor = color;
            for (var i = 0; i < RingPointCount; i++)
            {
                var angle = (Mathf.PI * 2f * i) / (RingPointCount - 1);
                _ringPoints[i] = new Vector3(
                    center.X + Mathf.Cos(angle) * radius,
                    0.075f,
                    center.Y + Mathf.Sin(angle) * radius);
            }

            ring.SetPositions(_ringPoints);
        }

        private void SetMarkersActive(bool active)
        {
            for (var i = 0; i < _socketRings.Count; i++) _socketRings[i].gameObject.SetActive(active && i == 0);
            if (_rajaShrineRing != null) _rajaShrineRing.gameObject.SetActive(active);
            if (_rivalShrineRing != null) _rivalShrineRing.gameObject.SetActive(active);
        }
    }
}
