# V1.0 Android candidate evidence — 2026-08-23

This record covers the offline Android-first candidate built from the current V1
working tree. It is evidence for a release-shaped prototype, not Play Store approval.

## Source and toolchain

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Source commit: `ab5b12ad7c86f425243fc3f2a9cbc83ae97e6f6d`
  (`art: add original offline V1 visual kit`); the artifacts were
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
| PlayMode | Passed with evidence: 64/64 | `C:\Projects\BattleRaja-v1-verify-20260823c\Builds\V1\TestResults\playmode-visual-kit-run.xml` |
| Source diff hygiene | Passed with evidence: `git diff --check` clean | local command output |

## Android artifacts

The builds were run in the disposable copy
`C:\Projects\BattleRaja-v1-verify-20260823c`, so Unity scene generation did not
rewrite the working tree.

### Release-shaped APK used for Lava performance and interaction checks

- Path: `C:\Projects\BattleRaja-v1-verify-20260823c\Builds\V1\Android\BattleRaja-V1.0-release-candidate.apk`
- Size: **40,420,983 bytes**
- SHA-256: `E70241D83E6DBDA977EECF9F476502FD68B89799438DBA06F024423D575E5532`
- Install: succeeded on `ST5GDW23LB004392`
- Package/version observed on device: `com.example.battleraja.m11`, version name `1.0.0`,
  version code `100`, target SDK 36, minimum SDK 28
- This APK was built with `BuildOptions.None`, so it has no Development Build label. It is
  still debug-signed and uses a temporary package identity; it is not publishable.

### Debug-signed release-shaped AAB

- Path: `C:\Projects\BattleRaja-v1-verify-20260823c\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab`
- Size: **36,251,072 bytes**
- SHA-256: `4B22FD2DADD26FB1A5FEA96FE5EAA19BC2D0EC4F130F87009969D38562FE84C6`
- Bundle inspection: base manifest present; 8 ARM64 native libraries; 0 other ABIs;
  450 archive entries; all eight ARM64 ELF libraries have `0x4000` LOAD alignment
  (static 16 KB check passed)
- Signing/package identity: intentionally not release-ready; the temporary package ID
  uses Unity's Android Debug certificate and no owner-approved release keystore was used

## Lava runtime evidence

Inspected captures are stored outside the repository at
`C:\Projects\BattleRaja-v1-verify-20260823c\Builds\V1\Lava\`:

- `visual-kit-menu.png`: branded offline menu with Play Offline, Tutorial Replay, Settings &
  Accessibility, and Help & Controls
- `visual-kit-mode.png`: solo offline mode route
- `visual-kit-fighters.png`: three-fighter selection with distinct fighter glyphs
- `visual-kit-match.png`, `visual-kit-combat.png` and `visual-kit-active.png`: Bazaar Bastion
  opening/active match, eight actors, HUD, controls and distinct arena/fighter silhouettes
  square underlays
- `visual-kit-moved.png`: a touch movement probe showing the player moved through the arena
- `visual-kit-logcat.txt`, `visual-kit-logcat-combat.txt` and `visual-kit-meminfo.txt`: bounded
  device samples

The device log had no application `FATAL EXCEPTION`, `AndroidRuntime` crash, Unity exception or
missing-reference marker in the captured application slice. Android `gfxinfo` exposed only
the Unity SurfaceView/render-node summary for this final APK, without a frame/jank histogram;
therefore no exact-current FPS or stable frame-pacing pass is claimed here.

## Measured device sample

Raw files are outside source under the same Lava evidence directory:

- `visual-kit-gfxinfo.txt` — Unity's SurfaceView did not expose a frame/jank histogram
  in this sample; no FPS pass is claimed from this file.
- `visual-kit-meminfo.txt` — **285,509 KB PSS**, **421,336 KB RSS**,
  **99,160 KB Graphics**, **79 KB swap**.
- `visual-kit-logcat.txt` and `visual-kit-logcat-combat.txt` — application log slices used for
  crash-marker review; no fatal application markers were found.

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
- Gadget pickup/use remains a human-facing evidence gap; the source-backed pickup and station
  identities are covered by PlayMode regression and render in the match, but a successful
  collection/use moment was not captured on Lava in this pass.
- Photon Fusion and PlayFab remain deliberately out of scope for this offline V1 build.
