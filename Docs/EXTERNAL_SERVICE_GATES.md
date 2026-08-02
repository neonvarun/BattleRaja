# External Service and Approval Gates

This file is the authoritative blocker list for the M1–M11 run. No blocked item is treated
as complete, and no secret is stored in the repository.

| Gate | Current state | Exact owner action | Unblocking evidence |
|---|---|---|---|
| Photon Fusion 2 | Blocked; no approved package, App ID, licence/account configuration | Create/approve the Fusion application, provide the authorized package/version and non-secret local App ID configuration, and approve terms | Real two-client Lava Android + desktop Web room with authority, prediction/reconciliation, reconnect and controlled latency/loss logs |
| Eight-slot online alpha | Blocked by the Photon real-session precondition | Complete the M8 real-session gate; separately approve any hosting/paid infrastructure | Stable 1–8 client/bot matches, room/backfill, disconnect grace, bot takeover, reconnect and stress evidence |
| PlayFab identity/progression | Blocked; no title ID, SDK, account or server secret channel | Create/approve a PlayFab title/environment, select the authorized API/SDK, configure server-only secret delivery, and approve privacy/retention/linking policy | Real Android/Web guest/link/cross-progression/reward/inventory/leaderboard tests with no client secret |
| Release signing / Google Play | Not authorised; no signing key or store submission | Owner approves signing identity, key storage, store/legal review and submission plan | Signed AAB/APK verification and explicit store approval; no public upload from this run |
| Public Web hosting/CDN | Not authorised; only local HTTP smoke exists | Owner selects and approves hosting, domain, HTTPS, headers, costs and rollback | Staging deployment and rollback test with approved access |
| Crash/analytics service | Not configured; compile-safe local adapters only | Owner selects service and approves data collection, privacy and secret configuration | Redacted event/crash delivery test in approved environment |
| Human/legal/cultural/accessibility review | Open | Owner and reviewers complete the documented checklists | Signed review decisions and resolved critical issues |

Safe local preparation completed: deterministic networking/progression mocks, release guards,
tests, setup docs and local Android/Web artifacts. These do not replace the gates above.
