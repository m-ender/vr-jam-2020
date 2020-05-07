using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace VRJam2020
{
    public class EnemyHealth : Health
    {
        [SerializeField] private Renderer modelRenderer;

        [SerializeField] private Color damageHighlightColor;
        [SerializeField] private float damageHighlightTime;

        protected override void Die()
        {
            StartCoroutine(DieNextFrame());
        }

        private IEnumerator DieNextFrame()
        {
            yield return null;

            Destroy(gameObject);
        }

        protected override void ShowDamageFeedback()
        {
            DOTween.Sequence()
                .Append(modelRenderer.material.DOColor(damageHighlightColor, damageHighlightTime / 2))
                .Append(modelRenderer.material.DOColor(Color.white, damageHighlightTime / 2));
        }
    }
}