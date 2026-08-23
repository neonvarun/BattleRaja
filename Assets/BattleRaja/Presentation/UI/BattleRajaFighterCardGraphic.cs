using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    public enum BattleRajaFighterCardKind
    {
        Bijli,
        Pehel,
        Maya
    }

    /// <summary>
    /// Small original fighter glyph used above the selection labels. The glyphs make
    /// identity readable by silhouette and shape, not colour alone.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class BattleRajaFighterCardGraphic : MaskableGraphic
    {
        [SerializeField] private BattleRajaFighterCardKind fighter = BattleRajaFighterCardKind.Bijli;

        public void SetFighter(BattleRajaFighterCardKind value)
        {
            fighter = value;
            SetVerticesDirty();
        }

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
            var rect = rectTransform.rect;
            var center = rect.center + Vector2.up * rect.height * 0.03f;
            var scale = Mathf.Min(rect.width, rect.height) * 0.32f;
            var color = fighter == BattleRajaFighterCardKind.Bijli ? BattleRajaUiTheme.Cyan :
                fighter == BattleRajaFighterCardKind.Pehel ? BattleRajaUiTheme.Saffron : BattleRajaUiTheme.Magenta;

            AddCircle(vertexHelper, center + Vector2.down * scale * 0.15f, scale * 0.86f, new Color(0f, 0f, 0f, 0.28f), 24);
            AddCircle(vertexHelper, center + Vector2.up * scale * 0.44f, scale * 0.30f, color, 20);
            switch (fighter)
            {
                case BattleRajaFighterCardKind.Bijli:
                    AddPolygon(vertexHelper, new[]
                    {
                        center + new Vector2(-scale * 0.18f, scale * 0.54f),
                        center + new Vector2(scale * 0.30f, scale * 0.54f),
                        center + new Vector2(scale * 0.03f, scale * 0.08f),
                        center + new Vector2(scale * 0.28f, scale * 0.08f),
                        center + new Vector2(-scale * 0.25f, -scale * 0.52f),
                        center + new Vector2(-scale * 0.02f, -scale * 0.02f),
                        center + new Vector2(-scale * 0.32f, -scale * 0.02f)
                    }, BattleRajaUiTheme.Gold);
                    break;
                case BattleRajaFighterCardKind.Pehel:
                    AddCircle(vertexHelper, center + Vector2.left * scale * 0.72f, scale * 0.35f, BattleRajaUiTheme.Gold, 18);
                    AddCircle(vertexHelper, center + Vector2.right * scale * 0.72f, scale * 0.35f, BattleRajaUiTheme.Gold, 18);
                    AddPolygon(vertexHelper, new[]
                    {
                        center + new Vector2(-scale * 0.72f, scale * 0.28f),
                        center + new Vector2(scale * 0.72f, scale * 0.28f),
                        center + new Vector2(scale * 0.46f, -scale * 0.64f),
                        center + new Vector2(-scale * 0.46f, -scale * 0.64f)
                    }, color);
                    break;
                default:
                    AddPolygon(vertexHelper, new[]
                    {
                        center + new Vector2(-scale * 0.82f, scale * 0.42f),
                        center + new Vector2(scale * 0.82f, scale * 0.42f),
                        center + new Vector2(scale * 0.50f, scale * 0.76f),
                        center + new Vector2(0f, scale * 0.96f),
                        center + new Vector2(-scale * 0.50f, scale * 0.76f)
                    }, color);
                    AddPolygon(vertexHelper, new[]
                    {
                        center + new Vector2(-scale * 0.58f, -scale * 0.04f),
                        center + new Vector2(scale * 0.58f, -scale * 0.04f),
                        center + new Vector2(scale * 0.40f, -scale * 0.22f),
                        center + new Vector2(-scale * 0.40f, -scale * 0.22f)
                    }, BattleRajaUiTheme.Mint);
                    break;
            }
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
