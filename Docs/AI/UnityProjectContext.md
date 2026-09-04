# BattleRaja Unity Project Context

**Planning snapshot:** 2026-09-05
**Verified source:** `codex/v1-playstore-release` at `feb9258` (`ui: surface friendly Bastion squad status`)
**Working directory:** `C:\Projects\BattleRaja`
**Status:** Bastion Crown 4v4 offline implementation, deterministic replay/squad hardening,
fair production pacing, original menu/fighter art, saved lifecycle animation states,
authoritative pickup/terminal boundaries and the friendly squad status strip are
integrated and test-green. The latest current-source pass is covered by full EditMode
173/173 and PlayMode 100/100, with strict 100-match production-bot simulation evidence
carried from the unchanged authority checkpoint. Final release gates remain open; see
`Docs/QA/V1_AUTHORITY_PICKUP_TERMINAL_BOUNDARIES_2026-09-05.md` for exact hashes and
Lava evidence.

This file is the compact orientation document for the V1 continuation agent. The agent must re-run repository, build and device checks because this snapshot can become stale. `PROJECT_STATUS.md`, `AGENTS.md`, and the source code remain the authority for current facts; the prompt pack is the execution contract for the next implementation pass.

## Product direction

BattleRaja is an original, stylised, top-down 3D mobile arena game. The V1 player-facing mode is **Bastion Crown**, an offline 4v4 team battle:

- Team Raja: one human plus three friendly AI bots.
- Rival team: four enemy AI bots.
- Eight fighters total, no network, no account, no ads, no IAP.
- Bijli, Pehel and Maya remain the initial roster; no fourth fighter is required until human playtests prove a role gap.
- Aandhi is retained as a pressure/overtime system, not as a reason to remove objective play.
- Existing Solo Raja/battle-royale foundations are preserved as a secondary/future mode when they remain healthy, but `PLAY OFFLINE` must launch the polished team mode.

The canonical rules live in `PROMPTS/03_4V4_MATCH_RULES_RESPAWN_SCORE_AND_OBJECTIVE.md`. Do not invent a second set of timers, team colors, score names or objective terminology in another stage.

## Verified repository baseline

The latest continuation is recorded in
`Docs/QA/V1_ANIMATION_STATE_AND_RESPAWN_PRESENTATION_2026-09-04.md`.
Static validation is **0/0**, EditMode is **168/168**, PlayMode is **99/99**, the
two-seed 8,400-tick Bastion replay soak remains green, and strict production-bot PlayMode
is **99/99** with 100/100 terminal matches. The exact APK/AAB hashes and the explicit
prior-revision Lava limitation in that report supersede the older baseline values below.

- The working tree contains the focused continuation changes and must be rechecked before
  any further edits; the release handoff records the resulting commit.
- The published baseline is `9c7e8cfdf417e4196d94bfe5e7837ba44b6959a9`; local `main` is
  historical and behind the release branch.
- `main` is historical and behind origin; do not plan against it.
- LFS pointer check passed.
- Stashes exist and are user-owned; do not apply, delete or rewrite them.
- The current continuation adds the bounded-lag squad blackboard, local 16 m enemy
  perception with authored LOS, eligible 18 m support fallback, deterministic support/escort
  handoffs, same-tick command-phase freeze, unique team placement after respawn and fair
  production pacing on top of the Bastion Crown baseline.

## Engine and package baseline

- Unity `6000.5.6f1`.
- URP `17.5.0`.
- Input System `1.20.0`.
- Unity Test Framework `1.7.0`; package lock: `Packages/packages-lock.json`.
- Android: IL2CPP, min API 28, target API 36, ARM64 candidate.
- Web and future networking seams exist in the repository but are out of V1 product scope.
- Photon/PlayFab must not enter the core domain or be enabled for the offline V1 build.

## Architecture map

The intended dependency direction is:

```text
Core.Domain (pure C# rules and value types)
        ↓
Core.Application (authority, commands, replay and orchestration)
        ↓
Gameplay composition / adapters
        ↓
Presentation (Unity scenes, controllers, UI, animation, VFX, audio)
        ↓
Infrastructure (input, persistence, platform/build adapters)
```

Important existing pieces:

- `Assets/BattleRaja/Core/Domain/OfflineMatch.cs` — Solo compatibility phases plus the explicit Bastion match definition.
- `Assets/BattleRaja/Core/Domain/BastionCrownContracts.cs` and `BastionCrownMatch.cs` — immutable team/objective contracts and deterministic mutable match rules.
- `Assets/BattleRaja/Core/Application/OfflineMatchAuthority.cs` — canonical movement, actions, cooldowns, projectiles, gadgets and fighter runtime.
- `Assets/BattleRaja/Core/Domain/BotAI.cs` and `Assets/BattleRaja/Presentation/AI/BotBrain.cs` — Solo utility baseline plus Bastion role/plan intent routing.
- `Assets/BattleRaja/Presentation/Match/OfflineMatchController.cs` — Unity mirror/controller around authority state.
- `Assets/BattleRaja/Presentation/AI/BotBrain.cs` and `BotPerceptionSensor.cs` — presentation-side bot driving/perception adapters.
- `Assets/BattleRaja/Presentation/Flow/ProductionFlowController.cs` — runtime menu, fighter selection, tutorial/settings navigation and local preferences.
- `Assets/BattleRaja/Presentation/Match/OfflineMatchHud.cs` — HUD, controls, settings and team-objective/results surface.
- `Assets/BattleRaja/Presentation/Visuals/BastionCrownObjectiveView.cs` — collider-free Crown socket/shrine telegraphs and carrier visual mirror.
- `Assets/BattleRaja/Editor/BuildEntrypoints.cs` — deterministic scene/build composition for the canonical 1+3 versus 4 production setup.

Preserve pure simulation, common human/bot commands, seeded randomness, fixed-step authority, data-driven definitions, replay identity and test seams. Do not make Unity objects or ScriptableObject assets the owner of mutable match state.

## Current gameplay truth

The current production path is Bastion Crown: one human and three friendly Raja bots versus four Rival bots, with Crown Spark objective play, KOs, shared tickets, protected respawn, Aandhi pressure and team results. `CombatFaction.Player/Enemy` remains a presentation compatibility enum; `BastionCrownMatch` is the authoritative team model. Solo Raja remains explicit in domain/application fixtures and is not advertised by the primary menu. The team mode uses exactly eight actor slots and never creates an accidental ninth actor.

## Current presentation and asset truth

The repository contains a useful, owned generated baseline:

- `ProductionArtBuilder` saves faceted meshes, deterministic small textures and materials.
- `ProductionPresentationBuilder` saves a two-bone presentation rig, Animator controller/clips and particle cue prefabs.
- `ProductionEnvironmentBuilder` saves a Bazaar environment/prefab and materials.
- `ProductionAudioBuilder` saves deterministic PCM WAVs and mixer groups.
- `BuildEntrypoints` now references `Art/V1/BattleRaja-FeatureArt-OriginalCandidate.png`, an
  original Bazaar Bastion shrine/fighter key-art candidate with no vehicles or racing motifs;
  isolated fighter previews, closer camera and warm/cool lighting remain presentation-only.

Those assets are editable and provenance-safe. The current implementation adds a readable Crown Spark objective view, team-coloured shrine/socket rings, carrier tint/slowdown, authored fighter portraits and a team HUD while preserving the existing provenance boundary. The saved fighter Animator now also exposes `GadgetUse`, `CrownPickup`, `CrownCarry`, `CrownDeposit`, `KO`, `Respawn` and `Spectator`; `FighterPresentation` consumes only immutable Bastion snapshots, and the controller clears a defeated view only after confirmed authority respawn. Generated images are not acceptable substitutes for a modeled, rigged, animated gameplay character; commissioned final art, mix, accessibility and human-fun review remain open.

## Scenes and flow

- `Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity` is the current Bastion Crown gameplay scene.
- `Assets/BattleRaja/Scenes/Tutorial/TutorialArena.unity` is the tutorial route.
- The production scene composes one Raja player, three Raja allies and four Rival bots around the Bazaar arena; the controlled editor generator enforces actor IDs 1–8 and faction/team identity.
- Main menu, mode selection, fighter selection, loading, tutorial, settings, pause, spectator, results and rematch are runtime-built/controlled; the primary route now advertises Bastion Crown 4v4 while keeping Solo fixtures explicit.

## Tests and evidence baseline

The current implementation checkpoint is:

- Static validation: `0/0` reported issues.
- EditMode: `168/168` passed, including Bastion replay soak, planner metrics, dead-target
  event-identity regressions and the saved presentation asset contract.
- PlayMode: `99/99` passed, including canonical production composition, objective telegraphs/HUD,
  Crown pickup/deposit, gadget-use animation and confirmed respawn presentation through the
  controller adapter.
- Deterministic replay: the Bastion v2 combined digest reproduces two 8,400-tick seeded
  matches with zero divergence; the older 1000-seed production-bot evidence remains Solo-only.
- Seeded Solo production-bot health: 100/100 matches in the documented 240–360 second window, with combat, bot-to-bot damage, gadget use and no stuck/protected/invalid failures.
- These are strong local gates, not evidence that final authored presentation/performance or physical-device behavior is complete.

## Android/device facts

Approved physical device for evidence: Lava `ST5GDW23LB004392` (`LAVA_LXX508`, Android 14/API 34). The Oppo `b60e53b3` is not an approved evidence device. A prior inventory recorded BattleRaja package `com.example.battleraja.m11`, Brawl Stars `com.supercell.brawlstars` and Smash Karts `com.tallteam.citychase` on that Lava, but the inventory was not reverified in this rerun.

Current candidate artifacts are temporary/debug identity, not publishable:

- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,687,756 bytes, SHA-256 `9F2E70034CFF9B0DE4A04490B084E1FCEBFD1B8C703C9B1FC5A3D8D0D692B613`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,513,267 bytes, SHA-256 `750EB62BF1012BB54938E0402FF2E0D84B3450145180D95186AA0B4A8C84A13D`.
- Package metadata: version name `1.0.0`, code `100`, min API 28, target API 36, ARM64; package ID `com.example.battleraja.m11` is temporary and debug-signed.

The approved Lava `ST5GDW23LB004392` is not visible in the current ADB inventory, so no
device result is claimed for this exact source. The visible Oppo `b60e53b3` is outside the
evidence scope and must not be used. Prior Lava captures remain historical and do not
substitute for a current-source install, physical 16 KB runtime proof or normalized
performance/endurance evidence.

## Research and policy anchors

The implementation agent must re-check official sources before changing unstable requirements. The planning pass recorded:

- Android/Play target API: https://developer.android.com/google/play/requirements/target-sdk
- Android page sizes and 16 KB validation: https://developer.android.com/guide/practices/page-sizes
- Play Data Safety: https://support.google.com/googleplay/android-developer/answer/10787469
- Play content rating: https://support.google.com/googleplay/android-developer/answer/9859655
- Play review preparation: https://support.google.com/googleplay/android-developer/answer/9859455

Record URLs, dates, claims and decisions in `Docs/RESEARCH_LOG.md`. Use installed Brawl Stars and Smash Karts only for high-level observation of product principles. Do not extract, decompile, intercept, purchase, copy or ship their assets, terminology, layouts or trade dress.

## Open release gates

The current classification remains **Prototype — Android offline release candidate in progress**. Open work includes complete human Lava comfort/fun/accessibility review across all fighters, gadgets, Aandhi, tutorial, results and rematch; sustained normalized performance/thermal/battery/endurance; physical 16 KB proof; final authored models/textures/animation/VFX/audio mix and cultural review; permanent package/signing identity; privacy/Data Safety; and owner/legal Play Console material. No public upload or owner-only release action was performed.

## Agent operating rule

Start every implementation stage by rechecking source, scene, tests and current artifact. Preserve user changes. Keep one coherent V1 design, update decisions/research when architecture or policy changes, and leave an evidence trail that another engineer can reproduce. A stage is not complete because files exist; it is complete only when its binary gate and visual/device evidence pass.
