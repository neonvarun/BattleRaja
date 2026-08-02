# Web Build Tools

Run from the repository root after Unity bootstrap:

```powershell
pwsh -File Tools/Build/Web/build.ps1
python -m http.server 8015 --directory Builds/M11/Web
```

The wrapper produces the current M11 WebGL/WebAssembly build through the editor build
entrypoint. Local serving is required; opening the build directly with `file://` is not
valid browser evidence.
