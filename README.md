# BattleRaja

BattleRaja is an Android-app and browser-Web stylised top-down 3D micro battle royale with short matches, readable character combat, original Indian-subcontinent-inspired worldbuilding, temporary **Jugaad Gadgets**, and a closing **Aandhi** battle zone.

## Current status

**Pre-production / Milestone 3 Bijli fighter complete provisionally; Milestone 4 is next**

Movement, aiming, the central projectile combat loop and a data-driven Bijli bolt-and-dash fighter are implemented as a grey-box laboratory. Bots, gadgets, networking, backend, economy and final art are not implemented yet.

## Source of truth

Read these files in order:

1. `AGENTS.md`
2. `Docs/MASTER_VISION.md`
3. `PROJECT_STATUS.md`
4. `Docs/DECISIONS.md`
5. `Docs/ARCHITECTURE.md`
6. `Docs/RESEARCH_LOG.md`
7. `Docs/MILESTONE_0_EXECUTION_PLAN.md`

## Initial product target

- Android application and browser-playable Web build
- One Unity `6000.5.6f1` + C# + URP project targeting Android and Web
- One compact arena
- Three fighters
- Three Jugaad Gadgets
- Eight combatants
- One human and seven bots before multiplayer
- One complete 4–6 minute match
- Offline simulation core before Photon or PlayFab
- Android–Web cross-play as an online-alpha target

- Use `PROMPTS/99_AUTOPILOT_M1_TO_M11.md` for one long Goal-mode run.
- Use numbered prompts for safer milestone-by-milestone execution.
- Read `COPY_INTO_EXISTING_PROJECT.md` first.

The target is an Android + Web closed-alpha/release candidate. Public launch and real external services still require credentials, approvals and human review.

## Current local run

Open the Unity project at the repository root with Unity `6000.5.6f1` and load `Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity`. Desktop uses WASD/arrow keys, mouse aim, left-click attack and Space dash; Android uses the two virtual sticks plus attack and dash controls.

Build commands are documented in `Tools/Build/Android/README.md` and `Tools/Build/Web/README.md`. Review `PROJECT_STATUS.md`, `Docs/MOVEMENT_LAB.md` and `Docs/MILESTONE_REPORTS/M3.md` before authorizing bot work.

## Human owner

Varunkumar Singh / Avinya Studios
