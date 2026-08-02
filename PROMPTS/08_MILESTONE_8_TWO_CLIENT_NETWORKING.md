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

