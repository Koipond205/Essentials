using UnityEngine;

public class GroundPoundAbility : MonoBehaviour
{
    [Header("Ground Pound")]
    public float groundPoundForce = 20f;
    public float groundPoundRadius = 5f;
    public LayerMask groundLayer;

    // Raycast parameters
    //public Transform groundCheck; // An empty GameObject at the player's feet
    //public float groundCheckDistance = 0.5f; // How far down the raycast goes
    
    private Rigidbody2D rb;

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
            // This is a basic example of checking for ground impact.
            // You can use a Coroutine for a more controlled ground pound effect.
            // StartCoroutine(GroundPoundCoroutine());
        }
    }

    bool IsGrounded()
    {
        // Visualize the raycast in the Scene view for debugging
        //Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, Color.red);

        // Perform a raycast from the 'groundCheck' position downwards
        //RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        
        // Return true if the raycast hits something on the ground layer
        //return hit.collider != null;

        // Implement your ground check logic here.
        // For example, using a raycast or an overlap circle.
        return Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);
    }
}
