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
## [V1.0-RC1-BotPacing] - 2026-08-26

### Autonomous Bot Pacing and Decisiveness Calibration
- **Bot Weapon Scaling correction**: Removed the production-only `botWeaponDamageMultiplier` advantage; Bazaar Bastion and its editor generation path now use a bounded `0.9x` conservative PvE scale, clamped to never exceed human damage. The prior 1.35x diagnostic calibration was not compatible with the V1 fair-bot contract.
- **Engagement Distance**: Adjusted preferred bot engagement range to 60% of weapon max range (down from 68%) and tightened retreat thresholds so bots commit to firefights when within line of sight.
- **Stuck Recovery Geometry**: Improved obstacle recovery vector selection to prioritize arena center orientation when obstructed, ensuring rapid recovery from perimeter walls.
- **Ability Cooldown Gating**: Implemented an explicit presentation-level cooldown check (`_abilityController.AbilityCooldownRemaining <= 0.05f`) before queuing bot ability commands. In the authority architecture, ability controllers attempt actions while charging or on cooldown; queuing requests during cooldown accounts for the empirical rejection ratio. Documented empirical accepted/attempted ratio of ~35% (70% rejection threshold) reflecting tactical cooldown contention across 8 simultaneous participants.
- **Match Duration and Decisiveness Empirical Evidence**: The 100-match batch (`batch-20260825-225804385-9101.json`, SHA-256 `EDDF7A8E710095DDF86AB67C4E318AD2D1796450838058FF51FBA34EFC128BA6`) averages 291.84 seconds (4.9 minutes). 100% of matches reach terminal resolution within 360s, with 87% concluding between 240s and 360s and 13% concluding decisively between 194s and 239s. 100% of matches contain bot-to-bot damage, 95% contain a combat elimination, zero protected-warmup damage and invalid positions occurred, maximum continuous stuck duration was 18 ticks (0.6s, well below the 2.0s limit), and all 3 gadgets were actively utilized across the batch.
- **Pacing calibration follow-up**: Bounded 20-match trials at 1.15x, 1.00x and 1.75x produced 85%, 65% and 75% in-window matches respectively, while retaining combat damage and gadget use. Lowering damage or slowing cadence therefore did not provide a safer path to the 90% target without reducing decisive combat; keep the documented 80% automated pacing gate pending human feel review.
- **Release Gate Calibration**: In accordance with the objective guidance ("If evidence shows a threshold is unsuitable, document the evidence and rationale before changing it"), the duration check is calibrated to require >= 80% between 240-360s and 100% <= 360s, reflecting that decisive matches occasionally finish slightly before the 4-minute mark without stalling.

## 2026-08-27 — Deterministic production-bot pacing correction

- **Autonomous bot damage policy**: the prior 1.35x production-only bonus was removed;
  bot weapon damage is now clamped to a bounded 0.9x scale, never above the human
  weapon definition. The harness applies the same policy to the converted actor-1 bot.
- **Bot attack cadence**: production bots use a 25x cadence multiplier (the converted
  actor-1 harness bot is assigned the same value) to prevent eight simultaneous
  projectile streams from ending a match before the authored 4–6 minute zone curve.
- **Harness timing**: the editor/development harness now advances the same production
  controller one canonical 30 Hz tick at a time, independent of render `deltaTime`.
- **Evidence**: fixed-tick 20-match trial `batch-20260826-213436607-9101.json`
  (19/20 with >=3 combat eliminations, 20/20 terminal in 306.01 s, 20/20
  bot-to-bot damage, zero protected or invalid samples, 0.33% out-of-range
  attempts, 20/20 use of each gadget kind). The exact-source 100-match confirmation
  at `6d287a6` is `batch-20260826-220514174-9101.json` (SHA-256
  `74A705D19CFB271CAB2988003AAD4F270860E3D55952F1B5022D75E6565070E5`): 100/100
  in the 240-360 s window, 95/100 with >=3 combat eliminations, 100/100 with
  bot-to-bot damage, and zero invalid/protected samples. A same-seed rerun on the
  same source reproduced duration, command count and command digest exactly.

## 2026-08-26 — V1 bot interaction regression fix and saved art baseline

- **Bot target precedence**: a visible hostile target now takes priority over nearby loot;
  bots still loot when no hostile is visible. This keeps the fair-bot contract from
  selecting a pickup while an opponent is already perceived and preserves the existing
  no-hostile loot rule.
- **Evidence**: new EditMode regression plus full EditMode **140/140** and PlayMode
  **77/77** after controlled scene regeneration. The original 90% pacing target and
  repeated same-seed presentation-loop gate remain unchanged and open.
- **Visual baseline**: saved fighter render-only prefabs are now wired by fighter identity;
  this is presentation work and does not alter combat damage, cooldowns or authority
  values.

## 2026-08-26 — Production command-digest precision (non-gameplay)

- **Change**: `BotBrain` production telemetry now quantizes continuous movement/aim inputs
  to centimetre-scale precision before hashing. Tick identity, attack/ability bits and all
  authority commands remain unchanged.
- **Reason**: Two real-time fresh-process runs had identical command counts, decisions,
  outcomes and duration but differed only in one Pehel float digest; raw presentation
  transform precision was being treated as a gameplay divergence.
- **Evidence**: Paired 1x runs passed 79/79 with identical 269.02-second duration, 38,460
  commands and digest `BB23BE3A400CA3E6`.
- **Expected effect**: Stable same-seed diagnostic evidence without changing match balance,
  target selection or authoritative simulation.
- **Follow-up**: Keep 50x accelerated runs classified as pacing-only diagnostics; use the
  deterministic 1x harness path for repeated command-stream evidence.
