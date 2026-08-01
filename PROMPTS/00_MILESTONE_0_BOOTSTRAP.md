# Prompt to paste into Codex — Milestone 0

You are the lead technical director and implementation agent for BattleRaja.

Work from the current repository root. Before changing anything, read in full:

- `AGENTS.md`
- `Docs/MASTER_VISION.md`
- `PROJECT_STATUS.md`
- `PROJECT_CONTEXT.json`
- `README.md`
- every existing file in `Docs/` that defines architecture, decisions, research, testing, security, culture or performance

Your task is to perform **Milestone 0 only: Repository and Research Foundation**.

## Required work

1. Inspect the repository and local environment:
   - Git status and configuration
   - installed Unity/Unity Hub versions
   - Android SDK, NDK, JDK and ADB
   - installed Unity Web build support
   - available browsers and browser testing options
   - available C#/.NET tooling
   - operating system constraints
   - available build and test commands
   - whether GitHub access or a remote repository is configured

2. Research current official documentation before making unstable technical decisions:
   - recommended production-supported Unity 6 release
   - compatible versions of URP, Input System, Cinemachine, AI Navigation, Addressables, Animation Rigging and Unity Test Framework
   - current Android target API and Google Play requirements
   - current Unity Web browser compatibility, technical limits, memory, networking and performance guidance
   - hosting compression/MIME/cache-header requirements and a preliminary hosting comparison
   - Unity Android build guidance
   - current Codex `AGENTS.md`, skills, MCP and subagent practices
   - Photon Fusion and PlayFab only at a high-level future-planning level; do not install them

3. Update or create:
   - `Docs/RESEARCH_LOG.md`
   - `Docs/DECISIONS.md`
   - `Docs/ARCHITECTURE.md`
   - `Docs/PERFORMANCE_BUDGET.md`
   - `Docs/TEST_STRATEGY.md`
   - `Docs/SECURITY.md`
   - `Docs/CULTURAL_GUIDE.md`
   - `Docs/NETWORK_MODEL.md`
   - `Docs/CODEX_WORKFLOW.md`
   - `PROJECT_STATUS.md`
   - root `AGENTS.md` only where an evidence-based improvement is needed

4. Propose the exact Unity version and package matrix. Do not silently install, update or lock versions before documenting the recommendation and risks.

5. Define assembly boundaries so:
   - pure domain rules do not depend on Unity UI, animation, scenes, Photon or PlayFab
   - bots and human input emit the same command types
   - runtime state is separate from ScriptableObject configuration
   - simulation timing is separate from rendering
   - automated tests can run without production scenes

6. Convert Milestones 0–5 into small implementation issues with:
   - ID
   - objective
   - dependencies
   - exact acceptance criteria
   - tests
   - expected files/subsystems
   - risks
   - human review point

   Place the plan in `Docs/MILESTONE_ISSUES.md`. Create GitHub issues only if repository access is configured and the operation is non-destructive; otherwise provide the issue-ready Markdown.

7. If this is not yet a valid Unity project:
   - determine the safest way to create the approved Unity URP project in this repository root
   - preserve all existing documents and Git files
   - create it only if the local approved Unity version is available and the operation is safe
   - otherwise document the exact human action required

8. Establish the minimum validation foundation:
   - Unity Test Framework
   - assembly definitions
   - one trivial pure-domain test
   - one minimal PlayMode/bootstrap smoke test if a Unity project exists
   - content validation entry point
   - command-line test/build entry point

9. Attempt minimal Android and Web development builds only when:
   - Unity and Android modules are correctly installed
   - build settings are explicit
   - no legal agreement, login or paid action is required
   - the build does not require secrets

10. Do **not** implement:
    - player movement
    - camera gameplay
    - weapons or damage
    - bots
    - Aandhi
    - Jugaad Gadgets
    - Photon
    - PlayFab
    - economy
    - monetisation
    - final UI
    - final art or audio

## Working method

Before implementation, provide a concise plan based on the repository you actually inspected.

Keep changes small and reviewable. Do not replace functioning work unnecessarily. Never invent test results, installed tools, credentials or successful builds.

## Completion report

Report:

- environment findings
- current official sources researched
- decisions made and decisions awaiting approval
- exact changed files
- commands executed
- tests and results
- Android and Web build results and artifact paths, if attempted
    - browser compatibility and hosting findings
- warnings/errors
- blockers
- assumptions
- architecture risks
- the single recommended next action

Stop after Milestone 0. Do not begin Milestone 1 automatically.
