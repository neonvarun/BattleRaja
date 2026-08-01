# Later prompt — Milestone 1: Movement Laboratory

Use this only after Milestone 0 is reviewed and approved.

Read all authoritative project documents first. Implement Milestone 1 only:

- One grey-box test arena
- One player-controlled placeholder fighter
- Code-driven top-down movement
- Independent aim direction
- Desktop controls
- Android twin-stick controls
- Browser keyboard/mouse controls
- Mobile-browser touch controls only where the support matrix permits
- Stable camera follow
- Aim indicator
- Safe-area-aware touch UI
- Configurable movement/input data
- Relevant EditMode and PlayMode tests
- Android development build
- Locally served Web development build

Do not add combat, bots, Photon, PlayFab, progression or final art.

Before implementation, inspect existing architecture and propose the exact files, tests and camera projection experiment. Record the approved camera decision in `Docs/DECISIONS.md`.

Completion requires evidence of frame-rate-independent movement, stable camera behaviour, passing tests and a successful Android and Web smoke builds where the toolchain permits it. Request human feedback specifically about responsiveness, stick position/size, camera angle and aim feel.
