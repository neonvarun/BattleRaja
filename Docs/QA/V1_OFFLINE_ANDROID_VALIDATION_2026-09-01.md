# BattleRaja V1 offline Android validation — 2026-09-01

This is the final local validation record for the current V1 working tree. It is a
technical release-candidate report, not a Play submission or an approval of branding,
cultural fit, accessibility comfort, balance, signing or legal declarations.

## Scope and source

- Branch: `codex/v1-playstore-release`.
- Product scope: offline Android Bastion Crown only; one human + three allied AI versus
  four rival AI. Photon, PlayFab, accounts, matchmaking, ads, IAP, cloud progression,
  online leaderboards and Web release work remain excluded.
- Unity: `6000.5.6f1`, URP, Android target SDK 36.
- Approved physical device: Lava `ST5GDW23LB004392` / `LAVA_LXX508`, Android 14/API 34.
  `getconf PAGE_SIZE` returned `4096`; this is not physical 16 KB runtime evidence.
- The runtime menu now references
  `Assets/BattleRaja/Art/V1/BattleRaja-FeatureArt-OriginalCandidate.png`, an original
  Bazaar Bastion shrine/fighter composition with no vehicles, karts, racing track, copied
  characters, logos or text. The prior feature-art file is retained as historical material
  but is not referenced by the runtime.

## Automated gates

| Gate | Command / evidence | Result |
| --- | --- | --- |
| Static repository validation | `Tools/Validation/validate.ps1` | **0 errors / 0 warnings** |
| EditMode | `Tools/Validation/run_unity_tests.ps1 ... -TestPlatform editmode` with `BATTLERAJA_BASTION_SOAK_MATCHES=2` and `BATTLERAJA_BASTION_SOAK_TICKS=8400`; `Builds/Local/V1GameplayTruth/Final/editmode.xml` | **155/155 passed** |
| PlayMode | `Tools/Validation/run_unity_tests.ps1 ... -TestPlatform playmode`; `Builds/Local/V1GameplayTruth/Final/playmode.xml` | **94/94 passed** |
| Bastion replay soak | `BastionReplaySoakTests.BastionReplay_ReproducesEightActorCombinedHashStream`; two seeds, 8,400 ticks each, serialized v2 replay re-execution | **0 divergences** |
| Squad planner coverage | `BastionCrownMatchTests.SquadPlannerMetricsCoverObjectiveEscortDefenseCollapseAndRetreat`; 32 deterministic seeds | **contest 64, escort 64, defend 96, collapse 64, Aandhi-retreat 32** |
| Diff hygiene | `git diff --check` | Passed |

The replay digest is captured at a coherent post-tick boundary after legacy combat,
healing/collection, Bastion mirroring, objective advancement and respawn. The digest covers
objective, team/ticket/stat state, participant protection/respawn state, sorted contribution
maps, event-ledger counts and Aandhi context. It does not prove network parity or human fun.

## Android artifacts and technical checker

Built with:

```text
Tools/Build/Android/build.ps1 -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidV1ReleaseCandidate
Tools/Build/Android/build.ps1 -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidV1ReleaseCandidateApk
Tools/Validation/check_v1_release_candidate.ps1 -ExpectedPackageId com.example.battleraja.m11
```

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 41,510,440 | `5F7438105FE450D6331CFEDEE1FAEEB87FB4F6677EB811A997A02CC8FD7C4AE9` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 37,335,957 | `87C835570B62C4C3A79C156F94CB7E15C6AD31FCB50A0E8ADB0FDE6672DC4858` |

The checker passed with package `com.example.battleraja.m11`, version `1.0.0`/100,
min SDK 28, target SDK 36, no network permissions, ARM64-only native libraries, static
16 KB ELF/zip alignment, and valid 512×512 icon / 1024×500 feature-graphic dimensions.
The package remains temporary/debug identity and is not signed for publication.

## Lava evidence

The APK was uninstalled, freshly installed and launched on the approved Lava device. The
top-resumed activity was `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`.
The bounded route captured:

- cold launch menu with the new Bazaar Bastion art;
- `PLAY OFFLINE` → Bastion Crown briefing → fighter choice;
- live opening arena with Crown/ticket HUD, four-v-four roster, objective shrine and touch
  controls;
- ability/attack/gadget input taps;
- in-match settings overlay (left-handed, reduced flashes, high contrast, aim assist and
  text controls) and return to live play.

Screenshots and raw activity/package/UI captures are under
`Builds/Local/V1GameplayTruth/Final/lava-20260901-final/`, including:

- `01-launch.png` (SHA-256 `9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`)
- `04-live-opening.png` (SHA-256 `0F5CD45AE2D38067605F77D2B2788694BEA99C4CA97B7E828078A31593719D96`)
- `08-pause-settings.png` (SHA-256 `23D505AAEF713C1F1428D68EF48960CEE6BE8E37150A38EF390D5B6137B624C9`)
- package/activity/UI XML and device property files.

The Unity surface is exposed as one native `SurfaceView`, so Android UIAutomator cannot
see individual Unity buttons; the touch coordinates were derived from the rendered capture.
Results/rematch and action-by-action tutorial completion were not re-run on this exact APK
in this bounded continuation; automated PlayMode and prior exact-candidate route evidence
remain separate.

## Bounded performance and crash scan

`Tools/Validation/capture_android_performance.ps1` ran against the live exact APK for 30
seconds with six five-second samples after clearing the device log buffer. Raw evidence is
under `Builds/Local/V1GameplayTruth/Final/lava-20260901-final/performance-30s-clean/` and the
manifest SHA-256 is `EF488B63F8A11405B0B73E2A625FBFEB8C0DCEDD282D504FDB35766BDE694C02`.

- PSS: **287,530–293,678 KB**; RSS: **426,940–433,088 KB**.
- Graphics PSS: **87,024–93,180 KB**.
- Instantaneous process `top` CPU: **111–118%** on Android's 100%-per-core scale.
- Thermal status: **0**; USB-powered battery stayed **98%**, 4,279→4,280 mV, 31.0 C.
- Configured app crash markers: **0** in the clean app-scoped logcat.
- Unity `gfxinfo` exposed no usable frame histogram; no normalized FPS/GC/GPU budget claim
  is made. This is bounded raw telemetry, not sustained endurance approval.

## Truthful classification and remaining owner gates

**Prototype — Android offline release candidate in progress.**

The source, deterministic rules/replay hardening, coordinated planner states, technical
Android package checks and bounded Lava launch/live evidence are green. The following remain
open and owner-controlled: final authored 3D model/rig/animation/audio polish and cultural
review; complete all-fighter/gadget/Aandhi/tutorial/results/rematch physical comfort review;
normalized sustained performance, unplugged battery and thermal endurance; physical ARM64
16 KB runtime coverage; permanent package/publisher identity and release signing; privacy
policy, Data Safety, IARC/content rating, support/rollback copy and any Play Console action.
No public upload, rollout, legal acceptance or paid service was performed.
