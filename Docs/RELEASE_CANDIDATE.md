# BattleRaja Closed-Test Release Candidate Checklist

This is a preparation checklist, not a store submission or legal approval.

## Candidate identity

- Unity editor: `6000.5.6f1`
- Candidate label: `M11.0.0` (development/closed-test only)
- Temporary Android application ID: `com.example.battleraja.m11` (not a store identity)
- Android smoke device: Lava `ST5GDW23LB004392` only
- Desktop Web browsers available: Chrome 150 and Edge 150

## Required pre-distribution checks

- Run `Tools/Validation/validate.ps1` and `git lfs fsck`.
- Run the full EditMode and PlayMode suites and archive XML results.
- Install/launch the exact APK on Lava after a clean install and upgrade test.
- Serve the exact Web output over HTTP; check Chrome/Edge bootstrap, refresh and focus.
- Inspect package/permission/secret scans and third-party licence inventory.
- Record APK/Web hashes, build logs, Unity/package lock and source checkpoint.
- Complete human gameplay, cultural, accessibility, privacy and legal review.

## Explicit blockers/approvals

- Photon Fusion package/App ID/account access is required for online cross-play.
- PlayFab title/SDK/account/secret delivery is required for real identity/progression.
- Signing, Google Play submission, public Web hosting, paid services, legal terms and final
  branding require explicit owner approval; none is performed by this checkpoint.

## Rollback/support

- Keep the prior artifact and checkpoint hash; never overwrite a tested artifact in place.
- Roll back by selecting the previous immutable build directory/hash and disabling the
  candidate link. Preserve logs and feedback IDs.
- Collect device model, OS, build label, scene, reproduction steps and sanitized logs; never
  request tokens, passwords or personal data in a bug report.
