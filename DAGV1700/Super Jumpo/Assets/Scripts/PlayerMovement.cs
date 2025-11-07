using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isFacingRight = true;
    public ParticleSystem smokeFX;
    CapsuleCollider2D playerCollider;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    //isDashing Connector
    private DashAbility dashAbility;
    bool dashing;
    //WebSwing Connector
    private WebSwingAbility webSwingAbility;
    bool webSwinging;

    [Header("Movement")]
    public float baseMoveSpeed = 5f;
    public float sprintBoost = 3f;
    private float moveSpeed;
    float horizontalMovement;
    private bool isSprinting;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 2;
    int jumpsRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;
    bool isOnPlatform;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;

    //Wall Jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    void Start()
    {
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        moveSpeed = baseMoveSpeed; //For sprinting
        playerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dashAbility = GetComponent<DashAbility>();
        webSwingAbility = GetComponent<WebSwingAbility>();
    }

    void FixedUpdate()
    {
        dashing = dashAbility.isDashing;
        webSwinging = webSwingAbility.isSwinging;
        if(!dashing && !webSwinging)
        {
            UpdateAnimationState();
            GroundCheck();
            UpdateAnimationState();
            Gravity();
            UpdateAnimationState();
            ProcessWallSlide();
            UpdateAnimationState();
            ProcessWallJump();
            UpdateAnimationState();
            UpdateSpeed();
            Flip();

            if (!isWallJumping)
            {
                rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            }
            UpdateAnimationState();
        }
    }

    private void UpdateAnimationState()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isFalling", rb.linearVelocity.y < 0);
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetBool("isJumping", rb.linearVelocity.y > 0 && !isWallSliding);
        animator.SetBool("isSprinting", isSprinting && isGrounded);
    }

    private void Gravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //Fall increasingly faster
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(!dashing)
        {
            horizontalMovement = context.ReadValue<Vector2>().x;
        }
    }

    // Sprint input
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
            SoundEffectManager.Instance.PlaySound("SquishSFX2");
            smokeFX.Play();
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    private void UpdateSpeed()
    {
        moveSpeed = isSprinting ? baseMoveSpeed + sprintBoost : baseMoveSpeed;
    }

    //Player Drops through platform
    public void Drop(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded && isOnPlatform && playerCollider.enabled)
        {
            //Coroutine Dropping
            StartCoroutine(DisablePlayerCollider(0.25f));
        }
    }

    private IEnumerator DisablePlayerCollider(float disableTime)
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        playerCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PassPlatform"))
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PassPlatform"))
        {
            isOnPlatform = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(jumpsRemaining > 0)
        {
            if(context.performed)
            {   
                //Hold down jump button = full height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
                JumpFX();
                SoundEffectManager.Instance.PlaySound("JumpSFX1");
                
            }
            else if (context.canceled)
            {
                //Light tap of jump button = half the height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
                JumpFX();
            }
        }

        //Wall Jump
        if(context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y); //Jump away from the wall
            wallJumpTimer = 0;
            JumpFX();
            SoundEffectManager.Instance.PlaySound("JumpSFX1");
            

            //Force flip
            /*if(transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
                Debug.Log("FlipForced");
            }*/

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); //wall Jump = 0.5f -- Jump again = 0.6f
        }
    }

    private void JumpFX()
    {
        smokeFX.Play();
    }

    private void GroundCheck()
    {
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessWallSlide()
    {
        //Not grounded & On a Wall & movement != 0
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            Debug.Log("WallSliding");
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed)); //Caps fall rate
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime; 

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if(isFacingRight & horizontalMovement < 0 || !isFacingRight & horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
            spriteRenderer.flipX = !spriteRenderer.flipX;

            if(rb.linearVelocity.y == 0)
            {
                smokeFX.Play();
                if(isSprinting)
                {
                    SoundEffectManager.Instance.PlaySound("HaltSFX1");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Ground and Wall check visuals
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
