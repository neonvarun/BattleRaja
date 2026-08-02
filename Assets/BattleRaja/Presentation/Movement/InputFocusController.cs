using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleRaja.Presentation.Movement
{
    public sealed class InputFocusController : MonoBehaviour
    {
        [SerializeField] private PlayerInputAdapter inputAdapter;

        private void Awake()
        {
            inputAdapter = inputAdapter != null ? inputAdapter : GetComponent<PlayerInputAdapter>();
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                inputAdapter?.ReleasePointerFocus();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                inputAdapter?.ResetInputState();
            }
        }
    }
}
