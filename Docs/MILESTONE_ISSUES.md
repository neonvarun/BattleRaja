# Milestone Issue Plan

Codex must populate this during Milestone 0 with small issue-ready tasks for Milestones 0–5.

Each issue must include:

- ID and title
- Objective
- Scope / non-scope
- Dependencies
- Acceptance criteria
- Tests
- Expected subsystem/files
- Risks
- Human review gate

## Issue-ready sequence

### BR-M0-001 — Freeze toolchain and platform baseline

- **Objective:** Approve Unity `6000.5.6f1`, package matrix, Android/Web modules, target/minimum API, graphics strategy and browser matrix.
- **Scope / non-scope:** Version and environment decisions only; no gameplay or public deployment.
- **Dependencies:** Existing repository and research log.
- **Acceptance criteria:** Decisions recorded with primary sources and owner approval; Unity `6000.5.6f1`, URP `17.5.0`, Input System `1.20.0`, Test Framework `1.7.0`, Android target API 36 and Chrome/Edge smoke targets are recorded.
- **Tests:** Environment version/module checks.
- **Expected subsystem/files:** `Docs/DECISIONS.md`, `Docs/RESEARCH_LOG.md`, `PROJECT_STATUS.md`.
- **Risks:** Unity/package/toolchain drift.
- **Human review gate:** Satisfied by the owner instruction to use `6000.5.6f1` and install required components.

### BR-M0-002 — Bootstrap the Unity root

- **Objective:** Create a clean Unity URP project at the repository root while preserving starter documentation.
- **Scope / non-scope:** Project settings, packages and Bootstrap scene only; no gameplay.
- **Dependencies:** BR-M0-001 and approved Unity installation.
- **Acceptance criteria:** Project opens cleanly, package lock is reproducible, URP is assigned, and the Bootstrap scene is in Build Settings.
- **Tests:** Batch compile, project validation and clean reopen.
- **Expected subsystem/files:** `ProjectSettings/`, `Packages/`, Bootstrap scene.
- **Risks:** Accidental overwrite or generated-file leakage.
- **Human review gate:** Satisfied by the owner instruction in the current task.

### BR-M0-003 — Establish assembly and test boundaries

- **Objective:** Add pure Domain/Application assemblies, Unity-facing adapters, test assemblies and minimal deterministic contracts.
- **Scope / non-scope:** Foundation contracts only; no movement, combat, bots or network SDKs.
- **Dependencies:** BR-M0-002.
- **Acceptance criteria:** Domain compiles without UnityEngine; EditMode and PlayMode smoke tests pass.
- **Tests:** EditMode, PlayMode and dependency-direction validation.
- **Expected subsystem/files:** `Assets/BattleRaja/Core`, `Gameplay`, `Presentation`, `Infrastructure`, `Tests`.
- **Risks:** Circular assembly dependencies.
- **Human review gate:** Satisfied for the M0 boundaries; later gameplay/network boundaries remain deferred.

### BR-M0-004 — Add validation and smoke-build tooling

- **Objective:** Add reproducible Unity CLI validation, Android development APK and Web local build wrappers.
- **Scope / non-scope:** Development artifacts only; no release signing, hosting or store submission.
- **Dependencies:** BR-M0-002 and BR-M0-003.
- **Acceptance criteria:** Validation runs, APK installs/launches, and local Web build starts in supported browsers.
- **Tests:** CLI tests, ADB smoke, local Chrome/Edge smoke.
- **Expected subsystem/files:** `Tools/Validation`, `Tools/Build/Android`, `Tools/Build/Web`.
- **Risks:** Missing modules, device availability, browser coverage.
- **Human review gate:** Completed; M1 work is now active and remains reviewable before M2.

### BR-M0-005 — Record M0 evidence and handoff

- **Objective:** Update status, decisions, research, test, Web and issue documents with exact evidence and limitations.
- **Scope / non-scope:** Documentation and review only; no M1 implementation.
- **Dependencies:** BR-M0-001 through BR-M0-004.
- **Acceptance criteria:** Every M0 acceptance criterion has authoritative evidence or an explicit blocker.
- **Tests:** `git diff --check`, validation summary and artifact review.
- **Expected subsystem/files:** `PROJECT_STATUS.md`, relevant `Docs/` files, package inventory.
- **Risks:** Overclaiming unverified runtime behavior.
- **Human review gate:** Completed for M1 start; review is required before M2.

### BR-M1-001 — Movement and camera lab

- **Objective:** Deliver a cross-platform grey-box movement laboratory with independent movement/aim, camera experiment, desktop bindings and Android touch controls.
- **Scope / non-scope:** Code-driven locomotion, command pipeline, aim indicator, orthographic/perspective camera modes, safe-area virtual sticks, arena geometry and tests. No combat, bots, networking, progression or final content.
- **Dependencies:** Accepted M0 project, Input System/uGUI package baseline, Unity Android/Web modules.
- **Acceptance criteria:** MovementLab is playable in Editor; 8 EditMode and 7 PlayMode tests pass; Android and Web M1 builds succeed; devices install/launch; local Web smoke page returns HTTP 200 in available browsers; camera decision and tuning are documented.
- **Tests:** Pure movement/aim tests, PlayMode spawn/integration/collision/release/indicator/touch reset tests, Android ADB smoke, Chrome/Edge local HTTP smoke.
- **Expected subsystem/files:** `Assets/BattleRaja/Core/Domain`, `Assets/BattleRaja/Presentation/Movement`, `Assets/BattleRaja/Scenes/MovementLab`, `Docs/MOVEMENT_LAB.md`, M1 build tooling/evidence.
- **Risks:** Human touch feel and safe-area coverage remain unverified; formal performance captures are pending; Unity emits known empty-boundary-assembly/licensing warnings.
- **Human review gate:** Required before Milestone 2 combat work.

### BR-M2-001 — Combat foundation

- **Objective:** Add one configurable straight-line projectile, central health/damage
  pipeline, resettable training dummy, cooldowns, collision/faction filtering,
  pooling and shared desktop/Android attack input.
- **Scope / non-scope:** Combat laboratory only; no named fighters, abilities, bots,
  gadgets, Aandhi, match state, Photon, PlayFab, progression or final assets.
- **Dependencies:** M1 movement laboratory and pinned Unity/package baseline.
- **Acceptance criteria:** One playable projectile-to-damage loop; central health
  mutation path; explicit range/lifetime/radius/layer/despawn policy; duplicate-hit
  prevention; bounded pooling; 15 EditMode and 13 PlayMode tests; Android/Web smoke
  builds and authorized device/browser launch evidence.
- **Tests:** `Builds/M2/TestResults/editmode.xml`, `playmode.xml`, ADB runtime logs,
  Chrome/Edge local HTTP checks.
- **Expected subsystem/files:** `Core/Domain` combat rules, `Core/Application`
  attack port, `Presentation/Combat`, MovementLab scene/build entrypoints, M2 report.
- **Risks:** Placeholder balance/feedback and manual touch/focus review remain open;
  development APK retains Unity's non-fatal AssetPackManager probe warning.
- **Human review gate:** Technical gate passed provisionally; subjective combat
  feel and damage tuning require human review before production combat work.

### BR-M3-001 — Bijli kit and shared command API

- **Objective:** Implement a reusable fighter definition/runtime boundary and the
  first complete Bijli bolt-and-dash kit.
- **Scope / non-scope:** Stable fighter/attack/ability IDs, immutable content asset,
  pure dash state machine, shared attack/ability commands, placeholder HUD/trail,
  collision/bounds checks and tests. No passive, Pehel, Maya, bots, gadgets, match
  loop, networking, backend or final art.
- **Dependencies:** M2 combat laboratory and pinned Unity/package baseline.
- **Acceptance criteria:** Bijli is spawnable in MovementLab; bolt and dash work end
  to end; startup/active/recovery/cooldown are not Animator-dependent; invalid and
  concurrent actions are rejected; Android/Web builds and smoke evidence exist.
- **Tests:** 20 EditMode and 16 PlayMode tests in M3 result XML; ADB and browser logs.
- **Expected subsystem/files:** `Core/Domain` fighter/ability contracts,
  `Presentation/Combat` fighter bridge/HUD/input, Bijli assets, MovementLab generator,
  M3 report/build wrappers.
- **Risks:** Placeholder balance/feedback, physical touch and formal performance
  review remain open; development APK keeps Unity's non-fatal warnings.
- **Human review gate:** Required for dash feel, bolt balance, HUD/touch ergonomics and
  final release; not blocking the provisional M4 bot implementation.

### BR-M4-001 — Offline bots

- **Objective:** Add fair, deterministic and debuggable Bijli bot opponents using the
  common human command interfaces.
- **Scope / non-scope:** Cached perception with world line of sight, utility target
  selection, reaction delay, aim noise, attack/dash command output, explore/engage/
  retreat/recover states, seven-bot stress setup and debug overlay. No Aandhi, loot,
  gadgets, match resolution, Pehel/Maya, Photon, PlayFab or final art.
- **Dependencies:** M3 Bijli fighter and pinned Unity/package baseline.
- **Acceptance criteria:** Seven bots spawn and fight, never read hidden actors,
  respect cooldown/collision, recover from stuck movement, remain intentionally
  imperfect, and have measured stress evidence plus Android/Web smoke builds.
- **Tests:** 26 EditMode and 19 PlayMode tests; seven-bot stress timing in the M4 log.
- **Expected subsystem/files:** `Core/Domain/BotAI.cs`, `Presentation/AI`, MovementLab
  bot generator, M4 build wrappers/report and docs.
- **Risks:** Lab navigation is bounded CharacterController recovery rather than authored
  pathfinding; performance evidence is headless-editor only; visual fairness requires
  human review.
- **Human review gate:** Required for bot fairness/readability and low-end-device
  performance; not blocking provisional M5 offline match work.

### BR-M5-001 — Offline micro battle royale

- **Objective:** Deliver one complete offline eight-combatant Solo Raja match with
  Aandhi pressure, elimination, placement, spectator/results and rematch.
- **Scope / non-scope:** Pure match phases/zone/placement, separated spawns, central
  outside-zone damage, simple neutral health pickups, seven-bot actor integration,
  results HUD and scene restart. No gadgets, Pehel/Maya, networking, accounts,
  progression, monetisation or production release.
- **Dependencies:** M4 bots, M3 combat/fighter layers and pinned toolchain.
- **Acceptance criteria:** The data definition targets 4–6 minutes; eight actors start
  separated; Aandhi phases and damage are authoritative; eliminations are idempotent;
  winner/placement/spectator/rematch state works; 20 accelerated matches complete.
- **Tests:** 31 EditMode and 22 PlayMode tests; accelerated 20-match soak evidence.
- **Expected subsystem/files:** `Core/Domain/OfflineMatch.cs`, `Presentation/Match`,
  MovementLab generator, M5 build wrappers/report and docs.
- **Risks:** Match controller is a grey-box bridge; pickup/results readability and
  full five-minute physical playthrough remain open; no formal memory capture.
- **Human review gate:** Required for match pacing, zone readability, spectator flow,
  touch/results UX and low-end-device performance; not blocking provisional M6 gadget
  work.

### BR-M6-001 — Jugaad gadget system

- **Objective:** Add three tactical gadgets with shared pickup/use rules for the human
  and seven bots in the offline match.
- **Scope / non-scope:** Stable definitions/assets, one-slot inventory, spawn eligibility,
  Umbrella Guard, Dhol Burst, Tiffin Station, central damage/healing/movement routing,
  contextual bot use, HUD/touch feedback and tests. No additional gadgets, Pehel/Maya,
  networking, backend, progression, monetisation or final art.
- **Dependencies:** M5 offline match, central combat health/resolver and pinned
  Unity/package baseline.
- **Acceptance criteria:** Three gadgets work in offline matches; pickup/use is validated;
  counterplay is documented; no obvious infinite shield, displacement or healing loop;
  37 EditMode and 25 PlayMode tests plus Android/Web smoke evidence.
- **Tests:** `Builds/M6/TestResults/editmode.xml`, `playmode.xml`, ADB runtime logs and
  Chrome/Edge local HTTP checks.
- **Risks:** Primitive telegraphs and station scan are grey-box; device logs retain a
  non-fatal SphereCollider creation warning from the existing projectile pool; balance,
  readability and low-end performance need human review.
- **Human review gate:** HR-006 is open and required before production tuning/release;
  it does not block provisional M7 planning.

### BR-M7-001 — Three-fighter vertical slice

- **Objective:** Make Bijli, Pehel and Maya a cohesive offline alpha roster while
  preserving shared command and simulation boundaries.
- **Scope / non-scope:** Stable fighter/special data, distinct weapon/movement tuning,
  serialized assets, scene roster and tests. Final art/audio, full bespoke special
  presentation, tutorial, menus, accessibility, networking, backend and monetisation
  remain outside the implemented checkpoint.
- **Dependencies:** M6 gadgets, M5 offline match and pinned Unity/package baseline.
- **Acceptance criteria:** Three distinct definitions/assets are playable in the lab;
  offline match/gadgets remain stable; Android/Web builds and smoke evidence exist.
- **Tests:** 40 EditMode and 27 PlayMode tests in M7 result XML.
- **Risks:** Grey-box identity, shared alpha special bridge, missing tutorial/accessibility
  and non-fatal IL2CPP SphereCollider warning require follow-up.
- **Human review gate:** HR-007 is open and required before release-quality claims;
  it does not block provisional M8 networking work.

### BR-M8-001 — Photon Fusion access is unavailable

- **Objective:** Complete a real two-client Android/Web networking proof with authoritative
  snapshots, prediction/reconciliation and transport-condition validation.
- **Current state:** Compile-safe Infrastructure contracts, deterministic mock and tests are
  complete. No Photon Fusion package, App ID, license/account approval or runtime session
  credentials are available, so the real gate is blocked.
- **Required human action:** Approve/create the Photon Fusion application and provide the
  package/version plus non-secret local configuration through an approved channel. Never
  commit App IDs/secrets.
- **Unblocking evidence:** One Lava Android client and one desktop Web client join the same
  room, exchange shared commands, observe server-owned snapshots/damage, and pass controlled
  latency/jitter/loss and reconnect checks with logs.
- **Status:** Blocked; mock-only technical pass is not a real online pass.
