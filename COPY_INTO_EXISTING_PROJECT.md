# Add This Pack to Your Existing M0 Project

Your Milestone 0 project already contains real environment decisions and code. Do **not** replace it with an older starter repository.

1. Back up or commit your current project.
2. Copy this pack's `PROMPTS/` files into your project's `PROMPTS/` directory.
3. Copy these files into your project's `Docs/` directory:
   - `FULL_BUILD_RUNBOOK.md`
   - `HUMAN_REVIEW_BACKLOG.md`
   - `EXTERNAL_SERVICE_GATES.md`
4. Preserve your current `AGENTS.md`, `PROJECT_STATUS.md`, `Docs/DECISIONS.md`, package files and Unity project settings.
5. Open the existing project in Codex Goal mode.
6. Paste `PROMPTS/99_AUTOPILOT_M1_TO_M11.md` for the long autonomous run, or use the numbered milestone prompts individually.

The numbered prompts are safer. The orchestrator is more autonomous but may stop at credentials, external services, legal approval or a broken quality gate.
