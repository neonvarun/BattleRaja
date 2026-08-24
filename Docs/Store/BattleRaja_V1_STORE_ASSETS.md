# BattleRaja V1 Store Assets

Status: **candidate assets; owner review required**
Product scope: offline Android V1.0, no login, no ads, no IAP, no online play.

## Asset inventory

| Asset | File | Source/evidence | Intended use | Status |
|---|---|---|---|---|
| Feature graphic | `V1/BattleRaja-V1-FeatureGraphic-1024x500.png` | Original generated art, no third-party source | Google Play feature graphic | Candidate; human visual/cultural approval required |
| Menu screenshot | `V1/BattleRaja-V1-Lava-Menu.png` | Real Lava capture from exact HEAD `062066b` release-shaped APK | Phone screenshot | Technical evidence; crop/annotation review required |
| Fighter selection | `V1/BattleRaja-V1-Lava-FighterSelection.png` | Real Lava capture from exact HEAD `062066b` release-shaped APK | Phone screenshot | Technical evidence; human review required |
| Live match | `V1/BattleRaja-V1-Lava-Match.png` | Real Lava capture with Bazaar, HUD, Aandhi and fighters from exact HEAD `062066b` | Phone screenshot | Technical evidence; human review required |
| Results | `V1/BattleRaja-V1-Lava-Results.png` | Real Lava results/rematch capture from exact HEAD `062066b` | Phone screenshot | Technical evidence; human review required |

The screenshots are copied from actual runtime captures; no gameplay state was
mocked, and the current copies contain no development-build label. The exact
runtime-source `062066b` APK used for these captures is
`C:\BRS\Builds\V1\Android\BattleRaja-V1.0-release-candidate.apk`
(SHA-256 `C7B16D01DEA3ED3ADA1B5E5AA421B82ADBA46F5E1A0A2B0283F409BC59F3E245`).
The matching runtime-source AAB is
`C:\BRS\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab`
(SHA-256 `4D3948F876580AC45A0655593DAA6FE4AF70BC9BACF78840F33EC63E8775E858`).
Both packages are debug-signed local candidates with temporary package ID
`com.example.battleraja.m11`; they are not upload-ready.
The feature graphic is marketing artwork depicting the same fictional
Bazaar Bastion setting and three fighters, but it is not a screenshot and must
not be presented as in-game capture.

## Feature graphic provenance

The feature graphic was generated for BattleRaja using an original prompt for a
stylized toy-box bazaar arena with fictional cyan, saffron and violet fighters.
It intentionally excludes text, logos, sacred/political symbols, recognizable
third-party characters, gore and watermarks. The source prompt and generated
output remain in the local Codex generated-art directory; the normalized
1024×500 RGB PNG is the repository candidate.

## Proposed store copy

Short description (under 80 characters):

> Survive the Aandhi in a fast, offline toy-box battle royale.

Draft full description:

> Drop into Bazaar Bastion for a colourful offline battle royale built for
> quick sessions. Choose Bijli, Pehel or Maya, outplay seven bots, read the
> closing Aandhi, and use Umbrella Guard, Dhol Burst or Tiffin Station to stay
> alive. Learn the loop in the tutorial, spectate after elimination, then
> rematch or return to the menu. No login and no internet connection are
> required.

This copy deliberately makes no online, account, ranking, monetisation or
performance claim. Final title, branding, cultural wording, age rating and
privacy/legal text require owner review.

## Required owner checks before upload

- Approve final title/package identity and feature-graphic art direction.
- Confirm every screenshot still represents the exact release AAB.
- Review cultural safety, fighter names, copy and violence presentation.
- Confirm privacy policy/Data Safety answers match the shipped permissions and
  SDK inventory.
- Supply release signing configuration and approve Play Console submission.
