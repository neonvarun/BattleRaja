using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace BattleRaja.Presentation.Flow
{
    /// <summary>
    /// Keeps older serialized scenes compatible after the project moves to the Input
    /// System-only handler. Existing user-owned scenes may still contain a legacy
    /// StandaloneInputModule, so replace it at every scene-load boundary before the
    /// first EventSystem update rather than rewriting those scene files in place.
    /// </summary>
    internal static class InputModuleCompatibilityBridge
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RegisterSceneLoadHandler()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var legacyModules = Object.FindObjectsByType<StandaloneInputModule>();
            for (var i = 0; i < legacyModules.Length; i++)
            {
                var legacy = legacyModules[i];
                if (legacy == null) continue;

                var eventSystem = legacy.GetComponent<EventSystem>();
                if (eventSystem == null) continue;

                legacy.enabled = false;
                var modern = eventSystem.GetComponent<InputSystemUIInputModule>();
                if (modern == null)
                {
                    modern = eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
                }

                modern.AssignDefaultActions();
                modern.enabled = true;
                Object.Destroy(legacy);
            }
        }
    }
}
