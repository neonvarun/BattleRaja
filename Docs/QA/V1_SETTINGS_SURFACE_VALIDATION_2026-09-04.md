# BattleRaja V1 settings-surface validation — 2026-09-04

## Scope

This continuation addresses a player-facing readability gap in the menu and in-match
pause settings. It adds original render-only setting glyphs/accent rails and makes every
accessibility toggle expose its current `ON/OFF` state. Existing uGUI button targets,
local preference keys, lifecycle pause behavior, common input commands, authority,
replay and collision are unchanged.

## Source and remote

- Branch: `codex/v1-playstore-release`
- Source checkpoint: `67e2cbc79367c37729d5051bb0dacf537b83cf0b`
- This evidence note is committed separately; the final synchronized `HEAD` is recorded
  by the completion report and remote verification.
- Remote update: fast-forward push only; worktree is clean after the evidence note commit.

## Changed source

- `Assets/BattleRaja/Presentation/UI/BattleRajaSettingsGlyph.cs` and `.meta`
- `Assets/BattleRaja/Presentation/UI/BattleRajaUiTheme.cs`
- `Assets/BattleRaja/Presentation/Flow/ProductionFlowController.cs`
- `Assets/BattleRaja/Presentation/Match/OfflineMatchHud.cs`
- `Assets/BattleRaja/Tests/PlayMode/OfflineMatchPlayModeTests.cs`
- `Docs/DECISIONS.md` ADR-085, `Docs/ASSET_PROVENANCE.md`,
  `Docs/PRODUCT_COMPLETION_STATUS.md`, `Docs/QA/CURRENT_STATE.md`,
  `Docs/RELEASE/V1_ANDROID_RELEASE_CHECKLIST.md`, `Docs/V1_RELEASE_PLAN.md` and
  `PROJECT_STATUS.md`.

The glyph is a repository-owned procedural `MaskableGraphic`; it does not use a
third-party icon pack, emoji font or reference-game asset. It is raycast-disabled, and
the parent `Button` remains the sole interaction target.

## Verification

Commands:

```powershell
pwsh -File Tools/Validation/validate.ps1 -RequireUnityProject -UnityExe `
  'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe'
pwsh -File Tools/Validation/run_unity_tests.ps1 -UnityExe `
  'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe' `
  -ProjectRoot C:\Projects\BattleRaja -TestPlatform editmode
pwsh -File Tools/Validation/run_unity_tests.ps1 -UnityExe `
  'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe' `
  -ProjectRoot C:\Projects\BattleRaja -TestPlatform playmode
pwsh -File Tools/Build/Android/build.ps1 -ProjectRoot C:\Projects\BattleRaja `
  -UnityExe 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe' `
  -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidV1ReleaseCandidateApk
pwsh -File Tools/Build/Android/build.ps1 -ProjectRoot C:\Projects\BattleRaja `
  -UnityExe 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe' `
  -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidV1ReleaseCandidate
pwsh -File Tools/Validation/check_v1_release_candidate.ps1 `
  -ProjectRoot C:\Projects\BattleRaja -ExpectedPackageId com.example.battleraja.m11 `
  -RequireCleanWorktree ...
```

Results:

- Static validation: **0 errors / 0 warnings**.
- EditMode: **159/159 passed**; XML SHA-256 `F8C9BF60F77873E9D906E2D0E8726946A58B8593C36CCCD845C086234A85914C`.
- PlayMode: **96/96 passed**; XML SHA-256 `3BE1BA5E2EA8887C4D52DC8F11AE3FD0D64921DB3960409DE30AF9080AAAAC4B`.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,678,776 bytes,
  SHA-256 `714ACE23E8C9DA859B91B14E12F9E7E65CA277ADAAAB315F1C81B4547D195C93`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,504,322 bytes,
  SHA-256 `74F7EDC96481EA868FF1A8F078E70D6C407126AD9A197BF2020D044F108445CC`.
- Clean checker: `Builds/Local/V1GameplayTruth/Next/release-checker-settings-polish-final-final-20260904.log`,
  SHA-256 `E473003AB9CD043A637AE878B01772609EABB28A7AC7DA6F23156C7127522FF4`.
- Checker: package `com.example.battleraja.m11`, version `1.0.0` / code `100`, min/target
  API `28/36`, no network permissions, seven ARM64 libraries, static `0x4000` load
  alignment and 512x512 / 1024x500 store-creative dimensions.

## Lava evidence

Approved device only: `ST5GDW23LB004392`, Lava LXX508, Android 14/API 34, 1080x2460,
4 KB pages. The exact APK was installed and the pulled base at
`Builds/Local/V1GameplayTruth/Next/settings-polish-final-20260904/installed-base.apk`
matches the APK hash.

Fresh captures are under
`Builds/Local/V1GameplayTruth/Next/settings-polish-final-20260904/`:

- `menu.png` — SHA-256 `06D6A4EC804C9C5CD70100E0A76720C65BBC64ADC3689B5577433EFC28A7484F`.
- `menu-settings.png` — SHA-256 `40A74B30114834E5BED1A8DF71BBC79749B43B6451DA2B4A703353BEB76C51FF`.
- `live.png` — SHA-256 `F45876D8AC529C5C4D26139AFA128AC6130841319BFCE66E2DD158082C69631E`.
- `pause-settings.png` — SHA-256 `2BD526B723F06409816F8375836A2AB042FD9138C1D0D74C59A1430CB6A269D8`.
- `pause-settings-high-contrast.png` — SHA-256 `D8EAB1A5A8D4883BB258532BB5E2EBADFE4A80C9BB828FD74284C70BC430514D`.
- `logcat.txt` — 0 configured fatal markers; SHA-256 `7CF2DB871C88DA85562485E7D72ACF323C60D6E27E0EB4A0BE2603416694AD36`.

The menu and pause screenshots show explicit setting states and original icon-backed
tiles. The high-contrast probe toggled the setting on and back off without a crash.

## Bounded live performance

The exact live sample is under
`Builds/Local/V1GameplayTruth/Next/performance-settings-polish-final-20260904/`;
`metrics-live.json` SHA-256 is recorded by the directory artifacts. Six samples over 30
seconds reported:

- PSS: **298,369–304,625 KB**
- RSS: **417,808–424,064 KB**
- Graphics PSS: **89,324–95,480 KB**
- Raw app CPU: **106–127%**, mean **116.3%**
- Battery: **63% → 63%**
- Thermal status: **0**
- Scoped logcat: **0** configured fatal markers

Android `gfxinfo` exposed no frame timing data. This is bounded device evidence, not a
normalized Unity GPU/GC/endurance or physical 16 KB runtime approval.

## Truthful classification and remaining gates

**Prototype — Android offline release candidate in progress.** The technical source,
offline packaging and this settings/readability route are verified, but final commissioned
art/audio, full physical all-fighter/tutorial/accessibility/lifecycle comfort, normalized
GPU/GC/endurance, physical 16 KB runtime, permanent package identity/signing, privacy/Data
Safety/IARC, cultural review and Play submission remain owner-controlled or unverified.
