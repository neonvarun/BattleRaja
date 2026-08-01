# BATTLERAJA — MASTER PROJECT VISION, REQUIREMENTS & AI DEVELOPMENT DIRECTIVE

**Document type:** Product vision + game design requirements + technical architecture + AI-agent execution contract  
**Primary consumer:** OpenAI Codex using the Luna 5.6 xhigh model or the strongest available high-reasoning coding model  
**Human project owner:** Varunkumar Singh / Avinya Studios  
**Working title:** BattleRaja  
**Document version:** 2.0  
**Research date:** 1 August 2026  
**Primary launch platforms:** Android application and browser-playable Web build  
**Secondary development platform:** Windows  
**Engine recommendation:** Unity 6 LTS/production-supported Unity 6 release, C#, Universal Render Pipeline  
**Status:** Pre-production source of truth  

---

# 0. HOW THE AI AGENT MUST USE THIS FILE

This document is the authoritative product and engineering brief for BattleRaja.

The coding agent must:

1. Read this entire file before creating or changing the project.
2. Treat explicit requirements as authoritative.
3. Distinguish:
   - **MUST**: required for the relevant milestone.
   - **SHOULD**: preferred unless technical evidence justifies deviation.
   - **MAY**: optional.
4. Never attempt to build the final live-service game in one unreviewed pass.
5. Convert the project into small milestones, GitHub issues and acceptance criteria.
6. Implement the offline deterministic core before adding networking.
7. Use placeholders where final art, audio, credentials or paid services are unavailable.
8. Never silently invent credentials, secret keys, legal claims, trademarks, paid assets or third-party licences.
9. Record assumptions and unresolved decisions in `/Docs/DECISIONS.md`.
10. Validate every milestone through compilation, automated tests and a playable build.
11. Prefer a correct, maintainable small feature over a broad but fragile implementation.
12. Preserve existing functioning systems unless the current task explicitly authorises replacement.
13. Stop and report a blocker only when it cannot be resolved through repository inspection, official documentation, local tooling or a safe placeholder.
14. Perform current web research from official sources before adopting packages, APIs, SDK versions, platform requirements or store policies.
15. Cite researched technical decisions inside `/Docs/RESEARCH_LOG.md`.

The agent is authorised to create code, project files, documentation, tests, editor tooling, CI scripts, placeholder assets and build automation. Destructive actions, paid purchases, production deployments, publishing, deletion of user data, accepting legal agreements and exposing secrets require human approval.

---

# 1. EXECUTIVE VISION

BattleRaja is a cross-platform Android-and-Web, fast-session, stylised top-down 3D arena battle royale inspired by the immediacy and visual readability of games such as Brawl Stars and the compact toy-like chaos of Smash Karts.

It must not be a clone.

Its identity comes from:

- An original fictional universe inspired by the landscapes, visual languages, everyday ingenuity, crafts, music, clothing silhouettes and architecture of the Indian subcontinent.
- Short 4–6 minute survival matches.
- Small, highly readable fighters.
- Twin-stick action combat.
- Temporary improvised match pickups called **Jugaad Gadgets**.
- A closing storm called the **Aandhi**.
- Strong performance on mid-range Android devices and supported browsers.
- Shared Android–Web gameplay and cross-play-compatible architecture.
- Fair competition without pay-to-win progression.
- A colourful, playful tone rather than realistic military violence.

## 1.1 Elevator pitch

> BattleRaja is a fast top-down battle royale where colourful champions combine signature abilities with temporary Jugaad Gadgets while an Aandhi closes around a constantly changing arena.

## 1.2 Player fantasy

The player should feel like a clever, expressive champion surviving a chaotic animated arena by combining mechanical skill, positioning, timing and improvised gadgets.

## 1.3 Target audience

Primary:

- Players aged approximately 13–30.
- Android users in India and other mobile-first markets.
- Players who enjoy short competitive sessions.
- Players attracted to colourful character-based games.
- Players who find long battle royales too time-consuming.

Secondary:

- Casual groups seeking private matches.
- PC players if a Windows build is later released.
- International players drawn to an original South Asian-inspired fantasy world.

## 1.4 Product principles

1. **Fun before scale.**
2. **Readability before visual complexity.**
3. **Responsive controls before content volume.**
4. **Offline core before online networking.**
5. **Skill expression without excessive cognitive load.**
6. **Cultural inspiration without caricature or sacred misuse.**
7. **Cosmetic monetisation without purchased combat power.**
8. **Mid-range Android and browser performance as first-class requirements.**
9. **Data-driven content instead of duplicated code.**
10. **Server authority for competitive public matches.**

---

# 2. NON-GOALS

The initial product is not:

- A 50–100 player PUBG-style battle royale.
- A realistic military shooter.
- An open-world game.
- A photorealistic game.
- A vehicle-only kart combat game.
- A direct Brawl Stars or Smash Karts clone.
- A blockchain, NFT or real-money economy product.
- A gacha-first product.
- A game containing real political parties, active religious figures or living public personalities.
- A project that requires a massive custom backend before gameplay is validated.
- A game with 20 characters at prototype stage.
- A full esports platform at launch.
- A native-PC/console-first title.
- A separate simplified browser remake disconnected from the main Unity gameplay code.

---

# 3. SUCCESS DEFINITION

## 3.1 Prototype success

The grey-box prototype succeeds when:

- A new player can understand movement, aiming and attacking within 30 seconds.
- One complete match can be played from spawn to victory.
- One human and seven bots can finish a 4–6 minute match.
- Combat remains visually understandable with eight combatants.
- The Android build maintains the target performance class.
- Playtesters voluntarily request another match.

## 3.2 Vertical-slice success

The vertical slice succeeds when:

- It contains one polished arena.
- It contains three distinct polished fighters.
- It contains at least three Jugaad Gadgets.
- It communicates an original BattleRaja identity within screenshots or 15 seconds of video.
- Combat feedback feels satisfying on a physical phone.
- Ten external playtesters can complete the tutorial without developer assistance.
- Severe crashes, progression loss and match blockers are absent.

## 3.3 Online alpha success

The online alpha succeeds when:

- Eight players/bots can complete matches under realistic Indian mobile-network conditions.
- Server authority prevents trivial client-side health, reward and damage manipulation.
- Disconnects and reconnects do not corrupt the match.
- Median matchmaking time in the test population is acceptable or bots fill vacant slots.
- Match results are validated before rewards are granted.
- Network simulation tests cover latency, jitter and packet loss.

## 3.4 Suggested product metrics after instrumentation

Do not optimise these before the game is fun, but prepare measurement capability:

- Tutorial completion rate.
- First-match completion rate.
- Match abandonment rate.
- Crash-free sessions.
- ANR-free sessions.
- Average and percentile frame time by device tier.
- Average match duration.
- Time-to-first-damage.
- Time-to-first-elimination.
- Matches per session.
- Day-1 and Day-7 retention.
- Rematch rate.
- Character and gadget pick/win rates.
- Network disconnect rate.
- Matchmaking queue time.
- Percentage of matches filled with bots.
- Player-reported fairness and control satisfaction.

---

# 4. CORE GAME FORMAT

## 4.1 Initial mode: Solo Raja

- Eight total combatants.
- One human and seven bots during the offline milestone.
- Eight human/bot slots during online alpha.
- Last surviving fighter wins.
- Target match duration: 4–6 minutes.
- Compact arena.
- No respawn in the main solo mode.
- Eliminated players enter spectator mode.
- Bots may backfill empty online slots.

## 4.2 Future modes

Not part of the first complete prototype:

- Duo Raja.
- Three-player squads.
- Team elimination.
- Capture-the-standard.
- King of the courtyard.
- Limited-time festival modes.
- Private custom rooms.
- Training and aim practice.

## 4.3 Match phases

1. **Load and warm-up**
   - Assets loaded.
   - Player roster displayed.
   - Input disabled until all critical systems are ready.

2. **Spawn**
   - Fighters spawn in separated safe locations.
   - Brief spawn protection prevents immediate unfair elimination.
   - Camera establishes the arena.

3. **Opening phase**
   - Players collect resources or gadgets.
   - The full arena remains available.
   - Early encounters are possible but not forced.

4. **Pressure phase**
   - Aandhi begins closing.
   - Higher-value gadgets or power resources appear in contested areas.
   - Arena events may modify paths.

5. **Final circle**
   - Small playable zone.
   - Reduced hiding potential.
   - Strong visibility and urgency.

6. **Resolution**
   - Winner declared only by authoritative match state.
   - Results screen shows placement, eliminations, damage and rewards.
   - Rematch and return-to-lobby options.

---

# 5. SIGNATURE SYSTEMS

## 5.1 Jugaad Gadgets

Jugaad Gadgets are temporary match pickups. They create improvisation and distinguish BattleRaja from games based only on permanent character kits.

Initial candidates:

1. **Umbrella Guard**
   - Brief directional shield.
   - Blocks or reduces incoming ranged damage from the facing direction.

2. **Dhol Burst**
   - Radial soundwave.
   - Pushes enemies and interrupts certain channelled actions.

3. **Kite-Line Hook**
   - Short grappling movement to valid terrain or pull toward a point.
   - Must not cross forbidden collision layers.

4. **Tiffin Station**
   - Temporary healing station.
   - Destroyable by enemies.
   - Healing stops or weakens while taking damage.

5. **Chilli Smoke**
   - Creates a visibility-disrupting cloud.
   - Must have consistent gameplay rules independent of graphics quality.

6. **Rickshaw Boost**
   - Short high-speed dash with limited steering.
   - Cannot be used to leave valid navigation space.

7. **Folding Barricade**
   - Deployable temporary cover.
   - Has health, lifetime and placement validation.

8. **Pressure Shock**
   - Triggered proximity trap with readable arming time.
   - Must not resemble instructions for a real harmful device; presentation is fantastical and harmless.

Vertical slice requirement: choose and polish exactly three.

## 5.2 Aandhi closing zone

The Aandhi is the battle royale pressure system.

Requirements:

- Data-driven timing and radius curve.
- Clear world-space boundary.
- Clear minimap/HUD warning.
- Damage or escalating penalty outside the safe region.
- Bots understand and prioritise returning to safety.
- Final zone cannot close into inaccessible terrain.
- Zone state belongs to deterministic/authoritative match simulation.
- Visual effects are presentation only and cannot define gameplay collision.
- The system must support circular zones initially.
- Future maps may add authored zone sequences.

## 5.3 Destructible and dynamic cover

Prototype:

- Destructible crates or walls with health.
- Destruction changes line of sight and navigation.
- Destruction is deterministic and network-replicable.

Future:

- Moving market carts.
- Flooded routes.
- Temporary bridges.
- Doors or gates.
- Arena event hazards.

## 5.4 Readability system

Every fighter must be readable at small screen size:

- Strong silhouette.
- Limited visual noise.
- Team/player colour ring.
- Health display visible only when useful.
- Distinct attack telegraph.
- Consistent danger colours.
- Status indicators.
- Accessible colour alternatives.
- Off-screen danger indicators when justified.
- No critical gameplay signal communicated by colour alone.

---

# 6. COMBAT MODEL

## 6.1 Control scheme

Mobile:

- Left virtual stick: movement.
- Right virtual stick: aim.
- Release or separate attack button configurable after testing.
- Ability button.
- Gadget button.
- Optional auto-aim for basic attacks at accessibility/casual settings.
- Haptic feedback where supported and user-enabled.
- Controls respect safe areas and device aspect ratios.
- Control positions and sizes may be adjustable.

Desktop test controls:

- WASD movement.
- Mouse aim.
- Primary mouse attack.
- Keyboard ability and gadget bindings.
- Controller support may be added after core mobile controls.

## 6.2 Movement

Requirements:

- Responsive acceleration.
- Controlled deceleration.
- No unintentional sliding.
- Collision with world geometry.
- Explicit slope policy.
- No physics instability from dynamic rigidbody interactions.
- Code-driven locomotion rather than root-motion locomotion.
- Character rotation may face aim direction independent of movement direction.
- Movement state is independent of animation.

Initial movement abilities:

- Base run.
- Short dash for selected fighters.
- Knockback.
- Slow.
- Stun/interrupt only if short and highly readable.
- No jump unless a character-specific ability requires it.

## 6.3 Attacks

The architecture must support:

- Hitscan/raycast attacks.
- Projectile attacks.
- Area attacks.
- Melee arcs.
- Persistent zones.
- Chained effects.
- Piercing.
- Bounce.
- Knockback.
- Healing.
- Shields.
- Damage over time.
- Status application.

Initial vertical slice should favour projectile and area attacks because they are readable in a top-down mobile game.

## 6.4 Damage pipeline

The damage pipeline must be centralised.

Suggested stages:

1. Attack request.
2. Input/authority validation.
3. Target eligibility.
4. Hit resolution.
5. Damage calculation.
6. Mitigation/shield.
7. Status effects.
8. Health mutation.
9. Elimination check.
10. Event publication.
11. Presentation feedback.
12. Analytics event.

Do not allow individual weapons to mutate target health directly.

## 6.5 Combat feel

Target feel:

- Immediate input response.
- Short, readable anticipation.
- Strong hit confirmation.
- Controlled camera impulse.
- Brief hit-stop only where it does not damage network simulation or input.
- Directional impact effects.
- Audio with layered transient and body.
- Damage numbers optional and configurable.
- Major abilities have stronger feedback than basic attacks.
- Effects never obscure incoming danger.

## 6.6 Balance principles

- Every fighter has a clear strength, weakness and counterplay.
- No ability should guarantee elimination without prior setup.
- Mobility must trade against damage, durability or control.
- Crowd control should be short.
- Healing should not create endless stalemates.
- Gadgets should offer situational advantage, not automatic victory.
- Balance values must be data-driven.
- Changes should be tracked in a balance changelog.
- Bot performance must not be used as the sole balance evidence.

---

# 7. INITIAL FIGHTER ROSTER

Only three fighters are required for the vertical slice.

## 7.1 Bijli — mobile damage

Fantasy: Fast electrical skirmisher.

- Basic attack: charged electric bolt projectile.
- Ability: rapid directional dash that leaves a brief electric trail.
- Passive candidate: successive accurate hits build a small speed bonus.
- Strengths: mobility, finishing weakened enemies.
- Weaknesses: low durability, punishable after dash.
- Visual silhouette: compact athletic frame, angular lightning motifs.
- Cultural inspiration should remain fictional and avoid direct religious symbolism.

## 7.2 Pehel — tank/grappler

Fantasy: Powerful arena wrestler.

- Basic attack: short-range sweeping strike.
- Ability: short charge into a controlled throw/knockback.
- Passive candidate: brief damage reduction after entering close range.
- Strengths: space control, durability.
- Weaknesses: vulnerable to kiting and ranged pressure.
- Visual silhouette: broad torso, strong arm shape, cloth/sport-inspired elements.

## 7.3 Maya — trickster

Fantasy: Deception and misdirection specialist.

- Basic attack: medium-range illusion shard.
- Ability: creates a decoy that copies movement for a limited time.
- Passive candidate: brief concealment or speed after decoy destruction.
- Strengths: confusing opponents, repositioning.
- Weaknesses: lower direct damage and reliance on timing.
- Visual silhouette: asymmetric cloak/scarf shapes and mirrored motifs.

## 7.4 Future concepts

- Raag: soundwave controller.
- Toofan: wind-based ranged fighter.
- Taraash: engineer and deployable cover specialist.
- Gati: courier/scout.
- Neer: healing rain support.
- A character inspired by coastal navigation.
- A character inspired by craft, textiles or kite traditions.

No future concept is approved for implementation until the first three fighters are fun and distinct.

---

# 8. WORLD AND CULTURAL DIRECTION

## 8.1 Creative approach

BattleRaja should feel inspired by the Indian subcontinent without reducing the region to clichés.

Use:

- Fictional architecture.
- Forts, courtyards, stepwells, bazaars, hill settlements, ports, mangroves and monsoon environments.
- Textile-inspired patterns.
- Vibrant but controlled colour palettes.
- Musical inspiration across percussion, strings and modern electronic sound.
- Everyday objects transformed into playful fantasy.
- Multiple regional influences across the wider subcontinent.

Avoid:

- Combat inside recognisable active sacred sites.
- Using gods or revered figures as damage-dealing playable characters.
- Flattening all regions into one “generic India.”
- Accent-based humour.
- Skin-tone stereotypes.
- Caste, religion, ethnicity or nationality as combat statistics.
- Politically contested maps or symbols unless thoroughly reviewed.
- Copying living communities’ ceremonial dress without context.
- Treating poverty or street congestion as comedy.

## 8.2 Cultural review process

Before public release:

- Maintain a cultural reference log.
- Record origin and intended transformation of major motifs.
- Use at least two sensitivity reviewers familiar with represented regions.
- Review names in relevant languages.
- Check unintended meanings, pronunciation and trademark conflicts.
- Remove or revise content that resembles sacred, politically sensitive or derogatory imagery.

## 8.3 Naming principles

Character and location names must:

- Be easy to pronounce internationally.
- Avoid implying a real caste, religion or ethnic group.
- Be searchable.
- Be checked for offensive meanings in major regional languages.
- Be checked for trademark and store conflicts.
- Support localisation.

**BattleRaja is a working title only.** Conduct legal trademark, domain, app-store and social-handle searches before branding investment.

---

# 9. ARENA DESIGN

## 9.1 First arena: Bazaar Bastion

Theme:

- Fictional fortified market courtyard.
- Colourful awnings.
- Crates, stalls, ramps and central open contest area.
- Small side lanes and breakable shortcuts.
- No direct copy of a real market or sacred architecture.

Gameplay goals:

- Multiple routes.
- No single dominant camping position.
- Strong central risk/reward.
- Line-of-sight breaks.
- Wide enough spaces for mobile aiming.
- Spawn separation.
- Valid final-zone positions.
- Bot navigation reliability.
- Clear map edges.
- Minimal visual occlusion from tall foreground props.

## 9.2 Arena metrics

Initial eight-player grey-box target, subject to playtesting:

- Traversal across the main playable diameter in approximately 15–25 seconds.
- Spawn-to-nearest-enemy time long enough to avoid involuntary immediate fights.
- At least two exits from most major zones.
- Cover density tested against ranged and melee fighters.
- No hidden collision mismatch.
- No narrow passage that consistently traps larger characters.
- Camera must not be blocked by geometry.

## 9.3 Future arenas

- Stepwell Stronghold.
- Monsoon Junction.
- Himalayan Hill Fort.
- Mangrove Maze.
- Coastal Spice Port.

These are concepts, not committed production scope.

---

# 10. ART DIRECTION

## 10.1 Rendering style

- Fully 3D world.
- Fixed or tightly controlled elevated perspective camera.
- Stylised low-poly geometry.
- Painted or simplified PBR materials.
- Controlled gradients.
- Exaggerated proportions.
- Large readable equipment.
- Soft contact shadows.
- Limited realtime lights.
- Strong VFX silhouettes.
- Minimal post-processing.

## 10.2 Camera

Initial target:

- Perspective camera with low field of view or orthographic-like perspective.
- Downward angle approximately 40–55 degrees, determined by testing.
- Smooth follow with bounded damping.
- Look-ahead based on aim only if it does not cause motion discomfort.
- Camera shake through impulse events.
- Spectator target switching.
- Safe framing for device aspect ratios.
- Foreground obstruction handling.

Do not assume orthographic projection is automatically superior. Test orthographic and low-FOV perspective, then record the decision.

## 10.3 Character art constraints

- Readable at gameplay scale.
- Approximately 2–4 primary material groups.
- Shared shader family.
- Reusable humanoid skeleton where practical.
- Texture atlases.
- LOD strategy if profiling proves useful.
- Controlled bone count.
- Consistent pivot, scale and import settings.
- Cosmetics should not change hitbox or silhouette so radically that gameplay becomes confusing.

## 10.4 Animation

Required states:

- Idle.
- Locomotion blend.
- Aim offset.
- Basic attack.
- Ability.
- Gadget.
- Hit reaction.
- Knockback.
- Elimination.
- Victory.
- Defeat.
- Spawn.

Rules:

- Root motion disabled for normal locomotion.
- Gameplay movement remains code-controlled.
- Upper-body aiming may use Animation Rigging or authored additive animation.
- Attack events must not rely solely on animation events for authoritative gameplay.
- Authoritative combat timing comes from simulation data.
- Animation events may trigger presentation effects.
- Blend trees and state machines must be documented.
- Avoid one enormous Animator Controller if modular layers or playable graphs are cleaner.

## 10.5 VFX

- Pool all frequently spawned effects.
- Use mobile-friendly particle counts.
- Limit overdraw.
- Avoid large full-screen transparent effects.
- Scale effect intensity by quality tier.
- Gameplay telegraphs remain visible on all quality tiers.
- Status effects use consistent visual language.
- Flashing content should respect accessibility and photosensitivity concerns.

## 10.6 UI style

- Bold shapes.
- Large touch targets.
- Strong hierarchy.
- Minimal text during combat.
- Indian-subcontinent-inspired patterns used as accents, not clutter.
- Localisation-ready layouts.
- Safe-area support.
- Scalable UI for multiple aspect ratios.
- HUD tested with fingers obscuring lower screen regions.

---

# 11. AUDIO DIRECTION

## 11.1 Goals

- Distinct attack sounds.
- Clear danger and hit confirmation.
- Strong but non-fatiguing percussion.
- Regional instrumentation blended with modern electronic production.
- Original compositions and properly licensed assets.
- Spatial audio where useful.
- Priority system prevents effect overload.

## 11.2 Audio categories

- Music.
- Ambient.
- UI.
- Weapon/attack.
- Ability.
- Impact.
- Voice/exertion.
- Announcer.
- Aandhi warning.
- Victory/defeat.

## 11.3 Technical requirements

- Audio mixer groups.
- User controls for music, effects and voice.
- Compression appropriate for mobile.
- Voice-count limits.
- Pooled or reusable audio sources where useful.
- Audio events decoupled from gameplay state.
- No copyrighted music or unlicensed samples.
- Placeholder audio clearly marked.

---

# 12. ACCESSIBILITY

Initial requirements:

- Adjustable control size.
- Left/right-handed layout exploration.
- Aim assist setting.
- Haptic toggle.
- Music, effects and voice volume.
- Colour-blind-safe indicators.
- Text scaling where practical.
- Critical signals not communicated only through colour.
- Reduced screen shake.
- Reduced flashes.
- High-contrast aiming/target indicators.
- Tutorial replay.
- Input remapping for desktop/controller if supported.
- Localisation-ready strings.
- Clear network and error messaging.

---

# 13. TECHNOLOGY STACK

## 13.1 Core stack

- **Engine:** Unity 6, production-supported release.
- **Language:** C#.
- **Render pipeline:** Universal Render Pipeline.
- **Primary platforms:** Android application and browser-playable Web build.
- **Secondary test platform:** Windows.
- **Version control:** GitHub.
- **Large files:** Git LFS.
- **IDE/agent:** OpenAI Codex app/CLI/IDE integration using Luna 5.6 xhigh or strongest appropriate model.
- **3D:** Blender.
- **UI:** Figma.
- **Testing:** Unity Test Framework, Unity Performance Testing package where appropriate.
- **Build automation:** Unity command-line builds and GitHub Actions or Unity Build Automation, selected after cost/security review.
- **Multiplayer:** Photon Fusion 2, introduced only after offline vertical slice.
- **Player backend:** PlayFab or a reviewed equivalent, introduced only when required.
- **Crash/performance:** Unity/Google Play tooling, Android vitals and Android Performance Tuner if appropriate.

## 13.2 Unity packages

Evaluate current compatible versions from official package documentation before installation.

Expected packages:

- Universal RP.
- Input System.
- Cinemachine.
- AI Navigation.
- Addressables.
- Animation Rigging.
- Unity Test Framework.
- Performance Testing.
- TextMeshPro.
- Photon Fusion SDK at multiplayer milestone.
- PlayFab SDK at account/progression milestone.

Avoid installing packages merely because they might be useful. Each dependency needs an owner, purpose and removal plan.

## 13.3 Why Unity

Unity is selected because:

- Strong Android deployment workflow.
- Suitable stylised 3D rendering.
- C# productivity.
- Mature mobile ecosystem.
- Compatible multiplayer/backend options.
- Command-line build and test support.
- Broad asset and tooling availability.
- Good fit for AI-assisted code generation.

The agent may propose a different engine only before implementation and only with a written comparative analysis covering mobile performance, networking, tooling, development effort and migration risk.

---

# 14. SOFTWARE ARCHITECTURE

## 14.1 Architectural goal

Build a simulation core that can run:

- Offline with one human and bots.
- In automated tests.
- In replay/debug tools.
- Under a network authority adapter.
- Without requiring UI, animations or live backend services.

## 14.2 Layer model

### Domain/simulation layer

Owns:

- Match state.
- Fighter state.
- Health.
- Cooldowns.
- Damage.
- Status effects.
- Zone state.
- Pickups.
- Win conditions.
- Deterministic rules.

Must not depend on:

- Unity UI.
- PlayFab.
- Photon.
- Animator.
- Particle systems.
- Scene-specific object references.

### Application/gameplay orchestration layer

Owns:

- Match lifecycle.
- Input command routing.
- Spawning.
- Bot command generation.
- Simulation stepping.
- Game service coordination.
- Save/load interfaces.

### Unity presentation layer

Owns:

- GameObjects.
- Rendering.
- Animation.
- VFX.
- Audio.
- Camera.
- HUD.
- Touch controls.

### Infrastructure layer

Owns adapters for:

- Photon.
- PlayFab.
- Analytics.
- Persistence.
- Remote config.
- Platform identity.
- Build environment.

## 14.3 Determinism policy

Perfect cross-platform lockstep determinism is not required unless later architecture demands it.

However:

- Gameplay rules must be explicit and testable.
- Randomness must use injectable seeded random sources.
- Simulation code must avoid hidden frame-rate dependencies.
- Fixed-step logic must be clearly separated from render update.
- Network authority must resolve canonical state.
- Replay/debug capture should store commands and important state snapshots where practical.

## 14.4 Dependency rules

- Core simulation cannot reference concrete backend/network SDKs.
- UI observes state through interfaces/events/view models.
- Weapons and abilities do not directly locate arbitrary scene objects.
- Static global mutable state is forbidden except narrowly justified engine infrastructure.
- Service locator use should be limited and documented.
- Prefer constructor or explicit dependency injection in pure C# systems.
- Avoid adding a heavy DI framework unless it solves demonstrated complexity.
- Use assembly definitions to enforce boundaries.

## 14.5 Events

Use strongly typed events or signals.

Distinguish:

- Domain events: damage applied, fighter eliminated, zone phase changed.
- Presentation events: play sound, camera impulse, spawn particle.
- Analytics events: match started, match completed, tutorial step completed.

Event ordering must be testable where it affects gameplay.

## 14.6 Data-driven content

Use ScriptableObjects or validated content data for:

- Fighter definitions.
- Abilities.
- Weapons.
- Gadgets.
- Status effects.
- Match rules.
- Zone curves.
- Bot profiles.
- Audio/VFX references.
- Quality settings.

Runtime mutable state must not be stored in shared ScriptableObject assets.

---

# 15. SUGGESTED REPOSITORY STRUCTURE

```text
BattleRaja/
├── AGENTS.md
├── README.md
├── LICENSE
├── .editorconfig
├── .gitattributes
├── .gitignore
├── Assets/
│   └── BattleRaja/
│       ├── Core/
│       │   ├── Domain/
│       │   ├── Application/
│       │   ├── Events/
│       │   ├── Math/
│       │   ├── Random/
│       │   └── Utilities/
│       ├── Gameplay/
│       │   ├── Fighters/
│       │   ├── Combat/
│       │   ├── Abilities/
│       │   ├── Weapons/
│       │   ├── Gadgets/
│       │   ├── StatusEffects/
│       │   ├── Pickups/
│       │   ├── Zone/
│       │   ├── Match/
│       │   └── Spectating/
│       ├── AI/
│       │   ├── Perception/
│       │   ├── Decisions/
│       │   ├── Navigation/
│       │   ├── Behaviours/
│       │   └── Debugging/
│       ├── Presentation/
│       │   ├── Characters/
│       │   ├── Animation/
│       │   ├── Camera/
│       │   ├── VFX/
│       │   ├── Audio/
│       │   └── UI/
│       ├── Infrastructure/
│       │   ├── Networking/
│       │   ├── Backend/
│       │   ├── Analytics/
│       │   ├── Persistence/
│       │   └── Platform/
│       ├── Content/
│       │   ├── Fighters/
│       │   ├── Abilities/
│       │   ├── Weapons/
│       │   ├── Gadgets/
│       │   ├── Maps/
│       │   └── Balance/
│       ├── Art/
│       ├── Audio/
│       ├── Scenes/
│       │   ├── Bootstrap/
│       │   ├── Frontend/
│       │   ├── Gameplay/
│       │   └── Test/
│       ├── Editor/
│       └── Tests/
│           ├── EditMode/
│           ├── PlayMode/
│           ├── Performance/
│           └── Fixtures/
├── Packages/
├── ProjectSettings/
├── Docs/
│   ├── MASTER_VISION.md
│   ├── GDD.md
│   ├── ARCHITECTURE.md
│   ├── NETWORK_MODEL.md
│   ├── ART_BIBLE.md
│   ├── CULTURAL_GUIDE.md
│   ├── PERFORMANCE_BUDGET.md
│   ├── TEST_STRATEGY.md
│   ├── SECURITY.md
│   ├── DECISIONS.md
│   ├── RESEARCH_LOG.md
│   ├── BALANCE_CHANGELOG.md
│   └── RELEASE_CHECKLIST.md
├── Tools/
│   ├── Build/
│   ├── Validation/
│   └── Content/
└── .github/
    ├── ISSUE_TEMPLATE/
    ├── pull_request_template.md
    └── workflows/
```

---

# 16. BOOTSTRAP AND SCENE STRATEGY

Recommended scenes:

1. **Bootstrap**
   - Composition root.
   - Persistent service initialisation.
   - Configuration validation.
   - Routes to frontend or gameplay.

2. **Frontend**
   - Main menu.
   - Settings.
   - Fighter selection.
   - Match entry.

3. **Gameplay_BazaarBastion**
   - Arena geometry.
   - Spawn points.
   - Navigation.
   - Gameplay presentation.

4. **Test_Combat**
   - Fast isolated combat testing.

5. **Test_AI**
   - Bot observation and debugging.

6. **Test_Performance**
   - Reproducible stress scene.

Avoid a single giant scene containing every system.

---

# 17. BOT AI

## 17.1 Goals

Bots should:

- Allow full offline matches.
- Fill online vacancies.
- Teach game rules indirectly.
- Create believable pressure.
- Avoid obviously cheating.
- Be tunable by difficulty/profile.

## 17.2 Bot capabilities

- Perceive visible enemies.
- Estimate threat.
- Choose target.
- Move toward safe zone.
- Seek cover.
- Attack within effective range.
- Retreat when disadvantaged.
- Collect useful gadgets.
- Avoid known hazards.
- Use ability and gadget with rules.
- Avoid oscillation and navigation deadlocks.
- Handle destroyed cover and changed routes.
- Respect reaction-time limits.
- Have imperfect aim.

## 17.3 AI architecture

Start with utility AI or a hierarchical state machine.

Suggested high-level states:

- Explore.
- Loot.
- Engage.
- Reposition.
- Retreat.
- Seek safety.
- Heal.
- Final-circle aggression.

Decision inputs:

- Distance to safe zone.
- Current health.
- Enemy health estimate.
- Weapon range.
- Cooldowns.
- Nearby cover.
- Number of threats.
- Available gadget.
- Recent damage source.
- Time remaining.

Do not begin with machine learning or reinforcement learning.

## 17.4 Bot fairness

Bots must not:

- Know hidden players without perception.
- Aim with impossible precision.
- React instantly.
- see through smoke unless their kit permits it.
- Ignore cooldowns.
- receive hidden damage multipliers solely to fake difficulty.

Difficulty should alter:

- Reaction delay.
- Aim noise.
- tactical weighting.
- aggression.
- prediction quality.
- gadget usage.

---

# 18. NETWORKING ARCHITECTURE

Networking is explicitly deferred until the offline core and vertical slice are stable.

## 18.1 Networking goals

- Responsive local input.
- Authoritative canonical results.
- Tolerance of mobile latency and jitter.
- Reconnection where feasible.
- Secure reward flow.
- Reusable offline gameplay logic.
- Clear diagnostics.

## 18.2 Recommended Photon Fusion progression

### Stage A — local offline

- No Photon dependency in domain systems.
- Human and bots generate the same command types.
- Match runs entirely locally.

### Stage B — two-client laboratory

- Private room.
- One arena.
- One fighter.
- Movement.
- One projectile.
- Damage.
- Elimination.
- Simulated latency.

### Stage C — host/shared prototype

Use only to validate integration quickly if appropriate.

### Stage D — authoritative public architecture

For ranked/public competitive matches:

- Dedicated authoritative server or equivalent trusted authority.
- Clients submit inputs.
- Server validates movement, attacks, damage, pickups, zone and results.
- Client prediction for local responsiveness.
- Reconciliation for mismatches.
- Snapshot interpolation for remote entities.
- Lag compensation for eligible attacks.
- Server submits match result/reward claims.

## 18.3 Tick and render targets

Initial hypothesis:

- Simulation/network tick around 30 Hz.
- Rendering target 60 FPS.
- Low-power rendering option 30 FPS.

These are starting points, not immutable requirements. Profile bandwidth, CPU and feel.

## 18.4 Replication

Replicate only required state.

Potential replicated state:

- Position/rotation.
- velocity or movement state where necessary.
- health/shield.
- action state.
- cooldown-relevant state.
- active status effects.
- projectile state.
- pickup availability.
- Aandhi state.
- match phase.
- elimination/placement.

Do not replicate cosmetic-only transient details if they can be reconstructed.

## 18.5 Anti-cheat baseline

- Never trust client-reported damage.
- Never trust client-reported rewards.
- Validate movement speed and ability constraints.
- Validate fire rate and cooldown.
- Validate pickup distance and availability.
- Sign or authenticate server-to-backend result submission.
- Use rate limiting.
- Avoid shipping PlayFab secret keys in clients.
- Log suspicious discrepancies.
- Do not claim the game is “cheat-proof.”

## 18.6 Reconnection

Define:

- Grace period.
- Bot takeover behaviour.
- Resume eligibility.
- State snapshot transfer.
- Reward rules for disconnected players.
- Abuse prevention.

---

# 19. BACKEND ARCHITECTURE

Backend integration is not required for the first offline prototype.

## 19.1 Proposed PlayFab responsibilities

- Player identity.
- Guest login.
- Google Play account linking.
- Profile.
- Inventory.
- Cosmetic ownership.
- Currency.
- Progression.
- Remote configuration.
- Statistics.
- Leaderboards.
- Match history summary.
- Server-side reward validation.
- Cloud functions or server logic where required.

## 19.2 Responsibilities not assigned to PlayFab by default

- Moment-to-moment combat simulation.
- Rendering.
- Client input.
- Local animation.
- Photon realtime state unless deliberately integrated.

## 19.3 Save policy

Local prototype:

- Versioned local settings.
- Optional local progression for testing.

Online product:

- Server-owned valuable progression.
- Cached local display data.
- Conflict strategy.
- Migration strategy.
- Idempotent reward grants.
- Never overwrite newer cloud progress with stale local data.

## 19.4 Economy principles

Currencies should be minimal.

Potential:

- Soft currency earned by play.
- Premium currency purchased.
- Seasonal progression points.

Rules:

- No purchased statistical advantage.
- No loot boxes at initial launch.
- Transparent pricing.
- Age-appropriate purchase controls.
- Server validation.
- Auditability.
- Regional price review.
- Legal and store-policy review before implementation.

---

# 20. WEB PLATFORM REQUIREMENTS

BattleRaja must be playable directly from the official website using a Unity Web build, while also shipping as an Android application. Both builds must share the same gameplay rules and content model.

## 20.1 Product intent

- A visitor should be able to open the website and play without installing a desktop executable.
- The browser version is not a marketing demo or a separate JavaScript remake.
- Android and Web should share fighters, maps, balance, matchmaking, accounts and progression where services permit.
- Android–Web cross-play is a target for the online alpha.
- Modern 64-bit desktop browsers are the first Web target.
- Mobile-browser support is desirable but must remain experimental until memory, loading, thermal and touch behaviour are proven on devices.

## 20.2 Browser support direction

Milestone 0 must research and record a support matrix for current versions of:

- Chrome and Chromium-based Edge.
- Firefox.
- Safari on macOS where practical.
- Android Chrome and iOS Safari as experimental mobile-Web targets.

Unsupported browsers must receive a clear compatibility message instead of a broken canvas.

## 20.3 One shared Unity project

- Use one Unity project with separate Android and Web build profiles.
- Domain/gameplay code must not contain Android- or browser-specific assumptions.
- Platform capabilities must be behind interfaces/adapters, including identity, local storage, haptics, fullscreen, external URLs, deep links, purchases, notifications, browser lifecycle and analytics.
- Platform compilation symbols may be used only in infrastructure/adapters or narrowly documented presentation code.
- Maintain separate Android and Web smoke-build pipelines.
- Shared content IDs, protocol versions and balance data must match unless an approved exception is recorded.

## 20.4 Web input

Desktop browser:

- Keyboard movement and mouse aiming.
- Mouse attack/ability controls.
- Controller support if verified.
- Fullscreen/pointer capture where appropriate.
- Prevent browser scrolling or context-menu interference only while the game canvas owns focus.
- Provide a clear way to release cursor focus.

Mobile browser:

- Reuse safe-area-aware twin-stick touch controls.
- Account for changing browser chrome and viewport size.
- Handle orientation and focus changes safely.
- Do not assume haptics or fullscreen availability.

Unity Web does not fully support physical-to-active keyboard layout mapping in the Input System, so non-English keyboard layouts must be tested.

## 20.5 Web performance and download budget

Create independent Web budgets for:

- Initial compressed download.
- Time to first interaction.
- Time to first playable match.
- WebAssembly memory.
- Browser cache behaviour.
- Shader warm-up.
- Draw calls and overdraw.
- Texture/audio payload.
- Addressables and deferred content.
- Repeat-visit loading.

Requirements:

- Keep the initial shell and first-match content small.
- Defer nonessential cosmetics and maps.
- Use batching, instancing and atlases.
- Avoid assumptions about managed threading.
- Avoid unsupported dynamic-code generation.
- Configure correct compression, MIME and cache headers.
- Show real loading progress and recoverable errors.
- Profile Web independently from Android.

## 20.6 Browser lifecycle

Browsers may heavily throttle or suspend background tabs.

Therefore:

- A browser client must never be the trusted authority for a public competitive match.
- A hidden tab must not stall canonical match simulation.
- Detect focus/visibility changes and provide reconnect/resume UX.
- Define abandonment and bot-takeover behaviour.
- Respect browser audio autoplay/user-gesture rules.
- Do not assume unrestricted filesystem access.

## 20.7 Web networking and cross-play

Browser security prevents ordinary native socket access.

- Use a multiplayer SDK/transport explicitly supporting Unity Web/WebGL.
- Photon Fusion currently lists WebGL as supported and publishes a Fusion WebGL sample; exact SDK version, topology and limitations must be re-verified when networking begins.
- Public matches should use trusted dedicated/server authority or another reviewed authoritative topology.
- Do not use the browser player as the authoritative host for ranked/public play.
- Test restrictive networks, refresh, tab suspension, reconnect, latency, jitter and packet loss.
- Android and Web clients must pass protocol/content-version compatibility checks before joining the same match.

## 20.8 Identity and cross-progression

- Web players may begin with a guest identity.
- Valuable progression requires a robust account-linking path.
- Browser data can be cleared; local browser identity must not be the only recovery mechanism.
- Link Web guest progress to the same persistent account used by Android.
- Prevent accidental overwrite during account linking.
- Never store trusted secrets in JavaScript, WebAssembly or downloadable static files.
- Validate purchases, entitlements, rewards and match results on trusted backend services.

## 20.9 Hosting and website shell

The website shell should provide:

- Responsive game container.
- Loading and error UI outside the Unity canvas.
- Compatibility checks.
- Privacy, terms and support links.
- Build/version display.
- Maintenance-message capability.
- HTTPS.
- Correct compression, MIME and cache headers.
- CDN/cache invalidation strategy.
- Optional Android-app call to action.

Hosting provider selection is an open decision. Compare maximum asset size, bandwidth pricing, geographic delivery, compression/header support, cache invalidation and deployment automation.

## 20.10 Web validation

Later test coverage must include:

- Chrome, Edge and Firefox desktop.
- Safari where available.
- Multiple resolutions and device-pixel ratios.
- Keyboard layouts, mouse, controller and touch.
- Pointer focus and fullscreen.
- Browser refresh and background-tab suspension.
- Cleared browser storage.
- Slow first load and cached repeat load.
- CDN/header validation.
- Android–Web cross-play.
- Memory growth across repeated matches.

# 21. ANDROID REQUIREMENTS

## 20.1 Platform target

The project must research current Google Play target API requirements at release time.

As of the research date, Google Play documentation indicates new apps and updates will need to target Android 16 / API level 36 from 31 August 2026, with policy details subject to change. The build pipeline must not hard-code assumptions without checking official Play Console documentation before submission.

## 20.2 Device tiers

Suggested test matrix:

### Low tier

- 4 GB RAM.
- Older or entry-level GPU.
- 30 FPS target.
- Reduced shadows/effects.

### Mid tier

- 6–8 GB RAM.
- Main target class.
- Stable 60 FPS where feasible.

### High tier

- 8+ GB RAM.
- 60/90 FPS experimentation only after stability.
- Higher effects without competitive advantage.

The developer’s OnePlus 11R is a useful high-tier device but is not the minimum target.

## 20.3 Performance budgets

Initial hypotheses to validate:

- 60 FPS: 16.67 ms total frame time.
- 30 FPS fallback: 33.33 ms.
- Avoid sustained thermal throttling.
- Avoid frequent garbage collection spikes.
- Keep scene load and match start practical on mid-range storage.
- Keep memory comfortably below OS pressure thresholds.
- Use adaptive quality where useful.

Create `/Docs/PERFORMANCE_BUDGET.md` with measured budgets for:

- Main thread.
- Render thread.
- GPU.
- GC allocations.
- Draw calls.
- triangles/vertices.
- texture memory.
- mesh memory.
- audio memory.
- peak total memory.
- network bandwidth.
- scene load time.
- APK/AAB and asset-delivery size.

## 20.4 Android settings

Evaluate and document:

- IL2CPP.
- ARM64.
- Vulkan and OpenGL ES fallback strategy.
- Optimised frame pacing.
- Application target frame rate.
- Managed stripping.
- texture compression formats.
- split application binary / asset delivery.
- Android App Bundle.
- min/target SDK.
- internet and notification permissions only when required.
- orientation policy.
- display cutouts and safe areas.
- game mode support.
- Play Integrity if justified later.

## 20.5 Performance tooling

Use:

- Unity Profiler.
- Profile Analyzer.
- Memory Profiler.
- Frame Debugger.
- Android GPU Inspector where compatible.
- Perfetto.
- `adb`.
- Android vitals.
- Android Performance Tuner if integrated.
- Physical-device thermal and battery testing.

---

# 22. TEST STRATEGY

## 21.1 Test pyramid

### Pure/EditMode tests

Test:

- Damage calculation.
- cooldowns.
- status effects.
- seeded randomness.
- zone timing.
- match state transitions.
- placement.
- reward calculation.
- bot utility scoring.
- content validation.

### PlayMode tests

Test:

- spawning.
- movement integration.
- projectile collision.
- arena boundaries.
- elimination presentation.
- input routing.
- scene bootstrap.
- pooled objects.
- navigation.
- spectator switching.

### Integration tests

Test:

- Photon adapter.
- PlayFab adapter.
- persistence.
- build configuration.
- account linking.
- server result flow.

### Performance tests

- Eight fighters under maximum common VFX.
- Projectile stress.
- destruction stress.
- bot decision stress.
- final-circle combat.
- scene loading.
- memory growth over repeated matches.

### Device/manual tests

- Touch feel.
- aspect ratios.
- safe areas.
- thermal throttling.
- interruptions.
- incoming calls/app backgrounding.
- network switching.
- low battery.
- offline/online recovery.

## 21.2 Definition of done for every feature

A feature is done only when:

- Acceptance criteria pass.
- Project compiles without new unexplained errors.
- Relevant automated tests pass.
- Test scene or reproduction steps exist.
- No secrets are introduced.
- Documentation is updated.
- Performance impact is considered.
- Android compatibility is considered.
- Changed files are reported.
- Known limitations are recorded.
- Human playtesting is requested for feel-dependent changes.

## 21.3 Regression rule

Every fixed bug should receive a regression test when practical.

---

# 23. SECURITY AND PRIVACY

Create `/Docs/SECURITY.md`.

Requirements:

- No secret keys in repository or client builds.
- Use environment/CI secrets.
- Validate backend requests.
- Minimise personal data.
- Do not collect contacts, precise location, microphone or files without a product requirement and explicit consent.
- Document analytics events and retention.
- Provide deletion/account policies before launch.
- Review SDK data collection for Google Play Data Safety.
- Review child-safety implications if targeting minors.
- Pin or review dependencies.
- Record third-party licences.
- Threat-model account, economy and match-result abuse.
- Do not log authentication tokens.
- Redact sensitive data from diagnostics.

---

# 24. ANALYTICS

Analytics must support product learning without invasive collection.

Initial events:

- app_open.
- tutorial_started.
- tutorial_step_completed.
- tutorial_completed.
- match_queue_entered.
- match_started.
- match_completed.
- match_abandoned.
- fighter_selected.
- gadget_picked.
- ability_used.
- elimination.
- player_eliminated.
- settings_changed.
- crash context.
- performance quality tier.
- network disconnect/reconnect.

Rules:

- Use stable event schemas.
- Version events.
- Avoid sending raw free text.
- Avoid unnecessary personal identifiers.
- Batch where appropriate.
- Support analytics disablement where legally required.
- Create a data dictionary.

---

# 25. LOCALISATION

Initial language:

- English.

Future candidates:

- Hindi.
- Marathi.
- Gujarati.
- Bengali.
- Tamil.
- Telugu.
- Kannada.
- Malayalam.
- Punjabi.
- Urdu.
- Nepali.
- International languages based on traction.

Requirements:

- All player-facing strings externalised.
- Do not concatenate translated sentence fragments.
- Support variable text expansion.
- Test fonts and scripts.
- Keep character names transliteration-friendly.
- Use culturally competent translators rather than raw machine output for release.
- Voice localisation is not required initially.

---

# 26. MONETISATION AND PROGRESSION

Not part of grey-box prototype.

## 25.1 Allowed launch direction

- Cosmetic skins.
- Emotes.
- victory poses.
- profile banners.
- effect variants.
- optional season pass.
- optional rewarded ads only after careful UX review.

## 25.2 Prohibited direction

- Paid combat stats.
- purchased damage/health advantage.
- forced ads during active matches.
- deceptive pricing.
- dark-pattern purchase flows.
- exploitative loot boxes.
- energy system blocking ordinary play at launch.

## 25.3 Progression

Progression may unlock:

- Fighters through fair play.
- cosmetics.
- mastery badges.
- profile customisation.
- quests.

Base competitive stats should remain standardised.

---

# 27. DEVELOPMENT MILESTONES

## Milestone 0 — Repository and research foundation

Deliverables:

- Unity version decision.
- repository.
- Git LFS.
- root `AGENTS.md`.
- package manifest.
- documentation skeleton.
- architecture decision records.
- official-source research log.
- Windows editor compile.
- Android empty-scene build.
- CI feasibility assessment.

Exit criteria:

- Clean project opens.
- Empty Android build launches.
- Test framework runs.
- No untracked generated directories.

## Milestone 1 — Movement laboratory

Deliverables:

- Grey-box arena.
- top-down camera.
- desktop input.
- touch input.
- one capsule fighter.
- collision.
- aim indicator.
- settings for sensitivity.
- movement tests.

Exit criteria:

- Movement feels responsive on device.
- No frame-dependent speed.
- camera never loses player in normal arena.

## Milestone 2 — Combat laboratory

Deliverables:

- health.
- damage pipeline.
- one projectile weapon.
- training dummy.
- hit feedback.
- cooldown.
- object pooling.
- combat tests.

Exit criteria:

- Damage is centralised.
- projectile behaviour is deterministic enough for later networking.
- repeated combat does not leak pooled objects.

## Milestone 3 — One complete fighter

Deliverables:

- Bijli grey-box kit.
- basic attack.
- dash.
- cooldown UI.
- status effects.
- bot-usable command API.

Exit criteria:

- Human and bot invoke the same gameplay command layer.
- ability timing does not depend on animation event.

## Milestone 4 — Bots

Deliverables:

- navigation.
- perception.
- engage/retreat/safety decisions.
- imperfect aiming.
- debug visualisation.
- seven-bot stress test.

Exit criteria:

- Bots complete matches.
- no common deadlock.
- bots react to Aandhi.

## Milestone 5 — Offline battle royale

Deliverables:

- spawn.
- eight fighters.
- pickups.
- Aandhi.
- elimination.
- spectator.
- placement.
- results.
- restart.

Exit criteria:

- complete 4–6 minute match on Android.
- 20 consecutive automated/simulated matches finish without blocker.
- no critical memory growth across repeated matches.

## Milestone 6 — Jugaad systems

Deliverables:

- three gadgets.
- spawn/rarity rules.
- pickup feedback.
- bot usage.
- counters.
- tests.

Exit criteria:

- each gadget has a distinct tactical use.
- no gadget dominates all situations.

## Milestone 7 — Three-fighter vertical slice

Deliverables:

- Bijli.
- Pehel.
- Maya.
- balance data.
- one polished map.
- animation.
- VFX.
- audio.
- polished HUD.
- tutorial.

Exit criteria:

- identifiable original visual identity.
- external playtest.
- performance within target on test tiers.

## Milestone 8 — Two-client networking proof

Deliverables:

- Photon Fusion isolated adapter.
- two-player room.
- movement.
- attacks.
- damage.
- elimination.
- latency simulation.
- diagnostic overlay.

Exit criteria:

- offline mode remains functional.
- no direct Photon dependency in domain layer.
- documented authority model.

## Milestone 9 — Online eight-slot alpha

Deliverables:

- matchmaking/private room.
- bots fill vacancies.
- authoritative match.
- disconnect handling.
- spectating.
- match result.
- network stress tests.

Exit criteria:

- realistic latency tests.
- no trivial client-side reward mutation.
- stable match completion.

## Milestone 10 — Account and progression

Deliverables:

- PlayFab identity.
- account linking.
- profile.
- inventory.
- progression.
- leaderboards.
- validated rewards.
- remote config.

Exit criteria:

- no secrets in client.
- idempotent rewards.
- account recovery/linking tests.

## Milestone 11 — Closed Android test

Deliverables:

- onboarding.
- consent/policies.
- analytics.
- crash reporting.
- store assets.
- device matrix.
- accessibility pass.
- localisation readiness.
- closed test release.

Exit criteria:

- Play policy checklist.
- crash-free target.
- severe bugs resolved.
- data safety declarations verified.

---

# 28. CODEX / LUNA EXECUTION WORKFLOW

## 27.1 Required first pass

The agent must not immediately implement gameplay.

It must first:

1. Inspect repository and local environment.
2. Read this file.
3. Research current official documentation.
4. Produce:
   - `/Docs/RESEARCH_LOG.md`
   - `/Docs/DECISIONS.md`
   - `/Docs/ARCHITECTURE.md`
   - `/Docs/PERFORMANCE_BUDGET.md`
   - `/Docs/TEST_STRATEGY.md`
   - root `AGENTS.md`
5. Propose Milestone 0 issues.
6. Identify unavailable dependencies or credentials.
7. Build an empty Android validation project.
8. Report findings.

## 27.2 Task size

Each implementation task should usually:

- Touch one subsystem.
- Have measurable acceptance criteria.
- Include tests.
- Avoid broad unrelated refactors.
- Be reviewable in one diff.

Bad task:

> Build the complete multiplayer battle royale.

Good task:

> Implement BR-COMBAT-003: pooled straight-line projectiles with collision filtering and central damage requests. Add EditMode tests for projectile configuration validation and PlayMode tests for hit/despawn behaviour.

## 27.3 Planning protocol

Before non-trivial work:

- State current understanding.
- inspect existing code.
- identify affected boundaries.
- list changed files expected.
- list tests.
- identify risks.
- implement.
- run tests/build.
- summarise actual changes and deviations.

## 27.4 Model delegation

Suggested:

- Luna 5.6 xhigh / strongest reasoning model:
  - architecture.
  - multiplayer.
  - difficult debugging.
  - security.
  - system-wide refactors.
  - milestone planning.
- Faster/cheaper model:
  - repetitive tests.
  - simple editor tooling.
  - documentation formatting.
  - isolated UI wiring.
  - renames.
  - content data entry.

The exact model names and pricing may change. The agent should use available model capabilities rather than embedding assumptions in code.

## 27.5 Subagent roles

Use parallel agents only with non-overlapping ownership.

Possible roles:

- Gameplay architect.
- combat engineer.
- bot AI engineer.
- networking engineer.
- Android performance engineer.
- QA/test engineer.
- build/release engineer.
- documentation/research agent.
- UI/accessibility reviewer.
- security reviewer.

One coordinating agent owns integration.

## 27.6 Human checkpoints

Human approval is required after:

- Engine/version/package selection.
- architecture foundation.
- control prototype.
- first complete match.
- fighter kit decisions.
- art-direction sample.
- networking authority design.
- backend/economy design.
- store submission.
- purchases or paid infrastructure.

---

# 29. REQUIRED ROOT AGENTS.MD CONTENT

The agent should generate a refined version, but it must include these principles:

```md
# BattleRaja Agent Rules

## Product
BattleRaja is an Android-first stylised top-down 3D micro battle royale.
Read Docs/MASTER_VISION.md before significant work.

## Current Scope
Work only on the active milestone. Do not prebuild live-service systems.

## Architecture
- Keep the domain simulation independent of Unity UI, Photon and PlayFab.
- Human input and bot decisions must produce common gameplay commands.
- Use data-driven fighter, weapon, gadget and match definitions.
- Do not store runtime mutable state in ScriptableObject assets.
- Avoid global mutable singletons.
- Use seeded injectable randomness for gameplay decisions.
- Separate simulation tick from rendering.

## Unity
- Use the approved Unity 6 version and URP.
- Do not update packages without a decision record.
- Do not blindly hand-edit large scene or prefab YAML.
- Use editor scripts or safe Unity tooling for structural scene changes.
- Do not commit Library, Temp, Logs, builds or secrets.

## Performance
- Android and browser Web are primary release targets.
- Avoid per-frame allocations in hot gameplay paths.
- Pool projectiles, VFX and repeated transient objects.
- Profile before claiming optimisation.
- Preserve critical telegraphs across quality levels.

## Networking
- No Photon dependency in core domain code.
- Never trust client-reported damage, cooldowns, pickups, rewards or results.
- Public competitive matches require trusted authority.

## Testing
- Add tests for rules and regression fixes.
- Run EditMode and PlayMode tests relevant to changes.
- Compile and run a build before milestone completion.
- Report test results and warnings honestly.

## Git
- One focused feature per branch/PR.
- Small descriptive commits.
- Do not mix generated art binaries with unrelated code changes.
- Update documentation and decision records.

## Safety
- Never expose or commit credentials.
- Never publish, purchase, delete production data or accept legal terms without human approval.
```

---

# 30. CUSTOM CODEX SKILLS TO CREATE

After the repository is stable, create and test project skills:

## `battleraja-feature`

Purpose:

- Inspect requirements.
- create branch/task plan.
- implement one gameplay feature.
- add tests.
- run validation.
- produce review summary.

## `unity-android-build`

Purpose:

- Validate Unity path/version.
- run tests.
- build development APK.
- install via ADB.
- launch.
- collect logcat.
- capture artifact paths.

## `battleraja-performance-audit`

Purpose:

- Reproduce stress scene.
- collect Unity/device evidence.
- compare against budget.
- identify CPU/GPU/memory bottlenecks.
- avoid speculative optimisation.

## `photon-authority-review`

Purpose:

- Trace every networked state mutation.
- confirm authority.
- identify client-trust vulnerabilities.
- test latency and packet-loss cases.

## `content-validator`

Purpose:

- Validate ScriptableObject IDs.
- missing references.
- duplicate IDs.
- invalid cooldown/range values.
- asset naming.
- localisation keys.
- addressable groups.

## `release-candidate-check`

Purpose:

- Tests.
- build.
- package.
- permissions.
- secrets.
- licences.
- policy checklist.
- crash/performance evidence.
- known issues.

---

# 31. TOOLING AND CONNECTORS

## Essential

### GitHub

Use for:

- source control.
- issues.
- pull requests.
- review.
- CI.
- releases.
- project board.

### Local shell / Codex CLI

Use for:

- repository inspection.
- build scripts.
- tests.
- asset validation.
- Git operations.
- ADB and logcat.

### Unity Editor

Use for:

- scenes.
- prefabs.
- package management.
- profiling.
- player builds.
- animation and content setup.

### Android SDK / ADB

Use for:

- installing builds.
- launching.
- logs.
- screenshots.
- device properties.
- performance traces.

## Recommended

### Figma connector/MCP

Use for:

- HUD.
- menus.
- fighter selection.
- settings.
- progression.
- design tokens.
- handoff.

### Unity MCP or controlled editor bridge

May be used to:

- inspect scenes.
- create GameObjects.
- configure components.
- run tests.
- capture console errors.
- manipulate prefabs safely.

Requirements:

- Community tools must be reviewed.
- Restrict permissions.
- Commit before large automated scene operations.
- Do not grant unnecessary filesystem/network access.

### Blender automation

Use for:

- import/export validation.
- batch naming.
- scale and orientation checks.
- simple placeholder generation.
- rig validation.

Do not expect AI automation alone to replace artistic review.

## Optional later

- PlayFab connector or SDK tooling.
- Photon dashboard/automation.
- analytics dashboard.
- crash reporting.
- localisation platform.
- asset inventory/licence tracker.

---

# 32. CI/CD

Initial CI should:

- Validate formatting where configured.
- compile assemblies.
- run EditMode tests.
- run selected PlayMode tests.
- validate content.
- check forbidden files/secrets.
- archive test results.

Later CI may:

- Build Windows test player.
- Build Android development APK/AAB.
- run performance smoke tests.
- create release notes.
- upload internal artifacts.
- deploy dedicated server only after explicit approval.

Unity licensing and CI cost must be reviewed before selecting hosted runners.

---

# 33. CONTENT PIPELINE

## 32.1 Naming

Adopt consistent conventions:

- `CHR_Bijli_Model`
- `CHR_Bijli_Animator`
- `ABL_Bijli_Dash`
- `WPN_Bijli_Bolt`
- `GDT_UmbrellaGuard`
- `MAP_BazaarBastion`
- `VFX_Hit_Electric_Small`
- `SFX_Bijli_Attack_01`
- `UI_Icon_Ability_BijliDash`

Exact convention may be refined and documented.

## 32.2 Asset validation

Automated validation for:

- scale.
- orientation.
- missing materials.
- texture sizes.
- compression.
- read/write flags.
- duplicate GUID-like content IDs.
- animator references.
- addressable labels.
- LOD.
- collider layers.
- audio import settings.

## 32.3 Third-party assets

For every asset:

- Source URL/vendor.
- licence.
- purchase account.
- permitted platforms.
- attribution requirement.
- modification rights.
- AI training/use restrictions if relevant.
- proof of licence.

Do not commit paid source assets to a public repository.

---

# 34. LAYERS, TAGS AND COLLISION POLICY

Create documented project layers such as:

- WorldStatic.
- WorldDestructible.
- Fighter.
- FighterHitbox.
- Projectile.
- Pickup.
- Gadget.
- Zone.
- VFXNoCollision.
- NavigationObstacle.
- UI.

Define a collision matrix. Do not rely on string tags in hot paths where typed components or layers are more reliable.

---

# 35. ERROR HANDLING AND DIAGNOSTICS

Requirements:

- Structured logging categories.
- Development-only debug overlays.
- Match ID.
- player/session ID redacted or pseudonymous.
- network statistics.
- simulation tick.
- FPS/frame time.
- bot state display.
- zone state.
- object pool counts.
- clear assertions for invalid content in development.
- graceful player-facing errors in release.

Do not spam logs per frame.

---

# 36. PERFORMANCE-SENSITIVE IMPLEMENTATION RULES

- Avoid LINQ in measured hot loops unless profiling shows no issue.
- Avoid unnecessary `GetComponent` per frame.
- Cache stable references.
- Pool transient objects.
- Use non-allocating physics queries where useful.
- Avoid uncontrolled coroutines for core simulation timing.
- Avoid one `Update` per trivial object where central systems are measurably better.
- Do not use ECS/DOTS merely for fashion; adopt only after measured need.
- Avoid excessive transparent UI and particles.
- Atlas textures and sprites.
- Limit realtime shadows.
- Prefer baked/static lighting where possible.
- Use quality tiers.
- Profile actual devices.

---

# 37. UI FLOW

Initial vertical-slice flow:

1. Splash/bootstrap.
2. Main menu.
3. Settings.
4. Fighter selection.
5. Play.
6. Loading.
7. Match.
8. Results.
9. Rematch or menu.

Online later:

1. Account state.
2. lobby.
3. matchmaking.
4. reconnect handling.
5. progression/rewards.

Menus should be functional before decorative.

---

# 38. TUTORIAL

Tutorial goals:

- Move.
- aim.
- basic attack.
- ability.
- pick up/use gadget.
- understand Aandhi.
- eliminate bot.
- recognise victory.

Rules:

- Under approximately 3 minutes.
- Minimal text.
- guided interaction.
- skippable after first completion.
- replayable.
- analytics events.
- no forced account creation before experiencing gameplay unless platform requirements demand it.

---

# 39. LEGAL AND STORE READINESS

Before release:

- Trademark search for BattleRaja and major character names.
- Privacy policy.
- Terms as required.
- Data Safety form.
- content rating.
- ads declaration.
- target audience declaration.
- third-party SDK disclosures.
- export/compliance review where relevant.
- copyright and licence inventory.
- store screenshots and truthful descriptions.
- current target API compliance.
- testing-track requirements for the developer account.
- account deletion flow if accounts are created.
- support contact.

This document is not legal advice. Use qualified legal review for final decisions.

---

# 40. KEY RISKS AND MITIGATIONS

## Risk: Scope explosion

Mitigation:

- Three fighters.
- one map.
- one mode.
- three gadgets.
- offline first.
- milestone gates.

## Risk: Game is technically functional but not fun

Mitigation:

- Playtest every milestone.
- grey-box before art.
- instrument match flow.
- prioritise feel tuning.
- cut systems that do not improve decisions.

## Risk: Multiplayer complexity

Mitigation:

- network-independent domain.
- two-client proof.
- clear authority.
- latency simulation.
- server validation.

## Risk: Poor Android performance

Mitigation:

- performance budget.
- device tiers.
- stress scene.
- early physical-device builds.
- limited lights/overdraw.
- quality settings.

## Risk: Inconsistent AI-generated code

Mitigation:

- AGENTS.md.
- architecture tests.
- small tasks.
- code review.
- decision records.
- integration owner.
- no parallel edits to shared systems.

## Risk: Weak or insensitive cultural representation

Mitigation:

- fictionalisation.
- cultural guide.
- source log.
- sensitivity review.
- avoid sacred/political misuse.

## Risk: Expensive backend before traction

Mitigation:

- local prototype.
- bots.
- staged services.
- cost monitoring.
- usage caps and alerts.

## Risk: Asset/licence problems

Mitigation:

- licence registry.
- provenance.
- no unknown downloaded assets.
- public/private repository separation.

---

# 41. OPEN DECISIONS

The agent must not silently decide these without recording rationale:

1. Exact Unity 6 release.
2. Perspective versus orthographic-like camera.
3. Character controller implementation.
4. Input attack behaviour: release-to-fire versus separate button.
5. Initial three Jugaad Gadgets.
6. Physics versus custom kinematic projectiles.
7. Photon topology for alpha.
8. Dedicated server hosting provider.
9. PlayFab versus alternative backend.
10. Analytics provider.
11. Minimum supported Android version.
12. Vulkan/OpenGL strategy.
13. Art production method and asset budget.
14. Final title and trademark.
15. Monetisation timing.
16. Matchmaking regions.
17. Bot replacement/reconnection policy.
18. Exact target device performance floor.

---

# 42. RESEARCH REQUIREMENTS FOR THE AGENT

Before implementing each major system, consult current primary sources.

Research topics:

- Current Unity 6 production/LTS recommendations.
- Package compatibility.
- Android target API and Play policies.
- Photon Fusion current topology, pricing and dedicated server guidance.
- PlayFab current identity/economy/leaderboard/server APIs and pricing.
- Codex current AGENTS.md, skills, MCP and subagent practices.
- Google Play testing requirements.
- Android performance and memory guidance.
- Relevant third-party licences.

Use official documentation wherever possible.

Record:

- Date.
- source.
- claim.
- decision impact.
- uncertainty.
- follow-up date.

---

# 43. INITIAL OFFICIAL RESEARCH SOURCES

These are starting references, not substitutes for fresh research.

## OpenAI / Codex

- https://developers.openai.com/codex/agent-configuration/agents-md
- https://developers.openai.com/codex/build-skills
- https://developers.openai.com/codex/subagents
- https://developers.openai.com/codex/learn/best-practices
- https://github.com/openai/codex
- https://openai.com/index/introducing-the-codex-app/

## Unity

- https://unity.com/releases/unity-6
- https://docs.unity3d.com/6000.3/Documentation/Manual/urp/creating-a-new-project-with-urp.html
- https://docs.unity3d.com/6000.0/Documentation/Manual/com.unity.inputsystem.html
- https://docs.unity3d.com/6000.0/Documentation/Manual/com.unity.cinemachine.html
- https://docs.unity3d.com/6000.0/Documentation/Manual/com.unity.ai.navigation.html
- https://docs.unity3d.com/6000.0/Documentation/Manual/com.unity.test-framework.html
- https://docs.unity3d.com/6000.0/Documentation/Manual/build-command-line.html

## Photon

- https://doc.photonengine.com/fusion/v2/fusion-intro
- https://doc.photonengine.com/fusion/v2/manual/advanced/lag-compensation
- https://doc.photonengine.com/fusion/current/technical-samples/dedicated-server/

## PlayFab

- https://developer.microsoft.com/en-us/games/products/playfab/
- https://developer.microsoft.com/en-us/games/products/playfab/pricing/
- https://learn.microsoft.com/en-us/gaming/playfab/
- https://learn.microsoft.com/en-us/gaming/playfab/community/leaderboards/

## Unity Web

- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-technical-overview.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-performance.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/system-requirements.html
- https://docs.unity3d.com/6000.5/Documentation/Manual/webgl-gettingstarted.html
- https://doc.photonengine.com/fusion/v2/technical-samples/fusion-webgl
- https://doc.photonengine.com/fusion/v2/getting-started/sdk-download

## Android / Google Play

- https://developer.android.com/games/engines/unity/start-in-unity
- https://developer.android.com/games/optimize/overview
- https://developer.android.com/games/sdk/frame-pacing
- https://developer.android.com/games/sdk/performance-tuner/unity
- https://support.google.com/googleplay/android-developer/answer/11926878
- https://support.google.com/googleplay/android-developer/answer/9859152

---

# 44. FIRST MASTER PROMPT FOR LUNA 5.6 XHIGH

Use this prompt from the repository root after saving this document as `Docs/MASTER_VISION.md`.

```text
You are the lead technical director and implementation agent for BattleRaja.

Read, in full:
- Docs/MASTER_VISION.md
- AGENTS.md if it exists
- README.md
- Packages/manifest.json if this is already a Unity repository
- all existing architecture and decision documents

Your job is to build BattleRaja incrementally into a production-quality
Android-first stylised top-down 3D micro battle royale. Do not attempt to
build the entire final game in one pass.

For this first task, perform Milestone 0 only.

Required actions:

1. Inspect the repository, installed Unity versions, Android tooling, Git
   configuration and available build environment.
2. Research current official documentation as required by MASTER_VISION.md.
3. Recommend the exact Unity 6 production-supported version and compatible
   package versions. Do not upgrade or install anything before documenting
   the recommendation.
4. Create or refine:
   - AGENTS.md
   - README.md
   - Docs/RESEARCH_LOG.md
   - Docs/DECISIONS.md
   - Docs/ARCHITECTURE.md
   - Docs/PERFORMANCE_BUDGET.md
   - Docs/TEST_STRATEGY.md
   - Docs/SECURITY.md
5. Design the assembly-definition and folder boundaries so the pure gameplay
   domain does not depend on Photon, PlayFab, UI, animation or scene objects.
6. Create a GitHub issue plan for Milestones 0–5 with small tasks,
   dependencies, acceptance criteria and test requirements.
7. If a Unity project already exists, validate it rather than replacing it.
   If none exists and local tooling permits, create the minimum approved Unity
   URP project.
8. Configure the minimum Test Framework setup.
9. Create command-line validation/build entry points for Android and Web.
10. Produce empty or minimal Android and Web development builds if the SDK and Unity
    installation allow it.
11. Do not implement player movement, combat, bots, networking, economy,
    final UI or final art in this task.
12. Do not add Photon or PlayFab yet.
13. Never expose credentials or accept paid/legal terms.
14. Run every available validation and report:
    - changed files
    - commands
    - test results
    - build result/artifact path
    - warnings
    - blockers
    - assumptions
    - decisions requiring human approval

Quality requirements:

- Prefer official current sources.
- Cite sources in Docs/RESEARCH_LOG.md.
- Keep changes small and reviewable.
- Do not claim completion without evidence.
- Preserve a clean path for offline deterministic simulation first.
```

---

# 45. SECOND PROMPT — MOVEMENT MILESTONE

Run only after Milestone 0 is approved.

```text
Implement Milestone 1: Movement Laboratory.

Read Docs/MASTER_VISION.md and all project guidance first.

Scope:
- One grey-box test arena.
- One player-controlled placeholder fighter.
- Code-driven top-down movement.
- Independent aim direction.
- Desktop controls.
- Android twin-stick controls.
- Camera follow and obstruction-safe framing.
- Aim indicator.
- configurable movement and input settings.
- safe-area-aware UI.
- EditMode and PlayMode tests.
- Android development build.

Do not add:
- damage.
- weapons.
- bots.
- Photon.
- PlayFab.
- progression.
- final art.

Before implementation:
- inspect existing boundaries.
- propose files and tests.
- identify camera projection recommendation and document the decision.

Acceptance:
- no frame-rate-dependent movement.
- no unintended sliding.
- controls function on physical Android.
- camera remains stable.
- relevant tests pass.
- build succeeds.
- no new unexplained warnings.

After implementation, report evidence and request human feedback specifically
on movement responsiveness, stick size, stick position, camera angle and aim feel.
```

---

# 46. QUALITY BAR FOR AI-GENERATED WORK

The agent must not confuse output quantity with progress.

Reject or revise work that:

- Uses one enormous manager class.
- Couples gameplay to UI.
- hard-codes balance values throughout scripts.
- creates circular dependencies.
- adds SDKs before need.
- stores secrets in code.
- mutates shared ScriptableObject runtime state.
- relies on animation events for authoritative hits.
- edits scene YAML blindly.
- has no tests.
- claims mobile optimisation without device evidence.
- claims server authority while trusting client damage.
- copies copyrighted assets.
- imitates reference games too closely.
- generates culturally careless designs.
- creates placeholder systems labelled production-ready.
- ignores warnings or test failures.
- replaces working architecture without a decision record.

---

# 47. FINAL PRODUCT VISION

BattleRaja should ultimately feel like a small animated toy-box battle:

- Immediate enough for a player to start in seconds.
- Tactical enough to reward mastery.
- Short enough for mobile sessions.
- Original enough to stand without reference comparisons.
- Warm, colourful and culturally rich without becoming a stereotype.
- Technically disciplined enough to scale from offline bots to online authority.
- Fair enough that spending changes expression, not combat power.
- Efficient enough to run reliably on ordinary Android phones.

The first mission is not to create a content-heavy live service.

The first mission is to prove this:

> One arena, three fighters, three Jugaad Gadgets, eight combatants and one
> complete four-minute match are fun enough that players immediately choose
> “Play Again.”

Everything else follows from that evidence.
