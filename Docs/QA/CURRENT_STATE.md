# BattleRaja V1 current-state index

Updated: 2026-09-04

## Evidence location policy

All new builds, logs, screenshots and test reports must remain under the ignored
`Builds/Local/` tree inside `C:\Projects\BattleRaja`. Historical append-only
notes may mention retired disposable paths for provenance, but no new work should
create or depend on evidence outside this repository root.

## Truthful classification

**Prototype — Android offline release candidate in progress.**

## Latest current-source evidence — 2026-09-04 — visual polish and menu presentation

Source commits `775497d` and `281eeb4` add saved faceted shoulder, gauntlet and leg
armor meshes to the Bijli, Pehel and Maya production prefabs and expand the original
Bazaar Bastion feature artwork to fill the menu card. Source `4ebf65f` then corrects the
presentation adapter to mirror the confirmed post-respawn authority snapshot instead of
reapplying the stale zero-health ready state; Bastion domain rules, colliders, hitboxes,
replay state and input contracts remain unchanged.

Static validation is **0 errors / 0 warnings**. Full EditMode is **164/164** (final
menu-layout XML SHA-256
`5D43596B6246CFF916B83A43459728537CBADD5B8947DB33CABE37F7EBE5D5DF`) and the
respawn-handoff rerun remains **164/164** (XML SHA-256
`1F744F11D3C3A71F2A26AE16A2E4B1E88D6147226675657D9A1A0EAC2AE60C53`). Full PlayMode is
**99/99** (XML SHA-256
`67E02CE34AFB41222D56A8A4633392FDC9716CB8A7D40AD4D1228A34669D93B4`). The corrected
candidate APK is 41,683,648 bytes (SHA-256
`6A16D07EBA66C7420E5F1AABD7982E27C40C6BB017FC639E2D87974B85DE60DC`) and the AAB is
37,509,156 bytes (SHA-256
`337C15FF7169A97FED2F711822C5366BF731A388D954C8778A9BF33A9E4DB9DA`). The post-commit
technical checker passed **0 errors / 0 warnings** for temporary package
`com.example.battleraja.m11`, API `28/36`, offline permissions, ARM64/static alignment
and store dimensions; its log is
`Builds/Local/V1GameplayTruth/Next/respawn-fix-20260904/release-checker.log` (SHA-256
`647D3B48D0F1C9C86FA626F48E62D0AAFB1497450E4555058D70BCDED107E4E5`).

Approved Lava `ST5GDW23LB004392` installed the corrected APK and the pulled base hash
matches. The focused route captured a terminal `BASTION CROWN RESULTS` card, Bijli at
`0/85` with `OUT OF ACTION • respawn or spectate an ally`, and the same player card at
`85/85` after the confirmed respawn. Its scoped logcat has zero configured crash,
native, shader or managed-exception markers. Lava reports 4 KB pages. The broader
current-source route also selected Pehel and Maya, visibly deployed Tiffin, exercised
in-match accessibility toggles, HOME/resume and Rematch, and reset to a fresh 00:04
match with all eight fighters. Full evidence and hashes are in
`Docs/QA/V1_VISUAL_POLISH_2026-09-04.md`. The earlier bounded 30-second diagnostic
observed settled PSS 248–259 MB, RSS 371–382 MB, graphics PSS 75–80 MB, instantaneous
CPU 39–62% and thermal status 0, but no usable frame histogram; no normalized FPS,
GPU, GC or endurance claim is made. The exact candidate also launched on the local
`BattleRaja_16K` Android 16/API 36 AVD with `getconf PAGESIZE=16384` and zero configured
crash/native/shader markers; this supplements but does not replace physical Lava proof.

This closes a presentation, bounded lifecycle and respawn-mirror checkpoint only.
Commissioned final art/audio, spectate-camera interaction comfort, complete all-fighter action coverage,
accessibility/cultural/fun review, normalized sustained performance, genuine 16 KB
runtime, final identity/signing and owner Play approvals remain open.

## Latest current-source evidence — 2026-09-04 — authoritative respawn handoff

Source commit `0d0f875` adds an explicit authority confirmation to the Bastion Crown
respawn handoff. A ticket is spent once when the timer expires, the actor remains dead
and spectating until the application confirms the successful respawn, and retries emit
the same actor without double-spending. Mirror-side revival, pre-live/terminal damage,
and stale pending state are rejected. The issued marker participates in the deterministic
hash; Solo rules, Crown scoring and fighter balance are unchanged.

Static validation is **0 errors / 0 warnings**. Focused Bastion tests are **16/16**,
full EditMode **164/164**, full PlayMode **98/98**, and strict production-bot PlayMode
**98/98**. The 100-match report recorded **100/100** terminal matches, **89/100** in the
240–360 second window, **92/100** with combat eliminations, bot-to-bot damaging pairs in
**100/100**, **0/100** Aandhi-only resolutions, zero protected-warmup damage, zero invalid
positions, **284** respawns, and **0** stuck ticks. Exact XML/report hashes and the
authority regression details are in
`Docs/QA/V1_RESPAWN_HANDOFF_AUTHORITY_2026-09-04.md`.

The rebuilt temporary-ID candidate is an APK of 41,681,228 bytes (SHA-256
`0A4E7C96531F16ABAFDB4BDFB2CD587175360210B543FADEC19BF9B06DB91108`) and an AAB of
37,506,760 bytes (SHA-256
`19E0B84A8CACB760CA18DFDD8FC7AA3B5AE9232FB7F4E52F47A22B28DA6E842E`). The technical
checker is **0 errors / 0 warnings** for `com.example.battleraja.m11`, version `1.0.0`
code `100`, API `28/36`, offline permissions, ARM64/static alignment and store
dimensions.

Approved Lava `ST5GDW23LB004392` installed the exact APK; the pulled base hash matches,
the branded menu rendered, and the scoped logcat has zero configured fatal/ANR/SIGSEGV/
SIGABRT markers. Lava reports 4 KB pages, and this is bounded menu evidence only—not a
complete current-source physical route, genuine 16 KB runtime proof, normalized
performance/endurance approval or human review.

## Latest current-source evidence — 2026-09-04 — coarse Crown timer determinism

`BastionCrownMatch.AdvanceCrown` now preserves overdue time when one advance crosses
multiple objective rotation intervals, so coarse diagnostic steps and fixed-step replays
land on the same Crown socket and timer. Source commit is `bad12de`; combat, scoring,
respawn, squad cadence and Solo rules are unchanged.

Static validation is **0 errors / 0 warnings**. Full EditMode is **162/162** (XML SHA-256
`5C172CD9B52C598277D3C00F43A276D0A08FF5DA4FCE276C2C326F9C1C3892C1`) and full PlayMode is
**98/98** (XML SHA-256
`108D32758C5C0D783011FD7C4F6691684D6E0279CB9157FBB46BBCD80FACE855`). The fresh exact
APK is 41,680,452 bytes (SHA-256
`E92E5994C36B35414DB44D32C082DC8992A3E413F9B67BD87FF776BF5C42DF6C`) and the AAB is
37,505,982 bytes (SHA-256
`19882B28E14DE5D9A0B73CCF7016FCA0983325C1F93C4E4BDD36D7E908FB470F`). The release
checker is **0 errors / 0 warnings** with the temporary package identity.

Approved Lava `ST5GDW23LB004392` installed the exact APK after clearing the package, and
the pulled base matches the APK hash. Fresh menu and process-scoped crash-marker evidence
are under `Builds/Local/V1GameplayTruth/Next/crown-rotation-20260904/`. This bounded smoke
does not claim complete physical 4v4 comfort, normalized performance, genuine 16 KB runtime
or owner approval. The evidence record is
`Docs/QA/V1_CROWN_TIMER_DETERMINISM_2026-09-04.md`.

The canonical AI/replay metrics remain carried forward; this timer fix only makes objective
rotation step-size behavior explicit and does not establish final fairness or fun.

## Latest current-source evidence — 2026-09-04 — squad command-window determinism

The Bastion squad blackboard now keeps one shared snapshot for the entire controller-owned
bot callback window. Callback-side state mutations are deferred to the next preparation
tick, while pure-domain callers outside a command window can still force an immediate
refresh. This closes a deterministic coordination edge case without changing combat,
objective scoring, respawn rules or the legacy Solo path. Source commit is `8e3563a`.

Static validation is **0 errors / 0 warnings**. Full EditMode is **161/161** (XML SHA-256
`8B4DCC3B571FC51AADC646604F5B875398861890E4A84EC2F152C4EE18DF892A`) and full PlayMode is
**98/98** (XML SHA-256
`B3FE89180E76435A1912733EF00750DD334A2C9770472B1F6C2E9ED72B40BEA5`). The fresh exact
APK is 41,680,960 bytes (SHA-256
`976EE4D767DC4BC88DB9EB3D499603515D576DF9A205E4E07BF1D87A1CBAA43A`) and the AAB is
37,506,508 bytes (SHA-256
`CE06B7B8C9CA9B67D8AF4796FD6360CEF4430B539BF34F379BC32D9E5F1ECF8F`). The release
checker is **0 errors / 0 warnings**; the refreshed candidate remains temporary-ID and
debug-signed.

Approved Lava `ST5GDW23LB004392` installed the exact APK after a package clear, and the
pulled base matches the APK hash. The fresh menu capture and process-scoped crash-marker
log are under `Builds/Local/V1GameplayTruth/Next/squad-window-20260904/`. This bounded
smoke does not claim a full physical 4v4 route, normalized performance, genuine 16 KB
runtime or owner approval. The evidence record is
`Docs/QA/V1_SQUAD_COMMAND_WINDOW_2026-09-04.md`.

The canonical 100-match bot/replay metrics remain carried forward because this change only
affects same-tick squad snapshot cadence; no new balance or fun claim is inferred. Final
physical AI fairness, authored presentation, accessibility, performance/endurance,
identity/signing, privacy/Data Safety, IARC, cultural review and Play Console gates remain
open.

## Latest current-source evidence — 2026-09-04 — tutorial safety and GitHub validation repair

The tutorial now uses a dedicated rule definition with automatic timeout and
last-participant resolution disabled. `TutorialOverlay` resolves through the authority only
when the user advances into the Victory lesson, preventing Results from appearing while a
guided lesson is still waiting. Solo and Bastion behavior is unchanged. The repository
workflow's POSIX secret scan and Node 24-compatible checkout path are green on the final
source commit [`a7ea3ce`](https://github.com/neonvarun/BattleRaja/commit/a7ea3ce), run
[`33836993117`](https://github.com/neonvarun/BattleRaja/actions/runs/33836993117).

Static validation is **0 errors / 0 warnings**. EditMode is **160/160** (XML SHA-256
`095E4483D76F97FC0053969C91585DFBA40B5F6841FA75DCBD5EDF5550A54D7D`) and PlayMode is
**98/98** (XML SHA-256
`2FF73B300915CB2198111EDDEBBAD62EB9FE5EEEECA707A4A7241D7F0F3AB808`). The exact APK is
**41,680,960 bytes** (SHA-256
`36AEBACF19F098D3F5763539CBB854C1A0BE6E4F8ADB3CC38BF6171E0856CB0D`) and AAB is
**37,506,488 bytes** (SHA-256
`1D26950F1C85A4F97FD7BB5E2D0938D906EF1DC03D1B7408A4BE77B6282C730A`). The clean
release-checker log is
`Builds/Local/V1GameplayTruth/Next/release-checker-tutorial-safety-20260904.log` (SHA-256
`B4363F769459F7E7783212C7AC5E691E140B39F4A32246C45AE18F0988A91265`).

Approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, `1080x2460`, reported
4 KB pages) installed the exact APK after a package clear. Real touch reached movement,
aim, attack, ability and the live gadget lesson; the route captures are under
`Builds/Local/V1GameplayTruth/Next/tutorial-safety-20260904/`. The SKIP path still shows
the dismissible `TUTORIAL COMPLETE 8/8` card. Full post-fix physical pickup, elimination,
Victory and Results comfort was not completed in this bounded pass, so no such claim is
made. The full evidence record is
`Docs/QA/V1_TUTORIAL_SAFETY_AND_CI_VALIDATION_2026-09-04.md`.

This closes a reproducible tutorial/CI defect, not the product gate. Generated baseline
art/audio, full physical comfort/accessibility/lifecycle review, normalized performance /
endurance, physical 16 KB runtime, permanent identity/signing, privacy/Data Safety,
IARC/content rating, cultural review and Play Console actions remain open.

## Latest current-source evidence — 2026-09-04 — touch-control clarity pass

The production mobile controls now use compact portrait sizing and original vector glyphs
inside each action/twin-stick surface: move arrows, aim crosshair, attack bolt, ability burst
and gadget parcel. `BattleRajaTouchGlyph.cs` is a render-only uGUI graphic; input pointer
ownership, command routing and authority are unchanged.

Static validation is **0 errors / 0 warnings**. EditMode is **159/159** (XML SHA-256
`7C2A2BF9CD76422E8CA454DDF3CD6CDD1D92E888E1EB8298DC29349B87A92F8E`) and PlayMode is
**95/95** (XML SHA-256
`0D2A47CF1EE234545496658DFA19F3D7A76B2934B22D5A02C76A447D09AC4AFB`). The exact APK is
**41,668,888 bytes** (SHA-256
`8B9D11BFDB40A75D7C301A255B71D74516BD83F7D5672730FFFFA34A635E9C71`) and AAB is
**37,494,403 bytes** (SHA-256
`F877BF07F6CBBCF890DB2968B5D48CBB93FC64CCB2DC10D4CCA709679EC99BBC`). The release
checker passes API 28/36, offline permissions, ARM64/static 16 KB alignment and store
dimensions.

Approved Lava `ST5GDW23LB004392` (`1080x2460`, Android 14/API 34, 4 KB pages) has the exact
APK installed and the pulled base matches its SHA-256. Fresh menu/briefing/fighter-select/
live/settings/action captures are in `Builds/Local/V1GameplayTruth/Next/touch-glyph-final2/`.
The action probe reached the expected Tiffin invalid-placement edge state; process-scoped
logcat has **0** configured fatal markers (SHA-256
`3CD27C11D6A63BF6EE08862EA0828F9B7C1E51DA03A7E506F2B807C67B4D98CF`). The six-sample/
30-second capture under `Next/performance-touch-glyph-final2-20260904/` reports PSS
**296,208–302,448 KB**, RSS **408,496–422,188 KB**, graphics PSS **87,528–93,684 KB**,
raw app CPU **97–140%** (mean **113.3%**), battery **69% → 69%**, thermal status **0**.
SurfaceFlinger returned no frame timestamps, so it is retained only as a limitation artifact.

This is a player-facing clarity improvement, not final commissioned art/audio or proof of
complete physical comfort/accessibility. The candidate remains **Prototype — Android
offline release candidate in progress**; final identity/signing, physical 16 KB runtime,
normalized GPU/GC/endurance, full tutorial/all-fighter/lifecycle/accessibility review and
owner privacy/Data Safety/IARC/cultural/Play approvals remain open.

## Latest current-source evidence — 2026-09-04 — settings surface clarity pass

The menu and in-match pause settings now use original icon-backed tiles and accent rails,
with explicit ON/OFF labels for every accessibility toggle and a redundant high-contrast
state cue. `BattleRajaSettingsGlyph.cs` is render-only; existing uGUI targets, preference
keys, lifecycle pause behavior and gameplay authority are unchanged.

Static validation is **0 errors / 0 warnings**. Fresh EditMode is **159/159** (XML SHA-256
`F8C9BF60F77873E9D906E2D0E8726946A58B8593C36CCCD845C086234A85914C`) and PlayMode is
**96/96** (XML SHA-256
`3BE1BA5E2EA8887C4D52DC8F11AE3FD0D64921DB3960409DE30AF9080AAAAC4B`). The exact APK is
**41,678,776 bytes** (SHA-256
`714ACE23E8C9DA859B91B14E12F9E7E65CA277ADAAAB315F1C81B4547D195C93`) and AAB is
**37,504,322 bytes** (SHA-256
`74F7EDC96481EA868FF1A8F078E70D6C407126AD9A197BF2020D044F108445CC`). The technical
checker passes API 28/36, offline permissions, ARM64/static 16 KB alignment and store
dimensions; the clean-tree checker log is
`Next/release-checker-settings-polish-final-clean-20260904.log` (SHA-256
`E473003AB9CD043A637AE878B01772609EABB28A7AC7DA6F23156C7127522FF4`).

Approved Lava `ST5GDW23LB004392` (`1080x2460`, Android 14/API 34, 4 KB pages) has the
exact APK installed and the pulled base hash matches. Fresh menu/menu-settings/live/
pause-settings/high-contrast captures are in
`Builds/Local/V1GameplayTruth/Next/settings-polish-final-20260904/`; the scoped logcat
contains **0** configured fatal markers. The exact live six-sample/30-second capture in
`Builds/Local/V1GameplayTruth/Next/performance-settings-polish-final-20260904/` reports
PSS **298,369–304,625 KB**, RSS **417,808–424,064 KB**, graphics PSS **89,324–95,480 KB**,
raw app CPU **106–127%** (mean **116.3%**), battery **63% → 63%**, thermal status **0**,
and zero configured fatal markers. `gfxinfo` returned no frame timing data, so no FPS claim
is attached. This is a UI-state and bounded device evidence pass, not final commissioned
art/audio, normalized GPU/GC/endurance, physical 16 KB runtime, full physical comfort,
or owner privacy/Data Safety/IARC/cultural/Play approval.

## Latest current-source evidence — 2026-09-04 — hero silhouette and portrait framing pass

The refreshed presentation pass adds connected neck collars, chest plates, limb segments,
waist bands, knee guards and fighter-specific signature weapons to the saved Bijli, Pehel
and Maya render-only prefabs. The meshes are authored in
`Assets/BattleRaja/Editor/ProductionArtBuilder.cs`; the existing presentation rig remains
the only animation parent and the prefabs still own no colliders, authority or gameplay
state. `TopDownCameraController` now uses a **1.6** portrait framing cap multiplier so the
Bastion plaza and fighters occupy more of the tall-phone viewport.

Static validation is **0 errors / 0 warnings**. Fresh EditMode is **159/159** (XML
SHA-256 `CAFECE2084FCB0A73980F51FB486125F2BA9EAE04C24579ADD125D377296827B`) and PlayMode
is **95/95** (XML SHA-256
`1D8CA1AD5C09C82880569C4D9F6AA49BF019D988267CB0E069567FC9F13AE6D9`); the earlier
framing run is also **95/95** (XML SHA-256
`A66ABE558015325098C24EF7486060924FC84E240C71BEC1B7800B62CEDA68A0`).

The exact rebuilt APK is **41,667,212 bytes** (SHA-256
`675945B1E3CB7C1471CE7C65C299B17A0969104C969D57EE5083F608436FFA04`) and the AAB is
**37,492,707 bytes** (SHA-256
`FC0A070E204F3F8C788A38E72A66C70934162382E2B7FC74CA8FB18844C72556`). The release
checker passed target SDK 36, the offline network-permission gate, ARM64-only native
libraries, static 16 KB ELF alignment and store-creative dimensions.

Approved Lava `ST5GDW23LB004392` (`1080x2460`, Android 14/API 34, 4 KB pages) has the
exact APK installed; the pulled installed base hash is the same
`675945B1E3CB7C1471CE7C65C299B17A0969104C969D57EE5083F608436FFA04`. Fresh rendered
evidence is under `Builds/Local/V1GameplayTruth/Next/hero-framing-20260904/`: menu
(`menu.png`, SHA-256 `0F24911929B8C99AA6D9D0C4FC6B4E45F5055167E25762B1E571369501C396E6`),
fighter select (`fighter-select.png`, SHA-256
`7C49892FFABA807F951A8E84DF28C606EF2959E4AE6D6BB18067B14523008BA`), live 4v4
(`live.png`, SHA-256 `92B09898FA4CBD3A1C1583A5B9F15A6DC581831C93D011EC9DA81B868AEA9E6A`)
and rematch (`rematch.png`, SHA-256
`B8954CF43302A56B669398AC71D91B5D5DF1AD6AC5E1349DF8A9AEECC994DA98`). The frame now
shows the arena and role silhouettes at a larger scale; this is a visual readability
improvement, not final commissioned art approval.

A bounded six-sample/30-second live capture is under
`Builds/Local/V1GameplayTruth/Next/performance-hero-framing-20260904/` (manifest SHA-256
`38506BBAA2DA6BA79B5654602899B9F7BE715CE9C08263276DF29DEC83C0AC2E`). It reports PSS
**298,267–300,427 KB**, RSS **410,240–419,944 KB**, graphics PSS **89,288–93,392 KB**,
raw app CPU **81.8–138%**, CPU/GPU **41.993–42.987 C**, battery **74% → 74%**, thermal
status **0** and no configured fatal markers. The SurfaceFlinger diagnostic on this run
did not provide valid per-frame timestamps (zero-filled ring entries), so no new FPS
claim is made here; prior bounded compositor captures remain historical diagnostics.

The candidate remains **Prototype — Android offline release candidate in progress**.
Generated presentation baseline, full all-fighter/tutorial/accessibility/lifecycle comfort,
normalized GPU/GC/endurance, physical 16 KB runtime, permanent identity/signing and
privacy/Data Safety/IARC/cultural/Play owner gates remain open.

The offline product loop, authority/replay foundation, original procedural/vector
presentation kit, tutorial action gates, settings surfaces and Android packaging
tooling exist. A Play Store Release Candidate claim is not yet justified because
physical Lava action-by-action route review, sustained performance, final identity/
signing, accessibility, legal/privacy/cultural approval and Play Console review
remain open.

The latest continuation additionally hardens `TutorialArena` to use the authoritative
movement/combat/pickup path, with a tutorial-only outside-zone safety cadence and forgiving
opening gadget radius. The exact candidate APK is **41,520,532 bytes** (SHA-256
`56F3BAB99E304A15548D8073BA6B41EDDCBDE17A2C7476D923B06094D5A9649E`) and the matching AAB
is **37,346,030 bytes** (SHA-256
`19E2E7CCFFD7B2CBA993DE3608D8D62F4A351425AA76D0085138C1DF6DD96BCA`). Post-change static
validation is **0/0**, EditMode **159/159** and PlayMode **94/94**. Fresh approved-Lava
evidence is under `Builds/Local/V1GameplayTruth/Next/lava-tutorial-20260902/` and reaches
the authoritative elimination and Victory tutorial cards. The scene remains a legacy
MovementLab/Solo tutorial layout rather than a dedicated 4v4 Bastion tutorial, and full
results/rematch comfort remains open.

The final visual-fix continuation regenerated the three production fighter prefabs with
camera-facing Bijli/Pehel/Maya identity accents (including the Maya mask), then rebuilt
the exact APK/AAB. The current APK is **41,516,076 bytes** (SHA-256
`6050E1A6EC329F27BC14A1118FB166D278293237B4BC6CBA716B7B700D9FD6FF`) and the AAB is
**37,341,603 bytes** (SHA-256
`A2F440649987A8FA04398B629F956AC44267AA1D33FEF571C3264B97051CCB4C`). The current
approved-Lava route is under
`Builds/Local/V1GameplayTruth/Next/lava-release-final-20260902/` and reaches the live
Bastion match, authoritative results and rematch on the installed APK. This is still a
generated V1 presentation baseline, not commissioned final art.

The current development harness follow-up adds schema-v2 telemetry projected from the
canonical Bastion authority. The strict 100-match run remains **94/94** with 100/100
terminal matches and now records team score/deposits/KOs, Crown timing and socket
rotations, objective time, tickets/respawns, healing, gadget/ability use, overtime
stalemates, squad communication and alive-ally spacing. Aggregate values are **524/472**
score, **123/88** deposits, **155/208** KOs, **376** tickets spent/respawns, **7** overtime
stalemates, **238** socket rotations, **179,431** squad signals and **8,436,824** spacing
samples. The fresh physical route under
`Builds/Local/V1GameplayTruth/Next/lava-exact-20260902/` reaches Aandhi, combat, results
and rematch; no Crown deposit or explicit spectator transition was observed in the two
sampled matches, so the human/device gate remains open.

The superseded post-telemetry source rebuild is retained in the QA report for provenance.
The final visual-fix checker again passes the temporary package/target/ARM64/static-
alignment gates; final signing and identity are not selected.

## Latest current-source evidence — 2026-09-04 — production Bastion player HUD

The live Bastion route now uses one compact `PlayerStatusCard` for the human fighter.
`BastionPlayerHud.cs` groups fighter role, health, attack/ability readiness and gadget
feedback, and hides the legacy Solo-only `BijliHud` / `GadgetHud` cards only in Bastion.
MovementLab keeps the legacy cards for its focused fixtures. The new PlayMode regression
asserts the production card and suppression behavior.

Fresh automated gates are EditMode **159/159** (XML SHA-256
`04CE454016166816A88BE34E53BCAEFFDCA929786D0161F9DBCC3EC3E4527DD3`) and PlayMode
**95/95** (XML SHA-256
`CA5E0AC3FAC1E623612DC19874DA91429772B048A3D0B89E5DEF2E5B0B3B2054`); static
validation remains **0 errors / 0 warnings**. The exact APK is **41,608,148 bytes**
(`3CA0777D8B94E5381D08794BCA48BDCDE070675F4B54CB32EEFF31FE004C07F2`) and AAB is
**37,433,660 bytes**
(`1996369E050A8C77BE6098618BC20ACB6388CD11C2A2A54A44249780BDFE6E95`).

Approved Lava `ST5GDW23LB004392` (`1080x2460`, Android 14/API 34, 4 KB pages) matches
the local installed APK hash. Evidence under
`Builds/Local/V1GameplayTruth/Next/lava-player-card-20260904/` includes menu, briefing,
fighter choice, live 4v4 with the production card, results with the card hidden, and a
fresh live-only route. The accessibility dump still exposes only `unitySurfaceView`.
The concurrent six-sample/30-second live capture is under
`performance-player-card-20260904/` (manifest SHA-256
`6A4B00F6CBD8CBF2C68A10770BFE7BC7D488127A68A6D439E2E69E0B57321BFF`): PSS
**296,836–301,395 KB**, RSS **407,992–421,284 KB**, graphics PSS **89,244–93,348 KB**,
raw app CPU **94.2–103%**, CPU/GPU **41.03–41.657 C**, battery **81% → 81%**, thermal
status **0**, no configured fatal markers. The live-only SurfaceFlinger file
`surfaceflinger-latency-live-player-card-10s.txt` (SHA-256
`81316B42C3F642188C7ACBB36C086109D7EDA5079BB474EFCBDD7DAB0B2B10D8`) contains 126
presented frames / 125 intervals: mean **16.934 ms** (~**59.05 FPS**), p50 **16.534 ms**,
p95 **16.585 ms**, p99 **33.097 ms**, max **33.350 ms**, one interval over 33.33 ms and
none over 50 ms. This remains bounded device/compositor evidence, not normalized Unity
GPU/GC profiling, ten-rematch endurance or physical 16 KB runtime proof.

The clean technical release-checker log is
`release-checker-player-card-20260904.log` (SHA-256
`80AE8FF493B75D46083859544692A92D91715E00E32C9EBAD0B74F4D17B0A3B2`). It passed
repository validation, target SDK 36, offline permissions, ARM64-only native libraries,
static 16 KB ELF alignment and store-creative dimensions; the artifact remains temporary
debug-signed and owner approval is still required for identity, privacy, rating, culture
and Play submission.

The release candidate remains **Prototype — Android offline release candidate in
progress**: final identity/signing, full lifecycle and all-fighter/tutorial/accessibility
comfort, commissioned art/audio/cultural review, privacy/Data Safety/IARC and Play owner
gates remain open.

## Latest physical KO/spectator/respawn evidence — 2026-09-04

The exact current APK (`3CA0777D8B94E5381D08794BCA48BDCDE070675F4B54CB32EEFF31FE004C07F2`)
was exercised on approved Lava `ST5GDW23LB004392`. The player was held outside the
shrinking Aandhi until the production card showed **5/85** in
`lava-spectator-attempt-20260904/ko-watch-05.png` (SHA-256
`3997C4E3950FAFCB520F9310456670E680B0972821DABBD09581812D938D86AD`), then **0/85**
with the visible `OUT OF ACTION • respawn or spectate an ally` state in
`ko-watch-06.png` (SHA-256
`FB543967E71126D7A9BF84063EC97A998EEC8E667DAC5044B6D0CB913DD0675C`) and the held
spectator frame `ko-watch-07.png` (SHA-256
`D8191EAAE9751B7199ECB5626CBAC029BB6FA451C07FE344B635DA3ABCCFE5BE`). The card
returned at **81/85** after the shared Raja tickets changed from 11 to 10 in
`ko-watch-09.png` (SHA-256
`F1F7C5D7682CAEE1DE0F4004374095392855AA632D08A0ADE460046387B0B8B3`). This directly
proves the current-candidate player KO → spectator → respawn loop. The run's final
`airplane_mode_on` value was `0`; it supplements the separately recorded airplane-mode
offline launch evidence. Unity's accessibility dump still exposes only
`unitySurfaceView`, so this is rendered-state evidence rather than an accessibility
approval.

## Latest current-source evidence — 2026-09-04 — environment readability and portrait framing refinement

The current source carries a focused presentation follow-up for the mobile readability
review. `ProductionEnvironmentBuilder` now emits restrained woven/banded environment
detail rather than a high-contrast checker treatment, and
`TopDownCameraController` uses a **2.2** portrait framing cap multiplier so fighters,
Crown sockets and telegraphs occupy more of the Lava screen while the full arena stays
visible. The rebuild regenerated 12 environment materials and 16 environment texture
assets plus `BazaarBastionProduction.prefab`; the changes are editable and do not alter
authority, replay, AI or economy rules. Environment rebuild log SHA-256 is
`30F4DE942E01C7368DE7BB6DACDEE8E4FFDA0FC901D4FFA2DE85DF28846345E7`.

Fresh EditMode is **159/159** (XML SHA-256
`FF15744802560C6D2CFAB8E77BC11F2A3048F3F6D9C9116DC0D2FC70FF6FD4FF`) and PlayMode is
**94/94** (XML SHA-256
`4FA8EA81AB372436B6AB7796C31024262BF2016074FEFF88A812C87899F0A4AE`). The current
checkout was rebaselined again with EditMode **159/159** in
`Builds/Local/V1GameplayTruth/Next/editmode-rebaseline-20260904.xml` (SHA-256
`A04D0CC0C31459C241EEF3E3B63A479A43C5EC2B4E5C6BED78D439ECE3CBF9C2`) and PlayMode
**94/94** in `Builds/Local/V1GameplayTruth/Next/playmode-rebaseline-20260904.xml`
(SHA-256 `E95C6AED5922793791AD7B42064EDB80D782CEEC5AC12DC10A170966D77FB287`). The
rebaseline included the two-seed × 8,400-tick Bastion replay soak and deterministic
seeded soak, both with zero divergence. The current
APK is **41,579,392 bytes** (`54AF0801C2FD696DAD3224E6AD1CDDB7F15D8386094CAB8AAD68F5DFABB950E7`)
and the AAB is **37,404,926 bytes**
(`CAE58A648792BB14E77767E9036C073256BA2C8C1CED86DCCD31A42BE656A2F0`).

Fresh approved-Lava evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-camera-art-20260904/`: menu, Bastion briefing,
fighter choice, tight-framed live 4v4, action/mid-match/endgame states, authoritative
results at 04:02 (Raja 4/15 with 4 Crown deposits; Rival 0/15) and a rematch reset to
00:04 with Raja carrying. The results/rematch captures are
`09-results.png` (SHA-256
`4CD1558E905B25B9AE22590D40B573610146F6E6222D09C9BD7AA4B9E3A7B5CE`) and
`10-rematch-live.png` (SHA-256
`1E7F87D51E6F143098C5FE8BDB4F3C063A2018B6DF356C586C7FBFE00B594334`). The installed
APK hash matched on `ST5GDW23LB004392` (`1080x2460`, Android 14/API 34, 4 KB pages).
The package emitted no configured fatal markers. A point sample measured **298,676 KB
PSS**, **409,568 KB RSS**, **91,292 KB graphics PSS**, raw app CPU **2%**, thermal
status **0**, CPU/GPU **42.314 C** and battery **33 C**; Unity `gfxinfo` still exposed
no usable frame histogram. The accessibility tree remains `unitySurfaceView` only,
and no explicit player spectator transition was observed. This is bounded physical
evidence, not normalized FPS/GC/GPU/endurance or commissioned-final-art approval.
The clean-tree release-checker log is
`Builds/Local/V1GameplayTruth/Next/release-checker-camera-art-20260904.log` (SHA-256
`E8DA3A7106A0A64421DD709585B549852350088FA75199EF62F9DEBC8B83EF1F`) and passed
repository, target SDK 36, offline-permission, ARM64, static 16 KB and store-dimension
checks.

The current Lava compositor-side frame capture is retained at
`Builds/Local/V1GameplayTruth/Next/lava-camera-art-20260904/surfaceflinger-latency-10s.txt`
(SHA-256 `F7D32C24480683590FFADD1736F6E796FBAA0F51EAE64AE1E3492B1F597FDCAD`). Its
available history contains 127 presented frames / 125 intervals at a 16.667 ms display
period: mean **16.670 ms** (estimated **59.99 FPS**), p50 **16.534 ms**, p95 **16.569 ms**,
p99 **16.585 ms**, maximum **33.357 ms**, one interval over 33.33 ms and none over 50 ms.
A concurrent six-sample/30-second capture records PSS **301,862–306,949 KB**, graphics
PSS **87,212–93,368 KB**, raw CPU **105–112%**, thermal status **0**, CPU temperature
**40.847–41.489 C**, battery **32 C**, level **87% → 87%**, and zero fatal markers
(manifest SHA-256 `CD0DB79CF18A00DE0503C4CEA61966961B18F74CE58663B4538146CF3CFB16D4`).
This is bounded compositor/memory evidence; GPU utilization, GC spikes and ten-rematch
endurance remain unmeasured.

## Latest current-source evidence — 2026-09-04 — portrait backplate grounding

The final visual refinement adds an editable, collider-free **unlit** Bazaar backplate
under the gameplay plaza. `ProductionEnvironmentBuilder.cs` now generates
`Environment/Meshes/BackdropBox.asset` with tiled UVs and `Environment/Materials/Backdrop.mat`,
then saves the updated `BazaarBastionProduction.prefab`. This fills portrait camera
overscan with the authored woven palette without adding the full-screen lit/shadow pass
that made the earlier mosaic experiment miss frame pacing. Gameplay authority, replay,
AI, economy and input code remain unchanged.

The exact current APK is **41,598,480 bytes** (SHA-256
`9E5BFF2F28FC857D6E65E11A158942565E59A64AF68DA7F653C1F511060901B8`) and the matching
AAB is **37,424,015 bytes** (SHA-256
`91AE29A0165589FC9A5065A7B1579F991DEB54557689F62B94F227C75F1D98EA`). The build log
SHA-256 is `A0C952E2DC0EFA60FCC9D0D3009CF5647296D77AA212F9F9E3D8E3A75B896518`; the
environment rebuild log SHA-256 is
`1E870405C02EA5768E0A44853B029D4A3DABED94FFFB9770ABB17DFF12C1B954`. The APK is
installed on approved Lava `ST5GDW23LB004392`; the device base APK hash matches the
local APK. Exact route captures are `28-final-menu.png`, `29-final-briefing.png`,
`30-final-fighter-select.png`, `31-final-live.png`, `26-backdrop-unlit-near-results.png`
and `27-backdrop-unlit-rematch.png` under
`Builds/Local/V1GameplayTruth/Next/lava-camera-art-20260904/`. The route reaches
menu → Bastion briefing → fighter selection → live 4v4, an authoritative 04:02 result
(Rival winner; Raja 1 deposit, Rival 4 deposits), and a fresh 00:05 rematch reset.

The exact current-source automated gates are EditMode **159/159** (XML SHA-256
`FA1A2211ED4730DEF9B28CBCCECE47D8567353A69E232EA9E97E196A0113D158`) and PlayMode
**94/94** (XML SHA-256
`3AEB88102BB7C37BAAD6761063EB43C5C76CC910A078B093B0535DF6E2FE581B`). Static
validation remains **0 errors / 0 warnings**; the configured two-seed × 8,400-tick
replay/deterministic soak has zero divergence.

The exact final APK's six-sample/30-second performance capture is under
`performance-backdrop-unlit-20260904/` (manifest SHA-256
`EF523B47ECFCE00B5517BD96972C192E83AC32E19B921D8CA589A8358B8F5E25`): PSS
**295,058–306,074 KB**, RSS **405,692–425,404 KB**, graphics PSS **87,060–97,320 KB**,
raw CPU **100–127%**, CPU/GPU temperature **57.938 C**, battery temperature **33 C**,
thermal status **0**, battery **87% → 87%**, PSS change **2.40%**, and no fatal markers.
The compositor history (`surfaceflinger-latency-backdrop-unlit-10s.txt`, SHA-256
`F96E961687E92F139B6096805A6E93B54A16DA53D0C6EDCF1B33B58BF6661C63`) contains 127
presented frames / 125 valid intervals at 16.667 ms: mean **16.670 ms** (~**59.99 FPS**),
p50 **16.535 ms**, p95 **16.565 ms**, p99 **16.590 ms**, max **33.355 ms**, one
interval over 33.33 ms and none over 50 ms. This remains bounded compositor/device
evidence, not normalized Unity GPU/GC profiling, unplugged endurance, or a final-art
approval.

## Latest current-source evidence — 2026-09-04 — presentation identity pass

The current source refines the generated presentation baseline for mobile readability
without changing the canonical Bastion authority or replay rules. `ProductionArtBuilder`
now produces restrained woven/banded material detail and camera-facing Bijli/Pehel/Maya
eyes, jaw guards and role accents; `ProductionPresentationBuilder` keeps those parts in
the production rig. The pass regenerated 14 repository-owned texture assets and the
three production fighter prefabs. Fresh EditMode is **159/159** and PlayMode is
**94/94**. The art rebuild log SHA-256 is
`BB6AFB79D8658CF2333DAA5AEAF94EC94AD6529B1CEFF0837FA21B49A8485699`.

The current APK is **41,549,412 bytes**
(`E5F611282763C443B271F19C9EF63069AC3825E31EBD57DC3550187D3CC945EB`) and the AAB is
**37,374,943 bytes**
(`0F7C72459D66816E2E2EB2C20FD18FD15DB46018C45E78C52F65E1D3A65BE967`). The technical
checker passed the temporary package `com.example.battleraja.m11`, target SDK 36,
offline permissions, ARM64-only libraries, static 16 KB alignment and store dimensions.

Fresh approved-Lava evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-art-pass-20260904/`. The route reached menu,
Bastion briefing, fighter choice, live 4v4, a Rival-carrier state, a Raja-carrier
state, authoritative results at 04:02 and a rematch reset. The results card recorded
Raja 9/15 with **2 deposits** and Rival 1/15 with 0 deposits. The installed APK hash
matched the local APK. Lava is `1080x2460`, Android 14/API 34, and `getconf PAGE_SIZE`
is `4096`. The Unity accessibility tree still exposes only `unitySurfaceView`; no
explicit player spectator transition was observed. A point-in-time post-rematch sample
reported 294,934 KB PSS, 405,100 KB RSS, 93,560 KB graphics PSS, raw app CPU 2%,
thermal 0, CPU/GPU 41 C and battery 32 C; no normalized FPS/GC/GPU/endurance claim is
made. The presentation remains generated and editable, not commissioned final art.

## Latest current-source evidence — 2026-09-02 — canonical telemetry continuation

The superseding continuation record is `Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-02.md`.
Static validation is **0/0**, EditMode is **159/159**, PlayMode is **94/94**, and the
latest strict production-bot rerun is **94/94**. Its schema-v2 100-match report records
**92/100** in the 240–360 second window, **91/100** combat-positive, **61/100** with at
least three combat eliminations, **4** Aandhi-only resolutions, **100/100** bot-to-bot
damaging pairs, and zero protected-warmup or invalid-position samples. Canonical team
telemetry records **524/472** score, **123/88** deposits, **155/208** KOs, **376** shared
tickets spent/respawns, **7** overtime stalemates and **238** socket rotations. The v2
replay soak remains two 8,400-tick seeds with zero combined-hash divergence; squad planner
coverage remains contest 64, escort 64, defend 96, collapse 64 and Aandhi-retreat 32.
A preceding fresh process completed 100/100 but landed at 89/100 combat-positive; no
source changed between runs, so this pacing variance is retained as an open risk.

The preceding tutorial-authority APK is **41,520,532 bytes**
(`56F3BAB99E304A15548D8073BA6B41EDDCBDE17A2C7476D923B06094D5A9649E`), and the AAB is
**37,346,030 bytes**
(`19E2E7CCFFD7B2CBA993DE3608D8D62F4A351425AA76D0085138C1DF6DD96BCA`). The release
checker passes temporary package `com.example.battleraja.m11`, target SDK 36, no network
permissions, ARM64-only native libraries, static 16 KB alignment and store dimensions.
Fresh approved-Lava tutorial evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-tutorial-20260902/`; exact-final match evidence
remains under `Builds/Local/V1GameplayTruth/Final/lava-20260901-balanced/`. The 30-second
raw capture reports PSS **60,858–252,074 KB**, RSS **176,511–390,924 KB**, graphics PSS
**10,455–77,512 KB**, top CPU **35.7–57.1%**, thermal **0** and no configured fatal
markers. Lava reports 4 KB pages and Unity `gfxinfo` has no usable frame histogram. The
full Bastion tutorial, commissioned art, normalized endurance, physical 16 KB runtime,
permanent identity/signing, privacy/Data safety/IARC and Play owner gates remain open.

The final source-hygiene APK/AAB are the current artifacts: APK **41,516,076 bytes**
(`6050E1A6EC329F27BC14A1118FB166D278293237B4BC6CBA716B7B700D9FD6FF`) and AAB
**37,341,603 bytes**
(`A2F440649987A8FA04398B629F956AC44267AA1D33FEF571C3264B97051CCB4C`). Exact
post-regeneration gates are EditMode **159/159** and PlayMode **94/94**; physical
route evidence is under `Builds/Local/V1GameplayTruth/Next/lava-release-final-20260902/`.

## Previous current-source evidence — 2026-09-01 — offline Android continuation

The continuation record is `Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-01.md`.
Static validation is **0/0**, EditMode is **155/155**, PlayMode is **94/94**, and the
Bastion v2 replay soak reproduces two 8,400-tick seeds with zero combined-hash divergence.
The planner coverage run records contest 64, escort 64, defend 96, collapse 64 and
Aandhi-retreat 32 intents across 32 seeds. The runtime menu now uses the original
`BattleRaja-FeatureArt-OriginalCandidate.png` shrine/fighter scene; no vehicle/racing
motif is referenced by the V1 runtime.

The exact APK is **41,510,440 bytes** (`5F7438105FE450D6331CFEDEE1FAEEB87FB4F6677EB811A997A02CC8FD7C4AE9`),
and the AAB is **37,335,957 bytes** (`87C835570B62C4C3A79C156F94CB7E15C6AD31FCB50A0E8ADB0FDE6672DC4858`).
The release checker passes the temporary package `com.example.battleraja.m11`, target SDK 36,
offline permission gate, ARM64/static 16 KB checks and creative dimensions. Fresh approved-Lava
evidence reaches menu → Bastion briefing → fighter choice → live arena → settings. The clean
30-second live telemetry sample reports PSS 287,530–293,678 KB, RSS 426,940–433,088 KB,
graphics PSS 87,024–93,180 KB, CPU 111–118%, thermal status 0 and no configured app crash
markers. Lava reports 4 KB pages and Unity `gfxinfo` has no usable frame histogram. Final
authored/cultural/accessibility/fun review, complete physical action route, normalized endurance,
physical 16 KB runtime, permanent identity/signing, privacy/Data Safety/IARC and Play actions
remain open.

## Latest current-source evidence — 2026-08-31 — P66 source `e603ce7`

The exact current candidate hardens Android lifecycle input release: pause clears the player
adapter, virtual stick, attack, ability and gadget transient states, and the HUD clears the
adapter before its lifecycle boundary. Focused PlayMode is **1/1**, full EditMode is
**141/141**, full PlayMode is **92/92**, and static validation is **0 errors / 0 warnings**.
The rebuilt APK/AAB and checker hashes, plus the approved-Lava launch → HOME → resume route,
are indexed in P66 of `Docs/V1_RELEASE_PLAN.md`. The candidate remains temporary
debug-signed and the final human, physical-device, legal and Play gates remain open.

## Historical current-source evidence — 2026-08-30 — P55 source `56df201`

The exact prior candidate adds the generated `ZoneFinalCircle.wav` cue and plays it once when
the authoritative match phase enters Final Circle. Its static validation, 141/141 EditMode,
89/89 PlayMode, APK/AAB hashes and approved-Lava evidence remain indexed in P55 of
`Docs/V1_RELEASE_PLAN.md`.

## Historical current-source evidence — 2026-08-29 — documentation tip `b4b5649` (runtime/art `ac45479`)

The saved-presentation continuation adds a collider-free, textured
`BazaarBastionProduction.prefab` with a 32×32 three-submesh ground mosaic, deterministic
64×64 environment textures/materials, themed stalls/gates/banners and a backdrop LOD group.
Production scene generation removes the legacy runtime architecture instance and binds the
saved prefab with fallback disabled. Fighter prefabs now carry saved two-level LOD metadata
and far-silhouette meshes. Runtime feedback, projectile, gadget and Maya-decoy fallback
visuals use shared custom geometry; the decoy retains an explicit capsule collider for local
target perception. No gameplay/domain/network/package-policy boundary changed.

Static validation is **0 errors / 0 warnings**. Full EditMode is **141/141** and the exact
source production-bot PlayMode run is **87/87**, including the saved-environment/LOD and
decoy mesh/collider regressions. The exact 100-match batch is **100/100** terminal and in
the 240–360 second window, with **91/100** matches reaching at least three combat
eliminations, **100/100** with bot-to-bot damage, **0/100** Aandhi-only, zero protected or
invalid samples, and all three gadgets exercised. The detailed report and hashes are in
`Docs/V1_RELEASE_PLAN.md` P45.

This remains a generated V1 presentation baseline, not final commissioned art or Play
readiness. The exact rebuilt temporary-ID APK is **40,672,170 bytes** (SHA-256
`6103F42176726E8CACE0DA7C4880BD105A55E50FFD92EB1BA8B2F531BEAA231D`) and the matching
AAB is **36,497,323 bytes** (SHA-256
`9893493591C4474E517B3D80A5107986493A2E70F59C850D17AC08C8B2748404`). The composed
release checker passed **0 errors / 0 warnings**; the exact checker log is
`Builds/Local/Logs/release-checker-ac45479-b4b5649.log` (3,245 bytes; SHA-256
`F05A2E9FD98D5AD73D9B9E7F1C52222CC3F535AD82516C500EADA2A50A857CDB`). Bundletool
1.18.3 generated the universal APKS set (SHA-256
`4E864E09557DA59892C629BA0A2AD42FDA58562EFA8485BC81B3C8D93FCD66B3`); direct and
extracted APK zipalign checks passed, and v2/v3 signature verification passed for the
temporary Android Debug signer. These are technical local gates, not release signing.

Fresh approved-Lava evidence is under
`Builds/Local/Device/Performance/20260829-lava-ac45479-smoke/`: the exact APK installed
successfully and real touch reached menu → Solo Raja → Bijli selection → live opening.
The menu, mode, fighter-select and live-opening screenshots have SHA-256
`217984A80310452CDE4C0BBD804B255509376BAA47D01483CF5A28FEEB0EED43`,
`7E8C5B975C9AE357A82BC4C4D7522F331D3A9C2BD1029EBB991CD267F9E64830`,
`90F6750AD276150607A0D466F3421471928F92EB80E55FAE89F11EE309B57912` and
`615A72B4332E26DE3C0DADCEFFEA7184ABABE12722EAC3ABC8F11C533FD0DD48` respectively.
A six-sample, five-second-interval, approximately 30-second live capture is under
`Builds/Local/Device/Performance/20260829-lava-ac45479-30s/`; its final screenshot hash
is `8255FA6ED94AA563355964C0C9A4B32681A2660B69C8A18BD14E1F7612234C53`, PSS ranged
**267,957–272,145 KB**, graphics PSS **75,792–79,888 KB**, battery level stayed at
**62%**, and thermal status was **0**. The player was defeated and the app remained in
the spectator state; no configured fatal/ANR/SIGSEGV markers were found. This is bounded
raw device evidence, not normalized sustained-performance, battery or thermal approval.

The device is Lava `ST5GDW23LB004392` (Android 14/API 34, 4,096-byte pages); no Oppo
device was used. Physical 16 KB behavior, sustained performance, full tutorial/all-
fighter/accessibility and human art/audio/cultural review, final signing/identity,
privacy/Data Safety, content rating and Play Console actions remain open.

## Latest current-source evidence — 2026-08-29 — commit `bc392fd`

The focused presentation continuation adds deterministic UVs to every generated mesh and
saved two-bone primary body/cloak skins (`BijliSkinBody`, `PehelSkinBody` and
`MayaSkinCloak`). The source primary MeshRenderer is retained but disabled for reproducible
rebuilds; the visible SkinnedMeshRenderer remains presentation-only and owns no gameplay
state or colliders. Full EditMode is **141/141** and full PlayMode is **87/87**, including
UV coverage, bind-pose parity and non-zero two-bone blend regressions. Static repository
validation is **0 errors / 0 warnings**.

The exact debug-signed APK is 40,595,182 bytes (SHA-256
`9A0F3715BFFA208F4D821B786D68EFE22A13C05053D05CA8611F6A614D318060`) and the matching AAB
is 36,420,355 bytes (SHA-256
`C8CA4351D4778E5C117F9E9CA29D9C2CEA5C1BFF041718D6175AA7559CF14105`). The release checker,
bundletool universal extraction, direct/extracted `zipalign -P 16` and v2/v3 verification
pass. Approved Lava `ST5GDW23LB004392` reached menu -> Solo Raja -> Bijli -> live opening
with the exact APK. Its bounded six-sample, 30-second live capture reported PSS
263,051–269,743 KB, graphics PSS 75,128–79,224 KB, thermal status 0 and no configured
fatal markers; this is raw diagnostic data, not normalized performance approval.

The exact APK also reached the same live opening on genuine `BattleRaja_16K` (Android 36,
`sdk_gphone16k_x86_64`, `getconf PAGE_SIZE=16384`) with no configured fatal markers. This is
emulator diagnostic evidence only: Lava reports 4,096-byte pages, and physical 16 KB,
sustained performance, full tutorial/all-fighter/accessibility and human art/audio/cultural
review remain open. The two prompt files in the working tree are intentional owner files;
the focused code/art commit itself is clean and no remote mutation was performed.

## Latest current-source evidence — 2026-08-28

The focused presentation source is cleanly committed at `816d9ac` on
`codex/v1-playstore-release`. The saved Bijli, Pehel and Maya production prefabs now use
distinct repository-owned faceted low-poly torso/cloak and accessory meshes. The explicit
editor rebuild action preserves the render-only boundary; no gameplay authority, collider,
input, network or package-policy code changed. Full EditMode is **141/141**, full PlayMode
is **87/87**, including the faceted silhouette regression, and static validation is
**0 errors / 0 warnings**.

The exact rebuilt APK is 40,542,342 bytes (SHA-256
`0517EE901A9EAE943140538366B0574E893DC6BD66A5D1714D630C2379EF5FAC`) and the matching AAB
is 36,367,513 bytes (SHA-256
`BF52E649BFD92F277F5C9933A7FDF34FFB25410F1D5A18EF6FC3097AA31BA331`). Offline manifest,
ARM64/static 16 KB, bundletool universal extraction, zipalign, v3 verification and store
creative dimension checks pass. The APK reached the live opening match on approved Lava
`ST5GDW23LB004392`; a bounded six-sample, 30-second live-state capture found thermal
status 0 and no configured fatal markers. Exact evidence is indexed in
`Docs/V1_RELEASE_PLAN.md` P43.

This is a stronger generated presentation baseline, not final commissioned art or Play
readiness. Lava reports 4 KB pages, and physical 16 KB runtime coverage, sustained
performance, full tutorial/all-fighter/accessibility and human feel review, final signing/
identity, privacy/Data Safety, cultural/legal approval and Play Console actions remain open.

## Latest current-source evidence — 2026-08-27

The runtime/presentation source is clean and committed on branch
`codex/v1-playstore-release` at exact candidate commit
`2a113e0c4798e8e51a43379a0fa0facd7e8f0fe1`. Full EditMode is **141/141**, full
PlayMode is **86/86**, and static validation is **0 errors / 0 warnings**. The new
PlayMode regression proves that a pre-collected tutorial gadget is reconciled when its
lesson begins; the earlier live-authority Elimination regression remains covered. The
carried-forward 1,000-seed deterministic replay soak has 2,000 executions and zero
divergence, and the current-tip rerun also passes with the same result. Exact current-tip
XML/log hashes are indexed in `Docs/V1_RELEASE_PLAN.md` P40; this is same-machine evidence
and does not establish cross-machine parity. P42 now closes the available durable production
replay gate: the development-only production harness emits ordered `.brr` captures with
per-tick canonical snapshots/hashes, and an exact generated file re-executes cleanly against
the authority. Cross-machine parity and human review of cosmetic animation/audio/VFX remain
open.

The P42 production replay artifact is
`Builds/Local/V1GameplayTruth/ProductionBotReports/Replays/match-9101-20260827-160257598.brr`
(5,802,977 bytes; SHA-256
`48C0DC38A417934331245FBB28B8EE15589502C23E93619EC688310C1E487736`), with report metadata
in `Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-160256013-9101.json`.
The current source also passes the 100-seed production-bot release batch (100/100 in the
240-360 second window, 94/100 with at least three combat eliminations, 100/100 with
bot-to-bot damage, zero Aandhi-only resolutions); the aggregate report and exact same-seed
reproducibility evidence are recorded in P42. The exact-file verification is recorded in
P42 of `Docs/V1_RELEASE_PLAN.md`.

The same exact candidate also completed a bounded Lava probe through Solo Raja, Bijli
selection, player defeat, spectator mode, Aandhi Final Circle, Results and two Rematch
transitions, with a third cycle returning to the menu. Settings/accessibility toggles
were exercised and restored. The 180-second six-sample capture had no configured fatal
markers, thermal status 0 and stable 63% USB-powered battery; PSS stabilized at
239,626-243,910 KB and RSS at 355,300-359,580 KB. Evidence and hashes are indexed in
`Docs/V1_RELEASE_PLAN.md` P41. This remains bounded device evidence, not full tutorial,
all-fighter human approval or normalized performance-budget sign-off.

The fresh exact-runtime fixed-tick production-bot batch completed **100/100** terminal
matches in the 240-360 second window, **94/100** with at least three combat eliminations,
**100/100** with bot-to-bot damage, and zero protected/invalid/stuck invariant samples.
Its report and hashes are indexed in `Docs/V1_RELEASE_PLAN.md` P38.
The exact APK/AAB are debug-signed local candidates. The final clean release checker
reports **0 errors / 0 warnings**, seven ARM64 libraries, no network permissions and
static 16 KB alignment; its evidence is indexed in P42. The APK installed and launched on approved Lava
`ST5GDW23LB004392`, which reports 4 KB pages; this is not genuine runtime-16-KB proof or
sustained performance approval.

The bounded exact-candidate touch route now has action-attributed Movement, Aim, Basic
Attack, Ability, Gadget and Aandhi transitions. `gadget-tap.png` shows the Gadget card
at `CONTINUE`, `aandhi-step.png` shows the Aandhi card at `CONTINUE`, and
`after-aandhi-continue.png` shows the Elimination card correctly waiting for a
player-attributed KO. Captures and hashes are recorded in P36 under
`Builds/Local/Device/Screenshots/20260827-754837e-release/tutorial-gadget-reconcile/minimal-route/`.
Full match/Victory/rematch, accessibility, normalized CPU/GPU/GC/repeated-match
endurance, final physical 16 KB coverage, authored final art/audio, cultural review,
release signing, privacy/Data Safety and Play/legal gates remain open. A genuine
16 KB emulator runtime check now passes; the emulator evidence is recorded in P39.

A fresh exact-candidate 30-second Lava diagnostic (six samples, 5-second interval) found
no configured fatal markers and thermal status 0 throughout. After startup, PSS was
230,576-240,186 KB and RSS 346,788-356,404 KB. This is bounded launch/idle evidence only;
it does not establish the documented dense-combat or repeated-rematch budgets. The
capture and hashes are recorded in `Docs/V1_RELEASE_PLAN.md` P37.

## Latest current-source evidence — 2026-08-26

The exact current workspace is branch `codex/v1-playstore-release` at HEAD
`fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus intentional working-tree edits (57
changes; no clean-source claim). Current EditMode is **140/140**, PlayMode is **79/79**,
the strict 100-match production-bot gate passed **79/79** on the recorded third attempt
(91/100 combat-elimination matches, 9/100 Aandhi-only), and the 1,000-seed replay soak
passed **1/1** with zero divergence. The matching APK/AAB and exact hashes are indexed in
`Docs/V1_RELEASE_PLAN.md` P11-P13. Static Android validation remains **0 errors / 0
warnings**, with ARM64-only payload and static 16 KB alignment passed.

Approved Lava `ST5GDW23LB004392` completed a 30-second, six-sample launch/menu capture
with no configured fatal markers and thermal status 0 before/after. The device reports
4 KB pages; genuine 16 KB runtime validation, sustained full-match performance, physical
touch/accessibility, final visual/cultural review, signing/package identity, privacy/Data
Safety, content rating and Play Console actions remain open.

## Exact current source

- Branch: `codex/v1-playstore-release`
- Current runtime-bearing source: `754837e4311b609560c63fa90558a1d29acec9cd`
  (`fix: reconcile tutorial gadget telemetry after bind`).
- Exact Android candidate source: `754837e4311b609560c63fa90558a1d29acec9cd`
- Documentation evidence anchor: the exact-source sections in this index and
  `Docs/QA/LATEST_HEAD_BASELINE.md`, updated with the release-flags candidate.
- The exact-current release-shaped APK/AAB include the safe-area HUD and
  reduced-flash fixes. They are archived under the root-only evidence policy.
- Unity: `6000.5.6f1`
- EditMode: **140/140**
- PlayMode: **86/86**
- Repository validation: **0 errors / 0 warnings**
- Git LFS: passed

The current presentation fixes keep the runtime match HUD inside the gameplay
safe area, propagate reduced-flash settings to combat impact, hit and Aandhi
feedback, and reconcile a gadget collected before the Gadget tutorial card binds.
The exact-current APK installed and launched on Lava `ST5GDW23LB004392`; fresh
tutorial probes are indexed in `Docs/V1_RELEASE_PLAN.md` P36. This proves install/
launch and selected action-gated transitions only; full-match, accessibility and
performance review remain open.
Earlier bounded Lava diagnostic captures remain useful measurements but do not replace
human review or establish final frame, thermal, battery or memory budgets.

## Prior Android artifacts used for the performance-tool baseline

Built from `1d743b0`; the retired disposable checkout is not part of the current
workspace and its raw package files are not retained as current evidence:

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| APK | 39,486,559 | `89156306717C5EB27EE193AD1D46809DFE19159112ADC3C77008D4C6A3C89DE0` |
| AAB | 35,313,996 | `F5776F6AF19EE1C0A803D76050A80E62E883710E00296EF9603CED279D2227C1` |

The AAB is ARM64-only and passed static 16 KB ELF alignment. The package is
debug-signed with temporary ID `com.example.battleraja.m11`; it is not
publishable. Installation succeeded only on Lava `ST5GDW23LB004392`. The phone
was locked during launch, so no interactive route evidence is attributed to this
candidate.

## Current open gates

- Perform the real tutorial action sequence, full match, all
  fighters/gadgets, spectator, results, rematch, settings and background/resume.
  The exact 2080383 candidate now has partial machine/device evidence through
  spectator/results and REMATCH, but the complete owner-operated route remains open.
- Interpret the exact 2080383 120-second match capture from
  `Tools/Validation/capture_android_performance.ps1` against explicit budgets and add
  normalized frame/GC/GPU/repeated-rematch evidence where tooling permits.
- Refresh store screenshots from the final human-reviewed signed candidate.
- Supply final package identity and release signing; recheck manifest, permissions,
  target API, 64-bit and 16 KB compatibility in the owner-controlled Play flow.
- Complete privacy/Data Safety, content rating, cultural, accessibility and visual
  review; no publication is authorized yet.

## Deliberate non-scope

Photon gameplay, PlayFab, accounts, matchmaking, online progression, Web release,
ads, IAP and public deployment remain outside V1.0.

## Current presentation refresh — 2026-08-26

The current dirty source (`HEAD fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus intentional
working-tree edits) now has saved production presentation assets rather than only runtime
silhouettes. Unity-generated fighter prefabs contain a named transform rig, a shared
nine-state Animator controller with nine editable clips, and four particle cues per fighter.
The saved VFX library contains 14 bounded particle prefabs for fighter signatures, hit/
elimination, gadget/heal/shield and Aandhi phases. `ProductionVfxCue` is presentation-only;
it cannot mutate authority state. Scene references were refreshed through Unity serialization
after prefab regeneration changed root IDs.

Post-refresh evidence: EditMode **140/140** and PlayMode **80/80**. The focused rig/VFX test
passed **1/1**. The V1 mixer contains named Music and Combat buses with guarded source-volume
fallback. This remains a generated presentation baseline;
human-authored sculpt/skinning polish, final VFX readability, cultural review, sustained
Lava performance and final signed-package QA remain open.

## Final current-source rebuild — 2026-08-26

After the presentation refresh, the audio director was hardened to apply persisted source
volumes without probing absent editor-only mixer exposure names. The focused audio test passed
**1/1** with no targeted Unity warning/error markers; full EditMode passed **140/140** and full
PlayMode passed **80/80**. Exact XML/log hashes and the generated mixer hash are recorded in
`Docs/V1_RELEASE_PLAN.md` P15.

The same final source also passed the deep deterministic replay soak: **1,000 seeds executed
twice**, zero divergence, **542.3398755 s**; the exact XML and log hashes are recorded in P15.

The matching current APK/AAB were rebuilt and the composed release checker passed **0 errors /
0 warnings**. The bundletool 1.18.3 universal set and both direct/extracted APK zipalign
checks passed. The exact APK installed on approved Lava `ST5GDW23LB004392`; a six-sample,
30-second launch/menu capture had no configured fatal markers, thermal status 0 before/after,
and app PSS **55,262–236,543 KB**. The device reports 4 KB pages, so runtime 16 KB proof,
sustained full-match performance, physical touch/accessibility, final visual/audio/cultural
review, signing/package identity, privacy/Data Safety, content rating and Play Console actions
remain open.

The final-source strict production-bot gate was rerun twice with 100 seeded matches and the
existing 50x diagnostic playback. Both completed 100/100 and passed all safety/invariant checks,
but only 70/100 and 76/100 respectively met the 240–360 second pacing window. This is the
known timing-sensitive shortcut; the earlier passing run is retained as historical evidence,
and the release threshold remains unchanged/open.

## Exact clean-source refresh — 2026-08-27

The reviewed runtime/presentation source is clean at `2f9a6a0151e3b0c2359d9b0f8892c28e6404ec4b`;
the detailed evidence is `Docs/V1_RELEASE_PLAN.md` P16. Full EditMode passed **140/140** and
PlayMode **80/80**. The exact-source deterministic soak passed **1/1** with
`BATTLERAJA_SOAK_MATCHES=1000`, 1,000 seeded matches executed twice and zero divergence.
Matching APK/AAB builds, bundletool universal extraction and both 16 KB zipalign checks passed.

The exact APK was reinstalled on Lava `ST5GDW23LB004392`; a fresh six-sample, 30-second
launch/menu capture had no configured fatal markers and thermal status 0 before/after. The
device reports 4 KB pages, and no sustained full-match, touch/tutorial, accessibility or
runtime 16 KB proof is claimed. The strict production-bot pacing threshold was still open at
this earlier capture and is superseded by the exact-source P31 batch below; human signing,
legal, store and final-authored-content gates remain open.

## Exact-source production-bot gate refresh — 2026-08-27

From clean documentation tip `90670ff` (runtime-bearing source `126714a`), the release
assertion batch completed **100/100** seeded production-pipeline matches. Every match reached
terminal state within the 10,800-tick budget and lasted **306.014 s**, putting **100/100** in
the 240–360 s window. Every match contained bot-to-bot damage and a combat elimination;
Aandhi-only resolutions were **0/100**. Protected-warmup damage, invalid positions and
continuous stuck ticks were all zero, and Bijli, Pehel, Maya, Umbrella Guard, Dhol Burst and
Tiffin Station were all exercised. The exact report, NUnit XML and Unity log hashes are
recorded in `Docs/V1_RELEASE_PLAN.md` P31.

The automated production-bot pacing/safety gate is now passed. Human touch/tutorial,
accessibility, sustained frame/GC/GPU/battery performance, runtime 16 KB, final authored and
cultural review, package identity/signing, privacy/Data Safety, store assets and Play Console
actions remain open; this is not a Play-ready claim.

## Exact current-source aim-state refresh - 2026-08-30

The reviewed source tip for this refresh is `d0de9499e764045d72dbf092da4c8f2d85fb0b36`.
It adds a dedicated render-only Aim animation state and intent adapter while leaving the
offline authority, input command, damage, cooldown, movement, gadget and timing paths intact.
The saved clip/controller and regenerated fighter prefabs are covered by **141/141 EditMode**
and **87/87 PlayMode** passes; repository validation and the composed Android checker both
report **0 errors / 0 warnings**.

The exact temporary-ID APK/AAB and bundletool universal evidence are recorded in P46 of
`Docs/V1_RELEASE_PLAN.md`. Approved Lava `ST5GDW23LB004392` installation succeeded, and the
current route captured all fighter cards, live action/Aandhi/elimination/spectator/results/
rematch/settings/lifecycle observations plus tutorial `8/8 COMPLETE`. The raw action snapshot
is **265,746 KB PSS / 378,096 KB RSS / 75,792 KB graphics PSS**; no configured app fatal,
ANR or SIGSEGV marker was found. Because the phone reports 4 KB pages and `gfxinfo` has no
usable frame histogram, runtime 16 KB, sustained budgets and normalized performance remain
unclaimed. Owner gates for final art/audio/cultural/fun/accessibility, signing/package identity,
privacy/Data Safety, content rating and Play Console remain open.

## Latest exact terminal-outcome presentation candidate - 2026-08-30

Source `5d136fbb6be6a5554931f6ab859be8b9a8a995a2` adds saved Victory/Defeat particle cues and
routes authoritative result placement into persistent render-only fighter states. It does not
change the offline domain, authority, input, damage, cooldown, movement, gadget, zone, timing,
network or reward paths. The exact candidate passes repository validation **0/0**, EditMode
**141/141** and PlayMode **88/88**, including a regression that exercises both winner and
non-winner terminal presentation states. Exact Android/bundletool evidence is indexed in P47
of `Docs/V1_RELEASE_PLAN.md` and in the local Lava manifest
`Builds/Local/Device/Performance/20260830-lava-5d136fb-outcome/manifest.json`.

Approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34) installed the exact APK
successfully. Real touch reached menu, all three fighters, live action feedback, Aandhi
pressure/closing/final circle, defeat/spectating, results/rematch, settings toggles,
background/resume and tutorial `8/8 COMPLETE` via SKIP. The bounded snapshot is **273,885 KB
PSS / 385,796 KB RSS / 80,020 KB graphics PSS**, thermal status 0, with no configured app
fatal/ANR/SIGSEGV marker. The phone reports 4 KB pages; no runtime 16 KB, normalized FPS or
sustained performance claim is made. Final authored/cultural/fun/accessibility review,
production signing/package identity, privacy/Data Safety, content rating and Play Console
actions remain owner-controlled.

## P48 exact-candidate focused performance evidence - 2026-08-30

The provided Android performance capture ran against the exact `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`
candidate on Lava `ST5GDW23LB004392` for **180 seconds**, with 36 five-second samples and
movement/attack/ability/gadget input during a live match. The manifest is
`Builds/Local/Device/Performance/20260830-lava-5d136fb-perf2/manifest.json` (SHA-256
`7728C80ADFEA814D1D9E63D3344C527825CFCF413236AB89131C62C46C2D459D`). Warm-up-excluded
PSS was **261,702-273,769 KB**, RSS **384,112-396,696 KB**, graphics PSS **75,792-81,936 KB**,
and current-process `top` CPU **87.5-118.0%** (Android 100%-per-core scale). Battery stayed
at 76% while USB-powered, thermal status stayed 0, and no configured crash markers were
found. A 30-second Perfetto trace is retained, but no host trace processor was available;
Simpleperf correctly refused the non-profileable temporary candidate. Unity `gfxinfo` still
has no usable frame histogram. This strengthens raw warm-up/stability evidence only; no
normalized sustained performance, unplugged battery, genuine 16 KB runtime, or human approval
claim is made.

## P49 genuine 16 KB Android 16 AVD smoke - 2026-08-30

The exact terminal-outcome APK from `5d136fb` installed and launched on the `BattleRaja_16K`
Android 16/API 36 AVD with host-GPU rendering. The model is `sdk_gphone16k_x86_64`,
`getconf PAGESIZE` returned **16384**, and the ABI list includes `x86_64,arm64-v8a`. The
branded menu and live-match checkpoints rendered normally; the clean menu capture is
`Builds/Local/Device/Performance/20260830-16k-5d136fb/host-gpu/launch-final.png` (SHA-256
`919BA18BBCA77C4C843DD07EC1470E8D0DFAE4AC3C3F012266E102ACABD55FA0`). The app-scoped launch
logcat has no configured fatal, ANR, SIGSEGV, SIGABRT or shader-link marker.

The 90-second harness run (18 five-second samples) is indexed by manifest SHA-256
`AC691AF0BB69983AFE0001F87A4AF92543454D3F190C61FB974734A42EE48B61`. Warm-up samples
measured PSS **435,726-436,966 KB**, RSS **617,304-621,236 KB**, GraphicBufferAllocator
estimate **31,416 KB**, and process `top` CPU **96.1-123.0%** on Android's 100%-per-core
scale. The virtual battery stayed at 100%/5,000 mV/25 C and thermal status 0. Unity `gfxinfo`
has no usable frame histogram, so this is synthetic runtime/page-size evidence rather than a
product-tier performance pass.

The same AVD with SwiftShader reached combat but showed URP/Lit uniform-limit corruption;
that retained folder is superseded renderer diagnostics, while host-GPU is the authoritative
normal-rendering profile. Local classification is **16 KB host-GPU AVD smoke passed**;
physical 16 KB, other GPU profiles, normalized performance and human approval remain open.

## P50 exact-candidate Lava live-match SurfaceFlinger diagnostic - 2026-08-30

The exact terminal-outcome APK from source `5d136fb` was relaunched on approved Lava
`ST5GDW23LB004392` through Rematch. A 45-second SurfaceFlinger ring-buffer sample during
the live Solo Raja match produced **126 valid present timestamps** and **125 intervals**
after excluding one `Long.MaxValue` sentinel. The middle timestamp column yielded
min/median/p95/p99/max intervals of **16.447 / 16.534 / 16.565 / 33.078 / 33.367 ms**;
three intervals exceeded one refresh period and one exceeded 2×. Raw evidence and the
1,847-byte summary (SHA-256
`21369E4FC3BF33BF1DB234BE2F23F1A8D32BD45D0DF29F8682DC90D17489B144`) are under
`Builds/Local/Device/Performance/20260830-lava-5d136fb-sf/`; the raw latency file SHA-256
is `D83D61790C60E5D76CB9BBC5B0D25CA91D0AD044BC63686DAD417F71942B3D26`.

The end capture shows player defeat and spectator state while Aandhi closes. End telemetry
was **277,284 KB PSS / 400,500 KB RSS / 80,052 KB graphics PSS**, battery **75% / 4,120 mV /
31 C** while USB-powered, thermal status **0**, and no configured fatal markers. Android
`gfxinfo` still has no usable Unity histogram; Lava reports 4 KB pages. This is bounded raw
frame-present/stability evidence only, not normalized performance, runtime-16-KB or human
approval.

## Current release-handoff documentation tip — 2026-08-30 07:05 IST

The current documentation tip is aligned with `origin/main` after a docs-only continuation. It adds
`Docs/RELEASE/V1_RELEASE_NOTES_AND_SUPPORT_DRAFT.md` with release notes, invited-tester
quick-start steps, known limitations, support/feedback copy and an owner submission
checklist. It also corrects the Play metadata wording so the generated production baseline
is not described as “procedural placeholders”, and marks the older Web-inclusive store copy
as historical for the V1 Android scope. Store-creative checks and repository validation are
green; no runtime/build/test evidence changed, and P47-P50 remain the exact candidate index.

## P52 player-facing Umbrella Guard regression - 2026-08-30 07:21 IST

The latest test-bearing tip is `4c4c67cbbc20062e3723cc90ee3bb7c266bbeda4`, based on runtime/art
source `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`. A new `GadgetPlayModeTests` regression
exercises player Umbrella Guard pickup/use through the MovementLab authority path and checks
inventory consumption, configured shield duration, feedback, success telemetry, front-facing
projectile mitigation and the Aandhi bypass. Full
EditMode is **141/141**, PlayMode is **89/89**, and repository validation is **0/0**; exact
XML/log hashes are indexed in `Docs/V1_RELEASE_PLAN.md` P52. No runtime artifact or device
evidence changed, and P47-P50 remain authoritative. Human authored/cultural/fun/accessibility
review, normalized performance, physical 16 KB, signing, legal/privacy, rating and Play
Console gates remain open.

## P53 current exact-source production-bot gate refresh - 2026-08-30 08:03 IST

The fixed-tick production-bot harness was rerun from documentation tip `7167f33` with
Unity `6000.5.6f1`, 100 matches, release assertions enabled, playback scale 50 and base seed
9101. The runtime/art source remains `5d136fb`. PlayMode passed **89/89**. All **100/100**
matches reached terminal results at **306.0135193 s** each and landed in the 240-360 second
window; all had combat and bot-to-bot damage, **0/100** were Aandhi-only, and protected-
warmup/invalid-position samples were zero. Umbrella Guard, Dhol Burst and Tiffin Station
were each used in all 100 matches. Exact XML/log/batch paths and hashes are indexed in
`Docs/V1_RELEASE_PLAN.md` P53.

This closes the current automated production-bot pacing/safety gate locally. The accelerated
50x run remains separate from same-seed determinism evidence; P10's real-time result remains
the determinism record. Human route, comfort/accessibility/fun, authored/cultural review,
normalized sustained performance, physical 16 KB, signing, legal/privacy, rating and Play
Console gates remain open.

## P54 current-source saved-fighter presentation and portrait settings refresh - 2026-08-30

The current presentation source is committed at `ae0d294c97fa62386317e7e5ebf77cd5ebcbafee`,
following the saved-identity integration in `3d8fda7`. Saved Bijli, Pehel and Maya production
prefabs now replace the legacy root capsule at runtime; the root mesh remains available as a
fallback for fixtures without a saved identity, the root dash `TrailRenderer` is preserved,
and per-renderer base colors survive hit/elimination flash reset. Hit/elimination tinting
targets the saved mesh renderers. On compact portrait layouts, the settings surface is a
centered modal rather than a right-side sheet. ADR-071 records the boundary and rationale.

Fresh validation is **0 errors / 0 warnings**, EditMode **141/141**, and PlayMode **89/89**.
The rebuilt temporary-ID APK/AAB pass the offline manifest, ARM64/static 16 KB and store-
creative technical gates; exact hashes and logs are indexed in `Docs/V1_RELEASE_PLAN.md`
P54. The exact APK installed and launched on approved Lava `ST5GDW23LB004392` only. Fresh
captures show the saved faceted fighter identities in the live arena and the centered portrait
settings modal; sampled app logcat has no configured fatal/ANR/SIGSEGV/SIGABRT markers.

A current-source 180-second Lava capture contains 36 five-second samples with warm-up-
excluded PSS **238,249-244,525 KB** (average **241,729 KB**), RSS **360,572-366,836 KB**
(average **364,046 KB**), graphics PSS **70,288-74,396 KB** (average **72,267 KB**), and
process CPU **35.7-62.5%** (average **57.8%**, 100%-per-core scale). It was
USB-powered, reported 4 KB pages, and found no configured fatal markers. These are bounded
stability observations, not normalized FPS/frame-time/GC/GPU approval, unplugged endurance,
physical 16 KB runtime proof or final human visual/accessibility approval. Oppo was not used.

The product remains a **prototype / Android offline release candidate in progress**, not
Play-ready. Final authored/cultural/fun/accessibility review, physical 16 KB, normalized
sustained performance, final package identity/signing, privacy/Data Safety, content rating
and Play Console actions remain owner-controlled. The two prompt files under `PROMPTS/`
remain intentional uncommitted owner work.

## P55 current-source final-circle audio cue and exact Lava endgame capture - 2026-08-30

The current source tip is `56df201`. It adds the owned generated `ZoneFinalCircle.wav` cue,
loads it through `BattleRajaAudioDirector`, and plays it once on the authoritative transition
to `MatchPhase.FinalCircle`; gameplay rules and the offline/networking boundary are unchanged.
The cue hash is `269DD92C83A3592DDA9AE7F186C76A9D25C9F9BEFF882897DD3F6727581F4F85`.

Fresh validation is **0 errors / 0 warnings**, EditMode **141/141**, and PlayMode **89/89**.
The exact temporary-ID APK is 40,681,059 bytes (SHA-256
`AB4974445DA2BAEB023DBCEB5EFF557F161A53F25695B0FD9BD417045FF29855`) and the AAB is
36,506,374 bytes (SHA-256
`E1658F47D855693FB8F281385EB21176CA4E81C19D86554FC70F91FD94A7F90E`). The post-commit
release checker is **0 errors / 0 warnings**; package, API, offline permission, ARM64 and
static 16 KB details are indexed in `Docs/V1_RELEASE_PLAN.md` P55.

The exact APK installed successfully only on Lava `ST5GDW23LB004392`. Current captures under
`Builds/Local/Device/final-circle-20260830/` show menu/fighter/settings/live combat, the
`FINAL CIRCLE` state and the Results panel. Filtered route and HOME → relaunch app logs contain
no configured fatal/ANR/native-crash markers. Lava is a 4 KB device; this does not prove a
physical 16 KB runtime. The Unity SurfaceView has no semantic UI tree, and no action-by-action
tutorial comfort, final audio mix, authored-art, cultural, fun, accessibility, normalized
performance, battery/thermal, signing, legal/privacy or Play approval is claimed.

The truthful classification remains **Prototype — Android offline release candidate in
progress**. The two prompt files remain intentional uncommitted owner work.

## P57 tutorial elimination target readability and refreshed real-touch route - 2026-08-30

The source checkpoint is now committed as `c9e3d3091a38852be794f74ad97420b91461599a`
(`tutorial: place elimination target in readable lane`). This is a tutorial-only layout
adjustment: actor 11 is at `(0, 1, -3.2)` in the open south lane, while production spawns,
the MovementLab fixture, offline authority and package/network policy are unchanged. The
new PlayMode coverage checks the layout and resolves a local projectile against that target.

The exact candidate was rebuilt and checked locally: `validate.ps1` is **0/0** (post-checkpoint
log SHA-256 `7EF09129DBD03921DF243F43AC65AE932A8C74C4DD76FAD8E6A013BFC804E322`), EditMode is
**141/141**, the target checkpoint PlayMode is **90/90**, and the focused follow-up suite is
**91/91**. The APK is **40,681,055 bytes** (`DA6CC4B6B2F4160A2D62BDE9FFA4C1686D0D401AB0F354604AF8AC077269222B`); the AAB is
**36,506,363 bytes** (`A379C725D46E8829F9DE9EEF59E49D906E817F9F6392E031EE065A532DD6C37C`). The
release checker remains **0 errors / 0 warnings** and confirms the temporary package,
offline permissions, ARM64 payload and static alignment. Exact XML/log hashes and artifact
paths are indexed in `Docs/V1_RELEASE_PLAN.md` P57.

On approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 4 KB pages), Aim Assist
was enabled in Settings & Accessibility. A fresh real-touch run completed MOVEMENT, AIM,
BASIC ATTACK, ABILITY, GADGET and AANDHI; the adjusted target then unlocked the ELIMINATION
card. `Builds/Local/Device/final-circle-20260830/tutorial-target-aimassist-moveclose-step7-final.png`
(SHA-256 `27E22DED8D06035806F3EE85B339286A28019025B20B32BC099BC4BE2B77A76E`) is the exact
step-7 capture showing `CONTINUE`. The offline match then resolved to a real `RESULTS /
WINNER YOU / #1` placement (`tutorial-target-after-wait.png`, SHA-256
`9710C1C2A111652CC79E625D0D46E177BF65FC9490636F3FD6C47FCF088FBC7D`), and tapping
`FINISH TUTORIAL` produced `TUTORIAL COMPLETE 8/8`
(`tutorial-target-finished-results.png`, SHA-256
`76E40B9F20BCDAFAE13FF217029CA75A3B92D8B9A23B58F98802C224493BAA49`). The route index is
`Builds/Local/Device/final-circle-20260830/tutorial-target-touch-route-manifest.json`.

This closes the previously observed touch-elimination layout defect for the captured run and
records terminal placement plus tutorial completion, but does not claim full action-by-action
victory/rematch comfort. The product
remains **Prototype — Android offline release candidate in progress**, not Play-ready. Final
authored art/audio, cultural/fun/accessibility review, normalized performance and
battery/thermal endurance, physical 16 KB runtime, final identity/signing, privacy/Data
Safety, content rating and Play Console work remain open and owner-controlled.

## P56 real-touch tutorial comfort probe on the exact current candidate - 2026-08-30

The exact `56df201` candidate was replayed on approved Lava `ST5GDW23LB004392` after enabling
AIM ASSIST in Settings & Accessibility. Real touch advanced MOVEMENT, AIM, BASIC ATTACK,
ABILITY, GADGET and AANDHI. Exact captures and SHA-256 values are indexed in
`Builds/Local/Device/final-circle-20260830/tutorial-touch-route-manifest.json`; the settings
proof is `tutorial-settings-aim-assist-on.png`.

The ELIMINATION card remained waiting after repeated real aim/attack attempts. A later Results
panel showed `WINNER YOU`, `#1 YOU`, `KO 0`, `D 0`, so Aandhi ended that route without a player
KO. This is a useful comfort defect signal, not a completed tutorial claim. P47/P54's physical
`8/8 COMPLETE` result was obtained through SKIP, and the editor eight-step test is deterministic
logic coverage; neither is action-by-action touch comfort evidence.

The final Unity UI tree (`tutorial-route-ui.xml`) exposes only a SurfaceView, so no semantic
button coordinates were available. The app-scoped logcat (`tutorial-route-app-logcat.txt`, SHA
`909FDF92B11825C3229670134770FE238A23069141B871FBDF77DA54D85B1DF4`) contains no configured
fatal/ANR/native-crash markers. This probe does not change the release classification: the
project remains a **Prototype — Android offline release candidate in progress**. Final authored
art/audio, human cultural/fun/accessibility review, normalized performance, battery/thermal,
physical 16 KB, signing, legal/privacy, rating and Play Console gates remain open.

## P58 exact-candidate completion-card dismissal and Results/rematch route - 2026-08-31

Source checkpoint `888421f0b332a2e5b9b41fcb6ae669adec836612` adds a real completion-card
exit: the tutorial secondary action becomes `CLOSE CARD` after `8/8 COMPLETE`, while replay
and menu remain available. The PlayMode regression
`CompletedTutorialCanDismissOverlayForResultsAndRematch` verifies that dismissal hides the
card and is safe when repeated.

The exact rebuilt candidate passed repository validation (**0 errors / 0 warnings**), full
EditMode (**141/141**), full PlayMode (**92/92**) and the release checker (**0 errors / 0
warnings**). APK SHA-256 is
`B3D4EF4749270FDAD30474113683E050693BFA013173FF5EB1E3848C26C87F44`; AAB SHA-256 is
`CC5D2B362EA8330BB3FA22E93D530CD018D4933305744E26EF2504300B88D6F6`. Exact logs, sizes and
technical gate details are indexed in `Docs/V1_RELEASE_PLAN.md` P58.

On approved Lava `ST5GDW23LB004392` only, real touch opened Tutorial, used SKIP to reach
`TUTORIAL COMPLETE 8/8`, tapped `CLOSE CARD` to expose the live HUD, waited to Results, and
tapped REMATCH to open a fresh TutorialArena movement card. The exact screenshot/UI/logcat
hashes and route caveats are in
`Builds/Local/Device/final-circle-20260830/tutorial-dismiss-route-manifest.json`. The UI
tree exposes only Unity's SurfaceView, and the app-scoped log contains no configured fatal,
ANR, SIGSEGV or SIGABRT marker; known Lava gralloc/AHardwareBuffer format-allocation noise
is retained as a non-fatal observation. This is route evidence, not action-by-action tutorial,
repeated-rematch, human comfort, fun or final visual approval.

The project remains a **Prototype — Android offline release candidate in progress**. Final
authored art/audio, cultural/fun/accessibility review, normalized performance and endurance,
physical 16 KB runtime, final package identity/signing, privacy/Data Safety, rating and Play
Console gates remain open.

## P59 exact-candidate all-fighter and accessibility route - 2026-08-31

Without changing the P58 source or artifacts, the exact candidate was exercised further on
approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, reported 4 KB pages).
Fresh route evidence covers all three fighter cards and live openings for Bijli, Maya and
Pehel, attack and ability-input checkpoints, Tiffin Station feedback, in-match Settings &
Accessibility, left-handed controls, reduced flashes, high contrast, aim assist, text
scaling, persistence and reset-to-defaults. The connected Oppo was excluded.

The route index is
`Builds/Local/Device/final-circle-20260830/p58-fighter-accessibility-route-manifest.json`
(7,285 bytes; SHA-256
`F9D43C679971029EC9CC8881913A0BF62A28555A2F7C14C7A1FB94554C7D2409`). The exact APK/AAB
remain the P58 temporary-ID/debug-signed artifacts. The UI tree is SurfaceView-only. The
15,630-byte app-scoped logcat has no configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or
`SIGABRT` marker; known Lava gralloc/AHardwareBuffer, Play Core class-probe and Swappy
diagnostics remain recorded as non-fatal platform noise.

This adds bounded current-candidate observation only. It does not claim action-by-action
tutorial comfort, repeated-rematch comfort, normalized performance, physical 16 KB runtime
proof, final authored art/audio, human accessibility/fun/fairness/cultural approval or Play
Store readiness. The project remains **Prototype — Android offline release candidate in
progress**.

## P66 lifecycle input hardening and exact Android candidate rerun - 2026-08-31

The current source tip is `e603ce7e7f1cb279f5e3e9d606ea5eae89603ecb`. Android pause now
clears the player adapter, virtual stick and action-button transient state, and the HUD clears
the adapter before opening its lifecycle pause boundary (ADR-076). The focused lifecycle
regression is **1/1**, full EditMode is **141/141**, full PlayMode is **92/92**, and static
validation is **0 errors / 0 warnings**. The exact rebuilt APK is 40,679,115 bytes (SHA-256
`349F02C67DE4CC801C5CB81B9CEC375A18D89B136C1A3AD9BB9549E9640A41CB`) and the AAB is
36,504,445 bytes (SHA-256
`9A5BE261D2504007BCBAF4105568F19437CBA8A4DEFAA3383371DE35386D51E0`).

The APK was installed only on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android
14/API 34). The route manifest is
`Builds/Local/Device/final-circle-20260830/p66-lifecycle-route/p66-lifecycle-route-manifest.json`
(8,192 bytes; SHA-256
`217589DAE7592EC397328F12D8C3DF88246B7AEE035776584DF5FE9624499103`). It records a live
Solo Raja/Bijli Opening Fight capture, approximately five seconds at Android HOME, and a
return to the same RESUMED Unity activity. Before/after captures retain `ALIVE 8` and
`ZONE 14.0 > 11.0`; Lava reports 4,096-byte pages and the route logcat has zero configured
fatal, ANR, native-crash or managed-exception markers. Bundletool 1.18.3 universal extraction,
direct/extracted `zipalign -P 16` and temporary v3 `apksigner` verification also passed for
this exact AAB; APKS/universal hashes are retained in the P66 manifest.

This is bounded exact-candidate evidence. It does not close held-input comfort across every
phase, repeated rematches, normalized performance, unplugged endurance, physical 16 KB,
final authored art/audio/accessibility/cultural/fun approval, signing/identity, privacy or
Play Console gates. The truthful state remains **Prototype — Android offline release candidate
in progress**.

## P65 exact P61 physical all-gadget route - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` remained installed on
approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). No rebuild or source
change was made for this bounded route. The manifest is
`Builds/Local/Device/final-circle-20260830/p65-gadget-route/p65-gadget-route-manifest.json`
(4,269 bytes; SHA-256
`48598855AEEDCD286C837219632ACFC6B972CEC0CB69B7E9D8EE28163BDED807`).

The route physically used all three gadgets. Tiffin was deployed after setting an aim
direction (`13-tiffin-aimed.png`, SHA-256
`91928CA0C0FB3A9957DCDDF402A758C98C4460A98BF0D09237AD6CABE88A8748`), Dhol was collected
and used (`18-left-to-dhol.png`, SHA-256
`FD8C97861217A2A5AFBFD6C537954079BC00A328C15B392D58E52B8FBE5D7F4F`; `19-dhol-use.png`,
SHA-256 `C67D9FCBB8F873038598A4F92446AE7B40E9C61DF50B9F14906197BD85CD50E8`), and Umbrella
was collected and used in a fresh run (`23-diagonal-left-progress.png`, SHA-256
`EAD710DF6318380F8BCB8CBD04118BD8E4B3A05BE7B62672EF3BD20BC4E331F7`; `24-umbrella-use.png`,
SHA-256 `B35585DE8FC7A2448413502FCCBB2C2A139D6E2127BCB7DEE2DF836D4FCD0FC8`). The visible
feedback was `TIFFIN STATION DEPLOYED`, `DHOL BURST` and `UMBRELLA GUARD ACTIVE`.

App-scoped logcat `p65-gadget-app-logcat.txt` has SHA-256
`7237171A826BE8F5308B871270F1477551296A903F5E2B5DC626669DAE113E9F` and zero configured
fatal/ANR/native/managed-crash markers. Tutorial SKIP/CLOSE CARD was used after fresh
activity starts; action-by-action tutorial comfort, final presentation, audio, cultural,
endurance, normalized performance and Play gates remain open.

## P64 immediate-after-resume lifecycle pause observation - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` was started on approved
Lava `ST5GDW23LB004392` after tutorial completion. A live Opening Fight baseline was captured,
the app was sent to Android HOME for approximately five seconds, and the Unity activity was
resumed with a screenshot approximately 220 ms later. The route manifest is
`Builds/Local/Device/final-circle-20260830/p64-lifecycle-pause-manifest.json` (3,354 bytes;
SHA-256 `DC3677479D95C4E2EBA7DD79C6E46C03418D58F379AE32708A1C5B2FFCB4EA99`).

Both captures show Bijli at 85/85 HP in Opening Fight with `ALIVE 8` and `ZONE 14.0 > 11.0`;
the warning countdown is 11.6 seconds before HOME and 10.8 seconds in the immediate
post-resume capture. The five-second background interval therefore shows no observed
simulation/zone progression; the approximately 0.8-second movement is after resume during
activity start and screenshot capture. The retained trace and app logcat record the Unity
pause/stop and start/resume callbacks on the same process with no configured fatal marker.

This closes the narrow bounded lifecycle-observation gap, not full phase coverage, held-input
release comfort, endurance or human approval. Normalized performance, repeated-match growth,
physical 16 KB and Play gates remain open.

## P63 bounded exact-candidate live-match performance refresh - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` was already installed on
approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34) and remained in a live
Solo Raja match while the repository performance harness requested 120 seconds at five-second
intervals. Movement swipes plus attack, ability and gadget taps were sent during the run. Raw
evidence is under
`Builds/Local/Device/Performance/20260831-lava-f80b565-p62-perf120/`; the 24-sample
`manifest.json` is 5,611 bytes (SHA-256
`EBB5C4F43E27E0579E0EAC3E116E8A3DB0F47DEB35EAEDEE7C05A945FF067D7D`) and the checked
summary is 3,478 bytes (SHA-256
`4413810D5295345586E497846AA4A55C6B83AC9645FA807EB159ADDEA6B4467B`).

The start capture shows Bijli in Opening Fight (`live-start.png`, SHA-256
`4FCCA4F9BDFEAD5B3391FCCED6C86D1F1D567290A40DB806168C805D4CBAF8A4`); the end capture
shows Final Circle, player defeat/spectating and Alive 4 (`live-end.png`, SHA-256
`720F5D9361887079AAEB3950509360F92B8EA4D658CA42BC931C1BC94FD4C013`). After the first four
samples, PSS was 223,078-223,273 KB (average 223,190.8 KB), RSS was 356,972-357,168 KB
(average 357,085.0 KB), graphics PSS was 17,480 KB, and `top` CPU was 59.2-131.0%
(warm-sample average 113.8%). Thermal status stayed 0; HAL CPU/GPU was 43.537-49.148 C,
skin was 39.305-39.969 C, and USB-powered battery changed from 44% / 3,841 mV / 35 C to
43% / 3,825 mV / 35 C. The app logcat has no configured fatal marker.

This is bounded current-candidate telemetry only. Unity `gfxinfo` still has no usable frame
histogram, Lava reports 4 KB pages, and normalized FPS/frame-time/GC/GPU, unplugged endurance,
repeated-match growth, physical 16 KB runtime and human performance approval remain open.

## P62 accessibility, persistence and lifecycle route - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` was freshly installed on
approved Lava `ST5GDW23LB004392` after clearing app data. The route opened in-match Settings &
Accessibility, toggled left-handed controls, reduced flashes, high contrast, aim assist and
text scale, visibly moved the left-handed live action row, verified persistence after returning
to the menu and relaunching, then restored all changed settings to defaults. The route manifest
is `Builds/Local/Device/final-circle-20260830/p61-accessibility-route/p62-accessibility-route-manifest.json`
(8,901 bytes; SHA-256
`0DAE55EBFC6DD57F78D9BF28D0A9172659102FFB2D30BFBB2ADC8AB610D4BCF9`).

The bounded live background/resume pair returned to Opening Fight on the same exact artifact;
the warning timer moved during the observation window, so full simulation pause invariance is
not claimed. The UI tree remains SurfaceView-only and the 24,935-byte app-scoped logcat has no
configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or `SIGABRT` marker. Accessibility comfort,
smaller-device coverage, sustained lifecycle testing and human approval remain open.

## P61 compact portrait results metrics - 2026-08-31

The current runtime/test checkpoint is `f80b565372d7446e070cf1a37de042bd018345c4`. Compact
results now spell out `KOs`, `AST` and `DMG` instead of the ambiguous `K/A/D` sequence, and
portrait result type is 18 px before the saved text-scale preference. ADR-075 records the
presentation-only decision; placement, damage, rewards and authority state are unchanged.

The rebuilt APK is 40,679,695 bytes (SHA-256
`922DB673B579BD88705BB4483C36A21A2D903A1CD05D2C2F50F47D26A564EA91`) and the AAB is
36,504,994 bytes (SHA-256
`FDBCED4B1D6D69E4F637C283298188B037D58F152DE4D9B69F897147F85093CF`). Repository validation,
full EditMode (**141/141**), full PlayMode (**92/92**) and the release checker all report
**0 errors / 0 warnings**. The package remains temporary `com.example.battleraja.m11`, version
`1.0.0`/code `100`, API `28/36`, ARM64-only, statically 16 KB aligned and debug-signed.

On approved Lava `ST5GDW23LB004392` only, the exact rebuilt APK reached a fresh live match,
player defeat/spectator, Aandhi/final circle, Results and REMATCH. The result capture
`Builds/Local/Device/final-circle-20260830/p60-results-copy-route/05-results.png` (301,689
bytes; SHA-256 `313F180C6177C5A78F80B68D115C0E52E2E44C3FDB79CA157737B62BADC79676`) visibly
shows the expanded labels. The rematch capture `05-rematch-opening.png` is 322,280 bytes;
the route manifest is
`Builds/Local/Device/final-circle-20260830/p61-results-copy-manifest.json` (4,936 bytes;
SHA-256 `0C868F852FE57C409B914871845DF317EFC7C89398CEFD3A8AB98E5F1137671F`). The app log
has no configured fatal/ANR/SIGSEGV/SIGABRT marker; known Lava graphics, Play Core and Swappy
diagnostics remain recorded. This is exact-candidate readability/route evidence, not final
authored UX, localization, comfort, performance, physical 16 KB or Play approval.

The same rebuilt artifact also received a fresh all-fighter checkpoint: Bijli, Pehel and Maya
cards and live openings were captured, attack taps and Tiffin Station use were exercised, and
each ability was repeated after the Opening phase so the Bijli dash cooldown, Pehel charge
cooldown and Maya decoy-active feedback were visible. The route manifest is
`Builds/Local/Device/final-circle-20260830/p61-all-fighter-manifest.json` (5,506 bytes;
SHA-256 `9FA8762B504330189A686605A6DD60836C4992B5359E526B91E8527A813E1598`). This is bounded
exact-candidate action evidence; accessibility toggles, action-by-action tutorial comfort,
repeated-match comfort and human approval remain open.

## P60 compact portrait HUD copy - 2026-08-31

The current runtime checkpoint is `c3cfb27e08f13ecf4b91a4234269aa11e675bfe9`. The compact
portrait match status now spells out `ZONE` instead of the ambiguous one-letter `Z`; the
existing phase, alive count, zone radii, warning and closing information are unchanged.
The change is covered by the full **141/141 EditMode** and **92/92 PlayMode** reruns and
ADR-074.

The rebuilt APK/AAB passed repository validation and the release checker with **0 errors /
0 warnings**. The APK is 40,682,347 bytes (SHA-256
`4EFF24C7251DD57C2FCAA4D280C369175D33FA6C8D26B969ABBAA72D9EAF32A7`); the AAB is
36,507,651 bytes (SHA-256
`D60B09EE6324C0AA75781BF1F9DB8461A6A1AE05D788A9232EA227DBC1349936`). The package remains
temporary `com.example.battleraja.m11`, version `1.0.0`/code `100`, API `28/36`, ARM64-only,
statically 16 KB aligned and debug-signed.

On approved Lava `ST5GDW23LB004392` only, the fresh live-opening capture
`Builds/Local/Device/final-circle-20260830/p60-live-zone-copy.png` (324,315 bytes;
SHA-256 `13AEFABE9A51364B28B85B6293B2237D6D7189C32278863E591964C252FE8A3D`) shows
`GET READY` and `ALIVE 8  ZONE 14.0 > 14.0`. The route manifest is
`Builds/Local/Device/final-circle-20260830/p60-zone-copy-manifest.json` (3,822 bytes;
SHA-256 `B235CAC4A041644B7A05FED6C613A5BB2563CDD6929C19EF9E2B6F445F1C7E39`). The
app-scoped logcat has no configured fatal/ANR/SIGSEGV/SIGABRT marker; known Lava graphics
diagnostics remain recorded.

This is a localized readability improvement with bounded device evidence. It does not claim
final visual, localization, comfort, performance, thermal, battery, memory-growth, physical
16 KB or Play approval. The project remains **Prototype — Android offline release candidate
in progress**.
