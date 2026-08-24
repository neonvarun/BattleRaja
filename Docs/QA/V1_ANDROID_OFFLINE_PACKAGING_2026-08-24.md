# V1 offline Android packaging evidence — 2026-08-24

This note records the exact current checkout after the offline packaging hardening.
It is technical release-candidate evidence, not Play Store approval or final human
visual/performance sign-off.

## Latest Android runtime slice — `6920edd` — 2026-08-24

The latest V1 release source is `6920edd` (`android: brand V1 release splash`) on
`codex/v1-playstore-release`. The release-shaped APK was built in the disposable
worktree `C:\Projects\BattleRaja-validate-6920edd`, is **39,466,531 bytes**, and has
SHA-256 `8EE99741122A548F4B893F015F9656D30B343E5056BE7E409482A37D6D6D6383`.
The build uses Unity `6000.5.6f1`, ARM64, IL2CPP, min API 28 and target/compile API
36. The release entrypoint writes a BattleRaja-owned splash logo and disables the
Unity logo; the disposable build's `ProjectSettings.asset` records the icon logo,
dark background and two-second logo duration.

The exact APK was installed only on Lava `ST5GDW23LB004392`. Cold-launch captures
and logcat are outside the repository at
`C:\Users\USER\AppData\Local\Temp\battleraja-splash-6920edd\`; the log contains no
fatal/ANR/SIGSEGV marker. The most recent AAB remains the earlier `6ac5c12` artifact
until a new bundle build is run.

## Latest Android runtime slice — `b954a72` — 2026-08-24

The latest Android runtime source is `b954a72` (`ui: reflow match touch controls on
rotation`) on `codex/v1-playstore-release`. Repository validation is **0/0**, EditMode
is **125/125**, and PlayMode is **69/69**. Its exact release-shaped APK is
**39,525,752 bytes** with SHA-256
`3ABCEF91BF14239AD8D6ED5511D7C74D2C0DA3DB3CC35DCE838573AEB39E1630`, built in
`C:\Projects\BattleRaja-validate-b954a72` and installed only on Lava
`ST5GDW23LB004392`. This slice has no rebuilt AAB; the AAB table in the historical
`6ac5c12` section remains the most recent bundle evidence. The latest menu/match
captures and launch log are recorded in
`C:\Users\USER\AppData\Local\Temp\battleraja-control-rotation-b954a72\`.

The Web artifact below remains attributed to `6ac5c12`/`7751f53` historical source
and was not rebuilt for this Android-only presentation change.

## Latest exact-current slice — `6ac5c12` — 2026-08-24

The current source commit is `6ac5c12` (`ui: bind runtime input actions explicitly`)
on `codex/v1-playstore-release`. It keeps the offline-only Android boundary and
explicitly binds the Input System point/click/navigation actions at runtime, with a
PlayMode regression covering those bindings. Repository validation is **0 errors /
0 warnings**, EditMode is **125/125**, and PlayMode is **67/67**. The release-shaped
artifacts were built in the disposable worktree `C:\Projects\BattleRaja-validate-6ac5c12`
so generated scenes and Burst output did not touch the tracked checkout.

| Artifact | Size | SHA-256 | Evidence |
| --- | ---: | --- | --- |
| Release-shaped APK | 39,523,632 bytes | `09F5375FA8D5DEC066A09D8CCDF0BAF01269F4B402252EF2908691C773402EF3` | `C:\Projects\BattleRaja-validate-6ac5c12\Builds\V1\Android\BattleRaja-V1.0-release-candidate.apk` |
| Release-shaped AAB | 35,351,357 bytes | `70825F82A4D79E1E036F4DA8A286778244406D51B1D60A568BD066ED1B82DAA8` | `C:\Projects\BattleRaja-validate-6ac5c12\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab` |

The AAB check passed with 7 ARM64 libraries, no other ABIs, and `0x4000` ELF
LOAD alignment. The APK was installed only on Lava serial `ST5GDW23LB004392`
and launched into the branded menu; the inspected release-shaped capture has no
development-build watermark and no fatal/ANR/SIGSEGV marker. The log still records
Unity's known nonfatal optional Play Core `AssetPackManager` class-probe exception.
Raw files are in `C:\Users\USER\AppData\Local\Temp\battleraja-lava-6ac5c12\`.

The same exact source produced a 19-file Web build totalling **132,071,712 bytes**;
the WASM is **119,799,965 bytes** with SHA-256
`8CE68A5AA4C741DD27AD66B9BF61FBC0B17DE9F632F2C791181EC99F516DEA12`. It was
served over local HTTP on `127.0.0.1:8082`; Chrome and Edge both returned `200` for
the loader resources and produced screenshots, but stayed on the Unity loader during
the bounded headless wait. This is not full interactive Web approval.

## Historical exact source and packaging gates — `6ac5c12`

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Final documentation checkout: `HEAD` on `codex/v1-playstore-release`
- Runtime/package source: `6ac5c12` (the final checkout adds documentation only)
- Unity: `6000.5.6f1`
- Repository validation: **0 errors / 0 warnings**
- Full EditMode: **125/125 passed** (`Builds/V1/TestResults/editmode-touch-config.xml`)
- Full PlayMode: **67/67 passed** (`Builds/V1/TestResults/playmode-touch-config.xml`)

## Android artifacts

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 39,523,632 bytes | `09F5375FA8D5DEC066A09D8CCDF0BAF01269F4B402252EF2908691C773402EF3` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 35,351,357 bytes | `70825F82A4D79E1E036F4DA8A286778244406D51B1D60A568BD066ED1B82DAA8` |

The AAB contains 7 ARM64 native libraries, no other ABIs, and passed the static
16 KB ELF LOAD-segment alignment check. The APK package is
`com.example.battleraja.m11`, version `1.0.0`/code `100`, target API 36, label
`BattleRaja`, and launches `UnityPlayerGameActivity`.

## Offline permission boundary

The release APK was inspected with `aapt dump permissions` and the installed package
was checked with `dumpsys package`. It contains `VIBRATE` and Unity's dynamic-receiver
permission only; `INTERNET` and `ACCESS_NETWORK_STATE` are absent. Fusion remains in
the repository for a future approved online milestone, but its Android runtime
assemblies/sockets are disabled for this offline candidate and `Fusion.Unity` is
Editor-only. The custom manifest and importer metadata are packaging boundaries, not
an online integration.

Unity still emits a build diagnostic saying it attempted to add the removed network
permissions, and the legacy icon path emits a deprecation/quality warning. The final
APK inspection is authoritative for the shipped permission set; final signed-bundle
inspection remains required before Play submission.

## Lava validation

Only Lava serial `ST5GDW23LB004392` was used. The exact APK installed successfully,
launched into the branded offline menu, and left
`com.unity3d.player.UnityPlayerGameActivity` focused in the launch capture. The
post-launch log sample contains no `FATAL EXCEPTION`, `ANR`, or `SIGSEGV`; it does
contain a nonfatal optional Play Core `AssetPackManager` `ClassNotFoundException`
from Unity's asset-pack probe. Raw files
are outside the repository at
`C:\Users\USER\AppData\Local\Temp\battleraja-root-offline-manifest-lava\`.

The inspected portrait menu shows the original BattleRaja/Bazaar Bastion identity,
Play Offline, Tutorial Replay, Settings & Accessibility, and Help & Controls. Brawl
Stars was viewed read-only on the same phone as a high-level reference for menu
hierarchy and readability; no art, branding, copy, or layout was copied. Smash Karts
was not installed on the device.

## Web build smoke

The exact checkout produced a successful Web build at
`Builds/M11/Web-BazaarBastion`. The current output contains 19 files totalling
approximately 132.1 MB; the WASM is 119,799,965 bytes with SHA-256
`8CE68A5AA4C741DD27AD66B9BF61FBC0B17DE9F632F2C791181EC99F516DEA12`.
It was served over local HTTP at `127.0.0.1:8082`; `curl -I index.html` returned
`HTTP/1.0 200 OK`. Chrome and Edge headless both reached the Unity loader and
produced screenshots in `C:\Users\USER\AppData\Local\Temp\battleraja-web-6ac5c12\`.
This is a build/HTTP/loader smoke result, not a complete interactive browser route
or multi-browser approval; the headless environment did not advance beyond the
loader during the bounded capture.

## Remaining gates

The project remains **prototype**. Final application ID, release signing, Play policy
review, adaptive-icon cleanup, sustained Lava performance/thermal/battery evidence,
touch/accessibility review, full interactive Web review, privacy/legal/store approval,
and owner approval for any online service remain open. Photon and PlayFab were not
started by this packaging change.
