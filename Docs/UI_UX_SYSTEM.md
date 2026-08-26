# BattleRaja V1 UI/UX System

**Status:** Functional offline flow and accessibility settings exist; touch-ready visual polish
and human approval remain open.

## Hierarchy

The player-facing flow is Main Menu → Offline Play → Fighter Selection → Tutorial or Bazaar
match → Spectator/Results → Rematch. Internal IDs, authority terminology, keyboard-only
instructions and online-service navigation must not appear in the player surfaces.

## Visual language

- Warm parchment/sand surfaces with deep teal ink and saffron action accents.
- One primary action per panel; destructive/back actions are visually secondary.
- Combat HUD keeps health, alive count, Aandhi state and action cooldowns inside the safe area.
- Buttons expose readable ready, pressed, disabled and cooldown states.
- Fighter cards use the saved fighter identities, never concept art presented as gameplay.

## Accessibility contract

The settings surface exposes left-handed controls, high contrast, reduced flashes, aim assist,
haptics, music and effects volume. The implementation must preserve readable action labels,
safe-area layout, large touch targets and text reflow when scaling is tested on Lava.

## Remaining human verification

- Portrait and landscape choice requires owner approval after physical comparison.
- Complete tutorial and at least three match/rematch cycles through real touch input.
- Check fighter cards, HUD occlusion, results/rematch clarity, settings persistence,
  reduced-motion comfort and accessibility contrast on the approved Lava device.
