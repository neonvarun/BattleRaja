# BattleRaja V1 Art Bible

**Status:** V1 production-presentation baseline now includes saved transform rigs, Animator
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
reproducible transform-rig, Animator and particle-cue layer. It creates custom low-poly mesh assets under `Assets/BattleRaja/Content/Art/V1/Meshes`,
material assets under `.../Materials`, and render-only fighter prefabs under
`Assets/BattleRaja/Content/Prefabs/Production`.

The prefabs are deliberately made from saved mesh assets rather than runtime primitive
creation. Scene generation assigns all three prefabs to `FighterPresentation`; the active
fighter controller selects which model is instantiated. The prefabs contain no colliders or
gameplay scripts. Their `ProductionRig` hierarchy is presentation-only, and the shared
`FighterProduction.controller` selects nine visual states from the presentation state integer.
Particle cues are saved under `Art/V1/VFX` and are played only from existing presentation
notifications; particle lifetime never owns damage, cooldowns or action success.

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
`ProductionRig/Root/Hips/Chest` chain with hand, head and foot joints; meshes remain
render-only children of those joints.

Saved VFX prefabs cover fighter attack/ability signatures, hit, elimination, gadget use,
healing, shield, zone warning, zone closing and final-circle cues. They use local particle
systems with bounded bursts and no physics or gameplay callbacks. This is a controlled
Unity-generated presentation baseline, not a claim that a human-authored sculpt, skinning
rig or final VFX pass has been approved.

The four images under `Art/Concepts` are directional references only. They are not shipped
gameplay art and must not be presented as screenshots or final asset provenance.
