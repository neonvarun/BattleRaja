# BattleRaja M11 Closure Report - 2026-08-23

## Status

**Ready with limitations.** The bounded M11 closure slice produced reproducible
performance baselines, browser lifecycle evidence, two concrete UX fixes with
automated regressions, and a transport-free networking-readiness review. The
product remains a **prototype / closed-alpha foundation**. Final-tip Android/Web
rebuilds, physical rematch cycling, and human UX/accessibility approval remain
open because the owner stopped the session for clean publication.

## Repository And Runtime Baseline

- Starting state: local and remote `main` aligned at `9c91f76`; worktree clean;
  both pre-existing stashes preserved untouched.
- Exact-source validation at `9c91f76`: **0 errors / 0 warnings**.
- Runtime UX commits created during this slice:
  - `c11954b` - expose effects volume and preserve terminal results.
  - `5d6eeb8` - hide the completed tutorial overlay over results.
- Tests at `c11954b`: EditMode **125/125**, PlayMode **59/59**.
- Tests after `5d6eeb8`: PlayMode **59/59**. EditMode was not rerun after this
  presentation-only commit.
- The final tip is therefore test-covered at Level 3, but its exact-source
  player builds remain **not run**.

## Performance And Size Evidence

All raw evidence is under ignored
`Builds/Local/M11Closure/2026-08-23/`.

### Android

Baseline APK built from detached exact-source `9c91f76`:

- Path: `Builds/M11/Android/BattleRaja-BazaarBastion-M11-9c91f76.apk`
- Size/hash: **94,253,553 bytes**,
  SHA-256 `7ED0E10DDB2FD1F2D0D2C0E64584AC4BE8840CFBE2262E00C375E311FBEC81EB`
- Lava-only install/launch succeeded; cold-launch report `TotalTime: 480 ms`.

Focused Lava observations (`ST5GDW23LB004392`):

| State | PSS KB | RSS KB | Graphics KB | Swap PSS KB |
| --- | ---: | ---: | ---: | ---: |
| Menu after cold launch | 407,918 | 535,216 | 82,088 | 74 |
| Active movement/attack | 460,776 | 588,808 | 99,436 | 65 |
| Late Aandhi/spectator | 466,716 | 594,800 | 101,484 | 65 |
| Paused near resolution | 452,348 | 580,084 | 95,340 | 67 |

- Battery sample: 28%, USB powered, 38.1 V reported by dumpsys units, temperature
  340 in Android tenths-of-a-degree units (**34.0 C**).
- Two Perfetto captures were retained as `active.pftrace` and `late.pftrace`,
  including frame-timeline categories. No host trace-processor was available, so
  numeric FPS/frame percentiles, CPU/GPU function costs, GC rate, draw calls,
  triangles, texture/mesh/audio memory remain **unmeasured**.
- `dumpsys gfxinfo` again exposed no Unity frame/jank histogram.
- Sampled app logcat had **0 fatal exception, SIGSEGV, SIGABRT, or ANR markers**.
- System gralloc/thermal log noise was present and is not attributed to the game.

### Web

Baseline build from detached exact-source `9c91f76`:

- Path: `Builds/M11/Web-BazaarBastion-9c91f76`
- **19 files / 134,170,348 bytes**
- WASM: **121,427,473 bytes**, SHA-256
  `BB722EC437DE934CDDEEF06D1A594604A46F495BDE14A442C138A3ECAF8B14CB`

Chrome and Edge each passed desktop/tablet/portrait routes with **zero console
errors** and **zero failed requests**. Browser rAF summaries were:

| Browser | Route | Mean range | P95 range | Observed max range |
| --- | --- | ---: | ---: | ---: |
| Chrome | Menu | 5.014-5.029 ms | 5.700-5.900 ms | 9.100-13.800 ms |
| Chrome | Match | 5.012-5.066 ms | 5.900-6.400 ms | 10.100-34.300 ms |
| Edge | Menu | 5.010-5.015 ms | 5.700-5.900 ms | 6.800-11.300 ms |
| Edge | Match | 5.009-5.058 ms | 5.700-6.100 ms | 8.500-23.400 ms |

- Local uncompressed cold transfer totaled **134,069,638 bytes** per route;
  WASM encoded body was **121,427,473 bytes**. Python's local server supplied no
  compression, so production CDN compression remains unmeasured.
- Cached reload avoided the data payload but still fetched WASM: warm transfer
  totaled **121,428,073 bytes** per sampled route.
- Scripted route durations were approximately **70.2-71.6 seconds** including
  fixed loading waits; this is not a production time-to-playable measurement.
- Background-tab suspension and return passed in Chrome and Edge with no console
  errors or failed requests; post-return rAF remained near **5.02 ms mean**.
- Browser JS/WASM heap usage was unavailable through the visited API; only the
  4,395,630,592-byte heap limit was returned.

## UX Defects Found And Fixed

1. **Effects volume had no settings control.**
   State was loaded, saved, summarized, and supported by the audio director, but
   the Bootstrap settings screen exposed only music controls. Added explicit
   effect-volume decrease/increase controls and a PlayMode persistence regression.

2. **Terminal authority state could miss result publication.**
   The match controller returned immediately when simulation was already ended,
   without ensuring `PublishResults()` had run. It now republishes idempotently
   and returns. A direct authority-terminal PlayMode regression covers the HUD.

3. **A completed tutorial overlay could obscure results.**
   A timeout run reached the results panel, but the completed replayable tutorial
   card stayed active above it. Completed state now hides the panel; tutorial
   tests assert hidden/visible transitions.

## Platform Candidate Before Stop

Exact-source `c11954b` development artifacts were rebuilt from a detached
worktree and copied into the main workspace:

| Artifact | Result |
| --- | --- |
| Android APK | `Builds/M11/Android/BattleRaja-BazaarBastion-M11-c11954b.apk`; **97,529,808 bytes**; SHA-256 `B12C2BCD3C749D1D5ABAF01A2E37C71816B9E8B8AE71BEBC9EA8D1744A952502` |
| Web output | `Builds/M11/Web-BazaarBastion-c11954b`; **19 files / 134,170,499 bytes** |
| WASM | **121,427,571 bytes**; SHA-256 `8D6B5673D598D881FF62A3B45AF24A18828BB75497BC757CF802003FF97F31EE` |

The final APK was installed and launched once on Lava. That smoke exposed the
tutorial/results defect fixed in `5d6eeb8`. The final tip was **not rebuilt or
reinstalled**, so no final-tip platform pass is claimed.

## Remaining Gates

- Rebuild Android/Web at `5d6eeb8`, reinstall only on Lava, repeat home/resume
  and results/rematch interaction, and run Chrome/Edge six-route smoke.
- Run at least 20 consecutive physical rematch/load cycles and record memory.
- Capture Unity Profiler CPU/GPU/GC/draw-call/triangle evidence and parse
  Perfetto frame timing with an available trace processor.
- Measure production compressed transfer, CDN cache behavior, cold WAN load,
  mobile browsers, Firefox/Safari, shader warm-up, and release/AAB size.
- Complete visual/audio/accessibility QA and obtain owner/human review.
- Keep Photon/PlayFab, signing, public hosting, analytics/crash services, store
  submission, legal/privacy, branding, and classification promotion gated.

