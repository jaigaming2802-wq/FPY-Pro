using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float attackDetectionRange = 1.5f;

    [Header("Height Check")]
    [SerializeField] private float maxHeightDifference = 1f;

    [Header("Hitbox")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.6f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Damage")]
    [SerializeField] private int attackDamage = 20;

    public bool PlayerInAttackRange(Transform player)
    {
        if (player == null)
            return false;

        float xDistance = Mathf.Abs(transform.position.x - player.position.x);
        float yDistance = Mathf.Abs(transform.position.y - player.position.y);

        return xDistance <= attackDetectionRange &&
               yDistance <= maxHeightDifference;
    }

    // Animation Event
    public void Attack()
    {
        Collider[] players = Physics.OverlapSphere(
            attackPoint.position,
            attackRadius,
            playerLayer);

        foreach (Collider player in players)
        {
            float yDistance = Mathf.Abs(
                transform.position.y -
                player.transform.position.y);

            if (yDistance > maxHeightDifference)
                continue;

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(
                    attackDamage,
                    transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDetectionRange);

        if (attackPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}