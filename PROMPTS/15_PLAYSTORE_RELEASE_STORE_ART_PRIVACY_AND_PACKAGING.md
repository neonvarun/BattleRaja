# 15 — Play Store Release, Store Art, Privacy and Packaging

## Context

The repository has a candidate APK/AAB and release drafts, but the package is temporary/debug-signed (`com.example.battleraja.m11`) and the store/privacy/legal materials are not final. This stage prepares everything safe to prepare for an offline 4v4 V1 while leaving signing identity, legal approval and Play upload to the owner.

## Objective

Produce a truthful, reproducible release package and store-asset set that represents the actual BattleRaja build: offline Bastion Crown 4v4, no account/network dependency, original art/audio, accurate permissions and honest data-safety/content-rating/support text.

## Current-state audit

Inspect Unity/Android build settings, Gradle/IL2CPP/NDK versions, manifest/permissions, package ID/version code, ARM64 libraries, target/min SDK, debug/development flags, symbols, AAB/APK hashes, bundletool/zipalign reports, dependency/license inventory, privacy/data-safety/rating drafts, store creative brief and exact final-art screenshots. Re-check current official Google Play/Android requirements at execution time.

## Preserve

Preserve offline/no-login/no-ads/no-IAP behavior, future networking seams without runtime dependency, provenance/attribution, reproducible build scripts, local settings and accurate release notes. Keep owner-only gates explicit and never invent a permanent identity or sign/upload without authorization.

## Replace/fix

Replace temporary package/signing metadata, Solo-only store copy, debug labels, inaccurate permissions, placeholder/fake screenshots, stale privacy/data-safety claims, missing content-rating/support fields and any dependency that secretly uploads data or requires a network.

## Implementation tasks

1. Re-check official sources: target API policy, 16 KB page-size guidance, Play Data Safety, content rating and app-review preparation. Record URL/date/claim/impact in `Docs/RESEARCH_LOG.md`.
2. Prepare a release configuration with target API 36 or newer if currently required, min API 28 or a documented change, ARM64 IL2CPP, offline manifest, no unnecessary network permissions/services, release stripping and reproducible versioning. Use a clearly marked placeholder ID only until the owner supplies the permanent package identity.
3. Produce release APK for local QA and AAB for Play validation. Validate manifest, ABI, signing mode, version code/name, bundletool installability, zip alignment, native library page-size compatibility and absence of development/debug configuration. Keep final signing-key handling owner-gated.
4. Inventory Unity/package/native dependencies and licenses; scan secrets and bundled SDK behavior. Any data collection by a third-party SDK must be reflected or the SDK removed.
5. Draft privacy policy, Data Safety answers, content-rating questionnaire, target-audience declaration, ads declaration, support information, tester instructions, known issues, short/full descriptions and release notes for actual offline behavior. Mark legal/owner approval required.
6. Create original store assets from the exact final-art build: icon, feature graphic, portrait/landscape phone screenshots of menu, fighter selection, Bazaar combat, Crown/gadget action, team HUD/Aandhi and results/rematch. Do not fake gameplay or use reference captures.
7. Check screenshots for readable 4v4 team/objective truth, no debug labels, no temporary package/build watermark and no unapproved claims. Retain source files and provenance.

## Asset tasks

Create editable 512/1024 app icon variants, Play feature graphic in the current required dimensions, original screenshot frames/crops, store captions, localized-safe text layers and release-note artwork. Keep source/provenance/license records and ensure every image is a real frame from the final candidate.

## Integration points

Integrate build settings with Unity scenes/assets, Android manifest, Gradle/IL2CPP/NDK, package/license scanner, artifact hashing, store metadata drafts, privacy/data-safety behavior and QA evidence. Do not alter gameplay to stage a store shot.

## Performance constraints

Store assets must not increase runtime memory or ship unused high-resolution art. Release AAB must meet measured size/performance budgets from prompt 13. Validate installation and launch from the same AAB that produced representative screenshots where practical.

## Tests

Add/execute static release checks for target/min SDK, ARM64, 16 KB compatibility, permissions, debug flags, package/version, dependency/license/secrets, Data Safety consistency and no online startup. Run full gameplay tests, AAB bundletool install, APK/AAB hash capture and store-art provenance checks.

## Visual QA

Inspect every store image against the actual device build and every major in-app surface at final aspect/accessibility settings. Reject fake compositions, unreadable HUD, placeholder art, temporary branding, misleading score/objective claims or copied reference visuals.

## Lava verification

Install the exact release candidate on Lava `ST5GDW23LB004392` in airplane mode and verify launch, `PLAY OFFLINE`, 4v4, results/rematch, settings and no network prompt. Capture final representative frames and package metadata; never use Oppo. Physical 16 KB proof must be a separately qualified environment, not inferred from Lava's 4 KB report.

## Failure cases

Test AAB install/update, missing ABI, target/API mismatch, 4 KB/16 KB validation failure, debug flag, permission prompt, package mismatch, offline launch, stale store screenshot, missing policy field, third-party data behavior, broken support link and signing-key/owner gate. Stop rather than fabricate a pass.

## Binary acceptance gate

Pass only when a reproducible release APK/AAB is built from final-art source, technical Play checks pass, artifacts/hashes/dependencies/licenses are recorded, store creatives are genuine and complete, privacy/Data Safety/rating/support drafts match behavior, and all owner-only actions are clearly separated. Temporary/debug identity or unverified 16 KB is not release-ready.

## Evidence to retain

Official-source log, build settings, manifest/ABI/page-size/bundletool/zipalign reports, dependency/license/secret scan, APK/AAB hashes, provenance/source creative files, final screenshot contact sheet, privacy/Data Safety/rating drafts, Lava install notes and owner-gate list.

## Non-scope

Do not create or expose signing keys, accept legal agreements, upload to Play Console, publish/roll out, add analytics/ads/IAP/accounts or change online architecture.

## Stop condition

Stop before prompt 16 if the final candidate cannot be installed/verified, store art is not representative, package/signing/16 KB status is unknown, policy drafts are inaccurate or any owner-only action would be required.
