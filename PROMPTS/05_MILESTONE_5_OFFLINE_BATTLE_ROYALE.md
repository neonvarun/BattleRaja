# Goal — Milestone 5: Complete Offline Battle Royale

/goal Complete BattleRaja Milestone 5 — Deliver one full offline eight-combatant battle-royale match.


Before changing anything, read completely:

- `AGENTS.md`
- `PROJECT_STATUS.md`
- `PROJECT_CONTEXT.json`
- `Docs/MASTER_VISION.md`
- `Docs/WEB_PLATFORM.md`
- `Docs/ARCHITECTURE.md`
- `Docs/DECISIONS.md`
- `Docs/PERFORMANCE_BUDGET.md`
- `Docs/TEST_STRATEGY.md`
- `Docs/RESEARCH_LOG.md`
- `Docs/MILESTONE_ISSUES.md`
- the prior milestone report under `Docs/MILESTONE_REPORTS/`, when present
- the milestone prompt named in this goal

Treat the repository documents as authoritative. Inspect the actual code, Git state, installed tools, package versions and previous test evidence before implementation. Preserve approved architecture and working behaviour.


## OUTCOME

Create the first complete game loop with one human and seven bots:

- Match state machine
- Valid separated spawn system
- Eight combatants
- Aandhi closing zone
- Basic neutral pickups/resources required for the loop
- Elimination and placement
- Spectator mode
- Winner/results screen
- Restart/rematch
- 4–6 minute target duration
- Automated simulation soak tests
- Android/Web builds

## MATCH PHASES

Implement explicit phases:

1. Load/warm-up
2. Spawn and short protection
3. Opening phase
4. Aandhi pressure phases
5. Final circle
6. Resolution/results

The authoritative local simulation owns the phase, zone, placement and winner state. UI and effects only present it.

## AANDHI

- Data-driven phase timing, target radius and damage curve
- Clear world boundary and HUD warning
- Safe final-zone validation against inaccessible terrain
- Bots prioritise returning to safety
- Outside-zone damage follows the central damage pipeline
- Presentation intensity may scale by quality, but gameplay boundary cannot

## ELIMINATION AND SPECTATING

- One authoritative elimination transition per fighter
- Placement order is stable and testable
- Eliminated player input is disabled
- Spectator cycles valid living targets
- Match ends exactly once
- Results show placement, eliminations and damage; no economy rewards yet

## SOAK/QUALITY

Create deterministic simulation or accelerated test support where practical. Run at least 20 consecutive complete bot-filled matches and report:

- completion count
- average/min/max duration
- unresolved deadlocks
- exceptions
- peak/ending object counts or memory evidence

## TESTS

- Spawn separation and valid positions
- Phase transitions
- Aandhi radius/damage timing
- Safe-zone destination validation
- Elimination idempotency
- Placement and winner logic
- Spectator target selection
- Restart cleans all runtime state
- 20-match soak completion
- Repeated-match memory/object growth

## NON-SCOPE

No Jugaad Gadgets, Pehel/Maya, polished final map, Photon, accounts, progression, monetisation or production release.

## COMPLETION GATE

- One complete 4–6 minute match is playable from start to results and rematch.
- Seven bots handle Aandhi and finish matches.
- Twenty automated/simulated matches complete without a critical blocker.
- No critical repeated-match memory growth is observed.
- Tests pass and Android/Web builds are attempted.


## WORKING METHOD

1. Verify that the previous milestone is complete enough to support this milestone.
2. Inspect the existing implementation before proposing changes.
3. Create a concise execution plan and expected file/test list.
4. Implement in small checkpoints.
5. Compile and run the most relevant tests after each checkpoint.
6. Fix regressions before adding more scope.
7. Update documentation as decisions become real.
8. Attempt both Android and locally served Web builds when the milestone affects runtime code.
9. Use current official primary documentation for SDK, platform or policy facts that may have changed.
10. Never invent successful tests, playtests, credentials, builds, performance numbers or external reviews.

Routine reversible decisions consistent with the master vision may be made without pausing. Record them. Pause only for destructive actions, paid services, legal acceptance, secrets, public deployment, final branding, or a material contradiction in the approved architecture.



## FINAL REPORT

Create `Docs/MILESTONE_REPORTS/M5.md` and report in chat:

- What was implemented
- Acceptance criteria and gate status
- Files created or changed
- Commands executed
- EditMode, PlayMode, integration and performance test results
- Android build result and artifact path
- Web build result, local serving command and browser smoke-test result
- Performance observations
- Warnings and failures
- Known limitations and technical debt
- Human playtest/review debt
- External-service or credential blockers
- Git status and concise diff summary
- Local checkpoint commit hash, when created
- The single recommended next action

Update `PROJECT_STATUS.md`. Do not begin the next milestone unless this goal is being executed by the approved M1–M11 orchestrator.

