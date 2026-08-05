using UnityEngine;

public class CameraJumpFall : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraTarget cameraTarget;
    [SerializeField] private PlayerMovement player;

    [Header("Offsets")]
    [SerializeField] private float jumpOffset = 0.5f;
    [SerializeField] private float fallOffset = -1f;

    [Header("Threshold")]
    [SerializeField] private float jumpThreshold = 1f;
    [SerializeField] private float fallThreshold = -1f;

    [Header("Smooth")]
    [SerializeField] private float offsetSmooth = 4f;

    private float currentOffset;

    private void Update()
    {
        if (player == null || cameraTarget == null)
            return;

        float targetOffset = 0f;

        // Ground
        if (player.IsGrounded)
        {
            targetOffset = 0f;
        }
        // Jump
        else if (player.GetVerticalVelocity() > jumpThreshold)
        {
            targetOffset = jumpOffset;
        }
        // Fall
        else if (player.GetVerticalVelocity() < fallThreshold)
        {
            targetOffset = fallOffset;
        }

        currentOffset = Mathf.Lerp(
            currentOffset,
            targetOffset,
            offsetSmooth * Time.deltaTime);

        cameraTarget.SetYOffset(currentOffset);
    }
}