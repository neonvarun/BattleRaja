using UnityEngine;

namespace BattleRaja.Presentation.Movement
{
    public enum CameraProjectionMode
    {
        Orthographic = 0,
        Perspective = 1
    }

    public sealed class TopDownCameraController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private CameraProjectionMode projectionMode = CameraProjectionMode.Orthographic;
        [SerializeField] private Vector3 targetOffset = new Vector3(0f, 0.75f, 0f);
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 12f, -8f);
        [SerializeField] private float followSmoothTime = 0.08f;
        [SerializeField] private float orthographicSize = 9.5f;
        [SerializeField] private float perspectiveFieldOfView = 48f;
        [SerializeField] private LayerMask obstructionMask = 1;
        [SerializeField] private float obstructionPadding = 0.25f;

        private Camera _camera;
        private Vector3 _followVelocity;

        public CameraProjectionMode ProjectionMode => projectionMode;
        public Transform FollowTarget => followTarget;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            ApplyProjection();
        }

        private void LateUpdate()
        {
            if (followTarget == null)
            {
                return;
            }

            var targetPosition = followTarget.position + targetOffset;
            var desiredPosition = targetPosition + cameraOffset;
            var direction = desiredPosition - targetPosition;
            var distance = direction.magnitude;

            if (distance > 0.001f && obstructionMask.value != 0 && Physics.Raycast(targetPosition, direction.normalized, out var hit, distance, obstructionMask, QueryTriggerInteraction.Ignore))
            {
                desiredPosition = targetPosition + direction.normalized * Mathf.Max(0.1f, hit.distance - obstructionPadding);
            }

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _followVelocity, followSmoothTime);
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
        }

        public void SetProjectionMode(CameraProjectionMode mode)
        {
            projectionMode = mode;
            ApplyProjection();
        }

        private void ApplyProjection()
        {
            if (_camera == null)
            {
                return;
            }

            _camera.orthographic = projectionMode == CameraProjectionMode.Orthographic;
            if (_camera.orthographic)
            {
                _camera.orthographicSize = orthographicSize;
            }
            else
            {
                _camera.fieldOfView = perspectiveFieldOfView;
            }
        }
    }
}
