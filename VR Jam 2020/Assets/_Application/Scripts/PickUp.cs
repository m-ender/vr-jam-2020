using UnityEngine;

namespace VRJam2020
{
    public class PickUp : MonoBehaviour
    {
        [SerializeField] private BallAbilities grantedAbility = BallAbilities.Spy;

        private void OnTriggerEnter(Collider other)
        {
            var ballController = other.transform.parent?.GetComponent<BallController>();
            if (ballController)
            {
                ballController.Unlock(grantedAbility);
            }

            Destroy(gameObject);
        }
    }
}
