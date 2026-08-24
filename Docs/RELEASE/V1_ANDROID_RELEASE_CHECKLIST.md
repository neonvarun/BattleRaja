# BattleRaja V1.0 Android release checklist

This is the offline Android release-candidate gate for BattleRaja. It deliberately does not
start Photon, PlayFab, accounts, ads, IAP, cloud progression or Web release work.

## Candidate scope

- Unity `6000.5.6f1`, URP, ARM64, IL2CPP.
- Android target API 36, minimum API 28.
- One local human plus seven deterministic bots in Bazaar Bastion.
- Bijli, Pehel and Maya; Umbrella Guard, Dhol Burst and Tiffin Station; Aandhi; tutorial;
  spectator; results; rematch; local settings.
- No account, online room or server-owned progression is used by the offline candidate.
  The exact offline packaging candidate removes `INTERNET` and
  `ACCESS_NETWORK_STATE` from the APK while retaining the future-facing Fusion files
  outside the Android runtime. Final signed-bundle inspection is still required before
  Play submission.

## Exact current source cleanup — `e808830` — 2026-08-24

The current source uses cached actor views for Pehel authority-result presentation
instead of a scene-wide target scan. Validation is **0/0**, EditMode **125/125**
and PlayMode **71/71**. Fresh release-shaped packages from `C:\BRHotpathAndroid`
are APK `2404F4BB2EB3AAA08ED8B92CA3F658F6127F70C1D197F46D8AF1511720803271`
(39,485,171 bytes) and AAB
`8FEF3AF9BEC7DB0F5C809B52F49B953D600EE5CCE7D680B268575B5A60E29C70`
(35,312,622 bytes). The AAB is ARM64-only and passed static 16 KB alignment;
installation/launch on Lava succeeded, but the lock screen blocked interaction.
Manifest inspection reports temporary package `com.example.battleraja.m11`,
version `1.0.0` / code `100`, min SDK 28, target/compile SDK 36, and only
`VIBRATE` plus Unity's dynamic receiver permission; `INTERNET` and
`ACCESS_NETWORK_STATE` are absent. Signing, package identity, device,
performance and Play approval gates remain open.

## Exact current candidate — docs `be0c510` / source `1d743b0` / runtime `d96d3f2` — 2026-08-24

- Validation: **0 errors / 0 warnings**; EditMode **125/125**; PlayMode **71/71**.
- Release-shaped APK: **39,486,559 bytes**, SHA-256
  `89156306717C5EB27EE193AD1D46809DFE19159112ADC3C77008D4C6A3C89DE0`.
- Release-shaped AAB: **35,313,996 bytes**, SHA-256
  `F5776F6AF19EE1C0A803D76050A80E62E883710E00296EF9603CED279D2227C1`.
- AAB: 7 ARM64 native libraries, 0 other ABIs, static 16 KB alignment passed.
- Lava `ST5GDW23LB004392`: exact APK installation succeeded; launch resolved to
  the Unity activity, but the active lock screen blocked interactive menu,
  tutorial and match QA. No physical route claim is made from this run.
- The Android lifecycle pause guard is automated-test covered; sustained
  performance and human background/resume review remain open.

## Latest exact-current evidence — `6ac5c12` — 2026-08-24

- Validation: **0 errors / 0 warnings**; EditMode **125/125**; PlayMode **67/67**.
- Release-shaped APK: **39,523,632 bytes**, SHA-256
  `09F5375FA8D5DEC066A09D8CCDF0BAF01269F4B402252EF2908691C773402EF3`.
- Release-shaped AAB: **35,351,357 bytes**, SHA-256
  `70825F82A4D79E1E036F4DA8A286778244406D51B1D60A568BD066ED1B82DAA8`.
- AAB: 7 ARM64 native libraries, no other ABIs, and all checked ELF LOAD segments
  aligned to `0x4000`.
- Lava `ST5GDW23LB004392`: release-shaped APK installed and launched to the branded
  menu; runtime action bindings are explicit, but the Unity surface exposes no
  actionable UI nodes, so physical touch route and sustained performance review remain open.
- Web: exact source built and served over local HTTP; Chrome/Edge reached the Unity
  loader only in bounded headless captures, so Web is not a release gate.

## Current release gates

| Gate | Current state | Owner action |
| --- | --- | --- |
| Product/package identity | Blocked: build entrypoint still uses `com.example.battleraja.m11` | Approve final application ID and branding |
| Signing | Not started | Approve upload key/Play App Signing path; never commit the key |
| Target API | Configured to API 36 | Recheck against current Play policy at upload time |
| 64-bit | Passed with evidence for the current debug-signed AAB: 7 ARM64 libraries, 0 other ABIs | Re-run inspection after any package/plugin change |
| 16 KB pages | Static evidence passed: zipalign `-P 16` and all eight ARM64 ELF LOAD segments at `0x4000`; runtime 16 KB environment still open | Re-run the checker after any package/plugin change and install on a 16 KB Android environment when available |
| Permissions | **Passed for the exact debug APK**: `VIBRATE` and Unity's dynamic-receiver permission only; no `INTERNET`, `ACCESS_NETWORK_STATE` or SD-card permission | Recheck the final signed AAB/APK and document any future online permission change |
| Device QA | Release-shaped launch/menu smoke passed on Lava (`ST5GDW23LB004392`); full touch, accessibility, battery and thermal review open | Owner performs touch, accessibility, battery and thermal review |
| Store/legal | Draft only | Approve privacy, data-safety, content rating, cultural and legal copy |
| Play Console | Not started | Owner creates the app and decides rollout/release track |

## Latest local candidate evidence (2026-08-24)

### Exact offline packaging hardening

The final documentation checkout is `HEAD` on `codex/v1-playstore-release`;
runtime/package source is `f4425d6`. Validation is
**0 errors / 0 warnings**; EditMode is
**125/125** and PlayMode is **66/66**. The fresh release-shaped APK is **39,529,326
bytes** (SHA-256
`AE74717B597C4CBCFDECF7D8DB719C177100F495CC084ABFD0E1EA6AAD3E2C52`) and the AAB is
**35,357,477 bytes** (SHA-256
`8EB49EFC8D58D144E5A792224FC9A3570FF4E37F121E06B6E55093C9D4D5F5E7`). The AAB
contains 7 ARM64 libraries, no other ABIs, and passed static 16 KB ELF alignment.

`aapt dump permissions` and Lava `dumpsys package` show `VIBRATE` plus Unity's
dynamic-receiver permission only; `INTERNET` and `ACCESS_NETWORK_STATE` are absent.
The APK installed and launched only on Lava `ST5GDW23LB004392`, with the branded
offline menu visible and no fatal/ANR/SIGSEGV marker. Raw captures are recorded in
`Docs/QA/V1_ANDROID_OFFLINE_PACKAGING_2026-08-24.md`.

### Exact current checkout artifact

Current branch tip: `357dfdf1e6289c172dab60e514f555ba3d5bc914`; runtime-equivalent build
source: `46724ac2dfa403f40f58669240e61918c2a94d1b`.

- Exact validation: **0 errors / 0 warnings**.
- APK: **40,431,927 bytes**, SHA-256
  `0694958A43F1BADD30E697095F249733992F9D6904E10E1923CD0CAF01010C78`.
- AAB: **36,262,036 bytes**, SHA-256
  `906D85FA00E4A9787A0C1DE892DC3F27A098ACF21BB1735E08C977565A1D09A4`.
- AAB checker: 8 ARM64 libraries, 0 other ABIs, all checked native LOAD segments at
  `0x4000`.
- Lava-only launch: `UnityPlayerGameActivity` top-resumed, branded offline menu visible,
  no fatal/ANR/SIGSEGV marker. Raw captures remain outside the repository.
- Manifest: package `com.example.battleraja.m11`, version `1.0.0`, code `100`, target API
  36; `VIBRATE`, `INTERNET`, `ACCESS_NETWORK_STATE` and Unity's dynamic-receiver
  permission are present. This is a release gate, not a Play submission.

### Current visual-polish source

The latest exact-source tutorial/UI correction candidate is `c6badbf6cf5b1c7340fa907821aeb4cbf2194bc0`
from disposable copy `C:\Projects\BattleRaja-v1-tutorial-verify`:

- Validation **0 errors / 0 warnings**; EditMode **125/125**; PlayMode **66/66**.
- APK **40,431,923 bytes**, SHA-256
  `E6CBEAD6F97C036C0C9D1663CA5972799AEF3B330D75A3D2AAA94D5E699C7DB3`.
- AAB **36,262,021 bytes**, SHA-256
  `124E14ABE6012B3B42D7B7741D0C647416E278E82ABFE358EF89A53BAAD64021`.
- Bundle inspection passed: base manifest, 8 ARM64 libraries, no other ABIs and
  `0x4000` alignment for every checked native ELF LOAD segment.
- The exact APK was installed only on Lava. The tutorial SKIP action visibly reaches the
  completion card with replay/menu actions. The preceding exact visual candidate covered
  menu, mode, fighter-selection, Bazaar match, movement, match resolution and REMATCH. The
  correction APK also captured a successful player-owned Tiffin use at spawn; a later edge
  placement probe honestly returned `InvalidPlacement`.

The visual change is presentation-only: the Bazaar center uses a fictional six-panel
canopy/gold-orb landmark and the menu hero is larger at the phone viewport. This remains
a debug-signed, temporary-ID prototype candidate and is not a Play submission.

The exact V1 source was validated in disposable copy
`C:\Projects\BattleRaja-v1-verify-20260824j`:

The latest checkout also contains editor/test-only warning cleanup at `649d0bb`. Its
fresh APK build (`51D86184F6C69DD30CD249D273FA0F8F5BA96B4159D86DD1472FE4FD54320DA5`,
40,431,911 bytes) and matching AAB (`518102EAE7DDB71DA9393ABE3E948A47440260C9DF8D19532AAFF14FA1BE98B0`,
36,262,033 bytes) recorded zero `CS0618` warnings, zero C# errors and passed the static
16 KB alignment check; the installed visual artifact remains the `c6badbf` correction
candidate documented above.

- Unity `6000.5.6f1` (`0e0577a1a2ac`), validation **0 errors / 0 warnings**.
- Exact runtime source: `d825832bced4c5e07c7967d891696842eb55609a`.
- EditMode **125/125** and PlayMode **66/66** passed.
- Release-shaped Lava APK: **40,429,675 bytes**, SHA-256
  `50FD2D7F9C29F4888F2965810F9FD8130F7C2857F2A15AD7E3A5CF5908E7BFCC`.
- Debug-signed AAB: **36,259,768 bytes**, SHA-256
  `052F9CAB180E15AEEC0C2D8DCAB47187C53C58F07629C69F81A647697DB9FBF1`;
  base manifest present, 8 ARM64 libraries, 0 other ABIs, 450 entries, all ARM64
  ELF LOAD segments statically aligned to `0x4000`.
- Lava screenshots and raw metrics are recorded in
  `Docs/QA/V1_ANDROID_VISUAL_FEEDBACK_2026-08-24.md`; the Tiffin pickup/use route is
  now visually captured (raw files remain outside source).

This evidence is a release-shaped prototype candidate. The APK/AAB is debug-signed and
not publishable, the package ID remains temporary, legacy icon configuration still emits
a Unity deprecation warning, runtime 16 KB confirmation, performance, store/legal and
human review gates remain open.
The exact Lava visual pass captured successful Tiffin pickup/use. Tutorial completion,
results/rematch observation, touch/accessibility, performance, signing, store/legal and
human review remain explicit gates even though the automated authority regression is green.

## Local artifact command

Run from a clean, disposable project copy so scene generation cannot overwrite the working
tree:

```powershell
$unity = 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe'
pwsh -File Tools/Build/Android/build.ps1 `
  -ProjectRoot . `
  -UnityExe $unity `
  -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidV1ReleaseCandidate
```

Expected output: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`. The bundle is
release-shaped but not a Play submission: it uses the current temporary package ID and no
owner-approved signing identity.

## Artifact inspection

Record the exact source SHA, Unity revision, AAB byte size and SHA-256. Inspect the bundle
with Android Studio/bundletool and record:

1. manifest package, version name and monotonically increasing version code;
2. target/min SDK and merged permissions;
3. ARM64-only native libraries and ABI splits;
4. native ELF load-segment alignment for 16 KB pages;
5. debug symbols/profiling flags and signing certificate state;
6. dependency and licence inventory.

For the installable APK companion, run the repository manifest checker before any device
route evidence:

```powershell
pwsh -File Tools/Validation/check_android_manifest.ps1 `
  -ApkPath Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk `
  -AaptPath "$env:LOCALAPPDATA\Android\Sdk\build-tools\36.0.0\aapt.exe" `
  -ExpectedVersionName 1.0.0 `
  -ExpectedVersionCode 100 `
  -ExpectedMinSdk 28 `
  -ExpectedTargetSdk 36
```

Do not treat an APK install as proof that a Play bundle is acceptable. Google Play performs
bundle processing and signing checks that must be repeated in the owner-controlled console.

## Lava validation

Use only the approved Lava serial. Do not use the Oppo phone or the local emulator for the
release evidence.

```powershell
$adb = 'C:\Users\USER\AppData\Local\Android\Sdk\platform-tools\adb.exe'
& $adb devices -l
& $adb -s ST5GDW23LB004392 install -r Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk
& $adb -s ST5GDW23LB004392 shell monkey -p com.example.battleraja.m11 1
& $adb -s ST5GDW23LB004392 shell dumpsys meminfo com.example.battleraja.m11
& $adb -s ST5GDW23LB004392 shell dumpsys gfxinfo com.example.battleraja.m11
```

Capture cold launch, menu, tutorial, fighter selection, match opening, ability/gadget use,
Aandhi, elimination/spectator, results/rematch, settings, background/resume and a repeated
match run. Store raw screenshots/logcat/measurements outside tracked source until reviewed.

## Google Play source links

- [Target API level requirements](https://support.google.com/googleplay/android-developer/answer/11926878?hl=en-GB_ALL)
- [16 KB page-size compatibility](https://developer.android.com/guide/practices/page-sizes)
- [Create and set up an app, signing and version codes](https://support.google.com/googleplay/android-developer/answer/9859152?hl=en)
- [Play App Signing](https://support.google.com/googleplay/android-developer/answer/9842756?hl=en)

## Stop conditions

Stop the release claim if the exact source does not compile, the full test suites fail, the
AAB is not produced, the merged manifest requests an unapproved permission, 16 KB alignment
cannot be demonstrated, Lava launch is unavailable, or any human/legal gate is incomplete.
