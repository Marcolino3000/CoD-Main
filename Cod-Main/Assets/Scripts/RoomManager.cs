using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public GameObject outside;
    public List<GameObject> indoorRooms; // Hallway, Kitchen, Bathroom

    private GameObject currentRoom;

    void Start()
    {
        ActivateRoom(outside);
    }

    public void ActivateRoom(GameObject newRoom)
    {
        if (currentRoom == newRoom) return;

        if (newRoom == outside)
        {
            // DRAUSSEN → alles aktiv
            outside.SetActive(true);

            foreach (GameObject room in indoorRooms)
            {
                room.SetActive(true);
            }
        }
        else
        {
            // DRINNEN → nur aktueller Raum aktiv
            outside.SetActive(false);

            foreach (GameObject room in indoorRooms)
            {
                room.SetActive(room == newRoom);
            }
        }

        currentRoom = newRoom;
    }
}