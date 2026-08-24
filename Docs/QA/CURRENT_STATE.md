# BattleRaja V1 current-state index

Updated: 2026-08-24

## Evidence location policy

All new builds, logs, screenshots and test reports must remain under the ignored
`Builds/Local/` tree inside `C:\Projects\BattleRaja`. Historical append-only
notes may mention retired disposable paths for provenance, but no new work should
create or depend on evidence outside this repository root.

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
- Current branch tip is `73f20a2` (`accessibility: propagate reduced flash mode`),
  after the exact Android artifact source `edbf4d0`.
- Exact Android candidate source: `edbf4d0`
- Current runtime-bearing source: `73f20a2` (`accessibility: propagate reduced flash mode`)
- Documentation evidence anchor: the exact-source sections in this index and
  `Docs/QA/LATEST_HEAD_BASELINE.md`, updated with the release-flags candidate.
- The last artifact validation source is `edbf4d0`; current presentation fixes
  are in `73f20a2` and need a fresh package rebuild before release attribution.
- Unity: `6000.5.6f1`
- EditMode: **125/125**
- PlayMode: **73/73**
- Repository validation: **0 errors / 0 warnings**
- Git LFS: passed

The post-candidate presentation fixes keep the runtime match HUD inside the
gameplay safe area and propagate reduced-flash settings to combat impact, hit and
Aandhi feedback. Current tests are green, but the fresh release-shaped APK/AAB
below were built from `edbf4d0` before these two presentation commits. The APK
installed and launched on Lava `ST5GDW23LB004392`; the portrait menu is captured
at `Builds/Local/V1Evidence/edbf4d0/Android/lava-launch.png`, with foreground
window evidence in `lava-window-state.txt`. A new exact-HEAD Android rebuild is
required before attributing release evidence to `73f20a2`.

## Prior Android artifacts used for the performance-tool baseline

Built from `1d743b0`; the retired disposable checkout is not part of the current
workspace and its raw package files are not retained as current evidence:

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

- Perform the real tutorial action sequence, full match, all
  fighters/gadgets, spectator, results, rematch, settings and background/resume.
- Rebuild the Android APK/AAB from current HEAD `73f20a2` before release review;
  the archived artifact pair is from pre-fix source `edbf4d0`.
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
