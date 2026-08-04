using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerMovement player,
                     PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Fall Enter");
        player.SetAnimationSpeed(0f);
    }

    public override void Update()
    {
        // Dash
        if (player.DashPressed &&
            player.PlayerDash.CanDash)
        {
            stateMachine.ChangeState(
                new DashState(player, stateMachine)
            );
            return;
        }

        // Landed
        if (player.PlayerJump.IsGrounded)
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

            return;
        }

        if (player.Attack1Pressed)
        {
            stateMachine.ChangeState(
                new AttackState(player, stateMachine, false)
            );
            return;
        }

        // Heavy Attack
        if (player.Attack2Pressed)
        {
            stateMachine.ChangeState(
                new AttackState(player, stateMachine, true)
            );
            return;
        }
    }

    public override void FixedUpdate()
    {
        // Don't allow normal movement while dashing
        if (player.PlayerDash.IsDashing)
            return;

        // Air movement
        player.Move();
    }

    public override void Exit()
    {

    }
}