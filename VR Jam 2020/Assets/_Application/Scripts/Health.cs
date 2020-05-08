using UnityEngine;

namespace VRJam2020
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int initialHealth = 3;

        [ReadOnly]
        [SerializeField] protected int currentHealth;

        [ReadOnly]
        [SerializeField] protected bool isOneHitKill;

        private void Awake()
        {
            currentHealth = initialHealth;
        }
        public void TakeDamage(int amount)
        {
            if (initialHealth <= amount)
                isOneHitKill = true;

            currentHealth -= amount;

            if (currentHealth <= 0)
                Die();
            else
                ShowDamageFeedback();

        }

        protected virtual void ShowDamageFeedback() { }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}