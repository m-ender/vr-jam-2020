using System.Collections;
using UnityEngine;
using Valve.VR;

namespace VRJam2020
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BallState))]
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private float fadeTime;
        [SerializeField] private SteamVR_Action_Boolean toggleTeleport;
        //TODO find out how to reference Player position / head position without assigning field.
        [SerializeField] private Transform cameraRig;
        [SerializeField] private Transform head;

        private bool isTeleporting;
        private BallState ballState;

        private void Awake()
        {
            ballState = GetComponent<BallState>();
        }
        private void Update()
        {
            if (toggleTeleport.GetStateUp(SteamVR_Input_Sources.Any) /* && ball is in hand*/)
            {
                ballState.ControllableState = ControllableState.Teleport;
            }

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<TeleportTarget>() 
                && ballState.ControllableState == ControllableState.Teleport)
            {
                TeleportCameraRigToThis();
                ballState.ControllableState = ControllableState.Bounce;
            }
        }

        //TODO better names?!
        private void TeleportCameraRigToThis()
        {
            if (isTeleporting)
                return;

            Vector3 groundPosition = new Vector3(head.position.x, cameraRig.position.y, head.position.z);
            Vector3 translateVector = transform.position - groundPosition;

            StartCoroutine(MoveRig(cameraRig, translateVector));
        }

        private IEnumerator MoveRig(Transform cameraRig, Vector3 translation)
        {
            isTeleporting = true;

            SteamVR_Fade.Start(Color.black, fadeTime, true);

            yield return new WaitForSeconds(fadeTime);
            cameraRig.position += translation;

            SteamVR_Fade.Start(Color.clear, fadeTime, true);

            isTeleporting = false;
        }
    } 
}
