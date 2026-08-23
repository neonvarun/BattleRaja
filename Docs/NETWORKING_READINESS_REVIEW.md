# Networking Readiness Review

Date: 2026-08-23  
Scope: transport-free readiness audit for the offline authority foundation.  
Decision: **Not ready for public multiplayer.** Photon/PlayFab implementation
remains blocked behind owner approval. This review does not add transport.

## Current Foundation

- `BattleRaja.Core.Domain/Application` owns canonical match setup, commands,
  collision, abilities, projectiles, damage/healing/collection, eliminations,
  placements, stable event identities, deterministic hashes, and replay streams.
- Presentation consumes immutable authority outputs and mirrors view health; it
  must not feed gameplay mutations back into Core.
- `MatchEventIdentityTracker` maintains independent damage, healing, collection,
  and elimination counters, assigns identities only after accepted application,
  rejects duplicates semantically, resets on restart, and participates in hashes.
- Replay headers carry scenario/fixed-step/seed/spawns/participants/pickups;
  frames carry ordered movement, attack, ability/decoy context, and gadget input.
  The executable runner reconstructs authority and verifies hash streams.
- Infrastructure contains transport-independent `NetworkSessionMock`,
  `PhotonFusionAdapter` credential seam, and `AuthoritativeMatchServer` proof
  seams for eight slots, bot backfill, grace/reconnect, and bot takeover.

## Required Contracts Before Real Transport

1. **Session envelope:** protocol/content version, match/session IDs, reconnect
   epoch, sender identity, sequence, tick, authentication claims, and checksum.
2. **Command envelope:** actor ID, authoritative tick, monotonic client input
   sequence, movement/aim/attack/ability/gadget payload, finite-vector checks,
   origin/faction/loadout derived server-side rather than trusted from client.
3. **Event envelope:** event class, stable event ID, source actor, execution ID,
   authoritative tick, epoch, applied result, and content/version fingerprint.
4. **Snapshot envelope:** tick, epoch, complete participant/projectile/item/
   station/decoy state needed for late join, plus terminal result signature/seam.
5. **Replay/result evidence:** header/content version, seed, setup, ordered
   frames, complete hash stream, terminal hash, and server-owned reward evidence.

## Duplicate Protection And Rollover Policy

- Maintain one recent-ID window per `(epoch, event class)` on the trusted
  receiver. Reject older/equal IDs; accept only strictly newer valid IDs.
- Do not rely on integer comparison across rollover. Include an epoch and treat
  rollover as an epoch boundary: freeze acceptance until both sides negotiate a
  fresh epoch/window, then start IDs at zero.
- Commands use `(reconnect epoch, client ID, input sequence)` uniqueness. A
  stale/future/out-of-window command is rejected without consuming cooldown or
  authority event identities.
- Bounded windows must be deterministic in tests and large enough for the 30 Hz
  send rate plus maximum reorder delay; document the chosen bound with latency
  evidence before online alpha.

## Reconnect And Lifecycle Contract

- Server owns slots and states: vacant, connected, disconnected-grace,
  bot-takeover, eliminated.
- Reconnect requires matching account/session identity, protocol/content version,
  current epoch, and a live grace window. Expired grace converts to bot takeover
  and cannot be reclaimed in the same competitive match.
- A new epoch invalidates old command/event windows. Late-join clients receive a
  complete snapshot before submitting commands.
- Browser background/tab suspension must never become public-match authority.
  Clients suspend/resume through the same reconnect contract as Android.

## Trust Boundary

Never trust client-reported position outcomes, damage, healing, pickup ownership,
gadget validity, cooldowns, elimination, placement, rewards, or match result.
Client payloads are requests only. Public competitive matches require a trusted
dedicated/host-authoritative simulation outside the browser. Fusion Shared/Host
modes are not automatically equivalent to trusted dedicated-server authority.

## Test Matrix To Gate M8/M9

- Protocol/content mismatch, room full, duplicate join, stale credentials.
- Good/moderate/poor profiles at minimum 50/10/0%, 100/25/2%, 200/60/5%.
- Reordered, duplicated, replayed, forged-actor, malformed/non-finite inputs.
- Disconnect at opening/pressure/final circle; expire grace; bot takeover;
  reconnect within grace; refresh/background-return on WebGL.
- Eight human/bot completion, simultaneous hits/eliminations, timeout ranking.
- Deterministic replay/hash equality under each network condition.
- Reward retry/replay rejection using server-validated evidence/idempotency key.

## Approval Gates

Owner approval is required for Fusion licence/account terms, App ID/use, paid
relay/server infrastructure, PlayFab title/environment/secrets, privacy/identity
policy, public hosting/CDN, crash/analytics, signing, store submission, and any
promotion beyond prototype. No secrets may enter the repository or client.

