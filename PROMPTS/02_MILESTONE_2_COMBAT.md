# Goal — Milestone 2: Combat Laboratory

/goal Complete BattleRaja Milestone 2 — Cross-Platform Combat Laboratory.


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

Add a testable combat foundation to the movement laboratory:

- Central health and damage pipeline
- One configurable straight-line projectile weapon
- One training dummy or target actor
- Collision filtering and hit resolution
- Cooldown/fire-rate rules
- Object pooling for projectiles and repeated hit feedback
- Basic hit, damage and reset feedback
- Tests and Android/Web smoke builds

## ARCHITECTURE

- Weapons must create validated attack/damage requests; they may not directly mutate arbitrary target health.
- Authoritative gameplay timing must not depend on animation events, particles or audio.
- Keep attack configuration separate from runtime state.
- Projectiles and damage rules must remain usable by bots and future network authority.
- Use typed target/instigator IDs or approved entity references rather than scene-wide searches.
- Presentation feedback observes domain/application results.

## REQUIRED BEHAVIOUR

- Basic attack input works on mouse/keyboard and an Android attack control.
- Web focus loss cannot leave attack input held.
- Projectiles have explicit speed, range/lifetime, radius, layer policy and despawn reason.
- A target cannot receive duplicate damage from one hit unless explicitly configured.
- Friendly/self-hit policy is explicit.
- Health clamps correctly and emits a clear zero-health event.
- For this laboratory, the dummy may reset after defeat; full elimination and placement belong to M5.
- Add readable but placeholder hit flash, impact effect, sound hook and optional damage number.
- Pool projectiles and frequent impact effects.

## TESTS

### Pure/EditMode

- Health clamping
- Damage, mitigation hook and zero-health event
- Invalid weapon configuration
- Cooldown/fire-rate enforcement
- Projectile travel/lifetime calculations
- Collision/faction eligibility
- Duplicate-hit prevention
- Seed-independent deterministic rule behaviour

### PlayMode

- Attack input spawns the approved projectile
- Projectile hits a valid target and despawns
- Projectile ignores invalid layers
- Pool reuse works
- Repeated firing produces no unbounded object growth
- Dummy defeat/reset works
- Web/Android input adapters invoke the same attack command

## NON-SCOPE

No named fighter kit, dash ability, passive, bots, pickups, gadgets, Aandhi, battle-royale match state, Photon, backend, progression or final assets.

## COMPLETION GATE

- Movement still works without regression.
- Central damage pipeline is the only approved health mutation path.
- One complete projectile-to-damage loop is playable.
- Pooling and cooldowns are verified.
- Tests pass and Android/Web builds are attempted.
- No Milestone 3 ability or fighter-specific architecture is prematurely hard-coded.


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

Create `Docs/MILESTONE_REPORTS/M2.md` and report in chat:

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

