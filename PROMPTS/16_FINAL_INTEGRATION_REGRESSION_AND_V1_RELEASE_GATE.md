# 16 — Final Integration, Regression and V1 Release Gate

## Context

This is the final engineering/QA gate after the 4v4 rules, squad AI, fighter kits, authored assets, map, gadgets, feedback, UI, audio, tutorial, performance and packaging stages. The current repository is not allowed to be called a Play Store Release Candidate until evidence covers the actual final build.

## Objective

Prove a complete, polished, original offline BattleRaja V1 from cold launch to Bastion Crown 4v4 match, result and rematch on Lava, with deterministic regression, acceptable performance, representative store package and a transparent list of remaining owner gates.

## Current-state audit

Re-run source/branch/dirty-worktree checks and read every stage report. Confirm no prompt was marked complete on file existence alone, no canonical rule drift exists, no Solo test was silently removed, no debug/Online surface remains, and the exact final-art APK/AAB/hash/device/settings are known.

## Preserve

Preserve the authority/replay foundation, explicit 4v4 data contracts, Solo compatibility where documented, provenance/licensing, offline behavior, evidence history and owner/legal boundary. Keep failed evidence visible; do not rewrite reports to hide it.

## Replace/fix

Fix any cross-stage mismatch in teams, score, tickets, respawn, Crown, fighter roles, map anchors, UI terms, VFX/audio cues, tutorial steps, quality tiers, package metadata or store claims. Remove stale prototype/debug assets and dead Online entry points discovered in the final pass.

## Implementation tasks

1. Run the full static, EditMode, PlayMode, deterministic replay/soak, team simulation, bot fairness, asset import, UI/accessibility, audio, performance and Android release suites.
2. Perform a clean install on Lava and run the end-to-end matrix: menu/mode/fighter/tutorial/settings → 4v4 → all fighters/gadgets → Crown/tickets/respawn/spectator → Aandhi/overtime → result/stats/rematch → menu.
3. Repeat lifecycle/background/resume, safe-area/orientation, reduced-flash/high-contrast/text-scale/left-handed/haptic/music/effects and low-quality routes. Repeat ten rematches for memory/thermal/battery/endurance.
4. Validate APK/AAB hashes, bundletool install, target/min SDK, ARM64, 16 KB status, permissions, debug flags, dependency/license/secrets and store-art representative frames.
5. Review all reports for contradictions. Fix code/assets/docs and rerun affected gates; do not simply append a “known issue” for a critical defect.
6. Produce a final classification: `Play Store Release Candidate`, `Candidate with named blockers` or `Prototype`. Use the strictest honest classification.

## Asset tasks

Build a final asset inventory covering character source/model/rig/animation, map modules/props/materials, gadgets/Crown/shrines, VFX, UI/icons/portraits, audio/haptics and store creatives. Verify no placeholder/greybox/provenance gap remains in the player path.

## Integration points

Cross-check every first-party layer and scene: domain/application authority, team AI, fighters/gadgets, map, camera/VFX, UI/flow, audio/haptics, tutorial/accessibility, Android build and release/store docs. Verify future network seams remain isolated and unused.

## Performance constraints

Use prompt 13's normalized protocol and thresholds on the same final-art build used for Lava QA and representative screenshots. Report any device-limited or unqualified metric separately; never aggregate incompatible evidence.

## Tests

The gate requires exact command/output counts, zero unexplained failures, replay parity, multi-seed 4v4 results, lifecycle/input safety, asset validation, AAB validation and physical-device matrix. Any skipped test must have a named reason, scope and owner—not a pass.

## Visual QA

Inspect cold launch, every screen, every fighter/gadget/objective state, all team/score/ticket/Aandhi states, animation/VFX/audio pairing, low-quality/accessibility modes, results/rematch and store captures. The final contact sheet must match what the player actually receives.

## Lava verification

Use only Lava `ST5GDW23LB004392` for physical Android proof and record the exact final package/build/hash/API/settings. Do not substitute Oppo, an emulator or an earlier Solo candidate. A physical 16 KB claim requires a genuinely qualified physical/approved environment; otherwise report it as open.

## Failure cases

Test clean install/update, airplane mode, app kill/resume, lifecycle input, back navigation, no Crown score, all tickets exhausted, team wipe/tie, rematch reset, low memory/thermal, missing asset, AAB install, ABI/page-size/policy mismatch and every known defect. Critical failure means no RC classification.

## Binary acceptance gate

Classify as `Play Store Release Candidate` only if: offline 4v4 is complete from launch to rematch; all three fighters/gadgets/objective/map/UI/audio/VFX/tutorial/accessibility are production-ready; full automated/replay/soak tests pass; normalized final-art performance is acceptable; Lava end-to-end evidence is complete; AAB/technical Play checks pass; store/privacy/rating drafts are accurate; and no critical known crash/gameplay/release defect remains. Otherwise classify the exact blocker honestly.

## Evidence to retain

Final source commit/diff, all test counts/output, deterministic/replay reports, simulation metrics, asset/provenance inventory, screenshots/video/contact sheet, Lava run sheet/logs, normalized performance/thermal/battery/endurance reports, APK/AAB hashes, Play technical reports, store/policy drafts and owner-gate list.

## Non-scope

Do not upload/roll out to Play, accept legal agreements, choose permanent branding/signing identity without owner approval, add online/social/economy features or hide failed evidence.

## Stop condition

Stop and report `Candidate with named blockers` or `Prototype` if any binary gate fails, evidence is missing/unqualified, the final build differs from tested screenshots, or a critical defect remains. Never advance to a release claim by optimism.
