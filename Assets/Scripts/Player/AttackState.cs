using UnityEngine;

public class AttackState : PlayerState
{
    private bool heavyAttack;

    public AttackState(
        PlayerMovement player,
        PlayerStateMachine stateMachine,
        bool heavyAttack)
        : base(player, stateMachine)
    {
        this.heavyAttack = heavyAttack;
    }

    public override void Enter()
    {
        player.Stop();

        if (heavyAttack)
        {
            player.GetComponent<Animator>()
                .SetTrigger("Attack2");
        }
        else
        {
            player.GetComponent<Animator>()
                .SetTrigger("Attack1");
        }
    }

    public override void Update()
    {
        Animator animator = player.GetComponent<Animator>();

        AnimatorStateInfo state =
            animator.GetCurrentAnimatorStateInfo(0);

        bool finished =
            state.normalizedTime >= 1f;

        if (finished)
        {
            if (Mathf.Abs(player.MoveInput.x) > 0.01f)
            {
                stateMachine.ChangeState(
                    new MoveState(player, stateMachine)
                );
            }
            else
            {
                stateMachine.ChangeState(
                    new IdleState(player, stateMachine)
                );
            }
        }
    }

    public override void FixedUpdate()
    {
        player.Stop();
    }
}
