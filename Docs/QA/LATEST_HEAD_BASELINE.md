# Latest HEAD baseline

## Exact current Android candidate — `1d743b0` / runtime `d96d3f2` — 2026-08-24

The exact checked-out source is `1d743b0` (`tools: add repeatable Lava
performance capture`); the latest runtime-bearing change is `d96d3f2`, which
pauses the offline match when Android loses focus or backgrounds the app and
resumes only the lifecycle-created pause. Full EditMode is **125/125** and
PlayMode is **71/71**.

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| APK (`BattleRaja-V1.0-release-candidate.apk`) | 39,486,559 bytes | `89156306717C5EB27EE193AD1D46809DFE19159112ADC3C77008D4C6A3C89DE0` |
| AAB (`BattleRaja-V1.0-release-candidate.aab`) | 35,313,996 bytes | `F5776F6AF19EE1C0A803D76050A80E62E883710E00296EF9603CED279D2227C1` |

The packages were built from exact source in disposable worktree
`C:\BRLifecycle`. The AAB contains seven ARM64 native libraries, no other
native ABIs, and passed the static 16 KB ELF LOAD alignment check. The APK
installed only on Lava `ST5GDW23LB004392`; launch resolved to
`com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`, but
the device lock screen remained active (`mDreamingLockscreen=true`). No
interactive menu, tutorial, or full-route evidence is attributed to this
candidate. The packages are debug-signed with temporary identity
`com.example.battleraja.m11` and are not publishable. The repeatable capture
harness is `Tools/Validation/capture_android_performance.ps1`.

## Exact tutorial action-gate candidate — `65e6001` — 2026-08-24

The exact current runtime source is `65e6001` (`tutorial: gate progression on
real offline actions`). Tutorial progression is now authority/telemetry-driven:
the continue button remains locked until the required movement, aim, attack,
ability, gadget collection/use, Aandhi, elimination or victory action has been
observed. Replay reloads a fresh tutorial arena. EditMode is **125/125** and
PlayMode is **70/70**.

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| APK (`BattleRaja-V1.0-release-candidate.apk`) | 39,484,371 bytes | `0F9AD792D2479ADC1F57BCCACF28921DB327A6D926990560762A7FC977DD7DB8` |
| AAB (`BattleRaja-V1.0-release-candidate.aab`) | 35,311,798 bytes | `F0049D127717FE7EB1BF8FB6F13E7699133F76FBA135EB18960AD48AE01EEAE7` |

Both packages were built from the exact source in disposable worktree
`C:\BRTutorial`. The AAB contains seven ARM64 native libraries, no other native
ABIs, and passed the static 16 KB ELF LOAD alignment check. The APK installed
successfully on the approved Lava device `ST5GDW23LB004392`. The device was
locked during the attempted interactive run, so the screenshots are lock-screen
captures and do not constitute tutorial UI or action-by-action evidence. The
tutorial remains **In progress / Human review required** until the owner unlocks
Lava and performs the real flow. These debug-signed packages use temporary
identity `com.example.battleraja.m11` and are not publishable. No Web build was
attempted for this Android-only V1 scope.

## Exact runtime release-shaped Android package baseline — `062066b` — 2026-08-24

The latest runtime-bearing source is `062066b` (`docs: align exact head after
visual cleanup`). A fresh release-shaped Android APK and AAB were built from
this exact source in disposable worktree `C:\BRS`; only the APK was installed
on the approved Lava device `ST5GDW23LB004392`. Later commits on this branch
are documentation/store-evidence follow-ups and do not change runtime code.

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| APK (`BattleRaja-V1.0-release-candidate.apk`) | 39,482,035 bytes | `C7B16D01DEA3ED3ADA1B5E5AA421B82ADBA46F5E1A0A2B0283F409BC59F3E245` |
| AAB (`BattleRaja-V1.0-release-candidate.aab`) | 35,309,464 bytes | `4D3948F876580AC45A0655593DAA6FE4AF70BC9BACF78840F33EC63E8775E858` |

The APK completed the real menu → mode → fighter selection → live eight-actor
offline match → Aandhi/resolution → results route on Lava. The matching AAB
contains seven ARM64 native libraries, no other ABIs, and passed the static
16 KB ELF LOAD alignment check. These local artifacts are debug-signed with
the temporary package identity `com.example.battleraja.m11`; they are not
publishable. Exact Web build/browser evidence remains blocked by the known
Unity Bee/Burst issue.

## Exact current lifecycle follow-up — `53da0f3` — 2026-08-24

The current HEAD is `53da0f3` (`visuals: release generated ground mesh on
teardown`). This is a lifecycle-only follow-up to the tested visual slice at
`a5597f5`: generated Bazaar mesh objects are now explicitly released on scene
teardown. Repository validation is **0 errors / 0 warnings**, EditMode is
**125/125**, and PlayMode is **70/70**. The Android artifacts and Lava captures
below were built from the runtime-equivalent parent `a5597f5`; no new package
was required for this cleanup-only change. Web remains unbuilt and blocked by
the existing Bee/Burst issue.

## Exact current visual slice — `a5597f5` — 2026-08-24

The checked-out branch is `codex/v1-playstore-release` at `a5597f5`
(`visuals: add original ground mosaic and UI frame treatment`). Repository
validation is **0 errors / 0 warnings**, EditMode is **125/125**, and PlayMode is
**70/70**. The change is presentation-only: Bazaar Bastion now has a single
render-only, three-material ground mosaic with no collider, and the UI backdrop
has a restrained cyan/warm frame treatment that is non-interactive.

A fresh development APK was built from this exact source in disposable worktree
`C:\BRv1vis` and installed only on Lava `ST5GDW23LB004392`:

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| APK (`BattleRaja-BazaarBastion-M11.apk`) | 92,734,792 bytes | `05CBD516DD429EAE8AF1882E719E2FAA95E63949C90261208215B02AF56B9A18` |
| AAB (`BattleRaja-V1.0-release-candidate.aab`) | 35,310,447 bytes | `0B32C23A9F4E656790B9470AEE80D6375FF6E264A7551956A201129D3CDA7729` |

The APK reached the real offline match route and displayed the mosaic, fighters,
gadget HUD, action controls and Aandhi boundary. Captures are outside the
repository at `C:\Users\USER\AppData\Local\Temp\battleraja-a5597f5-menu.png`
and `C:\Users\USER\AppData\Local\Temp\battleraja-a5597f5-match2.png`.
The sampled log contained no fatal/ANR/SIGSEGV/NullReferenceException/
UnityException markers. This is technical presentation evidence, not final
visual, touch, accessibility, performance or Play Store approval. The exact
Web build remains blocked by the existing Bee/Burst issue and was not rerun for
this Android-only slice.

## Exact current packaging refresh — `3bbe7d1` — 2026-08-24

The checked-out branch is `codex/v1-playstore-release` at `3bbe7d1` (Unity
metadata-only follow-up to the runtime source `f3dea5d`). A fresh local
release-shaped Android bundle and APK were built in disposable worktree
`C:\Projects\BattleRaja-v1-current`:

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| AAB | 35,301,185 bytes | `20709BDDC90F418EFFED493E209A1CA943F5F1B119017AE415672491B9FC9EFF` |
| APK | 39,473,743 bytes | `796675A71F6127AAB95B4B6C2CEB727888C77904937CAF23B1D53E3A92DFC771` |

Bundle validation found seven arm64-v8a libraries, no other native ABIs, and
passed 16 KB ELF LOAD alignment. The exact-current APK was installed only on
Lava `ST5GDW23LB004392`; the cold-launch menu capture is outside the repository
at `C:\Users\USER\AppData\Local\Temp\battleraja-current-3bbe7d1-menu.png`.
No fatal/ANR/SIGSEGV/NullReferenceException/UnityException marker was found in
the sampled launch log. The bundle is debug-signed and non-publishable; package
identity, release signing, Web build, performance, accessibility, human review
and Play Store gates remain open.

## Documentation HEAD route confirmation — `3600d8b` / runtime `f3dea5d` — 2026-08-24

The documentation HEAD is `3600d8b` (`docs: record current V1 bundle evidence`);
the installed runtime APK used for this route capture is the unchanged runtime
source `f3dea5d`. On Lava `ST5GDW23LB004392`, correctly derived 1080×2460 portrait
coordinates completed menu → offline mode → fighter selection → live eight-actor
match → Aandhi warning/closing → results → menu. The capture directory is
`C:\Users\USER\AppData\Local\Temp\battleraja-current-f3dea5d\` and is intentionally
outside the repository. No fatal, ANR, SIGSEGV, NullReferenceException, or
UnityException marker was found in the sampled route log. This is technical
route evidence only; a compact-layout tap also changed the gadget HUD to empty
with `Tiffin Station deployed` feedback. Pickup accessibility, touch ergonomics,
sustained performance, human visual/cultural review, signing, and Play Store gates
remain open. The exact Web build remains blocked in the disposable Bee/Burst
diagnostic and no Web pass is claimed.

Local exact-HEAD checks after the evidence update: repository validation **0
errors / 0 warnings**, EditMode **125/125**, and PlayMode **70/70**. Unity's
batch-test logs still contain the known Fusion editor custom-dependency import
warning and licensing-client warning; these did not fail compilation or tests
and are not counted by repository validation.

## Exact current source update — `f463b1b` — 2026-08-24

The checked-out branch is `codex/v1-playstore-release` at `f463b1b`
(`vfx: add render-only Aandhi boundary cue`). Repository validation is **0 errors
/ 0 warnings**. A fresh Android release-candidate APK built from this exact
source in disposable worktree `C:\Projects\BattleRaja-validate-aandhi`:

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| APK | 39,473,715 bytes | `ADFE38B3C11DE2119D7180967C48165682095C4818F9FD0140FB694F1198A666` |
| AAB | 35,301,175 bytes | `C19A238FB31530EAC2AA920ED7B760F76C91D2590B6C27506887FED8170766B8` |

The cue is render-only: it draws the current boundary and warning-state next
boundary from the existing match snapshot. The APK was not installed on Lava.
The matching AAB was built at
`C:\Projects\BattleRaja-v1-aab-current\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab`;
bundle validation found a base manifest, seven arm64-v8a libraries, no other
native ABIs, and passed 16 KB ELF LOAD alignment checks.
The exact Web build was attempted locally but the Unity Web Bee/Burst backend
repeatedly returned exit code 4 without producing a completed player; no Web
artifact or browser smoke pass is claimed for this source.

## Exact current source update — `ecdb25b` — 2026-08-24

The checked-out branch is `codex/v1-playstore-release` at `ecdb25b`
(`ui: hide bot diagnostics in release builds`). Repository validation is **0
errors / 0 warnings**. A fresh Android release-candidate APK built from this
exact source in disposable worktree `C:\Projects\BattleRaja-validate-ecdb25b`:

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| APK | 39,465,083 bytes | `7BDEA277C28CED29367CD3A76A73DCC7DFF45EDBBCAD1F9F9415C64FA7B57AD4` |

The change gates engineering bot labels to editor/development builds; no scene
YAML was changed. The APK was not installed on Lava for this focused presentation
change, and no current AAB, browser build or formal performance pass is claimed.

## Exact current source update — `fe80582` — 2026-08-24

The checked-out branch is `codex/v1-playstore-release` at
`fe80582efc35368a03afea314d53e071ac1872bf` (`android: bind production UI to
touchscreen input`). Repository validation is **0 errors / 0 warnings** and the
full PlayMode suite is **70/70** after adding explicit `<Touchscreen>` point and
press bindings to the runtime UI module. The exact release-shaped APK was built
from this source in `C:\Projects\BattleRaja-validate-fe80582`:

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| APK | 39,465,411 bytes | `BC74E8C09C853AB8EBE089B0B6F5C063D47A9963F2F940EF48D657FFADAB3E23` |

The APK was installed only on Lava `ST5GDW23LB004392`. The exact-current portrait
menu and splash transition were captured under
`C:\Users\USER\AppData\Local\Temp\battleraja-touch-fe80582\`; no fatal/ANR/SIGSEGV
marker was found in the sampled log. ADB coordinate taps did not provide a
formal route proof, so physical touch navigation, accessibility and the full
menu-to-match path remain human-review gates. No AAB was rebuilt from this
source; the prior AAB evidence must not be attributed to `fe80582`.

Date: 2026-08-24
Branch: `codex/v1-playstore-release` (ahead of local and remote `main`; exact count is recorded by Git)
Local/remote `main`: `ca6ec3e17e695042664cf3bdbf9889b259b33144`
Latest validated runtime source: `dff3a89` (`docs: record branded Android splash evidence`)
Latest checked-out source: `dff3a89`; the checked-out Git HEAD is the authoritative
current documentation and release-settings state.
Balance-fix runtime source: `17a8c75` (`fix(balance): land documented weapon retune in Core definitions`)
Pre-fix baseline source: `35d723f` (`fix: bot perception no longer treats fighter hulls as line-of-sight blockers`; the tip of local `main` before this branch)
Unity: `6000.5.6f1` (`C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe`)

## Exact current V1 branded-splash baseline — 2026-08-24

The current HEAD is `dff3a8925304fee72f391aa89047119cbbbc6f69` (`docs: record branded
Android splash evidence`). Repository validation remains **0 errors / 0 warnings**;
EditMode is **125/125** and PlayMode is **69/69**. The release-shaped APK was built
from this exact source in `C:\Projects\BattleRaja-validate-dff3a89` and is
**39,466,543 bytes** with SHA-256
`A6760651223052BEFB426DA08F5434ED71922A3FF9309336C1827945474F4A91`. The matching
AAB is **35,293,988 bytes** with SHA-256
`567EF167654BC53A1836035297385278E2673411C7BD06A6257E550737E3CBF4`.

The APK was installed only on Lava `ST5GDW23LB004392`. Cold-launch evidence is in
`C:\Users\USER\AppData\Local\Temp\battleraja-splash-dff3a89\`: the dark native
surface transitions to the original BattleRaja icon and then the offline menu. No
fatal/ANR/SIGSEGV marker was found. The release entrypoint and project settings now
disable the Unity logo for V1 and use the BattleRaja icon as the two-second splash
logo. The AAB has 7 ARM64 libraries, no other ABIs and passed static 16 KB ELF
alignment. Full post-splash route review, performance evidence and human approval
are still required.

## Exact current Android control-rotation baseline — 2026-08-24

The latest runtime source is `b954a72`. Repository validation is **0/0**; EditMode is
**125/125** and PlayMode is **69/69**. The exact-source APK is **39,525,752 bytes**
with SHA-256
`3ABCEF91BF14239AD8D6ED5511D7C74D2C0DA3DB3CC35DCE838573AEB39E1630`; it was
installed only on Lava `ST5GDW23LB004392`, where the menu-to-match diagnostic route
and opening frame were captured. The APK is the current Android evidence boundary;
the Web artifact below remains from `7751f53` and is not exact-current after this
Android-only control change.

## Exact current responsive-menu baseline — 2026-08-24

The latest runtime source is `7751f53`. Repository validation is **0/0**; EditMode is
**125/125** and PlayMode is **68/68**. The exact-source release-shaped APK is
**39,526,908 bytes** with SHA-256
`620832E983A33505FEA97AF638E7D681A334E8E51474F7014B2F8D011C801F4A`; it was
installed only on Lava `ST5GDW23LB004392`. The wide menu capture is outside the
repository at
`C:\Users\USER\AppData\Local\Temp\battleraja-responsive-7751f53\menu-landscape.png`.
The capture verifies the responsive hero/CTA hierarchy; touch navigation and human
accessibility approval remain open.

The exact Web output contains 19 files totalling **132,073,844 bytes**; WASM is
**119,801,877 bytes** (`E91A5242FB71733E41FA52182572DE784CE97413DB3B15C4D4302FAFEAD3CAE0`).
Local HTTP returned 200 and Chrome/Edge loader captures succeeded, but neither reached
the interactive menu in the bounded headless wait.

## Exact current offline packaging baseline — 2026-08-24

The latest exact-current source is `6ac5c12`. Repository validation is **0/0**;
EditMode is **125/125** and PlayMode is **67/67**. The release-shaped APK is
**39,523,632 bytes** (`09F5375FA8D5DEC066A09D8CCDF0BAF01269F4B402252EF2908691C773402EF3`)
and the AAB is **35,351,357 bytes**
(`70825F82A4D79E1E036F4DA8A286778244406D51B1D60A568BD066ED1B82DAA8`). The AAB is
ARM64-only and passed the static 16 KB alignment check. The APK was installed only on
Lava `ST5GDW23LB004392`; the release-shaped menu is visible and no fatal marker was
found. Runtime EventSystem point/click bindings are now explicit and covered by the
PlayMode suite, but the Unity surface exposes no actionable Android UI nodes, so
physical touch navigation remains a human-review gate. Raw capture is outside the
repository at
`C:\Users\USER\AppData\Local\Temp\battleraja-lava-6ac5c12\`.

The exact Web build contains 19 files totalling **132,071,712 bytes**; WASM is
**119,799,965 bytes** (`8CE68A5AA4C741DD27AD66B9BF61FBC0B17DE9F632F2C791181EC99F516DEA12`).
Local HTTP, Chrome and Edge loader smoke succeeded, but bounded headless captures did
not reach an interactive menu. This remains a prototype baseline, not a release gate.

The final documentation checkout is `HEAD` on `codex/v1-playstore-release`;
runtime/package source is `f4425d6`. Repository validation is
**0 errors / 0 warnings**;
fresh exact-current EditMode is **125/125** and PlayMode is **66/66**. The release-
shaped APK is **39,529,326 bytes**, SHA-256
`AE74717B597C4CBCFDECF7D8DB719C177100F495CC084ABFD0E1EA6AAD3E2C52`; the AAB is
**35,357,477 bytes**, SHA-256
`8EB49EFC8D58D144E5A792224FC9A3570FF4E37F121E06B6E55093C9D4D5F5E7`. The AAB has
7 ARM64 native libraries, no other ABIs, and passed static 16 KB ELF alignment.

The APK was installed only on Lava `ST5GDW23LB004392`, where the branded offline
menu was captured and no fatal/ANR/SIGSEGV marker appeared. Its inspected permission
set is `VIBRATE` plus Unity's dynamic receiver only; `INTERNET` and
`ACCESS_NETWORK_STATE` are absent. This packaging boundary retains the imported
Fusion files but excludes their Android runtime assemblies from the offline APK.

The exact checkout also built Web successfully. The current Web output contains
19 files (about 132.1 MB); WASM is **119,799,945 bytes**, SHA-256
`05EF2D0A69EE3E6DD8B7552913E892D749266135F216F17061560FAFDA8BD09F`. Local HTTP
returned 200 and Edge headless reached the Unity loader; full interactive browser
route approval remains open. Raw Android captures are outside the repository at
`C:\Users\USER\AppData\Local\Temp\battleraja-root-offline-manifest-lava\`.

## Exact current checkout release-shaped Android evidence — 2026-08-24

The checked-out branch tip is now `357dfdf1e6289c172dab60e514f555ba3d5bc914`;
the release artifacts below were built from the immediately preceding runtime-equivalent
source `46724ac2dfa403f40f58669240e61918c2a94d1b`
(`Revert "android: disable forced internet permission for V1"`). The two short-lived
Android permission experiments immediately before this commit were reverted completely;
the runtime source is therefore unchanged from the warning-clean V1 candidate. The two
commits after the build contain documentation only. Exact-source validation at the
runtime-equivalent checkout reports **0 errors / 0 warnings**. The last full suites on the
runtime-equivalent source remain **125/125 EditMode** and **66/66 PlayMode**; the current
HEAD changes contain no gameplay or editor source changes after that run.

| Check | Result | Evidence |
| --- | --- | --- |
| Release-shaped APK | **40,431,927 bytes**, SHA-256 `0694958A43F1BADD30E697095F249733992F9D6904E10E1923CD0CAF01010C78` | `C:\Projects\BattleRaja-v1-final-verify\Builds\V1\Android\BattleRaja-V1.0-release-candidate.apk` |
| Release-shaped AAB | **36,262,036 bytes**, SHA-256 `906D85FA00E4A9787A0C1DE892DC3F27A098ACF21BB1735E08C977565A1D09A4` | `C:\Projects\BattleRaja-v1-final-verify\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab` |
| AAB structure | base manifest, 8 ARM64 libraries, 0 other ABIs, 450 entries | `check_android_bundle.ps1`; all native LOAD segments `0x4000` |
| APK manifest | package `com.example.battleraja.m11`, version `1.0.0`/code `100`, target API 36; `VIBRATE`, Unity dynamic-receiver permission, `INTERNET` and `ACCESS_NETWORK_STATE` remain | `aapt dump badging/permissions`; remove or justify network permissions before Play submission |
| Lava launch | foreground Unity activity, branded offline menu, no fatal/ANR/SIGSEGV markers | `C:\Users\USER\AppData\Local\Temp\battleraja-final-head-lava\menu.png` and `logcat.txt` |
| Bounded device sample | 257,340 KB PSS / 393,462 KB RSS / 83,862 KB Graphics / 83 KB swap; SurfaceView log samples around 59.45–60.59 FPS | `C:\Users\USER\AppData\Local\Temp\battleraja-final-head-lava\meminfo.txt`, `logcat-postlaunch.txt` |

This remains technical release-candidate evidence, not a Play approval: the package ID,
signing identity, manifest permission review, sustained performance, accessibility,
human visual/cultural review and store/legal gates remain open. The Web and Photon/PlayFab
scopes remain intentionally untouched.

## Current Android V1 visual-polish source — 2026-08-24

The current runtime-bearing source is `c6badbf6cf5b1c7340fa907821aeb4cbf2194bc0`
(`fix: keep tutorial completion card visible`) on `codex/v1-playstore-release`. It retains the
offline Android scope and the authority-backed Tiffin route from `d825832b`, while
replacing the Bazaar center landmark's intersecting bars with a six-panel fictional
canopy and enlarging the phone hero graphic. No networking or backend work was added.

| Check | Result | Evidence |
| --- | --- | --- |
| Repository validation | **0 errors / 0 warnings** | root `Tools/Validation/validate.ps1` |
| Full EditMode | **125/125 passed** | disposable `C:\Projects\BattleRaja-v1-visual-verify\Builds\V1\TestResults\editmode-visual-polish4.xml` |
| Full PlayMode | **66/66 passed** | disposable `C:\Projects\BattleRaja-v1-visual-verify\Builds\V1\TestResults\playmode-visual-polish.xml` |
| Android APK | **40,431,923 bytes**, SHA-256 `E6CBEAD6F97C036C0C9D1663CA5972799AEF3B330D75A3D2AAA94D5E699C7DB3` | exact tutorial-fix disposable build, Lava install |
| Android AAB | **36,262,021 bytes**, SHA-256 `124E14ABE6012B3B42D7B7741D0C647416E278E82ABFE358EF89A53BAAD64021` | base manifest, 8 ARM64 libraries, 0 other ABIs, `0x4000` ELF LOAD alignment |
| Lava visual inspection | tutorial skip reaches visible completion card; exact correction also shows successful Tiffin use; preceding exact visual source covered menu, match, results and rematch | `C:\Projects\BattleRaja-v1-tutorial-verify\Builds\V1\Lava\tutorial-fix-complete.png`, `tutorial-fix-gadget-after.png` plus prior `visual-polish-*.png` |

The APK/AAB remain debug-signed with temporary package ID `com.example.battleraja.m11`.
The preceding exact visual run reached results and returned through REMATCH to a fresh
match; the current correction run also captured a successful Tiffin pickup/use. A separate
placement probe after moving to an edge returned `InvalidPlacement`, which is retained as
a truthful negative observation. Stable FPS, performance series, final art/audio,
accessibility, signing, store, legal, cultural and human review gates remain open.
Classification remains **prototype**.

## Latest source warning-clean continuation — 2026-08-24

Checkout source now includes `649d0bb` (`fix: remove obsolete Unity lookup overloads`).
This is editor/test-only, so the runtime-bearing candidate remains `c6badbf`; it removes
the remaining authored Unity 6 lookup deprecations. The disposable exact-source run
passed EditMode **125/125** and PlayMode **66/66**. The Android build produced a
**40,431,911-byte** APK with SHA-256
`51D86184F6C69DD30CD249D273FA0F8F5BA96B4159D86DD1472FE4FD54320DA5`, and its log had
**0 `CS0618` warnings** and **0 C# errors**. The matching AAB is **36,262,033 bytes**,
SHA-256 `518102EAE7DDB71DA9393ABE3E948A47440260C9DF8D19532AAFF14FA1BE98B0`; the
bundle checker passed ARM64-only and `0x4000` ELF alignment. The build was not installed because no
runtime code changed; current Lava captures remain tied to `c6badbf`.

## Current offline Android gadget-route slice — 2026-08-24

Runtime source `d825832bced4c5e07c7967d891696842eb55609a` retains render-only fighter
state motion, pooled impact halos and animated gadget identity visuals, and makes the
production Tiffin pickup/use route player-owned. Full details and the exact
artifacts/captures are in
`Docs/QA/V1_ANDROID_VISUAL_FEEDBACK_2026-08-24.md`.

| Check | Result | Evidence |
| --- | --- | --- |
| Repository validation | **0 errors / 0 warnings** | `Tools/Validation/validate.ps1 -RequireUnityProject -UnityExe ...6000.5.6f1...` |
| Full EditMode | **125/125 passed** | disposable `...20260824e/Builds/V1/TestResults/editmode-visual-feedback.xml` |
| Full PlayMode | **66/66 passed** | disposable `...20260824j/Builds/V1/TestResults/playmode-route2.xml` |
| Android APK | **40,429,675 bytes**, SHA-256 `50FD2D7F9C29F4888F2965810F9FD8130F7C2857F2A15AD7E3A5CF5908E7BFCC` | installed/launched on Lava only |
| Android AAB | **36,259,768 bytes**, SHA-256 `052F9CAB180E15AEEC0C2D8DCAB47187C53C58F07629C69F81A647697DB9FBF1` | 8 ARM64 libraries; static 16 KB alignment passed |
| Lava visual smoke | Menu, mode, fighter selection, match, movement, attack/ability and Tiffin use; no fatal marker | raw captures outside source |
| Gadget visual smoke | Tiffin pickup and use captured; authority HUD and visible station confirmed | `v1-match-near.png`, `v1-tiffin-used.png` |
| Device sample | **285,919 KB PSS / 422,810 KB RSS / 100,226 KB Graphics / 69 KB swap**; no usable frame histogram | raw Lava dumps |

Classification remains **prototype**. The debug-signed temporary package, runtime 16 KB
environment, FPS/performance series, successful gadget-use capture, human review and store
gates remain open.

## Current offline Android V1 candidate — 2026-08-23

The V1 release-shaped candidate is recorded on `codex/v1-playstore-release` after the
intentional offline-product changes were committed. This section supersedes the older
closure rows for Android V1 claims.

V1 runtime artifact source commit: `ab5b12ad7c86f425243fc3f2a9cbc83ae97e6f6d`.
The evidence/doc update follows that focused runtime commit; generated Unity/build
outputs remain outside the repository.

| Check | Result | Evidence |
| --- | --- | --- |
| Repository validation | **0 errors / 0 warnings** | `Tools/Validation/validate.ps1 -RequireUnityProject -UnityExe ...6000.5.6f1...` |
| Full EditMode | **125/125 passed** | `Builds/V1/TestResults/editmode-v1-final.xml` |
| Full PlayMode | **64/64 passed** | `C:\Projects\BattleRaja-v1-verify-20260823c\Builds\V1\TestResults\playmode-visual-kit-run.xml` |
| Android release-shaped APK | **40,420,983 bytes**, SHA-256 `E70241D83E6DBDA977EECF9F476502FD68B89799438DBA06F024423D575E5532` | non-development debug-signed APK, installed on Lava |
| Android release-shaped AAB | **36,251,072 bytes**, SHA-256 `4B22FD2DADD26FB1A5FEA96FE5EAA19BC2D0EC4F130F87009969D38562FE84C6` | base manifest, 8 ARM64 libraries, 0 other ABIs, 450 entries; static 16 KB alignment passed |
| Lava runtime | Menu, mode, fighter selection, match opening, movement and attack/ability interaction; distinct fighter/arena identities; no app fatal markers | `Docs/QA/V1_ANDROID_EVIDENCE_2026-08-23.md`, raw evidence outside source |
| Performance sample | **285,509 KB PSS / 421,336 KB RSS / 99,160 KB Graphics / 79 KB swap**; no frame histogram exposed by `gfxinfo` | Lava `dumpsys`/log files outside source |
| Release boundary | Prototype; package ID/signing/adaptive icon/16 KB/human/store/legal/performance gates open | V1 evidence and release checklist |

## M11 closure slice - 2026-08-23

Bounded performance/UX closure stopped for owner-requested clean publication.
Full details and limitations: `Docs/QA/M11_CLOSURE_REPORT_2026-08-23.md`.

| Check | Result | Evidence |
| --- | --- | --- |
| Starting repository | `main` aligned at `9c91f76`, clean; two stashes preserved | preflight output |
| Baseline validation | **0 errors / 0 warnings** | detached exact-source `9c91f76` validation |
| Baseline Android | exact APK **94,253,553 bytes**, SHA-256 `7ED0E10DDB2FD1F2D0D2C0E64584AC4BE8840CFBE2262E00C375E311FBEC81EB`; Lava cold launch passed | `Builds/Local/M11Closure/2026-08-23/artifact-manifest.json`, device captures |
| Baseline Web | **19 files / 134,170,348 bytes**; WASM **121,427,473 bytes**, SHA-256 `BB722EC437DE934CDDEEF06D1A594604A46F495BDE14A442C138A3ECAF8B14CB`; Chrome/Edge six routes passed with 0 errors/failed requests | `webperf/chrome-performance.json`, `edge-performance.json`, screenshots |
| Runtime fixes/tests | `c11954b` EditMode **125/125**, PlayMode **59/59**; `5d6eeb8` PlayMode **59/59** | `TestResults/editmode-final.xml`, `playmode-final.xml`, `playmode-tutorial-fix.xml` |
| Platform candidate | `c11954b` APK **97,529,808 bytes**, SHA-256 `B12C2BCD3C749D1D5ABAF01A2E37C71816B9E8B8AE71BEBC9EA8D1744A952502`; Web **19 files / 134,170,499 bytes**; WASM SHA-256 `8D6B5673D598D881FF62A3B45AF24A18828BB75497BC757CF802003FF97F31EE` | `final-artifact-manifest.json` |
| Not claimed at final tip | Android/Web rebuild/install/smoke, 20-device rematch cycle, parsed CPU/GPU/frame profiling, human UX approval | closure report remaining-gates table |

## Milestone 11 Phase 3-5 exact-source regression — 2026-08-23

Validated from detached exact-source worktree `C:\Projects\BattleRaja-headbuild`
at `73237c8`. The main checkout's owner-protected scene/prompt changes were not
used, staged, committed, or overwritten.

| Check | Result | Evidence |
| --- | --- | --- |
| Repository validation | **0 errors / 0 warnings** | `Tools\Validation\validate.ps1` |
| Full EditMode | **125/125 passed**, 0 failed/skipped | `Builds/Local/TestResults/editmode-final.xml`, `Builds/Local/Logs/editmode-final.log` |
| Full PlayMode | **57/57 passed**, 0 failed/skipped; duration **40.8994214 s** | `Builds/Local/TestResults/playmode-head.xml`, `Builds/Local/Logs/playmode-head.log` |
| Deep replay soak | **1,000 seeds x 2 = 2,000 matches**, zero divergence, duration **416.1411007 s** | `Builds/Local/TestResults/deep-soak-1000.xml`, `Builds/Local/Logs/deep-soak-1000.log`, `Docs/QA/REPLAY_AND_SOAK_REPORT.md` |
| Android build | BazaarBastion development APK succeeded; APK **94,258,745 bytes**; SHA-256 `F7E76A5DFB88633047075BB9EA28655F15B9CA65FE1EAE3205D165A4EB56A376`; two known editor `CS0618` warnings, no C# errors/build failure | `Builds/M11/Android/BattleRaja-BazaarBastion-M11-73237c8.apk`, `Builds/Local/FinalEvidence/android-build-73237c8.log` |
| Lava Android runtime | Installed/launched only on Lava `ST5GDW23LB004392`; `UnityPlayerGameActivity` was top-resumed; sampled memory **418,669 KB PSS / 523,264 KB RSS / 82,088 KB Graphics / 460 KB swap**; home/resume returned to top-resumed activity; sampled app logcat had no fatal exception, SIGSEGV/SIGABRT marker | ADB output, `Builds/Local/Device/lava-head-logcat.txt`, `Builds/Local/Device/lava-head-logcat-resume.txt` |
| Web build | BazaarBastion development Web succeeded; **19 files / 134,170,277 bytes**; WASM **121,427,473 bytes**, SHA-256 `BB722EC437DE934CDDEEF06D1A594604A46F495BDE14A442C138A3ECAF8B14CB`; same two known editor warnings, no C# errors/build failure | `Builds/M11/Web-BazaarBastion-73237c8`, `Builds/Local/FinalEvidence/web-build-73237c8.log` |
| Web serve | Local HTTP returned **200** for page, data, framework and WASM | Python HTTP server on `127.0.0.1:8016`, server request log |
| Chrome + Edge smoke | Chrome and Edge each passed desktop **1280x720**, tablet **1024x768**, and portrait **390x844**: mode selection, fighter selection with Bijli selected, and active match were visually verified. Totals: **6/6 routes passed**, console errors **0**, failed requests **0** | `Builds/Local/WebSmoke/smoke-results.json`, `Builds/Local/WebSmoke/*-mode.png`, `*-fighter.png`, `*-match.png` |

Build-time scene generation normalized TutorialArena YAML in the detached worktree,
and Unity removed orphan Photon `.meta` files whose binary payloads are intentionally
untracked/prohibited at runtime. These changes stayed isolated to the disposable
detached worktree and were not copied back to `main`.

## Phase 0 exact-current-source rebaseline (2026-08-22)

Fresh evidence captured from the exact current sources after a handoff snapshot that
was recorded against stale HEADs (`d64da36` and older). Two bounded commits were made
on this branch after the pre-fix baseline was captured; both are documented below.
The worktree contained recurring editor churn; its root cause was diagnosed and fixed
rather than re-stashed (see the weapon-retune correction note in
`Docs/BALANCE_CHANGELOG.md`).

### Pre-fix baseline — source `35d723f`

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository state | Local `main` ahead of `origin/main` by 4 unpushed commits; worktree dirtied only by editor churn (weapon assets contradicting BALANCE_CHANGELOG, scene field-sync) plus tracked test artifacts; churn preserved in git stash `phase0-rebaseline-preserved` | `git log origin/main..main`, 2026-08-22 |
| Repository validation | `Tools\Validation\validate.ps1 -RequireUnityProject -UnityExe ...6000.5.6f1...` — **0 errors, 0 warnings** | command output, 2026-08-22 |
| Full EditMode | **114/114 passed**, 0 failed, 0 skipped | `Builds/Local/TestResults/editmode.xml` |
| Full PlayMode | **57/57 passed**, 0 failed, 0 skipped | `Builds/Local/TestResults/playmode.xml` |
| Android build | `BuildAndroidBazaarBastionDevelopment` succeeded; APK **165,864,897 bytes**, SHA-256 `77B2BD047B525505AC47B6745C22E2A2799522D2D377C67BA7576ED399A43D2A`; build log had no C# error/warning lines | `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk`, `Builds/M11/Logs/android-build.log` |
| Lava runtime | Exact APK installed/launched only on `ST5GDW23LB004392`; `UnityPlayerGameActivity` top-resumed; sampled memory **426,104 KB PSS / 529,736 KB RSS / 84,136 KB Graphics / 436 KB swap**; 0 fatal/AndroidRuntime-crash/SIGSEGV markers in sampled logcat | ADB output, 2026-08-22 |
| Web build | `BuildWebBazaarBastionDevelopment` succeeded; **19 files / 134,007,166 bytes**; `Web-BazaarBastion.wasm` **121,275,261 bytes**, SHA-256 `F931B040E3F59B0DC9D32FD9F5FCE1A09DDB70154F6C4CB81273354087C3FDC0`; no C# error/warning lines | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |
| Web serve/smoke | Local HTTP `127.0.0.1:8015/index.html` returned **200**; Playwright Chromium smoke: Chrome 1280×720, Chrome 390×844 portrait, Edge 1280×720 each rendered the main menu canvas with **52 console messages, 0 errors, 0 failed requests** | temp Playwright harness screenshots, 2026-08-22 |

### Defect found during rebaseline

The 2026-08-21 balance entry recorded Bijli/Pehel/Maya damage as 12/20/9 but only in
serialized `.asset` files; the authoritative Core definitions still carried 18/28/12,
and `BuildEntrypoints` re-syncs assets from definitions, silently reverting every
regeneration and shipping unretuned damage in all prior builds. Fixed at `17a8c75`
(see `Docs/BALANCE_CHANGELOG.md` correction note). The same diagnosis closed the
recurring "phase0-preserved-wip" stash churn.

### Post-fix evidence — source `17a8c75` (+ scene/artifact chore `c78433a`)

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | **0 errors, 0 warnings** | command output, 2026-08-22 |
| Full EditMode | **114/114 passed**, exit 0 | `Builds/Local/TestResults/editmode-balancefix.xml` |
| Full PlayMode | **57/57 passed**, exit 0 | `Builds/Local/TestResults/playmode-balancefix.xml` |
| Asset stability | Weapon assets remained 12/20/9 through full test reruns and both player builds; the pre-fix asset flip churn no longer occurs | `Select-String` on `M3/M7-*.asset`, 2026-08-22 |
| Android build | Succeeded from `17a8c75`; APK **165,864,870 bytes**, SHA-256 `FB8AD7D705B51728FAAD2FA37F486647A94A24AED9DC8BE4D4B9FD7928E69542` | `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk` |
| Lava runtime | Exact fixed APK installed/launched only on `ST5GDW23LB004392`; activity top-resumed; sampled memory **422,823 KB PSS / 526,444 KB RSS / 82,088 KB Graphics / 452 KB swap**; sampled logcat showed only informational AndroidRuntime VM lines from an unrelated system process | ADB output, 2026-08-22 |
| Web build | Succeeded; **19 files / 134,007,148 bytes**; `Web-BazaarBastion.wasm` **121,275,261 bytes**, SHA-256 `C0FE8D7DBD320F07687B6647CB0D9EBF9129749C5FE7A2ECEFBF42349676C184` | `Builds/M11/Web-BazaarBastion` |
| Web serve/smoke | HTTP **200**; Chrome 1280×720, Chrome 390×844 and Edge 1280×720 each rendered the menu with **52 console messages, 0 errors** | temp Playwright harness output, 2026-08-22 |

Scope remains prototype/closed-alpha-foundation evidence: this baseline does not close
performance measurement, soak, visual/human review, Photon or PlayFab gates.

## Historical baselines (pre-2026-08-22)

### 2026-08-04 snapshot

Date: 2026-08-04
Branch: `codex/product-completion`
Latest validated repository HEAD: `af2e0d8` (`docs: record exact Goal B platform evidence`)
Latest validated runtime source: `a5fdde8` (`authority: canonicalize collision and ability placement`)
Latest runtime-bearing candidate: `a5fdde8`; exact-source platform candidate: `7ad7e42` (current HEAD is docs-only after the platform candidate)
Unity: `6000.5.6f1` (`C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe`)

## Goal B deterministic collision and placement slice (`a5fdde8`; exact platform candidate `7ad7e42`, 2026-08-04)

This focused slice adds a Unity/vendor-free arena collision contract and deterministic
axis-separated bounds/ordered-AABB solver. Production authority movement, Bijli/Pehel
displacement, Dhol displacement and Tiffin placement now resolve through canonical
authority positions; Maya decoy placement ignores caller-supplied remote positions.
The current Bazaar Bastion default contract has bounds and no authored obstacles yet,
so this is not a complete arena-collision or ability-completion claim. Projectile
travel/collision, stable combat event IDs, atomic match resolution, replay/soak and
Photon remain out of scope. The exact platform candidate is `7ad7e42`, whose only
change after `a5fdde8` is evidence documentation.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Source commit | Runtime `a5fdde8`; exact platform candidate `7ad7e42` on `codex/product-completion`; only focused authority/test/docs files were committed; pre-existing scene/Burst/Resources/Playwright/screenshot changes remain unstaged | `git rev-parse`, 2026-08-04 |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject ...` — **0 errors, 0 warnings** | command output from 2026-08-04 |
| Full EditMode | **109/109 passed**, 0 failed, 0 skipped | `Builds/GoalB/TestResults/collision-editmode-final.xml` |
| Full PlayMode | **55/55 passed**, 0 failed, 0 skipped | `Builds/GoalB/TestResults/collision-playmode-final.xml` |
| Collision/placement coverage | Bounds clamp, deterministic obstacle slide/order, canonical Maya placement and canonical Tiffin origin tests pass; production Pehel fixture now selects an enabled bot and waits for authority throw resolution | `Assets/BattleRaja/Tests/EditMode/ArenaCollisionTests.cs`, `Assets/BattleRaja/Tests/PlayMode/VerticalSlicePlayModeTests.cs` |
| Android build | Exact candidate `7ad7e42` built with `BuildAndroidBazaarBastionDevelopment`; APK **94,028,145 bytes**, SHA-256 `FA0CB54C04DC9309D8B21DAE02CE1D3D8A9961DA1C77F5ADE47F0B6AD280053A` | `C:\Projects\BattleRaja-GoalB-Validation\\Builds\\M11\\Android\\BattleRaja-BazaarBastion-M11.apk`, `Builds/M11/Logs/android-build.log` |
| Lava runtime | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); Unity activity was top-resumed; sampled memory **402,013 KB PSS / 537,532 KB RSS / 82,088 KB Graphics / 40 KB swap** | `C:\Projects\BattleRaja-GoalB-Validation\\Builds\\GoalB\\Android\\lava-exact-a5fdde8.png`, ADB output from 2026-08-04 |
| Web build | Exact candidate `7ad7e42` built successfully; **19 files / 133,747,764 bytes** including the development debug-information text; `Web-BazaarBastion.wasm` **121,033,616 bytes**, SHA-256 `D84155637B493182BF380FF91A9ED0D49ECE8F684FAE08E1FA85F0A68F318708` | `C:\Projects\BattleRaja-GoalB-Validation\\Builds\\M11\\Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |
| Web serve/smoke | Local HTTP `127.0.0.1:8142/index.html`, WASM and data requests returned **200**. Chrome and Edge canvas smoke passed at 1280×720, 1024×768 and 390×844; offline mode, fighter selection and active-match routes were reached. Browser console had **0 errors** and one known Unity persistent-data-path deprecation warning after match load | `C:\Projects\BattleRaja-GoalB-Validation\\.playwright-cli\\*.png`, `console-*.log`, 2026-08-04 |
| Web visual probe | Menu, mode, fighter-selection, Chrome active-match and Edge active-match captures were inspected; no blank canvas or fatal browser error observed. Portrait menu remains readable but is still prototype UI and not human approval | detached-worktree Playwright screenshots, 2026-08-04 |

The project remains prototype evidence. The committed Goal B slice does not include
generated scene rewrites or authored obstacle data, and it does not close presentation
projectile authority, atomic events, performance/soak, visual QA or human review. The
platform smoke is current-source evidence, not a performance or release pass.

## Goal A canonical adapter continuation (`0e531bb`, 2026-08-04)

This focused continuation routes authority-driven Bijli, Pehel, Maya and gadget
adapter steps through `OfflineMatchController.SimulationTickAdvanced`. Their local
MovementLab paths retain local clocks, while production attack/bot commands and
authority-driven abilities no longer consume independent gameplay clocks. Projectile
travel/collision remains presentation-only; deterministic arena collision, stable
combat event IDs, replay/soak and Photon remain out of scope.

The clean detached worktree at `C:\Projects\BattleRaja-GoalA2-Validation` was used
for the fresh platform artifacts so the main worktree's user-owned/generated files
remain untouched.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Source commit | `0e531bb` on `codex/product-completion`; only focused source/docs commits are ahead of origin; pre-existing dirty assets remain unstaged | `git rev-parse`, 2026-08-04 |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject ...` — **0 errors, 0 warnings** | command output from 2026-08-04 |
| Full EditMode | **104/104 passed**, 0 failed, 0 skipped | `Builds/GoalA/TestResults/editmode-canonical-abilities.xml` |
| Full PlayMode | **55/55 passed**, 0 failed, 0 skipped | `Builds/GoalA/TestResults/playmode-canonical-abilities-2.xml` |
| Android build | `BuildAndroidBazaarBastionDevelopment` succeeded; APK **93,986,577 bytes**, SHA-256 `0D6F54E5083886E5543C261DEB918708009A12479786293968827BB7D7178AF3` | `C:\Projects\BattleRaja-GoalA2-Validation\\Builds\\M11\\Android\\BattleRaja-BazaarBastion-M11.apk` and `android-build.log` |
| Lava runtime | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); Unity activity top-resumed; sampled memory **350,551 KB PSS / 487,160 KB RSS / 69,556 KB Graphics / 3 KB swap** | ADB output from 2026-08-04 |
| Web build | `BuildWebBazaarBastionDevelopment` succeeded; **19 files / 133,693,325 bytes**; `Web-BazaarBastion.wasm` **120,983,326 bytes**, SHA-256 `9BC3A5451695EE90DD53C5EB0F1BECB1E7065E8DAAD5C5B895314ACC49CC47FD` | `C:\Projects\BattleRaja-GoalA2-Validation\\Builds\\M11\\Web-BazaarBastion` and `web-build.log` |
| Web serve/smoke | Local HTTP `127.0.0.1:8141/index.html` returned **200**; Chrome and Edge Playwright canvas smoke tests each passed with no captured page/console errors | detached-worktree WebSmoke screenshots and test output, 2026-08-04 |
| Web visual probe | Main menu canvas inspected at Chrome 1280×720 and 390×844 plus Edge 1280×720; no blank canvas or fatal browser error observed | detached-worktree screenshots, 2026-08-04 |

The project remains prototype evidence. Known non-fatal Unity licensing/Fusion
editor-import and WebGL debugging warnings, plus Lava gralloc/audio noise, do not
constitute a performance, visual, multiplayer or release pass.

## Goal A exact-source rebaseline and command-authority slice (`7889672`, 2026-08-04)

This focused Goal A commit establishes a single match-controller tick event for
production command producers, keeps attack input malformed values available for
authority rejection, and moves weapon, faction, tick-rate, muzzle origin and
cooldown configuration behind `OfflineMatchAuthority`. Warmup, spawn-protection,
resolution, stale/duplicate sequence, bounded-future tick and invalid direction
inputs are rejected. The Unity projectile remains a presentation-only consumer of
an accepted authority attack; projectile travel/collision, deterministic arena
collision, replay and Photon remain out of scope.

The build and browser evidence below was produced in a clean detached worktree at
`7889672` (`C:\Projects\BattleRaja-GoalA-Validation`) so the main worktree's
user-owned scene/Burst/Resources/Playwright/screenshot changes were not staged or
overwritten. The detached worktree contains build-generated scene rewrites and is
not a source baseline.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Source commit | `7889672` on `codex/product-completion`; main worktree remains dirty only with pre-existing user/generated files | `git rev-parse`, 2026-08-04 |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject ...` — **0 errors, 0 warnings** | command output from 2026-08-04 |
| Full EditMode | **104/104 passed**, 0 failed, 0 skipped | `Builds/GoalA/TestResults/editmode.xml` (main-worktree test run) |
| Full PlayMode | **55/55 passed**, 0 failed, 0 skipped | `Builds/GoalA/TestResults/playmode.xml` (main-worktree test run) |
| Android build | `BuildAndroidBazaarBastionDevelopment` succeeded on Unity `6000.5.6f1`; APK **93,969,457 bytes**, SHA-256 `7558B505785A8E89C847E7039E900B2B2269E42E1BDD4EC2EA72D04609EF9FA9` | `C:\Projects\BattleRaja-GoalA-Validation\\Builds\\M11\\Android\\BattleRaja-BazaarBastion-M11.apk` and `android-build.log` |
| Lava runtime | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); Unity activity top-resumed; sampled memory **399,752 KB PSS / 536,648 KB RSS / 81,956 KB Graphics / 368 KB swap** | ADB output from 2026-08-04 |
| Web build | `BuildWebBazaarBastionDevelopment` succeeded; **19 files / 133,684,703 bytes**; `Web-BazaarBastion.wasm` **120,975,513 bytes**, SHA-256 `B86AF336D02063AE04729C5874D14DCC3B8FC6E15203473A7580AE6284494DDE` | `C:\Projects\BattleRaja-GoalA-Validation\\Builds\\M11\\Web-BazaarBastion` and `web-build.log` |
| Web serve/smoke | Local HTTP `127.0.0.1:8140/index.html` returned **200**; Playwright Chromium/Chrome and Edge canvas smoke tests each passed with no captured page/console errors | `Builds/GoalA/WebSmoke/*.png` and detached-worktree Playwright output, 2026-08-04 |
| Web visual probe | Main menu canvas inspected at Chrome 1280×720 and 390×844 plus Edge 1280×720; no blank canvas or fatal browser error observed | detached-worktree screenshots, 2026-08-04 |

Known non-fatal build/device noise remains: Unity reports an unavailable access
token while resolving the locally assigned Personal entitlement, Fusion's editor
import hook reports an already-existing generated config asset, WebGL ignores
`AllowDebugging`, and Lava emits known gralloc/audio warnings. None prevented the
successful build, launch or browser smoke. This is still prototype evidence, not
Goal A completion: remaining per-component clocks, presentation collision/projectile
authority, stable combat event IDs, replay/soak evidence and human review are open.

## Phase 1 authority attack-command continuation (`583106e`, 2026-08-03)

This focused continuation moves production attack command ordering, alive-state checks
and weapon cooldown consumption into `OfflineMatchAuthority`. The Unity attack controller
now submits the common command and spawns a presentation projectile only after authority
acceptance. This is an offline vertical-slice seam, not a trusted network combat server;
projectile collision projection and the remaining presentation-owned rules are still open.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Source commit | `583106e` pushed to `origin/codex/product-completion` | `git rev-parse`, 2026-08-03 |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject ...` — **0 errors, 0 warnings**; Core dependency and presentation-mutation scans clean | command output from 2026-08-03 |
| Full EditMode | **102/102 passed**, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-attack-editmode-final-20260803.xml` |
| Full PlayMode | **55/55 passed**, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-attack-playmode-final2-20260803.xml` |
| Android build | `BuildAndroidBazaarBastionDevelopment` succeeded; APK **151,541,453 bytes**, SHA-256 `10CD9FBC5B720519797702A43BA922F352A28AB6058DDDCBE561C6F7B37CC609` | `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk`, `Builds/M11/Logs/android-build.log` |
| Lava runtime | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); `UnityPlayerGameActivity` top-resumed; sampled memory **403,525 KB PSS / 540,544 KB RSS / 76,800 KB Graphics / 252 KB swap** | ADB output and `Docs/QA/Visual/Phase7/android-authority-attack-20260803.png` |
| Web build | A fresh `BuildWebBazaarBastionDevelopment` was started against this source, then stopped at the owner's request before `Build Finished, Result: Success`; no new exact-source Web artifact is claimed | `Builds/M11/Logs/web-build.log` (stopped during Bee player build) |
| Web baseline | The prior successful `6f0fe8b` Web output remains available for historical smoke reference, but is not evidence for the new attack-authority source commit | prior Web hash recorded above |

The new EditMode/PlayMode regressions cover duplicate/out-of-order attack rejection,
authority cooldown ownership, finite weapon inputs and production controller routing.
The full Phase 1 authority-separation acceptance remains **In progress** because
projectile collision, remaining presentation-owned mutation paths, soak evidence and
real multiplayer transport have not yet migrated.

## Current source rebaseline after Unity 6 object-lookup cleanup (`6f0fe8b`, 2026-08-03)

This is the fresh validation/test/build baseline for source commit `6f0fe8b`. The
production Pehel controller submits commands and consumes immutable authority results,
the production Bazaar route has gadget pickup/use coverage, bots respect load
warmup/spawn protection before combat, and authored Unity 6 object-lookup calls no
longer use deprecated APIs. No Photon gameplay claim is made. Build-generated
Bootstrap/Bazaar/Tutorial fixture rewrites and the pre-existing user-owned MovementLab
and Burst changes remain unstaged.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Git baseline | Source commit `6f0fe8b` on branch `codex/product-completion`; origin is pushed to the same commit | `git rev-parse`, `git push`, 2026-08-03 |
| Unity/toolchain | Unity `6000.5.6f1`; Git 2.53.0; Git LFS 3.7.1; embedded ADB 36.0.2 | installed tool output |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject ...` — 0 errors, 0 warnings | command output from 2026-08-03 |
| Full EditMode | 101 passed, 0 failed, 0 skipped; no C# compiler-warning lines | `Builds/M11/TestResults/findany-editmode-20260803.xml`, `Builds/M11/Logs/findany-editmode-20260803.log` |
| Full PlayMode | 54 passed, 0 failed, 0 skipped; no C# compiler-warning lines | `Builds/M11/TestResults/findany-playmode-20260803.xml`, `Builds/M11/Logs/findany-playmode-20260803.log` |
| Authored Unity API scan | No `FindFirstObjectByType` or `FindObjectsSortMode` calls in authored editor/presentation/test C# | `rg` output from 2026-08-03 |
| Android build | `BuildAndroidBazaarBastionDevelopment` succeeded; APK 151,551,952 bytes; SHA-256 `11624AFA7A9DB1CDEFC66FCACA5BBEC9CEDBD4C3316AA9AA7BE153BC33141AF4`; build log has 0 CS0618 and 0 C# errors | `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk`, `Builds/M11/Logs/android-build.log` |
| Lava runtime | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); `UnityPlayerGameActivity` top-resumed; process `31528`; no strict fatal/app-process marker was observed. The optional Play Asset Delivery class-probe warning remains known. | `Docs/QA/Visual/Phase7/android-api-warning-clean-20260803.png`, ADB output |
| Lava memory | `TOTAL PSS: 408290 KB`; `TOTAL RSS: 545224 KB`; `Graphics: 82088 KB`; swap 252 KB | `dumpsys meminfo` from 2026-08-03 |
| Web build | `BuildWebBazaarBastionDevelopment` succeeded; 19 files / 133,581,884 bytes; `Web-BazaarBastion.wasm` 120,872,476 bytes; SHA-256 `3663611AE374B5B481905341DA451BE335A0A79C704C888D856ACBA8E1D9C585`; build log has 0 CS0618 and 0 C# errors | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |
| Web serve/console | `http://127.0.0.1:8139/index.html` returned HTTP 200 (`8670` bytes); Playwright session `brweb5` reported 0 errors and 0 warnings across 52 messages after reload | Playwright session `brweb5`, 2026-08-03 |
| Web visual/interaction | Fresh rebuilt candidate inspected at 1024×768; the main menu rendered without blank canvas or clipping. Existing production captures cover mode/fighter/loading/opening/active pressure/Aandhi/spectator/results/rematch/error; gadget use remains unverified. | `Docs/QA/Visual/Phase7/web-api-warning-clean-1024x768-20260803.png` plus existing Phase 7 captures |

| Bot spawn protection | Production bots remain movement-capable but cannot use gadgets, attack, or activate abilities until the Opening phase; the new regression passes | `Assets/BattleRaja/Presentation/AI/BotBrain.cs`, `Assets/BattleRaja/Presentation/Match/OfflineMatchController.cs`, `Builds/M11/TestResults/bot-protection-full-playmode-20260803.xml` |

## Runtime performance continuation (`e90ad19` docs-only, 2026-08-03)

The runtime-bearing source remains `42e93e7`; this documentation-only continuation records
bounded observations from the connected Lava device and the local Chrome 150 Web route.
Lava reported 460,165 KB PSS / 597,680 KB RSS / 101,480 KB Graphics / 240 KB swap in a
20-second active-match sample. The process samples reported 87% instantaneous `top` CPU
and 50% user / 13% kernel in `dumpsys cpuinfo`; `gfxinfo` exposed no frame/jank histogram.
Chrome reported a 120,872,306-byte WASM transfer, 5.603 ms mean browser rAF and 0/0
console errors/warnings after warm-up. These are smoke observations, not FPS, GPU, GC,
thermal, repeated-match, cold-load, mobile-Web or release sign-off. Full interpretation:
`Docs/QA/Performance/runtime-smoke-20260803.md`.

The baseline passes compile/test/build/smoke gates, but remains prototype evidence:
greybox art, gadget-use capture, performance/soak, multi-browser coverage, real Photon
and PlayFab services, and human approval are still open.

## Fresh Bazaar prefab Android/Web artifact continuation (`8f190cd`, 2026-08-03)

The connected Bazaar architecture prefab was rebuilt for both primary targets after
the production-scene boundary work. This is current technical smoke evidence for the
prefab-bearing candidate; it is not final-art, gadget-use, performance-closure or
human visual approval.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Android build | `Tools\\Build\\Android\\build.ps1 ... -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidBazaarBastionDevelopment` — succeeded | `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk` |
| Android artifact | 151,511,951 bytes; SHA-256 `B485AEA6050CD16093C3A17C77DC8E75484BB7AFFB3875880E36F4110E742B61` | APK hash from 2026-08-03 |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); `UnityPlayerGameActivity` remained top-resumed; sampled logcat had 0 fatal markers | `Builds/M11/Logs/android-lava-bazaar-match-20260803.png` and ADB output |
| Lava memory snapshot | `TOTAL PSS: 411216 KB`; `TOTAL RSS: 547836 KB`; `Graphics: 82088 KB` | ADB `dumpsys meminfo` from 2026-08-03 |
| Android visual probe | Menu, mode, fighter-selection and live Bazaar match screens inspected; match HUD showed Bijli HP, attack/ability readiness and touch labels | `Builds/M11/Logs/android-lava-bazaar-match-20260803.png` |
| Web build | `Tools\\Build\\Web\\build.ps1 ... -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildWebBazaarBastionDevelopment` — succeeded | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |
| Web artifact | 19 files; 133,496,123 bytes; `Web-BazaarBastion.wasm` 120,790,368 bytes; SHA-256 `C1375376EF0906364B33798E046F719C041ECB1BF0277DBFCA32B91ABA165C68` | recursive file/hash inventory from 2026-08-03 |
| Web serve | Existing local server on `http://127.0.0.1:8139/index.html` returned HTTP 200 (`8,670` bytes) | `Invoke-WebRequest` output from 2026-08-03 |
| Regression after prefab | 100/100 EditMode and 51/51 PlayMode; repository validator 0 errors/0 warnings | `Builds/M11/TestResults/bazaar-prefab-editmode-full-20260803.xml`, `bazaar-prefab-playmode-full-20260803.xml` |

Gadget pickup/use was probed on Lava but remained visually unverified (`GADGET [G]`
empty in the inspected match capture). Greybox presentation, authored art/audio,
browser interaction parity, FPS/GC/GPU/soak measurements and human approval remain open.

## Bazaar architecture prefab continuation (`8f190cd`, 2026-08-03)

The existing `BazaarArchitecture` hierarchy is now a connected prefab at
`Assets/BattleRaja/Content/Prefabs/BazaarArchitecture.prefab`. The editor entrypoint
creates/connects it through Unity's `PrefabUtility`, remains idempotent on reruns, and
validation requires the asset. This is reusable greybox content evidence, not final art or
actor prefab extraction.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| Full EditMode regression | 100 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/bazaar-prefab-editmode-full-20260803.xml` |
| Full PlayMode regression | 51 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/bazaar-prefab-playmode-full-20260803.xml` |
| Prefab boundary | Prefab exists, and Bazaar scene contains a prefab instance with the matching architecture asset GUID | `Assets/BattleRaja/Content/Prefabs/BazaarArchitecture.prefab`, `Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity` |
| Artifact/runtime scope | No Android/Web rebuild for this content-only continuation; prior `d993a5b` Lava/Web smoke remains the latest artifact evidence | authority-Maya section below |

## Bazaar production-scene contract continuation (`26b11cc`, 2026-08-03)

`BazaarBastion.unity` now carries a dedicated `BazaarBastionScene` contract and no longer
serializes the `MovementLabScene` marker. The editor entrypoint opens the existing Bazaar
scene instead of copying the user-owned MovementLab fixture, preserves TutorialArena in
build settings, and creates Bazaar architecture idempotently. This is a scene-boundary
hardening step, not prefab extraction or final-art evidence.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| Focused production PlayMode | 15 passed, 0 failed, 0 skipped (`VerticalSlicePlayModeTests`) | `Builds/M11/TestResults/bazaar-boundary-playmode-final-20260803.xml` |
| Full EditMode regression | 100 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/bazaar-boundary-editmode-full-20260803.xml` |
| Full PlayMode regression | 51 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/bazaar-boundary-playmode-full-20260803.xml` |
| Scene boundary | One `BazaarBastionScene`, zero `MovementLabScene` markers, one `BazaarArchitecture`, and serialized player/camera/match/projectile/damage references | `Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity`, `VerticalSlicePlayModeTests` |
| Artifact/runtime scope | No Android/Web rebuild for this scene-only continuation; prior `d993a5b` Lava/Web smoke remains the latest artifact evidence | `Docs/QA/LATEST_HEAD_BASELINE.md` authority-Maya section |

## Authority-driven Maya decoy continuation (`d993a5b`, 2026-08-03)

Production Maya decoys are now owned by `OfflineMatchAuthority`: spawn, follow position,
remaining lifetime, health and duplicate-tick damage are resolved in the pure/application
path. `MayaFighterController` consumes immutable snapshots and creates only the generated
capsule view. The local non-authority probe path remains available; this is not a real
network-server claim.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 100 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-maya-decoy-editmode-full-20260803.xml` |
| PlayMode tests | 51 passed, 0 failed, 0 skipped; includes production Maya authority lifetime/damage | `Builds/M11/TestResults/authority-maya-decoy-playmode-full-20260803.xml`, `Builds/M11/TestResults/authority-maya-decoy-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,512,643 bytes; SHA-256 `F12EEBB2A6E8968992B38F9446E1D9B55A5C2BD0EBB0F6AB0306A23786E9BEB9` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); `UnityPlayerGameActivity` remained top-resumed; sampled logcat had 0 fatal/AndroidRuntime/SIGSEGV markers | ADB output from 2026-08-03; `Builds/M11/Logs/android-lava-authority-maya-20260803.png` |
| Web build/serve | 21 files, 133,498,294 bytes; `Web.wasm` 120,789,716 bytes, SHA-256 `FE33609725B6C8AE097A56F1C29928B41EB8DBCBAFB42217129018E5DB2B97DD`; local port 8137 returned HTTP 200 | `Builds/M11/Web`, `Builds/M11/Logs/web-build.log` |

## Authority-driven fighter displacement continuation (`9100e69`, 2026-08-03)

Bijli dash, Pehel charge and Pehel throw now submit resolved displacement through a
tick-validated `OfflineMatchAuthority` seam whenever the actor uses production
authority-driven movement. Non-authority lab fixtures retain their local controller
fallback. This closes the disabled-`CharacterController` production behavior gap, but
does not claim server-owned ability runtime state or network authority.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 99 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-ability-displacement-editmode-full-rerun-20260803.xml` |
| PlayMode tests | 50 passed, 0 failed, 0 skipped; includes the live Bijli authority displacement test | `Builds/M11/TestResults/authority-ability-displacement-playmode-full-rerun-20260803.xml`, `Builds/M11/TestResults/authority-bijli-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,465,096 bytes; SHA-256 `00D83717FAC958A588B4F4964504C92ACA70CDE23DFE800C99234CC48A6ED3E8` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity` remained top-resumed; sampled logcat had no fatal/AndroidRuntime marker | ADB output from 2026-08-03; `Builds/M11/Logs/android-lava-authority-bijli-20260803.png` |
| Web build/serve | 21 files, 133,477,944 bytes; `Web.wasm` SHA-256 `F6C37FFC4D3CAE8DE87BDFAF5202FE32F7FF504AE66BB4C55D7834C50852D6EB`; local port 8137 returned HTTP 200 | `Builds/M11/Web`, `Builds/M11/Logs/web-build.log` |

## Authority-driven Dhol displacement continuation (`78fa990`, 2026-08-03)

Dhol Burst now applies each validated displacement to the canonical participant position
before returning the immutable effect. The presentation adapter consumes the resulting
snapshot, so production authority movement no longer depends on a local
`CharacterController.Move` call. Pehel charge displacement, Bijli dash displacement and
real network authority remain open.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 98 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-gadget-displacement-editmode-20260803.xml` |
| PlayMode tests | 49 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-gadget-displacement-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,472,675 bytes; SHA-256 `B8D6DCEB124B0CA9DA42A2E341720B7AC7F325FA734FDD2EA3650F9F3B53AF80` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); top-resumed Unity activity observed with process id 8128 and zero sampled fatal/AndroidRuntime markers | ADB output from 2026-08-03; `Builds/M11/Logs/android-lava-authority-gadget-20260803.png` |
| Web build/serve | 21 files, 133,469,527 bytes; `Web.wasm` SHA-256 `C51E0EBB3B086C16EAA0C079532FC37C0015CD827F437B89C009DF892F4846F6`; local port 8137 returned HTTP 200 | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

## Authority-driven production movement continuation (`204e4f0`, 2026-08-03)

Bazaar Bastion now routes actor movement commands through `OfflineMatchAuthority` at the
fixed simulation tick. The pure motor runs once per actor/tick, canonical positions are
stored in `OfflineMatchSimulation`, duplicate ticks are rejected, and the presentation
adapter applies the returned snapshot. MovementLab intentionally retains its local
observation path for movement fixtures. Fighter ability displacement and real network
authority remain open.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 98 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-movement-editmode-20260803-final.xml` |
| PlayMode tests | 49 passed, 0 failed, 0 skipped, including `ProductionMatchRoutesMovementThroughAuthoritySnapshots` | `Builds/M11/TestResults/authority-movement-playmode-20260803-final2.xml` |
| Android build | `BattleRaja-M11.apk`, 151,468,901 bytes; SHA-256 `378B0474577A575139FEE1F797EF848C2FC27335CCA69427F64297860A768A04` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); top-resumed Unity activity observed with process id 7419 and zero sampled fatal/AndroidRuntime markers | ADB output from 2026-08-03; `Builds/M11/Logs/android-lava-authority-movement-20260803.png` |
| Web build/serve | 21 files, 133,467,594 bytes; `Web.wasm` SHA-256 `DFA4599679634349459DBD68973467B929F9470D7FAC057A56B90D5C137C46FC`; local port 8137 returned HTTP 200 | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

## Authority-first healing continuation (`678acb0`, 2026-08-03)

Health pickups and Tiffin station healing now apply to canonical simulation health
through `OfflineMatchAuthority.ApplyHealing`; Unity applies only the resulting health
snapshot. The controller no longer mirrors view health back into authority every render
frame. The legacy `SyncHealth` method remains for test/server compatibility while movement
reconciliation is still incremental.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -ProjectRoot .` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 97 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-health-editmode-20260803.xml` |
| PlayMode tests | 48 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-health-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,415,947 bytes; SHA-256 `4B1BAF633BA448555B5D3E6C4C21666C61462499D0C3451CFE596AEA1234CFD3` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); Unity activity remained resumed and no sampled fatal crash marker was found | ADB output from 2026-08-03 |
| Android visual inspection | Portrait live match remains readable after canonical healing migration | `Docs/QA/Visual/Phase7/android-lava-authority-healing-20260803.png` |
| Web build/serve | 21 files, 133,374,016 bytes; `Web.wasm` SHA-256 `1DBDC8DA65514A660DC4651EF7116FB83C522C42F62E0678716B059F7B76438E`; local port 8137 returned HTTP 200 | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

Movement authority and remaining non-health presentation reconciliation are still open.

## Authority-first actor damage continuation (`08c6f2e`, 2026-08-03)

Production actor damage now resolves against `OfflineMatchAuthority` and canonical
simulation health/statistics before Unity applies the returned snapshot/event to the
view. Non-authority lab targets continue through the local presentation pipeline.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -ProjectRoot .` — 0 errors, 0 warnings; `git diff --check` clean | command output from 2026-08-03 |
| EditMode tests | 96 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-damage-editmode-20260803.xml` |
| PlayMode tests | 48 passed, 0 failed, 0 skipped, including authority-first actor damage and existing results/rematch/Maya coverage | `Builds/M11/TestResults/authority-damage-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,414,630 bytes; SHA-256 `5BF1657D1BDA0D059E72C48A6D85AA03441C6096DFFD679E4A8146FC6996B6C4` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | Exact APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); Unity activity remained resumed and no sampled fatal crash marker was found | ADB output from 2026-08-03 |
| Android visual inspection | Portrait live match remains readable with labeled touch controls after the authority change | `Docs/QA/Visual/Phase7/android-lava-authority-damage-20260803.png` |
| Web build/serve | 21 files, 133,372,172 bytes; `Web.wasm` SHA-256 `9D2101263269276649D596A9F8A229F97B6447483B9D0B20CDFB8E45B3C7C8E7`; local port 8137 returned HTTP 200 | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

This closes the duplicate/late actor-damage accounting gap for the offline authority.
Movement authority, health/pickup reconciliation, remaining gadget presentation adapters,
real Photon transport and final network/server authority remain open.

## Touch-control readability continuation (`a170746`, 2026-08-03)

The three runtime combat touch surfaces now create non-raycast labels for `ATTACK`,
`ABILITY` and `GADGET`. This keeps the scene controls data-light while making the
actions discoverable on Android and Web. The labels are covered by a PlayMode
regression and were inspected on the permitted Lava device.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -ProjectRoot .` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 95 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/touch-labels-editmode-retry-20260803.xml` |
| PlayMode tests | 47 passed, 0 failed, 0 skipped, including `TouchControlsExposeReadableActionLabels` | `Builds/M11/TestResults/touch-labels-playmode-20260803.xml` |
| Android build | `BattleRaja-M11.apk`, 151,407,488 bytes; SHA-256 `959D766A0D218F8D531B7BA85D0D8199A77A39E90DD0C23B0AAD8A3C85A5E18F` | `Builds/M11/Logs/android-build.log` |
| Lava smoke | APK installed/launched only on `ST5GDW23LB004392` (`LAVA LXX508`); active Unity activity and no sampled fatal crash marker | ADB output from 2026-08-03 |
| Android visual inspection | Portrait match visibly shows all three labels on the touch surfaces | `Docs/QA/Visual/Phase7/android-lava-touch-labels-20260803.png` |
| Web build | 21 files, 133,364,566 bytes; `Web.wasm` SHA-256 `D75EE1355A111711A716C72AEFA6714252FC2C08943CC8AE4FBE707464885C69` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |

This closes the technical readability gap only. Touch ergonomics, loading-state behavior,
gadget pickup/use, multi-browser coverage and final human presentation approval remain open.

## Authority and results/rematch continuation (`044b1b8`, 2026-08-03)

This is the current source/runtime checkpoint after the authority event-routing fix.
`OfflineMatchController` now reports resolved combat events through
`OfflineMatchAuthority.RecordDamage`; the production Web candidate was also driven
through a real Results screen and a subsequent Rematch transition in Chrome 150.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 95 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-routing-editmode-20260803.xml` |
| PlayMode tests | 46 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-routing-playmode-20260803.xml` |
| Android production smoke | APK 151,412,235 bytes; SHA-256 `9433136F638F622597DEB816E023999811208EB8827AE9A9ADE565AD17C39D87`; installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `27998`; crash-marker scan count 0 | `Builds/M11/Logs/android-build.log` and ADB output from 2026-08-03 |
| Web production smoke | 19 files, 133,358,779 bytes; `Web-BazaarBastion.wasm` SHA-256 `66D159C1291809BA04A3365A47976AA0042145E53F2335CA9BC545213A2BF6DA`; port 8139 HTTP 200 | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |
| Results capture | Chrome showed all eight placements, statistics, `REMATCH` and `MENU` controls at 1280x720; the exact APK also reached the portrait Results panel on Lava | `Docs/QA/Visual/Phase7/playwright-1280x720-results-20260803.png`, `Docs/QA/Visual/Phase7/android-lava-results-authority-20260803.png` |
| Rematch transition | Clicking `REMATCH` returned to a fresh live match; screenshot inspected after the transition; Chrome session recorded 61 messages, 0 errors and 1 known `JS_FileSystem_Sync` deprecation warning | `Docs/QA/Visual/Phase7/playwright-1280x720-rematch-match-20260803.png`, `.playwright-cli/console-2026-08-03T06-02-11-380Z.log` |
| Performance smoke | Chrome warm local run: DOMContentLoaded 152 ms, load 251.4 ms, WASM transfer 120,662,567 bytes, rAF mean/p95 5.620/6.1 ms, JS heap used 30,296,011 bytes; Lava PSS/RSS/Graphics PSS 458,974/596,588/95,468 KB; Android gfxinfo had no frame histogram | `Docs/QA/Performance/authority-runtime-20260803.md` and sibling raw captures |

The Results/rematch observation is technical interaction evidence, not human visual
approval. Gadget pickup/use remains an evidence gap; Android touch ergonomics remain
open for owner review. The known Unity API-obsolescence and Web persistent-data-path
warnings are non-fatal and are not counted as product crashes.

The repository then received docs/CI-only commit `2809165`; it does not change the
runtime-bearing code or the Android/Web artifact hashes recorded above.

## Production-flow continuation (`d1b33d1` + `4d3ae6a`)

The production flow now creates `InputSystemUIInputModule` when it has to create an
EventSystem, and the combat HUD binds its identity and ability text to the selected
fighter (Bijli, Pehel or Maya). The static formatter is covered independently so the
selection labels cannot silently regress to Bijli.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 94 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fighter-hud-final-editmode-20260803.xml` |
| PlayMode tests | 46 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fighter-hud-final-playmode-20260803.xml` |
| Production Android build | Unity `Build Finished, Result: Success`; APK 151,378,890 bytes; SHA-256 `F47DC800BF351A1AD5A29A48C40ECE633E76D14E967DDF6F391D5F6935C2F4D6` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk` |
| Lava smoke | Exact production APK installed/launched on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `26699`; no process death or fatal crash marker in the sampled log. Unity emitted the known optional Play Asset Delivery `AssetPackManager` class-probe warning. | ADB output from 2026-08-03 |
| Production Web build | Unity WebGL build completed; 19 files, 133,357,921 bytes; `Web-BazaarBastion.wasm` SHA-256 `BC3689B1D48DFD099DA8C619E9E9059EA709BB3D394C74B3411B17FCCFD03BAC` | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |
| Local production Web serve | `http://127.0.0.1:8139/index.html` returned HTTP 200; Chrome 150 loaded one Unity canvas and reported 52 console messages with 0 errors and 0 warnings after reload | Playwright/HTTP output from 2026-08-03 |
| Browser focus/timing observation | Canvas focus contract remains `tabindex=0`; prior local Chrome observation: DOMContentLoaded 401.7 ms, load 520.4 ms, WASM transfer 120,659,088 bytes, browser `requestAnimationFrame` p95 6.0 ms, JS heap used 30.2 MB | `Docs/QA/Visual/Phase7/playwright-input-system-runtime-20260803.png` and measurement output from 2026-08-03 |
| Lava memory observation | PSS 502,948 KB, RSS 639,560 KB, Graphics PSS 101,708 KB; `dumpsys gfxinfo` exposed no frame/jank histogram, so no FPS claim is made | `Builds/M11/Logs/input-system-lava-meminfo-20260803.txt`, `input-system-lava-gfxinfo-20260803.txt`, `input-system-lava-top-20260803.txt` |

The production Chrome captures show the Bazaar Bastion route and fighter-specific HUD
text for Pehel and Maya. They are still greybox/prototype evidence; the 390x844
MovementLab capture shows bot debug-label/HUD overlap, so visual approval, touch
ergonomics and final authored presentation remain human-review items.

## Maya bot-perception continuation (`ff2a3e4`)

Maya decoy spawn/destruction now refreshes bot perception targets. Decoys are deactivated
before deferred destruction so a bot cannot retain a stale target after the decoy has left
gameplay. `VerticalSlicePlayModeTests.MayaDecoySpawnsFollowsAndCanBeDestroyedByCombat`
now asserts that a sensor created before the decoy observes it after spawn.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 94 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/maya-bot-decoy-editmode-20260803.xml` |
| PlayMode tests | 46 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/maya-bot-decoy-playmode-20260803.xml` |
| Android production smoke | APK 151,385,439 bytes; SHA-256 `535620433C81BF9B146E783FCE5F05F779B1E1BCB7B9CBA1FF4724A123D5B876`; installed/launched on Lava `ST5GDW23LB004392` as process `27459`; crash-marker scan count 0 | `Builds/M11/Logs/android-build.log` and ADB output from 2026-08-03 |
| Web production smoke | 19 files, 133,358,415 bytes; `Web-BazaarBastion.wasm` SHA-256 `65BD44CD401E9A1AD16C684AD168F8E71B4932465DC719A4A2C2D38AE1DD580`; port 8139 HTTP 200; Chrome one canvas, 52 console messages, 0 errors and 0 warnings | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |

The Android build still reports the repository's existing Unity API-obsolescence warnings;
the Web wrapper still reports the local websockify port-35020 collision, but both Unity
builds report success. No visual-approval or performance-readiness claim is added by this
behavioral fix.

## Authority event-routing continuation (`8645254`)

`OfflineMatchAuthority.RecordDamage` is now the application-owned entry point for resolved
combat events. `OfflineMatchController` reports immutable `CombatDamageEvent` values through
that method instead of calling `OfflineMatchSimulation.RecordDamage` directly. This keeps
placements, eliminations, damage and assists behind the authority boundary while leaving
Unity responsible for applying view-side health changes.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 95 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-routing-editmode-20260803.xml` |
| PlayMode tests | 46 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/authority-routing-playmode-20260803.xml` |
| Android production smoke | APK 151,412,235 bytes; SHA-256 `9433136F638F622597DEB816E023999811208EB8827AE9A9ADE565AD17C39D87`; exact APK installed/launched on Lava `ST5GDW23LB004392` as process `27998`; crash-marker scan count 0 | `Builds/M11/Logs/android-build.log` and ADB output from 2026-08-03 |
| Web production smoke | 19 files, 133,358,779 bytes; `Web-BazaarBastion.wasm` SHA-256 `66D159C1291809BA04A3365A47976AA0042145E53F2335CA9BC545213A2BF6DA`; port 8139 HTTP 200; Chrome one canvas, 52 console messages, 0 errors and 0 warnings | `Builds/M11/Web-BazaarBastion`, `Builds/M11/Logs/web-build.log` |

This phase reduces the presentation-authority coupling but does not establish real network
authority, server transport, or release readiness.

## Scope and repository note

The requested goal path `Docs/AI/RepositoryAuditAndCompletionGoal.md` is absent. The matching file `Docs/AI/BattleRaja_Repository_Audit_and_Completion_Goal.md` was read in full and used as the authoritative continuation brief. This path discrepancy remains a documentation issue; no source from the requested path was available.

The baseline intentionally excludes unrelated working-tree changes in `Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity` and `Data/Plugins/lib_burst_generated.wasm`. Those files were not staged or altered by this baseline work.

## Input System-only continuation (`5f4566d`)

The project now uses Unity's Input System as the active handler. Generated scenes use
`InputSystemUIInputModule`, audio gesture detection uses Input System device state, and a
scene-load bridge keeps older serialized scenes runnable without rewriting their YAML.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| PlayMode tests | 45 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/input-system-playmode-fixed-20260803.xml` |
| EditMode baseline | 94 passed, 0 failed, 0 skipped on the immediately preceding rebaseline; a fresh invocation was blocked by pre-existing Unity editor processes holding the project lock | `Builds/M11/TestResults/rebaseline-editmode-20260803.xml` |
| Android development build | Unity log reports `Build Finished, Result: Success`; APK 151,373,009 bytes; SHA-256 `60AEF8C395B21E9C0CA5EF142411AB57214A9B2D50053BE5FF623544CF2D9812` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Lava smoke | Exact APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `23328`; sampled log has no fatal exception, SIGSEGV or missing-component marker | ADB command output from 2026-08-03 |
| Web development build | Unity log reports `Build Finished, Result: Success`; 21 files, 133,358,665 bytes; WASM 120,658,772 bytes; SHA-256 `790349B1D3203B173EDF8075D827426DD20AD74737C99FB7567C52E2FD54B5E2` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web/Build/Web.wasm` |
| Local Web serve | `http://127.0.0.1:8137/index.html` returned HTTP 200 | local HTTP check from 2026-08-03 |

The Web build log also contains the expected local websockify `EADDRINUSE` warning on
port 35020 because another local wrapper already owned that port; Unity completed the
player build successfully. Existing compiler deprecation warnings remain technical debt.

## Fresh latest-HEAD rebaseline (`9291d85`)

This is the current rebaseline requested after the Photon Fusion import. It was run
against the current project at Unity `6000.5.6f1`; the two pre-existing user-owned
tracked edits listed above remained untouched and no generated scene changes were
included in source control.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Unity/toolchain | Unity `6000.5.6f1`; AndroidPlayer, WebGLSupport and WindowsStandaloneSupport present; embedded ADB `36.0.0`; Lava `ST5GDW23LB004392` connected | command output from 2026-08-03 |
| Photon/package check | Photon Fusion `2.1.1 Stable 2177` files present under `Assets/Photon/Fusion`; Input System `1.20.0`; URP `17.5.0`; Test Framework `1.7.0` | `Assets/Photon/Fusion/build_info.txt`, `Packages/manifest.json` |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 89 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fresh-editmode-20260803.xml` |
| PlayMode tests | 43 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fresh-playmode-20260803.xml` |
| Android development build | `Tools\\Build\\Android\\build.ps1` completed; APK 151,312,094 bytes; SHA-256 `2C6E9861E8D1D1011B3EF3A1B891B8BC92FA075410B8DA97015AA4BEB4BCA353` | `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | `Tools\\Build\\Web\\build.ps1` completed; 21 files, 133,194,576 bytes; WASM SHA-256 `E029D2925F6E0B7463DA8F236584EE560F011EE0A59D99884A484E05EDA8EDAB` | `Builds/M11/Web`, `Builds/M11/Web/Build/Web.wasm` |
| Local Web serve | `http://127.0.0.1:8137/index.html` returned HTTP 200 | local HTTP check from 2026-08-03 |
| Chrome bootstrap/runtime | Playwright loaded the build, found one Unity canvas after 8 seconds, and reported 53 console messages with 0 errors and 0 warnings; inspected screenshot shows the live greybox match | `Docs/QA/Visual/Phase7/playwright-fresh-baseline-20260803.png`, `.playwright-cli/console-2026-08-03T03-18-15-589Z.log` |
| Lava smoke | Fresh APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `17939`; portrait screenshot shows the live match/HUD; process-scoped log has no fatal, SIGSEGV, missing-component or monotonic-tick markers | `Docs/QA/Visual/Phase7/android-lava-fresh-baseline-20260803.png`, `Builds/M11/Logs/fresh-baseline-lava-20260803.txt` |

Known baseline warnings remain separate from pass/fail results: Unity batchmode logs
report the empty `BattleRaja.Gameplay` asmdef, Photon editor custom-dependency scheduling,
no AudioListener in the generated `MovementLab` fixture, and an unavailable batchmode
licensing handshake/access token. None caused a test or build failure. The ADB inventory
also showed an Oppo device, but no Oppo command was used; physical smoke testing used
only the instructed Lava serial.

## Fresh assist/statistics phase (`5845485`)

The pure match-statistics change adds one deterministic assist credit to each living,
non-finishing participant who contributed damage to an eliminated target. Duplicate lethal
events, environmental damage and self-damage do not create additional assists.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 90 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/assist-editmode-20260803.xml` |
| PlayMode tests | 43 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/assist-playmode-20260803.xml` |
| Android development build | Unity log reports `Build Finished, Result: Success`; APK 151,365,343 bytes; SHA-256 `7217CFDE7ACC1F9F61FF8ACA90C95B07DA8CB952E760760EC3F1AC1748CFD4D3` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | Unity log reports success; 21 files, 133,247,876 bytes; WASM SHA-256 `F48A35AE2F9DB985DC28689B31A1BF8C42F5A1CED70531A7F374608F5B7FA3D3` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Browser runtime | Chrome/Playwright found one canvas after an 8-second warm-up; 53 console messages, 0 errors and 0 warnings; inspected screenshot shows the live greybox match | `Docs/QA/Visual/Phase7/playwright-assist-phase-20260803.png`, `.playwright-cli/console-2026-08-03T03-36-22-982Z.log` |
| Lava smoke | Exact assist APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `20176`; process-scoped log contains no fatal, SIGSEGV, missing-component or monotonic-tick marker | `Docs/QA/Visual/Phase7/android-lava-assist-phase-20260803.png`, `Builds/M11/Logs/assist-lava-20260803.txt` |

The Android wrapper printed a stale PowerShell `$LASTEXITCODE` after Unity had already
logged a successful build; the Unity build log and artifact were checked directly. This
wrapper quirk is recorded rather than treated as a product failure.

## Fresh aim-assist accessibility phase (`c23982e` + `d69b74b` test coverage)

The accessibility setting now changes only the local aim direction within a bounded
18-degree, 10-metre cone. Target selection is pure and deterministic; Unity gathers
eligible enemy colliders with a fixed non-allocating buffer, and projectile authority is
unchanged.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 94 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/aimassist-editmode-v5-20260803.xml` |
| PlayMode tests | 44 passed, 0 failed, 0 skipped, including the in-match settings toggle/persistence test | `Builds/M11/TestResults/aimassist-playmode-v2-20260803.xml` |
| Android development build | Unity log reports `Build Finished, Result: Success`; APK 151,378,712 bytes; SHA-256 `94A5C3D933BB00C99BBF293FA832DD6E67E3DB7D37C3BD63581E3BA7878772E8` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | Unity log reports success; 21 files, 133,250,977 bytes; WASM SHA-256 `3B505C77E2F567878CDFAEC34BDE83A23B53AE3DA4943C3FBE474555D536505F` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Browser runtime | Chrome/Playwright found one canvas after an 8-second warm-up; 53 console messages, 0 errors and 0 warnings; inspected screenshot shows the live greybox match | `Docs/QA/Visual/Phase7/playwright-aimassist-phase-20260803.png`, `.playwright-cli/console-2026-08-03T04-33-13-788Z.log` |
| Lava smoke | Exact aim-assist APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `21626`; process-scoped log contains no fatal, SIGSEGV, missing-component or monotonic-tick marker | `Docs/QA/Visual/Phase7/android-lava-aimassist-phase-20260803.png`, `Builds/M11/Logs/aimassist-lava-20260803.txt` |

The functional setting still requires human review for touch ergonomics, fairness and
accessibility approval; this evidence does not close the broader visual gate.

## Results-screen continuation (`810f484`)

The offline Results surface now lists every placement in deterministic placement/ID order,
including eliminations, assists, damage and survival time. Compact portrait mode uses a
shorter row format and smaller results typography; the formatter is pure and does not
mutate the match snapshots.

| Check | Command/result | Evidence |
| --- | --- | --- |
| EditMode tests | 94 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/results-editmode-20260803.xml` |
| PlayMode tests | 45 passed, 0 failed, 0 skipped; includes the placement/statistics formatter regression | `Builds/M11/TestResults/results-playmode-20260803.xml` |
| Android development build | Unity log reports `Build Finished, Result: Success`; APK 151,408,183 bytes; SHA-256 `AF7A862E47BE8F50C885AC3784C01964503AF2B96802CA21319E3E1AEC2D8AE4` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | Unity log reports success; 21 files, 133,352,571 bytes; WASM SHA-256 `70210B58F843B51A29499AA5D8DB3A4D64AAAB0E994FA8425064AEF78762A0C7` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Browser runtime | `http://127.0.0.1:8137/index.html` returned HTTP 200; Chrome/Playwright reached the live match with 53 console messages, 0 errors and 0 warnings | `Docs/QA/Visual/Phase7/playwright-results-20260803.png`, `.playwright-cli/console-2026-08-03T04-59-04-584Z.log` |
| Lava smoke | Exact APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `22304`; capture shows a live portrait match and log scan found no fatal, SIGSEGV, AndroidRuntime, missing-component or null-reference marker | `Docs/QA/Visual/Phase7/android-lava-results-20260803.png`, `Builds/M11/Logs/results-lava-20260803.txt` |

The runtime captures are live-match technical evidence for the build. They do not claim
that the Results/rematch state has passed human visual review; that state still needs a
deliberate capture and owner approval.

## Web input-focus continuation (`3e00b02`)

The Web template now gives the Unity canvas keyboard focus semantics (`tabindex="0"`, an
accessible game label and pointer-down focus restoration). The repository validator checks
these invariants so keyboard/pointer browser QA does not silently run against an unfocusable
canvas.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\Validation\validate.ps1 -RequireUnityProject -UnityExe ...\6000.5.6f1\Editor\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| Fresh regression baseline | EditMode 94/94 and PlayMode 45/45 passed sequentially | `Builds/M11/TestResults/rebaseline-editmode-20260803.xml`, `Builds/M11/TestResults/rebaseline-playmode-20260803.xml` |
| Web build | Unity Web build completed; 21 files, 133,353,130 bytes; WASM SHA-256 `70210B58F843B51A29499AA5D8DB3A4D64AAAB0E994FA8425064AEF78762A0C7` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web/index.html` |
| Chrome focus/runtime | `http://127.0.0.1:8137/index.html` returned HTTP 200; after clicking the canvas, `document.activeElement.id` was `unity-canvas` and `tabIndex` was `0`; 53 console messages, 0 errors and 0 warnings | `Docs/QA/Visual/Phase7/playwright-web-focus-20260803.png`, `.playwright-cli/console-2026-08-03T05-12-58-426Z.log` |
| Edge focus/runtime | After clicking the canvas, `document.activeElement.id` was `unity-canvas` and `tabIndex` was `0`; 53 console messages, 0 errors and 0 warnings | `.playwright-cli/console-2026-08-03T05-13-26-439Z.log` |

This closes the DOM/canvas focus defect only. It does not turn the unverified gadget-use or
Results/rematch screenshot rows into visual approval, and it does not replace manual touch
ergonomics review.

## Fresh Phase 6 continuation (`8544f55`)

This section supersedes the older Phase 2 checkpoint below for the current source
HEAD. It covers the authority-driven spatial gadget collection path, immediate live
results publication after lethal damage, and the rematch scene reload surface.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 89 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/phase6-full-editmode-20260803.xml` |
| PlayMode tests | 43 passed, 0 failed, 0 skipped; includes spatial gadget collection, live results/rematch reload, three repeated rematch cleanup cycles and the full eight-step tutorial walkthrough | `Builds/M11/TestResults/tutorial-full-playmode-20260803.xml` |
| Android development build | Unity exit 0; IL2CPP APK 151,312,094 bytes; SHA-256 `285A7973A0487281F0F79BCFD14827114232D5C6B0F2BA3AD67BC57134B6B6BF` | `Builds/M11/Logs/android-build.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development build | Unity exit 0; 21 files, 133,194,614 bytes; WASM 120,501,844 bytes; SHA-256 `E029D2925F6E0B7463DA8F236584EE560F011EE0A59D99884A484E05EDA8EDAB` | `Builds/M11/Logs/web-build.log`, `Builds/M11/Web` |
| Local Web serve | `http://127.0.0.1:8137/index.html` returned HTTP 200 | local HTTP check from 2026-08-03 |
| Browser runtime | Fresh Chrome/Playwright load reached the live match after an 8-second warmup; screenshot shows the arena and HUD; console scan returned 0 errors/0 warnings | `Docs/QA/Visual/Phase7/playwright-phase6-runtime-20260803.png`; Playwright CLI output from 2026-08-03 |
| Lava smoke | Exact Phase 6 APK installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), process `14841`; portrait screenshot shows the live match and HUD; process-scoped log contains no fatal crash, monotonic-tick or missing-component marker | `Docs/QA/Visual/Phase7/android-lava-phase6-runtime-20260803.png`, `Builds/M11/Logs/phase6-runtime-lava-process-20260803.txt` |
| Runtime warnings | The process sample still contains the known optional `com.google.android.play.core.assetpacks.AssetPackManager` lookup and Lava `gralloc`/vendor buffer noise; these did not terminate the process | `Builds/M11/Logs/phase6-runtime-lava-process-20260803.txt` |
| Performance smoke snapshot | Lava after ~20 seconds: 507,397 KB PSS, 644,576 KB RSS, 97,884 KB Graphics PSS; Chrome: 120,502,144-byte WASM transfer, 274.8 ms WASM resource duration, 30,464,015-byte used JS heap | `Docs/PERFORMANCE_BUDGET.md`, `Builds/M11/Logs/phase6-lava-gfxinfo-20260803.txt`, `Builds/M11/Logs/phase6-lava-meminfo-20260803.txt` |

The new PlayMode coverage is functional evidence, not visual approval: it relocates an
authored Dhol pickup before restarting the authority-backed scene to exercise spatial
collection deterministically, invokes the generated Results/Rematch buttons after
authoritative lethal damage, repeats three real reload cycles while freezing scene
activation long enough to disable bot-side fixture activity, and walks all eight visible
tutorial prompts before replaying and skipping the overlay. The visual QA gate remains
in progress because no human-facing gadget-use or results/rematch screenshot has been
captured.

## Fresh Phase 2 continuation (`1f59a68`)

This section supersedes the older historical checkpoints below for the current source
HEAD. The fresh artifacts and runtime samples were generated from the Phase 2 live
fighter-controller coverage commit.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode tests | 89 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/phase2-full-editmode-20260803.xml` |
| PlayMode tests | 39 passed, 0 failed, 0 skipped; includes live Pehel and Maya ability tests | `Builds/M11/TestResults/phase2-full-playmode-20260803.xml` |
| Android development build | Unity exit 0; IL2CPP APK 151,305,385 bytes; SHA-256 `42BCF8B076AACC8C099462BB0DE3028183A84E2D7959DBE5A1BF77BC2C57CEDB` | `Builds/M11/Logs/android-build.log` |
| Web development build | Unity exit 0; 21 files, 133,194,254 bytes; WASM 120,501,562 bytes; SHA-256 `B8CED1C03AE4F9D6BCF1B601A3B333DD149C9E5228A97BAB7171E5A17D7EA805` | `Builds/M11/Logs/web-build.log` |
| Local Web serve | `http://127.0.0.1:8137/index.html` returned HTTP 200 | local HTTP check from 2026-08-03 |
| Browser runtime | Fresh Chrome/Playwright load reached the live match after an 8-second warmup; inspected capture has no blank canvas and console scan returned 0 errors/0 warnings | `Docs/QA/Visual/Phase7/playwright-phase2-runtime-20260803.png`; Playwright CLI output from 2026-08-03 |
| Photon/package check | Unity `6000.5.6f1`; Fusion build `2.1.1 Stable 2177`; Input System `1.20.0`; URP `17.5.0`; Test Framework `1.7.0` | `Assets/Photon/Fusion/build_info.txt`, `Packages/manifest.json` |
| Android device inventory | `adb devices -l` showed Lava `ST5GDW23LB004392` and Oppo `b60e53b3`; only the Lava serial was used | ADB output from 2026-08-03 |
| Lava smoke result | Exact Phase 2 APK installed/launched only on Lava `ST5GDW23LB004392`; process `13173` remained alive; no fatal Unity/Android exception, missing-component marker or monotonic-tick error in the captured process sample | `Docs/QA/Visual/Phase7/android-lava-phase2-runtime-20260803.png`, `Builds/M11/Logs/phase2-runtime-lava-logcat-20260803.txt` |

The Phase 2 controller tests also removed invalid ScriptableObject component lookups
from fighter-definition fallback paths. Pehel's charge cast now ignores fighter
colliders while retaining static-geometry blocking, so an opposing target can be
captured instead of prematurely ending the charge.

The prior `46a3d1e` APK exposed a real multi-step render-frame tick defect; the
pre-fix evidence is retained at `Docs/QA/Visual/Phase7/android-lava-pre-fix-tick-error-46a3d1e.png`.
The earlier `a245f24` baseline records the corrected fixed-tick retest below.

## Historical checkpoints retained from earlier runs

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-02 |
| Unity compile | Unity batchmode compile — exit 0; no compiler failure markers | `Builds/M11/Logs/phase1-clock-20260803-compile.log` |
| EditMode tests | 71 passed, 0 failed | `Builds/M11/TestResults/phase1-clock-20260803-editmode.xml` |
| PlayMode tests | 27 passed, 0 failed | `Builds/M11/TestResults/phase1-clock-20260803-playmode.xml` |
| Phase 1 timeout regression | Deterministic health/distance/id ranking; complete placements; EditMode 59/59 and PlayMode 27/27 pass | `Builds/M11/TestResults/phase1-editmode.xml`, `Builds/M11/TestResults/phase1-playmode.xml` |
| Fixed-clock integration | 30 Hz accumulator separates render frames from authoritative match steps; EditMode 60/60 and PlayMode 27/27 pass | `Builds/M11/TestResults/clock-editmode.xml`, `Builds/M11/TestResults/clock-playmode.xml` |
| Aandhi interpolation | Zone radius is continuous across opening/pressure transitions; EditMode 60/60 and PlayMode 27/27 pass | `Builds/M11/TestResults/aandhi-editmode.xml`, `Builds/M11/TestResults/aandhi-playmode.xml` |
| Bot zone awareness | Bot perception includes current/next zone and the decision engine repositions proactively; EditMode 61/61 and PlayMode 27/27 pass | `Builds/M11/TestResults/botzone-editmode.xml`, `Builds/M11/TestResults/botzone-playmode.xml` |
| Authority seam | `OfflineMatchAuthority` owns zone-damage cadence and emits immutable `DamageRequest` intents; EditMode 62/62 and PlayMode 27/27 pass | `Builds/M11/TestResults/authority-editmode.xml`, `Builds/M11/TestResults/authority-playmode.xml` |
| Combat statistics | Instigator-aware events record damage, eliminations, survival time and duplicate-credit prevention; EditMode 70/70 and PlayMode 27/27 pass | `Builds/M11/TestResults/fighter-final-editmode.xml`, `Builds/M11/TestResults/fighter-final-playmode.xml` |
| Aandhi warning/preview | Warning state, remaining warning time and next-radius preview are part of the immutable tick result; EditMode 70/70 and PlayMode 27/27 pass | `Builds/M11/TestResults/fighter-final-editmode.xml`, `Builds/M11/TestResults/fighter-final-playmode.xml` |
| Tick-based presentation timing | Player movement, Bijli ability, projectile stepping, gadget timers and weapon attack cooldown consume a 30 Hz clock; EditMode 70/70 and PlayMode 27/27 pass | `Builds/M11/TestResults/fighter-final-editmode.xml`, `Builds/M11/TestResults/fighter-final-playmode.xml` |
| Authority item collection | Pickup availability/respawn and gadget collection are decided in `OfflineMatchAuthority`; EditMode 70/70 and PlayMode 27/27 pass | `Assets/BattleRaja/Core/Domain/MatchItems.cs`, `Builds/M11/TestResults/fighter-final-editmode.xml`, `Builds/M11/TestResults/fighter-final-playmode.xml` |
| Fighter-specific ability boundary | `IFighterAbilityController` selects fixed-tick Pehel charge/capture/throw and Maya targetable decoy adapters; domain tests pass, but regenerated-scene runtime coverage is still open | `Assets/BattleRaja/Presentation/Combat/PehelFighterController.cs`, `Assets/BattleRaja/Presentation/Combat/MayaFighterController.cs`, `Builds/M11/TestResults/fighter-final-editmode.xml` |
| Android build | Latest validated development IL2CPP APK — exit 0; Unity build report succeeded | `Builds/M11/Logs/phase1-clock-2c6958a-android-build.log` |
| Android artifact | 150,927,346 bytes; SHA-256 `4AB8D6537CC7BFD2F06547D3938E1D11C379830C3928EA15A01F6F77EA7C637B` | `Builds/M11/Android/BattleRaja-M11.apk` |
| Android device smoke | Installed and launched on Lava `ST5GDW23LB004392` only; focused activity `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`; process `26865` observed | `Docs/QA/Visual/phase1-clock-2c6958a-android.png`; `Builds/M11/Logs/phase1-clock-2c6958a-android-logcat.txt` |
| Android runtime scan | No `FATAL EXCEPTION`, `SIGSEGV`, `AndroidRuntime`, `Can't add component`, or `SphereCollider` matches in the post-install log sample | ADB logcat sample after launch |
| Web build | Latest validated development Web build — exit 0; 21 files, 132,400,579 bytes; `index.html` present | `Builds/M11/Logs/phase1-clock-2c6958a-web-build.log`, `Builds/M11/Web` |
| Local Web serve | `python -m http.server 8020 --directory Builds/M11/Web`; `http://127.0.0.1:8020/index.html` returned HTTP 200 | local server/check output |
| Browser smoke | Chrome loaded the latest validated build in a fresh tab; DOM exposed the Unity player controls and post-load error/warning log query returned zero entries | `Docs/QA/Visual/phase1-clock-2c6958a-web.png` |

## Phase 1 authority/tick continuation (`ac60062`)

The following checks were rerun after the authority-tick changes. The user-owned
`MovementLab.unity` scene remained unstaged; the Bijli PlayMode test now places its
fixture actor explicitly so scene-local wall edits cannot change the movement assertion.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode authority/tick suite | 83 passed, 0 failed | `Builds/M11/TestResults/phase1-authority-editmode-20260803.xml` |
| Fixed-clock render-rate equivalence | `CoreFoundationTests.FixedClockProducesTheSameOfflineMatchStateAcrossRenderRates` passed for 30/60/90 Hz and a variable render cadence | `Builds/M11/TestResults/phase1-authority-editmode-20260803.xml` |
| Gadget authority cooldown | `AuthorityAdvancesGadgetCooldownOnAuthoritativeTicks` passed after 300 authoritative 30 Hz ticks | `Builds/M11/TestResults/phase1-authority-editmode-20260803.xml` |
| Aandhi intent tick identity | Authority test passed with `MatchAuthorityTick.SimulationTick == DamageRequest.SimulationTick` | `Builds/M11/TestResults/phase1-authority-editmode-20260803.xml` |
| Isolated Bijli regression | 1 passed, 0 failed after explicit fixture placement | `Builds/M11/TestResults/phase1-authority-bijli-fixture-20260803.xml` |
| PlayMode authority/tick suite | 33 passed, 0 failed | `Builds/M11/TestResults/phase1-authority-playmode-v2-20260803.xml` |

The new authority seam is still offline-only. Presentation gadget effect execution
remains outside the application authority, and the Android artifact has not yet been
installed on the connected Lava device for this exact `1437e5c` source.

## Phase 1 authority collection continuation (`1437e5c`)

Item proximity and collector selection now run in `OfflineMatchAuthority` from
validated domain positions/radii. The controller applies returned collection intents
to Unity health/inventory components and synchronizes view availability; it no longer
chooses collectors by scene iteration order.

| Check | Command/result | Evidence |
| --- | --- | --- |
| EditMode collection suite | 84 passed, 0 failed | `Builds/M11/TestResults/phase1-authority-collection-editmode-v2-20260803.xml` |
| PlayMode collection regression | 33 passed, 0 failed | `Builds/M11/TestResults/phase1-authority-collection-playmode-v2-20260803.xml` |
| Authority collection rule | Deterministic lowest-ID eligible collector, health/full-health filtering, authored position/radius and non-contiguous pickup-ID lookup covered by `AuthorityFoundationTests` | `Assets/BattleRaja/Core/Application/OfflineMatchAuthority.cs`, `Assets/BattleRaja/Tests/EditMode/AuthorityFoundationTests.cs` |
| Android development smoke build | Unity exit 0; APK 151,198,767 bytes; SHA-256 `360A4A0F4A595E5714B579226D6A157E06AA7992F1B3285DF7C4C26C7A43438C` | `Builds/M11/Logs/phase1-authority-collection-android-20260803.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development smoke build | Unity exit 0; 21 files, 132,979,355 bytes; `Build/Web.wasm` 120,294,960 bytes; SHA-256 `E957A343680D4C0205BFB2806946C0CEB5F95FD868AFA61D9FEA1199B9D048D1` | `Builds/M11/Logs/phase1-authority-collection-web-20260803.log`, `Builds/M11/Web` |
| Local Web serve | `python -m http.server 8136 --bind 127.0.0.1 --directory Builds/M11/Web`; `curl -I http://127.0.0.1:8136/index.html` returned HTTP 200 | local server check from 2026-08-03 |
| Lava physical smoke | Not run: `adb devices -l` listed only Oppo `b60e53b3`; the instructed Lava serial `ST5GDW23LB004392` was absent, so no other device was used | device-gate blocker |

This phase does not claim full visual/browser interaction correctness, real Photon
multiplayer, or authoritative gadget effect execution. The previous browser bootstrap
evidence remains the latest browser runtime evidence.

## Phase 2 fighter-controller continuation (`b7d1a60`)

`BotBrain` no longer defaults a missing reference to `BijliFighterController`. It resolves
the attached `IFighterAbilityController`, preserving Pehel and Maya ability identity when
scene serialization omits an explicit reference.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings | command output from 2026-08-03 |
| EditMode suite | 84 passed, 0 failed | `Builds/M11/TestResults/phase2-fighter-editmode-20260803.xml` |
| Targeted controller test | 1 passed, 0 failed; every production bot resolves the controller attached to its actor | `Builds/M11/TestResults/phase2-fighter-bot-controller-playmode-v2-20260803.xml` |
| Full PlayMode suite | 34 passed, 0 failed | `Builds/M11/TestResults/phase2-fighter-playmode-20260803.xml` |

This is a controller-boundary correction, not proof of final fighter balance, authored
animation/VFX/audio, human counterplay approval, or real network authority. The latest
Android/Web artifacts remain the `1437e5c` smoke builds until a new candidate is built.

## Phase 1 gadget-authority continuation (`dd35f53`)

Dhol Burst now evaluates living targets from the application snapshot and returns
per-target displacement intents. `GadgetUser` applies those intents to Unity actors; its
actor-scan path is retained only for an isolated non-authoritative lab.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | 0 errors, 0 warnings | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` |
| EditMode suite | 84 passed, 0 failed | `Builds/M11/TestResults/phase1-dhol-authority-editmode-20260803.xml` |
| PlayMode suite | 34 passed, 0 failed | `Builds/M11/TestResults/phase1-dhol-authority-playmode-20260803.xml` |
| Dhol authority rule | Dhol use emits one deterministic target displacement for the in-radius participant; duplicate command remains rejected | `Builds/M11/TestResults/phase1-dhol-authority-editmode-20260803.xml` |

No new Android/Web artifact was built for this focused authority-only change; the
`1437e5c` artifacts remain the latest cross-platform smoke builds. Tiffin station
damage forwarding and Umbrella mitigation remain presentation-owned.

## Phase 1 Tiffin-authority continuation (`39149f1`)

Tiffin station healing cadence and lifetime now run in the pure authority runtime.
Authority ticks emit target healing intents and station-expiry IDs; authority-driven
Unity station objects render the result without running a duplicate `Time.deltaTime`
healing loop. Local non-authoritative lab stations retain their fallback behavior.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | 0 errors, 0 warnings | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` |
| EditMode suite | 85 passed, 0 failed | `Builds/M11/TestResults/phase1-tiffin-authority-editmode-20260803.xml` |
| PlayMode suite | 34 passed, 0 failed | `Builds/M11/TestResults/phase1-tiffin-authority-playmode-20260803.xml` |
| Tiffin authority rule | Pure test observes healing intent at fixed cadence and expiry by the configured lifetime | `Builds/M11/TestResults/phase1-tiffin-authority-editmode-20260803.xml` |

No new Android/Web artifact was built for this focused authority-only change; the
`1437e5c` artifacts remain the latest cross-platform smoke builds. Station damage
forwarding and Umbrella mitigation remain open.

## Phase 1 gadget-authority candidate (`d328132`)

Umbrella Guard duration and front-facing mitigation now run through an authority-owned
runtime before damage reaches the presentation pipeline. Together with the Dhol and
Tiffin continuations above, all three gadget definitions have fixed-tick application
state for their currently implemented effects.

| Check | Command/result | Evidence |
| --- | --- | --- |
| EditMode suite | 86 passed, 0 failed | `Builds/M11/TestResults/phase1-umbrella-authority-editmode-v4-20260803.xml` |
| PlayMode suite | 34 passed, 0 failed | `Builds/M11/TestResults/phase1-umbrella-authority-playmode-v2-20260803.xml` |
| Android development smoke build | Unity exit 0; APK 151,286,644 bytes; SHA-256 `B50F74E56A3F3FA95A164834223739B3A0A2F1681DBA3D12F7B82B956F682F9B` | `Builds/M11/Logs/phase1-gadget-authority-android-20260803.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development smoke build | Unity exit 0; 21 files, 133,177,869 bytes; `Build/Web.wasm` 120,488,383 bytes; SHA-256 `6538B2ECF83940FEB7F9AE7F1B160114557E04F2E49F847048AD9795D711EB9E` | `Builds/M11/Logs/phase1-gadget-authority-web-20260803.log`, `Builds/M11/Web` |
| Local Web serve | `curl -I http://127.0.0.1:8136/index.html` returned HTTP 200 | local server check from 2026-08-03 |
| Build warning | Web build succeeded with a non-fatal websockify `EADDRINUSE` shutdown message on helper port 35020 | `Builds/M11/Logs/phase1-gadget-authority-web-20260803.log` |
| Lava physical smoke | Not run: `adb devices -l` listed only Oppo `b60e53b3`; the instructed Lava serial `ST5GDW23LB004392` was absent | device-gate blocker |

The fresh artifacts are development builds only. No production signing, store upload,
real Photon room, or final visual/performance claim is made.

## Fix included in this baseline

`Assets/BattleRaja/Presentation/Combat/CombatProjectilePool.cs` now references the concrete `SphereCollider` type before removing the generated primitive collider. This prevents IL2CPP/WebGL stripping from producing the runtime `Can't add component because class 'SphereCollider' doesn't exist!` error observed in the first Web/Android smoke pass.

## Warnings and limitations

- Unity batchmode logs contain non-fatal licensing-handshake messages, obsolete `PlayerSettings` API warnings, and expected Fusion/native-extension warnings.
- The screenshots are technical smoke evidence, not visual-approval evidence. The current scene is still a greybox/prototype with overlapping HUD text and has not passed visual QA.
- Only the connected Lava phone was used, per project instruction. The connected Oppo device was intentionally not used.
- No public deployment, store submission, production signing, service credential handling, or multiplayer claim was made.

## Post-checkpoint runtime revalidation

## Bazaar Bastion vertical-slice validation (`579bc37`)

- Compile/validation: Unity `ValidateProject` completed with exit 0; repository validation reported 0 errors and 0 warnings (`Builds/M11/Logs/bazaar-final-20260803-compile.log`).
- Regression: EditMode 72/72 and PlayMode 28/28 passed (`Builds/M11/TestResults/bazaar-final-20260803-editmode.xml`, `Builds/M11/TestResults/bazaar-final-20260803-playmode.xml`). The PlayMode suite loaded `BazaarBastion` and asserted the Pehel/Maya fighter-specific controllers and `BazaarArchitecture` root.
- Android build: development IL2CPP APK succeeded under Unity 6000.5.6f1 (`Builds/M11/Logs/bazaar-runtime-20260803-android-build.log`). Artifact `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk`: 150,960,195 bytes; SHA-256 `2A4AC8ACDB4A7873F07362E67B0ACA8B265C44840E66CC3A6AC8EB648B88D175`.
- Android smoke: installed and launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), package `com.example.battleraja.m11`. Screenshot: `Docs/QA/Visual/bazaar-runtime-579bc37-android.png`; logcat: `Builds/M11/Logs/bazaar-runtime-579bc37-android-logcat.txt`. No fatal Unity/Android crash markers were observed; the sample includes non-fatal device noise and a Google Play AssetPackManager `ClassNotFoundException` from the development player, which is not a crash but remains a release-hardening follow-up.
- Web build: development Web build succeeded (`Builds/M11/Logs/bazaar-runtime-20260803-web-build.log`), 18 files, 132,379,495 bytes, served from `http://127.0.0.1:8021/index.html` with HTTP 200.
- Browser smoke: Chrome loaded the fresh local Web tab, exposed the Unity player controls, and returned zero captured error/warning entries after load. Screenshot: `Docs/QA/Visual/bazaar-runtime-579bc37-web.png`.
- Visual boundary: the scene is a stylised greybox with Bazaar palette blocks/stalls and overlapping prototype HUD text. This is technical vertical-slice evidence, not final visual approval.

## Visual/audio foundation validation (`4ecc467`)

- Compile: Unity `ValidateProject` completed with exit 0; the first visual-pass compile caught a missing `CombatFaction.Ally` reference, which was corrected before the passing compile (`Builds/M11/Logs/phase4-visual-20260803-compile-v3.log`).
- Regression: EditMode 72/72 and PlayMode 29/29 passed after regenerating Bazaar Bastion with `FighterPresentation` and `BattleRajaAudioDirector` (`Builds/M11/TestResults/phase4-visual-20260803-editmode.xml`, `Builds/M11/TestResults/phase4-visual-20260803-playmode-v2.xml`).
- Android build: development IL2CPP APK succeeded (`Builds/M11/Logs/phase4-visual-20260803-android-build.log`). Artifact: 151,031,756 bytes; SHA-256 `A26456EB4FC447E35AA10421361BD0F1818B1622356D2003686CCE4B9B1A5C4B`.
- Android smoke: installed/launched only on Lava `ST5GDW23LB004392`; no fatal Unity/Android crash markers, missing-component errors or null-reference markers were observed in the captured sample (`Builds/M11/Logs/phase4-visual-20260803-android-logcat.txt`). Screenshot: `Docs/QA/Visual/phase4-visual-20260803-android.png`.
- Web build: development Web build succeeded (`Builds/M11/Logs/phase4-visual-20260803-web-build.log`), 19 files, 132,677,491 bytes; local HTTP `200` from `http://127.0.0.1:8021/index.html`.
- Chrome smoke: the fresh tab loaded the Unity player, exposed the expected Profile/Unload controls, and returned zero captured error/warning entries. Screenshot: `Docs/QA/Visual/phase4-visual-20260803-web.png`.
- Presentation boundary: original procedural placeholder cues are gesture-gated for Web and optional mixer hooks are present, while imported animation clips, authored VFX/music/SFX, reduced-flash quality profiles and final visual approval remain open.

The current latest-head revalidation supersedes the earlier post-authority artifact values above:

- HEAD: `2c6958a` (`fix: finish fixed tick bot and winner seams`).
- Compile: exit 0; EditMode 71/71; PlayMode 27/27.
- Android: `Builds/M11/Android/BattleRaja-M11.apk`, 150,927,346 bytes, SHA-256 `4AB8D6537CC7BFD2F06547D3938E1D11C379830C3928EA15A01F6F77EA7C637B`; installed/launched on Lava `ST5GDW23LB004392` only; no fatal/crash/AndroidRuntime/SphereCollider markers in the captured logcat sample.
- Web: 21 files, 132,400,579 bytes; local HTTP 200; fresh Chrome tab loaded with zero captured error/warning entries; screenshot `Docs/QA/Visual/phase1-clock-2c6958a-web.png`.

After the authority seam commit `1a92b6c`, the runtime artifacts were rebuilt and smoke-tested again:

- Web: 21 files, 132,170,851 bytes; local HTTP `200`; Chrome fresh-tab smoke reported 0 JavaScript errors. Updated screenshot: `Docs/QA/Visual/latest-head-web.png`.
- Android: 150,813,125 bytes; SHA-256 `A6176D2BCE8A7856555F512B56C9A2679590E601B93F670CAC5FCD8AD4DDA455`; installed/launched on Lava `ST5GDW23LB004392` only (PID `23626`) with the Unity game activity focused. No fatal, crash, SphereCollider or AndroidRuntime matches were found in the post-launch log sample. Updated screenshot: `Docs/QA/Visual/latest-head-android.png`.
- Authority/statistics/tick/item/fighter regression: 70 EditMode and 27 PlayMode tests pass after the focused foundation changes.

## Canvas match UI foundation validation (`380781b`)

- Repository validation: `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` — 0 errors, 0 warnings.
- Regression: EditMode 72/72 and PlayMode 29/29 passed after the Canvas HUD and Unity 6
  `LegacyRuntime.ttf` runtime-font correction (`Builds/M11/TestResults/phase5-ui-20260803-editmode-final.xml`,
  `Builds/M11/TestResults/phase5-ui-20260803-playmode-final.xml`). The first PlayMode
  run caught the unsupported `Arial.ttf` resource and was not counted as final evidence.
- Web build: development build succeeded under Unity 6000.5.6f1 (`Builds/M11/Logs/phase5-ui-20260803-web-build-final2.log`), 19 files and 132,679,391 bytes; the main WASM SHA-256 is `611E64BE25E6C953BA72EC7F7DDE268581E3EB4EF42C9B330382AAF5F161A6F8`.
- Chrome Web smoke: `http://127.0.0.1:8021/index.html` returned HTTP 200; the fresh tab exposed the Unity player controls and captured zero error/warning entries. Screenshot: `Docs/QA/Visual/phase5-ui-20260803-web.png`.
- Android build: development IL2CPP APK succeeded (`Builds/M11/Logs/phase5-ui-20260803-android-build-final2.log`). Actual artifact size is 151,033,372 bytes; SHA-256 is `3D83BE32A990F66DE000A7F34A00AF75B54A85CDBF1346F93321304409464789`.
- Android smoke: installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`), package `com.example.battleraja.m11`. The post-launch log sample contains zero fatal/crash/missing-component/Unity-error markers (`Builds/M11/Logs/phase5-ui-20260803-android-logcat-final.txt`). Screenshot: `Docs/QA/Visual/phase5-ui-20260803-android.png`.
- Product boundary: the Canvas surface now covers match/zone status, pause/settings,
  spectator cycling, results/rematch and left-handed/reduced-flash/high-contrast hooks.
  Bootstrap/main-menu flow, full tutorial/offline progression, localization assets,
  controller rebinding, functional aim assist, final authored UI and human visual review
  remain open. The scene and screenshots are still stylised greybox evidence, not a
  release or store-readiness claim.

## Production flow and fighter selection validation (`2c36bbb`)

- Source/control: pushed branch `codex/product-completion` is at `2c36bbb`; the user-owned
  dirty `MovementLab.unity` and Burst WASM files were explicitly excluded from the commit.
- Compile/validation: `BattleRaja.Editor.BuildEntrypoints.ValidateProject` exited 0 and
  `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe`
  reported 0 errors and 0 warnings (`Builds/M11/Logs/flow-compile-20260803c.log`).
- EditMode: 77 passed, 0 failed (`Builds/M11/TestResults/flow-editmode-final-20260803.xml`).
- PlayMode: 31 passed, 0 failed (`Builds/M11/TestResults/flow-playmode-final-20260803.xml`).
  This includes Bootstrap main-menu/offline/online-error routing and the Bazaar runtime
  Pehel controller-binding test.
- Android: development IL2CPP build succeeded with Bootstrap and Bazaar scenes in build
  order (`Builds/M11/Logs/flow-android-head-2c36bbb.log`). Actual APK size is 151,092,503
  bytes; SHA-256 is `B664BD85DAD93C6151A93E3C93DEE7640A077F395C2246E71AB3E802BBE83856`.
  No install or physical-device claim was made in this flow turn.
- Web: development build succeeded with Bootstrap and Bazaar scenes
  (`Builds/M11/Logs/flow-web-head-2c36bbb.log`), 19 files and 132,757,798 bytes. Main WASM
  is 120,095,092 bytes with SHA-256
  `8FAE0E0063B46B97CCC5579010E52BF1076C8839E9089DD3723F7A2EC41E7CBF`.
- Web runtime: local HTTP `http://127.0.0.1:8123/index.html` returned 200. At 1280×720,
  browser inspection showed Main Menu, Mode Selection, Fighter Selection (including Maya),
  Settings and the main-menu Online path's explicit Photon-unavailable error. Screenshots
  are stored under `Docs/QA/Visual/Flow/`. Browser logs contained no errors/warnings in
  the final HEAD smoke.
- Boundary: this is production-flow evidence, not final visual QA. The UI remains original
  stylised greybox presentation; tutorial, multi-viewport visual gate, Android Lava runtime
  check for this exact APK, performance profiling, real Photon and real PlayFab remain open
  or externally blocked.

## Replayable tutorial arena validation (`4391f09`)

- Source/control: pushed branch `codex/product-completion` is at `4391f09`; the existing
  user-owned `MovementLab.unity`, Burst WASM and audit-brief files remain unstaged.
- Compile/validation: `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` reported 0 errors and 0 warnings.
- EditMode: 81 passed, 0 failed, 0 skipped (`Builds/M11/TestResults/tutorial-editmode-20260803.xml`). This includes the pure ordered/replayable `TutorialStepMachine` and the tutorial flow route.
- PlayMode: 33 passed, 0 failed, 0 skipped (`Builds/M11/TestResults/cleanup-playmode-20260803.xml`). `TutorialArenaPlayModeTests` loaded the real `OfflineMatchController`, observed eight valid authority participants, verified all bot brains are disabled while actors remain in the simulation, and advanced the overlay to completion. `OfflineMatchPlayModeTests.RepeatedProductionSceneLoadsKeepOneOfflineRuntimeGraph` loaded the production arena three times and found one match controller, eight authority actors and a reset time scale on every iteration.
- Android: development IL2CPP build succeeded under Unity 6000.5.6f1 (`Builds/M11/Logs/tutorial-android-head-4391f09.log`). Actual APK `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk` is 151,120,618 bytes; SHA-256 `5366ACE6BA99BA25370E974399D70D0D76327F07D842F3C583D8B9C9A3C7C260`. This turn did not install this exact APK on a physical device; Lava remains the only permitted device for a later runtime check.
- Web: development build succeeded with Bootstrap, Tutorial Arena and Bazaar Bastion (`Builds/M11/Logs/tutorial-web-head-4391f09.log`), 19 files and 132,784,825 bytes. Main WASM is 120,095,092 bytes with SHA-256 `EF4548D592B4B7263B418192DBC0DBA478B1630E5AA5B30E5C28A531BEB11392`.
- Local Web serve: `http://localhost:8123/index.html` returned HTTP 200 from `Builds/M11/Web-BazaarBastion`.
- Browser smoke: at 1280×720, the fresh Web tab opened Main Menu → `TUTORIAL REPLAY`, rendered the real Tutorial Arena, and advanced Movement → Aim. Screenshots: `Docs/QA/Visual/Flow/web-tutorial-arena.png` and `Docs/QA/Visual/Flow/web-tutorial-aim.png`. Browser logs contained no errors; one non-fatal Unity WebGL warning reports that manual `persistentDataPath` synchronization is deprecated.
- Boundary: tutorial prompts are replayable guidance layered over the real controls, match authority, Aandhi/HUD and pickups; they do not automatically certify player competency. Multi-viewport visual QA, Lava smoke for this exact APK, performance/soak evidence, final authored presentation, real Photon and real PlayFab remain open or externally blocked.

## Visual and interaction QA validation (`511f2f4` source / `4391f09` runtime)

- Local Web candidate served successfully at `http://localhost:8123/index.html` and was inspected in a fresh in-app browser tab at `1280×720`.
- Captured states include main menu, mode selection, fighter selection, match loading/opening, active combat, Aandhi closing pressure, pause/settings and the explicit online/Fusion error. Evidence is stored under `Docs/QA/Visual/Phase7/` and summarized in `Docs/QA/VISUAL_QA_REPORT.md`.
- The available browser surface did not expose viewport resizing, so 1920×1080, 1440×900, 1024×768 and portrait checks were not claimed. Successful gadget pickup/use, spectator, results/rematch and final visual approval were not reached through the honest smoke path.
- This is an **In progress** visual gate. The screenshots show a stylised greybox/prototype with dense HUD coverage; they are not final visual approval.

## Visual and interaction QA Playwright revalidation (`511f2f4` source / `4391f09` runtime)

- The same `Builds/M11/Web-BazaarBastion` candidate was served at `http://localhost:8124/index.html` and exercised with the Playwright CLI.
- Required desktop viewports were captured: `1920×1080`, `1440×900`, `1280×720` and `1024×768`. Main-menu evidence exists for all four; match-opening evidence exists for all four. Portrait `390×844` was also captured because the browser supports resizing.
- Desktop menu, mode, fighter, match, pressure, spectator, settings and online-error surfaces were visually inspected. Evidence is under `Docs/QA/Visual/Phase7/playwright-*.png` and summarized in `Docs/QA/VISUAL_QA_REPORT.md`.
- Portrait menu fits, but portrait gameplay is horizontally cropped and the tutorial overlay is clipped. Gadget pickup/use, a distinct loading surface and results/rematch were not captured through the honest run.
- This remains an **In progress** visual gate and does not establish final art quality, mobile-Web readiness, physical Lava ergonomics, or human approval.

## Fresh latest-HEAD rebaseline (`1a41c09`)

- Repository validation: `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` returned 0 errors and 0 warnings.
- Unity compile/import: Unity `6000.5.6f1` completed the batchmode import/compile path with exit code 0. Photon Fusion editor import emitted a non-fatal custom-dependency scheduling warning; no compiler error was observed.
- EditMode: 81 passed, 0 failed, 0 skipped (`Builds/M11/TestResults/rebaseline-20260803-editmode-v4.xml`).
- PlayMode: 33 passed, 0 failed, 0 skipped (`Builds/M11/TestResults/rebaseline-20260803-playmode-v1.xml`).
- Android: fresh development IL2CPP build succeeded (`Builds/M11/Logs/rebaseline-20260803-android-v1.log`). `Builds/M11/Android/BattleRaja-M11.apk` is 151,120,318 bytes; SHA-256 `1B74943F56D3474238320819F853D2C21DF5AB2648CDB0DC88929F85E328393E`. This exact APK was not installed; the Lava-only physical-device gate remains open.
- Web: fresh development build succeeded (`Builds/M11/Logs/rebaseline-20260803-web-v1.log`). The output contains 19 build files (excluding the two older DOM-check text files) totaling 132,773,639 bytes; `Build/Web.wasm` is 120,109,688 bytes with SHA-256 `EF4548D592B4B7263B418192DBC0DBA478B1630E5AA5B30E5C28A531BEB11392`.
- Local Web serve: `python -m http.server 8130` from `Builds/M11/Web`; `http://127.0.0.1:8130/index.html` returned HTTP 200.
- Browser bootstrap: Playwright loaded the fresh URL in Chromium; the Unity player title and controls appeared, and the console reported 0 errors and 0 warnings. This is bootstrap evidence, not visual gameplay approval.
- Build warnings/limitations: existing obsolete Unity `FindObjectsByType` API warnings and non-fatal licensing/Fusion/native-extension messages remain. The build's internal websockify helper also logged an `EADDRINUSE` shutdown warning while the player build itself reported success. Build-generated changes to `Bootstrap.unity` and `TutorialArena.unity` were restored because those files were clean before the baseline.

## Phase 1 Tiffin station-authority continuation (`a9c93fc`)

Tiffin station damage now enters the same application-owned runtime as healing and
lifetime. The resolver validates the target request, forwards damage to the authority,
applies only the authoritative amount to the Unity view, and expires the view when the
authority reports destruction.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | 0 errors, 0 warnings | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` |
| EditMode suite | 87 passed, 0 failed | `Builds/M11/TestResults/phase1-station-authority-editmode-final-20260803.xml` |
| PlayMode suite | 35 passed, 0 failed | `Builds/M11/TestResults/phase1-station-authority-playmode-final-20260803.xml` |
| Authority station rule | Pure test covers partial damage, capped destruction and removal; PlayMode resolves damage through the live match authority | `AuthorityFoundationTests.AuthorityOwnsTiffinStationDamageAndRemovesDestroyedStations`, `GadgetPlayModeTests.TiffinStationDamageIsAcceptedThroughMatchAuthority` |
| Android development smoke build | Unity build success; APK 151,286,056 bytes; SHA-256 `9D6EFD324FE0C83427868AE59136FF276F51A608F5C78A43A5B7A5AE3D1482E5` | `Builds/M11/Logs/phase1-station-authority-android-20260803.log`, `Builds/M11/Android/BattleRaja-M11.apk` |
| Web development smoke build | Unity build success; 21 files, 133,182,055 bytes; `Build/Web.wasm` 120,491,763 bytes; SHA-256 `B66C0F97A06FB2E60384D9B59D5C3323BBA53E8B3344FC8BF6657233ED1D8AD7` | `Builds/M11/Logs/phase1-station-authority-web-20260803.log`, `Builds/M11/Web` |
| Local Web serve | `curl -I http://127.0.0.1:8136/index.html` returned HTTP 200 | local server check from 2026-08-03 |
| Web build warning | Build succeeded; non-fatal Unity WebGL script-debugging warning and websockify helper shutdown message remain | `Builds/M11/Logs/phase1-station-authority-web-20260803.log` |
| Lava physical smoke | Not run: `adb devices -l` still did not list `ST5GDW23LB004392`; the connected Oppo device was intentionally not used | device-gate blocker |

This is authority/test/build evidence only. It does not establish final visual quality,
physical-device validation, performance, real Photon multiplayer or PlayFab integration.

## Compact portrait HUD continuation (`913988a`)

- Repository validation: `Tools\\Validation\\validate.ps1 -RequireUnityProject
  -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` returned 0 errors and 0 warnings.
- EditMode: 87 passed, 0 failed, 0 skipped
  (`Builds/M11/TestResults/hud-compact-editmode-20260803.xml`).
- PlayMode: 37 passed, 0 failed, 0 skipped
  (`Builds/M11/TestResults/hud-compact-playmode-20260803.xml`). The added
  `CompactMatchStatusKeepsZoneTelemetryReadable` regression test covers the compact
  two-line format and warning text.
- Android: fresh development IL2CPP build succeeded under Unity 6000.5.6f1
  (`Builds/M11/Logs/hud-compact-android-20260803.log`).
  `Builds/M11/Android/BattleRaja-M11.apk` is 151,304,957 bytes; SHA-256
  `A417601C7D4FDCFD5B6D18EBAC88AC28486496740881518A285EF93983FD48C4`. This exact
  APK was not installed; the Lava-only physical-device gate remains open because
  serial `ST5GDW23LB004392` was not connected.
- Web: fresh development build succeeded with the responsive template
  (`Builds/M11/Logs/hud-responsive-web-20260803.log`). The output contains 21 files
  totaling 133,185,154 bytes; `Build/Web.wasm` is 120,493,880 bytes with SHA-256
  `FB8380DC4FCD5B8674D03EF99CF25BBF65B2C280EBB98F3638C05159C248EF09`.
- Browser smoke: local HTTP `http://127.0.0.1:8137/index.html` returned 200. A fresh
  Playwright 390×844 capture is stored at
  `Docs/QA/Visual/Phase7/playwright-390x844-hud-compact.png`; phase/alive/zone
  telemetry is visibly separated from the fighter/gadget HUD. This is a visual
  readability observation, not final visual approval.
- Boundary: gadget success, loading/results surfaces, touch ergonomics, physical Lava
  validation, performance, real Photon and PlayFab remain open or externally blocked.

## Responsive Web framing continuation (`67a4d40`)

- Repository validation: `BattleRaja.Editor.BuildEntrypoints.ValidateProject` exited 0
  with the expected non-fatal Unity licensing/import messages; no project validation
  errors or warnings were reported.
- EditMode: 87 passed, 0 failed, 0 skipped
  (`Builds/M11/TestResults/phase7-responsive-editmode-20260803.xml`).
- PlayMode: 36 passed, 0 failed, 0 skipped
  (`Builds/M11/TestResults/phase7-responsive-playmode-20260803.xml`). The added
  framing assertion covers unchanged 16:9 size and expanded narrow-viewport framing.
- Android: fresh development IL2CPP build succeeded under Unity 6000.5.6f1
  (`Builds/M11/Logs/phase7-responsive-android-20260803.log`). `Builds/M11/Android/
  BattleRaja-M11.apk` is 151,288,356 bytes; SHA-256
  `1F9A7E6DAF65AE963E167FE8D055AE8C94757C4A2E9EE1ECE8B2BCDCA24BEA9F`. This exact
  APK was not installed; the Lava-only physical-device gate remains open because
  serial `ST5GDW23LB004392` was not connected.
- Web: fresh development build succeeded with the project-owned responsive template
  (`Builds/M11/Logs/phase7-responsive-web-v2-20260803.log`). The output contains 21
  files totaling 133,184,187 bytes; `Build/Web.wasm` is 120,493,105 bytes with
  SHA-256 `027599E8E1A201157C86305DA775A8F500048E89E9318FA002DE812E68186E74`.
- Browser smoke: local HTTP `http://127.0.0.1:8137/index.html` returned 200. A fresh
  Playwright run at 390×844 captured `Docs/QA/Visual/Phase7/
  playwright-390x844-responsive-gameplay.png`; the responsive host and camera framing
  fill the portrait viewport without the earlier fixed 960×600 horizontal crop. The
  capture still shows prototype-density HUD and is not mobile-Web or final visual
  approval.
- Production-flow Web recheck: `BuildWebBazaarBastionDevelopment` also succeeded in
  `Builds/M11/Web-BazaarBastion` (19 files, 133,179,836 bytes; the same main WASM
  hash as above). Local HTTP `http://127.0.0.1:8138/index.html` returned 200. The
  fresh route reached Bootstrap → Tutorial Arena at 390×844 and captured
  `Docs/QA/Visual/Phase7/playwright-390x844-responsive-tutorial.png`; the tutorial
  card and controls remained inside the portrait viewport. The browser emitted only
  Unity's known non-fatal `JS_FileSystem_Sync()` deprecation warning.
- Boundary: this is responsive framing and build evidence only. Gadget success,
  loading/results surfaces, touch ergonomics, physical Lava validation, performance,
  real Photon and PlayFab remain open or externally blocked. Build-generated changes
  to `Bootstrap.unity` and `TutorialArena.unity` were restored because those files
  were clean before the baseline.

## Fixed-tick runtime correction (`a245f24`)

The Lava baseline exposed a real multi-step render-frame defect: the fixed clock
advanced several ticks, while every presentation consumer reused the final tick for
each consumed step. `OfflineMatchAuthority.Advance` therefore rejected repeated tick
identities on the device. The correction adds `FixedSimulationClock.GetConsumedTick`
and updates the authority controller, movement, attacks, projectiles, bots, gadgets and
Bijli/Pehel/Maya ability adapters to pass the per-step tick.

| Check | Command/result | Evidence |
| --- | --- | --- |
| Repository validation | 0 errors, 0 warnings | `Tools\\Validation\\validate.ps1 -RequireUnityProject -UnityExe ...\\6000.5.6f1\\Editor\\Unity.exe` |
| EditMode regression | 89 passed, 0 failed, 0 skipped; includes sequential multi-step tick and authority acceptance tests | `Builds/M11/TestResults/fixed-tick-runtime-editmode-20260803.xml` |
| PlayMode regression | 37 passed, 0 failed, 0 skipped | `Builds/M11/TestResults/fixed-tick-runtime-playmode-20260803.xml` |
| Android retest | Unity exit 0; APK 151,318,469 bytes; SHA-256 `154F0F81FA6F068302FC57A6513A6C38CB4A7A8298C856264A04B14EE796574F` | `Builds/M11/Logs/fixed-tick-runtime-android-20260803.log` |
| Lava retest | Exact fixed-tick APK installed/launched on `ST5GDW23LB004392`; focused Unity activity remained alive; no current Unity-process monotonic-tick exception, fatal exception or SIGSEGV; inspected match capture is clean of the prior red Unity console error | `Docs/QA/Visual/Phase7/android-lava-fixed-tick-46a3d1e.png`, `Builds/M11/Logs/fixed-tick-runtime-lava-logcat-20260803.txt` |
| Web retest | Unity exit 0; 21 files, 133,187,266 bytes; WASM 120,495,480 bytes; SHA-256 `F02758353612C68904777C2F25F4F56E22DA4225F0F426A205792BE41E111EC6` | `Builds/M11/Logs/fixed-tick-runtime-web-20260803.log` |
| Browser retest | HTTP 200; fresh Playwright render reached live match; inspected screenshot has no blank canvas and console error/warning scan returned 0 | `Docs/QA/Visual/Phase7/playwright-1000x1000-fixed-tick-46a3d1e.png` |

Known non-blocking device log noise remains: Android/Unity reports absent optional
Google Play Asset Pack classes and Lava gralloc format warnings. These are separate
from the fixed-tick exception and did not stop the current Unity process. This phase
does not establish final visual quality, performance, real Photon multiplayer or
PlayFab integration.
