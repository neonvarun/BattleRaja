# BattleRaja V1 attack + Tiffin proof pass — 2026-09-05 (late night)

## Scope and truthful result

Human-operated two-leg route on the **exact current APK** (`98C3FFAE…FBEB`,
source `feb9258` + docs tips through `6b02710`; no code, asset, balance or
package change in this pass). Leg 1: aimed attack holds at visible enemies.
Leg 2: Tiffin deploy attempt on open ground. Hold-method inputs (500–800 ms
swipes) per the 2026-09-05 methodology correction; file-based on-device
screenshots (≤5 s cadence where the loop allowed).

Result: **Prototype — Android offline release candidate in progress.**
Both individual proofs stay **open (inconclusive, not negative)**: no
projectile frame and no Tiffin deploy frame was captured, and the
`TIFFIN ready` / `Bolt ready • Dash ready` labels never visibly changed.
Team-level combat stayed live throughout (RAJA scored, tickets were
consumed, healing happened). No new defect. Zero configured crash markers.

## Source and device identity

- Branch `main`, HEAD `6b02710` (= prior pass record commit); local APK
  SHA-256 `98C3FFAE5865B80D4B85963FB41638DDB2AC30C7D42957D2455CA6155505FBEB`
  (41,711,004 bytes); installed package `com.example.battleraja.m11`,
  v1.0.0/100, min28/target36, pid 32718 throughout.
- Device: approved Lava `ST5GDW23LB004392` only (`LAVA LXX508`, Android
  14/API 34, `1080x2460`, 4 KB pages). Oppo visible on ADB, never touched.
- Battery 25–31% USB-powered, 35 °C at close; not a charged endurance claim.
- Static validation carried 0/0 (ran twice in the prior pass, no source
  change since). EditMode 173/173 + PlayMode 100/100 carried.

## Leg 1 — aimed attack (captures `attack-tiffin-20260905-235015/00–07`)

1. Baseline `00-baseline.png`: prior unattended result `WINNER RIVAL •
   Clock` (RAJA 1/0/1, RIVAL 10/1/7 at 04:02).
2. REMATCH hold → fresh match as human Bijli (`01-fresh.png`, 00:10):
   85/85, RAJA 0/15 TIX 12, RIVAL 0/15 TIX 12, `RIVAL CARRIER`,
   `TIFFIN ready`.
3. MOVE hold up the lane → `02-approach.png` (00:51): enemies visible top
   and right at close range, player still 85/85.
4. Aim hold up + ATTACK 700 ms → `03-attack-hold.png` (01:50): player
   73/85 (taking enemy fire), RAJA TIX 12→11. No bolt visible in frame.
5. Closed distance right → `04-close.png` (02:30): player 13/85. No bolt
   visible.
6. Aim up-right + ATTACK 700 ms → `05-attack2.png` (03:22): player
   restored 85/85 (respawn consumed, RAJA TIX 10), rival point-blank
   adjacent. No bolt visible.
7. ATTACK 700 ms point-blank → `06-attack3.png` (04:02 live frame, no
   results panel yet): player 80/85, **RAJA 1/15 TIX 9**, RIVAL 3/15
   TIX 11. RAJA scored during the attack leg, but the scorer is not
   attributable to the human from the HUD.
8. `07-terminal.png` results: `WINNER RIVAL • Clock`, RAJA 1/0/1/TIX 9/
   DMG 392/HEAL 232/OBJ 0.0s/GAD 1/ABIL 85 vs RIVAL 3/0/3/TIX 11/DMG 558/
   HEAL 273/OBJ 239.4s/GAD 1/ABIL 59.

Attack verdict: **unconfirmed**. Three 700 ms holds with enemies visible
(once point-blank) produced no captured projectile and no score attribution
to the player. Consistent with the known edge states (aim-cone validation,
bolt-vs-background readability, single-frame sampling) — not a new defect.

## Leg 2 — Tiffin on open ground (captures `08–11`)

1. REMATCH hold → `08-tiffin-fresh.png` (00:11): spawn scrum, player 49/85.
2. MOVE hold south → `09-openground.png` (01:09): player restored 85/85,
   RAJA 7/15 TIX 11, RIVAL 1/15 TIX 11 — open area south of the shrine,
   no enemy adjacent.
3. Aim forward + GADGET 700 ms → `10-tiffin-hold.png` (02:08): player
   1/85 under fire, `TIFFIN ready` unchanged, no station/deployed marker
   visible.
4. `11-tiffin-after.png` (02:39): player 25/85 (+24), RAJA TIX 11→10
   (a Raja respawn consumed). Survival-plus-heal at 1 HP is consistent
   with either an ally Tiffin aura or respawn-adjacent healing, but no
   `TIFFIN STATION DEPLOYED` feedback and no station visual were captured.

Tiffin verdict: **unconfirmed**. One aimed 700 ms gadget hold on open
ground left the HUD label unchanged with no deploy feedback or station
visual. Inconclusive, not negative (placement validation / held-state
sampling / single-frame capture all remain in play).

## Technical evidence

- Saved logcat `logcat-final.txt` (5002 lines, SHA-256
  `DF1EEDDA0539490A30CAA1BD39CFAA2BE54810954D80750386BCF1AFB2E1D538`):
  **0** `FATAL EXCEPTION`, **0** `ANR in`, **0** `SIGSEGV`, **0** `SIGABRT`,
  **0** `NullReferenceException`, **0** `UnityException`,
  **0** `SetLODs: Attempting to force`.
- Point meminfo (live, pid 32718): PSS **265,823 KB**, RSS **417,432 KB**,
  graphics **17,468 KB**.
- No FPS claim (`SurfaceFlinger --latency` returns the period only;
  `gfxinfo` has no frame rows). Compositor-side logcat only: game
  SurfaceView ~58–60.5 fps during the window.
- All 12 screenshots file-based (`shell screencap` + `pull`); `exec-out`
  was not used after the prior pass proved it stale.

## Key capture hashes (`attack-tiffin-20260905-235015/`)

- `01-fresh.png` `C93A14E4F25E1D33D95841DC597E8850BA886FB9F7E55F2C953AC442AAB47A6B`
- `02-approach.png` `9C18B55591D2C40D632174895EA5C382484B88137594833FD1388A588BFDF0E3`
- `03-attack-hold.png` `0EB24AFBF4E492401143FF40CF45FA1D4A427DD8A0E793164758A82858EAF269`
- `04-close.png` `C1E861300285DB6CD99E5E37EE88F498960459F1785BD1358218D1C1EE51A0D4`
- `05-attack2.png` `2CF0ED59DCE487099BD5435501BDE5A58D8E449C2F23F54232E39FD9631649B5`
- `06-attack3.png` `D49B428729BA5E377E5B791FA0739E592D0A56D60399B772C8A2A945CD81E716`
- `07-terminal.png` `9A40A34443E5CB5748D2103910615DEED7596AF5F46253F0EF09F0D1F29E49B7`
- `08-tiffin-fresh.png` `B2F1F4A9638E0E3C9C457D176AB495C47D8ADDC32850E6BFEC652FED51FE077F`
- `09-openground.png` `CECCFE9A0CD0C2A4F674B1380F5E67ACF751D7A5A3778C40D322A769B258735E`
- `10-tiffin-hold.png` `1D709454132BAB619E71299E15A2A36F365A79714BF385DB56CF95FE81DF5739`
- `11-tiffin-after.png` `2B9FDC580BFE10A72427435045BDDABC8FF63E6A75C3C65B0EB50157417F6372`

## Open items after this pass (owner gates unchanged)

Human attack projectile frame + score attribution, human Tiffin deploy
feedback + station visual, ability/dash effect, Crown deposit by the
player, explicit 0/85 spectator-frame sampling, overtime, tutorial
re-proof, 10-rematch endurance, normalized frame/GC/GPU profiling,
physical 16 KB runtime, commissioned art/audio, spawn-pressure tuning
review, and all identity/signing/privacy/Play gates.
