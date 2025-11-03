using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MapPlayer : MonoBehaviour
{
    public MapNode currentNode;
    public float moveSpeed = 5f;
    private bool isMoving = false;
    public Animator transition;
    public float transitionTime = 1f;

    void Update()
    {
        if (isMoving) return;

        // Directional movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
            TryMove(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            TryMove(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            TryMove(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            TryMove(Vector2.right);

        // Enter level
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(currentNode.levelSceneName))
        {
            EnterLevel();
        }
    }

    //TransitionAnimator
    public void EnterLevel()
    {
        StartCoroutine(LoadMap());
    }

    IEnumerator LoadMap()
    {
        //Play animation
        transition.SetTrigger("Start");
        //Wait
        yield return new WaitForSeconds(transitionTime);
        //Load scene
        SceneManager.LoadScene(currentNode.levelSceneName);
    }

    void TryMove(Vector2 direction)
    {
        // Find the closest node in that direction
        MapNode nextNode = null;
        float bestDot = 0.5f; // Only move if node is roughly in that direction

        foreach (var node in currentNode.connectedNodes)
        {
            Vector2 toNode = (node.transform.position - transform.position).normalized;
            float dot = Vector2.Dot(toNode, direction);

            if (dot > bestDot)
            {
                bestDot = dot;
                nextNode = node;
            }
        }

        if (nextNode != null)
            StartCoroutine(MoveToNode(nextNode));
    }

    IEnumerator MoveToNode(MapNode target)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = target.transform.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        currentNode = target;
        isMoving = false;
    }
}
