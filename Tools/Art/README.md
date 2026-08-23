# BattleRaja source-backed store creatives

`generate_store_creatives.py` creates the current original draft icon and feature graphic
from simple geometric shapes and system fonts. It does not download or embed third-party
game art, logos, characters, screenshots or sounds.

Generated drafts:

- `Assets/BattleRaja/Art/V1/BattleRaja-AppIcon-PlayStore.png` — 512x512 launcher draft.
- `Assets/BattleRaja/Art/V1/BattleRaja-FeatureGraphic-PlayStore.png` — 1024x500 feature
  graphic draft.

These are source-backed technical candidates, not owner-approved final store assets. Human
branding, legal, cultural and Play Console review remains required before publication.

Regenerate locally with:

```powershell
python Tools/Art/generate_store_creatives.py
pwsh -File Tools/Validation/check_store_creative.ps1
```
