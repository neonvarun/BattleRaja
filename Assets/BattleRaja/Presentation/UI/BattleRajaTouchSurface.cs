using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Circular, renderer-independent touch surface for the twin-stick controls. A custom
    /// graphic avoids the opaque square default UI sprite and keeps the action silhouettes
    /// readable on small Android screens.
    /// </summary>
    public sealed class BattleRajaTouchSurface : MaskableGraphic
    {
        [SerializeField] private Color accent = new Color(0.20f, 0.80f, 0.95f, 0.22f);
        [SerializeField] private bool drawRing = true;

        public void Configure(Color color, bool ring)
        {
            accent = color;
            drawRing = ring;
            SetVerticesDirty();
        }

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = true;
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
            var rect = rectTransform.rect;
            var center = rect.center;
            var radius = Mathf.Min(rect.width, rect.height) * 0.47f;
            var baseAlpha = drawRing ? Mathf.Max(0.08f, accent.a * 0.65f) : Mathf.Max(0.38f, accent.a);
            AddCircle(vertexHelper, center, radius, new Color(accent.r, accent.g, accent.b, baseAlpha), 32);
            if (!drawRing) return;

            var outer = radius * 0.98f;
            var inner = radius * 0.88f;
            AddRing(vertexHelper, center, outer, inner, new Color(accent.r, accent.g, accent.b, Mathf.Max(0.35f, accent.a * 2.3f)), 32);
        }

        private static void AddCircle(VertexHelper vertexHelper, Vector2 center, float radius, Color circleColor, int segments)
        {
            var centerIndex = vertexHelper.currentVertCount;
            AddVert(vertexHelper, center, circleColor);
            for (var i = 0; i <= segments; i++)
            {
                var angle = (Mathf.PI * 2f * i) / segments;
                AddVert(vertexHelper, center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius, circleColor);
                if (i == 0) continue;
                vertexHelper.AddTriangle(centerIndex, centerIndex + i, centerIndex + i + 1);
            }
        }

        private static void AddRing(VertexHelper vertexHelper, Vector2 center, float outer, float inner, Color ringColor, int segments)
        {
            var start = vertexHelper.currentVertCount;
            for (var i = 0; i <= segments; i++)
            {
                var angle = (Mathf.PI * 2f * i) / segments;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                AddVert(vertexHelper, center + direction * outer, ringColor);
                AddVert(vertexHelper, center + direction * inner, ringColor);
                if (i == 0) continue;
                var previous = start + (i - 1) * 2;
                var current = start + i * 2;
                vertexHelper.AddTriangle(previous, current, previous + 1);
                vertexHelper.AddTriangle(previous + 1, current, current + 1);
            }
        }

        private static void AddVert(VertexHelper vertexHelper, Vector2 position, Color vertexColor)
        {
            var vertex = UIVertex.simpleVert;
            vertex.position = position;
            vertex.color = vertexColor;
            vertexHelper.AddVert(vertex);
        }
    }
}
