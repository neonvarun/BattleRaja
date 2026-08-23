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
        private const float PortraitFramingCapMultiplier = 3.5f;

        [SerializeField] private Transform followTarget;
        [SerializeField] private CameraProjectionMode projectionMode = CameraProjectionMode.Orthographic;
        [SerializeField] private Vector3 targetOffset = new Vector3(0f, 0.75f, 0f);
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 12f, -8f);
        [SerializeField] private float followSmoothTime = 0.08f;
        [SerializeField] private float orthographicSize = 9.5f;
        [SerializeField] private float referenceAspect = 16f / 9f;
        [SerializeField] private float perspectiveFieldOfView = 48f;
        [SerializeField] private LayerMask obstructionMask = 1;
        [SerializeField] private float obstructionPadding = 0.25f;

        private Camera _camera;
        private Vector3 _followVelocity;
        private float _lastAspect = -1f;

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

            if (projectionMode == CameraProjectionMode.Orthographic)
            {
                ApplyResponsiveOrthographicSize();
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

        public void SetFollowTarget(Transform target)
        {
            followTarget = target;
        }

        public static float CalculateResponsiveOrthographicSize(float baseSize, float aspect, float referenceAspect)
        {
            if (float.IsNaN(baseSize) || float.IsInfinity(baseSize) || baseSize <= 0f)
            {
                return 0.1f;
            }

            if (float.IsNaN(aspect) || float.IsInfinity(aspect) || aspect <= 0f ||
                float.IsNaN(referenceAspect) || float.IsInfinity(referenceAspect) || referenceAspect <= 0f)
            {
                return baseSize;
            }

            if (aspect >= referenceAspect) return baseSize;

            // Preserve the full arena on narrow screens without letting a very tall
            // portrait device shrink the readable play space into a postage stamp.
            // The cap is based on the authored arena scale (28 world units wide at
            // the V1 target) and keeps desktop framing unchanged.
            var portraitSize = baseSize * (referenceAspect / aspect);
            return Mathf.Min(portraitSize, baseSize * PortraitFramingCapMultiplier);
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
                _lastAspect = -1f;
                ApplyResponsiveOrthographicSize();
            }
            else
            {
                _camera.fieldOfView = perspectiveFieldOfView;
            }
        }

        private void ApplyResponsiveOrthographicSize()
        {
            if (_camera == null) return;
            var aspect = _camera.aspect;
            if (Mathf.Abs(aspect - _lastAspect) < 0.001f) return;
            _lastAspect = aspect;
            _camera.orthographicSize = CalculateResponsiveOrthographicSize(orthographicSize, aspect, referenceAspect);
        }
    }
}
