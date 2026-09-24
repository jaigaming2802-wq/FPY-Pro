using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [SerializeField] private Transform startingRespawnPoint;

    private Vector3 lastCheckpointPosition;
    private Quaternion lastCheckpointRotation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (startingRespawnPoint != null)
        {
            lastCheckpointPosition =
                startingRespawnPoint.position;

            lastCheckpointRotation =
                startingRespawnPoint.rotation;
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        if (checkpoint == null)
            return;

        lastCheckpointPosition =
            checkpoint.position;

        lastCheckpointRotation =
            checkpoint.rotation;

        Debug.Log(
            "Checkpoint Saved: " +
            checkpoint.name);
    }

    public Vector3 GetCheckpointPosition()
    {
        return lastCheckpointPosition;
    }

    public Quaternion GetCheckpointRotation()
    {
        return lastCheckpointRotation;
    }
}