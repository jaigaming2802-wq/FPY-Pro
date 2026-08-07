using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Follow")]
    [SerializeField] private float ySmoothTime = 0.15f;
    [SerializeField] private float ledgeSmoothTime = 0.4f;

    private float currentY;
    private float yVelocity;

    private float currentYOffset;

    private void Start()
    {
        currentY = player.position.y;
    }

    private void LateUpdate()
    {
        if (player == null || playerMovement == null)
            return;

        float targetY = player.position.y + currentYOffset;

        float smoothTime = playerMovement.LockJumpFallCamera
            ? ledgeSmoothTime
            : ySmoothTime;

        currentY = Mathf.SmoothDamp(
            currentY,
            targetY,
            ref yVelocity,
            smoothTime);

        transform.position = new Vector3(
            player.position.x,
            currentY,
            player.position.z);
    }

    public void SetYOffset(float offset)
    {
        currentYOffset = offset;
    }
}