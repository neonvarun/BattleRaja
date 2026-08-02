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

### ADR-012 — Milestone 11 truthful release-candidate preparation

- **Date:** 2026-08-02
- **Status:** Accepted for a local closed-test candidate; publication/signing/legal gates
  remain open.
- **Context:** The candidate must be testable without claiming unavailable online services,
  crash infrastructure, browser/device coverage or store/legal approval.
- **Options considered:** Present the offline lab as a release-ready online game; add
  unauthorised services/keys; or ship a development candidate with compile-safe analytics/
  crash/release seams and explicit draft checklists.
- **Decision:** Use a bounded non-PII development analytics sink, unavailable crash adapter,
  secret/admin guard, local Android/Web artifacts and truthful closed-test/rollback/privacy
  documents. Keep publication, signing, external services and human/legal approval explicit.
- **Consequences:** The exact candidate can be installed and browser-smoked without secrets,
  but it is not a public release, online cross-play candidate or legal compliance claim.
- **Evidence/sources:** M11 prompt, 57 EditMode and 27 PlayMode tests, M11 Android/Web
  artifacts, Lava/Chrome/Edge smoke and release documents.
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

### ADR-013 — Photon Fusion 2.1.1 local setup checkpoint

- **Date:** 2026-08-02
- **Status:** Accepted for local integration preparation; the real-session gate remains open.
- **Context:** The owner supplied the official Fusion 2.1.1 Build 2177 package and created a
  Fusion 2 application. The repository needs a reproducible local SDK/App ID setup without
  putting secrets in source control.
- **Options considered:** Keep the package entirely outside the repository; import an
  unverified SDK; or import the owner-supplied package, record its version, and configure
  only the non-secret App ID while retaining the transport seam.
- **Decision:** Import `Photon-Fusion-2.1.1-Stable-2177.unitypackage` into `Assets/Photon`,
  accept the package-managed `com.unity.nuget.mono-cecil` dependency and Fusion scripting
  defines, and configure `AppIdFusion` in `PhotonAppSettings.asset`. Do not add Photon
  account passwords, secret keys or fabricated real-session evidence.
- **Consequences:** Fusion assemblies are available for compile-time integration work and
  the App ID is ready for a runtime test. Unity `6000.5.6f1` compatibility and the actual
  two-client room remain to be validated; M8/M9 are not reclassified as complete.
- **Evidence/sources:** Official Photon SDK/download and Fusion quickstart documentation;
  local package import exit code 0; Unity compile validation exit code 0; 57/57 EditMode
  and 27/27 PlayMode tests; `PhotonAppSettings.asset` App ID configuration.
- **Owner:** Human project owner

### ADR-014 — Offline match authority boundary

- **Date:** 2026-08-02
- **Status:** Accepted for the offline foundation; pickup/gadget extraction remains open.
- **Context:** `OfflineMatchController` was coordinating the fixed clock, match simulation, zone-damage cadence and Unity damage application in one presentation object.
- **Options considered:** Keep scene timing as the authority; duplicate the rules in a future network adapter; or isolate transport-independent match ticking and damage intents in Core/Application while leaving Unity as an adapter.
- **Decision:** Use `OfflineMatchAuthority` to own the offline simulation reference, fixed-step zone-damage cadence and immutable `DamageRequest` intents. `OfflineMatchController` consumes those intents through the existing central damage resolver. Pickup/gadget proximity and spectator/UI behavior remain presentation adapters until their domain contracts are defined.
- **Consequences:** Zone damage and match resolution have a reusable pure/application seam for future server or Fusion adapters. Unity remains responsible for object lookup, damage presentation and pickup effects. The authority boundary is intentionally partial and is not a real multiplayer claim.
- **Evidence/sources:** `OfflineMatchAuthority`, `OfflineMatchController`, `AuthorityFoundationTests`, 62 EditMode and 27 PlayMode tests.
- **Owner:** Human project owner

### ADR-015 — Authoritative combat statistics and Aandhi warning state

- **Date:** 2026-08-02
- **Status:** Accepted for the offline foundation; assists and complete fixed-tick presentation migration remain open.
- **Context:** Health polling could establish alive/current-health state but could not attribute damage, eliminations or survival time, and the safe-zone result exposed only a radius.
- **Options considered:** Infer results from presentation polling; add ad hoc UI counters; or emit typed combat events into the pure match simulation and expose warning/preview data in immutable tick results.
- **Decision:** Emit `CombatDamageEvent` with instigator, target, applied amount, post-hit health, defeat state and tick. `OfflineMatchSimulation` owns damage dealt, elimination credit, survival time and deterministic timeout ranking. `MatchTickResult` exposes Aandhi warning/closing state, warning time and next radius; Unity renders the data.
- **Consequences:** Results are reusable for future server authority and duplicate elimination credit is rejected. Assists are not yet attributed; weapon attack cooldown now uses the shared 30 Hz tick, while input buffering, movement, projectiles, gadgets and fighter abilities remain separate fixed-tick tasks.
- **Evidence/sources:** `CombatDamageEvent`, `OfflineMatchSimulation.RecordDamage`, `MatchTickResult`, `WeaponCooldownState`, `CombatAttackController`, `OfflineMatchTests`, 66 EditMode and 27 PlayMode tests.
- **Owner:** Human project owner

### ADR-016 — Shared 30 Hz presentation simulation clock

- **Date:** 2026-08-02
- **Status:** Accepted for the offline foundation; full input/ability migration remains open.
- **Context:** The offline authority already used a fixed 30 Hz clock, but player movement,
  weapon cooldown, projectiles, fighter timing and gadget timers could still diverge with
  rendered frame rate.
- **Options considered:** Keep each MonoBehaviour on `Time.deltaTime`; use Unity's variable
  `FixedUpdate` without command identity; or use explicit per-component accumulators that
  consume render time into deterministic 30 Hz steps and carry commands across the boundary.
- **Decision:** Use `FixedSimulationClock` at 30 Hz for the player movement agent, Bijli
  ability presentation, combat projectile stepping, gadget timers and player attack cooldown.
  Render-frame input is sampled into a short-lived buffer and applied on the next authoritative
  step. Bot commands remain injectable through existing command sinks; remaining bot movement,
  ability-runtime cooldowns and online adapters are tracked as follow-up work.
- **Consequences:** These presentation paths now have stable simulation ticks and can be
  compared across render rates without making Unity's render loop the authority. Physics queries
  still occur on Unity's scene objects, and the migration is intentionally incremental.
- **Evidence/sources:** `FixedSimulationClock`, `MovementPlayerAgent`, `BijliFighterController`,
  `CombatProjectile`, `GadgetUser`, `CombatAttackController`, `CoreFoundationTests`, 67 EditMode
  and 27 PlayMode tests.
- **Owner:** Human project owner

### ADR-017 — Application-owned match item collection

- **Date:** 2026-08-02
- **Status:** Accepted for the offline foundation; gadget effect execution remains an adapter concern.
- **Context:** Scene `MatchPickup` and `GadgetPickup` components previously decided availability,
  respawn and collection directly while `OfflineMatchController` scanned actors.
- **Options considered:** Keep item state inside scene components; duplicate pickup rules in a
  future online adapter; or move availability/respawn/collection decisions into pure runtimes
  and let Unity apply only accepted health/inventory effects.
- **Decision:** `OfflineMatchAuthority` owns configured `MatchPickupRuntime` and
  `GadgetPickupRuntime` instances. Collection receives health/inventory observations and returns
  typed results; `OfflineMatchController` supplies proximity and applies those results to Unity
  components. Gadget effect execution remains behind the existing `GadgetUser` presentation
  adapter until effect events have a dedicated application contract.
- **Consequences:** Duplicate collection and respawn rules cannot be accepted by a future client
  path, and item behavior can be tested without a scene. Scene objects remain visual adapters and
  are not the public-match authority.
- **Evidence/sources:** `MatchItems`, `OfflineMatchAuthority`, `OfflineMatchController`,
  `AuthorityFoundationTests`, 68 EditMode and 27 PlayMode tests.
- **Owner:** Human project owner

### ADR-018 — Fighter-specific ability executors behind a common command boundary

- **Date:** 2026-08-02
- **Status:** In progress for the offline alpha; generated-scene runtime coverage remains open.
- **Context:** Pehel and Maya definitions existed, but scene actors routed every ability through
  the Bijli dash-oriented component, making distinct kits presentation-only data.
- **Options considered:** Keep one dash bridge and branch on IDs; add bespoke controllers with
  duplicated input paths; or keep one command/movement contract and select fighter-specific
  executors from data definitions.
- **Decision:** Use `IFighterAbilityController` and `IFighterMovementLock` as the shared boundary.
  Keep Bijli's dash controller, add fixed-tick `PehelFighterController` backed by
  `ChargeThrowRuntime`, and add fixed-tick `MayaFighterController` backed by `DecoyRuntime`.
  The editor generator chooses the executor from the fighter definition; all damage still goes
  through `CombatDamageResolver` and common ability commands.
- **Consequences:** Pehel validates enemy capture, prevents duplicate capture, emits a controlled
  throw and central damage/knockback; Maya creates a targetable, health-bounded decoy with copied
  movement, cooldown and destruction. Full generated-scene PlayMode evidence, VFX/audio and
  production readability remain follow-up work.
- **Evidence/sources:** `FighterAbilityPorts`, `ChargeThrowRuntime`, `DecoyRuntime`,
  `PehelFighterController`, `MayaFighterController`, `BuildEntrypoints`, `VerticalSliceFighterTests`,
  70 EditMode and 27 PlayMode tests.
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

### ADR-020 — Latest-head fixed-tick bot and result-snapshot seam

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 1 authority/fixed-clock continuation; broader
  replay/input buffering and runtime coverage remain open.
- **Context:** The fixed simulation clock had reached movement, combat, gadgets and fighter
  abilities, but bot decisions still used render-frame timing. The match tick also exposed a
  next radius without an explicit next-zone centre, and the reported winner could fall back to
  participant-list order after timeout.
- **Decision:** Run `BotBrain` decisions, navigation progress, movement commands and bot combat
  commands from a 30 Hz `FixedSimulationClock`. Expose `NextZoneCenter` in the immutable
  `MatchTickResult` and consume it in presentation. Select the participant with placement 1 as
  the winner after resolution, with deterministic ranking fallback before resolution. Movement
  command sinks reject commands while a fighter's shared movement-lock port is active.
- **Consequences:** Bot behaviour and result IDs no longer depend on render FPS or actor/list
  order. A future moving-zone definition can populate the explicit next centre without changing
  the bot observation contract. Full client input buffering/replay and physical runtime coverage
  remain required before claiming network-ready authority.
- **Evidence/sources:** `Assets/BattleRaja/Core/Domain/OfflineMatch.cs`,
  `Assets/BattleRaja/Presentation/AI/BotBrain.cs`, latest Phase 1 EditMode 71/71 and
  PlayMode 27/27 test results.
- **Owner:** Human project owner

### ADR-021 — Offline authority owns gadget inventory and use validation

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 1 authority seam; effect execution remains a presentation
  adapter until the combat-effect intent set is expanded.
- **Context:** The offline lab's `GadgetUser` could validate and consume a gadget locally even
  after `OfflineMatchAuthority` became responsible for pickup availability. That would allow a
  future client or scene object to bypass authoritative inventory/cooldown decisions.
- **Decision:** Create per-participant `GadgetInventory` and `GadgetRuntime` instances inside
  `OfflineMatchAuthority`. Expose collection and use requests through the authority. The Unity
  `GadgetUser` mirrors authority-approved pickup/use results and applies the immutable effect as
  presentation; direct local setup remains an explicit request path for offline tests only.
- **Consequences:** Duplicate gadget use is rejected by the application layer and cooldown state
  is no longer authoritative in a MonoBehaviour. Dhol knockback and Tiffin station spawning still
  need typed effect intents before they can be claimed as fully application-owned.
- **Evidence/sources:** `OfflineMatchAuthority`, `GadgetUser`, `AuthorityFoundationTests`,
  latest 72/72 EditMode and 27/27 PlayMode results.
- **Owner:** Human project owner

### ADR-022 — Bazaar Bastion is a controlled production-scene copy

- **Date:** 2026-08-03
- **Status:** Accepted for the M7 vertical slice; final art, audio, UI and content review remain open.
- **Context:** The technical MovementLab fixture proves the simulation and presentation seams but is not an acceptable production-facing arena. M7 needs one authored-feeling scene while preserving the regression fixture and avoiding hand-edited Unity YAML.
- **Options considered:** Mutate MovementLab in place; hand-author a new scene YAML; or use a deterministic editor generator to copy the fixture, apply palette/architecture content and select data-driven fighter adapters.
- **Decision:** Generate `Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity` from the on-disk MovementLab scene through `BuildEntrypoints.CreateBazaarBastionScene`. Keep MovementLab unchanged as the technical fixture, register Bazaar Bastion first in build settings, and configure Pehel/Maya through their shared command and movement-lock boundaries. The generator owns repeatability; the scene asset is committed as the build/test artifact.
- **Consequences:** Android/Web smoke builds and PlayMode tests exercise the same production scene. The current result is a deliberate stylised greybox with palette blocks and stalls, not final art; scene regeneration must be rerun through the editor entrypoint after fixture or content changes.
- **Evidence/sources:** `Assets/BattleRaja/Editor/BuildEntrypoints.cs`, `Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity`, `Assets/BattleRaja/Tests/PlayMode/VerticalSlicePlayModeTests.cs`, Bazaar Bastion Android/Web smoke logs and screenshots.
- **Owner:** Human project owner

### ADR-023 — Use replaceable code-driven presentation primitives before final art

- **Date:** 2026-08-03
- **Status:** Accepted for the presentation foundation; final art/audio approval remains open.
- **Context:** The vertical slice needed readable silhouettes, gameplay state communication and platform-safe feedback, but no licensed production art or audio was available. Animation-event timing would also risk coupling authoritative combat to presentation.
- **Options considered:** Wait for final art; add unlicensed reference assets; or provide original, replaceable primitives and procedural cues behind presentation-only adapters.
- **Decision:** Add `FighterPresentation` for colour rings, health bars, telegraphs, code-driven action states and hit/elimination readability. Add scene-owned `BattleRajaAudioDirector` with generated original tones, optional mixer-group hooks and Web user-gesture gating. Combat controllers notify these adapters after accepted commands; authoritative hit/ability results remain independent of animation/audio.
- **Consequences:** Android/Web and PlayMode tests now exercise readable presentation scaffolding without external asset licensing or autoplay failures. The scene remains a stylised greybox; imported animation, VFX, music, authored SFX, quality tiers and final review are follow-up work.
- **Evidence/sources:** `FighterPresentation.cs`, `BattleRajaAudioDirector.cs`, combat controller integrations, `VerticalSlicePlayModeTests`, phase-4 Android/Web smoke logs and screenshots.
- **Owner:** Human project owner

### ADR-024 — Use an anchored Canvas HUD for the first production match flow

- **Date:** 2026-08-03
- **Status:** Accepted for the M13 UI foundation; full flow, accessibility and final visual review remain open.
- **Context:** The production scene still used an immediate-mode zone/results overlay and
  needed a platform-safe surface for pause, spectator, rematch and settings controls.
- **Options considered:** Keep immediate-mode labels; hand-author a large scene/prefab UI
  before the flow is stable; or create a small runtime Canvas surface with normalized
  anchors and explicit adapter callbacks.
- **Decision:** `OfflineMatchHud` creates a `CanvasScaler`/`GraphicRaycaster` surface when
  needed, keeps status/results presentation separate from `OfflineMatchController`
  authority, and exposes touch-ready buttons for pause/settings, spectator cycling,
  rematch and accessibility-oriented settings. `FighterPresentation.ReducedFlashMode`
  is the presentation hook for reduced flashes. The aim-assist control is labeled as
  ready-only until a real command/aim policy is implemented.
- **Consequences:** Android and Web now exercise a readable, responsive HUD without
  introducing a UI singleton or moving simulation rules into scene code. Bootstrap/main
  menu, localization assets, controller rebinding, safe-area review and final authored
  UI remain follow-up work; Unity 6 uses `LegacyRuntime.ttf` for runtime-created labels.
- **Evidence/sources:** `OfflineMatchHud.cs`, `FighterPresentation.cs`, phase-5 EditMode
  72/72 and PlayMode 29/29 results, Lava Android and Chrome Web smoke screenshots.
- **Owner:** Human project owner

### ADR-025 — Keep production flow pure and bind the selected fighter at the scene boundary

- **Date:** 2026-08-03
- **Status:** Accepted for the first production-flow slice; tutorial, final UX and real
  service gates remain open.
- **Context:** The Canvas match HUD was usable only after opening a gameplay scene directly.
  A product-facing build needs a deterministic bootstrap/menu/mode/fighter/loading/error path,
  while the selected fighter must change the real actor rather than remain a decorative menu
  choice.
- **Options considered:** Put flow state in a Unity MonoBehaviour; make the menu load a
  fixed Bijli scene; or keep transitions in a pure application state machine and apply the
  persisted fighter choice through an explicit presentation boundary on actor 1.
- **Decision:** `ProductionFlowMachine` owns only deterministic flow state, mode/fighter intent
  and error transitions in `BattleRaja.Core.Application`. `ProductionFlowController` renders
  that state in a runtime Canvas, persists local presentation preferences with `PlayerPrefs`,
  asynchronously loads `BazaarBastion`, and reports online/Fusion unavailability without
  fabricating a room. `PlayerFighterSelection` then enables the selected first-party
  `BijliFighterController`, `PehelFighterController` or `MayaFighterController`, updates the
  shared movement-lock and attack definition seams, and leaves bots/match authority unchanged.
- **Consequences:** Bootstrap is first in Android/Web build settings, the production scenes
  are loadable by name, and menu/fighter routing is testable without Unity. Local preferences
  are not authoritative progression or network state. Scene generation must be rerun through
  the editor entrypoint after fixture/content changes; the final authored UI and tutorial are
  still follow-up work.
- **Evidence/sources:** `ProductionFlowMachine.cs`, `ProductionFlowController.cs`,
  `PlayerFighterSelection.cs`, `BootstrapPlayModeTests.cs`,
  `PlayerFighterSelectionPlayModeTests.cs`, 77/77 EditMode and 31/31 PlayMode results,
  `Docs/QA/Visual/Flow/` screenshots, and HEAD `2c36bbb` Android/Web build logs.
- **Owner:** Human project owner

### ADR-026 — Add a replayable tutorial as a presentation layer over real offline authority

- **Date:** 2026-08-03
- **Status:** Accepted for the tutorial/offline onboarding slice; competency certification,
  final UX and full-loop reliability remain open.
- **Context:** The production menu could reach the offline match but offered no guided first-run
  route. The tutorial must teach controls and the Aandhi/gadget/combat vocabulary without creating
  a second simulation or making prompts authoritative.
- **Options considered:** Hard-code tutorial rules into the match controller; create a fake
  training sandbox; or use a replayable, scene-owned overlay backed by a pure step machine while
  keeping the real authority, HUD, pickups and controls active.
- **Decision:** Use `TutorialStepMachine` for deterministic Movement → Aim → BasicAttack → Ability
  → Gadget → Aandhi → Elimination → Victory → Complete progression. `TutorialOverlay` renders
  replay/skip/menu controls and persists only local completion. `CreateTutorialArenaScene` copies
  the tested MovementLab scene, disables BotBrain decisions but keeps eight actor spawns valid,
  and registers `TutorialArena` in the production Android/Web build order.
- **Consequences:** New players can reach a clear, replayable guidance loop from Main Menu, and
  PlayMode verifies the real `OfflineMatchController` starts with eight authority participants.
  Prompts do not certify that a player performed an action; full match reliability, performance,
  visual review and external service gates remain independent.
- **Evidence/sources:** `TutorialStepMachine.cs`, `TutorialOverlay.cs`, `TutorialArena.unity`,
  `TutorialArenaPlayModeTests.cs`, 81/81 EditMode and 32/32 PlayMode results, and the 4391f09
  Android/Web/browser evidence in `Docs/QA/LATEST_HEAD_BASELINE.md`.
- **Owner:** Human project owner

### ADR-027 — Carry authoritative simulation ticks through damage and gadget intents

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 1 authority/fixed-clock continuation; transport-specific
  prediction and replay remain open.
- **Context:** The offline authority already advanced at a fixed step, but Aandhi damage
  requests carried no tick identity and authoritative gadget runtimes were never advanced.
  That weakened auditability and could leave a server-owned gadget cooldown permanently active.
- **Options considered:** Infer intent timing from render callbacks; keep cooldowns in the
  presentation `GadgetUser`; or carry the authoritative tick through application intents and
  advance all application-owned runtimes from the same fixed step.
- **Decision:** Add `SimulationTick` to `DamageRequest` and `MatchAuthorityTick`, require
  monotonic ticks on the explicit authority overload, advance authoritative gadget runtimes
  on every fixed step, and preserve the tick through projectile, Pehel and Aandhi damage paths.
  Keep the float overload as a compatibility helper for pure offline callers; production
  presentation passes the shared match clock tick explicitly.
- **Consequences:** Damage attribution can be ordered and replayed by tick, and gadget
  cooldowns no longer depend on a scene-local timer. The authority still delegates proximity
  sensing and effect rendering to Unity adapters, and real Fusion transport remains blocked.
- **Evidence/sources:** `DamageRequest`, `OfflineMatchAuthority`,
  `OfflineMatchController`, `AuthorityFoundationTests`, and the fixed-clock render-rate
  equivalence test in `CoreFoundationTests`.
- **Owner:** Human project owner

### ADR-028 — Resolve item proximity and collector selection in application authority

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 1 authority continuation; network transport and
  presentation effect execution remain open.
- **Context:** Pickup respawn/inventory state was application-owned, but the presentation
  controller still chose collectors using hard-coded distances and actor iteration order.
  That made a public-match outcome depend on scene callback order and left non-contiguous
  scene pickup IDs fragile.
- **Options considered:** Keep proximity checks in `OfflineMatchController`; add a physics
  service to the scene; or pass authored item positions into application definitions and
  emit deterministic collection intents from the authority.
- **Decision:** Store item position and collection radius in validated domain definitions.
  `OfflineMatchAuthority.CollectNearby` selects the lowest-ID eligible living participant,
  applies the pure pickup/inventory runtime, and returns immutable heal/gadget collection
  intents. Unity applies those intents to health, `GadgetUser` and visual availability only.
  Authority lookup uses authored pickup IDs rather than assuming contiguous arrays.
- **Consequences:** Offline collection outcomes are deterministic and testable without a
  scene, while physics-backed actor positioning and visual effect application remain
  presentation responsibilities. Real Fusion authority still requires a later adapter.
- **Evidence/sources:** `MatchItems`, `MatchCollectionIntents`, `OfflineMatchAuthority`,
  `OfflineMatchController`, `AuthorityFoundationTests`, and the Phase 1 Android/Web smoke
  builds recorded in `Docs/QA/LATEST_HEAD_BASELINE.md`.
- **Owner:** Human project owner

### ADR-029 — Resolve bot abilities from the configured fighter controller

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 2 fighter continuation.
- **Context:** `BotBrain` used `BijliFighterController` as its missing-reference fallback.
  A production bot whose serialized reference was absent could therefore silently issue
  the dash ability even when its actor carried Pehel or Maya.
- **Decision:** Resolve the configured `IFighterAbilityController` directly, or discover
  the interface on the same actor. Do not select a fighter-specific fallback by name.
  Expose the resolved controller for diagnostics and assert in the production PlayMode
  suite that each bot resolves the controller actually attached to its actor.
- **Consequences:** Missing scene references fail safely instead of changing fighter
  identity; authored scene configuration remains responsible for attaching one controller.
  The networked fighter adapter and server-side ability authority remain future work.
- **Evidence/sources:** `BotBrain`, `VerticalSlicePlayModeTests` and the Phase 2 full
  PlayMode result recorded in `Docs/QA/LATEST_HEAD_BASELINE.md`.
- **Owner:** Human project owner

### ADR-030 — Emit authoritative Dhol displacement intents

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 1 authority continuation; full network transport
  and other gadget effects remain open.
- **Context:** Dhol Burst use and cooldown were validated by the authority, but the
  presentation `GadgetUser` independently scanned every movement actor and selected
  knockback targets. That made target selection and displacement scene-order dependent.
- **Decision:** When the authority accepts a Dhol command, it evaluates living
  participants from the application snapshot and returns immutable per-target
  `GadgetDisplacementIntent` values. Unity applies those impulses to matching
  `CharacterController` views. A local non-authoritative lab fallback remains for
  isolated gadget testing only.
- **Consequences:** Public-match target selection is deterministic and testable without
  Physics queries; actual collision movement and VFX remain presentation adapters. Tiffin
  healing/station lifetime and Umbrella mitigation still require the same treatment.
- **Evidence/sources:** `Gadgets`, `OfflineMatchAuthority`, `GadgetUser`, and the Dhol
  displacement assertion in `AuthorityFoundationTests`.
- **Owner:** Human project owner
