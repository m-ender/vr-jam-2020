using UnityEngine;

namespace VRJam2020
{
    [RequireComponent(typeof(BallState))]
    public class BallElementController : MonoBehaviour
    {
        [SerializeField] ParticleSystem burningEffect = null;
        [SerializeField] float elementEffectTime = 0;

        private float effectTimeLeft = 0;

        private BallState ballState;

        private void Awake()
        {
            ballState = gameObject.GetComponent<BallState>();
        }

        private void Update()
        {
            effectTimeLeft -= Time.deltaTime;

            if (effectTimeLeft < 0)
                if (ballState.ElementalState == ElementalState.Burning)
                    RemoveElementEffect(burningEffect);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FireSource>())
            {
                SetElementEffect(ElementalState.Burning, burningEffect);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            //TODO refactor this to generalise to other elements.
            BurnFlammableObjects(collision);
        }

        private void SetElementEffect(ElementalState elementalState, ParticleSystem elementalEffect)
        {
            ballState.ElementalState = elementalState;
            elementalEffect.Play();

            effectTimeLeft = elementEffectTime;
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