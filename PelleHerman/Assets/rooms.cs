using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rooms : MonoBehaviour
{
    public List<Room> RoomList = new List<Room>();
    public int currentRoom;
    public class Room
    {
        public Room(int roomNumber, float minX, float maxX, float minY, float maxY)
        {
            this.roomNumber = roomNumber;
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }
        int roomNumber;
        public float minX;
        public float maxX;
        public float minY;
        public float maxY;
    }
    // Start is called before the first frame update
    void Start()
    {
        RoomList.Add(new Room(0, 0, 0,0,0));
        RoomList.Add(new Room(1, 32, 64,0,0));
        RoomList.Add(new Room(2, 64, 64, 20, 40));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
