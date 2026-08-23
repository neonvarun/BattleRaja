# V1.0 Android candidate evidence — 2026-08-23

This record covers the offline Android-first candidate built from the current V1
working tree. It is evidence for a release-shaped prototype, not Play Store approval.

## Source and toolchain

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Source commit: `24feb5d7c08301ebb44548fbd0f10ffe78b6e9ec`
  (`ui: polish offline android candidate presentation`); the artifacts were
  built from that exact committed change set in the disposable verification copy.
- This evidence document is a follow-up documentation commit; the runtime artifact
  source remains the code commit above.
- Unity: `6000.5.6f1` (`0e0577a1a2ac`)
- Android target/minimum: API 36 / API 28
- Scripting backend/ABI: IL2CPP / ARM64
- Approved device: Lava `ST5GDW23LB004392` (`LAVA_LXX508`); the emulator and Oppo phone
  were not used for this evidence.

## Automated gates

| Gate | Result | Evidence |
| --- | --- | --- |
| Repository validation | Passed with evidence: 0 errors, 0 warnings | `Tools/Validation/validate.ps1 -RequireUnityProject -UnityExe ...6000.5.6f1...` |
| EditMode | Passed with evidence: 125/125 | `Builds/V1/TestResults/editmode-v1-final.xml` |
| PlayMode | Passed with evidence: 63/63 | `Builds/V1/TestResults/playmode-release-polish-final.xml` |
| Source diff hygiene | Passed with evidence: `git diff --check` clean | local command output |

## Android artifacts

The builds were run in the disposable copy
`C:\Projects\BattleRaja-v1-verify-20260823b`, so Unity scene generation did not
rewrite the working tree.

### Release-shaped APK used for Lava performance and interaction checks

- Path: `C:\Projects\BattleRaja-v1-verify-20260823b\Builds\V1\Android\BattleRaja-V1.0-release-candidate.apk`
- Size: **40,430,947 bytes**
- SHA-256: `1699EA241EA9BC85985F05A4EB1BC0C24854CF96571685F6AF51744312DD6E46`
- Install: succeeded on `ST5GDW23LB004392`
- Package/version observed on device: `com.example.battleraja.m11`, version name `1.0.0`,
  version code `100`, target SDK 36, minimum SDK 28
- This APK was built with `BuildOptions.None`, so it has no Development Build label. It is
  still debug-signed and uses a temporary package identity; it is not publishable.

### Debug-signed release-shaped AAB

- Path: `C:\Projects\BattleRaja-v1-verify-20260823b\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab`
- Size: **36,261,037 bytes**
- SHA-256: `ADD545042DD2397EDE9B7908C9C7BE3954F4E5232500315E6662F36B9C64B0D9`
- Bundle inspection: base manifest present; 8 ARM64 native libraries; 0 other ABIs;
  450 archive entries; all eight ARM64 ELF libraries have `0x4000` LOAD alignment
  (static 16 KB check passed)
- Signing/package identity: intentionally not release-ready; the temporary package ID
  uses Unity's Android Debug certificate and no owner-approved release keystore was used

## Lava runtime evidence

Inspected captures are stored outside the repository at
`C:\Projects\BattleRaja-v1-verify-20260823b\Builds\V1\Lava\`:

- `release-final-menu.png`: branded offline menu with Play Offline, Tutorial Replay, Settings &
  Accessibility, and Help & Controls
- `release-final-mode.png`: solo offline mode route
- `release-final-fighters.png`: three-fighter selection with distinct vector identities
- `release-final-match.png`: Bazaar Bastion opening, eight actors, HUD, controls and no
  square underlays
- `release-final-gfxinfo.txt`, `release-final-meminfo.txt`, `release-final-top.txt`,
  `release-final-thermal.txt` and `release-final-logcat.txt`: bounded device samples

The device log had no application `FATAL EXCEPTION`, `AndroidRuntime` crash, Unity exception or
missing-reference marker in the captured application slice. Android `gfxinfo` exposed only
the Unity SurfaceView/render-node summary for this final APK, without a frame/jank histogram;
therefore no exact-current FPS or stable frame-pacing pass is claimed here.

## Measured device sample

Raw files are outside source under the same Lava evidence directory:

- `release-final-gfxinfo.txt` — Unity's SurfaceView did not expose a frame/jank histogram
  in this sample; no FPS pass is claimed from this file.
- `release-final-meminfo.txt` — **284,282 KB PSS**, **420,364 KB RSS**,
  **99,896 KB Graphics**, **77 KB swap**.
- `release-final-top.txt` — one sample at approximately **81.8% CPU** for the
  process.
- `release-final-thermal.txt` — thermal status 0; current-HAL CPU/GPU about 42.7 C,
  skin about 39.4 C and battery about 35 C.
- `release-final-logcat.txt` — application log slice used for crash-marker review.

These measurements keep the candidate classified as a prototype: frame pacing, single-sample
CPU data, repeated-match behavior, longer thermal/battery series and human performance review
still require deliberate follow-up.

## Known release blockers

- Final application ID, branding, signing/upload key and Play App Signing path are not
  approved.
- Adaptive icon configuration still emits Unity's legacy-icon deprecation warning.
- Static 16 KB checks passed for this exact AAB: the bundle checker found all eight
  ARM64 ELF libraries at `0x4000` alignment for every LOAD segment. A
  runtime install on a dedicated 16 KB-page Android environment remains open.
- The package is debug-signed/non-publishable and has not been uploaded to Play Console.
- Store copy, data-safety declaration, content rating, privacy/legal and cultural review
  remain human gates.
- Final tutorial completion, results/rematch, settings/accessibility and repeated-match
  cleanup need explicit human observation on the exact candidate.
- Photon Fusion and PlayFab remain deliberately out of scope for this offline V1 build.
