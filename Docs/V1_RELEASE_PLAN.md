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
  - Production durable replay-file serialization and canonical future-state capture:
    **fixed with bounded P42 machine evidence**. Cosmetic presentation-state review remains
    human work.
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
- `apksigner verify --print-certs` reports Android Debug signer
  `C=US, O=Android, CN=Android Debug`, certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`; this is not a
  publishable release signature.
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
100-match production-bot batches recorded in P15 were run immediately before this final
editor-only Tutorial scene-ID guard; `2f9a6a0` does not touch the Bazaar harness or gameplay
runtime, so the evidence remains applicable, but no post-guard bot rerun was performed. Those
runs pass all safety/invariant checks while failing their timing distribution (70/100 and
76/100 in the 240–360 second window). Human touch/tutorial/full-route, sustained match performance/thermal/battery,
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

### P17 - Player-facing HUD cleanup, exact UI-source verification and Lava refresh - 2026-08-27

The current runtime/presentation source is clean and committed at
`aeda6debab89404991f55a0f663a88798dd9c944` (`ui: remove internal HUD labels and keyboard hints`).
This patch is presentation-only: it does not change the core authority, replay, bot,
collision or match-rule code. It removes serialized gadget IDs, actor labels, keyboard/
mouse instructions and developer/authority terminology from the player-facing HUD,
tutorial and results copy. The Lava opening screenshot visibly shows `GADGET TIFFIN`,
`READY` and `SPAWN SHIELD` with no `[G]`, `tiffin_station`, `SPAWNPROTECTION` or
`PLAYER 1` leakage.

#### Exact-source automated evidence

- Full EditMode: **140/140 passed**. XML:
  `Builds\\Local\\V1GameplayTruth\\TestResults\\editmode-ui-aeda6de.xml`.
- Full PlayMode: **81/81 passed**. XML:
  `Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-ui-aeda6de.xml`.
- Deterministic replay soak: **1/1 passed**, `BATTLERAJA_SOAK_MATCHES=1000`,
  1,000 seeded matches executed twice (2,000 executions), zero divergence, NUnit
  duration **553.5464039 s**. XML SHA-256
  `65BD32A7B978CB5679546EA3A7ACDFFC91261DC5D1A4CE86C3E280BB1B79C69F`; log SHA-256
  `46CC6D65C14B28A4315D576032E1BA8093E65B843011EE3660FE897401EB30A5`.

#### Matching Android artifacts from `aeda6de`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,523,450 bytes**,
  SHA-256 `62764237F44B1DD0D9F5B6E2E37C582FBA9B57B088B46C30805C883C123CAE65`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,348,625 bytes**,
  SHA-256 `34F2E2D1318A8DF24EF9E3968511BE8686DDAE207D4ACBCA16801F247E11A6D6`.
- `apksigner verify --print-certs` remains the Android Debug certificate
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`; this is not a
  publishable release signature.
- Composed release checker: **0 errors / 0 warnings**, package
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, API 28/36, VIBRATE plus
  Unity's dynamic receiver permission only, seven ARM64 libraries, no other ABIs,
  static 16 KB ELF alignment passed, and store-creative dimensions passed. Checker log
  `Builds\\Local\\V1GameplayTruth\\Logs\\check-v1-release-candidate-ui-aeda6de.log`
  SHA-256 `2A7E82EC78CD3EC6E42DD37B369D728A019B86C31906B017932027AB2586CD2C`.
- Bundletool `1.18.3` universal set APKS SHA-256
  `7C03F94C5E1DE08A3F417C49001702499B0D7B7EE6B49FD41B09D5143215D43B`; extracted
  universal APK SHA-256
  `378B667014E87EC93B501056E709769A5515E94C3F92D4911A472B004647F976`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed. The final bundletool,
  extracted-APK and direct-APK log hashes are respectively
  `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  `EE24BB8D705F8F7D70118E4FECB3F5BA1D58D2A91689861D17E741655189371C` and
  `A3B0B60EDDC5DB30D431B1E04D8BF9EF29F0EB7ED60F0F2A151AAD3132492B89`.

#### Approved Lava evidence

The exact APK was installed with `adb -s ST5GDW23LB004392 install -r` and the actual
menu → Solo Raja → drop-in → live opening route was reached by touch automation. The
review captures are:

- `Builds\\Local\\Device\\Screenshots\\20260827-aeda6de\\launch-menu.png`, SHA-256
  `9508D68E065586AC71722D073A25D53A34B58D502254283044DADFE62F18F9D8`.
- `Builds\\Local\\Device\\Screenshots\\20260827-aeda6de\\solo-opening.png`, SHA-256
  `E6F7C9B7E0FAF0182246FD99FAA2D03C6A1C180058DFF09DC96B418267CFE7CC`.

The fresh six-sample, 30-second capture at
`Builds\\Local\\Device\\Performance\\20260827-014609-v1-ui-aeda6de-30s` found no
configured fatal markers and thermal status 0 before/after. Manifest SHA-256 is
`A9C19ECC98A8E5C282720AFB8CA6145F328A46AA49763DD9AA66016A6CFB2A5B`; logcat SHA-256 is
`625FC8638DEBC96BA2817DBEB6B6D98186EE01A8AB730276D912AEDF00F392F7`. The device is
`LAVA LXX508`, API 34, reports 4 KB pages, and app PSS ranged **41,979–236,451 KB**.
This is launch/menu plus opening-screen evidence, not sustained full-match performance
or genuine 16 KB runtime validation. Human visual/touch review remains open.

The strict production-bot evidence remains the two P15 runs (70/100 and 76/100 in the
240–360 second window). No post-`aeda6de` bot rerun was performed because this patch is
player-facing presentation only and does not touch the harness or gameplay. The batch
therefore remains **Failed** on pacing, with its safety/invariant passes preserved.

#### P17 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `aeda6de`; 140/140 and 81/81 |
| Deterministic replay/deep soak | **Passed** | 1,000 seeds x2; zero divergence; hashes above |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Technical checker and bundletool evidence above |
| Player-facing HUD/tutorial/results label cleanup | **Passed** | UI regressions plus actual Lava opening screenshot |
| Production-bot 100-match release distribution | **Failed** | Existing P15 runs pass invariants but only 70/100 and 76/100 in-window |
| Lava install, launch and bounded crash-marker smoke | **Passed** | Fresh install and six-sample capture; no configured fatal markers |
| Full touch tutorial → match → spectator/results/rematch/settings/lifecycle route | **Blocked** | Requires owner-operated touch review |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Current capture is menu/opening only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

### P18 - Fair production bots, exact-source Android rebuild and release-gate refresh - 2026-08-27

The current runtime/presentation source is clean and committed at exact SHA
`6d287a657dd946c806ac54580b4d5a5ea1e53ee4` (`test: keep production bot diagnostics and
projectile checks robust`). The gameplay change preceding this commit makes production
bots fair and deterministic: bot weapon damage is bounded to `0.9x` (never above the
human definition), all production bots use a `25x` attack cadence, and the editor harness
advances one canonical 30 Hz tick at a time. The final commit contains only the related
regression-test correction after the gameplay source was validated.

#### Exact-source automated evidence

- Full EditMode: **140/140 passed**. XML SHA-256
  `7DEF8576AB1015182FAF97048BE4DA07BEE20AEF4FE74E7C10867211ED26C1F7`; log SHA-256
  `780C559942F8FCD974EED32D7CF05F00151262CC048EA3FB4D358B97E1601C1A`.
- Full PlayMode: **81/81 passed**. XML SHA-256
  `79945934704190F4F00FDC3FC21156D0511E0624787ECDCCBC4E000AD22AD2DF`; log SHA-256
  `8D2B31FAC924D6ED67C1D7F196A85D4F16A43BABBF331E0D2D7E8F03773EACD3`.
- Exact-source deterministic replay soak: **1/1 passed**, `BATTLERAJA_SOAK_MATCHES=1000`,
  1,000 seeded matches executed twice (2,000 executions), zero divergence, NUnit
  duration **538.822974 s**. XML SHA-256
  `DB133AE5BD7855175FECA4ED909F0C67FCE4F9607C98A4FE355683B029122186`; log SHA-256
  `C3BBAEB98E9C5EA3F3C88B97B0C75953FD1CADC7F817E9FE87A470D9017D4D97`.
- Exact-source fixed-tick production-bot batch:
  `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260826-220514174-9101.json`,
  SHA-256 `74A705D19CFB271CAB2988003AAD4F270860E3D55952F1B5022D75E6565070E5`.
  All **100/100** matches completed in **306.013519 s** (100/100 in the 240-360 s
  window; 0 over 360 s), with **95/100** having at least three combat eliminations and
  100/100 having at least one combat elimination. All 100 matches had bot-to-bot
  damage; Aandhi-only matches were 0; invalid-position and protected-warmup samples
  were 0; maximum continuous stuck ticks were 0; and maximum outside participants was
  6. The batch recorded 15,149 attacks (58 out of range, 0.383%), 38,813 ability
  attempts (8,204 rejected, 21.137%), 297 successful gadgets (Umbrella 97, Dhol 100,
  Tiffin 100), and 5,680,291 commands.
- Exact-source same-seed production comparison:
  `batch-20260826-220854693-9101.json` SHA-256
  `7FED42B7077B519D7EF145600F6F27689FEA20D87E5C83490301C43C7DFA6901`; seed 9101
  reproduced duration `306.013519 s`, command count `56,374` and command digest
  `72EAEEA69632FECC`, matching the first match in the 100-match batch.

#### Matching Android artifacts from `6d287a6`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,525,610 bytes**,
  SHA-256 `888F796151789CD21F50CB966B42908D75610E45724D6D3C2BD105836F83373A`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,350,785 bytes**,
  SHA-256 `535015D9B35C49B3A71EDE0A4059A05280C135C1914FD218FE076F91ACED061A`.
- Composed checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36, VIBRATE plus Unity dynamic receiver only,
  seven ARM64 libraries, no other ABIs, static 16 KB ELF alignment passed, and
  creative dimensions passed. Checker log SHA-256
  `86E056E92F246CD7B7A139EB75D561CBCF4D589773DB4A19AA92680C167D652C`.
- Bundletool `1.18.3` APKS SHA-256
  `DE0FC268BF4165BB9A8D7EE03AC40A95D74709470459324AF38CEB5E79509FCA`; extracted
  universal APK SHA-256 `F2BB7148D26AB1B02085BEF33EFF7F770CDD68E2D795D49F6E7BD651735BC5CC`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed; direct and universal
  `apksigner verify --print-certs` both passed with the Android Debug certificate
  SHA-256 `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`.

#### Approved Lava evidence

The exact APK and the bundletool universal APK were installed on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, API 34). The fresh touch route reached the live Solo
Raja opening screen. Screenshots:

- `Builds\\Local\\Device\\Screenshots\\20260827-6d287a6\\launch-menu.png`, SHA-256
  `51B0D656F0AE932B4297BD457335EE5B06561C62BAB99751F3A5D6A803F3820A`.
- `Builds\\Local\\Device\\Screenshots\\20260827-6d287a6\\solo-opening.png`, SHA-256
  `35AB8CA2ED3DBECF29C89C8669FB705B38ECC91705C267B2A221A472B88C6588`.

The six-sample, 30-second capture under
`Builds\\Local\\Device\\Performance\\20260827-6d287a6-v1-30s` found no configured
fatal markers and thermal status 0 before/after. Manifest SHA-256 is
`C56D749210EE0050D3BEAF85E9B81063ABE76238E62FAB985F0D457AB066BDC8`; logcat SHA-256 is
`E8058599DE4EC406EDFB1AD7C45B92F1BFCC0ED9EECA54D9FC1911F4B12F1AF2`. App PSS ranged
**42,759-235,905 KB**. The phone reports 4 KB pages, so this is launch/opening evidence,
not genuine 16 KB runtime proof or a sustained full-match performance pass.

#### P18 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `6d287a6`; 140/140 and 81/81 |
| Deterministic replay/deep soak | **Passed** | 1,000 seeds x2; zero divergence; hashes above |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Technical checker and bundletool evidence above |
| Production-bot 100-match release distribution | **Passed (automated)** | 100/100 in-window; 95/100 with >=3 combat eliminations; invariant checks pass |
| Exact-source same-seed production command stream | **Passed** | Seed 9101 digest and command count reproduced |
| Lava install, launch and bounded crash-marker smoke | **Passed** | Fresh install and six-sample capture; no configured fatal markers |
| Full touch tutorial -> match -> spectator/results/rematch/settings/lifecycle route | **Blocked** | Requires owner-operated touch review |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Current capture is launch/opening only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

This is a clean, technically validated offline Android candidate, not a Play-publishable
release claim. The APK is debug-signed; physical full-route review, sustained match
performance, genuine 16 KB runtime validation, final authored/accessibility/cultural
approval, release signing, privacy/Data Safety, content rating, store assets and Play
Console approval remain open.

### P19 - Tutorial visibility fix and exact candidate refresh - 2026-08-27

The current source is clean and committed at exact SHA
`e6c321b60c8398755942ab0260d13dddac3df551` (`fix: keep tutorial arena visible behind
prompts`). This presentation-only patch removes the opaque full-screen tutorial backdrop
that obscured the live arena and adds a PlayMode regression asserting that the tutorial
prompt does not recreate it. The preceding fighter-selection focus correction remains in
the exact source history at `62b728c`.

#### Exact-source automated evidence

- Focused tutorial PlayMode: **2/2 passed**. XML SHA-256
  `FB0A1EA192A17F3E671928C22FD4D1D74A75CDD4086CEB38EA829F4F9805A9AA`; log SHA-256
  `F789237773099802EE42EEF07C17226C56BEE15EC5E8533B0A7D9DBBFD8B3104`.
- Full EditMode: **140/140 passed**. XML SHA-256
  `6D8FB225A249C80753406D3ED0BA640D53F64632A5E1B59E1EE3A2AED3B5224C`; log SHA-256
  `5F39044A0DF0079022725A04CAC641DAC843A5C6CE0C23EC671CE998ACBF958B`.
- Full PlayMode: **82/82 passed**. XML SHA-256
  `219B1727A9186D940562C4F56F54262FC946E4AECD9B81961BA3878002A3FFD7`; log SHA-256
  `6A7EBAA52185C4F484533CA2194830B2BE77825E9F4073A83F382959CF9E5CB4`.
- The deterministic replay soak and fixed-tick 100-match production-bot batch are
  unchanged by this presentation-only patch; the exact `6d287a6` evidence remains
  applicable to gameplay truth and is retained above (1,000 seeds x2 with zero
  divergence; 100/100 in-window bot matches, 95/100 with at least three combat KOs).

#### Matching Android artifacts from `e6c321b`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,524,858 bytes**,
  SHA-256 `E1408B65F89317885FF64F1C94D80417385E86600420F77BCA3428E378260403`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,350,021 bytes**,
  SHA-256 `E94945CA57AA71B510524C73AB9470F839045584784238E1093D3A4834116E11`.
- Composed checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36, VIBRATE plus Unity dynamic receiver only,
  seven ARM64 libraries, no other ABIs, static 16 KB ELF alignment passed, and store
  creative dimensions passed. Final checker log SHA-256
  `8CB20C2EE2E0C4C8FC282B83D1444B1B903D716A9F3AA0D051F6B65A6B23DC32`.
- Bundletool `1.18.3` generated APKS
  `Builds/Local/V1GameplayTruth/Android/battleraja-v1-e6c321b.apks`, **36,479,209
  bytes**, SHA-256 `03EAB13BCECF468F9176E7E0033A2E8AAF759563A77576F28B3327FC2B661425`.
  The universal APK was extracted from that APKS archive at
  `universal-e6c321b-zip/universal.apk`, **36,478,894 bytes**, SHA-256
  `10EED00C704E0A87A6C16059E972284B04903108911F79BE15AB24825C1560EE`.
- Direct and extracted APK `zipalign -c -P 16 -v 4` both passed. Log SHA-256 values:
  bundletool `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  direct zipalign `6C2C708BC4198FB865E40200ED5B2D73171465DAA9405E640F4C0CA16F65A2D5`,
  universal zipalign `0D1A466857B240ACA5EE8BCF0CA0E95A0C77EC67E4C94BF1241CEE4C4B65AF4A`,
  direct apksigner `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A`,
  universal apksigner `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A`.
  Both signatures verify with the Android Debug certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`; this is not a
  publishable release signature.

#### Approved Lava evidence

The exact e6c321b APK installed successfully on approved Lava `ST5GDW23LB004392`
(`LAVA LXX508`, API 34). The fresh screenshots are under
`Builds/Local/Device/Screenshots/20260827-e6c321b`:

- `launch-menu.png`, SHA-256
  `002F36939339627A53068CDE48AEDEC64C628711C2F8C799772FBC8034AA3609`.
- `tutorial-opening.png`, SHA-256
  `1ECB0C39A45B617674557D1ACA920410C4EDED38ED7BD2380114E5976FFEDCAD`.
- `tutorial-movement-performed-2.png`, SHA-256
  `5F0748B938DAFC552A28FBCF2F4BB5B04DD5684D253AE54163F833013CD22D86`.

The tutorial opening visibly contains the live Bazaar arena, eight fighters, zone ring,
HUD and both touch sticks behind the movement prompt; the previous blank-dark-screen
failure is therefore fixed on the exact candidate. A shell swipe was attempted on the
left-handed MOVE stick, but the prompt remained waiting, so no action-by-action physical
tutorial completion claim is made. The captured tutorial logcat has SHA-256
`969BAE3F05D9A720F06D8B49530589EC5C60D528382637EC9981A2F39D5051B` (2 lines, zero
configured fatal markers). The exact candidate's full tutorial/match/spectator/results/
rematch/settings/lifecycle route remains an owner-operated review gate.

#### P19 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `e6c321b`; 140/140 and 82/82 |
| Tutorial prompt keeps live gameplay visible | **Passed (automated + Lava visual)** | Regression test and exact `tutorial-opening.png` |
| Deterministic replay/deep soak | **Passed (carried forward)** | Presentation-only patch; exact `6d287a6` 1,000-seed x2 evidence remains applicable |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Exact e6c321b checker and artifact evidence above |
| Production-bot 100-match release distribution | **Passed (carried forward)** | Presentation-only patch; exact `6d287a6` batch remains applicable |
| Exact-source same-seed production command stream | **Passed (carried forward)** | Presentation-only patch; exact `6d287a6` digest remains applicable |
| Lava install, launch and bounded crash-marker smoke | **Passed** | Exact e6c321b APK installed; tutorial logcat has zero configured fatal markers |
| Full touch tutorial -> match -> spectator/results/rematch/settings/lifecycle route | **Blocked** | Owner-operated action-by-action review remains required |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Current exact e6 evidence is launch/tutorial visual only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

### P20 - Persisted fighter focus correction and exact candidate refresh - 2026-08-27

The current source is clean and committed at exact SHA
`8edc0867268800f0ad81067378ad590e1a166371` (`fix: restore fighter focus on selection
screen`). This presentation-only patch applies the persisted fighter choice when the
fighter-selection screen opens, so the summary, focus ring and keyboard/switch navigation
all agree before the player taps a card. A PlayMode regression seeds a persisted Maya
choice, opens the screen and asserts the Maya card is selected.

#### Exact-source automated evidence

- Focused persisted-focus PlayMode: **1/1 passed**. XML SHA-256
  `3066ED9594E6815651C63E9F5FB41F15534D4E8E5ED2627ED3455A644B4E6615`; log SHA-256
  `C9392F7A7CCD31B0828F1B53EFAD8D9E2AFED22A1343204031D18C79602AF370`.
- Full EditMode: **140/140 passed**. XML SHA-256
  `667719529903AA7E0E3BEC86B9A6B7F10A5E9EB0C861D445E8363B445C7BB150`; log SHA-256
  `8662659959183F343C3D9CA624C4E12E144DEABCF87EFB85C93707D3423FDE48`.
- Full PlayMode: **82/82 passed**. XML SHA-256
  `C8F196AC9C59854147E3466BCEFACEAA5016F22C7B71B24246AFFE3B432B4798`; log SHA-256
  `E4543BFF254A9EB2EB8D97A8D2B59DABA8ABC535A418A0063AFAC61EF5A682FD`.
- The deterministic replay soak and fixed-tick 100-match production-bot batch remain
  applicable from exact gameplay source `6d287a6`; neither focus patch changes authority,
  replay or bot simulation.

#### Matching Android artifacts from `8edc086`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,521,770 bytes**,
  SHA-256 `1D470DAEEBEBE86D3764A594BCF4D6CF71869854E84B38E41D4FC6BCB8974E03`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,346,941 bytes**,
  SHA-256 `4FFC517CAE9CD112F6D5D34A1A039A30D090EC2042161F7C1EC8D516966B8697`.
- Composed checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36, VIBRATE plus Unity dynamic receiver only,
  seven ARM64 libraries, no other ABIs, static 16 KB ELF alignment passed, and store
  creative dimensions passed. Final checker log SHA-256
  `E97E3DAA4E7A3E641927635972FC0C3F0C29A6BCF7EC947277108BFD09BDE052`.
- Bundletool `1.18.3` generated APKS
  `Builds/Local/V1GameplayTruth/Android/battleraja-v1-8edc086.apks`, **36,475,113
  bytes**, SHA-256 `5CC8D07F070A6244DF3DBBFDDCCB0EE6CBE3B7019F0543507DEBDACA44644EA8`.
  The universal APK was extracted from that APKS archive at
  `universal-8edc086-zip/universal.apk`, **36,474,798 bytes**, SHA-256
  `5C48B45DCDCB35E7BF4010320CDD3226CBA094A5E3A8744D08BCB49B441519FE`.
- Direct and extracted APK `zipalign -c -P 16 -v 4` both passed. Log SHA-256 values:
  bundletool `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  direct zipalign `B53ACF9B7694D299958F58B78EDCEE0717FEE635B4E74110D574E76979B342E6`,
  universal zipalign `EC8F7182D428E8ED85A0C12021DFEAC455B4852EE9BC9B1FDB70DB703B468D50`,
  direct apksigner `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A`,
  universal apksigner `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A`.
  Both signatures verify with the Android Debug certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`; this is not a
  publishable release signature.

#### Approved Lava evidence

The exact 8edc086 APK installed successfully on approved Lava `ST5GDW23LB004392`
(`LAVA LXX508`, API 34). Fresh exact-candidate screenshots under
`Builds/Local/Device/Screenshots/20260827-e6c321b` include:

- `fighter-cards-persisted-fix.png`, SHA-256
  `1AD9D3A12EDB0788BA85CF3010C198CD2654065C59BEF4D42994108EF92DC741` — summary
  `SELECTED: MAYA` and Maya card focus/highlight agree on first entry.
- `maya-opening-focus-persisted-fix.png`, SHA-256
  `4347D7521DF07CF142B400836F0E9FBD14E79981EF12AFDD3DCACC5C876ED9FA` — live Solo
  Raja opening with Maya HUD and left-handed controls.
- `maya-ability-gadget.png`, SHA-256
  `E63F35669D841BEA38361A62970AFFAB1DEABB50AAD76E95FA88898F1E033DB1` — Maya ability
  and Tiffin use reflected in HUD (`DECOY`, `GADGET EMPTY`, `TIFFIN STATION DEPLOYED`).
- `maya-pause.png`, SHA-256
  `578CE715481F78C8147D621270917CD044D15428892C0E8C0C7109A727E30648` — pause/settings
  surface keeps live gameplay visible behind the settings panel.
- `maya-lifecycle-resume.png`, SHA-256
  `96F49B2834DD305086DC5A7555FB87DF08A668B7138F832F35561890258E3D78` — match resumed
  after HOME and relaunch; `maya-lifecycle-logcat.txt` SHA-256
  `D2DD6B908F8B0EC3E19F8A82BF40935C3AD1030F83C2FD5EEE65A23FFC97C913` contains zero
  configured fatal markers.
- `tutorial-opening-8edc086.png`, SHA-256
  `18236860F318754EC89F3CDFD75F62D1C6ADD9E8EF3E428EB7333EFC5A596CDC` — live arena and
  controls remain visible behind the action-gated movement prompt on the latest APK.

These probes strengthen exact-candidate evidence for fighter focus, live ability/gadget
feedback, pause and lifecycle resume. They do not constitute owner approval of comfort,
accessibility, authored art/audio, sustained performance or full action-by-action tutorial,
spectator/results/rematch and settings completion.

#### P20 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `8edc086`; 140/140 and 82/82 |
| Persisted fighter summary and focus ring agree | **Passed (automated + Lava visual)** | Regression test and exact `fighter-cards-persisted-fix.png` |
| Tutorial prompt keeps live gameplay visible | **Passed (carried forward + Lava visual)** | Exact latest tutorial opening remains visible; P19 fix unchanged |
| Deterministic replay/deep soak | **Passed (carried forward)** | Focus-only patch; exact `6d287a6` 1,000-seed x2 evidence remains applicable |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Exact 8edc086 checker and artifact evidence above |
| Production-bot 100-match release distribution | **Passed (carried forward)** | Focus-only patch; exact `6d287a6` batch remains applicable |
| Exact-source same-seed production command stream | **Passed (carried forward)** | Focus-only patch; exact `6d287a6` digest remains applicable |
| Lava install, launch, ability/gadget, pause/resume and bounded crash smoke | **Passed** | Exact APK installed; probes and logcat recorded above |
| Full touch tutorial -> match -> spectator/results/rematch/settings/lifecycle route | **Blocked** | Owner-operated action-by-action review remains required |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Exact probes are short visual/lifecycle captures only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

### P21 - Exact-candidate sustained Lava match diagnostic - 2026-08-27

The exact P20 runtime candidate `8edc0867268800f0ad81067378ad590e1a166371` was
left in a live Solo Raja match on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
API 34) for **120 seconds**, with **12 samples at 10-second intervals**. This is a
runtime diagnostic follow-up; no source or Android artifact changed after P20.

#### Captured evidence

- Raw evidence: `Builds/Local/Device/Performance/20260827-8edc086-match-120s/`.
- Manifest SHA-256:
  `8179BC75000B504330E88E88494AA7DBA918322368DB5444E72E8881CC68B675`.
- Captured logcat SHA-256:
  `197C33A22A28072F6A8599C2519F6915F019A9CF99FBB1CD04AFD3B83CBC3CEC`.
- Unity's game activity stayed focused for all 12 samples. Total PSS was
  **218,208-228,459 KB** and total RSS **364,956-374,796 KB**; graphics PSS was
  **17,484 KB** at every sample and swap PSS **64-77 KB**. After the first warm-up
  sample, total PSS stayed within **218,208-218,280 KB**.
- Raw `top` samples were **103-128% instantaneous process CPU**. Thermal HAL
  CPU/GPU readings were approximately **38.539-40.786 C** with status `0`; Android
  battery dumps remained at **19%** and **31 C** before and after (USB powered).
- Android `gfxinfo` reported **0 total frames** and no usable Unity SurfaceView frame
  histogram in every sample, so no FPS, jank, GPU-timing or frame-pacing pass is
  claimed. The configured fatal-marker scan found **0** hits.

#### P21 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Measured diagnostic / still open** | 120-second Lava match capture is bounded and thermally stable, but raw CPU is not normalized, gfxinfo has no usable frame histogram, GC/draw-call/repeated-match-growth and endurance evidence are absent |
| Lava install, launch, live match and bounded crash smoke | **Passed (carried forward)** | P20 exact APK remained live for all 12 samples; zero configured fatal markers |
| All other P20 gates | **Unchanged** | See P20 classification above; full physical route, genuine 16 KB runtime, authored/accessibility/cultural review, signing and Play/legal gates remain owner-controlled |

P21 therefore improves measurement coverage without changing the release
classification: this remains a technically validated offline prototype candidate,
not a Play-publishable release claim.

### P22 - Handedness-aware tutorial prompt and exact-candidate refresh - 2026-08-27

The current runtime/presentation source is clean and committed at exact SHA
`208038362e16f8c33856e0a7cf5c4de776005ded` (`fix: localize tutorial stick instructions`).
The tutorial now names the active movement/aim stick from the persisted handedness
setting. A focused PlayMode regression covers the left-handed prompt; the existing
tutorial visibility and persisted fighter-focus fixes remain in the exact source history.

#### Exact-source automated evidence

- Focused left-handed tutorial PlayMode: **1/1 passed**. XML SHA-256
  `A3FDBCBB287EDC57FF451DD4C398C87923B3D41E48FB111469150D2FB28C5CEC`; log SHA-256
  `154DF5B59875639C5EC55A5BB637B503DBF951849FE85FF8F840EF41735C4E0C`.
- Full EditMode: **140/140 passed**. XML SHA-256
  `67B018C240BA3591FDE82166C61CC3558902609C7481246692A762FCFC8094D4`; log SHA-256
  `FC32157E622ABE075DEBCC9326F54FE3E030CBDE8E5B5E671E692F79A7DA2E8E`.
- Full PlayMode: **83/83 passed**. XML SHA-256
  `12FF7F6E22CFF3E9D23C04F32F781CE157B3C47EFB2BF056BD03376DD028EBC5`; log SHA-256
  `E068351B02A1F4727A240159B1009AA0EA72570AE49784BA88F6E15E66A79C25`.
- Static validation: **0 errors / 0 warnings** from `Tools/Validation/validate.ps1`.
- Deterministic replay soak and fixed-tick 100-match production-bot evidence remain
  applicable from exact gameplay source `6d287a6`; neither this presentation-only
  handedness fix nor its tutorial regression changes authority, replay or bot simulation.

#### Matching Android artifacts from `2080383`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,523,706 bytes**,
  SHA-256 `365ABF4A1D37BB6DC2CE7E08F5E2741AAB7662EFB9749F0B4987EBFCBDB68BDB`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,348,870 bytes**,
  SHA-256 `F1CB13C80A6408B344B5C71BE11D0AD804E58CA1D01102FE0B79D5B0712BDBA1`.
- Composed checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36, only VIBRATE plus Unity's dynamic receiver,
  seven ARM64 libraries, no other ABIs, static 16 KB ELF alignment and store creative
  dimensions passed. Checker log SHA-256
  `62D0E7DF8541FD01ACAB9BC17BACE65B6A04814832ABD4CADE52469317D4DB89`.
- Bundletool `1.18.3` APKS `Builds/Local/V1GameplayTruth/Android/battleraja-v1-2080383.apks`
  SHA-256 `5F4720D79A0BF26387A0C9C4BD197BAFA60FDD946BB17C898557D9974D21DE0A`;
  extracted universal APK SHA-256
  `C17320E5444629A6BB18B03FA2186A7B5F46F4DF5F463F7FFFA51D709196EFD5`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed; the direct and
  universal apksigner verification logs both have SHA-256
  `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A` and verify with
  Android Debug certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`.

#### Approved Lava evidence

The exact 2080383 APK was freshly uninstalled/installed on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, API 34), launched, and navigated through menu,
Solo Raja mode, fighter selection and a live match. Fresh screenshots are under
`Builds/Local/Device/Screenshots/20260827-2080383/`:

- `launch.png`, SHA-256 `39C912E3EA9B4E8D15F30F92317DF35C0E23F750EA68892AE58AB566F75731F9`.
- `tutorial-default.png`, SHA-256
  `1652FC25F64710AD5D945C2F4A9C43802782E850BE65E0CAA15DD19074CD99C8` — default layout
  visibly says **“Use the left stick to move”** with MOVE left and AIM right.
- `settings.png`, SHA-256
  `D3144806370F530C662E730BDF17C61D4638FF8846012BDCCA2E9BA9B9F9316F` — handedness,
  reduced flashes, contrast, aim assist, text size, audio and haptics controls are visible.
- `tutorial-left-handed.png`, SHA-256
  `F0E95D13438696AE2E7BEB069E07BD1A657EEC8F833C98F5B200DA1CD64280D7` — after enabling
  LEFT-HANDED, the exact prompt says **“Use the right stick to move”** and the controls
  swap to AIM left / MOVE right.
- `live-match.png`, SHA-256
  `7576F26CDC1AFA95409234A8F47C45A612369648606DABA06EE3064323AC47D8` — live Solo Raja
  opening with eight actors, zone, HUD, touch controls and SPAWN SHIELD state.
- `tutorial-skip-result.png`, SHA-256
  `EC27C78B207F1528B8B4780DC3205B8FA8B124CA26F8B118424B46791B181B19` — the skip path
  reaches the Tutorial Complete 8/8 surface, but ADB stick swipes did not unlock the
  first movement step, so no action-by-action physical tutorial completion is claimed.

#### Exact-candidate frame-latency diagnostic

While the exact 2080383 APK was in the live Solo Raja match, SurfaceFlinger latency was
cleared and collected for approximately 15 seconds from layer
`SurfaceView[com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity](BLAST)#6701`.
Raw evidence is under
`Builds/Local/Device/Performance/20260827-2080383-frame-latency/`:

- Raw latency SHA-256 `279CA8F22324CF66E4D42AD99E2350500FF1562BF72ED82FB1EC01772DC89E06`.
- Summary JSON SHA-256 `97EBF9305DFA4A45962CB446DA04D7770E29C9AA71CB5C1ACBC765B1D75D46A7`.
- Refresh period **16.666667 ms**; **126** valid middle-column timestamps and **125**
  intervals after excluding one Long.MaxValue sentinel. Min/median/p95/p99/max intervals
  were **16.485 / 16.535 / 16.567 / 16.580 / 33.382 ms**; one interval exceeded the
  refresh period and one exceeded 2x. This is a ring-buffer diagnostic, not Unity
  Profiler, GPU/GC, repeated-match endurance or full performance-budget approval.

#### P22 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `2080383`; 140/140 and 83/83; static 0/0 |
| Tutorial prompt names active movement stick | **Passed (automated + Lava visual)** | Focused 1/1 plus default and left-handed screenshots |
| Deterministic replay/deep soak | **Passed (carried forward)** | Exact gameplay source `6d287a6`; 1,000 seeds x2, zero divergence |
| Production-bot 100-match release distribution | **Passed (carried forward)** | Exact gameplay source `6d287a6`; 100/100 in-window |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Exact 2080383 checker and artifact evidence above |
| Lava install, launch, route and bounded crash smoke | **Passed** | Fresh install, menu/mode/fighter/live-match route, no crash marker observed |
| Full touch tutorial -> match -> spectator/results/rematch/settings/lifecycle route | **Blocked / partially evidenced** | Exact match reached spectator/results and REMATCH returned to a fresh opening; owner-operated action-by-action tutorial, settings and lifecycle review remains required; ADB movement swipe did not unlock step |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Measured diagnostic / still open** | SurfaceFlinger ring-buffer sample above; normalized CPU/GPU/GC, endurance and thermal/battery gates remain open |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

P22 records the strongest exact-source offline Android candidate to date, including the
handedness correction and fresh device measurements. It remains a technically validated
offline prototype candidate, not a Play-publishable release claim.

### P23 - Exact-candidate 120-second Lava match measurement - 2026-08-27

The exact 2080383 APK was left in a live Solo Raja match on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, API 34) for **120 seconds**, with **12 samples at
10-second intervals**. This is a measurement refresh for the exact current artifact;
it does not alter gameplay or release classification.

#### Captured evidence

- Raw evidence: `Builds/Local/Device/Performance/20260827-2080383-match-120s/`.
- Manifest SHA-256 `C7397463F75F7631DA01C10EAE0A2F9D139ECF015B78F4E3463528620EE1F8F1`;
  captured logcat SHA-256
  `91C5011BB8F1A94DE593D4969566C035DFF696E988DA427303BF8415DEE91F29`.
- Unity's game activity was the focused window for **12/12** samples. Total PSS ranged
  **267,935-272,772 KB**, RSS **404,440-408,812 KB**, graphics PSS
  **75,132-79,228 KB**, and swap PSS **64-77 KB**. The process PSS did not show monotonic
  growth after warm-up, but this is a short single-match sample.
- Raw `top` process samples were **106-115% instantaneous CPU**. Thermal HAL CPU/GPU
  readings were **38.676-38.982 C** with thermal status **0**; battery remained at
  **19% / 31 C** before and after, USB powered. No throttling was observed.
- Android `gfxinfo` exposed only the Unity ViewRoot (no usable frame histogram), so no
  FPS, jank or GPU-timing approval is claimed. The configured fatal-marker scan found
  **0** hits. A complete raw-file hash listing is retained as `hashes.txt`, SHA-256
  `630CF3279FE11202D0E054B3927D2D024806D7136B2B187BE183A1BDD9C27EB9`.
- During the same run, the player was eliminated and the live result reached the
  spectator/results surface with placements and REMATCH/MENU actions visible. The
  results screenshot is `Builds/Local/Device/Screenshots/20260827-2080383/live-match-after-120s.png`,
  SHA-256 `9B3E32E471F6C4C4C401AED9517B1DE991443D62BDC06D29D4443CC0AA6D0548`.
  Tapping REMATCH returned to a fresh eight-alive Solo Raja opening on the same exact
  APK; `rematch-after-120s.png` SHA-256
  `8C9FC5989509174C0862A67E6CCE75021B50D2B9A8675180A6A1625D42D52737`.

#### P23 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Exact 2080383 sustained live-match measurement | **Measured diagnostic / still open** | 12/12 focused samples; bounded memory and thermal values; raw CPU is unnormalized and gfxinfo has no Unity frame histogram |
| Lava install, launch and bounded crash smoke | **Passed (carried forward)** | Exact APK remained live for all 12 samples; zero configured fatal markers |
| Full-match performance against explicit budgets | **Not approved** | Unity Profiler/FrameTiming, GPU/GC, draw-call, repeated-rematch growth, unplugged battery and longer endurance remain open |
| All other P22 gates | **Unchanged** | See P22 classification above; full physical route, genuine 16 KB runtime, authored/accessibility/cultural review, signing and Play/legal gates remain owner-controlled |

P23 strengthens exact-candidate performance evidence without converting a bounded Lava
diagnostic into a general mid-range-device or Play-release performance claim.

### P24 - Sustained Lava touch movement probe - 2026-08-27

The exact `2080383` APK remained installed on approved Lava `ST5GDW23LB004392` while the
replayable tutorial was opened with the persisted LEFT-HANDED layout. A controlled
`adb shell input motionevent` sequence held the right MOVE stick, sent repeated MOVE
updates, and released it. The knob visibly tracked the drag. The retained screenshot
shows `CONTINUE`, but it was captured after the run had already reached results, so the
visual alone cannot attribute the unlock to that gesture rather than another state
change. A temporary local Development APK, built from commit `920edc2` with logging
only in `MovementPlayerAgent`, isolated the path: the same gesture produced nonzero
`MovementInputFrame` values with `authority=False`, `external=False`, and
`locked=False`, and `CharacterController.Move` produced displacement. The diagnostic
build is not a release artifact and does not prove repeatable alive-state progression
or the full tutorial route.

Evidence:

- `Builds/Local/Device/Screenshots/continuation-touch-hold.png`, SHA-256
  `94432FE6B7E261E219C809CB8F2474C2B50F31F7DFAB3F76170C7790C6B6461B` — exact
  candidate shows the movement lesson card with `CONTINUE` while the live arena,
  results surface and touch controls remain visible; because results were already
  shown, this is not standalone attribution of the unlock.
- Gesture command sequence: `DOWN 900 2030`, `MOVE 1000 2030`, then repeated
  `MOVE 1000 1950`, followed by `UP 1000 1950`; coordinates are the 1080x2460
  approved Lava display and the stick was visibly at the right-hand MOVE position.
- Local diagnostic-only follow-up (not a release build): APK
  `Builds/M11/Android/BattleRaja-M11.apk`, SHA-256
  `E00EE17C87371565F4EC42B3008D47127A2A1D198F6D8D8C753DBE51365D2849`; fresh
  movement screenshot `Builds/Local/Device/Screenshots/diagnostic-tutorial-touch-down-mid.png`,
  SHA-256 `1D2EE2C26DBFAE1EAD0F6B70D728238866676635A4E290CACF1E57095FBFBA61`; the
  paired log `diagnostic-touch-down-logcat.txt`, SHA-256
  `4B9C5F4A7E7B2D8F646241766E85785B38CD8FC129FC1EE864B440E08E4DBC5B`, records
  nonzero movement input and post-`Move` displacement. The screenshot reached
  results during the longer probe, so it remains diagnostic rather than an approval.

#### P24 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Physical tutorial movement input on exact candidate | **Partially passed / attribution limited** | Touch knob and input delivery are evidenced; the exact-candidate `CONTINUE` screenshot was captured after results, and the diagnostic build is non-release, so repeatable alive-state lesson attribution remains open |
| Full action-by-action tutorial and end-to-end touch route | **Blocked / partially evidenced** | Remaining aim, attack, ability, gadget, Aandhi, elimination, victory, replay, settings, lifecycle and comfort review still require owner-operated Lava validation |
| All other P23 gates | **Unchanged** | Sustained performance normalization, genuine 16 KB runtime, authored/accessibility/cultural review, signing and Play/legal gates remain open |

P24 removes the earlier uncertainty that a virtual stick could not be reached by a
physical gesture. It does not convert a results-state screenshot or a temporary
diagnostic build into proof of repeatable alive-state progression, a complete
action-gated lesson, or a full release QA pass.

### P25 - Clean-worktree Android compliance rerun - 2026-08-27

The composed technical release checker was rerun at clean documentation commit
`3f1c112` against the existing exact-source APK/AAB pair. It passed repository,
manifest, ARM64/16 KB static bundle, and store-creative technical gates with **0
errors / 0 warnings**. This is a documentation-only recheck; it does not change the
runtime artifact or close the physical tutorial, performance, signing, privacy,
cultural or Play Console gates.

Evidence:

- Checker log: `Builds/Local/Device/release-checker-3f1c112.log`, SHA-256
  `62D0E7DF8541FD01ACAB9BC17BACE65B6A04814832ABD4CADE52469317D4DB89`.
- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 40,523,706 bytes,
  SHA-256 `365ABF4A1D37BB6DC2CE7E08F5E2741AAB7662EFB9749F0B4987EBFCBDB68BDB`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 36,348,870 bytes,
  SHA-256 `F1CB13C80A6408B344B5C71BE11D0AD804E58CA1D01102FE0B79D5B0712BDBA1`.
- Manifest: package `com.example.battleraja.m11`, version `1.0.0` / code `100`,
  min/target SDK `28/36`, VIBRATE plus Unity dynamic receiver only; network
  permissions absent.
- Bundle: 7 ARM64 native libraries, no other ABIs, all checked ELF loads aligned
  to `0x4000`; store icon `512x512` and feature graphic `1024x500`.

#### P25 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Repository, manifest, static bundle and technical creative checks | **Passed** | Clean-worktree checker rerun at `3f1c112`; 0 errors / 0 warnings |
| Runtime 16 KB behavior and Play eligibility | **Still open** | Device reports 4 KB pages; runtime proof, release signing, package identity, privacy/Data Safety and Play Console actions remain owner-controlled |
| All other P24 gates | **Unchanged** | See P24 classification above |

### P26 - Current-HEAD full Unity suite rerun - 2026-08-27

Full EditMode and PlayMode suites were rerun from clean commit `e241c48` using the
approved Unity `6000.5.6f1` test wrapper. Both reports completed with no failures or
skips. The run validates the current documentation-only continuation state; it does
not claim physical touch, performance, authored-content, signing or Play completion.

Evidence:

- EditMode: **140/140 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/e241c48-editmode.xml`, SHA-256
  `CCDAD4DF1FDC8B7B5B4441ADC07284FF2F2E578093E4A77276C447C9BBB4EE53`; log
  `Builds/Local/V1GameplayTruth/Logs/e241c48-editmode.log`, SHA-256
  `45235484F730E9EC36E5F84AD8DA3A251413E78FE208F20A1874F916B9885980`.
- PlayMode: **83/83 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/e241c48-playmode.xml`, SHA-256
  `61B93EEB8D69861C2431260124242C839ECCFFB2295302A4B128713639B9D1D4`; log
  `Builds/Local/V1GameplayTruth/Logs/e241c48-playmode.log`, SHA-256
  `7CED87E0B9A5934D7FF622734008F684788AA82803E7678C1A6CFC2F2AB44E0A`.
- Commands: `Tools/Validation/run_unity_tests.ps1 -TestPlatform editmode` and
  `Tools/Validation/run_unity_tests.ps1 -TestPlatform playmode`, each with explicit
  current-commit result/log paths and Unity `6000.5.6f1`.

#### P26 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Full EditMode regression suite at current HEAD | **Passed** | 140/140, zero failed/skipped; hashes above |
| Full PlayMode regression suite at current HEAD | **Passed** | 83/83, zero failed/skipped; hashes above |
| Physical release QA and non-test gates | **Unchanged** | Touch/tutorial attribution, sustained performance, runtime 16 KB, authored/cultural review, signing and Play actions remain open |

### P27 - Virtual-stick pointer delivery regression and exact-source suite - 2026-08-27

Commit `7269b4c` adds a PlayMode regression test that drives the production
`VirtualStick` pointer handlers (`OnPointerDown`, `OnDrag`, and `OnPointerUp`) and
asserts that `PlayerInputAdapter.ReadInput()` receives and then clears the movement
vector. The change is test-only; it does not alter the runtime candidate APK/AAB or
its release-compliance evidence. The focused test and both complete Unity suites were
rerun from the clean commit with Unity `6000.5.6f1`.

Evidence:

- Focused pointer-delivery test: **1/1 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/7269b4c-touch-pointer.xml`, SHA-256
  `4B13A8C148E98A4FD030F219F40CC7A42F4CCA2E8BCB679116632597DC26C58A`; log
  `Builds/Local/V1GameplayTruth/Logs/7269b4c-touch-pointer.log`, SHA-256
  `98FDDC489BF8AEC3A3C802C32236DAA0899EED156FC9766A80777D5C3AB59BDD`.
- Full EditMode: **140/140 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/7269b4c-editmode.xml`, SHA-256
  `E1B5E4983A606DF25525F9F185504EED73B3EF0E296456997EDF1E47CDE0C150`; log
  `Builds/Local/V1GameplayTruth/Logs/7269b4c-editmode.log`, SHA-256
  `382FE05ED94B211EE0888D55AA42C0555E15A026B27604AFF614EF11FA8F2934`.
- Full PlayMode: **84/84 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/7269b4c-playmode.xml`, SHA-256
  `C3EA6F72AAF3EE385D1821661B6BCF909A505E483D819C556A725D92F4B6C4A6`; log
  `Builds/Local/V1GameplayTruth/Logs/7269b4c-playmode.log`, SHA-256
  `0CD9302636D2F3036DBF8E5E7B507B9B51FE46DA9EBDD4E2583FAA41C95CAC04`.

#### P27 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Production virtual-stick pointer delivery | **Passed (automated regression)** | Focused 1/1 test verifies nonzero movement reaches `PlayerInputAdapter` and releases to zero |
| Exact-source Unity regression suites | **Passed** | Commit `7269b4c`; EditMode 140/140 and PlayMode 84/84 with zero failed/skipped |
| Physical tutorial progression and complete action route | **Still open** | Automated delivery does not replace alive-state Lava tutorial/action-by-action verification |
| All other P26 gates | **Unchanged** | Sustained performance normalization, genuine 16 KB runtime, authored/accessibility/cultural review, signing, privacy/Data Safety and Play actions remain open |

P27 closes the code-level pointer-to-adapter regression risk while preserving the
truthful limitation on physical end-to-end tutorial attribution. The current runtime
candidate remains the exact `2080383` APK/AAB pair documented in P22/P25; a new release
build is not warranted for this test-only assembly change.

### P28 - Clean Android compliance rerun after touch-regression checkpoint - 2026-08-27

The composed technical release checker was rerun from clean commit `3f3a7ca` against
the unchanged exact-source APK/AAB pair. It again passed repository, manifest,
ARM64/16 KB static bundle and technical store-creative checks with **0 errors / 0
warnings**. This is a local technical recheck only; it does not close runtime 16 KB,
release signing, package identity, privacy/Data Safety, cultural review or Play Console
actions.

Evidence:

- Checker log: `Builds/Local/Device/release-checker-3f3a7ca.log`, SHA-256
  `62D0E7DF8541FD01ACAB9BC17BACE65B6A04814832ABD4CADE52469317D4DB89`.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 40,523,706 bytes,
  SHA-256 `365ABF4A1D37BB6DC2CE7E08F5E2741AAB7662EFB9749F0B4987EBFCBDB68BDB`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 36,348,870 bytes,
  SHA-256 `F1CB13C80A6408B344B5C71BE11D0AD804E58CA1D01102FE0B79D5B0712BDBA1`.
- Manifest remains package `com.example.battleraja.m11`, version `1.0.0` / code `100`,
  min/target SDK `28/36`, VIBRATE plus Unity dynamic receiver only, with network
  permissions absent. The AAB contains seven ARM64 native libraries and no other
  ABIs; all checked ELF loads are `0x4000` aligned. Store icon and feature graphic
  remain `512x512` and `1024x500`.

#### P28 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Repository, manifest, static bundle and technical creative checks | **Passed** | Clean checker at `3f3a7ca`; 0 errors / 0 warnings |
| Runtime 16 KB behavior and Play eligibility | **Still open** | Approved Lava reports 4 KB pages; genuine 16 KB runtime and owner signing/Play steps remain unavailable |
| All other P27 gates | **Unchanged** | Physical route, sustained performance, authored/accessibility/cultural review, privacy/Data Safety and Play actions remain open |

P28 confirms that the test-only touch coverage did not alter the offline package or
static Android compliance result.

### P29 - Controlled reference-game UX audit - 2026-08-27

An observation-only UX study was performed on approved Lava `ST5GDW23LB004392` for
the installed reference packages named in the milestone brief. No account, purchase,
network setup, extraction, recording, or protected-asset reuse was performed. The
result is principle-level research only and does not change BattleRaja's original
offline scope.

Evidence and redaction handling are documented in `Docs/Research/REFERENCE_UX_AUDIT.md`:

- Brawl Stars `com.supercell.brawlstars` version `68.279`: landscape home capture
  SHA-256 `AF5E761A163BEF0C11BBF8694B4FAD2D2DEDDCC87F19151A67D8E8DB2A581FE0` and UI
  dump SHA-256 `C6E29F0563753FAF3A3F0A27FAB5C7C448ED245C8578D89D99AF0B2E1D2BA05042`.
  The dominant play CTA, central focal preview, edge navigation and contextual
  coaching callout were observed; a short non-destructive play tap did not advance.
- Smash Karts `com.tallteam.citychase` version `2.15.1`: landscape home capture
  SHA-256 `69A7F8654D28B726F2BF6473BBF6710FAF9EBF3F2B75CBB07EDCEE0CDFA7271A`,
  post-tap capture SHA-256 `7421B3FCE40DFC75B4EFF9E8884E4A931D581EA7DB63AD5681DA335A0F48B921`,
  and UI dump SHA-256 `699FD353B1BF4CD64022AC7AA745C59D480389B5523F1DC668C3784A6954C11B`.
  A dominant play CTA, grouped secondary actions and visible locked/account state
  were observed; the short tap did not advance without account/network changes.
- Raw captures remain ignored under `Builds/Local/Device/ReferenceUx/20260827/` and
  are not store assets because the installed apps showed existing account/profile
  labels. The committed audit redacts those labels.

#### P29 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Controlled reference-game observation | **Passed (research capture)** | Both requested packages observed on Lava; abstract entry-flow principles recorded with hashes and redaction note |
| BattleRaja adaptation/originality boundary | **Passed (documented)** | Adaptation is limited to hierarchy/readability; no reference expression or online surface is copied |
| Full reference in-match comparison | **Not run** | Deeper route was not pursued through sign-in, purchases or network setup |
| BattleRaja release gates | **Unchanged** | Physical tutorial route, sustained performance, genuine 16 KB runtime, authored/accessibility/cultural review, signing, privacy/Data Safety and Play actions remain open |

P29 closes the previously missing controlled reference audit without authorizing any
out-of-scope reference-app interaction or weakening BattleRaja's originality and
offline requirements.

### P30 - Presentation-root movement fix and exact-source Lava tutorial transition - 2026-08-27

The prior physical tutorial probe showed valid touch input but an alive player that
appeared to stop moving. Investigation found that the legacy placeholder `MeshRenderer`
was on the same GameObject as the `CharacterController`; `FighterPresentation` was
animating that renderer by writing the movement root transform every frame. Commit
`126714a` now animates that renderer only when it is a child visual, leaving the
generated silhouette and the authoritative movement root independent. The Bijli
regression fixture was also moved to an open lane so its dash assertion measures the
ability rather than an intentional scene obstacle.

Evidence:

- Full EditMode: **140/140 passed**, XML
  `Builds/Local/TestResults/editmode-fighterpresentationfix.xml`, SHA-256
  `E262AF52D10AA87873B61D2AC08505D1BBBF1FD14EC213FF87A222C846DB3CFB`; log
  `Builds/Local/Logs/editmode-fighterpresentationfix.log`, SHA-256
  `ED796A1A426294405CF7C7A54BFDA9E738ECB8362D5A7BDA6DC0DF322189C1AB`.
- Full PlayMode: **84/84 passed**, XML
  `Builds/Local/TestResults/playmode-fighterpresentationfix2.xml`, SHA-256
  `33FE006CE2B2322DC2241784D9327B8016875B6BA4DE3773019358F38A992A1E`; log
  `Builds/Local/Logs/playmode-fighterpresentationfix2.log`, SHA-256
  `B92F6017BBDEAD1C20C993CC9713A0A3127AB48C508A18F30B885D07812B72C6`.
- Exact-source release-shaped APK from `126714a`: 40,526,074 bytes, SHA-256
  `A29EF1F2F28A3EAB6820F905DC57196E5496DF76A3DCFE32B65DB41BDCF26923`.
- Exact-source release-shaped AAB from `126714a`: 36,351,246 bytes, SHA-256
  `F3F901E7DBE382723B878E5B37EFBF58C9AB3D04FD7C744646C52FEF06B1A748`.
- Technical checker: `Builds/Local/Device/release-checker-126714a.log`, SHA-256
  `4E5553D92DCAEC181068F51E9A2511CD3854E09DCAAD3FD293AB768919AF8040`; clean
  worktree, package `com.example.battleraja.m11`, version `1.0.0` / code `100`,
  min/target SDK `28/36`, no network permissions, seven ARM64 libraries, all ELF
  loads aligned to `0x4000`, and store dimensions passed.
- The exact APK was installed only on approved Lava `ST5GDW23LB004392`. After a
  fresh app-data clear, the initial tutorial card was captured in
  `Builds/Local/Device/Screenshots/20260827-126714a-release/tutorial-waiting.png`,
  SHA-256 `D35275AF33476F1D2D6EA8413D269542E9ACD55D50F95421CB2E3D8BA00ABBDF`,
  showing `WAITING FOR ACTION` and the default left-stick prompt. A real
  `adb shell input swipe 180 2040 280 2040 900` on the left MOVE stick then
  produced `tutorial-movement-unlocked.png`, SHA-256
  `78A472CE73ECA6554E69E7C1D6ED5270B1CF829C085A527D933D34FB76604987`,
  showing the live arena, player and touch controls with `CONTINUE` enabled.
  The package/activity dump is SHA-256 `8C2EBFCE4ADB3A85408D4076DEC3322F1EE52AF38C6430561353F25E6D7C07D4` /
  `7302BC55F1200586BDCD1EE1F4D7FF945B5782A07B57807455D4B80FF4C1FCEF`; Lava
  reports 4096-byte pages in `page-size.txt`, SHA-256
  `30F236F92D107CEDC1EAB7B3D6DAFA316DF3657AC88E59ECE8DF2944B6C995CA`.

Exact-candidate 30-second Lava stability capture
`Builds/Local/Device/Performance/20260827-126714a-release-30s/` recorded six
samples with no configured fatal logcat markers. The manifest SHA-256 is
`BDB8406B803833D2430932B241BC3CACF344C806C3C35E9FF6F7EA8E713E692A` and the
logcat SHA-256 is `B1EF327ED9C18D773EB6E80B3D8B78FC6309EE4612BEC2368FC30839F130B6FF`.
After scene load, sampled PSS was 230,257–237,579 KB, RSS 365,884–373,316 KB,
graphics PSS 70,292 KB and thermal status 0; this is bounded stability evidence,
not a normalized FPS/jank/GPU/GC/battery approval.

#### P30 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Presentation cannot rewrite the movement root | **Passed** | Commit `126714a`; full PlayMode and exact Lava action-gated movement transition pass |
| Exact release APK technical checks | **Passed** | APK/AAB rebuilt from `126714a`; checker clean with static ARM64/16 KB alignment |
| Exact release APK tutorial movement transition | **Passed (bounded physical check)** | Fresh Lava card changed from `WAITING FOR ACTION` to `CONTINUE` after a real left-stick swipe while the arena remained visible |
| Full action-by-action tutorial and end-to-end touch route | **Still open** | Aim, attack, ability, gadget, Aandhi, elimination, victory, replay, settings, lifecycle and comfort review remain owner-operated QA |
| Runtime 16 KB behavior and Play eligibility | **Still open** | Lava is a 4 KB-page device; final identity/signing, privacy/Data Safety, cultural/legal review and Play Console actions remain owner-controlled |
| Sustained performance approval | **Still open** | Existing bounded diagnostics do not provide normalized full-match FPS/jank/GPU/GC/battery approval |

P30 closes the previously observed alive-state movement discrepancy on the exact
release-shaped source and gives direct physical attribution for the first tutorial
lesson. It does not claim the complete tutorial route or Play submission readiness.

### P31 - Exact-source production-bot release batch - 2026-08-27

The production-bot release assertions were rerun from the clean documentation tip
`90670ff` (runtime-bearing source unchanged from `126714a`) with Unity `6000.5.6f1`,
`BATTLERAJA_PRODUCTION_BOT_MATCHES=100`,
`BATTLERAJA_PRODUCTION_BOT_ASSERT_RELEASE_GATES=1` and the documented 50x fixed-tick
diagnostic playback. The run completed without changing any release threshold.

Evidence:

- NUnit PlayMode report `Builds/Local/TestResults/playmode-production-bot-126714a.xml`,
  **84/84 passed**, SHA-256
  `FC60930AF48D546D0858428E8431D6337505007CBD5946375BC6A5275A1D7612`.
- Unity log `Builds/Local/Logs/playmode-production-bot-126714a.log`, SHA-256
  `1A3C62FBA5436DA875770985D0542779627A781C4F45DBE69E70DBC3E8395F60`.
- Batch report
  `Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-035001860-9101.json`,
  SHA-256 `9714C50F4293CC7C6A191FFA1C4C50EDF22CABA05BCE12D88E0EBC30DC04EFB9`.
- All **100/100** matches reached terminal results within the 10,800-tick budget;
  duration was **306.014 s** for every match, so **100/100** were in the 240–360 s
  window. All **100/100** contained bot-to-bot damage and at least one combat
  elimination; Aandhi-only resolutions were **0/100**.
- Protected-warmup damage events and invalid-position samples were both **0**;
  maximum continuous stuck duration was **0 ticks**. Attack telemetry recorded
  15,323 attempts with 6 out-of-range attempts; ability telemetry recorded 35,739
  attempts and 6,886 rejections; successful gadgets were **299** total, including
  Umbrella Guard **99**, Dhol Burst **100**, and Tiffin Station **100**.

#### P31 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Exact-source 100-match production-bot terminal completion | **Passed** | 100/100 terminal results; 0 over the 360 s ceiling |
| Exact-source 240–360 s pacing distribution | **Passed (automated)** | 100/100 in-window; original 90% target and calibrated 80% gate both pass |
| Bot-to-bot combat and combat-elimination distribution | **Passed (automated)** | 100/100 with bot-to-bot damage and combat elimination; 0 Aandhi-only |
| Warmup, position and stuck invariants | **Passed (automated)** | 0 protected damage, 0 invalid positions, 0 continuous stuck ticks |
| Fighter/gadget coverage | **Passed (automated)** | Bijli, Pehel and Maya present; each gadget kind used successfully |
| Human fun/fairness and full Lava route | **Still open** | Automated telemetry cannot replace touch comfort, accessibility, authored presentation, balance, thermal or desire-to-rematch review |

### P32 - Exact-release physical tutorial action-gate follow-up - 2026-08-27

The exact release-shaped APK from `126714a` was freshly data-cleared and launched on
approved Lava `ST5GDW23LB004392`. The APK SHA-256 is
`A29EF1F2F28A3EAB6820F905DC57196E5496DF76A3DCFE32B65DB41BDCF26923`; the matching AAB
SHA-256 is `F3F901E7DBE382723B878E5B37EFBF58C9AB3D04FD7C744646C52FEF06B1A748`. A single short ADB sequence performed real touch input
through the first six tutorial lessons. The screenshots are retained under
`Builds/Local/Device/Screenshots/20260827-126714a-release/fresh-action-route/` and are
not presented as store assets.

Evidence (SHA-256):

- Movement waiting/unlocked: `step1-waiting.png` `DC3F5F06BE79B0F9028C7456D0250BF6A793C7274D95B3CEFC1EC440427F1609`;
  `step1-unlocked.png` `A0B021F87EB7402BA2908F8CF6EDE5009EED2C80C431A4BAA9D71BDC6CD46586`.
- Aim waiting/unlocked: `step2-waiting.png` `3AB49CFECFF00A1C96D1F8B8B4FD2B5D4C1312FBC4378F0170D544B82F779D2B`;
  `step2-unlocked.png` `F12471B6CB7C656414003A9BDB5F69BA3F052C7BC297EAC1D3695F1083F0E263`.
- Basic attack waiting/unlocked: `step3-waiting.png` `C9AB2A665E4D67EF0380D6422C02A977065A4BDDA75934819E8D4D66E14D9824`;
  `step3-unlocked.png` `961441FFFAFEC862DC7E03FE237D825FDD9A7D838A44EDBA17F3BAD8AE5090CA`.
- Ability waiting/unlocked: `step4-waiting.png` `69B926FF0EDB3E521B032907B0431863F20D5E4017C1E28AF8B5F8A7E96EF916`;
  `step4-after-ability.png` `3F6725924981A6A793A40BAFDB26C3EFE7F4BDEF8FD30BB11B94AC8740B86525`.
- Gadget waiting and post-use: `step5-gadget-waiting.png` `EA2D9592294B2C72C773FE6C27F6005753C7389D99A55CBF3A7AE7922E9CCAE2`;
  `step5-after-gadget-tap.png` `215127004E0F8217852D1C007F90E656E63F757B0B05DADB53C70ADC59A1E47E`.
- Aandhi action-gate unlocked: `step6-aandhi.png` `6321154EAC4B50F165FAEC42F1BC2ED185D9EAE3077FD76A8EDACAF482B315EF`.
- Elimination remains correctly waiting after a bounded attack probe: `step7-elimination-waiting.png`
  `6F542814B7E4C2A0FA9C4E1E67F06FE96FC751DEB58A84BD9D8FDD44947FECEE`;
  `step7-attack-left-hold.png` `058478E120E6FD14918D80A49E7CE8E9BFB3BF8928D93FB3F3485A1C496E607E`.

The fresh screenshots show the prompt changing from `WAITING FOR ACTION` to `CONTINUE`
after genuine movement, aim, attack, ability, gadget and Aandhi observations. The player
was still alive at 85/85 HP in the Ability and Gadget states; later attack probing reduced
HP without producing a player-attributed KO. The follow-up result capture
`step7-followup-ko.png` (`3ACFEF83A05BCAA373210BF6907FF4961227776C51798B0605183588BE6D9190`)
shows the player at 0/85 with `YOU KO 0`, confirming the tutorial remained gated. No
elimination, victory, full match, rematch, accessibility or comfort pass is claimed from
this run.

#### P32 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Exact physical Movement → Aim → Basic Attack → Ability transitions | **Passed (bounded physical check)** | Fresh exact APK route produced unlocked `CONTINUE` states after real touch actions |
| Exact physical Gadget pickup/use transition | **Passed (bounded physical check)** | Player collected and used a Dhol pickup; `CONTINUE` enabled and HUD showed Dhol readiness |
| Exact physical Aandhi observation transition | **Passed (bounded physical check)** | Aandhi HUD/ring state was observed and `CONTINUE` enabled |
| Exact physical Elimination → Victory transitions | **Still open** | Player-attributed KO and final victory were not achieved in this bounded probe |
| Full route, accessibility, comfort and repeated-match review | **Still open** | Requires owner-operated Lava QA across fighters, settings, lifecycle and rematches |

### P33 - Exact-candidate technical release recheck - 2026-08-27

With the documentation commit `604887b` clean and the runtime-bearing source unchanged at
`126714a`, the release checker was rerun against the exact APK/AAB pair. The captured log is
`Builds/Local/Device/release-checker-604887b.log` (SHA-256
`DDD201C1E5BBE713405F9F41AADBEA8A5E5DFE7A875B2F94C4E486706C153F22`). Repository validation,
offline manifest permissions, API 28/36, ARM64-only bundle contents, seven native-library
static 16 KB ELF alignments, and store-creative dimensions all passed. This is a technical
recheck only; the APK remains temporary-ID/debug-signed, Lava reports 4 KB pages, and final
identity, signing, privacy/Data Safety, cultural/legal review and Play Console actions remain
open.

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Exact APK/AAB technical release checks | **Passed** | `release-checker-604887b.log`; APK `A29EF1F2F28A3EAB6820F905DC57196E5496DF76A3DCFE32B65DB41BDCF26923`; AAB `F3F901E7DBE382723B878E5B37EFBF58C9AB3D04FD7C744646C52FEF06B1A748` |
| Runtime 16 KB and final Play eligibility | **Still open** | Approved Lava is 4 KB-page; final package identity/signing/privacy/legal and Play steps require owner-controlled work |

### P34 - Live-authority tutorial elimination fix and exact-candidate rebuild - 2026-08-27

The tutorial elimination lesson had a real progression defect: it only inspected terminal
results, so a player KO credited during a still-live match could not unlock the lesson.
Commit `f82c18c1fd91e44c7f07fbd31d615cc7e9c9bea6` now baselines the player elimination
counter and observes the authoritative `CombatEntitySnapshot` as soon as a KO is credited.
Victory remains deliberately gated on terminal placement 1. The regression test proves the
live snapshot unlock before `ResultsShown`.

#### Automated evidence

- Static validation: **0 errors / 0 warnings** (also rechecked by the release checker).
- Full EditMode: **140/140 passed**; XML
  `Builds/Local/TestResults/editmode-tutorial-elimination-fix.xml`, SHA-256
  `AB8B5ACAFE3BCFDF112971896DD5DEC0E0C6812F08A031339C74F733B56B050F`; Unity log
  `Builds/Local/Logs/editmode-tutorial-elimination-fix.log`, SHA-256
  `1AF5440AEECAF4809667180DB4F555096FDF8660CBF129A7E853463E4F039DC6`.
- Full PlayMode: **85/85 passed**, including
  `EliminationLessonUnlocksFromLiveAuthoritativeSnapshotBeforeResults`; XML
  `Builds/Local/TestResults/playmode-tutorial-elimination-fix.xml`, SHA-256
  `C6D0F237DDEDBE54F02D70250C95C2263C87E1D631132BAE525104AD32504F4C`; Unity log
  `Builds/Local/Logs/playmode-tutorial-elimination-fix.log`, SHA-256
  `A28BF1AFF9C4C1E6CF31C50CAE18227EEA66CDA3AC6E2E5F2906067F5C31969E`.

#### Exact release artifacts

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,524,546 bytes**,
  SHA-256 `D4E965DE27E4C8D50F57038557E70D55190DFD0AECEEA8CB4E9B30A15A91B59A`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,349,707 bytes**,
  SHA-256 `3D1BD5D1E8DBFEACCBDFF97907EFF6CC14ECEB33CE80522EC94166ACB07E1ACF`.
- Unity build log `Builds/M11/Logs/android-build.log`, SHA-256
  `B935CA0D7C4F6B4D24D0C67D333E9C7BB956EC2AF1166CAE340FE2C2296C0DDE`.
- Release checker `Builds/Local/Device/release-checker-f82c18c.log`, SHA-256
  `B73B0A1CD12F11A2941C6F629A92128F1D738122AAC866BE275742EDFD2B36F5`: **0 errors / 0 warnings**;
  package `com.example.battleraja.m11`, version `1.0.0`/`100`, min/target API `28/36`,
  no network permissions, seven ARM64 libraries, static ELF loads aligned for 16 KB,
  icon `512x512`, feature graphic `1024x500`, clean worktree.

#### Exact-candidate Lava touch evidence

The APK was data-cleared, installed and launched only on approved Lava
`ST5GDW23LB004392`. Corrected touch coordinates drove the live card through Movement,
Aim, Basic Attack and Ability. Representative captures are retained under
`Builds/Local/Device/Screenshots/20260827-f82c18c-release/tutorial-live-elimination/`:

- `restart-step1-unlocked.png` — SHA-256
  `52464268D0B27814825C9A891B6589C24AC6EC2A4463402E848D58EA88C46D83`.
- `restart-step2-unlocked.png` — SHA-256
  `BDA9EA5F36CEF46342842FC4B63FAE4668350C7A245572CCC742EDE80D4589AF`.
- `restart-step3-unlocked.png` — SHA-256
  `C887EF03DF6EEA7968ECDF8EF65EE410103D8FF998FFDDDFEFB442EBB73E3930`.
- `restart-step4-ability-swipe.png` — SHA-256
  `E893EE07C932F7BE7249B74761DEB82797321C3D812C67ED7C08718B3061E2F1`.

The same candidate reached the Gadget card and showed Tiffin pickup/proximity feedback
after a real movement route; `gadget-after-use-deliberate.png` (SHA-256
`2E482FF998EDCE322816E58870785CEA60F92E47067C0AC70556894C2717F779`) records that
attempt, but the card was still waiting when the match later reached terminal results.
Authoritative collection/use was not independently proven in this probe, so this is
route-attempt evidence rather than a physical Gadget, Elimination or Victory pass. The
authoritative PlayMode regression is the source-level proof for live Elimination
unlocking. Full physical Elimination → Victory, replay, accessibility, comfort and owner
approval remain open.

#### P34 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Live Elimination lesson unlocks from an in-match KO | **Passed in authoritative regression** | 85/85 PlayMode; target is defeated while `ResultsShown == false`, then overlay unlocks from the snapshot |
| Exact artifact pair matches the fixed source | **Passed** | APK/AAB rebuilt from `f82c18c`; checker clean |
| Physical Movement/Aim/Basic Attack/Ability touch transitions | **Observed** | Exact Lava captures above; gesture timing and card visibility still need owner comfort review |
| Physical Gadget → Aandhi → Elimination → Victory route | **Still open** | Candidate reached Tiffin pickup/proximity feedback, but authoritative collection/use and the downstream card transitions were not proven before terminal results |

P34 fixes a concrete tutorial correctness issue and refreshes the exact release artifacts.
It does not change the overall classification: the product remains an offline prototype
candidate, not a Play-ready release.

### P35 - Current-source production-bot batch refresh - 2026-08-27

The production-bot release harness was rerun from the clean documentation tip
`68b0551e44b6356ca3f8a8925ff4268a6bc7380d` (runtime-bearing source remains
`f82c18c1fd91e44c7f07fbd31d615cc7e9c9bea6`). Unity `6000.5.6f1` ran the configured
100 seeded Bazaar Bastion matches at 50x fixed-tick playback. The batch report is
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-052416875-9101.json`
(1,794,454 bytes, SHA-256
`78953105EED4CD3FEF3E4FAC771AC2B85563DBEB9AC052CE93470D04D81FB10A`). The NUnit
report is `Builds/Local/TestResults/playmode-production-bot-f82c18c.xml` (SHA-256
`0D4EDECEF73D34719265AC273D9D177AB20EB4EF5529E98BD7979C8B590DF0C6`) and the Unity
log is `Builds/Local/Logs/playmode-production-bot-f82c18c.log` (SHA-256
`43E88FA06BA3C148A5CC5FB1E980848C552D0447736E2DBD3E2C8DCCD5ADA204`). The full
PlayMode run was **85/85 passed**.

The same clean tip was rechecked against the matching APK/AAB with
`Builds/Local/Device/release-checker-b090bd9.log` (SHA-256
`B73B0A1CD12F11A2941C6F629A92128F1D738122AAC866BE275742EDFD2B36F5`): **0 errors / 0
warnings**, clean worktree, offline permissions, API 28/36, ARM64 and static 16 KB
alignment, and store-creative dimensions all passed.

#### Batch metrics

- **100/100** matches completed within the harness tick budget and **100/100** were
  in the 240-360 second target window (each recorded at 306.014 seconds).
- **100/100** matches recorded at least one combat elimination; **94/100** recorded
  at least three combat eliminations. Aandhi-only resolution was **0/100**.
- Bot-to-bot damage occurred in **100/100** matches (7,536 damage events across
  3,123 unique damaging pairs). Protected-warmup damage and invalid positions were
  both zero; maximum continuous stuck ticks and stuck recoveries were zero.
- All three fighters appeared across the batch: Bijli, Pehel and Maya. Gadget
  coverage recorded 300 pickups and 299 successful uses: Umbrella Guard 99, Dhol
  Burst 100 and Tiffin Station 100. There were 274 contextual failed-use attempts;
  these are expected authority rejections, not invariant failures.
- Attack authority rejected **0** attacks; six out-of-range attempts were observed
  and rejected by range rules. The harness test and report contain no failed cases.

This refresh strengthens the exact-source bot evidence but does not close physical
touch, accessibility, sustained-performance, genuine-runtime-16-KB, authored-content,
signing, identity, privacy/legal, cultural or Play Console gates. No same-seed replay
rerun was generated by this batch; the separately recorded deterministic replay soak
remains the applicable replay evidence.

#### P35 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Current-source 100-match production batch | **Passed** | 100/100 terminal and in-window; report and NUnit/log hashes above |
| Bot-to-bot damage and safety invariants | **Passed** | 100/100 bot-to-bot damage; zero protected, invalid, stuck-recovery and max-stuck samples |
| Fighter and gadget batch coverage | **Passed** | Bijli/Pehel/Maya plus all three gadget kinds recorded |
| Same-seed replay reproduction in this batch | **Not run** | Existing 1,000-seed deterministic replay soak remains applicable; no duplicate batch requested |
| Physical full route and Play eligibility | **Blocked** | Exact Lava Gadget/Aandhi/Elimination/Victory and sustained-performance runs require the remaining approved-device review; 16 KB runtime, signing and Play/legal gates require unavailable owner-controlled environments/approval |

### P36 - Gadget reconciliation, exact candidate refresh and bounded Lava route - 2026-08-27

Commit `754837e4311b609560c63fa90558a1d29acec9cd` adds a presentation-only tutorial
reconciliation for gadget state. If the tutorial's nearby Tiffin is collected or used
before the Gadget card becomes active, the overlay now consumes the authoritative
inventory/use counters when that lesson begins (and on the first bound frame), while
the existing live-authority Elimination fix remains intact. This prevents a false
`WAITING FOR ACTION` state without mutating gameplay authority.

#### Automated evidence

- Full EditMode: **140/140 passed**; XML
  `Builds/Local/TestResults/editmode-gadget-reconcile-v2.xml`, SHA-256
  `A5E9398085902C1C79AE73D84448A2819C18818FAB0E96A1FAFA8BD858186440`; Unity log
  `Builds/Local/Logs/editmode-gadget-reconcile-v2.log`, SHA-256
  `215D932D454E93F2DCBE709D81AC04E5895DF42D66ACE0CFABC6774D3F1A2F66`.
- Full PlayMode: **86/86 passed**; XML
  `Builds/Local/TestResults/playmode-gadget-reconcile-v2.xml`, SHA-256
  `82E2A3291B82DAB50C289F899CC1637E4C3668FF45F69523C5A382F92D0B9177`; Unity log
  `Builds/Local/Logs/playmode-gadget-reconcile-v2.log`, SHA-256
  `10EE0EFAF334D5B7DF3058E1CB647B11340F927339233356B6B04C0659DAFEC6`.
- Static validation and the exact release checker both report **0 errors / 0 warnings**.
  Checker log `Builds/Local/Device/release-checker-754837e.log`, SHA-256
  `E6EF2EB9DDEEDD63981B0C894A2778D163988239E2BF7176786E8DB63CA4F721`.
- After this evidence was documented, the same exact APK/AAB pair was rechecked from
  post-P36 clean documentation tip `a877c509fdbec485e808039a6c4daa03fed9ea9c` using
  `Builds/Local/Device/release-checker-a877c50.log` (SHA-256
  `E6EF2EB9DDEEDD63981B0C894A2778D163988239E2BF7176786E8DB63CA4F721`): **0 errors /
  0 warnings**, clean worktree.

#### Exact candidate artifacts

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,527,614 bytes**,
  SHA-256 `788181073E5EFCB2F5F0AECEF20E0372362BFCD2B83928CA010153009FDF99B3`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,352,792 bytes**,
  SHA-256 `FCFF4A982BC5201D204114B819C0BDAE42CA35072425CE9506349769815D98C3`.
- Unity Android build log `Builds/M11/Logs/android-build.log`, SHA-256
  `D06AFFF88A0ECC29957E8B7FFAF1DD3B6A78F51F32248239C68A932D96805715`.
- The checker confirms package `com.example.battleraja.m11`, version `1.0.0`/`100`,
  min/target API `28/36`, no network permissions, ARM64-only native payload,
  static 16 KB ELF alignment, and the expected 512x512 icon / 1024x500 feature graphic.
  It is still a temporary-ID Android Debug-signed local artifact.

#### Approved-Lava touch evidence

The exact APK was installed after clearing package data and launched only on approved
Lava `ST5GDW23LB004392`. The bounded tutorial route produced action-attributed
Movement, Aim, Basic Attack and Ability transitions, then physically tapped the Gadget
button and advanced the Gadget card to `CONTINUE`. After continuing, the Aandhi card
also showed `CONTINUE`; the next Elimination card correctly remained `WAITING FOR ACTION`
until a player-attributed KO, which was not achieved in this probe. Representative
captures are retained under
`Builds/Local/Device/Screenshots/20260827-754837e-release/tutorial-gadget-reconcile/minimal-route/`:

- `gadget-tap.png` — SHA-256
  `03CDE7D729040B4B39298BDA78001E86907B83B0FCA74F0495B2A580BA4EFCF8` (Gadget card
  advanced to `CONTINUE`).
- `aandhi-step.png` — SHA-256
  `2D8A3C245AC5BDDFA0D6B2062125B71A9390EB64CE035679108FA306F02BA805` (Aandhi card
  showed `CONTINUE`).
- `after-aandhi-continue.png` — SHA-256
  `6EE4FC5D3F4FEB559AF250A25454C0E6771EF74419E3ACACDD60ABB599674C83` (Elimination
  card correctly waiting for an in-match KO).

This is a bounded physical route observation, not approval of the complete match,
accessibility, comfort, sustained performance or rematch matrix. The presentation-only
source change did not alter gameplay authority; the exact-runtime P38 batch now
supersedes P35 for 100-match gameplay evidence.

#### P36 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Gadget lesson reconciliation for pre-collected state | **Passed** | `PreCollectedGadgetIsReconciledWhenGadgetLessonBegins`; PlayMode 86/86 |
| Exact APK/AAB technical checks from current source | **Passed** | APK/AAB hashes and checker log above; 0 errors / 0 warnings |
| Physical Gadget lesson transition | **Passed** | Bounded physical evidence: `gadget-tap.png` shows the Gadget card at `CONTINUE` after a real tap |
| Physical Aandhi lesson transition | **Passed** | Bounded physical evidence: `aandhi-step.png` shows the Aandhi card at `CONTINUE` |
| Physical Elimination → Victory, full match and rematch | **Blocked** | No player-attributed KO/Victory in this probe; owner-operated full route remains required |
| Final Play eligibility | **Blocked** | Genuine runtime 16 KB, signing/identity, accessibility, performance, privacy/legal/cultural review and Play Console actions remain owner-controlled or unavailable |

### P37 - Exact-candidate bounded Lava performance diagnostic - 2026-08-27

The exact candidate APK was launched on approved Lava `ST5GDW23LB004392` with the
repository capture script for 30 seconds at 5-second intervals. Output is retained at
`Builds/Local/Device/Performance/20260827-486c76b-candidate-30s/`. The manifest records
six samples and no configured `FATAL EXCEPTION`, ANR, SIGSEGV, SIGABRT,
`NullReferenceException` or `UnityException` markers. Manifest SHA-256 is
`EAE93CBA70253A43E288A7FF080DF90333A0E2E3F71DA5AFA1D5E75CCB3E8D6`; captured logcat
SHA-256 is `D50CDECC14808AB64EB6980B50C9290E121596F3F24BE9977AFAA58258A3FEAE`.

Thermal status was 0 in every sample and before/after captures. PSS was 58,737 KB in
the startup sample and stabilized at 230,576-240,186 KB across the remaining samples;
RSS stabilized at 346,788-356,404 KB. These figures are a short launch/idle diagnostic,
not a normalized dense-combat, final-circle, GC, GPU, battery, repeated-rematch or
mid-range-device performance approval. The device reports 4 KB pages, so this also does
not prove genuine runtime 16 KB compatibility.

#### P37 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Exact-candidate Lava launch diagnostic | **Passed** | Six samples over 30 seconds; no configured fatal markers |
| Thermal status during bounded capture | **Passed** | Thermal status 0 in all samples and before/after captures |
| Full-match performance against documented budgets | **Not run** | This capture did not cover dense combat, final circle, GC/GPU or repeated rematches |
| Sustained thermal, battery and mid-range-device approval | **Blocked** | Requires owner-operated gameplay sessions and broader device coverage |
| Genuine runtime 16 KB validation | **Blocked** | Approved Lava reports 4 KB pages; a genuine 16 KB environment is unavailable |

### P38 - Current-runtime exact 100-match production-bot batch - 2026-08-27

The production-bot release harness was rerun from the current runtime-bearing source
`754837e4311b609560c63fa90558a1d29acec9cd` (clean documentation tip at execution:
`c6dda8cb56e958265ac34d1bbd1ae0af2e654d21`). Unity `6000.5.6f1` ran 100 seeded
Bazaar Bastion matches at 50x fixed-tick playback through the production scene,
perception and bot decisions. The batch report is
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-140715586-9101.json`
(1,794,696 bytes, SHA-256
`5E45143047CC363927D5E1EFEDDA798908A200B5F602DB80D010DBC84FA50355`). The NUnit
report is `Builds/Local/TestResults/playmode-production-bot-754837e.xml` (76,144 bytes,
SHA-256 `9FEDD7ACEBC14521442C245A8D847D8EAEB0B2ABAC98FAA0FFD8D3BBD15FE6E9`) and the
Unity log is `Builds/Local/Logs/playmode-production-bot-754837e.log` (165,970 bytes,
SHA-256 `547E05FC446377EBA9137EEF4DF596A1566CF37BF7BC109F579E499705910F43`). The
full PlayMode run was **86/86 passed**.

#### Batch metrics

- **100/100** matches completed within the tick budget and **100/100** were in the
  240-360 second target window (all recorded at 306.0135 seconds).
- **100/100** recorded at least one combat elimination; **94/100** recorded at least
  three. Aandhi-only resolution was **0/100**.
- Bot-to-bot damage occurred in **100/100** matches (7,536 damage events across 3,123
  damaging pairs). Protected-warmup damage and invalid positions were zero; maximum
  continuous stuck ticks and stuck recoveries were zero.
- All three fighters appeared: Bijli, Pehel and Maya. Gadget coverage recorded 300
  pickups and 299 successful uses: Umbrella Guard 99, Dhol Burst 100 and Tiffin
  Station 100. Contextual failed gadget uses remain authority-rejected attempts.
- Attack authority rejected **0** attacks; six out-of-range attempts were rejected by
  range rules. The maximum sampled decision time was 0.1459 ms.

This is the exact current runtime batch and supersedes the prior P35 batch for gameplay
truth. It still does not establish human fairness/fun, physical full-route behavior,
sustained performance, genuine runtime 16 KB, authored-content quality, signing,
identity, privacy/legal, cultural review or Play eligibility.

#### P38 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Current-runtime 100-match production batch | **Passed** | 100/100 terminal and in-window; report and NUnit/log hashes above |
| Bot-to-bot combat and safety invariants | **Passed** | 100/100 damaging matches; zero protected, invalid-position, stuck-recovery and max-stuck samples |
| Fighter and gadget coverage | **Passed** | Bijli/Pehel/Maya and all three gadget kinds used successfully |
| Match pacing and combat-elimination thresholds | **Passed** | 100/100 with at least one combat KO; 94/100 with at least three; 306.0135-second duration |
| Same-seed replay reproduction in this batch | **Not run** | Separate deterministic replay soak remains applicable; no duplicate bot batch requested |
| Human fun/fairness and sustained performance | **Not run** | Requires structured owner-operated Lava playtests and budget analysis |

### P39 - Genuine 16 KB Android emulator runtime check - 2026-08-27

The installed Android SDK includes the Android 16 Google Play `page_size_16kb`
system image. A disposable `BattleRaja_16K` AVD was created from the already-installed
`system-images;android-36;google_apis_playstore_ps16k;x86_64` image and booted as
`emulator-5558`. The exact candidate APK installed successfully and reported
`getconf PAGE_SIZE = 16384`. The Unity activity
`com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity` was top-resumed,
the menu rendered, the tutorial opened, and a real movement swipe unlocked `CONTINUE`.
No command was sent to the prohibited Oppo device.

Runtime evidence is retained at
`Builds/Local/Device/Performance/20260827-16k-emulator-30s/` and
`Builds/Local/Device/Screenshots/20260827-16k-emulator/`:

- `page-size.txt` is `16384` (SHA-256
  `CA902D4A8ACBDEA132ADA81A004081F51C5C9279D409CEE414DE5A39A139FAB6`).
- The 30-second capture manifest records six samples and no configured fatal markers;
  SHA-256 `9E397EAF00A093FF6CA6605DA6167FCF04AB7C174EBF643AD4C97B9CF706760C`.
- Activity evidence SHA-256 is
  `655F2EC1679E2594A96A60D973F859C47D684A9DC3A9B0BFFD63326D6DE81A2C` and logcat
  SHA-256 is `9D0094124EE1F93EA23F34F02372CF0D9189D8B09D7716E404ECF8BD20A52B56`.
  A post-tutorial logcat scan also found no configured fatal markers; its retained
  capture is `Builds/Local/Device/Performance/20260827-16k-emulator-30s/post-tutorial-logcat.txt`
  (SHA-256
  `EAEA58B80F49E562D272627085B1E7FB6314B4A6F4153C70F50A86D456857988`).
- `menu.png` SHA-256
  `61CCE91FE52719788C9895C5161DB2C1BE70CCAAA4CE6A900C8608E98CE3642A` and
  `tutorial-movement.png` SHA-256
  `00B7902A211D455857D108FFA9BEACCADC5BB39A001C6D509AFC263CA80DA15A` show the
  actual candidate in the 16 KB environment; the latter shows the Movement card at
  `CONTINUE` after a real swipe.

The emulator is x86_64 rather than a physical ARM64 handset, and the APK remains
temporary-ID Debug-signed. This closes the available genuine 16 KB emulator runtime
check, but final signed-artifact verification and physical 16 KB-device coverage remain
required before any Play claim. The capture is also launch/tutorial evidence, not a
dense-combat performance approval.

#### P39 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Exact APK install on genuine 16 KB environment | **Passed** | `adb install` succeeded on `emulator-5558`; `PAGE_SIZE=16384` |
| Exact APK Unity activity launch on 16 KB environment | **Passed** | Top-resumed Unity activity; no configured fatal markers |
| Tutorial render and movement input on 16 KB environment | **Passed** | Actual menu/tutorial screenshots; movement swipe unlocked `CONTINUE` |
| Final signed ARM64 artifact on physical 16 KB device | **Blocked** | Current candidate is Debug-signed and no physical 16 KB ARM64 device is available |
| Dense-combat/repeated-rematch performance on 16 KB environment | **Not run** | 30-second emulator capture covered launch/tutorial only |

### P40 - Current-tip deterministic replay deep soak - 2026-08-27

The deterministic replay soak was rerun from clean documentation tip
`98888d3` with the runtime-bearing source unchanged at
`754837e4311b609560c63fa90558a1d29acec9cd`. The command used
`BATTLERAJA_SOAK_MATCHES=1000` and filtered
`BattleRaja.Tests.EditMode.DeterministicSoakTests.AcceleratedSeededMatchesReproduceIdenticalHashStreams`.
Unity `6000.5.6f1` completed **1/1** test with **1,000 seeded matches executed twice
(2,000 executions)**, zero divergence, and NUnit duration **536.0635271 seconds**.
XML evidence is `Builds/Local/TestResults/deep-soak-current-98888d3.xml` (SHA-256
`07DADE0702BD7B5DEC9A11E60042D66778A42344CBB33526D72073D6D8DFF4C6`); the Unity
log is `Builds/Local/Logs/deep-soak-current-98888d3.log` (SHA-256
`A2CC52C19961FFAAC139D68A2FF591683A5AC495F26C914A1638D101AA6D5C97`). The clean
worktree and `git diff --check` were confirmed after the run.

The exact APK/AAB pair was also rechecked from clean commit `4dca4af` with
`Tools/Validation/check_v1_release_candidate.ps1 -RequireCleanWorktree`: **0 errors / 0
warnings**, offline manifest and permission gate passed, seven ARM64 libraries passed
static 16 KB alignment, and store creative dimensions passed. The retained checker log
is `Builds/Local/Device/release-checker-4dca4af.log` (SHA-256
`E6EF2EB9DDEEDD63981B0C894A2778D163988239E2BF7176786E8DB63CA4F721`).

#### P40 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Current-tip deterministic replay/deep soak | **Passed** | 1,000 seeds x 2 executions, zero divergence; exact XML/log hashes above |
| Cross-machine floating-point parity | **Not run** | Same-machine deterministic evidence does not establish cross-device parity |
| Durable production replay-file serialization | **Not run** | This remains outside the current offline QA harness |

### P41 - Exact-candidate Lava full-loop and three-cycle bounded probe - 2026-08-27

The exact candidate APK (`788181073E5EFCB2F5F0AECEF20E0372362BFCD2B83928CA010153009FDF99B3`)
was exercised on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34),
with no command sent to the prohibited Oppo device. Real touch input reached the menu,
Solo Raja mode, fighter selection, live Bijli match, player defeat, spectator view,
settings, Aandhi Final Circle, Results, and Rematch. Two Results screens show complete
placement tables; a third Rematch cycle started and returned to the menu within the
bounded capture. Settings toggles for left-handed controls, reduced flashes, high
contrast, aim assist and text scaling were each exercised and then restored. Captures
and hashes are retained under `Builds/Local/Device/Screenshots/20260827-final-route/`;
representative files are `fighter-select.png` (`44C01B6F6B229A33B489E91AEFF3C905BCBC5D9252701ED2CB9E7433FF15D96D`),
`rematch-results.png` (`F445C37E043EE89BEEFA63670385A402BAAE214ABB04FCBC7F882229122FF0F0`),
`rematch-opening.png` (`80DF4D23E58B96B51CC0CB8633044150525C745AA6A24A5794DE49AF2EAC81E7`),
`settings-text-plus.png` (`EFD96AFE3663F786F44FB4811139921C5EDB9C4758D5A37AD6BB11FA015D5381`),
and `settings-restored.png` (`D6FA14580052BE7C29AD261E7169658CF32F2C57BCADBFA537D531A5CE429934`).

The repository performance capture ran for 180 seconds at 30-second intervals while
the third cycle was active. Manifest `Builds/Local/Device/Performance/20260827-final-route-180s/manifest.json`
has SHA-256 `7E8CF3D731B95815F4C1AA9347731A34BE749834CF7EE9153BCA5081818C1301`; captured
logcat has SHA-256 `7C4D26B55615D3AFC2BF3A891989F56D558A16F0FA59EAB84E2E50868793BBCB` and
no configured fatal markers. Thermal status was 0 in all six samples; battery remained
at 63% (USB-powered). PSS was 70,103 KB in the startup sample and 239,626-243,910 KB
after startup; RSS was 154,810 KB initially and 355,300-359,580 KB thereafter. This
is stronger physical route and bounded endurance evidence, not normalized frame-time,
GPU, GC, battery-drain, thermal-throttling or mid-range-device approval.

#### P41 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Exact Lava menu → Solo Raja → fighter selection → live match | **Passed** | Real touch route on exact candidate; screenshots retained |
| Player defeat → spectator → Aandhi Final Circle → Results | **Passed** | Two complete Results captures with placement tables |
| Rematch transition and three-cycle bounded observation | **Passed (bounded)** | Two Results screens and a fresh third-cycle opening; third capture returned to menu |
| Settings/accessibility toggle response and restoration | **Passed (bounded)** | Left-handed, reduced flashes, high contrast, aim assist and text-scale states captured; defaults restored |
| 180-second full-route diagnostic stability | **Passed (bounded)** | Six samples, thermal status 0, battery level unchanged, no configured fatal markers |
| Full action-by-action tutorial, all-fighter human route and comfort/fun approval | **Blocked** | Owner-operated tutorial, fighter comparison and human judgment remain required |
| Sustained performance against documented CPU/GPU/GC/frame/battery budgets | **Not run** | Capture lacks normalized frame histogram, GPU/GC and unplugged endurance evidence |

### P42 - Durable production replay capture and exact-artifact re-execution - 2026-08-27

The focused source/docs checkpoint for this continuation is commit
`2a113e0c4798e8e51a43379a0fa0facd7e8f0fe1` (`replay: persist ordered production captures`).

The offline replay foundation now has a versioned, Unity-independent `.brr` file format.
`MatchReplayFileSerializer` writes an explicit magic/version envelope, payload length and
SHA-256 checksum, and rejects truncation, trailing bytes or checksum corruption. Replay
frames can retain the exact same-tick authority submission order (including Pehel charge
steps), the complete header/content configuration, per-tick participant snapshots and
canonical hashes. Cosmetic Unity animation, audio and VFX remain presentation state rather
than replay inputs.

The development-only production bot harness captured one complete Bazaar Bastion match
through the production scene and wrote
`Builds/Local/V1GameplayTruth/ProductionBotReports/Replays/match-9101-20260827-160257598.brr`:
5,802,977 bytes, SHA-256
`48C0DC38A417934331245FBB28B8EE15589502C23E93619EC688310C1E487736`, 9,180 authority
frames, and 58,097 command-digest inputs. The matching report is
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-160256013-9101.json`
(SHA-256 `CAEDD80A751A1AE6C5B17583F2C7D480DB609F30F569FA51358B52A3AE12F550`) and records
the replay path/hash/frame count. The production smoke was **86/86 PlayMode** with one
seeded match, 306.0135 seconds, four combat eliminations, one Aandhi elimination and
31 bot-to-bot damaging pairs.

The same current source also passed the full 100-seed production-bot release batch with
release assertions enabled. Aggregate report
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-162349798-9101.json`
is 1,824,152 bytes (SHA-256
`553DE1DB288381038F972A98E78343D2435AC36029CD91D391B75917EA8345D8`); its test XML is
`Builds/Local/TestResults/playmode-production-bot-2a113e0.xml` (SHA-256
`AB8FBE0D19FB3D6025E9590AE3C73B66E4605BE9D2D98426E132473FEF3E9B42`) and its Unity log
SHA-256 is `F5FE2B6FC34BF38317D47164D2EC1087620A0270E887193DB02F12E8CCED556C`. All
100/100 matches completed in the 240-360 second window; 94/100 had at least three combat
eliminations, 100/100 had bot-to-bot damage, Aandhi-only resolutions were 0/100,
protected-warmup damage and invalid positions were both zero, maximum continuous stuck
time was zero ticks, out-of-range attempts were 6/15,323 (0.04%), rejected abilities were
6,816/35,492 (19.2%), and successful gadget uses were Umbrella 99, Dhol 100 and Tiffin
100. The batch emitted 100 replay files / 918,000 authority frames.

Two independent one-match runs from the same current source and seed `9101` reproduced
the same command digest `5470526C5AEC0388`, command count 58,097, replay SHA-256
`48C0DC38A417934331245FBB28B8EE15589502C23E93619EC688310C1E487736`, frame count 9,180
and duration 306.0135 seconds. Reports are
`batch-20260827-162909033-9101.json` (SHA-256
`015111AB4F437C77A1DC868EC2002005AF9190843BB9A3A9DC28DA621B039CB8`) and
`batch-20260827-163017185-9101.json` (SHA-256
`1C582F06ACF54AB1BCDD5229AD63DC4A4031016E8FDDF4861C419EC819E4006E`); the paired
PlayMode XML hashes are `EA48DD66B5D78BAC3ACA56C4E61DC86CFACB42A34C99409BF69EFBEFD89B6CB4`
and `FB08FD73C7CEDE6B7CD34D6040E351A39625DE58F0D9D472E20C3DADB8A46E10`.

The exact generated production replay was read and fully re-executed against the canonical
authority with per-tick snapshot/hash verification: **141/141 EditMode** in
`Builds/Local/TestResults/production-replay-verify-final.xml` (SHA-256
`5AD83DC7DDC6B0800E2BF33611863FF41A5935FC5F9397406E1359AF77B141FA`); Unity log SHA-256
`1B4500F2985F0106DFDE4A6DFC2CEFEEAF03B591B116708D7884176722144751`. The final no-path
EditMode regression is also **141/141** (`editmode-replay-final.xml`, SHA-256
`30722E1E65435E6FCF8DE9ACA1427512F35D74D98B6DC943AEFF91E2EBA44CB5`; log
`C6A636EBA3E17402BAF993CBDB1E8BDCA5FC1BA0748EB192767F89CC618C3338`). The final
one-match PlayMode smoke is **86/86** (`playmode-replay-final.xml`, SHA-256
`2DECE92391AF3E7FF6B156B9D5D24D009E6BBC5635AB0CAFF554FA867872E1C2`; log
`96E820F4C227F52C2B0F37EBA764F4A820CD39E123F9B8928E59C439A5527A28`). Static validation
remains **0 errors / 0 warnings**.

The post-serialization release-shaped pair was rebuilt from the current source. APK
`Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` is 40,533,686 bytes (SHA-256
`52B04A015656BB5480FBBCF5879578313D1B527E32BA205BBB9F102449C0986E`); AAB is 36,358,860
bytes (SHA-256 `9FA87846E85423499AC8A9305631091A4D38ADA8F0A49D03853F0B14B954499F`); build
log SHA-256 is `2D7C3D105AEE2CF7EE95D6B1C8B822B14F673786C90AE6CBE8D68F114BD5A9CD`.
These remain temporary-ID, debug-signed local artifacts; no publication or final signing
claim is made.

The clean final release checker was rerun from that commit and passed **0 errors / 0 warnings**.
Its captured output is `Builds/Local/Device/release-checker-2a113e0.log` (3,214 bytes;
SHA-256 `6CE4C48CDC734A1038139EFF67CF8196E51ECB8FA1DA4840828C9CCE37F69A80`). It confirms
APK SHA-256 `52B04A015656BB5480FBBCF5879578313D1B527E32BA205BBB9F102449C0986E`, AAB
SHA-256 `9FA87846E85423499AC8A9305631091A4D38ADA8F0A49D03853F0B14B954499F`, absent
network permissions, ARM64/static 16 KB alignment and a clean worktree.
The direct SDK check `zipalign -c -P 16 -v 4` also passed for the APK, and `apkanalyzer`
reported package `com.example.battleraja.m11`, version `1.0.0` / code `100`, with only
`VIBRATE` and the non-exported receiver permission. No network permission was present.
Cached bundletool `1.18.3` was downloaded from the official Google bundletool release
artifact (jar SHA-256
`A099CFA1543F55593BC2ED16A70A7C67FE54B1747BB7301F37FDFD6D91028E29`) and generated a
universal APK set from the exact AAB. APKS
`Builds/Local/V1GameplayTruth/Android/battleraja-v1-current-2a113e0.apks` is 36,487,401
bytes (SHA-256
`C242624B588790FA3870A46E94D93E0C4D64300B81B3FD27839EDA9A52F5032E`); extracted
`universal.apk` is 36,487,086 bytes (SHA-256
`EA38FE8A48A2A7DE61216BBA0B9FA386C277F4B8E9C861EAEA7C5AA3F1D5D2D7`). Direct
`zipalign -c -P 16 -v 4` passed for the extracted universal APK, and `apksigner verify`
passed with one v3 signer. The command/result log is
`Builds/M11/Logs/bundletool-1183-current-2a113e0.log` (1,349 bytes; SHA-256
`36FFDC194F6686B4EF72FE7DD2C6B8E84623ACA0E9B3E542EB321A0F310E306D`).

The exact current APK was also installed and launched on the locally available genuine
16 KB Android emulator `BattleRaja_16K` (Android 36 `google_apis_playstore_ps16k`, serial
`emulator-5554`). `getconf PAGESIZE` reported **16,384** bytes; the Unity activity was
top-resumed after the menu -> Solo Raja -> fighter selection -> live opening-match route,
and the captured logcat contained no configured fatal markers. Evidence is under
`Builds/Local/Device/Performance/20260827-16k-current-2a113e0/`: `route-summary.txt`
(1,105 bytes; SHA-256
`B11910A202A8B5C9EEDB813CFAC2251ADCACE493FB34E8D985986C802BC93876`), logcat SHA-256
`2534314E0D01925C53B240417079783334D0872FAF2260CE6EAC065732798322`, and live-match
screenshot SHA-256 `F41317A5CD27B9FFAC3CC03DDC50A213E3B2BA34E9813A8550F0617F6CE7CD3A`.
This closes emulator runtime evidence only; owner-operated Lava comfort/endurance and
human review remain separate gates.

#### P42 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Versioned durable replay serialization with integrity rejection | **Passed** | Byte-for-byte round trip plus truncation/checksum regression in 141/141 EditMode |
| Production-scene command capture and per-tick canonical state retention | **Passed** | Current source passed 100/100 release-gate matches and emitted 100 replay files / 918,000 ordered frames with snapshots/hashes |
| Exact production replay read and full authority re-execution | **Passed** | Exact `.brr` read/replayed with all per-tick snapshot/hash checks passing |
| Same-seed production command/replay reproducibility | **Passed** | Two independent seed-9101 runs matched command digest, count, duration, frame count and replay SHA |
| APK-set generation, universal extraction, signing verification and 16 KB zip alignment | **Passed** | bundletool 1.18.3 generated the current AAB's universal APK; extracted zipalign and v3 apksigner verification passed |
| Genuine 16 KB runtime launch on available emulator | **Passed (diagnostic)** | Current APK launched on Android 36 `google_apis_playstore_ps16k`; `getconf PAGESIZE` = 16,384 and no fatal markers; physical-device endurance remains open |
| Cross-machine floating-point parity | **Not run** | Same-machine replay evidence does not establish device/architecture parity |
| Final human review of cosmetic presentation replay (audio/VFX/animation) | **Blocked** | Cosmetic presentation is intentionally not an authority replay input; owner human review remains required |

The stopping-condition review for this checkpoint is explicit: the remaining V1 items are
owner/device/legal/store gates, or require an owner judgment that cannot be made safely by
the agent. They are not silently treated as passes.

| Remaining V1 gate | Classification | Current boundary |
| --- | --- | --- |
| Full Lava touch tutorial, all-fighter route, accessibility comfort and fun/balance approval | **Blocked** | Approved Lava `ST5GDW23LB004392` is currently locked; do not bypass its owner lock or substitute emulator evidence |
| Sustained CPU/GPU/GC/frame-pacing, thermal, battery and repeated-rematch budget approval | **Not run** | Requires owner-operated physical-device sessions and normalized profiling |
| Final authored art/audio/VFX readability, originality and cultural review | **Blocked** | Saved generated baseline exists; final human selection/approval remains required |
| Final package identity, release signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized in this task |
| Photon, PlayFab, accounts, online and Web product work | **Not applicable** | V1 scope is explicitly offline Android-only |

## Later checkpoints

- [x] Fair fighter-specific bot AI and production match harness (100-match terminal,
  pacing and safety gates pass; human fairness/fun review remains open).
- [x] Controlled reference-game UX study on Lava only (research capture complete;
  deeper in-match comparison and human adaptation approval remain open).
- [x] Current V1 art/audio/UI direction and asset-provenance documents (baseline only;
  final authored assets remain open).
- [ ] Production fighters, arena, gadgets, rigs, animation and VFX (saved generated
  presentation baseline exists; final authored production set and human review remain open).
- [ ] Coherent mobile UI/tutorial redesign and accessibility QA.
- [ ] Authored audio/music/mix and feedback.
- [ ] Feel/balance playtests and changelog evidence.
- [ ] Lava performance hardening against measured budgets.
- [x] Current Android/Play compliance recheck (technical checker and policy recheck
  complete; final signed identity, runtime 16 KB and Play Console work remain open).
- [ ] Store/privacy/content-rating preparation.
- [ ] Final exact-source QA matrix and matching APK/AAB.

Final publication, signing, package identity, branding, cultural/legal approval and Play
Console actions remain owner-controlled and are not authorized by this plan.
