using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Block")]
    // Damage percentage received while blocking.
    // Example: 0.2f means 20% chip damage.
    [SerializeField] private float blockDamageMultiplier = 0.2f;

    private int currentHealth;

    public bool IsDead { get; private set; }

    public bool IsBlocking { get; set; }

    public bool IsParrying { get; set; }

    private PlayerMovement player;


    private void Awake()
    {
        currentHealth = maxHealth;

        player = GetComponent<PlayerMovement>();
    }


    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        // Ignore damage if the player is already dead.
        if (IsDead)
            return;


        // =========================================
        // PERFECT PARRY
        // =========================================

        // Perfect parry completely prevents damage.
        if (IsParrying)
        {
            Debug.Log("Perfect Parry!");
            return;
        }


        // =========================================
        // NORMAL BLOCK
        // =========================================

        // While blocking, player receives only
        // a percentage of the original damage.
        if (IsBlocking)
        {
            int chipDamage =
                Mathf.RoundToInt(
                    damage * blockDamageMultiplier);


            currentHealth -= chipDamage;


            currentHealth = Mathf.Clamp(
                currentHealth,
                0,
                maxHealth);


            Debug.Log(
                "Blocked! Chip Damage : " +
                chipDamage);

            Debug.Log(
                "Player Health : " +
                currentHealth);


            // Check whether chip damage killed the player.
            if (currentHealth <= 0)
            {
                IsDead = true;


                // Change to Death State when
                // the player dies while blocking.
                player.StateMachine.ChangeState(
                    new DeathState(
                        player,
                        player.StateMachine));
            }


            // Knockback is not applied while blocking.
            return;
        }


        // =========================================
        // NORMAL DAMAGE
        // =========================================

        currentHealth -= damage;


        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth);


        Debug.Log(
            "Player Health : " +
            currentHealth);


        // =========================================
        // DEATH
        // =========================================

        // Check if the damage killed the player.
        if (currentHealth <= 0)
        {
            IsDead = true;


            // Change to Death State.
            player.StateMachine.ChangeState(
                new DeathState(
                    player,
                    player.StateMachine));


            return;
        }


        // =========================================
        // KNOCKBACK
        // =========================================

        // Knockback is currently disabled.
        // The code is kept for future implementation.
        //
        // player.ApplyKnockback(attackerPosition);


        // =========================================
        // HURT STATE
        // =========================================

        // Play Hurt State after taking damage.
        player.StateMachine.ChangeState(
            new HurtState(
                player,
                player.StateMachine));
    }


    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}