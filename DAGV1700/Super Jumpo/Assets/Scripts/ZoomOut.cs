using UnityEngine;
using Cinemachine;

public class ZoomOut : MonoBehaviour
{
    [Header("Assign Box Colliders")]
    public CinemachineVirtualCamera virtualCam; 
    public BoxCollider2D zoomBox, player;
    public float targetOrthoSize = 10f;
    public float zoomSpeed = 2f;

    private float currentOrthoSize;

    [Header("Status")]
    public bool areTouching;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentOrthoSize = virtualCam.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {

        // Check if they overlap/touch
        areTouching = zoomBox.IsTouching(player);

        if (areTouching)
        {
            currentOrthoSize = Mathf.Lerp(currentOrthoSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
            virtualCam.m_Lens.OrthographicSize = currentOrthoSize;
        }
        else
        {
            
        }
    }
}
