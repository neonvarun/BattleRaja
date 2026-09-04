# BattleRaja V1 authoritative respawn handoff — 2026-09-04

## Scope and source

This record covers the Bastion Crown respawn-authority hardening in source commit
`0d0f8759eb2fd6ccdf9a8c59126e2c7a610b19d0` (`0d0f875`) on branch
`codex/v1-playstore-release`. The change is intentionally narrow: it closes an
authority handoff gap without changing fighter balance, ticket counts, Crown scoring,
or the Solo rule path.

## Reproduced cause and decision

The timer previously exposed a ready actor through `RespawnedActors`, leaving the
adapter/application boundary without an explicit authoritative acknowledgement. A
mirror-side `SyncParticipant(alive: true)` could therefore revive a dead participant,
and a failed adapter handoff had no canonical retry state. Combat delivery also needed
to reject pre-live and post-terminal events.

The authority now reserves and spends a ticket once, marks the actor as
`RespawnIssued`, and keeps the participant dead and spectating until the application
successfully calls `ConfirmRespawn`. Until confirmation, each authority advance
re-emits the same actor without spending another ticket. `ConfirmRespawn` is the only
path that makes the actor live, applies spawn health/protection, and clears the pending
state. `RespawnIssued` participates in the deterministic hash. `SyncParticipant` rejects
an unapproved revival without mutating the participant, and combat damage requires a
started, live target.

The Unity controller and deterministic replay runner now perform the same ordered
handoff: authority respawn, adapter/view application, then authoritative confirmation.

## Changed files

- `Assets/BattleRaja/Core/Domain/BastionCrownMatch.cs`
- `Assets/BattleRaja/Presentation/Match/OfflineMatchController.cs`
- `Assets/BattleRaja/Core/Application/DeterministicReplayRunner.cs`
- `Assets/BattleRaja/Tests/EditMode/BastionCrownMatchTests.cs`

## Automated evidence

- Repository validation: **0 errors / 0 warnings** via
  `Tools/Validation/validate.ps1`.
- Focused `BastionCrownMatchTests`: **16/16 passed**. XML:
  `Builds/Local/V1GameplayTruth/Next/respawn-handoff-20260904/focused.xml`, SHA-256
  `B8E1799410373EE97E5C210E68EDF6CAA54BDA5E89B74C065BE35526DE630C19`.
- Full EditMode: **164/164 passed**, 0 failed/skipped. XML SHA-256
  `C502048AC2B71FB99DC9F2715B9629100EEFB5B885D5B628AF2026A26DEF46C5`.
- Full PlayMode: **98/98 passed**, 0 failed/skipped. XML SHA-256
  `82B43F49D33920943EFCC22BA2F83692C5E09E30062CFF1C5FB96B801FA4B051`.
- Strict production-bot PlayMode (`100` matches, release gates asserted,
  `50x` playback scale): **98/98 passed**, 0 failed/skipped. XML SHA-256
  `1C556756BCD45C8525EAA59FD113FAAD1B82FEB5FA3EF22974EF3B2F731B2551`.
- Production report:
  `Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260904-060314382-9101.json`,
  SHA-256 `A780A589803963B0F1D19B2D62D3C8326655086E9B6CF3341BF451D2773453B7`.
  All **100/100** matches terminated; **89/100** were in the 240–360 second window;
  **92/100** had combat eliminations; bot-to-bot damaging pairs were present in
  **100/100**; Aandhi-only resolutions were **0/100**; protected-warmup damage and
  invalid-position samples were both **0**. The batch recorded **284** respawns,
  **278** combat KOs and **10** Aandhi KOs, with **0** stuck ticks and **0**
  outside-participant samples. This is accelerated diagnostics; real-time replay
  evidence remains the determinism source of truth.
- Full EditMode replay/soak coverage remained green with no observed divergence.
- `git diff --check` passed and `git lfs fsck --pointers` reported `Git LFS fsck OK`.

## Android candidate evidence

The exact temporary/debug-signed candidate was rebuilt from this source:

- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,681,228 bytes,
  SHA-256 `0A4E7C96531F16ABAFDB4BDFB2CD587175360210B543FADEC19BF9B06DB91108`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,506,760 bytes,
  SHA-256 `19E0B84A8CACB760CA18DFDD8FC7AA3B5AE9232FB7F4E52F47A22B28DA6E842E`.
- Package/version: `com.example.battleraja.m11`, version `1.0.0`, code `100`, min API
  `28`, target API `36`.
- Technical release checker: **0 errors / 0 warnings**; offline permissions, ARM64
  payload, static 16 KB ELF alignment and store-creative dimensions passed. Captured
  output: `Builds/Local/V1GameplayTruth/Next/respawn-handoff-20260904/release-checker.log`,
  SHA-256 `3E80ABFA52F4872A430590818EDD257A975A28ABA31B58787AD153E2BC9DAB33`.

## Approved Lava evidence

Only Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 1080×2460) was used.
The exact APK installed successfully, and the pulled `base.apk` matches the APK SHA-256.
The bounded menu smoke rendered the branded portrait menu with `PLAY OFFLINE`,
`TUTORIAL REPLAY`, `SETTINGS & ACCESSIBILITY`, and `HELP & CONTROLS`. Process-scoped
logcat contained **0** configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV`, or `SIGABRT`
markers.

Evidence under `Builds/Local/V1GameplayTruth/Next/respawn-handoff-20260904/lava/`:

- `menu.png` — SHA-256
  `9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`.
- `base.apk` — SHA-256
  `0A4E7C96531F16ABAFDB4BDFB2CD587175360210B543FADEC19BF9B06DB91108`.
- `logcat.txt` — SHA-256
  `DC1952933B639A4DA8839BB5E0906D9CB383581BE3B7767E2A7CF9ABBB8F5213`.

The device reports 4 KB pages. This is a bounded menu/crash-marker smoke, not proof of
physical 16 KB runtime compatibility or complete action-by-action physical route,
comfort, accessibility, fun, cultural acceptance, or final visual/audio approval.

## Gate impact and remaining work

This checkpoint closes the reproducible respawn handoff, duplicate-ticket, mirror-revival,
and terminal-damage authority edge cases. It does not close the remaining final-art,
animation/VFX, audio, human gameplay/comfort, cultural/originality, normalized
performance/endurance, physical 16 KB, permanent package identity, signing, privacy/Data
Safety, content-rating, support URL, or Play Console gates. Truthful classification remains
**Prototype — Android offline release candidate in progress**.

