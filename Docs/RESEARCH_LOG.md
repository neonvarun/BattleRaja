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
