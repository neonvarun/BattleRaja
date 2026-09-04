# BattleRaja V1 charged Lava comfort/evidence pass — 2026-09-05

## Scope and truthful result

Human-operated end-to-end route on the **exact current APK** (`98C3FFAE…FBEB`,
source `feb9258`/`493b972`, temporary package `com.example.battleraja.m11`).
No source, asset, balance or package change was made in this pass; it is
bounded comfort/observation evidence, not a new release gate.

Result: **Prototype — Android offline release candidate in progress.**
The full menu → briefing → fighter select → live 4v4 → results → rematch →
pause/settings → HOME/resume loop works on approved Lava with zero configured
crash markers. Player KO → spectator → respawn was **not** re-observed in this
run (prior exact-APK proof in `Docs/QA/V1_VISUAL_POLISH_2026-09-04.md` stands);
human ability/gadget taps showed no visible effect (consistent with the known
aim/placement validation edge states, not a new defect).

## Source and device identity

- Branch `main`, local HEAD `493b972aec5dd0eec1230dff225c87ec03449037`,
  `origin/main` identical at pass start; worktree clean; 2 pre-existing stashes
  preserved untouched; `git lfs fsck` OK.
- Unity `6000.5.6f1` (per `ProjectSettings/ProjectVersion.txt`).
- APK `Builds/V1/Android/BattleRaja-V1.0-release-candidate.apk`, 41,711,004 bytes,
  SHA-256 `98C3FFAE5865B80D4B85963FB41638DDB2AC30C7D42957D2455CA6155505FBEB`.
- AAB 37,536,525 bytes,
  SHA-256 `4474291CC74919F1FCD73C55CE2E44EA1C28D42C014FFF5CF3CCEFA90305611A`
  (carried from the 2026-09-05 checkpoint; not rebuilt here).
- Device: approved Lava `ST5GDW23LB004392` (`LAVA LXX508`, Android 14/API 34,
  `1080x2460`, `getconf PAGESIZE=4096`). The disallowed Oppo `b60e53b3` was
  visible on ADB and was never touched. Pulled installed `base.apk` SHA-256
  matches the local APK exactly.
- Battery caveat: the run executed at **38% on USB power** (thermal status 0,
  battery 32 °C), not a full charge, so this is a bounded pass, not a charged
  endurance claim.

## Route observed (all screenshots under `Builds/Local/V1GameplayTruth/Next/charged-comfort-20260905-033636/`)

1. Cold launch → menu (`01-cold-launch-menu.png`): `PLAY OFFLINE` dominant,
   `TUTORIAL REPLAY`, `SETTINGS & ACCESSIBILITY`, `HELP & CONTROLS`,
   `OFFLINE 4V4 • 8 FIGHTERS • AIRPLANE MODE READY`.
2. `PLAY OFFLINE` → Bastion briefing (`03-briefing.png`):
   `BASTION CROWN • 4V4`, `TEAM RAJA: YOU + 3 ALLIES`,
   `RIVAL: 4 BOTS • CROWN + KOs + SHARED TICKETS`.
3. Mode card → `CHOOSE YOUR RAJA` (`07-fighter-select.png`): BIJLI / PEHEL /
   MAYA cards with portraits and hints. Note: the persisted selection was
   Pehel and the human stayed Pehel (a Bijli-card tap at an estimated position
   missed); Bijli/Maya-as-human remain open comfort items.
4. `START BASTION CROWN` → live 4v4 (`08-live-early.png`, 00:14):
   player card `PEHEL • ANCHOR 85/85`, squad strip `A2 BIJLI / A3 PEHEL /
   A4 MAYA` with roles and health fills, `RIVAL CARRIER`, twin sticks,
   ATTACK/ABILITY/GADGET buttons, PAUSE, SPECTATE.
5. Action probes: MOVE swipe moved the human; ATTACK taps coincided with
   projectile streaks in later frames and team score progression; ABILITY and
   GADGET taps left `Charge ready` / `TIFFIN ready` unchanged on screen
   (`09-move-attack-probe.png`, `10-ability-gadget-probe.png`). This matches
   the known Pehel-charge-aim and Tiffin-invalid-placement edge states; the
   results card later confirmed RAJA `GAD 0 / ABIL 39` (player contributed no
   ability) vs RIVAL `GAD 1 / ABIL 156`.
6. Mid-match progression (`11-push-to-combat.png` 01:42 → `13-combat-round2.png`
   02:51 → `14-exit-push.png` 03:49): score went RAJA 0→1→3, RIVAL 0→1;
   tickets RAJA 12→11, RIVAL 12→11→9; Crown state `RIVAL CARRIER` →
   `CROWN DROPPED` → `RAJA CARRIER`; A4 MAYA healed 26→65→72 with no ticket
   spend (live healing path working); player HP 85→73→54 (enemy AI engages
   the human); Aandhi white boundary ring visibly closes in.
7. Results at 04:02 (`15-ko-watch.png`, SHA-256
   `BCDD7B56B0CCC18C1B0FF4A0A8A8C39303002CBCF05B6D91B3D21507A5DA3D64`):
   `BASTION CROWN RESULTS / WINNER TEAM RAJA • Clock / RAJA 3 DEPOSITS 0
   KOs 3 TICKETS 11 DMG 487 HEAL 546 OBJ 62.6s GAD 0 ABIL 39 / RIVAL 2
   DEPOSITS 0 KOs 2 TICKETS 9 DMG 805 HEAL 178 OBJ 164.2s GAD 1 ABIL 156`,
   with REMATCH / MENU. Zero deposits either side with 62.6 s / 164.2 s
   objective time: objective conversion stays a tuning watch-item, consistent
   with earlier bot-batch variance notes.
8. REMATCH → fresh live match at 00:12 (`16-rematch-live.png`): scores and
   tickets reset (0/15, 12/12), new seed. Spawn-pressure note: at 00:12 the
   player was already 61/85 and A4 60/72.
9. PAUSE → settings sheet (`17-pause-settings.png`): `LEFT-HANDED ON`,
   `REDUCED FLASHES ON`, `HIGH CONTRAST ON`, `AIM ASSIST ON`, `TEXT -/+`,
   `RETURN TO MENU`, `CLOSE`, all with explicit ON labels and icon tiles.
10. HOME → launcher (`18-home-screen.png`) → relaunch → settings sheet intact,
    clock frozen at 01:28 (`19-resume-after-home.png`): pause/lifecycle holds
    the sim; no input leak observed on return.
11. CLOSE → live match resumed at 01:32 (`20-settings-closed.png`).

## Technical evidence

- Scoped logcat `logcat-final.txt` (SHA-256
  `F124E18A1463BEF3CBF29518F5AF4F079FF0A66ECC10E6FD0F43413AE53979D2`):
  **0** `FATAL EXCEPTION`, **0** `ANR in`, **0** `SIGSEGV`, **0** `SIGABRT`,
  **0** `NullReferenceException`, **0** `UnityException`,
  **0** `SetLODs: Attempting to force`.
- Point performance samples (meminfo, exact APK, live match):
  mid-match PSS **288,429 KB**, RSS **427,804 KB**, graphics **91,660 KB**,
  instantaneous app CPU **132%**, thermal **0**; post-rematch PSS **232,879 KB**,
  RSS **383,060 KB**, thermal **0**, battery 38% / 32 °C.
- `gfxinfo` reports `Total frames rendered: 0`; `SurfaceFlinger --latency`
  returned only the refresh period. No FPS/frame-pacing claim is made — the
  same Unity-surface tooling limitation as every prior pass.
- Static validation `Tools/Validation/validate.ps1 -RequireUnityProject`:
  **0 errors / 0 warnings** (re-ran in this pass). EditMode **173/173** and
  PlayMode **100/100** are carried from the exact-source checkpoint
  (`V1_AUTHORITY_PICKUP_TERMINAL_BOUNDARIES_2026-09-05.md`); no code changed,
  so no test/build gate was re-run.

## Limitations and open items (unchanged owner gates)

- Player KO → spectator → respawn not re-observed here; covered by prior
  exact-APK evidence, still needs routine re-proof.
- Human Bijli/Maya, human use of all three gadgets, Crown deposit by the
  player, overtime match, 10-rematch endurance, normalized GPU/GC/endurance
  profiling, physical 16 KB runtime, commissioned final art/audio, full
  accessibility/cultural/fun review, permanent identity/signing, privacy/Data
  Safety/IARC/cultural/Play approvals remain open.
- Battery was 38% USB-powered, and the disallowed Oppo shared the ADB bus
  (unused); mobile-mcp init was unusable with two devices, so the pass used
  explicit `adb -s ST5GDW23LB004392` throughout.

## Key capture hashes

- `08-live-early.png` `4575853C6DE028EE216BC86025FD2C713504A1B11F888656937D19393E7CBB8D`
- `13-combat-round2.png` `9B87A43A330C5C8B7D27DD5CB09CA3C622622EE1AB6ADEE08D943B8F2509EFA7`
- `14-exit-push.png` `02455EB3A6D9BFEEA7D96796CDF2FA7118C9FA6E5583516FD68097F0D1758FBA`
- `15-ko-watch.png` `BCDD7B56B0CCC18C1B0FF4A0A8A8C39303002CBCF05B6D91B3D21507A5DA3D64`
- `16-rematch-live.png` `3F48D3B7963108B8E66E2551FD3E6C9AE5F8DFA92450E73CA7E86C594ACFC4DD`
- `17-pause-settings.png` `4209BA7F62013A4B58676A35DF49491D039E32A55F93318A5F4854BB9090361C`
- `19-resume-after-home.png` `08BDE6335981C9914F3C72DBB69A1C0EB01F84B84344DC5C580BE72FE6AC73F7`
- `20-settings-closed.png` `1BA73B6D861999C4AA485715F8316D489CAE3B5B6E8ABAACCB5DDCFDD966CA7F`
