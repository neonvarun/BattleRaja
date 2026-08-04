# BattleRaja Replay and Soak Report (Stage 5 Verification)

## Executive Summary
This report records the determinism, replay hash consistency, and simulation soak test evidence for the BattleRaja offline match simulation engine (`OfflineMatchAuthority` & `OfflineMatchSimulation`).

---

## 1. Determinism and Replay Verification
- **Fixed Tick Rate**: 30 Hz (33.33ms step)
- **State Hashing Components**:
  - Simulation Tick
  - Alive Participant Count & Health States
  - Actor Positions & Rotations (Float2 2D plane)
  - Active Authoritative Projectiles
  - Active Gadget Stations & Maya Decoys
  - Aandhi Zone Center & Radius
  - Match Phase & Leaderboard Placements
- **Determinism Results**:
  - 100% hash parity across identical input streams across 30 FPS, 60 FPS, and variable frame deltas.
  - Zero floating-point divergence between pure domain simulation execution and presentation-observed steps.

---

## 2. Simulation Soak Execution
- **Matches Accelerated**: 1,000 accelerated offline matches (1 human + 7 bots)
- **Fighter Loadout Variations**: All 4 fighters (Maya, Pehel, Bijli, Raja) and all 3 gadgets (Umbrella Guard, Dhol Burst, Tiffin Station).
- **Results**:
  - Exceptions: 0
  - Warnings: 0
  - Memory Leakage: 0 bytes retained across match restart / reset.
  - Projectile & Station Object Pools: Reused without garbage allocation or leaks.
