# 11 — Audio, Music, Haptics and Game Feel

## Context

The repository contains generated deterministic PCM WAVs and mixer groups for music, ambience, UI, combat, abilities, gadgets and Aandhi. They prove routing but are not yet a final authored mix. This stage gives Bastion Crown a compact original audio identity and tactile response without importing unlicensed material.

## Objective

Players should hear and feel what matters: their fighter's attack, a confirmed hit, ally sustain, Crown carrier/drop/deposit, ticket loss/respawn, Aandhi warning, victory/defeat and the invitation to rematch. Audio must clarify rather than overwhelm the mobile HUD and must honor music/effects/haptic settings.

## Current-state audit

Inspect `ProductionAudioBuilder`, WAV assets, mixer groups/snapshots, audio sources, pooling, cue triggers, listener settings, volume preferences, haptics adapter, pause/lifecycle behavior and release stripping. Listen on Lava speaker and headphones; check clipping, loops, priority, latency, duplicate cues and generated tone quality.

## Preserve

Preserve owned/provenance-safe audio, mixer group separation, local music/effects preferences, haptic toggle, pause/AudioListener behavior, authority event identity and pooled cue infrastructure. Keep all gameplay truth in authority; audio is a render/feedback consumer.

## Replace/fix

Replace flat sine/noise placeholder feel, duplicate or late cues, clipping, uncontrolled ambience, missing team/objective states, haptics that fire during pause/reduced-flash and cues that compete with speech/critical alerts. Do not copy a reference game's melody, samples, voice or sound signature.

## Implementation tasks

1. Write an audio style brief: playful fictional Bazaar materials, compact per-fighter motif, neutral team identity, Crown chime, shrine deposit phrase, Aandhi pressure, ticket/respawn and result cadence. Keep it original and culturally respectful.
2. Create or synthesize source assets with provenance: UI tap/back/error, movement/attack/impact/knockback/heal, each ability, each gadget, Crown pickup/drop/carrier/deposit, shrine channel interruption, ticket/respawn, Aandhi warning/closing, KO/spectator, victory/defeat/rematch and ambience/music loops.
3. Route through mixer groups `Music`, `Ambience`, `UI`, `Combat`, `Abilities`, `Gadgets`, `Zone` (or documented equivalent). Add snapshots/ducking so Crown/Aandhi/result cues cut through without clipping.
4. Add event deduplication, distance/priority rules, pooling and deterministic cue IDs. Do not drive score, cooldown, damage or timing from audio.
5. Integrate haptic patterns for attack/hit/ability/gadget/Crown deposit/KO/Aandhi/result, with a reduced/disabled path, settings persistence and lifecycle cancellation.
6. Validate loudness and mix on device; document target integrated/peak ranges and any intentional artistic deviation. Keep package size bounded with compression/streaming policy.

## Asset tasks

Produce source/project files, exported WAV/OGG assets, loop points, loudness notes, provenance/license record, mixer snapshots, haptic patterns and fallback/no-audio behavior. Use short nonverbal cues where possible; if voice is added, author it and keep it fictional/optional. No unlicensed packs or reference-derived content.

## Integration points

Integrate authority/replay events with fighter/gadget/objective systems, UI state, VFX timing, `BattleRajaAudioDirector`, local settings, pause/lifecycle, tutorial and results/rematch. Ensure Music/Effects controls affect the correct groups and haptics never alter simulation.

## Performance constraints

Pool one-shots, cap simultaneous voices, avoid per-event allocations, use compressed/streamed music where appropriate and keep memory/decoder cost measured. Profile worst-case eight actors plus Crown, all gadgets, VFX and Aandhi on Lava.

## Tests

Add unit/integration tests for cue mapping/deduplication, mixer-volume/preferences, pause/resume, reduced-flash/haptic-off, missing-asset fallback, pooled source reuse and deterministic event IDs. Add PlayMode checks for all mandatory cues and no audio-driven gameplay state.

## Visual QA

Pair each cue with its visual event in a busy match. Confirm a player can hear Crown/Aandhi/respawn/result over combat, can identify fighter ability by sound+shape, and does not get startled by excessive flash/shake/haptic. Check settings feedback and rematch silence/loop reset.

## Lava verification

On Lava `ST5GDW23LB004392`, listen through speaker and headphones across a full 4v4 match, tutorial, pause, settings, KO/respawn, Crown deposit, Aandhi, results and rematch. Toggle music/effects/haptics/reduced flashes and verify immediate, persistent behavior; never use Oppo.

## Failure cases

Test missing/late clip, duplicate event, simultaneous eight-actor impacts, audio focus loss, Bluetooth route change, lifecycle pause, muted volume, haptic unavailable, mixer snapshot failure, voice/ambience masking Crown, pool exhaustion and low-memory asset fallback.

## Binary acceptance gate

Pass only when all mandatory gameplay/UI/objective states have original, correctly mixed, settings-aware audio/haptic feedback; no clipping/critical masking/duplicates occur; assets have provenance; tests pass; and Lava speaker/headphone observation confirms game-feel improvement. A folder of generated WAVs without a verified mix is a fail.

## Evidence to retain

Audio style/provenance brief, source/project files, exported asset inventory, mixer/snapshot diagram, loudness/voice report, haptic table, cue tests, memory/voice metrics and Lava listening notes/captures with build/settings.

## Non-scope

Do not add online voice chat, licensed/copyrighted music, progression/economy audio, new fighters or change authority rules.

## Stop condition

Stop before prompt 12 if critical objective cues are masked, settings do not work, audio/haptics continue through pause, unlicensed assets exist, the mix clips or device performance/memory regresses without a fix.
