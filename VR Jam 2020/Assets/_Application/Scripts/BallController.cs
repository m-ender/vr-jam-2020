﻿using System;
using System.Collections;
using UnityEngine;
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

        [SerializeField] private float flyingSpeed = 10;
        [SerializeField] private float fadeTime = 1;

        private BallState ballState;
        private new Rigidbody rigidbody;

        private Transform targetHand;

        private SteamVR_Input_Sources leftHandSource;
        private SteamVR_Input_Sources rightHandSource;
        private SteamVR_Input_Sources anyHandSource;
        private SteamVR_Input_Sources activeHandSource;
        private SteamVR_Input_Sources inactiveHandSource;

        private Transform activeHandTransform;

        private bool isSpyMode;

        private void Awake()
        {
            ballState = GetComponent<BallState>();
            rigidbody = GetComponent<Rigidbody>();

            leftHandSource = SteamVR_Input_Sources.LeftHand;
            rightHandSource = SteamVR_Input_Sources.RightHand;

            anyHandSource = SteamVR_Input_Sources.Any;

            ballState.BaseState = BaseState.Free;
        }

        private void Update()
        {
            if (ballState.BaseState.IsBeingHeld())
                UpdateWhileHeld();
            else if (ballState.BaseState.IsFlying())
                UpdateWhileFlying();
            else
                UpdateWhileFree();
        }

        private void UpdateWhileFree()
        {
            if (SteamVR_Actions.default_SummonBall.GetState(leftHandSource))
                StartFlyingToLeftHand();
            else if (SteamVR_Actions.default_SummonBall.GetState(rightHandSource))
                StartFlyingToRightHand();

            if (SteamVR_Actions.default_BallCamera.GetStateDown(activeHandSource))
                ActivateSpyMode();

            if (SteamVR_Actions.default_BallCamera.GetStateUp(activeHandSource))
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
            else if (SteamVR_Actions.default_Sticky.GetStateDown(activeHandSource))
                ballState.CollisionState = CollisionState.Sticky;
        }

        private void StartFlyingToLeftHand()
        {
            ballState.BaseState = BaseState.FlyingToLeftHand;
            targetHand = leftHand;
            ActivateLeftHand();
        }

        private void StartFlyingToRightHand()
        {
            ballState.BaseState = BaseState.FlyingToRightHand;
            targetHand = rightHand;
            ActivateRightHand();
        }

        private void StopFlying()
        {
            ballState.BaseState = BaseState.Free;
        }

        private void FlyTowardsHand()
        {
            if (ballState.CollisionState == CollisionState.Sticky)
                Unstick();

            if (isSpyMode)
                DeactivateSpyMode();

            rigidbody.velocity = (targetHand.position - transform.position).normalized * flyingSpeed;
        }

        public void OnPickedUp()
        {
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
            ballState.BaseState = BaseState.Free;
            activeHandSource = anyHandSource;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (ballState.CollisionState == CollisionState.Teleport
                && collision.gameObject.GetComponent<TeleportTarget>())
            {
                TeleportCameraRigToBall();
                ballState.CollisionState = CollisionState.Bounce;
                StopBall();
            }

            //GetComponentInChildren couldn't detect the player component for some reason.

            if (ballState.CollisionState == CollisionState.Sticky
                && !collision.gameObject.transform.root.GetComponent<Player>())
                StickOnCollision(collision);
        }

        private void StopBall()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        private void TeleportCameraRigToBall()
        {
            Vector3 groundPosition = new Vector3(head.position.x, cameraRig.position.y, head.position.z);
            Vector3 translateVector = transform.position - groundPosition;

            StartCoroutine(MoveRig(cameraRig, cameraRig.position + translateVector));
        }

        // TODO: This method should probably live somewhere on the camera rig.
        private IEnumerator MoveRig(Transform cameraRig, Vector3 targetPosition)
        {
            SteamVR_Fade.Start(Color.black, fadeTime, true);

            yield return new WaitForSeconds(fadeTime);

            cameraRig.position = targetPosition;

            SteamVR_Fade.Start(Color.clear, fadeTime, true);
        }

        private void StickOnCollision(Collision collision)
        {
            //Empty game object assigned to parent of ball, so that parented objects don't scale the ball.
            GameObject glue = new GameObject("Glue");
            glue.transform.SetParent(collision.transform);
            transform.SetParent(glue.transform);
            StopBall();
            rigidbody.isKinematic = true;
        }

        public void Unstick()
        {
            transform.parent = null;
            rigidbody.isKinematic = false;
            ballState.CollisionState = CollisionState.Bounce;
        }
        
        private void ActivateLeftHand()
        {
            activeHandSource = leftHandSource;
            activeHandTransform = leftHand;
            inactiveHandSource = rightHandSource;
        }

        private void ActivateRightHand()
        {
            activeHandSource = rightHandSource;
            activeHandTransform = rightHand;
            inactiveHandSource = leftHandSource;
        }

        private void ActivateSpyMode()
        {
            isSpyMode = true;
            BallCamQuad.SetActive(true);
            var ballCam = GetComponentInChildren<BallCameraController>();
            ballCam.gameObject.GetComponent<Camera>().enabled = true;
            //transform.SetPositionAndRotation(ballCam.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            ballCam.targetTransform = activeHandTransform;
        }

        private void DeactivateSpyMode()
        {
            isSpyMode = false;
            BallCamQuad.SetActive(false);
            var ballCam = GetComponentInChildren<BallCameraController>();
            ballCam.gameObject.GetComponent<Camera>().enabled = true;
        }
    } 
}
