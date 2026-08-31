using System;
using System.Collections.Generic;
using BattleRaja.Core.Domain;
using NUnit.Framework;

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
            Assert.That(match.TryGetParticipant(new CombatEntityId(5), out var respawned), Is.True);
            Assert.That(respawned.Alive, Is.True);
            Assert.That(respawned.SpawnProtected, Is.True);
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
    }
}
