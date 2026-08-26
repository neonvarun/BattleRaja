# BattleRaja Replay and Soak Report

## Latest exact-source result — `aeda6de` — 2026-08-27

The presentation-only HUD/tutorial/results cleanup was verified without changing the
authority or replay code. At exact source `aeda6debab89404991f55a0f663a88798dd9c944`,
`BATTLERAJA_SOAK_MATCHES=1000` passed **1/1**: 1,000 seeded matches executed twice
(2,000 executions), zero divergence, NUnit duration **553.5464039 seconds**. XML
`Builds/Local/V1GameplayTruth/TestResults/deep-soak-ui-aeda6de.xml` SHA-256
`65BD32A7B978CB5679546EA3A7ACDFFC91261DC5D1A4CE86C3E280BB1B79C69F`; log SHA-256
`46CC6D65C14B28A4315D576032E1BA8093E65B843011EE3660FE897401EB30A5`.

Full EditMode and PlayMode at the same source also passed **140/140** and **81/81**.
This remains deterministic offline evidence; it does not establish cross-machine
floating-point parity, production-bot pacing, or physical full-route QA.

## Current method — 2026-08-23

Validated at local and remote `main` commit `1412802`
(`soak: verify recorded complete replay input streams`).

`Assets/BattleRaja/Tests/EditMode/DeterministicSoakTests.cs` now builds a
complete replay for every seeded match. The header records the fixed 30 Hz step,
Solo Raja scenario, all eight spawns, fighter-specific weapon and movement setup,
factions, health pickups, gadget pickups, and arena version. Every frame records
the ordered movement, attack, ability, Maya-decoy context, and gadget-use inputs.

The soak covers Bijli, Pehel, Maya, canonical attacks and projectiles, health
pickup collection, Dhol Burst, Umbrella Guard, Tiffin Station, Pehel charge,
Maya decoys, Aandhi phases, elimination, and terminal resolution. For every seed
it first executes the generated stream without an expected hash file to capture
the authoritative hashes, writes those hashes into the replay, then executes the
exact recorded stream again with full hash verification.

`DeterministicReplayHasher.CalculateTickHash` delegates to the authority's
canonical digest. It includes tick/phase/zone state, participant statistics and
positions, movement motors, attack cooldowns and sequences, weapon configuration,
gadget inventories/cooldowns, guards, charge runtimes, decoys, stations, pickup
availability and timers, identity counters, projectiles, match end, and winner.

## Deep-soak result — 2026-08-23

- Source: `1412802`, clean except the owner-protected scene/prompt working-tree files.
- Command environment: `BATTLERAJA_SOAK_MATCHES=1000`.
- Command filter:
  `BattleRaja.Tests.EditMode.DeterministicSoakTests.AcceleratedSeededMatchesReproduceIdenticalHashStreams`.
- Result: **passed**; 1 test / 1 passed / 0 failed / 0 skipped.
- Execution scope: **1,000 seeded matches x 2 executions = 2,000 full matches**.
- Recorded-replay divergence: **zero**.
- NUnit duration: **416.1411007 seconds**.
- XML evidence:
  `Builds/Local/TestResults/deep-soak-1000.xml`.
- Log evidence:
  `Builds/Local/Logs/deep-soak-1000.log`; Unity reported
  `Test run completed. Exiting with code 0 (Ok). Run completed.`
- Full EditMode at the same source after the deep soak: **125/125 passed**
  (`Builds/Local/TestResults/editmode-head.xml`).

Non-fatal startup log entries included a transient Unity licensing signature/token
message followed by successful Personal entitlement resolution, plus a warning that
an empty Gameplay assembly has no scripts. No C# compilation error or test failure was
present.

## Scope and honest limitations

- Same-machine stream parity does not prove cross-machine floating-point determinism.
- This EditMode soak does not prove memory-leak freedom; repeated-runtime PlayMode checks remain the relevant evidence.
- Replay execution is currently Core/Application test coverage; production presentation capture and durable replay-file serialization are not yet wired.
- Network transport correctness is out of scope for this offline authority soak.
