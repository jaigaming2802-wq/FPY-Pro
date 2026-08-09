using System;
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


    // Called when the knockback is completely finished.
    public Action OnKnockbackFinished;


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
            attackPointOffset =
                attackPoint.localPosition;
        }
    }


    private void FixedUpdate()
    {
        HandleKnockback();
    }


    // =========================================
    // KNOCKBACK
    // =========================================

    private void HandleKnockback()
    {
        if (!isKnockedBack)
            return;


        knockbackTimer -=
            Time.fixedDeltaTime;


        if (knockbackTimer <= 0f)
        {
            isKnockedBack = false;


            // Stop horizontal knockback movement.
            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f);


            // Tell EnemyHealth that
            // knockback has finished.
            OnKnockbackFinished?.Invoke();


            // Clear callback so it doesn't
            // accidentally get called again.
            OnKnockbackFinished = null;
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


    // =========================================
    // NORMAL MOVEMENT
    // =========================================

    public void Move(Vector3 targetPosition)
    {
        if (isKnockedBack)
            return;


        float distance =
            targetPosition.x -
            transform.position.x;


        if (Mathf.Abs(distance) < 0.1f)
        {
            rb.linearVelocity =
                new Vector3(
                    0f,
                    rb.linearVelocity.y,
                    0f);

            return;
        }


        float direction =
            Mathf.Sign(distance);


        // Player air-la irundha
        // enemy facing change panna koodadhu.
        if (enemy != null &&
            enemy.PlayerJump != null &&
            !enemy.PlayerJump.IsGrounded)
        {
            direction = facingDirection;
        }


        rb.linearVelocity =
            new Vector3(
                direction * currentSpeed,
                rb.linearVelocity.y,
                0f);


        // Flip only when player is grounded.
        if (enemy == null ||
            enemy.PlayerJump == null ||
            enemy.PlayerJump.IsGrounded)
        {
            Flip(direction);
        }
    }


    // =========================================
    // STOP
    // =========================================

    public void Stop()
    {
        if (isKnockedBack)
            return;


        rb.linearVelocity =
            new Vector3(
                0f,
                rb.linearVelocity.y,
                0f);
    }


    // =========================================
    // FLIP
    // =========================================

    private void Flip(float direction)
    {
        if (direction == 0)
            return;


        facingDirection =
            (int)Mathf.Sign(direction);


        if (sprite != null)
        {
            sprite.flipX =
                direction < 0;
        }


        if (attackPoint != null)
        {
            float x =
                Mathf.Abs(
                    attackPointOffset.x);


            attackPoint.localPosition =
                new Vector3(
                    direction < 0 ? -x : x,
                    attackPointOffset.y,
                    attackPointOffset.z);
        }
    }


    // =========================================
    // FACE PLAYER
    // =========================================

    public void FaceTarget(Vector3 targetPosition)
    {
        if (enemy != null &&
            enemy.PlayerJump != null &&
            !enemy.PlayerJump.IsGrounded)
        {
            return;
        }


        float direction =
            Mathf.Sign(
                targetPosition.x -
                transform.position.x);


        if (direction != 0)
        {
            Flip(direction);
        }
    }


    // =========================================
    // RANGE
    // =========================================

    public bool IsPlayerInRange(
        Transform player)
    {
        return Mathf.Abs(
            transform.position.x -
            player.position.x)
            <= chaseRange;
    }


    // =========================================
    // APPLY KNOCKBACK
    // =========================================

    public void ApplyKnockback(
        Vector3 attackerPosition)
    {
        isKnockedBack = true;

        knockbackTimer =
            knockbackDuration;


        // Calculate direction away
        // from the attacker.
        float direction =
            Mathf.Sign(
                transform.position.x -
                attackerPosition.x);


        // Apply knockback.
        rb.linearVelocity =
            new Vector3(
                direction * knockbackForce,
                knockbackHeight,
                0f);
    }


    // =========================================
    // DEBUG
    // =========================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            chaseRange);
    }
}