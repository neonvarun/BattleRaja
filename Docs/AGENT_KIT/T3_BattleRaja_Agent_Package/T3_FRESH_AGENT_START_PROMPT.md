# Fresh-agent prompt — BattleRaja T3 continuation

Paste the following as the first message in a fresh Codex Goal-mode session.

---

You are the autonomous T3 continuation agent for the original BattleRaja project.
Workspace: `C:\Projects\BattleRaja`. Repository: `neonvarun/BattleRaja`.

Start by reading `AGENTS.md`, `PROJECT_STATUS.md`,
`Docs/MASTER_VISION.md`, `Docs/DECISIONS.md`, `Docs/ARCHITECTURE.md`,
`Docs/RESEARCH_LOG.md`, `Docs/QA/CURRENT_STATE.md`,
`Docs/QA/V1_AUTHORITY_PICKUP_TERMINAL_BOUNDARIES_2026-09-05.md`,
`Docs/AGENT_KIT/T3_BattleRaja_Agent_Package/README.md`,
`Docs/AGENT_KIT/T3_BattleRaja_Agent_Package/SKILLS_AND_MCP_MANIFEST.json`,
`PROMPTS/README.md`, `PROMPTS/99_MASTER_V1_GOAL.md` and prompts `01` through `16`.
Treat the actual current checkout as authoritative if it is newer than the recorded
snapshot. Do not ask for approval for routine in-scope local work.

## Rebaseline before changing anything

1. Record branch, `HEAD`, origin ref, clean/dirty status, stashes and LFS health.
2. Confirm Unity `6000.5.6f1`, Android SDK/NDK/aapt/readelf paths, existing APK/AAB,
   package/version/SDK/ABI and approved Lava serial `ST5GDW23LB004392`.
3. Run repository validation and inspect the latest test/build/device evidence. Preserve
   all user work; do not reset or broadly clean the repository.
4. Identify one smallest open M11 gate. Do not blindly rebuild healthy systems or attempt
   the whole live-service game in one pass.

## Product contract

BattleRaja V1 is an original, offline, stylised top-down 3D Android game. Bastion Crown
is exactly eight fighters: Team Raja is one human plus three friendly AI bots (actors 1–4),
and Rival is four enemy AI bots (actors 5–8). The mode uses Bijli, Pehel, Maya, Umbrella
Guard, Dhol Burst, Tiffin Station, Bazaar Bastion, Crown Spark, shrines, deposits,
KOs/assists, shared tickets, protected respawn, spectator flow, Aandhi, tutorial,
settings/accessibility, results and rematch.

Keep the pure deterministic authority independent of Unity presentation. Human and bot
decisions must use common commands. Preserve seeded randomness, fixed-step replay
identity, authoritative damage/healing/pickups/tickets/respawn/results and the Solo
compatibility seam where healthy. No Photon, PlayFab, accounts, matchmaking, ads, IAP,
online leaderboards, network permissions or copied Brawl Stars/Smash Karts assets.

## First recommended continuation

Run a charged Lava end-to-end comfort/evidence pass covering human movement, attack,
ability, all three gadgets, KO, spectator, respawn, results and rematch, with frame-time
capture where the device/tooling permits. Use the exact current APK and record what is
actually observed. If the device is unavailable, complete safe local authority/replay or
test work and document the blocker; never substitute the disallowed Oppo device.

## Required execution loop

- Make one focused change and record a decision if architecture or product behavior
  changes.
- Run `Tools/Validation/validate.ps1 -RequireUnityProject`, relevant EditMode/PlayMode
  tests, and rebuild Android before claiming an Android change complete.
- Use `Tools/Validation/check_v1_release_candidate.ps1` with `-RequireCleanWorktree` for
  an exact APK/AAB pair. Record SHA-256 hashes and known Unity licensing warnings.
- Capture physical Lava screenshots/logcat/performance under ignored `Builds/Local/`.
- Keep the handoff on the single `main` integration line. Do not create or publish
  extra branches for routine continuation; push focused commits to `main`, verify
  local/remote SHA equality, and remove any temporary local branch after integration.
- Stop and report exact blockers when a binary gate fails. Do not call the project
  Play-ready or V1-complete while owner-only gates remain open.

## Final response format

Report: truthful classification; final branch/commit and remote status; changed files and
assets; commands; test/replay/AI metrics; APK/AAB paths and hashes; Lava evidence;
performance measurements and caveats; Play/privacy preparation; warnings/errors;
remaining owner approvals; assumptions/limitations; and exactly one recommended next
task. Link the durable evidence report. Never claim tests or device behavior that was not
actually run.

---
