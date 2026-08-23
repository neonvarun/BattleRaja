# V1.0 Android candidate evidence — 2026-08-23

This record covers the offline Android-first candidate built from the current V1
working tree. It is evidence for a release-shaped prototype, not Play Store approval.

## Source and toolchain

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Source commit: `b22cfe34ffa1401d89acd5ebf93aef83b4cea9a6`
  (`android: add release-shaped performance validation target`); the artifacts were
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
| PlayMode | Passed with evidence: 61/61 | `Builds/V1/TestResults/playmode-v1-final.xml` |
| Source diff hygiene | Passed with evidence: `git diff --check` clean | local command output |

## Android artifacts

The builds were run in the disposable copy
`C:\Projects\BattleRaja-v1-verify-20260823b`, so Unity scene generation did not
rewrite the working tree.

### Release-shaped APK used for Lava performance and interaction checks

- Path: `C:\Projects\BattleRaja-v1-verify-20260823b\Builds\V1\Android\BattleRaja-V1.0-release-candidate.apk`
- Size: **40,418,923 bytes**
- SHA-256: `629C8BA2E7F3C2B4A4911D32A72E0957EE7564C0783A415E4C6617C21F105FC9`
- Install: succeeded on `ST5GDW23LB004392`
- Package/version observed on device: `com.example.battleraja.m11`, version name `1.0.0`,
  version code `100`, target SDK 36, minimum SDK 28
- This APK was built with `BuildOptions.None`, so it has no Development Build label. It is
  still debug-signed and uses a temporary package identity; it is not publishable.

### Debug-signed release-shaped AAB

- Path: `C:\Projects\BattleRaja-v1-verify-20260823b\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab`
- Size: **36,249,028 bytes**
- SHA-256: `C6D19FCB9FFDF1FC525371CDBD751732F2EE738E00F9F09C259D15DEAD756D1B`
- Bundle inspection: base manifest present; 8 ARM64 native libraries; 0 other ABIs;
  450 archive entries; all eight ARM64 ELF libraries have `0x4000` LOAD alignment
  (static 16 KB check passed)
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
- `release-60fps-match2.png`: active Pehel match from the release-shaped APK with eight
  actors, HUD, controls and no Development Build label

The device log had no `FATAL EXCEPTION`, `AndroidRuntime` crash, Unity exception or
missing-reference marker in the captured application slice. The release-shaped APK's
buffer-queue sample reported consecutive one-second windows around **59.47–60.62 FPS**;
one transient window fell to **45.31 FPS** with a 272 ms maximum frame, so this is a
bounded near-60 observation and not a stable performance pass.

## Measured device sample

Raw files are outside source under the same Lava evidence directory:

- `release-60fps-match2-frame-log.txt` — buffer-queue frame windows around 59.47–60.62
  FPS with one 45.31 FPS transient hitch.
- `release-60fps-match2-meminfo.txt` — **279,283 KB PSS**, **414,748 KB RSS**,
  **95,544 KB Graphics**, **64 KB swap**.
- `release-60fps-match2-top.txt` — one sample at approximately **81.8% CPU** for the
  process.
- `release-60fps-match2-thermal.txt` — thermal status 0; sampled CPU/GPU about 42.4 C,
  skin about 39.2 C and battery about 35 C.
- `release-60fps-match2-logcat.txt` — application log slice used for crash-marker review.

These measurements keep the candidate classified as a prototype: the one-frame hitch,
single CPU sample, repeated-match behavior, longer thermal/battery series and human
performance review still require deliberate follow-up.

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
