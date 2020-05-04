using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRJam2020
{
    public class BallCameraController : MonoBehaviour
    {
        public Transform targetTransform;

        // Update is called once per frame
        void Update()
        {
            if(targetTransform)
                transform.rotation = targetTransform.rotation;
        }
    }
}

