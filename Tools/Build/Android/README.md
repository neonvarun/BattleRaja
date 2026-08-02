# Android Build Tools

Run from the repository root after Unity bootstrap:

```powershell
pwsh -File Tools/Build/Android/build.ps1
```

The wrapper produces the current M8 development APK through the editor build entrypoint
and records logs under `Builds/M8/Logs`. Device installation and launch are separate
evidence steps using ADB; M8 Android smoke is limited to the connected Lava phone.
