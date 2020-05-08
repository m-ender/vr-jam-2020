using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VRJam2020
{
    public class PlayerHealth : Health
    {
        [SerializeField] private Color damageHighlightColor = Color.red;
        [SerializeField] private float damageHighlightTime = 0.3f;

        [SerializeField] private Image solidColorOverlay = null;
        [SerializeField] private TextManager textManager = null;
        [SerializeField] private float gameOverDuration = 5f;

        private BallController ballController;

        protected override void Awake()
        {
            base.Awake();
            ballController = FindObjectOfType<BallController>();
        }

        protected override void Die()
        {
            Destroy(ballController.gameObject);

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