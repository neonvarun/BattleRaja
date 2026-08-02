# Project Status

## Active milestone

**Milestone 1 — Cross-Platform Movement Laboratory**

## Current state

- Product vision: drafted
- Root agent rules: active
- Milestone 0: complete and committed locally
- Unity project: verified at the repository root with URP and MovementLab scenes
- Unity version: `6000.5.6f1`
- Android toolchain: Unity-managed SDK/NDK/OpenJDK modules installed and verified; SDK/Build Tools 36.0.0, NDK r27c `27.2.12479018`, OpenJDK 17.0.18, embedded ADB 36.0.0
- Unity Web build support: present and used successfully
- Browser test environment: Chrome 150 and Edge 150 available; Firefox/Playwright/WebDriver unavailable
- Packages: Input System `1.20.0`, uGUI `2.5.0`, URP `17.5.0`, Test Framework `1.7.0`; lockfile is authoritative
- Movement laboratory: implemented with grey-box arena, placeholder player, independent movement/aim, orthographic camera, aim indicator, desktop bindings and safe-area touch sticks
- Combat/gameplay progression: not started; movement prototype only
- Multiplayer: deliberately deferred
- Backend/economy: deliberately deferred
- Final art/audio/animation: not started
- Git/LFS: local repository initialized and LFS configured; no remote configured locally

## M1 execution evidence — 2026-08-02

- Pure movement tests: 8/8 EditMode tests passed in `Builds/M1/TestResults/editmode.xml`.
- Movement integration tests: 7/7 PlayMode tests passed in `Builds/M1/TestResults/playmode.xml`.
- Editor validation: `BattleRaja.Editor.BuildEntrypoints.ValidateProject` passed.
- Android: M1 IL2CPP ARM64 development APK built with min API 28/target API 36, installed and launched on Lava LXX508 (Android 14/API 34) and Oppo CPH2487 (Android 16/API 36). Artifact: `Builds/M1/Android/BattleRaja-M1.apk`.
- Web: M1 WebGL2/WebAssembly development build completed under `Builds/M1/Web`, served over local HTTP on port 8001, and returned HTTP 200. Chrome 150 and Edge 150 DOM checks found Unity bootstrap content.
- Android runtime smoke found no M1 application exception after serializing the aim-indicator material. Unity still logs the known Play Asset Delivery `AssetPackManager` class-probe warning on a development APK.

## M0 evidence retained

- M0 validation, tests and smoke artifacts remain under ignored `Builds/M0/`.
- The M0 foundation commit is preserved as the parent of the current working changes.

## Performance and limitations

- Movement hot paths use cached component references, value types and no per-frame LINQ/managed collection creation.
- No formal Editor Profiler, Android GPU/CPU capture or Web browser performance profile has been collected yet; values remain explicitly unmeasured in `Docs/PERFORMANCE_BUDGET.md`.
- Physical touch interaction, safe-area variations, browser keyboard/mouse focus by manual play, and visual camera comparison still require human playtesting.

## Approval gates

Review is required before:

1. Beginning Milestone 2 combat implementation.
2. Adding Photon, PlayFab, monetisation or paid infrastructure.
3. Publishing, deploying, signing for release or submitting to stores.
4. Changing the pinned editor/package baseline or final branding/trademarks.
