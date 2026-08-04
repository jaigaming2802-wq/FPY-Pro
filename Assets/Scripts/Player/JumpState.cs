using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerMovement player,
                     PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Jump Enter");

        player.PlayerJump.Jump();

        player.PlayJumpAnimation();

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

        // Jump Cut
        if (player.JumpReleased)
        {
            player.PlayerJump.CutJump();
        }

        // Start Falling
        if (!player.PlayerJump.IsGrounded &&
            player.GetVerticalVelocity() < 0f)
        {
            stateMachine.ChangeState(
                new FallState(player, stateMachine)
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
    }

    public override void FixedUpdate()
    {
        // Don't move normally while dashing
        if (player.PlayerDash.IsDashing)
            return;

        // Air Control
        player.Move();
    }

    public override void Exit()
    {
    }
}