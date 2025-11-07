using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    // Public variables to configure enemy behavior
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float hoverSpeed = 1f;
    public Transform[] patrolPoints;

    [Header("Detection")]
    public float detectionDistance = 5f;
    public float losePlayerDistance = 10f;
    public float hoverRange = 1f;

    // Private variables for internal logic
    private Transform playerTransform;
    private int currentPatrolIndex = 0;
    private Rigidbody2D rb;

    // State machine variables
    private enum EnemyState { Patrolling, Chasing, Hovering }
    private EnemyState currentState = EnemyState.Patrolling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ensure gravity is off for flying movement
        rb.gravityScale = 0;
        // Find the player GameObject based on its tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // State machine logic
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionDistance)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Chasing:
                // Transition to hovering if close enough
                if (distanceToPlayer <= hoverRange)
                {
                    currentState = EnemyState.Hovering;
                }
                // Transition back to patrolling if the player gets away
                else if (distanceToPlayer > losePlayerDistance)
                {
                    currentState = EnemyState.Patrolling;
                }
                else
                {
                    ChasePlayer();
                }
                break;

            case EnemyState.Hovering:
                HoverNearPlayer();
                // Start chasing again if the player moves too far
                if (distanceToPlayer > hoverRange)
                {
                    currentState = EnemyState.Chasing;
                }
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime));

        // Check if the enemy has reached the patrol point
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Move to the next point in the array
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        // Move directly towards the player's position
        rb.MovePosition(Vector2.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime));
    }

    void HoverNearPlayer()
    {
        // Add a gentle, random hovering motion
        Vector2 randomOffset = Random.insideUnitCircle * hoverSpeed * Time.deltaTime;
        Vector2 targetPosition = (Vector2)playerTransform.position + randomOffset;
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, hoverSpeed * Time.deltaTime));
    }
}