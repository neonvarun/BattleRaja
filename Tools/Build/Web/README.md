# Web Build Tools

Run from the repository root after Unity bootstrap:

```powershell
pwsh -File Tools/Build/Web/build.ps1
python -m http.server 8000 --directory Builds/M2/Web
```

The wrapper produces the M2 combat-lab development WebGL/WebAssembly build through the editor build entrypoint. Local serving is required; opening the build directly with `file://` is not valid browser evidence.
