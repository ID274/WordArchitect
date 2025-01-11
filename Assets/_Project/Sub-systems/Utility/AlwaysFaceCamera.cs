using UnityEngine;
using System.Collections;

// Source: https://web.archive.org/web/20201111215913/http://wiki.unity3d.com/index.php?title=CameraFacingBillboard

public class AlwaysFaceCamera : MonoBehaviour
{
    public Camera cameraToFace;

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraToFace.transform.rotation * Vector3.forward, cameraToFace.transform.rotation * Vector3.up);
    }
}
