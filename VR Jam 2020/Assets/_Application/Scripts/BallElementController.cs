using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRJam2020;

namespace VRJam2020
{
    [RequireComponent(typeof(BallState))]
    public class BallElementController : MonoBehaviour
    {
        [SerializeField] GameObject burningEffect;
        [SerializeField] float elementEffectTime;
        private BallState ballState;

        private void Awake()
        {
            ballState = gameObject.GetComponent<BallState>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<FireSource>())
            {
                StartCoroutine(SetElementEffect(ElementalState.Burning, burningEffect));
            }

            //TODO refactor this to generalise to other elements.
            BurnFlammableObjects(collision);
        }

        private IEnumerator SetElementEffect(ElementalState elementalState, GameObject elementalEffect)
        {
            ballState.ElementalState = elementalState;
            elementalEffect.SetActive(true);

            yield return new WaitForSeconds(elementEffectTime);

            if(ballState.ElementalState == elementalState)
                RemoveElementEffect(burningEffect);
        }

        private void RemoveElementEffect(GameObject elementEffect)
        {
            ballState.ElementalState = ElementalState.None;
            elementEffect.SetActive(false);
        }

        private void BurnFlammableObjects(Collision collision)
        {
            if (collision.gameObject.GetComponent<Flammable>()
                && ballState.ElementalState == ElementalState.Burning)
            {
                StartCoroutine(collision.gameObject.GetComponent<Flammable>().SetAlight());
                RemoveElementEffect(burningEffect);
            }
        }
    }
}