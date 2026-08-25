using System.Linq;
using BattleRaja.Core.Domain;
using BattleRaja.Presentation.Match;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.AI;
using UnityEngine;

namespace BattleRaja.Tests.PlayMode
{
    internal static class PlayModeTestHelpers
    {
        public static T FindPlayer<T>() where T : Component
        {
            var lab = Object.FindAnyObjectByType<MovementLabScene>();
            var authoredPlayer = lab != null ? lab.Player : null;
            if (authoredPlayer != null)
            {
                var authoredComponent = authoredPlayer.GetComponent<T>();
                if (authoredComponent != null) return authoredComponent;
            }

            return Object.FindObjectsByType<T>()
                .Where(component => component.GetComponent<MovementPlayerAgent>()?.ActorId == 1 ||
                                    component.GetComponentInParent<MovementPlayerAgent>()?.ActorId == 1)
                .OrderBy(component => component.GetComponent<MovementPlayerAgent>()?.ActorId ??
                                      component.GetComponentInParent<MovementPlayerAgent>()?.ActorId ?? int.MaxValue)
                .First();
        }

        public static void DisableBots()
        {
            foreach (var bot in Object.FindObjectsByType<BotBrain>())
            {
                bot.enabled = false;
            }
        }

        public static void AdvanceToCombatPhase(OfflineMatchController match)
        {
            while (match.Simulation.Phase < MatchPhase.Opening)
            {
                match.Simulation.Advance(1f / 30f);
            }
        }
    }
}
