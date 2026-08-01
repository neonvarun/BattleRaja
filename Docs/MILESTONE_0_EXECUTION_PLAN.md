# BattleRaja Milestone 0 Execution Plan

## Summary

Milestone 0 establishes a reproducible Unity project foundation for Android and desktop Web. It must not implement movement, combat, bots, networking, economy, final art, or live-service systems.

Approved baseline: Unity **6000.5.6f1** (Unity 6.5 family), using the 6000.5 URP package baseline. Web Build Support is already installed; install Android Build Support with its child SDK/NDK/OpenJDK modules. Lock the generated package manifest and lock file. Android uses a development IL2CPP ARM64 APK; Web uses a development WebGL2/WebAssembly build served over local HTTP.

## Verified baseline

- Windows 11 Pro x64, build 26200.
- Unity Hub 3.20.0 and Unity Editor 6000.5.6f1 are installed.
- Android SDK platforms 28, 30, 34, 35, 36 and Unity-managed 37.0 are installed.
- Unity-managed Android Build Tools/platform-tools 36.0.0 are installed; external ADB 36.0.2 remains available.
- Unity 6000.5 Android support is installed with Unity-managed NDK r27c `27.2.12479018`.
- Unity-managed OpenJDK Temurin 17.0.18 and embedded ADB 36.0.0 are installed; external JDK/ADB remain available.
- Lava API 34 and Oppo API 36 devices are authorized through ADB.
- Git 2.53.0 and Git LFS 3.7.1 are installed; the directory is a local Git worktree with LFS configured and no remote.
- Chrome 150 and Edge 150 are installed; Firefox, Playwright, WebDriver, and the .NET SDK are unavailable.

## Version and package decisions

### Milestone 0 install/lock set

| Component | Version | Decision |
|---|---:|---|
| Unity Editor | `6000.5.6f1` | Exact owner-approved editor baseline. |
| URP | `17.5.0` resolved | Unity 6000.5 resolves the URP/core/shadergraph family to 17.5.0; `packages-lock.json` is authoritative. |
| Input System | `1.20.0` | Use the 6000.5-compatible release and lock it. |
| Unity Test Framework | `1.7.0` resolved | Unity 6000.5 resolves the built-in Test Framework to 1.7.0; `packages-lock.json` is authoritative. |
| Android Build Support | Unity module | Install with SDK/NDK Tools and OpenJDK. |
| Web Build Support | Unity module | Install for WebGL2/WebAssembly builds. |

Defer Cinemachine, AI Navigation, Addressables, Animation Rigging, Profile Analyzer, Photon, and PlayFab until their approved milestones require them. Mathematics, Collections, Burst, Searcher, Mono Cecil, and Performance Testing appear only as package dependencies of the approved 6000.5 URP/Input/Test Framework baseline; they are not directly referenced by BattleRaja code in M0. Any version discrepancy discovered during installation requires a decision-log entry.

## Android strategy

- Use Unity-managed SDK/NDK/OpenJDK for reproducibility unless an exception is approved.
- Use target API 36 and minimum API 28 for the owner-approved local development profile; final store target/minimum policy must be rechecked before release.
- Use IL2CPP and ARM64.
- Use Vulkan first with OpenGL ES 3 fallback, then validate on the available API 34 and API 36 devices.
- Produce a development/debug APK only. Do not produce a release-signed APK or AAB.
- Use an owner-approved temporary development application ID; do not choose final branding or store identifiers.

## Web strategy

- Use WebGL2, HTML5, 64-bit WebAssembly, and a desktop-browser build profile.
- Build only the empty Bootstrap scene.
- Use an uncompressed development build served over HTTP with `python -m http.server`.
- Smoke-test Chrome and Edge manually. Firefox, Safari, mobile Web, WebDriver, HTTPS, CORS, compression, CDN caching, and public hosting remain untested/deferred.
- Do not add direct sockets, browser-authoritative networking, Photon, or PlayFab.
- For a future hosted build, configure Brotli/gzip `Content-Encoding`, `application/wasm`, HTTPS, CORS, and cache headers.

## Assembly and architecture boundaries

- `BattleRaja.Core.Domain`: pure C# (`noEngineReferences`), value types, entities, commands, events, rules, deterministic random interfaces, and fixed-step simulation contracts.
- `BattleRaja.Core.Application`: pure orchestration and ports, referencing Domain only.
- `BattleRaja.Gameplay`: feature composition, initially empty except for approved foundation contracts.
- `BattleRaja.Presentation`: Unity-facing views and MonoBehaviours, referencing Application/Gameplay but never owning gameplay truth.
- `BattleRaja.Infrastructure`: platform, persistence, analytics, and future networking adapters implementing Application ports.
- `BattleRaja.Infrastructure.Android` and `BattleRaja.Infrastructure.Web`: platform-specific adapters.
- `BattleRaja.Editor`: editor-only validation/build tooling.
- `BattleRaja.Tests.EditMode` and `BattleRaja.Tests.PlayMode`: pure and lifecycle smoke tests.

Human and future bot input must produce the same immutable gameplay-command representation. Runtime mutable state must not live in ScriptableObject assets. Simulation time is fixed-step and independent of rendering time. No Photon or PlayFab assembly may exist in Milestone 0.

## Repository changes

Preserve the current root and documentation. Convert `C:\Projects\BattleRaja` itself into the Unity project root.

Expected additions include:

- `ProjectSettings/ProjectVersion.txt` and Unity-generated project settings.
- `Packages/manifest.json` and `Packages/packages-lock.json`.
- Assembly definitions under `Assets/BattleRaja`.
- `Assets/BattleRaja/Scenes/Bootstrap/Bootstrap.unity`.
- Minimal Bootstrap composition code and EditMode/PlayMode tests.
- Editor validation/build entrypoints.
- `Tools/Build/Android/build.ps1`, `Tools/Build/Web/build.ps1`, and `Tools/Validation/validate.ps1`.

Update `PROJECT_STATUS.md`, `Docs/ARCHITECTURE.md`, `Docs/DECISIONS.md`, `Docs/RESEARCH_LOG.md`, `Docs/TEST_STRATEGY.md`, `Docs/WEB_PLATFORM.md`, and `Docs/MILESTONE_ISSUES.md` with evidence. Do not commit `Library`, `Temp`, `Obj`, `Build`, `Builds`, `Logs`, `UserSettings`, IDE caches, signing files, or secrets.

## Expected commands

```powershell
git init
git lfs install --local
git lfs fsck
git diff --check
git status --short
```

```powershell
& $UnityExe -batchmode -nographics -projectPath C:\Projects\BattleRaja -runTests -testPlatform editmode -testResults Builds\M0\TestResults\editmode.xml -logFile Builds\M0\Logs\editmode.log
& $UnityExe -batchmode -nographics -projectPath C:\Projects\BattleRaja -runTests -testPlatform playmode -testResults Builds\M0\TestResults\playmode.xml -logFile Builds\M0\Logs\playmode.log
& $UnityExe -batchmode -nographics -quit -projectPath C:\Projects\BattleRaja -executeMethod BattleRaja.Editor.BuildEntrypoints.ValidateProject -logFile Builds\M0\Logs\validation.log
```

```powershell
& $UnityExe -batchmode -nographics -quit -projectPath C:\Projects\BattleRaja -executeMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidDevelopment -buildTarget Android -logFile Builds\M0\Logs\android-build.log
adb devices -l
adb -s <serial> install -r Builds\M0\Android\BattleRaja-M0.apk
adb -s <serial> shell monkey -p <approved.application.id> 1
adb -s <serial> logcat -d -s Unity
```

```powershell
& $UnityExe -batchmode -nographics -quit -projectPath C:\Projects\BattleRaja -executeMethod BattleRaja.Editor.BuildEntrypoints.BuildWebDevelopment -buildTarget WebGL -logFile Builds\M0\Logs\web-build.log
python -m http.server 8000 --directory Builds/M0/Web
curl.exe -I http://127.0.0.1:8000/
# Installed-browser smoke (headless DOM/bootstrap check)
& 'C:\Program Files\Google\Chrome\Application\chrome.exe' --headless=new --disable-gpu --dump-dom --virtual-time-budget=5000 http://127.0.0.1:8000/
& 'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe' --headless=new --disable-gpu --dump-dom --virtual-time-budget=5000 http://127.0.0.1:8000/
```

## Execution order

1. Record the owner-approved Unity/package/toolchain, Git, architecture, Android, browser, and temporary application-ID decisions.
2. Record the baseline and preserve the starter repository.
3. Initialize Git and Git LFS, then create a baseline commit.
4. Add Android Build Support (including SDK/NDK Tools and OpenJDK) to Unity 6000.5.6f1 and verify dependencies; retain the installed Web module.
5. Create the Unity URP project at the repository root without deleting documentation.
6. Add the approved package set and lock files.
7. Add assembly definitions, pure foundation contracts, Bootstrap scene, and tests.
8. Add validation/build entrypoints and PowerShell wrappers.
9. Configure Android and Web build profiles.
10. Run tests and validation, build/install the Android APK, and run the Web local smoke test.
11. Update documentation with exact evidence and hold the Milestone 0 review gate.

## Milestone issue sequence

- M0: toolchain, Unity root, packages, assemblies, tests, validation, Android APK, and Web smoke build.
- M1: movement, input abstraction, and camera lab.
- M2: health, damage, projectiles, cooldowns, and pooling.
- M3: data-driven Bijli fighter kit and shared command API.
- M4: navigation, perception, seeded bot decisions, and profiling.
- M5: offline match lifecycle, elimination, zone, placement, restart, and spectator flow.

Only M0 may be implemented under this plan.

## Risks, blockers, and assumptions

- Android Build Support is installed through the approved Unity Hub toolchain; retain the evidence in the status/research records.
- `sdkmanager` reports an SDK XML compatibility warning.
- No Git remote currently exists; local history is intentionally sufficient for M0.
- Browser validation is limited to manual Chrome and Edge.
- Minimum API 28, target API 36, graphics settings and temporary application ID were owner-approved for local M0 development; final store identity remains unapproved.
- Public hosting, CI, store submission, credentials, paid infrastructure, and final branding are outside M0.

## Required approvals

1. Use the installed Unity Hub/Editor 6000.5.6f1 and install Android Build Support plus its required child modules; retain Web Build Support. **Approved by the owner in the current task; completed.**
2. Initialize Git and configure Git LFS. **Completed locally; choosing a remote remains optional and requires owner direction.**
3. Convert the repository root into a Unity project. **Approved by the owner in the current task; completed.**
4. Lock the package matrix and approve deviations. **Approved for M0 by the owner; exact resolved versions are recorded in the lockfile.**
5. Approve assembly boundaries. **Accepted for M0; later gameplay/network boundaries require review.**
6. Approve Android API, graphics, IL2CPP/ARM64, and temporary application ID settings. **Approved for local development by the owner; final store identity is not approved.**
7. Approve the Chrome/Edge-only M0 browser matrix. **Accepted for M0; broader browser/hosting coverage remains deferred.**
8. Approve any future CI, hosting, cost, public deployment, branding, or store submission. **Still required before those actions.**
9. Approve beginning Milestone 1 after M0 review. **Still required.**

## Acceptance criteria

- Unity 6000.5.6f1 and required Android/Web modules are installed and verified.
- The root opens cleanly as a Unity URP project and preserves existing documentation.
- Package manifest/lock files contain only approved M0 packages.
- Domain/Application assemblies compile without UnityEngine references.
- EditMode, PlayMode, architecture, content, secret, and forbidden-file checks pass.
- A development Android APK builds and launches on at least one authorized device; both available devices should be tested when possible.
- A Web development build starts when served over HTTP; Chrome and Edge both returned Unity bootstrap content from the served page.
- Untested Firefox/Safari/mobile/production-hosting behavior is documented.
- No later-milestone gameplay, networking, backend, economy, final content, or public release systems are present.
- Documentation and status contain exact commands, results, warnings, blockers, assumptions, and approval gates.
