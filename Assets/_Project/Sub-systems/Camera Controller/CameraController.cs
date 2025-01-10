using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform cameraPivot;

    [SerializeField] private Vector3 offset;
    private void Update()
    {
        Vector3 newPos = BlockTracker.Instance.nextBlockPosition + offset;
        aimTarget.position = Vector3.Lerp(aimTarget.position, newPos, Time.deltaTime * 10);
    }
}