# MCP and skill setup for a fresh T3 agent

Codex plugins/MCP servers are installed per host/account. This repository package cannot
silently edit that registry or accept service terms. Use the exact IDs below in the Codex
plugin picker (or the host's approved plugin installer), then enable the listed skills.
If a plugin is unavailable, continue with the local scripts and the documented fallback;
do not add a dependency to the Unity project merely because an MCP is missing.

## Install/enable these plugins

| Plugin ID | Display name | Use |
|---|---|---|
| `unity-workbench@openai-curated-remote` | Unity Essentials | Unity onboarding, health, implementation, build validation and optional Unity MCP |
| `test-android-apps@openai-curated-remote` | Test Android Apps | ADB, emulator/Lava route QA and performance capture |
| `game-studio@openai-curated-remote` | Game Studio | Original game UI, playtest and editable asset pipeline when the active task requires it |
| `app-69f271663a288191ac98f46bed7cb032@openai-curated-remote` | Tavily AI | Current web research when the research task needs Tavily |
| `github@openai-curated-remote` | GitHub | Optional Actions/PR inspection; `git` plus GitHub's public API are the fallback |

The required and conditional skill IDs are listed in
`SKILLS_AND_MCP_MANIFEST.json`. At minimum enable:

- `unity-workbench:unity-project-onboarding`
- `unity-workbench:unity-project-health-check`
- `unity-workbench:unity-feature-implementation`
- `unity-workbench:unity-build-validation`
- `test-android-apps:android-emulator-qa`
- `test-android-apps:android-performance`
- `deep-research-work:deep-research`

Enable `unity-workbench:unity-mcp-workflow`, the Game Studio skills, or the security scan
only when the active checkpoint needs them. Skills do not replace reading `AGENTS.md` and
the required BattleRaja docs.

## Local toolchain preflight

From `C:\Projects\BattleRaja` run:

```powershell
pwsh -File Tools/AgentKit/Install-T3AgentKit.ps1 -ProjectRoot (Get-Location).Path
pwsh -File Tools/Validation/validate.ps1 -ProjectRoot (Get-Location).Path -RequireUnityProject
```

Confirm Unity `6000.5.6f1`, Android SDK/NDK build tools, `aapt.exe`,
`llvm-readelf.exe`, and ADB. Physical evidence is limited to Lava serial
`ST5GDW23LB004392`; do not use the Oppo serial.

## Research and GitHub fallbacks

- Use `web__run` with official Android/Google Play sources when Tavily is unavailable;
  record URL, date, claim and decision impact in `Docs/RESEARCH_LOG.md`.
- Use `git fetch`, `git ls-remote`, and the public GitHub Actions REST endpoint when the
  GitHub plugin is unavailable. Verify local/remote SHA equality after every push.
- Do not add network permissions, Photon, PlayFab, accounts, ads, IAP or online services
  to make an MCP integration work.
