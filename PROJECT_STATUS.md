# Project Status

## Active milestone

**Milestone 0 — Repository and Research Foundation**

## Current state

- Product vision: drafted
- Root agent rules: drafted
- Milestone 0 execution plan: present in `Docs/MILESTONE_0_EXECUTION_PLAN.md`
- Unity project: bootstrapped and verified at the repository root with URP, Bootstrap scene and pinned package lock
- Unity version: owner-approved and installed `6000.5.6f1`
- Android toolchain: Unity-managed SDK/NDK/OpenJDK modules installed and verified; SDK platform/Build Tools 36.0.0, NDK r27c `27.2.12479018`, OpenJDK 17.0.18, embedded ADB 36.0.0
- Unity Web build support: present and used successfully in `6000.5.6f1`
- Browser test environment: Chrome 150 and Edge 150 available; Firefox/Playwright/WebDriver unavailable
- Gameplay: not started
- Multiplayer: deliberately deferred
- Backend: deliberately deferred
- Final art/audio: not started
- Git/LFS: Git 2.53.0 and Git LFS 3.7.1 installed; local repository initialized and LFS configured, with no remote configured
- Repository remote: not configured

## Evidence and blockers

- Milestone 0 project conversion, package resolution, assembly boundaries and build tooling are complete.
- No active M0 blocker remains. Unity licensing handshake warnings appeared in batch logs, but entitlement resolution completed and validation/builds succeeded.
- Deliberately untested or deferred: production signing/hosting, Firefox/Safari/mobile Web, automated WebDriver, compression/CDN behavior, performance profiling, public networking, Photon, PlayFab and store submission.

## Milestone 0 execution evidence — 2026-08-02

- Approved editor: Unity `6000.5.6f1`; Hub `3.20.0`.
- Embedded Android dependencies: SDK platforms 34/36/37, Build Tools 36.0.0, platform-tools/ADB 36.0.0, NDK r27c `27.2.12479018`, OpenJDK Temurin 17.0.18.
- Packages: Input System `1.20.0`, URP family `17.5.0`, built-in Test Framework `1.7.0`; exact transitive resolution is pinned in `Packages/packages-lock.json`.
- Project validation: `Tools/Validation/validate.ps1 -RequireUnityProject` passed with 0 errors and 0 warnings; editor `ValidateProject` passed.
- EditMode tests: 2/2 passed in `Builds/M0/TestResults/editmode.xml`.
- PlayMode tests: 1/1 passed in `Builds/M0/TestResults/playmode.xml`.
- Android: development IL2CPP ARM64 APK built for min API 28/target API 36, installed and launched on Lava LXX508 (Android 14/API 34) and Oppo CPH2487 (Android 16/API 36).
- Web: WebGL2/WebAssembly development build served over local HTTP; Chrome 150 and Edge 150 both returned a DOM containing Unity bootstrap content.
- Artifacts remain under ignored `Builds/M0/`; source control contains only project/configuration/tests/tooling and documentation.

## Milestone 0 objective

Establish a verified local toolchain, approved Unity version and package plan, clean architecture boundaries, tests, command-line validation and minimal Android and browser Web development builds.

## Human approval gates

The owner explicitly approved Unity `6000.5.6f1`, installation of required modules/packages, root conversion, and Milestone 0 validation in the current task. Review is still required before:

1. Beginning Milestone 1 gameplay implementation.
2. Adding Photon, PlayFab, monetisation or paid infrastructure.
3. Publishing, deploying, signing for release or submitting to stores.
4. Changing the pinned editor/package baseline or final branding/trademarks.
