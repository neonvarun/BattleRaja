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

