using System;
using System.Collections;
using BattleRaja.Presentation.Combat;
using BattleRaja.Presentation.Match;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class ReducedFlashPropagationPlayModeTests
    {
        private GameObject _testRoot;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _testRoot = new GameObject("ReducedFlashPropagationRoot");
            _testRoot.AddComponent<CombatImpactFeedbackPool>();
            _testRoot.AddComponent<CombatHitFlash>();
            _testRoot.AddComponent<AandhiZoneVisual>();
            _testRoot.AddComponent<OfflineMatchHud>();
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_testRoot != null) UnityEngine.Object.Destroy(_testRoot);
            PlayerPrefs.DeleteKey("battleraja.settings.reduced_flashes");
            PlayerPrefs.Save();
            yield return null;
        }

        [UnityTest]
        public IEnumerator ReducedFlashTogglePropagatesToBrightFeedbackConsumers()
        {
            var hud = _testRoot.GetComponent<OfflineMatchHud>();
            var toggle = typeof(OfflineMatchHud).GetMethod(
                "ToggleReducedFlashes",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.That(toggle, Is.Not.Null);

            toggle.Invoke(hud, Array.Empty<object>());
            yield return null;

            Assert.That(_testRoot.GetComponent<CombatImpactFeedbackPool>().ReducedFlashMode, Is.True);
            Assert.That(_testRoot.GetComponent<CombatHitFlash>().ReducedFlashMode, Is.True);
            Assert.That(_testRoot.GetComponent<AandhiZoneVisual>().ReducedFlashMode, Is.True);

            toggle.Invoke(hud, Array.Empty<object>());
            yield return null;

            Assert.That(_testRoot.GetComponent<CombatImpactFeedbackPool>().ReducedFlashMode, Is.False);
            Assert.That(_testRoot.GetComponent<CombatHitFlash>().ReducedFlashMode, Is.False);
            Assert.That(_testRoot.GetComponent<AandhiZoneVisual>().ReducedFlashMode, Is.False);
        }
    }
}
