# Android and Web Performance Budget

## Hot-path lookup cleanup — `e808830` — 2026-08-24

Static inspection of the authority-driven Pehel path found a scene-wide
`FindObjectsByType<CombatTarget>` scan in the per-tick result adapter. The
adapter now uses the eight-actor cache already owned by
`OfflineMatchController`; no gameplay or authority rule changed. EditMode
125/125, PlayMode 71/71 and repository validation 0/0 pass for this source.
No before/after device frame or allocation measurement is claimed yet; the
repeatable Lava capture remains blocked by the locked device and must be rerun
before any performance budget conclusion.

## Exact current source and capture tooling — docs `be0c510` / source `1d743b0` / runtime `d96d3f2` — 2026-08-24

The documentation HEAD is `be0c510`; the measured runtime/validation source is
`1d743b0`. It has full EditMode **125/125** and PlayMode **71/71**. Fresh
exact-source Android packages built in disposable worktree `C:\BRLifecycle` are
an APK of **39,486,559 bytes** (SHA-256
`89156306717C5EB27EE193AD1D46809DFE19159112ADC3C77008D4C6A3C89DE0`) and an AAB
of **35,313,996 bytes** (SHA-256
`F5776F6AF19EE1C0A803D76050A80E62E883710E00296EF9603CED279D2227C1`). The AAB
is ARM64-only and passed static 16 KB alignment. The APK installed only on Lava
`ST5GDW23LB004392`; interactive performance sampling is blocked while the
owner's device lock screen is active. No sustained CPU, GPU, GC, frame-pacing,
thermal, battery or repeated-match pass is claimed.

`Tools/Validation/capture_android_performance.ps1` now provides a repeatable
Lava-only sample protocol. It records device/package identity, activity,
per-sample `meminfo`, `gfxinfo`, `cpuinfo`, `top`, thermal, battery, activity and
logcat output, plus a manifest and configured fatal-marker scan. A one-sample
non-launch smoke capture was recorded outside source under
`Builds/Local/Device/Performance/script-smoke`; it is tooling validation, not a
product performance result.

## Exact current release-shaped package — `062066b` — 2026-08-24

The exact checked-out HEAD produced a local release-shaped APK of **39,482,035
bytes** (SHA-256
`C7B16D01DEA3ED3ADA1B5E5AA421B82ADBA46F5E1A0A2B0283F409BC59F3E245`) and AAB
of **35,309,464 bytes** (SHA-256
`4D3948F876580AC45A0655593DAA6FE4AF70BC9BACF78840F33EC63E8775E858`). The AAB
is ARM64-only and passed the static 16 KB ELF LOAD alignment check. These are
artifact-size observations only; no sustained performance pass is claimed.

## Exact current lifecycle follow-up — `53da0f3` — 2026-08-24

HEAD is a cleanup-only follow-up to the visual slice at `a5597f5`: the generated
ground mesh is explicitly released on teardown. No new artifact or performance
sample was needed; the Lava smoke sample below remains attributed to the
runtime-equivalent parent. Formal sustained Android/Web performance gates are
still open.

## Exact current device sample — `a5597f5` — 2026-08-24

The exact current visual slice was exercised in the offline match on Lava
`ST5GDW23LB004392` using the development APK from `a5597f5`. A single shell
sample recorded **434,681 KB PSS**, **567,248 KB RSS**, **79,244 KB Graphics**,
and **69 KB swap**. `top` observed **113% instantaneous process CPU** on the
device-wide 800% scale. Thermal status was **0**; current HAL readings were
approximately **46.84 C CPU/GPU**, **41.07 C skin**, and **36.0 C battery**.
`dumpsys gfxinfo` exposed the Unity ViewRoot/render nodes but no usable frame
histogram. Raw output is outside the repository at
`C:\Users\USER\AppData\Local\Temp\battleraja-a5597f5-perf.txt`.

Interpretation: this confirms the new visual slice is alive on the approved
device, but it is only a one-point smoke sample. It does **not** establish
stable frame pacing, CPU/GPU/GC/draw-call budgets, repeated-match memory
growth, thermal/battery endurance, or a low-end-device pass. Those release
gates remain open.

## Exact current source update — `f463b1b` — 2026-08-24

The current source is `f463b1b`. A local Android release-shaped APK built from
this exact source is **39,473,715 bytes** with SHA-256
`ADFE38B3C11DE2119D7180967C48165682095C4818F9FD0140FB694F1198A666`.
The attempted exact-source Web build did not complete: Unity's Web Bee/Burst
backend repeatedly returned exit code 4 and no finished player was emitted. No
new performance measurements are claimed; existing platform measurements below
remain historical unless their source is explicitly named.

## Exact current source update — `ecdb25b` — 2026-08-24

The current source is `ecdb25b`. A local Android release-shaped APK built from
this exact source is **39,465,083 bytes** with SHA-256
`7BDEA277C28CED29367CD3A76A73DCC7DFF45EDBBCAD1F9F9415C64FA7B57AD4`. This is a
focused presentation-gating build only; it was not installed on Lava, and no
new frame-time, GC, GPU, memory, thermal, battery, WebAssembly or browser
performance measurements were taken. Existing measurements below remain
historical unless their source is explicitly named.

## Exact current source update — `fe80582` — 2026-08-24

The current exact source is `fe80582efc35368a03afea314d53e071ac1872bf`. Its
release-shaped APK is **39,465,411 bytes** with SHA-256
`BC74E8C09C853AB8EBE089B0B6F5C063D47A9963F2F940EF48D657FFADAB3E23`; it was
installed only on Lava `ST5GDW23LB004392` and reached the portrait offline menu.
This is a launch/artifact smoke observation, not a performance pass. No current
source AAB, sustained frame-time/GC/GPU sample, repeated-match memory soak,
thermal/battery run or low-end-device result exists for this commit. The prior
device measurements below remain historical and must not be attributed to
`fe80582`.

**Status:** Android evidence is current for the exact V1 presentation candidate, but formal
frame-time/profiler budgets and human performance approval remain open.

## Exact-current V1 match sample — 2026-08-24

The exact current release-shaped APK/AAB source is `dff3a89`. The APK is **39,466,543
bytes** (SHA-256
`A6760651223052BEFB426DA08F5434ED71922A3FF9309336C1827945474F4A91`) and the AAB is
**35,293,988 bytes** (SHA-256
`567EF167654BC53A1836035297385278E2673411C7BD06A6257E550737E3CBF4`). The AAB has
7 ARM64 libraries, no other ABIs, and all checked ELF LOAD segments aligned to
`0x4000`.

The APK was installed and exercised only on Lava `ST5GDW23LB004392`. After a
bounded diagnostic route to the opening offline match, a 20-second device sample
recorded **257,811 KB PSS**, **393,914 KB RSS**, **78,562 KB Graphics**, and **88 KB
swap**. `top` sampled the Unity process at **93.3% instantaneous CPU** (device-wide
800% scale); this is not a sustained CPU budget. Current HAL temperatures were
**44.47 C CPU/GPU**, **39.51 C skin**, and **35.0 C battery**, with thermal status 0.
`dumpsys gfxinfo` exposed the Unity ViewRoot and render nodes but no frame histogram,
so no stable-FPS or frame-pacing claim is made. Raw files and the inspected match
capture are outside the repository at
`C:\Users\USER\AppData\Local\Temp\battleraja-profile-dff3a89\`.

Interpretation: the release-shaped offline match is alive on the approved device,
but formal CPU/GPU/GC/draw-call profiling, repeated-match memory growth, thermal and
battery soak, and human performance approval remain open.

## Exact offline packaging observation — 2026-08-24

The runtime/package source `f4425d6` (final documentation checkout `HEAD`) produced a fresh
offline release-shaped APK of **39,529,326 bytes** and AAB of **35,357,477 bytes**.
The AAB is ARM64-only and passed the static 16 KB alignment check. The exact APK
launched on Lava `ST5GDW23LB004392`; a bounded menu sample recorded no fatal/ANR/SIGSEGV
markers. This reduces the package artifact from the earlier ~40 MB candidate, but it
does not establish a memory, CPU, GPU, GC, frame-pacing, thermal, battery or repeated-
match budget.

The same checkout produced a successful Web build with a current WASM of
119,799,945 bytes (SHA-256
`05EF2D0A69EE3E6DD8B7552913E892D749266135F216F17061560FAFDA8BD09F`). Local HTTP
returned 200 and Edge headless reached the Unity loader. This is a build/loader smoke
measurement only; cold/warm interactive load, compression/cache headers, WebAssembly
memory and full route performance remain open.

## Exact current checkout sample — 2026-08-24

The current branch tip is `357dfdf1e6289c172dab60e514f555ba3d5bc914`; its runtime content
is documentation-equivalent to `46724ac2dfa403f40f58669240e61918c2a94d1b`, from which
the release-shaped APK/AAB were rebuilt in disposable copy
`C:\Projects\BattleRaja-v1-final-verify` and the APK was installed only on Lava
`ST5GDW23LB004392`.

- APK: **40,431,927 bytes**, SHA-256
  `0694958A43F1BADD30E697095F249733992F9D6904E10E1923CD0CAF01010C78`.
- AAB: **36,262,036 bytes**, SHA-256
  `906D85FA00E4A9787A0C1DE892DC3F27A098ACF21BB1735E08C977565A1D09A4`.
- AAB: 8 ARM64 native libraries, no other ABIs, and static 16 KB alignment passed.
- Lava bounded sample: **257,340 KB PSS**, **393,462 KB RSS**, **83,862 KB Graphics**,
  **83 KB swap**. SurfaceView log windows reported approximately **59.45–60.59 FPS**;
  this is a short compositor observation, not a frame-pacing pass.
- No fatal/ANR/SIGSEGV marker appeared in the post-launch log capture.

Raw files: `C:\Users\USER\AppData\Local\Temp\battleraja-final-head-lava\`.
Formal CPU/GPU/GC/draw-call profiling, sustained thermal/battery runs, repeated-match
memory growth and low-end-device evidence remain open.

## Latest Android V1 bounded observation — 2026-08-24

The tutorial-completion correction candidate was rebuilt from `c6badbf6cf5b1c7340fa907821aeb4cbf2194bc0`
and exercised only on Lava `ST5GDW23LB004392`. This updates artifact-size and memory
observations, but remains a smoke measurement rather than a performance pass:

- APK: **40,431,923 bytes**, SHA-256
  `E6CBEAD6F97C036C0C9D1663CA5972799AEF3B330D75A3D2AAA94D5E699C7DB3`.
- AAB: **36,262,021 bytes**, SHA-256
  `124E14ABE6012B3B42D7B7741D0C647416E278E82ABFE358EF89A53BAAD64021`.
- Static bundle check: base manifest, 8 ARM64 libraries, no other ABIs and all checked
  native ELF LOAD segments aligned to `0x4000`.
- Current process sample during the offline match: **304,939 KB PSS**, **452,134 KB RSS**,
  **17,394 KB Graphics**, **76 KB swap**.
- A single active-match shell sample reported **78% user + 21% kernel** for the process
  in `dumpsys cpuinfo` and **94.1% instantaneous CPU** in `top`. HAL temperatures were
  **42.45 C CPU/GPU**, **37.93 C skin** and **33.0 C battery**, with thermal status **0**;
  battery level was **37%** while USB-powered. These are short observations, not a
  sustained thermal, battery or CPU-budget result.
- `dumpsys gfxinfo` again reported **0 total frames** and an empty histogram for the
  Unity SurfaceView, so no stable-FPS, frame-pacing or GPU conclusion is claimed.
- Raw files: `C:\Projects\BattleRaja-v1-tutorial-verify\Builds\V1\Lava\`.

The smaller non-development artifact is encouraging for the V1 candidate, but it does
not establish a product memory budget, thermal/battery behavior, repeated-match growth,
CPU/GPU/GC/draw-call cost or low-end Android performance. Those measurements and human
approval remain open.

## V1.0 Android candidate measurement — 2026-08-23

The release-shaped offline Android candidate was installed and exercised only on Lava
`ST5GDW23LB004392` from the disposable verification copy. This is bounded technical
evidence, not a performance sign-off:

- Exact source: `ab5b12ad7c86f425243fc3f2a9cbc83ae97e6f6d`.
- APK: **40,420,983 bytes**, SHA-256
  `E70241D83E6DBDA977EECF9F476502FD68B89799438DBA06F024423D575E5532`.
- Device process sample: **285,509 KB PSS**, **421,336 KB RSS**, **99,160 KB Graphics**,
  **79 KB swap**.
- Android `gfxinfo` for the final APK exposed only the Unity SurfaceView/render-node
  summary and no frame/jank histogram. No exact-current FPS or stable frame-pacing pass
  is claimed from this sample.
- Thermal status was 0 in the sample; current-HAL CPU/GPU were about 42.7 C, skin about
  39.4 C and
  battery about 35 C. This is not a thermal or battery soak.
- Raw files and screenshots: `C:\Projects\BattleRaja-v1-verify-20260823c\Builds\V1\Lava\`.

Interpretation: the offline loop is runnable on the approved phone and the prior default
30-FPS presentation cap is no longer representative of the release-shaped APK. The
sample still does not establish an acceptable memory budget, long-run frame pacing,
thermal/battery behavior, or repeated-match cleanup. Those remain owner-reviewed release
gates.

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

## M11 closure baseline - 2026-08-23 (`9c91f76`)

Raw captures and JSON summaries live under ignored
`Builds/Local/M11Closure/2026-08-23/`. These are development-build measurements,
not release-budget sign-off.

- Lava menu PSS/RSS/Graphics/Swap: **407,918 / 535,216 / 82,088 / 74 KB**.
- Active pressure PSS/RSS/Graphics/Swap: **460,776 / 588,808 / 99,436 / 65 KB**.
- Late Aandhi/spectator PSS/RSS/Graphics/Swap:
  **466,716 / 594,800 / 101,484 / 65 KB**.
- Battery sample: 28% while USB-powered; temperature **34.0 C**.
- Chrome and Edge each passed three viewports with zero console errors/failed
  requests. Browser rAF means were **5.009-5.066 ms**; observed maxima reached
  **34.3 ms**. These are browser frame observations, not Unity profiler FPS.
- Local uncompressed cold transfer: **134,069,638 bytes**, including WASM
  encoded body **121,427,473 bytes**. Warm sampled transfer remained
  **121,428,073 bytes** because WASM was refetched while `.data` reused cache.
- Background suspension/return passed in Chrome and Edge with no errors/failed
  requests.

Still open: parsed frame percentiles, Unity Profiler CPU/render/GPU/GC/draw-call/
triangle/texture/mesh/audio capture, thermal soak, repeated physical rematches,
production compression/cache headers, WAN/mobile-browser load, release/AAB size,
and human performance approval. Raw Perfetto traces exist, but no trace processor
was installed on this host.

## Latest bounded smoke measurements — 2026-08-03 (`42e93e7` runtime / `e90ad19` docs)

These are bounded runtime observations, not release-performance sign-off. The raw
Android captures are retained under `Builds/M11/Logs/`; the interpretation is tracked
in `Docs/QA/Performance/runtime-smoke-20260803.md`.

- **Lava `ST5GDW23LB004392` (`LAVA LXX508`)**, active Bazaar match for 20 seconds:
  total PSS **460,165 KB**, total RSS **597,680 KB**, Graphics PSS **101,480 KB**,
  swap **240 KB**. The process-scoped sample peaked at an instantaneous **87% CPU**;
  `dumpsys cpuinfo` reported **50% user / 13% kernel** for the process in its sample.
  `dumpsys gfxinfo` exposed only the ViewRoot/render-node summary and no frame/jank
  histogram, so Android FPS, frame time, GPU and GC-rate claims remain unmade.
- **Chrome 150, local production Bazaar Bastion Web** after an 8-second warm-up:
  DOMContentLoaded **17.4 ms**, load **62.5 ms**, WASM transfer **120,872,306 bytes**
  (decoded **120,872,006 bytes**) over **1,058.9 ms**; browser `requestAnimationFrame`
  sample mean **5.603 ms**, p50 **5.5 ms**, p95 **6.1 ms**, max **6.1 ms**. JavaScript
  heap used **58,307,579 bytes** of **64,410,431 bytes** (browser heap limit
  **4,395,630,592 bytes**). The rAF values are browser observations and are not Unity
  FPS; this was a local run, not a cold CDN or mobile-Web measurement. Console summary:
  **0 errors / 0 warnings**.
- **Open measurements**: Unity Profiler CPU/GPU/GC/draw-call capture, Android frame
  pacing, thermal/battery soak, repeated-match memory growth, Web cold-load and
  mobile-browser coverage, shader warm-up, compression/cache headers and network
  bandwidth remain unmeasured.

## Authority candidate smoke measurement — 2026-08-03 (`044b1b8`)

- Chrome 150, local production Web after an 8-second warm-up: DOMContentLoaded **152
  ms**, load **251.4 ms**, WASM transfer **120,662,567 bytes** over **1,903.5 ms**;
  120 browser `requestAnimationFrame` samples mean **5.620 ms**, p50 **5.5 ms**,
  p95 **6.1 ms**, max **6.1 ms**. `performance.memory` reported **30,296,011 bytes**
  used of **31,053,339 bytes** total (browser heap limit **4,395,630,592 bytes**).
  The page had one canvas, 0 console errors and 14 autoplay-policy warnings from the
  automated reload; these are browser observations, not Unity FPS/GPU/GC evidence.
- Lava `ST5GDW23LB004392` (`LAVA LXX508`) process `com.example.battleraja.m11`:
  **458,974 KB PSS**, **596,588 KB RSS**, **95,468 KB Graphics PSS**, **100 KB swap**.
  `dumpsys gfxinfo` still exposed no frame/jank histogram, so Android FPS, frame time,
  GPU and GC-rate remain unmeasured.
- Raw captured files and the full interpretation are tracked in
  `Docs/QA/Performance/authority-runtime-20260803.md` and its sibling `.txt` files.

These values are smoke baselines only. They do not establish release budgets,
low-end-device performance, thermal/battery behavior, repeated-match memory growth,
cold-load behavior, multi-browser parity or Unity Profiler data.

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
