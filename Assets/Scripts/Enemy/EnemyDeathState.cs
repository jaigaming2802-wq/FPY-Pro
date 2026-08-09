using UnityEngine;

public class EnemyDeathState : EnemyState
{
    private bool animationStarted;

    public EnemyDeathState(Enemy enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy Death Enter");

        enemy.EnemyMovement.Stop();

        enemy.SetAnimationSpeed(0f);

        animationStarted = false;

        enemy.Animator.ResetTrigger("Attack");
        enemy.Animator.ResetTrigger("Hurt");
        enemy.Animator.ResetTrigger("Death");

        enemy.Animator.SetTrigger("Death");
    }

    public override void Update()
    {
        AnimatorStateInfo state =
            enemy.Animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Death"))
        {
            animationStarted = true;
        }

        if (!animationStarted)
            return;

        if (state.normalizedTime >= 1f)
        {
            Object.Destroy(enemy.gameObject);
        }
    }

    public override void FixedUpdate()
    {
        enemy.EnemyMovement.Stop();
    }

    public override void Exit()
    {
        Debug.Log("Enemy Death Exit");
    }
}
