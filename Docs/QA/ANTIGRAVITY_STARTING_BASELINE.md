# BattleRaja Starting Baseline Evidence (Stage 0)

Date: 2026-08-04  
Branch: `antigravity/closed-alpha-completion`  
Starting HEAD Commit: `a1e084d9a8111562eb2ff2129c35ec93dc575800` (merged `main`)  
Unity Version: `6000.5.6f1`  
Packages: Input System `1.20.0`, uGUI `2.5.0`, URP `17.5.0`, Test Framework `1.7.0`

---

## 1. Repository Validation & Integrity

- **Command**: `pwsh -File Tools/Validation/validate.ps1 -ProjectRoot .`
- **Result**: **0 errors, 0 warnings**
- **Git LFS Integrity**: `git lfs fsck --pointers` -> `OK`
- **Whitespace / Syntax Check**: `git diff --check` -> Clean

---

## 2. Test Suite Baseline

- **EditMode Test Suite**:
  - Command: `"C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe" -batchmode -nographics -projectPath . -runTests -testPlatform editmode -testResults Builds/M11/TestResults/editmode.xml -logFile Builds/M11/Logs/editmode.log`
  - Result: **114 / 114 passed** (0 failed, 0 skipped)
  - Execution Time: ~0.1s
- **PlayMode Test Suite**:
  - Command: `"C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe" -batchmode -nographics -projectPath . -runTests -testPlatform playmode -testResults Builds/M11/TestResults/playmode.xml -logFile Builds/M11/Logs/playmode.log`
  - Result: **54 / 54 passed** (0 failed, 0 skipped)
  - Execution Time: ~13.7s

---

## 3. Platform & Target Status

- **Android Target**: Verified toolchain (SDK 36.0.0, NDK r27c, OpenJDK 17.0.18). Connected Lava testing device ready (`LAVA LXX508`).
- **Web Target**: WebGL2 / WebAssembly template verified with canvas focus handlers and local HTTP server support.
- **Networking Seam**: Photon Fusion 2.1.1 present in `Assets/Photon`; gated behind `PhotonFusionAdapter` (no active multiplayer claim).
- **Backend Seam**: `FakeProgressionBackend` active; `PlayFabBackendAdapter` gated behind explicit credential checks.

---

## 4. Stage 0 Baseline Conclusion

The repository baseline passes all technical integrity, assembly boundary, and automated test checks without errors or warnings. Stage 0 baseline complete. Ready to proceed to Stage 1.
