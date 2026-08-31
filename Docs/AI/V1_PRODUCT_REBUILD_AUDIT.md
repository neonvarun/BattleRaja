# BattleRaja V1 Product Rebuild Audit

**Audit date/time:** 2026-08-31 10:20 IST
**Source audited:** `3bed64e82be0a84c8bf978d871ae322604b3f7ff`
**Device evidence:** Lava `ST5GDW23LB004392` only
**Classification:** Prototype — Android offline release candidate in progress

This is a planning audit for the next implementation agent. It is intentionally critical. A passing automated test or a generated asset is evidence of a foundation, not evidence of a player-ready product.

## Executive assessment

BattleRaja has a stronger technical foundation than its current presentation suggests. The pure domain/application split, authority-owned combat, seeded replay identity, deterministic tests, offline flow and Android candidate build are valuable. The product is nevertheless still pre-alpha from a player perspective: the shipped path is Solo Raja, not the newly intended 4v4 team battle; the current bots have no squad/objective model; and the visible art, UI, feedback and audio are generated technical baselines that need authored direction and device validation.

The next V1 effort must be a controlled rebuild around one original, repeatable 4v4 objective mode. It must not be a cosmetic pass over free-for-all rules, and it must not turn into an online/live-service rewrite.

## Evidence reviewed

- `AGENTS.md`, `PROJECT_STATUS.md`, `PROJECT_CONTEXT.json`.
- `Docs/MASTER_VISION.md`, `Docs/ARCHITECTURE.md`, `Docs/DECISIONS.md`, `Docs/ART_BIBLE.md`, `Docs/AUDIO_BIBLE.md`, `Docs/ASSET_PROVENANCE.md`.
- `Docs/QA/CURRENT_STATE.md`, `Docs/QA/LATEST_HEAD_BASELINE.md`, replay/soak and M11 closure reports.
- `Docs/RELEASE/V1_ANDROID_RELEASE_CHECKLIST.md`, store creative/release drafts and human-review backlog.
- First-party domain/application/presentation/editor code, scenes, prefabs, tests and build tooling.
- Current candidate APK/AAB and captures in `Builds/Local/Device/audits/20260831-presentation/`.
- Controlled observation of installed `com.supercell.brawlstars` and `com.tallteam.citychase` on Lava. Observations are principles only; no extraction, decompilation, account action, traffic interception or copied content was used.
- Official policy sources recorded on 2026-08-31: Android target API, 16 KB page size, Play Data Safety, content rating and app review preparation.

## Quality scorecard

| Area | Evidence-backed status | Severity | Planning decision |
|---|---|---:|---|
| Domain authority and deterministic replay | Genuinely strong Solo foundation; 141/141 EditMode, 92/92 PlayMode, replay/soak evidence | Low | Preserve and extend behind explicit team contracts |
| Offline launch-to-results loop | Technically works for Solo; lifecycle/settings/tutorial/rematch routes exist | Medium | Keep flow, make 4v4 the player-facing default and test every transition |
| 4v4 rules/objective/respawn | Not present as first-class domain state | Blocker | Implement before art polish is considered complete |
| Team AI | Fair FFA utility bot only; no squad roles, shared objective or ally assistance | Blocker | Add deterministic team perception/coordination and difficulty profiles |
| Fighter kits | Bijli, Pehel and Maya function in Solo with existing combat seams | High | Rebalance for team roles and objective pressure; preserve command/authority seams |
| Character presentation | Saved faceted meshes, two-bone rigs and clips; readable baseline but technical/generated | Blocker | Re-author production silhouettes, materials, animation personality and camera proof |
| Bazaar map | Saved environment and collision-compatible arena; readable baseline but sparse/greybox-like | Blocker | Build one authored flagship 4v4 combat space with lanes, cover and objective sockets |
| Gadgets/interactables | Three vertical-slice gadgets and pickups exist mechanically | High | Reconcile with team objective, telegraph, feedback and team utility |
| VFX/camera/feedback | Cues and zone visual exist; impact and readability are inconsistent on device | High | Build readable low-flash feedback and objective/team signals |
| UI/UX | Runtime-built menu/HUD/settings work; visible surfaces are sparse/oversized and Solo/debug-oriented | Blocker | Replace with a coherent 4v4 mobile design system |
| Audio | Owned deterministic WAV/mixer baseline exists | High | Author a coherent mix and validate loudness/clipping/priority on Lava |
| Performance | Bounded telemetry exists; no normalized final-art frame/endurance proof | Blocker | Profile release art build, memory growth, thermal/battery and 16 KB compatibility |
| Store readiness | Target API 36 and candidate artifacts exist; temporary ID/debug signing/drafts remain | Blocker | Prepare package/store/privacy materials; stop before owner-only upload/sign/legal actions |

## What is genuinely production-quality or worth retaining

1. **Authority boundaries.** `OfflineMatchAuthority` centralizes canonical state and action eligibility. Presentation mirrors it instead of becoming a second rules engine.
2. **Pure-testable core.** Domain/application code can be exercised without loading a production scene. Keep this property for teams, score, tickets, objective and respawn.
3. **Common command path.** Human input and bots can feed the same command/authority path. Extend the command vocabulary rather than special-casing allied bots in Unity.
4. **Determinism and identity.** Seeded randomness, replay event identity and existing long-run checks are an excellent safety net for new team rules and AI.
5. **Existing fighter/gadget seams.** Bijli, Pehel, Maya and Umbrella Guard/Dhol Burst/Tiffin Station have useful definitions, cooldowns and authority integration.
6. **Offline flow and lifecycle hardening.** Menu, tutorial, settings, pause, rematch and Android input-release work provide a good shell.
7. **Owned provenance.** Current generated assets are repository-owned and do not rely on unlicensed packs. Keep provenance manifests and replace weak output deliberately.

## What merely technically works

- A Solo match can spawn eight participants, run Aandhi, resolve placement, show results and rematch.
- Existing bots can explore, engage, reposition, retreat, recover and loot under a fair FFA perception model.
- The generated presentation prefab/Animator/VFX/audio builders can regenerate a consistent baseline.
- The current APK/AAB can install and route through the documented Lava smoke path.
- Settings have persistent local preferences and lifecycle pause handling.

These facts do not prove 4v4 combat, squad behavior, player-friendly ally targeting, objective pressure, final visual quality, sustained performance or Play submission readiness.

## Prototype-looking or player-facing weaknesses

### Gameplay/product

- The authoritative match is still Solo Raja: no `TeamId`, shared score, team tickets, objective carrier, team wipe, respawn or team result model.
- The primary CTA and mode language do not yet promise a clear, teachable team objective.
- A 4v4 layout cannot be achieved by changing faction colors; allies need non-hostile targeting, spacing, assist behavior and readable team signals.
- Spectator exists as a Solo/result concept but is not defined for a defeated 4v4 fighter's respawn interval.

### AI

- `BotAI` target selection is hostile/faction aware but does not understand roles, crown carrier priority, shrine defense, escort, regrouping, ally blocking or shared cooldown windows.
- There is no team blackboard or deterministic coordination budget. Without one, four bots will look like independent enemies with the same color.
- Current difficulty parameters cover reaction/aim/retreat timing but not decision quality, coordination or objective risk.

### Characters and animation

- The Lava gameplay capture shows chunky, faceted bodies whose proportions and accessories collapse into generic shapes at camera distance.
- The two-bone rig and state clips are useful technical scaffolding, but identical/limited motion does not supply personality, weight, anticipation or readable role language.
- Menu previews are too small and the feature image reads as a background insertion rather than an integrated character-led presentation.

### Environment

- Bazaar structures, props and ground treatment are simple and repeated. Landmarks and cover affordances are not strong enough for a team objective map.
- Current visual dressing must never move authority collision casually. The new map must be designed around validated walkable/collision geometry, then dressed with performance-aware modular art.

### UI/UX

- Current HUD strings such as `ALIVE`, `ZONE`, and `MATCH SPAWN SHIELD` are Solo/debug vocabulary. They do not communicate team score, tickets, crown state, ally health or objective intent.
- Large translucent touch surfaces and runtime text make the match look like a prototype even though they are functional.
- Menu hierarchy is sparse compared with polished mobile references: the character focal point, CTA, mode explanation and return/rematch paths need a stronger composition.

### VFX/audio/game feel

- Current particle cues and generated WAVs prove integration, not impact. Attacks, ability windows, carrier state, healing, tickets and team success need distinct readable layers.
- Reduced-flash behavior and color-blind/team-color redundancy need validation with the final VFX palette.

### Release/performance

- The candidate is `com.example.battleraja.m11`, code 100, debug-signed and temporary. It is not a publishable identity.
- Lava evidence is bounded telemetry; no normalized final-art frame histogram, repeated-match memory-growth, endurance or thermal proof exists.
- The physical Lava reports 4 KB pages; this cannot certify 16 KB page compatibility.
- Data Safety, privacy, content rating, support and permanent signing are drafts/owner gates, not completed submission evidence.

## Architecture and technical debt

### Must preserve

- Domain/application separation, authority ownership, fixed-step rules, seeded randomness and common command contracts.
- Solo compatibility until the 4v4 path has parity and its own regression suite.
- Existing replay/event identities, test harnesses, build entry points and safe lifecycle behavior.

### Must redesign or add

- First-class team identity/relationship API separate from the legacy `CombatFaction` enum.
- Match mode abstraction that can host Solo and Bastion Crown without leaking team state into Solo or presentation-only objects.
- Team score, objective, tickets, respawn/spectator and result snapshots owned by authority.
- Team-aware perception, role assignment, squad blackboard and deterministic command arbitration.
- Data-driven map spawn/socket definitions and authored team visual overlays.

### Debt to control

- Runtime scene/UI construction must not become a reason to leave final screens unstructured. Add explicit screen/component contracts and safe-area tests.
- Generated builder output must have provenance, import validation, LOD/material limits and a human-readable asset manifest.
- Avoid global mutable singletons, runtime `Find*` in hot loops, per-frame allocation and hidden fallback logic.
- Update `Docs/ARCHITECTURE.md`, `Docs/DECISIONS.md`, `Docs/RESEARCH_LOG.md`, balance and release docs when the new design is implemented; this planning pass does not silently rewrite those authorities.

## Proposed V1 product

V1 should ship one excellent offline mode, **Bastion Crown**, on one flagship Bazaar Bastion map. The mode is an original objective-combat game, not a rule-for-rule copy of Gem Grab, Brawl Ball, Knockout or Hot Zone. Full canonical numbers and tie/overtime behavior are in prompt 03.

- Eight fighters: player + three allied bots versus four rival bots.
- A neutral Crown Spark rotates among three authored sockets. A carrier takes it to the allied shrine to score; KOs and team tickets keep combat meaningful.
- Four-minute live match, Aandhi pressure in the final minute, deterministic 30-second maximum overtime.
- Defeated fighters briefly spectate and respawn while team tickets remain; exhausted fighters remain spectators.
- All three fighters and all three gadgets must have objective use cases and readable team feedback.
- `PLAY OFFLINE` goes directly to mode/fighter choice for Bastion Crown. Solo remains hidden behind a secondary route or future flag until it receives its own honest product treatment.

## QA gaps that block a release claim

1. No 4v4 domain/authority tests for team hostility, score, tickets, respawn, objective pickup/drop/deposit, overtime or deterministic tie breaks.
2. No squad AI simulation metrics for ally assistance, objective contribution, role spacing or fair difficulty.
3. No final-art visual review on Lava for all three fighters, gadgets, map, UI, VFX, audio and settings.
4. No normalized frame-time histogram/GC/memory-growth/thermal/battery/endurance report from the final candidate.
5. No physical ARM64 16 KB proof.
6. No permanent package/signing identity or final owner/legal Play materials.

## Required implementation order

Use the files in `PROMPTS/README.md` and the master prompt in order: audit → product/mode contracts → authority/rules → team AI → fighter gameplay → character assets → map/world → gadgets/objectives → VFX/camera → UI → audio → tutorial/accessibility → performance → Lava QA → Play packaging → final regression. The order is a dependency graph, not a checklist to mark without evidence.

## Binary rebuild exit condition

Do not classify BattleRaja as a Play Store Release Candidate until the implementation agent can show: 4v4 player-visible gameplay from cold launch through rematch; real modeled/rigged/animated characters; an authored readable flagship map; team-aware bots; objective/ticket/respawn rules; coherent UI/audio/VFX; complete automated regression; normalized final-art Android performance; physical 16 KB evidence or a clearly documented blocker; a valid release AAB; and honest owner/legal gates. Current status remains Prototype.
