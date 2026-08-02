# Latest HEAD baseline

Date: 2026-08-02
Branch: `codex/product-completion`
Starting HEAD: `afbbe2d297900c9cb6afd6d7e52d55a47b978f54` (`chore: add Photon Fusion 2.1.1 setup`)
Unity: `6000.5.6f1` (`C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe`)

## Scope and repository note

The requested goal path `Docs/AI/RepositoryAuditAndCompletionGoal.md` is absent. The matching file `Docs/AI/BattleRaja_Repository_Audit_and_Completion_Goal.md` was read in full and used as the authoritative continuation brief. This path discrepancy remains a documentation issue; no source from the requested path was available.

The baseline intentionally excludes unrelated working-tree changes in `Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity` and `Data/Plugins/lib_burst_generated.wasm`. Those files were not staged or altered by this baseline work.

## Validation results

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-02 |
| Unity compile | Unity batchmode compile — exit 0; no `error CS`, `Exception`, or `Failed to compile` matches | `Builds/M11/Logs/latest-head-compile.log` |
| EditMode tests | 67 passed, 0 failed | `Builds/M11/TestResults/fixed-presentation-editmode-v2.xml` |
| PlayMode tests | 27 passed, 0 failed | `Builds/M11/TestResults/latest-head-playmode.xml` |
| Phase 1 timeout regression | Deterministic health/distance/id ranking; complete placements; EditMode 59/59 and PlayMode 27/27 pass | `Builds/M11/TestResults/phase1-editmode.xml`, `Builds/M11/TestResults/phase1-playmode.xml` |
| Fixed-clock integration | 30 Hz accumulator separates render frames from authoritative match steps; EditMode 60/60 and PlayMode 27/27 pass | `Builds/M11/TestResults/clock-editmode.xml`, `Builds/M11/TestResults/clock-playmode.xml` |
| Aandhi interpolation | Zone radius is continuous across opening/pressure transitions; EditMode 60/60 and PlayMode 27/27 pass | `Builds/M11/TestResults/aandhi-editmode.xml`, `Builds/M11/TestResults/aandhi-playmode.xml` |
| Bot zone awareness | Bot perception includes current/next zone and the decision engine repositions proactively; EditMode 61/61 and PlayMode 27/27 pass | `Builds/M11/TestResults/botzone-editmode.xml`, `Builds/M11/TestResults/botzone-playmode.xml` |
| Authority seam | `OfflineMatchAuthority` owns zone-damage cadence and emits immutable `DamageRequest` intents; EditMode 62/62 and PlayMode 27/27 pass | `Builds/M11/TestResults/authority-editmode.xml`, `Builds/M11/TestResults/authority-playmode.xml` |
| Combat statistics | Instigator-aware events record damage, eliminations, survival time and duplicate-credit prevention; EditMode 67/67 and PlayMode 27/27 pass | `Builds/M11/TestResults/fixed-presentation-editmode-v2.xml`, `Builds/M11/TestResults/fixed-presentation-playmode-v2.xml` |
| Aandhi warning/preview | Warning state, remaining warning time and next-radius preview are part of the immutable tick result; EditMode 67/67 and PlayMode 27/27 pass | `Builds/M11/TestResults/fixed-presentation-editmode-v2.xml`, `Builds/M11/TestResults/fixed-presentation-playmode-v2.xml` |
| Tick-based presentation timing | Player movement, Bijli ability, projectile stepping, gadget timers and weapon attack cooldown consume a 30 Hz clock; EditMode 67/67 and PlayMode 27/27 pass | `Builds/M11/TestResults/fixed-presentation-editmode-v2.xml`, `Builds/M11/TestResults/fixed-presentation-playmode-v2.xml` |
| Android build | Development IL2CPP APK — exit 0 | `Builds/M11/Logs/android-build.log` |
| Android artifact | 150,735,210 bytes; SHA-256 `22B55FCD3B82EB641F6B04EF02BF3572301D4DEB119EDC193D6BFE834AE5ECE9` | `Builds/M11/Android/BattleRaja-M11.apk` |
| Android device smoke | Installed and launched on Lava `ST5GDW23LB004392` only; focused activity `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`; process `22867` observed | `Docs/QA/Visual/latest-head-android.png`; ADB/logcat output |
| Android runtime scan | No `FATAL EXCEPTION`, `SIGSEGV`, `AndroidRuntime`, `Can't add component`, or `SphereCollider` matches in the post-install log sample | ADB logcat sample after launch |
| Web build | Development Web build — exit 0; 21 files, 132,058,830 bytes; `index.html` present | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Local Web serve | `python -m http.server 8020 --directory Builds/M11/Web`; `http://127.0.0.1:8020/index.html` returned HTTP 200 | local server/check output |
| Browser smoke | Chrome loaded the local build; post-fix fresh tab reported zero JavaScript error entries | `Docs/QA/Visual/latest-head-web.png` |

## Fix included in this baseline

`Assets/BattleRaja/Presentation/Combat/CombatProjectilePool.cs` now references the concrete `SphereCollider` type before removing the generated primitive collider. This prevents IL2CPP/WebGL stripping from producing the runtime `Can't add component because class 'SphereCollider' doesn't exist!` error observed in the first Web/Android smoke pass.

## Warnings and limitations

- Unity batchmode logs contain non-fatal licensing-handshake messages, obsolete `PlayerSettings` API warnings, and expected Fusion/native-extension warnings.
- The screenshots are technical smoke evidence, not visual-approval evidence. The current scene is still a greybox/prototype with overlapping HUD text and has not passed visual QA.
- Only the connected Lava phone was used, per project instruction. The connected Oppo device was intentionally not used.
- No public deployment, store submission, production signing, service credential handling, or multiplayer claim was made.

## Post-checkpoint runtime revalidation

After the authority seam commit `1a92b6c`, the runtime artifacts were rebuilt and smoke-tested again:

- Web: 21 files, 132,170,851 bytes; local HTTP `200`; Chrome fresh-tab smoke reported 0 JavaScript errors. Updated screenshot: `Docs/QA/Visual/latest-head-web.png`.
- Android: 150,813,125 bytes; SHA-256 `A6176D2BCE8A7856555F512B56C9A2679590E601B93F670CAC5FCD8AD4DDA455`; installed/launched on Lava `ST5GDW23LB004392` only (PID `23626`) with the Unity game activity focused. No fatal, crash, SphereCollider or AndroidRuntime matches were found in the post-launch log sample. Updated screenshot: `Docs/QA/Visual/latest-head-android.png`.
- Authority/statistics/tick regression: 67 EditMode and 27 PlayMode tests pass after the focused foundation changes.
