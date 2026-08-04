# BattleRaja Current-State Audit (Stage 0)

Date: 2026-08-04  
Branch: `antigravity/closed-alpha-completion`  
HEAD Commit: `a1e084d` (merged `main`)

---

## 1. Architecture & Assembly Dependency Map

```
[BattleRaja.Core.Domain]  <--- pure C#, noEngineReferences=true
       ^
       |
[BattleRaja.Core.Application] <--- pure C#, noEngineReferences=true
       ^
       +------------------------------------+
       |                                    |
[BattleRaja.Gameplay]            [BattleRaja.Infrastructure]
       ^                                    ^
       |                                    |
[BattleRaja.Presentation] ------------------+ (Unity MonoBehaviours, Views, UI)
```

- **Domain Layer (`BattleRaja.Core.Domain`)**: Pure C# logic containing mathematical types (`Float2`), fighter state, movement motor, health state, attack/ability commands, gadget catalogs, match simulation, and Aandhi zone rules. Absolutely zero Unity or vendor dependencies.
- **Application Layer (`BattleRaja.Core.Application`)**: Command sinks, `OfflineMatchAuthority`, bot brain interfaces, and flow state machines. Zero Unity engine references.
- **Presentation Layer (`BattleRaja.Presentation`)**: MonoBehaviours, CharacterControllers, visual views, Canvas HUD, Input System adapters, audio directors, and particle feedback.
- **Infrastructure Layer (`BattleRaja.Infrastructure`)**: Network adapters (`PhotonFusionAdapter`, `NetworkSessionMock`), progression adapters (`PlayFabBackendAdapter`, `FakeProgressionBackend`), and platform services.

---

## 2. Match Tick Flow

1. `OfflineMatchController` (Presentation) runs a 30 Hz `FixedSimulationClock`.
2. On each 30 Hz tick step, `OfflineMatchAuthority` (Application) advances the match simulation:
   - Evaluates input commands from `PlayerInputAdapter` and 7 `BotBrain` instances.
   - Updates `OfflineMatchSimulation` phase, Aandhi radius, and outside-zone tick damage.
   - Ticks application-owned gadget runtimes and pickup collection proximity (`CollectNearby`).
   - Publishes `MatchTickResult` snapshot to presentation observers (`OfflineMatchHud`, `SpectatorSelector`).

---

## 3. State Ownership & Trust Boundaries

- **Authority-Owned State**: Match phase, zone radius, actor health, elimination state, placement rankings, gadget inventory, pickup availability, weapon cooldowns, bot decisions.
- **Presentation-Owned View State**: Transform position rendering, animator triggers, UI canvas elements, visual flash effects, procedural audio cues.
- **Input Trust Boundary**: Inputs are received as unverified `MovementInputFrame`, `AttackCommand`, and `AbilityCommand` intents. `OfflineMatchAuthority` validates direction finiteness, phase bounds, spawn protection, and cooldown eligibility before executing commands.

---

## 4. Projectile & Collision Flow

- **Current Projectile Flow**: `CombatAttackController` converts input to `AttackCommand`. If accepted by `OfflineMatchAuthority`, `CombatProjectilePool` spawns visual `CombatProjectile`. Currently, `CombatProjectile` performs physics sphere-casts in Presentation and invokes `CombatDamageResolver` to request damage. *Stage 3 will move projectile travel, sweeping, collision, and hit selection completely into Core Domain.*
- **Current Collision Flow**: `ArenaCollisionDefinition` currently owns arena AABB bounds and deterministic sliding math in Core Domain (`a5fdde8`). *Stage 1 will populate Bazaar Bastion static obstacle geometry into `ArenaCollisionDefinition` to enforce 100% pure Core collision for movement, dashes, charges, decoys, and placements.*

---

## 5. Direct Mutation Escape Hatches & Status

- `OfflineMatchSimulation.SyncHealth` / `ApplyDamage`: Direct presentation calls are forbidden and blocked by validation rules (`validate.ps1`).
- `CombatDamageResolver`: Serves as the single authorized presentation bridge for executing domain `DamageRequest` intents on presentation `CombatHealth`.
- Photon Fusion (`Assets/Photon`): SDK imported (v2.1.1 Build 2177); currently gated behind `PhotonFusionAdapter` returning explicit `CredentialsRequired`. Real multiplayer remains dormant until offline authority gates pass.
- PlayFab: Interfaces & fake implementation present; real adapter returns `CredentialsRequired`.

---

## 6. Confirmed Open Defect Audit

1. **Bazaar Obstacle Bake**: Default collision definition contains arena bounds, but authored Bazaar Bastion static obstacle colliders are not yet baked into `ArenaCollisionDefinition`. (Target: Stage 1)
2. **Presentation Projectiles**: Projectile travel & sphere-cast hit detection remain in `CombatProjectile` (Presentation). (Target: Stage 3)
3. **Event Identity Bounds**: Event IDs for attacks, hits, abilities, and eliminations need formal bounded identity sequence objects. (Target: Stage 4)
