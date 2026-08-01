# Build Tools

Milestone 0 wrappers invoke editor-owned build entrypoints so the build settings remain versioned in the Unity project rather than duplicated in shell scripts.

- `Android/build.ps1` invokes `BattleRaja.Editor.BuildEntrypoints.BuildAndroidDevelopment`.
- `Web/build.ps1` invokes `BattleRaja.Editor.BuildEntrypoints.BuildWebDevelopment`.

Both wrappers fail early when Unity or the Unity project markers are missing. They are prepared now but cannot produce artifacts until the approved Unity installation and root conversion are complete.
