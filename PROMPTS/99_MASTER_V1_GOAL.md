# BattleRaja V1 — Luna Max Goal-Mode Master Prompt (Continuation)

**Prompt revision:** 1.1
**Prepared:** 2026-09-01 00:03 IST
**Repository:** `neonvarun/BattleRaja`
**Workspace:** `C:\Projects\BattleRaja`
**Known checkpoint:** `56313096d0ad8e2e23468d004eaa77d71ed3a233` (the latest source always wins)

[@Test Android Apps](plugin://test-android-apps@openai-curated-remote)
[@Tavily AI](plugin://app-69f271663a288191ac98f46bed7cb032@openai-curated-remote)
[@Unity Essentials](plugin://unity-workbench@openai-curated-remote)
[@Game Studio](plugin://game-studio@openai-curated-remote)
[@GitHub](plugin://github@openai-curated-remote)

You are the **Luna Max implementation agent** in a fresh Goal-mode session with no assumed
conversation context. Work autonomously through all safe local engineering, asset creation,
research, testing, profiling and release preparation. Make informed product decisions from
the evidence and document them. Do not call the project complete because a file exists, a
scene launches, a generated asset exists, a legacy suite is green, or a human approval is
still open.

## Mission

Take the exact current checkout at `C:\Projects\BattleRaja` and finish the strongest
truthful **BattleRaja V1 offline Android release candidate**: an original, polished,
replayable, mobile-first **4v4 Bastion Crown** game.

The player experience must be:

- Team Raja: one human plus three friendly AI bots.
- Rival: four enemy AI bots.
- Exactly eight fighters, no internet, no login and no account requirement.
- Bijli, Pehel and Maya with meaningful team roles.
- Umbrella Guard, Dhol Burst and Tiffin Station, all mechanically and visually complete.
- One authored Bazaar Bastion flagship arena.
- Crown Spark objective, shrines, KOs/assists, shared tickets, protected respawn,
  spectator state, Aandhi pressure, tutorial, settings/accessibility, results and rematch.

## Important current-state correction

The known latest commit `5631309` already contains a real first implementation of this
4v4 layer. It includes `BastionCrownContracts`, `BastionCrownMatch`, a Unity controller
adapter, canonical actor IDs 1–8, team HUD/objective telegraphs, basic role-aware bot
intent, a regenerated Bazaar Bastion scene and local rerun evidence of 148/148 EditMode
and 94/94 PlayMode. It is **not** a finished release.

The current team state is mirrored beside the legacy `OfflineMatchSimulation`; squad AI is
destination-level rather than a proven coordinated blackboard; Bastion replay/soak proof,
damage-interrupted deposit behavior, post-deposit socket semantics, healing attribution and
several timing/edge cases are incomplete or unverified. The generated/provenance-safe art,
audio and UI are a baseline, not final approved content. Lava, normalized final-art
performance, physical 16 KB runtime proof, permanent package/signing identity and Play/legal
materials are open. **Audit and harden what exists; do not reimplement it blindly.**

## Product and scope lock

### Canonical Bastion Crown contract

- Mode ID: `BR_BastionCrown_V1`.
- Team Raja actors 1–4: actor 1 human; actors 2–4 friendly AI. Rival actors 5–8 are enemy AI.
- Exactly eight slots; no accidental ninth actor, hidden replacement or public online selector.
- One original 32 m × 32 m walkable Bazaar Bastion arena with west/east spawn banks,
  three Crown sockets and one shrine per team. Authority collision is canonical.
- Three-second ready phase; 240-second live clock; deterministic maximum 30-second overtime.
- Neutral Crown Spark: 0.25-second contact pickup; carrier speed multiplier 0.88 (12% slower);
  defeat drops it for six seconds with a 1.25-second pickup lock; allied shrine deposit is a
  1.25-second channel and must be explicitly interruptible by the documented events.
- Crown rotates on the 35-second cadence and must advance deterministically after a deposit;
  verify/fix the current implementation if it resets to the same socket.
- Confirmed enemy KO gives +1 team score; completed Crown deposit gives +3. First team to 15
  wins. At live-clock tie compare score, deposits, KOs and tickets remaining; otherwise enter
  overtime. Overtime resolves on a valid Crown/sudden-death result, team wipe or the 30-second
  cap; an unresolved tie is an explicit draw. Do not invent a competing tie-break.
- Each team has 12 shared tickets. A defeat presents four seconds of spectator state and
  returns at five seconds if a ticket remains; consume only on actual respawn. Exhausted
  fighters stay spectators. A team wipe means all four are out with no valid pending return.
- Respawn protection lasts 2.5 seconds or until that fighter deals damage. Protected actors
  cannot receive combat or Aandhi damage. Friendly fire is off and ally collision is soft.
- Team identity uses redundant shape/icon/outline cues, not hue alone. Results expose KOs,
  deaths, assists, damage, healing, Crown pickups/deposits, objective time, gadget/ability
  use and tickets spent. Rematch keeps local settings/fighter choice, creates a new seed and
  resets all state.

### Explicitly out of V1

Do not build Photon gameplay, PlayFab, accounts, matchmaking, social/clans, cloud progression,
shop, ads, IAP, online leaderboards, cross-platform networking, Web release work or copied/
licensed reference-game assets. Preserve future seams only where they already exist and keep
them outside the offline core. Do not purchase services/assets or accept legal agreements.

## Mandatory first actions: rebaseline before changing code

1. Read `AGENTS.md`, `PROJECT_STATUS.md`, `PROJECT_CONTEXT.json`, `Docs/MASTER_VISION.md`,
   `Docs/ARCHITECTURE.md`, `Docs/DECISIONS.md`, `Docs/CULTURAL_GUIDE.md`, `Docs/ART_BIBLE.md`,
   `Docs/AUDIO_BIBLE.md`, `Docs/ASSET_PROVENANCE.md`, `Docs/RESEARCH_LOG.md`, current QA/
   performance/release docs, `Docs/AI/UnityProjectContext.md`, `Docs/AI/V1_PRODUCT_REBUILD_AUDIT.md`,
   `Docs/AI/V1_EXECUTION_REBASELINE_2026-08-31.md`, `Docs/AI/V1_REFERENCE_DESIGN_MATRIX.md`,
   `Docs/AI/PROMPT_REWRITE_MANIFEST.md`, `Docs/AI/BASTION_CROWN_PRODUCT_BRIEF_2026-08-31.md`,
   and every file in `PROMPTS/`.
2. Run and retain output for:

   ```powershell
   git fetch --all --prune
   git status --short --branch
   git rev-parse HEAD
   git rev-parse origin/main
   git log --oneline --decorate -25
   git stash list
   git lfs fsck --pointers
   ```

   Preserve all user changes and stashes. Never reset, clean, discard, apply or delete them
   without explicit direction. If the source advanced beyond `5631309`, update the context,
   audit and prompt manifest before implementation.
3. Confirm Unity/package versions, scenes, assemblies, build scripts, package permissions,
   current APK/AAB identity and all test commands. Run repository validation, full EditMode,
   full PlayMode, current deterministic replay/soak and the relevant Bastion tests before
   major changes. Treat any failure as a blocker to understand, not as noise.
4. Verify the exact candidate artifacts and install/run the current source on approved Lava
   when it is connected. Capture the complete route: cold launch → `PLAY OFFLINE` → briefing
   → fighter choice → ready → live team match → Crown pickup/deposit → KO/respawn/spectator
   → Aandhi → result → rematch → settings/tutorial/lifecycle. If Lava is unavailable, use the
   approved Android 16 AVD only as a qualified emulator fallback and record the limitation.

## Deep research and reference conduct

Use Tavily/web and first-party documentation for all unstable facts. At execution time,
re-check the official Android/Play target API, 16 KB page-size, Data Safety, content-rating,
review and current Unity/Android package guidance. Record URL, access date, claim, uncertainty,
decision impact and local evidence in `Docs/RESEARCH_LOG.md`.

If the approved Lava is connected, use `[@Test Android Apps]` to launch the installed
Brawl Stars package `com.supercell.brawlstars` and Smash Karts package
`com.tallteam.citychase`. Observe only public, readily accessible entry/onboarding/UI surfaces
for high-level principles: hierarchy, immediacy, touch comfort, silhouette readability,
objective communication, low-end rendering, audio priority and replay friction. Store notes
and captures under `Builds/Local/PlanningAudit/References/`, outside production assets.
Never decompile, extract, inspect private data, intercept traffic, alter an account, purchase,
copy screenshots/content/terminology/timings/trade dress or use their assets. If a screen needs
an account, payment or unsafe action, stop and record the limitation. BattleRaja must remain
visibly and mechanically original; reference research is not an implementation specification.

## Stage execution order

Read and execute prompts `01` through `16` in this order. `99_MASTER_V1_GOAL.md` is this
orchestration prompt, not an additional stage:

```text
01 current-state/reference audit
02 product scope and 4v4 contract
03 authority rules, respawn, score, objective and replay
04 team bot AI, squad behavior and difficulty
05 fighter kits, roles, balance and combat
06 character concepts, 3D models, rigs and animation
07 maps, environment art, lighting and world building
08 gadgets, pickups, objectives and interactables
09 combat VFX, camera, feedback and readability
10 UI/UX, menu, HUD, team signals and results
11 audio, music, haptics and game feel
12 tutorial, onboarding, accessibility and settings
13 performance, memory, rendering and Android optimization
14 Lava end-to-end visual, gameplay and device QA
15 Play Store release, store art, privacy and packaging
16 final integration, regression and V1 release gate
```

For every stage, first audit the latest source and any user edits, then preserve healthy
implementation and fix only the missing/unsafe behavior. Implement real code/assets, update
tests and decisions, build the relevant candidate, inspect the result and retain evidence.
Each stage prompt's binary gate must pass before advancing. If a gate fails, fix it locally
or report the exact blocker and stop; never mark it complete optimistically.

## Authority and AI completion requirements

- Keep Core.Domain/Core.Application pure C# and independent of Unity UI, Photon, PlayFab and
  external SDKs. Mutable team/objective/combat/ticket/respawn/result state belongs to one
  authority; views only consume ticks/events.
- Resolve the current legacy/team mirror safely: prove or consolidate health, damage, defeat,
  Crown drop, respawn, result and replay event ownership. Add event IDs, fixed-step boundaries,
  duplicate-delivery tests, simultaneous-action tests, deterministic capture/replay and
  multi-seed long-run soak. Do not silently weaken Solo compatibility.
- Complete damage-interrupted deposit behavior, after-deposit socket rotation, overtime/sudden
  death semantics, healing attribution, Aandhi interactions, team-wipe/queued-respawn edge
  cases, rematch seed reset and all gadget/objective interactions. Keep all values data-driven
  and record balance changes.
- Replace destination-only bot intent with a deterministic squad blackboard or equivalent:
  role assignment, Crown contest/escort/intercept, shrine defense, cover/spacing, focus/peel,
  ally heal/support, ticket risk, regroup, retreat from Aandhi and recovery from stuck states.
  Friendly bots must support the human without body-blocking; rivals must pressure fairly.
  Use the same perception limits and commands for all actors; no hidden vision, damage,
  health, cooldown, speed or pathing cheats. If autonomous-bot damage scaling remains, justify
  it as an explicit PvE policy and prove it does not distort fairness.
- Add reproducible metrics across many seeds: match duration, score/deposits/KOs, Crown time,
  ally assistance, objective contribution, spacing, gadget/ability use, stalemates, wipes,
  respawn/ticket behavior, invalid events and difficulty separation. Include real-time Lava
  matches, not only accelerated fixtures.

## Original asset and product-quality requirements

Create and integrate all missing final assets yourself with Unity/Blender/procedural/local
generation tooling. For every asset retain editable source and provenance:

```text
concept/turnaround → clean model/topology → UV/material → rig/skin
→ animation → export/import → prefab/sockets → LOD/quality tiers
→ gameplay-camera test → Lava device test → provenance record
```

Bijli, Pehel and Maya must have distinct proportions, silhouettes, topology/LOD budgets,
materials/palettes, rig hierarchies, animation personality, attack/ability/Crown VFX and
fighter portraits. The Bazaar kit needs intentional lanes, flanks, cover, landmarks, shrines,
sockets, walkable readability and optimized materials. Model gadgets, pickups, Crown/shrines,
impact/heal/knockback/spawn/KO/Aandhi effects, UI icons, logo/app icon, feature graphic,
tutorial art and store screenshots from the real final build. Create original UI sound,
combat/ability/gadget/objective/Aandhi/victory/defeat cues, mix groups and haptics.

No greybox, primitive body, static concept image, recolored duplicate animation, random
background, unlicensed pack, copied reference expression or debug label may remain in the
player path. Preserve authority collision; art conforms to validated gameplay geometry.
Generated assets are acceptable only when they are editable, provenance-safe, integrated,
readable at mobile camera distance and pass device review—not merely because they exist.

## UI, accessibility and feel

Make the complete flow mobile-first: safe areas/aspect ratios, large touch targets, left-handed
layout, aim assist, reduced flashes, high contrast, practical text scaling, music/effects
volume, haptic toggle, pause/resume, tutorial replay, spectator/respawn clarity and readable
team/objective signals using shape/icon/outline plus color. `PLAY OFFLINE` is the public CTA;
do not expose a broken online mode. Audit every screen and transition on the actual device.

## Performance and Android release hardening

Profile the final-art candidate, not the old generated build. Measure CPU/main/render/GPU where
available, p50/p95/p99 frame time, GC allocations/spikes, managed/native/graphics memory,
draw calls/shader variants, loading, repeated-match growth, thermal and battery behavior.
Use pooling and no per-frame searches/allocations in hot paths. Preserve telegraphs at every
quality tier. Use a documented target appropriate to Lava; as a starting gate aim for 60 FPS
where sustained, p95 ≤20 ms, p99 ≤33 ms, no unexplained >1 s stall, no >10% persistent memory
growth over ten rematches, no >50 ms GC spike and PSS ≤450 MB unless measured device limits
justify another threshold.

Re-check current official Play requirements immediately before packaging. Verify target API 36+
if required at submission time, ARM64/IL2CPP, 16 KB alignment and genuine 16 KB runtime where
available, bundle/install/zip alignment, permissions, debug flags, dependency/licenses/secrets,
offline startup and crash/fatal logs. Produce release-shaped APK/AAB and accurate privacy,
Data Safety, IARC/content-rating, support, description, short-description, release-note,
tester and known-issue drafts. Do not choose a permanent publisher/package identity, handle
signing keys, accept legal agreements or upload/roll out without owner authorization.

## Evidence, stop rules and final report

Retain timestamped source/build/test/device evidence under the existing `Builds/Local` QA layout:
commit/branch/dirty state, commands/output, test counts, replay/soak reports, screenshots/video,
device/API/orientation/quality/settings, APK/AAB paths and hashes, performance data, policy
sources, provenance and known limitations. A green test suite never substitutes for visual or
physical evidence. Do not overwrite failed evidence; append corrected reruns.

At the end report:

- final source commit/branch and clean/dirty status;
- gameplay/authority/AI, art/model/animation, map, VFX, UI, audio, tutorial and release work;
- exact changed files and provenance/source locations;
- commands, test counts, replay/soak/simulation metrics and any failures;
- Lava device/build/hash and complete visual/gameplay route, or the exact unavailable-device limit;
- normalized frame/CPU/GPU/GC/memory/thermal/battery/endurance results;
- APK/AAB paths, SHA-256, package/version, ABI/page-size/bundle validation;
- store/privacy/Data Safety/content-rating/support drafts and approval status;
- remaining owner-only approvals and exact blockers;
- truthful final classification: `Play Store Release Candidate`, `Candidate with named blockers`
  or `Prototype`.

Only use `Play Store Release Candidate` when the same final-art source/build has passed the
full offline 4v4 route, authority/AI/replay tests, all three fighters/gadgets, authored map and
presentation, accessibility/settings, normalized physical performance, Android/package checks,
and representative store/privacy preparation. Until then retain the honest classification
`Prototype — Bastion Crown implementation checkpoint`.

## Short launcher text for a fresh Goal-mode input box

```text
Read and execute C:\Projects\BattleRaja\PROMPTS\99_MASTER_V1_GOAL.md. Rebaseline the exact current repository first, then read PROMPTS\README.md, prompts 01–16, PROJECT_STATUS.md and the current AI audit/context files. Continue from the latest Bastion Crown offline 4v4 implementation (1 human + 3 friendly AI vs 4 rival AI); do not reimplement healthy work blindly. Harden authority/replay/rules and proper squad AI, create and integrate the real original 3D/audio/UI/VFX assets, finish the Bazaar Bastion player flow, use the approved Lava ST5GDW23LB004392 for evidence when connected, use Brawl Stars/Smash Karts only for high-level observation, and complete performance/Android/Play preparation without multiplayer, copying, signing, legal approval or upload. Stop on failed gates and report evidence, limitations and owner-only blockers honestly.
```
