# BattleRaja V1 comfort hold-proof pass — 2026-09-05 (late)

## Scope and truthful result

Human-operated end-to-end route on the **exact current APK** (`98C3FFAE…FBEB`,
source `feb9258` + docs tips `493b972`/`7fcb147`/`f0198ad`; no code, asset,
balance or package change in this pass). Hold-method inputs (≥300–800 ms
swipes) per the 2026-09-05 methodology correction.

Result: **Prototype — Android offline release candidate in progress.**
Live 4v4 → results → rematch → fresh-live loop re-proven on approved Lava
with zero configured crash markers. Human movement re-confirmed; human
attack/ability/gadget/deposit effects remain individually unconfirmed and
stay the next device task. No new defect.

## Source and device identity

- Branch `main`, local HEAD `f0198ad`, `origin/main` identical at pass start;
  worktree clean except pre-existing untracked `.commandcode/` and
  `untitled-project/`; 2 pre-existing stashes preserved untouched;
  `git lfs fsck --pointers` OK.
- Unity `6000.5.6f1` (per `ProjectSettings/ProjectVersion.txt`).
- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,711,004
  bytes, SHA-256 `98C3FFAE5865B80D4B85963FB41638DDB2AC30C7D42957D2455CA6155505FBEB`.
  Pulled installed `base.apk` from Lava hashes identically.
- AAB 37,536,525 bytes, SHA-256
  `4474291CC74919F1FCD73C55CE2E44EA1C28D42C014FFF5CF3CCEFA90305611A`
  (carried; not rebuilt here).
- Installed package: `com.example.battleraja.m11`, version `1.0.0`/code `100`,
  minSdk 28 / targetSdk 36, process pid 32718.
- Device: approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34,
  `1080x2460`, `getconf PAGESIZE=4096`). Disallowed Oppo `b60e53b3` visible
  on ADB, never touched. All device commands used explicit `-s`.
- Battery caveat: 32% → 31% USB-powered, 34 → 35 °C; thermal-repeater log
  shows a benign `recvMdThermalInfo ERROR` diagnostic. Not a charged
  endurance claim.
- Static validation `Tools/Validation/validate.ps1 -RequireUnityProject`:
  **0 errors / 0 warnings** (re-ran in this pass). EditMode **173/173**,
  PlayMode **100/100** and the strict bot batch are carried from the
  exact-source checkpoint (no runtime change since `feb9258` except docs).

## Methodology notes

- `adb shell input tap` is too short for held-state combat sampling; this
  pass used `input swipe x y x y 300–800` holds for ATTACK/ABILITY/GADGET/
  MOVE/REMATCH.
- `adb exec-out screencap` returned a stale identical frame repeatedly
  (hashes `908570F0…`, `F3B23AF6…`, `560053E6…`, `92408195…` all show the
  same 04:02 results card). Switched to on-device `shell screencap -p
  /sdcard/brq_NN.png` + `pull`, which gave live frames. Future passes
  should use the file-based path only.
- REMATCH rect (1080x2460 portrait): REMATCH ≈ (430, 1575), MENU ≈ (649,
  1600). A 300 ms hold on REMATCH started the fresh match.

## Route observed (captures under `Builds/Local/V1GameplayTruth/Next/comfort-holdproof-20260905-222528/`)

1. Foregrounded app showed a prior unattended match result: `WINNER TEAM
   RAJA • Clock`, RAJA 8 / DEPOSITS 2 / KOs 2 / TICKETS 7 vs RIVAL 5 /
   DEPOSITS 0 / KOs 5 at 04:02 (`01-foreground-state.png`).
2. REMATCH hold → fresh live match as human Bijli (`06-filebased.png`
   02:09): player 61/85, RAJA 0/15 TIX 11, RIVAL 1/15 TIX 12,
   `RIVAL CARRIER`, squad strip `A2 BIJLI / A3 PEHEL / A4 MAYA` with roles
   and health fills, `TIFFIN ready`, `Bolt ready • Dash ready`.
3. Live progression: 02:41 player 13/85 under rival pressure (`07-live2.png`);
   03:21 player restored 85/85, RAJA 0→1 unlikely — RAJA stayed 0/15 while
   TIX 11→10 and `CROWN DROPPED` showed (`08-move-probe.png`); 03:47
   unchanged (`09-attack-hold.png`).
4. Results at 04:02 (`10-aim-attack.png`): `WINNER RIVAL • Clock`, RAJA 1 /
   DEPOSITS 0 / KOs 1 / TICKETS 9 / DMG 392 / HEAL 232 / OBJ 0.0s / GAD 1 /
   ABIL 85 vs RIVAL 3 / DEPOSITS 0 / KOs 3 / TICKETS 11 / DMG 558 /
   HEAL 273 / OBJ 239.4s / GAD 1 / ABIL 59.
5. REMATCH hold → fresh match 00:08, scores 0/15, TIX 12/12, new seed
   (`11-rematch-fresh.png`). Rematch reset proven.
6. Fresh-match probes: 00:52 RAJA 1/15 TIX 11 vs RIVAL 1/15 TIX 12,
   `CROWN DROPPED`, player 85/85 (`12-fresh-move.png`) — human-side KO
   scoring path live. 01:26 `RAJA CARRIER`, TIX 10/11
   (`13-ability-hold.png`). 02:03 player 49/85 with one small cyan dot
   left of the player — possible bolt, single-frame inconclusive
   (`14-gadget-hold.png`). 02:32 player 13/85, RAJA TIX 10→8, RIVAL 4/15
   (`15-gadget-after.png`) — respawn consumption live, explicit 0/85
   `OUT OF ACTION` frame not sampled.
7. MOVE holds moved the human and camera across all live frames; ATTACK
   (500/600 ms), ABILITY (600 ms) and GADGET (600 ms) holds left
   `TIFFIN ready` / `Bolt ready • Dash ready` labels unchanged on screen —
   inconclusive per the known aim/placement edge states, not a new defect.

## Technical evidence

- Saved logcat `logcat-final.txt` (597,922 bytes, 5002 lines, SHA-256
  `3D40D9F7F4FF6D1798275C310CF9D16FC2F4D38C699742C2225D152DA9DDDA18`):
  **0** `FATAL EXCEPTION`, **0** `ANR in`, **0** `SIGSEGV`, **0** `SIGABRT`,
  **0** `NullReferenceException`, **0** `UnityException`,
  **0** `SetLODs: Attempting to force`.
- Point meminfo (live 02:32, pid 32718): PSS **227,081 KB**, RSS
  **377,664 KB**, graphics **17,468 KB**.
- `dumpsys SurfaceFlinger --latency` returns only the refresh period
  (16666667 ns); Unity `gfxinfo` exposes no frame rows — no FPS claim.
  Compositor-side diagnostic only: logcat `BufferQueueProducer` for the
  game SurfaceView reports ~58–60.5 fps, `hwcomposer` display ~59.4–60.5,
  `libPerfCtl xgfGetFPS pid:32718` 58–60 during the pass.

## Key capture hashes (`comfort-holdproof-20260905-222528/`)

- `06-filebased.png` `3A90C0BDC6ED824132325E64A8450FC1436EFCE315EDD69679C180F2EF15F610`
- `07-live2.png` `859154C378436F75B62617CC2B395636B092CE0CE146B5585FD673EF892BDCF3`
- `08-move-probe.png` `830D1FE3E0E081BD22D1EBE6179E968FF7429CA478EAF0FE77A504229130AA2B`
- `09-attack-hold.png` `51C50B69C362B66BEE05AAF6BCD0D218291F1B1AF5F5B51EE4657114CF8399DD`
- `10-aim-attack.png` `0C258B37CB5DCF3E88E857943E0AB8B4B39E659C60E0A5C76D5E1C260026D4AE`
- `11-rematch-fresh.png` `838D09E5A49CB69506A1A5FBC792474382B913DF739D865D7DAEFAC9277A1179`
- `12-fresh-move.png` `09B3F71A484DB7A1FD4A1B2FD168CC95799919842803B9B381B1290D846AB12A`
- `13-ability-hold.png` `140827632E6827A2F6757AA20A3FFA86BB7453F0AB1217D9B8B518BE322DC514`
- `14-gadget-hold.png` `5C02939AAF7D1800C0C398D54D54312386308E001855C4AA9E4E18387D7E8125`
- `15-gadget-after.png` `5F872C0F2D5D3FB1E52D7EFCDC9A5565DD51A31CB240A817152DE21B48B9D3AC`

## Open items after this pass (owner gates unchanged)

Human attack/ability/gadget/deposit individual proof (hold method, aim at
a visible enemy, open ground for Tiffin), explicit 0/85 spectator-frame
sampling at ≤5 s cadence, overtime observation, tutorial re-proof on this
APK, 10-rematch endurance, normalized frame/GC/GPU profiling, physical
16 KB runtime, commissioned art/audio, spawn-pressure tuning review, and
all identity/signing/privacy/Play gates.
