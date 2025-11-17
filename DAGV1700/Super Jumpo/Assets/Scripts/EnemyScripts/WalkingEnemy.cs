using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 3f;
    public float attackRange = 1f;
    public float chaseRange = 5f;
    public float attackDelay = 1f;
    private float lastAttackTime;

    public int damage = 1;

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {

        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

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
            Debug.Log("Enemy attacks!");
            lastAttackTime = Time.time;
        }
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}