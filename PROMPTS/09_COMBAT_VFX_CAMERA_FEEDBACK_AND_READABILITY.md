# 09 — Combat VFX, Camera, Feedback and Readability

## Context

The current project has particle cue prefabs, Aandhi visuals, hit/KO hooks and a closer presentation camera, but the Lava match still reads as technical: impacts are light, UI/debug labels compete with the arena and team/objective states are not layered. This stage owns the sensory language that lets a player understand a fast 4v4 fight.

## Objective

Make every important event legible within one glance and one short sound: aim, attack wind-up, projectile path, hit, damage, heal, knockback, ability, gadget, Crown carrier/drop/deposit, ticket/respawn, ally/enemy identity, Aandhi, victory and defeat. Feedback must remain readable in reduced-flash, high-contrast and low-quality modes.

## Current-state audit

Inspect fighter VFX prefabs, shader/material choices, particle counts, camera follow/zoom, hit-stop/shake, damage numbers, health/status markers, Aandhi visual, animator events, audio cue hooks, reduced-flash logic and `OfflineMatchHud` debug strings. Capture current effects in a busy eight-actor scene and measure overdraw/GC.

## Preserve

Preserve authority event order, fixed-step state, existing VFX sockets, Aandhi state, camera collision/bounds, reduced-flash preference, pooled transient infrastructure and the rule that VFX never owns gameplay. Keep team/target semantics in authority.

## Replace/fix

Replace random particle bursts, same-looking impacts, screen-filling flashes, camera jitter, unreadable overlap, floating debug text, color-only team cues and telegraphs that vanish on low quality. Fix event duplication, VFX after actor destruction, camera clipping and effects that hide Crown/shrines or ally health.

## Implementation tasks

1. Create a layered feedback taxonomy: anticipation, travel, contact, damage state, control/knockback, sustain, objective state, team signal, Aandhi and results. Map each authority event to one primary visual, one optional accent and one audio/haptic hook.
2. Author distinct fighter language: Bijli electric ribbon/dash, Pehel ground/weight/impact, Maya split/echo/decoy. Use shape, timing and motion as well as palette.
3. Author gadget/Crown language: Umbrella canopy arc, Dhol radial wave, Tiffin restorative pulse, Crown carrier halo/drop beacon/deposit channel, ticket loss/respawn shield and Aandhi boundary/target warning.
4. Add readable hit confirmation, damage/heal numbers or bars, knockback direction, invulnerability indication and ally/enemy marker hierarchy. Avoid stacking more than two transient accents on one actor.
5. Tune a stable top-down camera: player framing, soft follow, bounds, objective visibility, aim lead, death/spectator target and result transition. Provide compact aspect-ratio behavior and no camera shake when reduced flashes is enabled.
6. Implement quality tiers with consistent telegraph priority: never remove Crown, shrine, carrier, threat, respawn or Aandhi cues; reduce count, bloom, shake and secondary particles first.
7. Pool effects and enforce lifetime/instance caps. Add an instrumentation view for event-to-effect latency and duplicate suppression, disabled from release builds.

## Asset tasks

Create editable VFX/graphic assets with provenance: three fighter signature sets, three gadget sets, Crown socket/carrier/drop/deposit, shrine channel, ticket/respawn, KO/spectator, Aandhi warning/closing/final, team ping/ally marker and result burst. Specify mesh/texture/particle counts, blend mode, duration, colorblind/high-contrast/reduced-flash variant, audio/haptic cue and LOD/fallback. Avoid copied effects or sacred motifs.

## Integration points

Integrate authority/replay events with `OfflineMatchController`, animator/VFX sockets, camera, UI HUD, audio/haptics, gadget/objective systems, quality settings and team AI signals. Keep event identity deterministic and render-only.

## Performance constraints

Target no more than 40 simultaneous world particle systems and 12 transient UI effects in the worst-case baseline, with measured justification if changed. Prefer unlit/simple shaders, atlases, pooled buffers and short lifetimes. Record overdraw, GPU/CPU frame impact, GC and memory on Lava.

## Tests

Add event-to-effect mapping tests, duplicate/lifetime/pooling tests, reduced-flash/high-contrast tests, camera bounds/target transition tests and deterministic replay visual-event IDs. Add PlayMode overlap scenarios with eight fighters, Crown, all gadgets and Aandhi.

## Visual QA

Inspect stills and continuous motion at gameplay distance, close hit, busy objective, low/high quality, reduced flashes, high contrast, tall/wide aspect ratios and color-vision-safe marker combinations. A player must identify ally/enemy/carrier/channel/KO without pausing.

## Lava verification

On Lava `ST5GDW23LB004392`, capture attack/ability/gadget/Crown/respawn/Aandhi/result sequences in a real 4v4 match. Check brightness, bloom, overdraw feel, camera comfort, speaker/haptic timing and no hidden controls; never use Oppo.

## Failure cases

Test missing socket, destroyed actor, duplicate replay event, simultaneous hits, reduced-flash toggle mid-match, high-contrast material, low-tier fallback, camera at map edge, spectator target death, paused particle/time scale and effect-pool exhaustion.

## Binary acceptance gate

Pass only when all mandatory events have distinct, non-blocking, accessible feedback, camera/readability survive a busy 4v4, pooling/quality tiers are measured, automated mapping tests pass and Lava captures show the player can parse objective/combat state without debug labels. Particle quantity without comprehension is a fail.

## Evidence to retain

Event-to-feedback matrix, source VFX/graphic files and provenance, particle/shader budget, quality-tier table, mapping/pooling tests, latency/overdraw metrics and Lava captures with settings/build/hash.

## Non-scope

Do not change authority rules, add copied effects, add online/social feedback, redesign final UI structure or author final music/mix.

## Stop condition

Stop before prompt 10 if effects obscure allies/objective, differ from authority timing, fail reduced-flash/high-contrast, cause frame/GC spikes or the camera cannot keep the Crown/shrines readable.
