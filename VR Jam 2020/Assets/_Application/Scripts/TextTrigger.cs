using UnityEngine;

namespace VRJam2020
{
    [RequireComponent(typeof(Collider))]
    public class TextTrigger : MonoBehaviour
    {
        [SerializeField] string popUpText = null;
        [SerializeField] TextManager textManager = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.transform.parent?.GetComponent<BallState>())
                StartCoroutine(textManager.TypeText(popUpText));
        }
    }
}
