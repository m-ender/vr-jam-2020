using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRJam2020
{
    public class BallFlames : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject ball = null;

        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.SetPositionAndRotation(ball.transform.position, Quaternion.Euler(new Vector3 (0,0,0)));
        }
    }
}
