# Android Build Tools

Run from the repository root after Unity bootstrap:

```powershell
pwsh -File Tools/Build/Android/build.ps1
```

The wrapper produces a development APK through the editor build entrypoint and records logs under `Builds/M0/Logs`. Device installation and launch are separate evidence steps using ADB.
