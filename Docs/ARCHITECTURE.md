# Architecture

**Status:** Implemented and accepted for Milestones 0–3. Changes beyond these boundaries require a new decision record.

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
