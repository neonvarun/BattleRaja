using UnityEngine;

namespace BattleRaja.Presentation.AI
{
    public sealed class BotDebugOverlay : MonoBehaviour
    {
        [SerializeField] private BotBrain brain;
        [SerializeField] private bool showOverlay;

        private void Awake()
        {
            brain = brain != null ? brain : GetComponent<BotBrain>();
        }

        private void OnGUI()
        {
#if !UNITY_EDITOR
            // Engineering labels are useful in the editor and development builds,
            // but they must never ship in the player-facing release surface.
            if (!Debug.isDebugBuild)
            {
                return;
            }
#endif
            if (!showOverlay || brain == null || Camera.main == null)
            {
                return;
            }

            var screen = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.2f);
            if (screen.z <= 0f)
            {
                return;
            }

            var rect = new Rect(screen.x, Screen.height - screen.y, 280f, 22f);
            GUI.Label(rect, brain.DebugSummary);
        }
    }
}
