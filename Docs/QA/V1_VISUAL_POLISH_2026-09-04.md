# V1 visual polish evidence — 2026-09-04

## Scope and classification

This checkpoint records a bounded presentation pass on the offline Android release
candidate. It improves the readability of the three original fighter silhouettes and
uses the existing Bazaar Bastion feature artwork as a full menu-card background. The
changes are render-only: authority, colliders, hitboxes, match rules, replay state and
input contracts were not changed.

The product remains **Prototype — Android offline release candidate in progress**.
This is an editable procedural art baseline, not commissioned final art or a store-
approved release.

## Source and changed assets

Source commits:

- `775497d` — `art: polish fighter silhouettes and menu presentation`
- `281eeb4` — `build: persist crystal accent material keyword`

Presentation/editor changes:

- `Assets/BattleRaja/Editor/ProductionArtBuilder.cs` adds saved faceted shoulder,
  gauntlet and leg-armor meshes and assigns distinct fighter silhouettes.
- `Assets/BattleRaja/Editor/ProductionPresentationBuilder.cs` attaches leg armor to
  the existing render-only foot rig.
- `Assets/BattleRaja/Presentation/Flow/ProductionFlowController.cs` expands the
  feature-art anchor to the full menu card with an envelope crop while preserving the
  controls and safe-area layout.
- `Assets/BattleRaja/Tests/PlayMode/VerticalSlicePlayModeTests.cs` asserts the
  silhouette mesh family is present on every production fighter.

Generated/saved assets:

- `Assets/BattleRaja/Content/Art/V1/Meshes/GauntletPlate.asset`
- `Assets/BattleRaja/Content/Art/V1/Meshes/LegArmor.asset`
- `Assets/BattleRaja/Content/Art/V1/Meshes/ShoulderPlate.asset`
- `Assets/BattleRaja/Content/Prefabs/Production/BijliProduction.prefab`
- `Assets/BattleRaja/Content/Prefabs/Production/MayaProduction.prefab`
- `Assets/BattleRaja/Content/Prefabs/Production/PehelProduction.prefab`
- `Assets/BattleRaja/Content/Art/V1/Materials/Crystal.mat`

The generated prefab YAML is large because Unity reserialized nested prefab instances;
`git diff --check` passed and no gameplay component or collider was intentionally
changed.

## Automated validation

- Static repository validation: **0 errors / 0 warnings**.
- Full EditMode: **164/164 passed**. Final menu-layout XML:
  `Builds/Local/V1GameplayTruth/Next/visual-polish-20260904/editmode-menu.xml`,
  SHA-256 `5D43596B6246CFF916B83A43459728537CBADD5B8947DB33CABE37F7EBE5D5DF`.
- Full PlayMode: **98/98 passed**. XML SHA-256
  `7F827E33249211F45469CF14596653A912286DC24B3A2ACEC885752CC5E48AD6`.
- The production-art regression verifies that Bijli, Pehel and Maya each expose the
  faceted shoulder and leg-armor render meshes.
- The strict 100-match bot/replay evidence and authority/respawn regressions remain
  indexed in `Docs/QA/V1_RESPAWN_HANDOFF_AUTHORITY_2026-09-04.md`; this art pass does
  not change those gameplay results.

## Android candidate

Built with Unity `6000.5.6f1` using the V1 release-candidate entry points:

- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,681,892 bytes,
  SHA-256 `739742F5D401F5B02F9213B71EA78EA2C41F8F66E9D78792AE3430ED84DD0A7B`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,507,386 bytes,
  SHA-256 `4BE9951BDAA517ED01F2D35472D41FA415A919063BA1A976BF1ADA5EF357DD94`.

The technical release checker passed with **0 errors / 0 warnings** for temporary
package `com.example.battleraja.m11`, version `1.0.0` / code `100`, min/target API
`28/36`, offline permissions, seven ARM64 libraries, static ELF alignment checks and
store icon/feature dimensions. No final package identity or signing decision was made.

## Approved Lava evidence

Device: Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 1080x2460). The
exact APK installed successfully; the pulled base APK at
`Builds/Local/V1GameplayTruth/Next/visual-polish-20260904/lava/base.apk` is 41,681,892
bytes and has the same SHA-256 as the candidate above. Package dump reports version
`1.0.0`, code `100`, minSdk `28` and targetSdk `36`.

Visual captures from that exact install:

- `menu-final.png` — full Bazaar feature artwork fills the menu card behind the
  controls; SHA-256 `C65B20CB5C4DC3128B3785FE39472ADD51CC194B548A1702A21A10370CD8B466`.
- `fighter-select-final.png` — the three cards show distinct angular shoulders and
  readable colored legs; SHA-256
  `33107FF0E2C9278F092E47BD12246262F99E64945F71DEA9F67A5295F0ECAE46`.
- `live-final.png` — the Bastion Crown HUD and all three fighter family silhouettes
  are visible at gameplay scale; SHA-256
  `7F01DE771AD41A658DE918B717CDF6C435FB26EA3C2DB9DA8C556286161C4756`.

The scoped logcat at
`Builds/Local/V1GameplayTruth/Next/visual-polish-20260904/lava/logcat.txt` contains
zero configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or `SIGABRT` markers (SHA-256
`DBA4708E80E520CAE8C43DBF156B8962354ECD8492C7A78E2D8E1BA3F2B82877`). Lava reports
4 KB pages; this is not genuine 16 KB runtime proof.

### Current-source all-fighter route continuation

The same installed APK was relaunched on the approved Lava and exercised through the
real menu → Bastion Crown → fighter-selection route. The route folder is
`Builds/Local/V1GameplayTruth/Next/visual-polish-20260904/lava/all-fighters/`.

- Pehel was selected in `03-pehel-selected.png` (SHA-256
  `0F3EE01F3A1FE298714C2E8736120B88511E7946F62B76D453261C8FD12DAD85`) and opened
  with the `PEHEL • ANCHOR` live HUD in `04-pehel-live.png` (SHA-256
  `062730D5C3E746B55AC29300ED4EF22C14156D93C1D995AB1DC93DD95F2595F3`). The later
  gadget probe visibly reports `TIFFIN STATION DEPLOYED` in `07-pehel-gadget.png`
  (SHA-256 `3548B5D1D3B0F63118E4A5225B9DFE3EB812180A1AD6E5347ABBD363CE9C476E`).
- Maya was selected in `12-maya-selected.png` (SHA-256
  `2CE703EC436774CDD0355A33C1E27431CF9ED0D4B417D83438F88443253BF1D2`) and opened
  with the `MAYA • RUNNER` live HUD in `13-maya-live.png` (SHA-256
  `73E2406A64FC7290009C37F2C5783D4BA41B922E5426CD9344251E0A32DED760`). The
  follow-up sequence includes a live decoy/pressure frame and a visible Tiffin
  deployment in `16-maya-gadget.png` (SHA-256
  `FCD595B76919E5E3698C4783ACB2C0B0691FCBD4B091F22C554DE95272BDAB6A`).
- The route logcat `route-logcat.txt` has zero configured fatal/ANR/SIGSEGV/SIGABRT
  markers (SHA-256
  `28BA1C10B1B5285C1ADAC803F7F695057ED5F0B833B70EACA4C9B753E034FB35`).

This supplements the Bijli route and confirms current-source fighter-specific selection,
HUD identity and a gadget-use visual on device. The Pehel attack/ability probe was
captured after that actor had already been eliminated, so the sequence is not claimed as
full all-fighter action or lifecycle comfort; human touch, accessibility, combat-effect
and results/rematch review remain open.

## Genuine 16 KB runtime smoke

The exact candidate APK also installed and launched on the local
`BattleRaja_16K` Android 16/API 36 AVD (`sdk_gphone16k_x86_64`) while the emulator's
airplane-mode setting was enabled. `adb shell getconf PAGESIZE` returned **16384** and
the ABI list was `x86_64,arm64-v8a`; the package dump reports version `1.0.0`, code
`100`, minSdk `28` and targetSdk `36`.

- Menu capture: `Builds/Local/V1GameplayTruth/Next/16k-visual-polish-20260904/menu-final.png`,
  2,082,962 bytes, SHA-256
  `7C30C7668FFA09AA528F5C4F75F7683F732D89EFF085FD87184468AFE567354E`.
- App-scoped launch log: `.../16k-visual-polish-20260904/logcat-final.txt`, 783,564
  bytes, SHA-256
  `148DFECE57401813D28CDFFF56C19B6ACC72054CBC5314FD51CA300D1751E795`; configured
  crash/native/shader markers: **0**.

This is emulator evidence for the static 16 KB-compatible candidate and does not replace
physical Lava evidence, broader device coverage, normalized performance or final human
release approval.

## Bounded performance diagnostic

The 30-second capture is under
`Builds/Local/V1GameplayTruth/Next/performance-visual-polish-20260904/` and contains
six samples with no configured fatal markers. Settled process observations were PSS
248–259 MB, RSS 371–382 MB, graphics PSS 75–80 MB and instantaneous `top` CPU samples
39–62%; thermal status remained 0. Unity did not expose a usable frame-time histogram
through `dumpsys gfxinfo`, so this capture makes no normalized FPS, GC, GPU or sustained
endurance claim.

## Remaining gates

This checkpoint does not close commissioned final art/audio, full current-source
physical lifecycle coverage, accessibility/cultural/fun review, normalized sustained
performance, genuine 16 KB runtime evidence, final branding/package/signing or owner
Play submission approvals. The prior bounded Lava route also should not be silently
treated as proof of every KO, respawn, spectator and results transition for this exact
art candidate.

Repository validation for the published documentation/art tip `ec063d2` completed
successfully as GitHub run `33870986803` (run #96). The branch was fast-forwarded to
`origin/main` and the worktree was clean after publication.
