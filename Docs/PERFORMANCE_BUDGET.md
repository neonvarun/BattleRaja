# Android and Web Performance Budget

**Status:** M1 movement path has been implemented with allocation-avoidance constraints, but formal frame-time/profiler budgets remain unmeasured.

## Target classes

- Low tier: 30 FPS
- Mid tier: stable 60 FPS where feasible
- High tier: optional higher-refresh experiments after stability

## Required measured budgets

- Main thread
- Render thread
- GPU
- GC allocations
- Draw calls
- Triangle/vertex count
- Texture, mesh and audio memory
- Peak process memory
- Scene/match load time
- Network bandwidth
- APK/AAB and downloadable asset size

Never claim optimisation without profiling evidence.

## Web-specific measured budgets

- Compressed initial download
- Time to first interaction
- Time to first playable match
- Peak WebAssembly memory
- Browser frame time by browser
- Cached repeat-load time
- Shader warm-up
- CDN compression/MIME/cache-header validation

Report Android and Web results separately.

## Milestone 1 baseline evidence — 2026-08-02

- Editor: 8 EditMode and 7 PlayMode tests passed; no profiler capture was collected.
- Android: M1 development APK built at approximately 82.9 MB on disk, installed/launched on Lava API 34 and Oppo API 36; no frame-time or GPU capture was collected.
- Web: M1 WebGL build completed at approximately 112.8 MB uncompressed; Chrome/Edge local HTTP bootstrap checks passed; no browser frame-time or WebAssembly memory capture was collected.
- Movement hot paths use cached references, value-type command/state data, no per-frame LINQ, no repeated component searches and no per-frame managed collection construction by design. This is an implementation constraint, not a measured optimization claim.

Required next measurement: capture a short Editor, Lava and Oppo session with Unity Profiler/Android profiling, plus Chrome and Edge frame/memory observations, before setting numeric M1 budgets or claiming performance readiness.
