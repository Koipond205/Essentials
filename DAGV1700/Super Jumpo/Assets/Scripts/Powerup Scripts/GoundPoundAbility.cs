using UnityEngine;

public class GroundPoundAbility : MonoBehaviour
{
    [Header("Ground Pound")]
    public float groundPoundForce = 20f;
    public float groundPoundRadius = 5f;
    public LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private bool wasInAir = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    public void Activate()
    {
        // Check if the player is in the air
        if (!IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical velocity
            rb.AddForce(Vector2.down * groundPoundForce, ForceMode2D.Impulse); // Add downward force
            wasInAir = true;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the ground layer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            // Check if the player was previously in the air and the sound clip is assigned
            if (wasInAir)
            {
                SoundEffectManager.Instance.PlaySound("GroundPoundSFX2");
                wasInAir = false;
            }
        }
    }
}
