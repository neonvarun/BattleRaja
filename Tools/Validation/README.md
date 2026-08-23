# Validation Tools

`validate.ps1` performs repository and Milestone 0 preflight checks without requiring Unity. It verifies required documentation, Unity project markers, LFS attributes, prohibited package references, tracked generated paths, and obvious secret assignments.

Usage from the repository root:

```powershell
pwsh -File Tools/Validation/validate.ps1
pwsh -File Tools/Validation/validate.ps1 -RequireUnityProject -UnityExe 'C:\Program Files\Unity\Hub\Editor\6000.3.20f1\Editor\Unity.exe'
```

The first form is suitable before Unity installation. The second form is the post-bootstrap gate and fails if Unity project markers or a discoverable `Unity.exe` are absent.

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

`check_store_creative.ps1` performs a dependency-free PNG dimension check for the draft
icon, feature graphic and optional screenshot directory. It is intentionally report-only
until `-RequireFinal` is supplied; a passing dimension check is not legal, cultural, brand,
or human visual approval:

```powershell
pwsh -File Tools/Validation/check_store_creative.ps1
pwsh -File Tools/Validation/check_store_creative.ps1 -ScreenshotDirectory 'C:\path\to\reviewed-captures'
pwsh -File Tools/Validation/check_store_creative.ps1 -RequireFinal
```
