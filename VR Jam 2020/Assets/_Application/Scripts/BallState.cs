using System;
using UnityEngine;

namespace VRJam2020
{
    public class BallState : MonoBehaviour
    {
        [SerializeField] private MeshRenderer ballRenderer = null;
        [SerializeField] private Light ballGlow = null;

        [SerializeField] private CollisionState initialControllableState = CollisionState.Bounce;
        [SerializeField] private Material bounceMaterial = null;
        [SerializeField] private Material teleportMaterial = null;
        [SerializeField] private Material stickyMaterial = null;
        [SerializeField] private ParticleSystem teleportEffect = null;

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
        
        private bool _isGlowing = false;
        public bool IsGlowing
        {
            get => _isGlowing;
            set
            {
                _isGlowing = value;
                UpdateGlowEffect();
            }
        }

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
                teleportEffect.Play();
                break;
            case CollisionState.Sticky:
                ballRenderer.material = stickyMaterial;
                break;
            }

            if (IsGlowing)
            {
                ballRenderer.material.EnableKeyword("_EMISSION");

                if (CollisionState == CollisionState.Teleport)
                    ballRenderer.material.SetColor("_EmissionColor", new Color(0, 0.7490196f, 0.8117647f)*4);
                if (CollisionState == CollisionState.Sticky)
                    ballRenderer.material.SetColor("_EmissionColor", new Color(0.06517824f, 0.28f, 0.079f)*2);
                if (CollisionState == CollisionState.Bounce)
                    ballRenderer.material.SetColor("_EmissionColor", new Color(0.6f, 0.6f, 0.6f));
            }
            else
            {
                if (CollisionState == CollisionState.Teleport)
                    ballRenderer.material.SetColor("_EmissionColor", new Color(0, 0.7490196f, 0.8117647f, 0f));
                if (CollisionState == CollisionState.Sticky)
                    ballRenderer.material.SetColor("_EmissionColor", new Color(0.06517824f, 0.28f, 0.079f));
                if (CollisionState == CollisionState.Bounce)
                    ballRenderer.material.SetColor("_EmissionColor", new Color(0f, 0f, 0f, 0f) * 1);
            }
        }

        private void UpdateGlowEffect()
        {
            ballGlow.enabled = IsGlowing;
        }
    }
}