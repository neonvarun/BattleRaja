using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;
using NUnit.Framework;
using UnityEngine;

namespace BattleRaja.Tests.EditMode
{
    public sealed class BastionCrownMatchTests
    {
        private static List<BastionCrownSlot> CreateSlots()
        {
            var slots = new List<BastionCrownSlot>(BastionCrownMatch.ParticipantCount);
            var fighters = new[]
            {
                FighterDefinition.Bijli.FighterId,
                FighterDefinition.Pehel.FighterId,
                FighterDefinition.Maya.FighterId,
                FighterDefinition.Bijli.FighterId,
                FighterDefinition.Pehel.FighterId,
                FighterDefinition.Maya.FighterId,
                FighterDefinition.Bijli.FighterId,
                FighterDefinition.Pehel.FighterId
            };

            for (var i = 0; i < fighters.Length; i++)
            {
                var actorId = i + 1;
                var team = i < 4 ? BastionTeamId.Raja : BastionTeamId.Rival;
                var role = fighters[i].Equals(FighterDefinition.Pehel.FighterId)
                    ? BastionRole.Anchor
                    : fighters[i].Equals(FighterDefinition.Maya.FighterId) ? BastionRole.Runner : BastionRole.Skirmisher;
                var member = new TeamMember(new CombatEntityId(actorId), team, fighters[i], role, actorId == 1);
                var x = i < 4 ? -10f : 10f;
                var z = (i % 4 - 1.5f) * 2.5f;
                slots.Add(new BastionCrownSlot(member, new Float2(x, z), 100));
            }

            return slots;
        }

        private static BastionCrownMatch Start(uint seed = 42u)
        {
            var match = new BastionCrownMatch(seed);
            match.Start(CreateSlots());
            match.Advance(3.1f);
            return match;
        }

        [Test]
        public void StartRequiresExactCanonicalComposition()
        {
            var match = new BastionCrownMatch(1u);
            Assert.Throws<ArgumentException>(() => match.Start(CreateSlots().GetRange(0, 7)));

            var invalid = CreateSlots();
            var rivalAsHuman = new TeamMember(new CombatEntityId(2), BastionTeamId.Raja, FighterDefinition.Pehel.FighterId, BastionRole.Anchor, true);
            invalid[1] = new BastionCrownSlot(rivalAsHuman, invalid[1].SpawnPosition, 100);
            Assert.Throws<ArgumentException>(() => new BastionCrownMatch(2u).Start(invalid));
        }

        [Test]
        public void TeamRelationshipsAndTicketPoolsAreExplicit()
        {
            var match = Start();
            Assert.That(match.GetTeam(new CombatEntityId(1)), Is.EqualTo(BastionTeamId.Raja));
            Assert.That(match.AreAllies(new CombatEntityId(1), new CombatEntityId(4)), Is.True);
            Assert.That(match.AreEnemies(new CombatEntityId(1), new CombatEntityId(5)), Is.True);
            Assert.That(match.GetTickets(BastionTeamId.Raja).Remaining, Is.EqualTo(12));
            Assert.That(match.GetTickets(BastionTeamId.Rival).Remaining, Is.EqualTo(12));
        }

        [Test]
        public void FriendlyFireAndSpawnProtectionRejectWithoutMutation()
        {
            var match = Start();
            var ally = match.ApplyDamage(new DamageRequest(new CombatEntityId(1), new CombatEntityId(2), CombatFaction.Player, 20, DamageType.Projectile), 1);
            Assert.That(ally.RejectionReason, Is.EqualTo(DamageRejectionReason.FriendlyFire));

            var protectedTarget = match.ApplyDamage(new DamageRequest(new CombatEntityId(5), new CombatEntityId(1), CombatFaction.Enemy, 20, DamageType.Projectile), 2);
            Assert.That(protectedTarget.RejectionReason, Is.EqualTo(DamageRejectionReason.SpawnProtection));
            Assert.That(match.TryGetParticipant(new CombatEntityId(1), out var snapshot), Is.True);
            Assert.That(snapshot.CurrentHealth, Is.EqualTo(100));
        }

        [Test]
        public void CrownPickupDropLockAndDepositAwardScoreOnce()
        {
            var match = Start(5u);
            var crown = match.Crown;
            var initialSocket = crown.SocketIndex;
            match.SetPosition(new CombatEntityId(1), crown.Position);
            Assert.That(match.TryPickupCrown(new CombatEntityId(1), 0.10f), Is.False);
            Assert.That(match.TryPickupCrown(new CombatEntityId(1), 0.15f), Is.True);
            Assert.That(match.Crown.CarrierId.Value, Is.EqualTo(1));

            match.DropCrown(new CombatEntityId(1));
            Assert.That(match.Crown.Dropped, Is.True);
            Assert.That(match.TryPickupCrown(new CombatEntityId(2), 0.3f), Is.False);
            match.Advance(1.25f);
            match.SetPosition(new CombatEntityId(2), match.Crown.Position);
            Assert.That(match.TryPickupCrown(new CombatEntityId(2), 0.25f), Is.True);
            match.SetPosition(new CombatEntityId(2), ModeDefinition.BastionCrown.Raja.ShrinePosition);
            Assert.That(match.TryBeginDeposit(new CombatEntityId(2)), Is.True);
            match.Advance(1.25f);
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).Score, Is.EqualTo(3));
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).Deposits, Is.EqualTo(1));
            Assert.That(match.Crown.IsCarried, Is.False);
            Assert.That(match.Crown.SocketIndex, Is.EqualTo((initialSocket + 1) % 3));
        }

        [Test]
        public void DamageInterruptsDepositBeforeChannelCanComplete()
        {
            var match = Start(7u);
            var carrierId = new CombatEntityId(1);
            var attackerId = new CombatEntityId(5);
            match.ClearSpawnProtection(carrierId);
            match.ClearSpawnProtection(attackerId);
            match.SetPosition(carrierId, match.Crown.Position);
            Assert.That(match.TryPickupCrown(carrierId, 0.25f), Is.True);
            match.SetPosition(carrierId, ModeDefinition.BastionCrown.Raja.ShrinePosition);
            Assert.That(match.TryBeginDeposit(carrierId), Is.True);
            match.Advance(0.50f);

            var damage = match.ApplyDamage(new DamageRequest(
                attackerId,
                carrierId,
                CombatFaction.Enemy,
                1,
                DamageType.Projectile),
                101);
            Assert.That(damage.Applied, Is.True);
            Assert.That(match.Crown.ChannelActorId.Value, Is.EqualTo(0));
            match.Advance(1.0f);
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).Deposits, Is.EqualTo(0));
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).Score, Is.EqualTo(0));
        }

        [Test]
        public void HealingAndActionUsageAreAttributedOnce()
        {
            var match = Start(9u);
            var healer = new CombatEntityId(2);
            var target = new CombatEntityId(1);
            match.SetHealth(target, 50);
            Assert.That(match.NotifyHealing(healer, target, 10, 31), Is.True);
            Assert.That(match.NotifyHealing(healer, target, 10, 31), Is.False);
            Assert.That(match.TryGetParticipant(healer, out var healerSnapshot), Is.True);
            Assert.That(healerSnapshot.HealingDone, Is.EqualTo(10));
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).HealingDone, Is.EqualTo(10));

            Assert.That(match.RecordGadgetUse(healer, 41), Is.True);
            Assert.That(match.RecordGadgetUse(healer, 41), Is.False);
            Assert.That(match.RecordAbilityUse(healer, 51), Is.True);
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).GadgetUses, Is.EqualTo(1));
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).AbilityUses, Is.EqualTo(1));
        }

        [Test]
        public void SquadPlannerAssignsRolesAndSpacingFromCanonicalState()
        {
            var match = Start(11u);
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(2), out var anchor), Is.True);
            Assert.That(anchor.Plan, Is.EqualTo(BastionSquadPlan.DefendShrine));
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(3), out var runner), Is.True);
            Assert.That(runner.Plan, Is.EqualTo(BastionSquadPlan.ContestCrown));

            var carrierId = new CombatEntityId(3);
            match.SetPosition(carrierId, match.Crown.Position);
            Assert.That(match.TryPickupCrown(carrierId, 0.25f), Is.True);
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(4), out var escort), Is.True);
            Assert.That(escort.Plan, Is.EqualTo(BastionSquadPlan.EscortCarrier));
            Assert.That(match.TryGetParticipant(carrierId, out var carrier), Is.True);
            Assert.That(escort.Destination, Is.EqualTo(carrier.Position));
            Assert.That(match.TryGetSquadIntent(carrierId, out var carrierIntent), Is.True);
            Assert.That(carrierIntent.Plan, Is.EqualTo(BastionSquadPlan.EscortCarrier));
            Assert.That(carrierIntent.Destination, Is.EqualTo(ModeDefinition.BastionCrown.Raja.ShrinePosition));
        }

        [Test]
        public void SquadPlannerRetreatsFromClosingAandhiWithoutChangingObjectiveState()
        {
            var match = Start(13u);
            var anchorId = new CombatEntityId(2);
            match.SetPosition(anchorId, new Float2(-10f, 0f));
            Assert.That(match.SyncAandhi(Float2.Zero, 2f, AandhiState.Closing), Is.True);

            Assert.That(match.TryGetSquadIntent(anchorId, out var intent), Is.True);
            Assert.That(intent.Plan, Is.EqualTo(BastionSquadPlan.RetreatFromAandhi));
            Assert.That(intent.Destination, Is.EqualTo(Float2.Zero));
            Assert.That(intent.Movement.SqrMagnitude, Is.GreaterThan(0.1f));
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).Score, Is.EqualTo(0));
        }

        [Test]
        public void SquadPlannerMetricsCoverObjectiveEscortDefenseCollapseAndRetreat()
        {
            var counts = new int[7];
            const int seedCount = 32;
            for (var seed = 101u; seed < 101u + seedCount; seed++)
            {
                var match = Start(seed);
                RecordPlan(match, new CombatEntityId(2), counts);
                RecordPlan(match, new CombatEntityId(3), counts);
                RecordPlan(match, new CombatEntityId(4), counts);

                var crownPosition = match.Crown.Position;
                match.SetPosition(new CombatEntityId(3), crownPosition);
                Assert.That(match.TryPickupCrown(new CombatEntityId(3), 0.25f), Is.True);
                RecordPlan(match, new CombatEntityId(2), counts);
                RecordPlan(match, new CombatEntityId(3), counts);
                RecordPlan(match, new CombatEntityId(4), counts);

                match.DropCrown(new CombatEntityId(3));
                match.Advance(1.26f);
                match.SetPosition(new CombatEntityId(5), match.Crown.Position);
                Assert.That(match.TryPickupCrown(new CombatEntityId(5), 0.25f), Is.True);
                RecordPlan(match, new CombatEntityId(2), counts);
                RecordPlan(match, new CombatEntityId(3), counts);
                RecordPlan(match, new CombatEntityId(4), counts);

                match.SetPosition(new CombatEntityId(4), new Float2(20f, 0f));
                Assert.That(match.SyncAandhi(Float2.Zero, 5f, AandhiState.Closing), Is.True);
                RecordPlan(match, new CombatEntityId(4), counts);
            }

            Debug.Log($"Bastion squad planner metrics: seeds={seedCount} contest={counts[(int)BastionSquadPlan.ContestCrown]} escort={counts[(int)BastionSquadPlan.EscortCarrier]} defend={counts[(int)BastionSquadPlan.DefendShrine]} collapse={counts[(int)BastionSquadPlan.CollapseTarget]} retreat={counts[(int)BastionSquadPlan.RetreatFromAandhi]}");
            Assert.That(counts[(int)BastionSquadPlan.ContestCrown], Is.GreaterThan(0));
            Assert.That(counts[(int)BastionSquadPlan.EscortCarrier], Is.GreaterThan(0));
            Assert.That(counts[(int)BastionSquadPlan.DefendShrine], Is.GreaterThan(0));
            Assert.That(counts[(int)BastionSquadPlan.CollapseTarget], Is.GreaterThan(0));
            Assert.That(counts[(int)BastionSquadPlan.RetreatFromAandhi], Is.EqualTo(seedCount));
        }

        private static void RecordPlan(BastionCrownMatch match, CombatEntityId actorId, int[] counts)
        {
            Assert.That(match.TryGetSquadIntent(actorId, out var intent), Is.True);
            counts[(int)intent.Plan]++;
        }

        [Test]
        public void KnockoutCreditsOneScoreAndAssistIsNotDoubleCounted()
        {
            var match = Start();
            match.ClearSpawnProtection(new CombatEntityId(1));
            match.ClearSpawnProtection(new CombatEntityId(2));
            match.ClearSpawnProtection(new CombatEntityId(5));
            var first = match.ApplyDamage(new DamageRequest(new CombatEntityId(1), new CombatEntityId(5), CombatFaction.Player, 40, DamageType.Projectile), 10);
            Assert.That(first.Applied, Is.True);
            var assist = match.ApplyDamage(new DamageRequest(new CombatEntityId(2), new CombatEntityId(5), CombatFaction.Player, 60, DamageType.Projectile), 11);
            Assert.That(assist.TargetDefeated, Is.True);
            var duplicate = match.NotifyCombatDamage(new CombatEntityId(2), new CombatEntityId(5), 60, true, 11);
            Assert.That(duplicate, Is.False);
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).Score, Is.EqualTo(1));
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).KOs, Is.EqualTo(1));
            Assert.That(match.TryGetParticipant(new CombatEntityId(1), out var assister), Is.True);
            Assert.That(assister.Assists, Is.EqualTo(1));
        }

        [Test]
        public void RejectedDamageAgainstDeadTargetDoesNotConsumeEventIdentity()
        {
            var match = Start();
            match.ClearSpawnProtection(new CombatEntityId(1));
            match.ClearSpawnProtection(new CombatEntityId(5));
            var defeated = match.ApplyDamage(new DamageRequest(new CombatEntityId(1), new CombatEntityId(5), CombatFaction.Player, 100, DamageType.Projectile), 30);
            Assert.That(defeated.TargetDefeated, Is.True);

            Assert.That(match.NotifyCombatDamage(new CombatEntityId(1), new CombatEntityId(5), 5, false, 31), Is.False);
            var respawnTick = match.Advance(5f);
            Assert.That(Array.IndexOf(respawnTick.RespawnedActors, new CombatEntityId(5)), Is.GreaterThanOrEqualTo(0));
            Assert.That(match.ConfirmRespawn(new CombatEntityId(5)), Is.True);
            match.ClearSpawnProtection(new CombatEntityId(5));

            Assert.That(match.NotifyCombatDamage(new CombatEntityId(1), new CombatEntityId(5), 5, false, 31), Is.True);
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).DamageDealt, Is.EqualTo(105));
        }

        [Test]
        public void DefeatConsumesTicketOnFiveSecondRespawnAndReturnsProtected()
        {
            var match = Start();
            match.ClearSpawnProtection(new CombatEntityId(1));
            match.ClearSpawnProtection(new CombatEntityId(5));
            var defeated = match.ApplyDamage(new DamageRequest(new CombatEntityId(1), new CombatEntityId(5), CombatFaction.Player, 100, DamageType.Projectile), 20);
            Assert.That(defeated.TargetDefeated, Is.True);
            Assert.That(match.TryGetParticipant(new CombatEntityId(5), out var dead), Is.True);
            Assert.That(dead.RespawnPending, Is.True);
            Assert.That(match.GetTickets(BastionTeamId.Rival).Remaining, Is.EqualTo(12));
            var tick = match.Advance(5f);
            Assert.That(Array.IndexOf(tick.RespawnedActors, new CombatEntityId(5)), Is.GreaterThanOrEqualTo(0));
            Assert.That(match.GetTickets(BastionTeamId.Rival).Remaining, Is.EqualTo(11));
            Assert.That(match.TryGetParticipant(new CombatEntityId(5), out var ready), Is.True);
            Assert.That(ready.Alive, Is.False);
            Assert.That(ready.RespawnPending, Is.True);
            Assert.That(match.ConfirmRespawn(new CombatEntityId(5)), Is.True);
            Assert.That(match.TryGetParticipant(new CombatEntityId(5), out var respawned), Is.True);
            Assert.That(respawned.Alive, Is.True);
            Assert.That(respawned.SpawnProtected, Is.True);
            Assert.That(match.ConfirmRespawn(new CombatEntityId(5)), Is.False);
        }

        [Test]
        public void RespawnRequiresAuthorityConfirmationAndRetriesWithoutDoubleSpending()
        {
            var match = Start();
            match.ClearSpawnProtection(new CombatEntityId(1));
            match.ClearSpawnProtection(new CombatEntityId(5));
            var defeated = match.ApplyDamage(new DamageRequest(
                new CombatEntityId(1),
                new CombatEntityId(5),
                CombatFaction.Player,
                100,
                DamageType.Projectile),
                61);
            Assert.That(defeated.TargetDefeated, Is.True);

            Assert.That(match.TryGetParticipant(new CombatEntityId(5), out var before), Is.True);
            var rejectedPosition = new Float2(12f, 12f);
            Assert.That(match.SyncParticipant(new CombatEntityId(5), rejectedPosition, 100, true), Is.False);
            Assert.That(match.TryGetParticipant(new CombatEntityId(5), out var stillPending), Is.True);
            Assert.That(stillPending.Alive, Is.False);
            Assert.That(stillPending.Position, Is.EqualTo(before.Position));
            Assert.That(match.ConfirmRespawn(new CombatEntityId(5)), Is.False);

            var readyTick = match.Advance(5f);
            Assert.That(Array.IndexOf(readyTick.RespawnedActors, new CombatEntityId(5)), Is.GreaterThanOrEqualTo(0));
            Assert.That(match.GetTickets(BastionTeamId.Rival).Remaining, Is.EqualTo(11));
            Assert.That(match.TryGetParticipant(new CombatEntityId(5), out var ready), Is.True);
            Assert.That(ready.Alive, Is.False);
            Assert.That(ready.RespawnRemaining, Is.EqualTo(0f));

            var retryTick = match.Advance(0.1f);
            Assert.That(Array.IndexOf(retryTick.RespawnedActors, new CombatEntityId(5)), Is.GreaterThanOrEqualTo(0));
            Assert.That(match.GetTickets(BastionTeamId.Rival).Remaining, Is.EqualTo(11));
            Assert.That(match.ConfirmRespawn(new CombatEntityId(5)), Is.True);
            Assert.That(match.ConfirmRespawn(new CombatEntityId(5)), Is.False);

            var postConfirmTick = match.Advance(0.1f);
            Assert.That(Array.IndexOf(postConfirmTick.RespawnedActors, new CombatEntityId(5)), Is.LessThan(0));
            Assert.That(match.GetTickets(BastionTeamId.Rival).Remaining, Is.EqualTo(11));
        }

        [Test]
        public void AuthorityDamageMirrorRejectsReadyAndTerminalDelivery()
        {
            var preLive = new BastionCrownMatch(37u);
            preLive.Start(CreateSlots());
            Assert.That(preLive.NotifyCombatDamage(
                new CombatEntityId(1),
                new CombatEntityId(5),
                10,
                false,
                71), Is.False);
            Assert.That(preLive.TryGetParticipant(new CombatEntityId(5), out var readyTarget), Is.True);
            Assert.That(readyTarget.CurrentHealth, Is.EqualTo(100));

            var match = Start(39u);
            match.ClearSpawnProtection(new CombatEntityId(5));
            match.ForceResolve(BastionTeamId.Raja, BastionMatchResultReason.Clock);
            Assert.That(match.NotifyCombatDamage(
                new CombatEntityId(1),
                new CombatEntityId(5),
                10,
                false,
                72), Is.False);
            Assert.That(match.TryGetParticipant(new CombatEntityId(5), out var terminalTarget), Is.True);
            Assert.That(terminalTarget.CurrentHealth, Is.EqualTo(100));
            Assert.That(match.GetTeamScore(BastionTeamId.Raja).DamageDealt, Is.EqualTo(0));
        }

        [Test]
        public void ClockTieEntersDeterministicOvertimeAndCapCanDraw()
        {
            var match = Start();
            var tick = match.Advance(240f);
            Assert.That(tick.Overtime, Is.True);
            Assert.That(match.IsEnded, Is.False);
            tick = match.Advance(30f);
            Assert.That(tick.MatchEnded, Is.True);
            Assert.That(tick.Result.IsDraw, Is.True);
            Assert.That(tick.Result.Reason, Is.EqualTo(BastionMatchResultReason.Draw));
        }

        [Test]
        public void CrownRotationPreservesOverdueTimeAcrossCoarseAdvance()
        {
            var coarse = Start(17u);
            var fixedStep = Start(17u);

            coarse.Advance(105f, 105);
            for (var tick = 1; tick <= 105; tick++) fixedStep.Advance(1f, tick);

            Assert.That(coarse.Crown.SocketIndex, Is.EqualTo(fixedStep.Crown.SocketIndex));
            Assert.That(coarse.Crown.Position, Is.EqualTo(fixedStep.Crown.Position));
            Assert.That(coarse.Crown.RotationRemaining, Is.EqualTo(fixedStep.Crown.RotationRemaining).Within(0.0001f));
            Assert.That(coarse.CalculateDeterministicHash(), Is.EqualTo(fixedStep.CalculateDeterministicHash()));
        }
    }
}
