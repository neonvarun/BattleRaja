# Runtime smoke measurements — 2026-08-03

## Scope

- Runtime source baseline: `42e93e7` (`test: cover production gadget authority route`).
- Documentation commit carrying this note: `e90ad19` (`qa: capture settings and service-error routes`).
- This note records one bounded Android active-match sample and one local Web browser
  sample. It does not close the performance, soak, release, or multi-browser gates.
- No production code was changed to obtain these measurements.

## Android — connected Lava only

| Field | Observation |
| --- | --- |
| Device | `ST5GDW23LB004392`, `LAVA LXX508` |
| Package/process | `com.example.battleraja.m11` |
| Route | Bazaar Bastion active match |
| Sample window | 20 seconds after `dumpsys gfxinfo ... reset` |
| Total PSS | 460,165 KB |
| Total RSS | 597,680 KB |
| Graphics PSS | 101,480 KB |
| Swap | 240 KB |
| Instantaneous `top` CPU sample | 87% |
| `dumpsys cpuinfo` process sample | 50% user / 13% kernel |
| Frame/jank evidence | Not exposed by this `dumpsys gfxinfo` output |

Raw captures (local build artifacts, intentionally not committed):

- `Builds/M11/Logs/android-lava-performance-20260803-meminfo.txt`
- `Builds/M11/Logs/android-lava-performance-20260803-gfxinfo.txt`
- `Builds/M11/Logs/android-lava-performance-20260803-top.txt`
- `Builds/M11/Logs/android-lava-performance-20260803-cpu.txt`

Interpretation: the app remained alive on the connected Lava device during the sample,
but the available output is insufficient for an FPS, frame-time, GPU, allocation-rate,
thermal, battery, or repeated-match-growth claim.

## Web — Chrome 150

| Field | Observation |
| --- | --- |
| Browser/target | Chrome 150, local `http://127.0.0.1:8139/index.html` |
| Viewport | 1280 × 720 |
| Warm-up | 8 seconds |
| WASM transfer | 120,872,306 bytes |
| WASM decoded body | 120,872,006 bytes |
| WASM transfer duration | 1,058.9 ms |
| DOMContentLoaded | 17.4 ms |
| Load | 62.5 ms |
| 120-sample rAF mean / p50 / p95 / max | 5.603 / 5.5 / 6.1 / 6.1 ms |
| JS heap used / total | 58,307,579 / 64,410,431 bytes |
| JS heap limit | 4,395,630,592 bytes |
| Console | 0 errors / 0 warnings |

These are browser timing and heap observations, not Unity FPS, GPU, GC, cold-CDN,
mobile-Web, or cross-browser evidence.

## Commands and boundaries

- Android evidence was collected with `adb` against the Lava serial above only.
- Web evidence was collected with the Playwright CLI against the existing local server
  on port 8139; the server was not stopped as part of this task.
- Remaining closure work includes Unity Profiler CPU/GPU/GC/draw-call capture, Android
  frame pacing and thermal/battery soak, repeated-match memory growth, Web cold-load,
  mobile-browser and second-browser coverage, shader warm-up, cache/compression and
  network-bandwidth measurements.
