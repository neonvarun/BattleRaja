using System.Collections.Generic;
using BattleRaja.Core.Application;
using BattleRaja.Core.Domain;
using NUnit.Framework;

namespace BattleRaja.Tests.EditMode
{
    public sealed class AuthorityFoundationTests
    {
        [Test]
        public void MatchAuthorityEmitsZoneDamageIntentsOutsideTheCurrentZone()
        {
            var authority = new OfflineMatchAuthority(OfflineMatchDefinition.SoloRaja, 1f);
            authority.Start(new List<MatchSpawn>
            {
                new MatchSpawn(new CombatEntityId(1), new Float2(0f, 0f), 100),
                new MatchSpawn(new CombatEntityId(2), new Float2(8f, 0f), 100)
            });
            authority.SetPosition(new CombatEntityId(1), new Float2(15f, 0f));

            authority.Advance(8f);
            var tick = authority.Advance(1f);

            Assert.That(tick.Result.OutsideDamagePerSecond, Is.EqualTo(5));
            Assert.That(tick.OutsideDamageRequests, Has.Length.EqualTo(1));
            Assert.That(tick.OutsideDamageRequests[0].TargetId.Value, Is.EqualTo(1));
            Assert.That(tick.OutsideDamageRequests[0].DamageType, Is.EqualTo(DamageType.Aandhi));
        }
    }
}
