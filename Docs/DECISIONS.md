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

### ADR-058 — Unify action eligibility behind the authority clock

- **Date:** 2026-08-25
- **Status:** Accepted for the V1 gameplay-truth slice.
- **Context:** Attack, movement, displacement, damage, healing, gadget, decoy and
  station effects previously checked phase rules in several places. That made it
  possible for one action type to gain a warmup/protection exception while another
  stayed correctly gated, and it scattered combat-phase policy through call sites.
- **Options considered:** Duplicate phase checks at every new action; add a separate
  eligibility service outside the authority; or centralize actor/action eligibility in
  `OfflineMatchAuthority` while preserving action-specific typed rejection reasons.
- **Decision:** Add one authority-owned active-combat eligibility check covering
  Opening through Final Circle for live known actors. Route movement, ability
  displacement, attacks, fighter abilities, decoy spawning/damage, direct/projectile
  damage, healing, gadget use/station effects/healing and outside-zone damage through
  it before state mutation. Keep existing action-specific failure enums at the public
  boundary and reject without consuming command identities.
- **Consequences:** Noncombat setup and resolution states cannot mutate canonical
  combat state through an action side door. Tests now use the canonical 241-tick
  30 Hz boundary instead of scattered 240/250 assumptions. Production PlayMode tests
  explicitly advance the pure match to Opening rather than depending on earlier test
  order. Future action types must call this eligibility path before mutation.
- **Evidence/sources:** `OfflineMatchAuthority.GetActionEligibility`,
  `IsCombatActionPhase`, focused/full EditMode results under
  `Builds/Local/V1GameplayTruth/TestResults`, full PlayMode result, and the new
  unified-eligibility regression.
- **Owner:** Human project owner

### ADR-059 — Keep offline Solo bots fair in free-for-all combat

- **Date:** 2026-08-25
- **Status:** Accepted for V1 offline gameplay truth.
- **Context:** The presentation scene labeled every bot `CombatFaction.Enemy`, while
  bot target selection treated that label as hostility. In a true eight-way Solo
  free-for-all this could make bots avoid legitimate fights or reason about the wrong
  relationships. Bot attack range also used tactical preference plus a constant rather
  than the equipped weapon's maximum range.
- **Options considered:** Introduce seven unique presentation factions; move all bot
  decisions into match authority; or retain presentation-side perception and give the
  pure decision model explicit self-faction plus weapon facts.
- **Decision:** Extend bot perception with the actor's own faction and weapon
  definition. Solo bots ignore same-faction and neutral actors, respect
  `ProjectileWeaponDefinition.MaxRange` for attacks, and use gadgets only when a
  visible hostile exists. Scene generation supplies fighter-specific weapon assets.
  `CombatFaction` remains a presentation compatibility label; canonical damage still
  uses authority-owned combat groups.
- **Consequences:** Bots no longer rely on stale enemy/all-player assumptions. Weapon
  range is data-driven and cannot exceed projectile reach. Future team mode must pass
  explicit authority-backed groups into bot perception. This does not yet prove human
  fairness/feel approval, balance quality or navigation quality.
- **Evidence/sources:** `BotPerceptionSnapshot`, `BotDecisionEngine.SelectTarget`,
  `GadgetUser.UseForContext`, production scene generation, new free-for-all bot
  regression, full EditMode 133/133, PlayMode 75/75 and 2,000-match zero-divergence soak.
- **Owner:** Human project owner

### ADR-000 — Milestone 0 toolchain and architecture baseline

- **Date:** 2026-08-02
- **Status:** Accepted
- **Context:** The repository is a Unity-oriented starter without a Unity project, installed Unity editor, Git worktree, or validated Android/Web build modules.
- **Options considered:** Unity 6.0 LTS, Unity 6.3 LTS, Unity 6.4 supported update; mixed external Android toolchains versus Unity-managed dependencies; Unity references in the core versus pure C# core.
- **Decision:** Use Unity `6000.5.6f1`, the 6000.5-compatible URP baseline, Unity-managed Android dependencies, WebGL2/WebAssembly, pure C# Domain/Application assemblies, and separate Presentation/Infrastructure adapters. Use Input System `1.20.0`; accept the editor-resolved URP family `17.5.0` and built-in Test Framework `1.7.0`, plus their transitive package dependencies, as recorded in `Packages/packages-lock.json`.
- **Consequences:** Android child modules must be installed before Android validation. The exact generated package lock is authoritative, and any registry patch adjustment is recorded rather than silently overridden. The temporary local Android application ID is not a store identity.
- **Evidence/sources:** Unity 6000.5.6f1 release notes; Unity 6000.5 Android dependency table; Google Play target API policy; Unity Package Registry snapshot and live package resolution in `Packages/packages-lock.json`; live toolchain evidence recorded in `Docs/RESEARCH_LOG.md`; owner instruction in the current task; `Docs/MILESTONE_0_EXECUTION_PLAN.md`.
- **Owner:** Human project owner

### ADR-054 — Keep V1 Android fully offline and disable Unity service telemetry

- **Date:** 2026-08-24
- **Status:** Accepted for the V1 offline Android candidate; revisit only through an
  owner-approved service/data-safety decision.
- **Context:** V1 is intended to require no account or internet connection. The project
  configuration still had the Unity analytics-submission flag enabled even though Unity
  Connect, Analytics, Ads and Performance Reporting services were disabled and no runtime
  upload adapter existed.
- **Decision:** Set `submitAnalytics: 0` in `ProjectSettings.asset`, retain all Unity
  service `m_Enabled: 0` settings, and make repository validation fail if the offline
  candidate re-enables analytics, Ads or Performance Reporting. Keep the bounded
  in-memory development analytics fixture test-only and non-identifying.
- **Consequences:** The V1 manifest/privacy worksheet now match the intended offline,
  no-upload behavior. Any future online service or telemetry change requires a new
  data-safety review, policy update and explicit owner approval.
- **Evidence/sources:** `ProjectSettings/ProjectSettings.asset`,
  `ProjectSettings/UnityConnectSettings.asset`, `Tools/Validation/validate.ps1`,
  `Docs/PRIVACY_DATA_SAFETY_WORKSHEET.md`, and the exact analytics-disabled APK/AAB gate.
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

### ADR-031 — Tick Tiffin healing and lifetime in application authority

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 1 authority continuation; station damage and
  network transport remain open.
- **Context:** Tiffin healing and expiry were driven by `GadgetStation.Update` using
  `Time.deltaTime` and a scene-wide health scan. In a public match that would make
  healing cadence and lifetime presentation-owned.
- **Decision:** Add a pure `GadgetStationRuntime` owned by `OfflineMatchAuthority`.
  Accepted Tiffin uses receive a station ID; fixed authority ticks emit immutable
  healing intents and station-expiry IDs. Unity renders the station and applies those
  intents to actor health. Authority-driven station views no longer run their own
  healing/lifetime loop; isolated local-lab stations retain the fallback loop.
- **Consequences:** Tiffin healing cadence and expiry are deterministic and testable
  without a scene. Station damage forwarding and authoritative Umbrella mitigation are
  still required before claiming complete gadget rule separation.
- **Evidence/sources:** `Gadgets`, `OfflineMatchAuthority`, `GadgetStation`,
  `OfflineMatchController`, `AuthorityFoundationTests` and the 85/34 Phase 1 results.
- **Owner:** Human project owner

### ADR-032 — Tick Umbrella Guard mitigation in application authority

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 1 authority continuation.
- **Context:** Umbrella Guard facing, duration and 30% front mitigation were held in
  `GadgetUser`, so damage resolution could depend on a presentation component and its
  local clock.
- **Decision:** `OfflineMatchAuthority` owns one `UmbrellaGuardRuntime` per participant,
  activates it only after an accepted authoritative use, advances it on authority ticks,
  and rewrites incoming damage requests before the presentation pipeline. Aandhi and
  generic damage bypass the guard. The old `GadgetUser` mitigation path remains only for
  isolated non-authoritative labs.
- **Consequences:** Shield duration and mitigation are deterministic and testable without
  Unity; visual facing/feedback still lives in the presentation adapter. Station damage
  forwarding and network replication remain open.
- **Evidence/sources:** `Gadgets`, `OfflineMatchAuthority`, `CombatDamageResolver`,
  `GadgetUser` and the Umbrella authority assertions in `AuthorityFoundationTests`.
- **Owner:** Human project owner

### ADR-033 — Route Tiffin station damage through application authority

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 1 authority continuation; network replication and
  broader presentation extraction remain open.
- **Context:** Tiffin healing and lifetime were already fixed-tick authority rules, but
  station damage still mutated only the Unity `CombatHealth` view. A client could therefore
  destroy a rendered station without changing the canonical station runtime.
- **Decision:** `OfflineMatchAuthority.TryDamageStation` validates and applies station
  damage against the authoritative `GadgetStationRuntime`, returns the applied amount and
  destruction state, and removes destroyed runtimes immediately. `CombatDamageResolver`
  validates the target request before forwarding it, then applies the returned amount to the
  Unity view and expires that view when authority reports destruction. The local station loop
  remains only for isolated non-authoritative lab objects.
- **Consequences:** Station destruction and remaining health are deterministic and testable
  without Unity, and the presentation station cannot be the source of public-match outcome.
  The adapter still renders local health feedback, while Fusion transport, server-side event
  replication and other scene-owned presentation details remain future work.
- **Evidence/sources:** `Gadgets`, `OfflineMatchAuthority`, `OfflineMatchController`,
  `CombatDamageResolver`, `AuthorityFoundationTests`, `GadgetPlayModeTests`, and the
  `phase1-station-authority-*` test/build evidence in `Docs/QA/LATEST_HEAD_BASELINE.md`.
- **Owner:** Human project owner

### ADR-034 — Keep Web host and orthographic gameplay framing responsive

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 7 visual continuation; mobile-Web quality and final
  human UX approval remain open.
- **Context:** The built-in Web template emitted a fixed 960×600 desktop canvas. At a
  390×844 browser viewport, the host introduced a horizontal crop even after the game
  camera had enough world framing to support the portrait aspect.
- **Decision:** Copy Unity's installed Default Web template into the project as
  `Assets/WebGLTemplates/BattleRaja`, select it with the project template path, and let
  the desktop/resized canvas fill the host viewport. `TopDownCameraController` expands
  orthographic size only for aspects narrower than the 16:9 reference, preserving the
  existing landscape framing while keeping more of the arena visible in portrait.
- **Consequences:** Browser layout and gameplay projection now respond together and are
  testable with a 390×844 Playwright smoke. This does not certify touch ergonomics,
  tutorial/results layout, mobile-Web performance or final visual quality.
- **Evidence/sources:** `Assets/WebGLTemplates/BattleRaja`, `BuildEntrypoints`,
  `TopDownCameraController`, `VerticalSlicePlayModeTests`,
  `playwright-390x844-responsive-gameplay.png` and the `phase7-responsive-*` evidence
  in `Docs/QA/LATEST_HEAD_BASELINE.md`.
- **Owner:** Human project owner

### ADR-035 — Compact match telemetry on narrow viewports

- **Date:** 2026-08-03
- **Status:** Accepted for the Phase 5/7 UI continuation; final visual and mobile UX
  approval remain open.
- **Context:** The responsive canvas fixed the portrait crop, but the original single-line
  match status still squeezed the long `SPAWNPROTECTION` phase and zone values beside
  the fighter and gadget HUD. This reduced readability on a 390×844 viewport.
- **Decision:** Format match telemetry as two lines, shorten warning labels, and switch
  to a compact `Z current > next` representation below a 0.75 aspect ratio. The
  presentation-only formatter remains driven by immutable authority properties; no
  gameplay rule or timing changes are introduced.
- **Consequences:** Portrait telemetry is more legible while landscape retains the
  labelled format. The HUD still requires human review for density, touch ergonomics,
  localization and final visual hierarchy.
- **Evidence/sources:** `OfflineMatchHud`, `VerticalSlicePlayModeTests`,
  `playwright-390x844-hud-compact.png` and the `hud-compact-*` evidence in
  `Docs/QA/LATEST_HEAD_BASELINE.md`.
- **Owner:** Human project owner

### ADR-036 — Preserve per-step tick identity across render-frame catch-up

- **Date:** 2026-08-03
- **Status:** Accepted for the fixed-clock foundation; replay recording, network
  prediction and longer soak coverage remain open.
- **Context:** A render frame can accumulate multiple 30 Hz simulation steps. The
  clock advanced all of them before presentation loops ran, so consumers that reused
  the final `Tick` sent duplicate identities to `OfflineMatchAuthority`. The exact
  Android Lava smoke exposed `Simulation ticks must increase monotonically` while the
  automated single-step tests stayed green.
- **Decision:** `FixedSimulationClock` records the number of steps consumed by the
  latest render frame and exposes `GetConsumedTick(stepIndex)`. Every fixed-step
  presentation consumer uses the corresponding per-step identity: authority, movement,
  attacks, projectiles, bots, gadgets, and Bijli/Pehel/Maya ability adapters.
- **Consequences:** Catch-up frames preserve monotonic command, damage and authority
  identities without making the render loop authoritative. The correction does not
  alter the 30 Hz rule rate or claim production networking.
- **Evidence/sources:** `FixedSimulationClock`, `OfflineMatchController`, all fixed-step
  consumers, `CoreFoundationTests`, `fixed-tick-runtime-*` test/build logs, and Lava
  before/after captures in `Docs/QA/Visual/Phase7/`.
- **Owner:** Human project owner

### ADR-037 — Do not treat fighter targets as Pehel charge walls

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical-slice controller boundary; networked
  authority and broader collision policy remain future work.
- **Context:** The live Pehel controller's sphere cast used the all-collision mask. An
  opposing `CombatTarget` collider could therefore be returned as the nearest wall,
  reducing available travel to zero and putting the ability into cooldown before the
  active-phase capture query ran.
- **Decision:** Use a non-allocating sphere-cast buffer and ignore colliders that belong
  to `CombatTarget` objects when selecting static obstacles. Target capture remains a
  separate faction/radius check; static geometry and play bounds still constrain the
  charge. Invalid runtime `FighterDefinitionAsset` component lookups are removed because
  the definition is a `ScriptableObject` and serialized scene references already provide
  the intended data path.
- **Consequences:** Live Pehel capture/throw works against a real target collider without
  sacrificing wall blocking or adding per-frame cast allocations. The controller remains
  a presentation adapter around the pure `ChargeThrowRuntime`; this does not make the
  client authoritative for public matches.
- **Evidence/sources:** `PehelFighterController`, `MayaFighterController`,
  `BijliFighterController`, `PlayerFighterSelection`, `VerticalSlicePlayModeTests`,
  `phase2-full-*` results, the Phase 2 Android Lava sample and Chrome Web smoke capture.
- **Owner:** Human project owner

### ADR-038 — Publish offline results when authoritative damage ends the match

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; network replication and final
  results UX remain future work.
- **Context:** `CombatDamageResolver` records lethal damage into the offline authority
  immediately. When the last elimination left one participant alive, the domain match
  entered `Resolution` before the next controller tick; the controller then returned
  early for an ended simulation and never published `Results` to the HUD.
- **Decision:** Centralize result publication in `OfflineMatchController.PublishResults`.
  Call it both from the normal `MatchTickResult.MatchEnded` path and immediately after
  an authority-recorded damage event observes `Simulation.IsEnded`. Add PlayMode
  coverage for the generated Results panel and Rematch button reload.
- **Consequences:** Results/rematch state is available regardless of whether the match
  ends on a fixed simulation tick or during a presentation damage callback. The rule
  remains offline-only; networked result replication, final art and human UX review are
  not implied.
- **Evidence/sources:** `OfflineMatchController`, `OfflineMatchPlayModeTests`,
  `Builds/M11/TestResults/phase6-full-playmode-20260803.xml` and the Phase 6 baseline
  in `Docs/QA/LATEST_HEAD_BASELINE.md`.
- **Owner:** Human project owner

### ADR-039 — Attribute assists from authoritative damage contributions

- **Date:** 2026-08-03
- **Status:** Accepted for the offline match simulation; network event replication and
  balance review remain future work.
- **Context:** Participant snapshots exposed an `Assists` field, but the match simulation
  only tracked the finishing instigator's elimination and total damage. That made results
  incomplete and left assist credit outside the authoritative rules.
- **Decision:** Keep a per-target, per-participant damage contribution ledger in the pure
  `OfflineMatchSimulation`. On a valid lethal event, credit one assist to each living,
  non-finishing participant who contributed damage, reject duplicate post-elimination
  events, and discard the target ledger. Environmental damage and self-damage never create
  assist credit.
- **Consequences:** Results now expose deterministic assist counts without Unity or
  transport dependencies. The ledger is event-driven and bounded by active match targets;
  server replication, assist thresholds and final balance remain open.
- **Evidence/sources:** `OfflineMatchSimulation.RecordDamage`,
  `OfflineMatchTests.DamageContributionsCreditAssistOnceToNonFinisher`,
  `Builds/M11/TestResults/assist-editmode-20260803.xml` and
  `Builds/M11/TestResults/assist-playmode-20260803.xml`.
- **Owner:** Human project owner

### ADR-040 — Keep aim assist as bounded local input guidance

- **Date:** 2026-08-03
- **Status:** Accepted for the offline accessibility surface; final balance and networked
  input-policy review remain open.
- **Context:** The settings flow persisted an aim-assist preference, but no gameplay path
  applied it and the in-match toggle was a no-op. Any assist must improve touch/mouse
  usability without granting a client authority over hit or damage outcomes.
- **Decision:** Add a pure `AimAssistTargeting` selector that considers only live targets
  inside a configured range/cone and uses quantized angular scoring, distance and entity ID
  for deterministic ties. `PlayerInputAdapter` gathers candidates through a fixed
  `OverlapSphereNonAlloc` buffer and only adjusts the local aim direction; projectile
  collision and damage remain unchanged. The match HUD toggle persists the setting and
  updates the adapter immediately.
- **Consequences:** Aim assist is bounded, testable and available on Android/Web input
  without per-frame managed collection growth. It does not claim server authority,
  auto-targeting outside the cone or final accessibility/balance approval.
- **Evidence/sources:** `AimAssistTargeting`, `PlayerInputAdapter`, `OfflineMatchHud`,
  `AimAssistTests`, `Builds/M11/TestResults/aimassist-editmode-v5-20260803.xml`,
  `Builds/M11/TestResults/aimassist-playmode-20260803.xml` and the fresh Phase 7 smoke
  captures in `Docs/QA/Visual/Phase7/`.
- **Owner:** Human project owner

### ADR-041 — Use Unity Input System only with a legacy-scene compatibility bridge

- **Date:** 2026-08-03
- **Status:** Accepted for the current Android/Web project baseline; serialized legacy
  scene cleanup and broader controller rebinding remain future work.
- **Context:** The project had both legacy `StandaloneInputModule` scenes and the new
  Input System package enabled. Unity's `activeInputHandler: Both` generated an editor
  warning and allowed presentation code to keep depending on the legacy `UnityEngine.Input`
  API, while switching directly to Input System would break user-owned serialized scenes.
- **Decision:** Set `ProjectSettings.activeInputHandler` to Input System only. Generated
  scenes now use `InputSystemUIInputModule`, audio gesture detection uses Input System
  device state, and `InputModuleCompatibilityBridge` replaces any legacy module at scene
  load so existing scenes remain runnable without rewriting their YAML in place.
- **Consequences:** Android and Web builds share one input API and no longer depend on
  legacy polling in project code. The bridge is a transitional runtime boundary; vendor
  Photon code and old serialized scene data may still contain legacy module references,
  and controller rebinding, touch ergonomics and final input QA remain open.
- **Evidence/sources:** `ProjectSettings/ProjectSettings.asset`,
  `BuildEntrypoints`, `BattleRajaAudioDirector`, `InputModuleCompatibilityBridge`,
  `Builds/M11/TestResults/input-system-playmode-fixed-20260803.xml`, the successful
  Android/Web M11 builds, and `Tools/Validation/validate.ps1`.
- **Owner:** Human project owner

### ADR-042 — Route resolved combat events through application authority

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; network transport and remaining
  presentation adapters remain future work.
- **Context:** `OfflineMatchController` subscribed to Unity health callbacks but then called
  `OfflineMatchSimulation.RecordDamage` directly. That made the presentation bridge the
  visible owner of the canonical combat-statistics mutation path even though the simulation
  itself was pure.
- **Decision:** Expose `OfflineMatchAuthority.RecordDamage(CombatDamageEvent)` as the
  application-owned event ingress. The controller reports immutable resolved events through
  the authority; the authority delegates to the simulation and retains ownership of
  elimination, placement, damage and assist outcomes. Add a duplicate-elimination regression.
- **Consequences:** The offline presentation bridge no longer reaches into the pure simulation
  for combat mutation. Unity still applies health visuals and transport integration is not
  implied; a future network adapter must call the same authority contract under server rules.
- **Evidence/sources:** `OfflineMatchAuthority`, `OfflineMatchController`,
  `AuthorityFoundationTests.MatchAuthorityRoutesDamageEventsAndRejectsDuplicateEliminations`,
  `Builds/M11/TestResults/authority-routing-editmode-20260803.xml` and the latest baseline.
- **Owner:** Human project owner

### ADR-043 — Resolve production actor damage before Unity view mutation

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; movement reconciliation, network
  transport and remaining presentation adapters remain future work.
- **Context:** The previous event bridge applied damage to a Unity `CombatHealth` first and
  then reported the resulting event to the match simulation. That ordering made a view-side
  health mutation precede canonical statistics/elimination state and could double-count or
  accept late events.
- **Decision:** `OfflineMatchAuthority.ResolveDamage` validates mitigation and applies actor
  damage to the pure simulation first. `CombatDamageResolver` applies only the returned
  authoritative health snapshot/event to registered match actors; non-authority lab targets
  continue through the local pipeline. Results are published immediately when an authoritative
  damage event ends the match.
- **Consequences:** Production actor damage, eliminations, placements and statistics no longer
  depend on a controller callback after view mutation. `SyncHealth` remains a transitional
  reconciliation seam for the offline adapter, and real server authority is not implied.
- **Evidence/sources:** `OfflineMatchAuthority.ResolveDamage`, `OfflineMatchSimulation.ApplyDamage`,
  `CombatHealth.ApplyAuthoritativeDamage`, `AuthorityFoundationTests`,
  `OfflineMatchPlayModeTests.CombatDamageResolverAppliesAuthorityDamageOnceToViewAndSnapshot`,
  `Builds/M11/TestResults/authority-damage-editmode-20260803.xml`,
  `Builds/M11/TestResults/authority-damage-playmode-20260803.xml` and the latest baseline.
- **Owner:** Human project owner

### ADR-044 — Apply production healing through match authority

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; movement reconciliation and network
  transport remain future work.
- **Context:** Health pickup and Tiffin healing were accepted by application runtimes but
  then mutated Unity `CombatHealth` directly. The controller mirrored view health back into
  authority each render frame, leaving a presentation-owned reconciliation path.
- **Decision:** Add `OfflineMatchSimulation.Heal` and `OfflineMatchAuthority.ApplyHealing`.
  Pickup and Tiffin intents update canonical participant health first, then the controller
  applies the resulting snapshot to Unity via `CombatHealth.SetAuthoritativeHealth`.
  Keep `SyncHealth` only as an explicit compatibility seam for tests and the transport proof.
- **Consequences:** Production pickup/Tiffin healing no longer depends on render-frame view
  state. Movement still enters authority through a transitional position observation seam, and
  local lab pickups/stations remain presentation adapters by design.
- **Evidence/sources:** `OfflineMatchSimulation.Heal`, `OfflineMatchAuthority.ApplyHealing`,
  `CombatHealth.SetAuthoritativeHealth`, `AuthorityFoundationTests.MatchAuthorityAppliesHealingToCanonicalHealth`,
  fresh authority-health EditMode/PlayMode XML and the latest baseline.
- **Owner:** Human project owner

### ADR-045 — Resolve production movement in offline match authority

- **Date:** 2026-08-03
- **Status:** Accepted for the offline Bazaar Bastion vertical slice; fighter ability
  displacement, Photon transport and server deployment remain future work.
- **Context:** Production movement still copied the Unity transform into the simulation
  each render frame, allowing presentation state to become the effective owner of actor
  placement and making duplicate/local movement possible.
- **Decision:** Register one `MovementMotor` and `MovementTuning` per authority participant.
  `OfflineMatchController` submits fixed-tick `MovementCommand` values to
  `OfflineMatchAuthority.ResolveMovement`; the authority applies the pure motor exactly
  once per monotonically increasing tick and returns an immutable step/position snapshot.
  Bazaar Bastion uses this path, while MovementLab retains its observation path for local
  movement regression fixtures. The presentation adapter applies the canonical position
  directly and disables its local `CharacterController` in authority mode so Unity
  collision projection cannot reject a valid canonical result.
- **Consequences:** Production actor placement is now canonical in the offline authority,
  with duplicate-tick rejection and PlayMode coverage. Fighter ability movement is not yet
  authority-owned and therefore remains explicitly out of scope for this continuation;
  this is not a real multiplayer or trusted-server claim.
- **Evidence/sources:** `OfflineMatchAuthority.ResolveMovement`, `MovementMotor`,
  `OfflineMatchController`, `MovementPlayerAgent`, `AuthorityFoundationTests`,
  `VerticalSlicePlayModeTests`, `Builds/M11/TestResults/authority-movement-editmode-20260803-final.xml`,
  `Builds/M11/TestResults/authority-movement-playmode-20260803-final2.xml` and the latest baseline.
- **Owner:** Human project owner

### ADR-046 — Apply Dhol Burst displacement through canonical match state

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; fighter ability movement and
  network transport remain future work.
- **Context:** The authority computed Dhol Burst displacement intents, but the Unity
  gadget adapter applied them only through `CharacterController.Move`. In the production
  authority movement path that controller is disabled to prevent local collision
  projection from rejecting canonical movement, so Dhol could appear to succeed without
  changing the authoritative participant position.
- **Decision:** `OfflineMatchAuthority.TryUseGadget` applies each valid Dhol displacement
  to the canonical simulation before returning the immutable intent. The controller then
  applies the resulting snapshot through `MovementPlayerAgent`; a local controller move
  remains only as a fallback when no match adapter is available.
- **Consequences:** Dhol Burst now changes canonical and presentation positions through
  the same authority boundary and duplicate gadget commands remain rejected. Collision
  resolution, Pehel charge displacement and Bijli dash displacement are still not
  authority-owned and must not be described as network-ready.
- **Evidence/sources:** `OfflineMatchAuthority.TryUseGadget`, `GadgetUser`,
  `OfflineMatchController.ApplyAuthoritativeDisplacement`, `MovementPlayerAgent`,
  `AuthorityFoundationTests.MatchAuthorityOwnsGadgetUseAndRejectsDuplicateCommands`,
  `Builds/M11/TestResults/authority-gadget-displacement-editmode-20260803.xml` and
  `Builds/M11/TestResults/authority-gadget-displacement-playmode-20260803.xml`.
- **Owner:** Human project owner

### ADR-047 — Route fighter ability displacement through the authority seam

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; ability runtime ownership,
  collision policy and network transport remain future work.
- **Context:** Bazaar Bastion disables local `CharacterController` projection while
  movement is authority-driven. Bijli dash, Pehel charge and Pehel throw therefore
  needed an explicit path to update the canonical participant position rather than
  silently relying on a disabled Unity controller.
- **Decision:** Add a tick-validated `OfflineMatchAuthority.ResolveAbilityDisplacement`
  contract. Bijli and Pehel adapters submit their already-resolved displacement through
  `OfflineMatchController` when the actor is authority-driven; the returned canonical
  position is applied to `MovementPlayerAgent`. Non-authority lab fixtures retain their
  local controller fallback. Invalid, duplicate-tick, dead-actor and non-finite requests
  are rejected without mutating simulation state.
- **Consequences:** Production ability displacement now changes the same canonical
  position used by authority movement, and the live Bijli path has PlayMode coverage.
  Fighter cooldown/runtime state, collision projection and authoritative ability command
  validation are still presentation-owned or transitional; this does not establish a
  trusted multiplayer server.
- **Evidence/sources:** `OfflineMatchAuthority.ResolveAbilityDisplacement`,
  `BijliFighterController`, `PehelFighterController`, `MovementPlayerAgent`,
  `AuthorityFoundationTests.MatchAuthorityResolvesAbilityDisplacementExactlyOncePerTick`,
  `VerticalSlicePlayModeTests.ProductionBijliAbilityRoutesDisplacementThroughAuthority`,
  and the latest baseline.
- **Owner:** Human project owner

### ADR-048 — Keep production Maya decoys in match authority

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; network transport and final
  fighter presentation remain future work.
- **Context:** Maya's presentation controller created a decoy `CombatHealth` and
  `CombatTarget` directly. In Bazaar Bastion that allowed decoy lifetime, follow
  position and damage to bypass the same authority boundary used by participant
  movement and combat.
- **Decision:** `OfflineMatchAuthority` owns one deterministic `DecoyRuntime` per
  participant, validates tick-ordered spawn and damage, advances the decoy from the
  canonical owner snapshot, and exposes immutable `MatchAuthorityDecoy` snapshots.
  `MayaFighterController` consumes those snapshots to create/update/destroy only the
  Unity view. `CombatDamageResolver` routes authority decoy damage through the
  application seam; non-authority lab probes retain the local runtime path.
- **Consequences:** Production Maya decoy health, lifetime, follow position and duplicate
  damage rejection are now authority-owned and regression-tested. The view still uses a
  generated capsule placeholder, and fighter ability command validation/cooldown policy
  is transitional rather than a real network-server implementation.
- **Evidence/sources:** `OfflineMatchAuthority.TrySpawnMayaDecoy`,
  `OfflineMatchAuthority.ResolveMayaDecoyDamage`, `MayaFighterController`,
  `CombatDamageResolver`, `AuthorityFoundationTests.MatchAuthorityOwnsMayaDecoyLifetimeAndDamage`,
  `VerticalSlicePlayModeTests.ProductionMayaDecoyRoutesLifetimeAndDamageThroughAuthority`,
  and the latest baseline.
- **Owner:** Human project owner

### ADR-049 — Give Bazaar Bastion its own production-scene contract

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; prefab extraction and network
  transport remain future work.
- **Context:** `CreateBazaarBastionScene` recreated the production scene by copying
  `MovementLab.unity`, and the resulting Bazaar scene retained a `MovementLabScene`
  marker. That coupled production serialization to a lab fixture and made rerunning the
  editor entrypoint capable of importing unrelated lab changes or dropping build scenes.
- **Decision:** The entrypoint now opens the existing `BazaarBastion.unity`, validates and
  updates its production graph in place, removes the lab-only marker, and serializes a
  `BazaarBastionScene` contract with player, camera, match, projectile-pool and damage
  resolver references. Architecture creation is idempotent, and TutorialArena remains
  in EditorBuildSettings. MovementLab remains an independent observation fixture.
- **Consequences:** Production scene ownership is explicit and rerunnable without copying
  user-owned lab serialization. The scene is still greybox and project gameplay actors are
  not yet extracted into reusable prefabs; those remain the next production-content step.
- **Evidence/sources:** `BuildEntrypoints.CreateBazaarBastionScene`,
  `BazaarBastionScene`, `BazaarBastion.unity`,
  `VerticalSlicePlayModeTests.ProductionSceneUsesFighterSpecificAbilityControllers`,
  `Builds/M11/TestResults/bazaar-boundary-playmode-final-20260803.xml`,
  `Builds/M11/TestResults/bazaar-boundary-editmode-full-20260803.xml` and the latest
  baseline.
- **Owner:** Human project owner

### ADR-050 — Extract reusable Bazaar architecture into a prefab boundary

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; fighter actor prefab extraction
  and final authored content remain future work.
- **Context:** Bazaar architecture was serialized as a large inline hierarchy inside the
  production scene, while the editor generator still owned the construction details. That
  made scene diffs noisy and prevented reusable content validation.
- **Decision:** Save the existing `BazaarArchitecture` hierarchy as
  `Assets/BattleRaja/Content/Prefabs/BazaarArchitecture.prefab` with Unity's
  `PrefabUtility.SaveAsPrefabAssetAndConnect`. `CreateBazaarBastionScene` is idempotent:
  it creates the hierarchy only when absent, connects it to the prefab, and validation
  requires the asset. Fighter and gameplay definitions remain separate data assets.
- **Consequences:** The production scene now references a reusable architecture asset and
  the generator no longer expands the geometry into every scene copy. The prefab contains
  greybox geometry/material references only; actor prefabs, authored art, animation, VFX
  and final review remain open.
- **Evidence/sources:** `Assets/BattleRaja/Content/Prefabs/BazaarArchitecture.prefab`,
  `BuildEntrypoints.EnsureBazaarArchitecturePrefab`,
  `Builds/M11/TestResults/bazaar-prefab-editmode-full-20260803.xml`,
  `Builds/M11/TestResults/bazaar-prefab-playmode-full-20260803.xml` and the latest
  baseline.
- **Owner:** Human project owner

### ADR-051 — Resolve production Pehel charge throw in match authority

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; world-collision projection,
  transport replication and final ability presentation remain future work.
- **Context:** Production Pehel previously advanced its charge runtime and selected
  capture targets inside `PehelFighterController`. That made the visible controller the
  source of target selection, damage and throw displacement, even though Bazaar movement
  was already canonical in `OfflineMatchAuthority`.
- **Decision:** Register authenticated participant factions with the authority and keep a
  `ChargeThrowRuntime` per Pehel actor in `OfflineMatchAuthority`. The authority validates
  the common ability command and tick, selects the nearest living enemy from canonical
  snapshots with an entity-id tie-break, applies ability damage, and commits throw
  displacement before returning an immutable `MatchAuthorityChargeThrow`. The Unity
  controller consumes those results only to update health/transform views; MovementLab
  retains the non-authority local runtime fallback.
- **Consequences:** Production Pehel cooldown/state, capture, damage and throw position no
  longer depend on a client collider query or local ability runtime. The authority still
  receives a fixed offline arena-distance budget because the offline domain has no world
  collision map; this is not a real Fusion server implementation.
- **Evidence/sources:** `OfflineMatchAuthority.TryStartPehelCharge`,
  `OfflineMatchAuthority.AdvancePehelCharge`, `PehelFighterController`,
  `AuthorityFoundationTests.AuthorityOwnsPehelChargeCaptureDamageAndThrowDisplacement`,
  and the next full EditMode/PlayMode baseline.
- **Owner:** Human project owner

### ADR-052 — Validate production attack commands in match authority

- **Date:** 2026-08-03
- **Status:** Accepted for the offline vertical slice; projectile collision, network
  replication and broader rule migration remain future work.
- **Context:** Production `CombatAttackController` previously owned the fire-rate gate
  and could spawn a projectile after only local presentation checks. That allowed
  duplicate or stale commands to bypass the same fixed-tick authority boundary used by
  movement and fighter abilities.
- **Decision:** Add `OfflineMatchAuthority.TryAcceptAttack` as the transport-independent
  validation seam. It rejects invalid/non-finite commands, unknown or defeated actors,
  duplicate/out-of-order ticks and cooldown violations, and consumes an authority-owned
  `WeaponCooldownState`. `CombatAttackController` submits the common command and only
  spawns the presentation projectile after an accepted result; the HUD reads the
  authority cooldown for production actors. Core assembly dependency checks and a
  presentation-mutation scan now enforce the boundary in repository validation.
- **Consequences:** Production attack ordering, alive-state validation and cooldown
  policy are deterministic and testable without Unity. Projectile instantiation and
  collision projection remain Unity presentation responsibilities, so this is not yet
  a trusted multiplayer combat implementation. The Pehel throw path also resolves its
  damage through the existing authority seam while world collision remains offline.
- **Evidence/sources:** `OfflineMatchAuthority.TryAcceptAttack`,
  `CombatAttackController`, `OfflineMatchController`,
  `AuthorityFoundationTests.MatchAuthorityRejectsDuplicateAndOutOfOrderAttackCommands`,
  `OfflineMatchPlayModeTests.ProductionAttackCommandsUseAuthorityOrderingAndCooldown`,
  `Tools/Validation/validate.ps1`, and the Phase 1 continuation in
  `Docs/QA/LATEST_HEAD_BASELINE.md`.
- **Owner:** Human project owner

### ADR-053 - Anchor attack cooldowns to the canonical match clock

- **Date:** 2026-08-22
- **Status:** Accepted
- **Context:** The Phase 1 authority audit found that `TryAcceptAttack` bounded
  caller-supplied command ticks above (`FutureTick`) but not below. A producer could
  submit long-outdated ticks that passed per-actor ordering checks and consume weapon
  cooldown entirely in the past, bypassing fire rate.
- **Options considered:** Trust producer ticks with ordering checks only; clamp cooldown
  consumption silently; or reject clearly stale input and anchor consumption/reporting to
  `max(command tick, authority tick)`.
- **Decision:** Reject commands older than `MaxAttackInputStalenessTicks` (2) behind the
  canonical clock with a distinct `MatchAuthorityAttackFailure.StaleTick` reason, and
  anchor accepted attacks' cooldown consumption and reporting to
  `max(command.SimulationTick, _lastSimulationTick)`. Production controllers and bots
  already stamp commands with the current canonical tick, so no production behavior
  changes; the rejection window only affects untrusted/lagging producers.
- **Consequences:** Fire-rate policy can no longer be compressed by replayed or lagging
  input regardless of producer behavior. Follow-up verification confirmed gadget use
  (`GadgetRuntime`), Pehel charge (`ChargeThrowRuntime`) and Maya decoy (`DecoyRuntime`)
  cooldowns are already seconds-based and advanced only by authority fixed steps, so
  their caller-supplied ticks carry ordering identity only and cannot bypass any rate
  limit; no further staleness window is required there. Transport-level duplicate-event
  dedup remains Phase 8 scope.
- **Evidence/sources:** `OfflineMatchAuthority.TryAcceptAttack`,
  `AuthorityFoundationTests.MatchAuthorityRejectsStaleAttackCommandsAndAnchorsCooldownToAuthorityClock`
  (EditMode 115/115), PlayMode 57/57 at commit `ee573ad`, validate.ps1 0 errors/0 warnings.
- **Owner:** Human project owner

### ADR-055 - Own combat groups and publish projectile damage per tick

- **Date:** 2026-08-25
- **Status:** Accepted for the offline Solo Raja V1 authority.
- **Context:** Solo Raja must be a true eight-participant free-for-all, but the
  seven production bots shared `CombatFaction.Enemy`. The same view faction also made
  projectile selection and Pehel capture reject valid bot-to-bot relationships. Separately,
  authority projectiles applied canonical damage during projectile advancement but did not
  place the resulting event in the canonical tick, so visible health and elimination
  feedback could lag until another damage source mirrored state.
- **Options considered:** Give every bot a unique `CombatFaction` value; allow friendly fire
  globally; or add an authority-owned combat-group relationship while retaining
  `CombatFaction` as a presentation compatibility label. For health parity, options were
  polling views or publishing already-applied authoritative events in the same tick.
- **Decision:** Add authority-owned positive combat groups, defaulting each Solo Raja
  participant to its own group and allowing explicit groups in a future team mode. Projectile,
  Pehel capture/throw and Maya decoy eligibility use different combat groups rather than view
  factions. Authority projectile hits now return their stable `CombatDamageEvent` in
  `MatchAuthorityTick.DamageEvents`; presentation continues to mirror immutable results only.
  Aim assist considers living non-neutral targets other than the player's own fighter/decoy,
  while stations remain excluded.
- **Consequences:** Bot-to-bot attacks, damage, eliminations and credit are valid in true
  Solo Raja. Canonical health, visible health, elimination presentation, perception removal and
  spectator transition use the same projectile outcome without presentation mutation. Combat
  groups are included in replay hashing. This does not implement full team modes, complete
  action-eligibility unification or transport-level duplicate suppression.
- **Evidence/sources:** `OfflineMatchAuthority`, `DeterministicReplayRunner`,
  `ChargeThrowRuntime`, `PlayerInputAdapter`, focused authority tests, and
  `VerticalSlicePlayModeTests.ProductionBotProjectileUpdatesHealthEliminationPerceptionAndSpectator`.
- **Owner:** Human project owner

### ADR-056 - Harden deterministic replay identity and malformed input rejection

- **Date:** 2026-08-25
- **Status:** Accepted for the offline V1 replay foundation.
- **Context:** The replay audit found that assist contributions, simulation-local damage
  identity counters, the next station identity, arena collision content and decoy-damage tick
  keys were absent or incomplete in canonical hashing. Station/decoy traversal relied on
  dictionary enumeration order for projectile ties. A non-finite movement command could also
  enter the motor and throw during a shared authority tick.
- **Options considered:** Trust dictionary insertion order and validate only at collision;
  expose mutable simulation internals; or add sorted read-only snapshots/content hashing and
  reject malformed commands before mutation.
- **Decision:** Add sorted damage-contribution snapshots to the pure simulation. Hash those
  contributions, last/emitted damage identities and next station ID. Content-address arena bounds,
  radius, version and ordered obstacles. Hash decoy damage by decoy ID. Sort station and decoy
  traversal used by projectile tie-breaking. Reject non-finite movement/aim before motor evaluation.
- **Consequences:** Replay hashes detect more future-affecting state and arena changes, while
  projectile ties no longer depend on dictionary iteration order. One malformed command cannot halt
  later participants in the same tick. This does not yet add Bijli dash replay support or production
  presentation capture.
- **Evidence/sources:** `OfflineMatchAuthority.CalculateDeterministicTickHash`,
  `OfflineMatchSimulation.GetDamageContributions`, `ArenaCollisionDefinition.CalculateStableHash`,
  `DeterministicReplayRunner.MatchStateHashBuilder`, focused replay/authority regressions and deep soak.
- **Owner:** Human project owner

### ADR-057 - Own Bijli dash state in the match authority

- **Date:** 2026-08-25
- **Status:** Accepted for the offline V1 gameplay-truth foundation.
- **Context:** Production Bijli advanced `FighterRuntimeState` in its Unity controller,
  then asked authority to approve displacement. That left dash phase, cooldown, distance
  and direction outside canonical replay hashing, so recorded streams could not reproduce
  complete future-affecting dash state. Ability starts also lacked the same warmup/spawn-protection
  gate already applied to attacks.
- **Options considered:** Record per-tick displacement arrays in replays; keep presentation
  runtime and serialize every intermediate step; or move one immutable fighter runtime per
  actor into the authority, advance it during the canonical tick, hash it, and let replay record
  only the original command.
- **Decision:** Add authority-owned `FighterRuntimeState` runtimes for accepted Bijli commands.
  The authority validates ability identity, pressed state, stale/duplicate ticks, action phase and
  alive eligibility; advances each active/cooldown runtime once per fixed tick with deterministic
  arena collision; publishes immutable displacement results in `MatchAuthorityTick.BijliDashSteps`;
  includes dash state and ordering ticks in the canonical hash. Replay records only the common
  ability command. Production movement is suppressed from the same lock source while a dash/charge
  is active, and Unity receives collision-resolved positions as view instructions.
- **Consequences:** Bijli cooldown, phase progression, travelled distance, direction and collision
  outcome are now deterministic and replayable without per-tick displacement capture. The change also
  applies action-phase gating to Pehel charge starts. It does not yet unify gadget/healing/Aandhi
  eligibility, add durable replay-file serialization, or replace MovementLab fallbacks.
- **Evidence/sources:** `OfflineMatchAuthority.TryStartBijliDash`, `AdvanceBijliDash`,
  `GetBijliDashState`, `IsAuthorityMovementLocked`, `DeterministicReplayExecutor.ApplyFrame`,
  `AuthorityFoundationTests.AuthorityOwnsBijliDashEligibilityCollisionAndReplayState`,
  `ReplayDeterminismTests.ReplayExecutor_ReproducesCompleteAuthorityHashStream`, full EditMode/
  PlayMode baselines and the 2,000-match zero-divergence soak.
- **Owner:** Human project owner

### ADR-058 - Keep production fighter art in saved render-only prefab assets

- **Date:** 2026-08-26
- **Status:** Accepted for the V1 offline presentation baseline.
- **Context:** Fighter identity was previously assembled entirely from runtime primitive
  construction, which made the production scene difficult to inspect, version and review
  as an authored visual asset. Gameplay state must remain independent of visual identity.
- **Options considered:** Keep all silhouettes runtime-generated; place gameplay components
  on visual prefabs; or generate saved mesh/material/prefab assets and inject them only into
  the render layer.
- **Decision:** Use a controlled editor builder to create three saved fighter prefabs and
  their mesh/material assets. `FighterPresentation` selects the active fighter prefab and
  treats it as render-only; colliders, health, movement, input, ability controllers and
  authority remain on the actor root. A runtime silhouette fallback remains only for
  scenes/assets that have not yet been regenerated.
- **Consequences:** Production visuals are inspectable and asset-addressable, while the
  pure gameplay/domain layer remains unaware of Unity art assets. The baseline still lacks
  final rigs, authored animation, final gadget/arena assets, authored audio and human art
  review; generated meshes are not a claim of final commissioned art.
- **Evidence/sources:** `ProductionArtBuilder`, the three production prefabs, controlled
  scene-generation logs and `ProductionFighterArtUsesSavedRenderOnlyPrefabs`.
- **Owner:** Human project owner

### ADR-059 - Keep production gadget art in saved render-only prefab assets

- **Date:** 2026-08-26
- **Status:** Accepted for the V1 offline presentation baseline.
- **Context:** Gadget pickups still used runtime primitives for their identity, while the
  scene contract needed inspectable Umbrella, Dhol and Tiffin visuals without moving
  collision or collection authority into art assets.
- **Options considered:** Keep all gadget visuals runtime-generated; add colliders and
  gameplay state to gadget prefabs; or generate saved render-only prefabs and inject them
  under the existing `GadgetIdentityVisual` root.
- **Decision:** Generate three saved gadget prefabs and reusable mesh/material assets with
  the controlled editor builder. `GadgetPickupVisuals` selects the correct prefab from the
  authoritative gadget ID; the prefab hierarchy owns only MeshFilter/MeshRenderer parts.
  Controlled scene generation explicitly reconciles serialized prefab references after
  component creation so editor `Awake` ordering cannot save the primitive fallback.
- **Consequences:** Gadget identity is inspectable and asset-addressable while pickup
  availability, collection, collider and authority remain on the existing pickup actor.
  A runtime primitive fallback remains for unreconciled legacy scenes. The generated
  assets are a V1 baseline, not final commissioned art or cultural approval.
- **Evidence/sources:** `ProductionArtBuilder`, `GadgetPickupVisuals`, `BuildEntrypoints`,
  the three production gadget prefabs, Bazaar scene serialized references and
  `ProductionGadgetArtUsesSavedRenderOnlyPrefabs`.
- **Owner:** Human project owner

### ADR-060 - Prefer owned reproducible WAV sources with a mixer-backed runtime fallback

- **Date:** 2026-08-26
- **Status:** Accepted for the V1 audio baseline; final mix review remains open.
- **Context:** The vertical slice used runtime sine tones and an in-memory loop. That was
  useful for smoke tests but did not provide inspectable source audio, fighter/gadget
  identity or a mixer-backed release asset boundary.
- **Options considered:** Keep runtime tones as the shipped audio; add third-party packs;
  or generate owned WAV sources with reproducible synthesis settings and retain a small
  runtime fallback for missing imports.
- **Decision:** `ProductionAudioBuilder` emits original PCM WAV sources and a
  `BattleRajaV1.mixer` with Music, Ambience, UI, Combat, Abilities, Gadgets and Zone buses.
  `BattleRajaAudioDirector` loads those Resources assets first, routes music/effects when
  the mixer exists, and only creates temporary fallback clips when a source is missing.
  Fighter and gadget presentation events choose their identity-specific clips.
- **Consequences:** Audio is now asset-addressable, inspectable and provenance-traceable
  without external licences or network dependencies. Human loudness, clipping, voice-limit,
  thermal and cultural review are still required; generated synthesis is not claimed to be
  final commissioned music.
- **Evidence/sources:** `ProductionAudioBuilder`, `BattleRajaAudioDirector`,
  `Resources/Audio/V1`, `AUDIO_BIBLE.md` and `ProductionAudioUsesOwnedSourcesAndMixerGroups`.
- **Owner:** Human project owner

### ADR-061 - Quantize continuous bot inputs in the production determinism diagnostic

- **Date:** 2026-08-26
- **Status:** Accepted for V1 harness evidence.
- **Context:** The production harness command digest included raw movement/aim floats.
  Two real-time runs had identical command counts, decisions, outcomes and duration, but
  one Pehel digest differed due to harmless presentation transform precision. Accelerated
  playback also changes frame scheduling and is not suitable as a replay-equivalence clock.
- **Options considered:** Treat every float-bit difference as gameplay divergence; remove
  the command digest; or retain a stable semantic diagnostic by quantizing continuous inputs
  while preserving discrete tick/attack/ability fields exactly.
- **Decision:** `BotBrain` hashes movement and aim components after `Mathf.RoundToInt(value
  * 100f)` (centimetre-scale diagnostic precision), while simulation tick, attack and ability
  bits remain exact. The harness exposes a test-only playback-scale environment override;
  release-batch default remains 50x, and same-seed evidence uses the deterministic 1x path.
- **Consequences:** Fresh Unity processes now reproduce the same aggregate digest at 1x
  (two 79/79 runs, 269.02 s, 38,460 commands, digest `BB23BE3A400CA3E6`). This is a
  diagnostic tolerance, not a change to authority commands or gameplay rules. The 50x
  shortcut remains unsuitable for determinism claims because it intentionally stresses
  variable frame scheduling.
- **Evidence/sources:** `BotBrain`, `ProductionBotHarnessPlayModeTests`, P10 in
  `V1_RELEASE_PLAN.md`, and the paired 2026-08-26 scale-1 reports.
- **Owner:** Human project owner

### ADR-062 - Generate a presentation-only transform rig, Animator and particle cue layer

- **Date:** 2026-08-26
- **Status:** Accepted for the V1 offline presentation baseline; final authored art review
  remains open.
- **Context:** Saved fighter prefabs had reusable meshes and materials but no inspectable rig,
  animation controller or authored VFX assets. Runtime primitive fallback made it possible for
  regenerated prefab root IDs to leave scenes displaying prototype geometry.
- **Options considered:** Keep code-driven presentation only; add gameplay components to
  visual prefabs; or generate a lightweight render-only rig/controller/VFX layer and refresh
  scene references through Unity serialization.
- **Decision:** `ProductionPresentationBuilder` generates named transform joints, nine
  editable Animator clips, one shared controller and bounded particle prefabs. `ProductionVfxCue`
  is triggered only by existing presentation notifications. `FighterPresentation` continues
  to own visual state while authority, collision and timing stay in the gameplay layer. A
  controlled scene-reference pass rewrites serialized prefab fields after regeneration.
- **Consequences:** The production baseline is asset-addressable and testable without moving
  gameplay authority into art. The generated transform rig is not a claim of commissioned
  skinned art, final VFX direction or cultural approval; mobile readability and human review
  remain open.
- **Evidence/sources:** `ProductionPresentationBuilder`, `ProductionVfxCue`, generated
  `FighterProduction.controller`, nine clips, 14 VFX prefabs, three refreshed scenes and
  `ProductionFighterArtUsesSavedRigAnimatorAndVfxCues`.
- **Owner:** Human project owner

### ADR-063 - Guard optional mixer exposure metadata for offline volume settings

- **Date:** 2026-08-26
- **Status:** Accepted for the V1 audio baseline; final loudness review remains open.
- **Context:** The generated mixer contains named buses, but Unity's editor-only exposed
  parameter metadata is not stable across generated assets and can make direct runtime
  `SetFloat` calls emit warnings.
- **Decision:** `BattleRajaAudioDirector` always applies the persisted source-volume controls;
  it does not probe absent editor-only mixer parameter names at runtime. `ProductionAudioBuilder`
  clears invalid generated exposure metadata, while the named Music and Combat buses remain
  asset-addressable. The PlayMode audio test verifies those buses and owned clips without relying
  on fragile editor-only exposure state.
- **Consequences:** Offline settings remain warning-free and robust across Unity-authored mixer
  variants, with source-volume fallback available even when no compatible exposed parameters
  exist. Final device loudness, clipping and voice-limit review remains human work.
- **Evidence/sources:** `ProductionAudioBuilder`, `BattleRajaV1.mixer`,
  `BattleRajaAudioDirector` and `ProductionAudioUsesOwnedSourcesAndMixerGroups`.
- **Owner:** Human project owner

### ADR-064 - Never animate a legacy root renderer that owns authoritative movement

- **Date:** 2026-08-27
- **Status:** Accepted for the V1 offline presentation boundary.
- **Context:** The legacy Bazaar scene fixture can place a placeholder `MeshRenderer` on
  the same GameObject as `CharacterController` and `MovementPlayerAgent`. Per-frame visual
  bobbing on that renderer therefore rewrote the authoritative movement root and made a
  valid touch swipe appear to plateau.
- **Options considered:** Keep animating every discovered renderer; require a scene-wide
  prefab migration before continuing; or animate only child visual renderers while leaving
  the actor root transform under movement authority.
- **Decision:** `FighterPresentation` animates `bodyRenderer` only when it is a child of the
  actor root. Root renderers remain visually static, while the generated `_silhouetteRoot`
  and future production visual children carry presentation motion. Movement, collision and
  authority never depend on a presentation transform write.
- **Consequences:** Legacy scenes remain compatible without allowing presentation to mutate
  movement. New prefabs should keep all animated visuals below the movement root. The exact
  release candidate now attributes a real Lava movement swipe to the tutorial unlock; final
  authored visual migration and human feel review remain open.
- **Evidence/sources:** `FighterPresentation`, the Bijli open-lane regression fixture,
  full EditMode/PlayMode results and exact-source Lava evidence in `V1_RELEASE_PLAN.md` P30.
- **Owner:** Human project owner

### ADR-065 - Persist ordered production replay captures as checksummed core files

- **Date:** 2026-08-27
- **Status:** Accepted for the V1 offline diagnostic/replay foundation.
- **Context:** Deterministic replay execution and hash soaks existed only as in-memory
  EditMode coverage. Production bot runs could not retain the exact authority inputs or
  inspect canonical state after a match, leaving a durable replay-file gate open.
- **Options considered:** Serialize Unity presentation objects; record only a summary digest;
  or persist the transport-independent command stream and canonical snapshots at match end.
- **Decision:** Add a Unity-independent version-1 binary replay envelope with explicit magic,
  payload length and SHA-256 checksum. Production/development harness capture records ordered
  movement, attack, ability, gadget and Pehel charge-step submissions, the complete replay
  header/content configuration, per-tick participant snapshots and canonical hashes. The
  serializer supports deterministic byte-for-byte round trips and rejects truncation,
  trailing data and checksum corruption. Capture remains diagnostic-only; cosmetic Unity
  animation, audio and VFX are not gameplay authority state and are not treated as replay
  inputs.
- **Consequences:** A production match can now emit a durable `.brr` artifact that can be
  re-read and fully re-executed against the offline authority, including same-tick action-lock
  ordering. The format is not a network protocol, player save format or cross-machine
  floating-point proof; signed release and human presentation review remain separate gates.
- **Evidence/sources:** `MatchReplayFileSerializer`, `MatchReplayFrame.CommandOrder`,
  `ProductionBotMatchHarness`, `OfflineMatchController` diagnostic capture taps,
  `ReplayDeterminismTests.MatchReplayFileSerializer_RoundTripsDeterministicallyAndRejectsCorruption`,
  and P42 in `Docs/V1_RELEASE_PLAN.md`.
- **Owner:** Human project owner

### ADR-066 - Replace primitive fighter pieces with authored faceted silhouette profiles

- **Date:** 2026-08-28
- **Status:** Accepted for the V1 offline presentation baseline; final authored art,
  cultural review and human feel approval remain open.
- **Context:** The saved fighter prefabs were already render-only and asset-addressable,
  but their body, head, shoulder and accessory recipes still read as simple box/lathe
  construction at phone scale. That weakened the distinct Bijli, Pehel and Maya
  silhouettes even though the gameplay identity boundary was correct.
- **Options considered:** Keep the existing primitive-like baseline; move visual detail
  into runtime shaders or procedural scene code; or add saved low-poly loft/extrusion
  profiles with an explicit visual rebuild path while retaining the production prefab and
  rig boundaries.
- **Decision:** Add reproducible faceted loft and extruded-polygon mesh recipes to
  `ProductionArtBuilder`. Bijli, Pehel and Maya now use distinct torso/cloak, visor,
  shoulder, arm, boot, sash, mask, scarf and badge profiles saved under
  `Content/Art/V1/Meshes`. `Rebuild V1 Production Fighter Art` is an explicit editor
  action; ordinary scene/build generation keeps committed asset identities stable.
  `ProductionPresentationBuilder` reparents the new parts into the existing
  presentation-only rig and preserves the existing Animator/VFX asset boundary. No
  collider, authority, simulation, input, or network code is attached to the art.
- **Consequences:** The three fighter silhouettes are now inspectable, mesh-backed and
  regression-tested as distinct profiles with at least 260 combined vertices per
  production presentation instance. The saved meshes remain a locally generated V1
  baseline rather than commissioned final art; mobile frame time, final animation/VFX,
  cultural safety and human visual approval remain separate gates.
- **Evidence/sources:** `ProductionArtBuilder`, `ProductionPresentationBuilder`, the
  33-file art/test commit `816d9ac`, `VerticalSlicePlayModeTests.ProductionFighterArtUsesDistinctFacetedSilhouetteMeshes`,
  full 141/141 EditMode and 87/87 PlayMode results, and P43 in
  `Docs/V1_RELEASE_PLAN.md`.
- **Owner:** Human project owner

### ADR-067 - Keep generated UVs and a minimal primary skin inside the presentation boundary

- **Date:** 2026-08-29
- **Status:** Accepted for the V1 offline presentation baseline; final authored modeling,
  texturing and cultural review remain open.
- **Context:** The saved faceted fighter meshes were reusable and distinct, but they had no
  UV channels and every visible piece was a static MeshRenderer. That left the generated
  baseline unable to accept a future authored texture pass or demonstrate that the saved
  rig can deform a primary silhouette without moving gameplay authority.
- **Options considered:** Leave the procedural meshes unwrapped and static; add runtime
  texture/procedural deformation; or generate deterministic UVs and a small saved two-bone
  body/cloak skin while retaining static accessory parts and the render-only prefab boundary.
- **Decision:** `ProductionArtBuilder.CreateMesh` assigns deterministic planar/cylindrical
  UVs to every generated mesh. `ProductionPresentationBuilder` copies Bijli/Pehel `Body`
  and Maya `Cloak` into saved `SkinnedMeshRenderer` meshes with hips/chest bind poses and
  waist-blended weights. The source MeshFilter remains for reproducible rebuilds, its source
  renderer is disabled, and the derived renderer has no collider, input, health, damage,
  movement or action-success state. Accessories remain saved MeshFilter/MeshRenderer parts.
- **Consequences:** The baseline now has inspectable UV coverage and real saved skin data,
  while authority and collision remain unchanged. The generated weights and UVs are
  technical foundations, not commissioned sculpt/textures, final animation direction,
  mobile-performance approval or cultural sign-off.
- **Evidence/sources:** `ProductionArtBuilder`, `ProductionPresentationBuilder`, saved
  `BijliSkinBody`, `PehelSkinBody` and `MayaSkinCloak` meshes, the UV/skinning assertions in
  `VerticalSlicePlayModeTests`, full 141/141 EditMode and 87/87 PlayMode results, and P44
  in `Docs/V1_RELEASE_PLAN.md`.
- **Owner:** Human project owner

### ADR-068 - Save the Bazaar environment and keep runtime fallback geometry custom

- **Date:** 2026-08-29
- **Status:** Accepted for the V1 offline presentation baseline; final authored environment,
  cultural review and human feel approval remain open.
- **Context:** The production scene still depended on a large runtime-generated architecture
  branch and several presentation helpers constructed Unity primitives. That made the visual
  identity harder to inspect, increased scene/build churn and left a clear distinction between
  the saved fighter/gadget assets and the arena surface.
- **Options considered:** Keep the runtime architecture and primitive helpers; move the
  environment into one opaque scene-only object; or create a saved, textured render-only
  environment prefab and use a tiny shared custom-mesh library only for development/readability
  fallbacks.
- **Decision:** `ProductionEnvironmentBuilder` emits a deterministic saved
  `BazaarBastionProduction.prefab` with a three-submesh 32×32 ground mosaic, themed meshes,
  16 small texture/material pairs and a backdrop LOD group. `BuildEntrypoints` removes the
  legacy `BazaarArchitecture` instance and binds the saved prefab with runtime fallback
  disabled. `PresentationMeshFactory` replaces runtime visual primitive construction in
  feedback, projectile, gadget and decoy presentation paths; the Maya decoy adds its
  targetability capsule explicitly rather than obtaining it from a primitive helper.
- **Consequences:** The active production scene now has inspectable asset provenance, stable
  prefab references, textured UV-bearing environment geometry and an explicit low-detail path.
  The environment remains presentation-only; authored collision/navigation and all authority
  state stay in the existing scene/domain layers. Generated art is still a technical baseline,
  not final commissioned art, cultural approval or sustained mobile-performance approval.
- **Evidence/sources:** `ProductionEnvironmentBuilder`, `BazaarBastionVisuals`,
  `PresentationMeshFactory`, `ProductionPresentationBuilder.BuildLodGroup`, the environment
  and LOD assertions in `VerticalSlicePlayModeTests`, commit `ac45479`, and P45 in
  `Docs/V1_RELEASE_PLAN.md`.
- **Owner:** Human project owner

### ADR-069 - Keep fighter Aim animation render-only and input-intent driven

- **Date:** 2026-08-30
- **Status:** Accepted for the V1 offline presentation baseline; final authored animation,
  accessibility and human feel review remain open.
- **Context:** The saved fighter controller had action, ability, hit and locomotion states but
  no distinct visual response while the player held the aim control. Adding that response must
  not create a second input path or allow presentation to mutate authoritative combat.
- **Options considered:** Reuse locomotion/idle, add a physics-facing aim mode, or expose the
  existing player aim intent to a dedicated render-only state.
- **Decision:** `PlayerInputAdapter.IsAimHeld` reports the existing focus/virtual-stick/action
  intent, `FighterPresentation` selects `AnimationState.Aim` after higher-priority attack,
  ability, hit and movement checks, and `ProductionPresentationBuilder` saves a looping
  `FighterAim.anim` clip/controller state with a subtle torso/hand pose. The state does not
  own aim assist, projectile direction, cooldowns, damage, movement, physics or action
  success; those remain in the canonical input/authority path.
- **Consequences:** Players receive a clear aim silhouette cue and the saved controller is
  inspectable, while gameplay determinism and authority boundaries remain unchanged. The clip
  is generated baseline art, not final authored animation or proof of touch comfort.
- **Evidence/sources:** `PlayerInputAdapter`, `FighterPresentation`,
  `ProductionPresentationBuilder`, saved `FighterAim.anim`, full 141/141 EditMode and 87/87
  PlayMode results, and P46 in `Docs/V1_RELEASE_PLAN.md`.
- **Owner:** Human project owner

### ADR-070 - Drive persistent terminal outcome cues from authoritative placement

- **Date:** 2026-08-30
- **Status:** Accepted for the V1 offline presentation baseline; final authored VFX, cultural,
  accessibility and human feel review remain open.
- **Context:** Results were presented through the existing overlay, but fighter presentation
  could lose its terminal state or leave the result visually ambiguous. A clearer cue must not
  duplicate placement logic or let a particle system become gameplay authority.
- **Options considered:** Leave the result overlay as the only cue; infer Victory/Defeat from
  local health or eliminated flags; or pass the already-authoritative result placement into a
  render-only presentation adapter with saved VFX assets.
- **Decision:** `OfflineMatchController.PublishResults` calls `FighterPresentation.SetVictory`
  for each result. First place selects persistent `AnimationState.Victory` and the saved gold
  `VictoryVfx`; all other placements select persistent `AnimationState.Defeat` and the saved
  red `DefeatVfx`. `ProductionVfxCue` owns only cue playback counters and particle references;
  placement, health, elimination, rewards, cooldowns, timing and match completion remain in
  the domain/authority layer. The winner is not marked eliminated as a presentation shortcut.
- **Consequences:** The result state remains readable behind/after the overlay and the cue
  contract is inspectable in the saved prefabs. The generated colors, burst recipes and clips
  are technical baseline art, not final commissioned VFX, cultural direction or proof of touch
  comfort. Rematch reloads the scene, so the normal lifecycle resets the presentation state.
- **Evidence/sources:** `OfflineMatchController`, `FighterPresentation`, `ProductionVfxCue`,
  `ProductionPresentationBuilder`, saved `VictoryVfx.prefab`/`DefeatVfx.prefab`, the terminal
  outcome PlayMode regression, and P47 in `Docs/V1_RELEASE_PLAN.md`.
- **Owner:** Human project owner

### ADR-071 - Suppress legacy fighter meshes when saved identity art is active

- **Date:** 2026-08-30
- **Status:** Accepted for the V1 offline presentation baseline; final authored art,
  accessibility and human feel review remain open.
- **Context:** The production scenes retain a capsule `MeshRenderer` on the same actor
  GameObject as the authoritative `CharacterController`. `FighterPresentation` correctly
  instantiated the saved faceted identity prefab, but left that legacy mesh enabled, so the
  capsule visually overlaid the production silhouette on the Android portrait camera.
- **Options considered:** Migrate every scene fixture immediately; remove the fallback
  mesh and risk old fixtures becoming invisible; or keep the fallback for scenes without a
  saved model and suppress only direct root mesh renderers after a valid saved identity is
  instantiated.
- **Decision:** `FighterPresentation` keeps the root mesh as a deterministic emergency
  fallback, but disables only direct root `MeshRenderer` components when a saved production
  prefab has mesh/skinned renderers. Hit and elimination tinting now uses the saved renderers
  through `MaterialPropertyBlock`; the root `TrailRenderer` remains available for Bijli's
  dash telegraph. No movement, collision, authority, input, timing, or reward state is
  changed.
- **Consequences:** The saved Bijli, Pehel and Maya silhouettes are the actual gameplay
  presentation surface on Android, while legacy fixtures remain recoverable when no saved
  identity is available. The generated meshes and tint path remain a technical V1 baseline,
  not commissioned final art, animation, cultural approval or sustained mobile-performance
  approval.
- **Evidence/sources:** `FighterPresentation`, `ProductionFighterArtUsesSavedRenderOnlyPrefabs`,
  fresh 141/141 EditMode and 89/89 PlayMode results, the rebuilt technical gate, and the
  approved-Lava captures indexed in `Docs/V1_RELEASE_PLAN.md` P54.
- **Owner:** Human project owner

### ADR-072 - Place the tutorial elimination target in a readable open lane

- **Date:** 2026-08-30
- **Status:** Accepted for the V1 offline presentation baseline; action-by-action comfort,
  accessibility and human-fun review remain open.
- **Context:** On the approved Lava route, the tutorial elimination target spawned in a
  diagonal corner position that was visually reachable but difficult to acquire with the
  portrait touch controls. This blocked the intended elimination lesson even though the
  authority projectile path was valid.
- **Options considered:** Keep the diagonal spawn and rely on repeated aim gestures; alter
  authority collision or aim rules; or move only the tutorial target to a stationary open
  lane while preserving production spawns and rules.
- **Decision:** Keep actor 11's tutorial-only transform at `(0, 1, -3.2)` in the open south
  lane. `BuildEntrypoints.ConfigureTutorialEliminationTarget` applies the same placement when
  generating or repairing the saved TutorialArena. Production spawns, MovementLab fixtures,
  collision, damage, timing and offline authority remain unchanged.
- **Consequences:** A short forward touch movement now lines the player up with the target,
  and the refreshed exact-candidate Lava route unlocked ELIMINATION, reached terminal
  RESULTS / WINNER YOU / #1, and completed TUTORIAL 8/8. The route remains evidence for a
  release candidate, not final comfort or fun approval.
- **Evidence/sources:** `BuildEntrypoints`, saved `TutorialArena.unity`,
  `TutorialArenaPlayModeTests`, focused 91/91 PlayMode results, and the P57 route captures
  indexed in `Docs/V1_RELEASE_PLAN.md`.
- **Owner:** Human project owner

### ADR-073 - Make the completed tutorial card dismissible without mutating match state

- **Date:** 2026-08-31
- **Status:** Accepted for the V1 offline presentation baseline; action-by-action comfort,
  accessibility and human-fun review remain open.
- **Context:** The tutorial completion card remained over the live Results surface after the
  walkthrough ended, so the player could not inspect placement or choose the underlying
  REMATCH control without replaying the tutorial. The fix must not bypass authoritative
  results or create a second rematch path.
- **Options considered:** Keep the completion card permanently visible; route completion
  directly into a new match; or expose a secondary dismissal action while preserving the
  existing replay and menu actions.
- **Decision:** Keep the completion card as the primary terminal acknowledgement, change its
  secondary action to `CLOSE CARD`, and have `DismissCompletionCard` hide only the tutorial
  panel once `_steps.IsComplete` is true. Incomplete cards remain guarded and refresh instead
  of dismissing. The existing `OfflineMatchHud` remains the sole owner of Results, REMATCH,
  MENU, timing and match state.
- **Consequences:** Players can inspect the exact Results/REMATCH surface after tutorial
  completion, and the behavior is covered by a 92/92 PlayMode run plus a real-touch Lava
  capture on the exact rebuilt candidate. Rematch from TutorialArena still reloads the
  tutorial scene; this route observation is not a claim of repeated-rematch comfort. The
  generated presentation baseline and all human review gates remain open.
- **Evidence/sources:** `TutorialOverlay`,
  `TutorialArenaPlayModeTests.CompletedTutorialCanDismissOverlayForResultsAndRematch`,
  exact candidate P58 artifacts and Lava route in `Docs/V1_RELEASE_PLAN.md`, and
  `tutorial-dismiss-route-manifest.json`.
- **Owner:** Human project owner

### ADR-074 - Keep compact zone telemetry player-facing on portrait HUDs

- **Date:** 2026-08-31
- **Status:** Accepted for the V1 offline presentation baseline; final mobile readability
  and localization review remain open.
- **Context:** The portrait/compact match HUD abbreviated the zone label to `Z`, which was
  ambiguous beside the alive count and read like internal debug telemetry even though the
  radii are useful player information.
- **Options considered:** Keep the one-letter abbreviation; remove zone radii entirely; or
  retain the same two-line telemetry while spelling out `ZONE`.
- **Decision:** Use `ZONE {current} > {next}` in both wide and compact player-facing match
  status formats. Keep the existing phase, alive count, warning and closing labels, and do
  not move any authoritative state into the presentation layer.
- **Consequences:** Portrait screenshots now show `ALIVE 8  ZONE 14.0 > 14.0`, removing
  the internal-looking abbreviation without adding a new layout or runtime allocation.
  The copy remains short enough for the tested 1080x2460 Lava viewport; smaller devices,
  localization and final visual approval remain open.
- **Evidence/sources:** `OfflineMatchHud.FormatMatchStatus`,
  `VerticalSlicePlayModeTests.CompactMatchStatusKeepsZoneTelemetryReadable`, the full
  141/141 EditMode and 92/92 PlayMode reruns, and the P60 approved-Lava capture in
  `Docs/V1_RELEASE_PLAN.md`.
- **Owner:** Human project owner

### ADR-075 - Spell out compact results metrics on portrait HUDs

- **Date:** 2026-08-31
- **Status:** Accepted for the V1 offline presentation baseline; final mobile readability
  and localization review remain open.
- **Context:** The compact results formatter shortened eliminations, assists and damage to
  `K1 A1 D163`. That saved a few characters but read like internal telemetry on the
  1080x2460 approved-Lava capture and was harder to scan at 16 px type.
- **Options considered:** Retain single-letter labels; fully expand every metric; or keep the
  compact one-line-per-placement structure while using the same short player-facing labels as
  the wide card.
- **Decision:** Use `KOs`, `AST` and `DMG` labels in compact results and raise the compact
  result text from 16 px to 18 px before the user's text-scale preference is applied. This is
  presentation-only; placement, damage, rewards and authority state are unchanged.
- **Consequences:** The result card is more scannable without adding another panel or risking
  long `ELIMINATIONS`/`ASSISTS`/`DAMAGE` wrapping on the tested portrait width. Smaller devices,
  localization and final visual approval remain open.
- **Evidence/sources:** `OfflineMatchHud.FormatResults`, the compact result regression in
  `VerticalSlicePlayModeTests.ResultsFormatterListsPlacementsAndCombatStats`, and the exact
  P60 Lava result capture under `Builds/Local/Device/final-circle-20260830/p60-full-route`.
- **Owner:** Human project owner
