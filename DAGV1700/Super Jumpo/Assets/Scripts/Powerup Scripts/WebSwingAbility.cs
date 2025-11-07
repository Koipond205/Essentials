using UnityEngine;

public class WebSwingAbility : MonoBehaviour
{      
    [Header("Web Swing")]
    public float maxDistance = 30f;
    public float swingSpeed = 10f;
    public LayerMask webbableLayer;
    public LineRenderer lineRenderer;
    public bool isSwinging;

    [Header("Joint Settings")]
    public float sjDistanceFromPointMult = 0.95f;
    private Joint2D joint;
    private Vector2 grapplePoint;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;  //IMPORTANT (I changed all the transform.position to rb.transform.position and I dont know if thats right)
    bool facingRight;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (isSwinging)
        {
            DrawRope();
        }
    }

    private void DrawRope()
    {

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, rb.transform.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    public void ShootWeb()
    {
        lineRenderer.enabled = true;
        SoundEffectManager.Instance.PlaySound("LandingSFX1");

        facingRight = playerMovement.isFacingRight;
        Vector2 direction = playerMovement.isFacingRight ? Vector2.right : Vector2.left;
        
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, direction, maxDistance, webbableLayer);
        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isSwinging = true;

            DistanceJoint2D distanceJoint = gameObject.AddComponent<DistanceJoint2D>();
            distanceJoint.autoConfigureConnectedAnchor = false;
            distanceJoint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector2.Distance(rb.position, grapplePoint);

            distanceJoint.distance = distanceFromPoint * sjDistanceFromPointMult;
            distanceJoint.enableCollision = true; // ensures player collides with environment while swinging

            joint = distanceJoint;
            
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, rb.transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
        else
        {
            // If the raycast misses
            lineRenderer.enabled = false;
            isSwinging = false;
        }
    }

    public void StopWeb()
    {
        // Stop swinging and destroy the joint
        isSwinging = false;
        if (joint != null)
        {
            Destroy(joint);
        }
        lineRenderer.enabled = false;
    }
}
