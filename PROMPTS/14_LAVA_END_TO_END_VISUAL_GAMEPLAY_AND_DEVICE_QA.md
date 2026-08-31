# 14 — Lava End-to-End Visual, Gameplay and Device QA

## Context

Code/tests/builds have previously passed bounded routes, but the current candidate still has open human visual, comfort, fun and endurance gates. This stage is the independent physical-device proof of the final offline 4v4 player experience.

## Objective

Demonstrate on one approved normal Android phone that a fresh player can launch offline, understand the mode, play all fighters/gadgets, read team/objective/Aandhi state, die/respawn/spectate, finish, inspect results, rematch, change settings and resume after lifecycle events without visible release defects.

## Current-state audit

Confirm exact source commit, APK/AAB hash, package/version, device serial/model/API, install state, orientation, quality tier and settings. Read all prior QA reports and identify which evidence is stale, Solo-only, host/AVD-only or SKIP-path-only. Start with a clean install or documented reset.

## Preserve

Preserve user data outside the test package, deterministic seed/log conventions, existing QA routes, offline behavior and the rule that Lava is the only physical evidence device. Keep Brawl Stars/Smash Karts captures outside production assets.

## Replace/fix

Fix any clipping, touch miss, input leak, debug text, placeholder art, broken transition, unreadable team/objective cue, bad animation, camera discomfort, audio clipping, thermal/battery regression, crash or stale package discovered by hands-on use. Do not downgrade a failure to “human review.”

## Implementation tasks

1. Prepare a timestamped run sheet with build/hash, device/API, battery/thermal state, orientation, quality/accessibility settings and test operator.
2. Run cold launch/airplane mode → main menu → `PLAY OFFLINE` → mode explanation → fighter selection → tutorial/settings → ready → 4v4 match.
3. Exercise Bijli, Pehel and Maya as the human across multiple matches; observe all three gadgets, Crown pickup/drop/deposit, shrine channel, team signals, ally assistance, enemy pressure, KO/respawn/spectator, Aandhi/overtime and team result.
4. Complete results → stats → rematch with a new seed, then return to menu. Repeat ten rematches for memory/lifecycle sanity.
5. Toggle left-handed, reduced flashes, high contrast, text scale, aim assist, music/effects, haptics and quality; repeat critical route. Background/foreground and pause at movement, attack, channel, KO and results.
6. Capture visually important states and write reproduction steps for every defect. Retest each fix on the same build and a clean install.

## Asset tasks

Capture final character/map/gadget/VFX/UI/audio evidence; do not edit captures to hide defects. Create a small shot list for store-eligible gameplay screenshots only after the build is proven representative. Keep raw diagnostic captures local and provenance notes for any final creative.

## Integration points

Exercise the complete product stack: bootstrap/flow, authority, team AI, fighter prefabs, map, gadgets/Crown, VFX/camera, UI, audio/haptics, tutorial/settings, lifecycle and Android build configuration.

## Performance constraints

Use the protocol and thresholds from prompt 13. Capture normalized frame/memory/GC/thermal/battery/endurance evidence from the exact final-art candidate rather than relying on subjective smoothness. Stop if heat, battery drain, stalls or leaks make the route unsafe or uncomfortable.

## Tests

Run full static, EditMode, PlayMode, deterministic replay/soak and Android build/AAB checks before and after the device pass. Maintain a matrix covering all fighters, gadgets, team states, results, settings, lifecycle, airplane mode and repeated matches. Every failed cell must have a fix or explicit blocker.

## Visual QA

Inspect motion and touch, not only screenshots: launch/menu hierarchy, cards, camera, silhouettes, attack/ability/impact, Crown/shrine/tickets, team HUD/signals, Aandhi, controls, pause, spectator/respawn, result/rematch, tutorial/settings and low/high accessibility states. Compare reference principles only at a high level.

## Lava verification

Use serial `ST5GDW23LB004392` (`LAVA_LXX508`) exclusively. Never use Oppo `b60e53b3`. Record package/version/build/hash, ADB/logcat/profiler evidence, captures and exact settings. Airplane mode must remain on for the offline proof; reference-app observation is separate.

## Failure cases

Test clean install/update, no network, denied/irrelevant permissions, rotation/display cutout, background kill/resume, pause input, rapid taps, low battery/thermal, audio focus, haptic absence, text scaling, accessibility toggles, blocked spawn, no Crown score, team wipe, rematch and package mismatch.

## Binary acceptance gate

Pass only when the complete 4v4 flow is played and visually inspected on Lava with all fighters/gadgets/settings/lifecycle paths, no critical defects remain, tests/builds pass, performance evidence is attached and every failure is fixed or honestly blocked. A scripted SKIP route, emulator-only result or screenshot-only review is a fail.

## Evidence to retain

Run sheet, clean-install/build identity, test matrix, screenshots/video, logs/profiler captures, defect ledger and retest results, ten-rematch memory/thermal notes, airplane-mode proof and approved-device metadata.

## Non-scope

Do not use Oppo, publish/upload, accept legal terms, change signing keys, copy reference content or add new gameplay features during a QA run except a clearly tracked defect fix.

## Stop condition

Stop before prompt 15 if any critical flow, fighter, gadget, accessibility/lifecycle route or final-art performance check fails on Lava, or if the exact build under test cannot be identified.
