# BattleRaja V1.0 release notes, tester and support draft

This is an owner-selectable handoff for the exact offline Android candidate. It is a draft,
not a Play Console submission, legal approval, support commitment or final public copy. Do
not publish it until the package identity, signer, privacy policy, Data safety answers,
content rating, target audience, screenshots and support destination are approved.

## Candidate anchor

- Source checkpoint: `5d136fbb6be6a5554931f6ab859be8b9a8a995a2`.
- Candidate version: `1.0.0` / version code `100`.
- Current temporary application ID: `com.example.battleraja.m11`.
- Candidate APK: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`.
- Candidate AAB: `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab`.
- The APK and AAB are temporary Android Debug-signed artifacts. They are not production-
  signed and must not be uploaded as the final release.

## Release notes draft — v1.0.0

Enter Bazaar Bastion for a compact, fully offline micro battle royale:

- Choose Bijli, Pehel or Maya, each with a distinct attack, ability and play pattern.
- Face seven deterministic bots in a local Solo Raja match.
- Collect and use Umbrella Guard, Dhol Burst and Tiffin Station.
- Read the closing Aandhi, survive the final circle and keep fighting after elimination in
  spectator mode.
- Review placement, rematch immediately and tune local settings for your play style.
- Use the tutorial to learn movement, aim, attack, ability, gadget, Aandhi, elimination and
  results in sequence.

This draft describes the current offline candidate only. It does not promise accounts,
matchmaking, multiplayer, cloud progression, ads, purchases or an internet connection.

## Invited-tester quick start

1. Install the exact APK supplied by the owner. For the current evidence route, use Lava
   `ST5GDW23LB004392`; do not substitute the excluded Oppo device for release evidence.
2. Launch with the device online or offline, then choose **Play Offline** → **Solo Raja**.
3. Open each fighter card once. In a match, verify movement, aim, basic attack, ability and
   the available gadget feedback. Let the Aandhi close the arena and observe defeat,
   spectator, placement, rematch and return-to-menu behavior.
4. Open Settings and exercise handedness, reduced flashes, high contrast, text scale, aim
   assist, haptics, music and effects controls. Background and resume the app and confirm
   settings remain persisted.
5. Replay the tutorial and confirm the visible completion state. Record the selected fighter,
   gadget, route, build label, device model/OS and whether the issue reproduces.

Do not enter real credentials, attach private data, or place tokens/passwords in feedback.
The V1 product scope is offline Android; Web and online-service checks are not release gates
for this candidate.

## Known issues and open limitations

- The current package ID is the temporary `com.example.battleraja.m11`; final publisher
  identity and package choice remain owner-controlled.
- The current APK/AAB use a temporary debug signer; final signing-key handling and Play App
  Signing enrollment remain owner-controlled.
- Lava `ST5GDW23LB004392` reports 4 KB pages. The host-GPU Android 16 `BattleRaja_16K`
  emulator provided a genuine 16 KB smoke, but this is not physical 16 KB proof or coverage
  for every renderer/device profile.
- The Lava P48/P50 captures are bounded diagnostics. `gfxinfo` supplied no usable frame
  histogram, and no normalized sustained CPU/GPU, battery or thermal approval is claimed.
- The current fixed-tick production-bot release-gate rerun completed **100/100** seeded
  matches in the 240-360 second window with combat and bot-to-bot damage in every match,
  zero Aandhi-only resolutions and all three gadgets exercised in every match. The exact
  report and test hashes are indexed in `Docs/V1_RELEASE_PLAN.md` P53. Historical timing-
  sensitive retries are retained for auditability; accelerated 50x playback is not used
  as same-seed determinism evidence.
- Physical route observation has not individually verified every Umbrella Guard, Dhol Burst
  and Victory presentation path on Lava, even though automated and other route evidence
  covers the underlying systems.
- Final authored-art, cultural, accessibility comfort, fun, privacy/legal, content-rating,
  target-audience, publisher-identity and Play Console review remain human/owner gates.
- The SwiftShader 16 KB AVD run exposed a renderer-profile URP/Lit uniform-limit failure;
  the host-GPU profile is the supported smoke evidence and the SwiftShader result is retained
  as superseded renderer diagnostics.

## Support copy draft

Use only after the owner replaces the placeholders with an approved support destination:

> Need help with BattleRaja? Please contact **[approved support URL or email]** and include
> your build label, device model and Android version, the fighter/gadget selected, the scene
> where the issue occurred, exact reproduction steps, expected versus observed behavior,
> frequency/severity and a sanitized log excerpt. Do not send passwords, authentication
> tokens, payment details or other personal information.

### Feedback record

- Build label / APK or AAB hash:
- Device model / serial policy / Android version / page size:
- Scene and selected fighter/gadget:
- Steps to reproduce:
- Expected result:
- Observed result:
- Frequency and severity:
- Screenshot or video (if approved):
- Sanitized log excerpt (no credentials or personal data):

## Owner submission checklist

- [ ] Choose and approve the permanent package ID and publisher identity.
- [ ] Produce the final release-signed AAB without exposing or committing the signing key.
- [ ] Re-run manifest, version, ARM64, native-dependency, ELF/zip alignment and 16 KB checks
      against that exact signed artifact.
- [ ] Approve the privacy policy, Data safety answers, target audience and content-rating
      questionnaire with the final build and dependency inventory.
- [ ] Approve final screenshots, icon, feature graphic, store copy, cultural framing,
      accessibility presentation and support destination.
- [ ] Paste the approved release notes and tester/support copy into the appropriate Play
      Console fields; complete Play Console questionnaires and upload only after review.
- [ ] Confirm closed-test track, tester list, rollout plan, monitoring and rollback artifact.

See `Docs/RELEASE/V1_ANDROID_RELEASE_CHECKLIST.md`,
`Docs/RELEASE/PRIVACY_POLICY_DRAFT.md`, `Docs/PRIVACY_DATA_SAFETY_WORKSHEET.md`,
`Docs/RELEASE/PLAY_CONTENT_RATING_PREP.md` and `Docs/CLOSED_TEST_INSTRUCTIONS.md` for the
corresponding technical and policy checklists.
