using UnityEngine;
using UnityEngine.Events;

namespace VRJam2020
{
    public class AttackEvent : MonoBehaviour
    {
        public UnityEvent OnAttack;

        public void Attack()
        {
            OnAttack.Invoke();
        }
    }
}
