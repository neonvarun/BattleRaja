# BattleRaja visual and interaction QA report

Date: 2026-08-03
Runtime-bearing candidate: `1f59a68` (`test: exercise live fighter abilities`)
Current source HEAD: `1f59a68` (Phase 2 live-controller coverage after the fixed-tick correction)
Unity: `6000.5.6f1`
Web candidate: `Builds/M11/Web` served over local HTTP at `http://127.0.0.1:8137/index.html`

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
| Gadget pickup/use | Not verified | `playwright-1280x720-gadget-prompt.png` captures the honest tutorial prompt, but the smoke path continued to show `GADGET [G] empty`; no successful pickup/use was captured. |
| Pause/settings | Observed | `playwright-1280x720-settings.png`; the overlay is readable but covers a substantial portion of the arena. |
| Spectator | Observed | `playwright-1280x720-spectator.png`; the player was eliminated during the run and the spectator surface was entered through the real `SPECTATE` control. |
| Results/rematch | Not verified | The run reached `MATCH RESOLUTION` with one survivor but did not reach a results/rematch surface; no fake result was produced. |
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
