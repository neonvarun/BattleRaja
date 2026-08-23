using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Original vector-style hero illustration for the offline menu. It is intentionally
    /// rendered as UI geometry so the release surface has a coherent identity without
    /// depending on a licensed image or a large runtime texture.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class BattleRajaHeroGraphic : MaskableGraphic
    {
        private static readonly Color Ink = new Color(0.025f, 0.075f, 0.11f, 1f);
        private static readonly Color Teal = new Color(0.08f, 0.62f, 0.66f, 1f);
        private static readonly Color Cyan = new Color(0.16f, 0.86f, 0.96f, 1f);
        private static readonly Color Saffron = new Color(1f, 0.56f, 0.12f, 1f);
        private static readonly Color Gold = new Color(1f, 0.82f, 0.18f, 1f);
        private static readonly Color Magenta = new Color(0.84f, 0.28f, 0.78f, 1f);
        private static readonly Color Mint = new Color(0.30f, 0.92f, 0.64f, 1f);

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
            var rect = rectTransform.rect;
            var center = rect.center;
            var width = Mathf.Max(1f, rect.width);
            var height = Mathf.Max(1f, rect.height);
            var unit = Mathf.Min(width, height) * 0.42f;

            AddCircle(vertexHelper, center + new Vector2(0f, -height * 0.03f), unit * 0.70f, new Color(0f, 0f, 0f, 0.20f), 32);

            // Central canopy landmark.
            AddPolygon(vertexHelper, new[]
            {
                P(center, unit, -0.70f, -0.30f), P(center, unit, 0.70f, -0.30f),
                P(center, unit, 0.54f, 0.34f), P(center, unit, 0f, 0.54f),
                P(center, unit, -0.54f, 0.34f)
            }, Teal);
            AddPolygon(vertexHelper, new[]
            {
                P(center, unit, -0.56f, 0.12f), P(center, unit, -0.18f, 0.28f),
                P(center, unit, 0.18f, 0.28f), P(center, unit, 0.56f, 0.12f),
                P(center, unit, 0.42f, -0.10f), P(center, unit, -0.42f, -0.10f)
            }, Saffron);
            AddPolygon(vertexHelper, new[]
            {
                P(center, unit, -0.38f, -0.12f), P(center, unit, 0.38f, -0.12f),
                P(center, unit, 0.30f, -0.54f), P(center, unit, -0.30f, -0.54f)
            }, Ink);
            AddPolygon(vertexHelper, new[]
            {
                P(center, unit, -0.08f, 0.50f), P(center, unit, 0.08f, 0.50f),
                P(center, unit, 0.13f, 0.70f), P(center, unit, -0.13f, 0.70f)
            }, Gold);
            AddPolygon(vertexHelper, new[]
            {
                P(center, unit, -0.10f, 0.18f), P(center, unit, 0.22f, 0.18f),
                P(center, unit, 0.03f, -0.08f), P(center, unit, 0.22f, -0.08f),
                P(center, unit, -0.18f, -0.42f), P(center, unit, -0.01f, -0.10f),
                P(center, unit, -0.23f, -0.10f)
            }, Cyan);

            // Three distinct fighter tokens orbit the landmark.
            AddFighterToken(vertexHelper, center + new Vector2(-unit * 0.82f, -unit * 0.08f), unit * 0.21f, Cyan, 0);
            AddFighterToken(vertexHelper, center + new Vector2(unit * 0.82f, -unit * 0.08f), unit * 0.21f, Saffron, 1);
            AddFighterToken(vertexHelper, center + new Vector2(0f, -unit * 0.88f), unit * 0.21f, Magenta, 2);

            AddPolygon(vertexHelper, new[]
            {
                P(center, unit, -1.10f, -0.62f), P(center, unit, -0.96f, -0.54f),
                P(center, unit, -0.84f, -0.70f), P(center, unit, -0.98f, -0.76f)
            }, Mint);
            AddPolygon(vertexHelper, new[]
            {
                P(center, unit, 0.84f, -0.70f), P(center, unit, 0.98f, -0.76f),
                P(center, unit, 1.10f, -0.62f), P(center, unit, 0.96f, -0.54f)
            }, Gold);
        }

        private static void AddFighterToken(VertexHelper vertexHelper, Vector2 center, float radius, Color color, int style)
        {
            AddCircle(vertexHelper, center, radius, new Color(0f, 0f, 0f, 0.30f), 20);
            AddCircle(vertexHelper, center + Vector2.up * (radius * 0.12f), radius * 0.72f, color, 20);
            if (style == 0)
            {
                AddPolygon(vertexHelper, new[]
                {
                    center + new Vector2(-radius * 0.32f, radius * 0.22f),
                    center + new Vector2(radius * 0.46f, radius * 0.20f),
                    center + new Vector2(radius * 0.02f, -radius * 0.58f)
                }, Gold);
            }
            else if (style == 1)
            {
                AddCircle(vertexHelper, center + Vector2.left * (radius * 0.74f), radius * 0.28f, Gold, 14);
                AddCircle(vertexHelper, center + Vector2.right * (radius * 0.74f), radius * 0.28f, Gold, 14);
            }
            else
            {
                AddPolygon(vertexHelper, new[]
                {
                    center + new Vector2(-radius * 0.70f, radius * 0.34f),
                    center + new Vector2(radius * 0.70f, radius * 0.34f),
                    center + new Vector2(0f, radius * 0.84f)
                }, Mint);
            }
        }

        private static Vector2 P(Vector2 center, float scale, float x, float y)
        {
            return center + new Vector2(x * scale, y * scale);
        }

        private static void AddCircle(VertexHelper vertexHelper, Vector2 center, float radius, Color color, int segments)
        {
            var centerIndex = vertexHelper.currentVertCount;
            AddVert(vertexHelper, center, color);
            for (var i = 0; i <= segments; i++)
            {
                var angle = Mathf.PI * 2f * i / segments;
                AddVert(vertexHelper, center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius, color);
                if (i > 0) vertexHelper.AddTriangle(centerIndex, centerIndex + i, centerIndex + i + 1);
            }
        }

        private static void AddPolygon(VertexHelper vertexHelper, Vector2[] points, Color color)
        {
            if (points == null || points.Length < 3) return;
            var start = vertexHelper.currentVertCount;
            for (var i = 0; i < points.Length; i++) AddVert(vertexHelper, points[i], color);
            for (var i = 1; i < points.Length - 1; i++) vertexHelper.AddTriangle(start, start + i, start + i + 1);
        }

        private static void AddVert(VertexHelper vertexHelper, Vector2 position, Color color)
        {
            var vertex = UIVertex.simpleVert;
            vertex.position = position;
            vertex.color = color;
            vertexHelper.AddVert(vertex);
        }
    }
}
