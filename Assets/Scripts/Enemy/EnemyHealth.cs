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

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth);

        Debug.Log("Enemy Health : " + currentHealth);


        // =========================================
        // HIT FLASH
        // =========================================

        // Play the additional manual white flash
        // whenever the enemy gets hit.
        //
        // This is separate from the existing
        // white flash inside the Hurt animation.
        // HIT FLASH
        if (enemy.EnemyHitFlash != null)
        {
            enemy.EnemyHitFlash.PlayHitFlash();
        }

        // CAMERA SHAKE
        if (enemy.CameraShake != null)
        {
            enemy.CameraShake.EnemyHit();
        }


        // =========================
        // LAST HIT
        // =========================

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;


            // =========================================
            // LAST HIT KNOCKBACK
            // =========================================

            // Knockback happens only on the
            // final hit that kills the enemy.
            enemy.EnemyMovement.ApplyKnockback(
                attackerPosition);


            // =========================================
            // DEATH AFTER KNOCKBACK
            // =========================================

            // Wait until knockback is completely finished.
            // Then play Death animation.
            enemy.EnemyMovement.OnKnockbackFinished = () =>
            {
                enemy.StateMachine.ChangeState(
                    new EnemyDeathState(
                        enemy,
                        enemy.StateMachine));
            };


            return;
        }


        // =========================
        // NORMAL HIT
        // =========================

        // IMPORTANT:
        // Normal hits DO NOT apply knockback.
        //
        // Only the white hit flash + Hurt animation
        // will happen.

        enemy.StateMachine.ChangeState(
            new EnemyHurtState(
                enemy,
                enemy.StateMachine));
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