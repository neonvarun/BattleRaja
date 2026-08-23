"""Generate original, editable source-backed BattleRaja store graphics.

The drawings intentionally use simple geometric toy-box forms rather than any third-party
game art. Runtime gameplay screenshots are captured separately from the real Android build.
"""

from pathlib import Path
from PIL import Image, ImageDraw, ImageFont


ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / "Assets" / "BattleRaja" / "Art" / "V1"
OUT.mkdir(parents=True, exist_ok=True)


def font(size: int, bold: bool = False):
    candidates = [
        Path("C:/Windows/Fonts/segoeuib.ttf" if bold else "C:/Windows/Fonts/segoeui.ttf"),
        Path("C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf"),
    ]
    for candidate in candidates:
        if candidate.exists():
            return ImageFont.truetype(str(candidate), size)
    return ImageFont.load_default()


def poly(draw, points, fill):
    draw.polygon(points, fill=fill)


def draw_shield(draw, center, scale):
    x, y = center
    outer = [(x - 0.78 * scale, y - 0.72 * scale), (x + 0.78 * scale, y - 0.72 * scale),
             (x + 0.92 * scale, y + 0.02 * scale), (x, y + 1.05 * scale),
             (x - 0.92 * scale, y + 0.02 * scale)]
    inner = [(x - 0.62 * scale, y - 0.56 * scale), (x + 0.62 * scale, y - 0.56 * scale),
             (x + 0.72 * scale, y + 0.03 * scale), (x, y + 0.82 * scale),
             (x - 0.72 * scale, y + 0.03 * scale)]
    poly(draw, outer, "#F6A928")
    poly(draw, inner, "#08202B")
    bolt = [(x - 0.16 * scale, y - 0.58 * scale), (x + 0.30 * scale, y - 0.08 * scale),
            (x + 0.06 * scale, y - 0.08 * scale), (x + 0.34 * scale, y + 0.55 * scale),
            (x - 0.08 * scale, y + 0.12 * scale), (x - 0.34 * scale, y + 0.12 * scale)]
    poly(draw, bolt, "#48D9E8")
    poly(draw, [(x + 0.10 * scale, y - 0.48 * scale), (x + 0.48 * scale, y - 0.04 * scale),
                 (x + 0.20 * scale, y - 0.04 * scale), (x + 0.02 * scale, y + 0.44 * scale),
                 (x - 0.14 * scale, y + 0.04 * scale), (x - 0.38 * scale, y + 0.04 * scale)], "#FFD45A")


def draw_token(draw, center, radius, color, style):
    x, y = center
    draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill="#07151E")
    draw.ellipse((x - radius * 0.78, y - radius * 0.78, x + radius * 0.78, y + radius * 0.78), fill=color)
    if style == 0:
        poly(draw, [(x - radius * 0.25, y - radius * 0.48), (x + radius * 0.42, y - radius * 0.35),
                    (x + radius * 0.02, y + radius * 0.58), (x - radius * 0.18, y + radius * 0.10)], "#FFD45A")
    elif style == 1:
        draw.ellipse((x - radius * 1.02, y - radius * 0.20, x - radius * 0.42, y + radius * 0.38), fill="#FFD45A")
        draw.ellipse((x + radius * 0.42, y - radius * 0.20, x + radius * 1.02, y + radius * 0.38), fill="#FFD45A")
    else:
        poly(draw, [(x - radius * 0.72, y - radius * 0.24), (x + radius * 0.72, y - radius * 0.24),
                    (x + radius * 0.38, y + radius * 0.56), (x, y + radius * 0.82),
                    (x - radius * 0.38, y + radius * 0.56)], "#7BE6BC")


def make_icon():
    scale = 4
    image = Image.new("RGBA", (512 * scale, 512 * scale), "#07151E")
    draw = ImageDraw.Draw(image)
    draw.rounded_rectangle((24 * scale, 24 * scale, 488 * scale, 488 * scale), radius=82 * scale, fill="#0A3440", outline="#F6A928", width=10 * scale)
    draw.ellipse((74 * scale, 74 * scale, 438 * scale, 438 * scale), fill="#0D5360", outline="#43D1C4", width=6 * scale)
    draw_shield(draw, (256 * scale, 236 * scale), 142 * scale)
    draw.rectangle((106 * scale, 418 * scale, 406 * scale, 442 * scale), fill="#F06A2F")
    image.resize((512, 512), Image.Resampling.LANCZOS).save(OUT / "BattleRaja-AppIcon-PlayStore.png", optimize=True)


def make_feature_graphic():
    scale = 2
    width, height = 1024 * scale, 500 * scale
    image = Image.new("RGB", (width, height), "#07151E")
    draw = ImageDraw.Draw(image)
    # A warm/cool toy-box market horizon.
    draw.rectangle((0, int(height * 0.60), width, height), fill="#5A3D32")
    for x in range(-80, width + 80, 160):
        draw.polygon([(x, height), (x + 120, int(height * 0.50)), (x + 240, height)], fill="#65473A")
    for x, color in [(20, "#F06A2F"), (220, "#3DD1C4"), (420, "#F6A928"), (620, "#A957D5"), (820, "#3DD1C4")]:
        draw.rectangle((x * scale, 280 * scale, (x + 120) * scale, 330 * scale), fill=color)
        draw.polygon([(x * scale, 280 * scale), ((x + 60) * scale, 238 * scale), ((x + 120) * scale, 280 * scale)], fill="#F8D17C")
    draw.rounded_rectangle((330 * scale, 180 * scale, 694 * scale, 414 * scale), radius=28 * scale, fill="#0B4C55", outline="#F6A928", width=8 * scale)
    draw.rectangle((500 * scale, 118 * scale, 524 * scale, 181 * scale), fill="#F6A928")
    draw_shield(draw, (512 * scale, 255 * scale), 72 * scale)
    draw_token(draw, (170 * scale, 332 * scale), 48 * scale, "#36D4F0", 0)
    draw_token(draw, (850 * scale, 330 * scale), 48 * scale, "#F06A2F", 1)
    draw_token(draw, (510 * scale, 430 * scale), 44 * scale, "#A957D5", 2)
    draw.text((42 * scale, 35 * scale), "BATTLE RAJA", font=font(64 * scale, True), fill="#FFFFFF", stroke_width=2 * scale, stroke_fill="#07151E")
    draw.text((46 * scale, 112 * scale), "OFFLINE TOY-BOX BATTLE ROYALE", font=font(24 * scale, True), fill="#7BE6BC")
    draw.text((46 * scale, 438 * scale), "BAZAAR BASTION  •  1 RAJA + 7 RIVALS", font=font(20 * scale, True), fill="#FFD45A")
    image.resize((1024, 500), Image.Resampling.LANCZOS).save(OUT / "BattleRaja-FeatureGraphic-PlayStore.png", optimize=True)


if __name__ == "__main__":
    make_icon()
    make_feature_graphic()
    print(f"Generated {OUT / 'BattleRaja-AppIcon-PlayStore.png'}")
    print(f"Generated {OUT / 'BattleRaja-FeatureGraphic-PlayStore.png'}")
