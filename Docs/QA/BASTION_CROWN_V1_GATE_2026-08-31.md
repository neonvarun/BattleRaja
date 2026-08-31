# Bastion Crown V1 implementation gate — 2026-08-31

## Summary

The player-facing offline route now exercises the original Bastion Crown 4v4
contract: actor 1 human plus actors 2–4 allied AI (Team Raja) versus actors 5–8
Rival AI. Solo Raja remains explicit in the pure domain/application fixtures. No
multiplayer, account, economy, Photon or PlayFab work was added.

## Source and scene

- Unity: `6000.5.6f1` (`0e0577a1a2ac`), URP `17.5.0`, Input System `1.20.0`.
- Production scene: `Assets/BattleRaja/Scenes/Gameplay/BazaarBastion.unity`.
- Controlled scene regeneration: `BattleRaja.Editor.BuildEntrypoints.CreateBazaarBastionScene`.
- Canonical identities: actor IDs 1–8; Raja 1–4; Rival 5–8.
- Objective presentation: three Crown socket rings, Raja/Rival shrine rings and
  a pulsing carrier/drop Crown visual, all collider-free and render-only.

## Automated evidence

| Gate | Result | Evidence |
|---|---:|---|
| Static repository validation | 0 errors / 0 warnings | `Tools/Validation/validate.ps1` |
| EditMode | 148/148 passed | `Builds/Local/V1GameplayTruth/TestResults/bastion-objective-editmode-20260831.xml` |
| PlayMode | 94/94 passed | `Builds/Local/V1GameplayTruth/TestResults/bastion-objective-playmode-20260831-rerun.xml` |
| Pure Bastion rules | 7 regressions included in EditMode | `Assets/BattleRaja/Tests/EditMode/BastionCrownMatchTests.cs` |
| Production objective loop | team composition, markers/HUD, pickup, slowdown and deposit covered | `Assets/BattleRaja/Tests/PlayMode/VerticalSlicePlayModeTests.cs` |
| Android V1 technical gate | passed; offline manifest, ARM64-only, static 16 KB alignment, store dimensions | `Tools/Validation/check_v1_release_candidate.ps1` |

## Android artifact

- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`
- Size: 41,438,372 bytes
- SHA-256: `6EDC5C0E5D304529A6059A94F00F7AB32AB9C71A4464044D3B0D3ED5D3E2C507`
- Package: `com.example.battleraja.m11` (temporary/debug identity)
- Version: `1.0.0` / code 100; min API 28; target API 36; ARM64
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, rebuilt from
  the same checkpoint: 37,263,881 bytes, SHA-256
  `3D12A358E0F9159A2CA3749A4E53DBB712AF19B0FCCC6E6D80F96DE5944508EE`.
- Technical package checks: APK manifest/package/version/API and offline
  permissions passed; AAB contains seven ARM64 native libraries, no other ABI,
  and every inspected ELF LOAD segment is 16 KB aligned. The app icon (512x512)
  and feature graphic (1024x500) dimension checks passed.

## Android runtime observation

The APK was installed and launched on the local `BattleRaja_16K` Android 16
AVD (`emulator-5556`) using the ANGLE renderer. Captures are under
`Builds/Local/V1GameplayTruth/AndroidQA/emulator-5556/`:

- `menu-rerun.png`: branded BattleRaja menu with `PLAY OFFLINE`.
- `mode-angle.png`: Bastion Crown 4v4 briefing and shared-ticket copy.
- `fighter-angle.png`: isolated Bijli, Pehel and Maya production portraits.
- `gameplay-angle.png`: live team HUD, 8 fighters, Bazaar arena and controls.
- `gameplay-angle-after7s.png`: live `RIVAL CARRIER` state at the Crown objective.
- `gameplay-app-logcat.txt`: app log; no fatal, ANR, SIGSEGV or SIGABRT marker.

The default host renderer crashed the AVD during the fighter-screen transition;
the ANGLE rerun completed the route. This is an emulator configuration limitation,
not a production-device crash diagnosis. The emulator is x86_64 and is not a
substitute for ARM64 Lava evidence.

## Unverified / owner-only gates

- Lava `ST5GDW23LB004392` was not connected during this run; no fresh physical
  screenshot, sustained performance, thermal, battery or 16 KB claim is made.
- The installed Brawl Stars and Smash Karts packages could not be opened because
  the approved phone was unavailable; no reference-game asset, terminology or
  trade dress was copied.
- Final authored art/audio/cultural/fun review, permanent package identity,
  release signing, privacy/Data Safety, store listing and Play Console submission
  remain owner-gated.
