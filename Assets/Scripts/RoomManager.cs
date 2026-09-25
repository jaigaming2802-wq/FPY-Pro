using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;

    private int currentRoom = 0;

    private void Awake()
    {
        // Game start la Room 1 mattum active
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].SetActive(i == 0);
        }
    }

    public void EnterRoom(int roomIndex)
    {
        currentRoom = roomIndex;
        UpdateRooms();
    }

    private void UpdateRooms()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            bool shouldBeActive =
                i >= currentRoom - 1 &&
                i <= currentRoom + 1;

            rooms[i].SetActive(shouldBeActive);
        }
    }
}