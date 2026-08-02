# Latest HEAD baseline

Date: 2026-08-03
Branch: `codex/product-completion`
Latest validated source HEAD: `b7d1a60` (`fix: preserve fighter-specific bot abilities`)
Latest runtime-bearing candidate: `4391f09` (`feat: add replayable tutorial arena`)
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

## Phase 1 authority/tick continuation (`ac60062`)

The following checks were rerun after the authority-tick changes. The user-owned
`MovementLab.unity` scene remained unstaged; the Bijli PlayMode test now places its
fixture actor explicitly so scene-local wall edits cannot change the movement assertion.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode authority/tick suite | 83 passed, 0 failed | `Builds/M11/TestResults/phase1-authority-editmode-20260803.xml` |
| Fixed-clock render-rate equivalence | `CoreFoundationTests.FixedClockProducesTheSameOfflineMatchStateAcrossRenderRates` passed for 30/60/90 Hz and a variable render cadence | `Builds/M11/TestResults/phase1-authority-editmode-20260803.xml` |
| Gadget authority cooldown | `AuthorityAdvancesGadgetCooldownOnAuthoritativeTicks` passed after 300 authoritative 30 Hz ticks | `Builds/M11/TestResults/phase1-authority-editmode-20260803.xml` |
| Aandhi intent tick identity | Authority test passed with `MatchAuthorityTick.SimulationTick == DamageRequest.SimulationTick` | `Builds/M11/TestResults/phase1-authority-editmode-20260803.xml` |
| Isolated Bijli regression | 1 passed, 0 failed after explicit fixture placement | `Builds/M11/TestResults/phase1-authority-bijli-fixture-20260803.xml` |
| PlayMode authority/tick suite | 33 passed, 0 failed | `Builds/M11/TestResults/phase1-authority-playmode-v2-20260803.xml` |

The new authority seam is still offline-only. Presentation gadget effect execution
remains outside the application authority, and the Android artifact has not yet been
installed on the connected Lava device for this exact `1437e5c` source.

## Phase 1 authority collection continuation (`1437e5c`)

Item proximity and collector selection now run in `OfflineMatchAuthority` from
validated domain positions/radii. The controller applies returned collection intents
to Unity health/inventory components and synchronizes view availability; it no longer
chooses collectors by scene iteration order.

| Check | Command/result | Evidence |
| --- | --- | --- |
| EditMode collection suite | 84 passed, 0 failed | `Builds/M11/TestResults/phase1-authority-collection-editmode-v2-20260803.xml` |
| PlayMode collection regression | 33 passed, 0 failed | `Builds/M11/TestResults/phase1-authority-collection-playmode-v2-20260803.xml` |
| Authority collection rule | Deterministic lowest-ID eligible collector, health/full-health filtering, authored position/radius and non-contiguous pickup-ID lookup covered by `AuthorityFoundationTests` | `Assets/BattleRaja/Core/Application/OfflineMatchAuthority.cs`, `Assets/BattleRaja/Tests/EditMode/AuthorityFoundationTests.cs` |
| Android development smoke build | Unity exit 0; APK 151,198,767 bytes; SHA-256 `360A4A0F4A595E5714B579226D6A157E06AA7992F1B3285DF7C4C26C7A43438C` | `Builds/M11/Logs/phase1-authority-collection-android-20260803.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development smoke build | Unity exit 0; 21 files, 132,979,355 bytes; `Build/Web.wasm` 120,294,960 bytes; SHA-256 `E957A343680D4C0205BFB2806946C0CEB5F95FD868AFA61D9FEA1199B9D048D1` | `Builds/M11/Logs/phase1-authority-collection-web-20260803.log`, `Builds/M11/Web` |
| Local Web serve | `python -m http.server 8136 --bind 127.0.0.1 --directory Builds/M11/Web`; `curl -I http://127.0.0.1:8136/index.html` returned HTTP 200 | local server check from 2026-08-03 |
| Lava physical smoke | Not run: `adb devices -l` listed only Oppo `b60e53b3`; the instructed Lava serial `ST5GDW23LB004392` was absent, so no other device was used | device-gate blocker |

This phase does not claim full visual/browser interaction correctness, real Photon
multiplayer, or authoritative gadget effect execution. The previous browser bootstrap
evidence remains the latest browser runtime evidence.

## Phase 2 fighter-controller continuation (`b7d1a60`)

`BotBrain` no longer defaults a missing reference to `BijliFighterController`. It resolves
the attached `IFighterAbilityController`, preserving Pehel and Maya ability identity when
scene serialization omits an explicit reference.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode suite | 84 passed, 0 failed | `Builds/M11/TestResults/phase2-fighter-editmode-20260803.xml` |
| Targeted controller test | 1 passed, 0 failed; every production bot resolves the controller attached to its actor | `Builds/M11/TestResults/phase2-fighter-bot-controller-playmode-v2-20260803.xml` |
| Full PlayMode suite | 34 passed, 0 failed | `Builds/M11/TestResults/phase2-fighter-playmode-20260803.xml` |

This is a controller-boundary correction, not proof of final fighter balance, authored
animation/VFX/audio, human counterplay approval, or real network authority. The latest
Android/Web artifacts remain the `1437e5c` smoke builds until a new candidate is built.

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

## Canvas match UI foundation validation (`380781b`)

- Repository validation: `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings.
- Regression: EditMode 72/72 and PlayMode 29/29 passed after the Canvas HUD and Unity 6
  `LegacyRuntime.ttf` runtime-font correction (`Builds/M11/TestResults/phase5-ui-20260803-editmode-final.xml`,
  `Builds/M11/TestResults/phase5-ui-20260803-playmode-final.xml`). The first PlayMode
  run caught the unsupported `Arial.ttf` resource and was not counted as final evidence.
- Web build: development build succeeded under Unity 6000.5.6f1 (`Builds/M11/Logs/phase5-ui-20260803-web-build-final2.log`), 19 files and 132,679,391 bytes; the main WASM SHA-256 is `611E64BE25E6C953BA72EC7F7DDE268581E3EB4EF42C9B330382AAF5F161A6F8`.
- Chrome Web smoke: `http://127.0.0.1:8021/index.html` returned HTTP 200; the fresh tab exposed the Unity player controls and captured zero error/warning entries. Screenshot: `Docs/QA/Visual/phase5-ui-20260803-web.png`.
- Android build: development IL2CPP APK succeeded (`Builds/M11/Logs/phase5-ui-20260803-android-build-final2.log`). Actual artifact size is 151,033,372 bytes; SHA-256 is `3D83BE32A990F66DE000A7F34A00AF75B54A85CDBF1346F93321304409464789`.
- Android smoke: installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), package `com.example.battleraja.m11`. The post-launch log sample contains zero fatal/crash/missing-component/Unity-error markers (`Builds/M11/Logs/phase5-ui-20260803-android-logcat-final.txt`). Screenshot: `Docs/QA/Visual/phase5-ui-20260803-android.png`.
- Product boundary: the Canvas surface now covers match/zone status, pause/settings,
  spectator cycling, results/rematch and left-handed/reduced-flash/high-contrast hooks.
  Bootstrap/main-menu flow, full tutorial/offline progression, localization assets,
  controller rebinding, functional aim assist, final authored UI and human visual review
  remain open. The scene and screenshots are still stylised greybox evidence, not a
  release or store-readiness claim.

## Production flow and fighter selection validation (`2c36bbb`)

- Source/control: pushed branch `codex/product-completion` is at `2c36bbb`; the user-owned
  dirty `MovementLab.unity` and Burst WASM files were explicitly excluded from the commit.
- Compile/validation: `BattleRaja.Editor.BuildEntrypoints.ValidateProject` exited 0 and
  `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe`
  reported 0 errors and 0 warnings (`Builds/M11/Logs/flow-compile-20260803c.log`).
- EditMode: 77 passed, 0 failed (`Builds/M11/TestResults/flow-editmode-final-20260803.xml`).
- PlayMode: 31 passed, 0 failed (`Builds/M11/TestResults/flow-playmode-final-20260803.xml`).
  This includes Bootstrap main-menu/offline/online-error routing and the Bazaar runtime
  Pehel controller-binding test.
- Android: development IL2CPP build succeeded with Bootstrap and Bazaar scenes in build
  order (`Builds/M11/Logs/flow-android-head-2c36bbb.log`). Actual APK size is 151,092,503
  bytes; SHA-256 is `B664BD85DAD93C6151A93E3C93DEE7640A077F395C2246E71AB3E802BBE83856`.
  No install or physical-device claim was made in this flow turn.
- Web: development build succeeded with Bootstrap and Bazaar scenes
  (`Builds/M11/Logs/flow-web-head-2c36bbb.log`), 19 files and 132,757,798 bytes. Main WASM
  is 120,095,092 bytes with SHA-256
  `8FAE0E0063B46B97CCC5579010E52BF1076C8839E9089DD3723F7A2EC41E7CBF`.
- Web runtime: local HTTP `http://127.0.0.1:8123/index.html` returned 200. At 1280×720,
  browser inspection showed Main Menu, Mode Selection, Fighter Selection (including Maya),
  Settings and the main-menu Online path's explicit Photon-unavailable error. Screenshots
  are stored under `Docs/QA/Visual/Flow/`. Browser logs contained no errors/warnings in
  the final HEAD smoke.
- Boundary: this is production-flow evidence, not final visual QA. The UI remains original
  stylised greybox presentation; tutorial, multi-viewport visual gate, Android Lava runtime
  check for this exact APK, performance profiling, real Photon and real PlayFab remain open
  or externally blocked.

## Replayable tutorial arena validation (`4391f09`)

- Source/control: pushed branch `codex/product-completion` is at `4391f09`; the existing
  user-owned `MovementLab.unity`, Burst WASM and audit-brief files remain unstaged.
- Compile/validation: `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` reported 0 errors and 0 warnings.
- EditMode: 81 passed, 0 failed, 0 skipped (`Builds/M11/TestResults/tutorial-editmode-20260803.xml`). This includes the pure ordered/replayable `TutorialStepMachine` and the tutorial flow route.
- PlayMode: 33 passed, 0 failed, 0 skipped (`Builds/M11/TestResults/cleanup-playmode-20260803.xml`). `TutorialArenaPlayModeTests` loaded the real `OfflineMatchController`, observed eight valid authority participants, verified all bot brains are disabled while actors remain in the simulation, and advanced the overlay to completion. `OfflineMatchPlayModeTests.RepeatedProductionSceneLoadsKeepOneOfflineRuntimeGraph` loaded the production arena three times and found one match controller, eight authority actors and a reset time scale on every iteration.
- Android: development IL2CPP build succeeded under Unity 6000.5.6f1 (`Builds/M11/Logs/tutorial-android-head-4391f09.log`). Actual APK `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk` is 151,120,618 bytes; SHA-256 `5366ACE6BA99BA25370E974399D70D0D76327F07D842F3C583D8B9C9A3C7C260`. This turn did not install this exact APK on a physical device; Lava remains the only permitted device for a later runtime check.
- Web: development build succeeded with Bootstrap, Tutorial Arena and Bazaar Bastion (`Builds/M11/Logs/tutorial-web-head-4391f09.log`), 19 files and 132,784,825 bytes. Main WASM is 120,095,092 bytes with SHA-256 `EF4548D592B4B7263B418192DBC0DBA478B1630E5AA5B30E5C28A531BEB11392`.
- Local Web serve: `http://localhost:8123/index.html` returned HTTP 200 from `Builds/M11/Web-BazaarBastion`.
- Browser smoke: at 1280×720, the fresh Web tab opened Main Menu → `TUTORIAL REPLAY`, rendered the real Tutorial Arena, and advanced Movement → Aim. Screenshots: `Docs/QA/Visual/Flow/web-tutorial-arena.png` and `Docs/QA/Visual/Flow/web-tutorial-aim.png`. Browser logs contained no errors; one non-fatal Unity WebGL warning reports that manual `persistentDataPath` synchronization is deprecated.
- Boundary: tutorial prompts are replayable guidance layered over the real controls, match authority, Aandhi/HUD and pickups; they do not automatically certify player competency. Multi-viewport visual QA, Lava smoke for this exact APK, performance/soak evidence, final authored presentation, real Photon and real PlayFab remain open or externally blocked.

## Visual and interaction QA validation (`511f2f4` source / `4391f09` runtime)

- Local Web candidate served successfully at `http://localhost:8123/index.html` and was inspected in a fresh in-app browser tab at `1280×720`.
- Captured states include main menu, mode selection, fighter selection, match loading/opening, active combat, Aandhi closing pressure, pause/settings and the explicit online/Fusion error. Evidence is stored under `Docs/QA/Visual/Phase7/` and summarized in `Docs/QA/VISUAL_QA_REPORT.md`.
- The available browser surface did not expose viewport resizing, so 1920×1080, 1440×900, 1024×768 and portrait checks were not claimed. Successful gadget pickup/use, spectator, results/rematch and final visual approval were not reached through the honest smoke path.
- This is an **In progress** visual gate. The screenshots show a stylised greybox/prototype with dense HUD coverage; they are not final visual approval.

## Visual and interaction QA Playwright revalidation (`511f2f4` source / `4391f09` runtime)

- The same `Builds/M11/Web-BazaarBastion` candidate was served at `http://localhost:8124/index.html` and exercised with the Playwright CLI.
- Required desktop viewports were captured: `1920×1080`, `1440×900`, `1280×720` and `1024×768`. Main-menu evidence exists for all four; match-opening evidence exists for all four. Portrait `390×844` was also captured because the browser supports resizing.
- Desktop menu, mode, fighter, match, pressure, spectator, settings and online-error surfaces were visually inspected. Evidence is under `Docs/QA/Visual/Phase7/playwright-*.png` and summarized in `Docs/QA/VISUAL_QA_REPORT.md`.
- Portrait menu fits, but portrait gameplay is horizontally cropped and the tutorial overlay is clipped. Gadget pickup/use, a distinct loading surface and results/rematch were not captured through the honest run.
- This remains an **In progress** visual gate and does not establish final art quality, mobile-Web readiness, physical Lava ergonomics, or human approval.

## Fresh latest-HEAD rebaseline (`1a41c09`)

- Repository validation: `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` returned 0 errors and 0 warnings.
- Unity compile/import: Unity `6000.5.6f1` completed the batchmode import/compile path with exit code 0. Photon Fusion editor import emitted a non-fatal custom-dependency scheduling warning; no compiler error was observed.
- EditMode: 81 passed, 0 failed, 0 skipped (`Builds/M11/TestResults/rebaseline-20260803-editmode-v4.xml`).
- PlayMode: 33 passed, 0 failed, 0 skipped (`Builds/M11/TestResults/rebaseline-20260803-playmode-v1.xml`).
- Android: fresh development IL2CPP build succeeded (`Builds/M11/Logs/rebaseline-20260803-android-v1.log`). `Builds/M11/Android/BattleRaja-M11.apk` is 151,120,318 bytes; SHA-256 `1B74943F56D3474238320819F853D2C21DF5AB2648CDB0DC88929F85E328393E`. This exact APK was not installed; the Lava-only physical-device gate remains open.
- Web: fresh development build succeeded (`Builds/M11/Logs/rebaseline-20260803-web-v1.log`). The output contains 19 build files (excluding the two older DOM-check text files) totaling 132,773,639 bytes; `Build/Web.wasm` is 120,109,688 bytes with SHA-256 `EF4548D592B4B7263B418192DBC0DBA478B1630E5AA5B30E5C28A531BEB11392`.
- Local Web serve: `python -m http.server 8130` from `Builds/M11/Web`; `http://127.0.0.1:8130/index.html` returned HTTP 200.
- Browser bootstrap: Playwright loaded the fresh URL in Chromium; the Unity player title and controls appeared, and the console reported 0 errors and 0 warnings. This is bootstrap evidence, not visual gameplay approval.
- Build warnings/limitations: existing obsolete Unity `FindObjectsByType` API warnings and non-fatal licensing/Fusion/native-extension messages remain. The build's internal websockify helper also logged an `EADDRINUSE` shutdown warning while the player build itself reported success. Build-generated changes to `Bootstrap.unity` and `TutorialArena.unity` were restored because those files were clean before the baseline.
