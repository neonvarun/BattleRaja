# BattleRaja V1 current-state index

Updated: 2026-08-30

## Evidence location policy

All new builds, logs, screenshots and test reports must remain under the ignored
`Builds/Local/` tree inside `C:\Projects\BattleRaja`. Historical append-only
notes may mention retired disposable paths for provenance, but no new work should
create or depend on evidence outside this repository root.

## Truthful classification

**Prototype — Android offline release candidate in progress.**

The offline product loop, authority/replay foundation, original procedural/vector
presentation kit, tutorial action gates, settings surfaces and Android packaging
tooling exist. A Play Store Release Candidate claim is not yet justified because
physical Lava action-by-action route review, sustained performance, final identity/
signing, accessibility, legal/privacy/cultural approval and Play Console review
remain open.

## Latest current-source evidence — 2026-08-29 — documentation tip `b4b5649` (runtime/art `ac45479`)

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
