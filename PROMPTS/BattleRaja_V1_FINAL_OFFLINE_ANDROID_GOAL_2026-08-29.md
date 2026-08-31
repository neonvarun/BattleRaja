# BattleRaja V1.0 — Final Offline Android Game Completion Goal

[@GitHub](plugin://github@openai-curated-remote)
[@Test Android Apps](plugin://test-android-apps@openai-curated-remote)
[@Tavily AI](plugin://app-69f271663a288191ac98f46bed7cb032@openai-curated-remote)
[@Unity Essentials](plugin://unity-workbench@openai-curated-remote)
[@Game Studio](plugin://game-studio@openai-curated-remote)

You are a fresh senior game-development and Android-release agent with zero assumed conversation context.

Use Goal mode. Work through the primary agent only. Do not create subagents and do not use Sol subagents.

Work in:

C:\Projects\BattleRaja

Repository:

neonvarun/BattleRaja

Approved physical Android device:

- Lava phone only.
- Historical serial: ST5GDW23LB004392.
- Never use the Oppo device for evidence.

The previous Codex task containing project assets and context is:

codex://threads/01a034a5-6471-7470-aa7b-eae82bc4142b

Read it when accessible, but current source, current build configuration, current runtime behavior, and exact current evidence always win.

This file is the local V1 Goal handoff. Read it completely before implementation. Do not treat a historical prompt, report, screenshot, or build as current without verifying it.

Goal invocation: `/goal Read and execute PROMPTS/BattleRaja_V1_FINAL_OFFLINE_ANDROID_GOAL_2026-08-29.md from the current BattleRaja workspace. Rebaseline its live gate snapshot before making changes.`

---

# 1. Live Gate Snapshot

Snapshot recorded: 2026-08-29 16:04:40 IST.

The last observed clean repository state was:

- Branch: codex/v1-playstore-release.
- HEAD: ad078d32b44f697a51e8013ad55861fc43852f11.
- origin/main: ad078d32b44f697a51e8013ad55861fc43852f11.
- Latest presentation-bearing source: 816d9ac.
- Latest policy-evidence documentation commit: ad078d3.

Do not assume these values remain current. At the beginning of Goal execution, record a new timestamp and refresh this gate table from the repository. Do not edit this prompt merely to rewrite history; append a dated update only when the current handoff itself has materially changed.

The latest exact-source presentation evidence records:

- EditMode: 141/141 passed.
- PlayMode: 87/87 passed.
- Static validation: 0 errors / 0 warnings.
- APK: 40,542,342 bytes, SHA-256 0517EE901A9EAE943140538366B0574E893DC6BD66A5D1714D630C2379EF5FAC.
- AAB: 36,367,513 bytes, SHA-256 BF52E649BFD92F277F5C9933A7FDF34FFB25410F1D5A18EF6FC3097AA31BA331.
- Offline manifest, ARM64, static 16 KB, bundletool extraction, zipalign, v3 verification, and store-dimension checks pass for that temporary candidate.
- A bounded Lava launch/live-match diagnostic found no configured fatal markers and thermal status 0.

The latest gameplay/replay evidence records:

- 1,000-seed deterministic replay soak executed twice: 2,000 executions with zero divergence.
- Ordered persistent .brr production replay capture and exact-file re-execution.
- 100/100 production-bot terminal matches in the 240–360 second window.
- 94/100 matches with at least three combat eliminations.
- 100/100 matches with bot-to-bot damage.
- Zero Aandhi-only resolutions.
- No protected, invalid-position, or continuous-stuck invariant failures.
- A genuine 16 KB Android emulator runtime check.
- Bounded Lava evidence through match, defeat, spectator, results, rematch, and settings paths.

The truthful classification at this snapshot is:

> Prototype — Android offline release candidate in progress.

Open gates include final authored character/environment presentation, complete all-fighter/action-by-action physical QA, accessibility and human feel review, sustained normalized performance, final identity and signing, privacy/Data Safety, cultural/legal approval, and Play Console actions.

Use Docs/QA/CURRENT_STATE.md, Docs/QA/LATEST_HEAD_BASELINE.md, and Docs/V1_RELEASE_PLAN.md as the exact evidence indexes. The 16 KB emulator gate is not the same as physical Lava 16 KB proof; Lava reports 4 KB pages. Bounded PSS, RSS, thermal, and SurfaceFlinger captures are not normalized final performance approval.

---

# 2. First Repository Inspection

Run:

~~~powershell
git fetch --all --prune
git status --short --branch
git rev-parse HEAD
git rev-parse origin/main
git log --oneline --decorate -40
git branch -vv
git stash list
git lfs fsck --pointers
~~~

If the Goal environment already provides an isolated worktree or branch, use it. If the clean current branch already points to current origin/main, continue there. Do not create a second branch just because an older prompt requested one.

Create a branch such as codex/v1-final-completion only when the environment is not isolated and a separate branch is genuinely needed.

Never reset, rewrite, delete, overwrite, or discard user work. Do not apply or remove stashes without explicit approval. Keep new evidence under Builds/Local/ inside the repository.

GitHub access is read-only unless the owner explicitly authorizes a mutation. Do not push, open or merge pull requests, create tags/releases, upload binaries, modify issues, or change repository settings.

---

# 3. Mandatory Reading

Read the complete current versions of:

- AGENTS.md
- PROJECT_STATUS.md
- PROJECT_CONTEXT.json
- Docs/MASTER_VISION.md
- Docs/ARCHITECTURE.md
- Docs/DECISIONS.md
- Docs/RESEARCH_LOG.md
- Docs/CULTURAL_GUIDE.md
- Docs/ART_BIBLE.md
- Docs/AUDIO_BIBLE.md
- Docs/ASSET_PROVENANCE.md
- Docs/V1_RELEASE_PLAN.md
- Docs/BALANCE_CHANGELOG.md
- Docs/PERFORMANCE_BUDGET.md
- Docs/HUMAN_REVIEW_BACKLOG.md
- Docs/RELEASE_CHECKLIST.md
- Docs/RELEASE/V1_ANDROID_RELEASE_CHECKLIST.md
- Docs/RELEASE/STORE_CREATIVE_BRIEF.md
- Docs/QA/CURRENT_STATE.md
- Docs/QA/LATEST_HEAD_BASELINE.md
- Docs/QA/REPLAY_AND_SOAK_REPORT.md
- Docs/QA/VISUAL_QA_REPORT.md
- Docs/Research/REFERENCE_UX_AUDIT.md
- all exact-current reports linked by those indexes.

Inspect actual first-party source, scenes, prefabs, colliders, models, meshes, materials, textures, animation clips/controllers, VFX, UI, audio, tests, editor tools, build tools, Android settings, manifests, packages, and existing artifacts.

START_HERE.md is historical Milestone-0 material. Do not restart completed milestones.

Use this evidence order when documents conflict:

1. Current source and serialized configuration.
2. Exact-current machine evidence.
3. Current status and QA indexes.
4. Historical append-only reports.
5. Older prompts and assumptions.

---

# 4. V1 Scope Lock

V1 is offline Android only:

- One human plus seven bots.
- Bazaar Bastion.
- Bijli, Pehel, and Maya.
- Umbrella Guard, Dhol Burst, and Tiffin Station.
- Aandhi.
- Tutorial.
- Full match.
- Elimination and spectator mode.
- Results.
- Rematch.
- Settings and accessibility.
- No login and no internet requirement.

Do not implement or expand Photon, PlayFab, multiplayer, matchmaking, accounts, online progression, cloud progression, Web release, ads, IAP, online leaderboards, social systems, or live-service infrastructure.

Preserve future seams internally, but spend no meaningful V1 development time on them. Hide unfinished Online functionality from the player-facing V1 menu.

---

# 5. Architecture and Evidence Rules

Preserve the proven authority, deterministic simulation, replay, bot-command, and data-driven architecture.

- Keep pure gameplay/domain code independent of Unity UI, scenes, animation, Photon, and PlayFab.
- Human input and bot decisions must produce common gameplay commands.
- Do not store mutable runtime state in shared ScriptableObjects.
- Keep seeded injectable randomness and simulation/render separation.
- Art, animation, and VFX must never decide damage, collision, cooldown, movement, capture, or elimination.
- Never change authority collision merely to fit decoration.
- Avoid hidden global mutable state.
- Do not casually rewrite authority or replay code.

Only change gameplay/authority when there is a reproducible defect or evidenced feel problem. Add regression coverage, assess replay implications, record before/after evidence, and update Docs/DECISIONS.md for material architectural choices.

Do not rerun every already-closed expensive gate before changes. Re-run the smallest affected gate after each material change, then run the complete final matrix after the candidate stabilizes.

---

# 6. Plugins and Their Boundaries

Use the requested plugins when available:

- Unity Essentials: Unity project inspection, implementation, editor tooling, build validation, and Unity-specific QA.
- Test Android Apps: Android installation, ADB/UI-tree navigation, screenshots, logs, lifecycle testing, and Lava evidence. Use only the approved Lava device for physical evidence.
- Tavily AI: focused current web research. For Play, Android, Unity, SDK, policy, or legal facts, use official primary sources and record sources/date/impact in Docs/RESEARCH_LOG.md.
- Game Studio: cross-domain game-quality planning, player-loop, asset, UI, and playtest reasoning. This V1 runtime is Unity, not a browser-game implementation; do not route work into a Web game or change the engine.
- GitHub: read-only repository and history inspection unless the owner authorizes mutations.

Do not install tools, download paid assets, accept licenses, purchase services, or add unapproved dependencies without owner approval.

---

# 7. Originality and Reference-Game Research

Use a coherent original direction:

> Colourful stylized fictional bazaar-fortress battle arena with bold silhouettes, readable mobile combat, playful materials, compact low-poly geometry, and toy-box energy.

Use warm sandstone, terracotta, teal, saffron, rose, electric cyan, and other documented BattleRaja colors where appropriate. Follow Docs/CULTURAL_GUIDE.md. Avoid sacred imagery, political symbols, stereotypes, community caricatures, and direct imitation.

Brawl Stars and Smash Karts may be observed only for abstract usability qualities such as touch-target size, information hierarchy, menu depth, tutorial pacing, camera readability, feedback timing, settings discoverability, and rematch friction.

The controlled observation is already documented in Docs/Research/REFERENCE_UX_AUDIT.md. Do not repeat it unless a narrow unanswered design question or a material BattleRaja navigation change justifies it.

Do not decompile, extract assets, inspect private app data, intercept traffic, bypass protections, create accounts, make purchases, send messages, or accept legal agreements. Do not copy characters, costumes, silhouettes, maps, props, icons, fonts, logos, terminology, layouts, animation, audio, music, VFX, colors, progression, or trade dress.

---

# 8. 3D Characters and Animation

The repository has a generated/faceted, repository-owned fighter presentation baseline. Preserve it as a fallback and integration scaffold. It is not automatically final commissioned art or human cultural/fun approval.

Build one complete production vertical slice first, preferably Bijli. Validate gameplay-camera silhouette, topology, UVs, materials, rigging, skinning, animation deformation, attachment points, prefab integration, reduced-flash readability, Android performance, editable source, and provenance before applying the pipeline to Pehel and Maya.

Create original mobile-appropriate models, materials, rigs, skinning, prefabs, and animation for:

## Bijli

Slim/agile, angular, electric, cyan/gold direction, clearly fast. Finish idle, locomotion, aim, attack, dash, hit, knockback, elimination, victory, defeat, electric attack, dash trail, and impact feedback.

## Pehel

Broad, grounded, heavy grappler/tank, warm clay/cream/metal direction, immediately distinct from Bijli. Finish idle, locomotion, aim, attack, charge, capture, carry, throw, hit, knockback, elimination, victory, defeat, charge dust, and throw impact.

## Maya

Asymmetric agile trickster, original scarf/shard/crystal language, violet/mint/rose direction, distinct from both others. Finish idle, locomotion, aim, attack, decoy summon, recovery, hit, knockback, elimination, victory, defeat, projectile, shimmer, and decoy disappearance. Keep player and decoy distinguishable.

Use Blender if already available and suitable; otherwise use the strongest safe local Unity/editor/modeling pipeline. Do not leave permanent cubes, capsules, or unrefined primitives merely because structural tests pass. Do not spend polygons or texture memory on details invisible from the actual phone camera.

Every replacement must retain editable source, Unity export, mobile topology, UVs/materials, rig/skin, animation integration, prefab, provenance, production-camera captures, and performance evidence.

---

# 9. Arena, Gadgets, Pickups, and VFX

Finish Bazaar Bastion without changing validated combat geometry. Create/polish stalls, shop fronts, fortress walls, arches, awnings, canopies, pottery, crates, lanterns, banners, fictional signage, ground tiles, mosaic accents, central and edge landmarks, skyline/background, readable cover, and combat lanes.

Finish Tutorial Arena presentation without adding unapproved V1 maps.

Finish original recognizable assets and feedback for:

- Umbrella Guard: shield state, facing, protected arc, vulnerable direction, expiry.
- Dhol Burst: activation, radius, shockwave, timing, knockback.
- Tiffin Station: deployment, healing radius/ticks, damage, expiry/destruction.
- Health and gadget pickups, pickup beacons, projectiles, spawn protection, damage, healing, knockback, elimination, victory, defeat, and Aandhi.

Create a coherent low-overdraw VFX language for Bijli bolts/dash, Pehel charge/capture/throw, Maya projectiles/decoys, all gadgets, pickups, impacts, eliminations, and the Aandhi warning/closing/final circle.

Reduced-flash mode must preserve gameplay information through shape, timing, motion, contrast, and outlines. Pool repeated effects and avoid per-frame allocations.

---

# 10. UI, Audio, and Player Experience

Polish splash/loading, logo, main menu, dominant Play Offline CTA, fighter selection/cards, tutorial, match HUD, health/status, Aandhi indicator, ability/gadget controls, pause, settings, spectator, results, rematch, return-to-menu, help, and error/retry states.

Use original BattleRaja geometry, typography, icons, layouts, and copy. Remove debug labels, developer controls, dead Online buttons, prototype text, clipping, overlaps, weak spacing, and unreadable hierarchy.

Verify configured orientation policy, safe areas, aspect ratios, large touch targets, left-handed mode, reduced flashes, high contrast, text scaling where practical, haptics, aim assist, tutorial replay, music volume, and effects volume.

Audit the existing owned WAV files and mixer rather than treating the project as runtime-only procedural audio. Complete distinct cues for all fighters, abilities, gadgets, attacks, hits, healing, pickups, eliminations, Aandhi, victory, defeat, UI, Bazaar ambience, match music, and final-circle intensity. Validate routing, voice limits, clipping, repetition, settings, background/resume, and Lava loudness.

Use only original, generated, recorded, or properly licensed audio. Do not use reference-game audio.

---

# 11. Gameplay Feel and Bot Quality

Play the actual game repeatedly. Automation does not prove fun.

Inspect movement, aiming, camera, attack cadence, TTK, abilities, dash, Pehel capture/throw, Maya decoy, gadget value, encounter frequency, Aandhi pressure, match duration, spectator pacing, results, and rematch desire. Target roughly 4–6 minute matches unless evidence supports another range.

Evaluate bots for navigation, target selection, threat response, range management, ability/gadget timing, pickups, Aandhi awareness, retreat/re-engagement, fighter-specific behavior, bot-to-bot combat, endgame behavior, rejected commands, and deterministic reproducibility.

Bots must use the same command and authority rules as humans. They must not cheat through impossible information, direct state mutation, invalid movement, or bypassed cooldowns.

Make balance changes only from reproducible before/after evidence and update Docs/BALANCE_CHANGELOG.md.

---

# 12. Performance and Android Hardening

Profile the final-art build, not the old greybox build. Measure Unity main/render CPU, GPU or frame timing, frame pacing, GC, allocations, managed/native/graphics memory, batches, draw calls, triangles, textures, shaders, VFX, audio voices, loading, APK/AAB size, repeated-match growth, background/resume, thermal, and battery behavior.

Optimize evidenced problems in meshes, textures, materials, shaders, particle count, overdraw, pooling, searches, per-frame allocations, unnecessary Updates, and audio voices. Preserve critical telegraphs.

Recheck current official Google Play/Android requirements immediately before final signed-AAB preparation and before upload. Verify target API 36 or newer as required at submission time, ARM64, IL2CPP, 16 KB compatibility, AAB, versioning, permissions, offline behavior, native dependencies, licences, secrets, debug flags, and release configuration.

The current temporary package is com.example.battleraja.m11. Prepare owner-selectable permanent package/publisher options, but do not silently choose an irreversible final identity. Do not handle or expose the final signing key.

---

# 13. Lava QA and Final Validation

On the exact final candidate and approved Lava device, verify launch, menu, tutorial, all three fighters, all three gadgets, every attack/ability, Aandhi, elimination, spectator, victory/defeat, results, rematch, settings, left-handed mode, reduced flashes, high contrast, text scaling, haptics, aim assist, audio, background/resume, and several consecutive matches.

Use UI-tree-derived coordinates for Android taps when the Android QA plugin is available. Capture screenshots, logs, package identity, build hash, device identity, and exact route. Do not claim human comfort, fun, cultural, accessibility, or action-by-action approval unless actually performed.

After candidate stabilization, run the applicable full matrix:

- Repository/static validation.
- Full EditMode and PlayMode.
- Deterministic replay and soak when invalidated.
- Production-bot batch when invalidated.
- Persistent replay capture/re-execution.
- Release APK/AAB build.
- Bundletool, ABI, manifest, alignment, signature, 16 KB, and checker validation.
- Secret/licence/dependency scan.
- Crash/fatal-log review.
- Lava route, repeated-match, settings, lifecycle, and accessibility QA.
- Final-art performance profiling.
- Store-asset dimension validation.

Do not classify untested gates as passed.

---

# 14. Store Assets and Owner Gates

Create final original app icon, feature graphic, menu screenshot, fighter-selection screenshot, Bazaar combat screenshot, ability/gadget screenshot, Aandhi screenshot, results screenshot, and useful accessibility/settings screenshot.

Gameplay screenshots must come from the actual final game. Do not fake gameplay with generated imagery.

Image generation may assist with original concept development or non-gameplay store composition, but final gameplay screenshots must come from the exact game candidate.

Prepare accurate title, short/full description, release notes, tester instructions, known issues, support copy, privacy-policy draft, Data Safety draft, target-audience/content-rating preparation, and submission checklist.

Owner approval is required for final package ID, publisher identity, signing key, paid tools/services/assets, legal/privacy approval, cultural approval, final subjective art/fun/accessibility approval, Play Console questionnaires, upload, and rollout.

Complete every local/reversible task before stopping. Do not use human review as an excuse to leave obvious technical or visual work unfinished.

---

# 15. Truthful Completion Gate

Call the project a Play Store Release Candidate only when:

1. Offline launch → tutorial/match → spectator → results → rematch works.
2. Bijli, Pehel, and Maya are production-ready and immediately distinguishable.
3. Models, materials, rigs, skinning, animation, VFX, gadgets, pickups, and Bazaar Bastion are finished.
4. UI, audio, accessibility, tutorial, and bot behavior are coherent and usable.
5. Human gameplay and physical-device review find no critical issue.
6. Automated regression, deterministic replay, soak, and required bot gates pass.
7. Final-art performance and repeated-match stability are measured and acceptable.
8. Android target/API, ARM64, AAB, alignment, 16 KB, permissions, and release checks pass.
9. Final store assets and metadata drafts are ready.
10. No Critical or High known release defect remains.

Otherwise classify the state honestly as prototype, internal alpha, release candidate in progress, or technically ready pending named owner gates.

Maintain a gate table with: machine-passed, machine-failed, not tested, human review required, owner/external action required, and superseded evidence.

---

# 16. Stopping Condition and Final Report

Stop only when all authorized local work is complete or further progress genuinely requires an owner decision, signing secret, paid service, legal acceptance, Play Console access, unavailable physical interaction, public upload, or subjective final approval.

Do not begin Photon, PlayFab, multiplayer, Web product development, or unrelated feature work.

At completion report:

- Final classification.
- Final branch, commit, and working-tree status.
- Whether anything was pushed.
- Changed files and documentation.
- Complete fighter/model/editable-source inventory.
- Rig/animation inventory.
- Environment, gadget, pickup, VFX, UI, and audio inventory.
- Tests, replay/soak, and bot-batch results.
- Lava routes completed and limitations.
- Final performance, memory, thermal, and battery measurements.
- APK/AAB absolute paths, sizes, hashes, package ID, version, and signing classification.
- Screenshot and store-creative paths.
- Warnings, failures, skipped checks, and superseded evidence.
- Remaining owner gates.
- Exact Play Store submission checklist.
- One recommended next action.

Never report a debug or temporary-signed artifact as production-signed. Never claim legal, cultural, accessibility, visual, Play Console, or human approval that did not occur.

The final objective is an original, polished, fun, readable, performant, fully offline Android V1.0 game that is genuinely ready for players and ready for the owner’s final release actions.
