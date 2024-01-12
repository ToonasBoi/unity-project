
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{ 
    public BoxCollider2D groundcheck;
    public bool isgrounded; 
    public LayerMask ground;
    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    public Vector2 movement;
    // Update is called once per frame
    void Update()
     
    {
        isgrounded = Physics2D.BoxCast(groundcheck.bounds.center, groundcheck.bounds.size, 0f, Vector2.down, 0.1f,ground);

        movement = Gamepad.current.leftStick.ReadValue();
        if (Gamepad.current.buttonSouth.wasPressedThisFrame && isgrounded==true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);

        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x *speed * Time.fixedDeltaTime, rb.velocity.y);
    }
}
