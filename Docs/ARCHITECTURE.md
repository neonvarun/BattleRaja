# Architecture

**Status:** Implemented and accepted for Milestones 0–6. Changes beyond these boundaries require a new decision record.

## Goals

- Offline-first testable simulation
- Unity presentation separated from domain rules
- Common command model for humans and bots
- External SDKs behind adapters
- Android and Web performance awareness
- Platform services isolated behind Android/Web adapters

## Proposed layers

1. Domain / simulation
2. Application orchestration
3. Unity presentation
4. Infrastructure adapters

## Dependency rule

Dependencies point inward. The domain layer must not reference Photon, PlayFab, Unity UI, Animator, VFX, production scenes or platform SDKs.

## Assembly boundaries

- `Assets/BattleRaja/Core/BattleRaja.Core.Domain.asmdef`: pure C# (`noEngineReferences`) for values, commands, seeded randomness and fixed-step contracts.
- `Assets/BattleRaja/Core/Application/BattleRaja.Core.Application.asmdef`: pure orchestration and ports; references Domain only (`noEngineReferences`).
- `Assets/BattleRaja/Gameplay/BattleRaja.Gameplay.asmdef`: feature composition; references Domain/Application and remains Unity-independent in M0.
- `Assets/BattleRaja/Presentation/BattleRaja.Presentation.asmdef`: Unity views and MonoBehaviours; references Domain/Application/Gameplay.
- `Assets/BattleRaja/Infrastructure/BattleRaja.Infrastructure.asmdef`: platform, persistence, analytics and future networking adapters; references Domain/Application.
- `Assets/BattleRaja/Infrastructure/Platform/Android/BattleRaja.Infrastructure.Android.asmdef` and `.../Web/BattleRaja.Infrastructure.Web.asmdef`: platform-specific adapters selected by `Android` and `WebGL` include-platform filters.
- `Assets/BattleRaja/Editor/BattleRaja.Editor.asmdef`: editor-only validation and build entrypoints.
- `Assets/BattleRaja/Tests/EditMode/BattleRaja.Tests.EditMode.asmdef` and `.../PlayMode/BattleRaja.Tests.PlayMode.asmdef`: pure and lifecycle tests.

Human and bot inputs use the same immutable gameplay-command model. Runtime state is separate from ScriptableObject configuration. Simulation stepping is fixed-step and independent from rendering.

## Deferred decisions

- Selecting the exact simulation clock/event interfaces
- Save/network adapter boundaries for later milestones
- Content validation implementation after Unity project creation

The M0 assembly names, inward dependency direction and `noEngineReferences` rules are accepted. Empty feature/infrastructure boundary assemblies are intentional until their first milestone-specific implementation.

## M1 movement implementation

- `BattleRaja.Core.Domain` owns `Float2`, `MovementTuning`, `MovementInputFrame`, `MovementCommand`, `MovementCommandFactory`, `MovementMotor` and `MovementStep`. These are Unity-independent and are testable without a scene.
- `BattleRaja.Core.Application` exposes `IMovementCommandSink`; `MovementPlayerAgent` implements it so future bot/network producers can submit the same command type.
- `BattleRaja.Presentation.Movement.PlayerInputAdapter` translates Input System actions and virtual-stick values into domain input frames. It never writes a Transform.
- `BattleRaja.Presentation.Movement.MovementPlayerAgent` owns the Unity `CharacterController` bridge and applies domain displacement/rotation. Runtime velocity and aim state are not stored in the tuning asset.
- `TopDownCameraController`, `AimDirectionIndicator`, `VirtualStick`, `SafeAreaPanel` and `InputFocusController` are presentation/lifecycle components only.
- `Assets/BattleRaja/Editor/BuildEntrypoints.cs` creates the MovementLab scene and serialized assets through controlled editor automation; scene YAML is not hand-authored.

No external networking/backend SDK or combat authority was added. The command and agent boundary is deliberately compatible with later bot and network command producers.

## M2 combat implementation

- `BattleRaja.Core.Domain` owns `CombatEntityId`, factions, damage requests/results,
  `HealthState`, `DamagePipeline`, projectile weapon definitions, cooldown state,
  attack commands, projectile travel and duplicate-hit tracking. These types do not
  reference UnityEngine and are usable by future bots and network authority.
- `BattleRaja.Presentation.Combat.ProjectileWeaponAsset` stores attack configuration;
  `CombatAttackController` converts shared player input into `AttackCommand` values
  and submits them to `CombatProjectilePool`.
- `CombatDamageResolver` is the only presentation entry point that applies a damage
  request to `CombatHealth`; `CombatHealth` delegates mutation to the pure
  `DamagePipeline` and raises result notifications for feedback.
- `CombatProjectile` uses an explicit layer mask, speed, radius, range, lifetime,
  despawn reason, faction policy and per-projectile `ProjectileHitTracker`. It never
  mutates target health directly and returns to the bounded pool after hit, collision,
  range or lifetime expiry.
- `TrainingDummy`, `CombatHitFlash`, `CombatImpactFeedbackPool` and `AttackButton`
  are presentation feedback/input components. They do not add named fighter, bot,
  gadget, match, networking, backend or progression concepts.

## Platform boundary

Android and Web share domain, application and most presentation code. Platform-specific identity, storage, haptics, fullscreen, deep links, purchases, browser lifecycle and hosting integration belong in infrastructure adapters.

## M3 fighter implementation

- `BattleRaja.Core.Domain` owns stable `ContentId` values, immutable `FighterDefinition`
  and `DashAbilityDefinition` data, `AbilityCommand`, and `FighterRuntimeState`.
  Dash startup/active/recovery/cooldown transitions, direction fallback and bounded
  displacement are pure C# rules.
- `FighterDefinitionAsset` composes immutable content references for movement and the
  Bijli electric bolt; runtime state is instantiated per fighter and never written back
  into the asset.
- `BijliFighterController` is the Unity bridge. It converts the shared input adapter
  into an `AbilityCommand`, performs physics/bounds availability checks, applies the
  pure dash displacement through `CharacterController`, and exposes presentation state.
- `CombatAttackController` consumes the fighter definition's basic-attack data through
  the same `IAttackCommandSink` used by M2. `BijliHud`, `AbilityButton` and the
  `TrailRenderer` are presentation-only feedback and input surfaces.
- No passive, bots, gadgets, match state, networking or backend concepts were added.

## M4 bot implementation

- `BattleRaja.Core.Domain.BotAI` owns immutable difficulty profiles, observation/value
  contracts, seeded randomness, target scoring, imperfect aim, reaction delay and
  navigation-stuck recovery. It consumes only perceived targets and emits the same
  movement, attack and ability command values used by the player.
- `BotPerceptionSensor` is a Unity adapter. It caches the current combat-target set,
  applies an explicit world-only line-of-sight mask, and publishes a bounded snapshot;
  hidden actors are not exposed to the decision engine.
- `BotBrain` is a presentation/application bridge that runs decisions at a tunable
  interval, continuously submits the current movement command, respects existing
  attack/ability cooldowns and records debug decision cost. `BotDebugOverlay` is an
  optional non-authoritative overlay.
- M4 uses seven Bijli bot actors in MovementLab for stress testing. No loot, Aandhi,
  match resolution, gadget, network or backend authority was added.

## M5 offline match implementation

- `OfflineMatchSimulation` owns the local authoritative phase, Aandhi radius/damage
  data, spawn validation, participant health snapshots, idempotent elimination,
  placement and winner state. It has no Unity or presentation dependency.
- `OfflineMatchController` bridges scene actors into the simulation, applies Aandhi
  damage through `CombatDamageResolver`, disables player input on elimination and
  retargets the camera through the spectator selector. `OfflineMatchHud` is a
  presentation-only phase/zone/results overlay.
- `MatchPickup` is a small neutral health resource with respawn timing. It does not
  own combat authority or progression rewards.
- The first offline lab uses the existing eight Bijli actors and grey-box arena. The
  simulation definition targets a 298-second match; accelerated deterministic tests
  cover repeated completion and restart without requiring five minutes of wall time.

## M6 gadget implementation

- `BattleRaja.Core.Domain.Gadgets` owns stable Gadget IDs, immutable definitions,
  catalog lookup, one-slot inventory, use/cooldown validation, effect contracts and
  deterministic spawn eligibility. It remains Unity/Photon/PlayFab independent.
- `GadgetDefinitionAsset` is the serialized content bridge for versionable M6 balance
  data. `GadgetUser`, `GadgetPickup`, `GadgetStation` and `GadgetHud` are presentation
  adapters; they route damage/healing through the existing combat health/resolver paths.
- `OfflineMatchAuthority` owns per-participant gadget inventory, pickup collection and
  cooldown/use validation. `GadgetUser` submits use intents and consumes the immutable effect
  result for presentation; it does not decide whether a public-match gadget use is valid.
- Umbrella Guard stores facing and duration state on the user; projectile hit direction
  is part of `DamageRequest` so directional mitigation is explicit. Dhol Burst uses
  CharacterController displacement and does not add a second movement authority.
- Tiffin Station owns finite lifetime and targetable `CombatHealth`; its scan is bounded
  to the small offline lab and is a known scale-up task.
- `BotBrain` calls the same `GadgetUser` use path after contextual perception; there is
  no random or bot-only effect path. No networking/backend dependency entered the core.

## M7 vertical-slice fighter implementation

- `FighterDefinition.Pehel` and `.Maya` provide stable IDs and distinct data-driven
  health, movement and attack baselines. `FighterSpecialDefinition`, `ChargeThrowRuntime`
  and `DecoyRuntime` keep capture/throw, targetability, cooldown and expiry rules testable
  without Unity.
- M7 serialized fighter/weapon assets are loaded by the editor scene generator after
  scene creation so Unity asset lifecycle cannot silently fall back to Bijli. Actors
  still share `MovementPlayerAgent`, `CombatAttackController` and the common
  `IFighterAbilityController`/`IFighterMovementLock` command boundary, while the editor
  generator selects `BijliFighterController`, `PehelFighterController` or
  `MayaFighterController` from the data definition.
- Pehel's adapter performs fixed-tick charge/capture/throw collision and central damage;
  Maya's adapter spawns a targetable, health-bounded decoy with copied movement and expiry.
  `BuildEntrypoints.CreateBazaarBastionScene` regenerates a controlled production-scene
  copy from the MovementLab fixture, applies the Bazaar palette/architecture and selects
  the fighter-specific adapters without mutating the lab fixture. Bespoke VFX/audio,
  final UI, and full offline/tutorial progression remain explicit alpha debt.

## M8 networking boundary

- `BattleRaja.Infrastructure.Networking` owns transport-facing contracts only:
  `NetworkSessionConfig`, `NetworkInputFrame`, `NetworkActorSnapshot`, diagnostics and
  `INetworkSessionAdapter`. `BattleRaja.Core.Domain` and `BattleRaja.Core.Application`
  remain free of Photon/Fusion references.
- `NetworkSessionMock` is a deterministic correctness harness for two clients, protocol
  versions, room capacity, input delivery, authoritative damage/elimination and seeded
  packet loss. It is not a production server and must not be presented as online evidence.
- `PhotonFusionAdapter` is an explicit compile-safe seam that reports credential blockage
  until the approved Fusion package/App ID/configuration is supplied. A browser client is
  never treated as trusted public-match authority; client inputs remain intents and the
  server/host must own snapshots, damage, cooldowns and results in the real integration.
- Real prediction, reconciliation, interpolation, reconnect and latency/jitter/loss
  behavior require the external Fusion gate and two-client Android/Web validation.

## M9 online-alpha preparation boundary

- `AuthoritativeMatchServer` is a transport-independent preparation seam around the pure
  `OfflineMatchSimulation`. It owns eight-slot configuration, separated spawns, bot
  backfill, bounded movement input, authoritative health/elimination calls and disconnect
  grace/reconnect/bot-takeover state.
- This class is not a deployed headless server and does not provide matchmaking, a room
  service or public authority until Photon Fusion and deployment approvals exist. It must
  remain behind Infrastructure; no client/UI or browser code may become authoritative.
- The M9 completion gate remains blocked by the M8 real-session precondition. Do not claim
  cross-play, stress, reconnect or browser lifecycle behavior from the local preparation
  tests.

## M10 backend/progression boundary

- `IProgressionBackend` and the fake implementation live in Infrastructure; Domain remains
  free of PlayFab SDK types and client credentials. Profiles, cosmetic ownership, currencies,
  XP and leaderboard entries are snapshots, not mutable shared assets.
- Reward writes require server-validated evidence and an idempotency key. Client-facing code
  must not set currency, ownership, statistics or match results directly. `PlayFabBackendAdapter`
  stays unavailable until a server-controlled title configuration exists.
- The M10 partial gate is intentionally useful without a service: fake identity/link,
  conflict, cache/retry and reward tests establish contracts, but no cross-device persistence
  or account recovery claim is made.

## M11 release-candidate boundary

- `ReleaseProof.cs` contains only bounded development analytics, an unavailable crash adapter
  and a release-candidate safety guard. It does not collect identity/PII, enable admin tools,
  or add a third-party reporting SDK.
- Store copy, privacy/data-safety worksheet, closed-test instructions and rollback guidance
  are documentation drafts. They are not legal declarations, store submissions or public
  deployment automation.

## M12 presentation foundation

- `FighterPresentation` is a replaceable Unity presentation adapter. It creates gameplay
  colour rings, health/status bars, attack/ability telegraphs and code-driven idle,
  locomotion, attack, ability, hit and elimination states without storing gameplay rules.
  Hit timing remains driven by combat results, not animation events.
- `BattleRajaAudioDirector` owns original procedural placeholder cues, effects/music volume
  controls, optional `AudioMixer`/group references and user-gesture-gated startup for Web.
  It is scene-owned and does not introduce a global singleton or external audio dependency.
- Final imported animation clips, authored VFX, licensed/original production audio and
  visual approval remain human-review gates. The current presentation is an explicit
  replaceable stylised greybox.

## M13 Canvas match UI foundation

- `OfflineMatchHud` now builds a `CanvasScaler`-backed, anchored HUD at runtime rather
  than using immediate-mode GUI. It exposes match/zone status, spectator cycling,
  pause/settings, rematch and a results panel without moving authoritative state into
  the UI layer.
- The settings surface includes left-handed stick swapping, reduced-flash propagation
  to `FighterPresentation`, high-contrast status text and a Web-safe audio user-gesture
  entry point. Labels remain short and replaceable by localization keys; the current
  `AIM ASSIST (READY)` control is intentionally a placeholder, not a gameplay claim.
- The layout uses normalized anchors and a scale-with-screen-size canvas so Android
  safe-area/device review and Web responsive review remain explicit follow-up gates.
  Main-menu/bootstrap flow, complete offline/tutorial progression, localization assets,
  controller rebinding and final authored UI remain outside this foundation slice.

## M14 production flow and selected-fighter boundary

- `BattleRaja.Core.Application.ProductionFlowMachine` is a Unity-independent state machine
  for Bootstrap, MainMenu, ModeSelection, FighterSelection, MatchLoading, Gameplay,
  Paused, Settings, Spectator, Results and Error. It carries local mode/fighter intent and
  explicit error codes; it does not own match outcomes, credentials or network authority.
- `BattleRaja.Presentation.Flow.ProductionFlowController` creates the Bootstrap Canvas,
  safe-area root and EventSystem, renders the menu/mode/fighter/loading/settings/error
  surfaces, persists only local presentation preferences, and loads the production scene
  asynchronously. The online path is deliberately an honest unavailable state until the
  approved Fusion account/session gate exists.
- `PlayerFighterSelection` is the explicit scene-boundary adapter for actor 1. It selects
  one of the existing first-party fighter ability controllers, refreshes the shared movement
  lock and attack definition seams, and never writes authoritative match state. Bots remain
  statically configured by the production-scene generator. `OfflineMatchHud` adds pause/
  menu cleanup and local settings persistence without moving simulation rules into UI.
- Build entrypoints register `Bootstrap.unity` before `BazaarBastion.unity` for production
  Android/Web builds. `MovementLab` remains a regression fixture. The flow is proven by pure,
  EditMode and PlayMode tests plus a 1280×720 Web smoke; responsive multi-viewport visual QA,
  tutorial replay and final authored presentation remain explicit gates.

## M15 replayable tutorial boundary

- `TutorialStepMachine` remains Unity-independent and owns only ordered, idempotent prompt
  progression from movement through victory. It does not infer success, mutate match state or
  replace human/bot commands.
- `TutorialOverlay` is a scene-owned presentation adapter. It renders concise control, combat,
  gadget and Aandhi guidance over the real `OfflineMatchController`, Canvas HUD and touch/mouse
  controls. Replay/skip/completion persistence is local presentation state only.
- `TutorialArena.unity` is generated from the tested MovementLab fixture. Bot decision components
  are disabled while their actor GameObjects remain active, preserving eight valid authority
  spawns for the real offline simulation. The arena is registered after Bootstrap and before
  Bazaar Bastion in Android/Web build entrypoints.
- Tutorial prompts are guidance, not automated competency or balance evidence. Full-length
  offline reliability, memory/soak testing, responsive visual QA, final authored presentation,
  real Photon and real PlayFab remain separate gates.
