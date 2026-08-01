# BattleRaja Agent Instructions

## Mandatory reading

Before significant work, read:

- `Docs/MASTER_VISION.md`
- `PROJECT_STATUS.md`
- `Docs/DECISIONS.md`
- `Docs/ARCHITECTURE.md`
- `Docs/RESEARCH_LOG.md`

`Docs/MASTER_VISION.md` is the authoritative product and engineering brief.

## Product

BattleRaja is an Android-app and browser-Web stylised top-down 3D **micro battle royale**. It must be an original game, not a clone of Brawl Stars, Smash Karts or another reference title.

## Active scope

Work only on the active milestone recorded in `PROJECT_STATUS.md`.

Never attempt the complete live-service game in one pass. Do not prebuild systems for later milestones unless the active task explicitly requires them.

## Architecture

- Keep the pure gameplay/domain simulation independent of Unity UI, Photon, PlayFab, animation and scene-specific objects.
- Human input and bot decisions must produce common gameplay commands.
- Use data-driven fighter, weapon, gadget, status and match definitions.
- Never store runtime mutable state in shared ScriptableObject assets.
- Avoid global mutable singletons and hidden state.
- Use injectable seeded randomness for gameplay decisions.
- Separate simulation timing from rendering.
- Core rules must be testable without loading a production scene.
- Record material architectural choices in `Docs/DECISIONS.md`.

## Unity

- Use only the Unity version approved during Milestone 0.
- Use URP unless an approved decision record changes it.
- Do not update Unity or package versions silently.
- Do not blindly hand-edit large scene, prefab or asset YAML files.
- Use safe editor tooling or controlled Unity automation for structural changes.
- Do not commit `Library`, `Temp`, `Logs`, generated builds, IDE caches or secrets.

## Android, Web and performance

- Android and browser Web are primary product platforms.
- Target mid-range Android devices, not only the owner's OnePlus 11R.
- Treat modern desktop browsers as the initial Web target; mobile Web remains experimental until tested.
- Maintain separate Android and Web build profiles and smoke tests.
- Avoid per-frame allocations in hot paths.
- Pool projectiles, VFX and repeated transient objects.
- Preserve critical telegraphs on every quality tier.
- Profile before claiming an optimisation.
- Run physical-device Android validation and multi-browser Web validation when tooling is available.
- Treat browser lifecycle, memory, download size and background-tab behaviour as first-class constraints.

## Networking

- Do not add Photon during the offline foundation milestones.
- No Photon dependency may enter the core domain layer.
- Never trust client-reported damage, cooldowns, movement, pickups, rewards or match results.
- Public competitive matches require a trusted authoritative simulation.
- Never make a browser tab the trusted public-match authority.
- Networking work must include latency, jitter, packet-loss and reconnection tests.

## Backend and economy

- Do not add PlayFab before the approved backend milestone.
- Never include service secrets in client code or the repository.
- Valuable progression and rewards must be server-owned.
- The game must not sell combat power.

## Testing

- Add tests for gameplay rules and regression fixes.
- Run relevant EditMode and PlayMode tests.
- Compile after material changes.
- Build Android before declaring an Android milestone complete.
- Build and locally serve a Web smoke build before declaring a cross-platform milestone complete.
- Report exact commands, results, warnings and untested areas honestly.
- A feature is not complete merely because code was generated.

## Research

- Use current official primary documentation before selecting SDK versions, APIs, packages, policies or deployment methods.
- Log sources, dates, claims and decision impact in `Docs/RESEARCH_LOG.md`.
- Do not rely on stale model memory for current Unity, Android, Photon, PlayFab or Codex behaviour.

## Culture and originality

- Follow `Docs/CULTURAL_GUIDE.md` and the cultural requirements in `Docs/MASTER_VISION.md`.
- Use fictional inspiration rather than direct sacred, political or community caricature.
- Do not copy protected characters, arenas, sounds, UI or distinctive art from reference games.

## Git

- Keep one focused feature per branch or pull request.
- Prefer small descriptive commits.
- Do not combine unrelated generated binary assets and code refactors.
- Update documentation and decision records.
- Never rewrite history or delete user work without explicit approval.

## Safety and authority

You may create code, tests, documentation, editor tooling, placeholder assets and local builds.

Human approval is required before:

- Publishing or deploying publicly
- Purchasing services or assets
- Accepting legal terms
- Deleting production/user data
- Rotating or exposing credentials
- Choosing final branding or trademarks
- Adding paid infrastructure
- Store submission

## Completion report

End each substantial task with:

- Summary
- Changed files
- Commands executed
- Tests and builds
- Warnings/errors
- Assumptions
- Known limitations
- Decisions requiring human approval
- Recommended next single task
