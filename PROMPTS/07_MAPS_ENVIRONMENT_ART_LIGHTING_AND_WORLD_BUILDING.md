# 07 — Maps, Environment Art, Lighting and World Building

## Context

`BazaarBastion.unity` and the generated environment builder provide a collision-compatible, readable baseline, but the Lava capture is sparse and greybox-like. Four-versus-four objective combat needs intentional lanes, flanks, cover, shrines and Crown sockets without sacrificing top-down readability or mobile performance.

## Objective

Deliver one flagship authored Bazaar Bastion map that makes Crown decisions legible and movement fun: both teams can leave spawn, contest, escort, intercept, retreat and regroup through multiple fair routes. The map should feel like BattleRaja's fictional toy-box world, not a copied Brawl Stars layout or an untextured test arena.

## Current-state audit

Inspect scene hierarchy, authority collision/walkability, spawn transforms, cover colliders, nav/path representation, camera bounds, lighting, materials, props, textures, occlusion/LOD and generated builder output. Trace which geometry affects simulation and which is render-only. Measure overdraw/draw calls and check current bot route assumptions.

## Preserve

Preserve validated collision and movement bounds until a deliberate map revision has tests. Preserve seeded spawn configuration, camera framing, Aandhi center/radius contract, modular builder/provenance and any landmark that remains readable. Keep Solo compatibility through a separate map/mode definition if needed.

## Replace/fix

Replace repeated primitives, empty expanses, ambiguous cover, dead ends, spawn sightline unfairness, clutter around the objective, flat lighting and any art that moves/occludes authority collision accidentally. Do not polish a collision layout that cannot support fair 4v4 routes.

## Implementation tasks

1. Block a top-down 32 m × 32 m walkable plan with west/east team spawn banks, three Crown sockets (center/north/south), two shrines, three readable lanes and two flank loops. Keep each spawn's first cover and route symmetrical in travel time, not necessarily mirrored in decoration.
2. Validate visibility, time-to-objective, escape routes, cover spacing, projectile lanes, Pehel charge room, Bijli dash safety and Maya decoy ambiguity. Ensure no spawn can see the enemy shrine directly at match start.
3. Build a modular fictional Bazaar kit: sandstone/terracotta walls, painted cloth awnings, repair benches, stacked crates, water/reflective accents, signage-like abstract shapes and neutral banners. Avoid sacred symbols, political marks, stereotypes and recognizable reference props.
4. Author floor, wall, cover, landmark, shrine plinth and Crown socket meshes with clean pivots, UVs, material instances, collision proxies and LODs. Use a consistent scale and grid so future maps can reuse the kit.
5. Dress the map in gameplay-readable layers: walkable ground, soft cover, hard cover, non-walkable border and objective highlight. Keep team colors/markers as overlays, not permanent map tint.
6. Light for warm readable base state and cooler Aandhi state. Bake or precompute where appropriate; avoid high-cost realtime lights/shadows on every prop. Add explicit low-quality fallback that keeps telegraphs and markers.
7. Add deterministic map validation: spawn reachability, cover/path graph, objective socket availability, shrine channel space, camera bounds and no collider/render drift. Update map and architecture decisions with evidence.

## Asset tasks

Produce an asset sheet and provenance for: floor tile families, four wall/corner modules, awning/market stall, repair bench, crate/barrel families, signage panels, shrine plinths, Crown socket pedestals, team-neutral spawn markers, Aandhi boundary, two landmarks and background silhouettes. Each asset needs pivot/scale, texture atlas, material count, LOD0/1/2, collision policy and high-contrast/reduced-flash variant. Target ≤8k triangles LOD0 for large props, ≤2k for small props and ≤1024 textures unless a measured exception is justified.

## Integration points

Integrate with `BazaarBastion.unity`, `BuildEntrypoints`, map/mode definitions, Crown/ticket authority anchors, camera, bot route/perception, Aandhi visual, gadget placement, lighting profiles and quality tiers. Never bind authority to a decorative prefab child.

## Performance constraints

Use static batching/instancing where helpful, pooled/limited particles, atlas materials, occlusion only when measured, LOD without visible pops and no per-frame scene searches. Record draw calls, triangles, overdraw, texture memory, light/shadow cost and load time on Lava with eight fighters.

## Tests

Add EditMode/map-data tests for bounds, three sockets, shrines, eight spawn slots, route lengths and collision proxies. Add PlayMode/nav checks for all fighter movement, dash/charge, projectiles, Crown pickup/deposit and Aandhi. Add multi-seed bot path/cover simulations and regression checks for Solo maps.

## Visual QA

Inspect the full arena at normal gameplay camera, minimap/top-down plan, spawn perspective, each lane/flank, objective court, shrine channel, Aandhi state, low/high quality, reduced flashes and high contrast. Cover must read as cover; decorative clutter must not hide Crown, carriers, allies or telegraphs.

## Lava verification

On Lava `ST5GDW23LB004392`, run the complete 4v4 scene with all three fighters, gadgets and bots. Capture a walk/aim route through every lane and flank, Crown deposit at both shrines, Aandhi contraction and a result. Check camera clipping, touch readability, loading time and thermal/frame behavior; never use Oppo.

## Failure cases

Test missing socket/shrine, unreachable spawn, collider/render mismatch, projectile snag, dash/charge out of bounds, camera edge, blocked bot route, cover that hides objective UI, LOD pop, texture missing, lightmap failure, low-quality telegraph loss and map reload memory growth.

## Binary acceptance gate

Pass only when one authored flagship map supports fair 4v4 routes/objective play, all anchors and collision are validated, no greybox/placeholder art remains in the player view, bots can navigate it, all quality tiers preserve telegraphs, and Lava captures show readable combat across lanes/flanks. A pretty backdrop with untested routes is a fail.

## Evidence to retain

Map plan/measurements, source meshes/materials/textures, provenance/LOD/collision manifest, route/nav metrics, scene/build diff, performance sample, quality-tier captures and Lava video/screenshots with build/hash/settings.

## Non-scope

Do not build a second map unless the flagship passes and time/performance budget remains, do not copy reference maps/props, do not change network/Web mode, and do not redesign fighter kits.

## Stop condition

Stop before prompt 08 if spawns/routes/objective space are unfair or unreachable, if decorative art changes authority collision, if any map tier hides critical telegraphs, or if the final-art scene causes an unexplained device performance regression.
