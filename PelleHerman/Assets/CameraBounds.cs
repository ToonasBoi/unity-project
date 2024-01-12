using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public rooms rooms;
    public Transform cam;
    public Transform player;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.position = new Vector2(player.position.x, player.position.y);
        if(cam.position.x > rooms.RoomList[rooms.currentRoom].maxX)
        {
            cam.position = new Vector3(rooms.RoomList[rooms.currentRoom].maxX, cam.position.y, -10);
        }
        if (cam.position.x < rooms.RoomList[rooms.currentRoom].minX)
        {
            cam.position = new Vector3(rooms.RoomList[rooms.currentRoom].minX, cam.position.y, -10);
        }
        if (cam.position.y > rooms.RoomList[rooms.currentRoom].maxY)
        {
            cam.position = new Vector3(cam.position.x, rooms.RoomList[rooms.currentRoom].maxY, -10);
        }
        if (cam.position.y < rooms.RoomList[rooms.currentRoom].minY)
        {
            cam.position = new Vector3(cam.position.x, rooms.RoomList[rooms.currentRoom].minY, -10);
        }
    }
}
