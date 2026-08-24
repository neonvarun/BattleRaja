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
- Replay completeness for all fighter abilities, production streams, mutable state,
  event identities and deterministic parity.
- Unified action eligibility across movement, attacks, abilities, gadgets, healing,
  knockback and Aandhi: **still open**.

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
