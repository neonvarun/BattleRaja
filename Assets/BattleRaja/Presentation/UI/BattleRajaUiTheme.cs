using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Small, code-driven visual language shared by the offline product surfaces.
    /// Keeping the palette here prevents the menu, tutorial and in-match HUD from
    /// drifting into unrelated prototype styles while still leaving every surface
    /// replaceable by authored art later.
    /// </summary>
    public static class BattleRajaUiTheme
    {
        public static readonly Color Ink = new Color(0.035f, 0.055f, 0.085f, 1f);
        public static readonly Color Surface = new Color(0.055f, 0.090f, 0.135f, 0.98f);
        public static readonly Color SurfaceRaised = new Color(0.085f, 0.145f, 0.205f, 0.98f);
        public static readonly Color Cyan = new Color(0.18f, 0.82f, 0.93f, 1f);
        public static readonly Color Saffron = new Color(1f, 0.56f, 0.16f, 1f);
        public static readonly Color Magenta = new Color(0.86f, 0.34f, 0.76f, 1f);
        public static readonly Color Gold = new Color(1f, 0.82f, 0.18f, 1f);
        public static readonly Color Mint = new Color(0.30f, 0.92f, 0.64f, 1f);
        public static readonly Color Green = new Color(0.38f, 0.92f, 0.52f, 1f);
        public static readonly Color Danger = new Color(1f, 0.30f, 0.28f, 1f);
        public static readonly Color Text = new Color(0.95f, 0.98f, 1f, 1f);
        public static readonly Color MutedText = new Color(0.65f, 0.76f, 0.84f, 1f);

        public static Font DefaultFont => Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        public static void StyleText(Text text, int size, TextAnchor alignment, Color? color = null, bool bold = false)
        {
            if (text == null) return;
            text.font = DefaultFont;
            text.fontSize = size;
            text.alignment = alignment;
            text.color = color ?? Text;
            text.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
        }

        public static void StylePanel(GameObject panel, Color? color = null)
        {
            if (panel == null) return;
            var image = panel.GetComponent<Image>();
            if (image != null)
            {
                image.color = color ?? Surface;
                image.raycastTarget = false;
            }

            var outline = panel.GetComponent<Outline>() ?? panel.AddComponent<Outline>();
            outline.effectColor = new Color(Cyan.r, Cyan.g, Cyan.b, 0.16f);
            outline.effectDistance = new Vector2(2f, -2f);

            var shadow = panel.GetComponent<Shadow>() ?? panel.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.30f);
            shadow.effectDistance = new Vector2(0f, -6f);
        }

        public static void StyleButton(Button button, bool primary = false, bool danger = false)
        {
            if (button == null) return;

            StyleButton(button, danger ? Danger : primary ? Saffron : Cyan, primary);
        }

        public static void StyleButton(Button button, Color accent, bool primary = false)
        {
            if (button == null) return;
            var image = button.GetComponent<Image>();
            if (image == null) return;

            // The primary action must read immediately on a busy game surface. A
            // brighter accent and a grounded shadow give it the tactile weight of a
            // finished mobile game while secondary actions remain quieter.
            var normal = Color.Lerp(SurfaceRaised, accent, primary ? 0.52f : 0.12f);
            var highlighted = Color.Lerp(normal, accent, 0.28f);
            var pressed = Color.Lerp(normal, Ink, 0.22f);
            image.color = normal;
            image.raycastTarget = true;

            var colors = button.colors;
            colors.normalColor = normal;
            colors.highlightedColor = highlighted;
            colors.pressedColor = pressed;
            colors.selectedColor = highlighted;
            colors.disabledColor = new Color(normal.r, normal.g, normal.b, 0.40f);
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.08f;
            button.colors = colors;
            button.targetGraphic = image;

            var outline = button.GetComponent<Outline>() ?? button.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(accent.r, accent.g, accent.b, primary ? 0.70f : 0.40f);
            outline.effectDistance = new Vector2(2f, -2f);

            var shadow = button.GetComponent<Shadow>() ?? button.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, primary ? 0.42f : 0.28f);
            shadow.effectDistance = new Vector2(0f, -6f);

            var label = button.GetComponentInChildren<Text>(true);
            if (label != null)
            {
                var settingsTile = button.transform.Find("SettingsGlyph") != null;
                StyleText(label, label.fontSize > 0 ? label.fontSize : 18,
                    settingsTile ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter, Text, true);
            }
        }

        /// <summary>
        /// Styles a preference tile as a product surface rather than a plain debug
        /// button. The icon and accent rail are render-only children, so the button
        /// keeps a single, unambiguous pointer target and remains keyboard/switch
        /// accessible.
        /// </summary>
        public static void StyleSettingsButton(
            Button button,
            BattleRajaSettingsGlyph.Kind kind,
            Color accent,
            bool enabled)
        {
            if (button == null) return;
            StyleButton(button, accent, false);

            var label = button.GetComponentInChildren<Text>(true);
            if (label != null)
            {
                label.rectTransform.anchorMin = new Vector2(0.19f, 0.06f);
                label.rectTransform.anchorMax = new Vector2(0.78f, 0.94f);
                label.rectTransform.offsetMin = new Vector2(4f, 2f);
                label.rectTransform.offsetMax = new Vector2(-4f, -2f);
                label.alignment = TextAnchor.MiddleLeft;
                label.fontSize = Mathf.Min(label.fontSize, 17);
            }

            var rail = button.transform.Find("SettingsAccentRail") as RectTransform;
            if (rail == null)
            {
                var railObject = new GameObject("SettingsAccentRail", typeof(RectTransform), typeof(Image));
                railObject.transform.SetParent(button.transform, false);
                rail = railObject.GetComponent<RectTransform>();
                rail.SetAsFirstSibling();
            }

            rail.anchorMin = new Vector2(0f, 0.10f);
            rail.anchorMax = new Vector2(0.026f, 0.90f);
            rail.offsetMin = Vector2.zero;
            rail.offsetMax = Vector2.zero;
            var railImage = rail.GetComponent<Image>();
            railImage.color = new Color(accent.r, accent.g, accent.b, enabled ? 0.90f : 0.48f);
            railImage.raycastTarget = false;

            var glyph = button.transform.Find("SettingsGlyph")?.GetComponent<BattleRajaSettingsGlyph>();
            if (glyph == null)
            {
                var glyphObject = new GameObject("SettingsGlyph", typeof(RectTransform), typeof(BattleRajaSettingsGlyph));
                glyphObject.transform.SetParent(button.transform, false);
                glyphObject.transform.SetAsLastSibling();
                glyph = glyphObject.GetComponent<BattleRajaSettingsGlyph>();
            }

            var glyphRect = glyph.rectTransform;
            glyphRect.anchorMin = new Vector2(0.045f, 0.18f);
            glyphRect.anchorMax = new Vector2(0.16f, 0.82f);
            glyphRect.offsetMin = Vector2.zero;
            glyphRect.offsetMax = Vector2.zero;
            glyph.Configure(kind, accent, enabled);
        }

        public static void SetSettingsState(Button button, bool enabled)
        {
            if (button == null) return;
            var glyph = button.transform.Find("SettingsGlyph")?.GetComponent<BattleRajaSettingsGlyph>();
            glyph?.SetEnabled(enabled);
            var rail = button.transform.Find("SettingsAccentRail")?.GetComponent<Image>();
            if (rail != null)
            {
                var color = rail.color;
                color.a = enabled ? 0.90f : 0.48f;
                rail.color = color;
            }
        }

        /// <summary>
        /// Applies the accessibility contrast treatment without relying on a colour
        /// being the only state signal. The normal palette is restored deterministically
        /// when the setting is turned off.
        /// </summary>
        public static void ApplyContrast(Transform root, bool highContrast)
        {
            if (root == null) return;
            foreach (var image in root.GetComponentsInChildren<Image>(true))
            {
                if (image == null) continue;
                if (image.gameObject.name == "SettingsAccentRail") continue;
                var button = image.GetComponent<Button>();
                if (button != null)
                {
                    var primary = image.gameObject.name.IndexOf("Start", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                  image.gameObject.name.IndexOf("Offline", StringComparison.OrdinalIgnoreCase) >= 0;
                    StyleButton(button, primary);
                    if (highContrast)
                    {
                        image.color = Color.Lerp(Color.black, Color.white, 0.18f);
                        var colors = button.colors;
                        colors.normalColor = image.color;
                        colors.highlightedColor = Color.white;
                        colors.pressedColor = new Color(0.72f, 0.72f, 0.72f, 1f);
                        colors.selectedColor = Color.white;
                        button.colors = colors;
                    }
                    continue;
                }

                var isPanel = image.gameObject.name.EndsWith("Panel", StringComparison.Ordinal) ||
                              image.gameObject.name.IndexOf("Card", StringComparison.OrdinalIgnoreCase) >= 0;
                image.color = highContrast
                    ? (isPanel ? Color.black : Color.white)
                    : (isPanel ? Surface : SurfaceRaised);
            }

            foreach (var text in root.GetComponentsInChildren<Text>(true))
            {
                if (text == null) continue;
                text.color = highContrast ? Color.white : Text;
            }
        }
    }
}
