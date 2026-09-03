# BattleRaja V1 offline Android validation — 2026-09-02

This superseding record covers the current working tree after the Bastion Crown
authority, replay, squad-coordination and fair-pacing pass. It is a technical local
release-candidate record, not Play submission approval or a claim of final art,
accessibility comfort, cultural fit, signing or legal approval.

## Scope and source

- Branch: `codex/v1-playstore-release`; the working tree is intentionally being
  prepared for a fast-forward update to `origin/main`.
- Product scope: offline Android Bastion Crown only; one human + three allied AI
  versus four rival AI. Photon, PlayFab, accounts, matchmaking, ads, IAP, cloud
  progression, online leaderboards and Web release work remain excluded.
- Unity `6000.5.6f1`, URP, Android target SDK 36, ARM64 IL2CPP candidate.
- Approved device: Lava `ST5GDW23LB004392` / `LAVA_LXX508`, Android 14/API 34.
  `getconf PAGE_SIZE` returned `4096`; this is not physical 16 KB runtime proof.
- Current entry art and production fighter prefabs are repository-owned and editable.
  They remain a generated V1 presentation baseline, not commissioned final art.

## Production Bastion player HUD consolidation — 2026-09-04

The live 4v4 route now uses a single production `PlayerStatusCard` for the human fighter.
`BastionPlayerHud.cs` groups fighter identity/role, health, attack and ability readiness,
and gadget feedback in the portrait-safe area. Bastion suppresses the legacy Solo-only
`BijliHud` and `GadgetHud` objects while leaving them available to MovementLab fixtures.
The focused PlayMode regression asserts the card exists, contains the player-facing
Bijli identity and does not stack the legacy cards.

Fresh current-source gates are EditMode **159/159** (XML SHA-256
`04CE454016166816A88BE34E53BCAEFFDCA929786D0161F9DBCC3EC3E4527DD3`) and PlayMode
**95/95** (XML SHA-256
`CA5E0AC3FAC1E623612DC19874DA91429772B048A3D0B89E5DEF2E5B0B3B2054`). Static
validation is **0 errors / 0 warnings**.

The exact current APK/AAB pair is:

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 41,608,148 | `3CA0777D8B94E5381D08794BCA48BDCDE070675F4B54CB32EEFF31FE004C07F2` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 37,433,660 | `1996369E050A8C77BE6098618BC20ACB6388CD11C2A2A54A44249780BDFE6E95` |

The matching Unity build log is `Builds/M11/Logs/android-build.log` (SHA-256
`C248A5CE489C78AC4552A68249ABFAE02794E1F73EB3CA9B95C7ADC03409F772`). The APK was
installed on approved Lava `ST5GDW23LB004392`; its on-device base APK SHA-256 matched
the local APK. Route captures are under
`Builds/Local/V1GameplayTruth/Next/lava-player-card-20260904/`: the live capture
`05-live-only.png` is SHA-256
`BF4BF640E4FFADE883B03681F618719C87E43AB35F84F89A11909A11B5964454`, and the results
capture after the bounded performance window (`04-live-after-perf.png`) is SHA-256
`2AFBBAAF462D6DB39147028B7CDEE65FCCD1A57A8AC1E0831CA442D2BA32E992`. The route
visibly shows the single player card in live 4v4, hides it on results, and starts a
fresh live route again; the accessibility tree exposes only `unitySurfaceView`.

The six-sample/30-second live capture is under
`performance-player-card-20260904/` (manifest SHA-256
`6A4B00F6CBD8CBF2C68A10770BFE7BC7D488127A68A6D439E2E69E0B57321BFF`): PSS
**296,836–301,395 KB**, RSS **407,992–421,284 KB**, graphics PSS **89,244–93,348 KB**,
raw app CPU **94.2–103%**, CPU/GPU **41.03–41.657 C**, battery **81% → 81%**, thermal
status **0**, and zero configured fatal markers. The live-only SurfaceFlinger record
`surfaceflinger-latency-live-player-card-10s.txt` (SHA-256
`81316B42C3F642188C7ACBB36C086109D7EDA5079BB474EFCBDD7DAB0B2B10D8`) has 126 presented
frames / 125 intervals at 16.667 ms: mean **16.934 ms** (~**59.05 FPS**), p50 **16.534 ms**,
p95 **16.585 ms**, p99 **33.097 ms**, max **33.350 ms**, one interval over 33.33 ms and
none over 50 ms. This is bounded compositor evidence, not normalized Unity GPU/GC,
unplugged endurance, physical 16 KB runtime or final-art approval.

The clean technical release-checker log is
`Next/release-checker-player-card-20260904.log` (SHA-256
`80AE8FF493B75D46083859544692A92D91715E00E32C9EBAD0B74F4D17B0A3B2`). It passed
repository validation, target SDK 36, offline permissions, ARM64-only native libraries,
static 16 KB ELF alignment and store-creative dimensions. The artifact remains temporary
debug-signed and final identity, privacy, content rating, cultural and Play approvals are
owner-controlled.

The release candidate remains a temporary debug-signed package and the final package
identity, signing, privacy/Data Safety, IARC/content rating, cultural review and Play
submission are owner-controlled.

## Player KO → spectator → respawn proof — approved Lava — 2026-09-04

The exact APK above was exercised on approved Lava `ST5GDW23LB004392` in the Bastion
route. The player was moved beyond the shrinking Aandhi boundary until the production
card reached **5/85** (`Builds/Local/V1GameplayTruth/Next/lava-spectator-attempt-20260904/ko-watch-05.png`,
SHA-256 `3997C4E3950FAFCB520F9310456670E680B0972821DABBD09581812D938D86AD`), then
**0/85** with the visible `OUT OF ACTION • respawn or spectate an ally` state
(`ko-watch-06.png`, SHA-256
`FB543967E71126D7A9BF84063EC97A998EEC8E667DAC5044B6D0CB913DD0675C`). A second
held frame confirms the spectator presentation (`ko-watch-07.png`, SHA-256
`D8191EAAE9751B7199ECB5626CBAC029BB6FA451C07FE344B635DA3ABCCFE5BE`), and the same
route returns the player at **81/85** after Raja tickets change from 11 to 10
(`ko-watch-09.png`, SHA-256
`F1F7C5D7682CAEE1DE0F4004374095392855AA632D08A0ADE460046387B0B8B3`). This is
direct physical evidence of the current-candidate player KO → spectator → respawn
loop. The run ended with `airplane_mode_on=0`, so it supplements the separately
recorded airplane-mode offline launch evidence; it does not claim a new airplane-mode
route. The Unity accessibility tree still exposes only `unitySurfaceView`, making this
rendered-state evidence rather than an accessibility approval.

## Presentation grounding pass — 2026-09-04

The portrait readability follow-up now also grounds the camera outside the playable
plaza. `ProductionEnvironmentBuilder.cs` adds a collider-free, unlit woven Bazaar
backplate below the authored `GroundMosaic`; `BackdropBox.asset` carries tiled UVs so
the extra portrait pixels retain the same surface language without a full-screen lit
shadow pass. The generated `Backdrop.mat` and
`Assets/BattleRaja/Content/Prefabs/Production/BazaarBastionProduction.prefab` are
editable release inputs. Authority, replay, AI, economy and touch-input code were
unchanged.

The controlled rebuild log is
`Builds/Local/V1GameplayTruth/Next/environment-rebuild-backdrop-unlit-20260904.log`
(SHA-256 `1E870405C02EA5768E0A44853B029D4A3DABED94FFFB9770ABB17DFF12C1B954`). The
current exact non-development APK/AAB pair was rebuilt afterward:

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 41,598,480 | `9E5BFF2F28FC857D6E65E11A158942565E59A64AF68DA7F653C1F511060901B8` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 37,424,015 | `91AE29A0165589FC9A5065A7B1579F991DEB54557689F62B94F227C75F1D98EA` |

`Builds/M11/Logs/android-build.log` has SHA-256
`A0C952E2DC0EFA60FCC9D0D3009CF5647296D77AA212F9F9E3D8E3A75B896518`. The APK was
installed on approved Lava `ST5GDW23LB004392`; its on-device base APK hash matched
`9e5bff2f28fc857d6e65e11a158942565e59a64af68da7f653c1f511060901b8`. The exact
current route captures are `28-final-menu.png` (SHA-256
`9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`),
`29-final-briefing.png` (`640C44BD938E1ED44B9D203804127AB6115B93F9B705C4214C275D91591A6435`),
`30-final-fighter-select.png` (`70AE4024AE16FECAED36F9E321705035B53D358932463656F6CF0AFB0114EE79`),
`31-final-live.png` (`F710402425871CEEA42A73E5630898B8B8AFEE71F91D2F97326344189A5645B2`),
`26-backdrop-unlit-near-results.png` (`C6801D90B377940618516392BE271B855DFDEBE5AB330DCAF8066EABCAD997FE`) and
`27-backdrop-unlit-rematch.png` (`504F51184CE7A223FA8D9F54817AA8ACA0BB023880F736FE3EDBD75AC2261DA9`).
The route reached the live 4v4 Bastion Crown HUD, a 04:02 authoritative results card
(Rival winner, Raja 1 deposit, Rival 4 deposits), and a fresh 00:05 rematch with
scores/tickets reset and Raja carrying.

The final current-source automated rerun is **159/159 EditMode** (`Next/editmode-
backdrop-unlit-20260904.xml`, SHA-256
`FA1A2211ED4730DEF9B28CBCCECE47D8567353A69E232EA9E97E196A0113D158`) and **94/94
PlayMode** (`Next/playmode-backdrop-unlit-20260904.xml`, SHA-256
`3AEB88102BB7C37BAAD6761063EB43C5C76CC910A078B093B0535DF6E2FE581B`). Their logs are
`1400FD6D534897B0A80F000FA13393F487F00A9AAE8926EFE05C99E8CD737FB7` and
`50C4D22557F1A1BA3CEEEB1B26263958BBA995E16469609C59A4F750AAE55197`. The two-seed
× 8,400-tick replay/deterministic soak completed with zero divergence in this exact
source rerun. Static validation remained 0 errors / 0 warnings.

For the exact final APK, the bounded 30-second Lava capture is under
`Next/lava-camera-art-20260904/performance-backdrop-unlit-20260904/` (manifest SHA-256
`EF523B47ECFCE00B5517BD96972C192E83AC32E19B921D8CA589A8358B8F5E25`). It contains six
samples with PSS **295,058–306,074 KB**, RSS **405,692–425,404 KB**, graphics PSS
**87,060–97,320 KB**, raw app CPU **100–127%** on Android's per-core scale, CPU/GPU
temperature **57.938 C**, battery temperature **33 C**, battery **87% → 87%**, thermal
status **0**, PSS change **2.40%**, and no configured fatal markers. This is bounded
device evidence, not unplugged endurance or a normalized GPU/GC budget.

The matching compositor history is
`surfaceflinger-latency-backdrop-unlit-10s.txt` (SHA-256
`F96E961687E92F139B6096805A6E93B54A16DA53D0C6EDCF1B33B58BF6661C63`). After removing
the tool's invalid/sentinel intervals, it contains 127 presented frames / 125
intervals at a 16.667 ms display period: mean **16.670 ms** (estimated **59.99 FPS**),
p50 **16.535 ms**, p95 **16.565 ms**, p99 **16.590 ms**, max **33.355 ms**, one
interval over 33.33 ms and none over 50 ms. This is compositor-side evidence only;
Unity GPU timing, GC slices and ten-rematch endurance remain unmeasured.

## Canonical Bastion telemetry follow-up — 2026-09-02

The development-only production harness now emits schema-v2 JSON from the canonical
`BastionCrownMatch` state. The projection includes team score/deposits/KOs/assists,
Crown pickups and first-event timing, objective seconds, ticket pools, damage/healing,
gadget/ability use, winner/reason/overtime, socket rotations, respawn and team-wipe
transitions, peak spectators, squad-blackboard counters and measured alive-ally spacing.
Each participant also carries the authority team, role, lifecycle and objective counters.
The projection is report-only and does not mutate simulation state.

The current strict run is `Next/production-bot-100-telemetry.xml` (**94/94 passed**, SHA-256
`A51EADF828A27B5E2D5BA90DEDEC8A215E76713850965BF02223D9412F3D9A59`) with log SHA-256
`EA805C25A84DD1E4BECFDF356A99435DC2208EBC81BE366E3AF1531C189A58CC`. Its schema-v2
batch report is `Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260901-224046635-9101.json`
(SHA-256 `55049198B0E93D5A037055CEA4A3687F54F083FE39DFF6D5778410FBC9D48DEA`):

- 100/100 matches completed; duration **136.867–273.022 s**, average **239.132 s**;
  **92/100** landed in the 240–360 s window, **91/100** were combat-positive and
  **61/100** reached at least three combat eliminations. Four were Aandhi-only.
- Bot-to-bot damaging pairs were present in **100/100**; protected-warmup damage and
  invalid-position samples were both **0**. Aggregate team score was **524/472** from
  **123/88** Crown deposits and **155/208** KOs. Crown pickups were **152/200**.
- The authority spent **376** shared tickets and produced **376** respawns; peak
  simultaneous spectators was **2**. No team-wipe transition occurred in this sample.
  Seven matches entered overtime and were classified as clock/overtime stalemates;
  **84/100** recorded a first deposit. Crown socket rotations totaled **238**.
- Canonical objective time was **2,858.7/18,806.2 s** (Raja/Rival); healing was
  **26,547/22,159**; gadget uses **153/101**; ability uses **5,446/8,341**.
  Squad signals totaled **179,431**, support assignments **296,654**, escort handoffs
  **149**, retreat signals **73,960**, and maximum signal age was **4 ticks**.
  Alive-ally spacing contributed **8,436,824** samples (aggregate per-match mean
  **4.60 m**, observed range **0–25.30 m**).
- Maximum bot decision time was **0.371 ms**; maximum outside participants remained **7**
  with **135,612** outside-participant ticks. These are retained tuning signals, not
  hidden or converted into pass/fail claims.

The first serialized replay from this run is
`Builds/Local/V1GameplayTruth/ProductionBotReports/Replays/match-9101-20260901-224048351.brr`
with SHA-256 `D0BE5FD5A31F8D256B022C6FCE2176975C46C369C6308872DC007646E0808EDE`.

## Superseded exact artifact rebuild after telemetry — 2026-09-02

The non-development APK and release-shaped AAB were rebuilt after the telemetry source
change from the same Unity `6000.5.6f1` project. The composed technical checker passed
repository validation, offline manifest, target SDK, ARM64 payload, static 16 KB ELF
alignment and store-creative dimensions:

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 41,516,072 | `33AFA202C521632B4662764340574471F982DF189BC1C5D5F724757BA8680B6E` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 37,341,598 | `3EAE3460F8106AFDD8CD46B10E8DC20F373D66F0BD8731E38F3B0B2CCA48DDF2` |

Build log: `Builds/M11/Logs/android-build.log` (SHA-256
`4181E16C71D4EBD5CBBE168AE99CB8D358DE2E96768500A96744511DE0CB5497`). The checker
reported package `com.example.battleraja.m11`, version `1.0.0`/100, min SDK 28, target
SDK 36, only VIBRATE plus the dynamic receiver permission, seven ARM64 native libraries,
no other native ABIs and static `0x4000` LOAD alignment. This remains a debug-signed
temporary identity; it is not a publishable artifact.

## Superseded exact artifact rebuild after camera-facing identity fix — 2026-09-02

The generated fighter prefabs were rebuilt after moving the Bijli, Pehel and Maya
identity accents onto the gameplay camera-facing side (including the Maya mask). The
non-development APK and release-shaped AAB were then rebuilt from the same Unity
`6000.5.6f1` project. The composed technical checker again passed repository
validation, offline manifest, target SDK, ARM64 payload, static 16 KB ELF alignment
and store-creative dimensions:

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 41,516,080 | `E81A035BE7AAF50D5ED1A994C60B68A2765B92CBDC2228528957713BB62702A0` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 37,341,603 | `DFD0C4516BBC44907E30F16BAAC4D0C373BE81AEC6B3C43DF4FF3C3510972276` |

The final build log is `Builds/M11/Logs/android-build.log` (SHA-256
`2C818B39FB2D8CDF7603D7752A4AC57CECA8738233B2259DC2C301A2D8292341`). The installed
Lava package hash matched the APK exactly
(`e81a035be7aaf50d5ed1a994c60b68a2765b92cbdc2228528957713bb62702a0`); package
`com.example.battleraja.m11` remained version `1.0.0`/100, min SDK 28, target SDK 36.
The candidate is still debug-signed with a temporary package identity and is not a
publishable artifact.

The post-regeneration gates are **159/159 EditMode** (`Next/editmode-final-visualfix.xml`,
SHA-256 `17483B1D96DC466517352B22F574A306CBB2466C738D90938244F5D2328F9B3B`) and
**94/94 PlayMode** (`Next/playmode-final-visualfix.xml`, SHA-256
`B839085E347BE24062787235D5E9EF2DAA47E0705E414FE5C9672B6D6851339E`). The controlled
prefab regeneration log is `Next/rebuild-presentation-camera-facing-3.log` (SHA-256
`7A02732C731A1527BC2E38A22109C8534E2348C76FAA8B3EC5F74E8C4EDA7440`).

## Exact artifact rebuild after final source hygiene — 2026-09-02

The generated prefab YAML was normalized to remove two non-semantic trailing-space
records, then the non-development APK and release-shaped AAB were rebuilt once more from
the checked-in source. This is the current artifact pair; the composed technical checker
again passed repository validation, offline manifest, target SDK, ARM64 payload, static
16 KB ELF alignment and store-creative dimensions:

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 41,516,076 | `6050E1A6EC329F27BC14A1118FB166D278293237B4BC6CBA716B7B700D9FD6FF` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 37,341,603 | `A2F440649987A8FA04398B629F956AC44267AA1D33FEF571C3264B97051CCB4C` |

The final build log is `Builds/M11/Logs/android-build.log` (SHA-256
`A4921F6CCBE1111DB954BB7587987E7515AAD717B9EFF7E4426845A1C3B728AD`). The installed
Lava package hash matched the APK exactly; package `com.example.battleraja.m11` remained
version `1.0.0`/100, min SDK 28, target SDK 36. The candidate is still debug-signed
with a temporary package identity and is not a publishable artifact.

## Tutorial authority and safety follow-up — 2026-09-02

The tutorial scene now exercises the same authoritative movement, damage, collection and
replay path as production. `tutorialMode` is enabled only on `TutorialArena`: it keeps the
Aandhi warning/zone preview visible while delaying outside-zone damage, and widens the
tutorial gadget pickup radius to 3 m. Production Solo/Bastion scenes remain unchanged.
The layout is still the legacy MovementLab/Solo training arena with bots disabled; this is
an onboarding hardening pass, not a claim that it is a dedicated 4v4 Bastion tutorial.

The post-change gates are **159/159 EditMode** (`Next/editmode-tutorial-authority.xml`),
**94/94 PlayMode** (`Next/playmode-tutorial-authority.xml`) and static validation **0/0**.
The rebuilt technical candidate passed the same checker:

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 41,520,532 | `56F3BAB99E304A15548D8073BA6B41EDDCBDE17A2C7476D923B06094D5A9649E` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 37,346,030 | `19E2E7CCFFD7B2CBA993DE3608D8D62F4A351425AA76D0085138C1DF6DD96BCA` |

Approved-Lava evidence from that exact APK is under
`Builds/Local/V1GameplayTruth/Next/lava-tutorial-20260902/`. The route reached the real
Movement, Aim, Basic Attack, Ability, Gadget (Tiffin use), Aandhi, Elimination and Victory
cards; `73-elimination-authority.png` shows the target defeated and `74-victory-authority.png`
shows the final lesson waiting on results. Selected hashes are `67-tutorial-authority.png`
`0BE45D7DAEA6B9F1BF61A0BB8A1C3A7AC7360B570E6184BBFFEE29E33478A546`,
`70-gadget-used-authority.png`
`69A2D5B00E40BAEE125C6EC76DE7FD569D493E2392F9586A322ABDF45365FB72`,
`73-elimination-authority.png`
`5A260BDDE2D899E40FF355AC23BC858EA570EF994FAD17A0FACFDAE8FF6068DB` and
`74-victory-authority.png`
`41451CD9DA388468067C3BBC97777C26944094FC6DBB1685359090E6057F8CB0`.

The top-resumed activity remained `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`;
the sampled logcat window contained no configured fatal markers. This route does not prove
the full Bastion Crown tutorial layout, final results/rematch comfort, normalized
performance, physical 16 KB runtime compatibility or Play readiness.

## Automated gates

| Gate | Evidence | Result |
| --- | --- | --- |
| Static repository validation | `Tools/Validation/validate.ps1` | **0 errors / 0 warnings** |
| EditMode | `Builds/Local/V1GameplayTruth/Next/editmode-final-visualfix.xml` | **159/159 passed** |
| PlayMode | `Builds/Local/V1GameplayTruth/Next/playmode-final-visualfix.xml` | **94/94 passed** |
| Bastion replay soak | two seeds × 8,400 ticks; v2 serialized replay re-execution | **0 combined-hash divergences** |
| Squad planner coverage | 32 deterministic seeds | contest **64**, escort **64**, defend **96**, collapse **64**, Aandhi-retreat **32** |
| Production bot gate | `Next/production-bot-100-tutorial-authority-rerun.xml`; strict release assertions | **94/94 tests passed** |
| Diff hygiene | `git diff --check` | Passed |

The authority now exposes a bounded-lag squad blackboard. It snapshots all eight
participants at a fixed communication cadence, selects deterministic support/peel
handoffs, and freezes the command phase so same-tick authority mutations cannot
change the decision source. Team-mode final placement is reassigned deterministically
after respawns, so terminal results contain unique placements.

## Strict 100-match production-bot evidence

Report: `Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260901-220113865-9101.json`
(SHA-256 `D44275C62CDF18ADDD9581020088FAB39685279E0AD896CE8F799C20DA867E73`).

- 100/100 completed; duration **109.765–273.022 s**, average **237.891 s**.
- **91/100** landed in the 240–360 s target window; **92/100** were combat-positive;
  **64/100** had at least three combat eliminations; **3** were Aandhi-only. The report
  exposes the pacing window rather than treating terminal completion as sufficient.
- **100/100** contained a bot-to-bot damaging pair; protected-warmup damage **0**;
  invalid-position samples **0**.
- Attacks **17,714/17,714** accepted; projectile hits **8,293**; abilities
  **12,281/15,110** accepted; effective abilities **560**.
- Gadgets **248/293** successful attempts, with all three gadget kinds exercised;
  failed gadget uses **45**.
- Maximum decision time **0.265 ms**; maximum outside participants **7** and
  outside-participant ticks **103,557** are retained as tuning evidence, not hidden.
- Production bot weapon damage is equal to the human definition (`1.0x`), and the
  shared production/validation cadence is bounded at `15x`; neither is a combat-power
  advantage over a human.

The first serialized replay contains 7,290 frames and has SHA-256
`88E2A0A4147A7E36A543802AA12D33EA4E9EF574CE371A6B9FBC0D6B56998E4B`.

An immediately preceding fresh-process run completed all 100 matches but failed the
combat-positive assertion at 89/100 (`Next/production-bot-100-tutorial-authority.xml`).
No source changed between runs; the no-code-change rerun above passed at 92/100. This
variance is kept as an open balance/pacing risk and is not hidden by the selected pass.

## Android artifacts and technical checker

This section preserves the immediately preceding tutorial-authority artifact for
provenance. The current post-telemetry APK/AAB and checker are recorded in the exact
artifact rebuild section above.

The direct Unity release entry points completed successfully. The release checker
passed package `com.example.battleraja.m11`, version `1.0.0`/100, min SDK 28, target
SDK 36, no network permissions, ARM64-only native libraries, static 16 KB ELF/zip
alignment, and the 512×512 icon / 1024×500 feature-graphic dimensions.

| Artifact | Bytes | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 41,514,464 | `7243A7A324E43FC2C2A274DDF1B27C89166E5E9CF5F39C981D650355F696E9B6` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 37,339,995 | `ABC16E0F7B499690BA41ECC9CBAB5D243C35E85783B6051E5E1982B51ACE8D48` |

Build logs are `Builds/M11/Logs/android-build-apk-final.log` and
`Builds/M11/Logs/android-build-aab-final-2.log`. The package is temporary/debug
identity and is not signed for publication. Unity emitted only known unused-prefab
Photon/Fusion script-reference warnings; the repository validator and release checker
are the authoritative zero-warning gates.

## Approved Lava route

The exact APK was freshly installed with ADB on `ST5GDW23LB004392`; the top-resumed
activity was `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`.
Evidence is under `Builds/Local/V1GameplayTruth/Final/lava-20260901-balanced/`:

- menu and original Bazaar Bastion entry art (`01-launch.png`);
- `PLAY OFFLINE` → `BASTION CROWN • 4V4` briefing → Bijli/Pehel/Maya fighter choice;
- live eight-actor arena with Crown, tickets, ability, gadget, attack and touch HUD;
- gadget, ability and attack taps; settings/accessibility controls including left-handed,
  reduced flashes, high contrast, aim assist and text size;
- resumed match, progress snapshots through Aandhi, results, rematch reset and airplane
  mode enabled/disabled with final global airplane state `0`.

Selected evidence hashes: `01-launch.png`
`9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`,
`04-live-opening.png`
`E2E2870EB31013D81FC00A0649A2A77A3A179109898FC2F4D03BCB6451B68121`,
`13-progress-04.png`
`59CFF86B73646A89E78942817399906FBB6935B9A3556AED522CDA59F434DC25`, and
`14-rematch.png`
`46C0966FF55FFFF0DDE228857E0E8273D86CAC73F690264E14E04BE005A74BB6`.

The exact final APK route did not complete the full action-by-action tutorial. The
tutorial exists and is covered by automated tests, but a complete physical tutorial,
all-fighter comfort and long-session human-fun review remain open.

## Physical Bastion route refresh — approved Lava — 2026-09-02

The current tutorial-authority APK was reinstalled on the approved Lava and exercised
through a fresh Bastion route. Evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-route-20260902/`:

- cold launch/menu (`00-launch.png`), Bastion briefing (`01-briefing.png`), fighter
  choice (`02-fighter-select.png`), ready/live opening (`03-ready.png`,
  `04-live-start.png`) and Crown approach/movement (`05-move-toward-crown.png`);
- attack/intercept attempts and combat contact (`06-intercept-attempt.png`,
  `07-attack-right.png`, `13-attack-hold.png`, `14-combat-contact.png`,
  `15-attack-cluster.png`, `16-post-combat.png`);
- Aandhi pressure (`09-live-aandhi.png`, SHA-256
  `89D7C832E75D84C1E6FB561CE407F60C51B69D743C5343A06F955F062529BC37`), a terminal
  results surface (`10-terminal-or-results.png`, SHA-256
  `1F4306377F7645CBE9E1E367C7E8317738B28907E191B348B688EADE087A5722`), and a fresh
  rematch with scores/tickets reset (`11-rematch-opening.png`, SHA-256
  `8E3483424022E8D612309B5DB96E8D677DAB401FD08B9DF62576C94FABEDA418`);
- a second-match terminal outside-zone/results capture (`18-outside-zone.png`,
  SHA-256 `5A323EB25723C5843D17CA37464F83C75F4828B2DAD1FB021E30F1DCE5CF497F`).

The Unity view exposes only a `unitySurfaceView` node in the Android accessibility tree,
so the Unity controls were exercised at known rendered coordinates; this is recorded as
a tooling limitation, not accessibility approval. The sampled results showed combat and
ticket state, but both physical matches ended with **0 Crown deposits** and no explicit
player spectator transition was captured. Crown deposit, respawn/spectator comfort,
complete all-fighter interaction and human fun review therefore remain open despite the
automated authority coverage.

## Physical route on exact post-telemetry artifact — approved Lava — 2026-09-02

After the artifact rebuild, the exact APK with SHA-256
`33AFA202C521632B4662764340574471F982DF189BC1C5D5F724757BA8680B6E` was installed and
replayed from a cleared package. The append-only evidence directory is
`Builds/Local/V1GameplayTruth/Next/lava-exact-20260902/`. It reaches menu → Bastion
briefing → Bijli/Pehel/Maya choice → live eight-actor arena, then shows a live Aandhi /
Rival-carrier state (`04-live-aandhi-window.png`, SHA-256
`5F8A6E08AF446F112A9B5C66D33345C6650E24E021A4C25FDCCC1386E742A943`), the authoritative
results card (`05-results.png`, SHA-256
`9C1D85EF4FCC5F7F21E79B86B5E9806C98330315862BA8E68AD32511B6120D59`) and a fresh rematch
with scores/tickets reset and the Crown on a new socket (`06-rematch-opening.png`, SHA-256
`4813881543D3EBABE0CB8263BE6131938B6C5A12CA44AA405F428237798E97B3`). The rematch live
capture (`07-rematch-live.png`, SHA-256
`D7CF3A843B70FAEFCDCAB8F82CDFD0686737A359835A35D262B9555CC8227083`) confirms the new
match is active. The focused activity remained
`com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`.

This exact-artifact route still did not physically observe a Crown deposit or explicit
spectator transition; those steps remain open for human/device review. The Android
accessibility dump again exposed only Unity's `unitySurfaceView`, so rendered Unity
controls were exercised at known coordinates and this limitation is not presented as an
accessibility pass.

## Superseded physical route on the visual-fix artifact — approved Lava — 2026-09-02

The final APK (`E81A035BE7AAF50D5ED1A994C60B68A2765B92CBDC2228528957713BB62702A0`)
was freshly installed after the Maya camera-facing mask correction and replayed from a
cleared package. Append-only evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-visualfix-final-20260902/`:

- `00-launch.png` (menu) SHA-256
  `9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`;
- `01-briefing.png` (Bastion Crown briefing) SHA-256
  `640C44BD938E1ED44B9D203804127AB6115B93F9B705C4214C275D91591A6435`;
- `02-fighter-select.png` (Bijli/Pehel/Maya choice) SHA-256
  `709916D26E97D1DC46537AAC260CFFDA7B0C0A40A1D3E00D3855647C609C0238`;
- `03-ready.png` (fresh live 4v4) SHA-256
  `05406442A202D3C4D8359B512EE9A7475BBFF838BFF843B0D7AB399F1E9368C8`;
- `04-live-30s.png` (combat/objective HUD) SHA-256
  `72A8E1AAB9A355C62B23D7A24FB3A0A65A7819E9AD9CBAEC7264BFD47198619B`;
- `07-live-~4m.png` shows the authoritative results card (Rival winner by clock,
  Raja 3 deposits 0 / Rival 5 deposits 1, tickets 10/12) with SHA-256
  `71BFB6DF4C0479AB5D2DDA3AE1C085103985152B6A83FE2CAAA1AD9CF9A735F8`;
- `08-rematch-live.png` shows a reset `00:05` match with scores/tickets reset and
  the Crown carrier state, SHA-256
  `7165821EEDA250FDC3358957B2DC0F59097071648B99C49894796B80798446A9`.

The installed package hash was verified on-device against the local APK. Lava remained
`1080x2460`, Android 14/API 34, and `getconf PAGE_SIZE` returned `4096`. The focused
activity remained
`com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`; the captured
logcat window contained no configured fatal markers. The Unity accessibility tree still
exposes only `unitySurfaceView`, so rendered controls were exercised at known
coordinates. This exact route still did not physically observe a Crown deposit or an
explicit player spectator transition; those, long-session comfort and human-fun review
remain open.

## Physical route on the current final artifact — approved Lava — 2026-09-02

The current APK (`6050E1A6EC329F27BC14A1118FB166D278293237B4BC6CBA716B7B700D9FD6FF`)
was freshly installed after final source hygiene and replayed from a cleared package.
Append-only evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-release-final-20260902/`:

- `00-launch.png` (menu) SHA-256
  `9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`;
- `01-briefing.png` (Bastion Crown briefing) SHA-256
  `640C44BD938E1ED44B9D203804127AB6115B93F9B705C4214C275D91591A6435`;
- `02-fighter-select.png` (Bijli/Pehel/Maya choice) SHA-256
  `28A08C5A159B1639276174A6253B6B5BE0DB6B1686281FA9F2461389C2E4A24B`;
- `03-ready.png` (fresh live 4v4) SHA-256
  `E5470E04E71B5689A7E7BA31DA60189A870164A9EA43D5EA1A2C88FA5D576B48`;
- `04-live-30s.png` (combat/objective HUD) SHA-256
  `ADCADA4588DCEA762AF54AC02C201413629A05EE5DEE7E8C5FCBE0F6CBB1E9CC`;
- `05-live-3m.png` reached the authoritative results card at `04:02` (Raja 9/15,
  Rival 1/15, Raja winner by clock, deposits 3/0, tickets 11/9) with SHA-256
  `45AC9FAA869E28073476F684728B4448153D629F67340ADF2F2B5EB0382B0D74`;
- `06-rematch-live.png` shows a reset `00:05` match with scores/tickets reset and
  the new Crown carrier state, SHA-256
  `0F720BB0BFA8443A12CF9B8B76290B0FBD415923E6938D88C44DBF236BCBACEF`.

The installed package hash was verified on-device against the local APK. Lava remained
`1080x2460`, Android 14/API 34, and `getconf PAGE_SIZE` returned `4096`. The focused
activity remained
`com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`; the captured
logcat window contained no configured fatal markers. The Unity accessibility tree still
exposes only `unitySurfaceView`, so rendered controls were exercised at known
coordinates. This current route still did not physically observe a Crown deposit or an
explicit player spectator transition; those, long-session comfort and human-fun review
remain open.

## Presentation identity pass — 2026-09-04

This focused continuation addresses the mobile readability review without changing
the Bastion authority, replay, AI or economy rules. `ProductionArtBuilder` now emits
restrained woven/banded material detail instead of a high-contrast checker pattern,
and the Bijli, Pehel and Maya recipes add camera-facing eyes, jaw guards and role
silhouette accents. `ProductionPresentationBuilder` reparents those identity parts
into the production rig so animation and facing stay consistent. The pass regenerated
the 14 repository-owned texture assets (`BijliCyan`, `BijliGold`, `Crystal`,
`GadgetDhol`, `GadgetHighlight`, `GadgetInk`, `GadgetTiffin`, `GadgetUmbrella`,
`Ink`, `MayaMint`, `MayaRose`, `MayaViolet`, `PehelClay`, `PehelCream`) and the three
production fighter prefabs. These remain editable generated assets; they are not a
commissioned final art pack.

- Art rebuild log: `Builds/Local/V1GameplayTruth/Next/art-rebuild-20260904.log`,
  SHA-256 `BB6AFB79D8658CF2333DAA5AEAF94EC94AD6529B1CEFF0837FA21B49A8485699`.
- Fresh EditMode: **159/159 passed**, XML SHA-256
  `15D55D73C1C70CAFA9146ABABD36778937F3F80C06084845069655E2E3F0C4EF`.
- Fresh PlayMode: **94/94 passed**, XML SHA-256
  `F03C2ED4756A38B6FE3512E485AB8E1F3A609DBCDCC5259EEAB84A5BDFF1287C`.
- Fresh Android artifacts: APK **41,549,412 bytes**, SHA-256
  `E5F611282763C443B271F19C9EF63069AC3825E31EBD57DC3550187D3CC945EB`; AAB
  **37,374,943 bytes**, SHA-256
  `0F7C72459D66816E2E2EB2C20FD18FD15DB46018C45E78C52F65E1D3A65BE967`.
  The technical checker passed the temporary package, target/API, offline-permission,
  ARM64, static-alignment and store-dimension gates before the documentation commit.

Fresh approved-Lava evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-art-pass-20260904/`:

- `00-launch.png` (menu), SHA-256
  `9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`;
- `01-after-play.png` (Bastion Crown briefing), SHA-256
  `640C44BD938E1ED44B9D203804127AB6115B93F9B705C4214C275D91591A6435`;
- `02-fighter-select-fresh.png` (Bijli/Pehel/Maya choice), SHA-256
  `A5B6ABB0223EBCBCCEB1991BD647FB9560C881D17640AF9DB25905A9B4F8AC7B`;
- `03-ready-fresh.png` (live 4v4 at 00:01), SHA-256
  `671E7DD0204DC0B6F25A5BE16C1098C61B130E1EA1513F03F03BE01DAA540EE6`;
- `04-live-art-pass.png` (combat HUD and role silhouettes at 00:25), SHA-256
  `1EAFE12E2D742A60718A3B34E673367FCAF9ACACAB92203CF045A4728697F7F3`;
- `05-live-late.png` (Rival carrier at 01:04), SHA-256
  `07E49A21F2A59528FFFBFF1AE40C3BE6B7ED987984131003BF2C66D78A0BCA15`;
- `06-live-combat.png` (Raja carrier at 02:28), SHA-256
  `40531D2F4310A22BB078640529DA064D8703D553751AEC3BE500C14CE51B335D`;
- `07-live-endgame.png` (authoritative results at 04:02: Raja winner by clock,
  9/15 score, 2 deposits, 11 tickets; Rival 1/15, 0 deposits, 9 tickets), SHA-256
  `C138991B25D5466E930E35FCFA757215F890C4821D5F674C234EB3C919A25E79`;
- `08-rematch-live.png` (fresh 00:03 rematch, scores/tickets reset, socket 3),
  SHA-256 `781F227A1F791A2B8665EE392F7E78BB9ABAFA406181AF605C242B3A113CA249`.

The installed APK hash matched the local APK on `ST5GDW23LB004392`; the focused
activity remained `com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`.
The route physically observed a Crown deposit in the results card and a working
rematch. The accessibility dump still exposes only Unity's `unitySurfaceView`, so
rendered controls were exercised at known coordinates rather than presented as an
accessibility pass. No explicit player spectator transition was observed. A point-in-
time post-rematch diagnostic measured **294,934 KB PSS**, **405,100 KB RSS**,
**93,560 KB graphics PSS**, raw app CPU **2%**, thermal status **0**, current CPU/GPU
**41 C** and battery **32 C**; Unity `gfxinfo` exposed only the surface view and no
usable frame histogram. These values are not a normalized FPS, GC/GPU, endurance,
battery or physical-16-KB approval.

## Presentation readability refinement — 2026-09-04

This follow-up keeps the authority, replay, AI and economy source unchanged while
addressing the remaining mobile readability concern. `ProductionEnvironmentBuilder`
now generates restrained woven/banded environment detail in place of the earlier
high-contrast checker treatment. `TopDownCameraController` lowers the portrait
framing cap multiplier from **2.8** to **2.2**, keeping the 25.6-unit arena in view
while making fighters, Crown sockets and telegraphs materially larger on the 1080x2460
device. The regenerated source includes 12 environment materials, 16 environment
texture assets, `BazaarBastionProduction.prefab`,
`ProductionEnvironmentBuilder.cs` and `TopDownCameraController.cs`; all remain
editable repository-owned assets.

- Environment rebuild log: `Builds/Local/V1GameplayTruth/Next/environment-rebuild-20260904.log`,
  SHA-256 `30F4DE942E01C7368DE7BB6DACDEE8E4FFDA0FC901D4FFA2DE85DF28846345E7`.
- Fresh EditMode: **159/159 passed**, XML SHA-256
  `FF15744802560C6D2CFAB8E77BC11F2A3048F3F6D9C9116DC0D2FC70FF6FD4FF`.
- Fresh PlayMode: **94/94 passed**, XML SHA-256
  `4FA8EA81AB372436B6AB7796C31024262BF2016074FEFF88A812C87899F0A4AE`.
- Current-checkout rebaseline rerun: EditMode **159/159** in
  `Next/editmode-rebaseline-20260904.xml` (SHA-256
  `A04D0CC0C31459C241EEF3E3B63A479A43C5EC2B4E5C6BED78D439ECE3CBF9C2`) and PlayMode
  **94/94** in `Next/playmode-rebaseline-20260904.xml` (SHA-256
  `E95C6AED5922793791AD7B42064EDB80D782CEEC5AC12DC10A170966D77FB287`). The EditMode
  run included the two-seed × 8,400-tick Bastion replay soak and the deterministic seeded
  soak; both fixtures passed with zero replay divergence.
- Current Android artifacts: APK **41,579,392 bytes**, SHA-256
  `54AF0801C2FD696DAD3224E6AD1CDDB7F15D8386094CAB8AAD68F5DFABB950E7`; AAB
  **37,404,926 bytes**, SHA-256
  `CAE58A648792BB14E77767E9036C073256BA2C8C1CED86DCCD31A42BE656A2F0`.

Fresh approved-Lava evidence is under
`Builds/Local/V1GameplayTruth/Next/lava-camera-art-20260904/`:

- `00-launch.png` (menu), SHA-256
  `9457B5360CBD099504980784750C4FC21D4F24D84BEE86D66A1615D3F9839A82`;
- `01-briefing.png` (Bastion Crown briefing), SHA-256
  `640C44BD938E1ED44B9D203804127AB6115B93F9B705C4214C275D91591A6435`;
- `02-fighter-select.png` (Bijli/Pehel/Maya choice), SHA-256
  `902EC747D12ED936EE2398BBA426F5242DE655846A820A156623F16B95E4600F`;
- `03-live-tight-frame.png` (portrait live arena with the tighter camera), SHA-256
  `DA8C559E10DA9F07006115D97195B4CD1CC96F929FFC319DF0DF626B46B5620E`;
- `04-live-30s.png` (combat/objective HUD), SHA-256
  `656939DE6A0CF0CEAECA8AC041EF719EE9C3D588E71CAB76A9C9C7257D435FD0`;
- `05-action-state.png` (player action state), SHA-256
  `E4AE0C7EA110F234886091270D3182C6D6E05E24338FF43DE41FADB8CBFFCBA7`;
- `06-live-midmatch.png` (mid-match Crown contest), SHA-256
  `DEF3EF37D55BBAADF94645329D7E7B617980406525DA10405663BA36078EEBA1`;
- `07-live-late.png` (late-match Aandhi/objective state), SHA-256
  `E0002D84CE93B311A2CB90CFFE5166B45BE7B8BE8E2A8ED57E812ECDCE1D5994`;
- `08-live-final-window.png` (04:00 endgame window), SHA-256
  `8ED60C08350065D597BD4BA9640902A88DD1736356DECFDA9D1B1D36591118B1`;
- `09-results.png` (authoritative results at 04:02: Raja winner by clock, 4/15
  score and 4 Crown deposits; Rival 0/15), SHA-256
  `4CD1558E905B25B9AE22590D40B573610146F6E6222D09C9BD7AA4B9E3A7B5CE`;
- `10-rematch-live.png` (fresh 00:04 rematch, scores/tickets reset and Raja carrier),
  SHA-256 `1E7F87D51E6F143098C5FE8BDB4F3C063A2018B6DF356C586C7FBFE00B594334`.
- `13-spectator-transition.png` (spectator-control probe; the rendered view remained
  live, so no transition is claimed), SHA-256
  `70BD049A8572623D15CDE5D5AF540D2875DDF68A3D869E0B705B08B9C96B9BA9`.

The installed package hash matched the local APK on Lava `ST5GDW23LB004392`; the
device remained `1080x2460`, Android 14/API 34, with `getconf PAGE_SIZE` **4096**.
The focused activity remained
`com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity`, and the
captured logcat window contained no configured fatal markers. A post-rematch point
sample measured **298,676 KB PSS**, **409,568 KB RSS**, **91,292 KB graphics PSS** and
raw app CPU **2%**; thermal status was **0**, current CPU/GPU were **42.314 C** and
battery was **33 C**. Unity `gfxinfo` exposed only the surface view and no usable
frame histogram. The accessibility dump still exposes only `unitySurfaceView`, so
rendered controls were exercised at known coordinates. The route now explicitly
observes results and rematch on this artifact, but still does not observe an explicit
player spectator transition. These values are bounded diagnostics, not normalized
FPS, GC/GPU, battery, endurance, physical-16-KB runtime or commissioned-final-art
approval.

The final clean-tree release-checker output is retained at
`Builds/Local/V1GameplayTruth/Next/release-checker-camera-art-20260904.log`, SHA-256
`E8DA3A7106A0A64421DD709585B549852350088FA75199EF62F9DEBC8B83EF1F`. It passed
repository validation, target SDK 36, offline network-permission, ARM64-only native
libraries, static 16 KB ELF alignment and Play creative dimensions.

### Current Lava frame and memory sample

The approved Lava BLAST layer
`SurfaceView[com.example.battleraja.m11/com.unity3d.player.UnityPlayerGameActivity](BLAST)#2153`
was cleared, observed and dumped through SurfaceFlinger while the current live match was
running. The raw timestamp capture is
`Builds/Local/V1GameplayTruth/Next/lava-camera-art-20260904/surfaceflinger-latency-10s.txt`,
SHA-256 `F7D32C24480683590FFADD1736F6E796FBAA0F51EAE64AE1E3492B1F597FDCAD`. The device
reported a **16.667 ms** display period; the available history contained **127 presented
frames / 125 intervals**, with mean interval **16.670 ms** (estimated **59.99 FPS**),
p50 **16.534 ms**, p95 **16.569 ms**, p99 **16.585 ms**, and maximum **33.357 ms**.
There was one interval above 16.67 ms, one above 33.33 ms, and none above 50 ms. This
is a compositor-side history window rather than a full-session Unity frame profiler.

The concurrent six-sample/30-second raw capture is under
`Builds/Local/V1GameplayTruth/Next/lava-camera-art-20260904/performance-rebaseline-20260904/`;
its manifest SHA-256 is `CD0DB79CF18A00DE0503C4CEA61966961B18F74CE58663B4538146CF3CFB16D4`.
Across the six samples: PSS **301,862–306,949 KB**, RSS **421,588–426,676 KB**, graphics
PSS **87,212–93,368 KB**, raw app CPU **105–112%** on Android's per-core scale, current
CPU temperature **40.847–41.489 C**, battery **32 C**, thermal status **0**, and battery
level **87% → 87%**. The capture reported zero configured fatal markers. The roughly
**1.7%** PSS change is bounded to 30 seconds and does not prove the ten-rematch growth
gate. GPU utilization, managed GC allocation/spike data and a Unity frame-timing
histogram were not exposed by this device/runtime.

## Bounded performance and crash scan

`Tools/Validation/capture_android_performance.ps1` captured six samples over 30 seconds
from the exact final APK. Raw evidence is under
`Builds/Local/V1GameplayTruth/Final/lava-20260901-balanced/performance-30s-final/`;
the manifest SHA-256 is `AD5F16B226D84A08D159617643E7B966216EBAC11CB3BCED5AC2411E5F92EF5D`.

- PSS: **60,858–252,074 KB**; RSS: **176,511–390,924 KB**.
- Graphics PSS: **10,455–77,512 KB**.
- Raw `top` app CPU samples: **35.7–57.1%** on Android's per-core scale.
- Thermal status was **0** for all samples; configured fatal log markers were **0**.
- Lava reports 4 KB pages and Unity `gfxinfo` exposed no usable frame histogram.

The superseded visual-fix artifact also had a post-rematch point-in-time diagnostic of
292,803 KB PSS, 433,044 KB RSS, 91,260 KB graphics PSS and 126% raw `top` CPU. On the
current final artifact, the post-rematch snapshot recorded **290,132 KB PSS**,
**429,284 KB RSS**, **89,076 KB graphics PSS**, and a raw `top` sample of **111% CPU**
on Android's per-core scale. Thermal status was **0** with current CPU/GPU temperatures
**38.401 C** and battery **30.0 C**; the captured logcat window again contained no
configured fatal markers. These single-point values are retained alongside the bounded
six-sample history and are not a substitute for a normalized FPS/GC/GPU/battery
endurance run.

This is bounded raw diagnostic evidence, not normalized FPS, GC/GPU, unplugged battery,
thermal-endurance or physical-16-KB approval.

## Remaining gates and classification

The truthful classification remains **Prototype — Android offline release candidate in
progress**. Open gates are: final authored 3D models/rigs/animation/VFX/audio and cultural
review; complete physical tutorial/all-fighter/accessibility/comfort/fun route; normalized
sustained performance and battery/thermal endurance; physical 16 KB runtime; permanent
package/publisher identity and release signing; privacy policy/Data safety/IARC/support
copy; and human Play Console review/submission. No public upload, rollout, legal acceptance,
paid service or final trademark decision was performed.
