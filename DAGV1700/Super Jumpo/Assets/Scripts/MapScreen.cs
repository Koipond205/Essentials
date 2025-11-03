using UnityEngine;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{
    public GameObject mapPanel;
    public Image playerMarker;
    public Transform playerTransform;
    public Camera mapCamera;

    private bool isMapOpen = false;

    void Start()
    {
        // Ensure the map is closed at the start of the game
        mapPanel.SetActive(false);
    }

    void Update()
    {
        // Toggle the map screen on 'M' key press
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
        
        if (isMapOpen)
        {
            UpdatePlayerMarkerPosition();
        }
    }

    public void ToggleMap()
    {
        isMapOpen = !isMapOpen;
        mapPanel.SetActive(isMapOpen);
        
        // Optionally, pause the game while the map is open
        Time.timeScale = isMapOpen ? 0 : 1;
    }

    private void UpdatePlayerMarkerPosition()
    {
        // Convert the player's world position to a viewport point using the map camera
        Vector3 viewportPoint = mapCamera.WorldToViewportPoint(playerTransform.position);
        
        // Convert the viewport point to a local position within the map panel's rect transform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mapPanel.GetComponent<RectTransform>(), 
            new Vector2(viewportPoint.x * Screen.width, viewportPoint.y * Screen.height), 
            null, 
            out Vector2 localPoint
        );
        
        // Apply the local position to the player marker
        playerMarker.rectTransform.localPosition = localPoint;
    }
}