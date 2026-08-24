# Reference UI audit — 2026-08-24

This is a read-only visual reference note for the V1 offline Android candidate. It is
not an approval to copy another game's art, UI, layout, copy, characters, sounds, or
monetisation patterns.

## Device and evidence

- Device used: approved Lava Android device `ST5GDW23LB004392` only.
- BattleRaja package was relaunched after the reference inspection:
  `com.example.battleraja.m11`.
- Reference screenshot: `C:\Users\USER\AppData\Local\Temp\battleraja-reference-brawl-2026-08-24\brawl-menu.png`.
- Reference UI dump: `C:\Users\USER\AppData\Local\Temp\battleraja-reference-brawl-2026-08-24\window.xml`.
- No reference screenshots or external assets were imported into the repository.

## Installed-reference result

- Brawl Stars was installed as `com.supercell.brawlstars`, version `68.279`.
- Its launch surface rendered at approximately `2460x1080` in landscape rotation.
- Android UIAutomator exposed a single game surface rather than semantic button nodes;
  this note therefore records visual observations only.
- No installed package matched `smash`, `kart`, or `smashkarts`. Smash Karts was not
  available for inspection on this device and is not treated as observed evidence.

## High-level observations retained as inspiration

- A single visually dominant play action is easy to locate.
- A central character/arena focal point gives the home screen an immediate game identity.
- Secondary destinations are grouped into compact rails rather than competing with the
  primary play action.
- Status and progression information is separated into small, high-contrast zones.
- The landscape layout uses deliberate wide-screen composition and strong silhouette
  contrast.

## BattleRaja decisions

- Keep BattleRaja's original fictional Bazaar Bastion identity, geometric hero mark,
  cyan/saffron/magenta palette, and offline-first copy.
- Keep the V1 menu free of accounts, currencies, shops, clubs, social prompts, ads,
  matchmaking and internet-dependent status. Those patterns are outside the V1 scope.
- Preserve the current portrait Android menu and the existing safe-area/accessibility
  work until a human device review approves any orientation change.
- Use the observations above only to guide hierarchy, CTA prominence, readable grouping,
  and silhouette contrast. Do not reproduce Brawl Stars' characters, map, typography,
  icons, sound, panels, or distinctive composition.

## Open review items

- Human approval is still required for the final BattleRaja visual direction, touch
  ergonomics, combat readability, cultural review, and store creative package.
- Smash Karts comparison remains unavailable unless the owner installs it on the
  approved device or supplies an independently reviewable reference.
- The BattleRaja physical-touch route remains unproven; the Android UI tree exposes a
  Unity surface rather than semantic controls, so this note does not claim a touch pass.
