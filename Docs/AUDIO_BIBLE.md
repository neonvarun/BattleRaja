# BattleRaja V1 Audio Bible

**Status:** Owned reproducible V1 source-audio baseline; final human mix and device review
remain open.

## Mix intent

Audio should feel like a bright, compact arena toybox: short percussive transients, warm
low-mid ambience, and clear fighter/gadget signatures. Gameplay information must remain
audible at low volume and in reduced-flash mode.

## Required mixer groups

- Music
- Ambience
- UI
- Combat
- Abilities
- Gadgets
- Zone

`Assets/BattleRaja/Editor/ProductionAudioBuilder.cs` generates original, deterministic PCM
WAV sources under `Assets/BattleRaja/Resources/Audio/V1/` from documented synthesis recipes.
The runtime `BattleRajaAudioDirector` loads those owned sources first and keeps its small
in-memory tones only as a development fallback if an asset is unavailable. `BattleRajaV1.mixer`
contains the Master, Music, Ambience, UI, Combat, Abilities, Gadgets and Zone buses; the
runtime routes music and effects through the mixer when it is present.

The source set includes UI confirm/back, generic and fighter-specific attack/ability cues,
hit, elimination, pickup, generic and gadget-specific cues, zone warning/closing, victory,
defeat, Bazaar ambience and match music. Combat cues are mono 44.1 kHz PCM; ambience/music
are stereo 22.05 kHz PCM. Generated files are owned project sources, not extracted,
recorded or imitated reference-game audio.

`BattleRajaV1.mixer` contains named Music and Combat buses. `BattleRajaAudioDirector` routes
music/effects through those buses and always applies the persisted source-volume controls;
it does not probe fragile editor-only exposed-parameter metadata at runtime. The PlayMode
audio-structure test verifies the buses and owned clips without relying on that metadata.

## V1 authored-pass checklist

- Human-mix and device-review the owned attack, ability, hit, elimination, pickup, gadget,
  zone, victory and defeat WAV sources.
- Add fighter-specific attack/ability signatures for Bijli, Pehel and Maya.
- Add Umbrella, Dhol and Tiffin identity cues plus station healing feedback.
- Add Bazaar ambience and a loopable match bed with a clear final-circle layer.
- Route and meter each group, cap simultaneous voices, and check clipping on Lava.
- Preserve a silent/reduced-flash route and respect music/effects settings.

Remaining audio gates are human mix/voice-limit/clipping review on Lava and final authored
performance polish. The generated source provenance is recorded in the builder and this
document; no third-party audio licence is claimed.
