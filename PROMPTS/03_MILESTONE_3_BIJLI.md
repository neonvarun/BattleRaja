# Goal — Milestone 3: First Complete Fighter — Bijli

/goal Complete BattleRaja Milestone 3 — Implement the first complete playable fighter, Bijli.


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

Turn the generic combat actor into a data-driven fighter framework and implement Bijli:

- Fighter definition/content model
- Bijli basic electric-bolt attack
- Bijli directional dash ability
- Cooldown and ability-state rules
- Readable placeholder presentation
- HUD for health and cooldowns
- Bot/network-compatible command interfaces
- Balance/tuning data, tests and Android/Web builds

## BIJLI KIT

### Basic attack — Electric Bolt

- Medium-range readable projectile
- Clear telegraph, impact and range
- Configurable damage, speed, lifetime/range, cooldown and projectile size
- No chain effect unless already approved as a small, testable extension

### Ability — Electric Dash

- Direction chosen from current aim, falling back to movement/facing only by documented rule
- Fixed configurable distance/duration
- Collision-safe; cannot pass through forbidden geometry or leave valid play space
- Clear startup, active and recovery states
- Leaves a non-authoritative electric trail
- Cannot bypass cooldown or be retriggered illegally
- Network-friendly state transition and command validation

### Passive

Do not add a passive unless `Docs/DECISIONS.md` already approves it. Prefer a complete, balanced attack-and-dash kit over speculative scope.

## FRAMEWORK REQUIREMENTS

- Stable content IDs for fighters, attacks and abilities
- Immutable definition data plus per-fighter runtime state
- Generic cooldown/status/action-state infrastructure where justified
- Ability execution does not directly depend on Animator state
- Animation/VFX/audio are presentation hooks and may be placeholders
- Fighter hitbox and movement footprint remain explicit
- Debug inspector or overlay exposes current state and cooldowns

## TESTS

- Fighter definition validation and unique IDs
- Attack and dash cooldowns
- Dash distance across frame steps
- Dash collision and boundary handling
- Invalid/zero direction fallback
- Action interruption and illegal concurrent action prevention
- Health, attack and movement regressions
- HUD state updates
- Android and Web input invocation

## PLAYTEST TARGET

Bijli must feel mobile and readable, not invulnerable. Record current tuning and precise human questions about speed, bolt cadence, dash distance, recovery and camera interaction.

## NON-SCOPE

No Pehel, Maya, bots, Jugaad Gadgets, Aandhi, match loop, networking, accounts, final production model, production animation or monetisation.

## COMPLETION GATE

- Bijli is selectable/spawnable as the active fighter.
- Basic attack and dash work end to end.
- No ability timing depends on presentation.
- The fighter framework can support later fighters without copy-paste classes.
- Tests pass and Android/Web builds are attempted.
- Tuning and human-review debt are documented.


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

Create `Docs/MILESTONE_REPORTS/M3.md` and report in chat:

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

