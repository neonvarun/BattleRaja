# BattleRaja Replay and Soak Report (Stage 5 Verification)

Rewritten 2026-08-22 with reproducible evidence from the exact current source
(`phase0/exact-source-rebaseline`). The previous version of this file claimed a
1,000-match soak with no commands or artifacts; those claims were unverified and are
replaced by the evidence below. The "Raja" fighter mentioned previously does not exist
in the product roster (Bijli, Pehel, Maya) and no longer appears here.

## Method

`Assets/BattleRaja/Tests/EditMode/DeterministicSoakTests.cs` runs accelerated offline
matches entirely through `OfflineMatchAuthority`: eight participants on verified spawn
positions under `ArenaCollisionDefinition.BazaarBastion`, seeded per-actor movement
commands resolved every tick, sparse seeded attack submissions gated to live phases,
canonical 30 Hz ticks until match resolution, and an FNV-1a state hash per tick via
`DeterministicReplayHasher.CalculateTickHash` (tick, phase, zone, participant
health/positions, projectile positions).

Every match is executed twice with the same seed; the two per-tick hash streams must be
element-for-element identical.

## Results — 2026-08-22

| Run | Command | Result | Evidence |
| --- | --- | --- | --- |
| Deep soak | `$env:BATTLERAJA_SOAK_MATCHES='1000'`; Unity `-runTests -testPlatform editmode -testFilter BattleRaja.Tests.EditMode.DeterministicSoakTests -testResults Builds/Local/TestResults/soak-1000.xml -logFile Builds/Local/Logs/soak-1000.log` | **Passed**; 1,000 seeded matches × 2 executions = 2,000 full matches, **zero divergence**; test duration 406.8 s, exit 0 | `Builds/Local/TestResults/soak-1000.xml`, `Builds/Local/Logs/soak-1000.log` |
| Full suite | default depth (4 matches × 2); full EditMode suite | **120/120 passed**, exit 0 | `Builds/Local/TestResults/editmode-a2.xml` |

Determinism across render rates remains covered by
`CoreFoundationTests.FixedClockProducesTheSameTickCountForDifferentRenderRates`, and
replay hashing/frame recording by `ReplayDeterminismTests`.

## Scope and honest limitations

- Soak drives movement + attacks through the authority; gadgets/pickups are not yet
  configured in soak matches (recorded follow-up, not hidden).
- Hash parity proves simulation determinism for identical command streams; it does not
  prove cross-machine float determinism or network transport correctness.
- Memory-leak observations are covered by PlayMode repeated-match regressions
  (`RepeatedResultsRematchesKeepRuntimeGraphClean`,
  `RepeatedProductionSceneLoadsKeepOneOfflineRuntimeGraph`), not by this EditMode soak.
