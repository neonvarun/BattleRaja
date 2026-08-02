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

### ADR-003 — Milestone 2 central projectile combat laboratory

- **Date:** 2026-08-02
- **Status:** Accepted for M2; damage balance and feedback remain subject to human playtest.
- **Context:** M2 requires one complete projectile-to-damage loop without prematurely
  hard-coding a named fighter, bots, gadgets, battle-royale state or network SDK.
- **Options considered:** Direct MonoBehaviour health mutation versus a pure damage
  request/pipeline; Rigidbody projectile simulation versus code-driven travel with
  presentation collision queries; unbounded Instantiate/Destroy versus a bounded pool.
- **Decision:** Use typed `CombatEntityId`/faction values, validated immutable
  `DamageRequest` values, a pure `DamagePipeline`/`HealthState`, and a configurable
  `ProjectileWeaponDefinition`. The Unity layer owns a bounded projectile and impact
  pool, performs explicit layer-filtered sphere casts, and sends collision results to
  `CombatDamageResolver`. The training dummy resets after defeat for laboratory use.
- **Consequences:** Health mutation has one auditable path and can be reused by bots
  and future authoritative networking. Projectile collision remains a Unity physics
  concern, while travel/range/lifetime/cooldown/eligibility rules are EditMode-testable.
  The temporary Android application ID is `com.example.battleraja.m2`; this is not a
  store identity. No Photon, PlayFab or named fighter architecture enters M2.
- **Evidence/sources:** `PROMPTS/02_MILESTONE_2_COMBAT.md`; 15 EditMode and 13 PlayMode
  tests; M2 Android/Web build logs; Lava/Oppo ADB smoke; Chrome/Edge local HTTP smoke.
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

### ADR-002 — Milestone 1 movement and camera laboratory

- **Date:** 2026-08-02
- **Status:** Accepted for M1; tuning remains subject to human playtest.
- **Context:** M1 needs a cross-platform movement experiment without introducing combat, networking or a large framework.
- **Options considered:** Unity physics/Rigidbody movement versus a code-driven kinematic motor; orthographic camera versus low-field-of-view perspective; direct transform mutation from input versus command-driven presentation integration; legacy input versus the installed Input System.
- **Decision:** Use a pure C# `MovementMotor` and immutable `MovementCommand` pipeline, apply horizontal displacement through a Unity `CharacterController`, use the Input System `1.20.0` plus builtin uGUI `2.5.0`, and select an elevated orthographic camera by default. Keep a perspective mode in the same controller for repeatable comparison.
- **Consequences:** Movement is frame-step independent and testable without a scene; collision remains a Unity presentation concern; runtime state cannot leak into the shared tuning asset. Touch controls require uGUI and safe-area/pointer reset handling. Perspective remains available for human comparison but is not the M1 default.
- **Evidence/sources:** `Docs/MOVEMENT_LAB.md`; 8 EditMode and 7 PlayMode tests; Unity 6000.5.6f1 package lock; Android and Web M1 development smoke builds; human playtest questions remain open.
- **Owner:** Human project owner

### ADR-004 — Milestone 3 data-driven Bijli fighter and dash state machine

- **Date:** 2026-08-02
- **Status:** Accepted for M3; tuning and physical playtest remain open.
- **Context:** M3 requires one complete fighter without duplicating fighter-specific
  classes or making ability timing depend on Animator/VFX state.
- **Options considered:** A bespoke `BijliController` with hard-coded constants;
  mutable ScriptableObject runtime state; a pure runtime state machine composed from
  stable content IDs and immutable definitions.
- **Decision:** Use `FighterDefinition`/`FighterDefinitionAsset` composition with
  stable fighter, attack and ability IDs. `FighterRuntimeState` owns per-instance
  action phases, cooldown and dash distance. `AbilityCommand` is the bot/network-safe
  input boundary. Unity physics and play-space checks provide an available-distance
  budget; pure C# applies the final bounded displacement. No passive is included.
- **Consequences:** Later fighters can reuse the same definition/runtime/command
  interfaces. Dash collision remains a presentation query, but timing, fallback,
  cooldown and collision truncation are EditMode-testable. The M3 app ID remains
  provisional (`com.example.battleraja.m3`).
- **Evidence/sources:** `PROMPTS/03_MILESTONE_3_BIJLI.md`; 20 EditMode and 16
  PlayMode tests; M3 Android/Web smoke evidence; `FighterRuntimeState` implementation.
- **Owner:** Human project owner

### ADR-005 — Milestone 4 fair offline bot decision loop

- **Date:** 2026-08-02
- **Status:** Accepted for M4; difficulty tuning and human fairness review remain open.
- **Context:** M4 needs seven bots that can fight with Bijli without hidden vision,
  perfect aim, cooldown bypasses or a second gameplay command path.
- **Options considered:** Unity NavMesh/Animator-driven bespoke bots; per-bot direct
  Transform/health mutation; a pure perception/decision model feeding the existing
  movement, attack and ability command ports.
- **Decision:** Use cached target observations with an explicit world-only line-of-sight
  mask, deterministic seeded randomness, target utility scoring, reaction delay and
  bounded aim noise. `BotNavigationRecovery` tracks blocked progress and emits a
  recover state. `BotBrain` submits common commands at a configurable decision interval;
  all combat cooldown and collision rules remain in the existing systems.
- **Consequences:** Bots remain debuggable and testable without final art or navigation
  assets. Dynamic target registration and authored pathfinding remain future work; the
  current lab uses CharacterController collision and bounded explore/recover movement.
  Seven-bot stress evidence is an editor/headless baseline, not a device performance
  guarantee.
- **Evidence/sources:** `PROMPTS/04_MILESTONE_4_BOTS.md`; 26 EditMode and 19 PlayMode
  tests; M4 seven-bot stress log; Android/Web smoke evidence.
- **Owner:** Human project owner
