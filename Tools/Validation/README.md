# Validation Tools

## Lava performance capture

`capture_android_performance.ps1` provides repeatable, Lava-only Android evidence.
It refuses the Oppo serial, resolves the installed Unity activity, captures memory,
CPU, graphics, battery, thermal, activity and logcat samples, and writes a manifest
with the exact device/package/sample window. It does not uninstall the app, clear
application data or alter the device beyond launching the requested package.

```powershell
pwsh -File Tools/Validation/capture_android_performance.ps1 `
  -AdbPath "$env:LOCALAPPDATA\Android\Sdk\platform-tools\adb.exe" `
  -DeviceSerial ST5GDW23LB004392 `
  -PackageId com.example.battleraja.m11 `
  -DurationSeconds 120 `
  -IntervalSeconds 5
```

Keep generated captures outside tracked source or under the ignored
`Builds/Local/Device/Performance/<timestamp>` path. Treat shell samples as raw
evidence; they are not a performance pass without a sustained match scenario,
frame-time interpretation and human review.

`validate.ps1` performs repository and Milestone 0 preflight checks without requiring Unity. It verifies required documentation, Unity project markers, LFS attributes, prohibited package references, tracked generated paths, and obvious secret assignments.

Usage from the repository root:

```powershell
pwsh -File Tools/Validation/validate.ps1
pwsh -File Tools/Validation/validate.ps1 -RequireUnityProject -UnityExe 'C:\Program Files\Unity\Hub\Editor\6000.3.20f1\Editor\Unity.exe'
```

The first form is suitable before Unity installation. The second form is the post-bootstrap gate and fails if Unity project markers or a discoverable `Unity.exe` are absent.

`run_unity_tests.ps1` runs one complete Unity EditMode or PlayMode suite and verifies the
generated NUnit report. It intentionally omits `-quit`: Unity's test runner exits after
writing the report, while combining `-quit` with `-runTests` can stop the editor before
tests execute.

```powershell
pwsh -File Tools/Validation/run_unity_tests.ps1 `
  -UnityExe 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe' `
  -ProjectRoot . `
  -TestPlatform editmode `
  -TestResults C:\BRV1Tests\editmode.xml `
  -LogFile C:\BRV1Tests\editmode.log

pwsh -File Tools/Validation/run_unity_tests.ps1 `
  -UnityExe 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe' `
  -ProjectRoot . `
  -TestPlatform playmode `
  -TestResults C:\BRV1Tests\playmode.xml `
  -LogFile C:\BRV1Tests\playmode.log
```

`update_package_manifest.ps1` refreshes byte counts and SHA-256 hashes after repository changes while excluding generated directories and the manifest itself:

```powershell
pwsh -File Tools/Validation/update_package_manifest.ps1
```

`check_android_bundle.ps1` reports the exact AAB hash/size and verifies that the base module
contains ARM64 native libraries before the owner performs bundletool and Play Console checks:

```powershell
pwsh -File Tools/Validation/check_android_bundle.ps1 `
  -AabPath Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab `
  -RequireArm64
```

For the Android 16 KB static gate, add Unity's NDK `llvm-readelf.exe`; the checker
extracts each ARM64 library, verifies every ELF `LOAD` segment is aligned to `0x4000`,
and removes only its temporary extraction directory:

```powershell
pwsh -File Tools/Validation/check_android_bundle.ps1 `
  -AabPath Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab `
  -RequireArm64 `
  -Require16KPageAlignment `
  -ReadElfPath 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Data\PlaybackEngines\AndroidPlayer\NDK\toolchains\llvm\prebuilt\windows-x86_64\bin\llvm-readelf.exe'
```

`check_android_manifest.ps1` makes the offline APK manifest gate repeatable. It records the
package ID, version, SDK levels, permissions and SHA-256, and rejects `INTERNET` and
`ACCESS_NETWORK_STATE` unless `-AllowNetworkPermissions` is explicitly supplied:

```powershell
pwsh -File Tools/Validation/check_android_manifest.ps1 `
  -ApkPath Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk `
  -AaptPath "$env:LOCALAPPDATA\Android\Sdk\build-tools\36.0.0\aapt.exe" `
  -ExpectedVersionName 1.0.0 `
  -ExpectedVersionCode 100 `
  -ExpectedMinSdk 28 `
  -ExpectedTargetSdk 36
```

`check_store_creative.ps1` performs a dependency-free PNG dimension check for the draft
icon, feature graphic and optional screenshot directory. It is intentionally report-only
until `-RequireFinal` is supplied; a passing dimension check is not legal, cultural, brand,
or human visual approval:

```powershell
pwsh -File Tools/Validation/check_store_creative.ps1
pwsh -File Tools/Validation/check_store_creative.ps1 -ScreenshotDirectory 'C:\path\to\reviewed-captures'
pwsh -File Tools/Validation/check_store_creative.ps1 -RequireFinal
```

`check_v1_release_candidate.ps1` composes the local V1 technical gates for an exact
APK/AAB pair. It validates the repository, APK manifest, ARM64/16 KB AAB contents and
technical store-asset dimensions. It does not sign, upload, publish, or replace owner
approval for package identity, privacy, cultural review or Play Console submission:

```powershell
pwsh -File Tools/Validation/check_v1_release_candidate.ps1 `
  -ProjectRoot . `
  -ApkPath C:\path\to\BattleRaja-V1.0-release-candidate.apk `
  -AabPath C:\path\to\BattleRaja-V1.0-release-candidate.aab `
  -AaptPath "$env:LOCALAPPDATA\Android\Sdk\build-tools\36.0.0\aapt.exe" `
  -ReadElfPath 'C:\path\to\llvm-readelf.exe' `
  -UnityExe 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe' `
  -ExpectedPackageId com.example.battleraja.m11 `
  -RequireCleanWorktree
```
