using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public PlayerMovement playerMovement;
   
    [Header("Smooth")]
    public float xSmoothTime = 0.25f;
    public float ySmoothTime = 0.08f;

    private float currentX;
    private float currentY;

    private float xVelocity;
    private float yVelocity;

    private float yOffset;

    // --------------------------
    // Area Reveal Lock
    // --------------------------
    private bool cameraLocked = false;

    // --------------------------
    // Room Lock
    // --------------------------
    private bool roomLocked = false;
    private Vector3 lockedPosition;

    private void Start()
    {
        currentX = player.position.x;
        currentY = player.position.y;
    }

    private void LateUpdate()
    {
        //=========================================
        // ROOM LOCK
        //=========================================

        if (roomLocked)
        {
            currentX = Mathf.SmoothDamp(
                currentX,
                lockedPosition.x,
                ref xVelocity,
                xSmoothTime);

            currentY = Mathf.SmoothDamp(
                currentY,
                lockedPosition.y,
                ref yVelocity,
                ySmoothTime);

            transform.position = new Vector3(
                currentX,
                currentY,
                player.position.z);

            return;
        }

        //=========================================
        // AREA REVEAL LOCK
        //=========================================

        if (cameraLocked)
        {
            transform.position = new Vector3(
                currentX,
                currentY,
                player.position.z);

            return;
        }

        float targetX = player.position.x;

        currentX = Mathf.SmoothDamp(
            currentX,
            targetX,
            ref xVelocity,
            xSmoothTime);

        //=========================================
        // Ledge Camera
        //=========================================

        float targetY = player.position.y + yOffset;

        currentY = Mathf.SmoothDamp(
            currentY,
            targetY,
            ref yVelocity,
            ySmoothTime);

        transform.position = new Vector3(
            currentX,
            currentY,
            player.position.z);
    }

    //=========================================
    // Ledge
    //=========================================

    public void SetYOffset(float offset)
    {
        yOffset = offset;
    }

    //=========================================
    // Area Reveal
    //=========================================

    public void SetCameraLock(bool value)
    {
        cameraLocked = value;
    }

    //=========================================
    // Room Lock
    //=========================================

    public void LockCamera(Vector3 roomCenter)
    {
        roomLocked = true;
        lockedPosition = roomCenter;
    }

    public void UnlockCamera()
    {
        roomLocked = false;
    }
}