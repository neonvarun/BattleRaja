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
