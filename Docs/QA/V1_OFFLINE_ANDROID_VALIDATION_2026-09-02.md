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
| EditMode | `Builds/Local/V1GameplayTruth/Next/editmode-final.xml` | **159/159 passed** |
| PlayMode | `Builds/Local/V1GameplayTruth/Next/playmode-final.xml` | **94/94 passed** |
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
- **92/100** were combat-positive; **64/100** had at least three combat eliminations;
  **3** were Aandhi-only. The report exposes the pacing window rather than treating
  terminal completion as sufficient.
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
