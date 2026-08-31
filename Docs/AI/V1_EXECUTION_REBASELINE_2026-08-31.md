# BattleRaja V1 execution rebaseline

**Captured:** 2026-09-01 00:03 IST
**Source checkout:** `C:\Projects\BattleRaja`
**Branch:** `codex/v1-playstore-release`
**HEAD:** `56313096d0ad8e2e23468d004eaa77d71ed3a233`
**Remote:** `origin/main` resolves to the same commit at capture time
**Unity:** `6000.5.6f1`
**Approved device:** Lava `ST5GDW23LB004392` (`LAVA_LXX508`, Android 14/API 34, 4 KB pages)
**Classification:** Prototype — Bastion Crown implementation checkpoint; release gates remain open

This is the current execution baseline for the Goal-mode prompt pack. It supersedes the
pre-implementation reading captured on 2026-08-31 at `3bed64e`. The latest commit contains
a real offline 4v4 Bastion Crown domain and production-scene adapter, but it is not evidence
of a polished, signed or Play-submittable game. The agent must re-run all relevant checks
because this document can become stale after any commit.

## Continuation evidence — 2026-09-01 01:30 IST

The working tree now includes the authority/replay hardening continuation and the original
menu-art replacement. The authoritative machine/device record is
`Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-01.md`.

- Static validation: **0 errors / 0 warnings**.
- EditMode: **155/155 passed**; PlayMode: **94/94 passed**.
- Bastion replay v2 soak: **2 seeds × 8,400 ticks**, zero combined-hash divergence after
  serialize/deserialize re-execution.
- Squad planner coverage: 32 seeds; contest **64**, escort **64**, defend **96**, collapse
  **64**, Aandhi-retreat **32**.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,510,440 bytes,
  SHA-256 `5F7438105FE450D6331CFEDEE1FAEEB87FB4F6677EB811A997A02CC8FD7C4AE9`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,335,957 bytes,
  SHA-256 `87C835570B62C4C3A79C156F94CB7E15C6AD31FCB50A0E8ADB0FDE6672DC4858`.
- Release checker: passed package `com.example.battleraja.m11`, version 1.0.0/100, min SDK
  28, target SDK 36, no network permissions, ARM64-only native libraries, static 16 KB
  alignment and store-creative dimensions. Identity remains temporary/debug.
- Approved Lava `ST5GDW23LB004392` was freshly installed and exercised through menu → Bastion
  briefing → fighter selection → live opening, control taps and settings. Clean 30-second
  telemetry: PSS 287,530–293,678 KB, RSS 426,940–433,088 KB, graphics PSS 87,024–93,180 KB,
  CPU 111–118%, thermal status 0, USB-powered battery 98%, no configured app crash markers.
  The phone reports 4 KB pages; Unity `gfxinfo` exposes no usable frame histogram.
- `BattleRaja-FeatureArt-OriginalCandidate.png` is now the runtime menu/mode backdrop. It is
  an original Bazaar Bastion shrine/fighter composition with no vehicles, karts, racing track,
  copied characters, logos or text; the prior feature image is not referenced by runtime.

The truthful state is still **Prototype — Android offline release candidate in progress**:
complete physical all-fighter/tutorial/gadget/Aandhi/results/rematch comfort review, final
authored art/audio/cultural approval, normalized endurance, physical 16 KB runtime evidence,
permanent identity/signing, privacy/Data Safety/IARC and Play actions remain open.

## Repository and artifact evidence

- `git status --short --branch`: clean; `HEAD == origin/main` at `5631309`.
- `git lfs fsck --pointers`: passed.
- Two pre-existing user-owned stashes remain untouched; do not apply, delete or rewrite them.
- Latest commit: `feat: implement Bastion Crown offline 4v4 V1`.
- Candidate APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`,
  41,438,372 bytes, SHA-256
  `6EDC5C0E5D304529A6059A94F00F7AB32AB9C71A4464044D3B0D3ED5D3E2C507`.
- Candidate AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`,
  37,263,881 bytes, SHA-256
  `3D12A358E0F9159A2CA3749A4E53DBB712AF19B0FCCC6E6D80F96DE5944508EE`.
- The artifact is temporary/debug identity `com.example.battleraja.m11`, version
  `1.0.0`/code `100`, ARM64 and not a publishable signed package.

## Automated evidence checked

| Gate | Result | Evidence |
|---|---:|---|
| Repository validation | 0 errors, 0 warnings | `Tools/Validation/validate.ps1` run on 2026-09-01 |
| Bastion EditMode rerun | 148/148 passed | `Builds/Local/V1GameplayTruth/TestResults/bastion-objective-editmode-20260831.xml` |
| Bastion PlayMode rerun | 94/94 passed | `Builds/Local/V1GameplayTruth/TestResults/bastion-objective-playmode-20260831-rerun.xml` |
| Pure Bastion contract subset | 148/148 passed | `bastion-contracts-editmode-20260831.xml` |
| Existing Solo replay/soak fixture | 141/141 passed | `goal-baseline-deep-soak-20260831.xml` |
| LFS pointer integrity | passed | `git lfs fsck --pointers` |

The non-rerun integration XML files contain known transient failures and must not be used
as the current green claim. The rerun above is the evidence to reproduce, and the full
suites must be run again after any material change.

## What the latest implementation actually adds

- `BastionCrownContracts` defines explicit teams, roles, objective, ticket pool, respawn
  policy, score and result contracts.
- `BastionCrownMatch` owns mutable Crown, team score, KO/assist, ticket, respawn,
  spawn-protection, overtime and draw state for exactly actors 1–8.
- `OfflineMatchController` adapts the existing Unity combat/scene graph to the team layer;
  `BuildEntrypoints` regenerates the canonical scene composition.
- The production route exposes `PLAY OFFLINE` → Bastion Crown briefing → fighter choice →
  live arena, team HUD, Crown/shrine/socket telegraphs, result copy and rematch.
- Basic role-aware bot intent exists (contest, escort, defend, collapse), but it is not yet
  a tested squad blackboard, cover/spacing/assist planner or difficulty system.
- Existing Solo authority/replay fixtures remain valuable and must not be silently removed.

## Confirmed gaps that the next Goal session must resolve or evidence

1. The team result layer and legacy `OfflineMatchSimulation` currently form a hybrid mirror.
   Prove event ordering, health/respawn ownership, replay serialization and no double scoring,
   or consolidate the authority where evidence shows the mirror is unsafe.
2. Deposit is described as interruptible, but the current pure path explicitly cancels on
   death/leave and does not have a tested “combat damage interrupts channel” rule. Add the
   canonical behavior and regression coverage or revise the documented contract with evidence.
3. A completed deposit currently resets at the current socket; verify that this is the intended
   rotation rule and fix/document it before balance is frozen.
4. Overtime comparison currently resolves on overtime deposits only (or draw). Verify the
   promised sudden-death Crown behavior and deterministic tie-break sequence.
5. `HealingDone` is present in result data but has no confirmed Bastion event bridge. Validate
   ally healing/stat reporting and add tests if it is part of V1.
6. Bot intent is destination-level and has no multi-agent coordination metrics. Build
   deterministic multi-seed tests for ally support, objective contribution, spacing,
   retreat/regroup and fair difficulty without hidden cheats.
7. Current Bastion tests cover core happy paths but not all simultaneous pickup, boundary-time,
   repeated event, channel interruption, team-wipe, Aandhi, gadget, rematch or long-run cases.
   Expand them before calling the rules release-ready.
8. Final authored fighter/map/prop assets, animation personality, VFX, audio mix,
   accessibility review and fun/comfort review are not promoted by generated assets or green tests.
9. Physical Lava evidence, normalized frame/GC/memory/thermal/battery/endurance and physical
   16 KB runtime proof are open. The Android 16 AVD ANGLE route is emulator-only.
10. Package identity/signing, privacy policy, Data Safety, IARC/content rating, support URL,
    store copy/creative approval and Play Console actions remain owner-controlled.

## Device and reference evidence

- Latest rerun could not discover the approved Lava serial; no fresh physical claim is made.
- `Builds/Local/V1GameplayTruth/AndroidQA/emulator-5556/` contains Android 16 AVD ANGLE
  captures for menu, Bastion briefing, fighter choice and live arena. The host renderer
  crash during a fighter transition is an emulator configuration limitation, not physical proof.
- The 2026-08-31 Brawl Stars/Smash Karts observation is high-level, public-surface,
  anti-copy research only. It does not authorize extraction, decompilation, account or
  purchase actions, or use of their assets.

## Execution decision

Start the next Goal session from `5631309` and audit before changing code. Do not reimplement
the Bastion domain blindly. Prioritize authority hardening and tests, then real authored
assets and team AI, followed by device/performance/release evidence. Keep the final
classification **Prototype** until every critical gate is evidenced.
