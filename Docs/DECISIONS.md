# Architecture and Product Decisions

Record every material choice here. Do not silently overwrite old decisions.

## ADR template

### ADR-000 — Title

- **Date:**
- **Status:** Proposed / Accepted / Superseded
- **Context:**
- **Options considered:**
- **Decision:**
- **Consequences:**
- **Evidence/sources:**
- **Owner:**

### ADR-000 — Milestone 0 toolchain and architecture baseline

- **Date:** 2026-08-02
- **Status:** Accepted
- **Context:** The repository is a Unity-oriented starter without a Unity project, installed Unity editor, Git worktree, or validated Android/Web build modules.
- **Options considered:** Unity 6.0 LTS, Unity 6.3 LTS, Unity 6.4 supported update; mixed external Android toolchains versus Unity-managed dependencies; Unity references in the core versus pure C# core.
- **Decision:** Use Unity `6000.5.6f1`, the 6000.5-compatible URP baseline, Unity-managed Android dependencies, WebGL2/WebAssembly, pure C# Domain/Application assemblies, and separate Presentation/Infrastructure adapters. Use Input System `1.20.0`; accept the editor-resolved URP family `17.5.0` and built-in Test Framework `1.7.0`, plus their transitive package dependencies, as recorded in `Packages/packages-lock.json`.
- **Consequences:** Android child modules must be installed before Android validation. The exact generated package lock is authoritative, and any registry patch adjustment is recorded rather than silently overridden. The temporary local Android application ID is not a store identity.
- **Evidence/sources:** Unity 6000.5.6f1 release notes; Unity 6000.5 Android dependency table; Google Play target API policy; Unity Package Registry snapshot and live package resolution in `Packages/packages-lock.json`; live toolchain evidence recorded in `Docs/RESEARCH_LOG.md`; owner instruction in the current task; `Docs/MILESTONE_0_EXECUTION_PLAN.md`.
- **Owner:** Human project owner

### ADR-001 — Android modules for the approved editor

- **Date:** 2026-08-02
- **Status:** Accepted
- **Context:** Unity 6000.5.6f1 and Web Build Support are installed, while Android Build Support and embedded Android dependencies are absent.
- **Options considered:** Install Android modules through Unity Hub; use an external Android toolchain without Unity module support; change editor versions.
- **Decision:** Install the `android` module with its child SDK/NDK Tools and OpenJDK modules through the installed Unity Hub, then verify the resulting paths and versions before project conversion.
- **Consequences:** The project remains pinned to 6000.5.6f1; the Unity-managed toolchain is the reproducibility baseline. Existing external SDK/JDK/ADB installations remain useful for device inspection but are not the primary Unity build dependency unless the editor reports otherwise.
- **Evidence/sources:** Local editor/module inspection; Unity 6000.5.6f1 release notes; Unity Hub module documentation; Unity 6000.5 Android dependency documentation; owner instruction in the current task.
- **Owner:** Human project owner
