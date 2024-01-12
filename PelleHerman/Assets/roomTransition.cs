using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomTransition : MonoBehaviour
{
    public bool transitioning;
    public Transform cam;
    private float x, y;
    public Transform p;
    public bool vertical;
    public Rigidbody2D prb;
    private float Ydirection;
    public int RoomLeftDown;
    public int RoomRightUp;
    public rooms rooms;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transitioning = true;
            x = cam.position.x;
            y = cam.position.y;
        }
    }
    private void FixedUpdate()
    {
        if(prb.velocity.y > 0)
        {
            Ydirection = 1;
        }
        if (prb.velocity.y < 0)
        {
            Ydirection = -1;
        }
        if (transitioning && !vertical)
        {
            cam.position = Vector3.Lerp(new Vector3(x, y, -10f), new Vector3(x + 32f * p.localScale.x, y, -10f), 1f);
            transitioning = false;
            p.position = new Vector2(p.position.x + 2.1f * p.localScale.x, p.position.y);
            if(prb.velocity.x > 0)
            {
                rooms.currentRoom = RoomRightUp;
            }
            else
            {
                rooms.currentRoom = RoomLeftDown;
            }
        }
        else if (transitioning) 
        {
            cam.position = Vector3.Lerp(new Vector3(x, y, -10f), new Vector3(x, y + 20 * Ydirection, -10f), 1f);
            transitioning = false;
            p.position = new Vector2(p.position.x, p.position.y + 2.1f * Ydirection);
            if(Ydirection == 1f)
            {
                prb.velocity = new Vector2(prb.velocity.x, 22);
                rooms.currentRoom = RoomRightUp;
            }
            else
            {
                rooms.currentRoom = RoomLeftDown;
            }
        }
    }
}
