# Goal — Milestone 6: Jugaad Gadget System

/goal Complete BattleRaja Milestone 6 — Implement three tactical Jugaad Gadgets.


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


## DEFAULT VERTICAL-SLICE GADGETS

Unless an approved decision already selects different gadgets, implement:

1. **Umbrella Guard** — brief directional ranged-damage protection.
2. **Dhol Burst** — readable radial knockback/interrupt pulse.
3. **Tiffin Station** — temporary destroyable healing station with clear limits.

These defaults provide defence, control and support without resembling real harmful instructions.

## OUTCOME

- Generic data-driven gadget definition and runtime system
- Pickup, inventory capacity and use command
- Spawn/rarity/distribution rules
- The three complete gadgets
- Bot evaluation and use
- Counters and readable telegraphs
- HUD slot and feedback
- Balance data and tests
- Android/Web builds

## SYSTEM RULES

- Initial capacity is one held gadget unless an approved decision says otherwise.
- Pickup and use are validated through the application/simulation layer.
- Gadgets cannot directly bypass central damage, healing, status or movement rules.
- Spawn logic must avoid inaccessible or immediately unsafe positions.
- Stable content IDs and versionable definitions are required.
- Bot use must evaluate context rather than fire randomly.
- Effects must remain readable at low quality settings.

## GADGET-SPECIFIC REQUIREMENTS

### Umbrella Guard

- Explicit facing arc/direction
- Duration and mitigation/block policy
- Counterplay from rear, melee/area effects or duration pressure as documented
- Clear start/end visual state

### Dhol Burst

- Explicit radius and knockback curve
- Short interrupt only where allowed
- Cannot repeatedly stun-lock
- Collision-safe displacement

### Tiffin Station

- Validated placement
- Health and lifetime
- Healing rate/cap and damage-interruption policy
- Destroyable and targetable
- Cannot create infinite final-circle stalemates

## TESTS

- Definition and ID validation
- Pickup capacity and replacement policy
- Spawn eligibility
- Directional shield eligibility
- Knockback bounds/collision safety
- Station placement, healing, destruction and expiry
- Bot contextual use
- Restart cleanup
- Interaction with Aandhi, elimination and spectator state

## BALANCE VALIDATION

Run bot simulations and targeted scenarios. Report use rate, apparent value, failure cases and whether any gadget dominates. Do not claim human balance validation.

## NON-SCOPE

No additional gadgets, Pehel/Maya, final art overhaul, networking, backend or monetisation.

## COMPLETION GATE

- Three distinct gadgets work in full offline matches.
- Humans and bots can pick up and use them.
- Every gadget has documented counterplay.
- No obvious infinite-heal, displacement or shield exploit remains.
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

Create `Docs/MILESTONE_REPORTS/M6.md` and report in chat:

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

