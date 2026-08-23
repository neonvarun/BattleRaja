# BattleRaja Continuation Prompt — Performance, UX, Accessibility and Networking Readiness

You are continuing BattleRaja from local and remote `main` at or after `846852a`
(`docs: record exact-source platform regression`). The preceding runtime-bearing
source is `73237c8`. Treat `PROJECT_STATUS.md`, `Docs/QA/LATEST_HEAD_BASELINE.md`,
`Docs/QA/REPLAY_AND_SOAK_REPORT.md` and `Docs/PRODUCT_COMPLETION_STATUS.md` as the
current evidence baseline. Read `AGENTS.md`, `Docs/MASTER_VISION.md`,
`Docs/DECISIONS.md`, `Docs/ARCHITECTURE.md` and `Docs/RESEARCH_LOG.md` before
substantial work.

## Completed baseline

- Repository validation: 0 errors / 0 warnings.
- Full EditMode: 125/125.
- Full PlayMode: 57/57.
- Deep recorded-replay soak: 1,000 seeds x 2 executions = 2,000 matches, zero divergence,
  416.1411007 seconds.
- Android: BazaarBastion development APK built, installed, launched, backgrounded and
  resumed on Lava only (`ST5GDW23LB004392`) with no fatal markers.
  APK SHA-256: `F7E76A5DFB88633047075BB9EA28655F15B9CA65FE1EAE3205D165A4EB56A376`.
- Web: BazaarBastion development build served locally; Chrome and Edge each passed mode,
  fighter-selection and active-match routes at desktop/tablet/portrait viewports.
  WASM SHA-256: `BB722EC437DE934CDDEEF06D1A594604A46F495BDE14A442C138A3ECAF8B14CB`.

The truthful product classification remains **prototype**. Do not promote it without
new human-approved evidence.

## Hard constraints

- Work on `main` only unless the owner explicitly changes that instruction.
- Preserve owner work. Do not edit, reset, delete, stage or commit these dirty/untracked
  files without explicit approval:
  - `Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity`
  - `Assets/BattleRaja/Scenes/Tutorial/TutorialArena.unity`
  - `PROMPTS/BattleRaja_Goal_Master_Handoff_2026-08-22.md`
- Android testing uses only Lava `ST5GDW23LB004392`; never target Oppo or an emulator.
- Keep Core Domain/Application free of Unity, Photon and PlayFab dependencies.
- Presentation may consume authoritative immutable events but must never mutate simulation state directly.
- Do not add Photon runtime behavior or PlayFab integration in this session.
- Do not publish, deploy, sign for release, accept terms, purchase services or submit to a store.
- Never claim success without reproducible command/result evidence.

## Next bounded objective

Close one measurable slice at a time in this order:

1. **Performance and size**
   - Profile the current development APK on Lava during menu, active match, Aandhi pressure and results/rematch.
   - Capture frame time, CPU/GPU where available, memory growth across repeated rematches, battery/thermal observations if supported, cold/warm Web load, compressed transfer size and browser rAF timing.
   - Compare against `Docs/PERFORMANCE_BUDGET.md`; record measurements before optimizing.
   - Optimize only demonstrated hot spots; rerun focused tests plus Android/Web smoke afterward.

2. **Offline loop and UX polish**
   - Playtest full offline match loops on Lava and desktop Web.
   - Fix concrete friction in movement, aim, attack feel, ability/gadget readability, pickup feedback, zone warning, spectator transition, results/rematch and tutorial pacing.
   - Preserve authority ownership and add deterministic regressions for every gameplay rule change.

3. **Visual/audio/accessibility QA**
   - Audit HUD overlap, portrait safe areas, contrast, reduced-flash behavior, volume controls, focus handling and touch ergonomics.
   - Capture representative screenshots at desktop/tablet/portrait and Lava.
   - Route subjective art/audio/branding decisions to human review rather than silently finalizing them.

4. **Networking-readiness review**
   - Audit remaining presentation-owned gameplay decisions and event/replay serialization gaps without adding transport code.
   - Define the minimal authoritative-server/session contracts needed by Phase 8, including bounded duplicate protection, reconnect epochs and sequence rollover behavior.
   - Keep Photon behind explicit owner approval; use Core/Application mocks and tests until then.

## Validation floor

For each material change:

```powershell
$unity = 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe'
pwsh -File Tools\Validation\validate.ps1 -ProjectRoot .
& $unity -batchmode -nographics -projectPath . -runTests -testPlatform editmode `
  -testResults Builds/Local/TestResults/editmode.xml -logFile Builds/Local/Logs/editmode.log
& $unity -batchmode -nographics -projectPath . -runTests -testPlatform playmode `
  -testResults Builds/Local/TestResults/playmode.xml -logFile Builds/Local/Logs/playmode.log
```

Do not pass `-quit` to Unity test runs. Poll for the XML result file because the Windows
Unity launcher can detach.

Use a detached worktree for exact-source platform builds so protected main-worktree
changes are excluded. Build Android/Web only when claiming platform evidence, install
only to the Lava serial, serve Web locally, and inspect screenshots plus console/network
counts. Record artifact sizes, hashes, durations, warnings and limitations honestly.

End the session with updated QA/status documents and a truthful completion report.
