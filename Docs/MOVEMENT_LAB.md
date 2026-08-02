# Movement Laboratory — Milestone 1

## Scope

This scene is a grey-box experiment for movement, aiming, camera readability and cross-platform input. It intentionally contains no attacks, projectiles, damage, abilities, bots, match flow, networking or final content.

Load `Assets/BattleRaja/Scenes/MovementLab/MovementLab.unity` with Unity `6000.5.6f1`.

## Current tuning

The shared asset is `Assets/BattleRaja/Content/Movement/M1-MovementTuning.asset`:

| Value | Current setting |
|---|---:|
| Maximum speed | 5.5 world units/second |
| Acceleration | 24 world units/second² |
| Deceleration | 30 world units/second² |
| Rotation speed | 720 degrees/second |
| Movement dead zone | 0.12 |
| Aim dead zone | 0.14 |
| Input sensitivity | 1.0 |

Runtime velocity and aim direction live in `MovementMotor`; the asset remains configuration only.

## Camera decision

The lab supports both orthographic and low-field-of-view perspective modes through `TopDownCameraController`, with orthographic selected by default:

- Orthographic keeps lane widths and aim telegraphs visually stable across the arena, avoids perspective distortion, and is inexpensive on Android/Web.
- Perspective provides stronger depth cues but introduces scale variation and more obstruction risk for a small competitive arena.
- The chosen orthographic setup uses an elevated offset `(0, 12, -8)`, smooth follow time `0.08s`, and orthographic size `9.5`.

This is a readability decision for the current 2.5D laboratory, not a final art or camera-shake decision. A human visual comparison is still required before camera settings are treated as final.

## Touch controls

- Left virtual stick: movement.
- Right virtual stick: aim.
- Radius: 92 UI units; control surface: 220 UI units; knob: 94 UI units.
- Stick opacity: 0.72 for the knob and approximately 0.18 for the surface.
- Safe-area anchoring updates when `Screen.safeArea` changes.
- Each stick owns one pointer ID and resets on disable or application focus loss.

## Desktop/Web controls

`Assets/BattleRaja/Content/Movement/BattleRajaMovement.inputactions` contains configurable WASD, arrow-key, mouse-position and gamepad-stick bindings. Escape releases pointer focus and clears touch/input state. The action adapter does not mutate transforms; it creates movement commands consumed by `MovementPlayerAgent`.

## Known limitations

- Physical touch interaction and safe-area validation across multiple aspect ratios still require manual device playtesting.
- Browser keyboard/mouse focus has been smoke-tested through the served page bootstrap, but deliberate canvas interaction and browser-scroll behavior need human validation.
- No formal Editor, Android GPU/CPU or Web performance capture has been collected.
- The placeholder arena and materials are grey-box only and must not be treated as final visual direction.

## Human playtest questions

1. Does orthographic framing make movement lanes and the aim indicator easier to read than the perspective alternative?
2. Does acceleration/deceleration feel responsive without making direction changes twitchy?
3. Is the aim indicator length and color readable on both the dark floor and wall materials?
4. Are the virtual sticks reachable without covering the player or obscuring the arena on the Lava and Oppo devices?
5. After rotating the device, backgrounding the app, or moving a finger between sticks, do controls reset and remain independent?
6. In Chrome and Edge, does pressing Escape release input focus without leaving movement stuck or allowing unwanted page scrolling?
