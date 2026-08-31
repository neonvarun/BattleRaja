# BattleRaja V1 execution rebaseline

**Captured:** 2026-08-31 (IST)
**Source checkout:** `codex/v1-playstore-release`
**HEAD:** `3bed64e82be0a84c8bf978d871ae322604b3f7ff`
**Remote:** `origin/main` resolves to the same commit at capture time
**Unity:** `6000.5.6f1` (`0e0577a1a2ac`)
**Approved device:** Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 4 KB pages)

This report records the exact pre-implementation state for the rewritten V1 prompt pack. It is evidence, not a release claim. The current checkout contains protected user-authored prompt/audit changes; no reset, stash mutation or deletion was performed.

## Repository and artifact evidence

- `git status --short --branch`: branch is aligned with `origin/main`; protected prompt rewrite files are deleted/untracked or modified in the working tree.
- `git stash list`: two pre-existing stashes were preserved unchanged.
- `git lfs fsck --pointers`: passed.
- Current temporary candidate: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` (temporary/debug-signed, package `com.example.battleraja.m11`, version `1.0.0`, code `100`).
- APK SHA-256 at rebaseline: `3F12B3D36F5B34DE9288F55C838530A41C584D11122308A2420B0A2F0AA3B059`.
- The AAB is also temporary/debug-signed and remains a preparation artifact, not a publishable package.

## Automated baseline

All commands used the approved Unity editor and the repository validation scripts.

| Gate | Result | Evidence |
|---|---:|---|
| Static validation | 0 errors, 0 warnings | `Tools/Validation/validate.ps1` output captured in the goal run |
| EditMode | 141/141 passed | `Builds/Local/V1GameplayTruth/TestResults/goal-baseline-editmode-20260831.xml` |
| PlayMode | 92/92 passed | `Builds/Local/V1GameplayTruth/TestResults/goal-baseline-playmode-20260831.xml` |
| Deterministic deep soak | 141/141 passed; 1,000-match fixture duration 561.1169086 s | `Builds/Local/V1GameplayTruth/TestResults/goal-baseline-deep-soak-20260831.xml` |
| LFS pointer integrity | passed | `git lfs fsck --pointers` |

The replay/soak fixture is a Solo Raja fixture. It is not evidence that Bastion Crown 4v4 rules exist.

## Lava cold-launch evidence

The device was launched in airplane mode using the existing installed candidate. Fresh captures are under:

`Builds/Local/PlanningAudit/BattleRaja/20260831-baseline/`

Retained files include `cold-launch.png`, `cold-launch-ui.xml`, `cold-launch-logcat.txt`, `package.txt`, `after-play-offline.png`, `after-play-offline-ui.xml`, `after-drop-in.png`, `after-drop-in-ui.xml`, `live-solo.png` and `live-solo-ui.xml`.

- Package identity matched `com.example.battleraja.m11`, version code `100`, target API `36`.
- Airplane mode remained enabled.
- No configured fatal/ANR/SIGSEGV/SIGABRT/UnityException marker was found in the captured logcat.
- The UI hierarchy exposed only Unity's `SurfaceView`; taps were therefore screenshot-derived landscape coordinates rather than UI-tree-derived coordinates. This is a device-input limitation, not a successful accessibility-tree claim.
- The visible primary route is still Solo: `1 RAJA + 7 RIVALS`, `SOLO RAJA`, `ALIVE 8`, no team score/tickets/Crown/shrine/respawn state. This is the binary gap that the next stages must close.

## Current-state conclusion

The source/build foundation is reproducibly green for its existing Solo contract. The intended `BR_BastionCrown_V1` contract is not implemented yet: there is no first-class team identity, Crown Spark objective, team score, ticket pool, respawn state, team result, or squad AI. The product therefore remains **Prototype — 4v4 offline rebuild in progress**. No art, release or human-approval gate is promoted by this rebaseline.

## Stage-01 audit decision

The exact current route, source hazards and evidence limitations are now recorded. Prompt 02 may define the product/mode contracts; authority implementation remains gated on those contracts and tests.
