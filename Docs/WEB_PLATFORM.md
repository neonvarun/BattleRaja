# Web Platform Strategy

**Status:** First-class product requirement; Milestone 0 WebGL2/WebAssembly smoke path implemented and locally verified. Production hosting and broader browser support remain deferred.

BattleRaja will ship as an Android application and as a browser-playable Unity Web build from one shared project. The browser version is not a separate remake.

## Initial support direction

- Desktop Chrome/Edge/Firefox: primary browser targets.
- macOS Safari: support where testing confirms compatibility.
- Android Chrome/iOS Safari: experimental until performance and touch UX are proven.

## Hard rules

- Shared gameplay/domain assemblies.
- Separate Android and Web platform adapters and build profiles.
- Browser client is never trusted public-match authority.
- Cross-play protocol/content-version checks.
- Separate performance, input, lifecycle and smoke-build validation.
- No secrets in WebAssembly, JavaScript or static hosting files.

## Milestone 0 research

- Browser compatibility and WebGL 2/WebAssembly requirements.
- Memory and managed-threading limits.
- Browser networking transport.
- Background-tab throttling and reconnect.
- Initial download and caching.
- Hosting compression/MIME/cache headers.
- Photon Fusion WebGL support and authority topology.
- Identity/account linking and cross-progression.

## Milestone 0 implementation baseline

- Unity WebGL2/WebAssembly development build from the shared project.
- Local HTTP serving with Python for the first smoke test.
- Chrome and Edge are the available desktop browser targets on the current machine.
- Firefox, Safari, mobile Web, automated browser tooling, and public hosting are deferred until available and approved.
- Do not embed secrets or add browser-authoritative networking.

## Milestone 0 evidence (2026-08-02)

- Unity `6000.5.6f1` WebGL development build completed at `Builds/M0/Web`.
- `index.html` was served over `http://127.0.0.1:8000/` with Python's local HTTP server and returned HTTP 200.
- Chrome 150 and Edge 150 headless DOM checks both returned Unity bootstrap content from the served page.
- The build uses uncompressed development output for readable local diagnostics. Unity reports that `BuildOptions.AllowDebugging` is ignored on WebGL; this is expected and not a release configuration.
- Not validated: canvas rendering by visual inspection, browser console cleanliness, Firefox/Safari, mobile Web, automated WebDriver, compression/MIME/cache headers on a real host, HTTPS, CDN, or background-tab behavior.

## Official sources checked 2026-08-02

- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-browsercompatibility.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-technical-overview.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-performance.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-memory.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-gettingstarted.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-deploying.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-networking.html
- https://doc.photonengine.com/fusion/v2/technical-samples/fusion-webgl
- https://doc.photonengine.com/fusion/v2/getting-started/sdk-download
