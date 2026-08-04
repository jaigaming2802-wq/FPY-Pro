using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    private void LateUpdate()
    {
        if (player == null)
            return;

        transform.position = player.position;
    }
}