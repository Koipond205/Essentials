using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    public Transform player; // Assign the player's Transform in the Inspector
    public float chaseSpeed = 3f;
    public float attackRange = 1f;
    public float chaseRange = 5f;
    public float attackDelay = 1f; // Delay between attacks
    private float lastAttackTime;

    public int damage = 1;

    void Update()
    {
        if (player == null) return; // Ensure player is assigned

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // In attack range
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // In chase range, but not attack range
            ChasePlayer();
        }
        else
        {
            // Player is out of range, enemy could patrol or remain idle
            // Implement patrolling or idle behavior here if needed
        }
    }

    void ChasePlayer()
    {
        // Calculate direction to player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        // Optional: Flip enemy sprite to face the player
        if (direction.x > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    void AttackPlayer()
    {
        if (Time.time > lastAttackTime + attackDelay)
        {
            // Perform attack action (e.g., play attack animation, deal damage)
            Debug.Log("Enemy attacks!");
            lastAttackTime = Time.time;
            // Add your attack logic here, e.g., calling a method on the player to take damage
        }
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}