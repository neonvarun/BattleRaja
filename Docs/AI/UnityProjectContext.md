# Unity Project Context

## Milestone 0 baseline

- Repository: `C:\Projects\BattleRaja`
- Editor: Unity `6000.5.6f1` (`0e0577a1a2ac`)
- Hub: Unity Hub `3.20.0`
- Render pipeline: URP; `Assets/BattleRaja/Content/BattleRaja-M0-URP.asset` is assigned as the project default.
- Bootstrap scene: `Assets/BattleRaja/Scenes/Bootstrap/Bootstrap.unity`, enabled in Build Settings.
- Direct packages: Input System `1.20.0`, URP requested/resolved to `17.5.0`, Test Framework requested/resolved to `1.7.0`.
- Lockfile: `Packages/packages-lock.json` is authoritative for all transitive package versions.
- Android: IL2CPP ARM64 development profile, min API 28, target/compile API 36, Unity-managed NDK r27c and OpenJDK 17.
- Web: WebGL2/WebAssembly development profile, uncompressed local HTTP smoke output.

## Assembly boundaries

- `BattleRaja.Core.Domain` and `BattleRaja.Core.Application` have `noEngineReferences` and must remain pure C#.
- `BattleRaja.Gameplay` is the Unity-independent feature composition boundary.
- `BattleRaja.Presentation` owns Unity views and scene-facing behavior.
- `BattleRaja.Infrastructure` owns adapters; Android and Web use platform-filtered child assemblies.
- `BattleRaja.Editor` owns validation/build entrypoints; tests are split into EditMode and PlayMode assemblies.
- Human input and future bot decisions must produce the same immutable gameplay-command model.

## M0 evidence

- Project validation: passed with 0 errors/0 warnings.
- EditMode: 2/2 passed; PlayMode: 1/1 passed.
- Android APK installed/launched on Lava LXX508 API 34 and Oppo CPH2487 API 36.
- Web build served over local HTTP; Chrome 150 and Edge 150 returned Unity bootstrap DOM content.

## Constraints and known limitations

- Do not add Photon, PlayFab, public networking, economy or gameplay systems before their approved milestones.
- Do not commit `Library`, `Temp`, `Logs`, `Builds`, `UserSettings`, IDE caches or secrets.
- Empty feature/infrastructure boundary assemblies intentionally generate editor warnings until their first real implementation.
- Unity batch logs include licensing-handshake warnings that resolved successfully; WebGL reports `AllowDebugging` is ignored; Android development logs include a Play Asset Delivery class-probe warning.
- Firefox/Safari/mobile Web, automated browser tooling, production hosting, signing and store submission are not validated.
