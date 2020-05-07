using System.Collections;

namespace VRJam2020
{
    public class EnemyHealth : Health
    {
        protected override void Die()
        {
            StartCoroutine(DieNextFrame());
        }

        private IEnumerator DieNextFrame()
        {
            yield return null;

            Destroy(gameObject);
        }
    }
}