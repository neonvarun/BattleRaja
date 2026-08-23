# V1.0 Android candidate evidence — 2026-08-23

This record covers the offline Android-first candidate built from the current V1
working tree. It is evidence for a release-shaped prototype, not Play Store approval.

## Source and toolchain

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Source commit: `1896c138484149e774bd23bee5ecd4b1064852da`
  (`v1: establish offline Android release candidate`); the artifacts were built from
  that exact committed change set in the disposable verification copy.
- Final branch tip after evidence-only documentation/validator updates:
  `607fa9fe7666e884cb12ea8a774d40453bc98b69`.
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
| PlayMode | Passed with evidence: 61/61 | `Builds/V1/TestResults/playmode-v1-final.xml` |
| Source diff hygiene | Passed with evidence: `git diff --check` clean | local command output |

## Android artifacts

The builds were run in the disposable copy
`C:\Projects\BattleRaja-v1-verify-20260823b`, so Unity scene generation did not
rewrite the working tree.

### Development APK used for Lava

- Path: `C:\Projects\BattleRaja-v1-verify-20260823b\Builds\M11\Android\BattleRaja-BazaarBastion-M11.apk`
- Size: **166,221,332 bytes**
- SHA-256: `9A93BF85AC5CD557C5DC1E1A166B99F18137F2D45F5EDDE5F6C4790F5F13F5F7`
- Install: succeeded on `ST5GDW23LB004392`
- Package/version observed on device: `com.example.battleraja.m11`, version name `1.0.0`,
  version code `100`, target SDK 36, minimum SDK 28

### Debug-signed release-shaped AAB

- Path: `C:\Projects\BattleRaja-v1-verify-20260823b\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab`
- Size: **36,250,956 bytes**
- SHA-256: `78CFF3B021B41A2194E9F961D506CC376A92D8D24AC3D7ECFD4CE258976645EC`
- Bundle inspection: base manifest present; 8 ARM64 native libraries; 0 other ABIs;
  450 archive entries
- Signing/package identity: intentionally not release-ready; the temporary package ID
  uses Unity's Android Debug certificate and no owner-approved release keystore was used

## Lava runtime evidence

Inspected captures are stored outside the repository at
`C:\Projects\BattleRaja-v1-verify-20260823b\Builds\V1\Lava\`:

- `menu-final.png`: branded offline menu with Play Offline, Tutorial Replay, Settings &
  Accessibility, and Help & Controls
- `menu-authoritative.png`: fresh post-build menu capture from the latest APK install
- `fighter-latest.png`: three-fighter selection with persisted-selection behavior
- `match-final.png`: Bazaar Bastion opening, eight actors, HUD, circular twin-stick and
  action controls without square underlays
- `combat-final.png`: movement swipe plus attack/ability interaction; active match and
  Aandhi warning visible

The device log had no `FATAL EXCEPTION`, `AndroidRuntime` crash, Unity exception or
missing-reference marker in the captured application slice. The runtime frame cadence
was visibly around 30 FPS in the device buffer queue during the sample; this is not a
60-FPS pass.

## Measured device sample

Raw files are outside source under the same Lava evidence directory:

- `gfxinfo-final.txt` — Unity's native `ViewRootImpl` reported no Android jank histogram
  for the SurfaceView.
- `meminfo-final.txt` — **462,618 KB PSS**, **593,576 KB RSS**, **99,988 KB Graphics**,
  **75 KB swap**.
- `top-final.txt` — one sample at approximately **96.9% CPU** for the process.
- `thermal-final.txt` — thermal thresholds were readable, but no stable temperature
  series was captured.
- `logcat-final.txt` — application log slice used for crash-marker review.

These measurements keep the candidate classified as a prototype: memory, CPU, frame
pacing, thermal/battery and repeated-match behavior still require a deliberate owner
performance review and optimization pass.

## Known release blockers

- Final application ID, branding, signing/upload key and Play App Signing path are not
  approved.
- Adaptive icon configuration still emits Unity's legacy-icon deprecation warning.
- Static 16 KB checks passed for this exact AAB: `zipalign -c -P 16` succeeded and all
  eight ARM64 ELF libraries reported `0x4000` alignment for every LOAD segment. A
  runtime install on a dedicated 16 KB-page Android environment remains open.
- The package is debug-signed/non-publishable and has not been uploaded to Play Console.
- Store copy, data-safety declaration, content rating, privacy/legal and cultural review
  remain human gates.
- Final tutorial completion, results/rematch, settings/accessibility and repeated-match
  cleanup need explicit human observation on the exact candidate.
- Photon Fusion and PlayFab remain deliberately out of scope for this offline V1 build.
