using UnityEngine;
using Valve.VR.InteractionSystem;

namespace VRJam2020
{
    [RequireComponent(typeof(Collider))]
    public class TextTrigger : MonoBehaviour
    {
        [SerializeField] private PopUpType type = PopUpType.PlayerDialogue;
        [SerializeField] private string popUpText = null;
        [SerializeField] private float displayTimeBeforeFade = 0;
        [SerializeField] private bool isDynamicallyTyped = false;
        [SerializeField] private bool singleUse = false;
        [SerializeField] private bool abilityDependant = false;
        [SerializeField] private BallAbilities ability = BallAbilities.Glow;

        private TextManager textManager;
        private BallController ball;
        private void Awake()
        {
            textManager = FindObjectOfType<TextManager>();
            ball = FindObjectOfType<BallController>();
        }
        private void OnTriggerEnter(Collider other)
        {
            var ballState = other.GetComponentInParent<BallState>();
            var player = other.GetComponentInParent<Player>();

            if (ballState && ballState.CollisionState == CollisionState.Teleport
                || player)
            {
                if (abilityDependant)
                {
                    if (!ball.unlockedAbilities.Contains(ability))
                        TypeOrShowText();
                }
                else
                    TypeOrShowText();

                if (singleUse)
                    Destroy(gameObject);
            }
        }

        private void TypeOrShowText()
        {
            if (isDynamicallyTyped)
                textManager.TypeText(type, popUpText, displayTimeBeforeFade);
            else
                textManager.ShowText(type, popUpText, displayTimeBeforeFade);
        }
    }
}
