using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Block")]
    [SerializeField] private float blockDamageMultiplier = 0.2f; // 20% Chip Damage

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
        if (IsDead)
            return;


        // Perfect Parry
        if (IsParrying)
        {
            Debug.Log("Perfect Parry!");
            return;
        }


        // Normal Block (Chip Damage)
        if (IsBlocking)
        {
            int chipDamage = Mathf.RoundToInt(damage * blockDamageMultiplier);

            currentHealth -= chipDamage;

            currentHealth = Mathf.Clamp(
                currentHealth,
                0,
                maxHealth);

            Debug.Log("Blocked! Chip Damage : " + chipDamage);
            Debug.Log("Player Health : " + currentHealth);

            if (currentHealth <= 0)
            {
                IsDead = true;

                player.StateMachine.ChangeState(
                    new DeathState(
                        player,
                        player.StateMachine));
            }

            return;
        }


        // Normal Damage
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth);

        Debug.Log("Player Health : " + currentHealth);

        // Death
        if (currentHealth <= 0)
        {
            IsDead = true;

            player.StateMachine.ChangeState(
                new DeathState(
                    player,
                    player.StateMachine));

            return;
        }

        // Knockback
        player.ApplyKnockback(attackerPosition);

        // Hurt State
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