using BattleRaja.Core.Domain;
using UnityEngine;

namespace BattleRaja.Presentation.Combat
{
    public sealed class CombatHitFlash : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Color flashColor = new Color(1f, 0.92f, 0.28f, 1f);
        [SerializeField] private float flashDuration = 0.12f;

        private MaterialPropertyBlock _propertyBlock;
        private float _remaining;

        private void Awake()
        {
            targetRenderer = targetRenderer != null ? targetRenderer : GetComponentInChildren<Renderer>();
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void Flash(DamageResult result)
        {
            if (!result.Applied || targetRenderer == null)
            {
                return;
            }

            _remaining = Mathf.Max(0.01f, flashDuration);
            targetRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_BaseColor", flashColor);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }

        private void Update()
        {
            if (_remaining <= 0f || targetRenderer == null)
            {
                return;
            }

            _remaining -= Time.deltaTime;
            if (_remaining <= 0f)
            {
                targetRenderer.SetPropertyBlock(null);
            }
        }
    }
}
