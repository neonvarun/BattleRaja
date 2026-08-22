# Balance Changelog

Record every fighter, weapon, gadget, Aandhi or match-rule balance change with:

- Date/build
- Old value
- New value
- Reason
- Evidence
- Expected effect
- Follow-up result

## 2026-08-21 — Closed-alpha pacing and lethality retune (HEAD 355a9b0+)

- **SoloRaja zone curve** (Docs source: OfflineMatchDefinition.SoloRaja)
  - Opening: duration 90 s -> 105 s, warningSeconds 8 -> 50 (full arena held ~63 s from match start instead of ~16 s).
  - Pressure: radius target 8 -> 11 at phase start (gentle first squeeze), warningSeconds 8 -> 25; closing 11 -> 4 across the phase.
  - FinalCircle: radius 3.5 -> 4 (open plaza containment), duration 80 -> 78.
  - Total match time 298 s -> 306 s (within the 4-6 minute product target).
  - Reason: on-device evidence showed Closing state during Opening and storm-driven zero-damage eliminations within the first minute.
- **Bot difficulty defaults** (BotBrain serialized defaults)
  - reactionDelayTicks 8 -> 15; aimNoise 0.10 -> 0.12 (an interim 0.18 pass was reverted after on-device evidence showed storm-only eliminations); preferredRange 5.5 -> 6.5; decisionIntervalSeconds 0.16 -> 0.20.
  - Reason: duels resolved in seconds; target is readable mid-match pressure, not instant cascades.
- **Weapon damage**
  - BijliElectricBolt 18 -> 12; PehelSweep 28 -> 20; MayaShard 12 -> 9 (TrainingBolt lab-only value unchanged).
  - Reason: raise time-to-kill toward closed-alpha readability targets for all three fighters uniformly.
- Evidence: EditMode 114/114, PlayMode 56/56 including new
  ProductionProjectileViewsRetireThroughAuthoritySnapshots regression; device
  re-verification recorded after this entry.
- Follow-up result: pending full-length physical match pacing review (human gate).
- **Correction 2026-08-22 (`phase0/exact-source-rebaseline`):** the three weapon-damage
  values in this entry were recorded only in the serialized `.asset` files and never
  applied to the authoritative Core definitions
  (`FighterDefinition.Pehel/Maya.BasicAttack`, `ProjectileWeaponDefinition.BijliElectricBolt`),
  so every editor regeneration silently reverted the assets to 18/28/12 and all prior
  builds shipped those unretuned values. The definitions now carry the documented
  targets (Bijli 12, Pehel sweep 20, Maya shard 9), assets are synced from code by the
  editor entrypoint, and `BijliFoundationTests` pins Bijli bolt damage at 12.
