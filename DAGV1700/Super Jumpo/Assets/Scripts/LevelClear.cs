using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour
{
    public UnityEvent onFlagHit; // Event to trigger when hit
    private Animator animator;
    public string nextSceneName; 

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Flag");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger zone is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the flag! Loading next level: " + nextSceneName);
            animator.SetTrigger("Hit"); // Set the "Hit" trigger in the Animator
            SoundEffectManager.Instance.PlaySound("LevelClearSFX1");
            EndLevel();
        }
    }

    void EndLevel()
    {
        // Load the specified scene name
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("nextSceneName is not set in the Inspector on " + gameObject.name + "!");
        }
    }
}
