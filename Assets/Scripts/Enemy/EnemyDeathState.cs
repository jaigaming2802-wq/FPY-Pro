using UnityEngine;

public class EnemyDeathState : EnemyState
{
    private bool animationStarted;

    public EnemyDeathState(
        Enemy enemy,
        EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy Death Enter");

        // Knockback already completed before entering DeathState.
        // So Stop() call panna koodadhu.

        enemy.SetAnimationSpeed(0f);

        animationStarted = false;

        // Reset other animation triggers
        enemy.Animator.ResetTrigger("Attack");
        enemy.Animator.ResetTrigger("Hurt");
        enemy.Animator.ResetTrigger("Recovered");
        enemy.Animator.ResetTrigger("Death");

        // Play Death animation
        enemy.Animator.SetTrigger("Death");
    }

    public override void Update()
    {
        AnimatorStateInfo state =
            enemy.Animator.GetCurrentAnimatorStateInfo(0);

        // Death animation start aayiducha?
        if (state.IsName("Death"))
        {
            animationStarted = true;
        }

        if (!animationStarted)
            return;

        // Death animation complete
        if (state.normalizedTime >= 1f)
        {
            Object.Destroy(enemy.gameObject);
        }
    }

    public override void FixedUpdate()
    {
        // Nothing here.
        // DeathState-la enemy movement control panna vendam.
    }

    public override void Exit()
    {
        Debug.Log("Enemy Death Exit");
    }
}