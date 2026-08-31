# 08 — Gadgets, Pickups, Objectives and Interactables

## Context

Umbrella Guard, Dhol Burst and Tiffin Station already have a useful mechanical baseline, and the new authority owns a Crown Spark, shrines, sockets and shared tickets. This stage makes those systems feel like one readable team-objective game rather than unrelated Solo pickups.

## Objective

Every gadget and interactable should create a clear, fair decision for a human or bot: cross danger, open space, sustain an ally, carry/deposit the Crown, contest a socket or survive Aandhi. A player must understand availability, ownership, duration, counterplay and result from visuals/audio/UI.

## Current-state audit

Inspect `Gadgets.cs`, gadget authority/components, pickup spawning/respawn, cooldowns, damage/heal/shield/knockback, current scene anchors, bot use and replay events. Verify which objects are authority state versus presentation and whether any gadget assumes Solo-only hostility or no respawn.

## Preserve

Preserve authority validation, deterministic seeded spawn/timing, common command path, three required gadget identities, pooling, existing replay/event identity and Solo behavior where healthy. Keep gadget state out of shared mutable assets.

## Replace/fix

Replace generic or ambiguous pickup presentation, gadget effects that are invisible in team combat, unsafe ally/enemy targeting, shield/heal exploits, simultaneous-use spam, objective interactions that bypass score/tickets and any world pickup that looks like a Crown. Fix missing cooldown/availability feedback.

## Implementation tasks

1. Define the V1 gadget contract and role synergies. `Umbrella Guard` creates a short directional cover window for a carrier/ally crossing danger; `Dhol Burst` is a readable radial disruption/space-maker with friendly-safe rules; `Tiffin Station` is a deployable healing/restoration point with duration, capacity and counterplay.
2. Validate all gadget commands against team relationship, spawn protection, Crown carrier state, cooldown, range, charges, map bounds and Aandhi. Abilities cannot score a Crown deposit or spend a ticket twice.
3. Define deterministic pickup spawning/rotation and a bounded number of simultaneous world interactables. Avoid power spikes that make a single pickup mandatory; report encounter frequency and usage by human/bots.
4. Implement Crown sockets, shrines, carrier/drop state, deposit channel and ticket/respawn anchors from prompt 03. The Crown is an objective, not a consumable gadget, and must never be confused with a pickup.
5. Expose readable events for pickup, cooldown, deployment, shield, heal, disruption, Crown pickup/drop/deposit, ticket spend, respawn and expiry. Record one stat per valid event.
6. Give squad AI safe utility signals: escort Umbrella, place Tiffin after regroup, Dhol to peel a shrine, contest Crown without all bots clumping. Keep difficulty fair.

## Asset tasks

Create editable, original gameplay assets with silhouette-first specifications:

- **Umbrella Guard:** compact folded-tool silhouette, unmistakable open canopy/segment shape, matte violet/teal fabric with brass ribs, one 512–1024 atlas, LODs ≤1.5k/800 triangles, deploy/open/close/impact VFX, carrier-facing arc indicator and high-contrast outline.
- **Dhol Burst:** fictional compact double-sided drum device, red/clay body with cream rings, clear charge pulse and radial wave (not a copied instrument/logo), ≤2 materials, deploy/charge/burst/cooldown cues and low-flash ripple.
- **Tiffin Station:** stackable lunch-container repair/heal station, warm brass/cream/teal, readable lid/steam/heart-shape-free heal icon, ≤2k triangles LOD0, deploy/active/empty/expire states and color-plus-shape ally-safe marker.
- **Crown Spark/shrines/sockets:** neutral geometric crown/spark motif and two distinct shrine silhouettes, socket beacon, drop ring, carrier halo, channel progress, ticket pip and respawn beacon. No sacred/political symbol; all markers must survive high contrast and reduced flashes.

Each asset needs concept, model/mesh, UV/material, LOD, prefab, provenance, device-camera test and performance record. Images are not runtime substitutes.

## Integration points

Integrate with authority commands/events, fighter kits, team AI, map anchors, `OfflineMatchController`, UI HUD/results/tutorial, VFX/audio directors, replay and save/rematch. Keep decorative children separate from colliders and authority objects.

## Performance constraints

Pool gadget/projectile/VFX instances, cap active pickups and particles, avoid runtime material clones, bound overlap queries and keep objective state allocation-free in ticks. Profile worst case: eight fighters, three gadgets, Crown carrier, Tiffin, Dhol and Aandhi together.

## Tests

Add pure tests for each gadget's targeting/duration/cooldown/charges/damage/heal/shield and expiry, ally safety, Crown/ticket idempotence, pickup spawn determinism, map bounds, pooling and replay. Add scenario tests for every gadget in attack/defend/escort/retreat/Aandhi states and multi-seed usage metrics.

## Visual QA

Inspect each gadget and objective state in a busy 4v4 match, at distance, under warm/cool light, low quality, high contrast and reduced flashes. A player must distinguish pickup versus Crown, ally healing versus enemy disruption, active versus expired station and channel interruption instantly.

## Lava verification

On Lava `ST5GDW23LB004392`, use all three gadgets as human and observe bots use them. Capture Crown pickup/drop/deposit, Umbrella escort, Dhol peel, Tiffin regroup, pickup refresh, cooldown, ticket spend and Aandhi. Verify no network is needed and never use Oppo.

## Failure cases

Test gadget during KO/respawn, Crown deposit, spawn protection, pause/resume, missing prefab, blocked deploy position, duplicate pickup, full/empty Tiffin, Dhol through walls, Umbrella orientation, Crown outside socket, ticket underflow, object pooling reuse and low-quality VFX fallback.

## Binary acceptance gate

Pass only when all three gadgets and Crown/shrine/ticket interactables are mechanically authoritative, deterministic, team-safe, counterable, visually distinct, integrated with bots/UI/tutorial, pooled/performance-tested and demonstrated on Lava. A colored primitive or particle-only placeholder is a fail.

## Evidence to retain

Gadget/objective definitions, source asset/provenance/LOD manifests, event traces, unit/scenario/soak output, usage balance metrics, VFX/audio/UI links and Lava captures with build/hash/settings.

## Non-scope

Do not add new gadgets, online inventory/economy, monetization, copied props, final audio mix (prompt 11) or a second map.

## Stop condition

Stop before prompt 09 if any gadget/objective can harm allies unintentionally, score twice, bypass tickets, disappear without feedback, create unbounded allocations or cannot be read on Lava in reduced-flash/high-contrast mode.
