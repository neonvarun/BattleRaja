# 10 — UI/UX, Menu, HUD, Team Signals and Results

## Context

The current `ProductionFlowController` and `OfflineMatchHud` build a functional runtime UI with settings, safe-area anchors and lifecycle handling, but the visible menu/HUD is sparse, oversized and still uses Solo/debug vocabulary. This stage creates the coherent mobile product surface for Bastion Crown.

## Objective

A new player should understand the mode, choose a fighter, control a match, read team/objective state, pause safely and rematch without friction. UI must be legible on a normal Android phone, support left-handed/reduced-flash/high-contrast/text-scale settings and never expose a broken Online path.

## Current-state audit

Inspect flow state machine, runtime-created canvas hierarchy, safe-area/CanvasScaler behavior, button hit targets, text/font assets, menu feature art, fighter previews, HUD strings, controls, results, settings persistence, pause/lifecycle and all aspect-ratio tests. Capture current menu, fighter selection, live HUD and results on Lava.

## Preserve

Preserve local offline navigation, tutorial/settings/rematch routes, safe-area and CanvasScaler foundations, input/lifecycle hardening, aim assist, left-handed/reduced-flash/high-contrast/text-scale/music/effects/haptics preferences and authority-owned results. Keep future networking internal and hidden.

## Replace/fix

Replace sparse prototype composition, giant translucent controls, `ALIVE`/`ZONE`/`MATCH SPAWN SHIELD` Solo labels, mode ambiguity, weak card hierarchy, clipped text, inaccessible contrast and any Online button/spinner in the public V1 flow. Do not use screenshots or layout clones from reference products.

## Implementation tasks

1. Establish a design system: safe-area grid, spacing, type scale, button states, panel surfaces, neutral/team colors, shape/icon redundancy, focus/pressed/disabled/error states and localization-safe copy length.
2. Build the flow: splash/loading → main menu with dominant `PLAY OFFLINE` → Bastion Crown explanation → fighter selection → ready → match → pause/settings → result → rematch/menu. Tutorial is always reachable; no login/account/network gate.
3. Build fighter cards with original portraits, silhouette, role, attack/ability/gadget summary, difficulty and team-neutral outline. Use readable 1×/2×/3× exports and text scale.
4. Build the match HUD: Raja/Rival score, team ticket pips, Crown socket/carrier/drop/deposit state, Aandhi clock, player health/status, ally health/role/intent pips, ability/gadget cooldowns, aim/attack controls, pause and brief spectating/respawn state. Keep the hierarchy compact and hide secondary stats during combat.
5. Add low-noise team signals: defend/escort/contest/regroup/retreat icons and short haptic/audio cues. Signals must not require chat/network and must not block aim/input.
6. Build result and rematch surfaces: winner/tie, team score, deposits, KOs, assists, damage/healing, objective time, gadget/ability use, tickets spent, highlight and new-seed rematch. Provide clear error/retry/back states.
7. Make all surfaces responsive for landscape/tall aspect ratios, display cutouts, large text, left-handed controls, high contrast and reduced flashes. Add UI automation IDs or test hooks without release debug labels.

## Asset tasks

Create an original logo lockup, app/menu background treatment, mode card, three fighter portraits, team/score/ticket/Crown/shrine/role/ability/gadget/respawn/Aandhi icons, button states, panel textures, loading graphic, results badges and safe-area control rings. Provide vector/source files, raster exports, high-contrast/reduced-flash variants and provenance. No copied reference fonts/icons/trade dress.

## Integration points

Integrate with `ProductionFlowController`, mode/flow contracts, fighter definitions, authority result snapshots, team AI signals, Crown/ticket/Aandhi events, VFX/audio/haptics, local preferences, `OfflineMatchController`, tutorial and Android safe-area APIs.

## Performance constraints

Avoid rebuilding the entire canvas every frame, runtime font/material churn, per-frame allocations and unbounded damage-number objects. Pool transient UI, atlas icons, cap text updates to state changes and measure canvas rebuild/layout cost on Lava.

## Tests

Add EditMode/PlayMode flow tests for every navigation/error/rematch transition, UI binding tests for score/tickets/Crown/Aandhi/results, safe-area/aspect/text-scale/left-handed/high-contrast/reduced-flash tests, input/lifecycle tests and no-online/no-login assertions. Preserve existing tests.

## Visual QA

Inspect every important screen in motion, not only a screenshot: logo/menu, mode explanation, cards, loading, live HUD, controls, pause, settings, spectator/respawn, result/rematch and tutorial. Check hierarchy, clipping, touch size, readable team colors/shapes, no debug labels and no overlay hiding the objective.

## Lava verification

On Lava `ST5GDW23LB004392`, cold-launch in airplane mode and traverse every flow with all three fighters. Toggle every setting, rotate/resize where supported, pause/resume, die/respawn/spectate, score/deposit, finish and rematch. Capture 16:9/tall safe-area states and use only this phone for evidence.

## Failure cases

Test no Canvas/EventSystem, missing portrait/icon, long text, extreme text scale, display cutout, orientation change during loading, rapid double tap, pause during channel/respawn, lifecycle input leak, missing result snapshot, rematch while loading, airplane mode and stale preference values.

## Binary acceptance gate

Pass only when the complete offline 4v4 flow is coherent, responsive, accessible, touchable and visibly original; HUD communicates score/tickets/Crown/Aandhi/ally state; result/rematch resets correctly; Online is not public; automated UI/flow tests pass; and every major screen is verified on Lava. A functional but prototype-looking canvas is a fail.

## Evidence to retain

Design tokens/component inventory, source UI/icon/portrait assets and provenance, screen-flow map, automated test output, aspect/safe-area matrix, UI performance sample and Lava screenshots/video with build/hash/settings.

## Non-scope

Do not add online lobby/social/economy, copied UI, localization beyond layout-safe copy unless already supported, new game rules or final audio mix.

## Stop condition

Stop before prompt 11 if any critical screen clips, cannot be operated with one hand, hides team/objective state, exposes Online, fails accessibility/lifecycle tests or still reads as debug UI on Lava.
