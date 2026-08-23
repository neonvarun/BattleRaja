using UnityEngine;

namespace BattleRaja.Presentation.UI
{
    /// <summary>
    /// Small platform adapter for optional tactile feedback. It is a no-op outside
    /// Android and never participates in gameplay authority.
    /// </summary>
    public static class BattleRajaHaptics
    {
        private const string EnabledKey = "battleraja.settings.haptics";

        public static bool Enabled
        {
            get => PlayerPrefs.GetInt(EnabledKey, 1) != 0;
            set
            {
                PlayerPrefs.SetInt(EnabledKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public static void Pulse()
        {
            if (!Enabled || Application.platform != RuntimePlatform.Android) return;
#if UNITY_ANDROID && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }
}
