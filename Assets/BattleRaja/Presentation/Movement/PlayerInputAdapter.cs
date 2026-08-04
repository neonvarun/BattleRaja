using BattleRaja.Core.Domain;
using UnityEngine;
using UnityEngine.InputSystem;
using BattleRaja.Presentation.Combat;

namespace BattleRaja.Presentation.Movement
{
    public sealed class PlayerInputAdapter : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actionsAsset;
        [SerializeField] private Camera worldCamera;
        [SerializeField] private VirtualStick movementStick;
        [SerializeField] private VirtualStick aimStick;
        [SerializeField] private AttackButton attackButton;
        [SerializeField] private AbilityButton abilityButton;
        [SerializeField] private Transform aimOrigin;
        [SerializeField] private float aimAssistRange = 10f;
        [SerializeField] private float aimAssistConeDegrees = 18f;

        private InputActionMap _playerMap;
        private InputAction _moveAction;
        private InputAction _mousePositionAction;
        private InputAction _aimStickAction;
        private InputAction _attackAction;
        private InputAction _abilityAction;
        private Collider[] _aimAssistColliders;
        private AimAssistCandidate[] _aimAssistCandidates;
        private bool _aimAssistEnabled;
        private bool _hasFocus = true;

        public bool HasFocus => _hasFocus;
        public bool AimAssistEnabled => _aimAssistEnabled;
        public bool IsAttackHeld => _hasFocus && ((_attackAction != null && _attackAction.IsPressed()) || (attackButton != null && attackButton.IsPressed));
        public bool IsAbilityPressed => _hasFocus && ((_abilityAction != null && _abilityAction.IsPressed()) || (abilityButton != null && abilityButton.IsPressed));

        private void Awake()
        {
            aimOrigin = aimOrigin != null ? aimOrigin : transform;
            _aimAssistColliders = new Collider[32];
            _aimAssistCandidates = new AimAssistCandidate[32];
            _aimAssistEnabled = PlayerPrefs.GetInt("battleraja.settings.aim_assist", 0) != 0;
            if (actionsAsset != null)
            {
                _playerMap = actionsAsset.FindActionMap("Player", throwIfNotFound: false);
                _moveAction = _playerMap?.FindAction("Move", throwIfNotFound: false);
                _mousePositionAction = _playerMap?.FindAction("MousePosition", throwIfNotFound: false);
                _aimStickAction = _playerMap?.FindAction("AimStick", throwIfNotFound: false);
                _attackAction = _playerMap?.FindAction("Attack", throwIfNotFound: false);
                _abilityAction = _playerMap?.FindAction("Ability", throwIfNotFound: false);
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

            if (_aimAssistEnabled)
            {
                aim = ApplyAimAssist(aim);
            }

            return new MovementInputFrame(
                new Float2(movement.x, movement.y),
                new Float2(aim.x, aim.y));
        }

        public void SetAimAssistEnabled(bool enabled)
        {
            _aimAssistEnabled = enabled;
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
            attackButton?.ResetButton();
            abilityButton?.ResetButton();
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

        private Vector2 ApplyAimAssist(Vector2 inputAim)
        {
            if (aimOrigin == null || inputAim.sqrMagnitude <= 0.0001f) return inputAim;

            var colliderCount = Physics.OverlapSphereNonAlloc(
                aimOrigin.position,
                Mathf.Max(0.1f, aimAssistRange),
                _aimAssistColliders);
            var candidateCount = 0;
            var origin = new Float2(aimOrigin.position.x, aimOrigin.position.z);
            for (var i = 0; i < colliderCount && candidateCount < _aimAssistCandidates.Length; i++)
            {
                var collider = _aimAssistColliders[i];
                if (collider == null) continue;
                var target = collider.GetComponentInParent<CombatTarget>();
                if (target == null || target.Faction != CombatFaction.Enemy ||
                    target.Health == null || target.Health.Snapshot.IsDefeated) continue;

                _aimAssistCandidates[candidateCount++] = new AimAssistCandidate(
                    target.Id,
                    new Float2(target.transform.position.x, target.transform.position.z));
            }

            return AimAssistTargeting.TryAssist(
                origin,
                new Float2(inputAim.x, inputAim.y),
                _aimAssistCandidates,
                candidateCount,
                Mathf.Max(0.1f, aimAssistRange),
                Mathf.Clamp(aimAssistConeDegrees, 1f, 179f),
                out var assisted)
                ? new Vector2(assisted.X, assisted.Y)
                : inputAim;
        }
    }
}
