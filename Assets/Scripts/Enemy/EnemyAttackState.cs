using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float cooldownTimer;
    private float windupTimer;

    private bool attackStarted;

    private const float windupTime = 0.25f;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        enemy.EnemyMovement.Stop();

        // Face player before attacking
        enemy.EnemyMovement.FaceTarget(enemy.Player.position);

        enemy.SetAnimationSpeed(0f);

        cooldownTimer = enemy.attackCooldown;
        windupTimer = windupTime;

        attackStarted = false;

        enemy.Animator.ResetTrigger("Attack");
        enemy.Animator.ResetTrigger("Recovered");
    }

    public override void Update()
    {
        // Player escaped attack range
        if (!enemy.IsPlayerInAttackRange())
        {
            enemy.Animator.SetTrigger("Recovered");

            if (enemy.IsPlayerInChaseRange())
            {
                stateMachine.ChangeState(
                    new EnemyChaseState(enemy, stateMachine));
            }
            else
            {
                stateMachine.ChangeState(
                    new EnemyPatrolState(enemy, stateMachine));
            }

            return;
        }

        // Always face player
        enemy.EnemyMovement.FaceTarget(enemy.Player.position);

        // Wind-up
        if (!attackStarted)
        {
            windupTimer -= Time.deltaTime;

            if (windupTimer <= 0f)
            {
                attackStarted = true;

                enemy.SetAnimationSpeed(1f);

                enemy.Animator.SetTrigger("Attack");
            }

            return;
        }

        AnimatorStateInfo state =
            enemy.Animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Attack") &&
            state.normalizedTime >= 1f)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                attackStarted = false;

                windupTimer = windupTime;

                cooldownTimer = enemy.attackCooldown;

                enemy.Animator.ResetTrigger("Attack");
            }
        }
    }

    public override void FixedUpdate()
    {
        enemy.EnemyMovement.Stop();
    }

    public override void Exit()
    {
        enemy.Animator.ResetTrigger("Attack");
    }
}
