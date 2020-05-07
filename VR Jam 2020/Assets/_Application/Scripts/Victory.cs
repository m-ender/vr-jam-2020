using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRJam2020
{
    public class Victory : MonoBehaviour
    {
        [SerializeField] private float successScreenDuration;

        private TextManager textManager;
        
        private void Awake()
        {
            textManager = FindObjectOfType<TextManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            BallState ballState = other.GetComponentInParent<BallState>();
            if (ballState && ballState.CollisionState == CollisionState.Teleport)
            {
                textManager.ShowText(PopUpType.Alert, "You beat the game! Can you find any other paths through the castle?", successScreenDuration);
                // Restart game (i.e. load main scene) after timer

                GameObject playerRoot = transform.root.gameObject;

                DOTween.Sequence()
                    .InsertCallback(successScreenDuration, () => {
                        Destroy(playerRoot);
                        SceneManager.LoadScene("MainScene");
                    });

                Destroy(this);
            }
        }
    } 
}
