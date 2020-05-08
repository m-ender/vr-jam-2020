using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VRJam2020
{
    public class Victory : MonoBehaviour
    {
        [SerializeField] private float successScreenDuration = 10f;

        private TextManager textManager;
        private Image solidColorOverlay;
        private GameObject playerRoot;
        
        private void Awake()
        {
            textManager = FindObjectOfType<TextManager>();
            solidColorOverlay = GameObject.FindWithTag("SolidColorOverlay").GetComponent<Image>();
            playerRoot = GameObject.FindWithTag("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            BallState ballState = other.GetComponentInParent<BallState>();
            if (ballState && ballState.CollisionState == CollisionState.Teleport)
            {
                DOTween.Sequence()
                    .AppendInterval(10f)
                    .AppendCallback(() => {
                        solidColorOverlay.DOColor(Color.black, 1f);
                        textManager.ShowText(PopUpType.Alert, "You beat the game! Can you find any other paths through the castle?", successScreenDuration);
                    })
                    .AppendInterval(successScreenDuration)
                    .AppendCallback(() => {
                        Destroy(playerRoot);
                        Destroy(ballState.gameObject);
                        SceneManager.LoadScene("MainScene");
                    });

                Destroy(this);
            }
        }
    } 
}
