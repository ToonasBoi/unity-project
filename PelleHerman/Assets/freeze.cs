using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeze : MonoBehaviour
{
    public roomTransition room;
    public Rigidbody2D rb;
    void Update()
    {
        if (room.transitioning)
        {
            rb.isKinematic = true;
        }
    }
}
