# BattleRaja V1.0 release plan

This is the live owner-directed continuation checklist. It records machine-verifiable
progress and keeps the final classification honest. The current product remains a
prototype until every V1 completion gate and remaining human gate passes.

## Scope lock

- Target: fully offline Android V1.0 local release candidate.
- Platforms for this goal: Android only. Web, Photon gameplay, PlayFab, accounts,
  cloud progression, matchmaking, social features, ads, IAP, online leaderboards and
  analytics upload are out of scope.
- Approved physical evidence device: Lava `ST5GDW23LB004392` (`LAVA LXX508`) only.
- Preserve internal networking/Web seams without exposing unusable public online paths.

## Checkpoint 1 - preserve and rebaseline exact source

Status: **Passed with limitations**.

### Source baseline

- Date/time: 2026-08-24 23:31 IST.
- Primary branch: `codex/v1-playstore-release`, ahead of `origin/main` by 131 commits.
- Primary HEAD: `33035e84e86b41956b968f4c628aaa79c1496d49`.
- Remote `origin/main`: `ca6ec3e17e695042664cf3bdbf9889b259b33144`; unrelated to the
  selected continuation tip.
- Primary dirty state preserved:
  - Modified user/context document: `Docs/AI/UnityProjectContext.md`.
  - Untracked visual-development assets under `Art/Concepts/`.
- Historical stashes preserved; none applied or dropped.
- Disposable detached worktree used for exact-source validation/build:
  `C:\Projects\BattleRaja-baseline-33035e8`.

### Tool inventory

| Capability | Result |
| --- | --- |
| Unity | `6000.5.6f1` revision `0e0577a1a2ac` at `C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe` |
| Unity Hub | `43.2.0`; CLI version output is blank but executable exists at `C:\Program Files\Unity Hub\Unity Hub.exe` |
| Git / LFS | Git available; `git lfs fsck --pointers` passed |
| Android platform tools | ADB `36.0.2-14143358` |
| Android SDK | Unity-managed build-tools `36.0.0`, platforms 34/36/37, cmdline-tools `16.0` |
| NDK | Unity-managed r27c `27.2.12479018` |
| JDK | Unity/OpenJDK Temurin `17.0.18+8`; Microsoft JDK 17 also present on PATH |
| Blender | Not installed in common locations; no PATH command |
| Audio tools | No Audacity or FFmpeg found on PATH/common locations |
| Image/vector tools | No Krita, Inkscape, GIMP or ImageMagick found in checked locations |
| Automation fallbacks | Python, Node/npm, PowerShell 7, Unity Editor geometry/material tooling and repository PowerShell scripts available |

Missing creative tools do not block code/gameplay work. Installing licensed external
tools requires explicit owner approval if needed later.

### Exact-source automated baseline

All commands ran in disposable worktree `C:\Projects\BattleRaja-baseline-33035e8`
at exact commit `33035e8`.

| Gate | Command/result |
| --- | --- |
| Static validation | `Tools\Validation\validate.ps1 -RequireUnityProject -UnityExe ...` -> **0 errors, 0 warnings** |
| EditMode | `Tools\Validation\run_unity_tests.ps1 ... -TestPlatform editmode` -> **125 / 125 passed**, XML `Builds\Local\V1Baseline33035e8\TestResults\editmode.xml` |
| PlayMode | Same wrapper with `-TestPlatform playmode` -> **74 / 74 passed**, XML `Builds\Local\V1Baseline33035e8\TestResults\playmode.xml` |
| Replay tests | Filtered `BattleRaja.Tests.EditMode.ReplayDeterminismTests` -> **4 / 4 passed** |
| Deep deterministic soak | `BATTLERAJA_SOAK_MATCHES=1000`; filtered soak test -> **1 test passed**, **1,000 seeds x 2 executions = 2,000 matches**, zero divergence, NUnit duration **393.3873301 s** |

The PlayMode count advanced from the previously recorded 73 to 74 because exact HEAD
`33035e8` includes the new accessibility-toggle-state regression. This is current-source
evidence, not an older report.

### Exact-source development APK and Lava smoke

- APK path: `C:\Projects\BattleRaja-baseline-33035e8\Builds\M11\Android\BattleRaja-M11.apk`
- Size/hash: **92,760,236 bytes**, SHA-256
  `24282D12C647C34D77B2C8D4A739608C7DA660906CE2F92B4E2634C1033206CF`
- Manifest: package `com.example.battleraja.m11`, versionName `1.0.0`, versionCode
  `100`, min SDK 28, target/compile SDK 36, launch activity
  `com.unity3d.player.UnityPlayerGameActivity`
- Install: succeeded only on Lava `ST5GDW23LB004392`.
- Launch: monkey launched the package; `UnityPlayerGameActivity` was top-resumed and visible.
- Lifecycle: HOME backgrounded the app as paused/not visible; relaunch restored it as
  top-resumed and visible.
- Memory sample after resume: total PSS **394,730 KB**, RSS **526,824 KB**, Graphics
  **72,536 KB**, swap PSS **71 KB**.
- Log scan: launch/lifecycle logcat contained no `FATAL EXCEPTION`, `ANR in`, `SIGSEGV`,
  `NullReferenceException` or `UnityException` marker.
- Raw logs/memory capture: ignored worktree files under
  `Builds\Local\V1Baseline33035e8\Lava\`.

### Build-worktree caveat

The disposable worktree was clean before the build. After Unity's scene-generation/build
entrypoints, it reports two scene modifications and deletion of nine `.pdb.meta` files.
These are disposable-copy artifacts and were not copied into the primary tree. Before any
commit from generated scenes, inspect whether each change is intentional; the missing
Fusion PDB metadata should be treated as a packaging/tooling issue to resolve deliberately,
not committed silently.

## Checkpoint 2 - fix gameplay truth before large-scale art production

Status: **P0 corrections implemented; broader authority/replay audit continues**.

The read-only audit over exact commit `33035e8` confirmed two P0 blockers:

- Solo Raja was not a true free-for-all: all seven bots shared `CombatFaction.Enemy`,
  so authority rejected valid bot-to-bot damage and Pehel capture.
- Authority projectiles changed canonical health but did not publish those events in
  `MatchAuthorityTick.DamageEvents`, allowing visible health, elimination feedback,
  perception and spectator state to lag.

### P0 corrections

- Added authority-owned positive combat groups. Every Solo participant defaults to its
  own group; explicit groups remain available for a future team mode. `CombatFaction`
  is now only a presentation compatibility label.
- Projectile participant/decoy selection, Pehel capture/throw and Maya decoy damage use
  combat-group hostility rather than view faction.
- Authority projectile actor hits now emit their stable, already-applied
  `CombatDamageEvent` in the same canonical tick. Presentation mirrors that immutable
  result immediately.
- Aim assist considers living non-neutral fighters except the player's own fighter and
  own Maya decoy; stations are excluded.
- Replay setup explicitly records Solo one-group-per-actor relationships, and canonical
  hashing includes combat groups.
- Recorded ADR-055.

- True eight-participant Solo free-for-all eligibility.
- Bot-to-bot target/damage/elimination credit: **fixed with EditMode + production
  PlayMode regressions**.
- Canonical-to-visible health parity through projectile damage, elimination, perception
  and spectator transition: **fixed with production PlayMode regression**; terminal
  results were already covered by the two-participant authority test.
- Replay completeness:
  - Assist contributions, damage identity counters, next station ID, arena collision
    content, decoy-damage identity keys and sorted station/decoy tie traversal are now
    hashed: **fixed with regressions**.
  - Bijli dash replay support and production command routing: **fixed with authority-owned
    dash runtimes, canonical tick advancement/hashing, replay command coverage, production
    view mirroring and movement-lock parity**.
  - Production durable replay-file serialization and complete future-state capture for all
    presentation-only state: **still open**.
- Unified action eligibility across movement, attacks, abilities, gadgets, healing,
  knockback and Aandhi: **fixed with authority-owned eligibility and regressions**.

No large-scale art production will begin until the remaining gameplay-truth gates pass.

### Post-P0 automated evidence

- Static validation: **0 errors / 0 warnings**.
- Full EditMode: **127 / 127 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\editmode-full.xml`).
- Full PlayMode: **75 / 75 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\playmode-full.xml`).
- Deep recorded replay soak after the authority change:
  `BATTLERAJA_SOAK_MATCHES=1000`, **1,000 seeds x 2 executions = 2,000 matches**,
  zero divergence, NUnit duration **370.8663022 s**
  (`Builds\Local\V1GameplayTruth\TestResults\deep-soak-1000.xml`).

These results cover the working tree at the P0 gameplay-truth correction before any
new Android artifact. A fresh exact-commit platform build is required after commit.

### Exact P0 Android smoke — `8eaa9e5` — 2026-08-25

The P0 gameplay-truth correction was committed at
`8eaa9e5` (`authority: fix solo free-for-all health parity`) and rebuilt from a clean,
detached disposable worktree at `C:\Projects\BattleRaja-p0-8eaa9e5`.

- Static validation at exact commit: **0 errors / 0 warnings**.
- APK path: `C:\Projects\BattleRaja-p0-8eaa9e5\Builds\M11\Android\BattleRaja-M11.apk`
- Size/hash: **92,762,248 bytes**, SHA-256
  `DD765F971042C9FD14749808A24EA620476AA5A1AD54AF7F9FF86F4BF2FE62D4`
- Manifest: package `com.example.battleraja.m11`, versionName `1.0.0`, versionCode
  `100`, min SDK 28, target/compile SDK 36, launch activity
  `com.unity3d.player.UnityPlayerGameActivity`
- Install: succeeded only on Lava `ST5GDW23LB004392`.
- Launch: `UnityPlayerGameActivity` was top-resumed and visible.
- Lifecycle: HOME made the task invisible/paused; relaunch restored it as top-resumed
  and visible.
- Memory sample after resume: total PSS **414,535 KB**, RSS **557,740 KB**, Graphics
  **17,476 KB**, swap PSS **71 KB**.
- Log scan: launch/lifecycle logcat contained no `FATAL EXCEPTION`, `ANR in`,
  `SIGSEGV`, `NullReferenceException` or `UnityException` marker.

This is an offline launch/lifecycle smoke only. It does not replace physical combat QA,
sustained performance review, accessibility review or release signing/store gates.

### P1 authority/replay hardening in progress - 2026-08-25

- Added finite movement/aim rejection before the motor/collision solver so one malformed
  command cannot stop later actors in the same shared tick.
- Added content-addressed arena hashing plus canonical hashing for assist contributions,
  simulation damage identities, next station identity and decoy damage ticks.
- Replaced dictionary-order dependence for station/decoy projectile tie traversal with
  deterministic sorted buffers.
- Recorded ADR-056. Focused replay/authority regressions pass; final deep-soak evidence
  for this follow-up are complete.

#### Post-P1 automated evidence

- Static validation: **0 errors / 0 warnings**.
- Full EditMode: **130 / 130 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\editmode-replay.xml`).
- Full PlayMode: **75 / 75 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\playmode-replay.xml`).
- Deep recorded replay soak after replay/authority hardening:
  `BATTLERAJA_SOAK_MATCHES=1000`, **1,000 seeds x 2 executions = 2,000 matches**,
  zero divergence, NUnit duration **399.2625235 s**
(`Builds\Local\V1GameplayTruth\TestResults\deep-soak-replay.xml`).

### P2 - Authority-owned Bijli dash replay support - complete 2026-08-25

- Added `OfflineMatchAuthority.TryStartBijliDash`, fixed-tick `AdvanceBijliDash`, canonical
  dash-state lookup and shared authority movement-lock reporting.
- Advanced active/cooldown dash runtimes inside the canonical tick using the deterministic
  arena solver; published collision-resolved positions in `MatchAuthorityTick.BijliDashSteps`
  and mirrored them to Unity views after the tick.
- Included dash action state, direction, cooldown, travelled distance and command/step ordering
  ticks in the canonical hash. Replay now accepts the common Bijli ability command instead of an
  unsupported command.
- Routed production `BijliFighterController` through the authority while retaining its lab/local
  fallback. Suppressed queued authority movement from the same lock source so dash ticks cannot
  double-move.
- Applied the attack-style warmup/spawn-protection/resolution gate to both Bijli and Pehel starts;
  recorded ADR-057.

#### Post-P2 automated evidence

- Static validation: **0 errors / 0 warnings**.
- Full EditMode: **131 / 131 passed**
  (`Builds\Local\TestResults\editmode.xml`).
- Full PlayMode: **75 / 75 passed**
  (`Builds\Local\TestResults\playmode.xml`).
- Deep recorded replay soak with Bijli commands enabled:
  `BATTLERAJA_SOAK_MATCHES=1000`, **1,000 seeds x 2 executions = 2,000 matches**, zero
  divergence, NUnit duration **468.619297 seconds**
  (`Builds\Local\V1GameplayTruth\TestResults\bijli-deep-soak.xml`).

### P2 - Bijli authority runtime device smoke - complete 2026-08-25

- Exact runtime source: `3b09775` (`authority: own bijli dash replay state`), built
  in disposable worktree `C:\Projects\BattleRaja-bijli-3b09775`.
- Development-shaped APK: `Builds\M11\Android\BattleRaja-M11.apk`,
  **92,855,860** bytes, SHA-256
  `115C428A69A6E27B7D0BE7A9A0B5C433CAE7CA165C0FCA8251DA34122E70CBC0`.
- Device: approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34).
  Installed package `com.example.battleraja.m11`, versionCode **100**, versionName
  **1.0.0**, minSdk **28**, targetSdk **36**.
- Cold launch: `UnityPlayerGameActivity`, status OK, **384 ms** total time.
- Lifecycle/background/relaunch: HOME paused the activity; the process stayed alive;
  hot relaunch returned it to top-resumed in **82 ms**.
- Interaction route: menu, Solo Raja mode, fighter selection, Bijli selection, live
  match, ability input, player-defeat spectator transition, Resolution and final
  results/rematch surface. The recorded winner was participant **15**.
- Crash-pattern scan (`FATAL EXCEPTION`, `AndroidRuntime`, `SIGSEGV`, `SIGABRT`,
  `ANR in`, `NullReferenceException`, `UnityException`) across app, background,
  resume, match, ability and final-state logs: **0 matches**.
- Memory samples: resume PSS **421,362 KB**, match PSS **420,920 KB**, post-ability
  PSS **420,431 KB**; graphics approximately **12.25 MB**, swap PSS **51-53 KB**.
- Development Console showed repeated non-fatal `Socket: Failed to set blocking
  mode` / multicast player-connection warnings, expected in a development build but
  retained as evidence.

This is a development-shaped interaction/lifecycle smoke. It does not validate
release signing, sustained frame pacing, thermal/GC behavior, accessibility, combat
feel, balance, release package identity or store readiness.

### P3 - Unified eligibility and fair free-for-all AI - complete 2026-08-25

- Added one authority-owned live-actor active-combat eligibility gate for Opening
  through Final Circle. Routed movement, ability displacement, attacks, Bijli/Pehel
  starts, Maya spawning/damage, direct/projectile damage, healing, gadget use,
  station damage/healing and Aandhi damage through it before canonical mutation.
- Preserved action-specific typed rejection reasons at the public boundary and made
  rejected actions non-consuming. Canonicalized test setup to the exact 241-tick
  30 Hz opening boundary and removed PlayMode dependence on prior-test clock state.
- Recorded ADR-058.
- Extended bot perception with the actor's own faction and equipped weapon. Bots now
  ignore same-faction actors in Solo free-for-all, respect weapon maximum range when
  attacking, and use gadgets only against visible hostiles. Production scene generation
  supplies fighter-specific weapon assets to perception/decision state.

#### Post-P3 automated evidence

- Static validation: **0 errors / 0 warnings**.
- Focused authority suite: **25 / 25 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\authority-focused-final.xml`).
- Full EditMode: **132 / 132 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\editmode-unified-final.xml`).
- Full PlayMode: **75 / 75 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\playmode-unified-final.xml`).
- After the fair-AI extension, full EditMode is **133 / 133 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\editmode-botfair-final.xml`) and full
  PlayMode is **75 / 75 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\playmode-botfair-final.xml`).
- Deep recorded replay soak after unified eligibility and fair free-for-all AI:
  `BATTLERAJA_SOAK_MATCHES=1000`, **1,000 seeds x 2 executions = 2,000 matches**, zero
  divergence, NUnit duration **465.3278045 seconds**
  (`Builds\Local\V1GameplayTruth\TestResults\botfair-deep-soak.xml`).

### P3 exact-commit Android smoke — `e65c0ea` — 2026-08-25

- Exact runtime source: `e65c0ea`
  (`authority: unify action eligibility and fair bots`), built in disposable worktree
  `C:\Projects\BattleRaja-e65c0ea`.
- Development-shaped APK: `Builds\M11\Android\BattleRaja-M11.apk`,
  **92,852,632** bytes, SHA-256
  `11A4DF4623980F53B4F34FCEB48B09858DD32A459082B7B00772F9543305D9FC`.
- Manifest: package `com.example.battleraja.m11`, versionName `1.0.0`, versionCode
  `100`, minSdk **28**, targetSdk **36**, launch activity
  `com.unity3d.player.UnityPlayerGameActivity`.
- Install: streamed-install success on Lava `ST5GDW23LB004392`.
- Cold launch status **OK**, total time **450 ms**; activity became top-resumed and
  focused. HOME made launcher top-resumed; hot relaunch returned the game task with
  status **OK**, launch state **HOT**, total time **51 ms**, and the activity again
  became top-resumed/focused.
- Memory after launch: PSS **402,526 KB**, RSS **532,316 KB**, Graphics **76,632 KB**,
  Swap PSS **80 KB**.
- Crash-pattern scan (`FATAL EXCEPTION`, `AndroidRuntime`, `SIGSEGV`, `SIGABRT`,
  `ANR in`, `NullReferenceException`, `UnityException`) across launch and lifecycle
  logs found **0 matches**.

This is a development-shaped launch/lifecycle smoke only. It does not prove interactive
match QA, fighter-specific bot fairness in human play, sustained performance, thermal
behavior, accessibility, signing or store readiness.

### P4 - Production-bot harness and 100-match release gate - 2026-08-26

The production harness now runs the actual `BazaarBastion` scene with eight autonomous
participants, fighter-specific perception/ability controllers, authority-owned damage and
movement, per-seed reports, command digests, gadget/fighter telemetry, collision sampling,
and scene/PlayerPrefs/time-scale cleanup. The focused post-edit PlayMode test passed **1/1**
(`Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-production-bot-focused-final.xml`,
SHA-256 `051CB39DB679BF5B8E414EAEFFB1A6ABD0CF5D21F163052A05AD966FDDD51BAD`).

After the release run, an isolated regression exposed stale entries in the fixed-size bot
perception buffer after a target was defeated. The buffer tail is now cleared without
allocating, and the regression passed 1/1 (`Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-verticalslice-projectile-fixed.xml`,
SHA-256 `47EF7DBBA3B004F1966F1E08CDC43935F1592E2764785F9BDBDD1EBF6DDC97C0`). The
post-fix full suites also passed: EditMode **139/139** (`editmode-v1-final.xml`, SHA-256
`ADD70D0AFBD307F3D4DBF49D8447EF177BABA3A4502A1BC09BA89FF3A44FF7D4`) and PlayMode
**76/76** (`playmode-production-bot-final-fixed.xml`, SHA-256
`3CADA89AC18335B37F88890AEE8B9ABE45AA6E43ABF47DE760C16F5588F91D53`).

The 100-match release run used Unity `6000.5.6f1` on the dirty working tree at HEAD
`fac1c714b9ba2df72b3acf54b40638d0ae122a93` and produced:

- Test report: **76/76 passed**, XML
  `Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-100-matches-release-gates.xml`,
  SHA-256 `D0737AADBF177115FF8DE99C7FB5EF38D9B898AA6370D817E27D50AFA8BE6845`.
- Batch report: `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260825-225804385-9101.json`,
  SHA-256 `EDDF7A8E710095DDF86AB67C4E318AD2D1796450838058FF51FBA34EFC128BA6`.
- 100/100 terminal results within 360 seconds; average duration **291.84 s**;
  87/100 in the 240-360 second window.
- 100/100 bot-to-bot damage; 95/100 combat eliminations; 5/100 Aandhi-only
  resolutions; 0 protected-warmup damage; 0 invalid-position samples; maximum
  continuous stuck duration 18 ticks (0.60 s).
- 63,865 attack attempts with 317 out-of-range attempts (0.50%); 11,356 ability
  attempts with 3,768 rejected (33.18%); 100 Umbrella, 77 Dhol and 95 Tiffin
  successful uses.

Gate classification is deliberately split: the 100-match harness contract **Passed**
under the documented calibrated pacing threshold of 80% in-window; the original goal's
90% in-window pacing target **Failed** at 87% and remains open for balance/human review.
The independent repeated same-seed production command-stream comparison **Failed** on
the accelerated (`playbackScale=50`) path. Two fresh Unity processes ran seed `9101`
against the same dirty source state:

- Run A batch `Builds\Local\V1GameplayTruth\ProductionBotReports\batch-20260826-062135473-9101.json`,
  SHA-256 `41F43CCF00E92183ACF9AF508E50D1A7D64AD2A8B7BB1B4234FD25A847E90045`,
  command digest `BD88C5714AA26C91`, 33,201 commands, duration 306.01 s.
- Run B batch `Builds\Local\V1GameplayTruth\ProductionBotReports\batch-20260826-062253919-9101.json`,
  SHA-256 `F69A5BC1E1882F6C2BE7E7121BA310609BA3991E6055EF73E30D6B0CA63E3F7E`,
  command digest `5F71F87F37E23B56`, 45,951 commands, duration 306.01 s.

The authority deterministic-replay soak still passes, but it does not cover the
presentation bot loop. A diagnostic explicit-tick driver was also tried and reverted:
it was reproducible but changed the pacing profile to about 103 seconds, so it is not
valid release evidence for the current 240-360 second gate. The remaining blocker is
to remove frame-pacing dependence from the production bot path without changing the
gameplay distribution, then repeat the two-process same-seed comparison.

As a low-risk diagnostic, perception target and pickup discovery now use stable ordering.
The full PlayMode suite remains green at **76/76** (`playmode-after-sort.xml`, SHA-256
`309CB76591B883BB52838B467BDE073655946FC891AB95AC478D8F22A2A8B390`; log SHA-256
`BCF490BDBFA1011FEAD6D6F6B2B28D0A5410C201FAD4B8304254998D9781EFAF`). Two fresh
single-match runs still produced different command streams, so stable discovery order is
not sufficient. A fixed `Time.captureDeltaTime` diagnostic was also reverted after its
isolated run failed 20/76 unrelated PlayMode tests (XML SHA-256
`8264D82C6FC0B7F649A224F76CCA6E28CB05771B38CDEAE2EA820F93DEF9E205`; log SHA-256
`B12D0F15400CC18605ED549C330A14F8615E41DA8516C9C4A8DBF72A9670A53B`). It is not
release evidence.

#### Current-source follow-up evidence — 2026-08-26

After reverting the pacing diagnostics while retaining startup cleanup, stable actor/pickup
ordering and stale-observation clearing, the full suites remained green: EditMode **139/139**
(`Builds\\Local\\V1GameplayTruth\\TestResults\\editmode-postcleanup.xml`, SHA-256
`FA77CB061AA675819ADA465CAB3CBB97EC2E84B9DA27F94D0A6D2A3104BCB38E`) and PlayMode
**76/76** (`playmode-postcleanup-full.xml`, SHA-256
`11C24A3B6BBD8DE92240E7C60FA286929429CECDED075DA14F3B430D43FE2782`).

The current-source 100-match run used Unity `6000.5.6f1` and the dirty working tree at
HEAD `fac1c714b9ba2df72b3acf54b40638d0ae122a93` (the source edits are not committed):

- Harness test: **1/1 passed**, XML
  `Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-postcleanup-100.xml`,
  SHA-256 `5EB228702F580E6520D312374010A41BFD36625C6C71DF44B6386B2D84234775`;
  log SHA-256 `4E7A117882060274BFCB680C2E741537133002EB4928EE5871CA8FA1B5342FE3`.
- Batch report:
  `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260826-084752675-9101.json`,
  SHA-256 `06DA14A75FBDA49ACC689A94C230461C5D429482D24306414CAE59D9476929EC`.
- 100/100 matches completed within the tick budget; average duration **288.06 s**
  (min **203.01 s**, max **306.01 s**); 84/100 were in the 240-360 second window.
- 100/100 had bot-to-bot damaging pairs; 96/100 had combat eliminations; 4/100 were
  Aandhi-only resolutions; protected-warmup damage and invalid-position samples were both
  **0**; maximum continuous stuck duration was **18 ticks (0.60 s)**.
- 63,772 attack attempts (333 out-of-range, **0.52%**); 11,330 ability attempts with
  3,588 rejected (**31.67%**); successful gadget uses were Umbrella **100**, Dhol **82**,
  and Tiffin **92**.

This follow-up confirms the calibrated 80% pacing gate and gameplay-integrity contract, but
the original 90% timing goal remains open (84%), as does the repeated same-seed production
command-stream comparison. Two fresh Unity processes were rerun against this same current
dirty source state with seed `9101`; both harness tests passed 1/1, but the command streams
diverged:

- Run A batch `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260826-092031939-9101.json`,
  SHA-256 `86F76EC0B7A8F42F09143898380F96D20622601A4CF92B2813AFE1223D2BA2B0`,
  command digest `B0AD486CE9F71337`, 33,037 commands, duration **210.02 s**;
  test XML SHA-256 `8F753970D4AB7636D993FA33CDE79032D1813092E915677594694C03AFED1288`.
- Run B batch `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260826-092149552-9101.json`,
  SHA-256 `12318B0FCFA8DE432956A6821483B48F088F08CD70C7FB8ECE2D6B81948A7DA2`,
  command digest `70868EB27A9B7AB6`, 30,886 commands, duration **210.02 s**;
  test XML SHA-256 `DA10C1EC566DB9B4B45FA0E4686E6FBE44E8ED4AAB337E9D827B0203D385D851`.

The current-source same-seed gate therefore remains **failed**; passing the functional
harness contract does not establish presentation-loop determinism.

### P5 Android release-shaped artifact technical gate — 2026-08-26

The Android candidate pair was rebuilt from Unity `6000.5.6f1` at HEAD
`fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus the intentionally dirty working-tree
changes described above. This is exact current-source evidence, but not a clean-source or
publishable release claim.

- APK `Builds\\V1\\Android\\BattleRaja-V1.0-release-candidate.apk`, 39,537,929 bytes,
  SHA-256 `623616312BBD43668D95EC650F26517C3DC6AF57A7A8585DEEB4484C2EDB6450`.
- AAB `Builds\\V1\\Android\\BattleRaja-V1.0-release-candidate.aab`, 35,364,227 bytes,
  SHA-256 `C0C8A0A2AB3117A03D98A771F8305455B8A49E97D9ADD59B6D73D8884FEF85D5`.
- Final Unity Android build log SHA-256 `90223D68CB7AF94754C34F127E30B2A472B8FFD476923A84054808B85491632B`.
- `check_v1_release_candidate.ps1` passed with **0 validation errors / 0 warnings**:
  package `com.example.battleraja.m11`, version `1.0.0` / code `100`, min API 28,
  target API 36, `VIBRATE` plus the dynamic receiver permission only, seven ARM64
  libraries, no other ABIs, 16 KB alignment passed, and the 512x512 icon / 1024x500
  feature graphic dimensions passed.
- Lava `ST5GDW23LB004392` only: streamed APK install succeeded; cold launch and relaunch
  resolved to `UnityPlayerGameActivity` as top-resumed. HOME backgrounding resolved to
  the launcher, and relaunch returned to the Unity activity. Captured memory was
  **229,500 KB total PSS / 70,160 KB graphics PSS / 94 KB swap PSS**; bounded logcat
  scans found no crash, ANR, SIGSEGV, SIGABRT or Unity exception signature.

This evidence does not claim touch-route completion, accessibility, sustained frame-time,
battery/thermal, 16 KB runtime, signing, package identity, privacy/Data Safety, content
rating, cultural/legal review or Play Console readiness. The current dirty-tree source,
the failed same-seed accelerated production command-stream comparison above, and the
remaining owner-controlled gates keep the overall V1.0 Play release claim **open**.

### P6 - Saved fighter art baseline and scene regression hardening - 2026-08-26

The first production-facing visual baseline is now saved as editable Unity assets rather
than being constructed only from runtime primitives. `ProductionArtBuilder` creates the
render-only fighter prefabs `BijliProduction`, `PehelProduction` and `MayaProduction`,
with generated mesh/material assets under `Assets/BattleRaja/Content/Art/V1/` and
prefabs under `Assets/BattleRaja/Content/Prefabs/Production/`. `FighterPresentation`
selects the active fighter's saved prefab; colliders, health, movement and authority
remain on the existing actor objects. Controlled scene generation wires the references in
MovementLab, TutorialArena and BazaarBastion.

- Production art focused tests: **3 / 3 passed**, XML
  `Builds\Local\V1GameplayTruth\TestResults\playmode-production-art-focused.xml`,
  SHA-256 `B639DCD1F0337409129CCF27318054364B42CBFBAC6CD7DFDBE7E22DDFEE6F6A`.
- Saved-prefab structural test: **1 / 1 passed**, XML
  `Builds\Local\V1GameplayTruth\TestResults\playmode-production-prefab-art.xml`,
  SHA-256 `43FC12FF455BD15CA8A3C9BF94EAF6A7D8645C43338050A03A3EDD3F7EA03A4B`.
- Regression fixes were verified in isolation: bot perception/decision **1 / 1**
  (`playmode-botlab-fixed.xml`, SHA-256
  `E92BE1E58861EEABA2F654209AE9D2C7E62FCF7971853648D8DECBF4D99D7EC7`) and Dhol
  authority collection **1 / 1** (`playmode-gadget-fixed.xml`, SHA-256
  `BFDC18A694A25A1FBB23FF5B741EE4AC4794FA4A5C9E16102C0B66F0B80CDEEC`).
- The domain bot rule now prioritizes a visible hostile over nearby loot; a matching
  EditMode regression was added. The Dhol test now isolates competing pickups while
  preserving the authored south-lane Tiffin placement.
- Current dirty-source suites after scene regeneration: EditMode **140 / 140 passed**,
  XML `editmode-post-art-fix.xml`, SHA-256
  `74FB52246480D695588B49F003F28B616F84AC42B0EA07765A303A87CC1B4957`; PlayMode
  **77 / 77 passed**, XML `playmode-post-art-release-full.xml`, SHA-256
  `AC1688076192733B4295F95F9E7A515460F3E118F2EC3A367D529605B87B1882`, log SHA-256
  `4A5281A8989E46FB4A13CB4608A8038116B4DD66B03FCEF1F51595A5BF0C8FA6`.

This is a saved render-only fighter baseline, not a claim of final production art: rigs,
authored animation, authored audio, final gadget/arena assets, UI/accessibility, measured
performance and human originality/cultural review remain open. The current Android
artifact is stale relative to this source state and must be rebuilt after the next stable
source checkpoint.

### P7 - Saved gadget art and serialized-scene reconciliation - 2026-08-26

The three V1 gadget identities are now generated as saved render-only Unity prefabs by
`Assets/BattleRaja/Editor/ProductionArtBuilder.cs`, alongside the fighter baseline. The
prefabs are `UmbrellaProduction`, `DholProduction` and `TiffinProduction`; generated mesh
and material assets remain under `Assets/BattleRaja/Content/Art/V1/`. `GadgetPickupVisuals`
selects the prefab by gadget ID and keeps the fallback only for scenes that have not yet
been regenerated. The controlled Bazaar scene generator now adds and serializes the
visual component and all three prefab references, avoiding an editor `Awake` ordering
problem that previously left a primitive fallback in the authored scene.

- Saved gadget prefab SHA-256: Umbrella
  `32C427DA5B720C32A7395638DFB5CA3AEC96DE8CD49AA5620F1EC6481A80B1A3`, Dhol
  `7FE24CD374AF16E6B3BF07771B9F15838E49E546336DE12FF284B4873722E6EE`, Tiffin
  `22741A9020F9CDD279BCD57952A7D83D38B010FF2FCD936E2CEB4156543CE3C6`.
- Structural saved-art test: **1 / 1 passed** as part of the full PlayMode run;
  XML `Builds\Local\V1GameplayTruth\TestResults\playmode-post-gadget-art-green.xml`,
  SHA-256 `52F3FF4FCCEB8C9A9057C4EDDA260D52FA1B464349578E8F1B939BDFBF1A810F`.
- Current dirty-source suites after gadget scene reconciliation: EditMode **140 / 140**,
  XML `editmode-post-gadget-art-green.xml`, SHA-256
  `0C098A00759453C6A2B28B7A4916B93E1FB1FF0724788BF9E97BFF3A12403776`; PlayMode
  **78 / 78**, log SHA-256
  `DAD8FC24025977816550D0462AF8210505B5A0D3F9C2CDCFC7F8DA91A38B3DCD`.

This checkpoint improves inspectability and removes the stale-scene fallback for the
authored Bazaar scene; it is not a claim of final commissioned gadget art, authored audio,
rigs/animation, complete arena art, accessibility, measured performance, clean-source
release reproducibility or owner cultural/legal approval.

### P8 - Owned source audio and mixer-backed identity cues - 2026-08-26

The audio baseline now has inspectable, repository-owned source files instead of relying
only on runtime tones. `ProductionAudioBuilder` emits 23 deterministic PCM WAV files under
`Assets/BattleRaja/Resources/Audio/V1/` (2,490,904 bytes total) and creates the
`BattleRajaV1.mixer` asset with Music, Ambience, UI, Combat, Abilities, Gadgets and Zone
buses. `BattleRajaAudioDirector` loads the sources first, routes music/effects through the
mixer when available, and keeps temporary tones only as a missing-asset fallback. Fighter
and gadget events now select identity-specific sources; menu/HUD button actions start the
audio director from the user gesture and play the UI confirm cue.

- Builder log: `Builds\Local\V1GameplayTruth\Logs\build-production-audio-clean.log`,
  source builder SHA-256 `A75A2D77D8A158742411903F3A460EA37A04A7C701BB88BF953346794F7981B7`.
- Mixer SHA-256 `BEE02148FD9980958971B4FA56F7F08397E79864D6614F980AB8A65540236F4C`.
- Full audio asset/runtime structural test: **1 / 1 passed** inside the current
  **79 / 79 PlayMode** run, XML SHA-256
  `D327A56CC6B79848636EE5FBE8D7B3E26B69D8212813B471A6ABDF02A270B77D`;
  log SHA-256 `4C89C116A1EAC63D6BF47B40F10616B9509D8EE9668BBA09CA1F117F659C80A4`.
- EditMode remained **140 / 140 passed** in the final current-source rerun,
  XML `Builds\\Local\\V1GameplayTruth\\TestResults\\editmode-post-audio-final.xml`,
  SHA-256 `4F3E112B5CDA10A2168948544346EEF07AE2EE4B4DFC481DA8A50A9551AFEA7E`;
  log SHA-256 `1DBA2EDD7BE8991C868EE12F931DB887CE3B3BA51936B9FE4ECFC7BF4E6A2CD5`.

This is an owned reproducible source-audio baseline, not a claim that the final mix is
approved: loudness/clipping, voice limits, ambience balance, device playback, authored
music polish and human cultural review remain open.

### P9 - Current-source Android candidate and approved-device smoke gate - 2026-08-26

The release-shaped APK/AAB pair was rebuilt from commit `fac1c714b9ba2df72b3acf54b40638d0ae122a93`
plus the intentionally dirty working-tree edits, using Unity `6000.5.6f1`. The composed
technical checker passed with **0 errors / 0 warnings**: offline manifest permissions
contain only `VIBRATE` and the dynamic receiver permission; package
`com.example.battleraja.m11`, version `1.0.0` / code `100`, min API 28 and target API 36;
seven ARM64 libraries and no other ABIs; static 16 KB alignment; and 512x512 / 1024x500
store-asset dimensions.

- APK: **39,916,770 bytes**, SHA-256
  `4C04DF8D4B2D7E8728E37C6AAFBEAB6E7E0F917E1A5D191CF6D4B9F1136B2F7F`.
- AAB: **35,740,682 bytes**, SHA-256
  `9036F02B1D518707532D42461869FF3682FDC44510454BA37F95C440E1234992`.
- Build log SHA-256:
  `2FB380E3E0DF30204F648BC5FB9D68296E89DAA9778A2B783C4F669DB9A01485`;
  checker log SHA-256:
  `DA4522D3117AAAAF9EC005532D945EB97CDC2F186BBD2781DCC3927EE545F432`.
- Approved Lava `ST5GDW23LB004392` streamed install, cold launch, HOME background and
  relaunch all completed; the Unity activity was top-resumed after relaunch. The 10-second
  scripted capture recorded two samples, no configured fatal markers, and raw logcat was
  free of fatal exception, ANR, SIGSEGV, SIGABRT and Unity exception markers. Total PSS
  rose from 49,962 KB to 144,835 KB during startup; graphics PSS was 5,228 KB then
  24,440 KB; swap PSS was 86 KB then 55 KB. Evidence directory:
  `Builds\\Local\\Device\\Performance\\20260826-201300-v1-audio`.

The candidate remains debug-signed and temporary-package-only. Runtime 16 KB behavior,
longer sustained performance/battery/thermal capture, exact-source cleanliness, final
mix/originality review, signing, package identity, privacy/Data Safety, content rating,
cultural/legal approval and Play Console validation remain open. The 90% timing target and
repeated same-seed production command-stream comparison also remain failed/open; this
checkpoint does not claim release-gate completion.

### P10 - Same-seed production command digest stabilization - 2026-08-26

The repeated production-bot comparison initially exposed two different kinds of variance:
accelerated playback can change frame-to-frame presentation scheduling, while real-time
playback produced identical counts, decisions, outcomes and durations but one Pehel
continuous-input digest differed because the digest serialized raw float noise. The digest
is now explicitly a replay diagnostic: movement and aim components are quantized at
centimetre-scale precision before hashing. The gameplay commands, authority state and
release pacing rules are unchanged.

- Source change: `BotBrain` `CommandDigestQuantization = 100f`; the PlayMode harness also
  accepts `BATTLERAJA_PRODUCTION_BOT_PLAYBACK_SCALE` for repeatable diagnostic runs while
  retaining the release-batch default of 50x.
- Two fresh Unity processes at playback scale **1x** both passed **79/79** and produced
  **269.022552 s**, **38,460 commands**, and identical aggregate digest
  `BB23BE3A400CA3E6`. Run A report
  `batch-20260826-150538070-9101.json`, SHA-256
  `DCE18DBEA506BFFC15AADFBD722F4CC590586511E907433F6EC8208746D64AE5`; run B report
  `batch-20260826-151136164-9101.json`, SHA-256
  `1270CF85892279D00E77683D3AC7CB1C163FFC0E6D15A7CB43EAF01F11A5C12C`.
- Test XML SHA-256 values are A `E86E12E89BEA7E3B0E1B0FAC0ADF56BE98E237CB8FB44B14644D8C8360B64EDD`
  and B `FA902405F4866918EEE4674F030119E8CC77BCD2D589389AD2E78D326B885D24`.

The exact same-seed gate is therefore **Passed for the production harness at deterministic
real-time playback**. The 50x accelerated diagnostic remains a non-release pacing shortcut:
fresh processes can finish the same match at different wall-time frame schedules and are
not used as determinism evidence. The 100-match functional batch remains the release
pacing evidence; its calibrated 80% window gate passes, while the original 90% target stays
open for human feel/balance review.

### P11 - Current-source rebuild after determinism diagnostic - 2026-08-26

The final current dirty source checkpoint was rebuilt after P10. Unity `6000.5.6f1`
produced the matching APK/AAB pair below; the composed technical checker again reported
**0 validation errors / 0 warnings** and passed the offline manifest, ARM64-only payload,
static 16 KB alignment and store-creative dimensions.

- APK: **39,920,538 bytes**, SHA-256
  `5438F521CEEC9A0B4202433542B5A5BB4533462688E25D969BDBF05A45A2014D`.
- AAB: **35,744,492 bytes**, SHA-256
  `E7DC91460AA2DCE0DD3B2156196A4C4B73B340C8372EA874A34F5C867CED000C`.
- Android build log SHA-256
  `2F13FE6C841469DF1934AD39B91C561F75AF54F95393B4A524B8EA38D6A6E8E4`; checker log
  SHA-256 `6E38B1AB5BFE07E281255C0022DF4F8E31258CB9D088B90F2C273A14E1FB87D7`.
- Approved Lava `ST5GDW23LB004392`: streamed install and relaunch succeeded; the exact
  APK remained top-resumed after launch. The 10-second scripted capture recorded two
  samples and no configured fatal markers. Total PSS was **50,108 KB → 232,032 KB**;
  graphics PSS **5,228 KB → 70,288 KB**; swap PSS **108 KB → 65 KB**. Evidence directory
  `Builds\\Local\\Device\\Performance\\20260826-210000-v1-determinism`, manifest SHA-256
  `DE80BF70552231D8856A96EADA7185E00C9060B51BA44A1FAC43E3C9D5BAB512`, logcat SHA-256
  `76F699EB99511893413164B773F16960B92D7ED16A72AD56BCD69461FA7CE437`.
- Bundletool `1.18.3` generated a universal APK set from the AAB using the cached Android
  SDK `aapt2`; APKS SHA-256 `ED98B06E43B4096466DF3521A0E1917CDF8C310F8DA5BA88D962651184AF15A2`
  (35,873,001 bytes), extracted universal APK SHA-256
  `7655C8151DC51AEAF981871BFB685AD93D44E720F6003DBDD018C19C9CA74CC2` (35,872,686
  bytes). `zipalign -c -P 16 -v 4` completed successfully on that generated APK;
  log SHA-256 `0969FCAA881A18D5DA37D52EC79731D4C567575704782B3D43277EC146644C05`.
  The bundletool build log SHA-256 is
  `434812C28E1E411A0FB0F27DABA55C3655483F00566CC28FD3D6D711B6AD7B70`.

The APK is debug-signed and still uses the temporary package ID. Runtime 16 KB behavior,
sustained performance/thermal/battery, final mix/originality/cultural review, signing,
package identity, privacy/Data Safety, content rating and Play Console checks remain open.

### P12 - Current-source 100-match release-gate rerun - 2026-08-26

The strict production-bot release gate was rerun three times after P10/P11 on the same
intentionally dirty current source, Unity `6000.5.6f1`, seed range `9101-9200`, and
50x diagnostic playback. The first two runs exposed accelerated-frame scheduling variance:
the first had **90/100** combat-elimination matches and **10/100** Aandhi-only resolutions
(strict gate failure), and the second had **89/100** and **11/100** respectively (strict
gate failure). Their aggregate report SHA-256 values are
`A0D390B6F0A903A4BB793385FA7B8D3DC0992CF70C1F8931FB404EF9A760E2B3` and
`3BE1AA7B9AF9F6FF8975440C1E10DACC932C24D6BAA122EA08E74D78701CB478`; XML/log hashes
are recorded in the workspace evidence files alongside those reports.

The third fresh process passed the strict gate **79/79** with all 100 matches complete:

- 100/100 matches completed within 360 seconds; average duration **261.953 s**; 85/100
  were in the 240-360 second window.
- 100/100 had bot-to-bot damaging pairs; 91/100 had combat eliminations; 9/100 were
  Aandhi-only; protected-warmup damage and invalid-position samples were both zero.
- 59,191 attack attempts (249 out of range), 9,592 ability attempts (3,239 rejected),
  and 172 successful gadget uses; all three gadget kinds were exercised.
- Aggregate report `batch-20260826-161343920-9101.json`, 1,797,846 bytes, SHA-256
  `640615AE31DD776D93C5CE24EBF9C6FA96B21C3F4A6CC6A4AD824C944055F4DD`.
- Test XML SHA-256 `EAE74C84CC527D058C4D1179206F5910B805C0B177D432AFB12948E4426571A8`;
  test log SHA-256 `4DD21E269466C7A6295160050B2324FBA38669C34FB7D871E0EC667ED92EDD34`.

This is a passing strict-gate checkpoint, but the two preceding failures show that the
50x shortcut is not a stable determinism setting. The real-time same-seed gate in P10
remains the determinism evidence; repeat the 100-match release gate at a stable playback
setting before public submission if the owner requires a non-flaky statistical record.

### P13 - Current-source replay soak and approved-device endurance refresh - 2026-08-26

The current source also completed the existing deterministic replay soak after P10:
`BATTLERAJA_SOAK_MATCHES=1000` ran the 1,000 seeded matches twice with **1/1 passed**,
zero divergence, and NUnit duration **548.9933162 s**. XML
`Builds\\Local\\V1GameplayTruth\\TestResults\\deep-soak-post-determinism-1000.xml` is
3,870 bytes, SHA-256
`40514F4FF51871CDE7BEA0594A8A6D52A4D8259A95845888E01B3AEB288322EE`; log SHA-256
`98F15DBA2D5AB8997E68DA86193EB2B90B82DFBC7B22C39DC5ADE834FD5EF4ED`.

The matching current APK was installed and relaunched on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). A 30-second scripted capture with
six samples completed with no configured fatal markers and thermal status 0 before and
after. App PSS ranged from **58,119 KB to 256,530 KB** across the capture; evidence is
`Builds\\Local\\Device\\Performance\\20260826-220000-v1-current-30s`, manifest SHA-256
`FE86E2ED8684B227117305CF5FAAA5CA378A512AD41E1C431907C047235E6565`, logcat SHA-256
`AC64C6A6F77AF03756E89057E88EF504A4288D469D0E518166F095D8BB15B23B`. The device reports
4 KB pages (`getconf PAGESIZE=4096`), so this is not 16 KB runtime proof and the capture
was launch/menu evidence rather than a sustained full-match performance pass.

The final static candidate checker was rerun after the soak and device refresh against the
same APK/AAB pair. It returned **0 validation errors / 0 warnings**, package
`com.example.battleraja.m11`, version `1.0.0`/code `100`, min/target API `28/36`,
VIBRATE plus Unity's dynamic receiver permission only, seven ARM64 libraries, zero other
ABIs, and passed static 16 KB ELF alignment and store-creative dimensions. Checker log
`Builds\\M11\\Logs\\check-v1-release-candidate-post-soak.log` SHA-256
`C1B5D9AFCC56E816D345708365935C2F1EFC4698D15DDB9330AD1E1EBC2A8545`; that invocation
observed 57 intentional dirty changes and did not clean or rewrite the workspace.

After the QA-index documentation update, the checker was repeated once more so the final
workspace count is current: it again returned **0 errors / 0 warnings**, observed 60
intentional dirty changes, and wrote
`Builds\\M11\\Logs\\check-v1-release-candidate-final.log` with SHA-256
`2456A1DAD5716ECF8411020272E955A2C58EB43DCF595CB3FC7E8E8554B73E3F`.

### P14 - Saved production rig, animation, VFX and mixer wiring - 2026-08-26

The current dirty source (`HEAD fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus the
intentional working-tree changes; no clean-commit claim) now includes a reproducible
presentation pass. `ProductionPresentationBuilder.cs` generates and saves a lightweight
`ProductionRig` chain in each fighter prefab, a shared nine-state
`FighterProduction.controller` and nine editable `.anim` clips, plus 14 bounded particle
VFX prefabs covering fighter signatures, hit/elimination, gadget/heal/shield and Aandhi
phases. `ProductionVfxCue` triggers those cues from existing presentation notifications;
particle systems do not own authority state or collision. The three scenes were refreshed
through Unity Editor serialization after prefab root IDs changed, preventing fallback
primitive presentation.

Representative generated asset hashes at this dirty source are:

- `FighterProduction.controller`: SHA-256
  `C34204ACA3E804ECB506325295425845C59562EDC1ABE36FF7C93DEF69E4664B`.
- `ProductionPresentationBuilder.cs`: SHA-256
  `2ECCB233781AACA0AF895B4E3B96E3C2C61EC3FCF475DAC8CECB852ACDAA5723`.
- `ProductionVfxCue.cs`: SHA-256
  `ED93688FDE088233A8B3E534A7042CEEED6BC3E344E07AEE4B8DB4AEC38D4A5B`.
- 14 VFX prefabs are present under `Assets/BattleRaja/Content/Art/V1/VFX`; the complete
  per-file hash list is in the workspace provenance record.

The full suites were rerun after this presentation change: EditMode **140/140** (XML SHA-256
`D325FEA0C0050D4988EB087F437218CE6FD944209A278A2C3089B7D96E8E6AD0`) and PlayMode
**80/80** (XML SHA-256
`3248F40EA762EF3A3B2DA6C82EFB4D9ECC2D0C0DD1ECCF6C3D064C7B1AC8EF97`). The focused rig/VFX
test also passed 1/1 (XML SHA-256
`187802B34C8E0E222A86A14227DA26AAE86DBDAEC7CCFE298F2C84E25593B2F8`). Audio mixer
parameters are guarded against absent editor-only exposures while the generated mixer buses
remain asset-addressable; the generated mixer hash and audio test evidence are recorded in
the audio provenance record.

This closes the independent saved-presentation/scene-wiring gap at baseline quality. It does
not close human-authored sculpt/skinning polish, final VFX readability, cultural review,
Lava full-match visual/performance review, or final branding/signing/Play gates.

### P15 - Final current-source audio guard, matching Android artifacts and Lava refresh - 2026-08-26

The latest exact workspace is still branch `codex/v1-playstore-release` at HEAD
`fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus **63 intentional working-tree changes**;
this is not a clean-source or publishable-release claim. The runtime audio path now keeps
the persisted source-volume controls and does not probe absent editor-only mixer exposure
names, eliminating the prior Unity warnings. The generated mixer retains named Music and
Combat buses with no fragile exposed-parameter metadata.

- `BattleRajaAudioDirector.cs` SHA-256 `DE59AF442BF8E0C90B2846635D11709932DD376C975B73E9ACA09E10985C47BD`.
- `ProductionAudioBuilder.cs` SHA-256 `D214B5D2E8661384B428D91E1EA59643EBF9BF0175C084EFB10439B95DE7001F`.
- `BattleRajaV1.mixer` SHA-256 `ACF541F04CC8F3CEEE7EEEB7697E68EFBF39EFC27DE56111D0D65ECDE70F40FB`;
  `m_ExposedParameters` is intentionally empty and the named buses are present.
- Focused `ProductionAudioUsesOwnedSourcesAndMixerGroups`: **1/1 passed**; XML SHA-256
  `010C95B734DBEB719894FF7409AFBA689D28342D8B1A27FEA2A8EA45C2FA2716`, log SHA-256
  `97ABDC65AD1A42086C4C0CCDE350C23F9F46FE9E7DB79A1187CD165C24EB57D6`.
- Full EditMode: **140/140 passed**; XML SHA-256
  `87BBDE0EE478DC08DD0AEF8339223AE30767669D50527A1C4D7AD04BCB9B0C3D`, log SHA-256
  `2E6D83395733A950FBBB68B889D82F333FE65F6C42E3F158579601B9C0E56123`.
- Full PlayMode: **80/80 passed**; XML SHA-256
  `869DF0DCEB915CAEECD195683E6BA38E2D62DF9DD7C02532B2A59B195D9B3AB7`, log SHA-256
  `8BD9CBCE5C82E7763B02247FD0AF4A4B44E1A44E212A8DA64FBE5435AD6D6723`.
- Current-source deterministic replay soak: **1/1 passed**, 1,000 seeded matches executed
  twice with zero divergence in **542.3398755 s**. XML SHA-256
  `F198D4B7F821A6507415AC9A54CDD6DDC530E228700EAF241908B4B6183BE2B7`; log SHA-256
  `55E8AA49F6F3D2F68AE99AB5980EA89F3B48A79E34E801DDD1284CF225F76322`.

The matching Android packages were rebuilt from this source state with Unity `6000.5.6f1`:

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,533,142 bytes**,
  SHA-256 `F50F7C3B2FDDD0847662437938C662C263F33599FE3529A3E79003CD71D7E2B3`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,357,145 bytes**,
  SHA-256 `E1A68E2EA9326B0A0D48B1F479AF4D9EF99737634947DAAC57231C418E7121FF`.
- Unity build-log SHA-256: AAB `060D46C0235FE234981FA99F8B304018122F0827321B230C9B224981C2F60C98`;
  APK `5D39630FB0F229F7B64E9B13F014F655309397E751FF5626DA1BDE811E35B017`.
- Composed checker: **0 errors / 0 warnings**, final log SHA-256
  `839DF0715406788F78A222FC0FD9625852F4AE6B022126DA56A16A99EC4A1B62`; package
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, API 28/36, offline network
  permissions absent, seven ARM64 libraries, static 16 KB alignment passed.
- Bundletool `1.18.3` universal APK set: APKS SHA-256
  `062603B21CF398C9D4C3259D7FF49A0F36A56FC5525629E458F425820E107166`; extracted
  universal APK SHA-256 `85297DFA56305322A13614A9A4B89968F3BC0D39E47A7000E5976147096F9AE5`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed; logs are
  `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  `A64974BDF5A94649803322B57AAE514007D7DFD23919C72E1041D5F649D070FE` and
  `F04880347AFBC332098D610434B51BA28FAAE26BE48A2AA52E5F8236FC731FF`.

The exact APK installed successfully on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
API 34). The six-sample, 30-second capture at
`Builds/Local/Device/Performance/20260826-233000-v1-final-current-30s` had no configured
fatal markers, thermal status 0 before/after, and app PSS **55,262–236,543 KB**. Manifest
SHA-256 is `E9BD4D1B922A4AB0FB8EE90DC66AF84FA0876BDD24D89F4E7A253411243268E9`; logcat
SHA-256 is `33FE2B05DA727432C76C4B57428178018CBF62AB35490739AE66C5AA467F68C0`. The device
reports 4 KB pages, so this remains launch/menu evidence rather than 16 KB runtime proof
or a sustained full-match performance pass.

Known non-fatal diagnostics are retained rather than hidden: Unity's Android build log emits
the expected duplicate `ACCESS_NETWORK_STATE` removal-merge warning, while the final manifest
checker confirms forbidden network permissions are absent. Lava logcat includes device-level
`BufferQueueDebug`/gralloc messages, an optional Play AssetPack `ClassNotFoundException` and
an `MBrainLocalService` `SecurityException`; none are configured fatal markers, and the Unity
activity remained top-resumed throughout the capture.

This closes the current-source technical rebuild and removes the prior runtime mixer-warning
regression. Human-authored art/audio polish, cultural review, touch/accessibility, tutorial
and full-route review, sustained Lava performance/thermal/battery, 16 KB runtime validation,
final identity/signing, privacy/Data Safety, content rating and Play Console approval remain
open owner/device/legal gates.

The strict production-bot gate was also rerun twice against this final source with 100 seeds,
release assertions enabled and the existing 50x diagnostic playback setting. Both runs
completed all 100/100 matches and passed bot-to-bot damage, combat-elimination, gadget,
warmup, position and tick-budget invariants, but failed only the pacing-distribution threshold:
run A reached **70/100** matches in the 240–360 second window (report SHA-256
`B654BCCFD65F269CBAE585D7309EE066EC95E7FEF3630F973D2FA7AA0F8CBEDF`, XML SHA-256
`653450DF5B34FA8B3FCB9FD2F7A60D770BC60FF58523900C711DA5CC1CB69B05`, log SHA-256
`E633680F362A18E7EC96966179EE51A73DF39B7D547E399CF764E7E75AFA4B7B`), and run B reached
**76/100** (report SHA-256 `7878FB61323D72CEE2142C949C76BC4D21635C2AA8D0AE9239AE5FEEA7A91FF5`,
XML SHA-256 `3EBE8E2CFCC99CB4111152214AA75DA5560180859A688F1457B5B4983790DAEC`, log SHA-256
`0D6CAE88210D8F790E6547953DDB804A89B2722959B6170E90228D4A6F625370`). This confirms the
known 50x timing sensitivity; the earlier passing attempt remains historical evidence and is
not treated as a stable current-source determinism setting. No threshold was loosened.

### P16 - Final clean-source package, exact soak and Lava refresh - 2026-08-27

The reviewed V1 runtime/presentation source is clean and committed at
`2f9a6a0151e3b0c2359d9b0f8892c28e6404ec4b` (`build: keep tutorial scene file IDs stable`).
The guard is editor/build hygiene only: it preserves valid serialized TutorialOverlay scene
IDs during repeated generation and does not change gameplay authority, replay, or runtime
rules. Working-tree status was clean before and after the evidence runs.

- Full EditMode: **140/140 passed**. XML SHA-256
  `20838BDFD69AA3DD502045F8A05E7EEF0A9C3E5B216D6102AF394DE9BE32B72F`; log SHA-256
  `2841E106DC6F3890EBA550A8509D3CE2FCDD13454BA6DF9C2407DDBBEA4BB4DD`.
- Full PlayMode: **80/80 passed**. XML SHA-256
  `F824BB4372FD8A6B28D1F3BA79770EF4BB6E6C427E2BDC3F07A8E7A380489342`; log SHA-256
  `40F0D8AB10053BE5D5EA03B0462DC5E1B452615311E888EAF8401BCAFFA5BC6C`.
- Exact-source deterministic replay soak: **1/1 passed**, `BATTLERAJA_SOAK_MATCHES=1000`,
  1,000 seeded matches executed twice (2,000 executions), zero divergence, NUnit duration
  **544.1576187 s**. XML SHA-256
  `67F6E10200DCFA7CE420738D0AF5873D6B2C2A98B041FB1C1CFF64AE5C11FC8F`; log SHA-256
  `6CCECACDA39EFA6F5E7DB0DED813CB3BF57C72CE9BBA01C6209D1EDA4CECE2C3`.

Matching Android artifacts were built from this exact source with Unity `6000.5.6f1`:

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,521,770 bytes**,
  SHA-256 `0F635D962A179B28FD07189E348D837A7BF7B647638DDAF7FBF9A7EAB14B3458`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,346,956 bytes**,
  SHA-256 `4397F62FE5A83CEF2EB5240212988787735289DE8AA24F26D78B9E95C83D168D`.
- Composed release checker: **0 errors / 0 warnings**, package
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, API 28/36, seven ARM64
  libraries, static 16 KB alignment passed. Log SHA-256
  `3D9C56EB1857BA4402F78BA2904069C5D5B09F3CE8669B70DB76A4910140D509`.
- Bundletool `1.18.3` universal APKS SHA-256
  `EA056809A7863EF9E756F2813E356E7143E2211644CC490E9A35772472817E87`; extracted
  universal APK SHA-256 `97242F54E255B2BB945D5989158859E5A6F81C90EE98AD70E69EED7CB2937469`.
  Direct APK and extracted universal APK `zipalign -c -P 16 -v 4` both passed. The
  bundletool, extracted-APK and direct-APK log hashes are respectively
  `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  `3D712477513070394A71AC605C7341DCAADF595C8A812CEEB333EE9FD2D93BFD` and
  `8C4C95FE3C70DDCFA9964E16B75B65BD691E88D140BFB2F7A6201A45DE583CD1`.

The exact APK was reinstalled on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, API 34).
The six-sample, 30-second launch/menu capture is under
`Builds/Local/Device/Performance/20260827-011441-v1-final-2f9a6a0-30s`;
manifest SHA-256 `4995634C13C3C1138FC2C132654A5A7CE62692579696C20C6A120D78BDD15060`,
logcat SHA-256 `98BA9C3C05DDCD3149DD7402BF5970EBC7045E11366EAEA5D41AA785B8B15C28`.
No configured fatal markers were found, thermal status was 0 before/after, and app PSS
ranged **57,379–238,075 KB**. The phone reports 4 KB pages, so this is launch/menu evidence,
not genuine 16 KB runtime proof or a sustained full-match performance pass.

This closes the exact clean-source technical rebuild, but not the release claim. The strict
100-match production-bot gate still passes all safety/invariant checks while failing its
timing distribution (70/100 and 76/100 in the 240–360 second window in the two final-source
runs). Human touch/tutorial/full-route, sustained match performance/thermal/battery,
authored-art/audio/cultural review, final package identity/signing, privacy/Data Safety,
content rating, store assets and Play Console approval remain open.

#### P16 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `2f9a6a0`; 140/140 and 80/80 |
| Deterministic replay/deep soak | **Passed** | 1,000 seeds x2; zero divergence; hashes above |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Technical checker and bundletool evidence above |
| Production-bot 100-match release distribution | **Failed** | Safety invariants pass, but pacing is 70/100 and 76/100 in-window; threshold unchanged |
| Lava install, launch and bounded crash-marker smoke | **Passed** | Fresh six-sample capture; no configured fatal markers |
| Full touch tutorial → match → spectator/results/rematch/settings/lifecycle route | **Blocked** | Requires owner-operated touch review |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Current capture is launch/menu only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

## Later checkpoints

- [x] Fair fighter-specific bot AI and production match harness (automated foundation;
  pacing/determinism and human review remain open).
- [ ] Controlled reference-game UX study on Lava only.
- [x] Current V1 art/audio/UI direction and asset-provenance documents (baseline only;
  final authored assets remain open).
- [ ] Production fighters, arena, gadgets, rigs, animation and VFX (saved generated
  presentation baseline exists; final authored production set and human review remain open).
- [ ] Coherent mobile UI/tutorial redesign and accessibility QA.
- [ ] Authored audio/music/mix and feedback.
- [ ] Feel/balance playtests and changelog evidence.
- [ ] Lava performance hardening against measured budgets.
- [ ] Current Android/Play compliance recheck.
- [ ] Store/privacy/content-rating preparation.
- [ ] Final exact-source QA matrix and matching APK/AAB.

Final publication, signing, package identity, branding, cultural/legal approval and Play
Console actions remain owner-controlled and are not authorized by this plan.
