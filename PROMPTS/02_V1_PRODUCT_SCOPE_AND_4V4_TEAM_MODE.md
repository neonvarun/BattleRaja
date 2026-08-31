# 02 — V1 Product Scope and 4v4 Team Mode

## Context

The approved V1 direction is a polished offline 4v4 team game, and the latest checkpoint already exposes Bastion Crown in the production route. This stage audits that implementation against one coherent product contract before further authority, AI or art work continues.

## Objective

From a player's perspective, a fresh install must promise and enter one understandable game: **Bastion Crown**, one human and three friendly bots against four rival bots, with a clear objective, short replayable match, fair combat and a satisfying rematch. The product must remain unmistakably BattleRaja, not a clone of a familiar arena mode.

## Current-state audit

Confirm the current `ProductionFlowController`, mode enum/state machine, scene bootstrap, `BuildEntrypoints` actor slots, `OfflineMatch`/`BastionCrownMatch` definitions, results model and Solo compatibility route. Identify any remaining hard-coded assumption that actor 1 is the only player, actors 2–8 are hostile, `ALIVE` is the only top-level status, or resolution means last survivor. Check where `CombatFaction` is used and whether the new explicit team model can drift from the legacy combat adapter or break Solo.

## Preserve

Preserve the offline-first promise, existing Solo foundation and replay coverage as a secondary/future mode, fighter selection, tutorial/settings/rematch shell, command/authority seams, local preferences and the no-network/no-account/no-ads/no-IAP boundary.

## Replace/fix

Replace ambiguous mode copy, FFA-only language and inaccessible online entry points. Replace the idea that team mode is “eight actors with different colors” with explicit team identity, objective language, team results and a teachable loop. Remove any public-facing Online button from the V1 player flow while retaining internal future seams.

## Implementation tasks

1. Write a product brief for `BR_BastionCrown_V1` using the canonical contract in `PROMPTS/README.md` and prompt 03.
2. Define data contracts for `TeamDefinition`, `TeamMember`, `ModeDefinition`, `ObjectiveDefinition`, `TeamScore`, `TeamTicketPool`, `RespawnPolicy` and `ResultSummary` without putting mutable runtime state in shared assets.
3. Define the eight deterministic participant slots: actor 1 human, actors 2–4 allies, actors 5–8 rivals. Define explicit role/fighter assignment and safe failure if a slot cannot initialize.
4. Define the primary flow: cold launch → `PLAY OFFLINE` → Bastion Crown explanation → fighter selection → ready → match → result/brief spectator → rematch or menu. Solo may be a non-primary route only when it cannot dilute this flow.
5. Decide and document whether Solo is hidden, secondary or debug-only for this build; update a decision record rather than leaving two public promises.
6. Create a small terminology table: Team Raja, Rival, Crown Spark, shrine, ticket, deposit, KO, assist and Aandhi. Use these names consistently in code/UI/tutorial.
7. Update product/release docs only after confirming the implementation plan; note the current `MASTER_VISION` Solo contradiction explicitly.

## Asset tasks

Specify, but do not yet fully produce, the mode card, team banners, Crown icon, shrine icon, ticket icon, role badges, objective tutorial diagrams and results badges. Each asset needs a provenance note, 1×/2×/3× UI export, high-contrast fallback and a gameplay-camera readability check.

## Integration points

Integrate with `ProductionFlowController`, `ProductionFlowMachine`, fighter definitions, `BuildEntrypoints`, scene bootstrap, local preferences, authority constructor/configuration, tutorial entry, results/rematch and build profiles. Keep the public V1 flow independent of Photon/PlayFab and network availability.

## Performance constraints

Product contracts must not require a second always-loaded scene, hidden network handshake or duplicate actor graph. Team/objective state should be compact, deterministic and serializable for replay. Do not make UI polling or asset loading part of the simulation tick.

## Tests

Add pure tests for mode selection, exact 4+4 composition, no online dependency, valid role/fighter assignment, deterministic mode seed/config and rematch reset. Add flow tests for every accepted/rejected transition, including returning from settings/tutorial and a failed scene load. Preserve all Solo tests.

## Visual QA

Review the intended flow as a product storyboard on a phone aspect ratio. A new player should know: who is my team, what is the Crown, where do I take it, how do I read tickets/score, and how do I rematch. Reject screens that still look like a developer mode selector or an online lobby.

## Lava verification

On Lava `ST5GDW23LB004392`, exercise cold launch in airplane mode, `PLAY OFFLINE`, mode explanation, all three fighter choices, back navigation, tutorial/settings entry and rematch shell. Use installed Brawl Stars/Smash Karts only to compare hierarchy and immediacy; do not copy their surfaces.

## Failure cases

Test wrong actor count, duplicate/missing team member, invalid fighter definition, stale Solo save, Online selection, missing objective data, orientation change during selection, back press during loading, scene-load failure and no-network launch. Fail closed with a useful local error; never silently start a 1v7 match under a 4v4 label.

## Binary acceptance gate

Pass only when one reviewed product contract names Bastion Crown, fixes exact 4v4 composition and flow, defines terminology and Solo disposition, has data contracts accepted by the architecture, has pure/flow tests, and the offline Lava route visibly communicates the new promise. No authority implementation or art stage may proceed with unresolved rule/name contradictions.

## Evidence to retain

Product brief, terminology/scope table, data-contract diagram, flow transition tests, screenshots/video of the offline entry route, device/build identity, Solo compatibility note and updated decision/research references.

## Non-scope

Do not implement detailed combat scoring, bot tactics, final models, final map dressing, online mode, progression/economy or Play submission. Those belong to later prompts.

## Stop condition

Stop before prompt 03 if team composition, primary flow, Solo disposition, mode vocabulary or data ownership is ambiguous. Resolve the contract in docs/tests first.
