# BattleRaja V1.0 release plan

This is the live owner-directed continuation checklist. It records machine-verifiable
progress and keeps the final classification honest. The current product remains a
prototype until every V1 completion gate and remaining human gate passes.

## Scope lock

- Target: fully offline Android V1.0 local release candidate.
- Platforms for this goal: Android only. Web, Photon gameplay, PlayFab, accounts,
  cloud progression, matchmaking, social features, ads, IAP, online leaderboards and
  analytics upload are out of scope.
- Approved physical evidence device: Lava `ST5GDW23LB004392` (`LAVA LXX508`) only.
- Preserve internal networking/Web seams without exposing unusable public online paths.

## Checkpoint 0p - visual polish and menu presentation — 2026-09-04

The editable production-art baseline now gives Bijli, Pehel and Maya separated faceted
shoulder, gauntlet and leg silhouettes. The Bazaar Bastion feature art fills the menu
card with an envelope crop, removing the low-information dead block while keeping the
controls and safe-area layout intact. Source commits: `775497d`, `281eeb4`, `4ebf65f`;
the last commit mirrors the confirmed post-respawn state into the visible actor and
health card without changing Bastion domain rules.

- Static validation: **0 errors / 0 warnings**.
- Full EditMode: **164/164 passed**; final XML SHA-256
  `5D43596B6246CFF916B83A43459728537CBADD5B8947DB33CABE37F7EBE5D5DF`.
- Full PlayMode: **99/99 passed** after the respawn handoff regression; XML SHA-256
  `67E02CE34AFB41222D56A8A4633392FDC9716CB8A7D40AD4D1228A34669D93B4`.
- Corrected APK: **41,683,648 bytes**, SHA-256
  `6A16D07EBA66C7420E5F1AABD7982E27C40C6BB017FC639E2D87974B85DE60DC`.
- Corrected AAB: **37,509,156 bytes**, SHA-256
  `337C15FF7169A97FED2F711822C5366BF731A388D954C8778A9BF33A9E4DB9DA`.
- Technical release checker: pending its post-commit clean-tree rerun for temporary
  package `com.example.battleraja.m11`, API `28/36`, offline permissions, ARM64/static
  alignment and store dimensions.
- Approved Lava `ST5GDW23LB004392`: corrected APK installed and pulled base hash
  matched. The focused Bijli route captured the explicit `OUT OF ACTION` card at `0/85`
  and restored `85/85` after respawn; the scoped route logcat is marker-clean. The
  broader exact-source continuation selected Pehel and Maya, visibly deployed Tiffin,
  exercised accessibility toggles, HOME/resume and Rematch, and Lava reports 4 KB pages.
- The exact APK also launched on the local `BattleRaja_16K` Android 16/API 36 AVD with
  `getconf PAGESIZE=16384`, ABI `x86_64,arm64-v8a` and zero configured app
  crash/native/shader markers. This is emulator evidence, not physical-device proof.
- Bounded 30-second diagnostic: settled PSS 248–259 MB, RSS 371–382 MB, graphics PSS
  75–80 MB, instantaneous CPU 39–62%, thermal status 0; no normalized FPS or
  endurance claim because `gfxinfo` exposed no usable histogram.
- Follow-up approved-Lava route on the same APK (Maya selection persisted) exercised
  the in-match accessibility toggles, HOME/resume, terminal Results and the visible
  Rematch control. `16-results.png` records `WINNER RIVAL • Clock` with Raja 6/15 and
  Rival 11/15; `17-rematch-live.png` resets to 00:04 with Maya at 85/85 and all eight
  fighters visible. The route logcat is marker-clean. The corrected route below adds
  explicit out-of-action/respawn evidence; reliable attack/ability success and
  spectate-camera comfort remain open.

Evidence: `Docs/QA/V1_VISUAL_POLISH_2026-09-04.md`. This closes presentation
readability plus a bounded current-source lifecycle/respawn route; commissioned final
assets/audio, spectate-camera comfort, complete all-fighter action coverage,
accessibility/cultural/fun review, sustained performance, genuine 16 KB runtime and
owner Play gates remain open.

## Checkpoint 0o - authoritative Bastion respawn handoff — 2026-09-04

The Bastion Crown authority now requires an explicit confirmation after a respawn has
been issued. The ticket is reserved once, the actor stays dead/spectating while the
adapter applies the spawn, and retry advances re-emit the same actor without a second
spend. `SyncParticipant` cannot mirror-revive a participant, combat delivery is rejected
before live state and after terminal resolution, and the handoff marker is included in
the deterministic hash. Source commit: `0d0f875`.

- Static validation: **0 errors / 0 warnings**.
- Focused Bastion tests: **16/16 passed**.
- Full EditMode: **164/164 passed**.
- Full PlayMode: **98/98 passed**.
- Strict production-bot PlayMode: **98/98 passed** across the 100-match harness.
- Batch metrics: **100/100** terminal, **89/100** in the 240–360 second window,
  **92/100** combat-positive, **100/100** with bot-to-bot damaging pairs, **0/100**
  Aandhi-only, zero protected-warmup damage, zero invalid positions, **284** respawns
  and **0** stuck ticks.
- APK: **41,681,228 bytes**, SHA-256
  `0A4E7C96531F16ABAFDB4BDFB2CD587175360210B543FADEC19BF9B06DB91108`.
- AAB: **37,506,760 bytes**, SHA-256
  `19E0B84A8CACB760CA18DFDD8FC7AA3B5AE9232FB7F4E52F47A22B28DA6E842E`.
- Technical release checker: **0 errors / 0 warnings** for temporary package
  `com.example.battleraja.m11`, API `28/36`, offline permissions, ARM64/static 16 KB
  checks and store dimensions.
- Approved Lava `ST5GDW23LB004392`: exact APK installed, pulled base hash matched,
  branded menu rendered and scoped crash-marker log was clean. Lava reports 4 KB pages;
  this bounded smoke is not physical 16 KB proof or a complete route review.

The full evidence record is `Docs/QA/V1_RESPAWN_HANDOFF_AUTHORITY_2026-09-04.md`.
This closes the authority handoff edge case only; final authored assets/audio, physical
route and comfort, normalized performance/endurance, genuine 16 KB runtime, human
reviews and owner Play gates remain open.

## Checkpoint 0n - Bastion Crown timer step-size determinism — 2026-09-04

The Crown rotation timer now carries overdue time through every crossed interval, so a
coarse authority advance and the equivalent fixed-step replay land on the same socket and
remaining timer. Source commit: `bad12de`.

- Static validation: **0 errors / 0 warnings**.
- Focused coarse-vs-fixed Crown regression passed.
- Full EditMode: **162/162 passed**; XML SHA-256
  `5C172CD9B52C598277D3C00F43A276D0A08FF5DA4FCE276C2C326F9C1C3892C1`.
- Full PlayMode: **98/98 passed**; XML SHA-256
  `108D32758C5C0D783011FD7C4F6691684D6E0279CB9157FBB46BBCD80FACE855`.
- APK: **41,680,452 bytes**, SHA-256
  `E92E5994C36B35414DB44D32C082DC8992A3E413F9B67BD87FF776BF5C42DF6C`.
- AAB: **37,505,982 bytes**, SHA-256
  `19882B28E14DE5D9A0B73CCF7016FCA0983325C1F93C4E4BDD36D7E908FB470F`.
- Technical release checker: **0 errors / 0 warnings**; package remains temporary
  `com.example.battleraja.m11`, API `28/36`, offline permissions, seven ARM64 libraries
  and static 16 KB ELF alignment.
- Approved Lava `ST5GDW23LB004392`: exact APK installed after package clear; pulled base
  matches. Fresh menu/crash-marker evidence is under
  `Builds/Local/V1GameplayTruth/Next/crown-rotation-20260904/`.

The full evidence record is `Docs/QA/V1_CROWN_TIMER_DETERMINISM_2026-09-04.md`. This
closes one objective-timer edge case only; AI fairness, physical comfort, normalized
performance/endurance, genuine 16 KB runtime, authored assets, accessibility/cultural/fun
review and owner Play gates remain open.

## Checkpoint 0m - Bastion squad command-window determinism — 2026-09-04

The authoritative squad blackboard now keeps one shared snapshot for the entire bot
callback window. Callback-side state mutations become visible on the next preparation tick,
so callback order cannot make later teammates replan at the same simulation tick. Pure-domain
callers outside the window retain immediate refresh behavior. Source commit: `8e3563a`.

- Static validation: **0 errors / 0 warnings**.
- Focused command-window regression passed.
- Full EditMode: **161/161 passed**; XML SHA-256
  `8B4DCC3B571FC51AADC646604F5B875398861890E4A84EC2F152C4EE18DF892A`.
- Full PlayMode: **98/98 passed**; XML SHA-256
  `B3FE89180E76435A1912733EF00750DD334A2C9770472B1F6C2E9ED72B40BEA5`.
- APK: **41,680,960 bytes**, SHA-256
  `976EE4D767DC4BC88DB9EB3D499603515D576DF9A205E4E07BF1D87A1CBAA43A`.
- AAB: **37,506,508 bytes**, SHA-256
  `CE06B7B8C9CA9B67D8AF4796FD6360CEF4430B539BF34F379BC32D9E5F1ECF8F`.
- Technical release checker: **0 errors / 0 warnings**; package remains temporary
  `com.example.battleraja.m11`, API `28/36`, offline permissions, seven ARM64 libraries
  and static 16 KB ELF alignment.
- Approved Lava `ST5GDW23LB004392`: exact APK installed after package clear; pulled base
  matches the APK hash. Fresh menu/crash-marker evidence is under
  `Builds/Local/V1GameplayTruth/Next/squad-window-20260904/`.

The full evidence record is `Docs/QA/V1_SQUAD_COMMAND_WINDOW_2026-09-04.md`. This closes
one deterministic coordination edge case only. AI fairness, full physical 4v4 comfort,
normalized performance/endurance, genuine 16 KB runtime, authored presentation,
accessibility/cultural/fun review and owner Play gates remain open.

## Checkpoint 0l - tutorial safety and repository CI repair — 2026-09-04

The tutorial terminal path now has an explicit rule definition: automatic timeout and
last-participant resolution are disabled until the player completes the guided lessons and
advances into Victory. The authority then publishes a deterministic player-first result;
normal Solo and Bastion definitions are unchanged. The repository validation workflow was
also repaired after run `33816621445` failed in the secret scanner: the scan now uses
portable POSIX ERE with explicit native exit handling, and checkout uses `actions/checkout@v5`.

- Source: `a7ea3ce`; remote `main` is fast-forward aligned.
- GitHub repository validation: run
  [`33836993117`](https://github.com/neonvarun/BattleRaja/actions/runs/33836993117), all
  steps **passed**.
- Static validation: **0 errors / 0 warnings**.
- EditMode: **160/160 passed**, XML SHA-256
  `095E4483D76F97FC0053969C91585DFBA40B5F6841FA75DCBD5EDF5550A54D7D`.
- PlayMode: **98/98 passed**, XML SHA-256
  `2FF73B300915CB2198111EDDEBBAD62EB9FE5EEEECA707A4A7241D7F0F3AB808`.
- APK: 41,680,960 bytes, SHA-256
  `36AEBACF19F098D3F5763539CBB854C1A0BE6E4F8ADB3CC38BF6171E0856CB0D`.
- AAB: 37,506,488 bytes, SHA-256
  `1D26950F1C85A4F97FD7BB5E2D0938D906EF1DC03D1B7408A4BE77B6282C730A`.
- Technical release checker: **0 errors / 0 warnings**, temporary package
  `com.example.battleraja.m11`, API `28/36`, ARM64-only and static 16 KB checks passed.
- Approved Lava `ST5GDW23LB004392`: exact APK installed after a package clear. Real touch
  reached movement, aim, attack, ability and the live gadget lesson; captures are under
  `Builds/Local/V1GameplayTruth/Next/tutorial-safety-20260904/`. The SKIP path still
  exposes the dismissible `TUTORIAL COMPLETE 8/8` card. Full post-fix physical pickup,
  elimination, Victory and Results comfort remains untested.

The full evidence record is `Docs/QA/V1_TUTORIAL_SAFETY_AND_CI_VALIDATION_2026-09-04.md`.
This closes a reproducible tutorial/CI defect, not the V1 product gate. Generated art/audio,
full physical comfort/accessibility/lifecycle review, normalized performance/endurance,
physical 16 KB runtime, permanent identity/signing, privacy/Data Safety, IARC/content
rating, cultural review and Play Console actions remain open.

## Checkpoint 0k - settings surface clarity pass — 2026-09-04

The menu and in-match pause settings now use original icon-backed tiles and accent rails,
with explicit ON/OFF state labels for all four accessibility toggles and a redundant
high-contrast state cue. `BattleRajaSettingsGlyph` is a render-only uGUI graphic; existing
button targets, local preference keys, lifecycle pause behavior, authority, replay and
collision are unchanged.

- Static validation: **0 errors / 0 warnings**.
- EditMode: **159/159 passed**, XML SHA-256
  `F8C9BF60F77873E9D906E2D0E8726946A58B8593C36CCCD845C086234A85914C`.
- PlayMode: **96/96 passed**, XML SHA-256
  `3BE1BA5E2EA8887C4D52DC8F11AE3FD0D64921DB3960409DE30AF9080AAAAC4B`.
- APK: **41,678,776 bytes**, SHA-256
  `714ACE23E8C9DA859B91B14E12F9E7E65CA277ADAAAB315F1C81B4547D195C93`.
- AAB: **37,504,322 bytes**, SHA-256
  `74F7EDC96481EA868FF1A8F078E70D6C407126AD9A197BF2020D044F108445CC`.
- Technical release checker: 0 errors / 0 warnings; package `com.example.battleraja.m11`,
  version `1.0.0` / code `100`, API `28/36`, no network permissions, seven ARM64 native
  libraries, static 16 KB alignment and store dimensions. Log:
  `Builds/Local/V1GameplayTruth/Next/release-checker-settings-polish-final-clean-20260904.log`
  (SHA-256 `E473003AB9CD043A637AE878B01772609EABB28A7AC7DA6F23156C7127522FF4`).
- Approved Lava `ST5GDW23LB004392`: exact APK installed and pulled base hash matches.
  Fresh menu/menu-settings/live/pause-settings/high-contrast captures are in
  `Builds/Local/V1GameplayTruth/Next/settings-polish-final-20260904/`; scoped logcat has
  zero configured fatal markers. Live six-sample/30-second bounded capture:
  PSS **298,369–304,625 KB**, RSS **417,808–424,064 KB**, graphics PSS
  **89,324–95,480 KB**, raw CPU **106–127%** (mean **116.3%**), battery **63% → 63%**,
  thermal **0**. Device `gfxinfo` exposed no frame timing; no FPS claim is attached.

This closes a settings-state/readability gap only. Final commissioned art/audio, full
physical comfort/accessibility/lifecycle review, normalized GPU/GC/endurance, physical
16 KB runtime, permanent identity/signing and owner privacy/Data Safety/IARC, cultural
and Play approvals remain open; classification remains **Prototype — Android offline
release candidate in progress**.

## Checkpoint 0j - touch-control clarity pass — 2026-09-04

The production touch surfaces now carry original vector glyphs for move, aim, attack,
ability and gadget, with slightly tighter portrait sizing that preserves comfortable target
areas. The pass is presentation-only: common input commands, authority, replay and collision
remain unchanged.

- Static validation: **0 errors / 0 warnings**.
- EditMode: **159/159 passed**, XML SHA-256
  `7C2A2BF9CD76422E8CA454DDF3CD6CDD1D92E888E1EB8298DC29349B87A92F8E`.
- PlayMode: **95/95 passed**, XML SHA-256
  `0D2A47CF1EE234545496658DFA19F3D7A76B2934B22D5A02C76A447D09AC4AFB`.
- APK: **41,668,888 bytes**, SHA-256
  `8B9D11BFDB40A75D7C301A255B71D74516BD83F7D5672730FFFFA34A635E9C71`.
- AAB: **37,494,403 bytes**, SHA-256
  `F877BF07F6CBBCF890DB2968B5D48CBB93FC64CCB2DC10D4CCA709679EC99BBC`.
- Technical release checker: passed target SDK 36, offline permissions, ARM64/static 16 KB
  alignment and store dimensions.
- Approved Lava `ST5GDW23LB004392`: exact APK installed (pulled base SHA matches) and the
  action probe reached the expected Tiffin invalid-placement edge state. Captures are in
  `Builds/Local/V1GameplayTruth/Next/touch-glyph-final2/`; process-scoped logcat had zero
  configured fatal markers. Six-sample, 30-second performance evidence under
  `performance-touch-glyph-final2-20260904/` reports PSS **296,208–302,448 KB**, RSS
  **408,496–422,188 KB**, graphics PSS **87,528–93,684 KB**, raw app CPU **97–140%** (mean
  **113.3%**), thermal **0**, battery **69% → 69%**. SurfaceFlinger returned no frame
  timestamps, so no FPS claim is attached.

This improves control discoverability but does not close final art/audio, full physical
comfort/accessibility/lifecycle, normalized performance/endurance, physical 16 KB runtime,
temporary identity/signing, privacy/Data Safety/IARC, cultural review or Play approval.

## Checkpoint 0i - hero silhouette and portrait framing pass — 2026-09-04

The latest visual continuation keeps the established render-only authority boundary while
adding connected neck, chest, limb, waist and knee pieces plus signature weapon silhouettes
to Bijli, Pehel and Maya. The meshes are generated and saved by
`ProductionArtBuilder.cs`, attached to the existing presentation rig by
`ProductionPresentationBuilder.cs`, and carry no colliders or gameplay state. The portrait
camera cap is now **1.6x**, keeping the Bastion lane and fighters larger on the tall Lava
viewport.

- Static validation: **0 errors / 0 warnings**.
- EditMode: **159/159 passed**, XML SHA-256
  `CAFECE2084FCB0A73980F51FB486125F2BA9EAE04C24579ADD125D377296827B`.
- PlayMode: **95/95 passed**, XML SHA-256
  `1D8CA1AD5C09C82880569C4D9F6AA49BF019D988267CB0E069567FC9F13AE6D9`; the framing
  follow-up also passed **95/95**, XML SHA-256
  `A66ABE558015325098C24EF7486060924FC84E240C71BEC1B7800B62CEDA68A0`.
- APK: **41,667,212 bytes**, SHA-256
  `675945B1E3CB7C1471CE7C65C299B17A0969104C969D57EE5083F608436FFA04`.
- AAB: **37,492,707 bytes**, SHA-256
  `FC0A070E204F3F8C788A38E72A66C70934162382E2B7FC74CA8FB18844C72556`.
- Technical release checker: passed target SDK 36, offline permissions, ARM64/static
  16 KB alignment and store dimensions.
- Approved Lava `ST5GDW23LB004392`: exact APK installed and pulled base hash matches;
  device is `1080x2460`, Android 14/API 34, 4 KB pages. Fresh route captures are under
  `Builds/Local/V1GameplayTruth/Next/hero-framing-20260904/` (menu, fighter select,
  live 4v4, rematch). Bounded 30-second live sample under
  `Builds/Local/V1GameplayTruth/Next/performance-hero-framing-20260904/` reports PSS
  **298,267–300,427 KB**, RSS **410,240–419,944 KB**, graphics PSS
  **89,288–93,392 KB**, raw app CPU **81.8–138%**, CPU/GPU **41.993–42.987 C**,
  battery **74% → 74%**, thermal **0**, and no configured fatal markers. The
  SurfaceFlinger ring was zero-filled for this sample, so it is not treated as an FPS
  claim.

This improves phone readability but does not close the product gate. Generated baseline
art, full all-fighter/tutorial/accessibility/lifecycle comfort, normalized GPU/GC and
endurance, physical 16 KB runtime, permanent identity/signing and owner privacy/
Data Safety/IARC/cultural/Play approvals remain open. Classification stays
**Prototype — Android offline release candidate in progress**.

## Checkpoint 0h - production Bastion player HUD consolidation — 2026-09-04

The live Bastion Crown route now presents one compact production player card instead of
stacking the legacy Solo diagnostics. `BastionPlayerHud` is an editable runtime adapter
that shows the human fighter's identity/role, health, attack and ability readiness,
gadget state and friendly feedback; it hides `BijliHud` and `GadgetHud` only for Bastion
and keeps them available in the MovementLab fixture. A focused PlayMode test covers the
card and suppression contract.

- Fresh EditMode: **159/159 passed**, XML SHA-256
  `04CE454016166816A88BE34E53BCAEFFDCA929786D0161F9DBCC3EC3E4527DD3`.
- Fresh PlayMode: **95/95 passed**, XML SHA-256
  `CA5E0AC3FAC1E623612DC19874DA91429772B048A3D0B89E5DEF2E5B0B3B2054`.
- Static validation: **0 errors / 0 warnings**; `git diff --check` passed.
- APK: **41,608,148 bytes**, SHA-256
  `3CA0777D8B94E5381D08794BCA48BDCDE070675F4B54CB32EEFF31FE004C07F2`.
- AAB: **37,433,660 bytes**, SHA-256
  `1996369E050A8C77BE6098618BC20ACB6388CD11C2A2A54A44249780BDFE6E95`.
- Unity build log SHA-256:
  `C248A5CE489C78AC4552A68249ABFAE02794E1F73EB3CA9B95C7ADC03409F772`.
- Approved Lava `ST5GDW23LB004392` route evidence is under
  `Builds/Local/V1GameplayTruth/Next/lava-player-card-20260904/`; it visibly confirms
  the card in live 4v4 and its removal on the results screen. The installed APK base
  hash matches the local APK; the Unity accessibility dump remains `unitySurfaceView`
  only.
- Six-sample/30-second live capture manifest SHA-256:
  `6A4B00F6CBD8CBF2C68A10770BFE7BC7D488127A68A6D439E2E69E0B57321BFF`. PSS is
  **296,836–301,395 KB**, RSS **407,992–421,284 KB**, graphics PSS **89,244–93,348 KB**,
  raw app CPU **94.2–103%**, CPU/GPU **41.03–41.657 C**, battery **81% → 81%**, thermal
  **0**, no configured fatal markers.
- Live-only SurfaceFlinger file SHA-256:
  `81316B42C3F642188C7ACBB36C086109D7EDA5079BB474EFCBDD7DAB0B2B10D8`; 126 presented
  frames / 125 intervals, mean **16.934 ms** (~**59.05 FPS**), p50 **16.534 ms**, p95
  **16.585 ms**, p99 **33.097 ms**, max **33.350 ms**, one interval above 33.33 ms and
  none over 50 ms. This remains bounded compositor evidence, not normalized GPU/GC or
  endurance approval.
- Clean technical checker log:
  `Builds/Local/V1GameplayTruth/Next/release-checker-player-card-20260904.log`, SHA-256
  `80AE8FF493B75D46083859544692A92D91715E00E32C9EBAD0B74F4D17B0A3B2`; repository,
  target SDK 36, offline permissions, ARM64/static 16 KB and store dimensions passed.

The candidate remains **Prototype — Android offline release candidate in progress**;
commissioned final art, full tutorial/accessibility/lifecycle comfort, physical 16 KB
runtime, normalized performance/endurance, final identity/signing and privacy/
Data Safety/IARC/Play owner gates remain open.

## Checkpoint 0a - physical player KO/spectator/respawn proof — 2026-09-04

The exact HUD APK (`3CA0777D8B94E5381D08794BCA48BDCDE070675F4B54CB32EEFF31FE004C07F2`)
was driven on approved Lava `ST5GDW23LB004392`. The player was held outside the
shrinking Aandhi until the card showed **5/85** (`ko-watch-05.png`, SHA-256
`3997C4E3950FAFCB520F9310456670E680B0972821DABBD09581812D938D86AD`), then **0/85**
with `OUT OF ACTION • respawn or spectate an ally` visible (`ko-watch-06.png`,
SHA-256 `FB543967E71126D7A9BF84063EC97A998EEC8E667DAC5044B6D0CB913DD0675C`; held
in `ko-watch-07.png`, SHA-256
`D8191EAAE9751B7199ECB5626CBAC029BB6FA451C07FE344B635DA3ABCCFE5BE`). The card
returned at **81/85** after Raja tickets changed from 11 to 10
(`ko-watch-09.png`, SHA-256
`F1F7C5D7682CAEE1DE0F4004374095392855AA632D08A0ADE460046387B0B8B3`). This closes
the missing physical player KO/spectator/respawn observation for this candidate.
The run ended with `airplane_mode_on=0`, so the existing airplane-mode offline launch
evidence remains the authority for that separate condition; Unity's accessibility
tree still exposes only `unitySurfaceView`.

## Checkpoint 0g - portrait backplate grounding — 2026-09-04

The latest visual refinement keeps the tighter portrait framing while grounding the
overscan outside the playable plaza. `ProductionEnvironmentBuilder` now creates an
editable tiled `BackdropBox` mesh and an unlit `Backdrop` material beneath the
collider-free gameplay mosaic. This preserves the Bazaar palette without the extra
full-screen lit/shadow cost of the rejected mosaic experiment; authority, replay, AI,
economy and input code are unchanged.

- Environment rebuild log:
  `Builds/Local/V1GameplayTruth/Next/environment-rebuild-backdrop-unlit-20260904.log`,
  SHA-256 `1E870405C02EA5768E0A44853B029D4A3DABED94FFFB9770ABB17DFF12C1B954`.
- Exact APK: **41,598,480 bytes**, SHA-256
  `9E5BFF2F28FC857D6E65E11A158942565E59A64AF68DA7F653C1F511060901B8`.
- Exact AAB: **37,424,015 bytes**, SHA-256
  `91AE29A0165589FC9A5065A7B1579F991DEB54557689F62B94F227C75F1D98EA`.
- Unity build log SHA-256:
  `A0C952E2DC0EFA60FCC9D0D3009CF5647296D77AA212F9F9E3D8E3A75B896518`.
- Exact source gates: EditMode **159/159** (SHA-256
  `FA1A2211ED4730DEF9B28CBCCECE47D8567353A69E232EA9E97E196A0113D158`) and PlayMode
  **94/94** (SHA-256
  `3AEB88102BB7C37BAAD6761063EB43C5C76CC910A078B093B0535DF6E2FE581B`); static
  validation **0/0**; two-seed × 8,400-tick replay/deterministic soak **0 divergence**.
- Approved Lava `ST5GDW23LB004392`: exact APK installed and hash-matched. The current
  route under `Builds/Local/V1GameplayTruth/Next/lava-camera-art-20260904/` reaches
  menu, briefing, fighter choice, live 4v4, 04:02 results (Rival winner) and a 00:05
  rematch reset. Final compositor evidence reports mean **16.670 ms** (~**59.99 FPS**),
  p95 **16.565 ms**, p99 **16.590 ms**, max **33.355 ms**, one interval over 33.33 ms,
  none over 50 ms. The six-sample/30-second capture reports PSS **295,058–306,074 KB**,
  raw CPU **100–127%**, thermal status **0**, battery **87% → 87%**, PSS change **2.40%**
  and no fatal markers.

The candidate remains **Prototype — Android offline release candidate in progress**:
final identity/signing, physical 16 KB runtime, normalized endurance/GPU/GC profiling,
full action-by-action spectator/tutorial/accessibility comfort, commissioned art and
human cultural/legal/Play approvals remain open.

## Checkpoint 0f - environment readability and portrait framing refinement — 2026-09-04

This focused presentation checkpoint keeps the Bastion authority, replay determinism,
AI and economy rules unchanged. `ProductionEnvironmentBuilder` now emits restrained
woven/banded environment detail instead of the earlier high-contrast checker treatment,
and `TopDownCameraController` lowers the portrait framing cap multiplier from **2.8**
to **2.2**. The source rebuild regenerated 12 environment materials, 16 environment
texture assets and `BazaarBastionProduction.prefab`; the assets remain editable and
repository-owned. The environment rebuild log is
`Builds/Local/V1GameplayTruth/Next/environment-rebuild-20260904.log` (SHA-256
`30F4DE942E01C7368DE7BB6DACDEE8E4FFDA0FC901D4FFA2DE85DF28846345E7`).

- EditMode: **159/159 passed**; XML SHA-256
  `FF15744802560C6D2CFAB8E77BC11F2A3048F3F6D9C9116DC0D2FC70FF6FD4FF`.
- PlayMode: **94/94 passed**; XML SHA-256
  `4FA8EA81AB372436B6AB7796C31024262BF2016074FEFF88A812C87899F0A4AE`.
- Current-checkout rebaseline: EditMode **159/159** (`Next/editmode-rebaseline-20260904.xml`,
  SHA-256 `A04D0CC0C31459C241EEF3E3B63A479A43C5EC2B4E5C6BED78D439ECE3CBF9C2`) and
  PlayMode **94/94** (`Next/playmode-rebaseline-20260904.xml`, SHA-256
  `E95C6AED5922793791AD7B42064EDB80D782CEEC5AC12DC10A170966D77FB287`). The rerun
  included the two-seed × 8,400-tick Bastion replay soak and deterministic seeded soak;
  both passed with zero divergence.
- APK: **41,579,392 bytes**, SHA-256
  `54AF0801C2FD696DAD3224E6AD1CDDB7F15D8386094CAB8AAD68F5DFABB950E7`.
- AAB: **37,404,926 bytes**, SHA-256
  `CAE58A648792BB14E77767E9036C073256BA2C8C1CED86DCCD31A42BE656A2F0`.
- Approved Lava evidence: `Builds/Local/V1GameplayTruth/Next/lava-camera-art-20260904/`.
  The route reaches menu, briefing, fighter choice, tight-framed live 4v4, action and
  endgame states, authoritative results at 04:02 (Raja 4/15 with 4 Crown deposits)
  and a fresh 00:04 rematch reset. Results SHA-256 is
  `4CD1558E905B25B9AE22590D40B573610146F6E6222D09C9BD7AA4B9E3A7B5CE`; rematch
  SHA-256 is `1E7F87D51E6F143098C5FE8BDB4F3C063A2018B6DF356C586C7FBFE00B594334`.

The installed APK hash matched on Lava `ST5GDW23LB004392` (`1080x2460`, Android 14/API
34, 4 KB pages), and the fatal-marker scan was empty. A bounded post-rematch sample
measured 298,676 KB PSS, 409,568 KB RSS, 91,292 KB graphics PSS, raw app CPU 2%,
thermal status 0, CPU/GPU 42.314 C and battery 33 C; Unity `gfxinfo` exposed no usable
frame histogram. No explicit player spectator transition, normalized sustained
performance, physical 16 KB runtime or commissioned final-art approval is claimed.
The clean-tree checker output is retained at
`Builds/Local/V1GameplayTruth/Next/release-checker-camera-art-20260904.log` (SHA-256
`E8DA3A7106A0A64421DD709585B549852350088FA75199EF62F9DEBC8B83EF1F`) and passed
repository validation, target SDK 36, offline permissions, ARM64-only native libraries,
static 16 KB alignment and store creative dimensions.

The current approved-Lava frame sample now has a compositor-side record:
`surfaceflinger-latency-10s.txt` (SHA-256
`F7D32C24480683590FFADD1736F6E796FBAA0F51EAE64AE1E3492B1F597FDCAD`) reports 127
presented frames / 125 intervals at 16.667 ms, estimated **59.99 FPS**, p95 **16.569 ms**,
p99 **16.585 ms**, maximum **33.357 ms**, one interval above 33.33 ms and none above
50 ms. The concurrent 30-second Lava capture (`performance-rebaseline-20260904`,
manifest SHA-256 `CD0DB79CF18A00DE0503C4CEA61966961B18F74CE58663B4538146CF3CFB16D4`)
records PSS **301,862–306,949 KB**, graphics PSS **87,212–93,368 KB**, raw CPU
**105–112%**, thermal status **0**, CPU temperature **40.847–41.489 C**, battery level
**87% → 87%**, and zero fatal markers. The history is bounded; GPU utilization, GC
spikes and ten-rematch endurance are not claimed.

## Checkpoint 0e - presentation identity/readability pass — 2026-09-04

The production presentation source now uses restrained woven/banded material detail
and camera-facing Bijli, Pehel and Maya identity accents (eyes, jaw guards and role
silhouette parts). The three production fighter prefabs and 14 generated texture assets
were regenerated from editable source; authority, replay, AI and economy code was not
changed. The art rebuild log is
`Builds/Local/V1GameplayTruth/Next/art-rebuild-20260904.log` (SHA-256
`BB6AFB79D8658CF2333DAA5AEAF94EC94AD6529B1CEFF0837FA21B49A8485699`).

- EditMode: **159/159 passed**; PlayMode: **94/94 passed**.
- APK: **41,549,412 bytes**, SHA-256
  `E5F611282763C443B271F19C9EF63069AC3825E31EBD57DC3550187D3CC945EB`.
- AAB: **37,374,943 bytes**, SHA-256
  `0F7C72459D66816E2E2EB2C20FD18FD15DB46018C45E78C52F65E1D3A65BE967`.
- Approved Lava evidence: `Builds/Local/V1GameplayTruth/Next/lava-art-pass-20260904/`.
  The 04:02 results card recorded Raja 9/15 with 2 Crown deposits and the rematch
  reset to a fresh 00:03 match on socket 3. No explicit player spectator transition was
  observed; the Unity accessibility tree still exposes only `unitySurfaceView`.

The release checker technical gates pass, but this is still a generated presentation
baseline rather than commissioned final art. Normalized performance/endurance,
physical 16 KB runtime, permanent identity/signing and human accessibility, cultural,
legal/privacy and Play review gates remain open.

## Checkpoint 0d - canonical Bastion telemetry and physical route refresh — 2026-09-02

The development-only production harness now writes schema-v2 canonical Bastion telemetry
from `BastionCrownMatch` rather than relying only on the legacy combat mirror. The exact
strict run passed **94/94** (`Next/production-bot-100-telemetry.xml`, SHA-256
`A51EADF828A27B5E2D5BA90DEDEC8A215E76713850965BF02223D9412F3D9A59`). The batch report is
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260901-224046635-9101.json`
(SHA-256 `55049198B0E93D5A037055CEA4A3687F54F083FE39DFF6D5778410FBC9D48DEA`). Across
100 matches it records 100/100 terminal, 92/100 in the 240–360 s window, 91/100
combat-positive, 61/100 with at least three combat KOs, 4 Aandhi-only, 100/100 bot-to-bot
pairs, 524/472 score from 123/88 deposits and 155/208 KOs, 376 tickets spent/respawns,
7 overtime stalemate results, 238 socket rotations, 179,431 squad signals, and
8,436,824 alive-ally spacing samples. Protected-warmup damage and invalid positions remain
zero. The first replay is
`Builds/Local/V1GameplayTruth/ProductionBotReports/Replays/match-9101-20260901-224048351.brr`
with SHA-256 `D0BE5FD5A31F8D256B022C6FCE2176975C46C369C6308872DC007646E0808EDE`.

The superseded post-telemetry Android rebuild passed the composed technical checker:

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,516,072 bytes,
  SHA-256 `33AFA202C521632B4662764340574471F982DF189BC1C5D5F724757BA8680B6E`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,341,598 bytes,
  SHA-256 `3EAE3460F8106AFDD8CD46B10E8DC20F373D66F0BD8731E38F3B0B2CCA48DDF2`.
- Temporary package `com.example.battleraja.m11`, version 1.0.0/100, target SDK 36,
  no network permissions, seven ARM64 native libraries and static 16 KB alignment.
  The artifact remains debug-signed and not publishable.

The same current APK was freshly exercised on approved Lava
`ST5GDW23LB004392` through menu → Bastion briefing → fighter choice → live movement/
combat → Aandhi → results → rematch → second result. Evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-route-20260902/`; selected Aandhi/results/rematch
hashes and the UI-tree limitation are recorded in the superseding QA report. The route
did not physically observe a Crown deposit or an explicit spectator transition, so those
human/device gates remain open.

The post-telemetry artifact route is separately preserved under
`Builds/Local/V1GameplayTruth/Next/lava-exact-20260902/`. It reaches the same 4v4 flow,
an Aandhi/Rival-carrier state, authoritative results and a rematch reset; selected
screens are hashed in `Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-02.md`. No Crown
deposit or explicit spectator transition was observed on this exact-artifact route.

The final camera-facing identity-fix rebuild supersedes those artifact hashes:

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,516,080 bytes,
  SHA-256 `E81A035BE7AAF50D5ED1A994C60B68A2765B92CBDC2228528957713BB62702A0`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,341,603 bytes,
  SHA-256 `DFD0C4516BBC44907E30F16BAAC4D0C373BE81AEC6B3C43DF4FF3C3510972276`.
- The exact final-Lava route is under
  `Builds/Local/V1GameplayTruth/Next/lava-visualfix-final-20260902/`; it reaches live
  4v4, results and rematch on the installed APK. The subsequent source-hygiene rebuild
  is the current artifact pair: APK **41,516,076 bytes** (`6050E1A6EC329F27BC14A1118FB166D278293237B4BC6CBA716B7B700D9FD6FF`) and AAB
  **37,341,603 bytes** (`A2F440649987A8FA04398B629F956AC44267AA1D33FEF571C3264B97051CCB4C`).
  Its exact-Lava route is under
  `Builds/Local/V1GameplayTruth/Next/lava-release-final-20260902/` and reaches the same
  flow. Post-regeneration EditMode and PlayMode are **159/159** and **94/94**. Neither
  physical route observed a Crown deposit or explicit player spectator transition.

## Checkpoint 0 - exact-final balanced continuation — 2026-09-02 (superseded by 0b/0c)

The current working tree supersedes the 2026-09-01 continuation record with the
authority/squad blackboard hardening, deterministic post-respawn team placement and
fair production pacing. The exact evidence record is
`Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-02.md`.

- Static validation: **0 errors / 0 warnings**.
- EditMode: **159/159 passed** (`Builds/Local/V1GameplayTruth/Next/editmode-final.xml`).
- PlayMode: **94/94 passed** (`Builds/Local/V1GameplayTruth/Next/playmode-final.xml`).
- Strict production-bot gate at this checkpoint: **94/94 tests passed**; the earlier
  selected report recorded 100/100 terminal matches, 93/100 in the 240–360 s window,
  93/100 combat-positive and 61/100 with ≥3 combat eliminations. The superseding 0c
  rerun records 92/100 combat-positive and is the current selected evidence.
- Bastion v2 replay soak: two seeds × 8,400 ticks, zero combined-hash divergence; planner
  coverage remains contest 64, escort 64, defend 96, collapse 64 and Aandhi-retreat 32.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, **41,514,464 bytes**,
  SHA-256 `7243A7A324E43FC2C2A274DDF1B27C89166E5E9CF5F39C981D650355F696E9B6`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, **37,339,995 bytes**,
  SHA-256 `ABC16E0F7B499690BA41ECC9CBAB5D243C35E85783B6051E5E1982B51ACE8D48`.
- Release checker: passed temporary package `com.example.battleraja.m11`, target SDK 36,
  no network permissions, ARM64-only libraries, static 16 KB alignment and creative
  dimensions. The package remains temporary/debug identity.
- Approved Lava `ST5GDW23LB004392`: exact APK install, menu → Bastion briefing → fighter
  choice → live arena → action/settings → results → rematch → airplane-mode toggle. The
  six-sample/30-second raw capture reports PSS 60,858–252,074 KB, RSS 176,511–390,924 KB,
  graphics PSS 10,455–77,512 KB, raw top CPU 35.7–57.1%, thermal 0 and no configured
  fatal markers. Lava reports 4 KB pages; no normalized frame histogram is available.

This closes the current local technical continuation gate but not final release approval.
The truthful classification remains **Prototype — Android offline release candidate in
progress**. Final commissioned art/cultural/accessibility/fun review, complete tutorial
physical route, normalized endurance, physical 16 KB runtime, permanent identity/signing,
privacy/Data safety/IARC and Play Console actions remain open; no public publication was
performed.

## Checkpoint 0c - strict production-bot rerun — 2026-09-02

The same current source was exercised again in a fresh Unity process after the tutorial
authority/safety patch. The strict release assertions passed **94/94**. Report:
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260901-220113865-9101.json`
(SHA-256 `D44275C62CDF18ADDD9581020088FAB39685279E0AD896CE8F799C20DA867E73`). It records
100/100 terminal matches, **91/100** in the 240–360 s window, **92/100** combat-positive,
**64/100** with at least three
combat eliminations, **3** Aandhi-only, **100/100** bot-to-bot damaging pairs, zero
protected/invalid samples, 17,714 accepted attacks, 8,293 projectile hits, 12,281/15,110
accepted abilities, 560 effective abilities and 248/293 successful gadget uses. Duration
was **109.765–273.022 s** (average **237.891 s**). The first fresh process completed
100/100 but missed the combat-positive assertion at 89/100; because no source changed,
the difference is retained as pacing variance and an open tuning risk.

## Checkpoint 0b - tutorial authority and safety follow-up — 2026-09-02

The tutorial scene now uses the real authority-driven movement, combat, pickup and replay
path. Its scoped `tutorialMode` delays outside-zone damage while lessons are visible and
widens only the opening gadget collection radius; production Solo/Bastion cadence and
pickup rules are unchanged. The authored layout remains the legacy MovementLab/Solo
training arena with bots disabled, so this follow-up does not claim a dedicated Bastion
Crown tutorial.

- Static validation: **0 errors / 0 warnings**.
- EditMode: **159/159 passed** (`Builds/Local/V1GameplayTruth/Next/editmode-tutorial-authority.xml`).
- PlayMode: **94/94 passed** (`Builds/Local/V1GameplayTruth/Next/playmode-tutorial-authority.xml`).
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, **41,520,532 bytes**,
  SHA-256 `56F3BAB99E304A15548D8073BA6B41EDDCBDE17A2C7476D923B06094D5A9649E`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, **37,346,030 bytes**,
  SHA-256 `19E2E7CCFFD7B2CBA993DE3608D8D62F4A351425AA76D0085138C1DF6DD96BCA`.
- Checker: temporary package `com.example.battleraja.m11`, target SDK 36, no network
  permissions, ARM64-only native libraries, static 16 KB alignment and creative dimensions
  all passed. Final identity/signing remains intentionally unset.
- Lava `ST5GDW23LB004392`: exact APK route reached the Movement, Aim, Basic Attack,
  Ability, Gadget/Tiffin, Aandhi, Elimination and Victory tutorial cards. The target defeat
  is visible in `Builds/Local/V1GameplayTruth/Next/lava-tutorial-20260902/73-elimination-authority.png`;
  the final Victory card is visible in `74-victory-authority.png`. No configured fatal
  markers were present in the sampled logcat window.

The truthful classification remains **Prototype — Android offline release candidate in
progress**. Full Bastion tutorial content, physical results/rematch comfort, final authored
art/cultural/fun review, normalized endurance, physical 16 KB runtime, permanent identity/
signing, privacy/Data Safety/IARC and Play Console actions remain open.

## Checkpoint 0a - previous offline V1 continuation — 2026-09-01

The current working tree completes the Bastion Crown authority/replay hardening pass and
replaces the reference-like menu hero with the original
`Assets/BattleRaja/Art/V1/BattleRaja-FeatureArt-OriginalCandidate.png`. The exact evidence
record is `Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-01.md`.

- Static validation: **0 errors / 0 warnings**.
- EditMode: **155/155 passed** (`Builds/Local/V1GameplayTruth/Final/editmode.xml`).
- PlayMode: **94/94 passed** (`Builds/Local/V1GameplayTruth/Final/playmode.xml`).
- Bastion v2 replay soak: two seeds × 8,400 ticks, zero combined-hash divergence.
- Squad planner coverage: 32 seeds; contest 64, escort 64, defend 96, collapse 64,
  Aandhi-retreat 32.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, **41,510,440 bytes**,
  SHA-256 `5F7438105FE450D6331CFEDEE1FAEEB87FB4F6677EB811A997A02CC8FD7C4AE9`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, **37,335,957 bytes**,
  SHA-256 `87C835570B62C4C3A79C156F94CB7E15C6AD31FCB50A0E8ADB0FDE6672DC4858`.
- Release checker: passed offline manifest, target SDK 36, no network permissions, ARM64-only
  native libraries, static 16 KB alignment and store-creative dimensions. Package identity
  remains temporary/debug `com.example.battleraja.m11`.
- Lava: fresh install and cold launch on `ST5GDW23LB004392`, with menu → Bastion briefing →
  fighter choice → live opening → ability/attack/gadget taps → settings capture. A clean
  six-sample/30-second live telemetry capture found no configured app crash markers; PSS was
  287,530–293,678 KB, RSS 426,940–433,088 KB, graphics PSS 87,024–93,180 KB, CPU
  111–118%, and thermal status 0. Lava reports 4 KB pages, and Unity `gfxinfo` has no
  usable frame histogram.

This closes the local technical continuation gate but not final release approval. The truthful
classification remains **Prototype — Android offline release candidate in progress**. Final
authored/cultural/accessibility/fun review, complete physical route, normalized endurance,
physical 16 KB runtime, permanent identity/signing, privacy/Data Safety/IARC and Play actions
remain open and no public publication was performed.

## Checkpoint 1 - preserve and rebaseline exact source

Status: **Passed with limitations**.

### Source baseline

- Date/time: 2026-08-24 23:31 IST.
- Primary branch: `codex/v1-playstore-release`, ahead of `origin/main` by 131 commits.
- Primary HEAD: `33035e84e86b41956b968f4c628aaa79c1496d49`.
- Remote `origin/main`: `ca6ec3e17e695042664cf3bdbf9889b259b33144`; unrelated to the
  selected continuation tip.
- Primary dirty state preserved:
  - Modified user/context document: `Docs/AI/UnityProjectContext.md`.
  - Untracked visual-development assets under `Art/Concepts/`.
- Historical stashes preserved; none applied or dropped.
- Disposable detached worktree used for exact-source validation/build:
  `C:\Projects\BattleRaja-baseline-33035e8`.

### Tool inventory

| Capability | Result |
| --- | --- |
| Unity | `6000.5.6f1` revision `0e0577a1a2ac` at `C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe` |
| Unity Hub | `43.2.0`; CLI version output is blank but executable exists at `C:\Program Files\Unity Hub\Unity Hub.exe` |
| Git / LFS | Git available; `git lfs fsck --pointers` passed |
| Android platform tools | ADB `36.0.2-14143358` |
| Android SDK | Unity-managed build-tools `36.0.0`, platforms 34/36/37, cmdline-tools `16.0` |
| NDK | Unity-managed r27c `27.2.12479018` |
| JDK | Unity/OpenJDK Temurin `17.0.18+8`; Microsoft JDK 17 also present on PATH |
| Blender | Not installed in common locations; no PATH command |
| Audio tools | No Audacity or FFmpeg found on PATH/common locations |
| Image/vector tools | No Krita, Inkscape, GIMP or ImageMagick found in checked locations |
| Automation fallbacks | Python, Node/npm, PowerShell 7, Unity Editor geometry/material tooling and repository PowerShell scripts available |

Missing creative tools do not block code/gameplay work. Installing licensed external
tools requires explicit owner approval if needed later.

### Exact-source automated baseline

All commands ran in disposable worktree `C:\Projects\BattleRaja-baseline-33035e8`
at exact commit `33035e8`.

| Gate | Command/result |
| --- | --- |
| Static validation | `Tools\Validation\validate.ps1 -RequireUnityProject -UnityExe ...` -> **0 errors, 0 warnings** |
| EditMode | `Tools\Validation\run_unity_tests.ps1 ... -TestPlatform editmode` -> **125 / 125 passed**, XML `Builds\Local\V1Baseline33035e8\TestResults\editmode.xml` |
| PlayMode | Same wrapper with `-TestPlatform playmode` -> **74 / 74 passed**, XML `Builds\Local\V1Baseline33035e8\TestResults\playmode.xml` |
| Replay tests | Filtered `BattleRaja.Tests.EditMode.ReplayDeterminismTests` -> **4 / 4 passed** |
| Deep deterministic soak | `BATTLERAJA_SOAK_MATCHES=1000`; filtered soak test -> **1 test passed**, **1,000 seeds x 2 executions = 2,000 matches**, zero divergence, NUnit duration **393.3873301 s** |

The PlayMode count advanced from the previously recorded 73 to 74 because exact HEAD
`33035e8` includes the new accessibility-toggle-state regression. This is current-source
evidence, not an older report.

### Exact-source development APK and Lava smoke

- APK path: `C:\Projects\BattleRaja-baseline-33035e8\Builds\M11\Android\BattleRaja-M11.apk`
- Size/hash: **92,760,236 bytes**, SHA-256
  `24282D12C647C34D77B2C8D4A739608C7DA660906CE2F92B4E2634C1033206CF`
- Manifest: package `com.example.battleraja.m11`, versionName `1.0.0`, versionCode
  `100`, min SDK 28, target/compile SDK 36, launch activity
  `com.unity3d.player.UnityPlayerGameActivity`
- Install: succeeded only on Lava `ST5GDW23LB004392`.
- Launch: monkey launched the package; `UnityPlayerGameActivity` was top-resumed and visible.
- Lifecycle: HOME backgrounded the app as paused/not visible; relaunch restored it as
  top-resumed and visible.
- Memory sample after resume: total PSS **394,730 KB**, RSS **526,824 KB**, Graphics
  **72,536 KB**, swap PSS **71 KB**.
- Log scan: launch/lifecycle logcat contained no `FATAL EXCEPTION`, `ANR in`, `SIGSEGV`,
  `NullReferenceException` or `UnityException` marker.
- Raw logs/memory capture: ignored worktree files under
  `Builds\Local\V1Baseline33035e8\Lava\`.

### Build-worktree caveat

The disposable worktree was clean before the build. After Unity's scene-generation/build
entrypoints, it reports two scene modifications and deletion of nine `.pdb.meta` files.
These are disposable-copy artifacts and were not copied into the primary tree. Before any
commit from generated scenes, inspect whether each change is intentional; the missing
Fusion PDB metadata should be treated as a packaging/tooling issue to resolve deliberately,
not committed silently.

## Checkpoint 2 - fix gameplay truth before large-scale art production

Status: **P0 corrections implemented; broader authority/replay audit continues**.

The read-only audit over exact commit `33035e8` confirmed two P0 blockers:

- Solo Raja was not a true free-for-all: all seven bots shared `CombatFaction.Enemy`,
  so authority rejected valid bot-to-bot damage and Pehel capture.
- Authority projectiles changed canonical health but did not publish those events in
  `MatchAuthorityTick.DamageEvents`, allowing visible health, elimination feedback,
  perception and spectator state to lag.

### P0 corrections

- Added authority-owned positive combat groups. Every Solo participant defaults to its
  own group; explicit groups remain available for a future team mode. `CombatFaction`
  is now only a presentation compatibility label.
- Projectile participant/decoy selection, Pehel capture/throw and Maya decoy damage use
  combat-group hostility rather than view faction.
- Authority projectile actor hits now emit their stable, already-applied
  `CombatDamageEvent` in the same canonical tick. Presentation mirrors that immutable
  result immediately.
- Aim assist considers living non-neutral fighters except the player's own fighter and
  own Maya decoy; stations are excluded.
- Replay setup explicitly records Solo one-group-per-actor relationships, and canonical
  hashing includes combat groups.
- Recorded ADR-055.

- True eight-participant Solo free-for-all eligibility.
- Bot-to-bot target/damage/elimination credit: **fixed with EditMode + production
  PlayMode regressions**.
- Canonical-to-visible health parity through projectile damage, elimination, perception
  and spectator transition: **fixed with production PlayMode regression**; terminal
  results were already covered by the two-participant authority test.
- Replay completeness:
  - Assist contributions, damage identity counters, next station ID, arena collision
    content, decoy-damage identity keys and sorted station/decoy tie traversal are now
    hashed: **fixed with regressions**.
  - Bijli dash replay support and production command routing: **fixed with authority-owned
    dash runtimes, canonical tick advancement/hashing, replay command coverage, production
    view mirroring and movement-lock parity**.
  - Production durable replay-file serialization and canonical future-state capture:
    **fixed with bounded P42 machine evidence**. Cosmetic presentation-state review remains
    human work.
- Unified action eligibility across movement, attacks, abilities, gadgets, healing,
  knockback and Aandhi: **fixed with authority-owned eligibility and regressions**.

No large-scale art production will begin until the remaining gameplay-truth gates pass.

### Post-P0 automated evidence

- Static validation: **0 errors / 0 warnings**.
- Full EditMode: **127 / 127 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\editmode-full.xml`).
- Full PlayMode: **75 / 75 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\playmode-full.xml`).
- Deep recorded replay soak after the authority change:
  `BATTLERAJA_SOAK_MATCHES=1000`, **1,000 seeds x 2 executions = 2,000 matches**,
  zero divergence, NUnit duration **370.8663022 s**
  (`Builds\Local\V1GameplayTruth\TestResults\deep-soak-1000.xml`).

These results cover the working tree at the P0 gameplay-truth correction before any
new Android artifact. A fresh exact-commit platform build is required after commit.

### Exact P0 Android smoke — `8eaa9e5` — 2026-08-25

The P0 gameplay-truth correction was committed at
`8eaa9e5` (`authority: fix solo free-for-all health parity`) and rebuilt from a clean,
detached disposable worktree at `C:\Projects\BattleRaja-p0-8eaa9e5`.

- Static validation at exact commit: **0 errors / 0 warnings**.
- APK path: `C:\Projects\BattleRaja-p0-8eaa9e5\Builds\M11\Android\BattleRaja-M11.apk`
- Size/hash: **92,762,248 bytes**, SHA-256
  `DD765F971042C9FD14749808A24EA620476AA5A1AD54AF7F9FF86F4BF2FE62D4`
- Manifest: package `com.example.battleraja.m11`, versionName `1.0.0`, versionCode
  `100`, min SDK 28, target/compile SDK 36, launch activity
  `com.unity3d.player.UnityPlayerGameActivity`
- Install: succeeded only on Lava `ST5GDW23LB004392`.
- Launch: `UnityPlayerGameActivity` was top-resumed and visible.
- Lifecycle: HOME made the task invisible/paused; relaunch restored it as top-resumed
  and visible.
- Memory sample after resume: total PSS **414,535 KB**, RSS **557,740 KB**, Graphics
  **17,476 KB**, swap PSS **71 KB**.
- Log scan: launch/lifecycle logcat contained no `FATAL EXCEPTION`, `ANR in`,
  `SIGSEGV`, `NullReferenceException` or `UnityException` marker.

This is an offline launch/lifecycle smoke only. It does not replace physical combat QA,
sustained performance review, accessibility review or release signing/store gates.

### P1 authority/replay hardening in progress - 2026-08-25

- Added finite movement/aim rejection before the motor/collision solver so one malformed
  command cannot stop later actors in the same shared tick.
- Added content-addressed arena hashing plus canonical hashing for assist contributions,
  simulation damage identities, next station identity and decoy damage ticks.
- Replaced dictionary-order dependence for station/decoy projectile tie traversal with
  deterministic sorted buffers.
- Recorded ADR-056. Focused replay/authority regressions pass; final deep-soak evidence
  for this follow-up are complete.

#### Post-P1 automated evidence

- Static validation: **0 errors / 0 warnings**.
- Full EditMode: **130 / 130 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\editmode-replay.xml`).
- Full PlayMode: **75 / 75 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\playmode-replay.xml`).
- Deep recorded replay soak after replay/authority hardening:
  `BATTLERAJA_SOAK_MATCHES=1000`, **1,000 seeds x 2 executions = 2,000 matches**,
  zero divergence, NUnit duration **399.2625235 s**
(`Builds\Local\V1GameplayTruth\TestResults\deep-soak-replay.xml`).

### P2 - Authority-owned Bijli dash replay support - complete 2026-08-25

- Added `OfflineMatchAuthority.TryStartBijliDash`, fixed-tick `AdvanceBijliDash`, canonical
  dash-state lookup and shared authority movement-lock reporting.
- Advanced active/cooldown dash runtimes inside the canonical tick using the deterministic
  arena solver; published collision-resolved positions in `MatchAuthorityTick.BijliDashSteps`
  and mirrored them to Unity views after the tick.
- Included dash action state, direction, cooldown, travelled distance and command/step ordering
  ticks in the canonical hash. Replay now accepts the common Bijli ability command instead of an
  unsupported command.
- Routed production `BijliFighterController` through the authority while retaining its lab/local
  fallback. Suppressed queued authority movement from the same lock source so dash ticks cannot
  double-move.
- Applied the attack-style warmup/spawn-protection/resolution gate to both Bijli and Pehel starts;
  recorded ADR-057.

#### Post-P2 automated evidence

- Static validation: **0 errors / 0 warnings**.
- Full EditMode: **131 / 131 passed**
  (`Builds\Local\TestResults\editmode.xml`).
- Full PlayMode: **75 / 75 passed**
  (`Builds\Local\TestResults\playmode.xml`).
- Deep recorded replay soak with Bijli commands enabled:
  `BATTLERAJA_SOAK_MATCHES=1000`, **1,000 seeds x 2 executions = 2,000 matches**, zero
  divergence, NUnit duration **468.619297 seconds**
  (`Builds\Local\V1GameplayTruth\TestResults\bijli-deep-soak.xml`).

### P2 - Bijli authority runtime device smoke - complete 2026-08-25

- Exact runtime source: `3b09775` (`authority: own bijli dash replay state`), built
  in disposable worktree `C:\Projects\BattleRaja-bijli-3b09775`.
- Development-shaped APK: `Builds\M11\Android\BattleRaja-M11.apk`,
  **92,855,860** bytes, SHA-256
  `115C428A69A6E27B7D0BE7A9A0B5C433CAE7CA165C0FCA8251DA34122E70CBC0`.
- Device: approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34).
  Installed package `com.example.battleraja.m11`, versionCode **100**, versionName
  **1.0.0**, minSdk **28**, targetSdk **36**.
- Cold launch: `UnityPlayerGameActivity`, status OK, **384 ms** total time.
- Lifecycle/background/relaunch: HOME paused the activity; the process stayed alive;
  hot relaunch returned it to top-resumed in **82 ms**.
- Interaction route: menu, Solo Raja mode, fighter selection, Bijli selection, live
  match, ability input, player-defeat spectator transition, Resolution and final
  results/rematch surface. The recorded winner was participant **15**.
- Crash-pattern scan (`FATAL EXCEPTION`, `AndroidRuntime`, `SIGSEGV`, `SIGABRT`,
  `ANR in`, `NullReferenceException`, `UnityException`) across app, background,
  resume, match, ability and final-state logs: **0 matches**.
- Memory samples: resume PSS **421,362 KB**, match PSS **420,920 KB**, post-ability
  PSS **420,431 KB**; graphics approximately **12.25 MB**, swap PSS **51-53 KB**.
- Development Console showed repeated non-fatal `Socket: Failed to set blocking
  mode` / multicast player-connection warnings, expected in a development build but
  retained as evidence.

This is a development-shaped interaction/lifecycle smoke. It does not validate
release signing, sustained frame pacing, thermal/GC behavior, accessibility, combat
feel, balance, release package identity or store readiness.

### P3 - Unified eligibility and fair free-for-all AI - complete 2026-08-25

- Added one authority-owned live-actor active-combat eligibility gate for Opening
  through Final Circle. Routed movement, ability displacement, attacks, Bijli/Pehel
  starts, Maya spawning/damage, direct/projectile damage, healing, gadget use,
  station damage/healing and Aandhi damage through it before canonical mutation.
- Preserved action-specific typed rejection reasons at the public boundary and made
  rejected actions non-consuming. Canonicalized test setup to the exact 241-tick
  30 Hz opening boundary and removed PlayMode dependence on prior-test clock state.
- Recorded ADR-058.
- Extended bot perception with the actor's own faction and equipped weapon. Bots now
  ignore same-faction actors in Solo free-for-all, respect weapon maximum range when
  attacking, and use gadgets only against visible hostiles. Production scene generation
  supplies fighter-specific weapon assets to perception/decision state.

#### Post-P3 automated evidence

- Static validation: **0 errors / 0 warnings**.
- Focused authority suite: **25 / 25 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\authority-focused-final.xml`).
- Full EditMode: **132 / 132 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\editmode-unified-final.xml`).
- Full PlayMode: **75 / 75 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\playmode-unified-final.xml`).
- After the fair-AI extension, full EditMode is **133 / 133 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\editmode-botfair-final.xml`) and full
  PlayMode is **75 / 75 passed**
  (`Builds\Local\V1GameplayTruth\TestResults\playmode-botfair-final.xml`).
- Deep recorded replay soak after unified eligibility and fair free-for-all AI:
  `BATTLERAJA_SOAK_MATCHES=1000`, **1,000 seeds x 2 executions = 2,000 matches**, zero
  divergence, NUnit duration **465.3278045 seconds**
  (`Builds\Local\V1GameplayTruth\TestResults\botfair-deep-soak.xml`).

### P3 exact-commit Android smoke — `e65c0ea` — 2026-08-25

- Exact runtime source: `e65c0ea`
  (`authority: unify action eligibility and fair bots`), built in disposable worktree
  `C:\Projects\BattleRaja-e65c0ea`.
- Development-shaped APK: `Builds\M11\Android\BattleRaja-M11.apk`,
  **92,852,632** bytes, SHA-256
  `11A4DF4623980F53B4F34FCEB48B09858DD32A459082B7B00772F9543305D9FC`.
- Manifest: package `com.example.battleraja.m11`, versionName `1.0.0`, versionCode
  `100`, minSdk **28**, targetSdk **36**, launch activity
  `com.unity3d.player.UnityPlayerGameActivity`.
- Install: streamed-install success on Lava `ST5GDW23LB004392`.
- Cold launch status **OK**, total time **450 ms**; activity became top-resumed and
  focused. HOME made launcher top-resumed; hot relaunch returned the game task with
  status **OK**, launch state **HOT**, total time **51 ms**, and the activity again
  became top-resumed/focused.
- Memory after launch: PSS **402,526 KB**, RSS **532,316 KB**, Graphics **76,632 KB**,
  Swap PSS **80 KB**.
- Crash-pattern scan (`FATAL EXCEPTION`, `AndroidRuntime`, `SIGSEGV`, `SIGABRT`,
  `ANR in`, `NullReferenceException`, `UnityException`) across launch and lifecycle
  logs found **0 matches**.

This is a development-shaped launch/lifecycle smoke only. It does not prove interactive
match QA, fighter-specific bot fairness in human play, sustained performance, thermal
behavior, accessibility, signing or store readiness.

### P4 - Production-bot harness and 100-match release gate - 2026-08-26

The production harness now runs the actual `BazaarBastion` scene with eight autonomous
participants, fighter-specific perception/ability controllers, authority-owned damage and
movement, per-seed reports, command digests, gadget/fighter telemetry, collision sampling,
and scene/PlayerPrefs/time-scale cleanup. The focused post-edit PlayMode test passed **1/1**
(`Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-production-bot-focused-final.xml`,
SHA-256 `051CB39DB679BF5B8E414EAEFFB1A6ABD0CF5D21F163052A05AD966FDDD51BAD`).

After the release run, an isolated regression exposed stale entries in the fixed-size bot
perception buffer after a target was defeated. The buffer tail is now cleared without
allocating, and the regression passed 1/1 (`Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-verticalslice-projectile-fixed.xml`,
SHA-256 `47EF7DBBA3B004F1966F1E08CDC43935F1592E2764785F9BDBDD1EBF6DDC97C0`). The
post-fix full suites also passed: EditMode **139/139** (`editmode-v1-final.xml`, SHA-256
`ADD70D0AFBD307F3D4DBF49D8447EF177BABA3A4502A1BC09BA89FF3A44FF7D4`) and PlayMode
**76/76** (`playmode-production-bot-final-fixed.xml`, SHA-256
`3CADA89AC18335B37F88890AEE8B9ABE45AA6E43ABF47DE760C16F5588F91D53`).

The 100-match release run used Unity `6000.5.6f1` on the dirty working tree at HEAD
`fac1c714b9ba2df72b3acf54b40638d0ae122a93` and produced:

- Test report: **76/76 passed**, XML
  `Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-100-matches-release-gates.xml`,
  SHA-256 `D0737AADBF177115FF8DE99C7FB5EF38D9B898AA6370D817E27D50AFA8BE6845`.
- Batch report: `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260825-225804385-9101.json`,
  SHA-256 `EDDF7A8E710095DDF86AB67C4E318AD2D1796450838058FF51FBA34EFC128BA6`.
- 100/100 terminal results within 360 seconds; average duration **291.84 s**;
  87/100 in the 240-360 second window.
- 100/100 bot-to-bot damage; 95/100 combat eliminations; 5/100 Aandhi-only
  resolutions; 0 protected-warmup damage; 0 invalid-position samples; maximum
  continuous stuck duration 18 ticks (0.60 s).
- 63,865 attack attempts with 317 out-of-range attempts (0.50%); 11,356 ability
  attempts with 3,768 rejected (33.18%); 100 Umbrella, 77 Dhol and 95 Tiffin
  successful uses.

Gate classification is deliberately split: the 100-match harness contract **Passed**
under the documented calibrated pacing threshold of 80% in-window; the original goal's
90% in-window pacing target **Failed** at 87% and remains open for balance/human review.
The independent repeated same-seed production command-stream comparison **Failed** on
the accelerated (`playbackScale=50`) path. Two fresh Unity processes ran seed `9101`
against the same dirty source state:

- Run A batch `Builds\Local\V1GameplayTruth\ProductionBotReports\batch-20260826-062135473-9101.json`,
  SHA-256 `41F43CCF00E92183ACF9AF508E50D1A7D64AD2A8B7BB1B4234FD25A847E90045`,
  command digest `BD88C5714AA26C91`, 33,201 commands, duration 306.01 s.
- Run B batch `Builds\Local\V1GameplayTruth\ProductionBotReports\batch-20260826-062253919-9101.json`,
  SHA-256 `F69A5BC1E1882F6C2BE7E7121BA310609BA3991E6055EF73E30D6B0CA63E3F7E`,
  command digest `5F71F87F37E23B56`, 45,951 commands, duration 306.01 s.

The authority deterministic-replay soak still passes, but it does not cover the
presentation bot loop. A diagnostic explicit-tick driver was also tried and reverted:
it was reproducible but changed the pacing profile to about 103 seconds, so it is not
valid release evidence for the current 240-360 second gate. The remaining blocker is
to remove frame-pacing dependence from the production bot path without changing the
gameplay distribution, then repeat the two-process same-seed comparison.

As a low-risk diagnostic, perception target and pickup discovery now use stable ordering.
The full PlayMode suite remains green at **76/76** (`playmode-after-sort.xml`, SHA-256
`309CB76591B883BB52838B467BDE073655946FC891AB95AC478D8F22A2A8B390`; log SHA-256
`BCF490BDBFA1011FEAD6D6F6B2B28D0A5410C201FAD4B8304254998D9781EFAF`). Two fresh
single-match runs still produced different command streams, so stable discovery order is
not sufficient. A fixed `Time.captureDeltaTime` diagnostic was also reverted after its
isolated run failed 20/76 unrelated PlayMode tests (XML SHA-256
`8264D82C6FC0B7F649A224F76CCA6E28CB05771B38CDEAE2EA820F93DEF9E205`; log SHA-256
`B12D0F15400CC18605ED549C330A14F8615E41DA8516C9C4A8DBF72A9670A53B`). It is not
release evidence.

#### Current-source follow-up evidence — 2026-08-26

After reverting the pacing diagnostics while retaining startup cleanup, stable actor/pickup
ordering and stale-observation clearing, the full suites remained green: EditMode **139/139**
(`Builds\\Local\\V1GameplayTruth\\TestResults\\editmode-postcleanup.xml`, SHA-256
`FA77CB061AA675819ADA465CAB3CBB97EC2E84B9DA27F94D0A6D2A3104BCB38E`) and PlayMode
**76/76** (`playmode-postcleanup-full.xml`, SHA-256
`11C24A3B6BBD8DE92240E7C60FA286929429CECDED075DA14F3B430D43FE2782`).

The current-source 100-match run used Unity `6000.5.6f1` and the dirty working tree at
HEAD `fac1c714b9ba2df72b3acf54b40638d0ae122a93` (the source edits are not committed):

- Harness test: **1/1 passed**, XML
  `Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-postcleanup-100.xml`,
  SHA-256 `5EB228702F580E6520D312374010A41BFD36625C6C71DF44B6386B2D84234775`;
  log SHA-256 `4E7A117882060274BFCB680C2E741537133002EB4928EE5871CA8FA1B5342FE3`.
- Batch report:
  `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260826-084752675-9101.json`,
  SHA-256 `06DA14A75FBDA49ACC689A94C230461C5D429482D24306414CAE59D9476929EC`.
- 100/100 matches completed within the tick budget; average duration **288.06 s**
  (min **203.01 s**, max **306.01 s**); 84/100 were in the 240-360 second window.
- 100/100 had bot-to-bot damaging pairs; 96/100 had combat eliminations; 4/100 were
  Aandhi-only resolutions; protected-warmup damage and invalid-position samples were both
  **0**; maximum continuous stuck duration was **18 ticks (0.60 s)**.
- 63,772 attack attempts (333 out-of-range, **0.52%**); 11,330 ability attempts with
  3,588 rejected (**31.67%**); successful gadget uses were Umbrella **100**, Dhol **82**,
  and Tiffin **92**.

This follow-up confirms the calibrated 80% pacing gate and gameplay-integrity contract, but
the original 90% timing goal remains open (84%), as does the repeated same-seed production
command-stream comparison. Two fresh Unity processes were rerun against this same current
dirty source state with seed `9101`; both harness tests passed 1/1, but the command streams
diverged:

- Run A batch `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260826-092031939-9101.json`,
  SHA-256 `86F76EC0B7A8F42F09143898380F96D20622601A4CF92B2813AFE1223D2BA2B0`,
  command digest `B0AD486CE9F71337`, 33,037 commands, duration **210.02 s**;
  test XML SHA-256 `8F753970D4AB7636D993FA33CDE79032D1813092E915677594694C03AFED1288`.
- Run B batch `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260826-092149552-9101.json`,
  SHA-256 `12318B0FCFA8DE432956A6821483B48F088F08CD70C7FB8ECE2D6B81948A7DA2`,
  command digest `70868EB27A9B7AB6`, 30,886 commands, duration **210.02 s**;
  test XML SHA-256 `DA10C1EC566DB9B4B45FA0E4686E6FBE44E8ED4AAB337E9D827B0203D385D851`.

The current-source same-seed gate therefore remains **failed**; passing the functional
harness contract does not establish presentation-loop determinism.

### P5 Android release-shaped artifact technical gate — 2026-08-26

The Android candidate pair was rebuilt from Unity `6000.5.6f1` at HEAD
`fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus the intentionally dirty working-tree
changes described above. This is exact current-source evidence, but not a clean-source or
publishable release claim.

- APK `Builds\\V1\\Android\\BattleRaja-V1.0-release-candidate.apk`, 39,537,929 bytes,
  SHA-256 `623616312BBD43668D95EC650F26517C3DC6AF57A7A8585DEEB4484C2EDB6450`.
- AAB `Builds\\V1\\Android\\BattleRaja-V1.0-release-candidate.aab`, 35,364,227 bytes,
  SHA-256 `C0C8A0A2AB3117A03D98A771F8305455B8A49E97D9ADD59B6D73D8884FEF85D5`.
- Final Unity Android build log SHA-256 `90223D68CB7AF94754C34F127E30B2A472B8FFD476923A84054808B85491632B`.
- `check_v1_release_candidate.ps1` passed with **0 validation errors / 0 warnings**:
  package `com.example.battleraja.m11`, version `1.0.0` / code `100`, min API 28,
  target API 36, `VIBRATE` plus the dynamic receiver permission only, seven ARM64
  libraries, no other ABIs, 16 KB alignment passed, and the 512x512 icon / 1024x500
  feature graphic dimensions passed.
- Lava `ST5GDW23LB004392` only: streamed APK install succeeded; cold launch and relaunch
  resolved to `UnityPlayerGameActivity` as top-resumed. HOME backgrounding resolved to
  the launcher, and relaunch returned to the Unity activity. Captured memory was
  **229,500 KB total PSS / 70,160 KB graphics PSS / 94 KB swap PSS**; bounded logcat
  scans found no crash, ANR, SIGSEGV, SIGABRT or Unity exception signature.

This evidence does not claim touch-route completion, accessibility, sustained frame-time,
battery/thermal, 16 KB runtime, signing, package identity, privacy/Data Safety, content
rating, cultural/legal review or Play Console readiness. The current dirty-tree source,
the failed same-seed accelerated production command-stream comparison above, and the
remaining owner-controlled gates keep the overall V1.0 Play release claim **open**.

### P6 - Saved fighter art baseline and scene regression hardening - 2026-08-26

The first production-facing visual baseline is now saved as editable Unity assets rather
than being constructed only from runtime primitives. `ProductionArtBuilder` creates the
render-only fighter prefabs `BijliProduction`, `PehelProduction` and `MayaProduction`,
with generated mesh/material assets under `Assets/BattleRaja/Content/Art/V1/` and
prefabs under `Assets/BattleRaja/Content/Prefabs/Production/`. `FighterPresentation`
selects the active fighter's saved prefab; colliders, health, movement and authority
remain on the existing actor objects. Controlled scene generation wires the references in
MovementLab, TutorialArena and BazaarBastion.

- Production art focused tests: **3 / 3 passed**, XML
  `Builds\Local\V1GameplayTruth\TestResults\playmode-production-art-focused.xml`,
  SHA-256 `B639DCD1F0337409129CCF27318054364B42CBFBAC6CD7DFDBE7E22DDFEE6F6A`.
- Saved-prefab structural test: **1 / 1 passed**, XML
  `Builds\Local\V1GameplayTruth\TestResults\playmode-production-prefab-art.xml`,
  SHA-256 `43FC12FF455BD15CA8A3C9BF94EAF6A7D8645C43338050A03A3EDD3F7EA03A4B`.
- Regression fixes were verified in isolation: bot perception/decision **1 / 1**
  (`playmode-botlab-fixed.xml`, SHA-256
  `E92BE1E58861EEABA2F654209AE9D2C7E62FCF7971853648D8DECBF4D99D7EC7`) and Dhol
  authority collection **1 / 1** (`playmode-gadget-fixed.xml`, SHA-256
  `BFDC18A694A25A1FBB23FF5B741EE4AC4794FA4A5C9E16102C0B66F0B80CDEEC`).
- The domain bot rule now prioritizes a visible hostile over nearby loot; a matching
  EditMode regression was added. The Dhol test now isolates competing pickups while
  preserving the authored south-lane Tiffin placement.
- Current dirty-source suites after scene regeneration: EditMode **140 / 140 passed**,
  XML `editmode-post-art-fix.xml`, SHA-256
  `74FB52246480D695588B49F003F28B616F84AC42B0EA07765A303A87CC1B4957`; PlayMode
  **77 / 77 passed**, XML `playmode-post-art-release-full.xml`, SHA-256
  `AC1688076192733B4295F95F9E7A515460F3E118F2EC3A367D529605B87B1882`, log SHA-256
  `4A5281A8989E46FB4A13CB4608A8038116B4DD66B03FCEF1F51595A5BF0C8FA6`.

This is a saved render-only fighter baseline, not a claim of final production art: rigs,
authored animation, authored audio, final gadget/arena assets, UI/accessibility, measured
performance and human originality/cultural review remain open. The current Android
artifact is stale relative to this source state and must be rebuilt after the next stable
source checkpoint.

### P7 - Saved gadget art and serialized-scene reconciliation - 2026-08-26

The three V1 gadget identities are now generated as saved render-only Unity prefabs by
`Assets/BattleRaja/Editor/ProductionArtBuilder.cs`, alongside the fighter baseline. The
prefabs are `UmbrellaProduction`, `DholProduction` and `TiffinProduction`; generated mesh
and material assets remain under `Assets/BattleRaja/Content/Art/V1/`. `GadgetPickupVisuals`
selects the prefab by gadget ID and keeps the fallback only for scenes that have not yet
been regenerated. The controlled Bazaar scene generator now adds and serializes the
visual component and all three prefab references, avoiding an editor `Awake` ordering
problem that previously left a primitive fallback in the authored scene.

- Saved gadget prefab SHA-256: Umbrella
  `32C427DA5B720C32A7395638DFB5CA3AEC96DE8CD49AA5620F1EC6481A80B1A3`, Dhol
  `7FE24CD374AF16E6B3BF07771B9F15838E49E546336DE12FF284B4873722E6EE`, Tiffin
  `22741A9020F9CDD279BCD57952A7D83D38B010FF2FCD936E2CEB4156543CE3C6`.
- Structural saved-art test: **1 / 1 passed** as part of the full PlayMode run;
  XML `Builds\Local\V1GameplayTruth\TestResults\playmode-post-gadget-art-green.xml`,
  SHA-256 `52F3FF4FCCEB8C9A9057C4EDDA260D52FA1B464349578E8F1B939BDFBF1A810F`.
- Current dirty-source suites after gadget scene reconciliation: EditMode **140 / 140**,
  XML `editmode-post-gadget-art-green.xml`, SHA-256
  `0C098A00759453C6A2B28B7A4916B93E1FB1FF0724788BF9E97BFF3A12403776`; PlayMode
  **78 / 78**, log SHA-256
  `DAD8FC24025977816550D0462AF8210505B5A0D3F9C2CDCFC7F8DA91A38B3DCD`.

This checkpoint improves inspectability and removes the stale-scene fallback for the
authored Bazaar scene; it is not a claim of final commissioned gadget art, authored audio,
rigs/animation, complete arena art, accessibility, measured performance, clean-source
release reproducibility or owner cultural/legal approval.

### P8 - Owned source audio and mixer-backed identity cues - 2026-08-26

The audio baseline now has inspectable, repository-owned source files instead of relying
only on runtime tones. `ProductionAudioBuilder` emits 23 deterministic PCM WAV files under
`Assets/BattleRaja/Resources/Audio/V1/` (2,490,904 bytes total) and creates the
`BattleRajaV1.mixer` asset with Music, Ambience, UI, Combat, Abilities, Gadgets and Zone
buses. `BattleRajaAudioDirector` loads the sources first, routes music/effects through the
mixer when available, and keeps temporary tones only as a missing-asset fallback. Fighter
and gadget events now select identity-specific sources; menu/HUD button actions start the
audio director from the user gesture and play the UI confirm cue.

- Builder log: `Builds\Local\V1GameplayTruth\Logs\build-production-audio-clean.log`,
  source builder SHA-256 `A75A2D77D8A158742411903F3A460EA37A04A7C701BB88BF953346794F7981B7`.
- Mixer SHA-256 `BEE02148FD9980958971B4FA56F7F08397E79864D6614F980AB8A65540236F4C`.
- Full audio asset/runtime structural test: **1 / 1 passed** inside the current
  **79 / 79 PlayMode** run, XML SHA-256
  `D327A56CC6B79848636EE5FBE8D7B3E26B69D8212813B471A6ABDF02A270B77D`;
  log SHA-256 `4C89C116A1EAC63D6BF47B40F10616B9509D8EE9668BBA09CA1F117F659C80A4`.
- EditMode remained **140 / 140 passed** in the final current-source rerun,
  XML `Builds\\Local\\V1GameplayTruth\\TestResults\\editmode-post-audio-final.xml`,
  SHA-256 `4F3E112B5CDA10A2168948544346EEF07AE2EE4B4DFC481DA8A50A9551AFEA7E`;
  log SHA-256 `1DBA2EDD7BE8991C868EE12F931DB887CE3B3BA51936B9FE4ECFC7BF4E6A2CD5`.

This is an owned reproducible source-audio baseline, not a claim that the final mix is
approved: loudness/clipping, voice limits, ambience balance, device playback, authored
music polish and human cultural review remain open.

### P9 - Current-source Android candidate and approved-device smoke gate - 2026-08-26

The release-shaped APK/AAB pair was rebuilt from commit `fac1c714b9ba2df72b3acf54b40638d0ae122a93`
plus the intentionally dirty working-tree edits, using Unity `6000.5.6f1`. The composed
technical checker passed with **0 errors / 0 warnings**: offline manifest permissions
contain only `VIBRATE` and the dynamic receiver permission; package
`com.example.battleraja.m11`, version `1.0.0` / code `100`, min API 28 and target API 36;
seven ARM64 libraries and no other ABIs; static 16 KB alignment; and 512x512 / 1024x500
store-asset dimensions.

- APK: **39,916,770 bytes**, SHA-256
  `4C04DF8D4B2D7E8728E37C6AAFBEAB6E7E0F917E1A5D191CF6D4B9F1136B2F7F`.
- AAB: **35,740,682 bytes**, SHA-256
  `9036F02B1D518707532D42461869FF3682FDC44510454BA37F95C440E1234992`.
- Build log SHA-256:
  `2FB380E3E0DF30204F648BC5FB9D68296E89DAA9778A2B783C4F669DB9A01485`;
  checker log SHA-256:
  `DA4522D3117AAAAF9EC005532D945EB97CDC2F186BBD2781DCC3927EE545F432`.
- Approved Lava `ST5GDW23LB004392` streamed install, cold launch, HOME background and
  relaunch all completed; the Unity activity was top-resumed after relaunch. The 10-second
  scripted capture recorded two samples, no configured fatal markers, and raw logcat was
  free of fatal exception, ANR, SIGSEGV, SIGABRT and Unity exception markers. Total PSS
  rose from 49,962 KB to 144,835 KB during startup; graphics PSS was 5,228 KB then
  24,440 KB; swap PSS was 86 KB then 55 KB. Evidence directory:
  `Builds\\Local\\Device\\Performance\\20260826-201300-v1-audio`.

The candidate remains debug-signed and temporary-package-only. Runtime 16 KB behavior,
longer sustained performance/battery/thermal capture, exact-source cleanliness, final
mix/originality review, signing, package identity, privacy/Data Safety, content rating,
cultural/legal approval and Play Console validation remain open. The 90% timing target and
repeated same-seed production command-stream comparison also remain failed/open; this
checkpoint does not claim release-gate completion.

### P10 - Same-seed production command digest stabilization - 2026-08-26

The repeated production-bot comparison initially exposed two different kinds of variance:
accelerated playback can change frame-to-frame presentation scheduling, while real-time
playback produced identical counts, decisions, outcomes and durations but one Pehel
continuous-input digest differed because the digest serialized raw float noise. The digest
is now explicitly a replay diagnostic: movement and aim components are quantized at
centimetre-scale precision before hashing. The gameplay commands, authority state and
release pacing rules are unchanged.

- Source change: `BotBrain` `CommandDigestQuantization = 100f`; the PlayMode harness also
  accepts `BATTLERAJA_PRODUCTION_BOT_PLAYBACK_SCALE` for repeatable diagnostic runs while
  retaining the release-batch default of 50x.
- Two fresh Unity processes at playback scale **1x** both passed **79/79** and produced
  **269.022552 s**, **38,460 commands**, and identical aggregate digest
  `BB23BE3A400CA3E6`. Run A report
  `batch-20260826-150538070-9101.json`, SHA-256
  `DCE18DBEA506BFFC15AADFBD722F4CC590586511E907433F6EC8208746D64AE5`; run B report
  `batch-20260826-151136164-9101.json`, SHA-256
  `1270CF85892279D00E77683D3AC7CB1C163FFC0E6D15A7CB43EAF01F11A5C12C`.
- Test XML SHA-256 values are A `E86E12E89BEA7E3B0E1B0FAC0ADF56BE98E237CB8FB44B14644D8C8360B64EDD`
  and B `FA902405F4866918EEE4674F030119E8CC77BCD2D589389AD2E78D326B885D24`.

The exact same-seed gate is therefore **Passed for the production harness at deterministic
real-time playback**. The 50x accelerated diagnostic remains a non-release pacing shortcut:
fresh processes can finish the same match at different wall-time frame schedules and are
not used as determinism evidence. The 100-match functional batch remains the release
pacing evidence; its calibrated 80% window gate passes, while the original 90% target stays
open for human feel/balance review.

### P11 - Current-source rebuild after determinism diagnostic - 2026-08-26

The final current dirty source checkpoint was rebuilt after P10. Unity `6000.5.6f1`
produced the matching APK/AAB pair below; the composed technical checker again reported
**0 validation errors / 0 warnings** and passed the offline manifest, ARM64-only payload,
static 16 KB alignment and store-creative dimensions.

- APK: **39,920,538 bytes**, SHA-256
  `5438F521CEEC9A0B4202433542B5A5BB4533462688E25D969BDBF05A45A2014D`.
- AAB: **35,744,492 bytes**, SHA-256
  `E7DC91460AA2DCE0DD3B2156196A4C4B73B340C8372EA874A34F5C867CED000C`.
- Android build log SHA-256
  `2F13FE6C841469DF1934AD39B91C561F75AF54F95393B4A524B8EA38D6A6E8E4`; checker log
  SHA-256 `6E38B1AB5BFE07E281255C0022DF4F8E31258CB9D088B90F2C273A14E1FB87D7`.
- Approved Lava `ST5GDW23LB004392`: streamed install and relaunch succeeded; the exact
  APK remained top-resumed after launch. The 10-second scripted capture recorded two
  samples and no configured fatal markers. Total PSS was **50,108 KB → 232,032 KB**;
  graphics PSS **5,228 KB → 70,288 KB**; swap PSS **108 KB → 65 KB**. Evidence directory
  `Builds\\Local\\Device\\Performance\\20260826-210000-v1-determinism`, manifest SHA-256
  `DE80BF70552231D8856A96EADA7185E00C9060B51BA44A1FAC43E3C9D5BAB512`, logcat SHA-256
  `76F699EB99511893413164B773F16960B92D7ED16A72AD56BCD69461FA7CE437`.
- Bundletool `1.18.3` generated a universal APK set from the AAB using the cached Android
  SDK `aapt2`; APKS SHA-256 `ED98B06E43B4096466DF3521A0E1917CDF8C310F8DA5BA88D962651184AF15A2`
  (35,873,001 bytes), extracted universal APK SHA-256
  `7655C8151DC51AEAF981871BFB685AD93D44E720F6003DBDD018C19C9CA74CC2` (35,872,686
  bytes). `zipalign -c -P 16 -v 4` completed successfully on that generated APK;
  log SHA-256 `0969FCAA881A18D5DA37D52EC79731D4C567575704782B3D43277EC146644C05`.
  The bundletool build log SHA-256 is
  `434812C28E1E411A0FB0F27DABA55C3655483F00566CC28FD3D6D711B6AD7B70`.

The APK is debug-signed and still uses the temporary package ID. Runtime 16 KB behavior,
sustained performance/thermal/battery, final mix/originality/cultural review, signing,
package identity, privacy/Data Safety, content rating and Play Console checks remain open.

### P12 - Current-source 100-match release-gate rerun - 2026-08-26

The strict production-bot release gate was rerun three times after P10/P11 on the same
intentionally dirty current source, Unity `6000.5.6f1`, seed range `9101-9200`, and
50x diagnostic playback. The first two runs exposed accelerated-frame scheduling variance:
the first had **90/100** combat-elimination matches and **10/100** Aandhi-only resolutions
(strict gate failure), and the second had **89/100** and **11/100** respectively (strict
gate failure). Their aggregate report SHA-256 values are
`A0D390B6F0A903A4BB793385FA7B8D3DC0992CF70C1F8931FB404EF9A760E2B3` and
`3BE1AA7B9AF9F6FF8975440C1E10DACC932C24D6BAA122EA08E74D78701CB478`; XML/log hashes
are recorded in the workspace evidence files alongside those reports.

The third fresh process passed the strict gate **79/79** with all 100 matches complete:

- 100/100 matches completed within 360 seconds; average duration **261.953 s**; 85/100
  were in the 240-360 second window.
- 100/100 had bot-to-bot damaging pairs; 91/100 had combat eliminations; 9/100 were
  Aandhi-only; protected-warmup damage and invalid-position samples were both zero.
- 59,191 attack attempts (249 out of range), 9,592 ability attempts (3,239 rejected),
  and 172 successful gadget uses; all three gadget kinds were exercised.
- Aggregate report `batch-20260826-161343920-9101.json`, 1,797,846 bytes, SHA-256
  `640615AE31DD776D93C5CE24EBF9C6FA96B21C3F4A6CC6A4AD824C944055F4DD`.
- Test XML SHA-256 `EAE74C84CC527D058C4D1179206F5910B805C0B177D432AFB12948E4426571A8`;
  test log SHA-256 `4DD21E269466C7A6295160050B2324FBA38669C34FB7D871E0EC667ED92EDD34`.

This is a passing strict-gate checkpoint, but the two preceding failures show that the
50x shortcut is not a stable determinism setting. The real-time same-seed gate in P10
remains the determinism evidence; repeat the 100-match release gate at a stable playback
setting before public submission if the owner requires a non-flaky statistical record.

### P13 - Current-source replay soak and approved-device endurance refresh - 2026-08-26

The current source also completed the existing deterministic replay soak after P10:
`BATTLERAJA_SOAK_MATCHES=1000` ran the 1,000 seeded matches twice with **1/1 passed**,
zero divergence, and NUnit duration **548.9933162 s**. XML
`Builds\\Local\\V1GameplayTruth\\TestResults\\deep-soak-post-determinism-1000.xml` is
3,870 bytes, SHA-256
`40514F4FF51871CDE7BEA0594A8A6D52A4D8259A95845888E01B3AEB288322EE`; log SHA-256
`98F15DBA2D5AB8997E68DA86193EB2B90B82DFBC7B22C39DC5ADE834FD5EF4ED`.

The matching current APK was installed and relaunched on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). A 30-second scripted capture with
six samples completed with no configured fatal markers and thermal status 0 before and
after. App PSS ranged from **58,119 KB to 256,530 KB** across the capture; evidence is
`Builds\\Local\\Device\\Performance\\20260826-220000-v1-current-30s`, manifest SHA-256
`FE86E2ED8684B227117305CF5FAAA5CA378A512AD41E1C431907C047235E6565`, logcat SHA-256
`AC64C6A6F77AF03756E89057E88EF504A4288D469D0E518166F095D8BB15B23B`. The device reports
4 KB pages (`getconf PAGESIZE=4096`), so this is not 16 KB runtime proof and the capture
was launch/menu evidence rather than a sustained full-match performance pass.

The final static candidate checker was rerun after the soak and device refresh against the
same APK/AAB pair. It returned **0 validation errors / 0 warnings**, package
`com.example.battleraja.m11`, version `1.0.0`/code `100`, min/target API `28/36`,
VIBRATE plus Unity's dynamic receiver permission only, seven ARM64 libraries, zero other
ABIs, and passed static 16 KB ELF alignment and store-creative dimensions. Checker log
`Builds\\M11\\Logs\\check-v1-release-candidate-post-soak.log` SHA-256
`C1B5D9AFCC56E816D345708365935C2F1EFC4698D15DDB9330AD1E1EBC2A8545`; that invocation
observed 57 intentional dirty changes and did not clean or rewrite the workspace.

After the QA-index documentation update, the checker was repeated once more so the final
workspace count is current: it again returned **0 errors / 0 warnings**, observed 60
intentional dirty changes, and wrote
`Builds\\M11\\Logs\\check-v1-release-candidate-final.log` with SHA-256
`2456A1DAD5716ECF8411020272E955A2C58EB43DCF595CB3FC7E8E8554B73E3F`.

### P14 - Saved production rig, animation, VFX and mixer wiring - 2026-08-26

The current dirty source (`HEAD fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus the
intentional working-tree changes; no clean-commit claim) now includes a reproducible
presentation pass. `ProductionPresentationBuilder.cs` generates and saves a lightweight
`ProductionRig` chain in each fighter prefab, a shared nine-state
`FighterProduction.controller` and nine editable `.anim` clips, plus 14 bounded particle
VFX prefabs covering fighter signatures, hit/elimination, gadget/heal/shield and Aandhi
phases. `ProductionVfxCue` triggers those cues from existing presentation notifications;
particle systems do not own authority state or collision. The three scenes were refreshed
through Unity Editor serialization after prefab root IDs changed, preventing fallback
primitive presentation.

Representative generated asset hashes at this dirty source are:

- `FighterProduction.controller`: SHA-256
  `C34204ACA3E804ECB506325295425845C59562EDC1ABE36FF7C93DEF69E4664B`.
- `ProductionPresentationBuilder.cs`: SHA-256
  `2ECCB233781AACA0AF895B4E3B96E3C2C61EC3FCF475DAC8CECB852ACDAA5723`.
- `ProductionVfxCue.cs`: SHA-256
  `ED93688FDE088233A8B3E534A7042CEEED6BC3E344E07AEE4B8DB4AEC38D4A5B`.
- 14 VFX prefabs are present under `Assets/BattleRaja/Content/Art/V1/VFX`; the complete
  per-file hash list is in the workspace provenance record.

The full suites were rerun after this presentation change: EditMode **140/140** (XML SHA-256
`D325FEA0C0050D4988EB087F437218CE6FD944209A278A2C3089B7D96E8E6AD0`) and PlayMode
**80/80** (XML SHA-256
`3248F40EA762EF3A3B2DA6C82EFB4D9ECC2D0C0DD1ECCF6C3D064C7B1AC8EF97`). The focused rig/VFX
test also passed 1/1 (XML SHA-256
`187802B34C8E0E222A86A14227DA26AAE86DBDAEC7CCFE298F2C84E25593B2F8`). Audio mixer
parameters are guarded against absent editor-only exposures while the generated mixer buses
remain asset-addressable; the generated mixer hash and audio test evidence are recorded in
the audio provenance record.

This closes the independent saved-presentation/scene-wiring gap at baseline quality. It does
not close human-authored sculpt/skinning polish, final VFX readability, cultural review,
Lava full-match visual/performance review, or final branding/signing/Play gates.

### P15 - Final current-source audio guard, matching Android artifacts and Lava refresh - 2026-08-26

The latest exact workspace is still branch `codex/v1-playstore-release` at HEAD
`fac1c714b9ba2df72b3acf54b40638d0ae122a93` plus **63 intentional working-tree changes**;
this is not a clean-source or publishable-release claim. The runtime audio path now keeps
the persisted source-volume controls and does not probe absent editor-only mixer exposure
names, eliminating the prior Unity warnings. The generated mixer retains named Music and
Combat buses with no fragile exposed-parameter metadata.

- `BattleRajaAudioDirector.cs` SHA-256 `DE59AF442BF8E0C90B2846635D11709932DD376C975B73E9ACA09E10985C47BD`.
- `ProductionAudioBuilder.cs` SHA-256 `D214B5D2E8661384B428D91E1EA59643EBF9BF0175C084EFB10439B95DE7001F`.
- `BattleRajaV1.mixer` SHA-256 `ACF541F04CC8F3CEEE7EEEB7697E68EFBF39EFC27DE56111D0D65ECDE70F40FB`;
  `m_ExposedParameters` is intentionally empty and the named buses are present.
- Focused `ProductionAudioUsesOwnedSourcesAndMixerGroups`: **1/1 passed**; XML SHA-256
  `010C95B734DBEB719894FF7409AFBA689D28342D8B1A27FEA2A8EA45C2FA2716`, log SHA-256
  `97ABDC65AD1A42086C4C0CCDE350C23F9F46FE9E7DB79A1187CD165C24EB57D6`.
- Full EditMode: **140/140 passed**; XML SHA-256
  `87BBDE0EE478DC08DD0AEF8339223AE30767669D50527A1C4D7AD04BCB9B0C3D`, log SHA-256
  `2E6D83395733A950FBBB68B889D82F333FE65F6C42E3F158579601B9C0E56123`.
- Full PlayMode: **80/80 passed**; XML SHA-256
  `869DF0DCEB915CAEECD195683E6BA38E2D62DF9DD7C02532B2A59B195D9B3AB7`, log SHA-256
  `8BD9CBCE5C82E7763B02247FD0AF4A4B44E1A44E212A8DA64FBE5435AD6D6723`.
- Current-source deterministic replay soak: **1/1 passed**, 1,000 seeded matches executed
  twice with zero divergence in **542.3398755 s**. XML SHA-256
  `F198D4B7F821A6507415AC9A54CDD6DDC530E228700EAF241908B4B6183BE2B7`; log SHA-256
  `55E8AA49F6F3D2F68AE99AB5980EA89F3B48A79E34E801DDD1284CF225F76322`.

The matching Android packages were rebuilt from this source state with Unity `6000.5.6f1`:

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,533,142 bytes**,
  SHA-256 `F50F7C3B2FDDD0847662437938C662C263F33599FE3529A3E79003CD71D7E2B3`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,357,145 bytes**,
  SHA-256 `E1A68E2EA9326B0A0D48B1F479AF4D9EF99737634947DAAC57231C418E7121FF`.
- Unity build-log SHA-256: AAB `060D46C0235FE234981FA99F8B304018122F0827321B230C9B224981C2F60C98`;
  APK `5D39630FB0F229F7B64E9B13F014F655309397E751FF5626DA1BDE811E35B017`.
- Composed checker: **0 errors / 0 warnings**, final log SHA-256
  `839DF0715406788F78A222FC0FD9625852F4AE6B022126DA56A16A99EC4A1B62`; package
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, API 28/36, offline network
  permissions absent, seven ARM64 libraries, static 16 KB alignment passed.
- Bundletool `1.18.3` universal APK set: APKS SHA-256
  `062603B21CF398C9D4C3259D7FF49A0F36A56FC5525629E458F425820E107166`; extracted
  universal APK SHA-256 `85297DFA56305322A13614A9A4B89968F3BC0D39E47A7000E5976147096F9AE5`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed; logs are
  `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  `A64974BDF5A94649803322B57AAE514007D7DFD23919C72E1041D5F649D070FE` and
  `F04880347AFBC332098D610434B51BA28FAAE26BE48A2AA52E5F8236FC731FF`.

The exact APK installed successfully on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
API 34). The six-sample, 30-second capture at
`Builds/Local/Device/Performance/20260826-233000-v1-final-current-30s` had no configured
fatal markers, thermal status 0 before/after, and app PSS **55,262–236,543 KB**. Manifest
SHA-256 is `E9BD4D1B922A4AB0FB8EE90DC66AF84FA0876BDD24D89F4E7A253411243268E9`; logcat
SHA-256 is `33FE2B05DA727432C76C4B57428178018CBF62AB35490739AE66C5AA467F68C0`. The device
reports 4 KB pages, so this remains launch/menu evidence rather than 16 KB runtime proof
or a sustained full-match performance pass.

Known non-fatal diagnostics are retained rather than hidden: Unity's Android build log emits
the expected duplicate `ACCESS_NETWORK_STATE` removal-merge warning, while the final manifest
checker confirms forbidden network permissions are absent. Lava logcat includes device-level
`BufferQueueDebug`/gralloc messages, an optional Play AssetPack `ClassNotFoundException` and
an `MBrainLocalService` `SecurityException`; none are configured fatal markers, and the Unity
activity remained top-resumed throughout the capture.

This closes the current-source technical rebuild and removes the prior runtime mixer-warning
regression. Human-authored art/audio polish, cultural review, touch/accessibility, tutorial
and full-route review, sustained Lava performance/thermal/battery, 16 KB runtime validation,
final identity/signing, privacy/Data Safety, content rating and Play Console approval remain
open owner/device/legal gates.

The strict production-bot gate was also rerun twice against this final source with 100 seeds,
release assertions enabled and the existing 50x diagnostic playback setting. Both runs
completed all 100/100 matches and passed bot-to-bot damage, combat-elimination, gadget,
warmup, position and tick-budget invariants, but failed only the pacing-distribution threshold:
run A reached **70/100** matches in the 240–360 second window (report SHA-256
`B654BCCFD65F269CBAE585D7309EE066EC95E7FEF3630F973D2FA7AA0F8CBEDF`, XML SHA-256
`653450DF5B34FA8B3FCB9FD2F7A60D770BC60FF58523900C711DA5CC1CB69B05`, log SHA-256
`E633680F362A18E7EC96966179EE51A73DF39B7D547E399CF764E7E75AFA4B7B`), and run B reached
**76/100** (report SHA-256 `7878FB61323D72CEE2142C949C76BC4D21635C2AA8D0AE9239AE5FEEA7A91FF5`,
XML SHA-256 `3EBE8E2CFCC99CB4111152214AA75DA5560180859A688F1457B5B4983790DAEC`, log SHA-256
`0D6CAE88210D8F790E6547953DDB804A89B2722959B6170E90228D4A6F625370`). This confirms the
known 50x timing sensitivity; the earlier passing attempt remains historical evidence and is
not treated as a stable current-source determinism setting. No threshold was loosened.

### P16 - Final clean-source package, exact soak and Lava refresh - 2026-08-27

The reviewed V1 runtime/presentation source is clean and committed at
`2f9a6a0151e3b0c2359d9b0f8892c28e6404ec4b` (`build: keep tutorial scene file IDs stable`).
The guard is editor/build hygiene only: it preserves valid serialized TutorialOverlay scene
IDs during repeated generation and does not change gameplay authority, replay, or runtime
rules. Working-tree status was clean before and after the evidence runs.

- Full EditMode: **140/140 passed**. XML SHA-256
  `20838BDFD69AA3DD502045F8A05E7EEF0A9C3E5B216D6102AF394DE9BE32B72F`; log SHA-256
  `2841E106DC6F3890EBA550A8509D3CE2FCDD13454BA6DF9C2407DDBBEA4BB4DD`.
- Full PlayMode: **80/80 passed**. XML SHA-256
  `F824BB4372FD8A6B28D1F3BA79770EF4BB6E6C427E2BDC3F07A8E7A380489342`; log SHA-256
  `40F0D8AB10053BE5D5EA03B0462DC5E1B452615311E888EAF8401BCAFFA5BC6C`.
- Exact-source deterministic replay soak: **1/1 passed**, `BATTLERAJA_SOAK_MATCHES=1000`,
  1,000 seeded matches executed twice (2,000 executions), zero divergence, NUnit duration
  **544.1576187 s**. XML SHA-256
  `67F6E10200DCFA7CE420738D0AF5873D6B2C2A98B041FB1C1CFF64AE5C11FC8F`; log SHA-256
  `6CCECACDA39EFA6F5E7DB0DED813CB3BF57C72CE9BBA01C6209D1EDA4CECE2C3`.

Matching Android artifacts were built from this exact source with Unity `6000.5.6f1`:

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,521,770 bytes**,
  SHA-256 `0F635D962A179B28FD07189E348D837A7BF7B647638DDAF7FBF9A7EAB14B3458`.
- `apksigner verify --print-certs` reports Android Debug signer
  `C=US, O=Android, CN=Android Debug`, certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`; this is not a
  publishable release signature.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,346,956 bytes**,
  SHA-256 `4397F62FE5A83CEF2EB5240212988787735289DE8AA24F26D78B9E95C83D168D`.
- Composed release checker: **0 errors / 0 warnings**, package
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, API 28/36, seven ARM64
  libraries, static 16 KB alignment passed. Log SHA-256
  `3D9C56EB1857BA4402F78BA2904069C5D5B09F3CE8669B70DB76A4910140D509`.
- Bundletool `1.18.3` universal APKS SHA-256
  `EA056809A7863EF9E756F2813E356E7143E2211644CC490E9A35772472817E87`; extracted
  universal APK SHA-256 `97242F54E255B2BB945D5989158859E5A6F81C90EE98AD70E69EED7CB2937469`.
  Direct APK and extracted universal APK `zipalign -c -P 16 -v 4` both passed. The
  bundletool, extracted-APK and direct-APK log hashes are respectively
  `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  `3D712477513070394A71AC605C7341DCAADF595C8A812CEEB333EE9FD2D93BFD` and
  `8C4C95FE3C70DDCFA9964E16B75B65BD691E88D140BFB2F7A6201A45DE583CD1`.

The exact APK was reinstalled on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, API 34).
The six-sample, 30-second launch/menu capture is under
`Builds/Local/Device/Performance/20260827-011441-v1-final-2f9a6a0-30s`;
manifest SHA-256 `4995634C13C3C1138FC2C132654A5A7CE62692579696C20C6A120D78BDD15060`,
logcat SHA-256 `98BA9C3C05DDCD3149DD7402BF5970EBC7045E11366EAEA5D41AA785B8B15C28`.
No configured fatal markers were found, thermal status was 0 before/after, and app PSS
ranged **57,379–238,075 KB**. The phone reports 4 KB pages, so this is launch/menu evidence,
not genuine 16 KB runtime proof or a sustained full-match performance pass.

This closes the exact clean-source technical rebuild, but not the release claim. The strict
100-match production-bot batches recorded in P15 were run immediately before this final
editor-only Tutorial scene-ID guard; `2f9a6a0` does not touch the Bazaar harness or gameplay
runtime, so the evidence remains applicable, but no post-guard bot rerun was performed. Those
runs pass all safety/invariant checks while failing their timing distribution (70/100 and
76/100 in the 240–360 second window). Human touch/tutorial/full-route, sustained match performance/thermal/battery,
authored-art/audio/cultural review, final package identity/signing, privacy/Data Safety,
content rating, store assets and Play Console approval remain open.

#### P16 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `2f9a6a0`; 140/140 and 80/80 |
| Deterministic replay/deep soak | **Passed** | 1,000 seeds x2; zero divergence; hashes above |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Technical checker and bundletool evidence above |
| Production-bot 100-match release distribution | **Failed** | Safety invariants pass, but pacing is 70/100 and 76/100 in-window; threshold unchanged |
| Lava install, launch and bounded crash-marker smoke | **Passed** | Fresh six-sample capture; no configured fatal markers |
| Full touch tutorial → match → spectator/results/rematch/settings/lifecycle route | **Blocked** | Requires owner-operated touch review |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Current capture is launch/menu only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

### P17 - Player-facing HUD cleanup, exact UI-source verification and Lava refresh - 2026-08-27

The current runtime/presentation source is clean and committed at
`aeda6debab89404991f55a0f663a88798dd9c944` (`ui: remove internal HUD labels and keyboard hints`).
This patch is presentation-only: it does not change the core authority, replay, bot,
collision or match-rule code. It removes serialized gadget IDs, actor labels, keyboard/
mouse instructions and developer/authority terminology from the player-facing HUD,
tutorial and results copy. The Lava opening screenshot visibly shows `GADGET TIFFIN`,
`READY` and `SPAWN SHIELD` with no `[G]`, `tiffin_station`, `SPAWNPROTECTION` or
`PLAYER 1` leakage.

#### Exact-source automated evidence

- Full EditMode: **140/140 passed**. XML:
  `Builds\\Local\\V1GameplayTruth\\TestResults\\editmode-ui-aeda6de.xml`.
- Full PlayMode: **81/81 passed**. XML:
  `Builds\\Local\\V1GameplayTruth\\TestResults\\playmode-ui-aeda6de.xml`.
- Deterministic replay soak: **1/1 passed**, `BATTLERAJA_SOAK_MATCHES=1000`,
  1,000 seeded matches executed twice (2,000 executions), zero divergence, NUnit
  duration **553.5464039 s**. XML SHA-256
  `65BD32A7B978CB5679546EA3A7ACDFFC91261DC5D1A4CE86C3E280BB1B79C69F`; log SHA-256
  `46CC6D65C14B28A4315D576032E1BA8093E65B843011EE3660FE897401EB30A5`.

#### Matching Android artifacts from `aeda6de`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,523,450 bytes**,
  SHA-256 `62764237F44B1DD0D9F5B6E2E37C582FBA9B57B088B46C30805C883C123CAE65`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,348,625 bytes**,
  SHA-256 `34F2E2D1318A8DF24EF9E3968511BE8686DDAE207D4ACBCA16801F247E11A6D6`.
- `apksigner verify --print-certs` remains the Android Debug certificate
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`; this is not a
  publishable release signature.
- Composed release checker: **0 errors / 0 warnings**, package
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, API 28/36, VIBRATE plus
  Unity's dynamic receiver permission only, seven ARM64 libraries, no other ABIs,
  static 16 KB ELF alignment passed, and store-creative dimensions passed. Checker log
  `Builds\\Local\\V1GameplayTruth\\Logs\\check-v1-release-candidate-ui-aeda6de.log`
  SHA-256 `2A7E82EC78CD3EC6E42DD37B369D728A019B86C31906B017932027AB2586CD2C`.
- Bundletool `1.18.3` universal set APKS SHA-256
  `7C03F94C5E1DE08A3F417C49001702499B0D7B7EE6B49FD41B09D5143215D43B`; extracted
  universal APK SHA-256
  `378B667014E87EC93B501056E709769A5515E94C3F92D4911A472B004647F976`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed. The final bundletool,
  extracted-APK and direct-APK log hashes are respectively
  `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  `EE24BB8D705F8F7D70118E4FECB3F5BA1D58D2A91689861D17E741655189371C` and
  `A3B0B60EDDC5DB30D431B1E04D8BF9EF29F0EB7ED60F0F2A151AAD3132492B89`.

#### Approved Lava evidence

The exact APK was installed with `adb -s ST5GDW23LB004392 install -r` and the actual
menu → Solo Raja → drop-in → live opening route was reached by touch automation. The
review captures are:

- `Builds\\Local\\Device\\Screenshots\\20260827-aeda6de\\launch-menu.png`, SHA-256
  `9508D68E065586AC71722D073A25D53A34B58D502254283044DADFE62F18F9D8`.
- `Builds\\Local\\Device\\Screenshots\\20260827-aeda6de\\solo-opening.png`, SHA-256
  `E6F7C9B7E0FAF0182246FD99FAA2D03C6A1C180058DFF09DC96B418267CFE7CC`.

The fresh six-sample, 30-second capture at
`Builds\\Local\\Device\\Performance\\20260827-014609-v1-ui-aeda6de-30s` found no
configured fatal markers and thermal status 0 before/after. Manifest SHA-256 is
`A9C19ECC98A8E5C282720AFB8CA6145F328A46AA49763DD9AA66016A6CFB2A5B`; logcat SHA-256 is
`625FC8638DEBC96BA2817DBEB6B6D98186EE01A8AB730276D912AEDF00F392F7`. The device is
`LAVA LXX508`, API 34, reports 4 KB pages, and app PSS ranged **41,979–236,451 KB**.
This is launch/menu plus opening-screen evidence, not sustained full-match performance
or genuine 16 KB runtime validation. Human visual/touch review remains open.

The strict production-bot evidence remains the two P15 runs (70/100 and 76/100 in the
240–360 second window). No post-`aeda6de` bot rerun was performed because this patch is
player-facing presentation only and does not touch the harness or gameplay. The batch
therefore remains **Failed** on pacing, with its safety/invariant passes preserved.

#### P17 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `aeda6de`; 140/140 and 81/81 |
| Deterministic replay/deep soak | **Passed** | 1,000 seeds x2; zero divergence; hashes above |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Technical checker and bundletool evidence above |
| Player-facing HUD/tutorial/results label cleanup | **Passed** | UI regressions plus actual Lava opening screenshot |
| Production-bot 100-match release distribution | **Failed** | Existing P15 runs pass invariants but only 70/100 and 76/100 in-window |
| Lava install, launch and bounded crash-marker smoke | **Passed** | Fresh install and six-sample capture; no configured fatal markers |
| Full touch tutorial → match → spectator/results/rematch/settings/lifecycle route | **Blocked** | Requires owner-operated touch review |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Current capture is menu/opening only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

### P18 - Fair production bots, exact-source Android rebuild and release-gate refresh - 2026-08-27

The current runtime/presentation source is clean and committed at exact SHA
`6d287a657dd946c806ac54580b4d5a5ea1e53ee4` (`test: keep production bot diagnostics and
projectile checks robust`). The gameplay change preceding this commit makes production
bots fair and deterministic: bot weapon damage is bounded to `0.9x` (never above the
human definition), all production bots use a `25x` attack cadence, and the editor harness
advances one canonical 30 Hz tick at a time. The final commit contains only the related
regression-test correction after the gameplay source was validated.

#### Exact-source automated evidence

- Full EditMode: **140/140 passed**. XML SHA-256
  `7DEF8576AB1015182FAF97048BE4DA07BEE20AEF4FE74E7C10867211ED26C1F7`; log SHA-256
  `780C559942F8FCD974EED32D7CF05F00151262CC048EA3FB4D358B97E1601C1A`.
- Full PlayMode: **81/81 passed**. XML SHA-256
  `79945934704190F4F00FDC3FC21156D0511E0624787ECDCCBC4E000AD22AD2DF`; log SHA-256
  `8D2B31FAC924D6ED67C1D7F196A85D4F16A43BABBF331E0D2D7E8F03773EACD3`.
- Exact-source deterministic replay soak: **1/1 passed**, `BATTLERAJA_SOAK_MATCHES=1000`,
  1,000 seeded matches executed twice (2,000 executions), zero divergence, NUnit
  duration **538.822974 s**. XML SHA-256
  `DB133AE5BD7855175FECA4ED909F0C67FCE4F9607C98A4FE355683B029122186`; log SHA-256
  `C3BBAEB98E9C5EA3F3C88B97B0C75953FD1CADC7F817E9FE87A470D9017D4D97`.
- Exact-source fixed-tick production-bot batch:
  `Builds\\Local\\V1GameplayTruth\\ProductionBotReports\\batch-20260826-220514174-9101.json`,
  SHA-256 `74A705D19CFB271CAB2988003AAD4F270860E3D55952F1B5022D75E6565070E5`.
  All **100/100** matches completed in **306.013519 s** (100/100 in the 240-360 s
  window; 0 over 360 s), with **95/100** having at least three combat eliminations and
  100/100 having at least one combat elimination. All 100 matches had bot-to-bot
  damage; Aandhi-only matches were 0; invalid-position and protected-warmup samples
  were 0; maximum continuous stuck ticks were 0; and maximum outside participants was
  6. The batch recorded 15,149 attacks (58 out of range, 0.383%), 38,813 ability
  attempts (8,204 rejected, 21.137%), 297 successful gadgets (Umbrella 97, Dhol 100,
  Tiffin 100), and 5,680,291 commands.
- Exact-source same-seed production comparison:
  `batch-20260826-220854693-9101.json` SHA-256
  `7FED42B7077B519D7EF145600F6F27689FEA20D87E5C83490301C43C7DFA6901`; seed 9101
  reproduced duration `306.013519 s`, command count `56,374` and command digest
  `72EAEEA69632FECC`, matching the first match in the 100-match batch.

#### Matching Android artifacts from `6d287a6`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,525,610 bytes**,
  SHA-256 `888F796151789CD21F50CB966B42908D75610E45724D6D3C2BD105836F83373A`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,350,785 bytes**,
  SHA-256 `535015D9B35C49B3A71EDE0A4059A05280C135C1914FD218FE076F91ACED061A`.
- Composed checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36, VIBRATE plus Unity dynamic receiver only,
  seven ARM64 libraries, no other ABIs, static 16 KB ELF alignment passed, and
  creative dimensions passed. Checker log SHA-256
  `86E056E92F246CD7B7A139EB75D561CBCF4D589773DB4A19AA92680C167D652C`.
- Bundletool `1.18.3` APKS SHA-256
  `DE0FC268BF4165BB9A8D7EE03AC40A95D74709470459324AF38CEB5E79509FCA`; extracted
  universal APK SHA-256 `F2BB7148D26AB1B02085BEF33EFF7F770CDD68E2D795D49F6E7BD651735BC5CC`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed; direct and universal
  `apksigner verify --print-certs` both passed with the Android Debug certificate
  SHA-256 `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`.

#### Approved Lava evidence

The exact APK and the bundletool universal APK were installed on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, API 34). The fresh touch route reached the live Solo
Raja opening screen. Screenshots:

- `Builds\\Local\\Device\\Screenshots\\20260827-6d287a6\\launch-menu.png`, SHA-256
  `51B0D656F0AE932B4297BD457335EE5B06561C62BAB99751F3A5D6A803F3820A`.
- `Builds\\Local\\Device\\Screenshots\\20260827-6d287a6\\solo-opening.png`, SHA-256
  `35AB8CA2ED3DBECF29C89C8669FB705B38ECC91705C267B2A221A472B88C6588`.

The six-sample, 30-second capture under
`Builds\\Local\\Device\\Performance\\20260827-6d287a6-v1-30s` found no configured
fatal markers and thermal status 0 before/after. Manifest SHA-256 is
`C56D749210EE0050D3BEAF85E9B81063ABE76238E62FAB985F0D457AB066BDC8`; logcat SHA-256 is
`E8058599DE4EC406EDFB1AD7C45B92F1BFCC0ED9EECA54D9FC1911F4B12F1AF2`. App PSS ranged
**42,759-235,905 KB**. The phone reports 4 KB pages, so this is launch/opening evidence,
not genuine 16 KB runtime proof or a sustained full-match performance pass.

#### P18 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `6d287a6`; 140/140 and 81/81 |
| Deterministic replay/deep soak | **Passed** | 1,000 seeds x2; zero divergence; hashes above |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Technical checker and bundletool evidence above |
| Production-bot 100-match release distribution | **Passed (automated)** | 100/100 in-window; 95/100 with >=3 combat eliminations; invariant checks pass |
| Exact-source same-seed production command stream | **Passed** | Seed 9101 digest and command count reproduced |
| Lava install, launch and bounded crash-marker smoke | **Passed** | Fresh install and six-sample capture; no configured fatal markers |
| Full touch tutorial -> match -> spectator/results/rematch/settings/lifecycle route | **Blocked** | Requires owner-operated touch review |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Current capture is launch/opening only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

This is a clean, technically validated offline Android candidate, not a Play-publishable
release claim. The APK is debug-signed; physical full-route review, sustained match
performance, genuine 16 KB runtime validation, final authored/accessibility/cultural
approval, release signing, privacy/Data Safety, content rating, store assets and Play
Console approval remain open.

### P19 - Tutorial visibility fix and exact candidate refresh - 2026-08-27

The current source is clean and committed at exact SHA
`e6c321b60c8398755942ab0260d13dddac3df551` (`fix: keep tutorial arena visible behind
prompts`). This presentation-only patch removes the opaque full-screen tutorial backdrop
that obscured the live arena and adds a PlayMode regression asserting that the tutorial
prompt does not recreate it. The preceding fighter-selection focus correction remains in
the exact source history at `62b728c`.

#### Exact-source automated evidence

- Focused tutorial PlayMode: **2/2 passed**. XML SHA-256
  `FB0A1EA192A17F3E671928C22FD4D1D74A75CDD4086CEB38EA829F4F9805A9AA`; log SHA-256
  `F789237773099802EE42EEF07C17226C56BEE15EC5E8533B0A7D9DBBFD8B3104`.
- Full EditMode: **140/140 passed**. XML SHA-256
  `6D8FB225A249C80753406D3ED0BA640D53F64632A5E1B59E1EE3A2AED3B5224C`; log SHA-256
  `5F39044A0DF0079022725A04CAC641DAC843A5C6CE0C23EC671CE998ACBF958B`.
- Full PlayMode: **82/82 passed**. XML SHA-256
  `219B1727A9186D940562C4F56F54262FC946E4AECD9B81961BA3878002A3FFD7`; log SHA-256
  `6A7EBAA52185C4F484533CA2194830B2BE77825E9F4073A83F382959CF9E5CB4`.
- The deterministic replay soak and fixed-tick 100-match production-bot batch are
  unchanged by this presentation-only patch; the exact `6d287a6` evidence remains
  applicable to gameplay truth and is retained above (1,000 seeds x2 with zero
  divergence; 100/100 in-window bot matches, 95/100 with at least three combat KOs).

#### Matching Android artifacts from `e6c321b`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,524,858 bytes**,
  SHA-256 `E1408B65F89317885FF64F1C94D80417385E86600420F77BCA3428E378260403`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,350,021 bytes**,
  SHA-256 `E94945CA57AA71B510524C73AB9470F839045584784238E1093D3A4834116E11`.
- Composed checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36, VIBRATE plus Unity dynamic receiver only,
  seven ARM64 libraries, no other ABIs, static 16 KB ELF alignment passed, and store
  creative dimensions passed. Final checker log SHA-256
  `8CB20C2EE2E0C4C8FC282B83D1444B1B903D716A9F3AA0D051F6B65A6B23DC32`.
- Bundletool `1.18.3` generated APKS
  `Builds/Local/V1GameplayTruth/Android/battleraja-v1-e6c321b.apks`, **36,479,209
  bytes**, SHA-256 `03EAB13BCECF468F9176E7E0033A2E8AAF759563A77576F28B3327FC2B661425`.
  The universal APK was extracted from that APKS archive at
  `universal-e6c321b-zip/universal.apk`, **36,478,894 bytes**, SHA-256
  `10EED00C704E0A87A6C16059E972284B04903108911F79BE15AB24825C1560EE`.
- Direct and extracted APK `zipalign -c -P 16 -v 4` both passed. Log SHA-256 values:
  bundletool `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  direct zipalign `6C2C708BC4198FB865E40200ED5B2D73171465DAA9405E640F4C0CA16F65A2D5`,
  universal zipalign `0D1A466857B240ACA5EE8BCF0CA0E95A0C77EC67E4C94BF1241CEE4C4B65AF4A`,
  direct apksigner `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A`,
  universal apksigner `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A`.
  Both signatures verify with the Android Debug certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`; this is not a
  publishable release signature.

#### Approved Lava evidence

The exact e6c321b APK installed successfully on approved Lava `ST5GDW23LB004392`
(`LAVA LXX508`, API 34). The fresh screenshots are under
`Builds/Local/Device/Screenshots/20260827-e6c321b`:

- `launch-menu.png`, SHA-256
  `002F36939339627A53068CDE48AEDEC64C628711C2F8C799772FBC8034AA3609`.
- `tutorial-opening.png`, SHA-256
  `1ECB0C39A45B617674557D1ACA920410C4EDED38ED7BD2380114E5976FFEDCAD`.
- `tutorial-movement-performed-2.png`, SHA-256
  `5F0748B938DAFC552A28FBCF2F4BB5B04DD5684D253AE54163F833013CD22D86`.

The tutorial opening visibly contains the live Bazaar arena, eight fighters, zone ring,
HUD and both touch sticks behind the movement prompt; the previous blank-dark-screen
failure is therefore fixed on the exact candidate. A shell swipe was attempted on the
left-handed MOVE stick, but the prompt remained waiting, so no action-by-action physical
tutorial completion claim is made. The captured tutorial logcat has SHA-256
`969BAE3F05D9A720F06D8B49530589EC5C60D528382637EC9981A2F39D5051B` (2 lines, zero
configured fatal markers). The exact candidate's full tutorial/match/spectator/results/
rematch/settings/lifecycle route remains an owner-operated review gate.

#### P19 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `e6c321b`; 140/140 and 82/82 |
| Tutorial prompt keeps live gameplay visible | **Passed (automated + Lava visual)** | Regression test and exact `tutorial-opening.png` |
| Deterministic replay/deep soak | **Passed (carried forward)** | Presentation-only patch; exact `6d287a6` 1,000-seed x2 evidence remains applicable |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Exact e6c321b checker and artifact evidence above |
| Production-bot 100-match release distribution | **Passed (carried forward)** | Presentation-only patch; exact `6d287a6` batch remains applicable |
| Exact-source same-seed production command stream | **Passed (carried forward)** | Presentation-only patch; exact `6d287a6` digest remains applicable |
| Lava install, launch and bounded crash-marker smoke | **Passed** | Exact e6c321b APK installed; tutorial logcat has zero configured fatal markers |
| Full touch tutorial -> match -> spectator/results/rematch/settings/lifecycle route | **Blocked** | Owner-operated action-by-action review remains required |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Current exact e6 evidence is launch/tutorial visual only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

### P20 - Persisted fighter focus correction and exact candidate refresh - 2026-08-27

The current source is clean and committed at exact SHA
`8edc0867268800f0ad81067378ad590e1a166371` (`fix: restore fighter focus on selection
screen`). This presentation-only patch applies the persisted fighter choice when the
fighter-selection screen opens, so the summary, focus ring and keyboard/switch navigation
all agree before the player taps a card. A PlayMode regression seeds a persisted Maya
choice, opens the screen and asserts the Maya card is selected.

#### Exact-source automated evidence

- Focused persisted-focus PlayMode: **1/1 passed**. XML SHA-256
  `3066ED9594E6815651C63E9F5FB41F15534D4E8E5ED2627ED3455A644B4E6615`; log SHA-256
  `C9392F7A7CCD31B0828F1B53EFAD8D9E2AFED22A1343204031D18C79602AF370`.
- Full EditMode: **140/140 passed**. XML SHA-256
  `667719529903AA7E0E3BEC86B9A6B7F10A5E9EB0C861D445E8363B445C7BB150`; log SHA-256
  `8662659959183F343C3D9CA624C4E12E144DEABCF87EFB85C93707D3423FDE48`.
- Full PlayMode: **82/82 passed**. XML SHA-256
  `C8F196AC9C59854147E3466BCEFACEAA5016F22C7B71B24246AFFE3B432B4798`; log SHA-256
  `E4543BFF254A9EB2EB8D97A8D2B59DABA8ABC535A418A0063AFAC61EF5A682FD`.
- The deterministic replay soak and fixed-tick 100-match production-bot batch remain
  applicable from exact gameplay source `6d287a6`; neither focus patch changes authority,
  replay or bot simulation.

#### Matching Android artifacts from `8edc086`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,521,770 bytes**,
  SHA-256 `1D470DAEEBEBE86D3764A594BCF4D6CF71869854E84B38E41D4FC6BCB8974E03`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,346,941 bytes**,
  SHA-256 `4FFC517CAE9CD112F6D5D34A1A039A30D090EC2042161F7C1EC8D516966B8697`.
- Composed checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36, VIBRATE plus Unity dynamic receiver only,
  seven ARM64 libraries, no other ABIs, static 16 KB ELF alignment passed, and store
  creative dimensions passed. Final checker log SHA-256
  `E97E3DAA4E7A3E641927635972FC0C3F0C29A6BCF7EC947277108BFD09BDE052`.
- Bundletool `1.18.3` generated APKS
  `Builds/Local/V1GameplayTruth/Android/battleraja-v1-8edc086.apks`, **36,475,113
  bytes**, SHA-256 `5CC8D07F070A6244DF3DBBFDDCCB0EE6CBE3B7019F0543507DEBDACA44644EA8`.
  The universal APK was extracted from that APKS archive at
  `universal-8edc086-zip/universal.apk`, **36,474,798 bytes**, SHA-256
  `5C48B45DCDCB35E7BF4010320CDD3226CBA094A5E3A8744D08BCB49B441519FE`.
- Direct and extracted APK `zipalign -c -P 16 -v 4` both passed. Log SHA-256 values:
  bundletool `DF3173BCAED672FE955EC394A49B6A91A47557D4DCBB68C4AED8612E71506EEC`,
  direct zipalign `B53ACF9B7694D299958F58B78EDCEE0717FEE635B4E74110D574E76979B342E6`,
  universal zipalign `EC8F7182D428E8ED85A0C12021DFEAC455B4852EE9BC9B1FDB70DB703B468D50`,
  direct apksigner `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A`,
  universal apksigner `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A`.
  Both signatures verify with the Android Debug certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`; this is not a
  publishable release signature.

#### Approved Lava evidence

The exact 8edc086 APK installed successfully on approved Lava `ST5GDW23LB004392`
(`LAVA LXX508`, API 34). Fresh exact-candidate screenshots under
`Builds/Local/Device/Screenshots/20260827-e6c321b` include:

- `fighter-cards-persisted-fix.png`, SHA-256
  `1AD9D3A12EDB0788BA85CF3010C198CD2654065C59BEF4D42994108EF92DC741` — summary
  `SELECTED: MAYA` and Maya card focus/highlight agree on first entry.
- `maya-opening-focus-persisted-fix.png`, SHA-256
  `4347D7521DF07CF142B400836F0E9FBD14E79981EF12AFDD3DCACC5C876ED9FA` — live Solo
  Raja opening with Maya HUD and left-handed controls.
- `maya-ability-gadget.png`, SHA-256
  `E63F35669D841BEA38361A62970AFFAB1DEABB50AAD76E95FA88898F1E033DB1` — Maya ability
  and Tiffin use reflected in HUD (`DECOY`, `GADGET EMPTY`, `TIFFIN STATION DEPLOYED`).
- `maya-pause.png`, SHA-256
  `578CE715481F78C8147D621270917CD044D15428892C0E8C0C7109A727E30648` — pause/settings
  surface keeps live gameplay visible behind the settings panel.
- `maya-lifecycle-resume.png`, SHA-256
  `96F49B2834DD305086DC5A7555FB87DF08A668B7138F832F35561890258E3D78` — match resumed
  after HOME and relaunch; `maya-lifecycle-logcat.txt` SHA-256
  `D2DD6B908F8B0EC3E19F8A82BF40935C3AD1030F83C2FD5EEE65A23FFC97C913` contains zero
  configured fatal markers.
- `tutorial-opening-8edc086.png`, SHA-256
  `18236860F318754EC89F3CDFD75F62D1C6ADD9E8EF3E428EB7333EFC5A596CDC` — live arena and
  controls remain visible behind the action-gated movement prompt on the latest APK.

These probes strengthen exact-candidate evidence for fighter focus, live ability/gadget
feedback, pause and lifecycle resume. They do not constitute owner approval of comfort,
accessibility, authored art/audio, sustained performance or full action-by-action tutorial,
spectator/results/rematch and settings completion.

#### P20 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `8edc086`; 140/140 and 82/82 |
| Persisted fighter summary and focus ring agree | **Passed (automated + Lava visual)** | Regression test and exact `fighter-cards-persisted-fix.png` |
| Tutorial prompt keeps live gameplay visible | **Passed (carried forward + Lava visual)** | Exact latest tutorial opening remains visible; P19 fix unchanged |
| Deterministic replay/deep soak | **Passed (carried forward)** | Focus-only patch; exact `6d287a6` 1,000-seed x2 evidence remains applicable |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Exact 8edc086 checker and artifact evidence above |
| Production-bot 100-match release distribution | **Passed (carried forward)** | Focus-only patch; exact `6d287a6` batch remains applicable |
| Exact-source same-seed production command stream | **Passed (carried forward)** | Focus-only patch; exact `6d287a6` digest remains applicable |
| Lava install, launch, ability/gadget, pause/resume and bounded crash smoke | **Passed** | Exact APK installed; probes and logcat recorded above |
| Full touch tutorial -> match -> spectator/results/rematch/settings/lifecycle route | **Blocked** | Owner-operated action-by-action review remains required |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Not run** | Exact probes are short visual/lifecycle captures only |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

### P21 - Exact-candidate sustained Lava match diagnostic - 2026-08-27

The exact P20 runtime candidate `8edc0867268800f0ad81067378ad590e1a166371` was
left in a live Solo Raja match on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
API 34) for **120 seconds**, with **12 samples at 10-second intervals**. This is a
runtime diagnostic follow-up; no source or Android artifact changed after P20.

#### Captured evidence

- Raw evidence: `Builds/Local/Device/Performance/20260827-8edc086-match-120s/`.
- Manifest SHA-256:
  `8179BC75000B504330E88E88494AA7DBA918322368DB5444E72E8881CC68B675`.
- Captured logcat SHA-256:
  `197C33A22A28072F6A8599C2519F6915F019A9CF99FBB1CD04AFD3B83CBC3CEC`.
- Unity's game activity stayed focused for all 12 samples. Total PSS was
  **218,208-228,459 KB** and total RSS **364,956-374,796 KB**; graphics PSS was
  **17,484 KB** at every sample and swap PSS **64-77 KB**. After the first warm-up
  sample, total PSS stayed within **218,208-218,280 KB**.
- Raw `top` samples were **103-128% instantaneous process CPU**. Thermal HAL
  CPU/GPU readings were approximately **38.539-40.786 C** with status `0`; Android
  battery dumps remained at **19%** and **31 C** before and after (USB powered).
- Android `gfxinfo` reported **0 total frames** and no usable Unity SurfaceView frame
  histogram in every sample, so no FPS, jank, GPU-timing or frame-pacing pass is
  claimed. The configured fatal-marker scan found **0** hits.

#### P21 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Measured diagnostic / still open** | 120-second Lava match capture is bounded and thermally stable, but raw CPU is not normalized, gfxinfo has no usable frame histogram, GC/draw-call/repeated-match-growth and endurance evidence are absent |
| Lava install, launch, live match and bounded crash smoke | **Passed (carried forward)** | P20 exact APK remained live for all 12 samples; zero configured fatal markers |
| All other P20 gates | **Unchanged** | See P20 classification above; full physical route, genuine 16 KB runtime, authored/accessibility/cultural review, signing and Play/legal gates remain owner-controlled |

P21 therefore improves measurement coverage without changing the release
classification: this remains a technically validated offline prototype candidate,
not a Play-publishable release claim.

### P22 - Handedness-aware tutorial prompt and exact-candidate refresh - 2026-08-27

The current runtime/presentation source is clean and committed at exact SHA
`208038362e16f8c33856e0a7cf5c4de776005ded` (`fix: localize tutorial stick instructions`).
The tutorial now names the active movement/aim stick from the persisted handedness
setting. A focused PlayMode regression covers the left-handed prompt; the existing
tutorial visibility and persisted fighter-focus fixes remain in the exact source history.

#### Exact-source automated evidence

- Focused left-handed tutorial PlayMode: **1/1 passed**. XML SHA-256
  `A3FDBCBB287EDC57FF451DD4C398C87923B3D41E48FB111469150D2FB28C5CEC`; log SHA-256
  `154DF5B59875639C5EC55A5BB637B503DBF951849FE85FF8F840EF41735C4E0C`.
- Full EditMode: **140/140 passed**. XML SHA-256
  `67B018C240BA3591FDE82166C61CC3558902609C7481246692A762FCFC8094D4`; log SHA-256
  `FC32157E622ABE075DEBCC9326F54FE3E030CBDE8E5B5E671E692F79A7DA2E8E`.
- Full PlayMode: **83/83 passed**. XML SHA-256
  `12FF7F6E22CFF3E9D23C04F32F781CE157B3C47EFB2BF056BD03376DD028EBC5`; log SHA-256
  `E068351B02A1F4727A240159B1009AA0EA72570AE49784BA88F6E15E66A79C25`.
- Static validation: **0 errors / 0 warnings** from `Tools/Validation/validate.ps1`.
- Deterministic replay soak and fixed-tick 100-match production-bot evidence remain
  applicable from exact gameplay source `6d287a6`; neither this presentation-only
  handedness fix nor its tutorial regression changes authority, replay or bot simulation.

#### Matching Android artifacts from `2080383`

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,523,706 bytes**,
  SHA-256 `365ABF4A1D37BB6DC2CE7E08F5E2741AAB7662EFB9749F0B4987EBFCBDB68BDB`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,348,870 bytes**,
  SHA-256 `F1CB13C80A6408B344B5C71BE11D0AD804E58CA1D01102FE0B79D5B0712BDBA1`.
- Composed checker: **0 errors / 0 warnings**, package `com.example.battleraja.m11`,
  version `1.0.0`/code `100`, API 28/36, only VIBRATE plus Unity's dynamic receiver,
  seven ARM64 libraries, no other ABIs, static 16 KB ELF alignment and store creative
  dimensions passed. Checker log SHA-256
  `62D0E7DF8541FD01ACAB9BC17BACE65B6A04814832ABD4CADE52469317D4DB89`.
- Bundletool `1.18.3` APKS `Builds/Local/V1GameplayTruth/Android/battleraja-v1-2080383.apks`
  SHA-256 `5F4720D79A0BF26387A0C9C4BD197BAFA60FDD946BB17C898557D9974D21DE0A`;
  extracted universal APK SHA-256
  `C17320E5444629A6BB18B03FA2186A7B5F46F4DF5F463F7FFFA51D709196EFD5`.
  Direct and extracted APK `zipalign -c -P 16 -v 4` both passed; the direct and
  universal apksigner verification logs both have SHA-256
  `BCABF5EE2B220F3F612B5C16A74690E469631C321076863D84320010CA0BFF0A` and verify with
  Android Debug certificate SHA-256
  `b0a94c79c2d3fa527d4160b46a3067fbe25bd4db0e1a2dafe1a62b1bce41b28c`.

#### Approved Lava evidence

The exact 2080383 APK was freshly uninstalled/installed on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, API 34), launched, and navigated through menu,
Solo Raja mode, fighter selection and a live match. Fresh screenshots are under
`Builds/Local/Device/Screenshots/20260827-2080383/`:

- `launch.png`, SHA-256 `39C912E3EA9B4E8D15F30F92317DF35C0E23F750EA68892AE58AB566F75731F9`.
- `tutorial-default.png`, SHA-256
  `1652FC25F64710AD5D945C2F4A9C43802782E850BE65E0CAA15DD19074CD99C8` — default layout
  visibly says **“Use the left stick to move”** with MOVE left and AIM right.
- `settings.png`, SHA-256
  `D3144806370F530C662E730BDF17C61D4638FF8846012BDCCA2E9BA9B9F9316F` — handedness,
  reduced flashes, contrast, aim assist, text size, audio and haptics controls are visible.
- `tutorial-left-handed.png`, SHA-256
  `F0E95D13438696AE2E7BEB069E07BD1A657EEC8F833C98F5B200DA1CD64280D7` — after enabling
  LEFT-HANDED, the exact prompt says **“Use the right stick to move”** and the controls
  swap to AIM left / MOVE right.
- `live-match.png`, SHA-256
  `7576F26CDC1AFA95409234A8F47C45A612369648606DABA06EE3064323AC47D8` — live Solo Raja
  opening with eight actors, zone, HUD, touch controls and SPAWN SHIELD state.
- `tutorial-skip-result.png`, SHA-256
  `EC27C78B207F1528B8B4780DC3205B8FA8B124CA26F8B118424B46791B181B19` — the skip path
  reaches the Tutorial Complete 8/8 surface, but ADB stick swipes did not unlock the
  first movement step, so no action-by-action physical tutorial completion is claimed.

#### Exact-candidate frame-latency diagnostic

While the exact 2080383 APK was in the live Solo Raja match, SurfaceFlinger latency was
cleared and collected for approximately 15 seconds from layer
`SurfaceView[com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity](BLAST)#6701`.
Raw evidence is under
`Builds/Local/Device/Performance/20260827-2080383-frame-latency/`:

- Raw latency SHA-256 `279CA8F22324CF66E4D42AD99E2350500FF1562BF72ED82FB1EC01772DC89E06`.
- Summary JSON SHA-256 `97EBF9305DFA4A45962CB446DA04D7770E29C9AA71CB5C1ACBC765B1D75D46A7`.
- Refresh period **16.666667 ms**; **126** valid middle-column timestamps and **125**
  intervals after excluding one Long.MaxValue sentinel. Min/median/p95/p99/max intervals
  were **16.485 / 16.535 / 16.567 / 16.580 / 33.382 ms**; one interval exceeded the
  refresh period and one exceeded 2x. This is a ring-buffer diagnostic, not Unity
  Profiler, GPU/GC, repeated-match endurance or full performance-budget approval.

#### P22 gate classification

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Clean committed source, compile, full EditMode/PlayMode | **Passed** | `2080383`; 140/140 and 83/83; static 0/0 |
| Tutorial prompt names active movement stick | **Passed (automated + Lava visual)** | Focused 1/1 plus default and left-handed screenshots |
| Deterministic replay/deep soak | **Passed (carried forward)** | Exact gameplay source `6d287a6`; 1,000 seeds x2, zero divergence |
| Production-bot 100-match release distribution | **Passed (carried forward)** | Exact gameplay source `6d287a6`; 100/100 in-window |
| APK/AAB manifest, ARM64, static 16 KB, bundletool and zipalign | **Passed** | Exact 2080383 checker and artifact evidence above |
| Lava install, launch, route and bounded crash smoke | **Passed** | Fresh install, menu/mode/fighter/live-match route, no crash marker observed |
| Full touch tutorial -> match -> spectator/results/rematch/settings/lifecycle route | **Blocked / partially evidenced** | Exact match reached spectator/results and REMATCH returned to a fresh opening; owner-operated action-by-action tutorial, settings and lifecycle review remains required; ADB movement swipe did not unlock step |
| Sustained full-match CPU/GPU/GC/thermal/battery budget | **Measured diagnostic / still open** | SurfaceFlinger ring-buffer sample above; normalized CPU/GPU/GC, endurance and thermal/battery gates remain open |
| Genuine 16 KB runtime device validation | **Blocked** | Approved Lava reports 4 KB pages; requires a genuine 16 KB environment |
| Final authored art/audio, accessibility, balance and cultural review | **Blocked** | Human review and authored polish remain |
| Final identity/signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized |
| Photon, PlayFab, accounts, online and Web release | **Not applicable** | Explicit V1 offline scope lock |

P22 records the strongest exact-source offline Android candidate to date, including the
handedness correction and fresh device measurements. It remains a technically validated
offline prototype candidate, not a Play-publishable release claim.

### P23 - Exact-candidate 120-second Lava match measurement - 2026-08-27

The exact 2080383 APK was left in a live Solo Raja match on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, API 34) for **120 seconds**, with **12 samples at
10-second intervals**. This is a measurement refresh for the exact current artifact;
it does not alter gameplay or release classification.

#### Captured evidence

- Raw evidence: `Builds/Local/Device/Performance/20260827-2080383-match-120s/`.
- Manifest SHA-256 `C7397463F75F7631DA01C10EAE0A2F9D139ECF015B78F4E3463528620EE1F8F1`;
  captured logcat SHA-256
  `91C5011BB8F1A94DE593D4969566C035DFF696E988DA427303BF8415DEE91F29`.
- Unity's game activity was the focused window for **12/12** samples. Total PSS ranged
  **267,935-272,772 KB**, RSS **404,440-408,812 KB**, graphics PSS
  **75,132-79,228 KB**, and swap PSS **64-77 KB**. The process PSS did not show monotonic
  growth after warm-up, but this is a short single-match sample.
- Raw `top` process samples were **106-115% instantaneous CPU**. Thermal HAL CPU/GPU
  readings were **38.676-38.982 C** with thermal status **0**; battery remained at
  **19% / 31 C** before and after, USB powered. No throttling was observed.
- Android `gfxinfo` exposed only the Unity ViewRoot (no usable frame histogram), so no
  FPS, jank or GPU-timing approval is claimed. The configured fatal-marker scan found
  **0** hits. A complete raw-file hash listing is retained as `hashes.txt`, SHA-256
  `630CF3279FE11202D0E054B3927D2D024806D7136B2B187BE183A1BDD9C27EB9`.
- During the same run, the player was eliminated and the live result reached the
  spectator/results surface with placements and REMATCH/MENU actions visible. The
  results screenshot is `Builds/Local/Device/Screenshots/20260827-2080383/live-match-after-120s.png`,
  SHA-256 `9B3E32E471F6C4C4C401AED9517B1DE991443D62BDC06D29D4443CC0AA6D0548`.
  Tapping REMATCH returned to a fresh eight-alive Solo Raja opening on the same exact
  APK; `rematch-after-120s.png` SHA-256
  `8C9FC5989509174C0862A67E6CCE75021B50D2B9A8675180A6A1625D42D52737`.

#### P23 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Exact 2080383 sustained live-match measurement | **Measured diagnostic / still open** | 12/12 focused samples; bounded memory and thermal values; raw CPU is unnormalized and gfxinfo has no Unity frame histogram |
| Lava install, launch and bounded crash smoke | **Passed (carried forward)** | Exact APK remained live for all 12 samples; zero configured fatal markers |
| Full-match performance against explicit budgets | **Not approved** | Unity Profiler/FrameTiming, GPU/GC, draw-call, repeated-rematch growth, unplugged battery and longer endurance remain open |
| All other P22 gates | **Unchanged** | See P22 classification above; full physical route, genuine 16 KB runtime, authored/accessibility/cultural review, signing and Play/legal gates remain owner-controlled |

P23 strengthens exact-candidate performance evidence without converting a bounded Lava
diagnostic into a general mid-range-device or Play-release performance claim.

### P24 - Sustained Lava touch movement probe - 2026-08-27

The exact `2080383` APK remained installed on approved Lava `ST5GDW23LB004392` while the
replayable tutorial was opened with the persisted LEFT-HANDED layout. A controlled
`adb shell input motionevent` sequence held the right MOVE stick, sent repeated MOVE
updates, and released it. The knob visibly tracked the drag. The retained screenshot
shows `CONTINUE`, but it was captured after the run had already reached results, so the
visual alone cannot attribute the unlock to that gesture rather than another state
change. A temporary local Development APK, built from commit `920edc2` with logging
only in `MovementPlayerAgent`, isolated the path: the same gesture produced nonzero
`MovementInputFrame` values with `authority=False`, `external=False`, and
`locked=False`, and `CharacterController.Move` produced displacement. The diagnostic
build is not a release artifact and does not prove repeatable alive-state progression
or the full tutorial route.

Evidence:

- `Builds/Local/Device/Screenshots/continuation-touch-hold.png`, SHA-256
  `94432FE6B7E261E219C809CB8F2474C2B50F31F7DFAB3F76170C7790C6B6461B` — exact
  candidate shows the movement lesson card with `CONTINUE` while the live arena,
  results surface and touch controls remain visible; because results were already
  shown, this is not standalone attribution of the unlock.
- Gesture command sequence: `DOWN 900 2030`, `MOVE 1000 2030`, then repeated
  `MOVE 1000 1950`, followed by `UP 1000 1950`; coordinates are the 1080x2460
  approved Lava display and the stick was visibly at the right-hand MOVE position.
- Local diagnostic-only follow-up (not a release build): APK
  `Builds/M11/Android/BattleRaja-M11.apk`, SHA-256
  `E00EE17C87371565F4EC42B3008D47127A2A1D198F6D8D8C753DBE51365D2849`; fresh
  movement screenshot `Builds/Local/Device/Screenshots/diagnostic-tutorial-touch-down-mid.png`,
  SHA-256 `1D2EE2C26DBFAE1EAD0F6B70D728238866676635A4E290CACF1E57095FBFBA61`; the
  paired log `diagnostic-touch-down-logcat.txt`, SHA-256
  `4B9C5F4A7E7B2D8F646241766E85785B38CD8FC129FC1EE864B440E08E4DBC5B`, records
  nonzero movement input and post-`Move` displacement. The screenshot reached
  results during the longer probe, so it remains diagnostic rather than an approval.

#### P24 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Physical tutorial movement input on exact candidate | **Partially passed / attribution limited** | Touch knob and input delivery are evidenced; the exact-candidate `CONTINUE` screenshot was captured after results, and the diagnostic build is non-release, so repeatable alive-state lesson attribution remains open |
| Full action-by-action tutorial and end-to-end touch route | **Blocked / partially evidenced** | Remaining aim, attack, ability, gadget, Aandhi, elimination, victory, replay, settings, lifecycle and comfort review still require owner-operated Lava validation |
| All other P23 gates | **Unchanged** | Sustained performance normalization, genuine 16 KB runtime, authored/accessibility/cultural review, signing and Play/legal gates remain open |

P24 removes the earlier uncertainty that a virtual stick could not be reached by a
physical gesture. It does not convert a results-state screenshot or a temporary
diagnostic build into proof of repeatable alive-state progression, a complete
action-gated lesson, or a full release QA pass.

### P25 - Clean-worktree Android compliance rerun - 2026-08-27

The composed technical release checker was rerun at clean documentation commit
`3f1c112` against the existing exact-source APK/AAB pair. It passed repository,
manifest, ARM64/16 KB static bundle, and store-creative technical gates with **0
errors / 0 warnings**. This is a documentation-only recheck; it does not change the
runtime artifact or close the physical tutorial, performance, signing, privacy,
cultural or Play Console gates.

Evidence:

- Checker log: `Builds/Local/Device/release-checker-3f1c112.log`, SHA-256
  `62D0E7DF8541FD01ACAB9BC17BACE65B6A04814832ABD4CADE52469317D4DB89`.
- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 40,523,706 bytes,
  SHA-256 `365ABF4A1D37BB6DC2CE7E08F5E2741AAB7662EFB9749F0B4987EBFCBDB68BDB`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 36,348,870 bytes,
  SHA-256 `F1CB13C80A6408B344B5C71BE11D0AD804E58CA1D01102FE0B79D5B0712BDBA1`.
- Manifest: package `com.example.battleraja.m11`, version `1.0.0` / code `100`,
  min/target SDK `28/36`, VIBRATE plus Unity dynamic receiver only; network
  permissions absent.
- Bundle: 7 ARM64 native libraries, no other ABIs, all checked ELF loads aligned
  to `0x4000`; store icon `512x512` and feature graphic `1024x500`.

#### P25 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Repository, manifest, static bundle and technical creative checks | **Passed** | Clean-worktree checker rerun at `3f1c112`; 0 errors / 0 warnings |
| Runtime 16 KB behavior and Play eligibility | **Still open** | Device reports 4 KB pages; runtime proof, release signing, package identity, privacy/Data Safety and Play Console actions remain owner-controlled |
| All other P24 gates | **Unchanged** | See P24 classification above |

### P26 - Current-HEAD full Unity suite rerun - 2026-08-27

Full EditMode and PlayMode suites were rerun from clean commit `e241c48` using the
approved Unity `6000.5.6f1` test wrapper. Both reports completed with no failures or
skips. The run validates the current documentation-only continuation state; it does
not claim physical touch, performance, authored-content, signing or Play completion.

Evidence:

- EditMode: **140/140 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/e241c48-editmode.xml`, SHA-256
  `CCDAD4DF1FDC8B7B5B4441ADC07284FF2F2E578093E4A77276C447C9BBB4EE53`; log
  `Builds/Local/V1GameplayTruth/Logs/e241c48-editmode.log`, SHA-256
  `45235484F730E9EC36E5F84AD8DA3A251413E78FE208F20A1874F916B9885980`.
- PlayMode: **83/83 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/e241c48-playmode.xml`, SHA-256
  `61B93EEB8D69861C2431260124242C839ECCFFB2295302A4B128713639B9D1D4`; log
  `Builds/Local/V1GameplayTruth/Logs/e241c48-playmode.log`, SHA-256
  `7CED87E0B9A5934D7FF622734008F684788AA82803E7678C1A6CFC2F2AB44E0A`.
- Commands: `Tools/Validation/run_unity_tests.ps1 -TestPlatform editmode` and
  `Tools/Validation/run_unity_tests.ps1 -TestPlatform playmode`, each with explicit
  current-commit result/log paths and Unity `6000.5.6f1`.

#### P26 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Full EditMode regression suite at current HEAD | **Passed** | 140/140, zero failed/skipped; hashes above |
| Full PlayMode regression suite at current HEAD | **Passed** | 83/83, zero failed/skipped; hashes above |
| Physical release QA and non-test gates | **Unchanged** | Touch/tutorial attribution, sustained performance, runtime 16 KB, authored/cultural review, signing and Play actions remain open |

### P27 - Virtual-stick pointer delivery regression and exact-source suite - 2026-08-27

Commit `7269b4c` adds a PlayMode regression test that drives the production
`VirtualStick` pointer handlers (`OnPointerDown`, `OnDrag`, and `OnPointerUp`) and
asserts that `PlayerInputAdapter.ReadInput()` receives and then clears the movement
vector. The change is test-only; it does not alter the runtime candidate APK/AAB or
its release-compliance evidence. The focused test and both complete Unity suites were
rerun from the clean commit with Unity `6000.5.6f1`.

Evidence:

- Focused pointer-delivery test: **1/1 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/7269b4c-touch-pointer.xml`, SHA-256
  `4B13A8C148E98A4FD030F219F40CC7A42F4CCA2E8BCB679116632597DC26C58A`; log
  `Builds/Local/V1GameplayTruth/Logs/7269b4c-touch-pointer.log`, SHA-256
  `98FDDC489BF8AEC3A3C802C32236DAA0899EED156FC9766A80777D5C3AB59BDD`.
- Full EditMode: **140/140 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/7269b4c-editmode.xml`, SHA-256
  `E1B5E4983A606DF25525F9F185504EED73B3EF0E296456997EDF1E47CDE0C150`; log
  `Builds/Local/V1GameplayTruth/Logs/7269b4c-editmode.log`, SHA-256
  `382FE05ED94B211EE0888D55AA42C0555E15A026B27604AFF614EF11FA8F2934`.
- Full PlayMode: **84/84 passed**, XML
  `Builds/Local/V1GameplayTruth/TestResults/7269b4c-playmode.xml`, SHA-256
  `C3EA6F72AAF3EE385D1821661B6BCF909A505E483D819C556A725D92F4B6C4A6`; log
  `Builds/Local/V1GameplayTruth/Logs/7269b4c-playmode.log`, SHA-256
  `0CD9302636D2F3036DBF8E5E7B507B9B51FE46DA9EBDD4E2583FAA41C95CAC04`.

#### P27 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Production virtual-stick pointer delivery | **Passed (automated regression)** | Focused 1/1 test verifies nonzero movement reaches `PlayerInputAdapter` and releases to zero |
| Exact-source Unity regression suites | **Passed** | Commit `7269b4c`; EditMode 140/140 and PlayMode 84/84 with zero failed/skipped |
| Physical tutorial progression and complete action route | **Still open** | Automated delivery does not replace alive-state Lava tutorial/action-by-action verification |
| All other P26 gates | **Unchanged** | Sustained performance normalization, genuine 16 KB runtime, authored/accessibility/cultural review, signing, privacy/Data Safety and Play actions remain open |

P27 closes the code-level pointer-to-adapter regression risk while preserving the
truthful limitation on physical end-to-end tutorial attribution. The current runtime
candidate remains the exact `2080383` APK/AAB pair documented in P22/P25; a new release
build is not warranted for this test-only assembly change.

### P28 - Clean Android compliance rerun after touch-regression checkpoint - 2026-08-27

The composed technical release checker was rerun from clean commit `3f3a7ca` against
the unchanged exact-source APK/AAB pair. It again passed repository, manifest,
ARM64/16 KB static bundle and technical store-creative checks with **0 errors / 0
warnings**. This is a local technical recheck only; it does not close runtime 16 KB,
release signing, package identity, privacy/Data Safety, cultural review or Play Console
actions.

Evidence:

- Checker log: `Builds/Local/Device/release-checker-3f3a7ca.log`, SHA-256
  `62D0E7DF8541FD01ACAB9BC17BACE65B6A04814832ABD4CADE52469317D4DB89`.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 40,523,706 bytes,
  SHA-256 `365ABF4A1D37BB6DC2CE7E08F5E2741AAB7662EFB9749F0B4987EBFCBDB68BDB`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 36,348,870 bytes,
  SHA-256 `F1CB13C80A6408B344B5C71BE11D0AD804E58CA1D01102FE0B79D5B0712BDBA1`.
- Manifest remains package `com.example.battleraja.m11`, version `1.0.0` / code `100`,
  min/target SDK `28/36`, VIBRATE plus Unity dynamic receiver only, with network
  permissions absent. The AAB contains seven ARM64 native libraries and no other
  ABIs; all checked ELF loads are `0x4000` aligned. Store icon and feature graphic
  remain `512x512` and `1024x500`.

#### P28 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Repository, manifest, static bundle and technical creative checks | **Passed** | Clean checker at `3f3a7ca`; 0 errors / 0 warnings |
| Runtime 16 KB behavior and Play eligibility | **Still open** | Approved Lava reports 4 KB pages; genuine 16 KB runtime and owner signing/Play steps remain unavailable |
| All other P27 gates | **Unchanged** | Physical route, sustained performance, authored/accessibility/cultural review, privacy/Data Safety and Play actions remain open |

P28 confirms that the test-only touch coverage did not alter the offline package or
static Android compliance result.

### P29 - Controlled reference-game UX audit - 2026-08-27

An observation-only UX study was performed on approved Lava `ST5GDW23LB004392` for
the installed reference packages named in the milestone brief. No account, purchase,
network setup, extraction, recording, or protected-asset reuse was performed. The
result is principle-level research only and does not change BattleRaja's original
offline scope.

Evidence and redaction handling are documented in `Docs/Research/REFERENCE_UX_AUDIT.md`:

- Brawl Stars `com.supercell.brawlstars` version `68.279`: landscape home capture
  SHA-256 `AF5E761A163BEF0C11BBF8694B4FAD2D2DEDDCC87F19151A67D8E8DB2A581FE0` and UI
  dump SHA-256 `C6E29F0563753FAF3A3F0A27FAB5C7C448ED245C8578D89D99AF0B2E1D2BA05042`.
  The dominant play CTA, central focal preview, edge navigation and contextual
  coaching callout were observed; a short non-destructive play tap did not advance.
- Smash Karts `com.tallteam.citychase` version `2.15.1`: landscape home capture
  SHA-256 `69A7F8654D28B726F2BF6473BBF6710FAF9EBF3F2B75CBB07EDCEE0CDFA7271A`,
  post-tap capture SHA-256 `7421B3FCE40DFC75B4EFF9E8884E4A931D581EA7DB63AD5681DA335A0F48B921`,
  and UI dump SHA-256 `699FD353B1BF4CD64022AC7AA745C59D480389B5523F1DC668C3784A6954C11B`.
  A dominant play CTA, grouped secondary actions and visible locked/account state
  were observed; the short tap did not advance without account/network changes.
- Raw captures remain ignored under `Builds/Local/Device/ReferenceUx/20260827/` and
  are not store assets because the installed apps showed existing account/profile
  labels. The committed audit redacts those labels.

#### P29 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Controlled reference-game observation | **Passed (research capture)** | Both requested packages observed on Lava; abstract entry-flow principles recorded with hashes and redaction note |
| BattleRaja adaptation/originality boundary | **Passed (documented)** | Adaptation is limited to hierarchy/readability; no reference expression or online surface is copied |
| Full reference in-match comparison | **Not run** | Deeper route was not pursued through sign-in, purchases or network setup |
| BattleRaja release gates | **Unchanged** | Physical tutorial route, sustained performance, genuine 16 KB runtime, authored/accessibility/cultural review, signing, privacy/Data Safety and Play actions remain open |

P29 closes the previously missing controlled reference audit without authorizing any
out-of-scope reference-app interaction or weakening BattleRaja's originality and
offline requirements.

### P30 - Presentation-root movement fix and exact-source Lava tutorial transition - 2026-08-27

The prior physical tutorial probe showed valid touch input but an alive player that
appeared to stop moving. Investigation found that the legacy placeholder `MeshRenderer`
was on the same GameObject as the `CharacterController`; `FighterPresentation` was
animating that renderer by writing the movement root transform every frame. Commit
`126714a` now animates that renderer only when it is a child visual, leaving the
generated silhouette and the authoritative movement root independent. The Bijli
regression fixture was also moved to an open lane so its dash assertion measures the
ability rather than an intentional scene obstacle.

Evidence:

- Full EditMode: **140/140 passed**, XML
  `Builds/Local/TestResults/editmode-fighterpresentationfix.xml`, SHA-256
  `E262AF52D10AA87873B61D2AC08505D1BBBF1FD14EC213FF87A222C846DB3CFB`; log
  `Builds/Local/Logs/editmode-fighterpresentationfix.log`, SHA-256
  `ED796A1A426294405CF7C7A54BFDA9E738ECB8362D5A7BDA6DC0DF322189C1AB`.
- Full PlayMode: **84/84 passed**, XML
  `Builds/Local/TestResults/playmode-fighterpresentationfix2.xml`, SHA-256
  `33FE006CE2B2322DC2241784D9327B8016875B6BA4DE3773019358F38A992A1E`; log
  `Builds/Local/Logs/playmode-fighterpresentationfix2.log`, SHA-256
  `B92F6017BBDEAD1C20C993CC9713A0A3127AB48C508A18F30B885D07812B72C6`.
- Exact-source release-shaped APK from `126714a`: 40,526,074 bytes, SHA-256
  `A29EF1F2F28A3EAB6820F905DC57196E5496DF76A3DCFE32B65DB41BDCF26923`.
- Exact-source release-shaped AAB from `126714a`: 36,351,246 bytes, SHA-256
  `F3F901E7DBE382723B878E5B37EFBF58C9AB3D04FD7C744646C52FEF06B1A748`.
- Technical checker: `Builds/Local/Device/release-checker-126714a.log`, SHA-256
  `4E5553D92DCAEC181068F51E9A2511CD3854E09DCAAD3FD293AB768919AF8040`; clean
  worktree, package `com.example.battleraja.m11`, version `1.0.0` / code `100`,
  min/target SDK `28/36`, no network permissions, seven ARM64 libraries, all ELF
  loads aligned to `0x4000`, and store dimensions passed.
- The exact APK was installed only on approved Lava `ST5GDW23LB004392`. After a
  fresh app-data clear, the initial tutorial card was captured in
  `Builds/Local/Device/Screenshots/20260827-126714a-release/tutorial-waiting.png`,
  SHA-256 `D35275AF33476F1D2D6EA8413D269542E9ACD55D50F95421CB2E3D8BA00ABBDF`,
  showing `WAITING FOR ACTION` and the default left-stick prompt. A real
  `adb shell input swipe 180 2040 280 2040 900` on the left MOVE stick then
  produced `tutorial-movement-unlocked.png`, SHA-256
  `78A472CE73ECA6554E69E7C1D6ED5270B1CF829C085A527D933D34FB76604987`,
  showing the live arena, player and touch controls with `CONTINUE` enabled.
  The package/activity dump is SHA-256 `8C2EBFCE4ADB3A85408D4076DEC3322F1EE52AF38C6430561353F25E6D7C07D4` /
  `7302BC55F1200586BDCD1EE1F4D7FF945B5782A07B57807455D4B80FF4C1FCEF`; Lava
  reports 4096-byte pages in `page-size.txt`, SHA-256
  `30F236F92D107CEDC1EAB7B3D6DAFA316DF3657AC88E59ECE8DF2944B6C995CA`.

Exact-candidate 30-second Lava stability capture
`Builds/Local/Device/Performance/20260827-126714a-release-30s/` recorded six
samples with no configured fatal logcat markers. The manifest SHA-256 is
`BDB8406B803833D2430932B241BC3CACF344C806C3C35E9FF6F7EA8E713E692A` and the
logcat SHA-256 is `B1EF327ED9C18D773EB6E80B3D8B78FC6309EE4612BEC2368FC30839F130B6FF`.
After scene load, sampled PSS was 230,257–237,579 KB, RSS 365,884–373,316 KB,
graphics PSS 70,292 KB and thermal status 0; this is bounded stability evidence,
not a normalized FPS/jank/GPU/GC/battery approval.

#### P30 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Presentation cannot rewrite the movement root | **Passed** | Commit `126714a`; full PlayMode and exact Lava action-gated movement transition pass |
| Exact release APK technical checks | **Passed** | APK/AAB rebuilt from `126714a`; checker clean with static ARM64/16 KB alignment |
| Exact release APK tutorial movement transition | **Passed (bounded physical check)** | Fresh Lava card changed from `WAITING FOR ACTION` to `CONTINUE` after a real left-stick swipe while the arena remained visible |
| Full action-by-action tutorial and end-to-end touch route | **Still open** | Aim, attack, ability, gadget, Aandhi, elimination, victory, replay, settings, lifecycle and comfort review remain owner-operated QA |
| Runtime 16 KB behavior and Play eligibility | **Still open** | Lava is a 4 KB-page device; final identity/signing, privacy/Data Safety, cultural/legal review and Play Console actions remain owner-controlled |
| Sustained performance approval | **Still open** | Existing bounded diagnostics do not provide normalized full-match FPS/jank/GPU/GC/battery approval |

P30 closes the previously observed alive-state movement discrepancy on the exact
release-shaped source and gives direct physical attribution for the first tutorial
lesson. It does not claim the complete tutorial route or Play submission readiness.

### P31 - Exact-source production-bot release batch - 2026-08-27

The production-bot release assertions were rerun from the clean documentation tip
`90670ff` (runtime-bearing source unchanged from `126714a`) with Unity `6000.5.6f1`,
`BATTLERAJA_PRODUCTION_BOT_MATCHES=100`,
`BATTLERAJA_PRODUCTION_BOT_ASSERT_RELEASE_GATES=1` and the documented 50x fixed-tick
diagnostic playback. The run completed without changing any release threshold.

Evidence:

- NUnit PlayMode report `Builds/Local/TestResults/playmode-production-bot-126714a.xml`,
  **84/84 passed**, SHA-256
  `FC60930AF48D546D0858428E8431D6337505007CBD5946375BC6A5275A1D7612`.
- Unity log `Builds/Local/Logs/playmode-production-bot-126714a.log`, SHA-256
  `1A3C62FBA5436DA875770985D0542779627A781C4F45DBE69E70DBC3E8395F60`.
- Batch report
  `Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-035001860-9101.json`,
  SHA-256 `9714C50F4293CC7C6A191FFA1C4C50EDF22CABA05BCE12D88E0EBC30DC04EFB9`.
- All **100/100** matches reached terminal results within the 10,800-tick budget;
  duration was **306.014 s** for every match, so **100/100** were in the 240–360 s
  window. All **100/100** contained bot-to-bot damage and at least one combat
  elimination; Aandhi-only resolutions were **0/100**.
- Protected-warmup damage events and invalid-position samples were both **0**;
  maximum continuous stuck duration was **0 ticks**. Attack telemetry recorded
  15,323 attempts with 6 out-of-range attempts; ability telemetry recorded 35,739
  attempts and 6,886 rejections; successful gadgets were **299** total, including
  Umbrella Guard **99**, Dhol Burst **100**, and Tiffin Station **100**.

#### P31 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Exact-source 100-match production-bot terminal completion | **Passed** | 100/100 terminal results; 0 over the 360 s ceiling |
| Exact-source 240–360 s pacing distribution | **Passed (automated)** | 100/100 in-window; original 90% target and calibrated 80% gate both pass |
| Bot-to-bot combat and combat-elimination distribution | **Passed (automated)** | 100/100 with bot-to-bot damage and combat elimination; 0 Aandhi-only |
| Warmup, position and stuck invariants | **Passed (automated)** | 0 protected damage, 0 invalid positions, 0 continuous stuck ticks |
| Fighter/gadget coverage | **Passed (automated)** | Bijli, Pehel and Maya present; each gadget kind used successfully |
| Human fun/fairness and full Lava route | **Still open** | Automated telemetry cannot replace touch comfort, accessibility, authored presentation, balance, thermal or desire-to-rematch review |

### P32 - Exact-release physical tutorial action-gate follow-up - 2026-08-27

The exact release-shaped APK from `126714a` was freshly data-cleared and launched on
approved Lava `ST5GDW23LB004392`. The APK SHA-256 is
`A29EF1F2F28A3EAB6820F905DC57196E5496DF76A3DCFE32B65DB41BDCF26923`; the matching AAB
SHA-256 is `F3F901E7DBE382723B878E5B37EFBF58C9AB3D04FD7C744646C52FEF06B1A748`. A single short ADB sequence performed real touch input
through the first six tutorial lessons. The screenshots are retained under
`Builds/Local/Device/Screenshots/20260827-126714a-release/fresh-action-route/` and are
not presented as store assets.

Evidence (SHA-256):

- Movement waiting/unlocked: `step1-waiting.png` `DC3F5F06BE79B0F9028C7456D0250BF6A793C7274D95B3CEFC1EC440427F1609`;
  `step1-unlocked.png` `A0B021F87EB7402BA2908F8CF6EDE5009EED2C80C431A4BAA9D71BDC6CD46586`.
- Aim waiting/unlocked: `step2-waiting.png` `3AB49CFECFF00A1C96D1F8B8B4FD2B5D4C1312FBC4378F0170D544B82F779D2B`;
  `step2-unlocked.png` `F12471B6CB7C656414003A9BDB5F69BA3F052C7BC297EAC1D3695F1083F0E263`.
- Basic attack waiting/unlocked: `step3-waiting.png` `C9AB2A665E4D67EF0380D6422C02A977065A4BDDA75934819E8D4D66E14D9824`;
  `step3-unlocked.png` `961441FFFAFEC862DC7E03FE237D825FDD9A7D838A44EDBA17F3BAD8AE5090CA`.
- Ability waiting/unlocked: `step4-waiting.png` `69B926FF0EDB3E521B032907B0431863F20D5E4017C1E28AF8B5F8A7E96EF916`;
  `step4-after-ability.png` `3F6725924981A6A793A40BAFDB26C3EFE7F4BDEF8FD30BB11B94AC8740B86525`.
- Gadget waiting and post-use: `step5-gadget-waiting.png` `EA2D9592294B2C72C773FE6C27F6005753C7389D99A55CBF3A7AE7922E9CCAE2`;
  `step5-after-gadget-tap.png` `215127004E0F8217852D1C007F90E656E63F757B0B05DADB53C70ADC59A1E47E`.
- Aandhi action-gate unlocked: `step6-aandhi.png` `6321154EAC4B50F165FAEC42F1BC2ED185D9EAE3077FD76A8EDACAF482B315EF`.
- Elimination remains correctly waiting after a bounded attack probe: `step7-elimination-waiting.png`
  `6F542814B7E4C2A0FA9C4E1E67F06FE96FC751DEB58A84BD9D8FDD44947FECEE`;
  `step7-attack-left-hold.png` `058478E120E6FD14918D80A49E7CE8E9BFB3BF8928D93FB3F3485A1C496E607E`.

The fresh screenshots show the prompt changing from `WAITING FOR ACTION` to `CONTINUE`
after genuine movement, aim, attack, ability, gadget and Aandhi observations. The player
was still alive at 85/85 HP in the Ability and Gadget states; later attack probing reduced
HP without producing a player-attributed KO. The follow-up result capture
`step7-followup-ko.png` (`3ACFEF83A05BCAA373210BF6907FF4961227776C51798B0605183588BE6D9190`)
shows the player at 0/85 with `YOU KO 0`, confirming the tutorial remained gated. No
elimination, victory, full match, rematch, accessibility or comfort pass is claimed from
this run.

#### P32 gate delta

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Exact physical Movement → Aim → Basic Attack → Ability transitions | **Passed (bounded physical check)** | Fresh exact APK route produced unlocked `CONTINUE` states after real touch actions |
| Exact physical Gadget pickup/use transition | **Passed (bounded physical check)** | Player collected and used a Dhol pickup; `CONTINUE` enabled and HUD showed Dhol readiness |
| Exact physical Aandhi observation transition | **Passed (bounded physical check)** | Aandhi HUD/ring state was observed and `CONTINUE` enabled |
| Exact physical Elimination → Victory transitions | **Still open** | Player-attributed KO and final victory were not achieved in this bounded probe |
| Full route, accessibility, comfort and repeated-match review | **Still open** | Requires owner-operated Lava QA across fighters, settings, lifecycle and rematches |

### P33 - Exact-candidate technical release recheck - 2026-08-27

With the documentation commit `604887b` clean and the runtime-bearing source unchanged at
`126714a`, the release checker was rerun against the exact APK/AAB pair. The captured log is
`Builds/Local/Device/release-checker-604887b.log` (SHA-256
`DDD201C1E5BBE713405F9F41AADBEA8A5E5DFE7A875B2F94C4E486706C153F22`). Repository validation,
offline manifest permissions, API 28/36, ARM64-only bundle contents, seven native-library
static 16 KB ELF alignments, and store-creative dimensions all passed. This is a technical
recheck only; the APK remains temporary-ID/debug-signed, Lava reports 4 KB pages, and final
identity, signing, privacy/Data Safety, cultural/legal review and Play Console actions remain
open.

| Gate | Status | Evidence / owner action |
| --- | --- | --- |
| Exact APK/AAB technical release checks | **Passed** | `release-checker-604887b.log`; APK `A29EF1F2F28A3EAB6820F905DC57196E5496DF76A3DCFE32B65DB41BDCF26923`; AAB `F3F901E7DBE382723B878E5B37EFBF58C9AB3D04FD7C744646C52FEF06B1A748` |
| Runtime 16 KB and final Play eligibility | **Still open** | Approved Lava is 4 KB-page; final package identity/signing/privacy/legal and Play steps require owner-controlled work |

### P34 - Live-authority tutorial elimination fix and exact-candidate rebuild - 2026-08-27

The tutorial elimination lesson had a real progression defect: it only inspected terminal
results, so a player KO credited during a still-live match could not unlock the lesson.
Commit `f82c18c1fd91e44c7f07fbd31d615cc7e9c9bea6` now baselines the player elimination
counter and observes the authoritative `CombatEntitySnapshot` as soon as a KO is credited.
Victory remains deliberately gated on terminal placement 1. The regression test proves the
live snapshot unlock before `ResultsShown`.

#### Automated evidence

- Static validation: **0 errors / 0 warnings** (also rechecked by the release checker).
- Full EditMode: **140/140 passed**; XML
  `Builds/Local/TestResults/editmode-tutorial-elimination-fix.xml`, SHA-256
  `AB8B5ACAFE3BCFDF112971896DD5DEC0E0C6812F08A031339C74F733B56B050F`; Unity log
  `Builds/Local/Logs/editmode-tutorial-elimination-fix.log`, SHA-256
  `1AF5440AEECAF4809667180DB4F555096FDF8660CBF129A7E853463E4F039DC6`.
- Full PlayMode: **85/85 passed**, including
  `EliminationLessonUnlocksFromLiveAuthoritativeSnapshotBeforeResults`; XML
  `Builds/Local/TestResults/playmode-tutorial-elimination-fix.xml`, SHA-256
  `C6D0F237DDEDBE54F02D70250C95C2263C87E1D631132BAE525104AD32504F4C`; Unity log
  `Builds/Local/Logs/playmode-tutorial-elimination-fix.log`, SHA-256
  `A28BF1AFF9C4C1E6CF31C50CAE18227EEA66CDA3AC6E2E5F2906067F5C31969E`.

#### Exact release artifacts

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,524,546 bytes**,
  SHA-256 `D4E965DE27E4C8D50F57038557E70D55190DFD0AECEEA8CB4E9B30A15A91B59A`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,349,707 bytes**,
  SHA-256 `3D1BD5D1E8DBFEACCBDFF97907EFF6CC14ECEB33CE80522EC94166ACB07E1ACF`.
- Unity build log `Builds/M11/Logs/android-build.log`, SHA-256
  `B935CA0D7C4F6B4D24D0C67D333E9C7BB956EC2AF1166CAE340FE2C2296C0DDE`.
- Release checker `Builds/Local/Device/release-checker-f82c18c.log`, SHA-256
  `B73B0A1CD12F11A2941C6F629A92128F1D738122AAC866BE275742EDFD2B36F5`: **0 errors / 0 warnings**;
  package `com.example.battleraja.m11`, version `1.0.0`/`100`, min/target API `28/36`,
  no network permissions, seven ARM64 libraries, static ELF loads aligned for 16 KB,
  icon `512x512`, feature graphic `1024x500`, clean worktree.

#### Exact-candidate Lava touch evidence

The APK was data-cleared, installed and launched only on approved Lava
`ST5GDW23LB004392`. Corrected touch coordinates drove the live card through Movement,
Aim, Basic Attack and Ability. Representative captures are retained under
`Builds/Local/Device/Screenshots/20260827-f82c18c-release/tutorial-live-elimination/`:

- `restart-step1-unlocked.png` — SHA-256
  `52464268D0B27814825C9A891B6589C24AC6EC2A4463402E848D58EA88C46D83`.
- `restart-step2-unlocked.png` — SHA-256
  `BDA9EA5F36CEF46342842FC4B63FAE4668350C7A245572CCC742EDE80D4589AF`.
- `restart-step3-unlocked.png` — SHA-256
  `C887EF03DF6EEA7968ECDF8EF65EE410103D8FF998FFDDDFEFB442EBB73E3930`.
- `restart-step4-ability-swipe.png` — SHA-256
  `E893EE07C932F7BE7249B74761DEB82797321C3D812C67ED7C08718B3061E2F1`.

The same candidate reached the Gadget card and showed Tiffin pickup/proximity feedback
after a real movement route; `gadget-after-use-deliberate.png` (SHA-256
`2E482FF998EDCE322816E58870785CEA60F92E47067C0AC70556894C2717F779`) records that
attempt, but the card was still waiting when the match later reached terminal results.
Authoritative collection/use was not independently proven in this probe, so this is
route-attempt evidence rather than a physical Gadget, Elimination or Victory pass. The
authoritative PlayMode regression is the source-level proof for live Elimination
unlocking. Full physical Elimination → Victory, replay, accessibility, comfort and owner
approval remain open.

#### P34 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Live Elimination lesson unlocks from an in-match KO | **Passed in authoritative regression** | 85/85 PlayMode; target is defeated while `ResultsShown == false`, then overlay unlocks from the snapshot |
| Exact artifact pair matches the fixed source | **Passed** | APK/AAB rebuilt from `f82c18c`; checker clean |
| Physical Movement/Aim/Basic Attack/Ability touch transitions | **Observed** | Exact Lava captures above; gesture timing and card visibility still need owner comfort review |
| Physical Gadget → Aandhi → Elimination → Victory route | **Still open** | Candidate reached Tiffin pickup/proximity feedback, but authoritative collection/use and the downstream card transitions were not proven before terminal results |

P34 fixes a concrete tutorial correctness issue and refreshes the exact release artifacts.
It does not change the overall classification: the product remains an offline prototype
candidate, not a Play-ready release.

### P35 - Current-source production-bot batch refresh - 2026-08-27

The production-bot release harness was rerun from the clean documentation tip
`68b0551e44b6356ca3f8a8925ff4268a6bc7380d` (runtime-bearing source remains
`f82c18c1fd91e44c7f07fbd31d615cc7e9c9bea6`). Unity `6000.5.6f1` ran the configured
100 seeded Bazaar Bastion matches at 50x fixed-tick playback. The batch report is
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-052416875-9101.json`
(1,794,454 bytes, SHA-256
`78953105EED4CD3FEF3E4FAC771AC2B85563DBEB9AC052CE93470D04D81FB10A`). The NUnit
report is `Builds/Local/TestResults/playmode-production-bot-f82c18c.xml` (SHA-256
`0D4EDECEF73D34719265AC273D9D177AB20EB4EF5529E98BD7979C8B590DF0C6`) and the Unity
log is `Builds/Local/Logs/playmode-production-bot-f82c18c.log` (SHA-256
`43E88FA06BA3C148A5CC5FB1E980848C552D0447736E2DBD3E2C8DCCD5ADA204`). The full
PlayMode run was **85/85 passed**.

The same clean tip was rechecked against the matching APK/AAB with
`Builds/Local/Device/release-checker-b090bd9.log` (SHA-256
`B73B0A1CD12F11A2941C6F629A92128F1D738122AAC866BE275742EDFD2B36F5`): **0 errors / 0
warnings**, clean worktree, offline permissions, API 28/36, ARM64 and static 16 KB
alignment, and store-creative dimensions all passed.

#### Batch metrics

- **100/100** matches completed within the harness tick budget and **100/100** were
  in the 240-360 second target window (each recorded at 306.014 seconds).
- **100/100** matches recorded at least one combat elimination; **94/100** recorded
  at least three combat eliminations. Aandhi-only resolution was **0/100**.
- Bot-to-bot damage occurred in **100/100** matches (7,536 damage events across
  3,123 unique damaging pairs). Protected-warmup damage and invalid positions were
  both zero; maximum continuous stuck ticks and stuck recoveries were zero.
- All three fighters appeared across the batch: Bijli, Pehel and Maya. Gadget
  coverage recorded 300 pickups and 299 successful uses: Umbrella Guard 99, Dhol
  Burst 100 and Tiffin Station 100. There were 274 contextual failed-use attempts;
  these are expected authority rejections, not invariant failures.
- Attack authority rejected **0** attacks; six out-of-range attempts were observed
  and rejected by range rules. The harness test and report contain no failed cases.

This refresh strengthens the exact-source bot evidence but does not close physical
touch, accessibility, sustained-performance, genuine-runtime-16-KB, authored-content,
signing, identity, privacy/legal, cultural or Play Console gates. No same-seed replay
rerun was generated by this batch; the separately recorded deterministic replay soak
remains the applicable replay evidence.

#### P35 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Current-source 100-match production batch | **Passed** | 100/100 terminal and in-window; report and NUnit/log hashes above |
| Bot-to-bot damage and safety invariants | **Passed** | 100/100 bot-to-bot damage; zero protected, invalid, stuck-recovery and max-stuck samples |
| Fighter and gadget batch coverage | **Passed** | Bijli/Pehel/Maya plus all three gadget kinds recorded |
| Same-seed replay reproduction in this batch | **Not run** | Existing 1,000-seed deterministic replay soak remains applicable; no duplicate batch requested |
| Physical full route and Play eligibility | **Blocked** | Exact Lava Gadget/Aandhi/Elimination/Victory and sustained-performance runs require the remaining approved-device review; 16 KB runtime, signing and Play/legal gates require unavailable owner-controlled environments/approval |

### P36 - Gadget reconciliation, exact candidate refresh and bounded Lava route - 2026-08-27

Commit `754837e4311b609560c63fa90558a1d29acec9cd` adds a presentation-only tutorial
reconciliation for gadget state. If the tutorial's nearby Tiffin is collected or used
before the Gadget card becomes active, the overlay now consumes the authoritative
inventory/use counters when that lesson begins (and on the first bound frame), while
the existing live-authority Elimination fix remains intact. This prevents a false
`WAITING FOR ACTION` state without mutating gameplay authority.

#### Automated evidence

- Full EditMode: **140/140 passed**; XML
  `Builds/Local/TestResults/editmode-gadget-reconcile-v2.xml`, SHA-256
  `A5E9398085902C1C79AE73D84448A2819C18818FAB0E96A1FAFA8BD858186440`; Unity log
  `Builds/Local/Logs/editmode-gadget-reconcile-v2.log`, SHA-256
  `215D932D454E93F2DCBE709D81AC04E5895DF42D66ACE0CFABC6774D3F1A2F66`.
- Full PlayMode: **86/86 passed**; XML
  `Builds/Local/TestResults/playmode-gadget-reconcile-v2.xml`, SHA-256
  `82E2A3291B82DAB50C289F899CC1637E4C3668FF45F69523C5A382F92D0B9177`; Unity log
  `Builds/Local/Logs/playmode-gadget-reconcile-v2.log`, SHA-256
  `10EE0EFAF334D5B7DF3058E1CB647B11340F927339233356B6B04C0659DAFEC6`.
- Static validation and the exact release checker both report **0 errors / 0 warnings**.
  Checker log `Builds/Local/Device/release-checker-754837e.log`, SHA-256
  `E6EF2EB9DDEEDD63981B0C894A2778D163988239E2BF7176786E8DB63CA4F721`.
- After this evidence was documented, the same exact APK/AAB pair was rechecked from
  post-P36 clean documentation tip `a877c509fdbec485e808039a6c4daa03fed9ea9c` using
  `Builds/Local/Device/release-checker-a877c50.log` (SHA-256
  `E6EF2EB9DDEEDD63981B0C894A2778D163988239E2BF7176786E8DB63CA4F721`): **0 errors /
  0 warnings**, clean worktree.

#### Exact candidate artifacts

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`: **40,527,614 bytes**,
  SHA-256 `788181073E5EFCB2F5F0AECEF20E0372362BFCD2B83928CA010153009FDF99B3`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`: **36,352,792 bytes**,
  SHA-256 `FCFF4A982BC5201D204114B819C0BDAE42CA35072425CE9506349769815D98C3`.
- Unity Android build log `Builds/M11/Logs/android-build.log`, SHA-256
  `D06AFFF88A0ECC29957E8B7FFAF1DD3B6A78F51F32248239C68A932D96805715`.
- The checker confirms package `com.example.battleraja.m11`, version `1.0.0`/`100`,
  min/target API `28/36`, no network permissions, ARM64-only native payload,
  static 16 KB ELF alignment, and the expected 512x512 icon / 1024x500 feature graphic.
  It is still a temporary-ID Android Debug-signed local artifact.

#### Approved-Lava touch evidence

The exact APK was installed after clearing package data and launched only on approved
Lava `ST5GDW23LB004392`. The bounded tutorial route produced action-attributed
Movement, Aim, Basic Attack and Ability transitions, then physically tapped the Gadget
button and advanced the Gadget card to `CONTINUE`. After continuing, the Aandhi card
also showed `CONTINUE`; the next Elimination card correctly remained `WAITING FOR ACTION`
until a player-attributed KO, which was not achieved in this probe. Representative
captures are retained under
`Builds/Local/Device/Screenshots/20260827-754837e-release/tutorial-gadget-reconcile/minimal-route/`:

- `gadget-tap.png` — SHA-256
  `03CDE7D729040B4B39298BDA78001E86907B83B0FCA74F0495B2A580BA4EFCF8` (Gadget card
  advanced to `CONTINUE`).
- `aandhi-step.png` — SHA-256
  `2D8A3C245AC5BDDFA0D6B2062125B71A9390EB64CE035679108FA306F02BA805` (Aandhi card
  showed `CONTINUE`).
- `after-aandhi-continue.png` — SHA-256
  `6EE4FC5D3F4FEB559AF250A25454C0E6771EF74419E3ACACDD60ABB599674C83` (Elimination
  card correctly waiting for an in-match KO).

This is a bounded physical route observation, not approval of the complete match,
accessibility, comfort, sustained performance or rematch matrix. The presentation-only
source change did not alter gameplay authority; the exact-runtime P38 batch now
supersedes P35 for 100-match gameplay evidence.

#### P36 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Gadget lesson reconciliation for pre-collected state | **Passed** | `PreCollectedGadgetIsReconciledWhenGadgetLessonBegins`; PlayMode 86/86 |
| Exact APK/AAB technical checks from current source | **Passed** | APK/AAB hashes and checker log above; 0 errors / 0 warnings |
| Physical Gadget lesson transition | **Passed** | Bounded physical evidence: `gadget-tap.png` shows the Gadget card at `CONTINUE` after a real tap |
| Physical Aandhi lesson transition | **Passed** | Bounded physical evidence: `aandhi-step.png` shows the Aandhi card at `CONTINUE` |
| Physical Elimination → Victory, full match and rematch | **Blocked** | No player-attributed KO/Victory in this probe; owner-operated full route remains required |
| Final Play eligibility | **Blocked** | Genuine runtime 16 KB, signing/identity, accessibility, performance, privacy/legal/cultural review and Play Console actions remain owner-controlled or unavailable |

### P37 - Exact-candidate bounded Lava performance diagnostic - 2026-08-27

The exact candidate APK was launched on approved Lava `ST5GDW23LB004392` with the
repository capture script for 30 seconds at 5-second intervals. Output is retained at
`Builds/Local/Device/Performance/20260827-486c76b-candidate-30s/`. The manifest records
six samples and no configured `FATAL EXCEPTION`, ANR, SIGSEGV, SIGABRT,
`NullReferenceException` or `UnityException` markers. Manifest SHA-256 is
`EAE93CBA70253A43E288A7FF080DF90333A0E2E3F71DA5AFA1D5E75CCB3E8D6`; captured logcat
SHA-256 is `D50CDECC14808AB64EB6980B50C9290E121596F3F24BE9977AFAA58258A3FEAE`.

Thermal status was 0 in every sample and before/after captures. PSS was 58,737 KB in
the startup sample and stabilized at 230,576-240,186 KB across the remaining samples;
RSS stabilized at 346,788-356,404 KB. These figures are a short launch/idle diagnostic,
not a normalized dense-combat, final-circle, GC, GPU, battery, repeated-rematch or
mid-range-device performance approval. The device reports 4 KB pages, so this also does
not prove genuine runtime 16 KB compatibility.

#### P37 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Exact-candidate Lava launch diagnostic | **Passed** | Six samples over 30 seconds; no configured fatal markers |
| Thermal status during bounded capture | **Passed** | Thermal status 0 in all samples and before/after captures |
| Full-match performance against documented budgets | **Not run** | This capture did not cover dense combat, final circle, GC/GPU or repeated rematches |
| Sustained thermal, battery and mid-range-device approval | **Blocked** | Requires owner-operated gameplay sessions and broader device coverage |
| Genuine runtime 16 KB validation | **Blocked** | Approved Lava reports 4 KB pages; a genuine 16 KB environment is unavailable |

### P38 - Current-runtime exact 100-match production-bot batch - 2026-08-27

The production-bot release harness was rerun from the current runtime-bearing source
`754837e4311b609560c63fa90558a1d29acec9cd` (clean documentation tip at execution:
`c6dda8cb56e958265ac34d1bbd1ae0af2e654d21`). Unity `6000.5.6f1` ran 100 seeded
Bazaar Bastion matches at 50x fixed-tick playback through the production scene,
perception and bot decisions. The batch report is
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-140715586-9101.json`
(1,794,696 bytes, SHA-256
`5E45143047CC363927D5E1EFEDDA798908A200B5F602DB80D010DBC84FA50355`). The NUnit
report is `Builds/Local/TestResults/playmode-production-bot-754837e.xml` (76,144 bytes,
SHA-256 `9FEDD7ACEBC14521442C245A8D847D8EAEB0B2ABAC98FAA0FFD8D3BBD15FE6E9`) and the
Unity log is `Builds/Local/Logs/playmode-production-bot-754837e.log` (165,970 bytes,
SHA-256 `547E05FC446377EBA9137EEF4DF596A1566CF37BF7BC109F579E499705910F43`). The
full PlayMode run was **86/86 passed**.

#### Batch metrics

- **100/100** matches completed within the tick budget and **100/100** were in the
  240-360 second target window (all recorded at 306.0135 seconds).
- **100/100** recorded at least one combat elimination; **94/100** recorded at least
  three. Aandhi-only resolution was **0/100**.
- Bot-to-bot damage occurred in **100/100** matches (7,536 damage events across 3,123
  damaging pairs). Protected-warmup damage and invalid positions were zero; maximum
  continuous stuck ticks and stuck recoveries were zero.
- All three fighters appeared: Bijli, Pehel and Maya. Gadget coverage recorded 300
  pickups and 299 successful uses: Umbrella Guard 99, Dhol Burst 100 and Tiffin
  Station 100. Contextual failed gadget uses remain authority-rejected attempts.
- Attack authority rejected **0** attacks; six out-of-range attempts were rejected by
  range rules. The maximum sampled decision time was 0.1459 ms.

This is the exact current runtime batch and supersedes the prior P35 batch for gameplay
truth. It still does not establish human fairness/fun, physical full-route behavior,
sustained performance, genuine runtime 16 KB, authored-content quality, signing,
identity, privacy/legal, cultural review or Play eligibility.

#### P38 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Current-runtime 100-match production batch | **Passed** | 100/100 terminal and in-window; report and NUnit/log hashes above |
| Bot-to-bot combat and safety invariants | **Passed** | 100/100 damaging matches; zero protected, invalid-position, stuck-recovery and max-stuck samples |
| Fighter and gadget coverage | **Passed** | Bijli/Pehel/Maya and all three gadget kinds used successfully |
| Match pacing and combat-elimination thresholds | **Passed** | 100/100 with at least one combat KO; 94/100 with at least three; 306.0135-second duration |
| Same-seed replay reproduction in this batch | **Not run** | Separate deterministic replay soak remains applicable; no duplicate bot batch requested |
| Human fun/fairness and sustained performance | **Not run** | Requires structured owner-operated Lava playtests and budget analysis |

### P39 - Genuine 16 KB Android emulator runtime check - 2026-08-27

The installed Android SDK includes the Android 16 Google Play `page_size_16kb`
system image. A disposable `BattleRaja_16K` AVD was created from the already-installed
`system-images;android-36;google_apis_playstore_ps16k;x86_64` image and booted as
`emulator-5558`. The exact candidate APK installed successfully and reported
`getconf PAGE_SIZE = 16384`. The Unity activity
`com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity` was top-resumed,
the menu rendered, the tutorial opened, and a real movement swipe unlocked `CONTINUE`.
No command was sent to the prohibited Oppo device.

Runtime evidence is retained at
`Builds/Local/Device/Performance/20260827-16k-emulator-30s/` and
`Builds/Local/Device/Screenshots/20260827-16k-emulator/`:

- `page-size.txt` is `16384` (SHA-256
  `CA902D4A8ACBDEA132ADA81A004081F51C5C9279D409CEE414DE5A39A139FAB6`).
- The 30-second capture manifest records six samples and no configured fatal markers;
  SHA-256 `9E397EAF00A093FF6CA6605DA6167FCF04AB7C174EBF643AD4C97B9CF706760C`.
- Activity evidence SHA-256 is
  `655F2EC1679E2594A96A60D973F859C47D684A9DC3A9B0BFFD63326D6DE81A2C` and logcat
  SHA-256 is `9D0094124EE1F93EA23F34F02372CF0D9189D8B09D7716E404ECF8BD20A52B56`.
  A post-tutorial logcat scan also found no configured fatal markers; its retained
  capture is `Builds/Local/Device/Performance/20260827-16k-emulator-30s/post-tutorial-logcat.txt`
  (SHA-256
  `EAEA58B80F49E562D272627085B1E7FB6314B4A6F4153C70F50A86D456857988`).
- `menu.png` SHA-256
  `61CCE91FE52719788C9895C5161DB2C1BE70CCAAA4CE6A900C8608E98CE3642A` and
  `tutorial-movement.png` SHA-256
  `00B7902A211D455857D108FFA9BEACCADC5BB39A001C6D509AFC263CA80DA15A` show the
  actual candidate in the 16 KB environment; the latter shows the Movement card at
  `CONTINUE` after a real swipe.

The emulator is x86_64 rather than a physical ARM64 handset, and the APK remains
temporary-ID Debug-signed. This closes the available genuine 16 KB emulator runtime
check, but final signed-artifact verification and physical 16 KB-device coverage remain
required before any Play claim. The capture is also launch/tutorial evidence, not a
dense-combat performance approval.

#### P39 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Exact APK install on genuine 16 KB environment | **Passed** | `adb install` succeeded on `emulator-5558`; `PAGE_SIZE=16384` |
| Exact APK Unity activity launch on 16 KB environment | **Passed** | Top-resumed Unity activity; no configured fatal markers |
| Tutorial render and movement input on 16 KB environment | **Passed** | Actual menu/tutorial screenshots; movement swipe unlocked `CONTINUE` |
| Final signed ARM64 artifact on physical 16 KB device | **Blocked** | Current candidate is Debug-signed and no physical 16 KB ARM64 device is available |
| Dense-combat/repeated-rematch performance on 16 KB environment | **Not run** | 30-second emulator capture covered launch/tutorial only |

### P40 - Current-tip deterministic replay deep soak - 2026-08-27

The deterministic replay soak was rerun from clean documentation tip
`98888d3` with the runtime-bearing source unchanged at
`754837e4311b609560c63fa90558a1d29acec9cd`. The command used
`BATTLERAJA_SOAK_MATCHES=1000` and filtered
`BattleRaja.Tests.EditMode.DeterministicSoakTests.AcceleratedSeededMatchesReproduceIdenticalHashStreams`.
Unity `6000.5.6f1` completed **1/1** test with **1,000 seeded matches executed twice
(2,000 executions)**, zero divergence, and NUnit duration **536.0635271 seconds**.
XML evidence is `Builds/Local/TestResults/deep-soak-current-98888d3.xml` (SHA-256
`07DADE0702BD7B5DEC9A11E60042D66778A42344CBB33526D72073D6D8DFF4C6`); the Unity
log is `Builds/Local/Logs/deep-soak-current-98888d3.log` (SHA-256
`A2CC52C19961FFAAC139D68A2FF591683A5AC495F26C914A1638D101AA6D5C97`). The clean
worktree and `git diff --check` were confirmed after the run.

The exact APK/AAB pair was also rechecked from clean commit `4dca4af` with
`Tools/Validation/check_v1_release_candidate.ps1 -RequireCleanWorktree`: **0 errors / 0
warnings**, offline manifest and permission gate passed, seven ARM64 libraries passed
static 16 KB alignment, and store creative dimensions passed. The retained checker log
is `Builds/Local/Device/release-checker-4dca4af.log` (SHA-256
`E6EF2EB9DDEEDD63981B0C894A2778D163988239E2BF7176786E8DB63CA4F721`).

#### P40 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Current-tip deterministic replay/deep soak | **Passed** | 1,000 seeds x 2 executions, zero divergence; exact XML/log hashes above |
| Cross-machine floating-point parity | **Not run** | Same-machine deterministic evidence does not establish cross-device parity |
| Durable production replay-file serialization | **Not run** | This remains outside the current offline QA harness |

### P41 - Exact-candidate Lava full-loop and three-cycle bounded probe - 2026-08-27

The exact candidate APK (`788181073E5EFCB2F5F0AECEF20E0372362BFCD2B83928CA010153009FDF99B3`)
was exercised on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34),
with no command sent to the prohibited Oppo device. Real touch input reached the menu,
Solo Raja mode, fighter selection, live Bijli match, player defeat, spectator view,
settings, Aandhi Final Circle, Results, and Rematch. Two Results screens show complete
placement tables; a third Rematch cycle started and returned to the menu within the
bounded capture. Settings toggles for left-handed controls, reduced flashes, high
contrast, aim assist and text scaling were each exercised and then restored. Captures
and hashes are retained under `Builds/Local/Device/Screenshots/20260827-final-route/`;
representative files are `fighter-select.png` (`44C01B6F6B229A33B489E91AEFF3C905BCBC5D9252701ED2CB9E7433FF15D96D`),
`rematch-results.png` (`F445C37E043EE89BEEFA63670385A402BAAE214ABB04FCBC7F882229122FF0F0`),
`rematch-opening.png` (`80DF4D23E58B96B51CC0CB8633044150525C745AA6A24A5794DE49AF2EAC81E7`),
`settings-text-plus.png` (`EFD96AFE3663F786F44FB4811139921C5EDB9C4758D5A37AD6BB11FA015D5381`),
and `settings-restored.png` (`D6FA14580052BE7C29AD261E7169658CF32F2C57BCADBFA537D531A5CE429934`).

The repository performance capture ran for 180 seconds at 30-second intervals while
the third cycle was active. Manifest `Builds/Local/Device/Performance/20260827-final-route-180s/manifest.json`
has SHA-256 `7E8CF3D731B95815F4C1AA9347731A34BE749834CF7EE9153BCA5081818C1301`; captured
logcat has SHA-256 `7C4D26B55615D3AFC2BF3A891989F56D558A16F0FA59EAB84E2E50868793BBCB` and
no configured fatal markers. Thermal status was 0 in all six samples; battery remained
at 63% (USB-powered). PSS was 70,103 KB in the startup sample and 239,626-243,910 KB
after startup; RSS was 154,810 KB initially and 355,300-359,580 KB thereafter. This
is stronger physical route and bounded endurance evidence, not normalized frame-time,
GPU, GC, battery-drain, thermal-throttling or mid-range-device approval.

#### P41 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Exact Lava menu → Solo Raja → fighter selection → live match | **Passed** | Real touch route on exact candidate; screenshots retained |
| Player defeat → spectator → Aandhi Final Circle → Results | **Passed** | Two complete Results captures with placement tables |
| Rematch transition and three-cycle bounded observation | **Passed (bounded)** | Two Results screens and a fresh third-cycle opening; third capture returned to menu |
| Settings/accessibility toggle response and restoration | **Passed (bounded)** | Left-handed, reduced flashes, high contrast, aim assist and text-scale states captured; defaults restored |
| 180-second full-route diagnostic stability | **Passed (bounded)** | Six samples, thermal status 0, battery level unchanged, no configured fatal markers |
| Full action-by-action tutorial, all-fighter human route and comfort/fun approval | **Blocked** | Owner-operated tutorial, fighter comparison and human judgment remain required |
| Sustained performance against documented CPU/GPU/GC/frame/battery budgets | **Not run** | Capture lacks normalized frame histogram, GPU/GC and unplugged endurance evidence |

### P42 - Durable production replay capture and exact-artifact re-execution - 2026-08-27

The focused source/docs checkpoint for this continuation is commit
`2a113e0c4798e8e51a43379a0fa0facd7e8f0fe1` (`replay: persist ordered production captures`).

The offline replay foundation now has a versioned, Unity-independent `.brr` file format.
`MatchReplayFileSerializer` writes an explicit magic/version envelope, payload length and
SHA-256 checksum, and rejects truncation, trailing bytes or checksum corruption. Replay
frames can retain the exact same-tick authority submission order (including Pehel charge
steps), the complete header/content configuration, per-tick participant snapshots and
canonical hashes. Cosmetic Unity animation, audio and VFX remain presentation state rather
than replay inputs.

The development-only production bot harness captured one complete Bazaar Bastion match
through the production scene and wrote
`Builds/Local/V1GameplayTruth/ProductionBotReports/Replays/match-9101-20260827-160257598.brr`:
5,802,977 bytes, SHA-256
`48C0DC38A417934331245FBB28B8EE15589502C23E93619EC688310C1E487736`, 9,180 authority
frames, and 58,097 command-digest inputs. The matching report is
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-160256013-9101.json`
(SHA-256 `CAEDD80A751A1AE6C5B17583F2C7D480DB609F30F569FA51358B52A3AE12F550`) and records
the replay path/hash/frame count. The production smoke was **86/86 PlayMode** with one
seeded match, 306.0135 seconds, four combat eliminations, one Aandhi elimination and
31 bot-to-bot damaging pairs.

The same current source also passed the full 100-seed production-bot release batch with
release assertions enabled. Aggregate report
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260827-162349798-9101.json`
is 1,824,152 bytes (SHA-256
`553DE1DB288381038F972A98E78343D2435AC36029CD91D391B75917EA8345D8`); its test XML is
`Builds/Local/TestResults/playmode-production-bot-2a113e0.xml` (SHA-256
`AB8FBE0D19FB3D6025E9590AE3C73B66E4605BE9D2D98426E132473FEF3E9B42`) and its Unity log
SHA-256 is `F5FE2B6FC34BF38317D47164D2EC1087620A0270E887193DB02F12E8CCED556C`. All
100/100 matches completed in the 240-360 second window; 94/100 had at least three combat
eliminations, 100/100 had bot-to-bot damage, Aandhi-only resolutions were 0/100,
protected-warmup damage and invalid positions were both zero, maximum continuous stuck
time was zero ticks, out-of-range attempts were 6/15,323 (0.04%), rejected abilities were
6,816/35,492 (19.2%), and successful gadget uses were Umbrella 99, Dhol 100 and Tiffin
100. The batch emitted 100 replay files / 918,000 authority frames.

Two independent one-match runs from the same current source and seed `9101` reproduced
the same command digest `5470526C5AEC0388`, command count 58,097, replay SHA-256
`48C0DC38A417934331245FBB28B8EE15589502C23E93619EC688310C1E487736`, frame count 9,180
and duration 306.0135 seconds. Reports are
`batch-20260827-162909033-9101.json` (SHA-256
`015111AB4F437C77A1DC868EC2002005AF9190843BB9A3A9DC28DA621B039CB8`) and
`batch-20260827-163017185-9101.json` (SHA-256
`1C582F06ACF54AB1BCDD5229AD63DC4A4031016E8FDDF4861C419EC819E4006E`); the paired
PlayMode XML hashes are `EA48DD66B5D78BAC3ACA56C4E61DC86CFACB42A34C99409BF69EFBEFD89B6CB4`
and `FB08FD73C7CEDE6B7CD34D6040E351A39625DE58F0D9D472E20C3DADB8A46E10`.

The exact generated production replay was read and fully re-executed against the canonical
authority with per-tick snapshot/hash verification: **141/141 EditMode** in
`Builds/Local/TestResults/production-replay-verify-final.xml` (SHA-256
`5AD83DC7DDC6B0800E2BF33611863FF41A5935FC5F9397406E1359AF77B141FA`); Unity log SHA-256
`1B4500F2985F0106DFDE4A6DFC2CEFEEAF03B591B116708D7884176722144751`. The final no-path
EditMode regression is also **141/141** (`editmode-replay-final.xml`, SHA-256
`30722E1E65435E6FCF8DE9ACA1427512F35D74D98B6DC943AEFF91E2EBA44CB5`; log
`C6A636EBA3E17402BAF993CBDB1E8BDCA5FC1BA0748EB192767F89CC618C3338`). The final
one-match PlayMode smoke is **86/86** (`playmode-replay-final.xml`, SHA-256
`2DECE92391AF3E7FF6B156B9D5D24D009E6BBC5635AB0CAFF554FA867872E1C2`; log
`96E820F4C227F52C2B0F37EBA764F4A820CD39E123F9B8928E59C439A5527A28`). Static validation
remains **0 errors / 0 warnings**.

The post-serialization release-shaped pair was rebuilt from the current source. APK
`Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` is 40,533,686 bytes (SHA-256
`52B04A015656BB5480FBBCF5879578313D1B527E32BA205BBB9F102449C0986E`); AAB is 36,358,860
bytes (SHA-256 `9FA87846E85423499AC8A9305631091A4D38ADA8F0A49D03853F0B14B954499F`); build
log SHA-256 is `2D7C3D105AEE2CF7EE95D6B1C8B822B14F673786C90AE6CBE8D68F114BD5A9CD`.
These remain temporary-ID, debug-signed local artifacts; no publication or final signing
claim is made.

The clean final release checker was rerun from that commit and passed **0 errors / 0 warnings**.
Its captured output is `Builds/Local/Device/release-checker-2a113e0.log` (3,214 bytes;
SHA-256 `6CE4C48CDC734A1038139EFF67CF8196E51ECB8FA1DA4840828C9CCE37F69A80`). It confirms
APK SHA-256 `52B04A015656BB5480FBBCF5879578313D1B527E32BA205BBB9F102449C0986E`, AAB
SHA-256 `9FA87846E85423499AC8A9305631091A4D38ADA8F0A49D03853F0B14B954499F`, absent
network permissions, ARM64/static 16 KB alignment and a clean worktree.
The direct SDK check `zipalign -c -P 16 -v 4` also passed for the APK, and `apkanalyzer`
reported package `com.example.battleraja.m11`, version `1.0.0` / code `100`, with only
`VIBRATE` and the non-exported receiver permission. No network permission was present.
Cached bundletool `1.18.3` was downloaded from the official Google bundletool release
artifact (jar SHA-256
`A099CFA1543F55593BC2ED16A70A7C67FE54B1747BB7301F37FDFD6D91028E29`) and generated a
universal APK set from the exact AAB. APKS
`Builds/Local/V1GameplayTruth/Android/battleraja-v1-current-2a113e0.apks` is 36,487,401
bytes (SHA-256
`C242624B588790FA3870A46E94D93E0C4D64300B81B3FD27839EDA9A52F5032E`); extracted
`universal.apk` is 36,487,086 bytes (SHA-256
`EA38FE8A48A2A7DE61216BBA0B9FA386C277F4B8E9C861EAEA7C5AA3F1D5D2D7`). Direct
`zipalign -c -P 16 -v 4` passed for the extracted universal APK, and `apksigner verify`
passed with one v3 signer. The command/result log is
`Builds/M11/Logs/bundletool-1183-current-2a113e0.log` (1,349 bytes; SHA-256
`36FFDC194F6686B4EF72FE7DD2C6B8E84623ACA0E9B3E542EB321A0F310E306D`).

The exact current APK was also installed and launched on the locally available genuine
16 KB Android emulator `BattleRaja_16K` (Android 36 `google_apis_playstore_ps16k`, serial
`emulator-5554`). `getconf PAGESIZE` reported **16,384** bytes; the Unity activity was
top-resumed after the menu -> Solo Raja -> fighter selection -> live opening-match route,
and the captured logcat contained no configured fatal markers. Evidence is under
`Builds/Local/Device/Performance/20260827-16k-current-2a113e0/`: `route-summary.txt`
(1,105 bytes; SHA-256
`B11910A202A8B5C9EEDB813CFAC2251ADCACE493FB34E8D985986C802BC93876`), logcat SHA-256
`2534314E0D01925C53B240417079783334D0872FAF2260CE6EAC065732798322`, and live-match
screenshot SHA-256 `F41317A5CD27B9FFAC3CC03DDC50A213E3B2BA34E9813A8550F0617F6CE7CD3A`.
This closes emulator runtime evidence only; owner-operated Lava comfort/endurance and
human review remain separate gates.

#### P42 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Versioned durable replay serialization with integrity rejection | **Passed** | Byte-for-byte round trip plus truncation/checksum regression in 141/141 EditMode |
| Production-scene command capture and per-tick canonical state retention | **Passed** | Current source passed 100/100 release-gate matches and emitted 100 replay files / 918,000 ordered frames with snapshots/hashes |
| Exact production replay read and full authority re-execution | **Passed** | Exact `.brr` read/replayed with all per-tick snapshot/hash checks passing |
| Same-seed production command/replay reproducibility | **Passed** | Two independent seed-9101 runs matched command digest, count, duration, frame count and replay SHA |
| APK-set generation, universal extraction, signing verification and 16 KB zip alignment | **Passed** | bundletool 1.18.3 generated the current AAB's universal APK; extracted zipalign and v3 apksigner verification passed |
| Genuine 16 KB runtime launch on available emulator | **Passed (diagnostic)** | Current APK launched on Android 36 `google_apis_playstore_ps16k`; `getconf PAGESIZE` = 16,384 and no fatal markers; physical-device endurance remains open |
| Cross-machine floating-point parity | **Not run** | Same-machine replay evidence does not establish device/architecture parity |
| Final human review of cosmetic presentation replay (audio/VFX/animation) | **Blocked** | Cosmetic presentation is intentionally not an authority replay input; owner human review remains required |

The stopping-condition review for this checkpoint is explicit: the remaining V1 items are
owner/device/legal/store gates, or require an owner judgment that cannot be made safely by
the agent. They are not silently treated as passes.

| Remaining V1 gate | Classification | Current boundary |
| --- | --- | --- |
| Full Lava touch tutorial, all-fighter route, accessibility comfort and fun/balance approval | **Blocked** | Approved Lava `ST5GDW23LB004392` is currently locked; do not bypass its owner lock or substitute emulator evidence |
| Sustained CPU/GPU/GC/frame-pacing, thermal, battery and repeated-rematch budget approval | **Not run** | Requires owner-operated physical-device sessions and normalized profiling |
| Final authored art/audio/VFX readability, originality and cultural review | **Blocked** | Saved generated baseline exists; final human selection/approval remains required |
| Final package identity, release signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Owner/legal/store actions are not authorized in this task |
| Photon, PlayFab, accounts, online and Web product work | **Not applicable** | V1 scope is explicitly offline Android-only |

### P43 - Faceted fighter silhouette pass and exact Android refresh - 2026-08-28

The focused local art continuation is commit `816d9ac` (`art: replace fighter primitives
with faceted silhouettes`). `ProductionArtBuilder` now contains reproducible faceted loft
and extruded-polygon mesh recipes, and its explicit `Rebuild V1 Production Fighter Art`
entry point regenerates the saved presentation assets after a reviewed visual change.
Bijli, Pehel and Maya use distinct saved torso/cloak, visor, shoulder, arm, boot, sash,
mask, scarf, badge and crystal profiles. `ProductionPresentationBuilder` reparents the
new parts into the existing presentation-only rig while retaining the Animator/VFX
assets. No gameplay, collider, authority, input, network or package policy code changed.

The generated-art batch log is
`Builds/Local/Logs/rebuild-production-art-faceted-20260828.log` (202,129 bytes;
SHA-256 `701C22E3C9584AF34A69840A33BB51F637543C32600C0054EBF454F0712B5CD0`) and ends
with the explicit rebuild completion message and Unity exit code 0. The asset-quality
regression requires at least 260 combined vertices per instantiated production silhouette
and exactly three distinct mesh profiles.

#### P43 validation and artifacts

- Static repository validation: **0 errors / 0 warnings**.
- Full EditMode: **141/141 passed**, 0 failed, 0 skipped. XML
  `Builds/Local/TestResults/editmode-faceted-20260828.xml` is 109,808 bytes (SHA-256
  `F74CDDA815056DECDE881BBFBCB38364225E3EC59B6ACD6CD347A7C0356BCB49`); Unity log
  SHA-256 `DED4DFF35248DCA70392D0B9E8E046971C8768C83CC10AB347834DABC7E11268`.
- Full PlayMode: **87/87 passed**, 0 failed, 0 skipped, including the new faceted
  silhouette regression. XML `Builds/Local/TestResults/playmode-faceted-20260828.xml`
  is 76,853 bytes (SHA-256
  `0D5876BC26F3BDA4AA83024868BE28F2A7349EAFD2A60A36D9A4F65916F1B103`); Unity log
  SHA-256 `A347344747F506AE39BB63D6E039DA3960CB46B5680DEF982AF3CA3965BE3C81`.
- Rebuilt debug-signed APK: 40,542,342 bytes, SHA-256
  `0517EE901A9EAE943140538366B0574E893DC6BD66A5D1714D630C2379EF5FAC`.
- Rebuilt release-shaped AAB: 36,367,513 bytes, SHA-256
  `BF52E649BFD92F277F5C9933A7FDF34FFB25410F1D5A18EF6FC3097AA31BA331`.
- The composed Android checker passed the offline manifest, API 28/36, no-network
  permission, seven-ARM64-library and static 16 KB ELF gates, plus 512x512 icon and
  1024x500 feature-graphic dimensions. The checker was run before the documentation
  commit while the art change was the only source delta. The clean-worktree rerun after
  the documentation commit is captured in
  `Builds/Local/Device/release-checker-00a3a21.log` (SHA-256
  `DF1A1AC058DDB8C78AF17B77EA63EEFB1F301C04E4D99C3AB40D69A6C577417C`) and passed with
  the worktree clean.
- bundletool 1.18.3 generated
  `Builds/Local/V1GameplayTruth/Android/battleraja-v1-faceted-20260828.apks` (36,495,593
  bytes; SHA-256
  `B6EF2801694750F33967234AEC34F4605DEEF54D0AECFF7B53DF044239FA1B7F`). The extracted
  universal APK is 36,495,278 bytes (SHA-256
  `84CDE9E1ADA09E92426EF1E01BC6539D3F3734559833345F2FE4ECB9BE7509DE`); direct
  `zipalign -c -P 16 -v 4` passed and `apksigner verify --verbose` passed with one v3
  signer.

The rebuilt APK installed on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android
14/API 34, 4,096-byte pages). Real touch input reached the menu, Solo Raja, Bijli
selection and the live opening match. Screenshots are retained under
`Builds/Local/Device/Performance/20260828-lava-faceted-smoke/`: `launch.png` SHA-256
`217984A80310452CDE4C0BBD804B255509376BAA47D01483CF5A28FEEB0EED43`, `mode.png`
`7E8C5B975C9AE357A82BC4C4D7522F331D3A9C2BD1029EBB991CD267F9E64830`,
`fighter-select.png` `90F6750AD276150607A0D466F3421471928F92EB80E55FAE89F11EE309B57912`,
and `live-opening.png`
`5390F653CFBAA2C5D0049DF6A28379C14DC043062C7C6677072BE8887184E243`. A bounded
30-second, six-sample capture from the live state is under
`Builds/Local/Device/Performance/20260828-lava-faceted-30s/`; its manifest is 1,118
bytes (SHA-256 `5416A7A3334858D8CF5B6C904B51D7D4CB7BE5C5F750620060E48B9A71C8228C`),
logcat is SHA-256
`E67E88C8FE69548264DA29C0509FC65051BDFA08663BD57E783A321E23457FD1`, thermal status
was 0 in all six samples, and no configured fatal markers were found.

The same exact APK also installed and launched on the genuine 16 KB `BattleRaja_16K`
emulator (Android 36, `sdk_gphone16k_x86_64`). `getconf PAGESIZE` reported **16,384**;
the Unity activity was top-resumed after the same menu -> Solo Raja -> Bijli selection ->
live opening-match route, and the route logcat contained no configured fatal markers.
Current-source captures are under `Builds/Local/Device/Performance/20260828-16k-faceted-smoke/`:
`launch.png` SHA-256
`919BA18BBCA77C4C843DD07EC1470E8D0DFAE4AC3C3F012266E102ACABD55FA0`, `mode.png`
`A79F97AC6650C7157ADF7427A2991A0932D8345A540695A851A6CAE858EA77B4`,
`fighter-select.png` `DFAB20D0F495B5E9624C3DDCB44FE9D705319F7E19F4988F36A6F3309856FA92`,
`live-opening.png`
`8BA29FCF641059474D99EE4D730AA8C83CD4E90E6B62CC8218A3DC1922551EBA`, and route
logcat SHA-256 `BB2B906201FA723FE5A87089B3664857131C1500A3B770CA4F8340D2AE3B15C6`.
This is emulator runtime evidence only; the approved Lava device reports 4,096-byte
pages and remains the physical-device gate.

#### P43 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Saved faceted fighter meshes and distinct production profiles | **Passed (machine-verified baseline)** | 87/87 PlayMode, >=260 vertices per instance and three distinct profiles; final human art approval remains open |
| Exact Android APK/AAB rebuild and offline technical gates | **Passed** | APK/AAB hashes above; checker, ARM64/static 16 KB, bundletool, zipalign and v3 verification passed |
| Exact APK Lava menu -> Solo Raja -> fighter selection -> live opening match | **Passed (bounded smoke)** | Real touch route and screenshots retained; full tutorial/all-fighter/Results/rematch review remains open |
| Exact APK Lava 30-second live-state diagnostic | **Passed (bounded)** | Six samples, thermal status 0 and no configured fatal markers; not sustained performance approval |
| Genuine 16 KB Android runtime on current exact APK | **Passed (diagnostic)** | Current APK route completed on `BattleRaja_16K` with `PAGESIZE=16384`; physical ARM64 16 KB coverage remains unproven |
| Genuine physical 16 KB runtime | **Not established** | Lava reports 4,096-byte pages; current exact emulator evidence is diagnostic and does not prove physical ARM64 runtime behavior |
| Final commissioned art, animation/VFX direction, audio mix, cultural and human feel review | **Blocked** | The saved faceted meshes strengthen the generated baseline but do not replace owner approval |
| Package identity, release signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Temporary `com.example.battleraja.m11` debug-signed artifacts; owner/legal actions remain required |

The generated faceted meshes are original repository-owned procedural assets and are not
claimed as final commissioned art. No public upload, signing-key use or remote update was
performed in this continuation.

### P44 - UV-ready primary skins and exact Android verification - 2026-08-29

The focused local continuation is commit `bc392fd` (`art: add UV-ready skinned fighter
primaries`). It stays inside the offline presentation boundary: no gameplay/domain,
authority, input, network, package-policy or store code changed. `ProductionArtBuilder`
now assigns deterministic planar/cylindrical UVs to every generated mesh. The explicit
production-art rebuild derives the Bijli and Pehel `Body` meshes and Maya `Cloak` into
saved `SkinnedMeshRenderer` assets (`BijliSkinBody`, `PehelSkinBody`, `MayaSkinCloak`) with
hips/chest bind poses and waist-blended weights; accessory parts remain saved static
render-only children. The source primary MeshFilter is retained for reproducible rebuilds,
with only its renderer disabled. This is technical generated art, not final commissioned
modeling, texturing, animation direction or cultural approval.

#### P44 validation and artifacts

- Controlled rebuild: `Builds/Local/Logs/rebuild-production-art-uv-skin-20260829.log`
  (60,901 bytes; SHA-256
  `A328F236BC85E2E522CEC090334AB1FD8E067F955C6662BE27C6514C2472809E`), Unity exit code
  0, no skin-build skips.
- Full EditMode: **141/141 passed**, 0 failed, 0 skipped. XML
  `Builds/Local/TestResults/editmode-uv-skin-final-20260829.xml` (109,806 bytes; SHA-256
  `F63DE276D6EA6E6EE6B464E032D311A8AE3A5014B3370C1DDCBA608E39BFAEC`); log SHA-256
  `21053599E557F8EF79C40B58E8B95782BBB97508DCBB7FA93C01CD7D1601D770`.
- Full PlayMode: **87/87 passed**, 0 failed, 0 skipped. XML
  `Builds/Local/TestResults/playmode-uv-skin-final-20260829.xml` (76,849 bytes; SHA-256
  `4BF3FE9F93CD5BD06A62920291DA46C3126B03547315E1CC4707A0C23E2EB828`); log SHA-256
  `54AEDE98F26BCD0BB151D9F9298008267E80DEF31D14E31430D53178BE1A676C`.
  The new art assertions cover UV length, one saved two-bone primary per fighter,
  bind-pose/bone parity and non-zero blended weights. The same run's bounded production
  harness smoke completed 2/2 matches at 306.0 s average with 8 combat KOs, 2 Aandhi KOs,
  317/317 accepted attacks, 0 out-of-range attempts, 6/6 gadget uses and zero stuck
  recoveries. The 100-seed domain/bot release batch remains valid as carry-forward evidence
  from `ad078d3` because this commit changes only presentation assets and tests.
- Static repository validation: **0 errors / 0 warnings**.
- Exact debug-signed APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`,
  40,595,182 bytes, SHA-256
  `9A0F3715BFFA208F4D821B786D68EFE22A13C05053D05CA8611F6A614D318060`.
- Matching release-shaped AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`,
  36,420,355 bytes, SHA-256
  `C8CA4351D4778E5C117F9E9CA29D9C2CEA5C1BFF041718D6175AA7559CF14105`.
  The Unity Android build wrapper completed successfully for both entrypoints; the final
  APK build log is `Builds/M11/Logs/android-build.log` (399,350 bytes; SHA-256
  `6F5703F20ECC4FABCEE15233AD58AAD89D8F42347AE49518DF0FD8B5922BFE09`).
- The composed release checker passed the offline manifest/API, ARM64/static 16 KB and
  store-dimension gates. Its output is
  `Builds/Local/Logs/release-checker-uv-skin-20260829.log` (3,055 bytes; SHA-256
  `74AE7571DB083889F16B88F877565A33758FF894974ECA83ED09D7E6E142F31D`). It reports
  package `com.example.battleraja.m11`, version `1.0.0` / code `100`, min API 28 / target
  API 36, only `VIBRATE` plus the non-exported receiver permission, seven ARM64 libraries,
  no network permissions, and 512x512 / 1024x500 creative dimensions. The worktree was
  intentionally dirty only because of the two owner prompt files, so no clean-tree claim is
  made for this run.
- Bundletool `1.18.3` generated
  `Builds/Local/V1GameplayTruth/Android/battleraja-v1-uv-skin-20260829.apks` (36,548,841
  bytes; SHA-256
  `359D24270F6D3A126A52D4E611A902F31DCD104F6C22E05E4FC36BB383C9B391`). Extracted
  `universal.apk` is 36,548,526 bytes (SHA-256
  `942EABC3E278D56C9699CF27D68F14EC30487C4CBD38F42DB50634243087D8BC`). Direct and
  extracted `zipalign -c -P 16 -v 4` and v2/v3 `apksigner verify --verbose` all passed;
  command/result log SHA-256 is
  `7A71E363259D878177638482917D3DFFFB2F0338C0353C3741EAAEE14255D6BC`.

#### P44 runtime evidence

The exact APK was installed on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14,
4 KB pages) and real touch reached menu -> Solo Raja -> Bijli -> live opening. Screenshots
are under `Builds/Local/Device/Performance/20260829-lava-uv-skin-smoke/`: `launch.png`
SHA-256 `217984A80310452CDE4C0BBD804B255509376BAA47D01483CF5A28FEEB0EED43`, `mode.png`
`7E8C5B975C9AE357A82BC4C4D7522F331D3A9C2BD1029EBB991CD267F9E64830`, `fighter-select.png`
`90F6750AD276150607A0D466F3421471928F92EB80E55FAE89F11EE309B57912`, and
`live-opening.png` `F38E682D459D88C7F358A26F5B425AD72E5CFF75F94DA06BC9AAA8C8EFD5D214`.
A six-sample, 30-second live-state diagnostic is under
`Builds/Local/Device/Performance/20260829-lava-uv-skin-30s/`: manifest SHA-256
`4B0BE568E635CDF02BF7FF430C148507478F1274A218ED972CFE773B17197419`, logcat SHA-256
`9428E76888162201E876A8B4C652693BB68E0E165030723317377C0D95E2AEB8`, thermal status 0
before/after and no configured fatal markers. Raw PSS was 263,051–269,743 KB, graphics PSS
75,128–79,224 KB and swap PSS 389–698 KB; no normalized CPU/GPU/GC/frame-pacing or
endurance approval is claimed.

The exact APK also reached the live opening on genuine `BattleRaja_16K` (`sdk_gphone16k_x86_64`,
Android 36, `getconf PAGE_SIZE=16384`) with no configured fatal markers. Current evidence
is under `Builds/Local/Device/Performance/20260829-16k-uv-skin-runtime/`: `device-info.txt`
SHA-256 `10C8E7E3AC0710CC280B8D39D53B0BE3D0E9B0605EB01510F8C3A3154FE28B9F`,
`route-activity.txt` SHA-256 `17675F7DAF3BC61C6619B86BE22D6954306EDBDAB3430DC112F055797E9FBC57`,
`route-logcat.txt` SHA-256 `ECCD1B18BA5269D911463C5505DCC4266869FE5A8A64F14566AA3F21D702D8CD`,
and `live-opening.png` SHA-256
`367169ECF4323D7AFF59CFC42C280CA633F3AAF25E4882B428AD6E25037A0BDB`. This is emulator
diagnostic evidence only; it does not prove physical ARM64 16 KB runtime behavior.

#### P44 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Deterministic UV coverage on generated meshes | **Passed** | All generated MeshFilters and saved primary skins have one UV per vertex; EditMode/PlayMode assertions pass on `bc392fd` |
| Saved two-bone primary skin for Bijli, Pehel and Maya | **Passed (machine-verified baseline)** | Three saved meshes, hips/chest bind poses and blended weights; final authored skinning/texturing remains open |
| Exact APK/AAB rebuild and offline technical gates | **Passed** | APK/AAB hashes above; checker, ARM64/static 16 KB, bundletool, zipalign and signature verification pass |
| Exact APK Lava menu -> Solo Raja -> fighter selection -> live opening | **Passed (bounded smoke)** | Real touch route and screenshots retained; full tutorial/all-fighter/results/rematch review remains open |
| Exact APK Lava 30-second live-state diagnostic | **Passed (bounded)** | Six samples, thermal status 0 and no configured fatal markers; raw-only diagnostic |
| Genuine 16 KB Android runtime on current exact APK | **Passed (diagnostic)** | `BattleRaja_16K` route completed with `PAGE_SIZE=16384`; physical ARM64 16 KB coverage remains unproven |
| 100-seed production bot release batch | **Passed (carry-forward)** | Prior `ad078d3` batch remains applicable because `bc392fd` is presentation-only; 2-match smoke also passed |
| Final commissioned art, animation/VFX direction, audio mix, originality and cultural review | **Blocked** | Generated UV/skinned baseline is not owner approval |
| Physical Lava tutorial/all-fighter/accessibility/comfort/fun review | **Blocked** | Owner-operated action-by-action review remains required |
| Sustained performance, thermal, battery and repeated-rematch budgets | **Not run** | Current Lava capture is bounded raw telemetry, not normalized sign-off |
| Package identity, release signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Temporary package/debug signing; owner/legal/store actions remain required |

The two prompt files remain intentional uncommitted owner work. No remote mutation was
performed in P44; `origin/main` remains at `ad078d3` while local branch
`codex/v1-playstore-release` contains the focused unpushed commit `bc392fd`.

### P45 — Saved textured Bazaar environment, LOD coverage, runtime mesh fallback hardening and exact-source gate refresh — 2026-08-29

The exact local art/runtime commit is `ac45479` (`art: integrate saved Bazaar environment
and runtime mesh fallbacks`). It is still inside the offline presentation boundary: no
gameplay/domain, authority, input, network or package-policy code changed. The controlled
editor path now saves `BazaarBastionProduction.prefab` with a 32×32 ground mosaic (4,096
vertices, three material submeshes), six environment mesh assets, 16 deterministic 64×64
textures and matching URP materials, themed gates/stalls/banners/rugs/lanterns/crates/palms
and a backdrop LOD group. The active Bazaar scene removes the legacy `BazaarArchitecture`
instance and binds this prefab with runtime fallback disabled. The saved environment contains
no colliders; the existing authored collision/navigation layer remains authoritative.

Fighter production prefabs now carry two LOD levels and saved far-silhouette meshes. The
shared `PresentationMeshFactory` supplies custom boxes/cylinders/rings/discs/faceted orbs to
runtime feedback, projectile, gadget and decoy fallback visuals. The Maya decoy visible
surface is custom geometry and its targetability capsule is explicit, preserving local bot
perception/projectile probes without a Unity primitive construction path. The generated
textures, meshes, LODs and fallback library are technical V1 baselines, not commissioned
final art, animation/VFX direction, cultural approval or performance sign-off.

#### P45 editor and automated evidence

- Controlled Unity generation completed with exit code 0. Logs are
  `Builds/Local/Logs/production-art-build-fixed.log` (97,975 bytes; SHA-256
  `B8103ADABA834A6EA526B5832A28BCA33057888F23F4F1610CF313374A1F142D`),
  `production-environment-build.log` (65,938 bytes; SHA-256
  `67E92DA78574FE5CF09A379F858E4D7332EBC9A745271361EEFEB071ADDE6B0D`),
  `bazaar-scene-integration.log` (48,961 bytes; SHA-256
  `A21A3473F83C32B0CE65AAA850029CCC4F44DE82C783374826DB9799A8307D26`) and
  `production-art-rebuild-corrected.log` (69,155 bytes; SHA-256
  `351C6D3B693C9A3CA42142924CBA4D45201B1A1E1373464400D01A50BACCC609`). Unity emitted
  its known local licensing-token warning while still resolving the project entitlement;
  no generation failure was reported.
- Repository validation: **0 errors / 0 warnings** using
  `Tools/Validation/validate.ps1 -RequireUnityProject` with Unity `6000.5.6f1`.
- Full EditMode: **141/141 passed**, 0 failed, 0 skipped. XML
  `Builds/Local/TestResults/editmode-current-ac45479.xml` (109,798 bytes; SHA-256
  `72CCEF3D8A0139EA4F8853E714D74D14586D526EB5C26D155013D22AD20CCF1C`); Unity log
  SHA-256 `514B93C7B5F93BA789EC14C49582DE5EA984CE071869370AACAF208D8812031E`.
- Exact-source PlayMode/production-bot run: **87/87 passed**, 0 failed, 0 skipped. XML
  `Builds/Local/TestResults/production-bot-100-ac45479.xml` (76,941 bytes; SHA-256
  `715A4E12F57C0C9F103D85FB2A172D2DE1D514A9833F99E0997CB6A22495AC61`); Unity log
  SHA-256 `9A75CF71124BDAB898BCCC538751E38C45FDAF0682ECA70BF30E8D022DFA3DD0`.
- Exact-source deterministic replay soak: **141/141 passed**, 0 failed, 0 skipped, with
  `BATTLERAJA_SOAK_MATCHES=1000` (1,000 seeds executed twice; zero divergence), duration
  **565.8438552 s**. XML `Builds/Local/TestResults/deep-soak-1000-ac45479.xml`
  (109,844 bytes; SHA-256
  `C7E034C316A1FB89C271EB8FC4DEF2A3A26904229B05B88BC22388428B0F87EC`); Unity log
  SHA-256 `C94B285EDB2D59450D353FAEFDD296A95E25D6D44557D4D28953F90AE3B594C3`.

#### P45 exact 100-match production-bot batch

The fresh report is
`Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260829-154336643-9101.json`
(1,823,902 bytes; SHA-256
`6AA089797E55919CFD990C38CDA39640AB86B5D31E9CFEA4FDD47E6ACF83E1AB`). Unity
`6000.5.6f1` ran the `BazaarBastion` scene with base seed 9101 and playback scale 90:

- **100/100** matches completed within the tick budget and **100/100** landed in the
  240–360 second window (every match 306.0135 seconds in this fixed-step harness).
- **91/100** reached at least three combat eliminations; **100/100** had combat damage;
  **0/100** were Aandhi-only; **100/100** had bot-to-bot damage.
- Attacks were accepted **15,512/15,512**; 7 out-of-range attempts were observed by the
  diagnostic counter. Abilities were accepted **28,975/35,827** (6,852 rejected); effective
  ability work was 171,573 steps.
- Successful gadgets were **300/496**, with Umbrella, Dhol and Tiffin each used in all
  100 matches; 196 contextual attempts were rejected. There was 1 stuck recovery,
  maximum continuous stuck ticks 10, and maximum decision time 0.401 ms.
- Protected-warmup damage and invalid-position samples were zero. These are automated
  authority/safety gates; human bot fairness, visual readability and fun review remain open.

#### P45 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Saved textured Bazaar environment and render-only authority boundary | **Passed (machine-verified baseline)** | Saved prefab, 4,096-vertex three-submesh ground, 16 textures/materials, zero environment colliders and production-scene binding; final environment art/cultural review remains open |
| Fighter far-silhouette LOD coverage | **Passed (machine-verified baseline)** | Three saved far meshes, two LOD levels per fighter, PlayMode assertions; transition/pop and human readability review remain open |
| Runtime fallback primitive removal | **Passed (machine-verified baseline)** | Shared custom mesh library used by feedback/projectile/gadget/decoy presentation paths; explicit decoy collider retained; editor fixture primitives remain development-only |
| Exact-source Unity regression suite | **Passed** | 141/141 EditMode and 87/87 PlayMode on `ac45479` |
| Exact-source 100-match production-bot pacing/safety gate | **Passed** | 100/100 terminal/in-window, 91/100 ≥3 combat KOs, 100/100 bot-to-bot damage, 0 Aandhi-only, zero protected/invalid samples |
| Exact-source deterministic replay soak | **Passed** | 1,000 seeds executed twice, zero divergence; XML/log hashes above; same-machine evidence only |
| Matching Android APK/AAB and approved-Lava refresh | **Passed (bounded technical gate)** | Exact APK 40,672,170 bytes (`6103F42176726E8CACE0DA7C4880BD105A55E50FFD92EB1BA8B2F531BEAA231D`) and AAB 36,497,323 bytes (`9893493591C4474E517B3D80A5107986493A2E70F59C850D17AC08C8B2748404`); release checker 0/0, bundletool/zipalign/signature checks passed; approved Lava route reached live opening and 30-second capture reached spectator; evidence below |
| Genuine physical 16 KB runtime | **Not established** | Approved Lava reports 4,096-byte pages; emulator-only 16 KB evidence remains diagnostic |
| Final commissioned art, animation/VFX direction, audio mix, originality and cultural review | **Blocked** | Owner review remains required for the generated baseline |
| Sustained performance, thermal, battery and repeated-rematch budgets | **Not run** | Fresh six-sample/30-second Lava capture is raw bounded evidence only: PSS 267,957–272,145 KB, graphics PSS 75,792–79,888 KB, thermal 0, battery 62%; it does not establish normalized sign-off |
| Package identity, release signing, privacy/Data Safety, content rating and Play Console | **Blocked** | Temporary `com.example.battleraja.m11` debug-signed artifacts; owner/legal/store actions remain required |

#### P45 exact Android and approved-Lava evidence

- APK build command completed with exit code 0 from the reviewed source; artifact:
  `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` (40,672,170 bytes; SHA-256
  `6103F42176726E8CACE0DA7C4880BD105A55E50FFD92EB1BA8B2F531BEAA231D`).
- AAB build command completed with exit code 0; artifact:
  `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` (36,497,323 bytes; SHA-256
  `9893493591C4474E517B3D80A5107986493A2E70F59C850D17AC08C8B2748404`).
- Composed checker log `Builds/Local/Logs/release-checker-ac45479-b4b5649.log` is
  3,245 bytes (SHA-256
  `F05A2E9FD98D5AD73D9B9E7F1C52222CC3F535AD82516C500EADA2A50A857CDB`); it passed
  offline permissions/API/package, ARM64/static 16 KB, icon/feature dimensions and
  reported 0 errors / 0 warnings. The APK has only `VIBRATE` plus Unity's dynamic
  receiver permission; no network permission is present.
- Bundletool 1.18.3 APKS evidence is
  `Builds/Local/V1GameplayTruth/Android/battleraja-v1-ac45479.apks` (SHA-256
  `4E864E09557DA59892C629BA0A2AD42FDA58562EFA8485BC81B3C8D93FCD66B3`); direct and
  extracted universal APK zipalign checks passed. Signature verification passed for
  the temporary Android Debug signer only; final release-key handling remains open.
- Approved Lava `ST5GDW23LB004392` only: install succeeded and real touch reached
  menu → Solo Raja → Bijli selection → live opening. Screenshots are under
  `Builds/Local/Device/Performance/20260829-lava-ac45479-smoke/`; `launch.png`,
  `mode.png`, `fighter-select.png` and `live-opening.png` SHA-256 values are
  `217984A80310452CDE4C0BBD804B255509376BAA47D01483CF5A28FEEB0EED43`,
  `7E8C5B975C9AE357A82BC4C4D7522F331D3A9C2BD1029EBB991CD267F9E64830`,
  `90F6750AD276150607A0D466F3421471928F92EB80E55FAE89F11EE309B57912` and
  `615A72B4332E26DE3C0DADCEFFEA7184ABABE12722EAC3ABC8F11C533FD0DD48`.
- Six samples at five-second intervals plus final capture are under
  `Builds/Local/Device/Performance/20260829-lava-ac45479-30s/`; final screenshot
  SHA-256 is `8255FA6ED94AA563355964C0C9A4B32681A2660B69C8A18BD14E1F7612234C53`,
  final logcat SHA-256 is
  `633C80D97AAD955DFAD03E44C15EC9DB04B2598AA922AE46CD056D26C0DACF23`, and the
  captured state reached player defeat/spectator without configured fatal, ANR or
  SIGSEGV markers. Lava reports 4,096-byte pages; thermal status remained 0 in the
  capture and battery level was 62% at the final sample. This is bounded raw device
  evidence, not normalized performance, battery, thermal, accessibility or full-route
  approval.

The two prompt files remain intentional uncommitted owner work. The reviewed local commits
are ready for a non-rewriting fast-forward to remote `main`; remote mutation is still
pending the final push verification.

## Later checkpoints

- [x] Fair fighter-specific bot AI and production match harness (100-match terminal,
  pacing and safety gates pass; human fairness/fun review remains open).
- [x] Controlled reference-game UX study on Lava only (research capture complete;
  deeper in-match comparison and human adaptation approval remain open).
- [x] Current V1 art/audio/UI direction and asset-provenance documents (baseline only;
  final authored assets remain open).
- [ ] Production fighters, arena, gadgets, rigs, animation and VFX (saved generated
  presentation baseline exists; final authored production set and human review remain open).
- [ ] Coherent mobile UI/tutorial redesign and accessibility QA.
- [ ] Authored audio/music/mix and feedback.
- [ ] Feel/balance playtests and changelog evidence.
- [ ] Lava performance hardening against measured budgets.
- [x] Current Android/Play compliance recheck (technical checker and policy recheck
  complete; final signed identity, runtime 16 KB and Play Console work remain open).
- [ ] Store/privacy/content-rating preparation.
- [ ] Final exact-source QA matrix and matching APK/AAB.

Final publication, signing, package identity, branding, cultural/legal approval and Play
Console actions remain owner-controlled and are not authorized by this plan.

### P46 - Exact d0de949 aim-state candidate and approved Lava route - 2026-08-30

The exact runtime/art source for this checkpoint is commit `d0de9499e764045d72dbf092da4c8f2d85fb0b36`
(`art: add dedicated fighter aim animation state`). `PlayerInputAdapter.IsAimHeld` now exposes
the existing player aim intent without physics or aim-assist side effects; `FighterPresentation`
selects a dedicated render-only `Aim` state; and `ProductionPresentationBuilder` saves a
looping `FighterAim.anim` clip into the existing controller. No authority, damage, cooldown,
movement, gadget, or simulation timing rule changed. Rebuilding the source art first and then
the presentation prefabs preserved the saved accessory meshes.

#### Machine evidence

- Repository validation: **0 errors / 0 warnings** from `Tools/Validation/validate.ps1`.
- EditMode: **141/141 passed**, XML `Builds/Local/TestResults/editmode-d0de949.xml`.
- PlayMode: **87/87 passed**, XML `Builds/Local/TestResults/playmode-d0de949.xml`.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, **40,676,862 bytes**,
  SHA-256 `334EC0F8E1F0F2B04CEF52DB44586842E3004E76B28143007FB10EC310B308E9`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, **36,502,035 bytes**,
  SHA-256 `958792924DA7925474AAB40C9B5A5D588E4776AE756E333D0C8437EF4D5FF086`.
- Release checker: `Builds/Local/Logs/release-checker-d0de949.log`, **0 errors / 0 warnings**;
  package `com.example.battleraja.m11`, version `1.0.0`/code `100`, min/target API `28/36`,
  only VIBRATE plus Unity's dynamic receiver permission, no network permission, seven ARM64
  native libraries, static 16 KB ELF alignment, and icon/feature dimensions `512x512`/`1024x500`.
- Bundletool 1.18.3: `battleraja-v1-d0de949.apks` SHA-256
  `A96663A491A41AF5782328317115B10DB32E32A766CB06FFB1F1ABE972C17862`; extracted universal
  APK SHA-256 `19787B8DE1CD71937E53051029A6E9013BE830035255C7D250B62668F0D9F17F`; direct and
  extracted `zipalign -c -P 16 -v 4` checks passed, and temporary debug-signature verification
  passed. This is not production signing.

#### Approved Lava evidence

The exact APK installed successfully only on approved Lava `ST5GDW23LB004392`
(`LAVA LXX508`, Android 14/API 34, reported 4,096-byte pages). The evidence folder is
`Builds/Local/Device/Performance/20260830-lava-d0de949-aim/`; its `manifest.json` indexes
all filenames and hashes. Real touch reached menu, Solo Raja, all three fighter cards
(Bijli, Pehel and Maya), live opening, attack/ability/gadget action feedback, mid-match
Aandhi warning/closing, player elimination, spectator/final-circle state, results with
placement and rematch, pause/settings, left-handed/high-contrast/aim-assist toggles, restored
settings, background/resume, and tutorial completion. The tutorial route reached `8/8 COMPLETE`
through the in-app SKIP control after the player was eliminated by the closing Aandhi; this is
valid completion-state evidence, not a claim that every action-gated step was comfortable or
that a victory was observed on the physical device.

The action telemetry snapshot records **265,746 KB total PSS**, **378,096 KB total RSS** and
**75,792 KB graphics PSS**. The captured logcat has no configured app fatal/ANR/SIGSEGV marker;
`gfxinfo` exposes only the Unity view hierarchy, so these are bounded raw observations rather
than normalized FPS, frame-time, GC, thermal, battery, accessibility or sustained-match approval.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Source, tests, package manifest, offline permissions and static alignment | **Passed locally** | Exact d0de949 results and release checker above |
| Approved-device install, launch, route and bounded crash-marker smoke | **Passed locally** | Lava folder and manifest; only `ST5GDW23LB004392` was used |
| Tutorial completion, spectator/results/rematch/settings/lifecycle observations | **Observed / bounded** | Exact screenshots; owner comfort and repeated-route review remain |
| Genuine 16 KB runtime | **Open** | Lava reports 4 KB pages; requires a genuine 16 KB runtime environment |
| Sustained performance, thermal, battery, GC/frame-time and repeated rematches | **Open** | Current telemetry is raw and bounded; no normalized budget pass claimed |
| Final authored art/audio, cultural, fun and accessibility approval | **Owner review required** | Generated presentation baseline remains a candidate |
| Final package identity, release signing, privacy/Data Safety, IARC/content rating and Play Console | **Owner-controlled** | Drafts/checklists are prepared; no upload or public deployment performed |

The candidate remains a **prototype / Android offline release candidate in progress**, not
Play-ready. The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P58 - Dismissible tutorial completion card and exact-candidate Results/rematch route - 2026-08-31

The current UI/test source checkpoint is `888421f0b332a2e5b9b41fcb6ae669adec836612`
(`ui: release results after tutorial completion`). The completed `TutorialOverlay` state now
uses `CLOSE CARD` for its secondary action instead of leaving `SKIP` in place. The new public
`DismissCompletionCard` method hides only the tutorial panel after completion; `REPLAY TUTORIAL`
and `MENU` remain available. This preserves the authoritative `OfflineMatchHud` Results,
REMATCH and MENU controls underneath the card without changing simulation, authority, timing,
or reward state. `TutorialArenaPlayModeTests.CompletedTutorialCanDismissOverlayForResultsAndRematch`
asserts the label, visibility transition and idempotent repeated dismissal.

#### Machine evidence

- `Tools/Validation/validate.ps1` returned **0 errors / 0 warnings**. Log:
  `Builds/Local/Logs/validate-tutorial-dismiss-20260830.log` (SHA-256
  `7EF09129DBD03921DF243F43AC65AE932A8C74C4DD76FAD8E6A013BFC804E322`).
- Full EditMode returned **141/141 passed**. XML
  `Builds/Local/TestResults/editmode-tutorial-dismiss-20260830.xml` (SHA-256
  `3C192135EFB96B491189B2B0E85F7D1E0D786A1236803221870134E3B70FA829`); Unity log SHA-256
  `1B0669DD49E9404C30D008613CC0AFB792E85388AD22638C5530FFB5884DB0E5`.
- Full PlayMode returned **92/92 passed**. XML
  `Builds/Local/TestResults/playmode-tutorial-dismiss-20260830.xml` (SHA-256
  `F829645D0B4F901DF1CFB8A1DFD1ABBD8CC65AC956E15CD39A81DA7EFBA1A92F`); Unity log SHA-256
  `5993EB13B9FA30C1EEBBCDDF6A8B5E9931B5DB653D1243A9F2E854F415E3D611`.
- Unity's Android APK entrypoint completed successfully. APK
  `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` is **40,682,359 bytes**,
  SHA-256 `B3D4EF4749270FDAD30474113683E050693BFA013173FF5EB1E3848C26C87F44`; build-log
  SHA-256 `756CDBBFBFFAF9DCEEA1DAF69C61FCD587AEB914CB453EA445BC9B48F04559BA`.
- Unity/Gradle AAB packaging completed successfully. AAB
  `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` is **36,507,683 bytes**,
  SHA-256 `CC5D2B362EA8330BB3FA22E93D530CD018D4933305744E26EF2504300B88D6F6`; build-log
  SHA-256 `62E7DA4E4FF745B435C0BDF2754AF37D5216EC0CED32E2BF020234BA94FBA617`.
- `Tools/Validation/check_v1_release_candidate.ps1` returned **0 errors / 0 warnings**.
  Log `Builds/Local/Logs/release-checker-tutorial-dismiss-20260830.log` has SHA-256
  `A397FA27D8FF577EDF7FC0EC4A2181DED55C2BF794BA48E9E020DCF029C88446`. It reports the
  temporary package `com.example.battleraja.m11`, version `1.0.0`/code `100`, min/target
  API `28/36`, no INTERNET or ACCESS_NETWORK_STATE permission, seven ARM64 libraries,
  static 16 KB alignment and temporary/debug signing. Worktree dirtiness is expected from
  the two owner prompt files.

#### Approved Lava evidence

The rebuilt APK was installed and exercised only on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, reported 4 KB pages). The route opened
Tutorial from the branded menu, used the in-app `SKIP` control to reach `TUTORIAL COMPLETE
8/8`, tapped the new `CLOSE CARD` action, and captured the exposed live HUD. The offline
match then reached its Results panel with placement data and `REMATCH`/`MENU`; tapping
REMATCH opened a fresh TutorialArena movement card. Because this route intentionally used
SKIP and the rematch run was not a full action-by-action replay, it is route evidence only,
not tutorial-comfort, repeated-rematch, fun or accessibility approval.

Exact retained evidence is indexed by
`Builds/Local/Device/final-circle-20260830/tutorial-dismiss-route-manifest.json` (5,230
bytes; SHA-256
`85D7E29C683C04D71C12F9FADB7720C49F68D6947A5E2C94A0F278BB9389D42D`):

- `tutorial-dismiss-complete-card.png` — 111,545 bytes, SHA-256
  `091BAC6D49F04A8ABCA8EBB062F9FE8C6694EA40E0F4ADC9EAE3BB761FF05C06`.
- `tutorial-dismiss-live-after-close.png` — 81,757 bytes, SHA-256
  `BF64E635D06E75A337B50D912BD4A18234BFC4C0F4C81629A894F3AE2F1D0598`.
- `tutorial-dismiss-results-final.png` — 108,834 bytes, SHA-256
  `9571D928D827B26C6389AF85A384A53DD16F3FD9D109CF050B667B17C519DE8D`.
- `tutorial-dismiss-rematch-opening.png` — 102,465 bytes, SHA-256
  `DEC4C3CBD6AABA813A09CE05F2FBBAE0738C534E629A01F81583075D0DCD5E0A`.
- UI tree `tutorial-dismiss-route-ui.xml` is 2,546 bytes, SHA-256
  `A8235CD2CFEE7BFCFB0A515F9337E4ABF6E6C16C10ACDCF446DE87E9AEF094BD`; it exposes only
  Unity's `SurfaceView`, so the touch coordinates were visually derived and are not semantic
  UI-locator evidence.
- App-scoped logcat `tutorial-dismiss-route-app-logcat.txt` is 15,054 bytes, SHA-256
  `F6347EC04E3156B8CF5056DC51B175D2646F5A6430613F1AE0A40262D2E8F0FE`; no configured app
  fatal/ANR/SIGSEGV/SIGABRT marker was found. Known Lava gralloc/AHardwareBuffer
  format-allocation noise is retained as a non-fatal observation.

#### P58 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Completion card can release the underlying Results/REMATCH surface | **Passed locally** | 92/92 PlayMode regression plus exact Lava `CLOSE CARD` and live-HUD captures |
| Exact source, APK/AAB rebuild, offline permissions and static alignment | **Passed locally** | Exact artifact hashes, checker 0/0 and manifest above; artifacts remain temporary/debug-signed |
| Exact-candidate Results and rematch route | **Observed / bounded** | Results placement and REMATCH were reached; rematch reopened TutorialArena after an idle run, so repeated comfort remains open |
| Full action-by-action tutorial and all-fighter/all-gadget touch comfort | **Open** | SKIP was used for this route; repeat the exact candidate with human-owned comfort review |
| Genuine physical 16 KB runtime | **Open** | Lava reports 4 KB; host-GPU Android 16 AVD evidence is profile-specific |
| Normalized performance, battery, thermal and repeated-match memory | **Open** | Existing captures remain bounded raw diagnostics, not acceptance |
| Final authored art/audio, cultural, accessibility, fun and fairness approval | **Owner/human review required** | Generated presentation baseline remains a candidate |
| Final identity/signing/privacy/Data Safety/rating/support URL/Play Console | **Owner-controlled** | No final key, upload, legal acceptance or public deployment performed |

The candidate remains a **prototype / Android offline release candidate in progress**, not
Play-ready. P58 closes the captured completion-card obstruction; it does not claim human
approval or close the remaining physical, subjective, performance, identity, legal or Play
gates. The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P59 - Exact-candidate all-fighter and accessibility route evidence - 2026-08-31

The P58 candidate (`888421f0b332a2e5b9b41fcb6ae669adec836612`) was exercised further on the
approved Lava phone without changing source or artifacts. This closes a useful local
observation gap: the current APK now has fresh live-opening, attack, ability-input and
gadget-feedback captures for Bijli, Maya and Pehel, plus a fresh accessibility/settings
route. It is observation evidence rather than a claim of human comfort, fun, final art,
or repeated-match approval.

#### Approved Lava route

Only `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, reported 4 KB pages) was used;
the connected Oppo was explicitly excluded. From a fresh launch, real touch opened Play
Offline / Solo Raja, inspected all three fighter cards, launched each fighter in the live
arena, and exercised attack, ability input and Tiffin Station gadget input. The route then
opened in-match Settings & Accessibility, toggled left-handed controls, reduced flashes,
high contrast, aim assist and text scaling, observed the left-handed live layout, verified
settings persistence from the main menu, and reset all toggles to defaults. The Unity UI
tree remains SurfaceView-only, so coordinates are visually derived rather than semantic
node locators.

The machine-readable index is
`Builds/Local/Device/final-circle-20260830/p58-fighter-accessibility-route-manifest.json`
(7,285 bytes; SHA-256
`F9D43C679971029EC9CC8881913A0BF62A28555A2F7C14C7A1FB94554C7D2409`).
It indexes 23 screenshot captures, the 2,546-byte UI tree
(`A8235CD2CFEE7BFCFB0A515F9337E4ABF6E6C16C10ACDCF446DE87E9AEF094BD`) and the app-scoped
logcat (15,630 bytes;
`F75016454F2A8A56716AED56E9942CAC918CDF44083B55C4A80B108B60E8D9AD`). The log has no
configured `FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or `SIGABRT` marker. Known non-fatal Lava
gralloc/AHardwareBuffer format-allocation diagnostics, the Unity Play Core
`AssetPackManager` class-probe, and Swappy `libgame.so` lookup diagnostics are retained,
not hidden or reclassified as a clean zero-error log.

Representative current-candidate captures include:

- `p58-bijli-live-opening.png`, `p58-bijli-attack.png`, `p58-bijli-ability.png` and
  `p58-bijli-gadget.png` — Bijli live/action feedback.
- `p58-maya-live-opening.png`, `p58-maya-attack.png`, `p58-maya-ability.png` and
  `p58-maya-gadget.png` — Maya live/action feedback.
- `p58-pehel-live-opening.png`, `p58-pehel-attack.png`, `p58-pehel-ability.png`,
  `p58-pehel-ability-followup.png` and `p58-pehel-gadget.png` — Pehel live/action input
  checkpoints and Tiffin Station feedback.
- `p58-left-handed-live.png`, `p58-settings-aim-assist.png`,
  `p58-settings-reduced-flashes.png`, `p58-settings-high-contrast.png`,
  `p58-settings-text-plus.png` and `p58-settings-reset.png` — accessibility toggles,
  left-handed layout and restored defaults.

#### P59 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Current APK live opening for Bijli, Maya and Pehel | **Observed / bounded** | Fresh exact-candidate Lava captures; no human visual or comfort approval claimed |
| Current APK attack, ability input and Tiffin Station feedback across all fighters | **Observed / bounded** | Screenshot route index; effect timing/comfort still needs human review |
| Settings, left-handed controls, reduced flashes, high contrast, aim assist, text scale and reset | **Observed / bounded** | Fresh Lava captures; accessibility comfort and device-size coverage remain open |
| App-scoped crash-marker smoke | **Passed locally with known platform noise** | No configured app fatal marker; Lava graphics diagnostics retained |
| Full action-by-action tutorial comfort, repeated rematches and multiple consecutive matches | **Open** | P58 SKIP/rematch route and this fighter route do not replace sustained human review |
| Genuine physical 16 KB runtime, normalized performance, battery/thermal and memory growth | **Open** | Lava reports 4 KB; existing data remains bounded diagnostics |
| Final authored art/audio, cultural, accessibility, fun and fairness approval | **Owner/human review required** | Current saved faceted assets remain a generated presentation baseline |
| Final package identity/signing/privacy/Data Safety/rating/support URL/Play Console | **Owner-controlled** | No irreversible identity, signing, legal or upload action performed |

The project remains a **prototype / Android offline release candidate in progress**, not
Play-ready. P59 adds current-candidate all-fighter and accessibility observations only; it
does not close the subjective, physical, performance, identity, legal or Play gates. The two
prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P66 - Lifecycle input hardening and exact Android candidate rerun - 2026-08-31

The release-candidate source tip is `e603ce7e7f1cb279f5e3e9d606ea5eae89603ecb`
(`android: clear input on lifecycle pause`). `PlayerInputAdapter`, `VirtualStick`,
`AttackButton`, `AbilityButton` and `GadgetUseButton` now clear transient state on
`OnApplicationPause`; `OfflineMatchHud` clears the player adapter before opening its
lifecycle pause boundary. ADR-076 records this presentation/input-only decision. No
authority, simulation-timing, replay, networking or package-policy rule changed.

The exact APK/AAB pair was rebuilt locally from this source. The APK is 40,679,115 bytes
(SHA-256 `349F02C67DE4CC801C5CB81B9CEC375A18D89B136C1A3AD9BB9549E9640A41CB`) and the AAB is
36,504,445 bytes (SHA-256
`9A5BE261D2504007BCBAF4105568F19437CBA8A4DEFAA3383371DE35386D51E0`). The composed release
checker passed **0 errors / 0 warnings** for package `com.example.battleraja.m11`, version
`1.0.0`/code `100`, API `28/36`, no network permissions, seven ARM64 libraries and static
`0x4000` ELF load alignment. Checker log:
`Builds/Local/Logs/release-checker-results-p66-20260831.log` (3,296 bytes; SHA-256
`C84C63625E9063698CE2DDD3CCEDD90901B94A5E0B56309CE1C39189C2D27337`). The Android build log
is `Builds/M11/Logs/android-build.log` (419,793 bytes; SHA-256
`770AC42466F4461093ECAA699B4DA2A0CEAE2271B55BE39562208F2E73D993C7`).

#### P66 machine evidence

- The focused PlayMode regression `OfflineMatchPlayModeTests.BackgroundLifecyclePausesAndResumesMatchSafely`
  passed **1/1** (0 failed, 0 skipped). Result XML is
  `Builds/Local/TestResults/p66-lifecycle-results-20260831.xml` (3,857 bytes; SHA-256
  `CF2AB2BE427AC2B50DA99311FAA52465D008FB29D2C64C20D15AB1F06F4DC38D`); log is
  `Builds/Local/Logs/p66-lifecycle-results-20260831.log` (47,280 bytes; SHA-256
  `1E6B99E76F5FECF9FE71B7625AF92966C149B3AA3DC0DFA7B7824A05AE56775A`). The test presses
  Attack, sends pause callbacks, verifies the held action is cleared and focus is false,
  then resumes and verifies the settings boundary closes with focus restored.
- Full EditMode passed **141/141** and full PlayMode passed **92/92**, both with zero failed
  or skipped tests. The result/log hashes are recorded in the P66 route manifest.
- Static repository validation passed **0 errors / 0 warnings**; log
  `Builds/Local/Logs/validate-results-p66-20260831.log` is 94 bytes (SHA-256
  `7EF09129DBD03921DF243F43AC65AE932A8C74C4DD76FAD8E6A013BFC804E322`).
- The rebuilt APK was installed only on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
  Android 14/API 34). The route manifest is
  `Builds/Local/Device/final-circle-20260830/p66-lifecycle-route/p66-lifecycle-route-manifest.json`
  (8,192 bytes; SHA-256 `217589DAE7592EC397328F12D8C3DF88246B7AEE035776584DF5FE9624499103`).
  It records the exact install, Solo Raja/Bijli live Opening Fight state, Android HOME for
  approximately five seconds, return to the same `UnityPlayerGameActivity`, Android 14,
  Lava's 4,096-byte page size and all evidence hashes. `live-before-rerun.png` is 332,133
  bytes (SHA-256 `C5A8F9A0385905257E20F42C9BBAEAF6762D9F9814EE2AB8A43494BA48CA272A`) and
  `live-after-resume-rerun.png` is 332,100 bytes (SHA-256
  `38DB3E26D7900971CA485CF1D0450F042FC1FD58008E69CD58DAF4C02685AC33`). Both show
  `OPENING FIGHT`, `ALIVE 8`, `ZONE 14.0 > 11.0` and the same live arena presentation.
  The focused activity dump is RESUMED/visible after return; the route logcat is 74,073
  bytes (SHA-256 `18DF04B1E92D7F323DB81B95DC90B7D4695DE960256870040531A97EF2D842EE`) with
  zero configured fatal, ANR, native-crash or managed-exception markers. Bundletool 1.18.3
  generated `battleraja-v1-p66.apks` (36,635,054 bytes; SHA-256
  `C49CE1483292B32895753FA0400537F6CF4A7A185C88BF7F5845CD9EEF0A4FC8`) and a universal APK
  (36,634,739 bytes; SHA-256
  `319CA36522B7899FFB913C875D15A2F70DFDD3CA54B7A2A31704734808CBAB25`). Direct universal
  `zipalign -c -P 16 -v 4` and `apksigner verify --verbose` both passed; the latter reports
  one temporary v3 signer. The bundletool log is
  `Builds/Local/Logs/bundletool-p66-20260831.log` (35,293 bytes; SHA-256
  `F7C41244F025B2A9442C31C6BB19A1B03BDE1821FEA7165B572438DDA45A574A`).

#### P66 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Android lifecycle clears held transient input at pause | **Passed by focused PlayMode regression** | Attack-held pause/resume assertion in `OfflineMatchPlayModeTests`; adapter and all touch-control reset paths covered by source review |
| Exact rebuilt APK/AAB technical gate | **Passed locally** | APK/AAB hashes, static validation and composed release checker above |
| Exact-device launch, HOME/resume return and live presentation | **Passed by bounded exact-device observation** | P66 manifest, paired Lava captures, RESUMED activity dump and marker-clean route log |
| Full lifecycle, all phases, repeated rematches, normalized performance, endurance and physical 16 KB | **Open** | Lava is 4 KB; physical route is short and does not replace sustained or 16 KB coverage |
| Final authored art/audio, accessibility, cultural, fairness and fun approval | **Owner/human review required** | Machine tests and bounded capture do not establish subjective approval |
| Final package identity/signing/privacy/Data Safety/rating/support URL/Play Console | **Owner-controlled** | Candidate remains temporary debug-signed; no irreversible upload or publication action |

The truthful classification remains **prototype / Android offline release candidate in
progress**, not Play-ready. P66 hardens lifecycle input release and refreshes the exact
candidate evidence; it does not close owner-controlled product, physical-device or Play
gates. The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P65 - Exact P61 physical all-gadget route on approved Lava - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` remained installed on
approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). No build or source
change was made for this route. The route manifest is
`Builds/Local/Device/final-circle-20260830/p65-gadget-route/p65-gadget-route-manifest.json`
(4,269 bytes; SHA-256 `48598855AEEDCD286C837219632ACFC6B972CEC0CB69B7E9D8EE28163BDED807`).

#### P65 machine evidence

- In the first Solo Raja/Bijli run, the initially held Tiffin was used with a valid aim
  direction. `13-tiffin-aimed.png` is 90,107 bytes (SHA-256
  `91928CA0C0FB3A9957DCDDF402A758C98C4460A98BF0D09237AD6CABE88A8748`) and shows
  `TIFFIN STATION DEPLOYED` with an empty gadget slot.
- The route reached the Dhol pickup and the HUD showed `GADGET DHOL` / `DHOL READY` in
  `18-left-to-dhol.png` (80,572 bytes; SHA-256
  `FD8C97861217A2A5AFBFD6C537954079BC00A328C15B392D58E52B8FBE5D7F4F`).
  `19-dhol-use.png` is 84,281 bytes (SHA-256
  `C67D9FCBB8F873038598A4F92446AE7B40E9C61DF50B9F14906197BD85CD50E8`) and shows
  `DHOL BURST` feedback.
- In a fresh Solo Raja/Bijli run, Tiffin was deployed again before the left-side route;
  `22-tiffin-use-before-umbrella.png` is 90,754 bytes (SHA-256
  `0A3682598BD16C39E68C28686680CC9DA4B6678EB90418E7C08B6FD2EC992E97`). The route reached
  `GADGET UMBRELLA` / `UMBRELLA READY` in `23-diagonal-left-progress.png` (77,258 bytes;
  SHA-256 `EAD710DF6318380F8BCB8CBD04118BD8E4B3A05BE7B62672EF3BD20BC4E331F7`), then
  `24-umbrella-use.png` (82,924 bytes; SHA-256
  `B35585DE8FC7A2448413502FCCBB2C2A139D6E2127BCB7DEE2DF836D4FCD0FC8`) showed
  `UMBRELLA GUARD ACTIVE` feedback.
- App-scoped logcat `p65-gadget-app-logcat.txt` is 264,235 bytes (SHA-256
  `7237171A826BE8F5308B871270F1477551296A903F5E2B5DC626669DAE113E9F`) with zero
  configured fatal, ANR, SIGSEGV, SIGABRT, NullReferenceException or UnityException markers.
  The tutorial overlay appeared after fresh activity starts; the in-app SKIP and CLOSE CARD
  controls were used, so this route is not action-by-action tutorial comfort evidence.

#### P65 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Physical Tiffin Station pickup and use | **Passed by bounded exact-device observation** | `13-tiffin-aimed.png` / `22-tiffin-use-before-umbrella.png`; valid aim and deployment feedback |
| Physical Dhol Burst pickup and use | **Passed by bounded exact-device observation** | `18-left-to-dhol.png` and `19-dhol-use.png`; pickup and `DHOL BURST` feedback |
| Physical Umbrella Guard pickup and use | **Passed by bounded exact-device observation** | `23-diagonal-left-progress.png` and `24-umbrella-use.png`; pickup and `UMBRELLA GUARD ACTIVE` feedback |
| Human all-gadget presentation, comfort, audio and cultural review | **Open** | Device captures do not replace subjective owner review |
| Full match, repeated rematch, normalized performance, endurance and physical 16 KB | **Open** | Existing P61-P64 limits remain unchanged |

The truthful classification remains **prototype / Android offline release candidate in
progress**, not Play-ready. P65 changes no runtime code or APK/AAB artifact. The prompt files
under `PROMPTS/` remain intentional uncommitted owner work.

### P64 - Immediate-after-resume lifecycle pause observation on exact P61 candidate - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` was started on approved
Lava `ST5GDW23LB004392` after tutorial completion. A live Opening Fight baseline was captured,
the app was sent to Android HOME for approximately five seconds, and the Unity activity was
resumed with a screenshot approximately 220 ms later. The route manifest is
`Builds/Local/Device/final-circle-20260830/p64-lifecycle-pause-manifest.json` (3,354 bytes;
SHA-256 `DC3677479D95C4E2EBA7DD79C6E46C03418D58F379AE32708A1C5B2FFCB4EA99`).

#### P64 machine evidence

- `lifecycle-tight-warn4-before.png` is 81,148 bytes (SHA-256
  `7F39DFDEDDB0E559BBDFB402F85D0505CCEF8F4A013577EA2AD4D0F4BAD39D0C`) and shows
  `BIJLI HP 85/85`, `OPENING FIGHT`, `ALIVE 8`, `ZONE 14.0 > 11.0`, `WARN 11.6s`.
  `lifecycle-tight-warn4-after-220ms.png` is 82,140 bytes (SHA-256
  `64EF1598B2D145588B718224CD18F08CE059144CA6F9D58DA03F55E01A9AF3BB`) and shows the
  same live state and zone values with `WARN 10.8s`.
- `lifecycle-tight-warn4-trace.txt` records the baseline, HOME, resume and capture times
  (295 bytes; SHA-256
  `78964CC53E6A81112B44052E6D0A63B6C4CDBA474EC70415DF11937EF58083B0`). The requested
  background window was five seconds; the post-resume capture delay was approximately
  0.74 seconds. App-scoped logcat `p64-lifecycle-app-logcat.txt` is 24,519 bytes (SHA-256
  `BEE20E9200B5C9FFC85D8CF7B5D700B62E418241B8CDDFDED10A6230A857D7C3`) and records
  `APP_CMD_PAUSE/STOP` followed by `APP_CMD_START/RESUME` on the same process, with no
  configured fatal, ANR, SIGSEGV or SIGABRT marker.
- No simulation state or zone value changed across the five-second background interval in
  these paired captures. The warning countdown's roughly 0.8-second movement is consistent
  with the post-resume activity/screenshot delay; this is bounded lifecycle evidence, not a
  claim of long-duration timing or endurance behavior.

#### P64 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Exact P61 HOME/resume lifecycle callback and live-state return | **Passed by bounded exact-device observation** | P64 manifest, paired Opening Fight captures and lifecycle logcat |
| Simulation pause invariant across the measured background interval | **Observed / bounded** | Zone/live state held across five seconds; repeat across phases and held-input cases for final approval |
| Full lifecycle, repeated-match, battery and thermal acceptance | **Open** | P64 is a short plugged observation; endurance and memory-growth testing remain required |
| Normalized FPS, frame-time, GC/GPU and physical 16 KB runtime | **Open** | Existing gfxinfo/4 KB Lava limitations remain unchanged |
| Final authored/accessibility/fun/cultural/performance approval | **Owner/human review required** | Device evidence does not replace comfort or presentation review |

The truthful classification remains **prototype / Android offline release candidate in
progress**, not Play-ready. P64 changes no runtime code or artifact. The prompt files under
`PROMPTS/` remain intentional uncommitted owner work.

### P63 - Exact P61 candidate bounded Lava live-match performance refresh - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` was already installed on
approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34) and was kept in a live
Solo Raja match while `Tools/Validation/capture_android_performance.ps1` requested a
120-second capture at five-second intervals. Movement swipes plus attack, ability and gadget
taps were sent during the sample window. No application data was cleared and no artifact was
rebuilt. The raw capture is under
`Builds/Local/Device/Performance/20260831-lava-f80b565-p62-perf120/`.

#### P63 machine evidence

- The harness wrote **24 samples**. `manifest.json` is 5,611 bytes (SHA-256
  `EBB5C4F43E27E0579E0EAC3E116E8A3DB0F47DEB35EAEDEE7C05A945FF067D7D`), and the checked
  summary is `p63-performance-route-summary.json`, 3,478 bytes (SHA-256
  `4413810D5295345586E497846AA4A55C6B83AC9645FA807EB159ADDEA6B4467B`).
- The opening capture `live-start.png` is 323,756 bytes (SHA-256
  `4FCCA4F9BDFEAD5B3391FCCED6C86D1F1D567290A40DB806168C805D4CBAF8A4`) and shows the
  Bijli live HUD in Opening Fight. The ending capture `live-end.png` is 317,880 bytes
  (SHA-256 `720F5D9361887079AAEB3950509360F92B8EA4D658CA42BC931C1BC94FD4C013`) and
  shows Final Circle, player defeat/spectating and Alive 4.
- Across all samples, total PSS was **223,078-244,059 KB** and RSS was
  **356,732-365,932 KB**. After the first four samples, PSS was **223,078-223,273 KB**
  (average **223,190.8 KB**) and RSS was **356,972-357,168 KB** (average
  **357,085.0 KB**); graphics PSS stayed at **17,480 KB**. `top` reported **59.2-131.0%**
  instantaneous process CPU (warm-sample average **113.8%** on Android's per-core scale).
- Thermal status was **0** before, during and after; HAL CPU/GPU readings were
  **43.537-49.148 C** and skin readings **39.305-39.969 C**. Battery remained USB-powered;
  the service changed from **44% / 3,841 mV / 35 C** to **43% / 3,825 mV / 35 C**.
  The app-scoped logcat is 1,058,413 bytes (SHA-256
  `139ED06981B4524B4C288A47DC0FBDF9859F8DAC2767024C4A3ED252F80112B2`) with no configured
  fatal, ANR, SIGSEGV, SIGABRT, NullReferenceException or UnityException marker.
- Android `gfxinfo` reported zero total frames and no usable Unity SurfaceView frame
  histogram. Lava reports 4 KB pages. This is raw current-candidate telemetry, not a
  normalized frame-time, GC, GPU, battery-endurance or physical 16 KB result.

#### P63 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Exact P61 live-match telemetry and bounded route | **Passed as raw diagnostic evidence** | 24-sample manifest, live Opening Fight to Final Circle/spectator captures, and clean marker scan |
| Warm memory stability during this capture | **Observed / bounded** | PSS/RSS/graphics ranges above; repeated-match growth and budget approval remain open |
| Normalized FPS, frame-time, GC, GPU and render-thread budget | **Open** | Unity `gfxinfo` exposes no usable histogram; use supported profiler/trace interpretation |
| Battery endurance and sustained thermal acceptance | **Open** | This run was USB-powered and short; unplugged endurance and owner criteria remain required |
| Genuine physical 16 KB runtime | **Open** | Lava reports 4 KB pages; the host-GPU AVD smoke is profile-specific |
| Final authored/accessibility/fun/cultural/performance approval | **Owner/human review required** | Raw telemetry does not replace comfort, presentation, fairness or cultural review |

The truthful classification remains **prototype / Android offline release candidate in
progress**, not Play-ready. This P63 refresh does not change the P61 APK/AAB or any gameplay
rule. The prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P62 - Re-run accessibility, persistence and lifecycle route on the exact P61 candidate - 2026-08-31

The exact rebuilt P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` was installed on
approved Lava `ST5GDW23LB004392` after clearing app data. The fresh route exercised menu and
in-match Settings & Accessibility, all five requested accessibility changes, left-handed live
controls, persistence across return/relaunch, reset-to-defaults and a bounded live
background/resume cycle. The route index is
`Builds/Local/Device/final-circle-20260830/p61-accessibility-route/p62-accessibility-route-manifest.json`
(8,901 bytes; SHA-256
`0DAE55EBFC6DD57F78D9BF28D0A9172659102FFB2D30BFBB2ADC8AB610D4BCF9`).

#### Exact-candidate route evidence

- The menu Settings summary captured all defaults, then showed `LEFT-HANDED: ON`,
  `REDUCED FLASHES: ON`, `HIGH CONTRAST: ON`, `AIM ASSIST: ON` and `TEXT SIZE: 110%` after
  the in-match changes. The left-handed live capture visibly moved the action row to the
  opposite side. The settings were toggled back and the post-relaunch summary returned to
  `OFF`/`100%` defaults.
- The route retained 25 selected screenshots, including
  `18-menu-settings-persisted.png` (99,659 bytes; SHA-256
  `31F424FE3F8BC5AB9B49F2C9DA2A09490307ECC088D2F255CA6384FE248CF59D`),
  `15-left-handed-live-retry.png` (311,075 bytes; SHA-256
  `0A088A13908B94ED1639E08F54F85251966A5C729CA2DB1CB50E3C08046AD5DD`) and
  `25-menu-settings-reset-persisted.png` (98,072 bytes; SHA-256
  `D3144806370F530C662E730BDF17C61D4638FF8846012BDCCA2E9BA9B9F9316F`).
- The bounded background/resume pair returned to the same live Opening Fight state. The
  warning timer decreased during the observation window, so this is lifecycle return evidence,
  not a claim that the simulation remained frozen for the entire background interval.
- The UI tree remains SurfaceView-only: `p61-accessibility-ui.xml` is 2,546 bytes (SHA-256
  `A8235CD2CFEE7BFCFB0A515F9337E4ABF6E6C16C10ACDCF446DE87E9AEF094BD`). The app-scoped
  logcat is 24,935 bytes (SHA-256
  `2AA7EA1065F8FA035226E760454961ACC9BE770ACBDA9B7A003F5037ED278C27`) with no configured
  `FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or `SIGABRT` marker. Known Lava gralloc/AHardwareBuffer,
  Play Core class-probe and Swappy diagnostics remain recorded.

#### P62 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Accessibility toggles, left-handed layout, persistence and reset on exact P61 APK | **Passed by bounded exact-candidate observation** | P62 manifest and 25 selected captures; smaller-device coverage and human comfort remain open |
| Live background/resume return on exact P61 APK | **Observed / bounded** | P62 before/after captures return to live Opening Fight; simulation pause invariance and sustained lifecycle testing remain open |
| Exact rebuilt APK/AAB technical checks | **Passed locally** | P61 artifacts, repository validation and release checker above remain unchanged |
| Full action-by-action tutorial, repeated matches, final authored content and human approval | **Open / owner-controlled** | Physical route evidence does not replace action comfort, fun, cultural or accessibility review |
| Sustained performance, battery/thermal, physical 16 KB and Play gates | **Open / owner-controlled** | Lava reports 4 KB pages; existing bounded diagnostics do not replace these approvals |

The truthful classification remains **prototype / Android offline release candidate in
progress**, not Play-ready. The prompt files under `PROMPTS/` remain intentional uncommitted
owner work.

### P61 - Clarify compact results metrics on the portrait HUD - 2026-08-31

The current runtime/test checkpoint is `f80b565372d7446e070cf1a37de042bd018345c4`
(`ui: clarify compact result metrics`). The compact results card now uses the player-facing
`KOs`, `AST` and `DMG` labels instead of the ambiguous single-letter `K/A/D` sequence, and its
portrait result text rises from 16 px to 18 px before the saved text-scale preference is
applied. Placement, damage, rewards and authority state are unchanged. ADR-075 records the
decision and the compact formatter regression.

#### Machine evidence

- `Tools/Validation/validate.ps1` returned **0 errors / 0 warnings**. Log:
  `Builds/Local/Logs/validate-results-copy-20260831.log` (SHA-256
  `7EF09129DBD03921DF243F43AC65AE932A8C74C4DD76FAD8E6A013BFC804E322`).
- Full EditMode returned **141/141 passed**. XML
  `Builds/Local/TestResults/editmode-results-copy-20260831.xml` (SHA-256
  `25F2F5E0E6BC89405D2D5371412C6A9E057C2EEED9C847BB18460B3700A15486`).
- Full PlayMode returned **92/92 passed**. XML
  `Builds/Local/TestResults/playmode-results-copy-20260831.xml` (SHA-256
  `1BE01E5420DD4674598417DF26C4CE9A8470E386CAF442D1C041B56767AA58B9`).
- The rebuilt APK is 40,679,695 bytes (SHA-256
  `922DB673B579BD88705BB4483C36A21A2D903A1CD05D2C2F50F47D26A564EA91`) and the AAB is
  36,504,994 bytes (SHA-256
  `FDBCED4B1D6D69E4F637C283298188B037D58F152DE4D9B69F897147F85093CF`). Build logs are
  `Builds/Local/Logs/android-v1-apk-results-copy-20260831.log` (SHA-256
  `C2159CD2A0165B101E57FB92F60B536CC81C94271274EADC3D468FF10E61A182`) and
  `Builds/Local/Logs/android-v1-aab-results-copy-20260831.log` (SHA-256
  `D1017D1AC97FC908D6B98FB95538058FF2EDB6371AB51150E0082212C69ED0CD`).
- The release checker returned **0 errors / 0 warnings**. Final log:
  `Builds/Local/Logs/release-checker-results-copy-20260831.log` (SHA-256
  `25A9FF28668DAB739B5CD795487783BBCF8668D981FF5891B51EA38B9277AD6F`). It reports package
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, API 28/36, no network permission,
  seven ARM64 native libraries and static 16 KB alignment.

#### Approved Lava exact-candidate route

The rebuilt APK installed successfully on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
Android 14/API 34, reported 4 KB pages). Fresh touch reached menu → Play Offline → Solo Raja →
Bijli → live match, then player defeat/spectator, Aandhi pressure/final circle, Results and
REMATCH. The exact current result capture
`Builds/Local/Device/final-circle-20260830/p60-results-copy-route/05-results.png` is 301,689
bytes (SHA-256
`313F180C6177C5A78F80B68D115C0E52E2E44C3FDB79CA157737B62BADC79676`) and visibly shows
`KOs`, `AST` and `DMG` on every placement row. The fresh rematch capture
`05-rematch-opening.png` is 322,280 bytes (SHA-256
`2137A249A70D9005A563AA259652F672D182C132B96069B21ED2DBB731D2FF26`) and shows
`SPAWN SHIELD` / `ALIVE 8  ZONE 14.0 > 14.0`.

The route index is
`Builds/Local/Device/final-circle-20260830/p61-results-copy-manifest.json` (4,936 bytes;
SHA-256 `0C868F852FE57C409B914871845DF317EFC7C89398CEFD3A8AB98E5F1137671F`). The current
app-scoped log is 15,783 bytes (SHA-256
`D88E416E0487E13919C3C928A10E4372EACA3E624C29A7FC0BC0980FF8BBA833`) with no configured
`FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or `SIGABRT` marker; known Lava gralloc/AHardwareBuffer,
Play Core class-probe and Swappy diagnostics remain recorded. The UI tree is SurfaceView-only
and screenshot-derived coordinates were required.

A second exact-candidate route exercised all three fighter cards and live action checkpoints
on the same rebuilt APK: Bijli, Pehel and Maya selection/opening, attack taps, Tiffin Station
use, and ability feedback after the Opening phase (Bijli dash cooldown, Pehel charge cooldown,
Maya decoy active). Its manifest is
`Builds/Local/Device/final-circle-20260830/p61-all-fighter-manifest.json` (5,506 bytes;
SHA-256 `9FA8762B504330189A686605A6DD60836C4992B5359E526B91E8527A813E1598`). The route's
current app-scoped log is 15,630 bytes (SHA-256
`4F8734236C6CA4D314B38F441FE12617CABE8D85E2C8A04DB0E0E0E8A5DEF0AB`) with no configured
fatal/ANR/SIGSEGV/SIGABRT marker. This is bounded action observation; accessibility comfort,
full tutorial comfort and repeated-match approval remain open.

#### P61 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Compact results metric copy and portrait type size | **Passed by exact source/test/device evidence** | `OfflineMatchHud.FormatResults`, ADR-075, full 141/141 + 92/92 reruns, exact rebuilt Lava result/rematch capture |
| Exact rebuilt APK/AAB technical checks | **Passed locally** | Rebuilt artifacts, repository validation and release checker above |
| All three fighter cards and live action checkpoints on this rebuilt artifact | **Passed by bounded exact-candidate observation** | P61 all-fighter manifest and 15 capture set; action-by-action comfort and sustained play remain open |
| Accessibility settings and left-handed route on this rebuilt artifact | **Open / prior observation remains** | P59 captured the toggles on the preceding copy-only candidate; fresh comfort review remains required |
| Final authored art/audio, localization, cultural, fun and accessibility approval | **Owner/human review required** | Generated presentation baseline remains a candidate |
| Sustained performance, battery/thermal, physical 16 KB and Play gates | **Open / owner-controlled** | Existing bounded evidence and drafts do not replace these approvals |

The project remains a **prototype / Android offline release candidate in progress**, not
Play-ready. The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P60 - Spell out compact zone telemetry for the portrait HUD - 2026-08-31

The current runtime/test checkpoint is `c3cfb27e08f13ecf4b91a4234269aa11e675bfe9`
(`ui: spell out compact zone telemetry`). The portrait HUD previously rendered the zone
label as a one-letter `Z`, which was ambiguous beside the alive count and read like internal
debug text. `OfflineMatchHud` now uses the same player-facing `ZONE {current} > {next}` copy
in compact and wide formats; authority, timing, input and match state are unchanged. The
PlayMode regression now asserts the full label and rejects the old abbreviation. ADR-074
records the rationale and scope.

#### Machine evidence

- `Tools/Validation/validate.ps1` returned **0 errors / 0 warnings**. Log:
  `Builds/Local/Logs/validate-zone-copy-20260831.log` (SHA-256
  `7EF09129DBD03921DF243F43AC65AE932A8C74C4DD76FAD8E6A013BFC804E322`).
- Full EditMode returned **141/141 passed**. XML
  `Builds/Local/TestResults/editmode-zone-copy-20260831.xml` (SHA-256
  `6191ABFE488A415899818C0B4BAC6CD7690A0829B86B2433D8A09C8C5F4F0018`); Unity log
  SHA-256 `94046A10E17E05038C8644FFAD73E9D76033EDB1BA182D87794BE39EDE4A70A1`.
- Full PlayMode returned **92/92 passed**. XML
  `Builds/Local/TestResults/playmode-zone-copy-20260831.xml` (SHA-256
  `8D76395DE9168D1171784615BCB0DA5F4FE08FE035433B17998C6D5CB66B89B4`); Unity log
  SHA-256 `2BBDE2E4CA7E35DF0C1698CDE587F0739F571C081FF8ED9590A7E3E88116B880`.
- The APK entrypoint completed successfully. APK
  `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` is **40,682,347 bytes**,
  SHA-256 `4EFF24C7251DD57C2FCAA4D280C369175D33FA6C8D26B969ABBAA72D9EAF32A7`;
  build-log SHA-256 `AD2DEC322AB36C04BF60FFCB00ECB85168A6BA21DAAADA279D8DEF15B42E31FE`.
- The AAB entrypoint completed successfully. AAB
  `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` is **36,507,651 bytes**,
  SHA-256 `D60B09EE6324C0AA75781BF1F9DB8461A6A1AE05D788A9232EA227DBC1349936`;
  build-log SHA-256 `EE53E5BDE4C6304FF8E5B9B0421769A690283DECBD464D15F542024DB285C2CC`.
- `Tools/Validation/check_v1_release_candidate.ps1` returned **0 errors / 0 warnings**.
  Log `Builds/Local/Logs/release-checker-zone-copy-20260831.log` (SHA-256
  `92C300B878DC5292C9EED5E04D1884B97AA57AF80386054A6D5B224ACCF06895`) confirms package
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, API `28/36`, no network
  permissions, seven ARM64 libraries, static 16 KB alignment and temporary/debug signing.

#### Approved Lava evidence

The rebuilt APK was installed and launched only on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, reported 4 KB pages). A fresh
Play Offline → Solo Raja → Bijli route captured the compact HUD text as
`GET READY` / `ALIVE 8  ZONE 14.0 > 14.0` in
`Builds/Local/Device/final-circle-20260830/p60-live-zone-copy.png` (324,315 bytes;
SHA-256 `13AEFABE9A51364B28B85B6293B2237D6D7189C32278863E591964C252FE8A3D`). The route
index is
`Builds/Local/Device/final-circle-20260830/p60-zone-copy-manifest.json` (3,822 bytes;
SHA-256 `B235CAC4A041644B7A05FED6C613A5BB2563CDD6929C19EF9E2B6F445F1C7E39`). The
app-scoped logcat (15,630 bytes; SHA-256
`82D8E71FA20D555DFB8170A5AEB3B3060674B7EAD37AA07469BB3D592107E23E`) has no configured
`FATAL EXCEPTION`, `ANR in`, `SIGSEGV` or `SIGABRT` marker. Known Lava gralloc/
AHardwareBuffer, Unity Play Core class-probe and Swappy diagnostics remain recorded as
non-fatal platform noise. The UI tree is SurfaceView-only (2,546 bytes; SHA-256
`A8235CD2CFEE7BFCFB0A515F9337E4ABF6E6C16C10ACDCF446DE87E9AEF094BD`).

#### P60 gate delta

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Compact portrait HUD zone label is player-facing | **Passed locally** | 141/141 EditMode, 92/92 PlayMode, and exact Lava capture show `ZONE` rather than `Z` |
| Exact APK/AAB rebuild and Android technical checks | **Passed locally** | Artifact hashes, release checker 0/0, ARM64/static 16 KB and offline permissions |
| Exact Lava launch and compact-HUD route | **Observed / bounded** | Fresh install/launch and live opening on approved Lava; no human visual approval claimed |
| Smaller-device localization, final visual, comfort and accessibility review | **Open** | Tested viewport is 1080x2460; owner review remains required |
| Sustained performance, battery/thermal, memory growth and physical 16 KB runtime | **Open** | Existing data remains bounded diagnostics; Lava reports 4 KB |
| Final identity/signing/privacy/Data Safety/rating/support URL/Play Console | **Owner-controlled** | No irreversible identity, signing, legal or upload action performed |

The project remains a **prototype / Android offline release candidate in progress**, not
Play-ready. P60 improves player-facing compact-HUD copy and adds exact evidence; it does not
close final authored-content, subjective, physical, performance, identity, legal or Play
gates. The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P57 tutorial elimination target readability and real-touch route refresh - 2026-08-30

The current source checkpoint is commit `c9e3d3091a38852be794f74ad97420b91461599a`
(`tutorial: place elimination target in readable lane`). The change is tutorial-scoped: it
places actor 11 at `(0, 1, -3.2)` in the open south lane, keeps the target stationary, and
leaves production spawns, the MovementLab fixture, the offline authority and all package
policy settings unchanged. The PlayMode coverage adds a readable-lane invariant and a local
projectile regression against the real target path.

#### Machine evidence

- `Tools/Validation/validate.ps1`: **0 errors / 0 warnings** after the source checkpoint;
  post-checkpoint log `Builds/Local/Logs/validate-tutorial-target-postcommit-20260830.log`
  (SHA-256 `7EF09129DBD03921DF243F43AC65AE932A8C74C4DD76FAD8E6A013BFC804E322`).
- EditMode: **141/141 passed**, XML
  `Builds/Local/TestResults/editmode-tutorial-target-20260830.xml` (SHA-256
  `3ABEB32CC5A0270F3199ABA3FF81EEA5F24365D7A0921E4411D397E9B5EB042E`).
- PlayMode target checkpoint: **90/90 passed**, XML
  `Builds/Local/TestResults/playmode-tutorial-target-20260830.xml` (SHA-256
  `5215CB2AED45EF29782A9A801CF53F89D7DECF57110CF8B27D05349235A27E7F`).
- Focused follow-up PlayMode suite including the local projectile regression: **91/91
  passed**, XML `Builds/Local/TestResults/playmode-tutorial-local-attack-20260830.xml`
  (SHA-256 `2C65048E75501A3C75DCCD6315453B39C0C1AF3BC973B801BC35D3B9BDBCD327`).
- Exact temporary-ID APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`,
  **40,681,055 bytes**, SHA-256
  `DA6CC4B6B2F4160A2D62BDE9FFA4C1686D0D401AB0F354604AF8AC077269222B`.
- Exact release-shaped AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`,
  **36,506,363 bytes**, SHA-256
  `A379C725D46E8829F9DE9EEF59E49D906E817F9F6392E031EE065A532DD6C37C`.
- `check_v1_release_candidate.ps1`: **0 validation errors / 0 warnings**;
  `Builds/Local/Logs/release-checker-tutorial-target-20260830.log` (SHA-256
  `1D19EB98F4E726DA1BDB4B54D3C9C397718F84DEDBC71E98578F225E72F1C25F`). The package remains
  `com.example.battleraja.m11`, version `1.0.0`/code `100`, min/target API `28/36`, offline
  permissions absent, ARM64-only, statically 16 KB aligned, and temporary debug-signed.

#### Approved Lava real-touch evidence

The exact APK was installed and exercised only on approved Lava `ST5GDW23LB004392`
(`LAVA LXX508`, Android 14/API 34, reported 4 KB pages). The complete machine-readable
index is `Builds/Local/Device/final-circle-20260830/tutorial-target-touch-route-manifest.json`
(5,757 bytes, SHA-256
`75D43FB3C56E18A6706800EB27409258184B7B72F87BC494040494303DCFC7A7`).
Aim Assist was enabled in the in-app Settings & Accessibility surface; the proof capture is
`tutorial-target-settings-check.png` (SHA-256
`C481E30733BFF3892B0ECC41EE11FE1FCEDF8DD8599519353FCD2C980F19BB8B`).

The fresh route completed MOVEMENT, AIM, BASIC ATTACK, ABILITY, GADGET and AANDHI by real
touch. After a short forward movement to align the player's marker with the new open-lane
target, the ELIMINATION card visibly unlocked: `tutorial-target-aimassist-moveclose-step7-final.png`
(SHA-256 `27E22DED8D06035806F3EE85B339286A28019025B20B32BC099BC4BE2B77A76E`). The offline
match then resolved to a real `RESULTS / WINNER YOU / #1` placement
(`tutorial-target-after-wait.png`, SHA-256
`9710C1C2A111652CC79E625D0D46E177BF65FC9490636F3FD6C47FCF088FBC7D`). Tapping `FINISH
TUTORIAL` produced `TUTORIAL COMPLETE 8/8` (`tutorial-target-finished-results.png`, SHA-256
`76E40B9F20BCDAFAE13FF217029CA75A3B92D8B9A23B58F98802C224493BAA49`). This closes the
previous P56 elimination-comfort defect for the observed run and records terminal tutorial
completion, but it does not close full action-by-action victory/rematch comfort.

The route's earlier card captures, retained in the same folder, are:

- MOVEMENT `tutorial-target-fresh-step1-moved.png` —
  `46D9E7B6DE60E925A8AB48704FFC4ADFC65D5AD04194094325E0A3FB64409CC7`.
- AIM `tutorial-target-fresh-step2-aimed.png` —
  `BDF0FAFE8EC2C8C66F6BB293048B2AED417D34823D579B6B5D731D234D58B507`.
- BASIC ATTACK `tutorial-target-fresh-step3-attacked.png` —
  `4552D81480F0CC266A991E3CC917C24D136BFF0A6A6EE5A3D0F2EFD2944F4232`.
- ABILITY `tutorial-target-fresh-step4-ability.png` —
  `23D07DADCC3AD9B2C35002B1A502AA8C4909DA9FBF6FB788AABA1765F6C1FD60`.
- GADGET `tutorial-target-aimassist-moveclose-step5-gadget.png` —
  `13FFE4B026AE1AB56AF440B181EFB95EE9E4A12E8E4DE5B7A73147E1F201CBD8`.
- AANDHI `tutorial-target-aimassist-moveclose-step6.png` —
  `A81AEA6B3FDE73B65842495774FA8CF568285591CB4627D8433298076CB0AF5A`.

The Android UI tree remains a Unity `SurfaceView` only, so visually derived touch
coordinates were used and no semantic-locator claim is made. The final route UI tree is
`tutorial-target-route-ui.xml` (2,546 bytes, SHA-256
`A8235CD2CFEE7BFCFB0A515F9337E4ABF6E6C16C10ACDCF446DE87E9AEF094BD`). The app-scoped
logcat is `tutorial-target-route-app-logcat.txt` (884 bytes, SHA-256
`F7B3FE5885A0760F5F9E381254F4A770E49DB34BA9F853D19D90A7133DAEFC7D`); it has no configured
fatal/ANR/native-crash markers and contains one non-fatal BLASTBufferQueue max-frame warning.
The exact paths and observations are recorded in the manifest.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Tutorial movement through AANDHI by real touch | **Observed locally** | Exact Lava captures and manifest above |
| Tutorial elimination by real touch | **Observed locally on refreshed target layout** | Step-7 card unlocked after target alignment; repeat on human-owned candidate for comfort approval |
| Tutorial victory/result by real touch | **Observed locally** | Terminal `RESULTS / WINNER YOU / #1` and `TUTORIAL COMPLETE 8/8` captures above |
| Full action-by-action victory comfort and rematch on this refreshed route | **Open** | Repeat with human-owned comfort review and exercise the underlying REMATCH control |
| Exact source, tests, APK/AAB, offline manifest and static alignment | **Passed locally** | Machine evidence above; temporary signing only |
| Genuine 16 KB runtime | **Open** | Lava reports 4 KB pages; static alignment is not runtime proof |
| Sustained performance, thermal, battery, GC/frame-time and repeated rematches | **Open** | Existing observations remain bounded, not normalized acceptance |
| Final authored art/audio, cultural, fun and accessibility approval | **Owner review required** | Generated presentation baseline remains a candidate |
| Package identity, release signing, privacy/Data Safety, IARC/content rating and Play Console | **Owner-controlled** | No upload or public deployment performed |

The product remains a **Prototype — Android offline release candidate in progress**, not
Play-ready. P56 remains the prior-candidate comfort record; this P57 checkpoint supersedes its
tutorial target layout while preserving its evidence. The two prompt files under `PROMPTS/`
remain intentional uncommitted owner work.

### P56 current-candidate real-touch tutorial comfort probe - 2026-08-30

This probe used the exact current candidate built from runtime/audio source `56df201` on the
approved Lava `ST5GDW23LB004392` only. Aim Assist was enabled through the in-app Settings &
Accessibility surface before replaying TutorialArena. Because Android UI automation exposes
only Unity's `SurfaceView` for this build, the route used visually derived control coordinates;
the captured UI tree is retained as evidence rather than treated as a semantic locator.

The real-touch route advanced the first six tutorial lessons:

- MOVEMENT: `tutorial-aimassist-step1-moved.png` (SHA-256
  `09AD92A4CC207B605AB88A16C731F2086040E96316FCBA684BBC63C97A3B25DD`).
- AIM: `tutorial-aimassist-step2-aimed.png` (SHA-256
  `6093A1988D2F26FFB68ECD239479E27B8E01E9847FC25E3A5958ABF8E7C744F5`).
- BASIC ATTACK: `tutorial-aimassist-step3-attacked.png` (SHA-256
  `64D6471FBEA782CD5494AD0B15B6A6CF1A54167CAA0658DB953109546CB30003`).
- ABILITY: `tutorial-aimassist-step4-ability.png` (SHA-256
  `E83A435B3C59CEA1EC8BC37852F1E332A31A3E0B6EFAB31BEBEB8D8377B00F9D`).
- GADGET: `tutorial-aimassist-step5-gadget-first.png` (SHA-256
  `C200A518521A3421ACF91F0816FA68F5C348BCD86FE0FBB918EDB4BF073C841E`). The nearby Tiffin
  is authoritatively reconciled before this lesson (covered by
  `PreCollectedGadgetIsReconciledWhenGadgetLessonBegins`); a real gadget touch produced the
  station and enabled CONTINUE.
- AANDHI: `tutorial-aimassist-step6-aandhi.png` (SHA-256
  `9D0DA7BF5862E56114A6DF0C8624E2BE4BB37EA135D28736D8ABB8F065703D4C`).

The ELIMINATION lesson did **not** advance after several real aim/attack attempts. The latest
waiting-state capture is `tutorial-aimassist-close2-step7-single-leftup.png` (SHA-256
`0C4616071D940071F64C56E08CAE8C4C11302281CD4A8F8EF2032691827706CA`); an additional burst
attempt is `tutorial-aimassist-step7-tap-burst.png` (SHA-256
`163BCC182038A22BBD76F575257E9E36232810303AC0B0F1A7E981EEABA30310`). A later terminal panel
showed `WINNER YOU` and `#1 YOU` with `KO 0` / `D 0` in
`tutorial-aimassist-step7-attack-left.png` (SHA-256
`A87735215BB1F8F1FAED9EF2A2C975C3791401C5D5F0CC0DCE114886631CBBC1`), indicating that Aandhi
resolved the route without satisfying the player-elimination lesson. No action-by-action
elimination or victory comfort claim is made. The earlier P47/P54 physical `8/8 COMPLETE`
route used the in-app SKIP control; editor tests cover deterministic eight-step progression,
but neither substitutes for the missing real-touch elimination review.

The Settings capture proving the probe configuration is
`tutorial-settings-aim-assist-on.png` (SHA-256
`9100144E6411C2BA9D6C2D0B40FD036A2849946BED6D1E68F82AD5D7069C5F56`). The final UI tree is
`Builds/Local/Device/final-circle-20260830/tutorial-route-ui.xml` (SHA-256
`A8235CD2CFEE7BFCFB0A515F9337E4ABF6E6C16C10ACDCF446DE87E9AEF094BD`), and the app-scoped
logcat is `tutorial-route-app-logcat.txt` (1,693 bytes; SHA-256
`909FDF92B11825C3229670134770FE238A23069141B871FBDF77DA54D85B1DF4`) with no configured
fatal/ANR/native-crash markers. The complete machine-readable index is
`Builds/Local/Device/final-circle-20260830/tutorial-touch-route-manifest.json` (5,200 bytes;
SHA-256 `5ACCBA1BCDD4CDC4563AFB5F137F50544F1E8442804379D8B49895C1EC2E3863`).

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Tutorial movement, aim, attack, ability, gadget and Aandhi lessons by real touch | **Observed locally** | Six exact Lava captures listed above; Aim Assist setting recorded. |
| Tutorial elimination and victory comfort by real touch | **Open** | Elimination remained waiting and the terminal result had KO 0; reproduce on a human-owned candidate and review control readability/feedback. |
| Tutorial 8/8 through SKIP and deterministic editor progression | **Passed as scoped evidence** | P47/P54 physical SKIP route and `TutorialArenaPlayModeTests`; not action-by-action comfort proof. |
| Exact candidate, crash-marker smoke and offline route baseline | **Passed locally / observed** | P55 artifacts and this probe's clean app-scoped logcat; Lava reports 4 KB pages. |
| Final authored art/audio, human cultural/fun/accessibility approval, normalized performance, battery/thermal, physical 16 KB, signing, legal/privacy, rating and Play Console | **Open / owner-controlled** | No local evidence closes these gates. |

The truthful classification remains **Prototype — Android offline release candidate in
progress**, not Play-ready. The two prompt files under `PROMPTS/` remain intentional
uncommitted owner work.

### P55 - Current-source final-circle audio cue and exact Lava endgame capture - 2026-08-30

The current source tip is commit `56df201` (`audio: add final-circle escalation cue`). It adds
the owned generated `ZoneFinalCircle.wav` cue, loads it through `BattleRajaAudioDirector`, and
plays it once when the authoritative `MatchPhase` enters `FinalCircle`. No gameplay authority,
damage, cooldown, movement, pickup, reward, networking or package-policy rule changed. The
audio bible and provenance record the generated source and leave final human mix/loudness/voice
review open.

#### Machine evidence

- `Tools/Validation/validate.ps1`: **0 errors / 0 warnings**.
- EditMode: **141/141 passed**, XML
  `Builds/Local/TestResults/editmode-v1-final-circle-20260830.xml` (SHA-256
  `601B03047869E4C9AEB5FA8B51520400DAB9D2F369A22F14619D8345FC9C2F11`).
- PlayMode: **89/89 passed**, XML
  `Builds/Local/TestResults/playmode-v1-final-circle-20260830.xml` (SHA-256
  `637991EA95D8399BCD5A7A5E090B7F486BB07B799C473B50520144FC78AC24A6`), including the
  owned-audio-clip contract with `ZoneFinalCircle`.
- Generated cue: `Assets/BattleRaja/Resources/Audio/V1/ZoneFinalCircle.wav`, 37,088 bytes,
  SHA-256 `269DD92C83A3592DDA9AE7F186C76A9D25C9F9BEFF882897DD3F6727581F4F85`.
- Exact temporary-ID APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`,
  40,681,059 bytes, SHA-256
  `AB4974445DA2BAEB023DBCEB5EFF557F161A53F25695B0FD9BD417045FF29855`.
- Exact release-shaped AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`,
  36,506,374 bytes, SHA-256
  `E1658F47D855693FB8F281385EB21176CA4E81C19D86554FC70F91FD94A7F90E`.
- Both packages report `com.example.battleraja.m11`, version `1.0.0`/code `100`, min/target
  API `28/36`, ARM64-only native payload, no `INTERNET` or `ACCESS_NETWORK_STATE`, seven
  ARM64 native libraries and static 16 KB ELF alignment. The post-commit checker log
  `Builds/Local/Logs/release-checker-v1-final-circle-postcommit-20260830.log` is **0 errors /
  0 warnings**, SHA-256 `50DF0FD36AC182E2444E1C89A24BC1A955CB5F1CFAB3C115B260A07D861D614A`.
  The checker reports the two intentional prompt-file changes as the only dirty worktree
  entries; the packages are temporary/debug-signed candidates, not production-signed builds.

#### Approved Lava evidence

The exact APK installed successfully with `adb install -r -d` only on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). The evidence folder is
`Builds/Local/Device/final-circle-20260830/`. Captures show the branded menu, Solo Raja,
all three fighter cards, selected Pehel and Maya detail states, portrait settings toggles and
restoration, live opening, combat actions, player defeat/spectating, `FINAL CIRCLE` with the
cyan endgame ring, and the Results placement panel. The exact final-circle capture is
`midmatch-120s.png` (SHA-256
`55152F4A7DDE2E980E7442B8F0D2359A4F0D76673A3F2019C420237324F87EFB`); the subsequent Results
panel is `final-circle-60s.png` (SHA-256
`BA062B378221F2BF83115C03FEEABF6DEA9DC873B2FB748CABDF0E873CE328C2`). Other key capture hashes are
`live-opening.png` `8BC68F1F41F025ECF997FA0DB21FFA7B7F0BD34106A831B4730AA85ABB458D2C`,
`combat-actions.png` `51AF19BC7BC2D4871E7505256A545E40A88F8DA634CAEE868C33E6088EC82448`,
`settings-toggles.png` `C820A4D388BBBBEC9B6B6479AB9C5142470CB828B3D430C67C8170D1FA12FDC5`, and
`settings-restored.png` `3006D20CA7775F230705371B9B206B28521E2E521B7044A8D7F58D6A84739F39`.

The route's filtered app-process log contains **144 lines** and no configured
`AndroidRuntime`, `FATAL EXCEPTION`, `ANR in`, `SIGSEGV`, `SIGABRT`, `NullReferenceException`
or `UnityException` marker (`post-actions-app-logcat.txt`, SHA-256
`001E3A26040D1E39D3BE0417BE0CFCF573D4713F674BA17CC1F3CD5A253DA09F`). A separate HOME →
monkey relaunch capture returned to the app without configured fatal markers;
`lifecycle-app-logcat.txt` SHA-256 is
`7112BD61F851CF84996502CCFD3BF9CA9FD14C0E2B6A8EE91806CA4757DD923A` and the route record
`lifecycle-route.txt` SHA-256 is `A9A450A4C4C53F5923F505D3C4BC07E2903BA4F15A16489A5A5B3E16B527432C`.
The Lava phone reports 4 KB pages; this is not physical 16 KB runtime evidence. The Unity
SurfaceView exposes no semantic UI tree, so the route coordinates were visually verified and
the capture does not claim action-by-action tutorial comfort or human accessibility approval.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Final-circle audio source, authoritative phase transition and owned-clip regression | **Passed locally** | Generated WAV, source wiring and 89/89 PlayMode contract; final mix/loudness/voice review remains open |
| Exact source, tests, APK/AAB, offline manifest, ARM64 and static alignment | **Passed locally** | 0/0 validation, test XMLs, package hashes and post-commit checker above |
| Approved Lava final-circle/results/lifecycle smoke | **Passed locally / observed** | Exact APK and current screenshot/logcat folder above; full comfort, repeated-match and tutorial action review remain |
| Genuine physical 16 KB runtime | **Open** | Lava reports 4 KB; P49 host-GPU Android 16 AVD is profile-specific |
| Normalized FPS/frame-time/GC/GPU, unplugged battery and sustained thermal acceptance | **Open** | Existing captures are bounded diagnostics, not budget approval |
| Final authored art/audio, cultural, fun and accessibility approval | **Owner review required** | Generated presentation baseline and audio remain candidates |
| Package identity, release signing, privacy/Data Safety, IARC/content rating and Play Console | **Owner-controlled** | No upload or public deployment performed |

The candidate remains a **prototype / Android offline release candidate in progress**, not
Play-ready. The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P54 - Current-source saved-fighter presentation and portrait settings refresh - 2026-08-30

The current presentation source is committed at `ae0d294c97fa62386317e7e5ebf77cd5ebcbafee`,
following the saved-identity integration in `3d8fda7`. `FighterPresentation` keeps the legacy
root capsule as an emergency fallback, suppresses direct root `MeshRenderer` components when
a saved Bijli, Pehel or Maya identity prefab supplies mesh/skinned renderers, and preserves
each saved renderer's base color after hit/elimination flash reset. Hit and elimination tinting
uses the saved mesh renderers through `MaterialPropertyBlock`, while the root `TrailRenderer`
remains available for Bijli's dash telegraph. `OfflineMatchHud` presents a centered portrait
settings modal (`0.06..0.94` width, `0.10..0.90` height) and retains the side-sheet layout for
wide screens. Gameplay authority, collision, input, timing, networking and reward code are
unchanged. The decision is recorded as ADR-071.

#### Machine evidence

- Repository validation: **0 errors / 0 warnings**.
- EditMode: **141/141 passed**. XML `Builds/Local/TestResults/editmode-v1-color-reset-20260830.xml`
  (109,805 bytes; SHA-256 `5F8618A00354B574DCB5990615B7616A84E667153F05B997324D5B53E5B2299F`);
  log SHA-256 `44EBEF77C680967E819E98C3A62FB490620FE7BA87BCAF8324594A91E1308709`.
- PlayMode: **89/89 passed**. XML `Builds/Local/TestResults/playmode-v1-color-reset-20260830.xml`
  (78,381 bytes; SHA-256 `8C274CA588B96D79436F2E3CFC4D5B365EF2A293E8DE31E7ABC91FA4EB156F36`);
  log SHA-256 `70AB6ED5B1354AE1D9457CAA28388A989636DEA2B9AA753BA2C2031C8FC07099`.
  The suite now asserts that each production fighter is using saved tintable renderers and
  that any legacy root `MeshRenderer` is disabled.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, **40,676,742 bytes**,
  SHA-256 `1A8E1CA746E9209F404B52920B1D2A5B7BF2BCFC0FE50F5A56525EBE36D489ED`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, **36,501,907 bytes**,
  SHA-256 `F14D1D592662DDFFE9EF9FD33BAD2D07A5BE85E25A3D4F11972D22D37B678891`.
- APK build log `Builds/Local/Logs/android-v1-apk-color-reset-20260830.log` (420,293 bytes;
  SHA-256 `FD25A8E259FF074811114CDD38E37E665FF200CE813B8D5E9AD3B5A4F011751B`); AAB build log
  `Builds/Local/Logs/android-v1-aab-color-reset-20260830.log` (419,447 bytes; SHA-256
  `B6E9A66FB7EEEC0220A303DAADAA97EB55AEFA251798BB3000C99577D12C2209`).
- Final audit release checker log `Builds/Local/Logs/release-checker-v1-final-audit-20260830.log`
  (3,245 bytes; SHA-256 `815C2812F4C519D91BC1573835F5C5350585E8B9ED3D7A74DAA4EF6C308AC971`)
  passed the offline manifest gate, ARM64-only/static 16 KB alignment gate and store
  creative dimension gate. It reports package `com.example.battleraja.m11`, version
  `1.0.0`/code `100`, min/target API `28/36`, VIBRATE plus Unity's dynamic receiver
  permission, and no network permissions. The signer remains temporary/debug-only.

#### Approved Lava smoke

The exact APK installed and launched on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
Android 14/API 34). A fresh route reached the branded menu, Solo Raja, the fighter cards,
live opening match and settings. The menu capture
`Builds/Local/Device/lava-v1-color-reset-menu-20260830.png` (71,755 bytes; SHA-256
`217984A80310452CDE4C0BBD804B255509376BAA47D01483CF5A28FEEB0EED43`) and live capture
`Builds/Local/Device/lava-v1-color-reset-live-20260830.png` (324,777 bytes; SHA-256
`CACAA52627DCBE7DF82414917519F15180280965E7765BBDE73EA1BBB81F0878`) show the current
candidate. The fighter-selection capture
`Builds/Local/Device/lava-v1-color-reset-selection-20260830.png` (89,725 bytes; SHA-256
`90F6750AD276150607A0D466F3421471928F92EB80E55FAE89F11EE309B57912`) shows all three
distinct fighter cards and their ability summaries; the clean menu settings capture
`Builds/Local/Device/lava-v1-color-reset-menu-settings-20260830.png` (104,506 bytes; SHA-256
`70295A74F05C4EADFB1C820543C773D071967B1DE4ACC63E22C1D992965B78D4`) shows the centered
portrait settings surface. App-scoped logcat
`Builds/Local/Device/lava-v1-color-reset-settings-logcat-20260830.txt` (15,477 bytes; SHA-256
`C2665DD1058B5029B43C1CAE5CF8CD808F209B028F53DC71EAA94BEF7A3CFDBC`) had no `FATAL EXCEPTION`,
`AndroidRuntime`, `SIGSEGV`, `SIGABRT` or ANR marker. The exact-final lifecycle log
`Builds/Local/Device/lava-v1-color-reset-lifecycle-20260830.txt` (387 bytes; SHA-256
`EEF4118A2736B1DA0D2EF6F6AEE4DB529C7C4A250CD2B6E3F75A68EF712565C1`) records HOME moving
to the launcher, relaunch returning `UnityPlayerGameActivity` top-resumed, and zero app-scoped
fatal markers. Oppo was not used.

A fresh 180-second Lava capture ran with 36 five-second samples under
`Builds/Local/Device/Performance/v1-color-reset-180s-20260830/`; the manifest is 4,927 bytes
(SHA-256 `D2F8FF8021094911A585444F9639E32B2E08C45993A3253B8B3E4CEBE36C054B`) and the
logcat is 1,110,450 bytes (SHA-256
`144C41A54EB24235028C014963D6DF2472DAD0392A5A2E571A82F29B329E5B34`). No configured fatal
markers were found. Warm-up-excluded samples 11-36 measured **238,249-244,525 KB PSS**
(average **241,729 KB**), **360,572-366,836 KB RSS** (average **364,046 KB**),
**70,288-74,396 KB graphics PSS** (average **72,267 KB**), and a constant reported battery
temperature of **34 C**. `top` sampled the app at **35.7-62.5% CPU** (average **57.8%**;
Android's 100%-per-core scale). The phone was USB-powered and reports 4 KB pages, so this
is bounded stability telemetry rather than normalized FPS/GC/GPU approval, unplugged battery
endurance, or genuine physical 16 KB runtime evidence.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Saved fighter identity is the visible production surface | **Passed locally** | PlayMode renderer assertion and Lava live capture show saved faceted profiles; ADR-071 |
| Portrait settings readability | **Passed locally / owner review remains** | Centered modal in exact Lava capture; owner still reviews touch comfort and accessibility |
| Exact tests, APK/AAB manifest, offline permissions, ARM64/static 16 KB and store dimensions | **Passed locally** | P54 XML/log/checker hashes above |
| 180-second current-source Lava stability smoke | **Passed as bounded diagnostic** | 36 samples, no configured fatal markers; USB-powered and not a normalized performance pass |
| Genuine 16 KB physical runtime, normalized frame/GC/GPU budgets, unplugged endurance | **Open** | Lava reports 4 KB pages; P49 host-GPU AVD is profile-specific evidence |
| Final authored art/audio, cultural/fun/accessibility approval | **Owner review required** | Generated presentation remains a V1 technical baseline |
| Final package identity/signing, privacy/Data Safety, IARC/content rating and Play Console | **Owner-controlled** | Drafts/checklists prepared; no upload or public deployment performed |

The candidate remains a **prototype / Android offline release candidate in progress**, not
Play-ready. The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.

### P51 - Exact current release-handoff documentation tip - 2026-08-30 07:05 IST

This docs-only continuation is aligned with `origin/main` and does not change the runtime source,
candidate artifacts or P47-P50 evidence. The current owner handoff now has one explicit
draft for release notes, invited-tester steps, known issues, support copy and the final
submission checklist at `Docs/RELEASE/V1_RELEASE_NOTES_AND_SUPPORT_DRAFT.md`. The metadata
draft no longer calls the saved production presentation “procedural placeholders”, and the
older Web-inclusive store copy is marked historical/superseded for the V1 Android scope.

- `Tools/Validation/check_store_creative.ps1 -ScreenshotDirectory Docs/Store/V1` — all
  supplied icon, feature-graphic and screenshot dimensions pass.
- `Tools/Validation/validate.ps1 -ProjectRoot C:\Projects\BattleRaja -RequireUnityProject
  -UnityExe C:\Program Files\Unity\Hub\Editor\6000.5.6f1\Editor\Unity.exe` — **0 errors /
  0 warnings**.
- `git diff --check` and `git lfs fsck --pointers` — pass.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Release notes, tester, known-issues and support draft | **Prepared locally** | New owner-selectable draft; replace support destination and approve before publication |
| Current Play metadata wording and V1 scope | **Clarified locally** | Metadata links the new handoff; historical Web copy is explicitly superseded |
| Runtime/build/test evidence | **Unchanged / still current** | P47-P50 exact `5d136fb` candidate evidence remains authoritative |
| Final identity, signing, legal/privacy, cultural/accessibility/fun review and Play Console | **Owner/human gate required** | No submission, signing-key handling, questionnaire or deployment was performed |

### P53 - Current exact-source 50x production-bot release-gate refresh - 2026-08-30 08:03 IST

The current fixed-tick production-bot harness was rerun from documentation tip
`7167f33839a15a0f0c7e11eb27a97302f93c0b37` with Unity `6000.5.6f1`,
`BATTLERAJA_PRODUCTION_BOT_MATCHES=100`,
`BATTLERAJA_PRODUCTION_BOT_ASSERT_RELEASE_GATES=1`,
`BATTLERAJA_PRODUCTION_BOT_PLAYBACK_SCALE=50`, and base seed `9101`. The runtime/art
source is unchanged from `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`; the later source
change is test-only Umbrella Guard coverage. The run uses the actual `BazaarBastion`
scene and the canonical 30 Hz fixed-tick driver.

#### P53 machine evidence

- NUnit PlayMode report: **89/89 passed**, 0 failed, 0 skipped, duration **268.1340989 s**.
  XML `Builds/Local/TestResults/production-bot-50x-current.xml` (78,474 bytes; SHA-256
  `329FCEA04321DFDFEBD640537CD34C78772F9653506813B34CE4D2D62585DB77`); Unity log
  `Builds/Local/Logs/production-bot-50x-current.log` (167,249 bytes; SHA-256
  `5B5D76684E977E13578AC93ED68B082835D76FDE02E36BD804A740F4928FBB2D`). The passing
  test includes `ProductionBotHarnessPlayModeTests.Harness_CompletesSeededMatches_ThroughProductionPipeline`
  and the release assertions were enabled.
- Batch report `Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260830-022934170-9101.json`
  (1,823,815 bytes; SHA-256
  `EFAD6078F7C3BFFD8563CC4D758135CA77511F63BA2795681A4C92D3DE4EFEBA`). It records
  `CapturedAtUtc=2026-08-30 02:32:54 AM`, `PlaybackScale=50`, `SceneName=BazaarBastion`
  and 100 seeded match records.
- **100/100** matches reached terminal results within the 10,800-tick budget and every
  duration was **306.0135193 s**, so **100/100** landed in the 240-360 second window.
  All **100/100** had at least one combat elimination and bot-to-bot damage; Aandhi-only
  resolutions were **0/100**. Aggregate combat eliminations were 362 and Aandhi
  eliminations 64.
- Protected-warmup damage and invalid-position samples were both **0**. There was one
  stuck recovery, maximum continuous stuck duration was **10 ticks (0.333 s)**, and the
  maximum outside-participant count was 5. Attack telemetry accepted **15,512/15,512**
  attacks with 7 out-of-range diagnostic attempts. Ability telemetry accepted
  **28,152/34,726** attempts (6,574 rejected). Successful contextual gadgets were
  **300/496**, with Umbrella Guard, Dhol Burst and Tiffin Station each used in all 100
  matches.

This fresh current-source run supersedes the stale P15/P16 timing observations for the
fixed-tick production harness; those historical reports remain retained for auditability.
The 50x path is still an accelerated diagnostic and is not used as same-seed command-stream
determinism evidence. The real-time same-seed result in P10 remains the determinism record.

#### P53 gate delta

| Gate | Classification | Evidence / limitation |
| --- | --- | --- |
| Current exact-source 100-match terminal, pacing and safety assertions | **Passed locally** | 100/100 terminal and in-window; release assertions enabled; batch/XML/log hashes above |
| Combat, bot-to-bot and fighter/gadget distribution | **Passed locally** | 100/100 combat-positive and bot-to-bot; 0 Aandhi-only; all three gadgets used in all 100 matches |
| Accelerated same-seed command-stream determinism | **Not claimed** | 50x remains frame-scheduling-sensitive; use P10 real-time evidence for this separate gate |
| Human bot fairness, touch comfort, accessibility, presentation and sustained device budgets | **Owner/human gate required** | Automated fixed-tick telemetry cannot replace full route, balance, authored-content or performance review |

### P52 - Player-facing Umbrella Guard route regression - 2026-08-30 07:21 IST

This test-only continuation is based on commit `4c4c67cbbc20062e3723cc90ee3bb7c266bbeda4`,
which adds direct PlayMode coverage for collecting and using the player-facing Umbrella Guard
through the existing MovementLab authority path. The regression asserts successful pickup and
use, one-slot consumption, the configured shield duration, player feedback, the success
telemetry counter, front-facing projectile mitigation and the Aandhi bypass. No runtime-bearing
gameplay, art, package, manifest or build input changed;
the exact `5d136fb` APK/AAB and P47-P50 device evidence remain the release-candidate artifacts.

#### P52 machine evidence

- Repository validation: **0 errors / 0 warnings** from `Tools/Validation/validate.ps1`.
- EditMode: **141/141 passed** in **3.1482472 s**. XML
  `Builds/Local/TestResults/editmode-umbrella-mitigation.xml` (109,806 bytes; SHA-256
  `F04ADFE67CF5AB0277E38B258066CC19BCCC457F26240803092D84F8BEA520B7`); log
  `Builds/Local/Logs/editmode-umbrella-mitigation.log` (35,805 bytes; SHA-256
  `63D1E76E775DE4CACB7FE3477EF855D9911B8C35312E07B77405F52478A531F7`).
- PlayMode: **89/89 passed** in **71.8220509 s**. XML
  `Builds/Local/TestResults/playmode-umbrella-mitigation.xml` (78,376 bytes; SHA-256
  `B980C51E835CCE4B4047423D2996A7E49D49413D665FDD250E72B43FF939B44F`); log
  `Builds/Local/Logs/playmode-umbrella-mitigation.log` (110,661 bytes; SHA-256
  `47A16D698B3E320AF799073CBA2059E65E39DA0228293F57AABB950887824E88`).
- `git diff --check` passed. The two prompt files under `PROMPTS/` remain intentional
  uncommitted owner work.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Player-facing Umbrella Guard pickup/use/mitigation route | **Passed locally** | New `GadgetPlayModeTests.PlayerCanCollectAndUseUmbrellaGuard` regression; full PlayMode 89/89 |
| Domain, authority and production-bot gadget coverage | **Unchanged / retained** | Existing EditMode, production-bot and production-scene coverage; no rule change |
| APK/AAB and device evidence | **Unchanged / retained** | P47-P50 exact `5d136fb` runtime/art candidate remains authoritative; no rebuild needed for test-only source |
| Final authored/accessibility/fun/cultural/performance approval | **Owner/human gate required** | Test evidence does not replace subjective review or sustained device acceptance |

### P50 - Exact-candidate Lava live-match SurfaceFlinger diagnostic - 2026-08-30

The exact terminal-outcome candidate from source `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`
was relaunched on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34)
through the documented Rematch route. A SurfaceFlinger latency ring-buffer sample was
cleared and collected for approximately 45 seconds while the player was in a live Solo
Raja match. The player was defeated during the sample; the end capture remains in the
spectator state while Aandhi is closing.

#### Machine evidence

- Raw evidence is under `Builds/Local/Device/Performance/20260830-lava-5d136fb-sf/`.
  `summary.json` is 1,847 bytes with SHA-256
  `21369E4FC3BF33BF1DB234BE2F23F1A8D32BD45D0DF29F8682DC90D17489B144`; the raw
  `surfaceflinger-latency-live-45s.txt` is 6,239 bytes with SHA-256
  `D83D61790C60E5D76CB9BBC5B0D25CA91D0AD044BC63686DAD417F71942B3D26`.
- The 16.666667 ms refresh period produced **126 valid present timestamps** and **125
  intervals** after excluding one `Long.MaxValue` sentinel. The middle timestamp column
  is the present-time series, matching the earlier P21 diagnostic. Min/median/p95/p99/max
  intervals were **16.447 / 16.534 / 16.565 / 33.078 / 33.367 ms**; three intervals were
  over one refresh period and one was over 2×.
- The fresh opening screenshot is `rematch-start.png` (SHA-256
  `5B7E42E4AD6DC1EE333DEE902D4E5F8877AB8CEF944CF1D0E00D480B751A1A80`); the live end
  screenshot is `live-end.png` (SHA-256
  `E8E27A5A92CB8098F28A235885AF64133EFE70EC8DA7635956D14230F98CA973`). End-of-sample
  Android telemetry was **277,284 KB PSS / 400,500 KB RSS / 80,052 KB graphics PSS**,
  battery **75% / 4,120 mV / 31 C** while USB-powered, thermal status **0**, and zero
  configured fatal/ANR/SIGSEGV markers.
- Android `gfxinfo` still has no usable Unity frame histogram. This is a bounded compositor
  diagnostic, not Unity Profiler data, normalized FPS/frame-time/GC/GPU approval, or a
  physical 16 KB result; Lava reports 4 KB pages.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Live-match SurfaceFlinger frame-present stream | **Passed as raw diagnostic evidence** | 126 valid present timestamps; summary and raw-file hashes above |
| Normalized FPS, frame-time, GC, GPU and render-thread budget | **Open** | Unity `gfxinfo` has no usable histogram; use supported Unity/Perfetto tooling |
| Battery endurance and sustained thermal acceptance | **Open** | Unplugged, longer repeated-match capture with owner criteria |
| Genuine 16 KB runtime | **Open on Lava** | Lava reports 4 KB pages; P49 host-GPU AVD smoke is profile-specific |
| Final authored/accessibility/fun/performance approval | **Owner review required** | Raw compositor telemetry does not replace human review |

### P49 - Genuine 16 KB Android 16 AVD host-GPU smoke - 2026-08-30

The exact P47 terminal-outcome APK (`31D982D7334B08D0DE759CE755784547CFCF843D9CFCFB1DB0E041E7EEE2DF2D`)
was installed successfully on the genuine `BattleRaja_16K` Android Virtual Device using the
host-GPU renderer. The environment reports model `sdk_gphone16k_x86_64`, Android 16/API 36,
`ro.product.cpu.abilist=x86_64,arm64-v8a` and `getconf PAGESIZE=16384`; the package remains
`com.example.battleraja.m11` with version `1.0.0`/code `100` and the ARM64 native payload from
the release candidate.

- The exact install evidence is `Builds/Local/Device/Performance/20260830-16k-5d136fb/install.txt`.
  A clean direct relaunch produced the normal branded menu in
  `Builds/Local/Device/Performance/20260830-16k-5d136fb/host-gpu/launch-final.png`
  (SHA-256 `919BA18BBCA77C4C843DD07EC1470E8D0DFAE4AC3C3F012266E102ACABD55FA0`). Its
  app-scoped logcat has no configured fatal, ANR, SIGSEGV, SIGABRT or shader-link marker
  (SHA-256 `28379F7FE3650BD8C0B9013802242055D06416277E23A6EEAF49747E6F6DF8F6`).
- The repository performance harness captured **18 samples at 5-second intervals over
  90 seconds** under `Builds/Local/Device/Performance/20260830-16k-5d136fb-host/`;
  manifest SHA-256 is `AC691AF0BB69983AFE0001F87A4AF92543454D3F190C61FB974734A42EE48B61`.
  Warm-up-excluded samples 11-18 measured **435,726-436,966 KB PSS** (average
  **436,659 KB**), **617,304-621,236 KB RSS** (average **618,586 KB**) and a constant
  **31,416 KB GraphicBufferAllocator estimate**. `top` reported **96.1-123.0%** process
  CPU (average **104.5%**, Android's 100%-per-core scale); these are synthetic-emulator
  observations, not a mid-range-device budget.
- The emulator battery remained at 100%/5,000 mV/25 C and thermal status stayed **0**;
  this powered virtual battery is not endurance evidence. Unity `gfxinfo` exposed the
  ViewRoot and buffer summary but no usable frame histogram. The live-match checkpoint
  screenshots are `...-host/match.png` (SHA-256
  `22413DF9E3E5D6BA765E77D91B462ED3A8790EA0D4A7A96F4297467490353967`) and
  `...-host/match-end.png` (SHA-256
  `174FCDEE2BABAB93A1D4E5561744BB1A45737121E9BE8A83977F210AC3F8B4C5`); the latter is
  a live-match checkpoint, not a terminal results screen.

The same AVD was also tried with `-gpu swiftshader_indirect`. That diagnostic reached the
match but rendered severe red/black geometry corruption and logged repeated
`Universal Render Pipeline/Lit` GLSL link failures because the SwiftShader profile exceeded
`GL_MAX_VERTEX_UNIFORM_VECTORS (256)`. The raw evidence is retained at
`Builds/Local/Device/Performance/20260830-16k-5d136fb-route/` (`after15.png` SHA-256
`D0F773030BDD2BA2BC78BBC8CE3D0A65143650464359FF44221ECBB1BA81C481`, logcat SHA-256
`2D176EF6AA53318B38BF6BD61C21045E31E6F3CAB293556FA769B4EA18A3B3F4`). It is classified as
a superseded renderer-profile limitation; no source-wide material rewrite was made because
the host-GPU run renders normally.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Genuine 16 KB runtime page-size smoke | **Passed for host-GPU Android 16 AVD profile** | `getconf PAGESIZE=16384`, exact APK install, normal menu/live-match launch, 90-second capture and clean app-scoped logcat |
| Universal/physical 16 KB compatibility | **Open** | Repeat on an ARM64 physical 16 KB device and other supported GPU profiles; Lava is 4 KB |
| Normalized FPS, frame-time, GC, GPU and render-thread budget | **Open** | No usable Unity frame histogram; emulator CPU/battery are not product-tier evidence |
| SwiftShader AVD profile | **Superseded diagnostic / known limitation** | URP/Lit uniform-limit corruption retained for follow-up if SwiftShader is a required target |
| Final authored/accessibility/fun/performance approval | **Owner review required** | Runtime smoke does not replace human review |

### P48 - Exact-candidate 180-second Lava focused performance capture - 2026-08-30

The exact `5d136fbb6be6a5554931f6ab859be8b9a8a995a2` APK was relaunched on the approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). The provided
`Tools/Validation/capture_android_performance.ps1` ran for **180 seconds**, sampling every
5 seconds (**36 samples**) while movement, attack, ability and gadget input were exercised
after the menu -> Solo Raja -> fighter route. The run manifest is
`Builds/Local/Device/Performance/20260830-lava-5d136fb-perf2/manifest.json`, SHA-256
`7728C80ADFEA814D1D9E63D3344C527825CFCF413236AB89131C62C46C2D459D`.

- Warm-up-excluded samples 11-36 measured **261,702-273,769 KB PSS** (average 270,386 KB),
  **384,112-396,696 KB RSS** (average 393,151 KB), **75,792-81,936 KB graphics PSS**
  (average 79,314 KB), and current-process `top` CPU **87.5-118.0%** (average 108.0%;
  Android's 100%-per-core scale).
- Battery remained at **76%** (4,128 -> 4,129 mV; USB-powered), battery temperature
  30.0-31.0 C, thermal status **0**, and the logcat scan found no configured fatal marker.
  Logcat SHA-256 is `3F3825762955C1DD4500D11C97E397C32FF6171B831AF567C8FE4F831F91C8FB`.
- A 30-second Perfetto trace is retained at
  `Builds/Local/Device/Performance/20260830-lava-5d136fb-perf2/battleraja-perf2.pftrace`
  (33,418,400 bytes; SHA-256
  `46FF1407EC657F800AEE7B5498A19620A7DEEC38305B75C4D2D7966B9A5680AE`). No host
  trace-processor was available, so no frame-timeline claim is made. Simpleperf was
  attempted and refused because the temporary candidate is not debuggable/profileable.
- Unity `gfxinfo` still supplies no usable frame histogram. Lava reports 4 KB pages; this
  is not physical 16 KB runtime evidence.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| 180-second exact-candidate warm-up/stability capture | **Passed as raw diagnostic evidence** | 36-sample manifest and per-sample meminfo/top/thermal/battery/activity files |
| Normalized FPS, frame-time, GC, GPU and render-thread budget | **Open** | Unity SurfaceView has no usable gfxinfo histogram; parse a future Perfetto trace or use a supported profiler |
| Battery endurance and sustained thermal acceptance | **Open** | Run unplugged, longer repeated-match capture with owner acceptance criteria |
| Genuine 16 KB runtime | **Open** | Lava reports 4 KB pages; use a genuine 16 KB runtime |
| Final authored/accessibility/fun/performance approval | **Owner review required** | Raw telemetry does not replace human review |

### P47 - Exact 5d136fb terminal-outcome presentation candidate and Lava route - 2026-08-30

The exact runtime/art source for this checkpoint is commit
`5d136fbb6be6a5554931f6ab859be8b9a8a995a2` (`art: wire terminal outcome presentation cues`).
The saved presentation layer now includes gold Victory and red Defeat VFX prefabs, and
`OfflineMatchController` drives the render-only terminal state from authoritative results.
`FighterPresentation` keeps Victory/Defeat persistent after results publication without
marking a winner eliminated. No damage, cooldown, movement, gadget, zone, timing, authority,
networking or reward rule changed.

#### Machine evidence

- Repository validation: **0 errors / 0 warnings** from `Tools/Validation/validate.ps1`.
- EditMode: **141/141 passed**, XML `Builds/Local/TestResults/editmode-outcome-vfx-final.xml`
  (SHA-256 `E51A3F7384F3144B7AE114AD4351A7BD9FC9DE9994825013DD37592924C5E581`).
- PlayMode: **88/88 passed**, XML `Builds/Local/TestResults/playmode-5d136fb-final.xml`
  (SHA-256 `5A3442B484486C3770626D97BBC7A3207C2388AFBC8E6CD856699588E914208A`). This includes
  the terminal outcome persistence regression for Victory and Defeat VFX.
- APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, **40,673,654 bytes**,
  SHA-256 `31D982D7334B08D0DE759CE755784547CFCF843D9CFCFB1DB0E041E7EEE2DF2D`.
- AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, **36,498,821 bytes**,
  SHA-256 `D49E01B63C5106B68133040F03B2D7F11831DEA80E388D32296FCC6B705C20CA`.
- Build log hashes: APK `CB8CABFA532C429D614526AE8C61B6FCBC5F1692002BFF89F925290192E354A1`;
  AAB `147D2FA6F596FB509683572BF65D8EBC29DCF278F4BE439500B89F3C2A66F223`.
- Release checker `Builds/Local/Logs/release-checker-5d136fb.log` is **0 errors / 0 warnings**
  (SHA-256 `5D704A38FEC64F154382E19593AB0A82F9DB3557DAF2519D8D3852955118409D`). It reports
  package `com.example.battleraja.m11`, version `1.0.0`/code `100`, min/target API `28/36`,
  VIBRATE plus Unity's dynamic receiver permission only, no network permission, seven ARM64
  native libraries, static 16 KB ELF alignment, and icon/feature dimensions `512x512` and
  `1024x500`. The signer is temporary/debug only.
- Bundletool 1.18.3 generated
  `Builds/Local/V1GameplayTruth/Android/battleraja-v1-5d136fb.apks` (**36,626,665 bytes**,
  SHA-256 `0D1961247BDACCD88343A36966201F09EB62388FEAFEB648A546E0F3A1691941`) and the
  approved-Lava universal extraction (**36,626,350 bytes**, SHA-256
  `EB14C2826287A8083B3A2D9D610256B269D367A482F7F12DA8BBDF17DD03B24F`). Direct and extracted
  `zipalign -c -P 16 -v 4` checks passed and temporary `apksigner` verification passed.
  The evidence log is `Builds/Local/Logs/bundletool-5d136fb.log` (SHA-256
  `2703521F09ACB310D802292F1FA0EF7F23DCF00BC49AE2FBC0C21E008E7BFF30`).

#### Approved Lava evidence

The exact APK installed successfully only on approved Lava `ST5GDW23LB004392`
(`LAVA LXX508`, Android 14/API 34, reported 4,096-byte pages). The complete route manifest is
`Builds/Local/Device/Performance/20260830-lava-5d136fb-outcome/manifest.json` (7,337 bytes;
SHA-256 `0BB2714E7940954C0EEE6E164012DE3AE2D63996973328617A20056A036EA915`); it records
artifact, route, telemetry and screenshot hashes. Real touch reached menu, Solo Raja, all
Bijli/Pehel/Maya cards, live opening, attack/ability/gadget action feedback, Aandhi
pressure/closing/final-circle states, player defeat and spectating, results with placement #6,
rematch, settings toggles, background/resume, and tutorial `8/8 COMPLETE` via the in-app SKIP
control. This is route evidence, not owner approval of touch comfort, accessibility, final art,
victory feel or cultural presentation.

Selected unmodified PNG copies for owner store review are tracked under `Docs/Store/V1/` with
the `5d136fb` suffix; their paths and hashes are listed in `Docs/Store/BattleRaja_V1_STORE_ASSETS.md`.

The bounded raw snapshot records **273,885 KB total PSS**, **385,796 KB total RSS** and
**80,020 KB graphics PSS** with thermal status **0**; the final full-route logcat
`logcat-5d136fb-final.txt` is 1,200,228 bytes (SHA-256
`42C665A195A53A2E7BCD3BB250A68E5E7D94A9D9E5554807F890FF04597114DE`) and contains no configured
app fatal/ANR/SIGSEGV marker. `gfxinfo` exposes only Unity's view hierarchy, so no normalized
FPS, frame-time, GC, battery, sustained thermal or performance-budget pass is claimed. The
Lava phone is a 4 KB runtime device; static 16 KB alignment is not genuine 16 KB runtime proof.

| Gate | Current classification | Evidence / remaining action |
| --- | --- | --- |
| Outcome Victory/Defeat presentation wiring and saved VFX assets | **Passed (machine-verified baseline)** | Six-cue fighter prefabs, saved `VictoryVfx.prefab`/`DefeatVfx.prefab`, controller state persistence test in PlayMode |
| Exact source, tests, package manifest, offline permissions and static alignment | **Passed locally** | Source/build/test hashes above; release checker and bundletool log |
| Approved-device install, full route, tutorial completion and bounded crash-marker smoke | **Passed locally / observed** | Lava manifest and exact screenshot set; comfort/accessibility and repeated-route review remain |
| Genuine 16 KB runtime | **Open for the P47 snapshot; superseded by P49 profile smoke** | Lava reports 4,096-byte pages; P49 adds host-GPU AVD `PAGESIZE=16384` evidence, while physical/other-profile coverage remains open |
| Sustained performance, thermal, battery, GC/frame-time and repeated rematches | **Open** | Raw bounded telemetry only; no normalized budget approval |
| Final authored art/audio, cultural, fun and accessibility approval | **Owner review required** | Generated presentation baseline remains a candidate |
| Final package identity, release signing, privacy/Data Safety, IARC/content rating and Play Console | **Owner-controlled** | Drafts/checklists are prepared; no upload or public deployment performed |

The candidate remains a **prototype / Android offline release candidate in progress**, not
Play-ready. The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.
