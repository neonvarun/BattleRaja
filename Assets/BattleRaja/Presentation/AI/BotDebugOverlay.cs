using UnityEngine;

namespace BattleRaja.Presentation.AI
{
    public sealed class BotDebugOverlay : MonoBehaviour
    {
        [SerializeField] private BotBrain brain;
        [SerializeField] private bool showOverlay = true;

        private void Awake()
        {
            brain = brain != null ? brain : GetComponent<BotBrain>();
        }

        private void OnGUI()
        {
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
