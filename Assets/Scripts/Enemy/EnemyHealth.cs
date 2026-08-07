using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Death Effect")]
    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private float effectScale = 1f;
    [SerializeField] private float effectLifetime = 3f;

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

            // Spawn Death Effect
            if (deathEffectPrefab != null)
            {
                GameObject effect = Instantiate(
                    deathEffectPrefab,
                    transform.position,
                    Quaternion.identity);

                // Effect Size
                effect.transform.localScale = Vector3.one * effectScale;

                // Destroy Effect after given time
                Destroy(effect, effectLifetime);
            }

            // Change to Death State
            enemy.StateMachine.ChangeState(
                new EnemyDeathState(enemy, enemy.StateMachine));

            return;
        }

        // Apply Knockback
        enemy.EnemyMovement.ApplyKnockback(attackerPosition);

        // Hurt State
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