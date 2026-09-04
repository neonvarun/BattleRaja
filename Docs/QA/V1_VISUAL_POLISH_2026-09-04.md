# V1 visual polish evidence — 2026-09-04

## Scope and classification

This checkpoint records a bounded presentation pass on the offline Android release
candidate. It improves the readability of the three original fighter silhouettes and
uses the existing Bazaar Bastion feature artwork as a full menu-card background. The
changes are render-only: colliders, hitboxes, match rules, replay state and input
contracts were not changed. A follow-up adapter correction mirrors the confirmed
post-respawn authority snapshot into the visible actor and health card; it does not alter
the Bastion domain rules.

The product remains **Prototype — Android offline release candidate in progress**.
This is an editable procedural art baseline, not commissioned final art or a store-
approved release.

## Source and changed assets

Source commits:

- `775497d` — `art: polish fighter silhouettes and menu presentation`
- `281eeb4` — `build: persist crystal accent material keyword`
- `4ebf65f` — `fix: mirror confirmed Bastion respawns to views`

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
- Full PlayMode: **99/99 passed** after the respawn handoff regression. XML SHA-256
  `67E02CE34AFB41222D56A8A4633392FDC9716CB8A7D40AD4D1228A34669D93B4`.
- The production-art regression verifies that Bijli, Pehel and Maya each expose the
  faceted shoulder and leg-armor render meshes.
- The new `BastionRespawnHandoffMirrorsConfirmedHealthAndSpectatorState` regression
  verifies defeat → out-of-action spectator state → confirmed respawn restores the
  participant, visible max health and non-spectating player state.
- The strict 100-match bot/replay evidence and authority/respawn regressions remain
  indexed in `Docs/QA/V1_RESPAWN_HANDOFF_AUTHORITY_2026-09-04.md`; this art pass does
  not change those gameplay results.

## Android candidate

Built with Unity `6000.5.6f1` using the V1 release-candidate entry points:

- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,683,648 bytes,
  SHA-256 `6A16D07EBA66C7420E5F1AABD7982E27C40C6BB017FC639E2D87974B85DE60DC`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,509,156 bytes,
  SHA-256 `337C15FF7169A97FED2F711822C5366BF731A388D954C8778A9BF33A9E4DB9DA`.

The post-commit technical release checker passed with **0 errors / 0 warnings**. The
checker log is `Builds/Local/V1GameplayTruth/Next/respawn-fix-20260904/release-checker-final.log`
(SHA-256 `647D3B48D0F1C9C86FA626F48E62D0AAFB1497450E4555058D70BCDED107E4E5`); it
verified the temporary package `com.example.battleraja.m11`, version `1.0.0` / code
`100`, min/target API `28/36`, offline permissions, seven ARM64 libraries, static ELF
alignment checks, store dimensions and a clean worktree. No final package identity or
signing decision was made.

## Approved Lava evidence

Device: Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 1080x2460). The
corrected exact APK installed successfully; the pulled base APK at
`Builds/Local/V1GameplayTruth/Next/visual-polish-20260904/lava/respawn-fix-route/base.apk`
is 41,683,648 bytes and has the same SHA-256 as the corrected candidate above. Package dump reports version
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

### Current-source full-route lifecycle continuation

The same exact APK was then exercised on Lava through a second real-touch route. The
selected fighter persisted as Maya from the preceding selection run, so this is a
Maya route rather than a fresh Bijli route. The route folder is
`Builds/Local/V1GameplayTruth/Next/visual-polish-20260904/lava/full-route/`.

- Menu → Bastion Crown → fighter selection → live match was reached with all eight
  fighters visible. The route toggled left-handed controls, reduced flashes, high
  contrast and aim assist in the in-match settings panel, then returned to the live
  match. `14-home-background.png` records the Android launcher after HOME and
  `15-live-after-resume.png` records the same live match after relaunch; this is
  lifecycle evidence, not a product screenshot.
- The first match reached a real terminal card in `16-results.png` (SHA-256
  `05482C7E49613E0862F53EBE024D33ECD60A462EB8DCE27DE8D5B07AFAFA6344`). It visibly
  reports `BASTION CROWN RESULTS`, `WINNER RIVAL • Clock`, Raja 6/15 with two
  deposits, zero KOs and seven tickets, and Rival 11/15 with two deposits and five
  KOs. This proves a current-source authoritative results presentation on the
  installed art candidate.
- Tapping the visible REMATCH control reset the match to `00:04` with Maya at 85/85,
  both teams at 0/15 and all eight fighters rendered in `17-rematch-live.png` (SHA-256
  `D066EDF78B2136E1D8323BE6A16D632B231F3413D984EBF9D5136711FCA5EA54`). A second
  terminal poll at `21-rematch-poll.png` (SHA-256
  `FFF603C9EA6D2F479B255FE0BFF0E639B9B372A8C709259DA9958AA30B6A226D`) again showed
  the Results card, confirming the rematch run can resolve normally.
- Real touch taps were sent to attack, ability and gadget during the rematch. The
  attack/ability frames (`18-rematch-attack.png`, SHA-256
  `CED7A50DB9DCE9FE190A8F1A44D62852225B2B2C11A7AFD98D0D1C85BB6CB306`; and
  `19-rematch-ability.png`, SHA-256
  `360353B33B5ADAB49EFF94BA809DBD2D5FEF86E8DC847657CE054698A51E0913`) do not expose
  a reliable player-facing counter, so action success is not claimed from those two
  stills. The gadget frame visibly reports `tiffin station deployed` in
  `20-rematch-gadget.png` (SHA-256
  `85CAA5AA150A13C217DB54CDF0C647E004F03730021758A05E993322E8403987`).
- The earlier `07-spectator.png` still showed the actor at 1/85 after the tap, so it is
  retained as post-spectate/respawn context rather than explicit proof of the
  out-of-action spectator card. The corrected respawn route below now supplies the
  explicit player-death card; only the spectate-camera interaction and action-by-action
  comfort review remain open.

The completed route logcat, including the rematch taps, is
`.../lava/full-route/route-logcat-complete.txt` (1,744,914 bytes; SHA-256
`D1DFAEA650E308EBDC1A268F645DD73D0BA251271C111C470D087ACB9DA34A30`). It contains
zero configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV`, `SIGABRT` or shader-marker
matches. This continuation strengthens current-source lifecycle evidence but does
not close spectate-camera interaction, all-fighter action coverage, commissioned art/audio,
normalized performance, physical 16 KB runtime, or owner release approvals.

### Corrected Bastion respawn handoff route

After the physical route exposed a stale zero-health mirror on respawn, source `4ebf65f`
was rebuilt into the corrected candidate above. The exact APK was reinstalled on Lava
and exercised through a focused Bijli Bastion route. The route folder is
`Builds/Local/V1GameplayTruth/Next/visual-polish-20260904/lava/respawn-fix-route/`.

- The route reached a terminal Results card in `respawn-fix-results.png` (SHA-256
  `D7FF9468984A2B1162739BD6978712F89CB26E37411CCD8A2826DB110D4EBC34`).
- During the rematch, `respawn-fix-out-of-action.png` (SHA-256
  `0EB4249EFDC3BFFD6E87531F1E8B2184CB065342616CADAE62D712C3A1C12B27`) visibly shows
  Bijli at `0/85` with `OUT OF ACTION • respawn or spectate an ally`.
- The later `respawn-fix-respawned.png` (SHA-256
  `429E18CA0ABD3CCB1244B8C430EA0255C1C37809607CCDD92E651FA9242CB75B`) shows the same
  player card restored to `85/85` after the authority-confirmed respawn. This is the
  physical counterpart to the new PlayMode regression; no ticket or health mirror is
  inferred from the screenshot alone.

The pulled installed base APK is `base.apk` in that folder (41,683,648 bytes; SHA-256
`6A16D07EBA66C7420E5F1AABD7982E27C40C6BB017FC639E2D87974B85DE60DC`). The route
logcat `route-logcat.txt` is 2,319,622 bytes (SHA-256
`0B16E9E55FC91EF3871970E520BD8DFC9F74E5D31E720E9777D87117DC4AADE0`) with zero
configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV`, `SIGABRT`, shader-error,
`NullReferenceException` or `UnityException` markers. A deliberate tap-through of the
spectate-camera branch was not captured before that short run terminated, so physical
camera-follow comfort remains open; the domain/UI handoff is covered by the regression.

## Genuine 16 KB runtime smoke

The corrected candidate APK also installed and launched on the local
`BattleRaja_16K` Android 16/API 36 AVD (`sdk_gphone16k_x86_64`) while the emulator's
airplane-mode setting was enabled. `adb shell getconf PAGESIZE` returned **16384** and
the ABI list was `x86_64,arm64-v8a`; the package dump reports version `1.0.0`, code
`100`, minSdk `28` and targetSdk `36`.

- Menu capture: `Builds/Local/V1GameplayTruth/Next/respawn-fix-20260904/16k/menu-02.png`,
  2,082,962 bytes, SHA-256
  `7C30C7668FFA09AA528F5C4F75F7683F732D89EFF085FD87184468AFE567354E`.
- App-scoped launch log: `Builds/Local/V1GameplayTruth/Next/respawn-fix-20260904/16k/logcat-app.txt`,
  18,115 bytes, SHA-256
  `E09D039100386F9096D1773BB84267992465EBABE34D746B4A56E357AA6B7282`; configured
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

This checkpoint does not close commissioned final art/audio, spectate-camera interaction
comfort, complete all-fighter action coverage, accessibility/cultural/fun review,
normalized sustained performance, genuine 16 KB runtime, final branding/package/signing
or owner Play submission approvals. The route now proves a current-source terminal
Results card, rematch reset and background/resume path, but it should not be silently
treated as proof of every KO, respawn, spectator and action transition for this exact
art candidate.

Repository validation for the published documentation/art tip `ec063d2` completed
successfully as GitHub run `33870986803` (run #96). The branch was fast-forwarded to
`origin/main` and the worktree was clean after publication.
