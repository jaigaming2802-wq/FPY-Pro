using UnityEngine;

public class AreaRevealManager : MonoBehaviour
{
    [SerializeField] private CameraTarget cameraTarget;

    private bool unlocked;

    public void EnterArea()
    {
        if (unlocked)
            return;

        cameraTarget.SetCameraLock(true);
    }

    public void CancelArea()
    {
        if (unlocked)
            return;

        cameraTarget.SetCameraLock(false);
    }

    public void CompleteArea()
    {
        if (unlocked)
            return;

        unlocked = true;

        cameraTarget.SetCameraLock(false);

        Destroy(gameObject);
    }
}