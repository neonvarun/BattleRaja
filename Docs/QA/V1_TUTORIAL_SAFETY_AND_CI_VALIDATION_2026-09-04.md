# V1 tutorial safety and CI validation — 2026-09-04

## Scope

This checkpoint covers the tutorial terminal-resolution fix and the GitHub repository
validation repair. It is an incremental offline Android candidate checkpoint; it does not
replace the full V1 release matrix or owner-controlled gates.

## Repository and CI diagnosis

The failed GitHub run was
[`33816621445`](https://github.com/neonvarun/BattleRaja/actions/runs/33816621445) on
`e9294b1`. The only job, **Static repository, LFS and secret checks**, failed in
**Scan tracked source for forbidden secret assignments**. The same run also displayed a
Node.js 20 deprecation warning for the checkout action.

The scanner used a PCRE inline `(?i)` prefix with `git grep -E`, whose Git regex engine
expects POSIX ERE, and the PowerShell runner treated the normal no-match exit code `1` as
a terminating native-command failure. The final workflow uses a portable case-insensitive
POSIX pattern, explicitly distinguishes exit `1` (no matches) from an actual grep error,
and pins `actions/checkout@v5`, which runs on the Node 24 runtime. The repair history is
kept as focused commits `e728ef8`, `1170fe1` and `840097b`.

The final tutorial commit `a7ea3ce` also passed the workflow run
[`33836993117`](https://github.com/neonvarun/BattleRaja/actions/runs/33836993117). Its
job `100911438127` completed successfully; checkout, repository validation, whitespace /
LFS checks and the secret scan all passed.

## Tutorial defect and fix

A fresh pre-fix Lava walkthrough could reach `MATCH RESULTS` while the tutorial card was
still waiting on the gadget lesson after the inherited Solo idle target elapsed. The
underlying cause was that the tutorial reused automatic timeout / last-participant
resolution even though its lessons were not complete.

The tutorial now uses an explicit `OfflineMatchDefinition.Tutorial` with automatic timeout
and last-participant resolution disabled. `TutorialOverlay` asks the live authority to
resolve only when the user advances into the Victory lesson; the validated player actor is
then preferred for that guided result. Solo and Bastion definitions retain their existing
automatic resolution behavior. EditMode and PlayMode regressions cover both the live-after-
timeout invariant and the explicit Victory transition.

## Automated evidence

- Static repository validation: **0 errors / 0 warnings**.
- EditMode: **160/160 passed**. XML SHA-256
  `095E4483D76F97FC0053969C91585DFBA40B5F6841FA75DCBD5EDF5550A54D7D`;
  log SHA-256 `A522ED7F2290494F731BDBA20637FCAD38D304A151D59761CE6A54BD0FAD1088`.
- PlayMode: **98/98 passed**. XML SHA-256
  `2FF73B300915CB2198111EDDEBBAD62EB9FE5EEEECA707A4A7241D7F0F3AB808`;
  log SHA-256 `4D7462B8F1B3E1BB4298397920C470D2EE6FE2DC1ED94BCC4740B71C075A1041`.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,680,960 bytes,
  SHA-256 `36AEBACF19F098D3F5763539CBB854C1A0BE6E4F8ADB3CC38BF6171E0856CB0D`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,506,488 bytes,
  SHA-256 `1D26950F1C85A4F97FD7BB5E2D0938D906EF1DC03D1B7408A4BE77B6282C730A`.
- Technical release checker: **0 errors / 0 warnings**. Log SHA-256
  `B4363F769459F7E7783212C7AC5E691E140B39F4A32246C45AE18F0988A91265`.
  Package is temporary `com.example.battleraja.m11`, version `1.0.0` / code `100`,
  API `28/36`, ARM64-only, offline permissions and static 16 KB alignment passed.

## Approved Lava evidence

The exact APK was installed on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android
14/API 34, `1080x2460`, reported 4 KB pages) after clearing the package. Fresh rendered
captures are under `Builds/Local/V1GameplayTruth/Next/tutorial-safety-20260904/`:

- `00-menu.png` — fresh offline menu, SHA-256
  `9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`.
- `01-tutorial-start.png` and `02-move-observed.png` — tutorial launch and movement gate.
- `04-aim-observed.png`, `07-attack-observed.png` and `10-ability-grid.png` — aim,
  attack and ability lessons reached through real touch.
- `11-gadget-prompt.png` and subsequent route captures — the gadget lesson remained live
  while the pickup was being approached; it did not publish Results during this probe.
- `20-tutorial-complete-card.png` — the explicit SKIP path still exposes the dismissible
  `TUTORIAL COMPLETE 8/8` card. This is not action-by-action comfort evidence.

The full post-fix physical route through gadget collection, Aandhi, elimination, explicit
Victory and Results was not completed in this bounded session. No claim of physical 16 KB
runtime compatibility, FPS, comfort or human fun approval is made. The device UI dump
exposes Unity's `unitySurfaceView` only; app-level accessibility semantics remain open.

## Carried-forward evidence

Presentation-only tutorial and workflow changes did not alter the canonical gameplay,
replay or bot harness. The canonical 100-match report remains
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260901-220113865-9101.json`
(SHA-256 `D44275C62CDF18ADDD9581020088FAB39685279E0AD896CE8F799C20DA867E73`), with
the documented target-window, combat-positive, bot-to-bot damage, KO, ticket/respawn,
objective and planner metrics in `Docs/QA/CURRENT_STATE.md`.

## Classification and remaining gates

The truthful classification remains **Prototype — Android offline release candidate in
progress**. Generated baseline art/audio, full physical tutorial and accessibility
comfort, normalized performance/endurance, repeated-match growth, physical 16 KB runtime,
permanent package/signing identity, privacy/Data Safety, IARC/content rating, cultural /
brand review and Play Console actions remain open owner or human gates.
