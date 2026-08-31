# BattleRaja Replay and Soak Report

## Current Bastion Crown replay result — 2026-09-01

`Assets/BattleRaja/Tests/EditMode/BastionReplaySoakTests.cs` builds canonical eight-actor
Bastion streams with fighter abilities, gadgets, Crown interactions and Aandhi context.
Two seeds were executed for 8,400 fixed ticks each, serialized through replay envelope v2,
and re-executed with combined legacy-plus-Bastion hash verification: **zero divergences**.
The full current gates are **155/155 EditMode** and **94/94 PlayMode**. The exact XML/logs
are under `Builds/Local/V1GameplayTruth/Final/`; the broader production-bot 100-match
evidence below remains Solo-only and is not blended with this Bastion result.

## Latest durable production replay result — source `2a113e0` — 2026-08-27

The development-only production bot harness now persists ordered authority inputs,
per-tick participant snapshots and canonical hashes in a checksummed Unity-independent
`.brr` file. One complete production-scene match passed **86/86 PlayMode** and emitted
`Builds/Local/V1GameplayTruth/ProductionBotReports/Replays/match-9101-20260827-160257598.brr`
(5,802,977 bytes; SHA-256
`48C0DC38A417934331245FBB28B8EE15589502C23E93619EC688310C1E487736`; 9,180 frames).
The exact file was read and fully re-executed with per-tick snapshot/hash verification in
**141/141 EditMode**. The current source also passed the 100-seed production-bot release
batch and reproduced the same seed-9101 command/replay digest across two independent runs.
P42 in `Docs/V1_RELEASE_PLAN.md` records the matching report, test/log hashes, format
decision and remaining cross-machine/cosmetic-review limits.

## Latest current-tip result — runtime `754837e` / clean docs `98888d3` — 2026-08-27

The current runtime-bearing candidate is presentation-only relative to the deterministic
gameplay core. From clean documentation tip `98888d3`, Unity `6000.5.6f1` ran
`BATTLERAJA_SOAK_MATCHES=1000` with the filtered
`BattleRaja.Tests.EditMode.DeterministicSoakTests.AcceleratedSeededMatchesReproduceIdenticalHashStreams`
fixture. The result was **1/1 passed**: 1,000 seeded matches executed twice (2,000
executions), zero divergence, NUnit duration **536.0635271 seconds**. XML
`Builds/Local/TestResults/deep-soak-current-98888d3.xml` has SHA-256
`07DADE0702BD7B5DEC9A11E60042D66778A42344CBB33526D72073D6D8DFF4C6`; Unity log
`Builds/Local/Logs/deep-soak-current-98888d3.log` has SHA-256
`A2CC52C19961FFAAC139D68A2FF591683A5AC495F26C914A1638D101AA6D5C97`. The worktree
was clean after the run. This is same-machine deterministic evidence only; it does
not establish cross-machine floating-point parity. Durable production replay-file
serialization is now covered by the bounded P42 production capture/re-execution evidence.
The exact-runtime production bot batch remains recorded in P38.

## Latest exact-source result — `8edc086` — 2026-08-27

At exact source `8edc0867268800f0ad81067378ad590e1a166371`, the fighter-focus presentation
patch leaves gameplay code unchanged, so the preceding exact gameplay source remains the
applicable soak source. At exact source `6d287a657dd946c806ac54580b4d5a5ea1e53ee4`,
`BATTLERAJA_SOAK_MATCHES=1000` passed **1/1**: 1,000 seeded matches executed twice
(2,000 executions), zero divergence, NUnit duration **538.822974 seconds**. XML
`Builds/Local/V1GameplayTruth/TestResults/deep-soak-final-6d287a6.xml` SHA-256
`DB133AE5BD7855175FECA4ED909F0C67FCE4F9607C98A4FE355683B029122186`; log SHA-256
`C3BBAEB98E9C5EA3F3C88B97B0C75953FD1CADC7F817E9FE87A470D9017D4D97`.

Full EditMode and PlayMode at the same source also passed **140/140** and **81/81**.
The exact-source fixed-tick production-bot batch separately completed 100/100 matches
in-window with 95/100 reaching at least three combat eliminations; a same-seed rerun
reproduced its command digest and command count. This remains deterministic offline
evidence; it does not establish cross-machine floating-point parity, physical full-route
QA or sustained full-match performance.

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
