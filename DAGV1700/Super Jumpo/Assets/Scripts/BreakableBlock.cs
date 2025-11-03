using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public void Break()
    {
        // You can add particle effects or sound here.
        Destroy(gameObject);
    }
}
