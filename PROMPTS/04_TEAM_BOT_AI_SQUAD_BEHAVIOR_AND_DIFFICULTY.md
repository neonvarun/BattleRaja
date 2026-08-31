# 04 — Team Bot AI, Squad Behavior and Difficulty

## Context

The latest checkpoint gives `BotBrain`/`OfflineMatchController` basic role-aware Bastion intents (contest, escort, defend and collapse), while the shared `BotAI` remains largely a Solo utility controller. This stage turns those seams into believable four-bot squads without cheating, while preserving the tested Solo behavior.

## Objective

Friendly bots should make the human feel supported rather than blocked, while rivals should create pressure without clairvoyance or hidden damage. Every bot should understand its team, role, Crown/tickets/Aandhi state and the current squad plan, and should make imperfect but legible decisions.

## Current-state audit

Read `BotAI`, `BotBrain`, `BotPerceptionSensor`, `OfflineMatchController.TryGetBastionBotIntent`, command adapters, movement/collision, fighter/gadget definitions and simulation tests. Measure current target selection, reaction delay, aim noise, stuck recovery, gadget use, bot-to-bot damage and the new intent distribution. Identify every place that infers hostility from `CombatFaction`, scans Unity objects or makes a destination-only decision rather than consuming an authority/team snapshot. Add multi-seed squad metrics before tuning.

## Preserve

Preserve fair information boundaries, seeded decisions, common commands, existing movement/aim/attack/gadget adapters, stuck recovery, retreat/health behavior and Solo bot tests. Keep difficulty free of damage/health/cooldown cheats unless a documented accessibility setting explicitly changes a local mode.

## Replace/fix

Replace independent FFA-only targeting with a deterministic squad blackboard and explicit role behavior. Fix ally blocking, focus-fire dogpiles, suicidal Crown runs, ignored shrine defense, aim through walls, simultaneous gadget waste and bots that never regroup or help the human. Do not solve “bad AI” by making enemies stronger numerically.

## Implementation tasks

1. Add `TeamPerceptionSnapshot` with self/team IDs, allies/enemies, visible Crown state, shrines, tickets, score, Aandhi, role, health, cooldowns, recent damage and valid cover/route hints. Information must be line-of-sight/range/age bounded and deterministic.
2. Add roles with data-driven weights: `Anchor` (shrine/cover defense), `Runner` (Crown pickup/deposit), `Skirmisher` (flank/finish), and `Flex` (assist/regroup). Map Bijli/Pehel/Maya to suitable defaults and allow one duplicate role only when the selected roster requires it.
3. Add a squad blackboard/intent with one active plan (`ContestCrown`, `EscortCarrier`, `DefendShrine`, `RecoverTickets`, `CollapseTarget`, `Regroup`, `RetreatFromAandhi`). One bot proposes, the authority/arbiter resolves conflicts, and plans expire rather than becoming hidden global state.
4. Implement friendly priorities: keep a clear lane for actor 1, assist a threatened human, escort a carrier, peel attackers from a carrier/shrine, heal or deploy Tiffin when safe, use Umbrella to cross danger, and regroup after a failed contest.
5. Implement rival priorities: contest the Crown, pressure the carrier, defend their shrine, use cover and flanks, retreat at low health/tickets, and coordinate imperfectly with bounded communication delay. Enemies must not see hidden actors or future Crown sockets.
6. Use gadgets/abilities according to utility windows, cooldowns, target count and risk. Do not all trigger on the same tick. Add anti-stall behavior when a bot is stuck, pathless or outside Aandhi.
7. Define three fair difficulty profiles that vary reaction delay, aim noise, target scoring, plan quality, communication age and retreat risk. Keep health/damage/cooldown values identical across difficulties and record the balance rationale.
8. Add bot telemetry (decision state, role, plan, target, route, assist, Crown action, gadget/ability action, stuck recovery) behind a development capture flag that is absent from release UI.

## Asset tasks

Specify original role badges, team intent/ping icons, carrier/defend/retreat indicators and small ally status markers. They must work in high contrast, reduced flashes and text-free/icon+shape form. No reference game's icons or voice lines may be reused.

## Integration points

Integrate the blackboard with authority snapshots/events, fighter kits, Crown/ticket rules from prompt 03, navigation/collision, bot command adapters, `OfflineMatchController`, team HUD/signals and replay capture. Keep policy in pure C# where possible and Unity perception as an adapter.

## Performance constraints

Eight bots may evaluate at staggered deterministic intervals; do not run expensive path searches or allocations every frame. Cap perception targets, route nodes and blackboard history. No `Find*`, LINQ or per-bot garbage in the hot loop; profile CPU on Lava with final scene dressing later.

## Tests

Add deterministic unit tests for ally/enemy filtering, role assignment, information limits, plan arbitration, escort/defend/regroup/retreat transitions, gadget timing, difficulty invariants and stuck recovery. Add multi-seed simulations measuring Crown pickups/deposits, assists, human proximity, friendly-fire absence, objective contribution, deaths, ticket usage, Aandhi survival and no hidden-vision events. Preserve Solo AI tests.

## Visual QA

Observe a live match long enough to see an ally escort or peel, a shrine defense, a regroup, a retreat/heal, a rival flank and a fair miss. The squad should look intentional but not robotic. Reject bots that clump, body-block the human, stand outside Aandhi, camp a spawn or instantly know unseen events.

## Lava verification

On Lava `ST5GDW23LB004392`, play at least three matches with different player fighters/difficulties. Capture one friendly assist/escort, one rival objective pressure, one retreat/respawn and one gadget/ability decision. Run airplane mode and record seed/build; do not use Oppo.

## Failure cases

Test missing role data, no route, blocked shrine, Crown socket unavailable, actor death during plan, simultaneous plan proposals, stale perception, occluded target, pause/resume, low ticket pool, Aandhi relocation, player standing still, repeated rematch, bot component exception and difficulty misconfiguration. Recover deterministically or fail the match start with a useful local error.

## Binary acceptance gate

Pass only when both teams use explicit roles/plans, friendly and rival behavior is visibly different from FFA, information is fair, difficulty changes decisions rather than stats, multi-seed metrics meet documented ranges, Solo remains green and Lava shows objective-aware assistance/pressure. Four bots with a shared color but independent FFA logic is a fail.

## Evidence to retain

Bot architecture decision, role/weight tables, deterministic traces, difficulty comparison, simulation metrics, no-cheat assertions, test output, Lava gameplay captures and any tuning notes in `Docs/BALANCE_CHANGELOG.md`.

## Non-scope

Do not add online networking, server authority, voice chat, social coordination, new fighters, final animation/audio, economy or hidden difficulty buffs.

## Stop condition

Stop before prompt 05 if bots can see hidden information, cannot distinguish allies, cannot contribute to Crown/tickets, block the human, fail deterministic replay or require numerical cheats to be competitive.
