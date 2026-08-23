# BattleRaja — Fresh-Agent Goal-Mode Master Handoff

> **Target agent:** Codex / Luna 5.6 Max  
> **Recommended mode:** Goal  
> **Workspace:** `C:\Projects\BattleRaja`  
> **Repository:** `neonvarun/BattleRaja`  
> **Primary targets:** Android + Unity Web  
> **Unity:** `6000.5.6f1`

---

# 0. Fresh-Agent Operating Context

You are a fresh implementation agent with zero prior conversation context.

Work in:

```text
C:\Projects\BattleRaja
```

The project is **BattleRaja**, owned by **Varunkumar Singh / Avinya Studios**.

Your job in this session is to understand the current repository deeply, verify the exact current source rather than trusting historical documentation, and continue the project through the next bounded engineering closure:

1. exact-current-source rebaseline,
2. remaining event-identity plumbing,
3. atomic authority closure,
4. replay/determinism/soak completion,
5. exact Android + Web regression evidence.

Do **not** begin Photon or PlayFab in this execution.

Do **not** attempt to rebuild the game from scratch.

Do **not** restart M0–M11.

Do **not** assume a document saying “Passed” means a system is actually complete.

The repository, source code, tests, exact-current builds, runtime behavior, device evidence, browser evidence, and reproducible artifacts are the source of truth.

---

# 1. Product We Are Building

BattleRaja is an original stylized top-down 3D micro battle royale.

The intended player-facing game is:

- Fast roughly 4–6 minute survival matches.
- One compact original launch arena: **Bazaar Bastion**.
- Eight total combatants.
- One human + seven bots in offline Solo Raja mode.
- Three canonical launch fighters:
  - **Bijli** — mobile electric damage fighter.
  - **Pehel** — tank/grappler with charge, capture, and throw.
  - **Maya** — trickster using a decoy.
- Three temporary Jugaad Gadgets:
  - Umbrella Guard.
  - Dhol Burst.
  - Tiffin Station.
- A closing **Aandhi** zone that pressures players toward encounters.
- Basic attacks, fighter abilities, gadgets, healing, damage, eliminations, placements, assists, spectator mode, results, and rematch.
- Android touch twin-stick controls.
- Desktop Web keyboard/mouse support.
- Shared gameplay rules across Android and Web.
- No pay-to-win combat.
- Progression, when added later, must not sell combat power.
- Original fictional South-Asian-inspired worldbuilding.
- No sacred misuse, political caricature, stereotypes, or copied protected characters, sounds, arenas, UI, or distinctive designs.

The game must **not** turn into:

- A PUBG-scale battle royale.
- A realistic military shooter.
- A Brawl Stars clone.
- A Smash Karts clone.
- A vehicle-only game.
- A photorealistic game.
- A gacha-first/pay-to-win game.
- A public live-service release without legal, privacy, security, signing, infrastructure, and human approval gates.

---

# 2. Intended Player Journey

The intended complete player loop is:

1. Unity loading screen with useful progress.
2. Main menu.
3. Mode selection.
4. Fighter selection.
5. Match loading.
6. Offline Solo Raja match:
   - Spawn/warm-up.
   - Movement and aiming.
   - Basic attack.
   - Fighter ability.
   - Gadget pickup and use.
   - Aandhi warning and closure.
   - Combat, damage, healing, and eliminations.
   - Spectator mode after elimination.
   - Winner and complete placements.
7. Results:
   - Placement.
   - Eliminations.
   - Damage dealt.
   - Survival time.
   - Assists where authoritative.
   - Rematch.
   - Return to menu.
8. Settings:
   - Left-handed controls.
   - Reduced flashes.
   - High contrast.
   - Text scaling where practical.
   - Music/effects volume.
   - Aim assist.
   - Tutorial replay.
9. Tutorial:
   - Movement.
   - Aim.
   - Attack.
   - Fighter ability.
   - Gadget pickup/use.
   - Aandhi.
   - Elimination.
   - Victory.
   - Replay/rematch.

The current visual target is a polished closed-alpha presentation, not AAA final art.

Replaceable original placeholder art is acceptable while systems are being completed.

---

# 3. LAST-KNOWN REMOTE SNAPSHOT — VERIFY, DO NOT ASSUME

At the time this handoff was prepared, the remote repository was verified as:

- Repository: `neonvarun/BattleRaja`
- Remote branch: `main`
- Remote `main` HEAD:
  `4f0abd95fa22ed3f87a08b1a07f0ea244ff08011`
- Commit:
  `chore: add Unity meta for deterministic soak tests`
- Date: 2026-08-22
- Remote branch search returned only `main`.

This is **last-known information**, not permission to assume your local checkout still matches it.

Before doing anything, independently verify:

- Working directory.
- Local branch.
- Local HEAD.
- `origin/main`.
- Whether remote main advanced beyond `4f0abd95`.
- Worktree cleanliness.
- Untracked files.
- Git LFS state.
- Existing local branches.
- Existing stashes.
- Connected devices.
- Installed browsers.
- Unity version.

If actual current state differs from this snapshot, **current reality wins**.

---

# 4. Important Changes Already Landed Since the Old August 4 Snapshot

Do not blindly redo these.

Since old `main` commit `d64da367...`, the remote branch advanced by at least 16 focused commits before this handoff.

Important work already present in current `main` includes:

## Projectile presentation reconciliation

Commit family around:

```text
60cb5b0 — feat: reconcile projectile views with authority snapshots and retune match pacing
```

This work:

- Binds production projectile views to authority projectile IDs.
- Reconciles visual shells from `DomainProjectileSnapshot`.
- Retires production projectile views from authority despawn state.
- Uses canonical hit/despawn reasons for impact feedback.
- Keeps the older local MovementLab/non-authority projectile path for lab fixtures.
- Wires `OfflineMatchController` to reconcile projectile snapshots.

Do not reintroduce a second production-authority projectile simulation.

Verify the implementation and tests before changing it.

## Outer-wall raycast correction

Commit family around:

```text
355a9b0 — fix: repair stale collision/projectile fixtures and restore local lab projectile damage
```

This includes a fix for rays/projectiles starting inside the play area not detecting the forward outer-wall exit intersection.

Do not assume the old August 4 outer-boundary bug still exists.

Verify current tests and implementation.

## Bot-perception fix

Commit:

```text
35d723f — fix: bot perception no longer treats fighter hulls as line-of-sight blockers
```

Bots previously failed to engage because target actor colliders incorrectly occluded line of sight.

Current implementation should ignore actor hull hits while preserving true world-cover occlusion.

Verify; do not rewrite unless a real current defect exists.

## Balance correction

Commits:

```text
60cb5b0
6de9982
17a8c75
```

Current intended basic-attack damage targets are recorded as approximately:

- Bijli bolt: 12
- Pehel sweep: 20
- Maya shard: 9

The critical correction at `17a8c75` moved those values into authoritative Core definitions so editor regeneration/build tooling no longer silently reverted serialized assets.

Do not retune these values merely because historical assets/docs differ.

Any balance change must be evidence-backed and recorded in `Docs/BALANCE_CHANGELOG.md`.

## Scene serialization/test-artifact cleanup

Commit:

```text
c78433a — chore(scene): sync serialized scene fields and drop tracked test artifacts
```

This removed recurring Unity Test Framework performance artifacts from tracking and synchronized expected serialized scene fields.

Do not re-add generated performance JSON files or unnecessary Unity churn.

## Stale attack-tick authority fix

Commit:

```text
ee573ad — authority: reject stale attack ticks and anchor cooldowns to the match clock
```

Current authority should:

- reject attack ticks outside the accepted stale window,
- distinguish `StaleTick`,
- anchor cooldown consumption to the canonical authority clock.

Commit `2d7f566` subsequently documented that gadget/ability cooldowns are authority-time-based and do not share the same stale-tick bypass.

Verify instead of reimplementing.

## Collision edge-case verification

Commit:

```text
669c4a9 — collision: make IsPointBlocked solver-consistent and pin corner/thin-wall behavior
```

Current code includes fixes/tests for:

- contact-boundary floating-point tolerance,
- diagonal corner behavior,
- thin-wall tunneling prevention,
- seeded movement remaining outside obstacle footprints.

Verify the current solver before altering it.

## Stable damage event IDs

Commit:

```text
fa8f46e — authority: assign stable identities to recorded damage events
```

Damage events applied through the match simulation now receive stable sequential IDs after validation.

Same-tick damage from different attackers should remain distinct.

This closes the old “damage IDs completely absent” claim.

However, healing, collection, elimination, gadget-use, ability, and other event identities still need a current-source audit.

Do not assume the entire event-identity system is complete.

## Genuine reproducible deterministic soak

Commit:

```text
aa59f60 — soak: replace unverifiable soak claims with reproducible 1000-match evidence
```

A real deterministic soak now exists:

- `Assets/BattleRaja/Tests/EditMode/DeterministicSoakTests.cs`
- 1,000 seeded matches.
- Each seed executed twice.
- 2,000 full accelerated match executions total.
- Per-tick hash streams compared.
- Zero recorded divergence in the documented deep run.
- Deep run duration recorded around 406.8 seconds.
- Full EditMode suite documented as 120/120 after this addition.
- Phantom fourth fighter “Raja” removed from soak documentation.

This is real progress.

Do not recreate a fake soak report.

But the current soak has known scope limits:

- It exercises movement + attacks.
- Gadgets/pickups are not yet configured into deep soak.
- Same-machine hash parity does not prove cross-machine floating-point determinism.
- EditMode soak does not by itself prove memory leak freedom.

---

# 5. Current Truthful Classification

Treat BattleRaja as:

**PROTOTYPE / CLOSED-ALPHA FOUNDATION**

Do not call it:

- closed alpha,
- release candidate,
- production ready,
- multiplayer ready,
- backend complete,

unless fresh evidence in this session satisfies the relevant gates.

The latest product-status documentation still classifies the project as **prototype**.

---

# 6. Documentation Reliability Rules

Several repository documents contain useful historical evidence but may lag current `main`.

This is expected.

## Source-of-truth order

When claims disagree, use this priority:

1. Exact current source code.
2. Exact current automated tests.
3. Exact current runtime behavior.
4. Exact current Android/Web artifacts.
5. Reproducible logs/profiler/browser/device evidence.
6. Current architecture decisions.
7. Current status documents.
8. Historical milestone/status documents.

Documentation must describe proven implementation.

Implementation must never be altered merely to make an old completion claim appear true.

## `START_HERE.md`

`START_HERE.md` is historical Milestone-0 onboarding material.

Read it for project history only.

Do **not**:

- restart Milestone 0,
- paste `PROMPTS/00_MILESTONE_0_BOOTSTRAP.md`,
- wait for Milestone 1 approval,
- conclude gameplay is intentionally absent.

Current source, this handoff, `Docs/MASTER_VISION.md`, and `AGENTS.md` supersede obsolete starter instructions.

## `Docs/CLOSED_ALPHA_COMPLETION_STATUS.md`

Treat all entries as claims requiring verification.

Older versions of this file overclaimed multiple Antigravity stages and contained local `file:///c:/...` links.

Do not inherit a “Passed with evidence” status automatically.

## `PROJECT_STATUS.md`

Useful, but it can lag the remote tip.

At the time of this handoff, remote main had advanced to `4f0abd95`, while portions of `PROJECT_STATUS.md` still described `c78433a` or intermediate Phase 3 state as latest validated evidence.

Verify current source.

## `Docs/PRODUCT_COMPLETION_STATUS.md`

This contains useful Phase 0/1/2/3 audit notes, but parts can be stale relative to commits that landed later the same day.

For example, an older Phase 3 note says production damage-event IDs were not stamped; commit `fa8f46e` subsequently added stable IDs to recorded damage events.

Read chronologically and verify current source.

## `Docs/QA/LATEST_HEAD_BASELINE.md`

This is valid historical exact-source evidence for the commits it names.

It is **not automatically evidence for `4f0abd95` or any newer current HEAD**.

Fresh current-source builds are required.

---

# 7. Mandatory Architecture Rules

These rules must remain true.

## Core Domain/Application

`BattleRaja.Core.Domain` and `BattleRaja.Core.Application` must remain:

- Unity-free.
- Photon/Fusion-free.
- PlayFab-free.
- Presentation-free.
- Deterministic where gameplay authority requires it.
- Testable without a Unity production scene.

Core should own authoritative:

- Match state.
- Match tick.
- Participant roster.
- Fighter/loadout/weapon configuration.
- Command validation.
- Movement rules.
- Arena collision.
- Ability rules.
- Projectile entities/travel/collision.
- Target selection.
- Damage/mitigation.
- Healing.
- Aandhi damage.
- Pickup collection.
- Gadget inventory/use.
- Elimination.
- Placement.
- Result statistics.
- Stable event identities.
- Replay inputs.
- State hashes.
- Immutable snapshots/events.

## Presentation

Presentation may:

- Collect player input.
- Collect bot observations.
- Submit commands.
- Render authoritative snapshots/events.
- Render actors/projectiles/VFX/HUD/audio.
- Perform visual interpolation.
- Maintain local presentation settings.
- Handle Unity/device/browser lifecycle.

Presentation must not authoritatively decide:

- Damage.
- Health.
- Position.
- Collision result.
- Projectile hit.
- Pickup ownership.
- Gadget validity.
- Cooldown.
- Elimination.
- Winner.
- Rewards.
- Match result.

Local MovementLab/test fixtures may retain explicitly non-authoritative presentation simulation where required by historical lab tests, but production Bazaar gameplay must not depend on it.

## Infrastructure

Infrastructure may adapt:

- Android.
- Web.
- Persistence.
- Analytics seams.
- Photon/Fusion.
- PlayFab.
- Platform lifecycle.

Infrastructure must not move gameplay authority out of Core.

## General authority rules

- All incoming commands are untrusted.
- Same-tick events require deterministic ordering.
- Duplicate/retransmitted events must be rejectable safely before networking.
- Runtime mutable state must not live in shared ScriptableObject assets.
- Avoid global mutable singletons.
- Avoid hot-path runtime object searches.
- Unity physics may support presentation/labs but cannot decide production authoritative outcomes.
- Do not introduce browser-authoritative competitive gameplay.
- Do not put Photon/PlayFab references into Core.

---

# 8. Mandatory Reading Before Runtime Changes

Read completely where present:

## Root

1. `AGENTS.md`
2. `README.md`
3. `START_HERE.md` — historical only
4. `PROJECT_STATUS.md`
5. `PROJECT_CONTEXT.json`
6. `.gitignore`
7. `.gitattributes`

## Product/architecture

8. `Docs/MASTER_VISION.md`
9. `Docs/ARCHITECTURE.md`
10. `Docs/DECISIONS.md`
11. `Docs/RESEARCH_LOG.md`
12. `Docs/BALANCE_CHANGELOG.md`
13. `Docs/PRODUCT_COMPLETION_STATUS.md`
14. `Docs/CLOSED_ALPHA_COMPLETION_STATUS.md`

## QA

15. `Docs/QA/LATEST_HEAD_BASELINE.md`
16. `Docs/QA/REPLAY_AND_SOAK_REPORT.md`
17. `Docs/QA/VISUAL_QA_REPORT.md`
18. Any newer files under `Docs/QA/`
19. `Docs/PERFORMANCE_BUDGET.md`
20. `Docs/TEST_STRATEGY.md`

## Gates/security

21. `Docs/EXTERNAL_SERVICE_GATES.md`
22. `Docs/HUMAN_REVIEW_BACKLOG.md`
23. `Docs/SECURITY.md`
24. `Docs/CI.md`
25. `Docs/CULTURAL_GUIDE.md`

## Historical planning

26. Relevant files under `Docs/AI/`
27. Relevant milestone reports under `Docs/MILESTONE_REPORTS/`

## First-party source

Read all relevant first-party code under:

- `Assets/BattleRaja/Core/Domain`
- `Assets/BattleRaja/Core/Application`
- `Assets/BattleRaja/Gameplay`
- `Assets/BattleRaja/Presentation`
- `Assets/BattleRaja/Infrastructure`
- `Assets/BattleRaja/Editor`
- `Assets/BattleRaja/Tests`

Inspect:

- Bootstrap.
- Bazaar Bastion.
- MovementLab.
- Tutorial Arena.
- Fighter content.
- Projectile content.
- Gadget content.
- UI.
- Build tooling.
- Validation tooling.

Vendor code under `Assets/Photon` only needs to be inventoried unless later approved networking work genuinely requires deeper inspection.

---

# 9. Git and Workspace Safety

Before changing anything, run:

```powershell
Set-Location 'C:\Projects\BattleRaja'

git fetch --all --prune
git status --short --branch
git branch --show-current
git rev-parse HEAD
git rev-parse origin/main
git log --oneline --decorate --graph -30
git branch -a
git stash list
git lfs status
git lfs fsck --pointers
git diff --check
```

Record:

- Local branch.
- Local HEAD.
- Remote main HEAD.
- Local/remote divergence.
- Worktree changes.
- Untracked files.
- Stashes.
- Git LFS status.

Never:

- `git reset --hard`
- `git clean -fd`
- overwrite user work
- force-push
- rewrite published history
- delete branches/stashes without approval
- commit secrets
- commit signing keys
- commit generated Unity caches/builds/logs
- commit unreviewed giant scene rewrites

If the current workspace contains user-owned changes that cannot safely coexist with this task, create a clean sibling worktree from exact `origin/main`.

Do not silently stash the user’s work.

---

# 10. Working Branch

After verifying the exact current `origin/main`, create a fresh focused feature branch.

Preferred:

```text
codex/authority-atomic-replay-closure
```

If that name already exists, choose a unique suffix.

Do not work directly on `main`.

Do not create a pull request during this execution unless explicitly asked later.

---

# 11. Approved Local Toolchain

Expected Unity executable:

```powershell
$unity = 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe'
```

Verify that path rather than assuming it.

Expected major packages include:

- Input System `1.20.0`
- URP `17.5.0`
- Test Framework `1.7.0`
- Test Framework Performance `3.5.0`
- uGUI `2.5.0`
- Photon Fusion `2.1.1 Stable Build 2177`

Do not upgrade Unity/packages during this task unless a verified current-source blocker genuinely requires it and the change is explicitly justified.

---

# 12. Android and Browser Constraints

## Android

Use **only the connected Lava phone** for physical-device evidence.

Known historical serial:

```text
ST5GDW23LB004392
```

Verify the connected-device list before use.

Do not use the Oppo device.

Do not claim Android success from an old APK.

Install and launch the APK generated from the exact source being validated.

## Web

Use local HTTP serving.

Use Chrome and Edge when installed.

Do not treat `file://` testing as valid.

Do not treat HTTP 200 alone as gameplay validation.

Check the actual Unity canvas and representative interactions.

Firefox/Safari remain unavailable unless actually installed and tested in the current environment.

## CI

Do not rely on GitHub Actions as your primary validation route.

At the time of this handoff, the current remote HEAD had no combined status contexts associated with it.

Use local PowerShell validation, Unity batch mode, Android ADB, and local browser testing.

Do not add Unity licence secrets to CI.

---

# 13. Phase 0 — Exact-Current-HEAD Rebaseline

This is mandatory because the last complete Android/Web baseline predates several August 22 authority/collision/damage/soak commits.

Do this before additional runtime implementation.

## 13.1 Repository validation

Run the repository-supported validation command.

Expected form:

```powershell
pwsh -File Tools/Validation/validate.ps1 `
  -ProjectRoot . `
  -RequireUnityProject `
  -UnityExe $unity
```

Also run:

```powershell
git diff --check
git lfs fsck --pointers
```

## 13.2 Full EditMode

Use exact-current source:

```powershell
& $unity -batchmode -nographics -quit `
  -projectPath . `
  -runTests -testPlatform editmode `
  -testResults Builds/Local/TestResults/editmode-current.xml `
  -logFile Builds/Local/Logs/editmode-current.log
```

Do not assume the historical 120/120 count is still current.

Record actual totals.

## 13.3 Full PlayMode

```powershell
& $unity -batchmode -nographics -quit `
  -projectPath . `
  -runTests -testPlatform playmode `
  -testResults Builds/Local/TestResults/playmode-current.xml `
  -logFile Builds/Local/Logs/playmode-current.log
```

Record actual totals.

## 13.4 Deep deterministic soak

Inspect the current `DeterministicSoakTests` implementation first.

Run the exact current deep soak using the repository-supported mechanism.

Historical form:

```powershell
$env:BATTLERAJA_SOAK_MATCHES='1000'
```

Then run the filtered EditMode soak test.

Record:

- Match count.
- Number of executions per seed.
- Duration.
- Divergence count.
- Exact test XML/log paths.
- Any warning/error.
- Current source SHA.

Clear the environment variable afterwards.

## 13.5 Android exact-source build

Use the repository build script/entry point.

Expected form:

```powershell
pwsh -File Tools/Build/Android/build.ps1
```

Record:

- APK path.
- Size.
- SHA-256.
- Build duration if available.
- Build warnings/errors.

Then:

```powershell
adb devices
adb -s ST5GDW23LB004392 install -r <fresh-apk>
adb -s ST5GDW23LB004392 shell monkey -p <verified.application.id> 1
adb -s ST5GDW23LB004392 shell dumpsys activity activities
adb -s ST5GDW23LB004392 shell dumpsys meminfo <verified.application.id>
```

Check fresh logcat for:

- fatal exception,
- AndroidRuntime,
- native crash,
- SIGSEGV,
- Unity application exceptions.

## 13.6 Web exact-source build

Expected:

```powershell
pwsh -File Tools/Build/Web/build.ps1
```

Serve over HTTP:

```powershell
python -m http.server 8015 --directory <verified-web-build-directory>
```

Verify:

- index loads.
- data/WASM loads.
- main menu renders.
- no blank canvas.
- no fatal console errors.
- no failed essential network requests.

Use Chrome and Edge.

At minimum test:

- 1280×720.
- 1024×768.
- 390×844 experimental portrait sanity.

When practical, navigate:

- menu,
- offline mode,
- fighter selection,
- active match.

## 13.7 Baseline output

Create or update one current-source evidence file, preferably:

```text
Docs/QA/LATEST_HEAD_BASELINE.md
```

Do not erase useful history.

Clearly separate the new current-head section from older evidence.

## Gate

Do not proceed if:

- validation fails,
- compilation fails,
- full EditMode fails,
- full PlayMode fails,
- exact Android build fails,
- exact Web build fails.

Fix blockers first.

---

# 14. Phase 1 — Re-Audit the Latest Authority Delta

Do not repeat old audits mechanically.

Read the exact current source after Phase 0 and classify the following as:

- Verified complete.
- Partially complete.
- Missing.
- Regressed.
- No longer relevant.

## Verify current solved work

Specifically verify that these remain correct:

- Canonical 30 Hz match tick.
- Stale/future attack tick rejection.
- Attack cooldown anchored to authority time.
- Authority-owned weapon/faction/origin/tick-rate.
- Duplicate/out-of-order attack-sequence handling.
- Spawn-protection/warmup/resolution action rejection.
- Bazaar authored obstacle set.
- Thin-wall and corner behavior.
- Authority-owned production projectile travel/collision.
- Authority projectile → presentation reconciliation.
- Outer arena wall collision.
- Bot LOS actor-hull fix.
- Current 12/20/9 attack balance source values.
- Stable damage-event IDs.
- Real deterministic soak test.

Do not change verified systems without a demonstrated defect.

---

# 15. Phase 2 — Finish Stable Gameplay Event Identities

Current main has:

- stable attack-execution IDs,
- stable projectile IDs,
- stable recorded damage-event IDs.

Audit the remaining gameplay events and complete identities where still missing.

Candidates include:

- Ability execution.
- Gadget use.
- Healing.
- Pickup collection.
- Gadget collection.
- Station spawn/use/expiry if transported as events.
- Elimination.
- Match-result transition.
- Any presentation-facing authoritative combat event that could later be retransmitted.

## Requirements

Identity must be:

- assigned only after validation,
- unique within the match lifetime for its event class or composite key,
- deterministic,
- not derived from Unity instance IDs,
- safely reset on match restart,
- compatible with future transport deduplication,
- safe around integer rollover,
- test-covered.

Do not merely call `Next...Id()` without attaching the ID to an actual immutable event/record.

## Duplicate/replay policy

Design the offline structures so networking can later add bounded replay protection without changing gameplay semantics.

Where appropriate introduce:

- event sequence,
- source actor,
- execution ID,
- event ID,
- bounded recent-ID tracking,

but do not build Photon transport yet.

## Tests

Add focused tests for:

- same-tick hits from different actors,
- repeated duplicate event rejection,
- IDs assigned after validation,
- rejected actions not consuming IDs,
- restart reset,
- rollover policy,
- two different event classes on the same tick,
- deterministic order.

---

# 16. Phase 3 — Complete Atomic Match Authority

This is one of the most important remaining architecture tasks.

Current source still exposes authority tick outputs such as:

- `OutsideDamageRequests`
- `GadgetHealingIntents`
- `ExpiredStationIds`
- projectile snapshots

and the current `OfflineMatchController` still performs presentation-layer feedback loops such as:

- routing outside damage through `CombatDamageResolver`,
- calling `_authority.ApplyHealing(...)` for gadget healing,
- calling `_authority.ApplyHealing(...)` after pickup collection,
- applying gadget acquisition through presentation-side `GadgetUser`,
- updating pickup/station presentation separately.

This means the authoritative simulation is not yet fully atomic.

## Target architecture

A canonical match tick should:

1. accept validated commands,
2. resolve movement,
3. resolve abilities,
4. resolve gadget actions,
5. advance projectiles,
6. resolve projectile hits/damage,
7. resolve Aandhi damage,
8. resolve healing,
9. resolve pickups,
10. resolve gadget collection,
11. resolve station state,
12. resolve decoy state,
13. resolve eliminations/assists/placements,
14. resolve match end,
15. emit immutable snapshots/events.

Presentation should **not feed gameplay mutations back into Core after the tick**.

## Required changes

Move authoritative mutation fully inside Core for:

### Aandhi

- Apply outside-zone damage in authority.
- Record damage/event IDs appropriately.
- Do not emit a request requiring Unity to feed damage back.

### Healing

- Tiffin healing mutates canonical health within authority.
- Pickup healing mutates canonical health within authority.
- Emit immutable healing events/snapshots for presentation.

### Pickups

- Collector selection remains authority-owned.
- Availability mutation remains authority-owned.
- Health/inventory mutation occurs in the same authority operation.
- Presentation only shows availability/result.

### Gadgets

- Gadget acquisition/inventory mutation occurs in authority.
- Gadget use validity/cooldown/effect occurs in authority.
- Presentation `GadgetUser` becomes input/view adaptation rather than gameplay owner.

### Station lifecycle

- Spawn.
- Active state.
- Damage.
- Healing.
- Expiry.

must have one canonical owner.

### Health synchronization

`CombatHealth` should be a presentation mirror of canonical health for production matches.

Avoid repeated presentation-side mutations that can diverge from `OfflineMatchSimulation`.

## Event batch

Prefer one immutable tick result containing the authoritative presentation-facing outputs needed for:

- participant snapshots,
- projectile snapshots,
- damage events,
- healing events,
- collection events,
- gadget events,
- elimination events,
- station/decoy changes,
- match phase/result changes.

Do not add unnecessary abstraction layers if the existing architecture can be extended cleanly.

## Direct mutation escape hatches

Audit public methods such as:

- `ApplyHealing`
- `SyncHealth`
- `RecordDamage`
- `SetPosition`
- direct pickup mutation
- direct gadget mutation

Restrict, internalize, remove, or explicitly isolate test-only compatibility paths where safe.

Do not break legitimate test/lab fixtures without replacement.

## Tests

Add tests proving:

- Aandhi damage is applied exactly once.
- Gadget healing is applied exactly once.
- Pickup healing is applied exactly once.
- Gadget collection is atomic.
- Duplicate collection cannot double-grant.
- Eliminations happen within the same authoritative resolution path.
- Presentation cannot double-apply authority events.
- Same-tick damage/healing ordering is deterministic.
- Match restart clears pending events.
- Event batches do not leak across scene reloads.

---

# 17. Phase 4 — Replay, State Hashing, and Soak Completion

Do not throw away the genuine 1,000-match soak already added.

Extend the existing foundation.

## Replay audit

Determine whether the current replay system actually supports:

- recording movement inputs,
- recording attack commands,
- recording ability commands,
- recording gadget commands,
- storing the match seed,
- storing collision/content/protocol version,
- reconstructing an authority instance,
- applying each recorded frame,
- advancing canonical ticks,
- comparing expected vs actual hashes,
- reporting first divergence.

If any item is missing, implement it.

Do not call data containers a complete replay engine unless they execute a replay.

## State hash completeness

The current documented deterministic soak hash includes:

- tick,
- phase,
- zone,
- participant health/positions,
- projectile positions.

Audit and extend the state hash to cover all authoritative gameplay state necessary to catch a real divergence, including where applicable:

- elapsed match state,
- next zone state,
- actor alive/eliminated state,
- placement,
- survival state,
- aim/rotation if authoritative,
- attack cooldowns,
- ability runtime/cooldowns,
- gadget inventory,
- gadget cooldowns,
- pickups,
- gadget pickups,
- stations,
- Maya decoys,
- projectiles,
- projectile remaining range/lifetime,
- damage contribution/assist state where relevant,
- event identity counters,
- deterministic random/seed state,
- match end/winner/result state.

Guarantee deterministic ordering before hashing dictionaries/collections.

## Deep soak extension

The existing 1,000-match soak is useful but currently documented as movement + attack coverage.

Extend deep soak so representative deterministic runs include:

- all three canonical fighters,
- all three gadgets,
- pickup collection,
- healing,
- decoy activity,
- station activity,
- ability use,
- Aandhi,
- simultaneous hits,
- simultaneous eliminations,
- timeout/late-match paths.

Do not necessarily execute every combination in every seed if that would be wasteful.

Use a deterministic scenario matrix with explicit coverage.

## Repeated runtime lifecycle

Keep/extend PlayMode regressions for:

- repeated rematches,
- repeated Bazaar scene loads,
- one runtime graph,
- one authority,
- one canonical clock,
- no duplicate subscriptions,
- projectile-pool cleanup,
- station cleanup,
- decoy cleanup,
- pickup reset,
- event-buffer reset.

## Cross-run limits

Document honestly:

- same-process determinism,
- same-machine determinism,
- cross-render-rate determinism,
- cross-machine determinism,
- Android/Web equivalence,

and do not claim categories you have not tested.

---

# 18. Phase 5 — Exact-Source Regression Evidence After Authority Changes

After Phases 2–4 are complete:

Run again:

- repository validation,
- `git diff --check`,
- Git LFS integrity,
- complete EditMode,
- complete PlayMode,
- deep soak,
- Android build,
- Lava install/launch,
- Web build,
- Chrome smoke,
- Edge smoke.

Record exact final:

- source commit,
- test counts,
- soak counts,
- APK size/hash,
- Web file count/size,
- WASM size/hash,
- Android memory smoke,
- browser console errors,
- browser failed requests,
- known warnings.

When practical, perform an end-to-end gameplay smoke:

```text
Menu
→ Offline
→ Fighter selection
→ Match
→ Attack
→ Ability
→ Gadget pickup/use
→ Aandhi
→ Elimination or match end
→ Results
→ Rematch
```

Do not fabricate interaction evidence if tooling cannot automate a step.

---

# 19. CURRENT EXECUTION BOUNDARY

This Goal-mode session may execute:

- Phase 0 — Exact-current-source rebaseline
- Phase 1 — Latest-authority delta audit
- Phase 2 — Stable gameplay event identities
- Phase 3 — Atomic match authority
- Phase 4 — Replay/state-hash/soak completion
- Phase 5 — Exact-source regression evidence

**STOP after Phase 5.**

Do not start:

- broad performance optimization,
- final art pass,
- large UI redesign,
- Photon,
- PlayFab,
- store signing,
- public deployment.

At the end, produce the next continuation prompt for:

```text
Performance + size
→ offline product-loop polish
→ visual/interaction/accessibility QA
→ networking-readiness gate
```

This session should prioritize **correctness, authority, determinism, reproducibility, and evidence** over breadth.

---

# 20. Performance — Record but Do Not Broadly Optimize Yet

The latest fresh development evidence before the newest authority commits reported roughly:

- Android development APK around 165.9 MB.
- Lava PSS around 420+ MB in one sample.
- Web output around 134 MB.
- Web WASM around 121 MB.

These are development-smoke numbers, not release performance budgets.

During this execution:

- record obvious regressions,
- avoid introducing allocation-heavy hot paths,
- remove clear accidental waste caused by your changes,

but do not launch a broad performance campaign until the authority/replay closure passes.

The next session should profile before optimizing.

---

# 21. Existing Offline/Product Systems to Preserve

Do not replace working systems unnecessarily.

The repository already contains substantial foundations for:

- `OfflineMatchAuthority`
- `OfflineMatchSimulation`
- `ArenaCollisionDefinition`
- `DeterministicCollisionSolver`
- `AuthoritativeProjectile`
- `MatchEventIdentity`
- deterministic replay/hash structures
- deterministic soak tests
- `ProductionFlowMachine`
- `TutorialStepMachine`
- Bijli
- Pehel
- Maya
- gadgets
- Aandhi
- bots
- offline eight-slot match
- Canvas UI
- fighter selection
- spectator/results/rematch
- Android/Web build tooling
- deterministic network mock
- Photon adapter seam
- progression interface
- fake progression backend
- release/security seams

Improve and verify; do not restart them.

---

# 22. Product Roster Rule

The canonical launch roster is exactly:

- Bijli
- Pehel
- Maya

There is **no fourth product-facing launch fighter named Raja** unless the current authoritative product documents explicitly change that decision.

A previous soak report incorrectly mentioned Raja; that error has already been removed.

Do not silently expand the roster.

---

# 23. Presentation and Lab Compatibility

Production Bazaar gameplay and MovementLab/testing do not necessarily use identical presentation paths.

Current repository intentionally preserves an older local projectile damage path for non-authority laboratory fixtures while production authority projectiles are snapshot-reconciled.

When refactoring:

- preserve useful isolated MovementLab tests,
- clearly label non-authoritative paths,
- do not let local lab code become the production authority path,
- do not delete lab support merely because production no longer uses it unless replacement coverage exists.

---

# 24. Implementation Priority Rule

When runtime/source and documentation disagree:

1. inspect source,
2. reproduce behavior,
3. add/fix a regression test,
4. fix runtime architecture/behavior,
5. rerun focused tests,
6. rerun full tests,
7. rebuild affected platforms,
8. update documentation last.

Do not spend the implementation session primarily rewriting status files.

Do not create documentation-only “completion” claims for work that lacks evidence.

---

# 25. Testing Discipline

Every meaningful defect fix should receive a regression test where practical.

For every implementation phase:

1. Run focused tests.
2. Run repository validation.
3. Run full EditMode when Core/Application changed.
4. Run full PlayMode when Presentation/integration changed.
5. Inspect warnings.
6. Check `git diff --check`.
7. Inspect actual diff before commit.

Tests must assert behavior, not merely class existence.

Avoid tests that cannot fail meaningfully.

Do not reduce coverage simply to make a suite green.

---

# 26. Git Commit Discipline

Use focused commits.

Suggested families:

```text
test: rebaseline exact current main
authority: complete stable gameplay event identities
authority: resolve zone healing and pickups atomically
authority: emit immutable authoritative event batches
refactor: remove production authority feedback loops
replay: execute recorded command streams deterministically
replay: hash complete authoritative match state
soak: cover fighters gadgets pickups and abilities
qa: record exact-source authority closure evidence
docs: align truthful completion status
```

Do not create a commit for every tiny edit.

Do not combine unrelated scene rewrites, binaries, or generated files with Core refactors.

---

# 27. Human Approval Gates

Do not proceed without explicit owner approval for:

- Photon licence/account acceptance.
- Photon App ID changes/use when terms approval is required.
- Paid Photon hosting/relay/server infrastructure.
- PlayFab title/environment creation.
- Backend secrets.
- Identity-linking/privacy/retention decisions.
- Paid hosting/CDN/analytics.
- Paid or externally licensed art/audio.
- Final branding/trademark decisions.
- Cultural-sensitive representation approval.
- CI Unity credentials/repository secrets.
- Release signing keys.
- Google Play submission.
- Public Web deployment.
- Final legal/privacy approval.
- Reclassification to release candidate/public product.

Local code, tests, local builds, local browser testing, Lava testing, documentation, editor tooling, and original placeholders may proceed without those external approvals.

---

# 28. Photon — Explicitly Out of Scope This Session

Photon Fusion may already be installed.

That does **not** mean multiplayer works.

Do not implement real Photon gameplay in this execution.

Before networking later, the offline authority gate must prove:

- deterministic authority,
- stable event identities,
- replay protection design,
- atomic match mutation,
- exact Android/Web builds,
- genuine soak,
- clean lifecycle,
- no critical known authority defect.

Later networking work must keep Fusion outside Core and must distinguish:

- deterministic mock,
- local Fusion proof,
- Shared mode,
- Host mode,
- trusted dedicated-server authority.

Never describe Shared/Host client authority as a trusted public server.

Never make a Web browser the trusted authority for public competitive matches.

---

# 29. PlayFab — Explicitly Out of Scope This Session

Do not implement PlayFab now.

Keep the fake backend for tests.

When later authorized, backend work must preserve:

- server-owned valuable rewards,
- no client secrets,
- idempotent reward processing,
- recoverable account linking,
- conflict resolution,
- privacy/export/deletion paths.

---

# 30. Security Review During Authority Work

While modifying authority/event/replay structures, think ahead to:

- stale commands,
- future commands,
- duplicate commands,
- replayed transport events,
- forged actor IDs,
- forged loadouts,
- forged origins,
- forged factions,
- client-reported damage,
- client-reported healing,
- client-reported rewards,
- same-tick event ordering,
- reconnect epochs,
- sequence rollover,
- result tampering.

Do not build transport yet, but avoid architecture that would require trusting the client later.

---

# 31. Documentation Update Rules

At the end of each completed phase, update only the documents that materially need it.

Primary current status files:

- `PROJECT_STATUS.md`
- `Docs/PRODUCT_COMPLETION_STATUS.md`
- `Docs/QA/LATEST_HEAD_BASELINE.md`
- `Docs/QA/REPLAY_AND_SOAK_REPORT.md`
- `Docs/DECISIONS.md` for architectural decisions

Do not duplicate the same full evidence block into many files.

Historical evidence should remain identifiable as historical.

Replace stale claims only after fresh evidence.

---

# 32. Final Report Required From This Session

At the end of Phase 5, report:

## Git

- Starting local branch.
- Starting local SHA.
- Starting `origin/main` SHA.
- Working branch.
- Ending SHA.
- Commits created.
- Whether worktree is clean.

## Architecture

- Authority changes.
- Event identity changes.
- Atomic-resolution changes.
- Replay changes.
- State-hash changes.
- Presentation synchronization changes.
- Removed/restricted mutation escape hatches.

## Tests

- Validation result.
- Full EditMode exact count.
- Full PlayMode exact count.
- Deep soak match count.
- Deep soak execution count.
- Deep soak duration.
- Divergence count.
- Focused regression tests added.

## Android

- Fresh APK path.
- Size.
- SHA-256.
- Lava serial/model.
- Install result.
- Launch/top-resumed result.
- Memory sample.
- Fatal/logcat findings.

## Web

- Fresh build path.
- File count.
- Total size.
- WASM size.
- WASM SHA-256.
- HTTP status.
- Chrome result.
- Edge result.
- Console errors.
- Failed network requests.
- Viewports tested.

## Remaining work

Classify every remaining item as:

- Not started.
- In progress.
- Passed with evidence.
- Blocked.
- Human review required.

Include:

- remaining authority gaps,
- replay limitations,
- soak limitations,
- performance debt,
- visual/UI debt,
- external gates,
- human-review gates.

## Truthful project classification

Choose only:

- Prototype.
- Technical vertical slice.
- Playable vertical slice.
- Closed alpha.
- Release candidate.
- Blocked.

Do not promote the classification without evidence.

## Continuation prompt

Produce a self-contained continuation prompt for the next fresh agent session covering:

1. performance/size profiling,
2. offline loop and UX polish,
3. visual/audio/accessibility QA,
4. final networking-readiness review,

while still keeping Photon/PlayFab behind explicit approval gates.

---

# 33. Final Truthfulness Rules

Never claim:

- tests passed without running them,
- Android works because an older APK worked,
- Web works because HTTP returned 200,
- visual QA passed without inspecting the canvas,
- soak passed because a document says so,
- replay is complete because replay classes exist,
- event IDs provide network replay protection merely because counters exist,
- authority is atomic while presentation feeds mutations back into Core,
- multiplayer works because Fusion is installed,
- PlayFab works while using a fake backend,
- performance passed from one memory sample,
- human review was automated,
- legal/cultural/accessibility approval exists without humans,
- the project is complete because milestone/status files say so.

**Source presence is not proof.  
Documentation is not proof.  
Old builds are not proof.  
Old QA evidence is not proof.  
Exact-current-source reproducible evidence is proof.**

---

# 34. Start Now

Start by performing:

**Phase 0 — Exact-Current-HEAD Rebaseline**

Do not modify runtime code until you have:

1. verified the actual current local and remote repository state,
2. read the mandatory product/architecture/status files,
3. inspected the latest authority/projectile/replay/soak code,
4. run the exact-current baseline,
5. reported any baseline failure or discrepancy.

Then proceed autonomously through Phases 1–5.

Stop after Phase 5.

Do not begin Photon.

Do not begin PlayFab.

Do not publish.

Do not use the Oppo device.

Preserve user work.

Treat exact current source and reproducible runtime evidence as the source of truth.
