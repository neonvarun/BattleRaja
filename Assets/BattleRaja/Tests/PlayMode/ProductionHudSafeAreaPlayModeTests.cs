using System.Collections;
using BattleRaja.Presentation.Match;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace BattleRaja.Tests.PlayMode
{
    public sealed class ProductionHudSafeAreaPlayModeTests
    {
        [UnityTest]
        public IEnumerator ProductionHudRootStaysInsideSafeArea()
        {
            yield return SceneManager.LoadSceneAsync("BazaarBastion", LoadSceneMode.Single);
            yield return null;
            yield return null;

            var root = GameObject.Find("ProductionHudRoot");
            Assert.That(root, Is.Not.Null);

            var safeArea = root.transform.parent;
            Assert.That(safeArea.name, Is.EqualTo("SafeArea"));
            Assert.That(safeArea.parent.GetComponent<Canvas>(), Is.Not.Null);
            Assert.That(safeArea.GetComponent<SafeAreaPanel>(), Is.Not.Null);

            var safeCorners = new Vector3[4];
            var hudCorners = new Vector3[4];
            ((RectTransform)safeArea).GetWorldCorners(safeCorners);
            ((RectTransform)root.transform).GetWorldCorners(hudCorners);

            const float tolerance = 0.01f;
            Assert.That(hudCorners[0].x, Is.GreaterThanOrEqualTo(safeCorners[0].x - tolerance));
            Assert.That(hudCorners[0].y, Is.GreaterThanOrEqualTo(safeCorners[0].y - tolerance));
            Assert.That(hudCorners[2].x, Is.LessThanOrEqualTo(safeCorners[2].x + tolerance));
            Assert.That(hudCorners[2].y, Is.LessThanOrEqualTo(safeCorners[2].y + tolerance));
        }
    }
}
