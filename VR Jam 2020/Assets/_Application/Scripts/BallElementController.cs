using System.Collections;
using UnityEngine;

namespace VRJam2020
{
    [RequireComponent(typeof(BallState))]
    public class BallElementController : MonoBehaviour
    {
        [SerializeField] ParticleSystem burningEffect = null;
        [SerializeField] float elementEffectTime = 0;
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

        private IEnumerator SetElementEffect(ElementalState elementalState, ParticleSystem elementalEffect)
        {
            ballState.ElementalState = elementalState;
            elementalEffect.Play();

            yield return new WaitForSeconds(elementEffectTime);

            if(ballState.ElementalState == elementalState)
                RemoveElementEffect(burningEffect);
        }

        private void RemoveElementEffect(ParticleSystem elementEffect)
        {
            ballState.ElementalState = ElementalState.None;
            elementEffect.Stop();
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