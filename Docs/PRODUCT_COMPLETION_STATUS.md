# Product completion status

Updated: 2026-08-22
Classification: **prototype**

Latest continuation: Phase 0 exact-current-source rebaseline — local `main` tip `35d723f`
baseline (validate 0/0, EditMode 114/114, PlayMode 57/57, Android APK
`77B2BD04…A43D2A`, Lava launch clean, Chrome/Edge smoke clean), followed by the
`phase0/exact-source-rebaseline` branch: `17a8c75` lands the documented weapon retune
in Core definitions and `c78433a` syncs scene serialization plus removes tracked test
artifacts, with fresh post-fix evidence recorded in `Docs/QA/LATEST_HEAD_BASELINE.md`.
The production Bazaar scene retains its explicit `BazaarBastionScene` contract,
connected architecture prefab and authority-routed combat path.

This file records evidence-backed status only. Allowed status values are: `Not started`, `In progress`, `Passed with evidence`, `Blocked`, and `Human review required`.

## Phase 0 rebaseline continuation — 2026-08-22

Exact-current-source evidence replaced stale `af2e0d8`/`7ad7e42`/`a5fdde8` references.
Key correction: the 2026-08-21 weapon-damage balance entry (Bijli 12 / Pehel sweep 20 /
Maya shard 9) had never been applied to the authoritative Core definitions, so prior
builds shipped unretuned damage; definitions now carry the documented values and are
covered by `BijliFoundationTests`. Recurring worktree churn (asset flips, scene field
re-sync, tracked Unity Test Framework performance artifacts) was eliminated at the root.
Full tables: `Docs/QA/LATEST_HEAD_BASELINE.md`.

## Phase 1 authority audit closure — 2026-08-22

A full audit of `OfflineMatchAuthority` against the Phase 1 checklist found one real
gap: attack commands bounded caller ticks above but not below, allowing stale-tick
submissions to consume weapon cooldown in the past (fire-rate bypass). Fixed at
`ee573ad` (ADR-053): stale commands are rejected with a distinct `StaleTick` failure and
accepted attacks anchor cooldown consumption/reporting to the canonical clock. The rest
of the checklist verified clean from source: canonical monotonic 30 Hz tick, complete
rejection matrix, authority-owned roster/loadout/faction/origin/tick-rate/cooldown,
same-tick hits from different attackers preserved by construction, scene-reload cleanup
covered by two PlayMode regressions, presentation mutation/vendor scans active, and all
production gameplay consumers on the fixed simulation clock (scene-side clocks are
lab-only fallbacks or per-tick authority reconciliations). Follow-up verification
confirmed gadget and ability cooldowns are authority-time-based (not tick-derived), so
they cannot bypass rate limits; transport-level duplicate-event dedup remains Phase 8
scope.

## Goal B deterministic collision/placement continuation — 2026-08-04

Runtime source `a5fdde8` adds the Core `ArenaCollisionDefinition` and deterministic bounds/
ordered-obstacle solver, routes production movement and current fighter/gadget
displacements through canonical positions, and rejects remote Maya/Tiffin placement
authority. Repository validation is **0 errors / 0 warnings**; full EditMode is
**109/109** and full PlayMode is **55/55**. The default Bazaar collision contract has
no authored obstacles yet. Exact platform candidate `7ad7e42` (docs-only after the
runtime commit) now has fresh Android/Web smoke evidence. Goal B is therefore still
**In progress**, not a product-completion pass.

| Area | Status | Evidence / boundary |
| --- | --- | --- |
| Unity project and package baseline | Passed with evidence | Unity `6000.5.6f1`; latest validated repository HEAD `af2e0d8`, exact platform candidate `7ad7e42`, runtime source `a5fdde8`; Photon Fusion `2.1.1 Stable 2177`; Input System-only handler with legacy-scene compatibility bridge; repository validation clean; authored Unity 6 object-lookup warnings removed; see `Docs/QA/LATEST_HEAD_BASELINE.md` |
| Photon Fusion import | Passed with evidence | Fusion 2.1.1 stable build 2177 is present and imported; no public multiplayer claim |
| EditMode and PlayMode regression baseline | Passed with evidence | Exact Goal B source `a5fdde8` passes **109/109 EditMode** and **55/55 PlayMode** tests, including deterministic collision/placement and authority-driven Pehel regressions; see `Docs/QA/LATEST_HEAD_BASELINE.md` |
| Android smoke build | Passed with evidence | Exact platform candidate `7ad7e42` APK (**94,028,145** bytes; SHA-256 `FA0CB54C04DC9309D8B21DAE02CE1D3D8A9961DA1C77F5ADE47F0B6AD280053A`) installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`); Unity activity remained top-resumed. Memory snapshot: 402,013 KB PSS / 537,532 KB RSS / 82,088 KB Graphics / 40 KB swap. This is a development smoke artifact, not a size/performance pass. |
| Web smoke build | Passed with evidence | Exact platform candidate `7ad7e42` Web build contains 19 files / 133,747,764 bytes including development debug-information text; `Web-BazaarBastion.wasm` is 121,033,616 bytes (SHA-256 `D84155637B493182BF380FF91A9ED0D49ECE8F684FAE08E1FA85F0A68F318708`). Local HTTP returned 200 for page/data/WASM. Chrome and Edge Playwright canvas smoke reached menu, mode, fighter-selection and active-match routes at desktop and portrait probes; consoles had 0 errors plus one known Unity persistent-data-path deprecation warning after match load. This is a development smoke artifact; compressed transfer, cold/warm load, mobile interaction and final performance remain open. |
| Timeout/winner correctness | Passed with evidence | Deterministic timeout ranking and complete placements implemented; phase-1 EditMode 59/59 and PlayMode 27/27 pass |
| Eliminations and match statistics | Passed with evidence | Instigator-aware combat events now record damage dealt, eliminations, deterministic non-finisher assists, survival time and duplicate-credit prevention; fresh EditMode 90/90 and PlayMode 43/43 pass |
| Explicit fixed simulation clock | In progress | Goal A's canonical match-controller tick event drives production attack, bot, Bijli, Pehel, Maya and gadget command/ability steps. Local MovementLab clocks and presentation projectile timing remain intentionally separate. Replay recording, broader consolidation and soak coverage remain open |
| Continuously interpolated Aandhi | Passed with evidence | Warning/closing state, next-radius preview and deterministic interpolation are exposed; EditMode 70/70 and PlayMode 27/27 pass |
| Bot current/next-zone awareness | Passed with evidence | Bot snapshots carry explicit current/next zone centre/radius data and proactively reposition from the fixed clock; EditMode 71/71 and PlayMode 27/27 pass |
| Authoritative rule separation | In progress | Goal A makes production attack configuration, phase/protection/tick/sequence checks, canonical origin/direction and cooldown ownership authority-driven. Goal B adds deterministic bounds/ordered-obstacle collision and canonical production movement, Bijli/Pehel/Dhol displacement, Maya placement and Tiffin placement. The default Bazaar definition still has no authored obstacles; projectile collision, remaining presentation adapters, stable event IDs, atomic resolution and network transport remain open |
| Bazaar Bastion production vertical slice | Passed with evidence | `BazaarBastion.unity` has a dedicated `BazaarBastionScene` contract, zero `MovementLabScene` markers and a connected `Content/Prefabs/BazaarArchitecture.prefab`; full 100/100 EditMode and 51/51 PlayMode pass after prefab extraction. Existing Lava/Web smoke evidence remains from `d993a5b`; actor prefab extraction, greybox replacement and human review are required |
| Fighter roster, progression, and complete offline loop | In progress | Common ability/movement interfaces select fighter-specific Pehel Charge Throw and Maya Decoy adapters; production Pehel charge/capture/damage/throw, attack-command acceptance and Bazaar gadget pickup/use now have authority-routed PlayMode coverage. Goal B routes current displacement/placement through the collision authority. Production bot spawn protection is covered by a dedicated PlayMode regression. Latest regression is 109/109 EditMode and 55/55 PlayMode. Progression, full-loop reliability/soak, final presentation and audio remain |
| Visual/audio placeholder foundation | Passed with evidence | `FighterPresentation` supplies replaceable colour rings, health bars, code-driven action states, attack/ability telegraphs and hit/elimination feedback; `BattleRajaAudioDirector` supplies original procedural cues, volume hooks and Web gesture-gated startup. Final art, animation clips, VFX, authored audio and visual approval remain open |
| Canvas match UI foundation | Passed with evidence | `OfflineMatchHud` provides anchored match/zone status, pause/settings, spectator, full-placement results/rematch, locally persisted presentation settings and a functional bounded aim-assist toggle. Pure aim-assist/results tests plus fresh 94/94 EditMode and 45/45 PlayMode regression pass; localization assets, controller rebinding and human UI approval remain open; see ADR-024, ADR-040 and latest-head evidence |
| Production flow and fighter selection | Passed with evidence | `ProductionFlowMachine` is pure/application-owned and covered by 81 EditMode tests. Bootstrap Canvas navigation covers main menu, offline/online mode selection, fighter selection, async match loading, explicit service error/retry, settings and safe-area/focus behavior. `TutorialStepMachine` and `TutorialArenaPlayModeTests` cover the replayable tutorial route; Web screenshots in `Docs/QA/Visual/Flow/` show the 1280×720 route. Full match-loop reliability, Lava smoke for the exact APK and final authored UX remain open |
| Visual and interaction QA | In progress | Exact candidate `7ad7e42` Web Playwright smoke reports 0 errors and one known Unity persistent-data-path deprecation warning after match load; inspected captures include menu at 1280×720, 1024×768 and 390×844, mode/fighter selection, and active match in Chrome and Edge. Lava menu capture is also inspected. Gadget use, loading-state human observation, touch ergonomics, multi-browser coverage and final human approval remain open |
| Real Photon multiplayer | Not started | Imported SDK is not an adapter or multiplayer validation |
| PlayFab/backend/economy | Not started | No production backend claim |
| Performance, soak, multi-browser, and release gates | In progress | Current bounded active-match Lava sample records 460,165 KB PSS, 597,680 KB RSS, 101,480 KB Graphics and 240 KB swap; process samples report 87% instantaneous `top` CPU and 50% user / 13% kernel in `dumpsys cpuinfo`. Chrome 150 local Web sample records 120,872,306-byte WASM transfer and 5.603 ms mean browser rAF. Unity 6 obsolete PlayerSettings and object-lookup calls are now removed from authored code and the fresh Android/Web compile logs contain 0 CS0618 lines. These are smoke observations; frame-time/FPS/GPU/GC, repeated-match growth, thermal/battery, cold-load and multi-browser release measurements remain open; see `Docs/PERFORMANCE_BUDGET.md` and `Docs/QA/Performance/runtime-smoke-20260803.md` |
| CI, security and release preparation | In progress | Read-only static validation/LFS/secret checks are defined in `.github/workflows/repository-validation.yml` and documented in `Docs/CI.md`; Unity licensed tests/builds, artifact retention, dependency review, AAB/signing, publication and legal/privacy approval remain owner-gated |
| Visual/audio/UI approval | Human review required | Current smoke screenshots show greybox/prototype presentation |

## Documentation discrepancy

The requested `Docs/AI/RepositoryAuditAndCompletionGoal.md` path does not exist. The available matching file `Docs/AI/BattleRaja_Repository_Audit_and_Completion_Goal.md` was read completely and used as the continuation brief. A human should decide whether the filename should be normalized later.
