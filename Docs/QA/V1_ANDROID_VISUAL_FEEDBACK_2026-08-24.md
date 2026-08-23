# V1 Android visual-feedback evidence — 2026-08-24

This is a bounded presentation slice for the offline Android V1 candidate. It is
technical evidence, not Play Store approval or final human visual approval.

## Exact source and automated gates

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Runtime source: `d825832bced4c5e07c7967d891696842eb55609a`
  (`content: make tiffin route player-owned`)
- Unity: `6000.5.6f1` (`0e0577a1a2ac`)
- Disposable verification copy:
  `C:\Projects\BattleRaja-v1-verify-20260824e`
- Repository validation: **0 errors / 0 warnings**
- EditMode: **125/125 passed**
- PlayMode: **66/66 passed** (including the production Tiffin reachability regression)

The current slice keeps the existing render-only fighter motion, pooled impact
feedback and animated gadget identity visuals, and makes the production Tiffin
route player-owned: the pickup is authored near the protected player spawn,
other gadget pickups are kept out of the initial claim radius, the HUD exposes a
nearby-pickup hint, and a PlayMode regression proves the authority pickup/use
path. The beacon is presentation-only; pickup ownership, inventory and use still
resolve through the authority.

## Android artifacts

Both artifacts were built from the exact source above in the disposable copy, so
Unity scene generation and build output did not touch the tracked checkout.

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 40,429,675 bytes | `50FD2D7F9C29F4888F2965810F9FD8130F7C2857F2A15AD7E3A5CF5908E7BFCC` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 36,259,768 bytes | `052F9CAB180E15AEEC0C2D8DCAB47187C53C58F07629C69F81A647697DB9FBF1` |

The AAB check passed: base manifest present, 8 ARM64 libraries, 0 other ABIs,
450 entries, and every checked ARM64 ELF LOAD segment aligned to `0x4000`.
The APK/AAB remains debug-signed with the temporary package ID
`com.example.battleraja.m11`; it is not publishable.

## Lava smoke evidence

Only Lava serial `ST5GDW23LB004392` was used. The APK installed successfully and
`com.unity3d.player.UnityPlayerGameActivity` was top-resumed. Captures and raw
logs are outside the repository under
`C:\Projects\BattleRaja-v1-verify-20260824j\Builds\V1\Lava\`:

- `visual-feedback-menu.png`: offline menu with Play Offline, Tutorial Replay,
  Settings & Accessibility, and Help & Controls.
- `visual-feedback-mode.png`: Solo Raja route.
- `visual-feedback-match-opening.png`: fighter selection route after entering
  the offline mode.
- `visual-feedback-match.png`: Bazaar Bastion opening with eight actors,
  readable HUD, twin-stick controls and distinct fighter identities.
- `visual-feedback-combat.png`: post-warmup interaction probe with attack and
  ability input; no fatal application marker was observed.
- `v1-match-near.png`: match opening with the player HUD showing
  `GADGET [G] tiffin_station` and the authority pickup event.
- `v1-tiffin-used.png`: post-use capture showing `GADGET [G] empty`, the
  cooldown and the visible Tiffin Station near the player.
- `visual-feedback-menu-ui.xml` and `visual-feedback-match-ui.xml`: Unity's
  canvas is exposed as one full-screen surface, so semantic button bounds were
  unavailable; coordinate taps were used only for this bounded smoke probe.
- `visual-feedback-*-logcat.txt`, `visual-feedback-activities.txt`,
  `visual-feedback-meminfo.txt`, and `visual-feedback-gfxinfo.txt`: raw runtime
  evidence and crash-marker/performance inspection.

The sampled device memory was **285,919 KB PSS**, **422,810 KB RSS**, **100,226 KB
Graphics**, and **69 KB swap**. `gfxinfo` did not expose a usable frame/jank
histogram, so no stable-FPS or performance pass is claimed. The captured app
logcat contained no fatal exception, AndroidRuntime, SIGSEGV, SIGABRT,
NullReferenceException or MissingReferenceException marker.

## Reference-app boundary

Brawl Stars (`com.supercell.brawlstars`) was opened read-only on Lava for high-level
UX observation: a clear primary Play action, high-contrast action labels, and
edge-anchored panels/resources. BattleRaja keeps its own shield/bolt identity,
vector fighter silhouettes, toy-bazaar arena and offline-first flow; no Brawl
Stars branding, character, arena, UI art, sound or protected asset was copied.
Smash Karts was not installed on the approved device and was not installed for
this pass.

## Open evidence gap

The production Tiffin pickup/use route is now captured on Lava and the automated
authority regression remains green. Tutorial completion, results/rematch,
accessibility, touch ergonomics, long-run performance, final art/audio, signing,
store/legal and cultural review remain open. The project remains classified as
**prototype**.
