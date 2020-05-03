using System.Collections;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))] 
public class Teleport : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    [SerializeField] private SteamVR_Action_Boolean teleportAction;
    //TODO find out how to reference Player position / head position without assigning field.
    [SerializeField] private Transform cameraRig;
    [SerializeField] private Transform head;

    private bool isTeleportable;
    private bool isTeleporting;
    void Update()
    {
        if (teleportAction.GetStateUp(SteamVR_Input_Sources.Any))
            TeleportCameraRigToThis();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO discuss if more logic is required for teleportable objects otherwise use tag instead.
        if (collision.gameObject.GetComponent<TeleportTarget>())
            isTeleportable = true;
        else
            isTeleportable = false;
    }

    //TODO better names?!
    private void TeleportCameraRigToThis()
    {
        if (!isTeleportable || isTeleporting)
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
