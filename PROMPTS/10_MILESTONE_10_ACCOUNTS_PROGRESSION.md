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

