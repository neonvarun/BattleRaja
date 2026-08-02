# Latest HEAD baseline

Date: 2026-08-03
Branch: `codex/product-completion`
Latest validated source HEAD: `4ecc467` (`feat: add readable visual and audio presentation foundation`)
Unity: `6000.5.6f1` (`C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe`)

## Scope and repository note

The requested goal path `Docs/AI/RepositoryAuditAndCompletionGoal.md` is absent. The matching file `Docs/AI/BattleRaja_Repository_Audit_and_Completion_Goal.md` was read in full and used as the authoritative continuation brief. This path discrepancy remains a documentation issue; no source from the requested path was available.

The baseline intentionally excludes unrelated working-tree changes in `Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity` and `Data/Plugins/lib_burst_generated.wasm`. Those files were not staged or altered by this baseline work.

## Validation results

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-02 |
| Unity compile | Unity batchmode compile — exit 0; no compiler failure markers | `Builds/M11/Logs/phase1-clock-20260803-compile.log` |
| EditMode tests | 71 passed, 0 failed | `Builds/M11/TestResults/phase1-clock-20260803-editmode.xml` |
| PlayMode tests | 27 passed, 0 failed | `Builds/M11/TestResults/phase1-clock-20260803-playmode.xml` |
| Phase 1 timeout regression | Deterministic health/distance/id ranking; complete placements; EditMode 59/59 and PlayMode 27/27 pass | `Builds/M11/TestResults/phase1-editmode.xml`, `Builds/M11/TestResults/phase1-playmode.xml` |
| Fixed-clock integration | 30 Hz accumulator separates render frames from authoritative match steps; EditMode 60/60 and PlayMode 27/27 pass | `Builds/M11/TestResults/clock-editmode.xml`, `Builds/M11/TestResults/clock-playmode.xml` |
| Aandhi interpolation | Zone radius is continuous across opening/pressure transitions; EditMode 60/60 and PlayMode 27/27 pass | `Builds/M11/TestResults/aandhi-editmode.xml`, `Builds/M11/TestResults/aandhi-playmode.xml` |
| Bot zone awareness | Bot perception includes current/next zone and the decision engine repositions proactively; EditMode 61/61 and PlayMode 27/27 pass | `Builds/M11/TestResults/botzone-editmode.xml`, `Builds/M11/TestResults/botzone-playmode.xml` |
| Authority seam | `OfflineMatchAuthority` owns zone-damage cadence and emits immutable `DamageRequest` intents; EditMode 62/62 and PlayMode 27/27 pass | `Builds/M11/TestResults/authority-editmode.xml`, `Builds/M11/TestResults/authority-playmode.xml` |
| Combat statistics | Instigator-aware events record damage, eliminations, survival time and duplicate-credit prevention; EditMode 70/70 and PlayMode 27/27 pass | `Builds/M11/TestResults/fighter-final-editmode.xml`, `Builds/M11/TestResults/fighter-final-playmode.xml` |
| Aandhi warning/preview | Warning state, remaining warning time and next-radius preview are part of the immutable tick result; EditMode 70/70 and PlayMode 27/27 pass | `Builds/M11/TestResults/fighter-final-editmode.xml`, `Builds/M11/TestResults/fighter-final-playmode.xml` |
| Tick-based presentation timing | Player movement, Bijli ability, projectile stepping, gadget timers and weapon attack cooldown consume a 30 Hz clock; EditMode 70/70 and PlayMode 27/27 pass | `Builds/M11/TestResults/fighter-final-editmode.xml`, `Builds/M11/TestResults/fighter-final-playmode.xml` |
| Authority item collection | Pickup availability/respawn and gadget collection are decided in `OfflineMatchAuthority`; EditMode 70/70 and PlayMode 27/27 pass | `Assets/BattleRaja/Core/Domain/MatchItems.cs`, `Builds/M11/TestResults/fighter-final-editmode.xml`, `Builds/M11/TestResults/fighter-final-playmode.xml` |
| Fighter-specific ability boundary | `IFighterAbilityController` selects fixed-tick Pehel charge/capture/throw and Maya targetable decoy adapters; domain tests pass, but regenerated-scene runtime coverage is still open | `Assets/BattleRaja/Presentation/Combat/PehelFighterController.cs`, `Assets/BattleRaja/Presentation/Combat/MayaFighterController.cs`, `Builds/M11/TestResults/fighter-final-editmode.xml` |
| Android build | Latest validated development IL2CPP APK — exit 0; Unity build report succeeded | `Builds/M11/Logs/phase1-clock-2c6958a-android-build.log` |
| Android artifact | 150,927,346 bytes; SHA-256 `4AB8D6537CC7BFD2F06547D3938E1D11C379830C3928EA15A01F6F77EA7C637B` | `Builds/M11/Android/BattleRaja-M11.apk` |
| Android device smoke | Installed and launched on Lava `ST5GDW23LB004392` only; focused activity `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`; process `26865` observed | `Docs/QA/Visual/phase1-clock-2c6958a-android.png`; `Builds/M11/Logs/phase1-clock-2c6958a-android-logcat.txt` |
| Android runtime scan | No `FATAL EXCEPTION`, `SIGSEGV`, `AndroidRuntime`, `Can't add component`, or `SphereCollider` matches in the post-install log sample | ADB logcat sample after launch |
| Web build | Latest validated development Web build — exit 0; 21 files, 132,400,579 bytes; `index.html` present | `Builds/M11/Logs/phase1-clock-2c6958a-web-build.log`, `Builds/M11/Web` |
| Local Web serve | `python -m http.server 8020 --directory Builds/M11/Web`; `http://127.0.0.1:8020/index.html` returned HTTP 200 | local server/check output |
| Browser smoke | Chrome loaded the latest validated build in a fresh tab; DOM exposed the Unity player controls and post-load error/warning log query returned zero entries | `Docs/QA/Visual/phase1-clock-2c6958a-web.png` |

## Fix included in this baseline

`Assets/BattleRaja/Presentation/Combat/CombatProjectilePool.cs` now references the concrete `SphereCollider` type before removing the generated primitive collider. This prevents IL2CPP/WebGL stripping from producing the runtime `Can't add component because class 'SphereCollider' doesn't exist!` error observed in the first Web/Android smoke pass.

## Warnings and limitations

- Unity batchmode logs contain non-fatal licensing-handshake messages, obsolete `PlayerSettings` API warnings, and expected Fusion/native-extension warnings.
- The screenshots are technical smoke evidence, not visual-approval evidence. The current scene is still a greybox/prototype with overlapping HUD text and has not passed visual QA.
- Only the connected Lava phone was used, per project instruction. The connected Oppo device was intentionally not used.
- No public deployment, store submission, production signing, service credential handling, or multiplayer claim was made.

## Post-checkpoint runtime revalidation

## Bazaar Bastion vertical-slice validation (`579bc37`)

- Compile/validation: Unity `ValidateProject` completed with exit 0; repository validation reported 0 errors and 0 warnings (`Builds/M11/Logs/bazaar-final-20260803-compile.log`).
- Regression: EditMode 72/72 and PlayMode 28/28 passed (`Builds/M11/TestResults/bazaar-final-20260803-editmode.xml`, `Builds/M11/TestResults/bazaar-final-20260803-playmode.xml`). The PlayMode suite loaded `BazaarBastion` and asserted the Pehel/Maya fighter-specific controllers and `BazaarArchitecture` root.
- Android build: development IL2CPP APK succeeded under Unity 6000.5.6f1 (`Builds/M11/Logs/bazaar-runtime-20260803-android-build.log`). Artifact `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk`: 150,960,195 bytes; SHA-256 `2A4AC8ACDB4A7873F07362E67B0ACA8B265C44840E66CC3A6AC8EB648B88D175`.
- Android smoke: installed and launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), package `com.example.battleraja.m11`. Screenshot: `Docs/QA/Visual/bazaar-runtime-579bc37-android.png`; logcat: `Builds/M11/Logs/bazaar-runtime-579bc37-android-logcat.txt`. No fatal Unity/Android crash markers were observed; the sample includes non-fatal device noise and a Google Play AssetPackManager `ClassNotFoundException` from the development player, which is not a crash but remains a release-hardening follow-up.
- Web build: development Web build succeeded (`Builds/M11/Logs/bazaar-runtime-20260803-web-build.log`), 18 files, 132,379,495 bytes, served from `http://127.0.0.1:8021/index.html` with HTTP 200.
- Browser smoke: Chrome loaded the fresh local Web tab, exposed the Unity player controls, and returned zero captured error/warning entries after load. Screenshot: `Docs/QA/Visual/bazaar-runtime-579bc37-web.png`.
- Visual boundary: the scene is a stylised greybox with Bazaar palette blocks/stalls and overlapping prototype HUD text. This is technical vertical-slice evidence, not final visual approval.

## Visual/audio foundation validation (`4ecc467`)

- Compile: Unity `ValidateProject` completed with exit 0; the first visual-pass compile caught a missing `CombatFaction.Ally` reference, which was corrected before the passing compile (`Builds/M11/Logs/phase4-visual-20260803-compile-v3.log`).
- Regression: EditMode 72/72 and PlayMode 29/29 passed after regenerating Bazaar Bastion with `FighterPresentation` and `BattleRajaAudioDirector` (`Builds/M11/TestResults/phase4-visual-20260803-editmode.xml`, `Builds/M11/TestResults/phase4-visual-20260803-playmode-v2.xml`).
- Android build: development IL2CPP APK succeeded (`Builds/M11/Logs/phase4-visual-20260803-android-build.log`). Artifact: 151,031,756 bytes; SHA-256 `A26456EB4FC447E35AA10421361BD0F1818B1622356D2003686CCE4B9B1A5C4B`.
- Android smoke: installed/launched only on Lava `ST5GDW23LB004392`; no fatal Unity/Android crash markers, missing-component errors or null-reference markers were observed in the captured sample (`Builds/M11/Logs/phase4-visual-20260803-android-logcat.txt`). Screenshot: `Docs/QA/Visual/phase4-visual-20260803-android.png`.
- Web build: development Web build succeeded (`Builds/M11/Logs/phase4-visual-20260803-web-build.log`), 19 files, 132,677,491 bytes; local HTTP `200` from `http://127.0.0.1:8021/index.html`.
- Chrome smoke: the fresh tab loaded the Unity player, exposed the expected Profile/Unload controls, and returned zero captured error/warning entries. Screenshot: `Docs/QA/Visual/phase4-visual-20260803-web.png`.
- Presentation boundary: original procedural placeholder cues are gesture-gated for Web and optional mixer hooks are present, while imported animation clips, authored VFX/music/SFX, reduced-flash quality profiles and final visual approval remain open.

The current latest-head revalidation supersedes the earlier post-authority artifact values above:

- HEAD: `2c6958a` (`fix: finish fixed tick bot and winner seams`).
- Compile: exit 0; EditMode 71/71; PlayMode 27/27.
- Android: `Builds/M11/Android/BattleRaja-M11.apk`, 150,927,346 bytes, SHA-256 `4AB8D6537CC7BFD2F06547D3938E1D11C379830C3928EA15A01F6F77EA7C637B`; installed/launched on Lava `ST5GDW23LB004392` only; no fatal/crash/AndroidRuntime/SphereCollider markers in the captured logcat sample.
- Web: 21 files, 132,400,579 bytes; local HTTP 200; fresh Chrome tab loaded with zero captured error/warning entries; screenshot `Docs/QA/Visual/phase1-clock-2c6958a-web.png`.

After the authority seam commit `1a92b6c`, the runtime artifacts were rebuilt and smoke-tested again:

- Web: 21 files, 132,170,851 bytes; local HTTP `200`; Chrome fresh-tab smoke reported 0 JavaScript errors. Updated screenshot: `Docs/QA/Visual/latest-head-web.png`.
- Android: 150,813,125 bytes; SHA-256 `A6176D2BCE8A7856555F512B56C9A2679590E601B93F670CAC5FCD8AD4DDA455`; installed/launched on Lava `ST5GDW23LB004392` only (PID `23626`) with the Unity game activity focused. No fatal, crash, SphereCollider or AndroidRuntime matches were found in the post-launch log sample. Updated screenshot: `Docs/QA/Visual/latest-head-android.png`.
- Authority/statistics/tick/item/fighter regression: 70 EditMode and 27 PlayMode tests pass after the focused foundation changes.
