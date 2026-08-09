using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isDead;

    private Enemy enemy;

    // Camera Shake reference
    private CameraShake cameraShake;

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;


    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        // Find CameraShake in the scene
        cameraShake = FindFirstObjectByType<CameraShake>();
    }


    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(
        int damage,
        Vector3 attackerPosition)
    {
        // Ignore damage if enemy is already dead
        if (isDead)
            return;


        // =========================================
        // DAMAGE
        // =========================================

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth);


        Debug.Log(
            "Enemy Health : " +
            currentHealth);


        // =========================================
        // HIT FLASH
        // =========================================

        // Play additional manual white flash
        // whenever the enemy gets hit.
        if (enemy.EnemyHitFlash != null)
        {
            enemy.EnemyHitFlash.PlayHitFlash();
        }


        // =========================================
        // CAMERA SHAKE
        // =========================================

        // Small camera shake whenever
        // the enemy gets hit.
        if (cameraShake != null)
        {
            cameraShake.EnemyHit();
        }


        // =========================================
        // LAST HIT
        // =========================================

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            isDead = true;


            // =========================================
            // LAST HIT KNOCKBACK
            // =========================================

            // Knockback happens only on
            // the final hit.
            enemy.EnemyMovement.ApplyKnockback(
                attackerPosition);


            // =========================================
            // DEATH AFTER KNOCKBACK
            // =========================================

            // Wait until knockback is completely
            // finished before playing Death animation.
            enemy.EnemyMovement.OnKnockbackFinished =
                () =>
                {
                    enemy.StateMachine.ChangeState(
                        new EnemyDeathState(
                            enemy,
                            enemy.StateMachine));
                };


            return;
        }


        // =========================================
        // NORMAL HIT
        // =========================================

        // Normal hits do NOT apply knockback.
        //
        // Only:
        // 1. Hit Flash
        // 2. Camera Shake
        // 3. Hurt Animation
        // will happen.

        enemy.StateMachine.ChangeState(
            new EnemyHurtState(
                enemy,
                enemy.StateMachine));
    }


    // =========================================
    // HEAL
    // =========================================

    public void Heal(int amount)
    {
        // Cannot heal after death
        if (isDead)
            return;


        currentHealth += amount;


        // Don't exceed maximum health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}