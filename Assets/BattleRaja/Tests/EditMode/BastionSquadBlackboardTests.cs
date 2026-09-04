using System.Collections.Generic;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class BastionSquadBlackboardTests
    {
        private static BastionCrownMatch Start(uint seed = 91u)
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
                var team = i < BastionCrownMatch.TeamSize ? BastionTeamId.Raja : BastionTeamId.Rival;
                var role = fighters[i].Equals(FighterDefinition.Pehel.FighterId)
                    ? BastionRole.Anchor
                    : fighters[i].Equals(FighterDefinition.Maya.FighterId)
                        ? BastionRole.Runner
                        : BastionRole.Skirmisher;
                var member = new TeamMember(
                    new CombatEntityId(actorId),
                    team,
                    fighters[i],
                    role,
                    actorId == 1);
                var x = i < BastionCrownMatch.TeamSize ? -10f : 10f;
                var z = (i % BastionCrownMatch.TeamSize - 1.5f) * 2.5f;
                slots.Add(new BastionCrownSlot(member, new Float2(x, z), 100));
            }

            var match = new BastionCrownMatch(seed);
            match.Start(slots);
            match.Advance(3.1f);
            return match;
        }

        [Test]
        public void SharedPlansRefreshOnceAndSignalAgeStaysBounded()
        {
            var match = Start();
            match.PrepareSquadIntents(10);
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(2), out var first), Is.True);
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(3), out var second), Is.True);
            Assert.That(first.Plan, Is.EqualTo(BastionSquadPlan.DefendShrine));
            Assert.That(second.Plan, Is.EqualTo(BastionSquadPlan.ContestCrown));

            match.PrepareSquadIntents(11);
            match.PrepareSquadIntents(12);
            match.PrepareSquadIntents(13);
            var bounded = match.SquadMetrics;
            Assert.That(bounded.SignalUpdates, Is.EqualTo(1));
            Assert.That(bounded.PlanRefreshes, Is.EqualTo(1));
            Assert.That(bounded.MaxSignalAgeTicks, Is.LessThan(BastionSquadBlackboard.DefaultCommunicationDelayTicks));

            match.PrepareSquadIntents(14);
            Assert.That(match.SquadMetrics.SignalUpdates, Is.EqualTo(2));
            Assert.That(match.SquadMetrics.PlanRefreshes, Is.EqualTo(2));
        }

        [Test]
        public void LowHealthTeammateGetsOneDeterministicSupportAssignment()
        {
            var match = Start();
            var targetId = new CombatEntityId(3);
            match.SetHealth(targetId, 40);
            match.PrepareSquadIntents(20, true);
            Assert.That(match.TryGetParticipant(targetId, out var target), Is.True);

            var supportCount = 0;
            var supporterId = 0;
            for (var actorId = 1; actorId <= BastionCrownMatch.TeamSize; actorId++)
            {
                Assert.That(match.TryGetSquadIntent(new CombatEntityId(actorId), out var intent), Is.True);
                // The base planner may name the weakest teammate as a
                // potential support target for several actors. The blackboard
                // handoff is the one intent whose movement destination is
                // rewritten to that teammate's canonical position.
                if (intent.SupportTargetId != targetId || !intent.Destination.Equals(target.Position)) continue;
                supportCount++;
                supporterId = actorId;
            }

            Assert.That(supportCount, Is.EqualTo(1));
            Assert.That(supporterId, Is.EqualTo(2), "Anchor is the deterministic support/peel handoff.");
            Assert.That(match.SquadMetrics.SupportAssignments, Is.EqualTo(1));
        }

        [Test]
        public void CrownCarrierChangeProducesAnEscortHandoffMetric()
        {
            var match = Start();
            var crownPosition = match.Crown.Position;
            match.SetPosition(new CombatEntityId(3), crownPosition);
            Assert.That(match.TryPickupCrown(new CombatEntityId(3), 0.25f), Is.True);
            match.PrepareSquadIntents(30, true);
            var before = match.SquadMetrics.EscortHandoffs;

            match.DropCrown(new CombatEntityId(3));
            match.Advance(1.26f);
            match.SetPosition(new CombatEntityId(4), match.Crown.Position);
            Assert.That(match.TryPickupCrown(new CombatEntityId(4), 0.25f), Is.True);
            match.PrepareSquadIntents(31, true);

            Assert.That(match.SquadMetrics.EscortHandoffs, Is.GreaterThan(before));
            Assert.That(match.SquadMetrics.EscortAssignments, Is.GreaterThan(0));
        }

        [Test]
        public void CommandWindowKeepsOneSharedSnapshotAfterStateMutation()
        {
            var match = Start();
            match.PrepareSquadIntents(0, true);
            var before = match.SquadMetrics;
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(3), out var prepared), Is.True);

            // The controller opens the callback window before bots are asked for
            // commands. A state mutation from one callback must not cause the
            // next teammate to observe a force-refreshed plan at the same tick.
            match.SetPosition(new CombatEntityId(3), new Float2(0f, 0f));
            match.BeginSquadCommandPhase(1);
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(3), out var first), Is.True);
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(4), out var second), Is.True);

            var duringWindow = match.SquadMetrics;
            Assert.That(duringWindow.SignalUpdates, Is.EqualTo(before.SignalUpdates));
            Assert.That(duringWindow.PlanRefreshes, Is.EqualTo(before.PlanRefreshes));
            Assert.That(first.Movement, Is.EqualTo(prepared.Movement));
            Assert.That(second.Plan, Is.EqualTo(BastionSquadPlan.ContestCrown));
            match.EndSquadCommandPhase(1);

            // Outside the controller-owned window, pure-domain callers may ask
            // for a fresh plan immediately after the mutation.
            match.SetPosition(new CombatEntityId(2), new Float2(-8f, 0f));
            Assert.That(match.TryGetSquadIntent(new CombatEntityId(2), out _), Is.True);
            Assert.That(match.SquadMetrics.SignalUpdates, Is.EqualTo(before.SignalUpdates + 1));
        }
    }
}
