using UnityEngine;
using UnityEngine.Events;

public class BlockScript : MonoBehaviour
{
    public UnityEvent onBlockHit; // Event to trigger when hit
    private Animator animator;
    private bool canBeHit = true;

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        if (animator == null)
        {
            Debug.LogError("Animator component not found on QuestionBlock!");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check for player tag and collision from below
        if (other.collider.CompareTag("Player") && canBeHit)
        {
            if (other.contacts.Length > 0 && other.contacts[0].normal.y > 0.5f)
            {
                TriggerAnimation();
                onBlockHit.Invoke(); // Trigger the custom event in the Inspector
            }
        }
    }

    public void TriggerAnimation()
    {
        if (canBeHit && animator != null)
        {
            animator.SetTrigger("Hit"); // Set the "Hit" trigger in the Animator
            SoundEffectManager.Instance.PlaySound("SquishSFX1");
            // If you want the block to only be hit once, set canBeHit to false here.
            // canBeHit = false; 
        }
    }
}
