using System.Linq;
using UnityEngine;

namespace BattleRaja.Presentation.Match
{
    public sealed class OfflineMatchHud : MonoBehaviour
    {
        [SerializeField] private OfflineMatchController match;
        [SerializeField] private bool showZoneOverlay = true;

        private void Awake()
        {
            match = match != null ? match : FindFirstObjectByType<OfflineMatchController>();
        }

        private void OnGUI()
        {
            if (!showZoneOverlay || match == null) return;
            var warning = match.AandhiState == BattleRaja.Core.Domain.AandhiState.Warning
                ? $"  WARNING {match.AandhiWarningRemainingSeconds:0.0}s"
                : match.AandhiState == BattleRaja.Core.Domain.AandhiState.Closing ? "  CLOSING" : string.Empty;
            var status = $"MATCH {match.CurrentPhase.ToString().ToUpperInvariant()}  ALIVE {match.AliveCount}  ZONE {match.ZoneRadius:0.0}  NEXT {match.NextZoneRadius:0.0}{warning}";
            GUI.Label(new Rect(18f, 18f, 520f, 30f), status);
            if (match.PlayerSpectating) GUI.Label(new Rect(18f, 48f, 520f, 30f), "SPECTATING — press TAB to cycle");
            if (match.ResultsShown)
            {
                var winner = match.Results?.FirstOrDefault(snapshot => snapshot.Placement == 1);
                var winnerId = winner.HasValue ? winner.Value.Id.Value : 0;
                GUI.Label(new Rect(Screen.width * 0.5f - 180f, Screen.height * 0.5f - 50f, 360f, 40f), $"RESULTS  WINNER {winnerId}");
                if (winner.HasValue)
                {
                    GUI.Label(new Rect(Screen.width * 0.5f - 180f, Screen.height * 0.5f - 10f, 420f, 30f),
                        $"KOs {winner.Value.Eliminations}  DAMAGE {winner.Value.DamageDealt}  SURVIVAL {winner.Value.SurvivalTimeSeconds:0.0}s");
                }
                GUI.Label(new Rect(Screen.width * 0.5f - 180f, Screen.height * 0.5f, 360f, 40f), "Press R to rematch");
            }
        }

        private void Update()
        {
            if (match != null && match.PlayerSpectating && Input.GetKeyDown(KeyCode.Tab)) match.CycleSpectator();
            if (match != null && match.ResultsShown && Input.GetKeyDown(KeyCode.R)) match.RestartMatch();
        }
    }
}
