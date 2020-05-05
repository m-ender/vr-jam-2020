using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR.InteractionSystem;
using DG.Tweening;

namespace VRJam2020
{
    [RequireComponent(typeof(Collider))]
    public class TextTrigger : MonoBehaviour
    {
        [SerializeField] string popUpText = null;
        [SerializeField] TextManager textManager = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.transform.root.GetComponent<Player>())
                StartCoroutine(textManager.TypeText(popUpText));
        }
    }
}

