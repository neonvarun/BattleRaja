# Unity Project Context

## Project summary

BattleRaja is an original stylised top-down 3D micro battle royale for Android and
desktop-browser Web. The active product slice is an offline eight-actor Solo Raja
match with three selectable fighters, three gadgets, Aandhi zone pressure,
spectator/results/rematch flow and a replayable tutorial.

The repository is deliberately offline-first. Domain rules are pure C# and can run
without Unity, scenes, Photon or PlayFab. Unity owns presentation, physics queries,
scene lifecycle, input adapters and platform builds. External services remain behind
compile-safe infrastructure seams until explicitly approved.

## Confirmed environment

- Repository root: `C:\Projects\BattleRaja`
- Editor: Unity `6000.5.6f1`, revision `0e0577a1a2ac`
- Render pipeline: URP `17.5.0`; project default is
  `Assets/BattleRaja/Content/BattleRaja-M0-URP.asset`
- Input: Unity Input System only (`activeInputHandler: 1`), Input System `1.20.0`,
  uGUI `2.5.0`
- Test framework: Unity Test Framework `1.7.0`
- Android profile: IL2CPP, ARM64-only, min API 28, target API 36, custom candidate
  icon/splash, debug-signed release-shaped candidate only
- Web profile: WebGL2/WebAssembly, uncompressed development output, focused canvas
  template at `Assets/WebGLTemplates/BattleRaja/index.html`
- Services: Unity Analytics/Ads/Performance Reporting disabled; V1 Android is
  intended to be fully offline
- Git branch at analysis time: `codex/v1-playstore-release`
- Last analyzed commit: `33035e84e86b41956b968f4c628aaa79c1496d49` on 2026-08-24;
  working tree was clean at that commit before this context-document refresh

## Important packages and frameworks

Direct registry dependencies are intentionally small:

- Universal Render Pipeline `17.5.0`
- Input System `1.20.0`
- uGUI `2.5.0`
- Unity Test Framework `1.7.0`
- Mono Cecil `1.10.2`, retained by the locally imported Photon Fusion vendor tree

Photon Fusion 2.1.1 Build 2177 exists as a locally imported vendor preparation under
`Assets/Photon`, including a non-secret Fusion App ID. It is not a direct package-manifest
dependency and no production first-party gameplay code depends on it. PlayFab is absent.
Real online sessions remain blocked by documented external approval/runtime gates.

## Directory structure

- `Assets/BattleRaja/Core`: pure domain and application orchestration assemblies.
- `Assets/BattleRaja/Gameplay`: reserved Unity-independent feature composition boundary.
- `Assets/BattleRaja/AI`: presentation-side perception/debugging support.
- `Assets/BattleRaja/Presentation`: Unity views, controllers, HUD, audio, camera,
  touch/mouse/gamepad adapters and scene bridges.
- `Assets/BattleRaja/Infrastructure`: networking, backend, analytics/release seams.
- `Assets/BattleRaja/Infrastructure/Platform`: Android and Web platform-filtered adapters.
- `Assets/BattleRaja/Content`: ScriptableObject definitions, weapons/fighters/gadgets,
  movement tuning, materials and Bazaar architecture prefab.
- `Assets/BattleRaja/Scenes`: Bootstrap, Tutorial Arena, Bazaar Bastion, MovementLab.
- `Assets/BattleRaja/Editor`: controlled scene generation, validation and build entrypoints.
- `Assets/BattleRaja/Tests`: EditMode and PlayMode test assemblies.
- `Tools/Build` and `Tools/Validation`: PowerShell build/test/packaging/validation helpers.
- `Docs`: mandatory product/architecture briefs, decisions, research, QA evidence and budgets.

## Assembly boundaries

- `BattleRaja.Core.Domain`: pure C#, no engine references, no references.
- `BattleRaja.Core.Application`: pure C#, references Domain only.
- `BattleRaja.Gameplay`: pure C# feature-composition boundary; references Domain/Application.
- `BattleRaja.Presentation`: Unity-facing views/adapters; references Domain/Application/Gameplay/Input System.
- `BattleRaja.Infrastructure`: transport/backend/analytics/platform seams; references Domain/Application.
- `BattleRaja.Infrastructure.Android`: include-platform Android adapter.
- `BattleRaja.Infrastructure.Web`: include-platform WebGL adapter.
- `BattleRaja.Editor`: editor-only generation/validation/build entrypoints.
- `BattleRaja.Tests.EditMode` and `.PlayMode`: separate test assemblies.

Dependency direction is inward. Static validation rejects Unity/vendor types in Core and
rejects Presentation code calling simulation mutators directly. Presentation must go through
application authority ports.

## Scenes and startup flow

Build order:

1. `Assets/BattleRaja/Scenes/Bootstrap/Bootstrap.unity`
2. `Assets/BattleRaja/Scenes/Tutorial/TutorialArena.unity`
3. `Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity`
4. `Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity`

Runtime flow:

1. `ProductionFlowController` builds the bootstrap Canvas, safe area, EventSystem and UI.
2. `ProductionFlowMachine`, a pure C# machine, owns deterministic menu/mode/fighter/loading/error state.
3. Fighter selection persists locally and `PlayerFighterSelection` enables Bijli, Pehel or
   Maya on actor 1, refreshes its attack definition and shared movement-lock seam.
4. The controller asynchronously loads Tutorial Arena or Bazaar Bastion.
5. `OfflineMatchController` starts the authoritative match and publishes canonical tick data.
6. Match end publishes immutable participant snapshots to the Canvas HUD/rematch surface.

Bazaar Bastion is the production greybox scene. MovementLab remains a technical regression
fixture. Tutorial Arena overlays guidance on real match authority while bot decisions stay
disabled there.

## Architecture

### Command and authority model

Human input, bot decisions and future network clients produce common immutable commands:
`MovementCommand`, `AttackCommand`, `AbilityCommand` and `GadgetUseCommand`. There is no
bot-only mutation shortcut.

`OfflineMatchController` advances one explicit 30 Hz `FixedSimulationClock`. For each consumed
step it emits `SimulationTickAdvanced`, collects movement commands, resolves them through
`OfflineMatchAuthority`, advances authority once, reconciles projectile shells, mirrors damage,
healing, pickup/gadget collection and expiry intents into Unity, updates spectator state and
publishes results when terminal.

`OfflineMatchAuthority` wraps the pure `OfflineMatchSimulation` plus authority-owned runtimes
for cooldowns, projectiles, movement/collision, gadget inventory/effects/stations, Pehel charge
throw and Maya decoy. It returns immutable snapshots/intents; Unity never directly mutates
canonical health, position, inventory, projectile identity or results.

`DeterministicCollisionSolver` handles bounds and ordered axis-aligned obstacle sliding for
authority movement/ability displacement. Physics remains available to presentation for raycasts,
perception, aim-assist discovery and visual collision where explicitly allowed.

### Gameplay systems

- Match/Aandhi: phases, spawn protection, warning/closing state, outside damage, elimination,
  placement, winner, damage ledger and assists live in `OfflineMatchSimulation`.
- Fighters: Bijli dash, Pehel charge throw and Maya decoy share stable IDs, definitions and
  command interfaces; fighter-specific executors sit behind `IFighterAbilityController`.
- Combat: attack validation/cooldown/projectile identity are authority-owned; pooled Unity
  projectile shells reconcile against canonical snapshots.
- Gadgets: Umbrella Guard mitigation, Dhol Burst displacement and Tiffin healing/lifetime/damage
  resolve through authority before presentation effects run.
- Bots: `BotPerceptionSensor` builds bounded observations, `BotDecisionEngine` performs seeded
  utility selection/reaction/noise, and `BotBrain` submits normal player-shaped commands.
- Flow/tutorial: pure machines own transition/step logic; Unity components render prompts and
  persist only local preferences/completion.

### Infrastructure seams

- `NetworkSessionMock` proves protocol/version/room/input/packet-loss semantics locally.
- `PhotonFusionAdapter` reports credential-required until approved real integration.
- `AuthoritativeMatchServer` is a local transport-independent M9 proof around the pure match,
  not a deployed server.
- `FakeProgressionBackend` proves guest/link/profile/idempotent reward contracts.
- `PlayFabBackendAdapter` reports credentials-required.
- `DevelopmentAnalyticsSink`, `CrashReportingAdapter` and release configuration guards keep M11
  truthful without uploading telemetry or shipping secrets.

## Coding conventions

Evidence from representative files:

- One primary type per file; PascalCase public members; camelCase private fields prefixed `_`.
- Serialized fields use `[SerializeField] private` with explicit defaults/ranges where useful.
- Runtime mutable state belongs in per-instance classes/structs, never shared ScriptableObjects.
- Pure systems validate inputs and return typed results/reasons rather than throwing normally.
- Presentation adapters cache component references and avoid repeated scene scans in hot paths.
- Scene/prefab structural changes use editor entrypoints rather than hand-editing large YAML.
- Documentation and decision records accompany material architectural changes.

## Testing and validation

- `Tools/Validation/run_unity_tests.ps1` runs batch-mode EditMode or PlayMode suites and parses XML.
- EditMode covers pure movement, combat, fighters, bots, gadgets, match authority, collisions,
  projectiles, replay determinism, soak, network/server proofs, backend proofs, flow and tutorial rules.
- PlayMode covers generated scenes, bootstrap/fighter selection, movement/combat/bot/gadget labs,
  offline match routes, tutorial arena, HUD/safe-area/accessibility propagation and reduced-flash settings.
- `Tools/Validation/validate.ps1` checks required paths, package/service policy, assembly purity,
  forbidden presentation mutation, generated-path tracking, LFS declarations and secret patterns.
- `Tools/Build/Android/build.ps1` and `Tools/Build/Web/build.ps1` invoke approved Unity build methods.
- Editor methods regenerate Bootstrap/Bazaar/Tutorial scenes, validate pinned versions/content,
  configure ARM64/API 36/IL2CPP or Web template, and create APK/AAB candidates.

At exact artifact source `35de9f3`, recorded fresh validation was 0 errors/0 warnings, EditMode
125/125 and PlayMode 73/73. Later commits may be runtime-bearing; tests/builds must be rerun
before claiming current-HEAD validation.

## Available Unity tooling

No Unity MCP provider/tool session was used in this inspection. Repository evidence and shell
inspection were sufficient. Unity MCP capabilities should therefore be treated as unverified,
not installed or configured by this analysis.

## Important constraints and gates

- Work only within Milestone 11 unless the owner changes scope.
- Never add Photon/PlayFab/package/editor-version changes silently.
- Never put external SDK types in Core/Application.
- Never trust client-reported combat, movement, pickups, rewards or match results.
- Never treat the browser tab as trusted public-match authority.
- Keep V1 Android offline, analytics-free and secret-free.
- Publication, signing, final package identity, store/legal/cultural review, paid infrastructure
  and deployment require human approval.

## Unknowns, confidence and risks

- Confirmed: environment, packages, assembly graph, scene/build order, architecture and code paths.
- Likely: performance behavior from recorded device/browser smoke; formal profiler budgets still open.
- Unknown/unverified: real Fusion sessions, real PlayFab integration, sustained frame pacing/GC/GPU,
  low-end-device stability, mobile Web, Firefox/Safari, final signed-store readiness.
- Current risks: human interaction/accessibility/performance review, Web rebuild reproducibility,
  temporary package/signing identity, and the need to rerun all validation after later runtime commits.

## Source files inspected

Representative authoritative/config sources:

- Mandatory documents: `AGENTS.md`, `PROJECT_STATUS.md`, `Docs/MASTER_VISION.md`,
  `Docs/DECISIONS.md`, `Docs/ARCHITECTURE.md`, `Docs/RESEARCH_LOG.md`
- Environment/settings: `Packages/manifest.json`, `ProjectSettings/*`, all first-party `.asmdef`
  files, `Assets/BattleRaja/Content/Movement/BattleRajaMovement.inputactions`
- Core/application: `OfflineMatchAuthority.cs`, `ProductionFlowMachine.cs`, `TutorialStepMachine.cs`,
  `OfflineMatch.cs`, `Gadgets.cs`, `FighterDefinition.cs`, `FighterKits.cs`, `BotAI.cs`,
  `AuthoritativeProjectile.cs`, `DeterministicCollisionSolver.cs`
- Presentation: `OfflineMatchController.cs`, `MovementPlayerAgent.cs`, `PlayerInputAdapter.cs`,
  `CombatAttackController.cs`, `CombatDamageResolver.cs`, `CombatProjectilePool.cs`,
  `BotBrain.cs`, `BotPerceptionSensor.cs`, `PlayerFighterSelection.cs`,
  `ProductionFlowController.cs`
- Infrastructure/tooling: `NetworkProof.cs`, `ServerMatchProof.cs`, `ProgressionBackendProof.cs`,
  `ReleaseProof.cs`, `BuildEntrypoints.cs`, `validate.ps1`, `run_unity_tests.ps1`, Android/Web build scripts
