using UnityEngine;

namespace BattleRaja.Presentation.Movement
{
    public sealed class SafeAreaPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform target;
        private Rect _lastSafeArea;

        private void Awake()
        {
            target = target != null ? target : transform as RectTransform;
            ApplySafeArea();
        }

        private void Update()
        {
            if (_lastSafeArea != Screen.safeArea)
            {
                ApplySafeArea();
            }
        }

        private void ApplySafeArea()
        {
            if (target == null || Screen.width <= 0 || Screen.height <= 0)
            {
                return;
            }

            _lastSafeArea = Screen.safeArea;
            target.anchorMin = new Vector2(_lastSafeArea.xMin / Screen.width, _lastSafeArea.yMin / Screen.height);
            target.anchorMax = new Vector2(_lastSafeArea.xMax / Screen.width, _lastSafeArea.yMax / Screen.height);
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
        }
    }
}
