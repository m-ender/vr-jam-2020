using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace VRJam2020
{
    [RequireComponent(typeof(BallState))]
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Transform cameraRig = null;
        [SerializeField] private Transform head = null;
        [SerializeField] private Transform leftHand = null;
        [SerializeField] private Transform rightHand = null;
        [SerializeField] private GameObject BallCamQuad = null;
        [SerializeField] private Image solidColorOverlay = null;
        [SerializeField] private Renderer ballRenderer = null;

        [SerializeField] private float flyingSpeed = 10;
        [SerializeField] private float fadeTime = 1;

        [SerializeField] private float hoveringAmplitude = 0.2f;
        [SerializeField] private float hoveringFrequency = 2f;

        private Vector3 initialPosition;

        private BallState ballState;
        private new Rigidbody rigidbody;

        public HashSet<BallAbilities> unlockedAbilities { get; private set; } = new HashSet<BallAbilities>();

        private Transform targetHand;

        private SteamVR_Input_Sources leftHandSource;
        private SteamVR_Input_Sources rightHandSource;
        private SteamVR_Input_Sources anyHandSource;
        private SteamVR_Input_Sources activeHandSource;
        private SteamVR_Input_Sources inactiveHandSource;

        private bool isSpyMode;

        private void Awake()
        {
            ballState = GetComponent<BallState>();
            rigidbody = GetComponent<Rigidbody>();

            leftHandSource = SteamVR_Input_Sources.LeftHand;
            rightHandSource = SteamVR_Input_Sources.RightHand;

            anyHandSource = SteamVR_Input_Sources.Any;

            ballState.BaseState = BaseState.Hovering;
            initialPosition = transform.localPosition;
            TurnOnKinematic();
        }

        private void Update()
        {
            if (ballState.BaseState.IsBeingHeld())
                UpdateWhileHeld();
            else if (ballState.BaseState.IsFlying())
                UpdateWhileFlying();
            else
                UpdateWhileFree();

            if (SteamVR_Actions.default_ToggleGlow.GetStateDown(anyHandSource))
                ToggleGlow();

        }

        private void UpdateWhileFree()
        {
            if (ballState.BaseState == BaseState.Hovering)
            {
                float displacement = hoveringAmplitude * Mathf.Sin(Time.time * hoveringFrequency * 2 * Mathf.PI);
                transform.localPosition = initialPosition + Vector3.up * displacement;
            }

            if (SteamVR_Actions.default_SummonBall.GetState(leftHandSource))
                StartFlyingToLeftHand();
            else if (SteamVR_Actions.default_SummonBall.GetState(rightHandSource))
                StartFlyingToRightHand();

            if (SteamVR_Actions.default_BallCamera.GetStateDown(anyHandSource) && !isSpyMode)
                ActivateSpyMode();

            if (SteamVR_Actions.default_BallCamera.GetStateUp(anyHandSource))
                DeactivateSpyMode();
        }

        private void UpdateWhileFlying()
        {
            if (SteamVR_Actions.default_SummonBall.GetStateUp(activeHandSource))
                StopFlying();

            if (ballState.BaseState.IsFlying())
                FlyTowardsHand();
        }

        private void UpdateWhileHeld()
        {
            if (SteamVR_Actions.default_Bounce.GetStateDown(activeHandSource))
                ballState.CollisionState = CollisionState.Bounce;
            else if (SteamVR_Actions.default_Teleport.GetStateDown(activeHandSource))
                ballState.CollisionState = CollisionState.Teleport;
            else if (SteamVR_Actions.default_Sticky.GetStateDown(activeHandSource) 
                && unlockedAbilities.Contains(BallAbilities.Sticky))
                ballState.CollisionState = CollisionState.Sticky;
        }

        private void StartFlyingToLeftHand()
        {
            ballState.BaseState = BaseState.FlyingToLeftHand;
            targetHand = leftHand;
            TurnOnKinematic();
            ActivateLeftHand();
        }

        private void StartFlyingToRightHand()
        {
            ballState.BaseState = BaseState.FlyingToRightHand;
            targetHand = rightHand;
            TurnOnKinematic();
            ActivateRightHand();
        }

        private void StopFlying()
        {
            ballState.BaseState = BaseState.Free;
            TurnOffKinematic();
            rigidbody.velocity = (targetHand.position - transform.position).normalized * flyingSpeed;
        }

        private void FlyTowardsHand()
        {
            if (ballState.CollisionState == CollisionState.Sticky)
            {
                Unstick();
                ballState.CollisionState = CollisionState.Bounce;
            }

            if (isSpyMode)
                DeactivateSpyMode();

            Vector3 vectorToHand = targetHand.position - transform.position;
            float distance = Mathf.Min(flyingSpeed * Time.deltaTime, vectorToHand.magnitude);

            transform.position += vectorToHand.normalized * distance;
        }

        public void OnPickedUp()
        {
            if (ballState.CollisionState == CollisionState.Sticky)
                ballState.CollisionState = CollisionState.Bounce;

            if (isSpyMode)
                DeactivateSpyMode();

            switch (transform.parent.GetComponent<Hand>().handType)
            {
            case SteamVR_Input_Sources.LeftHand:
                ballState.BaseState = BaseState.HeldInLeftHand;
                ActivateLeftHand();
                break;
            case SteamVR_Input_Sources.RightHand:
                ballState.BaseState = BaseState.HeldInRightHand;
                ActivateRightHand();
                break;
            default:
                Debug.Log($"What now? Ball got attached to {transform.parent.name}.");
                break;
            }
        }

        public void OnLetGo()
        {
            //forces an Unstick incase ball was picked up directly when sticky.
            Unstick();
            ballState.BaseState = BaseState.Free;
            activeHandSource = anyHandSource;
        }

        public void Unlock(BallAbilities grantedAbility)
        {
            unlockedAbilities.Add(grantedAbility);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (ballState.CollisionState == CollisionState.Teleport
                && collision.gameObject.GetComponent<TeleportTarget>())
            {
                if (IsFloorCollision(collision))
                {
                    TeleportCameraRigToBall();
                    ballState.CollisionState = CollisionState.Bounce;
                    StopBall(collision);
                }
            }

            if (ballState.CollisionState == CollisionState.Bounce)
            {
                var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemyHealth)
                    enemyHealth.TakeDamage(ballState.ElementalState == ElementalState.Burning ? 3 : 1);
            }

            if (ballState.CollisionState == CollisionState.Sticky
                && !collision.gameObject.transform.root.GetComponent<Player>())
                StickOnCollision(collision);
        }

        // Based on http://answers.unity.com/answers/650322/view.html
        private bool IsFloorCollision(Collision collision)
        {
            // find collision point and normal. You may want to average over all contacts
            ContactPoint contactPoint = collision.GetContact(0);
            Vector3 point = contactPoint.point;
            Vector3 dir = -contactPoint.normal; // you need vector pointing TOWARDS the collision, not away from it
            // step back a bit
            point -= dir;
            // cast a ray twice as far as your step back. This seems to work in all
            // situations, at least when speeds are not ridiculously big
            if (collision.collider.Raycast(new Ray(point, dir), out RaycastHit hitInfo, 2))
            {
                // this is the collider surface normal
                Vector3 normal = hitInfo.normal;

                float angleToVertical = Vector3.Angle(Vector3.up, normal);

                return angleToVertical < 45;
            }

            return false;
        }

        private Vector3 getTrueCollisionPoint(Collision collision)
        {
            // find collision point and normal. You may want to average over all contacts
            ContactPoint contactPoint = collision.GetContact(0);
            Vector3 point = contactPoint.point;
            Vector3 dir = -contactPoint.normal; // you need vector pointing TOWARDS the collision, not away from it
            // step back a bit
            point -= dir;

            SphereCollider sphere = gameObject.GetComponentInChildren<SphereCollider>();
            Vector3 relativeBallPos = new Vector3();

            float ballRadius = sphere.transform.localScale.x * sphere.radius;

            // cast a ray twice as far as your step back. This seems to work in all
            // situations, at least when speeds are not ridiculously big
            if (collision.collider.Raycast(new Ray(point, dir), out RaycastHit hitInfo, 2))
            {
                // this is the collider surface normal
                Vector3 normal = hitInfo.normal;
                relativeBallPos = normal.normalized * ballRadius;

            }

                return (contactPoint.point + relativeBallPos);
        }


        private void StopBall(Collision collision)
        {
            transform.position = getTrueCollisionPoint(collision);
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        private void TeleportCameraRigToBall()
        {
            Vector3 groundPosition = new Vector3(head.position.x, cameraRig.position.y, head.position.z);
            Vector3 translateVector = transform.position - groundPosition;

            DOTween.Sequence()
                .Append(solidColorOverlay.DOColor(Color.black, fadeTime))
                .AppendCallback(() => cameraRig.position += translateVector)
                .Append(solidColorOverlay.DOColor(Color.clear, fadeTime));
        }

        private void StickOnCollision(Collision collision)
        {
            //Empty game object assigned to parent of ball, so that parented objects don't scale the ball.
            StopBall(collision);
            GameObject glue = new GameObject("Glue");
            glue.transform.SetParent(collision.transform);
            transform.SetParent(glue.transform);
            TurnOnKinematic();
        }

        public void Unstick()
        {
            transform.parent = null;
            TurnOffKinematic();
        }

        private void ActivateSpyMode()
        {
            if (!unlockedAbilities.Contains(BallAbilities.Spy))
                return;

            isSpyMode = true;
            BallCamQuad.SetActive(true);
            var ballCam = GetComponentInChildren<BallCameraController>();
            ballCam.gameObject.GetComponent<Camera>().enabled = true;
            ballCam.targetTransform = head;
        }

        private void DeactivateSpyMode()
        {
            if (!unlockedAbilities.Contains(BallAbilities.Spy))
                return;

            isSpyMode = false;
            BallCamQuad.SetActive(false);
            var ballCam = GetComponentInChildren<BallCameraController>();
            ballCam.gameObject.GetComponent<Camera>().enabled = false;
        }

        private void ActivateLeftHand()
        {
            activeHandSource = leftHandSource;
            inactiveHandSource = rightHandSource;
        }

        private void ActivateRightHand()
        {
            activeHandSource = rightHandSource;
            inactiveHandSource = leftHandSource;
        }

        private void ToggleGlow()
        {
            if (!unlockedAbilities.Contains(BallAbilities.Glow))
                return;

            ballState.IsGlowing = !ballState.IsGlowing;

            Material ballMaterial = ballRenderer.material;
            
            if(ballState.IsGlowing)
            {
                ballMaterial.EnableKeyword("_EMISSION");

                if (ballState.CollisionState == CollisionState.Teleport)
                    ballMaterial.SetColor("_EmissionColor", new Color(0, 0.7490196f, 0.8117647f)*4);
                if (ballState.CollisionState == CollisionState.Sticky)
                    ballMaterial.SetColor("_EmissionColor", new Color(0.06517824f, 0.28f, 0.079f)*2);
                if (ballState.CollisionState == CollisionState.Bounce)
                    ballMaterial.SetColor("_EmissionColor", new Color(0.6f, 0.6f, 0.6f));
            }
            else
            {
                if (ballState.CollisionState == CollisionState.Teleport)
                    ballMaterial.SetColor("_EmissionColor", new Color(0, 0.7490196f, 0.8117647f, 0f));
                if (ballState.CollisionState == CollisionState.Sticky)
                    ballMaterial.SetColor("_EmissionColor", new Color(0.06517824f, 0.28f, 0.079f));
                if (ballState.CollisionState == CollisionState.Bounce)
                    ballMaterial.SetColor("_EmissionColor", new Color(0f, 0f, 0f, 0f)*1);
            }
        }

        private void TurnOnKinematic()
        {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.isKinematic = true;
        }

        private void TurnOffKinematic()
        {
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    } 
}
