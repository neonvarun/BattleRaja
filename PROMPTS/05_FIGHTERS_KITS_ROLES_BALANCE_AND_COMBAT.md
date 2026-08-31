# 05 — Fighters, Kits, Roles, Balance and Combat

## Context

Bijli, Pehel and Maya already have authority-backed combat definitions and presentation hooks, but they were tuned for Solo Raja. Bastion Crown needs three clearly different team roles, fair objective interactions and readable counterplay without inventing a fourth fighter by default.

## Objective

A player should choose a fighter because its movement, range, risk and team contribution feel meaningfully different. In a 4v4 match each kit must have a useful Crown/shrine/ticket decision, a clear counter and satisfying feedback at mobile camera distance.

## Current-state audit

Inspect `FighterDefinition`, `FighterKits`, projectile/weapon definitions, movement, ability/cooldown validation, damage/knockback/heal, gadget hooks, animation states and all fighter tests. Capture current health, speed, range, damage, cooldown and hitbox values; compare them to team objective and respawn rules. Verify ally filtering and carrier interactions.

## Preserve

Preserve authority ownership, common commands, seeded combat, existing validated hit/damage/knockback semantics, the three-fighter roster, Solo compatibility and useful gadget/cooldown infrastructure. Keep numerical tuning data-driven and documented.

## Replace/fix

Replace color-only differentiation, generic movement and FFA-only balance. Fix attacks that are hard to attribute, abilities that lack counterplay, duplicate role value, carrier interactions that feel punitive, healing/shield exploits and any hitbox that disagrees with the model/collision.

## Implementation tasks

1. Lock role language: Bijli = mobile skirmisher/finisher; Pehel = frontline bruiser/space-maker; Maya = trickster/control/route pressure. A fourth fighter is out unless playtest evidence proves an unfixable support/role gap.
2. Create a role matrix for health, speed, range, burst, sustain, objective carry risk, crowd control, escape, gadget synergy and counterplay. Use normalized budgets rather than arbitrary buffs.
3. Re-tune attacks/abilities for 4v4 with identical base stats across difficulty levels. Make ally targeting, healing and self-damage rules explicit.
4. Give every ability a readable wind-up, active window, recovery, miss behavior, interruption rule and counter. Ensure Crown carrier penalty and respawn protection cannot be abused by any kit.
5. Add combat events for hit, crit/bonus where valid, shield, heal, knockback, Crown drop, KO, assist and ticket spend. Attribute stats once and preserve replay identity.
6. Add deterministic balance scenarios: 1v1, 2v2, 4v4 contest, escort, shrine defense, retreat, Aandhi and low-ticket endgame. Tune toward a few-minute match, not a spreadsheet-only target.
7. Update `Docs/BALANCE_CHANGELOG.md` with each changed constant, reason, expected player effect and evidence.

## Asset tasks

Create/define original fighter role badges, attack/ability icons, cooldown states, carrier overlays and portrait crops. Each kit needs a distinct color-plus-shape language, impact motif and high-contrast/reduced-flash fallback; do not use color alone for team or ability meaning.

## Integration points

Integrate fighter definitions with Bastion Crown authority, team AI role assignment, Crown/ticket events, gadgets, animator parameters, VFX/audio cues, HUD and tutorial. Keep runtime data immutable and per-match state in authority.

## Performance constraints

Use pooled projectiles and transient effects, bounded hit queries and no allocations in attack/ability ticks. Avoid per-frame searches for targets or animator parameters. Profile all three kits together with eight actors and final map density later.

## Tests

Add unit tests for every kit's valid/invalid command, cooldown, range, damage/heal/knockback, ally/enemy filtering, Crown carrier interaction, spawn protection and stat attribution. Add deterministic scenario tests for counterplay, assist eligibility, no duplicate score and replay parity. Add balance simulations with documented ranges and preserve Solo tests.

## Visual QA

Inspect each fighter in selection, gameplay idle/move/attack/ability/hit/KO/respawn/victory/defeat, with and without Crown, at normal and compact camera. The roles must be identifiable from silhouette/motion/effect before reading text. Test reduced flashes, high contrast and similar-team-color situations.

## Lava verification

On Lava `ST5GDW23LB004392`, play one complete or representative match as each fighter, including a Crown contest, ability/gadget interaction, hit/knockback, KO/respawn and result. Record build/seed/settings and capture device video/screenshots; never use Oppo.

## Failure cases

Test ability during spawn protection, gadget while carrying, healing an enemy, projectile through a dead actor, simultaneous KO/score, respawn into blocked space, Maya decoy mistaken for ally/enemy, Pehel knockback across map bounds, Bijli dash through forbidden collision and low-health Aandhi behavior.

## Binary acceptance gate

Pass only when Bijli, Pehel and Maya are mechanically and visually distinct, each has a viable team/objective role and counterplay, stats are deterministic/idempotent, automated scenarios are green, balance changes are recorded and all three kits are playable on Lava. A role that exists only in a card label or color is a fail.

## Evidence to retain

Role/balance matrix, data assets/schema, combat event traces, unit/scenario/simulation output, before/after device captures, balance changelog entries, replay hashes and known limitations.

## Non-scope

Do not add a fourth fighter without evidence, online combat, progression, cosmetic economy, final character modeling/rigging (prompt 06), map dressing or store assets.

## Stop condition

Stop before prompt 06 if any fighter cannot contribute safely to the objective, if kits require hidden numerical cheats, if team/ally filtering is wrong, if combat events are not deterministic or if one fighter is unreadable at gameplay distance.
