# BattleRaja visual and interaction QA report

Date: 2026-08-03  
Runtime-bearing candidate: `4391f09` (`feat: add replayable tutorial arena`)  
Current source HEAD: `0699980` (visual QA evidence/documentation follow-up commits after the runtime-bearing candidate)  
Unity: `6000.5.6f1`  
Web candidate: `Builds/M11/Web-BazaarBastion` served over local HTTP at `http://localhost:8124/index.html`

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
- The supported `390×844` portrait menu fits, but portrait gameplay is horizontally cropped (`playwright-390x844-match-crop.png`) and the tutorial overlay is clipped (`playwright-390x844-tutorial-crop.png`). Mobile Web is therefore not a passing layout target.
- The match HUD is visibly dense: the top-left status strings are close together and the pause/settings panel covers gameplay. This remains prototype-quality presentation, not visual approval.
- Touch controls occupy meaningful screen area and should be reviewed on the Lava phone before any release claim.
- Gadget success and results/rematch remain evidence gaps rather than passed states. Spectator is technically observed, but still needs human UX review.

## Gate decision

Phase 7 is **In progress**, not passed. Desktop viewport coverage is now evidenced, but portrait gameplay fails responsive layout, the loading/gadget/results surfaces remain incomplete, and final visual quality, mobile ergonomics and human UX approval are still open.
