using UnityEngine;

namespace BattleRaja.Presentation.Performance
{
    /// <summary>
    /// Establishes the Android presentation frame-rate contract for the offline V1
    /// player. Authority timing remains fixed-step and independent from this setting.
    /// Devices that cannot sustain 60 FPS naturally fall back to their measured rate;
    /// the release policy accepts a stable 30 FPS fallback after profiling.
    /// </summary>
    internal static class AndroidPerformanceBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ConfigureAndroidFramePacing()
        {
            if (Application.platform != RuntimePlatform.Android) return;

            // Unity's default Android presentation can settle at 30 FPS even when the
            // display and workload can support 60. Let the platform scheduler present
            // up to the product target while preserving the fixed authority tick.
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
