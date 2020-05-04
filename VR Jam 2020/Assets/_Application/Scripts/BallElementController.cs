using System.Collections;
using UnityEngine;

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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FireSource>())
            {
                StartCoroutine(SetElementEffect(ElementalState.Burning, burningEffect));
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
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
            if (ballState.ElementalState == ElementalState.Burning)
            {
                var flammable = collision.gameObject.GetComponent<Flammable>();

                if (flammable)
                {
                    StartCoroutine(flammable.SetAlight());
                    RemoveElementEffect(burningEffect);
                }
            }
        }
    }
}