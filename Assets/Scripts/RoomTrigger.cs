using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private int roomIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        roomManager.EnterRoom(roomIndex);
    }
}