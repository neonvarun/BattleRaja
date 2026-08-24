# Android Build Tools

Run from the repository root after Unity bootstrap:

```powershell
pwsh -File Tools/Build/Android/build.ps1
```

The wrapper produces the current M11 development APK through the editor build entrypoint
and records logs under `Builds/M11/Logs`. Device installation and launch are separate
evidence steps using ADB; M11 Android smoke is limited to the connected Lava phone.

The build entrypoints use the temporary development ID `com.example.battleraja.m11` by
default. Before an owner-approved release build, set the final package identity in the
process environment; it is validated but never committed:

```powershell
$env:BATTLERAJA_ANDROID_APPLICATION_ID = 'com.yourcompany.battleraja'
pwsh -File Tools/Build/Android/build.ps1 `
  -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidV1ReleaseCandidate
Remove-Item Env:BATTLERAJA_ANDROID_APPLICATION_ID
```

The final signing key, Play App Signing path and branding remain explicit owner gates.
