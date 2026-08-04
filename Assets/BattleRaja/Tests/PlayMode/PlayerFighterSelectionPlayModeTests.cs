using System.Collections;
using BattleRaja.Core.Application;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Movement;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class PlayerFighterSelectionPlayModeTests
    {
        [UnityTest]
        public IEnumerator SelectedPehelBindsTheSharedPlayerActor()
        {
            const string key = "battleraja.selected_fighter";
            var hadPrevious = PlayerPrefs.HasKey(key);
            var previous = PlayerPrefs.GetInt(key, (int)ProductionFighter.Bijli);
            PlayerPrefs.SetInt(key, (int)ProductionFighter.Pehel);
            PlayerPrefs.Save();

            try
            {
                yield return SceneManager.LoadSceneAsync("BazaarBastion", LoadSceneMode.Single);
                yield return null;
                yield return null;

                var selection = Object.FindAnyObjectByType<PlayerFighterSelection>();
                Assert.That(selection, Is.Not.Null);
                var agent = selection.GetComponent<MovementPlayerAgent>();
                Assert.That(agent, Is.Not.Null);
                Assert.That(selection.ActiveFighter, Is.EqualTo(ProductionFighter.Pehel));
                Assert.That(selection.GetComponent<PehelFighterController>().enabled, Is.True);
                Assert.That(selection.GetComponent<BijliFighterController>().enabled, Is.False);
                Assert.That(agent.ActorId, Is.EqualTo(1));
            }
            finally
            {
                if (hadPrevious) PlayerPrefs.SetInt(key, previous);
                else PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }
    }
}
