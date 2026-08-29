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
mocked, and the current copies contain no development-build label. The older
runtime package used for these captures is no longer retained outside the
repository; the screenshot files remain at `Assets/BattleRaja/Art/V1/` and must
be refreshed and re-reviewed against the current candidate before upload.
Both packages are debug-signed local candidates with temporary package ID
`com.example.battleraja.m11`; they are not upload-ready.
These store screenshot candidates predate the tutorial action-gate source
`65e6001` and must be refreshed and re-reviewed against the final signed AAB
before upload.
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

## Exact current gameplay screenshot evidence - 2026-08-30

The latest technical screenshot set was captured from the exact d0de949 APK on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). Files remain in the ignored local
evidence folder `Builds/Local/Device/Performance/20260830-lava-d0de949-aim/`; the companion
`manifest.json` records every filename and SHA-256. This set supersedes the older screenshot
copies for source verification but is still **technical evidence, not approved listing art**.

| Route | Current evidence | Status |
| --- | --- | --- |
| Menu, Solo Raja and fighter selection | `menu.png`, `mode.png`, `fighter-select.png`, `fighter-pehel.png`, `fighter-bijli.png` | Exact temporary-ID runtime; owner crop/art review required |
| Live combat and Aandhi | `live-opening.png`, `combat-actions.png`, `midmatch.png`, `aandhi-final.png` | Exact runtime; presentation/fun review required |
| Elimination, spectator, results and rematch | `postmatch.png`, `postmatch-2.png`, `rematch.png` | Exact runtime; winner/placement and copy review required |
| Settings, accessibility toggles and lifecycle | `pause-settings.png`, `settings-toggles.png`, `settings-restored.png`, `match-after-settings.png`, `lifecycle-before-home.png`, `lifecycle-resume.png` | Exact runtime; owner comfort/accessibility review required |
| Tutorial | `tutorial-start.png`, movement/aim/attack evidence, `tutorial-skip-current.png`, `menu-after-tutorial-current.png` | `8/8 COMPLETE` observed; action-by-action comfort and full victory route remain open |

The generated feature graphic/icon and these screenshots must not be presented as final
commissioned/culturally approved artwork. The package ID remains temporary and debug-signed;
the owner must select final branding, signing, privacy/Data Safety answers, rating and Play
Console materials before upload.

## Exact terminal-outcome candidate screenshots - 2026-08-30

For source traceability, a fresh screenshot set was captured from the exact `5d136fb`
temporary-ID APK on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34).
The ignored evidence folder is
`Builds/Local/Device/Performance/20260830-lava-5d136fb-outcome/`; its `manifest.json` records
the APK/AAB, route, telemetry and screenshot hashes. The set includes menu, Solo Raja, all
three fighter cards, live/action feedback, Aandhi pressure/closing/final circle, defeat and
spectating, results/rematch, settings toggles, background/resume and tutorial `8/8 COMPLETE`
via the in-app SKIP control.

These are technical evidence candidates, not approved listing art. The player reached results
at placement #6 in this route, so no physical victory presentation was observed; the winner
and non-winner terminal cues are covered by the exact PlayMode regression. The package remains
debug-signed with temporary ID `com.example.battleraja.m11`. Owner approval is still required
for final crops, visual quality, cultural safety, accessibility, copy, privacy/Data Safety,
rating, package identity/signing and Play Console use.

### Tracked current-candidate copies

The following copies are tracked under `Docs/Store/V1/` so the owner can review candidates
without relying on ignored build evidence. Each is an unmodified PNG from the exact Lava route
(SHA-256 values match the manifest):

| Intended listing use | Tracked candidate | SHA-256 |
|---|---|---|
| Menu | `Docs/Store/V1/BattleRaja-V1-Lava-5d136fb-Menu.png` | `217984A80310452CDE4C0BBD804B255509376BAA47D01483CF5A28FEEB0EED43` |
| Fighter selection | `Docs/Store/V1/BattleRaja-V1-Lava-5d136fb-FighterSelection.png` | `90F6750AD276150607A0D466F3421471928F92EB80E55FAE89F11EE309B57912` |
| Combat / ability / gadget | `Docs/Store/V1/BattleRaja-V1-Lava-5d136fb-CombatActions.png` | `E4458ED5CD0CE6A346FB6F656F9D7FBC692BB664EBB475E304644EC2403D8DFC` |
| Aandhi pressure | `Docs/Store/V1/BattleRaja-V1-Lava-5d136fb-Aandhi.png` | `2706C7C74B7F1E17F4075DA167C95CDD6DB2534F2018B20F06B71571FF0670A8` |
| Results | `Docs/Store/V1/BattleRaja-V1-Lava-5d136fb-Results.png` | `926367A3D5C5250867B1D95AD73AD5846AE4D3414A4560B658B7A2F112C035FF` |
| Settings / accessibility | `Docs/Store/V1/BattleRaja-V1-Lava-5d136fb-Settings.png` | `3EE1868598C408A8A0A959116874375ADFD5A2F8E0F38D7E12D030BFD93A769B` |

These tracked PNGs are technical candidates only. The owner must approve crops, text/UI
readability, cultural safety, accessibility, final AAB identity and listing use before any
submission.
