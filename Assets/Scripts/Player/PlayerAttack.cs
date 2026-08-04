using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackRadius = 0.6f;

    [Header("Damage")]
    [SerializeField] private int lightAttackDamage = 20;
    [SerializeField] private int heavyAttackDamage = 35;

    // Animation Event - Attack 1
    public void LightAttack()
    {
        DealDamage(lightAttackDamage);
    }

    // Animation Event - Attack 2
    public void HeavyAttack()
    {
        DealDamage(heavyAttackDamage);
    }

    private void DealDamage(int damage)
    {
        Collider[] enemies = Physics.OverlapSphere(
            attackPoint.position,
            attackRadius,
            enemyLayer);

        foreach (Collider enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(
                    damage,
                    transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRadius);
    }
}