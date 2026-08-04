using UnityEngine;

public class EnemyHurtState : EnemyState
{
    private bool animationStarted;

    public EnemyHurtState(Enemy enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy Hurt Enter");

        // ❌ Stop() call pannadhe
        // enemy.EnemyMovement.Stop();

        enemy.SetAnimationSpeed(0f);

        animationStarted = false;

        enemy.Animator.ResetTrigger("Attack");
        enemy.Animator.ResetTrigger("Hurt");
        enemy.Animator.ResetTrigger("Recovered");

        enemy.Animator.SetTrigger("Hurt");
    }

    public override void Update()
    {
        AnimatorStateInfo state =
            enemy.Animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("hurt"))
        {
            animationStarted = true;
        }

        if (!animationStarted)
            return;

        if (state.normalizedTime >= 1f)
        {
            enemy.Animator.SetTrigger("Recovered");

            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(
                    new EnemyAttackState(enemy, stateMachine));
            }
            else if (enemy.IsPlayerInChaseRange())
            {
                stateMachine.ChangeState(
                    new EnemyChaseState(enemy, stateMachine));
            }
            else
            {
                stateMachine.ChangeState(
                    new EnemyPatrolState(enemy, stateMachine));
            }
        }
    }

    public override void FixedUpdate()
    {
        // ❌ Stop() call pannadhe
    }

    public override void Exit()
    {
        Debug.Log("Enemy Hurt Exit");
    }
}