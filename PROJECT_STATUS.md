# Project Status

## Active milestone

**Milestone 11 — Closed-Test Release Candidate Preparation (external gates open)**

## Current state

- Product vision: drafted
- Autonomous M1–M11 sequential execution: active; milestone gates and external
  service approvals remain explicit.
- Latest validated source HEAD: `4d3ae6a`; production-flow EventSystem creation is
  Input System-only and the selected-fighter HUD path is covered by the current
  regression suite.
- Root agent rules: active
- Milestone 0: complete and committed locally
- Unity project: verified at the repository root with URP and MovementLab scenes
- Unity version: `6000.5.6f1`
- Android toolchain: Unity-managed SDK/NDK/OpenJDK modules installed and verified; SDK/Build Tools 36.0.0, NDK r27c `27.2.12479018`, OpenJDK 17.0.18, embedded ADB 36.0.0
- Unity Web build support: present and used successfully
- Browser test environment: Chrome 150 and Edge 150 available; Firefox/Playwright/WebDriver unavailable
- Packages: Input System `1.20.0`, uGUI `2.5.0`, URP `17.5.0`, Test Framework `1.7.0`; lockfile is authoritative
- Movement laboratory: implemented with grey-box arena, placeholder player, independent movement/aim, orthographic camera, aim indicator, desktop bindings and safe-area touch sticks
- M1 report: `Docs/MILESTONE_REPORTS/M1.md`; technical gate passed provisionally with
  subjective human-review debt recorded.
- M2 report: `Docs/MILESTONE_REPORTS/M2.md`; technical gate passed provisionally with
  combat/touch/balance review debt recorded.
- Combat/gameplay progression: M3 Bijli, M4 bot, M5 offline match, M6 gadget and M7
  three-fighter roster gates passed provisionally. Match pacing, Aandhi, pickup,
  spectator/results, gadget counterplay, fighter feel, alpha UI/accessibility and device
  review remain open. Networking, backend and progression remain unimplemented.
- Multiplayer: M8 adapter/mock proof implemented; Photon Fusion 2.1.1 package and non-secret
  App ID are configured locally; the real two-client gate remains blocked pending runtime
  integration, terms approval and transport evidence
- M9: transport-independent eight-slot authoritative server/match preparation is present;
  online-alpha completion is blocked by the M8 real-session precondition
- M10: backend-neutral identity/progression interfaces and deterministic fake are present;
  real PlayFab integration is blocked pending title/account/secret approval
- M11: release-candidate preparation, analytics/crash/release seams and truthful draft docs
  are present; publication, signing, online services and human/legal gates remain open
- Backend/economy: deliberately deferred
- Final art/audio/animation: not started
- Git/LFS: local repository initialized and LFS configured; `origin` points to `https://github.com/neonvarun/BattleRaja.git`

## M1 execution evidence — 2026-08-02

- Pure movement tests: 8/8 EditMode tests passed in `Builds/M1/TestResults/editmode.xml`.
- Movement integration tests: 7/7 PlayMode tests passed in `Builds/M1/TestResults/playmode.xml`.
- Editor validation: `BattleRaja.Editor.BuildEntrypoints.ValidateProject` passed.
- Android: M1 IL2CPP ARM64 development APK built with min API 28/target API 36, installed and launched on Lava LXX508 (Android 14/API 34) and Oppo CPH2487 (Android 16/API 36). Artifact: `Builds/M1/Android/BattleRaja-M1.apk`.
- Web: M1 WebGL2/WebAssembly development build completed under `Builds/M1/Web`, served over local HTTP on port 8001, and returned HTTP 200. Chrome 150 and Edge 150 DOM checks found Unity bootstrap content.
- Android runtime smoke found no M1 application exception after serializing the aim-indicator material. Unity still logs the known Play Asset Delivery `AssetPackManager` class-probe warning on a development APK.

## M2 execution evidence — 2026-08-02

- Pure combat tests: 15/15 EditMode tests passed in `Builds/M2/TestResults/editmode.xml`.
- Combat integration tests: 13/13 PlayMode tests passed in `Builds/M2/TestResults/playmode.xml`.
- Android: M2 IL2CPP ARM64 development APK built at `Builds/M2/Android/BattleRaja-M2.apk`, installed/launched on Lava API 34 and Oppo API 36, with zero fatal application exceptions in the captured logs.
- Web: M2 WebGL2/WebAssembly development build completed under `Builds/M2/Web`, served over local HTTP port 8002, returned HTTP 200, and exposed Unity bootstrap content in Chrome 150 and Edge 150.
- Combat runtime: serialized weapon/input/tuning references were verified in the generated scene; projectile hit, despawn, pooling, collision filtering and dummy reset passed PlayMode coverage.

## M3 execution evidence — 2026-08-02

- Pure fighter/ability tests: 20/20 EditMode tests passed in `Builds/M3/TestResults/editmode.xml`.
- Fighter integration tests: 16/16 PlayMode tests passed in `Builds/M3/TestResults/playmode.xml`.
- Bijli is spawnable in MovementLab with stable fighter/attack/ability IDs, a data-driven electric bolt, directional dash, explicit startup/active/recovery/cooldown states, collision/bounds truncation, a touch ability button and a health/cooldown HUD.
- Android: M3 IL2CPP ARM64 development APK built at `Builds/M3/Android/BattleRaja-M3.apk`, installed/launched on Lava API 34 and Oppo API 36, with zero fatal application exceptions in captured logs. Oppo `pm clear` is restricted by device policy but install/launch succeeded.
- Web: M3 WebGL2/WebAssembly development build completed under `Builds/M3/Web`; local HTTP port 8011 returned 200 and Chrome 150/Edge 150 headless DOM checks found Unity bootstrap content.

## M4 execution evidence — 2026-08-02

- Pure bot tests: 26/26 EditMode tests passed in `Builds/M4/TestResults/editmode.xml`.
- Bot integration tests: 19/19 PlayMode tests passed in `Builds/M4/TestResults/playmode.xml`.
- Seven Bijli bots spawn with unique actor IDs, cached line-of-sight perception,
  seeded utility decisions, aim noise, reaction delay, attack/dash commands and
  bounded stuck recovery. Stress output measured 91 decisions in 2 seconds with a
  maximum decision duration of 0.024 ms in the headless editor.
- Android: M4 IL2CPP ARM64 development APK built at `Builds/M4/Android/BattleRaja-M4.apk`, installed/launched on Lava API 34 and Oppo API 36, with zero fatal application exceptions in captured logs.
- Web: M4 WebGL2/WebAssembly development build completed under `Builds/M4/Web`; local HTTP port 8011 returned 200 and Chrome 150/Edge 150 headless DOM checks found Unity bootstrap content.

## M5 execution evidence — 2026-08-02

- Offline match tests: 31/31 EditMode tests passed in `Builds/M5/TestResults/editmode.xml`.
- Match integration tests: 22/22 PlayMode tests passed in `Builds/M5/TestResults/playmode.xml`.
- `OfflineMatchSimulation` owns a 298-second phase definition, Aandhi radius/damage,
  separated spawns, idempotent elimination, placement, winner and spectator selection;
  MovementLab now contains eight actors, three neutral health pickups and results/rematch
  presentation.
- Pure soak completed 20 accelerated matches with explicit restart state cleanup.
- Android/Web M5 builds and local/device smoke are recorded in `Docs/MILESTONE_REPORTS/M5.md`.

## M6 execution evidence — 2026-08-02

- Gadget tests: 37/37 EditMode and 25/25 PlayMode passed in `Builds/M6/TestResults/`.
- The Domain gadget catalog contains stable Umbrella Guard, Dhol Burst and Tiffin
  Station definitions with one-slot inventory, use/cooldown validation and spawn rules.
- MovementLab contains three gadget pickups, player touch/keyboard use, HUD feedback,
  contextual bot use, directional shield mitigation, bounded Dhol displacement and
  finite destroyable/healing Tiffin stations.
- Android/Web M6 builds, two-device launch smoke and Chrome/Edge local HTTP checks are
  recorded in `Docs/MILESTONE_REPORTS/M6.md`.

## M7 execution evidence — 2026-08-02

- Fighter tests: 40/40 EditMode and 27/27 PlayMode passed in `Builds/M7/TestResults/`.
- Stable Pehel and Maya fighter/special definitions and serialized M7 assets are present;
  MovementLab now contains a two-Bijli, three-Pehel, two-Maya bot mix plus the player.
- Android/Web M7 builds, two-device launch and Chrome/Edge local HTTP checks are recorded
  in `Docs/MILESTONE_REPORTS/M7.md`.

## M8 execution evidence — 2026-08-02

- `BattleRaja.Infrastructure.Networking` contains compile-safe session contracts, a
  deterministic two-client mock and an explicit credential-blocked Photon adapter seam;
  no Photon/PlayFab dependency or secret entered the repository.
- EditMode: 44/44 passed in `Builds/M8/TestResults/editmode.xml`.
- PlayMode: 27/27 passed in `Builds/M8/TestResults/playmode.xml`.
- Android/Web M8 build and desktop-browser smoke are recorded in
  `Docs/MILESTONE_REPORTS/M8.md`; Android validation from this point forward uses only the
  connected Lava phone per owner instruction.
- M8 full gate is **blocked**, not complete, until a real Fusion package/App ID/account
  configuration enables one Lava Android client and one desktop Web client to share a room.

## M9 execution evidence — 2026-08-02

- `AuthoritativeMatchServer` preparation covers eight slots, bot backfill, bounded input,
  server-owned health/elimination, disconnect grace, reconnect and bot takeover without
  Photon/PlayFab or deployment.
- M9 is **blocked**, not complete, because M8 has no validated real two-client Fusion
  session. No online alpha, cross-play, matchmaking or headless-server result is claimed.
- Preparation tests and any M9 regression/build evidence are recorded in
  `Docs/MILESTONE_REPORTS/M9.md`.

## M10 execution evidence — 2026-08-02

- `IProgressionBackend`, `FakeProgressionBackend`, `PlayFabBackendAdapter` and
  `BackendConfiguration.LocalProof` establish guest/link/profile/reward/inventory/
  leaderboard/remote-config contracts without SDKs or secrets.
- M10 is **credential-blocked partial**, not a real service pass. No PlayFab title, SDK,
  cross-device persistence, account recovery or economy deployment is claimed.
- Evidence and test/build status are recorded in `Docs/MILESTONE_REPORTS/M10.md`.

## M11 execution evidence — 2026-08-03

- Release candidate configuration rejects admin tools/secrets; bounded development analytics
  and unavailable crash adapter are compile-safe and service-neutral.
- Closed-test, rollback/support, store-draft and privacy/data-safety worksheets are in `Docs/`.
- M11 artifact, test and smoke evidence is recorded in `Docs/MILESTONE_REPORTS/M11.md`.
- Current continuation HEAD `5f4566d` includes functional bounded aim assist,
  authoritative offline assist attribution and focus-hardened Web keyboard/pointer input
  plus an Input System-only project baseline with legacy-scene compatibility bridge,
  in addition to the authority-driven spatial gadget collection
  coverage, immediate live Results/Rematch publication, three repeated rematch cleanup
  cycles and the complete eight-step tutorial walkthrough. Fresh latest-HEAD regression
  is 94/94 EditMode and 45/45 PlayMode; the latest development APK/Web smoke artifacts
  and Lava/Chrome/Edge runtime captures are recorded in `Docs/QA/LATEST_HEAD_BASELINE.md`.
- No public publication, store submission, signing-key use, paid service, legal acceptance or
  final approval is claimed.
- External blockers and exact owner actions are tracked in `Docs/EXTERNAL_SERVICE_GATES.md`.

## M0 evidence retained

- M0 validation, tests and smoke artifacts remain under ignored `Builds/M0/`.
- The M0 foundation commit is preserved as the parent of the current working changes.

## Performance and limitations

- Movement hot paths use cached component references, value types and no per-frame LINQ/managed collection creation.
- No formal Editor Profiler, Android GPU/CPU capture or Web browser performance profile has been collected yet; values remain explicitly unmeasured in `Docs/PERFORMANCE_BUDGET.md`.
- Physical touch interaction, safe-area variations, browser keyboard/mouse focus by manual play, and visual camera comparison still require human playtesting.
- Bijli bolt/dash balance, HUD legibility, dash collision feel and low-end-device profiling remain unmeasured.
- Bot fairness, debug-overlay usefulness, device-tier seven-bot frame time and
  authored navigation remain unmeasured.
- Full five-minute physical match pacing, Aandhi/pickup readability, results UX and
  repeated-runtime memory/object growth remain unmeasured.
- Gadget counterplay/readability, bot gadget value, station scale behavior and device
  performance remain unmeasured. M6 device logs include a non-fatal SphereCollider
  creation warning from the existing projectile pool.
- M7 bespoke specials, final art/audio, tutorial/menu/accessibility UI, fighter balance
  and performance remain unmeasured; the same non-fatal SphereCollider warning persists.

## Approval gates

Review is required before:

1. Adding Photon, PlayFab, monetisation or paid infrastructure.
2. Publishing, deploying, signing for release or submitting to stores.
3. Changing the pinned editor/package baseline or final branding/trademarks.
4. Human review remains required for movement/combat feel, touch ergonomics, camera
   choice, balance, cultural sensitivity, legal/privacy, and release approval.
5. Photon Fusion package/App ID/licence/account approval is required before real online
   session testing; never commit secrets or treat the mock as production networking.
