using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;

namespace VRJam2020
{
    public class PlayerHealth : Health
    {
        [SerializeField] private Color damageHighlightColor;
        [SerializeField] private float damageHighlightTime;

        [SerializeField] private Image solidColorOverlay;
        [SerializeField] private TextManager textManager;
        [SerializeField] private float gameOverDuration;

        protected override void Die()
        {
            // Turn screen black
            solidColorOverlay.DOColor(Color.black, 1f);
            // Display message
            textManager.ShowText(PopUpType.Alert, "You died. Better luck next time!", gameOverDuration);
            // Restart game (i.e. load main scene) after timer

            GameObject playerRoot = transform.root.gameObject;

            DOTween.Sequence()
                .InsertCallback(gameOverDuration, () => {
                    Destroy(playerRoot);
                    SceneManager.LoadScene("MainScene");
                });

            Destroy(this);
        }

        protected override void ShowDamageFeedback()
        {
            DOTween.Sequence()
                .Append(solidColorOverlay.DOColor(damageHighlightColor, damageHighlightTime / 2))
                .Append(solidColorOverlay.DOColor(Color.clear, damageHighlightTime / 2));
        }
    }
}