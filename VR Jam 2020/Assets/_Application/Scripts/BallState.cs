using UnityEngine;

namespace VRJam2020
{
    public class BallState : MonoBehaviour
    {
        [SerializeField] private ControllableState initialControllableState;
        [SerializeField] private Color32 bounceColour;
        [SerializeField] private Color32 teleportColour;
        [SerializeField] private Color32 stickyColour;
        public ControllableState ControllableState { get; set; }
        public ElementalState ElementalState { get; set; } = ElementalState.None;
        public bool IsGlowing { get; set; } = false;

        private Material ballMaterial;

        private void Awake()
        {
            ballMaterial = GetComponentInChildren<Renderer>().material;
            ControllableState = initialControllableState;
        }

        private void Update()
        {
            UpdateBallColour();
        }

        private void UpdateBallColour()
        {
            switch (ControllableState)
            {
            case ControllableState.Bounce:
                ballMaterial.color = bounceColour;
                break;
            case ControllableState.Teleport:
                ballMaterial.color = teleportColour;
                break;
            case ControllableState.Sticky:
                ballMaterial.color = stickyColour;
                break;
            default:
                break;
            }

        }
    }
}