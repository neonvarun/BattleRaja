# 13 — Performance, Memory, Rendering and Android Optimization

## Context

The current candidate runs on Lava and has bounded telemetry, but the latest report lacks a normalized frame histogram and repeated-match/endurance proof. The visual assets are about to become denser, and Bastion Crown adds eight actors, team UI, objective VFX and respawns. This stage profiles the exact final-art Android candidate before release claims.

## Objective

Deliver a stable, comfortable 4v4 game on ordinary Android hardware, with responsive input, predictable frame pacing, bounded memory/GC, controlled thermal/battery cost and a build that is compatible with current Play requirements. Richness must degrade gracefully without removing gameplay telegraphs.

## Current-state audit

Inspect Unity quality tiers, URP settings, shader variants, materials/textures/meshes/VFX/audio, pooling, allocations, scene loading, addressable/resources policy, Android Gradle/IL2CPP/NDK settings, package permissions, architecture splits and current APK/AAB reports. Re-run a cold install and collect baseline on Lava `ST5GDW23LB004392`; do not reuse unscoped desktop or AVD numbers as physical proof.

## Preserve

Preserve gameplay determinism, authority timing, critical VFX/UI telegraphs, ARM64 IL2CPP path, safe lifecycle behavior, offline/no-network scope and healthy pooling/build scripts. Preserve Web/network seams without spending V1 performance work on an untested Web release.

## Replace/fix

Remove per-frame allocations/searches, duplicate canvases, unbounded particles/audio, shader/texture waste, scene-load hitches, memory growth between rematches, UI rebuild churn, unnecessary permissions/services and any optimization that hides Crown/team/Aandhi/ability state.

## Implementation tasks

1. Define and publish measurement protocol: device/model/API/build/hash, orientation, quality tier, battery/thermal state, warm-up, capture duration, match seed, profiler source and aggregation (average, p50, p95, p99, worst frame).
2. Profile the final 4v4 art candidate through cold launch, menu, tutorial, live combat, Crown contest/deposit, all gadgets, Aandhi, KO/respawn, results and ten rematches. Capture CPU main/render, GPU where available, frame pacing, GC, managed/native/graphics memory, draw calls, triangles, shader variants, audio voices, load time, temperature and battery.
3. Use a practical target budget: 60 FPS where the Lava device sustains it; p95 frame ≤20 ms, p99 ≤33 ms, no unexplained >1 s stall; no persistent growth >10% across ten identical rematches; no GC spike >50 ms; PSS target ≤450 MB unless a measured device limit requires a documented adjustment. Treat these as gates to validate, not numbers to fake.
4. Optimize in measured order: pooling/allocations/searches; canvas rebuilds; mesh/material/texture/LOD/atlas; VFX/overdraw/shadows/lights; audio voices/streaming; load/unload and scene lifetime. Record before/after evidence and avoid speculative rewrites.
5. Create quality tiers for low/mid/high that preserve actor silhouettes, Crown/shrine/ticket/team markers, attack/ability telegraphs and Aandhi. Test the actual phone tier rather than assuming it.
6. Validate Android packaging: target API 36 or newer as currently required, 64-bit native libraries, 16 KB page-size readiness, IL2CPP/NDK toolchain, zip alignment, no unnecessary network permissions/services and no debug/development symbols in release. Re-check current official requirements at execution time.
7. Produce release APK for local QA and AAB for Play validation; keep signing/package identity owner-gated.

## Asset tasks

Create measured LOD/atlas/texture-compression profiles, low-quality VFX/material fallbacks, icon/font atlases, audio compression/streaming variants and an asset budget manifest. Do not remove a critical telegraph to meet a number; redesign/LOD/pool it instead.

## Integration points

Integrate with all final fighter/map/gadget/VFX/UI/audio/tutorial assets, Unity quality settings, Android build scripts, profiler capture, lifecycle, local preferences and release checklist. Keep performance instrumentation out of the public release UI.

## Performance constraints

Use the targets above as the minimum evidence plan. Report device limitations separately from app faults. Keep hot loops allocation-free, pool repeated transient objects, cap voices/particles, avoid runtime material clones, keep loading under 8 seconds to first playable target where measured, and document any justified deviation.

## Tests

Add static/build checks for development flags, permissions, target/min SDK, ARM64, resources/shaders, duplicate assets and package metadata. Add performance smoke/soak tests, ten-rematch memory test, lifecycle test, quality-tier telegraph test, AAB bundle validation and deterministic replay after optimization. Run full EditMode/PlayMode/replay suites.

## Visual QA

Inspect low/mid/high tiers in busy 4v4: team markers, Crown, shrine channel, ticket/respawn, ability/gadget and Aandhi cues must remain readable; no LOD/shader/texture pop, UI hitch or camera discomfort. Compare before/after on the same device and settings.

## Lava verification

Use Lava `ST5GDW23LB004392` only for physical performance evidence. Install exact release candidate, run the protocol and repeated matches in airplane mode, collect frame/memory/thermal/battery logs, background/resume behavior and 4 KB page fact. Do not claim physical 16 KB compatibility from this phone; use an actual 16 KB-capable physical/approved environment or document the blocker. Never use Oppo.

## Failure cases

Test low battery/thermal state, background/foreground, orientation/layout change, low-memory pressure, audio route change, quality-tier switch, shader warm-up, missing compressed asset, AAB install, 4 KB/16 KB validation failure, permission regression, rematch leak, pool exhaustion and profiler disconnect.

## Binary acceptance gate

Pass only when final-art 4v4 is measured with the stated protocol, targets are met or deviations are justified, no repeated-match growth/crash/thermal issue remains, all quality tiers preserve telegraphs, Android build checks pass, AAB validates and physical 16 KB status is proven or honestly blocked. Bounded old telemetry is not enough.

## Evidence to retain

Profiler captures/raw summaries, frame histogram, CPU/GPU/GC/memory/draw/texture/VFX/audio tables, ten-match growth chart, thermal/battery log, quality-tier screenshots, APK/AAB hashes, bundle/zipalign/ABI/16 KB reports and exact device/build/settings.

## Non-scope

Do not add network/multiplayer, change gameplay for a benchmark without balance evidence, remove critical feedback, purchase infrastructure or finalize signing/upload/legal declarations.

## Stop condition

Stop before prompt 14 if the final-art build misses the frame/memory/thermal gate without a mitigation, grows across rematches, crashes under lifecycle/low-memory, hides telegraphs on low tier, or packaging/16 KB status is unknown.
