# Bastion Crown V1 product brief

**Mode ID:** `BR_BastionCrown_V1`
**Primary player flow:** cold launch → `PLAY OFFLINE` → Bastion Crown briefing → fighter choice → ready → match → brief spectate/result → rematch or menu.
**Roster:** Bijli (mobile skirmisher), Pehel (frontline bruiser), Maya (trickster/control).
**Composition:** Team Raja is actor 1 human plus actors 2–4 friendly AI. Rival is actors 5–8 rival AI. Exactly eight slots are valid.
**Network boundary:** no login, matchmaking, Photon, PlayFab, ads, IAP, cloud progression or internet requirement.

## Player promise

Bastion Crown is a short, replayable objective battle in an original bazaar-fortress. Fighters contest a neutral Crown Spark, escort it through readable lanes and deposit it at their shrine while KOs and shared tickets create room for recovery. Aandhi compresses the final minute toward the active objective region. The mode is not a free-for-all with recoloured actors and does not copy a reference game's rules or trade dress.

## Canonical vocabulary

| Term | Player meaning |
|---|---|
| Team Raja | The player's team: actor 1 and three allied bots |
| Rival | The opposing four-bot team |
| Crown Spark | Neutral objective that rotates between three sockets |
| Shrine | A team's deposit anchor |
| Ticket | One shared team respawn resource |
| Deposit | A completed 1.25-second Crown channel at the allied shrine (+3) |
| KO | A confirmed enemy defeat (+1) |
| Assist | Meaningful prior damage contribution; no second KO score |
| Aandhi | The shrinking storm pressure in the final minute |

## State ownership

`ModeDefinition`, `TeamDefinition`, `TeamMember`, `ObjectiveDefinition`, `RespawnPolicy`, `TeamScore`, `TeamTicketPool` and `BastionResultSummary` are immutable domain contracts. `BastionCrownMatch` owns mutable participant, Crown, score, ticket, respawn, channel, overtime and result state. Unity views, UI, animation, VFX and audio mirror immutable ticks and never award score or revive actors.

## Canonical numbers

- Ready: 3 seconds.
- Live clock: 240 seconds; deterministic maximum overtime: 30 seconds.
- First to 15; otherwise score → deposits → KOs → tickets remaining → overtime Crown result; unresolved overtime is an explicit draw.
- Three Crown sockets; 0.25-second contact pickup; 12% carrier speed penalty; 35-second socket rotation; 6-second dropped lifetime with 1.25-second pickup lock.
- Shrine deposit: 1.25-second interruptible channel.
- Two shared pools of 12 tickets. A KO starts 4 seconds of spectator presentation and a 5-second respawn timer. A ticket is consumed only on the actual respawn.
- Respawn protection: 2.5 seconds, or until the respawned fighter deals damage. Protected actors cannot receive combat or Aandhi damage.

## Solo disposition

Solo Raja remains in the pure domain/application and replay fixtures as a compatibility/future route. It is not advertised by the primary V1 menu. Bastion Crown is the only player-facing production route in this rebuild; no public Online selector is exposed.

## Acceptance evidence

- Contract implementation: `Assets/BattleRaja/Core/Domain/BastionCrownContracts.cs` and `BastionCrownMatch.cs`.
- Pure regression coverage: `Assets/BattleRaja/Tests/EditMode/BastionCrownMatchTests.cs` (included in the 148-test EditMode run).
- Exact current audit: `Docs/AI/V1_EXECUTION_REBASELINE_2026-08-31.md`.
- Runtime integration and device evidence are intentionally tracked separately and may not be promoted until the full route is exercised on Lava `ST5GDW23LB004392`.
