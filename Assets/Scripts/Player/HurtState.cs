using UnityEngine;

public class HurtState : PlayerState
{
    private bool animationStarted;

    public HurtState(PlayerMovement player,
                      PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.SetAnimationSpeed(0f);

        animationStarted = false;

        player.anim.ResetTrigger("Attack1");
        player.anim.ResetTrigger("Attack2");

        player.anim.SetTrigger("Hurt");
    }

    public override void Update()
    {
        player.anim.ResetTrigger("Attack1");
        player.anim.ResetTrigger("Attack2");

        AnimatorStateInfo state =
            player.anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Hurt"))
        {
            animationStarted = true;
        }

        if (animationStarted &&
            state.IsName("Hurt") &&
            state.normalizedTime >= 1f)
        {
            if (player.PlayerHealth.IsDead)
            {
                stateMachine.ChangeState(
                    new DeathState(player, stateMachine));
            }
            else if (Mathf.Abs(player.MoveInput.x) > 0.01f)
            {
                stateMachine.ChangeState(
                    new MoveState(player, stateMachine));
            }
            else
            {
                stateMachine.ChangeState(
                    new IdleState(player, stateMachine));
            }
        }
    }

    public override void FixedUpdate()
    {
        // Knockback handles movement.
        // Don't call player.Stop() here.
    }
}