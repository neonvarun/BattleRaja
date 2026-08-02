# BattleRaja Full Build Runbook — M1 to M11

## Recommended use

The safest workflow is one milestone goal at a time. The master autopilot goal is available for a long unattended run, but it can only pass gates that the local environment and configured services allow.

## Sequence

1. M1 — Movement, aim, camera and cross-platform input
2. M2 — Combat foundation
3. M3 — Bijli
4. M4 — Bot AI
5. M5 — Complete offline battle royale
6. M6 — Three Jugaad Gadgets
7. M7 — Three-fighter vertical slice
8. M8 — Two-client networking proof
9. M9 — Eight-slot Android–Web online alpha
10. M10 — Accounts and cross-progression
11. M11 — Closed-test release candidate

## What “finished” means here

M11 produces a closed-alpha/release-candidate codebase and build artifacts. It does not automatically provide final bespoke art, legal approval, public cloud infrastructure, store publication, real users, human fun/balance validation or third-party accounts.

## Checkpoint files

Each milestone must create `Docs/MILESTONE_REPORTS/Mx.md`. The orchestrator must maintain:

- `Docs/HUMAN_REVIEW_BACKLOG.md`
- `Docs/EXTERNAL_SERVICE_GATES.md`
- `PROJECT_STATUS.md`

## Rollback

Use a clean local Git commit after every passing milestone. Do not push or publish unless already authorised.
