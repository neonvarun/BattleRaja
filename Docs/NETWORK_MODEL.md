# Network Model

**Status:** Future design; networking is not active during Milestone 0.

## Core rule

Offline gameplay logic must work without Photon. Photon later becomes an infrastructure adapter around the approved simulation/command boundaries.

## Public match direction

- Trusted authority
- Client inputs, not client-owned outcomes
- Prediction and reconciliation
- Remote interpolation
- Validated damage, cooldowns, pickups, movement and results
- Latency, jitter, packet-loss and reconnection testing

Do not install or integrate Photon yet.

## Web requirements

Photon Fusion currently documents WebGL support, but exact SDK/version/topology must be re-verified at the networking milestone. Browser tabs must never be trusted public-match authorities. Public Android–Web cross-play should use trusted server authority and protocol/content-version compatibility checks.
