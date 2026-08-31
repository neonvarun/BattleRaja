# 12 — Tutorial, Onboarding, Accessibility and Settings

## Context

The current tutorial and settings shell can route through a target lane, local preferences and lifecycle-safe controls, but it does not yet teach Bastion Crown's team roles, Crown/tickets/respawn or ally signals. Accessibility must be validated in the finished 4v4 flow, not assumed from toggle code.

## Objective

A first-time player can finish a short, skippable, replayable tutorial and enter a real match knowing how to move, aim, attack, use an ability/gadget, support allies, pick up/deposit the Crown, read tickets, respawn/spectate and respond to Aandhi. A returning player can change comfort/accessibility settings without losing state or entering an online flow.

## Current-state audit

Inspect `TutorialArena`, tutorial controller/targets, flow transitions, local preference keys/defaults, UI binding, safe-area handling, input adapter, haptics, reduced-flash/high-contrast/text-scale/left-handed/aim-assist paths and pause/lifecycle behavior. Test fresh install versus returning install and Solo compatibility.

## Preserve

Preserve local-only preferences, tutorial replay entry, skip/complete semantics, input/lifecycle hardening, existing target-lane teaching assets and authority-owned simulation. Keep settings independent of network/account/economy.

## Replace/fix

Replace target-lane-only instruction, unexplained team/objective vocabulary, inaccessible color-only cues, text clipping, non-persistent toggles, tutorial actions that bypass authority and any first launch that drops a player into a confusing 4v4.

## Implementation tasks

1. Define onboarding sequence: welcome/goal → movement/safe camera → aim/attack → ability → gadget choice → ally marker/signal → Crown pickup/carry/deposit → ticket/KO/respawn/spectator → Aandhi → results/rematch. Each step has one verb, one success condition and a retry/skip path.
2. Use a controlled tutorial sandbox with one human, scripted allies, one rival and a Crown/shrine; teach through the real command/authority path, not fake UI-only state. Keep tutorial duration under 3 minutes for a first completion.
3. Explain Bijli/Pehel/Maya role differences and show a small team composition example without requiring a fourth fighter. Let the player replay individual lessons from settings.
4. Implement settings: left-handed controls, reduced flashes/shake, high contrast, text scale, aim assist, music volume, effects volume, haptics, tutorial replay and any device-safe quality option. Persist locally, apply immediately and reset safely.
5. Add non-color redundancy: shape/icon/outline/pattern for teams, Crown, ticket, danger, ally/enemy and ability states. Ensure important text survives 0.9–1.3 scale and localization-safe width.
6. Validate pause/resume, background/foreground, orientation/layout changes and controller loss; release all input when paused or unfocused.

## Asset tasks

Create original tutorial panels, hand/aim/control illustrations, Crown/shrine/ticket/respawn/Aandhi diagrams, role cards, signal icons, high-contrast/reduced-flash variants, voice-free captions and settings icons. Record source/provenance and keep assets compact.

## Integration points

Integrate tutorial with mode contracts, fighter/gadget/objective authority, team AI scripted behavior, UI design system, VFX/audio/haptics, local preferences, Android lifecycle and results/rematch. Do not fork a second simulation for teaching.

## Performance constraints

Tutorial must load without duplicating production scenes/assets unnecessarily, avoid per-step allocations and release temporary actors/effects on exit. Settings changes must not trigger a full-scene rebuild or shader hitch.

## Tests

Add tests for fresh/returning tutorial state, each lesson success/failure/skip/replay, all settings defaults/persistence/immediate application, safe area/text scale/left-handed/high contrast/reduced flashes, pause/lifecycle/input release and no-network start. Run full EditMode/PlayMode/replay suites.

## Visual QA

Observe first-run tutorial and replay at normal/tall aspect, large text, high contrast, reduced flashes, haptics off and left-handed controls. A player unfamiliar with BattleRaja must be able to state the Crown goal, ticket consequence and basic ally action without developer help.

## Lava verification

On Lava `ST5GDW23LB004392`, clear local tutorial state or use a fresh install, complete and skip lessons, replay from settings, toggle every preference, background/resume, rotate/layout-check and enter a real 4v4. Capture screenshots/video and exact build/settings; never use Oppo.

## Failure cases

Test missing tutorial target, objective not spawning, lesson trigger out of order, player dies, bot/script timeout, skip during save, settings corrupted, text overflow, no haptics, audio muted, lifecycle pause during input, focus loss, airplane mode and rematch after tutorial.

## Binary acceptance gate

Pass only when a first-time player can complete or skip a real, authority-backed 4v4 tutorial; every required setting works and persists; accessibility cues are redundant/readable; lifecycle/input is safe; tests pass; and Lava observation confirms comprehension. A scripted slideshow or untested toggle is a fail.

## Evidence to retain

Lesson/state diagram, source tutorial assets/provenance, settings/default matrix, automated output, fresh/returning save logs, safe-area/accessibility screenshots and Lava completion notes/video with build/hash.

## Non-scope

Do not add online onboarding, accounts, monetization, voice chat, new fighters or a separate gameplay ruleset.

## Stop condition

Stop before prompt 13 if the tutorial does not teach the objective/respawn loop, any setting is cosmetic/non-persistent, text/controls fail on Lava, or pause/background leaves input or simulation unsafe.
