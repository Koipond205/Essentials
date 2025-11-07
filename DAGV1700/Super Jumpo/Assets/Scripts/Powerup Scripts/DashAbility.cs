using UnityEngine;
using System.Collections;

public class DashAbility : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private TrailRenderer trailRenderer;

    [Header("Dashing")]
    public float dashSpeed = 20;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    public bool isDashing;
    public bool canDash = true;
    bool facingRight;
 
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void Activate()
    {
        if(canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        canDash = false;
        isDashing = true;
        SoundEffectManager.Instance.PlaySound("SwooshSFX1");
        trailRenderer.emitting = true;

        facingRight = playerMovement.isFacingRight;
        float dashDirection = facingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y); //Dash Movement 

        yield return new WaitForSeconds(dashDuration);

        if (isDashing) // Check in case another action stopped the dash
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            isDashing = false;
            trailRenderer.emitting = false;
        } // Reset horizontal velocity

        isDashing = false;
        trailRenderer.emitting = false;
        Physics2D.IgnoreLayerCollision(8, 9, false);

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}
