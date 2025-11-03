using UnityEngine;
using System.Collections.Generic;

public class MapNode : MonoBehaviour
{
    [Header("Connected Nodes")]
    public List<MapNode> connectedNodes; // Adjacent nodes you can move to

    [Header("Level Info")]
    public string levelSceneName; // Optional: name of the scene to load

    [HideInInspector]
    public bool isActive = false;

    private void OnDrawGizmos()
    {
        // Draw connections in the editor for clarity
        Gizmos.color = Color.yellow;
        foreach (var node in connectedNodes)
        {
            if (node != null)
                Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
