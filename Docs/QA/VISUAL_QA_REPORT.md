# BattleRaja visual and interaction QA report

Date: 2026-08-03  
Runtime-bearing candidate: `4391f09` (`feat: add replayable tutorial arena`)  
Current source HEAD: `511f2f4` (the later change only adds a PlayMode cleanup regression test and documentation)  
Unity: `6000.5.6f1`  
Web candidate: `Builds/M11/Web-BazaarBastion` served over local HTTP at `http://localhost:8123/index.html`

## Test surface

The fresh in-app browser tab loaded the WebGL candidate and was inspected at the browser's available `1280×720` viewport. The browser surface does not expose a viewport-resize control in this environment, so the required `1920×1080`, `1440×900`, `1024×768` and portrait viewports were not claimed.

## State coverage

| State | Result | Evidence / notes |
| --- | --- | --- |
| Loading | Observed | `Visual/Phase7/1280x720-loading.png`; the scene activated quickly and the frame shows the real match warmup HUD. |
| Main menu | Observed | `Visual/Phase7/1280x720-main-menu.png`; five buttons fit without clipping. |
| Mode selection | Observed | `Visual/Phase7/1280x720-mode-selection.png`. |
| Fighter selection | Observed | `Visual/Phase7/1280x720-fighter-selection.png`; Bijli, Pehel and Maya choices are readable. |
| Match opening | Observed | `Visual/Phase7/1280x720-match-opening.png`; eight actors, HUD, touch controls and arena geometry render. |
| Active combat | Observed | `Visual/Phase7/1280x720-active-combat.png`; projectiles/telegraphs and fighter silhouettes are visible. |
| Aandhi pressure | Observed | `Visual/Phase7/1280x720-aandhi-pressure.png`; the HUD reports `CLOSING` and a shrinking zone value. |
| Gadget pickup/use | Not verified | The smoke path showed `GADGET [G] empty`; `Visual/Phase7/1280x720-gadget-key.png` records the honest unavailable state, not a successful pickup/use. |
| Pause/settings | Observed | `Visual/Phase7/1280x720-pause.png` and `Visual/Phase7/1280x720-settings.png`; the overlay is readable but covers a substantial portion of the arena. |
| Spectator | Not verified | The normal smoke path did not eliminate the player; clicking the spectator affordance while alive did not enter spectator mode. |
| Results/rematch | Not verified | Requires a completed or forced match outcome; no fake result was produced for visual evidence. |
| Online/error | Observed | `Visual/Phase7/1280x720-online-error.png`; the build states that Photon access is unavailable and does not fabricate a room. |

## Findings

- No blank canvas, missing-material screen or browser fatal error was observed in the tested route.
- Menu, mode, fighter, settings and error labels remain inside the 1280×720 canvas.
- The match HUD is visibly dense: the top-left status strings are close together and the pause/settings panel covers gameplay. This remains prototype-quality presentation, not visual approval.
- Touch controls occupy meaningful screen area and should be reviewed on the Lava phone before any release claim.
- Gadget success, spectator, results/rematch and alternate viewports remain evidence gaps rather than passed states.

## Gate decision

Phase 7 is **In progress**, not passed. This report is technical evidence for the one available browser viewport; it does not certify responsive behavior, final visual quality, mobile ergonomics, successful gadget use, spectator UX or results UX.
