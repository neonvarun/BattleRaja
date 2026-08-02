# Goal — Milestone 4: Bot AI Laboratory

/goal Complete BattleRaja Milestone 4 — Implement fair, debuggable bot opponents.


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

Implement a scalable bot AI layer that controls the same fighter command interface as a human:

- Navigation and path recovery
- Perception with line of sight and limited memory
- Utility or hierarchical-state decision system
- Target selection
- Range management
- Attack and dash use
- Retreat/reposition behaviour
- Imperfect aim and reaction delay
- Debug overlays
- One-, four- and seven-bot stress scenarios
- Tests and Android/Web builds

## FAIRNESS RULES

Bots must not:

- Read hidden actors without perception
- See through future smoke/cover rules
- Ignore cooldowns or collision
- Aim perfectly
- React instantly
- Teleport or receive secret combat multipliers merely to fake difficulty

Difficulty profiles may vary reaction delay, aim noise, tactical scoring, aggression and prediction quality.

## AI ARCHITECTURE

- Bot perception, decision and command output are separate modules.
- Decisions use seeded/injectable randomness where randomness is needed.
- Navigation failures have timeout and recovery behaviour.
- Bot logic must not depend on final graphics.
- Debug overlay shows current state, target, utility scores, perceived threats and stuck recovery.
- Keep update frequency tunable; expensive decisions need not run every rendered frame.

## INITIAL STATES/CONSIDERATIONS

- Explore/patrol
- Engage
- Reposition
- Retreat
- Recover from stuck state
- Seek future objective hook

Aandhi safety, loot seeking, healing and gadget use are added in M5/M6, but interfaces may be prepared without fake implementations.

## TESTS

- Perception and line-of-sight eligibility
- Target scoring
- Reaction delay and aim noise bounds
- Cooldown-respecting commands
- Retreat threshold
- Navigation stuck recovery
- Deterministic decision output under fixed seed
- Multiple bots do not share mutable runtime definitions
- Seven-bot stress test and allocation/performance evidence

## NON-SCOPE

No complete battle royale, Aandhi, pickups, placement, results, gadgets, Pehel/Maya, Photon, PlayFab or final art.

## COMPLETION GATE

- At least seven bots can spawn and fight with Bijli using the common command layer.
- Common deadlocks and permanent stuck states have recovery.
- Bots are intentionally imperfect and debuggable.
- Performance is measured under seven bots.
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

Create `Docs/MILESTONE_REPORTS/M4.md` and report in chat:

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

