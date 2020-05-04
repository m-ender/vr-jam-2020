using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRJam2020
{
    public class Flammable : MonoBehaviour
    {
        [SerializeField] private GameObject Fire;
        [SerializeField] private float burningTime;

        private bool isBurning;
        public IEnumerator SetAlight()
        {
            if (isBurning)
                yield break;

            isBurning = true;
            Instantiate(Fire, gameObject.transform);
            yield return new WaitForSeconds(burningTime);
            gameObject.SetActive(false);
        }
    }
}


