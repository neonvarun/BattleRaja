# V1 squad perception and support fairness — 2026-09-04

## Scope

This checkpoint hardens the offline Bastion Crown squad layer without changing the
canonical Crown, ticket, damage, respawn or reward rules. It addresses a fairness defect
in which a squad plan could treat every canonical enemy position as globally visible, and
a support defect in which an out-of-range high-priority supporter could prevent a nearby
teammate from receiving a Tiffin handoff.

Source commit: `8120932` (`fix: bound squad focus and support perception`).

## Behavioural contract

- Enemy carrier/focus nominations are advisory and require the observer to be within the
  bounded `16 m` perception radius and have a clear ray through the authored
  `ArenaCollisionDefinition.BazaarBastion` geometry.
- `BotDecisionEngine` only accepts a nominated focus when the local sensor snapshot still
  contains a visible hostile target; it never forces a bot to fire through cover or beyond
  its normal target rules.
- Ally support is bounded to `18 m`. The blackboard assigns at most one supporter per
  weak ally, choosing the highest-priority *eligible* teammate rather than failing when a
  preferred Anchor is too far away. Tiffin Station receives the support context; Umbrella
  Guard and Dhol Burst keep their existing threat-gated rules.
- Presentation adapters consume immutable intent IDs as hints. Authority remains the
  source of truth for movement, targeting, gadget placement and all state changes.

## Automated evidence

- Repository validation: `Tools/Validation/validate.ps1 -RequireUnityProject` — **0
  errors / 0 warnings**.
- Full EditMode: **168/168 passed**, XML
  `Builds/Local/TestResults/squad-support-fallback-editmode.xml` (SHA-256
  `9E16F7BB8294CCD72BE0CCD7B6BA5E2AF6367585D0DE69A9C4FDCCCF58DFBA5E`), log SHA-256
  `3F72725EBC4F08B48B583BF15C83823917353805072CF5A520D1C60EE437A276`.
  This includes the authored-line-of-sight, preferred-target, support fallback and
  no-eligible-support regressions.
- Full PlayMode: **99/99 passed**, XML
  `Builds/Local/TestResults/squad-support-fallback-playmode.xml` (SHA-256
  `26D6D69B37FCF02F32FF52F5DCD52BE48E71AD1D73D5B01DD39FB82410D3DA40`), log SHA-256
  `40785C4007BDFFF3C60F987E8C6DF9ADC60DCE21DA6A8D78DE5E37C2E92DB322`.
- Strict production-bot PlayMode: **99/99 passed** for 100 seeded matches. Report
  `Builds/Local/V1GameplayTruth/ProductionBotReports/batch-20260904-155419012-9101.json`
  (SHA-256 `38089F35DB44A64CA46BF2D28E3FCD12121DE477F4123C5B34B426636283BD18`); NUnit
  XML SHA-256 `86ACD6851DA8E555CCF96AF2B8D1586398525F72A6ED66CC97B8B6DF4CE2F9C9`; log
  SHA-256 `0FD8954249467C26228A4523E829B9DB2EBC9D45BA5D49D0990D98755088120B`.

### Production-bot metrics

The 100/100 matches reached terminal results. **90/100** landed in the documented
240–360 second window (mean `236.34 s`, range `89.73–273.02 s`), **100/100** had at
least one combat elimination, **100/100** had bot-to-bot damaging pairs, and
**0/100** were Aandhi-only. Authority recorded zero invalid-position samples and zero
continuous-stuck ticks; 55 out-of-range attack attempts occurred across 18,527 attacks.

Squad telemetry recorded 177,287 signal updates (maximum signal age `4` ticks), 597,792
escort assignments, 312,829 support assignments, 183 escort handoffs, 73,691 retreat
signals and 8,212,570 ally-spacing samples. All three gadgets were exercised: 69
successful Umbrella Guard uses, 90 Dhol Burst uses and 100 Tiffin Station uses. The
batch recorded 649 combat KOs, 1,911 unique bot-to-bot damaging pairs, 653 respawns,
133 Raja deposits and 75 Rival deposits.

## Exact Android artifacts

The source was rebuilt with Unity `6000.5.6f1`, IL2CPP, ARM64, min API 28 and target API
36:

- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,685,584 bytes,
  SHA-256 `60CCC7F80617872C81CEF8A31810E0CEF34DBA415863EFF8FA3F3C41ABC14328`.
- AAB `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`, 37,511,088 bytes,
  SHA-256 `916853FBAEFFBDF9C6ABD6A39FBE97D65C53FB64B9C6DD62E45CFEA77B08FBC2`.
- The technical checker was run against the exact pair with the temporary package
  `com.example.battleraja.m11`: offline network-permission gate, ARM64-only bundle,
  static 16 KB ELF alignment and 512×512 / 1024×500 store dimensions all passed. The
  pre-documentation checker log is
  `Builds/Local/V1GameplayTruth/Next/squad-perception-release-checker.log` (SHA-256
  `5C9249BA30828B3D79CC20B51AEB449897F9665187C6A5BEEDF31E49C4226226`); a clean-tree
  rerun is required after this documentation commit.

## Approved Lava evidence

Approved device: Lava `ST5GDW23LB004392` / `LAVA_LXX508`, Android 14/API 34,
1080×2460, reported 4 KB pages. The exact APK that was installed before the final
support-fallback source change produced menu, Bastion briefing, fighter-select and live
captures under `Builds/Local/V1GameplayTruth/Next/squad-perception-20260904/lava/`.
Those captures are retained as a **prior-revision baseline**, not claimed as exact
evidence for commit `8120932`:

- `menu.png` — SHA-256
  `C65B20CB5C4DC3128B3785FE39472ADD51CC194B548A1702A21A10370CD8B466`.
- `after-play.png` — SHA-256
  `640C44BD938E1ED44B9D203804127AB6115B93F9B705C4214C275D91591A6435`.
- `fighter-select.png` — SHA-256
  `9B812DB7169FDBE04864B71A56B90BE011B3D35A989DBE7007F7A69BF3EDACC8`.
- `live-opening.png` — SHA-256
  `2D785971D764654AC4DE21708C67FA518F7BD96A9351E1E601CDF8A7E48F0886`.
- `live-after-30s.png` — SHA-256
  `65D7112C5E1779BD986A6F4A71C35A42C56D84274CD4686A16E89220A91C0CBD`.

The 30-second diagnostic in `lava/performance-30s/` recorded six samples with PSS
`292,004–299,028 KB`, RSS `407,124–422,868 KB`, graphics PSS `89,356 KB` in the first
sample, thermal status `0`, and SurfaceFlinger compositor lines ranging from about
`44–60 fps`; the captured logcat has no configured fatal markers (SHA-256
`7DCFE01447674F12430DDD04849990D1DB9AA7B1886F40866605F6C9F32FC9BD`). This is bounded
diagnostic evidence, not a normalized GPU/GC/endurance result. After the final rebuild,
the Lava serial disappeared from ADB and could not be reinstalled in this pass, so an
exact-current-source physical route, physical 16 KB runtime proof and charged sustained
performance run remain open.

## Remaining gates

This checkpoint does not close commissioned final models/rigs/animation, full
all-fighter action VFX/audio, spectate-camera comfort, normalized endurance, physical
16 KB runtime, permanent package identity/signing, privacy/Data Safety/IARC, cultural
review or Play publication. Truthful classification remains **Prototype — Android
offline release candidate in progress**.

