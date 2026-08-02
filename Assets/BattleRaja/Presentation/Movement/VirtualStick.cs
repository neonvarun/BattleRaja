using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleRaja.Presentation.Movement
{
    public sealed class VirtualStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private RectTransform knob;
        [SerializeField] private float radius = 92f;
        [Range(0f, 0.99f)] [SerializeField] private float deadZone = 0.14f;
        [Range(0f, 1f)] [SerializeField] private float opacity = 0.72f;

        private RectTransform _rectTransform;
        private int _activePointerId = int.MinValue;
        private Vector2 _value;

        public Vector2 Value => _value;
        public bool IsActive => _activePointerId != int.MinValue;
        public float Radius => radius;
        public float DeadZone => deadZone;
        public float Opacity => opacity;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            if (knob == null)
            {
                knob = _rectTransform;
            }
        }

        private void OnDisable()
        {
            ResetStick();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                ResetStick();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsActive)
            {
                return;
            }

            _activePointerId = eventData.pointerId;
            UpdateValue(eventData.position, eventData.pressEventCamera);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId == _activePointerId)
            {
                UpdateValue(eventData.position, eventData.pressEventCamera);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId == _activePointerId)
            {
                ResetStick();
            }
        }

        public void ResetStick()
        {
            _activePointerId = int.MinValue;
            _value = Vector2.zero;
            if (knob != null)
            {
                knob.anchoredPosition = Vector2.zero;
            }
        }

        private void UpdateValue(Vector2 screenPosition, Camera eventCamera)
        {
            if (_rectTransform == null)
            {
                return;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPosition, eventCamera, out var localPoint))
            {
                return;
            }

            var normalized = Vector2.ClampMagnitude(localPoint / Mathf.Max(1f, radius), 1f);
            _value = normalized;
            if (knob != null)
            {
                knob.anchoredPosition = normalized * radius;
            }
        }
    }
}
