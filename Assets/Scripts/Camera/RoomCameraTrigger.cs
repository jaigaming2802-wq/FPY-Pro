using UnityEngine;

public class RoomCameraTrigger : MonoBehaviour
{
    [SerializeField] private CameraTarget cameraTarget;
    [SerializeField] private Transform roomCenter;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        cameraTarget.LockCamera(roomCenter.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        cameraTarget.UnlockCamera();
    }
}