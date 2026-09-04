# V1 squad command-window determinism — 2026-09-04

## Scope

This checkpoint hardens the offline Bastion Crown squad blackboard. It is a narrow
authority/replay regression fix; it does not close the broader AI balance, physical 4v4,
performance, authored-art or Play Store gates.

## Defect and fix

`BastionCrownMatch.TryGetSquadIntent` could force-refresh the shared blackboard after a
state revision changed, even while the controller was inside the current bot callback
window. A callback-side mutation could therefore make only later teammates consume a new
plan at the same simulation tick. The fix keeps the snapshot prepared by
`BeginSquadCommandPhase` stable until `EndSquadCommandPhase`; mutations are visible on the
next preparation tick. Pure-domain callers outside a command window retain immediate
refresh behavior.

Source commit: `8e3563a` (`fix: preserve shared squad snapshot during callbacks`).

## Automated evidence

- Static validation: **0 errors / 0 warnings**.
- Focused regression:
  `BattleRaja.Tests.EditMode.BastionSquadBlackboardTests.CommandWindowKeepsOneSharedSnapshotAfterStateMutation`
  passed; XML `Builds/Local/V1GameplayTruth/Next/editmode-squad-window-20260904-rerun.xml`.
- Full EditMode: **161/161 passed**. XML SHA-256
  `8B4DCC3B571FC51AADC646604F5B875398861890E4A84EC2F152C4EE18DF892A`; log SHA-256
  `6CE7D7764BF83F3BDBAE55837BB783D5833423FA4C6DE16CE5AC263541ECF202`.
- Full PlayMode: **98/98 passed**. XML SHA-256
  `B3FE89180E76435A1912733EF00750DD334A2C9770472B1F6C2E9ED72B40BEA5`; log SHA-256
  `FA7CE9CBB79A43A2EF93AD9056F143F71F9E584824FFA039E987C651AA826739`.

The full EditMode suite includes the Bastion replay soak. No replay divergence was
observed in the completed suite.

## Android candidate evidence

The source change was included in fresh release-candidate builds:

- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,680,960 bytes,
  SHA-256 `976EE4D767DC4BC88DB9EB3D499603515D576DF9A205E4E07BF1D87A1CBAA43A`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,506,508 bytes,
  SHA-256 `CE06B7B8C9CA9B67D8AF4796FD6360CEF4430B539BF34F379BC32D9E5F1ECF8F`.
- Build log SHA-256:
  `3F1681CA0F4EF08E1A8A7E9A6701D514371A6726DA08C087C377584691C6241E`.
- Technical release checker: **0 errors / 0 warnings**; log SHA-256
  `188E5C059F9D98752D627B5691D33DFA3BDF4E2B9296EC0F62DC3878D790CB74`.
  Package remains the temporary `com.example.battleraja.m11`, version `1.0.0` / code
  `100`, API `28/36`, offline permissions, seven ARM64 libraries and static 16 KB ELF
  alignment.

## Approved Lava smoke

The exact APK was installed after clearing `com.example.battleraja.m11` on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 1080x2460, reported 4 KB pages).
The pulled installed base is 41,680,960 bytes and matches the APK SHA-256 above. A fresh
menu capture is `Builds/Local/V1GameplayTruth/Next/squad-window-20260904/00-menu.png`
(SHA-256 `9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`). The
process-scoped logcat is `postfix-logcat.txt` (17,837 bytes, SHA-256
`55B05C6DFE0685B2339BC7D4CFC0BDB423427193B3C9F07D507C09E10B5210A6`) with zero
configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or `SIGABRT` markers.

This is an install/menu/crash-marker smoke for the refreshed artifact, not proof of a
complete physical Bastion 4v4 route, normalized FPS/GPU/GC, endurance, genuine 16 KB
runtime, touch comfort, final authored art/audio, cultural review or owner approval.
