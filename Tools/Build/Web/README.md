# Web Build Tools

Run from the repository root after Unity bootstrap:

```powershell
pwsh -File Tools/Build/Web/build.ps1
python -m http.server 8000 --directory Builds/M6/Web
```

The wrapper produces the M6 gadget-system development WebGL/WebAssembly build through the editor build entrypoint. Local serving is required; opening the build directly with `file://` is not valid browser evidence.
