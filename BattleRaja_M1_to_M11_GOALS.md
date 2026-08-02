# BattleRaja — Complete Goal Prompt Collection (M1–M11)
Use the individual files in the repository. This combined copy is provided for reference.

---

# Goal — Milestone 1: Cross-Platform Movement Laboratory

/goal Complete BattleRaja Milestone 1 — Cross-Platform Movement Laboratory.


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

Create a polished grey-box movement laboratory containing:

- One small test arena with open space, walls, a narrow lane, corners and simple obstacles
- One placeholder fighter
- Responsive code-driven top-down movement
- Independent movement and aiming directions
- Stable elevated camera
- Aim-direction indicator
- Windows/desktop-Web keyboard and mouse input
- Android twin-stick input
- Safe-area-aware, configurable touch controls
- Tests and Android/Web smoke builds

## REQUIREMENTS

### Movement

- Human input must enter through the approved command/application boundary.
- The command shape must remain usable by bots and networking later.
- Movement must be frame-rate independent and code-driven, not normal-locomotion root motion.
- Support configurable speed, acceleration, deceleration, rotation, sensitivity and dead zones.
- Prevent diagonal speed gain, unintended sliding, stuck input and unstable collision behaviour.
- Runtime state must not be stored in shared ScriptableObject assets.

### Aiming and input

- Aim direction remains stable when movement stops.
- Mouse and right virtual stick must drive the same abstract aim action.
- On Web, prevent stuck input after focus loss and avoid unwanted page scrolling while the canvas owns deliberate input focus.
- On Android, separate touch IDs for each stick and reset safely on interruption.
- Do not add attack, ability or gadget buttons.

### Camera experiment

Compare orthographic and low-FOV perspective in a repeatable test scene. Select one using readability, depth, distortion, obstruction risk, aiming clarity and Android/Web performance. Record the evidence and decision in `Docs/DECISIONS.md`.

### Tests

- Input normalisation and diagonal movement
- Dead zones
- Acceleration/deceleration and max speed
- Invalid tuning values
- Aim persistence
- No movement after release/focus loss
- Spawn, collision and camera follow
- Bootstrap scene smoke test

### Performance

Avoid per-frame allocation and repeated component searches in hot paths. Record measured baseline data where available; mark unmeasured values honestly.

## NON-SCOPE

No combat, health, damage, abilities, bots, pickups, Aandhi, match flow, Photon, PlayFab, progression, final art, final animation or final audio.

## COMPLETION GATE

- The movement laboratory is playable in Editor.
- Keyboard/mouse works.
- Android touch implementation exists and is tested as far as tooling allows.
- Movement and aim are independent.
- Camera decision is documented.
- Relevant tests pass.
- Android and Web builds are attempted with real results.
- No unexplained compile errors or Milestone 2 systems exist.


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

Create `Docs/MILESTONE_REPORTS/M1.md` and report in chat:

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


---

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


---

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


---

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


---

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


---

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


---

# Goal — Milestone 7: Three-Fighter Cross-Platform Vertical Slice

/goal Complete BattleRaja Milestone 7 — Deliver a cohesive three-fighter vertical slice.


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

Transform the functional prototype into a recognisable alpha-quality BattleRaja vertical slice:

- Refined Bijli
- Complete Pehel
- Complete Maya
- One cohesive Bazaar Bastion arena
- Shared stylised low-poly visual language
- Gameplay animation system
- VFX and audio feedback
- Polished combat HUD and menus needed for the slice
- Short interactive tutorial
- Accessibility settings
- Balance data and automated playtest evidence
- Android and Web performance pass

## FIGHTERS

### Bijli

Refine attack, dash, telegraph, tuning and presentation without changing the core fantasy unnecessarily.

### Pehel — tank/grappler

- Short-range sweeping basic attack
- Short charge into validated throw/knockback
- Durable but kiteable
- Collision-safe displacement
- No unavoidable long crowd control

### Maya — trickster

- Medium-range illusion-shard basic attack
- Decoy that copies movement for a limited duration
- Decoy has explicit targeting, damage and expiry rules
- Lower direct damage compensated by deception/repositioning
- No hidden-information cheat or unreadable invisibility

## ART AND ASSET POLICY

- Create original assets only.
- Do not reproduce protected characters, UI, maps, sounds or signature effects from reference games.
- Follow `Docs/CULTURAL_GUIDE.md` and maintain provenance/licence records.
- Prefer a coherent alpha-quality procedural/low-poly in-engine style over inconsistent downloaded assets.
- Use Blender/editor automation or approved generation tools where available.
- If production-quality bespoke 3D art is impossible without a human artist, create polished replaceable alpha assets, maintain clean pivots/rigs/materials and document the replacement backlog. Do not falsely label placeholders as final art.

## BAZAAR BASTION

- Fictional fortified market courtyard
- Multiple routes and line-of-sight breaks
- Central risk/reward area
- No dominant camping location
- Foreground geometry must not obscure gameplay
- Valid spawns, final zones and bot navigation
- Mobile/Web-friendly lighting, shadows, materials and occlusion

## ANIMATION/VFX/AUDIO

- Code-controlled movement; no authoritative root-motion combat
- Reusable base rig where practical
- Idle, locomotion, aim, attack, ability, gadget, hit, knockback, elimination, victory and defeat states
- Pool frequent VFX
- Limit transparent overdraw
- Critical telegraphs survive low quality
- Original or properly licensed audio with mixer controls
- Browser autoplay rules handled

## UI/TUTORIAL/ACCESSIBILITY

- Main menu, settings, fighter select, loading, match HUD, pause and results for the slice
- Tutorial teaches movement, aim, attack, ability, gadget, Aandhi and elimination in roughly three minutes
- Safe areas, scalable layouts and localisation-ready strings
- Reduced shake/flashes, colour-safe indicators, volume controls, haptic toggle and control tuning

## QA AND PERFORMANCE

- Automated fighter kit and interaction tests
- Bot-vs-bot balance simulations with results treated as evidence, not truth
- Browser self-play/Computer Use where available
- Stress scene with eight fighters, gadgets and representative VFX
- Mid-range Android and desktop-browser budgets
- Repeat-match memory test

External human playtesting cannot be invented. Create a precise playtest script and `Docs/HUMAN_REVIEW_BACKLOG.md`. The orchestrator may provisionally continue after automated QA, but must record the uncompleted human review gate.

## NON-SCOPE

No Photon, online matchmaking, PlayFab, purchases, public deployment or store submission.

## COMPLETION GATE

- Three fighters are mechanically distinct and playable.
- Bazaar Bastion and the UI communicate an original BattleRaja identity.
- Tutorial completes end to end.
- Offline match remains stable.
- Performance and accessibility checks are documented.
- Tests and Android/Web builds pass or have explicit blockers.
- Human-review debt is recorded honestly.


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

Create `Docs/MILESTONE_REPORTS/M7.md` and report in chat:

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


---

# Goal — Milestone 8: Two-Client Networking Proof

/goal Complete BattleRaja Milestone 8 — Build a two-client Android/Web-capable networking proof.


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


## PRECONDITION AND RESEARCH

Re-verify current official Photon Fusion documentation, Unity/Android/Web support, licensing and topology options before integration. Record sources and the selected version/topology in decision and research logs.

Do not expose credentials. If a Photon App ID, package download, licence acceptance or account action is required and unavailable, complete every compile-safe adapter, mock, test and setup instruction possible, then mark the external gate clearly. Do not fabricate a successful cloud session.

## OUTCOME

- Photon Fusion isolated in infrastructure assemblies
- Network command/input adapter around existing gameplay commands
- Two-client room/session flow
- Networked movement, aim, basic attacks, Bijli ability, damage and elimination
- Local prediction/reconciliation strategy where supported
- Remote interpolation
- Lag/packet-loss simulation
- Network diagnostics overlay
- Android–Web protocol/content compatibility checks
- Offline mode remains fully functional

## AUTHORITY

For the proof, select the safest practical topology supported by the available environment and document its limitations. The architecture must preserve a path to trusted server authority.

- Clients send inputs/commands, not authoritative damage or rewards.
- Canonical hit/damage/elimination state belongs to the selected authority.
- Browser tab suspension must not be treated as reliable authority.
- No public ranked claim is allowed from a host/shared prototype.

## TESTS

- Two local/remote clients join and leave
- Movement and aim replication
- Attack/damage/elimination once-only semantics
- Ability cooldown validation
- Late join policy or explicit rejection
- Disconnect and cleanup
- 50/100/200 ms latency scenarios
- Jitter and packet-loss scenarios
- Offline mode regression
- Version mismatch rejection
- Android client with Web client where tooling and credentials permit

## NON-SCOPE

No eight-player matchmaking, dedicated public deployment, account progression, economy, public rewards, production anti-cheat or store release.

## COMPLETION GATE

Full gate:

- Two real clients complete a networked combat session.
- Offline mode still passes.
- Core domain has no direct Photon dependency.
- Authority and limitations are documented.
- Latency tests have real evidence.

Credential-blocked partial gate:

- Adapter, configuration validation, mocks, tests and setup documentation are complete.
- Project compiles without secrets.
- Exact human action/credential required is documented.
- Milestone is marked **blocked**, not complete.


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

Create `Docs/MILESTONE_REPORTS/M8.md` and report in chat:

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


---

# Goal — Milestone 9: Eight-Slot Android–Web Online Alpha

/goal Complete BattleRaja Milestone 9 — Build a stable eight-slot cross-play online alpha.


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


## PRECONDITION

M8 must have a validated real two-client session. If it remains credential-blocked, prepare independent server/match architecture work but do not claim M9 completion.

## OUTCOME

- Eight match slots
- Private-room flow and development matchmaking
- Bots fill vacant slots
- Trusted authoritative match simulation using the approved server topology
- Android–Web cross-play
- Networked Aandhi, gadgets, pickups, elimination, spectating and results
- Disconnect, grace period, bot takeover and reconnect
- Version/protocol/content validation
- Network stress tests and diagnostics
- Local/headless server build and deployment instructions

Public cloud deployment is not authorised unless credentials, cost and explicit approval are already present.

## AUTHORITY AND SECURITY

- Server validates movement limits, fire rate, cooldowns, gadget use, pickup distance, Aandhi, damage, placement and results.
- Clients cannot grant rewards.
- Rate limits and suspicious-state logging exist.
- Browser refresh/background-tab cases are handled.
- Match IDs and structured network logs support debugging.
- Dedicated/headless server does not depend on rendering or client UI.

## MATCHMAKING/ROOMS

- Private room code or invite flow
- Development queue capable of creating/finding an appropriate session
- Configurable region policy
- Bots backfill empty slots and may take over disconnected players by documented rule
- Clear errors for incompatible build, unavailable service, full room and timeout

## TESTS

- 1–8 clients/bots
- Android–Web cross-play
- Server authority rejection tests
- Mid-match disconnect/reconnect
- Browser refresh and background suspension
- Bot takeover and restoration policy
- Latency/jitter/packet loss
- Server process restart/failure behaviour where feasible
- Repeated online matches and memory/bandwidth evidence
- No duplicate results

## NON-SCOPE

No valuable account rewards, production-scale public deployment, payments, final live-ops, global launch or unsupported anti-cheat claims.

## COMPLETION GATE

- Eight slots complete stable matches under the approved development topology.
- Android and Web clients interoperate.
- A trusted server owns canonical public-alpha gameplay state.
- Disconnect/reconnect and bot fill are verified.
- Network stress evidence exists.
- No trivial client-side result/damage mutation succeeds.

If infrastructure credentials or paid hosting block real validation, mark the milestone blocked and continue only safe independent preparation.


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

Create `Docs/MILESTONE_REPORTS/M9.md` and report in chat:

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


---

# Goal — Milestone 10: Accounts, Cross-Progression and Cosmetic Economy Foundation

/goal Complete BattleRaja Milestone 10 — Implement secure accounts, progression and cross-platform persistence.


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


## PRECONDITION AND RESEARCH

Re-verify current official PlayFab identity, data, economy, leaderboard, server logic, pricing and Unity SDK guidance. Record the chosen SDK/version and alternatives. No secret may be placed in Android, JavaScript, WebAssembly or Git.

If PlayFab credentials/title configuration are unavailable, implement the full interfaces, local deterministic fake backend, migration tests, configuration validation and exact setup guide. Mark real-service integration blocked rather than inventing success.

## OUTCOME

- Guest identity for Android and Web
- Account-linking flow with conflict protection
- Profile and display name rules
- Shared cross-progression
- Fighter/cosmetic inventory
- Minimal soft and premium currency data model without purchases
- Experience/mastery progression
- Match history summary
- Leaderboards/statistics
- Remote configuration
- Server-validated idempotent match rewards
- Local cache/offline error handling
- Admin/debug tools that cannot ship enabled accidentally

## SECURITY AND DATA RULES

- Valuable state is server-owned.
- Reward grants use idempotency keys and trusted match-result evidence.
- Client cannot set currency, ownership, stats or leaderboard scores directly.
- Account linking cannot silently overwrite newer progress.
- Browser data clearing does not permanently destroy a linked account.
- Logs redact tokens and personal data.
- Data collection and retention are documented.
- No pay-to-win stats or loot boxes.

## INITIAL PROGRESSION

Keep it minimal and testable:

- Account XP or level
- Fighter mastery progress
- Cosmetic ownership
- Soft currency earned through validated matches
- Placeholder premium currency ledger with no purchasing flow
- Leaderboard/stat statistics
- Remote match/balance configuration where safe

## TESTS

- Guest creation and repeat login
- Android/Web link flow
- Link conflict and recovery
- Idempotent reward retry
- Tampered client reward rejection
- Inventory ownership
- Cache/cloud conflict
- Browser storage cleared
- Network failure/retry
- Leaderboard update through trusted path
- No secrets in builds/repository

## NON-SCOPE

No real-money purchases, battle pass, ads, production CRM, public economy tuning, global data migration or final legal policy acceptance.

## COMPLETION GATE

Full gate:

- A real configured test backend supports guest login, linking, cross-progression and validated rewards on Android/Web.
- Security tests pass.
- No secrets ship in clients.

Credential-blocked partial gate:

- Interfaces, fake backend, tests, setup docs and configuration checks are complete.
- Real service remains clearly blocked.


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

Create `Docs/MILESTONE_REPORTS/M10.md` and report in chat:

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


---

# Goal — Milestone 11: Android + Web Closed-Test Release Candidate

/goal Complete BattleRaja Milestone 11 — Prepare a closed-test release candidate for Android and Web.


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

Produce the highest-quality closed-alpha/release-candidate build possible without unauthorised publication:

- Onboarding and tutorial polish
- Settings persistence
- Consent/privacy surfaces required by integrated SDKs
- Analytics event schema and test/development validation
- Crash/error reporting integration or compile-safe adapter
- Accessibility pass
- Localisation readiness and English string audit
- Android device-tier matrix and browser matrix
- Performance, memory, loading and network optimisation
- Bug bash and regression suite
- Android AAB/APK candidate
- Hosted-locally or staging-ready Web build and website shell
- Store/web assets and truthful draft copy
- Release, rollback and support documentation
- Known-issues and human-review backlog

Do not publish to Google Play, deploy publicly, buy services, accept legal terms or assert legal compliance without explicit human approval.

## QUALITY WORKSTREAMS

### Reliability

- Crash/exception triage
- Repeated-match soak
- App background/resume
- Web refresh/tab suspension
- Network switching and reconnect
- Account/cache recovery
- Clean install and upgrade/migration test

### Performance

- Low/mid/high Android quality tiers
- Chrome/Edge/Firefox and Safari where available
- Web initial download and repeat-cache timing
- Peak memory and GC
- Eight-player final-circle stress
- Thermal/battery observations where a device is connected
- Correct Web compression/MIME/cache-header documentation

### Accessibility and UX

- Safe areas and aspect ratios
- Touch control customisation
- Reduced shake/flashes
- Colour-independent critical signals
- Text scaling/localisation expansion review
- Volume/haptics controls
- Keyboard/mouse focus and remapping review
- Clear offline, service and compatibility errors

### Release preparation

- Versioning and build provenance
- Third-party licence inventory
- Secrets and permission scan
- Google Play Data Safety input worksheet, not a fabricated submission
- Privacy/terms placeholders requiring legal review
- Store screenshots/icon/feature-graphic drafts if approved tools exist
- Web landing/game shell with support/privacy/version surfaces
- Deployment and rollback instructions

## QA

- Full automated suite
- Android and Web smoke tests
- Cross-play matrix where online services are configured
- Critical-path browser Computer Use/self-play where available
- Device and browser result table
- Severity-ranked bug backlog
- No open blocker/critical issue in the candidate

Human testing cannot be fabricated. Create exact closed-test instructions and a feedback form/template. Any unperformed device, cultural, legal, art or human-fun review remains explicit.

## COMPLETION GATE

A release candidate is ready when:

- Critical automated tests pass.
- Candidate Android and Web artifacts are produced or exact external blockers are documented.
- No known blocker/critical defect remains in tested paths.
- Performance evidence exists for available device/browser tiers.
- Security/secrets/licence checks pass.
- Release/rollback/support docs exist.
- Human, legal, cultural and publication approvals are listed as outstanding rather than assumed.

This milestone does **not** authorise public launch.


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

Create `Docs/MILESTONE_REPORTS/M11.md` and report in chat:

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


---

# Master Goal — Execute Milestones 1–11 Sequentially

/goal Build BattleRaja from the completed Milestone 0 foundation through the Milestone 11 Android + Web closed-test release candidate.


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


Also read every milestone prompt:

- `PROMPTS/01_MILESTONE_1_MOVEMENT.md`
- `PROMPTS/02_MILESTONE_2_COMBAT.md`
- `PROMPTS/03_MILESTONE_3_BIJLI.md`
- `PROMPTS/04_MILESTONE_4_BOTS.md`
- `PROMPTS/05_MILESTONE_5_OFFLINE_BATTLE_ROYALE.md`
- `PROMPTS/06_MILESTONE_6_JUGAAD_GADGETS.md`
- `PROMPTS/07_MILESTONE_7_VERTICAL_SLICE.md`
- `PROMPTS/08_MILESTONE_8_TWO_CLIENT_NETWORKING.md`
- `PROMPTS/09_MILESTONE_9_ONLINE_ALPHA.md`
- `PROMPTS/10_MILESTONE_10_ACCOUNTS_PROGRESSION.md`
- `PROMPTS/11_MILESTONE_11_CLOSED_TEST_RELEASE_CANDIDATE.md`

## OPERATING CONTRACT

Execute milestones strictly in numeric order. Treat each milestone file as its complete scope and acceptance contract.

### Before starting

- Verify M0 evidence and the current repository state.
- Require a clean or intentionally documented Git working tree.
- Create a local checkpoint commit before M1 when possible.
- Create `Docs/MILESTONE_REPORTS/`, `Docs/HUMAN_REVIEW_BACKLOG.md` and `Docs/EXTERNAL_SERVICE_GATES.md` if absent.
- Record that autonomous sequential execution is active in `PROJECT_STATUS.md`.

### Per-milestone loop

For each milestone M1 through M11:

1. Read its prompt and prior report.
2. Inspect actual code and plan small checkpoints.
3. Implement only that milestone.
4. Compile and run relevant tests repeatedly.
5. Run Android and locally served Web smoke builds when runtime code changes.
6. Use browser/game self-play and visual checks where tools permit.
7. Fix critical regressions before advancing.
8. Update project documents and create `Docs/MILESTONE_REPORTS/Mx.md`.
9. Evaluate the milestone's completion gate honestly.
10. Create a local checkpoint commit such as `milestone Mx: ...` when Git is configured and the repository is in a valid state.
11. Advance only if the technical gate passes, or if the only remaining items are explicitly recorded subjective human-review debt that does not make later architecture unsafe.

### Subjective review policy

Movement feel, balance, art taste and fun normally benefit from human review. For this autonomous run:

- Perform automated tests, browser self-play, simulation analysis and visual QA.
- Make conservative reversible choices aligned with the master vision.
- Record every unperformed human review item in `Docs/HUMAN_REVIEW_BACKLOG.md`.
- You may provisionally advance past subjective review debt, but may not claim it was human-approved.

### External-service gate policy

Photon, PlayFab, analytics, crash reporting, hosting, Google Play and public deployment may require accounts, credentials, licences, payment or legal acceptance.

- Never invent or expose credentials.
- Never accept paid/legal terms or deploy publicly without explicit permission.
- Complete compile-safe interfaces, mocks, tests, configuration validation and setup documentation when an external service is unavailable.
- Continue independent later work where technically safe.
- Mark affected milestones **blocked/partial**, not complete.
- Do not claim the complete online/live-service game is validated if real services were not tested.

### Failure recovery

- When a test or build fails, diagnose and attempt focused fixes.
- Do not repeatedly rewrite architecture to escape a local bug.
- Revert the smallest faulty checkpoint if needed.
- Stop the entire run when the repository cannot compile, data could be destroyed, a security boundary would be violated, or later work would be built on an invalid foundation.
- Continue unrelated safe work when only one platform/tool/service is blocked.

### Scope control

- Do not add unrequested modes, large rosters, battle passes, purchases, ads, voice chat, clans, esports, procedural open worlds or other feature creep.
- Use exactly the staged product described in the milestone prompts.
- Keep the first release candidate to one main solo battle-royale mode, three fighters, three gadgets and Bazaar Bastion.
- Optimise for a coherent closed alpha, not a content-heavy global launch.

## FINAL OUTCOME

The best possible autonomous result is:

- A tested Android and Web closed-alpha/release-candidate project
- Complete offline game loop
- Three fighters and three gadgets
- Bot matches
- Cross-play online alpha when real networking services are available
- Accounts/cross-progression when real backend services are available
- Release candidate artifacts and documentation

This goal cannot authorise publication, buy infrastructure, provide human taste approval, perform unavailable physical-device tests, accept legal terms or create missing third-party accounts. Report those honestly.

## FINAL REPORT

After M11 or when no further safe progress is possible, create `Docs/MILESTONE_REPORTS/FINAL_AUTOPILOT_REPORT.md` containing:

- Milestone-by-milestone gate table: passed / provisional / partial / blocked
- Playable features
- Android/Web artifacts
- Online/backend validation status
- Automated test totals
- Performance evidence
- Human-review backlog
- External-service gates
- Security/legal/cultural/publication gates
- Known defects and technical debt
- Git checkpoint history
- Exact actions the human owner must take next

Do not call the product commercially finished or publicly launched unless all real services, human review, legal review and deployment approvals actually occurred.
