using UnityEngine;

namespace VRJam2020
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int initialHealth = 3;

        [ReadOnly]
        [SerializeField] protected int currentHealth;

        private void Awake()
        {
            currentHealth = initialHealth;
        }
        public void TakeDamage(int amount)
        {
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