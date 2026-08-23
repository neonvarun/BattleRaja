# BattleRaja V1.0 Android release checklist

This is the offline Android release-candidate gate for BattleRaja. It deliberately does not
start Photon, PlayFab, accounts, ads, IAP, cloud progression or Web release work.

## Candidate scope

- Unity `6000.5.6f1`, URP, ARM64, IL2CPP.
- Android target API 36, minimum API 28.
- One local human plus seven deterministic bots in Bazaar Bastion.
- Bijli, Pehel and Maya; Umbrella Guard, Dhol Burst and Tiffin Station; Aandhi; tutorial;
  spectator; results; rematch; local settings.
- No account, network permission, online room or server-owned progression is required by the
  offline candidate.

## Current release gates

| Gate | Current state | Owner action |
| --- | --- | --- |
| Product/package identity | Blocked: build entrypoint still uses `com.example.battleraja.m11` | Approve final application ID and branding |
| Signing | Not started | Approve upload key/Play App Signing path; never commit the key |
| Target API | Configured to API 36 | Recheck against current Play policy at upload time |
| 64-bit | Passed with evidence for the current debug-signed AAB: 8 ARM64 libraries, 0 other ABIs | Re-run inspection after any package/plugin change |
| 16 KB pages | Static evidence passed: zipalign `-P 16` and all eight ARM64 ELF LOAD segments at `0x4000`; runtime 16 KB environment still open | Re-run the checker after any package/plugin change and install on a 16 KB Android environment when available |
| Permissions | Forced Internet and SD-card permissions are disabled | Inspect the merged manifest after each plugin/build change |
| Device QA | Automated smoke passed on Lava (`ST5GDW23LB004392`); human review open | Owner performs touch, accessibility, battery and thermal review |
| Store/legal | Draft only | Approve privacy, data-safety, content rating, cultural and legal copy |
| Play Console | Not started | Owner creates the app and decides rollout/release track |

## Latest local candidate evidence (2026-08-23)

The exact V1 source was validated in disposable copy
`C:\Projects\BattleRaja-v1-verify-20260823c`:

- Unity `6000.5.6f1` (`0e0577a1a2ac`), validation **0 errors / 0 warnings**.
- Exact runtime source: `ab5b12ad7c86f425243fc3f2a9cbc83ae97e6f6d`.
- EditMode **125/125** and PlayMode **64/64** passed.
- Release-shaped Lava APK: **40,420,983 bytes**, SHA-256
  `E70241D83E6DBDA977EECF9F476502FD68B89799438DBA06F024423D575E5532`.
- Debug-signed AAB: **36,251,072 bytes**, SHA-256
  `4B22FD2DADD26FB1A5FEA96FE5EAA19BC2D0EC4F130F87009969D38562FE84C6`;
  base manifest present, 8 ARM64 libraries, 0 other ABIs, 450 entries, all ARM64
  ELF LOAD segments statically aligned to `0x4000`.
- Lava screenshots and raw metrics are recorded in
  `Docs/QA/V1_ANDROID_EVIDENCE_2026-08-23.md` (raw files remain outside source).

This evidence is a release-shaped prototype candidate. The APK/AAB is debug-signed and
not publishable, the package ID remains temporary, legacy icon configuration still emits
a Unity deprecation warning, runtime 16 KB confirmation, performance, store/legal and
human review gates remain open.

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
