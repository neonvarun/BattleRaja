# 06 — Character Concepts, 3D Models, Rigs and Animation

## Context

The repository has generated faceted meshes, small procedural textures, a two-bone rig and saved Animator/VFX cues for Bijli, Pehel and Maya. They are provenance-safe technical scaffolding, but the Lava captures still read as chunky generated placeholders rather than authored game characters. This stage owns the complete editable character pipeline for the V1 roster.

## Objective

At a normal mobile gameplay camera, a player must identify Bijli, Pehel and Maya by silhouette, motion and effect before reading a name. In the fighter screen, each must feel like a finished BattleRaja character with a memorable pose, role, material language and animation personality.

## Current-state audit

Inspect current prefabs, meshes, materials, textures, UVs, rig hierarchy, skin weights, Animator controller/clips, hitboxes, VFX attachment points, portrait crops and `ProductionArtBuilder`/`ProductionPresentationBuilder` provenance. Measure triangles, bones, texture dimensions, material count, bounds and camera-distance readability. Check that render-only children never move authority collision.

## Preserve

Preserve gameplay root/collision/authority anchors, fighter IDs, animator state contract where useful, deterministic generated assets that remain readable, existing provenance and any healthy VFX sockets. Preserve the three-fighter roster and do not change gameplay rules to fit art.

## Replace/fix

Replace primitive-looking bodies, arbitrary faceting, identical proportions/animations, unintegrated concept images, tiny/unclear portraits and materials that flatten under mobile lighting. Fix skinning pops, foot sliding, attack timing that disagrees with authority, bounds/culling errors, z-fighting, excessive materials and visual team markers that rely on hue alone.

## Implementation tasks

1. For each fighter create a one-page concept sheet and turnaround with front/side/three-quarter/silhouette views, role notes and originality/provenance. Use native image generation only for exploration/reference; never ship a generated image as a 3D substitute.
2. Model an editable low-poly production mesh in Blender, Unity tooling or another available native 3D path. Keep clean manifold topology, deliberate bevels, readable planes and a stable origin/scale. Do not simply stretch primitives or reuse a reference character.
3. Author UVs and material regions for face/body/accessory/team-neutral surfaces. Use compact atlases/material instances, hand-authored or procedural textures with provenance and high-contrast variants. Avoid sacred symbols, political marks, stereotypes or direct reference imitation.
4. Build a compact rig: root, pelvis, spine/chest, head, aim/weapon, two arms, two legs, accessory/VFX sockets. Use only the bones needed for the target animation; record weights and export settings.
5. Skin and validate deformations for crouch, dash, charge, throw, decoy and hit poses. Keep gameplay hit/collision volumes separate and authority-owned.
6. Author and integrate at minimum: `Idle`, `Locomotion`, `Aim`, `Attack`, `Ability`, `GadgetUse`, `Hit`, `Knockback`, `CrownPickup`, `CrownCarry`, `CrownDeposit`, `KO`, `Respawn`, `Victory`, `Defeat`, `Spectator` and `EmotePlaceholder` only if it has a V1 use. Each fighter must have different anticipation, recovery, weight and locomotion rhythm.
7. Export/import with deterministic scale, naming, compression and root-motion policy. Configure Animator transitions to follow authority events; never let animation decide damage or movement.
8. Create gameplay and menu portraits/crops, neutral/team outline variants, role badge hooks and a camera test scene. Validate all materials under warm Bazaar, cool Aandhi and high-contrast lighting.
9. Produce an asset manifest with source file, authoring tool/version, license/provenance, triangle/texture/bone counts, LOD policy and known limitations.

## Asset tasks

The following are the required character specifications for the continuation pass.

### Bijli — mobile skirmisher

- **Silhouette/proportions:** lean athletic torso, slightly forward center of gravity, one asymmetrical lightning-shaped shoulder/antenna profile and a compact trailing sash; head/shoulder width must remain distinct at 20–25 m gameplay equivalent.
- **Motif/materials:** fictional storm-runner equipment, matte teal/cyan cloth or polymer, warm brass/gold conductive accents, restrained emissive cyan only on attack sockets; no real-world sacred/political mark.
- **Palette:** cyan/teal body, saffron-gold accent, deep ink outline; team overlay is a separate neutral rim/shape.
- **Topology/LOD:** target 8–12k triangles LOD0, 4–6k LOD1, ≤2k LOD2; one 1024 atlas or two 512 atlases; no more than two skinned materials in gameplay.
- **Rig/animation:** light footwork, readable dash anticipation and landing, quick attack recoil, clear recovery; keep weapon/bolt socket stable during aim.
- **VFX/portrait:** bolt trail, dash ribbon, hit spark and Crown carry arc with reduced-flash alternatives; portrait must show face/accessory silhouette, not only a color tile.

### Pehel — frontline bruiser

- **Silhouette/proportions:** broad shoulders/hips, low stable stance, large readable gauntlet/brace profile and a weighty scarf/strap; must never collapse into Bijli's narrow outline.
- **Motif/materials:** fictional bazaar guardian/repairer, warm clay/terracotta armor plates, cream fabric, dark rubber/leather-like joints and controlled metallic wear; no caricature or militarized community costume.
- **Palette:** rust/clay, cream, muted teal utility marks, ink outline; team overlay separate.
- **Topology/LOD:** target 10–14k triangles LOD0, 5–7k LOD1, ≤2.5k LOD2; one 1024 atlas or two 512 atlases; two skinned materials maximum.
- **Rig/animation:** planted locomotion, charge wind-up, sweep/throw arcs with visible weight, hit stagger and landing recovery; avoid foot sliding and arm intersection.
- **VFX/portrait:** ground sweep, capture/throw ring, impact dust, guard/knockback cues and a clear portrait with the gauntlet profile.

### Maya — trickster/control

- **Silhouette/proportions:** medium, angular cloak/hood or layered shoulder silhouette with a deliberately offset decoy accessory; body and decoy must differ by outline/marker, not only color.
- **Motif/materials:** fictional illusionist tinkerer, violet/mint fabric, rose enamel/glass accents, ink underlayers and restrained translucent material only where it remains readable on low quality.
- **Palette:** deep violet, mint, rose, pale highlight and ink; avoid making team color the primary Maya identity.
- **Topology/LOD:** target 9–13k triangles LOD0, 4–6k LOD1, ≤2.5k LOD2; atlas ≤1024; transparent surfaces must be limited and sorted safely.
- **Rig/animation:** sly idle, offset locomotion, shard release, decoy spawn/vanish, hit/KO with a readable real-body silhouette; decoy animation must not imply a second player without an icon.
- **VFX/portrait:** shard ribbon, decoy outline, false trail and control pulse with reduced-flash/shape alternatives; portrait must communicate the asymmetrical cloak/hood.

## Integration points

Integrate prefabs with fighter definitions, authority sockets, animator state IDs, gadget/ability events, Crown markers, team overlays, camera framing, UI portraits, VFX/audio attachment and `BazaarBastion.unity`. Keep collision and nav geometry unchanged unless prompt 07 explicitly updates and tests it.

## Performance constraints

Target the triangle/material/texture limits above as a starting budget; report measured deviations. Use LOD cross-fade only if it does not shimmer, avoid runtime shader compilation, keep skinned bones ≤24 per fighter where practical, pool VFX and avoid per-frame material instantiation. Profile eight fighters together on Lava.

## Tests

Add asset import/validation tests for scale, bounds, required bones, clips, sockets, materials, LODs, atlas size, prefab references and missing textures. Add PlayMode checks that authority state still controls movement/attack/KO and no render child changes collision. Add deterministic animation-event integration tests where applicable.

## Visual QA

Inspect each fighter at selection, 20–25 m gameplay camera, close portrait, warm/cool lighting, team/enemy overlay, Crown carry/deposit, all core states, reduced flashes and high contrast. Compare silhouettes as a three-fighter lineup; reject any pair that is distinguishable only by color or name.

## Lava verification

Install the final-art candidate on Lava `ST5GDW23LB004392`. Capture selection cards and continuous gameplay for each fighter through move, attack, ability, Crown action, hit, KO, respawn, victory and defeat. Check frame pacing, texture shimmer, clipping and readability in landscape/compact safe area; never use Oppo.

## Failure cases

Test missing material/texture, broken rig import, scale mismatch, bind-pose flash, foot sliding, animation event after KO, VFX socket missing, decoy indistinguishable, team outline hidden by lighting, LOD popping, transparent sorting, low-memory texture fallback and prefab/collision drift.

## Binary acceptance gate

Pass only when all three fighters have original concept/provenance, editable modeled meshes, clean UV/materials, validated rig/skin, distinct animation sets, LODs, portraits, VFX sockets and gameplay-camera proof on Lava; no placeholder/primitive body remains in the player path; and authority/collision/tests remain green. A concept image, recolored primitive or static preview is not complete.

## Evidence to retain

Concept/turnaround files, source 3D files, imported prefabs, manifest/provenance, triangle/material/bone report, animation checklist, validation/test output, camera-distance lineup and Lava captures with build/hash/settings.

## Non-scope

Do not add a fourth fighter, online skins, progression cosmetics, copied reference assets, new gameplay rules, map remodeling or store screenshots beyond necessary asset proof.

## Stop condition

Stop before prompt 07 if any fighter lacks a real model/rig/animation, if silhouettes collapse at the gameplay camera, if assets are unlicensed/unprovenanced, if collision changes are untested or if the Lava candidate shows severe clipping, shimmer or frame-time regressions.
