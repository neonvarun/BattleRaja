# BattleRaja V1 Product Rebuild Audit

**Audit date/time:** 2026-09-01 00:03 IST
**Source audited:** `56313096d0ad8e2e23468d004eaa77d71ed3a233` (`origin/main`)
**Branch:** `codex/v1-playstore-release`
**Device scope:** approved Lava `ST5GDW23LB004392` only; latest rerun did not discover it
**Classification:** Prototype — Bastion Crown implementation checkpoint; release work remains

This audit supersedes the pre-implementation audit at `3bed64e`. It describes what is
actually in the latest source, what the automated evidence proves, and what a new Goal
session must still build or verify. It is deliberately not a release claim.

## Continuation evidence — 2026-09-01 01:30 IST

The earlier checkpoint below is historical. The current working tree has now addressed the
specific replay/authority and basic squad-intent gaps it listed:

- static validation **0/0**, EditMode **155/155**, PlayMode **94/94**;
- versioned Bastion replay capture with a coherent post-tick boundary, deterministic combined
  digest and a two-seed/8,400-tick soak with zero divergence;
- explicit dead-target event-id rejection, completed-deposit cleanup, ready-state Crown clock
  guard and mirrored Aandhi state;
- deterministic squad intent coverage across 32 seeds: contest 64, escort 64, defend 96,
  collapse 64 and Aandhi-retreat 32;
- original menu feature art at `Assets/BattleRaja/Art/V1/BattleRaja-FeatureArt-OriginalCandidate.png`
  replacing the vehicle/racing-like historical candidate in the runtime.

Fresh APK/AAB, checker, Lava route and bounded performance facts are recorded in
`Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-01.md`. This is still a generated/presentation
baseline and technical release candidate, not Play-ready: final authored 3D/animation/audio,
cultural/accessibility/fun approval, complete physical route, normalized endurance, physical
16 KB runtime, permanent identity/signing, privacy/Data Safety/IARC and Play actions remain
open.

## Executive assessment

The latest commit is a meaningful product change, not merely a menu rename. BattleRaja now
has a first-class offline Bastion Crown 4v4 contract, eight canonical actors, explicit teams,
shared tickets, Crown pickup/drop/deposit, KOs/assists, protected respawn, overtime/draw
resolution, a Unity adapter, team HUD/objective telegraphs and a player-facing offline route.
The existing Solo authority/replay foundation remains useful.

The implementation is still a technical checkpoint. The team state is mirrored alongside
the legacy combat simulation, bot coordination is only a basic role-to-destination intent,
and the tests cover happy paths rather than the full adversarial/timing/replay matrix. The
current authored-looking output is a provenance-safe generated baseline, not final approved
character, map, animation, VFX, audio or store presentation. Lava, normalized performance,
physical 16 KB behavior, permanent identity/signing and Play/legal gates remain open.

## Evidence reviewed

- `AGENTS.md`, `PROJECT_STATUS.md`, `PROJECT_CONTEXT.json` and the mandatory architecture,
  vision, culture, asset, audio, QA, performance and release documents.
- `Assets/BattleRaja/Core/Domain/BastionCrownContracts.cs` and `BastionCrownMatch.cs`.
- `OfflineMatchAuthority`, `OfflineMatchController`, `BotBrain`, `ProductionFlowController`,
  `OfflineMatchHud`, `BastionCrownObjectiveView`, `BuildEntrypoints` and `BazaarBastion.unity`.
- `BastionCrownMatchTests.cs` and the updated `VerticalSlicePlayModeTests.cs`.
- Current XML test evidence under `Builds/Local/V1GameplayTruth/TestResults/`.
- Candidate APK/AAB and Android 16 AVD ANGLE captures under `Builds/Local/V1GameplayTruth/AndroidQA/emulator-5556/`.
- 2026-08-31 observation-only public entry surfaces of Brawl Stars and Smash Karts on Lava;
  those observations remain high-level design research, not source or asset input.
- Official Android/Play sources logged in `Docs/RESEARCH_LOG.md`; the next session must
  recheck them before changing release settings or declarations.

## Reproduced baseline

| Gate | Result | Evidence and interpretation |
|---|---:|---|
| Repository validation | 0 errors, 0 warnings | `Tools/Validation/validate.ps1` on 2026-09-01 |
| Bastion EditMode rerun | 148/148 | Includes seven pure `BastionCrownMatch` tests; good local rule baseline |
| Bastion PlayMode rerun | 94/94 | Canonical composition, objective telegraphs/HUD and adapter pickup/deposit path |
| Solo replay/soak fixture | 141/141 | Valuable compatibility evidence, not Bastion replay proof |
| APK | 41,438,372 bytes | SHA-256 `6EDC5C0E5D304529A6059A94F00F7AB32AB9C71A4464044D3B0D3ED5D3E2C507` |
| AAB | 37,263,881 bytes | SHA-256 `3D12A358E0F9159A2CA3749A4E53DBB712AF19B0FCCC6E6D80F96DE5944508EE` |
| Local Android technical checks | passed | Offline manifest, ARM64-only/static 16 KB alignment and store dimensions |

The non-rerun integration XMLs contain transient failures and are not promoted as current
green evidence. Any code or asset change requires a fresh full run.

## Quality scorecard

| Area | Current evidence-backed status | Severity | Next decision |
|---|---|---:|---|
| Pure authority contracts | Explicit teams/objective/tickets/respawn/score/result exist; tests green | Medium | Harden ownership and edge cases; preserve Solo seams |
| Unity integration | Canonical 1+3 versus 4, HUD, telegraphs and pickup/deposit route work in PlayMode | High | Prove no mirror drift, double events or unsafe respawn ordering |
| Replay/determinism | Bastion v2 post-tick digest and two-seed/8,400-tick soak are green; broad network parity remains out of scope | Medium | Extend multi-seed duration and production-route replay coverage when needed |
| Squad AI | Deterministic role/escort/defend/collapse/Aandhi-retreat planner coverage is green; full match-balance and human review remain open | High | Add production-match metrics, difficulty tuning and human fairness review |
| Fighter kits/combat | Bijli/Pehel/Maya and gadgets have existing authority seams | High | Tune for team objective, ally support and mobile counterplay |
| Character assets | Owned generated/faceted baseline and saved rigs/clips | High | Finish distinct modeled/rigged/animated assets; device proof |
| Bazaar Bastion | Collision-compatible regenerated scene and readable baseline | High | Author lanes, cover, landmarks, shrines/sockets and quality tiers |
| Crown/gadgets/VFX | Objective view and mechanics exist; feedback/boundaries incomplete | High | Test interruption/rotation and make every state readable/reduced-flash safe |
| UI/UX | Offline Bastion menu, selection, HUD, results/rematch/settings shell exists | High | Remove prototype residue; complete safe-area/accessibility/device review |
| Audio | Owned WAV/mixer routing exists | High | Author/mix priority, loudness, haptics and event coverage on Lava |
| Performance | Static checks plus bounded six-sample Lava evidence; no normalized Unity frame histogram or endurance approval | Blocker | Profile final-art candidate on Lava: frame/GC/memory/thermal/battery/endurance |
| Android/Play | API 36, ARM64 and static checks pass; temp debug identity | Blocker | Prepare accurate drafts; owner handles identity/signing/legal/upload |

## What is now implemented and worth retaining

1. `BastionCrownContracts` keeps mode, team, role, objective, ticket, respawn, score and
   result definitions immutable and data-shaped.
2. `BastionCrownMatch` owns mutable team-mode state for exactly eight actors, including
   event-id deduplication, Crown carrier/drop state, KO/assist attribution, tickets,
   respawn protection, overtime and explicit draw results.
3. The production route is `PLAY OFFLINE` → Bastion Crown briefing → fighter selection →
   ready/live arena → result/rematch. `BuildEntrypoints` regenerates actor IDs 1–8 and
   team-aware scene composition.
4. The controller adapter preserves common combat commands and legacy Solo compatibility;
   the new objective view is render-only and does not own score or collision.
5. Existing offline authority, fighter/gadget definitions, lifecycle handling, provenance
   manifests, local preferences and deterministic Solo tests remain valuable foundations.

## Confirmed or likely release-blocking gaps

### Authority and rules

- `OfflineMatchController` mirrors health/position/damage from the legacy simulation into
  `BastionCrownMatch`. The next agent must prove a single authoritative event order for
  damage, defeat, Crown drop, respawn, result and replay; do not assume a mirror is safe.
- Deposit is documented as interruptible, but the pure path currently cancels on death or
  leaving range and has no tested combat-damage interruption. Decide and implement one
  canonical rule, then add boundary and duplicate-event tests.
- Deposit resets the Crown to the current socket. Verify whether “after a deposit” means
  rotate immediately or restart that socket's cadence; record the decision and test it.
- Overtime currently compares overtime deposits only, otherwise draws. Prove that this is
  the intended sudden-death Crown rule and that the documented tie-break order is consistent.
- `HealingDone` is exposed in result data but has no confirmed Bastion event bridge. Either
  wire ally healing/stat attribution or remove the promise from results with evidence.
- `ConfirmRespawn` is present while the Unity adapter currently drives legacy respawn from
  the tick. Prove there is no stale state, duplicate revive or accidental ninth actor.

### Team AI and play quality

- `TryGetBastionBotIntent` currently chooses a destination and a broad plan. It does not
  yet provide a team blackboard, formation/spacing, cover selection, ally peel/heal,
  carrier escort handoff, ticket-risk strategy, regroup timing or robust Aandhi retreat.
- Difficulty must vary reaction, risk and coordination quality under the same information
  model. The current autonomous-bot weapon scaling is a documented PvE policy but must be
  reviewed for fairness and must not become hidden damage/health/cooldown/vision cheating.
- Add deterministic multi-seed simulations with metrics for Crown time, deposits, KOs,
  ally support, distance/spacing, gadget use, stalemates, team wipes and match duration.

### Content and presentation

- Generated/procedural meshes, textures, rigs, clips, VFX and WAVs are useful owned baseline
  assets, but they do not yet prove final authored silhouettes, animation personality,
  material hierarchy, audio mix or cultural/fun approval.
- The map needs device-tested lanes, flanks, cover, landmarks, readable shrine/socket
  affordances and quality-tier behavior while preserving authority collision.
- Audit every Bastion screen for hidden Solo/debug vocabulary, placeholder glyphs, clipping,
  text scale, touch targets, left-handed layout, reduced-flash/high-contrast behavior,
  pause/resume and results/rematch clarity.

### Device, performance and release

- Fresh Lava menu → Bastion → fighter → live/settings evidence now exists under
  `Builds/Local/V1GameplayTruth/Final/lava-20260901-final/`; the Android 16 AVD ANGLE route remains
  emulator-only and is not a substitute for physical 16 KB proof.
- No normalized final-art CPU/GPU/frame histogram, GC spike, memory-growth, thermal,
  battery or repeated-rematch endurance report exists for this checkpoint.
- Lava's 4 KB page environment and static host alignment are not physical 16 KB runtime proof.
- Package `com.example.battleraja.m11` is temporary/debug-signed. Permanent package ID,
  publisher identity, signing key, privacy policy, Data Safety, IARC/content rating,
  support URL, store copy and Play Console upload remain owner-controlled.

## Required continuation order

1. Rebaseline source, worktree, Unity, artifacts, device and policy; update this audit if
   the repository has advanced.
2. Harden `BastionCrownMatch`/adapter ownership and fill rule, replay, edge-case and soak tests.
3. Implement and measure fair squad AI; tune the three fighters/gadgets only after team
   behavior is observable.
4. Finish original modeled/rigged/animated characters, authored Bazaar map, objective/gadget
   visuals, VFX, UI, audio and tutorial/accessibility; keep a provenance trail.
5. Profile the final-art candidate, then run the complete approved Lava route and capture
   evidence for all fighters, gadgets, Aandhi, KO/respawn/spectator, results/rematch,
   settings, lifecycle and offline behavior.
6. Prepare technical/store/privacy drafts and a release AAB. Stop before signing, legal
   approval, identity choice, agreement acceptance, upload or rollout.

## Binary release exit condition

Do not classify the project as a Play Store Release Candidate until the same final-art
source/build has: player-visible 4v4 behavior from cold launch through rematch; tested
authority/replay/AI edge cases; distinct authored characters and map; complete gadget,
objective, VFX, UI, audio and accessibility treatment; normalized physical-device
performance; physical 16 KB evidence or a clearly named external blocker; valid release
AAB checks; accurate store/privacy drafts; and no critical known defect. Until then the
truthful classification is **Prototype — Bastion Crown implementation checkpoint**.
