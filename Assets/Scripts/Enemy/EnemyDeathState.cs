using UnityEngine;

public class EnemyDeathState : EnemyState
{
    private bool animationStarted;
    private bool finished;

    public EnemyDeathState(
        Enemy enemy,
        EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy Death Enter");

        enemy.EnemyMovement.Stop();

        enemy.SetAnimationSpeed(0f);

        animationStarted = false;
        finished = false;

        enemy.Animator.ResetTrigger("Attack");
        enemy.Animator.ResetTrigger("Hurt");
        enemy.Animator.ResetTrigger("Death");

        enemy.Animator.SetTrigger("Death");
    }

    public override void Update()
    {
        if (finished)
            return;

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
            finished = true;

            if (EnemyManager.Instance != null)
            {
                // Enemy + pointA + pointB moonum inactive
                EnemyManager.Instance.OnEnemyDied(enemy);
            }
            else
            {
                enemy.gameObject.SetActive(false);
            }
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