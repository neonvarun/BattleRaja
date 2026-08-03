# Latest HEAD baseline

Date: 2026-08-03
Branch: `codex/product-completion`
Latest validated runtime source HEAD: `d993a5b` (`refactor: route Maya decoy through authority`)
Latest runtime-bearing candidate: `d993a5b` (authority-first movement/gadget/fighter displacement/Maya decoy/damage/healing Android/Web candidates)
Unity: `6000.5.6f1` (`C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe`)

## Authority-driven Maya decoy continuation (`d993a5b`, 2026-08-03)

Production Maya decoys are now owned by `OfflineMatchAuthority`: spawn, follow position,
remaining lifetime, health and duplicate-tick damage are resolved in the pure/application
path. `MayaFighterController` consumes immutable snapshots and creates only the generated
capsule view. The local non-authority probe path remains available; this is not a real
network-server claim.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 100 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-maya-decoy-editmode-full-20260803.xml` |
| PlayMode tests | 51 passed, 0 failed, 0 skipped; includes production Maya authority lifetime/damage | `Builds/M11/TestResults/authority-maya-decoy-playmode-full-20260803.xml`, `Builds/M11/TestResults/authority-maya-decoy-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,512,643 bytes; SHA-256 `F12EEBB2A6E8968992B38F9446E1D9B55A5C2BD0EBB0F6AB0306A23786E9BEB9` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); `UnityPlayerGameActivity` remained top-resumed; sampled logcat had 0 fatal/AndroidRuntime/SIGSEGV markers | ADB output from 2026-08-03; `Builds/M11/Logs/android-lava-authority-maya-20260803.png` |
| Web build/serve | 21 files, 133,498,294 bytes; `Web.wasm` 120,789,716 bytes, SHA-256 `FE33609725B6C8AE097A56F1C29928B41EB8DBCBAFB42217129018E5DB2B97DD`; local port 8137 returned HTTP 200 | `Builds/M11/Web`, `Builds/M11/Logs/web-build.log` |

## Authority-driven fighter displacement continuation (`9100e69`, 2026-08-03)

Bijli dash, Pehel charge and Pehel throw now submit resolved displacement through a
tick-validated `OfflineMatchAuthority` seam whenever the actor uses production
authority-driven movement. Non-authority lab fixtures retain their local controller
fallback. This closes the disabled-`CharacterController` production behavior gap, but
does not claim server-owned ability runtime state or network authority.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 99 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-ability-displacement-editmode-full-rerun-20260803.xml` |
| PlayMode tests | 50 passed, 0 failed, 0 skipped; includes the live Bijli authority displacement test | `Builds/M11/TestResults/authority-ability-displacement-playmode-full-rerun-20260803.xml`, `Builds/M11/TestResults/authority-bijli-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,465,096 bytes; SHA-256 `00D83717FAC958A588B4F4964504C92ACA70CDE23DFE800C99234CC48A6ED3E8` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity` remained top-resumed; sampled logcat had no fatal/AndroidRuntime marker | ADB output from 2026-08-03; `Builds/M11/Logs/android-lava-authority-bijli-20260803.png` |
| Web build/serve | 21 files, 133,477,944 bytes; `Web.wasm` SHA-256 `F6C37FFC4D3CAE8DE87BDFAF5202FE32F7FF504AE66BB4C55D7834C50852D6EB`; local port 8137 returned HTTP 200 | `Builds/M11/Web`, `Builds/M11/Logs/web-build.log` |

## Authority-driven Dhol displacement continuation (`78fa990`, 2026-08-03)

Dhol Burst now applies each validated displacement to the canonical participant position
before returning the immutable effect. The presentation adapter consumes the resulting
snapshot, so production authority movement no longer depends on a local
`CharacterController.Move` call. Pehel charge displacement, Bijli dash displacement and
real network authority remain open.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 98 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-gadget-displacement-editmode-20260803.xml` |
| PlayMode tests | 49 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-gadget-displacement-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,472,675 bytes; SHA-256 `B8D6DCEB124B0CA9DA42A2E341720B7AC7F325FA734FDD2EA3650F9F3B53AF80` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); top-resumed Unity activity observed with process id 8128 and zero sampled fatal/AndroidRuntime markers | ADB output from 2026-08-03; `Builds/M11/Logs/android-lava-authority-gadget-20260803.png` |
| Web build/serve | 21 files, 133,469,527 bytes; `Web.wasm` SHA-256 `C51E0EBB3B086C16EAA0C079532FC37C0015CD827F437B89C009DF892F4846F6`; local port 8137 returned HTTP 200 | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

## Authority-driven production movement continuation (`204e4f0`, 2026-08-03)

Bazaar Bastion now routes actor movement commands through `OfflineMatchAuthority` at the
fixed simulation tick. The pure motor runs once per actor/tick, canonical positions are
stored in `OfflineMatchSimulation`, duplicate ticks are rejected, and the presentation
adapter applies the returned snapshot. MovementLab intentionally retains its local
observation path for movement fixtures. Fighter ability displacement and real network
authority remain open.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 98 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-movement-editmode-20260803-final.xml` |
| PlayMode tests | 49 passed, 0 failed, 0 skipped, including `ProductionMatchRoutesMovementThroughAuthoritySnapshots` | `Builds/M11/TestResults/authority-movement-playmode-20260803-final2.xml` |
| Android build | `BattleRaja-M11.apk`, 151,468,901 bytes; SHA-256 `378B0474577A575139FEE1F797EF848C2FC27335CCA69427F64297860A768A04` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); top-resumed Unity activity observed with process id 7419 and zero sampled fatal/AndroidRuntime markers | ADB output from 2026-08-03; `Builds/M11/Logs/android-lava-authority-movement-20260803.png` |
| Web build/serve | 21 files, 133,467,594 bytes; `Web.wasm` SHA-256 `DFA4599679634349459DBD68973467B929F9470D7FAC057A56B90D5C137C46FC`; local port 8137 returned HTTP 200 | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

## Authority-first healing continuation (`678acb0`, 2026-08-03)

Health pickups and Tiffin station healing now apply to canonical simulation health
through `OfflineMatchAuthority.ApplyHealing`; Unity applies only the resulting health
snapshot. The controller no longer mirrors view health back into authority every render
frame. The legacy `SyncHealth` method remains for test/server compatibility while movement
reconciliation is still incremental.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -ProjectRoot .` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 97 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-health-editmode-20260803.xml` |
| PlayMode tests | 48 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-health-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,415,947 bytes; SHA-256 `4B1BAF633BA448555B5D3E6C4C21666C61462499D0C3451CFE596AEA1234CFD3` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); Unity activity remained resumed and no sampled fatal crash marker was found | ADB output from 2026-08-03 |
| Android visual inspection | Portrait live match remains readable after canonical healing migration | `Docs/QA/Visual/Phase7/android-lava-authority-healing-20260803.png` |
| Web build/serve | 21 files, 133,374,016 bytes; `Web.wasm` SHA-256 `1DBDC8DA65514A660DC4651EF7116FB83C522C42F62E0678716B059F7B76438E`; local port 8137 returned HTTP 200 | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

Movement authority and remaining non-health presentation reconciliation are still open.

## Authority-first actor damage continuation (`08c6f2e`, 2026-08-03)

Production actor damage now resolves against `OfflineMatchAuthority` and canonical
simulation health/statistics before Unity applies the returned snapshot/event to the
view. Non-authority lab targets continue through the local presentation pipeline.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -ProjectRoot .` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 96 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-damage-editmode-20260803.xml` |
| PlayMode tests | 48 passed, 0 failed, 0 skipped, including authority-first actor damage and existing results/rematch/Maya coverage | `Builds/M11/TestResults/authority-damage-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,414,630 bytes; SHA-256 `5BF1657D1BDA0D059E72C48A6D85AA03441C6096DFFD679E4A8146FC6996B6C4` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); Unity activity remained resumed and no sampled fatal crash marker was found | ADB output from 2026-08-03 |
| Android visual inspection | Portrait live match remains readable with labeled touch controls after the authority change | `Docs/QA/Visual/Phase7/android-lava-authority-damage-20260803.png` |
| Web build/serve | 21 files, 133,372,172 bytes; `Web.wasm` SHA-256 `9D2101263269276649D596A9F8A229F97B6447483B9D0B20CDFB8E45B3C7C8E7`; local port 8137 returned HTTP 200 | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

This closes the duplicate/late actor-damage accounting gap for the offline authority.
Movement authority, health/pickup reconciliation, remaining gadget presentation adapters,
real Photon transport and final network/server authority remain open.

## Touch-control readability continuation (`a170746`, 2026-08-03)

The three runtime combat touch surfaces now create non-raycast labels for `ATTACK`,
`ABILITY` and `GADGET`. This keeps the scene controls data-light while making the
actions discoverable on Android and Web. The labels are covered by a PlayMode
regression and were inspected on the permitted Lava device.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -ProjectRoot .` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 95 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/touch-labels-editmode-retry-20260803.xml` |
| PlayMode tests | 47 passed, 0 failed, 0 skipped, including `TouchControlsExposeReadableActionLabels` | `Builds/M11/TestResults/touch-labels-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,407,488 bytes; SHA-256 `959D766A0D218F8D531B7BA85D0D8199A77A39E90DD0C23B0AAD8A3C85A5E18F` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); active Unity activity and no sampled fatal crash marker | ADB output from 2026-08-03 |
| Android visual inspection | Portrait match visibly shows all three labels on the touch surfaces | `Docs/QA/Visual/Phase7/android-lava-touch-labels-20260803.png` |
| Web build | 21 files, 133,364,566 bytes; `Web.wasm` SHA-256 `D75EE1355A111711A716C72AEFA6714252FC2C08943CC8AE4FBE707464885C69` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

This closes the technical readability gap only. Touch ergonomics, loading-state behavior,
gadget pickup/use, multi-browser coverage and final human presentation approval remain open.

## Authority and results/rematch continuation (`044b1b8`, 2026-08-03)

This is the current source/runtime checkpoint after the authority event-routing fix.
`OfflineMatchController` now reports resolved combat events through
`OfflineMatchAuthority.RecordDamage`; the production Web candidate was also driven
through a real Results screen and a subsequent Rematch transition in Chrome 150.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 95 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-routing-editmode-20260803.xml` |
| PlayMode tests | 46 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-routing-playmode-20260803.xml` |
| Android production smoke | APK 151,412,235 bytes; SHA-256 `9433136F638F622597DEB816E023999811208EB8827AE9A9ADE565AD17C39D87`; installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `27998`; crash-marker scan count 0 | `Builds/M11/Logs/android-build.log` and ADB output from 2026-08-03 |
| Web production smoke | 19 files, 133,358,779 bytes; `Web-BazaarBastion.wasm` SHA-256 `66D159C1291809BA04A3365A47976AA0042145E53F2335CA9BC545213A2BF6DA`; port 8139 HTTP 200 | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |
| Results capture | Chrome showed all eight placements, statistics, `REMATCH` and `MENU` controls at 1280x720; the exact APK also reached the portrait Results panel on Lava | `Docs/QA/Visual/Phase7/playwright-1280x720-results-20260803.png`, `Docs/QA/Visual/Phase7/android-lava-results-authority-20260803.png` |
| Rematch transition | Clicking `REMATCH` returned to a fresh live match; screenshot inspected after the transition; Chrome session recorded 61 messages, 0 errors and 1 known `JS_FileSystem_Sync` deprecation warning | `Docs/QA/Visual/Phase7/playwright-1280x720-rematch-match-20260803.png`, `.playwright-cli/console-2026-08-03T06-02-11-380Z.log` |
| Performance smoke | Chrome warm local run: DOMContentLoaded 152 ms, load 251.4 ms, WASM transfer 120,662,567 bytes, rAF mean/p95 5.620/6.1 ms, JS heap used 30,296,011 bytes; Lava PSS/RSS/Graphics PSS 458,974/596,588/95,468 KB; Android gfxinfo had no frame histogram | `Docs/QA/Performance/authority-runtime-20260803.md` and sibling raw captures |

The Results/rematch observation is technical interaction evidence, not human visual
approval. Gadget pickup/use remains an evidence gap; Android touch ergonomics remain
open for owner review. The known Unity API-obsolescence and Web persistent-data-path
warnings are non-fatal and are not counted as product crashes.

The repository then received docs/CI-only commit `2809165`; it does not change the
runtime-bearing code or the Android/Web artifact hashes recorded above.

## Production-flow continuation (`d1b33d1` + `4d3ae6a`)

The production flow now creates `InputSystemUIInputModule` when it has to create an
EventSystem, and the combat HUD binds its identity and ability text to the selected
fighter (Bijli, Pehel or Maya). The static formatter is covered independently so the
selection labels cannot silently regress to Bijli.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 94 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fighter-hud-final-editmode-20260803.xml` |
| PlayMode tests | 46 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fighter-hud-final-playmode-20260803.xml` |
| Production Android build | Unity `Build Finished, Result: Success`; APK 151,378,890 bytes; SHA-256 `F47DC800BF351A1AD5A29A48C40ECE633E76D14E967DDF6F391D5F6935C2F4D6` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk` |
| Lava smoke | Exact production APK installed/launched on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `26699`; no process death or fatal crash marker in the sampled log. Unity emitted the known optional Play Asset Delivery `AssetPackManager` class-probe warning. | ADB output from 2026-08-03 |
| Production Web build | Unity WebGL build completed; 19 files, 133,357,921 bytes; `Web-BazaarBastion.wasm` SHA-256 `BC3689B1D48DFD099DA8C619E9E9059EA709BB3D394C74B3411B17FCCFD03BAC` | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |
| Local production Web serve | `http://127.0.0.1:8139/index.html` returned HTTP 200; Chrome 150 loaded one Unity canvas and reported 52 console messages with 0 errors and 0 warnings after reload | Playwright/HTTP output from 2026-08-03 |
| Browser focus/timing observation | Canvas focus contract remains `tabindex=0`; prior local Chrome observation: DOMContentLoaded 401.7 ms, load 520.4 ms, WASM transfer 120,659,088 bytes, browser `requestAnimationFrame` p95 6.0 ms, JS heap used 30.2 MB | `Docs/QA/Visual/Phase7/playwright-input-system-runtime-20260803.png` and measurement output from 2026-08-03 |
| Lava memory observation | PSS 502,948 KB, RSS 639,560 KB, Graphics PSS 101,708 KB; `dumpsys gfxinfo` exposed no frame/jank histogram, so no FPS claim is made | `Builds/M11/Logs/input-system-lava-meminfo-20260803.txt`, `input-system-lava-gfxinfo-20260803.txt`, `input-system-lava-top-20260803.txt` |

The production Chrome captures show the Bazaar Bastion route and fighter-specific HUD
text for Pehel and Maya. They are still greybox/prototype evidence; the 390x844
MovementLab capture shows bot debug-label/HUD overlap, so visual approval, touch
ergonomics and final authored presentation remain human-review items.

## Maya bot-perception continuation (`ff2a3e4`)

Maya decoy spawn/destruction now refreshes bot perception targets. Decoys are deactivated
before deferred destruction so a bot cannot retain a stale target after the decoy has left
gameplay. `VerticalSlicePlayModeTests.MayaDecoySpawnsFollowsAndCanBeDestroyedByCombat`
now asserts that a sensor created before the decoy observes it after spawn.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 94 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/maya-bot-decoy-editmode-20260803.xml` |
| PlayMode tests | 46 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/maya-bot-decoy-playmode-20260803.xml` |
| Android production smoke | APK 151,385,439 bytes; SHA-256 `535620433C81BF9B146E783FCE5F05F779B1E1BCB7B9CBA1FF4724A123D5B876`; installed/launched on Lava `ST5GDW23LB004392` as process `27459`; crash-marker scan count 0 | `Builds/M11/Logs/android-build.log` and ADB output from 2026-08-03 |
| Web production smoke | 19 files, 133,358,415 bytes; `Web-BazaarBastion.wasm` SHA-256 `65BD44CD401E9A1AD16C684AD168F8E71B4932465DC719A4A2C2D38AE1DD580`; port 8139 HTTP 200; Chrome one canvas, 52 console messages, 0 errors and 0 warnings | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |

The Android build still reports the repository's existing Unity API-obsolescence warnings;
the Web wrapper still reports the local websockify port-35020 collision, but both Unity
builds report success. No visual-approval or performance-readiness claim is added by this
behavioral fix.

## Authority event-routing continuation (`8645254`)

`OfflineMatchAuthority.RecordDamage` is now the application-owned entry point for resolved
combat events. `OfflineMatchController` reports immutable `CombatDamageEvent` values through
that method instead of calling `OfflineMatchSimulation.RecordDamage` directly. This keeps
placements, eliminations, damage and assists behind the authority boundary while leaving
Unity responsible for applying view-side health changes.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 95 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-routing-editmode-20260803.xml` |
| PlayMode tests | 46 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-routing-playmode-20260803.xml` |
| Android production smoke | APK 151,412,235 bytes; SHA-256 `9433136F638F622597DEB816E023999811208EB8827AE9A9ADE565AD17C39D87`; exact APK installed/launched on Lava `ST5GDW23LB004392` as process `27998`; crash-marker scan count 0 | `Builds/M11/Logs/android-build.log` and ADB output from 2026-08-03 |
| Web production smoke | 19 files, 133,358,779 bytes; `Web-BazaarBastion.wasm` SHA-256 `66D159C1291809BA04A3365A47976AA0042145E53F2335CA9BC545213A2BF6DA`; port 8139 HTTP 200; Chrome one canvas, 52 console messages, 0 errors and 0 warnings | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |

This phase reduces the presentation-authority coupling but does not establish real network
authority, server transport, or release readiness.

## Scope and repository note

The requested goal path `Docs/AI/RepositoryAuditAndCompletionGoal.md` is absent. The matching file `Docs/AI/BattleRaja_Repository_Audit_and_Completion_Goal.md` was read in full and used as the authoritative continuation brief. This path discrepancy remains a documentation issue; no source from the requested path was available.

The baseline intentionally excludes unrelated working-tree changes in `Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity` and `Data/Plugins/lib_burst_generated.wasm`. Those files were not staged or altered by this baseline work.

## Input System-only continuation (`5f4566d`)

The project now uses Unity's Input System as the active handler. Generated scenes use
`InputSystemUIInputModule`, audio gesture detection uses Input System device state, and a
scene-load bridge keeps older serialized scenes runnable without rewriting their YAML.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| PlayMode tests | 45 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/input-system-playmode-fixed-20260803.xml` |
| EditMode baseline | 94 passed, 0 failed, 0 skipped on the immediately preceding rebaseline; a fresh invocation was blocked by pre-existing Unity editor processes holding the project lock | `Builds/M11/TestResults/rebaseline-editmode-20260803.xml` |
| Android development build | Unity log reports `Build Finished, Result: Success`; APK 151,373,009 bytes; SHA-256 `60AEF8C395B21E9C0CA5EF142411AB57214A9B2D50053BE5FF623544CF2D9812` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Lava smoke | Exact APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `23328`; sampled log has no fatal exception, SIGSEGV or missing-component marker | ADB command output from 2026-08-03 |
| Web development build | Unity log reports `Build Finished, Result: Success`; 21 files, 133,358,665 bytes; WASM 120,658,772 bytes; SHA-256 `790349B1D3203B173EDF8075D827426DD20AD74737C99FB7567C52E2FD54B5E2` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web/Build/Web.wasm` |
| Local Web serve | `http://127.0.0.1:8137/index.html` returned HTTP 200 | local HTTP check from 2026-08-03 |

The Web build log also contains the expected local websockify `EADDRINUSE` warning on
port 35020 because another local wrapper already owned that port; Unity completed the
player build successfully. Existing compiler deprecation warnings remain technical debt.

## Fresh latest-HEAD rebaseline (`9291d85`)

This is the current rebaseline requested after the Photon Fusion import. It was run
against the current project at Unity `6000.5.6f1`; the two pre-existing user-owned
tracked edits listed above remained untouched and no generated scene changes were
included in source control.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Unity/toolchain | Unity `6000.5.6f1`; AndroidPlayer, WebGLSupport and WindowsStandaloneSupport present; embedded ADB `36.0.0`; Lava `ST5GDW23LB004392` connected | command output from 2026-08-03 |
| Photon/package check | Photon Fusion `2.1.1 Stable 2177` files present under `Assets/Photon/Fusion`; Input System `1.20.0`; URP `17.5.0`; Test Framework `1.7.0` | `Assets/Photon/Fusion/build_info.txt`, `Packages/manifest.json` |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 89 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fresh-editmode-20260803.xml` |
| PlayMode tests | 43 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fresh-playmode-20260803.xml` |
| Android development build | `Tools\\Build\\Android\\build.ps1` completed; APK 151,312,094 bytes; SHA-256 `2C6E9861E8D1D1011B3EF3A1B891B8BC92FA075410B8DA97015AA4BEB4BCA353` | `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | `Tools\\Build\\Web\\build.ps1` completed; 21 files, 133,194,576 bytes; WASM SHA-256 `E029D2925F6E0B7463DA8F236584EE560F011EE0A59D99884A484E05EDA8EDAB` | `Builds/M11/Web`, `Builds/M11/Web/Build/Web.wasm` |
| Local Web serve | `http://127.0.0.1:8137/index.html` returned HTTP 200 | local HTTP check from 2026-08-03 |
| Chrome bootstrap/runtime | Playwright loaded the build, found one Unity canvas after 8 seconds, and reported 53 console messages with 0 errors and 0 warnings; inspected screenshot shows the live greybox match | `Docs/QA/Visual/Phase7/playwright-fresh-baseline-20260803.png`, `.playwright-cli/console-2026-08-03T03-18-15-589Z.log` |
| Lava smoke | Fresh APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `17939`; portrait screenshot shows the live match/HUD; process-scoped log has no fatal, SIGSEGV, missing-component or monotonic-tick markers | `Docs/QA/Visual/Phase7/android-lava-fresh-baseline-20260803.png`, `Builds/M11/Logs/fresh-baseline-lava-20260803.txt` |

Known baseline warnings remain separate from pass/fail results: Unity batchmode logs
report the empty `BattleRaja.Gameplay` asmdef, Photon editor custom-dependency scheduling,
no AudioListener in the generated `MovementLab` fixture, and an unavailable batchmode
licensing handshake/access token. None caused a test or build failure. The ADB inventory
also showed an Oppo device, but no Oppo command was used; physical smoke testing used
only the instructed Lava serial.

## Fresh assist/statistics phase (`5845485`)

The pure match-statistics change adds one deterministic assist credit to each living,
non-finishing participant who contributed damage to an eliminated target. Duplicate lethal
events, environmental damage and self-damage do not create additional assists.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 90 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/assist-editmode-20260803.xml` |
| PlayMode tests | 43 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/assist-playmode-20260803.xml` |
| Android development build | Unity log reports `Build Finished, Result: Success`; APK 151,365,343 bytes; SHA-256 `7217CFDE7ACC1F9F61FF8ACA90C95B07DA8CB952E760760EC3F1AC1748CFD4D3` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | Unity log reports success; 21 files, 133,247,876 bytes; WASM SHA-256 `F48A35AE2F9DB985DC28689B31A1BF8C42F5A1CED70531A7F374608F5B7FA3D3` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Browser runtime | Chrome/Playwright found one canvas after an 8-second warm-up; 53 console messages, 0 errors and 0 warnings; inspected screenshot shows the live greybox match | `Docs/QA/Visual/Phase7/playwright-assist-phase-20260803.png`, `.playwright-cli/console-2026-08-03T03-36-22-982Z.log` |
| Lava smoke | Exact assist APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `20176`; process-scoped log contains no fatal, SIGSEGV, missing-component or monotonic-tick marker | `Docs/QA/Visual/Phase7/android-lava-assist-phase-20260803.png`, `Builds/M11/Logs/assist-lava-20260803.txt` |

The Android wrapper printed a stale PowerShell `$LASTEXITCODE` after Unity had already
logged a successful build; the Unity build log and artifact were checked directly. This
wrapper quirk is recorded rather than treated as a product failure.

## Fresh aim-assist accessibility phase (`c23982e` + `d69b74b` test coverage)

The accessibility setting now changes only the local aim direction within a bounded
18-degree, 10-metre cone. Target selection is pure and deterministic; Unity gathers
eligible enemy colliders with a fixed non-allocating buffer, and projectile authority is
unchanged.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 94 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/aimassist-editmode-v5-20260803.xml` |
| PlayMode tests | 44 passed, 0 failed, 0 skipped, including the in-match settings toggle/persistence test | `Builds/M11/TestResults/aimassist-playmode-v2-20260803.xml` |
| Android development build | Unity log reports `Build Finished, Result: Success`; APK 151,378,712 bytes; SHA-256 `94A5C3D933BB00C99BBF293FA832DD6E67E3DB7D37C3BD63581E3BA7878772E8` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | Unity log reports success; 21 files, 133,250,977 bytes; WASM SHA-256 `3B505C77E2F567878CDFAEC34BDE83A23B53AE3DA4943C3FBE474555D536505F` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Browser runtime | Chrome/Playwright found one canvas after an 8-second warm-up; 53 console messages, 0 errors and 0 warnings; inspected screenshot shows the live greybox match | `Docs/QA/Visual/Phase7/playwright-aimassist-phase-20260803.png`, `.playwright-cli/console-2026-08-03T04-33-13-788Z.log` |
| Lava smoke | Exact aim-assist APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `21626`; process-scoped log contains no fatal, SIGSEGV, missing-component or monotonic-tick marker | `Docs/QA/Visual/Phase7/android-lava-aimassist-phase-20260803.png`, `Builds/M11/Logs/aimassist-lava-20260803.txt` |

The functional setting still requires human review for touch ergonomics, fairness and
accessibility approval; this evidence does not close the broader visual gate.

## Results-screen continuation (`810f484`)

The offline Results surface now lists every placement in deterministic placement/ID order,
including eliminations, assists, damage and survival time. Compact portrait mode uses a
shorter row format and smaller results typography; the formatter is pure and does not
mutate the match snapshots.

| Check | Command/result | Evidence |
| --- | --- | --- |
| EditMode tests | 94 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/results-editmode-20260803.xml` |
| PlayMode tests | 45 passed, 0 failed, 0 skipped; includes the placement/statistics formatter regression | `Builds/M11/TestResults/results-playmode-20260803.xml` |
| Android development build | Unity log reports `Build Finished, Result: Success`; APK 151,408,183 bytes; SHA-256 `AF7A862E47BE8F50C885AC3784C01964503AF2B96802CA21319E3E1AEC2D8AE4` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | Unity log reports success; 21 files, 133,352,571 bytes; WASM SHA-256 `70210B58F843B51A29499AA5D8DB3A4D64AAAB0E994FA8425064AEF78762A0C7` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Browser runtime | `http://127.0.0.1:8137/index.html` returned HTTP 200; Chrome/Playwright reached the live match with 53 console messages, 0 errors and 0 warnings | `Docs/QA/Visual/Phase7/playwright-results-20260803.png`, `.playwright-cli/console-2026-08-03T04-59-04-584Z.log` |
| Lava smoke | Exact APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `22304`; capture shows a live portrait match and log scan found no fatal, SIGSEGV, AndroidRuntime, missing-component or null-reference marker | `Docs/QA/Visual/Phase7/android-lava-results-20260803.png`, `Builds/M11/Logs/results-lava-20260803.txt` |

The runtime captures are live-match technical evidence for the build. They do not claim
that the Results/rematch state has passed human visual review; that state still needs a
deliberate capture and owner approval.

## Web input-focus continuation (`3e00b02`)

The Web template now gives the Unity canvas keyboard focus semantics (`tabindex="0"`, an
accessible game label and pointer-down focus restoration). The repository validator checks
these invariants so keyboard/pointer browser QA does not silently run against an unfocusable
canvas.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\Validation\validate.ps1 -RequireUnityProject -UnityExe ...\6000.5.6f1\Editor\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| Fresh regression baseline | EditMode 94/94 and PlayMode 45/45 passed sequentially | `Builds/M11/TestResults/rebaseline-editmode-20260803.xml`, `Builds/M11/TestResults/rebaseline-playmode-20260803.xml` |
| Web build | Unity Web build completed; 21 files, 133,353,130 bytes; WASM SHA-256 `70210B58F843B51A29499AA5D8DB3A4D64AAAB0E994FA8425064AEF78762A0C7` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web/index.html` |
| Chrome focus/runtime | `http://127.0.0.1:8137/index.html` returned HTTP 200; after clicking the canvas, `document.activeElement.id` was `unity-canvas` and `tabIndex` was `0`; 53 console messages, 0 errors and 0 warnings | `Docs/QA/Visual/Phase7/playwright-web-focus-20260803.png`, `.playwright-cli/console-2026-08-03T05-12-58-426Z.log` |
| Edge focus/runtime | After clicking the canvas, `document.activeElement.id` was `unity-canvas` and `tabIndex` was `0`; 53 console messages, 0 errors and 0 warnings | `.playwright-cli/console-2026-08-03T05-13-26-439Z.log` |

This closes the DOM/canvas focus defect only. It does not turn the unverified gadget-use or
Results/rematch screenshot rows into visual approval, and it does not replace manual touch
ergonomics review.

## Fresh Phase 6 continuation (`8544f55`)

This section supersedes the older Phase 2 checkpoint below for the current source
HEAD. It covers the authority-driven spatial gadget collection path, immediate live
results publication after lethal damage, and the rematch scene reload surface.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 89 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/phase6-full-editmode-20260803.xml` |
| PlayMode tests | 43 passed, 0 failed, 0 skipped; includes spatial gadget collection, live results/rematch reload, three repeated rematch cleanup cycles and the full eight-step tutorial walkthrough | `Builds/M11/TestResults/tutorial-full-playmode-20260803.xml` |
| Android development build | Unity exit 0; IL2CPP APK 151,312,094 bytes; SHA-256 `285A7973A0487281F0F79BCFD14827114232D5C6B0F2BA3AD67BC57134B6B6BF` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | Unity exit 0; 21 files, 133,194,614 bytes; WASM 120,501,844 bytes; SHA-256 `E029D2925F6E0B7463DA8F236584EE560F011EE0A59D99884A484E05EDA8EDAB` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Local Web serve | `http://127.0.0.1:8137/index.html` returned HTTP 200 | local HTTP check from 2026-08-03 |
| Browser runtime | Fresh Chrome/Playwright load reached the live match after an 8-second warmup; screenshot shows the arena and HUD; console scan returned 0 errors/0 warnings | `Docs/QA/Visual/Phase7/playwright-phase6-runtime-20260803.png`; Playwright CLI output from 2026-08-03 |
| Lava smoke | Exact Phase 6 APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `14841`; portrait screenshot shows the live match and HUD; process-scoped log contains no fatal crash, monotonic-tick or missing-component marker | `Docs/QA/Visual/Phase7/android-lava-phase6-runtime-20260803.png`, `Builds/M11/Logs/phase6-runtime-lava-process-20260803.txt` |
| Runtime warnings | The process sample still contains the known optional `com.google.android.play.core.assetpacks.AssetPackManager` lookup and Lava `gralloc`/vendor buffer noise; these did not terminate the process | `Builds/M11/Logs/phase6-runtime-lava-process-20260803.txt` |
| Performance smoke snapshot | Lava after ~20 seconds: 507,397 KB PSS, 644,576 KB RSS, 97,884 KB Graphics PSS; Chrome: 120,502,144-byte WASM transfer, 274.8 ms WASM resource duration, 30,464,015-byte used JS heap | `Docs/PERFORMANCE_BUDGET.md`, `Builds/M11/Logs/phase6-lava-gfxinfo-20260803.txt`, `Builds/M11/Logs/phase6-lava-meminfo-20260803.txt` |

The new PlayMode coverage is functional evidence, not visual approval: it relocates an
authored Dhol pickup before restarting the authority-backed scene to exercise spatial
collection deterministically, invokes the generated Results/Rematch buttons after
authoritative lethal damage, repeats three real reload cycles while freezing scene
activation long enough to disable bot-side fixture activity, and walks all eight visible
tutorial prompts before replaying and skipping the overlay. The visual QA gate remains
in progress because no human-facing gadget-use or results/rematch screenshot has been
captured.

## Fresh Phase 2 continuation (`1f59a68`)

This section supersedes the older historical checkpoints below for the current source
HEAD. The fresh artifacts and runtime samples were generated from the Phase 2 live
fighter-controller coverage commit.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 89 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/phase2-full-editmode-20260803.xml` |
| PlayMode tests | 39 passed, 0 failed, 0 skipped; includes live Pehel and Maya ability tests | `Builds/M11/TestResults/phase2-full-playmode-20260803.xml` |
| Android development build | Unity exit 0; IL2CPP APK 151,305,385 bytes; SHA-256 `42BCF8B076AACC8C099462BB0DE3028183A84E2D7959DBE5A1BF77BC2C57CEDB` | `Builds/M11/Logs/android-build.log` |
| Web development build | Unity exit 0; 21 files, 133,194,254 bytes; WASM 120,501,562 bytes; SHA-256 `B8CED1C03AE4F9D6BCF1B601A3B333DD149C9E5228A97BAB7171E5A17D7EA805` | `Builds/M11/Logs/web-build.log` |
| Local Web serve | `http://127.0.0.1:8137/index.html` returned HTTP 200 | local HTTP check from 2026-08-03 |
| Browser runtime | Fresh Chrome/Playwright load reached the live match after an 8-second warmup; inspected capture has no blank canvas and console scan returned 0 errors/0 warnings | `Docs/QA/Visual/Phase7/playwright-phase2-runtime-20260803.png`; Playwright CLI output from 2026-08-03 |
| Photon/package check | Unity `6000.5.6f1`; Fusion build `2.1.1 Stable 2177`; Input System `1.20.0`; URP `17.5.0`; Test Framework `1.7.0` | `Assets/Photon/Fusion/build_info.txt`, `Packages/manifest.json` |
| Android device inventory | `adb devices -l` showed Lava `ST5GDW23LB004392` and Oppo `b60e53b3`; only the Lava serial was used | ADB output from 2026-08-03 |
| Lava smoke result | Exact Phase 2 APK installed/launched only on Lava `ST5GDW23LB004392`; process `13173` remained alive; no fatal Unity/Android exception, missing-component marker or monotonic-tick error in the captured process sample | `Docs/QA/Visual/Phase7/android-lava-phase2-runtime-20260803.png`, `Builds/M11/Logs/phase2-runtime-lava-logcat-20260803.txt` |

The Phase 2 controller tests also removed invalid ScriptableObject component lookups
from fighter-definition fallback paths. Pehel's charge cast now ignores fighter
colliders while retaining static-geometry blocking, so an opposing target can be
captured instead of prematurely ending the charge.

The prior `46a3d1e` APK exposed a real multi-step render-frame tick defect; the
pre-fix evidence is retained at `Docs/QA/Visual/Phase7/android-lava-pre-fix-tick-error-46a3d1e.png`.
The earlier `a245f24` baseline records the corrected fixed-tick retest below.

## Historical checkpoints retained from earlier runs

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

## Phase 1 gadget-authority continuation (`dd35f53`)

Dhol Burst now evaluates living targets from the application snapshot and returns
per-target displacement intents. `GadgetUser` applies those intents to Unity actors; its
actor-scan path is retained only for an isolated non-authoritative lab.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | 0 errors, 0 warnings | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` |
| EditMode suite | 84 passed, 0 failed | `Builds/M11/TestResults/phase1-dhol-authority-editmode-20260803.xml` |
| PlayMode suite | 34 passed, 0 failed | `Builds/M11/TestResults/phase1-dhol-authority-playmode-20260803.xml` |
| Dhol authority rule | Dhol use emits one deterministic target displacement for the in-radius participant; duplicate command remains rejected | `Builds/M11/TestResults/phase1-dhol-authority-editmode-20260803.xml` |

No new Android/Web artifact was built for this focused authority-only change; the
`1437e5c` artifacts remain the latest cross-platform smoke builds. Tiffin station
damage forwarding and Umbrella mitigation remain presentation-owned.

## Phase 1 Tiffin-authority continuation (`39149f1`)

Tiffin station healing cadence and lifetime now run in the pure authority runtime.
Authority ticks emit target healing intents and station-expiry IDs; authority-driven
Unity station objects render the result without running a duplicate `Time.deltaTime`
healing loop. Local non-authoritative lab stations retain their fallback behavior.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | 0 errors, 0 warnings | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` |
| EditMode suite | 85 passed, 0 failed | `Builds/M11/TestResults/phase1-tiffin-authority-editmode-20260803.xml` |
| PlayMode suite | 34 passed, 0 failed | `Builds/M11/TestResults/phase1-tiffin-authority-playmode-20260803.xml` |
| Tiffin authority rule | Pure test observes healing intent at fixed cadence and expiry by the configured lifetime | `Builds/M11/TestResults/phase1-tiffin-authority-editmode-20260803.xml` |

No new Android/Web artifact was built for this focused authority-only change; the
`1437e5c` artifacts remain the latest cross-platform smoke builds. Station damage
forwarding and Umbrella mitigation remain open.

## Phase 1 gadget-authority candidate (`d328132`)

Umbrella Guard duration and front-facing mitigation now run through an authority-owned
runtime before damage reaches the presentation pipeline. Together with the Dhol and
Tiffin continuations above, all three gadget definitions have fixed-tick application
state for their currently implemented effects.

| Check | Command/result | Evidence |
| --- | --- | --- |
| EditMode suite | 86 passed, 0 failed | `Builds/M11/TestResults/phase1-umbrella-authority-editmode-v4-20260803.xml` |
| PlayMode suite | 34 passed, 0 failed | `Builds/M11/TestResults/phase1-umbrella-authority-playmode-v2-20260803.xml` |
| Android development smoke build | Unity exit 0; APK 151,286,644 bytes; SHA-256 `B50F74E56A3F3FA95A164834223739B3A0A2F1681DBA3D12F7B82B956F682F9B` | `Builds/M11/Logs/phase1-gadget-authority-android-20260803.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development smoke build | Unity exit 0; 21 files, 133,177,869 bytes; `Build/Web.wasm` 120,488,383 bytes; SHA-256 `6538B2ECF83940FEB7F9AE7F1B160114557E04F2E49F847048AD9795D711EB9E` | `Builds/M11/Logs/phase1-gadget-authority-web-20260803.log`, `Builds/M11/Web` |
| Local Web serve | `curl -I http://127.0.0.1:8136/index.html` returned HTTP 200 | local server check from 2026-08-03 |
| Build warning | Web build succeeded with a non-fatal websockify `EADDRINUSE` shutdown message on helper port 35020 | `Builds/M11/Logs/phase1-gadget-authority-web-20260803.log` |
| Lava physical smoke | Not run: `adb devices -l` listed only Oppo `b60e53b3`; the instructed Lava serial `ST5GDW23LB004392` was absent | device-gate blocker |

The fresh artifacts are development builds only. No production signing, store upload,
real Photon room, or final visual/performance claim is made.

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

## Phase 1 Tiffin station-authority continuation (`a9c93fc`)

Tiffin station damage now enters the same application-owned runtime as healing and
lifetime. The resolver validates the target request, forwards damage to the authority,
applies only the authoritative amount to the Unity view, and expires the view when the
authority reports destruction.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | 0 errors, 0 warnings | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` |
| EditMode suite | 87 passed, 0 failed | `Builds/M11/TestResults/phase1-station-authority-editmode-final-20260803.xml` |
| PlayMode suite | 35 passed, 0 failed | `Builds/M11/TestResults/phase1-station-authority-playmode-final-20260803.xml` |
| Authority station rule | Pure test covers partial damage, capped destruction and removal; PlayMode resolves damage through the live match authority | `AuthorityFoundationTests.AuthorityOwnsTiffinStationDamageAndRemovesDestroyedStations`, `GadgetPlayModeTests.TiffinStationDamageIsAcceptedThroughMatchAuthority` |
| Android development smoke build | Unity build success; APK 151,286,056 bytes; SHA-256 `9D6EFD324FE0C83427868AE59136FF276F51A608F5C78A43A5B7A5AE3D1482E5` | `Builds/M11/Logs/phase1-station-authority-android-20260803.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development smoke build | Unity build success; 21 files, 133,182,055 bytes; `Build/Web.wasm` 120,491,763 bytes; SHA-256 `B66C0F97A06FB2E60384D9B59D5C3323BBA53E8B3344FC8BF6657233ED1D8AD7` | `Builds/M11/Logs/phase1-station-authority-web-20260803.log`, `Builds/M11/Web` |
| Local Web serve | `curl -I http://127.0.0.1:8136/index.html` returned HTTP 200 | local server check from 2026-08-03 |
| Web build warning | Build succeeded; non-fatal Unity WebGL script-debugging warning and websockify helper shutdown message remain | `Builds/M11/Logs/phase1-station-authority-web-20260803.log` |
| Lava physical smoke | Not run: `adb devices -l` still did not list `ST5GDW23LB004392`; the connected Oppo device was intentionally not used | device-gate blocker |

This is authority/test/build evidence only. It does not establish final visual quality,
physical-device validation, performance, real Photon multiplayer or PlayFab integration.

## Compact portrait HUD continuation (`913988a`)

- Repository validation: `Tools\\Validation\\validate.ps1 -RequireUnityProject
  -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` returned 0 errors and 0 warnings.
- EditMode: 87 passed, 0 failed, 0 skipped
  (`Builds/M11/TestResults/hud-compact-editmode-20260803.xml`).
- PlayMode: 37 passed, 0 failed, 0 skipped
  (`Builds/M11/TestResults/hud-compact-playmode-20260803.xml`). The added
  `CompactMatchStatusKeepsZoneTelemetryReadable` regression test covers the compact
  two-line format and warning text.
- Android: fresh development IL2CPP build succeeded under Unity 6000.5.6f1
  (`Builds/M11/Logs/hud-compact-android-20260803.log`).
  `Builds/M11/Android/BattleRaja-M11.apk` is 151,304,957 bytes; SHA-256
  `A417601C7D4FDCFD5B6D18EBAC88AC28486496740881518A285EF93983FD48C4`. This exact
  APK was not installed; the Lava-only physical-device gate remains open because
  serial `ST5GDW23LB004392` was not connected.
- Web: fresh development build succeeded with the responsive template
  (`Builds/M11/Logs/hud-responsive-web-20260803.log`). The output contains 21 files
  totaling 133,185,154 bytes; `Build/Web.wasm` is 120,493,880 bytes with SHA-256
  `FB8380DC4FCD5B8674D03EF99CF25BBF65B2C280EBB98F3638C05159C248EF09`.
- Browser smoke: local HTTP `http://127.0.0.1:8137/index.html` returned 200. A fresh
  Playwright 390×844 capture is stored at
  `Docs/QA/Visual/Phase7/playwright-390x844-hud-compact.png`; phase/alive/zone
  telemetry is visibly separated from the fighter/gadget HUD. This is a visual
  readability observation, not final visual approval.
- Boundary: gadget success, loading/results surfaces, touch ergonomics, physical Lava
  validation, performance, real Photon and PlayFab remain open or externally blocked.

## Responsive Web framing continuation (`67a4d40`)

- Repository validation: `BattleRaja.Editor.BuildEntrypoints.ValidateProject` exited 0
  with the expected non-fatal Unity licensing/import messages; no project validation
  errors or warnings were reported.
- EditMode: 87 passed, 0 failed, 0 skipped
  (`Builds/M11/TestResults/phase7-responsive-editmode-20260803.xml`).
- PlayMode: 36 passed, 0 failed, 0 skipped
  (`Builds/M11/TestResults/phase7-responsive-playmode-20260803.xml`). The added
  framing assertion covers unchanged 16:9 size and expanded narrow-viewport framing.
- Android: fresh development IL2CPP build succeeded under Unity 6000.5.6f1
  (`Builds/M11/Logs/phase7-responsive-android-20260803.log`). `Builds/M11/Android/
  BattleRaja-M11.apk` is 151,288,356 bytes; SHA-256
  `1F9A7E6DAF65AE963E167FE8D055AE8C94757C4A2E9EE1ECE8B2BCDCA24BEA9F`. This exact
  APK was not installed; the Lava-only physical-device gate remains open because
  serial `ST5GDW23LB004392` was not connected.
- Web: fresh development build succeeded with the project-owned responsive template
  (`Builds/M11/Logs/phase7-responsive-web-v2-20260803.log`). The output contains 21
  files totaling 133,184,187 bytes; `Build/Web.wasm` is 120,493,105 bytes with
  SHA-256 `027599E8E1A201157C86305DA775A8F500048E89E9318FA002DE812E68186E74`.
- Browser smoke: local HTTP `http://127.0.0.1:8137/index.html` returned 200. A fresh
  Playwright run at 390×844 captured `Docs/QA/Visual/Phase7/
  playwright-390x844-responsive-gameplay.png`; the responsive host and camera framing
  fill the portrait viewport without the earlier fixed 960×600 horizontal crop. The
  capture still shows prototype-density HUD and is not mobile-Web or final visual
  approval.
- Production-flow Web recheck: `BuildWebBazaarBastionDevelopment` also succeeded in
  `Builds/M11/Web-BazaarBastion` (19 files, 133,179,836 bytes; the same main WASM
  hash as above). Local HTTP `http://127.0.0.1:8138/index.html` returned 200. The
  fresh route reached Bootstrap → Tutorial Arena at 390×844 and captured
  `Docs/QA/Visual/Phase7/playwright-390x844-responsive-tutorial.png`; the tutorial
  card and controls remained inside the portrait viewport. The browser emitted only
  Unity's known non-fatal `JS_FileSystem_Sync()` deprecation warning.
- Boundary: this is responsive framing and build evidence only. Gadget success,
  loading/results surfaces, touch ergonomics, physical Lava validation, performance,
  real Photon and PlayFab remain open or externally blocked. Build-generated changes
  to `Bootstrap.unity` and `TutorialArena.unity` were restored because those files
  were clean before the baseline.

## Fixed-tick runtime correction (`a245f24`)

The Lava baseline exposed a real multi-step render-frame defect: the fixed clock
advanced several ticks, while every presentation consumer reused the final tick for
each consumed step. `OfflineMatchAuthority.Advance` therefore rejected repeated tick
identities on the device. The correction adds `FixedSimulationClock.GetConsumedTick`
and updates the authority controller, movement, attacks, projectiles, bots, gadgets and
Bijli/Pehel/Maya ability adapters to pass the per-step tick.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | 0 errors, 0 warnings | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` |
| EditMode regression | 89 passed, 0 failed, 0 skipped; includes sequential multi-step tick and authority acceptance tests | `Builds/M11/TestResults/fixed-tick-runtime-editmode-20260803.xml` |
| PlayMode regression | 37 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fixed-tick-runtime-playmode-20260803.xml` |
| Android retest | Unity exit 0; APK 151,318,469 bytes; SHA-256 `154F0F81FA6F068302FC57A6513A6C38CB4A7A8298C856264A04B14EE796574F` | `Builds/M11/Logs/fixed-tick-runtime-android-20260803.log` |
| Lava retest | Exact fixed-tick APK installed/launched on `ST5GDW23LB004392`; focused Unity activity remained alive; no current Unity-process monotonic-tick exception, fatal exception or SIGSEGV; inspected match capture is clean of the prior red Unity console error | `Docs/QA/Visual/Phase7/android-lava-fixed-tick-46a3d1e.png`, `Builds/M11/Logs/fixed-tick-runtime-lava-logcat-20260803.txt` |
| Web retest | Unity exit 0; 21 files, 133,187,266 bytes; WASM 120,495,480 bytes; SHA-256 `F02758353612C68904777C2F25F4F56E22DA4225F0F426A205792BE41E111EC6` | `Builds/M11/Logs/fixed-tick-runtime-web-20260803.log` |
| Browser retest | HTTP 200; fresh Playwright render reached live match; inspected screenshot has no blank canvas and console error/warning scan returned 0 | `Docs/QA/Visual/Phase7/playwright-1000x1000-fixed-tick-46a3d1e.png` |

Known non-blocking device log noise remains: Android/Unity reports absent optional
Google Play Asset Pack classes and Lava gralloc format warnings. These are separate
from the fixed-tick exception and did not stop the current Unity process. This phase
does not establish final visual quality, performance, real Photon multiplayer or
PlayFab integration.
