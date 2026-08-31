# 01 — Current State and Reference Audit

## Context

You are auditing the exact BattleRaja checkout before continuing offline 4v4 V1 completion. At the 2026-09-01 checkpoint, the repository has a real Bastion Crown team/objective layer on top of a technically strong Solo foundation, generated/provenance-safe presentation assets and a temporary Android candidate. The approved physical evidence phone is Lava `ST5GDW23LB004392`; Brawl Stars and Smash Karts may be installed there for observation-only reference work. Reconfirm every fact because the source can advance.

## Objective

Produce a reproducible baseline that tells the next stages what is real, what is missing and what is visually unacceptable. From a player's perspective, the audit must answer why the current game feels pre-alpha and what must change for an original, polished 4v4 team game.

## Current-state audit

Re-run `git fetch --all --prune`, status/branch/HEAD/origin/log/stash/LFS checks, Unity/package checks, static validation, full EditMode/PlayMode, Bastion replay/soak coverage and the current Android build/install. Trace `BastionCrownContracts`, `BastionCrownMatch`, `OfflineMatchAuthority`, `OfflineMatchController`, `BotBrain`, `OfflineMatchHud` and `BuildEntrypoints` so the report distinguishes canonical state from legacy mirrors. Capture menu, briefing, fighter selection, gameplay, Crown pickup/deposit, Aandhi, defeat/respawn/spectator, results, rematch, tutorial, settings and lifecycle behavior. On Lava, explicitly launch `com.supercell.brawlstars` and `com.tallteam.citychase` only if the approved device is connected; observe public surfaces and store notes/captures outside production assets. Use current official/first-party online sources for policy, Unity/Android APIs, accessibility, mobile performance and high-level product principles; record dates and decisions. Do not copy implementation or content.

## Preserve

Keep the pure domain/application split, authority ownership, common human/bot command path, seeded randomness, replay identity, existing Solo tests, provenance manifests, lifecycle hardening and any asset/code proven healthy. Leave user changes and stashes untouched.

## Replace/fix

Flag every Solo-only assumption, missing team state, independent-only bot behavior, generated/primitive-looking character or environment content, sparse menu hierarchy, debug-like HUD, weak impact/audio, unnormalized performance claim and temporary package/release identity. Do not excuse a visible defect because an automated test passes.

## Implementation tasks

1. Build a source inventory with exact paths and owning layer.
2. Build a behavior inventory: current Solo loop, actor count, spawn, combat, gadgets, Aandhi, spectator/results/rematch.
3. Build a visual inventory with camera-distance screenshots and severity labels.
4. Build a release/performance inventory with artifact hashes, package metadata, tests, device/API and known gaps.
5. Build a reference matrix entry for each useful principle and its BattleRaja-specific interpretation.
6. Record contradictions between current docs and the new 4v4 product decision; do not silently rewrite authoritative product docs in this stage.

## Asset tasks

Do not create final art in this audit. Verify provenance and dimensions of current models, textures, animation clips, VFX, audio, UI graphics and store images. Mark whether each item is a concept, technical baseline or release candidate asset.

## Integration points

Inspect `OfflineMatch`, `OfflineMatchAuthority`, `BotAI`, `BotBrain`, `BotPerceptionSensor`, `OfflineMatchController`, `OfflineMatchHud`, `ProductionFlowController`, `BuildEntrypoints`, production builders, `BazaarBastion.unity`, `TutorialArena.unity`, tests and release tooling. Link findings to `Docs/AI/V1_PRODUCT_REBUILD_AUDIT.md`, `Docs/AI/V1_REFERENCE_DESIGN_MATRIX.md` and the prompt rewrite manifest.

## Performance constraints

Use the exact final candidate identity for telemetry. Separate bounded Lava measurements, host/AVD 16 KB smoke and physical Lava page-size facts. Do not report a frame-rate or memory budget without the capture method, settings, duration and limitations.

## Tests

Run the existing static, EditMode, PlayMode and deterministic replay/soak suites. Confirm the baseline count and failures. Do not change tests merely to make the audit green. Add only small audit checks or scripts when they improve repeatability.

## Visual QA

Capture cold launch, menu hierarchy, fighter cards, live camera, HUD, controls, map cover, all three fighters, gadget cues, Aandhi and results at gameplay distance. Inspect the images and motion on the device; desktop/editor screenshots alone are insufficient.

## Lava verification

Use Lava serial `ST5GDW23LB004392` only. Confirm installed package/version/build identity before overwriting. Airplane-mode launch must be tested. Never use Oppo `b60e53b3` as evidence. Store captures under a timestamped local audit folder without committing raw device dumps.

## Failure cases

Test dirty worktree, stale installed package, failed build/install, missing LFS assets, scene bootstrap failure, orientation/safe-area changes, pause/resume input leakage, offline launch, missing reference app surface and incomplete test output. Record the exact blocker and do not guess.

## Binary acceptance gate

Pass only when the current source/branch/build/device are identified; all baseline tests have recorded results; the full current route has been observed; the severe flaws and retained foundations are documented; reference observations have anti-copy interpretations; and every unverified area is explicitly listed. A launch screenshot or green test suite alone is a fail.

## Evidence to retain

Commit/branch log, package metadata, APK/AAB hashes, test output, device/API details, route captures, reference captures outside production assets, source inventory, severity table and the audit report paths.

## Non-scope

Do not implement 4v4, remodel assets, rewrite runtime systems, add Photon/PlayFab, alter package identity, upload anything or delete documentation outside `PROMPTS/`.

## Stop condition

Stop before prompt 02 if source identity, baseline tests, approved-device route or the major Solo/4v4 contradictions are unknown. Fix the audit evidence first.
