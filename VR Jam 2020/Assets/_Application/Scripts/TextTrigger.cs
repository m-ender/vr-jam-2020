using UnityEngine;
using UnityEngine.UI;

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
            if (other.gameObject.transform.parent?.GetComponent<BallState>() 
                && other.gameObject.transform.parent.GetComponent<BallState>().CollisionState == CollisionState.Teleport)
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
