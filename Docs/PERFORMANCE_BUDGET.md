# Android and Web Performance Budget

**Status:** Unmeasured hypotheses only until device evidence exists.

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
