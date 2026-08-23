using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Small vector-like BattleRaja mark used in menus and loading surfaces. The logo is
    /// intentionally original and procedural so a release build does not depend on a
    /// third-party font, texture or licensed brand asset.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class BattleRajaLogoGraphic : MaskableGraphic
    {
        private static readonly Color ShieldOuter = new Color(1f, 0.57f, 0.12f, 1f);
        private static readonly Color ShieldInner = new Color(0.025f, 0.10f, 0.16f, 1f);
        private static readonly Color BoltCyan = new Color(0.16f, 0.88f, 0.95f, 1f);
        private static readonly Color BoltGold = new Color(1f, 0.83f, 0.12f, 1f);

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
            var rect = rectTransform.rect;
            var scale = Mathf.Min(rect.width, rect.height) * 0.46f;
            var center = rect.center;

            var outer = new[]
            {
                P(center, scale, -0.78f, 0.86f), P(center, scale, 0.78f, 0.86f),
                P(center, scale, 0.92f, 0.08f), P(center, scale, 0.0f, -1.0f),
                P(center, scale, -0.92f, 0.08f)
            };
            var inner = new[]
            {
                P(center, scale * 0.82f, -0.70f, 0.73f), P(center, scale * 0.82f, 0.70f, 0.73f),
                P(center, scale * 0.82f, 0.78f, 0.07f), P(center, scale * 0.82f, 0.0f, -0.82f),
                P(center, scale * 0.82f, -0.78f, 0.07f)
            };
            AddPolygon(vertexHelper, outer, ShieldOuter);
            AddPolygon(vertexHelper, inner, ShieldInner);

            var cyanBolt = new[]
            {
                P(center, scale * 0.78f, -0.20f, 0.66f), P(center, scale * 0.78f, 0.24f, 0.12f),
                P(center, scale * 0.78f, 0.04f, 0.12f), P(center, scale * 0.78f, 0.30f, -0.62f),
                P(center, scale * 0.78f, -0.08f, -0.06f), P(center, scale * 0.78f, -0.36f, -0.06f)
            };
            var goldBolt = new[]
            {
                P(center, scale * 0.78f, 0.14f, 0.54f), P(center, scale * 0.78f, 0.48f, 0.02f),
                P(center, scale * 0.78f, 0.20f, 0.04f), P(center, scale * 0.78f, 0.06f, -0.48f),
                P(center, scale * 0.78f, -0.12f, -0.02f), P(center, scale * 0.78f, -0.40f, -0.02f)
            };
            AddPolygon(vertexHelper, cyanBolt, BoltCyan);
            AddPolygon(vertexHelper, goldBolt, BoltGold);
        }

        private static Vector2 P(Vector2 center, float scale, float x, float y)
        {
            return center + new Vector2(x * scale, y * scale);
        }

        private static void AddPolygon(VertexHelper vertexHelper, Vector2[] points, Color polygonColor)
        {
            if (points == null || points.Length < 3) return;
            var start = vertexHelper.currentVertCount;
            for (var i = 0; i < points.Length; i++)
            {
                var vertex = UIVertex.simpleVert;
                vertex.position = points[i];
                vertex.color = polygonColor;
                vertexHelper.AddVert(vertex);
            }

            for (var i = 1; i < points.Length - 1; i++)
            {
                vertexHelper.AddTriangle(start, start + i, start + i + 1);
            }
        }
    }
}
