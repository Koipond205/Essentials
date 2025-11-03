using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class MiningAbility : MonoBehaviour
{
    public Animator animator; // Reference to the Player's Animator component
    public float interactionDistance = 1.5f; // Max distance to break a block
    public Tilemap breakableTilemap; // Reference to the Tilemap containing breakable blocks
    public LayerMask breakableLayer; // Layer for breakable blocks (if not using Tilemap)

    private Vector2 facingDirection = Vector2.right; // Default direction

    void Update()
    {
        // Update facing direction based on movement if you have a movement script
        // For this example, we'll use input keys directly to set the direction for mining.
        HandleDirectionalInput();
        
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SwingPickaxe();
        //}
    }

    private void HandleDirectionalInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            // Prioritize the direction with greater magnitude for cardinal directions
            if (Mathf.Abs(h) > Mathf.Abs(v))
            {
                facingDirection = new Vector2(h, 0);
            }
            else
            {
                facingDirection = new Vector2(0, v);
            }
        }
    }

    private void SwingPickaxe()
    {
        // Trigger animation
        if (animator != null)
        {
            // Set animator parameters to control the animation direction
            animator.SetFloat("DirX", facingDirection.x);
            animator.SetFloat("DirY", facingDirection.y);
            animator.SetTrigger("Swing");
        }

        // Perform raycast to find breakable blocks
        Vector2 raycastOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, facingDirection, interactionDistance, breakableLayer);
        Debug.DrawRay(raycastOrigin, facingDirection * interactionDistance, Color.red, 1f); // Visualize the raycast

        if (hit.collider != null)
        {
            if (breakableTilemap != null)
            {
                // Handle Tilemap interaction
                Vector3 hitPosition = hit.point;
                // Need to get the cell position, adjusting slightly by the hit normal to be inside the block
                Vector3Int cellPosition = breakableTilemap.WorldToCell(hitPosition + (Vector3)hit.normal * 0.1f); 
                
                // Check if there is a tile at that position
                if (breakableTilemap.GetTile(cellPosition) != null)
                {
                   StartCoroutine(BreakTileAfterDelay(cellPosition, 0.2f)); // Add a small delay to sync with animation
                }
            }
            else
            {
                // Handle non-tilemap (Game Object) interaction
                BreakableBlock block = hit.collider.GetComponent<BreakableBlock>();
                if (block != null)
                {
                   StartCoroutine(BreakObjectAfterDelay(block, 0.2f)); // Add small delay
                   
                }
            }
        }
    }

    IEnumerator BreakTileAfterDelay(Vector3Int cellPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        breakableTilemap.SetTile(cellPosition, null);
    }

    IEnumerator BreakObjectAfterDelay(BreakableBlock block, float delay)
    {
        yield return new WaitForSeconds(delay);
        block.Break();
    }
}
