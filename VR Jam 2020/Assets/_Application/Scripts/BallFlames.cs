using UnityEngine;

namespace VRJam2020
{
    public class BallFlames : MonoBehaviour
    {
        [SerializeField] private GameObject ball = null;

        private void LateUpdate()
        {
            transform.SetPositionAndRotation(ball.transform.position, Quaternion.Euler(new Vector3 (0,0,0)));
        }
    }
}
