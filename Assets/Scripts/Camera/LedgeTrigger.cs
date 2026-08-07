using UnityEngine;
using System.Collections;

public class LedgeTrigger : MonoBehaviour
{
    [SerializeField] private CameraTarget cameraTarget;
    [SerializeField] private float ledgeOffset = -1.5f;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        player.LockJumpFallCamera = true;

        cameraTarget.SetYOffset(ledgeOffset);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        player.LockJumpFallCamera = false;

        cameraTarget.SetYOffset(0f);
    }
}