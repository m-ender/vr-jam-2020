using UnityEngine;

namespace VRJam2020
{
    public class BallState : MonoBehaviour
    {
        [SerializeField] private MeshRenderer ballRenderer = null;

        [SerializeField] private CollisionState initialControllableState = CollisionState.Bounce;
        [SerializeField] private Color bounceColour = Color.white;
        [SerializeField] private Color teleportColour = Color.white;
        [SerializeField] private Color stickyColour = Color.white;

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
                ballRenderer.material.color = bounceColour;
                break;
            case CollisionState.Teleport:
                ballRenderer.material.color = teleportColour;
                break;
            case CollisionState.Sticky:
                ballRenderer.material.color = stickyColour;
                break;
            }
        }
    }
}