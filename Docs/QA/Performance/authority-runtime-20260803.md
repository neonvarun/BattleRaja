# Authority candidate runtime measurement — 2026-08-03

Candidate: `044b1b8` / production `Builds/M11/Web-BazaarBastion` and
`Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk`.

## Chrome 150 local Web observation

- URL: `http://127.0.0.1:8139/index.html`
- Warm-up: page reload followed by 8 seconds before measurement.
- Unity canvas count: 1.
- Navigation timing: DOMContentLoaded 152 ms; load 251.4 ms.
- WASM resource: `Web-BazaarBastion.wasm`, transfer size 120,662,567 bytes,
  resource duration 1,903.5 ms.
- 120 `requestAnimationFrame` samples: mean 5.620 ms, p50 5.5 ms, p95 6.1 ms,
  maximum 6.1 ms. These are browser callback intervals, not Unity FPS, GPU time,
  or frame pacing proof.
- `performance.memory`: used JS heap 30,296,011 bytes; total JS heap
  31,053,339 bytes; browser heap limit 4,395,630,592 bytes.
- Console after the automated reload: 0 errors and 14 warnings. The warnings were
  repeated Chrome autoplay `AudioContext` policy messages; the context later resumed
  after the documented user-gesture path. This is a browser policy observation, not a
  claim that audio UX is approved.

## Lava `LAVA LXX508` observation

- Serial: `ST5GDW23LB004392`; no other device was used.
- Process: `com.example.battleraja.m11`, PID 30158.
- `dumpsys meminfo`: total PSS 458,974 KB; total RSS 596,588 KB; Graphics PSS
  95,468 KB; swap 100 KB.
- `dumpsys gfxinfo` exposed the Unity ViewRoot and 9 views but no frame/jank
  histogram, so no Android FPS, frame-time, GPU or GC-rate claim is made.
- A one-shot `top` sample recorded the process alive; it is not a sustained CPU or
  thermal measurement.

Raw captures:

- `lava-authority-meminfo-20260803.txt`
- `lava-authority-gfxinfo-20260803.txt`
- `lava-authority-top-20260803.txt`

## Gate interpretation

This is a repeatable smoke measurement and establishes concrete baselines for future
comparison. It does not establish release budgets, low-end-device performance,
thermal/battery behavior, repeated-match memory stability, Web cold-load performance,
multi-browser parity, Unity Profiler CPU/GPU/GC data, or human approval.
