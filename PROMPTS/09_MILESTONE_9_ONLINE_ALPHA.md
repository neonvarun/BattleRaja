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

