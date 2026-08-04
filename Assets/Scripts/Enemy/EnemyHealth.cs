using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isDead;

    private Enemy enemy;

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log("Enemy Health : " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;

            enemy.StateMachine.ChangeState(
                new EnemyDeathState(enemy, enemy.StateMachine));

            return;
        }

        // Apply Knockback
        enemy.EnemyMovement.ApplyKnockback(attackerPosition);

        // Hurt
        enemy.StateMachine.ChangeState(
            new EnemyHurtState(enemy, enemy.StateMachine));
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}