# V1 Crown timer determinism — 2026-09-04

## Scope

This checkpoint hardens the Bastion Crown objective timer for coarse diagnostic/replay
steps. It is an incremental authority fix and does not close the broader physical 4v4,
AI balance, performance, authored presentation or Play Store gates.

## Defect and fix

`BastionCrownMatch.AdvanceCrown` previously called `RotateCrown()` at most once when one
advance crossed multiple 35-second rotation intervals. A coarse step could therefore land
on a different socket than the equivalent fixed-step replay. The timer now carries overdue
time through every required rotation, preserving socket, position and remaining timer
state across step sizes.

Source commit: `bad12de` (`fix: preserve coarse crown rotation timing`).

## Automated evidence

- Static validation: **0 errors / 0 warnings**.
- Focused regression:
  `BattleRaja.Tests.EditMode.BastionCrownMatchTests.CrownRotationPreservesOverdueTimeAcrossCoarseAdvance`
  passed.
- Full EditMode: **162/162 passed**. XML SHA-256
  `5C172CD9B52C598277D3C00F43A276D0A08FF5DA4FCE276C2C326F9C1C3892C1`; log SHA-256
  `3D8F5BB40E620D56B3275441A290352F4B70446C18B7848D0DCF800F8BFDEA2F`.
- Full PlayMode: **98/98 passed**. XML SHA-256
  `108D32758C5C0D783011FD7C4F6691684D6E0279CB9157FBB46BBCD80FACE855`; log SHA-256
  `ECF309896F775D6560738D276A011DF336856ACD7C4880352B7672E7321EC712`.

The full PlayMode run remains unchanged by this pure-domain timer fix and passed again.

## Android candidate evidence

- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,680,452 bytes,
  SHA-256 `E92E5994C36B35414DB44D32C082DC8992A3E413F9B67BD87FF776BF5C42DF6C`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,505,982 bytes,
  SHA-256 `19882B28E14DE5D9A0B73CCF7016FCA0983325C1F93C4E4BDD36D7E908FB470F`.
- Build log SHA-256: `0BF04322DFFFDB3345B7F2330A82E755AFB71DC53FDE1B312EB9F646C1ACD58B`.
- Technical release checker: **0 errors / 0 warnings**; log SHA-256
  `D0AEFF927B7221178B05F73E2247BE0FAF1DA6839E47E15D57FA4F04E46F61B0`.
  Package remains temporary `com.example.battleraja.m11`, version `1.0.0` / code `100`,
  API `28/36`, offline permissions, seven ARM64 libraries and static 16 KB ELF alignment.

## Approved Lava smoke

The exact APK was installed after clearing `com.example.battleraja.m11` on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 1080x2460, reported 4 KB pages).
The pulled installed base matches the APK hash. Fresh menu evidence is
`Builds/Local/V1GameplayTruth/Next/crown-rotation-20260904/00-menu.png` (SHA-256
`9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`). The scoped logcat
is `postfix-logcat.txt` (SHA-256
`B8B3C7EA33036FFCD8FBD6CFFA7B353352ACF6F2B69602CBB1BB986475AD99D5`) with zero
configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or `SIGABRT` markers.

This is a bounded artifact/menu smoke, not proof of a complete physical Crown-deposit,
spectator/rematch route, normalized FPS/GPU/GC, endurance, genuine 16 KB runtime, touch
comfort, final authored assets, cultural review or owner approval.
