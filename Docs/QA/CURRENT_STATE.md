# BattleRaja V1 current-state index

Updated: 2026-08-27

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

## Latest current-source evidence — 2026-08-27

The runtime/presentation source is clean and committed on branch
`codex/v1-playstore-release` at exact source `8edc0867268800f0ad81067378ad590e1a166371`
(`fix: restore fighter focus on selection screen`). Full EditMode is **140/140**, full
PlayMode is **82/82**, and the 1,000-seed deterministic replay soak remains applicable
from the preceding gameplay-only source with 2,000 executions and zero divergence.
Matching APK/AAB hashes, bundletool/zipalign evidence and gate classifications are in
`Docs/V1_RELEASE_PLAN.md` P20.

The exact-source fixed-tick production-bot batch completes **100/100** matches in the
240-360 second window, with **95/100** reaching at least three combat eliminations;
100/100 have bot-to-bot damage and all recorded safety/invariant checks pass. An exact
same-seed rerun reproduces the command digest and command count. The exact APK is
debug-signed and remains a local candidate, not a publishable Play artifact. The release
checker reports **0 errors / 0 warnings**, seven ARM64 libraries, no network permissions,
and static 16 KB alignment. The APK and bundletool universal APK installed on approved
Lava `ST5GDW23LB004392`; actual menu-to-opening touch navigation reached a live match
screen with player-facing labels `GADGET TIFFIN`, `READY` and `SPAWN SHIELD`.
Fresh 30-second device evidence found no configured fatal markers, but the device reports
4 KB pages and the capture is not a sustained full-match performance pass.

The exact Lava candidate now also shows persisted Maya focus alignment, live ability/gadget
feedback, pause overlay and lifecycle resume; the tutorial opening visibly keeps the arena,
zone ring, HUD and touch sticks behind the prompt, with zero configured fatal log markers.
Action-by-action tutorial progression/full-route, accessibility, sustained performance,
genuine 16 KB runtime, authored final art/audio, cultural review, release signing and
Play/legal gates remain open.

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
- Current branch tip contains documentation follow-ups after the exact artifact
  source `35de9f3`; latest runtime-bearing source is `73f20a2`
  (`accessibility: propagate reduced flash mode`).
- Exact Android candidate source: `35de9f3`
- Current runtime-bearing source: `73f20a2` (`accessibility: propagate reduced flash mode`)
- Documentation evidence anchor: the exact-source sections in this index and
  `Docs/QA/LATEST_HEAD_BASELINE.md`, updated with the release-flags candidate.
- The exact-current release-shaped APK/AAB include the safe-area HUD and
  reduced-flash fixes. They are archived under the root-only evidence policy.
- Unity: `6000.5.6f1`
- EditMode: **125/125**
- PlayMode: **73/73**
- Repository validation: **0 errors / 0 warnings**
- Git LFS: passed

The current presentation fixes keep the runtime match HUD inside the gameplay
safe area and propagate reduced-flash settings to combat impact, hit and Aandhi
feedback. The exact-current APK installed and launched on Lava
`ST5GDW23LB004392`; the portrait menu is captured at
`Builds/Local/V1Evidence/35de9f3/Android/lava-launch.png`, with foreground window
evidence in `lava-window-state.txt`. This proves install/launch/menu presentation
only; tutorial, full-match, accessibility and performance review remain open.
The automated route reached results and returned to the menu; a 120-second
Lava diagnostic capture with 12 samples is stored under
`Builds/Local/V1Evidence/35de9f3/Android/performance-120s/`. It has no configured
fatal markers, but it does not replace human review or establish final frame,
thermal, battery or memory budgets.

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
- Run a sustained match/performance capture with the new
  `Tools/Validation/capture_android_performance.ps1` harness and interpret frame,
  CPU, memory, thermal and battery evidence.
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
runtime 16 KB proof is claimed. The strict production-bot pacing threshold and all human,
signing, legal, store and final-authored-content gates remain open.
