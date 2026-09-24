using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Patrol")]
    public Transform pointA;
    public Transform pointB;

    public float reachDistance = 0.2f;

    [HideInInspector]
    public bool MoveToPointA;

    [Header("Player")]
    [SerializeField] private Transform player;

    public Transform Player => player;

    public PlayerJump PlayerJump { get; private set; }

    [Header("Attack Timing")]
    public float attackCooldown = 1.2f;

    [Header("Height Check")]
    [SerializeField] private float maxHeightDifference = 1f;

    public EnemyMovement EnemyMovement { get; private set; }
    public EnemyAttack EnemyAttack { get; private set; }
    public Animator Animator { get; private set; }
    public EnemyHitFlash EnemyHitFlash { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }

    public EnemyStateMachine StateMachine { get; private set; }

    private Rigidbody rb;


    private void Awake()
    {
        EnemyMovement = GetComponent<EnemyMovement>();
        EnemyAttack = GetComponent<EnemyAttack>();
        Animator = GetComponent<Animator>();
        EnemyHitFlash = GetComponent<EnemyHitFlash>();
        EnemyHealth = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody>();

        if (player != null)
        {
            PlayerJump = player.GetComponent<PlayerJump>();
        }

        StateMachine = new EnemyStateMachine();
    }


    private void Start()
    {
        // Game start la mattum run aagum
        MoveToPointA = false;

        StateMachine.Initialize(
            new EnemyPatrolState(
                this,
                StateMachine));
    }


    private void Update()
    {
        StateMachine.Update();
    }


    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }


    // =========================================
    // RESET (EnemyManager call pannum)
    // =========================================

    public void ResetEnemy(Vector3 position, Quaternion rotation)
    {
        // Position reset
        transform.SetPositionAndRotation(position, rotation);

        // Velocity reset
        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Health + isDead reset
        if (EnemyHealth != null)
        {
            EnemyHealth.ResetHealth();
        }

        // Old knockback callback irundha remove
        if (EnemyMovement != null)
        {
            EnemyMovement.OnKnockbackFinished = null;
        }

        // Animator marubadi first state ku
        if (Animator != null)
        {
            Animator.Rebind();
            Animator.Update(0f);
        }

        // Patrol state la marubadi start
        MoveToPointA = false;

        StateMachine.Initialize(
            new EnemyPatrolState(
                this,
                StateMachine));
    }


    public void SetPlayer(Transform target)
    {
        player = target;

        if (player != null)
        {
            PlayerJump = player.GetComponent<PlayerJump>();
        }
    }


    public void SetPatrolPoints(
        Transform newPointA,
        Transform newPointB)
    {
        pointA = newPointA;
        pointB = newPointB;
    }


    public void SetAnimationSpeed(float speed)
    {
        if (Animator != null)
        {
            Animator.SetFloat("Speed", speed);
        }
    }


    public bool IsPlayerInChaseRange()
    {
        if (Player == null || EnemyMovement == null)
            return false;

        float xDistance =
            Mathf.Abs(transform.position.x - Player.position.x);

        float yDistance =
            Mathf.Abs(transform.position.y - Player.position.y);

        return xDistance <= EnemyMovement.ChaseRange &&
               yDistance <= maxHeightDifference;
    }


    public bool IsPlayerInAttackRange()
    {
        if (EnemyAttack == null || Player == null)
            return false;

        return EnemyAttack.PlayerInAttackRange(Player);
    }
}