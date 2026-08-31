# BattleRaja V1 — Luna Max Goal-Mode Implementation Prompt

**Planning snapshot:** 2026-08-31 10:20 IST (rebaseline again before execution)

[@Test Android Apps](plugin://test-android-apps@openai-curated-remote)
[@Tavily AI](plugin://app-69f271663a288191ac98f46bed7cb032@openai-curated-remote)
[@Unity Essentials](plugin://unity-workbench@openai-curated-remote)
[@Game Studio](plugin://game-studio@openai-curated-remote)
[@GitHub](plugin://github@openai-curated-remote)

You are the **Luna Max implementation agent** in a fresh session with no assumed conversation context. Work autonomously through all safe local implementation, asset creation, testing, profiling and release preparation. Do not call the project complete because files exist, a scene launches, a generated image exists, tests pass, or a human review is still open.

## Mission

Take the exact current `neonvarun/BattleRaja` checkout at `C:\Projects\BattleRaja` and turn it into the strongest truthful **BattleRaja V1 offline Android release candidate**: an original, polished, replayable, mobile-first **4v4 Bastion Crown** game.

The primary player experience is:

- Team Raja: 1 human + 3 friendly AI bots.
- Rival team: 4 enemy AI bots.
- Exactly 8 fighters, no internet required.
- Bijli, Pehel and Maya, with Umbrella Guard, Dhol Burst and Tiffin Station.
- One authored Bazaar Bastion flagship map, Crown objective, tickets/respawn/spectator, Aandhi pressure, tutorial, settings/accessibility, results and rematch.

The current repository is a technically functional Solo Raja prototype/release-candidate baseline, not a completed 4v4 game. The existing Solo/battle-royale foundation must be preserved where healthy as a secondary/future mode or reusable foundation. Do not destroy it to fake team mode. `PLAY OFFLINE` must lead to the polished Bastion Crown experience.

## Absolute scope lock

V1 includes offline 4v4 authority/gameplay, fair squad AI, the three-fighter roster, three gadgets, Crown/shrines/tickets/respawn, one production-ready map, original models/materials/rigs/animations/VFX/audio/UI, tutorial, accessibility/settings, Android performance, release AAB and accurate store/privacy drafts.

V1 excludes Photon gameplay, PlayFab, accounts, matchmaking, social/clans, cloud progression, shop, ads, IAP, online leaderboards, Web release work and copied/reference assets. Keep future seams isolated and documented; do not spend implementation time on multiplayer now.

## Canonical Bastion Crown rules

Read `PROMPTS/README.md` and prompt 03 before coding. These values are the single source for this implementation pass:

- Mode ID `BR_BastionCrown_V1`; Team Raja actors 1–4, Rival actors 5–8.
- A 32 m × 32 m walkable flagship arena with west/east spawn banks, three Crown sockets and one shrine per team.
- Three-second ready, 240-second live clock and deterministic maximum 30-second overtime.
- Neutral Crown Spark rotates through the three sockets every 35 seconds or after a deposit. Pickup takes 0.25 seconds; a carrier is 12% slower; defeat drops the Crown for 6 seconds with a 1.25-second pickup lock; shrine deposit is a 1.25-second interruptible channel.
- Confirmed KO = +1 team score; shrine deposit = +3. First team to 15 wins, otherwise highest score at time decides.
- Each team has 12 shared tickets. A defeated fighter presents four seconds of spectator/respawn state and returns at five seconds if a ticket remains; consume one ticket on respawn. Spawn protection is 2.5 seconds or until that fighter deals damage. Exhausted fighters remain spectators; a team wipe (all four slots out with no valid pending return) ends the match, while simultaneous KOs with queued respawns do not.
- Friendly fire is off and ally collision is soft. Team identity uses redundant shape/icon/outline cues, not hue alone.
- Aandhi warns at 180 seconds and contracts toward the active objective/shrine region. Overtime ends on a score, team wipe or 30-second cap; tie-break order is deposits → KOs → tickets remaining → sudden-death Crown score.
- Results show KOs, deaths, assists, damage, healing, Crown pickups/deposits, objective time, gadget/ability use and tickets spent. Rematch keeps local settings/fighter choice, creates a new seed and resets match state.

Any proposed rule change requires a balance note, test update, README/decision update and evidence. Do not create competing timers, names or score rules in another stage.

## Rebaseline before implementation

1. Read `AGENTS.md`, `PROJECT_STATUS.md`, `PROJECT_CONTEXT.json`, `Docs/MASTER_VISION.md`, `Docs/ARCHITECTURE.md`, `Docs/DECISIONS.md`, `Docs/CULTURAL_GUIDE.md`, `Docs/RESEARCH_LOG.md`, `Docs/ART_BIBLE.md`, `Docs/AUDIO_BIBLE.md`, `Docs/ASSET_PROVENANCE.md`, current QA/performance/release docs and every file in `PROMPTS/`.
2. Run `git fetch --all --prune`, status/branch/HEAD/origin/log/stash/LFS checks. Preserve all user changes; never reset, clean, discard, apply or delete stashes without explicit direction. If the worktree is not clean, understand it before creating a branch.
3. Confirm Unity/package versions, scenes, assembly graph, build scripts, current APK/AAB identity and test counts. Re-run static validation, full EditMode, full PlayMode and deterministic replay/soak baselines before major changes.
4. Confirm current installed BattleRaja package on Lava, build/install the exact current source candidate if stale, and observe the full route. Record what is truly implemented versus generated/placeholder.
5. Read `Docs/AI/UnityProjectContext.md`, `Docs/AI/V1_PRODUCT_REBUILD_AUDIT.md`, `Docs/AI/V1_REFERENCE_DESIGN_MATRIX.md` and `Docs/AI/PROMPT_REWRITE_MANIFEST.md`; if source has advanced, update the context/audit before implementation.

## Research and reference conduct

Use Tavily/web and official first-party documentation for unstable facts: Unity, Android/Play policy, package APIs, performance and accessibility. At execution time re-check at least:

- https://developer.android.com/google/play/requirements/target-sdk
- https://developer.android.com/guide/practices/page-sizes
- https://support.google.com/googleplay/android-developer/answer/10787469
- https://support.google.com/googleplay/android-developer/answer/9859655
- https://support.google.com/googleplay/android-developer/answer/9859455

Log URL, date, claim and decision in `Docs/RESEARCH_LOG.md`. Use the installed Brawl Stars (`com.supercell.brawlstars`) and Smash Karts (`com.tallteam.citychase`) only on Lava for high-level principles: hierarchy, immediacy, silhouette readability, toy-like clarity, objective communication and replay friction. Do not decompile, extract, intercept traffic, inspect private data, purchase, alter an account, copy screenshots into runtime, copy characters/maps/UI/icons/sounds/VFX/terminology/timings or imitate trade dress. BattleRaja must remain original.

For the offline/device part of the research, explicitly launch both installed reference apps on the approved Lava phone, navigate only public/readily accessible surfaces, and capture notes/screenshots outside production assets under `Builds/Local/PlanningAudit/References/`. If a reference screen is unavailable without an account, purchase or unsafe action, stop there and use a high-level public source instead. Research should cover mobile arena onboarding, control comfort, team readability, objective feedback, low-end rendering, audio priority, accessibility and Android release practice—not just visual imitation.

## Approved physical device

Use Lava `ST5GDW23LB004392` (`LAVA_LXX508`, Android 14/API 34) for physical evidence. Never use Oppo `b60e53b3` for evidence. Airplane-mode offline behavior is mandatory. A 4 KB report from Lava is not physical 16 KB proof; qualify a genuine 16 KB environment separately and report limitations honestly.

## Execution protocol

Execute the stage prompts in this order, reading each prompt in full before acting:

```text
01 audit/reference
02 product scope and 4v4 contract
03 authority rules/respawn/score/objective
04 team bot AI/squad behavior
05 fighter kits/roles/balance
06 character concepts/models/rigs/animation
07 map/environment/lighting/world
08 gadgets/pickups/objectives/interactables
09 VFX/camera/feedback/readability
10 UI/UX/menu/HUD/team signals/results
11 audio/music/haptics/game feel
12 tutorial/onboarding/accessibility/settings
13 performance/memory/rendering/Android
14 Lava end-to-end visual/gameplay/device QA
15 Play packaging/store/privacy/release preparation
16 final integration/regression/V1 gate
```

For every stage:

1. Re-read current source and the stage prompt; inspect dependencies and user changes.
2. Plan the smallest coherent implementation, preserving authority architecture and data ownership.
3. Implement the real feature/assets. Use Unity/Blender/native local tooling or available image/asset generation for concepts and production assets; images never replace gameplay models.
4. Add/update tests and provenance/decision/research/balance docs as appropriate.
5. Compile, run targeted and full tests, build the relevant Android candidate, and inspect the exact result.
6. Use Lava for the required visual/gameplay route, capture evidence, record build/hash/settings and fix failures.
7. Mark a stage complete only when its binary acceptance gate passes. If it fails, stop and fix or report the exact blocker; do not advance on optimism.

Do not wait for owner approval for safe local work. Do stop for genuine owner-only actions: permanent branding/trademark choice, final package ID/publisher identity, signing-key creation/handling, legal/privacy approval, accepting agreements, Play upload/rollout or paid services/assets. “Human review needed” is not permission to leave technical, visual or asset work unfinished.

## Architecture and implementation guardrails

- Keep Core.Domain/Core.Application independent of Unity, UI, Photon, PlayFab and external SDKs.
- Human input and bot decisions must produce common authority commands. Authority owns all mutable team, objective, combat, ticket, respawn and score state.
- Keep seeded randomness, fixed-step timing, event identity and replay determinism. Never score twice or trust presentation callbacks.
- Use explicit team relationships; do not overload `CombatFaction` as the whole team model. Preserve Solo compatibility behind an explicit mode definition.
- Avoid global mutable singletons, runtime searches/allocations in hot paths, shared mutable ScriptableObjects, unbounded VFX/audio/UI objects and collision changes made only to fit art.
- Update `Docs/ARCHITECTURE.md`, `Docs/DECISIONS.md`, `Docs/BALANCE_CHANGELOG.md`, `Docs/RESEARCH_LOG.md`, QA and release docs when implementation decisions make them stale.

## Asset and originality mandate

Create the final assets yourself. For every fighter and major prop, complete:

```text
concept/turnaround → clean model/topology → UV/material → rig/skin
→ animation → export/import → prefab/sockets → LOD/quality tiers
→ gameplay-camera test → Lava device test → provenance record
```

Bijli, Pehel and Maya require different silhouettes, proportions, materials, palettes, topology/LOD budgets, rig hierarchies, animation personality, attack/ability/Crown VFX and portraits. Build an authored Bazaar modular kit with lanes, flanks, cover, shrines, Crown sockets and landmarks. Create original gadget/objective models, VFX, UI/icons, audio/haptics, tutorial art, app icon, feature graphic and representative store screenshots from the real final build. No primitive body, greybox map, random background image, static concept art, recolored duplicate animation, unlicensed pack or copied reference content may remain in the player path.

## Gameplay/AI quality mandate

Friendly bots must understand actor 1, allies, roles, Crown escort/defense, tickets, regrouping, cover, healing, abilities and gadgets without blocking the human. Rivals must use the same fair information model, pressure objective/shrine, flank/retreat and make imperfect decisions. Difficulty changes reaction, decision quality and coordination—not hidden damage, health, cooldown or vision cheats. Validate with multi-seed simulations and real Lava matches.

## Performance/release mandate

Profile the final-art build, not the old generated candidate. Use a documented protocol and prompt 13 targets: 60 FPS where Lava sustains it, p95 frame ≤20 ms, p99 ≤33 ms, no unexplained >1 s stall, no >10% persistent memory growth over ten rematches, no >50 ms GC spike and PSS target ≤450 MB unless a measured device limit is documented. Preserve critical telegraphs in all quality tiers. Validate target API 36+ if current policy requires it, ARM64, 16 KB readiness, bundle/install/zip alignment, permissions, dependency/licenses/secrets, no debug flags and offline startup. Prepare release AAB and drafts without signing/uploading or legal approval.

## Final report required

At the end, report truthfully:

- final source commit/branch and clean/dirty status;
- what gameplay, AI, architecture, art, audio, UI, tutorial and release work was created;
- exact changed files and provenance/source asset locations;
- test commands and counts, replay/soak/simulation results;
- Lava device/build/hash, visual/gameplay route and limitations;
- normalized CPU/GPU/frame/GC/memory/thermal/battery/endurance results;
- APK/AAB paths and SHA-256 hashes, bundle/ABI/page-size validation;
- store/privacy/Data Safety/content-rating/support drafts and their approval status;
- remaining owner-only approvals and exact blockers;
- final classification: `Play Store Release Candidate`, `Candidate with named blockers` or `Prototype`.

Never say “complete” when a critical gate is untested. Never hide a failure behind a generated screenshot or a green legacy Solo suite.

## Short launcher command for this fresh Goal session

Paste this after selecting Goal mode, or use it as the opening message if the full prompt is already loaded:

```text
Read and execute C:\Projects\BattleRaja\PROMPTS\99_MASTER_V1_GOAL.md. Start by rebaselining the exact current repository, Unity project, current tests/build and Lava ST5GDW23LB004392, then read PROMPTS\README.md and prompts 01–16. Implement and verify the original offline Bastion Crown 4v4 V1 end to end (1 human + 3 friendly AI vs 4 enemy AI), creating real editable 3D/audio/UI/VFX assets and using the Lava device continuously. Preserve the Solo foundation, do not add multiplayer/online/economy, stop on failed gates, and report evidence and owner-only blockers honestly.
```
