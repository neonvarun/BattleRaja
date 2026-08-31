# 03 — 4v4 Match Rules, Respawn, Score and Objective

## Context

The latest checkpoint adds `BastionCrownContracts` and `BastionCrownMatch` for explicit teams, Crown state, score, tickets, respawn and results, while `OfflineMatchAuthority` still owns the legacy combat/runtime mirror. This stage owns the continuation audit and hardening of `BR_BastionCrown_V1`; do not duplicate or replace healthy work without evidence, and keep the simulation pure and deterministic.

## Objective

Implement a fair, readable and replayable four-versus-four match that feels complete from ready screen to result. Every score, KO, assist, ticket, pickup, deposit, respawn and overtime outcome must be produced by authority state and be reproducible from a seed.

## Current-state audit

Read `BastionCrownContracts.cs`, `BastionCrownMatch.cs`, `OfflineMatch.cs`, `OfflineMatchAuthority.cs`, `OfflineMatchController.cs`, combat-group/faction code, projectile/gadget resolution, replay events, spawn configuration and all related tests. Trace the exact order in which legacy damage becomes team damage, defeat, Crown drop, respawn, score and result snapshots. Test for mirror drift, duplicate event IDs, deposit interruption, socket rotation, team wipe, overtime, Aandhi, healing and rematch. List compatibility hazards before changing `CombatFaction` or any constructor used by Solo tests.

## Preserve

Preserve fixed-step timing, seeded randomness, event identity, authority validation, common command handling, damage attribution, gadget/cooldown rules, Aandhi math and Solo behavior behind an explicit mode definition. Keep rejected commands side-effect free and do not move mutable state into ScriptableObjects or Unity objects.

## Replace/fix

Add explicit `TeamId`/relationship semantics instead of overloading `CombatFaction.Player/Enemy`. Replace last-survivor-only resolution in the Bastion Crown mode with team score/ticket/objective resolution. Remove any path that treats an allied bot as hostile, awards score twice, respawns a ninth actor or lets a defeated actor keep affecting the match.

## Implementation tasks

1. Add a data-driven `BastionCrownDefinition` (or equivalent) containing the canonical values below, with validation and serialization/replay support.
2. Spawn exactly eight participants: Team Raja actor 1 human + 2–4 friendly AI; Rival actors 5–8 enemy AI. Define west/east spawn banks and deterministic slot order.
3. Add team relationships, ally/enemy target filtering, soft ally collision and friendly-fire-off validation for projectiles, abilities, gadgets, Aandhi and environmental damage.
4. Add `CrownSparkState` with three seeded socket locations, carrier ID, drop/lock/rotation timers and deposit channel state. Pickup is 0.25 seconds of valid contact; a carrier moves 12% slower; defeat drops the Crown for 6 seconds with a 1.25-second pickup lock; shrine deposit is a 1.25-second interruptible channel.
5. Add team score: +1 for a confirmed KO, +3 for a shrine deposit. Assists record contribution but do not award a second KO score. Clamp/validate all event identities and prevent duplicate scoring on replay or repeated damage.
6. Add two shared ticket pools, 12 each. A KO consumes no ticket immediately; the defeated actor enters four seconds of spectator/respawn presentation and respawns at five seconds only if its team has a ticket. Consume exactly one ticket at respawn. When a pool is empty the defeated fighter remains a spectator. A team wipe means all four team slots are out with no valid pending return and ends the match; simultaneous KOs with queued respawns do not.
7. Apply spawn protection for 2.5 seconds or until that actor deals damage; show the state in authority and presentation. Damage to protected actors must follow one consistent, tested policy.
8. Implement the clock: ready 3 seconds, live 240 seconds, and maximum 30-second overtime. Aandhi warns at 180 seconds, contracts toward the active Crown socket/shrines through the final minute and never bypasses score/ticket authority.
9. Resolve first-to-15 immediately after a valid score; otherwise at time expiry use score, then deposits, KOs, tickets remaining, and sudden-death Crown score. Overtime ends on a deposit, team wipe or 30-second cap; a fully unresolved tie is an explicit deterministic draw result, never a random winner.
10. Emit immutable result snapshots containing team score, deposits, KOs, assists, deaths, damage, healing, Crown pickups/deposits, objective time, gadget/ability uses, tickets spent, winner/tie and seed. Rematch must construct fresh state with the selected fighter/settings and a new seed.
11. Add explicit commands/events for pause/resume and brief defeated-player spectating; preserve full Solo spectator behavior where applicable.

## Asset tasks

Define data references for three Crown socket markers, two shrine anchors, ticket pips, carrier/drop/deposit states, respawn countdown and team score. Do not bind authority to a mesh or UI prefab. Later visual prompts must create the assets and map dressing against these anchors.

## Integration points

Integrate with `OfflineMatchAuthority`, `OfflineMatchSimulation`, damage/projectile/gadget resolution, replay recorder, `OfflineMatchController`, `BotPerceptionSensor`, scene spawn anchors, results UI and tutorial. Add a mode adapter so Solo definitions continue to compile and test without team-only fields.

## Performance constraints

Use fixed-size participant/team arrays where practical, avoid per-tick allocations, avoid LINQ/reflection in hot paths, and keep event payloads compact. Objective queries should be O(actors + sockets), not repeated scene searches. Replay must remain deterministic across editor/player/Android.

## Tests

Add pure EditMode/unit tests for exact 4+4 composition, team hostility, friendly-fire rejection, soft collision, Crown pickup/drop/lock/rotation/deposit, channel interruption, +1/+3 score idempotence, assists, 12-ticket respawn, spawn protection, spectator state, team wipe, first-to-15, timer expiry, Aandhi interaction, overtime/tie-break order and new-seed rematch reset. Run existing Solo/replay tests and a multi-seed 4v4 simulation soak with event traces.

## Visual QA

Before moving on, inspect a live match at camera distance: team markers, Crown carrier/drop, shrine channel, ticket loss, respawn countdown/invulnerability, score change, Aandhi warning, overtime and results must be understandable without debug labels. Confirm the player can distinguish ally/enemy even when skins have similar colors.

## Lava verification

Build/install the exact current candidate on Lava `ST5GDW23LB004392` in airplane mode. Play at least one match far enough to score by KO and deposit, die and respawn, exhaust or approach tickets, enter Aandhi/overtime, view results and rematch. Record seed/build/settings and capture the route; never use Oppo for evidence.

## Failure cases

Test duplicate event delivery, damage after death, pickup during drop lock, simultaneous shrine channels, carrier leaving the map, objective socket missing, no valid spawn, all four allies defeated, ticket count underflow, score overflow, timer boundary at exactly 240/270 seconds, pause during channel/respawn and deterministic replay across platforms.

## Binary acceptance gate

Pass only when all canonical rules are authority-owned, deterministic, replayable and covered by passing tests; Solo still passes; exactly eight actors and two teams appear in a real build; the player can score, respawn/spectate, reach Aandhi/overtime and receive the correct result/rematch state on Lava. A colored 1v7 match, a UI-only score or a non-deterministic tie is a fail.

## Evidence to retain

Mode definition asset/schema, architecture decision, event trace, unit/integration/soak output, replay hashes, source/build commit, Lava route screenshots/video, result snapshot sample and a list of any tuned constants with balance rationale.

## Non-scope

Do not tune squad strategy, create final character/map/audio assets, add network transport, add progression/economy or change Play package identity. Those are later prompts.

## Stop condition

Stop before prompt 04 if any rule is presentation-owned, if Solo regressions exist, if event identity is not idempotent, if a 4v4 match cannot be replayed deterministically, or if the Lava route cannot demonstrate score and respawn.
