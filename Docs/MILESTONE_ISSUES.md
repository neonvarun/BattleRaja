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

Health, damage, projectiles, cooldowns and pooling. Depends on M1; not part of this execution.

### BR-M3-001 — Bijli kit and shared command API

Data-driven fighter, weapon, gadget and status composition. Depends on M2; not part of this execution.

### BR-M4-001 — Offline bots

Perception, navigation, seeded decisions and profiling. Depends on M3; not part of this execution.

### BR-M5-001 — Offline micro battle royale

Match lifecycle, elimination, zone, placement, restart and spectator flow. Depends on M4; not part of this execution.
