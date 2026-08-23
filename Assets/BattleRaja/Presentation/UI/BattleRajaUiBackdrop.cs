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

            // Keep the backdrop deliberately calm behind touch controls. A previous
            // diagonal implementation produced rectangular interpolation bands on some
            // mobile GPUs, so the identity accents now live in the logo and button edges.
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
