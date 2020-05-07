using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRJam2020
{

    public class RotateObject : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 0;
        void Update()
        {
            transform.Rotate(0, rotateSpeed, 0, Space.World);
        }
    }

}
