using BattleRaja.Core.Domain;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleRaja.Presentation.Movement
{
    public sealed class PlayerInputAdapter : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actionsAsset;
        [SerializeField] private Camera worldCamera;
        [SerializeField] private VirtualStick movementStick;
        [SerializeField] private VirtualStick aimStick;
        [SerializeField] private Transform aimOrigin;

        private InputActionMap _playerMap;
        private InputAction _moveAction;
        private InputAction _mousePositionAction;
        private InputAction _aimStickAction;
        private bool _hasFocus = true;

        public bool HasFocus => _hasFocus;

        private void Awake()
        {
            aimOrigin = aimOrigin != null ? aimOrigin : transform;
            if (actionsAsset != null)
            {
                _playerMap = actionsAsset.FindActionMap("Player", throwIfNotFound: false);
                _moveAction = _playerMap?.FindAction("Move", throwIfNotFound: false);
                _mousePositionAction = _playerMap?.FindAction("MousePosition", throwIfNotFound: false);
                _aimStickAction = _playerMap?.FindAction("AimStick", throwIfNotFound: false);
            }
        }

        private void OnEnable()
        {
            _playerMap?.Enable();
        }

        private void OnDisable()
        {
            _playerMap?.Disable();
            ResetInputState();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _hasFocus = hasFocus;
            if (!hasFocus)
            {
                ResetInputState();
            }
        }

        public MovementInputFrame ReadInput()
        {
            if (!_hasFocus)
            {
                return new MovementInputFrame(Float2.Zero, Float2.Zero);
            }

            var movement = ReadMovement();
            var aim = ReadVirtualAim();
            if (aim.sqrMagnitude <= 0.0001f)
            {
                aim = ReadMouseAim();
            }

            return new MovementInputFrame(
                new Float2(movement.x, movement.y),
                new Float2(aim.x, aim.y));
        }

        public void ReleasePointerFocus()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ResetInputState();
        }

        public void ResetInputState()
        {
            movementStick?.ResetStick();
            aimStick?.ResetStick();
        }

        private Vector2 ReadMovement()
        {
            var touchMovement = movementStick != null && movementStick.IsActive
                ? movementStick.Value
                : Vector2.zero;
            if (touchMovement.sqrMagnitude > 0.0001f)
            {
                return Vector2.ClampMagnitude(touchMovement, 1f);
            }

            return _moveAction != null ? Vector2.ClampMagnitude(_moveAction.ReadValue<Vector2>(), 1f) : Vector2.zero;
        }

        private Vector2 ReadVirtualAim()
        {
            var touchAim = aimStick != null && aimStick.IsActive
                ? aimStick.Value
                : Vector2.zero;
            if (touchAim.sqrMagnitude > 0.0001f)
            {
                return Vector2.ClampMagnitude(touchAim, 1f);
            }

            return _aimStickAction != null ? Vector2.ClampMagnitude(_aimStickAction.ReadValue<Vector2>(), 1f) : Vector2.zero;
        }

        private Vector2 ReadMouseAim()
        {
            if (worldCamera == null || aimOrigin == null || _mousePositionAction == null)
            {
                return Vector2.zero;
            }

            var screenPosition = _mousePositionAction.ReadValue<Vector2>();
            var ray = worldCamera.ScreenPointToRay(screenPosition);
            var plane = new Plane(Vector3.up, new Vector3(0f, aimOrigin.position.y, 0f));
            if (!plane.Raycast(ray, out var distance))
            {
                return Vector2.zero;
            }

            var worldPoint = ray.GetPoint(distance);
            var direction = worldPoint - aimOrigin.position;
            direction.y = 0f;
            return new Vector2(direction.x, direction.z).normalized;
        }
    }
}
