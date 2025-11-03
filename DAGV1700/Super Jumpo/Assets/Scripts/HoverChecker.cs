using UnityEngine;

public class HoverChecker : MonoBehaviour
{
    [SerializeField]
    GameObject button;
    public float scaleSizeX = 1.1f;
    public float scaleSizeY = 1.1f;
    public float returnSizeX = 1f;
    public float returnSizeY = 1f;
    public float scaleSpeed = 0.1f;

    public void HoverOn()
    {
        LeanTween.scale(button, new Vector3(scaleSizeX,scaleSizeY,0f), scaleSpeed);
    }

    public void HoverOff()
    {
        LeanTween.scale(button, new Vector3(returnSizeX,returnSizeY,0f), scaleSpeed);
    }
}
