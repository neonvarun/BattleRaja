# V1 Android visual-feedback evidence — 2026-08-24

This is a bounded presentation slice for the offline Android V1 candidate. It is
technical evidence, not Play Store approval or final human visual approval.

## Latest exact-current release-shaped slice — `6ac5c12` — 2026-08-24

The exact current source is `6ac5c12` on `codex/v1-playstore-release`. Validation is
0/0, EditMode is **125/125**, and PlayMode is **67/67**. The release-shaped APK is
**39,523,632 bytes** (`09F5375FA8D5DEC066A09D8CCDF0BAF01269F4B402252EF2908691C773402EF3`)
and the AAB is **35,351,357 bytes**
(`70825F82A4D79E1E036F4DA8A286778244406D51B1D60A568BD066ED1B82DAA8`). The AAB is
ARM64-only and passed the static 16 KB alignment check. Only Lava
`ST5GDW23LB004392` was used; the release-shaped APK launch/menu capture is
`C:\Users\USER\AppData\Local\Temp\battleraja-lava-6ac5c12\release-menu.png`.
It shows the player-facing `OFFLINE ARCADE • 8-ACTOR MATCHES` footer and no
development-build watermark. Runtime now explicitly binds Input System point/click
actions and the PlayMode suite asserts those bindings. The Unity surface still
exposes no actionable nodes in the Android UI tree, so this capture does not prove
physical touch navigation; ADB coordinate probes remain diagnostic only.

The exact Web build is 19 files / **132,071,712 bytes**, with a
119,799,965-byte WASM (`8CE68A5AA4C741DD27AD66B9BF61FBC0B17DE9F632F2C791181EC99F516DEA12`).
Chrome and Edge loader screenshots are in
`C:\Users\USER\AppData\Local\Temp\battleraja-web-6ac5c12\`; both stayed on the
Unity loader during the bounded headless wait. Full interactive Web QA remains open.

## Exact current offline packaging candidate — 2026-08-24

The final documentation checkout is `HEAD` on `codex/v1-playstore-release`,
with runtime/package source `f4425d6` (documentation-only continuation). It now has
a fresh offline Android package boundary. Validation is **0 errors / 0 warnings**, EditMode is
**125/125**, and PlayMode is **66/66**. The APK is **39,529,326 bytes** with SHA-256
`AE74717B597C4CBCFDECF7D8DB719C177100F495CC084ABFD0E1EA6AAD3E2C52`; the AAB is
**35,357,477 bytes** with SHA-256
`8EB49EFC8D58D144E5A792224FC9A3570FF4E37F121E06B6E55093C9D4D5F5E7`. The AAB is
ARM64-only (7 native libraries) and passed static 16 KB alignment.

The exact APK was installed only on Lava `ST5GDW23LB004392`. The branded portrait
offline menu was visible, the Unity activity was focused, and the captured log
contains no fatal/ANR/SIGSEGV marker. A nonfatal optional Play Core
`AssetPackManager` `ClassNotFoundException` was present during Unity's asset-pack
probe. `aapt` and installed-package inspection show
`VIBRATE` and Unity's dynamic-receiver permission only; `INTERNET` and
`ACCESS_NETWORK_STATE` are absent. Raw files are outside the repository under
`C:\Users\USER\AppData\Local\Temp\battleraja-root-offline-manifest-lava\`.

This is packaging and launch evidence, not final touch/accessibility or performance
approval. The Web build was also refreshed from this checkout; its separate loader
smoke is recorded in `Docs/QA/V1_ANDROID_OFFLINE_PACKAGING_2026-08-24.md`.

## Exact current checkout artifact and Lava launch — 2026-08-24

The current branch tip is `357dfdf1e6289c172dab60e514f555ba3d5bc914`; the artifacts below
were built from runtime-equivalent source `46724ac2dfa403f40f58669240e61918c2a94d1b`.
The commits after that build are documentation only; no runtime gameplay code changed.
Exact runtime-source validation is **0 errors / 0 warnings**.

- APK: **40,431,927 bytes**, SHA-256
  `0694958A43F1BADD30E697095F249733992F9D6904E10E1923CD0CAF01010C78`.
- AAB: **36,262,036 bytes**, SHA-256
  `906D85FA00E4A9787A0C1DE892DC3F27A098ACF21BB1735E08C977565A1D09A4`.
- Static bundle validation: base manifest, 8 ARM64 libraries, no other ABIs and all
  checked ELF LOAD segments aligned to `0x4000`.
- The exact APK was installed only on Lava `ST5GDW23LB004392`. It launched the branded
  offline menu with `UnityPlayerGameActivity` top-resumed and no fatal/ANR/SIGSEGV marker.
- The APK manifest still contains `INTERNET` and `ACCESS_NETWORK_STATE` because the
  preserved future-facing Unity/Fusion build graph contributes them. The game does not
  start an online session in V1, but the permission must be removed or explicitly
  justified before a Play submission.
- Raw menu, memory, thermal/battery and logcat files are outside the repository at
  `C:\Users\USER\AppData\Local\Temp\battleraja-final-head-lava\`.
- The same Lava run opened Settings & Accessibility. Left-handed, reduced-flashes and
  high-contrast toggles visibly changed the persisted summary and high-contrast panel;
  captures are `settings.png`, `settings-accessibility-on3.png` and
  `menu-accessibility-on3.png` in that raw directory. This is an interaction probe, not
  final touch/accessibility approval.

The device sample is bounded only: **257,340 KB PSS**, **393,462 KB RSS**, **83,862 KB
Graphics**, **83 KB swap**, and SurfaceView log windows around 59.45–60.59 FPS. This is
not sustained performance approval.

## Tutorial completion correction — 2026-08-24

The follow-up source `c6badbf6cf5b1c7340fa907821aeb4cbf2194bc0`
(`fix: keep tutorial completion card visible`) closes a real Android UX defect found
while exercising the prior visual candidate: completing or skipping the tutorial hid
the only card while its full-screen backdrop remained, leaving a blank dark screen.
The completion card now stays visible with `TUTORIAL COMPLETE`, `8 / 8 COMPLETE`,
`REPLAY TUTORIAL` and `MENU` actions. Full validation remains 0/0, EditMode **125/125**
and PlayMode **66/66** in disposable copy
`C:\Projects\BattleRaja-v1-tutorial-verify`.

The exact correction APK is **40,431,923 bytes**, SHA-256
`E6CBEAD6F97C036C0C9D1663CA5972799AEF3B330D75A3D2AAA94D5E699C7DB3`. The exact AAB
is **36,262,021 bytes**, SHA-256
`124E14ABE6012B3B42D7B7741D0C647416E278E82ABFE358EF89A53BAAD64021`; bundle inspection
again found 8 ARM64 libraries, no other ABIs and `0x4000` native ELF LOAD alignment.
The APK was installed only on Lava `ST5GDW23LB004392`; `tutorial-fix-complete.png`
visually confirms the completion card after tapping SKIP. This is a focused UI fix and
does not replace the preceding exact-source Bazaar visual/results/rematch evidence. The
same exact correction APK also auto-collected the player-owned Tiffin Station at spawn;
`tutorial-fix-gadget-after.png` shows the held slot empty, a 13.8-second cooldown and the
visible deployed station after a valid use.

## Unity warning-clean continuation — 2026-08-24

The latest source commit `649d0bb` (`fix: remove obsolete Unity lookup overloads`) is
editor/test-only and leaves the `c6badbf` runtime unchanged. Full EditMode **125/125**
and PlayMode **66/66** passed in disposable copy
`C:\Projects\BattleRaja-v1-warning-verify`. The fresh non-development APK was
**40,431,911 bytes**, SHA-256
`51D86184F6C69DD30CD249D273FA0F8F5BA96B4159D86DD1472FE4FD54320DA5`.
The matching AAB is **36,262,033 bytes**, SHA-256
`518102EAE7DDB71DA9393ABE3E948A47440260C9DF8D19532AAFF14FA1BE98B0`; its bundle
checker passed the 8-ARM64/no-other-ABI/`0x4000` alignment gate. The Android build log
contains **0 `CS0618` warnings** and **0 C# errors**. No new
device install was needed because the changed files are editor/test-only; the exact
`c6badbf` correction APK remains the latest installed visual artifact.

## Android V1.0 visual-polish continuation — 2026-08-24

The exact current visual slice is source commit `abe9ae4816054e9704d13496bcd50bb7720eaa4f`
(`fix: add canopy gold material`) on `codex/v1-playstore-release`. It follows the
player-owned Tiffin route at `d825832b` without changing the authority contract.
Validation remains **0 errors / 0 warnings**, EditMode **125/125**, and PlayMode
**66/66** in disposable copy `C:\Projects\BattleRaja-v1-visual-verify`.

This slice makes the Bazaar Bastion landmark a six-panel fictional canopy with a
single gold orb instead of intersecting bars that could read as a sacred or unfinished
greybox symbol. It enlarges the procedural hero illustration for phone readability.
The visual change is presentation-only; no combat, collision, networking or service
scope was added.

The exact release-shaped APK and AAB were rebuilt from this source. APK size is
**40,432,119 bytes**, SHA-256
`5AFA5AFD4670520ED8B02340C117236F2B7118F824E3C819A6AD86CB7E2F2D91`. AAB size is
**36,262,217 bytes**, SHA-256
`01AC1DDFF458B768B0EA5E2585637FB6BDED4F4CE31706F5D8042DF16B245478`.
The bundle checker found the base manifest, 8 ARM64 libraries, 0 other ABIs and
all native ELF LOAD segments aligned to `0x4000`.

The exact APK was installed only on Lava `ST5GDW23LB004392`. Inspected captures in
`C:\Projects\BattleRaja-v1-visual-verify\Builds\V1\Lava\` cover the menu, solo mode,
fighter selection, active Bazaar match, movement and a gadget-placement attempt.
The menu and match show the revised canopy without the prior intersecting-bar shape.
The same run reached match resolution with eight placements and a visible REMATCH/MENU
route, then tapped REMATCH and returned to a fresh eight-actor match. The placement
attempt returned `InvalidPlacement`; this continuation does not claim a new successful
gadget-use capture, so the earlier successful Tiffin evidence below remains the
authoritative gadget route evidence. Device frame/jank and human visual approval remain
open.

## Exact source and automated gates

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Runtime source: `d825832bced4c5e07c7967d891696842eb55609a`
  (`content: make tiffin route player-owned`)
- Unity: `6000.5.6f1` (`0e0577a1a2ac`)
- Disposable verification copy:
  `C:\Projects\BattleRaja-v1-verify-20260824e`
- Repository validation: **0 errors / 0 warnings**
- EditMode: **125/125 passed**
- PlayMode: **66/66 passed** (including the production Tiffin reachability regression)

The current slice keeps the existing render-only fighter motion, pooled impact
feedback and animated gadget identity visuals, and makes the production Tiffin
route player-owned: the pickup is authored near the protected player spawn,
other gadget pickups are kept out of the initial claim radius, the HUD exposes a
nearby-pickup hint, and a PlayMode regression proves the authority pickup/use
path. The beacon is presentation-only; pickup ownership, inventory and use still
resolve through the authority.

## Android artifacts

Both artifacts were built from the exact source above in the disposable copy, so
Unity scene generation and build output did not touch the tracked checkout.

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 40,429,675 bytes | `50FD2D7F9C29F4888F2965810F9FD8130F7C2857F2A15AD7E3A5CF5908E7BFCC` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 36,259,768 bytes | `052F9CAB180E15AEEC0C2D8DCAB47187C53C58F07629C69F81A647697DB9FBF1` |

The AAB check passed: base manifest present, 8 ARM64 libraries, 0 other ABIs,
450 entries, and every checked ARM64 ELF LOAD segment aligned to `0x4000`.
The APK/AAB remains debug-signed with the temporary package ID
`com.example.battleraja.m11`; it is not publishable.

## Lava smoke evidence

Only Lava serial `ST5GDW23LB004392` was used. The APK installed successfully and
`com.unity3d.player.UnityPlayerGameActivity` was top-resumed. Captures and raw
logs are outside the repository under
`C:\Projects\BattleRaja-v1-verify-20260824j\Builds\V1\Lava\`:

- `visual-feedback-menu.png`: offline menu with Play Offline, Tutorial Replay,
  Settings & Accessibility, and Help & Controls.
- `visual-feedback-mode.png`: Solo Raja route.
- `visual-feedback-match-opening.png`: fighter selection route after entering
  the offline mode.
- `visual-feedback-match.png`: Bazaar Bastion opening with eight actors,
  readable HUD, twin-stick controls and distinct fighter identities.
- `visual-feedback-combat.png`: post-warmup interaction probe with attack and
  ability input; no fatal application marker was observed.
- `v1-match-near.png`: match opening with the player HUD showing
  `GADGET [G] tiffin_station` and the authority pickup event.
- `v1-tiffin-used.png`: post-use capture showing `GADGET [G] empty`, the
  cooldown and the visible Tiffin Station near the player.
- `visual-feedback-menu-ui.xml` and `visual-feedback-match-ui.xml`: Unity's
  canvas is exposed as one full-screen surface, so semantic button bounds were
  unavailable; coordinate taps were used only for this bounded smoke probe.
- `visual-feedback-*-logcat.txt`, `visual-feedback-activities.txt`,
  `visual-feedback-meminfo.txt`, and `visual-feedback-gfxinfo.txt`: raw runtime
  evidence and crash-marker/performance inspection.

The sampled device memory was **285,919 KB PSS**, **422,810 KB RSS**, **100,226 KB
Graphics**, and **69 KB swap**. `gfxinfo` did not expose a usable frame/jank
histogram, so no stable-FPS or performance pass is claimed. The captured app
logcat contained no fatal exception, AndroidRuntime, SIGSEGV, SIGABRT,
NullReferenceException or MissingReferenceException marker.

## Reference-app boundary

Brawl Stars (`com.supercell.brawlstars`) was opened read-only on Lava for high-level
UX observation: a clear primary Play action, high-contrast action labels, and
edge-anchored panels/resources. BattleRaja keeps its own shield/bolt identity,
vector fighter silhouettes, toy-bazaar arena and offline-first flow; no Brawl
Stars branding, character, arena, UI art, sound or protected asset was copied.
Smash Karts was not installed on the approved device and was not installed for
this pass.

## Open evidence gap

The production Tiffin pickup/use route is now captured on Lava and the automated
authority regression remains green. Tutorial completion, results/rematch,
accessibility, touch ergonomics, long-run performance, final art/audio, signing,
store/legal and cultural review remain open. The project remains classified as
**prototype**.
