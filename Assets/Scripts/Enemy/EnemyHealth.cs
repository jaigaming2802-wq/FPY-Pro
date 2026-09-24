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

        // Start ku munnadiye health set aaganum
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

        if (enemy.EnemyHitFlash != null)
        {
            enemy.EnemyHitFlash.PlayHitFlash();
        }


        // =========================================
        // CAMERA SHAKE
        // =========================================

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


            // Knockback happens only on the final hit.
            enemy.EnemyMovement.ApplyKnockback(
                attackerPosition);


            // Knockback mudinja apram Death state
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


    // =========================================
    // RESET (Enemy respawn aagum bodhu)
    // =========================================

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
}