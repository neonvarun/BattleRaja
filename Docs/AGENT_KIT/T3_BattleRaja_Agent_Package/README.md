# BattleRaja T3 Agent Kit

This is the portable handoff package for a fresh Codex/Goal-mode agent continuing
BattleRaja V1 from the current offline Android release-candidate branch. It is tracked
in the repository so another agent can clone the project and start without relying on
this conversation.

## Contents

- `T3_FRESH_AGENT_START_PROMPT.md` — paste this into a fresh Goal-mode session.
- `SKILLS_AND_MCP_MANIFEST.json` — machine-readable skill, plugin and environment map.
- `MCP_INSTALL.md` — exact plugin IDs, skill IDs and host setup notes.
- `Tools/AgentKit/Install-T3AgentKit.ps1` — safe local preflight/copy helper.

The authoritative product contract remains `PROMPTS/99_MASTER_V1_GOAL.md` plus
`PROMPTS/README.md` and prompts `01`–`16`. The latest repository state always wins over
the snapshot described here.

## Fresh-agent bootstrap

From a clean clone:

```powershell
git clone https://github.com/neonvarun/BattleRaja.git
Set-Location BattleRaja
git fetch origin main
git switch main
pwsh -File Tools/AgentKit/Install-T3AgentKit.ps1 -ProjectRoot (Get-Location).Path
```

If the checkout already contains newer work, keep its actual `HEAD`; do not reset it to
the branch snapshot above. The helper validates the project contract and prints the
plugin/skill manifest. With `-Destination <explicit-folder>` it copies only this kit to
that folder; it never deletes files, changes credentials or edits the Codex host registry.

In the Codex app, install or enable the plugin IDs in `MCP_INSTALL.md`, then paste
`T3_FRESH_AGENT_START_PROMPT.md`. MCP/plugin installation is host-level, so the kit can
declare exact IDs and verify the project but cannot silently install services into a
fresh Codex account.

## Operating rules

- Work only on the active milestone in `PROJECT_STATUS.md` (currently M11 external
  gates); audit before implementing and keep one focused checkpoint per commit.
- This handoff intentionally keeps one integration line: `main` locally and on
  `origin`. Do not create or publish extra branches for routine continuation; commit
  focused checkpoints to `main` and remove any temporary local branch after integration.
- Preserve user work, stashes and healthy systems. Never use `git reset --hard`, broad
  `git clean -fdx`, or recursive deletion of Unity/generated directories as a shortcut.
- Keep the offline core free of Photon, PlayFab, accounts, ads, IAP, network permissions
  and copied reference-game assets.
- Use the approved Lava `ST5GDW23LB004392` for physical evidence; never use the Oppo
  serial recorded in the QA docs.
- Routine local engineering needs no approval. Final signing, permanent identity,
  legal/privacy acceptance, Play submission and public deployment remain owner-only.
- End every substantial checkpoint with exact commits, files, commands, tests, builds,
  hashes, device evidence, warnings, limitations and one recommended next task.

## Current handoff pointer

The last verified branch tip at package creation is `9b3e01a`. It contains the
authority-boundary hardening and friendly allied-squad HUD checkpoint documented in
`Docs/QA/V1_AUTHORITY_PICKUP_TERMINAL_BOUNDARIES_2026-09-05.md`. A fresh agent must
rebaseline and use the actual current tip before acting.
