# Start Here — BattleRaja

This folder is the **Milestone 0 Unity project foundation** for BattleRaja.

## What to do

1. Extract the ZIP to a normal local development directory.
2. Open the extracted `BattleRaja_Android_Web_Codex_Starter` folder in the Codex app.
3. Confirm that Codex has access to the local shell and Git.
4. Paste the complete prompt from:
   - `PROMPTS/00_MILESTONE_0_BOOTSTRAP.md`
5. Review `Docs/MILESTONE_0_EXECUTION_PLAN.md` and the evidence recorded in `PROJECT_STATUS.md`.
6. Approve Milestone 1 separately before gameplay implementation begins.

The approved execution plan is recorded in `Docs/MILESTONE_0_EXECUTION_PLAN.md`. Milestone 0 has now converted and validated the root Unity project; gameplay remains approval-gated.

## Important

- `Docs/MASTER_VISION.md` is the full source of truth.
- `AGENTS.md` contains the short operational rules Codex should follow automatically.
- Milestone 0 is a valid Unity project pinned to Unity `6000.5.6f1`; the Bootstrap scene and package lock are present. Gameplay is intentionally not implemented.
- Do not ask Codex to build the whole game in one pass.
- Do not add Photon, PlayFab, monetisation or final art during Milestone 0.

## Repository layout

```text
BattleRaja_Android_Web_Codex_Starter/
├── AGENTS.md
├── START_HERE.md
├── README.md
├── PROJECT_STATUS.md
├── PROJECT_CONTEXT.json
├── Docs/
│   ├── MASTER_VISION.md
│   └── supporting document templates
├── PROMPTS/
│   ├── 00_MILESTONE_0_BOOTSTRAP.md
│   └── later milestone prompts
├── Assets/BattleRaja/
│   └── intended Unity source structure
├── Tools/
└── .github/
```

The first goal is evidence, not volume: a clean repository, approved architecture, automated validation and minimal Android and browser Web smoke builds.
