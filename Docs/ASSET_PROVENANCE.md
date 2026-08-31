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

The 2026-08-29 technical-art continuation is recorded in commit `bc392fd`. The generator now
assigns deterministic UVs to every saved mesh. The presentation builder derives the primary
Bijli/Pehel body and Maya cloak into the saved `BijliSkinBody.asset`, `PehelSkinBody.asset`
and `MayaSkinCloak.asset` meshes with two-bone hips/chest bind poses and waist-blended
weights. The source MeshFilters remain in the prefabs for deterministic rebuilds, while only
their primary MeshRenderers are disabled; the saved SkinnedMeshRenderer is the visible
presentation surface. This remains repository-owned generated art, not a claim of final
commissioned modeling, texturing or cultural approval.

The 2026-08-29 environment continuation is recorded in commit `ac45479`. The repository
owned `Assets/BattleRaja/Editor/ProductionEnvironmentBuilder.cs` creates the saved
`BazaarBastionProduction.prefab`, its low-detail backdrop, six environment meshes, 16
64×64 texture assets and matching URP materials. The environment is deliberately
collider-free so the existing authored collision/navigation layer remains authoritative.
`Assets/BattleRaja/Presentation/Visuals/PresentationMeshFactory.cs` supplies a small
allocation-free-after-first-use cache for emergency rings, boxes, cylinders, discs and
faceted orbs. It is not an external asset source; all runtime-created fallback geometry is
generated from checked-in code, and production scenes resolve saved prefabs first.

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
- `Assets/BattleRaja/Content/Art/V1/Environment/Meshes/*.asset`
- `Assets/BattleRaja/Content/Art/V1/Environment/Textures/*.asset`
- `Assets/BattleRaja/Content/Art/V1/Environment/Materials/*.mat`
- `Assets/BattleRaja/Content/Prefabs/Production/BazaarBastionProduction.prefab`
- `Assets/BattleRaja/Resources/Audio/V1/*.wav` (including the generated `ZoneFinalCircle`
  endgame cue) and `BattleRajaV1.mixer`

The fighter and gadget prefabs are render-only identity assets. Their rig joints, primary
skinned renderer, particle systems, beacon, pedestal and themed parts have no colliders;
pickup availability, collection radius, inventory and authority remain on the scene actors.
The Bazaar, Tutorial and
MovementLab scenes store explicit references to the current generated prefab roots, and the
editor generator includes a reference-refresh pass after prefab regeneration.

The generator is the editable source of truth for these procedural meshes. Unity import
settings and prefab references are committed as ordinary text/binary project assets after
human review.

## Menu feature art replacement — 2026-09-01

`Assets/BattleRaja/Art/V1/BattleRaja-FeatureArt-OriginalCandidate.png` is a new,
repository-owned presentation candidate generated with the built-in image-generation tool
from an original BattleRaja brief: a portrait Bazaar Bastion fortress scene with the Crown
Spark shrine and the Bijli, Pehel and Maya silhouettes. The prompt explicitly prohibited
vehicles, karts, racing-track motifs, copied characters, copied arena layouts, logos, text
and watermarks. The Unity build entry point now references this file for the menu and mode
backdrops. The older `BattleRaja-FeatureArt-Candidate.png` is retained as historical source
material but is not referenced by the V1 runtime or build. This is still a presentation
candidate pending human originality, cultural-safety, accessibility, composition and
commissioned-art review; it is not a claim of final store art.

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

## Terminal outcome VFX continuation — 2026-08-30

Commit `5d136fb` extends the repository-owned generated set with
`Assets/BattleRaja/Content/Art/V1/VFX/VictoryVfx.prefab` and `DefeatVfx.prefab`, plus their
`VictoryVfxMaterial.mat` and `DefeatVfxMaterial.mat` materials. `ProductionPresentationBuilder`
creates these assets deterministically from local particle recipes (gold Victory, red Defeat)
and attaches them to the three generated fighter prefabs. `OfflineMatchController` routes the
authoritative placement result to the existing `FighterPresentation` adapter; the VFX remain
render-only and cannot change health, placement, cooldown, rewards or match authority.

The six-cue prefab contract and terminal persistence are covered by the exact 141/141
EditMode and 88/88 PlayMode gates in release-plan P47. These are repository-owned generated
technical art assets with no downloaded or third-party source. They are not a claim of final
commissioned VFX, cultural approval, accessibility approval or store-ready art direction.
