# BattleRaja V1 Maya + Bijli human loops with KO/respawn re-proof — 2026-09-05

## Scope and truthful result

Two human-operated matches on the **exact current APK** (`98C3FFAE…FBEB`,
source `493b972`/`7fcb147` docs tip; no code/asset change in this pass):
Maya-as-human (full match, RIVAL win) and Bijli-as-human (FirstToScore RAJA
win plus a live player KO → respawn re-proof). Adds a test-methodology
correction: `adb shell input tap` presses are too short for the held-state
attack/ability/gadget sampling and must be replaced by ≥300 ms holds.

Result: **Prototype — Android offline release candidate in progress.**
Human movement is confirmed for all three fighters; human attack/ability/
gadget/deposit effects remain individually unconfirmed and are carried as the
next device task. No crash markers; no new defect.

## Device identity and caveats

- Approved Lava `ST5GDW23LB004392` only (Oppo visible on ADB, never used).
- Mid-pass interruption: device asleep + notification shade open + secure
  lockscreen; owner unlocked on request. Pre-unlock black captures
  (`13/14/17/18/19/20/21/24`) are environmental artifacts, not an app defect:
  post-unlock menu rendered immediately and the game resumed normally.
- Battery 36% → 32%, USB-powered, thermal status 0 throughout.
- `E/Unity ClassNotFoundException: ...play.core.assetpacks.AssetPackManager`
  appears once per cold start and is non-fatal (startup continues); recorded
  as an observation, not a blocker.

## Maya-as-human match (RIVAL 7 • Clock at 04:02)

Route: results(`00-current-state.png`: prior unattended rematch resolved
`WINNER RIVAL`, RAJA 8/DEPOSITS 2 vs RIVAL 9/DEPOSITS 1 — bot deposit proof) →
MENU → PLAY OFFLINE → briefing → `SELECTED: MAYA` (`03-maya-selected.png`) →
live (`04-live-early.png` 00:15, player 65/85 already under spawn pressure).

- Move/attack/decoy/gadget probes (`05-maya-probes.png` 00:47): player
  65→37 under rival pressure; `Decoy ready`/`TIFFIN ready` persisted.
- Player held 13/85 for ~80 s (`07` 02:08 → `09` 03:25) while rivals
  disengaged: consistent with the 16 m + line-of-sight perception bound,
  recorded as fair-AI-consistent rather than a defect.
- Results (`10-ko-watch-4.png`): `WINNER RIVAL • Clock`, RAJA 0/DEPOSITS 0/
  KOs 0/TICKETS 5 vs RIVAL 7/DEPOSITS 0/KOs 7, OBJ 0.0 s vs 239.4 s.
  Player survived the full match at 13 HP.

## Bijli-as-human match (RAJA 15 • FirstToScore at 01:30)

`SELECTED: BIJLI` persisted across force-stops (`26-select-again.png`).
Live at 00:17 (`27-bijli-live.png`): player 25/85 in a spawn scrum —
third consecutive match with ~20+ damage inside the first 15 s, recorded as
spawn-separation tuning feedback for the owner.

- 01:02 (`28-bijli-push.png`): RAJA 0→6, `SOCKET 3` rotation visible, player
  restored to 85/85 with RAJA TIX 12→11 (KO → respawn consumed a ticket).
- Dash attempt used aim-hold + ability tap; match ended seconds later so the
  individual dash effect stays unconfirmed.
- Results (`29-dash-attempt.png` frame + `34-match-end.png` for the following
  match): `WINNER TEAM RAJA • FirstToScore`, RAJA 15/DEPOSITS 4/KOs 3 vs
  RIVAL 1/DEPOSITS 0/KOs 1 — first-to-15 path proven live with deposits.

## Player KO → spectator → respawn re-proof (this exact APK)

- `32-longpress-attack.png` 02:49: player 13/85 grappling a rival.
- `33-ko-watch-5.png` 03:47: player 85/85 back in arena; RAJA TIX 8→6;
  ally strip shows `A2 BIJLI … RETURN 3s` — the friendly-strip respawn
  countdown rendering live; `CROWN DROPPED`; Aandhi ring closed to final
  circle. The 0/85 intermediate frame was between 15 s samples (prior
  exact-APK proof covers it); ticket decrement + full-HP return + strip
  countdown jointly prove the loop on the current candidate.

## Test-methodology correction (applies to all future passes)

`PlayerInputAdapter.IsAttackHeld` (and ability/gadget equivalents) samples
held state per frame. `adb shell input tap` delivers DOWN+UP inside one
vsync, so event-driven uGUI menu Buttons fire but held-state combat inputs
can fall between frames. All prior "no visible attack/ability/gadget effect"
observations are therefore **inconclusive, not negative**. Future passes must
use `input swipe x y x y 300+` holds for combat inputs. First hold
(`32-longpress-attack.png`) shows a possible cyan bolt mid-frame; individual
confirmation still open.

## Technical evidence

- Scoped logcat `logcat-bijli.txt` (SHA-256
  `913C1D26260AC041AE46A0C14C4ECE68DB39CB33659E69658076B177B47B2ACF`):
  **0** fatal/ANR/native/managed markers (7-pattern scan).
- Point samples: end-of-match PSS **284,792 KB**, RSS **423,584 KB**,
  graphics **86,724 KB**, thermal **0**, battery 32% / 34 °C.
- `gfxinfo`/`SurfaceFlinger` still expose no frame rows: no FPS claim.
- Static validation re-ran **0/0**; EditMode 173/173 + PlayMode 100/100 and
  the strict bot batch are carried (no code changed).

## Open items after this pass

Human attack/ability/gadget/deposit individual proof (long-press method),
overtime observation, tutorial re-proof on current APK, 10-rematch
endurance, normalized frame profiling, spawn-pressure tuning review,
physical 16 KB runtime, commissioned art/audio, and all owner
identity/signing/privacy/Play gates.

## Key capture hashes (`maya-human-20260905-034500/`)

- `04-live-early.png` `C5DE0DE4D737831D997CA324E86C33CEA1FAC83B05649B1ACB828444222AAA69`
- `05-maya-probes.png` `1CBA3EC8ACE70E1D3D4D3C9457E27DA3F1EF1EE9546CDCEA42EF101DCFB50D9A`
- `07-ko-watch-2.png` `51AF3BD7889208FE644BCC418B3EB3721F9F6EC6FD712FF20781B34E03A0E23D`
- `10-ko-watch-4.png` `9F9023B8A2A7AA6A800E352D6A702AA380BE1B5004A6DD9CD396770A4248FAE7`
- `27-bijli-live.png` `405EDF8B7AAACA11595EBE89A1AA46C44FB0F93B4A5F5344BDA6576AC9DE1136`
- `28-bijli-push.png` `823678EE1D4CFE1C76B8E1D5DD42B8CFAEA8F483D8C4E4CBAB3D93D0A72BF1D7`
- `32-longpress-attack.png` `0E082FDE1C54A520D357CF5A62E3AD7A786EF7B02CC2EF7D60DA17BAE4DFD012`
- `33-ko-watch-5.png` `3B6A0DBC327206F8DDBC2A38005830AB34ED6DBC834D529280FABC5FE12C695D`
- `34-match-end.png` `3DC911F978F5802002F7FD7E3F04E7ED2990A1A3BD4663F35D82688D3B2D38BA`
