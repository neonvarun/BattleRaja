# Project Status

## Active milestone

**Milestone 11 — Closed-Test Release Candidate Preparation (external gates open)**

## Current state

- **Latest offline Android continuation (2026-09-02; evidence captured 2026-09-01):**
  The working tree now includes the bounded-lag squad blackboard, deterministic support/
  escort handoffs, same-tick command-phase freeze, unique team placements after respawn,
  and fair production pacing (`1.0x` bot damage, shared `15x` cadence). Static validation
  is **0/0**, EditMode **159/159**, PlayMode **94/94**, and the strict production-bot run
  is **94/94** with 100/100 terminal matches. The 100-match report records **93/100** in
  the 240–360 s target window, **93/100** combat-positive, **61/100** with at least three
  combat eliminations, **3** Aandhi-only resolutions, **100/100** bot-to-bot damaging pairs,
  and zero protected-warmup or invalid-position samples. The exact APK is **41,514,464
  bytes**, SHA-256 `7243A7A324E43FC2C2A274DDF1B27C89166E5E9CF5F39C981D650355F696E9B6`; the
  AAB is **37,339,995 bytes**, SHA-256
  `ABC16E0F7B499690BA41ECC9CBAB5D243C35E85783B6051E5E1982B51ACE8D48`. The release
  checker passes temporary package `com.example.battleraja.m11`, target SDK 36, no network
  permissions, ARM64-only libraries and static 16 KB alignment. Fresh exact-final approved-
  Lava evidence is under `Builds/Local/V1GameplayTruth/Final/lava-20260901-balanced/` and
  reaches menu → Bastion briefing → fighter choice → live arena → actions/settings →
  results → rematch → airplane-mode toggle. A six-sample/30-second raw capture reports
  PSS **60,858–252,074 KB**, RSS **176,511–390,924 KB**, graphics PSS **10,455–77,512 KB**,
  raw top CPU **35.7–57.1%**, thermal **0** and no configured fatal markers. Lava reports
  4 KB pages and Unity `gfxinfo` has no usable frame histogram. The full tutorial action
  route, final commissioned art, normalized endurance, physical 16 KB runtime, permanent
  identity/signing, privacy/Data safety/IARC and Play owner gates remain open. The exact
  evidence index is `Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-02.md`.

- **Previous offline Android continuation (2026-09-01):** Bastion Crown authority/replay
  hardening and squad-planner coverage are green: static validation **0/0**, EditMode
  **155/155**, PlayMode **94/94**, and a two-seed/8,400-tick v2 replay soak has zero
  combined-hash divergence. The deterministic planner emitted contest **64**, escort **64**,
  defend **96**, collapse **64** and Aandhi-retreat **32** intents across 32 seeds. The menu
  now references the original `Assets/BattleRaja/Art/V1/BattleRaja-FeatureArt-OriginalCandidate.png`
  (Bazaar Bastion shrine/fighters; no vehicles or racing motifs); the older feature image is
  retained as non-runtime historical material. The exact APK is **41,510,440 bytes**,
  SHA-256 `5F7438105FE450D6331CFEDEE1FAEEB87FB4F6677EB811A997A02CC8FD7C4AE9`; the AAB is
  **37,335,957 bytes**, SHA-256
  `87C835570B62C4C3A79C156F94CB7E15C6AD31FCB50A0E8ADB0FDE6672DC4858`. The release checker
  passes package `com.example.battleraja.m11`, version 1.0.0/100, target SDK 36, no network
  permissions, ARM64-only libraries and static 16 KB alignment. Fresh approved-Lava
  `ST5GDW23LB004392` evidence reaches menu → Bastion briefing → fighter choice → live arena,
  control taps and settings; a clean six-sample/30-second live capture reports PSS
  287,530–293,678 KB, RSS 426,940–433,088 KB, graphics PSS 87,024–93,180 KB, thermal 0 and
  no configured app crash markers. Lava reports 4 KB pages and Unity `gfxinfo` has no usable
  frame histogram. Full all-fighter/tutorial/gadget/results physical comfort, normalized
  endurance, physical 16 KB runtime, final authored/cultural/accessibility/fun review,
  permanent identity/signing, privacy/Data Safety/IARC and Play publication remain open. The
  exact evidence index is `Docs/QA/V1_OFFLINE_ANDROID_VALIDATION_2026-09-01.md`.

- **Bastion Crown V1 implementation checkpoint (2026-08-31):** The player-facing
  production route is now the original offline Bastion Crown 4v4 loop: actor 1 human
  plus actors 2–4 allied AI versus actors 5–8 rival AI. The pure domain owns canonical
  teams, roles, Crown Spark pickup/drop/rotation/deposit, KOs/assists, shared tickets,
  protected respawn, overtime and draw resolution; the Unity controller is an adapter
  and the existing Solo foundation remains available to deterministic fixtures. The
  production scene is regenerated through `BuildEntrypoints` with canonical actor IDs,
  team-aware bot plans, carrier slowdown, shrine/socket telegraphs, team HUD and
  Bastion result copy. New gates are **148/148 EditMode** and **94/94 PlayMode**;
  static validation remains **0 errors / 0 warnings**. Evidence is indexed in
  `Docs/AI/BASTION_CROWN_PRODUCT_BRIEF_2026-08-31.md` and
  `Builds/Local/V1GameplayTruth/TestResults/`.

- **Fresh Android candidate (2026-08-31):** The non-development ARM64 APK was rebuilt
  from the implementation checkpoint: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`,
  **41,438,372 bytes**, SHA-256
  `6EDC5C0E5D304529A6059A94F00F7AB32AB9C71A4464044D3B0D3ED5D3E2C507`.
  The matching release-shaped AAB was rebuilt from the same source: **37,263,881
  bytes**, SHA-256
  `3D12A358E0F9159A2CA3749A4E53DBB712AF19B0FCCC6E6D80F96DE5944508EE`.
  The local V1 technical gate passed the offline manifest, ARM64-only/static
  16 KB alignment and store-creative dimension checks.
  It remains temporary/debug identity `com.example.battleraja.m11` and is not a
  publishable signed artifact. A local Android 16 emulator (ANGLE renderer) reached
  the menu, Bastion Crown briefing, fighter portraits and live arena; the approved
  Lava serial `ST5GDW23LB004392` was not connected in this run, so physical-device
  and reference-app evidence remain open.

- **Working-tree presentation pass (2026-08-31):** An observation-only review of the
  installed Brawl Stars and Smash Karts entry surfaces on approved Lava informed an
  original BattleRaja polish pass. The menu now uses the owned Bazaar feature art with
  a dominant `PLAY OFFLINE` action; mode and fighter screens retain that context; square
  isolated portraits render distinct Bijli, Pehel and Maya production prefabs; and the
  live arena uses authored near fighter meshes with a closer camera and restrained
  warm/cool lighting. The exact candidate APK is **41,375,996 bytes** with SHA-256
  `3F12B3D36F5B34DE9288F55C838530A41C584D11122308A2420B0A2F0AA3B059`.
  Static validation is **0 errors / 0 warnings**, full EditMode is **141/141** and full
  PlayMode is **92/92**. Approved-Lava evidence is under
  `Builds/Local/Device/audits/20260831-presentation/`; this remains a presentation-only
  offline candidate and not a Play Store submission. Commit `425e30d`, ADR-077 and the 2026-08-31
  reference rerun in `Docs/RESEARCH_LOG.md` record the scope and originality boundary.

- **Previous exact-source lifecycle checkpoint (2026-08-31):** source tip was
  `e603ce7e7f1cb279f5e3e9d606ea5eae89603ecb` (`android: clear input on lifecycle pause`).
  Android pause clears transient player input across the adapter, virtual stick and
  attack/ability/gadget buttons, and the HUD clears the adapter before the lifecycle pause
  boundary; ADR-076 records the presentation/input-only decision. Focused lifecycle PlayMode
  is **1/1**, full EditMode is **141/141**, full PlayMode is **92/92**, and static validation
  is **0 errors / 0 warnings**. The exact rebuilt APK is 40,679,115 bytes (SHA-256
  `349F02C67DE4CC801C5CB81B9CEC375A18D89B136C1A3AD9BB9549E9640A41CB`) and AAB is
  36,504,445 bytes (SHA-256
  `9A5BE261D2504007BCBAF4105568F19437CBA8A4DEFAA3383371DE35386D51E0`). Approved-Lava
  launch → HOME → resume evidence is recorded in P66 of `Docs/V1_RELEASE_PLAN.md`.
  The product remains a prototype: physical 16 KB, full lifecycle/held-input comfort,
  normalized performance, endurance, final authored/accessibility/cultural/fun review,
  signing/identity, privacy/Data Safety and Play Console gates remain open. The two prompt
  files are intentional owner work.

- **Historical runtime/art/audio presentation checkpoint (2026-08-30):** source tip was
  `56df201` (`audio: add final-circle escalation cue`), following runtime/art source
  `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`. It adds the owned generated `ZoneFinalCircle`
  cue and plays it once on authoritative `MatchPhase.FinalCircle`; the final mix/loudness/
  voice review remains open. The exact current APK/AAB, 141/141 EditMode, 89/89 PlayMode,
  static validation and approved-Lava Final Circle/results/lifecycle evidence are indexed in
  `Docs/V1_RELEASE_PLAN.md` P55. No gameplay authority, package-policy or networking rule
  changed.
  The prior P54 runtime/art source remains
  `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`; the earlier test-bearing follow-up was
  `4c4c67cbbc20062e3723cc90ee3bb7c266bbeda4`. The terminal-outcome continuation saves
  Victory/Defeat VFX and routes authoritative placement into persistent render-only fighter
  states; the follow-up adds direct player Umbrella Guard pickup/use/mitigation PlayMode coverage without changing
  runtime behavior. Full EditMode is **141/141**, PlayMode is **89/89**, static validation is **0 errors / 0
  warnings**, and the exact temporary-ID APK/AAB pass offline manifest, ARM64/static 16 KB,
  bundletool, alignment, signature and store-dimension checks. Fresh exact APK evidence on
  approved Lava `ST5GDW23LB004392` reached the full menu → fighter → match → Aandhi →
  spectate → results/rematch route, settings/lifecycle and tutorial `8/8 COMPLETE`; the
  player reached placement #6. Raw telemetry is **273,885 KB PSS / 385,796 KB RSS /
  80,020 KB graphics PSS**, thermal status 0 and no configured fatal/ANR/SIGSEGV markers.
  This is bounded diagnostic evidence only. The product remains a prototype: final authored
  art/audio/cultural review, accessibility/fun review, normalized sustained performance,
  physical 16 KB, final identity/signing, privacy/Data Safety and Play Console actions
  remain open. The two prompt files are intentional owner work.
  The exact APK also passed a genuine 16 KB host-GPU Android 16 AVD smoke; physical ARM64
  coverage, other GPU profiles and normalized performance remain open (P49).
  A fresh current-source fixed-tick production-bot release-gate rerun at 50x completed
  **100/100** seeded matches in the 240-360 second window, with combat-positive and
  bot-to-bot damage in all 100 matches, zero Aandhi-only resolutions, zero protected or
  invalid samples, and all three gadgets used in every match; the 89/89 PlayMode report,
  batch report and hashes are indexed in P53. This closes the automated pacing/safety
  gate for the current harness, while the accelerated path remains separate from the
  real-time same-seed determinism evidence in P10.

- **Latest exact-source release-gate checkpoint (2026-08-27):** commit
  `2a113e0c4798e8e51a43379a0fa0facd7e8f0fe1` persists ordered production replay captures
  in a checksummed Unity-independent `.brr` format. Full EditMode is **141/141** and
  PlayMode is **86/86**; static validation and the clean release checker are **0 errors /
  0 warnings**. The rebuilt APK/AAB pass offline manifest, ARM64/static 16 KB and
  store-dimension checks. P42 records the 9,180-frame production capture and exact-file
  re-execution, plus the current APK/AAB/checker hashes. The APK remains Android
  Debug-signed with temporary identity and the product remains a prototype: full
  touch-route, accessibility, sustained performance approval, final physical 16 KB
  coverage, final authored identity/cultural review, signing, privacy/Data Safety and Play
  Console actions remain open. Earlier 100-match bot, Lava route and deep-soak evidence
  remain indexed in P38-P41.

- **Working-tree gameplay truth update (2026-08-25):** unified action eligibility
  is now owned by `OfflineMatchAuthority`. Movement, ability displacement, attack,
  fighter ability, decoy, direct/projectile damage, healing, gadget use, station
  effects/healing and outside-zone damage all reject outside Opening-Final Circle
  before mutating canonical state. Bots now ignore same-faction actors in Solo
  free-for-all, respect their weapon's true maximum range and use gadgets only against
  visible hostiles. Static validation is **0 errors / 0 warnings**, focused authority
  validation is **25/25**, full EditMode is **133/133**, and full PlayMode is **75/75**.
  The deep recorded-replay soak passes **1,000 seeds x 2 executions = 2,000 matches**
  with zero divergence in **465.3278045 seconds**; see
  `Builds/Local/V1GameplayTruth/TestResults/botfair-deep-soak.xml`.
  Compiler scans of the final test logs found **0** C# errors and warnings. This is
  local test evidence plus an exact-commit development-shaped Android launch smoke.
  Commit `e65c0ea` produced APK **92,852,632** bytes (SHA-256
  `11A4DF4623980F53B4F34FCEB48B09858DD32A459082B7B00772F9543305D9FC`). It installed on
  Lava `ST5GDW23LB004392`, cold-launched in **450 ms**, and hot-resumed in **51 ms**;
  crash-pattern scans found **0** matches. This is launch/lifecycle evidence only: it
  does not claim interactive match QA, sustained performance or release readiness.
- Exact runtime HEAD `3b09775` (`authority: own bijli dash replay state`) adds
  authority-owned Bijli dash lifecycle, canonical collision advancement, replay
  hashing, production view mirroring and action-phase gating for Bijli/Pehel starts.
  Static validation is **0 errors / 0 warnings**, EditMode is **131/131**, PlayMode
  is **75/75**, and the `BATTLERAJA_SOAK_MATCHES=1000` recorded-replay soak passes
  **1,000 seeds x 2 executions = 2,000 matches** with zero divergence in
  **468.619297 seconds**; see
  `Builds/Local/V1GameplayTruth/TestResults/bijli-deep-soak.xml`.

  The same runtime source also has a fresh development-shaped Lava smoke: APK
  **92,855,860** bytes, SHA-256
  `115C428A69A6E27B7D0BE7A9A0B5C433CAE7CA165C0FCA8251DA34122E70CBC0`; cold launch
  **384 ms** and hot resume **82 ms**. The route reached Bijli selection, live Solo
  Raja, ability input, player-defeat spectator mode and the final results/rematch
  UI. Crash-pattern scans across launch, background, resume, match, ability and
  final-state logs found **0** matches. Peak recorded PSS during the route was
  **421,362 KB**. Development-player socket warnings appeared in the Development
  Console but were non-fatal. This is interaction/lifecycle evidence only: it does
  not claim release signing, sustained performance, accessibility or store
  readiness. See the exact evidence section in `Docs/V1_RELEASE_PLAN.md`.

- The current branch tip contains documentation follow-ups after the exact artifact source `35de9f3`; the latest runtime-bearing source is `73f20a2` (`accessibility: propagate reduced flash mode`). Repository validation is **0/0**, fresh EditMode is **125/125**, and fresh PlayMode is **73/73**. Fresh release-shaped packages archived inside `Builds/Local/V1Evidence/35de9f3/Android` are APK **39,487,017** bytes (SHA-256 `41432964C104C7EA58A1DECC3423F611515D909E7EFA61F85E6AC46D7BFBE389`) and AAB **35,313,317** bytes (SHA-256 `34A0BAEFBF68A6A7679244EADB342556B5F3EBA87D3AB61CACDA48CAE04BE785`). The
  AAB is ARM64-only and passed static 16 KB alignment. Lava install and launch
  succeeded; the portrait menu is captured in
  `Builds/Local/V1Evidence/edbf4d0/Android/lava-launch.png`. This is menu-only
  evidence; no tutorial, full-match, accessibility or performance pass is
  claimed. The artifact pair is exact for current HEAD `35de9f3`.

- Documentation evidence is anchored by the exact-source sections in
  `Docs/QA/CURRENT_STATE.md` and `Docs/QA/LATEST_HEAD_BASELINE.md`, updated with
  the release-flags candidate;
  the measured runtime/validation source for the current Android package is `1d743b0`
  (`tools: add repeatable Lava performance capture`); the preceding lifecycle
  change is `d96d3f2` (`android: pause offline match on app lifecycle loss`) and
  the latest runtime-bearing change is `e808830` (Pehel cached actor views). Full
  EditMode is
  **125/125** and PlayMode is **71/71**, including the new background/resume
  pause guard. Fresh release-shaped packages were built from exact source
  `1d743b0` in disposable worktree `C:\BRLifecycle`: APK `39,486,559` bytes,
  SHA-256
  `89156306717C5EB27EE193AD1D46809DFE19159112ADC3C77008D4C6A3C89DE0`; AAB
  `35,313,996` bytes, SHA-256
  `F5776F6AF19EE1C0A803D76050A80E62E883710E00296EF9603CED279D2227C1`.
  The AAB contains seven ARM64 native libraries, no other ABIs, and passed the
  static 16 KB ELF alignment check. The APK installed only on Lava
  `ST5GDW23LB004392`; launch reached the Unity activity, but the owner's lock
  screen remained active, so no interactive route or tutorial completion claim
  is made. Classification remains **prototype** and the packages remain
  debug-signed with temporary package ID `com.example.battleraja.m11`.

- Latest runtime-bearing source is now `65e6001` (`tutorial: gate progression on
  real offline actions`). The tutorial overlay now requires the matching arena
  action for each step, requires both pickup and use for the gadget step, and
  reloads a fresh tutorial arena for replay. EditMode is **125/125** and
  PlayMode is **70/70** after the change. A fresh release-shaped APK
  (`39,484,371` bytes, SHA-256
  `0F9AD792D2479ADC1F57BCCACF28921DB327A6D926990560762A7FC977DD7DB8`) and
  AAB (`35,311,798` bytes, SHA-256
  `F0049D127717FE7EB1BF8FB6F13E7699133F76FBA135EB18960AD48AE01EEAE7`) were
  built in disposable worktree `C:\BRTutorial`. The AAB is ARM64-only with
  seven native libraries and passed the static 16 KB ELF alignment check. The
  APK installed successfully on Lava `ST5GDW23LB004392`, but interactive
  tutorial QA is blocked by the owner's active device lock screen; no physical
  action-by-action completion claim is made. The packages remain debug-signed
  with temporary package ID `com.example.battleraja.m11` and are not
  publishable. Classification remains **prototype**.

- Latest runtime-bearing source is `062066b` (`docs: align exact head after
  visual cleanup`). A fresh release-shaped APK (`39,482,035` bytes, SHA-256
  `C7B16D01DEA3ED3ADA1B5E5AA421B82ADBA46F5E1A0A2B0283F409BC59F3E245`) and
  AAB (`35,309,464` bytes, SHA-256
  `4D3948F876580AC45A0655593DAA6FE4AF70BC9BACF78840F33EC63E8775E858`) were
  built in disposable worktree `C:\BRS`. The APK was installed only on Lava
  `ST5GDW23LB004392` and completed the real offline menu-to-results route. The
  AAB is ARM64-only and passed static 16 KB alignment. These artifacts are
  debug-signed with temporary package ID `com.example.battleraja.m11`; they
  are not publishable. Exact Web build/browser evidence remains blocked by
  Unity Bee/Burst. Later commits on this branch are documentation/store-evidence
  follow-ups and do not change runtime code. Classification remains **prototype**.

- Exact checked-out HEAD is now `53da0f3` (`visuals: release generated ground
  mesh on teardown`). This cleanup-only follow-up releases the runtime-created
  Bazaar mesh on scene teardown; it preserves the tested visual slice from
  `a5597f5`. Validation is **0/0**, EditMode **125/125**, and PlayMode **70/70**.
  The fresh APK/AAB and Lava captures documented below are from the runtime-
  equivalent parent because this lifecycle change does not alter the rendered
  result. Classification remains **prototype**.

- Exact checked-out HEAD is `a5597f5` (`visuals: add original ground mosaic and
  UI frame treatment`). Validation is **0/0**, EditMode is **125/125**, and
  PlayMode is **70/70**. The render-only visual slice adds a three-material
  Bazaar ground mosaic without colliders and adds non-interactive UI frame
  accents. A fresh development APK (`92,734,792` bytes,
  SHA-256 `05CBD516DD429EAE8AF1882E719E2FAA95E63949C90261208215B02AF56B9A18`)
  and release-shaped AAB (`35,310,447` bytes,
  SHA-256 `0B32C23A9F4E656790B9470AEE80D6375FF6E264A7551956A201129D3CDA7729`)
  were built in disposable worktree `C:\BRv1vis`; the APK was installed only
  on Lava and reached the live offline match with no sampled fatal/ANR/SIGSEGV/
  NullReferenceException/UnityException markers. The exact Web build remains
  blocked and was not rerun for this Android-only visual slice. Classification
  remains **prototype**.

- Exact checked-out HEAD is `3bbe7d1` (`chore: track Unity metadata for
  presentation assets`). A fresh debug-signed V1 AAB is 35,301,185 bytes
  (`20709BDDC90F418EFFED493E209A1CA943F5F1B119017AE415672491B9FC9EFF`) and the
  matching APK is 39,473,743 bytes
  (`796675A71F6127AAB95B4B6C2CEB727888C77904937CAF23B1D53E3A92DFC771`). The
  bundle is ARM64-only and passes 16 KB alignment. The APK was installed only
  on Lava and cold-launched successfully; the exact Web build remains blocked.

- Documentation HEAD is `3600d8b` (`docs: record current V1 bundle evidence`);
  the latest runtime-bearing source remains `f3dea5d` because the intervening
  commits only added tests/documentation. The exact-source Web build is still
  blocked by the Unity Bee/Burst player stage, so no current Web artifact or
  browser pass is claimed. Local exact-HEAD validation is **0/0**, EditMode is
  **125/125**, and PlayMode is **70/70**. Classification remains **prototype**.

- Exact current Android route evidence was refreshed on 2026-08-24 from the
  runtime `f3dea5d` APK (the documentation HEAD is `3600d8b`) on Lava
  `ST5GDW23LB004392`: menu → offline mode → fighter selection → live eight-actor
  match → Aandhi closing → results → menu all completed with correctly derived
  portrait coordinates. No fatal/ANR/SIGSEGV/UnityException markers were found.
  A correctly derived compact-layout tap also changed the gadget HUD from held
  `tiffin_station` to empty with `Tiffin Station deployed` feedback. This confirms
  technical route/gadget reachability; pickup accessibility, touch ergonomics,
  sustained performance, human visual review, signing and store gates remain open.

- The exact current APK also opened `TUTORIAL REPLAY`, advanced through all eight
  tutorial cards to the visible `TUTORIAL COMPLETE` state, and returned to `1 / 8`
  after `REPLAY TUTORIAL` on Lava. This confirms the tutorial state loop; it does
  not substitute for human verification that each instruction is performed.

- Exact current source is `f463b1b` (`vfx: add render-only Aandhi boundary cue`)
  on `codex/v1-playstore-release`. Repository validation is **0/0** and a fresh
  Android release-candidate APK was built locally (39,473,715 bytes; SHA-256
  `ADFE38B3C11DE2119D7180967C48165682095C4818F9FD0140FB694F1198A666`). The
  matching AAB is 35,301,175 bytes with SHA-256
  `C19A238FB31530EAC2AA920ED7B760F76C91D2590B6C27506887FED8170766B8`; bundle
  checks found arm64-only native libraries and passed 16 KB alignment. The
  Aandhi cue is render-only and consumes the match snapshot. The exact Web build
  is blocked by Unity Web Bee/Burst exit code 4 with no completed artifact; no
  browser smoke pass or Lava install is claimed. Classification remains
  **prototype**.

- Exact current source is `ecdb25b` (`ui: hide bot diagnostics in release builds`)
  on `codex/v1-playstore-release`. Repository validation is **0/0** and a fresh
  Android release-candidate APK was built locally from this source (39,465,083
  bytes; SHA-256
  `7BDEA277C28CED29367CD3A76A73DCC7DFF45EDBBCAD1F9F9415C64FA7B57AD4`). The
  focused build was not installed on Lava; no current AAB/Web/performance pass is
  claimed. Classification remains **prototype**.

- Exact current V1 source is `fe80582efc35368a03afea314d53e071ac1872bf` on
  `codex/v1-playstore-release`. Validation is **0/0** and PlayMode is **70/70**;
  runtime UI now binds touchscreen position/press actions explicitly. The exact
  APK is **39,465,411 bytes**, SHA-256
  `BC74E8C09C853AB8EBE089B0B6F5C063D47A9963F2F940EF48D657FFADAB3E23`, built in
  `C:\Projects\BattleRaja-validate-fe80582` and installed only on Lava
  `ST5GDW23LB004392`. The portrait menu launch is verified; no current-source
  AAB exists, and physical touch/full-route, performance, signing, store/legal
  and human review gates remain open. Classification stays **prototype**.

- Latest V1 Android source is `dff3a89` (`docs: record branded Android splash
  evidence`) on `codex/v1-playstore-release`. The exact release-shaped APK is
  **39,466,543 bytes** (SHA-256
  `A6760651223052BEFB426DA08F5434ED71922A3FF9309336C1827945474F4A91`) and the
  matching AAB is **35,293,988 bytes** (SHA-256
  `567EF167654BC53A1836035297385278E2673411C7BD06A6257E550737E3CBF4`), both built
  in a disposable worktree. The APK was installed only on Lava
  `ST5GDW23LB004392`; cold-launch evidence shows the BattleRaja icon splash and
  offline menu with no fatal/ANR/SIGSEGV marker. Full route QA, sustained
  performance evidence and human visual/touch approval remain open; classification
  stays **prototype**.

- Product vision: drafted
- Autonomous M1–M11 sequential execution: active; milestone gates and external
  service approvals remain explicit.
- Phase 0 exact-current-source rebaseline (2026-08-22): fresh validate/test/build/
  Lava/Chrome/Edge evidence captured for local `main` tip `35d723f` and the
  `phase0/exact-source-rebaseline` branch (`17a8c75` weapon-retune correction,
  `c78433a` scene/artifact chore). The rebaseline diagnosed that the 2026-08-21
  weapon-damage retune existed only in serialized assets while the Core definitions
  still shipped 18/28/12; the documented 12/20/9 targets are now authoritative in
  code (see `Docs/BALANCE_CHANGELOG.md` and `Docs/QA/LATEST_HEAD_BASELINE.md`).
- Latest closure candidate: `5d6eeb8` on local `main`; PlayMode **59/59** passes
  at this tip. Exact-source platform candidates and full test/platform evidence
  are recorded separately by runtime commit below. See
  `Docs/QA/M11_CLOSURE_REPORT_2026-08-23.md`.
- Current exact offline Android V1 checkout: branch `codex/v1-playstore-release` at
  runtime source `b954a72`. Repository validation is **0/0**;
  exact-current EditMode is **125/125** and PlayMode is **69/69**. The fresh
  release-shaped APK is **39,525,752 bytes** (SHA-256
  `3ABCEF91BF14239AD8D6ED5511D7C74D2C0DA3DB3CC35DCE838573AEB39E1630`). The most
  recent AAB remains the prior `6ac5c12` artifact at **35,351,357 bytes** (SHA-256
  `70825F82A4D79E1E036F4DA8A286778244406D51B1D60A568BD066ED1B82DAA8`) and has not
  been rebuilt from `b954a72`. It is ARM64-only with static 16 KB alignment. The focused source change removes internal
  runtime touch-action ambiguity by binding point/click/navigation actions explicitly;
  the latest presentation slices make both the menu and in-match controls reflow
  between wide and portrait orientations and were visually inspected on Lava in wide
  orientation.
- The exact APK was installed only on Lava `ST5GDW23LB004392`; the branded offline menu
  was visible and the post-launch capture contains no fatal/ANR/SIGSEGV marker. The
  Unity surface exposes no actionable Android UI nodes, so physical touch navigation
  remains a human-review gate despite the explicit runtime binding and regression.
  The
  inspected APK has `VIBRATE` and Unity's dynamic-receiver permission only; it has no
  `INTERNET` or `ACCESS_NETWORK_STATE`. Fusion remains preserved for a future approved
  online milestone but is excluded from this Android runtime package. Full details are
  in `Docs/QA/V1_ANDROID_OFFLINE_PACKAGING_2026-08-24.md`.
- The prior `7751f53` checkout produced the latest Web build with a
  **119,799,965-byte** WASM
  (`8CE68A5AA4C741DD27AD66B9BF61FBC0B17DE9F632F2C791181EC99F516DEA12`); local HTTP
  returned 200 and Chrome/Edge headless reached the Unity loader. This is build/loader
  smoke, not full interactive browser approval; Web has not been rebuilt from `b954a72`.
  The project remains **prototype**: temporary
  package identity, debug signing, adaptive-icon warning, sustained performance,
  interactive Web QA, accessibility, human/store/legal review and publication gates remain open.
- Prior checkout continuation `649d0bb` removed the remaining authored Unity 6 lookup
  deprecation overloads from editor/test code. Full EditMode **125/125**, PlayMode **66/66**,
  and a fresh Android build passed with **0 `CS0618` warnings** and **0 C# errors**; runtime
  evidence remains correctly attributed to `c6badbf` because no player runtime code changed.
- Prior full baseline at runtime-bearing source `73237c8`: EditMode **125/125**,
  PlayMode **57/57**, deep soak **2,000 matches / zero divergence**, Lava
  launch/resume, and Chrome+Edge six-route Web smoke all pass. Repository
  validation is **0 errors / 0 warnings**. See `Docs/QA/LATEST_HEAD_BASELINE.md`.
- Phase 1 authority audit (2026-08-22): complete at `ee573ad` with EditMode **115/115**
  and PlayMode **57/57**. One real gap found and fixed — stale attack ticks could bypass
  weapon cooldown; commands are now rejected as `StaleTick` beyond a 2-tick window and
  cooldowns anchor to the canonical clock (ADR-053). Gadget/ability cooldowns verified
  authority-time-based (no bypass path); transport-level duplicate-event dedup remains
  Phase 8 scope.
- Phase 2 collision/ability/movement verification (2026-08-22): complete at `669c4a9`
  with EditMode **118/118** and PlayMode **57/57**. Solver, authored Bazaar geometry,
  spawn separation and all ability displacements verified from source; one query-API
  defect fixed (`IsPointBlocked` float-fragile boundary comparisons) and three new
  deterministic fixtures pin corner clamping, thin-wall no-tunneling and a seeded-walk
  footprint invariant.
- Phase 3 authoritative-projectile/event-identity audit (2026-08-23): complete at
  `5fa12e3`. Aandhi damage, healing, pickups, gadget collection/use, ability starts,
  projectiles and related identity streams are resolved or emitted through canonical
  authority ticks and covered by deterministic regressions. EditMode **123/123** passed.
- Phase 4 replay/determinism/soak completion (2026-08-23): implemented through
  `1412802`. Canonical authority hashes cover cooldowns, inventories, pickups, stations,
  decoys, ability runtimes, identity counters, projectiles and terminal match state.
  The executable replay runner reconstructs complete setup and input streams. The deep
  soak executed **1,000 seeds x 2 = 2,000 matches** with **zero divergence** in
  **416.1411007 s**; EditMode **125/125** also passed at the same source. Production
  presentation capture wiring remains open.
- Phase 5 exact-source regression (2026-08-23): passed at runtime-bearing source
  `73237c8`. Full EditMode **125/125**, PlayMode **57/57**, validation **0/0**, the
  recorded-replay deep soak above, exact APK install/launch/home/resume on Lava, and
  Chrome/Edge desktop/tablet/portrait Web smoke are complete. Evidence is recorded in
  `Docs/QA/LATEST_HEAD_BASELINE.md`. Human visual/performance approval and publication
  gates remain open.
- Root agent rules: active
- Milestone 0: complete and committed locally
- Unity project: verified at the repository root with URP and MovementLab scenes
- Unity version: `6000.5.6f1`
- Android toolchain: Unity-managed SDK/NDK/OpenJDK modules installed and verified; SDK/Build Tools 36.0.0, NDK r27c `27.2.12479018`, OpenJDK 17.0.18, embedded ADB 36.0.0
- Unity Web build support: present and used successfully
- Browser test environment: Chrome 150, Edge 150 and Playwright CLI available; Firefox unavailable
- Packages: Input System `1.20.0`, uGUI `2.5.0`, URP `17.5.0`, Test Framework `1.7.0`; lockfile is authoritative
- Movement laboratory: implemented with grey-box arena, placeholder player, independent movement/aim, orthographic camera, aim indicator, desktop bindings and safe-area touch sticks
- M1 report: `Docs/MILESTONE_REPORTS/M1.md`; technical gate passed provisionally with
  subjective human-review debt recorded.
- M2 report: `Docs/MILESTONE_REPORTS/M2.md`; technical gate passed provisionally with
  combat/touch/balance review debt recorded.
- Combat/gameplay progression: M3 Bijli, M4 bot, M5 offline match, M6 gadget and M7
  three-fighter roster gates passed provisionally. Match pacing, Aandhi, pickup,
  spectator/results, gadget counterplay, fighter feel, alpha UI/accessibility and device
  review remain open. Networking, backend and progression remain unimplemented.
- Multiplayer: M8 adapter/mock proof implemented; Photon Fusion 2.1.1 package and non-secret
  App ID are configured locally; the real two-client gate remains blocked pending runtime
  integration, terms approval and transport evidence
- M9: transport-independent eight-slot authoritative server/match preparation is present;
  online-alpha completion is blocked by the M8 real-session precondition
- M10: backend-neutral identity/progression interfaces and deterministic fake are present;
  real PlayFab integration is blocked pending title/account/secret approval
- M11: release-candidate preparation, analytics/crash/release seams, explicit Bazaar scene
  boundary and truthful draft docs
  are present; publication, signing, online services and human/legal gates remain open
- Backend/economy: deliberately deferred
- Final art/audio/animation: not started
- Git/LFS: local repository initialized and LFS configured; `origin` points to `https://github.com/neonvarun/BattleRaja.git`

## Exact Goal B deterministic collision/placement evidence — 2026-08-04

- Runtime source commit: `a5fdde8` (`authority: canonicalize collision and ability placement`); exact platform candidate: `7ad7e42` (`docs: record Goal B collision evidence`).
- Repository validation: **0 errors, 0 warnings**. Full EditMode **109/109** and
  full PlayMode **55/55** passed. The new coverage exercises immutable arena bounds,
  deterministic ordered obstacle sliding, canonical Maya placement and canonical
  Tiffin placement. The production Pehel regression now targets an enabled bot and
  waits for the authority-owned throw resolution rather than assuming damage at the
  capture frame.
- Authority scope: production movement, Bijli/Pehel displacement, Dhol displacement
  and Tiffin placement use the Core collision solver; Maya ignores caller-supplied
  remote placement. Bazaar's default collision definition currently contains bounds
  and no authored obstacles, so full arena geometry and all ability edge cases remain
  open.
- Android: the exact `7ad7e42` candidate produced a **94,028,145-byte** APK
  (SHA-256 `FA0CB54C04DC9309D8B21DAE02CE1D3D8A9961DA1C77F5ADE47F0B6AD280053A`).
  It was installed/launched only on Lava `ST5GDW23LB004392`; the activity was
  top-resumed and the sample reported **402,013 KB PSS / 537,532 KB RSS /
  82,088 KB Graphics / 40 KB swap**. This is development smoke evidence, not a
  performance or size pass.
- Web: the exact candidate produced **19 files / 133,747,764 bytes** including the
  development debug-information text. `Web-BazaarBastion.wasm` is **121,033,616
  bytes** (SHA-256
  `D84155637B493182BF380FF91A9ED0D49ECE8F684FAE08E1FA85F0A68F318708`). Local HTTP
  returned 200 for the page, data and WASM. Chrome and Edge smoke reached the menu,
  mode, fighter-selection and active-match routes at desktop and portrait probes;
  browser consoles had 0 errors and one known Unity persistent-data-path
  deprecation warning after match load.
- User-owned/generated scene YAML, Burst output, Resources, Playwright files and
  screenshots remain untouched and unstaged.

## Exact Goal A canonical adapter rebaseline evidence — 2026-08-04

- Source commit: `0e531bb` (`authority: route fighter and gadget ticks through match clock`).
- Repository validation: 0 errors, 0 warnings. Full EditMode **104/104** and full
  PlayMode **55/55** passed after routing authority-driven Bijli, Pehel, Maya and
  gadget adapter steps through the match controller's canonical tick event.
- Android: clean detached-worktree build on Unity `6000.5.6f1` produced a
  **93,986,577-byte** APK (SHA-256
  `0D6F54E5083886E5543C261DEB918708009A12479786293968827BB7D7178AF3`). It was
  installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`); the Unity
  activity was top-resumed. The sampled process reported **350,551 KB PSS /
  487,160 KB RSS / 69,556 KB Graphics / 3 KB swap**.
- Web: the exact source built successfully in the same clean detached worktree;
  output contains **19 files / 133,693,325 bytes**, with
  `Web-BazaarBastion.wasm` **120,983,326 bytes** (SHA-256
  `9BC3A5451695EE90DD53C5EB0F1BECB1E7065E8DAAD5C5B895314ACC49CC47FD`). Local
  HTTP returned 200, and Chrome plus Edge Playwright canvas smoke tests passed
  without captured page/console errors at desktop and portrait probes.
- Build-generated scene rewrites remain isolated to the detached worktree; the
  main worktree's pre-existing scene YAML, Burst, Resources, Playwright and
  screenshot changes remain untouched and unstaged. Projectile/collision authority,
  replay, soak/performance measurements and human review remain open.

## Exact Goal A source rebaseline evidence — 2026-08-04

- Source commit: `7889672` (`authority: harden attack commands and canonical match ticks`).
- Repository validation: 0 errors, 0 warnings. Full EditMode **104/104** and full
  PlayMode **55/55** passed. The new command-authority regressions cover malformed
  direction rejection, authority-owned weapon/faction/tick-rate configuration,
  canonical origin derivation, phase/spawn-protection/future-tick rejection and
  duplicate/out-of-order attack sequences.
- Android: clean detached-worktree build on Unity `6000.5.6f1` produced a
  **93,969,457-byte** APK (SHA-256
  `7558B505785A8E89C847E7039E900B2B2269E42E1BDD4EC2EA72D04609EF9FA9`). It was
  installed/launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`); the Unity
  activity was top-resumed. The sampled process reported **399,752 KB PSS /
  536,648 KB RSS / 81,956 KB Graphics / 368 KB swap**.
- Web: the exact source built successfully in the same clean detached worktree;
  output contains **19 files / 133,684,703 bytes**, with
  `Web-BazaarBastion.wasm` **120,975,513 bytes** (SHA-256
  `B86AF336D02063AE04729C5874D14DCC3B8FC6E15203473A7580AE6284494DDE`). Local
  HTTP returned 200, and Chrome plus Edge Playwright canvas smoke tests passed
  without captured page/console errors at desktop and portrait probes.
- The detached build worktree was used to keep the main worktree's pre-existing
  scene YAML, Burst, Resources, Playwright and screenshot changes untouched. The
  build is technical smoke evidence only: projectile/collision authority, replay,
  soak/performance measurements and human review remain open.

## Current source rebaseline evidence — 2026-08-03

- Repository validation: 0 errors, 0 warnings; full EditMode 101/101 and full PlayMode
  54/54 passed from source commit `6f0fe8b`; authored Unity 6 object-lookup calls are
  absent and the full test logs contain no C# compiler-warning lines; focused Pehel authority and production
  gadget route tests pass 1/1 each in PlayMode, and production bot spawn protection has
  a dedicated passing regression.
- Android: current Bazaar APK `Builds/M11/Android/BattleRaja-BazaarBastion-M11.apk`
  is 151,551,952 bytes (SHA-256
  `11624AFA7A9DB1CDEFC66FCACA5BBEC9CEDBD4C3316AA9AA7BE153BC33141AF4`); exact APK
  launched only on Lava `ST5GDW23LB004392` with top-resumed Unity activity and no strict
  fatal/app-process marker. Memory snapshot: 408,290 KB PSS / 545,224 KB RSS / 82,088 KB Graphics.
- Web: `Builds/M11/Web-BazaarBastion` contains 19 files / 133,581,884 bytes;
  `Web-BazaarBastion.wasm` SHA-256
  `3663611AE374B5B481905341DA451BE335A0A79C704C888D856ACBA8E1D9C585`; local port 8139
  returned HTTP 200 and Playwright `brweb5` reported 0 errors/0 warnings. Visual flow and
  results/rematch captures are recorded. A later bounded smoke sample recorded
  120,872,306 bytes of WASM transfer and 5.603 ms mean browser rAF; gadget-use capture,
  formal performance closure, multi-browser coverage and human review remain open.

## Phase 1 authority attack-command continuation — 2026-08-03

- Source commit `583106e` adds `OfflineMatchAuthority.TryAcceptAttack`, authority-owned
  weapon cooldown/tick ordering and the production `CombatAttackController` submission
  seam. Duplicate/out-of-order commands, defeated actors, invalid/non-finite inputs and
  cooldown violations are rejected before presentation projectile spawning.
- Repository validation is **0 errors / 0 warnings**. Full EditMode is **102/102** and
  full PlayMode is **55/55**, including the new authority foundation and production
  attack-routing regressions. Core assemblies remain Unity/vendor independent and the
  presentation mutation scan is clean.
- Exact Android artifact is **151,541,453 bytes**, SHA-256
  `10CD9FBC5B720519797702A43BA922F352A28AB6058DDDCBE561C6F7B37CC609`; it was installed
  and launched only on Lava `ST5GDW23LB004392` (`LAVA LXX508`).
- The exact-source Web rebuild was started but stopped at the owner's request before
  Unity reported success. The previous successful Web artifact remains historical only;
  no fresh Web pass is claimed for `583106e`.
- Phase 1 remains **In progress**: projectile collision, remaining presentation-owned
  state, broader authority migration, soak/performance evidence and real network
  authority are still open.

## M1 execution evidence — 2026-08-02

- Pure movement tests: 8/8 EditMode tests passed in `Builds/M1/TestResults/editmode.xml`.
- Movement integration tests: 7/7 PlayMode tests passed in `Builds/M1/TestResults/playmode.xml`.
- Editor validation: `BattleRaja.Editor.BuildEntrypoints.ValidateProject` passed.
- Android: M1 IL2CPP ARM64 development APK built with min API 28/target API 36, installed and launched on Lava LXX508 (Android 14/API 34) and Oppo CPH2487 (Android 16/API 36). Artifact: `Builds/M1/Android/BattleRaja-M1.apk`.
- Web: M1 WebGL2/WebAssembly development build completed under `Builds/M1/Web`, served over local HTTP on port 8001, and returned HTTP 200. Chrome 150 and Edge 150 DOM checks found Unity bootstrap content.
- Android runtime smoke found no M1 application exception after serializing the aim-indicator material. Unity still logs the known Play Asset Delivery `AssetPackManager` class-probe warning on a development APK.

## M2 execution evidence — 2026-08-02

- Pure combat tests: 15/15 EditMode tests passed in `Builds/M2/TestResults/editmode.xml`.
- Combat integration tests: 13/13 PlayMode tests passed in `Builds/M2/TestResults/playmode.xml`.
- Android: M2 IL2CPP ARM64 development APK built at `Builds/M2/Android/BattleRaja-M2.apk`, installed/launched on Lava API 34 and Oppo API 36, with zero fatal application exceptions in the captured logs.
- Web: M2 WebGL2/WebAssembly development build completed under `Builds/M2/Web`, served over local HTTP port 8002, returned HTTP 200, and exposed Unity bootstrap content in Chrome 150 and Edge 150.
- Combat runtime: serialized weapon/input/tuning references were verified in the generated scene; projectile hit, despawn, pooling, collision filtering and dummy reset passed PlayMode coverage.

## M3 execution evidence — 2026-08-02

- Pure fighter/ability tests: 20/20 EditMode tests passed in `Builds/M3/TestResults/editmode.xml`.
- Fighter integration tests: 16/16 PlayMode tests passed in `Builds/M3/TestResults/playmode.xml`.
- Bijli is spawnable in MovementLab with stable fighter/attack/ability IDs, a data-driven electric bolt, directional dash, explicit startup/active/recovery/cooldown states, collision/bounds truncation, a touch ability button and a health/cooldown HUD.
- Android: M3 IL2CPP ARM64 development APK built at `Builds/M3/Android/BattleRaja-M3.apk`, installed/launched on Lava API 34 and Oppo API 36, with zero fatal application exceptions in captured logs. Oppo `pm clear` is restricted by device policy but install/launch succeeded.
- Web: M3 WebGL2/WebAssembly development build completed under `Builds/M3/Web`; local HTTP port 8011 returned 200 and Chrome 150/Edge 150 headless DOM checks found Unity bootstrap content.

## M4 execution evidence — 2026-08-02

- Pure bot tests: 26/26 EditMode tests passed in `Builds/M4/TestResults/editmode.xml`.
- Bot integration tests: 19/19 PlayMode tests passed in `Builds/M4/TestResults/playmode.xml`.
- Seven Bijli bots spawn with unique actor IDs, cached line-of-sight perception,
  seeded utility decisions, aim noise, reaction delay, attack/dash commands and
  bounded stuck recovery. Stress output measured 91 decisions in 2 seconds with a
  maximum decision duration of 0.024 ms in the headless editor.
- Android: M4 IL2CPP ARM64 development APK built at `Builds/M4/Android/BattleRaja-M4.apk`, installed/launched on Lava API 34 and Oppo API 36, with zero fatal application exceptions in captured logs.
- Web: M4 WebGL2/WebAssembly development build completed under `Builds/M4/Web`; local HTTP port 8011 returned 200 and Chrome 150/Edge 150 headless DOM checks found Unity bootstrap content.

## M5 execution evidence — 2026-08-02

- Offline match tests: 31/31 EditMode tests passed in `Builds/M5/TestResults/editmode.xml`.
- Match integration tests: 22/22 PlayMode tests passed in `Builds/M5/TestResults/playmode.xml`.
- `OfflineMatchSimulation` owns a 298-second phase definition, Aandhi radius/damage,
  separated spawns, idempotent elimination, placement, winner and spectator selection;
  MovementLab now contains eight actors, three neutral health pickups and results/rematch
  presentation.
- Pure soak completed 20 accelerated matches with explicit restart state cleanup.
- Android/Web M5 builds and local/device smoke are recorded in `Docs/MILESTONE_REPORTS/M5.md`.

## M6 execution evidence — 2026-08-02

- Gadget tests: 37/37 EditMode and 25/25 PlayMode passed in `Builds/M6/TestResults/`.
- The Domain gadget catalog contains stable Umbrella Guard, Dhol Burst and Tiffin
  Station definitions with one-slot inventory, use/cooldown validation and spawn rules.
- MovementLab contains three gadget pickups, player touch/keyboard use, HUD feedback,
  contextual bot use, directional shield mitigation, bounded Dhol displacement and
  finite destroyable/healing Tiffin stations.
- Android/Web M6 builds, two-device launch smoke and Chrome/Edge local HTTP checks are
  recorded in `Docs/MILESTONE_REPORTS/M6.md`.

## M7 execution evidence — 2026-08-02

- Fighter tests: 40/40 EditMode and 27/27 PlayMode passed in `Builds/M7/TestResults/`.
- Stable Pehel and Maya fighter/special definitions and serialized M7 assets are present;
  MovementLab now contains a two-Bijli, three-Pehel, two-Maya bot mix plus the player.
- Android/Web M7 builds, two-device launch and Chrome/Edge local HTTP checks are recorded
  in `Docs/MILESTONE_REPORTS/M7.md`.

## M8 execution evidence — 2026-08-02

- `BattleRaja.Infrastructure.Networking` contains compile-safe session contracts, a
  deterministic two-client mock and an explicit credential-blocked Photon adapter seam;
  no Photon/PlayFab dependency or secret entered the repository.
- EditMode: 44/44 passed in `Builds/M8/TestResults/editmode.xml`.
- PlayMode: 27/27 passed in `Builds/M8/TestResults/playmode.xml`.
- Android/Web M8 build and desktop-browser smoke are recorded in
  `Docs/MILESTONE_REPORTS/M8.md`; Android validation from this point forward uses only the
  connected Lava phone per owner instruction.
- M8 full gate is **blocked**, not complete, until a real Fusion package/App ID/account
  configuration enables one Lava Android client and one desktop Web client to share a room.

## M9 execution evidence — 2026-08-02

- `AuthoritativeMatchServer` preparation covers eight slots, bot backfill, bounded input,
  server-owned health/elimination, disconnect grace, reconnect and bot takeover without
  Photon/PlayFab or deployment.
- M9 is **blocked**, not complete, because M8 has no validated real two-client Fusion
  session. No online alpha, cross-play, matchmaking or headless-server result is claimed.
- Preparation tests and any M9 regression/build evidence are recorded in
  `Docs/MILESTONE_REPORTS/M9.md`.

## M10 execution evidence — 2026-08-02

- `IProgressionBackend`, `FakeProgressionBackend`, `PlayFabBackendAdapter` and
  `BackendConfiguration.LocalProof` establish guest/link/profile/reward/inventory/
  leaderboard/remote-config contracts without SDKs or secrets.
- M10 is **credential-blocked partial**, not a real service pass. No PlayFab title, SDK,
  cross-device persistence, account recovery or economy deployment is claimed.
- Evidence and test/build status are recorded in `Docs/MILESTONE_REPORTS/M10.md`.

## M11 execution evidence — 2026-08-03

- Release candidate configuration rejects admin tools/secrets; bounded development analytics
  and unavailable crash adapter are compile-safe and service-neutral.
- Closed-test, rollback/support, store-draft and privacy/data-safety worksheets are in `Docs/`.
- M11 artifact, test and smoke evidence is recorded in `Docs/MILESTONE_REPORTS/M11.md`.
- Current continuation HEAD `d993a5b` includes authority-driven Bazaar movement,
  fighter displacement and production Maya decoy lifetime/health,
  functional bounded aim assist,
  authoritative offline assist attribution and focus-hardened Web keyboard/pointer input
  plus an Input System-only project baseline with legacy-scene compatibility bridge,
  in addition to the authority-driven spatial gadget collection
  coverage, immediate live Results/Rematch publication, three repeated rematch cleanup
  cycles and the complete eight-step tutorial walkthrough. Fresh latest-HEAD regression
  is 100/100 EditMode and 51/51 PlayMode; the latest development APK/Web smoke artifacts
  and Lava runtime capture are recorded in `Docs/QA/LATEST_HEAD_BASELINE.md`.
- No public publication, store submission, signing-key use, paid service, legal acceptance or
  final approval is claimed.
- External blockers and exact owner actions are tracked in `Docs/EXTERNAL_SERVICE_GATES.md`.

## M0 evidence retained

- M0 validation, tests and smoke artifacts remain under ignored `Builds/M0/`.
- The M0 foundation commit is preserved as the parent of the current working changes.

## Performance and limitations

- Movement hot paths use cached component references, value types and no per-frame LINQ/managed collection creation.
- No formal Editor Profiler, Android GPU/CPU capture or Web browser performance profile has been collected yet; values remain explicitly unmeasured in `Docs/PERFORMANCE_BUDGET.md`.
- Physical touch interaction, safe-area variations, browser keyboard/mouse focus by manual play, and visual camera comparison still require human playtesting.
- Bijli bolt/dash balance, HUD legibility, dash collision feel and low-end-device profiling remain unmeasured.
- Bot fairness, debug-overlay usefulness, device-tier seven-bot frame time and
  authored navigation remain unmeasured.
- Full five-minute physical match pacing, Aandhi/pickup readability, results UX and
  repeated-runtime memory/object growth remain unmeasured.
- Gadget counterplay/readability, bot gadget value, station scale behavior and device
  performance remain unmeasured. M6 device logs include a non-fatal SphereCollider
  creation warning from the existing projectile pool.
- M7 bespoke specials, final art/audio, tutorial/menu/accessibility UI, fighter balance
  and performance remain unmeasured; the same non-fatal SphereCollider warning persists.

## Approval gates

Review is required before:

1. Adding Photon, PlayFab, monetisation or paid infrastructure.
2. Publishing, deploying, signing for release or submitting to stores.
3. Changing the pinned editor/package baseline or final branding/trademarks.
4. Human review remains required for movement/combat feel, touch ergonomics, camera
   choice, balance, cultural sensitivity, legal/privacy, and release approval.
5. Photon Fusion package/App ID/licence/account approval is required before real online
   session testing; never commit secrets or treat the mock as production networking.

## Milestone 11 continuation — 2026-08-26

Current branch `codex/v1-playstore-release` remains intentionally dirty at HEAD
`fac1c714b9ba2df72b3acf54b40638d0ae122a93`; the exact evidence index is `Docs/V1_RELEASE_PLAN.md`
P15. The current source passes full EditMode **140/140** and PlayMode **80/80**, including
the saved presentation and audio-structure checks. The matching APK/AAB pass the composed
technical checker (**0 errors / 0 warnings**), bundletool universal extraction and static
16 KB alignment checks. The current source also passes the 1,000-seed deterministic replay
soak twice with zero divergence. The exact APK installs on approved Lava
`ST5GDW23LB004392` and the 30-second launch/menu capture has no configured fatal markers.

At that 2026-08-26 checkpoint, the strict 50x shortcut was timing-sensitive and two
final-source attempts reached 70/100 and 76/100 in the 240-360 second window; those
historical results remain retained. Subsequent fixed-tick current-source batches, including
P31, P45 and the fresh P53 rerun, complete 100/100 in-window without loosening the threshold.
The accelerated path is still not same-seed determinism evidence. Human full-route/touch/
accessibility, sustained performance/thermal/battery, runtime 16 KB, authored art/audio/
cultural review, signing/package identity, privacy/Data Safety, content rating and Play
Console actions remain open; this is not a Play-ready completion claim.

## Milestone 11 exact clean-source refresh — 2026-08-27

The reviewed runtime/presentation source is clean at commit
`2f9a6a0151e3b0c2359d9b0f8892c28e6404ec4b`; the full evidence index is `Docs/V1_RELEASE_PLAN.md`
P16. Full EditMode is **140/140**, full PlayMode **80/80**, and the exact-source deterministic
soak is **1/1** with 1,000 seeded matches executed twice and zero divergence. Matching APK/AAB
artifacts, bundletool universal extraction and direct/extracted 16 KB zipalign evidence are
recorded with hashes in P16. The exact APK was reinstalled on approved Lava
`ST5GDW23LB004392`; the fresh six-sample launch/menu capture found no configured fatal markers
and thermal status 0 before/after.

This is a clean technical Android candidate, not a Play-ready claim. The pacing status in this
historical refresh is superseded by the exact-source P31 batch below. Human full-route/touch/accessibility,
sustained match performance,
runtime 16 KB, final authored/cultural review, package identity/signing, privacy/Data Safety,
content rating, store assets and Play Console actions remain owner-controlled or unverified.

## Milestone 11 exact-source production-bot refresh — 2026-08-27

The latest clean documentation tip is `90670ff`; its runtime-bearing source remains
`126714a`. The exact-source production-bot release assertions completed **100/100** seeded
matches with **100/100** terminal results, **100/100** in the 240–360 second pacing window,
**100/100** bot-to-bot damage, **100/100** combat eliminations, and **0/100** Aandhi-only
resolutions. Protected-warmup damage, invalid positions and continuous stuck ticks were all
zero. All three fighters and all three gadgets were exercised successfully. The full PlayMode
test report remains **84/84**; report, log and batch hashes are recorded in `Docs/V1_RELEASE_PLAN.md`
P31.

This closes the automated production-bot pacing and safety gate. It does not close human
touch/tutorial/accessibility, sustained performance/thermal/battery, runtime 16 KB, final
authored/cultural review, package identity/signing, privacy/Data Safety, content rating, store
asset or Play Console gates. No Play-ready or publication claim is made.

## Milestone 11 replay persistence continuation — 2026-08-27

The remaining offline replay gap is now closed at the machine-verifiable boundary. The
development-only production bot harness captures ordered movement, attack, ability, gadget
and Pehel charge-step submissions, complete replay header/content configuration, per-tick
participant snapshots and canonical hashes into a checksummed Unity-independent `.brr` file.
A one-match production smoke passed **86/86 PlayMode** and emitted a 9,180-frame artifact;
the exact file was read and fully re-executed with snapshot/hash verification in **141/141
EditMode**. Serializer round-trip and corruption rejection are covered by the same baseline.
The current source also passed the full 100-seed production-bot release batch (100/100
terminal and in-window, 94/100 with at least three combat eliminations, 100/100 with
bot-to-bot damage) and two independent seed-9101 runs reproduced the same command/replay
digest.
See `Docs/V1_RELEASE_PLAN.md` P42 and `Docs/DECISIONS.md` ADR-065.

The post-change release-shaped APK/AAB were rebuilt from the current source and remain
temporary-ID, debug-signed local artifacts. Cross-machine floating-point parity, physical
16 KB ARM64 runtime coverage, normalized performance/GC/GPU/battery approval, authored
presentation/cultural review, final signing/identity, privacy/Data Safety, content rating
and Play Console work remain open and owner-controlled.

## Milestone 11 faceted fighter presentation continuation — 2026-08-28

The focused art source is cleanly committed at `816d9ac` (`art: replace fighter primitives
with faceted silhouettes`). The three saved fighter presentation prefabs now use distinct
repository-owned faceted low-poly mesh profiles instead of the former primitive-like body
and accessory recipes. A new PlayMode regression enforces a 260-vertex presentation floor
and exactly three distinct fighter mesh profiles while preserving the render-only authority
boundary. Full EditMode is **141/141**, PlayMode is **87/87**, and repository validation is
**0 errors / 0 warnings**.

The exact rebuilt local APK is 40,542,342 bytes (SHA-256
`0517EE901A9EAE943140538366B0574E893DC6BD66A5D1714D630C2379EF5FAC`) and the AAB is
36,367,513 bytes (SHA-256
`BF52E649BFD92F277F5C9933A7FDF34FFB25410F1D5A18EF6FC3097AA31BA331`). Both pass the
offline manifest, ARM64/static 16 KB, bundletool, zipalign, v3 verification and store-
creative dimension checks. The exact APK installed on approved Lava
`ST5GDW23LB004392`, reached the live opening match by touch, and produced a bounded
30-second six-sample diagnostic with thermal status 0 and no configured fatal markers.
Evidence and all hashes are indexed in `Docs/V1_RELEASE_PLAN.md` P43.

This remains a generated V1 presentation baseline, not final commissioned art or Play
readiness. Physical 16 KB runtime behavior, sustained performance, full tutorial/all-
fighter/accessibility and human feel review, final identity/signing, privacy/Data Safety,
cultural/legal approval and Play Console actions remain owner-controlled. No remote push
was performed.

## Milestone 11 saved environment and runtime presentation continuation — 2026-08-29

The exact local art/runtime commit is `ac45479` (`art: integrate saved Bazaar environment
and runtime mesh fallbacks`). It adds a saved textured `BazaarBastionProduction.prefab`
with a 32×32 three-submesh ground mosaic, deterministic environment textures/materials,
themed Bazaar dressing and a backdrop LOD group. The production scene removes the legacy
runtime architecture instance and binds the saved prefab with fallback disabled. Fighter
prefabs now carry saved far-silhouette LOD meshes, while runtime feedback/projectile/gadget/
decoy fallback visuals use shared custom mesh geometry; Maya decoy targetability is retained
through an explicit capsule collider.

Repository validation is **0 errors / 0 warnings**, full EditMode is **141/141** and the
exact-source PlayMode/production-bot run is **87/87**. The strict 100-match batch is
**100/100** terminal and in the 240–360 second window, **91/100** with at least three
combat eliminations, **100/100** with bot-to-bot damage, **0/100** Aandhi-only and zero
protected/invalid samples; Umbrella, Dhol and Tiffin each exercised in all 100 matches.
The complete gate table and report hashes are in `Docs/V1_RELEASE_PLAN.md` P45.

This remains a generated presentation baseline, not final commissioned art or Play
readiness. Fresh Android rebuild/device evidence, physical 16 KB behavior, sustained
performance, full tutorial/all-fighter/accessibility/fun review, authored art/audio/cultural
approval, final signing/identity, privacy/Data Safety, content rating and Play Console work
remain open and owner-controlled. The two prompt files in `PROMPTS/` remain intentional
uncommitted owner files.

## Milestone 11 exact d0de949 Aim-state and Android evidence continuation - 2026-08-30

The latest runtime/art source is `d0de9499e764045d72dbf092da4c8f2d85fb0b36`. It adds a
dedicated render-only fighter Aim animation state driven by existing input intent; no offline
authority, balance, networking or package-policy behavior changed. Repository validation is
**0/0**, EditMode is **141/141**, PlayMode is **87/87**, and the exact APK/AAB release checker
passes **0/0**. The exact artifact hashes and route table are recorded in `Docs/V1_RELEASE_PLAN.md`
P46.

The exact APK installed successfully on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
Android 14/API 34) and real touch reached menu, Solo Raja, all three fighter cards, live
actions/Aandhi/elimination/spectator/results/rematch, settings/lifecycle and tutorial
`8/8 COMPLETE`. The local evidence manifest is
`Builds/Local/Device/Performance/20260830-lava-d0de949-aim/manifest.json`. This remains a
bounded technical candidate: the phone reports 4 KB pages, raw telemetry does not establish
normalized sustained budgets, and final authored/cultural/fun/accessibility approval,
package identity/signing, privacy/Data Safety, content rating and Play Console actions remain
owner-controlled. The prompt files remain intentional uncommitted owner work.

## Milestone 11 exact terminal-outcome presentation and Android evidence - 2026-08-30

The latest runtime/art source is `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`. It saves explicit
Victory/Defeat VFX assets and routes authoritative result placement into persistent
render-only fighter presentation; no offline rule, timing, authority, networking or reward
behavior changed. Repository validation is **0/0**, EditMode **141/141**, PlayMode **88/88**
and the exact APK/AAB release checker is **0/0**. The full artifact and test hash index is
`Docs/V1_RELEASE_PLAN.md` P47.

The matching APK installed successfully on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`,
Android 14/API 34). Real touch reached the complete local route, settings/lifecycle and
tutorial `8/8 COMPLETE` via SKIP; the player reached results at placement #6. Evidence and
hashes are indexed in `Builds/Local/Device/Performance/20260830-lava-5d136fb-outcome/manifest.json`.
Raw telemetry is **273,885 KB PSS / 385,796 KB RSS / 80,020 KB graphics PSS**, thermal status 0,
and no configured app fatal/ANR/SIGSEGV marker was found. This is bounded technical evidence:
the phone reports 4 KB pages, `gfxinfo` has no usable frame histogram, and sustained
performance/runtime-16-KB approval is not claimed.

The project remains a generated presentation baseline / Android offline release candidate in
progress, not Play-ready. Final authored/cultural/fun/accessibility approval, final package
identity and production signing, privacy/Data Safety, content rating, genuine 16 KB runtime,
sustained performance/thermal/battery validation and Play Console upload/public deployment
remain owner-controlled. The two prompt files under `PROMPTS/` remain intentional
uncommitted owner work.

## Milestone 11 P48 focused Android performance evidence - 2026-08-30

The exact terminal-outcome candidate was exercised on approved Lava
`ST5GDW23LB004392` for 180 seconds with 36 five-second samples during a live match. After
startup warm-up, PSS stayed within **261,702-273,769 KB**, RSS within **384,112-396,696 KB**,
graphics PSS within **75,792-81,936 KB**, and `top` reported **87.5-118.0%** instantaneous
process CPU (100%-per-core scale). Battery remained 76% while USB-powered, thermal status
remained 0, and no configured fatal markers were found. The raw manifest and 30-second
Perfetto trace are indexed in `Docs/V1_RELEASE_PLAN.md` P48. The capture strengthens local
stability evidence but does not close normalized frame/GC/GPU, unplugged battery, genuine
16 KB runtime or human approval gates.

## Milestone 11 P49 genuine 16 KB Android 16 AVD smoke - 2026-08-30

The exact terminal-outcome APK from `5d136fb` installed and launched on the genuine
`BattleRaja_16K` Android 16/API 36 AVD with host-GPU rendering. The model is
`sdk_gphone16k_x86_64`, the ABI list includes `x86_64,arm64-v8a`, and
`adb shell getconf PAGESIZE` returned **16384**. The clean branded-menu capture and app-scoped
logcat are under `Builds/Local/Device/Performance/20260830-16k-5d136fb/host-gpu/`; no
configured fatal, ANR, SIGSEGV, SIGABRT or shader-link marker was found.

The 90-second harness capture (18 five-second samples) is indexed in
`Docs/V1_RELEASE_PLAN.md` P49. Warm-up PSS was **435,726-436,966 KB**, RSS
**617,304-621,236 KB**, GraphicBufferAllocator estimate **31,416 KB**, and process `top` CPU
**96.1-123.0%** on Android's 100%-per-core scale; thermal status was 0. This is a genuine
host-GPU 16 KB page-size/runtime smoke result, but emulator CPU/battery and the missing Unity
frame histogram do not establish a product-tier performance budget.

The same AVD under SwiftShader showed URP/Lit uniform-limit corruption and is retained as a
superseded renderer-profile diagnostic. Physical ARM64 16 KB coverage, other GPU profiles,
normalized performance, final authored/accessibility/cultural approval and owner-controlled
identity/signing/privacy/Play gates remain open. The two prompt files under `PROMPTS/` remain
intentional uncommitted owner work.

## Milestone 11 P50 Lava live-match SurfaceFlinger diagnostic - 2026-08-30

The exact terminal-outcome APK from `5d136fb` was relaunched on approved Lava
`ST5GDW23LB004392` through Rematch. A 45-second live Solo Raja sample produced **126 valid
SurfaceFlinger present timestamps** and **125 intervals**, excluding one `Long.MaxValue`
sentinel. Middle-column min/median/p95/p99/max intervals were **16.447 / 16.534 / 16.565 /
33.078 / 33.367 ms**; three intervals exceeded one refresh period and one exceeded 2×. The
raw evidence and summary are indexed in `Docs/V1_RELEASE_PLAN.md` P50 (summary SHA-256
`21369E4FC3BF33BF1DB234BE2F23F1A8D32BD45D0DF29F8682DC90D17489B144`).

The end state was player defeat/spectating during Aandhi closing. End telemetry was
**277,284 KB PSS / 400,500 KB RSS / 80,052 KB graphics PSS**, battery **75% / 4,120 mV /
31 C** while USB-powered, thermal status 0 and no configured fatal markers. This is bounded
raw compositor evidence only; Lava reports 4 KB pages, `gfxinfo` has no usable Unity
histogram, and normalized performance, physical 16 KB, final authored/accessibility/cultural
approval and owner-controlled identity/signing/privacy/Play gates remain open.

## Milestone 11 current release-handoff documentation tip — 2026-08-30 07:05 IST

The docs-only continuation is aligned with `origin/main`. It adds the owner-selectable V1 release-notes, tester,
known-issues and support handoff at `Docs/RELEASE/V1_RELEASE_NOTES_AND_SUPPORT_DRAFT.md`,
removes stale “procedural placeholders” wording from the Play metadata draft, and labels the
older multi-platform store copy as superseded for V1 Android. Store-creative dimensions,
repository validation (**0 errors / 0 warnings**), `git diff --check` and Git LFS pointer
validation pass. No runtime source, build artifact or package identity changed; the exact
`5d136fb` candidate evidence and remaining owner/human gates in P47-P50 remain authoritative.

## Milestone 11 P52 player-facing Umbrella Guard regression - 2026-08-30 07:21 IST

The test-only tip is `4c4c67cbbc20062e3723cc90ee3bb7c266bbeda4`. It adds a direct MovementLab
PlayMode assertion that the player can collect and use Umbrella Guard, consume the one-slot
inventory, receive the configured shield duration and feedback, record the successful-use
counter, mitigate front-facing projectile damage and leave Aandhi damage unmitigated. Full EditMode is **141/141**, PlayMode is **89/89**, repository validation is
**0 errors / 0 warnings**, and `git diff --check` passes. No runtime-bearing code, package,
APK/AAB or P47-P50 device evidence changed, so the exact `5d136fb` candidate artifacts remain
authoritative. Final authored/cultural/fun/accessibility approval, sustained performance,
physical 16 KB, signing, privacy/Data Safety, content rating and Play Console actions remain
owner-controlled; the two prompt files remain intentional uncommitted owner work.

## Milestone 11 P53 current exact-source production-bot gate refresh - 2026-08-30 08:03 IST

The current fixed-tick production-bot harness was rerun with Unity `6000.5.6f1`, 100
matches, release assertions enabled, playback scale 50 and base seed 9101. The runtime/art
source remains `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`; the current documentation tip was
`7167f33839a15a0f0c7e11eb27a97302f93c0b37`. PlayMode passed **89/89**. All **100/100**
matches reached terminal results in the 240-360 second window (306.0135193 seconds each),
had combat and bot-to-bot damage, and exercised Umbrella Guard, Dhol Burst and Tiffin Station;
zero protected-warmup or invalid-position samples were observed. Exact report, XML/log
hashes and aggregate telemetry are indexed in `Docs/V1_RELEASE_PLAN.md` P53.

This closes the current automated production-bot pacing/safety gate locally. The 50x path
remains an accelerated diagnostic rather than same-seed determinism evidence; P10 retains
the real-time determinism record. Human full-route/touch/accessibility/fun review, normalized
sustained performance, physical 16 KB, final authored/cultural approval, signing/package
identity, privacy/Data Safety, content rating and Play Console actions remain owner-controlled.

## Milestone 11 P54 saved-fighter presentation and portrait settings refresh - 2026-08-30

The current presentation source is committed at `ae0d294c97fa62386317e7e5ebf77cd5ebcbafee`,
following the saved-identity integration in `3d8fda7`. `FighterPresentation` suppresses the
legacy root capsule only after a saved Bijli, Pehel or Maya production prefab provides
mesh/skinned renderers; the root dash `TrailRenderer` remains available, saved renderers are
the tint targets, and per-renderer base colors survive hit-flash reset. `OfflineMatchHud` uses
a centered modal for compact portrait settings while wide layouts retain the side sheet.
Gameplay authority, collision, input, timing, networking and reward code are unchanged. ADR-071
records the decision.

Fresh validation is **0 errors / 0 warnings**, EditMode **141/141**, and PlayMode **89/89**.
The rebuilt temporary-ID APK/AAB pass the offline manifest, ARM64/static 16 KB and store-
creative technical gates; exact hashes and logs are indexed in `Docs/V1_RELEASE_PLAN.md`
P54. The exact APK installed and launched on approved Lava `ST5GDW23LB004392` only, and
fresh captures show the saved faceted identities in the live arena plus the centered portrait
settings modal. A current-source 180-second Lava capture (36 five-second samples) found no
configured fatal markers; warm-up-excluded PSS was **238,249-244,525 KB** (average
**241,729 KB**), RSS **360,572-366,836 KB** (average **364,046 KB**), graphics PSS
**70,288-74,396 KB** (average **72,267 KB**), and process CPU **35.7-62.5%** (average
**57.8%**) on Android's 100%-per-core scale. The run was USB-powered and Lava reports
4 KB pages, so this is bounded stability evidence rather than normalized performance,
unplugged endurance or physical 16 KB proof. Oppo was not used.

The product remains a generated presentation baseline / Android offline release candidate in
progress, not Play-ready. Final authored/cultural/fun/accessibility approval, genuine physical
16 KB runtime, normalized sustained performance, final package identity/signing, privacy/Data
Safety, content rating and Play Console actions remain owner-controlled. The two prompt files
under `PROMPTS/` remain intentional uncommitted owner work.

## Milestone 11 P55 final-circle audio cue and current Lava endgame evidence - 2026-08-30

The current source tip is `56df201` (`audio: add final-circle escalation cue`). It adds the
owned generated `ZoneFinalCircle.wav` source and wires a one-shot cue to the authoritative
`MatchPhase.FinalCircle` transition through `BattleRajaAudioDirector`. Gameplay authority,
damage, cooldown, movement, pickup, reward, networking and package-policy code are unchanged;
final mix, loudness and voice-priority review remain open.

Fresh static validation is **0 errors / 0 warnings**, EditMode is **141/141**, and PlayMode is
**89/89**. The current temporary-ID APK/AAB are the exact artifacts listed in P55 of
`Docs/V1_RELEASE_PLAN.md`; the post-commit release checker is **0 errors / 0 warnings** and
confirms package `com.example.battleraja.m11`, version `1.0.0`/code `100`, API `28/36`,
offline network permissions absent, ARM64-only payload and static 16 KB alignment.

The APK installed successfully only on approved Lava `ST5GDW23LB004392`. Current evidence
under `Builds/Local/Device/final-circle-20260830/` reaches the branded menu, all three fighter
cards, settings toggles/restoration, live combat, Aandhi Final Circle, Results and a HOME →
relaunch lifecycle. Filtered app logs contain no configured fatal/ANR/native-crash markers.
Lava reports 4 KB pages, so physical 16 KB compatibility remains open; the host-GPU Android 16
AVD result is profile-specific. Final authored art/audio, cultural/fun/accessibility review,
normalized performance, battery/thermal endurance, signing, legal/privacy, rating and Play
Console actions remain owner-controlled. The truthful state remains **Prototype — Android
offline release candidate in progress**; the two prompt files are intentional uncommitted owner
work.

## Milestone 11 P66 lifecycle input hardening and exact Android candidate rerun - 2026-08-31

Source tip `e603ce7e7f1cb279f5e3e9d606ea5eae89603ecb` clears transient player input on
Android pause across the adapter, virtual stick and attack/ability/gadget buttons, while the
HUD clears the adapter before its lifecycle pause boundary. Focused lifecycle PlayMode is
**1/1**, full EditMode is **141/141**, full PlayMode is **92/92**, and static validation is
**0 errors / 0 warnings**. ADR-076 records the presentation/input-only decision; gameplay
authority, simulation timing, replay and networking boundaries are unchanged.

The exact rebuilt APK is 40,679,115 bytes (SHA-256
`349F02C67DE4CC801C5CB81B9CEC375A18D89B136C1A3AD9BB9549E9640A41CB`) and AAB is 36,504,445
bytes (SHA-256
`9A5BE261D2504007BCBAF4105568F19437CBA8A4DEFAA3383371DE35386D51E0`). The APK was installed
only on approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34), reached a live
Solo Raja/Bijli Opening Fight, stayed at Android HOME for approximately five seconds, and
returned to the same RESUMED Unity activity. The route manifest is
`Builds/Local/Device/final-circle-20260830/p66-lifecycle-route/p66-lifecycle-route-manifest.json`
(8,192 bytes; SHA-256
`217589DAE7592EC397328F12D8C3DF88246B7AEE035776584DF5FE9624499103`). Paired captures retain
`ALIVE 8` and `ZONE 14.0 > 11.0`; Lava reports 4 KB pages and route logcat has zero
configured fatal/ANR/native/managed-crash markers. Bundletool 1.18.3 universal extraction,
zipalign and temporary v3 signature verification also pass for the exact AAB.

This is bounded exact-device evidence, not a claim of full lifecycle comfort, physical 16 KB,
normalized performance, endurance, final authored/cultural/accessibility/fun approval,
production signing, legal/privacy completion or Play approval. The truthful state remains
**Prototype — Android offline release candidate in progress**.

## Milestone 11 P65 exact P61 physical all-gadget route - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` remained installed on
approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34). No rebuild or source
change was made. The manifest is
`Builds/Local/Device/final-circle-20260830/p65-gadget-route/p65-gadget-route-manifest.json`
(4,269 bytes; SHA-256
`48598855AEEDCD286C837219632ACFC6B972CEC0CB69B7E9D8EE28163BDED807`).

The route physically collected and used Tiffin Station, Dhol Burst and Umbrella Guard. The
captured HUD feedback was `TIFFIN STATION DEPLOYED` (`13-tiffin-aimed.png`), `DHOL BURST`
(`19-dhol-use.png`) and `UMBRELLA GUARD ACTIVE` (`24-umbrella-use.png`), with pickup states
captured in `18-left-to-dhol.png` and `23-diagonal-left-progress.png`. App-scoped logcat
SHA-256 is `7237171A826BE8F5308B871270F1477551296A903F5E2B5DC626669DAE113E9F`, with zero
configured fatal/ANR/native/managed-crash markers. Tutorial SKIP/CLOSE CARD was used after
fresh activity starts; this is bounded device evidence, not action-by-action tutorial,
comfort, audio, cultural, endurance, normalized performance, physical 16 KB or Play approval.

The truthful state remains **Prototype — Android offline release candidate in progress**, not
Play-ready.

## Milestone 11 P64 immediate-after-resume lifecycle pause observation - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` was started on approved
Lava `ST5GDW23LB004392` after tutorial completion. A live Opening Fight baseline was captured,
Android HOME was held for approximately five seconds, and the Unity activity was resumed with
a screenshot approximately 220 ms later. The route manifest is
`Builds/Local/Device/final-circle-20260830/p64-lifecycle-pause-manifest.json` (3,354 bytes;
SHA-256 `DC3677479D95C4E2EBA7DD79C6E46C03418D58F379AE32708A1C5B2FFCB4EA99`).

Both captures show Bijli at 85/85 HP in Opening Fight with `ALIVE 8` and `ZONE 14.0 > 11.0`;
the warning countdown is 11.6 seconds before HOME and 10.8 seconds in the immediate
post-resume capture. Unity logcat records the pause/stop and start/resume callbacks on the
same process. This supports a bounded pause-invariant observation; repeat across phases and
held-input cases for final comfort approval. Full lifecycle/endurance, normalized performance,
physical 16 KB, final identity/signing, legal/privacy, and Play gates remain open. The truthful
state remains **Prototype — Android offline release candidate in progress**, not Play-ready.

## Milestone 11 P63 bounded exact-candidate live-match performance refresh - 2026-08-31

The exact P61 APK from `f80b565372d7446e070cf1a37de042bd018345c4` remained installed on
approved Lava `ST5GDW23LB004392` during a requested 120-second performance-harness capture at
five-second intervals. Movement swipes plus attack, ability and gadget taps were sent while
the run advanced from Opening Fight to Final Circle, player defeat/spectating and Alive 4. Raw
evidence is under
`Builds/Local/Device/Performance/20260831-lava-f80b565-p62-perf120/`; the 24-sample manifest is
5,611 bytes (SHA-256
`EBB5C4F43E27E0579E0EAC3E116E8A3DB0F47DEB35EAEDEE7C05A945FF067D7D`) and the checked summary
is 3,478 bytes (SHA-256
`4413810D5295345586E497846AA4A55C6B83AC9645FA807EB159ADDEA6B4467B`).

After the first four samples, PSS was 223,078-223,273 KB (average 223,190.8 KB), RSS was
356,972-357,168 KB (average 357,085.0 KB), graphics PSS was 17,480 KB, and instantaneous
`top` CPU was 59.2-131.0% (warm average 113.8%). Thermal status stayed 0; HAL CPU/GPU was
43.537-49.148 C and skin was 39.305-39.969 C. USB-powered battery changed from 44% /
3,841 mV / 35 C to 43% / 3,825 mV / 35 C. The app logcat has no configured fatal marker.
Unity `gfxinfo` still has no usable frame histogram and Lava reports 4 KB pages, so this is
bounded raw telemetry only; normalized performance, endurance, physical 16 KB and owner review
remain open. The truthful state remains **Prototype — Android offline release candidate in
progress**, not Play-ready.

## Milestone 11 P62 accessibility, persistence and lifecycle route - 2026-08-31

The exact P61 APK (`f80b565372d7446e070cf1a37de042bd018345c4`) was freshly installed on
approved Lava `ST5GDW23LB004392` after clearing app data. The route exercised in-match Settings
& Accessibility, left-handed controls, reduced flashes, high contrast, aim assist and text
scale, observed the left-handed live action layout, verified persistence after return/relaunch,
and restored defaults. The route manifest is
`Builds/Local/Device/final-circle-20260830/p61-accessibility-route/p62-accessibility-route-manifest.json`
(8,901 bytes; SHA-256
`0DAE55EBFC6DD57F78D9BF28D0A9172659102FFB2D30BFBB2ADC8AB610D4BCF9`).

A bounded live background/resume pair returned to Opening Fight. The warning timer moved during
the observation window, so full simulation pause invariance is not claimed. The route log has no
configured fatal/ANR/native-crash marker; smaller-device coverage, accessibility comfort,
sustained lifecycle testing and human approval remain open.

## Milestone 11 P61 compact portrait results metrics - 2026-08-31

The runtime/test checkpoint is `f80b565372d7446e070cf1a37de042bd018345c4`. Compact results
now spell out `KOs`, `AST` and `DMG` instead of the ambiguous single-letter `K/A/D` sequence,
and portrait result type rises from 16 px to 18 px. ADR-075 records the presentation-only
decision; placement, damage, rewards and authority state remain unchanged. The change is
covered by full EditMode **141/141** and PlayMode **92/92** reruns.

The rebuilt temporary-ID APK/AAB passed repository validation and the release checker with
**0 errors / 0 warnings**. APK SHA-256 is
`922DB673B579BD88705BB4483C36A21A2D903A1CD05D2C2F50F47D26A564EA91`; AAB SHA-256 is
`FDBCED4B1D6D69E4F637C283298188B037D58F152DE4D9B69F897147F85093CF`.

On approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 4 KB pages), the
exact rebuilt APK reached live match, defeat/spectator, Aandhi/final circle, Results and
REMATCH. The result capture visibly shows `KOs`, `AST` and `DMG`; the route index is
`Builds/Local/Device/final-circle-20260830/p61-results-copy-manifest.json` (4,936 bytes;
SHA-256 `0C868F852FE57C409B914871845DF317EFC7C89398CEFD3A8AB98E5F1137671F`). The app log
has no configured fatal/ANR/SIGSEGV/SIGABRT marker; known Lava graphics diagnostics remain
recorded. The truthful state remains **Prototype — Android offline release candidate in
progress**; final authored/cultural/fun/accessibility, normalized performance/endurance,
physical 16 KB, identity/signing, legal/privacy, rating and Play gates remain open.

The same exact rebuilt artifact also has bounded all-fighter action evidence: Bijli, Pehel and
Maya selection/live/attack/Tiffin checkpoints plus Opening-phase ability feedback. The route
index is `Builds/Local/Device/final-circle-20260830/p61-all-fighter-manifest.json` (5,506 bytes;
SHA-256 `9FA8762B504330189A686605A6DD60836C4992B5359E526B91E8527A813E1598`). Accessibility
comfort, action-by-action tutorial comfort, repeated-match comfort and human approval remain
open.

## Milestone 11 P60 compact portrait HUD copy - 2026-08-31

The runtime/test checkpoint is `c3cfb27e08f13ecf4b91a4234269aa11e675bfe9`. The compact
portrait match status now spells out `ZONE` instead of the ambiguous one-letter `Z`; phase,
alive count, radii, warning and closing state remain unchanged. ADR-074 records the
player-facing copy decision and the change is covered by full EditMode **141/141** and
PlayMode **92/92** reruns.

The rebuilt temporary-ID APK/AAB passed repository validation and the release checker with
**0 errors / 0 warnings**. APK SHA-256 is
`4EFF24C7251DD57C2FCAA4D280C369175D33FA6C8D26B969ABBAA72D9EAF32A7`; AAB SHA-256 is
`D60B09EE6324C0AA75781BF1F9DB8461A6A1AE05D788A9232EA227DBC1349936`.

On approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 4 KB pages), the
exact APK launched through Play Offline → Solo Raja → Bijli and captured the compact HUD
as `GET READY` / `ALIVE 8  ZONE 14.0 > 14.0`. The route index is
`Builds/Local/Device/final-circle-20260830/p60-zone-copy-manifest.json` (3,822 bytes;
SHA-256 `B235CAC4A041644B7A05FED6C613A5BB2563CDD6929C19EF9E2B6F445F1C7E39`). The app
log has no configured fatal/ANR/SIGSEGV/SIGABRT marker; known Lava graphics diagnostics
remain recorded.

This is a localized readability improvement with bounded device evidence, not final visual,
localization, comfort, performance, thermal, battery, memory-growth, physical 16 KB or Play
approval. The truthful state remains **Prototype — Android offline release candidate in
progress**, and the two prompt files remain intentional uncommitted owner work.

## Milestone 11 P57 tutorial elimination target readability and refreshed touch route - 2026-08-30

The latest source checkpoint is `c9e3d3091a38852be794f74ad97420b91461599a`
(`tutorial: place elimination target in readable lane`). The change is limited to the
TutorialArena layout/build helper and PlayMode coverage: actor 11 is placed at `(0, 1, -3.2)`
in the open south lane, with production spawns, the MovementLab fixture, offline authority
and package/network policy unchanged.

Validation after the checkpoint remains green: static validation **0 errors / 0 warnings**
(post-checkpoint log SHA-256 `7EF09129DBD03921DF243F43AC65AE932A8C74C4DD76FAD8E6A013BFC804E322`),
EditMode **141/141**, target checkpoint PlayMode **90/90**, and focused local-projectile
follow-up PlayMode **91/91**. The exact APK is 40,681,055 bytes (SHA-256
`DA6CC4B6B2F4160A2D62BDE9FFA4C1686D0D401AB0F354604AF8AC077269222B`); the exact AAB is
36,506,363 bytes (SHA-256
`A379C725D46E8829F9DE9EEF59E49D906E817F9F6392E031EE065A532DD6C37C`). The release checker
is **0 errors / 0 warnings** and still reports temporary package `com.example.battleraja.m11`,
API `28/36`, offline network permissions absent, ARM64-only payload and static alignment.
These are local debug-signed candidates, not production-signed uploads.

On approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 4 KB pages), Aim Assist
was enabled in Settings & Accessibility. A fresh real-touch route completed MOVEMENT, AIM,
BASIC ATTACK, ABILITY, GADGET and AANDHI, then unlocked the ELIMINATION lesson after a short
forward alignment movement. The exact step-7 capture is
`Builds/Local/Device/final-circle-20260830/tutorial-target-aimassist-moveclose-step7-final.png`
(SHA-256 `27E22DED8D06035806F3EE85B339286A28019025B20B32BC099BC4BE2B77A76E`). The VICTORY
card then resolved to a real `RESULTS / WINNER YOU / #1` placement (`tutorial-target-after-wait.png`,
SHA-256 `9710C1C2A111652CC79E625D0D46E177BF65FC9490636F3FD6C47FCF088FBC7D`). Tapping
`FINISH TUTORIAL` produced `TUTORIAL COMPLETE 8/8` over that result
(`tutorial-target-finished-results.png`, SHA-256
`76E40B9F20BCDAFAE13FF217029CA75A3B92D8B9A23B58F98802C224493BAA49`). Full exact paths,
earlier card captures, device facts, UI-tree interpretation and the app-scoped logcat are
indexed in `Docs/V1_RELEASE_PLAN.md` P57 and
`Builds/Local/Device/final-circle-20260830/tutorial-target-touch-route-manifest.json`.

The product remains a **generated presentation baseline / Android offline release candidate
in progress**, not Play-ready. The refreshed target closes the captured touch-elimination
layout defect and the route now records terminal placement plus tutorial completion; it does
not close full action-by-action victory/rematch comfort, final authored/cultural/fun/
accessibility approval, normalized performance and endurance, physical 16 KB runtime,
package identity/signing, privacy/Data safety, IARC/content rating or Play Console gates.
The two prompt files under `PROMPTS/` remain intentional uncommitted owner work.

## Milestone 11 P56 real-touch tutorial comfort probe - 2026-08-30

The exact current candidate from source `56df201` was replayed on approved Lava
`ST5GDW23LB004392` with AIM ASSIST enabled through the in-app settings surface. Real touch
advanced the MOVEMENT, AIM, BASIC ATTACK, ABILITY, GADGET and AANDHI lessons. The exact
screenshots, hashes, device facts, UI-tree capture and app-scoped logcat are indexed in
`Builds/Local/Device/final-circle-20260830/tutorial-touch-route-manifest.json` and the P56
section of `Docs/V1_RELEASE_PLAN.md`.

The ELIMINATION lesson remained waiting after repeated real aim/attack attempts. A later
terminal Results panel displayed `WINNER YOU`, `#1 YOU`, `KO 0`, `D 0`, meaning Aandhi resolved
that route without satisfying the player-elimination lesson. Therefore action-by-action
elimination/victory tutorial comfort remains **open**. P47/P54 physical `8/8 COMPLETE` evidence
used SKIP; editor tests cover deterministic progression but do not replace touch comfort review.

The probe's Unity UI tree exposed only a SurfaceView, and its app-scoped logcat contained no
configured fatal/ANR/native-crash markers. The product remains **Prototype — Android offline
release candidate in progress**, not Play-ready. Final authored/cultural/fun/accessibility
approval, normalized sustained performance, battery/thermal endurance, physical 16 KB runtime,
final package identity/signing, privacy/Data Safety, content rating and Play Console actions
remain owner-controlled. The two prompt files under `PROMPTS/` remain intentional uncommitted
owner work.

## Milestone 11 P58 dismissible tutorial completion card and exact-candidate route - 2026-08-31

The current UI/test checkpoint is `888421f0b332a2e5b9b41fcb6ae669adec836612`
(`ui: release results after tutorial completion`). In the completed tutorial state,
`TutorialOverlay` now changes its secondary action from `SKIP` to `CLOSE CARD`; dismissal
hides only the tutorial surface so the authoritative Results/REMATCH/MENU controls can be
inspected. Replay and return-to-menu behavior remain available, and the new
`CompletedTutorialCanDismissOverlayForResultsAndRematch` PlayMode regression checks the
state transition and idempotent dismissal.

The exact candidate was rebuilt from that source content: static validation is **0 errors / 0
warnings**, EditMode is **141/141**, PlayMode is **92/92**, and the release checker is **0
errors / 0 warnings**. The APK is 40,682,359 bytes (SHA-256
`B3D4EF4749270FDAD30474113683E050693BFA013173FF5EB1E3848C26C87F44`); the AAB is 36,507,683
bytes (SHA-256
`CC5D2B362EA8330BB3FA22E93D530CD018D4933305744E26EF2504300B88D6F6`). Package, version,
offline permissions, ARM64 payload and static alignment remain the temporary-ID/debug-signed
candidate recorded in `Docs/V1_RELEASE_PLAN.md` P58.

On approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, 4 KB pages), the
exact APK was launched, Tutorial was opened, in-app SKIP reached `TUTORIAL COMPLETE 8/8`,
and real touch on `CLOSE CARD` exposed the live HUD. The match then reached Results with
REMATCH/MENU, and REMATCH opened a fresh TutorialArena movement card. Evidence and hashes are
under `Builds/Local/Device/final-circle-20260830/` in
`tutorial-dismiss-route-manifest.json` (5,230 bytes; SHA-256
`85D7E29C683C04D71C12F9FADB7720C49F68D6947A5E2C94A0F278BB9389D42D`). The UI tree remains
Unity SurfaceView-only; the app log has no configured fatal/ANR/SIGSEGV/SIGABRT marker but
contains known Lava gralloc/AHardwareBuffer format-allocation noise.

This closes the captured completion-card obstruction and records the exact candidate's
Results/rematch route, but it does not claim action-by-action tutorial comfort, repeated
rematch comfort, final authored/cultural/fun/accessibility approval, normalized performance
or endurance, physical 16 KB runtime, final identity/signing, privacy/Data Safety, rating or
Play Console approval. The two prompt files under `PROMPTS/` remain intentional uncommitted
owner work.

## Milestone 11 P59 exact-candidate all-fighter and accessibility route - 2026-08-31

The unchanged P58 candidate was exercised further on approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34, reported 4 KB pages). Fresh evidence
covers all three fighter cards and current live openings for Bijli, Maya and Pehel, attack
and ability-input checkpoints, Tiffin Station feedback, in-match Settings & Accessibility,
left-handed controls, reduced flashes, high contrast, aim assist, text scaling, persistence
and reset-to-defaults. The connected Oppo was excluded.

The exact route index is
`Builds/Local/Device/final-circle-20260830/p58-fighter-accessibility-route-manifest.json`
(7,285 bytes; SHA-256
`F9D43C679971029EC9CC8881913A0BF62A28555A2F7C14C7A1FB94554C7D2409`). The UI tree is
SurfaceView-only. The 15,630-byte app-scoped logcat has no configured fatal/ANR/SIGSEGV/
SIGABRT marker; known Lava gralloc/AHardwareBuffer, Unity Play Core class-probe and Swappy
diagnostics remain recorded as non-fatal platform noise.

This closes a current-candidate observation gap, not the human action-by-action tutorial,
repeated-rematch, accessibility comfort, fun/fairness, final authored art/audio, normalized
performance, battery/thermal, physical 16 KB, final identity/signing, privacy/Data Safety,
content rating or Play Console gates. The truthful state remains **Prototype — Android offline
release candidate in progress**, and the two prompt files remain intentional uncommitted owner
work.
