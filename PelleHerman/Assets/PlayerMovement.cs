using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    //Moving
    [SerializeField] Rigidbody2D rb;
    private bool facingRight;
    public Vector2 moveInput;
    [SerializeField] float speed;

    //Jumping
    [SerializeField] Transform feet;
    [SerializeField] LayerMask ground;
    [SerializeField] float jumpForce;

    [SerializeField] float fallingCap;

    [SerializeField] float jumpBufferTime;
    [SerializeField] float jumpBufferCounter;

    [SerializeField] float megaJumpTime;
    [SerializeField] float megaJumpCounter;


    [SerializeField] float coyoteJumpTime;
    [SerializeField] float coyotejumpCounter;

    //WallClimbing
    [SerializeField] Transform hands;
    [SerializeField] float wallSlidingSpeed = 2;
    public bool isWallSliding;

    private bool canDash = true;
    public bool isDashing = false;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    public bool dashed;

    public bool megaJumping;

    public Animator anim;

    //WallJumping
    private bool isWalljumping;
    private float walljumpingDirection;
    private float wallJumpingTime = 0.1f;
    public float wallJumpingCounter;
    public float walljumpingDuration = 0.15f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if ((moveInput.x < -0.3f || moveInput.x > 0.3f) && !megaJumping)
        {
            if (!isWalljumping)
            {
                rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
                anim.SetBool("Walking", true);
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            anim.SetBool("Walking", false);
        }
    }
    void Update()
    {
        anim.SetBool("IsGrounded", IsGrounded());
        anim.SetFloat("Yvel", rb.velocity.y);
        anim.SetBool("Slide", isWallSliding);
        anim.SetBool("Dashing", isDashing);
        if (IsWalled())
        {
            isDashing = false;
            rb.gravityScale = 5f;
        }
        if (isDashing)
        {
            return;
        }
        if (Gamepad.current == null)
        {
            return;
        }
        if (Gamepad.current.buttonSouth.wasPressedThisFrame && !megaJumping)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        if (Gamepad.current.buttonSouth.wasReleasedThisFrame && rb.velocity.y > 0 && !megaJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Gamepad.current.rightShoulder.wasPressedThisFrame && canDash)
        {
            if (!megaJumping && !dashed)
            {
                StartCoroutine(Dash());
            }
        }
        if (Gamepad.current.leftShoulder.wasPressedThisFrame && !isWallSliding && IsGrounded())
        {
            megaJumpCounter = megaJumpTime;
            anim.SetBool("MegaJumpStart", true);
            megaJumping = true;
        }
        if (Gamepad.current.leftShoulder.wasReleasedThisFrame && !isWallSliding && IsGrounded())
        {
            if (megaJumpCounter < 0f && megaJumping)
            {
                MegaJump();
            }
            anim.SetBool("MegaJumpStart", false);
            anim.SetBool("MegaJumpDone", false);
            megaJumping = false;
        }
        if (megaJumping && megaJumpCounter < 0f)
        {
            anim.SetBool("MegaJumpDone", true);
        }
        if (megaJumpCounter >= 0f)
        {
            megaJumpCounter -= Time.deltaTime;
        }
        if (!isWalljumping)
        {
            if (moveInput.x < -0f)
            {
                transform.localScale = new Vector2(-1, 1);
                facingRight = false;
            }
            else if (moveInput.x > 0f)
            {
                transform.localScale = new Vector2(1, 1);
                facingRight = true;
            }
            else
            {
                moveInput.x = 0f;
            }
        }
        if (jumpBufferCounter > 0f)
        {
            if (coyotejumpCounter > 0f)
            {
                coyotejumpCounter = 0f;
                Jump();
            }
            else if (wallJumpingCounter > 0f)
            {
                WallJump();
            }
        }
        if (jumpBufferCounter >= 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        WallSlide();

        WallJumpActivator();
        if (!isDashing)
        {
            if (rb.velocity.y < -0.1f)
            {
                rb.gravityScale = 7f;
                speed = 8f;
            }
            else
            {
                rb.gravityScale = 5f;
                speed = 6f;
            }
        }
        if (IsGrounded())
        {
            coyotejumpCounter = coyoteJumpTime;
            dashed = false;
        }
        if (coyotejumpCounter > 0f)
        {
            coyotejumpCounter -= Time.deltaTime;
        }
        if (rb.velocity.y < fallingCap)
        {
            rb.velocity = new Vector2(rb.velocity.x, fallingCap);
        }
    }

    //MovementInput
    private void OnMove(InputValue inputValue)
    {
        moveInput = Gamepad.current.leftStick.ReadValue();
    }

    //JumpingInput
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void MegaJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.5f);
    }


    private void StopWallJumping()
    {
        isWalljumping = false;
    }

    //Wallcheck
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(hands.position, 0.05f, ground) && !IsGrounded() && rb.velocity.y < 0f;
    }

    private void WallSlide()
    {
        //Wallslide
        if (IsWalled() && !IsGrounded() && (moveInput.x < -0.1f || moveInput.x > 0.1f))
        {
            isWallSliding = true;
            dashed = false;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void WallJumpActivator()
    {
        if (isWallSliding == true)
        {
            isWalljumping = false;
            walljumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else if (wallJumpingCounter > 0f)
        {
            wallJumpingCounter -= Time.deltaTime;
        }
    }
    private void WallJump()
    {
        isWalljumping = true;
        rb.velocity = new Vector2(walljumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
        wallJumpingCounter = 0f;
        if (transform.localScale.x != walljumpingDirection)
        {
            facingRight = !facingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
        Invoke(nameof(StopWallJumping), walljumpingDuration);
    }
    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(new Vector2(feet.position.x - 0.2f, feet.position.y), 0.2f, ground))
        {
            return true;
        }
        else if (Physics2D.OverlapCircle(new Vector2(feet.position.x + 0.2f, feet.position.y), 0.2f, ground))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        dashed = true;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
