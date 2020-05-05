using UnityEngine;

namespace VRJam2020
{
    public class BallCameraController : MonoBehaviour
    {
        public Transform targetTransform;

        private void Update()
        {
            if (targetTransform)
                transform.rotation = targetTransform.rotation;
        }
    }
}

