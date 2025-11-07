using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupAbility : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 1.5f;
    public Transform holdPoint; // Empty transform above player’s head
    public Transform dropPoint;
    public LayerMask pickupLayer;

    [Header("Throw Settings")]
    public float consistentThrowForce = 10f;
    float throwAngle = 30f * Mathf.Deg2Rad;

    private Rigidbody2D heldObject;
    private Rigidbody2D rb;
    private bool isHolding;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

     private void Update()
    {
        // If holding an object, keep it positioned over the player
        if (isHolding && heldObject != null)
        {
            heldObject.transform.position = Vector2.Lerp(
            heldObject.transform.position,
            holdPoint.position,
            Time.deltaTime * 15f
            );
        }
    }

    public void Activate()
    {
        if (!isHolding)
        {
            TryPickup();
        }
        else
        {
            DropObject();
            // If already holding something, start charging for throw
            //ThrowObject(consistentThrowForce);
        }
    }

    private void TryPickup()
    {
        // Detect nearby pickup objects
        Collider2D hit = Physics2D.OverlapCircle(transform.position, pickupRange, pickupLayer);

        if (hit != null)
        {
            Rigidbody2D objectRb = hit.attachedRigidbody;
            if (objectRb != null)
            {
                PickUpObject(objectRb);
            }
        }
    }

    private void PickUpObject(Rigidbody2D objectRb)
    {
        SoundEffectManager.Instance.PlaySound("YanyaSFX1");
        heldObject = objectRb;
        heldObject.bodyType = RigidbodyType2D.Kinematic;
        heldObject.transform.position = holdPoint.position;
        heldObject.transform.SetParent(holdPoint);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), heldObject.GetComponent<Collider2D>(), true);
        isHolding = true;
    }

    private void DropObject()
    {
        if (heldObject == null) return;

        SoundEffectManager.Instance.PlaySound("YanyaSFX2");
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), heldObject.GetComponent<Collider2D>(), false);

        heldObject.bodyType = RigidbodyType2D.Dynamic;
        heldObject.transform.parent = null;
        // Determine direction based on facing
        float direction = playerMovement.isFacingRight ? 1f : -1f;

        // Compute world drop position relative to player
        Vector2 dropPos = (Vector2)transform.position + new Vector2(direction * 0.75f, 0.0f);

        heldObject.transform.position = dropPos;
        heldObject.linearVelocity = Vector2.zero;
        heldObject = null;
        isHolding = false;
    }

    private void ThrowObject(float force)
    {
        if (heldObject == null) return;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), heldObject.GetComponent<Collider2D>(), false);

        heldObject.bodyType = RigidbodyType2D.Dynamic;
        heldObject.transform.parent = null;

        Vector2 throwDir = new Vector2(
            playerMovement.isFacingRight ? Mathf.Cos(throwAngle) : -Mathf.Cos(throwAngle),
            Mathf.Sin(throwAngle)
        );
        heldObject.AddForce(throwDir * force, ForceMode2D.Impulse);

        heldObject = null;
        isHolding = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}