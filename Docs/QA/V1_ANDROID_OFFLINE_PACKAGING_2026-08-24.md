# V1 offline Android packaging evidence — 2026-08-24

This note records the exact current checkout after the offline packaging hardening.
It is technical release-candidate evidence, not Play Store approval or final human
visual/performance sign-off.

## Latest exact-current slice — `7b13f8f` — 2026-08-24

The current source commit is `7b13f8f` (`ui: remove internal candidate status from
offline menu`) on `codex/v1-playstore-release`. It keeps the offline-only Android
boundary and changes only player-facing menu copy plus its PlayMode regression.
Repository validation is **0 errors / 0 warnings**, EditMode is **125/125**, and
PlayMode is **67/67**. The release-shaped artifacts were built in the disposable
worktree `C:\Projects\BattleRaja-validate-7b13f8f` so generated scenes and Burst
output did not touch the tracked checkout.

| Artifact | Size | SHA-256 | Evidence |
| --- | ---: | --- | --- |
| Release-shaped APK | 39,524,160 bytes | `B55EF5268E19D7B84FAB14E072769431CD7F20F6A1AFD99009A3C5E1DF1CB733` | `C:\Projects\BattleRaja-validate-7b13f8f\Builds\V1\Android\BattleRaja-V1.0-release-candidate.apk` |
| Release-shaped AAB | 35,351,879 bytes | `B84EA775120DA203C6F70D522D316F59224FB7FAF6F5A889EFE13E5C089FDC0F` | `C:\Projects\BattleRaja-validate-7b13f8f\Builds\V1\Android\BattleRaja-V1.0-release-candidate.aab` |

The AAB check passed with 7 ARM64 libraries, no other ABIs, and `0x4000` ELF
LOAD alignment. The APK was installed only on Lava serial `ST5GDW23LB004392`
and launched into the branded menu; the inspected release-shaped capture has no
development-build watermark and no fatal/ANR/SIGSEGV marker. Raw files are in
`C:\Users\USER\AppData\Local\Temp\battleraja-lava-7b13f8f\`.

The same exact source produced a 19-file Web build totalling **132,071,670 bytes**;
the WASM is **119,799,945 bytes** with SHA-256
`53354CCE0B394FD0CEC7F9C3553B6BF8CDBB0E18A1F96F03274BA38104B6EECF`. It was
served over local HTTP on `127.0.0.1:8081`; Chrome and Edge both returned `200` for
the loader resources and produced screenshots, but stayed on the Unity loader during
the bounded headless wait. This is not full interactive Web approval.

## Exact source and automated gates

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Final documentation checkout: `HEAD` on `codex/v1-playstore-release`
- Runtime/package source: `f4425d6` (the final checkout adds documentation only)
- Unity: `6000.5.6f1`
- Repository validation: **0 errors / 0 warnings**
- Full EditMode: **125/125 passed** (`Builds/V1/TestResults/editmode-exact-current.xml`)
- Full PlayMode: **66/66 passed** (`Builds/V1/TestResults/playmode-exact-current.xml`)

## Android artifacts

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 39,529,326 bytes | `AE74717B597C4CBCFDECF7D8DB719C177100F495CC084ABFD0E1EA6AAD3E2C52` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 35,357,477 bytes | `8EB49EFC8D58D144E5A792224FC9A3570FF4E37F121E06B6E55093C9D4D5F5E7` |

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
approximately 132.1 MB; the WASM is 119,799,945 bytes with SHA-256
`05EF2D0A69EE3E6DD8B7552913E892D749266135F216F17061560FAFDA8BD09F`.
It was served over local HTTP at `127.0.0.1:8080`; `curl -I index.html` returned
`HTTP/1.0 200 OK`. Edge headless reached the Unity loader and produced a screenshot.
This is a build/HTTP/loader smoke result, not a complete interactive browser route
or multi-browser approval; the headless environment did not advance beyond the
loader during the bounded capture.

## Remaining gates

The project remains **prototype**. Final application ID, release signing, Play policy
review, adaptive-icon cleanup, sustained Lava performance/thermal/battery evidence,
touch/accessibility review, full interactive Web review, privacy/legal/store approval,
and owner approval for any online service remain open. Photon and PlayFab were not
started by this packaging change.
