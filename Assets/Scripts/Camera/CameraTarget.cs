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

    // Camera Lock
    private bool cameraLocked;
    private Vector3 lockedPosition;

    private bool roomLocked;
    private Vector3 roomLockPosition;
    [SerializeField] private float roomLockSmoothTime = 0.4f;

    private Vector3 roomVelocity;

    private void Start()
    {
        currentY = player.position.y;
    }

    private void LateUpdate()
    {
        if (player == null || playerMovement == null)
            return;

        // Camera Lock
        if (cameraLocked)
        {
            transform.position = lockedPosition;
            return;
        }
        if (roomLocked)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                roomLockPosition,
                ref roomVelocity,
                roomLockSmoothTime);

            return;
        }
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

    public void SetCameraLock(bool value)
    {
        cameraLocked = value;

        if (value)
        {
            lockedPosition = transform.position;
        }
    }
    public void LockCamera(Vector3 position)
    {
        roomLocked = true;
        roomLockPosition = position;
    }

    public void UnlockCamera()
    {
        roomLocked = false;
    }
}