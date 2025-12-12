using UnityEngine;
using UnityEngine.Events;

public class KeyScript : MonoBehaviour
{
    public UnityEvent onKeyGrab; // Event to trigger when hit

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                onKeyGrab.Invoke();
                Debug.LogError("Unlock the door");
            }
        }
    }
}
