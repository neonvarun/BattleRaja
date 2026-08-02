# Build Tools

Milestone 1 wrappers invoke editor-owned build entrypoints so the build settings remain versioned in the Unity project rather than duplicated in shell scripts.

- `Android/build.ps1` invokes `BattleRaja.Editor.BuildEntrypoints.BuildAndroidDevelopment`.
- `Web/build.ps1` invokes `BattleRaja.Editor.BuildEntrypoints.BuildWebDevelopment`.

Both wrappers fail early when Unity or the Unity project markers are missing and produce artifacts under `Builds/M1/`.
