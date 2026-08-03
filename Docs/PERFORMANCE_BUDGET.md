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

## Milestone 2 baseline evidence — 2026-08-02

- Editor: 15 EditMode and 13 PlayMode tests passed.
- Android: M2 development IL2CPP ARM64 APK built at approximately 83.0 MB on disk;
  Unity's build report serialized approximately 1.22 GB before packaging. The APK
  installed/launched on Lava API 34 and Oppo API 36; no formal CPU/GPU/GC capture was
  collected.
- Web: M2 WebGL2/WebAssembly build succeeded with approximately 113.0 MB reported
  build output; local Chrome/Edge DOM bootstrap checks passed. No frame-time,
  WebAssembly-memory or cached-load capture was collected.
- Combat projectiles and impact feedback are bounded pools; cooldown, travel and
  damage rules do not allocate per frame. This is an implementation constraint, not
  a measured performance claim.

## Milestone 3 baseline evidence — 2026-08-02

- Bijli dash uses value-type domain state and a bounded `TrailRenderer`; no runtime
  Instantiate/Destroy or per-frame managed collection is introduced by the fighter
  controller or HUD.
- M3 development APK is approximately 83.1 MB on disk; the Web output is approximately
  113 MB as reported by Unity. These are development-build baselines, not release
  budgets.
- No formal Editor Profiler, Android GPU/CPU capture, browser memory capture or device
  frame-time profile was collected. Dash feel, HUD readability and low-end-device
  performance remain human/measurement debt.

## Milestone 4 baseline evidence — 2026-08-02

- Seven-bot PlayMode stress measured 91 decisions across 2 seconds with a maximum
  decision duration of 0.024 ms in the headless editor run. Decisions are interval-
  scheduled; movement commands continue each rendered frame and use no per-frame
  collection construction in the bot bridge.
- M4 development APK is approximately 83.2 MB on disk; the Web output is approximately
  113.2 MB locally. These development artifacts are not release budgets.
- No Lava/Oppo CPU/GPU/GC capture, browser frame-time/memory capture or low-end-device
  seven-bot profile was collected. Treat the stress number as a repeatable code-path
  baseline, not a mobile performance claim.

## Milestone 5 baseline evidence — 2026-08-02

- The offline match simulation uses bounded eight-participant state and an explicit
  298-second phase definition. Twenty accelerated matches completed in EditMode; this
  is correctness/soak evidence, not wall-clock performance evidence.
- M5 development APK/Web artifact sizes and device/browser smoke are recorded in the
  M5 report. No Android CPU/GPU/GC, browser frame-time/memory or repeated-runtime
  object-count capture was collected.

## Milestone 6 baseline evidence — 2026-08-02

- M6 development APK is approximately 83.4 MB on disk; the Web output is approximately
  113.5 MB locally. These are development artifacts, not release budgets.
- Gadget inventory/runtime state is bounded to one held item per actor. Tiffin healing
  scans active health components once per interval; this is acceptable for eight actors
  but is not a scale claim.
- No Android CPU/GPU/GC, browser frame-time/memory, or device-tier gadget profile was
  captured. Human telegraph/readability and bot-use value remain unmeasured.

## Milestone 7 baseline evidence — 2026-08-02

- M7 development APK is approximately 83.6 MB on disk; Web output is approximately
  113.5 MB locally. These are development artifacts, not release budgets.
- The three-fighter actor mix remains eight actors with bounded gadget state and pooled
  projectile effects. No formal Android CPU/GPU/GC or browser frame-time/memory capture
  was collected.
- Final art/audio overdraw, tutorial/menu UI, accessibility settings and low-end device
  stability remain unmeasured and are tracked in HR-007.

## Milestone 8 networking baseline evidence — 2026-08-02

- The deterministic network mock uses bounded two-client state, one input frame per
  submitted tick and explicit diagnostics counters. Its packet-loss profiles are a
  correctness harness, not a transport or frame-time claim.
- No Photon transport, prediction/reconciliation, interpolation, reconnect, Android
  radio profile or Web memory/frame-time capture was collected because the external Fusion
  package/App ID gate is unavailable.
- When unblocked, capture authoritative tick drift, input/snapshot rate, reconnect time,
  packet loss/jitter behavior and device/browser CPU/GC/memory on Lava plus desktop Chrome
  and Edge before calling the online budget established.

## Milestone 9 preparation baseline — 2026-08-02

- `AuthoritativeMatchServer` uses bounded eight-slot dictionaries and a pure offline match
  snapshot; local proof tests measure correctness only, not server throughput or bandwidth.
- No headless server process, network packet rate, Android radio usage, browser lifecycle,
  reconnect latency or eight-slot CPU/memory capture exists. These remain blocked with the
  M8 real-session precondition.

## Milestone 10 progression baseline — 2026-08-02

- The fake backend uses bounded in-memory accounts, identity links, reward keys, cosmetics
  and leaderboard entries. Tests establish correctness only; they are not PlayFab latency,
  quota, storage, bandwidth or cost measurements.
- Real account/login, cache/cloud conflict, browser-storage recovery, service retries and
  Android/Web memory/network captures remain unmeasured until the approved backend exists.

## Milestone 11 candidate baseline — 2026-08-02

- M11 development APK is 83,561,850 bytes on disk; Web output is 113,899,365 bytes
  uncompressed. These are development baselines, not store/download budgets.
- Local smoke covers Lava, Chrome 150 and Edge 150 only. No formal CPU/GPU/GC/memory,
  thermal/battery, repeat-cache, Firefox/Safari/mobile-Web or eight-slot online profile was
  collected.
- Before distribution, capture cold/warm load, repeated-match object growth, frame-time and
  memory on approved device/browser tiers and document Web compression/MIME/cache headers.

## Phase 6 smoke measurement snapshot — 2026-08-03 (`8544f55`)

- Lava `LAVA LXX508` (`ST5GDW23LB004392`) was left in the live development match for
  approximately 20 seconds. `adb shell dumpsys meminfo` reported **507,397 KB total PSS**,
  **644,576 KB total RSS**, and **97,884 KB Graphics PSS**. These are development-player
  observations, not a release memory budget.
- Lava `dumpsys gfxinfo` exposed the Unity `ViewRootImpl` but emitted no frame/jank
  histogram for this surface, so FPS, frame time, GPU time and GC allocation rate remain
  unmeasured. The raw captures are `Builds/M11/Logs/phase6-lava-gfxinfo-20260803.txt`
  and `Builds/M11/Logs/phase6-lava-meminfo-20260803.txt`.
- Chrome/Playwright loaded the local Web candidate with a canvas present. The observed
  navigation timing was DOMContentLoaded **11.7 ms**, load **203 ms**; the WASM resource
  reported **120,502,144 bytes transferred** and **274.8 ms** resource duration. Chromium
  exposed **30,464,015 bytes used JS heap** of **39,391,851 bytes total** (heap limit
  4,395,630,592 bytes). This was a local smoke run with existing browser caching and is
  not a cold-download, WebAssembly peak, frame-time or multi-browser budget.
- No optimization or release readiness claim follows from this snapshot. Formal Unity
  Profiler/Android GPU/CPU/GC capture, repeated-match growth, thermal/battery, Web frame
  pacing and multi-browser measurements remain required.
