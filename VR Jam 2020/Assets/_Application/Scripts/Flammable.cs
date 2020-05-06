using System.Collections;
using UnityEngine;

namespace VRJam2020
{
    public class Flammable : MonoBehaviour
    {
        [SerializeField] private GameObject fire = null;
        [SerializeField] private float burningTime = 0f;

        private bool isBurning;
        public IEnumerator SetAlight()
        {
            if (isBurning)
                yield break;

            isBurning = true;
            Instantiate(fire, gameObject.transform);
            yield return new WaitForSeconds(burningTime);

            var ball = GetComponentInChildren<BallController>();

            if (ball)
                ball.Unstick();

            Destroy(gameObject);
        }
    }
}
