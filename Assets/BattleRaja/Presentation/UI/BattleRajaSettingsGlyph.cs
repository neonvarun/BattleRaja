using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Original vector icon for a settings tile. It gives each accessibility and
    /// comfort option a visual anchor without relying on emoji, a font icon or a
    /// third-party asset. The small status dot is deliberately redundant with the
    /// text label so state is still readable in high-contrast mode.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class BattleRajaSettingsGlyph : MaskableGraphic
    {
        public enum Kind
        {
            LeftHanded,
            ReducedFlashes,
            HighContrast,
            AimAssist,
            Haptics,
            Music,
            Effects,
            TextSize
        }

        [SerializeField] private Kind kind;
        [SerializeField] private Color accent = Color.white;
        [SerializeField] private bool enabledState;

        public Kind GlyphKind => kind;
        public bool EnabledState => enabledState;

        public void Configure(Kind value, Color color, bool isEnabled)
        {
            kind = value;
            accent = color;
            enabledState = isEnabled;
            SetVerticesDirty();
        }

        public void SetEnabled(bool isEnabled)
        {
            if (enabledState == isEnabled) return;
            enabledState = isEnabled;
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
            var size = Mathf.Min(rect.width, rect.height);
            if (size <= 1f) return;

            var center = rect.center;
            var radius = size * 0.34f;
            var active = enabledState ? accent : Color.Lerp(accent, BattleRajaUiTheme.MutedText, 0.58f);
            var shell = new Color(active.r, active.g, active.b, enabledState ? 0.24f : 0.11f);
            AddCircle(vertexHelper, center, radius * 1.28f, new Color(0f, 0f, 0f, 0.26f), 20);
            AddCircle(vertexHelper, center, radius, shell, 20);
            AddRing(vertexHelper, center, radius * 0.88f, radius * 0.72f, active, 20);

            switch (kind)
            {
                case Kind.LeftHanded:
                    DrawHand(vertexHelper, center, radius * 0.76f, active);
                    break;
                case Kind.ReducedFlashes:
                    DrawFlash(vertexHelper, center, radius * 0.78f, active);
                    break;
                case Kind.HighContrast:
                    DrawContrast(vertexHelper, center, radius * 0.78f, active);
                    break;
                case Kind.AimAssist:
                    DrawAim(vertexHelper, center, radius * 0.78f, active);
                    break;
                case Kind.Haptics:
                    DrawHaptics(vertexHelper, center, radius * 0.78f, active);
                    break;
                case Kind.Music:
                case Kind.Effects:
                    DrawSliders(vertexHelper, center, radius * 0.78f, active, kind == Kind.Effects);
                    break;
                default:
                    DrawText(vertexHelper, center, radius * 0.78f, active);
                    break;
            }

            // The dot is a compact, colour-independent state cue. It is also useful
            // when the text scale is increased and the label wraps onto two lines.
            var statusColor = enabledState ? BattleRajaUiTheme.Mint : BattleRajaUiTheme.MutedText;
            AddCircle(vertexHelper, center + new Vector2(radius * 0.82f, -radius * 0.82f), radius * 0.22f, statusColor, 14);
            if (enabledState)
            {
                AddLine(vertexHelper,
                    center + new Vector2(radius * 0.70f, -radius * 0.84f),
                    center + new Vector2(radius * 0.79f, -radius * 0.94f),
                    radius * 0.10f, Color.white);
                AddLine(vertexHelper,
                    center + new Vector2(radius * 0.79f, -radius * 0.94f),
                    center + new Vector2(radius * 0.98f, -radius * 0.70f),
                    radius * 0.10f, Color.white);
            }
        }

        private static void DrawHand(VertexHelper vh, Vector2 center, float scale, Color color)
        {
            AddLine(vh, center + new Vector2(-scale * 0.10f, -scale * 0.50f), center + new Vector2(-scale * 0.10f, scale * 0.30f), scale * 0.18f, color);
            AddLine(vh, center + new Vector2(-scale * 0.10f, scale * 0.22f), center + new Vector2(-scale * 0.58f, scale * 0.46f), scale * 0.16f, color);
            AddLine(vh, center + new Vector2(-scale * 0.02f, scale * 0.28f), center + new Vector2(scale * 0.04f, scale * 0.62f), scale * 0.16f, color);
            AddLine(vh, center + new Vector2(scale * 0.08f, scale * 0.28f), center + new Vector2(scale * 0.25f, scale * 0.64f), scale * 0.16f, color);
            AddLine(vh, center + new Vector2(scale * 0.22f, scale * 0.24f), center + new Vector2(scale * 0.48f, scale * 0.53f), scale * 0.16f, color);
        }

        private static void DrawFlash(VertexHelper vh, Vector2 center, float scale, Color color)
        {
            AddPolygon(vh, new[]
            {
                P(center, scale, -0.10f, 0.72f), P(center, scale, 0.28f, 0.10f),
                P(center, scale, 0.04f, 0.10f), P(center, scale, 0.16f, -0.72f),
                P(center, scale, -0.28f, -0.04f), P(center, scale, -0.04f, -0.04f)
            }, color);
            AddLine(vh, center + Vector2.left * scale * 0.80f, center + Vector2.left * scale * 0.52f, scale * 0.10f, color);
            AddLine(vh, center + Vector2.right * scale * 0.80f, center + Vector2.right * scale * 0.52f, scale * 0.10f, color);
        }

        private static void DrawContrast(VertexHelper vh, Vector2 center, float scale, Color color)
        {
            AddCircle(vh, center, scale * 0.54f, color, 20);
            AddQuad(vh, center + new Vector2(-scale * 0.02f, -scale * 0.54f), center + new Vector2(scale * 0.54f, scale * 0.54f), BattleRajaUiTheme.Ink);
            AddLine(vh, center + Vector2.up * scale * 0.68f, center + Vector2.down * scale * 0.68f, scale * 0.09f, color);
        }

        private static void DrawAim(VertexHelper vh, Vector2 center, float scale, Color color)
        {
            AddRing(vh, center, scale * 0.54f, scale * 0.38f, color, 20);
            AddLine(vh, center + Vector2.left * scale * 0.72f, center + Vector2.right * scale * 0.72f, scale * 0.09f, color);
            AddLine(vh, center + Vector2.down * scale * 0.72f, center + Vector2.up * scale * 0.72f, scale * 0.09f, color);
            AddCircle(vh, center, scale * 0.10f, color, 14);
        }

        private static void DrawHaptics(VertexHelper vh, Vector2 center, float scale, Color color)
        {
            AddLine(vh, center + Vector2.down * scale * 0.58f, center + Vector2.up * scale * 0.58f, scale * 0.12f, color);
            AddRingArc(vh, center, scale * 0.56f, scale * 0.38f, 0.70f, 2.44f, color, 12);
            AddRingArc(vh, center, scale * 0.78f, scale * 0.63f, 0.70f, 2.44f, color, 12);
        }

        private static void DrawSliders(VertexHelper vh, Vector2 center, float scale, Color color, bool offset)
        {
            var x = offset ? scale * 0.18f : -scale * 0.18f;
            AddLine(vh, center + new Vector2(-scale * 0.62f, scale * 0.42f), center + new Vector2(scale * 0.62f, scale * 0.42f), scale * 0.09f, color);
            AddLine(vh, center + new Vector2(-scale * 0.62f, -scale * 0.42f), center + new Vector2(scale * 0.62f, -scale * 0.42f), scale * 0.09f, color);
            AddCircle(vh, center + new Vector2(x, scale * 0.42f), scale * 0.15f, color, 14);
            AddCircle(vh, center - new Vector2(x, scale * 0.42f), scale * 0.15f, color, 14);
        }

        private static void DrawText(VertexHelper vh, Vector2 center, float scale, Color color)
        {
            AddLine(vh, center + new Vector2(-scale * 0.52f, scale * 0.52f), center + new Vector2(scale * 0.52f, scale * 0.52f), scale * 0.13f, color);
            AddLine(vh, center + Vector2.up * scale * 0.52f, center + Vector2.down * scale * 0.60f, scale * 0.13f, color);
        }

        private static void AddRingArc(VertexHelper vh, Vector2 center, float outer, float inner, float start, float end, Color color, int segments)
        {
            var previousOuter = center + new Vector2(Mathf.Cos(start), Mathf.Sin(start)) * outer;
            var previousInner = center + new Vector2(Mathf.Cos(start), Mathf.Sin(start)) * inner;
            for (var i = 1; i <= segments; i++)
            {
                var t = Mathf.Lerp(start, end, i / (float)segments);
                var direction = new Vector2(Mathf.Cos(t), Mathf.Sin(t));
                var nextOuter = center + direction * outer;
                var nextInner = center + direction * inner;
                AddQuad(vh, previousInner, nextOuter, color, previousOuter, nextInner);
                previousOuter = nextOuter;
                previousInner = nextInner;
            }
        }

        private static void AddLine(VertexHelper vh, Vector2 start, Vector2 end, float width, Color color)
        {
            var direction = (end - start).normalized;
            if (direction.sqrMagnitude < 0.0001f) return;
            var side = new Vector2(-direction.y, direction.x) * (width * 0.5f);
            AddQuad(vh, start - side, end + side, color);
        }

        private static void AddRing(VertexHelper vh, Vector2 center, float outer, float inner, Color color, int segments)
        {
            var start = vh.currentVertCount;
            for (var i = 0; i <= segments; i++)
            {
                var angle = Mathf.PI * 2f * i / segments;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                AddVert(vh, center + direction * outer, color);
                AddVert(vh, center + direction * inner, color);
                if (i == 0) continue;
                var previous = start + (i - 1) * 2;
                var current = start + i * 2;
                vh.AddTriangle(previous, current, previous + 1);
                vh.AddTriangle(previous + 1, current, current + 1);
            }
        }

        private static void AddCircle(VertexHelper vh, Vector2 center, float radius, Color color, int segments)
        {
            var centerIndex = vh.currentVertCount;
            AddVert(vh, center, color);
            for (var i = 0; i <= segments; i++)
            {
                var angle = Mathf.PI * 2f * i / segments;
                AddVert(vh, center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius, color);
                if (i > 0) vh.AddTriangle(centerIndex, centerIndex + i, centerIndex + i + 1);
            }
        }

        private static void AddQuad(VertexHelper vh, Vector2 min, Vector2 max, Color color)
        {
            AddQuad(vh, min, new Vector2(min.x, max.y), color, new Vector2(max.x, min.y), max);
        }

        private static void AddQuad(VertexHelper vh, Vector2 a, Vector2 b, Color color, Vector2 c, Vector2 d)
        {
            var start = vh.currentVertCount;
            AddVert(vh, a, color);
            AddVert(vh, b, color);
            AddVert(vh, d, color);
            AddVert(vh, c, color);
            vh.AddTriangle(start, start + 1, start + 2);
            vh.AddTriangle(start + 2, start + 3, start);
        }

        private static void AddPolygon(VertexHelper vh, Vector2[] points, Color color)
        {
            if (points == null || points.Length < 3) return;
            var start = vh.currentVertCount;
            for (var i = 0; i < points.Length; i++) AddVert(vh, points[i], color);
            for (var i = 1; i < points.Length - 1; i++) vh.AddTriangle(start, start + i, start + i + 1);
        }

        private static Vector2 P(Vector2 center, float scale, float x, float y)
        {
            return center + new Vector2(x * scale, y * scale);
        }

        private static void AddVert(VertexHelper vh, Vector2 position, Color color)
        {
            var vertex = UIVertex.simpleVert;
            vertex.position = position;
            vertex.color = color;
            vh.AddVert(vertex);
        }
    }
}
