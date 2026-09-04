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

## Latest local gate — 2026-09-04 — touch-control clarity continuation

The candidate now renders original vector glyphs inside the production touch surfaces and
uses tighter portrait sizing. Static validation is **0/0**, EditMode **159/159** and PlayMode
**95/95**. The exact APK is **41,668,888 bytes** (`8B9D11BFDB40A75D7C301A255B71D74516BD83F7D5672730FFFFA34A635E9C71`); the
AAB is **37,494,403 bytes** (`F877BF07F6CBBCF890DB2968B5D48CBB93FC64CCB2DC10D4CCA709679EC99BBC`). The
technical checker passes package `com.example.battleraja.m11`, version `1.0.0` / code
`100`, API `28/36`, no network permissions, ARM64-only native libraries, static 16 KB
alignment and the supplied store dimensions.

The exact APK is installed on approved Lava `ST5GDW23LB004392` (`1080x2460`, Android
14/API 34, 4 KB pages) and the pulled base matches the APK SHA-256. Fresh menu/briefing/
fighter-select/live/settings/action captures are under
`Builds/Local/V1GameplayTruth/Next/touch-glyph-final2/`. The action probe reached the
expected Tiffin invalid-placement edge state and process-scoped logcat had zero configured
fatal markers. The six-sample/30-second performance capture reports PSS **296,208–302,448 KB**,
RSS **408,496–422,188 KB**, graphics PSS **87,528–93,684 KB**, raw app CPU **97–140%**
(mean **113.3%**), battery **69% → 69%**, thermal status **0**. SurfaceFlinger returned no
frame timestamps and is not used for an FPS claim. The candidate remains temporary debug-signed and not Play-ready; final
identity/signing, physical 16 KB runtime, normalized performance/endurance, complete
comfort/accessibility review, commissioned art/audio and owner privacy/Data Safety/IARC,
cultural and Play approvals remain required.

## Latest local gate — 2026-09-04 — settings surface clarity continuation

The menu and in-match pause settings now use original icon-backed tiles and accent rails,
with explicit ON/OFF state labels for all accessibility toggles. Static validation is
**0/0**, EditMode **159/159**, PlayMode **96/96**. The exact APK is **41,678,776 bytes**
(`714ACE23E8C9DA859B91B14E12F9E7E65CA277ADAAAB315F1C81B4547D195C93`); the AAB is
**37,504,322 bytes** (`74F7EDC96481EA868FF1A8F078E70D6C407126AD9A197BF2020D044F108445CC`).
The technical checker passes package `com.example.battleraja.m11`, version `1.0.0` /
code `100`, API `28/36`, no network permissions, ARM64-only native libraries, static
16 KB alignment and store dimensions; log SHA-256 is
`7F7A42F2BD60B8ADE04868174E83DB7A8F8F00CB6108ACBD1723498D7E04B0F7`.

The exact APK is installed on approved Lava `ST5GDW23LB004392` (`1080x2460`, Android
14/API 34, 4 KB pages). Fresh menu/menu-settings/live/pause-settings/high-contrast
captures are under `Builds/Local/V1GameplayTruth/Next/settings-polish-final-20260904/`;
the scoped logcat has zero configured fatal markers. The exact live six-sample/30-second
capture under `Builds/Local/V1GameplayTruth/Next/performance-settings-polish-final-20260904/`
reports PSS **298,369–304,625 KB**, RSS **417,808–424,064 KB**, graphics PSS
**89,324–95,480 KB**, raw app CPU **106–127%** (mean **116.3%**), battery **63% → 63%**,
thermal **0**. No FPS claim is attached because device `gfxinfo` exposed no frame timing.
This is not final commissioned art/audio, normalized endurance/GPU/GC, physical 16 KB
runtime or owner-approved identity/signing/privacy/Data Safety/IARC/cultural/Play evidence;
the candidate remains temporary debug-signed and not Play-ready.

## Latest local gate — 2026-09-04 — hero presentation continuation

The current candidate adds connected render-only hero silhouette parts and tighter tall-phone
framing. Static validation is **0/0**, EditMode **159/159**, and PlayMode **95/95**. The
exact APK is **41,667,212 bytes** (`675945B1E3CB7C1471CE7C65C299B17A0969104C969D57EE5083F608436FFA04`);
the AAB is **37,492,707 bytes** (`FC0A070E204F3F8C788A38E72A66C70934162382E2B7FC74CA8FB18844C72556`).
The technical checker passes package `com.example.battleraja.m11`, version `1.0.0` /
code `100`, API `28/36`, no network permissions, ARM64-only native libraries, static
16 KB alignment and the supplied store dimensions.

The exact APK is installed on approved Lava `ST5GDW23LB004392` (`1080x2460`, Android
14/API 34, 4 KB pages). Fresh rendered menu, fighter-select, live Bastion and rematch
captures are under `Builds/Local/V1GameplayTruth/Next/hero-framing-20260904/`; a bounded
30-second live sample is under
`Builds/Local/V1GameplayTruth/Next/performance-hero-framing-20260904/`. This sample has no
configured fatal markers, but the SurfaceFlinger ring was zero-filled, so it is not used
as an FPS claim. The candidate remains temporary debug-signed and not Play-ready; final
identity/signing, physical 16 KB runtime, normalized performance/endurance, complete
comfort/accessibility review, commissioned art/audio and owner privacy/Data Safety/IARC,
cultural and Play approvals remain required.

## Current local gate — 2026-09-01

See the complete evidence record in `Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-01.md`.
Static validation is **0/0**, EditMode **155/155**, PlayMode **94/94**, and the Bastion v2
replay soak has zero divergence across two 8,400-tick seeds. The technical checker passes
the exact APK (`41,510,440` bytes, SHA-256
`5F7438105FE450D6331CFEDEE1FAEEB87FB4F6677EB811A997A02CC8FD7C4AE9`) and AAB (`37,335,957`
bytes, SHA-256 `87C835570B62C4C3A79C156F94CB7E15C6AD31FCB50A0E8ADB0FDE6672DC4858`) for
temporary package `com.example.battleraja.m11`, target SDK 36, no network permissions,
ARM64/static 16 KB alignment and creative dimensions. Fresh approved-Lava evidence and a
clean six-sample live telemetry capture are under
`Builds/Local/V1GameplayTruth/Final/lava-20260901-final/`; the phone reports 4 KB pages and
the package remains unsigned for publication. No Play upload or owner-only declaration was
performed.

## Policy recheck baseline — 2026-08-24 (superseded by the 2026-08-30 addendum below)

- Google’s target-API guidance requires new apps and updates submitted from
  2026-08-31 to target Android 16/API 36 or higher; this candidate is configured
  for API 36.
- Google’s current 16 KB page-size guidance requires apps targeting API 35+ to support
  16 KB memory pages on 64-bit Google Play devices; from 2027-02-01, updates that do not
  support 16 KB pages cannot be released. The current AAB has passed static ARM64/16 KB
  checks and the exact debug APK has a host-GPU Android 16 16 KB AVD smoke; the final
  signed artifact still needs the same inspection and physical/other-profile runtime
  checks.
- Google requires an accurate Data safety form and privacy-policy link for apps
  published on closed, open or production tracks, including apps that collect no
  data. An app kept exclusively on internal testing is exempt from the Data safety
  form, but that exemption is not a release shortcut.
- Google also requires target-audience declarations and a completed content-rating
  questionnaire for a new Play app. These remain owner/legal gates.

Primary sources: `https://developer.android.com/google/play/requirements/target-sdk`,
`https://developer.android.com/guide/practices/page-sizes`,
`https://support.google.com/googleplay/android-developer/answer/10787469`, and
`https://support.google.com/googleplay/android-developer/answer/9898843`.

## Latest policy recheck — 2026-08-30

The current Android target-API page keeps the **2026-08-31** requirement for new apps and
updates to target Android 16/API 36 or higher. The current 16 KB guidance requires API 35+
apps to support 16 KB memory pages on 64-bit Google Play devices and states that unsupported
updates cannot be released from **2027-02-01**. Data safety declarations and a privacy-policy
link remain required for published tracks, including no-data apps; apps exclusively on
internal testing are exempt. Google Play requires an accurate IARC content-rating
questionnaire for every app. These policy facts do not change the local candidate settings,
but they do reinforce the owner-controlled signed-artifact, declaration and Play Console
gates. See the dated source record in `Docs/RESEARCH_LOG.md`.

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

## Latest exact-source lifecycle candidate — P66 — 2026-08-31

Source tip `e603ce7e7f1cb279f5e3e9d606ea5eae89603ecb` clears transient player input on
`OnApplicationPause` across the adapter, virtual stick and attack/ability/gadget buttons;
the HUD clears the adapter before its lifecycle pause boundary. ADR-076 records the
presentation/input-only decision. Focused lifecycle PlayMode is **1/1**, full EditMode is
**141/141**, full PlayMode is **92/92**, and static validation is **0 errors / 0 warnings**.

The exact rebuilt temporary-ID APK is **40,679,115 bytes** (SHA-256
`349F02C67DE4CC801C5CB81B9CEC375A18D89B136C1A3AD9BB9549E9640A41CB`) and matching AAB is
**36,504,445 bytes** (SHA-256
`9A5BE261D2504007BCBAF4105568F19437CBA8A4DEFAA3383371DE35386D51E0`). The composed checker
passes **0 errors / 0 warnings**: package `com.example.battleraja.m11`, version `1.0.0` /
code `100`, API `28/36`, no network permissions, seven ARM64 libraries and static `0x4000`
load alignment. These remain temporary debug-signed artifacts and are not Play-ready.

The APK was installed only on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android
14/API 34). P66 records a live Opening Fight → Android HOME for approximately five seconds
→ resumed Unity activity route in
`Builds/Local/Device/final-circle-20260830/p66-lifecycle-route/`; the manifest is 8,192 bytes
(SHA-256 `217589DAE7592EC397328F12D8C3DF88246B7AEE035776584DF5FE9624499103`). Both captures
retain `ALIVE 8` and `ZONE 14.0 > 11.0`, the activity dump is RESUMED, Lava reports 4 KB
pages, and route logcat has zero configured fatal/ANR/native/managed-crash markers.
Physical 16 KB, full lifecycle/held-input comfort, normalized performance, endurance,
final authored/cultural/accessibility/fun review, signing, privacy/Data Safety, rating,
support URL and Play Console actions remain open.

## Latest exact-source candidate — documentation tip `b4b5649` (runtime/art `ac45479`) — 2026-08-29

The saved-environment/runtime presentation continuation is documented in P45. The exact
temporary-ID APK is **40,672,170 bytes** (SHA-256
`6103F42176726E8CACE0DA7C4880BD105A55E50FFD92EB1BA8B2F531BEAA231D`) and the matching AAB
is **36,497,323 bytes** (SHA-256
`9893493591C4474E517B3D80A5107986493A2E70F59C850D17AC08C8B2748404`). The composed
release checker passed **0 errors / 0 warnings**; bundletool 1.18.3 universal extraction,
direct/extracted `zipalign -P 16`, and v2/v3 signature verification passed. These are
temporary Android Debug-signed artifacts and are not final release signing.

Fresh approved-Lava evidence is under
`Builds/Local/Device/Performance/20260829-lava-ac45479-smoke/`. The exact APK installed
successfully and real touch reached menu → Solo Raja → Bijli selection → live opening.
The six-sample/30-second bounded live capture is under
`Builds/Local/Device/Performance/20260829-lava-ac45479-30s/`; it continued into player
defeat/spectator state with PSS **267,957–272,145 KB**, graphics PSS **75,792–79,888 KB**,
battery level **62%**, thermal status **0**, and no configured fatal/ANR/SIGSEGV markers.
Lava `ST5GDW23LB004392` reports 4 KB pages; this evidence is raw bounded device smoke,
not sustained performance, battery, thermal or physical-16-KB approval. No Oppo device
was used. Final identity/signing, legal/privacy/Data Safety, cultural/final art,
accessibility/fun and Play Console gates remain owner-controlled.

## Latest UV/skinning candidate — `bc392fd` — 2026-08-29

The current local presentation commit `bc392fd` adds deterministic UV coverage to all
generated meshes and saved two-bone primary body/cloak skins. Full EditMode is **141/141**
and PlayMode is **87/87**; static validation is **0 errors / 0 warnings**. The exact
temporary-ID APK is **40,595,182 bytes** (SHA-256
`9A0F3715BFFA208F4D821B786D68EFE22A13C05053D05CA8611F6A614D318060`) and the matching AAB
is **36,420,355 bytes** (SHA-256
`C8CA4351D4778E5C117F9E9CA29D9C2CEA5C1BFF041718D6175AA7559CF14105`). The checker passes
the offline manifest, ARM64/static 16 KB, and creative-dimension gates. Bundletool 1.18.3
universal extraction, direct/extracted `zipalign -P 16`, and v2/v3 signature verification
also pass. Evidence is indexed in `Docs/V1_RELEASE_PLAN.md` P44.

The exact APK reached menu -> Solo Raja -> Bijli -> live opening on approved Lava
`ST5GDW23LB004392`; the bounded six-sample/30-second capture is raw diagnostic evidence
only. A genuine `BattleRaja_16K` Android 36 emulator reached the same route with
`PAGE_SIZE=16384`. Physical 16 KB, sustained performance, full touch/accessibility,
final signing/identity, privacy/Data Safety, cultural review and Play Console actions
remain open.

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
| 16 KB pages | Static evidence passed; host-GPU Android 16 `BattleRaja_16K` smoke also returned `PAGESIZE=16384` and rendered the exact APK | Re-run the checker after any package/plugin change; repeat on an ARM64 physical 16 KB device and supported GPU profiles before claiming universal compatibility |
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

Use only the approved Lava serial for physical release evidence. Do not use the Oppo phone.
The `BattleRaja_16K` emulator is valid only for the profile-specific 16 KB smoke indexed in
P49; it does not replace physical Lava route, thermal, battery, accessibility or owner review.

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

## Current saved-environment candidate preparation — 2026-08-29

The exact art/runtime source is `ac45479`. It binds the saved textured
`BazaarBastionProduction.prefab`, saved fighter LOD meshes and custom runtime fallback
geometry described in `Docs/V1_RELEASE_PLAN.md` P45. Rebuild both the APK and AAB from the
final reviewed commit before using any older artifact or screenshot as evidence; the older
`bc392fd` candidate hashes in this checklist are historical only. Install and exercise the
new APK on Lava `ST5GDW23LB004392` only, then capture the menu, tutorial, all-fighter route,
gadget/Aandhi flow, results/rematch and raw bounded telemetry. Do not treat the generated
environment/LOD baseline as final art or cultural approval, and do not infer physical 16 KB
runtime support from the 4 KB Lava phone; the P49 emulator result is profile-specific smoke,
not physical or universal 16 KB approval.

## Exact aim-state package refresh - 2026-08-30

The current artifact source is `d0de9499e764045d72dbf092da4c8f2d85fb0b36`. It contains the
dedicated render-only fighter Aim state and passes **141/141 EditMode**, **87/87 PlayMode**,
repository validation **0/0** and the composed release checker **0/0**.

- APK: **40,676,862 bytes**, SHA-256
  `334EC0F8E1F0F2B04CEF52DB44586842E3004E76B28143007FB10EC310B308E9`.
- AAB: **36,502,035 bytes**, SHA-256
  `958792924DA7925474AAB40C9B5A5D588E4776AE756E333D0C8437EF4D5FF086`.
- Manifest: `com.example.battleraja.m11`, version `1.0.0`/code `100`, min/target API `28/36`,
  no network permission, seven ARM64 libraries, static 16 KB alignment passed.
- Bundletool 1.18.3 universal extraction and direct/extracted static zipalign checks passed;
  signature verification is temporary debug signing only.
- Exact Lava route evidence and all screenshot hashes are indexed at
  `Builds/Local/Device/Performance/20260830-lava-d0de949-aim/manifest.json`. Installation,
  launch/resume, all-fighter cards, action/Aandhi/elimination/spectator/results/rematch,
  settings/lifecycle and tutorial completion were observed on `ST5GDW23LB004392` only.

The package is a local temporary-ID technical candidate, not an upload-ready release. Genuine
16 KB runtime, sustained performance/thermal/battery, complete physical action comfort,
final authored/cultural/accessibility approval, release identity/signing, privacy/Data Safety,
IARC/content rating and Play Console actions remain owner-controlled or untested.

## Exact terminal-outcome candidate refresh - 2026-08-30

The current reviewed source is `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`. It saves explicit
gold Victory and red Defeat particle prefabs and routes authoritative result placement to the
render-only fighter presentation. Full EditMode is **141/141**, PlayMode is **88/88** and
repository validation is **0/0**; the PlayMode suite includes winner/defeat persistence.

- APK: **40,673,654 bytes**, SHA-256
  `31D982D7334B08D0DE759CE755784547CFCF843D9CFCFB1DB0E041E7EEE2DF2D`.
- AAB: **36,498,821 bytes**, SHA-256
  `D49E01B63C5106B68133040F03B2D7F11831DEA80E388D32296FCC6B705C20CA`.
- Release checker: **0 errors / 0 warnings**, SHA-256
  `5D704A38FEC64F154382E19593AB0A82F9DB3557DAF2519D8D3852955118409D`; package remains
  temporary `com.example.battleraja.m11`, API 28/36, no network permission, seven ARM64
  libraries, static 16 KB alignment passed, and signer is temporary/debug only.
- Bundletool 1.18.3 APKS SHA-256
  `0D1961247BDACCD88343A36966201F09EB62388FEAFEB648A546E0F3A1691941`; extracted universal
  APK SHA-256 `EB14C2826287A8083B3A2D9D610256B269D367A482F7F12DA8BBDF17DD03B24F`; direct and
  extracted static zipalign checks passed. Full evidence log SHA-256 is
  `2703521F09ACB310D802292F1FA0EF7F23DCF00BC49AE2FBC0C21E008E7BFF30`.
- Approved Lava `ST5GDW23LB004392` installed the exact APK and completed the full local route:
  menu, all three fighter cards, combat/action feedback, Aandhi pressure/closing/final circle,
  defeat/spectating, results/rematch, settings toggles, background/resume and tutorial
  `8/8 COMPLETE` via the in-app SKIP control. The route manifest and screenshot hashes are in
  `Builds/Local/Device/Performance/20260830-lava-5d136fb-outcome/manifest.json`.
- Raw telemetry: **273,885 KB PSS / 385,796 KB RSS / 80,020 KB graphics PSS**, thermal status
  0 and no configured fatal/ANR/SIGSEGV marker. This is bounded evidence only; the phone
  reports 4 KB pages, `gfxinfo` has no usable frame histogram, and no sustained performance,
  runtime 16 KB, accessibility comfort or final-authored-content approval is claimed.

This remains a debug-signed, temporary-ID technical candidate. Final package identity and
signing, privacy/Data Safety, IARC/content rating, cultural/legal/fun/accessibility approval,
genuine 16 KB runtime testing, sustained performance/thermal/battery validation and Play
Console upload/public deployment remain owner-controlled.

## Current release-handoff documentation tip — 2026-08-30 07:05 IST

The current docs-only tip is aligned with `origin/main`. The current owner-selectable release notes, invited-tester quick start, known issues,
support copy and submission checklist are consolidated in
`Docs/RELEASE/V1_RELEASE_NOTES_AND_SUPPORT_DRAFT.md`. Store-creative dimensions and
repository validation remain green; P47-P50 runtime, artifact, Lava and 16 KB evidence is
unchanged. This does not change the temporary package ID/debug signing or close owner,
human, legal, performance or Play Console gates.

## P52 player-facing Umbrella Guard regression - 2026-08-30 07:21 IST

The latest test-bearing tip is `4c4c67cbbc20062e3723cc90ee3bb7c266bbeda4`, based on runtime/art source
`5d136fbb6be6a5554931f6ab859be8b9a8a995a2`. `GadgetPlayModeTests` now directly exercises
player pickup/use of Umbrella Guard and asserts one-slot consumption, configured shield
duration, player feedback, success telemetry, front-facing projectile mitigation and the Aandhi
bypass. Full EditMode is **141/141**, PlayMode is
**89/89**, and repository validation is **0 errors / 0 warnings**; exact hashes are in P52 of
`Docs/V1_RELEASE_PLAN.md`. No runtime artifact, APK/AAB, or device evidence changed, so P47-P50
remain authoritative. The candidate is still temporary-ID/debug-signed; final identity,
signing, human/cultural/accessibility/fun review, performance, privacy/rating and Play
Console gates remain open.

## P53 current exact-source production-bot gate refresh - 2026-08-30 08:03 IST

The current fixed-tick production-bot harness was rerun with Unity `6000.5.6f1`, 100
matches, release assertions enabled, playback scale 50 and base seed 9101. Runtime/art
source remains `5d136fb`; PlayMode passed **89/89**. All **100/100** matches reached
terminal results at **306.0135193 s** each and landed in the 240-360 second window. Every
match had combat and bot-to-bot damage, **0/100** were Aandhi-only, protected-warmup and
invalid-position samples were zero, and Umbrella Guard, Dhol Burst and Tiffin Station were
each used in all 100 matches. The exact XML/log/batch paths and SHA-256 values are indexed
in `Docs/V1_RELEASE_PLAN.md` P53.

This closes the current automated production-bot pacing/safety gate locally. The accelerated
50x path remains separate from same-seed determinism evidence; P10's real-time result remains
the determinism record. The candidate is still temporary-ID/debug-signed and owner gates for
route/accessibility/fun, authored/cultural review, normalized sustained performance, physical
16 KB, signing, legal/privacy, rating and Play Console remain open.

## Exact-candidate P48 performance refresh - 2026-08-30

The exact `5d136fbb6be6a5554931f6ab859be8b9a8a995a2` candidate was exercised on approved Lava
`ST5GDW23LB004392` using the repository performance-capture script for 180 seconds (36
five-second samples). Warm-up-excluded PSS was **261,702-273,769 KB**, RSS
**384,112-396,696 KB**, graphics PSS **75,792-81,936 KB**, and process `top` CPU
**87.5-118.0%** on Android's 100%-per-core scale. Battery stayed at 76% while USB-powered,
thermal status stayed 0, and no configured fatal markers were found. The full raw capture is
`Builds/Local/Device/Performance/20260830-lava-5d136fb-perf2/`; its manifest SHA-256 is
`7728C80ADFEA814D1D9E63D3344C527825CFCF413236AB89131C62C46C2D459D`. A 30-second Perfetto
trace is retained there, but no local trace processor was available; Unity `gfxinfo` still
has no usable frame histogram and Simpleperf cannot sample the non-profileable candidate.
This is stronger raw stability evidence, not normalized sustained performance, battery,
runtime-16-KB or final human approval.

## Genuine 16 KB runtime smoke - P49 - 2026-08-30

The exact `5d136fb` APK installed on the `BattleRaja_16K` Android 16/API 36 AVD with the
host-GPU renderer. The model is `sdk_gphone16k_x86_64`, the ABI list includes
`x86_64,arm64-v8a`, and `adb shell getconf PAGESIZE` returned **16384**. A clean direct launch
rendered the branded menu (`Builds/Local/Device/Performance/20260830-16k-5d136fb/host-gpu/launch-final.png`,
SHA-256 `919BA18BBCA77C4C843DD07EC1470E8D0DFAE4AC3C3F012266E102ACABD55FA0`) and existing
live-match checkpoints rendered normally. The app-scoped launch logcat has no configured
fatal, ANR, SIGSEGV, SIGABRT or shader-link marker.

The 90-second harness capture is under
`Builds/Local/Device/Performance/20260830-16k-5d136fb-host/` with manifest SHA-256
`AC691AF0BB69983AFE0001F87A4AF92543454D3F190C61FB974734A42EE48B61`; warm-up PSS was
**435,726-436,966 KB**, RSS **617,304-621,236 KB**, GraphicBufferAllocator estimate
**31,416 KB**, and process CPU **96.1-123.0%** on Android's 100%-per-core scale. Thermal
status was 0, but the virtual battery and emulator CPU are not product-tier endurance data;
Unity `gfxinfo` still has no usable frame histogram.

This closes only the **host-GPU AVD 16 KB smoke**. A SwiftShader attempt on the same AVD
showed URP/Lit GLSL uniform-limit corruption and is retained as superseded renderer diagnostics
under `Builds/Local/Device/Performance/20260830-16k-5d136fb-route/`. Physical ARM64 16 KB
coverage, other GPU profiles, normalized budgets and human approval remain open.

## Exact-candidate P50 Lava live-match SurfaceFlinger diagnostic - 2026-08-30

The exact `5d136fb` terminal-outcome APK was relaunched on approved Lava
`ST5GDW23LB004392` through Rematch. A roughly 45-second live-match SurfaceFlinger ring-buffer
sample recorded **126 valid present timestamps / 125 intervals** after excluding one
`Long.MaxValue` sentinel. The middle timestamp series measured min/median/p95/p99/max
intervals **16.447 / 16.534 / 16.565 / 33.078 / 33.367 ms**, with one interval over 2×
refresh. Raw evidence is under
`Builds/Local/Device/Performance/20260830-lava-5d136fb-sf/`; summary SHA-256 is
`21369E4FC3BF33BF1DB234BE2F23F1A8D32BD45D0DF29F8682DC90D17489B144` and raw latency SHA-256
is `D83D61790C60E5D76CB9BBC5B0D25CA91D0AD044BC63686DAD417F71942B3D26`.

The end capture remained in the spectator state after player defeat with Aandhi closing;
end telemetry was **277,284 KB PSS / 400,500 KB RSS / 80,052 KB graphics PSS**, battery
**75% / 4,120 mV / 31 C** while USB-powered, thermal status 0 and no configured fatal
markers. This is bounded compositor evidence only. Lava reports 4 KB pages, Unity `gfxinfo`
has no usable histogram, and normalized performance, physical 16 KB and human release
approval remain open.

## Latest exact UI candidate — P58 — 2026-08-31

The current UI/test source checkpoint is `888421f0b332a2e5b9b41fcb6ae669adec836612`.
The completed tutorial card now exposes `CLOSE CARD`, allowing the player to inspect the
underlying Results/REMATCH controls. Full EditMode is **141/141**, PlayMode is **92/92**,
static validation is **0 errors / 0 warnings**, and the exact rebuilt APK/AAB pass the local
release checker with **0 errors / 0 warnings**. APK SHA-256 is
`B3D4EF4749270FDAD30474113683E050693BFA013173FF5EB1E3848C26C87F44`; AAB SHA-256 is
`CC5D2B362EA8330BB3FA22E93D530CD018D4933305744E26EF2504300B88D6F6`. These remain
temporary Android Debug-signed artifacts with package `com.example.battleraja.m11`, version
`1.0.0`/code `100`, API `28/36`, no network permissions, ARM64 payload and static 16 KB
alignment.

The exact APK was installed only on approved Lava `ST5GDW23LB004392`. Real touch used the
in-app SKIP route to reach `TUTORIAL COMPLETE 8/8`, tapped `CLOSE CARD`, reached Results and
REMATCH, and observed a fresh TutorialArena opening after REMATCH. The route manifest and
screenshots are under `Builds/Local/Device/final-circle-20260830/`; this is bounded route
evidence, not action-by-action tutorial, repeated-rematch, accessibility, final art/audio,
physical 16 KB, normalized performance or human approval. See P58 in
`Docs/V1_RELEASE_PLAN.md` for exact hashes and remaining gates.

## Latest exact-candidate all-fighter/accessibility evidence — P59 — 2026-08-31

The unchanged P58 candidate was exercised further on approved Lava
`ST5GDW23LB004392` only. Fresh captures cover the Bijli, Maya and Pehel live openings,
attack and ability-input checkpoints, Tiffin Station feedback, in-match Settings &
Accessibility, left-handed control layout, reduced flashes, high contrast, aim assist,
text scaling, persistence and reset-to-defaults. The connected Oppo was excluded. The
machine-readable route index is
`Builds/Local/Device/final-circle-20260830/p58-fighter-accessibility-route-manifest.json`
(7,285 bytes; SHA-256
`F9D43C679971029EC9CC8881913A0BF62A28555A2F7C14C7A1FB94554C7D2409`).

The app-scoped logcat and UI-tree files are retained beside the screenshots. The UI tree is
SurfaceView-only, and the log has no configured app fatal/ANR/SIGSEGV/SIGABRT marker; known
Lava gralloc/AHardwareBuffer, Unity Play Core class-probe and Swappy diagnostics remain
recorded. This is bounded device observation, not action-by-action tutorial comfort,
repeated-rematch approval, normalized performance, physical 16 KB proof, final authored
content approval or Play Store approval. The project remains a prototype / Android offline
release candidate in progress.

## Latest exact-candidate P66 lifecycle input evidence — 2026-08-31

The exact rebuilt source tip is `e603ce7e7f1cb279f5e3e9d606ea5eae89603ecb`. Focused lifecycle
PlayMode is **1/1**, full EditMode is **141/141**, full PlayMode is **92/92**, and static
validation is **0 errors / 0 warnings**. The candidate APK/AAB hashes and checker output are
recorded above and in P66 of `Docs/V1_RELEASE_PLAN.md`.

On approved Lava `ST5GDW23LB004392`, the exact APK reached a live Solo Raja/Bijli Opening Fight,
was sent to Android HOME for approximately five seconds, and returned to the same RESUMED
`UnityPlayerGameActivity`. Paired captures retain `ALIVE 8` and `ZONE 14.0 > 11.0`; the
device reports Android 14/API 34 and 4 KB pages. Manifest:
`Builds/Local/Device/final-circle-20260830/p66-lifecycle-route/p66-lifecycle-route-manifest.json`
(8,192 bytes; SHA-256
`217589DAE7592EC397328F12D8C3DF88246B7AEE035776584DF5FE9624499103`). Route logcat has no
configured fatal, ANR, native-crash or managed-exception marker. This is bounded exact-device
evidence. Bundletool 1.18.3 universal extraction plus direct/extracted `zipalign -P 16` and
temporary v3 `apksigner` verification passed for the exact AAB; physical 16 KB, all-phase
lifecycle, held-input comfort, endurance and owner/Play gates remain open.

## Latest exact-candidate P65 all-gadget route evidence — 2026-08-31

The exact P61 APK (`f80b565372d7446e070cf1a37de042bd018345c4`) remained installed on approved
Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). No rebuild or source change was
made for this route. Manifest:
`Builds/Local/Device/final-circle-20260830/p65-gadget-route/p65-gadget-route-manifest.json`
(4,269 bytes; SHA-256
`48598855AEEDCD286C837219632ACFC6B972CEC0CB69B7E9D8EE28163BDED807`).

All three gadgets were physically collected and used. Tiffin deployment is shown in
`13-tiffin-aimed.png` (SHA-256
`91928CA0C0FB3A9957DCDDF402A758C98C4460A98BF0D09237AD6CABE88A8748`); Dhol pickup/use in
`18-left-to-dhol.png` (SHA-256
`FD8C97861217A2A5AFBFD6C537954079BC00A328C15B392D58E52B8FBE5D7F4F`) and
`19-dhol-use.png` (SHA-256
`C67D9FCBB8F873038598A4F92446AE7B40E9C61DF50B9F14906197BD85CD50E8`); Umbrella
pickup/use in `23-diagonal-left-progress.png` (SHA-256
`EAD710DF6318380F8BCB8CBD04118BD8E4B3A05BE7B62672EF3BD20BC4E331F7`) and
`24-umbrella-use.png` (SHA-256
`B35585DE8FC7A2448413502FCCBB2C2A139D6E2127BCB7DEE2DF836D4FCD0FC8`). Visible feedback
was `TIFFIN STATION DEPLOYED`, `DHOL BURST` and `UMBRELLA GUARD ACTIVE`.

This is bounded physical route evidence, not action-by-action tutorial comfort, final
presentation/audio/cultural approval, endurance, normalized performance, physical 16 KB or
Play approval. App-scoped logcat SHA-256 is
`7237171A826BE8F5308B871270F1477551296A903F5E2B5DC626669DAE113E9F` with zero configured
fatal/ANR/native/managed-crash markers.

## Latest exact-candidate P64 immediate-after-resume lifecycle evidence — 2026-08-31

The exact P61 APK (`f80b565372d7446e070cf1a37de042bd018345c4`) was started on approved Lava
`ST5GDW23LB004392` after tutorial completion. A live Opening Fight baseline was captured,
Android HOME was held for approximately five seconds, and the Unity activity was resumed with
a screenshot approximately 220 ms later. The route manifest is
`Builds/Local/Device/final-circle-20260830/p64-lifecycle-pause-manifest.json` (3,354 bytes;
SHA-256 `DC3677479D95C4E2EBA7DD79C6E46C03418D58F379AE32708A1C5B2FFCB4EA99`).

The paired captures show `BIJLI HP 85/85`, `OPENING FIGHT`, `ALIVE 8` and `ZONE 14.0 > 11.0`
both before and immediately after resume; the warning countdown is 11.6 seconds before HOME
and 10.8 seconds after the 220 ms resume capture. The trace and app logcat record
`APP_CMD_PAUSE/STOP` followed by `APP_CMD_START/RESUME` on the same process. This is bounded
pause-invariant evidence; repeat across match phases and held-input cases for final comfort
approval. It is not full lifecycle/endurance, normalized performance, physical 16 KB or Play
approval.

## Latest exact-candidate P63 bounded live-match performance evidence — 2026-08-31

The exact P61 APK (`f80b565372d7446e070cf1a37de042bd018345c4`) remained installed on approved
Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34) during a requested 120-second
performance-harness run at five-second intervals. Movement swipes plus attack, ability and
gadget taps were sent while the match advanced from Opening Fight to Final Circle, player
defeat/spectating and Alive 4. No data was cleared and no artifact was rebuilt. Raw evidence:
`Builds/Local/Device/Performance/20260831-lava-f80b565-p62-perf120/`.

The 24-sample `manifest.json` is 5,611 bytes (SHA-256
`EBB5C4F43E27E0579E0EAC3E116E8A3DB0F47DEB35EAEDEE7C05A945FF067D7D`); the checked summary is
`p63-performance-route-summary.json` (3,478 bytes; SHA-256
`4413810D5295345586E497846AA4A55C6B83AC9645FA807EB159ADDEA6B4467B`). Warm-sample PSS was
223,078-223,273 KB (average 223,190.8 KB), RSS 356,972-357,168 KB (average 357,085.0 KB),
graphics PSS 17,480 KB, and instantaneous `top` CPU 59.2-131.0% (warm average 113.8%).
Thermal status stayed 0; HAL CPU/GPU was 43.537-49.148 C and skin 39.305-39.969 C. The
phone was USB-powered and battery changed from 44% / 3,841 mV / 35 C to 43% / 3,825 mV / 35 C.
Logcat (1,058,413 bytes; SHA-256
`139ED06981B4524B4C288A47DC0FBDF9859F8DAC2767024C4A3ED252F80112B2`) has no configured
fatal/ANR/SIGSEGV/SIGABRT/managed-exception marker.

This is raw bounded telemetry, not a performance pass: Unity `gfxinfo` reports zero frames
and no usable SurfaceView histogram, Lava reports 4 KB pages, and normalized frame-time,
GC/GPU, unplugged endurance, repeated-match growth, physical 16 KB and owner approval remain
open.

## Latest exact-candidate accessibility, persistence and lifecycle evidence — P62 — 2026-08-31

The exact P61 APK (`f80b565372d7446e070cf1a37de042bd018345c4`) was freshly installed on
approved Lava `ST5GDW23LB004392` after clearing app data. The route opened in-match Settings &
Accessibility, exercised left-handed controls, reduced flashes, high contrast, aim assist and
text scale, observed the left-handed live action layout, verified persistence after return and
relaunch, and restored defaults. The route manifest is
`Builds/Local/Device/final-circle-20260830/p61-accessibility-route/p62-accessibility-route-manifest.json`
(8,901 bytes; SHA-256
`0DAE55EBFC6DD57F78D9BF28D0A9172659102FFB2D30BFBB2ADC8AB610D4BCF9`). It also records a
bounded live background/resume return; the warning timer moved during that interval, so full
simulation pause invariance is not claimed.

The route's UI tree is SurfaceView-only (`p61-accessibility-ui.xml`, 2,546 bytes; SHA-256
`A8235CD2CFEE7BFCFB0A515F9337E4ABF6E6C16C10ACDCF446DE87E9AEF094BD`). Its app-scoped logcat
is 24,935 bytes (SHA-256
`2AA7EA1065F8FA035226E760454961ACC9BE770ACBDA9B7A003F5037ED278C27`) with no configured
fatal/ANR/SIGSEGV/SIGABRT marker; known Lava graphics, Play Core and Swappy diagnostics remain
recorded. Smaller-device coverage, accessibility comfort, sustained lifecycle testing and
human approval remain open.

## Latest exact-candidate compact-results evidence — P61 — 2026-08-31

The P61 runtime checkpoint `f80b565372d7446e070cf1a37de042bd018345c4` keeps the P60 `ZONE`
label and replaces compact result rows' ambiguous `K/A/D` sequence with `KOs`, `AST` and
`DMG`; portrait result type rises from 16 px to 18 px. Full EditMode (**141/141**) and
PlayMode (**92/92**) remain green, repository validation and the release checker report
**0 errors / 0 warnings**, and the rebuilt temporary-ID APK/AAB fingerprints are recorded in
`Docs/V1_RELEASE_PLAN.md` P61.

The exact rebuilt APK installed successfully only on approved Lava
`ST5GDW23LB004392`. The route reached live match → player defeat/spectator → Aandhi/final
circle → Results → REMATCH. The result capture
`Builds/Local/Device/final-circle-20260830/p60-results-copy-route/05-results.png` is 301,689
bytes (SHA-256
`313F180C6177C5A78F80B68D115C0E52E2E44C3FDB79CA157737B62BADC79676`) and visibly shows the
expanded metric labels. `05-rematch-opening.png` is 322,280 bytes (SHA-256
`2137A249A70D9005A563AA259652F672D182C132B96069B21ED2DBB731D2FF26`). The route index is
`Builds/Local/Device/final-circle-20260830/p61-results-copy-manifest.json` (4,936 bytes;
SHA-256 `0C868F852FE57C409B914871845DF317EFC7C89398CEFD3A8AB98E5F1137671F`).

The same rebuilt candidate has a separate all-fighter manifest
`Builds/Local/Device/final-circle-20260830/p61-all-fighter-manifest.json` (5,506 bytes;
SHA-256 `9FA8762B504330189A686605A6DD60836C4992B5359E526B91E8527A813E1598`). It records
Bijli, Pehel and Maya selection/live/attack/Tiffin checkpoints plus Opening-phase ability
feedback. This is bounded action observation; accessibility comfort, action-by-action tutorial
comfort and human approval remain open.

This closes the exact-candidate compact-results copy observation only. Smaller-device,
localization, final visual, all-fighter action-by-action comfort, sustained performance,
physical 16 KB, release signing, legal/privacy and human approval remain open.

## Latest exact-candidate compact-HUD copy evidence — P60 — 2026-08-31

The P60 runtime checkpoint `c3cfb27e08f13ecf4b91a4234269aa11e675bfe9` replaces the compact
portrait HUD's ambiguous `Z` abbreviation with the player-facing `ZONE` label. Full
EditMode (**141/141**) and PlayMode (**92/92**) remain green; repository validation and the
release checker both report **0 errors / 0 warnings**. The rebuilt temporary-ID APK is
40,682,347 bytes (SHA-256
`4EFF24C7251DD57C2FCAA4D280C369175D33FA6C8D26B969ABBAA72D9EAF32A7`) and the AAB is
36,507,651 bytes (SHA-256
`D60B09EE6324C0AA75781BF1F9DB8461A6A1AE05D788A9232EA227DBC1349936`).

The exact APK installed successfully only on approved Lava `ST5GDW23LB004392`; the fresh
portrait live capture shows `GET READY` and `ALIVE 8  ZONE 14.0 > 14.0`:
`Builds/Local/Device/final-circle-20260830/p60-live-zone-copy.png` (324,315 bytes;
SHA-256 `13AEFABE9A51364B28B85B6293B2237D6D7189C32278863E591964C252FE8A3D`). The route
index is
`Builds/Local/Device/final-circle-20260830/p60-zone-copy-manifest.json` (3,822 bytes;
SHA-256 `B235CAC4A041644B7A05FED6C613A5BB2563CDD6929C19EF9E2B6F445F1C7E39`). This is
bounded readability evidence on a 1080x2460, 4 KB-page device; smaller-device,
localization, final visual and human comfort approval remain open.
