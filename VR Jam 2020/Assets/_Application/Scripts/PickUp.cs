using UnityEngine;

namespace VRJam2020
{
    public class PickUp : MonoBehaviour
    {
        [SerializeField] private BallAbilities grantedAbility = BallAbilities.Spy;
        [SerializeField] private string popUpText = "";

        private TextManager textManager;

        private void Awake()
        {
            textManager = FindObjectOfType<TextManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var ballController = other.GetComponentInParent<BallController>();
            if (ballController)
            {
                ballController.Unlock(grantedAbility);
                textManager.ShowText(PopUpType.Alert, popUpText, 5f);
            }

            Destroy(gameObject);
        }
    }
}
