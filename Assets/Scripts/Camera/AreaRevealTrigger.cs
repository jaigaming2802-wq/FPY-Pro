using UnityEngine;

public class AreaRevealTrigger : MonoBehaviour
{
    [SerializeField] private CameraTarget cameraTarget;

    private bool unlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        cameraTarget.SetCameraLock(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (unlocked)
            return;

        if (!other.CompareTag("Player"))
            return;

        unlocked = true;

        cameraTarget.SetCameraLock(false);

        Destroy(gameObject);
    }
}   