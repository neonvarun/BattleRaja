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

### ADR-009 — Milestone 8 authoritative networking seam

- **Date:** 2026-08-02
- **Status:** Accepted for the credential-blocked M8 proof; real Fusion integration is blocked.
- **Context:** Online work must preserve the pure command/domain boundary and cannot make a
  browser tab the trusted authority. Fusion package/App ID/account access is not available
  in this workspace.
- **Options considered:** Put Photon types in Domain; add an unverified package and fake
  cloud-room evidence; isolate transport behind Infrastructure and prove semantics with a
  deterministic mock until approved credentials exist.
- **Decision:** Use client-server topology as the proof baseline with a two-client,
  30-tick configuration. Keep `NetworkInputFrame` as intent, snapshots/damage as
  authoritative outputs, and expose a compile-safe `PhotonFusionAdapter` that returns an
  explicit credential failure. Use deterministic packet-loss profiles for local tests.
- **Consequences:** Core gameplay remains transport-independent and testable. The mock
  validates room/version/authority semantics but cannot establish real prediction,
  reconciliation, interpolation or cross-platform transport behavior. A human must approve
  the Fusion package, App ID and account/licence terms before the full gate.
- **Evidence/sources:** Official Photon Fusion Network Runner, topology, fixed-tick and
  network-condition simulation documentation logged in `Docs/RESEARCH_LOG.md`; 44 EditMode
  and 27 PlayMode tests; M8 report.
- **Owner:** Human project owner

### ADR-010 — Milestone 9 safe server/match preparation while Fusion is blocked

- **Date:** 2026-08-02
- **Status:** Accepted as preparation only; M9 completion is blocked by the M8 real-session
  precondition.
- **Context:** The online-alpha prompt requires eight slots, bot backfill, reconnect and
  server authority, but the approved Fusion package/App ID/account gate is unavailable.
- **Options considered:** Add a fake cloud-room implementation; move network assumptions
  into Domain; or prepare a transport-independent Infrastructure server seam around the
  existing pure offline simulation.
- **Decision:** Use `AuthoritativeMatchServer` with an eight-slot/30-tick config, bounded
  reconnect grace, explicit slot states, deterministic bot backfill and server-owned
  snapshot/health paths. Keep it local and un-deployed until Fusion access exists.
- **Consequences:** Server authority and disconnect policy can be tested without Unity UI or
  an external SDK, while real rooms, cross-play, stress, browser lifecycle and deployment
  remain unverified. No public or paid infrastructure is introduced.
- **Evidence/sources:** M9 prompt, M8 ADR-009, `OfflineMatchSimulation`, M9 EditMode proof
  tests and M9 report.
- **Owner:** Human project owner

### ADR-011 — Milestone 10 backend-neutral progression contract

- **Date:** 2026-08-02
- **Status:** Accepted for the credential-blocked partial gate; real PlayFab integration is
  blocked.
- **Context:** Accounts, cross-progression and rewards must be server-owned, while the PlayFab
  title/SDK/secret configuration is unavailable. Current PlayFab guidance also changes
  anonymous account creation and recommends idempotent Economy v2 inventory operations.
- **Options considered:** Put PlayFab SDK calls in gameplay/UI; add a fake success path that
  resembles a cloud account; or define a backend-neutral interface with a deterministic fake
  and an unavailable real adapter.
- **Decision:** Use `IProgressionBackend`, deterministic fake identity/link/profile/reward/
  leaderboard behavior and `PlayFabBackendAdapter` returning `CredentialsRequired`. Require
  server-validated evidence and idempotency keys for valuable writes; keep premium currency
  ledger-only and purchases out of scope.
- **Consequences:** Progression contracts and security regressions are testable without a
  service. Real identity recovery, cross-device persistence, Economy v2, statistics and remote
  config remain unverified until owner-approved server credentials exist.
- **Evidence/sources:** Official PlayFab authentication, anonymous-login, account-linking,
  Economy v2 inventory and leaderboard documentation logged in `Docs/RESEARCH_LOG.md`; M10
  fake-backend tests and report.
- **Owner:** Human project owner

### ADR-008 — Milestone 7 three-fighter alpha roster

- **Date:** 2026-08-02
- **Status:** Accepted for M7 alpha slice; bespoke specials, final art/audio and human
  balance review remain open.
- **Context:** The offline lab needs a recognizable three-fighter roster without
  duplicating movement/combat authorities or importing unproven third-party art.
- **Options considered:** Bespoke controller per fighter with direct scene mutation;
  mutable shared assets; stable Domain fighter/special definitions plus serialized
  weapon/fighter assets feeding the existing command bridges.
- **Decision:** Add Pehel and Maya as stable data definitions/assets with distinct health,
  movement and attack tuning. Keep special timing/expiry in pure `FighterSpecialDefinition`
  and `DecoyRuntime`; let the current shared Unity bridge carry alpha presentation while
  bespoke charge/throw/decoy visuals remain future work.
- **Consequences:** Three actors can be selected/spawned in the same offline match and
  tested without a new package or external service. Visual identity, tutorial,
  accessibility and bespoke special presentation remain explicit debt.
- **Evidence/sources:** `PROMPTS/07_MILESTONE_7_VERTICAL_SLICE.md`; 40 EditMode and
  27 PlayMode tests; M7 Android/Web smoke evidence.
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

### ADR-006 — Milestone 5 local authoritative match and Aandhi

- **Date:** 2026-08-02
- **Status:** Accepted for M5; full human match-flow review remains open.
- **Context:** M5 needs one complete eight-combatant offline match before any online
  service is introduced.
- **Options considered:** Scene-only timers and UI-owned phase state; a Unity singleton
  match manager; a pure deterministic match simulation bridged by a scene controller.
- **Decision:** Use `OfflineMatchSimulation` with data-driven phase durations, zone
  radii and outside damage. Scene actors sync position/health into the simulation;
  Aandhi damage is applied only through the central damage resolver. Placement,
  elimination and winner state are simulation-owned. A simple health pickup and
  spectator camera bridge complete the lab loop.
- **Consequences:** The 298-second Solo Raja definition can be accelerated in tests
  without changing gameplay rules. Scene restart is the rematch boundary. Loot,
  gadgets, economy, online authority and production UI remain deferred.
- **Evidence/sources:** `PROMPTS/05_MILESTONE_5_OFFLINE_BATTLE_ROYALE.md`; 31 EditMode
  and 22 PlayMode tests; M5 accelerated match/restart evidence.
- **Owner:** Human project owner

### ADR-007 — Milestone 6 data-driven Jugaad gadgets

- **Date:** 2026-08-02
- **Status:** Accepted for M6; balance/readability and device review remain open.
- **Context:** M6 needs three tactical gadgets that work for both the human and bots
  without bypassing the central damage, healing or movement rules.
- **Options considered:** Gadget-specific MonoBehaviours with direct health/Transform
  mutation; mutable ScriptableObject runtime state; pure definitions/inventory/use
  validation bridged by presentation effects and existing combat/movement services.
- **Decision:** Use stable Gadget content IDs and immutable Domain definitions for Umbrella
  Guard, Dhol Burst and Tiffin Station. `GadgetInventory` has capacity one and rejects a
  second pickup unless explicit replacement is requested. `GadgetRuntime` validates held
  item, cooldown, facing and placement before emitting a typed effect. Umbrella mitigation
  is applied by the central damage resolver using projectile hit direction; Dhol uses the
  existing CharacterController; Tiffin is a finite targetable health station. Bots invoke
  the same user path from contextual perception.
- **Consequences:** Three gadgets can be tuned as serialized assets and exercised in pure
  tests. The first station target scan is intentionally sized for eight actors and must
  be replaced by a registered set before scale-up. No Photon, PlayFab or new package is
  required.
- **Evidence/sources:** `PROMPTS/06_MILESTONE_6_JUGAAD_GADGETS.md`; 37 EditMode and
  25 PlayMode tests; M6 Android/Web smoke evidence.
- **Owner:** Human project owner
