# V1 Android visual-feedback evidence — 2026-08-24

This is a bounded presentation slice for the offline Android V1 candidate. It is
technical evidence, not Play Store approval or final human visual approval.

## Exact source and automated gates

- Repository: `neonvarun/BattleRaja`
- Branch: `codex/v1-playstore-release`
- Runtime source: `df9adb0519ba3284ce6cd86c10778b5e117cc1e3`
  (`presentation: deepen offline combat readability`)
- Unity: `6000.5.6f1` (`0e0577a1a2ac`)
- Disposable verification copy:
  `C:\Projects\BattleRaja-v1-verify-20260824e`
- Repository validation: **0 errors / 0 warnings**
- EditMode: **125/125 passed**
- PlayMode: **65/65 passed** (one new visual-feedback regression)

The new runtime behavior is presentation-only: fighter silhouette parts receive
state-specific procedural pose motion, impact feedback uses a bounded pooled halo
plus impact shape, and gadget identity visuals bob/rotate/pulse. No authority,
damage, input, inventory or match-state rule was changed.

## Android artifacts

Both artifacts were built from the exact source above in the disposable copy, so
Unity scene generation and build output did not touch the tracked checkout.

| Artifact | Size | SHA-256 |
| --- | ---: | --- |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk` | 40,428,259 bytes | `2E51CCF590149A6726302F0AFB56070D85BB6E9669FC084FC4BF4C6D5A6AB217` |
| `Builds/V1/Android/BattleRaja-V1.0-release-candidate.aab` | 36,258,342 bytes | `2B7CD24E01287E80B03C15824B593867C1B5A0AFF3347065D850343830F9204A` |

The AAB check passed: base manifest present, 8 ARM64 libraries, 0 other ABIs,
450 entries, and every checked ARM64 ELF LOAD segment aligned to `0x4000`.
The APK/AAB remains debug-signed with the temporary package ID
`com.example.battleraja.m11`; it is not publishable.

## Lava smoke evidence

Only Lava serial `ST5GDW23LB004392` was used. The APK installed successfully and
`com.unity3d.player.UnityPlayerGameActivity` was top-resumed. Captures and raw
logs are outside the repository under
`C:\Projects\BattleRaja-v1-verify-20260824e\Builds\V1\Lava\`:

- `visual-feedback-menu.png`: offline menu with Play Offline, Tutorial Replay,
  Settings & Accessibility, and Help & Controls.
- `visual-feedback-mode.png`: Solo Raja route.
- `visual-feedback-match-opening.png`: fighter selection route after entering
  the offline mode.
- `visual-feedback-match.png`: Bazaar Bastion opening with eight actors,
  readable HUD, twin-stick controls and distinct fighter identities.
- `visual-feedback-combat.png`: post-warmup interaction probe with attack and
  ability input; no fatal application marker was observed.
- `visual-feedback-gadget-route.png` and `visual-feedback-gadget-route2.png`:
  touch movement probes toward the production gadget route.
- `visual-feedback-gadget-use.png`: gadget button probe while the HUD still
  reported `No gadget held`.
- `visual-feedback-menu-ui.xml` and `visual-feedback-match-ui.xml`: Unity's
  canvas is exposed as one full-screen surface, so semantic button bounds were
  unavailable; coordinate taps were used only for this bounded smoke probe.
- `visual-feedback-*-logcat.txt`, `visual-feedback-activities.txt`,
  `visual-feedback-meminfo.txt`, and `visual-feedback-gfxinfo.txt`: raw runtime
  evidence and crash-marker/performance inspection.

The sampled device memory was **281,375 KB PSS**, **418,520 KB RSS**, **94,896 KB
Graphics**, and **69 KB swap**. `gfxinfo` did not expose a usable frame/jank
histogram, so no stable-FPS or performance pass is claimed.

## Reference-app boundary

Brawl Stars (`com.supercell.brawlstars`) was opened read-only on Lava for high-level
UX observation: a clear primary Play action, high-contrast action labels, and
edge-anchored panels/resources. BattleRaja keeps its own shield/bolt identity,
vector fighter silhouettes, toy-bazaar arena and offline-first flow; no Brawl
Stars branding, character, arena, UI art, sound or protected asset was copied.
Smash Karts was not installed on the approved device and was not installed for
this pass.

## Open evidence gap

The automated PlayMode gadget regression remains green, and the source-backed
gadget identities render in the match. This Lava pass did **not** capture a
successful human-facing gadget pickup and use; the HUD remained empty during the
route probe. Tutorial completion, results/rematch, accessibility, touch ergonomics,
long-run performance, final art/audio, signing, store/legal and cultural review
remain open. The project remains classified as **prototype**.
