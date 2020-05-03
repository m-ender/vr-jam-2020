using UnityEngine;

namespace VRJam2020
{
    public class BallState : MonoBehaviour
    {
        [SerializeField] private ControllableState initialControllableState;

        public ControllableState ControllableState { get; set; }
        public ElementalState ElementalState { get; set; } = ElementalState.None;
        public bool IsGlowing { get; set; } = false;

        private void Awake()
        {
            ControllableState = initialControllableState;
        }
    }
}