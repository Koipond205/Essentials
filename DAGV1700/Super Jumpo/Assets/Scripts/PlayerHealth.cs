using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private Animator animator;

    public HealthUI healthUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WalkingEnemy enemy = collision.GetComponent<WalkingEnemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);
        animator.SetTrigger("Hurt");
        SoundEffectManager.Instance.PlaySound("YanyaSFX3");

        if(currentHealth <= 0)
        {
            //player dies. Game Over Script here --------
        }
    }
}
