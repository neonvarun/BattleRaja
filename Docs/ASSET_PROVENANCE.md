# BattleRaja V1 Asset Provenance

**Status:** Local prototype provenance record; owner cultural/legal review remains required.

## Owned/generated assets

The current fighter presentation assets are generated locally by repository-owned Unity
Editor scripts `Assets/BattleRaja/Editor/ProductionArtBuilder.cs` and
`Assets/BattleRaja/Editor/ProductionPresentationBuilder.cs`. The scripts contain the mesh
profiles, colours, material settings, transforms, rig hierarchy, animation curves, particle
burst recipes and prefab composition. They do not download assets, call a remote generation
service, or require a third-party licence.

The 2026-08-28 faceted silhouette pass is recorded in commit `816d9ac`. Its lofted torso,
cloak, shoulder, arm and boot meshes, plus extruded badge/visor/sash/mask/scarf plates, are
reproducible from the checked-in generator. The explicit rebuild menu action is the reviewed
visual-change boundary; ordinary scene/build generation keeps the committed asset identities
stable.

Generated outputs:

- `Assets/BattleRaja/Content/Art/V1/Meshes/*.asset`
- `Assets/BattleRaja/Content/Art/V1/Materials/*.mat`
- `Assets/BattleRaja/Content/Prefabs/Production/BijliProduction.prefab`
- `Assets/BattleRaja/Content/Prefabs/Production/PehelProduction.prefab`
- `Assets/BattleRaja/Content/Prefabs/Production/MayaProduction.prefab`
- `Assets/BattleRaja/Content/Prefabs/Production/UmbrellaProduction.prefab`
- `Assets/BattleRaja/Content/Prefabs/Production/DholProduction.prefab`
- `Assets/BattleRaja/Content/Prefabs/Production/TiffinProduction.prefab`
- `Assets/BattleRaja/Content/Art/V1/Animation/FighterProduction.controller`
- `Assets/BattleRaja/Content/Art/V1/Animation/Clips/*.anim`
- `Assets/BattleRaja/Content/Art/V1/VFX/*.prefab`

The fighter and gadget prefabs are render-only identity assets. Their rig joints, particle
systems, beacon, pedestal and themed parts have no colliders; pickup availability, collection
radius, inventory and authority remain on the scene actors. The Bazaar, Tutorial and
MovementLab scenes store explicit references to the current generated prefab roots, and the
editor generator includes a reference-refresh pass after prefab regeneration.

The generator is the editable source of truth for these procedural meshes. Unity import
settings and prefab references are committed as ordinary text/binary project assets after
human review.

## Directional references

`Art/Concepts/key-art.png`, `champions-lineup.png`, `arena-bazaar-bastion-topdown.png`, and
`jugaad-gadgets-sheet.png` are local concept references supplied for visual direction. They
are not treated as final gameplay assets, are not used as runtime textures, and are not
represented as authored screenshots.

## Third-party inventory

| Source | Current use | Licence/status |
| --- | --- | --- |
| Unity 6000.5.6f1 / URP / Input System | Engine and package runtime | Existing project baseline; owner must recheck licence and package notices |
| Android SDK/NDK/JDK | Local build tooling | Toolchain, not shipped content; version evidence belongs in release QA |
| No external art/audio packs | None | No external asset licence currently claimed |

Before any Play submission, inventory all transitive Unity/package notices, replace any
temporary audio fallback if it is still observed, and obtain owner approval for brand,
cultural and legal wording. The owned generated WAV source set and mixer are recorded in
`Docs/AUDIO_BIBLE.md`; final mix and device loudness review remain open. Do not claim the
directional concept images as licensed production art.
