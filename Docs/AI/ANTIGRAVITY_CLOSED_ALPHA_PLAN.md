# BattleRaja Closed-Alpha Execution Plan

Date: 2026-08-04  
Branch: `antigravity/closed-alpha-completion`  
HEAD: `a1e084d`

---

## Stage 0 — Exact Current-Source Baseline
- **Objective**: Establish clean baseline before architectural changes.
- **Confirmed Problems**: Validated test suite, zero compiler errors/warnings, verified Android APK & Web artifact baselines.
- **Files to Change**: `Docs/QA/ANTIGRAVITY_STARTING_BASELINE.md`, `Docs/QA/ANTIGRAVITY_CURRENT_STATE_AUDIT.md`, `Docs/AI/ANTIGRAVITY_CLOSED_ALPHA_PLAN.md`.
- **Tests**: `pwsh -File Tools/Validation/validate.ps1 -ProjectRoot .`, EditMode (114/114), PlayMode (54/54).
- **Acceptance Criteria**: Repository clean, zero warnings, baseline document recorded with exact test counts and hashes.

---

## Stage 1 — Complete Deterministic Bazaar Collision
- **Objective**: Implement stable, Unity-independent collision representation matching Bazaar geometry.
- **Confirmed Problems**: Bazaar's domain collision definition contains bounds, but authored obstacle geometry is missing from `ArenaCollisionDefinition`.
- **Files Likely to Change**:
  - `Assets/BattleRaja/Core/Domain/ArenaCollisionDefinition.cs`
  - `Assets/BattleRaja/Editor/BazaarObstacleBaker.cs` [NEW]
  - `Assets/BattleRaja/Core/Domain/MovementMotor.cs`
  - `Assets/BattleRaja/Presentation/Fighters/*FighterController.cs`
- **Tests**: Bounds clamp, wall slide, thin-wall tunneling, Pehel capture/throw through cover, Maya placement.
- **Acceptance Criteria**: Core collision solver handles authored obstacles; core has zero Unity collider dependencies; full test suite passes.

---

## Stage 2 — Canonical Match Runner
- **Objective**: Consolidate match tick ordering into a single canonical authoritative execution loop.
- **Confirmed Problems**: Ticking steps are distributed across presentation accumulators.
- **Files Likely to Change**:
  - `Assets/BattleRaja/Core/Application/OfflineMatchAuthority.cs`
  - `Assets/BattleRaja/Core/Domain/OfflineMatchSimulation.cs`
- **Tests**: Fixed-clock tick equivalence at 30, 60, 90 rendering FPS and frame hitches.
- **Acceptance Criteria**: Identical input streams produce identical state hashes across variable render rates.

---

## Stage 3 — Authoritative Projectiles
- **Objective**: Move projectile travel, sweeping, collision, and target selection into Core Domain.
- **Confirmed Problems**: `CombatProjectile` in Presentation currently runs physics sphere-casts.
- **Files Likely to Change**:
  - `Assets/BattleRaja/Core/Domain/AuthoritativeProjectile.cs` [NEW]
  - `Assets/BattleRaja/Presentation/Combat/CombatProjectilePool.cs`
  - `Assets/BattleRaja/Presentation/Combat/CombatProjectile.cs`
- **Tests**: Wall collision, nearest target, equal distance tie-breaking, decoy/station hitting.
- **Acceptance Criteria**: `CombatProjectile` is a pure visual view interpolating domain snapshots.

---

## Stage 4 — Stable Identities & Atomic Resolution
- **Objective**: Introduce bounded identity counters for inputs, attacks, projectiles, abilities, gadgets, and events.
- **Confirmed Problems**: Sequence numbers and event IDs do not use bounded domain identity structures.
- **Files Likely to Change**:
  - `Assets/BattleRaja/Core/Domain/MatchEventIdentity.cs` [NEW]
  - `Assets/BattleRaja/Core/Application/OfflineMatchAuthority.cs`
- **Tests**: Sequence rollover, duplicate event rejection, atomic state mutation.
- **Acceptance Criteria**: Retransmissions/duplicates rejected; all gameplay mutations occur inside authority.

---

## Stage 5 — Replay & Soak Testing
- **Objective**: Implement input recording, deterministic replay runner, and long-running match soak tests.
- **Acceptance Criteria**: 1,000+ accelerated matches without state divergence, memory growth, or unhandled exceptions.

---

## Stage 6 — Performance & Size Optimization
- **Objective**: Profile and optimize Android APK memory/CPU and Web WASM bundle sizes against `Docs/PERFORMANCE_BUDGET.md`.

---

## Stage 7 & 8 — Closed-Alpha Presentation, Accessibility & Visual QA
- **Objective**: Finalize presentation feedback, telegraphs, audio routing, Canvas UI ergonomics, and multi-viewport visual QA.
