# BattleRaja V1 Art Bible

**Status:** V1 production-presentation baseline now includes saved faceted fighter meshes,
deterministic UVs, lightweight two-bone primary skins, transform rigs, Animator
clips/controllers and particle VFX cues; final authored art direction, cultural review and
human feel approval remain open.

## Identity

BattleRaja is a colourful stylised toy-box arena: warm sandstone, teal shadow planes, saffron
and rose cloth, and electric cyan accents. The world is a fictional bazaar fortress inspired
by broad South Asian colour, craft and street-market energy without depicting a sacred site,
real community, political symbol or historical character.

## Readability rules

- The camera is elevated and combat is readable at a small phone viewport.
- Every fighter has a distinct height/width rhythm, colour family and accessory profile:
  Bijli is narrow and angular, Pehel is broad and grounded, and Maya is asymmetric with a
  trailing scarf and crystal accents.
- Gameplay colour rings, health bars and telegraphs remain higher contrast than decoration.
- Render-only art never owns colliders, health, damage, movement or action success.
- Reduced-flash mode preserves the same information with shape, contrast, timing and audio.
- Materials use a restrained palette and matte-to-satin variation; no noisy texture detail is
  required for the first readable mobile tier.

## V1 editable asset pipeline

`Assets/BattleRaja/Editor/ProductionArtBuilder.cs` is the reproducible Unity Editor mesh and
prefab generator. `Assets/BattleRaja/Editor/ProductionPresentationBuilder.cs` adds the
reproducible transform-rig, two-bone primary skin, Animator and particle-cue layer. It
creates custom low-poly mesh assets under `Assets/BattleRaja/Content/Art/V1/Meshes`,
material assets under `.../Materials`, and render-only fighter prefabs under
`Assets/BattleRaja/Content/Prefabs/Production`.

The prefabs are deliberately made from saved mesh assets rather than runtime primitive
creation. Scene generation assigns all three prefabs to `FighterPresentation`; the active
fighter controller selects which model is instantiated. The prefabs contain no colliders or
gameplay scripts. Their `ProductionRig` hierarchy is presentation-only, and the shared
`FighterProduction.controller` selects nine visual states from the presentation state integer.
The primary `Body` (Bijli/Pehel) or `Cloak` (Maya) is copied into a saved
`SkinnedMeshRenderer` with hips/chest bind poses and blended weights; accessory parts remain
static saved render-only children. All generated meshes carry deterministic UVs so a future
authored texture pass does not require rebuilding the gameplay layer.
Particle cues are saved under `Art/V1/VFX` and are played only from existing presentation
notifications; particle lifetime never owns damage, cooldowns or action success.

## Current faceted silhouette pass — 2026-08-28

The explicit `BattleRaja/Rebuild V1 Production Fighter Art` editor action in commit
`816d9ac` replaces the former primitive-like body/accessory recipes with saved faceted
low-poly loft and extruded-polygon profiles. Bijli now has an angular electric torso,
visor, bolt badge, shoulder orbs, arm guards and sculpted boots; Pehel has a broad grounded
torso, sash plate, medallion, gauntlets and boots; Maya has a tapered cloak, mask plate,
scarf ribbons, trim and crystal core. These profiles are authored in the repository-owned
generator and remain render-only children of the existing presentation rig.

The PlayMode regression requires at least 260 combined mesh vertices per instantiated
production silhouette, exactly three distinct mesh profiles, UV coverage on every mesh and
one two-bone skinned primary per fighter. This is a machine-checked quality floor and a
stronger saved baseline, not approval of final commissioned models, authored skinning,
animation, VFX direction, cultural presentation or mobile performance.

## UV and lightweight skin continuation — 2026-08-29

Commit `bc392fd` adds deterministic planar/cylindrical UV generation to every mesh emitted
by `ProductionArtBuilder`. `ProductionPresentationBuilder` now derives and saves
`BijliSkinBody.asset`, `PehelSkinBody.asset` and `MayaSkinCloak.asset`, assigns hips/chest
bind poses and waist-blended weights, and disables only the source primary renderer while
retaining its MeshFilter for reproducible rebuilds. The skin is presentation-only: it owns
no collider, input, health, damage, movement or action-success state. Full EditMode and
PlayMode regressions verify UV lengths, bind-pose/bone parity and non-zero two-bone blends.

## Current inventory

| Asset | Editable source | Unity output | Status |
| --- | --- | --- | --- |
| Bijli | `ProductionArtBuilder.cs` profile + `ProductionPresentationBuilder.cs` rig/clip recipe | `BijliProduction.prefab`, `BijliAttackVfx.prefab`, `BijliAbilityVfx.prefab` | Saved V1 presentation baseline; final authored art/feel review open |
| Pehel | `ProductionArtBuilder.cs` profile + `ProductionPresentationBuilder.cs` rig/clip recipe | `PehelProduction.prefab`, `PehelAttackVfx.prefab`, `PehelAbilityVfx.prefab` | Saved V1 presentation baseline; final authored art/feel review open |
| Maya | `ProductionArtBuilder.cs` profile + `ProductionPresentationBuilder.cs` rig/clip recipe | `MayaProduction.prefab`, `MayaAttackVfx.prefab`, `MayaAbilityVfx.prefab` | Saved V1 presentation baseline; final authored art/feel review open |
| Bazaar Bastion | `BazaarBastionVisuals` and scene generation tooling | authored scene + existing architecture prefab | Visual polish and collision-overlay review open |
| Gadgets | `ProductionArtBuilder.cs` profiles and material recipes | `UmbrellaProduction.prefab`, `DholProduction.prefab`, `TiffinProduction.prefab` | Saved V1 identity prefabs; final use-state/VFX review open |

## Animation and VFX inventory

`Assets/BattleRaja/Content/Art/V1/Animation/FighterProduction.controller` contains the
shared nine-state presentation controller. Its editable clips are saved in
`Assets/BattleRaja/Content/Art/V1/Animation/Clips`: Idle, Locomotion, Attack, Ability, Hit,
Knockback, Eliminated, Victory and Defeat. Each fighter prefab contains the named
`ProductionRig/Root/Hips/Chest` chain with hand, head and foot joints. The primary body or
cloak is rendered by the saved two-bone `SkinnedMeshRenderer`; accessory meshes remain
render-only children of the rig joints.

Saved VFX prefabs cover fighter attack/ability signatures, hit, elimination, gadget use,
healing, shield, zone warning, zone closing and final-circle cues. They use local particle
systems with bounded bursts and no physics or gameplay callbacks. This is a controlled
Unity-generated presentation baseline, not a claim that a human-authored sculpt, production
skinning pass, final VFX direction or cultural review has been approved.

The four images under `Art/Concepts` are directional references only. They are not shipped
gameplay art and must not be presented as screenshots or final asset provenance.
