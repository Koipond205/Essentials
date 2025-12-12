using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect; // The speed the background moves relative to camera
    // Start is called before the first frame update
    void Start()
    {
        //only x will move
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate distance background moves based on cam movement
        float distance = cam.transform.position.x * parallaxEffect; // 0 = moves with cam || 1 = won't move || 0.5 = half
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        // y and z wont change
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        
        if(movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
