using UnityEngine;

namespace VRJam2020
{
    public class BallState : MonoBehaviour
    {
        [SerializeField] private MeshRenderer ballRenderer = null;

        [SerializeField] private CollisionState initialControllableState = CollisionState.Bounce;
        [SerializeField] private Material bounceMaterial = null;
        [SerializeField] private Material teleportMaterial = null;
        [SerializeField] private Material stickyMaterial = null;

        public BaseState BaseState { get; set; }

        private CollisionState _collisionState;
        public CollisionState CollisionState
        {
            get => _collisionState;
            set
            {
                _collisionState = value;
                UpdateBallColour();
            }
        }

        public ElementalState ElementalState { get; set; } = ElementalState.None;
        public bool IsGlowing { get; set; } = false;

        private void Awake()
        {
            CollisionState = initialControllableState;
        }

        private void UpdateBallColour()
        {
            switch (CollisionState)
            {
            case CollisionState.Bounce:
                ballRenderer.material = bounceMaterial;
                break;
            case CollisionState.Teleport:
                ballRenderer.material = teleportMaterial;
                break;
            case CollisionState.Sticky:
                ballRenderer.material = stickyMaterial;
                break;
            }
        }
    }
}