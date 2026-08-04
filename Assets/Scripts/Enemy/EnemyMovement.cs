using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;

    [Header("Detection")]
    [SerializeField] private float chaseRange = 6f;

    [Header("Attack Point")]
    [SerializeField] private Transform attackPoint;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 3f;
    [SerializeField] private float knockbackHeight = 1.5f;
    [SerializeField] private float knockbackDuration = 0.15f;

    private bool isKnockedBack;
    private float knockbackTimer;

    public float ChaseRange => chaseRange;

    private Rigidbody rb;
    private SpriteRenderer sprite;
    private Enemy enemy;

    private float currentSpeed;

    private Vector3 attackPointOffset;

    private int facingDirection = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();

        currentSpeed = patrolSpeed;

        if (attackPoint != null)
        {
            attackPointOffset = attackPoint.localPosition;
        }
    }

    private void FixedUpdate()
    {
        if (!isKnockedBack)
            return;

        knockbackTimer -= Time.fixedDeltaTime;

        if (knockbackTimer <= 0f)
        {
            isKnockedBack = false;
        }
    }

    public void SetPatrolSpeed()
    {
        currentSpeed = patrolSpeed;
    }

    public void SetChaseSpeed()
    {
        currentSpeed = chaseSpeed;
    }

    public void Move(Vector3 targetPosition)
    {
        if (isKnockedBack)
            return;

        float distance =
            targetPosition.x - transform.position.x;

        if (Mathf.Abs(distance) < 0.1f)
        {
            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f);

            return;
        }

        float direction = Mathf.Sign(distance);

        // Player air-la irundha reverse panna koodadhu
        if (enemy != null &&
            enemy.PlayerJump != null &&
            !enemy.PlayerJump.IsGrounded)
        {
            direction = facingDirection;
        }

        rb.linearVelocity = new Vector3(
            direction * currentSpeed,
            rb.linearVelocity.y,
            0f);

        if (enemy == null ||
            enemy.PlayerJump == null ||
            enemy.PlayerJump.IsGrounded)
        {
            Flip(direction);
        }
    }
    public void Stop()
    {
        if (isKnockedBack)
            return;

        rb.linearVelocity = new Vector3(
            0f,
            rb.linearVelocity.y,
            0f);
    }

    private void Flip(float direction)
    {
        if (direction == 0)
            return;

        facingDirection = (int)Mathf.Sign(direction);

        sprite.flipX = direction < 0;

        if (attackPoint != null)
        {
            float x = Mathf.Abs(attackPointOffset.x);

            attackPoint.localPosition = new Vector3(
                direction < 0 ? -x : x,
                attackPointOffset.y,
                attackPointOffset.z);
        }
    }

    // Face player only when player is grounded
    public void FaceTarget(Vector3 targetPosition)
    {
        if (enemy != null &&
            enemy.PlayerJump != null &&
            !enemy.PlayerJump.IsGrounded)
        {
            return;
        }

        float direction =
            Mathf.Sign(targetPosition.x - transform.position.x);

        if (direction != 0)
        {
            Flip(direction);
        }
    }

    public bool IsPlayerInRange(Transform player)
    {
        return Mathf.Abs(
            transform.position.x - player.position.x)
            <= chaseRange;
    }

    public void ApplyKnockback(Vector3 attackerPosition)
    {
        isKnockedBack = true;

        knockbackTimer = knockbackDuration;

        float direction =
            Mathf.Sign(
                transform.position.x - attackerPosition.x);

        rb.linearVelocity = new Vector3(
            direction * knockbackForce,
            knockbackHeight,
            0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            chaseRange);
    }
}