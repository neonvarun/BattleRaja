# BattleRaja V1 Luna Max Prompt Pack

**Pack version:** 1.1 continuation update
**Prepared:** 2026-09-01 00:03 IST
**Source snapshot used to update the pack:** `56313096d0ad8e2e23468d004eaa77d71ed3a233`
**Primary mode:** `BR_BastionCrown_V1` — **Bastion Crown**, offline 4v4

This directory is the execution specification for a fresh Luna Max Goal-mode session.
The latest source already contains a real Bastion Crown 4v4 implementation checkpoint;
the pack now describes continuation work: audit the implementation, harden missing rules,
complete the authored product, and prove the release gates. It is not a claim that the
game is player-ready or Play-submittable.

## Current checkpoint (must not be reimplemented blindly)

At `5631309`, the source includes `BastionCrownContracts`, `BastionCrownMatch`, a Unity
adapter, canonical actor IDs 1–8, team HUD/objective telegraphs, basic role-aware bot
intent, a regenerated Bazaar Bastion scene, and passing local rerun evidence (148/148
EditMode and 94/94 PlayMode). The implementation is still a hybrid between the new team
state and the legacy combat simulation, and the following are not proven: complete squad
coordination, damage-interrupted deposits, socket-rotation semantics, Bastion replay/soak
determinism, final authored assets/audio, sustained physical-device performance, physical
16 KB runtime compatibility, permanent identity/signing, and Play/legal materials.

The continuation agent must preserve healthy work, reproduce the baseline, and fix or
evidence these gaps before adding polish. If the repository has advanced beyond this
commit, the actual current source wins and this README must be refreshed.

## Start here

1. Rebaseline Git, Unity, tests, current APK/AAB and approved Lava device; preserve all user work. Start from the latest actual `HEAD`, not the snapshot above.
2. Read `Docs/AI/UnityProjectContext.md`, `Docs/AI/V1_PRODUCT_REBUILD_AUDIT.md`, `Docs/AI/V1_REFERENCE_DESIGN_MATRIX.md`, `Docs/AI/PROMPT_REWRITE_MANIFEST.md`, `AGENTS.md`, `PROJECT_STATUS.md`, `Docs/MASTER_VISION.md`, `Docs/ARCHITECTURE.md`, `Docs/DECISIONS.md`, `Docs/CULTURAL_GUIDE.md` and the current release/QA docs.
3. Execute prompts `01` through `16` in order. Each stage must first audit what the latest source already provides and then implement only missing or unsafe work. `99_MASTER_V1_GOAL.md` is the orchestration prompt to paste into a fresh Goal-mode session; it is not a separate stage.
4. At every stage, keep a small evidence report and do not advance while a binary gate fails.

## Canonical product contract

- **Teams:** Team Raja = actor 1 human + actors 2, 3 and 4 friendly AI. Rival = actors 5–8 enemy AI. Exactly eight slots; no accidental ninth actor.
- **Mode:** Bastion Crown is one original objective-combat mode. It is not a rule-for-rule copy of Gem Grab, Brawl Ball, Knockout, Hot Zone, Smash Karts or any other product.
- **Arena:** one authored `32 m × 32 m` walkable flagship map with west/east spawn banks, three Crown sockets and one shrine per team. Collision/authority geometry remains canonical.
- **Clock:** 3-second ready phase, 240-second live clock, deterministic maximum 30-second overtime. Aandhi warns at 180 seconds and contracts toward the active objective/shrine region.
- **Objective:** a neutral `Crown Spark` rotates through the three sockets every 35 seconds or after a deposit. A carrier moves 12% slower; a defeat drops the Crown; a 1.25-second interruptible channel at the allied shrine deposits it for +3 team score.
- **Combat score:** a confirmed KO is +1. Assists are recorded but do not double-count score. First team to 15 wins; otherwise score at time expires decides.
- **Tickets/respawn:** each team starts with 12 shared tickets. A defeated fighter has 4 seconds of spectator/respawn presentation and returns after 5 seconds if a ticket remains; the ticket is consumed on respawn. Spawn protection is 2.5 seconds or until that fighter deals damage. Exhausted fighters remain spectators; a team wipe (all four slots out with no valid pending return) ends the match, while simultaneous KOs with queued respawns do not.
- **Fairness:** friendly fire is off, allied collision is soft, team colors/markers are redundant with shape/icon cues, and bots never receive hidden damage/vision/cooldown cheats.
- **Tie break:** at live-clock expiry compare score first, then deposits → KOs → tickets remaining; if still tied, enter overtime and use the sudden-death Crown result. Overtime ends on a deposit, team wipe or 30-second cap.
- **Stats/rematch:** show KOs, deaths, assists, damage, healing, pickups, deposits, objective time, gadget/ability uses and tickets spent. Rematch keeps local settings/fighter choice, creates a new seed and resets all match state.
- **Roster:** Bijli (mobile skirmisher), Pehel (frontline bruiser), Maya (trickster/control). Do not add a fourth fighter unless playtest evidence proves the three-role roster cannot produce fair teams; a fourth is not a default requirement.
- **Content:** all three gadgets — Umbrella Guard, Dhol Burst and Tiffin Station — are mandatory and must have team/objective uses.

If a later prompt appears to change a canonical value, treat this README and prompt 03 as the conflict to resolve. Update this README, the relevant decision record, tests and evidence before proceeding.

## Stage contract

Each stage prompt includes the same minimum sections: Context, Objective, Current-state audit, Preserve, Replace/fix, Implementation tasks, Asset tasks, Integration points, Performance constraints, Tests, Visual QA, Lava verification, Failure cases, Binary acceptance gate, Evidence to retain, Non-scope and Stop condition. The implementation agent must fill the evidence paths with real outputs, not placeholders.

## Reference and originality rules

The installed Brawl Stars and Smash Karts apps may be observed on Lava for high-level principles such as hierarchy, immediacy, readability, toy-like clarity and frictionless replay. Never decompile, extract, intercept, purchase, copy or ship their characters, maps, UI, icons, sounds, VFX, terminology, timings or trade dress. Use `Docs/AI/V1_REFERENCE_DESIGN_MATRIX.md` as the translation boundary.

## Technical guardrails

- Keep pure C# domain/application code independent of Unity, UI, Photon, PlayFab and SDKs.
- Human and bot decisions produce common authority commands. Runtime mutable match state belongs to authority, not shared ScriptableObjects.
- Preserve deterministic seeded randomness, fixed-step simulation, replay identity and Solo compatibility where healthy.
- Do not add Photon, PlayFab, accounts, matchmaking, social, shop, ads, IAP, online leaderboards or Web release work to V1.
- Use only original/provenance-recorded assets. Images are concept/reference material, never a substitute for gameplay models.
- Do not change collision to make art fit without authority tests.
- Do not use Oppo for evidence. Physical Android evidence is Lava `ST5GDW23LB004392` only.

## Evidence convention

Use a timestamped directory under `Builds/Local/PlanningAudit/BattleRaja/` or the existing QA/build evidence layout. Retain commit/build identity, test command/output, screenshots/video where useful, device/API, settings, hash and known limitations. Do not commit raw captures or generated build folders unless repository policy explicitly requires them.

## Definition of done for the pack

The pack is useful only when a fresh agent can follow it without guessing what 4v4 means,
what is already implemented, what to preserve, what to build, how to test it, or when to
stop. A final report must distinguish implemented evidence, unverified areas and owner-only
gates. Never call the project a Play Store Release Candidate solely because the current
candidate APK launches or the legacy Solo suite is green.
