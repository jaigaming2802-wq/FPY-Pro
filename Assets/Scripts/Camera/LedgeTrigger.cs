using UnityEngine;

public class LedgeTrigger : MonoBehaviour
{
    [SerializeField] private CameraTarget cameraTarget;

    [SerializeField]
    private float lookDownOffset = -2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        cameraTarget.SetYOffset(lookDownOffset);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        cameraTarget.SetYOffset(0f);
    }
}