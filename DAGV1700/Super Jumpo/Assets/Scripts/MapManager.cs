using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapPlayer mapPlayer;

    public void UnlockNode(MapNode node)
    {
        node.isActive = true;
        // Maybe show some particle effect or change sprite
    }
}

