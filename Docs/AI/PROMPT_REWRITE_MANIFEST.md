# BattleRaja V1 Prompt Rewrite Manifest

**Prepared:** 2026-08-31 10:20 IST
**Repository/source recorded:** `neonvarun/BattleRaja`, `3bed64e82be0a84c8bf978d871ae322604b3f7ff`
**Branch:** `codex/v1-playstore-release`
**Purpose:** record the prompt architecture reset and the canonical execution graph for the next Luna Max Goal-mode implementation session.

## Repository state recorded before the rewrite

- `HEAD == origin/main` at `3bed64e82be0a84c8bf978d871ae322604b3f7ff`.
- Worktree was clean; no user changes were discarded.
- Git LFS pointer check passed.
- Existing stashes were left untouched.
- Current documented automated baseline: static `0/0`, EditMode `141/141`, PlayMode `92/92`; deterministic replay/soak evidence is Solo-only.
- Approved device is Lava `ST5GDW23LB004392`; Oppo is not an evidence device.
- Current APK/AAB are temporary/debug-signed and use `com.example.battleraja.m11`; neither is publishable identity proof.

## Old prompt files removed from `PROMPTS/`

The following historical files were deleted/replaced in this planning pass. Git history remains the archive; no documentation outside `PROMPTS/` was deleted.

| Historical file | Why it no longer controls V1 |
|---|---|
| `README.md` | Pointed at the old milestone sequence and Solo handoff |
| `BattleRaja_V1_FINAL_OFFLINE_ANDROID_GOAL_2026-08-29.md` | Solo-focused completion handoff; not a 4v4 implementation graph |
| `BattleRaja_Next_Continuation_Performance_UX_NetworkReadiness_2026-08-23.md` | Mixed future networking and continuation work |
| `BattleRaja_Goal_Master_Handoff_2026-08-22.md` | Historical M0–M11 handoff |
| `99_AUTOPILOT_M1_TO_M11.md` | Retrospective milestone autopilot |
| `00_MILESTONE_0_BOOTSTRAP.md` | Closed bootstrap milestone |
| `01_MILESTONE_1_MOVEMENT.md` | Closed movement milestone |
| `02_MILESTONE_2_COMBAT.md` | Closed combat milestone |
| `03_MILESTONE_3_BIJLI.md` | Closed single-fighter milestone |
| `04_MILESTONE_4_BOTS.md` | FFA bot milestone, not squad AI |
| `05_MILESTONE_5_OFFLINE_BATTLE_ROYALE.md` | Solo Raja mode, not team objective mode |
| `06_MILESTONE_6_JUGAAD_GADGETS.md` | Closed gadget milestone |
| `07_MILESTONE_7_VERTICAL_SLICE.md` | Old vertical slice scope |
| `08_MILESTONE_8_TWO_CLIENT_NETWORKING.md` | Explicitly out of V1 |
| `09_MILESTONE_9_ONLINE_ALPHA.md` | Explicitly out of V1 |
| `10_MILESTONE_10_ACCOUNTS_PROGRESSION.md` | Explicitly out of V1 |
| `11_MILESTONE_11_CLOSED_TEST_RELEASE_CANDIDATE.md` | Old release-candidate gate for the Solo prototype |

## New prompt files and dependency order

| Order | File | Responsibility | Must pass before |
|---:|---|---|---|
| 0 | `README.md` | Pack contract, canonical vocabulary and gate protocol | Every stage |
| 1 | `01_CURRENT_STATE_AND_REFERENCE_AUDIT.md` | Rebaseline source, device, references and evidence | Product decisions |
| 2 | `02_V1_PRODUCT_SCOPE_AND_4V4_TEAM_MODE.md` | Lock V1 scope and Bastion Crown product contracts | Authority work |
| 3 | `03_4V4_MATCH_RULES_RESPAWN_SCORE_AND_OBJECTIVE.md` | Implement deterministic team authority/rules | Squad AI and presentation |
| 4 | `04_TEAM_BOT_AI_SQUAD_BEHAVIOR_AND_DIFFICULTY.md` | Implement fair role-aware squad behavior | Full combat tuning |
| 5 | `05_FIGHTERS_KITS_ROLES_BALANCE_AND_COMBAT.md` | Reconcile fighter roles, kits and balance | Art and feedback tuning |
| 6 | `06_CHARACTER_CONCEPT_3D_MODELS_RIGS_AND_ANIMATION.md` | Create/import real authored fighter assets | Map/UI/VFX integration |
| 7 | `07_MAPS_ENVIRONMENT_ART_LIGHTING_AND_WORLD_BUILDING.md` | Build flagship 4v4 Bazaar combat space | Gadgets/objective dressing |
| 8 | `08_GADGETS_PICKUPS_OBJECTIVES_AND_INTERACTABLES.md` | Integrate gadgets, Crown and interactables | VFX/UI/audio |
| 9 | `09_COMBAT_VFX_CAMERA_FEEDBACK_AND_READABILITY.md` | Make combat/objective state readable | UI/audio/perf |
| 10 | `10_UI_UX_MENU_HUD_TEAM_SIGNALS_AND_RESULTS.md` | Replace prototype UI with mobile product flow | Tutorial/final QA |
| 11 | `11_AUDIO_MUSIC_HAPTICS_AND_GAME_FEEL.md` | Create and mix original audio/feedback | Final QA/perf |
| 12 | `12_TUTORIAL_ONBOARDING_ACCESSIBILITY_AND_SETTINGS.md` | Teach and expose the complete offline experience | Lava route |
| 13 | `13_PERFORMANCE_MEMORY_RENDERING_AND_ANDROID_OPTIMIZATION.md` | Profile/optimize final art candidate | Release packaging |
| 14 | `14_LAVA_END_TO_END_VISUAL_GAMEPLAY_AND_DEVICE_QA.md` | Prove real play on approved Lava | Play materials |
| 15 | `15_PLAYSTORE_RELEASE_STORE_ART_PRIVACY_AND_PACKAGING.md` | Prepare technical/store/legal drafts | Final gate |
| 16 | `16_FINAL_INTEGRATION_REGRESSION_AND_V1_RELEASE_GATE.md` | Run the complete release gate and report honestly | Classification |
| final | `99_MASTER_V1_GOAL.md` | Orchestrate Luna Max through all stages | Fresh Goal session |

## Canonical 4v4 design lock

The pack uses one name and one baseline so agents do not drift:

- Mode ID: `BR_BastionCrown_V1`; player-facing name: **Bastion Crown**.
- Team Raja: actor 1 human + actors 2–4 friendly AI. Rival: actors 5–8 enemy AI.
- One 32×32 walkable flagship arena, west/east team spawn banks, three Crown sockets and two team shrines.
- Three-minute warning/pressure window inside a four-minute live clock; deterministic maximum 30-second overtime.
- Team score: +1 confirmed KO, +3 Crown deposit. First team to 15 wins; otherwise highest score at time wins.
- Each team starts with 12 shared tickets. A defeated fighter spectates for 4 seconds and respawns after 5 seconds while tickets remain; each respawn consumes one ticket. Exhausted fighters stay spectators; a team wipe means all four slots are out with no valid pending return.
- Spawn protection lasts 2.5 seconds or until the fighter deals damage. Friendly fire is off, ally collision is soft and allied target selection/healing are explicit.
- Crown carrier has a 12% movement penalty, drops the Crown on defeat, and deposits through a 1.25-second interruptible channel at the team shrine. The Crown rotates through its three sockets on a deterministic 35-second cadence or after a deposit.
- Aandhi warns at 180 seconds and contracts toward the active socket/shrines; overtime ends on a score, team wipe, or the 30-second cap. Deterministic tie-break order is deposits, KOs, tickets remaining, then sudden-death Crown score.
- Stats: KOs, deaths, assists, damage, healing, Crown pickups/deposits, objective time, gadget/ability uses and tickets spent. Rematch keeps local settings/fighter choice but creates a new seed and resets match state.

These are starting balance values in data assets, not permission to hard-code mutable state. Any change requires a balance note, test update and evidence.

## Scope lock

Mandatory: 4v4 offline team mode, Bijli/Pehel/Maya, all three gadgets, Crown objective, tickets/respawn/spectator, team HUD/signals, one production-ready map, tutorial, settings/accessibility, audio/VFX, performance, Android package and store drafts. Solo Raja may remain as a secondary/future route only if it does not delay the primary mode.

Out of V1: Photon gameplay, PlayFab, accounts, matchmaking, social, clans, cloud progression, shop, ads, IAP, online leaderboards, Web release work and copied reference content.

## Gate protocol

Every stage must retain source diff, test output, build identity, screenshots/video where useful, and a short pass/fail report under the documented local evidence area. “Files exist”, “generated image exists”, “scene launches”, “tests pass” or “owner review pending” alone never satisfies a gate. Stop on a blocker, fix or document the exact external dependency, and do not silently mark the stage complete.
