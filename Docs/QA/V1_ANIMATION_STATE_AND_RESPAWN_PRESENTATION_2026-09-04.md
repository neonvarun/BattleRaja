# V1 animation state and Bastion lifecycle presentation — 2026-09-04

## Scope

This checkpoint closes a player-facing presentation gap in the offline Bastion Crown
route. The saved fighter Animator now has readable, authority-driven states for gadget
use, Crown pickup/carry/deposit, KO, confirmed respawn and spectator flow. A confirmed
respawn restores the render-only fighter colours, badge and pose only after the match
controller has accepted the canonical authority handoff. Crown, health, ticket,
targeting and replay ownership remain in the domain/controller authority.

Source commit: `abcbe04` (`feat: mirror Bastion lifecycle in fighter presentation`).

The follow-up documentation tip `5fbecb2` passed GitHub
[Repository validation run #105](https://github.com/neonvarun/BattleRaja/actions/runs/33898113032)
in 28 seconds, including the portable whitespace/LFS and tracked-source secret checks.

## Behavioural contract

- `FighterPresentation` mirrors immutable `BastionParticipantSnapshot` and
  `CrownSparkSnapshot` data. It does not award Crown state, revive actors, spend tickets
  or decide spectator targets.
- Accepted gadget commands trigger a short `GadgetUse` pose. Crown carrier transitions
  trigger `CrownPickup`, persistent carriage uses `CrownCarry`, and an active deposit
  channel uses `CrownDeposit`.
- Defeat holds a readable `KO` pose briefly, then shows `Spectator` when the authority
  marks the participant spectating. `Eliminated` remains a source-compatible enum alias.
- The controller calls `NotifyRespawned` only after the legacy actor respawn and
  `BastionCrownMatch.ConfirmRespawn` both succeed. The visible card therefore cannot
  reappear from a stale ready snapshot.
- The animation library is saved as editable `.anim` clips and a controller. The
  controlled art rebuild regenerated the three full authored fighter prefabs after the
  initial animation pass, preserving all accessory meshes and the two-bone production
  rig.

## Automated evidence

- Repository validation: `Tools/Validation/validate.ps1 -RequireUnityProject` — **0
  errors / 0 warnings**.
- Full EditMode: **168/168 passed**. XML
  `Builds/Local/TestResults/editmode-animation-20260904.xml` (130,607 bytes; SHA-256
  `B35DAED620E8A17BDDED6C127126723206E84F96665977321980CEB07F655E7F`); log SHA-256
  `B42C15D35BBDFECF55C68EFCF92103211763DF92D5E42D03514A150C99C04C01`.
- Full PlayMode: **99/99 passed**. XML
  `Builds/Local/TestResults/playmode-animation-20260904.xml` (88,867 bytes; SHA-256
  `6DF64D6702FB47891518CB32B16A45698133FFD30DEF8426FE0CEE8466689C7A`); log SHA-256
  `94D6A0CFB518BFDC461D9688E798828455AF81BF66493B5200E949C50DD1DBE8`.
- The PlayMode suite covers the saved clip library, full fighter rig/VFX composition,
  gadget-use state, terminal KO and the confirmed Bastion respawn mirror. The
  deterministic replay/soak contract remains unchanged; the fresh production-bot batch
  below confirms the full Bastion pipeline after this presentation change.

## Current-source production-bot batch

For additional confidence after the presentation hooks were integrated, the full
PlayMode suite was rerun with `BATTLERAJA_PRODUCTION_BOT_MATCHES=100`,
`BATTLERAJA_PRODUCTION_BOT_ASSERT_RELEASE_GATES=1`,
`BATTLERAJA_PRODUCTION_BOT_PLAYBACK_SCALE=50` and base seed `9101`. The test passed
**99/99** and generated 100 per-match replay records plus the aggregate report
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260904-170221495-9101.json`
(2,454,741 bytes; SHA-256
`3BC14EF496098B0C89FB543DEEF3E371A39115547557273006CAF5EAEB299491`). The NUnit
report is `Builds/Local/TestResults/production-bot-animation-20260904.xml`
(88,957 bytes; SHA-256
`D9E8598A82151ED1C16E0ADBF582E94B2C316937E071A367896E5FCCC0F88E93`) and its log is
`Builds/Local/Logs/production-bot-animation-20260904.log` (210,072 bytes; SHA-256
`ED11E68A3F01717D18DC7AD861257D78FF01283D1ED0B6DE018C23333B541B7F`).

All **100/100** matches reached a terminal result within the tick budget; **87/100**
landed in the documented 240–360 second window (mean `234.666 s`, range
`99.199–273.022 s`), **99/100** contained a combat elimination and **100/100** had
bot-to-bot damaging pairs. Aandhi-only resolutions were **0/100**. Protected-warmup
damage, invalid positions and continuous stuck ticks were all **0**. Attack telemetry
recorded 17,919 attempts with 69 out-of-range diagnostics; ability telemetry recorded
18,833 attempts with 4,394 rejections. Successful gadgets totaled **255**: Umbrella
Guard **65**, Dhol Burst **90** and Tiffin Station **100**. The batch recorded **618**
respawns, **612** combat KOs, **19** Aandhi KOs, 3,021 unique damaging pairs, 176,032
squad signal updates, 576,585 escort assignments, 310,776 support assignments and
84,813 retreat signals. These are fixed-tick diagnostics, not a normalized frame-rate,
thermal or human-fun claim.

## Android candidate artifacts

Built with Unity `6000.5.6f1`, IL2CPP, ARM64, min API 28 and target API 36, using the
temporary owner-unapproved package `com.example.battleraja.m11`:

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,687,756 bytes,
  SHA-256 `9F2E70034CFF9B0DE4A04490B084E1FCEBFD1B8C703C9B1FC5A3D8D0D692B613`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,513,267 bytes,
  SHA-256 `750EB62BF1012BB54938E0402FF2E0D84B3450145180D95186AA0B4A8C84A13D`.
- Clean technical release gate: **passed** with 0 validation errors/warnings,
  offline network-permission gate, ARM64-only bundle, all seven native libraries at
  `0x4000` ELF load alignment, and 512×512 / 1024×500 store-creative dimensions.
  Full output is `Builds/Local/V1GameplayTruth/Next/animation-lifecycle-20260904/release-checker.log`
  (3,214 bytes; SHA-256
  `55F6658FABFFE705D9DAD093BB9213E29EFA45694196F69A5616E21865CD852E`).

The build log is `Builds/M11/Logs/android-build.log` (441,615 bytes; SHA-256
`6572C84AE84F5A8BFD9BF9C4BEB127ADF077FD86365F9D5C81BFA13BD3D53115`). Unity emitted
its known non-fatal icon-export warning during Android post-processing; the build and
all technical gates completed successfully.

## Device and release limits

`adb devices -l` currently exposes only Oppo `b60e53b3`. The approved evidence device
is Lava `ST5GDW23LB004392`; it is not currently visible, so no device was installed or
used for this checkpoint. No Oppo result is treated as evidence. Current-source Lava
visual smoke, physical 16 KB runtime proof, normalized sustained performance and the
owner gates for permanent package identity, signing, privacy/Data Safety, IARC,
cultural review and Play submission remain open. The truthful classification remains
**Prototype — Android offline release candidate in progress**.
