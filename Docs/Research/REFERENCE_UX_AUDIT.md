# Reference-game UX audit (controlled Lava observation)

Updated: 2026-08-27 (IST)

This is a constrained, observation-only study performed on the approved Lava
`ST5GDW23LB004392` (`LAVA LXX508`, API 34). It records abstract UX principles for
BattleRaja. It is not a request to copy any reference game's art, wording, layout,
characters, audio, effects, monetisation, or trade dress.

No account was created, signed into, purchased from, posted to, or changed. No APK or
asset extraction, decompilation, network interception, sound/music recording, or pixel
tracing was performed. The captured reference screenshots remain in the ignored local
folder `Builds/Local/Device/ReferenceUx/20260827/`; they are not store assets and are
not committed because the existing app state visibly contains account/profile labels.

## Brawl Stars

- **Observed package/version:** `com.supercell.brawlstars`, version `68.279`
  (`versionCode 68279`, observed from the installed package on Lava).
- **Flow observed:** cold launch to the landscape home surface. A large bottom-right
  primary `PLAY` action, a central fighter preview, top resource/status strip, and
  vertically stacked side navigation were visible. A contextual upgrade callout was
  already present. A non-destructive tap probe on the primary action did not advance
  during the short observation window; no account or network action was attempted.
- **Abstract principle:** make the first playable action visually dominant and keep
  secondary systems on stable peripheral rails; contextual coaching can point at one
  actionable surface without replacing the whole scene.
- **BattleRaja adaptation:** keep `Offline Play` as the single high-priority menu
  action, use compact fighter/mode cards and a persistent settings affordance, and
  keep tutorial coaching over the visible Bazaar arena so the player retains spatial
  context.
- **Originality difference:** BattleRaja uses fictional Bazaar Bastion, Bijli/Pehel/
  Maya, Solo Raja and its own ink-and-saffron UI tokens; it does not reproduce the
  reference's characters, labels, resource economy, iconography, or panel geometry.
- **Evidence:** local observation capture `brawlstars-launch.png`, SHA-256
  `AF5E761A163BEF0C11BBF8694B4FAD2D2DEDDCC87F19151A67D8E8DB2A581FE0`; package UI
  hierarchy capture `brawlstars-ui.xml`, SHA-256
  `C6E29F0563753FAF3A3F0A27FAB5C7C448ED245C8578D89D99AF0B2E1D2BA05042`.
- **Uncertainty:** the installed app was in an existing local/account state and the
  short tap probe did not establish the complete selection or match route. Those
  routes were intentionally not pursued through sign-in, purchases, or network setup.

## Smash Karts

- **Observed package/version:** `com.tallteam.citychase`, version `2.15.1`
  (`versionCode 2000160`, observed from the installed package on Lava).
- **Flow observed:** cold launch to the landscape home surface. A single central
  `PLAY` button dominated the lower-middle area, with `Create` and `Join` below it;
  profile/progression counters sat along the top and settings/social affordances were
  grouped in the lower-right. The visible primary play surface showed a lock and the
  left panel offered sign-in/up; a non-destructive tap probe left the surface unchanged.
- **Abstract principle:** one clear primary CTA with lightweight secondary actions,
  while lock/availability state is visible before the user commits to a deeper flow.
- **BattleRaja adaptation:** expose a clear offline `PLAY` route, show mode/fighter
  readiness before loading a match, and make unavailable or owner-controlled features
  explicit rather than presenting dead online-looking controls.
- **Originality difference:** BattleRaja has no account, online room, social, or
  progression economy in V1; its readiness, rematch, and settings surfaces are built
  around an eight-participant offline arena and original controls.
- **Evidence:** local observation capture `smash-karts-launch.png`, SHA-256
  `69A7F8654D28B726F2BF6473BBF6710FAF9EBF3F2B75CBB07EDCEE0CDFA7271A`; the
  post-tap capture `smash-karts-play-tap.png`, SHA-256
  `7421B3FCE40DFC75B4EFF9E8884E4A931D581EA7DB63AD5681DA335A0F48B921`; package UI
  hierarchy capture `smash-karts-ui.xml`, SHA-256
  `699FD353B1BF4CD64022AC7AA745C59D480389B5523F1DC668C3784A6954C11B`.
- **Uncertainty:** the visible lock/sign-in state prevented a deeper offline play
  route without account changes. The observation therefore covers entry hierarchy,
  not in-match controls or results.

## BattleRaja decisions

1. Retain a single offline primary action with a visible arena/fighter context.
2. Keep mode, fighter, settings, accessibility and rematch paths explicit and local;
   do not add account-shaped placeholders to the V1 surface.
3. Use tutorial callouts that teach one action at a time while leaving gameplay visible.
4. Treat this audit as principle-level research only. Final orientation, touch comfort,
   contrast, motion, wording, cultural fit and store presentation still require owner
   review on BattleRaja itself.
