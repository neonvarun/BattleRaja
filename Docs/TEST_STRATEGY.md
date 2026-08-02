# Test Strategy

## EditMode / pure tests

- Damage and health rules
- Cooldowns
- Status effects
- Aandhi timing
- Match transitions
- Seeded randomness
- Bot utility scoring
- Content validation

## Milestone 0 tests

- Verify pure Domain/Application assemblies compile without UnityEngine references.
- Verify seeded randomness is repeatable.
- Verify a fixed-step simulation clock advances deterministically.
- Verify the common gameplay-command contract without loading a production scene.
- Verify the Bootstrap scene opens and exits cleanly in PlayMode.
- Verify project, package, assembly, platform, content, and secret checks through the editor validation entrypoint.

## PlayMode tests

- Bootstrap
- Spawning
- Movement integration
- Projectiles and collision
- Navigation
- Object pools
- Spectating
- Arena boundaries

## Device tests

- Touch controls
- Safe areas and aspect ratios
- Thermal behaviour
- App background/resume
- Network switching
- Performance tiers

Every practical bug fix should receive a regression test.

## Web tests

- Chrome, Edge and Firefox desktop smoke tests
- Safari where available
- Keyboard/mouse/controller and non-English keyboard layouts
- Canvas focus, pointer capture and fullscreen
- Background-tab suspension and browser refresh/reconnect
- Cleared storage and account recovery
- Slow first load and cached repeat load
- Hosting compression/MIME/cache headers
- Android–Web cross-play
- Mobile-browser touch and viewport after support approval

## Milestone 0 Web coverage

- Serve the development build over local HTTP, never `file://`.
- Manually smoke-test installed Chrome and Edge.
- Check loader startup, canvas bootstrap, reload, tab hide/show, console errors, and clean exit.
- Explicitly report Firefox, Safari, mobile Web, automated WebDriver, compression, HTTPS, CORS, and CDN behavior as untested.

## M0 command evidence (2026-08-02)

- `Tools/Validation/validate.ps1 -RequireUnityProject -UnityExe "C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe"` — passed with 0 errors and 0 warnings.
- `Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform editmode -testResults Builds/M0/TestResults/editmode.xml -logFile Builds/M0/Logs/editmode.log` — 2/2 passed.
- `Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform playmode -testResults Builds/M0/TestResults/playmode.xml -logFile Builds/M0/Logs/playmode.log` — 1/1 passed.
- `Tools/Build/Android/build.ps1 ...` — development IL2CPP ARM64 APK built; installed/launched on Lava LXX508 and Oppo CPH2487.
- `Tools/Build/Web/build.ps1 ...` — WebGL2/WebAssembly development build produced under `Builds/M0/Web`.
- Python local HTTP server returned 200 for `index.html`; Chrome 150 and Edge 150 headless DOM dumps both contained Unity bootstrap content.

Unity batch logs include resolved licensing-handshake warnings and intentional empty-boundary-assembly warnings. These did not produce test failures. WebGL also reports that `AllowDebugging` is ignored; browser automation, console inspection and production hosting remain outside M0 evidence.

## Milestone 1 movement coverage

### Pure/EditMode

- diagonal and normalised movement input
- independent dead zones and sensitivity
- invalid tuning rejection
- acceleration, deceleration and maximum speed
- aim-direction persistence after aim input stops
- equal elapsed-time frame-step behavior

### PlayMode

- MovementLab player and camera references
- command-pipeline movement integration
- CharacterController collision with the arena boundary
- deceleration after input release
- aim indicator response and aim persistence
- two touch sticks and reset behavior
- Bootstrap lifecycle smoke test

### M1 evidence (2026-08-02)

- `Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform editmode -testResults Builds/M1/TestResults/editmode.xml -logFile Builds/M1/Logs/editmode.log` — 8/8 passed.
- `Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform playmode -testResults Builds/M1/TestResults/playmode.xml -logFile Builds/M1/Logs/playmode.log` — 7/7 passed.
- `Tools/Build/Web/build.ps1 ...` — M1 WebGL2/WebAssembly build succeeded under `Builds/M1/Web`.
- `Tools/Build/Android/build.ps1 ...` — M1 Android IL2CPP ARM64 APK succeeded under `Builds/M1/Android`; installation and launch succeeded on both authorized devices.
- Python local HTTP server returned 200 for the M1 page; Chrome 150 and Edge 150 DOM checks found Unity bootstrap content.

Physical touch gestures, multiple safe-area/aspect-ratio layouts, deliberate browser canvas focus/scroll behavior, and formal performance profiling remain manual follow-up coverage.
