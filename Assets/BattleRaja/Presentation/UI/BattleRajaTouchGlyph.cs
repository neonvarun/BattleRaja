using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Small vector glyphs placed inside the touch controls. The symbols are drawn as
    /// UI geometry so the mobile controls communicate intent without depending on a
    /// font, emoji support or a third-party icon pack.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class BattleRajaTouchGlyph : MaskableGraphic
    {
        public enum Kind
        {
            Move,
            Aim,
            Attack,
            Ability,
            Gadget
        }

        [SerializeField] private Kind kind;
        [SerializeField] private Color accent = Color.white;

        public Kind GlyphKind => kind;

        public void Configure(Kind value, Color color)
        {
            kind = value;
            accent = color;
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
            var center = rect.center;
            var scale = Mathf.Min(rect.width, rect.height) * 0.46f;
            var ink = Color.Lerp(Color.white, accent, 0.24f);
            var glow = new Color(accent.r, accent.g, accent.b, 0.88f);

            switch (kind)
            {
                case Kind.Move:
                    DrawMove(vertexHelper, center, scale, ink, glow);
                    break;
                case Kind.Aim:
                    DrawAim(vertexHelper, center, scale, ink, glow);
                    break;
                case Kind.Attack:
                    DrawAttack(vertexHelper, center, scale, ink, glow);
                    break;
                case Kind.Ability:
                    DrawAbility(vertexHelper, center, scale, ink, glow);
                    break;
                default:
                    DrawGadget(vertexHelper, center, scale, ink, glow);
                    break;
            }
        }

        private static void DrawMove(VertexHelper vertexHelper, Vector2 center, float scale, Color ink, Color glow)
        {
            AddCircle(vertexHelper, center, scale * 0.16f, glow, 18);
            AddArrow(vertexHelper, center, Vector2.up, scale * 0.52f, scale * 0.22f, ink);
            AddArrow(vertexHelper, center, Vector2.right, scale * 0.52f, scale * 0.22f, ink);
            AddArrow(vertexHelper, center, Vector2.down, scale * 0.52f, scale * 0.22f, ink);
            AddArrow(vertexHelper, center, Vector2.left, scale * 0.52f, scale * 0.22f, ink);
        }

        private static void DrawAim(VertexHelper vertexHelper, Vector2 center, float scale, Color ink, Color glow)
        {
            AddRing(vertexHelper, center, scale * 0.48f, scale * 0.34f, ink, 24);
            AddLine(vertexHelper, center + Vector2.left * scale * 0.68f, center + Vector2.right * scale * 0.68f,
                scale * 0.095f, glow);
            AddLine(vertexHelper, center + Vector2.down * scale * 0.68f, center + Vector2.up * scale * 0.68f,
                scale * 0.095f, glow);
            AddCircle(vertexHelper, center, scale * 0.11f, glow, 18);
        }

        private static void DrawAttack(VertexHelper vertexHelper, Vector2 center, float scale, Color ink, Color glow)
        {
            AddPolygon(vertexHelper, new[]
            {
                P(center, scale, -0.16f, 0.62f), P(center, scale, 0.24f, 0.12f),
                P(center, scale, 0.04f, 0.12f), P(center, scale, 0.18f, -0.62f),
                P(center, scale, -0.24f, -0.06f), P(center, scale, -0.04f, -0.06f)
            }, ink);
            AddLine(vertexHelper, center + new Vector2(-scale * 0.44f, -scale * 0.34f),
                center + new Vector2(-scale * 0.72f, -scale * 0.54f), scale * 0.09f, glow);
            AddLine(vertexHelper, center + new Vector2(scale * 0.40f, scale * 0.30f),
                center + new Vector2(scale * 0.68f, scale * 0.48f), scale * 0.09f, glow);
        }

        private static void DrawAbility(VertexHelper vertexHelper, Vector2 center, float scale, Color ink, Color glow)
        {
            AddPolygon(vertexHelper, new[]
            {
                P(center, scale, 0f, 0.68f), P(center, scale, 0.22f, 0.22f),
                P(center, scale, 0.68f, 0f), P(center, scale, 0.22f, -0.22f),
                P(center, scale, 0f, -0.68f), P(center, scale, -0.22f, -0.22f),
                P(center, scale, -0.68f, 0f), P(center, scale, -0.22f, 0.22f)
            }, ink);
            AddCircle(vertexHelper, center, scale * 0.14f, glow, 18);
            AddLine(vertexHelper, center + Vector2.left * scale * 0.88f, center + Vector2.right * scale * 0.88f,
                scale * 0.065f, glow);
            AddLine(vertexHelper, center + Vector2.down * scale * 0.88f, center + Vector2.up * scale * 0.88f,
                scale * 0.065f, glow);
        }

        private static void DrawGadget(VertexHelper vertexHelper, Vector2 center, float scale, Color ink, Color glow)
        {
            var bodyMin = center + new Vector2(-scale * 0.52f, -scale * 0.34f);
            var bodyMax = center + new Vector2(scale * 0.52f, scale * 0.30f);
            AddQuad(vertexHelper, bodyMin, bodyMax, ink);
            AddLine(vertexHelper, center + new Vector2(-scale * 0.38f, scale * 0.30f),
                center + new Vector2(-scale * 0.24f, scale * 0.56f), scale * 0.10f, glow);
            AddLine(vertexHelper, center + new Vector2(scale * 0.38f, scale * 0.30f),
                center + new Vector2(scale * 0.24f, scale * 0.56f), scale * 0.10f, glow);
            AddLine(vertexHelper, center + Vector2.left * scale * 0.24f, center + Vector2.right * scale * 0.24f,
                scale * 0.10f, glow);
            AddLine(vertexHelper, center + Vector2.down * scale * 0.24f, center + Vector2.up * scale * 0.24f,
                scale * 0.10f, glow);
        }

        private static void AddArrow(VertexHelper vertexHelper, Vector2 center, Vector2 direction, float length, float head, Color color)
        {
            var tip = center + direction * length;
            var tail = center + direction * (length * 0.12f);
            AddLine(vertexHelper, tail, tip - direction * head * 0.42f, head * 0.26f, color);
            var side = new Vector2(-direction.y, direction.x);
            AddPolygon(vertexHelper, new[]
            {
                tip,
                tip - direction * head + side * head * 0.62f,
                tip - direction * head - side * head * 0.62f
            }, color);
        }

        private static void AddLine(VertexHelper vertexHelper, Vector2 start, Vector2 end, float width, Color color)
        {
            var direction = (end - start).normalized;
            if (direction.sqrMagnitude < 0.0001f) return;
            var side = new Vector2(-direction.y, direction.x) * (width * 0.5f);
            AddQuad(vertexHelper, start - side, end + side, color);
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

        private static void AddRing(VertexHelper vertexHelper, Vector2 center, float outer, float inner, Color color, int segments)
        {
            var start = vertexHelper.currentVertCount;
            for (var i = 0; i <= segments; i++)
            {
                var angle = Mathf.PI * 2f * i / segments;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                AddVert(vertexHelper, center + direction * outer, color);
                AddVert(vertexHelper, center + direction * inner, color);
                if (i == 0) continue;
                var previous = start + (i - 1) * 2;
                var current = start + i * 2;
                vertexHelper.AddTriangle(previous, current, previous + 1);
                vertexHelper.AddTriangle(previous + 1, current, current + 1);
            }
        }

        private static void AddQuad(VertexHelper vertexHelper, Vector2 min, Vector2 max, Color color)
        {
            var start = vertexHelper.currentVertCount;
            AddVert(vertexHelper, new Vector2(min.x, min.y), color);
            AddVert(vertexHelper, new Vector2(min.x, max.y), color);
            AddVert(vertexHelper, new Vector2(max.x, max.y), color);
            AddVert(vertexHelper, new Vector2(max.x, min.y), color);
            vertexHelper.AddTriangle(start, start + 1, start + 2);
            vertexHelper.AddTriangle(start + 2, start + 3, start);
        }

        private static void AddPolygon(VertexHelper vertexHelper, Vector2[] points, Color color)
        {
            if (points == null || points.Length < 3) return;
            var start = vertexHelper.currentVertCount;
            for (var i = 0; i < points.Length; i++) AddVert(vertexHelper, points[i], color);
            for (var i = 1; i < points.Length - 1; i++) vertexHelper.AddTriangle(start, start + i, start + i + 1);
        }

        private static Vector2 P(Vector2 center, float scale, float x, float y)
        {
            return center + new Vector2(x * scale, y * scale);
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
