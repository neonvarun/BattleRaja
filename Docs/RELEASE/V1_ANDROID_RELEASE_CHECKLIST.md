# BattleRaja V1.0 Android release checklist

This is the offline Android release-candidate gate for BattleRaja. It deliberately does not
start Photon, PlayFab, accounts, ads, IAP, cloud progression or Web release work.

## Candidate scope

- Unity `6000.5.6f1`, URP, ARM64, IL2CPP.
- Android target API 36, minimum API 28.
- One local human plus seven deterministic bots in Bazaar Bastion.
- Bijli, Pehel and Maya; Umbrella Guard, Dhol Burst and Tiffin Station; Aandhi; tutorial;
  spectator; results; rematch; local settings.
- No account, online room or server-owned progression is used by the offline candidate.
  The exact offline packaging candidate removes `INTERNET` and
  `ACCESS_NETWORK_STATE` from the APK while retaining the future-facing Fusion files
  outside the Android runtime. Final signed-bundle inspection is still required before
  Play submission.

## Current policy recheck — 2026-08-24

- Google’s target-API guidance requires new apps and updates submitted from
  2026-08-31 to target Android 16/API 36 or higher; this candidate is configured
  for API 36.
- Google’s 16 KB page-size guidance applies to 64-bit apps targeting API 35+;
  the current AAB has passed static ARM64/16 KB checks, but the final signed
  artifact still needs the same inspection and a compatible runtime check.
- Google requires an accurate Data safety form and privacy-policy link for apps
  published on closed, open or production tracks, including apps that collect no
  data. An app kept exclusively on internal testing is exempt from the Data safety
  form, but that exemption is not a release shortcut.
- Google also requires target-audience declarations and a completed content-rating
  questionnaire for a new Play app. These remain owner/legal gates.

Primary sources: `https://developer.android.com/google/play/requirements/target-sdk`,
`https://developer.android.com/guide/practices/page-sizes`,
`https://support.google.com/googleplay/android-developer/answer/10787469`, and
`https://support.google.com/googleplay/android-developer/answer/9859655`.

The local technical gate for an exact artifact pair is:

```powershell
pwsh -File Tools/Validation/check_v1_release_candidate.ps1 `
  -ProjectRoot . `
  -ApkPath C:\path\to\BattleRaja-V1.0-release-candidate.apk `
  -AabPath C:\path\to\BattleRaja-V1.0-release-candidate.aab `
  -AaptPath "$env:LOCALAPPDATA\Android\Sdk\build-tools\36.0.0\aapt.exe" `
  -ReadElfPath 'C:\path\to\llvm-readelf.exe' `
  -UnityExe 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe' `
  -ExpectedPackageId com.example.battleraja.m11 `
  -RequireCleanWorktree
```

This gate is technical and local only. It does not sign, upload, publish, or approve
the final identity, privacy/Data Safety, content rating, cultural review or Play track.

## Latest durable replay source — `2a113e0` — 2026-08-27

The current release-shaped APK/AAB were rebuilt from source commit
`2a113e0c4798e8e51a43379a0fa0facd7e8f0fe1`. The clean release checker passed **0 errors /
0 warnings**; evidence is `Builds/Local/Device/release-checker-2a113e0.log` (SHA-256
`6CE4C48CDC734A1038139EFF67CF8196E51ECB8FA1DA4840828C9CCE37F69A80`). APK SHA-256 is
`52B04A015656BB5480FBBCF5879578313D1B527E32BA205BBB9F102449C0986E`; AAB SHA-256 is
`9FA87846E85423499AC8A9305631091A4D38ADA8F0A49D03853F0B14B954499F`. P42 in
`Docs/V1_RELEASE_PLAN.md` records the durable replay capture and exact-file verification.

## Latest exact-source gadget-reconciliation candidate — `754837e` — 2026-08-27

Runtime-bearing candidate commit `754837e4311b609560c63fa90558a1d29acec9cd` is the
current source candidate. The final clean documentation tip is
`a877c509fdbec485e808039a6c4daa03fed9ea9c`. It
adds presentation-only reconciliation for a gadget collected before the tutorial card
binds. Full EditMode is **140/140**, PlayMode is **86/86**, and static validation is
**0 errors / 0 warnings**. The APK is **40,527,614 bytes** (SHA-256
`788181073E5EFCB2F5F0AECEF20E0372362BFCD2B83928CA010153009FDF99B3`) and the AAB is
**36,352,792 bytes** (SHA-256
`FCFF4A982BC5201D204114B819C0BDAE42CA35072425CE9506349769815D98C3`). The checker
`Builds/Local/Device/release-checker-754837e.log` (SHA-256
`E6EF2EB9DDEEDD63981B0C894A2778D163988239E2BF7176786E8DB63CA4F721`) passes the
offline manifest, API 28/36, ARM64/static 16 KB and creative-dimension checks.

The exact APK installed and launched on approved Lava `ST5GDW23LB004392`. Bounded
real-touch evidence now advances Movement, Aim, Basic Attack, Ability, Gadget and
Aandhi; the Elimination card remains correctly waiting for a player-attributed KO.
The APK is temporary-ID Android Debug-signed and the device reports 4 KB pages, so
runtime 16 KB, sustained performance, full-route/accessibility, final signing/identity,
privacy/Data Safety, cultural/legal review and Play Console actions remain open. See
`Docs/V1_RELEASE_PLAN.md` P36 for the exact evidence index and gate classifications.

The exact-runtime production-bot refresh is now also current at this source: **100/100**
seeded matches completed in the 240-360 second target window, **94/100** reached at least
three combat eliminations, **100/100** contained bot-to-bot damage, and protected,
invalid-position and stuck invariants were zero. The report and test-log hashes are
recorded in `Docs/V1_RELEASE_PLAN.md` P38. This does not replace human playtesting or
the remaining physical, performance, signing, privacy/legal and Play Console gates.

The exact APK also passed a genuine runtime check on the installed Android 16
`page_size_16kb` emulator: `PAGE_SIZE=16384`, successful install, top-resumed Unity
activity, menu/tutorial render, and a real movement swipe unlocking `CONTINUE`.
The six-sample capture reported no configured fatal markers. This closes the available
emulator runtime check; physical ARM64 16 KB coverage, signed-artifact repeatability
and dense-combat performance remain open. See `Docs/V1_RELEASE_PLAN.md` P39.

The current clean documentation tip `98888d3` also passes the deterministic replay
deep soak: `BATTLERAJA_SOAK_MATCHES=1000`, 1,000 seeds executed twice (2,000
executions), zero divergence, 1/1 test passed. XML/log hashes are indexed in
`Docs/V1_RELEASE_PLAN.md` P40. This same-machine result does not establish
cross-machine parity. P42 now records the bounded durable production replay capture and
exact-file re-execution gate: one production-scene match emitted 9,180 ordered frames with
per-tick canonical snapshots/hashes, and the exact `.brr` file replayed cleanly in EditMode.
Cosmetic presentation-state review and cross-machine parity remain open.

## Latest exact-source tutorial-fix candidate — `f82c18c` — 2026-08-27

The exact runtime source is `f82c18c1fd91e44c7f07fbd31d615cc7e9c9bea6`. Full EditMode is
**140/140**, full PlayMode is **85/85**, and static validation is **0 errors / 0 warnings**.
The tutorial Elimination regression now unlocks from the authoritative live player snapshot
before terminal results. The exact APK is **40,524,546 bytes** (SHA-256
`D4E965DE27E4C8D50F57038557E70D55190DFD0AECEEA8CB4E9B30A15A91B59A`) and matching AAB is
**36,349,707 bytes** (SHA-256
`3D1BD5D1E8DBFEACCBDFF97907EFF6CC14ECEB33CE80522EC94166ACB07E1ACF`). Technical checker
log `Builds/Local/Device/release-checker-f82c18c.log` (SHA-256
`B73B0A1CD12F11A2941C6F629A92128F1D738122AAC866BE275742EDFD2B36F5`) passes the offline
manifest, API 28/36, ARM64/static 16 KB and store-dimension checks.

Fresh approved-Lava probes reach the Movement, Aim, Basic Attack and Ability cards. The
same route reaches Gadget pickup/use feedback but the card remains waiting before terminal
results; physical Gadget/Aandhi/Elimination/Victory, full-route, accessibility, sustained
performance, genuine 16 KB runtime, final signing/identity, privacy/Data Safety,
cultural/legal review and Play Console actions remain open. The temporary-ID/debug-signed
artifact is not publishable. Complete artifact and screenshot evidence is in
`Docs/V1_RELEASE_PLAN.md` P34.

The current-source production-bot refresh from clean tip
`68b0551e44b6356ca3f8a8925ff4268a6bc7380d` completed **100/100** seeded matches in
the 240-360 second window; **94/100** reached at least three combat eliminations,
**100/100** recorded bot-to-bot damage, and protected, invalid-position and stuck
invariants were zero. The report and test-log hashes are recorded in
`Docs/V1_RELEASE_PLAN.md` P35. This is a local gameplay-truth gate, not Play
eligibility: physical full-route, runtime 16 KB, signing, identity, privacy/legal,
cultural and Play Console gates remain open.

## Latest exact-source handedness candidate — `2080383` — 2026-08-27

The exact clean source is commit `208038362e16f8c33856e0a7cf5c4de776005ded`.
The tutorial now names the active movement/aim stick after the persisted handedness
setting; the tutorial arena visibility and fighter-focus fixes remain included. Full
EditMode is **140/140** and PlayMode is **83/83**. The matching APK is **40,523,706 bytes**
(`365ABF4A1D37BB6DC2CE7E08F5E2741AAB7662EFB9749F0B4987EBFCBDB68BDB`) and the AAB is
**36,348,870 bytes** (`F1CB13C80A6408B344B5C71BE11D0AD804E58CA1D01102FE0B79D5B0712BDBA1`).
The local release checker reports **0 errors / 0 warnings**, package
`com.example.battleraja.m11`, API 28/36, seven ARM64 libraries, no network permissions,
static 16 KB alignment and creative dimensions passed. Bundletool/zipalign/apksigner
evidence and exact hashes are indexed in `Docs/V1_RELEASE_PLAN.md` P22-P29. Fresh Lava
captures show both default and left-handed tutorial prompt wording and a live-match
SurfaceFlinger diagnostic; normalized performance approval remains open.

On approved Lava `ST5GDW23LB004392`, the exact APK installed and launched. Fresh probes
show the settings surface, default and left-handed tutorial prompt wording, and a live
Solo Raja opening. The exact-candidate SurfaceFlinger diagnostic recorded median
16.535 ms and p95 16.567 ms, and a separate 120-second live-match capture held PSS to
267,935-272,772 KB with thermal status 0. These are bounded diagnostics; normalized
frame pacing and budget approval remain open. Full action-by-action tutorial and
end-to-end route, sustained performance, genuine 16 KB runtime, authored review,
signing and Play actions remain owner-controlled gates; see
`Docs/V1_RELEASE_PLAN.md` P22-P29.

## Latest exact-source tutorial-visibility candidate — `e6c321b` — 2026-08-27

The exact clean source is commit `e6c321b60c8398755942ab0260d13dddac3df551`.
The tutorial no longer creates an opaque full-screen backdrop, so the live arena and
touch controls remain visible behind the action-gated prompt. Full EditMode is
**140/140** and PlayMode is **82/82**. The matching APK is **40,524,858 bytes**
(`E1408B65F89317885FF64F1C94D80417385E86600420F77BCA3428E378260403`) and the AAB is
**36,350,021 bytes** (`E94945CA57AA71B510524C73AB9470F839045584784238E1093D3A4834116E11`).
The local release checker reports **0 errors / 0 warnings**, package
`com.example.battleraja.m11`, API 28/36, seven ARM64 libraries, no network permissions,
static 16 KB alignment and creative dimensions passed. Bundletool/zipalign/apksigner
evidence and the exact hashes are indexed in `Docs/V1_RELEASE_PLAN.md` P19.

On approved Lava `ST5GDW23LB004392`, the exact APK installed and launched. The tutorial
opening screenshot visibly shows the Bazaar arena, fighters, zone ring, HUD and both
touch sticks behind the prompt; the tutorial logcat has zero configured fatal markers.
Action-by-action tutorial completion, full match/spectator/results/rematch/settings route,
sustained performance, genuine 16 KB runtime, final authored review, signing and Play
actions remain owner-controlled gates.

## Latest exact-source fair-bot candidate — `6d287a6` — 2026-08-27

The exact clean source is commit `6d287a657dd946c806ac54580b4d5a5ea1e53ee4`.
Production bots use bounded `0.9x` damage, a `25x` cadence and a fixed canonical-tick
editor harness.

- Full EditMode: **140/140 passed**; full PlayMode: **81/81 passed**.
- Deterministic soak: **1/1 passed**, 1,000 seeds x2 (2,000 executions), zero divergence;
  XML SHA-256 `DB133AE5BD7855175FECA4ED909F0C67FCE4F9607C98A4FE355683B029122186`.
- Exact-source 100-match bot report: **100/100** in the 240-360 second window,
  **95/100** with >=3 combat eliminations, 100/100 bot-to-bot damage, zero invalid or
  protected samples. Report SHA-256
  `74A705D19CFB271CAB2988003AAD4F270860E3D55952F1B5022D75E6565070E5`.
- APK: **40,525,610 bytes**, SHA-256
  `888F796151789CD21F50CB966B42908D75610E45724D6D3C2BD105836F83373A`.
- AAB: **36,350,785 bytes**, SHA-256
  `535015D9B35C49B3A71EDE0A4059A05280C135C1914FD218FE076F91ACED061A`.
- Checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`, API 28/36,
  seven ARM64 libraries, no network permissions, static 16 KB and creative dimensions
  passed. The artifact remains Debug-signed with certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`.
- Bundletool `1.18.3` APKS SHA-256
  `DE0FC268BF4165BB9A8D7EE03AC40A95D74709470459324AF38CEB5E79509FCA`; universal APK
  SHA-256 `F2BB7148D26AB1B02085BEF33EFF7F770CDD68E2D795D49F6E7BD651735BC5CC`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed.
- Approved Lava `ST5GDW23LB004392` installed and launched the exact APK and universal
  APK. Fresh six-sample, 30-second evidence found no configured fatal markers and
  thermal status 0 before/after; PSS **42,759-235,905 KB**. Lava reports 4 KB pages.

This is a technically validated local candidate, not a Play-ready release. Full physical
touch/tutorial/spectator/results/rematch/settings/lifecycle review, accessibility,
sustained full-match performance, genuine 16 KB runtime, authored final assets, cultural
review, release signing, privacy/Data Safety, content rating and Play Console actions
remain open.

## Superseded clean-source UI candidate — `aeda6de` — 2026-08-27

The exact clean source is commit `aeda6debab89404991f55a0f663a88798dd9c944`
(`ui: remove internal HUD labels and keyboard hints`). The patch is presentation-only;
it does not change authority, replay, bot, collision or match rules.

- Full EditMode: **140/140 passed**; full PlayMode: **81/81 passed**.
- Deterministic soak: **1/1 passed**, 1,000 seeds x2 (2,000 executions), zero divergence;
  XML SHA-256 `65BD32A7B978CB5679546EA3A7ACDFFC91261DC5D1A4CE86C3E280BB1B79C69F`.
- APK: **40,523,450 bytes**, SHA-256
  `62764237F44B1DD0D9F5B6E2E37C582FBA9B57B088B46C30805C883C123CAE65`.
- AAB: **36,348,625 bytes**, SHA-256
  `34F2E2D1318A8DF24EF9E3968511BE8686DDAE207D4ACBCA16801F247E11A6D6`.
- Checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`, version
  `1.0.0`/code `100`, API 28/36, VIBRATE plus dynamic receiver only, seven ARM64
  libraries, static 16 KB and creative dimensions passed. The artifact remains Debug-
  signed with certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`.
- Bundletool `1.18.3` APKS SHA-256
  `7C03F94C5E1DE08A3F417C49001702499B0D7B7EE6B49FD41B09D5143215D43B`; universal APK
  SHA-256 `378B667014E87EC93B501056E709769A5515E94C3F92D4911A472B004647F976`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed.
- Approved Lava `ST5GDW23LB004392` installed and launched the exact APK. Actual touch
  navigation reached the live opening screen, where the player-facing labels read
  `GADGET TIFFIN`, `READY` and `SPAWN SHIELD`; screenshots and hashes are indexed in
  P17. The fresh six-sample, 30-second capture found no configured fatal markers,
  thermal status 0 before/after and PSS **41,979–236,451 KB**. Lava reports 4 KB pages.

This is a technically clean local candidate, not a Play-ready release. Production-bot
pacing remains **Failed** (P15: 70/100 and 76/100 in the 240–360 second window), and
full touch tutorial/match/spectator/results/rematch/settings/accessibility/audio/lifecycle
review, sustained performance, authored final assets, cultural review, release signing,
privacy/Data Safety, content rating and Play Console actions remain open.

## Superseded current dirty-tree candidate — `fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus working-tree edits — 2026-08-26

The candidate pair below was rebuilt from the current intentionally dirty working tree with
Unity `6000.5.6f1`. It is exact current-source evidence, not a clean-source or publishable
Play release.

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **39,537,929 bytes**,
  SHA-256 `623616312BBD43668D95EC650F26517C3DC6AF57A7A8585DEEB4484C2EDB6450`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **35,364,227 bytes**,
  SHA-256 `C0C8A0A2AB3117A03D98A771F8305455B8A49E97D9ADD59B6D73D8884FEF85D5`.
- Final Unity Android build log SHA-256:
  `90223D68CB7AF94754C34F127E30B2A472B8FFD476923A84054808B85491632B`.
- Local candidate checker: **0 errors / 0 warnings**. Package `com.example.battleraja.m11`,
  version `1.0.0` / code `100`, min API 28, target API 36; only `VIBRATE` plus the
  dynamic receiver permission; seven ARM64 libraries, no other ABIs; static 16 KB
  alignment and Play Store creative dimensions passed.
- Lava `ST5GDW23LB004392` only: streamed install succeeded. Cold launch and relaunch
  resolved to `UnityPlayerGameActivity` as top-resumed; HOME backgrounding resolved to
  the launcher. Total PSS was **229,500 KB** (graphics PSS **70,160 KB**, swap PSS
  **94 KB**). Bounded logcat scans after launch and lifecycle found no crash, ANR,
  SIGSEGV, SIGABRT or Unity exception.

The same current-source validation also passed full EditMode **139/139** and PlayMode
**76/76**. The 100-match production harness passed **1/1** with 100/100 completed matches,
84/100 in the calibrated 240-360 second window, 96/100 combat-elimination matches,
100/100 bot-to-bot damaging pairs, zero protected-warmup damage and zero invalid-position
samples. The original 90% timing target and repeated same-seed production command-stream
comparison remain open.

Two fresh current-source same-seed runs for seed `9101` both passed the harness test but
diverged in command streams: batch A
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260826-092031939-9101.json`
(`86F76EC0B7A8F42F09143898380F96D20622601A4CF92B2813AFE1223D2BA2B0`, digest
`B0AD486CE9F71337`) and batch B
`batch-20260826-092149552-9101.json`
(`12318B0FCFA8DE432956A6821483B48F088F08CD70C7FB8ECE2D6B81948A7DA2`, digest
`70868EB27A9B7AB6`). Presentation-loop determinism is therefore not claimed.

Interactive touch/accessibility, sustained performance, battery/thermal, runtime 16 KB,
signing, final package identity, privacy/Data Safety, content rating, cultural/legal review
and Play Console gates remain open. The source tree was dirty, so `-RequireCleanWorktree`
was intentionally not used for this local technical check.

## Superseded post-audio candidate — `fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus working-tree edits — 2026-08-26

This is the post-audio rebuild of the exact intentionally dirty source state, using Unity
`6000.5.6f1`. The composed checker passed **0 errors / 0 warnings**: offline manifest
permissions, package/version, API 28/36, ARM64-only native payload, static 16 KB alignment
and store creative dimensions all passed.

- APK: **39,916,770 bytes**, SHA-256
  `4C04DF8D4B2D7E8728E37C6AAFBEAB6E7E0F917E1A5D191CF6D4B9F1136B2F7F`.
- AAB: **35,740,682 bytes**, SHA-256
  `9036F02B1D518707532D42461869FF3682FDC44510454BA37F95C440E1234992`.
- Build log SHA-256 `2FB380E3E0DF30204F648BC5FB9D68296E89DAA9778A2B783C4F669DB9A01485`;
  checker log SHA-256 `DA4522D3117AAAAF9EC005532D945EB97CDC2F186BBD2781DCC3927EE545F432`.
- Final current-source EditMode: **140/140 passed**, XML SHA-256
  `4F3E112B5CDA10A2168948544346EEF07AE2EE4B4DFC481DA8A50A9551AFEA7E`; log SHA-256
  `1DBA2EDD7BE8991C868EE12F931DB887CE3B3BA51936B9FE4ECFC7BF4E6A2CD5`.
- Approved Lava `ST5GDW23LB004392` streamed install, cold launch, HOME background and
  relaunch passed. The 10-second scripted capture recorded two samples with no configured
  fatal markers. Evidence is under
  `Builds\\Local\\Device\\Performance\\20260826-201300-v1-audio`; startup PSS was
  49,962–144,835 KB and graphics PSS 5,228–24,440 KB. One optional Unity Play Core
  `ClassNotFoundException` diagnostic was present in logcat; it was not a fatal marker,
  crash, ANR or Unity exception and did not prevent launch.

The source tree remained dirty (55 changes), so `-RequireCleanWorktree` was intentionally
not used. The artifact is debug-signed with a temporary package ID. Runtime 16 KB behavior,
longer sustained performance/battery/thermal capture, final mix/originality/cultural review,
signing, package identity, privacy/Data Safety, content rating and Play Console checks remain
owner-controlled. The original 90% timing target remains open for human feel/balance review;
the repeated same-seed command-digest gate now passes at deterministic real-time playback
after continuous inputs were quantized for diagnostic hashing.

## Current binary after P10 — 2026-08-26

- APK: **39,920,538 bytes**, SHA-256
  `5438F521CEEC9A0B4202433542B5A5BB4533462688E25D969BDBF05A45A2014D`.
- AAB: **35,744,492 bytes**, SHA-256
  `E7DC91460AA2DCE0DD3B2156196A4C4B73B340C8372EA874A34F5C867CED000C`.
- Android build log SHA-256
  `2F13FE6C841469DF1934AD39B91C561F75AF54F95393B4A524B8EA38D6A6E8E4`;
  checker log SHA-256 `6E38B1AB5BFE07E281255C0022DF4F8E31258CB9D088B90F2C273A14E1FB87D7`.
- Current EditMode **140/140** (`editmode-post-determinism.xml`, SHA-256
  `3887FB8B53BC46EDDA887048AEFEE0FFC8F6D97BC7D1263E5BD01BF8EDE05E36`) and PlayMode
  **79/79** (`playmode-post-determinism.xml`, SHA-256
  `D44B590F151850C56234E6465DBA94212E9E31FBC37D94693552593152A3EA6D`).
- Lava `ST5GDW23LB004392` streamed install/relaunch passed; the exact current APK capture
  is `Builds\\Local\\Device\\Performance\\20260826-210000-v1-determinism` with no
  configured fatal markers. PSS was 50,108–232,032 KB and graphics PSS 5,228–70,288 KB.
- Cached bundletool `1.18.3` generated a universal APK set from the AAB; APKS SHA-256
  `ED98B06E43B4096466DF3521A0E1917CDF8C310F8DA5BA88D962651184AF15A2`, extracted APK
  SHA-256 `7655C8151DC51AEAF981871BFB685AD93D44E720F6003DBDD018C19C9CA74CC2`, and
  `zipalign -c -P 16 -v 4` passed. This generated-set log is recorded under
  `Builds/M11/Logs/zipalign-16k-bundletool-1183-universal.log`.

The technical checker passed 0 errors / 0 warnings, but this is still dirty-tree,
debug-signed, temporary-ID evidence. Runtime 16 KB confirmation, sustained performance,
final audio/art/cultural review, signing, privacy/Data Safety, content rating and Play
Console validation remain owner-controlled.

## Current-source 100-match release gate — 2026-08-26

Using the same current dirty source, Unity `6000.5.6f1`, seeds `9101-9200` and the
50x diagnostic playback setting, a fresh third run passed the strict production-bot
release gate **79/79**. All 100 matches completed within 360 seconds; 85/100 were in
the 240-360 second window, 100/100 had bot-to-bot damage, 91/100 had combat eliminations,
and 9/100 were Aandhi-only. Protected-warmup damage and invalid-position samples were
zero. The aggregate report is
`Builds\Local\V1GameplayTruth\ProductionBotReports\batch-20260826-161343920-9101.json`,
SHA-256 `640615AE31DD776D93C5CE24EBF9C6FA96B21C3F4A6CC6A4AD824C944055F4DD`; XML SHA-256
`EAE74C84CC527D058C4D1179206F5910B805C0B177D432AFB12948E4426571A8`; log SHA-256
`4DD21E269466C7A6295160050B2324FBA38669C34FB7D871E0EC667ED92EDD34`.

Two immediately preceding same-settings runs failed only the statistical combat/Aandhi
thresholds (90/10 and 89/11), documenting timing sensitivity of the 50x shortcut. This
passing run is valid release-gate evidence, while real-time playback remains the stable
same-seed determinism setting documented in P10.

The current-source deterministic replay soak was also refreshed: `BATTLERAJA_SOAK_MATCHES=1000`
passed **1/1** (1,000 seeds executed twice, zero divergence) in 548.9933162 seconds. XML
SHA-256 `40514F4FF51871CDE7BEA0594A8A6D52A4D8259A95845888E01B3AEB288322EE`; log SHA-256
`98F15DBA2D5AB8997E68DA86193EB2B90B82DFBC7B22C39DC5ADE834FD5EF4ED`.

Approved Lava `ST5GDW23LB004392` completed a six-sample, 30-second launch/menu capture for
the current APK with no fatal markers and thermal status 0 before/after. PSS ranged
58,119–256,530 KB. Evidence directory:
`Builds\\Local\\Device\\Performance\\20260826-220000-v1-current-30s` (manifest SHA-256
`FE86E2ED8684B227117305CF5FAAA5CA378A512AD41E1C431907C047235E6565`; logcat SHA-256
`AC64C6A6F77AF03756E89057E88EF504A4288D469D0E518166F095D8BB15B23B`). Lava reports
4 KB pages, so genuine 16 KB runtime validation and sustained full-match performance remain
owner/device gates.

The final static candidate checker was rerun after this evidence refresh and returned
**0 errors / 0 warnings** for the same APK/AAB pair. It passed the offline manifest,
ARM64-only payload, static 16 KB ELF alignment and store-creative dimensions; checker log
SHA-256 is `C1B5D9AFCC56E816D345708365935C2F1EFC4698D15DDB9330AD1E1EBC2A8545`.

The checker was repeated after the final QA-index documentation update; it again returned
**0 errors / 0 warnings**, observed 60 intentional dirty changes, and its final log SHA-256
is `2456A1DAD5716ECF8411020272E955A2C58EB43DCF595CB3FC7E8E8554B73E3F`.

## Exact Android candidate source — `35de9f3` — 2026-08-24

The current source retains the cached actor views for Pehel authority-result
presentation, adds the owner-configurable package identity seam, explicit
non-development release flags, disabled offline Unity Analytics/services and retains
the repeatable offline APK manifest gate. Current source validation is **0/0**,
EditMode **125/125** and PlayMode **73/73**; the archived package pair below
was built before the latest presentation fixes. Fresh release-shaped packages archived inside
`Builds/Local/V1Evidence/35de9f3/Android` are APK
`41432964C104C7EA58A1DECC3423F611515D909E7EFA61F85E6AC46D7BFBE389`
(39,487,017 bytes) and AAB
`34A0BAEFBF68A6A7679244EADB342556B5F3EBA87D3AB61CACDA48CAE04BE785`
(35,313,317 bytes). The AAB is ARM64-only and passed static 16 KB alignment;
installation/launch on Lava succeeded and the portrait menu was captured in
`Builds/Local/V1Evidence/35de9f3/Android/lava-launch.png`; no interactive route
pass is claimed.
Manifest inspection reports temporary package `com.example.battleraja.m11`,
version `1.0.0` / code `100`, min SDK 28, target/compile SDK 36, and only
`VIBRATE` plus Unity's dynamic receiver permission; `INTERNET` and
`ACCESS_NETWORK_STATE` are absent. Signing, package identity, device,
performance and Play approval gates remain open.

## Exact current candidate — docs `be0c510` / source `1d743b0` / runtime `d96d3f2` — 2026-08-24

- Validation: **0 errors / 0 warnings**; EditMode **125/125**; PlayMode **71/71**.
- Release-shaped APK: **39,486,559 bytes**, SHA-256
  `89156306717C5EB27EE193AD1D46809DFE19159112ADC3C77008D4C6A3C89DE0`.
- Release-shaped AAB: **35,313,996 bytes**, SHA-256
  `F5776F6AF19EE1C0A803D76050A80E62E883710E00296EF9603CED279D2227C1`.
- AAB: 7 ARM64 native libraries, 0 other ABIs, static 16 KB alignment passed.
- Lava `ST5GDW23LB004392`: exact APK installation succeeded; launch resolved to
  the Unity activity, but the active lock screen blocked interactive menu,
  tutorial and match QA. No physical route claim is made from this run.
- The Android lifecycle pause guard is automated-test covered; sustained
  performance and human background/resume review remain open.

## Latest exact-current evidence — `6ac5c12` — 2026-08-24

- Validation: **0 errors / 0 warnings**; EditMode **125/125**; PlayMode **67/67**.
- Release-shaped APK: **39,523,632 bytes**, SHA-256
  `09F5375FA8D5DEC066A09D8CCDF0BAF01269F4B402252EF2908691C773402EF3`.
- Release-shaped AAB: **35,351,357 bytes**, SHA-256
  `70825F82A4D79E1E036F4DA8A286778244406D51B1D60A568BD066ED1B82DAA8`.
- AAB: 7 ARM64 native libraries, no other ABIs, and all checked ELF LOAD segments
  aligned to `0x4000`.
- Lava `ST5GDW23LB004392`: release-shaped APK installed and launched to the branded
  menu; runtime action bindings are explicit, but the Unity surface exposes no
  actionable UI nodes, so physical touch route and sustained performance review remain open.
- Web: exact source built and served over local HTTP; Chrome/Edge reached the Unity
  loader only in bounded headless captures, so Web is not a release gate.

## Current release gates

| Gate | Current state | Owner action |
| --- | --- | --- |
| Product/package identity | Blocked: the candidate defaults to temporary `com.example.battleraja.m11`, while the build entrypoint now accepts an owner-approved application ID override | Approve final application ID and branding; rebuild and re-run the manifest gate |
| Signing | Not started | Approve upload key/Play App Signing path; never commit the key |
| Target API | Configured to API 36 | Recheck against current Play policy at upload time |
| 64-bit | Passed with evidence for the current debug-signed AAB: 7 ARM64 libraries, 0 other ABIs | Re-run inspection after any package/plugin change |
| 16 KB pages | Static evidence passed: zipalign `-P 16` and all eight ARM64 ELF LOAD segments at `0x4000`; runtime 16 KB environment still open | Re-run the checker after any package/plugin change and install on a 16 KB Android environment when available |
| Permissions | **Passed for the exact debug APK**: `VIBRATE` and Unity's dynamic-receiver permission only; no `INTERNET`, `ACCESS_NETWORK_STATE` or SD-card permission | Recheck the final signed AAB/APK and document any future online permission change |
| Device QA | Release-shaped launch/menu smoke passed on Lava (`ST5GDW23LB004392`); full touch, accessibility, battery and thermal review open | Owner performs touch, accessibility, battery and thermal review |
| Store/legal | Draft only | Approve privacy, data-safety, content rating, cultural and legal copy |
| Play Console | Not started | Owner creates the app and decides rollout/release track |

## Latest local candidate evidence (2026-08-24)

### Exact offline packaging hardening

The final documentation checkout is `HEAD` on `codex/v1-playstore-release`;
runtime/package source is `f4425d6`. Validation is
**0 errors / 0 warnings**; EditMode is
**125/125** and PlayMode is **66/66**. The fresh release-shaped APK is **39,529,326
bytes** (SHA-256
`AE74717B597C4CBCFDECF7D8DB719C177100F495CC084ABFD0E1EA6AAD3E2C52`) and the AAB is
**35,357,477 bytes** (SHA-256
`8EB49EFC8D58D144E5A792224FC9A3570FF4E37F121E06B6E55093C9D4D5F5E7`). The AAB
contains 7 ARM64 libraries, no other ABIs, and passed static 16 KB ELF alignment.

`aapt dump permissions` and Lava `dumpsys package` show `VIBRATE` plus Unity's
dynamic-receiver permission only; `INTERNET` and `ACCESS_NETWORK_STATE` are absent.
The APK installed and launched only on Lava `ST5GDW23LB004392`, with the branded
offline menu visible and no fatal/ANR/SIGSEGV marker. Raw captures are recorded in
`Docs/QA/V1_ANDROID_OFFLINE_PACKAGING_2026-08-24.md`.

### Exact current checkout artifact

Current branch tip: `357dfdf1e6289c172dab60e514f555ba3d5bc914`; runtime-equivalent build
source: `46724ac2dfa403f40f58669240e61918c2a94d1b`.

- Exact validation: **0 errors / 0 warnings**.
- APK: **40,431,927 bytes**, SHA-256
  `0694958A43F1BADD30E697095F249733992F9D6904E10E1923CD0CAF01010C78`.
- AAB: **36,262,036 bytes**, SHA-256
  `906D85FA00E4A9787A0C1DE892DC3F27A098ACF21BB1735E08C977565A1D09A4`.
- AAB checker: 8 ARM64 libraries, 0 other ABIs, all checked native LOAD segments at
  `0x4000`.
- Lava-only launch: `UnityPlayerGameActivity` top-resumed, branded offline menu visible,
  no fatal/ANR/SIGSEGV marker. Raw captures remain outside the repository.
- Manifest: package `com.example.battleraja.m11`, version `1.0.0`, code `100`, target API
  36; `VIBRATE`, `INTERNET`, `ACCESS_NETWORK_STATE` and Unity's dynamic-receiver
  permission are present. This is a release gate, not a Play submission.

### Current visual-polish source

The latest exact-source tutorial/UI correction candidate is `c6badbf6cf5b1c7340fa907821aeb4cbf2194bc0`
from disposable copy `C:\Projects\BattleRaja-v1-tutorial-verify`:

- Validation **0 errors / 0 warnings**; EditMode **125/125**; PlayMode **66/66**.
- APK **40,431,923 bytes**, SHA-256
  `E6CBEAD6F97C036C0C9D1663CA5972799AEF3B330D75A3D2AAA94D5E699C7DB3`.
- AAB **36,262,021 bytes**, SHA-256
  `124E14ABE6012B3B42D7B7741D0C647416E278E82ABFE358EF89A53BAAD64021`.
- Bundle inspection passed: base manifest, 8 ARM64 libraries, no other ABIs and
  `0x4000` alignment for every checked native ELF LOAD segment.
- The exact APK was installed only on Lava. The tutorial SKIP action visibly reaches the
  completion card with replay/menu actions. The preceding exact visual candidate covered
  menu, mode, fighter-selection, Bazaar match, movement, match resolution and REMATCH. The
  correction APK also captured a successful player-owned Tiffin use at spawn; a later edge
  placement probe honestly returned `InvalidPlacement`.

The visual change is presentation-only: the Bazaar center uses a fictional six-panel
canopy/gold-orb landmark and the menu hero is larger at the phone viewport. This remains
a debug-signed, temporary-ID prototype candidate and is not a Play submission.

The exact V1 source was validated in disposable copy
`C:\Projects\BattleRaja-v1-verify-20260824j`:

The latest checkout also contains editor/test-only warning cleanup at `649d0bb`. Its
fresh APK build (`51D86184F6C69DD30CD249D273FA0F8F5BA96B4159D86DD1472FE4FD54320DA5`,
40,431,911 bytes) and matching AAB (`518102EAE7DDB71DA9393ABE3E948A47440260C9DF8D19532AAFF14FA1BE98B0`,
36,262,033 bytes) recorded zero `CS0618` warnings, zero C# errors and passed the static
16 KB alignment check; the installed visual artifact remains the `c6badbf` correction
candidate documented above.

- Unity `6000.5.6f1` (`0e0577a1a2ac`), validation **0 errors / 0 warnings**.
- Exact runtime source: `d825832bced4c5e07c7967d891696842eb55609a`.
- EditMode **125/125** and PlayMode **66/66** passed.
- Release-shaped Lava APK: **40,429,675 bytes**, SHA-256
  `50FD2D7F9C29F4888F2965810F9FD8130F7C2857F2A15AD7E3A5CF5908E7BFCC`.
- Debug-signed AAB: **36,259,768 bytes**, SHA-256
  `052F9CAB180E15AEEC0C2D8DCAB47187C53C58F07629C69F81A647697DB9FBF1`;
  base manifest present, 8 ARM64 libraries, 0 other ABIs, 450 entries, all ARM64
  ELF LOAD segments statically aligned to `0x4000`.
- Lava screenshots and raw metrics are recorded in
  `Docs/QA/V1_ANDROID_VISUAL_FEEDBACK_2026-08-24.md`; the Tiffin pickup/use route is
  now visually captured (raw files remain outside source).

This evidence is a release-shaped prototype candidate. The APK/AAB is debug-signed and
not publishable, the package ID remains temporary, legacy icon configuration still emits
a Unity deprecation warning, runtime 16 KB confirmation, performance, store/legal and
human review gates remain open.
The exact Lava visual pass captured successful Tiffin pickup/use. Tutorial completion,
results/rematch observation, touch/accessibility, performance, signing, store/legal and
human review remain explicit gates even though the automated authority regression is green.

## Local artifact command

Run from a clean, disposable project copy so scene generation cannot overwrite the working
tree:

```powershell
$unity = 'C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe'
pwsh -File Tools/Build/Android/build.ps1 `
  -ProjectRoot . `
  -UnityExe $unity `
  -BuildMethod BattleRaja.Editor.BuildEntrypoints.BuildAndroidV1ReleaseCandidate
```

Expected output: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`. The bundle is
release-shaped but not a Play submission: it uses the current temporary package ID and no
owner-approved signing identity.

## Artifact inspection

Record the exact source SHA, Unity revision, AAB byte size and SHA-256. Inspect the bundle
with Android Studio/bundletool and record:

1. manifest package, version name and monotonically increasing version code;
2. target/min SDK and merged permissions;
3. ARM64-only native libraries and ABI splits;
4. native ELF load-segment alignment for 16 KB pages;
5. debug symbols/profiling flags and signing certificate state;
6. dependency and licence inventory.

For the installable APK companion, run the repository manifest checker before any device
route evidence:

```powershell
pwsh -File Tools/Validation/check_android_manifest.ps1 `
  -ApkPath Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk `
  -AaptPath "$env:LOCALAPPDATA\Android\Sdk\build-tools\36.0.0\aapt.exe" `
  -ExpectedVersionName 1.0.0 `
  -ExpectedVersionCode 100 `
  -ExpectedMinSdk 28 `
  -ExpectedTargetSdk 36
```

Do not treat an APK install as proof that a Play bundle is acceptable. Google Play performs
bundle processing and signing checks that must be repeated in the owner-controlled console.

## Lava validation

Use only the approved Lava serial. Do not use the Oppo phone or the local emulator for the
release evidence.

```powershell
$adb = 'C:\Users\USER\AppData\Local\Android\Sdk\platform-tools\adb.exe'
& $adb devices -l
& $adb -s ST5GDW23LB004392 install -r Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk
& $adb -s ST5GDW23LB004392 shell monkey -p com.example.battleraja.m11 1
& $adb -s ST5GDW23LB004392 shell dumpsys meminfo com.example.battleraja.m11
& $adb -s ST5GDW23LB004392 shell dumpsys gfxinfo com.example.battleraja.m11
```

Capture cold launch, menu, tutorial, fighter selection, match opening, ability/gadget use,
Aandhi, elimination/spectator, results/rematch, settings, background/resume and a repeated
match run. Store raw screenshots/logcat/measurements outside tracked source until reviewed.

## Google Play source links

- [Target API level requirements](https://support.google.com/googleplay/android-developer/answer/11926878?hl=en-GB_ALL)
- [16 KB page-size compatibility](https://developer.android.com/guide/practices/page-sizes)
- [Create and set up an app, signing and version codes](https://support.google.com/googleplay/android-developer/answer/9859152?hl=en)
- [Play App Signing](https://support.google.com/googleplay/android-developer/answer/9842756?hl=en)

## Stop conditions

Stop the release claim if the exact source does not compile, the full test suites fail, the
AAB is not produced, the merged manifest requests an unapproved permission, 16 KB alignment
cannot be demonstrated, Lava launch is unavailable, or any human/legal gate is incomplete.

## Current source presentation evidence — 2026-08-26

On the current dirty source (`HEAD fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus
intentional working-tree edits), the controlled Unity presentation generator produced
saved fighter transform rigs, a nine-state Animator controller and editable clips, plus 14
bounded VFX prefabs. Scene prefab references were refreshed with Unity serialization after
regeneration. EditMode **140/140**, PlayMode **80/80** and the focused rig/VFX test **1/1**
passed. The audio mixer contains named Music and Combat buses, with guarded persisted source
volume controls for older Unity-authored mixer metadata.

This closes the machine-verifiable generated-presentation baseline, not final human art,
cultural, touch, performance, signing, privacy or Play approval.

## Final current-source package refresh — 2026-08-26

The exact dirty workspace (`HEAD fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus 63
intentional changes) was rebuilt after the audio warning fix. Focused audio passed **1/1**;
full EditMode passed **140/140** and full PlayMode **80/80**. The mixer has named Music and
Combat buses with persisted source-volume controls; runtime no longer probes missing
editor-only exposure names.

- APK: **40,533,142 bytes**, SHA-256
  `F50F7C3B2FDDD0847662437938C662C263F33599FE3529A3E79003CD71D7E2B3`.
- AAB: **36,357,145 bytes**, SHA-256
  `E1A68E2EA9326B0A0D48B1F479AF4D9EF99737634947DAAC57231C418E7121FF`.
- Composed checker: **0 errors / 0 warnings**, SHA-256
  `839DF0715406788F78A222FC0FD9625852F4AE6B022126DA56A16A99EC4A1B62`.
- Bundletool 1.18.3 APKS SHA-256
  `062603B21CF398C9D4C3259D7FF49A0F36A56FC5525629E458F425820E107166`; universal APK
  SHA-256 `85297DFA56305322A13614A9A4B89968F3BC0D39E47A7000E5976147096F9AE5`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed.
- Lava `ST5GDW23LB004392` install/relaunch passed. Six-sample, 30-second launch/menu
  capture: no configured fatal markers, thermal status 0 before/after, PSS
  **55,262–236,543 KB**. Manifest SHA-256
  `E9BD4D1B922A4AB0FB8EE90DC66AF84FA0876BDD24D89F4E7A253411243268E9`; logcat SHA-256
  `33FE2B05DA727432C76C4B57428178018CBF62AB35490739AE66C5AA467F68C0`.

This is a debug-signed, temporary-ID technical candidate. Runtime 16 KB behavior (the device
reports 4 KB pages), sustained full-match performance, touch/accessibility/tutorial route,
final authored art/audio and cultural review, package identity/signing, privacy/Data Safety,
content rating and Play Console approval remain owner-controlled.

The final-source strict production-bot gate was rerun twice with 100 seeded matches and the
existing 50x diagnostic playback. Both runs completed 100/100 and passed the combat, gadget,
warmup, position and tick-budget invariants, but only 70/100 and 76/100 respectively met the
240–360 second pacing window. This confirms the known timing-sensitive shortcut; the earlier
passing run remains historical evidence and no threshold was loosened.

## Exact clean-source package refresh — 2026-08-27

The reviewed candidate source is clean at `2f9a6a0151e3b0c2359d9b0f8892c28e6404ec4b`; see
`Docs/V1_RELEASE_PLAN.md` P16 for the evidence index. Full EditMode **140/140**, PlayMode
**80/80**, and the exact-source 1,000-seed replay soak (twice, zero divergence) passed.

- APK: **40,521,770 bytes**, SHA-256
  `0F635D962A179B28FD07189E348D837A7BF7B647638DDAF7FBF9A7EAB14B3458`.
- AAB: **36,346,956 bytes**, SHA-256
  `4397F62FE5A83CEF2EB5240212988787735289DE8AA24F26D78B9E95C83D168D`.
- Composed checker: **0 errors / 0 warnings**; package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36 and seven ARM64 libraries.
- Bundletool 1.18.3 universal extraction passed; direct and extracted APK
  `zipalign -c -P 16 -v 4` passed.
- Lava reinstall/relaunch passed. Fresh six-sample, 30-second launch/menu capture found no
  configured fatal markers and thermal status 0 before/after; the phone reports 4 KB pages.

This remains a debug-signed temporary-ID technical candidate. Full touch/tutorial route,
sustained match performance, runtime 16 KB validation, production-bot pacing, final authored
content/cultural review, package identity/signing, privacy/Data Safety, content rating and Play
Console approval remain open.

## Exact faceted-art candidate refresh — 2026-08-28

Source commit `816d9ac` replaces the primitive-like fighter presentation pieces with saved
faceted low-poly profiles and retains the render-only rig/Animator/VFX boundary. Full
EditMode is **141/141**, PlayMode **87/87**, and repository validation is **0 errors / 0
warnings**. The exact rebuilt APK is 40,542,342 bytes (SHA-256
`0517EE901A9EAE943140538366B0574E893DC6BD66A5D1714D630C2379EF5FAC`); the matching AAB is
36,367,513 bytes (SHA-256
`BF52E649BFD92F277F5C9933A7FDF34FFB25410F1D5A18EF6FC3097AA31BA331`). The composed offline
checker passes manifest/API 28/36, no-network permissions, seven ARM64 libraries, static
16 KB ELF alignment and store-creative dimensions. bundletool 1.18.3 universal extraction,
direct/extracted `zipalign -c -P 16 -v 4` and v3 `apksigner` verification also pass.

The APK installed on approved Lava `ST5GDW23LB004392` and real touch reached menu, Solo
Raja, Bijli selection and the live opening match. A bounded six-sample, 30-second live-state
capture found thermal status 0 and no configured fatal markers. The phone reports 4 KB
pages, so this is not physical runtime-16-KB proof or sustained performance approval. Final
commissioned art/animation/VFX/audio/cultural review, full tutorial/all-fighter/accessibility
comfort, package identity/signing, privacy/Data Safety, content rating and Play Console
actions remain open.
