using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Follow")]
    [SerializeField] private float ySmoothTime = 0.15f;

    private float currentY;
    private float yVelocity;

    private float currentYOffset;

    private void Start()
    {
        currentY = player.position.y;
    }

    private void LateUpdate()
    {
        if (player == null)
            return;

        float targetY = player.position.y + currentYOffset;

        currentY = Mathf.SmoothDamp(
            currentY,
            targetY,
            ref yVelocity,
            ySmoothTime);

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
