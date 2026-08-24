# BattleRaja visual and interaction QA report

## Exact current candidate — docs `be0c510` / source `1d743b0` / runtime `d96d3f2` — 2026-08-24

The documentation HEAD is `be0c510`; the current exact Android candidate is the lifecycle-guard build documented in
`Docs/QA/LATEST_HEAD_BASELINE.md`. EditMode is **125/125** and PlayMode is
**71/71**. The APK installed only on Lava `ST5GDW23LB004392` and resolved to
the Unity activity, but the active lock screen prevented inspection of the
rendered menu, tutorial or match, so this report makes no screenshot or human
visual claim for this candidate. The lifecycle pause/resume behavior is covered
by automated PlayMode regression; physical background/resume, touch, safe-area,
accessibility and cultural review remain open. The prior screenshots in this
append-only report remain historical and are not silently reattributed.

## Exact current presentation update — `f463b1b` — 2026-08-24

The current branch tip is `f463b1b`. The presentation now renders a world-space
current Aandhi boundary and a warning-state next-boundary preview from the match
snapshot; the component is render-only and does not decide gameplay outcomes.
The exact-source Android APK built successfully, but was not installed on Lava.
The exact Web build was attempted locally and did not complete because Unity's Web
Bee/Burst backend repeatedly returned exit code 4. Therefore this slice has no
Android screenshot or browser-smoke evidence yet; human visual review remains
open.

Date: 2026-08-24
Runtime-bearing candidate: `dff3a89` (offline Android V1 release-shaped splash/menu/match)
Current validated runtime source: `dff3a89` (runtime content from `6920edd`; docs/settings
continuation)
Unity: `6000.5.6f1`
Web candidate: prior `7751f53` `Builds/M11/Web-BazaarBastion` served over local HTTP; it
was not rebuilt for the Android-only control change.

## Exact-current branded launch and match sample — 2026-08-24

The exact `dff3a89` release-shaped APK was installed only on Lava
`ST5GDW23LB004392`. Validation was 0/0, EditMode 125/125 and PlayMode 69/69. The
APK and matching AAB were rebuilt from the same disposable checkout; the APK is
`A6760651223052BEFB426DA08F5434ED71922A3FF9309336C1827945474F4A91` and the AAB is
`567EF167654BC53A1836035297385278E2673411C7BD06A6257E550737E3CBF4`.

Cold-launch captures under
`C:\Users\USER\AppData\Local\Temp\battleraja-splash-dff3a89\` show the dark
BattleRaja-owned splash, original icon and offline menu with no Unity logo. A
bounded diagnostic route reached the opening match; the inspected capture and
20-second device measurements are under
`C:\Users\USER\AppData\Local\Temp\battleraja-profile-dff3a89\`. The opening frame
shows the eight-actor Bazaar Bastion scene, player identity, Aandhi status, gadget
slot, circular movement/aim controls and action buttons. This remains diagnostic
evidence: coordinate taps do not replace human touch/accessibility review, and
`gfxinfo` did not expose a frame histogram.

## Exact current Android control-rotation pass — 2026-08-24

The exact `b954a72` APK was installed only on Lava `ST5GDW23LB004392`. Validation was
0/0, EditMode 125/125 and PlayMode 69/69. The inspected menu and opening-match
captures are outside source under
`C:\Users\USER\AppData\Local\Temp\battleraja-control-rotation-b954a72\`.
The menu uses the wide responsive hero/CTA rail; the match opening shows the Aandhi
status, Bazaar geometry, fighter identity, gadget state, sticks and action controls.
The exact APK diagnostic route reached menu → mode → fighter → match. This confirms
the current route is live, but coordinate automation is only diagnostic because the
Unity surface exposes no semantic Android UI nodes. Physical touch comfort, portrait
rotation, accessibility, sustained performance, full tutorial/results/rematch and
human visual/cultural approval remain open.

## Tutorial completion correction — 2026-08-24

The exact `c6badbf6cf5b1c7340fa907821aeb4cbf2194bc0` APK was installed only on Lava
`ST5GDW23LB004392`. Full validation and test suites passed (0/0, 125/125, 66/66).
The tutorial SKIP action now leaves a visible completion card instead of exposing the
backdrop alone; the inspected `tutorial-fix-complete.png` shows the replay and menu
actions. The same exact correction APK captured a successful player-owned Tiffin pickup/use
at spawn in `tutorial-fix-gadget-after.png` (held slot empty, cooldown and station visible).
This correction is technical UI evidence, not final accessibility or human visual approval.
The preceding `abe9ae4` captures remain the exact-source visual evidence for the revised
canopy, match resolution and rematch.

## Current Android V1 visual-polish slice — 2026-08-24

Exact source `abe9ae4816054e9704d13496bcd50bb7720eaa4f` was built in disposable copy
`C:\Projects\BattleRaja-v1-visual-verify` and installed only on Lava
`ST5GDW23LB004392`. Validation was 0/0, EditMode 125/125 and PlayMode 66/66.
Inspected captures cover the menu, solo mode, fighter selection, active Bazaar Bastion,
movement, match resolution and rematch, plus a gadget placement attempt. The center landmark now uses a six-panel canopy
and gold orb; the earlier intersecting-bar shape is absent. The menu hero is larger and
reads more clearly at the Lava landscape viewport. These are technical captures, not
final visual or cultural approval.

The placement probe returned `InvalidPlacement` after the player had moved against the
edge of the arena. It is recorded as a truthful negative observation; the successful
Tiffin pickup/use route remains evidenced by the prior `d825832b` captures. The same run
reached the results panel and returned through REMATCH to a fresh match. No new Web visual
claim was made in this Android-only continuation. Stable FPS, full tutorial, accessibility,
audio, multi-browser parity and human review remain open.

## Offline Android gadget-route continuation — 2026-08-24

The exact source `d825832bced4c5e07c7967d891696842eb55609a` was built and installed
only on Lava `ST5GDW23LB004392` from disposable copy
`C:\Projects\BattleRaja-v1-verify-20260824j`. The slice retains state-specific fighter
silhouette motion, a pooled impact halo/shape and animated gadget identity visuals, and
places the production Tiffin pickup near the protected player spawn. Full automated gates
passed: validation 0/0, EditMode 125/125 and PlayMode 66/66.

Inspected captures are outside source under
`C:\Projects\BattleRaja-v1-verify-20260824j\Builds\V1\Lava\`:

- `v1-menu.png`, `v1-mode.png`, `v1-fighter.png` and `v1-match-near.png` show the
  reachable offline flow, selection route, Bazaar Bastion and eight-actor match.
- `v1-match-near.png` shows the player HUD receiving `tiffin_station` from the
  production pickup route; `v1-tiffin-used.png` shows the held slot empty after use,
  the cooldown and the visible Tiffin Station.
- The bounded interaction capture is not frame-perfect proof of every transient effect,
  but no fatal application marker was found in the sampled logcat.

The Android UI tree exposes Unity as one full-screen surface without semantic button nodes;
coordinate taps were therefore used for this limited smoke probe and are recorded as a
tooling limitation. The sampled `gfxinfo` still exposed no usable frame/jank histogram;
stable FPS is not claimed. The debug-signed temporary package, final art/audio, touch and
accessibility approval, tutorial/results observation, performance series, store/legal and
cultural review remain open.

## Offline Android V1 candidate visual pass — 2026-08-23

Exact candidate source: `ab5b12ad7c86f425243fc3f2a9cbc83ae97e6f6d`.

The current Android-first candidate was installed and inspected only on Lava
`ST5GDW23LB004392` from the disposable verification copy. External captures are under
`C:\Projects\BattleRaja-v1-verify-20260823c\Builds\V1\Lava\`:

- `visual-kit-menu.png` shows the original shield/bolt identity, offline loop summary and
  reachable tutorial, settings/accessibility and help routes.
- `visual-kit-fighters.png` shows Bijli/Pehel/Maya cards, distinct vector glyphs and concise
  ability hints. The persisted selection is visible and intentional.
- `visual-kit-match.png` and `visual-kit-combat.png` show the eight-actor Bazaar Bastion opening,
  brighter runtime key lighting, distinct silhouettes, HUD and
  circular twin-stick/action controls with no square underlays.
- `visual-kit-moved.png` shows a real touch movement probe with the player displaced in the arena.
- The current UI code adds vector hero/fighter identity graphics, haptic settings and
  event-driven pickup, gadget, zone and result cues. These remain replaceable placeholder
  presentation and do not constitute final art approval.

This is a technical visual inspection, not human approval. The candidate still uses
replaceable source-backed placeholder presentation, a temporary package identity and a
debug-signed APK; final touch ergonomics, accessibility, pacing, audio, cultural and
store review remain open. Gadget pickup/use and complete tutorial/results/rematch
observation still require a dedicated human run on the exact candidate.

## Reference-app observation (Lava, read-only)

The installed Brawl Stars package was opened only to inspect high-level usability patterns:
landscape home composition, a clear primary Play action, edge-anchored resources/panels and
high-contrast icon labels. BattleRaja uses original shield/bolt, vector fighter and Bazaar
presentation; no Brawl Stars branding, characters, UI art, arena or audio was copied. Smash
Karts was not installed on the approved device, so it was not installed or inspected.

The final Lava `gfxinfo` capture exposed no frame/jank histogram. This is bounded technical
evidence, not a human performance approval; stable FPS and long-run pacing remain open.

## Current HEAD smoke — 2026-08-03

- The rebuilt Web candidate from `6f0fe8b` returned HTTP 200 on port 8139. Playwright
  session `brweb5` reported 52 console messages with **0 errors / 0 warnings** after
  reload. The inspected 1024×768 menu capture is
  `Phase7/web-api-warning-clean-1024x768-20260803.png`.
- The exact `6f0fe8b` Android APK was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`); `UnityPlayerGameActivity` remained top-resumed.
  The inspected portrait menu capture is
  `Phase7/android-api-warning-clean-20260803.png`.
- These captures confirm current-source startup/rendering only. Gadget pickup/use,
  loading-state observation, touch ergonomics, multi-browser parity, performance and
  human visual approval remain open.

## Offline attack-authority continuation — 2026-08-03

- The exact `583106e` Android APK was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). The inspected portrait menu capture is
  `Docs/QA/Visual/Phase7/android-authority-attack-20260803.png`; it renders the
  BattleRaja title and the offline/online/tutorial/settings route without a blank
  canvas or obvious clipping at the captured device viewport.
- A Web build was started against `583106e` but stopped at the owner's request before
  Unity reported success. Therefore this report retains the previous Web screenshots
  as historical references and makes no new exact-source Web visual claim.
- Human visual review, gadget-use readability, touch ergonomics, multi-browser parity,
  performance and final authored presentation remain open.

## Production Pehel authority smoke — 2026-08-03

- The rebuilt Web candidate was served at `http://127.0.0.1:8139/index.html`; the
  Playwright `brweb` session reported **0 console errors and 0 warnings** across 52
  messages. A fresh 1024×768 menu capture is
  `Docs/QA/Visual/Phase7/web-bazaar-pehel-authority-menu-1024x768-20260803.png`.
- The fresh Android APK was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`); `UnityPlayerGameActivity` remained top-resumed.
  The readable menu capture is
  `Builds/M11/Logs/android-lava-pehel-authority-menu-ready-20260803.png`.
- These are technical smoke captures for the authority continuation. Gadget use,
  authored art/audio, touch ergonomics, multi-browser parity and human visual approval
  remain open.

## Production gadget route evidence — 2026-08-03

- The production Bazaar PlayMode regression now proves Dhol pickup/use through the
  live authority route, but this is automated evidence rather than visual approval.
- The inspected Web and Lava captures still do not prove a human-readable pickup/use
  moment; gadget visual readability, touch ergonomics, authored effects, browser parity
  and human review remain open.

## Current HEAD Web/Android visual interaction gate (`efbb1ac` docs atop `8f190cd`, 2026-08-03)

- The current Web candidate was served at `http://127.0.0.1:8139/index.html` and
  inspected with Playwright. Menu screenshots were captured at **1280×720**, **1440×900**
  and **1024×768**. At 1024×768, screenshots cover mode selection, fighter selection,
  loading/warmup, match opening, active Aandhi pressure, spectator, results and rematch:
  `Docs/QA/Visual/Phase7/web-bazaar-*.png` and
  `web-bazaar-results-final-probe-1024x768-20260803.png`.
- The Web route rendered the shared Bazaar greybox scene, readable HUD labels, zone
  transitions, spectator state, complete placements/statistics and rematch restart.
  Playwright reported **0 console errors** and one Unity deprecation warning about
  manual `persistentDataPath` synchronization. This is interaction evidence, not final
  art, accessibility, multi-browser or human approval.
- Gadget movement/use was attempted with keyboard movement and the touch-positioned
  gadget button, but the held slot remained empty before the player was eliminated.
  Gadget pickup/use therefore remains **not visually verified** despite passing functional
  PlayMode coverage.
- The current Android candidate was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). The Unity activity remained top-resumed; the inspected
  menu capture is `Builds/M11/Logs/android-lava-rebaseline-20260803.png`. The current
  match capture is a technical smoke only; the greybox arena and touch labels are readable,
  while authored presentation and human approval remain open.

## Fresh Bazaar prefab Android/Web smoke (`8f190cd`, 2026-08-03)

- The fresh Android candidate was built from the connected Bazaar prefab candidate and
  installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`). The Unity game
  activity remained top-resumed and the sampled process log contained no fatal,
  `AndroidRuntime` or `SIGSEGV` marker. Captures cover the menu, mode, fighter-selection
  and live Bazaar match route; the live match capture is
  `Builds/M11/Logs/android-lava-bazaar-match-20260803.png`.
- The live match capture shows readable `BIJLI HP 85/85`, `BOLT READY`, `DASH READY`,
  `ATTACK`, `ABILITY` and `GADGET [G]` labels over the Bazaar greybox. Movement and
  gadget probes were attempted, but pickup/use remained unverified because the HUD
  stayed empty.
- The Web candidate contains 19 files and returned HTTP 200 from the existing local
  server on port 8139. This is technical smoke evidence, not browser visual approval:
  multi-browser parity, loading/results capture, touch ergonomics, authored art/audio,
  gadget-use observation and human review remain open.

## Bazaar architecture prefab continuation (`8f190cd`)

- Bazaar architecture is now a connected greybox prefab. This is a structural content
  boundary, not a visual approval or final-art result.
- Full EditMode (100/100) and full PlayMode (51/51) passed. No fresh Android/Web artifact
  was built for this content-only continuation; the `d993a5b` Lava/Web smoke remains the
  latest artifact evidence.
- Authored art/audio/animation, touch ergonomics, multi-browser parity and human review
  remain open.

## Settings and blocked-service error route — 2026-08-03

- The rebuilt Web candidate was visually inspected at **1024×768** for the settings
  panel and locked-service error/retry route, and at **1920×1080** for the main menu
  and settings panel.
- The inspected captures show readable text, bounded panels and no obvious clipping or
  overlap at those viewports:
  `web-bazaar-settings-1024x768-20260803.png`,
  `web-bazaar-settings-1920x1080-20260803.png`,
  `web-bazaar-menu-1920x1080-20260803.png`, and
  `web-bazaar-error-reconnect-1024x768-20260803.png`.
- The error screen is an intentional blocked-service state: Photon access/session
  configuration is not approved in this build and no fake room or client-authoritative
  match is started. This proves route readability, not real online connectivity.

## Bazaar production-scene contract continuation (`26b11cc`)

- The production scene now has a dedicated `BazaarBastionScene` contract and no
  `MovementLabScene` marker. This is a structural/runtime-boundary change; no fresh
  Android or Web artifact was built for it.
- Full EditMode (100/100), full PlayMode (51/51) and focused Bazaar PlayMode (15/15)
  passed. These are functional checks, not visual approval.
- The latest inspected Android/Web artifacts remain the `d993a5b` authority-Maya
  candidates below. Greybox presentation, authored art/audio, touch ergonomics,
  multi-browser parity and human review remain open.

## Authority-driven Maya decoy runtime smoke (`d993a5b`)

- The fresh Android candidate was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). The Unity game activity remained top-resumed,
  and the sampled process log contained no fatal, `AndroidRuntime` or `SIGSEGV` marker.
  Capture: `Builds/M11/Logs/android-lava-authority-maya-20260803.png`.
- The fresh Web output returned HTTP 200 from the existing local server on port 8137.
  Browser visual approval, multi-browser parity and a human-facing decoy-use capture
  were not rerun for this continuation.
- This is technical runtime evidence for authority-owned Maya decoy state, not final
  visual approval. The decoy remains a generated capsule placeholder; greybox art,
  touch ergonomics, multi-browser coverage and owner review remain open.

## Authority-driven fighter displacement runtime smoke (`9100e69`)

- The fresh Android candidate was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). The Unity game activity remained top-resumed,
  and the sampled process log contained no fatal or `AndroidRuntime` marker. Capture:
  `Builds/M11/Logs/android-lava-authority-bijli-20260803.png`.
- The fresh Web output returned HTTP 200 from the existing local server on port 8137.
  Browser visual approval, multi-browser parity and manual ability feel were not rerun
  for this continuation.
- This is technical runtime evidence for the authority displacement seam, not final
  visual approval. Greybox presentation, touch ergonomics, gadget-use observation,
  multi-browser coverage and owner review remain open.

## Authority-driven production movement continuation (`204e4f0`)

- The current Android candidate was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). The top-resumed Unity activity remained alive and
  the sampled log window contained no fatal application marker. Inspection capture:
  `Builds/M11/Logs/android-lava-authority-movement-20260803.png`.
- The current Web output contains 21 files and returned HTTP 200 from the existing local
  server on port 8137. Browser visual approval and multi-browser coverage were not rerun
  for this continuation.
- This is technical runtime evidence for the authority movement seam, not final visual
  approval. Greybox presentation, touch ergonomics, gadget use, multi-browser coverage
  and owner review remain open.

## Authority-driven Dhol displacement continuation (`78fa990`)

- The current Android candidate was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). The top-resumed Unity activity remained alive and
  the sampled log window contained no fatal application marker. Inspection capture:
  `Builds/M11/Logs/android-lava-authority-gadget-20260803.png`.
- The current Web output contains 21 files and returned HTTP 200 from the existing local
  server on port 8137. Browser visual approval and multi-browser coverage were not rerun
  for this continuation.
- This is technical runtime evidence for the authority Dhol seam, not final visual
  approval. Greybox presentation, touch ergonomics, deeper gadget interaction coverage,
  multi-browser coverage and owner review remain open.

## Authority-first healing continuation (`678acb0`)

- The latest Android candidate was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). The inspected portrait live-match screenshot
  remains readable after moving pickup/Tiffin healing into authority state:
  `Phase7/android-lava-authority-healing-20260803.png`.
- This is technical runtime/readability evidence, not final visual approval. Touch
  ergonomics, loading, gadget use, multi-browser coverage and human review remain open.

## Authority-first actor damage continuation (`08c6f2e`)

- The latest Android candidate was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). The inspected portrait live-match screenshot
  confirms the existing readable touch controls remain present after the authority
  migration: `Phase7/android-lava-authority-damage-20260803.png`.
- This is a runtime smoke/readability observation, not final visual approval. The
  authority change is functional; touch ergonomics, loading, gadget use, multi-browser
  coverage and human presentation review remain open.

## Touch-control readability continuation (`a170746`)

- `AttackButton`, `AbilityButton` and `GadgetUseButton` now add non-raycast runtime
  labels (`ATTACK`, `ABILITY`, `GADGET`) without changing pointer semantics. The new
  PlayMode regression passed with the full 47-test PlayMode suite.
- The fresh APK was installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`).
  The inspected portrait screenshot shows all three labels over the colored control
  surfaces: `Phase7/android-lava-touch-labels-20260803.png`.
- This is technical readability evidence, not final UX approval. Touch ergonomics,
  loading-state behavior, gadget pickup/use and authored visual review remain open.

## Results/rematch interaction continuation (`044b1b8`)

- Chrome 150 reached a live production Bazaar Bastion match, let the offline match
  resolve, and displayed the real Results panel. The inspected 1280x720 screenshot
  shows all eight ordered placements with KOs, assists, damage and survival values plus
  `REMATCH` and `MENU` controls: `Phase7/playwright-1280x720-results-20260803.png`.
- Clicking `REMATCH` returned to a fresh live match. The post-transition screenshot is
  `Phase7/playwright-1280x720-rematch-match-20260803.png`. The session recorded 61
  console messages, 0 errors and 1 known Unity `JS_FileSystem_Sync` deprecation warning.
- The same exact APK was driven on the permitted Lava phone until the offline match
  resolved; `Phase7/android-lava-results-authority-20260803.png` shows the portrait
  Results panel and rematch/menu controls. This is a device smoke observation, not a
  claim of touch ergonomics or human approval.
- This closes the deliberate Results/rematch technical capture gap, but it does not
  claim final visual quality, touch ergonomics or owner approval. Gadget pickup/use is
  still not visually verified.

## Production Bazaar Bastion flow (`d1b33d1` + `4d3ae6a`)

- `BuildWebBazaarBastionDevelopment` was rebuilt from the current source and served at
  `http://127.0.0.1:8139/index.html`; Chrome 150 loaded the exact production output with
  one Unity canvas and 52 console messages (0 errors, 0 warnings).
- The inspected production route covered main menu, settings, mode selection, fighter
  selection, and live Bazaar Bastion matches. Pehel and Maya captures show their own
  selected-fighter HUD identity and ability labels rather than the former hard-coded
  Bijli label. Captures: `Docs/QA/Visual/Phase7/playwright-production-main-menu-20260803.png`,
  `playwright-production-settings-20260803.png`, `playwright-production-mode-selection-20260803.png`,
  `playwright-production-fighter-selection-20260803.png`, `playwright-production-pehel-match-20260803.png`,
  and `playwright-production-maya-match-20260803.png`.
- This is technical greybox evidence, not final visual approval. The 390x844 MovementLab
  screenshot (`playwright-input-system-portrait-20260803.png`) still shows bot debug-label
  and compact-HUD overlap. Touch ergonomics, loading-state behavior, multi-browser coverage
  and human presentation approval remain open.

## Input System-only continuation (`5f4566d`)

- The active input handler is now Input System only. Generated EventSystems use
  `InputSystemUIInputModule`; older serialized scenes are adapted at load by the
  compatibility bridge. This is a technical input-path validation, not final UX approval.
- The exact development APK installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`) and remained alive under the sampled log window.
- The Web candidate returned HTTP 200 on port 8137 after the input migration. Browser
  focus, touch ergonomics, gadget use, Results/rematch capture and human visual approval
  remain open.

## Web input-focus continuation (`3e00b02`)

- The generated canvas now has `tabindex="0"`, an accessible game label and pointer-down
  focus restoration. The validator checks the template contract.
- Chrome and Edge both loaded the Web candidate on port 8137, reported `unity-canvas` as
  the active element after a canvas click, and produced 0 console errors/warnings.
- `playwright-web-focus-20260803.png` shows the live greybox match from the focused Chrome
  candidate. This is technical interaction evidence, not final visual approval.
- The focus fix removes one browser-QA blocker, but successful gadget use, Results/rematch
  capture, touch ergonomics, loading-state observation and human visual review remain open.

## Results-screen continuation (`810f484`)

- The pure formatter now emits a deterministic row for every placement with KOs, assists,
  damage and survival time, plus a compact portrait representation. EditMode remains
  94/94 and PlayMode is 45/45 after the new regression test.
- The exact Android candidate was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). `android-lava-results-20260803.png` shows the live
  portrait match after launch and its log sample contains no fatal application markers.
- The Web candidate returned HTTP 200 on port 8137. Chrome reached the live match;
  `playwright-results-20260803.png` shows the arena/HUD and the console scan reported
  0 errors and 0 warnings.
- These captures do not show the Results/rematch state itself. The human-facing Results
  and Rematch row remains **Not verified**, and no visual gate is claimed here.

## Fresh latest-HEAD runtime smoke (`9291d85`)

- The fresh Web candidate reached the live MovementLab match in Chrome after an
  eight-second warm-up. `playwright-fresh-baseline-20260803.png` shows the arena,
  fighters, HUD and controls; the Playwright console scan reported 0 errors and 0
  warnings.
- The fresh development APK was installed and launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). `android-lava-fresh-baseline-20260803.png`
  shows a live portrait match. The process-scoped log has no fatal crash,
  `AndroidRuntime`, `SIGSEGV`, missing-component or monotonic-tick marker.
- These captures are technical greybox evidence. They do not change the unverified
  gadget-use or results/rematch rows below into visual observations.

## Fresh assist/statistics runtime smoke (`5845485`)

- The assist-phase Web candidate reached the live match in Chrome after an eight-second
  warm-up. `playwright-assist-phase-20260803.png` shows the arena and HUD; the console
  scan reported 0 errors and 0 warnings.
- The assist-phase APK was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). `android-lava-assist-phase-20260803.png` shows
  the live portrait match, and the process-scoped sample has no fatal crash markers.
- These captures remain technical greybox evidence and do not close the visual gadget,
  results/rematch, loading, multi-browser or human-review gates.

## Fresh aim-assist runtime smoke (`c23982e`)

- The aim-assist Web candidate reached the live match in Chrome after an eight-second
  warm-up. `playwright-aimassist-phase-20260803.png` shows the arena and HUD; the
  console scan reported 0 errors and 0 warnings.
- The aim-assist APK was installed/launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). `android-lava-aimassist-phase-20260803.png`
  shows the live portrait match, and the process-scoped sample has no fatal crash
  markers.
- Runtime smoke confirms the setting does not break the path; it does not prove that
  aim-assist fairness, touch ergonomics or accessibility are human-approved.

## Fresh Phase 6 runtime smoke (`8544f55`)

- The fresh Web candidate reached the live MovementLab match in Chrome after an
  eight-second warm-up. `playwright-phase6-runtime-20260803.png` shows the arena,
  fighters, HUD and controls; the Playwright console scan reported 0 errors and 0
  warnings.
- The fresh development APK was installed and launched only on Lava
  `ST5GDW23LB004392` (`LAVA LXX508`). `android-lava-phase6-runtime-20260803.png`
  shows a live portrait match. The process-scoped log has no fatal crash,
  `AndroidRuntime`, `SIGSEGV`, missing-component or monotonic-tick marker. It still
  includes the known optional Google Play Asset Pack class lookup and Lava gralloc
  noise, both non-fatal in this sample.
- Automated PlayMode coverage now confirms authority-driven spatial Dhol collection,
  the generated live Results/Rematch reload flow, three repeated cleanup cycles and the
  complete eight-step tutorial walkthrough. Those assertions are functional evidence
  only; they do not convert the visual rows below to “Observed”.

## Test surface

The Playwright CLI loaded the WebGL candidate and exercised the required desktop viewports `1920×1080`, `1440×900`, `1280×720` and `1024×768`, plus a supported portrait viewport `390×844`. Screenshots are stored under `Docs/QA/Visual/Phase7/` with the `playwright-` prefix. The older in-app-browser captures remain useful as a separate 1280×720 reference, but are not the sole evidence for this gate.

## State coverage

| State | Result | Evidence / notes |
| --- | --- | --- |
| Loading | Not separately observed | The transition entered the real match warmup within the first captured frame; no distinct loading-progress surface was visible to capture. |
| Main menu | Observed | Playwright captures at all four desktop viewports (`playwright-1920x1080-main-menu.png`, `playwright-1440x900-main-menu.png`, `playwright-1280x720-main-menu.png`, `playwright-1024x768-main-menu.png`) and portrait (`playwright-390x844-main-menu.png`). Desktop controls fit; portrait menu also fits. |
| Mode selection | Observed | `playwright-1280x720-mode-selection.png`; offline/online labels and back control are readable. |
| Fighter selection | Observed | `playwright-1280x720-fighter-selection.png` and `playwright-1024x768-fighter-selection.png`; Bijli, Pehel and Maya choices remain readable. |
| Match opening | Observed | `playwright-1920x1080-match-opening.png`, `playwright-1440x900-match-opening.png`, `playwright-1280x720-match-opening.png` and `playwright-1024x768-match-opening.png`; actors, HUD, controls and arena render without desktop clipping. |
| Responsive portrait gameplay host | Observed | Fresh `playwright-390x844-responsive-gameplay.png` capture from the responsive Web template and orthographic framing path; the canvas fills the portrait viewport without the earlier fixed 960×600 horizontal crop. |
| Compact portrait match telemetry | Observed | Fresh `playwright-390x844-hud-compact.png` engineering capture and `playwright-390x844-hud-production.png` production Bazaar capture; phase, alive count and zone telemetry now occupy a compact two-line region rather than the earlier single squeezed line. |
| Responsive portrait tutorial | Observed | Fresh `playwright-390x844-responsive-tutorial.png` capture from the production Bootstrap → Tutorial Arena route; the tutorial card, controls and arena remain readable at 390×844. |
| Active combat | Observed | `playwright-1280x720-active-pressure.png`; fighters, projectiles/telegraphs and HUD remain visible during live pressure. |
| Aandhi pressure | Observed | `playwright-1280x720-aandhi-pressure.png`; the HUD reports `CLOSING` and the zone values change. |
| Gadget pickup/use | Not verified | Phase 6 PlayMode now proves authority-driven spatial collection and use, but no successful human-facing pickup/use capture exists; the prior Web prompt still showed `GADGET [G] empty`. |
| Pause/settings | Observed | `playwright-1280x720-settings.png`; the overlay is readable but covers a substantial portion of the arena. |
| Spectator | Observed | `playwright-1280x720-spectator.png`; the player was eliminated during the run and the spectator surface was entered through the real `SPECTATE` control. |
| Results/rematch | Observed (technical) | `Phase7/playwright-1280x720-results-20260803.png` shows the real Results panel and controls; `Phase7/playwright-1280x720-rematch-match-20260803.png` shows the fresh match after clicking `REMATCH`. Human visual approval remains open. |
| Online/error | Observed | `playwright-1280x720-online-error.png`; the build states that Photon access is unavailable and does not fabricate a room. |

## Findings

- No blank canvas, missing-material screen or browser fatal error was observed in the tested route.
- Menu, mode, fighter, settings and error labels remain inside the tested desktop canvases.
- The responsive Web template and orthographic camera path remove the earlier fixed-canvas portrait crop; `playwright-390x844-responsive-gameplay.png` shows the live match filling the tested viewport. The HUD remains dense and the tutorial overlay, touch ergonomics and mobile-Web quality still require review.
- The production tutorial route was also exercised at 390×844; `playwright-390x844-responsive-tutorial.png` shows the card and three actions inside the viewport. Text remains small and the touch ergonomics still require Lava review.
- The compact match status keeps phase/alive/zone data separated from the fighter and gadget HUD in the 390×844 capture; the MovementLab bot diagnostics remain intentionally visible in this engineering smoke scene, while production Bazaar scenes keep them hidden.
- The production Bazaar capture confirms the same narrow-viewport HUD treatment without MovementLab diagnostics; it remains a smoke-build observation, not final visual or mobile UX approval.
- The match HUD is visibly dense: the top-left status strings are close together and the pause/settings panel covers gameplay. This remains prototype-quality presentation, not visual approval.
- Touch controls occupy meaningful screen area and should be reviewed on the Lava phone before any release claim.
- Gadget success and results/rematch remain evidence gaps rather than passed states. Spectator is technically observed, but still needs human UX review.
- The production Web run logged Unity's known non-fatal `JS_FileSystem_Sync()` deprecation warning; no browser error or blank-canvas failure was observed.

- The Phase 6 smoke screenshots remain technical prototype evidence. They do not certify
  final art/audio, touch ergonomics, loading-state presentation, multi-browser parity,
  or human visual approval.

## Gate decision

Phase 7 is **In progress**, not passed. Desktop and responsive portrait framing are now evidenced, but the loading/gadget/results surfaces remain incomplete, the prototype HUD is dense, and final visual quality, mobile ergonomics and human UX approval are still open.

## Fixed-tick runtime retest (`a245f24`)

- The exact fixed-tick Android APK was installed and launched on Lava
  `ST5GDW23LB004392` only. The pre-fix capture showed the Unity console error
  `Simulation ticks must increase monotonically`; the fixed capture shows the same
  match continuing without that red runtime error.
- Evidence: `android-lava-pre-fix-tick-error-46a3d1e.png` and
  `android-lava-fixed-tick-46a3d1e.png` under `Docs/QA/Visual/Phase7/`, with the
  corresponding logcat samples in `Builds/M11/Logs/`.
- The fixed Web build was served from `http://127.0.0.1:8137/index.html`; a fresh
  Playwright render reached the live MovementLab match, the inspected screenshot has
  no blank canvas, and the console error/warning scan returned zero.
- This retest proves the tick-order runtime defect is corrected. It does not pass the
  broader visual gate: loading/gadget/results surfaces, multi-browser coverage,
  touch ergonomics and final human visual approval remain open.

## Phase 2 runtime smoke (`1f59a68`)

- The fresh Phase 2 APK was installed and launched on Lava `ST5GDW23LB004392` only.
  The inspected capture shows the live portrait match HUD and arena without the
  earlier monotonic-tick Unity error. The process remained alive after launch.
- The fresh Web build was served from `http://127.0.0.1:8137/index.html`. Chrome was
  held through the warm-up into the live match; the inspected capture shows gameplay,
  and Playwright reported 0 console errors and 0 warnings.
- These are smoke-build/runtime observations. They do not prove final visual quality,
  mobile touch ergonomics, multi-browser compatibility, gadget success, results/rematch,
  or human approval.
