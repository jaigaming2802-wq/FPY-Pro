using UnityEngine;

public class EnemyHurtState : EnemyState
{
    private bool animationStarted;

    public EnemyHurtState(
        Enemy enemy,
        EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy Hurt Enter");

        enemy.SetAnimationSpeed(0f);

        animationStarted = false;

        // Reset other animation triggers
        enemy.Animator.ResetTrigger("Attack");
        enemy.Animator.ResetTrigger("Hurt");
        enemy.Animator.ResetTrigger("Recovered");
        enemy.Animator.ResetTrigger("Death");

        // Play Hurt animation
        enemy.Animator.SetTrigger("Hurt");
    }

    public override void Update()
    {
        AnimatorStateInfo state =
            enemy.Animator.GetCurrentAnimatorStateInfo(0);

        // Hurt animation start aayiducha?
        if (state.IsName("hurt"))
        {
            animationStarted = true;
        }

        if (!animationStarted)
            return;

        // Hurt animation complete
        if (state.normalizedTime >= 1f)
        {
            enemy.Animator.SetTrigger("Recovered");

            // Player attack range-la irundha
            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(
                    new EnemyAttackState(
                        enemy,
                        stateMachine));
            }
            // Player chase range-la irundha
            else if (enemy.IsPlayerInChaseRange())
            {
                stateMachine.ChangeState(
                    new EnemyChaseState(
                        enemy,
                        stateMachine));
            }
            // Illana patrol
            else
            {
                stateMachine.ChangeState(
                    new EnemyPatrolState(
                        enemy,
                        stateMachine));
            }
        }
    }

    public override void FixedUpdate()
    {
        // Normal hurt-la movement stop panna vendam.
    }

    public override void Exit()
    {
        Debug.Log("Enemy Hurt Exit");
    }
}