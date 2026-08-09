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

    public EnemyStateMachine StateMachine { get; private set; }

    public EnemyHitFlash EnemyHitFlash { get; private set; }
    public CameraShake CameraShake { get; private set; }

    private void Awake()
    {
        EnemyMovement = GetComponent<EnemyMovement>();
        EnemyAttack = GetComponent<EnemyAttack>();
        Animator = GetComponent<Animator>();
        EnemyHitFlash = GetComponentInChildren<EnemyHitFlash>();

        if (player != null)
        {
            PlayerJump = player.GetComponent<PlayerJump>();
        }
        CameraShake = FindFirstObjectByType<CameraShake>();

        StateMachine = new EnemyStateMachine();
    }

    private void Start()
    {
        MoveToPointA = false;

        StateMachine.Initialize(
            new EnemyPatrolState(this, StateMachine));
    }

    private void Update()
    {
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
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

        float xDistance = Mathf.Abs(transform.position.x - Player.position.x);
        float yDistance = Mathf.Abs(transform.position.y - Player.position.y);

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