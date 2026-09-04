# BattleRaja V1 Reference-to-Design Matrix

**Prepared:** 2026-09-01 00:03 IST (continuation update)
**Source checkpoint:** `56313096d0ad8e2e23468d004eaa77d71ed3a233`
**Purpose:** translate high-level, publicly observable product principles into original BattleRaja decisions.
**References:** controlled observation of installed Brawl Stars and Smash Karts on Lava, plus official/current Android and Play documentation.
**Boundary:** references are not asset, code, terminology or trade-dress sources. No decompilation, extraction, traffic interception, purchase, account action or copied screenshot was used.

## Fresh public-entry observation — 2026-09-04

The installed public entry surfaces were re-opened on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 1080x2460). Brawl Stars
(`com.supercell.brawlstars`, 69.230) remained on a landscape illustrated connection/loading
surface; Smash Karts (`com.tallteam.citychase`, 2.15.1) remained on a landscape branded
loading surface. No account, private screen, gameplay route, network inspection, package
extraction or asset reuse was performed. The only translated principles are immediate
orientation, one dominant status cue, high-contrast hierarchy and low-friction loading
feedback. Captures and hashes are isolated under
`Builds/Local/PlanningAudit/References/20260904/README.md`; they are planning evidence,
not BattleRaja production assets.

## How to use this matrix

The implementation agent may adopt the principle and the player benefit, but must solve it with BattleRaja's own characters, names, map geometry, colors, rules, icons, audio and motion. Every row includes an explicit anti-copy guard. If a proposed implementation resembles a reference too closely, redesign the expression while preserving the underlying usability principle.

The “current problem” column was written before the Bastion Crown checkpoint and is a design
diagnostic, not a claim that the latest source is still Solo-only. Re-audit each row against
the actual current screen/build: the team route, objective HUD and basic telegraphs now exist,
while authored final content, squad coordination, accessibility proof and physical-device
evidence remain open.

## Matrix

| Reference principle | Why it works | BattleRaja current problem | Original BattleRaja interpretation | Do not copy | Evidence/verification |
|---|---|---|---|---|---|
| One obvious primary action on the home screen | A new player knows what to do without scanning a store-like menu | The tested menu hierarchy is clear; the historical feature image felt placed behind the UI and was replaced | Make `PLAY OFFLINE` the largest action; explain `Bastion Crown 4v4` in one sentence; keep Tutorial and Settings secondary; use an original shrine/fighter backdrop | Button shape, exact placement, typography, hero art, labels or trade dress | `Builds/Local/V1GameplayTruth/Final/lava-20260901-final/01-launch.png` plus touch route |
| Character-led focal presentation | A single strong subject gives the product a memorable identity | Current previews are small and technical/faceted | Show one authored fighter in a controlled idle vignette, with team-neutral lighting and role tag | Any reference character, pose, costume, background or animation | Menu screenshot and 10-second idle observation |
| Compact, glanceable fighter choice | Players can compare roles quickly | Three cards do not yet communicate role, range or objective value | Cards show silhouette portrait, role rhythm, difficulty, attack/ability/gadget summary and team-neutral outline | Card geometry, iconography, names or exact copy | Fighter-selection device route; text-size variants |
| Team/enemy readability at gameplay scale | Fast decisions require knowing friend from foe before reading names | Current FFA actors use player/enemy hostility; allies do not exist | Use redundant team rim light, banner pennant, outline and HUD health marker; never rely on hue alone | Exact team colors, outline treatment, character shapes or marker icons | 8-fighter stills at camera distance and color/contrast settings |
| Readable arena lanes and cover | Players plan movement and attacks from the camera view | Bazaar baseline is sparse and cover affordances are weak | Build three lanes, two flank loops, safe spawn pockets and an objective court on validated collision | Map layout, props, landmark arrangement or cover silhouettes | Top-down map overlay, navigation tests, Lava live capture |
| A central contest with changing decisions | A match needs a reason to move instead of trading shots at spawn | Solo Aandhi/loot pacing has no team objective | `Bastion Crown` rotates between three sockets; carry, escort, intercept and defend create changing priorities | Gem, ball, hot-zone or capture rules and their names; no rule-for-rule clone | Authority event trace and human match observations |
| Immediate state feedback without clutter | Players understand cause/effect and recover from mistakes | HUD uses Solo/debug vocabulary such as `ALIVE` and `ZONE` | Use a compact team score/ticket strip, crown carrier state, ally health pips and Aandhi clock; details open in pause/results | Exact HUD rails, fonts, icons, sound cues or layout | Screenshots at 16:9, tall phone and text-scale settings |
| Large touch targets with calm hierarchy | Mobile combat is playable under thumbs, not only a mouse | Current controls are functional but oversized/translucent | Preserve left/right handed anchors, attack/ability/gadget separation, aim assist and safe-area padding with clear states | Exact joystick/button art or control positions | Lava touch route; no accidental input after pause/resume |
| Attacks communicate wind-up, travel and impact | Players learn timing and can attribute hits | Existing generated particles prove events but lack authored impact | Give each kit a distinct anticipation, projectile/trail, contact, damage number/flash and recovery language; reduced-flash alternatives | Exact projectile shapes, VFX colors, hit sounds or animation timing | Slow capture, reduced-flash run and combat replay |
| Abilities have personality and counterplay | Cooldowns feel meaningful when players can anticipate and answer them | Current kits work in Solo but team roles/objective use are underdefined | Bijli creates mobility windows, Pehel opens frontline space, Maya creates deception/route pressure; all have readable tells | Any reference ability behavior, names, VFX or timing | Kit tests, bot simulations, device observation |
| Match results explain the story | Players understand why they won and want to rematch | Current results are placement/KOs/DMG for Solo | Show team score, deliveries, KOs, assists, damage/healing, tickets spent, objective time and one honest highlight; one-tap rematch | Exact result layout, medals, fonts, phrasing or progression hooks | Results route, rematch seed change, screenshot evidence |
| Low-friction repeat play | Short loops encourage another match | Rematch exists but is attached to the Solo result model | Rematch retains fighter/settings, uses a new deterministic seed, resets tickets/score, and clearly states the same mode | Any reference matchmaking, social loop, reward economy or button treatment | Ten consecutive local rematches and state reset log |
| Tutorial teaches verbs in context | Players retain mechanics better when each action has a reason | Existing tutorial is a target lane, not a complete team-objective lesson | Teach move → aim → attack → ability/gadget → ally signal → Crown pickup/deposit → revive/respawn → Aandhi; allow replay | Exact tutorial steps, narration, scene dressing or copy | Fresh install tutorial completion on Lava |
| Settings are discoverable and forgiving | Accessibility and comfort are part of the product, not a hidden debug menu | Settings exist but need final visual hierarchy and 4v4 options | Keep reduced flashes, high contrast, text scale, left-handed layout, aim assist, music/effects, haptics and tutorial replay; add team-marker redundancy | Exact toggle presentation, labels or defaults | Every setting toggled before and during a match |
| Audio layers reinforce state | Players can hear danger and success without staring at HUD | Generated PCM WAVs integrate but are not a final mix | Own a compact motif per fighter, objective, Aandhi, team score, respawn and result; mix ducks and respects effects/music sliders | Melody, samples, voice lines, sound signatures or mix copied from references | Headphone/device speaker loudness and clipping test |
| Toy-like low-poly clarity | Simple geometry reads at distance and performs on mobile | Current faceting reads as unfinished rather than intentional | Use controlled low-poly forms with authored proportions, bevels, color blocks, decals and LOD; every silhouette earns its shape | Smash Karts vehicle/prop silhouettes, colors, materials, maps or UI | Camera-distance stills, triangle/texture report |
| Stable top-level navigation | Players build muscle memory for where play/settings/tutorial live | Current runtime UI can shift across orientation/layout states | Define safe-area anchors, portrait fallback, landscape primary layout and deterministic back navigation | Navigation rails, icon set, screen transitions or labels | Matrix of aspect ratios/orientations on Lava |
| Honest offline promise | A no-account game starts immediately and avoids network failure states | Online seams and historical docs can confuse V1 scope | Hide Online from the public V1 menu, keep seams internal, show no fake network spinner or login | Reference social/store screens, account prompts or online labels | Airplane-mode launch and offline match |
| Performance-aware richness | Visual polish only matters if input and frame pacing survive a phone | Current telemetry is bounded and not normalized on final art | Profile a final 4v4 art build; use texture/mesh/particle tiers while preserving telegraphs and objective markers | Any reference optimization implementation or asset | Frame histogram, memory/thermal/battery/endurance report |

## BattleRaja design guardrails derived from the matrix

1. **Originality is a deliverable.** Keep a provenance row for each new asset and a short explanation of why its shape/rule is BattleRaja-specific.
2. **Readability beats decoration.** Team, Crown, tickets, Aandhi and ability states must survive the gameplay camera and reduced-flash/high-contrast settings.
3. **Principles are not clones.** A familiar usability pattern is acceptable; copying a specific screen, mode, character, map, icon, timing or trade dress is not.
4. **Device evidence closes the loop.** Desktop previews and isolated screenshots are useful design checks but cannot replace continuous Lava observation.
5. **Offline is a product feature.** No online mode, account, social rail, analytics upload, ad, IAP or cloud dependency should appear in the V1 player flow.
