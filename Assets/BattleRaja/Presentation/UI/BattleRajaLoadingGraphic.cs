using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Lightweight, original loading progress treatment for the offline flow. It is
    /// deliberately a small mesh so loading never depends on a network asset or a
    /// placeholder Unity progress bar.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class BattleRajaLoadingGraphic : MaskableGraphic
    {
        private const float TrackHeight = 10f;
        private float progress;

        public float Progress => progress;

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
            color = Color.white;
        }

        public void SetProgress(float value)
        {
            var next = Mathf.Clamp01(value);
            if (Mathf.Abs(next - progress) < 0.001f) return;
            progress = next;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
            var rect = rectTransform.rect;
            var horizontalPadding = Mathf.Min(18f, rect.width * 0.08f);
            var left = rect.xMin + horizontalPadding;
            var right = rect.xMax - horizontalPadding;
            var centerY = rect.center.y;
            var track = new Rect(left, centerY - TrackHeight * 0.5f, Mathf.Max(1f, right - left), TrackHeight);

            AddQuad(vertexHelper, track, new Color(BattleRajaUiTheme.SurfaceRaised.r,
                BattleRajaUiTheme.SurfaceRaised.g, BattleRajaUiTheme.SurfaceRaised.b, 0.92f));

            var fill = new Rect(track.x, track.y, track.width * progress, track.height);
            if (fill.width > 0.01f)
            {
                AddQuad(vertexHelper, fill, BattleRajaUiTheme.Cyan);
                AddDiamond(vertexHelper, new Vector2(fill.xMax, track.center.y), 7f, BattleRajaUiTheme.Gold);
            }

            // Three small markers make the bar read as a route, rather than a generic
            // percentage meter, even when the scene load completes very quickly.
            for (var i = 1; i <= 3; i++)
            {
                var markerX = Mathf.Lerp(track.xMin, track.xMax, i / 4f);
                AddDiamond(vertexHelper, new Vector2(markerX, track.center.y), 3.5f,
                    i / 4f <= progress ? BattleRajaUiTheme.Gold : BattleRajaUiTheme.MutedText);
            }
        }

        private static void AddQuad(VertexHelper vertexHelper, Rect rect, Color quadColor)
        {
            var start = vertexHelper.currentVertCount;
            AddVert(vertexHelper, new Vector2(rect.xMin, rect.yMin), quadColor);
            AddVert(vertexHelper, new Vector2(rect.xMin, rect.yMax), quadColor);
            AddVert(vertexHelper, new Vector2(rect.xMax, rect.yMax), quadColor);
            AddVert(vertexHelper, new Vector2(rect.xMax, rect.yMin), quadColor);
            vertexHelper.AddTriangle(start, start + 1, start + 2);
            vertexHelper.AddTriangle(start + 2, start + 3, start);
        }

        private static void AddDiamond(VertexHelper vertexHelper, Vector2 center, float radius, Color diamondColor)
        {
            var start = vertexHelper.currentVertCount;
            AddVert(vertexHelper, center + Vector2.up * radius, diamondColor);
            AddVert(vertexHelper, center + Vector2.right * radius, diamondColor);
            AddVert(vertexHelper, center + Vector2.down * radius, diamondColor);
            AddVert(vertexHelper, center + Vector2.left * radius, diamondColor);
            vertexHelper.AddTriangle(start, start + 1, start + 2);
            vertexHelper.AddTriangle(start + 2, start + 3, start);
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
