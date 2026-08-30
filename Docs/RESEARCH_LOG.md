# Research Log

Research current primary sources before selecting technical versions, APIs, SDKs, policies or deployment methods.

## Entry template

### Research item

- **Date checked:**
- **Question:**
- **Primary source:**
- **Relevant claim:**
- **Decision impact:**
- **Uncertainty:**
- **Recheck trigger/date:**

## Initial required topics

- Unity 6 production-supported release
- Unity package compatibility
- Android target API / Google Play requirements
- Unity Web browser support and technical limitations
- Web performance, memory, download and hosting headers
- Photon Fusion WebGL support and authority topology
- Unity Android build and performance guidance
- Codex repository instructions, skills, MCP and subagents
- Photon Fusion future topology and pricing
- PlayFab future services and pricing

### Current Google Play policy recheck — 2026-08-24

- **Date checked:** 2026-08-24
- **Question:** What technical and listing gates apply at the planned V1 submission date?
- **Primary source:** https://developer.android.com/google/play/requirements/target-sdk ; https://developer.android.com/guide/practices/page-sizes ; https://support.google.com/googleplay/android-developer/answer/10787469 ; https://support.google.com/googleplay/android-developer/answer/9859655
- **Relevant claim:** Starting 2026-08-31, new apps and updates submitted to Google Play must target Android 16/API 36 or higher. Apps targeting API 35+ must support 16 KB memory pages on 64-bit devices; Google Play blocks updates that lack support from 2027-02-01. Published apps, including closed/open/production testing tracks, must complete the Data safety form and provide a privacy-policy link even when no data is collected; apps exclusively on internal testing are exempt. New apps must declare target audience/content and complete the content-rating questionnaire.
- **Decision impact:** Keep target API 36, retain ARM64/16 KB checks, keep the offline build free of network permissions and data collection, and keep privacy/Data Safety, target-audience, content-rating and final signed-artifact inspection as owner-gated release tasks. Do not treat the current debug-signed bundle as publishable.
- **Uncertainty:** Play policy, developer verification and submission dates are time-sensitive; recheck immediately before creating the listing, signing or uploading any artifact.
- **Recheck trigger/date:** Before Play Console setup, final signing, or any target/API/package change.

### V1 offline Unity service configuration — 2026-08-24

- **Date checked:** 2026-08-24
- **Question:** Does the offline Android candidate have a runtime Unity telemetry, Ads or performance-reporting path that would conflict with the no-upload V1 scope?
- **Primary source:** https://docs.unity3d.com/2023.1/ScriptReference/Analytics.Analytics-enabled.html ; project configuration in `ProjectSettings/UnityConnectSettings.asset` and `ProjectSettings/ProjectSettings.asset`
- **Relevant claim:** Unity Analytics can be disabled at build/runtime; the checked-in project has Unity Connect, Analytics, Ads and Performance Reporting disabled, and the V1 release configuration now also disables the project analytics-submission flag.
- **Decision impact:** Keep V1 offline and no-upload; make validation fail if those service settings are re-enabled without a new data-safety decision.
- **Uncertainty:** Final signed-artifact SDK inventory and Play Console declarations still require owner/legal review.
- **Recheck trigger/date:** Before any telemetry, crash, ads, online-service, package or signing change.

### Current Google Play V1 release-policy recheck

- **Date checked:** 2026-08-24
- **Question:** Which current Android/Play technical and store-listing requirements must the offline V1 candidate satisfy?
- **Primary source:** https://developer.android.com/google/play/requirements/target-sdk ; https://developer.android.com/guide/practices/page-sizes ; https://support.google.com/googleplay/android-developer/answer/9866151 ; https://support.google.com/googleplay/android-developer/answer/13393723
- **Relevant claim:** New apps and updates must target Android 16/API 36 or higher from 2026-08-31; 64-bit apps targeting Android 15+ must support 16 KB pages; Play listings require accurate preview assets, with feature graphics at 1024×500 JPEG/24-bit PNG and phone screenshots from the supported screenshot limits. Store copy must stay within the short/full description limits and must not imply unsupported features, rankings or misleading relationships.
- **Decision impact:** Keep target API 36, retain the ARM64/16 KB bundle validation, and prepare a 1024×500 RGB feature-graphic candidate plus real Lava screenshots. Do not claim final Play readiness until package identity, signing, Data Safety/privacy, cultural review and exact-release screenshot review are approved.
- **Uncertainty:** Play policy and developer-verification requirements are time-sensitive; recheck immediately before submission. The current candidate screenshots and package evidence are attributed to exact HEAD `062066b`; final release screenshots must still be rechecked against the signed production AAB.
- **Recheck trigger/date:** Before creating the Play listing, signing the AAB or submitting any track.

## Milestone 0 research evidence

### Unity editor and Android dependency baseline

- **Date checked:** 2026-08-02
- **Question:** Which Unity release and Android dependencies should Milestone 0 use?
- **Primary source:** https://unity.com/releases/unity-6 ; https://unity.com/releases/editor/whats-new/6000.3.20f1 ; https://docs.unity3d.com/6000.3/Documentation/Manual/android-supported-dependency-versions.html
- **Relevant claim:** Unity 6.3 is the current LTS family reviewed; Unity 6000.3+ supports SDK Build Tools 36.0.0, command-line tools 16, platform-tools 36.0.0, NDK r27c `27.2.12479018`, and OpenJDK 17.
- **Decision impact:** Recommend Unity `6000.3.20f1` and Unity-managed Android dependencies. The locally installed NDK r27.1 is insufficient for the recommended baseline.
- **Uncertainty:** Installation must re-check the release page and generated package/toolchain versions.
- **Recheck trigger/date:** Before installation and before any release build.

### Android platform policy

- **Date checked:** 2026-08-02
- **Question:** What Android target API should the project prepare for?
- **Primary source:** https://support.google.com/googleplay/android-developer/answer/11926878?hl=en-GB ; https://developer.android.com/games/engines/unity/unity-on-android?hl=en
- **Relevant claim:** Google Play requires new apps and updates to target Android 16/API 36 from 31 August 2026; Unity’s Android workflow requires Android Build Support, SDK, NDK and JDK.
- **Decision impact:** Recommend target API 36 for development; use a separately approved minimum API 28 for broad mid-range coverage.
- **Uncertainty:** Store policy and Unity compatibility are time-sensitive.
- **Recheck trigger/date:** Before store submission or any target API change.

### Unity Web platform

- **Date checked:** 2026-08-02
- **Question:** What browser/runtime constraints affect the first Web smoke build?
- **Primary source:** https://docs.unity3d.com/6000.3/Documentation/Manual/webgl-browsercompatibility.html ; https://docs.unity3d.com/6000.3/Documentation/Manual/webgl-gettingstarted.html ; https://docs.unity3d.com/6000.3/Documentation/Manual/webgl-deploying.html ; https://docs.unity3d.com/6000.3/Documentation/Manual/webgl-networking.html
- **Relevant claim:** Desktop Web builds require WebGL2, HTML5, 64-bit and WebAssembly; local serving is required instead of `file://`; Web hosting needs correct compression, MIME, cache and CORS behavior; direct TCP/UDP is unavailable.
- **Decision impact:** Use a local uncompressed HTTP development build in Chrome and Edge only for Milestone 0, with hosting and networking deferred.
- **Uncertainty:** Firefox/Safari and production hosting are not available for validation on this machine.
- **Recheck trigger/date:** When browser automation, CI, or public hosting is introduced.

### Codex workflow and repository instructions

- **Date checked:** 2026-08-02
- **Question:** How should repository instructions and planning be applied?
- **Primary source:** https://developers.openai.com/codex/guides/agents-md ; https://developers.openai.com/codex/learn/best-practices ; https://developers.openai.com/codex/build-skills
- **Relevant claim:** Codex loads applicable `AGENTS.md` instructions and recommends explicit goals, constraints, plans, validation and evidence for complex work.
- **Decision impact:** Keep Milestone 0 bounded, document approval gates, and report exact commands/results rather than claiming unverified runtime behavior.
- **Uncertainty:** None material for this milestone.
- **Recheck trigger/date:** When repository instructions or the active milestone changes.

### Unity package registry compatibility snapshot

- **Date checked:** 2026-08-02
- **Question:** Which package versions are compatible candidates for Unity 6000.3?
- **Primary source:** Unity Package Registry endpoints `https://packages.unity.com/com.unity.inputsystem`, `https://packages.unity.com/com.unity.cinemachine`, `https://packages.unity.com/com.unity.ai.navigation`, `https://packages.unity.com/com.unity.addressables`, `https://packages.unity.com/com.unity.animation.rigging`, `https://packages.unity.com/com.unity.test-framework`, `https://packages.unity.com/com.unity.test-framework.performance`, and `https://packages.unity.com/com.unity.performance.profile-analyzer`.
- **Relevant claim:** Registry candidates were Input System `1.20.0`, Cinemachine `3.1.7`, AI Navigation `2.0.14`, Addressables `3.1.0`, Animation Rigging `1.4.1`, Test Framework `1.4.6`, Performance Testing `3.5.0`, and Profile Analyzer `1.4.0`.
- **Decision impact:** Install only URP template dependencies, Input System and Test Framework in M0. Defer feature/performance packages to the milestones that require them.
- **Uncertainty:** The 6000.3 template and generated `packages-lock.json` remain authoritative; registry metadata can change.
- **Recheck trigger/date:** At Unity project creation and whenever a deferred package is introduced.

### Photon Fusion local integration setup

- **Date checked:** 2026-08-02
- **Question:** Which Photon Fusion package and project configuration should be used for the first real-session preparation step?
- **Primary source:** https://doc.photonengine.com/fusion/current/getting-started/sdk-download ; https://doc.photonengine.com/fusion/current/game-samples/br200/quickstart
- **Relevant claim:** Photon lists Fusion SDK 2.1.1 Stable Build 2177, requires a Fusion 2 App ID and text asset serialization, and configures the App ID through `Tools/Fusion/Realtime Settings`; Android and WebGL are supported platforms. The main SDK page explicitly lists Unity 6.0.x and 6.3.x, so Unity `6000.5.6f1` compatibility remains an integration risk to validate.
- **Decision impact:** Import the owner-downloaded 2.1.1 Build 2177 `.unitypackage`, retain the package-managed Mono Cecil dependency and Fusion scripting defines, and configure only the non-secret Fusion App ID in `Assets/Photon/Fusion/Resources/PhotonAppSettings.asset`. Keep the real-session gate blocked until runtime validation.
- **Uncertainty:** No two-client Fusion room, prediction/reconciliation, reconnection or controlled-loss run has been executed; no claim of M8 completion is made.
- **Recheck trigger/date:** Before the first real Photon session and before changing the pinned Unity editor or Fusion package.

### Live Unity installation recheck

- **Date checked:** 2026-08-02
- **Question:** Does the installed Unity environment satisfy the approved M0 baseline?
- **Primary source:** https://unity.com/releases/editor/whats-new/6000.5.6f1 ; https://docs.unity3d.com/6000.5/Documentation/Manual/android-supported-dependency-versions.html
- **Relevant claim:** The installed editor is Unity `6000.5.6f1` (released 2026-07-29). Unity 6000.5 supports Android NDK r27c and OpenJDK 17, but the local editor installation has WebGLSupport only; AndroidPlayer and embedded Android dependencies are absent.
- **Decision impact:** Do not silently use 6000.5.6f1 in place of the approved 6000.3.20f1. Resolve the editor-version decision and add Android Build Support before project conversion or Android validation.
- **Uncertainty:** The owner may intentionally prefer 6000.5.6f1; that would require updating the approved plan and package baseline.
- **Recheck trigger/date:** After the owner resolves the version/module decision.

### Owner-approved Unity 6000.5.6f1 baseline and completed module installation

- **Date checked:** 2026-08-02
- **Question:** What exact editor and embedded Android dependencies are the M0 baseline after owner direction?
- **Primary source:** https://unity.com/releases/editor/whats-new/6000.5.6f1 ; https://docs.unity3d.com/6000.5/Documentation/Manual/android-supported-dependency-versions.html ; https://docs.unity.com/en-us/hub/hub-cli-reference ; https://docs.unity.com/en-us/hub/add-modules
- **Relevant claim:** The owner selected Unity `6000.5.6f1`. Hub module installation with child modules provides Android Build Support, SDK/NDK/OpenJDK; the installed editor reports NDK r27c `27.2.12479018`, OpenJDK 17.0.18, SDK/Build Tools/platform-tools 36.0.0 and embedded ADB 36.0.0.
- **Decision impact:** The earlier 6000.3 recommendation is superseded. Project settings and package lock are pinned to 6000.5.6f1 and its resolved URP/Test Framework versions.
- **Uncertainty:** Unity and Android release policies can change; recheck before release or target API changes.
- **Recheck trigger/date:** Before any editor/package upgrade or store submission.

### Live M0 validation and smoke evidence

- **Date checked:** 2026-08-02
- **Question:** Does the converted root satisfy the M0 project, test and platform smoke requirements?
- **Primary source:** https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-gettingstarted.html ; https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-deploying.html ; https://developer.android.com/google/play/requirements/target-sdk
- **Relevant claim:** Unity Web builds should be served over HTTP rather than `file://`; Android development builds require the installed Unity Android toolchain and target API policy remains time-sensitive.
- **Decision impact:** The M0 build profiles use Android min API 28/target API 36 and WebGL2/WebAssembly with local uncompressed HTTP smoke testing.
- **Local evidence:** `validate.ps1` passed; EditMode 2/2 and PlayMode 1/1 passed; Android IL2CPP ARM64 APK installed/launched on Lava API 34 and Oppo API 36; Web output returned HTTP 200 and Unity bootstrap DOM content in Chrome 150 and Edge 150.
- **Uncertainty:** Browser visual rendering, console cleanliness, mobile Web, Firefox/Safari, production hosting headers, signing and store submission remain untested.
- **Recheck trigger/date:** Before M1 platform work and before any production build.

### M1 Input System and uGUI baseline

- **Date checked:** 2026-08-02
- **Question:** Which installed Unity packages are needed for configurable desktop/gamepad input and Android virtual sticks?
- **Primary source:** Unity 6000.5 builtin package metadata at the installed editor's `Data/Resources/PackageManager/BuiltInPackages`; Unity Input System package source and documentation in the resolved package cache.
- **Relevant claim:** Unity 6000.5 provides Input System `1.20.0` and builtin uGUI `2.5.0`; uGUI supplies the EventSystem, Canvas, GraphicRaycaster and pointer interfaces required for virtual sticks.
- **Decision impact:** Add only builtin `com.unity.ugui` `2.5.0` to the direct M1 package manifest. Keep raw input in Presentation and convert it into pure movement commands.
- **Uncertainty:** Physical touch behavior and browser canvas focus remain device/manual validation items.
- **Recheck trigger/date:** Before changing input bindings, adding UI packages, or supporting additional platforms.

### M2 combat implementation baseline

- **Date checked:** 2026-08-02
- **Question:** Does the M2 combat laboratory require a new package or external SDK?
- **Primary source:** `PROMPTS/02_MILESTONE_2_COMBAT.md`; installed Unity 6000.5.6f1
  package lock; existing Unity physics/Input System/uGUI documentation and local
  build/test evidence.
- **Relevant claim:** M2 can use pure C# rules, Unity physics queries, existing Input
  System/uGUI controls and bounded presentation pools; Photon, PlayFab and named
  fighter systems are explicitly out of scope.
- **Decision impact:** Add no package and no external service. Keep attack commands,
  health/damage, cooldown, projectile travel, faction eligibility and duplicate-hit
  rules in the existing Domain/Application boundary; keep collision/feedback/input
  adapters in Presentation.
- **Local evidence:** 15/15 EditMode, 13/13 PlayMode, M2 Android/Web builds,
  authorized Lava/Oppo launches and Chrome/Edge local HTTP bootstrap checks.
- **Uncertainty:** Human combat feel/balance, formal profiling, browser console review,
  and release signing remain unverified.
- **Recheck trigger/date:** Before adding fighter abilities, networking, backend
  services, packages or production combat assets.

### M3 fighter implementation baseline

- **Date checked:** 2026-08-02
- **Question:** Can Bijli use the existing package/toolchain baseline without a new
  SDK or package while keeping ability timing network-compatible?
- **Primary source:** `PROMPTS/03_MILESTONE_3_BIJLI.md`; installed Unity 6000.5.6f1
  package lock; existing Input System/uGUI and CharacterController documentation.
- **Relevant claim:** M3 requires stable content IDs, immutable definitions, per-fighter
  runtime state, common command interfaces, collision-safe dash behavior and Android/Web
  smoke builds; passive and later services are non-scope.
- **Decision impact:** Add no package or external service. Keep IDs, dash phases,
  fallback, cooldown and bounded displacement in Domain; keep input, physics, trail and
  HUD in Presentation.
- **Local evidence:** 20/20 EditMode, 16/16 PlayMode, M3 Android/Web builds, two-device
  ADB launch and Chrome/Edge local bootstrap checks.
- **Uncertainty:** Human kit/balance/touch review and formal profiling remain open.
- **Recheck trigger/date:** Before adding bots, gadgets, networking or changing the
  fighter data model.

### M4 offline bot implementation baseline

- **Date checked:** 2026-08-02
- **Question:** Can seven Bijli bots use the existing command boundary without a new
  package, navigation SDK or hidden-authority shortcut?
- **Primary source:** `PROMPTS/04_MILESTONE_4_BOTS.md`; existing Domain/Application
  command interfaces; installed Unity 6000.5.6f1 physics/Input System baseline.
- **Relevant claim:** Bots must separate perception/decision/output, use seeded
  randomness, respect line of sight/cooldowns/collision and recover from stuck states;
  Aandhi, loot, gadgets, networking and backend are non-scope.
- **Decision impact:** Add no package or external service. Keep bot observations,
  scoring, reaction delay, aim noise and recovery in pure Domain; keep Physics linecasts,
  actor discovery, command submission and debug overlay in Presentation.
- **Local evidence:** 26/26 EditMode, 19/19 PlayMode, seven-bot stress output,
  M4 Android/Web builds, two-device ADB launch and Chrome/Edge HTTP checks.
- **Uncertainty:** Device-tier performance, human fairness/tuning, dynamic navigation
  and manual visual debug review remain open.
- **Recheck trigger/date:** Before adding Aandhi/loot in M5 or replacing the lab movement
  recovery with authored navigation.

### M5 offline match baseline

- **Date checked:** 2026-08-02
- **Question:** Can the first complete match remain deterministic and service-free while
  sharing the existing combat actors and command systems?
- **Primary source:** `PROMPTS/05_MILESTONE_5_OFFLINE_BATTLE_ROYALE.md`; existing Domain
  damage pipeline and M1–M4 scene actors.
- **Relevant claim:** M5 requires explicit match phases, Aandhi, separation, elimination,
  placement, spectator/results/rematch and 20-match soak; gadgets, networking, accounts
  and progression are non-scope.
- **Decision impact:** Add pure `OfflineMatchSimulation` and a Unity controller bridge;
  no package or external service. Use a 298-second data definition and accelerated pure
  tests for soak.
- **Local evidence:** 31/31 EditMode, 22/22 PlayMode, M5 Android/Web builds and smoke
  evidence.
- **Uncertainty:** Full five-minute physical playthrough, device-tier stability,
  pickup/readability and repeated-runtime memory profile remain open.
- **Recheck trigger/date:** Before adding Jugaad Gadgets, online authority or changing
  match timing/zone policy.

### M6 Jugaad gadget baseline

- **Date checked:** 2026-08-02
- **Question:** Can three tactical gadgets be added without bypassing the existing
  central damage, healing, movement and bot command boundaries or adding a package?
- **Primary source:** `PROMPTS/06_MILESTONE_6_JUGAAD_GADGETS.md`; existing
  `DamageRequest`/`DamagePipeline`, `CombatHealth`, `MovementPlayerAgent`, `BotAI` and
  M5 offline match implementation.
- **Relevant claim:** M6 requires three distinct gadgets, one held item, validated
  pickup/use, readable counters, contextual bot evaluation, finite station healing and
  Android/Web builds; networking/backend/final art are non-scope.
- **Decision impact:** Add pure Gadget definitions/inventory/runtime/spawn rules and
  serialized content assets; keep effect bridges in Presentation and route mitigation,
  healing and displacement through existing systems. No Photon, PlayFab or new package.
- **Local evidence:** 37/37 EditMode, 25/25 PlayMode, M6 Android/Web builds, two-device
  launch smoke and Chrome/Edge local HTTP checks.
- **Uncertainty:** Human counterplay/readability, bot use value, station scale behavior,
  device-tier performance and a pre-existing IL2CPP SphereCollider warning remain open.
- **Recheck trigger/date:** Before adding progression/cosmetics or changing the gadget
  data model for online authority.

### M7 three-fighter vertical-slice baseline

- **Date checked:** 2026-08-02
- **Question:** Can Pehel and Maya join the offline lab with distinct data and stable
  IDs while preserving the existing shared commands and no external services?
- **Primary source:** `PROMPTS/07_MILESTONE_7_VERTICAL_SLICE.md`; existing fighter,
  combat, movement, gadget and offline-match assemblies.
- **Relevant claim:** M7 requires three distinct fighters, original alpha identity,
  shared commands, tutorial/accessibility/performance review and Android/Web builds;
  networking/backend/release are non-scope.
- **Decision impact:** Add pure Pehel/Maya definitions and bounded special runtime plus
  serialized weapon/fighter assets. Keep the current shared presentation bridge as an
  explicit alpha implementation; do not add protected or downloaded art/audio.
- **Local evidence:** 40/40 EditMode, 27/27 PlayMode, M7 Android/Web builds, two-device
  launch and Chrome/Edge local bootstrap checks.
- **Uncertainty:** Bespoke special visuals, full UI/tutorial/accessibility, human balance,
  cultural/art review and device/browser performance remain open.
- **Recheck trigger/date:** Before online authority work or a final art/UI pass.

### M8 Photon Fusion networking proof baseline

- **Date checked:** 2026-08-02
- **Primary sources:** [Photon Fusion Network Runner](https://doc.photonengine.com/fusion/current/manual/network-runner),
  [network topologies](https://doc.photonengine.com/fusion/current/manual/network-topologies),
  [fixed-tick/network simulation loop](https://doc.photonengine.com/fusion/current/concepts-and-patterns/network-simulation-loop),
  [network-condition simulation](https://doc.photonengine.com/fusion/v2/manual/testing-and-tooling/simulating-network-conditions),
  and [Fusion getting started](https://doc.photonengine.com/fusion/v2/tutorials/shared-mode-basics/1-getting-started).
- **Relevant claims:** Fusion sessions are hosted through a NetworkRunner; topology and
  tick/simulation choices affect authority and replication; controlled latency/jitter/loss
  simulation is available for testing. These claims were checked against current official
  Photon docs before selecting the M8 seam.
- **Decision impact:** Keep Photon outside Domain/Application, use client-server as the
  proof topology, represent player actions as shared command/input intent, and test local
  authority semantics with a deterministic mock until the approved Fusion package/App ID
  exists. Do not fabricate a cloud session or put secrets in the repository.
- **Local evidence:** 44/44 EditMode and 27/27 PlayMode tests; M8 adapter/mock source;
  no real Fusion client evidence.
- **Uncertainty:** Package version/licence, App ID, exact API wiring, real prediction,
  reconciliation, interpolation, reconnect and Android/Web cross-platform behavior remain
  blocked and must be rechecked after owner approval.
- **Recheck trigger/date:** Immediately after Photon package/App ID/account access is
  approved; before claiming M8 complete or adding public matchmaking.

### M10 PlayFab identity/progression baseline

- **Date checked:** 2026-08-02
- **Primary sources:** [PlayFab authentication](https://learn.microsoft.com/en-us/gaming/playfab/identity/player-identity/authentication/),
  [anonymous login changes](https://learn.microsoft.com/en-us/xbox/playfab/identity/player-identity/platform-specific-authentication/anonymous-login),
  [account linking](https://learn.microsoft.com/en-us/xbox/playfab/identity/player-identity/login/quickstart),
  [Economy v2 inventory](https://learn.microsoft.com/en-us/gaming/playfab/economy-monetization/economy-v2/inventory/items-and-inventory-overview),
  [leaderboards](https://learn.microsoft.com/en-us/gaming/playfab/community/leaderboards/),
  and [official REST/API references](https://learn.microsoft.com/en-us/rest/api/playfab/).
- **Relevant claims:** PlayFab supports linked player identities; new titles disable client-side
  anonymous account creation by default and require server-side creation; Economy v2 inventory
  supports idempotency; leaderboards/statistics are intended to be trusted service writes.
- **Decision impact:** Keep title secrets server-only, use a backend-neutral interface and fake,
  require server-validated reward evidence/idempotency keys, and do not ship a client-side
  PlayFab SDK/App ID until the owner approves the title and secret channel.
- **Local evidence:** Five M10 fake-backend EditMode proof cases; no PlayFab account, SDK,
  cross-device persistence or service quota evidence.
- **Uncertainty:** Exact approved Unity SDK/API version, title configuration, account-linking
  provider, retention/legal policy and service pricing remain unresolved.
- **Recheck trigger/date:** After owner supplies PlayFab title/account access and before any
  real identity, economy or leaderboard claim.

### V1.0 Android Play release hardening (2026-08-23)

- **Question:** Which Android requirements must the offline V1.0 candidate satisfy before a
  Play Console submission can be considered?
- **Primary sources:** [Google Play target API requirements](https://support.google.com/googleplay/android-developer/answer/11926878?hl=en-GB_ALL),
  [Android 16 KB page-size guidance](https://developer.android.com/guide/practices/page-sizes),
  [Play Console app setup and signing](https://support.google.com/googleplay/android-developer/answer/9859152?hl=en),
  and [Unity 6000.5.0f1 release notes](https://unity.com/releases/editor/whats-new/6000.5.0f1).
- **Relevant claims:** New Google Play submissions and updates must target Android 16/API 36
  from 31 August 2026; 64-bit apps targeting Android 15/API 35+ must support 16 KB pages
  before the 2027 enforcement date; Play uploads use an Android App Bundle and require a
  signed application; Unity 6000.5 includes the Android 16/API 36 toolchain direction but
  the exact project build still needs an artifact-level 16 KB alignment check.
- **Decision impact:** Keep Unity 6000.5.6f1, ARM64, IL2CPP, target API 36 and min API 28;
  add a local release-shaped AAB entrypoint without signing or uploading; verify native
  library alignment from the actual bundle before any Play gate; keep the current example
  package identifier explicitly blocked for release until the owner approves the final
  application ID and signing identity.
- **Local evidence:** ProjectSettings already targets API 36 and requests no forced Internet
  or SD-card permission; the V1 release entrypoint is now source-controlled but its AAB and
  16 KB report are generated only during the release validation run.
- **Uncertainty:** Final package name, signing key, Play App Signing choice, content-rating/
  data-safety declarations, developer verification and device-specific 16 KB install proof
  remain human/release gates.
- **Recheck trigger/date:** Before the first signed AAB, after any Unity/Android package
  change, and before Play Console upload.

### V1.0 reference UI audit (2026-08-24)

- **Question:** Which high-level mobile game hierarchy patterns are useful references
  without copying protected identity or out-of-scope service surfaces?
- **Primary source:** Read-only launch inspection of the installed Brawl Stars package
  `com.supercell.brawlstars` version `68.279` on approved Lava device
  `ST5GDW23LB004392`; local screenshot and UI dump are recorded in
  `Docs/QA/REFERENCE_UI_AUDIT_2026-08-24.md`.
- **Relevant claim:** A dominant play action, central focal point, grouped secondary
  navigation, and high-contrast status zones improve first-glance hierarchy. At the
  2026-08-24 check the device did not have an installed Smash Karts package, so no
  Smash Karts claim was made in that entry; a later 2026-08-27 observation is recorded
  separately below after the package became available.
- **Decision impact:** Apply only the hierarchy principles to BattleRaja's original
  portrait, offline, account-free product. Do not import or reproduce reference assets,
  copy, characters, typography, audio, monetisation, social, or landscape composition.
- **Uncertainty:** Human approval of touch ergonomics, orientation policy, cultural safety,
  and final visual identity remains open.
- **Recheck trigger/date:** Before changing the V1 orientation or replacing the current
  procedural UI with authored store-facing art.

### V1.0 branded Android splash (2026-08-24)

- **Question:** How should the V1 Android candidate present its launch surface without
  shipping Unity branding as the product identity?
- **Primary sources:** [Unity Player Settings splash screen manual](https://docs.unity3d.com/6000.0/Documentation/Manual/class-PlayerSettingsSplashScreen.html)
  and [Unity SplashScreenLogo API](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PlayerSettings.SplashScreenLogo.Create.html).
- **Relevant claims:** Unity supports custom Sprite logos, a configurable splash
  background, and disabling the Unity logo; the logo duration is bounded by the
  editor's splash API rules.
- **Decision impact:** Keep the V1 release entrypoint and project settings on an
  original BattleRaja icon Sprite, dark ink background, two-second logo and
  `showUnityLogo = false`. Development/M11 build paths remain unchanged. Do not
  copy reference-game branding or assets.
- **Local evidence:** Exact current source `dff3a89`, APK SHA-256
  `A6760651223052BEFB426DA08F5434ED71922A3FF9309336C1827945474F4A91`, AAB
  SHA-256 `567EF167654BC53A1836035297385278E2673411C7BD06A6257E550737E3CBF4`,
  disposable `ProjectSettings.asset` with the custom logo, and Lava captures under
  `C:\Users\USER\AppData\Local\Temp\battleraja-splash-dff3a89\`.
- **Uncertainty:** Final splash art, launch pacing, accessibility and Play Store
  branding still require human approval.
- **Recheck trigger/date:** Before signing or uploading the V1 AAB, and after any
  Unity/Android player-settings change.

### V1.0 Android/Play policy recheck (2026-08-26)

- **Question:** Did the current Play target/API, 16 KB, Data safety, target-audience and
  content-rating requirements change before the current release-shaped rebuild?
- **Primary sources:** [Google Play target API requirements](https://developer.android.com/google/play/requirements/target-sdk),
  [Android 16 KB page-size guidance](https://developer.android.com/guide/practices/page-sizes),
  [Play Data safety](https://support.google.com/googleplay/android-developer/answer/10787469?hl=en-EN),
  [Play content ratings](https://support.google.com/googleplay/android-developer/answer/9898843?hl=en),
  and [target audience/content settings](https://support.google.com/googleplay/android-developer/answer/9867159?hl=en-GB).
- **Relevant claims:** From 31 August 2026, new apps and updates must target Android 16/API
  36 or higher; apps targeting Android 15/API 35+ must support 16 KB page-size devices;
  published apps on closed/open/production tracks need a Data safety form and privacy-policy
  link even when no user data is collected (internal-only testing is the stated exemption);
  and new Play submissions must complete the content-rating questionnaire and target-audience
  declarations.
- **Decision impact:** Keep target API 36, ARM64 and the static 16 KB gate in the release
  checker. Keep the offline manifest free of network permissions. Retain Data safety,
  privacy-policy, target-audience and IARC questionnaire work as owner-controlled release
  gates; do not claim the debug-signed temporary-ID artifact is Play-submittable.
- **Local evidence:** Current APK/AAB checker passed target/min API 36/28, seven ARM64
  libraries, static 16 KB alignment, no network permissions and creative dimensions. No
  Play Console account, final package identity, signed release key or policy submission was
  accessed.
- **Uncertainty:** Google may update policy text or enforcement timing; final signed bundle,
  developer verification, package identity, privacy policy and console declarations must be
  rechecked by the owner immediately before submission.
- **Recheck trigger/date:** Before the first signed AAB and again immediately before any
  Play Console upload.

### V1.0 Android/Play policy recheck (2026-08-27)

- **Question:** Does the current official guidance change the API, 16 KB, bundle or signing
  requirements for the final offline Android candidate?
- **Primary sources:** [Google Play target API requirements](https://developer.android.com/google/play/requirements/target-sdk),
  [Android 16 KB page-size guidance](https://developer.android.com/guide/practices/page-sizes),
  and [Play Console app setup, versioning and signing](https://support.google.com/googleplay/android-developer/answer/9859152?hl=en).
- **Access date:** 2026-08-27 (IST).
- **Relevant claims:** The current Android Developers page says new apps and updates must target
  Android 16/API 36 or higher from 31 August 2026. The 16 KB guidance says 64-bit apps targeting
  Android 15/API 35 or higher must support 16 KB page-size devices, recommends ELF and bundle
  alignment checks, and documents testing with `adb shell getconf PAGE_SIZE` in a genuine 16 KB
  environment. Play setup guidance requires a digitally signed artifact, an Android App Bundle
  for upload, and monotonically managed version codes.
- **Decision impact:** Keep target API 36, ARM64/IL2CPP, static ELF and bundle alignment checks,
  bundletool/zipalign evidence, and the temporary-ID/debug-signing block. Do not call the current
  candidate publishable until the owner supplies final identity/signing and repeats the checks on
  the signed AAB; the approved Lava device's 4 KB page size is insufficient runtime 16 KB proof.
- **Local evidence:** Clean runtime/artifact source `2080383`; APK/AAB checker reports API 28/36,
  seven ARM64 libraries, no network permissions, static 16 KB alignment passed, and Android Debug
  signer fingerprint recorded in `Docs/V1_RELEASE_PLAN.md` P22. The exact APK also has a fresh
  120-second Lava match diagnostic and SurfaceFlinger frame-latency sample documented in P22-P23.
- **Uncertainty:** Policy text and enforcement dates may change; final signed-bundle processing,
  developer verification, privacy/Data Safety, content rating and Play Console declarations remain
  owner-controlled.
- **Recheck trigger/date:** Immediately before the first signed AAB and again before Play Console
  upload, especially after any Unity/NDK/Android Gradle or native-dependency change.

### V1.0 controlled reference-game UX observation (2026-08-27)

- **Question:** Which high-level entry-flow and readability principles are worth adapting
  without copying protected reference-game expression?
- **Primary sources:** Observation-only sessions of the installed packages on approved Lava
  `ST5GDW23LB004392`: Brawl Stars `com.supercell.brawlstars` version `68.279` and Smash
  Karts `com.tallteam.citychase` version `2.15.1`. No web or extracted app content was used.
- **Relevant claims:** Both landscape home surfaces make one primary play action visually
  dominant, group secondary navigation at the edges, and expose availability/lock state
  before a deeper flow. Brawl Stars uses a contextual coaching callout over the home scene;
  Smash Karts presents account-shaped entry and a locked play state in the observed install.
- **Decision impact:** Keep BattleRaja's offline Play action, fighter/mode readiness, settings,
  accessibility and rematch paths explicit and local. Preserve visible Bazaar context behind
  tutorial prompts. Do not add accounts, social controls, progression economy, copied wording,
  icons, panel geometry, characters, audio, or effects.
- **Local evidence:** Redacted observation notes and hashes are in
  `Docs/Research/REFERENCE_UX_AUDIT.md`; raw screenshots/UI dumps remain ignored under
  `Builds/Local/Device/ReferenceUx/20260827/` because the installed apps visibly contained
  existing account/profile labels and are not BattleRaja store assets.
- **Uncertainty:** Existing account/network state prevented a complete reference selection or
  in-match route without unauthorized sign-in or online actions. The audit therefore informs
  entry hierarchy only; it does not approve BattleRaja orientation, touch comfort, wording,
  accessibility, cultural fit, or final store presentation.
- **Recheck trigger/date:** Revisit only if BattleRaja's entry/navigation hierarchy changes or
  before owner approval of final store-facing UX; continue to use observation-only controls.

### V1.0 Android/Play policy recheck (2026-08-29)

- **Question:** Does the current official Android/Play guidance change the technical
  requirements that apply to the offline V1 candidate before synchronising the reviewed
  source to `main`?
- **Primary sources:** [Google Play target API level requirement](https://developer.android.com/google/play/requirements/target-sdk),
  [Android 16 KB page-size guidance](https://developer.android.com/guide/practices/page-sizes),
  and [Play Console app setup, versioning and signing](https://support.google.com/googleplay/android-developer/answer/9859152?hl=en).
- **Access date:** 2026-08-29 (IST).
- **Relevant claims:** The target-API page currently states that new apps and updates must
  target Android 16/API 36 or higher from 31 August 2026. The 16 KB guidance requires native
  libraries to use 16 KB ELF/zip alignment and recommends testing in a genuine 16 KB
  environment; it documents `PAGE_ALIGNMENT_16K` bundle configuration and `adb shell
  getconf PAGE_SIZE` runtime checks. Play setup requires a digitally signed artifact and an
  Android App Bundle for upload, with monotonically increasing version codes.
- **Decision impact:** Keep the candidate on target API 36 with ARM64/IL2CPP, static ELF and
  bundle alignment checks, bundletool verification, and the offline no-network permission
  policy. The local APK/AAB remain debug-signed with temporary package ID
  `com.example.battleraja.m11`; the owner must choose the final identity, sign the AAB, and
  repeat the checks immediately before any Play upload.
- **Local evidence:** The exact candidate checker passed target/min API 36/28, seven ARM64
  libraries, static 16 KB alignment, no forbidden network permissions, store dimensions and
  clean-worktree validation. APK SHA-256 is
  `0517EE901A9EAE943140538366B0574E893DC6BD66A5D1714D630C2379EF5FAC`; AAB SHA-256 is
  `BF52E649BFD92F277F5C9933A7FDF34FFB25410F1D5A18EF6FC3097AA31BA331`.
- **Uncertainty:** Policy text, enforcement timing, developer verification, final package
  identity, privacy/Data Safety declarations and Play Console checks remain owner-controlled
  and must be rechecked on the signed artifact.
- **Recheck trigger/date:** Immediately before the first signed AAB and again before Play
  Console upload, especially after any Unity, NDK, Android Gradle or native-dependency change.

### V1.0 Play listing and declaration recheck (2026-08-29)

- **Question:** Which current Play Console listing and declaration inputs can be prepared
  locally without claiming owner/legal approval?
- **Primary sources:** [Google Play preview assets](https://support.google.com/googleplay/android-developer/answer/9866151?hl=en),
  [Google Play content ratings](https://support.google.com/googleplay/android-developer/answer/9898843?hl=en),
  and [Google Play Data safety](https://support.google.com/googleplay/android-developer/answer/10787469?hl=en-EN).
- **Access date:** 2026-08-29 (IST).
- **Relevant claims:** Play's current preview-asset guidance specifies a 512x512 PNG app
  icon and a 1024x500 feature graphic, with separate content and metadata policy rules.
  Each app and update requires the IARC content-rating questionnaire, while published
  closed/open/production tracks require a Data safety form and privacy-policy link even
  when the app collects no user data (internal-only testing is the documented exemption).
- **Decision impact:** Keep the generated icon/feature graphic and gameplay screenshots in
  the owner-selectable store-prep folder; retain the metadata, privacy/Data Safety worksheet
  and IARC preparation as drafts. The release checklist now treats final artwork selection,
  questionnaire answers, legal/privacy review and Play Console submission as owner gates.
- **Local evidence:** `Tools/Validation/check_store_creative.ps1` passes the icon and feature
  dimensions; current candidate screenshot captures are from the exact temporary-ID APK and
  are explicitly labelled technical evidence rather than approved listing art.
- **Uncertainty:** Play may revise asset rules, questionnaire wording or enforcement timing;
  the owner must recheck the linked pages and complete declarations immediately before any
  track upload.
- **Recheck trigger/date:** Before final store-art selection, the first signed AAB, and any
  Play Console submission or update.

### V1.0 Unity Android player-settings recheck (2026-08-29)

- **Question:** Does the approved Unity 6 player-settings guidance change the local Android
  candidate configuration?
- **Primary source:** [Unity 6 Android Player Settings](https://docs.unity3d.com/6000.0/Documentation/Manual/class-PlayerSettingsAndroid.html).
- **Access date:** 2026-08-29 (IST).
- **Relevant claim:** Unity's current Android settings documentation describes the Android
  target SDK, scripting backend, architecture, bundle/version and player-configuration
  controls used to produce the platform artifact; it does not replace artifact-level Play,
  signing or 16 KB validation.
- **Decision impact:** Preserve the pinned Unity `6000.5.6f1` project settings, ARM64,
  IL2CPP, target API 36, min API 28, bundle version `1.0.0`, code `100`, portrait policy,
  and the offline no-network manifest. Do not silently change Unity or package versions.
- **Local evidence:** `ProjectSettings/ProjectSettings.asset`, `Packages/packages-lock.json`,
  the exact APK/AAB checker, and the editor build log agree on the candidate settings.
- **Uncertainty:** Final signing, package identity, store processing and device-specific
  behavior still require the owner-controlled release flow.
- **Recheck trigger/date:** After any Unity/Android/package/build-setting change and before
  signing or uploading the AAB.

### V1.0 Android/Play policy recheck - 2026-08-30

- **Question:** Which current platform and Play requirements must the local V1 candidate keep
  visible before the owner signs or uploads it?
- **Primary sources:** [Android target API requirements](https://developer.android.com/google/play/requirements/target-sdk),
  [Android 16 KB page-size guidance](https://developer.android.com/guide/practices/page-sizes),
  [Google Play Data safety](https://support.google.com/googleplay/android-developer/answer/10787469?hl=en-EN),
  [Google Play target audience](https://support.google.com/googleplay/android-developer/answer/9867159?hl=en),
  and [Google Play content ratings](https://support.google.com/googleplay/android-developer/answer/9859655?hl=en).
- **Access date:** 2026-08-30 (IST).
- **Relevant claims:** Google's current target-API page says that from **2026-08-31** new
  apps and updates must target Android 16/API 36 or higher. Android's page-size guidance
  distinguishes static alignment from behavior on a genuine 16 KB runtime environment. Play's
  Data safety guidance requires a form and privacy-policy link for published closed/open/
  production tracks, even when no data is collected; internal-only testing is the documented
  exemption. Target-audience and IARC content-rating inputs remain Play Console declarations.
- **Decision impact:** Keep target API 36, static 16 KB checks, no-network permission evidence,
  privacy/Data Safety draft, target-audience worksheet and content-rating preparation in the
  release package. Treat the Lava 4 KB device and local debug-signed AAB as technical evidence
  only; do not claim Play eligibility or runtime 16 KB support.
- **Local evidence:** P46 exact d0de949 APK/AAB checker log, bundletool/zipalign outputs and
  `Builds/Local/Device/Performance/20260830-lava-d0de949-aim/manifest.json`.
- **Uncertainty:** Google may change target dates, declaration wording, review enforcement or
  page-size requirements; final package identity, signing, legal/privacy answers and Play
  Console submission remain owner-controlled.
- **Recheck trigger/date:** Immediately before selecting the final signed package, completing
  declarations, and uploading any Play track; repeat after any Unity/Android/NDK/dependency
  change.

### V1.0 genuine 16 KB runtime smoke refresh (2026-08-30)

- **Question:** Does the exact current offline candidate execute on a genuine 16 KB Android
  runtime, and what limitation remains after that check?
- **Primary source:** [Android 16 KB page-size guidance](https://developer.android.com/guide/practices/page-sizes).
- **Access date:** 2026-08-30 (IST).
- **Relevant claim:** Android's guidance distinguishes static ELF/bundle alignment from runtime
  behavior and recommends `adb shell getconf PAGE_SIZE` in a genuine 16 KB environment.
- **Local evidence:** The exact `5d136fb` APK installed on the `BattleRaja_16K` Android 16/API
  36 AVD (`sdk_gphone16k_x86_64`) with host-GPU rendering; `getconf PAGESIZE` returned `16384`.
  The branded menu and live-match checkpoints rendered normally, the app-scoped relaunch log
  had no configured fatal/ANR/SIGSEGV/SIGABRT/shader-link marker, and the 90-second harness
  capture recorded 18 samples (manifest SHA-256
  `AC691AF0BB69983AFE0001F87A4AF92543454D3F190C61FB974734A42EE48B61`).
- **Decision impact:** Classify genuine 16 KB runtime smoke as passed for this host-GPU AVD
  profile, while retaining physical ARM64 coverage, other GPU profiles, normalized performance
  and owner review as open. A same-AVD SwiftShader attempt showed URP/Lit uniform-limit
  corruption; retain it as a superseded renderer diagnostic rather than rewrite project
  materials without evidence that SwiftShader is a required target.
- **Uncertainty:** An x86_64 emulator with arm64 support is not a physical ARM64 device, and
  emulator CPU/battery/renderer behavior cannot establish universal 16 KB compatibility or a
  mid-range performance budget. Re-run on an ARM64 physical 16 KB device and supported GPU
  profiles before claiming broad compatibility.
- **Recheck trigger/date:** Before final signed AAB selection, after any Unity/NDK/native
  dependency or renderer change, and before any Play upload.
