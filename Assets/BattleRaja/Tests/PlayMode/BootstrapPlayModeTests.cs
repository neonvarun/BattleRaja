using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class BootstrapPlayModeTests
    {
        [UnityTest]
        public IEnumerator PlayerLoopAdvancesInPlayMode()
        {
            var frameBeforeYield = Time.frameCount;

            yield return null;

            Assert.That(Application.isPlaying, Is.True);
            Assert.That(Time.frameCount, Is.GreaterThanOrEqualTo(frameBeforeYield));
        }
    }
}
