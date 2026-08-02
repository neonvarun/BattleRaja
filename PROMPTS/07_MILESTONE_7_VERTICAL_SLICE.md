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

