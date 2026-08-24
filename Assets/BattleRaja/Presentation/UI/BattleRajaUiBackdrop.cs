using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Lightweight, code-driven backdrop for the offline product surfaces. It keeps the
    /// visual identity independent of scene assets without using a full-screen texture or
    /// adding any gameplay objects.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class BattleRajaUiBackdrop : MaskableGraphic
    {
        [SerializeField] private Color topColor = new Color(0.02f, 0.10f, 0.15f, 1f);
        [SerializeField] private Color bottomColor = new Color(0.055f, 0.035f, 0.09f, 1f);

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
            var rect = rectTransform.rect;
            AddQuad(vertexHelper,
                new Vector2(rect.xMin, rect.yMin),
                new Vector2(rect.xMax, rect.yMax),
                bottomColor,
                topColor);

            // Keep the backdrop calm behind touch controls while adding a restrained
            // frame language that survives portrait cropping and desktop scaling.
            var rail = new Color(0.22f, 0.78f, 0.82f, 0.22f);
            var warmRail = new Color(1f, 0.62f, 0.20f, 0.18f);
            const float railSize = 5f;
            AddQuad(vertexHelper, new Vector2(rect.xMin + 42f, rect.yMax - railSize),
                new Vector2(rect.xMax - 42f, rect.yMax), rail, rail);
            AddQuad(vertexHelper, new Vector2(rect.xMin + 42f, rect.yMin),
                new Vector2(rect.xMax - 42f, rect.yMin + railSize), warmRail, warmRail);
            AddQuad(vertexHelper, new Vector2(rect.xMin + 18f, rect.yMin + 72f),
                new Vector2(rect.xMin + 23f, rect.yMax - 72f), rail, rail);
            AddQuad(vertexHelper, new Vector2(rect.xMax - 23f, rect.yMin + 72f),
                new Vector2(rect.xMax - 18f, rect.yMax - 72f), warmRail, warmRail);

            var corner = new Color(0.70f, 0.90f, 0.84f, 0.26f);
            AddCorner(vertexHelper, rect.xMin + 42f, rect.yMax - 42f, corner, false, true);
            AddCorner(vertexHelper, rect.xMax - 42f, rect.yMax - 42f, corner, true, true);
            AddCorner(vertexHelper, rect.xMin + 42f, rect.yMin + 42f, corner, false, false);
            AddCorner(vertexHelper, rect.xMax - 42f, rect.yMin + 42f, corner, true, false);
        }

        private static void AddCorner(VertexHelper vertexHelper, float x, float y, Color color, bool right, bool top)
        {
            const float length = 28f;
            const float thickness = 4f;
            var horizontalMin = right ? x - length : x;
            var horizontalMax = right ? x : x + length;
            var verticalMin = top ? y - length : y;
            var verticalMax = top ? y : y + length;
            var horizontalYMin = top ? y - thickness : y;
            var horizontalYMax = top ? y : y + thickness;
            var verticalXMin = right ? x - thickness : x;
            var verticalXMax = right ? x : x + thickness;
            AddQuad(vertexHelper, new Vector2(horizontalMin, horizontalYMin),
                new Vector2(horizontalMax, horizontalYMax), color, color);
            AddQuad(vertexHelper, new Vector2(verticalXMin, verticalMin),
                new Vector2(verticalXMax, verticalMax), color, color);
        }

        private static void AddQuad(VertexHelper vertexHelper, Vector2 min, Vector2 max, Color bottom, Color top)
        {
            var start = vertexHelper.currentVertCount;
            AddVert(vertexHelper, new Vector3(min.x, min.y), bottom);
            AddVert(vertexHelper, new Vector3(min.x, max.y), top);
            AddVert(vertexHelper, new Vector3(max.x, max.y), top);
            AddVert(vertexHelper, new Vector3(max.x, min.y), bottom);
            vertexHelper.AddTriangle(start, start + 1, start + 2);
            vertexHelper.AddTriangle(start + 2, start + 3, start);
        }

        private static void AddVert(VertexHelper vertexHelper, Vector3 position, Color vertexColor)
        {
            var vertex = UIVertex.simpleVert;
            vertex.position = position;
            vertex.color = vertexColor;
            vertexHelper.AddVert(vertex);
        }
    }
}
