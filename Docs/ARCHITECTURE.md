# Architecture

**Status:** Implemented and accepted for Milestone 0. Changes beyond these boundaries require a new decision record.

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

## Platform boundary

Android and Web share domain, application and most presentation code. Platform-specific identity, storage, haptics, fullscreen, deep links, purchases, browser lifecycle and hosting integration belong in infrastructure adapters.
