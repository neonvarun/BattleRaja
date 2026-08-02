using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Movement
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class AimDirectionIndicator : MonoBehaviour
    {
        [SerializeField] private float length = 1.75f;
        [SerializeField] private float width = 0.08f;
        [SerializeField] private Color color = new Color(1f, 0.78f, 0.16f, 1f);
        [SerializeField] private Material indicatorMaterial;

        private LineRenderer _line;
        private MovementPlayerAgent _agent;
        private Float2 _aimDirection = Float2.Up;

        private void Awake()
        {
            _line = GetComponent<LineRenderer>();
            _agent = GetComponentInParent<MovementPlayerAgent>();
            _line.positionCount = 2;
            _line.useWorldSpace = true;
            _line.startWidth = width;
            _line.endWidth = width * 0.55f;
            _line.startColor = color;
            _line.endColor = new Color(color.r, color.g, color.b, 0.18f);
            if (_line.sharedMaterial == null)
            {
                _line.sharedMaterial = indicatorMaterial;
            }
        }

        private void LateUpdate()
        {
            if (_agent != null)
            {
                _aimDirection = _agent.AimDirection;
            }

            var start = transform.position + (Vector3.up * 0.12f);
            var end = start + new Vector3(_aimDirection.X, 0f, _aimDirection.Y) * length;
            _line.SetPosition(0, start);
            _line.SetPosition(1, end);
        }

        public void SetAimDirection(Float2 direction)
        {
            if (direction.SqrMagnitude > 0.000001f)
            {
                _aimDirection = direction.Normalized;
            }
        }
    }
}
