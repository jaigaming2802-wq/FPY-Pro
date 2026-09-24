
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBar;

    [Header("Block")]
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

        if (healthBar != null)
        {
            healthBar.fillAmount = 1f;
        }
    }

    public void TakeDamage(
        int damage,
        Vector3 attackerPosition)
    {
        if (IsDead)
            return;

        if (IsParrying)
        {
            Debug.Log("Perfect Parry!");
            return;
        }

        if (IsBlocking)
        {
            int chipDamage =
                Mathf.RoundToInt(
                    damage * blockDamageMultiplier);

            currentHealth -= chipDamage;

            currentHealth =
                Mathf.Clamp(
                    currentHealth,
                    0,
                    maxHealth);

            UpdateHealthBar();

            Debug.Log(
                "Blocked! Chip Damage : " +
                chipDamage);

            Debug.Log(
                "Player Health : " +
                currentHealth);

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

        currentHealth -= damage;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0,
                maxHealth);

        UpdateHealthBar();

        Debug.Log(
            "Player Health : " +
            currentHealth);

        if (currentHealth <= 0)
        {
            IsDead = true;

            player.StateMachine.ChangeState(
                new DeathState(
                    player,
                    player.StateMachine));

            return;
        }

        player.StateMachine.ChangeState(
            new HurtState(
                player,
                player.StateMachine));
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        IsDead = false;

        IsBlocking = false;
        IsParrying = false;

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.fillAmount =
            (float)currentHealth / maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}

