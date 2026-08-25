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

## Later checkpoints

- [ ] Fair fighter-specific bot AI and production match harness.
- [ ] Controlled reference-game UX study on Lava only.
- [ ] Final original art/audio/UI direction and provenance documents.
- [ ] Production fighters, arena, gadgets, rigs, animation and VFX.
- [ ] Coherent mobile UI/tutorial redesign and accessibility QA.
- [ ] Authored audio/music/mix and feedback.
- [ ] Feel/balance playtests and changelog evidence.
- [ ] Lava performance hardening against measured budgets.
- [ ] Current Android/Play compliance recheck.
- [ ] Store/privacy/content-rating preparation.
- [ ] Final exact-source QA matrix and matching APK/AAB.

Final publication, signing, package identity, branding, cultural/legal approval and Play
Console actions remain owner-controlled and are not authorized by this plan.
