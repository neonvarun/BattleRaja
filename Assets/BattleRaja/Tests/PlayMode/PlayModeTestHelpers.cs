using System.Linq;
using BattleRaja.Presentation.Movement;
using BattleRaja.Presentation.AI;
using UnityEngine;

namespace BattleRaja.Tests.PlayMode
{
    internal static class PlayModeTestHelpers
    {
        public static T FindPlayer<T>() where T : Component
        {
            return Object.FindObjectsByType<T>(FindObjectsSortMode.None)
                .First(component => component.GetComponent<MovementPlayerAgent>()?.ActorId == 1 ||
                                    component.GetComponentInParent<MovementPlayerAgent>()?.ActorId == 1);
        }

        public static void DisableBots()
        {
            foreach (var bot in Object.FindObjectsByType<BotBrain>(FindObjectsSortMode.None))
            {
                bot.enabled = false;
            }
        }
    }
}
