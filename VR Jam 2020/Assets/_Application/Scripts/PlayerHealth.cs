using UnityEngine;

namespace VRJam2020
{
    public class PlayerHealth : Health
    {
        protected override void Die()
        {
            Debug.Log("u ded. sad.");
        }
    }
}