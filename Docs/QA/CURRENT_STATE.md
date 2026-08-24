# BattleRaja V1 current-state index

Updated: 2026-08-24

## Truthful classification

**Prototype — Android offline release candidate in progress.**

The offline product loop, authority/replay foundation, original procedural/vector
presentation kit, tutorial action gates, settings surfaces and Android packaging
tooling exist. A Play Store Release Candidate claim is not yet justified because
physical Lava interaction/tutorial review, sustained performance, final identity/
signing, accessibility, legal/privacy/cultural approval and Play Console review
remain open.

## Exact current source

- Branch: `codex/v1-playstore-release`
- Current runtime source: `e808830` (`perf: reuse cached actor views for Pehel authority results`)
- Documentation HEAD: `b82aa76` (`docs: record current Android manifest checks`)
- Runtime/validation source: `1d743b0` (`tools: add repeatable Lava performance capture`)
- Latest runtime change: `d96d3f2` (Android lifecycle pause/resume guard)
- Unity: `6000.5.6f1`
- EditMode: **125/125**
- PlayMode: **71/71**
- Repository validation: **0 errors / 0 warnings**
- Git LFS: passed

The current `e808830` change is behavior-preserving presentation cleanup. Its
fresh release-shaped APK/AAB are recorded in the exact-current baseline; the
APK installed and launched on Lava, but the lock screen prevented interaction.
The package table below remains the prior lifecycle candidate for the separate
performance-tool baseline.

## Exact current Android artifacts

Built from `1d743b0` in disposable `C:\BRLifecycle`:

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| APK | 39,486,559 | `89156306717C5EB27EE193AD1D46809DFE19159112ADC3C77008D4C6A3C89DE0` |
| AAB | 35,313,996 | `F5776F6AF19EE1C0A803D76050A80E62E883710E00296EF9603CED279D2227C1` |

The AAB is ARM64-only and passed static 16 KB ELF alignment. The package is
debug-signed with temporary ID `com.example.battleraja.m11`; it is not
publishable. Installation succeeded only on Lava `ST5GDW23LB004392`. The phone
was locked during launch, so no interactive route evidence is attributed to this
candidate.

## Current open gates

- Unlock Lava and perform the real tutorial action sequence, full match, all
  fighters/gadgets, spectator, results, rematch, settings and background/resume.
- Run a sustained match/performance capture with the new
  `Tools/Validation/capture_android_performance.ps1` harness and interpret frame,
  CPU, memory, thermal and battery evidence.
- Refresh store screenshots from the final human-reviewed signed candidate.
- Supply final package identity and release signing; recheck manifest, permissions,
  target API, 64-bit and 16 KB compatibility in the owner-controlled Play flow.
- Complete privacy/Data Safety, content rating, cultural, accessibility and visual
  review; no publication is authorized yet.

## Deliberate non-scope

Photon gameplay, PlayFab, accounts, matchmaking, online progression, Web release,
ads, IAP and public deployment remain outside V1.0.
